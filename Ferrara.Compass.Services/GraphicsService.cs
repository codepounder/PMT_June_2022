using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.Services
{
    public class GraphicsService : IGraphicsService
    {
        public CompassListItem GetGraphicsItem(int itemId)
        {
            CompassListItem sgItem = new CompassListItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem item = spList.GetItemById(itemId);
                    if (item != null)
                    {
                        sgItem.CompassListItemId = item.ID;

                        // IPF Fields
                        //sgItem.IPF_ProjectNumber = Convert.ToString(item[CompassListFields.IPF_ProjectNumber]);
                        //// SAP Item Request Fields
                        //sgItem.SIR_SAPItemNumber = Convert.ToString(item[CompassListFields.SIR_SAPItemNumber]);
                        //sgItem.SIR_SAPDescription = Convert.ToString(item[CompassListFields.SIR_SAPDescription]);

                        //sgItem.OBM_ConfirmFGBOM = Convert.ToString(item[CompassListFields.OBM_ConfirmFGBOM]);
                        //sgItem.OBM_FirstFourMonthsDemand = Convert.ToDouble(item[CompassListFields.OBM_FirstFourMonthsDemand]);
                        //sgItem.OBM_EstimatedFirstProductionDate = Convert.ToDateTime(item[CompassListFields.FirstProductionDate]);
                        //sgItem.OBM_FlowThroughType = Convert.ToString(item[CompassListFields.OBM_FlowThroughType]);

                        //sgItem.REPORT_CriticalInitiative = Convert.ToString(item[CompassListFields.REPORT_CriticalInitiative]);
                        //sgItem.WorkflowStep = (WorkflowStep)Enum.Parse(typeof(WorkflowStep), item[CompassListFields.WORKFLOW_Step].ToString());
                    }
                }
            }
            return sgItem;
        }

        public void UpdateGraphicsItem(CompassListItem sgItem, int itemId)
        {
            // Get the current user
            SPUser currentUser = SPContext.Current.Web.CurrentUser;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        SPListItem item = spList.GetItemById(itemId);
                        if (item != null)
                        {
                            //item[CompassListFields.OBM_ConfirmFGBOM] = sgItem.OBM_ConfirmFGBOM;
                            //item[CompassListFields.OBM_FirstFourMonthsDemand] = sgItem.OBM_FirstFourMonthsDemand;
                            //if (sgItem.OBM_EstimatedFirstProductionDate != DateTime.MinValue)
                            //    item[CompassListFields.FirstProductionDate] = sgItem.OBM_EstimatedFirstProductionDate;
                            //item[CompassListFields.OBM_FlowThroughType] = sgItem.OBM_FlowThroughType;

                            //item[CompassListFields.WORKFLOW_Step] = sgItem.WorkflowStep;

                            item[CompassListFields.LastUpdatedFormName] = CompassForm.Graphics.ToString();

                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;

                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        #region Graphics Approvals
        public ApprovalItem GetGraphicsApprovalItem(int itemId)
        {
            ApprovalItem appItem = new ApprovalItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    spQuery.RowLimit = 1;

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        SPListItem item = compassItemCol[0];

                        if (item != null)
                        {
                            appItem.StartDate = Convert.ToString(item[ApprovalListFields.GRAPHICS_StartDate]);
                            appItem.ModifiedDate = Convert.ToString(item[ApprovalListFields.GRAPHICS_ModifiedDate]);
                            appItem.ModifiedBy = Convert.ToString(item[ApprovalListFields.GRAPHICS_ModifiedBy]);
                            appItem.SubmittedDate = Convert.ToString(item[ApprovalListFields.GRAPHICS_SubmittedDate]);
                            appItem.SubmittedBy = Convert.ToString(item[ApprovalListFields.GRAPHICS_SubmittedBy]);
                        }
                    }
                }
            }
            return appItem;
        }
        public void UpdateGraphicsApprovalItem(ApprovalItem approvalItem, bool bSubmitted)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + approvalItem.CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem appItem = compassItemCol[0];
                            if (appItem != null)
                            {
                                if ((bSubmitted) && (appItem[ApprovalListFields.OBMReview1_SubmittedDate] == null))
                                {
                                    appItem[ApprovalListFields.GRAPHICS_SubmittedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.GRAPHICS_SubmittedBy] = approvalItem.ModifiedBy;
                                }
                                else
                                {
                                    appItem[ApprovalListFields.GRAPHICS_ModifiedBy] = approvalItem.ModifiedBy;
                                    appItem[ApprovalListFields.GRAPHICS_ModifiedDate] = approvalItem.ModifiedDate;
                                }

                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void SetGraphicsStartDate(int compassListItemId, DateTime startDate)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem item = compassItemCol[0];
                            if (item != null)
                            {
                                // PM First Review Fields
                                if (item[ApprovalListFields.GRAPHICS_StartDate] == null)
                                {
                                    item[ApprovalListFields.GRAPHICS_StartDate] = startDate.ToString();
                                    item.Update();
                                }
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        #endregion
    }
}
