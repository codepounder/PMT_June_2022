using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;
using System.Text.RegularExpressions;

namespace Ferrara.Compass.Services
{
    public class ProjectNotesService : IProjectNotesService
    {
        #region Member Variables
        private readonly IExceptionService exceptionService;
        #endregion

        #region Constructors
        public ProjectNotesService()
        {

        }

        public ProjectNotesService(IExceptionService exceptionService)
        {
            this.exceptionService = exceptionService;
        }
        #endregion
        public string GetProjectComments(int itemId)
        {
            //InnovationListItem newItem = new InnovationListItem();
            string newItem = string.Empty;

            try
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        var item = spList.GetItemById(itemId);
                        if (item != null)
                        {
                            newItem = Convert.ToString(item[CompassListFields.ProjectNotes]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "ProjectNotesService", "GetProjectComments", string.Concat("itemId:", itemId));
            }
            return newItem;
        }
        public string GetProjectCommentsHistory(int itemId)
        {
            try
            {
                StringBuilder newItem = new StringBuilder();

                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        var item = spList.GetItemById(itemId);

                        if (item != null)
                        {
                            SPListItemVersionCollection versionCol = item.Versions;
                            string lastVersion = string.Empty;
                            string currentVersion = string.Empty;
                            bool bFirstTime = true;
                            SPListItemVersion previousVersion = item.Versions[0];

                            foreach (SPListItemVersion version in versionCol)
                            {

                                if (bFirstTime)
                                {
                                    previousVersion = version;
                                    lastVersion = Convert.ToString(previousVersion[CompassListFields.ProjectNotes]);
                                    bFirstTime = false;

                                    continue;
                                }

                                if (!string.IsNullOrEmpty(Convert.ToString(version[CompassListFields.ProjectNotes])) || (!string.IsNullOrEmpty(lastVersion)))
                                {
                                    currentVersion = Convert.ToString(version[CompassListFields.ProjectNotes]);

                                    if (!string.Equals(lastVersion, currentVersion))
                                    {
                                        //newItem.Append(string.Concat(previousVersion.VersionLabel, " ", previousVersion.CreatedBy.User.Name, ": ", GetLocalDateTime(previousVersion.Created.ToString())));
                                        newItem.Append(string.Concat("<b>", previousVersion.CreatedBy.User.Name, ": ", GetLocalDateTime(previousVersion.Created.ToString()), "</b>"));
                                        newItem.Append("<br>");
                                        newItem.Append(lastVersion);
                                        newItem.Append("<br>");

                                        previousVersion = version;
                                        lastVersion = Convert.ToString(previousVersion[CompassListFields.ProjectNotes]);
                                    }
                                }
                                previousVersion = version;
                            }
                        }
                    }
                }
                return newItem.ToString();
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "ProjectNotesService", "GetProjectCommentsHistory", "Exception while getting the project comments history. - itemId:" + itemId);
                return string.Empty;
            }

        }
        public void CopyProjectNotes(int copyFromId, int copyToId)
        {
            try
            {
                string oldComments = GetProjectCommentsHistory(copyFromId);
                string[] stringSeparators = new string[] { "<br>" };
                string[] parsedComments = oldComments.Split(stringSeparators, StringSplitOptions.None);
                List<string> commentsList = new List<string>();
                int i = 0;
                foreach (String m in parsedComments)
                {
                    i = Array.IndexOf(parsedComments, m);
                    if (i % 2 != 0 && !String.IsNullOrEmpty(m))
                    {
                        commentsList.Add(m + "<br>Original Commentor: " + parsedComments[i - 1]);
                    }
                }

                StringBuilder newItem = new StringBuilder();
                if (commentsList.Count > 0)
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (var spSite = new SPSite(SPContext.Current.Web.Url))
                        {
                            using (var spWeb = spSite.OpenWeb())
                            {
                                spWeb.AllowUnsafeUpdates = true;
                                var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                                var item = spList.GetItemById(copyToId);
                                if (item != null)
                                {
                                    foreach (string oldComment in commentsList)
                                    {
                                        item[CompassListFields.ProjectNotes] = oldComment;

                                        // Set Modified By to current user NOT System Account
                                        item["Editor"] = SPContext.Current.Web.CurrentUser;

                                        item.Update();
                                    }
                                }
                                spWeb.AllowUnsafeUpdates = false;
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "ProjectNotesService", "CopyProjectNotes", string.Concat("Exception while copying project notes.", " - copyFromId:", copyFromId, "- copyToId:", copyToId));
            }
        }
        public void UpdateProjectComments(int itemId, string comments)
        {
            try
            {
                if (string.IsNullOrEmpty(comments))
                    return;

                // Get the current user
                SPUser currentUser = SPContext.Current.Web.CurrentUser;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb spWeb = spSite.OpenWeb())
                        {
                            spWeb.AllowUnsafeUpdates = true;
                            SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                            SPListItem item = spList.GetItemById(itemId);

                            if (item != null)
                            {
                                if (!string.IsNullOrEmpty(comments))
                                {
                                    item[CompassListFields.ProjectNotes] = comments;

                                    // Set Modified By to current user NOT System Account
                                    item["Editor"] = SPContext.Current.Web.CurrentUser;

                                    item.Update();
                                }
                            }
                            spWeb.AllowUnsafeUpdates = false;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "ProjectNotesService", "UpdateProjectComments", "Exception while updating the project comments.");
            }
        }
        #region Stage Gate Project Methods
        public string GetStageGateProjectCommentsHistory(int itemId)
        {
            try
            {
                StringBuilder newItem = new StringBuilder();

                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);

                        var item = spList.GetItemById(itemId);

                        if (item != null)
                        {
                            SPListItemVersionCollection versionCol = item.Versions;
                            string lastVersion = string.Empty;
                            string currentVersion = string.Empty;
                            bool bFirstTime = true;
                            SPListItemVersion previousVersion = item.Versions[0];

                            foreach (SPListItemVersion version in versionCol)
                            {

                                //if (bFirstTime)
                                //{
                                //    previousVersion = version;
                                //    lastVersion = Convert.ToString(previousVersion[PMTProjectListFields.ProjectNotes]);
                                //    bFirstTime = false;

                                //    continue;
                                //}

                                if (bFirstTime || !string.IsNullOrEmpty(Convert.ToString(version[StageGateProjectListFields.ProjectNotes])) || (!string.IsNullOrEmpty(lastVersion)))
                                {
                                    currentVersion = Convert.ToString(version[StageGateProjectListFields.ProjectNotes]);

                                    if (!string.Equals(lastVersion, currentVersion))
                                    {
                                        if (!string.IsNullOrWhiteSpace(currentVersion))
                                        {
                                            newItem.Append(string.Concat("<b>", version.CreatedBy.User.Name, ": ", GetLocalDateTime(version.Created.ToString()), "</b>"));
                                            newItem.Append("<br>");
                                            newItem.Append(currentVersion);
                                            newItem.Append("<br>");
                                        }

                                        previousVersion = version;
                                        lastVersion = Convert.ToString(previousVersion[StageGateProjectListFields.ProjectNotes]);
                                    }
                                }
                                previousVersion = version;
                                bFirstTime = false;
                            }
                        }
                    }
                }
                return newItem.ToString();
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "ProjectNotesService", "GetStageGateProjectCommentsHistory", "Exception while getting the Stage Gate project comments history. - itemId:" + itemId);
                return string.Empty;
            }

        }
        public void UpdateStageGateProjectComments(int itemId, string comments)
        {
            try
            {
                if (string.IsNullOrEmpty(comments))
                    return;

                // Get the current user
                SPUser currentUser = SPContext.Current.Web.CurrentUser;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb spWeb = spSite.OpenWeb())
                        {
                            spWeb.AllowUnsafeUpdates = true;
                            SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);

                            SPListItem item = spList.GetItemById(itemId);

                            if (item != null)
                            {
                                if (!string.IsNullOrEmpty(comments))
                                {
                                    item[StageGateProjectListFields.ProjectNotes] = comments;

                                    // Set Modified By to current user NOT System Account
                                    item["Editor"] = SPContext.Current.Web.CurrentUser;

                                    item.Update();
                                }
                            }
                            spWeb.AllowUnsafeUpdates = false;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "ProjectNotesService", "UpdateStageGateProjectComments", "Exception while updating the Stage Gate project comments.");
            }
        }

        public string GetRegulatoryComments(int itemId)
        {
            string comments = string.Empty;
            try
            {
                // Get the current user
                SPUser currentUser = SPContext.Current.Web.CurrentUser;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb spWeb = spSite.OpenWeb())
                        {
                            spWeb.AllowUnsafeUpdates = true;
                            SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
                            SPQuery spQuery = new SPQuery();
                            spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Text\">" + itemId + "</Value></Eq></Where>";

                            SPListItemCollection compassItemCol = spList.GetItems(spQuery);

                            SPListItem item;
                            if (compassItemCol.Count > 0)
                            {
                                var CompassList2Item = compassItemCol[0];
                                comments = Convert.ToString(CompassList2Item[CompassList2Fields.RegulatoryComments]);
                            }

                            spWeb.AllowUnsafeUpdates = false;
                        }
                    }
                });
                return comments;
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "ProjectNotesService", "GetRegulatoryComments", "Exception while reading the regulatory omments.");
                return comments;
            }
        }
        public void UpdateRegulatoryComments(int itemId, string comments)
        {
            try
            {
                if (string.IsNullOrEmpty(comments))
                    return;

                // Get the current user
                SPUser currentUser = SPContext.Current.Web.CurrentUser;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb spWeb = spSite.OpenWeb())
                        {
                            spWeb.AllowUnsafeUpdates = true;
                            SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
                            SPQuery spQuery = new SPQuery();
                            spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Text\">" + itemId + "</Value></Eq></Where>";

                            SPListItemCollection compassItemCol = spList.GetItems(spQuery);

                            SPListItem item;
                            if (compassItemCol.Count > 0)
                            {
                                item = compassItemCol[0];
                            }
                            else
                            {
                                item = spList.AddItem();
                                item[CompassList2Fields.CompassListItemId] = itemId;
                            }

                            item[CompassList2Fields.RegulatoryComments] = comments;

                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;

                            item.Update();
                            spWeb.AllowUnsafeUpdates = false;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "ProjectNotesService", "UpdateRegulatoryComments", "Exception while updating the regulatory omments.");
            }
        }
        #endregion

        #region Private Methods
        private string GetLocalDateTime(string datetime)
        {
            DateTime result;
            try
            {
                if (DateTime.TryParse(datetime, out result))
                {
                    return result.ToLocalTime().ToString();
                }
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "ProjectNotesService", "GetLocalDateTime", string.Concat("Exception while date parsing - datetime:", datetime));
            }

            return datetime;
        }
        #endregion
    }
}
