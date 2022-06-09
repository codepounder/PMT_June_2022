using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.Event_Receivers.WorkflowTaskItemAddedEventReceiver
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class WorkflowTaskItemAddedEventReceiver : SPItemEventReceiver
    {
        /// <summary>
        /// An item was added.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);

            using (SPWeb web = properties.OpenWeb())
            {
                try
                {
                    SPListItem currentItem = properties.ListItem;

                    // Get workflow task values
                    string compassListItemId = Convert.ToString(currentItem["CompassListItemId"]);
                    string workflowStep = Convert.ToString(currentItem["WorkflowStep"]);

                    currentItem["DueDate"] = DetermineTaskDueDate(compassListItemId, workflowStep, web);
                    currentItem.Update();
                }
                catch (Exception ex)
                {
                    InsertLog(web, ex.Message);
                    throw ex;
                }
            }
        }

        private string DetermineTaskDueDate(string compassListItemId, string workflowStep, SPWeb web)
        {
            DateTime dueDate = DateTime.Now;
            string dueDateDays = string.Empty;

            // Check for an existing project specific row
            dueDateDays = GetProjectSpecificRow(compassListItemId, workflowStep, web);
            if (string.IsNullOrEmpty(dueDateDays))
            {
                // Didn't find project specific data, so get default setting.
                string projectTimelineType = GetProjectTypelineType(compassListItemId, web);

                dueDateDays = GetDefaultTimelineDays(workflowStep, projectTimelineType, web);
            }

            if (!string.IsNullOrEmpty(dueDateDays))
            {
                dueDate = dueDate.AddDays(Convert.ToDouble(dueDateDays));
            }

            return dueDate.ToString();
        }

        private string GetProjectSpecificRow(string compassListItemId, string workflowStep, SPWeb spWeb)
        {
            string value = string.Empty;

            try
            {
                SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectTimelineUpdateName);

                SPQuery spQuery = new SPQuery();
                spQuery.Query = "<Where><Eq><FieldRef Name=\"compassListItemId\" /><Value Type=\"Int\">" + compassListItemId + "</Value></Eq></Where>";
                spQuery.RowLimit = 1;

                SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                if (compassItemCol.Count > 0)
                {
                    SPListItem item = compassItemCol[0];
                    if (item[workflowStep] != null)
                        value = Convert.ToString(item[workflowStep]);
                }
            }
            catch (Exception ex)
            {
                InsertLog(spWeb, ex.Message);
            }

            return value;
        }
        private string GetProjectTypelineType(string compassListItemId, SPWeb spWeb)
        {
            string timelineType = "Standard";

            var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
            var item = spList.GetItemById(Convert.ToInt32(compassListItemId));
            if (item != null)
            {
                // Proposed Project Fields
                timelineType = Convert.ToString(item[CompassListFields.TimelineType]);
            }

            return timelineType;
        }
        private string GetDefaultTimelineDays(string workflowStep, string projectTimelineType, SPWeb spWeb)
        {
            string value = string.Empty;

            try
            {
                SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectTimelineTypeListName);
                SPQuery spQuery = new SPQuery();
                spQuery.Query = "<Where><Eq><FieldRef Name=\"WorkflowQuickStep\"></FieldRef><Value Type=\"Text\">" + workflowStep + "</Value></Eq></Where>";
                spQuery.RowLimit = 1;
                SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                if (compassItemCol.Count > 0)
                {
                    SPListItem item = compassItemCol[0];
                    if (item != null)
                    {
                        if (projectTimelineType.ToLower() == "standard")
                        {
                            value = Convert.ToString(item[ProjectTimelineTypeDays.Standard]);
                        }
                        else if (projectTimelineType.ToLower() == "expedited")
                        {
                            value = Convert.ToString(item[ProjectTimelineTypeDays.Expedited]);
                        }
                        else if (projectTimelineType.ToLower() == "ludicrous")
                        {
                            value = Convert.ToString(item[ProjectTimelineTypeDays.Ludicrous]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                InsertLog(spWeb, ex.Message);
            }

            return value;
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
                item[LogListNames.Form] = "WorkflowTaskItemAddedEventReceiver";
                item[LogListNames.Method] = "ItemAdded";
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