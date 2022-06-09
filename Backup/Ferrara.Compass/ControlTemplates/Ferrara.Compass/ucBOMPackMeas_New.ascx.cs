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
    public partial class ucBOMPackMeas_New : UserControl
    {
        #region Member Variables
        private IBOMSetupService bomsetupService;
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private string webUrl;
        private int iItemId = 0;
        private List<PackagingItem> packagingItems = new List<PackagingItem>();
        private const string _ascxPath = @"~/_controltemplates/15/Ferrara.Compass/ucBOMEditable_New.ascx";
        PlaceHolder phUC;
        #endregion

        #region Properties
        public int ParentId { get; set; }
        public int CompassItemId { get; set; }
        public string PackagingComponent { get; set; }
        public string MaterialNumber { get; set; }
        public string MaterialDesc { get; set; }
        public string SemiXferMakeLocation { get; set; }
        public string NewExisting { get; set; }
        public int NewComponentCount { get; set; }
        public string MakePackTransferLocation { get; set; }

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

            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            bomsetupService = DependencyResolution.DependencyMapper.Container.Resolve<IBOMSetupService>();
            EnsureScriptManager();
            EnsureUpdatePanelFixups();
        }
        protected void Page_Load(object sender, EventArgs e)
        {


            webUrl = SPContext.Current.Web.Url;
            iItemId = Utilities.GetItemIdFromProjectNumber(ProjectNumber);
            this.hiddenItemId.Value = iItemId.ToString();
            // phUC = (PlaceHolder)this.Parent.FindControl("phMsg");
            hdnUCBOMComponentType.Value = PackagingComponent;

            if (!Page.IsPostBack)
            {
                LoadFormData();
                LoadBOMItems();
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

        #region Data Transfer Methods
        private void LoadBOMItems()
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
        private void LoadFormData()
        {
            BOMSetupItem bomsetupitem = bomsetupService.GetPackMeasurementsItem(iItemId, ParentId);
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
        private BOMSetupItem ConstructFormData(ucBOMPackMeas_New ctrl)
        {
            var bomsetupitem = new BOMSetupItem();
            try
            {

                bomsetupitem.CompassListItemId = iItemId;
                bomsetupitem.ParentID = Convert.ToInt32(ctrl.hdnParentComponentId.Value);
                bomsetupitem.PalletSpecNumber = ctrl.txtPalletSpecNumber.Text;
                bomsetupitem.PackSpecNumber = ctrl.txtFGPackSpecNumber.Text;
                bomsetupitem.PalletSpecLink = ctrl.txPalletPatternLink.Text;
                bomsetupitem.NotesSpec = ctrl.txtSpecNotes.Text;
                bomsetupitem.SAPSpecsChange = ctrl.drpSAPSpecsChange.SelectedItem.Text;
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Packaging Component: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "PackagingComponentEntryForm", "ConstructFormData");
            }
            return bomsetupitem;
        }
        #endregion

        #region Button Methods

        public void saveData()
        {
            var phBOMGrid = (PlaceHolder)this.Parent.FindControl("phBOMGrid");
            foreach (UserControl ctrl in phBOMGrid.Controls)
            {
                if (ctrl is ucBOMPackMeas_New)
                {
                    var type = (ucBOMPackMeas_New)ctrl;
                    var item = ConstructFormData(type);
                    if (type.iItemId > 0)
                    {
                        int ctrlParentId = Convert.ToInt32(type.hdnParentComponentId.Value);
                        if (ctrlParentId > 0)
                        {
                            bomsetupService.UpdateTransferSemiMakePackLocations(ctrlParentId, type.lblTransferLocation.Text);
                        }
                        bomsetupService.UpsertPackMeasurementsItem(item, ProjectNumber);
                    }
                }
            }
        }

        #endregion
    }
}
