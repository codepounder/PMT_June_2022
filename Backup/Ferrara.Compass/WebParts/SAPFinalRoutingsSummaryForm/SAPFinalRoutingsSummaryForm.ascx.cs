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
using System.Text.RegularExpressions;
using System.Linq;

namespace Ferrara.Compass.WebParts.SAPFinalRoutingsSummaryForm
{
    [ToolboxItemAttribute(false)]
    public partial class SAPFinalRoutingsSummaryForm : WebPart
    {
        #region Member Variables
        private ISAPFinalRoutingsService sapFinalRoutingsService;
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
        private string SAPTask
        {
            get
            {
                if (Page.Request.QueryString[GlobalConstants.QUERYSTRING_SAPTask] != null)
                    return Page.Request.QueryString[GlobalConstants.QUERYSTRING_SAPTask];
                return GlobalConstants.QUERYSTRINGVALUE_FinalRoutings;
            }
        }
        #endregion

        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public SAPFinalRoutingsSummaryForm()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();

            sapFinalRoutingsService = DependencyResolution.DependencyMapper.Container.Resolve<ISAPFinalRoutingsService>();
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

        }
        private List<PackagingItem> GetComponentCostingPackagingItems()
        {
            List<PackagingItem> dtPackingItem = new List<PackagingItem>();

            if (ViewState["FinalRoutingItems"] == null)
            {
                if (iItemId > 0)
                {
                    dtPackingItem = packagingItemService.GetAllComponentCostingPackagingItemsForProject(iItemId);

                    ViewState["FinalRoutingItems"] = dtPackingItem;
                }
            }
            else
            {
                dtPackingItem = (List<PackagingItem>)ViewState["FinalRoutingItems"];
            }
            string packPlant = sapFinalRoutingsService.getProjectPackPlant(iItemId);
            packPlant = packPlant.Substring(packPlant.IndexOf("(") + 1, packPlant.Length - packPlant.IndexOf("(") - 2);
            foreach (PackagingItem item in dtPackingItem)
            {
                item.PackLocation = packPlant;
            }
            return dtPackingItem;
        }
        #endregion

        #region Data Transfer Methods
        private void LoadFormData()
        {
            string headerText = "";
            switch (SAPTask.ToUpper())
            {
                case "SAPROUTINGSETUP":
                    headerText = "Final Routings Updated in SAP";
                    break;
                case "SAPCOSTINGDETAILS":
                    headerText = "SAP Costing Details";
                    break;
                case "SAPWAREHOUSEINFO":
                    headerText = "SAP Warehouse Info";
                    break;
                case "STANDARDCOSTENTRY":
                    headerText = "Standard Cost Set";
                    break;
                case "COSTFINISHEDGOOD":
                    headerText = "Cost Finished Good";
                    break;
                case "FINALCOSTINGREVIEW":
                    headerText = "Final Costing Review";
                    break;
                case "PURCHASEPO":
                    headerText = "Purchased PO";
                    break;
                case "REMOVESAPBLOCKS":
                    headerText = "Remove SAP Blocks";
                    break;
                default:
                    headerText = "Final Routings Updated in SAP";
                    break;
            }
            headerTitle.Controls.Add(new LiteralControl(headerText));
            List<PackagingItem> packagingItems = new List<PackagingItem>();//GetComponentCostingPackagingItems();

            PackagingItem packagingItem = sapFinalRoutingsService.getCompassItem(iItemId);
            packagingItems.Add(packagingItem);
            //List<PackagingItem> packagingItems = GetComponentCostingPackagingItems();
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

            item.LastUpdatedFormName = CompassForm.FinalRoutingsSummary.ToString();

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

                Panel compStatus = ((Panel)e.Item.FindControl("divFinalRoutingsStatus"));
                TextBox lblSubmittedDate = ((TextBox)e.Item.FindControl("lblSubmittedDate"));
                Label lblStatus = ((Label)e.Item.FindControl("lblStatus"));
                Label lblDate = ((Label)e.Item.FindControl("lblDate"));
                if (compStatus != null)
                {
                    //string fileName = files[0].FileName;
                    compStatus.Controls.Add(new LiteralControl("|"));
                    string packPlant = Regex.Match(packagingItem.PackLocation, @"\(([^)]*)\)").Groups[1].Value;
                    try
                    {
                        packPlant = string.IsNullOrEmpty(packPlant) ? (packagingItem.PackLocation.Split('-').ToList().FirstOrDefault()) : packPlant;
                    }
                    catch (Exception ex)
                    {
                    }
                    FinalRoutingsItem frItem = sapFinalRoutingsService.GetSingleSAPFinalRoutingsItem(packagingItem.MaterialNumber, packPlant.Trim());
                    SAPApprovalListItem appItem = sapFinalRoutingsService.getSAPApprovalItem(iItemId);
                    string submittedDate = "";
                    string submitted = "";
                    string labelText = "";
                    switch (SAPTask.ToUpper())
                    {
                        case "SAPROUTINGSETUP":
                            labelText = "SAP Final Routing Setup";
                            submitted = frItem.SAPRoutings;
                            submittedDate = appItem.SAPRoutingSetup_SubmittedDate;
                            break;
                        case "SAPCOSTINGDETAILS":
                            labelText = "SAP Costing Details";
                            submitted = frItem.SourceListComplete;
                            submittedDate = appItem.SAPCostingDetails_SubmittedDate;
                            break;
                        case "SAPWAREHOUSEINFO":
                            labelText = "SAP Warehouse Info";
                            submitted = frItem.BBlockOnItem;
                            submittedDate = appItem.SAPWarehouseInfo_SubmittedDate;
                            break;
                        case "STANDARDCOSTENTRY":
                            labelText = "Standard Cost Set";
                            submitted = frItem.StandardCostSet;
                            submittedDate = appItem.StandardCostEntry_SubmittedDate;
                            break;
                        case "COSTFINISHEDGOOD":
                            submitted = frItem.BBlockOnItem;
                            break;
                        case "FINALCOSTINGREVIEW":
                            break;
                        case "PURCHASEPO":
                            submitted = frItem.POExists;
                            break;
                        case "REMOVESAPBLOCKS":
                            submitted = frItem.ZBlocksComplete;
                            break;
                        default:
                            labelText = "SAP Final Routings";
                            submitted = frItem.SAPRoutings;
                            submittedDate = appItem.SAPRoutingSetup_SubmittedDate;
                            break;
                    }
                    lblStatus.Text = labelText;
                    lblDate.Text = labelText;
                    if (!string.IsNullOrEmpty(submitted) && submitted.ToLower() == "y")
                    {
                        if (SAPTask.ToUpper() == "SAPWAREHOUSEINFO")
                        {
                            if (frItem.MRPType == "PD")
                            {
                                compStatus.CssClass = "greenClass";
                                lblSubmittedDate.Text = submittedDate;
                            }
                            else
                            {
                                compStatus.CssClass = "redClass";
                                lblSubmittedDate.Text = "Pending";
                            }
                        }
                        else
                        {
                            compStatus.CssClass = "greenClass";
                            lblSubmittedDate.Text = submittedDate;
                        }
                    }
                    else
                    {
                        compStatus.CssClass = "redClass";
                        lblSubmittedDate.Text = "Pending";
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

        #endregion

    }
}
