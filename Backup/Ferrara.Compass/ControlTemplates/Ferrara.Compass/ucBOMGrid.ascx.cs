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

namespace Ferrara.Compass.ControlTemplates.Ferrara.Compass
{
    public partial class ucBOMGrid : UserControl
    {
        #region Member Variables
        private IItemProposalService itemProposalService;
        private IPackagingItemService packagingItemService;
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private INotificationService notificationService;
        private IBillOfMaterialsService billOfMaterialsService;
        private ISAPMaterialMasterService sapMMService;
        private IShipperFinishedGoodService shipperFinishedGoodService;
        private IMixesService mixesService;
        private string webUrl;
        private int iItemId = 0;
        private List<PackagingItem> packagingItems = new List<PackagingItem>();
        private List<ShipperFinishedGoodItem> shipperFGItems = new List<ShipperFinishedGoodItem>();
        private const string _ascxPath = @"~/_controltemplates/15/Ferrara.Compass/ucPackagingEngineering.ascx";
        PlaceHolder phUC;
        #endregion

        #region Properties
        public int PackagingItemId { get; set; }
        public int ParentId { get; set; }
        public int CompassItemId { get; set; }
        public string PackagingComponent { get; set; }
        public string CompassListId { get; set; }
        public string FilmSubstrate { get; set; }
        public bool IsNew { get; set; }
        public string MaterialNumber { get; set; }
        public string MaterialDesc { get; set; }
        public string SemiXferMakeLocation { get; set; }
        public bool isChildItem { get; set; }
        public CallParentMethod openBtnSave { get; set; }
        public delegate void CallParentMethod();

        private string ProjectNumber
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo];
                return string.Empty;
            }
        }

        #endregion
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeScreen();
            // Page.EnableEventValidation = false;

            packagingItemService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            sapMMService = DependencyResolution.DependencyMapper.Container.Resolve<ISAPMaterialMasterService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
            billOfMaterialsService = DependencyResolution.DependencyMapper.Container.Resolve<IBillOfMaterialsService>();
            shipperFinishedGoodService = DependencyResolution.DependencyMapper.Container.Resolve<IShipperFinishedGoodService>();
            mixesService = DependencyResolution.DependencyMapper.Container.Resolve<IMixesService>();
            itemProposalService = DependencyResolution.DependencyMapper.Container.Resolve<IItemProposalService>();
            EnsureScriptManager();
            EnsureUpdatePanelFixups();
        }
        protected void Page_Load(object sender, EventArgs e)
        {


            webUrl = SPContext.Current.Web.Url;
            iItemId = Utilities.GetItemIdFromProjectNumber(ProjectNumber);
            phUC = (PlaceHolder)this.Parent.FindControl("phMsg");
            saveData();
            LoadBOMItems();


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
                PackagingItem packagingItem = (PackagingItem)e.Item.DataItem;

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
                if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_PE.ToLower()))
                {
                    if (requirementsCheckPE(packagingItem))
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
                else if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_PE2.ToLower()))
                {
                    if (requirementsCheckPE2(packagingItem))
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
                else if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_Proc.ToLower()))
                {
                    if (requirementsCheckPROC(packagingItem))
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
                else if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BOMSetupSAP.ToLower()))
                {
                    if (requirementsCheckPROC(packagingItem))
                    {
                        imgStatus.ImageUrl = "/_layouts/15/Ferrara.Compass/img/greenCircle.png";
                        imgStatus.CssClass = "green";
                        if (hdnComponentStatusChangeIds != null)
                            hdnComponentStatusChangeIds.Value = hdnComponentStatusChangeIds.Value + packagingItem.Id + ",";
                    }
                    /*PlaceHolder statusImage = (PlaceHolder)e.Item.FindControl("statusImage");
                    statusImage.Visible = false;
                    rptPackingItem.Controls[0].Controls[0].FindControl("statusHeader").Visible = false;*/
                }
                else if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_OBMFirstReview.ToLower()))
                {
                    if (requirementsCheckPMRev1(packagingItem))
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
                    if (requirementsCheckPE(packagingItem))
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

                if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_SAPBOMSetup.ToLower()))
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
                var ctrl = (ucBOMEditable)Page.LoadControl(@"~/_controltemplates/15/Ferrara.Compass/ucBOMEditable.ascx");
                ctrl.PackagingComponent = hdnPackagingComponent.Value;
                ctrl.PackagingItemId = Convert.ToInt32(id);
                ctrl.ParentID = hdnParID.Value.ToString();
                ctrl.PackagingComponent = packComp;
                ctrl.IsNew = false;
                var hdnPackagingID = (HiddenField)this.Parent.FindControl("hdnPackagingID");
                var hdnParentID = (HiddenField)this.Parent.FindControl("hdnParentID");
                var hdnPageState = (HiddenField)this.Parent.FindControl("hdnPageState");

                hdnPageState.Value = "PE";
                hdnParentID.Value = hdnParID.Value.ToString();
                hdnPackagingID.Value = id;

                var phPage = (PlaceHolder)this.Parent.FindControl("phPage");
                phPage.Visible = false;
                // Add messages to page

                try
                {
                    openBtnSave();
                }
                catch (Exception error)
                {

                }
                phUC.Controls.Clear();
                phUC.Controls.Add(ctrl);
            }
            else if (e.CommandName.ToLower() == "movebom")
            {

            }
        }
        #endregion

        #region Data Transfer Methods
        private void LoadBOMItems()
        {
            List<PackagingItem> dtPackingItem = new List<PackagingItem>();
            MaterialNumber = string.IsNullOrEmpty(MaterialNumber) ? "Needs New" : MaterialNumber;
            MaterialDesc = string.IsNullOrEmpty(MaterialDesc) ? "Needs New" : MaterialDesc;

            if (ParentId == 0)
            {
                dtPackingItem = packagingItemService.GetFinishedGoodItemsForProject(iItemId);
                lblTitle.Text = "Finished Good Summary";
                lblDesc.Text = ": " + MaterialNumber + " : " + MaterialDesc;
                dvAddFinishGoodItem.Visible = true;
                dvAddTransferSemi.Visible = false;

            }
            else
            {
                dtPackingItem = packagingItemService.GetSemiBOMItems(iItemId, ParentId);
                lblTitle.Text = PackagingComponent;

                lblDesc.Text = ": " + MaterialNumber + " : " + MaterialDesc;
                hdnParentComponentType.Value = isChildItem.ToString();

                dvAddFinishGoodItem.Visible = false;
                if (!string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_QA.ToLower()))
                {
                    dvAddTransferSemi.Visible = true;
                    btnAddTransferSemi.Text = "Add " + PackagingComponent + " Component";
                    btnAddTransferSemi.CommandArgument = ParentId.ToString() + "," + PackagingComponent;
                }
            }

            if (dtPackingItem.Count > 0)
            {
                dvUCBomPE.Visible = true;
                rptPackingItem.DataSource = dtPackingItem;
                rptPackingItem.DataBind();
            }
            else
            {
                dvUCBomPE.Visible = true;
                Panel emptyList = new Panel();
                emptyList.Controls.Add(new LiteralControl("No Components Found"));
                noResultsHolder.Controls.Add(emptyList);
            }

            HiddenField hdnPackagingNumbers = (HiddenField)this.Parent.FindControl("hdnPackagingNumbers");
            hdnPackagingNumbers.Value = "";

        }

        #endregion

        #region Button Methods
        protected void btnAddNewPackagingItem_Click(object sender, EventArgs e)
        {
            var obj = new PackagingItem();
            obj.ParentID = 0;
            int id = packagingItemService.InsertPackagingItem(obj, iItemId);

            var hdnPackagingID = (HiddenField)this.Parent.FindControl("hdnPackagingID");
            var hdnParentID = (HiddenField)this.Parent.FindControl("hdnParentID");
            var hdnPageState = (HiddenField)this.Parent.FindControl("hdnPageState");

            var ctrl = (ucBOMEditable)Page.LoadControl(@"~/_controltemplates/15/Ferrara.Compass/ucBOMEditable.ascx");
            ctrl.PackagingItemId = id;
            ctrl.ParentID = "0";
            ctrl.IsNew = true;

            hdnPageState.Value = "PE";
            hdnParentID.Value = "0";
            hdnPackagingID.Value = id.ToString();

            var phPage = (PlaceHolder)this.Parent.FindControl("phPage");
            phPage.Visible = false;
            // Add messages to page
            openBtnSave();
            phUC.Controls.Clear();
            phUC.Controls.Add(ctrl);
        }
        protected void btnAddTransferSemi_Click(object sender, EventArgs e)
        {
            string[] commandArgs = btnAddTransferSemi.CommandArgument.ToString().Split(new char[] { ',' });
            string parentId = commandArgs[0];
            string packComponent = commandArgs[1];
            var obj = new PackagingItem();
            obj.ParentID = Convert.ToInt32(parentId);
            //obj.PackagingComponent = packComponent;
            int id = packagingItemService.InsertPackagingItem(obj, iItemId);

            var ctrl = (ucBOMEditable)Page.LoadControl(@"~/_controltemplates/15/Ferrara.Compass/ucBOMEditable.ascx");
            ctrl.PackagingItemId = id;
            ctrl.ParentID = parentId;
            ctrl.PackagingComponent = packComponent;
            ctrl.IsNew = true;

            var hdnPackagingID = (HiddenField)this.Parent.FindControl("hdnPackagingID");
            var hdnParentID = (HiddenField)this.Parent.FindControl("hdnParentID");
            var hdnPageState = (HiddenField)this.Parent.FindControl("hdnPageState");

            hdnPageState.Value = "PE";
            hdnParentID.Value = parentId;
            hdnPackagingID.Value = id.ToString();

            var phPage = (PlaceHolder)this.Parent.FindControl("phPage");
            phPage.Visible = false;
            openBtnSave();
            // Add messages to page;
            phUC.Controls.Clear();
            phUC.Controls.Add(ctrl);

        }

        #endregion
        private bool requirementsCheckPE2(PackagingItem item)
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
            else if (item.NewExisting == "New" && item.CurrentLikeItemDescription == "")
            {
                completed = false;
            }
            else if (item.NewExisting == "Existing" && item.MaterialDescription == "")
            {
                completed = false;
            }
            else if ((item.PackagingComponent != GlobalConstants.COMPONENTTYPE_CandySemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi) && (item.NewExisting == "New" && item.CurrentLikeItemReason == ""))
            {
                completed = false;
            }
            else if ((item.PackagingComponent != GlobalConstants.COMPONENTTYPE_CandySemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi) && (item.GraphicsChangeRequired == "Select..." || item.ComponentContainsNLEA == "Select..."))
            {
                completed = false;
            }
            else if ((item.PackagingComponent != GlobalConstants.COMPONENTTYPE_CandySemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi) && (item.GraphicsChangeRequired == "Yes" && item.ExternalGraphicsVendor == "Select..."))
            {
                completed = false;
            }
            else if ((item.PackagingComponent != GlobalConstants.COMPONENTTYPE_CandySemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi) && (item.GraphicsChangeRequired == "Yes" && item.GraphicsBrief == ""))
            {
                completed = false;
            }
            if (item.NewExisting == "New")
            {
                if (!string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_TransferSemi) && !string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_CandySemi) && !string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_PurchasedSemi))
                {
                    if (item.Width == "" || item.Height == "" || item.Length == "")
                    {
                        completed = false;
                    }
                }
                if (string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_TransferSemi) || string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_PurchasedSemi))
                {
                    if (item.TransferSEMIMakePackLocations == "Select..." || item.TransferSEMIMakePackLocations == "")
                    {
                        completed = false;
                    }
                    else if (item.CountryOfOrigin == "Select..." || item.CountryOfOrigin == "")
                    {
                        completed = false;
                    }
                    else if (item.PackLocation == "Select..." || item.PackLocation == "")
                    {
                        completed = false;
                    }
                }
                else if (item.PackagingComponent.StartsWith("Rigid"))
                {

                    if (item.Structure == "Select..." || item.Structure == "")
                    {
                        completed = false;
                    }
                    else if (item.StructureColor == "Select..." || item.StructureColor == "")
                    {
                        completed = false;
                    }
                }
                else if (item.PackagingComponent.StartsWith("Label"))
                {
                    if (item.Structure == "Select..." || item.Structure == "")
                    {
                        completed = false;
                    }
                    else if (item.StructureColor == "Select..." || item.StructureColor == "")
                    {
                        completed = false;
                    }
                    else if (item.Unwind == "Select..." || item.Unwind == "")
                    {
                        completed = false;
                    }
                }
                else if (item.PackagingComponent.StartsWith("Film"))
                {
                    if (item.WebWidth == "" || item.ExactCutOff == "" || item.FilmMaxRollOD == "" || item.FilmRollID == "" || item.BagFace == "")
                    {
                        completed = false;
                    }
                    else if (item.FilmSubstrate == "Select..." || item.FilmSubstrate == "")
                    {
                        completed = false;
                    }
                    else if (item.Unwind == "Select..." || item.Unwind == "")
                    {
                        completed = false;
                    }
                    else if (item.Structure == "Select..." || item.Structure == "")
                    {
                        completed = false;
                    }
                    else if (item.FilmPrintStyle == "Select..." || item.FilmPrintStyle == "")
                    {
                        completed = false;
                    }
                    else if (item.FilmStyle == "Select..." || item.FilmStyle == "")
                    {
                        completed = false;
                    }
                    else if (item.BackSeam == "Select..." || item.BackSeam == "")
                    {
                        completed = false;
                    }
                }
                else if (item.PackagingComponent.StartsWith("Corrugated"))
                {
                    if (item.CorrugatedPrintStyle == "Select..." || item.CorrugatedPrintStyle == "")
                    {
                        completed = false;
                    }
                }
                if (!string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_TransferSemi) && !string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_CandySemi) && !string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_PurchasedSemi) && !item.PackagingComponent.Contains("Finished Good"))
                {
                    if (completed)
                    {
                        bool cadExists = packagingItemService.BOMAttachmentsExist(ProjectNumber, item.Id);
                        if (!cadExists)
                        {
                            completed = false;
                        }
                    }
                }
            }
            return completed;
        }
        private bool requirementsCheckPE(PackagingItem item)
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
            else if (item.NewExisting == "New" && item.CurrentLikeItemDescription == "")
            {
                completed = false;
            }
            else if (item.NewExisting == "Existing" && item.MaterialDescription == "")
            {
                completed = false;
            }
            else if ((item.PackagingComponent != GlobalConstants.COMPONENTTYPE_CandySemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi) && (item.NewExisting == "New" && item.CurrentLikeItemReason == ""))
            {
                completed = false;
            }
            else if ((item.PackagingComponent != GlobalConstants.COMPONENTTYPE_CandySemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi) && (item.GraphicsChangeRequired == "Select..." || item.ComponentContainsNLEA == "Select..."))
            {
                completed = false;
            }
            else if ((item.PackagingComponent != GlobalConstants.COMPONENTTYPE_CandySemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi) && (item.GraphicsChangeRequired == "Yes" && item.ExternalGraphicsVendor == "Select..."))
            {
                completed = false;
            }
            else if ((item.PackagingComponent != GlobalConstants.COMPONENTTYPE_CandySemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi) && (item.GraphicsChangeRequired == "Yes" && item.GraphicsBrief == ""))
            {
                completed = false;
            }
            return completed;
        }
        private bool requirementsCheckPMRev1(PackagingItem item)
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
            else if (item.NewExisting == "New" && item.CurrentLikeItemDescription == "")
            {
                completed = false;
            }
            else if (item.NewExisting == "Existing" && item.MaterialDescription == "")
            {
                completed = false;
            }
            else if (item.Flowthrough == "Select..." || item.Flowthrough == "")
            {
                completed = false;
            }
            else if ((item.PackagingComponent != GlobalConstants.COMPONENTTYPE_CandySemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi) && (item.NewExisting == "New" && item.CurrentLikeItemReason == ""))
            {
                completed = false;
            }
            else if ((item.PackagingComponent != GlobalConstants.COMPONENTTYPE_CandySemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi) && (item.GraphicsChangeRequired == "Select..." || item.ComponentContainsNLEA == "Select..."))
            {
                completed = false;
            }
            else if ((item.PackagingComponent != GlobalConstants.COMPONENTTYPE_CandySemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi) && (item.GraphicsChangeRequired == "Yes" && item.ExternalGraphicsVendor == "Select..."))
            {
                completed = false;
            }
            else if ((item.PackagingComponent != GlobalConstants.COMPONENTTYPE_CandySemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi) && (item.GraphicsChangeRequired == "Yes" && item.GraphicsBrief == ""))
            {
                completed = false;
            }
            return completed;
        }
        private bool requirementsCheckPROC(PackagingItem item)
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
            else if (item.NewExisting == "New" && item.CurrentLikeItemDescription == "")
            {
                completed = false;
            }
            else if (item.NewExisting == "Existing" && item.MaterialDescription == "")
            {
                completed = false;
            }
            else if ((item.PackagingComponent != GlobalConstants.COMPONENTTYPE_CandySemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi) && (item.NewExisting == "New" && item.CurrentLikeItemReason == ""))
            {
                completed = false;
            }
            else if ((item.PackagingComponent != GlobalConstants.COMPONENTTYPE_CandySemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi) && (item.GraphicsChangeRequired == "Select..." || item.ComponentContainsNLEA == "Select..."))
            {
                completed = false;
            }
            else if ((item.PackagingComponent != GlobalConstants.COMPONENTTYPE_CandySemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi) && (item.GraphicsChangeRequired == "Yes" && item.ExternalGraphicsVendor == "Select..."))
            {
                completed = false;
            }
            else if ((item.PackagingComponent != GlobalConstants.COMPONENTTYPE_CandySemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi) && (item.GraphicsChangeRequired == "Yes" && item.GraphicsBrief == ""))
            {
                completed = false;
            }
            else if (!string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_CandySemi) && !string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_TransferSemi) && !string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_PurchasedSemi) && !item.PackagingComponent.Contains("Finished Good"))
            {
                if (item.NewExisting == "New" && (item.PrinterSupplier == "Select..." || item.PrinterSupplier == ""))
                {
                    completed = false;
                }
            }
            return completed;
        }
        public void saveData()
        {
            var phBOM = (PlaceHolder)this.Parent.FindControl("phBOM");
            foreach (UserControl ctrl in phBOM.Controls)
            {
                if (ctrl is ucBOMGrid)
                {
                    var type = (ucBOMGrid)ctrl;
                    Repeater gridItems = type.rptPackingItem;
                    foreach (RepeaterItem item in gridItems.Items)
                    {
                        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                        {
                            string deletedStatus = ((HiddenField)item.FindControl("hdnDeletedStatus")).Value;
                            if (deletedStatus == "true")
                            {
                                string deleteId = ((HiddenField)item.FindControl("hdnItemID")).Value;
                                packagingItemService.DeletePackagingItem(Convert.ToInt32(deleteId));
                            }

                        }
                    }
                }
            }
        }

    }
}
