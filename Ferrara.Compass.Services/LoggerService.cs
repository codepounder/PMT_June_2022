using System;
using System.Text;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;

namespace Ferrara.Compass.Services
{
    public class LoggerService : ILoggerService
    {
        public void InsertLog(LogEntry logentry)
        {
            try
            {
                string currentWeb = SPContext.Current.Web.Url;

                if (currentWeb != null)
                {
                    SPUser user = null;
                    if (SPContext.Current != null)
                    {
                        user = SPContext.Current.Web.CurrentUser != null ?
                            SPContext.Current.Web.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName) : null;
                    }
                    int iUserId = 0;
                    if (user != null)
                        iUserId = user.ID;


                    SPSecurity.RunWithElevatedPrivileges(delegate
                    {
                        using (SPSite spSite = new SPSite(currentWeb))
                        {
                            using (SPWeb spWeb = spSite.OpenWeb())
                            {
                                SPList list = spWeb.Lists.TryGetList(GlobalConstants.LIST_LogsListName);
                                if (list != null)
                                {
                                    spWeb.AllowUnsafeUpdates = true;

                                    //DeleteLogs(currentWeb, list);
                                    SPListItem item = list.Items.Add();
                                    item[LogListNames.Category] = logentry.Category;
                                    item[LogListNames.Message] = logentry.Message;
                                    item[LogListNames.Title] = logentry.Title;
                                    item[LogListNames.Form] = logentry.Form;
                                    item[LogListNames.Method] = logentry.Method;
                                    item[LogListNames.AdditionalInfo] = logentry.AdditionalInfo;
                                    item[LogListNames.CreatedDate] = DateTime.Now;
                                    if (iUserId.Equals(0))
                                        iUserId = spWeb.CurrentUser.ID;
                                    item[LogListNames.CreatedBy] = iUserId;
                                    item.SystemUpdate(false);

                                    spWeb.AllowUnsafeUpdates = false;
                                }
                                spWeb.Update();
                            }
                        }
                    });
                }
            }
            catch (Exception exception)
            {
                //LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "IPF: " + exception.Message);
            }
        }

        public void InsertLog(LogEntry logentry, string webUrl)
        {
            try
            {
                string currentWeb = SPContext.Current.Web.Url;

                if (currentWeb != null)
                {
                    SPUser user = null;
                    if (SPContext.Current != null)
                    {
                        user = SPContext.Current.Web.CurrentUser != null ?
                            SPContext.Current.Web.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName) : null;
                    }
                    int iUserId = 0;
                    if (user != null)
                        iUserId = user.ID;


                    SPSecurity.RunWithElevatedPrivileges(delegate
                    {
                        using (SPSite spSite = new SPSite(webUrl))
                        {
                            using (SPWeb spWeb = spSite.OpenWeb())
                            {
                                SPList list = spWeb.Lists.TryGetList(GlobalConstants.LIST_LogsListName);
                                if (list != null)
                                {
                                    DateTime today = DateTime.Now;
                                    string folderDate = today.Month + "_" + today.Date + "_" + today.Year;
                                    spWeb.AllowUnsafeUpdates = true;

                                    //DeleteLogs(currentWeb, list);
                                    SPListItem item = list.Items.Add();
                                    item[LogListNames.Category] = logentry.Category;
                                    item[LogListNames.Message] = logentry.Message;
                                    item[LogListNames.Title] = logentry.Title;
                                    item[LogListNames.Form] = logentry.Form;
                                    item[LogListNames.Method] = logentry.Method;
                                    item[LogListNames.AdditionalInfo] = logentry.AdditionalInfo;
                                    item[LogListNames.CreatedDate] = DateTime.Now;
                                    if (iUserId.Equals(0))
                                        iUserId = spWeb.CurrentUser.ID;
                                    item[LogListNames.CreatedBy] = iUserId;
                                    item.SystemUpdate(false);

                                    spWeb.AllowUnsafeUpdates = false;
                                }
                                spWeb.Update();
                            }
                        }
                    });
                }
            }
            catch (Exception exception)
            {
                //LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "IPF: " + exception.Message);
            }
        }

        public void InsertGraphicsImportLog(LogEntry logentry)
        {
            try
            {
                string currentWeb = SPContext.Current.Web.Url;

                if (currentWeb != null)
                {
                    SPUser user = null;
                    if (SPContext.Current != null)
                    {
                        user = SPContext.Current.Web.CurrentUser != null ?
                            SPContext.Current.Web.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName) : null;
                    }
                    int iUserId = 0;
                    if (user != null)
                        iUserId = user.ID;


                    SPSecurity.RunWithElevatedPrivileges(delegate
                    {
                        using (SPSite spSite = new SPSite(currentWeb))
                        {
                            using (SPWeb spWeb = spSite.OpenWeb())
                            {
                                SPList list = spWeb.Lists.TryGetList(GlobalConstants.LIST_GraphicsLogsListName);
                                if (list != null)
                                {
                                    spWeb.AllowUnsafeUpdates = true;

                                    //DeleteLogs(currentWeb, list);
                                    SPListItem item = list.Items.Add();
                                    item[LogListNames.Category] = logentry.Category;
                                    item[LogListNames.Message] = logentry.Message;
                                    item[LogListNames.Title] = logentry.Title;
                                    item[LogListNames.Form] = logentry.Form;
                                    //item[LogListNames.Method] = logentry.Method;
                                    //item[LogListNames.AdditionalInfo] = logentry.AdditionalInfo;
                                    item.SystemUpdate(false);

                                    spWeb.AllowUnsafeUpdates = false;
                                }
                                spWeb.Update();
                            }
                        }
                    });
                }
            }
            catch (Exception exception)
            {
                //LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "IPF: " + exception.Message);
            }
        }

        private void DeleteLogs(string currentWeb, SPList list)
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
        }
    }
}
