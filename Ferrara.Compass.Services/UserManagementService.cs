using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IExceptionService exceptionService;
        private readonly ICacheManagementService cacheManagementService;
        private readonly IUtilityService utilityService;
        private static object _lock;

        public UserManagementService(IExceptionService exceptionService, ICacheManagementService cacheService, IUtilityService utilityService)
        {
            _lock = new object();
            this.exceptionService = exceptionService;
            this.cacheManagementService = cacheService;
            this.utilityService = utilityService;
        }

        public IEnumerable<string> GetAllGroups()
        {
            List<string> groups = new List<string>();
            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    groups.AddRange(from SPGroup g in web.Groups select g.Name);
                }
            }
            return groups;
        }

        public IEnumerable<string> GetCurrentUserGroups()
        {
            List<string> groups = new List<string>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPUser spUser = spWeb.CurrentUser;

                    if (spUser != null)
                    {
                        if (spUser.Groups != null)
                        {
                            groups.AddRange(from SPGroup g in spUser.Groups select g.Name);
                        }
                    }
                }
            }
            return groups;
        }

        public IEnumerable<string> GetEmailIds(List<string> groups)
        {
            List<string> emailAddresses = new List<string>();
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        foreach (string group in groups)
                        {
                            SPGroup approverGroup = web.Groups[group];
                            if (approverGroup != null)
                            {
                                emailAddresses.AddRange(
                                    from SPUser u in approverGroup.Users
                                    where !string.IsNullOrEmpty(u.Email)
                                    select u.Email);
                            }
                        }
                    }
                }
            });

            return emailAddresses;
        }

        public IEnumerable<string> GetEmailIds(List<string> groups, int compassItemId)
        {
            List<string> emailAddresses = new List<string>();
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        foreach (string group in groups)
                        {
                            if (group.Trim().Contains("@"))
                            {
                                // We have an individual email address
                                emailAddresses.Add(group.Trim());
                            }
                            else if (string.Equals(group.Trim(), GlobalConstants.GROUP_IndividualBrandManager))
                            {
                                // Retrieve the Marketing from the IPF
                                var marketingUsers = GetCompassMarketing(compassItemId);
                                if (marketingUsers.Count > 0)
                                {
                                    foreach (var marketingUser in marketingUsers)
                                    {
                                        emailAddresses.Add(marketingUser.Email);
                                    }
                                }
                                else
                                {
                                    SPGroup approverGroup = web.Groups[CheckIndividualGroup(group.Trim())];
                                    if (approverGroup != null)
                                    {
                                        emailAddresses.AddRange(
                                            from SPUser u in approverGroup.Users
                                            where !string.IsNullOrEmpty(u.Email)
                                            select u.Email);
                                    }
                                }
                            }
                            else if (string.Equals(group.Trim(), GlobalConstants.GROUP_IndividualProjectLeader))
                            {
                                // Retrieve Project Leader
                                List<SPUser> projectLeaders = GetCompassProjectLeader(compassItemId);
                                if (projectLeaders != null)
                                {
                                    foreach (var projectLeader in projectLeaders)
                                    {
                                        emailAddresses.Add(projectLeader.Email);
                                    }
                                }
                            }
                            else if (string.Equals(group.Trim(), GlobalConstants.GROUP_IndividualPackagingEngineer))
                            {
                                // Retrieve the Packaging Engineer from the Compass List
                                SPUser packEng = GetCompassPackagingEngineer(compassItemId);
                                if (packEng != null)
                                {
                                    emailAddresses.Add(packEng.Email);
                                }
                                else
                                {
                                    SPGroup approverGroup = web.Groups[CheckIndividualGroup(group.Trim())];
                                    if (approverGroup != null)
                                    {
                                        emailAddresses.AddRange(
                                            from SPUser u in approverGroup.Users
                                            where !string.IsNullOrEmpty(u.Email)
                                            select u.Email);
                                    }
                                }
                            }
                            else if (string.Equals(group.Trim(), GlobalConstants.GROUP_IndividualPM) || string.Equals(group.Trim(), GlobalConstants.GROUP_IndividualOBM))
                            {
                                // Retrieve the PM from the IPF
                                List<SPUser> PMs = GetCompassPM(compassItemId);
                                if (PMs != null)
                                {
                                    foreach (var PM in PMs)
                                    {
                                        emailAddresses.Add(PM.Email);
                                    }
                                }
                                else
                                {
                                    SPGroup approverGroup = web.Groups[CheckIndividualGroup(group.Trim())];
                                    if (approverGroup != null)
                                    {
                                        emailAddresses.AddRange(
                                            from SPUser u in approverGroup.Users
                                            where !string.IsNullOrEmpty(u.Email)
                                            select u.Email);
                                    }
                                }
                            }
                            else if (string.Equals(group.Trim(), GlobalConstants.GROUP_IndividualSeniorPM))
                            {
                                // Retrieve the PM from the IPF
                                List<SPUser> SrPms = GetCompassSrPM(compassItemId);
                                if (SrPms != null)
                                {
                                    foreach (var SrPm in SrPms)
                                    {
                                        emailAddresses.Add(SrPm.Email);
                                    }
                                }
                                else
                                {
                                    SPGroup approverGroup = web.Groups[CheckIndividualGroup(group.Trim())];
                                    if (approverGroup != null)
                                    {
                                        emailAddresses.AddRange(
                                            from SPUser u in approverGroup.Users
                                            where !string.IsNullOrEmpty(u.Email)
                                            select u.Email);
                                    }
                                }
                            }
                            else if (string.Equals(group.Trim(), GlobalConstants.GROUP_IndividualInTechLead))
                            {
                                // Retrieve the InTech Lead from the IPF
                                List<SPUser> inTechs = GetCompassInTechLead(compassItemId);
                                if (inTechs != null)
                                {
                                    foreach (var inTech in inTechs)
                                    {
                                        emailAddresses.Add(inTech.Email);
                                    }
                                }
                            }
                            else if (string.Equals(group.Trim(), GlobalConstants.GROUP_IndividualInitiator))
                            {
                                // Retrieve the Initiator from the IPF
                                List<SPUser> initiators = GetCompassInitiator(compassItemId);
                                if (initiators != null)
                                {
                                    foreach (var initiator in initiators)
                                    {
                                        emailAddresses.Add(initiator.Email);
                                    }
                                }
                            }
                            else
                            {
                                SPGroup approverGroup = web.Groups[group.Trim()];
                                if (approverGroup != null)
                                {
                                    emailAddresses.AddRange(
                                        from SPUser u in approverGroup.Users
                                        where !string.IsNullOrEmpty(u.Email)
                                        select u.Email);
                                }
                            }
                        }
                    }
                }
            });

            return emailAddresses;
        }

        public string GetEmailIds(WorkflowStep wfCurrentStep, int itemId)
        {
            string emailList = string.Empty;

            // Get the list of all Workflow Steps
            List<WFStepField> wfAllSteps = utilityService.GetWorkflowSteps();
            var currentWfStep = wfAllSteps.FirstOrDefault(x => x.WorkflowStep.Equals(wfCurrentStep));
            if (currentWfStep != null)
            {
                IEnumerable<string> emails = GetEmailIds(currentWfStep.EmailGroups, itemId).ToList();
                foreach (string email in emails)
                    emailList = emailList + email + " ";

                emailList = emailList.Replace("@ferrarausa.com", "");
                if (emailList.Length > 255)
                    emailList = emailList.Substring(0, 255);
                return emailList;
            }
            return emailList;
        }

        public bool IsCurrentUserInGroup(string groupName)
        {
            var currentUsergroups = GetCurrentUserGroups().ToList();
            return currentUsergroups.Exists(g => g.ToUpperInvariant() == groupName.ToUpperInvariant());
        }

        public bool IsCurrentUserInGroups(IList<string> groups)
        {
            bool isExist = false;

            try
            {
                foreach (string group in groups)
                {
                    isExist = SPContext.Current.Web.Groups[CheckIndividualGroup(group)].ContainsCurrentUser;
                    //isExist = currentUsergroups.Exists(g => g.ToUpperInvariant() == group.ToUpperInvariant());
                    if (isExist)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "User Management Service", "IsCurrentUserInGroups");
            }

            return isExist;
        }

        public bool IsCurrentUserInGroups(IList<string> groups, string url)
        {
            bool isExist = false;

            using (SPSite site = new SPSite(url))
            {
                using (SPWeb oWeb = site.OpenWeb())
                {
                    foreach (string group in groups)
                    {
                        isExist = oWeb.Groups[CheckIndividualGroup(group)].ContainsCurrentUser;
                        if (isExist)
                        {
                            return true;
                        }
                    }
                }
            }
            return isExist;
        }

        public string GetLoginNameFromPreferredName(string preferredName, string email)
        {
            string loginName = string.Empty;

            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPUser user;
                        if (string.IsNullOrEmpty(email))
                        {
                            user =
                                web.Users.Cast<SPUser>().FirstOrDefault(
                                    t =>
                                    t.Name.ToUpperInvariant().Equals(
                                        preferredName.ToUpperInvariant()));
                        }
                        else
                        {
                            user = web.Users.GetByEmail(email);
                        }
                        if (user != null)
                        {
                            loginName = user.LoginName;
                        }
                    }
                }
            });

            return loginName;
        }

        public string GetLoggedInUserEmailId(string url)
        {
            string emailId = "";
            using (SPSite site = new SPSite(url))
            {
                using (SPWeb oWeb = site.OpenWeb())
                {
                    SPUser user = oWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                    emailId = user.Email;
                }
            }

            return emailId;
        }

        #region Get User Methods
        private List<SPUser> GetCompassMarketing(int itemId)
        {
            List<SPUser> MarketingUsers = new List<SPUser>();

            try
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTeamListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassTeamListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                        var ipItems = spList.GetItems(spQuery);
                        if (ipItems != null && ipItems.Count > 0)
                        {
                            var item = ipItems[0];
                            var Marketing = Convert.ToString(item[CompassTeamListFields.Marketing]);
                            SPFieldUserValueCollection Marketingcollection = new SPFieldUserValueCollection(spWeb, Marketing);
                            foreach (SPFieldUserValue MarketingUser in Marketingcollection)
                            {
                                var spUser = MarketingUser.User;
                                MarketingUsers.Add(spUser);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "User Management Service", "GetCompassMarketing");
            }

            return MarketingUsers;
        }
        private List<SPUser> GetCompassProjectLeader(int itemId)
        {
            List<SPUser> ProjectLeaderUsers = new List<SPUser>();

            try
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTeamListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassTeamListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                        var ipItems = spList.GetItems(spQuery);
                        if (ipItems != null && ipItems.Count > 0)
                        {
                            var item = ipItems[0];
                            var ProjectLeaders = Convert.ToString(item[CompassTeamListFields.ProjectLeader]);
                            SPFieldUserValueCollection ProjectLeaderscollection = new SPFieldUserValueCollection(spWeb, ProjectLeaders);
                            foreach (SPFieldUserValue ProjectLeader in ProjectLeaderscollection)
                            {
                                var spUser = ProjectLeader.User;
                                ProjectLeaderUsers.Add(spUser);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "User Management Service", "GetCompassProjectLeader");
            }

            return ProjectLeaderUsers;
        }
        private SPUser GetCompassPackagingEngineer(int itemId)
        {
            SPUser packEng = null;

            try
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPListItem item = spList.GetItemById(itemId);
                        if (item != null)
                        {
                            if (item[CompassListFields.PackagingEngineerLead] != null)
                            {
                                var userField = (SPFieldUser)item.Fields.GetField(CompassListFields.PackagingEngineerLead);
                                var fieldValue = (SPFieldUserValue)userField.GetFieldValue((string)item[CompassListFields.PackagingEngineerLead]);
                                packEng = fieldValue.User;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "User Management Service", "GetCompassPackagingEngineer");
            }

            return packEng;
        }
        private List<SPUser> GetCompassPM(int itemId)
        {
            List<SPUser> Users = new List<SPUser>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem item = spList.GetItemById(itemId);
                    if (item != null)
                    {
                        var PMs = Convert.ToString(item[CompassListFields.PM]);
                        SPFieldUserValueCollection PMcollection = new SPFieldUserValueCollection(spWeb, PMs);
                        foreach (SPFieldUserValue PM in PMcollection)
                        {
                            var spUser = PM.User;
                            Users.Add(spUser);
                        }
                    }
                }
            }
            return Users;
        }

        private List<SPUser> GetCompassSrPM(int itemId)
        {
            List<SPUser> Users = new List<SPUser>();
            var team = GetCompassTeam(itemId);
            var SrPMs = team.SrProjectManager;
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPFieldUserValueCollection SrPmcollection = new SPFieldUserValueCollection(spWeb, SrPMs);
                    foreach (SPFieldUserValue PM in SrPmcollection)
                    {
                        var spUser = PM.User;
                        Users.Add(spUser);
                    }
                }
            }

            return Users;
        }
        private ItemProposalItem GetCompassTeam(int itemId)
        {
            ItemProposalItem newItem = new ItemProposalItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTeamListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    var ipItems = spList.GetItems(spQuery);
                    if (ipItems != null && ipItems.Count > 0)
                    {
                        var ipItem = ipItems[0];

                        newItem.ProjectLeader = Convert.ToString(ipItem[StageGateProjectListFields.ProjectLeader]);
                        newItem.ProjectLeaderName = Convert.ToString(ipItem[StageGateProjectListFields.ProjectLeaderName]);
                        newItem.SrProjectManager = Convert.ToString(ipItem[StageGateProjectListFields.SeniorProjectManager]);
                        newItem.SrProjectManagerName = Convert.ToString(ipItem[StageGateProjectListFields.SeniorProjectManagerName]);
                        newItem.QA = Convert.ToString(ipItem[StageGateProjectListFields.QAInnovation]);
                        newItem.QAName = Convert.ToString(ipItem[StageGateProjectListFields.QAInnovationName]);
                        newItem.InTech = Convert.ToString(ipItem[StageGateProjectListFields.InTech]);
                        newItem.InTechName = Convert.ToString(ipItem[StageGateProjectListFields.InTechName]);
                        newItem.InTechRegulatory = Convert.ToString(ipItem[StageGateProjectListFields.InTechRegulatory]);
                        newItem.InTechRegulatoryName = Convert.ToString(ipItem[StageGateProjectListFields.InTechRegulatoryName]);
                        newItem.RegulatoryQA = Convert.ToString(ipItem[StageGateProjectListFields.RegulatoryQA]);
                        newItem.RegulatoryQAName = Convert.ToString(ipItem[StageGateProjectListFields.RegulatoryQAName]);
                        newItem.PackagingEngineering = Convert.ToString(ipItem[StageGateProjectListFields.PackagingEngineering]);
                        newItem.PackagingEngineeringName = Convert.ToString(ipItem[StageGateProjectListFields.PackagingEngineeringName]);
                        newItem.SupplyChain = Convert.ToString(ipItem[StageGateProjectListFields.SupplyChain]);
                        newItem.SupplyChainName = Convert.ToString(ipItem[StageGateProjectListFields.SupplyChainName]);
                        newItem.Finance = Convert.ToString(ipItem[StageGateProjectListFields.Finance]);
                        newItem.FinanceName = Convert.ToString(ipItem[StageGateProjectListFields.FinanceName]);
                        newItem.Sales = Convert.ToString(ipItem[StageGateProjectListFields.Sales]);
                        newItem.SalesName = Convert.ToString(ipItem[StageGateProjectListFields.SalesName]);
                        newItem.Manufacturing = Convert.ToString(ipItem[StageGateProjectListFields.Manufacturing]);
                        newItem.ManufacturingName = Convert.ToString(ipItem[StageGateProjectListFields.ManufacturingName]);
                        newItem.OtherTeamMembers = Convert.ToString(ipItem[StageGateProjectListFields.OtherMember]);
                        newItem.OtherTeamMembersName = Convert.ToString(ipItem[StageGateProjectListFields.OtherMemberName]);
                        newItem.LifeCycleManagement = Convert.ToString(ipItem[StageGateProjectListFields.LifeCycleManagement]);
                        newItem.LifeCycleManagementName = Convert.ToString(ipItem[StageGateProjectListFields.LifeCycleManagementName]);
                        newItem.PackagingProcurement = Convert.ToString(ipItem[StageGateProjectListFields.PackagingProcurement]);
                        newItem.PackagingProcurementName = Convert.ToString(ipItem[StageGateProjectListFields.PackagingProcurementName]);
                        newItem.ExtManufacturingProc = Convert.ToString(ipItem[StageGateProjectListFields.ExtMfgProcurement]);
                        newItem.ExtManufacturingProcName = Convert.ToString(ipItem[StageGateProjectListFields.ExtMfgProcurementName]);
                    }
                }
            }
            return newItem;
        }

        private List<SPUser> GetCompassInTechLead(int itemId)
        {
            string newFormula = string.Empty;
            List<SPUser> Users = new List<SPUser>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTeamListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassTeamListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    SPListItemCollection items = spList.GetItems(spQuery);
                    if (items.Count > 0)
                    {
                        SPListItem item = items[0];
                        if (item != null)
                        {
                            var InTechLeads = Convert.ToString(item[CompassTeamListFields.InTech]);
                            SPFieldUserValueCollection InTechLeadscollection = new SPFieldUserValueCollection(spWeb, InTechLeads);
                            foreach (SPFieldUserValue InTechLead in InTechLeadscollection)
                            {
                                var spUser = InTechLead.User;
                                Users.Add(spUser);
                            }

                        }
                    }
                    SPList spList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem item2 = spList2.GetItemById(itemId);
                    if (item2 != null)
                    {
                        newFormula = Convert.ToString(item2[CompassListFields.NewFormula]);
                    }
                }
            }

            // Only email user if this is a new formula change
            if (!string.Equals(newFormula, "Yes"))
                return null;

            return Users;
        }

        private List<SPUser> GetCompassInitiator(int itemId)
        {
            List<SPUser> Users = new List<SPUser>();

            try
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPListItem item = spList.GetItemById(itemId);
                        if (item != null)
                        {
                            var Initiators = Convert.ToString(item[CompassListFields.Initiator]);
                            SPFieldUserValueCollection Initiatorcollection = new SPFieldUserValueCollection(spWeb, Initiators);
                            foreach (SPFieldUserValue Initiator in Initiatorcollection)
                            {
                                var spUser = Initiator.User;
                                Users.Add(spUser);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "User Management Service", "GetCompassInitiator");
            }

            return Users;
        }
        #endregion

        private string CheckIndividualGroup(string group)
        {
            if (string.Equals(group, GlobalConstants.GROUP_IndividualBrandManager))
                return GlobalConstants.GROUP_Marketing;
            else if (string.Equals(group, GlobalConstants.GROUP_IndividualSeniorPM))
                return GlobalConstants.GROUP_SeniorProjectManager;
            else if (string.Equals(group, GlobalConstants.GROUP_IndividualPackagingEngineer))
                return GlobalConstants.GROUP_PackagingEngineer;
            else if (string.Equals(group, GlobalConstants.GROUP_IndividualPM) || string.Equals(group, GlobalConstants.GROUP_IndividualOBM))
                return GlobalConstants.GROUP_ProjectManagers;
            else if (string.Equals(group, GlobalConstants.GROUP_IndividualInTechLead))
                return GlobalConstants.GROUP_InTech;
            else if (string.Equals(group, GlobalConstants.GROUP_IndividualInitiator))
                return GlobalConstants.GROUP_IndividualInitiator;
            else if (string.Equals(group, GlobalConstants.GROUP_IndividualProjectLeader))
                return GlobalConstants.GROUP_ProjectManagers;

            return group;
        }

        public string GetUserNameFromPersonField(string personField)
        {
            try
            {
                SPFieldUserValue userValue = new SPFieldUserValue(SPContext.Current.Web, personField);
                if (!string.IsNullOrEmpty(personField))
                {
                    return userValue.User.Name;
                }
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "User Management Service", "GetUserNameFromPersonField", string.Concat("Person Field: ", personField));
            }

            return string.Empty;
        }

        public bool HasReadAccess(CompassForm compassForm)
        {
            bool canAccess = false;
            List<FormAccessItem> formList = utilityService.GetFormAccessList();
            var formAccess = formList.FirstOrDefault(x => x.Title.Equals(compassForm.ToString()));
            if (formAccess != null)
            {
                //string accessgrps = string.Empty;
                //foreach (string group in formAccess.AccessGroups)
                //    accessgrps = accessgrps + group;
                //exceptionService.Handle(LogCategory.General, "Message", "User Management Service", "HasReadAccess", "CompassForm=" + compassForm.ToString() + " AccessGroups=" + accessgrps);
                canAccess = IsCurrentUserInGroups(formAccess.AccessGroups);
            }
            return canAccess;
        }

        public bool HasWriteAccess(CompassForm compassForm)
        {
            bool canAccess = false;
            List<FormAccessItem> formList = utilityService.GetFormAccessList();
            var formAccess = formList.FirstOrDefault(x => x.Title.Equals(compassForm.ToString()));
            if (formAccess != null)
            {
                canAccess = IsCurrentUserInGroups(formAccess.EditGroups);
            }
            return canAccess;
        }

        public bool HasWriteAccess(CompassForm compassForm, string url)
        {
            bool canAccess = false;
            List<FormAccessItem> formList = utilityService.GetFormAccessList(url);
            if (formList != null)
            {
                var formAccess = formList.FirstOrDefault(x => x.Title.Equals(compassForm.ToString()));
                if (formAccess != null)
                {
                    canAccess = IsCurrentUserInGroups(formAccess.EditGroups, url);
                }
            }
            return canAccess;
        }
    }
}
