using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Linq;
using System.ComponentModel;
using Ferrara.Compass.Classes;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Services;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Enum;
using System.Web;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Threading;

namespace Ferrara.Compass.WebParts.WorldSyncFuseFile
{
    [ToolboxItemAttribute(false)]
    public partial class WorldSyncFuseFile : WebPart
    {
        #region Member Variables
        private IExceptionService exceptionService;
        private INotificationService notificationService;
        private IUserManagementService userMgmtService;
        private IPackagingItemService packagingService;
        private IExcelExportSyncService exportService;
        private IItemProposalService proposalService;
        private IBillOfMaterialsService BOMservice;
        private IUtilityService utilityService;
        private string fileName = string.Empty;
        List<byte[]> fileContents = new List<byte[]>();

        #endregion

        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public WorldSyncFuseFile()
        {
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
            userMgmtService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            packagingService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            exportService = DependencyResolution.DependencyMapper.Container.Resolve<IExcelExportSyncService>();
            proposalService = DependencyResolution.DependencyMapper.Container.Resolve<IItemProposalService>();
            BOMservice = DependencyResolution.DependencyMapper.Container.Resolve<IBillOfMaterialsService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.divAccessDenied.Visible = false;
                this.divAccessRequest.Visible = false;
            }
            lblSuccess.Text = "";
            lblUploadError.Text = "";
            InitializeScreen();
            LoadUploadedFuseFiles();
        }
        private void InitializeScreen()
        {
            // If user does not belong to a valid group for the page, inform them that they do not hvae access rights
            divAccessDenied.Visible = !userMgmtService.HasReadAccess(CompassForm.WorldSyncFuseFile);
            divAccessRequest.Visible = false;
            headerFailedSAPNumbers.Visible = false;
        }

        #region Button Click events
        protected void lbHelpDeskEmail_Click(object sender, EventArgs e)
        {
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, GlobalConstants.PAGE_WorldSyncFuseFile);
            this.divAccessDenied.Visible = false;
            this.divAccessRequest.Visible = true;
        }
        protected void btnFUSEFileMassExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (docUpload.HasFile)
                {
                    //1.Read all the sap numbers
                    var line = string.Empty;
                    string[] values;
                    List<string> SAPNumbersEntered = new List<string>();
                    bool EndoFFile = false;

                    using (StreamReader csvreader = new StreamReader(docUpload.PostedFile.InputStream))
                    {
                        while (!EndoFFile)
                        {
                            line = csvreader.ReadLine();
                            values = line.Split(',');
                            EndoFFile = csvreader.EndOfStream;

                            if (string.IsNullOrWhiteSpace(values[0])) EndoFFile = true;
                            else SAPNumbersEntered.Add(values[0]);
                        }
                    }

                    if (SAPNumbersEntered.Count > 0)
                    {
                        var CompassListDetails = GetCompassListDetails(SAPNumbersEntered);

                        if (CompassListDetails.Count > 0)
                        {
                            foreach (KeyValuePair<int, string> CompassListDetail in CompassListDetails)
                            {
                                Export(CompassListDetail.Key, CompassListDetail.Value, GetFileName());
                            }
                            UploadFiles(fileName, fileContents);
                            this.lblSuccess.Text = "File " + fileName + " uploaded successfully";
                            LoadUploadedFuseFiles();
                        }
                        else
                        {
                            this.lblUploadError.Text = "No SAP numbers uploaded";
                        }
                    }
                    else
                    {
                        this.lblUploadError.Text = "No SAP numbers found in the csv file. Please add SAP numbers in the first column wthout header in the file";
                    }
                }
                else
                {
                    this.lblUploadError.Text = "Please select a file to upload.";
                }

            }
            catch (ThreadAbortException ex)
            {
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "UploadBulkFuseFiles: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "UploadBulkFuseFiles", "btnSubmit_Click");
            }
            finally
            {
            }
        }
        protected void btnGenerateFuseFile_Click(object sender, EventArgs e)
        {
            try
            {
                //1.Read all the sap numbers
                var EnteredSAPNumbers = GetEnteredSAPNumbers();

                var CompassListDetails = GetCompassListDetails(EnteredSAPNumbers.ToList());
                //2. Loop all SAP numbers and get the latest project 

                if (CompassListDetails.Count == 0)
                {
                    this.lblUploadError.Text = "No SAP numbers uploaded";
                }
                else
                {
                    foreach (KeyValuePair<int, string> CompassListDetail in CompassListDetails)
                    {
                        Export(CompassListDetail.Key, CompassListDetail.Value, GetFileName());
                    }
                    UploadFiles(fileName, fileContents);
                    this.lblSuccess.Text = "File " + fileName + " uploaded successfully";
                    LoadUploadedFuseFiles();
                }
            }
            catch (ThreadAbortException ex)
            {
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "UploadBulkFuseFiles: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "UploadBulkFuseFiles", "btnSubmit_Click");
            }
            finally
            {
            }
        }

        #endregion

        #region Provate Methods
        private string[] GetEnteredSAPNumbers()
        {
            return hdnSAPNumbers.Value?.Split(',');
        }
        private Dictionary<int, string> GetCompassListDetails(List<string> EnteredSAPNumbers)
        {
            var CompassListDetails = new Dictionary<int, string>();
            ulFailedSAPNumbers.Visible = false;
            headerFailedSAPNumbers.Visible = false;

            foreach (string SAPNumber in EnteredSAPNumbers)
            {
                var CompassListItems = utilityService.GetCompassListFromSAPNumber(SAPNumber);
                if (CompassListItems?.Count > 0)
                {
                    CompassListDetails.Add(CompassListItems.FirstOrDefault().CompassListItemId, CompassListItems.FirstOrDefault().ProjectNumber);
                }
                else
                {
                    headerFailedSAPNumbers.Visible = true;
                    ulFailedSAPNumbers.Visible = true;
                    HtmlGenericControl li = new HtmlGenericControl("li");
                    ulFailedSAPNumbers.Controls.Add(li);
                    HtmlGenericControl anchor = new HtmlGenericControl("a");
                    anchor.Attributes.Add("class", "errorMessage");
                    anchor.InnerText = SAPNumber;
                    li.Controls.Add(anchor);
                }
            }

            return CompassListDetails;
        }
        private void Export(int compassId, string ProjectNumber, string fileName)
        {

            List<Dictionary<string, string>> itemRows = null;
            Dictionary<string, string> publishRow = null, linkRow = null;

            GetExportData(compassId, ref itemRows, ref publishRow, ref linkRow);
            fileContents.Add(exportService.WriteToFile(ProjectNumber, itemRows, publishRow, linkRow));

        }
        private void UploadFiles(string fileName, List<byte[]> fileContent)
        {
            UploadFuseFileToDocumentLibrary(fileName, fileContent);

           // Page.Response.Clear();
           // Page.Response.ContentType = "application/force-download";
           // Page.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
           // foreach (byte[] data in fileContent)
           // {
           //     Page.Response.BinaryWrite(data);
           // }
           //HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        private void GetExportData(int compassId, ref List<Dictionary<string, string>> itemRows, ref Dictionary<string, string> publishRow,ref Dictionary<string, string> linkRow)
        {
            ItemProposalItem proposalItem;
            CompassPackMeasurementsItem measure;
            Dictionary<string, string> values;
            string todayDate, childDescription, onz = "";
            todayDate = DateTime.Now.ToString("s");
            proposalItem = proposalService.GetItemProposalItem(compassId);
            measure = BOMservice.GetPackMeasurementsItem(compassId, 0);
            childDescription = GetChildDescription(proposalItem.SAPDescription, ref onz);
            itemRows = new List<Dictionary<string, string>>();
            values = new Dictionary<string, string>();
            values.Add(GlobalConstants.EXP_SYNC_ITM_ItemID, proposalItem.CaseUCC);
            values.Add(GlobalConstants.EXP_SYNC_ITM_ItemName, proposalItem.SAPDescription);
            values.Add(GlobalConstants.EXP_SYNC_ITM_BrandName, proposalItem.MaterialGroup1Brand);
            values.Add(GlobalConstants.EXP_SYNC_ITM_Depth, Utilities.FormatDecimal(measure.CaseDimensionLength, 2));
            values.Add(GlobalConstants.EXP_SYNC_ITM_Height, Utilities.FormatDecimal(measure.CaseDimensionHeight, 2));
            values.Add(GlobalConstants.EXP_SYNC_ITM_Width, Utilities.FormatDecimal(measure.CaseDimensionWidth, 2));
            values.Add(GlobalConstants.EXP_SYNC_ITM_GrossWeight, Utilities.FormatDecimal(measure.CaseGrossWeight, 2));
            values.Add(GlobalConstants.EXP_SYNC_ITM_NetWeight, Utilities.FormatDecimal(measure.CaseNetWeight, 2));
            values.Add(GlobalConstants.EXP_SYNC_ITM_GS1TradeItemIDKeyValue, proposalItem.CaseUCC);
            values.Add(GlobalConstants.EXP_SYNC_ITM_ShortDescription, proposalItem.SAPDescription);
            values.Add(GlobalConstants.EXP_SYNC_ITM_ProductDescription, proposalItem.SAPDescription);
            values.Add(GlobalConstants.EXP_SYNC_ITM_FunctionalName, proposalItem.SAPDescription);
            values.Add(GlobalConstants.EXP_SYNC_ITM_Volume, Utilities.FormatDecimal(measure.CaseCube, 2));
            values.Add(GlobalConstants.EXP_SYNC_ITM_QtyofNextLevelItem, Utilities.FormatDecimal(proposalItem.RetailSellingUnitsBaseUOM, 2));
            values.Add(GlobalConstants.EXP_SYNC_ITM_NumberofItemsinaCompleteLayerGTINPalletTi, Utilities.FormatDecimal(measure.CasesPerLayer, 2));
            values.Add(GlobalConstants.EXP_SYNC_ITM_NumberofCompleteLayersContainedinItemGTINPalletHi, Utilities.FormatDecimal(measure.LayersPerPallet, 2));
            values.Add(GlobalConstants.EXP_SYNC_ITM_AlternateItemIdentificationId, proposalItem.SAPItemNumber);
            values.Add(GlobalConstants.EXP_SYNC_ITM_MinProductLifespanfromProduction, GetShelfLife(compassId));
            values.Add(GlobalConstants.EXP_SYNC_ITM_StartAvailabilityDate, todayDate);
            values.Add(GlobalConstants.EXP_SYNC_ITM_EffectiveDate, todayDate);
            itemRows.Add(values);
            values = new Dictionary<string, string>(1);
            values.Add(GlobalConstants.EXP_SYNC_ITM_ItemID, string.Empty);
            itemRows.Add(values);
            values = new Dictionary<string, string>();
            values.Add(GlobalConstants.EXP_SYNC_ITM_ItemID, proposalItem.UnitUPC);
            values.Add(GlobalConstants.EXP_SYNC_ITM_ItemName, childDescription);
            values.Add(GlobalConstants.EXP_SYNC_ITM_BrandName, proposalItem.MaterialGroup1Brand);
            values.Add(GlobalConstants.EXP_SYNC_ITM_Depth, Utilities.FormatDecimal(measure.UnitDimensionLength, 2));
            values.Add(GlobalConstants.EXP_SYNC_ITM_Height, Utilities.FormatDecimal(measure.UnitDimensionHeight, 2));
            values.Add(GlobalConstants.EXP_SYNC_ITM_Width, Utilities.FormatDecimal(measure.UnitDimensionWidth, 2));
            values.Add(GlobalConstants.EXP_SYNC_ITM_GrossWeight, onz);
            values.Add(GlobalConstants.EXP_SYNC_ITM_NetWeight, Utilities.FormatDecimal(measure.NetUnitWeight, 2));
            values.Add(GlobalConstants.EXP_SYNC_ITM_GS1TradeItemIDKeyValue, proposalItem.UnitUPC);
            values.Add(GlobalConstants.EXP_SYNC_ITM_ShortDescription, childDescription);
            values.Add(GlobalConstants.EXP_SYNC_ITM_ProductDescription, childDescription);
            values.Add(GlobalConstants.EXP_SYNC_ITM_FunctionalName, childDescription);
            values.Add(GlobalConstants.EXP_SYNC_ITM_StartAvailabilityDate, todayDate);
            values.Add(GlobalConstants.EXP_SYNC_ITM_EffectiveDate, todayDate);
            itemRows.Add(values);
            publishRow = new Dictionary<string, string>();
            publishRow.Add(GlobalConstants.EXP_SYNC_PUB_ItemID, proposalItem.CaseUCC);
            publishRow.Add(GlobalConstants.EXP_SYNC_PUB_PublishDate, todayDate);
            linkRow = new Dictionary<string, string>();
            linkRow.Add(GlobalConstants.EXP_SYNC_LKN_ParentItemID, proposalItem.CaseUCC);
            linkRow.Add(GlobalConstants.EXP_SYNC_LKN_ChildItemID, proposalItem.UnitUPC);
            linkRow.Add(GlobalConstants.EXP_SYNC_LKN_QtyofChildItem, Utilities.FormatDecimal(proposalItem.RetailSellingUnitsBaseUOM, 2));
        }
        private string GetShelfLife(int compassId)
        {
            List<PackagingItem> packagingItems;
            int current, shelfLife = int.MaxValue;
            packagingItems = packagingService.GetCandySemiItemsForProject(compassId);
            foreach (PackagingItem packItem in packagingItems)
            {
                if (packItem.ShelfLife == "")
                    continue;
                current = int.Parse(packItem.ShelfLife);
                if (current < shelfLife)
                    shelfLife = current;
            }
            return shelfLife == int.MaxValue ? "" : shelfLife.ToString();
        }
        private string GetChildDescription(string description, ref string onz)
        {
            int slashIdx, ozIdx, w;
            char ch;
            bool otherCharsFound;
            otherCharsFound = false;
            slashIdx = description.IndexOf('/');
            if (slashIdx <= 0 || slashIdx == description.Length - 1)
                return description;
            for (w = slashIdx - 1; w > 0; w--)
            {
                ch = description[w];
                if (ch == ' ')
                {
                    if (otherCharsFound)
                        break;
                }
                else
                    otherCharsFound = true;
            }
            if (w > 0)
            {
                ozIdx = description.ToLower().IndexOf("oz", slashIdx);
                if (ozIdx > -1)
                    onz = description.Substring(slashIdx + 1, ozIdx - slashIdx - 1).Trim();
                return description.Substring(0, w).Trim() + " " + description.Substring(slashIdx + 1);
            }
            return description;
        }
        private void LoadUploadedFuseFiles()
        {
            var files = utilityService.GetUploadedFuseFilesForLastSevenDays();

            if (files.Count > 0)
            {
                rptLatestFuseFiles.Visible = true;
                rptLatestFuseFiles.DataSource = files;
                rptLatestFuseFiles.DataBind();

                rptLatestFuseFiles.Visible = true;
                rptLatestFuseFiles.DataSource = files;
                rptLatestFuseFiles.DataBind();
            }
            else
            {
                rptLatestFuseFiles.Visible = false;
                rptLatestFuseFiles.Visible = false;
            }
        }
        private void UploadFuseFileToDocumentLibrary(string fileName, List<byte[]> FileContents)
        {
            if (FileContents != null)
            {
                var request = new List<WorldSyncFuseFileItem>();
                // Retrieve the data from the form
                foreach (byte[] FileContent in FileContents)
                {
                    request.Add(ConstructFuseFileAttachment(fileName, FileContent));
                }
                if (!utilityService.UploadAttachment(request))
                {
                    //   this.lblUploadError.Text = "Error uploading files...<br>Ensure your filename does not contain any of the following characters: ~, #, %, & , *, {, }, \\, :, <, >, ?, /, |, “";
                }
            }
        }
        private WorldSyncFuseFileItem ConstructFuseFileAttachment(string fileName, byte[] FileContent)
        {
            WorldSyncFuseFileItem request = null;
            HttpFileCollection uploads = HttpContext.Current.Request.Files;
            Stream FileStream = new MemoryStream(FileContent);
            var file = new FileAttribute();
            file.FileName = fileName;
            file.FileContentLength = FileContent.Length;
            file.FileStream = FileStream;
            file.DocType = "xlsx";
            file.FileContent = FileContent;
            request = new WorldSyncFuseFileItem();
            //request.SAPnumber = SAPnumber;
            request.RequestType = "FUSE FILE";
            //request.RequestId = RequestId;
            request.RequestStatus = GlobalConstants.WORLDSYNCREQ_InProcess;
            request.FileAttachment = file;
            return request;
        }
        private string GetFileName()
        {
            fileName = GlobalConstants.WorldSyncFuse_FILENAME.
                            Replace("{YYYY}", DateTime.Today.Year.ToString()).
                            Replace("{MM}", DateTime.Today.Month.ToString()).
                            Replace("{DD}", DateTime.Today.Day.ToString()).
                            Replace("{HH}", DateTime.Now.Hour.ToString()).
                            Replace("{MMM}", DateTime.Now.Minute.ToString()).
                            Replace("{SS}", DateTime.Now.Second.ToString());

            return fileName;
        }
        #endregion
    }
}
