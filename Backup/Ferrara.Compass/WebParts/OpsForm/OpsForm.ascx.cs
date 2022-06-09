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
using Ferrara.Compass.ControlTemplates.Ferrara.Compass;

namespace Ferrara.Compass.WebParts.OpsForm
{
    [ToolboxItemAttribute(false)]
    public partial class OpsForm : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public OpsForm()
        {
        }

        #region Member Variables
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private IWorkflowService workflowService;
        private IUserManagementService userMgmtService;
        private IPackagingItemService packagingItemService;
        private IMixesService mixesService;
        private IShipperFinishedGoodService shipperService;
        private int iItemId = 0;
        private INotificationService notificationService;
        private IOPSService OPSService;
        private List<PackagingItem> TransferSEMI = new List<PackagingItem>();
        private string webUrl;
        private ISAPBOMService sapBOMService;
        private ISAPMaterialMasterService sapMMService;
        private IInitialCapacityReviewService capacityReviewService;
        private IProjectTimelineTypeService timelineNumbers;
        private IProjectNotesService notesService;

        private const string _ucBOMgridPath = @"~/_controltemplates/15/Ferrara.Compass/ucBOMGrid.ascx";
        private const string _ucSemiDetsPath = @"~/_controltemplates/15/Ferrara.Compass/ucSemiDetails.ascx";
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
        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();

            OPSService = DependencyResolution.DependencyMapper.Container.Resolve<IOPSService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
            packagingItemService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            userMgmtService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
            sapBOMService = DependencyResolution.DependencyMapper.Container.Resolve<ISAPBOMService>();
            sapMMService = DependencyResolution.DependencyMapper.Container.Resolve<ISAPMaterialMasterService>();
            mixesService = DependencyResolution.DependencyMapper.Container.Resolve<IMixesService>();
            shipperService = DependencyResolution.DependencyMapper.Container.Resolve<IShipperFinishedGoodService>();
            capacityReviewService = DependencyResolution.DependencyMapper.Container.Resolve<IInitialCapacityReviewService>();
            timelineNumbers = DependencyResolution.DependencyMapper.Container.Resolve<IProjectTimelineTypeService>();
            notesService = DependencyResolution.DependencyMapper.Container.Resolve<IProjectNotesService>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            webUrl = SPContext.Current.Web.Url;
            if (!Page.IsPostBack)
            {
                try
                {
                    Utilities.BindDropDownItems(drpMakeLocation, GlobalConstants.LIST_ManufacturingLocationsLookup, webUrl);
                    Utilities.BindDropDownItems(drpPackLocation, GlobalConstants.LIST_PackingLocationsLookup, webUrl);
                    //Utilities.BindDropDownItems(drpSecondarylocation, GlobalConstants.LIST_PackingLocationsLookup, webUrl);
                    Utilities.BindDropDownItems(drpCountryOrigin, GlobalConstants.LIST_ManufacturerCountryOfOrigin, webUrl);

                    this.divAccessDenied.Visible = false;
                    this.divAccessRequest.Visible = false;

                    // Check for a valid project number
                    if (!CheckProjectNumber())
                        return;

                    InitializeScreen();

                    if (iItemId > 0)
                    {
                        LoadFormData();
                    }
                }
                catch (Exception exception)
                {
                    ErrorSummary.AddError(exception.Message, this.Page);
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.Operations.ToString() + ": " + exception.Message);
                    exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.Operations.ToString(), "Page_Load");
                }
            }
            else
            {
                iItemId = Convert.ToInt32(hiddenItemId.Value);
            }
            if (iItemId > 0)
            {
                LoadBOMItems();
            }
        }

        #region Private Methods
        private bool CheckProjectNumber()
        {
            iItemId = Utilities.GetItemIdFromProjectNumber(ProjectNumber);

            if (iItemId == 0)
            {
                // Invalid project Id supplied
                Page.Response.Redirect("/_layouts/Compass/AppPages/CompassErrorPage.aspx?ErrorId=1&ProjectNo=" + ProjectNumber, false);
                return false;
            }

            // Store Id in Hidden field
            this.hiddenItemId.Value = iItemId.ToString();
            return true;
        }
        private void InitializeScreen()
        {
            // If user does not belong to a valid group for the page, inform them that they do not hvae access rights
            if (!userMgmtService.HasReadAccess(CompassForm.Operations))
            {
                this.divAccessDenied.Visible = true;
            }

            //If user does not have rights to save/ submit the page, disable the Save and Submit buttons
            if (!userMgmtService.HasWriteAccess(CompassForm.Operations))
            {
                this.btnSave.Enabled = false;
                this.btnSubmit.Enabled = false;
            }

            //if (Utilities.LockScreen(wfStep.ToString()))
            //{
            //    if (!Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_OBMAdmins))
            //    {
            //        this.btnSave.Enabled = false;
            //        this.btnSubmit.Enabled = false;
            //    }
            //}

            string workflowPhase = utilityService.GetWorkflowPhase(iItemId);
            if ((string.Equals(workflowPhase, WorkflowStep.OnHold.ToString())) || (string.Equals(workflowPhase, WorkflowStep.Cancelled.ToString())) ||
                (string.Equals(workflowPhase, WorkflowStep.Completed.ToString())))
            {
                this.btnSave.Enabled = false;
                this.btnSubmit.Enabled = false;
            }
        }

        private void fixMonth(string FirstProductionDate)
        {
            if (!string.IsNullOrEmpty(FirstProductionDate))
            {
                string[] months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

                DateTime date = Convert.ToDateTime(FirstProductionDate);
                int selectedMonth = date.Month - 1;
                lbl1stmonthU.InnerText = months[selectedMonth] + " Projected Retail Selling Units:";
                lbl1stmonthC.InnerText = months[selectedMonth] + " Projected Cases:";
                lbl1stmonthL.InnerText = months[selectedMonth] + " Projected lbs:";


                if (selectedMonth + 1 > 11)
                    selectedMonth = -1;

                lbl2ndmonthU.InnerText = months[selectedMonth + 1] + " Projected Retail Selling Units:";
                lbl2ndmonthC.InnerText = months[selectedMonth + 1] + " Projected Cases:";
                lbl2ndmonthL.InnerText = months[selectedMonth + 1] + " Projected lbs:";

                if (selectedMonth + 2 > 11)
                    selectedMonth = -2;
                lbl3rdmonthU.InnerText = months[selectedMonth + 2] + " Projected Retail Selling Units:";
                lbl3rdmonthC.InnerText = months[selectedMonth + 2] + " Projected Cases:";
                lbl3rdmonthL.InnerText = months[selectedMonth + 2] + " Projected lbs:";
            }


        }
        #endregion

        #region Data Transfer Methods
        private void LoadFormData()
        {
            OPSItem opsItem = OPSService.GetOPSItem(iItemId);
            lblSAPBUOM.Text = opsItem.SAPBaseUOM;
            lblPackType.Text = opsItem.PackType;
            lblRevisedFirstShipDate.Text = Utilities.GetDateForDisplay(opsItem.RevisedFirstShipDate); ;
            lblLikeFGItem.Text = opsItem.LikeItem;

            if (opsItem.ProjectType == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
            {
                lblComments.Text = "Change Notes:";
            }
            else
            {
                lblComments.Text = "Item Concept:";
            }

            txtComments.Text = opsItem.CommentsfromMarketing;
            txtWorkCenterAddInfo.Text = opsItem.WorkCenterAddInfo;
            hdnMaterialDesc.Value = opsItem.FGItemDescription;
            hdnMaterialNumber.Value = opsItem.FGItemNumber;
            Utilities.SetDropDownValue(opsItem.CountryOfOrigin, this.drpCountryOrigin, this.Page);
            Utilities.SetDropDownValue(opsItem.PackingLocation, this.drpPackLocation, this.Page);
            Utilities.SetDropDownValue(opsItem.MfgLocationChange, this.ddlNetworkMove, this.Page);
            Utilities.SetDropDownValue(opsItem.ImmediateSPKChange, this.ddlImmediateSPKChange, this.Page);
            Utilities.SetDropDownValue(opsItem.MakeLocation, this.drpMakeLocation, this.Page);


            txtWhatNetworkMoveIsRequired.Text = opsItem.WhatNetworkMoveIsRequired;
            Utilities.SetDropDownValue(opsItem.ProjectApproved, this.ddlProjectApproved, this.Page);
            txtReasonForRejection.Text = opsItem.ReasonForRejection;

            hdnProjectType.Value = opsItem.ProjectType;
            hdnPageName.Value = GlobalConstants.PAGE_OPS;

            hddRetailSellingUnitsPerBaseUOM.Value = opsItem.RetailSellingUnitsBaseUOM == -9999 ? "0" : opsItem.RetailSellingUnitsBaseUOM.ToString();
            hdnComanClassification.Value = opsItem.CoManClassification;
            LoadUserControls();
            List<MixesItem> mixData;
            if (opsItem.MaterialGroup4ProductForm == "MIXES (MIX)")
            {
                mixData = GetMixesItem(iItemId);
                if (mixData.Count > 0)
                {
                    divMixes.Visible = true;
                    rpMixesSummary.DataSource = mixData;
                    rpMixesSummary.DataBind();
                }
            }
            List<ShipperFinishedGoodItem> shipperData;
            if (opsItem.MaterialGroup5PackType.ToLower() == "shipper (shp)" || opsItem.MaterialGroup5PackType.ToLower() == "shippers (shp)")
            {
                shipperData = GetShipperFGItem(iItemId);
                if (shipperData.Count > 0)
                {
                    divShipper.Visible = true;
                    rpShipperSummary.DataSource = shipperData;
                    rpShipperSummary.DataBind();
                }
            }

            // Set the Initial Approval Information
            try
            {
                lblAnnualProjectedUnits.Text = Utilities.FormatNumber(opsItem.AnnualProjectedUnits);

                if (opsItem.SAPBaseUOM.ToLower() == "cs")
                {
                    if (opsItem.RetailSellingUnitsBaseUOM != 0)
                        lblAnnualProjectedCases.Text = Utilities.FormatNumber(opsItem.AnnualProjectedUnits / opsItem.RetailSellingUnitsBaseUOM);
                    else
                        lblAnnualProjectedCases.Text = "NA";
                }
                else
                    lblAnnualProjectedCases.Text = "NA";

                lblAnnualProjectLbs.Text = Utilities.FormatNumber((opsItem.AnnualProjectedUnits * opsItem.RetailUnitWieghtOz) / 16);

                // Load Capacity Review
                lblLineOfBusiness.Text = opsItem.LineOfBusiness;
                Utilities.SetDropDownValue(opsItem.InitialCapacity_MakeIssues, this.ddlMakeLocationIssues, this.Page);
                Utilities.SetDropDownValue(opsItem.InitialCapacity_PackIssues, this.ddlPackLocationIssues, this.Page);
                txtCapacityComments.Text = opsItem.InitialCapacity_CapacityRiskComments;
                Utilities.SetDropDownValue(opsItem.InitialCapacity_Decision, this.ddlProjectDecision, this.Page);
                txtProjectAcceptance.Text = opsItem.InitialCapacity_AcceptanceComments;

                txtProjectUnit1.Text = Utilities.FormatNumber(opsItem.Month1ProjectedUnits);
                txtProjectUnit2.Text = Utilities.FormatNumber(opsItem.Month2ProjectedUnits);
                txtProjectUnit3.Text = Utilities.FormatNumber(opsItem.Month3ProjectedUnits);

                if (opsItem.SAPBaseUOM.ToLower() == "cs")
                {
                    if (opsItem.RetailSellingUnitsBaseUOM != 0)
                    {
                        txtProjectCase1.Text = Utilities.FormatNumber(opsItem.Month1ProjectedUnits / opsItem.RetailSellingUnitsBaseUOM);
                        txtProjectCase2.Text = Utilities.FormatNumber(opsItem.Month2ProjectedUnits / opsItem.RetailSellingUnitsBaseUOM);
                        txtProjectCase3.Text = Utilities.FormatNumber(opsItem.Month3ProjectedUnits / opsItem.RetailSellingUnitsBaseUOM);

                    }
                    else
                    {
                        txtProjectCase1.Text = "NA";
                        txtProjectCase2.Text = "NA";
                        txtProjectCase3.Text = "NA";
                    }
                }
                else
                {
                    txtProjectCase1.Text = "NA";
                    txtProjectCase2.Text = "NA";
                    txtProjectCase3.Text = "NA";

                }

                txtProjectlbs1.Text = Utilities.FormatNumber((opsItem.Month1ProjectedUnits * opsItem.RetailUnitWieghtOz) / 16);
                txtProjectlbs2.Text = Utilities.FormatNumber((opsItem.Month2ProjectedUnits * opsItem.RetailUnitWieghtOz) / 16);
                txtProjectlbs3.Text = Utilities.FormatNumber((opsItem.Month3ProjectedUnits * opsItem.RetailUnitWieghtOz) / 16);

                if ((opsItem.FirstProductionDate != null) && (opsItem.FirstProductionDate != DateTime.MinValue))
                {
                    txtProductionDate.Text = Utilities.GetDateForDisplay(opsItem.FirstProductionDate);
                }

                lblFirstShipDate.Text = Utilities.GetDateForDisplay(opsItem.RevisedFirstShipDate);
                fixMonth(opsItem.RevisedFirstShipDate.ToShortDateString());

                // Load Attachments
                GetAttachments();
            }
            catch (Exception ex)
            {
                //ErrorSummary.AddError(ex.Message, this.Page);
                //LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.InitialCapacity.ToString() + ": LoadFormData: " + ex.Message);
                //exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.InitialCapacity.ToString(), "LoadFormData");
            }
        }

        private void LoadUserControls()
        {
            List<PackagingItem> dtPackingItem;
            ucBOMGrid ctrl;
            bool isTransferSemiIncuded = false;

            ctrl = (ucBOMGrid)Page.LoadControl(_ucBOMgridPath);
            ctrl.PackagingComponent = "";
            ctrl.MaterialNumber = hdnMaterialNumber.Value;
            ctrl.MaterialDesc = hdnMaterialDesc.Value;
            ctrl.ParentId = 0;
            List<ucBOMGrid> TSGrids = new List<ucBOMGrid>();
            List<ucBOMGrid> PSGrids = new List<ucBOMGrid>();
            phBOM.Controls.Add(ctrl);

            dtPackingItem = packagingItemService.GetFinishedGoodItemsForProject(iItemId);
            foreach (PackagingItem item in dtPackingItem)
            {
                if (item.PackagingComponent.ToLower().Contains("purchased"))
                {
                    ucBOMGrid ctrl4 = (ucBOMGrid)Page.LoadControl(_ucBOMgridPath);
                    ctrl4.PackagingComponent = item.PackagingComponent;
                    ctrl4.ParentId = item.Id;
                    ctrl4.MaterialNumber = item.MaterialNumber;
                    ctrl4.MaterialDesc = item.MaterialDescription;
                    ctrl4.SemiXferMakeLocation = item.TransferSEMIMakePackLocations;
                    PSGrids.Add(ctrl4);


                }
                if (item.PackagingComponent.ToLower().Contains("transfer"))
                {
                    isTransferSemiIncuded = true;
                    ucBOMGrid ctrl3 = (ucBOMGrid)Page.LoadControl(_ucBOMgridPath);
                    ctrl3.PackagingComponent = item.PackagingComponent;
                    ctrl3.ParentId = item.Id;
                    ctrl3.MaterialNumber = item.MaterialNumber;
                    ctrl3.MaterialDesc = item.MaterialDescription;
                    ctrl3.SemiXferMakeLocation = item.TransferSEMIMakePackLocations;
                    TSGrids.Add(ctrl3);
                    phBOM.Controls.Add(ctrl3);

                }
            }
            foreach (ucBOMGrid PSGrid in PSGrids)
            {
                phBOM.Controls.Add(PSGrid);
            }
            foreach (ucBOMGrid TSGrid in TSGrids)
            {
                phBOM.Controls.Add(TSGrid);
            }
            LoadBOMItems();
            hddIsTransferSemiIncuded.Value = isTransferSemiIncuded ? "1" : "0";
        }
        public void SaveuserControls()
        {
            foreach (var ctrl in phBOM.Controls)
            {
                if (ctrl is ucBOMGrid)
                {
                    var type = (ucBOMGrid)ctrl;

                    type.saveData();

                }
            }
            foreach (var ctrl in phTransferSemi.Controls)
            {
                if (ctrl is ucBOMGrid)
                {
                    var type = (ucBOMGrid)ctrl;

                    type.saveData();

                }
            }
            foreach (var ctrl in phTransferSemiFields.Controls)
            {
                if (ctrl is ucSemiDetails)
                {
                    var type = (ucSemiDetails)ctrl;

                    type.saveData();

                }
            }
            foreach (var ctrl in phPurchasedSemiFields.Controls)
            {
                if (ctrl is ucSemiDetails)
                {
                    var type2 = (ucSemiDetails)ctrl;

                    type2.saveData();

                }
            }
        }
        private OPSItem ConstructFormData()
        {
            try { SaveuserControls(); }
            catch (Exception exception)
            {
                ErrorSummary.AddError(exception.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.Operations.ToString() + ": " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.Operations.ToString(), "SaveuserControls");
            }
            OPSItem opsItem = OPSService.GetOPSItem(iItemId);

            opsItem.CountryOfOrigin = drpCountryOrigin.SelectedItem.Text.Trim();
            opsItem.PackingLocation = drpPackLocation.SelectedItem.Text.Trim();
            if (!drpPackLocation.SelectedItem.Text.Trim().Contains("External") && !drpMakeLocation.SelectedItem.Text.Trim().Contains("External"))
            {
                opsItem.CoManClassification = "";
            }
            else
            {
                opsItem.CoManClassification = hdnComanClassification.Value;
            }

            opsItem.MakeLocation = drpMakeLocation.SelectedItem.Text;

            opsItem.WorkCenterAddInfo = txtWorkCenterAddInfo.Text;
            opsItem.MfgLocationChange = ddlNetworkMove.SelectedItem.Text;
            opsItem.ImmediateSPKChange = ddlImmediateSPKChange.SelectedItem.Text;

            opsItem.WhatNetworkMoveIsRequired = txtWhatNetworkMoveIsRequired.Text;
            opsItem.ProjectApproved = ddlProjectApproved.SelectedItem.Text;
            opsItem.ReasonForRejection = txtReasonForRejection.Text;

            #region ConstructIntialCapacityFormData
            opsItem.InitialCapacity_MakeIssues = ddlMakeLocationIssues.SelectedItem.Text;
            opsItem.InitialCapacity_PackIssues = ddlPackLocationIssues.SelectedItem.Text;
            opsItem.InitialCapacity_CapacityRiskComments = txtCapacityComments.Text;
            opsItem.InitialCapacity_Decision = ddlProjectDecision.SelectedItem.Text;
            opsItem.InitialCapacity_AcceptanceComments = txtProjectAcceptance.Text;

            if (!string.IsNullOrEmpty(txtProductionDate.Text))
                opsItem.FirstProductionDate = Utilities.GetDateFromField(txtProductionDate.Text);
            #endregion

            return opsItem;
        }

        private ApprovalItem ConstructApprovalData()
        {
            var item = new ApprovalItem();

            item.CompassListItemId = iItemId;
            item.ModifiedBy = SPContext.Current.Web.CurrentUser.Name;
            item.ModifiedDate = DateTime.Now.ToString();

            return item;
        }
        private List<MixesItem> GetMixesItem(int itemId)
        {
            List<MixesItem> dtMixesItem;
            if (ViewState["MixesItemTable"] == null)
            {
                dtMixesItem = mixesService.GetMixesItems(itemId);
                ViewState["MixesItemTable"] = dtMixesItem;
            }
            else
                dtMixesItem = (List<MixesItem>)ViewState["MixesItemTable"];
            return dtMixesItem;
        }
        private List<ShipperFinishedGoodItem> GetShipperFGItem(int itemId)
        {
            List<ShipperFinishedGoodItem> dtFGItem;
            if (ViewState["FGShipperItemTable"] == null)
            {
                dtFGItem = shipperService.GetShipperFinishedGoodItems(itemId);
                ViewState["FGShipperItemTable"] = dtFGItem;
            }
            else
                dtFGItem = (List<ShipperFinishedGoodItem>)ViewState["FGShipperItemTable"];
            return dtFGItem;
        }
        private void LoadBOMItems()
        {
            //Load User Controls
            ucSemiDetails ctrl = (ucSemiDetails)Page.LoadControl(_ucSemiDetsPath);
            ctrl.ComponentType = GlobalConstants.COMPONENTTYPE_TransferSemi;
            ctrl.ProjectNumber = ProjectNumber;
            ctrl.CompassItemId = iItemId;
            ctrl.ID = "TransferSemi";
            ucSemiDetails ctrl2 = (ucSemiDetails)Page.LoadControl(_ucSemiDetsPath);
            ctrl2.ComponentType = GlobalConstants.COMPONENTTYPE_PurchasedSemi;
            ctrl2.ProjectNumber = ProjectNumber;
            ctrl2.CompassItemId = iItemId;
            ctrl2.ID = "PurchasedCandy";
            phTransferSemiFields.Controls.Clear();
            phPurchasedSemiFields.Controls.Clear();
            phTransferSemiFields.Controls.Add(ctrl);
            phPurchasedSemiFields.Controls.Add(ctrl2);
        }
        #endregion

        #region Attachment Methods
        private void GetAttachments()
        {
            var files = utilityService.GetUploadedCompassFilesByDocType(ProjectNumber, GlobalConstants.DOCTYPE_CAPACITY);
            if (files.Count > 0)
            {
                rpAttachments.Visible = true;
                rpAttachments.DataSource = files;
                rpAttachments.DataBind();
            }
            else
            {
                rpAttachments.Visible = false;
            }
        }
        protected void lnkFileDelete_Click(object sender, EventArgs e)
        {
            var lnkButton = (LinkButton)sender;
            if (!string.IsNullOrEmpty(lnkButton.CommandName))
            {
                utilityService.DeleteAttachment(lnkButton.CommandName);
                GetAttachments();
            }
        }
        #endregion

        #region Button Methods
        protected void lnlIPF_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Utilities.RedirectPageForm(GlobalConstants.PAGE_ItemProposal, ProjectNumber), false);
        }
        protected void lbHelpDeskEmail_Click(object sender, EventArgs e)
        {
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "Operation Form");
            this.divAccessDenied.Visible = false;
            this.divAccessRequest.Visible = true;
        }
        protected void btnReloadAttachment_Click(object sender, EventArgs e)
        {
            GetAttachments();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userMgmtService.HasWriteAccess(CompassForm.Operations))
                {
                    ErrorSummary.AddError("You do not have proper access rights to save this page!", this.Page);
                    return;
                }
                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }
                OPSItem item = ConstructFormData();
                OPSService.UpdateOPSItem(item, iItemId);

                ApprovalItem approvalItem = ConstructApprovalData();
                OPSService.UpdateOPSApprovalItem(approvalItem, false);

                LoadUserControls();

                ////////// Load Attachments
                GetAttachments();

                lblSavedMessage.Text = "Changes Saved: " + DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.Operations.ToString() + ": btnSave_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.Operations.ToString(), "btnSave_Click");
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userMgmtService.HasWriteAccess(CompassForm.Operations))
                {
                    ErrorSummary.AddError("You do not have proper access rights to save this page!", this.Page);
                    return;
                }

                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }

                OPSItem item = ConstructFormData();
                OPSService.UpdateOPSItem(item, iItemId);

                ApprovalItem approvalItem = ConstructApprovalData();
                OPSService.UpdateOPSApprovalItem(approvalItem, true);

                // Complete the workflow task
                workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.Operations);

                if (hdnProjectType.Value == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly && ddlProjectApproved.SelectedItem.Text == "No")
                {
                    var ProjectCancelReasson = txtReasonForRejection.Text;
                    Utilities.UpdateProjectRejectionReason(iItemId, ProjectCancelReasson, GlobalConstants.PAGE_OPS);
                    //timelineNumbers.workflowStatusUpdate(iItemId, GlobalConstants.WORKFLOWPHASE_Cancelled);
                    //workflowService.UpdateWorkflowPhase(iItemId, GlobalConstants.WORKFLOWPHASE_Cancelled);
                    //notesService.UpdateProjectComments(iItemId, string.Concat("Reason for cancellation:", ProjectCancelReasson));
                    Page.Response.Redirect(
                        string.Concat(
                            SPContext.Current.Web.Url,
                            "/Pages/ItemProposal.aspx?ProjectNo=",
                            ProjectNumber,
                            "&IPFMode=",
                            GlobalConstants.QUERYSTRINGVALUE_IPFChange,
                            "&", GlobalConstants.QUERYSTRING_ProjectRejected, "=",
                            "Yes"
                            ), false);
                }
                else
                {
                    // Redirect to Home page after successfull Submit                        
                    Page.Response.Redirect(Utilities.RedirecttoHomePage(), false);
                }
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.Operations.ToString() + ": btnSave_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.Operations.ToString(), "btnSave_Click");
            }
        }
        #endregion
    }
}
