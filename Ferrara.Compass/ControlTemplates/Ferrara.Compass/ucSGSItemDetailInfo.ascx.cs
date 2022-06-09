using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Classes;
using Microsoft.Practices.Unity;
using Microsoft.SharePoint;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ferrara.Compass.ControlTemplates.Ferrara.Compass
{
    public partial class ucSGSItemDetailInfo : UserControl
    {
        #region Member Variables
        private IStageGateCreateProjectService sgsCreateProjectService;
        private IStageGateGeneralService sgsGeneralService;
        private IItemProposalService IPFService;
        private IUtilityService utilityService;
        private IExceptionService exceptionService;
        private string webUrl;
        #endregion

        #region Properties
        private string ParentProjectNumber
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo];
                return string.Empty;
            }
        }
        public string childProjectNo { get; set; }
        public bool IPFGenerated { get; set; }
        public int StageGateItemId { get; set; }
        public int StageGateChildItemId { get; set; }
        public bool firstLoad { get; set; }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            hdnChildProjectNo.Value = childProjectNo;
            hdnGenerated.Value = IPFGenerated.ToString().ToLower();
            hdnParentProjectNo.Value = ParentProjectNumber;
            hdnStageGateItemId.Value = StageGateItemId.ToString();
            hdnStageGateChildItemId.Value = StageGateChildItemId.ToString();
            webUrl = SPContext.Current.Web.Url;

            try
            {
                if (firstLoad == true)
                {
                    //if (ddlBrand_Material.Items.Count < 2)
                    //{
                    using (SPSite site = new SPSite(webUrl))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            Utilities.BindDropDownItems(ddlChannel, GlobalConstants.LIST_ChannelLookup, web.Url);
                            Utilities.BindDropDownItemsAddValues(ddlCustomer, GlobalConstants.LIST_CustomersLookup, web.Url);
                            Utilities.BindDropDownItems(ddlMaterialGroup4, GlobalConstants.LIST_MaterialGroup4Lookup, web.Url);
                            Utilities.BindDropDownItems(ddlMaterialGroup5, GlobalConstants.LIST_MaterialGroup5Lookup, webUrl);

                            Utilities.BindDropDownItems(ddlProductHierarchyLevel1, GlobalConstants.LIST_ProductHierarchyLevel1Lookup, webUrl);
                            Utilities.BindDropDownItems(ddlSAPBUOM, GlobalConstants.LIST_SAPBaseUOMLookup, webUrl);

                            InitializeScreen();

                        }
                    }
                    //}
                    if (StageGateItemId > 0)
                    {
                        LoadControlData();
                    }
                }
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "ucSGSItemDetailInfo", "Page_Load");
            }
        }
        private void InitializeScreen()
        {
        }
        private void EnsureScriptManager()
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);

            if (scriptManager == null)
            {
                scriptManager = new ScriptManager();
                scriptManager.EnablePartialRendering = true;

                if (Page.Form != null)
                {
                    Page.Form.Controls.AddAt(0, scriptManager);
                }
            }
        }
        private void EnsureUpdatePanelFixups()
        {
            if (this.Page.Form != null)
            {
                String formOnSubmitAtt = this.Page.Form.Attributes["onsubmit"];
                if (formOnSubmitAtt == "return _spFormOnSubmitWrapper ();")
                {
                    this.Page.Form.Attributes["onsubmit"] = "_spFormOnSubmitWrapper();";
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                "UpdatePanelFixup", "_spOriginalFormAction = document.forms[0].action; _spSuppressFormOnSubmitWrapper = true;", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SGSConditionalChecks", "SGSConditionalChecks();", true);

        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeScreen();
            // Page.EnableEventValidation = false;

            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            sgsCreateProjectService = DependencyResolution.DependencyMapper.Container.Resolve<IStageGateCreateProjectService>();
            sgsGeneralService = DependencyResolution.DependencyMapper.Container.Resolve<IStageGateGeneralService>();
            IPFService = DependencyResolution.DependencyMapper.Container.Resolve<IItemProposalService>();

            ddlNeedNewUnitUPC.Attributes.Add("onchange", "SGSConditionalChecks();");
            ddlBrand_Material.Attributes.Add("onchange", "ChangeBrandChecks();SAPNomenclature();");
            ddlCustomerSpecific.Attributes.Add("onchange", "SGSConditionalChecks();SAPNomenclature();");
            ddlNeedNewUPCUCC.Attributes.Add("onchange", "SGSConditionalChecks();");
            ddlNeedNewUnitUPC.Attributes.Add("onchange", "SGSConditionalChecks();");
            ddlNeedNewDisplayBoxUPC.Attributes.Add("onchange", "SGSConditionalChecks();");
            ddlSAPBUOM.Attributes.Add("onchange", "SGSConditionalChecks();");
            ddlNeedNewCaseUCC.Attributes.Add("onchange", "SGSConditionalChecks();");
            ddlNeedNewPalletUCC.Attributes.Add("onchange", "SGSConditionalChecks();");
            ddlTBDIndicator.Attributes.Add("onchange", "SGSConditionalChecks();SAPNomenclature(ddlTBDIndicator);");
            ddlMaterialGroup4.Attributes.Add("onchange", "SGSConditionalChecks();");
            ddlMaterialGroup5.Attributes.Add("onchange", "SGSConditionalChecks();SAPNomenclature();");
            ddlProductHierarchyLevel1.Attributes.Add("onchange", "SAPNomenclature();");
            ddlManuallyCreateSAPDescription.Attributes.Add("onchange", "SAPNomenclature();");
            ddlProductHierarchyLevel2.Attributes.Add("onchange", "SAPNomenclature();");
            ddlCustomer.Attributes.Add("onchange", "SAPNomenclature();");
            ddlFGReplacingAnExistingFG.Attributes.Add("onchange", "SGSConditionalChecks();");
            ddlIsThisAnLTOItem.Attributes.Add("onchange", "SGSConditionalChecks();");
            EnsureScriptManager();
            EnsureUpdatePanelFixups();
        }
        private void LoadControlData()
        {
            try
            {
                ItemProposalItem childProjectDetailsItem = new ItemProposalItem();

                if (IPFGenerated)
                {
                    pnProjectNo.Controls.Add(new LiteralControl(childProjectNo));
                    childProjectDetailsItem = IPFService.GetItemProposalItem(StageGateChildItemId);
                }
                else
                {
                    childProjectDetailsItem = sgsGeneralService.GetTempIPFItem(StageGateChildItemId, StageGateItemId);
                }

                Utilities.SetDropDownValue(childProjectDetailsItem.TBDIndicator, this.ddlTBDIndicator, this.Page);
                txtSAPItemNumber.Text = childProjectDetailsItem.SAPItemNumber;
                txtSAPItemDescription.Text = childProjectDetailsItem.SAPDescription;
                Utilities.SetDropDownValue(childProjectDetailsItem.ProductHierarchyLevel1, this.ddlProductHierarchyLevel1, this.Page);
                Utilities.SetDropDownValue(childProjectDetailsItem.ManuallyCreateSAPDescription, this.ddlManuallyCreateSAPDescription, this.Page);
                Utilities.SetDropDownValue(childProjectDetailsItem.ProductHierarchyLevel1, this.ddlProductHierarchyLevel1, this.Page);
                ddlProductHierarchyLevel1_SelectedIndexChanged(null, null);

                Utilities.SetDropDownValue(childProjectDetailsItem.ProductHierarchyLevel2, this.ddlProductHierarchyLevel2, this.Page);

                // Load the level 2 list
                ddlProductHierarchyLevel2_SelectedIndexChanged(null, null);

                // Load Brand List
                if (childProjectDetailsItem.MaterialGroup1Brand != "Multiple")
                {
                    Utilities.SetDropDownValue(childProjectDetailsItem.MaterialGroup1Brand, this.ddlBrand_Material, this.Page);
                }
                Utilities.SetDropDownValue(childProjectDetailsItem.MaterialGroup4ProductForm, this.ddlMaterialGroup4, this.Page);
                Utilities.SetDropDownValue(childProjectDetailsItem.MaterialGroup5PackType, this.ddlMaterialGroup5, this.Page);
                Utilities.SetDropDownValue(childProjectDetailsItem.RequireNewUPCUCC, this.ddlNeedNewUPCUCC, this.Page);
                Utilities.SetDropDownValue(childProjectDetailsItem.RequireNewUnitUPC, this.ddlNeedNewUnitUPC, this.Page);
                txtUnitUPC.Text = childProjectDetailsItem.UnitUPC;
                Utilities.SetDropDownValue(childProjectDetailsItem.RequireNewDisplayBoxUPC, this.ddlNeedNewDisplayBoxUPC, this.Page);
                txtDisplayUPCBox.Text = childProjectDetailsItem.DisplayBoxUPC;
                Utilities.SetDropDownValue(childProjectDetailsItem.SAPBaseUOM, this.ddlSAPBUOM, this.Page);
                Utilities.SetDropDownValue(childProjectDetailsItem.RequireNewCaseUCC, this.ddlNeedNewCaseUCC, this.Page);
                txtCaseUCC.Text = childProjectDetailsItem.CaseUCC;
                Utilities.SetDropDownValue(childProjectDetailsItem.RequireNewPalletUCC, this.ddlNeedNewPalletUCC, this.Page);
                txtPalletUCC.Text = childProjectDetailsItem.PalletUCC;

                Utilities.SetDropDownValue(childProjectDetailsItem.FGReplacingAnExistingFG, this.ddlFGReplacingAnExistingFG, this.Page);
                Utilities.SetDropDownValue(childProjectDetailsItem.IsThisAnLTOItem, this.ddlIsThisAnLTOItem, this.Page);
                Utilities.SetDropDownValue(childProjectDetailsItem.RequestChangeToFGNumForSameUCC, this.ddlRequestChangeToFGNumForSameUCC, this.Page);
                txtLTOTransitionStartWindowRDD.Text = Utilities.GetDateForDisplay(childProjectDetailsItem.LTOTransitionStartWindowRDD);
                txtLTOTransitionEndWindowRDD.Text = Utilities.GetDateForDisplay(childProjectDetailsItem.LTOTransitionEndWindowRDD);
                Utilities.SetDropDownValue(childProjectDetailsItem.LTOEndDateFlexibility, this.ddlLTOEndDateFlexibility, this.Page);

                Utilities.SetDropDownValue(childProjectDetailsItem.CustomerSpecific, this.ddlCustomerSpecific, this.Page);
                Utilities.SetDropDownValueMatchWithoutCodes(childProjectDetailsItem.Customer, this.ddlCustomer, this.Page);
                Utilities.SetDropDownValue(childProjectDetailsItem.Channel, this.ddlChannel, this.Page);
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "ucSGSItemDetailInfo", "LoadControlData");
            }
            firstLoad = false;
        }
        private ItemProposalItem ConstructFormData()
        {
            ItemProposalItem childIPFItem = new ItemProposalItem();
            try
            {
                childIPFItem.TBDIndicator = ddlTBDIndicator.SelectedItem.Text;
                childIPFItem.SAPItemNumber = txtSAPItemNumber.Text;
                childIPFItem.SAPDescription = txtSAPItemDescription.Text;
                childIPFItem.ProductHierarchyLevel1 = ddlProductHierarchyLevel1.SelectedItem.Text;
                childIPFItem.ManuallyCreateSAPDescription = ddlManuallyCreateSAPDescription.SelectedItem.Text;
                childIPFItem.ProductHierarchyLevel2 = ddlProductHierarchyLevel2.SelectedItem.Text;
                childIPFItem.MaterialGroup1Brand = ddlBrand_Material.SelectedItem.Text;
                childIPFItem.MaterialGroup4ProductForm = ddlMaterialGroup4.SelectedItem.Text;
                childIPFItem.MaterialGroup5PackType = ddlMaterialGroup5.SelectedItem.Text;
                childIPFItem.RequireNewUPCUCC = ddlNeedNewUPCUCC.SelectedItem.Text;
                childIPFItem.RequireNewUnitUPC = ddlNeedNewUnitUPC.SelectedItem.Text;
                childIPFItem.UnitUPC = txtUnitUPC.Text;
                childIPFItem.RequireNewDisplayBoxUPC = ddlNeedNewDisplayBoxUPC.SelectedItem.Text;
                childIPFItem.DisplayBoxUPC = txtDisplayUPCBox.Text;
                childIPFItem.SAPBaseUOM = ddlSAPBUOM.SelectedItem.Text;
                childIPFItem.RequireNewCaseUCC = ddlNeedNewCaseUCC.SelectedItem.Text;
                childIPFItem.CaseUCC = txtCaseUCC.Text;
                childIPFItem.RequireNewPalletUCC = ddlNeedNewPalletUCC.SelectedItem.Text;
                childIPFItem.PalletUCC = txtPalletUCC.Text;
                childIPFItem.CustomerSpecific = ddlCustomerSpecific.SelectedItem.Text;

                childIPFItem.FGReplacingAnExistingFG = ddlFGReplacingAnExistingFG.SelectedItem.Text;
                childIPFItem.IsThisAnLTOItem = ddlIsThisAnLTOItem.SelectedItem.Text;
                childIPFItem.RequestChangeToFGNumForSameUCC = ddlRequestChangeToFGNumForSameUCC.SelectedItem.Text;
                childIPFItem.LTOTransitionStartWindowRDD = string.IsNullOrEmpty(txtLTOTransitionStartWindowRDD.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtLTOTransitionStartWindowRDD.Text.Trim());
                childIPFItem.LTOTransitionEndWindowRDD = string.IsNullOrEmpty(txtLTOTransitionEndWindowRDD.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtLTOTransitionEndWindowRDD.Text.Trim());
                childIPFItem.LTOEndDateFlexibility = ddlLTOEndDateFlexibility.SelectedItem.Text;

                var customerList = ddlCustomer.SelectedItem.Text.Split('(');
                if (customerList != null)
                {
                    childIPFItem.Customer = customerList[0].Trim();
                }

                childIPFItem.Channel = ddlChannel.SelectedItem.Text;
                childIPFItem.CompassListItemId = StageGateChildItemId;
                childIPFItem.StageGateProjectListItemId = StageGateItemId;
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "IPF ITEM: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "ChildIPFEditForm", "ConstructFormData");
            }

            return childIPFItem;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdnGenerated.Value == "false")
                {
                    ItemProposalItem item = ConstructFormData();
                    sgsGeneralService.updateChildItemDetails(item);
                }
                dvMain.Visible = false;
                var hdnUCLoaded = (HiddenField)this.Parent.FindControl("hdnUCLoaded");
                hdnUCLoaded.Value = "false";
                Page.Response.Redirect(Utilities.RedirectPageForm(Utilities.GetCurrentPageName(), ParentProjectNumber), false);
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "ucSGSItemDetailInfo", "btnSave_Click");
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Utilities.RedirectPageForm(Utilities.GetCurrentPageName(), ParentProjectNumber), false);
        }
        protected void ddlProductHierarchyLevel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadProductHierarchyLevel2(this.ddlProductHierarchyLevel1.SelectedItem.Text);

        }
        private void ReloadProductHierarchyLevel2(string productHierarhcyLevel1)
        {
            // Get the Product Hierarchy Level 1 Value
            string level2 = Utilities.GetLookupValue(GlobalConstants.LIST_ProductHierarchyLevel1Lookup, productHierarhcyLevel1, webUrl);

            if ((!string.IsNullOrEmpty(level2)) && (!string.Equals(level2, "Select...")))
            {
                Utilities.BindDropDownItemsByValue(ddlProductHierarchyLevel2, GlobalConstants.LIST_ProductHierarchyLevel2Lookup, level2, webUrl);
                this.ddlProductHierarchyLevel2.SelectedIndex = -1;
            }
            else
            {
                this.ddlProductHierarchyLevel2.Items.Clear();
                ListItem li = new ListItem();
                li.Text = "Select...";
                li.Value = "-1";
                this.ddlProductHierarchyLevel2.Items.Add(li);
            }
            this.ddlBrand_Material.Items.Clear();
            ListItem li2 = new ListItem();
            li2.Text = "Select...";
            li2.Value = "-1";
            this.ddlBrand_Material.Items.Add(li2);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ReloadProductHierarchyLevel2", "SAPNomenclature();", true);
        }
        protected void ddlProductHierarchyLevel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadBrand(this.ddlProductHierarchyLevel2.SelectedItem.Text);

        }
        private void ReloadBrand(string productHierarhcyLevel2)
        {
            if ((!string.IsNullOrEmpty(productHierarhcyLevel2)) && (!string.Equals(productHierarhcyLevel2, "Select...")))
            {
                Utilities.BindDropDownItemsByValueAndColumn(ddlBrand_Material, GlobalConstants.LIST_MaterialGroup1Lookup, "ParentPHL2", productHierarhcyLevel2, webUrl);
                this.ddlBrand_Material.SelectedIndex = -1;
            }
            else
            {
                this.ddlBrand_Material.Items.Clear();
                ListItem li = new ListItem();
                li.Text = "Select...";
                li.Value = "-1";
                this.ddlBrand_Material.Items.Add(li);

            }
            //LoadControlData();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ReloadProductHierarchyLevel2", "SAPNomenclature();", true);
        }
    }
}
