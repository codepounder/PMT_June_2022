using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.ComponentModel;
using Ferrara.Compass.Classes;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Services;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Enum;
using Microsoft.Practices.Unity;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ferrara.Compass.Layouts.Ferrara.Compass.AppPages
{
    public partial class CopyIPF : LayoutsPageBase
    {
        #region Member Variables
        private IStageGateGeneralService stageGateGeneralService;
        private IStageGateCreateProjectService stageGateCreateProjectService;
        private IDashboardService dashboardService;
        private IPackagingItemService packagingItemService;
        private IItemProposalService IPFService;
        private IUtilityService utilityService;
        private IExceptionService exceptionService;
        private IMixesService mixesService;
        private IShipperFinishedGoodService shipperService;
        private IConfigurationManagementService configurationService;
        private ISAPMaterialMasterService iSAPMaterialMasterService;
        private string webUrl = string.Empty;
        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            stageGateGeneralService = DependencyResolution.DependencyMapper.Container.Resolve<IStageGateGeneralService>();
            IPFService = DependencyResolution.DependencyMapper.Container.Resolve<IItemProposalService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            stageGateCreateProjectService = DependencyResolution.DependencyMapper.Container.Resolve<IStageGateCreateProjectService>();
            packagingItemService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            dashboardService = DependencyResolution.DependencyMapper.Container.Resolve<IDashboardService>();
            mixesService = DependencyResolution.DependencyMapper.Container.Resolve<IMixesService>();
            shipperService = DependencyResolution.DependencyMapper.Container.Resolve<IShipperFinishedGoodService>();
            configurationService = DependencyResolution.DependencyMapper.Container.Resolve<IConfigurationManagementService>();
            iSAPMaterialMasterService = DependencyResolution.DependencyMapper.Container.Resolve<ISAPMaterialMasterService>();
        }

        #region Properties
        private int StageGateListItemId
        {
            get
            {
                if (Request.QueryString[GlobalConstants.QUERYSTRING_PMTListItemId] != null)
                    return Convert.ToInt32(Request.QueryString[GlobalConstants.QUERYSTRING_PMTListItemId]);
                return 0;
            }
        }
        private string ParentProjectNo
        {
            get
            {
                if (Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo] != null)
                    return Convert.ToString(Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo]);
                return string.Empty;
            }
        }
        private string CopyMode
        {
            get
            {
                if (Request.QueryString[GlobalConstants.QUERYSTRING_CopyMode] != null)
                    return Convert.ToString(Request.QueryString[GlobalConstants.QUERYSTRING_CopyMode]);
                return string.Empty;
            }
        }
        private string ChildProjectNo
        {
            get
            {
                if (Request.QueryString["ChildProjectNo"] != null)
                    return Convert.ToString(Request.QueryString["ChildProjectNo"]);
                return string.Empty;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            webUrl = SPContext.Current.Web.Url;

            if (!Page.IsPostBack)
            {
                try
                {
                    this.Form.Page.Title = "CopyIPF";
                    hdnChildProjectNo.Value = ChildProjectNo;
                    hdnParentProjectNo.Value = ParentProjectNo;
                    hdnStageGateListItemId.Value = StageGateListItemId.ToString();
                }
                catch (Exception ex)
                {
                    exceptionService.Handle(LogCategory.CriticalError, ex, "CopyIPF", "Page_Load");
                }
            }
        }

        #region Button/Link Methods
        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
                Context.Response.Flush();
            }
            catch (Exception exception)
            {

            }
            finally
            {
                Context.Response.End();
            }
        }
        protected void btnLookupProjectNumber_Click(object sender, EventArgs e)
        {
            try
            {
                List<ItemProposalItem> searchResults = new List<ItemProposalItem>();
                if (string.IsNullOrEmpty(hdnChildProjectNo.Value))
                {
                    searchResults = stageGateGeneralService.GetSearchProjectName(txtSearchProjectNo.Text);
                }
                else
                {
                    searchResults = stageGateGeneralService.GetSearchParentProjectName(txtSearchProjectNo.Text);
                }
                rblRadioButtonList.Items.Clear();
                if (searchResults.Count <= 0)
                {
                    lblUploadError.Text = "No results found.";
                    lblSelect.Visible = false;
                    lblProjectName.Visible = false;
                }
                else
                {
                    lblUploadError.Text = "";
                    lblSelect.Visible = true;
                    lblProjectName.Visible = true;
                    foreach (ItemProposalItem ipfItem in searchResults)
                    {
                        ListItem rb = new ListItem();
                        if (string.IsNullOrEmpty(hdnChildProjectNo.Value))
                        {
                            rb.Text = ipfItem.ProjectNumber + ": " + ipfItem.SAPItemNumber + ": " + ipfItem.SAPDescription;
                            rb.Value = ipfItem.CompassListItemId.ToString();
                        }
                        else
                        {
                            rb.Text = ipfItem.ProjectNumber + ": " + ipfItem.SAPItemNumber;
                            rb.Value = ipfItem.ProjectNumber;
                        }
                        rblRadioButtonList.Items.Add(rb);
                    }
                }
            }
            catch (Exception exception)
            {

            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool error = false;
            string testProject = "";
            int SGSListItemId = 0;
            try
            {
                foreach (ListItem item in rblRadioButtonList.Items)
                {
                    if (item.Selected)
                    {
                        int oldCompassId = 0;
                        string newProjectNo = "";
                        ItemProposalItem IPFitem = new ItemProposalItem();
                        if (string.IsNullOrEmpty(hdnChildProjectNo.Value))
                        {
                            StageGateCreateProjectItem projectItem = stageGateCreateProjectService.GetStageGateProjectItem(Convert.ToInt32(hdnStageGateListItemId.Value));
                            testProject = projectItem.TestProject;
                            oldCompassId = Convert.ToInt32(item.Value);
                            newProjectNo = Utilities.GetNextChildProjectNumber(hdnParentProjectNo.Value);

                            IPFitem = IPFService.GetItemProposalItem(oldCompassId);
                            string oldProjectNumber = IPFitem.ProjectNumber;
                            IPFitem.StageGateProjectListItemId = Convert.ToInt32(hdnStageGateListItemId.Value);
                            SGSListItemId = Convert.ToInt32(hdnStageGateListItemId.Value);
                            IPFitem.ParentProjectNumber = hdnParentProjectNo.Value;
                            IPFitem.ProjectNumber = newProjectNo;
                            IPFitem.TestProject = testProject;
                            IPFitem.NewIPF = "Yes";
                            //if (IPFitem.TBDIndicator == "No")
                            //{
                            //    string SAPDescription = GetSAPDescriptionFromSAPMaterialMasters(IPFitem.SAPItemNumber);
                            //    IPFitem.SAPDescription = string.IsNullOrEmpty(SAPDescription) ? IPFitem.SAPDescription : SAPDescription;
                            //}
                            int childCount = newProjectNo.LastIndexOf("-");
                            string sortOrder = Regex.Replace(newProjectNo.Substring(childCount + 1), "[^0-9.]", "");
                            int order;
                            if (int.TryParse(sortOrder, out order))
                            {
                                if (order > 0)
                                {
                                    IPFitem.GenerateIPFSortOrder = order + 1;
                                }
                            }

                            //Clear Project Team
                            IPFitem.Initiator = SPContext.Current.Web.CurrentUser.ID.ToString() + ";#" + SPContext.Current.Web.CurrentUser.LoginName;
                            IPFitem.InitiatorName = SPContext.Current.Web.CurrentUser.Name.ToString();
                            string userTrimmed = Regex.Replace(SPContext.Current.Web.CurrentUser.ID.ToString(), "[^0-9.]", "");
                            int id;
                            if (int.TryParse(userTrimmed, out id))
                            {
                                if (id > 0)
                                {
                                    IPFitem.AllUsers = "," + id.ToString() + ",";
                                }
                            }
                            IPFitem.PM = string.Empty;
                            IPFitem.PMName = string.Empty;
                            IPFitem.Marketing = string.Empty;
                            IPFitem.MarketingName = string.Empty;
                            IPFitem.ProjectLeader = string.Empty;
                            IPFitem.ProjectLeaderName = string.Empty;
                            IPFitem.SrProjectManager = string.Empty;
                            IPFitem.SrProjectManagerName = string.Empty;
                            IPFitem.QA = string.Empty;
                            IPFitem.QAName = string.Empty;
                            IPFitem.InTech = string.Empty;
                            IPFitem.InTechName = string.Empty;
                            IPFitem.InTechRegulatory = string.Empty;
                            IPFitem.InTechRegulatoryName = string.Empty;
                            IPFitem.RegulatoryQA = string.Empty;
                            IPFitem.RegulatoryQAName = string.Empty;
                            IPFitem.PackagingEngineering = string.Empty;
                            IPFitem.PackagingEngineeringName = string.Empty;
                            IPFitem.SupplyChain = string.Empty;
                            IPFitem.SupplyChainName = string.Empty;
                            IPFitem.Finance = string.Empty;
                            IPFitem.FinanceName = string.Empty;
                            IPFitem.Sales = string.Empty;
                            IPFitem.SalesName = string.Empty;
                            IPFitem.Manufacturing = string.Empty;
                            IPFitem.ManufacturingName = string.Empty;
                            IPFitem.OtherTeamMembers = string.Empty;
                            IPFitem.OtherTeamMembersName = string.Empty;
                            IPFitem.LifeCycleManagement = string.Empty;
                            IPFitem.LifeCycleManagementName = string.Empty;
                            IPFitem.PackagingProcurement = string.Empty;
                            IPFitem.PackagingProcurementName = string.Empty;
                            IPFitem.ExtManufacturingProc = string.Empty;
                            IPFitem.ExtManufacturingProcName = string.Empty;

                            string PMTWorkflowVersion = configurationService.GetConfiguration(SystemConfiguration.PMTWorkflowVersion);
                            IPFitem.PMTWorkflowVersion = string.IsNullOrEmpty(PMTWorkflowVersion) ? 9999 : Convert.ToInt32(PMTWorkflowVersion);
                            int newCompassId = IPFService.InsertItemProposalItem(IPFitem);
                            IPFitem.CompassListItemId = newCompassId;
                            //Compass List 2
                            IPFService.InsertCompassList2(IPFitem);

                            IPFService.CopyCompassItem(oldCompassId, newCompassId, CopyMode, false);
                            IPFService.CopyCompassList2Item(oldCompassId, newCompassId, newProjectNo, CopyMode);
                            IPFService.CopyMarketingClaimsItem(oldCompassId, newCompassId, newProjectNo, CopyMode);
                            try
                            {
                                mixesService.CopyMixesItem(oldCompassId, newCompassId);
                            }
                            catch (Exception ex)
                            {
                                exceptionService.Handle(LogCategory.CriticalError, ex, "mixesService", "CopyMixesItem");
                            }
                            try
                            {
                                shipperService.CopyShipperFinishedGoodItem(oldCompassId, newCompassId);
                            }
                            catch (Exception ex)
                            {
                                exceptionService.Handle(LogCategory.CriticalError, ex, "shipperService", "CopyShipperFinishedGoodItem");
                            }
                            if (CopyMode == "CopyExistingIPF")
                            {
                                try
                                {
                                    packagingItemService.CopyIPFPackagingItems(oldCompassId, oldProjectNumber, IPFitem);
                                }
                                catch (Exception exception)
                                {
                                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "CopyIPFPackagingItems: " + exception.Message);
                                    exceptionService.Handle(LogCategory.CriticalError, exception, "Move", "CopyIPFPackagingItems");
                                }
                            }
                            else
                            {
                                packagingItemService.CopyPackagingItems(oldCompassId, oldProjectNumber, IPFitem);
                            }
                            ApprovalItem approvalItem = new ApprovalItem();
                            approvalItem.CompassListItemId = newCompassId;

                            // Set submitting user information
                            approvalItem.ModifiedDate = DateTime.Now.ToString();
                            approvalItem.ModifiedBy = SPContext.Current.Web.CurrentUser.Name;
                            // Insert Approval Info
                            IPFService.InsertApprovalItem(approvalItem, newProjectNo);
                            // Insert Project Decision Info
                            IPFService.InsertProjectDecisionItem(newCompassId, newProjectNo);
                            // Insert Email Logging Info
                            IPFService.InsertEmailLoggingItem(newCompassId, newProjectNo);
                            // Insert Workflow Status Info
                            IPFService.InsertWorkflowStatusItem(newCompassId, newProjectNo);
                            Context.Response.Write("<script type='text/javascript'>window.top.location.assign('" + Utilities.RedirectPageForm(GlobalConstants.PAGE_StageGateGenerateIPFs, hdnParentProjectNo.Value) + "');</script>");
                        }
                        else
                        {
                            oldCompassId = Convert.ToInt32(hdnChildProjectNo.Value);
                            newProjectNo = Utilities.GetNextChildProjectNumber(item.Value);
                            IPFitem = IPFService.GetItemProposalItem(oldCompassId);
                            SGSListItemId = Utilities.GetItemIdByProjectNumberFromStageGateProjectList(item.Value);
                            IPFitem.StageGateProjectListItemId = SGSListItemId;
                            IPFitem.ParentProjectNumber = item.Value;
                            IPFitem.ProjectNumber = newProjectNo;
                            IPFitem.TestProject = IPFitem.TestProject;

                            int childCount = newProjectNo.LastIndexOf("-");
                            string sortOrder = Regex.Replace(newProjectNo.Substring(childCount + 1), "[^0-9.]", "");
                            int sort;
                            if (int.TryParse(sortOrder, out sort))
                            {
                                if (sort > 0)
                                {
                                    IPFitem.GenerateIPFSortOrder = sort + 1;
                                }
                            }
                            //if (IPFitem.TBDIndicator == "No")
                            //{
                            //    string SAPDescription = GetSAPDescriptionFromSAPMaterialMasters(IPFitem.SAPItemNumber);
                            //    IPFitem.SAPDescription = string.IsNullOrEmpty(SAPDescription) ? IPFitem.SAPDescription : SAPDescription;
                            //}
                            //Clear Project Team
                            IPFitem.Initiator = SPContext.Current.Web.CurrentUser.ID.ToString() + ";#" + SPContext.Current.Web.CurrentUser.LoginName;
                            IPFitem.InitiatorName = SPContext.Current.Web.CurrentUser.Name.ToString();
                            string userTrimmed = Regex.Replace(SPContext.Current.Web.CurrentUser.ID.ToString(), "[^0-9.]", "");
                            int id;
                            if (int.TryParse(userTrimmed, out id))
                            {
                                if (id > 0)
                                {
                                    IPFitem.AllUsers = "," + id.ToString() + ",";
                                }
                            }
                            IPFitem.PM = string.Empty;
                            IPFitem.PMName = string.Empty;
                            IPFitem.Marketing = string.Empty;
                            IPFitem.MarketingName = string.Empty;
                            IPFitem.InTech = string.Empty;
                            IPFitem.InTechName = string.Empty;
                            IPFitem.ProjectLeader = string.Empty;
                            IPFitem.ProjectLeaderName = string.Empty;
                            IPFitem.SrProjectManager = string.Empty;
                            IPFitem.SrProjectManagerName = string.Empty;
                            IPFitem.QA = string.Empty;
                            IPFitem.QAName = string.Empty; ;
                            IPFitem.InTechRegulatory = string.Empty;
                            IPFitem.InTechRegulatoryName = string.Empty;
                            IPFitem.RegulatoryQA = string.Empty;
                            IPFitem.RegulatoryQAName = string.Empty;
                            IPFitem.PackagingEngineering = string.Empty;
                            IPFitem.PackagingEngineeringName = string.Empty;
                            IPFitem.SupplyChain = string.Empty;
                            IPFitem.SupplyChainName = string.Empty;
                            IPFitem.Finance = string.Empty;
                            IPFitem.FinanceName = string.Empty;
                            IPFitem.Sales = string.Empty;
                            IPFitem.SalesName = string.Empty;
                            IPFitem.Manufacturing = string.Empty;
                            IPFitem.ManufacturingName = string.Empty;
                            IPFitem.OtherTeamMembers = string.Empty;
                            IPFitem.OtherTeamMembersName = string.Empty;
                            IPFitem.LifeCycleManagement = string.Empty;
                            IPFitem.LifeCycleManagementName = string.Empty;
                            IPFitem.PackagingProcurement = string.Empty;
                            IPFitem.PackagingProcurementName = string.Empty;
                            IPFitem.ExtManufacturingProc = string.Empty;
                            IPFitem.ExtManufacturingProcName = string.Empty;
                            string PMTWorkflowVersion = configurationService.GetConfiguration(SystemConfiguration.PMTWorkflowVersion);
                            IPFitem.PMTWorkflowVersion = string.IsNullOrEmpty(PMTWorkflowVersion) ? 9999 : Convert.ToInt32(PMTWorkflowVersion);
                            int newCompassId = IPFService.InsertItemProposalItem(IPFitem);
                            //Compass List 2
                            IPFService.InsertCompassList2(IPFitem);
                            IPFService.CopyCompassItem(oldCompassId, newCompassId, CopyMode, false);
                            IPFService.CopyCompassList2Item(oldCompassId, newCompassId, newProjectNo, CopyMode);
                            IPFService.CopyMarketingClaimsItem(oldCompassId, newCompassId, newProjectNo, string.Empty);
                            try
                            {
                                mixesService.CopyMixesItem(oldCompassId, newCompassId);
                            }
                            catch (Exception ex)
                            {
                                exceptionService.Handle(LogCategory.CriticalError, ex, "mixesService", "CopyMixesItem");
                            }
                            try
                            {
                                shipperService.CopyShipperFinishedGoodItem(oldCompassId, newCompassId);
                            }
                            catch (Exception ex)
                            {
                                exceptionService.Handle(LogCategory.CriticalError, ex, "mixesService", "CopyMixesItem");
                            }
                            try
                            {
                                if (CopyMode == "CopyExistingIPF")
                                {
                                    packagingItemService.CopyIPFPackagingItems(oldCompassId, newProjectNo, IPFitem);
                                }
                                else
                                {
                                    packagingItemService.CopyPackagingItems(oldCompassId, newProjectNo, IPFitem);
                                }
                            }
                            catch (Exception exception)
                            {
                                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "CopyIPFPackagingItems: " + exception.Message);
                                exceptionService.Handle(LogCategory.CriticalError, exception, "Move", "CopyIPFPackagingItems");
                            }
                            
                            ApprovalItem approvalItem = new ApprovalItem();
                            approvalItem.CompassListItemId = newCompassId;

                            // Set submitting user information
                            approvalItem.ModifiedDate = DateTime.Now.ToString();
                            approvalItem.ModifiedBy = SPContext.Current.Web.CurrentUser.Name;
                            // Insert Approval Info
                            IPFService.InsertApprovalItem(approvalItem, newProjectNo);
                            // Insert Project Decision Info
                            IPFService.InsertProjectDecisionItem(newCompassId, newProjectNo);
                            // Insert Email Logging Info
                            IPFService.InsertEmailLoggingItem(newCompassId, newProjectNo);
                            // Insert Workflow Status Info
                            IPFService.InsertWorkflowStatusItem(newCompassId, newProjectNo);
                            Context.Response.Write("<script type='text/javascript'>window.top.location.assign('" + Utilities.RedirectPageForm(GlobalConstants.PAGE_StageGateGenerateIPFs, item.Value) + "');</script>");
                        }


                        //Context.Response.Flush();
                    }
                }
                if (rblRadioButtonList.SelectedValue == "")
                {
                    lblUploadError.Text = "Please select a project to copy.";
                    error = true;
                }
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Move BOM Form: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "Move", "btnSubmit_Click");
            }
            finally
            {
                if (!error)
                {
                    if (SGSListItemId != 0)
                    {
                        List<ItemProposalItem> childProjectData = dashboardService.getRequestedChildProjectDetails(SGSListItemId);
                        if (childProjectData.Count <= 0)
                        {
                            dashboardService.setGenerateIPFStartDate(Convert.ToInt32(SGSListItemId));
                        }
                        Context.Response.End();
                    }
                }
            }
        }
        #endregion
        //private string GetSAPDescriptionFromSAPMaterialMasters(string FGNumber)
        //{
        //    string SAPDesc = "";
        //    try
        //    {
        //        SAPMaterialMasterListItem mmItem = iSAPMaterialMasterService.GetSAPMaterialMaster(FGNumber);
        //        if ((mmItem != null))
        //        {
        //            SAPDesc = mmItem.SAPDescription;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        exceptionService.Handle(LogCategory.CriticalError, ex, "CopyIPF", "GetSAPDescriptionFromSAPMaterialMasters");
        //    }
        //    return SAPDesc;
        //}
    }
}
