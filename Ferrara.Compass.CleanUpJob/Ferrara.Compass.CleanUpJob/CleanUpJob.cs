using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using System.Collections;

namespace Ferrara.Compass.CleanUpJob
{
    public class CleanUpJob : SPJobDefinition
    {
        #region Constants
        // Lists
        public const string LIST_CompassListName = "Compass List";
        public const string LIST_ApprovalListName = "Compass Approval List";
        public const string LIST_LogsListName = "Logs List";
        public const string LIST_WorkflowTaskListName = "Workflow Tasks";
        public const string LIST_PackagingItemListName = "Compass Packaging Item List";
        public const string LIST_SAPStatusListName = "SAP Status List";
        public const string LIST_DragonflyStatusListName = "Dragonfly Status List";
        #endregion

        #region Member Variables
        SPWeb web = null;
        #endregion

        #region Constructors
        public CleanUpJob() : base() { }
        public CleanUpJob(string jobName, SPService service)
            : base(jobName, service, null, SPJobLockType.Job)
        {
            this.Title = jobName;
        }
        public CleanUpJob(string jobName, SPWebApplication webapp)
            : base(jobName, webapp, null, SPJobLockType.Job)
        {
            this.Title = jobName;
        }
        #endregion

        public override void Execute(Guid targetInstanceId)
        {
            SPWebApplication webApp = this.Parent as SPWebApplication;
            web = webApp.Sites[0].RootWeb;

            web.AllowUnsafeUpdates = true;

            CleanUpCompassLogs();
            //ProcessDragonflyStatusListTasks();

            web.AllowUnsafeUpdates = false;
        }

        private void CleanUpCompassLogs()
        {
            SPList logsList = web.Lists.TryGetList(LIST_LogsListName);
            if (logsList == null)
            {
                LogError(string.Concat("Unable to find List: ", LIST_LogsListName), "CleanUpCompassLogs", "");
                return;
            }

            // Get all the old Logs
            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><Leq><FieldRef Name=\"Created\" /><Value Type=\"DateTime\" IncludeTimeValue=\"FALSE\"><Today OffsetDays=\"-7\" /></Value></Leq></Where>";
            spQuery.RowLimit = Convert.ToUInt32(1000);
            SPListItemCollection compassItemCol = logsList.GetItems(spQuery);

            // Create a string to batch delete
            StringBuilder sbDelete = new StringBuilder();
            sbDelete.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sbDelete.Append("<Batch>");

            string buildQuery = "<Method><SetList Scope=\"Request\">" + logsList.ID + "</SetList><SetVar Name=\"ID\">{0}</SetVar><SetVar Name=\"Cmd\">Delete</SetVar></Method>";

            // Loop thru all the logs and add to the delete batch
            foreach (SPListItem logItem in compassItemCol)
            {
                try
                {
                    sbDelete.Append(string.Format(buildQuery, logItem.ID.ToString()));
                }
                catch (Exception ex)
                {
                    LogError(ex.Message, "CleanUpCompassLogs", "");
                }
            }

            sbDelete.Append("</Batch>");
            web.ProcessBatchData(sbDelete.ToString());

            LogInfo("CleanUp Compass Logs: " + compassItemCol.Count.ToString());
        }
        private void CleanWorkflowTasks()
        {

        }

        #region Helper Methods
        private void LogError(string message, string method, string additionalInfo)
        {
            var list = web.Lists.TryGetList(LIST_LogsListName);
            if (list != null)
            {
                var item = list.Items.Add();
                item["Category"] = "CriticalError";
                item["Message"] = message;
                item["Title"] = message.Length > 250 ? message.Substring(0, 250) : message;
                item["Form"] = "Reporting Timer Job";
                item["Method"] = method;
                item["AdditionalInfo"] = additionalInfo;
                item["CreatedDate"] = DateTime.Now;
                item.SystemUpdate(false);
            }
        }
        private void LogInfo(string message)
        {
            var list = web.Lists.TryGetList(LIST_LogsListName);
            if (list != null)
            {
                var item = list.Items.Add();
                item["Category"] = "GeneralError";
                item["Message"] = message;
                item["Title"] = message.Length > 250 ? message.Substring(0, 250) : message;
                item["Form"] = "Reporting Timer Job";
                item["Method"] = "";
                item["AdditionalInfo"] = "";
                item["CreatedDate"] = DateTime.Now;
                item.SystemUpdate(false);
            }
        }
        #endregion
    }
}
