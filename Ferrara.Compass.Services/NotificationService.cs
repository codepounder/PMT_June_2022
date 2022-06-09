using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;
using Microsoft.SharePoint;
using System.Text.RegularExpressions;

namespace Ferrara.Compass.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailService emailService;
        private readonly IEmailTemplateService emailTemplateService;
        private readonly IEmailLoggingService emailLoggingService;
        private readonly IExceptionService exceptionService;
        private readonly IUtilityService utilityService;
        private readonly IConfigurationManagementService configurationService;
        private readonly IUserManagementService userManagementService;

        public NotificationService(IEmailService emailService, IEmailTemplateService emailTemplateService,
            IExceptionService exceptionService, IUtilityService utilityService, IConfigurationManagementService configurationService, IUserManagementService userManagementService, IEmailLoggingService emailLoggingService)
        {
            this.emailService = emailService;
            this.emailTemplateService = emailTemplateService;
            this.exceptionService = exceptionService;
            this.utilityService = utilityService;
            this.configurationService = configurationService;
            this.userManagementService = userManagementService;
            this.emailLoggingService = emailLoggingService;
        }
        public bool EmailWFStep(string currentWfStep, string pageName, int itemId, string projectNumber, string notes)
        {
            bool emailSent = false;
            CompassListItem item = GetCompassData(itemId);
            WorkflowStep setWF = (WorkflowStep)System.Enum.Parse(typeof(WorkflowStep), currentWfStep, true);
            item.WorkflowStep = setWF;
            item.WorkflowCurrentUrl = string.Concat(SPContext.Current.Web.Url, "/pages/", pageName, "?", GlobalConstants.QUERYSTRING_ProjectNo, "=", projectNumber);
            item.WorkflowCommercializationUrl = string.Concat(SPContext.Current.Web.Url, "/pages/", GlobalConstants.PAGE_CommercializationItemSummary, "?", GlobalConstants.QUERYSTRING_ProjectNo, "=", projectNumber);
            item.ProjectNotes = notes;

            List<string> emails = (item.TestProject.ToLower() != "yes") ? GetWorkflowStepEmailGroups(currentWfStep.ToString()) : new List<string>() { GlobalConstants.GROUP_Developers };
            List<string> EmailAddresses = userManagementService.GetEmailIds(emails, itemId).ToList();
            item.EmailAddresses = EmailAddresses;
            if (EmailAddresses.Count > 0)
            {
                bool sendEmail = SendNotificationEmail(item);
                if (sendEmail)
                {
                    try
                    {
                        emailSent = true;
                        emailLoggingService.LogSentEmailUpdate(itemId, currentWfStep.ToString(), projectNumber);
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            return emailSent;
        }
        public void resetSentStatus(int compassListItemId)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = site.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassWorkflowStatusListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassWorkflowStatusListFields.CompassListItemId + "\" /><Value Type=\"Integer\">" + compassListItemId + "</Value></Eq></Where>";
                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem appItem = compassItemCol[0];

                            if (appItem != null)
                            {
                                appItem[CompassWorkflowStatusListFields.SAPBOMSetupConfSent_Completed] = "No";

                                // Set Modified By to current user NOT System Account
                                appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                appItem.Update();
                            }

                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public List<String> GetWorkflowStepEmailGroups(string WFStep)
        {
            List<String> groups = new List<String>();
            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb oWeb = site.OpenWeb())
                {
                    SPList oList = oWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTaskAssignmentListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + WFStep + "</Value></Eq></Where>";
                    spQuery.RowLimit = 1;
                    SPListItemCollection compassItemCol = oList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        SPListItem item = compassItemCol[0];
                        if (item != null)
                        {
                            var email = Convert.ToString(item[TaskAssignmentFieldName.EmailGroups]);
                            if (!string.IsNullOrEmpty(email))
                            {
                                groups = email.Split(';').ToList();
                            }
                        }
                    }
                }
            }
            return groups;
        }
        public bool SendNotificationEmail(CompassListItem compassListItem)
        {
            // Locate the template required
            EmailTemplateField emailTemplate = emailTemplateService.GetEmailTemplate(compassListItem.WorkflowStep.ToString().ToUpper());
            if (emailTemplate != null)
            {
                compassListItem = GetKeys(compassListItem);
                ApprovalListItem approvalItem = GetApprovalKeys(compassListItem.CompassListItemId);
                Hashtable keysValue = new Hashtable
                {
                    { "ProjectNo", compassListItem.ProjectNumber ?? string.Empty },
                    { "PRJECTTYPE", compassListItem.ProjectType ?? string.Empty },
                    { "PRJECTTYPESUBCATEGORY", compassListItem.ProjectTypeSubCategory ?? string.Empty },
                    { "SAP FG#", compassListItem.SAPItemNumber ?? string.Empty },
                    { "SAP DESCRIPTION", compassListItem.SAPDescription ?? string.Empty},
                    { "PROD HIER LVL1", compassListItem.ProductHierarchyLevel1 ?? string.Empty },
                    { "PROD HIER LVL2", compassListItem.ProductHierarchyLevel2 ?? string.Empty },
                    { "BRAND", compassListItem.MaterialGroup1Brand ?? string.Empty },
                    { "OBM", GetPersonFieldForDisplay(compassListItem.PM) ?? string.Empty },
                    { "BRAND MGR", GetPersonFieldForDisplay(compassListItem.Marketing) ?? string.Empty },
                    { "FIRST SHIP DATE", compassListItem.FirstShipDate},
                    { "FORECAST1", compassListItem.Month1ProjectedDollars},
                    { "FORECAST2", compassListItem.Month2ProjectedDollars},
                    { "FORECAST3", compassListItem.Month3ProjectedDollars},
                    { "ANNUAL FORECAST", compassListItem.AnnualProjectedDollars?? string.Empty},
                    { "DISTRIBUTION", compassListItem.DistributionCenter?? string.Empty},
                    { "MAKE LOCATION", compassListItem.ManufacturingLocation?? string.Empty},
                    { "MAKE COUNTRY", compassListItem.ManufacturerCountryOfOrigin?? string.Empty},
                    { "PACK LOCATION1", compassListItem.PackingLocation?? string.Empty},
                    { "CUSTOMER SPECIFIC", compassListItem.CustomerSpecific?? string.Empty},
                    { "CUSTOMER", compassListItem.Customer?? string.Empty},
                    { "ORGANIC", compassListItem.Organic?? string.Empty},
                    { "FormLink", compassListItem.WorkflowCurrentUrl ?? string.Empty},
                    { "CommFormLink", compassListItem.WorkflowCommercializationUrl ?? string.Empty},
                    { "TBDFormLink", compassListItem.WorkflowTBDUrl ?? string.Empty},
                    { "ActionGroup", compassListItem.OBM_ActionGroup ?? string.Empty },
                    { "Extra", compassListItem.ProjectNotes ?? string.Empty }
                  };

                string subject = ConvertKeysToValue(emailTemplate.Subject, keysValue);
                if (!emailTemplate.Body.Contains("<#Extra#><br><br>"))
                {
                    emailTemplate.Body = emailTemplate.Body.Insert(0, "<#Extra#><br><br>");
                }
                string body = ConvertKeysToValue(emailTemplate.Body, keysValue);

                try
                {
                    emailService.SendEmail(compassListItem.EmailAddresses, null, subject, body);
                }
                catch (Exception ex)
                {
                    exceptionService.Handle(LogCategory.CriticalError, ex, "NotificationService", "SendNotificationEmail", string.Concat(compassListItem.ProjectNumber, " WorkflowStep=", compassListItem.WorkflowStep.ToString().ToUpper(), " To=", compassListItem.EmailAddresses));
                    configurationService.ResetConfiguration();
                }

                return true;
            }
            return false;
        }
        public bool EmailParentWFStep(string currentWfStep, StageGateCreateProjectItem item)
        {
            bool emailSent = false;
            try
            {
                WorkflowStep setWF = (WorkflowStep)System.Enum.Parse(typeof(WorkflowStep), currentWfStep, true);
                List<string> EmailAddresses = new List<string>();
                List<string> Users = new List<string>();

                if (item.TestProject.ToLower() == "yes")
                {
                    EmailAddresses = userManagementService.GetEmailIds(new List<string>() { GlobalConstants.GROUP_Developers }).ToList();
                }
                else
                {
                    Users.Add(item.ProjectLeader);
                    Users.Add(item.ProjectManager);
                    Users.Add(item.SeniorProjectManager);
                    Users.Add(item.Marketing);
                    Users.Add(item.InTech);
                    Users.Add(item.QAInnovation);
                    Users.Add(item.InTechRegulatory);
                    Users.Add(item.RegulatoryQA);
                    Users.Add(item.PackagingEngineering);
                    Users.Add(item.SupplyChain);
                    Users.Add(item.Finance);
                    Users.Add(item.Sales);
                    Users.Add(item.Manufacturing);
                    Users.Add(item.ExtMfgProcurement);
                    Users.Add(item.PackagingProcurement);
                    Users.Add(item.LifeCycleManagement);
                    Users.Add(item.Legal);
                    Users.Add(item.OtherMember);


                    foreach (var user in Users.Where(a => !string.IsNullOrEmpty(a)))
                    {
                        using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                        {
                            using (SPWeb spWeb = spSite.OpenWeb())
                            {
                                SPFieldUserValueCollection SPuserCollection = new SPFieldUserValueCollection(spWeb, user);
                                foreach (SPFieldUserValue SPuser in SPuserCollection)
                                {
                                    EmailAddresses.Add(SPuser.User.Email);
                                }
                            }
                        }
                    }
                }

                item.EmailAddresses = EmailAddresses;
                if (EmailAddresses.Count > 0)
                {
                    emailSent = SendParentNotificationEmail(item, currentWfStep);
                }
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "NotificationService", "EmailParentWFStep", string.Concat(item.ProjectNumber, " WorkflowStep=", currentWfStep.ToUpper(), " To=", item.EmailAddresses));
                configurationService.ResetConfiguration();
                return emailSent;
            }
            return emailSent;
        }
        public bool EmailBEQRCRequest(string currentWfStep, int itemId, string PackagingComponentsQRCodesTable)
        {
            CompassListItem compassListItem = GetCompassData(itemId);
            compassListItem.WorkflowStep = (WorkflowStep)System.Enum.Parse(typeof(WorkflowStep), currentWfStep, true); ;
            compassListItem.WorkflowCurrentUrl = string.Concat(SPContext.Current.Web.Url, "/pages/", GlobalConstants.PAGE_BEQRC, "?", GlobalConstants.QUERYSTRING_ProjectNo, "=", compassListItem.ProjectNumber);
            compassListItem.WorkflowCommercializationUrl = string.Concat(SPContext.Current.Web.Url, "/pages/", GlobalConstants.PAGE_CommercializationItemSummary, "?", GlobalConstants.QUERYSTRING_ProjectNo, "=", compassListItem.ProjectNumber);
            // Locate the template required
            EmailTemplateField emailTemplate = emailTemplateService.GetEmailTemplate(compassListItem.WorkflowStep.ToString().ToUpper());
            if (emailTemplate != null)
            {

                compassListItem = GetKeys(compassListItem);
                ApprovalListItem approvalItem = GetApprovalKeys(compassListItem.CompassListItemId);
                Hashtable keysValue = new Hashtable
                {
                    { "ProjectNo", compassListItem.ProjectNumber ?? string.Empty },
                    { "PRJECTTYPE", compassListItem.ProjectType ?? string.Empty },
                    { "PRJECTTYPESUBCATEGORY", compassListItem.ProjectTypeSubCategory ?? string.Empty },
                    { "SAP FG#", compassListItem.SAPItemNumber ?? string.Empty },
                    { "SAP DESCRIPTION", compassListItem.SAPDescription ?? string.Empty},
                    { "PROD HIER LVL1", compassListItem.ProductHierarchyLevel1 ?? string.Empty },
                    { "PROD HIER LVL2", compassListItem.ProductHierarchyLevel2 ?? string.Empty },
                    { "BRAND", compassListItem.MaterialGroup1Brand ?? string.Empty },
                    { "OBM", GetPersonFieldForDisplay(compassListItem.PM) ?? string.Empty },
                    { "BRAND MGR", GetPersonFieldForDisplay(compassListItem.Marketing) ?? string.Empty },
                    { "FIRST SHIP DATE", compassListItem.FirstShipDate},
                    { "FORECAST1", compassListItem.Month1ProjectedDollars},
                    { "FORECAST2", compassListItem.Month2ProjectedDollars},
                    { "FORECAST3", compassListItem.Month3ProjectedDollars},
                    { "ANNUAL FORECAST", compassListItem.AnnualProjectedDollars?? string.Empty},
                    { "DISTRIBUTION", compassListItem.DistributionCenter?? string.Empty},
                    { "MAKE LOCATION", compassListItem.ManufacturingLocation?? string.Empty},
                    { "MAKE COUNTRY", compassListItem.ManufacturerCountryOfOrigin?? string.Empty},
                    { "PACK LOCATION1", compassListItem.PackingLocation?? string.Empty},
                    { "CUSTOMER SPECIFIC", compassListItem.CustomerSpecific?? string.Empty},
                    { "CUSTOMER", compassListItem.Customer?? string.Empty},
                    { "ORGANIC", compassListItem.Organic?? string.Empty},
                    { "FormLink", compassListItem.WorkflowCurrentUrl ?? string.Empty},
                    { "CommFormLink", compassListItem.WorkflowCommercializationUrl ?? string.Empty},
                    { "TBDFormLink", compassListItem.WorkflowTBDUrl ?? string.Empty},
                    { "ActionGroup", compassListItem.OBM_ActionGroup ?? string.Empty },
                    { "Extra", compassListItem.ProjectNotes ?? string.Empty },
                    { "PackagingComponentsQRCodesTable", PackagingComponentsQRCodesTable ?? string.Empty },
                    { "ProjectLeader", GetPersonFieldForDisplay(compassListItem.ProjectLeader) ?? string.Empty },
                    { "Marketer", GetPersonFieldForDisplay(compassListItem.Marketing) ?? string.Empty },
                    { "UPC", compassListItem.UnitUPC ?? string.Empty },
                    { "DisplayUPC", compassListItem.DisplayBoxUPC ?? string.Empty },
                    { "UCC", compassListItem.CaseUCC ?? string.Empty },
                    { "NameOfTheRequester", GetLoggedInUserName() ?? string.Empty }
                  };

                string subject = ConvertKeysToValue(emailTemplate.Subject, keysValue);
                if (!emailTemplate.Body.Contains("<#Extra#><br><br>"))
                {
                    emailTemplate.Body = emailTemplate.Body.Insert(0, "<#Extra#><br><br>");
                }
                string body = ConvertKeysToValue(emailTemplate.Body, keysValue);

                List<string> EmailAddresses = new List<string>();
                List<string> EmailAddressesCC = new List<string>();
                if (compassListItem.TestProject.ToLower() == "yes")
                {
                    subject = "Test Project - PLEASE IGNORE !" + subject;
                    body = "THIS IS A TEST PROJECT PLEASE IGNORE!!!!!<br><br>" + body;
                    EmailAddresses = userManagementService.GetEmailIds(new List<string>() { GlobalConstants.GROUP_Developers }).ToList();
                }
                else
                {
                    List<string> emails = GetWorkflowStepEmailGroups(currentWfStep.ToString());

                    EmailAddresses = userManagementService.GetEmailIds(emails, compassListItem.CompassListItemId).ToList();
                    EmailAddressesCC = userManagementService.GetEmailIds(new List<string> { GlobalConstants.GROUP_IndividualInitiator, GlobalConstants.GROUP_IndividualPM, GlobalConstants.GROUP_IndividualProjectLeader, GlobalConstants.GROUP_IndividualBrandManager }, compassListItem.CompassListItemId).ToList();
                }

                try
                {
                    emailService.SendEmail(EmailAddresses, EmailAddressesCC, subject, body);
                    emailLoggingService.LogSentEmailUpdate(itemId, currentWfStep.ToString(), compassListItem.ProjectNumber);
                }
                catch (Exception ex)
                {
                    exceptionService.Handle(LogCategory.CriticalError, ex, "NotificationService", "EmailBEQRCRequest", string.Concat(compassListItem.ProjectNumber, " WorkflowStep=", compassListItem.WorkflowStep.ToString().ToUpper(), " To=", EmailAddresses));
                    configurationService.ResetConfiguration();
                }

                return true;
            }
            return false;
        }

        private string GetLoggedInUserName()
        {
            string Name = "";
            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb oWeb = site.OpenWeb())
                {
                    SPUser user = oWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                    Name = user.Name;
                }
            }

            return Name;
        }
        public bool SendParentNotificationEmail(StageGateCreateProjectItem ParentProjectItem, string EmailTemplateName)
        {
            // Locate the template required
            EmailTemplateField emailTemplate = emailTemplateService.GetEmailTemplate(EmailTemplateName.ToUpper());
            var ProjectInformationFormLink = string.Concat(SPContext.Current.Web.Url, "/pages/", GlobalConstants.PAGE_StageGateCreateProject, "?", GlobalConstants.QUERYSTRING_ProjectNo, "=", ParentProjectItem.ProjectNumber);
            var ProjectSummaryFormLink = string.Concat(SPContext.Current.Web.Url, "/pages/", GlobalConstants.PAGE_StageGateProjectPanel, "?", GlobalConstants.QUERYSTRING_ProjectNo, "=", ParentProjectItem.ProjectNumber);
            if (emailTemplate != null)
            {
                Hashtable keysValue = new Hashtable
                {
                    { "PARENTPROJECTNUMBER", ParentProjectItem.ProjectNumber ?? string.Empty },
                    { "PARENTPROJECTMANAGER", ParentProjectItem.ProjectManagerName ?? string.Empty },
                    { "PARENTPROJECTLEADER", ParentProjectItem.ProjectLeaderName ?? string.Empty },
                    { "PARENTPROJECTNAME", ParentProjectItem.ProjectName ?? string.Empty },
                    { "PROD HIER LVL1", ParentProjectItem.LineOfBisiness ?? string.Empty },
                    { "Material Group 1", ParentProjectItem.Brand ?? string.Empty },
                    { "PRJECTTYPE", ParentProjectItem.ProjectType ?? string.Empty },
                    { "PRJECTTYPESUBCATEGORY", ParentProjectItem.ProjectTypeSubCategory ?? string.Empty },
                    { "ProjectInformationFormLink", ProjectInformationFormLink},
                    { "ProjectSummaryFormLink", ProjectSummaryFormLink},
                  };

                string subject = ConvertKeysToValue(emailTemplate.Subject, keysValue);

                string body = ConvertKeysToValue(emailTemplate.Body, keysValue);

                try
                {
                    emailService.SendEmail(ParentProjectItem.EmailAddresses, null, subject, body);
                }
                catch (Exception ex)
                {
                    exceptionService.Handle(LogCategory.CriticalError, ex, "NotificationService", "SendParentNotificationEmail", string.Concat(ParentProjectItem.ProjectNumber, " WorkflowStep=", EmailTemplateName.ToUpper(), " To=", ParentProjectItem.EmailAddresses));
                    configurationService.ResetConfiguration();
                }

                return true;
            }
            return false;
        }

        public bool SendGraphicsEmail(CompassListItem compassListItem)
        {
            // Locate the template required
            //EmailTemplateField emailTemplate = emailTemplateService.GetEmailTemplate(EmailTemplateKey.EXT_GRAPHICS.ToString());
            //if (emailTemplate != null)
            //{
            //    compassListItem = GetKeys(compassListItem);
            //    ApprovalListItem approvalItem = GetApprovalKeys(compassListItem.CompassListItemId);
            //    Hashtable keysValue = new Hashtable
            //                    {
            //                        { "SAP FG#", compassListItem.SAPItemNumber ?? string.Empty }, 
            //                        { "SAP DESCRIPTION", compassListItem.SAPDescription ?? string.Empty},                                  
            //                        { "FormLink", compassListItem.WorkflowCurrentUrl ?? string.Empty},
            //                        { "ProjectNo", compassListItem.ProjectNumber ?? string.Empty }, 
            //                        { "Approver Comments", approvalItem.SrOBMApproval_Comments ?? string.Empty }, 
            //                        { "CommFormLink", compassListItem.WorkflowCommercializationUrl ?? string.Empty},
            //                        { "TBDFormLink", compassListItem.WorkflowTBDUrl ?? string.Empty},
            //                        { "OBM", GetPersonFieldForDisplay(compassListItem.OBM) ?? string.Empty },
            //                        { "ProdHierarchy1", compassListItem.ProductHierarchyLevel1 ?? string.Empty }, 
            //                        { "ProdHierarchy2", compassListItem.ProductHierarchyLevel2 ?? string.Empty }, 
            //                        { "MaterialGroup1Brand", compassListItem.MaterialGroup1Brand ?? string.Empty }
            //                    };

            //    string subject = ConvertKeysToValue(emailTemplate.Subject, keysValue);
            //    string body = ConvertKeysToValue(emailTemplate.Body, keysValue);

            //    try
            //    {
            //        emailService.SendEmail(compassListItem.EmailAddresses, null, subject, body, compassListItem.ProjectNumber);
            //    }
            //    catch (Exception ex)
            //    {
            //        exceptionService.Handle(LogCategory.CriticalError, ex, "NotificationService", "SendGraphicsEmail", "ProjectNo=" + compassListItem.ProjectNumber);
            //    }

            //    return true;
            //}
            return false;
        }

        public bool SendHelpDeskAccessEmail(string user, string userEmail, string form)
        {
            // Locate the template required
            EmailTemplateField emailTemplate = emailTemplateService.GetEmailTemplate(EmailTemplateKey.HELPDESK_ACCESS.ToString());
            if (emailTemplate != null)
            {
                Hashtable keysValue = new Hashtable
                                {
                                    { "User", user ?? string.Empty },
                                    { "UserEmail", userEmail ?? string.Empty },
                                    { "Form", form ?? string.Empty },
                  };

                string subject = ConvertKeysToValue(emailTemplate.Subject, keysValue);
                string body = ConvertKeysToValue(emailTemplate.Body, keysValue);
                string helpdeskEmail = string.Empty;

                try
                {
                    // Get helpdesk email address
                    helpdeskEmail = configurationService.GetConfiguration(SystemConfiguration.HelpDeskEmail);

                    emailService.SendEmail(helpdeskEmail, subject, body);
                }
                catch (Exception ex)
                {
                    exceptionService.Handle(LogCategory.CriticalError, ex, "NotificationService", "SendHelpDeskAccessEmail", "User=" + user + " Form=" + form);
                }

                return true;
            }
            return false;
        }

        public bool SendHelpDeskLookupRequest(string user, string userEmail, string lookupList, string value)
        {
            // Locate the template required
            EmailTemplateField emailTemplate = emailTemplateService.GetEmailTemplate(EmailTemplateKey.HELPDESK_LOOKUP_REQUEST.ToString());
            if (emailTemplate != null)
            {
                Hashtable keysValue = new Hashtable
                                {
                                    { "User", user ?? string.Empty },
                                    { "UserEmail", userEmail ?? string.Empty },
                                    { "LookupList", lookupList ?? string.Empty },
                                    { "Value", value ?? string.Empty },
                  };

                string subject = ConvertKeysToValue(emailTemplate.Subject, keysValue);
                string body = ConvertKeysToValue(emailTemplate.Body, keysValue);
                string helpdeskEmail = string.Empty;

                try
                {
                    // Get helpdesk email address
                    helpdeskEmail = configurationService.GetConfiguration(SystemConfiguration.HelpDeskEmail);

                    emailService.SendEmail(helpdeskEmail, subject, body);
                }
                catch (Exception ex)
                {
                    exceptionService.Handle(LogCategory.CriticalError, ex, "NotificationService", "SendHelpDeskLookupRequest", "User=" + user + " LookupList=" + lookupList);
                }

                return true;
            }
            return false;
        }

        public bool SendInnovationNotificationEmail(InnovationListItem innovationListItem, List<string> emailAddresses, WorkflowStep wfCurrentStep)
        {
            // Locate the template required
            EmailTemplateField emailTemplate = emailTemplateService.GetEmailTemplate(wfCurrentStep.ToString().ToUpper());
            if (emailTemplate != null)
            {
                Hashtable keysValue = new Hashtable
                                {
                                    { "FormLink", innovationListItem.InnovationLink ?? string.Empty},
                  };

                string subject = ConvertKeysToValue(emailTemplate.Subject, keysValue);
                string body = ConvertKeysToValue(emailTemplate.Body, keysValue);

                try
                {
                    emailService.SendEmail(emailAddresses, null, subject, body);
                }
                catch (Exception ex)
                {
                    exceptionService.Handle(LogCategory.CriticalError, ex, "NotificationService", "SendInnovationNotificationEmail", "InnovationItemId=" + innovationListItem.InnovationListItemId.ToString());
                }

                return true;
            }
            return false;
        }

        #region Private Methods

        private static string ConvertKeysToValue(string htmlBody, Hashtable htmlKeysValue)
        {
            foreach (DictionaryEntry de in htmlKeysValue)
            {
                string oldValue = string.Format("<#{0}#>", de.Key);
                string newValue = de.Value.ToString();

                if (de.Key.ToString() == "PRJECTTYPESUBCATEGORY")
                {
                    if (de.Value.ToString() == "NA")
                    {
                        newValue = "";
                    }
                    else
                    {
                        newValue = "Project Type SubCategory: " + de.Value.ToString();
                    }
                }

                htmlBody = htmlBody.Replace(oldValue, newValue);
            }
            string addBreaks = Regex.Replace(htmlBody, @"\r\n?|\n", "<br>");
            return addBreaks;
        }

        /// <summary>
        /// This method will get key values from the Compass list that are used in the email templates
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private CompassListItem GetKeys(CompassListItem newItem)
        {
            try
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPListItem item = spList.GetItemById(newItem.CompassListItemId);
                        if (item != null)
                        {
                            newItem.ProjectNumber = Convert.ToString(item[CompassListFields.ProjectNumber]);
                            newItem.ProjectType = Convert.ToString(item[CompassListFields.ProjectType]);
                            newItem.MaterialGroup1Brand = Convert.ToString(item[CompassListFields.MaterialGroup1Brand]);
                            newItem.SubmittedDate = Convert.ToDateTime(item[CompassListFields.SubmittedDate]);
                            newItem.ItemConcept = Convert.ToString(item[CompassListFields.ItemConcept]);
                            newItem.FirstShipDate = Convert.ToDateTime(item[CompassListFields.FirstShipDate]);
                            newItem.RevisedFirstShipDate = Convert.ToDateTime(item[CompassListFields.RevisedFirstShipDate]);
                            newItem.ProjectTypeSubCategory = Convert.ToString(item[CompassListFields.ProjectTypeSubCategory]);

                            // Project Team Fields
                            newItem.Initiator = Convert.ToString(item[CompassListFields.Initiator]);
                            newItem.PM = Convert.ToString(item[CompassListFields.PM]);
                            newItem.PMName = Convert.ToString(item[CompassListFields.PMName]);
                            // SAP Item # Fields
                            newItem.TBDIndicator = Convert.ToString(item[CompassListFields.TBDIndicator]);
                            newItem.SAPItemNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                            newItem.SAPDescription = Convert.ToString(item[CompassListFields.SAPDescription]);
                            newItem.LikeFGItemNumber = Convert.ToString(item[CompassListFields.LikeFGItemNumber]);
                            newItem.LikeFGItemDescription = Convert.ToString(item[CompassListFields.LikeFGItemDescription]);
                            // Project Specifications Fields
                            newItem.NewFormula = Convert.ToString(item[CompassListFields.NewFormula]);
                            newItem.Organic = Convert.ToString(item[CompassListFields.Organic]);
                            newItem.ServingSizeWeightChange = Convert.ToString(item[CompassListFields.ServingSizeWeightChange]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "NotificationService", "GetKeys", "ProjectNo=" + newItem.ProjectNumber);
            }

            return newItem;
        }

        private ApprovalListItem GetApprovalKeys(int itemId)
        {
            ApprovalListItem appItem = new ApprovalListItem();
            try
            {
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
                                appItem.ApprovalListItemId = item.ID;
                                appItem.CompassListItemId = Convert.ToInt32(item[ApprovalListFields.CompassListItemId]);

                                // Initial Costing Fields
                                //appItem.SrOBMApproval_Comments = Convert.ToString(item[ApprovalListFields.SrOBMApproval_Comments]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "NotificationService", "GetApprovalKeys");
            }

            return appItem;
        }

        private List<string> GetGraphicsAttachments(string projectNumber)
        {
            List<string> attachments = new List<string>();

            var projectFiles = utilityService.GetUploadedCompassFilesByDocType(projectNumber, GlobalConstants.DOCTYPE_NLEA);
            foreach (FileAttribute fileAttribute in projectFiles)
                attachments.Add(fileAttribute.FileUrl);

            projectFiles = utilityService.GetUploadedCompassFilesByDocType(projectNumber, GlobalConstants.DOCTYPE_PalletPattern);
            foreach (FileAttribute fileAttribute in projectFiles)
                attachments.Add(fileAttribute.FileUrl);

            projectFiles = utilityService.GetUploadedCompassFilesByDocType(projectNumber, GlobalConstants.DOCTYPE_GraphicsRequest);
            foreach (FileAttribute fileAttribute in projectFiles)
                attachments.Add(fileAttribute.FileUrl);

            projectFiles = utilityService.GetUploadedCompassFilesByDocType(projectNumber, GlobalConstants.DOCTYPE_CADDrawing);
            foreach (FileAttribute fileAttribute in projectFiles)
                attachments.Add(fileAttribute.FileUrl);

            return attachments;
        }

        public static string GetPersonFieldForDisplay(string person)
        {
            if (string.IsNullOrEmpty(person))
                return string.Empty;
            if (person.IndexOf("#") < 0)
                return person;

            return person.Substring(person.IndexOf("#") + 1);
        }
        #endregion
        private CompassListItem GetCompassData(int itemId)
        {
            var sgItem = new CompassListItem();
            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    var item = spList.GetItemById(itemId);
                    if (item != null)
                    {
                        sgItem.CompassListItemId = item.ID;
                        sgItem.SAPItemNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                        sgItem.SAPDescription = Convert.ToString(item[CompassListFields.SAPDescription]);
                        sgItem.ProjectNumber = Convert.ToString(item[CompassListFields.ProjectNumber]);
                        sgItem.MaterialGroup1Brand = Convert.ToString(item[CompassListFields.MaterialGroup1Brand]);
                        sgItem.TBDIndicator = Convert.ToString(item[CompassListFields.TBDIndicator]);
                        sgItem.ManufacturingLocation = Convert.ToString(item[CompassListFields.ManufacturingLocation]);
                        sgItem.PackingLocation = Convert.ToString(item[CompassListFields.PackingLocation]);
                        sgItem.ProjectType = Convert.ToString(item[CompassListFields.ProjectType]);
                        sgItem.ProjectTypeSubCategory = Convert.ToString(item[CompassListFields.ProjectTypeSubCategory]);
                        sgItem.PM = Convert.ToString(item[CompassListFields.PM]);
                        sgItem.PMName = Convert.ToString(item[CompassListFields.PMName]);
                        sgItem.UnitUPC = Convert.ToString(item[CompassListFields.UnitUPC]);
                        sgItem.DisplayBoxUPC = Convert.ToString(item[CompassListFields.DisplayBoxUPC]);
                        sgItem.CaseUCC = Convert.ToString(item[CompassListFields.CaseUCC]);
                        sgItem.RevisedFirstShipDate = Convert.ToDateTime(item[CompassListFields.RevisedFirstShipDate]);
                        sgItem.OBM_PackagingNumbers = Convert.ToString(item[CompassListFields.PackagingNumbers]);
                        sgItem.ProductHierarchyLevel1 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]);
                        sgItem.ProductHierarchyLevel2 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel2]);
                        sgItem.Month1ProjectedDollars = Convert.ToDouble(item[CompassListFields.Month1ProjectedDollars]);
                        sgItem.Month2ProjectedDollars = Convert.ToDouble(item[CompassListFields.Month2ProjectedDollars]);
                        sgItem.Month3ProjectedDollars = Convert.ToDouble(item[CompassListFields.Month3ProjectedDollars]);
                        sgItem.Month1ProjectedUnits = Convert.ToInt32(item[CompassListFields.Month1ProjectedUnits]);
                        sgItem.Month2ProjectedUnits = Convert.ToInt32(item[CompassListFields.Month2ProjectedUnits]);
                        sgItem.Month3ProjectedUnits = Convert.ToInt32(item[CompassListFields.Month3ProjectedUnits]);
                        sgItem.AnnualProjectedDollars = Convert.ToString(item[CompassListFields.AnnualProjectedUnits]);
                        sgItem.ManufacturerCountryOfOrigin = Convert.ToString(item[CompassListFields.ManufacturerCountryOfOrigin]);
                        sgItem.TestProject = Convert.ToString(item[CompassListFields.TestProject]);
                        SPUser user = spWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                        if (user != null)
                        {
                            sgItem.InitiatorName = SPContext.Current.Web.CurrentUser.LoginName;
                        }
                    }
                    var spList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTeamListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassTeamListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    var ipItems = spList2.GetItems(spQuery);

                    if (ipItems != null && ipItems.Count > 0)
                    {
                        var ipItem = ipItems[0];
                        sgItem.Marketing = Convert.ToString(ipItem[CompassTeamListFields.Marketing]);
                        sgItem.MarketingName = Convert.ToString(ipItem[CompassTeamListFields.MarketingName]);
                        sgItem.ProjectLeader = Convert.ToString(ipItem[CompassTeamListFields.ProjectLeader]);
                        sgItem.ProjectLeaderName = Convert.ToString(ipItem[CompassTeamListFields.ProjectLeaderName]);
                    }
                }
            }
            return sgItem;
        }
    }
}
