using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Services;
using Microsoft.Practices.Unity;
using System.Collections;

namespace Ferrara.Compass.Event_Receivers.SAPStatusListItemEventReceiver
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class SAPStatusListItemEventReceiver : SPItemEventReceiver
    {
        #region Member Variables
        int compassListItemId;
        string bBlock;
        string mrpType;
        string poExists;
        string sapRoutings;
        string sourceListComplete;
        string standardCostSet;
        string zBlocksComplete;
        #endregion

        /// <summary>
        /// An item is being updated.
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            //var exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            //exceptionService.Handle(LogCategory.General, "SAPStatusListItemEventReceiver", "ItemUpdating", "Here!!!", "");

            base.ItemUpdated(properties);

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPWeb web = properties.OpenWeb())
                {
                    try
                    {
                        SPListItem currentItem = properties.ListItem;

                        // Get workflow task values
                        compassListItemId = Convert.ToInt32(currentItem["CompassListItemId"]);
                        bBlock = Convert.ToString(currentItem[SAPStatusListFields.BBlockOnItem]);
                        mrpType = Convert.ToString(currentItem[SAPStatusListFields.MRPType]);
                        poExists = Convert.ToString(currentItem[SAPStatusListFields.POExists]);
                        sapRoutings = Convert.ToString(currentItem[SAPStatusListFields.SAPRoutings]);
                        sourceListComplete = Convert.ToString(currentItem[SAPStatusListFields.SourceListComplete]);
                        standardCostSet = Convert.ToString(currentItem[SAPStatusListFields.StandardCostSet]);
                        zBlocksComplete = Convert.ToString(currentItem[SAPStatusListFields.ZBlocksComplete]);

                        if (string.Equals(bBlock, "Complete"))
                            CompleteWorkflowStep(web, compassListItemId, WorkflowStep.REMOVESAPBLOCKS_NOTIFICATION);
                        if (string.Equals(mrpType, "Complete"))
                            CompleteWorkflowStep(web, compassListItemId, WorkflowStep.SAPWAREHOUSEINFO_NOTIFICATION);
                        if (string.Equals(poExists, "Complete"))
                            CompleteWorkflowStep(web, compassListItemId, WorkflowStep.PURCHASEPO_NOTIFICATION);
                        if (string.Equals(sapRoutings, "Complete"))
                            CompleteWorkflowStep(web, compassListItemId, WorkflowStep.SAPROUTINGSETUP_NOTIFICATION);
                        if (string.Equals(sourceListComplete, "Complete"))
                            CompleteWorkflowStep(web, compassListItemId, WorkflowStep.SAPCOSTINGDETAILS_NOTIFICATION);
                        if (string.Equals(standardCostSet, "Complete"))
                            CompleteWorkflowStep(web, compassListItemId, WorkflowStep.STANDARDCOSTENTRY_NOTIFICATION);
                        if (string.Equals(zBlocksComplete, "Complete"))
                            CompleteWorkflowStep(web, compassListItemId, WorkflowStep.REMOVESAPBLOCKS_NOTIFICATION);

                    }
                    catch (Exception ex)
                    {
                        InsertLog(web, ex.Message);
                        throw ex;
                    }
                }
            });
        }

        private void CompleteWorkflowStep(SPWeb spWeb, int itemId, WorkflowStep currentWFStep)
        {
            spWeb.AllowUnsafeUpdates = true;

            try
            {
                SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                SPListItem item = spList.GetItemById(itemId);
                if (item != null)
                {
                    InsertLog(spWeb, "Start loop");
                    foreach (SPWorkflowTask wfTask in item.Tasks)
                    {
                        string test = Convert.ToString(wfTask["WorkflowStep"]);
                        InsertLog(spWeb, "WorkflowStep: "+test);
                        if (string.Equals(test, currentWFStep.ToString()))
                        {
                            //if ()
                            Hashtable ht = new Hashtable();
                            ht[SPBuiltInFieldId.Completed] = true;
                            ht["Status"] = "Completed";
                            ht[SPBuiltInFieldId.PercentComplete] = 1.0;
                            ht[SPBuiltInFieldId.TaskStatus] = "#";
                            ht["TaskStatus"] = "#";

                            try
                            {
                                SPWorkflowTask.AlterTask(wfTask as SPListItem, ht, false);
                            }
                            catch (Exception ex)
                            {
                                InsertLog(spWeb, ex.Message);
                                //exceptionService.Handle(LogCategory.CriticalError, ex, "WorkflowService", "CompleteWorkflowTask(int itemId, WorkflowStep currentWFStep)", "Current Workflow Step: " + currentWFStep);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //exceptionService.Handle(LogCategory.CriticalError, ex, "WorkflowService", "CompleteWorkflowTask(int itemId, WorkflowStep currentWFStep)", "Current Workflow Step: " + currentWFStep);
            }

            spWeb.AllowUnsafeUpdates = false;
        }
        public void InsertLog(SPWeb spWeb, string message)
        {
            int iUserId = 0;
            SPList list = spWeb.Lists.TryGetList(GlobalConstants.LIST_LogsListName);
            if (list != null)
            {
                spWeb.AllowUnsafeUpdates = true;

                SPListItem item = list.Items.Add();
                item[LogListNames.Category] = LogCategory.General;
                item[LogListNames.Message] = message;
                item[LogListNames.Title] = message;
                if (message.Length > 250)
                    item[LogListNames.Title] = message.Substring(0, 250);
                item[LogListNames.Form] = "SAPStatusListItemEventReceiver";
                item[LogListNames.Method] = "ItemUpdated";
                item[LogListNames.AdditionalInfo] = "";
                item[LogListNames.CreatedDate] = DateTime.Now;
                if (iUserId.Equals(0))
                    iUserId = spWeb.CurrentUser.ID;
                item[LogListNames.CreatedBy] = iUserId;
                item.SystemUpdate(false);

                spWeb.AllowUnsafeUpdates = false;
            }
        }

    }
}