using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Practices.Unity;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.DependencyResolution;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Classes;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Web.UI.WebControls;

namespace Ferrara.Compass.WebParts.InitialCapacityReviewForm
{
    [ToolboxItemAttribute(false)]
    public partial class InitialCapacityReviewForm : WebPart
    {
        #region Member Variables
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private IWorkflowService workflowService;
        private IUserManagementService userMgmtService;
        private int iItemId = 0;
        private INotificationService notificationService;
        private IInitialCapacityReviewService capacityReviewService;
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
        public InitialCapacityReviewForm()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            capacityReviewService = DependencyResolution.DependencyMapper.Container.Resolve<IInitialCapacityReviewService>();
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
                }
                catch (Exception exception)
                {
                    //ErrorSummary.AddError(exception.Message, this.Page);
                    //LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.InitialCapacity.ToString() + ": " + exception.Message);
                    //exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.InitialCapacity.ToString(), "Page_Load");
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
                Page.Response.Redirect("/_layouts/Compass/AppPages/CompassErrorPage.aspx?ErrorId=1&ProjectNo=" + ProjectNumber, false);
                return false;
            }

            // Store Id in Hidden field
            this.hiddenItemId.Value = iItemId.ToString();
            return true;
        }
        private void InitializeScreen()
        {
            //// If user does not belong to a valid group for the page, inform them that they do not hvae access rights
            //if (!userMgmtService.HasReadAccess(CompassForm.InitialCapacity))
            //{
            //    this.divAccessDenied.Visible = true;
            //}

            //If user does not have rights to save/ submit the page, disable the Save and Submit buttons
            //if (!userMgmtService.HasWriteAccess(CompassForm.InitialCapacity))
            //{
            //    this.btnSave.Enabled = false;
            //    this.btnSubmit.Enabled = false;
            //}

            //if (Utilities.LockScreen(wfStep.ToString()))
            //{
            //    if (!Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_OBMAdmins))
            //    {
            //        this.btnSave.Enabled = false;
            //        this.btnSubmit.Enabled = false;
            //    }
            //}

            string workflowPhase = utilityService.GetWorkflowPhase(iItemId);
            if ((string.Equals(workflowPhase, WorkflowStep.OnHold.ToString())) || (string.Equals(workflowPhase, WorkflowStep.Cancelled.ToString())) ||
                (string.Equals(workflowPhase, WorkflowStep.Completed.ToString())))
            {
                this.btnSave.Enabled = false;
                this.btnSubmit.Enabled = false;
            }
        }
        private void fixMonth(string FirstProductionDate)
        {
            if (!string.IsNullOrEmpty(FirstProductionDate))
            {
                string[] months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

                DateTime date = Convert.ToDateTime(FirstProductionDate);
                int selectedMonth = date.Month - 1;
                lbl1stmonthU.InnerText = months[selectedMonth] + " Projected Retail Units:";
                lbl1stmonthC.InnerText = months[selectedMonth] + " Projected Cases:";
                lbl1stmonthL.InnerText = months[selectedMonth] + " Projected lbs:";


                if (selectedMonth + 1 > 11)
                    selectedMonth = -1;

                lbl2ndmonthU.InnerText = months[selectedMonth + 1] + " Projected Retail Units:";
                lbl2ndmonthC.InnerText = months[selectedMonth + 1] + " Projected Cases:";
                lbl2ndmonthL.InnerText = months[selectedMonth + 1] + " Projected lbs:";

                if (selectedMonth + 2 > 11)
                    selectedMonth = -2;
                lbl3rdmonthU.InnerText = months[selectedMonth + 2] + " Projected Retail Units:";
                lbl3rdmonthC.InnerText = months[selectedMonth + 2] + " Projected Cases:";
                lbl3rdmonthL.InnerText = months[selectedMonth + 2] + " Projected lbs:";
            }


        }
        #endregion

        #region Data Transfer Methods
        private void LoadFormData()
        {
            InitialCapacityReviewItem initialCapacityItem = capacityReviewService.GetInitialCapacityReviewItem(iItemId);
            // Set the Initial Approval Information
            try
            {
                lblAnnualProjectedUnits.Text = Utilities.FormatNumber(initialCapacityItem.AnnualProjectedUnits);

                if (initialCapacityItem.SAPBaseUOM.ToLower() == "cs")
                {
                    if (initialCapacityItem.RetailSellingUnitsBaseUOM != 0)
                        lblAnnualProjectedCases.Text = Utilities.FormatNumber(initialCapacityItem.AnnualProjectedUnits / initialCapacityItem.RetailSellingUnitsBaseUOM);
                    else
                        lblAnnualProjectedCases.Text = "NA";
                }
                else
                    lblAnnualProjectedCases.Text = "NA";

                lblAnnualProjectLbs.Text = Utilities.FormatNumber((initialCapacityItem.AnnualProjectedUnits * initialCapacityItem.RetailUnitWieghtOz) / 16);
                lblPrimaryMakeLocation.Text = initialCapacityItem.ManufacturingLocation;
                lblPrimaryPackLocation.Text = initialCapacityItem.PackingLocation;
                
                lblComments.Text = initialCapacityItem.SrOBMApproval_CapacityReviewComments;
                lblLineOfBusiness.Text = initialCapacityItem.LineOfBusiness;
                // Load Capacity Review
                Utilities.SetDropDownValue(initialCapacityItem.InitialCapacity_MakeIssues, this.ddlMakeLocationIssues, this.Page);
                Utilities.SetDropDownValue(initialCapacityItem.InitialCapacity_PackIssues, this.ddlPackLocationIssues, this.Page);
                txtCapacityComments.Text = initialCapacityItem.InitialCapacity_CapacityRiskComments;
                Utilities.SetDropDownValue(initialCapacityItem.InitialCapacity_Decision, this.ddlProjectDecision, this.Page);
                txtProjectAcceptance.Text = initialCapacityItem.InitialCapacity_AcceptanceComments;

                txtProjectUnit1.Text = Utilities.FormatNumber(initialCapacityItem.Month1ProjectedUnits);
                txtProjectUnit2.Text = Utilities.FormatNumber(initialCapacityItem.Month2ProjectedUnits);
                txtProjectUnit3.Text = Utilities.FormatNumber(initialCapacityItem.Month3ProjectedUnits);

                if (initialCapacityItem.SAPBaseUOM.ToLower() == "cs")
                {
                    if (initialCapacityItem.RetailSellingUnitsBaseUOM != 0)
                    {
                        txtProjectCase1.Text = Utilities.FormatNumber(initialCapacityItem.Month1ProjectedUnits / initialCapacityItem.RetailSellingUnitsBaseUOM);
                        txtProjectCase2.Text = Utilities.FormatNumber(initialCapacityItem.Month2ProjectedUnits / initialCapacityItem.RetailSellingUnitsBaseUOM);
                        txtProjectCase3.Text = Utilities.FormatNumber(initialCapacityItem.Month3ProjectedUnits / initialCapacityItem.RetailSellingUnitsBaseUOM);

                    }
                    else
                    {
                        txtProjectCase1.Text = "NA";
                        txtProjectCase2.Text = "NA";
                        txtProjectCase3.Text = "NA";
                    }
                }
                else
                {
                    txtProjectCase1.Text = "NA";
                    txtProjectCase2.Text = "NA";
                    txtProjectCase3.Text = "NA";

                }

                txtProjectlbs1.Text = Utilities.FormatNumber((initialCapacityItem.Month1ProjectedUnits * initialCapacityItem.RetailUnitWieghtOz) / 16);
                txtProjectlbs2.Text = Utilities.FormatNumber((initialCapacityItem.Month2ProjectedUnits * initialCapacityItem.RetailUnitWieghtOz) / 16);
                txtProjectlbs3.Text = Utilities.FormatNumber((initialCapacityItem.Month3ProjectedUnits * initialCapacityItem.RetailUnitWieghtOz) / 16);

                if ((initialCapacityItem.FirstProductionDate != null) && (initialCapacityItem.FirstProductionDate != DateTime.MinValue)) {
                    txtProductionDate.Text = Utilities.GetDateForDisplay(initialCapacityItem.FirstProductionDate);
                }
                lblFirstShipDate.Text = Utilities.GetDateForDisplay(initialCapacityItem.RevisedFirstShipDate);
                fixMonth(initialCapacityItem.RevisedFirstShipDate.ToShortDateString());
                
                // Load Attachments
                GetAttachments();
            }
            catch (Exception ex)
            {
                //ErrorSummary.AddError(ex.Message, this.Page);
                //LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.InitialCapacity.ToString() + ": LoadFormData: " + ex.Message);
                //exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.InitialCapacity.ToString(), "LoadFormData");
            }
        }
        private InitialCapacityReviewItem ConstructFormData()
        {
            InitialCapacityReviewItem item = new InitialCapacityReviewItem();
            item.CompassListItemId = iItemId;

            item.InitialCapacity_MakeIssues = ddlMakeLocationIssues.SelectedItem.Text;
            item.InitialCapacity_PackIssues = ddlPackLocationIssues.SelectedItem.Text;
            item.InitialCapacity_CapacityRiskComments = txtCapacityComments.Text;
            item.InitialCapacity_Decision = ddlProjectDecision.SelectedItem.Text;
            item.InitialCapacity_AcceptanceComments = txtProjectAcceptance.Text;
            
            if(!string.IsNullOrEmpty(txtProductionDate.Text))
            item.FirstProductionDate = Utilities.GetDateFromField(txtProductionDate.Text); 
          
            //item.first

            return item;
        }
        private ApprovalItem ConstructApprovalData()
        {
            ApprovalItem item = new ApprovalItem();
            item.CompassListItemId = iItemId;

            item.ModifiedBy = SPContext.Current.Web.CurrentUser.Name;
            item.ModifiedDate = DateTime.Now.ToString();

            return item;
        }

        #endregion

        #region Attachment Methods
        private void GetAttachments()
        {
            var files = utilityService.GetUploadedCompassFilesByDocType(ProjectNumber, GlobalConstants.DOCTYPE_CAPACITY);
            if (files.Count > 0)
            {
                rpAttachments.Visible = true;
                rpAttachments.DataSource = files;
                rpAttachments.DataBind();
            }
            else
            {
                rpAttachments.Visible = false;
            }
        }
        protected void lnkFileDelete_Click(object sender, EventArgs e)
        {
            var lnkButton = (LinkButton)sender;
            if (!string.IsNullOrEmpty(lnkButton.CommandName))
            {
                utilityService.DeleteAttachment(lnkButton.CommandName);
                GetAttachments();
            }
        }
        #endregion

        #region Button Methods
        protected void lnlIPF_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Utilities.RedirectPageForm(GlobalConstants.PAGE_ItemProposal, ProjectNumber), false);
        }
        protected void lbHelpDeskEmail_Click(object sender, EventArgs e)
        {
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "Initial Approver Review");
            this.divAccessDenied.Visible = false;
            this.divAccessRequest.Visible = true;
        }
        protected void btnReloadAttachment_Click(object sender, EventArgs e)
        {
            GetAttachments();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //if (!userMgmtService.HasWriteAccess(CompassForm.InitialCapacity))
                //{
                //    ErrorSummary.AddError("You do not have proper access rights to save this page!", this.Page);
                //    return;
                //}
                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }

                InitialCapacityReviewItem capacityItem = ConstructFormData();
                capacityReviewService.UpdateInitialCapacityReviewItem(capacityItem);

                ApprovalItem appItem = ConstructApprovalData();
                //capacityReviewService.UpdateInitialCapacityReviewApprovalItem(appItem, false);

                ////////// Load Attachments
                GetAttachments();

                lblSavedMessage.Text = "Changes Saved: " + DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                //ErrorSummary.AddError(ex.Message, this.Page);
                //LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.InitialCapacity.ToString() + ": btnSave_Click: " + ex.Message);
                //exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.InitialCapacity.ToString(), "btnSave_Click");
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //if (!userMgmtService.HasWriteAccess(CompassForm.InitialCapacity))
                //{
                //    ErrorSummary.AddError("You do not have proper access rights to submit this page!", this.Page);
                //    return;
                //}
                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }

                InitialCapacityReviewItem capacityItem = ConstructFormData();
                capacityReviewService.UpdateInitialCapacityReviewItem(capacityItem);

                ApprovalItem appItem = ConstructApprovalData();
                //capacityReviewService.UpdateInitialCapacityReviewApprovalItem(appItem, true);

                // Complete the workflow task
                //workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.InitialCapacity);

                // Redirect to Home page after successfull Submit                    
                Page.Response.Redirect(Utilities.RedirecttoHomePage(), false);
            }
            catch (Exception ex)
            {
                //ErrorSummary.AddError(ex.Message, this.Page);
                //LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.InitialCapacity.ToString() + ": btnSubmit_Click: " + ex.Message);
                //exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.InitialCapacity.ToString(), "btnSubmit_Click");
            }
        }
        #endregion
    }
}
