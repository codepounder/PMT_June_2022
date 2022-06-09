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
using System.Web;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using Ferrara.Compass.ControlTemplates.Ferrara.Compass;
using System.Web.UI.WebControls;

namespace Ferrara.Compass.WebParts.CompassWorldSyncMainForm
{
    [ToolboxItemAttribute(false)]
    public partial class CompassWorldSyncMainForm : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

        #region Member Variables
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private IUserManagementService userManagementService;
        private INotificationService notificationService;
        private IConfigurationManagementService configurationService;
        private ICompassWorldSyncService worldSyncService;
        private IWorldSyncNutritionalService nutritionalService;
        private string webUrl;
        private int iItemId = 0;
        private bool showLabel = true;
        private const string _ucWSChildPath = @"~/_controltemplates/15/Ferrara.Compass/ucWSChild.ascx";

        #region Member constants
        private const string hddNutritionalDetailId = "hddNutritionalDetailId";
        private const string cmbNutrientType = "cmbNutrientType";
        private const string txtNutrientQuantity = "txtNutrientQuantity";
        private const string cmbNutrientQuantityType = "cmbNutrientQuantityType";
        private const string txtPercentageDailyValue = "txtPercentageDailyValue";
        private const string trAddNutritional = "trAddNutritional";
        private const string lstTradeChannelSelected = "lstTradeChannelSelected";
        private const string lstTradeChannelAvailable = "lstTradeChannelAvailable";
        #endregion

        #region Properties
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
            InitializeControl();

            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            userManagementService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
            configurationService = DependencyResolution.DependencyMapper.Container.Resolve<IConfigurationManagementService>();
            worldSyncService = DependencyResolution.DependencyMapper.Container.Resolve<ICompassWorldSyncService>();
            nutritionalService = DependencyResolution.DependencyMapper.Container.Resolve<IWorldSyncNutritionalService>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Form.Enctype = "multipart/form-data";
            webUrl = SPContext.Current.Web.Url;
            
            if (!Page.IsPostBack)
            {
                try
                {
                    if (!CheckProjectNumber())
                        return;
                    Utilities.BindDropDownItems(ddlProductType, GlobalConstants.LIST_ProductTypeLookup, SPContext.Current.Web.Url);
                    Utilities.BindDropDownItems(ddlBrandOwnerGLN, GlobalConstants.LIST_BrandOwnerGLNlookup, SPContext.Current.Web.Url);
                    Utilities.BindDropDownItems(ddlGS1TradeItemsIDkeyCode, GlobalConstants.LIST_GS1tradeKeyCodeLookup, SPContext.Current.Web.Url);
                    Utilities.BindDropDownItems(ddlAlternateClassificationScheme, GlobalConstants.LIST_AlternateClassificationSchemeLookup, SPContext.Current.Web.Url);
                    Utilities.BindDropDownItems(ddlDataCarrierTypeCode, GlobalConstants.LIST_DataCarrierTypeCodeLookup, SPContext.Current.Web.Url);
                    Utilities.BindListBoxItems(lbChannelAvailable, GlobalConstants.LIST_TradeChannelLookup, SPContext.Current.Web.Url);

                }
                catch (Exception exception)
                {
                    ErrorSummary.AddError(exception.Message, this.Page);
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.Distribution.ToString() + ": " + exception.Message);
                    exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.Distribution.ToString(), "Page_Load");
                }
                LoadFormData();
                LoadWSChildItems();
            }
            else
            {
                iItemId = Convert.ToInt32(hiddenItemId.Value);
            }
        }
        private void LoadWSChildItems()
        {
            List<CompassWorldSyncListItem> WSChildItems = new List<CompassWorldSyncListItem>();
            WSChildItems = worldSyncService.GetCompassWorldSyncChildListItems(iItemId);
            
            rptWSChildItem.DataSource = WSChildItems;
            rptWSChildItem.DataBind();

        }
        private void LoadFormData()
        {
            int childId = Convert.ToInt32(hddWorldSyncChildId.Value);
            
            if (childId != -1)
            {
                CompassWorldSyncListItem worldSync = worldSyncService.GetCompassWorldSyncListItem(iItemId, childId);
                if (hdnPageState.Value != "child")
                {
                    Utilities.SetDropDownValue(worldSync.AlternateClassificationScheme, ddlAlternateClassificationScheme, this.Page);
                    txtCode.Text = worldSync.Code;
                    txtAlternateItemIdAgency.Text = worldSync.AlternateItemIdAgency;
                    txtTemperatureQualitiferCode.Text = worldSync.TemperatureQualitiferCode;
                    txtQtyNextLevelItems.Text = string.IsNullOrEmpty(worldSync.QtyOfNextLevelItems) ? "1 EACH" : worldSync.QtyOfNextLevelItems;
                    hddWorldSyncGlobalId.Value = worldSync.Id.ToString();
                    hddWorldSyncChildId.Value = "0";
                    hdnSelectedTradeChannel.Value = worldSync.TradeChannel;
                    pageHead.InnerText = "World Synchronization Global Information";
                }
                else
                {
                    pageHead.InnerText = "World Synchronization Child Information";
                    txtQtyNextLevelItems.Text = string.IsNullOrEmpty(worldSync.TargetMarket) ? "1" : worldSync.TargetMarket;
                    hddWorldSyncChildId.Value = worldSync.Id.ToString();
                    hddWorldSyncGlobalId.Value = worldSync.ParentId.ToString();
                }
                Utilities.SetDropDownValue(worldSync.ProductType, ddlProductType, this.Page);
                Utilities.SetDropDownValue(worldSync.BrandOwnerGLN, ddlBrandOwnerGLN, this.Page);
                Utilities.SetDropDownValue(worldSync.GS1TradeItemsIDKeyCode, ddlGS1TradeItemsIDkeyCode, this.Page);
                Utilities.SetDropDownValue(worldSync.DataCarrierTypeCode, ddlDataCarrierTypeCode, this.Page);
                Utilities.SetDropDownValue(worldSync.BaseUnitIndicator, ddlBaseUnitIndicator, this.Page);
                Utilities.SetDropDownValue(worldSync.ConsumerUnitIndicator, ddlConsumerUnitIndicator, this.Page);
                Utilities.SetDropDownValue(worldSync.OrderingUnitIndicator, ddlOrderingUnitIndicator, this.Page);
                Utilities.SetDropDownValue(worldSync.DispatchUnitIndicator, ddlDispatchUnitIndicator, this.Page);
                Utilities.SetDropDownValue(worldSync.InvoiceUnitIndicator, ddlInvoiceUnitIndicator, this.Page);

                txtNetContent.Text = worldSync.NetContent;
                txtTargetMarket.Text = string.IsNullOrEmpty(worldSync.TargetMarket) ? "United States" : worldSync.TargetMarket;
                txtCustomerBrandName.Text = worldSync.CustomerBrandName;
            }
            if(hdnPageState.Value == "child")
            {
                dvBtnCancel.Visible = true;
                dvBtnSubmit.Visible = false;
            }
            else
            {
                dvBtnCancel.Visible = false;
                dvBtnSubmit.Visible = true;
            }

        }
        private void loadNewChildData()
        {
            Utilities.SetDropDownValue("Select...", ddlAlternateClassificationScheme, this.Page);
            Utilities.SetDropDownValue("Select...", ddlProductType, this.Page);
            Utilities.SetDropDownValue("Select...", ddlBrandOwnerGLN, this.Page);
            Utilities.SetDropDownValue("Select...", ddlGS1TradeItemsIDkeyCode, this.Page);
            Utilities.SetDropDownValue("Select...", ddlDataCarrierTypeCode, this.Page);
            Utilities.SetDropDownValue("Select...", ddlBaseUnitIndicator, this.Page);
            Utilities.SetDropDownValue("Select...", ddlConsumerUnitIndicator, this.Page);
            Utilities.SetDropDownValue("Select...", ddlOrderingUnitIndicator, this.Page);
            Utilities.SetDropDownValue("Select...", ddlDispatchUnitIndicator, this.Page);
            Utilities.SetDropDownValue("Select...", ddlInvoiceUnitIndicator, this.Page);
            pageHead.InnerText = "World Synchronization Child Information";
            txtQtyNextLevelItems.Text = "1";

            txtNetContent.Text = "";
            txtTargetMarket.Text = "United States";
            txtCustomerBrandName.Text = "";
            dvBtnCancel.Visible = true;
            dvBtnSubmit.Visible = false;
        }
        protected void rptWSChildItem_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                CompassWorldSyncListItem WSChildItem = (CompassWorldSyncListItem)e.Item.DataItem;

                TextBox txtChildTargetMarket = (TextBox)e.Item.FindControl("txtChildTargetMarket");
                TextBox txtChildProductType = (TextBox)e.Item.FindControl("txtChildProductType");

                txtChildTargetMarket.Text = string.IsNullOrEmpty(WSChildItem.TargetMarket) ? "" : WSChildItem.TargetMarket;
                txtChildProductType.Text = string.IsNullOrEmpty(WSChildItem.ProductType) ? "" : WSChildItem.ProductType;
            }
        }
        protected void rptWSChildItem_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName.ToLower() == "delete")
            {
                if (id > 0)
                    worldSyncService.DeleteWorldSyncDetailItem(id);

                LoadWSChildItems();
            }
            if (e.CommandName.ToLower() == "loadcontrol")
            {
                showLabel = false;
                saveData("parent");
                hddWorldSyncChildId.Value = id.ToString();
                hdnPageState.Value = "child";
                LoadFormData();
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
                this.hiddenItemId.Value = iItemId.ToString();
                return false;
            }

            // Store Id in Hidden field
            this.hiddenItemId.Value = iItemId.ToString();
            return true;
        }
        #endregion

        #region Data Transfer Methods
        
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
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "Initial Approver Review");
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Utilities.RedirectPageForm(Utilities.GetCurrentPageName(), ProjectNumber), false);
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                showLabel = true;
                saveData(hdnPageState.Value);
                if(hdnPageState.Value == "child") {
                    Page.Response.Redirect(Utilities.RedirectPageForm(Utilities.GetCurrentPageName(), ProjectNumber), false);
                }
                else
                {
                    Page.Response.Redirect(Utilities.RedirecttoHomePage(), false);
                }
                
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.WorldSyncGlobal.ToString() + ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.WorldSyncGlobal.ToString(), "btnSubmit_Click");
            }
        }
        protected void addSyncDetail(object sender, EventArgs e)
        {
            showLabel = false;
            saveData("parent");
            hddWorldSyncChildId.Value = "-1";
            hdnPageState.Value = "child";    
            loadNewChildData();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            showLabel = true;
            saveData(hdnPageState.Value);
        }
        private void saveData(string updating)
        {
            int itemId;
            try
            {
                itemId = worldSyncService.UpsertCompassWorldSyncListItem(getCompassworldSync());
                if(updating != "child")
                {
                    hddWorldSyncGlobalId.Value = itemId.ToString();
                }
                else
                {
                    hddWorldSyncChildId.Value = itemId.ToString();
                    Page.Response.Redirect(Utilities.RedirectPageForm(Utilities.GetCurrentPageName(), ProjectNumber), false);
                }
                if (showLabel)
                {
                    lblSavedMessage.Text = "Changes Saved: " + DateTime.Now.ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.WorldSyncGlobal.ToString() + ": btnSave_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.WorldSyncGlobal.ToString(), "btnSave_Click");
            }
        }
        
        #endregion
        #region Private Methods
        private CompassWorldSyncListItem getCompassworldSync()
        {
            CompassWorldSyncListItem worldSync;
            worldSync = new CompassWorldSyncListItem();
            if (Page.IsPostBack)
            {
                
                
                if (hdnPageState.Value != "child")
                {
                    worldSync.Id = Convert.ToInt32(hddWorldSyncGlobalId.Value);
                    worldSync.ParentId = 0;
                    worldSync.AlternateClassificationScheme = ddlAlternateClassificationScheme.SelectedItem.Text;

                    worldSync.Code = ddlAlternateClassificationScheme.SelectedValue;
                    worldSync.AlternateItemIdAgency = txtAlternateItemIdAgency.Text;
                    worldSync.TemperatureQualitiferCode = txtTemperatureQualitiferCode.Text;
                    worldSync.TradeChannel = hdnSelectedTradeChannel.Value;
                }
                else
                {
                    worldSync.Id = Convert.ToInt32(hddWorldSyncChildId.Value);
                    worldSync.ParentId = Convert.ToInt32(hddWorldSyncGlobalId.Value);
                }

                worldSync.CompassListItemId = iItemId;
                worldSync.ProductType = ddlProductType.SelectedItem.Text;
                worldSync.BrandOwnerGLN = ddlBrandOwnerGLN.SelectedItem.Text;
                worldSync.GS1TradeItemsIDKeyCode = ddlGS1TradeItemsIDkeyCode.SelectedItem.Text;

                worldSync.DataCarrierTypeCode = ddlDataCarrierTypeCode.SelectedItem.Text;
                worldSync.BaseUnitIndicator = ddlBaseUnitIndicator.SelectedItem.Text;
                worldSync.ConsumerUnitIndicator = ddlConsumerUnitIndicator.SelectedItem.Text;
                worldSync.OrderingUnitIndicator = ddlOrderingUnitIndicator.SelectedItem.Text;
                worldSync.DispatchUnitIndicator = ddlDispatchUnitIndicator.SelectedItem.Text;
                worldSync.InvoiceUnitIndicator = ddlInvoiceUnitIndicator.SelectedItem.Text;

                worldSync.TargetMarket = txtTargetMarket.Text;
                worldSync.CustomerBrandName = txtCustomerBrandName.Text;
                worldSync.NetContent = txtNetContent.Text;
                worldSync.QtyOfNextLevelItems = txtQtyNextLevelItems.Text;
                
            }
            return worldSync;
        }

        #endregion
        #endregion
    }
}
