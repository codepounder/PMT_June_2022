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

namespace Ferrara.Compass.ControlTemplates.Ferrara.Compass
{
    public partial class ucTS : UserControl
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

        private const string _ucTSPath = @"~/_controltemplates/15/Ferrara.Compass/ucTS.ascx";
        #endregion

        #region Properties
        public int PackagingItemId { get; set; }
        public int ParentId { get; set; }
        public int CompassItemId { get; set; }
        public string ParentComponentType { get; set; }
        public string MaterialNumber { get; set; }
        public string MaterialDesc { get; set; }
        public string IPFMode { get; set; }
        public string ProjectNumber { get; set; }
        public int ActiveTab { get; set; }
        public List<PackagingItem> AllPIs { get; set; }
        public List<PackagingItem> SAPPIs { get; set; }
        public List<FileAttribute> projectAttachments { get; set; }
        public string firstLoad { get; set; }

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
            //exceptionService.Handle(LogCategory.General, "Looging timing for diagnostics", "UCTS", "strat - Page_Load", DateTime.Now.ToString("yyyy MM dd HH:mm:ss FFF"));
            webUrl = SPContext.Current.Web.Url;
            iItemId = CompassItemId;
            Label buttonLink = new Label();
            if (ParentId != 0)
            {
                if (ParentComponentType == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                {
                    buttonLink.Text = "PCS: " + MaterialNumber;
                }
                else
                {
                    buttonLink.Text = "TS: " + MaterialNumber;
                }
                PackagingItem parentIDParent = (from PI in AllPIs where PI.Id == ParentId select PI).FirstOrDefault();
                int parentIdParentId = parentIDParent.ParentID;
                if (parentIdParentId != 0)
                {
                    buttonLink.CssClass += " childItem ";
                }
                //lblTSNote.Text = "Please enter the Candy Semi and all components for the " + GlobalConstants.COMPONENTTYPE_PurchasedSemi + " BOM below:";
                lblTSNote.Text = " Please enter the Candy Semi and all components (if known or specifically indicated). Include graphic details for these material components.";
            }
            else
            {
                buttonLink.Text = "Finished Good";
                lblTSNote.Text = "Please enter semi candy or transfer semi and all components of the FG BOM. If you do not know if transfer semis will be used please enter the candy semi information as a component in the FG BOM.";
            }
            buttonLink.Attributes.Add("onclick", "activateTS(this)");
            buttonLink.CssClass += "tsButtonLink";
            TSButton.Controls.Add(buttonLink);
            LoadBOMItems();
            //exceptionService.Handle(LogCategory.General, "Looging timing for diagnostics", "UCTS", "End - Page_Load", DateTime.Now.ToString("yyyy MM dd HH:mm:ss FFF"));
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "drpCompType_load", "drpCompType_load();", true);
        }
        private void InitializeScreen()
        {

        }

        #endregion
        #region Repeater Events
        protected void rptTSItem_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            //exceptionService.Handle(LogCategory.General, "Looging timing for diagnostics", "UCTS", "start: rptTSItem_ItemDataBound", DateTime.Now.ToString("yyyy MM dd HH:mm:ss FFF"));
            string matLikeNumb, matLikeDesc;
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                PackagingItem packagingItem = (PackagingItem)e.Item.DataItem;

                TextBox txtMaterial = (TextBox)e.Item.FindControl("txtMaterial");
                TextBox txtMaterialDesc = (TextBox)e.Item.FindControl("txtMaterialDesc");
                TextBox txtLikeMaterialDesc = (TextBox)e.Item.FindControl("txtLikeMaterialDesc");
                TextBox txtLikeMaterial = (TextBox)e.Item.FindControl("txtLikeMaterial");
                TextBox txtOldMaterialDesc = (TextBox)e.Item.FindControl("txtOldMaterialDesc");
                TextBox txtOldMaterial = (TextBox)e.Item.FindControl("txtOldMaterial");
                TextBox txtTSComments = (TextBox)e.Item.FindControl("txtTSComments");
                DropDownList drpGraphicsNeeded = ((DropDownList)e.Item.FindControl("drpGraphicsNeeded"));
                DropDownList ddlGraphicsVendor = ((DropDownList)e.Item.FindControl("ddlGraphicsVendor"));
                DropDownList ddlFlowthrough = ((DropDownList)e.Item.FindControl("ddlFlowthrough"));
                DropDownList ddlProductHierarchyLevel1 = ((DropDownList)e.Item.FindControl("ddlPHL1"));
                DropDownList ddlBrand_Material = ((DropDownList)e.Item.FindControl("ddlBrand"));
                DropDownList ddlProductHierarchyLevel2 = ((DropDownList)e.Item.FindControl("ddlPHL2"));
                TextBox txtProfitCenter = (TextBox)e.Item.FindControl("txtProfitCenterUC");
                HiddenField hdnProfitCenterUC = (HiddenField)e.Item.FindControl("hdnProfitCenterUC");
                TextBox txtGraphicsBrief = (TextBox)e.Item.FindControl("txtGraphicsBrief");
                HtmlControl spanGraphicsBrief = (HtmlControl)e.Item.FindControl("spanGraphicsBrief");
                HtmlControl spanWhyComponent = (HtmlControl)e.Item.FindControl("spanWhyComponent");
                TextBox txtLikeReason = (TextBox)e.Item.FindControl("txtLikeReason");
                HtmlControl spanGraphicsVendor = (HtmlControl)e.Item.FindControl("spanGraphicsVendor");
                HiddenField hdnDeletedStatus = (HiddenField)e.Item.FindControl("hdnDeletedStatus");
                HiddenField hdnParentId = (HiddenField)e.Item.FindControl("hdnParentId");
                hdnParentId.Value = packagingItem.ParentID.ToString();
                HiddenField hdnComponentType = (HiddenField)e.Item.FindControl("hdnComponentType");
                //UpdatePanel hierarchyPanel2 = (UpdatePanel)e.Item.FindControl("hierarchyPanel2");
                //HiddenField plmFLag = (HiddenField)this.Parent.FindControl("hdnPLMFlag");
                //if (plmFLag.Value != "Yes")
                //{
                //    hierarchyPanel2.Visible = false;
                //}
                //else
                //{
                //    if (e.Item.ItemIndex % 2 != 0)
                //    {

                //        hierarchyPanel2.Attributes.Add("class", "comment-highlighted");
                //    }
                //}
                hdnComponentType.Value = ParentComponentType;
                TextBox txtPackQty = (TextBox)e.Item.FindControl("txtPackQty");
                txtTSComments.Text = packagingItem.Notes;
                List<KeyValuePair<int, string>> allItems = new List<KeyValuePair<int, string>>();
                List<int> idsOnly = new List<int>();
                DropDownList ddlMoveTS = ((DropDownList)e.Item.FindControl("ddlMoveTS"));

                allItems.Add(new KeyValuePair<int, string>(0, "Finished Good"));
                idsOnly.Add(0);

                foreach (PackagingItem item in AllPIs)
                {
                    if (item.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi || item.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi)
                    {
                        allItems.Add(new KeyValuePair<int, string>(item.Id, item.MaterialNumber + ": " + item.MaterialDescription));
                        idsOnly.Add(item.Id);
                    }
                }

                if (allItems != null && allItems.Count > 0)
                {
                    List<int> filteredParents = filterParents(idsOnly, CompassItemId, packagingItem.Id);
                    foreach (KeyValuePair<int, string> IDs in allItems)
                    {
                        int approvedId = filteredParents.IndexOf(IDs.Key);
                        if (IDs.Key != packagingItem.Id && approvedId > -1)
                        {
                            ddlMoveTS.Items.Add(new ListItem(IDs.Value, IDs.Key.ToString()));
                        }
                    }
                }
                hdnDeletedStatus.Value = packagingItem.Deleted;
                if (!string.IsNullOrEmpty(packagingItem.PackQuantity))
                {
                    var pacqty = (packagingItem.PackQuantity == "-9999" || string.IsNullOrEmpty(packagingItem.PackQuantity)) ? "0" : packagingItem.PackQuantity;
                    txtPackQty.Text = Math.Round(Convert.ToDouble(pacqty), 4).ToString();
                }
                txtLikeReason.Text = string.IsNullOrEmpty(packagingItem.CurrentLikeItemReason) ? "" : packagingItem.CurrentLikeItemReason;
                txtGraphicsBrief.Text = string.IsNullOrEmpty(packagingItem.GraphicsBrief) ? "" : packagingItem.GraphicsBrief;
                DropDownList ddlPackagingComponent = ((DropDownList)e.Item.FindControl("drpComponent"));
                Utilities.BindDropDownItems(ddlPackagingComponent, GlobalConstants.LIST_PackagingComponentTypesLookup, webUrl);
                Utilities.BindDropDownItems(ddlFlowthrough, GlobalConstants.LIST_FlowThroughTypeLookup, webUrl);
                Utilities.BindDropDownItemsPHL1(ddlProductHierarchyLevel1, GlobalConstants.LIST_ProductHierarchyLevel1Lookup, webUrl);
                Utilities.BindDropDownItemsPHL2(ddlProductHierarchyLevel2, GlobalConstants.LIST_ProductHierarchyLevel2Lookup, webUrl);
                Utilities.BindDropDownItemsBrand(ddlBrand_Material, GlobalConstants.LIST_MaterialGroup1Lookup, webUrl);
                // Load the level 1 list
                Utilities.SetDropDownValue(packagingItem.PHL1, ddlProductHierarchyLevel1, this.Page);
                //ddlProductHierarchyLevel1_SelectedIndexChanged(ddlProductHierarchyLevel1, null);

                Utilities.SetDropDownValue(packagingItem.PHL2, ddlProductHierarchyLevel2, this.Page);
                // Load the level 2 list
                //ddlProductHierarchyLevel2_SelectedIndexChanged(ddlProductHierarchyLevel2, null);

                // Load Brand List
                Utilities.SetDropDownValue(packagingItem.Brand, ddlBrand_Material, this.Page);
                //ddlBrand_SelectedIndexChanged(ddlBrand_Material, null);

                txtProfitCenter.Text = packagingItem.ProfitCenter;
                hdnProfitCenterUC.Value = packagingItem.ProfitCenter;

                if (packagingItem.PackagingComponent != null && packagingItem.PackagingComponent != string.Empty)
                {
                    Utilities.SetDropDownValue(packagingItem.PackagingComponent, ddlPackagingComponent, this.Page);
                    if (packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi || packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi)
                    {
                        int childCount = (from PI in AllPIs where PI.ParentID == packagingItem.Id select PI).Count();
                        if (childCount > 0)
                        {
                            ddlPackagingComponent.Enabled = false;
                            Panel lblCompNote = ((Panel)e.Item.FindControl("lblCompNote"));
                            lblCompNote.Visible = true;
                            if (e.Item.ItemIndex % 2 == 0)
                            {
                                lblCompNote.CssClass = "row";
                            }
                            else
                            {
                                lblCompNote.CssClass = "row blueRow";
                            }
                        }
                    }
                }

                DropDownList drpUnitOfMeasure = ((DropDownList)e.Item.FindControl("drpUnitOfMeasure"));
                Utilities.BindDropDownItems(drpUnitOfMeasure, GlobalConstants.LIST_PackUnitLookup, webUrl);
                if (packagingItem.PackUnit != null && packagingItem.PackUnit != string.Empty)
                {
                    Utilities.SetDropDownValue(packagingItem.PackUnit, drpUnitOfMeasure, this.Page);
                }

                DropDownList drpComponentContainsNLEA = ((DropDownList)e.Item.FindControl("drpComponentContainsNLEA"));
                if (packagingItem.ComponentContainsNLEA != null && packagingItem.ComponentContainsNLEA != string.Empty)
                    Utilities.SetDropDownValue(packagingItem.ComponentContainsNLEA, drpComponentContainsNLEA, this.Page);
                if (packagingItem.Flowthrough != null && packagingItem.Flowthrough != string.Empty)
                    Utilities.SetDropDownValue(packagingItem.Flowthrough, ddlFlowthrough, this.Page);
                matLikeNumb = string.IsNullOrEmpty(packagingItem.CurrentLikeItem) ? "" : packagingItem.CurrentLikeItem;
                matLikeDesc = string.IsNullOrEmpty(packagingItem.CurrentLikeItemDescription) ? "" : packagingItem.CurrentLikeItemDescription;
                if (matLikeDesc == "N/A")
                {
                    matLikeDesc = "Not Applicable";
                }
                if (matLikeNumb == "N/A")
                {
                    matLikeNumb = "Not Applicable";
                }
                matLikeNumb = string.IsNullOrEmpty(packagingItem.CurrentOldItem) ? "" : packagingItem.CurrentOldItem;
                matLikeDesc = string.IsNullOrEmpty(packagingItem.CurrentOldItemDescription) ? "" : packagingItem.CurrentOldItemDescription;
                if (matLikeDesc == "N/A")
                {
                    matLikeDesc = "Not Applicable";
                }
                if (matLikeNumb == "N/A")
                {
                    matLikeNumb = "Not Applicable";
                }
                txtOldMaterial.Text = matLikeNumb;
                txtOldMaterialDesc.Text = matLikeDesc;
                DropDownList drpNew = ((DropDownList)e.Item.FindControl("drpNew"));
                if (packagingItem.NewExisting != null && packagingItem.NewExisting != string.Empty)
                {
                    Utilities.SetDropDownValue(packagingItem.NewExisting, drpNew, this.Page);
                }

                Utilities.BindDropDownItems(ddlGraphicsVendor, GlobalConstants.LIST_ExternalGraphicsVendorLookup, webUrl);
                if (!string.IsNullOrEmpty(packagingItem.ExternalGraphicsVendor))
                {
                    Utilities.SetDropDownValue(packagingItem.ExternalGraphicsVendor, ddlGraphicsVendor, this.Page);
                }

                if (!string.IsNullOrEmpty(packagingItem.GraphicsChangeRequired))
                {
                    Utilities.SetDropDownValue(packagingItem.GraphicsChangeRequired, drpGraphicsNeeded, this.Page);

                    if (packagingItem.GraphicsChangeRequired.ToLower() == "yes")
                    {
                        txtGraphicsBrief.CssClass = "form-control GraphicsBrief PCBOMrequired";
                        spanGraphicsBrief.Attributes.Add("class", "markrequired spanGraphicsBrief showItempc");
                        ddlGraphicsVendor.CssClass = "form-control drpGraphicsVendor PCBOMrequired";
                        spanGraphicsVendor.Attributes.Add("class", "markrequired spanGraphicsVendor showItempc");
                    }
                    else
                    {
                        spanGraphicsBrief.Attributes.Add("class", "markrequired spanGraphicsBrief hideItem");
                        txtGraphicsBrief.CssClass = "form-control GraphicsBrief";
                        spanGraphicsVendor.Attributes.Add("class", "markrequired spanGraphicsVendor hideItem");
                        ddlGraphicsVendor.CssClass = "form-control drpGraphicsVendor";
                    }

                }

                #region Visual Reference / Rendering:
                HtmlImage imgAtt = (HtmlImage)e.Item.FindControl("btnAttachment");

                var files =
                    (
                        from
                            attachment in projectAttachments
                        where
                            attachment.PackagingComponentItemId == packagingItem.Id
                            &&
                            attachment.DocType == GlobalConstants.DOCTYPE_Rendering
                        select
                            attachment
                      ).ToList();

                if (files.Count > 0)
                {
                    ImageButton btnDeleteAttachment = (ImageButton)e.Item.FindControl("btnDeleteAttachment");
                    HtmlAnchor anc = ((HtmlAnchor)e.Item.FindControl("ancRendering"));
                    HiddenField DeletedVisualreferenceUrl = ((HiddenField)e.Item.FindControl("DeletedVisualreferenceUrl"));

                    btnDeleteAttachment.Visible = true;
                    if (anc != null)
                    {
                        string fileName = files[0].FileName;
                        fileName = fileName.Replace("_", " ");
                        anc.Controls.Add(new LiteralControl(fileName));
                        anc.HRef = files[0].FileUrl;
                        btnDeleteAttachment.CommandArgument = files[0].FileUrl;
                        DeletedVisualreferenceUrl.Value = files[0].FileUrl;
                    }

                    imgAtt.Visible = false;
                }
                #endregion

                #region Approved Graphics Asset
                HtmlImage btnApprovedGraphicsAsset = (HtmlImage)e.Item.FindControl("btnApprovedGraphicsAsset");

                var ApprovedGraphicsAssets =
                    (
                        from
                            attachment in projectAttachments
                        where
                            attachment.PackagingComponentItemId == packagingItem.Id
                            &&
                            attachment.DocType == GlobalConstants.DOCTYPE_ApprovedGraphicsAsset
                        select
                            attachment
                      ).ToList();

                if (ApprovedGraphicsAssets.Count > 0)
                {
                    ImageButton btnDeleteApprovedGraphicsAsset = (ImageButton)e.Item.FindControl("btnDeleteApprovedGraphicsAsset");
                    HtmlAnchor ancApprovedGraphicsAsset = ((HtmlAnchor)e.Item.FindControl("ancApprovedGraphicsAsset"));
                    HiddenField DeletedApprovedGraphicsAssetUrl = ((HiddenField)e.Item.FindControl("DeletedApprovedGraphicsAssetUrl"));

                    btnDeleteApprovedGraphicsAsset.Visible = true;
                    if (ancApprovedGraphicsAsset != null)
                    {
                        string fileName = ApprovedGraphicsAssets[0].FileName;
                        fileName = fileName.Replace("_", " ");
                        ancApprovedGraphicsAsset.Controls.Add(new LiteralControl(fileName));
                        ancApprovedGraphicsAsset.HRef = ApprovedGraphicsAssets[0].FileUrl;
                        btnDeleteApprovedGraphicsAsset.CommandArgument = ApprovedGraphicsAssets[0].FileUrl;
                        DeletedApprovedGraphicsAssetUrl.Value = ApprovedGraphicsAssets[0].FileUrl;
                    }

                    btnApprovedGraphicsAsset.Visible = false;
                }
                #endregion

                if (packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi || packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                {
                    ucTS ctrl2 = (ucTS)Page.LoadControl(_ucTSPath);
                    ctrl2.ParentId = packagingItem.Id;
                    ctrl2.MaterialNumber = packagingItem.MaterialNumber;
                    ctrl2.MaterialDesc = packagingItem.MaterialDescription;
                    ctrl2.ProjectNumber = ProjectNumber;
                    ctrl2.CompassItemId = iItemId;
                    ctrl2.AllPIs = AllPIs;
                    ctrl2.ParentComponentType = packagingItem.PackagingComponent;
                    ctrl2.projectAttachments = projectAttachments;
                    // ctrl2.firstLoad = "true";
                    string PCTS = "";
                    if (packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi)
                    {
                        PCTS = "TS";
                    }
                    else if (packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                    {
                        PCTS = "PCS";
                    }
                    ctrl2.ID = PCTS + packagingItem.Id;

                    this.Parent.FindControl("phTS").Controls.Add(ctrl2);
                }
            }
            if (firstLoad != null && firstLoad != "true")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ProjectTypeChange", "ProjectTypeChange();", true);
            }
            //exceptionService.Handle(LogCategory.General, "Looging timing for diagnostics", "UCTS", "end: rptTSItem_ItemDataBound", DateTime.Now.ToString("yyyy MM dd HH:mm:ss FFF"));
        }
        #endregion
        #region Data Transfer Methods

        private void LoadBOMItems()
        {
            //exceptionService.Handle(LogCategory.General, "Looging timing for diagnostics", "UCTS", "start: LoadBOMItems", DateTime.Now.ToString("yyyy MM dd HH:mm:ss FFF"));
            List<PackagingItem> dtPackingItem;
            if (ParentId == 0)
            {
                if (SAPPIs != null)
                {
                    if (SAPPIs.Count > 0)
                    {
                        AllPIs.AddRange(packagingItemService.InsertPackagingItems(SAPPIs, iItemId, ProjectNumber));
                    }
                }
                dtPackingItem = (from PIs in AllPIs where PIs.ParentID == 0 select PIs).ToList();
            }
            else
            {
                dtPackingItem = (from PIs in AllPIs where PIs.ParentID == ParentId select PIs).ToList();
            }

            btnAddNewPackagingItem.CommandArgument = ParentId.ToString();

            if (dtPackingItem.Count > 0)
            {
                if (!string.IsNullOrEmpty(IPFMode))
                {
                    // if this is a Copy or Change request, set all the Packaging Items Ids to a negative number.
                    // Since we are copying an existing item, we want to clear all the ids so we don't try to delete non-existing items.
                    foreach (PackagingItem pi in dtPackingItem)
                    {
                        pi.OriginalId = pi.Id;
                        pi.Id = Utilities.GetUniqueId();
                    }
                }

                rptTSItem.DataSource = dtPackingItem;
                rptTSItem.DataBind();
            }
            //exceptionService.Handle(LogCategory.General, "Looging timing for diagnostics", "UCTS", "end: LoadBOMItems", DateTime.Now.ToString("yyyy MM dd HH:mm:ss FFF"));
        }
        #endregion
        #region Button Methods
        protected void btnAddNewPackagingItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            saveData();

            var obj = new PackagingItem();
            int tempParentId = Convert.ToInt32(btnAddNewPackagingItem.CommandArgument.ToString());
            obj.ParentID = tempParentId;
            obj.CompassListItemId = iItemId.ToString();
            List<PackagingItem> dtPackingItem = new List<PackagingItem>();
            int id = packagingItemService.InsertPackagingItem(obj, iItemId);

            AllPIs = packagingItemService.GetAllPackagingItemsForProject(iItemId);
            projectAttachments = packagingItemService.GetRenderingUploadedFiles(ProjectNumber);
            projectAttachments.AddRange(packagingItemService.GetApprovedGraphicsAssetUploadedFiles(ProjectNumber));

            ucTS ctrl = (ucTS)this.Parent.Page.LoadControl(_ucTSPath);
            ctrl.ParentId = 0;
            ctrl.MaterialNumber = "";
            ctrl.MaterialDesc = "";
            ctrl.ParentComponentType = "";
            ctrl.ProjectNumber = ProjectNumber;
            ctrl.IPFMode = IPFMode;
            ctrl.AllPIs = AllPIs;
            ctrl.SAPPIs = SAPPIs;
            ctrl.CompassItemId = iItemId;
            ctrl.firstLoad = "false";
            ctrl.ID = "FinishedGood";
            ctrl.projectAttachments = projectAttachments;

            var activeTab = (HiddenField)this.Parent.FindControl("activeTabHolder");

            PlaceHolder phFG = (PlaceHolder)this.Parent.FindControl("phFG");
            PlaceHolder phTS = (PlaceHolder)this.Parent.FindControl("phTS");
            HiddenField hdnUCLoaded = (HiddenField)this.Parent.FindControl("hdnUCLoaded");
            hdnUCLoaded.Value = "false";

            phFG.Controls.Clear();
            phTS.Controls.Clear();
            phFG.Controls.Add(ctrl);
            activeTab.Value = tempParentId.ToString();
        }
        public void saveData()
        {
            List<PlaceHolder> ipfUC = new List<PlaceHolder>();
            PlaceHolder phFG = (PlaceHolder)this.Parent.FindControl("phFG");
            PlaceHolder phTS = (PlaceHolder)this.Parent.FindControl("phTS");
            ipfUC.Add(phFG);
            ipfUC.Add(phTS);
            List<PackagingItem> PIsToSave = new List<PackagingItem>();
            foreach (PlaceHolder phUC in ipfUC)
            {
                foreach (UserControl uc in phUC.Controls)
                {
                    var type = (ucTS)uc;
                    Repeater repeater = type.rptTSItem;
                    foreach (RepeaterItem item in repeater.Items)
                    {
                        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                        {
                            PackagingItem packagingItem = new PackagingItem();

                            packagingItem.PackagingComponent = ((DropDownList)item.FindControl("drpComponent")).SelectedItem.Text;
                            packagingItem.NewExisting = ((DropDownList)item.FindControl("drpNew")).SelectedItem.Text;
                            packagingItem.MaterialNumber = ((TextBox)item.FindControl("txtMaterial")).Text;
                            packagingItem.CurrentLikeItem = ((TextBox)item.FindControl("txtLikeMaterial")).Text;
                            packagingItem.MaterialDescription = ((TextBox)item.FindControl("txtMaterialDesc")).Text;
                            packagingItem.CurrentLikeItemDescription = ((TextBox)item.FindControl("txtLikeMaterialDesc")).Text;
                            packagingItem.CurrentLikeItemReason = ((TextBox)item.FindControl("txtLikeReason")).Text;
                            packagingItem.CurrentOldItem = ((TextBox)item.FindControl("txtOldMaterial")).Text;
                            packagingItem.CurrentOldItemDescription = ((TextBox)item.FindControl("txtOldMaterialDesc")).Text;
                            packagingItem.PackQuantity = ((TextBox)item.FindControl("txtPackQty")).Text;
                            packagingItem.PackUnit = ((DropDownList)item.FindControl("drpUnitOfMeasure")).SelectedItem.Text;
                            packagingItem.GraphicsBrief = ((TextBox)item.FindControl("txtGraphicsBrief")).Text;
                            packagingItem.GraphicsChangeRequired = ((DropDownList)item.FindControl("drpGraphicsNeeded")).SelectedItem.Text;
                            var CompId = ((HiddenField)item.FindControl("hdnItemID")).Value.Replace(",", "");
                            packagingItem.Id = string.IsNullOrEmpty(CompId) ? 0 : Convert.ToInt32(CompId);
                            packagingItem.ExternalGraphicsVendor = ((DropDownList)item.FindControl("ddlGraphicsVendor")).SelectedItem.Text;
                            packagingItem.ComponentContainsNLEA = ((DropDownList)item.FindControl("drpComponentContainsNLEA")).SelectedItem.Text;
                            if (packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi || packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi)
                            {
                                packagingItem.PHL1 = ((DropDownList)item.FindControl("ddlPHL1")).SelectedItem.Text;
                                packagingItem.PHL2 = ((DropDownList)item.FindControl("ddlPHL2")).SelectedItem.Text;
                                packagingItem.Brand = ((DropDownList)item.FindControl("ddlBrand")).SelectedItem.Text;
                                packagingItem.ProfitCenter = ((TextBox)item.FindControl("txtProfitCenterUC")).Text;
                                packagingItem.ProfitCenter = ((HiddenField)item.FindControl("hdnProfitCenterUC")).Value;
                            }
                            packagingItem.Flowthrough = ((DropDownList)item.FindControl("ddlFlowthrough")).SelectedItem.Text;
                            packagingItem.Notes = ((TextBox)item.FindControl("txtTSComments")).Text;
                            packagingItem.CompassListItemId = iItemId.ToString();

                            int moveId = Convert.ToInt32(((DropDownList)item.FindControl("ddlMoveTS")).SelectedItem.Value);
                            string movedPackType = ((DropDownList)item.FindControl("ddlMoveTS")).SelectedItem.Text;
                            if (moveId != -1)
                            {
                                packagingItem.ParentID = moveId;
                            }
                            else
                            {
                                var hdnParentId = ((HiddenField)item.FindControl("hdnParentId")).Value;
                                if (!string.IsNullOrEmpty(hdnParentId))
                                {
                                    packagingItem.ParentID = Convert.ToInt32(hdnParentId);
                                }
                            }
                            string deletedStatus = ((HiddenField)item.FindControl("hdnDeletedStatus")).Value;
                            if (deletedStatus == "deleted")
                            {
                                packagingItem.Deleted = "Yes";
                            }
                            if (packagingItem.Id <= 0 && deletedStatus != "deleted")
                            {
                                int newId = packagingItemService.InsertPackagingItem(packagingItem, iItemId);
                                HiddenField hidId = ((HiddenField)item.FindControl("hdnItemID"));
                                if (hidId != null)
                                    hidId.Value = newId.ToString();
                            }
                            else if (packagingItem.Id <= 0 && deletedStatus == "deleted")
                            {

                            }
                            else if (packagingItem.Id > 0 && deletedStatus == "deleted")
                            {
                                packagingItemService.DeletePackagingItem(packagingItem.Id);
                            }
                            else
                            {
                                PIsToSave.Add(packagingItem);
                                //packagingItemService.UpdateIPFPackagingItem(packagingItem);
                            }
                        }
                    }
                }
            }
            if (PIsToSave.Count > 0)
            {
                packagingItemService.UpdateIPFPackagingItems(PIsToSave, iItemId);
            }

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Packaging Component: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "PackagingComponentEntryForm", "btnSubmit_Click");
            }
        }
        #endregion
        public List<int> filterParents(List<int> IDs, int compassItemID, int movingId)
        {
            List<int> idsHolder = IDs;
            List<KeyValuePair<int, int>> AllItems = new List<KeyValuePair<int, int>>();
            foreach (PackagingItem item in AllPIs)
            {
                AllItems.Add(new KeyValuePair<int, int>(item.Id, item.ParentID));
            }
            foreach (KeyValuePair<int, int> KVP in AllItems)
            {
                bool includeItems = includeItem(KVP.Key, movingId, AllItems);
                int IDIndex = IDs.IndexOf(KVP.Key);
                if (!includeItems && IDIndex >= 0)
                {

                    idsHolder.RemoveAt(IDIndex);
                }
            }
            return idsHolder;
        }
        private bool includeItem(int lookupID, int compareID, List<KeyValuePair<int, int>> compareList)
        {
            bool includeID = true;
            int parentID = (from KVP in compareList where KVP.Key == lookupID select KVP.Value).FirstOrDefault();
            while (parentID >= 0)
            {
                if (parentID == 0)
                {
                    break;
                }
                if (parentID == compareID)
                {
                    includeID = false;
                    break;
                }
                int newParentID = (from KVP in compareList where KVP.Key == parentID select KVP.Value).FirstOrDefault();
                parentID = newParentID;
            }

            return includeID;
        }
        protected void ddlProductHierarchyLevel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlProductHierarhcyLevel1 = (DropDownList)sender;
            string productHierarhcyLevel1 = ddlProductHierarhcyLevel1.SelectedItem.Text;
            Control repeaterItem = (Control)ddlProductHierarhcyLevel1.Parent;
            DropDownList ddlBrand_Material = ((DropDownList)repeaterItem.FindControl("ddlBrand"));
            DropDownList ddlProductHierarchyLevel2 = ((DropDownList)repeaterItem.FindControl("ddlPHL2"));
            TextBox txtProfitCenter = (TextBox)repeaterItem.FindControl("txtProfitCenterUC");
            string level2 = Utilities.GetLookupValue(GlobalConstants.LIST_ProductHierarchyLevel1Lookup, productHierarhcyLevel1, webUrl);

            if ((!string.IsNullOrEmpty(level2)) && (!string.Equals(level2, "Select...")))
            {
                Utilities.BindDropDownItemsByValue(ddlProductHierarchyLevel2, GlobalConstants.LIST_ProductHierarchyLevel2Lookup, level2, webUrl);
                ddlProductHierarchyLevel2.SelectedIndex = -1;
            }
            else
            {
                ddlProductHierarchyLevel2.Items.Clear();
                ListItem li = new ListItem();
                li.Text = "Select...";
                li.Value = "-1";
                ddlProductHierarchyLevel2.Items.Add(li);
            }
            ddlBrand_Material.Items.Clear();
            ListItem li2 = new ListItem();
            li2.Text = "Select...";
            li2.Value = "-1";
            ddlBrand_Material.Items.Add(li2);
            txtProfitCenter.Text = "";
        }
        protected void ddlProductHierarchyLevel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlProductHierarchyLevel2 = (DropDownList)sender;
            String productHierarhcyLevel2 = ddlProductHierarchyLevel2.SelectedItem.Text;

            Control repeaterItem = (Control)ddlProductHierarchyLevel2.Parent;
            DropDownList productHierarhcyLevel1 = ((DropDownList)repeaterItem.FindControl("ddlPHL1"));
            DropDownList ddlBrand_Material = ((DropDownList)repeaterItem.FindControl("ddlBrand"));
            TextBox txtProfitCenter = (TextBox)repeaterItem.FindControl("txtProfitCenterUC");

            if ((!string.IsNullOrEmpty(productHierarhcyLevel2)) && (!string.Equals(productHierarhcyLevel2, "Select...")))
            {
                Utilities.BindDropDownItemsByValueAndColumn(ddlBrand_Material, GlobalConstants.LIST_MaterialGroup1Lookup, "ParentPHL2", productHierarhcyLevel2, webUrl);
                ddlBrand_Material.SelectedIndex = -1;
            }
            else
            {
                ddlBrand_Material.Items.Clear();
                ListItem li = new ListItem();
                li.Text = "Select...";
                li.Value = "-1";
                ddlBrand_Material.Items.Add(li);

            }
            txtProfitCenter.Text = "";
        }
        protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlBrand_Material = (DropDownList)sender;
            Control repeaterItem = (Control)ddlBrand_Material.Parent;
            DropDownList productHierarhcyLevel1 = ((DropDownList)repeaterItem.FindControl("ddlPHL1"));
            DropDownList ddlProductHierarchyLevel2 = ((DropDownList)repeaterItem.FindControl("ddlPHL2"));
            TextBox txtProfitCenter = (TextBox)repeaterItem.FindControl("txtProfitCenterUC");
            string profitCenter = Utilities.GetLookupDetailsByValueAndColumn("ProfitCenter", GlobalConstants.LIST_MaterialGroup1Lookup, "Title", ddlBrand_Material.SelectedItem.Text, "ParentPHL2", ddlProductHierarchyLevel2.SelectedItem.Text, webUrl);
            txtProfitCenter.Text = profitCenter;
        }
    }
}
