using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Linq;
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
using System.Web.UI.WebControls;

namespace Ferrara.Compass.WebParts.GraphicsRequestDetailForm_New
{
    [ToolboxItemAttribute(false)]
    public partial class GraphicsRequestDetailForm_New : WebPart
    {
        #region Member Variables
        private IItemProposalService itemProposalService;
        private IPackagingItemService packagingItemService;
        private IApprovalService approvalService;
        private IBillOfMaterialsService bomService;
        private IOPSService opsService;
        private IExternalManufacturingService extMfgService;
        private IBOMSetupService bomSetupService;
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private IUserManagementService userManagementService;
        private INotificationService notificationService;
        private IOBMFirstReviewService obmFirstReviewService;
        private int iItemId = 0;
        private int iPackagingItemId = 0;
        #endregion

        #region Properties
        private string ProjectNumber
        {
            get
            {
                if (Page.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo] != null)
                    return Page.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo];
                return string.Empty;
            }
        }
        private int PackagingItemId
        {
            get
            {
                try
                {
                    if (Page.Request.QueryString[GlobalConstants.QUERYSTRING_PackagingItemId] != null)
                        return Convert.ToInt32(Page.Request.QueryString[GlobalConstants.QUERYSTRING_PackagingItemId]);
                }
                catch (Exception ex)
                {
                    exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.Graphics.ToString(), "PackagingItemId");
                }

                return -1;
            }
        }
        #endregion

        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public GraphicsRequestDetailForm_New()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();

            itemProposalService = DependencyResolution.DependencyMapper.Container.Resolve<IItemProposalService>();
            packagingItemService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            approvalService = DependencyResolution.DependencyMapper.Container.Resolve<IApprovalService>();
            bomService = DependencyResolution.DependencyMapper.Container.Resolve<IBillOfMaterialsService>();
            opsService = DependencyResolution.DependencyMapper.Container.Resolve<IOPSService>();
            extMfgService = DependencyResolution.DependencyMapper.Container.Resolve<IExternalManufacturingService>();

            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            userManagementService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
            obmFirstReviewService = DependencyResolution.DependencyMapper.Container.Resolve<IOBMFirstReviewService>();
            bomSetupService = DependencyResolution.DependencyMapper.Container.Resolve<IBOMSetupService>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    // Check for a valid project number
                    if (!CheckProjectNumber())
                        return;

                    LoadFormData();
                }
                catch (Exception exception)
                {
                    ErrorSummary.AddError(exception.Message, this.Page);
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.Graphics.ToString() + ": " + exception.Message);
                    exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.Graphics.ToString(), "Page_Load");
                }
            }
            else
            {
                iItemId = Convert.ToInt32(hiddenItemId.Value);
            }
        }

        #region Private Methods
        private bool CheckProjectNumber()
        {
            iItemId = Utilities.GetItemIdFromProjectNumber(ProjectNumber);

            if (iItemId == 0)
            {
                // Invalid project Id supplied
                Page.Response.Redirect("/_layouts/Ferrara.Compass/AppPages/CompassErrorPage.aspx?ErrorId=1&ProjectNo=" + ProjectNumber, false);
                return false;
            }
            // Store Id in Hidden field
            this.hiddenItemId.Value = iItemId.ToString();

            iPackagingItemId = PackagingItemId;
            if (iPackagingItemId == -1)
            {
                // Invalid packaging Id supplied
                Page.Response.Redirect("/_layouts/Compass/AppPages/CompassErrorPage.aspx?ErrorId=6&ProjectNo=" + ProjectNumber, false);
                return false;
            }
            // Store Id in Hidden field
            this.hiddenPackagingItemId.Value = iItemId.ToString();

            return true;
        }
        #endregion

        #region Data Transfer Methods
        private void LoadFormData()
        {
            bool isExternallyManufacturedPackaged;
            ItemProposalItem ipItem = itemProposalService.GetItemProposalItem(iItemId);
            PackagingItem packItem = packagingItemService.GetPackagingItemByPackagingId(iPackagingItemId);
            PackagingItem ParentpackItem = (packItem.ParentID == 0) ? new PackagingItem() : packagingItemService.GetPackagingItemByPackagingId(packItem.ParentID);
            ApprovalListItem approvalItem = approvalService.GetApprovalItem(iItemId);
            BillofMaterialsItem bomItem = bomService.GetBillOfMaterialsItem(iItemId);
            OPSItem opsItem = opsService.GetOPSItem(iItemId);
            ExternalManufacturingItem extMfgItem = extMfgService.GetExternalManufacturingItem(iItemId);
            OBMFirstReviewItem obmFirstReviewItem = obmFirstReviewService.GetPMFirstReviewItem(iItemId);
            BOMSetupItem packMeasItem = bomSetupService.GetPackMeasurementsItem(iItemId, packItem.ParentID);

            int index = 1;
            lblSubmittedByText.Text = index + ". " + lblSubmittedByText.Text;
            lblSubmittedBy.Text = ipItem.PMName;

            index++;
            lblDateSubmittedText.Text = index + ". " + lblDateSubmittedText.Text;
            lblDateSubmitted.Text = approvalItem.GRAPHICS_StartDate;

            index++;
            lblFirstProductionDateText.Text = index + ". " + lblFirstProductionDateText.Text;
            lblFirstProductionDate.Text = Utilities.GetDateForDisplay(obmFirstReviewItem.FirstProductionDate);

            index++;
            lblFirstShipDateText.Text = index + ". " + lblFirstShipDateText.Text;
            lblFirstShipDate.Text = Utilities.GetDateForDisplay(ipItem.RevisedFirstShipDate);

            index++;
            lblProjectNumberText.Text = index + ". " + lblProjectNumberText.Text;
            lblProjectNumber.Text = ipItem.ProjectNumber;

            index++;
            lblLineOfBusinessText.Text = index + ". " + lblLineOfBusinessText.Text;
            lblLineOfBusiness.Text = ipItem.ProductHierarchyLevel1;

            index++;
            lblSeasonText.Text = index + ". " + lblSeasonText.Text;
            if (string.Equals(ipItem.ProductHierarchyLevel1, "Seasonal (000000023)"))
                lblSeason.Text = ipItem.ProductHierarchyLevel2;
            else
                lblSeason.Text = GlobalConstants.CONST_NotApplicable;


            index++;
            lblCustomerText.Text = index + ". " + lblCustomerText.Text;
            if (string.IsNullOrEmpty(ipItem.Customer) || ipItem.Customer.Equals(GlobalConstants.LIST_NoSelectionText))
                lblCustomer.Text = "Not Customer Specific";
            else
                lblCustomer.Text = ipItem.Customer;

            index++;
            lblBrandText.Text = index + ". " + lblBrandText.Text;
            lblBrand.Text = ipItem.MaterialGroup1Brand;

            index++;
            lblPackagingEngineerText.Text = index + ". " + lblPackagingEngineerText.Text;
            if (string.IsNullOrEmpty(bomItem.PackagingEngineerLeadName))
            {
                lblPackagingEngineer.Text = Utilities.GetPersonFieldForDisplay(bomItem.PackagingEngineerLead);
            }
            else
            {
                lblPackagingEngineer.Text = bomItem.PackagingEngineerLeadName;
            }

            index++;
            lblExternallyManufacturedPackagedText.Text = index + ". " + lblExternallyManufacturedPackagedText.Text;
            isExternallyManufacturedPackaged = (string.Equals(opsItem.MakeLocation, "Externally Manufactured")) ||
                (string.Equals(opsItem.PackingLocation, "Externally Packed"));
            if (isExternallyManufacturedPackaged)
                lblExternallyManufacturedPackaged.Text = extMfgItem.CoManufacturingClassification;
            else
                lblExternallyManufacturedPackaged.Text = "No";

            if (string.IsNullOrEmpty(opsItem.ExternalManufacturer) || string.Equals(opsItem.ExternalManufacturer, "Select..."))
            {
                divExternalManufacturer.Visible = false;
            }
            else
            {
                divExternalManufacturer.Visible = true;
                index++;
                lblExternalManufacturerText.Text = index + ". " + lblExternalManufacturerText.Text;
                lblExternalManufacturer.Text = opsItem.ExternalManufacturer;
            }

            if (string.IsNullOrEmpty(opsItem.ExternalPacker) || string.Equals(opsItem.ExternalManufacturer, "Select..."))
            {
                divExternalPacker.Visible = false;
            }
            else
            {
                divExternalPacker.Visible = true;
                index++;
                lblExternalPackerText.Text = index + ". " + lblExternalPackerText.Text;
                lblExternalPacker.Text = opsItem.ExternalPacker;
            }

            index++;
            lblMakeLocationText.Text = index + ". " + lblMakeLocationText.Text;
            lblMakeLocation.Text = opsItem.MakeLocation;

            index++;
            lblPackLocationText.Text = index + ". " + lblPackLocationText.Text;
            lblPackLocation.Text = opsItem.PackingLocation;

            index++;
            lblItemNumberText.Text = index + ". " + lblItemNumberText.Text;
            lblItemNumber.Text = ipItem.SAPItemNumber;

            index++;
            lblItemDescriptionText.Text = index + ". " + lblItemDescriptionText.Text;
            lblItemDescription.Text = ipItem.SAPDescription;

            index++;
            lblMaterialNumberText.Text = index + ". " + lblMaterialNumberText.Text;
            lblMaterialNumber.Text = packItem.MaterialNumber;

            index++;
            lblMaterialDescriptionText.Text = index + ". " + lblMaterialDescriptionText.Text;
            lblMaterialDescription.Text = packItem.MaterialDescription;

            index++;
            lblPrinterText.Text = index + ". " + lblPrinterText.Text;
            lblPrinter.Text = packItem.PrinterSupplier;

            index++;
            lblComponentTypeText.Text = index + ". " + lblComponentTypeText.Text;
            lblComponentType.Text = packItem.PackagingComponent;

            index++;
            lblOldMaterialNumberText.Text = index + ". " + lblOldMaterialNumberText.Text;
            lblOldMaterialNumber.Text = packItem.CurrentOldItem;

            index++;
            lblOldMaterialDescriptionText.Text = index + ". " + lblOldMaterialDescriptionText.Text;
            lblOldMaterialDescription.Text = packItem.CurrentOldItemDescription;


            index++;
            lblLikeMaterialNumberText.Text = index + ". " + lblLikeMaterialNumberText.Text;
            lblLikeMaterialNumber.Text = packItem.CurrentLikeItem;

            index++;
            lblLikeMaterialDescriptionText.Text = index + ". " + lblLikeMaterialDescriptionText.Text;
            lblLikeMaterialDescription.Text = packItem.CurrentLikeItemDescription;

            index++;
            lblLikeMaterialReasonText.Text = index + ". " + lblLikeMaterialReasonText.Text;
            lblLikeMaterialReason.Text = packItem.CurrentLikeItemReason;


            index++;
            lblOldFGItemNumberText.Text = index + ". " + lblOldFGItemNumberText.Text;
            lblOldFGItemNumber.Text = ipItem.OldFGItemNumber;

            index++;
            lblOldFGItemDescriptionText.Text = index + ". " + lblOldFGItemDescriptionText.Text;
            lblOldFGItemDescription.Text = ipItem.OldFGItemDescription;

            //14 Digit Bar code visibilty
            if (packItem.ParentID != 0)
            {
                if (packItem.PackagingComponent.ToLower().Contains("corrugated") && packItem.NewExisting.ToLower() == "new" && ParentpackItem.PackagingComponent.ToLower().Contains("transfer") && (ParentpackItem.PackLocation.Contains("FQ22") || ParentpackItem.PackLocation.Contains("FQ25")))
                {
                    div14DigitBarcode.Visible = true;
                    index++;
                    lbl14DigitBarcodeText.Text = index + ". " + lbl14DigitBarcodeText.Text;
                    lbl14DigitBarcode.Text = packItem.FourteenDigitBarCode;
                }
            }

            index++;
            lblPrintStyleText.Text = index + ". " + lblPrintStyleText.Text;
            if (packItem.PackagingComponent.ToLower().Contains("film"))
            {
                lblPrintStyle.Text = packItem.FilmPrintStyle;
            }
            else if (packItem.PackagingComponent.ToLower().Contains("corrugated") || packItem.PackagingComponent.ToLower().Contains("paperboard"))
            {
                lblPrintStyle.Text = packItem.CorrugatedPrintStyle;
            }
            else
            {
                lblPrintStyle.Text = GlobalConstants.CONST_NotApplicable;
            }

            index++;
            lblUnitUPCText.Text = index + ". " + lblUnitUPCText.Text;
            lblUnitUPC.Text = ipItem.UnitUPC;

            index++;
            lblJarDispalyUPCText.Text = index + ". " + lblJarDispalyUPCText.Text;
            lblJarDispalyUPC.Text = ipItem.DisplayBoxUPC;

            index++;
            var UPCAssociated = "";
            if (packItem.UPCAssociated == "Manual Entry")
            {
                UPCAssociated = packItem.UPCAssociatedManualEntry;
            }
            else
            {
                UPCAssociated = packItem.UPCAssociated;
            }
            lblUPCAssociatedWithThisPackagingComponentText.Text = index + ". " + lblUPCAssociatedWithThisPackagingComponentText.Text;
            lblUPCAssociatedWithThisPackagingComponent.Text = UPCAssociated;

            index++;
            lblCaseUCCText.Text = index + ". " + lblCaseUCCText.Text;
            lblCaseUCC.Text = ipItem.CaseUCC;

            index++;
            lblPalletUCCText.Text = index + ". " + lblPalletUCCText.Text;
            lblPalletUCC.Text = ipItem.PalletUCC;

            index++;
            lblRegSheetText.Text = index + ". " + lblRegSheetText.Text;
            var files = utilityService.GetUploadedCompassFilesByDocType(ProjectNumber, GlobalConstants.DOCTYPE_NLEA);
            rpNLEA.DataSource = files;
            rpNLEA.DataBind();

            index++;
            lblPackagingSpecificationNumberText.Text = index + ". " + lblPackagingSpecificationNumberText.Text;
            lblPackagingSpecificationNumber.Text = packItem.SpecificationNo;

            index++;
            lblHyperlinkToDielineInPLMText.Text = index + ". " + lblHyperlinkToDielineInPLMText.Text;
            anHyperlinktoDielineInPLM.InnerText = generateLinkName(packItem.ParentType, packItem.MaterialNumber);
            anHyperlinktoDielineInPLM.HRef = packItem.DielineURL;

            index++;
            lblDieLineText.Text = index + ". " + lblDieLineText.Text;
            files = bomSetupService.GetUploadedFiles(ProjectNumber, PackagingItemId, GlobalConstants.DOCTYPE_Dieline);
            rpDielines.DataSource = files;
            rpDielines.DataBind();

            index++;
            lblPalletSpecificationHyperlinkText.Text = index + ". " + lblPalletSpecificationHyperlinkText.Text;
            hlPalletSpecHyperlink.NavigateUrl = packMeasItem.PalletSpecLink;
            string title = string.IsNullOrEmpty(ParentpackItem.PackagingComponent) ? "Finished Good" : ParentpackItem.PackagingComponent;
            title = title + ": " + ParentpackItem.MaterialNumber + ": Pallet Pattern";
            hlPalletSpecHyperlink.Text = title;
            hlPalletSpecHyperlink.CssClass = "";

            index++;
            lblPalletSpecificationNumberText.Text = index + ". " + lblPalletSpecificationNumberText.Text;
            lblPalletSpecificationNumber.Text = packMeasItem.PalletSpecNumber;

            index++;
            lblGraphicsBriefText.Text = index + ". " + lblGraphicsBriefText.Text;
            lblGraphicsBrief.Text = packItem.GraphicsBrief;

            index++;
            lblRequireBELabelingText.Text = index + ". " + lblRequireBELabelingText.Text;
            lblRequireBELabeling.Text = packItem.BioEngLabelingRequired;

            index++;
            lblBEQRCodeFileText.Text = index + ". " + lblBEQRCodeFileText.Text;
            files = packagingItemService.GetUploadedFiles(ProjectNumber, PackagingItemId, GlobalConstants.DOCTYPE_BEQRCodeEPSFile);
            rptBEQRCodeFiles.DataSource = files;
            rptBEQRCodeFiles.DataBind();

            index++;
            lblProjectNotesMarketingText.Text = index + ". " + lblProjectNotesMarketingText.Text;
            lblProjectNotesMarketing.Text = ipItem.ItemConcept;

            index++;
            lblRenderingsText.Text = index + ". " + lblRenderingsText.Text;
            files = packagingItemService.GetUploadedFiles(ProjectNumber, PackagingItemId, GlobalConstants.DOCTYPE_Rendering);
            rpRenderings.DataSource = files;
            rpRenderings.DataBind();

            index++;
            lblApprovedGraphicAssetText.Text = index + ". " + lblApprovedGraphicAssetText.Text;
            files = packagingItemService.GetUploadedFiles(ProjectNumber, PackagingItemId, GlobalConstants.DOCTYPE_ApprovedGraphicsAsset);
            rptApprovedGraphicAsset.DataSource = files;
            rptApprovedGraphicAsset.DataBind();
        }

        #endregion

        #region Button Methods
        protected void btnGraphicsRequest_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Utilities.RedirectPageForm(GlobalConstants.PAGE_GraphicsRequest, ProjectNumber));
        }
        #endregion

        private string generateLinkName(string parentName, string matNumber)
        {
            if (parentName == "")
            {
                parentName = "Finished Good";
            }
            if (matNumber == "")
            {
                matNumber = "XXXXX";
            }
            return parentName + ": " + matNumber + ": Dieline Link";
        }
    }
}
