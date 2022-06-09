using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.DependencyResolution;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Classes;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ferrara.Compass.WebParts.WorldSyncNutritionalsForm
{
    [ToolboxItemAttribute(false)]
    public partial class WorldSyncNutritionalsForm : WebPart
    {
        #region Member Variables
        private IWorldSyncNutritionalService nutritionalService;
        private INotificationService notificationService;
        private IExceptionService exceptionService;
        private IUserManagementService userMgmtService;
        private IUtilityService utilityService;
        private int compassItemId = 0;
        private Random rnd = new Random();
        private bool addExtra = false;
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
        public WorldSyncNutritionalsForm()
        {
        }
        protected void lbHelpDeskEmail_Click(object sender, EventArgs e)
        {
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "Distribution");
            this.divAccessDenied.Visible = false;
            this.divAccessRequest.Visible = true;
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            nutritionalService = DependencyResolution.DependencyMapper.Container.Resolve<IWorldSyncNutritionalService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
            userMgmtService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
                compassItemId = Convert.ToInt32(hddCompassItemId.Value);
            else
            {
                this.divAccessDenied.Visible = false;
                this.divAccessRequest.Visible = false;
                try {
                    if (!CheckProjectNumber())
                        return;
                    //InitializeScreen();
                    Utilities.BindDropDownItems(ddlPreparationState, GlobalConstants.LIST_PreparationStateLookup, SPContext.Current.Web.Url);
                    Utilities.BindDropDownItems(ddlNutrientBasisQuantityType, GlobalConstants.LIST_NutrientQuantityContainedTypeLookup, SPContext.Current.Web.Url);
                    Utilities.BindDropDownItems(ddlServingSizeUOM, GlobalConstants.LIST_ServingSizeLookup, SPContext.Current.Web.Url);
                    Utilities.BindDropDownItems(ddlNutrientBasisQuantityUOM, GlobalConstants.LIST_NutrientQuantityUOMlookup, SPContext.Current.Web.Url);
                }
                catch (Exception exception)
                {
                    ErrorSummary.AddError(exception.Message, this.Page);
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.Distribution.ToString() + ": " + exception.Message);
                    exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.Distribution.ToString(), "Page_Load");
                }
                LoadFormData();
            }
        }
        private void LoadFormData()
        {
            WorldSyncNutritionalsListItem item = nutritionalService.GetNutritionalItem(compassItemId);
            if (item == null)
            {
                Utilities.SetDropDownValue("By_Serving", ddlNutrientBasisQuantityType, this.Page);
                Utilities.SetDropDownValue("Calorie-International (IT)", ddlNutrientBasisQuantityUOM, this.Page);
                Utilities.SetDropDownValue("Ready To Eat", ddlPreparationState, this.Page);
                txtNutrientBasisQty.Text = "2000";
                txtAllergenSpecificationAgency.Text = "FDA";
                txtAllergenSpecificationName.Text = "FALCPA";
            }
            else
            {
                Utilities.SetDropDownValue(item.NutrientBasisQtyType, ddlNutrientBasisQuantityType, this.Page);
                Utilities.SetDropDownValue(item.NutrientBasisQtyUOM, ddlNutrientBasisQuantityUOM, this.Page);
                Utilities.SetDropDownValue(item.ServingSizeUOM, ddlServingSizeUOM, this.Page);
                Utilities.SetDropDownValue(item.PreparationState, ddlPreparationState, this.Page);
                txtServingsPerPackage.Text = item.ServingsPerPackage.ToString();
                txtNutrientBasisQty.Text = item.NutrientBasisQty.ToString();
                txtServingSizeDescription.Text = item.ServingSizeDescription;
                txtServingSize.Text = item.ServingSize.ToString();
                txtIngredientStatement.Text = item.IngredientStatement.ToString();
                txtAllergenSpecificationAgency.Text = item.AllergenSpecificationAgency.ToString();
                txtAllergenSpecificationName.Text = item.AllergenSpecificationName.ToString();
                txtAllergenStatement.Text = item.AllergenStatement.ToString();
                hddNutritionalId.Value = item.Id.ToString();
            }
            
            bindNutrientData();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int itemId;
            try
            {
                itemId = nutritionalService.UpsertWorldSyncNutritionalsListItem(getNutritionalItem());
                SaveNutrientItem();
                hddNutritionalId.Value = itemId.ToString();
                addExtra = false;
                LoadFormData();
                lblSavedMessage.Text = "Changes Saved: " + DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.WorldSyncNutritionals.ToString() + ": btnSave_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.WorldSyncNutritionals.ToString(), "btnSave_Click");
            }
        }
        private void SaveNutrientItem()
        {
            foreach (RepeaterItem item in rptNutrientInfo.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    WorldSyncNutritionalsListDetailItem nutrientItem = new WorldSyncNutritionalsListDetailItem();
                    nutrientItem.NutrientQtyContained = Convert.ToInt32(((TextBox)item.FindControl("txtNutrientQtyContained")).Text);
                    nutrientItem.NutrientQtyContainedMeasPerc = ((DropDownList)item.FindControl("drpNutrientQtyContainedMeasPerc")).SelectedItem.Text;
                    nutrientItem.NutrientQtyContainedUOM = ((DropDownList)item.FindControl("drpNutrientQtyContainedUOM")).SelectedItem.Text;
                    nutrientItem.NutrientType = ((DropDownList)item.FindControl("drpNutrientType")).SelectedItem.Text;
                    nutrientItem.DailyValueIntakePct = ((DropDownList)item.FindControl("drpDailyValueIntakePct")).SelectedItem.Text;
                    nutrientItem.PctDailyValue = Convert.ToInt32(((TextBox)item.FindControl("txtPctDailyValue")).Text);
                    nutrientItem.CompassListItemId = Convert.ToInt32(hddCompassItemId.Value);

                    nutrientItem.Id = Convert.ToInt32(((HiddenField)item.FindControl("hdnItemID")).Value);
                    int newId = nutritionalService.UpsertWorldSyncNutritionalsListDetailItem(compassItemId, nutrientItem);
                        HiddenField hidId = ((HiddenField)item.FindControl("hidNewId"));
                        if (hidId != null)
                            hidId.Value = newId.ToString();
                    
                }
            }
            
        }

        #region Private Methods
        private WorldSyncNutritionalsListItem getNutritionalItem()
        {
            WorldSyncNutritionalsListItem nutritionalItem;
            nutritionalItem = new WorldSyncNutritionalsListItem();
            nutritionalItem.Id = Convert.ToInt32(hddNutritionalId.Value);
            nutritionalItem.CompassListItemId = compassItemId;
            nutritionalItem.NutrientBasisQty = txtNutrientBasisQty.Text;
            nutritionalItem.NutrientBasisQtyType = ddlNutrientBasisQuantityType.SelectedItem.Text;
            nutritionalItem.NutrientBasisQtyUOM = ddlNutrientBasisQuantityUOM.SelectedItem.Text;
            nutritionalItem.PreparationState = ddlPreparationState.SelectedItem.Text;
            nutritionalItem.ServingsPerPackage = txtServingsPerPackage.Text;
            nutritionalItem.ServingSizeDescription = txtServingSizeDescription.Text;
            nutritionalItem.ServingSize = txtServingSize.Text;
            nutritionalItem.ServingSizeUOM = ddlServingSizeUOM.SelectedItem.Text;
            nutritionalItem.IngredientStatement = txtIngredientStatement.Text;
            nutritionalItem.AllergenSpecificationAgency = txtAllergenSpecificationAgency.Text;
            nutritionalItem.AllergenSpecificationName = txtAllergenSpecificationName.Text;
            nutritionalItem.AllergenStatement = txtAllergenStatement.Text;
            return nutritionalItem;
        }
        private bool CheckProjectNumber()
        {
            compassItemId = Utilities.GetItemIdFromProjectNumber(ProjectNumber);

            if (compassItemId == 0)
            {
                // Invalid project Id supplied
                Page.Response.Redirect("/_layouts/Ferrara.Compass/AppPages/CompassErrorPage.aspx?ErrorId=1&ProjectNo=" + ProjectNumber, false);
                return false;
            }

            // Store Id in Hidden field
            this.hddCompassItemId.Value = compassItemId.ToString();
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

            string workflowPhase = utilityService.GetWorkflowPhase(compassItemId);
            if ((string.Equals(workflowPhase, WorkflowStep.OnHold.ToString())) || (string.Equals(workflowPhase, WorkflowStep.Cancelled.ToString())) ||
                (string.Equals(workflowPhase, WorkflowStep.Completed.ToString())))
            {
                this.btnSave.Enabled = false;
                this.btnSubmit.Enabled = false;
            }
        }

        #endregion
        public void bindNutrientData()
        {
            List<WorldSyncNutritionalsListDetailItem> nutrientlst = nutritionalService.GetNutritionalDetailItems(compassItemId);
            if (addExtra)
            {
                WorldSyncNutritionalsListDetailItem blankItem = new WorldSyncNutritionalsListDetailItem();
                blankItem.NutrientQtyContained = 0;
                blankItem.NutrientQtyContainedMeasPerc = "";
                blankItem.NutrientQtyContainedUOM = "";
                blankItem.NutrientType = "";
                blankItem.PctDailyValue = 0;
                blankItem.DailyValueIntakePct = "";

                nutrientlst.Add(blankItem);
            }

            rptNutrientInfo.DataSource = nutrientlst;
            rptNutrientInfo.DataBind();
        }
        protected void addNutritionalDetail(object sender, EventArgs e)
        {
            int itemId = nutritionalService.UpsertWorldSyncNutritionalsListItem(getNutritionalItem());
            SaveNutrientItem();
            hddNutritionalId.Value = itemId.ToString();
            addExtra = true;
            LoadFormData();
        }
        protected void rptNutrientInfo_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToLower() == "delete")
            {
                int id = Convert.ToInt32(e.CommandArgument.ToString());
                if (id > 0)
                {
                    nutritionalService.DeleteNutritionalDetailItem(id);
                }
                List<WorldSyncNutritionalsListDetailItem> nutrientlst = nutritionalService.GetNutritionalDetailItems(compassItemId);
                rptNutrientInfo.DataSource = nutrientlst;
                rptNutrientInfo.DataBind();

            }
        }

        protected void rptNutrientInfo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                if (e.Item.Visible)
                {
                    WorldSyncNutritionalsListDetailItem nutrientItem = (WorldSyncNutritionalsListDetailItem)e.Item.DataItem;

                    DropDownList drpNutrientType = ((DropDownList)e.Item.FindControl("drpNutrientType"));
                    Utilities.BindDropDownItems(drpNutrientType, GlobalConstants.LIST_NutrientTypeLookup, SPContext.Current.Web.Url);
                    Utilities.SetDropDownValue(nutrientItem.NutrientType, drpNutrientType, this.Page);

                    DropDownList drpNutrientQtyContainedMeasPerc = ((DropDownList)e.Item.FindControl("drpNutrientQtyContainedMeasPerc"));
                    Utilities.BindDropDownItems(drpNutrientQtyContainedMeasPerc, GlobalConstants.LIST_DailyValueIntakePctMeasPrecCodeLookup, SPContext.Current.Web.Url);
                    Utilities.SetDropDownValue(nutrientItem.NutrientQtyContainedMeasPerc, drpNutrientQtyContainedMeasPerc, this.Page);

                    DropDownList drpNutrientQtyContainedUOM = ((DropDownList)e.Item.FindControl("drpNutrientQtyContainedUOM"));
                    Utilities.BindDropDownItems(drpNutrientQtyContainedUOM, GlobalConstants.LIST_NutrientQuantityUOMlookup, SPContext.Current.Web.Url);
                    Utilities.SetDropDownValue(nutrientItem.NutrientQtyContainedUOM, drpNutrientQtyContainedUOM, this.Page);

                    DropDownList drpDailyValueIntakePct = ((DropDownList)e.Item.FindControl("drpDailyValueIntakePct"));
                    Utilities.BindDropDownItems(drpDailyValueIntakePct, GlobalConstants.LIST_DailyValueIntakePctMeasPrecCodeLookup, SPContext.Current.Web.Url);
                    Utilities.SetDropDownValue(nutrientItem.DailyValueIntakePct, drpDailyValueIntakePct, this.Page);

                    TextBox txtNutrientQtyContained = (TextBox)e.Item.FindControl("txtNutrientQtyContained");
                    txtNutrientQtyContained.Text = nutrientItem.NutrientQtyContained.ToString();

                    TextBox txtPctDailyValue = (TextBox)e.Item.FindControl("txtPctDailyValue");
                    txtPctDailyValue.Text = nutrientItem.PctDailyValue.ToString();
                
                }
            }
        }

    }
}
