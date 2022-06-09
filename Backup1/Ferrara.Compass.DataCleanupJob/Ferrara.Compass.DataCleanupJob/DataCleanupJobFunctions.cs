using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.DataCleanupJob
{
    public class DataCleanupJobFunctions : SPJobDefinition
    {
        #region Constants
        SPWeb spWeb = null;
        //FROM GLOBAL CONTANTS
        public const string LIST_LogsListName = "Logs List";
        public const string LIST_CompassListName = "Compass List";
        public const string LIST_PackagingItemListName = "Compass Packaging Item List";
        public const string LIST_WorkflowTaskListName1 = "Compass Workflow Tasks 1";
        public const string LIST_WorkflowTaskListName2 = "Compass Workflow Tasks 2";
        public const string LIST_WorkflowTaskListName3 = "Compass Workflow Tasks 3";
        #endregion
        #region Constructors
        public DataCleanupJobFunctions() : base() { }
        public DataCleanupJobFunctions(string jobName, SPService service)
            : base(jobName, service, null, SPJobLockType.Job)
        {
            this.Title = jobName;
        }
        public DataCleanupJobFunctions(string jobName, SPWebApplication webapp)
            : base(jobName, webapp, null, SPJobLockType.Job)
        {
            this.Title = jobName;
        }
        #endregion
        public static CultureInfo ci = new CultureInfo("en-US");
        public override void Execute(Guid targetInstanceId)
        {
            SPWebApplication webApp = this.Parent as SPWebApplication;
            if (webApp.Name == "Compass")
            {
                spWeb = webApp.Sites[0].RootWeb;
                
                    spWeb.AllowUnsafeUpdates = true;
                DeleteLogsListRows();
                DeleteComponentsListRows();
                    spWeb.AllowUnsafeUpdates = false;
                
            }
        }
        public void DeleteLogsListRows()
        {
            InsertLog("Version: 1", "Delete Logs List Rows", "DeleteLogsListRows");
            SPList logsList = spWeb.Lists.TryGetList(LIST_LogsListName);
            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><Leq><FieldRef Name=\"Created\" /><Value Type=\"DateTime\"><Today OffsetDays=\"-7\" /></Value></Leq></Where>";
            spQuery.RowLimit = 1000;
            SPListItemCollection logs = logsList.GetItems(spQuery);
            InsertLog("DeleteRows", "DeleteRows", "logs Count: "+logs.Count.ToString());
            // Loop thru all In Progress Workflow Tasks
            if (logs.Count > 0)
            {
                int deletedCount = 0;
                try
                {
                    for (int i = logs.Count - 1; i >= 0; i--)
                    {
                        try
                        {
                            logs.DeleteItemById(logs[i].ID);
                            deletedCount++;
                        }
                        catch (Exception ex)
                        {
                            InsertLog("DeleteRows", "DeleteRows", "Error in foreach: " + ex);
                        }

                    }
                }
                catch (Exception e)
                {
                    InsertLog("DeleteRows", "DeleteRows", "Error befre foreach: " + e);
                }
                InsertLog("DeleteRows", "DeleteRows", "deletedCount: " + deletedCount.ToString());
            }
        }
        public void DeleteComponentsListRows()
        {
            SPList compassList = spWeb.Lists.TryGetList(LIST_CompassListName);
            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><Or><Eq><FieldRef Name=\"WorkflowPhase\" /><Value Type=\"Text\">Completed</Value></Eq><Eq><FieldRef Name=\"WorkflowPhase\" /><Value Type=\"Text\">Cancelled</Value></Eq></Or></Where>";
            spQuery.ViewFields = "<FieldRef Name=\"ID\" />";
            SPListItemCollection projects = compassList.GetItems(spQuery);
            InsertLog("Cancelled/Completed Count: " + projects.Count.ToString(), "DeleteComponentsListRows", "DeleteComponentsListRows");
            // Loop thru all In Progress Workflow Tasks
            List<int> lstProjectIds = new List<int>();
            foreach (SPListItem project in projects)
            {
                if (projects != null)
                {
                    lstProjectIds.Add(Convert.ToInt32(project.ID));
                }
            }
            SPList PIList = spWeb.Lists.TryGetList(LIST_PackagingItemListName);
            SPQuery spQuery2 = new SPQuery();
            string query = "<Where><And><In><FieldRef Name=\"CompassListItemId\" LookupId=\"True\" /><Values>";
            foreach (int id in lstProjectIds)
            {
                query += "<Value Type=\"Integer\">" + id + "</Value>";
            }
            query += "</Values></In><Eq><FieldRef Name=\"Deleted\" /><Value Type=\"Text\">Yes</Value></Eq></And></Where>";
            spQuery2.Query = query;
            spQuery2.RowLimit = 500;
            spQuery2.ViewFields = "<ViewFields>" +
                "<FieldRef Name=\"ID\"></FieldRef>" +
                "<FieldRef Name=\"Deleted\"></FieldRef>" +
                "<FieldRef Name=\"CompassListItemId\"></FieldRef>" +
            "</ViewFields>";
            SPListItemCollection deletedComp = PIList.GetItems(spQuery2);
            List<int> deletedItemIds = new List<int>();
            if (deletedComp.Count > 0)
            {
                int deletedCount = 0;
                InsertLog("Deleted Component Count: " + deletedComp.Count.ToString(), "DeleteComponentsListRows", "DeleteComponentsListRows");
                try
                {
                    for (int i = deletedComp.Count - 1; i >= 0; i--)
                    {
                        try
                        {
                            SPListItem currentListItem = deletedComp.GetItemById(deletedComp[i].ID);
                            deletedItemIds.Add(currentListItem.ID);
                            currentListItem.Delete();
                            deletedCount++;
                        }
                        catch (Exception ex)
                        {
                            InsertLog("Error in foreach: " + ex, "DeleteComponentsListRows", "DeleteComponentsListRows");
                        }

                    }
                }
                catch (Exception e)
                {
                    InsertLog("Error befre foreach: " + e, "DeleteComponentsListRows", "DeleteComponentsListRows");
                }
                InsertLog("deletedCount: " + deletedCount.ToString(), "DeleteComponentsListRows", "DeleteComponentsListRows");
            }
            //Get non-deleted childred on deleted parents
            SPQuery spQuery3 = new SPQuery();
            string query2 = "<Where><In><FieldRef Name=\"ParentId\" LookupId=\"True\" /><Values>";
            foreach (int id in deletedItemIds)
            {
                query2 += "<Value Type=\"Integer\">" + id + "</Value>";
            }
            query2 += "</Values></In></Where>";
            spQuery3.Query = query;
            spQuery3.RowLimit = 500;
            spQuery3.ViewFields = "<ViewFields>" +
                "<FieldRef Name=\"ID\"></FieldRef>" +
                "<FieldRef Name=\"Deleted\"></FieldRef>" +
                "<FieldRef Name=\"CompassListItemId\"></FieldRef>" +
                "<FieldRef Name=\"ParentId\"></FieldRef>" +
            "</ViewFields>";
            SPListItemCollection deletedComp2 = PIList.GetItems(spQuery3);
            if (deletedComp2.Count > 0)
            {
                int deletedCount = 0;
                InsertLog("Deleted Child Component Count: " + deletedComp2.Count.ToString(), "DeleteChildComponentsListRows", "DeleteChildComponentsListRows");
                try
                {
                    for (int i = deletedComp2.Count - 1; i >= 0; i--)
                    {
                        try
                        {
                            SPListItem currentListItem = deletedComp2.GetItemById(deletedComp2[i].ID);
                            currentListItem.Delete();
                            deletedCount++;
                        }
                        catch (Exception ex)
                        {
                            InsertLog("Error in foreach: " + ex, "DeleteChildComponentsListRows", "DeleteChildComponentsListRows");
                        }

                    }
                }
                catch (Exception e)
                {
                    InsertLog("Error befre foreach: " + e, "DeleteChildComponentsListRows", "DeleteChildComponentsListRows");
                }
                InsertLog("deletedChildCount: " + deletedCount.ToString(), "DeleteChildComponentsListRows", "DeleteChildComponentsListRows");
            }
        }
        public void DeleteWorkflowTasks()
        {
            SPList WorkflowTask = spWeb.Lists.TryGetList(LIST_WorkflowTaskListName1);
            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><And><Eq><FieldRef Name=\"Status\" /><Value Type=\"Text\">Completed</Value></Eq><Leq><FieldRef Name=\"Modified\" /><Value Type=\"DateTime\"><Today OffsetDays=\"-30\" /></Value></Leq></And></Where>";
            spQuery.ViewFields = "<FieldRef Name=\"ID\" />";
            spQuery.RowLimit = 1000;
            SPListItemCollection tasks = WorkflowTask.GetItems(spQuery);
            InsertLog("Completed Task Count: " + tasks.Count.ToString(), "DeleteWorkflowTasks", "DeleteWorkflowTasks");
            if (tasks.Count > 0)
            {
                int deletedCount = 0;
                try
                {
                    for (int i = tasks.Count - 1; i >= 0; i--)
                    {
                        try
                        {
                            tasks.DeleteItemById(tasks[i].ID);
                            deletedCount++;
                        }
                        catch (Exception ex)
                        {
                            InsertLog("DeleteWorkflowTasks", "DeleteWorkflowTasks", "Error in foreach: " + ex);
                        }

                    }
                }
                catch (Exception e)
                {
                    InsertLog("DeleteWorkflowTasks", "DeleteWorkflowTasks", "Error befre foreach: " + e);
                }
                InsertLog("DeleteWorkflowTasks", "DeleteWorkflowTasks", "DeleteWorkflowTasks: " + deletedCount.ToString());
            }
        }
        private void InsertLog(string message, string method, string additionalInfo)
        {
            /*SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;*/
            var list = spWeb.Lists.TryGetList(LIST_LogsListName);
            if (list != null)
            {

                var item = list.Items.Add();
                item["Category"] = "CriticalError";
                item["Message"] = message;
                item["Title"] = message.Length > 250 ? message.Substring(0, 250) : message;
                item["Form"] = "Data Cleanup Job";
                item["Method"] = method;
                item["AdditionalInfo"] = additionalInfo;
                item["CreatedDate"] = DateTime.Now;
                item.SystemUpdate(false);

            }
            /* spWeb.AllowUnsafeUpdates = false;
             }*/
        }
    }
}
