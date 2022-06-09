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

namespace Ferrara.Compass.WebParts.GraphicsRequestDetailForm
{
    [ToolboxItemAttribute(false)]
    public partial class GraphicsRequestDetailForm : WebPart
    {
        #region Member Variables
        private IItemProposalService itemProposalService;
        private IPackagingItemService packagingItemService;
        private IApprovalService approvalService;
        private IBillOfMaterialsService bomService;
        private IOPSService opsService;
        private IExternalManufacturingService extMfgService;

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
        public GraphicsRequestDetailForm()
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
            ApprovalListItem approvalItem = approvalService.GetApprovalItem(iItemId);
            BillofMaterialsItem bomItem = bomService.GetBillOfMaterialsItem(iItemId);
            OPSItem opsItem = opsService.GetOPSItem(iItemId);
            ExternalManufacturingItem extMfgItem = extMfgService.GetExternalManufacturingItem(iItemId);
            OBMFirstReviewItem obmFirstReviewItem = obmFirstReviewService.GetPMFirstReviewItem(iItemId);
            CompassPackMeasurementsItem packMeasItem = bomService.GetPackMeasurementsItem(iItemId, packItem.ParentID);
            lblSubmittedBy.Text = ipItem.PMName;
            lblDateSubmitted.Text = approvalItem.GRAPHICS_StartDate;
            lblFirstShipDate.Text = Utilities.GetDateForDisplay(ipItem.RevisedFirstShipDate);
            lblFirstProductionDate.Text = Utilities.GetDateForDisplay(obmFirstReviewItem.FirstProductionDate);
            lblProjectNumber.Text = ipItem.ProjectNumber;
            lblItemNumber.Text = ipItem.SAPItemNumber;
            lblItemDescription.Text = ipItem.SAPDescription;
            lblMaterialNumber.Text = packItem.MaterialNumber;
            lblMaterialDescription.Text = packItem.MaterialDescription;
            lblLikeMaterialNumber.Text = packItem.CurrentLikeItem;
            lblLikeMaterialDescription.Text = packItem.CurrentLikeItemDescription;
            lblOldFGItemNumber.Text = ipItem.OldFGItemNumber;
            lblOldFGItemDescription.Text = ipItem.OldFGItemDescription;
            lblLikeMaterialReason.Text = packItem.CurrentLikeItemReason;
            lblOldMaterialNumber.Text = packItem.CurrentOldItem;
            lblOldMaterialDescription.Text = packItem.CurrentOldItemDescription;
            lblComponentType.Text = packItem.PackagingComponent;
            lblBackSeam.Text = packItem.BackSeam;
            lblLineOfBusiness.Text = ipItem.ProductHierarchyLevel1;
            lblMarketClaimsLabelingRequirements.Text = ipItem.MarketClaimsLabelingRequirements;

            if (string.Equals(ipItem.ProductHierarchyLevel1, "Seasonal (000000023)"))
                lblSeason.Text = ipItem.ProductHierarchyLevel2;
            else
                lblSeason.Text = GlobalConstants.CONST_NotApplicable;

            lblBrand.Text = ipItem.MaterialGroup1Brand;

            if (string.IsNullOrEmpty(ipItem.Customer) || ipItem.Customer.Equals(GlobalConstants.LIST_NoSelectionText))
                lblCustomer.Text = "Not Customer Specific";
            else
                lblCustomer.Text = ipItem.Customer;

            isExternallyManufacturedPackaged = (string.Equals(opsItem.MakeLocation, "Externally Manufactured")) ||
                (string.Equals(opsItem.PackingLocation, "Externally Packed"));
            if (isExternallyManufacturedPackaged)
                lblExternallyManufacturedPackaged.Text = extMfgItem.CoManufacturingClassification;
            else
                lblExternallyManufacturedPackaged.Text = "No";

            lblMakeLocation.Text = opsItem.MakeLocation;
            lblPackLocation.Text = opsItem.PackingLocation;
            if (string.IsNullOrEmpty(bomItem.PackagingEngineerLeadName))
            {
                lblPackagingEngineer.Text = Utilities.GetPersonFieldForDisplay(bomItem.PackagingEngineerLead);
            }
            else
            {
                lblPackagingEngineer.Text = bomItem.PackagingEngineerLeadName;
            }
            lblSubstrate.Text = packItem.Structure;

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


            if (string.IsNullOrEmpty(packItem.StructureColor))
                lblSubstrateColor.Text = GlobalConstants.CONST_NotApplicable;
            else
                lblSubstrateColor.Text = packItem.StructureColor;

            lblPrinter.Text = packItem.PrinterSupplier;

            if (string.IsNullOrEmpty(packItem.Unwind))
                lblUnwindNumber.Text = GlobalConstants.CONST_NotApplicable;
            else
                lblUnwindNumber.Text = packItem.Unwind;

            lblUnitUPC.Text = ipItem.UnitUPC;
            lblCaseUCC.Text = ipItem.CaseUCC;
            lblPalletUCC.Text = ipItem.PalletUCC;
            lblJarDispalyUPC.Text = ipItem.DisplayBoxUPC;

            var files = utilityService.GetUploadedCompassFilesByDocType(ProjectNumber, GlobalConstants.DOCTYPE_NLEA);
            rpNLEA.DataSource = files;
            rpNLEA.DataBind();

            files = packagingItemService.GetUploadedFiles(ProjectNumber, PackagingItemId, GlobalConstants.DOCTYPE_CADDrawing);
            rpDielines.DataSource = files;
            rpDielines.DataBind();
            lblPalletPatternChange.Text = packMeasItem.PalletPatternChange;
            files = utilityService.GetUploadedCompassFilesByDocType(ProjectNumber, GlobalConstants.DOCTYPE_PalletPattern);
            rpPalletPattern.DataSource = files;
            rpPalletPattern.DataBind();

            files = packagingItemService.GetUploadedFiles(ProjectNumber, PackagingItemId, GlobalConstants.DOCTYPE_Rendering);
            rpRenderings.DataSource = files;
            rpRenderings.DataBind();

            lblProjectNotesMarketing.Text = ipItem.ItemConcept;
            lblGraphicsBrief.Text = packItem.GraphicsBrief;
            lblComponentNotesPE.Text = packItem.Notes;
            lblGraphicsVendor.Text = packItem.ExternalGraphicsVendor;
        }
        #endregion

        #region Button Methods
        protected void btnGraphicsRequest_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Utilities.RedirectPageForm(GlobalConstants.PAGE_GraphicsRequest, ProjectNumber));
        }
        #endregion
    }
}
