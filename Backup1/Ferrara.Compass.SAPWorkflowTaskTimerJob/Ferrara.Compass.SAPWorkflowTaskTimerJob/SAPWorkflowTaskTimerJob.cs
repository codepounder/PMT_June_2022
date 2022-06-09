using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using System.Collections;
using Ferrara.Compass.SAPWorkflowTaskTimerJob.Classes;
using Ferrara.Compass.SAPWorkflowTaskTimerJob.Models;
using System.Net.Mail;

namespace Ferrara.Compass.SAPWorkflowTaskTimerJob
{
    public class SAPWorkflowTaskTimerJob : SPJobDefinition
    {
        #region Constants
        // Lists
        public const string LIST_CompassListName = "Compass List";
        public const string LIST_StageGateProjectListName = "Stage Gate Project List";
        public const string LIST_ApprovalListName = "Compass SAP Approval List";
        public const string LIST_LogsListName = "Logs List";
        public const string LIST_WorkflowTaskListName3 = "Compass Workflow Tasks 3";
        public const string LIST_PackagingItemListName = "Compass Packaging Item List";
        public const string LIST_SAPStatusListName = "SAP Status List";
        public const string LIST_EmailTemplates = "Email Templates";
        public const string LIST_Configurations = "Configurations";
        #endregion

        #region Member Variables
        SPWeb web = null;
        public const string Version = "Version: 2.8";
        #endregion

        #region Constructors
        public SAPWorkflowTaskTimerJob() : base() { }
        public SAPWorkflowTaskTimerJob(string jobName, SPService service)
            : base(jobName, service, null, SPJobLockType.Job)
        {
            this.Title = jobName;
        }
        public SAPWorkflowTaskTimerJob(string jobName, SPWebApplication webapp)
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

            //InsertLog("Execute ProcessSAPStatusListTasks", "Execute", "");
            ProcessSAPStatusListTasks(LIST_WorkflowTaskListName3);
            SendPostLaunchEmails();
            web.AllowUnsafeUpdates = false;
        }

        #region SAP Status List Methods
        private void ProcessSAPStatusListTasks(string listName)
        {
            InsertLog(Version, "SAP Workflow Task Timer Job", "ProcessSAPStatusListTasks");
            //string materialNumber = string.Empty;
            string projectNumber = string.Empty;
            int compassListItemId = 0;
            try
            {
                SPList taskList = web.Lists.TryGetList(listName);
                if (taskList == null)
                {
                    InsertLog(string.Concat("Unable to find List: ", listName), "ProcessSAPStatusListTasks", "");
                    return;
                }

                // Get all the currently "In Progrss" or "Not Started" workflow tasks
                SPQuery spQuery = new SPQuery();
                spQuery.Query = "<Where><Neq><FieldRef Name=\"Status\" /><Value Type=\"Text\">Completed</Value></Neq></Where>";
                SPListItemCollection taskItemCol = taskList.GetItems(spQuery);
                string currentWorkflowStep = string.Empty;

                // Loop thru all In Progress Workflow Tasks
                foreach (SPListItem taskItem in taskItemCol)
                {
                    // If task is one of our SAP tasks, check status
                    currentWorkflowStep = Convert.ToString(taskItem["WorkflowStep"]);
                    if ((string.Equals(currentWorkflowStep, "SAPROUTINGSETUP_NOTIFICATION")) ||
                        (string.Equals(currentWorkflowStep, "SAPCOSTINGDETAILS_NOTIFICATION")) ||
                        (string.Equals(currentWorkflowStep, "SAPWAREHOUSEINFO_NOTIFICATION")) ||
                        (string.Equals(currentWorkflowStep, "STANDARDCOSTENTRY_NOTIFICATION")) ||
                        (string.Equals(currentWorkflowStep, "COSTFINISHEDGOOD_NOTIFICATION")) ||
                        (string.Equals(currentWorkflowStep, "FINALCOSTINGREVIEW_NOTIFICATION")) ||
                        (string.Equals(currentWorkflowStep, "PURCHASEPO_NOTIFICATION")) ||
                        (string.Equals(currentWorkflowStep, "REMOVESAPBLOCKS_NOTIFICATION")))
                    {
                        // Get current Compass project
                        projectNumber = Convert.ToString(taskItem["ProjectNumber"]);
                        // Get the current Manufacturing Location
                        SPList compassList = web.Lists.TryGetList(LIST_CompassListName);
                        SPQuery cListQuery = new SPQuery();
                        cListQuery.Query = "<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + projectNumber + "</Value></Eq></Where>";
                        SPListItemCollection cListItemCol = compassList.GetItems(cListQuery);
                        SPListItem compassItem = cListItemCol[0];
                        string mfgPlant = Convert.ToString(compassItem["ManufacturingLocation"]);
                        mfgPlant = mfgPlant.Substring(0, 4);// mfgPlant.IndexOf("(") + 1, mfgPlant.Length - mfgPlant.IndexOf("(") - 2);
                        string packPlant = Convert.ToString(compassItem["PackingLocation"]);
                        packPlant = packPlant.Substring(0, 4);//.Substring(packPlant.IndexOf("(") + 1, packPlant.Length - packPlant.IndexOf("(") - 2);
                        string sapItemNumber = Convert.ToString(compassItem["SAPItemNumber"]);

                        compassListItemId = Convert.ToInt32(compassItem["ID"]);
                        bool bCompleted = true;
                        // Log

                        // Check the step we are on to determine if we need to check for the Finished Good or 
                        // for all NEW materials to be complete
                        if ((string.Equals(currentWorkflowStep, "SAPROUTINGSETUP_NOTIFICATION")) ||
                            (string.Equals(currentWorkflowStep, "COSTFINISHEDGOOD_NOTIFICATION")) ||
                            (string.Equals(currentWorkflowStep, "FINALCOSTINGREVIEW_NOTIFICATION")) ||
                            (string.Equals(currentWorkflowStep, "REMOVESAPBLOCKS_NOTIFICATION")))
                        {
                            //LoggingService.LogMessage(LoggingService.WebPartLoggingDiagnosticArea, string.Concat("PMT: SAPWorkflowTaskTimerJob-SAP Task Found: currentWorkflowStep: ", currentWorkflowStep, " compassListItemId: ", compassListItemId.ToString()));
                            //InsertLog("PMT: SAP Task Found", "ProcessSAPStatusListTasks", string.Concat("currentWorkflowStep: ", currentWorkflowStep, " project#: ", projectNumber, "material number: ", sapItemNumber));
                            //InsertLog("PMT: SAP Task Found", "SAPROUTINGSETUP_NOTIFICATION", projectNumber);

                            // Check that Finished Good is setup
                            // Check If step complete
                            if (!string.IsNullOrEmpty(sapItemNumber) && sapItemNumber.ToLower() != "needs new")
                            {
                                if (!IsStepCompleted(currentWorkflowStep, sapItemNumber, packPlant))
                                    bCompleted = false;
                            }
                        }/*
                        else
                        {
                         // Check that all NEW materials are complete
                         // Get all new components for Compass Project
                            SPList packagingList = web.Lists.TryGetList(LIST_PackagingItemListName);
                            SPQuery spPackagingQuery = new SPQuery();
                            spPackagingQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassListItemId + "</Value></Eq><Eq><FieldRef Name=\"NewExisting\" /><Value Type=\"Choice\">New</Value></Eq></And></Where>";
                            SPListItemCollection packagingItemCol = packagingList.GetItems(spPackagingQuery);
                            //materialNumber = string.Empty;

                            foreach (SPListItem packagingItem in packagingItemCol)
                            {
                            string materialNumber = Convert.ToString(packagingItem["MaterialNumber"]);
                            if (!string.IsNullOrEmpty(materialNumber) && materialNumber.ToLower() != "needs new" && !string.IsNullOrEmpty(packPlant))
                                {
                                // Check If step complete
                                if (!IsStepCompleted(currentWorkflowStep, materialNumber, packPlant))
                                {
                                    bCompleted = false;
                                    break;
                                }
                                }
                                else
                                {
                                    bCompleted = false;
                                }
                            }*/
                        //}
                        string NotificationOut = currentWorkflowStep.Replace("_NOTIFICATION", "");
                        //if (!string.IsNullOrEmpty(materialNumber) && materialNumber.ToLower() != "needs new")
                        //{
                        if (bCompleted)
                        {
                            CompleteWorkflowStep(compassListItemId, currentWorkflowStep);
                            UpdateSAPApprovalList(compassListItemId, projectNumber, NotificationOut, "_SubmittedDate");
                        }
                        else
                        {
                            UpdateSAPApprovalList(compassListItemId, projectNumber, NotificationOut, "_ModifiedDate");
                        }

                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                InsertLog("SAP Timer Job-Error", "ProcessSAPStatusListTasks", string.Concat("CompassId: ", compassListItemId.ToString(), "Project#: ", projectNumber, " Message: ", ex.Message));
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, string.Concat("SAP Timer Job-ProcessSAPStatusListTasks: ", ex.Message));
            }
        }
        private bool IsStepCompleted(string currentWorkflowStep, string materialNumber, string plant)
        {
            bool bCompleted = false;
            SPList statusList = web.Lists.TryGetList(LIST_SAPStatusListName);
            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><And><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + materialNumber + "</Value></Eq><Eq><FieldRef Name=\"Plant\" /><Value Type=\"Text\">" + plant + "</Value></Eq></And></Where>";
            SPListItemCollection statusItemCol = statusList.GetItems(spQuery);
            if (statusItemCol == null)
            {
                //InsertLog("SAP Timer Job-Step Complete", "IsStepCompleted", "No item found: " + materialNumber);
                return false;
            }
            SPListItem statusItem;
            if (statusItemCol != null && statusItemCol.Count <= 0)
            {
                //InsertLog("SAP Timer Job-Step Complete", "IsStepCompleted", "No status line found");
                return false;
            }
            else
            {
                statusItem = statusItemCol[0];
            }

            string value = string.Empty;
            string value2 = string.Empty;
            if (string.Equals(currentWorkflowStep, "SAPROUTINGSETUP_NOTIFICATION"))
            {
                value = Convert.ToString(statusItem["SAPRoutings"]);
                if (string.Equals(value, "Y"))
                    bCompleted = true;
            }
            else if (string.Equals(currentWorkflowStep, "SAPCOSTINGDETAILS_NOTIFICATION"))
            {
                value = Convert.ToString(statusItem["SourceListComplete"]);
                if (string.Equals(value, "Y"))
                    bCompleted = true;
            }
            else if (string.Equals(currentWorkflowStep, "SAPWAREHOUSEINFO_NOTIFICATION"))
            {
                value = Convert.ToString(statusItem["BBlockOnItem"]);
                value2 = Convert.ToString(statusItem["MRPType"]);
                if ((string.Equals(value, "Y")) && (string.Equals(value2, "PD")))
                    bCompleted = true;
            }
            else if (string.Equals(currentWorkflowStep, "STANDARDCOSTENTRY_NOTIFICATION"))
            {
                value = Convert.ToString(statusItem["StandardCostSet"]);
                if (string.Equals(value, "Y"))
                    bCompleted = true;
            }
            else if ((string.Equals(currentWorkflowStep, "COSTFINISHEDGOOD_NOTIFICATION")) ||
                (string.Equals(currentWorkflowStep, "FINALCOSTINGREVIEW_NOTIFICATION")))
            {
                value = Convert.ToString(statusItem["BBlockOnItem"]);
                if (string.Equals(value, "Y"))
                    bCompleted = true;
            }
            else if (string.Equals(currentWorkflowStep, "PURCHASEPO_NOTIFICATION"))
            {
                value = Convert.ToString(statusItem["POExists"]);
                if (string.Equals(value, "Y"))
                    bCompleted = true;
            }
            else if (string.Equals(currentWorkflowStep, "REMOVESAPBLOCKS_NOTIFICATION"))
            {
                value = Convert.ToString(statusItem["ZBlocksComplete"]);
                if (string.Equals(value, "Y"))
                    bCompleted = true;
            }
            return bCompleted;
        }
        #endregion
        #region Send Post Launch Emails
        private void SendPostLaunchEmails()
        {
            InsertLog(Version + " - START", "Send Post Launch Emails Job", "SendPostLaunchEmails");
            //InsertLog("1", "", "");
            try
            {
                SPList spList = web.Lists.TryGetList(LIST_StageGateProjectListName);
                SPQuery spQuery = new SPQuery();
                spQuery.Query = "<Where><Eq><FieldRef Name=\"Stage\" /><Value Type=\"Text\">" + "Post Launch" + "</Value></Eq></Where>";
                SPListItemCollection StageGateItemCol = spList.GetItems(spQuery);
                List<StageGateCreateProjectItem> sgItems = new List<StageGateCreateProjectItem>();
                //InsertLog("2", "", "");

                // InsertLog("3", "", "");

                if (StageGateItemCol.Count > 0)
                {
                    // InsertLog("4", "", "");
                    foreach (SPListItem item in StageGateItemCol)
                    {
                        if (item != null)
                        {
                            //InsertLog("5", "", "");
                            StageGateCreateProjectItem sgItem = new StageGateCreateProjectItem();
                            sgItem.StageGateProjectListItemId = item.ID;
                            sgItem.DesiredShipDate = Convert.ToDateTime(item["DesiredShipDate"]);
                            sgItem.RevisedShipDate = Convert.ToDateTime(item["RevisedShipDate"]);
                            sgItem.ProjectNumber = Convert.ToString(item["ProjectNumber"]);
                            sgItem.ProjectName = Convert.ToString(item["ProjectName"]);
                            sgItem.ProjectManager = Convert.ToString(item["ProjectManager"]);
                            sgItem.InTech = Convert.ToString(item["InTech"]);
                            sgItem.Marketing = Convert.ToString(item["Marketing"]);
                            sgItem.InTechRegulatory = Convert.ToString(item["InTechRegulatory"]);
                            sgItem.PostLaunch3MSent = Convert.ToString(item["Notification3MSent"]);
                            sgItem.PostLaunch6MSent = Convert.ToString(item["Notification6MSent"]);
                            sgItem.PostLaunch9MSent = Convert.ToString(item["Notification9MSent"]);
                            sgItem.TestProject = Convert.ToString(item["TestProject"]);
                            sgItems.Add(sgItem);
                        }
                    }
                }


                foreach (var sgItem in sgItems)
                {
                    try
                    {
                        //InsertLog("6", "", "");
                        DateTime CurrentDate = DateTime.Today;
                        DateTime ShipDate = DateTime.Today;

                        if ((sgItem.DesiredShipDate != null) && (sgItem.DesiredShipDate != DateTime.MinValue))
                        {
                            ShipDate = sgItem.DesiredShipDate;
                        }
                        else
                        {
                            if ((sgItem.RevisedShipDate != null) && (sgItem.RevisedShipDate != DateTime.MinValue))
                            {
                                ShipDate = sgItem.RevisedShipDate;
                            }
                        }

                        int dayDifference = (CurrentDate - ShipDate).Days;
                        string EmailType = "";
                        //Notification has to be triggered for post launch selected projects after 3 months minus 14 days of the revised first ship date, same for 6 months and 9 months.
                        if ((dayDifference + 14 >= 90 && dayDifference + 14 <= 95) && sgItem.PostLaunch3MSent != "Yes")
                        {
                            EmailType = "3M";
                        }
                        else if (dayDifference + 14 >= 180 && dayDifference + 14 <= 185 && sgItem.PostLaunch6MSent != "Yes")
                        {
                            EmailType = "6M";

                        }
                        else if (dayDifference + 14 >= 270 && dayDifference + 14 <= 275 && sgItem.PostLaunch9MSent != "Yes")
                        {
                            EmailType = "9M";

                        }

                        if (!string.IsNullOrEmpty(EmailType))
                        {
                            //InsertLog("7", "", "");
                            EmailTemplateField emailTemplate = new EmailTemplateField();
                            emailTemplate = GetEmailTemplateFromList("StageGatePostLaunchNotification");
                            SendPostLaunchEmailNotification(sgItem, emailTemplate, EmailType);
                            UpdateStageGateProjectList(sgItem.StageGateProjectListItemId, EmailType);
                        }
                    }
                    catch (Exception ex)
                    {
                        InsertLog("Send Post Launch Emails Job-Error at Parent proect looping: ", "SendPostLaunchEmails", string.Concat("Send Post Launch Emails Job-SendPostLaunchEmails: ", sgItem.ProjectNumber, ex.Message));
                        LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, string.Concat("Send Post Launch Emails Job-SendPostLaunchEmails: ", sgItem.ProjectNumber, ex.Message));
                    }
                }
                InsertLog("Send Post Launch Emails Job - END", "Send Post Launch Emails Job", "SendPostLaunchEmails");
            }
            catch (Exception ex)
            {
                InsertLog("Send Post Launch Emails Job-Error", "SendPostLaunchEmails", ex.Message);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, string.Concat("Send Post Launch Emails Job-SendPostLaunchEmails: ", ex.Message));
            }
        }
        #endregion

        #region Close Workflow Task
        private void CompleteWorkflowStep(int itemId, string currentWFStep)
        {
            try
            {
                SPList spList = web.Lists.TryGetList(LIST_CompassListName);
                SPListItem item = spList.GetItemById(itemId);
                if (item != null)
                {
                    foreach (SPWorkflowTask wfTask in item.Tasks)
                    {
                        string test = Convert.ToString(wfTask["WorkflowStep"]);
                        if (string.Equals(test, currentWFStep))
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
                                InsertLog("SAP Timer Job-Complete Task", "Complete Task", string.Concat("compassListItemId: ", itemId.ToString(), " currentWFStep: ", currentWFStep));
                                LoggingService.LogMessage(LoggingService.WebPartLoggingDiagnosticArea, string.Concat("SAP Timer Job-Complete Task: currentWorkflowStep: ", currentWFStep, " compassListItemId: ", itemId.ToString()));
                            }
                            catch (Exception ex)
                            {
                                InsertLog(ex.Message, "CompleteWorkflowStep", "SAPWorkflowTaskTimerJob");
                                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, string.Concat("SAP Timer Job-CompleteWorkflowStep Error: ", ex.Message));
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                InsertLog(ex.Message, "CompleteWorkflowStep", "SAPWorkflowTaskTimerJob");
                LoggingService.LogMessage(LoggingService.WebPartLoggingDiagnosticArea, string.Concat("SAP Timer Job-Complete Task Error: currentWorkflowStep: ", currentWFStep, " compassListItemId: ", itemId.ToString()));
            }
        }
        #endregion
        #region Popst Launch Email Helper functions
        private void UpdateSAPApprovalList(int itemId, string projectNo, string task, string field)
        {
            try
            {
                SPList spList = web.Lists.TryGetList(LIST_ApprovalListName);
                SPQuery spQuery = new SPQuery();
                spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                SPListItem item;
                if (compassItemCol != null)
                {
                    if (compassItemCol.Count > 0)
                    {
                        item = compassItemCol[0];
                    }
                    else
                    {
                        item = spList.AddItem();
                        item["CompassListItemId"] = itemId;
                        item[task + "_StartDate"] = DateTime.Now.ToString();
                        item["Title"] = projectNo;
                    }
                }
                else
                {
                    item = spList.AddItem();
                    item["CompassListItemId"] = itemId;
                    item[task + "_StartDate"] = DateTime.Now.ToString();
                    item["Title"] = projectNo;

                }
                if (string.IsNullOrEmpty(Convert.ToString(item[task + "_StartDate"])))
                {
                    item[task + "_StartDate"] = DateTime.Now.ToString();
                }
                item[task + field] = DateTime.Now.ToString();
                item.Update();
                LoggingService.LogMessage(LoggingService.WebPartLoggingDiagnosticArea, string.Concat("SAP Timer Job-Modified Approval currentWorkflowStep: ", task + field, " compassListItemId: ", itemId.ToString()));

            }
            catch (Exception ex)
            {
                InsertLog("SAP Timer Job-Updated Approval", "Update Approval Error", string.Concat("compassListItemId: ", itemId.ToString(), " currentWFStep: ", task + field, "error:" + ex));
                LoggingService.LogMessage(LoggingService.WebPartLoggingDiagnosticArea, string.Concat("SAP Timer Job-Modified Approval Error currentWorkflowStep: ", task + field, " compassListItemId: ", itemId.ToString(), "error: " + ex));
            }
        }
        public List<ItemProposalItem> getChildProjects(int parentID)
        {
            // InsertLog("9", "", "");
            List<ItemProposalItem> ChildProjectDetailsList = new List<ItemProposalItem>();
            SPList spList3 = web.Lists.TryGetList(LIST_CompassListName);
            SPQuery spQuery3 = new SPQuery();
            spQuery3.Query = "<Where><Eq><FieldRef Name=\"StageGateProjectListItemId\" /><Value Type=\"Integer\">" + parentID + "</Value></Eq></Where>";
            SPListItemCollection compassItemCol3 = spList3.GetItems(spQuery3);
            //InsertLog("10", "", "");
            if (compassItemCol3.Count > 0)
            {
                //InsertLog("11", "", "");
                foreach (SPListItem oldItem in compassItemCol3)
                {
                    //InsertLog("12", "", "");
                    if (oldItem != null)
                    {
                        try
                        {
                            ItemProposalItem newItem = new ItemProposalItem();
                            newItem.CompassListItemId = oldItem.ID;
                            newItem.StageGateProjectListItemId = Convert.ToInt32(oldItem["StageGateProjectListItemId"]);
                            newItem.ProjectNumber = Convert.ToString(oldItem["ProjectNumber"]);
                            newItem.SAPItemNumber = Convert.ToString(oldItem["SAPItemNumber"]);
                            newItem.SAPDescription = Convert.ToString(oldItem["SAPDescription"]);
                            newItem.RevisedFirstShipDate = Convert.ToDateTime(oldItem["RevisedFirstShipDate"]);

                            ChildProjectDetailsList.Add(newItem);
                        }
                        catch (Exception ex)
                        {
                            InsertLog("getChildProjects Failed", "getChildProjects", "projectNo :" + Convert.ToString(oldItem["ProjectNumber"]) + " : " + ex.Message);
                        }
                    }
                }
            }
            return ChildProjectDetailsList;
        }
        private void SendPostLaunchEmailNotification(StageGateCreateProjectItem sgItem, EmailTemplateField emailTemplate, string EmailType)
        {
            try
            {
                //InsertLog("8", "", "");
                List<ItemProposalItem> ChildProjects = getChildProjects(sgItem.StageGateProjectListItemId);

                //InsertLog("13", "", "");
                var childProjectRows = string.Empty;
                var childProjectTable = string.Empty;

                foreach (var ChildProject in ChildProjects)
                {
                    // InsertLog("14", "", "");
                    childProjectRows = childProjectRows + GetChildProjectTableRow(ChildProject);
                }

                childProjectTable =
                  "<table>" +
                       "<tr>" +
                           "<th style=\"text-align:left; \">Project Number</th>" +
                           "<th style=\"text-align:left; \">SAP Item #(FG)</th>" +
                           "<th style=\"text-align:left; \">SAP Description</th>" +
                           "<th style=\"text-align:left; \">Revised 1st Ship Date</th>" +
                        "</tr>" +
                        childProjectRows +
                  "</table>";



                emailTemplate.Subject = emailTemplate.Subject.Replace("<#NOTIFICATIONTYPE#>", EmailType);
                emailTemplate.Subject = emailTemplate.Subject.Replace("<#PARENTPROJECTNUMBER#>", sgItem.ProjectNumber);
                emailTemplate.Subject = emailTemplate.Subject.Replace("<#PARENTPROJECTNAME#>", sgItem.ProjectName);

                string projectName = string.Concat(sgItem.ProjectNumber, " - ", sgItem.ProjectName);
                string projectLink = string.Concat(web.Url, "/pages/StageGateProjectPanel.aspx?ProjectNo=", sgItem.ProjectNumber);
                string projectHyperLink = string.Concat("<a href=", projectLink, ">", projectName, "</a> ");

                emailTemplate.Body = emailTemplate.Body.Replace("<#NOTIFICATIONTYPECODE#>", EmailType);
                emailTemplate.Body = emailTemplate.Body.Replace("<#PARENTPROJECTNUMBER#>", projectHyperLink);
                emailTemplate.Body = emailTemplate.Body.Replace("<#CHILDPROJECTTABLE#>", childProjectTable);

                List<string> EmailTo = new List<string>();
                if (sgItem.TestProject == "Yes")
                {
                    //InsertLog("15", "", "");
                    SPGroup approverGroup = web.Groups["Developer Members"];
                    EmailTo.AddRange(
                        from SPUser u in approverGroup.Users
                        where !string.IsNullOrEmpty(u.Email)
                        select u.Email);

                    emailTemplate.Subject = "Test Project - Please Ignore! - " + emailTemplate.Subject;
                    emailTemplate.Body = "Test Project - Please Ignore! <br><br>" + emailTemplate.Body;
                }
                else
                {
                    // InsertLog("16", "", "");
                    EmailTo.AddRange(GetUsersEmails(sgItem.ProjectManager));
                    EmailTo.AddRange(GetUsersEmails(sgItem.InTech));
                    EmailTo.AddRange(GetUsersEmails(sgItem.Marketing));
                    EmailTo.AddRange(GetUsersEmails(sgItem.InTechRegulatory));
                }

                SendEmail(EmailTo, null, emailTemplate.Subject, emailTemplate.Body, sgItem.ProjectNumber);
                //InsertLog("17", "", "");
                InsertLog("Send Post Launch Emails Job-", "SendPostLaunchEmailNotification", string.Concat("Send Post Launch Emails Job-SendPostLaunchEmailNotification: Email sent successfully for projectNumber - ", sgItem.ProjectNumber));
                //InsertLog("21", "", "");
            }
            catch (Exception ex)
            {
                InsertLog("Send Post Launch Emails Job-Error", "SendPostLaunchEmailNotification", string.Concat("Send Post Launch Emails Job-SendPostLaunchEmailNotification: StageGateProjectListItemId - ", sgItem.StageGateProjectListItemId.ToString(), ex.Message));
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, string.Concat("Send Post Launch Emails Job-SendPostLaunchEmailNotification: StageGateProjectListItemId - ", sgItem.StageGateProjectListItemId.ToString(), ex.Message));
            }
        }
        public void SendEmail(IEnumerable<string> to, IEnumerable<string> cc, string subject, string body, string projectNumber)
        {
            try
            {
                // InsertLog("18", "", "");
                SmtpClient smtpClient = new SmtpClient { Host = GetConfigurationFromList("SMTPServerName") };
                using (MailMessage mailMessage = new MailMessage())
                {
                    foreach (string email in to.Where(email => !string.IsNullOrEmpty(email)))
                    {
                        mailMessage.To.Add(new MailAddress(email));
                    }

                    if (cc != null)
                    {
                        foreach (string email in cc.Where(email => !string.IsNullOrEmpty(email)))
                        {
                            mailMessage.To.Add(new MailAddress(email));
                        }
                    }

                    mailMessage.From = new MailAddress(GetConfigurationFromList("SMTPFromEmailAddress"));
                    //InsertLog("19", "", "");
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;

                    if (mailMessage.To.Count == 0)
                    {
                        InsertLog("Send Post Launch Emails Job-Error", "SendEmail", string.Concat("Send Post Launch Emails Job-SendEmail: projectNumber - ", projectNumber, "Email To is blank"));
                        LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, string.Concat("Send Post Launch Emails Job-SendPostLaunchEmailNotification: projectNumber - ", projectNumber, "Email To is blank"));
                    }
                    else
                    {
                        // InsertLog("20", "", "");
                        smtpClient.Send(mailMessage);
                    }
                }
            }
            catch (SmtpException ex)
            {
            }
        }
        private List<string> GetUsersEmails(string userNames)
        {
            List<string> UserEmails = new List<string>();

            if (!string.IsNullOrEmpty(userNames))
            {
                SPFieldUserValueCollection Usercollection = new SPFieldUserValueCollection(web, userNames);
                UserEmails.AddRange(
                    from SPFieldUserValue UserValue in Usercollection
                    where UserValue.User != null && !string.IsNullOrEmpty(UserValue.User.Email)
                    select UserValue.User.Email
                    );
                //foreach (SPFieldUserValue UserValue in Usercollection)
                //{
                //    var spUser = UserValue.User.;
                //    Users.Add(spUser);
                //}
            }


            return UserEmails;
        }
        public string GetConfigurationFromList(string key)
        {
            string configuration = string.Empty;
            SPList oList = web.Lists.TryGetList(LIST_Configurations);
            var oItem = oList.Items.OfType<SPListItem>().FirstOrDefault(x => x["Title"].Equals(key));
            if (oItem != null)
            {
                if (oItem["Value"] != null)
                    configuration = oItem["Value"].ToString();
            }

            return configuration;
        }
        private string GetChildProjectTableRow(ItemProposalItem ChildProject)
        {
            string projectLink = string.Concat(web.Url, "/pages/", "ProjectStatus.aspx?ProjectNo=", ChildProject.ProjectNumber);

            return
               "<tr>" +
                   "<td>" +
                       string.Concat("<a href=", projectLink, ">", ChildProject.ProjectNumber, "</a> ") +
                   "</td>" +
                    "<td>" +
                        ChildProject.SAPItemNumber +
                   "</td>" +
                   "<td>" +
                        ChildProject.SAPDescription +
                   "</td>" +
                   "<td>" +
                      GetDateForDisplay(ChildProject.RevisedFirstShipDate) +
                   "</td>" +
               "</tr>";
        }
        public EmailTemplateField GetEmailTemplateFromList(string key)
        {
            EmailTemplateField emailTemplate = new EmailTemplateField();


            SPList oList = web.Lists.TryGetList(LIST_EmailTemplates);
            var oItem = oList.Items.OfType<SPListItem>().FirstOrDefault(x => x["Title"].Equals(key));
            if (oItem != null)
            {
                emailTemplate.Body = oItem["Body"].ToString();
                emailTemplate.Subject = oItem["Subject"].ToString();
                emailTemplate.Title = oItem["Title"].ToString();
                return emailTemplate;
            }


            return null;
        }
        public static string GetDateForDisplay(DateTime currentValue)
        {
            DateTime DATETIME_MIN = new DateTime(1900, 1, 1);

            if (currentValue == Convert.ToDateTime(null))
                return string.Empty;
            else if (currentValue.Equals(DATETIME_MIN))
                return string.Empty;
            else if (currentValue.Equals("1/1/0001"))
                return string.Empty;
            else
                return currentValue.ToShortDateString();
        }
        private void UpdateStageGateProjectList(int StageGateProjectListId, string EmailType)
        {
            try
            {
                SPList spList = web.Lists.TryGetList(LIST_StageGateProjectListName);
                SPListItem item = spList.GetItemById(StageGateProjectListId);

                if (EmailType == "3M")
                {
                    item["Notification3MSent"] = "Yes";
                    item["Notification3MSentDate"] = DateTime.Now.ToString();
                }
                else if (EmailType == "6M")
                {
                    item["Notification6MSent"] = "Yes";
                    item["Notification6MSentDate"] = DateTime.Now.ToString();
                }
                else if (EmailType == "9M")
                {
                    item["Notification9MSent"] = "Yes";
                    item["Notification9MSentDate"] = DateTime.Now.ToString();
                }

                item.Update();
                LoggingService.LogMessage(LoggingService.WebPartLoggingDiagnosticArea, string.Concat(EmailType + "Notification Sent updated : ", " StageGateProjectListId: ", StageGateProjectListId.ToString()));
            }
            catch (Exception ex)
            {
                InsertLog(EmailType + "Notification sent update : ", "Update Error", string.Concat("StageGateProjectListId: ", StageGateProjectListId.ToString(), "error:" + ex));
                LoggingService.LogMessage(LoggingService.WebPartLoggingDiagnosticArea, string.Concat(EmailType + "Notification Sent updated : ", " StageGateProjectListId: ", StageGateProjectListId.ToString(), "error: " + ex));
            }
        }
        #endregion
        #region InsertLog Method
        private void InsertLog(string message, string method, string additionalInfo)
        {
            var list = web.Lists.TryGetList(LIST_LogsListName);
            if (list != null)
            {
                var item = list.Items.Add();
                item["Category"] = "CriticalError";
                item["Message"] = message;
                item["Title"] = message.Length > 250 ? message.Substring(0, 250) : message;
                item["Form"] = "SAP Workflow Task Timer Job";
                item["Method"] = method;
                item["AdditionalInfo"] = additionalInfo;
                item["CreatedDate"] = DateTime.Now;
                item.SystemUpdate(false);
            }
        }

        private void InsertDailyLog(string message, string method, string additionalInfo)
        {
            var list = web.Lists.TryGetList(LIST_LogsListName);
            if (list != null)
            {
                // Only insert the item if we haven't done this today already
                SPQuery spQuery = new SPQuery();
                spQuery.Query = "<Where><And><Eq><FieldRef Name=\"Message\" /><Value Type=\"Text\">" + message + "</Value></Eq><Eq><FieldRef Name=\"AdditionalInfo\" /><Value Type=\"Text\">" + additionalInfo + "</Value></Eq></And></Where>";
                SPListItemCollection taskItemCol = list.GetItems(spQuery);

                if ((taskItemCol == null) || (taskItemCol.Count == 0))
                {
                    SPListItem item = list.Items.Add();
                    item["Category"] = "CriticalError";
                    item["Message"] = message;
                    item["Title"] = message.Length > 250 ? message.Substring(0, 250) : message;
                    item["Form"] = "SAP Workflow Task Timer Job";
                    item["Method"] = method;
                    item["AdditionalInfo"] = additionalInfo;
                    item["CreatedDate"] = DateTime.Now;
                    item.SystemUpdate(false);
                }
                else
                {
                    // See if we log this today
                    DateTime today = DateTime.Now;
                    bool notLogged = true;

                    foreach (SPListItem taskItem in taskItemCol)
                    {
                        DateTime createdDate = Convert.ToDateTime(taskItem["CreatedDate"]);
                        if ((createdDate.Month == today.Month) && (createdDate.Day == today.Day))
                            notLogged = false;
                    }

                    if (notLogged)
                    {
                        SPListItem item = list.Items.Add();
                        item["Category"] = "CriticalError";
                        item["Message"] = message;
                        item["Title"] = message.Length > 250 ? message.Substring(0, 250) : message;
                        item["Form"] = "SAP Workflow Task Timer Job";
                        item["Method"] = method;
                        item["AdditionalInfo"] = additionalInfo;
                        item["CreatedDate"] = DateTime.Now;
                        item.SystemUpdate(false);
                    }
                }
            }
        }
        #endregion
    }
}
