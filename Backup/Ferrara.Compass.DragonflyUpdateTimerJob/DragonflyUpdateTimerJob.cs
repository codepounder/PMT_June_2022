using Ferrara.Compass.DragonflyUpdateTimerJob.Classes;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using OfficeOpenXml;
using OfficeOpenXml.Core.ExcelPackage;
using System.Data.OleDb;
using System.Data;
using Ferrara.Compass.DragonflyUpdateTimerJob.Models;

namespace Ferrara.Compass.DragonflyUpdateTimerJob
{
    public class DragonflyUpdateTimerJob : SPJobDefinition
    {
        #region Constants
        // Lists
        public const string LIST_CompassDragonflyListName = "Compass Dragonfly List";
        public const string LIST_CompassDragonflyErrorListName = "Compass Dragonfly Error List";
        public const string LIST_LogsListName = "Logs List";
        public const string DOCLIBRARY_CompassDragonflyLibraryName = "Compass Dragonfly Documents";
        public const string LIST_CompassList2Name = "Compass List 2";
        public const string LIST_CompassListName = "Compass List";
        #endregion

        #region Tasks
        public const string TaskName_CreateAndUploadProof = "Create and Upload Proof";
        public const string TaskName_FinalSeparatedProofApprovedNotification = "Final Separated Proof Approved Notification";
        public const string TaskName_FinalSeparatedProofApproved = "Final Separated Proof Approved";
        //public const string TaskName_FinalSeparatedProofApprovedSendToProjectLeaderAndObm = "Final Separated Proof Approved Notification";
        public const string TaskName_MaterialLaunchDate = "Material Launch";
        public const string TaskName_NotificationOfPlatesAndFinalFileRelease = "Notification of Plates & Final File Release";
        public const string TaskName_NotificationOfProductionArtFileRelease = "Notification of Production Art File Release";
        public const string TaskName_ProductionArtCompleted = "Production Art Completed";
        //public const string TaskName_ProofCreatedAndUploaded = "Proof Created and Uploaded";
        public const string TaskName_ScheduleProductionArtWithMetadata = "Schedule Production Art with Metadata";
        public const string TaskName_SendGraphicsRequestForm = "Send Graphics Request Form";
        public const string TaskName_SgsBuild1_UpPdfAndUploadIntoDf = "SGS Build 1-Up PDF and upload into DF";
        public const string TaskName_SgsOnsiteUploadsArtForApproval = "SGS On-Site Uploads Art for Approval";
        public const string TaskName_UploadFinalPdfFromCo_ManCanels = "Upload Final PDF from Co-Man Canels";
        #endregion

        #region Member Variables
        SPWeb web = null;
        public const string VersionNumber = "Version: 1.8";
        #endregion

        #region Constructors
        public DragonflyUpdateTimerJob() : base() { }
        public DragonflyUpdateTimerJob(string jobName, SPService service)
            : base(jobName, service, null, SPJobLockType.Job)
        {
            this.Title = jobName;
        }
        public DragonflyUpdateTimerJob(string jobName, SPWebApplication webapp)
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
            ProcessDragonflyRecords();
            web.AllowUnsafeUpdates = false;
        }
        #region Dragonfly list update methods
        private void ProcessDragonflyRecords()
        {
            InsertLog(string.Concat("Start - Dragonfly Update Timer Job - Version :", VersionNumber), "ProcessDragonflyRecords", "");
            List<DragonFlyRecordItem> FileRecords = ReadDragonflyFile();
            UpsertDragonflyRecords(FileRecords);
            MoveFileToArchive();
            InsertLog(string.Concat("End - Dragonfly Update Timer Job - Version :", VersionNumber), "ProcessDragonflyRecords", "");
        }

        private List<DragonFlyRecordItem> ReadDragonflyFile()
        {
            try
            {

                var file = GetCurrentUploadedFile();
                StreamReader csvreader = new StreamReader(file.FileStream);

                char[] delimiter = new char[] { '\t' };

                var line = string.Empty;
                string[] values;
                bool StartReadingData = false;

                List<DragonFlyRecordItem> DragonFlyRecordItems = new List<DragonFlyRecordItem>();

                while (csvreader.Peek() > -1)
                {
                    line = csvreader.ReadLine();
                    values = line.Split(delimiter);

                    if (StartReadingData)
                    {
                        var DragonFlyRecordItem = new DragonFlyRecordItem();
                        DragonFlyRecordItem.CompassProjectNumber = values[0];
                        DragonFlyRecordItem.ItemNumber = values[1];
                        DragonFlyRecordItem.MaterialNumber = values[2];
                        DragonFlyRecordItem.MaterialStatus = values[3];
                        DragonFlyRecordItem.TaskName = values[4];
                        DragonFlyRecordItem.TaskVersionNumber = values[5];
                        DragonFlyRecordItem.TaskStatus = values[6];
                        DragonFlyRecordItem.ActualStartDate = Convert.ToDateTime(values[7]);
                        DragonFlyRecordItem.ActualEndDate = Convert.ToDateTime(values[8]);
                        DragonFlyRecordItems.Add(DragonFlyRecordItem);
                    }

                    if (!StartReadingData && values[0] == "Compass Project #" && values[1] == "Item #")
                    {
                        StartReadingData = true;
                    }
                }
                return DragonFlyRecordItems;
            }
            catch (Exception ex)
            {
                InsertLog("Dragonfly Update Timer Job-Error", "ReadDragonflyFile", string.Concat(ex.Message));
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, string.Concat("Dragonfly Update Timer Job-ReadDragonflyFile: ", ex.Message));
                return new List<DragonFlyRecordItem>();
            }

        }
        public void UpsertDragonflyRecords(List<DragonFlyRecordItem> DragonflyRecords)
        {
            try
            {

                foreach (var DragonflyRecord in DragonflyRecords)
                {
                    var error = false;
                    if (string.IsNullOrEmpty(DragonflyRecord.CompassProjectNumber))
                    {
                        error = true;
                        DragonflyRecord.Error = "No Project Number. ";
                    }

                    if (string.IsNullOrEmpty(DragonflyRecord.MaterialNumber))
                    {
                        error = true;
                        DragonflyRecord.Error += "No Material Number. ";
                    }

                    if (string.IsNullOrEmpty(DragonflyRecord.ItemNumber))
                    {
                        error = true;
                        DragonflyRecord.Error += "No Item Number. ";
                    }

                    //TODO: IF Project Number or material number or item number have non-numeric value or equal to 0, add them to error records.

                    if (error)
                    {
                        InsertDragonflyErrorRecords(DragonflyRecord);
                        continue;
                    }

                    int ItemId = GetItemIdFromProjectNumber(DragonflyRecord.CompassProjectNumber);
                    string GraphicsTimeLineType = "Standard";

                    if (ItemId != 0)
                    {
                        GraphicsTimeLineType = GetGraphicsTimelineType(ItemId);
                    }

                    web.AllowUnsafeUpdates = true;
                    var spList = web.Lists.TryGetList(LIST_CompassDragonflyListName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where>" +
                                        "<And>" +
                                            "<And>" +
                                                "<Eq><FieldRef Name=\"CompassProjectNumber\" /><Value Type=\"Text\">" + DragonflyRecord.CompassProjectNumber + " </Value></Eq>" +
                                                "<Eq><FieldRef Name=\"ItemNumber\" /><Value Type=\"Text\">" + DragonflyRecord.ItemNumber + "</Value></Eq>" +
                                            "</And>" +
                                            "<And>" +
                                                "<Eq><FieldRef Name=\"MaterialNumber\" /><Value Type=\"Text\">" + DragonflyRecord.MaterialNumber + " </Value></Eq>" +
                                                "<Eq><FieldRef Name=\"MaterialNumber\" /><Value Type=\"Text\">" + DragonflyRecord.MaterialNumber + " </Value></Eq>" +
                                            "</And>" +
                                        "</And>" +
                                    "</Where>";

                    spQuery.RowLimit = 1;

                    SPListItemCollection StageGateItemCol = spList.GetItems(spQuery);
                    SPListItem item;
                    if (StageGateItemCol != null)
                    {
                        if (StageGateItemCol.Count < 1)
                        {
                            item = spList.AddItem();
                            item["Title"] = DragonflyRecord.CompassProjectNumber;
                        }
                        else
                        {
                            item = StageGateItemCol[0];
                        }

                        if (item != null)
                        {
                            item["CompassProjectNumber"] = DragonflyRecord.CompassProjectNumber;
                            item["CompassProjectNumber"] = DragonflyRecord.CompassProjectNumber;
                            item["ItemNumber"] = DragonflyRecord.ItemNumber;
                            item["MaterialNumber"] = DragonflyRecord.MaterialNumber;
                            item["MaterialStatus"] = DragonflyRecord.MaterialStatus;
                            // item["TaskName"] = DragonflyRecord.TaskName;
                            item["TaskVersionNumber"] = DragonflyRecord.TaskVersionNumber;
                            //item["TaskStatus"] = DragonflyRecord.TaskStatus;

                            #region Expedited
                            if (GraphicsTimeLineType.Contains("Expedited"))
                            {
                                if (DragonflyRecord.TaskName == TaskName_CreateAndUploadProof)
                                {
                                    if (item["ProofCreatedUploaded_ActEnd"] == null)
                                    {
                                        item["ProofCreatedUploaded_ActEnd"] = DragonflyRecord.ActualEndDate;

                                    }
                                    if (item["ProofApproved_ActStart"] == null)
                                    {
                                        item["ProofApproved_ActStart"] = DragonflyRecord.ActualEndDate;
                                    }
                                }
                                else if (DragonflyRecord.TaskName == TaskName_FinalSeparatedProofApprovedNotification || DragonflyRecord.TaskName == TaskName_FinalSeparatedProofApproved)
                                {
                                    if (item["ProofApproved_ActEnd"] == null)
                                    {
                                        item["ProofApproved_ActEnd"] = DragonflyRecord.ActualEndDate;
                                    }
                                    if (item["MakeAndShipPlates_ActStart"] == null)
                                    {
                                        item["MakeAndShipPlates_ActStart"] = DragonflyRecord.ActualEndDate;
                                    }
                                }
                                else if (DragonflyRecord.TaskName == TaskName_MaterialLaunchDate)
                                {
                                    if (item["ProjUploadedtoDF_ActEnd"] == null)
                                    {
                                        item["ProjUploadedtoDF_ActEnd"] = DragonflyRecord.ActualEndDate;
                                    }
                                }
                                else if (DragonflyRecord.TaskName == TaskName_NotificationOfPlatesAndFinalFileRelease)
                                {
                                    if (item["MakeAndShipPlates_ActEnd"] == null)
                                    {
                                        item["MakeAndShipPlates_ActEnd"] = DragonflyRecord.ActualEndDate;
                                    }
                                }
                                if (DragonflyRecord.TaskName == TaskName_NotificationOfProductionArtFileRelease)
                                {
                                }
                                else if (DragonflyRecord.TaskName == TaskName_ProductionArtCompleted)
                                {
                                    if (item["ArtworkApproved_ActEnd"] == null)
                                    {
                                        item["ArtworkApproved_ActEnd"] = DragonflyRecord.ActualEndDate;
                                    }
                                    if (item["ProofCreatedUploaded_ActStart"] == null)
                                    {
                                        item["ProofCreatedUploaded_ActStart"] = DragonflyRecord.ActualEndDate;
                                    }
                                }
                                //else if (DragonflyRecord.TaskName == TaskName_ProofCreatedAndUploaded)
                                //{
                                //    item["ProofApproved_ActStart"] = DragonflyRecord.ActualEndDate;
                                //}
                                else if (DragonflyRecord.TaskName == TaskName_SendGraphicsRequestForm)
                                {
                                }
                                else if (DragonflyRecord.TaskName == TaskName_ScheduleProductionArtWithMetadata)
                                {
                                    if (item["SGSOnsiteUploadsArt_ActStart"] == null)
                                    {
                                        item["SGSOnsiteUploadsArt_ActStart"] = DragonflyRecord.ActualEndDate;
                                    }
                                }
                                else if (DragonflyRecord.TaskName == TaskName_SgsBuild1_UpPdfAndUploadIntoDf)
                                {
                                    if (item["SGSOnsiteUploadsArt_ActEnd"] == null)
                                    {
                                        item["SGSOnsiteUploadsArt_ActEnd"] = DragonflyRecord.ActualEndDate;
                                    }
                                    if (item["ArtworkApproved_ActStart"] == null)
                                    {
                                        item["ArtworkApproved_ActStart"] = DragonflyRecord.ActualEndDate;
                                    }
                                }
                                else if (DragonflyRecord.TaskName == TaskName_SgsOnsiteUploadsArtForApproval)
                                {
                                }
                                else if (DragonflyRecord.TaskName == TaskName_UploadFinalPdfFromCo_ManCanels)
                                {
                                }
                            }
                            #endregion
                            #region Standard
                            else// (GraphicsTimeLineType == "Standard")
                            {
                                if (DragonflyRecord.TaskName == TaskName_CreateAndUploadProof)
                                {
                                    if (item["ProofCreatedUploaded_ActEnd"] == null)
                                    {
                                        item["ProofCreatedUploaded_ActEnd"] = DragonflyRecord.ActualEndDate;
                                    }
                                    if (item["ProofApproved_ActStart"] == null)
                                    {
                                        item["ProofApproved_ActStart"] = DragonflyRecord.ActualEndDate;
                                    }
                                }
                                else if (DragonflyRecord.TaskName == TaskName_FinalSeparatedProofApprovedNotification || DragonflyRecord.TaskName == TaskName_FinalSeparatedProofApproved)
                                {
                                    if (item["ProofApproved_ActEnd"] == null)
                                    {
                                        item["ProofApproved_ActEnd"] = DragonflyRecord.ActualEndDate;
                                    }
                                    if (item["MakeAndShipPlates_ActStart"] == null)
                                    {
                                        item["MakeAndShipPlates_ActStart"] = DragonflyRecord.ActualEndDate;
                                    }
                                }
                                else if (DragonflyRecord.TaskName == TaskName_MaterialLaunchDate)
                                {
                                    if (item["ProjUploadedtoDF_ActEnd"] == null)
                                    {
                                        item["ProjUploadedtoDF_ActEnd"] = DragonflyRecord.ActualEndDate;
                                    }
                                }
                                else if (DragonflyRecord.TaskName == TaskName_NotificationOfPlatesAndFinalFileRelease)
                                {
                                    if (item["MakeAndShipPlates_ActEnd"] == null)
                                    {
                                        item["MakeAndShipPlates_ActEnd"] = DragonflyRecord.ActualEndDate;
                                    }
                                }
                                if (DragonflyRecord.TaskName == TaskName_NotificationOfProductionArtFileRelease)
                                {
                                }
                                else if (DragonflyRecord.TaskName == TaskName_ProductionArtCompleted)
                                {
                                    if (item["ArtworkApproved_ActEnd"] == null)
                                    {
                                        item["ArtworkApproved_ActEnd"] = DragonflyRecord.ActualEndDate;
                                    }
                                    if (item["ProofCreatedUploaded_ActStart"] == null)
                                    {
                                        item["ProofCreatedUploaded_ActStart"] = DragonflyRecord.ActualEndDate;
                                    }
                                }
                                //else if (DragonflyRecord.TaskName == TaskName_ProofCreatedAndUploaded)
                                //{
                                //    item["ProofApproved_ActStart"] = DragonflyRecord.ActualEndDate;
                                //}
                                else if (DragonflyRecord.TaskName == TaskName_SendGraphicsRequestForm)
                                {
                                    if (item["ProjUploadedtoDF_ActStart"] == null)
                                    {
                                        item["ProjUploadedtoDF_ActStart"] = DragonflyRecord.ActualEndDate;
                                    }
                                }
                                else if (DragonflyRecord.TaskName == TaskName_ScheduleProductionArtWithMetadata)
                                {
                                    if (item["SGSOnsiteUploadsArt_ActStart"] == null)
                                    {
                                        item["SGSOnsiteUploadsArt_ActStart"] = DragonflyRecord.ActualEndDate;
                                    }
                                }
                                else if (DragonflyRecord.TaskName == TaskName_SgsBuild1_UpPdfAndUploadIntoDf)
                                {
                                }
                                else if (DragonflyRecord.TaskName == TaskName_SgsOnsiteUploadsArtForApproval)
                                {
                                    if (item["SGSOnsiteUploadsArt_ActEnd"] == null)
                                    {
                                        item["SGSOnsiteUploadsArt_ActEnd"] = DragonflyRecord.ActualEndDate;
                                    }
                                    if (item["ArtworkApproved_ActStart"] == null)
                                    {
                                        item["ArtworkApproved_ActStart"] = DragonflyRecord.ActualEndDate;
                                    }
                                }
                                else if (DragonflyRecord.TaskName == TaskName_UploadFinalPdfFromCo_ManCanels)
                                {
                                }
                            }
                            #endregion

                            item.Update();
                        }
                    }
                    web.AllowUnsafeUpdates = false;
                }
            }
            catch (Exception ex)
            {
                InsertLog("Dragonfly Update Timer Job-Error", "UpsertDragonflyRecords", string.Concat(ex.Message));
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, string.Concat("Dragonfly Update Timer Job-UpsertDragonflyRecords: ", ex.Message));
            }
        }
        public void InsertDragonflyErrorRecords(DragonFlyRecordItem DragonflyRecord)
        {
            try
            {
                web.AllowUnsafeUpdates = true;

                var spList = web.Lists.TryGetList(LIST_CompassDragonflyErrorListName);

                SPListItem item = spList.AddItem();

                item["Title"] = DragonflyRecord.CompassProjectNumber;
                item["CompassProjectNumber"] = DragonflyRecord.CompassProjectNumber;
                item["ItemNumber"] = DragonflyRecord.ItemNumber;
                item["MaterialNumber"] = DragonflyRecord.MaterialNumber;
                item["MaterialStatus"] = DragonflyRecord.MaterialStatus;
                item["TaskName"] = DragonflyRecord.TaskName;
                item["TaskVersionNumber"] = DragonflyRecord.TaskVersionNumber;
                item["TaskStatus"] = DragonflyRecord.TaskStatus;

                if (DragonflyRecord.ActualStartDate != null && DragonflyRecord.ActualStartDate != DateTime.MinValue)
                {
                    item["ActualStartDate"] = Convert.ToString(DragonflyRecord.ActualStartDate);
                }
                if (DragonflyRecord.ActualEndDate != null && DragonflyRecord.ActualEndDate != DateTime.MinValue)
                {
                    item["ActualEndDate"] = Convert.ToString(DragonflyRecord.ActualEndDate);
                }
                item["Error"] = DragonflyRecord.Error;
                item.Update();
                web.AllowUnsafeUpdates = false;
            }
            catch (Exception ex)
            {
                InsertLog("Dragonfly Update Timer Job-Error", "InsertDragonflyErrorRecords", string.Concat(ex.Message));
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, string.Concat("Dragonfly Update Timer Job-InsertDragonflyErrorRecords: ", ex.Message));
            }
        }
        public FileAttribute GetCurrentUploadedFile()
        {
            // Get current DateTime. It can be any DateTime object in your code.  
            DateTime aDate = DateTime.Now;

            // Format Datetime in different formats and display them  
            var CurrentDate = aDate.ToString("MM-dd-yyyy");

            FileAttribute file = new FileAttribute();

            SPDocumentLibrary documentLib = web.Lists.TryGetList(DOCLIBRARY_CompassDragonflyLibraryName) as SPDocumentLibrary;
            //string folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", CurrentDate);
            string folderUrl = documentLib.RootFolder.ServerRelativeUrl;
            if (web.GetFolder(folderUrl).Exists)
            {
                SPFolder stageGateProjectFolder = web.GetFolder(folderUrl);
                var spfile = stageGateProjectFolder.Files.OfType<SPFile>().FirstOrDefault();
                if (spfile != null)
                {
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
                    file.FileUrl = string.Concat(web.Url, "/", fileurl);
                    file.DisplayFileName = Convert.ToString(spfile.Item["Display File Name"]);
                    file.FileContent = spfile.OpenBinary();
                    file.FileStream = spfile.OpenBinaryStream();
                }
            }

            return file;
        }
        public void MoveFileToArchive()
        {
            try
            {
                var fileAttribute = GetCurrentUploadedFile();
                SPDocumentLibrary documentLib = web.Lists.TryGetList(DOCLIBRARY_CompassDragonflyLibraryName) as SPDocumentLibrary;
                string folderUrl = documentLib.RootFolder.ServerRelativeUrl;
                string ArchivefolderUrl = string.Concat(folderUrl, "/", "Archive");
                string NewFileUrl = fileAttribute.FileUrl.Replace("/CompassDragonflyDocuments/", "/CompassDragonflyDocuments/Archive/");
                if (web.GetFolder(folderUrl).Exists)
                {
                    web.AllowUnsafeUpdates = true;
                    SPFolder stageGateProjectFolder = web.GetFolder(folderUrl);
                    var spfile = stageGateProjectFolder.Files.OfType<SPFile>().FirstOrDefault();
                    if (web.GetFolder(ArchivefolderUrl).Exists)
                    {
                        SPFolder ArchiveFolder = web.GetFolder(ArchivefolderUrl);
                        string timestamp = DateTime.Now.ToString("MM-dd-yyyy hh.mm.ss tt");
                        string fileName = fileAttribute.FileName.Replace(".txt", "");
                        NewFileUrl = NewFileUrl.Replace(fileName, string.Concat(fileName, " ", timestamp));
                        ArchiveFolder.Files.Add(NewFileUrl, fileAttribute.FileStream);
                    }
                    spfile.Delete();
                    web.AllowUnsafeUpdates = false;
                }
            }
            catch (Exception ex)
            {
                InsertLog(string.Concat("Dragonfly Update Timer Job-Error"), "MoveFileToArchive", ex.Message);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, string.Concat("Dragonfly Update Timer Job-MoveFileToArchive: ", ex.Message));
            }
        }
        private string GetGraphicsTimelineType(int itemId)
        {
            string GraphicsTimelineType = "";
            try
            {
                var spList = web.Lists.TryGetList(LIST_CompassList2Name);
                SPQuery spQuery = new SPQuery();
                spQuery.Query = "<Where><Eq><FieldRef Name=\"" + "CompassListItemId" + "\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                var CompassList2Items = spList.GetItems(spQuery);
                if (CompassList2Items != null && CompassList2Items.Count > 0)
                {
                    var ipItem = CompassList2Items[0];
                    GraphicsTimelineType = Convert.ToString(ipItem["NeedSExpeditedWorkflowWithSGS"]);

                }
            }
            catch (Exception ex)
            {
                InsertLog(string.Concat("Dragonfly Update Timer Job-Error", " - itemId -", itemId), "GetGraphicsTimelineType", string.Concat(ex.Message));
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, string.Concat("Dragonfly Update Timer Job-GetGraphicsTimelineType: ", ex.Message));
            }

            return (string.IsNullOrEmpty(GraphicsTimelineType) || GraphicsTimelineType == "Select...") ? "Standard" : GraphicsTimelineType;
        }
        public int GetItemIdFromProjectNumber(string projectNumber)
        {
            int id = 0;
            if (string.IsNullOrEmpty(projectNumber))
                return id;
            try
            {

                SPList spList = web.Lists.TryGetList(LIST_CompassListName);
                var fields = new string[] { "ProjectNumber" };
                var item = spList.GetItems(fields).Cast<SPListItem>().FirstOrDefault(x => Convert.ToString(x["ProjectNumber"]).ToLower().Equals(projectNumber.ToLower()));
                if (item != null)
                {
                    id = item.ID;
                }
            }
            catch (Exception ex)
            {
                InsertLog(string.Concat("Dragonfly Update Timer Job-Error", " - Project No -", projectNumber), "GetItemIdFromProjectNumber", string.Concat(ex.Message));
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, string.Concat("Dragonfly Update Timer Job-GetItemIdFromProjectNumber: ", ex.Message));
            }
            return id;
        }
        #endregion
        #region InsertLog Method
        private void InsertLog(string message, string method, string additionalInfo)
        {
            web.AllowUnsafeUpdates = true;
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
            web.AllowUnsafeUpdates = false;
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
