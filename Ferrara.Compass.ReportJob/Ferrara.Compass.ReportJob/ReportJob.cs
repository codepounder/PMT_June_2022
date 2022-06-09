using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;

namespace Ferrara.Compass.ReportJob
{
    public class ReportJob : SPJobDefinition
    {
        #region Constants
        // Lists
        //public const string LIST_ProjectStatusReportListName = "Compass Status Report List";
        //public const string LIST_CompassListName = "Compass List";
        //public const string LIST_ProjectStatusDetailReportListName = "Compass Project Status Detail Report List";
        //public const string LIST_ProjectStatusHistoryReportListName = "Compass Status Report History List";
        //public const string LIST_ApprovalListName = "Compass Approval List";
        //public const string LIST_LogsListName = "Logs List";
        //public const string LIST_Configurations = "Configurations";

        ////public const string SITE_Name = "/sites/cfts";

        //// Compass List
        //public const string REPORT_ProjectStatus = "REPORT_ProjectStatus";
        //public const string OBM_RevisedFirstShipDate = "OBM_RevisedFirstShipDate";
        //public const string WORKFLOW_Step = "WorkflowStep";
        //public const string IPF_ProjectNumber = "ProjectNumber";
        //public const string IPF_SubmittedDate = "IPF_SubmittedDate";
        //public const string REPORT_ProjectStatusLastModified = "REPORT_ProjStatusLastModified";

        #endregion

        #region Member Variables
        SPWeb web = null;
        //SPListItem currentDetailItem;
        //int averageCount = 1000;
        #endregion

        #region Constructors
        public ReportJob() : base() { }

        public ReportJob(string jobName, SPService service)
            : base(jobName, service, null, SPJobLockType.Job)
        {
            this.Title = jobName;
        }

        public ReportJob(string jobName, SPWebApplication webapp)
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

            //LoadConfigurations();
            //LoadProjectStatusReportSettings();
            //ProcessCompassProjects();
            //UpdateProjectStatusStatistics();
            //UpdateProjectStatusHistory();

            web.AllowUnsafeUpdates = false;
        }
    }
}
