using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Practices.Unity;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.DependencyResolution;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Classes;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI;

namespace Ferrara.Compass.WebParts.ComponentCostingSummaryForm
{
    [ToolboxItemAttribute(false)]
    public partial class ComponentCostingSummaryForm : WebPart
    {
        #region Member Variables
        private IComponentCostingQuoteService componentCostingService;
        private IPackagingItemService packagingItemService;
        private IItemProposalService ipfService;
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
        public ComponentCostingSummaryForm()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();

            componentCostingService = DependencyResolution.DependencyMapper.Container.Resolve<IComponentCostingQuoteService>();
            packagingItemService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            ipfService = DependencyResolution.DependencyMapper.Container.Resolve<IItemProposalService>();
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

                    InitializeScreen();
                    LoadFormData();
                }
                catch (Exception exception)
                {
                    ErrorSummary.AddError(exception.Message, this.Page);
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.ComponentCosting.ToString() + ": " + exception.Message);
                    exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.ComponentCosting.ToString(), "Page_Load");
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
            if (!userMgmtService.HasReadAccess(CompassForm.ComponentCosting))
            {
                this.divAccessDenied.Visible = true;
            }

            // If user does not have rights to save/submit the page, disable the Save and Submit buttons
            /*if (!userMgmtService.HasWriteAccess(CompassForm.ComponentCosting))
            {
                //this.btnSave.Enabled = false;
                this.btnSubmit.Enabled = false;
            }

            string workflowPhase = utilityService.GetWorkflowPhase(iItemId);
            if ((string.Equals(workflowPhase, WorkflowStep.OnHold.ToString())) || (string.Equals(workflowPhase, WorkflowStep.Cancelled.ToString())) ||
                (string.Equals(workflowPhase, WorkflowStep.Completed.ToString())))
            {
                //this.btnSave.Enabled = false;
                this.btnSubmit.Enabled = false;
            }*/
        }
        private List<PackagingItem> GetComponentCostingPackagingItems()
        {
            List<PackagingItem> dtPackingItem = new List<PackagingItem>();

            if (ViewState["ComponentCostingItems"] == null)
            {
                if (iItemId > 0)
                {
                    ItemProposalItem ipfItem = ipfService.GetItemProposalItem(iItemId);
                    if (Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_PurchasingSeasonal))
                    {
                        if (string.Equals(ipfItem.ProductHierarchyLevel1, GlobalConstants.PRODUCT_HIERARCHY1_Seasonal))
                            dtPackingItem = packagingItemService.GetAllComponentCostingPackagingItemsForProject(iItemId);
                    }
                    else if (Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_PurchasingFilm))
                    {
                        if (!string.Equals(ipfItem.ProductHierarchyLevel1, GlobalConstants.PRODUCT_HIERARCHY1_Seasonal))
                            dtPackingItem = packagingItemService.GetFilmLabelRigidPlasticComponentCostingPackagingItemsForProject(iItemId);
                    }
                    else if (Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_PurchasingCorrugated))
                    {
                        if (!string.Equals(ipfItem.ProductHierarchyLevel1, GlobalConstants.PRODUCT_HIERARCHY1_Seasonal))
                            dtPackingItem = packagingItemService.GetCorrugatedPaperboardComponentCostingPackagingItemsForProject(iItemId);
                    }

                    ViewState["ComponentCostingItems"] = dtPackingItem;
                }
            }
            else
            {
                dtPackingItem = (List<PackagingItem>)ViewState["ComponentCostingItems"];
            }

            return dtPackingItem;
        }
        #endregion

        #region Data Transfer Methods
        private void LoadFormData()
        {
            List<PackagingItem> packagingItems = GetComponentCostingPackagingItems();

            if (packagingItems.Count == 0)
            {
                rptNewComponents.Visible = false;
                lblNoComponents.Visible = true;
            }
            else
            {
                rptNewComponents.DataSource = packagingItems;
                rptNewComponents.DataBind();
                lblNoComponents.Visible = false;
            }
        }
        private ComponentCostingQuoteItem ConstructFormData()
        {
            var item = new ComponentCostingQuoteItem();

            item.CompassListItemId = iItemId;

            item.LastUpdatedFormName = CompassForm.ComponentCosting.ToString();

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

        #region Repeater Methods
        protected void rptNewComponents_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {

        }
        protected void rptNewComponents_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                PackagingItem packagingItem = (PackagingItem)e.Item.DataItem;

                HtmlAnchor anc = ((HtmlAnchor)e.Item.FindControl("ancCostingQuote"));
                Panel compStatus = ((Panel)e.Item.FindControl("divCostQuoteStatus"));
                if (anc != null)
                {
                    //string fileName = files[0].FileName;
                    anc.InnerText = "Costing Quote";
                    anc.HRef = string.Concat(SPContext.Current.Web.Url, "/pages/", GlobalConstants.PAGE_ComponentCosting, "?", GlobalConstants.QUERYSTRING_ProjectNo, "=", ProjectNumber, "&", GlobalConstants.QUERYSTRING_PackagingItemId, "=", packagingItem.Id.ToString());
                    compStatus.Controls.Add(new LiteralControl("|"));
                    string submitted = packagingItem.CompCostSubmittedDate;
                    if(string.IsNullOrEmpty(submitted))
                    {
                        compStatus.CssClass = "redClass";
                    }
                    else
                    {
                        compStatus.CssClass = "greenClass";
                    }
                }
            }
        }
        #endregion

        #region Button Methods
        protected void lbHelpDeskEmail_Click(object sender, EventArgs e)
        {
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "Component Costing Summary");
            this.divAccessDenied.Visible = false;
            this.divAccessRequest.Visible = true;
        }

        //protected void btnSave_Click(object sender, EventArgs e)
        //{

        //}
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userMgmtService.HasWriteAccess(CompassForm.ComponentCosting))
                {
                    ErrorSummary.AddError("You do not have proper access rights to save this page!", this.Page);
                    return;
                }
                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }

                ApprovalItem approvalItem = ConstructApprovalData();
                //componentCostingService.UpdateComponentCostingApprovalItem(approvalItem, true);

                // Complete the workflow task
                ItemProposalItem ipfItem = ipfService.GetItemProposalItem(iItemId);
                if (Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_PurchasingSeasonal))
                {
                    if (string.Equals(ipfItem.ProductHierarchyLevel1, GlobalConstants.PRODUCT_HIERARCHY1_Seasonal))
                        workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.ComponentCostingSeasonal);
                }
                else if (Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_PurchasingFilm))
                {
                    if (!string.Equals(ipfItem.ProductHierarchyLevel1, GlobalConstants.PRODUCT_HIERARCHY1_Seasonal))
                        workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.ComponentCostingFilmLabelRigidPlastic);
                }
                else if (Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_PurchasingCorrugated))
                {
                    if (!string.Equals(ipfItem.ProductHierarchyLevel1, GlobalConstants.PRODUCT_HIERARCHY1_Seasonal))
                        workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.ComponentCostingCorrugatedPaperboard);
                }

                // Redirect to Home page after successfull Submit                    
                Page.Response.Redirect(Utilities.RedirecttoHomePage(), true);
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.ComponentCosting.ToString() + ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.ComponentCosting.ToString(), "btnSubmit_Click");
            }
        }
        #endregion

    }
}
