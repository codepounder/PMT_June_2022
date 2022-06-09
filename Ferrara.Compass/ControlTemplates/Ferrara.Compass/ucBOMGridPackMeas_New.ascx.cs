using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Ferrara.Compass.Classes;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Interfaces;
using System.Web;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Linq;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.ControlTemplates.Ferrara.Compass
{
    public partial class ucBOMGridPackMeas_New : UserControl
    {
        #region Member Variables
        private IExceptionService exceptionService;
        private IBOMSetupService BOMSetupService;
        private IPackagingItemService packagingItemService;
        private string webUrl;
        #endregion

        #region Properties
        public int iItemId { get; set; }
        public List<BOMSetupItem> packagingItems { get; set; }
        public string ProjectNumber { get; set; }
        public CallParentMethod openBtnSave { get; set; }
        public delegate void CallParentMethod();
        public int ParentId { get; set; }
        public string ParentComponentType { get; set; }
        public string ProjectType { get; set; }
        public string ProjectTypeSubCategory { get; set; }
        public BOMSetupItem GridItem { get; set; }
        public string TBDIndicator { get; set; }
        public string LOB { get; set; }
        public string MaterialNumber { get; set; }
        public string MaterialDesc { get; set; }
        public string SemiXferMakeLocation { get; set; }
        public string PackagingComponent { get; set; }
        public int NewComponentCount { get; set; }
        public string MakePackTransferLocation { get; set; }
        public string NewExisting { get; set; }
        #endregion
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeScreen();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            BOMSetupService = DependencyResolution.DependencyMapper.Container.Resolve<IBOMSetupService>();
            packagingItemService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            EnsureScriptManager();
            EnsureUpdatePanelFixups();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            webUrl = SPContext.Current.Web.Url;
            hdnUCBOMComponentType.Value = PackagingComponent;
            if (!Page.IsPostBack)
            {
                LoadBOMItems();
                LoadFormData();
                LoadBOMItemsForPackMeas();
            }
        }

        #region Private Methods       
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
        }
        private void InitializeScreen()
        {

        }
        #endregion

        #region Repeater Events
        protected void rptPackingItem_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                BOMSetupItem packagingItem = (BOMSetupItem)e.Item.DataItem;

                TextBox txtMaterial = (TextBox)e.Item.FindControl("txtMaterial");
                Label lblMaterialDesc = (Label)e.Item.FindControl("lbMaterialDesc");
                TextBox txtLikeMaterial = (TextBox)e.Item.FindControl("txtLikeMaterial");
                TextBox txtOldMaterial = (TextBox)e.Item.FindControl("txtOldMaterial");
                TextBox txtSAPMatGroup = (TextBox)e.Item.FindControl("txtSAPMatGroup");
                PlaceHolder phComponentType = (PlaceHolder)e.Item.FindControl("phComponentType");
                PlaceHolder phMaterialGroup = (PlaceHolder)e.Item.FindControl("phMaterialGroup");
                Image imgStatus = (Image)e.Item.FindControl("imgStatus");
                HiddenField hdnComponentStatusChangeIds = (HiddenField)this.Parent.FindControl("hdnComponentStatusChangeIds");
                ImageButton move = (ImageButton)e.Item.FindControl("move");
                move.OnClientClick = "moveBOM('" + packagingItem.Id + "','" + packagingItem.CompassListItemId + "');return false;";
                if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BOMSetupPE.ToLower()))
                {
                    if (requirementsCheckIPF(packagingItem, TBDIndicator, LOB) && requirementsCheckPE(packagingItem))
                    {
                        imgStatus.ImageUrl = "/_layouts/15/Ferrara.Compass/img/greenCircle.png";
                        imgStatus.CssClass = "green";
                        if (hdnComponentStatusChangeIds != null)
                        {
                            hdnComponentStatusChangeIds.Value = hdnComponentStatusChangeIds.Value + packagingItem.Id + ",";
                        }
                    }
                    PlaceHolder phBOMMove = (PlaceHolder)e.Item.FindControl("phBOMMove");
                    PlaceHolder phBOMMoveHeader = (PlaceHolder)rptPackingItem.Controls[0].Controls[0].FindControl("phBOMMoveHeader");
                    phBOMMove.Visible = true;
                    phBOMMoveHeader.Visible = true;

                }
                else if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BOMSetupPE2.ToLower()) || string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BOMSetupPE3.ToLower()))
                {
                    if (requirementsCheckIPF(packagingItem, TBDIndicator, LOB) && requirementsCheckPE(packagingItem) && requirementsCheckPE2(packagingItem) && requirementsCheckPROC(packagingItem))
                    {
                        imgStatus.ImageUrl = "/_layouts/15/Ferrara.Compass/img/greenCircle.png";
                        imgStatus.CssClass = "green";
                        if (hdnComponentStatusChangeIds != null)
                            hdnComponentStatusChangeIds.Value = hdnComponentStatusChangeIds.Value + packagingItem.Id + ",";
                    }
                    PlaceHolder phBOMMove = (PlaceHolder)e.Item.FindControl("phBOMMove");
                    PlaceHolder phBOMMoveHeader = (PlaceHolder)rptPackingItem.Controls[0].Controls[0].FindControl("phBOMMoveHeader");
                    phBOMMove.Visible = true;
                    phBOMMoveHeader.Visible = true;

                    if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BOMSetupPE2.ToLower()))
                    {
                        PlaceHolder phSupplierChgHeader = (PlaceHolder)rptPackingItem.Controls[0].Controls[0].FindControl("phSupplierChgHeader");
                        PlaceHolder phSupplierChg = (PlaceHolder)e.Item.FindControl("phSupplierChg");
                        phSupplierChgHeader.Visible = true;
                        phSupplierChg.Visible = true;
                    }
                }
                else if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BOMSetupProc.ToLower()))
                {
                    if (requirementsCheckIPF(packagingItem, TBDIndicator, LOB) && requirementsCheckPE(packagingItem) && requirementsCheckPROC(packagingItem))
                    {
                        imgStatus.ImageUrl = "/_layouts/15/Ferrara.Compass/img/greenCircle.png";
                        imgStatus.CssClass = "green";
                        if (hdnComponentStatusChangeIds != null)
                            hdnComponentStatusChangeIds.Value = hdnComponentStatusChangeIds.Value + packagingItem.Id + ":" + packagingItem.PackagingComponent + ";" + packagingItem.ReviewPrinterSupplier + ",";
                    }
                    PlaceHolder phPrinterSupplier = (PlaceHolder)e.Item.FindControl("phPrinterSupplier");
                    PlaceHolder phPrinterSupplierHeader = (PlaceHolder)rptPackingItem.Controls[0].Controls[0].FindControl("phPrinterSupplierHeader");
                    phPrinterSupplier.Visible = true;
                    phPrinterSupplierHeader.Visible = true;
                }
                else if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_SAPBOMSetup.ToLower()) ||
                    string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BOMSetupSAP.ToLower()))
                {
                    if (requirementsCheckIPF(packagingItem, TBDIndicator, LOB) && requirementsCheckPE(packagingItem) && requirementsCheckPROC(packagingItem) && requirementsCheckSAPBOMSetup(packagingItem))
                    {
                        imgStatus.ImageUrl = "/_layouts/15/Ferrara.Compass/img/greenCircle.png";
                        imgStatus.CssClass = "green";
                        if (hdnComponentStatusChangeIds != null)
                            hdnComponentStatusChangeIds.Value = hdnComponentStatusChangeIds.Value + packagingItem.Id + ",";
                    }
                }
                else if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_OBMFirstReview.ToLower()))
                {
                    if (requirementsCheckIPF(packagingItem, TBDIndicator, LOB))
                    {
                        imgStatus.ImageUrl = "/_layouts/15/Ferrara.Compass/img/greenCircle.png";
                        imgStatus.CssClass = "green";
                        if (hdnComponentStatusChangeIds != null)
                            hdnComponentStatusChangeIds.Value = hdnComponentStatusChangeIds.Value + packagingItem.Id + ",";
                    }
                    PlaceHolder phBOMMove = (PlaceHolder)e.Item.FindControl("phBOMMove");
                    PlaceHolder phBOMMoveHeader = (PlaceHolder)rptPackingItem.Controls[0].Controls[0].FindControl("phBOMMoveHeader");
                    phBOMMove.Visible = true;
                    phBOMMoveHeader.Visible = true;
                }
                else if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BOMSetupMaterialWarehouse.ToLower()))
                {
                    if (requirementsCheckIPF(packagingItem, TBDIndicator, LOB) && requirementsCheckPE(packagingItem) && requirementsCheckMWH(packagingItem))
                    {
                        imgStatus.ImageUrl = "/_layouts/15/Ferrara.Compass/img/greenCircle.png";
                        imgStatus.CssClass = "green";
                        if (hdnComponentStatusChangeIds != null)
                            hdnComponentStatusChangeIds.Value = hdnComponentStatusChangeIds.Value + packagingItem.Id + ",";
                    }

                    PlaceHolder phBOMMove = (PlaceHolder)e.Item.FindControl("phBOMMove");
                    PlaceHolder phBOMMoveHeader = (PlaceHolder)rptPackingItem.Controls[0].Controls[0].FindControl("phBOMMoveHeader");
                    phBOMMove.Visible = true;
                    phBOMMoveHeader.Visible = true;
                }
                else
                {
                    if (requirementsCheckIPF(packagingItem, TBDIndicator, LOB))
                    {
                        imgStatus.ImageUrl = "/_layouts/15/Ferrara.Compass/img/greenCircle.png";
                        imgStatus.CssClass = "green";
                        if (hdnComponentStatusChangeIds != null)
                            hdnComponentStatusChangeIds.Value = hdnComponentStatusChangeIds.Value + packagingItem.Id + ",";
                    }
                }

                DropDownList drpUnitOfMeasure = ((DropDownList)e.Item.FindControl("drpUnitOfMeasure"));
                Utilities.BindDropDownItems(drpUnitOfMeasure, GlobalConstants.LIST_PackUnitLookup, webUrl);
                if (packagingItem.PackUnit != null && packagingItem.PackUnit != string.Empty)
                    Utilities.SetDropDownValue(packagingItem.PackUnit, drpUnitOfMeasure, this.Page);
                DropDownList drpNew = ((DropDownList)e.Item.FindControl("drpNew"));
                if (packagingItem.NewExisting != null && packagingItem.NewExisting != string.Empty)
                {
                    if (packagingItem.NewExisting.ToLower() == "new")
                    {
                        txtMaterial.Text = string.IsNullOrEmpty(packagingItem.MaterialNumber) ? "Needs New" : packagingItem.MaterialNumber;
                        lblMaterialDesc.Text = string.IsNullOrEmpty(packagingItem.MaterialDescription) ? "Needs New" : packagingItem.MaterialDescription;
                        lblMaterialDesc.ToolTip = string.IsNullOrEmpty(packagingItem.MaterialDescription) ? "Needs New" : packagingItem.MaterialDescription;
                        //txtMaterial.CssClass = "numericNoMask form-control Component PCBOMrequired";
                    }
                }
                txtLikeMaterial.Text = string.IsNullOrEmpty(packagingItem.CurrentLikeItem) ? "" : packagingItem.CurrentLikeItem;
                txtOldMaterial.Text = string.IsNullOrEmpty(packagingItem.CurrentOldItem) ? "" : packagingItem.CurrentOldItem;

                if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_SAPBOMSetup.ToLower()) ||
                    string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BOMSetupSAP.ToLower()) ||
                    string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_SAPBOMSetup.ToLower()))
                {
                    if (string.IsNullOrEmpty(packagingItem.PackagingComponent))
                    {
                        txtSAPMatGroup.Text = "";
                    }
                    else
                    {
                        string componantType = Utilities.GetLookupDetailsByValueAndColumn("Value", GlobalConstants.LIST_PackagingComponentTypesLookup, "Title", packagingItem.PackagingComponent, webUrl);
                        txtSAPMatGroup.Text = componantType;
                    }
                    phComponentType.Visible = false;
                    phMaterialGroup.Visible = true;
                    PlaceHolder phComponentTypeHeader = (PlaceHolder)rptPackingItem.Controls[0].Controls[0].FindControl("phComponentTypeHeader");
                    phComponentTypeHeader.Visible = false;
                    PlaceHolder phMaterialGroupHeader = (PlaceHolder)rptPackingItem.Controls[0].Controls[0].FindControl("phMaterialGroupHeader");
                    phMaterialGroupHeader.Visible = true;
                }

                if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_QA.ToLower()))
                {
                    PlaceHolder phCnsmrLbl = (PlaceHolder)e.Item.FindControl("phCnsmrLbl");
                    PlaceHolder phCnsmrLblHeader = (PlaceHolder)rptPackingItem.Controls[0].Controls[0].FindControl("phCnsmrLblHeader");
                    phCnsmrLbl.Visible = true;
                    phCnsmrLblHeader.Visible = true;
                }
            }
        }
        protected void rptPackingItem_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            HiddenField hdnPackagingComponent = (HiddenField)e.Item.FindControl("hdnPackagingComponent");
            HiddenField hdnParID = (HiddenField)e.Item.FindControl("hdnParentID");

            if (e.CommandName.ToLower() == "loadcontrol")
            {

                string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
                string id = commandArgs[0];
                string packComp = hdnPackagingComponent.Value;
                var ctrl = (ucBOMEditable_New)Page.LoadControl(@"~/_controltemplates/15/Ferrara.Compass/ucBOMEditable_New.ascx");
                ctrl.PackagingItemId = Convert.ToInt32(id);
                ctrl.ParentID = hdnParID.Value.ToString();
                ctrl.ParentType = hdnParentComponentType.Value;
                ctrl.ProjectType = ProjectType;
                ctrl.ProjectTypeSubCategory = ProjectTypeSubCategory;
                ctrl.AllPIs = packagingItems;
                ctrl.IsNew = false;
                ctrl.firstLoad = "true";
                ctrl.CompassListItemId = iItemId;
                var hdnPackagingID = (HiddenField)this.Parent.FindControl("hdnPackagingID");
                var hdnParentID = (HiddenField)this.Parent.FindControl("hdnParentID");
                var hdnPageState = (HiddenField)this.Parent.FindControl("hdnPageState");

                hdnPageState.Value = "PE";
                hdnParentID.Value = hdnParID.Value.ToString();
                hdnPackagingID.Value = id;

                HideOtherContentsFromParentPage();
                // Add messages to page

                try
                {
                    openBtnSave();
                }
                catch (Exception error)
                {

                }
                var phBOMEdits = (PlaceHolder)this.Parent.FindControl("phBOMEdits");
                if (phBOMEdits == null)
                {
                    phBOMEdits = (PlaceHolder)this.Parent.FindControl("phMsg");
                }
                phBOMEdits.Controls.Clear();
                phBOMEdits.Controls.Add(ctrl);
            }
            else if (e.CommandName.ToLower() == "movebom")
            {

            }
        }

        private void HideOtherContentsFromParentPage()
        {
            var phPage = (PlaceHolder)this.Parent.FindControl("phPage");
            if (phPage != null)
            {
                phPage.Visible = false;
            }
            var pcProjectInformation = (PlaceHolder)this.Parent.FindControl("pcProjectInformation");
            if (pcProjectInformation != null)
            {
                pcProjectInformation.Visible = false;
            }

            if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_SAPBOMSetup.ToLower()) ||
                string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_SAPCompleteItemSetup.ToLower()))
            {
                var phTop = (PlaceHolder)this.Parent.FindControl("phTopBOMSETUP");
                phTop.Visible = false;

                var phBottom = (PlaceHolder)this.Parent.FindControl("phBottomBOMSETUP");
                phBottom.Visible = false;
            }
        }
        #endregion

        #region Data Transfer Methods
        private void LoadBOMItems()
        {
            lblTitle.Text = GridItem.PackagingComponent;
            lblDesc.Text = ": " + GridItem.MaterialNumber.ToUpper() + ": " + GridItem.MaterialDescription.ToUpper();

            if (string.Equals(GridItem.PackagingComponent, "Finished Good"))
            {
                dvAddFinishGoodItem.Visible = true;
                dvAddTransferSemi.Visible = false;
                hdnParentComponentType.Value = "Finished Good";
            }
            else
            {
                dvAddFinishGoodItem.Visible = false;
                if (!string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_QA.ToLower()))
                {
                    dvAddTransferSemi.Visible = true;
                    btnAddTransferSemi.Text = "Add " + GridItem.PackagingComponent + " Component";
                    //btnAddTransferSemi.CommandArgument = ParentId.ToString() + "," + PackagingComponent;
                }
                hdnParentComponentType.Value = GridItem.PackagingComponent;
            }
            //hdnParentPackagingType.Value = 
            if (packagingItems.Count > 0)
            {
                dvUCBomPE.Visible = true;
                rptPackingItem.DataSource = (from children in packagingItems where children.ParentID == ParentId select children).ToList();
                rptPackingItem.DataBind();
            }
            else
            {
                dvUCBomPE.Visible = true;
                Panel emptyList = new Panel();
                emptyList.Controls.Add(new LiteralControl("No Components Found"));
                noResultsHolder.Controls.Add(emptyList);
            }
        }
        private void LoadFormData()
        {
            BOMSetupItem bomsetupitem = BOMSetupService.GetPackMeasurementsItem(iItemId, ParentId);
            if (PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi || PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
            {
                lblTransferLocation.Text = MakePackTransferLocation;
            }
            txtPalletSpecNumber.Text = bomsetupitem.PalletSpecNumber;
            txPalletPatternLink.Text = bomsetupitem.PalletSpecLink;
            if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BOMSetupMaterialWarehouse.ToLower()) ||
                string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_SAPBOMSetup.ToLower()) ||
                string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BOMSetupSAP.ToLower()) ||
                string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_SAPCompleteItemSetup.ToLower())
                )
            {
                txPalletPatternLink.ReadOnly = true;
                divGenerateLink.Visible = false;
            }
            if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BOMSetupPE.ToLower()) || string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BOMSetupProc.ToLower()))
            {
                specsSection.Visible = false;
            }
            txtFGPackSpecNumber.Text = bomsetupitem.PackSpecNumber;
            txtSpecNotes.Text = bomsetupitem.NotesSpec;
            if (bomsetupitem.PalletSpecLink != "")
            {
                generatedLink.NavigateUrl = bomsetupitem.PalletSpecLink;
                string title = string.IsNullOrEmpty(PackagingComponent) ? "Finished Good" : PackagingComponent;
                title = title + ": " + MaterialNumber + ": Pallet Pattern";
                generatedLink.Text = title;
                generatedLink.CssClass = "";
            }
            Utilities.SetDropDownValue(bomsetupitem.SAPSpecsChange, drpSAPSpecsChange, this.Page);
            hdnParentComponentId.Value = Convert.ToString(ParentId);
            hdnNewExistingComp.Text = NewExisting;
        }
        private void LoadBOMItemsForPackMeas()
        {
            MaterialNumber = string.IsNullOrEmpty(MaterialNumber) ? "Needs New" : MaterialNumber;
            MaterialDesc = string.IsNullOrEmpty(MaterialDesc) ? "Needs New" : MaterialDesc;
            if (NewComponentCount > 0)
            {
                hdnNewComponentExists.Text = "true";
            }
            else
            {
                hdnNewComponentExists.Text = "false";
            }
            string labelText = "Finished Good";
            if (ParentId != 0)
            {
                if (PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi)
                {
                    hdnUCSEMiBOMComponentType.Value = "Transfer";
                    hdnIsTransferSemi.Text = "ts";
                }
                else if (PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                {
                    hdnUCSEMiBOMComponentType.Value = "Purchased Candy";
                    hdnIsTransferSemi.Text = "pcs";
                }
                labelText = PackagingComponent;
                lblTransferLocation.Text = SemiXferMakeLocation;
                dvTransferLocation.Visible = true;
                hdnid.Value = ParentId.ToString();
            }
            else
            {
                hdnUCBOMComponentType.Value = "BOM";
                hdnid.Value = "0";
            }
            lblPalletSpecNumber.Text = labelText;
            lblFGPackSpecNumber.Text = labelText;
            lblSAPSpecChange.Text = labelText;
            lblSAPSpecChange2.Text = labelText;
            lblSAPSpecChangeHeader.Text = labelText;
            lblPalletPatternLink.Text = labelText;
            dvElements.Visible = true;
        }
        #endregion
        #region Button Methods
        protected void btnAddNewPackagingItem_Click(object sender, EventArgs e)
        {
            try
            {
                openBtnSave();
            }
            catch (Exception error)
            {

            }
            var obj = new BOMSetupItem();
            obj.ParentID = 0;
            obj.ProjectNumber = ProjectNumber;
            obj.CompassListItemId = iItemId;
            int id = BOMSetupService.InsertBOMSetupItem(obj);

            var hdnPackagingID = (HiddenField)this.Parent.FindControl("hdnPackagingID");
            var hdnParentID = (HiddenField)this.Parent.FindControl("hdnParentID");
            var hdnPageState = (HiddenField)this.Parent.FindControl("hdnPageState");

            var ctrl = (ucBOMEditable_New)Page.LoadControl(@"~/_controltemplates/15/Ferrara.Compass/ucBOMEditable_New.ascx");
            ctrl.PackagingItemId = id;
            ctrl.ParentID = "0";
            ctrl.IsNew = true;
            ctrl.firstLoad = "true";
            ctrl.AllPIs = packagingItems;
            ctrl.ProjectType = ProjectType;
            ctrl.ProjectTypeSubCategory = ProjectTypeSubCategory;
            ctrl.CompassListItemId = iItemId;
            hdnPageState.Value = "PE";
            hdnParentID.Value = "0";
            hdnPackagingID.Value = id.ToString();

            HideOtherContentsFromParentPage();
            // Add messages to page
            // openBtnSave();
            var phBOMEdits = (PlaceHolder)this.Parent.FindControl("phBOMEdits");
            if (phBOMEdits == null)
            {
                phBOMEdits = (PlaceHolder)this.Parent.FindControl("phMsg");
            }
            phBOMEdits.Controls.Clear();
            phBOMEdits.Controls.Add(ctrl);
        }
        protected void btnAddTransferSemi_Click(object sender, EventArgs e)
        {
            try
            {
                openBtnSave();
            }
            catch (Exception error)
            {

            }
            var obj = new BOMSetupItem();
            obj.ParentID = ParentId;
            obj.ProjectNumber = ProjectNumber;
            obj.CompassListItemId = iItemId;
            int id = BOMSetupService.InsertBOMSetupItem(obj);

            var ctrl = (ucBOMEditable_New)Page.LoadControl(@"~/_controltemplates/15/Ferrara.Compass/ucBOMEditable_New.ascx");
            ctrl.PackagingItemId = id;
            ctrl.ParentID = ParentId.ToString();
            ctrl.ParentType = hdnParentComponentType.Value;
            ctrl.PackagingComponent = ParentComponentType;
            ctrl.ProjectType = ProjectType;
            ctrl.ProjectTypeSubCategory = ProjectTypeSubCategory;
            ctrl.firstLoad = "true";
            ctrl.IsNew = true;
            ctrl.CompassListItemId = iItemId;
            var hdnPackagingID = (HiddenField)this.Parent.FindControl("hdnPackagingID");
            var hdnParentID = (HiddenField)this.Parent.FindControl("hdnParentID");
            var hdnPageState = (HiddenField)this.Parent.FindControl("hdnPageState");

            hdnPageState.Value = "PE";
            hdnParentID.Value = ParentId.ToString();
            hdnPackagingID.Value = id.ToString();

            //openBtnSave();
            // Add messages to page;
            /*var phPage = (PlaceHolder)this.Parent.FindControl("phPage");
            phPage.Visible = false;*/
            var phBOMEdits = (PlaceHolder)this.Parent.FindControl("phBOMEdits");
            if (phBOMEdits == null)
            {
                phBOMEdits = (PlaceHolder)this.Parent.FindControl("phMsg");
            }
            phBOMEdits.Controls.Clear();
            phBOMEdits.Controls.Add(ctrl);

        }
        #endregion
        #region requirementsChecks
        private bool requirementsCheckPE2(BOMSetupItem item)
        {
            bool completed = true;
            if (!string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_TransferSemi) && !string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_CandySemi) && !string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_PurchasedSemi) && !item.PackagingComponent.Contains("Finished Good"))
            {
                if (string.IsNullOrEmpty(item.SpecificationNo))
                {
                    completed = false;
                }
                else if (string.IsNullOrEmpty(item.DielineURL) && item.NewExisting == "New")
                {
                    completed = false;
                }
            }
            if (item.PackagingComponent.Contains("Film"))
            {
                if ((string.IsNullOrEmpty(item.FilmPrintStyle) || string.Equals(item.FilmPrintStyle, "Select...")) && item.GraphicsChangeRequired == "Yes")
                {
                    completed = false;
                }
            }
            else if (item.PackagingComponent.Contains("Corrugate"))
            {
                if ((string.IsNullOrEmpty(item.CorrugatedPrintStyle) || string.Equals(item.CorrugatedPrintStyle, "Select...")) && item.GraphicsChangeRequired == "Yes")
                {
                    completed = false;
                }
            }

            return completed;
        }
        private bool requirementsCheckIPF(BOMSetupItem item, string TBDIndicator, string LOB)
        {
            bool completed = true;
            //Rules for all components
            if (item.NewExisting == "Select..." || item.NewExisting == "")
            {
                completed = false;
            }
            else if (item.PackagingComponent == "Select..." || item.PackagingComponent == "")
            {
                completed = false;
            }
            else if (item.PackUnit == "Select..." || item.PackUnit == "")
            {
                completed = false;
            }
            else if (item.PackQuantity == "Select..." || item.PackQuantity == "")
            {
                completed = false;
            }
            else if (item.NewExisting == "New" && item.CurrentLikeItem == "")
            {
                completed = false;
            }
            else if (item.NewExisting == "Existing" && item.MaterialNumber == "")
            {
                completed = false;
            }
            else if (item.Flowthrough == "" || item.Flowthrough == "Select...")
            {
                completed = false;
            }
            else if (item.NewExisting == "New" && item.CurrentLikeItemDescription == "")
            {
                completed = false;
            }
            else if (item.NewExisting == "Existing" && item.MaterialDescription == "")
            {
                completed = false;
            }
            else if (item.PackagingComponent != GlobalConstants.COMPONENTTYPE_CandySemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi && !item.PackagingComponent.Contains("Finished Good"))
            {
                if (item.NewExisting == "New" && item.CurrentLikeItemReason == "")
                {
                    completed = false;
                }
                else if (item.GraphicsChangeRequired == "Select..." || item.ComponentContainsNLEA == "Select...")
                {
                    completed = false;
                }
                else if (item.GraphicsChangeRequired == "" || item.ComponentContainsNLEA == "")
                {
                    completed = false;
                }
                else if (item.GraphicsChangeRequired == "Yes" && (item.ExternalGraphicsVendor == "Select..." || item.ExternalGraphicsVendor == ""))
                {
                    completed = false;
                }
                else if (item.GraphicsChangeRequired == "Yes" && item.GraphicsBrief == "")
                {
                    completed = false;
                }
            }
            else if (item.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi || item.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
            {
                if (item.NewExisting == "New")
                {
                    if (string.IsNullOrEmpty(item.PHL1) || item.PHL1 == "Select...")
                    {
                        completed = false;
                    }
                    else if (string.IsNullOrEmpty(item.PHL2) || item.PHL2 == "Select...")
                    {
                        completed = false;
                    }
                    else if (string.IsNullOrEmpty(item.Brand) || item.Brand == "Select...")
                    {
                        completed = false;
                    }
                    else if (string.IsNullOrEmpty(item.ProfitCenter))
                    {
                        completed = false;
                    }
                }
            }
            return completed;
        }
        private bool requirementsCheckPE(BOMSetupItem item)
        {
            bool completed = true;
            //Rules for all components
            if (item.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi || item.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
            {
                if (item.NewExisting == "New")
                {
                    if (string.IsNullOrEmpty(item.CountryOfOrigin) || item.CountryOfOrigin == "Select...")
                    {
                        completed = false;
                    }
                    else if (string.IsNullOrEmpty(item.PackLocation) || item.PackLocation == "Select...")
                    {
                        completed = false;
                    }
                    else if (string.IsNullOrEmpty(item.TransferSEMIMakePackLocations) || item.TransferSEMIMakePackLocations == "Select...")
                    {
                        completed = false;
                    }
                }
            }
            return completed;
        }
        private bool requirementsCheckMWH(BOMSetupItem item)
        {
            bool completed = true;
            //Rules for all components
            if (item.NewExisting == "Select..." || item.NewExisting == "")
            {
                completed = false;
            }
            else if (item.PackagingComponent == "Select..." || item.PackagingComponent == "")
            {
                completed = false;
            }
            else if (item.PackagingComponent != GlobalConstants.COMPONENTTYPE_CandySemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi && !item.PackagingComponent.Contains("Finished"))
            {
                var compassItem = BOMSetupService.GetProjectSummaryDetails(iItemId);
                if ((compassItem.ProjectSubCategory == "Complex Network Move" || compassItem.ProjectType == "Simple Network Move") && (item.NewExisting == "New" || item.NewExisting == "Existing"))
                {
                    if ((item.PurchasedIntoLocation == "Select..." || item.PurchasedIntoLocation == ""))
                    {
                        completed = false;
                    }
                }
                else
                {
                    if (item.NewExisting == "New" && (item.PurchasedIntoLocation == "Select..." || item.PurchasedIntoLocation == ""))
                    {
                        completed = false;
                    }
                }
            }
            else if (item.PackUnit == "Select..." || item.PackUnit == "")
            {
                completed = false;
            }
            else if (item.PackQuantity == "Select..." || item.PackQuantity == "")
            {
                completed = false;
            }
            else if (item.NewExisting == "New" && item.CurrentLikeItem == "")
            {
                completed = false;
            }
            else if (item.NewExisting == "Existing" && item.MaterialNumber == "")
            {
                completed = false;
            }
            else if (item.NewExisting == "New" && item.CurrentLikeItemDescription == "")
            {
                completed = false;
            }
            else if (item.NewExisting == "Existing" && item.MaterialDescription == "")
            {
                completed = false;
            }
            else if ((
                item.PackagingComponent != GlobalConstants.COMPONENTTYPE_CandySemi &&
                item.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi &&
                item.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi &&
                (item.NewExisting == "New" && item.CurrentLikeItemReason == "")))
            {
                completed = false;
            }
            else if ((
                item.PackagingComponent != GlobalConstants.COMPONENTTYPE_CandySemi &&
                item.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi &&
                item.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi &&
                !item.PackagingComponent.Contains("Finished")) &&
                (item.GraphicsChangeRequired == "Select..." || item.ComponentContainsNLEA == "Select..."))
            {
                completed = false;
            }
            else if ((
                item.PackagingComponent != GlobalConstants.COMPONENTTYPE_CandySemi &&
                item.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi &&
                item.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi &&
                !item.PackagingComponent.Contains("Finished")) &&
                (item.GraphicsChangeRequired == "Yes" && item.ExternalGraphicsVendor == "Select..."))
            {
                completed = false;
            }
            else if ((
                item.PackagingComponent != GlobalConstants.COMPONENTTYPE_CandySemi &&
                item.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi &&
                item.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi &&
                !item.PackagingComponent.Contains("Finished")) &&
                (item.GraphicsChangeRequired == "Yes" && item.GraphicsBrief == ""))
            {
                completed = false;
            }
            return completed;
        }
        private bool requirementsCheckPROC(BOMSetupItem item)
        {
            bool completed = true;
            //Rules for all components
            if (!string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_CandySemi) && !string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_TransferSemi) && !string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_PurchasedSemi) && !item.PackagingComponent.Contains("Finished Good"))
            {
                if (!(string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BOMSetupPE2.ToLower()) && ProjectType == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly))
                {
                    if (item.NewExisting == "New" && (item.PrinterSupplier == "Select..." || item.PrinterSupplier == ""))
                    {
                        completed = false;
                    }
                }
            }

            if (ProjectType == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly && (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BOMSetupProc.ToLower())))
            {
                if (!string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_CandySemi) && !string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_TransferSemi) && !string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_PurchasedSemi) && !item.PackagingComponent.Contains("Finished Good"))
                {
                    if (string.Equals(item.NewExisting, "New"))
                    {
                        if (string.IsNullOrEmpty(item.IsAllProcInfoCorrect) || string.Equals(item.IsAllProcInfoCorrect, "Select..."))
                        {
                            completed = false;
                        }
                        else if (string.Equals(item.IsAllProcInfoCorrect, "No") && string.IsNullOrEmpty(item.WhatProcInfoHasChanged))
                        {
                            completed = false;
                        }
                    }
                }
            }

            if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BOMSetupProc.ToLower()) && ProjectTypeSubCategory == GlobalConstants.PROJECTTYPESUBCATEGORY_ComplexNetworkMove && item.PackagingComponent.Contains("Corrugated") && string.Equals(item.NewExisting, "Existing"))
            {
                if (item.NewPrinterSupplierForLocation == "Select..." || item.NewPrinterSupplierForLocation == "")
                {
                    completed = false;
                }
            }

            return completed;
        }
        private bool requirementsCheckSAPBOMSetup(BOMSetupItem item)
        {
            bool completed = true;
            //Rules for all components

            if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_SAPBOMSetup.ToLower()))
            {
                if (string.IsNullOrEmpty(item.FourteenDigitBarcode) && TransferSemiBarcodeGenerationVisibility(item))
                {
                    completed = false;
                }
            }

            return completed;
        }
        private bool TransferSemiBarcodeGenerationVisibility(BOMSetupItem packagingItem)
        {
            bool visible = false;
            if (packagingItem.PackagingComponent.ToLower().Contains("corrugated") && packagingItem.NewExisting.ToLower() == "new")
            {
                int parentCompId = packagingItem.ParentID;
                if (parentCompId == 0) return visible;
                var parentItem = packagingItemService.GetPackagingItemByPackagingId(parentCompId);
                if (parentItem != null)
                {
                    if (parentItem.PackagingComponent.ToLower().Contains("transfer") && (parentItem.PackLocation.Contains("FQ22") || parentItem.PackLocation.Contains("FQ25")))
                    {
                        visible = true;
                    }
                }
            }
            return visible;
        }
        #endregion
        //public void saveData()
        //{
        //    var phBOMGrid = (PlaceHolder)this.Parent.FindControl("phBOMGrid");
        //    if (phBOMGrid == null)
        //    {
        //        phBOMGrid = (PlaceHolder)this.Parent.FindControl("phBOM");
        //    }
        //    foreach (UserControl ctrl in phBOMGrid.Controls)
        //    {
        //        if (ctrl is ucBOMGridPackMeas_New)
        //        {
        //            var type = (ucBOMGridPackMeas_New)ctrl;
        //            Repeater gridItems = type.rptPackingItem;
        //            foreach (RepeaterItem item in gridItems.Items)
        //            {
        //                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
        //                {
        //                    string deletedStatus = ((HiddenField)item.FindControl("hdnDeletedStatus")).Value;
        //                    if (deletedStatus == "true")
        //                    {
        //                        string deleteId = ((HiddenField)item.FindControl("hdnItemID")).Value;
        //                        BOMSetupService.DeleteBOMSetupItem(Convert.ToInt32(deleteId));
        //                    }

        //                }
        //            }
        //        }
        //        else if (ctrl is ucBOMGridPackMeas_New)
        //        {
        //            var type = (ucBOMGridPackMeas_New)ctrl;
        //            var item = ConstructFormDataForPackMeas(type);
        //            if (type.iItemId > 0)
        //            {
        //                int ctrlParentId = Convert.ToInt32(type.hdnParentComponentId.Value);
        //                if (ctrlParentId > 0)
        //                {
        //                    BOMSetupService.UpdateTransferSemiMakePackLocations(ctrlParentId, type.lblTransferLocation.Text);
        //                }
        //                BOMSetupService.UpsertPackMeasurementsItem(item, ProjectNumber);
        //            }
        //        }
        //    }
        //}
        //public void saveDataPackMeas()
        //{
        //    var phBOMGrid = (PlaceHolder)this.Parent.FindControl("phBOMGrid");
        //    foreach (UserControl ctrl in phBOMGrid.Controls)
        //    {
        //        if (ctrl is ucBOMGridPackMeas_New)
        //        {
        //            var type = (ucBOMGridPackMeas_New)ctrl;
        //            var item = ConstructFormDataForPackMeas(type);
        //            if (type.iItemId > 0)
        //            {
        //                int ctrlParentId = Convert.ToInt32(type.hdnParentComponentId.Value);
        //                if (ctrlParentId > 0)
        //                {
        //                    BOMSetupService.UpdateTransferSemiMakePackLocations(ctrlParentId, type.lblTransferLocation.Text);
        //                }
        //                BOMSetupService.UpsertPackMeasurementsItem(item, ProjectNumber);
        //            }
        //        }
        //    }
        //}
        //private BOMSetupItem ConstructFormDataForPackMeas(ucBOMGridPackMeas_New ctrl)
        //{
        //    var bomsetupitem = new BOMSetupItem();
        //    try
        //    {

        //        bomsetupitem.CompassListItemId = iItemId;
        //        bomsetupitem.ParentID = Convert.ToInt32(ctrl.hdnParentComponentId.Value);
        //        bomsetupitem.PalletSpecNumber = ctrl.txtPalletSpecNumber.Text;
        //        bomsetupitem.PackSpecNumber = ctrl.txtFGPackSpecNumber.Text;
        //        bomsetupitem.PalletSpecLink = ctrl.txPalletPatternLink.Text;
        //        bomsetupitem.NotesSpec = ctrl.txtSpecNotes.Text;
        //        bomsetupitem.SAPSpecsChange = ctrl.drpSAPSpecsChange.SelectedItem.Text;
        //    }
        //    catch (Exception exception)
        //    {
        //        LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Packaging Component: " + exception.Message);
        //        exceptionService.Handle(LogCategory.CriticalError, exception, "PackagingComponentEntryForm", "ConstructFormData");
        //    }
        //    return bomsetupitem;
        //}

    }
}
