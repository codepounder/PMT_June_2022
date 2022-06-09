using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Classes;
using Microsoft.Practices.Unity;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Ferrara.Compass.Layouts.Ferrara.Compass.AppPages
{
    public partial class UploadBulkFuseFiles : LayoutsPageBase
    {
        #region Member Variables
        private IExcelExportSyncService exportService;
        private IPackagingItemService packagingItemService;
        private IUtilityService utilityService;
        private IExceptionService exceptionService;
        private IItemProposalService proposalService;
        private IBillOfMaterialsService BOMservice;
        private IPackagingItemService packagingService;
        private string webUrl = string.Empty;
        private bool error = false;

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            packagingItemService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            exportService = DependencyResolution.DependencyMapper.Container.Resolve<IExcelExportSyncService>();
            proposalService = DependencyResolution.DependencyMapper.Container.Resolve<IItemProposalService>();
            BOMservice = DependencyResolution.DependencyMapper.Container.Resolve<IBillOfMaterialsService>();
            packagingService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
        }

        #region Properties
        private string ProjectNumber
        {
            get
            {
                if (Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo] != null)
                    return Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo];
                return string.Empty;
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            webUrl = SPContext.Current.Web.Url;

            if (!Page.IsPostBack)
            {
                try
                {
                    this.Form.Page.Title = "Upload Bulk Fuse Files";
                }
                catch (Exception ex)
                {
                    exceptionService.Handle(LogCategory.CriticalError, ex, "UploadBulkFuseFiles", "Page_Load");
                }
            }
        }

        #region Button/Link Methods
        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
                Context.Response.Flush();
            }
            catch (Exception exception)
            {

            }
            finally
            {
                Context.Response.End();
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var fileName = string.Empty;
            try
            {
                if (docUpload.HasFile)
                {
                    //1.Read all the sap numbers
                    StreamReader csvreader = new StreamReader(docUpload.PostedFile.InputStream);
                    var line = string.Empty;
                    string[] values;
                    List<string> SAPNumbersEntered = new List<string>();
                    bool EndoFFile = false;
                    while (!EndoFFile)
                    {
                        line = csvreader.ReadLine();
                        values = line.Split(',');
                        EndoFFile = csvreader.EndOfStream;

                        if (string.IsNullOrWhiteSpace(values[0])) EndoFFile = true;
                        else SAPNumbersEntered.Add(values[0]);
                    }

                    if (SAPNumbersEntered.Count > 0)
                    {
                        var CompassListDetails = GetCompassListDetails(SAPNumbersEntered);

                        fileName = GlobalConstants.WorldSyncFuse_FILENAME.
                                        Replace("{YYYY}", DateTime.Today.Year.ToString()).
                                        Replace("{MM}", DateTime.Today.Month.ToString()).
                                        Replace("{DD}", DateTime.Today.Day.ToString()).
                                        Replace("{HH}", DateTime.Now.Hour.ToString()).
                                        Replace("{MMM}", DateTime.Now.Minute.ToString()).
                                        Replace("{SS}", DateTime.Now.Second.ToString());

                        if (CompassListDetails.Count > 0)
                        {
                            foreach (KeyValuePair<int, string> CompassListDetail in CompassListDetails)
                            {
                                Export(CompassListDetail.Key, CompassListDetail.Value, fileName);
                            }

                            this.lblSuccess.Text = "File " + fileName + " uploaded successfully";
                            error = true;
                        }
                        else
                        {
                            this.lblUploadError.Text = "No SAP numbers uploaded";
                            error = true;
                        }
                    }
                    else
                    {
                        this.lblUploadError.Text = "No SAP numbers found in the csv file. Please add SAP numbers in the first column wthout header in the file";
                        error = true;
                    }
                }
                else
                {
                    this.lblUploadError.Text = "Please select a file to upload.";
                    error = true;
                }

            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "UploadBulkFuseFiles: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "UploadBulkFuseFiles", "btnSubmit_Click");
            }
            finally
            {
                if (!error)
                {
                    Context.Response.End();
                }
            }
        }
        #endregion

        #region Private Methods

        private Dictionary<int, string> GetCompassListDetails(List<string> EnteredSAPNumbers)
        {
            var CompassListDetails = new Dictionary<int, string>();
            var FailedSAPNumbers = new List<string>();

            foreach (string SAPNumber in EnteredSAPNumbers)
            {
                var CompassListItems = utilityService.GetCompassListFromSAPNumber(SAPNumber);
                if (CompassListItems?.Count > 0)
                {
                    CompassListDetails.Add(CompassListItems.FirstOrDefault().CompassListItemId, CompassListItems.FirstOrDefault().ProjectNumber);
                }
                else
                {
                    error = true;
                    FailedSAPNumbers.Add(SAPNumber);
                }
            }

            if (FailedSAPNumbers.Count > 0)
            {
                string FailedNumbers = string.Empty;
                foreach (string failedSAPNumber in FailedSAPNumbers)
                {
                    FailedNumbers = FailedNumbers + failedSAPNumber + ", ";
                }
                error = true;
                this.lblUploadError.Text = FailedNumbers.Remove(FailedNumbers.LastIndexOf(", "), (", ").Length).Insert(FailedNumbers.LastIndexOf(", "), ".");
            }
            return CompassListDetails;
        }
        private void Export(int compassId, string ProjectNumber, string fileName)
        {
            try
            {
                byte[] fileContent;
                List<Dictionary<string, string>> itemRows = null;
                Dictionary<string, string> publishRow = null, linkRow = null;


                GetExportData(compassId, ref itemRows, ref publishRow, ref linkRow);
                fileContent = exportService.WriteToFile(ProjectNumber, itemRows, publishRow, linkRow);

                UploadFuseFileToDocumentLibrary(fileName, fileContent);

                Page.Response.Clear();
                Page.Response.ContentType = "application/force-download";
                Page.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                Page.Response.BinaryWrite(fileContent);
                Page.Response.End();
            }
            catch (ThreadAbortException ex)
            {
            }
        }
        private void GetExportData(int compassId, ref List<Dictionary<string, string>> itemRows, ref Dictionary<string, string> publishRow,
            ref Dictionary<string, string> linkRow)
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
        private void UploadFuseFileToDocumentLibrary(string fileName, byte[] FileContent)
        {
            if (FileContent != null)
            {
                // Retrieve the data from the form
                WorldSyncFuseFileItem request = ConstructFuseFileAttachment(fileName, FileContent);
                //if (!utilityService.UploadAttachment(request))
                //{
                //    this.lblUploadError.Text = "Error uploading files...<br>Ensure your filename does not contain any of the following characters: ~, #, %, & , *, {, }, \\, :, <, >, ?, /, |, “";
                //}
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
        #endregion
    }
}

