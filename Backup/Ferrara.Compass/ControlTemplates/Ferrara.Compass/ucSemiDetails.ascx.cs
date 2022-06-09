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
    public partial class ucSemiDetails : UserControl
    {
        #region Member Variables
        private IPackagingItemService packagingItemService;
        private IExceptionService exceptionService;
        private string webUrl;
        private int iItemId = 0;
        private List<PackagingItem> packagingItems = new List<PackagingItem>();
        private List<ShipperFinishedGoodItem> shipperFGItems = new List<ShipperFinishedGoodItem>();
        #endregion

        #region Properties
        public int CompassItemId { get; set; }
        public string ComponentType { get; set; }
        public string ProjectNumber { get; set; }


        #endregion
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeScreen();
            // Page.EnableEventValidation = false;

            packagingItemService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            EnsureScriptManager();
            EnsureUpdatePanelFixups();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            webUrl = SPContext.Current.Web.Url;
            iItemId = CompassItemId;
            btnAddnew.Text = "Add New " + ComponentType;
            btnAddnew.CommandArgument = ComponentType;//ParentId.ToString();
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PCSRequirements", "PCSRequirements();", true);

        }
        private void InitializeScreen()
        {

        }

        #endregion
        #region Repeater Events
        protected void rptSemis_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                PackagingItem packagingItem = (PackagingItem)e.Item.DataItem;

                DropDownList drpNew = ((DropDownList)e.Item.FindControl("drpNewExisting"));

                if (packagingItem.NewExisting != null && packagingItem.NewExisting != string.Empty)
                {
                    Utilities.SetDropDownValue(packagingItem.NewExisting, drpNew, this.Page);
                }
                DropDownList ddlPackagingComponent = ((DropDownList)e.Item.FindControl("drpComponentType"));
                if (packagingItem.PackagingComponent != null && packagingItem.PackagingComponent != string.Empty)
                {
                    Utilities.SetDropDownValue(packagingItem.PackagingComponent, ddlPackagingComponent, this.Page);
                }
                HiddenField hdnComponentType = (HiddenField)e.Item.FindControl("hdnComponentType");
                hdnComponentType.Value = packagingItem.PackagingComponent;
                TextBox txtMaterial = (TextBox)e.Item.FindControl("txtMaterialNumber");
                txtMaterial.Text = packagingItem.MaterialNumber;
                TextBox txtMaterialDesc = (TextBox)e.Item.FindControl("txtMaterialDescription");
                txtMaterialDesc.Text = packagingItem.MaterialDescription;
                TextBox txtLikeMaterial = (TextBox)e.Item.FindControl("txtLikeMaterial");
                txtLikeMaterial.Text = packagingItem.CurrentLikeItem;
                TextBox txtLikeMaterialDesc = (TextBox)e.Item.FindControl("txtLikeMaterialDesc");
                txtLikeMaterialDesc.Text = packagingItem.CurrentLikeItemDescription;

                DropDownList drpTSCountryOfOrigin = ((DropDownList)e.Item.FindControl("drpTSCountryOfOrigin"));
                Utilities.BindDropDownItems(drpTSCountryOfOrigin, GlobalConstants.LIST_CountryOfOriginLookup, webUrl);
                Utilities.SetDropDownValue(packagingItem.CountryOfOrigin, drpTSCountryOfOrigin, this.Page);

                DropDownList drpXferLocation = ((DropDownList)e.Item.FindControl("drpXferLocation"));
                DropDownList drpTSPackLocation = ((DropDownList)e.Item.FindControl("drpTSPackLocation"));
                DropDownList ddlImmediateSPKChange = ((DropDownList)e.Item.FindControl("ddlImmediateSPKChange"));

                if (ComponentType == GlobalConstants.COMPONENTTYPE_TransferSemi)
                {
                    drpNew.Attributes.Add("onchange", "IsInternalSemiRequired(this);");
                    Utilities.BindDropDownWithTitleFilter(drpXferLocation, GlobalConstants.LIST_MakePackTransfersLookup, webUrl, false, "PURCHASED");
                    Utilities.BindDropDownWithTitleFilter(drpTSPackLocation, GlobalConstants.LIST_PackingLocationsLookup, webUrl, false, "External");
                }
                else if (ComponentType == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                {
                    drpNew.Attributes.Add("onchange", "OnChangeNewExistingForPCs(this);");
                    Utilities.BindDropDownWithTitleFilter(drpXferLocation, GlobalConstants.LIST_MakePackTransfersLookup, webUrl, true, "PURCHASED");
                    Utilities.BindDropDownItems(drpTSPackLocation, GlobalConstants.LIST_CoPackers, webUrl);
                }
                Utilities.SetDropDownValue(packagingItem.TransferSEMIMakePackLocations, drpXferLocation, this.Page);
                Utilities.SetDropDownValue(packagingItem.PackLocation, drpTSPackLocation, this.Page);
                Utilities.SetDropDownValue(packagingItem.ImmediateSPKChange, ddlImmediateSPKChange, this.Page);
            }
        }
        protected void rptSemis_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToLower() == "delete")
            {
                int id = Convert.ToInt32(e.CommandArgument.ToString());

                var Childlst = new List<PackagingItem>();
                if (ComponentType == GlobalConstants.COMPONENTTYPE_TransferSemi || ComponentType == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                {
                    Childlst = packagingItemService.GetPackagingChildren(id);
                }

                if (Childlst.Count > 0)
                {
                    Panel lblCompDeleteError = ((Panel)e.Item.FindControl("lblCompDeleteError"));
                    lblCompDeleteError.Visible = true;
                    if (e.Item.ItemIndex % 2 == 0)
                    {
                        lblCompDeleteError.CssClass = "row";
                    }
                    else
                    {
                        lblCompDeleteError.CssClass = "row blueRow";
                    }
                }
                else
                {
                    List<PackagingItem> lst = packagingItemService.GetTransferPurchasedSemiItemsForProject(iItemId, ComponentType);
                    var l = lst.FindIndex(r => r.Id == id);
                    lst.RemoveAt(l);

                    if (id > 0)
                        packagingItemService.DeletePackagingItem(id);

                    rptSemis.DataSource = lst;
                    rptSemis.DataBind();

                    if (lst.Count == 0)
                    {
                        LoadBOMItems();
                    }
                }
            }
        }
        #endregion

        #region Data Transfer Methods

        private void LoadBOMItems()
        {
            List<PackagingItem> dtPackingItem = new List<PackagingItem>();
            if (iItemId > 0)
            {
                dtPackingItem = packagingItemService.GetTransferPurchasedSemiItemsForProject(iItemId, ComponentType);
            }
            /*foreach (RepeaterItem item in rptSemis.Items)
            {
                //if (((DropDownList)item.FindControl("drpNewExisting")).SelectedValue == "-1" && ((DropDownList)item.FindControl("drpXferLocation")).SelectedValue == "-1" && ((HtmlInputControl)item.FindControl("txtMaterialNumber")).Value == "" && ((HtmlInputControl)item.FindControl("txtMaterialDescription")).Value == "" && ((HtmlInputControl)item.FindControl("txtSEMIComment")).Value == "" && ((DropDownList)item.FindControl("drpSharedTS")).SelectedValue == "-1")
                //{
                    Button deletebutton = (Button)item.FindControl("btndelete");
                    int id = Convert.ToInt32(deletebutton.CommandArgument);

                    var l = dtPackingItem.FindIndex(r => r.Id == id);
                    if (l > 0)
                        dtPackingItem.RemoveAt(l);

                    if (id > 0)
                        packagingItemService.DeletePackagingItem(id);
                //}
            }*/


            if (dtPackingItem.Count > 0)
            {
                rptSemis.DataSource = dtPackingItem;
                rptSemis.DataBind();
            }
        }
        #endregion

        #region Button Methods     
        public void saveData()
        {
            List<PlaceHolder> SemiUC = new List<PlaceHolder>();
            PlaceHolder phTS = (PlaceHolder)this.Parent.FindControl("phTransferSemiFields");
            PlaceHolder phPS = (PlaceHolder)this.Parent.FindControl("phPurchasedSemiFields");
            if (phTS != null)
            {
                SemiUC.Add(phTS);
            }
            if (phPS != null)
            {
                SemiUC.Add(phPS);
            }
            foreach (PlaceHolder phUC in SemiUC)
            {
                foreach (UserControl uc in phUC.Controls)
                {
                    var type = (ucSemiDetails)uc;
                    Repeater repeater = type.rptSemis;
                    foreach (RepeaterItem item in repeater.Items)
                    {
                        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                        {
                            PackagingItem packagingItem = new PackagingItem();

                            packagingItem.PackagingComponent = ((DropDownList)item.FindControl("drpComponentType")).SelectedItem.Text;
                            packagingItem.NewExisting = ((DropDownList)item.FindControl("drpNewExisting")).SelectedItem.Text;
                            packagingItem.MaterialNumber = ((TextBox)item.FindControl("txtMaterialNumber")).Text;
                            packagingItem.MaterialDescription = ((TextBox)item.FindControl("txtMaterialDescription")).Text;
                            packagingItem.CurrentLikeItem = ((TextBox)item.FindControl("txtLikeMaterial")).Text;
                            packagingItem.CurrentLikeItemDescription = ((TextBox)item.FindControl("txtLikeMaterialDesc")).Text;
                            packagingItem.TransferSEMIMakePackLocations = ((DropDownList)item.FindControl("drpXferLocation")).SelectedItem.Text;
                            packagingItem.CountryOfOrigin = ((DropDownList)item.FindControl("drpTSCountryOfOrigin")).SelectedItem.Text;
                            packagingItem.PackLocation = ((DropDownList)item.FindControl("drpTSPackLocation")).SelectedItem.Text;
                            packagingItem.ImmediateSPKChange = ((DropDownList)item.FindControl("ddlImmediateSPKChange")).SelectedItem.Text;
                            packagingItem.Notes = ((HtmlInputControl)item.FindControl("txtSEMIComment")).Value;
                            packagingItem.CompassListItemId = iItemId.ToString();
                            packagingItem.Id = Convert.ToInt32(((HiddenField)item.FindControl("hdnItemID")).Value);

                            if (packagingItem.Id <= 0)
                            {
                                int newId = packagingItemService.InsertPackagingItem(packagingItem, iItemId);
                                HiddenField hidId = ((HiddenField)item.FindControl("hdnItemID"));
                                if (hidId != null)
                                    hidId.Value = newId.ToString();
                            }
                            else
                            {
                                packagingItemService.UpdateOPSPackagingItem(packagingItem);
                            }
                        }
                    }
                }
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
        protected void btnAddnew_Click(object sender, EventArgs e)
        {

            saveData();

            ucSemiDetails ctrl = (ucSemiDetails)this.Parent.Page.LoadControl(@"~/_controltemplates/15/Ferrara.Compass/ucSemiDetails.ascx");
            ctrl.ComponentType = GlobalConstants.COMPONENTTYPE_TransferSemi;
            ctrl.ProjectNumber = ProjectNumber;
            ctrl.CompassItemId = iItemId;
            ctrl.ID = "TransferSemi";

            ucSemiDetails ctrl2 = (ucSemiDetails)this.Parent.Page.LoadControl(@"~/_controltemplates/15/Ferrara.Compass/ucSemiDetails.ascx");
            ctrl2.ComponentType = GlobalConstants.COMPONENTTYPE_PurchasedSemi;
            ctrl2.ProjectNumber = ProjectNumber;
            ctrl2.CompassItemId = iItemId;
            ctrl2.ID = "PurchasedCandy";

            PlaceHolder phTransferSemiFields = (PlaceHolder)this.Parent.FindControl("phTransferSemiFields");
            PlaceHolder phPurchasedSemiFields = (PlaceHolder)this.Parent.FindControl("phPurchasedSemiFields");


            var obj = new PackagingItem();

            obj.ParentID = 0;
            obj.CompassListItemId = iItemId.ToString();
            obj.PackagingComponent = btnAddnew.CommandArgument.ToString();
            int id = packagingItemService.InsertPackagingItem(obj, iItemId);
            if (phTransferSemiFields != null)
            {
                phTransferSemiFields.Controls.Clear();
                phTransferSemiFields.Controls.Add(ctrl);
            }
            if (phPurchasedSemiFields != null)
            {
                phPurchasedSemiFields.Controls.Clear();
                phPurchasedSemiFields.Controls.Add(ctrl2);
            }

        }
        #endregion
    }
}
