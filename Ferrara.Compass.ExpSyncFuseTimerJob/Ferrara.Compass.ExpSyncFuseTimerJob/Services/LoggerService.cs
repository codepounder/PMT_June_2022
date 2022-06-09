using System;
using System.Text;
using Microsoft.SharePoint;
using Ferrara.Compass.ExpSyncFuseTimerJob.Constants;
using Ferrara.Compass.ExpSyncFuseTimerJob.Models;

namespace Ferrara.Compass.ExpSyncFuseTimerJob.Services
{
    public class LoggerService
    {
        SPSite site;
        public LoggerService(SPSite site)
        {
            this.site = site;
        }
        public void InsertLog(LogEntry logentry)
        {
            SPListItem item;
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPWeb spWeb = site.OpenWeb())
                {
                    SPList list = spWeb.Lists.TryGetList(GlobalConstants.LIST_LogsListName);
                    if (list != null)
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        item = list.Items.Add();
                        item[LogListNames.Category] = logentry.Category;
                        item[LogListNames.Message] = logentry.Message;
                        item[LogListNames.Title] = logentry.Title;
                        item[LogListNames.Form] = logentry.Form;
                        item[LogListNames.Method] = logentry.Method;
                        item[LogListNames.AdditionalInfo] = logentry.AdditionalInfo;
                        item[LogListNames.CreatedDate] = DateTime.Now;
                        item.SystemUpdate(false);
                        spWeb.AllowUnsafeUpdates = false;
                    }
                    spWeb.Update();
                }
            });
        }

        /*private void DeleteLogs(string currentWeb, SPList list)
        {            
            var maximumNumberOfLogs = GlobalConstants.CONST_LogCount;
            if (list.Items.Count < maximumNumberOfLogs)
            {
                return;
            }
            var numberOfLogsToBeDeleted = GlobalConstants.CONST_LogCount;
            if (maximumNumberOfLogs == 0 || numberOfLogsToBeDeleted == 0)
            {
                return;
            }
            var totalNumberOfLogsToBeDeleted = (list.Items.Count - maximumNumberOfLogs) + 1;
            var numberOfCompleteBatches = totalNumberOfLogsToBeDeleted / numberOfLogsToBeDeleted;
            var sizeOfIncompleteBatch = totalNumberOfLogsToBeDeleted % numberOfLogsToBeDeleted;
            for (int j = 0; j < numberOfCompleteBatches; j++)
            {
                DeleteListItems(numberOfLogsToBeDeleted, list);
            }
            if (sizeOfIncompleteBatch != 0)
            {
                DeleteListItems(sizeOfIncompleteBatch, list);
            }
        }

        private static void DeleteListItems(int numberOfItemsToDelete, SPList list)
        {
            try
            {
                SPQuery spQuery = new SPQuery();
                string query =
                    string.Format(
                        "<View><Query><OrderBy><FieldRef Name='Id' /></OrderBy></Query><RowLimit>{0}</RowLimit></View>",
                        numberOfItemsToDelete);
                spQuery.ViewXml = query;
                SPListItemCollection sortedList = list.GetItems(spQuery);

                StringBuilder batchDeleteCommand = new StringBuilder();
                batchDeleteCommand.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?><Batch>");
                string command = "<Method><SetList Scope=\"Request\">" + list.ID +
                              "</SetList><SetVar Name=\"ID\">{0}</SetVar><SetVar Name=\"Cmd\">Delete</SetVar></Method>";
                if (sortedList != null && sortedList.Count > 0)
                {
                    for (int i = 0; i < sortedList.Count; i++)
                    {
                        batchDeleteCommand.Append(string.Format(command, sortedList[i].ID.ToString()));
                    }
                    batchDeleteCommand.Append("</Batch>");
                }
                list.ParentWeb.ProcessBatchData(batchDeleteCommand.ToString());
            }
            catch
            {

            }
        }*/
    }
}
