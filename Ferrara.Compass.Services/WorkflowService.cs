using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using System.Collections;
using System.Threading;

namespace Ferrara.Compass.Services
{
    public class WorkflowService : IWorkflowService
    {
        #region Member Variables
        private readonly IExceptionService exceptionService;
        private readonly IUserManagementService userMgmtService;
        private readonly IUtilityService utilityService;
        private readonly INotificationService notificationService;
        private readonly IEmailLoggingService emailLoggingService;
        private readonly IPackagingItemService packagingItemService;
        private readonly IApprovalService approvalService;
        #endregion

        #region Constructors
        static WorkflowService()
        {
        }

        public WorkflowService(IExceptionService exceptionService, IUserManagementService userManagementService, IUtilityService utilityService,
                INotificationService notificationService, IEmailLoggingService emailLoggingService, IPackagingItemService piService, IApprovalService appService)
        {
            this.exceptionService = exceptionService;
            this.userMgmtService = userManagementService;
            this.utilityService = utilityService;
            this.notificationService = notificationService;
            this.emailLoggingService = emailLoggingService;
            this.packagingItemService = piService;
            this.approvalService = appService;
        }
        #endregion

        #region Get/Set Data Methods
        private CompassListItem GetCompassData(int itemId)
        {
            var sgItem = new CompassListItem();
            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    var item = spList.GetItemById(itemId);
                    if (item != null)
                    {
                        sgItem.CompassListItemId = item.ID;
                        sgItem.SAPItemNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                        sgItem.SAPDescription = Convert.ToString(item[CompassListFields.SAPDescription]);
                        sgItem.ProjectNumber = Convert.ToString(item[CompassListFields.ProjectNumber]);
                        sgItem.MaterialGroup1Brand = Convert.ToString(item[CompassListFields.MaterialGroup1Brand]);
                        sgItem.TBDIndicator = Convert.ToString(item[CompassListFields.TBDIndicator]);
                        sgItem.ManufacturingLocation = Convert.ToString(item[CompassListFields.ManufacturingLocation]);
                        sgItem.PackingLocation = Convert.ToString(item[CompassListFields.PackingLocation]);
                        sgItem.ProjectType = Convert.ToString(item[CompassListFields.ProjectType]);
                        sgItem.PM = Convert.ToString(item[CompassListFields.PM]);
                        sgItem.RevisedFirstShipDate = Convert.ToDateTime(item[CompassListFields.RevisedFirstShipDate]);
                        //sgItem.REPORT_ProjectStatus = Convert.ToString(item[CompassListFields.REPORT_ProjectStatus]);
                        //sgItem.OBM_PackageEngineer = Convert.ToString(item[CompassListFields.OBM_PackageEngineer]);
                        sgItem.OBM_PackagingNumbers = Convert.ToString(item[CompassListFields.PackagingNumbers]);
                        sgItem.ProductHierarchyLevel1 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]);
                        sgItem.ProductHierarchyLevel2 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel2]);
                    }
                }
            }
            return sgItem;
        }
        public void UpdateWorkflowPhase(int itemId, string wfPhase)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        var item = spList.GetItemById(itemId);
                        if (item != null)
                        {
                            string OnHoldWFPhase = string.Empty;
                            if (string.Equals(wfPhase, "On Hold"))
                            {
                                if (string.Equals(Convert.ToString(item[CompassListFields.WorkflowPhase]), GlobalConstants.WORKFLOWPHASE_PreProduction))
                                {
                                    OnHoldWFPhase = GlobalConstants.WORKFLOWPHASE_PreProduction;
                                }
                            }
                            else if (string.Equals(wfPhase, "Remove On Hold"))
                            {
                                wfPhase = Convert.ToString(item[CompassListFields.WorkflowPhase]);
                                if (string.Equals(Convert.ToString(item[CompassListFields.OnHoldWorkflowPhase]), GlobalConstants.WORKFLOWPHASE_PreProduction))
                                {
                                    wfPhase = GlobalConstants.WORKFLOWPHASE_PreProduction;
                                }
                            }

                            item[CompassListFields.OnHoldWorkflowPhase] = OnHoldWFPhase;
                            item[CompassListFields.WorkflowPhase] = wfPhase;
                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            item.Update();

                            if (string.Equals("Completed", wfPhase) || string.Equals("Cancelled", wfPhase) || string.Equals("On Hold", wfPhase))
                            {
                                string strWFTemplateId = GetSPWFTemplateIdBasedOnName(spList, "PMT Cancel Workflows");
                                SPWorkflowAssociation spwaDocLib = spList.WorkflowAssociations[new Guid(strWFTemplateId)];
                                spSite.WorkflowManager.StartWorkflow(item, spwaDocLib, spwaDocLib.AssociationData, true);
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        public void StartCancelWorkflow(int itemId)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        var item = spList.GetItemById(itemId);
                        if (item != null)
                        {
                            string strWFTemplateId = GetSPWFTemplateIdBasedOnName(spList, "PMT Cancel Workflows");
                            SPWorkflowAssociation spwaDocLib = spList.WorkflowAssociations[new Guid(strWFTemplateId)];
                            spSite.WorkflowManager.StartWorkflow(item, spwaDocLib, spwaDocLib.AssociationData, true);
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        public void UpdateWorkflowPhaseForChangeRequest(int itemId, string wfPhase, Microsoft.SharePoint.SPFieldUrlValue ChangeRequestLink)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        var item = spList.GetItemById(itemId);
                        if (item != null)
                        {
                            item[CompassListFields.WorkflowPhase] = wfPhase;
                            item[CompassListFields.ChangeRequestNewProjectLink] = ChangeRequestLink;
                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            item.Update();

                            if (string.Equals("Cancelled", wfPhase))
                            {
                                string strWFTemplateId = GetSPWFTemplateIdBasedOnName(spList, "PMT Cancel Workflows");
                                SPWorkflowAssociation spwaDocLib = spList.WorkflowAssociations[new Guid(strWFTemplateId)];
                                spSite.WorkflowManager.StartWorkflow(item, spwaDocLib, spwaDocLib.AssociationData, true);
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        private string GetSPWFTemplateIdBasedOnName(SPList spList, string wfName)
        {
            string strWFTemplateId = string.Empty;
            try
            {
                SPWorkflowAssociationCollection spwfAssociations = spList.WorkflowAssociations;
                foreach (SPWorkflowAssociation spwfAssociation in spwfAssociations)
                {
                    if (spwfAssociation.Name.ToLower() == wfName.ToLower())
                    {
                        strWFTemplateId = spwfAssociation.Id.ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "WorkflowService", "GetSPWFTemplateIdBasedOnName", "Workflow Name: " + wfName);
            }
            return strWFTemplateId;
        }
        private void GetApprovalFieldNamesByFormName(string formName, ref string submittedBy, ref string submittedDate)
        {
            Dictionary<string, string> submittedColumns;
            string columns;
            string[] columnNames;
            submittedColumns = new Dictionary<string, string>(22);
            submittedColumns.Add(GlobalConstants.PAGE_ItemProposal.ToLower(), ApprovalListFields.IPF_SubmittedBy + "/" + ApprovalListFields.IPF_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_InitialApprovalReview.ToLower(), ApprovalListFields.SrOBMApproval_SubmittedBy + "/" + ApprovalListFields.SrOBMApproval_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_TradePromoGroup.ToLower(), ApprovalListFields.TradePromo_SubmittedBy + "/" + ApprovalListFields.TradePromo_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_EstPricing.ToLower(), ApprovalListFields.EstPricing_SubmittedBy + "/" + ApprovalListFields.EstPricing_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_EstBracketPricing.ToLower(), ApprovalListFields.EstBracketPricing_SubmittedBy + "/" + ApprovalListFields.EstBracketPricing_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_Distribution.ToLower(), ApprovalListFields.Distribution_SubmittedBy + "/" + ApprovalListFields.Distribution_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_OPS.ToLower(), ApprovalListFields.Operations_SubmittedBy + "/" + ApprovalListFields.Operations_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_ExternalManufacturing.ToLower(), ApprovalListFields.ExternalMfg_SubmittedBy + "/" + ApprovalListFields.ExternalMfg_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_InitialCostingReview.ToLower(), ApprovalListFields.InitialCosting_SubmittedBy + "/" + ApprovalListFields.InitialCosting_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_SAPInitialItemSetup.ToLower(), ApprovalListFields.SAPInitialSetup_SubmittedBy + "/" + ApprovalListFields.SAPInitialSetup_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_QA.ToLower(), ApprovalListFields.QA_SubmittedBy + "/" + ApprovalListFields.QA_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_OBMFirstReview.ToLower(), ApprovalListFields.OBMReview1_SubmittedBy + "/" + ApprovalListFields.OBMReview1_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_OBMSecondReview.ToLower(), ApprovalListFields.OBMReview2_SubmittedBy + "/" + ApprovalListFields.OBMReview2_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_BillofMaterialSetUpPE.ToLower(), ApprovalListFields.BOMSetupPE_SubmittedBy + "/" + ApprovalListFields.BOMSetupPE_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_BillofMaterialSetUpPE2.ToLower(), ApprovalListFields.BOMSetupPE2_SubmittedBy + "/" + ApprovalListFields.BOMSetupPE2_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_BillofMaterialSetUpProc.ToLower(), ApprovalListFields.BOMSetupProc_SubmittedBy + "/" + ApprovalListFields.BOMSetupProc_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_SAPBOMSetup.ToLower(), ApprovalListFields.SAPBOMSetup_SubmittedBy + "/" + ApprovalListFields.SAPBOMSetup_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_SecondaryApprovalReview.ToLower(), ApprovalListFields.SrOBMApproval2_SubmittedBy + "/" + ApprovalListFields.SrOBMApproval2_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_FinishedGoodPackSpec.ToLower(), ApprovalListFields.FGPackSpec_SubmittedBy + "/" + ApprovalListFields.FGPackSpec_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_ComponentCosting.ToLower(), ApprovalListFields.CompCostCorrPaper_SubmittedBy + "/" + ApprovalListFields.CompCostCorrPaper_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_GraphicsRequest.ToLower(), ApprovalListFields.GRAPHICS_SubmittedBy + "/" + ApprovalListFields.GRAPHICS_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_MaterialsReceivedCheck.ToLower(), ApprovalListFields.MaterialsReceivedChk_SubmittedBy + "/" + ApprovalListFields.MaterialsReceivedChk_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_FirstProductionCheck.ToLower(), ApprovalListFields.FirstProductionChk_SubmittedBy + "/" + ApprovalListFields.FirstProductionChk_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_DistributionCenterCheck.ToLower(), ApprovalListFields.DistributionCenterChk_SubmittedBy + "/" + ApprovalListFields.DistributionCenterChk_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_PrelimSAPInitialItemSetup.ToLower(), ApprovalListFields.PrelimSAPInitialSetup_SubmittedBy + "/" + ApprovalListFields.PrelimSAPInitialSetup_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_PE.ToLower(), ApprovalListFields.BOMSetupPE_SubmittedBy + "/" + ApprovalListFields.BOMSetupPE_SubmittedBy);
            submittedColumns.Add(GlobalConstants.PAGE_PE2.ToLower(), ApprovalListFields.BOMSetupPE2_SubmittedBy + "/" + ApprovalListFields.BOMSetupPE2_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_Proc.ToLower(), ApprovalListFields.BOMSetupProc_SubmittedBy + "/" + ApprovalListFields.BOMSetupProc_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_BOMSetupPE3.ToLower(), ApprovalListFields.BOMSetupPE3_SubmittedBy + "/" + ApprovalListFields.BOMSetupPE3_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_BOMSetupMaterialWarehouse.ToLower(), ApprovalListFields.BOMSetupMaterialWarehouse_SubmittedBy + "/" + ApprovalListFields.BOMSetupMaterialWarehouse_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_SAPCompleteItemSetup.ToLower(), ApprovalListFields.SAPCompleteItemSetup_SubmittedBy + "/" + ApprovalListFields.SAPCompleteItemSetup_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_BOMSetupSAP.ToLower(), ApprovalListFields.SAPBOMSetup_SubmittedBy + "/" + ApprovalListFields.SAPBOMSetup_SubmittedDate);
            submittedColumns.Add(GlobalConstants.PAGE_BEQRC.ToLower(), ApprovalListFields.BEQRC_SubmittedBy + "/" + ApprovalListFields.BEQRC_SubmittedDate);
            try
            {
                columns = submittedColumns[formName.ToLower()];
                columnNames = columns.Split('/');
                submittedBy = columnNames[0];
                submittedDate = columnNames[1];
            }
            catch (KeyNotFoundException e) { return; }
        }
        public ApprovalItem GetApprovalItemByFormName(string projectNumber, string formName)
        {
            SPList spList;
            SPItem spItem;
            SPListItemCollection collection;
            ApprovalItem approval = null;
            SPQuery query;
            string submittedBy = null, submittedDate = null;
            GetApprovalFieldNamesByFormName(formName, ref submittedBy, ref submittedDate);
            if (submittedBy == null || submittedDate == null)
                return null;
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    if (formName.ToLower() == GlobalConstants.PAGE_BEQRC.ToLower() || formName.ToLower() == GlobalConstants.PAGE_EstPricing.ToLower() || formName.ToLower() == GlobalConstants.PAGE_EstBracketPricing.ToLower())
                    {
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalList2Name);
                    }
                    else
                    {
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);
                    }
                    query = new SPQuery();
                    query.Query = "<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + projectNumber + "</Value></Eq></Where>";
                    query.RowLimit = 1;
                    collection = spList.GetItems(query);
                    if (collection.Count == 0)
                        return null;
                    spItem = collection[0];
                    if (spItem[submittedBy] == null || spItem[submittedDate] == null)
                        return null;
                    approval = new ApprovalItem();
                    approval.SubmittedBy = Convert.ToString(spItem[submittedBy]);
                    approval.SubmittedDate = Convert.ToString(spItem[submittedDate]);
                }
            }
            return approval;
        }
        #endregion

        #region Workflow Tasks Methods
        public void CreateWorkflowTask(int itemId, WorkflowStep currentWFStep, string emails)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        try
                        {
                            // Get Compass Data for Workflow task
                            CompassListItem sgItem = GetCompassData(itemId);
                            SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_WorkflowTaskListName);

                            // Check to make sure there is not an existing task for this step already
                            SPQuery spQuery = new SPQuery();
                            spQuery.Query = "<Where><And><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId.ToString() + "</Value></Eq><Eq><FieldRef Name=\"WorkflowStep\" /><Value Type=\"Text\">" + currentWFStep.ToString() + "</Value></Eq></And><Neq><FieldRef Name=\"Status\" /><Value Type=\"Text\">Completed</Value></Neq></And></Where>";
                            SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                            if (compassItemCol != null && compassItemCol.Count > 0)
                            {
                                // Current Task found
                                exceptionService.Handle(LogCategory.General, "Duplicate Task Found!", "Workflow Service", "CreateWorkflowTask", currentWFStep.ToString());
                            }
                            else
                            {
                                var wfAllSteps = utilityService.GetWorkflowSteps();
                                var ipfWfStep = wfAllSteps.FirstOrDefault(x => x.WorkflowStep.Equals(currentWFStep));

                                // No Task exists, create a new one
                                SPListItem item = spList.AddItem();

                                item["Title"] = ipfWfStep.WorkflowStepTaskDesc;
                                item[SPBuiltInFieldId.AssignedTo] = SPContext.Current.Web.CurrentUser;
                                item[SPBuiltInFieldId.TaskStatus] = "In Progress";
                                item[SPBuiltInFieldId.Priority] = "(2) Normal";
                                item[SPBuiltInFieldId.PercentComplete] = 0.0;

                                // Compass fields
                                item["SAPItemNumber"] = sgItem.SAPItemNumber;
                                item["SAPDescription"] = sgItem.SAPDescription;
                                item["ProjectNumber"] = sgItem.ProjectNumber;
                                item["WorkflowStep"] = currentWFStep.ToString();
                                item["CompassListItemId"] = sgItem.CompassListItemId;
                                item["RevisedFirstShipDate"] = sgItem.RevisedFirstShipDate;
                                //item["ProjectStatus"] = sgItem.ProjectStatus;
                                item["OBM"] = utilityService.GetPersonFieldForDisplay(sgItem.PM);
                                item["PackagingEngineer"] = utilityService.GetPersonFieldForDisplay(sgItem.OBM_PackagingEngineerUser);
                                item["PackagingNumbers"] = sgItem.OBM_PackagingNumbers;
                                item["SeasonType"] = sgItem.ProductHierarchyLevel2;

                                SPFieldUrlValue value = new SPFieldUrlValue();
                                value.Description = sgItem.ProjectNumber;
                                value.Url = string.Concat(SPContext.Current.Web.Url, "/pages/", ipfWfStep.PageName, "?", GlobalConstants.QUERYSTRING_ProjectNo, "=", sgItem.ProjectNumber);
                                item["PageLink"] = value;

                                value = new SPFieldUrlValue();
                                value.Description = sgItem.ProjectNumber;
                                value.Url = string.Concat(SPContext.Current.Web.Url, "/pages/", GlobalConstants.PAGE_CommercializationItemSummary, "?", GlobalConstants.QUERYSTRING_ProjectNo, "=", sgItem.ProjectNumber);
                                item["CommercializationLink"] = value;

                                item.Update();
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptionService.Handle(LogCategory.CriticalError, ex, "WorkflowService", "CreateWorkflowTask", "Current Workflow Step: " + currentWFStep);
                        }

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public bool CompleteWorkflowTask(int itemId, WorkflowStep currentWFStep)
        {
            SPUser user = SPContext.Current.Web.CurrentUser;
            bool completed = false;
            try
            {
                completed = CompleteWorkflowTask(itemId, currentWFStep.ToString(), GlobalConstants.LIST_WorkflowTaskListName1, true);
                if (!completed)
                    completed = CompleteWorkflowTask(itemId, currentWFStep.ToString(), GlobalConstants.LIST_WorkflowTaskListName2, true);
                if (!completed)
                    completed = CompleteWorkflowTask(itemId, currentWFStep.ToString(), GlobalConstants.LIST_WorkflowTaskListName3, true);
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "WorkflowService", "CompleteWorkflowTask(int itemId, WorkflowStep currentWFStep)", "Current Workflow Step: " + currentWFStep);
            }

            if (currentWFStep == WorkflowStep.BOMSetupPE2 && userMgmtService.IsCurrentUserInGroup(GlobalConstants.GROUP_PackagingEngineer))
                completed = CompleteWorkflowTask(itemId, currentWFStep.ToString(), GlobalConstants.LIST_WorkflowTaskListName2, false);
            else if (currentWFStep == WorkflowStep.IPF)
                completed = CompleteWorkflowTask(itemId, currentWFStep.ToString(), GlobalConstants.LIST_WorkflowTaskListName1, false);
            else if (userMgmtService.IsCurrentUserInGroup(GlobalConstants.GROUP_OBMAdmins))
            {
                completed = CompleteWorkflowTask(itemId, currentWFStep.ToString(), GlobalConstants.LIST_WorkflowTaskListName1, false);
                if (!completed)
                    completed = CompleteWorkflowTask(itemId, currentWFStep.ToString(), GlobalConstants.LIST_WorkflowTaskListName2, false);
                if (!completed)
                    completed = CompleteWorkflowTask(itemId, currentWFStep.ToString(), GlobalConstants.LIST_WorkflowTaskListName3, false);
            }
            return completed;
        }
        private bool CompleteWorkflowTask(int compassListId, string workflowStep)
        {
            bool completed = false;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        try
                        {
                            SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_WorkflowTaskListName);
                            SPQuery spQuery = new SPQuery();
                            spQuery.Query = "<Where><And><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassListId.ToString() + "</Value></Eq><Eq><FieldRef Name=\"WorkflowStep\" /><Value Type=\"Text\">" + workflowStep + "</Value></Eq></And><Neq><FieldRef Name=\"Status\" /><Value Type=\"Text\">Completed</Value></Neq></And></Where>";
                            SPListItemCollection compassItemCol = spList.GetItems(spQuery);

                            if (compassItemCol != null && compassItemCol.Count > 0)
                            {
                                foreach (SPListItem item in compassItemCol)
                                {
                                    item["Status"] = "Completed";
                                    item[SPBuiltInFieldId.PercentComplete] = 1.0;
                                    item[SPBuiltInFieldId.Completed] = true;
                                    item[SPBuiltInFieldId.TaskStatus] = "Completed";
                                    item["TaskStatus"] = "#";
                                    item.Update();
                                    completed = true;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptionService.Handle(LogCategory.CriticalError, ex, "WorkflowService", "CompleteWorkflowTask(int compassListId, string workflowStep)", string.Concat("Current Workflow Step: ", workflowStep, "CompassListItemId: ", compassListId.ToString()));
                        }

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return completed;
        }
        private bool CompleteWorkflowTask(int compassListId, string workflowStep, string workflowTaskList, bool currentUser)
        {
            string assignedTo;
            bool markCompleted, completed = false;
            SPUser user = SPContext.Current.Web.CurrentUser;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        try
                        {
                            exceptionService.Handle(LogCategory.General, "Completing other workflow tasks", "WorkflowService", "CompleteWorkflowTask", string.Concat("Current Workflow Step: ", workflowStep, " CompassListItemId: ", compassListId.ToString(), " List: ", workflowTaskList));

                            SPList spList = spWeb.Lists.TryGetList(workflowTaskList);
                            SPQuery spQuery = new SPQuery();
                            spQuery.Query = "<Where><And><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassListId.ToString() + "</Value></Eq><Eq><FieldRef Name=\"WorkflowStep\" /><Value Type=\"Text\">" + workflowStep + "</Value></Eq></And><Neq><FieldRef Name=\"Status\" /><Value Type=\"Text\">Completed</Value></Neq></And></Where>";
                            SPListItemCollection compassItemCol = spList.GetItems(spQuery);

                            if (compassItemCol != null && compassItemCol.Count > 0)
                            {
                                foreach (SPListItem item in compassItemCol)
                                {
                                    assignedTo = Convert.ToString(item["Assigned To"]);
                                    markCompleted = currentUser ? assignedTo.Contains(user.Name) : !assignedTo.Contains(user.Name);
                                    if (markCompleted)
                                    {
                                        item["Status"] = "Completed";
                                        item[SPBuiltInFieldId.PercentComplete] = 1.0;
                                        item[SPBuiltInFieldId.Completed] = true;
                                        completed = true;
                                        item.Update();
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptionService.Handle(LogCategory.CriticalError, ex, "WorkflowService", "CompleteWorkflowTaskError(int compassListId, string workflowStep)", string.Concat("Current Workflow Step: ", workflowStep, "CompassListItemId: ", compassListId.ToString()));
                        }

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return completed;
        }
        public void UpdateWorkflowTaskFirstShipDate(int itemId, string firstShipDate)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        try
                        {
                            SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                            SPListItem item = spList.GetItemById(itemId);
                            if (item != null)
                            {
                                foreach (SPWorkflowTask wfTask in item.Tasks)
                                {
                                    wfTask["FirstShipDate"] = firstShipDate;
                                    wfTask.Update();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptionService.Handle(LogCategory.CriticalError, ex, "WorkflowService", "UpdateWorkflowTaskDueDate", "CompassListItemId: " + itemId.ToString());
                        }

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        #endregion

        #region WorldSyncRequest
        public void StartWorkflowWorldSyncRequest(int requestId)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                SPWorkflowAssociation spwaDocLib;
                string strWFTemplateId;
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_WorldSyncRequestList);

                        var item = spList.GetItemById(requestId);
                        if (item != null)
                        {
                            strWFTemplateId = GetSPWFTemplateIdBasedOnName(spList, "WorldSyncRequestProcess");
                            spwaDocLib = spList.WorkflowAssociations[new Guid(strWFTemplateId)];
                            spSite.WorkflowManager.StartWorkflow(item, spwaDocLib, spwaDocLib.AssociationData, true);
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public bool CompleteWorldSyncReqWorkflowTask(int requestId, string currentWFStep)
        {
            SPUser user = SPContext.Current.Web.CurrentUser;
            bool completed = false;
            try
            {
                completed = CompleteWorldSyncReqWorkflowTask(requestId, currentWFStep, true);
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "WorkflowService", "CompleteWorldSyncReqWorkflowTask(int requestId, string workflowStep)", "Current Workflow Step: " + currentWFStep);
            }
            completed = CompleteWorldSyncReqWorkflowTask(requestId, currentWFStep, false);
            return completed;
        }
        private bool CompleteWorldSyncReqWorkflowTask(int requestId, string workflowStep, bool currentUser)
        {
            string assignedTo;
            bool markCompleted, completed = false;
            SPUser user = SPContext.Current.Web.CurrentUser;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        try
                        {
                            //exceptionService.Handle(LogCategory.General, "Completing other workflow tasks", "WorkflowService", "CompleteWorkflowTask", string.Concat("Current Workflow Step: ", workflowStep, " CompassListItemId: ", requestId.ToString(), " List: ", GlobalConstants.LIST_WorkflowTaskListName3));

                            SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_WorldSyncRequestTasks);
                            SPQuery spQuery = new SPQuery();
                            spQuery.Query = "<Where><And><And><Eq><FieldRef Name=\"RequestId\" /><Value Type=\"Number\">" + requestId.ToString() + "</Value></Eq><Eq><FieldRef Name=\"WorkflowStep\" /><Value Type=\"Text\">" + workflowStep + "</Value></Eq></And><Neq><FieldRef Name=\"Status\" /><Value Type=\"Text\">Completed</Value></Neq></And></Where>";
                            SPListItemCollection compassItemCol = spList.GetItems(spQuery);

                            if (compassItemCol != null && compassItemCol.Count > 0)
                            {
                                foreach (SPListItem item in compassItemCol)
                                {
                                    assignedTo = Convert.ToString(item["Assigned To"]);
                                    markCompleted = currentUser ? assignedTo.Contains(user.Name) : !assignedTo.Contains(user.Name);
                                    if (markCompleted)
                                    {
                                        item["Status"] = "Completed";
                                        item[SPBuiltInFieldId.PercentComplete] = 1.0;
                                        item[SPBuiltInFieldId.Completed] = true;
                                        completed = true;
                                        item.Update();
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptionService.Handle(LogCategory.CriticalError, ex, "WorkflowService", "CompleteWorldSyncReqWorkflowTask(int requestId, string workflowStep, bool currentUser)", string.Concat("Current Workflow Step: ", workflowStep, "requestId: ", requestId.ToString()));
                        }

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return completed;
        }
        public HashSet<int> GetRequestIdAssignedToCurrentUser()
        {
            SPList spList;
            SPListItemCollection collection;
            HashSet<int> list;
            SPQuery query;
            list = new HashSet<int>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_WorldSyncRequestTasks);
                    query = new SPQuery();
                    query.Query = "<Where><And><Eq><FieldRef Name=\"AssignedTo\" LookupId=\"TRUE\" /><Value Type=\"Integer\"><UserID /></Value></Eq>" +
                        "<Neq><FieldRef Name=\"Status\" /><Value Type=\"Text\">Completed</Value></Neq></And></Where>";
                    collection = spList.GetItems(query);
                    foreach (SPItem item in collection)
                        list.Add(Convert.ToInt32(item["RequestId"]));
                }
            }
            return list;
        }
        public HashSet<int> GetRequestIdAssigned()
        {
            SPList spList;
            SPListItemCollection collection;
            HashSet<int> list;
            SPQuery query;
            list = new HashSet<int>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_WorldSyncRequestTasks);
                    query = new SPQuery();
                    query.Query = "<Where><Neq><FieldRef Name=\"Status\" /><Value Type=\"Text\">Completed</Value></Neq></Where>";
                    collection = spList.GetItems(query);
                    foreach (SPItem item in collection)
                        list.Add(Convert.ToInt32(item["RequestId"]));
                }
            }
            return list;
        }
        #endregion
        public void StartSpecificWorkflow(int compassItemId, string workflowStepName)
        {
            // Get the current user
            SPUser currentUser = SPContext.Current.Web.CurrentUser;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        var item = spList.GetItemById(compassItemId);

                        string strWFTemplateId = GetSPWFTemplateIdBasedOnName(spList, workflowStepName);
                        SPWorkflowAssociation spwaDocLib = spList.WorkflowAssociations[new Guid(strWFTemplateId)];
                        spSite.WorkflowManager.StartWorkflow(item, spwaDocLib, spwaDocLib.AssociationData, true);
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void StartSpecificSGSWorkflow(int StageGateListItemId, string workflowStepName)
        {
            // Get the current user
            SPUser currentUser = SPContext.Current.Web.CurrentUser;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);

                        var item = spList.GetItemById(StageGateListItemId);

                        string strWFTemplateId = GetSPWFTemplateIdBasedOnName(spList, workflowStepName);
                        SPWorkflowAssociation spwaDocLib = spList.WorkflowAssociations[new Guid(strWFTemplateId)];
                        spSite.WorkflowManager.StartWorkflow(item, spwaDocLib, spwaDocLib.AssociationData, true);
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
    }
}
