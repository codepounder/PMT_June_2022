using System;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;
using System.Globalization;

namespace Ferrara.Compass.DeleteTimerJob
{

    public class DeleteTimerJob : SPJobDefinition
    {

        #region Constants
        SPWeb spWeb = null;
        //FROM GLOBAL CONTANTS
        public const string LIST_LogsListName = "Logs List";
        #endregion
        #region Constructors
        public DeleteTimerJob() : base() { }
        public DeleteTimerJob(string jobName, SPService service)
            : base(jobName, service, null, SPJobLockType.Job)
        {
            this.Title = jobName;
        }
        public DeleteTimerJob(string jobName, SPWebApplication webapp)
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
                DeleteRows();
                spWeb.AllowUnsafeUpdates = false;
            }

        }
        public void DeleteRows()
        {
            InsertLog("insertActualTimeline", "DeleteRows", "0: ");
            SPList logsList = spWeb.Lists.TryGetList(LIST_LogsListName);
            InsertLog("insertActualTimeline", "DeleteRows", "1: ");
            SPQuery spQuery = new SPQuery();
            InsertLog("insertActualTimeline", "DeleteRows", "2: ");
            spQuery.Query = "<Where><Leq><FieldRef Name=\"Created\" /><Value Type=\"DateTime\"><Today OffsetDays=\"-60\" /></Value></Leq></Where>";
            InsertLog("insertActualTimeline", "DeleteRows", "3: ");
            SPListItemCollection logs = logsList.GetItems(spQuery);
            InsertLog("insertActualTimeline", "DeleteRows", "4: ");
            //InsertLog(spWeb, "4", "UpdateTimelineReportFields", string.Concat("4: "));
            // Loop thru all In Progress Workflow Tasks

            foreach (SPListItem item in logs)
            {
                item.Delete();

            }
            InsertLog("insertActualTimeline", "DeleteRows", "5: ");
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
                item["Form"] = "Project Timeline Calulator";
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

