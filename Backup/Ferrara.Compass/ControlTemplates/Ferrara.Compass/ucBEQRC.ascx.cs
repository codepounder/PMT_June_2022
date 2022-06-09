using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Classes;
using System;
using System.Collections.Generic;
using System.Web.UI;
using Microsoft.Practices.Unity;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Linq;
using System.Web.UI.HtmlControls;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.ControlTemplates.Ferrara.Compass
{
    public partial class ucBEQRC : UserControl
    {
        #region Member Variables
        private IBEQRCService BEQRCService;
        private IPackagingItemService packagingItemService;
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private string webUrl;
        private int iItemId = 0;
        private List<PackagingItem> packagingItems = new List<PackagingItem>();
        private const string _ucBEQRCPath = @"~/_controltemplates/15/Ferrara.Compass/ucBEQRC.ascx";
        #endregion

        #region Properties
        public int ParentId { get; set; }
        public int CompassItemId { get; set; }
        public string ParentComponentType { get; set; }
        public string MaterialNumber { get; set; }
        public string MaterialDesc { get; set; }
        //public string IPFMode { get; set; }
        public string ProjectNumber { get; set; }
        public int ActiveTab { get; set; }
        public List<PackagingItem> AllPIs { get; set; }
        public List<FileAttribute> projectAttachments { get; set; }
        public string firstLoad { get; set; }
        public List<string> UPCAssociated { get; set; }

        #endregion
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeScreen();
            // Page.EnableEventValidation = false;

            BEQRCService = DependencyResolution.DependencyMapper.Container.Resolve<IBEQRCService>();
            packagingItemService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            EnsureScriptManager();
            EnsureUpdatePanelFixups();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
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
            string matLikeNumb, matLikeDesc;
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                PackagingItem packagingItem = (PackagingItem)e.Item.DataItem;

                DropDownList ddlPackagingComponent = ((DropDownList)e.Item.FindControl("drpComponent"));
                Utilities.BindDropDownItems(ddlPackagingComponent, GlobalConstants.LIST_PackagingComponentTypesLookup, webUrl);
                if (packagingItem.PackagingComponent != null && packagingItem.PackagingComponent != string.Empty)
                {
                    Utilities.SetDropDownValue(packagingItem.PackagingComponent, ddlPackagingComponent, this.Page);
                }

                DropDownList drpNew = ((DropDownList)e.Item.FindControl("drpNew"));
                if (packagingItem.NewExisting != null && packagingItem.NewExisting != string.Empty)
                {
                    Utilities.SetDropDownValue(packagingItem.NewExisting, drpNew, this.Page);
                }

                TextBox txtPackQty = (TextBox)e.Item.FindControl("txtPackQty");
                if (!string.IsNullOrEmpty(packagingItem.PackQuantity))
                {
                    var pacqty = (packagingItem.PackQuantity == "-9999" || string.IsNullOrEmpty(packagingItem.PackQuantity)) ? "0" : packagingItem.PackQuantity;
                    txtPackQty.Text = Math.Round(Convert.ToDouble(pacqty), 4).ToString();
                }

                DropDownList drpUnitOfMeasure = ((DropDownList)e.Item.FindControl("drpUnitOfMeasure"));
                Utilities.BindDropDownItems(drpUnitOfMeasure, GlobalConstants.LIST_PackUnitLookup, webUrl);
                if (packagingItem.PackUnit != null && packagingItem.PackUnit != string.Empty)
                {
                    Utilities.SetDropDownValue(packagingItem.PackUnit, drpUnitOfMeasure, this.Page);
                }

                DropDownList drpGraphicsNeeded = ((DropDownList)e.Item.FindControl("drpGraphicsNeeded"));
                if (!string.IsNullOrEmpty(packagingItem.GraphicsChangeRequired))
                {
                    Utilities.SetDropDownValue(packagingItem.GraphicsChangeRequired, drpGraphicsNeeded, this.Page);
                }

                DropDownList drpComponentContainsNLEA = ((DropDownList)e.Item.FindControl("drpComponentContainsNLEA"));
                if (packagingItem.ComponentContainsNLEA != null && packagingItem.ComponentContainsNLEA != string.Empty)
                {
                    Utilities.SetDropDownValue(packagingItem.ComponentContainsNLEA, drpComponentContainsNLEA, this.Page);
                }

                DropDownList ddlGraphicsVendor = ((DropDownList)e.Item.FindControl("ddlGraphicsVendor"));
                Utilities.BindDropDownItems(ddlGraphicsVendor, GlobalConstants.LIST_ExternalGraphicsVendorLookup, webUrl);
                if (!string.IsNullOrEmpty(packagingItem.ExternalGraphicsVendor))
                {
                    Utilities.SetDropDownValue(packagingItem.ExternalGraphicsVendor, ddlGraphicsVendor, this.Page);
                }
                HtmlControl spanGraphicsVendor = (HtmlControl)e.Item.FindControl("spanGraphicsVendor");
                if (!string.IsNullOrEmpty(packagingItem.GraphicsChangeRequired))
                {

                    if (packagingItem.GraphicsChangeRequired.ToLower() == "yes")
                    {
                        ddlGraphicsVendor.CssClass = "form-control drpGraphicsVendor PCBOMrequired";
                        spanGraphicsVendor.Attributes.Add("class", "markrequired spanGraphicsVendor showItempc");
                    }
                    else
                    {
                        ddlGraphicsVendor.CssClass = "form-control drpGraphicsVendor";
                        spanGraphicsVendor.Attributes.Add("class", "markrequired spanGraphicsVendor hideItem");
                    }
                }

                DropDownList ddlFlowthrough = ((DropDownList)e.Item.FindControl("ddlFlowthrough"));
                Utilities.BindDropDownItems(ddlFlowthrough, GlobalConstants.LIST_FlowThroughTypeLookup, webUrl);
                if (packagingItem.Flowthrough != null && packagingItem.Flowthrough != string.Empty)
                {
                    Utilities.SetDropDownValue(packagingItem.Flowthrough, ddlFlowthrough, this.Page);
                }

                TextBox txtMaterial = (TextBox)e.Item.FindControl("txtMaterial");
                TextBox txtMaterialDesc = (TextBox)e.Item.FindControl("txtMaterialDesc");

                TextBox txtLikeMaterialDesc = (TextBox)e.Item.FindControl("txtLikeMaterialDesc");
                TextBox txtLikeMaterial = (TextBox)e.Item.FindControl("txtLikeMaterial");

                TextBox txtOldMaterialDesc = (TextBox)e.Item.FindControl("txtOldMaterialDesc");
                TextBox txtOldMaterial = (TextBox)e.Item.FindControl("txtOldMaterial");
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

                DropDownList ddlProductHierarchyLevel1 = ((DropDownList)e.Item.FindControl("ddlPHL1"));
                Utilities.BindDropDownItemsPHL1(ddlProductHierarchyLevel1, GlobalConstants.LIST_ProductHierarchyLevel1Lookup, webUrl);
                Utilities.SetDropDownValue(packagingItem.PHL1, ddlProductHierarchyLevel1, this.Page);

                DropDownList ddlProductHierarchyLevel2 = ((DropDownList)e.Item.FindControl("ddlPHL2"));
                Utilities.BindDropDownItemsPHL2(ddlProductHierarchyLevel2, GlobalConstants.LIST_ProductHierarchyLevel2Lookup, webUrl);
                Utilities.SetDropDownValue(packagingItem.PHL2, ddlProductHierarchyLevel2, this.Page);

                DropDownList ddlBrand_Material = ((DropDownList)e.Item.FindControl("ddlBrand"));
                Utilities.BindDropDownItemsBrand(ddlBrand_Material, GlobalConstants.LIST_MaterialGroup1Lookup, webUrl);
                Utilities.SetDropDownValue(packagingItem.Brand, ddlBrand_Material, this.Page);

                TextBox txtProfitCenter = (TextBox)e.Item.FindControl("txtProfitCenterUC");
                txtProfitCenter.Text = packagingItem.ProfitCenter;
                HiddenField hdnProfitCenterUC = (HiddenField)e.Item.FindControl("hdnProfitCenterUC");
                hdnProfitCenterUC.Value = packagingItem.ProfitCenter;

                HtmlControl spanWhyComponent = (HtmlControl)e.Item.FindControl("spanWhyComponent");
                TextBox txtLikeReason = (TextBox)e.Item.FindControl("txtLikeReason");
                txtLikeReason.Text = string.IsNullOrEmpty(packagingItem.CurrentLikeItemReason) ? "" : packagingItem.CurrentLikeItemReason;

                TextBox txtGraphicsBrief = (TextBox)e.Item.FindControl("txtGraphicsBrief");
                HtmlControl spanGraphicsBrief = (HtmlControl)e.Item.FindControl("spanGraphicsBrief");

                if (!string.IsNullOrEmpty(packagingItem.GraphicsChangeRequired))
                {
                    if (packagingItem.GraphicsChangeRequired.ToLower() == "yes")
                    {
                        txtGraphicsBrief.CssClass = "form-control GraphicsBrief PCBOMrequired";
                        spanGraphicsBrief.Attributes.Add("class", "markrequired spanGraphicsBrief showItempc");
                    }
                    else
                    {
                        spanGraphicsBrief.Attributes.Add("class", "markrequired spanGraphicsBrief hideItem");
                        txtGraphicsBrief.CssClass = "form-control GraphicsBrief";
                    }

                }

                DropDownList ddlUPCAssociated = ((DropDownList)e.Item.FindControl("ddlUPCAssociated"));

                UPCAssociated.Clear();
                BEQRCItem beQRCitem = BEQRCService.GetBEQRCItem(iItemId);
                if (!string.IsNullOrEmpty(beQRCitem.UnitUPC))
                {
                    UPCAssociated.Add(beQRCitem.UnitUPC);
                }

                if (!string.IsNullOrEmpty(beQRCitem.DisplayBoxUPC))
                {
                    UPCAssociated.Add(beQRCitem.DisplayBoxUPC);
                }

                UPCAssociated.Add("NA");
                UPCAssociated.Add("Manual Entry");

                ddlUPCAssociated.Items.Clear();
                ddlUPCAssociated.ClearSelection();
                ddlUPCAssociated.Items.Add(new ListItem("Select...", "-1"));
                int value = 0;
                foreach (var item in UPCAssociated)
                {
                    ddlUPCAssociated.Items.Add(new ListItem(item, value.ToString()));
                    value++;
                }

                Utilities.SetDropDownValue(packagingItem.UPCAssociated, ddlUPCAssociated, this.Page);

                TextBox txtUPCAssociated = (TextBox)e.Item.FindControl("txtUPCAssociated");
                txtUPCAssociated.Text = string.IsNullOrEmpty(packagingItem.UPCAssociatedManualEntry) ? "" : packagingItem.UPCAssociatedManualEntry;

                DropDownList ddlBioEngLabelingRequired = ((DropDownList)e.Item.FindControl("ddlBioEngLabelingRequired"));
                Utilities.BindDropDownItemsPHL1(ddlBioEngLabelingRequired, GlobalConstants.LIST_BioEngineeringLabelingRequiredLookup, webUrl);
                Utilities.SetDropDownValue(packagingItem.BioEngLabelingRequired, ddlBioEngLabelingRequired, this.Page);

                TextBox txtFlowthroughMaterialsSpecs = (TextBox)e.Item.FindControl("txtFlowthroughMaterialsSpecs");
                txtFlowthroughMaterialsSpecs.Text = packagingItem.FlowthroughMaterialsSpecs;


                #region Hidden Fields
                HiddenField hdnComponentType = (HiddenField)e.Item.FindControl("hdnComponentType");
                hdnComponentType.Value = ParentComponentType;
                HiddenField hdnDeletedStatus = (HiddenField)e.Item.FindControl("hdnDeletedStatus");
                hdnDeletedStatus.Value = packagingItem.Deleted;
                HiddenField hdnParentId = (HiddenField)e.Item.FindControl("hdnParentId");
                hdnParentId.Value = packagingItem.ParentID.ToString();
                #endregion


                List<KeyValuePair<int, string>> allItems = new List<KeyValuePair<int, string>>();
                List<int> idsOnly = new List<int>();
                DropDownList ddlMoveTS = ((DropDownList)e.Item.FindControl("ddlMoveTS"));
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

                if (packagingItem.PackagingComponent != null && packagingItem.PackagingComponent != string.Empty)
                {
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

                #region BE QR Code EPS File
                HtmlImage btnBEQRCodeEPSFile = (HtmlImage)e.Item.FindControl("btnBEQRCodeEPSFile");

                var BEQRCodeEPSFiles =
                    (
                        from
                            attachment in projectAttachments
                        where
                            attachment.PackagingComponentItemId == packagingItem.Id
                            &&
                            attachment.DocType == GlobalConstants.DOCTYPE_BEQRCodeEPSFile
                        select
                            attachment
                      ).ToList();

                if (BEQRCodeEPSFiles.Count > 0)
                {
                    ImageButton btnDeleteBEQRCodeEPSFile = (ImageButton)e.Item.FindControl("btnDeleteBEQRCodeEPSFile");
                    HtmlAnchor ancBEQRCodeEPSFile = ((HtmlAnchor)e.Item.FindControl("ancRenderingBEQRCodeEPSFile"));
                    HiddenField DeletedBEQRCEPSFileUrl = ((HiddenField)e.Item.FindControl("DeletedBEQRCEPSFileUrl"));

                    btnDeleteBEQRCodeEPSFile.Visible = true;
                    if (ancBEQRCodeEPSFile != null)
                    {
                        string fileName = BEQRCodeEPSFiles[0].FileName;
                        fileName = fileName.Replace("_", " ");
                        ancBEQRCodeEPSFile.Controls.Add(new LiteralControl(fileName));
                        ancBEQRCodeEPSFile.HRef = BEQRCodeEPSFiles[0].FileUrl;
                        btnDeleteBEQRCodeEPSFile.CommandArgument = BEQRCodeEPSFiles[0].FileUrl;
                        DeletedBEQRCEPSFileUrl.Value = BEQRCodeEPSFiles[0].FileUrl; ;
                    }

                    btnBEQRCodeEPSFile.Visible = false;
                }
                #endregion

                if (packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi || packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                {
                    ucBEQRC ctrl2 = (ucBEQRC)Page.LoadControl(_ucBEQRCPath);
                    ctrl2.ParentId = packagingItem.Id;
                    ctrl2.MaterialNumber = packagingItem.MaterialNumber;
                    ctrl2.MaterialDesc = packagingItem.MaterialDescription;
                    ctrl2.ProjectNumber = ProjectNumber;
                    ctrl2.CompassItemId = iItemId;
                    ctrl2.AllPIs = AllPIs;
                    ctrl2.ParentComponentType = packagingItem.PackagingComponent;
                    ctrl2.projectAttachments = projectAttachments;
                    ctrl2.firstLoad = "true";
                    ctrl2.UPCAssociated = UPCAssociated;
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
        }
        #endregion
        #region Data Transfer Methods
        private void LoadBOMItems()
        {
            List<PackagingItem> dtPackingItem;
            if (ParentId == 0)
            {
                dtPackingItem = (from PIs in AllPIs where PIs.ParentID == 0 select PIs).ToList();
            }
            else
            {
                dtPackingItem = (from PIs in AllPIs where PIs.ParentID == ParentId select PIs).ToList();
            }

            btnAddNewPackagingItem.CommandArgument = ParentId.ToString();

            if (dtPackingItem.Count > 0)
            {
                rptTSItem.DataSource = dtPackingItem;
                rptTSItem.DataBind();
            }
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

            RefreshPage();
        }

        private void RefreshPage()
        {
            int tempParentId = Convert.ToInt32(btnAddNewPackagingItem.CommandArgument.ToString());
            AllPIs = packagingItemService.GetAllPackagingItemsForProject(iItemId);
            projectAttachments = packagingItemService.GetRenderingUploadedFiles(ProjectNumber);
            projectAttachments.AddRange(packagingItemService.GetApprovedGraphicsAssetUploadedFiles(ProjectNumber));
            projectAttachments.AddRange(BEQRCService.GetBEQRCodeEPSFileUploadedFiles(ProjectNumber));

            ucBEQRC ctrl = (ucBEQRC)this.Parent.Page.LoadControl(_ucBEQRCPath);
            ctrl.ParentId = 0;
            ctrl.MaterialNumber = "";
            ctrl.MaterialDesc = "";
            ctrl.ParentComponentType = "";
            ctrl.ProjectNumber = ProjectNumber;
            ctrl.AllPIs = AllPIs;
            ctrl.CompassItemId = iItemId;
            ctrl.firstLoad = "true";
            ctrl.ID = "FinishedGood";
            ctrl.UPCAssociated = UPCAssociated;
            ctrl.projectAttachments = projectAttachments;

            var activeTab = (HiddenField)this.Parent.FindControl("activeTabHolder");

            PlaceHolder phFG = (PlaceHolder)this.Parent.FindControl("phFG");
            PlaceHolder phTS = (PlaceHolder)this.Parent.FindControl("phTS");

            phFG.Controls.Clear();
            phTS.Controls.Clear();
            phFG.Controls.Add(ctrl);
            activeTab.Value = tempParentId.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "drpCompType_load", "drpCompType_load();", true);
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
                    var type = (ucBEQRC)uc;
                    Repeater repeater = type.rptTSItem;
                    foreach (RepeaterItem item in repeater.Items)
                    {
                        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                        {
                            PackagingItem packagingItem = new PackagingItem();

                            packagingItem.PackagingComponent = ((DropDownList)item.FindControl("drpComponent")).SelectedItem.Text;
                            packagingItem.NewExisting = ((DropDownList)item.FindControl("drpNew")).SelectedItem.Text;
                            packagingItem.PackQuantity = ((TextBox)item.FindControl("txtPackQty")).Text;
                            packagingItem.PackUnit = ((DropDownList)item.FindControl("drpUnitOfMeasure")).SelectedItem.Text;
                            packagingItem.GraphicsChangeRequired = ((DropDownList)item.FindControl("drpGraphicsNeeded")).SelectedItem.Text;
                            packagingItem.ComponentContainsNLEA = ((DropDownList)item.FindControl("drpComponentContainsNLEA")).SelectedItem.Text;
                            packagingItem.ExternalGraphicsVendor = ((DropDownList)item.FindControl("ddlGraphicsVendor")).SelectedItem.Text;
                            packagingItem.Flowthrough = ((DropDownList)item.FindControl("ddlFlowthrough")).SelectedItem.Text;
                            packagingItem.MaterialNumber = ((TextBox)item.FindControl("txtMaterial")).Text;
                            packagingItem.MaterialDescription = ((TextBox)item.FindControl("txtMaterialDesc")).Text;
                            packagingItem.CurrentLikeItem = ((TextBox)item.FindControl("txtLikeMaterial")).Text;
                            packagingItem.CurrentLikeItemDescription = ((TextBox)item.FindControl("txtLikeMaterialDesc")).Text;
                            packagingItem.CurrentOldItem = ((TextBox)item.FindControl("txtOldMaterial")).Text;
                            packagingItem.CurrentOldItemDescription = ((TextBox)item.FindControl("txtOldMaterialDesc")).Text;
                            if (packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi || packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi)
                            {
                                packagingItem.PHL1 = ((DropDownList)item.FindControl("ddlPHL1")).SelectedItem.Text;
                                packagingItem.PHL2 = ((DropDownList)item.FindControl("ddlPHL2")).SelectedItem.Text;
                                packagingItem.Brand = ((DropDownList)item.FindControl("ddlBrand")).SelectedItem.Text;
                                packagingItem.ProfitCenter = ((TextBox)item.FindControl("txtProfitCenterUC")).Text;
                                packagingItem.ProfitCenter = ((HiddenField)item.FindControl("hdnProfitCenterUC")).Value;
                            }
                            packagingItem.CurrentLikeItemReason = ((TextBox)item.FindControl("txtLikeReason")).Text;
                            packagingItem.GraphicsBrief = ((TextBox)item.FindControl("txtGraphicsBrief")).Text;
                            packagingItem.UPCAssociated = ((DropDownList)item.FindControl("ddlUPCAssociated")).SelectedItem.Text;
                            packagingItem.UPCAssociatedManualEntry = ((TextBox)item.FindControl("txtUPCAssociated")).Text;
                            packagingItem.BioEngLabelingRequired = ((DropDownList)item.FindControl("ddlBioEngLabelingRequired")).SelectedItem.Text;
                            packagingItem.FlowthroughMaterialsSpecs = ((TextBox)item.FindControl("txtFlowthroughMaterialsSpecs")).Text;

                            var CompId = ((HiddenField)item.FindControl("hdnItemID")).Value.Replace(",", "");
                            packagingItem.Id = string.IsNullOrEmpty(CompId) ? 0 : Convert.ToInt32(CompId);
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
                BEQRCService.UpdateBEQRCPackagingItems(PIsToSave, iItemId);
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

    }
}
