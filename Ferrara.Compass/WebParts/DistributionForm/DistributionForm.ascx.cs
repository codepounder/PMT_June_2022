using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Practices.Unity;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.DependencyResolution;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Classes;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Ferrara.Compass.WebParts.DistributionForm
{
    [ToolboxItemAttribute(false)]
    public partial class DistributionForm : WebPart
    {
        #region Member Variables
        private IDistributionService distributionService;
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private IWorkflowService workflowService;
        private INotificationService notificationService;
        private IUserManagementService userMgmtService;
        private IShipperFinishedGoodService shipperFinishedGoodService;
        private IMixesService mixesService;
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

        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public DistributionForm()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            distributionService = DependencyResolution.DependencyMapper.Container.Resolve<IDistributionService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
            userMgmtService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
            shipperFinishedGoodService = DependencyResolution.DependencyMapper.Container.Resolve<IShipperFinishedGoodService>();
            mixesService = DependencyResolution.DependencyMapper.Container.Resolve<IMixesService>();
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

                    LoadFormData();
                    InitializeScreen();
                    if (hdnProjectType.Value.Contains("Renovations"))
                    {
                        dvMain.Visible = false;
                        dvMsg.Visible = true;
                    }
                }
                catch (Exception exception)
                {
                    ErrorSummary.AddError(exception.Message, this.Page);
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.Distribution.ToString() + ": " + exception.Message);
                    exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.Distribution.ToString(), "Page_Load");
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
            return true;
        }
        private void InitializeScreen()
        {
            // If user does not belong to a valid group for the page, inform them that they do not hvae access rights
            if (!userMgmtService.HasReadAccess(CompassForm.Distribution))
            {
                this.divAccessDenied.Visible = true;
            }

            // If user does not have rights to save/submit the page, disable the Save and Submit buttons
            if (!userMgmtService.HasWriteAccess(CompassForm.Distribution))
            {
                this.btnSave.Enabled = false;
                this.btnSubmit.Enabled = false;
            }

            string workflowPhase = utilityService.GetWorkflowPhase(iItemId);
            if ((string.Equals(workflowPhase, WorkflowStep.OnHold.ToString())) || (string.Equals(workflowPhase, WorkflowStep.Cancelled.ToString())) ||
                (string.Equals(workflowPhase, WorkflowStep.Completed.ToString())))
            {
                this.btnSave.Enabled = false;
                this.btnSubmit.Enabled = false;
            }
        }
        #endregion
        #region Data Transfer Methods
        private void LoadFormData()
        {
            DistributionItem item = distributionService.GetDistributionItem(iItemId);
            hdnProjectType.Value = item.ProjectType;

            this.lblProjectType.Text = item.ProjectType;
            this.lblProjectTypeSubCategory.Text = item.ProjectTypeSubCategory;
            this.lblSoldOutsideUSA.Text = item.SoldOutsideUSA;
            this.lblCountryOfSale.Text = item.CountryOfSale;
            this.lblLikeItemNumber.Text = item.LikeItemNumber;
            this.lblLikeItemDescription.Text = item.LikeItemDescription;
            this.lblItemConcept.Text = item.ItemConcept;

            this.lblManufacturingLocation.Text = item.ManufacturingLocation;
            this.lblPackingLocation.Text = item.PackingLocation;
            this.lblProcurementType.Text = item.ProcurementType;
            this.lblExternalManufacturer.Text = item.ExternalManufacturer;
            this.lblExternalPacker.Text = item.ExternalPacker;
            this.lblPurchasedIntoLocation.Text = item.PurchasedIntoLocation;
            this.lblSAPBaseUOM.Text = item.SAPBaseUOM;

            this.lblProductHierarchyLevel1.Text = item.ProductHierarchyLevel1;
            this.lblProductHierarchyLevel2.Text = item.ProductHierarchyLevel2;
            this.lblMaterialGroup1.Text = item.MaterialGroup1Brand;
            this.lblMaterialGroup4.Text = item.MaterialGroup4ProductForm;
            this.lblMaterialGroup5.Text = item.MaterialGroup5PackType;

            LoadControlsByCompanyCode(item);

            #region Hide Controls
            if (this.lblCountryOfSale.Text == "" || this.lblCountryOfSale.Text == "Select...")
            {
                divCountryOfSale.Visible = false;
            }
            else
            {
                divCountryOfSale.Visible = true;
            }

            if (this.lblLikeItemNumber.Text == "" || this.lblLikeItemNumber.Text == "Select...")
            {
                divLikeItemNumber.Visible = false;
            }
            else
            {
                divLikeItemNumber.Visible = true;
            }

            if (this.lblLikeItemDescription.Text == "" || this.lblLikeItemDescription.Text == "Select...")
            {
                divLikeItemDescription.Visible = false;
            }
            else
            {
                divLikeItemDescription.Visible = true;
            }

            if (this.lblProcurementType.Text == "" || this.lblProcurementType.Text == "Select...")
            {
                divProcurementType.Visible = false;
            }
            else
            {
                divProcurementType.Visible = true;
            }

            if (this.lblExternalManufacturer.Text == "" || this.lblExternalManufacturer.Text == "Select...")
            {
                divExternalManufacturer.Visible = false;
            }
            else
            {
                divExternalManufacturer.Visible = true;
            }

            if (this.lblExternalPacker.Text == "" || this.lblExternalPacker.Text == "Select...")
            {
                divExternalPacker.Visible = false;
            }
            else
            {
                divExternalPacker.Visible = true;
            }

            if (this.lblPurchasedIntoLocation.Text == "" || this.lblPurchasedIntoLocation.Text == "Select...")
            {
                divPurchasedIntoLocation.Visible = false;
            }
            else
            {
                divPurchasedIntoLocation.Visible = true;
            }
            #endregion

            List<ShipperFinishedGoodItem> shipperData;
            if (item.MaterialGroup5PackType.ToLower() == "shipper (shp)" || item.MaterialGroup5PackType.ToLower() == "shippers (shp)")
            {
                shipperData = GetShipperFGItem(iItemId);
                if (shipperData.Count > 0)
                {
                    divShipper.Visible = true;
                    rpShipperSummary.DataSource = shipperData;
                    rpShipperSummary.DataBind();
                }
            }

            List<MixesItem> mixData;
            if (item.MaterialGroup4ProductForm == "MIXES (MIX)")
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
        private void LoadControlsByCompanyCode(DistributionItem item)
        {
            divSELLDCs.Visible = false;
            DivFERQDCs.Visible = false;
            #region SellDCs
            if (this.lblProductHierarchyLevel1.Text != GlobalConstants.PRODUCT_HIERARCHY1_LBB)
            {
                divSELLDCs.Visible = true;
                DivFERQDCs.Visible = false;
                LoadDropdownsByCompanyCode("SellDCs", "SellDCsSPKs");
                Utilities.SetDropDownValue(item.DeploymentModeofItem, this.ddlDeploymentModeofItem, this.Page);
                Utilities.SetDropDownValue(item.DesignateHUBDC, this.ddlDesignateHUBDC, this.Page);
                Utilities.SetDropDownValue(item.ExtendtoSL07, this.ddlExtendtoSL07, this.Page);
                Utilities.SetDropDownValue(item.SetSL07SPKto, this.ddlSetSL07SPKto, this.Page);
                Utilities.SetDropDownValue(item.ExtendtoSL13, this.ddlExtendtoSL13, this.Page);
                Utilities.SetDropDownValue(item.SetSL13SPKto, this.ddlSetSL13SPKto, this.Page);
                Utilities.SetDropDownValue(item.ExtendtoSL18, this.ddlExtendtoSL18, this.Page);
                Utilities.SetDropDownValue(item.SetSL18SPKto, this.ddlSetSL18SPKto, this.Page);
                Utilities.SetDropDownValue(item.ExtendtoSL19, this.ddlExtendtoSL19, this.Page);
                Utilities.SetDropDownValue(item.SetSL19SPKto, this.ddlSetSL19SPKto, this.Page);
                Utilities.SetDropDownValue(item.ExtendtoSL30, this.ddlExtendtoSL30, this.Page);
                Utilities.SetDropDownValue(item.SetSL30SPKto, this.ddlSetSL30SPKto, this.Page);
                Utilities.SetDropDownValue(item.ExtendtoSL14, this.ddlExtendtoSL14, this.Page);
                Utilities.SetDropDownValue(item.SetSL14SPKto, this.ddlSetSL14SPKto, this.Page);
            }
            #endregion
            #region FERQDCs
            else
            {
                divSELLDCs.Visible = false;
                DivFERQDCs.Visible = true;
                LoadDropdownsByCompanyCode("FERQDCs", "FERQDCsSPKs");
                Utilities.SetDropDownValue(item.DeploymentModeofItem, this.ddlDeploymentModeofItem, this.Page);
                Utilities.SetDropDownValue(item.DesignateHUBDC, this.ddlDesignateHUBDC, this.Page);
                Utilities.SetDropDownValue(item.ExtendtoFQ26, this.ddlExtendtoFQ26, this.Page);
                Utilities.SetDropDownValue(item.SetFQ26SPKto, this.ddlSetFQ26SPKto, this.Page);
                Utilities.SetDropDownValue(item.ExtendtoFQ27, this.ddlExtendtoFQ27, this.Page);
                Utilities.SetDropDownValue(item.SetFQ27SPKto, this.ddlSetFQ27SPKto, this.Page);
                Utilities.SetDropDownValue(item.ExtendtoFQ28, this.ddlExtendtoFQ28, this.Page);
                Utilities.SetDropDownValue(item.SetFQ28SPKto, this.ddlSetFQ28SPKto, this.Page);
                Utilities.SetDropDownValue(item.ExtendtoFQ29, this.ddlExtendtoFQ29, this.Page);
                Utilities.SetDropDownValue(item.SetFQ29SPKto, this.ddlSetFQ29SPKto, this.Page);
                Utilities.SetDropDownValue(item.ExtendtoFQ34, this.ddlExtendtoFQ34, this.Page);
                Utilities.SetDropDownValue(item.SetFQ34SPKto, this.ddlSetFQ34SPKto, this.Page);
                Utilities.SetDropDownValue(item.ExtendtoFQ35, this.ddlExtendtoFQ35, this.Page);
                Utilities.SetDropDownValue(item.SetFQ35SPKto, this.ddlSetFQ35SPKto, this.Page);
            }
            #endregion
        }

        private void LoadDropdownsByCompanyCode(string designateDCs, string columnNamne)
        {
            Utilities.BindDropDownWithColumnFilter(ddlDeploymentModeofItem, GlobalConstants.LIST_DistributionDeploymentModesLookup, SPContext.Current.Web.Url, designateDCs);
            Utilities.BindDropDownWithColumnFilter(ddlDesignateHUBDC, GlobalConstants.LIST_DistributionLookup, SPContext.Current.Web.Url, designateDCs);
            if (this.lblProductHierarchyLevel1.Text != GlobalConstants.PRODUCT_HIERARCHY1_LBB)
            {
                Utilities.BindDropDownWithColumnFilter(ddlSetSL07SPKto, GlobalConstants.LIST_DistributionLookup, SPContext.Current.Web.Url, columnNamne);
                Utilities.BindDropDownWithColumnFilter(ddlSetSL13SPKto, GlobalConstants.LIST_DistributionLookup, SPContext.Current.Web.Url, columnNamne);
                Utilities.BindDropDownWithColumnFilter(ddlSetSL18SPKto, GlobalConstants.LIST_DistributionLookup, SPContext.Current.Web.Url, columnNamne);
                Utilities.BindDropDownWithColumnFilter(ddlSetSL19SPKto, GlobalConstants.LIST_DistributionLookup, SPContext.Current.Web.Url, columnNamne);
                Utilities.BindDropDownWithColumnFilter(ddlSetSL30SPKto, GlobalConstants.LIST_DistributionLookup, SPContext.Current.Web.Url, columnNamne);
                Utilities.BindDropDownWithColumnFilter(ddlSetSL14SPKto, GlobalConstants.LIST_DistributionLookup, SPContext.Current.Web.Url, columnNamne);
            }
            else
            {
                Utilities.BindDropDownWithColumnFilter(ddlSetFQ26SPKto, GlobalConstants.LIST_DistributionLookup, SPContext.Current.Web.Url, columnNamne);
                Utilities.BindDropDownWithColumnFilter(ddlSetFQ27SPKto, GlobalConstants.LIST_DistributionLookup, SPContext.Current.Web.Url, columnNamne);
                Utilities.BindDropDownWithColumnFilter(ddlSetFQ28SPKto, GlobalConstants.LIST_DistributionLookup, SPContext.Current.Web.Url, columnNamne);
                Utilities.BindDropDownWithColumnFilter(ddlSetFQ29SPKto, GlobalConstants.LIST_DistributionLookup, SPContext.Current.Web.Url, columnNamne);
                Utilities.BindDropDownWithColumnFilter(ddlSetFQ34SPKto, GlobalConstants.LIST_DistributionLookup, SPContext.Current.Web.Url, columnNamne);
                Utilities.BindDropDownWithColumnFilter(ddlSetFQ35SPKto, GlobalConstants.LIST_DistributionLookup, SPContext.Current.Web.Url, columnNamne);
            }
        }

        protected void rpMixesSummary_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Label lblOzPerPiece;
            MixesItem mix;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                mix = (MixesItem)e.Item.DataItem;
                lblOzPerPiece = (Label)e.Item.FindControl("lblOzPerPiece");
                lblOzPerPiece.Text = (mix.NumberOfPieces * mix.OuncesPerPiece).ToString();
            }
        }
        private DistributionItem ConstructFormData()
        {
            var item = new DistributionItem();

            item.CompassListItemId = iItemId;
            item.ProjectNumber = ProjectNumber;
            item.DesignateHUBDC = this.ddlDesignateHUBDC.SelectedItem.Text;
            item.DeploymentModeofItem = this.ddlDeploymentModeofItem.SelectedItem.Text;
            #region SellDCs
            if (this.lblProductHierarchyLevel1.Text != GlobalConstants.PRODUCT_HIERARCHY1_LBB)
            {
                item.ExtendtoSL07 = this.ddlExtendtoSL07.SelectedItem.Text;
                if (item.ExtendtoSL07 == "Yes")
                {
                    item.SetSL07SPKto = this.ddlSetSL07SPKto.SelectedItem.Text;
                }
                else
                {
                    item.SetSL07SPKto = "";
                }

                item.ExtendtoSL13 = this.ddlExtendtoSL13.SelectedItem.Text;
                if (item.ExtendtoSL13 == "Yes")
                {
                    item.SetSL13SPKto = this.ddlSetSL13SPKto.SelectedItem.Text;
                }
                else
                {
                    item.SetSL13SPKto = "";
                }

                item.ExtendtoSL18 = this.ddlExtendtoSL18.SelectedItem.Text;
                if (item.ExtendtoSL18 == "Yes")
                {
                    item.SetSL18SPKto = this.ddlSetSL18SPKto.SelectedItem.Text;
                }
                else
                {
                    item.SetSL18SPKto = "";
                }

                item.ExtendtoSL19 = this.ddlExtendtoSL19.SelectedItem.Text;
                if (item.ExtendtoSL19 == "Yes")
                {
                    item.SetSL19SPKto = this.ddlSetSL19SPKto.SelectedItem.Text;
                }
                else
                {
                    item.SetSL19SPKto = "";
                }

                item.ExtendtoSL30 = this.ddlExtendtoSL30.SelectedItem.Text;
                if (item.ExtendtoSL30 == "Yes")
                {
                    item.SetSL30SPKto = this.ddlSetSL30SPKto.SelectedItem.Text;
                }
                else
                {
                    item.SetSL30SPKto = "";
                }

                item.ExtendtoSL14 = this.ddlExtendtoSL14.SelectedItem.Text;
                if (item.ExtendtoSL14 == "Yes")
                {
                    item.SetSL14SPKto = this.ddlSetSL14SPKto.SelectedItem.Text;
                }
                else
                {
                    item.SetSL14SPKto = "";
                }

                item.ExtendtoFQ26 = "";
                item.SetFQ26SPKto = "";
                item.ExtendtoFQ27 = "";
                item.SetFQ27SPKto = "";
                item.ExtendtoFQ28 = "";
                item.SetFQ28SPKto = "";
                item.ExtendtoFQ29 = "";
                item.SetFQ29SPKto = "";
                item.ExtendtoFQ34 = "";
                item.SetFQ34SPKto = "";
                item.ExtendtoFQ35 = "";
                item.SetFQ35SPKto = "";
            }
            #endregion
            #region FERQDCs
            else
            {
                item.ExtendtoSL07 = "";
                item.SetSL07SPKto = "";
                item.ExtendtoSL13 = "";
                item.SetSL13SPKto = "";
                item.ExtendtoSL18 = "";
                item.SetSL18SPKto = "";
                item.ExtendtoSL19 = "";
                item.SetSL19SPKto = "";
                item.ExtendtoSL30 = "";
                item.SetSL30SPKto = "";
                item.ExtendtoSL14 = "";
                item.SetSL14SPKto = "";

                item.ExtendtoFQ26 = this.ddlExtendtoFQ26.SelectedItem.Text;
                if (item.ExtendtoFQ26 == "Yes")
                {
                    item.SetFQ26SPKto = this.ddlSetFQ26SPKto.SelectedItem.Text;
                }
                else
                {
                    item.SetFQ26SPKto = "";
                }

                item.ExtendtoFQ27 = this.ddlExtendtoFQ27.SelectedItem.Text;
                if (item.ExtendtoFQ27 == "Yes")
                {
                    item.SetFQ27SPKto = this.ddlSetFQ27SPKto.SelectedItem.Text;
                }
                else
                {
                    item.SetFQ27SPKto = "";
                }

                item.ExtendtoFQ28 = this.ddlExtendtoFQ28.SelectedItem.Text;
                if (item.ExtendtoFQ28 == "Yes")
                {
                    item.SetFQ28SPKto = this.ddlSetFQ28SPKto.SelectedItem.Text;
                }
                else
                {
                    item.SetFQ28SPKto = "";
                }

                item.ExtendtoFQ29 = this.ddlExtendtoFQ29.SelectedItem.Text;
                if (item.ExtendtoFQ29 == "Yes")
                {
                    item.SetFQ29SPKto = this.ddlSetFQ29SPKto.SelectedItem.Text;
                }
                else
                {
                    item.SetFQ29SPKto = "";
                }

                item.ExtendtoFQ34 = this.ddlExtendtoFQ34.SelectedItem.Text;
                if (item.ExtendtoFQ34 == "Yes")
                {
                    item.SetFQ34SPKto = this.ddlSetFQ34SPKto.SelectedItem.Text;
                }
                else
                {
                    item.SetFQ34SPKto = "";
                }

                item.ExtendtoFQ35 = this.ddlExtendtoFQ35.SelectedItem.Text;
                if (item.ExtendtoFQ35 == "Yes")
                {
                    item.SetFQ35SPKto = this.ddlSetFQ35SPKto.SelectedItem.Text;
                }
                else
                {
                    item.SetFQ35SPKto = "";
                }
            }
            #endregion

            item.LastUpdatedFormName = CompassForm.Distribution.ToString();

            return item;
        }

        private ApprovalItem ConstructApprovalData()
        {
            var item = new ApprovalItem();

            item.CompassListItemId = iItemId;
            item.ModifiedBy = SPContext.Current.Web.CurrentUser.Name;
            item.ModifiedDate = DateTime.Now.ToString();

            return item;
        }
        #endregion
        #region Button Methods
        protected void lbHelpDeskEmail_Click(object sender, EventArgs e)
        {
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "Distribution");
            this.divAccessDenied.Visible = false;
            this.divAccessRequest.Visible = true;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userMgmtService.HasWriteAccess(CompassForm.Distribution))
                {
                    ErrorSummary.AddError("You do not have proper access rights to save this page!", this.Page);
                    return;
                }
                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }

                // Retrieve the data from the form
                DistributionItem item = ConstructFormData();
                distributionService.UpdateDistributionItem(item);

                ApprovalItem approvalItem = ConstructApprovalData();
                distributionService.UpdateDistributionApprovalItem(approvalItem, false);

                lblSavedMessage.Text = "Changes Saved: " + DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.Distribution.ToString() + ": btnSave_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.Distribution.ToString(), "btnSave_Click");
            }

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userMgmtService.HasWriteAccess(CompassForm.Distribution))
                {
                    ErrorSummary.AddError("You do not have proper access rights to submit this page!", this.Page);
                    return;
                }
                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }

                // Retrieve the data from the form
                DistributionItem item = ConstructFormData();
                distributionService.UpdateDistributionItem(item);

                ApprovalItem approvalItem = ConstructApprovalData();
                distributionService.UpdateDistributionApprovalItem(approvalItem, true);

                // Complete the workflow task
                workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.Distribution);

                // Redirect to Home page after successfull Submit                    
                Page.Response.Redirect(Utilities.RedirecttoHomePage(), false);
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.Distribution.ToString() + ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.Distribution.ToString(), "btnSubmit_Click");
            }
        }
        protected void lnlIPF_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Utilities.RedirectPageForm(GlobalConstants.PAGE_ItemProposal, ProjectNumber), false);
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
                dtFGItem = shipperFinishedGoodService.GetShipperFinishedGoodItems(itemId);
                ViewState["FGShipperItemTable"] = dtFGItem;
            }
            else
                dtFGItem = (List<ShipperFinishedGoodItem>)ViewState["FGShipperItemTable"];
            return dtFGItem;
        }
        #endregion

    }
}
