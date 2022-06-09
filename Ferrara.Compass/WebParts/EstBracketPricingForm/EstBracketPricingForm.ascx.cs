using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Classes;
using Microsoft.Practices.Unity;
using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;

namespace Ferrara.Compass.WebParts.EstBracketPricingForm
{
    [ToolboxItemAttribute(false)]
    public partial class EstBracketPricingForm : WebPart
    {
        #region Member Variables
        private ITradePromoGroupService tradePromoService;
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private IWorkflowService workflowService;
        private INotificationService notificationService;
        private IUserManagementService userMgmtService;
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
        public EstBracketPricingForm()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            tradePromoService = DependencyResolution.DependencyMapper.Container.Resolve<ITradePromoGroupService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
            userMgmtService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
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
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.EstBracketPricing.ToString() + ": " + exception.Message);
                    exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.EstBracketPricing.ToString(), "Page_Load");
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
            if (!userMgmtService.HasReadAccess(CompassForm.EstBracketPricing))
            {
                this.divAccessDenied.Visible = true;
            }

            // If user does not have rights to save/submit the page, disable the Save and Submit buttons
            if (!userMgmtService.HasWriteAccess(CompassForm.EstBracketPricing))
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
            var item = tradePromoService.GetTradePromoGroupItem(iItemId);

            if (item.ProjectType == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
            {
                lblComments.Text = "Change Notes:";
            }
            else
            {
                lblComments.Text = "Item Concept:";
            }

            txtItemConcept.Text = item.ItemConcept;

            this.lblProductHierarchyLevel1.Text = item.ProductHierarchyLevel1;
            this.lblProductHierarchyLevel2.Text = item.ProductHierarchyLevel2;
            this.lblMaterialGroup1.Text = item.MaterialGroup1Brand;
            this.lblMaterialGroup4.Text = item.MaterialGroup4ProductForm;
            this.lblMaterialGroup5.Text = item.MaterialGroup5PackType;
            this.lblCaseType.Text = item.CaseType;
            if (string.IsNullOrEmpty(item.UnitsInsideCarton))
            {
                divNumberOfUnitsInsideCarton.Visible = false;
            }
            else
            {
                this.txtNumberOfUnitsInsideCarton.Text = item.UnitsInsideCarton;
            }

            if (item.IndividualPouchWeight == -9999)
            {
                divIndividualPouchWeight.Visible = false;
            }
            else
            {
                this.txtIndividualPouchWeight.Text = Convert.ToString(item.IndividualPouchWeight);
            }


            if (item.NumberofTraysPerBaseUOM == -9999)
            {
                divNumberOfTraysPerBaseUOM.Visible = false;
            }
            else
            {
                this.txtNumberOfTraysPerBaseUOM.Text = Convert.ToString(item.NumberofTraysPerBaseUOM);
            }

            if (item.RetailSellingUnitsBaseUOM == -9999)
            {
                divRetailSellingUOM.Visible = false;
            }
            else
            {
                this.txtRetailSellingUOM.Text = item.RetailSellingUnitsBaseUOM.ToString();
            }

            if (item.RetailUnitWieghtOz == -9999)
            {
                divRetailUnitWeight.Visible = false;
            }
            else
            {
                this.txtRetailUnitWeight.Text = Convert.ToString(item.RetailUnitWieghtOz);
            }

            if (item.BaseUOMNetWeightLbs == -9999)
            {
                divBaseUOMNetWeight.Visible = false;
            }
            else
            {
                this.txtBaseUOMNetWeight.Text = Convert.ToString(item.BaseUOMNetWeightLbs);
            }


            this.txtBaseUOM.Text = item.SAPBaseUOM;
            this.txtTruckloadPricePerUnit.Text = "$" + Utilities.FormatCurrency(item.TruckLoadPricePerSellingUnit);
            this.txtChannelSpecific.Text = item.CustomerSpecific;
            this.txtChannel.Text = item.Channel;
            this.txtCustomer.Text = item.Customer;

            if (string.IsNullOrEmpty(txtCustomer.Text) || txtCustomer.Text.ToLower().Contains("select"))
            {
                dvCustomer.Visible = false;
            }

            if (string.IsNullOrEmpty(txtChannel.Text) || txtChannel.Text.ToLower().Contains("select"))
            {
                dvChannel.Visible = false;
            }

            double calc = (item.RetailSellingUnitsBaseUOM * item.TruckLoadPricePerSellingUnit);
            if (calc < 0)
                txtTruckloadPriceUOM.Text = "$0";
            else
                txtTruckloadPriceUOM.Text = "$" + Utilities.FormatCurrency(calc);

            hdnProjectType.Value = item.ProjectType;

            this.chkInitialEstimatedBracketPricing.Checked = (item.InitialEstimatedBracketPricing == "Yes" ? true : false);
        }
        private TradePromoGroupItem ConstructFormData()
        {
            var item = new TradePromoGroupItem();

            item.CompassListItemId = iItemId;
            item.InitialEstimatedBracketPricing = this.chkInitialEstimatedBracketPricing.Checked ? "Yes" : "No";
            item.LastUpdatedFormName = CompassForm.EstBracketPricing.ToString();

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
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "Customer Marketing");
            this.divAccessDenied.Visible = false;
            this.divAccessRequest.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userMgmtService.HasWriteAccess(CompassForm.EstBracketPricing))
                {
                    ErrorSummary.AddError("You do not have proper access rights to save this page!", this.Page);
                    return;
                }

                // Retrieve the data from the form
                var item = ConstructFormData();
                tradePromoService.UpdateEstimatedBracketPricingItem(item);

                var approvalItem = ConstructApprovalData();
                tradePromoService.UpdateEstimatedBracketPricingApprovalItem(approvalItem, false);

                lblSavedMessage.Text = "Changes Saved: " + DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.EstBracketPricing.ToString() + ": btnSave_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.EstBracketPricing.ToString(), "btnSave_Click");
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userMgmtService.HasWriteAccess(CompassForm.EstBracketPricing))
                {
                    ErrorSummary.AddError("You do not have proper access rights to submit this page!", this.Page);
                    return;
                }

                // Retrieve the data from the form
                var item = ConstructFormData();
                tradePromoService.UpdateEstimatedBracketPricingItem(item);

                var approvalItem = ConstructApprovalData();
                tradePromoService.UpdateEstimatedBracketPricingApprovalItem(approvalItem, true);

                // Complete the workflow task
                workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.EstBracketPricing);

                // Redirect to Home page after successfull Submit                    
                Page.Response.Redirect(Utilities.RedirecttoHomePage(), false);
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.EstBracketPricing.ToString() + ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.EstBracketPricing.ToString(), "btnSubmit_Click");
            }
        }
        #endregion

        protected void lnlIPF_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Utilities.RedirectPageForm(GlobalConstants.PAGE_ItemProposal, ProjectNumber), false);
        }
    }
}
