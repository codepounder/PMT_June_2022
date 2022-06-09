using System;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.ComponentModel;
using Ferrara.Compass.Classes;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Enum;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Ferrara.Compass.WebParts.PrelimSAPInitialItemSetupForm
{
    [ToolboxItemAttribute(false)]
    public partial class PrelimSAPInitialItemSetupForm : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public PrelimSAPInitialItemSetupForm()
        {
        }

        #region Member Variables

        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private IWorkflowService workflowService;
        private IUserManagementService userMgmtService;
        private INotificationService notificationService;
        private ISAPMaterialMasterService sapMMService;
        private ISAPInitialItemSetUpService sapInitialISetUpService;
        private IOPSService opsService;
        private IPackagingItemService packagingService;
        private IShipperFinishedGoodService shipperFinishedGoodService;
        private IMixesService mixesService;
        private IItemProposalService proposalService;
        private ISAPBOMSetupService sapBOMSetupService;

        private int iItemId = 0;

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

            sapInitialISetUpService = DependencyResolution.DependencyMapper.Container.Resolve<ISAPInitialItemSetUpService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
            userMgmtService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
            sapMMService = DependencyResolution.DependencyMapper.Container.Resolve<ISAPMaterialMasterService>();
            opsService = DependencyResolution.DependencyMapper.Container.Resolve<IOPSService>();
            packagingService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            shipperFinishedGoodService = DependencyResolution.DependencyMapper.Container.Resolve<IShipperFinishedGoodService>();
            mixesService = DependencyResolution.DependencyMapper.Container.Resolve<IMixesService>();
            proposalService = DependencyResolution.DependencyMapper.Container.Resolve<IItemProposalService>();
            sapBOMSetupService = DependencyResolution.DependencyMapper.Container.Resolve<ISAPBOMSetupService>();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
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
                    if (hdnProjectType.Value.Contains("Renovations"))
                    {
                        dvMain.Visible = false;
                        dvMsg.Visible = true;
                    }
                }
                catch (Exception exception)
                {
                    ErrorSummary.AddError(exception.Message, this.Page);
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.PrelimSAPInitialSetup.ToString() + ": " + exception.Message);
                    exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.PrelimSAPInitialSetup.ToString(), "Page_Load");
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
            if (!userMgmtService.HasReadAccess(CompassForm.SAPInitialSetup))
            {
                this.divAccessDenied.Visible = true;
            }

            //If user does not have rights to save/ submit the page, disable the Save and Submit buttons
            if (!userMgmtService.HasWriteAccess(CompassForm.SAPInitialSetup))
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
        private void SetLookupValues(string sapNumber, bool sapLookup)
        {
            // Add BOM Items
            SAPMaterialMasterListItem mmItem = sapMMService.GetSAPMaterialMaster(sapNumber);
            if (sapLookup)
            {
                txtSAPDescription.Text = mmItem.SAPDescription;
            }

        }
        #endregion

        #region Data Transfer Methods

        private void LoadFormData()
        {
            SAPInitialItemSetUp Item = sapInitialISetUpService.GetSAPInitialSetupItem(iItemId);
            OPSItem opsItem = opsService.GetOPSItem(iItemId);
            ItemProposalItem ipf = proposalService.GetItemProposalItem(iItemId);
            SAPBOMSetupItem sapbomsetupItem = sapBOMSetupService.GetSAPBOMSetupItem(iItemId);
            List<CheckBox> checkBoxBinding = checkboxSetup(ipf, sapbomsetupItem);
            rptCheckBoxes.DataSource = checkBoxBinding;
            rptCheckBoxes.DataBind();

            txtProjectType.Text = ipf.ProjectType;
            txtProjectTypeSubCategory.Text = ipf.ProjectTypeSubCategory;
            lblRetailSellingUnitsUOM.Text = Utilities.FormatNumber(Item.RetailSellingUnitsBaseUOM);
            lblSAPBaseUOM.Text = Item.SAPBaseUOM;
            lblRetailUnitWeight.Text = Utilities.FormatDecimal(Item.RetailUnitWieghtOz, 2);
            lblProductHierarchyLevel1.Text = Item.ProductHierarchyLevel1;
            lblProductHierarchyLevel2.Text = Item.ProductHierarchyLevel2;
            lblMaterialGroup.Text = Item.MaterialGroup1Brand;
            lblTradePromo.Text = Item.MaterialGroup2Pricing;
            lblMaterialGroup4.Text = Item.MaterialGroup4ProductForm;
            lblMaterialGroup5.Text = Item.MaterialGroup5PackType;
            lblProfitCenter.Text = Item.ProfitCenter;
            hdnProjectType.Value = Item.ProjectType;
            txtItemConcept.Text = ipf.ItemConcept;
            if (ipf.ProjectType == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
            {
                lblItemConcept.Text = "Change Notes:";
            }
            else
            {
                lblItemConcept.Text = "Item Concept:";
            }
            txtSAPItem.Text = string.IsNullOrEmpty(Item.SAPItemNumber) ? "NEEDS NEW" : Item.SAPItemNumber;
            txtSAPDescription.Text = string.IsNullOrEmpty(Item.SAPDescription) ? "" : Item.SAPDescription;
            txtSAPDescription.ReadOnly = (Item.NewIPF == "Yes") ? true : false;
            txtUnitUPC.Text = string.IsNullOrEmpty(Item.UnitUPC) ? "NEEDS NEW" : Item.UnitUPC;

            if (!string.IsNullOrEmpty(Item.DisplayBoxUPC))
                txtDisplayUPC.Text = Item.DisplayBoxUPC;
            else if (Item.RequireNewDisplayBoxUPC.ToLower() == "yes")
                txtDisplayUPC.Text = "Needs New";
            else
                txtDisplayUPC.Text = "N/A";

            if (Item.SAPBaseUOM.ToLower() == "pal")
            {
                txtCaseUCC.Text = "";
                dvCaseUcc.Visible = false;
            }
            else if (Item.SAPBaseUOM.ToLower() == "cs")
                txtCaseUCC.Text = string.IsNullOrEmpty(Item.CaseUCC) ? "Needs New" : Item.CaseUCC;

            if (Item.SAPBaseUOM.ToLower() == "cs")
            {
                txtPalletUCC.Text = "";
                dvPalletUcc.Visible = false;
            }
            else if (Item.SAPBaseUOM.ToLower() == "pal")
                txtPalletUCC.Text = string.IsNullOrEmpty(Item.PalletUCC) ? "Needs New" : Item.PalletUCC;
            List<ShipperFinishedGoodItem> shipperData;
            List<MixesItem> mixData;
            if (Item.MaterialGroup5PackType.ToLower() == "shipper (shp)" || Item.MaterialGroup5PackType.ToLower() == "shippers (shp)")
            {
                shipperData = GetShipperFGItem(iItemId);
                if (shipperData.Count > 0)
                {
                    divShipper.Visible = true;
                    rpShipperSummary.DataSource = shipperData;
                    rpShipperSummary.DataBind();
                }
            }
            if (Item.MaterialGroup4ProductForm == "MIXES (MIX)")
            {
                mixData = GetMixesItem(iItemId);
                if (mixData.Count > 0)
                {
                    divMixes.Visible = true;
                    rpMixesSummary.DataSource = mixData;
                    rpMixesSummary.DataBind();
                }
            }
        }
        private SAPInitialItemSetUp ConstructFormData()
        {
            SAPInitialItemSetUp sapInitialItemSetUp = new SAPInitialItemSetUp();

            sapInitialItemSetUp.SAPItemNumber = txtSAPItem.Text;
            sapInitialItemSetUp.SAPDescription = txtSAPDescription.Text;
            sapInitialItemSetUp.UnitUPC = txtUnitUPC.Text;
            sapInitialItemSetUp.DisplayBoxUPC = txtDisplayUPC.Text;

            sapInitialItemSetUp.CaseUCC = txtCaseUCC.Text;
            sapInitialItemSetUp.PalletUCC = txtPalletUCC.Text;
            sapInitialItemSetUp.CompassListItemId = iItemId;

            // hdnProjectType.Value = opsItem.ProjectType;
            return sapInitialItemSetUp;
        }
        private ApprovalItem ConstructApprovalData()
        {
            var item = new ApprovalItem();

            item.CompassListItemId = iItemId;
            item.ModifiedBy = SPContext.Current.Web.CurrentUser.Name;
            item.ModifiedDate = DateTime.Now.ToString();

            return item;
        }
        private List<CheckBox> checkboxSetup(ItemProposalItem itemProposalItem, SAPBOMSetupItem sapBomSetupItem)
        {
            List<CheckBox> cbs = new List<CheckBox>();
            CheckBox checkbox = new CheckBox();
            #region Open Sales Org 1000
            if (sapBomSetupItem.ProductHierarchyLevel1 != GlobalConstants.PRODUCT_HIERARCHY1_LBB)
            {
                checkbox = new CheckBox();
                checkbox.CssClass = "bomTask";
                checkbox.ID = "chkOpenSalesSELL";
                checkbox.Text = "Open Sales Org 1000";
                checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.OpenSalesSELL) ? false : sapBomSetupItem.OpenSalesSELL.ToLower().Equals("yes");
                cbs.Add(checkbox);

            }
            #endregion
            #region Open Sales Org 2000
            if (sapBomSetupItem.ProductHierarchyLevel1 == GlobalConstants.PRODUCT_HIERARCHY1_LBB)
            {
                checkbox = new CheckBox();
                checkbox.CssClass = "bomTask";
                checkbox.ID = "chkOpenSalesFERQ";
                checkbox.Text = "Open Sales Org 2000";
                checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.OpenSalesFERQ) ? false : sapBomSetupItem.OpenSalesFERQ.ToLower().Equals("yes");
                cbs.Add(checkbox);

            }
            #endregion

            return cbs;
        }
        protected void rptCheckBoxes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            CheckBox cb;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                cb = (CheckBox)e.Item.DataItem;
                CheckBox CheckBox1 = ((CheckBox)e.Item.FindControl("CheckBox1"));
                CheckBox1.Text = cb.Text;
                CheckBox1.CssClass = cb.ID + " bomTask";
                CheckBox1.Checked = cb.Checked;
            }
        }
        private SAPBOMSetupItem ConstructProjectDecisions()
        {
            var item = new SAPBOMSetupItem();
            item.CompassListItemId = iItemId;
            item.LastUpdatedFormName = CompassForm.SAPBOMSetup.ToString();
            foreach (RepeaterItem cbs in rptCheckBoxes.Items)
            {
                if (cbs.ItemType == ListItemType.Item || cbs.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox cb = ((CheckBox)cbs.FindControl("Checkbox1"));
                    string cbID = cb.CssClass.Replace(" bomTask", "");
                    cbID = cbID.Replace("chk", "");
                    string value = cb.Checked ? "Yes" : "No";
                    item.GetType().GetProperty(cbID).SetValue(item, value);
                }

            }

            return item;
        }
        #endregion

        #region Button Methods  
        protected void lnlIPF_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Utilities.RedirectPageForm(GlobalConstants.PAGE_ItemProposal, ProjectNumber), false);
        }
        protected void lbHelpDeskEmail_Click(object sender, EventArgs e)
        {
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "Preliminary SAP Initial Item Setup Form");
            this.divAccessDenied.Visible = false;
            this.divAccessRequest.Visible = true;
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            string sapNumber = string.Empty;
            if (!string.IsNullOrEmpty(txtSAPItem.Text))
            {
                sapNumber = txtSAPItem.Text.Trim();
            }
            else
            {
                ErrorSummary.AddError("Please enter a valid SAP number!", this.Page);
                return;
            }

            SetLookupValues(sapNumber, true);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userMgmtService.HasWriteAccess(CompassForm.SAPInitialSetup))
                {
                    ErrorSummary.AddError("You do not have proper access rights to save this page!", this.Page);
                    return;
                }
                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }
                SAPInitialItemSetUp item = ConstructFormData();
                sapInitialISetUpService.UpdateSAPInitialSetupItem(item, CompassForm.PrelimSAPInitialSetup.ToString());

                SAPBOMSetupItem sapbomsetupItem = ConstructProjectDecisions();
                sapBOMSetupService.UpdatePrelimSAPInitialItemSetup(sapbomsetupItem);

                ApprovalItem approvalItem = ConstructApprovalData();
                sapInitialISetUpService.UpdatePrelimSAPInitialSetupApprovalItem(approvalItem, false);
                lblSavedMessage.Text = "Changes Saved: " + DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Preliminary SAP Initial Item SetUp" + ": btnSave_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, "Preliminary SAP Initial Item SetUp", "btnSave_Click");
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userMgmtService.HasWriteAccess(CompassForm.SAPInitialSetup))
                {
                    ErrorSummary.AddError("You do not have proper access rights to save this page!", this.Page);
                    return;
                }
                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }
                SAPInitialItemSetUp item = ConstructFormData();
                sapInitialISetUpService.UpdateSAPInitialSetupItem(item, CompassForm.PrelimSAPInitialSetup.ToString());

                SAPBOMSetupItem sapbomsetupItem = ConstructProjectDecisions();
                sapBOMSetupService.UpdatePrelimSAPInitialItemSetup(sapbomsetupItem);

                ApprovalItem approvalItem = ConstructApprovalData();
                sapInitialISetUpService.UpdatePrelimSAPInitialSetupApprovalItem(approvalItem, true);

                // Complete the workflow task
                workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.PrelimSAPInitialSetup);

                // Redirect to Home page after successfull Submit                        
                Page.Response.Redirect(Utilities.RedirecttoHomePage(), false);
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Preliminary SAP Initial Item SetUp" + ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, "Preliminary SAP Initial Item SetUp", "btnSubmit_Click");
            }
        }

        #endregion
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
                dtFGItem = shipperFinishedGoodService.GetShipperFinishedGoodItems(itemId);
                ViewState["FGShipperItemTable"] = dtFGItem;
            }
            else
                dtFGItem = (List<ShipperFinishedGoodItem>)ViewState["FGShipperItemTable"];
            return dtFGItem;
        }
    }
}
