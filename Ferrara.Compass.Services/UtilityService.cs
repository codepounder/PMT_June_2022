using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;
using Microsoft.SharePoint;
using System.IO;

namespace Ferrara.Compass.Services
{
    public class UtilityService : IUtilityService
    {
        private readonly IExceptionService exceptionService;
        private readonly ICacheManagementService cacheManagementService;
        private static object _lock;

        public UtilityService(IExceptionService exceptionService, ICacheManagementService cacheManagementService)
        {
            this.exceptionService = exceptionService;
            this.cacheManagementService = cacheManagementService;
            _lock = new object();
        }

        #region Project Number Methods
        public int GetStageGateProjectListItemIdFromCompassListItemId(int compassItemId)
        {
            int StageGateProjectListItemId = 0;
            if (compassItemId == 0)
                return StageGateProjectListItemId;

            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem item = spList.GetItemById(compassItemId);
                    if (item != null)
                    {
                        StageGateProjectListItemId = Convert.ToInt32(item[CompassListFields.StageGateProjectListItemId]);
                    }
                }
            }
            return StageGateProjectListItemId;
        }
        public int GetStageGateProjectListItemIdFromProjectNumber(string projectNumber)
        {
            int id = 0;
            if (string.IsNullOrEmpty(projectNumber))
                return id;

            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);
                    var fields = new string[] { StageGateProjectListFields.ProjectNumber };
                    var item = spList.GetItems(fields).Cast<SPListItem>().FirstOrDefault(x => Convert.ToString(x[StageGateProjectListFields.ProjectNumber]).ToLower().Equals(projectNumber.ToLower()));
                    if (item != null)
                    {
                        id = item.ID;
                    }
                }
            }
            return id;
        }
        public int GetItemIdFromProjectNumber(string projectNumber)
        {
            int id = 0;
            if (string.IsNullOrEmpty(projectNumber))
                return id;

            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    var fields = new string[] { CompassListFields.ProjectNumber };
                    var item = spList.GetItems(fields).Cast<SPListItem>().FirstOrDefault(x => Convert.ToString(x[CompassListFields.ProjectNumber]).ToLower().Equals(projectNumber.ToLower()));
                    if (item != null)
                    {
                        id = item.ID;
                    }
                }
            }
            return id;
        }
        public string GetOnHoldWorkFlowPhase(int CompassListItemId)
        {
            string OnHOldWorkFlowPhase = string.Empty;
            if (CompassListItemId == 0)
                return OnHOldWorkFlowPhase;

            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    var item = spList.GetItemById(CompassListItemId);
                    if (item != null)
                    {
                        OnHOldWorkFlowPhase = Convert.ToString(item[CompassListFields.OnHoldWorkflowPhase]);
                    }
                }
            }
            return OnHOldWorkFlowPhase;
        }
        public int GetItemIdByProjectNumberFromStageGateProjectList(string projectNumber)
        {
            int id = 0;
            if (string.IsNullOrEmpty(projectNumber))
                return id;

            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);
                    var fields = new string[] { StageGateProjectListFields.ProjectNumber };
                    var item = spList.GetItems(fields).Cast<SPListItem>().FirstOrDefault(x => Convert.ToString(x[StageGateProjectListFields.ProjectNumber]).ToLower().Equals(projectNumber.ToLower()));
                    if (item != null)
                    {
                        id = item.ID;
                    }
                }
            }
            return id;
        }

        public string GetProjectNumberFromItemId(int compassItemId, string webUrl)
        {
            string projectNo = string.Empty;
            using (SPSite spSite = new SPSite(webUrl))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem item = spList.GetItemById(compassItemId);
                    if (item != null)
                    {
                        projectNo = Convert.ToString(item[CompassListFields.ProjectNumber]);
                    }
                }
            }
            return projectNo;
        }

        public List<string> CheckForDuplicateFinishedGoodProjects(string projectNumber, string sapNumber)
        {
            string projectNum;
            List<string> duplicates = new List<string>();

            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"" + CompassListFields.SAPItemNumber + "\" /><Value Type=\"Text\">" + sapNumber + "</Value></Eq><And><Neq><FieldRef Name=\"WorkflowPhase\" /><Value Type=\"Text\">Completed</Value></Neq><Neq><FieldRef Name=\"WorkflowPhase\" /><Value Type=\"Text\">Cancelled</Value></Neq></And></And></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);

                    if (compassItemCol != null)
                    {
                        projectNumber = CheckChangeRequestProjectNumber(projectNumber);
                        foreach (SPListItem item in compassItemCol)
                        {
                            projectNum = CheckChangeRequestProjectNumber(Convert.ToString(item[CompassListFields.ProjectNumber]));
                            if (!string.Equals(projectNum, projectNumber))
                                duplicates.Add(projectNum + " ");
                        }
                    }
                }
            }
            return duplicates;
        }
        private string CheckChangeRequestProjectNumber(string projectNum)
        {
            char lastChar;
            lastChar = projectNum[projectNum.Length - 1];
            if (lastChar >= 'A' && lastChar <= 'Z')
                projectNum = projectNum.Substring(0, projectNum.Length - 1);
            return projectNum;
        }

        public List<CompassListItem> GetCompassListFromSAPNumber(string sapNumber)
        {
            var compassItems = new List<CompassListItem>();

            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<OrderBy><FieldRef Name=\"ID\" Ascending=\"FALSE\"></FieldRef></OrderBy><Where><Eq><FieldRef Name=\"" + CompassListFields.SAPItemNumber + "\" /><Value Type=\"Text\">" + sapNumber + "</Value></Eq></Where>";
                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            var sgItem = new CompassListItem();
                            sgItem.CompassListItemId = item.ID;
                            sgItem.ProjectNumber = Convert.ToString(item[CompassListFields.ProjectNumber]);
                            sgItem.SAPItemNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                            sgItem.SAPDescription = Convert.ToString(item[CompassListFields.SAPDescription]);
                            compassItems.Add(sgItem);
                        }
                    }
                }
            }
            return compassItems;
        }
        #endregion

        #region Workflow Step Methods
        public string GetCurrentWFStepForProject(int compassItemId)
        {
            string wfStep = string.Empty;
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem item = spList.GetItemById(compassItemId);
                    if (item != null)
                    {
                        wfStep = string.Empty; // Convert.ToString(item[CompassListFields.WORKFLOW_Step]);
                    }
                }
            }
            return wfStep;
        }
        #endregion

        #region Attachment Methods
        public bool UploadAttachment(CompassListItem compassListItem)
        {
            bool isUploaded = false;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spweb = spsite.OpenWeb())
                    {
                        spweb.AllowUnsafeUpdates = true;
                        SPDocumentLibrary documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassLibraryName) as SPDocumentLibrary;
                        var folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", compassListItem.ProjectNumber);
                        if (!spweb.GetFolder(folderUrl).Exists)
                        {
                            SPListItem projectFolder = documentLib.Items.Add("", SPFileSystemObjectType.Folder, compassListItem.ProjectNumber);
                            projectFolder.Update();
                        }
                        SPFolder stagGateDocLibrary = spweb.GetFolder(folderUrl);
                        foreach (var file in compassListItem.FileAttachments)
                        {
                            try
                            {
                                SPFile spfile = null;
                                if (file.FileStream != null)
                                {
                                    spfile = stagGateDocLibrary.Files.Add(file.FileName, file.FileStream, true);
                                }
                                else if (file.FileContent != null)
                                {
                                    spfile = stagGateDocLibrary.Files.Add(file.FileName, file.FileContent, true);
                                }
                                else if (file.FileContentLength > 0)
                                {
                                    byte[] bytes = new byte[file.FileContentLength];
                                    spfile = stagGateDocLibrary.Files.Add(file.FileName, bytes, true);
                                }

                                spfile.Item[CompassListFields.DOCLIBRARY_CompassDocType] = file.DocType;
                                spfile.Item[CompassListFields.Title] = file.FileName;
                                spfile.Item.Update();
                                isUploaded = true;
                            }
                            catch (Exception ex)
                            {
                                exceptionService.Handle(LogCategory.CriticalError, ex, "UtilityService", "UploadAttachment(StageGateListItem stageGateListItem)", compassListItem.ProjectNumber);
                            }
                        }
                        stagGateDocLibrary.Update();
                        spweb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return isUploaded;
        }
        public bool UploadAttachment(WorldSyncRequestItem request)
        {
            SPDocumentLibrary documentLib;
            SPListItem projectFolder;
            SPFolder stagGateDocLibrary;
            SPFile spfile;
            byte[] bytes;
            bool isUploaded = false;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spweb = spsite.OpenWeb())
                    {
                        spweb.AllowUnsafeUpdates = true;
                        documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_WorldSyncRequestName) as SPDocumentLibrary;
                        var folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", request.SAPnumber);
                        if (!spweb.GetFolder(folderUrl).Exists)
                        {
                            projectFolder = documentLib.Items.Add("", SPFileSystemObjectType.Folder, request.SAPnumber);
                            projectFolder.Update();
                        }
                        stagGateDocLibrary = spweb.GetFolder(folderUrl);
                        if (request.FileAttachment != null)
                        {
                            try
                            {
                                spfile = null;
                                if (request.FileAttachment.FileStream != null)
                                {
                                    spfile = stagGateDocLibrary.Files.Add(request.FileAttachment.FileName, request.FileAttachment.FileStream, true);
                                }
                                else if (request.FileAttachment.FileContent != null)
                                {
                                    spfile = stagGateDocLibrary.Files.Add(request.FileAttachment.FileName, request.FileAttachment.FileContent, true);
                                }
                                else if (request.FileAttachment.FileContentLength > 0)
                                {
                                    bytes = new byte[request.FileAttachment.FileContentLength];
                                    spfile = stagGateDocLibrary.Files.Add(request.FileAttachment.FileName, bytes, true);
                                }
                                spfile.Item[WorldSyncRequestFields.DOCLIBRARY_RequestId] = request.RequestId;
                                spfile.Item[WorldSyncRequestFields.RequestType] = request.FileAttachment.DocType;
                                spfile.Item[WorldSyncRequestFields.DOCLIBRARY_DisplayFileName] = request.FileAttachment.FileName;
                                spfile.Item["Title"] = request.FileAttachment.FileName;
                                spfile.Item.Update();
                                isUploaded = true;
                            }
                            catch (Exception ex)
                            {
                                exceptionService.Handle(LogCategory.CriticalError, ex, "UtilityService", "UploadAttachment(StageGateListItem stageGateListItem)", request.SAPnumber);
                            }
                        }
                        stagGateDocLibrary.Update();
                        spweb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return isUploaded;
        }
        public bool UploadAttachment(List<WorldSyncFuseFileItem> requests)
        {
            SPDocumentLibrary documentLib;
            SPListItem projectFolder;
            SPFolder WorldSyncFUSELibrary;
            SPFile spfile;
            byte[] bytes;
            bool isUploaded = false;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spweb = spsite.OpenWeb())
                    {
                        spweb.AllowUnsafeUpdates = true;
                        documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_WorldSyncFUSELibraryName) as SPDocumentLibrary;
                        var folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", DateTime.Now.ToString("MM-dd-yyyy"));
                        if (!spweb.GetFolder(folderUrl).Exists)
                        {
                            projectFolder = documentLib.Items.Add("", SPFileSystemObjectType.Folder, DateTime.Now.ToString("MM-dd-yyyy"));
                            projectFolder.Update();
                        }
                        WorldSyncFUSELibrary = spweb.GetFolder(folderUrl);
                        foreach (WorldSyncFuseFileItem request in requests)
                        {
                            if (request.FileAttachment != null)
                            {
                                try
                                {
                                    spfile = null;
                                    if (request.FileAttachment.FileStream != null)
                                    {
                                        spfile = WorldSyncFUSELibrary.Files.Add(request.FileAttachment.FileName, request.FileAttachment.FileContent, null, SPContext.Current.Web.CurrentUser, SPContext.Current.Web.CurrentUser, DateTime.Now, DateTime.Now, true);
                                    }
                                    else if (request.FileAttachment.FileContent != null)
                                    {
                                        spfile = WorldSyncFUSELibrary.Files.Add(request.FileAttachment.FileName, request.FileAttachment.FileContent, true);
                                    }
                                    else if (request.FileAttachment.FileContentLength > 0)
                                    {
                                        bytes = new byte[request.FileAttachment.FileContentLength];
                                        spfile = WorldSyncFUSELibrary.Files.Add(request.FileAttachment.FileName, bytes, true);
                                    }
                                    spfile.Item[WorldSyncFuseFileFields.DOCLIBRARY_RequestId] = request.RequestId;
                                    spfile.Item[WorldSyncFuseFileFields.RequestType] = request.FileAttachment.DocType;
                                    spfile.Item[WorldSyncFuseFileFields.DOCLIBRARY_DisplayFileName] = request.FileAttachment.FileName;
                                    spfile.Item["Title"] = request.FileAttachment.FileName;
                                    spfile.Item.Update();
                                    isUploaded = true;
                                }
                                catch (Exception ex)
                                {
                                    exceptionService.Handle(LogCategory.CriticalError, ex, "UtilityService", "UploadAttachment(WorldSyncFuseFileItem request)");
                                }
                            }
                        }
                        WorldSyncFUSELibrary.Update();
                        spweb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return isUploaded;
        }
        public bool UploadCompassAttachment(List<FileAttribute> compassAttachments, string projectNumber)
        {
            bool isUploaded = false;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spweb = spsite.OpenWeb())
                    {
                        spweb.AllowUnsafeUpdates = true;
                        SPDocumentLibrary documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassLibraryName) as SPDocumentLibrary;
                        var folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", projectNumber);
                        if (!spweb.GetFolder(folderUrl).Exists)
                        {
                            SPListItem projectFolder = documentLib.Items.Add("", SPFileSystemObjectType.Folder, projectNumber);
                            projectFolder.Update();
                        }
                        SPFolder stagGateDocLibrary = spweb.GetFolder(folderUrl);
                        foreach (var file in compassAttachments)
                        {
                            try
                            {
                                SPFile spfile = null;
                                if (file.FileStream != null)
                                {
                                    spfile = stagGateDocLibrary.Files.Add(file.FileName, file.FileStream, true);
                                }
                                else if (file.FileContent != null)
                                {
                                    spfile = stagGateDocLibrary.Files.Add(file.FileName, file.FileContent, true);
                                }
                                else if (file.FileContentLength > 0)
                                {
                                    byte[] bytes = new byte[file.FileContentLength];
                                    spfile = stagGateDocLibrary.Files.Add(file.FileName, bytes, true);
                                }

                                spfile.Item[CompassListFields.DOCLIBRARY_CompassDocType] = file.DocType;
                                spfile.Item[CompassListFields.Title] = file.FileName;
                                spfile.Item.Update();
                                isUploaded = true;
                            }
                            catch (Exception ex)
                            {
                                exceptionService.Handle(LogCategory.CriticalError, ex, "UtilityService", "UploadCompassAttachment", projectNumber);
                            }
                        }
                        stagGateDocLibrary.Update();
                        spweb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return isUploaded;
        }
        public bool UploadPackagingAttachment(List<FileAttribute> fileList, string projectNo, int id)
        {
            bool isUploaded = false;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spweb = spsite.OpenWeb())
                    {
                        spweb.AllowUnsafeUpdates = true;
                        SPDocumentLibrary documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassLibraryName) as SPDocumentLibrary;
                        var folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", projectNo);
                        if (!spweb.GetFolder(folderUrl).Exists)
                        {
                            SPListItem projectFolder = documentLib.Items.Add("", SPFileSystemObjectType.Folder, projectNo);
                            projectFolder.Update();
                        }
                        SPFolder stagGateDocLibrary = spweb.GetFolder(folderUrl);
                        foreach (var file in fileList)
                        {
                            try
                            {
                                string fileName = id.ToString() + "_" + file.FileName;
                                SPFile spfile = stagGateDocLibrary.Files.Add(fileName, file.FileContent, true);
                                spfile.Item[CompassListFields.DOCLIBRARY_CompassDocType] = file.DocType;
                                spfile.Item[CompassListFields.DOCLIBRARY_PackagingComponentItemId] = id;
                                spfile.Item[CompassListFields.DOCLIBRARY_DisplayFileName] = file.FileName;
                                spfile.Item[CompassListFields.Title] = file.FileName;
                                spfile.Item.Update();
                                isUploaded = true;
                            }
                            catch (Exception ex)
                            {
                                exceptionService.Handle(LogCategory.CriticalError, ex, "UtilityService", "UploadAttachment(List<FileAttribute> fileList, string projectNo, int id)", projectNo);
                            }
                        }
                        stagGateDocLibrary.Update();
                        spweb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return isUploaded;
        }
        public bool UploadAttachment(InnovationListItem item)
        {
            bool isUploaded = false;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spweb = spsite.OpenWeb())
                    {
                        spweb.AllowUnsafeUpdates = true;
                        SPDocumentLibrary documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_InnovationLibraryName) as SPDocumentLibrary;
                        var folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", item.InnovationListItemId.ToString());
                        if (!spweb.GetFolder(folderUrl).Exists)
                        {
                            SPListItem projectFolder = documentLib.Items.Add("", SPFileSystemObjectType.Folder, item.InnovationListItemId.ToString());
                            projectFolder.Update();
                        }
                        SPFolder folder = spweb.GetFolder(folderUrl);
                        foreach (var file in item.FileAttachments)
                        {
                            try
                            {
                                SPFile spfile = null;
                                if (file.FileStream != null)
                                {
                                    spfile = folder.Files.Add(file.FileName, file.FileStream, true);
                                }
                                else if (file.FileContent != null)
                                {
                                    spfile = folder.Files.Add(file.FileName, file.FileContent, true);
                                }
                                else if (file.FileContentLength > 0)
                                {
                                    byte[] bytes = new byte[file.FileContentLength];
                                    spfile = folder.Files.Add(file.FileName, bytes, true);
                                }

                                spfile.Item[InnovationListFields.DOCLIBRARY_InnovationDocType] = file.DocType;
                                spfile.Item[InnovationListFields.DOCLIBRARY_InnovationItemId] = item.InnovationListItemId;
                                spfile.Item["Title"] = file.FileName;
                                spfile.Item.Update();
                                isUploaded = true;
                            }
                            catch (Exception ex)
                            {
                                exceptionService.Handle(LogCategory.CriticalError, ex, "UtilityService", "UploadAttachment(InnovationListItem item)", item.InnovationListItemId.ToString());
                            }
                        }
                        folder.Update();
                        spweb.AllowUnsafeUpdates = false;
                    }
                }
            });

            return isUploaded;
        }
        public bool UploadImportAttachment(FileAttribute file, string webUrl)
        {
            bool isUploaded = false;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spsite = new SPSite(webUrl))
                {
                    using (SPWeb spweb = spsite.OpenWeb())
                    {
                        spweb.AllowUnsafeUpdates = true;
                        SPDocumentLibrary documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassUploadsLibraryName) as SPDocumentLibrary;
                        try
                        {
                            SPFile spfile = documentLib.RootFolder.Files.Add(file.FileName, file.FileStream, true);
                            spfile.Item.Update();
                            isUploaded = true;
                        }
                        catch (Exception ex)
                        {
                            exceptionService.Handle(LogCategory.CriticalError, ex, "UtilityService", "UploadAttachment(FileAttribute file, string webUrl)");
                        }
                        spweb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return isUploaded;
        }
        public void CopyFiles(string sourceFolder, string destinationFolder)
        {
            string folderUrl = string.Empty;
            string destUrl = string.Empty;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                List<SPFile> spfiles;
                using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spweb = spsite.OpenWeb())
                    {
                        try
                        {
                            spweb.AllowUnsafeUpdates = true;
                            SPDocumentLibrary documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassLibraryName) as SPDocumentLibrary;
                            folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", sourceFolder);
                            destUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", destinationFolder);
                            if (!spweb.GetFolder(destUrl).Exists)
                            {
                                SPListItem projectFolder = documentLib.Items.Add("", SPFileSystemObjectType.Folder, destinationFolder);
                                projectFolder.Update();
                            }

                            if (spweb.GetFolder(folderUrl).Exists)
                            {
                                SPFolder stageGateProjectFolder = spweb.GetFolder(folderUrl);
                                spfiles = stageGateProjectFolder.Files.OfType<SPFile>().Where(x => x.Item[CompassListFields.DOCLIBRARY_PackagingComponentItemId] == null).ToList();
                                foreach (SPFile spfile in spfiles)
                                    spfile.CopyTo(string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", destinationFolder, "/", spfile.Name), true);
                            }
                            spweb.AllowUnsafeUpdates = false;
                        }
                        catch (Exception ex)
                        {
                            exceptionService.Handle(LogCategory.CriticalError, ex, "UtilityService", "CopyFiles", string.Concat("Source: ", folderUrl, " Destination: ", destUrl));
                        }
                    }
                }
            });
        }
        public void UpdatePackagingComponentIdForFiles(string destinationFolder, int oldId, int newId)
        {
            using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spweb = spsite.OpenWeb())
                {
                    SPDocumentLibrary documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassLibraryName) as SPDocumentLibrary;
                    var folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", destinationFolder);
                    int currentId;
                    if (spweb.GetFolder(folderUrl).Exists)
                    {
                        SPFolder stageGateProjectFolder = spweb.GetFolder(folderUrl);
                        foreach (SPFile spfile in stageGateProjectFolder.Files)
                        {
                            currentId = Convert.ToInt32(spfile.Item["PackagingComponentItemId"]);
                            if (currentId == oldId)
                            {
                                spfile.Item["PackagingComponentItemId"] = newId.ToString();
                                spfile.Item.Update();
                            }
                        }
                    }
                }
            }
        }
        public List<FileAttribute> GetUploadedFiles(string projectNo)
        {
            List<FileAttribute> files = new List<FileAttribute>();
            using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spweb = spsite.OpenWeb())
                {
                    SPDocumentLibrary documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassLibraryName) as SPDocumentLibrary;
                    var folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", projectNo);
                    if (spweb.GetFolder(folderUrl).Exists)
                    {
                        SPFolder stageGateProjectFolder = spweb.GetFolder(folderUrl);
                        foreach (SPFile spfile in stageGateProjectFolder.Files)
                        {
                            var file = new FileAttribute();
                            if (string.IsNullOrEmpty(spfile.Title))
                            {
                                file.FileName = spfile.Name;
                            }
                            else
                            {
                                file.FileName = spfile.Title;
                            }
                            string fileurl = spfile.Url;
                            fileurl = fileurl.Replace("’", "%27");
                            fileurl = fileurl.Replace("'", "%27");
                            file.FileUrl = string.Concat(spweb.Url, "/", fileurl);
                            file.DocType = Convert.ToString(spfile.Item[CompassListFields.DOCLIBRARY_CompassDocType]);
                            try
                            {
                                var PackagingComponentItemId = 0;
                                if (!string.IsNullOrEmpty(Convert.ToString(spfile.Item[CompassListFields.DOCLIBRARY_PackagingComponentItemId])))
                                {
                                    PackagingComponentItemId = Convert.ToInt32(spfile.Item[CompassListFields.DOCLIBRARY_PackagingComponentItemId]);
                                }
                                file.PackagingComponentItemId = PackagingComponentItemId;
                            }
                            catch (Exception e)
                            {
                            }

                            files.Add(file);
                        }
                    }
                }
            }
            return files;
        }

        public List<FileAttribute> GetUploadedFuseFilesForLastSevenDays()
        {
            var FuseFiles = new List<FileAttribute>();

            DateTime[] last7Days = Enumerable.Range(0, 7)
                                    .Select(i => DateTime.Now.Date.AddDays(-i))
                                    .ToArray();

            foreach (var day in last7Days)
            {
                var folderName = string.Concat(day.Month.ToString("00"), "-", day.Day.ToString("00"), "-", day.Year.ToString());
                FuseFiles.AddRange(GetUploadedFuseFilesByFolder(folderName));
            }

            return FuseFiles.OrderByDescending(d => d.ModifiedDateTime).ToList();
        }
        public List<FileAttribute> GetUploadedCompassFilesByDocType(string projectNo, string docType)
        {
            return GetUploadedFilesByDocType(GlobalConstants.DOCLIBRARY_CompassLibraryName, CompassListFields.DOCLIBRARY_CompassDocType, projectNo, docType);
        }
        public List<FileAttribute> GetUploadedWorldSyncReqFilesByDocType(string SAPnumber, string docType)
        {
            return GetUploadedFilesByDocType(GlobalConstants.DOCLIBRARY_WorldSyncRequestName, WorldSyncRequestFields.DOCLIBRARY_RequestType, SAPnumber, docType);
        }
        public void DeleteWorldSyncReqFilesByDocType(string SAPnumber, string docType)
        {
            DeleteAttachment(GlobalConstants.DOCLIBRARY_WorldSyncRequestName, WorldSyncRequestFields.DOCLIBRARY_RequestType, SAPnumber, docType);
        }
        public List<FileAttribute> GetUploadedFilesByDocType(string docLibrary, string docTypeField, string folder, string docType)
        {
            List<FileAttribute> files = new List<FileAttribute>();
            using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spweb = spsite.OpenWeb())
                {
                    SPDocumentLibrary documentLib = spweb.Lists.TryGetList(docLibrary) as SPDocumentLibrary;
                    var folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", folder);
                    if (spweb.GetFolder(folderUrl).Exists)
                    {
                        SPFolder stageGateProjectFolder = spweb.GetFolder(folderUrl);
                        try
                        {
                            var spFilesNullFilter = stageGateProjectFolder.Files.OfType<SPFile>().Where(x => x.Item[docTypeField] != null).ToList();
                            var spFiles = spFilesNullFilter.Where(x => x.Item[docTypeField].Equals(docType)).ToList();
                            foreach (SPFile spfile in spFiles)
                            {
                                var file = new FileAttribute();
                                if (string.IsNullOrEmpty(spfile.Title))
                                {
                                    file.FileName = spfile.Name;
                                }
                                else
                                {
                                    file.FileName = spfile.Title;
                                }
                                string fileurl = spfile.Url;
                                fileurl = fileurl.Replace("’", "%27");
                                fileurl = fileurl.Replace("'", "%27");
                                file.FileUrl = string.Concat(spweb.Url, "/", fileurl);
                                file.DocType = docType;
                                files.Add(file);
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptionService.Handle(LogCategory.CriticalError, ex, "UtilityService", "GetUploadedFilesByDocType(string docLibrary, string folder, string docType)", string.Concat(folder, "-", docType));
                        }
                    }
                }
            }
            return files;
        }
        public List<FileAttribute> GetUploadedFuseFilesByFolder(string folder)
        {
            List<FileAttribute> files = new List<FileAttribute>();
            using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spweb = spsite.OpenWeb())
                {
                    SPDocumentLibrary documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_WorldSyncFUSELibraryName) as SPDocumentLibrary;
                    var folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", folder);
                    if (spweb.GetFolder(folderUrl).Exists)
                    {
                        SPFolder stageGateProjectFolder = spweb.GetFolder(folderUrl);
                        try
                        {
                            var spFiles = stageGateProjectFolder.Files.OfType<SPFile>().ToList();
                            foreach (SPFile spfile in spFiles)
                            {
                                var file = new FileAttribute();
                                if (string.IsNullOrEmpty(spfile.Title))
                                {
                                    file.FileName = spfile.Name;
                                }
                                else
                                {
                                    file.FileName = spfile.Title;
                                }
                                string fileurl = spfile.Url;
                                fileurl = fileurl.Replace("’", "%27");
                                fileurl = fileurl.Replace("'", "%27");
                                file.FileUrl = string.Concat(spweb.Url, "/", fileurl);
                                file.ModifiedDateTime = spfile.TimeLastModified;
                                files.Add(file);
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptionService.Handle(LogCategory.CriticalError, ex, "UtilityService", "GetUploadedFuseFilesByFolder(string folder)", string.Concat(folder));
                        }
                    }
                }
            }
            return files;
        }
        public List<FileAttribute> GetUploadedFilesByDocTypeAndID(string projectNo, string docType, string id)
        {
            List<FileAttribute> files = new List<FileAttribute>();
            using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spweb = spsite.OpenWeb())
                {
                    SPDocumentLibrary documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassLibraryName) as SPDocumentLibrary;
                    var folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", projectNo);
                    if (spweb.GetFolder(folderUrl).Exists)
                    {
                        SPFolder stageGateProjectFolder = spweb.GetFolder(folderUrl);
                        try
                        {
                            var spFiles = stageGateProjectFolder.Files.OfType<SPFile>().Where(x => Convert.ToInt32(x.Item[CompassListFields.DOCLIBRARY_PackagingComponentItemId]).Equals(id) && x.Item[CompassListFields.DOCLIBRARY_CompassDocType].Equals(docType)).ToList();

                            foreach (SPFile spfile in spFiles)
                            {
                                var file = new FileAttribute();
                                if (string.IsNullOrEmpty(spfile.Title))
                                {
                                    file.FileName = spfile.Name;
                                }
                                else
                                {
                                    file.FileName = spfile.Title;
                                }
                                string fileurl = spfile.Url;
                                fileurl = fileurl.Replace("’", "%27");
                                fileurl = fileurl.Replace("'", "%27");
                                file.FileUrl = string.Concat(spweb.Url, "/", fileurl);
                                file.DocType = docType;
                                files.Add(file);
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptionService.Handle(LogCategory.CriticalError, ex, "UtilityService", "GetUploadedFilesByDocType(string projectNo, string docType)", string.Concat(projectNo, "-", docType));
                        }
                    }
                }
            }
            return files;
        }
        public List<FileAttribute> GetUploadedFiles(string projectNo, int packagingItemId, string docType, string webUrl)
        {
            List<FileAttribute> files = new List<FileAttribute>();
            using (SPSite spsite = new SPSite(webUrl))
            {
                using (SPWeb spweb = spsite.OpenWeb())
                {
                    SPDocumentLibrary documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassLibraryName) as SPDocumentLibrary;
                    string folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", projectNo);
                    if (spweb.GetFolder(folderUrl).Exists)
                    {
                        SPFolder stageGateProjectFolder = spweb.GetFolder(folderUrl);
                        var spfiles = stageGateProjectFolder.Files.OfType<SPFile>().Where(x => Convert.ToInt32(x.Item[CompassListFields.DOCLIBRARY_PackagingComponentItemId]).Equals(packagingItemId) && x.Item[CompassListFields.DOCLIBRARY_CompassDocType].Equals(docType)).ToList();
                        if (spfiles.Count > 0)
                        {
                            foreach (SPFile spfile in spfiles)
                            {
                                FileAttribute file = new FileAttribute();
                                if (string.IsNullOrEmpty(spfile.Title))
                                {
                                    file.FileName = spfile.Name;
                                }
                                else
                                {
                                    file.FileName = spfile.Title;
                                }
                                string fileurl = spfile.Url;
                                fileurl = fileurl.Replace("’", "%27");
                                fileurl = fileurl.Replace("'", "%27");
                                file.FileUrl = string.Concat(spweb.Url, "/", fileurl);

                                file.DocType = docType;
                                file.DisplayFileName = Convert.ToString(spfile.Item[CompassListFields.DOCLIBRARY_DisplayFileName]);

                                files.Add(file);
                            }
                        }
                    }
                }
            }
            return files;
        }
        public List<FileAttribute> GetUploadedFilesByDocTypeMaterialNumber(string projectNo, string docType, string materialNumber)
        {
            List<FileAttribute> files = new List<FileAttribute>();
            using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spweb = spsite.OpenWeb())
                {
                    SPDocumentLibrary documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassLibraryName) as SPDocumentLibrary;
                    var folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", projectNo);
                    if (spweb.GetFolder(folderUrl).Exists)
                    {
                        SPFolder stageGateProjectFolder = spweb.GetFolder(folderUrl);
                        try
                        {
                            var spFiles = stageGateProjectFolder.Files.OfType<SPFile>().Where(x => x.Item[CompassListFields.DOCLIBRARY_CompassDocType].Equals(docType));

                            foreach (SPFile spfile in spFiles)
                            {
                                var file = new FileAttribute();
                                if (string.IsNullOrEmpty(spfile.Title))
                                {
                                    file.FileName = spfile.Name;
                                }
                                else
                                {
                                    file.FileName = spfile.Title;
                                }
                                string fileurl = spfile.Url;
                                fileurl = fileurl.Replace("’", "%27");
                                fileurl = fileurl.Replace("'", "%27");
                                file.FileUrl = string.Concat(spweb.Url, "/", fileurl);

                                file.DocType = docType;
                                if (file.FileName.Contains(materialNumber))
                                    files.Add(file);
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptionService.Handle(LogCategory.CriticalError, ex, "UtilityService", "GetUploadedFilesByDocTypeMaterialNumber(string projectNo, string docType, string materialNumber)", string.Concat(projectNo, "-", docType, "-", materialNumber));
                        }
                    }
                }
            }
            return files;
        }
        public List<FileAttribute> GetUploadedInnovationFilesByDocType(int innovationListItemId, string docType)
        {
            List<FileAttribute> files = new List<FileAttribute>();
            using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spweb = spsite.OpenWeb())
                {
                    SPDocumentLibrary documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_InnovationLibraryName) as SPDocumentLibrary;
                    var folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", innovationListItemId.ToString());
                    if (spweb.GetFolder(folderUrl).Exists)
                    {
                        SPFolder folder = spweb.GetFolder(folderUrl);
                        try
                        {
                            var spFiles = folder.Files.OfType<SPFile>().Where(x => x.Item[InnovationListFields.DOCLIBRARY_InnovationDocType].Equals(docType));

                            foreach (SPFile spfile in spFiles)
                            {
                                var file = new FileAttribute();
                                if (string.IsNullOrEmpty(spfile.Title))
                                {
                                    file.FileName = spfile.Name;
                                }
                                else
                                {
                                    file.FileName = spfile.Title;
                                }
                                string fileurl = spfile.Url;
                                fileurl = fileurl.Replace("’", "%27");
                                fileurl = fileurl.Replace("'", "%27");
                                file.FileUrl = string.Concat(spweb.Url, "/", fileurl);

                                file.DocType = docType;
                                files.Add(file);
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptionService.Handle(LogCategory.CriticalError, ex, "UtilityService", "GetUploadedInnovationFilesByDocType", string.Concat(innovationListItemId.ToString(), "-", docType));
                        }
                    }
                }
            }
            return files;
        }
        public void DeleteAttachment(string fileUrl)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spweb = spsite.OpenWeb())
                    {
                        spweb.AllowUnsafeUpdates = true;
                        SPFile file = spweb.GetFile(fileUrl);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                        spweb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void DeleteAttachment(string fileUrl, string webUrl)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spsite = new SPSite(webUrl))
                {
                    using (SPWeb spweb = spsite.OpenWeb())
                    {
                        spweb.AllowUnsafeUpdates = true;
                        SPFile file = spweb.GetFile(fileUrl);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                        spweb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void DeleteAttachment(string docLibrary, string docTypeField, string folder, string docType)
        {
            SPDocumentLibrary documentLib;
            SPFolder stageGateProjectFolder;
            IEnumerable<SPFile> spFiles;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spweb = spsite.OpenWeb())
                    {
                        spweb.AllowUnsafeUpdates = true;

                        documentLib = spweb.Lists.TryGetList(docLibrary) as SPDocumentLibrary;
                        var folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", folder);
                        if (spweb.GetFolder(folderUrl).Exists)
                        {
                            stageGateProjectFolder = spweb.GetFolder(folderUrl);
                            try
                            {
                                spFiles = stageGateProjectFolder.Files.OfType<SPFile>().Where(x => x.Item[docTypeField].Equals(docType));
                                foreach (SPFile spfile in spFiles)
                                {
                                    spfile.Delete();
                                }
                            }
                            catch (Exception ex)
                            {
                                exceptionService.Handle(LogCategory.CriticalError, ex, "UtilityService", "DeleteAttachment(string docLibrary, string docTypeField, string folder, string docType)", string.Concat(docLibrary, "-", docTypeField, "-", folder, "-", docType));
                            }
                        }
                        spweb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public string CreateSafeFileName(string filename)
        {
            filename = filename.Replace("#", "");
            filename = filename.Replace("%", "");
            filename = filename.Replace("&", "");
            filename = filename.Replace("*", "");
            filename = filename.Replace(":", "");
            filename = filename.Replace("<", "");
            filename = filename.Replace(">", "");
            filename = filename.Replace("?", "");
            filename = filename.Replace("/", "");
            filename = filename.Replace("{", "");
            filename = filename.Replace("|", "");
            filename = filename.Replace("}", "");
            filename = filename.Replace('"', ' ');
            filename = filename.Replace(".", "");
            filename = filename.Replace("~", "");
            filename = filename.Replace("\\", "");

            filename = filename.Replace(" ", "");

            return filename;
        }
        #endregion

        #region Workflow Steps List
        public List<WFStepField> GetWorkflowSteps()
        {
            List<WFStepField> values = cacheManagementService.GetFromCache<List<WFStepField>>(CacheKeys.WFStepList);
            if (values == null)
            {
                values = new List<WFStepField>();
                lock (_lock)
                {
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb oWeb = site.OpenWeb())
                        {
                            SPList oList = oWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTaskAssignmentListName);
                            if (oList.Items.Count > 0)
                            {
                                foreach (SPListItem item in oList.Items)
                                {
                                    WFStepField field = new WFStepField();
                                    field.Title = Convert.ToString(item[TaskAssignmentFieldName.WorkflowStep]);
                                    //field.Sequence = Convert.ToInt32(item[WFStepFieldName.Sequence]);
                                    //field.WorkflowStep = (WorkflowStep)Enum.Parse(typeof(WorkflowStep), item[TaskAssignmentFieldName.WorkflowStep].ToString());
                                    //field.FormName = Convert.ToString(item[TaskAssignmentFieldName.FormName]);
                                    field.PageName = DeterminePageName(field.FormName);
                                    //var groups = Convert.ToString(item[WFStepFieldName.AccessGroups]);
                                    //if (!string.IsNullOrEmpty(groups))
                                    //{
                                    //    field.AccessGroups = groups.Split(',').ToList();
                                    //}
                                    var groups = Convert.ToString(item[TaskAssignmentFieldName.EmailGroups]);
                                    if (!string.IsNullOrEmpty(groups))
                                    {
                                        field.EmailGroups = groups.Split(',').ToList();
                                    }
                                    groups = Convert.ToString(item[TaskAssignmentFieldName.TaskGroups]);
                                    if (!string.IsNullOrEmpty(groups))
                                    {
                                        field.EditGroups = groups.Split(',').ToList();
                                    }
                                    field.WorkflowStepDesc = Convert.ToString(item[TaskAssignmentFieldName.TaskTitle]);
                                    field.WorkflowStepTaskDesc = Convert.ToString(item[TaskAssignmentFieldName.TaskDescription]);

                                    values.Add(field);
                                }
                                //values = values.OrderBy(x => x.Sequence).ToList();
                            }
                            cacheManagementService.AddToCache<List<WFStepField>>(CacheKeys.WFStepList, values, new TimeSpan(240, 0, 0));
                        }
                    }
                }
            }
            return values;
        }

        private string DeterminePageName(string form)
        {
            if (string.Equals(form, CompassForm.BOMSetupPE.ToString()))
                return GlobalConstants.PAGE_BillofMaterialSetUpPE;
            else if (string.Equals(form, CompassForm.BOMSetupPE2.ToString()))
                return GlobalConstants.PAGE_BillofMaterialSetUpPE2;
            else if (string.Equals(form, CompassForm.BOMSetupProc.ToString()))
                return GlobalConstants.PAGE_BillofMaterialSetUpProc;
            //else if (string.Equals(form, CompassForm.OBMREV2.ToString()))
            //    return GlobalConstants.PAGE_GraphicsDevelopment;
            //else if (string.Equals(form, CompassForm.FCOST.ToString()))
            //    return GlobalConstants.PAGE_FinalCosting;
            else if (string.Equals(form, CompassForm.IPF.ToString()))
                return GlobalConstants.PAGE_ItemProposal;
            else if (string.Equals(form, CompassForm.Operations.ToString()))
                return GlobalConstants.PAGE_OPS;
            else if (string.Equals(form, CompassForm.QA.ToString()))
                return GlobalConstants.PAGE_QA;
            else if (string.Equals(form, CompassForm.SAPInitialSetup.ToString()))
                return GlobalConstants.PAGE_SAPInitialItemSetup;
            else if (string.Equals(form, CompassForm.PrelimSAPInitialSetup.ToString()))
                return GlobalConstants.PAGE_PrelimSAPInitialItemSetup;
            else if (string.Equals(form, CompassForm.ExternalMfg.ToString()))
                return GlobalConstants.PAGE_ExternalManufacturing;
            //else if (string.Equals(form, CompassForm.GRAPH.ToString()))
            //    return GlobalConstants.PAGE_GraphicsSummaryRequest;
            //else if (string.Equals(form, CompassForm.InitialCapacity.ToString()))
            //    return GlobalConstants.PAGE_InitialCapacityReview;
            //else if (string.Equals(form, CompassForm.InitialCosting.ToString()))
            //return GlobalConstants.PAGE_InitialCostingReview;
            else if (string.Equals(form, CompassForm.SrOBMApproval.ToString()))
                return GlobalConstants.PAGE_InitialApprovalReview;
            else if (string.Equals(form, CompassForm.OBMReview1.ToString()))
                return GlobalConstants.PAGE_OBMFirstReview;
            else if (string.Equals(form, CompassForm.OBMReview3.ToString()))
                return GlobalConstants.PAGE_ProjectStatus;
            else if (string.Equals(form, CompassForm.OBMReview2.ToString()))
                return GlobalConstants.PAGE_OBMSecondReview;

            return string.Empty;
        }

        public string GetWorkflowStepDescription(WorkflowStep wfStep)
        {
            // Get the list of all Workflow Steps
            var wfAllSteps = GetWorkflowSteps();
            var currentWfStep = wfAllSteps.FirstOrDefault(x => x.WorkflowStep.Equals(wfStep));
            if (currentWfStep != null)
            {
                return currentWfStep.WorkflowStepDesc;
            }

            return string.Empty;
        }

        public string GetWorkflowStepTaskDescription(WorkflowStep wfStep)
        {
            // Get the list of all Workflow Steps
            var wfAllSteps = GetWorkflowSteps();
            var currentWfStep = wfAllSteps.FirstOrDefault(x => x.WorkflowStep.Equals(wfStep));
            if (currentWfStep != null)
            {
                return currentWfStep.WorkflowStepTaskDesc;
            }

            return string.Empty;
        }
        #endregion

        #region Form Access List
        public List<FormAccessItem> GetFormAccessList()
        {
            List<FormAccessItem> values = cacheManagementService.GetFromCache<List<FormAccessItem>>(CacheKeys.FormAccessList);
            if (values == null)
            {
                values = new List<FormAccessItem>();
                lock (_lock)
                {
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb oWeb = site.OpenWeb())
                        {
                            SPList oList = oWeb.Lists.TryGetList(GlobalConstants.LIST_FormAccessListName);
                            if (oList.Items.Count > 0)
                            {
                                foreach (SPListItem item in oList.Items)
                                {
                                    FormAccessItem field = new FormAccessItem();
                                    field.Title = Convert.ToString(item[FormAccessListFields.Title]);
                                    var groups = Convert.ToString(item[FormAccessListFields.AccessGroups]);
                                    if (!string.IsNullOrEmpty(groups))
                                    {
                                        field.AccessGroups = groups.Split(',').ToList();
                                    }
                                    groups = Convert.ToString(item[FormAccessListFields.EditGroups]);
                                    if (!string.IsNullOrEmpty(groups))
                                    {
                                        field.EditGroups = groups.Split(',').ToList();
                                    }
                                    values.Add(field);
                                }
                            }
                            cacheManagementService.AddToCache<List<FormAccessItem>>(CacheKeys.FormAccessList, values, new TimeSpan(240, 0, 0));
                        }
                    }
                }
            }
            return values;
        }

        public List<FormAccessItem> GetFormAccessList(string url)
        {
            List<FormAccessItem> values = cacheManagementService.GetFromCache<List<FormAccessItem>>(CacheKeys.FormAccessList);
            if (values == null)
            {
                values = new List<FormAccessItem>();
                lock (_lock)
                {
                    using (SPSite site = new SPSite(url))
                    {
                        using (SPWeb oWeb = site.OpenWeb())
                        {
                            SPList oList = oWeb.Lists.TryGetList(GlobalConstants.LIST_FormAccessListName);
                            if (oList.Items.Count > 0)
                            {
                                foreach (SPListItem item in oList.Items)
                                {
                                    FormAccessItem field = new FormAccessItem();
                                    field.Title = Convert.ToString(item[FormAccessListFields.Title]);
                                    var groups = Convert.ToString(item[FormAccessListFields.AccessGroups]);
                                    if (!string.IsNullOrEmpty(groups))
                                    {
                                        field.AccessGroups = groups.Split(',').ToList();
                                    }
                                    groups = Convert.ToString(item[FormAccessListFields.EditGroups]);
                                    if (!string.IsNullOrEmpty(groups))
                                    {
                                        field.EditGroups = groups.Split(',').ToList();
                                    }
                                    values.Add(field);
                                }
                            }
                            cacheManagementService.AddToCache<List<FormAccessItem>>(CacheKeys.FormAccessList, values, new TimeSpan(240, 0, 0));
                        }
                    }
                }
            }
            return values;
        }
        #endregion
        public string GetPersonFieldForDisplay(string person)
        {
            if (string.IsNullOrEmpty(person))
                return string.Empty;
            if (person.IndexOf("#") < 0)
                return person;

            return person.Substring(person.IndexOf("#") + 1);
        }
        public string GetPersonFieldForDisplayUpdated(string person)
        {
            if (string.IsNullOrEmpty(person))
                return string.Empty;
            if (person.IndexOf("#") < 0)
                return person;
            if (person.IndexOf(",, ") != -1)
            {
                int LNEnd = person.IndexOf(",, ");
                string lastname = person.Substring(4, LNEnd - 4);
                int FNEnd = person.IndexOf(",#i");
                string firstname = person.Substring(LNEnd + 2, FNEnd - LNEnd - 2);
                return lastname + ", " + firstname;
            }
            else
            {
                return GetPersonFieldForDisplay(person);
            }

        }
        public BrandItem GetBrandItem(string brand, string hierarchy)
        {
            BrandItem brandItem = new BrandItem();

            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_OBMBrandManagerLookupListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"MaterialGroup1Brand\" /><Value Type=\"Text\">" + brand + "</Value></Eq><Eq><FieldRef Name=\"ProductHierarchyLevel1\" /><Value Type=\"Text\">" + hierarchy + "</Value></Eq></And></Where>";
                    spQuery.RowLimit = 1;

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        SPListItem item = compassItemCol[0];
                        if (item != null)
                        {
                            brandItem.ProductHierarchyLevel1 = Convert.ToString(item["ProductHierarchyLevel1"]);
                            brandItem.MaterialGroup1Brand = Convert.ToString(item["MaterialGroup1Brand"]);
                            brandItem.BrandManager = Convert.ToString(item["Brand Manager"]);
                            brandItem.PM = Convert.ToString(item["PM"]);
                        }
                    }
                }
            }

            return brandItem;
        }

        public string GetWorkflowPhase(int iItemId)
        {
            string workflowPhase = string.Empty;

            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem item = spList.GetItemById(iItemId);
                    if (item != null)
                    {
                        workflowPhase = Convert.ToString(item[CompassListFields.WorkflowPhase]);
                    }
                }
            }
            return workflowPhase;
        }
        public List<Entity> GetLookupOptions(string lookupList)
        {
            SPList spList;
            List<Entity> options = null;
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    options = new List<Entity>();
                    spList = spWeb.Lists.TryGetList(lookupList);
                    string[] fields = { GlobalLookupFieldConstants.Id, GlobalLookupFieldConstants.Title, GlobalLookupFieldConstants.Active };
                    var items = spList.GetItems(fields).Cast<SPListItem>().Where(x => Convert.ToBoolean(x[GlobalLookupFieldConstants.Active]).Equals(true)).OrderBy(y => y.Title);
                    var enumItems = items.GetEnumerator();
                    while (enumItems.MoveNext())
                        options.Add(new Entity { Id = Convert.ToString(enumItems.Current.ID), Name = enumItems.Current.Title });
                }
            }
            return options;
        }
        public List<Entity> GetPreloadLookupOptions(string lookupList)
        {
            SPListItemCollection compassItemCol;
            SPList spList;
            SPQuery spQuery;
            List<Entity> options = null;
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    /*options = new List<Entity>();
                    spList = spWeb.Lists.TryGetList(lookupList);
                    string[] fields = { GlobalLookupFieldConstants.Id, GlobalLookupFieldConstants.Title, GlobalLookupFieldConstants.Active };
                    var items = spList.GetItems(fields).Cast<SPListItem>().Where(x => Convert.ToBoolean(x[GlobalLookupFieldConstants.Active]).Equals(true)
                        && Convert.ToString(x["Preload"]).Equals("1")).OrderBy(y => y.Title);
                    var enumItems = items.GetEnumerator();
                    while (enumItems.MoveNext())
                        options.Add(new Entity { Id = Convert.ToString(enumItems.Current.ID), Name = enumItems.Current.Title });*/
                    spList = spWeb.Lists.TryGetList(lookupList);
                    spQuery = new SPQuery();
                    /*spQuery.Query = "<Where><And><Eq><FieldRef Name=\"Active\" /><Value Type=\"Boolean\">true</Value></Eq>" +
                        "<Eq><FieldRef Name=\"Preload\" /><Value Type=\"Text\">1</Value></Eq></And></Where>";*/
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"Preload\" /><Value Type=\"Text\">1</Value></Eq></Where>";
                    compassItemCol = spList.GetItems(spQuery);
                    options = new List<Entity>();
                    foreach (SPListItem item in compassItemCol)
                        if (item != null)
                            options.Add(new Entity { Id = Convert.ToString(item.ID), Name = Convert.ToString(item[GlobalLookupFieldConstants.Title]) });
                }
            }
            return options;
        }       
    }
}
