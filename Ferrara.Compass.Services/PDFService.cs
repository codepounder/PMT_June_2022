using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Ferrara.Compass.Services
{
    public class PDFService : IPDFService
    {
        #region Member Variables
        private readonly IExceptionService exceptionService;
        private readonly IPackagingItemService packagingService;
        private readonly IApprovalService approvalService;
        private readonly IBillOfMaterialsService materialService;
        private readonly IUtilityService utilService;
        private readonly IItemProposalService ipService;
        private readonly IOPSService opsService;
        private readonly IExternalManufacturingService extMfgService;
        private readonly ITradePromoGroupService tradePromoService;
        private readonly IOBMFirstReviewService obmFirstReviewService;
        private readonly IStageGateGeneralService stageGateGeneralService;
        private IComponentCostingQuoteService componentCostingQuoteService;
        private IGraphicsService graphicsService;
        private IStageGateCreateProjectService stageGateCreateProjectService;
        private IStageGateFinancialServices stageGateFinancialServices;
        private IBOMSetupService bomSetupService;
        private readonly DateTime DATETIME_MIN = new DateTime(1900, 1, 1);
        private iTextSharp.text.Font titleFont;
        private iTextSharp.text.Font subTitleFont;
        private iTextSharp.text.Font subTitleFontNormal;
        private iTextSharp.text.Font normalFont;
        private iTextSharp.text.Font boldFont;
        private iTextSharp.text.Font smallFont;
        private iTextSharp.text.Font FirstFont;
        private iTextSharp.text.Font SecondFont;
        private iTextSharp.text.Font SecondFontGrey;
        private iTextSharp.text.Font FirstBoldFont;
        private iTextSharp.text.Font SecondBoldFont;
        private iTextSharp.text.Font SecondBoldFontGrey;
        private BaseColor alternateColor;
        #endregion
        #region Constructor
        public PDFService(IExceptionService exceptionService, IPackagingItemService packagingService, IApprovalService approvalService,
                        IBillOfMaterialsService materialService, IUtilityService utilService, IItemProposalService ipService, IOPSService opsService,
                        ITradePromoGroupService tradePromoService, IGraphicsService graphicsService, IExternalManufacturingService extMfgService,
                        IComponentCostingQuoteService componentCostingQuoteService, IOBMFirstReviewService obmFirstReviewService,
                        IStageGateCreateProjectService stageGateCreateProjectService, IStageGateGeneralService stageGateGeneralService,
                        IStageGateFinancialServices stageGateFinancialServices, IBOMSetupService bomSetupService)
        {
            this.exceptionService = exceptionService;
            this.packagingService = packagingService;
            this.approvalService = approvalService;
            this.materialService = materialService;
            this.utilService = utilService;
            this.ipService = ipService;
            this.opsService = opsService;
            this.tradePromoService = tradePromoService;
            this.graphicsService = graphicsService;
            this.extMfgService = extMfgService;
            this.componentCostingQuoteService = componentCostingQuoteService;
            this.obmFirstReviewService = obmFirstReviewService;
            this.stageGateCreateProjectService = stageGateCreateProjectService;
            this.stageGateGeneralService = stageGateGeneralService;
            this.stageGateFinancialServices = stageGateFinancialServices;
            this.bomSetupService = bomSetupService;
            // Create Fonts
            titleFont = iTextSharp.text.FontFactory.GetFont("Arial", 18, Font.BOLD);
            subTitleFont = iTextSharp.text.FontFactory.GetFont("Arial", 12, Font.BOLD);
            boldFont = iTextSharp.text.FontFactory.GetFont("Arial", 10, Font.BOLD);
            normalFont = iTextSharp.text.FontFactory.GetFont("Arial", 10, Font.NORMAL);
            alternateColor = new BaseColor(176, 216, 255);
        }
        #endregion
        #region Public Methods
        public void CreateGraphicsRequestPDF(int itemId, int packagingId)
        {
            bool isExternallyManufacturedPackaged;
            try
            {
                ItemProposalItem ipItem = ipService.GetItemProposalItem(itemId);
                PackagingItem packItem = packagingService.GetPackagingItemByPackagingId(packagingId);
                ApprovalListItem approvalItem = approvalService.GetApprovalItem(itemId);
                BillofMaterialsItem materialItem = materialService.GetBillOfMaterialsItem(itemId);
                CompassPackMeasurementsItem packMeasItem = materialService.GetPackMeasurementsItem(itemId, packagingId);
                OPSItem opsItem = opsService.GetOPSItem(itemId);
                ExternalManufacturingItem extMfgItem = extMfgService.GetExternalManufacturingItem(itemId);
                OBMFirstReviewItem obmFirstReviewItem = obmFirstReviewService.GetPMFirstReviewItem(itemId);
                string fileNames = string.Empty;

                // Create a ITextSharp Document object
                // Use the A4 Page Size which measures 595 x 842 pixels with a 36 point margin all around
                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 20, 20, 25, 25);

                // Create a new PdfWriter object, specifying the output stream
                MemoryStream output = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, output);

                // Open the Document for writing
                pdfDoc.Open();

                //iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(SPContext.Current.Web.Url + "/_layouts/Ferrara.Compass/Images/Graphics_Summary1.jpg");
                //logo.ScalePercent(10);
                //logo.SetAbsolutePosition(595 - 20 - 146, 842 - 25 - 60);
                //pdfDoc.Add(logo);

                pdfDoc.Add(new iTextSharp.text.Paragraph("Request for Graphics", titleFont));
                pdfDoc.Add(new iTextSharp.text.Paragraph(GetProjectTitle(ipItem), titleFont));

                // Set Table Details
                PdfPTable grTable = new PdfPTable(2);
                grTable.TotalWidth = 700;
                grTable.HorizontalAlignment = 0;
                grTable.SpacingBefore = 10;
                grTable.SpacingAfter = 10;
                grTable.DefaultCell.Border = 0;
                grTable.SetWidths(new int[] { 3, 4 });
                int currentRow = 1;
                bool alternateRow = false;

                // Create Table Cells
                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Submitted By:"));
                grTable.AddCell(CreateRow(alternateRow, ipItem.PMName));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Date Graphic Request Form Submitted:"));
                grTable.AddCell(CreateRow(alternateRow, approvalItem.GRAPHICS_StartDate));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "First Production Date:"));
                grTable.AddCell(CreateRow(alternateRow, GetDateForDisplay(obmFirstReviewItem.FirstProductionDate)));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "First Ship Date:"));
                grTable.AddCell(CreateRow(alternateRow, GetDateForDisplay(ipItem.RevisedFirstShipDate)));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Project Number:"));
                grTable.AddCell(CreateRow(alternateRow, ipItem.ProjectNumber));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Line of Business:"));
                grTable.AddCell(CreateRow(alternateRow, ipItem.ProductHierarchyLevel1));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Season:"));
                if (string.Equals(ipItem.ProductHierarchyLevel1, "Seasonal (000000023)"))
                    grTable.AddCell(CreateRow(alternateRow, ipItem.ProductHierarchyLevel2));
                else
                    grTable.AddCell(CreateRow(alternateRow, GlobalConstants.CONST_NotApplicable));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Customer:"));
                if (string.IsNullOrEmpty(ipItem.Customer) || ipItem.Customer.Equals(GlobalConstants.LIST_NoSelectionText))
                    grTable.AddCell(CreateRow(alternateRow, "Not Customer Specific"));
                else
                    grTable.AddCell(CreateRow(alternateRow, ipItem.Customer));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Brand:"));
                grTable.AddCell(CreateRow(alternateRow, ipItem.MaterialGroup1Brand));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Packaging Engineer:"));
                grTable.AddCell(CreateRow(alternateRow, GetPersonFieldForDisplay(materialItem.PackagingEngineerLead)));
                currentRow++;
                alternateRow = !alternateRow;

                isExternallyManufacturedPackaged = (string.Equals(opsItem.MakeLocation, "Externally Manufactured")) ||
                    (string.Equals(opsItem.PackingLocation, "Externally Packed"))
                    ;
                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Is Item externally manufactured or packaged?:"));
                if (isExternallyManufacturedPackaged)
                    grTable.AddCell(CreateRow(alternateRow, extMfgItem.CoManufacturingClassification));
                else
                    grTable.AddCell(CreateRow(alternateRow, "No"));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Make Location:"));
                grTable.AddCell(CreateRow(alternateRow, opsItem.MakeLocation));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Pack Location:"));
                grTable.AddCell(CreateRow(alternateRow, extMfgItem.PackingLocation));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Item #:"));
                grTable.AddCell(CreateRow(alternateRow, ipItem.SAPItemNumber));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Item Description:"));
                grTable.AddCell(CreateRow(alternateRow, ipItem.SAPDescription));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Material #:"));
                grTable.AddCell(CreateRow(alternateRow, packItem.MaterialNumber));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Material Description:"));
                grTable.AddCell(CreateRow(alternateRow, packItem.MaterialDescription));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Printer:"));
                grTable.AddCell(CreateRow(alternateRow, packItem.PrinterSupplier));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Component Type:"));
                grTable.AddCell(CreateRow(alternateRow, packItem.PackagingComponent));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Old Material Number:"));
                grTable.AddCell(CreateRow(alternateRow, packItem.CurrentOldItem));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Old Material Description:"));
                grTable.AddCell(CreateRow(alternateRow, packItem.CurrentOldItemDescription));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Substrate:"));
                grTable.AddCell(CreateRow(alternateRow, packItem.Structure));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Substrate Color:"));
                if (string.IsNullOrEmpty(packItem.StructureColor))
                    grTable.AddCell(CreateRow(alternateRow, GlobalConstants.CONST_NotApplicable));
                else
                    grTable.AddCell(CreateRow(alternateRow, packItem.StructureColor));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Print Style:"));
                if (packItem.PackagingComponent.ToLower().Contains("film"))
                {
                    grTable.AddCell(CreateRow(alternateRow, packItem.FilmPrintStyle));
                }
                else if (packItem.PackagingComponent.ToLower().Contains("corrugated") || packItem.PackagingComponent.ToLower().Contains("paperboard"))
                {
                    grTable.AddCell(CreateRow(alternateRow, packItem.CorrugatedPrintStyle));
                }
                else
                {
                    grTable.AddCell(CreateRow(alternateRow, GlobalConstants.CONST_NotApplicable));
                }
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Direct Print, Label or Offset:"));
                grTable.AddCell(CreateRow(alternateRow, packItem.PrinterSupplier));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Unwind #:"));
                if (string.IsNullOrEmpty(packItem.Unwind))
                    grTable.AddCell(CreateRow(alternateRow, GlobalConstants.CONST_NotApplicable));
                else
                    grTable.AddCell(CreateRow(alternateRow, packItem.Unwind));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Unit UPC:"));
                grTable.AddCell(CreateRow(alternateRow, ipItem.UnitUPC));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Jar/Display UPC:"));
                grTable.AddCell(CreateRow(alternateRow, ipItem.DisplayBoxUPC));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Case UCC:"));
                grTable.AddCell(CreateRow(alternateRow, ipItem.CaseUCC));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Pallet UCC:"));
                grTable.AddCell(CreateRow(alternateRow, ipItem.PalletUCC));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Old Finished Good Item Number:"));
                grTable.AddCell(CreateRow(alternateRow, packItem.CurrentOldItem));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Old Finished Good Item Description:"));
                grTable.AddCell(CreateRow(alternateRow, packItem.CurrentOldItemDescription));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Like Material Number:"));
                grTable.AddCell(CreateRow(alternateRow, packItem.CurrentLikeItem));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Like Material Description:"));
                grTable.AddCell(CreateRow(alternateRow, packItem.CurrentLikeItemDescription));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "How is it a Like Component Number:"));
                grTable.AddCell(CreateRow(alternateRow, packItem.CurrentLikeItemReason));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Reg Sheet:"));
                grTable.AddCell(CreateRow(alternateRow, GetFileNames(ipItem.ProjectNumber, GlobalConstants.DOCTYPE_NLEA)));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Dieline(s):"));
                grTable.AddCell(CreateRow(alternateRow, GetFileNames(ipItem.ProjectNumber, packagingId, GlobalConstants.DOCTYPE_CADDrawing)));
                currentRow++;
                alternateRow = !alternateRow;


                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Seal Info:"));
                grTable.AddCell(CreateRow(alternateRow, packItem.BackSeam));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Is Pallet Pattern Changing?:"));
                grTable.AddCell(CreateRow(alternateRow, packMeasItem.PalletPatternChange));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Pallet Pattern:"));
                grTable.AddCell(CreateRow(alternateRow, GetFileNames(ipItem.ProjectNumber, GlobalConstants.DOCTYPE_PalletPattern)));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Marketing Claims Labeling Requirements:"));
                grTable.AddCell(CreateRow(alternateRow, ipItem.MarketClaimsLabelingRequirements));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Component Graphics Brief (Marketing):"));
                grTable.AddCell(CreateRow(alternateRow, packItem.GraphicsBrief));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Project Direction/Notes (Marketing):"));
                grTable.AddCell(CreateRow(alternateRow, ipItem.ItemConcept));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Visual Reference/Rendering:"));
                grTable.AddCell(CreateRow(alternateRow, GetFileNames(ipItem.ProjectNumber, packagingId, GlobalConstants.DOCTYPE_Rendering)));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Component Notes (PE):"));
                grTable.AddCell(CreateRow(alternateRow, packItem.Notes));
                currentRow++;
                alternateRow = !alternateRow;

                pdfDoc.Add(grTable);
                pdfDoc.Close();

                // Create the PDF
                List<FileAttribute> uploadFile = new List<FileAttribute>();
                FileAttribute file = new FileAttribute();
                file.FileName = utilService.CreateSafeFileName(packItem.MaterialNumber + " Graphics Request Form") + ".pdf";
                file.FileContent = output.ToArray();
                file.DocType = GlobalConstants.DOCTYPE_GraphicsRequest;
                uploadFile.Add(file);
                utilService.UploadPackagingAttachment(uploadFile, ipItem.ProjectNumber, packagingId);
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.IPF.ToString(), "CreateGraphicsRequestPDF", "Export Error");
            }
        }
        public void CreateGraphicsRequestPDF_New(int itemId, int packagingId)
        {
            bool isExternallyManufacturedPackaged;
            try
            {
                ItemProposalItem ipItem = ipService.GetItemProposalItem(itemId);
                PackagingItem packItem = packagingService.GetPackagingItemByPackagingId(packagingId);
                PackagingItem ParentpackItem = (packItem.ParentID == 0) ? new PackagingItem() : packagingService.GetPackagingItemByPackagingId(packItem.ParentID);
                ApprovalListItem approvalItem = approvalService.GetApprovalItem(itemId);
                BillofMaterialsItem materialItem = materialService.GetBillOfMaterialsItem(itemId);
                BOMSetupItem packMeasItem = bomSetupService.GetPackMeasurementsItem(itemId, packItem.ParentID);
                OPSItem opsItem = opsService.GetOPSItem(itemId);
                ExternalManufacturingItem extMfgItem = extMfgService.GetExternalManufacturingItem(itemId);
                OBMFirstReviewItem obmFirstReviewItem = obmFirstReviewService.GetPMFirstReviewItem(itemId);
                string fileNames = string.Empty;

                // Create a ITextSharp Document object
                // Use the A4 Page Size which measures 595 x 842 pixels with a 36 point margin all around
                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 20, 20, 25, 25);

                // Create a new PdfWriter object, specifying the output stream
                MemoryStream output = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, output);

                // Open the Document for writing
                pdfDoc.Open();

                //iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(SPContext.Current.Web.Url + "/_layouts/Ferrara.Compass/Images/Graphics_Summary1.jpg");
                //logo.ScalePercent(10);
                //logo.SetAbsolutePosition(595 - 20 - 146, 842 - 25 - 60);
                //pdfDoc.Add(logo);

                pdfDoc.Add(new iTextSharp.text.Paragraph("Request for Graphics", titleFont));
                pdfDoc.Add(new iTextSharp.text.Paragraph(GetProjectTitle(ipItem), titleFont));

                // Set Table Details
                PdfPTable grTable = new PdfPTable(2);
                grTable.TotalWidth = 700;
                grTable.HorizontalAlignment = 0;
                grTable.SpacingBefore = 10;
                grTable.SpacingAfter = 10;
                grTable.DefaultCell.Border = 0;
                grTable.SetWidths(new int[] { 3, 4 });
                int currentRow = 1;
                bool alternateRow = true;

                // Create Table Cells
                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Submitted By:"));
                grTable.AddCell(CreateRow(alternateRow, ipItem.PMName));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Date Graphic Request Form Submitted:"));
                grTable.AddCell(CreateRow(alternateRow, approvalItem.GRAPHICS_StartDate));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Revised First Production Date:"));
                grTable.AddCell(CreateRow(alternateRow, GetDateForDisplay(obmFirstReviewItem.FirstProductionDate)));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Revised First Ship Date:"));
                grTable.AddCell(CreateRow(alternateRow, GetDateForDisplay(ipItem.RevisedFirstShipDate)));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Project Number:"));
                grTable.AddCell(CreateRow(alternateRow, ipItem.ProjectNumber));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Line of Business:"));
                grTable.AddCell(CreateRow(alternateRow, ipItem.ProductHierarchyLevel1));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Season:"));
                if (string.Equals(ipItem.ProductHierarchyLevel1, "Seasonal (000000023)"))
                    grTable.AddCell(CreateRow(alternateRow, ipItem.ProductHierarchyLevel2));
                else
                    grTable.AddCell(CreateRow(alternateRow, GlobalConstants.CONST_NotApplicable));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Customer:"));
                if (string.IsNullOrEmpty(ipItem.Customer) || ipItem.Customer.Equals(GlobalConstants.LIST_NoSelectionText))
                    grTable.AddCell(CreateRow(alternateRow, "Not Customer Specific"));
                else
                    grTable.AddCell(CreateRow(alternateRow, ipItem.Customer));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Brand:"));
                grTable.AddCell(CreateRow(alternateRow, ipItem.MaterialGroup1Brand));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Packaging Engineer:"));
                grTable.AddCell(CreateRow(alternateRow, GetPersonFieldForDisplay(materialItem.PackagingEngineerLead)));
                currentRow++;
                alternateRow = !alternateRow;

                isExternallyManufacturedPackaged = (string.Equals(opsItem.MakeLocation, "Externally Manufactured")) ||
                    (string.Equals(opsItem.PackingLocation, "Externally Packed"))
                    ;
                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Is Item externally manufactured or packaged?:"));
                if (isExternallyManufacturedPackaged)
                    grTable.AddCell(CreateRow(alternateRow, extMfgItem.CoManufacturingClassification));
                else
                    grTable.AddCell(CreateRow(alternateRow, "No"));
                currentRow++;
                alternateRow = !alternateRow;

                if (!string.IsNullOrEmpty(opsItem.ExternalManufacturer) && !string.Equals(opsItem.ExternalManufacturer, "Select..."))
                {
                    grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "External Manufacturer:"));
                    grTable.AddCell(CreateRow(alternateRow, opsItem.ExternalManufacturer));
                    currentRow++;
                    alternateRow = !alternateRow;
                }

                if (!string.IsNullOrEmpty(opsItem.ExternalPacker) && !string.Equals(opsItem.ExternalPacker, "Select..."))
                {
                    grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "External Packer:"));
                    grTable.AddCell(CreateRow(alternateRow, opsItem.ExternalPacker));
                    currentRow++;
                    alternateRow = !alternateRow;
                }

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Make Location:"));
                grTable.AddCell(CreateRow(alternateRow, opsItem.MakeLocation));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Pack Location:"));
                grTable.AddCell(CreateRow(alternateRow, extMfgItem.PackingLocation));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Item #:"));
                grTable.AddCell(CreateRow(alternateRow, ipItem.SAPItemNumber));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Item Description:"));
                grTable.AddCell(CreateRow(alternateRow, ipItem.SAPDescription));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Material #:"));
                grTable.AddCell(CreateRow(alternateRow, packItem.MaterialNumber));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Material Description:"));
                grTable.AddCell(CreateRow(alternateRow, packItem.MaterialDescription));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Printer:"));
                grTable.AddCell(CreateRow(alternateRow, packItem.PrinterSupplier));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Component Type:"));
                grTable.AddCell(CreateRow(alternateRow, packItem.PackagingComponent));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Old Material Number:"));
                grTable.AddCell(CreateRow(alternateRow, packItem.CurrentOldItem));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Old Material Description:"));
                grTable.AddCell(CreateRow(alternateRow, packItem.CurrentOldItemDescription));
                currentRow++;
                alternateRow = !alternateRow;

                //14 Digit Bar code visibilty
                if (packItem.ParentID != 0)
                {
                    if (packItem.PackagingComponent.ToLower().Contains("corrugated") && packItem.NewExisting.ToLower() == "new" && ParentpackItem.PackagingComponent.ToLower().Contains("transfer") && (ParentpackItem.PackLocation.Contains("FQ22") || ParentpackItem.PackLocation.Contains("FQ25")))
                    {
                        grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "14 Digit Barcode:"));
                        grTable.AddCell(CreateRow(alternateRow, packItem.FourteenDigitBarCode));
                        currentRow++;
                        alternateRow = !alternateRow;
                    }
                }

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Print Style:"));
                if (packItem.PackagingComponent.ToLower().Contains("film"))
                {
                    grTable.AddCell(CreateRow(alternateRow, packItem.FilmPrintStyle));
                }
                else if (packItem.PackagingComponent.ToLower().Contains("corrugated") || packItem.PackagingComponent.ToLower().Contains("paperboard"))
                {
                    grTable.AddCell(CreateRow(alternateRow, packItem.CorrugatedPrintStyle));
                }
                else
                {
                    grTable.AddCell(CreateRow(alternateRow, GlobalConstants.CONST_NotApplicable));
                }
                currentRow++;
                alternateRow = !alternateRow;

                //grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Direct Print, Label or Offset:"));
                //grTable.AddCell(CreateRow(alternateRow, packItem.PrinterSupplier));
                //currentRow++;
                //alternateRow = !alternateRow;

                //grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Unwind #:"));
                //if (string.IsNullOrEmpty(packItem.Unwind))
                //    grTable.AddCell(CreateRow(alternateRow, GlobalConstants.CONST_NotApplicable));
                //else
                //    grTable.AddCell(CreateRow(alternateRow, packItem.Unwind));
                //currentRow++;
                //alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Unit UPC:"));
                grTable.AddCell(CreateRow(alternateRow, ipItem.UnitUPC));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Jar/Display UPC:"));
                grTable.AddCell(CreateRow(alternateRow, ipItem.DisplayBoxUPC));
                currentRow++;
                alternateRow = !alternateRow;

                var UPCAssociated = "";
                if (packItem.UPCAssociated == "Manual Entry")
                {
                    UPCAssociated = packItem.UPCAssociatedManualEntry;
                }
                else
                {
                    UPCAssociated = packItem.UPCAssociated;
                }
                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "UPC Associated with this Packaging Component:"));
                grTable.AddCell(CreateRow(alternateRow, UPCAssociated));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Case UCC:"));
                grTable.AddCell(CreateRow(alternateRow, ipItem.CaseUCC));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Pallet UCC:"));
                grTable.AddCell(CreateRow(alternateRow, ipItem.PalletUCC));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Old Finished Good Item Number:"));
                grTable.AddCell(CreateRow(alternateRow, ipItem.OldFGItemNumber));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Old Finished Good Item Description:"));
                grTable.AddCell(CreateRow(alternateRow, ipItem.OldFGItemDescription));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Like Material Number:"));
                grTable.AddCell(CreateRow(alternateRow, packItem.CurrentLikeItem));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Like Material Description:"));
                grTable.AddCell(CreateRow(alternateRow, packItem.CurrentLikeItemDescription));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "How is it a Like Component Number:"));
                grTable.AddCell(CreateRow(alternateRow, packItem.CurrentLikeItemReason));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Reg Sheet:"));
                Font AnchorLink = FontFactory.GetFont("Arial", 10, Font.UNDERLINE, BaseColor.BLUE);
                var NLEAfiles = packagingService.GetUploadedFiles(ipItem.ProjectNumber, packagingId, GlobalConstants.DOCTYPE_NLEA);
                if (NLEAfiles.Count > 0)
                {
                    PdfPCell linkRegSheetPrdCell = new PdfPCell();
                    string NLEAfilesName = NLEAfiles[0].FileName;
                    var linkChunkRegSheet = new Chunk(NLEAfilesName, AnchorLink);
                    linkChunkRegSheet.SetAnchor(NLEAfiles[0].FileUrl);
                    linkRegSheetPrdCell.AddElement(linkChunkRegSheet);
                    linkRegSheetPrdCell.Border = Rectangle.NO_BORDER;
                    if (alternateRow) linkRegSheetPrdCell.BackgroundColor = alternateColor;
                    grTable.AddCell(linkRegSheetPrdCell);
                }
                else
                {
                    grTable.AddCell(CreateRow(alternateRow, ""));
                }
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Packaging Specification Number:"));
                grTable.AddCell(CreateRow(alternateRow, packItem.SpecificationNo));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Hyperlink to Dieline in PLM:"));
                var parentName = packItem.ParentType;
                var matNumber = packItem.MaterialNumber;
                if (parentName == "")
                {
                    parentName = "Finished Good";
                }
                if (matNumber == "")
                {
                    matNumber = "XXXXX";
                }
                PdfPCell linkPrdCell = new PdfPCell();
                var linkChunk = new Chunk(parentName + ": " + matNumber + ": Dieline Link", AnchorLink);
                linkChunk.SetAnchor(packItem.DielineURL);
                linkPrdCell.AddElement(linkChunk);
                linkPrdCell.Border = Rectangle.NO_BORDER;
                if (alternateRow) linkPrdCell.BackgroundColor = alternateColor;

                grTable.AddCell(linkPrdCell);
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Dieline Attachment:"));
                var Dielinefiles = bomSetupService.GetUploadedFiles(ipItem.ProjectNumber, packagingId, GlobalConstants.DOCTYPE_Dieline);
                string DielinefileNames = "";
                foreach (FileAttribute Dielinefile in Dielinefiles)
                {
                    if (string.IsNullOrEmpty(DielinefileNames))
                        DielinefileNames = DielinefileNames + Dielinefile.FileName;
                    else
                        DielinefileNames = DielinefileNames + ", " + Dielinefile.FileName;
                }
                grTable.AddCell(CreateRow(alternateRow, DielinefileNames));
                currentRow++;
                alternateRow = !alternateRow;


                //grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Seal Info:"));
                //grTable.AddCell(CreateRow(alternateRow, packItem.BackSeam));
                //currentRow++;
                //alternateRow = !alternateRow;

                //grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Is Pallet Pattern Changing?:"));
                //grTable.AddCell(CreateRow(alternateRow, packMeasItem.PalletPatternChange));
                //currentRow++;
                //alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Pallet Specification Hyperlink:"));
                string title = string.IsNullOrEmpty(ParentpackItem.PackagingComponent) ? "Finished Good" : ParentpackItem.PackagingComponent;
                title = title + ": " + ParentpackItem.MaterialNumber + ": Pallet Pattern";

                linkPrdCell = new PdfPCell();
                linkChunk = new Chunk(title, AnchorLink);
                linkChunk.SetAnchor(packMeasItem.PalletSpecLink);
                linkPrdCell.AddElement(linkChunk);
                linkPrdCell.Border = Rectangle.NO_BORDER;
                if (alternateRow) linkPrdCell.BackgroundColor = alternateColor;

                grTable.AddCell(linkPrdCell);

                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Pallet Specification Number:"));
                grTable.AddCell(CreateRow(alternateRow, packMeasItem.PalletSpecNumber));
                currentRow++;
                alternateRow = !alternateRow;

                //grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Marketing Claims Labeling Requirements:"));
                //grTable.AddCell(CreateRow(alternateRow, ipItem.MarketClaimsLabelingRequirements));
                //currentRow++;
                //alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Component Graphics Brief (Marketing):"));
                grTable.AddCell(CreateRow(alternateRow, packItem.GraphicsBrief));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Does this material require BioEngineering (BE) Labeling?:"));
                grTable.AddCell(CreateRow(alternateRow, packItem.BioEngLabelingRequired));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "BioEngineered (BE) QR Code File:"));
                var BEQRCodeEPSFilefiles = packagingService.GetUploadedFiles(ipItem.ProjectNumber, packagingId, GlobalConstants.DOCTYPE_BEQRCodeEPSFile);
                if (BEQRCodeEPSFilefiles.Count > 0)
                {
                    linkPrdCell = new PdfPCell();
                    var BEQRCodeEPSFileName = BEQRCodeEPSFilefiles[0].FileName;
                    linkChunk = new Chunk(BEQRCodeEPSFileName, AnchorLink);
                    linkChunk.SetAnchor(BEQRCodeEPSFilefiles[0].FileUrl);
                    linkPrdCell.AddElement(linkChunk);
                    linkPrdCell.Border = Rectangle.NO_BORDER;
                    if (alternateRow) linkPrdCell.BackgroundColor = alternateColor;
                    grTable.AddCell(linkPrdCell);
                }
                else
                {
                    grTable.AddCell(CreateRow(alternateRow, ""));
                }
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Project Direction/Notes (Marketing):"));
                grTable.AddCell(CreateRow(alternateRow, ipItem.ItemConcept));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Visual Reference/Rendering:"));
                var Renderingfiles = packagingService.GetUploadedFiles(ipItem.ProjectNumber, packagingId, GlobalConstants.DOCTYPE_Rendering);
                if (Renderingfiles.Count > 0)
                {
                    linkPrdCell = new PdfPCell();
                    var RenderingFileName = Renderingfiles[0].FileName;
                    linkChunk = new Chunk(RenderingFileName, AnchorLink);
                    linkChunk.SetAnchor(Renderingfiles[0].FileUrl);
                    linkPrdCell.AddElement(linkChunk);
                    linkPrdCell.Border = Rectangle.NO_BORDER;
                    if (alternateRow) linkPrdCell.BackgroundColor = alternateColor;
                    grTable.AddCell(linkPrdCell);
                }
                else
                {
                    grTable.AddCell(CreateRow(alternateRow, ""));
                }
                currentRow++;
                alternateRow = !alternateRow;

                if (ipItem.ProjectType == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
                {
                    grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Approved Graphic Asset:"));
                    var ApprovedGraphicsAssetfiles = packagingService.GetUploadedFiles(ipItem.ProjectNumber, packagingId, GlobalConstants.DOCTYPE_ApprovedGraphicsAsset);
                    if (ApprovedGraphicsAssetfiles.Count > 0)
                    {
                        linkPrdCell = new PdfPCell();
                        string ApprovedGraphicsAssetFileName = ApprovedGraphicsAssetfiles[0].FileName;
                        linkChunk = new Chunk(ApprovedGraphicsAssetFileName, AnchorLink);
                        linkChunk.SetAnchor(ApprovedGraphicsAssetfiles[0].FileUrl);
                        linkPrdCell.AddElement(linkChunk);
                        linkPrdCell.Border = Rectangle.NO_BORDER;
                        if (alternateRow) linkPrdCell.BackgroundColor = alternateColor;
                        grTable.AddCell(linkPrdCell);
                    }
                    else
                    {
                        grTable.AddCell(CreateRow(alternateRow, ""));
                    }
                    currentRow++;
                    alternateRow = !alternateRow;
                }

                pdfDoc.Add(grTable);
                pdfDoc.Close();

                // Create the PDF
                List<FileAttribute> uploadFile = new List<FileAttribute>();
                FileAttribute file = new FileAttribute();
                file.FileName = utilService.CreateSafeFileName(packItem.MaterialNumber + " Graphics Request Form") + ".pdf";
                file.FileContent = output.ToArray();
                file.DocType = GlobalConstants.DOCTYPE_GraphicsRequest;
                uploadFile.Add(file);
                utilService.UploadPackagingAttachment(uploadFile, ipItem.ProjectNumber, packagingId);
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.IPF.ToString(), "CreateGraphicsRequestPDF_New", "Export Error");
            }
        }
        public void CreateComponentCostingRequestPDF(int itemId, string packid, string StructureLabel)
        {
            try
            {
                int packagingId = Convert.ToInt32(packid);
                ComponentCostingQuoteItem obj = componentCostingQuoteService.GetComponentCostingQuoteItem(packagingId, itemId);
                ItemProposalItem ipItem = ipService.GetItemProposalItem(itemId);
                ApprovalListItem approvalItem = approvalService.GetApprovalItem(itemId);
                string fileNames = string.Empty;

                // Create a ITextSharp Document object
                // Use the A4 Page Size which measures 595 x 842 pixels with a 36 point margin all around
                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 20, 20, 25, 25);

                // Create a new PdfWriter object, specifying the output stream
                MemoryStream output = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, output);

                // Open the Document for writing
                pdfDoc.Open();

                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(SPContext.Current.Web.Url + "/_layouts/15/Ferrara.Compass/images/Ferrara_Logo.png");
                logo.ScalePercent(10);
                logo.SetAbsolutePosition(595 - 20 - 146, 842 - 25 - 60);
                pdfDoc.Add(logo);

                pdfDoc.Add(new iTextSharp.text.Paragraph("Request Component Costing Quote", titleFont));
                pdfDoc.Add(new iTextSharp.text.Paragraph(GetProjectTitle(ipItem), titleFont));

                // Set Table Details
                PdfPTable grTable = new PdfPTable(2);
                grTable.TotalWidth = 700;
                grTable.HorizontalAlignment = 0;
                grTable.SpacingBefore = 10;
                grTable.SpacingAfter = 10;
                grTable.DefaultCell.Border = 0;
                grTable.SetWidths(new int[] { 3, 4 });
                int currentRow = 1;
                bool alternateRow = false;

                // Create Table Cells
                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Procurement Manager:"));
                grTable.AddCell(CreateRow(alternateRow, GetPersonFieldForDisplay(obj.ProcurementManager)));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Date:"));
                DateTime costingQuoteDate;
                if (DateTime.TryParse(obj.CostingQuoteDate, out costingQuoteDate))
                {
                    grTable.AddCell(CreateRow(alternateRow, GetDateForDisplay(costingQuoteDate)));
                }
                else
                {
                    grTable.AddCell(CreateRow(alternateRow, GetDateForDisplay(DateTime.Now)));
                }
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Submitted By:"));
                grTable.AddCell(CreateRow(alternateRow, GetPersonFieldForDisplay(ipItem.PM)));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Date Submitted:"));
                grTable.AddCell(CreateRow(alternateRow, approvalItem.IPF_SubmittedDate));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "SAP Material #:"));
                grTable.AddCell(CreateRow(alternateRow, obj.MaterialNumber));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Material Description:"));
                grTable.AddCell(CreateRow(alternateRow, obj.MaterialDescription));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Base UOM:"));
                grTable.AddCell(CreateRow(alternateRow, obj.BaseUOM));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Annual Volume in EA:"));
                grTable.AddCell(CreateRow(alternateRow, obj.AnnualVolumeEA));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Vendor #:"));
                grTable.AddCell(CreateRow(alternateRow, obj.VendorNumber.ToString()));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Vendor Name:"));
                grTable.AddCell(CreateRow(alternateRow, obj.PrinterSupplier));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "SKU #:"));
                grTable.AddCell(CreateRow(alternateRow, obj.SKU));
                currentRow++;
                alternateRow = !alternateRow;

                if (obj.ComponentType.Contains("Film") || obj.ComponentType.Contains("Corrugated") || obj.ComponentType.IndexOf("Other") == 0)
                {
                    grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Print Style:"));
                    grTable.AddCell(CreateRow(alternateRow, obj.PrintStyle));
                    currentRow++;
                    alternateRow = !alternateRow;
                }

                if (obj.ComponentType.Contains("Film") || obj.ComponentType.IndexOf("Other") == 0)
                {
                    grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Style:"));
                    grTable.AddCell(CreateRow(alternateRow, obj.Style));
                    currentRow++;
                    alternateRow = !alternateRow;
                }

                if (obj.ComponentType.Contains("Film") || obj.ComponentType.Contains("Label") || obj.ComponentType.IndexOf("Rigid Plastic") == 0 || obj.ComponentType.IndexOf("Other") == 0)
                {
                    grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + StructureLabel + "Structure:"));
                    grTable.AddCell(CreateRow(alternateRow, obj.Structure));
                    currentRow++;
                    alternateRow = !alternateRow;
                }

                if (obj.ComponentType.Contains("Film") || obj.ComponentType.Contains("Label") || obj.ComponentType.IndexOf("Other") == 0)
                {
                    grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Web Width:"));
                    grTable.AddCell(CreateRow(alternateRow, obj.WebWidth));
                    currentRow++;
                    alternateRow = !alternateRow;

                    grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Exact Cut off:"));
                    grTable.AddCell(CreateRow(alternateRow, obj.ExactCutOff));
                    currentRow++;
                    alternateRow = !alternateRow;

                    grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Unwind:"));
                    grTable.AddCell(CreateRow(alternateRow, obj.Unwind));
                    currentRow++;
                    alternateRow = !alternateRow;

                    grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Core Size (roll ID):"));
                    grTable.AddCell(CreateRow(alternateRow, obj.CoreSize));
                    currentRow++;
                    alternateRow = !alternateRow;

                    grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Max Diameter:"));
                    grTable.AddCell(CreateRow(alternateRow, obj.MaxDiameter));
                    currentRow++;
                    alternateRow = !alternateRow;
                }

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Receiving Plant:"));
                grTable.AddCell(CreateRow(alternateRow, obj.ReceivingPlant));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Quantities to Quote:"));
                grTable.AddCell(CreateRow(alternateRow, obj.QuantityQuote));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "First 90 Days Volume:"));
                grTable.AddCell(CreateRow(alternateRow, obj.First90Days));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Comments on Forecast:"));
                grTable.AddCell(CreateRow(alternateRow, obj.ForecastComments));
                currentRow++;
                alternateRow = !alternateRow;

                grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Requested Due Date:"));
                grTable.AddCell(CreateRow(alternateRow, obj.RequestedDueDate));
                currentRow++;
                alternateRow = !alternateRow;

                if (obj.ComponentType.Contains("Film") || obj.ComponentType.Contains("Label") || obj.ComponentType.Contains("Corrugated") || obj.ComponentType.Contains("Paperboard") || obj.ComponentType.IndexOf("Other") == 0)
                {
                    grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Number of Colors:"));
                    grTable.AddCell(CreateRow(alternateRow, obj.NumberColors));
                    currentRow++;
                    alternateRow = !alternateRow;

                    grTable.AddCell(CreateRow(alternateRow, FormatRowNumber(currentRow) + "Ink Coverage %:"));
                    grTable.AddCell(CreateRow(alternateRow, obj.InkCoveragePercentage));
                    currentRow++;
                    alternateRow = !alternateRow;
                }

                pdfDoc.Add(grTable);
                pdfDoc.Close();

                // Create the PDF
                List<FileAttribute> uploadFile = new List<FileAttribute>();
                FileAttribute file = new FileAttribute();
                file.FileName = utilService.CreateSafeFileName("Component_Costing_Quote_" + DateTime.Now.ToShortDateString()) + ".pdf";
                file.FileContent = output.ToArray();
                file.DocType = GlobalConstants.DOCTYPE_COSTING;
                uploadFile.Add(file);
                utilService.UploadPackagingAttachment(uploadFile, ipItem.ProjectNumber, packagingId);
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.IPF.ToString(), "CreateComponentCostingRequestPDF", "Export Error");
            }
        }
        public FileAttribute StageGateGenerateBriefPDF(string ProjectNumber, int StageGateListItemId, int GateNo, int BriefNo, bool IncludeFinanceBriefInGateBrief)
        {
            try
            {
                #region Genaretae PDF File
                #region Get Data from Lists
                if (!(StageGateListItemId > 0))
                {
                    return new FileAttribute();
                }

                var stageGateItem = stageGateCreateProjectService.GetStageGateProjectItem(StageGateListItemId);
                var GateDetails = stageGateGeneralService.GetStageGateGateItem(StageGateListItemId, GateNo);
                var GateBriefs = stageGateGeneralService.GetStageGateBriefItem(StageGateListItemId, GateNo);
                var FinancialSummary = stageGateFinancialServices.GetStageGateConsolidatedFinancialSummaryItem(StageGateListItemId, GateNo.ToString(), BriefNo.ToString());
                var FinancialAnalysis = stageGateFinancialServices.GetAllStageGateFinancialAnalysisItemsByGateAndBriefNumber(StageGateListItemId, GateNo.ToString(), BriefNo.ToString());

                var GateBrief = GateBriefs.Where(i => i.BriefNo == BriefNo).FirstOrDefault();
                if (stageGateItem == null)
                {
                    return new FileAttribute();
                }
                #endregion
                #region Set Fonts
                titleFont = iTextSharp.text.FontFactory.GetFont("Calibri Body", 16, Font.BOLD);
                subTitleFont = iTextSharp.text.FontFactory.GetFont("Calibri Body", 9, Font.BOLD);
                subTitleFontNormal = iTextSharp.text.FontFactory.GetFont("Calibri Body", 9, Font.NORMAL);
                boldFont = iTextSharp.text.FontFactory.GetFont("Calibri Body", 8, Font.BOLD);
                normalFont = iTextSharp.text.FontFactory.GetFont("Calibri Body", 8, Font.NORMAL);
                iTextSharp.text.Font redBoldFont = iTextSharp.text.FontFactory.GetFont("Calibri Body", 8, Font.BOLD, BaseColor.RED);

                #endregion
                #region Files
                string fileNames = string.Empty;
                //// Use the landscape Page Size which measures width =11" and Height=7.1" 
                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(new Rectangle(792, 511.2f), 5, 5, 20, 10);

                // Create a new PdfWriter object, specifying the output stream
                MemoryStream output = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, output);

                // Open the Document for writing
                pdfDoc.Open();
                #endregion
                #region Main Tabe
                PdfPTable grTable = new PdfPTable(2);
                grTable.HorizontalAlignment = Element.ALIGN_CENTER;
                grTable.SpacingBefore = 30;
                grTable.SpacingAfter = 30;
                grTable.DefaultCell.Border = Rectangle.NO_BORDER;
                grTable.SetWidths(new int[] { 1, 1 });
                #endregion
                #region Header              
                PdfPTable ProjectInfoTable = new PdfPTable(2);
                ProjectInfoTable.DefaultCell.Border = Rectangle.NO_BORDER;
                ProjectInfoTable.SetWidths(new int[] { 8, 2 });
                #region Header Line
                PdfPCell cell = new PdfPCell((new Phrase("Stage Gate Project Submission Form (Gate 1-3)", titleFont)));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_BOTTOM;
                cell.Border = Rectangle.NO_BORDER;
                ProjectInfoTable.AddCell(cell);
                #endregion
                #region Logo
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(SPContext.Current.Web.Url + "/_layouts/15/Ferrara.Compass/images/Ferrara_Logo.png");
                logo.ScalePercent(60);
                PdfPCell imageCell = new PdfPCell(logo);
                imageCell.Border = Rectangle.NO_BORDER;
                imageCell.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                imageCell.VerticalAlignment = Rectangle.ALIGN_BOTTOM;
                ProjectInfoTable.AddCell(imageCell);
                ProjectInfoTable.SpacingAfter = 20f;
                cell = new PdfPCell(ProjectInfoTable);
                cell.Colspan = 2;
                cell.Border = Rectangle.NO_BORDER;
                #endregion
                grTable.AddCell(cell);
                #endregion
                #region Red Bottom Row
                ProjectInfoTable = new PdfPTable(1);
                ProjectInfoTable.DefaultCell.Border = Rectangle.NO_BORDER;
                ProjectInfoTable.SetWidths(new int[] { 100 });
                PdfPCell EmptyCell = new PdfPCell(new Phrase("    "));
                EmptyCell.Border = Rectangle.BOTTOM_BORDER;
                EmptyCell.BorderColor = BaseColor.RED;
                EmptyCell.BorderWidth = 2f;
                ProjectInfoTable.AddCell(EmptyCell);
                cell = new PdfPCell(ProjectInfoTable);
                cell.Colspan = 2;
                cell.Border = Rectangle.NO_BORDER;
                grTable.AddCell(cell);
                #endregion
                #region Add Blank Row with No Border
                cell = new PdfPCell(GetBlankRows());
                cell.Border = Rectangle.NO_BORDER;
                cell.Colspan = 2;
                grTable.AddCell(cell);
                #endregion
                #region Project Information - Project Name, Project ID, Project Leader, Project Manager
                ProjectInfoTable = new PdfPTable(2);
                ProjectInfoTable.DefaultCell.Border = Rectangle.NO_BORDER;
                ProjectInfoTable.SetWidths(new int[] { 35, 65 });

                ProjectInfoTable.AddCell(CreateCell("Project Name", subTitleFont));
                ProjectInfoTable.AddCell(CreateCell(string.Concat(": ", stageGateItem.ProjectName), subTitleFontNormal));
                ProjectInfoTable.AddCell(CreateCell("Project ID", subTitleFont));
                ProjectInfoTable.AddCell(CreateCell(string.Concat(": ", stageGateItem.ProjectNumber), subTitleFontNormal));
                ProjectInfoTable.AddCell(CreateCell("Project Leader", subTitleFont));
                ProjectInfoTable.AddCell(CreateCell(string.Concat(": ", stageGateItem.ProjectLeaderName.Remove(stageGateItem.ProjectLeaderName.LastIndexOf(";"), 1)), subTitleFontNormal));
                ProjectInfoTable.AddCell(CreateCell("Project Manager", subTitleFont));
                ProjectInfoTable.AddCell(CreateCell(string.Concat(": ", stageGateItem.ProjectManagerName.Remove(stageGateItem.ProjectManagerName.LastIndexOf(";"), 1)), subTitleFontNormal));

                grTable.AddCell(AddMainTableCell(ProjectInfoTable));
                #endregion
                #region Project Information - Presentation Date, Gate Approval Requested, Gate(1-3), Project Tier
                ProjectInfoTable = new PdfPTable(2);
                ProjectInfoTable.DefaultCell.Border = Rectangle.NO_BORDER;
                ProjectInfoTable.SetWidths(new int[] { 55, 45 });

                ProjectInfoTable.AddCell(CreateCell("Presentation Date", subTitleFont));
                ProjectInfoTable.AddCell(CreateCell((GateDetails.ActualSGMeetingDate == DateTime.MinValue ? ":" : (": " + GateDetails.ActualSGMeetingDate.ToShortDateString())), subTitleFontNormal));
                ProjectInfoTable.AddCell(CreateCell("Gate Approval Requested", subTitleFont));
                ProjectInfoTable.AddCell(CreateCell(string.Concat(": ", GetNextStage(stageGateItem.Stage)), subTitleFontNormal));
                ProjectInfoTable.AddCell(CreateCell("Gate(1 - 3)", subTitleFont));
                ProjectInfoTable.AddCell(CreateCell(string.Concat(": ", GateNo), subTitleFontNormal));
                ProjectInfoTable.AddCell(CreateCell("Project Tier/Priority", subTitleFont));
                ProjectInfoTable.AddCell(CreateCell(string.Concat(": ", stageGateItem.ProjectTier), subTitleFontNormal));

                grTable.AddCell(AddMainTableCell(ProjectInfoTable));
                #endregion
                #region High Level Project Summary
                ProjectInfoTable = new PdfPTable(2);
                ProjectInfoTable.DefaultCell.Border = Rectangle.NO_BORDER;
                ProjectInfoTable.SetWidths(new int[] { 1, 1 });
                ProjectInfoTable.AddCell(CreateCellColspan("High Level Project Summary", 2, boldFont, new BaseColor(154, 205, 50), Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOX));
                #region Product / SKUs / Pack formats
                List list = new List(List.UNORDERED, 10f);
                list.SetListSymbol("\u2022");
                var phrase = new Phrase() { new Chunk("Product / SKUs / Pack formats:", boldFont), };
                list.Add(new ListItem(phrase));

                List SecondLevellist = new List(List.UNORDERED, 10f);
                list.SetListSymbol("\u2022");
                foreach (var productFormat in GetListfromHtmlUL(GateBrief.ProductFormats))
                {
                    SecondLevellist.Add(new ListItem(new Phrase() { new Chunk(productFormat, normalFont) }));
                }
                list.Add(SecondLevellist);
                #endregion
                #region Revised 1st Ship Date
                phrase = new Phrase() {
                                new Chunk("Revised 1st Ship Date: ", boldFont),
                                new Chunk((stageGateItem.RevisedShipDate  == DateTime.MinValue ? "" : stageGateItem.RevisedShipDate.ToShortDateString()) , normalFont)
                    };
                list.Add(new ListItem(phrase));
                #endregion
                #region Retail Execution
                phrase = new Phrase() {
                                new Chunk("Retail Execution (Example of attribute): ", boldFont),
                                new Chunk(GateBrief.RetailExecution, normalFont)
                    };
                list.Add(new ListItem(phrase));
                #endregion
                #region Other key info
                list.Add(new ListItem(new Phrase() { new Chunk("Other key info(such as claims if new/ different, etc.) from project scope:", boldFont) }));
                SecondLevellist = new List(List.UNORDERED, 10f);
                list.SetListSymbol("\u2022");
                var OtherKeyInfos = GetListfromHtmlUL(GateBrief.OtherKeyInfo);
                foreach (var OtherKeyInfo in OtherKeyInfos)
                {
                    SecondLevellist.Add(new ListItem(new Phrase() { new Chunk(OtherKeyInfo, normalFont) }));
                }
                list.Add(SecondLevellist);
                ProjectInfoTable.AddCell(CreateCellColspan(list, 2, boldFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_CENTER));
                #endregion
                grTable.AddCell(AddMainTableCell(ProjectInfoTable));
                #endregion
                #region Prodcut Image
                ProjectInfoTable = new PdfPTable(2);
                ProjectInfoTable.DefaultCell.Border = Rectangle.NO_BORDER;
                ProjectInfoTable.SetWidths(new int[] { 1, 1 });

                ProjectInfoTable.AddCell(CreateCellColspan("Product Image", 2, boldFont, new BaseColor(154, 205, 50), Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOX));

                #region get Uploaded Product Image for the brief
                var files = stageGateGeneralService.GetUploadedStageGateFiles(ProjectNumber, GlobalConstants.DOCTYPE_StageGateBriefImage, GateNo.ToString(), BriefNo.ToString(), SPContext.Current.Web.Url);
                #endregion
                if (files.Count > 0)
                {
                    string fileUrl = files[0].FileUrl;
                    iTextSharp.text.Image ProductImage = iTextSharp.text.Image.GetInstance(files[0].FileContent);

                    System.Drawing.Image MSImage;
                    using (var ms = new MemoryStream(files[0].FileContent))
                    {
                        MSImage = System.Drawing.Image.FromStream(ms);
                    }

                    MSImage = Helper.FixedSize(MSImage, 300, 200);
                    ProductImage = iTextSharp.text.Image.GetInstance(Helper.ImageToByteArray(MSImage));
                    ProjectInfoTable.AddCell(CreateRowColspanWithImage(ProductImage, 2));
                }
                else
                {
                    PdfPCell MessageCell = new PdfPCell(new Phrase("Product Image not uploaded", redBoldFont));
                    MessageCell.Colspan = 2;
                    MessageCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    MessageCell.BackgroundColor = BaseColor.WHITE;

                    ProjectInfoTable.AddCell(MessageCell);
                }

                grTable.AddCell(AddMainTableCell(ProjectInfoTable));

                #endregion
                #region Project Health/Current Situation
                ProjectInfoTable = new PdfPTable(2);
                ProjectInfoTable.DefaultCell.Border = Rectangle.NO_BORDER;
                ProjectInfoTable.SetWidths(new int[] { 1, 1 });
                ProjectInfoTable.AddCell(CreateCellColspan("Project Health/Current Situation", 2, boldFont, new BaseColor(154, 205, 50), Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOX));
                #region Overall Risk
                list = new List(List.UNORDERED, 10f);
                list.SetListSymbol("\u2022");
                phrase = new Phrase() {
                                new Chunk("Overall Risk:", boldFont),
                                new Chunk(String.Concat(" ",GateBrief.OverallRisk," - ",GateBrief.OverallRiskReason), normalFont)
                    };
                list.Add(new ListItem(phrase));
                #endregion
                #region Overall status (issues)
                phrase = new Phrase() {
                                new Chunk("Overall status (issues):", boldFont),
                                new Chunk(String.Concat(" ",GateBrief.OverallStatus," - ", GateBrief.OverallStatusReason), normalFont)
                    };
                list.Add(new ListItem(phrase));
                #endregion
                #region Gate Readiness (Checklist completion)
                phrase = new Phrase() {
                                new Chunk("Gate Readiness (Checklist completion):", boldFont),
                                new Chunk(String.Concat(" ",GateBrief.ReadinessPct," - ",GateBrief.GateReadiness), normalFont)
                    };
                list.Add(new ListItem(phrase));
                #endregion
                #region Major Upcoming Milestones
                list.Add(new ListItem(new Phrase() { new Chunk("Major Upcoming Milestones:", boldFont) }));
                SecondLevellist = new List(List.UNORDERED, 10f);
                list.SetListSymbol("\u2022");
                foreach (var Milestone in GetListfromHtmlUL(GateBrief.Milestones))
                {
                    SecondLevellist.Add(new ListItem(new Phrase() { new Chunk(Milestone, normalFont) }));
                }
                list.Add(SecondLevellist);
                ProjectInfoTable.AddCell(CreateCellColspan(list, 2, normalFont, BaseColor.WHITE));
                #endregion
                grTable.AddCell(AddMainTableCell(ProjectInfoTable));
                #endregion
                #region Risk Assessment
                ProjectInfoTable = new PdfPTable(3);
                ProjectInfoTable.DefaultCell.Border = Rectangle.NO_BORDER;
                ProjectInfoTable.SetWidths(new float[] { 2, 0.75f, 7.25f });

                ProjectInfoTable.AddCell(CreateCellColspan("Risk Assessment", 3, boldFont, new BaseColor(154, 205, 50), Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOX));
                #region Marketing
                //list = new List(List.UNORDERED, 10f);
                //list.SetListSymbol("\u2022");
                //phrase = new Phrase() {
                //                new Chunk(GateDetails.MarketingComments, normalFont)
                //    };
                //list.Add(new ListItem(phrase));
                string MarketingColor = (GateDetails.MarketingColor == "N/A") ? GateDetails.MarketingColor : "";
                ProjectInfoTable.AddCell(CreateCellColspan("Marketing", 1, normalFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                ProjectInfoTable.AddCell(CreateCellColspan(MarketingColor, 1, normalFont, GetColor(GateDetails.MarketingColor), Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                ProjectInfoTable.AddCell(CreateCellColspan(GateDetails.MarketingComments, 1, normalFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                #endregion
                #region Sales
                //list = new List(List.UNORDERED, 10f);
                //list.SetListSymbol("\u2022");
                //phrase = new Phrase() {
                //                new Chunk(GateDetails.SalesComments, normalFont)
                //    };
                //list.Add(new ListItem(phrase));
                string SalesColor = (GateDetails.SalesColor == "N/A") ? GateDetails.SalesColor : "";
                ProjectInfoTable.AddCell(CreateCellColspan("Sales", 1, normalFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                ProjectInfoTable.AddCell(CreateCellColspan(SalesColor, 1, normalFont, GetColor(GateDetails.SalesColor), Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                ProjectInfoTable.AddCell(CreateCellColspan(GateDetails.SalesComments, 1, normalFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                #endregion
                #region Finance
                //list = new List(List.UNORDERED, 10f);
                //list.SetListSymbol("\u2022");
                //phrase = new Phrase() {
                //                new Chunk(GateDetails.FinanceComments, normalFont)
                //    };
                //list.Add(new ListItem(phrase));
                string FinanceColor = (GateDetails.FinanceColor == "N/A") ? GateDetails.FinanceColor : "";
                ProjectInfoTable.AddCell(CreateCellColspan("Finance", 1, normalFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                ProjectInfoTable.AddCell(CreateCellColspan(FinanceColor, 1, normalFont, GetColor(GateDetails.FinanceColor), Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                ProjectInfoTable.AddCell(CreateCellColspan(GateDetails.FinanceComments, 1, normalFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                #endregion
                #region InTech - R & D
                //list = new List(List.UNORDERED, 10f);
                //list.SetListSymbol("\u2022");
                //phrase = new Phrase() {
                //                new Chunk(GateDetails.RDComments, normalFont)
                //    };
                //list.Add(new ListItem(phrase));
                string RDColor = (GateDetails.RDColor == "N/A") ? GateDetails.RDColor : "";
                ProjectInfoTable.AddCell(CreateCellColspan("InTech", 1, normalFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                ProjectInfoTable.AddCell(CreateCellColspan(RDColor, 1, normalFont, GetColor(GateDetails.RDColor), Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                ProjectInfoTable.AddCell(CreateCellColspan(GateDetails.RDComments, 1, normalFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                #endregion
                #region QA
                //list = new List(List.UNORDERED, 10f);
                //list.SetListSymbol("\u2022");
                //phrase = new Phrase() {
                //                new Chunk(GateDetails.QAComments, normalFont)
                //    };
                //list.Add(new ListItem(phrase));
                string QAColor = (GateDetails.QAColor == "N/A") ? GateDetails.QAColor : "";
                ProjectInfoTable.AddCell(CreateCellColspan("QA", 1, normalFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                ProjectInfoTable.AddCell(CreateCellColspan(QAColor, 1, normalFont, GetColor(GateDetails.QAColor), Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                ProjectInfoTable.AddCell(CreateCellColspan(GateDetails.QAComments, 1, normalFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                #endregion
                #region PE
                //list = new List(List.UNORDERED, 10f);
                //list.SetListSymbol("\u2022");
                //phrase = new Phrase() {
                //                new Chunk(GateDetails.PEComments, normalFont)
                //    };
                //list.Add(new ListItem(phrase));
                string PEColor = (GateDetails.PEColor == "N/A") ? GateDetails.PEColor : "";
                ProjectInfoTable.AddCell(CreateCellColspan("PE", 1, normalFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                ProjectInfoTable.AddCell(CreateCellColspan(PEColor, 1, normalFont, GetColor(GateDetails.PEColor), Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                ProjectInfoTable.AddCell(CreateCellColspan(GateDetails.PEComments, 1, normalFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                #endregion
                #region Mfg
                //list = new List(List.UNORDERED, 10f);
                //list.SetListSymbol("\u2022");
                //phrase = new Phrase() {
                //                new Chunk(GateDetails.ManuComments, normalFont)
                //    };
                //list.Add(new ListItem(phrase));
                string ManuColor = (GateDetails.ManuColor == "N/A") ? GateDetails.ManuColor : "";
                ProjectInfoTable.AddCell(CreateCellColspan("Mfg", 1, normalFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                ProjectInfoTable.AddCell(CreateCellColspan(ManuColor, 1, normalFont, GetColor(GateDetails.ManuColor), Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                ProjectInfoTable.AddCell(CreateCellColspan(GateDetails.ManuComments, 1, normalFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                #endregion
                #region Supply Chain
                //list = new List(List.UNORDERED, 10f);
                //list.SetListSymbol("\u2022");
                //phrase = new Phrase() {
                //                new Chunk(GateDetails.SupplyChainComments, normalFont)
                //    };
                //list.Add(new ListItem(phrase));
                string SupplyChainColor = (GateDetails.SupplyChainColor == "N/A") ? GateDetails.SupplyChainColor : "";
                ProjectInfoTable.AddCell(CreateCellColspan("Supply Chain", 1, normalFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                ProjectInfoTable.AddCell(CreateCellColspan(SupplyChainColor, 1, normalFont, GetColor(GateDetails.SupplyChainColor), Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                ProjectInfoTable.AddCell(CreateCellColspan(GateDetails.SupplyChainComments, 1, normalFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                #endregion
                #region Add a blank row to make the risk assessment row sizes equal.
                ProjectInfoTable.AddCell(CreateCellColspan("    ", 3, normalFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                #endregion
                grTable.AddCell(AddMainTableCell(ProjectInfoTable));
                #endregion
                #region Impacts of Project Health / Current Situation:
                ProjectInfoTable = new PdfPTable(2);
                ProjectInfoTable.DefaultCell.Border = Rectangle.NO_BORDER;
                ProjectInfoTable.SetWidths(new int[] { 1, 1 });
                ProjectInfoTable.AddCell(CreateCellColspan("Impacts of Project Health / Current Situation:", 2, boldFont, new BaseColor(154, 205, 50), Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOX));

                list = new List(List.UNORDERED, 10f);
                list.SetListSymbol("\u2022");

                if (String.IsNullOrWhiteSpace(GateBrief.ImpactProjectHealth))
                {
                    ProjectInfoTable.AddCell(CreateCellColspan("  ", 2, normalFont, BaseColor.WHITE));
                }
                else
                {
                    var ImpactProjectHealths = GetListfromHtmlUL(String.IsNullOrWhiteSpace(GateBrief.ImpactProjectHealth) ? "   " : GateBrief.ImpactProjectHealth);

                    foreach (var ImpactProjectHealth in ImpactProjectHealths)
                    {
                        list.Add(new ListItem(new Phrase() { new Chunk(ImpactProjectHealth, normalFont) }));
                    }
                    ProjectInfoTable.AddCell(CreateCellColspan(list, 2, normalFont, BaseColor.WHITE));
                }

                grTable.AddCell(AddMainTableCell(ProjectInfoTable));
                #endregion
                #region Team's recommendation
                ProjectInfoTable = new PdfPTable(2);
                ProjectInfoTable.DefaultCell.Border = Rectangle.NO_BORDER;
                ProjectInfoTable.SetWidths(new int[] { 1, 1 });

                ProjectInfoTable.AddCell(CreateCellColspan("Team's Recommendation:", 2, boldFont, new BaseColor(238, 130, 238), Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOX));

                list = new List(List.UNORDERED, 10f);
                list.SetListSymbol("\u2022");

                if (String.IsNullOrWhiteSpace(GateBrief.TeamRecommendation))
                {
                    ProjectInfoTable.AddCell(CreateCellColspan("  ", 2, normalFont, BaseColor.WHITE));
                }
                else
                {
                    var TeamRecommendations = GetListfromHtmlUL(string.IsNullOrWhiteSpace(GateBrief.TeamRecommendation) ? "    " : GateBrief.TeamRecommendation);
                    foreach (var TeamRecommendation in TeamRecommendations)
                    {
                        list.Add(new ListItem(new Phrase() { new Chunk(TeamRecommendation, normalFont) }));
                    }
                    ProjectInfoTable.AddCell(CreateCellColspan(list, 2, normalFont, BaseColor.WHITE));
                }
                grTable.AddCell(AddMainTableCell(ProjectInfoTable));
                #endregion
                pdfDoc.Add(grTable);
                #region StageGateGenerateFinanceBriefPDF
                if (IncludeFinanceBriefInGateBrief)
                {
                    FinancialSummary = (FinancialSummary == null) ? new StageGateConsolidatedFinancialSummaryItem() : FinancialSummary;
                    pdfDoc.NewPage();
                    #region Set Fonts
                    titleFont = iTextSharp.text.FontFactory.GetFont("Calibri Body", 18, Font.BOLD, new BaseColor(8, 76, 97));
                    subTitleFont = iTextSharp.text.FontFactory.GetFont("Calibri Body", 12, Font.BOLD, new BaseColor(8, 76, 97));
                    subTitleFontNormal = iTextSharp.text.FontFactory.GetFont("Calibri Body", 10, Font.NORMAL, new BaseColor(8, 76, 97));
                    boldFont = iTextSharp.text.FontFactory.GetFont("Calibri Body", 8, Font.BOLD);
                    normalFont = iTextSharp.text.FontFactory.GetFont("Calibri Body", 8, Font.NORMAL);
                    smallFont = iTextSharp.text.FontFactory.GetFont("Calibri Body", 7, Font.NORMAL);

                    FirstFont = iTextSharp.text.FontFactory.GetFont("Calibri Body", 8, Font.NORMAL);
                    FirstBoldFont = iTextSharp.text.FontFactory.GetFont("Calibri Body", 8, Font.BOLD);
                    SecondFont = iTextSharp.text.FontFactory.GetFont("Calibri Body", 8, Font.NORMAL);
                    SecondBoldFont = iTextSharp.text.FontFactory.GetFont("Calibri Body", 8, Font.BOLD);
                    SecondFontGrey = iTextSharp.text.FontFactory.GetFont("Calibri Body", 8, Font.ITALIC, new BaseColor(150, 151, 155));
                    SecondBoldFontGrey = iTextSharp.text.FontFactory.GetFont("Calibri Body", 8, Font.BOLDITALIC, new BaseColor(150, 151, 155));
                    #endregion
                    #region Main Tabe
                    grTable = new PdfPTable(1);
                    grTable.HorizontalAlignment = Element.ALIGN_CENTER;
                    grTable.SpacingBefore = 100;
                    grTable.SpacingAfter = 30;
                    grTable.DefaultCell.Border = Rectangle.NO_BORDER;
                    grTable.SetWidths(new int[] { 1 });
                    #endregion
                    #region Logo
                    logo = iTextSharp.text.Image.GetInstance(SPContext.Current.Web.Url + "/_layouts/15/Ferrara.Compass/images/Ferrara_Logo.png");
                    logo.ScalePercent(60);

                    ProjectInfoTable = new PdfPTable(1);
                    ProjectInfoTable.DefaultCell.Border = Rectangle.NO_BORDER;
                    ProjectInfoTable.SetWidths(new int[] { 100 });
                    imageCell = new PdfPCell(logo);
                    imageCell.Border = Rectangle.NO_BORDER;
                    imageCell.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    imageCell.VerticalAlignment = Rectangle.ALIGN_BOTTOM;
                    ProjectInfoTable.AddCell(imageCell);
                    ProjectInfoTable.SpacingAfter = 20f;
                    grTable.AddCell(AddMainTableCell(ProjectInfoTable, Rectangle.NO_BORDER));
                    #endregion
                    #region Red Bottom Row
                    ProjectInfoTable = new PdfPTable(1);
                    ProjectInfoTable.DefaultCell.Border = Rectangle.NO_BORDER;
                    ProjectInfoTable.SetWidths(new int[] { 100 });
                    imageCell = new PdfPCell(new Phrase("    "));
                    imageCell.Border = Rectangle.BOTTOM_BORDER;
                    imageCell.BorderColor = BaseColor.RED;
                    imageCell.BorderWidth = 2f;
                    ProjectInfoTable.AddCell(imageCell);
                    grTable.AddCell(AddMainTableCell(ProjectInfoTable, Rectangle.NO_BORDER));
                    #endregion
                    #region Add Blank Row with No Border
                    grTable.AddCell(GetBlankRows());
                    #endregion
                    #region Brief Summary
                    ProjectInfoTable = new PdfPTable(2);
                    ProjectInfoTable.DefaultCell.Border = Rectangle.NO_BORDER;
                    ProjectInfoTable.SetWidths(new int[] { 10, 90 });

                    ProjectInfoTable.AddCell(CreateCellColspan("Brief Summary: ", 1, boldFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER, true));
                    ProjectInfoTable.AddCell(CreateCellColspan(FinancialSummary.BriefSummary, 1, normalFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                    grTable.AddCell(AddMainTableCell(ProjectInfoTable, Rectangle.NO_BORDER));
                    #endregion
                    #region Financial Summary
                    if (FinancialSummary.DispConsFinInProjBrief == "Yes")
                    {
                        #region  Total Project Financials
                        ProjectInfoTable = new PdfPTable(2);
                        ProjectInfoTable.DefaultCell.Border = Rectangle.NO_BORDER;
                        ProjectInfoTable.SetWidths(new int[] { 1, 9 });

                        ProjectInfoTable.AddCell(CreateCellColspan("in thousands", 1, smallFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM));
                        ProjectInfoTable.AddCell(CreateCellColspan("                                                                                 Total Project Financials", 1, boldFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.SpacingAfter = 20f;
                        grTable.AddCell(AddMainTableCell(ProjectInfoTable, Rectangle.NO_BORDER));
                        #endregion
                        ProjectInfoTable = new PdfPTable(9);
                        ProjectInfoTable.DefaultCell.Border = Rectangle.NO_BORDER;
                        ProjectInfoTable.SetWidths(new int[] { 13, 14, 14, 2, 14, 14, 2, 14, 13 });
                        #region Average Target Margin
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, boldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("Name : ", FinancialSummary.Name), 8, boldFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, boldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("Average Target Margin % : ", FinancialSummary.AverageTargetMargin, "%"), 8, boldFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        #endregion
                        #region Year Heading
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("Year 1", 2, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("Year 2", 2, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("Year 3", 2, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX));
                        #endregion
                        #region Total / Increment
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("Total", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("Incremental", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("Total", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("Incremental", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("Total", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("Incremental", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        #endregion
                        #region Volume (lbs.)
                        ProjectInfoTable.AddCell(CreateCellColspan("Volume (lbs.)", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(DoubleToString(FinancialSummary.VolumeTotal1), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(DoubleToString(FinancialSummary.VolumeIncremental1), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(DoubleToString(FinancialSummary.VolumeTotal2), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(DoubleToString(FinancialSummary.VolumeIncremental2), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(DoubleToString(FinancialSummary.VolumeTotal3), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(DoubleToString(FinancialSummary.VolumeIncremental3), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        #endregion
                        #region Gross Sales
                        ProjectInfoTable.AddCell(CreateCellColspan("Gross Sales", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.GrossSalesTotal1)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.GrossSalesIncremental1)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.GrossSalesTotal2)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.GrossSalesIncremental2)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.GrossSalesTotal3)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.GrossSalesIncremental3)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        #endregion
                        #region Net Sales
                        ProjectInfoTable.AddCell(CreateCellColspan("Net Sales", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.NetSalesTotal1)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.NetSalesIncremental1)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.NetSalesTotal2)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.NetSalesIncremental2)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.NetSalesTotal3)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.NetSalesIncremental3)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        #endregion
                        #region Gross Margin
                        ProjectInfoTable.AddCell(CreateCellColspan("Gross Margin", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER, true));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.GrossMarginTotal1)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.GrossMarginIncremental1)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.GrossMarginTotal2)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.GrossMarginIncremental2)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.GrossMarginTotal3)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.GrossMarginIncremental3)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        #endregion
                        #region Gross Margin %
                        ProjectInfoTable.AddCell(CreateCellColspan("Gross Margin %", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER, true));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat(DoubleToString(FinancialSummary.GrossMarginPctTotal1), "%"), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat(DoubleToString(FinancialSummary.GrossMarginPctIncremental1), "%"), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat(DoubleToString(FinancialSummary.GrossMarginPctTotal2), "%"), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat(DoubleToString(FinancialSummary.GrossMarginPctIncremental2), "%"), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat(DoubleToString(FinancialSummary.GrossMarginPctTotal3), "%"), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat(DoubleToString(FinancialSummary.GrossMarginPctIncremental3), "%"), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        #endregion
                        #region NS/LB
                        ProjectInfoTable.AddCell(CreateCellColspan("NS/LB", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.NSDollerperLB1)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.NSDollerperLB2)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.NSDollerperLB3)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        #endregion
                        #region COGS/LB
                        ProjectInfoTable.AddCell(CreateCellColspan("COGS/LB", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.COGSperLB1)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.COGSperLB2)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.COGSperLB3)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        #endregion
                        grTable.AddCell(AddMainTableCell(ProjectInfoTable));
                        #region Analysis included in Consolidated Financial Summary
                        ProjectInfoTable = new PdfPTable(1);
                        ProjectInfoTable.DefaultCell.Border = Rectangle.NO_BORDER;
                        ProjectInfoTable.SetWidths(new int[] { 100 });

                        phrase = new Phrase();
                        phrase.Add(new Chunk("Analyses included in Consolidated Financial Summary: ", SecondBoldFontGrey));
                        phrase.Add(new Chunk(FinancialSummary.Analysesincluded, SecondFontGrey));

                        PdfPCell AnalysisIncludedcell = new PdfPCell(phrase);
                        AnalysisIncludedcell.Border = Rectangle.NO_BORDER;
                        AnalysisIncludedcell.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        AnalysisIncludedcell.VerticalAlignment = Rectangle.ALIGN_TOP;
                        ProjectInfoTable.AddCell(AnalysisIncludedcell);
                        grTable.AddCell(AddMainTableCell(ProjectInfoTable, Rectangle.NO_BORDER));
                        #endregion
                    }
                    #endregion
                    #region Add Blank Row with No Border
                    grTable.AddCell(GetBlankRows());
                    #endregion
                    #region Financial Analysis
                    foreach (var analysis in FinancialAnalysis)
                    {
                        if (analysis.PLsinProjectBrief == "Yes")
                        {
                            #region Financial Analysis
                            BaseColor AnalysisBackGround = new BaseColor(255, 255, 255);

                            ProjectInfoTable = new PdfPTable(9);
                            ProjectInfoTable.DefaultCell.Border = Rectangle.NO_BORDER;
                            ProjectInfoTable.SetWidths(new int[] { 14, 14, 14, 1, 14, 14, 1, 14, 14 });
                            #region Name
                            ProjectInfoTable.AddCell(CreateCellColspan(string.IsNullOrEmpty(analysis.AnalysisName) ? "  " : analysis.AnalysisName, 9, SecondFont, BaseColor.PINK, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                            #endregion
                            #region Customer Channel
                            phrase = new Phrase();
                            phrase.Add(new Chunk("Customer/Channel: ", SecondBoldFont));
                            phrase.Add(new Chunk(analysis.CustomerChannel, SecondFont));

                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, boldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(phrase, 4, SecondBoldFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Rectangle.NO_BORDER, true));
                            #endregion
                            #region Brand/Season
                            phrase = new Phrase();
                            phrase.Add(new Chunk("Brand/Season: ", SecondBoldFont));
                            phrase.Add(new Chunk(analysis.BrandSeason, SecondFont));

                            ProjectInfoTable.AddCell(CreateCellColspan(phrase, 4, SecondBoldFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Rectangle.NO_BORDER, true));
                            #endregion
                            #region FG#
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, boldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER, true));
                            phrase = new Phrase();
                            phrase.Add(new Chunk("FG#: ", SecondBoldFont));
                            phrase.Add(new Chunk(analysis.FGNumber, SecondFont));

                            ProjectInfoTable.AddCell(CreateCellColspan(phrase, 2, SecondBoldFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Rectangle.NO_BORDER, true));
                            #endregion
                            #region Target Margin %
                            phrase = new Phrase();
                            phrase.Add(new Chunk("Target Margin %: ", SecondBoldFont));
                            phrase.Add(new Chunk(string.Concat(DoubleToString(analysis.TargetMarginPct), "%"), SecondFont));

                            ProjectInfoTable.AddCell(CreateCellColspan(phrase, 2, SecondBoldFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Rectangle.NO_BORDER, true));
                            #endregion
                            #region Product Form
                            phrase = new Phrase();
                            phrase.Add(new Chunk("Product Form: ", SecondBoldFont));
                            phrase.Add(new Chunk(analysis.ProductForm, SecondFont));

                            ProjectInfoTable.AddCell(CreateCellColspan(phrase, 4, SecondBoldFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Rectangle.NO_BORDER, true));
                            #endregion
                            #region Year Heading
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("Year 1", 2, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("Year 2", 2, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("Year 3", 2, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX));
                            #endregion
                            #region Total / Increment
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("Total", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("Incremental", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("Total", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("Incremental", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("Total", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("Incremental", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                            #endregion
                            #region Volume (lbs.)
                            ProjectInfoTable.AddCell(CreateCellColspan("Volume (lbs.)", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(DoubleToString(analysis.VolumeTotal1), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(DoubleToString(analysis.VolumeIncremental1), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(DoubleToString(analysis.VolumeTotal2), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(DoubleToString(analysis.VolumeIncremental2), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(DoubleToString(analysis.VolumeTotal3), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(DoubleToString(analysis.VolumeIncremental3), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                            #endregion
                            #region Gross Sales
                            ProjectInfoTable.AddCell(CreateCellColspan("Gross Sales", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.GrossSalesTotal1)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.GrossSalesIncremental1)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.GrossSalesTotal2)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.GrossSalesIncremental2)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.GrossSalesTotal3)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.GrossSalesIncremental3)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                            #endregion
                            #region Net Sales
                            ProjectInfoTable.AddCell(CreateCellColspan("Net Sales", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.NetSalesTotal1)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.NetSalesIncremental1)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOTTOM_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.NetSalesTotal2)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.NetSalesIncremental2)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOTTOM_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.NetSalesTotal3)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.NetSalesIncremental3)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                            #endregion
                            #region Gross Margin
                            ProjectInfoTable.AddCell(CreateCellColspan("Gross Margin", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.GrossMarginTotal1)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.GrossMarginIncremental1)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_RIGHT, Element.ALIGN_CENTER, Rectangle.BOTTOM_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.GrossMarginTotal2)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.GrossMarginIncremental2)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOTTOM_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.GrossMarginTotal3)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.GrossMarginIncremental3)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                            #endregion
                            #region Gross Margin %
                            ProjectInfoTable.AddCell(CreateCellColspan("Gross Margin %", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER, true));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat(DoubleToString(analysis.GrossMarginPctTotal1), "%"), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat(DoubleToString(analysis.GrossMarginPctIncremental1), "%"), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOTTOM_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat(DoubleToString(analysis.GrossMarginPctTotal2), "%"), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat(DoubleToString(analysis.GrossMarginPctIncremental2), "%"), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOTTOM_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat(DoubleToString(analysis.GrossMarginPctTotal3), "%"), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat(DoubleToString(analysis.GrossMarginPctIncremental3), "%"), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                            #endregion
                            #region NS$/lb
                            ProjectInfoTable.AddCell(CreateCellColspan("NS/LB", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.TOP_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.NSDollerperLB1)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.TOP_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.TOP_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.TOP_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.NSDollerperLB2)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.TOP_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.TOP_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.TOP_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.NSDollerperLB3)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.TOP_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.TOP_BORDER));
                            #endregion
                            #region COGS/LB
                            ProjectInfoTable.AddCell(CreateCellColspan("COGS/LB", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.COGSperLB1)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.COGSperLB2)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.COGSperLB3)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                            #endregion
                            #region Truckload/retail selling unit
                            ProjectInfoTable.AddCell(CreateCellColspan("Truckload/retail selling unit", 1, iTextSharp.text.FontFactory.GetFont("Calibri Body", 8, Font.BOLD), AnalysisBackGround, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER, true));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.TruckldPricePrRtlSllngUt1)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.TruckldPricePrRtlSllngUt2)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.TruckldPricePrRtlSllngUt3)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                            #endregion
                            #region Assumptions
                            ProjectInfoTable.AddCell(CreateCellColspan("Assumptions", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_RIGHT, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(analysis.Assumptions1, 2, SecondFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(analysis.Assumptions2, 2, SecondFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                            ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                            ProjectInfoTable.AddCell(CreateCellColspan(analysis.Assumptions3, 2, SecondFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                            #endregion
                            grTable.AddCell(AddMainTableCell(ProjectInfoTable, Rectangle.NO_BORDER));
                            #region Add Blank Row with No Border
                            grTable.AddCell(GetBlankRows());
                            #endregion
                            #endregion
                        }
                    }
                    #endregion
                    pdfDoc.Add(grTable);
                }
                #endregion
                pdfDoc.Close();
                #endregion
                #region Create File
                List<FileAttribute> uploadFile = new List<FileAttribute>();
                FileAttribute file = new FileAttribute();
                file.FileName = utilService.CreateSafeFileName(GlobalConstants.DOCTYPE_StageGateBriefPDF + "_" + GateNo + "_" + BriefNo) + ".pdf";
                file.FileContent = output.ToArray();
                file.DocType = GlobalConstants.DOCTYPE_StageGateBriefPDF;
                uploadFile.Add(file);
                stageGateGeneralService.UploadStageGateDocument(uploadFile, stageGateItem.ProjectNumber, GlobalConstants.DOCTYPE_StageGateFinanceBriefPDF, GateNo.ToString(), BriefNo.ToString());

                #endregion
                return file;
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.IPF.ToString(), "StageGateGenerateBriefPDF - StageGateListItemId = " + StageGateListItemId + ", GateNo = " + GateNo + ", BriefNo = " + BriefNo + " Export Error");
                return new FileAttribute();
            }
        }
        public string StageGateGenerateFinanceBriefPDF(string ProjectNo, int StageGateListItemId, int GateNo, int BriefNo)
        {
            try
            {
                #region StageGateGenerateFinanceBriefPDF
                #region Get Data from Lists
                if (!(StageGateListItemId > 0))
                {
                    return null;
                }

                var stageGateItem = stageGateCreateProjectService.GetStageGateProjectItem(StageGateListItemId);
                var GateDetails = stageGateGeneralService.GetStageGateGateItem(StageGateListItemId, GateNo);
                var GateBriefs = stageGateGeneralService.GetStageGateBriefItem(StageGateListItemId, GateNo);
                var FinancialSummary = stageGateFinancialServices.GetStageGateConsolidatedFinancialSummaryItem(StageGateListItemId, GateNo.ToString(), BriefNo.ToString());
                FinancialSummary = (FinancialSummary == null) ? new StageGateConsolidatedFinancialSummaryItem() : FinancialSummary;
                var FinancialAnalysis = stageGateFinancialServices.GetAllStageGateFinancialAnalysisItemsByGateAndBriefNumber(StageGateListItemId, GateNo.ToString(), BriefNo.ToString());

                var GateBrief = GateBriefs.Where(i => i.BriefNo == BriefNo).FirstOrDefault();
                if (stageGateItem == null)
                {
                    return null;
                }
                #endregion
                #region Set Fonts
                titleFont = iTextSharp.text.FontFactory.GetFont("Calibri Body", 18, Font.BOLD, new BaseColor(8, 76, 97));
                subTitleFont = iTextSharp.text.FontFactory.GetFont("Calibri Body", 12, Font.BOLD, new BaseColor(8, 76, 97));
                subTitleFontNormal = iTextSharp.text.FontFactory.GetFont("Calibri Body", 10, Font.NORMAL, new BaseColor(8, 76, 97));
                boldFont = iTextSharp.text.FontFactory.GetFont("Calibri Body", 8, Font.BOLD);
                normalFont = iTextSharp.text.FontFactory.GetFont("Calibri Body", 8, Font.NORMAL);
                smallFont = iTextSharp.text.FontFactory.GetFont("Calibri Body", 7, Font.NORMAL);


                FirstFont = iTextSharp.text.FontFactory.GetFont("Calibri Body", 8, Font.NORMAL);
                FirstBoldFont = iTextSharp.text.FontFactory.GetFont("Calibri Body", 8, Font.BOLD);
                SecondFont = iTextSharp.text.FontFactory.GetFont("Calibri Body", 8, Font.NORMAL);
                SecondBoldFont = iTextSharp.text.FontFactory.GetFont("Calibri Body", 8, Font.BOLD);
                SecondFontGrey = iTextSharp.text.FontFactory.GetFont("Calibri Body", 8, Font.ITALIC, new BaseColor(150, 151, 155));
                SecondBoldFontGrey = iTextSharp.text.FontFactory.GetFont("Calibri Body", 8, Font.BOLDITALIC, new BaseColor(150, 151, 155));
                #endregion
                #region Open Documents
                string fileNames = string.Empty;
                // Create a ITextSharp Document object
                // Use the landscape Page Size which measures width =11" and Height=7.1" 
                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(new Rectangle(792, 511.2f), 5, 5, 20, 10);

                // Create a new PdfWriter object, specifying the output stream
                MemoryStream output = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, output);

                // Open the Document for writing
                pdfDoc.Open();
                #endregion
                #region Main Tabe
                PdfPTable grTable = new PdfPTable(1);
                grTable.HorizontalAlignment = Element.ALIGN_CENTER;
                grTable.SpacingBefore = 100;
                grTable.SpacingAfter = 30;
                grTable.DefaultCell.Border = Rectangle.NO_BORDER;
                grTable.SetWidths(new int[] { 1 });
                #endregion
                #region Logo
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(SPContext.Current.Web.Url + "/_layouts/15/Ferrara.Compass/images/Ferrara_Logo.png");
                logo.ScalePercent(60);

                PdfPTable ProjectInfoTable = new PdfPTable(1);
                ProjectInfoTable.DefaultCell.Border = Rectangle.NO_BORDER;
                ProjectInfoTable.SetWidths(new int[] { 100 });
                PdfPCell imageCell = new PdfPCell(logo);
                imageCell.Border = Rectangle.NO_BORDER;
                imageCell.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                imageCell.VerticalAlignment = Rectangle.ALIGN_BOTTOM;
                ProjectInfoTable.AddCell(imageCell);
                ProjectInfoTable.SpacingAfter = 20f;
                grTable.AddCell(AddMainTableCell(ProjectInfoTable, Rectangle.NO_BORDER));
                #endregion
                #region Red Bottom Row
                ProjectInfoTable = new PdfPTable(1);
                ProjectInfoTable.DefaultCell.Border = Rectangle.NO_BORDER;
                ProjectInfoTable.SetWidths(new int[] { 100 });
                imageCell = new PdfPCell(new Phrase("    "));
                imageCell.Border = Rectangle.BOTTOM_BORDER;
                imageCell.BorderColor = BaseColor.RED;
                imageCell.BorderWidth = 2f;
                ProjectInfoTable.AddCell(imageCell);
                grTable.AddCell(AddMainTableCell(ProjectInfoTable, Rectangle.NO_BORDER));
                #endregion
                #region Add Blank Row with No Border
                grTable.AddCell(GetBlankRows());
                #endregion
                #region Brief Summary
                ProjectInfoTable = new PdfPTable(2);
                ProjectInfoTable.DefaultCell.Border = Rectangle.NO_BORDER;
                ProjectInfoTable.SetWidths(new int[] { 10, 90 });

                ProjectInfoTable.AddCell(CreateCellColspan("Brief Summary: ", 1, boldFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER, true));
                ProjectInfoTable.AddCell(CreateCellColspan(FinancialSummary.BriefSummary, 1, normalFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                grTable.AddCell(AddMainTableCell(ProjectInfoTable, Rectangle.NO_BORDER));
                #endregion
                #region Financial Summary
                if (FinancialSummary.DispConsFinInProjBrief == "Yes")
                {
                    #region  Total Project Financials
                    ProjectInfoTable = new PdfPTable(2);
                    ProjectInfoTable.DefaultCell.Border = Rectangle.NO_BORDER;
                    ProjectInfoTable.SetWidths(new int[] { 1, 9 });

                    ProjectInfoTable.AddCell(CreateCellColspan("in thousands", 1, smallFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM));
                    ProjectInfoTable.AddCell(CreateCellColspan("                                                                                 Total Project Financials", 1, boldFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    ProjectInfoTable.SpacingAfter = 20f;
                    grTable.AddCell(AddMainTableCell(ProjectInfoTable, Rectangle.NO_BORDER));
                    #endregion
                    ProjectInfoTable = new PdfPTable(9);
                    ProjectInfoTable.DefaultCell.Border = Rectangle.NO_BORDER;
                    ProjectInfoTable.SetWidths(new int[] { 13, 14, 14, 2, 14, 14, 2, 14, 13 });
                    #region Average Target Margin
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, boldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("Name : ", FinancialSummary.Name), 8, boldFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, boldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("Average Target Margin % : ", FinancialSummary.AverageTargetMargin, "%"), 8, boldFont, BaseColor.WHITE, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                    #endregion
                    #region Year Heading
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("Year 1", 2, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX));
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("Year 2", 2, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX));
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("Year 3", 2, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX));
                    #endregion
                    #region Total / Increment
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("Total", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("Incremental", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("Total", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("Incremental", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("Total", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("Incremental", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    #endregion
                    #region Volume (lbs.)
                    ProjectInfoTable.AddCell(CreateCellColspan("Volume (lbs.)", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(DoubleToString(FinancialSummary.VolumeTotal1), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(DoubleToString(FinancialSummary.VolumeIncremental1), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(DoubleToString(FinancialSummary.VolumeTotal2), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(DoubleToString(FinancialSummary.VolumeIncremental2), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(DoubleToString(FinancialSummary.VolumeTotal3), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(DoubleToString(FinancialSummary.VolumeIncremental3), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                    #endregion
                    #region Gross Sales
                    ProjectInfoTable.AddCell(CreateCellColspan("Gross Sales", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.GrossSalesTotal1)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.GrossSalesIncremental1)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.GrossSalesTotal2)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.GrossSalesIncremental2)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.GrossSalesTotal3)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.GrossSalesIncremental3)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                    #endregion
                    #region Net Sales
                    ProjectInfoTable.AddCell(CreateCellColspan("Net Sales", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.NetSalesTotal1)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.NetSalesIncremental1)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOTTOM_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.NetSalesTotal2)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.NetSalesIncremental2)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOTTOM_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.NetSalesTotal3)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.NetSalesIncremental3)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                    #endregion
                    #region Gross Margin
                    ProjectInfoTable.AddCell(CreateCellColspan("Gross Margin", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER, true));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.GrossMarginTotal1)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.GrossMarginIncremental1)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOTTOM_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.GrossMarginTotal2)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.GrossMarginIncremental2)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOTTOM_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.GrossMarginTotal3)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.GrossMarginIncremental3)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                    #endregion
                    #region Gross Margin %
                    ProjectInfoTable.AddCell(CreateCellColspan("Gross Margin %", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER, true));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat(DoubleToString(FinancialSummary.GrossMarginPctTotal1), "%"), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat(DoubleToString(FinancialSummary.GrossMarginPctIncremental1), "%"), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOTTOM_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat(DoubleToString(FinancialSummary.GrossMarginPctTotal2), "%"), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat(DoubleToString(FinancialSummary.GrossMarginPctIncremental2), "%"), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOTTOM_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat(DoubleToString(FinancialSummary.GrossMarginPctTotal3), "%"), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat(DoubleToString(FinancialSummary.GrossMarginPctIncremental3), "%"), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                    #endregion
                    #region NS/LB
                    ProjectInfoTable.AddCell(CreateCellColspan("NS/LB", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.NSDollerperLB1)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.NSDollerperLB2)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.NSDollerperLB3)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                    #endregion
                    #region COGS/LB
                    ProjectInfoTable.AddCell(CreateCellColspan("COGS/LB", 1, SecondBoldFont, BaseColor.WHITE, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.COGSperLB1)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.COGSperLB2)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(FinancialSummary.COGSperLB3)), 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                    ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, BaseColor.WHITE, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                    #endregion
                    grTable.AddCell(AddMainTableCell(ProjectInfoTable));
                    #region Analysis included in Consolidated Financial Summary
                    ProjectInfoTable = new PdfPTable(1);
                    ProjectInfoTable.DefaultCell.Border = Rectangle.NO_BORDER;
                    ProjectInfoTable.SetWidths(new int[] { 100 });

                    var phrase = new Phrase();
                    phrase.Add(new Chunk("Analyses included in Consolidated Financial Summary: ", SecondBoldFontGrey));
                    phrase.Add(new Chunk(FinancialSummary.Analysesincluded, SecondFontGrey));

                    PdfPCell AnalysisIncludedcell = new PdfPCell(phrase);
                    AnalysisIncludedcell.Border = Rectangle.NO_BORDER;
                    AnalysisIncludedcell.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    AnalysisIncludedcell.VerticalAlignment = Rectangle.ALIGN_TOP;
                    ProjectInfoTable.AddCell(AnalysisIncludedcell);
                    grTable.AddCell(AddMainTableCell(ProjectInfoTable, Rectangle.NO_BORDER));
                    #endregion 
                }
                #endregion
                #region Add Blank Row with No Border
                grTable.AddCell(GetBlankRows());
                #endregion
                #region Financial Analysis
                foreach (var analysis in FinancialAnalysis)
                {
                    if (analysis.PLsinProjectBrief == "Yes")
                    {
                        #region Financial Analysis
                        BaseColor AnalysisBackGround = new BaseColor(255, 255, 255);

                        ProjectInfoTable = new PdfPTable(9);
                        ProjectInfoTable.DefaultCell.Border = Rectangle.NO_BORDER;
                        ProjectInfoTable.SetWidths(new int[] { 14, 14, 14, 1, 14, 14, 1, 14, 14 });
                        #region Name
                        ProjectInfoTable.AddCell(CreateCellColspan(string.IsNullOrEmpty(analysis.AnalysisName) ? "  " : analysis.AnalysisName, 9, SecondFont, BaseColor.PINK, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        #endregion
                        #region Customer Channel
                        Phrase phrase = new Phrase();
                        phrase.Add(new Chunk("Customer/Channel: ", SecondBoldFont));
                        phrase.Add(new Chunk(analysis.CustomerChannel, SecondFont));

                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, boldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(phrase, 4, SecondBoldFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Rectangle.NO_BORDER, true));
                        #endregion
                        #region Brand/Season
                        phrase = new Phrase();
                        phrase.Add(new Chunk("Brand/Season: ", SecondBoldFont));
                        phrase.Add(new Chunk(analysis.BrandSeason, SecondFont));

                        ProjectInfoTable.AddCell(CreateCellColspan(phrase, 4, SecondBoldFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Rectangle.NO_BORDER, true));
                        #endregion
                        #region FG#
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, boldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER, true));
                        phrase = new Phrase();
                        phrase.Add(new Chunk("FG#: ", SecondBoldFont));
                        phrase.Add(new Chunk(analysis.FGNumber, SecondFont));

                        ProjectInfoTable.AddCell(CreateCellColspan(phrase, 2, SecondBoldFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Rectangle.NO_BORDER, true));
                        #endregion
                        #region Target Margin %
                        phrase = new Phrase();
                        phrase.Add(new Chunk("Target Margin %: ", SecondBoldFont));
                        phrase.Add(new Chunk(string.Concat(DoubleToString(analysis.TargetMarginPct), "%"), SecondFont));

                        ProjectInfoTable.AddCell(CreateCellColspan(phrase, 2, SecondBoldFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Rectangle.NO_BORDER, true));
                        #endregion
                        #region Product Form
                        phrase = new Phrase();
                        phrase.Add(new Chunk("Product Form: ", SecondBoldFont));
                        phrase.Add(new Chunk(analysis.ProductForm, SecondFont));

                        ProjectInfoTable.AddCell(CreateCellColspan(phrase, 4, SecondBoldFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Rectangle.NO_BORDER, true));
                        #endregion
                        #region Year Heading
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("Year 1", 2, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("Year 2", 2, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("Year 3", 2, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX));
                        #endregion
                        #region Total / Increment
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("Total", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("Incremental", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("Total", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("Incremental", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("Total", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("Incremental", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        #endregion
                        #region Volume (lbs.)
                        ProjectInfoTable.AddCell(CreateCellColspan("Volume (lbs.)", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(DoubleToString(analysis.VolumeTotal1), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(DoubleToString(analysis.VolumeIncremental1), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(DoubleToString(analysis.VolumeTotal2), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(DoubleToString(analysis.VolumeIncremental2), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(DoubleToString(analysis.VolumeTotal3), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(DoubleToString(analysis.VolumeIncremental3), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        #endregion
                        #region Gross Sales
                        ProjectInfoTable.AddCell(CreateCellColspan("Gross Sales", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.GrossSalesTotal1)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.GrossSalesIncremental1)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.GrossSalesTotal2)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.GrossSalesIncremental2)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.GrossSalesTotal3)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.GrossSalesIncremental3)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        #endregion
                        #region Net Sales
                        ProjectInfoTable.AddCell(CreateCellColspan("Net Sales", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.NetSalesTotal1)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.NetSalesIncremental1)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.NetSalesTotal2)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.NetSalesIncremental2)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.NetSalesTotal3)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.NetSalesIncremental3)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        #endregion
                        #region Gross Margin
                        ProjectInfoTable.AddCell(CreateCellColspan("Gross Margin", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.GrossMarginTotal1)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.GrossMarginIncremental1)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_RIGHT, Element.ALIGN_CENTER, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.GrossMarginTotal2)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.GrossMarginIncremental2)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.GrossMarginTotal3)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.GrossMarginIncremental3)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        #endregion
                        #region Gross Margin %
                        ProjectInfoTable.AddCell(CreateCellColspan("Gross Margin %", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER, true));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat(DoubleToString(analysis.GrossMarginPctTotal1), "%"), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat(DoubleToString(analysis.GrossMarginPctIncremental1), "%"), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat(DoubleToString(analysis.GrossMarginPctTotal2), "%"), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat(DoubleToString(analysis.GrossMarginPctIncremental2), "%"), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat(DoubleToString(analysis.GrossMarginPctTotal3), "%"), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat(DoubleToString(analysis.GrossMarginPctIncremental3), "%"), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                        #endregion
                        #region NS$/lb
                        ProjectInfoTable.AddCell(CreateCellColspan("NS/LB", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.TOP_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.NSDollerperLB1)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.TOP_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.TOP_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.TOP_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.NSDollerperLB2)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.TOP_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.TOP_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.TOP_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.NSDollerperLB3)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.TOP_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.TOP_BORDER));
                        #endregion
                        #region COGS/LB
                        ProjectInfoTable.AddCell(CreateCellColspan("COGS/LB", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.COGSperLB1)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.COGSperLB2)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.COGSperLB3)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        #endregion
                        #region Truckload/retail selling unit
                        ProjectInfoTable.AddCell(CreateCellColspan("Truckload/retail selling unit", 1, iTextSharp.text.FontFactory.GetFont("Calibri Body", 8, Font.BOLD), AnalysisBackGround, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER, true));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.TruckldPricePrRtlSllngUt1)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.TruckldPricePrRtlSllngUt2)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(string.Concat("$", DoubleToString(analysis.TruckldPricePrRtlSllngUt3)), 1, SecondFont, AnalysisBackGround, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        #endregion
                        #region Assumptions
                        ProjectInfoTable.AddCell(CreateCellColspan("Assumptions", 1, SecondBoldFont, AnalysisBackGround, Element.ALIGN_RIGHT, Element.ALIGN_CENTER, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(analysis.Assumptions1, 2, SecondFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(analysis.Assumptions2, 2, SecondFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                        ProjectInfoTable.AddCell(CreateCellColspan("", 1, SecondFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        ProjectInfoTable.AddCell(CreateCellColspan(analysis.Assumptions3, 2, SecondFont, AnalysisBackGround, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                        #endregion
                        grTable.AddCell(AddMainTableCell(ProjectInfoTable, Rectangle.NO_BORDER));
                        #region Add Blank Row with No Border
                        grTable.AddCell(GetBlankRows());
                        #endregion
                        #endregion 
                    }
                }
                #endregion
                pdfDoc.Add(grTable);
                pdfDoc.Close();
                #endregion
                #region Create the PDF
                List<FileAttribute> uploadFile = new List<FileAttribute>();
                FileAttribute file = new FileAttribute();
                file.FileName = utilService.CreateSafeFileName(GlobalConstants.DOCTYPE_StageGateFinanceBriefPDF + "_" + GateNo + "_" + BriefNo) + ".pdf";
                file.FileContent = output.ToArray();
                file.DocType = GlobalConstants.DOCTYPE_StageGateFinanceBriefPDF;
                uploadFile.Add(file);
                stageGateGeneralService.UploadStageGateDocument(uploadFile, ProjectNo, GlobalConstants.DOCTYPE_StageGateFinanceBriefPDF, GateNo.ToString(), BriefNo.ToString());
                return GlobalConstants.DOCTYPE_StageGateFinanceBriefPDF + "-" + GateNo + "-" + BriefNo + ".pdf";
                #endregion
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.IPF.ToString(), "StageGateGenerateFinanceBriefPDF - StageGateListItemId = " + StageGateListItemId + ", GateNo = " + GateNo + ", BriefNo = " + BriefNo + "Export Error: ");
                return null;
            }
        }
        #endregion
        #region Private Helper Methods
        private static Paragraph GetBlankRows(int Count = 1)
        {
            Paragraph para = new Paragraph();

            while (Count > 0)
            {
                para.Add(new Paragraph(new Chunk("\n")));
                Count--;
            }
            return para;
        }
        private static string DoubleToString(double value)
        {
            string strvalue;
            if (((double)Math.Abs(value) % 1) > 0)
            {
                strvalue = value.ToString("N");
            }
            else
            {
                strvalue = value.ToString("N0");
            }

            return strvalue;
        }
        private static List<string> GetListfromHtmlUL(string text)
        {
            if (text == HttpUtility.HtmlEncode(text) && text == HttpUtility.HtmlDecode(text))
            {
                return new List<string>() { text };
            }

            if (text != HttpUtility.HtmlDecode(text))
            {
                text = HttpUtility.HtmlDecode(text);
            }

            var items = new List<string>();
            text = Regex.Replace(text, "<strong>", "");
            text = Regex.Replace(text, "</strong>", "");
            text = Regex.Replace(text, "<em>", "");
            text = Regex.Replace(text, "</em>", "");
            text = Regex.Replace(text, "</span>", "");
            int startpos = 0;
            int index1 = 0;
            int index2 = 0;
            bool found = true;
            string OriginalText = text;
            do
            {
                index1 = OriginalText.IndexOf("<span", startpos);

                if (index1 == -1)
                {
                    found = false;
                }
                else
                {
                    index2 = OriginalText.IndexOf(">", index1);
                    text = OriginalText.Remove(index1, index2 - index1 + 1);
                    found = true;
                    startpos = index2;
                }
            } while (found);
            text = Regex.Replace(text, "<span style=\"text-decoration: underline;\" data-mce-style=\"text-decoration: underline;\">", "");
            var text1 = Regex.Replace(text, "</(.*?)>", "</GetListfromHtmlUL>");
            var text2 = Regex.Replace(text1, "(?!</(.*?)>)<(.*?)>", "<GetListfromHtmlUL>");
            var text3 = text2
                            .Replace("> <", "><")
                            .Replace("> </", ">< ")
                            .Replace(">  <", "><")
                            .Replace(">  </", ">< ")
                            .Replace("<GetListfromHtmlUL><GetListfromHtmlUL>", "<GetListfromHtmlUL>")
                            .Replace("</GetListfromHtmlUL></GetListfromHtmlUL>", "</GetListfromHtmlUL>");
            foreach (Match match in Regex.Matches(text3, "<GetListfromHtmlUL>(.*?)</GetListfromHtmlUL>"))
            {
                if (match.Groups[1].Value != "&nbsp;")
                {
                    items.Add(match.Groups[1].Value.Replace("&nbsp;", "").Replace("/<GetListfromHtmlUL>", "").Replace("<GetListfromHtmlUL>", ""));
                }
            }
            return items;
        }
        private static BaseColor GetColor(string color)
        {
            BaseColor Cellcolor = BaseColor.WHITE;

            if (color?.ToLower() == "white")
            {
                Cellcolor = BaseColor.WHITE;
            }
            else if (color?.ToLower() == "red")
            {
                Cellcolor = new BaseColor(255, 0, 0);
            }
            else if (color?.ToLower() == "green")
            {
                Cellcolor = new BaseColor(0, 176, 80);
            }
            else if (color?.ToLower() == "yellow")
            {
                Cellcolor = new BaseColor(255, 255, 0);
            }
            return Cellcolor;
        }
        private static string GetNextStage(string Stage)
        {
            var NextStage = string.Empty;
            if (string.IsNullOrEmpty(Stage)) { Stage = string.Empty; }
            if (Stage == GlobalConstants.StageLookup_Industrialize)
            {
                NextStage = "Go to Launch";
            }
            else if (Stage.Contains("Develop"))
            {
                NextStage = "Go to Industrialize";
            }
            else
            {
                NextStage = "Go to Develop";
            }

            return NextStage;
        }
        private static PdfPCell AddMainTableCell(PdfPTable ProjectInfoTable, int Border = Rectangle.BOX)
        {
            PdfPCell cell = new PdfPCell(ProjectInfoTable);
            cell.BorderColor = BaseColor.BLACK;
            cell.Border = Border;
            cell.BorderWidth = 1;
            return cell;
        }
        private PdfPCell CreateCell(string value, iTextSharp.text.Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(value, font));
            cell.Border = Rectangle.NO_BORDER;
            return cell;
        }
        private PdfPCell CreateCellColspan(string value, int colspan, iTextSharp.text.Font font, BaseColor BackgroundColor, int HorizontalAlign = Element.ALIGN_LEFT, int VerticalAlignment = Element.ALIGN_CENTER, int Border = Rectangle.NO_BORDER, bool Wrap = false)
        {
            PdfPCell cell = new PdfPCell(new Phrase(value, font));
            cell.Border = Border;
            cell.Colspan = colspan;
            cell.BackgroundColor = BackgroundColor;
            cell.HorizontalAlignment = HorizontalAlign;
            cell.VerticalAlignment = VerticalAlignment;
            cell.BorderWidth = 1;
            cell.NoWrap = Wrap;
            return cell;
        }

        private PdfPCell CreateCellColspan(Phrase Phrase, int colspan, iTextSharp.text.Font font, BaseColor BackgroundColor, int HorizontalAlign = Element.ALIGN_LEFT, int VerticalAlignment = Element.ALIGN_CENTER, int Border = Rectangle.NO_BORDER, bool Wrap = false)
        {
            PdfPCell cell = new PdfPCell(Phrase);
            cell.Border = Border;
            cell.Colspan = colspan;
            cell.BackgroundColor = BackgroundColor;
            cell.HorizontalAlignment = HorizontalAlign;
            cell.VerticalAlignment = VerticalAlignment;
            cell.BorderWidth = 1;
            cell.NoWrap = Wrap;
            return cell;
        }
        private PdfPCell CreateCellColspan(List list, int colspan, iTextSharp.text.Font font, BaseColor BackgroundColor, int HorizontalAlign = Element.ALIGN_LEFT, int VerticalAlignment = Element.ALIGN_CENTER, int Border = Rectangle.NO_BORDER)
        {
            PdfPCell cell = new PdfPCell();
            cell.AddElement(list);
            cell.Border = Border;
            cell.Colspan = colspan;
            cell.BackgroundColor = BackgroundColor;
            cell.HorizontalAlignment = HorizontalAlign;
            cell.VerticalAlignment = VerticalAlignment;
            return cell;
        }
        private string FormatRowNumber(int rowNumber)
        {
            if (rowNumber < 10)
                return " " + rowNumber.ToString() + ". ";

            return rowNumber.ToString() + ". ";
        }
        private PdfPCell CreateRow(bool alternateRow, string value)
        {
            if (alternateRow)
                return CreateAlternateRow(value);

            return CreateRow(value);
        }
        private PdfPCell CreateRow(string value)
        {
            PdfPCell cell = new PdfPCell(new Phrase(value, normalFont));
            cell.Border = Rectangle.NO_BORDER;
            return cell;
        }
        private PdfPCell CreateAlternateRow(string value)
        {
            PdfPCell cell = new PdfPCell(new Phrase(value, normalFont));
            cell.Border = Rectangle.NO_BORDER;
            cell.BackgroundColor = alternateColor;
            return cell;
        }
        private PdfPCell CreateAlternateRowColSpan(string value, int colspan)
        {
            PdfPCell cell = new PdfPCell(new Phrase(value, normalFont));
            cell.Border = Rectangle.NO_BORDER;
            cell.BackgroundColor = alternateColor;
            cell.Colspan = colspan;
            return cell;
        }
        private PdfPCell CreateRowColspan(string value, int colspan)
        {
            PdfPCell cell = new PdfPCell(new Phrase(value, normalFont));
            cell.Border = Rectangle.NO_BORDER;
            cell.Colspan = colspan;
            return cell;
        }
        private PdfPCell CreateRowColspanWithBorderBoldCenter(string value, int colspan)
        {
            PdfPCell cell = new PdfPCell(new Phrase(value, boldFont));
            cell.Colspan = colspan;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = BaseColor.GRAY;
            return cell;
        }
        private PdfPCell CreateRowColspanWithImage(iTextSharp.text.Image image, int colspan)
        {
            image.ScalePercent(60);
            PdfPCell cell = new PdfPCell(image, true);
            cell.Padding = 20;
            cell.Colspan = colspan;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            return cell;
        }
        private PdfPCell CreateRowColspanBold(string value, int colspan)
        {
            PdfPCell cell = new PdfPCell(new Phrase(value, boldFont));
            cell.Colspan = colspan;
            cell.Border = Rectangle.NO_BORDER;
            return cell;
        }
        private string GetFileNames(string projectNumber, string docType)
        {
            var projectFiles = utilService.GetUploadedCompassFilesByDocType(projectNumber, docType);
            string fileNames = string.Empty;
            foreach (FileAttribute fileAttribute in projectFiles)
            {
                if (string.IsNullOrEmpty(fileNames))
                    fileNames = fileNames + fileAttribute.FileName;
                else
                    fileNames = fileNames + ", " + fileAttribute.FileName;
            }

            return fileNames;
        }
        private string GetFileNames(string projectNumber, int packagingItemId, string docType)
        {
            var CADFiles = packagingService.GetUploadedFiles(projectNumber, packagingItemId, docType);
            string fileNames = string.Empty;
            foreach (FileAttribute fileAttribute in CADFiles)
            {
                if (string.IsNullOrEmpty(fileNames))
                    fileNames = fileNames + fileAttribute.FileName;
                else
                    fileNames = fileNames + ", " + fileAttribute.FileName;
            }

            return fileNames;
        }
        private string GetDateForDisplay(DateTime currentValue)
        {
            if (currentValue == Convert.ToDateTime(null))
                return string.Empty;
            else if (currentValue.Equals(DATETIME_MIN))
                return string.Empty;
            else
                return currentValue.ToShortDateString();
        }
        private string GetPersonFieldForDisplay(string person)
        {
            if (string.IsNullOrEmpty(person))
                return string.Empty;
            if (person.IndexOf("#") < 0)
                return person;

            return person.Substring(person.IndexOf("#") + 1);
        }
        private string GetProjectTitle(ItemProposalItem item)
        {
            string title = item.ProjectNumber + " : ";

            if (string.IsNullOrEmpty(item.SAPItemNumber))
                title = title + "XXXXX" + item.SAPDescription;
            else
                title = title + item.SAPItemNumber + " : " + item.SAPDescription;

            return title;
        }
        #endregion
    }

    public class Helper
    {
        public static byte[] ImageToByteArray(System.Drawing.Image MSImage)
        {
            //using (var ms = new MemoryStream())
            //{
            //    MSImage.Save(ms, MSImage.RawFormat);
            //    return ms.ToArray();
            //}
            System.Drawing.ImageConverter _imageConverter = new System.Drawing.ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(MSImage, typeof(byte[]));
            return xByte;
        }

        public static System.Drawing.Image FixedSize(System.Drawing.Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            System.Drawing.Bitmap bmPhoto = new System.Drawing.Bitmap(Width, Height,
                              System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);

            System.Drawing.Graphics grPhoto = System.Drawing.Graphics.FromImage(bmPhoto);
            grPhoto.Clear(System.Drawing.Color.White);
            grPhoto.InterpolationMode =
                    System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new System.Drawing.Rectangle(destX, destY, destWidth, destHeight),
                new System.Drawing.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                System.Drawing.GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
    }
}
