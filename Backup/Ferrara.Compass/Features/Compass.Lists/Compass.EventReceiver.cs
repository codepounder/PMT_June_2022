using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using System.Collections.Generic;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Classes;
using Ferrara.Compass.Abstractions.Enum;
using System.Linq;

namespace Ferrara.Compass.Features.Compass.Lists
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("21f60722-c8b3-4fc9-9ad2-7612c13f74a7")]
    public class CompassEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        #region Variables
        SPWeb web;
        List<string> yesNoChoices = new List<string>{
                    "Yes",
                    "No"
                };

        List<string> approvalStates = new List<string>{
                    "Approved",
                    "Request for Information",
                    "Request for Capacity-Costing",
                    "Rejected"
                };

        List<string> completedStates = new List<string>{
                    "Complete",
                    "Incomplete"
                };
        #endregion

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            web = properties.Feature.Parent as SPWeb;

            CreateCompassList(GlobalConstants.LIST_CompassListName, "Compass List");
            CreateCompassList2(GlobalConstants.LIST_CompassList2Name, "Compass List 2");
            CreateCompassTeamList(GlobalConstants.LIST_CompassTeamListName, "Compass Team List");
            CreateStageGateProjectList(GlobalConstants.LIST_StageGateProjectListName, "Stage Gate Project List");
            CreateStageGateConsolidatedFinancialSummaryList(GlobalConstants.LIST_StageGateConsolidatedFinancialSummaryListName, "Stage Gate Consolidated Financial Summary List");
            CreateStageGateFinancialAnalysisList(GlobalConstants.LIST_StageGateFinancialAnalysisListName, "Stage Gate Financial Analysis List");
            CreateStageGateGateList(GlobalConstants.LIST_PMTRAListName, "StageGate Gate List");
            CreateStageGateBriefList(GlobalConstants.LIST_SGSGateBriefList, "StageGate Gate Brief List");
            CreateStageGateDeliverablesList(GlobalConstants.LIST_PMTNecessaryDeliverablesListName, "Stage Gate Deliverable Details List");
            CreateSGSChildProjectTempList(GlobalConstants.LIST_SGSChildProjectTempList, "Stage Gate Child Project Temp List");
            CreatePackagingItemList(GlobalConstants.LIST_PackagingItemListName, "Compass Packaging Item List");
            CreateCompassPackMeasurementsList();
            CreateCompassShipperFinishedGoodList();
            CreateCompassMixesList();
            CreateCompassWorkflowStatusList();

            CreateCompassApprovalList(GlobalConstants.LIST_ApprovalListName, "Compass Approval List");
            CreateCompassApprovalList2(GlobalConstants.LIST_ApprovalList2Name, "Compass Approval List 2");
            CreateSAPApprovalList(GlobalConstants.LIST_SAPApprovalListName, "Compass SAP Approval List");
            CreateCompassProjectDecisionsList();
            CreateCompassListViews();
            CreateCompassEmailLoggingList();
            CreateTimelineTypeList();
            CreateProjectTimelineUpdateList();
            CreateStageGateDocumentLibrary();
            CreateCompassDocumentLibrary();
            CreateWorldSyncRequestDocumentLibrary();
            CreateCompassDragonflyLibrary();
            CreateWorldSyncFUSEDocumentLibrary();
            CreateCompassUploadsDocumentLibrary();
            CreateComponentCostingList();
            CreateAlternateUOMsList();
            CreateWorldSyncRequestList();
            CreateMarketingClaimsList();
            CreateCompassTemplatesDocumentLibrary();
            CreateDragonflyList(GlobalConstants.LIST_CompassDragonflyListName, GlobalConstants.LIST_CompassDragonflyListName);
            CreateDragonflyErrorList(GlobalConstants.LIST_CompassDragonflyErrorListName, GlobalConstants.LIST_CompassDragonflyErrorListName);
        }
        private void CreateStageGateProjectList(string listName, string description)
        {
            try
            {
                SPList splist = web.Lists.TryGetList(listName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, listName, description);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields
                var needsListUpdate = false;

                #region ListFields
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.ProjectNumber, StageGateProjectListFields.ProjectNumber_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.ProjectName, StageGateProjectListFields.ProjectName_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.DesiredShipDate, StageGateProjectListFields.DesiredShipDate_DisplayName, SPFieldType.DateTime, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.RevisedShipDate, StageGateProjectListFields.RevisedShipDate_DisplayName, SPFieldType.DateTime, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.Gate0ApprovedDate, StageGateProjectListFields.Gate0ApprovedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.LineOfBusiness, StageGateProjectListFields.LineOfBusiness_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.ProjectTier, StageGateProjectListFields.ProjectTier_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.NumberofNoveltySKUs, StageGateProjectListFields.NumberofNoveltySKUs_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.ProductHierarchyL2, StageGateProjectListFields.ProductHierarchyL2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.Brand, StageGateProjectListFields.Brand_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.SKUs, StageGateProjectListFields.SKUs_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.ProjectType, StageGateProjectListFields.ProjectType_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.ProjectTypeSubCategory, StageGateProjectListFields.ProjectTypeSubCategory_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.BusinessFunction, StageGateProjectListFields.BusinessFunction_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.BusinessFunctionOther, StageGateProjectListFields.BusinessFunctionOther_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, StageGateProjectListFields.NewFinishedGood, StageGateProjectListFields.NewFinishedGood_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, StageGateProjectListFields.NewBaseFormula, StageGateProjectListFields.NewBaseFormula_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, StageGateProjectListFields.NewShape, StageGateProjectListFields.NewShape_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, StageGateProjectListFields.NewPackType, StageGateProjectListFields.NewPackType_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, StageGateProjectListFields.NewNetWeight, StageGateProjectListFields.NewNetWeight_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, StageGateProjectListFields.NewGraphics, StageGateProjectListFields.NewGraphics_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, StageGateProjectListFields.NewFlavorColor, StageGateProjectListFields.NewFlavorColor_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.ProjectConceptOverview, StageGateProjectListFields.ProjectConceptOverview_DisplayName, SPFieldType.Note, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.Stage, StageGateProjectListFields.Stage_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.StageOnHold, StageGateProjectListFields.StageOnHold_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.TotalOnHoldDays, StageGateProjectListFields.TotalOnHoldDays_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.OnHoldStartDate, StageGateProjectListFields.OnHoldStartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.OnHoldEndDate, StageGateProjectListFields.OnHoldEndDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #region ProjectLeader
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.ProjectLeader, StageGateProjectListFields.ProjectLeader_DisplayName, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.ProjectLeaderName, StageGateProjectListFields.ProjectLeaderName_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region ProjectManager
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.ProjectManager, StageGateProjectListFields.ProjectManager_DisplayName, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.ProjectManagerName, StageGateProjectListFields.ProjectManagerName_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region SeniorProjectManager
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.SeniorProjectManager, StageGateProjectListFields.SeniorProjectManager_DisplayName, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.SeniorProjectManagerName, StageGateProjectListFields.SeniorProjectManagerName_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region Marketing
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.Marketing, StageGateProjectListFields.Marketing_DisplayName, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.MarketingName, StageGateProjectListFields.MarketingName_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region RnD
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.InTech, StageGateProjectListFields.InTech_DisplayName, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.InTechName, StageGateProjectListFields.InTechName_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region QAInnovation
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.QAInnovation, StageGateProjectListFields.QAInnovation_DisplayName, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.QAInnovationName, StageGateProjectListFields.QAInnovationName_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region RegulatoryRnD
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.InTechRegulatory, StageGateProjectListFields.InTechRegulatory_DisplayName, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.InTechRegulatoryName, StageGateProjectListFields.InTechRegulatoryName_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region RegulatoryQA
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.RegulatoryQA, StageGateProjectListFields.RegulatoryQA_DisplayName, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.RegulatoryQAName, StageGateProjectListFields.RegulatoryQAName_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region PackagingEngineering
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.PackagingEngineering, StageGateProjectListFields.PackagingEngineering_DisplayName, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.PackagingEngineeringName, StageGateProjectListFields.PackagingEngineeringName_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region SupplyChain
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.SupplyChain, StageGateProjectListFields.SupplyChain_DisplayName, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.SupplyChainName, StageGateProjectListFields.SupplyChainName_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region Finance
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.Finance, StageGateProjectListFields.Finance_DisplayName, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.FinanceName, StageGateProjectListFields.FinanceName_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region Sales
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.Sales, StageGateProjectListFields.Sales_DisplayName, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.SalesName, StageGateProjectListFields.SalesName_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region Manufacturing
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.Manufacturing, StageGateProjectListFields.Manufacturing_DisplayName, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.ManufacturingName, StageGateProjectListFields.ManufacturingName_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region TeamMembers
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.TeamMembers, StageGateProjectListFields.TeamMembers_DisplayName, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.TeamMembersNames, StageGateProjectListFields.TeamMembersNames_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region External Mfg - Procurement
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.ExtMfgProcurement, StageGateProjectListFields.ExtMfgProcurement_DisplayName, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.ExtMfgProcurementName, StageGateProjectListFields.ExtMfgProcurementName_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region Packaging Procurement
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.PackagingProcurement, StageGateProjectListFields.PackagingProcurement_DisplayName, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.PackagingProcurementName, StageGateProjectListFields.PackagingProcurementName_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region Life Cycle Management
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.LifeCycleManagement, StageGateProjectListFields.LifeCycleManagement_DisplayName, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.LifeCycleManagementName, StageGateProjectListFields.LifeCycleManagementName_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region Legal
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.Legal, StageGateProjectListFields.Legal_DisplayName, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.LegalName, StageGateProjectListFields.LegalName_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region OtherMember
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.OtherMember, StageGateProjectListFields.OtherMember_DisplayName, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.OtherMemberName, StageGateProjectListFields.OtherMemberName_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion

                if (SetupUtilities.CreateFieldNote(splist, StageGateProjectListFields.ProjectNotes, StageGateProjectListFields.ProjectNotes_DisplayName, true, false, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateFieldChoice(splist, StageGateProjectListFields.Notification3MSent, StageGateProjectListFields.Notification3MSent_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, StageGateProjectListFields.Notification6MSent, StageGateProjectListFields.Notification6MSent_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, StageGateProjectListFields.Notification9MSent, StageGateProjectListFields.Notification9MSent_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.Notification3MSentDate, StageGateProjectListFields.Notification3MSentDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.Notification6MSentDate, StageGateProjectListFields.Notification6MSentDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.Notification9MSentDate, StageGateProjectListFields.Notification9MSentDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.ProjectSubmittedSent, StageGateProjectListFields.ProjectSubmittedSent_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.ProjectCompletedSent, StageGateProjectListFields.ProjectCompletedSent_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.ProjectCancelledSent, StageGateProjectListFields.ProjectCancelledSent_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.PostLaunchActive, StageGateProjectListFields.PostLaunchActive_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Workflow Fields
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.WorkflowPhase, StageGateProjectListFields.WorkflowPhase_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Dates
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.FormSubmittedDate, StageGateProjectListFields.FormSubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.FormSubmittedBy, StageGateProjectListFields.FormSubmittedBy_DisplayName, SPFieldType.User, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.ModifiedBy, StageGateProjectListFields.ModifiedBy_DisplayName, SPFieldType.User, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.ModifiedDate, StageGateProjectListFields.ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.LastUpdatedFormName, StageGateProjectListFields.LastUpdatedFormName_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.IPFStartDate, StageGateProjectListFields.IPFStartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.IPFSubmitter, StageGateProjectListFields.IPFSubmitter_DisplayName, SPFieldType.User, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, StageGateProjectListFields.AllProjectUsers, StageGateProjectListFields.AllProjectUsers_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region TestProject
                if (SetupUtilities.CreateFieldChoice(splist, StageGateProjectListFields.TestProject, StageGateProjectListFields.TestProject_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                #endregion
                if (needsListUpdate)
                    splist.Update();

            }
            catch (Exception ex)
            {

            }
        }
        private void CreateStageGateGateList(string listName, string description)
        {
            try
            {
                SPList splist = web.Lists.TryGetList(listName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, listName, description);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields
                var needsListUpdate = false;

                #region ListFields
                if (SetupUtilities.CreateField(splist, PMTRiskAssessmentFIelds.StageGateProjectItemId, PMTRiskAssessmentFIelds.StageGateProjectItemId_DisplayName, SPFieldType.Number, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PMTRiskAssessmentFIelds.Gate, PMTRiskAssessmentFIelds.Gate_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PMTRiskAssessmentFIelds.MarketingComments, PMTRiskAssessmentFIelds.MarketingComments_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PMTRiskAssessmentFIelds.SalesComments, PMTRiskAssessmentFIelds.SalesComments_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PMTRiskAssessmentFIelds.FinanceComments, PMTRiskAssessmentFIelds.FinanceComments_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PMTRiskAssessmentFIelds.RDComments, PMTRiskAssessmentFIelds.RDComments_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PMTRiskAssessmentFIelds.QAComments, PMTRiskAssessmentFIelds.QAComments_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PMTRiskAssessmentFIelds.PEComments, PMTRiskAssessmentFIelds.PEComments_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PMTRiskAssessmentFIelds.ManuComments, PMTRiskAssessmentFIelds.ManuComments_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PMTRiskAssessmentFIelds.SupplyChainComments, PMTRiskAssessmentFIelds.SupplyChainComments_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PMTRiskAssessmentFIelds.MarketingColor, PMTRiskAssessmentFIelds.MarketingColor_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PMTRiskAssessmentFIelds.SalesColor, PMTRiskAssessmentFIelds.SalesColor_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PMTRiskAssessmentFIelds.FinanceColor, PMTRiskAssessmentFIelds.FinanceColor_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PMTRiskAssessmentFIelds.RDColor, PMTRiskAssessmentFIelds.RDColor_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PMTRiskAssessmentFIelds.QAColor, PMTRiskAssessmentFIelds.QAColor_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PMTRiskAssessmentFIelds.PEColor, PMTRiskAssessmentFIelds.PEColor_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PMTRiskAssessmentFIelds.ManuColor, PMTRiskAssessmentFIelds.ManuColor_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PMTRiskAssessmentFIelds.SupplyChainColor, PMTRiskAssessmentFIelds.SupplyChainColor_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PMTRiskAssessmentFIelds.SupplyChainComments, PMTRiskAssessmentFIelds.SupplyChainComments_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldDateTime(splist, PMTRiskAssessmentFIelds.SGMeetingDate, PMTRiskAssessmentFIelds.SGMeetingDate_DisplayName, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldDateTime(splist, PMTRiskAssessmentFIelds.ActualSGMeetingDate, PMTRiskAssessmentFIelds.ActualSGMeetingDate_DisplayName, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PMTRiskAssessmentFIelds.SGMeetingStatus, PMTRiskAssessmentFIelds.SGMeetingStatus_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PMTRiskAssessmentFIelds.GateReadinessPct, PMTRiskAssessmentFIelds.GateReadinessPct_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PMTRiskAssessmentFIelds.TotalApplicable, PMTRiskAssessmentFIelds.TotalApplicable_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PMTRiskAssessmentFIelds.TotalApplicableCompleted, PMTRiskAssessmentFIelds.TotalApplicableCompleted_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PMTRiskAssessmentFIelds.SubmittedDate, PMTRiskAssessmentFIelds.SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PMTRiskAssessmentFIelds.SubmittedBy, PMTRiskAssessmentFIelds.SubmittedBy, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion ListFields
                if (needsListUpdate)
                    splist.Update();
            }
            catch (Exception ex)
            {

            }
        }
        private void CreateStageGateBriefList(string listName, string description)
        {
            try
            {
                SPList splist = web.Lists.TryGetList(listName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, listName, description);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields
                var needsListUpdate = false;

                #region ListFields
                if (SetupUtilities.CreateField(splist, SGSGateBriefFields.StageGateProjectItemId, SGSGateBriefFields.StageGateProjectItemId_DisplayName, SPFieldType.Number, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSGateBriefFields.Gate, SGSGateBriefFields.Gate_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #region Gate Brief Fields
                if (SetupUtilities.CreateField(splist, SGSGateBriefFields.BriefName, SGSGateBriefFields.BriefName_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSGateBriefFields.BriefNo, SGSGateBriefFields.BriefNo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, SGSGateBriefFields.ProductFormats, SGSGateBriefFields.ProductFormats_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSGateBriefFields.RetailExecution, SGSGateBriefFields.RetailExecution_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, SGSGateBriefFields.OtherKeyInfo, SGSGateBriefFields.OtherKeyInfo_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSGateBriefFields.OverallRisk, SGSGateBriefFields.OverallRisk_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSGateBriefFields.OverallStatus, SGSGateBriefFields.OverallStatus_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, SGSGateBriefFields.Milestones, SGSGateBriefFields.Milestones_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, SGSGateBriefFields.ImpactProjectHealth, SGSGateBriefFields.ImpactProjectHealth_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, SGSGateBriefFields.TeamRecommendation, SGSGateBriefFields.TeamRecommendation_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, SGSGateBriefFields.OverallRiskReason, SGSGateBriefFields.OverallRiskReason_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, SGSGateBriefFields.OverallStatusReason, SGSGateBriefFields.OverallStatusReason_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, SGSGateBriefFields.GateReadiness, SGSGateBriefFields.GateReadiness_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSGateBriefFields.Deleted, SGSGateBriefFields.Deleted_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSGateBriefFields.FinanceBriefInGateBrief, SGSGateBriefFields.FinanceBriefInGateBrief_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                #endregion
                #endregion ListFields
                if (needsListUpdate)
                    splist.Update();
            }
            catch (Exception ex)
            {

            }
        }
        private void CreateStageGateDeliverablesList(string listName, string description)
        {
            try
            {
                SPList splist = web.Lists.TryGetList(listName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, listName, description);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, StageGateDeliverablesFields.StageGateListItemId, StageGateDeliverablesFields.StageGateListItemId_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateDeliverablesFields.Stage, StageGateDeliverablesFields.Stage_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateDeliverablesFields.DeliverableDetails, StageGateDeliverablesFields.DeliverableDetails_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateDeliverablesFields.Applicable, StageGateDeliverablesFields.Applicable_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateDeliverablesFields.Status, StageGateDeliverablesFields.Status_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateDeliverablesFields.Owner, StageGateDeliverablesFields.Owner_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateDeliverablesFields.Comments, StageGateDeliverablesFields.Comments_DisplayName, SPFieldType.Note, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateDeliverablesFields.ModifiedBy, StageGateDeliverablesFields.ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldDateTime(splist, StageGateDeliverablesFields.ModifiedDate, StageGateDeliverablesFields.ModifiedDate_DisplayName, false, false))
                {
                    needsListUpdate = true;
                }
                if (needsListUpdate)
                    splist.Update();
            }
            catch (Exception ex)
            {

            }
        }
        private void CreateSGSChildProjectTempList(string listName, string description)
        {
            try
            {
                SPList splist = web.Lists.TryGetList(listName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, listName, description);
                }
                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.ParentID, SGSChildProjectTempListFields.ParentID, SPFieldType.Number, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.ProjectNumber, SGSChildProjectTempListFields.ProjectNumber, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.TBDIndicator, SGSChildProjectTempListFields.TBDIndicator, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.FinishedGood, SGSChildProjectTempListFields.FinishedGood, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.Description, SGSChildProjectTempListFields.Description, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.ProductHierarchy1, SGSChildProjectTempListFields.ProductHierarchy1, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.ManuallyCreateSAPDescription, SGSChildProjectTempListFields.ManuallyCreateSAPDescription, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.ProductHierarchy2, SGSChildProjectTempListFields.ProductHierarchy2, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.BrandMaterialGroup1, SGSChildProjectTempListFields.BrandMaterialGroup1, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.ProductMaterialGroup4, SGSChildProjectTempListFields.ProductMaterialGroup4, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.PackTypeMaterialGroup5, SGSChildProjectTempListFields.PackTypeMaterialGroup5, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.ProjectStatus, SGSChildProjectTempListFields.ProjectStatus, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.CreateIPFBtn, SGSChildProjectTempListFields.CreateIPFBtn, SPFieldType.Boolean, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.NeedsNewBtn, SGSChildProjectTempListFields.NeedsNewBtn, SPFieldType.Boolean, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.DeleteBtn, SGSChildProjectTempListFields.DeleteBtn, SPFieldType.Boolean, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.RequireNewUPCUCC, SGSChildProjectTempListFields.RequireNewUPCUCC, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.RequireNewUnitUPC, SGSChildProjectTempListFields.RequireNewUnitUPC, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.UnitUPC, SGSChildProjectTempListFields.UnitUPC, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.RequireNewDisplayBoxUPC, SGSChildProjectTempListFields.RequireNewDisplayBoxUPC, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.DisplayBoxUPC, SGSChildProjectTempListFields.DisplayBoxUPC, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.RequireNewCaseUCC, SGSChildProjectTempListFields.RequireNewCaseUCC, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.CaseUCC, SGSChildProjectTempListFields.CaseUCC, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.RequireNewPalletUCC, SGSChildProjectTempListFields.RequireNewPalletUCC, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.PalletUCC, SGSChildProjectTempListFields.PalletUCC, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.SAPBaseUOM, SGSChildProjectTempListFields.SAPBaseUOM, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.CustomerSpecific, SGSChildProjectTempListFields.CustomerSpecific, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.Customer, SGSChildProjectTempListFields.Customer, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.Channel, SGSChildProjectTempListFields.Channel, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.GenerateIPFSortOrder, SGSChildProjectTempListFields.GenerateIPFSortOrder, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                #region LTO
                if (SetupUtilities.CreateFieldChoice(splist, SGSChildProjectTempListFields.FGReplacingAnExistingFG, SGSChildProjectTempListFields.FGReplacingAnExistingFG, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, SGSChildProjectTempListFields.IsThisAnLTOItem, SGSChildProjectTempListFields.IsThisAnLTOItem, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, SGSChildProjectTempListFields.RequestChangeToFGNumForSameUCC, SGSChildProjectTempListFields.RequestChangeToFGNumForSameUCC, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.LTOTransitionStartWindowRDD, SGSChildProjectTempListFields.LTOTransitionStartWindowRDD, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.LTOTransitionEndWindowRDD, SGSChildProjectTempListFields.LTOTransitionEndWindowRDD, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SGSChildProjectTempListFields.LTOEndDateFlexibility, SGSChildProjectTempListFields.LTOEndDateFlexibility, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                if (needsListUpdate)
                    splist.Update();
            }
            catch (Exception ex)
            {

            }
        }
        private void CreateStageGateConsolidatedFinancialSummaryList(string listName, string description)
        {
            try
            {
                SPList splist = web.Lists.TryGetList(listName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, listName, description);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.StageGateProjectListItemId, StageGateConsolidatedFinancialSummaryListFields.StageGateProjectListItemId, SPFieldType.Integer, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.Gate, StageGateConsolidatedFinancialSummaryListFields.Gate_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.BriefNumber, StageGateConsolidatedFinancialSummaryListFields.BriefNumber_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.BriefName, StageGateConsolidatedFinancialSummaryListFields.BriefName_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, StageGateConsolidatedFinancialSummaryListFields.BriefSummary, StageGateConsolidatedFinancialSummaryListFields.BriefSummary_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.Name, StageGateConsolidatedFinancialSummaryListFields.Name_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.AverageTargetMargin, StageGateConsolidatedFinancialSummaryListFields.AverageTargetMargin_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.DispConsFinInProjBrief, StageGateConsolidatedFinancialSummaryListFields.DispConsFinInProjBrief_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.VolumeTotal1, StageGateConsolidatedFinancialSummaryListFields.VolumeTotal1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.VolumeIncremental1, StageGateConsolidatedFinancialSummaryListFields.VolumeIncremental1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.GrossSalesTotal1, StageGateConsolidatedFinancialSummaryListFields.GrossSalesTotal1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.GrossSalesIncremental1, StageGateConsolidatedFinancialSummaryListFields.GrossSalesIncremental1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.NetSalesTotal1, StageGateConsolidatedFinancialSummaryListFields.NetSalesTotal1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.NetSalesIncremental1, StageGateConsolidatedFinancialSummaryListFields.NetSalesIncremental1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.COGSTotal1, StageGateConsolidatedFinancialSummaryListFields.COGSTotal1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.COGSIncremental1, StageGateConsolidatedFinancialSummaryListFields.COGSIncremental1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.GrossMarginTotal1, StageGateConsolidatedFinancialSummaryListFields.GrossMarginTotal1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.GrossMarginIncremental1, StageGateConsolidatedFinancialSummaryListFields.GrossMarginIncremental1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctTotal1, StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctTotal1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctIncremental1, StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctIncremental1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.VolumeTotal2, StageGateConsolidatedFinancialSummaryListFields.VolumeTotal2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.VolumeIncremental2, StageGateConsolidatedFinancialSummaryListFields.VolumeIncremental2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.GrossSalesTotal2, StageGateConsolidatedFinancialSummaryListFields.GrossSalesTotal2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.GrossSalesIncremental2, StageGateConsolidatedFinancialSummaryListFields.GrossSalesIncremental2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.NetSalesTotal2, StageGateConsolidatedFinancialSummaryListFields.NetSalesTotal2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.NetSalesIncremental2, StageGateConsolidatedFinancialSummaryListFields.NetSalesIncremental2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.COGSTotal2, StageGateConsolidatedFinancialSummaryListFields.COGSTotal2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.COGSIncremental2, StageGateConsolidatedFinancialSummaryListFields.COGSIncremental2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.GrossMarginTotal2, StageGateConsolidatedFinancialSummaryListFields.GrossMarginTotal2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.GrossMarginIncremental2, StageGateConsolidatedFinancialSummaryListFields.GrossMarginIncremental2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctTotal2, StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctTotal2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctIncremental2, StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctIncremental2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.VolumeTotal3, StageGateConsolidatedFinancialSummaryListFields.VolumeTotal3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.VolumeIncremental3, StageGateConsolidatedFinancialSummaryListFields.VolumeIncremental3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.GrossSalesTotal3, StageGateConsolidatedFinancialSummaryListFields.GrossSalesTotal3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.GrossSalesIncremental3, StageGateConsolidatedFinancialSummaryListFields.GrossSalesIncremental3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.NetSalesTotal3, StageGateConsolidatedFinancialSummaryListFields.NetSalesTotal3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.NetSalesIncremental3, StageGateConsolidatedFinancialSummaryListFields.NetSalesIncremental3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.COGSTotal3, StageGateConsolidatedFinancialSummaryListFields.COGSTotal3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.COGSIncremental3, StageGateConsolidatedFinancialSummaryListFields.COGSIncremental3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.GrossMarginTotal3, StageGateConsolidatedFinancialSummaryListFields.GrossMarginTotal3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.GrossMarginIncremental3, StageGateConsolidatedFinancialSummaryListFields.GrossMarginIncremental3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctTotal3, StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctTotal3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctIncremental3, StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctIncremental3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.NSDollerperLB1, StageGateConsolidatedFinancialSummaryListFields.NSDollerperLB1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.NSDollerperLB2, StageGateConsolidatedFinancialSummaryListFields.NSDollerperLB2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.NSDollerperLB3, StageGateConsolidatedFinancialSummaryListFields.NSDollerperLB3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.COGSperLB1, StageGateConsolidatedFinancialSummaryListFields.COGSperLB1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.COGSperLB2, StageGateConsolidatedFinancialSummaryListFields.COGSperLB2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.COGSperLB3, StageGateConsolidatedFinancialSummaryListFields.COGSperLB3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, StageGateConsolidatedFinancialSummaryListFields.Analysesincluded, StageGateConsolidatedFinancialSummaryListFields.Analysesincluded_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }


                #region Dates
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.FormSubmittedDate, StageGateConsolidatedFinancialSummaryListFields.FormSubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.FormSubmittedBy, StageGateConsolidatedFinancialSummaryListFields.FormSubmittedBy_DisplayName, SPFieldType.User, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.ModifiedBy, StageGateConsolidatedFinancialSummaryListFields.ModifiedBy_DisplayName, SPFieldType.User, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateConsolidatedFinancialSummaryListFields.ModifiedDate, StageGateConsolidatedFinancialSummaryListFields.ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                if (needsListUpdate)
                    splist.Update();

            }
            catch (Exception ex)
            {

            }
        }
        private void CreateStageGateFinancialAnalysisList(string listName, string description)
        {
            try
            {
                SPList splist = web.Lists.TryGetList(listName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, listName, description);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.StageGateProjectListItemId, StageGateFinancialAnalysisListFields.StageGateProjectListItemId, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.Gate, StageGateFinancialAnalysisListFields.Gate_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.BriefNumber, StageGateFinancialAnalysisListFields.BriefNumber_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.BriefName, StageGateFinancialAnalysisListFields.BriefName_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.AnalysisName, StageGateFinancialAnalysisListFields.AnalysisName_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.FGNumber, StageGateFinancialAnalysisListFields.FGNumber_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.CustomerChannel, StageGateFinancialAnalysisListFields.CustomerChannel_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.BrandSeason, StageGateFinancialAnalysisListFields.BrandSeason_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.ProductForm, StageGateFinancialAnalysisListFields.ProductForm_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.TargetMarginPct, StageGateFinancialAnalysisListFields.TargetMarginPct_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.PLsinProjectBrief, StageGateFinancialAnalysisListFields.PLsinProjectBrief_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.PLinConsolidatedFinancials, StageGateFinancialAnalysisListFields.PLinConsolidatedFinancials_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.VolumeTotal1, StageGateFinancialAnalysisListFields.VolumeTotal1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.VolumeIncremental1, StageGateFinancialAnalysisListFields.VolumeIncremental1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.GrossSalesTotal1, StageGateFinancialAnalysisListFields.GrossSalesTotal1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.GrossSalesIncremental1, StageGateFinancialAnalysisListFields.GrossSalesIncremental1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.TradeRateTotal1, StageGateFinancialAnalysisListFields.TradeRateTotal1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.TradeRateIncremental1, StageGateFinancialAnalysisListFields.TradeRateIncremental1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.OGTNTotal1, StageGateFinancialAnalysisListFields.OGTNTotal1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.OGTNIncremental1, StageGateFinancialAnalysisListFields.OGTNIncremental1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.NetSalesTotal1, StageGateFinancialAnalysisListFields.NetSalesTotal1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.NetSalesIncremental1, StageGateFinancialAnalysisListFields.NetSalesIncremental1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.COGSTotal1, StageGateFinancialAnalysisListFields.COGSTotal1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;

                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.COGSIncremental1, StageGateFinancialAnalysisListFields.COGSIncremental1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.GrossMarginTotal1, StageGateFinancialAnalysisListFields.GrossMarginTotal1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.GrossMarginIncremental1, StageGateFinancialAnalysisListFields.GrossMarginIncremental1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.GrossMarginPctTotal1, StageGateFinancialAnalysisListFields.GrossMarginPctTotal1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.GrossMarginPctIncremental1, StageGateFinancialAnalysisListFields.GrossMarginPctIncremental1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.VolumeTotal2, StageGateFinancialAnalysisListFields.VolumeTotal2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.VolumeIncremental2, StageGateFinancialAnalysisListFields.VolumeIncremental2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.GrossSalesTotal2, StageGateFinancialAnalysisListFields.GrossSalesTotal2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.GrossSalesIncremental2, StageGateFinancialAnalysisListFields.GrossSalesIncremental2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.TradeRateTotal2, StageGateFinancialAnalysisListFields.TradeRateTotal2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.TradeRateIncremental2, StageGateFinancialAnalysisListFields.TradeRateIncremental2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.OGTNTotal2, StageGateFinancialAnalysisListFields.OGTNTotal2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.OGTNIncremental2, StageGateFinancialAnalysisListFields.OGTNIncremental2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.NetSalesTotal2, StageGateFinancialAnalysisListFields.NetSalesTotal2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.NetSalesIncremental2, StageGateFinancialAnalysisListFields.NetSalesIncremental2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.COGSTotal2, StageGateFinancialAnalysisListFields.COGSTotal2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.COGSIncremental2, StageGateFinancialAnalysisListFields.COGSIncremental2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.GrossMarginTotal2, StageGateFinancialAnalysisListFields.GrossMarginTotal2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.GrossMarginIncremental2, StageGateFinancialAnalysisListFields.GrossMarginIncremental2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.GrossMarginPctTotal2, StageGateFinancialAnalysisListFields.GrossMarginPctTotal2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.GrossMarginPctIncremental2, StageGateFinancialAnalysisListFields.GrossMarginPctIncremental2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.VolumeTotal3, StageGateFinancialAnalysisListFields.VolumeTotal3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.VolumeIncremental3, StageGateFinancialAnalysisListFields.VolumeIncremental3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.GrossSalesTotal3, StageGateFinancialAnalysisListFields.GrossSalesTotal3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.GrossSalesIncremental3, StageGateFinancialAnalysisListFields.GrossSalesIncremental3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.TradeRateTotal3, StageGateFinancialAnalysisListFields.TradeRateTotal3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.TradeRateIncremental3, StageGateFinancialAnalysisListFields.TradeRateIncremental3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.OGTNTotal3, StageGateFinancialAnalysisListFields.OGTNTotal3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.OGTNIncremental3, StageGateFinancialAnalysisListFields.OGTNIncremental3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.NetSalesTotal3, StageGateFinancialAnalysisListFields.NetSalesTotal3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.NetSalesIncremental3, StageGateFinancialAnalysisListFields.NetSalesIncremental3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.COGSTotal3, StageGateFinancialAnalysisListFields.COGSTotal3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.COGSIncremental3, StageGateFinancialAnalysisListFields.COGSIncremental3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.GrossMarginTotal3, StageGateFinancialAnalysisListFields.GrossMarginTotal3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.GrossMarginIncremental3, StageGateFinancialAnalysisListFields.GrossMarginIncremental3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.GrossMarginPctTotal3, StageGateFinancialAnalysisListFields.GrossMarginPctTotal3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.GrossMarginPctIncremental3, StageGateFinancialAnalysisListFields.GrossMarginPctIncremental3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.NSDollerperLB1, StageGateFinancialAnalysisListFields.NSDollerperLB1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.NSDollerperLB2, StageGateFinancialAnalysisListFields.NSDollerperLB2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.NSDollerperLB3, StageGateFinancialAnalysisListFields.NSDollerperLB3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.COGSperLB1, StageGateFinancialAnalysisListFields.COGSperLB1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.COGSperLB2, StageGateFinancialAnalysisListFields.COGSperLB2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.COGSperLB3, StageGateFinancialAnalysisListFields.COGSperLB3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.TruckldPricePrRtlSllngUt1, StageGateFinancialAnalysisListFields.TruckldPricePrRtlSllngUt1_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.TruckldPricePrRtlSllngUt2, StageGateFinancialAnalysisListFields.TruckldPricePrRtlSllngUt2_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.TruckldPricePrRtlSllngUt3, StageGateFinancialAnalysisListFields.TruckldPricePrRtlSllngUt3_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, StageGateFinancialAnalysisListFields.Assumptions1, StageGateFinancialAnalysisListFields.Assumptions1_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, StageGateFinancialAnalysisListFields.Assumptions2, StageGateFinancialAnalysisListFields.Assumptions2_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, StageGateFinancialAnalysisListFields.Assumptions3, StageGateFinancialAnalysisListFields.Assumptions3_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, StageGateFinancialAnalysisListFields.Deleted, StageGateFinancialAnalysisListFields.Deleted_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }

                #region Dates
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.FormSubmittedDate, StageGateFinancialAnalysisListFields.FormSubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.FormSubmittedBy, StageGateFinancialAnalysisListFields.FormSubmittedBy_DisplayName, SPFieldType.User, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.ModifiedBy, StageGateFinancialAnalysisListFields.ModifiedBy_DisplayName, SPFieldType.User, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateFinancialAnalysisListFields.ModifiedDate, StageGateFinancialAnalysisListFields.ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                if (needsListUpdate)
                    splist.Update();

            }
            catch (Exception ex)
            {

            }
        }
        private void CreateCompassList(string listName, string description)
        {
            try
            {
                SPList splist = web.Lists.TryGetList(listName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, listName, description);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields
                var needsListUpdate = false;

                #region Item Proposal Form fields - IPF Form
                if (SetupUtilities.CreateField(splist, CompassListFields.StageGateProjectListItemId, CompassListFields.StageGateProjectListItemId, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.ParentProjectNumber, CompassListFields.ParentProjectNumber, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.SAPDescription, CompassListFields.SAPDescription_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.SAPItemNumber, CompassListFields.SAPItemNumber_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateCurrencyField(splist, CompassListFields.AnnualProjectedDollars, CompassListFields.AnnualProjectedDollars_DisplayName, false, SPNumberFormatTypes.TwoDecimals))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateCurrencyField(splist, CompassListFields.Month1ProjectedDollars, CompassListFields.Month1ProjectedDollars_DisplayName, false, SPNumberFormatTypes.TwoDecimals))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateCurrencyField(splist, CompassListFields.Month2ProjectedDollars, CompassListFields.Month2ProjectedDollars_DisplayName, false, SPNumberFormatTypes.TwoDecimals))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateCurrencyField(splist, CompassListFields.Month3ProjectedDollars, CompassListFields.Month3ProjectedDollars_DisplayName, false, SPNumberFormatTypes.TwoDecimals))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.AnnualProjectedUnits, CompassListFields.AnnualProjectedUnits_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.Month1ProjectedUnits, CompassListFields.Month1ProjectedUnits_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.Month2ProjectedUnits, CompassListFields.Month2ProjectedUnits_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.Month3ProjectedUnits, CompassListFields.Month3ProjectedUnits_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, CompassListFields.CaseType, CompassListFields.CaseType_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.Customer, CompassListFields.Customer_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.CustomerSpecific, CompassListFields.CustomerSpecific_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, CompassListFields.CustomerSpecificLotCode, CompassListFields.CustomerSpecificLotCode_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.LikeFGItemNumber, CompassListFields.LikeFGItemNumber_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.LikeFGItemDescription, CompassListFields.LikeFGItemDescription_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.OldFGItemNumber, CompassListFields.OldFGItemNumber_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.OldFGItemDescription, CompassListFields.OldFGItemDescription_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldDateTime(splist, CompassListFields.RevisedFirstShipDate, CompassListFields.RevisedFirstShipDate_DisplayName, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldDateTime(splist, CompassListFields.FirstShipDate, CompassListFields.FirstShipDate_DisplayName, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.Initiator, CompassListFields.Initiator_DisplayName, SPFieldType.User, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.ProjectNumber, CompassListFields.ProjectNumber_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateCurrencyField(splist, CompassListFields.TruckLoadPricePerSellingUnit, CompassListFields.TruckLoadPricePerSellingUnit_DisplayName, false, SPNumberFormatTypes.TwoDecimals))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.MarketClaimsLabelingRequirements, CompassListFields.MarketClaimsLabelingRequirements_DisplayName, SPFieldType.Note, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.CountryOfSale, CompassListFields.CountryOfSale_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                var projectTypes = new List<string>{
                    GlobalConstants.PROJECTTYPE_Innovation,
                    GlobalConstants.PROJECTTYPE_LineExtension,
                    GlobalConstants.PROJECTTYPE_DownweightTransition,
                    GlobalConstants.PROJECTTYPE_GraphicsChangesInternalAdjustments,
                    GlobalConstants.PROJECTTYPE_GraphicsChangeOnly,
                    GlobalConstants.PROJECTTYPE_SimpleNetworkMove
                };
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.ProjectType, CompassListFields.ProjectType_DisplayName, false, projectTypes))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.TBDIndicator, CompassListFields.TBDIndicator_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateCurrencyField(splist, CompassListFields.Last12MonthSales, CompassListFields.Last12MonthSales_DisplayName, false, SPNumberFormatTypes.TwoDecimals))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.SAPBaseUOM, CompassListFields.SAPBaseUOM_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.BaseUOMNetWeightLbs, CompassListFields.BaseUOMNetWeightLbs_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.Organic, CompassListFields.Organic_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.Channel, CompassListFields.Channel_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.MaterialGroup1Brand, CompassListFields.MaterialGroup1Brand_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.MaterialGroup4ProductForm, CompassListFields.MaterialGroup4ProductForm_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.MaterialGroup5PackType, CompassListFields.MaterialGroup5PackType_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.ProductFormDescription, CompassListFields.ProductFormDescription_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.ExpectedGrossMarginPercent, CompassListFields.ExpectedGrossMarginPercent_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.RevisedGrossMarginPercent, CompassListFields.RevisedGrossMarginPercent_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.ProductHierarchyLevel1, CompassListFields.ProductHierarchyLevel1_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.ManuallyCreateSAPDescription, CompassListFields.ManuallyCreateSAPDescription_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.ProductHierarchyLevel2, CompassListFields.ProductHierarchyLevel2_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.NoveltyProject, CompassListFields.NoveltyProject_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.PM, CompassListFields.PM_DisplayName, SPFieldType.User, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.OBM, CompassListFields.OBM_DisplayName, SPFieldType.User, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, CompassListFields.ItemConcept, CompassListFields.ItemConcept_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.CaseUCC, CompassListFields.CaseUCC_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.DisplayBoxUPC, CompassListFields.DisplayBoxUPC_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.PalletUCC, CompassListFields.PalletUCC_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.UnitUPC, CompassListFields.UnitUPC_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.DTVProject, CompassListFields.DTVProject_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.MfgLocationChange, CompassListFields.MfgLocationChange_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.ImmediateSPKChange, CompassListFields.ImmediateSPKChange_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.ServingSizeWeightChange, CompassListFields.ServingSizeWeightChange_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.SoldOutsideUSA, CompassListFields.SoldOutsideUSA_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.RequireNewUPCUCC, CompassListFields.RequireNewUPCUCC_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.RequireNewUnitUPC, CompassListFields.RequireNewUnitUPC_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.RequireNewDisplayBoxUPC, CompassListFields.RequireNewDisplayBoxUPC_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.RequireNewCaseUCC, CompassListFields.RequireNewCaseUCC_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.RequireNewPalletUCC, CompassListFields.RequireNewPalletUCC_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.Flowthrough, CompassListFields.Flowthrough_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, CompassListFields.FlowthroughDets, CompassListFields.FlowthroughDets_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.NumberofTraysPerBaseUOM, CompassListFields.NumberofTraysPerBaseUOM_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.RetailSellingUnitsBaseUOM, CompassListFields.RetailSellingUnitsBaseUOM_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.RetailUnitWieghtOz, CompassListFields.RetailUnitWieghtOz_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.TotalQuantityUnitsInDisplay, CompassListFields.TotalQuantityUnitsInDisplay_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldDateTime(splist, CompassListFields.ExpectedPackagingSwitchDate, CompassListFields.ExpectedPackagingSwitchDate_DisplayName, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.FilmSubstrate, CompassListFields.FilmSubstrate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.PegHoleNeeded, CompassListFields.PegHoleNeeded_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.InvolvesCarton, CompassListFields.InvolvesCarton_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.UnitsInsideCarton, CompassListFields.UnitsInsideCarton_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.IndividualPouchWeight, CompassListFields.IndividualPouchWeight_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.InitiatorName, CompassListFields.InitiatorName_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.PMName, CompassListFields.PMName_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.OBMName, CompassListFields.OBMName_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.ProjectTypeSubCategory, CompassListFields.ProjectTypeSubCategory_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Operations Form fields - OPS Form

                if (SetupUtilities.CreateField(splist, CompassListFields.ManufacturingLocation, CompassListFields.ManufacturingLocation_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.SecondaryManufacturingLocation, CompassListFields.SecondaryManufacturingLocation_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.PackingLocation, CompassListFields.PackingLocation_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.DistributionCenter, CompassListFields.DistributionCenter_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.SecondaryDistributionCenter, CompassListFields.SecondaryDistributionCenter_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.InternalTransferSemiNeeded, CompassListFields.InternalTransferSemiNeeded_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.PurchasedCandySemiNeeded, CompassListFields.PurchasedCandySemiNeeded_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.WorkCenterAdditionalInfo, CompassListFields.WorkCenterAdditionalInfo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region External Manufacturer Fields
                if (SetupUtilities.CreateField(splist, CompassListFields.ExternalMfgProjectLead, CompassListFields.ExternalMfgProjectLead_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.CoManufacturingClassification, CompassListFields.CoManufacturingClassification_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.DoesBulkSemiExistToBringInHouse, CompassListFields.DoesBulkSemiExistToBringInHouse_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.ExistingBulkSemiNumber, CompassListFields.ExistingBulkSemiNumber_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.BulkSemiDescription, CompassListFields.BulkSemiDescription_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.ExternalManufacturer, CompassListFields.ExternalManufacturer_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.DoesSupplierHaveMakeCapacity, CompassListFields.DoesSupplierHaveMakeCapacity_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.ManufacturerCountryOfOrigin, CompassListFields.ManufacturerCountryOfOrigin_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.ExternalPacker, CompassListFields.ExternalPacker_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.DoesSupplierHavePackCapacity, CompassListFields.DoesSupplierHavePackCapacity_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.PurchasedIntoLocation, CompassListFields.PurchasedIntoLocation_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.CurrentTimelineAcceptable, CompassListFields.CurrentTimelineAcceptable_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.LeadTimeFromSupplier, CompassListFields.LeadTimeFromSupplier_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldDateTime(splist, CompassListFields.FinalArtworkDueToSupplier, CompassListFields.FinalArtworkDueToSupplier_DisplayName, false, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Quality Assurance Form fields - QA Form

                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.NewFormula, CompassListFields.NewFormula_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.NewShape, CompassListFields.NewShape_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.NewFlavorColor, CompassListFields.NewFlavorColor_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.NewNetWeight, CompassListFields.NewNetWeight_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Trade Promo Group Form fields - TRADE Form

                if (SetupUtilities.CreateField(splist, CompassListFields.MaterialGroup2Pricing, CompassListFields.MaterialGroup2Pricing_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                #endregion

                #region Bill of Materials
                if (SetupUtilities.CreateField(splist, CompassListFields.PackagingEngineerLead, CompassListFields.PackagingEngineerLead_DisplayName, SPFieldType.User, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, CompassListFields.PackagingNumbers, CompassListFields.PackagingNumbers_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region PM 1st Review fields - OBMREV1
                var choices = new List<string>{
                    GlobalConstants.PROJECTSTATUS_Green,
                    GlobalConstants.PROJECTSTATUS_Red,
                    GlobalConstants.PROJECTSTATUS_Yellow
                };
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.ProjectStatus, CompassListFields.ProjectStatus_DisplayName, false, choices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldDateTime(splist, CompassListFields.FirstProductionDate, CompassListFields.FirstProductionDate_DisplayName, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.OBMFirstReviewCheck, CompassListFields.OBMFirstReviewCheck_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.SectionsOfConcern, CompassListFields.SectionsOfConcern_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.OBMFirstReviewComments, CompassListFields.OBMFirstReviewComments_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.DoesFirstShipNeedRevision, CompassListFields.DoesFirstShipNeedRevision_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.RevisedFirstShipDateComments, CompassListFields.RevisedFirstShipDateComments_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.OBMSecondReviewCheck, CompassListFields.OBMSecondReviewCheck_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Request For Graphics Form fields - RGF Form

                #endregion

                #region Page Links
                if (SetupUtilities.CreateField(splist, CompassListFields.CommercializationLink, CompassListFields.CommercializationLink_DisplayName, SPFieldType.URL, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.NewIPF, CompassListFields.NewIPF_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.PMTWorkflowVersion, CompassListFields.PMTWorkflowVersion_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.ChangeLink, CompassListFields.ChangeLink_DisplayName, SPFieldType.URL, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.ChangeRequestNewProjectLink, CompassListFields.ChangeRequestNewProjectLink_DisplayName, SPFieldType.URL, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.CopyLink, CompassListFields.CopyLink_DisplayName, SPFieldType.URL, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.WorkflowStatusLink, CompassListFields.WorkflowStatusLink_DisplayName, SPFieldType.URL, false))
                {
                    needsListUpdate = true;
                }

                #endregion

                #region Workflow Fields
                var workflowphases = new List<string>{
                    GlobalConstants.WORKFLOWPHASE_IPF,
                    GlobalConstants.WORKFLOWPHASE_SrOBMInitialReview,
                    GlobalConstants.WORKFLOWPHASE_PrelimSAPInitialItemSetup,
                    GlobalConstants.WORKFLOWPHASE_Coordination,
                    GlobalConstants.WORKFLOWPHASE_OBMFirstReview,
                    GlobalConstants.WORKFLOWPHASE_BOMCreation,
                    GlobalConstants.WORKFLOWPHASE_OBMSecondReview,
                    GlobalConstants.WORKFLOWPHASE_FinalSetup,
                    GlobalConstants.WORKFLOWPHASE_OBMThirdReview,
                    GlobalConstants.WORKFLOWPHASE_ProductionReadiness,
                    GlobalConstants.WORKFLOWPHASE_OnHold.ToString(),
                    GlobalConstants.WORKFLOWPHASE_Completed.ToString(),
                    GlobalConstants.WORKFLOWPHASE_Cancelled.ToString(),
                    GlobalConstants.WORKFLOWPHASE_GraphicsOnlyPhase.ToString(),
                    GlobalConstants.WORKFLOWPHASE_ExtGraphicsOnlyPhase.ToString()
                };
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.WorkflowPhase, CompassListFields.WorkflowPhase_DisplayName, false, workflowphases))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.OnHoldWorkflowPhase, CompassListFields.OnHoldWorkflowPhase_DisplayName, false, workflowphases))
                {
                    needsListUpdate = true;
                }
                var workflowupdatestatuses = new List<string>{
                    "None",
                    "Start",
                    "Started",
                    "Stop",
                    "Stopped",
                    "OnHold",
                    "ReleaseOnHold"
                };
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.PMTWorkflowUpdateStatus, CompassListFields.PMTWorkflowUpdateStatus_DisplayName, false, workflowupdatestatuses))
                {
                    needsListUpdate = true;
                }
                #endregion

                if (SetupUtilities.CreateFieldNote(splist, CompassListFields.ProjectNotes, CompassListFields.ProjectNotes_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldDateTime(splist, CompassListFields.SubmittedDate, CompassListFields.SubmittedDate_DisplayName, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.LastUpdatedFormName, CompassListFields.LastUpdatedFormName_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                List<string> timelineTypes = new List<string>{
                    "Standard",
                    "Expedited",
                    "Ludicrous"
                };
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.TimelineType, CompassListFields.TimelineType_DisplayName, false, timelineTypes))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.ReasonForChange, CompassListFields.ReasonForChange_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.TestProject, CompassListFields.TestProject_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, CompassListFields.GenerateIPFSortOrder, CompassListFields.GenerateIPFSortOrder_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, CompassListFields.AllProjectUsers, CompassListFields.AllProjectUsers_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.PLMProject, CompassListFields.PLMProject_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.ProfitCenter, CompassListFields.ProfitCenter_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.FinalUpdate, CompassListFields.FinalUpdate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.FirstShipDateMet, CompassListFields.FirstShipDateMet_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (needsListUpdate)
                    splist.Update();

            }
            catch (Exception ex)
            {

            }
        }
        private void CreateCompassList2(string listName, string description)
        {
            try
            {
                SPList splist = web.Lists.TryGetList(listName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, listName, description);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, CompassList2Fields.CompassListItemId, CompassList2Fields.CompassListItemId, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }

                #region Distribution Form Fields

                if (SetupUtilities.CreateField(splist, CompassList2Fields.DesignateHUBDC, CompassList2Fields.DesignateHUBDC_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, CompassList2Fields.DeploymentModeofItem, CompassList2Fields.DeploymentModeofItem_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                #region SELL DCs
                if (SetupUtilities.CreateField(splist, CompassList2Fields.ExtendtoSL07, CompassList2Fields.ExtendtoSL07_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.SetSL07SPKto, CompassList2Fields.SetSL07SPKto_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.ExtendtoSL13, CompassList2Fields.ExtendtoSL13_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.SetSL13SPKto, CompassList2Fields.SetSL13SPKto_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.ExtendtoSL18, CompassList2Fields.ExtendtoSL18_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.SetSL18SPKto, CompassList2Fields.SetSL18SPKto_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.ExtendtoSL19, CompassList2Fields.ExtendtoSL19_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.SetSL19SPKto, CompassList2Fields.SetSL19SPKto_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.ExtendtoSL30, CompassList2Fields.ExtendtoSL30_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.SetSL30SPKto, CompassList2Fields.SetSL30SPKto_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.ExtendtoSL14, CompassList2Fields.ExtendtoSL14_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.SetSL14SPKto, CompassList2Fields.SetSL14SPKto_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region FERQ DCs
                if (SetupUtilities.CreateField(splist, CompassList2Fields.ExtendtoFQ26, CompassList2Fields.ExtendtoFQ26_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.SetFQ26SPKto, CompassList2Fields.SetFQ26SPKto_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.ExtendtoFQ27, CompassList2Fields.ExtendtoFQ27_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.SetFQ27SPKto, CompassList2Fields.SetFQ27SPKto_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.ExtendtoFQ28, CompassList2Fields.ExtendtoFQ28_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.SetFQ28SPKto, CompassList2Fields.SetFQ28SPKto_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.ExtendtoFQ29, CompassList2Fields.ExtendtoFQ29_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.SetFQ29SPKto, CompassList2Fields.SetFQ29SPKto_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.ExtendtoFQ34, CompassList2Fields.ExtendtoFQ34_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.SetFQ34SPKto, CompassList2Fields.SetFQ34SPKto_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.ExtendtoFQ35, CompassList2Fields.ExtendtoFQ35_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.SetFQ35SPKto, CompassList2Fields.SetFQ35SPKto_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #endregion
                #region Graphics
                #region OPS
                if (SetupUtilities.CreateFieldNote(splist, CompassList2Fields.WhatNetworkMoveIsRequired, CompassList2Fields.WhatNetworkMoveIsRequired_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.ProjectApproved, CompassList2Fields.ProjectApproved_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.ReasonForRejection, CompassList2Fields.ReasonForRejection_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region PM Initial Review
                if (SetupUtilities.CreateField(splist, CompassList2Fields.NeedSExpeditedWorkflowWithSGS, CompassList2Fields.NeedSExpeditedWorkflowWithSGS_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region PM 2 Review
                if (SetupUtilities.CreateField(splist, CompassList2Fields.SGSExpeditedWorkflowApproved, CompassList2Fields.SGSExpeditedWorkflowApproved_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region  Ext Mfg
                if (SetupUtilities.CreateField(splist, CompassList2Fields.PackSupplierAndDielineSame, CompassList2Fields.PackSupplierAndDielineSame_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, CompassList2Fields.WhatChangeIsRequiredExtMfg, CompassList2Fields.WhatChangeIsRequiredExtMfg_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region InTech
                if (SetupUtilities.CreateField(splist, CompassList2Fields.IsRegulatoryinformationCorrect, CompassList2Fields.IsRegulatoryinformationCorrect_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, CompassList2Fields.WhatRegulatoryInfoIsIncorrect, CompassList2Fields.WhatRegulatoryInfoIsIncorrect_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.DoYouApproveThisProjectToProceed, CompassList2Fields.DoYouApproveThisProjectToProceed_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region PE2
                if (SetupUtilities.CreateField(splist, CompassList2Fields.AllAspectsApprovedFromPEPersp, CompassList2Fields.AllAspectsApprovedFromPEPersp_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, CompassList2Fields.WhatIsIncorrectPE, CompassList2Fields.WhatIsIncorrectPE_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #endregion

                #region IPF Fields
                if (SetupUtilities.CreateField(splist, CompassList2Fields.CopyFormsForGraphicsProject, CompassList2Fields.CopyFormsForGraphicsProject_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, CompassList2Fields.ExternalSemisItem, CompassList2Fields.ExternalSemisItem_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, CompassList2Fields.IPFCopiedFromCompassListItemId, CompassList2Fields.IPFCopiedFromCompassListItemId_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, CompassList2Fields.IsDisplayBoxConsumerUnit, CompassList2Fields.IsDisplayBoxConsumerUnit_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region RegulatoryComments
                if (SetupUtilities.CreateFieldNote(splist, CompassList2Fields.RegulatoryComments, CompassList2Fields.RegulatoryComments_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region EstimatedBracketPricing
                if (SetupUtilities.CreateFieldChoice(splist, CompassList2Fields.InitialEstimatedBracketPricing, CompassList2Fields.InitialEstimatedBracketPricing_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region InitialEstimatedPricing
                if (SetupUtilities.CreateFieldChoice(splist, CompassList2Fields.InitialEstimatedPricing, CompassList2Fields.InitialEstimatedPricing_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region BEQRC
                if (SetupUtilities.CreateField(splist, CompassList2Fields.ConsumerFacingProdDesc, CompassList2Fields.ConsumerFacingProdDesc_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region LTO
                if (SetupUtilities.CreateFieldChoice(splist, CompassList2Fields.FGReplacingAnExistingFG, CompassList2Fields.FGReplacingAnExistingFG_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassList2Fields.IsThisAnLTOItem, CompassList2Fields.IsThisAnLTOItem_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassList2Fields.RequestChangeToFGNumForSameUCC, CompassList2Fields.RequestChangeToFGNumForSameUCC_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.LTOTransitionStartWindowRDD, CompassList2Fields.LTOTransitionStartWindowRDD_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.LTOTransitionEndWindowRDD, CompassList2Fields.LTOTransitionEndWindowRDD_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassList2Fields.LTOEndDateFlexibility, CompassList2Fields.LTOEndDateFlexibility_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                if (SetupUtilities.CreateField(splist, CompassList2Fields.ModifiedBy, CompassList2Fields.ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, CompassList2Fields.ModifiedDate, CompassList2Fields.ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (needsListUpdate)
                    splist.Update();

            }
            catch (Exception ex)
            {

            }
        }
        private void CreateCompassTeamList(string listName, string description)
        {
            try
            {
                SPList splist = web.Lists.TryGetList(listName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, listName, description);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, CompassTeamListFields.CompassListItemId, CompassTeamListFields.CompassListItemId, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                #region ProjectLeader
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.ProjectLeader, CompassTeamListFields.ProjectLeader, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.ProjectLeaderName, CompassTeamListFields.ProjectLeaderName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region ProjectManager
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.ProjectManager, CompassTeamListFields.ProjectManager, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.ProjectManagerName, CompassTeamListFields.ProjectManagerName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region SeniorProjectManager
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.SeniorProjectManager, CompassTeamListFields.SeniorProjectManager, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.SeniorProjectManagerName, CompassTeamListFields.SeniorProjectManagerName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region Marketing
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.Marketing, CompassTeamListFields.Marketing, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.MarketingName, CompassTeamListFields.MarketingName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region QAInnovation
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.QAInnovation, CompassTeamListFields.QAInnovation, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.QAInnovationName, CompassTeamListFields.QAInnovationName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region RegulatoryRnD
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.InTechRegulatory, CompassTeamListFields.InTechRegulatory, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.InTechRegulatoryName, CompassTeamListFields.InTechRegulatoryName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region RegulatoryQA
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.RegulatoryQA, CompassTeamListFields.RegulatoryQA, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.RegulatoryQAName, CompassTeamListFields.RegulatoryQAName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region PackagingEngineering
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.PackagingEngineering, CompassTeamListFields.PackagingEngineering, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.PackagingEngineeringName, CompassTeamListFields.PackagingEngineeringName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region SupplyChain
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.SupplyChain, CompassTeamListFields.SupplyChain, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.SupplyChainName, CompassTeamListFields.SupplyChainName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region Finance
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.Finance, CompassTeamListFields.Finance, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.FinanceName, CompassTeamListFields.FinanceName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region Sales
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.Sales, CompassTeamListFields.Sales, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.SalesName, CompassTeamListFields.SalesName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region Manufacturing
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.Manufacturing, CompassTeamListFields.Manufacturing, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.ManufacturingName, CompassTeamListFields.ManufacturingName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region TeamMembers
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.TeamMembers, CompassTeamListFields.TeamMembers, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.TeamMembersNames, CompassTeamListFields.TeamMembersNames, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region External Mfg - Procurement
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.ExtMfgProcurement, CompassTeamListFields.ExtMfgProcurement, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.ExtMfgProcurementName, CompassTeamListFields.ExtMfgProcurementName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region Packaging Procurement
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.PackagingProcurement, CompassTeamListFields.PackagingProcurement, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.PackagingProcurementName, CompassTeamListFields.PackagingProcurementName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region Life Cycle Management
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.LifeCycleManagement, CompassTeamListFields.LifeCycleManagement, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.LifeCycleManagementName, CompassTeamListFields.LifeCycleManagementName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region Legal
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.Legal, CompassTeamListFields.Legal, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.LegalName, CompassTeamListFields.LegalName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region OtherMember
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.OtherMember, CompassTeamListFields.OtherMember, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.OtherMemberName, CompassTeamListFields.OtherMemberName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                #endregion
                //MOVED FROM COMPASS LIST
                #region InTech (Previously R&D Manager)
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.InTech, CompassTeamListFields.InTech, SPFieldType.User, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.InTechName, CompassTeamListFields.InTechName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region Marketing (previously Brand Manager)
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.Marketing, CompassTeamListFields.Marketing, SPFieldType.User, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.MarketingName, CompassTeamListFields.MarketingName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region BE QR Code Initiator
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.BEQRCodeInitiator, CompassTeamListFields.BEQRCodeInitiator, SPFieldType.User, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassTeamListFields.BEQRCodeInitiatorName, CompassTeamListFields.BEQRCodeInitiatorName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                if (needsListUpdate)
                    splist.Update();

            }
            catch (Exception ex)
            {

            }
        }
        private void CreateStageGateDocumentLibrary()
        {
            string description = "Stage Gate Document";
            try
            {
                SPList splist = web.Lists.TryGetList(GlobalConstants.DOCLIBRARY_StageGateLibraryName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateDocumentLibrary(web, GlobalConstants.DOCLIBRARY_StageGateLibraryName, description);
                }

                var needsListUpdate = false;
                var choices = new List<string>{
                    GlobalConstants.DOCTYPE_StageGateProjectBrief,
                    GlobalConstants.DOCTYPE_StageGateOthers,
                    GlobalConstants.DOCTYPE_StageGateBriefPDF,
                    GlobalConstants.DOCTYPE_StageGateFinanceBriefPDF,
                    GlobalConstants.DOCTYPE_StageGateBriefImage,
                    GlobalConstants.DOCTYPE_StageGateGateDocument
                };
                if (SetupUtilities.CreateFieldChoice(splist, StageGateProjectListFields.DOCLIBRARY_StageGateDocType, StageGateProjectListFields.DOCLIBRARY_StageGateDocType_DisplayName, false, choices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.DOCLIBRARY_StageGateGate, StageGateProjectListFields.DOCLIBRARY_StageGateGate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.DOCLIBRARY_StageGateBriefNo, StageGateProjectListFields.DOCLIBRARY_StageGateBriefNo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, StageGateProjectListFields.DOCLIBRARY_DisplayFileName, StageGateProjectListFields.DOCLIBRARY_DisplayFileName_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (needsListUpdate)
                    splist.Update();

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;
                splist.EnableFolderCreation = true;
            }
            catch (Exception ex)
            {

            }
        }
        private void CreateCompassDocumentLibrary()
        {
            string description = "Compass Attachments that were saved during the approval process";
            try
            {
                SPList splist = web.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassLibraryName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateDocumentLibrary(web, GlobalConstants.DOCLIBRARY_CompassLibraryName, description);
                }

                var needsListUpdate = false;
                var choices = new List<string>{
                    GlobalConstants.DOCTYPE_CADDrawing,
                    GlobalConstants.DOCTYPE_Formulation,
                    GlobalConstants.DOCTYPE_Graphics,
                    GlobalConstants.DOCTYPE_CAPACITY,
                    GlobalConstants.DOCTYPE_COSTING,
                    GlobalConstants.DOCTYPE_GraphicsRequest,
                    GlobalConstants.DOCTYPE_INNOVATION,
                    GlobalConstants.DOCTYPE_PalletPattern,
                    GlobalConstants.DOCTYPE_NLEA,
                    GlobalConstants.DOCTYPE_PackTrial,
                    GlobalConstants.DOCTYPE_BRACKETPRICING,
                    GlobalConstants.DOCTYPE_PACKSPECS
                };
                if (SetupUtilities.CreateFieldChoice(splist, CompassListFields.DOCLIBRARY_CompassDocType, CompassListFields.DOCLIBRARY_CompassDocType_DisplayName, false, choices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassListFields.DOCLIBRARY_PackagingComponentItemId, CompassListFields.DOCLIBRARY_PackagingComponentItemId_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, CompassListFields.DOCLIBRARY_DisplayFileName, CompassListFields.DOCLIBRARY_DisplayFileName_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (needsListUpdate)
                    splist.Update();

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;
                splist.EnableFolderCreation = true;
            }
            catch (Exception ex)
            {

            }
        }
        private void CreateWorldSyncRequestDocumentLibrary()
        {
            string description = "WorldSync Request Documents to request to add files to Finished Goods";
            try
            {
                SPList splist = web.Lists.TryGetList(GlobalConstants.DOCLIBRARY_WorldSyncRequestName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateDocumentLibrary(web, GlobalConstants.DOCLIBRARY_WorldSyncRequestName, description);
                }

                var needsListUpdate = false;
                var choices = new List<string>{
                    GlobalConstants.DOCTYPE_RequestImage,
                    GlobalConstants.DOCTYPE_RequestNutritional
                };
                if (SetupUtilities.CreateFieldChoice(splist, WorldSyncRequestFields.DOCLIBRARY_RequestType, WorldSyncRequestFields.DOCLIBRARY_RequestType_DisplayName, false, choices))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, WorldSyncRequestFields.DOCLIBRARY_DisplayFileName, WorldSyncRequestFields.DOCLIBRARY_DisplayFileName_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, WorldSyncRequestFields.DOCLIBRARY_RequestId, WorldSyncRequestFields.DOCLIBRARY_RequestId_DisplayName, SPFieldType.Integer, false))
                {
                    needsListUpdate = true;
                }

                if (needsListUpdate)
                    splist.Update();

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;
                splist.EnableFolderCreation = true;
            }
            catch (Exception ex)
            {

            }
        }
        private void CreateCompassDragonflyLibrary()
        {
            string description = "Compass Dragonfly Documents";
            try
            {
                SPList splist = web.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassDragonflyLibraryName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateDocumentLibrary(web, GlobalConstants.DOCLIBRARY_CompassDragonflyLibraryName, description);
                }

                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, WorldSyncFuseFileFields.DOCLIBRARY_DisplayFileName, WorldSyncFuseFileFields.DOCLIBRARY_DisplayFileName_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }


                if (needsListUpdate)
                    splist.Update();

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;
                splist.EnableFolderCreation = true;
            }
            catch (Exception ex)
            {

            }
        }
        private void CreateWorldSyncFUSEDocumentLibrary()
        {
            string description = "World Sync FUSE Documents";
            try
            {
                SPList splist = web.Lists.TryGetList(GlobalConstants.DOCLIBRARY_WorldSyncFUSELibraryName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateDocumentLibrary(web, GlobalConstants.DOCLIBRARY_WorldSyncFUSELibraryName, description);
                }

                var needsListUpdate = false;
                var choices = new List<string>{
                    GlobalConstants.DOCTYPE_RequestImage,
                    GlobalConstants.DOCTYPE_RequestNutritional
                };
                if (SetupUtilities.CreateFieldChoice(splist, WorldSyncFuseFileFields.DOCLIBRARY_RequestType, WorldSyncFuseFileFields.DOCLIBRARY_RequestType_DisplayName, false, choices))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, WorldSyncFuseFileFields.DOCLIBRARY_DisplayFileName, WorldSyncFuseFileFields.DOCLIBRARY_DisplayFileName_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, WorldSyncFuseFileFields.DOCLIBRARY_RequestId, WorldSyncFuseFileFields.DOCLIBRARY_RequestId_DisplayName, SPFieldType.Integer, false))
                {
                    needsListUpdate = true;
                }

                if (needsListUpdate)
                    splist.Update();

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;
                splist.EnableFolderCreation = true;
            }
            catch (Exception ex)
            {

            }
        }
        private void CreatePackagingItemList(string listName, string description)
        {
            try
            {
                SPList splist = web.Lists.TryGetList(listName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, listName, description);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, PackagingItemListFields.PackagingComponent, PackagingItemListFields.PackagingComponent_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                var newExistingTypes = new List<string>{
                    "Select...",
                    GlobalConstants.PACKAGINGNEWEXISTING_NEW,
                    GlobalConstants.PACKAGINGNEWEXISTINGE_EXISTING
                };
                if (SetupUtilities.CreateFieldChoice(splist, PackagingItemListFields.NewExisting, PackagingItemListFields.NewExisting_DisplayName, false, newExistingTypes))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.MaterialNumber, PackagingItemListFields.MaterialNumber_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.MaterialDescription, PackagingItemListFields.MaterialDescription_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.CurrentLikeItem, PackagingItemListFields.CurrentLikeItem_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.CurrentLikeItemDescription, PackagingItemListFields.CurrentLikeItemDescription_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.CurrentOldItem, PackagingItemListFields.CurrentOldItem_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.CurrentOldItemDescription, PackagingItemListFields.CurrentOldItemDescription_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, PackagingItemListFields.CurrentLikeItemReason, PackagingItemListFields.CurrentLikeItemReason_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.PackQuantity, PackagingItemListFields.PackQuantity_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.TareWeight, PackagingItemListFields.TareWeight_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.LeadPlateTime, PackagingItemListFields.LeadPlateTime_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.LeadMaterialTime, PackagingItemListFields.LeadMaterialTime_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.PrinterSupplier, PackagingItemListFields.PrinterSupplier_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, PackagingItemListFields.Notes, PackagingItemListFields.Notes_DisplayName, false, false, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.Length, PackagingItemListFields.Length_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.Width, PackagingItemListFields.Width_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.Height, PackagingItemListFields.Height_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.CADDrawing, PackagingItemListFields.CADDrawing_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.Structure, PackagingItemListFields.Structure_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.StructureColor, PackagingItemListFields.StructureColor_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.BackSeam, PackagingItemListFields.BackSeam_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.WebWidth, PackagingItemListFields.WebWidth_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.ExactCutOff, PackagingItemListFields.ExactCutOff_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.BagFace, PackagingItemListFields.BagFace_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.Unwind, PackagingItemListFields.Unwind_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.Description, PackagingItemListFields.Description_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.CompassListItemId, PackagingItemListFields.CompassListItemId, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.SpecificationNo, PackagingItemListFields.SpecificationNo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.PurchasedIntoLocation, PackagingItemListFields.PurchasedIntoLocation_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.NetWeight, PackagingItemListFields.NetWeight_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.FilmMaxRollOD, PackagingItemListFields.FilmMaxRollOD_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.FilmRollID, PackagingItemListFields.FilmRollID_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.FilmPrintStyle, PackagingItemListFields.FilmPrintStyle_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.FilmStyle, PackagingItemListFields.FilmStyle_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.CorrugatedPrintStyle, PackagingItemListFields.CorrugatedPrintStyle_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.GraphicsChangeRequired, PackagingItemListFields.GraphicsChangeRequired_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.ExternalGraphicsVendor, PackagingItemListFields.ExternalGraphicsVendor_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, PackagingItemListFields.GraphicsBrief, PackagingItemListFields.GraphicsBrief_DisplayName, false, false, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldDateTime(splist, PackagingItemListFields.BOMEffectiveDate, PackagingItemListFields.BOMEffectiveDate_DisplayName, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, PackagingItemListFields.FinalGraphicsDescription, PackagingItemListFields.FinalGraphicsDescription_DisplayName, false, false, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.ConfirmedNLEA, PackagingItemListFields.ConfirmedNLEA_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.KosherLabelRequired, PackagingItemListFields.KosherLabelRequired_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.EstimatedNumberOfColors, PackagingItemListFields.EstimatedNumberOfColors_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.BlockForDateCode, PackagingItemListFields.BlockForDateCode_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.ConfirmedDielineRestrictions, PackagingItemListFields.ConfirmedDielineRestrictions_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.RenderingProvided, PackagingItemListFields.RenderingProvided_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.PlatesShipped, PackagingItemListFields.PlatesShipped_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.PackUnit, PackagingItemListFields.PackUnit_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.MRPC, PackagingItemListFields.MRPC_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, PackagingItemListFields.Deleted, PackagingItemListFields.Deleted_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }

                #region Graphics Processing Dates
                if (SetupUtilities.CreateFieldDateTime(splist, PackagingItemListFields.Graphics_Routing_ModifiedDate, PackagingItemListFields.Graphics_Routing_ModifiedDate_DisplayName, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldDateTime(splist, PackagingItemListFields.Graphics_RoutingReleased_ModifiedDate, PackagingItemListFields.Graphics_RoutingReleased_ModifiedDate_DisplayName, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldDateTime(splist, PackagingItemListFields.Graphics_PDFApproved_ModifiedDate, PackagingItemListFields.Graphics_PDFApproved_ModifiedDate_DisplayName, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldDateTime(splist, PackagingItemListFields.Graphics_PlatesShipped_ModifiedDate, PackagingItemListFields.Graphics_PlatesShipped_ModifiedDate_DisplayName, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, PackagingItemListFields.Graphics_Notes, PackagingItemListFields.Graphics_Notes_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.TransferSEMIMakePackLocations, PackagingItemListFields.TransferSEMIMakePackLocations_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.ParentID, PackagingItemListFields.ParentID_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.PECompleted, PackagingItemListFields.PECompleted_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.PE2Completed, PackagingItemListFields.PE2Completed_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.ProcCompleted, PackagingItemListFields.ProcCompleted_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.CostingQuoteDate, PackagingItemListFields.CostingQuoteDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.ForecastComments, PackagingItemListFields.ForecastComments_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.VendorNumber, PackagingItemListFields.VendorNumber_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.InkCoveragePercentage, PackagingItemListFields.InkCoveragePercentage_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.QuantityQuote, PackagingItemListFields.QuantityQuote_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.ReviewPrinterSupplier, PackagingItemListFields.ReviewPrinterSupplier_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                /// <summary>
                /// Component Costing Quote fields 
                /// </summary>

                if (SetupUtilities.CreateField(splist, PackagingItemListFields.First90Days, PackagingItemListFields.First90Days_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.CommentForecast, PackagingItemListFields.CommentForecast_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.RequestedDueDate, PackagingItemListFields.RequestedDueDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.StandardOrderingQuantity, PackagingItemListFields.StandardOrderingQuantity_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.OrderUOM, PackagingItemListFields.OrderUOM_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.Incoterms, PackagingItemListFields.Incoterms_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.XferOfOwnership, PackagingItemListFields.XferOfOwnership_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.PRDateCategory, PackagingItemListFields.PRDateCategory_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.VendorMaterialNumber, PackagingItemListFields.VendorMaterialNumber_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.CostingCondition, PackagingItemListFields.CostingCondition_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.CostingUnit, PackagingItemListFields.CostingUnit_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.EachesPerCostingUnit, PackagingItemListFields.EachesPerCostingUnit_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.LBPerCostingUnit, PackagingItemListFields.LBPerCostingUnit_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.CostingUnitPerPallet, PackagingItemListFields.CostingUnitPerPallet_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.StandardCost, PackagingItemListFields.StandardCost_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.ReceivingPlant, PackagingItemListFields.ReceivingPlant_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.CompCostSubmittedDate, PackagingItemListFields.CompCostSubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.MakeLocation, PackagingItemListFields.MakeLocation_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.PackLocation, PackagingItemListFields.PackLocation_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.CountryOfOrigin, PackagingItemListFields.CountryOfOrigin_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.NewFormula, PackagingItemListFields.NewFormula_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.TrialsCompleted, PackagingItemListFields.TrialsCompleted_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.ShelfLife, PackagingItemListFields.ShelfLife_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.Kosher, PackagingItemListFields.Kosher_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.Allergens, PackagingItemListFields.Allergens_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.SAPMaterialGroup, PackagingItemListFields.SAPMaterialGroup_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.ComponentContainsNLEA, PackagingItemListFields.ComponentContainsNLEA_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.Substrate, PackagingItemListFields.Substrate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.Flowthrough, PackagingItemListFields.Flowthrough_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.ImmediateSPKChange, PackagingItemListFields.ImmediateSPKChange_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.LastFormUpdated, PackagingItemListFields.LastFormUpdated_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, PackagingItemListFields.DielineLink, PackagingItemListFields.DielineLink_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, PackagingItemListFields.IngredientsNeedToClaimBioEng, PackagingItemListFields.IngredientsNeedToClaimBioEng_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                #region Transfer Semi Barcode Generation
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.ThirteenDigitCode, PackagingItemListFields.ThirteenDigitCode_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.FourteenDigitBarcode, PackagingItemListFields.FourteenDigitBarcode_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.SAPDescAbbrev, PackagingItemListFields.SAPDescAbbrev_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                //New First for Hierarchy
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.PHL1, PackagingItemListFields.PHL1_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.PHL2, PackagingItemListFields.PHL2_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.Brand, PackagingItemListFields.Brand_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.ProfitCenter, PackagingItemListFields.ProfitCenter_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region BE QRC Form
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.UPCAssociated, PackagingItemListFields.UPCAssociated_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.UPCAssociatedManualEntry, PackagingItemListFields.UPCAssociatedManualEntry_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.BioEngLabelingRequired, PackagingItemListFields.BioEngLabelingRequired_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, PackagingItemListFields.FlowthroughMaterialsSpecs, PackagingItemListFields.FlowthroughMaterialsSpecs_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Proc
                if (SetupUtilities.CreateField(splist, PackagingItemListFields.IsAllProcInfoCorrect, PackagingItemListFields.IsAllProcInfoCorrect_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, PackagingItemListFields.WhatProcInfoHasChanged, PackagingItemListFields.WhatProcInfoHasChanged_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, PackagingItemListFields.NewPrinterSupplierForLocation, PackagingItemListFields.NewPrinterSupplierForLocation_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                #endregion

                if (needsListUpdate)
                    splist.Update();

            }
            catch (Exception ex)
            {

            }
        }
        private void CreateCompassPackMeasurementsList()
        {
            try
            {
                SPList splist = web.Lists.TryGetList(GlobalConstants.LIST_CompassPackMeasurementsListName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, GlobalConstants.LIST_CompassPackMeasurementsListName, GlobalConstants.LIST_CompassPackMeasurementsListName);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.CompassListItemId, CompassPackMeasurementsFields.CompassListItemId, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.ParentComponentId, CompassPackMeasurementsFields.ParentComponentId, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassPackMeasurementsFields.Deleted, CompassPackMeasurementsFields.Deleted_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }

                #region Material Numbers Pack

                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.CaseCube, CompassPackMeasurementsFields.CaseCube_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.CaseDimensionHeight, CompassPackMeasurementsFields.CaseDimensionHeight_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.CaseDimensionLength, CompassPackMeasurementsFields.CaseDimensionLength_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.CaseDimensionWidth, CompassPackMeasurementsFields.CaseDimensionWidth_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.CaseGrossWeight, CompassPackMeasurementsFields.CaseGrossWeight_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.CaseNetWeight, CompassPackMeasurementsFields.CaseNetWeight_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.CasePack, CompassPackMeasurementsFields.CasePack_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.CasesPerLayer, CompassPackMeasurementsFields.CasesPerLayer_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.CasesPerPallet, CompassPackMeasurementsFields.CasesPerPallet_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.DisplayDimensionsHeight, CompassPackMeasurementsFields.DisplayDimensionsHeight_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.DisplayDimensionsLength, CompassPackMeasurementsFields.DisplayDimensionsLength_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.DisplayDimensionsWidth, CompassPackMeasurementsFields.DisplayDimensionsWidth_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.DoubleStackable, CompassPackMeasurementsFields.DoubleStackable_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.LayersPerPallet, CompassPackMeasurementsFields.LayersPerPallet_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.NetUnitWeight, CompassPackMeasurementsFields.NetUnitWeight_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.PackTrialComments, CompassPackMeasurementsFields.PackTrialComments_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.PackTrialNeeded, CompassPackMeasurementsFields.PackTrialNeeded_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.PackTrialResult, CompassPackMeasurementsFields.PackTrialResult_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.PalletCube, CompassPackMeasurementsFields.PalletCube_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.PalletDimensionsHeight, CompassPackMeasurementsFields.PalletDimensionsHeight_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.PalletDimensionsLength, CompassPackMeasurementsFields.PalletDimensionsLength_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.PalletDimensionsWidth, CompassPackMeasurementsFields.PalletDimensionsWidth_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.PalletGrossWeight, CompassPackMeasurementsFields.PalletGrossWeight_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.PalletWeight, CompassPackMeasurementsFields.PalletWeight_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.SalesCaseDimensionsHeight, CompassPackMeasurementsFields.SalesCaseDimensionsHeight_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.SalesCaseDimensionsLength, CompassPackMeasurementsFields.SalesCaseDimensionsLength_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.SalesCaseDimensionsWidth, CompassPackMeasurementsFields.SalesCaseDimensionsWidth_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.SalesUnitDimensionsHeight, CompassPackMeasurementsFields.SalesUnitDimensionsHeight_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.SalesUnitDimensionsLength, CompassPackMeasurementsFields.SalesUnitDimensionsLength_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.SalesUnitDimensionsWidth, CompassPackMeasurementsFields.SalesUnitDimensionsWidth_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.SetUpDimensionsHeight, CompassPackMeasurementsFields.SetUpDimensionsHeight_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.SetUpDimensionsLength, CompassPackMeasurementsFields.SetUpDimensionsLength_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.SetUpDimensionsWidth, CompassPackMeasurementsFields.SetUpDimensionsWidth_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.UnitDimensionHeight, CompassPackMeasurementsFields.UnitDimensionHeight_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.UnitDimensionLength, CompassPackMeasurementsFields.UnitDimensionLength_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.UnitDimensionWidth, CompassPackMeasurementsFields.UnitDimensionWidth_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.UnitDimensionWidth, CompassPackMeasurementsFields.UnitDimensionWidth_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.ParentID, CompassPackMeasurementsFields.ParentID_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.PalletPatternChange, CompassPackMeasurementsFields.PalletPatternChange_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.SAPSpecsChange, CompassPackMeasurementsFields.SAPSpecsChange_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                //New Fields for PLM Changes
                if (SetupUtilities.CreateFieldNote(splist, CompassPackMeasurementsFields.NotesSpec, CompassPackMeasurementsFields.NotesSpec_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.PackSpecNumber, CompassPackMeasurementsFields.PackSpecNumber_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassPackMeasurementsFields.PalletSpecNumber, CompassPackMeasurementsFields.PalletSpecNumber_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, CompassPackMeasurementsFields.PalletSpecLink, CompassPackMeasurementsFields.PalletSpecLink_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                if (needsListUpdate)
                    splist.Update();

            }
            catch (Exception ex)
            {

            }
        }
        private void CreateCompassShipperFinishedGoodList()
        {
            try
            {
                SPList splist = web.Lists.TryGetList(GlobalConstants.LIST_CompassShipperFinishedGoodListName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, GlobalConstants.LIST_CompassShipperFinishedGoodListName, GlobalConstants.LIST_CompassShipperFinishedGoodListName);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, ShipperFinishedGoodListFields.CompassListItemId, ShipperFinishedGoodListFields.CompassListItemId, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ShipperFinishedGoodListFields.FGItemNumber, ShipperFinishedGoodListFields.FGItemNumber_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ShipperFinishedGoodListFields.FGItemDescription, ShipperFinishedGoodListFields.FGItemDescription_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ShipperFinishedGoodListFields.FGItemNumberUnits, ShipperFinishedGoodListFields.FGItemNumberUnits_DisplayName, SPFieldType.Integer, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ShipperFinishedGoodListFields.FGItemOuncesPerUnit, ShipperFinishedGoodListFields.FGItemOuncesPerUnit_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ShipperFinishedGoodListFields.FGPackUnit, ShipperFinishedGoodListFields.FGPackUnit_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, ShipperFinishedGoodListFields.FGDeleted, ShipperFinishedGoodListFields.FGDeleted_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ShipperFinishedGoodListFields.FGShelfLife, ShipperFinishedGoodListFields.FGShelfLife_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ShipperFinishedGoodListFields.IngredientsNeedToClaimBioEng, ShipperFinishedGoodListFields.IngredientsNeedToClaimBioEng_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (needsListUpdate)
                    splist.Update();

            }
            catch (Exception ex)
            {

            }
        }
        private void CreateCompassMixesList()
        {
            try
            {
                SPList splist = web.Lists.TryGetList(GlobalConstants.LIST_CompassMixesListName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, GlobalConstants.LIST_CompassMixesListName, GlobalConstants.LIST_CompassMixesListName);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, MixesListFields.CompassListItemId, MixesListFields.CompassListItemId, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, MixesListFields.ItemNumber, MixesListFields.ItemNumber_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MixesListFields.ItemDescription, MixesListFields.ItemDescription_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MixesListFields.NumberOfPieces, MixesListFields.NumberOfPieces_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MixesListFields.OuncesPerPiece, MixesListFields.OuncesPerPiece_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, MixesListFields.MixDeleted, MixesListFields.MixDeleted_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }

                if (needsListUpdate)
                    splist.Update();
            }
            catch (Exception ex)
            {

            }
        }
        private void CreateCompassProjectDecisionsList()
        {
            try
            {
                SPList splist = web.Lists.TryGetList(GlobalConstants.LIST_ProjectDecisionsListName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, GlobalConstants.LIST_ProjectDecisionsListName, GlobalConstants.LIST_ProjectDecisionsListName);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, CompassProjectDecisionsListFields.CompassListItemId, CompassProjectDecisionsListFields.CompassListItemId, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.NewMaterialsinBOM, CompassProjectDecisionsListFields.NewMaterialsinBOM_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.NewTransferSemi, CompassProjectDecisionsListFields.NewTransferSemi_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.NetworkMoveTransferSemi, CompassProjectDecisionsListFields.NetworkMoveTransferSemi_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.NewPurCandySemi, CompassProjectDecisionsListFields.NewPurCandySemi_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.NewFilmLabelRigidPlasticinBOM, CompassProjectDecisionsListFields.NewFilmLabelRigidPlasticinBOM_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.NewCorrugatedPaperboardinBOM, CompassProjectDecisionsListFields.NewCorrugatedPaperboardinBOM_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }

                #region Initial Approval Form fields

                if (SetupUtilities.CreateFieldNote(splist, CompassProjectDecisionsListFields.SrOBMApproval_Comments, CompassProjectDecisionsListFields.SrOBMApproval_Comments_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, CompassProjectDecisionsListFields.SrOBMApproval_CostingReviewComments, CompassProjectDecisionsListFields.SrOBMApproval_CostingReviewComments_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, CompassProjectDecisionsListFields.SrOBMApproval_CapacityReviewComments, CompassProjectDecisionsListFields.SrOBMApproval_CapacityReviewComments_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SrOBMApproval_Decision, CompassProjectDecisionsListFields.SrOBMApproval_Decision_DisplayName, false, approvalStates))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SrOBMApproval_CostingDecision, CompassProjectDecisionsListFields.SrOBMApproval_CostingDecision_DisplayName, false, approvalStates))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SrOBMApproval_CapacityDecision, CompassProjectDecisionsListFields.SrOBMApproval_CapacityDecision_DisplayName, false, approvalStates))
                {
                    needsListUpdate = true;
                }
                //if (SetupUtilities.CreateFieldNote(splist, CompassProjectDecisionsListFields.SrOBMApproval2_Comments, CompassProjectDecisionsListFields.SrOBMApproval2_Comments_DisplayName, false, false, false))
                //{
                //    needsListUpdate = true;
                //}
                if (SetupUtilities.CreateField(splist, CompassProjectDecisionsListFields.SrOBMApproval2_Decision, CompassProjectDecisionsListFields.SrOBMApproval2_Decision_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                //if (SetupUtilities.CreateFieldNote(splist, CompassProjectDecisionsListFields.SrOBMApproval2_Approval, CompassProjectDecisionsListFields.SrOBMApproval2_Approval_DisplayName, false, false, false))
                //{
                //    needsListUpdate = true;
                //}
                #endregion

                #region Initial Capacity Form fields

                if (SetupUtilities.CreateFieldNote(splist, CompassProjectDecisionsListFields.InitialCapacity_AcceptanceComments, CompassProjectDecisionsListFields.InitialCapacity_AcceptanceComments, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassProjectDecisionsListFields.InitialCapacity_Decision, CompassProjectDecisionsListFields.InitialCapacity_Decision_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, CompassProjectDecisionsListFields.InitialCapacity_CapacityRiskComments, CompassProjectDecisionsListFields.InitialCapacity_CapacityRiskComments_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassProjectDecisionsListFields.InitialCapacity_MakeIssues, CompassProjectDecisionsListFields.InitialCapacity_MakeIssues_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassProjectDecisionsListFields.InitialCapacity_PackIssues, CompassProjectDecisionsListFields.InitialCapacity_PackIssues_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Initial Costing Review Form fields
                if (SetupUtilities.CreateFieldNote(splist, CompassProjectDecisionsListFields.InitialCosting_Comments, CompassProjectDecisionsListFields.InitialCosting_Comments_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassProjectDecisionsListFields.InitialCosting_Decision, CompassProjectDecisionsListFields.InitialCosting_Decision_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassProjectDecisionsListFields.InitialCosting_GrossMarginAccurate, CompassProjectDecisionsListFields.InitialCosting_GrossMarginAccurate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region Prelim SAP Initial Item Setup
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.PrelimSAPSetup_OpenSalesFPCO, CompassProjectDecisionsListFields.PrelimSAPSetup_OpenSalesFPCO_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.PrelimSAPSetup_OpenSalesSELL, CompassProjectDecisionsListFields.PrelimSAPSetup_OpenSalesSELL_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.PrelimSAPSetup_OpenSalesFERQ, CompassProjectDecisionsListFields.PrelimSAPSetup_OpenSalesFERQ_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region SAP Initial Item Setup
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPInitSet_EmptyTurnkeyAtFC01, CompassProjectDecisionsListFields.SAPInitSet_EmptyTurnkeyAtFC01_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPInitSet_EmptyTurnkeyPCsAtFC01, CompassProjectDecisionsListFields.SAPInitSet_EmptyTurnkeyPCsAtFC01_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPInitSet_ExtPCToSalesOrg0001, CompassProjectDecisionsListFields.SAPInitSet_ExtPCToSalesOrg0001_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPInitSet_ExtTSToSalesOrg0001, CompassProjectDecisionsListFields.SAPInitSet_ExtTSToSalesOrg0001_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPInitSet_ExtTSToSalesOrg1000, CompassProjectDecisionsListFields.SAPInitSet_ExtTSToSalesOrg1000_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPInitSet_ExtPCToSalesOrg1000, CompassProjectDecisionsListFields.SAPInitSet_ExtPCToSalesOrg1000_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPInitSet_ExtTSToSalesOrg2000, CompassProjectDecisionsListFields.SAPInitSet_ExtTSToSalesOrg2000_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPInitSet_ExtFGToSalesOrg0001, CompassProjectDecisionsListFields.SAPInitSet_ExtFGToSalesOrg0001_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPInitSet_ExtFGToSalesOrg1000, CompassProjectDecisionsListFields.SAPInitSet_ExtFGToSalesOrg1000_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPInitSet_ExtFGToCompSale2000, CompassProjectDecisionsListFields.SAPInitSet_ExtFGToCompSale2000_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPInitSet_ExtFGToSalesOrg2000, CompassProjectDecisionsListFields.SAPInitSet_ExtFGToSalesOrg2000_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region SAPBOMSetup
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_FinishedGoodBOMSetup, CompassProjectDecisionsListFields.SAPBOMSetup_FinishedGoodBOMSetup_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_NewMaterialNumbersCreated, CompassProjectDecisionsListFields.SAPBOMSetup_NewMaterialNumbersCreated_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_ContBuildFGBOM, CompassProjectDecisionsListFields.SAPBOMSetup_ContBuildFGBOM_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_TransferSemiBOMSetup, CompassProjectDecisionsListFields.SAPBOMSetup_TransferSemiBOMSetup_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_TransferMatNumCreatd, CompassProjectDecisionsListFields.SAPBOMSetup_TransferMatNumCreatd_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_TSSAPSpecsChangeComp, CompassProjectDecisionsListFields.SAPBOMSetup_TransferSAPSpecsChange_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_FGSAPSpecsChangeComp, CompassProjectDecisionsListFields.SAPBOMSetup_FGSAPSpecsChange_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_HardSoftTransition, CompassProjectDecisionsListFields.SAPBOMSetup_HardSoftTransition_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_TurnkeyFGMaterialMasterCreated, CompassProjectDecisionsListFields.SAPBOMSetup_TurnkeyFGMaterialMasterCreated_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_CompleteFGBOMCreated, CompassProjectDecisionsListFields.SAPBOMSetup_CompleteFGBOMCreated_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_CompleteTSBOMCreated, CompassProjectDecisionsListFields.SAPBOMSetup_CompleteTSBOMCreated_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_PackMatsCreatedInPackLoc, CompassProjectDecisionsListFields.SAPBOMSetup_PackMatsCreatedInPackLoc_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_FGBOMCreatedInNewPackLoc, CompassProjectDecisionsListFields.SAPBOMSetup_FGBOMCreatedInNewPackLoc_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_SPKUpdatedInDCsPack, CompassProjectDecisionsListFields.SAPBOMSetup_SPKUpdatedInDCsPack_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_TSCompsCreatedInNewMPLoc, CompassProjectDecisionsListFields.SAPBOMSetup_TSCompsCreatedInNewMPLoc_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_TSCompsExtendedInNewMPLoc, CompassProjectDecisionsListFields.SAPBOMSetup_TSCompsExtendedInNewMPLoc_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_SPKsUpdatedPerDeployment, CompassProjectDecisionsListFields.SAPBOMSetup_SPKsUpdatedPerDeployment_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_TSFGBOMCreatedInNewMakeLoc, CompassProjectDecisionsListFields.SAPBOMSetup_TSFGBOMCreatedInNewMakeLoc_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_SPKUpdatedInDCsMake, CompassProjectDecisionsListFields.SAPBOMSetup_SPKUpdatedInDCsMake_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_ProdVersionCreated, CompassProjectDecisionsListFields.SAPBOMSetup_ProdVersionCreated_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_CreateNewPURCNDYSAPMatNum, CompassProjectDecisionsListFields.SAPBOMSetup_CreateNewPURCNDYSAPMatNum_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_NewFGBOMCreated, CompassProjectDecisionsListFields.SAPBOMSetup_NewFGBOMCreated_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_NewTSMaterialNumbersCreated, CompassProjectDecisionsListFields.SAPBOMSetup_NewTSMaterialNumbersCreated_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_NewTSComponentPackNumsCreated, CompassProjectDecisionsListFields.SAPBOMSetup_NewTSComponentPackNumsCreated_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_InitialFGBOMCreated, CompassProjectDecisionsListFields.SAPBOMSetup_InitialFGBOMCreated_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_InitialTSBOMCreated, CompassProjectDecisionsListFields.SAPBOMSetup_InitialTSBOMCreated_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_FGSubConBOMCreated, CompassProjectDecisionsListFields.SAPBOMSetup_FGSubConBOMCreated_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_ExtendFGToDCs, CompassProjectDecisionsListFields.SAPBOMSetup_ExtendFGToDCs_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_VerifyFGBOMInDCs, CompassProjectDecisionsListFields.SAPBOMSetup_VerifyFGBOMInDCs_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_GS1Calculator, CompassProjectDecisionsListFields.SAPBOMSetup_GS1Calculator_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_FGPrivateLable, CompassProjectDecisionsListFields.SAPBOMSetup_FGPrivateLable_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_FGDCFP07, CompassProjectDecisionsListFields.SAPBOMSetup_FGDCFP07_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_FGDCFP13, CompassProjectDecisionsListFields.SAPBOMSetup_FGDCFP13_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_FGSPKOthers, CompassProjectDecisionsListFields.SAPBOMSetup_FGSPKOthers_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_VerifyPrivateLabel, CompassProjectDecisionsListFields.SAPBOMSetup_VerifyPrivateLabel_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_VerifyFGDCFP07, CompassProjectDecisionsListFields.SAPBOMSetup_VerifyFGDCFP07_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_VerifyFGDCFP13, CompassProjectDecisionsListFields.SAPBOMSetup_VerifyFGDCFP13_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_VerifyFGSPKOthers, CompassProjectDecisionsListFields.SAPBOMSetup_VerifyFGSPKOthers_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_ExtendFGHL12Brand, CompassProjectDecisionsListFields.SAPBOMSetup_ExtendFGHL12Brand_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_ApplySemiHL12Brand, CompassProjectDecisionsListFields.SAPBOMSetup_ApplySemiHL12Brand_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_AddOldComp, CompassProjectDecisionsListFields.SAPBOMSetup_AddOldComp_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_AddZSTOMatEntry, CompassProjectDecisionsListFields.SAPBOMSetup_AddZSTOMatEntry_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region SAP Complete Item Setup Item
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPCmpltItmStup_CmpltFGMtrlMstr, CompassProjectDecisionsListFields.SAPCmpltItmStup_CmpltFGMtrlMstr_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPCmpltItmStup_TurnkeyFGMMCrtd, CompassProjectDecisionsListFields.SAPCmpltItmStup_TurnkeyFGMMCrtd_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPCmpltItmStup_FGSAPSpChCmpltd, CompassProjectDecisionsListFields.SAPCmpltItmStup_FGSAPSpChCmpltd_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPCmpltItmStup_CmpltTSBOM, CompassProjectDecisionsListFields.SAPCmpltItmStup_CmpltTSBOM_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_ExtProfitCenterToDC, CompassProjectDecisionsListFields.SAPBOMSetup_ExtProfitCenterToDC_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassProjectDecisionsListFields.SAPBOMSetup_ClckNewTSPCPrftCntr, CompassProjectDecisionsListFields.SAPBOMSetup_ClckNewTSPCPrftCntr_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region PM Second review Form

                if (SetupUtilities.CreateField(splist, CompassProjectDecisionsListFields.OBMSecondReview_Check, CompassProjectDecisionsListFields.OBMSecondReview_Check_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassProjectDecisionsListFields.OBMSecondReview_Comments, CompassProjectDecisionsListFields.OBMSecondReview_Comments, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassProjectDecisionsListFields.OBMSecondReview_Concern, CompassProjectDecisionsListFields.OBMSecondReview_Concern_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region Project Rejection
                if (SetupUtilities.CreateField(splist, CompassProjectDecisionsListFields.RejectedBy, CompassProjectDecisionsListFields.RejectedBy_DisplayName, SPFieldType.User, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassProjectDecisionsListFields.RejectedByName, CompassProjectDecisionsListFields.RejectedByName_DisplayName, SPFieldType.Text, true))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassProjectDecisionsListFields.FunctionRejected, CompassProjectDecisionsListFields.FunctionRejected_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, CompassProjectDecisionsListFields.ReasonForRejection, CompassProjectDecisionsListFields.ReasonForRejection_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, CompassProjectDecisionsListFields.OBMSecondReview_Check, CompassProjectDecisionsListFields.OBMSecondReview_Check_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region Emails table fields
                if (SetupUtilities.CreateFieldNote(splist, CompassProjectDecisionsListFields.PackagingCompInitialSetup, CompassProjectDecisionsListFields.PackagingCompInitialSetup_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, CompassProjectDecisionsListFields.PackagingCompSAPsetupBOM, CompassProjectDecisionsListFields.PackagingCompSAPsetupBOM_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, CompassProjectDecisionsListFields.NewTSPackLocations, CompassProjectDecisionsListFields.NewTSPackLocations_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, CompassProjectDecisionsListFields.NetworkMoveTSPackLocations, CompassProjectDecisionsListFields.NetworkMoveTSPackLocations_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, CompassProjectDecisionsListFields.NMTSsForSAPIntItemSetup, CompassProjectDecisionsListFields.NMTSsForSAPIntItemSetup_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, CompassProjectDecisionsListFields.NewPurCandySemis, CompassProjectDecisionsListFields.NewPurCandySemis_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, CompassProjectDecisionsListFields.NewPurCandySemisPackLocation, CompassProjectDecisionsListFields.NewPurCandySemisPackLocation_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, CompassProjectDecisionsListFields.NMPurCandySemisPackLocation, CompassProjectDecisionsListFields.NMPurCandySemisPackLocation_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, CompassProjectDecisionsListFields.NewPackagingComponents, CompassProjectDecisionsListFields.NewPackagingComponents_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, CompassProjectDecisionsListFields.NewWarehouseDetails, CompassProjectDecisionsListFields.NewWarehouseDetails_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, CompassProjectDecisionsListFields.NewStdCostEntryDetails, CompassProjectDecisionsListFields.NewStdCostEntryDetails_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, CompassProjectDecisionsListFields.NewSAPCostDetails, CompassProjectDecisionsListFields.NewSAPCostDetails_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldNote(splist, CompassProjectDecisionsListFields.CancellationReasons, CompassProjectDecisionsListFields.CancellationReasons_DisplayName, false, false, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                if (needsListUpdate)
                    splist.Update();
            }
            catch (Exception ex)
            {

            }
        }
        private void CreateCompassApprovalList(string listName, string description)
        {
            try
            {
                SPList splist = web.Lists.TryGetList(listName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, listName, description);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields - We are adding all dates as strings since there is a limit of 48 dates per list. We will break this limit with the amount of dates
                // being gathered.
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, ApprovalListFields.CompassListItemId, ApprovalListFields.CompassListItemId, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }

                #region Item Proposal Form fields - IPF Form
                if (SetupUtilities.CreateField(splist, ApprovalListFields.IPF_StartDate, ApprovalListFields.IPF_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.IPF_SubmittedBy, ApprovalListFields.IPF_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.IPF_SubmittedDate, ApprovalListFields.IPF_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.IPF_ModifiedBy, ApprovalListFields.IPF_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.IPF_ModifiedDate, ApprovalListFields.IPF_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.IPF_NumberResubmits, ApprovalListFields.IPF_NumberResubmits_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Initial Approval Form fields
                if (SetupUtilities.CreateField(splist, ApprovalListFields.IPF_NumberApproverDays, ApprovalListFields.IPF_NumberApproverDays_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SrOBMApproval_ModifiedBy, ApprovalListFields.SrOBMApproval_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SrOBMApproval_ModifiedDate, ApprovalListFields.SrOBMApproval_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SrOBMApproval_SubmittedBy, ApprovalListFields.SrOBMApproval_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SrOBMApproval_SubmittedDate, ApprovalListFields.SrOBMApproval_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SrOBMApproval_StartDate, ApprovalListFields.SrOBMApproval_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.SrOBMApproval2_ModifiedBy, ApprovalListFields.SrOBMApproval2_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SrOBMApproval2_ModifiedDate, ApprovalListFields.SrOBMApproval2_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SrOBMApproval2_SubmittedBy, ApprovalListFields.SrOBMApproval2_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SrOBMApproval2_SubmittedDate, ApprovalListFields.SrOBMApproval2_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SrOBMApproval2_StartDate, ApprovalListFields.SrOBMApproval2_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region SAP Item Request Form fields

                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPInitialSetup_ModifiedBy, ApprovalListFields.SAPInitialSetup_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPInitialSetup_ModifiedDate, ApprovalListFields.SAPInitialSetup_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPInitialSetup_StartDate, ApprovalListFields.SAPInitialSetup_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPInitialSetup_SubmittedDate, ApprovalListFields.SAPInitialSetup_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPInitialSetup_SubmittedBy, ApprovalListFields.SAPInitialSetup_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region Preliminary SAP Item Request Form fields

                if (SetupUtilities.CreateField(splist, ApprovalListFields.PrelimSAPInitialSetup_ModifiedBy, ApprovalListFields.PrelimSAPInitialSetup_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.PrelimSAPInitialSetup_ModifiedDate, ApprovalListFields.PrelimSAPInitialSetup_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.PrelimSAPInitialSetup_StartDate, ApprovalListFields.PrelimSAPInitialSetup_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.PrelimSAPInitialSetup_SubmittedDate, ApprovalListFields.PrelimSAPInitialSetup_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.PrelimSAPInitialSetup_SubmittedBy, ApprovalListFields.PrelimSAPInitialSetup_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Distribution Form fields

                if (SetupUtilities.CreateField(splist, ApprovalListFields.Distribution_ModifiedBy, ApprovalListFields.Distribution_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.Distribution_ModifiedDate, ApprovalListFields.Distribution_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.Distribution_SubmittedBy, ApprovalListFields.Distribution_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.Distribution_SubmittedDate, ApprovalListFields.Distribution_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.Distribution_StartDate, ApprovalListFields.Distribution_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Trade Promo Group Form fields

                if (SetupUtilities.CreateField(splist, ApprovalListFields.TradePromo_ModifiedBy, ApprovalListFields.TradePromo_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.TradePromo_ModifiedDate, ApprovalListFields.TradePromo_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.TradePromo_SubmittedBy, ApprovalListFields.TradePromo_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.TradePromo_SubmittedDate, ApprovalListFields.TradePromo_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.TradePromo_StartDate, ApprovalListFields.TradePromo_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Make Pack Fields - Make Pack Form

                if (SetupUtilities.CreateField(splist, ApprovalListFields.Operations_ModifiedBy, ApprovalListFields.Operations_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.Operations_ModifiedDate, ApprovalListFields.Operations_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.Operations_SubmittedBy, ApprovalListFields.Operations_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.Operations_SubmittedDate, ApprovalListFields.Operations_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.Operations_StartDate, ApprovalListFields.Operations_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }


                #endregion

                #region External Manufacturing Form fields - ExternalMfg Form

                if (SetupUtilities.CreateField(splist, ApprovalListFields.ExternalMfg_ModifiedBy, ApprovalListFields.ExternalMfg_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ExternalMfg_ModifiedDate, ApprovalListFields.ExternalMfg_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.ExternalMfg_SubmittedBy, ApprovalListFields.ExternalMfg_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ExternalMfg_SubmittedDate, ApprovalListFields.ExternalMfg_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ExternalMfg_StartDate, ApprovalListFields.ExternalMfg_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                #endregion

                #region Quality Assurance Form fields - QA Form

                if (SetupUtilities.CreateField(splist, ApprovalListFields.QA_ModifiedBy, ApprovalListFields.QA_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.QA_ModifiedDate, ApprovalListFields.QA_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.QA_SubmittedBy, ApprovalListFields.QA_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.QA_SubmittedDate, ApprovalListFields.QA_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.QA_StartDate, ApprovalListFields.QA_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                #endregion

                #region PM Review 1 fields
                if (SetupUtilities.CreateField(splist, ApprovalListFields.OBMReview1_ModifiedBy, ApprovalListFields.OBMReview1_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.OBMReview1_ModifiedDate, ApprovalListFields.OBMReview1_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.OBMReview1_SubmittedBy, ApprovalListFields.OBMReview1_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.OBMReview1_SubmittedDate, ApprovalListFields.OBMReview1_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.OBMReview1_StartDate, ApprovalListFields.OBMReview1_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Packaging Form Fields 

                if (SetupUtilities.CreateField(splist, ApprovalListFields.BOMSetupPE_ModifiedBy, ApprovalListFields.BOMSetupPE_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BOMSetupPE_ModifiedDate, ApprovalListFields.BOMSetupPE_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BOMSetupPE_SubmittedBy, ApprovalListFields.BOMSetupPE_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BOMSetupPE_SubmittedDate, ApprovalListFields.BOMSetupPE_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BOMSetupPE_StartDate, ApprovalListFields.BOMSetupPE_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BOMSetupProc_ModifiedBy, ApprovalListFields.BOMSetupProc_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BOMSetupProc_ModifiedDate, ApprovalListFields.BOMSetupProc_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BOMSetupProc_SubmittedBy, ApprovalListFields.BOMSetupProc_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BOMSetupProc_SubmittedDate, ApprovalListFields.BOMSetupProc_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BOMSetupProc_StartDate, ApprovalListFields.BOMSetupProc_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BOMSetupPE2_ModifiedBy, ApprovalListFields.BOMSetupPE2_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BOMSetupPE2_ModifiedDate, ApprovalListFields.BOMSetupPE2_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BOMSetupPE2_SubmittedBy, ApprovalListFields.BOMSetupPE2_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BOMSetupPE2_SubmittedDate, ApprovalListFields.BOMSetupPE2_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BOMSetupPE2_StartDate, ApprovalListFields.BOMSetupPE2_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BOMSetupPE3_ModifiedBy, ApprovalListFields.BOMSetupPE3_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BOMSetupPE3_ModifiedDate, ApprovalListFields.BOMSetupPE3_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BOMSetupPE3_SubmittedBy, ApprovalListFields.BOMSetupPE3_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BOMSetupPE3_SubmittedDate, ApprovalListFields.BOMSetupPE3_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BOMSetupPE3_StartDate, ApprovalListFields.BOMSetupPE3_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region PM Review 2 fields
                if (SetupUtilities.CreateField(splist, ApprovalListFields.OBMReview2_ModifiedBy, ApprovalListFields.OBMReview2_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.OBMReview2_ModifiedDate, ApprovalListFields.OBMReview2_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.OBMReview2_SubmittedBy, ApprovalListFields.OBMReview2_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.OBMReview2_SubmittedDate, ApprovalListFields.OBMReview2_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.OBMReview2_StartDate, ApprovalListFields.OBMReview2_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                #endregion

                #region Graphics Form fields 

                if (SetupUtilities.CreateField(splist, ApprovalListFields.GRAPHICS_ModifiedBy, ApprovalListFields.GRAPHICS_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.GRAPHICS_ModifiedDate, ApprovalListFields.GRAPHICS_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.GRAPHICS_SubmittedBy, ApprovalListFields.GRAPHICS_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.GRAPHICS_SubmittedDate, ApprovalListFields.GRAPHICS_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.GRAPHICS_StartDate, ApprovalListFields.GRAPHICS_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                #endregion

                #region SAPBOMSetup Form fields

                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPBOMSetup_ModifiedBy, ApprovalListFields.SAPBOMSetup_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPBOMSetup_ModifiedDate, ApprovalListFields.SAPBOMSetup_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPBOMSetup_SubmittedBy, ApprovalListFields.SAPBOMSetup_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPBOMSetup_SubmittedDate, ApprovalListFields.SAPBOMSetup_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPBOMSetup_StartDate, ApprovalListFields.SAPBOMSetup_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                #endregion
                #region SAPCompleteItemSetup
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPCompleteItemSetup_StartDate, ApprovalListFields.SAPCompleteItemSetup_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPCompleteItemSetup_ModifiedDate, ApprovalListFields.SAPCompleteItemSetup_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPCompleteItemSetup_ModifiedBy, ApprovalListFields.SAPCompleteItemSetup_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPCompleteItemSetup_SubmittedDate, ApprovalListFields.SAPCompleteItemSetup_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPCompleteItemSetup_SubmittedBy, ApprovalListFields.SAPCompleteItemSetup_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region BOMSetupMaterialWarehouse
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BOMSetupMaterialWarehouse_StartDate, ApprovalListFields.BOMSetupMaterialWarehouse_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BOMSetupMaterialWarehouse_ModifiedDate, ApprovalListFields.BOMSetupMaterialWarehouse_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BOMSetupMaterialWarehouse_ModifiedBy, ApprovalListFields.BOMSetupMaterialWarehouse_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BOMSetupMaterialWarehouse_SubmittedDate, ApprovalListFields.BOMSetupMaterialWarehouse_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BOMSetupMaterialWarehouse_SubmittedBy, ApprovalListFields.BOMSetupMaterialWarehouse_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region FGPackSpec Form fields

                if (SetupUtilities.CreateField(splist, ApprovalListFields.FGPackSpec_ModifiedBy, ApprovalListFields.FGPackSpec_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.FGPackSpec_ModifiedDate, ApprovalListFields.FGPackSpec_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.FGPackSpec_SubmittedBy, ApprovalListFields.FGPackSpec_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.FGPackSpec_SubmittedDate, ApprovalListFields.FGPackSpec_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.FGPackSpec_StartDate, ApprovalListFields.FGPackSpec_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                #endregion

                #region CostingQuote Form fields

                if (SetupUtilities.CreateField(splist, ApprovalListFields.CompCostFLRP_ModifiedBy, ApprovalListFields.CompCostFLRP_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.CompCostFLRP_ModifiedDate, ApprovalListFields.CompCostFLRP_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.CompCostFLRP_SubmittedBy, ApprovalListFields.CompCostFLRP_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.CompCostFLRP_SubmittedDate, ApprovalListFields.CompCostFLRP_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.CompCostFLRP_StartDate, ApprovalListFields.CompCostFLRP_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }


                if (SetupUtilities.CreateField(splist, ApprovalListFields.CompCostCorrPaper_ModifiedBy, ApprovalListFields.CompCostCorrPaper_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.CompCostCorrPaper_ModifiedDate, ApprovalListFields.CompCostCorrPaper_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.CompCostCorrPaper_SubmittedBy, ApprovalListFields.CompCostCorrPaper_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.CompCostCorrPaper_SubmittedDate, ApprovalListFields.CompCostCorrPaper_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.CompCostCorrPaper_StartDate, ApprovalListFields.CompCostCorrPaper_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.CompCostSeasonal_ModifiedBy, ApprovalListFields.CompCostSeasonal_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.CompCostSeasonal_ModifiedDate, ApprovalListFields.CompCostSeasonal_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.CompCostSeasonal_SubmittedBy, ApprovalListFields.CompCostSeasonal_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.CompCostSeasonal_SubmittedDate, ApprovalListFields.CompCostSeasonal_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.CompCostSeasonal_StartDate, ApprovalListFields.CompCostSeasonal_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                #endregion

                // Final Setup Phase Notifications

                #region SAP Routing Setup Notification
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPRoutingSetup_ModifiedBy, ApprovalListFields.SAPRoutingSetup_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPRoutingSetup_ModifiedDate, ApprovalListFields.SAPRoutingSetup_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPRoutingSetup_SubmittedBy, ApprovalListFields.SAPRoutingSetup_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPRoutingSetup_SubmittedDate, ApprovalListFields.SAPRoutingSetup_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPRoutingSetup_StartDate, ApprovalListFields.SAPRoutingSetup_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region SAP Costing Details Notification
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPCostingDetails_ModifiedBy, ApprovalListFields.SAPCostingDetails_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPCostingDetails_ModifiedDate, ApprovalListFields.SAPCostingDetails_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPCostingDetails_SubmittedBy, ApprovalListFields.SAPCostingDetails_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPCostingDetails_SubmittedDate, ApprovalListFields.SAPCostingDetails_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPCostingDetails_StartDate, ApprovalListFields.SAPCostingDetails_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region SAP Warehouse Info Notification
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPWarehouseInfo_ModifiedBy, ApprovalListFields.SAPWarehouseInfo_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPWarehouseInfo_ModifiedDate, ApprovalListFields.SAPWarehouseInfo_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPWarehouseInfo_SubmittedBy, ApprovalListFields.SAPWarehouseInfo_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPWarehouseInfo_SubmittedDate, ApprovalListFields.SAPWarehouseInfo_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.SAPWarehouseInfo_StartDate, ApprovalListFields.SAPWarehouseInfo_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Standard Cost Entry Notification
                if (SetupUtilities.CreateField(splist, ApprovalListFields.StandardCostEntry_ModifiedBy, ApprovalListFields.StandardCostEntry_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.StandardCostEntry_ModifiedDate, ApprovalListFields.StandardCostEntry_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.StandardCostEntry_SubmittedBy, ApprovalListFields.StandardCostEntry_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.StandardCostEntry_SubmittedDate, ApprovalListFields.StandardCostEntry_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.StandardCostEntry_StartDate, ApprovalListFields.StandardCostEntry_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Cost Finished Good Notification
                if (SetupUtilities.CreateField(splist, ApprovalListFields.CostFinishedGood_ModifiedBy, ApprovalListFields.CostFinishedGood_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.CostFinishedGood_ModifiedDate, ApprovalListFields.CostFinishedGood_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.CostFinishedGood_SubmittedBy, ApprovalListFields.CostFinishedGood_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.CostFinishedGood_SubmittedDate, ApprovalListFields.CostFinishedGood_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.CostFinishedGood_StartDate, ApprovalListFields.CostFinishedGood_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Final Costing Review Notification
                if (SetupUtilities.CreateField(splist, ApprovalListFields.FinalCostingReview_ModifiedBy, ApprovalListFields.FinalCostingReview_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.FinalCostingReview_ModifiedDate, ApprovalListFields.FinalCostingReview_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.FinalCostingReview_SubmittedBy, ApprovalListFields.FinalCostingReview_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.FinalCostingReview_SubmittedDate, ApprovalListFields.FinalCostingReview_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.FinalCostingReview_StartDate, ApprovalListFields.FinalCostingReview_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Purchased PO Notification
                if (SetupUtilities.CreateField(splist, ApprovalListFields.PurchasedPO_ModifiedBy, ApprovalListFields.PurchasedPO_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.PurchasedPO_ModifiedDate, ApprovalListFields.PurchasedPO_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.PurchasedPO_SubmittedBy, ApprovalListFields.PurchasedPO_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.PurchasedPO_SubmittedDate, ApprovalListFields.PurchasedPO_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.PurchasedPO_StartDate, ApprovalListFields.PurchasedPO_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Remove SAP Blocks
                if (SetupUtilities.CreateField(splist, ApprovalListFields.RemoveSAPBlocks_ModifiedBy, ApprovalListFields.RemoveSAPBlocks_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.RemoveSAPBlocks_ModifiedDate, ApprovalListFields.RemoveSAPBlocks_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.RemoveSAPBlocks_SubmittedBy, ApprovalListFields.RemoveSAPBlocks_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.RemoveSAPBlocks_SubmittedDate, ApprovalListFields.RemoveSAPBlocks_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.RemoveSAPBlocks_StartDate, ApprovalListFields.RemoveSAPBlocks_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Customer POs can be Entered Notification
                if (SetupUtilities.CreateField(splist, ApprovalListFields.CustomerPO_ModifiedBy, ApprovalListFields.CustomerPO_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.CustomerPO_ModifiedDate, ApprovalListFields.CustomerPO_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.CustomerPO_SubmittedBy, ApprovalListFields.CustomerPO_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.CustomerPO_SubmittedDate, ApprovalListFields.CustomerPO_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.CustomerPO_StartDate, ApprovalListFields.CustomerPO_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Materials Received Check
                if (SetupUtilities.CreateField(splist, ApprovalListFields.MaterialsReceivedChk_SubmittedBy, ApprovalListFields.MaterialsReceivedChk_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.MaterialsReceivedChk_SubmittedDate, ApprovalListFields.MaterialsReceivedChk_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.MaterialsReceivedChk_StartDate, ApprovalListFields.MaterialsReceivedChk_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region First Production Check
                if (SetupUtilities.CreateField(splist, ApprovalListFields.FirstProductionChk_SubmittedBy, ApprovalListFields.FirstProductionChk_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.FirstProductionChk_SubmittedDate, ApprovalListFields.FirstProductionChk_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.FirstProductionChk_StartDate, ApprovalListFields.FirstProductionChk_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Distribution Center Check
                if (SetupUtilities.CreateField(splist, ApprovalListFields.DistributionCenterChk_SubmittedBy, ApprovalListFields.DistributionCenterChk_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.DistributionCenterChk_SubmittedDate, ApprovalListFields.DistributionCenterChk_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.DistributionCenterChk_StartDate, ApprovalListFields.DistributionCenterChk_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Other Workflow State fields
                if (SetupUtilities.CreateField(splist, ApprovalListFields.OnHold_ModifiedDate, ApprovalListFields.OnHold_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.OnHold_ModifiedBy, ApprovalListFields.OnHold_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.PreProduction_ModifiedDate, ApprovalListFields.PreProduction_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.PreProduction_ModifiedBy, ApprovalListFields.PreProduction_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.Completed_ModifiedDate, ApprovalListFields.Completed_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.Completed_ModifiedBy, ApprovalListFields.Completed_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.Cancelled_ModifiedDate, ApprovalListFields.Cancelled_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.Cancelled_ModifiedBy, ApprovalListFields.Cancelled_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProductionCompleted_ModifiedDate, ApprovalListFields.ProductionCompleted_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProductionCompleted_ModifiedBy, ApprovalListFields.ProductionCompleted_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                #endregion

                if (needsListUpdate)
                    splist.Update();

            }
            catch (Exception ex)
            {

            }
        }
        private void CreateCompassApprovalList2(string listName, string description)
        {
            try
            {
                SPList splist = web.Lists.TryGetList(listName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, listName, description);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields - We are adding all dates as strings since there is a limit of 48 dates per list. We will break this limit with the amount of dates
                // being gathered.
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, ApprovalListFields.CompassListItemId, ApprovalListFields.CompassListItemId, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }

                #region Packaging Form Fields 
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcAncillary_SubmittedBy, ApprovalListFields.ProcAncillary_SubmittedBy, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcAncillary_SubmittedDate, ApprovalListFields.ProcAncillary_SubmittedDate, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcCorrugated_SubmittedBy, ApprovalListFields.ProcCorrugated_SubmittedBy, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcCorrugated_SubmittedDate, ApprovalListFields.ProcCorrugated_SubmittedDate, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcPurchased_SubmittedBy, ApprovalListFields.ProcPurchased_SubmittedBy, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcPurchased_SubmittedDate, ApprovalListFields.ProcPurchased_SubmittedDate, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcFilm_SubmittedBy, ApprovalListFields.ProcFilm_SubmittedBy, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcFilm_SubmittedDate, ApprovalListFields.ProcFilm_SubmittedDate, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcLabel_SubmittedBy, ApprovalListFields.ProcLabel_SubmittedBy, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcLabel_SubmittedDate, ApprovalListFields.ProcLabel_SubmittedDate, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcMetal_SubmittedBy, ApprovalListFields.ProcMetal_SubmittedBy, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcMetal_SubmittedDate, ApprovalListFields.ProcMetal_SubmittedDate, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcOther_SubmittedBy, ApprovalListFields.ProcOther_SubmittedBy, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcOther_SubmittedDate, ApprovalListFields.ProcOther_SubmittedDate, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcPaperboard_SubmittedBy, ApprovalListFields.ProcPaperboard_SubmittedBy, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcPaperboard_SubmittedDate, ApprovalListFields.ProcPaperboard_SubmittedDate, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcRigidPlastic_SubmittedBy, ApprovalListFields.ProcRigidPlastic_SubmittedBy, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcRigidPlastic_SubmittedDate, ApprovalListFields.ProcRigidPlastic_SubmittedDate, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcExternal_SubmittedBy, ApprovalListFields.ProcExternal_SubmittedBy, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcExternal_SubmittedDate, ApprovalListFields.ProcExternal_SubmittedDate, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcSeasonal_SubmittedBy, ApprovalListFields.ProcSeasonal_SubmittedBy, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcSeasonal_SubmittedDate, ApprovalListFields.ProcSeasonal_SubmittedDate, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcCoMan_SubmittedBy, ApprovalListFields.ProcCoMan_SubmittedBy, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcCoMan_SubmittedDate, ApprovalListFields.ProcCoMan_SubmittedDate, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcNovelty_SubmittedBy, ApprovalListFields.ProcNovelty_SubmittedBy, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcNovelty_SubmittedDate, ApprovalListFields.ProcNovelty_SubmittedDate, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcExternalAncillary_SubmittedBy, ApprovalListFields.ProcExternalAncillary_SubmittedBy, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcExternalAncillary_SubmittedDate, ApprovalListFields.ProcExternalAncillary_SubmittedDate, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcExternalCorrugated_SubmittedBy, ApprovalListFields.ProcExternalCorrugated_SubmittedBy, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcExternalCorrugated_SubmittedDate, ApprovalListFields.ProcExternalCorrugated_SubmittedDate, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcExternalPurchased_SubmittedBy, ApprovalListFields.ProcExternalPurchased_SubmittedBy, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcExternalPurchased_SubmittedDate, ApprovalListFields.ProcExternalPurchased_SubmittedDate, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcExternalFilm_SubmittedBy, ApprovalListFields.ProcExternalFilm_SubmittedBy, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcExternalFilm_SubmittedDate, ApprovalListFields.ProcExternalFilm_SubmittedDate, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcExternalLabel_SubmittedBy, ApprovalListFields.ProcExternalLabel_SubmittedBy, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcExternalLabel_SubmittedDate, ApprovalListFields.ProcExternalLabel_SubmittedDate, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcExternalMetal_SubmittedBy, ApprovalListFields.ProcExternalMetal_SubmittedBy, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcExternalMetal_SubmittedDate, ApprovalListFields.ProcExternalMetal_SubmittedDate, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcExternalOther_SubmittedBy, ApprovalListFields.ProcExternalOther_SubmittedBy, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcExternalOther_SubmittedDate, ApprovalListFields.ProcExternalOther_SubmittedDate, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcExternalPaperboard_SubmittedBy, ApprovalListFields.ProcExternalPaperboard_SubmittedBy, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcExternalPaperboard_SubmittedDate, ApprovalListFields.ProcExternalPaperboard_SubmittedDate, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcExternalRigidPlastic_SubmittedBy, ApprovalListFields.ProcExternalRigidPlastic_SubmittedBy, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProcExternalRigidPlastic_SubmittedDate, ApprovalListFields.ProcExternalRigidPlastic_SubmittedDate, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #region BEQRC
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BEQRC_StartDate, ApprovalListFields.BEQRC_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BEQRC_ModifiedDate, ApprovalListFields.BEQRC_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BEQRC_ModifiedBy, ApprovalListFields.BEQRC_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BEQRC_SubmittedDate, ApprovalListFields.BEQRC_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.BEQRC_SubmittedBy, ApprovalListFields.BEQRC_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region EstimatedPricing
                if (SetupUtilities.CreateField(splist, ApprovalListFields.EstPricing_StartDate, ApprovalListFields.EstPricing_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.EstPricing_ModifiedDate, ApprovalListFields.EstPricing_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.EstPricing_ModifiedBy, ApprovalListFields.EstPricing_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.EstPricing_SubmittedDate, ApprovalListFields.EstPricing_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.EstPricing_SubmittedBy, ApprovalListFields.EstPricing_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Estimated Bracket Pricing
                if (SetupUtilities.CreateField(splist, ApprovalListFields.EstBracketPricing_StartDate, ApprovalListFields.EstBracketPricing_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.EstBracketPricing_ModifiedDate, ApprovalListFields.EstBracketPricing_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.EstBracketPricing_ModifiedBy, ApprovalListFields.EstBracketPricing_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.EstBracketPricing_SubmittedDate, ApprovalListFields.EstBracketPricing_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.EstBracketPricing_SubmittedBy, ApprovalListFields.EstBracketPricing_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #endregion

                if (needsListUpdate)
                    splist.Update();

            }
            catch (Exception ex)
            {

            }
        }
        private void CreateSAPApprovalList(string listName, string description)
        {
            try
            {
                SPList splist = web.Lists.TryGetList(listName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, listName, description);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields - We are adding all dates as strings since there is a limit of 48 dates per list. We will break this limit with the amount of dates
                // being gathered.
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, SAPApprovalListFields.CompassListItemId, SAPApprovalListFields.CompassListItemId, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                // Final Setup Phase Notifications
                #region SAP Routing Setup Notification

                if (SetupUtilities.CreateField(splist, SAPApprovalListFields.SAPRoutingSetup_ModifiedDate, SAPApprovalListFields.SAPRoutingSetup_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, SAPApprovalListFields.SAPRoutingSetup_SubmittedDate, SAPApprovalListFields.SAPRoutingSetup_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPApprovalListFields.SAPRoutingSetup_StartDate, SAPApprovalListFields.SAPRoutingSetup_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region SAP Costing Details Notification

                if (SetupUtilities.CreateField(splist, SAPApprovalListFields.SAPCostingDetails_ModifiedDate, SAPApprovalListFields.SAPCostingDetails_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, SAPApprovalListFields.SAPCostingDetails_SubmittedDate, SAPApprovalListFields.SAPCostingDetails_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPApprovalListFields.SAPCostingDetails_StartDate, SAPApprovalListFields.SAPCostingDetails_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region SAP Warehouse Info Notification

                if (SetupUtilities.CreateField(splist, SAPApprovalListFields.SAPWarehouseInfo_ModifiedDate, SAPApprovalListFields.SAPWarehouseInfo_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, SAPApprovalListFields.SAPWarehouseInfo_SubmittedDate, SAPApprovalListFields.SAPWarehouseInfo_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPApprovalListFields.SAPWarehouseInfo_StartDate, SAPApprovalListFields.SAPWarehouseInfo_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Standard Cost Entry Notification

                if (SetupUtilities.CreateField(splist, SAPApprovalListFields.StandardCostEntry_ModifiedDate, SAPApprovalListFields.StandardCostEntry_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, SAPApprovalListFields.StandardCostEntry_SubmittedDate, SAPApprovalListFields.StandardCostEntry_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPApprovalListFields.StandardCostEntry_StartDate, SAPApprovalListFields.StandardCostEntry_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                /*Waiting to see if these are needed
                #region Cost Finished Good Notification
                if (SetupUtilities.CreateField(splist, ApprovalListFields.CostFinishedGood_ModifiedBy, ApprovalListFields.CostFinishedGood_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.CostFinishedGood_ModifiedDate, ApprovalListFields.CostFinishedGood_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.CostFinishedGood_SubmittedBy, ApprovalListFields.CostFinishedGood_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.CostFinishedGood_SubmittedDate, ApprovalListFields.CostFinishedGood_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.CostFinishedGood_StartDate, ApprovalListFields.CostFinishedGood_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Final Costing Review Notification
                if (SetupUtilities.CreateField(splist, ApprovalListFields.FinalCostingReview_ModifiedBy, ApprovalListFields.FinalCostingReview_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.FinalCostingReview_ModifiedDate, ApprovalListFields.FinalCostingReview_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.FinalCostingReview_SubmittedBy, ApprovalListFields.FinalCostingReview_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.FinalCostingReview_SubmittedDate, ApprovalListFields.FinalCostingReview_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.FinalCostingReview_StartDate, ApprovalListFields.FinalCostingReview_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Purchased PO Notification
                if (SetupUtilities.CreateField(splist, ApprovalListFields.PurchasedPO_ModifiedBy, ApprovalListFields.PurchasedPO_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.PurchasedPO_ModifiedDate, ApprovalListFields.PurchasedPO_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.PurchasedPO_SubmittedBy, ApprovalListFields.PurchasedPO_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.PurchasedPO_SubmittedDate, ApprovalListFields.PurchasedPO_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.PurchasedPO_StartDate, ApprovalListFields.PurchasedPO_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Remove SAP Blocks
                if (SetupUtilities.CreateField(splist, ApprovalListFields.RemoveSAPBlocks_ModifiedBy, ApprovalListFields.RemoveSAPBlocks_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.RemoveSAPBlocks_ModifiedDate, ApprovalListFields.RemoveSAPBlocks_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.RemoveSAPBlocks_SubmittedBy, ApprovalListFields.RemoveSAPBlocks_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.RemoveSAPBlocks_SubmittedDate, ApprovalListFields.RemoveSAPBlocks_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.RemoveSAPBlocks_StartDate, ApprovalListFields.RemoveSAPBlocks_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Customer POs can be Entered Notification
                if (SetupUtilities.CreateField(splist, ApprovalListFields.CustomerPO_ModifiedBy, ApprovalListFields.CustomerPO_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.CustomerPO_ModifiedDate, ApprovalListFields.CustomerPO_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.CustomerPO_SubmittedBy, ApprovalListFields.CustomerPO_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.CustomerPO_SubmittedDate, ApprovalListFields.CustomerPO_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.CustomerPO_StartDate, ApprovalListFields.CustomerPO_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Materials Received Check
                if (SetupUtilities.CreateField(splist, ApprovalListFields.MaterialsReceivedChk_SubmittedBy, ApprovalListFields.MaterialsReceivedChk_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.MaterialsReceivedChk_SubmittedDate, ApprovalListFields.MaterialsReceivedChk_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.MaterialsReceivedChk_StartDate, ApprovalListFields.MaterialsReceivedChk_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region First Production Check
                if (SetupUtilities.CreateField(splist, ApprovalListFields.FirstProductionChk_SubmittedBy, ApprovalListFields.FirstProductionChk_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.FirstProductionChk_SubmittedDate, ApprovalListFields.FirstProductionChk_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.FirstProductionChk_StartDate, ApprovalListFields.FirstProductionChk_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Distribution Center Check
                if (SetupUtilities.CreateField(splist, ApprovalListFields.DistributionCenterChk_SubmittedBy, ApprovalListFields.DistributionCenterChk_SubmittedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.DistributionCenterChk_SubmittedDate, ApprovalListFields.DistributionCenterChk_SubmittedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ApprovalListFields.DistributionCenterChk_StartDate, ApprovalListFields.DistributionCenterChk_StartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion



                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProductionCompleted_ModifiedDate, ApprovalListFields.ProductionCompleted_ModifiedDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ApprovalListFields.ProductionCompleted_ModifiedBy, ApprovalListFields.ProductionCompleted_ModifiedBy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                #endregion
                */
                if (needsListUpdate)
                    splist.Update();

            }
            catch (Exception ex)
            {

            }
        }
        private void CreateCompassListViews()
        {
            SPList spList = web.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

            #region Agenda Everyday View
            SPView view = spList.Views.Cast<SPView>().FirstOrDefault(w => w.Title.ToUpper().Equals(GlobalConstants.VIEW_AgendaViewEveryday.ToUpper()));
            if (view == null)
            {
                var viewFields = new System.Collections.Specialized.StringCollection();
                AddAgendaFields(viewFields);

                string query = "<OrderBy><FieldRef Name=\"Modified\" Ascending=\"FALSE\"></FieldRef></OrderBy><Where><And><And><And><And><Neq><FieldRef Name='" + CompassListFields.ProductHierarchyLevel2 + "' /><Value Type=\"Text\">EAS14-NEW</Value></Neq><Neq><FieldRef Name='" + CompassListFields.ProductHierarchyLevel2 + "' /><Value Type=\"Text\">VAL14-NEW</Value></Neq></And><Neq><FieldRef Name='" + CompassListFields.ProductHierarchyLevel2 + "' /><Value Type=\"Text\">HALLWEEN 2014</Value></Neq></And><Neq><FieldRef Name='" + CompassListFields.ProductHierarchyLevel2 + "' /><Value Type=\"Text\">XMAS14-NEW</Value></Neq></And><Neq><FieldRef Name='" + CompassListFields.ProductHierarchyLevel2 + "' /><Value Type=\"Text\">SUMMER14-NEW</Value></Neq></And></Where>";
                spList.Views.Add(GlobalConstants.VIEW_AgendaViewEveryday, viewFields, query, 20, true, false);
                spList.Update();
            }
            #endregion

            #region Agenda Seasonal View
            view = spList.Views.Cast<SPView>().FirstOrDefault(w => w.Title.ToUpper().Equals(GlobalConstants.VIEW_AgendaViewSeasonal.ToUpper()));
            if (view == null)
            {
                var viewFields = new System.Collections.Specialized.StringCollection();
                AddAgendaFields(viewFields);

                string query = "<OrderBy><FieldRef Name=\"Modified\" Ascending=\"FALSE\"></FieldRef></OrderBy><Where><Or><Or><Or><Or><Eq><FieldRef Name='" + CompassListFields.ProductHierarchyLevel2 + "' /><Value Type=\"Text\">EAS14-NEW</Value></Eq><Eq><FieldRef Name='" + CompassListFields.ProductHierarchyLevel2 + "' /><Value Type=\"Text\">VAL14-NEW</Value></Eq></Or><Eq><FieldRef Name='" + CompassListFields.ProductHierarchyLevel2 + "' /><Value Type=\"Text\">HALLWEEN 2014</Value></Eq></Or><Eq><FieldRef Name='" + CompassListFields.ProductHierarchyLevel2 + "' /><Value Type=\"Text\">XMAS14-NEW</Value></Eq></Or><Eq><FieldRef Name='" + CompassListFields.ProductHierarchyLevel2 + "' /><Value Type=\"Text\">SUMMER14-NEW</Value></Eq></Or></Where>";
                spList.Views.Add(GlobalConstants.VIEW_AgendaViewSeasonal, viewFields, query, 20, true, false);
                spList.Update();
            }
            #endregion

            #region All Open Projects View
            view = spList.Views.Cast<SPView>().FirstOrDefault(w => w.Title.ToUpper().Equals(GlobalConstants.VIEW_AllOpenProjects.ToUpper()));
            if (view == null)
            {
                var viewFields = new System.Collections.Specialized.StringCollection();
                viewFields.Add(CompassListFields.ProjectStatus);
                viewFields.Add(CompassListFields.MaterialGroup1Brand);
                viewFields.Add(CompassListFields.ProductHierarchyLevel1);
                viewFields.Add(CompassListFields.ProductHierarchyLevel2);
                viewFields.Add(CompassListFields.SAPItemNumber);
                viewFields.Add(CompassListFields.SAPDescription);
                viewFields.Add(CompassListFields.FirstShipDate);
                viewFields.Add(CompassListFields.ManufacturingLocation);
                //viewFields.Add(CompassListFields.OBM_NLEAPosted);
                //viewFields.Add(CompassListFields.OBM_ProofPosted);
                //viewFields.Add(CompassListFields.OBM_GraphicsFileSent);
                //viewFields.Add(CompassListFields.OBM_ProofApproved);
                //viewFields.Add(CompassListFields.OBM_PlatesToPrinter);
                //viewFields.Add(CompassListFields.OBM_SAPStatus);
                //viewFields.Add(CompassListFields.OBM_PackagingArrivalDate);
                //viewFields.Add(CompassListFields.OBM_EstimatedProductionDate);
                //viewFields.Add(CompassListFields.OBM_PlannedProductionDate);
                //viewFields.Add(CompassListFields.OBM_EstimatedGraphicHours);
                viewFields.Add(CompassListFields.RevisedFirstShipDate);
                //viewFields.Add(CompassListFields.OBM_PEExpectedCompletionDate);
                //viewFields.Add(CompassListFields.OBM_GraphicsExpectedCompletionDate);
                viewFields.Add(CompassListFields.AnnualProjectedUnits);
                viewFields.Add(CompassListFields.AnnualProjectedDollars);
                viewFields.Add(CompassListFields.ItemConcept);
                //viewFields.Add(CompassListFields.OBM_CustomGrouping);

                string query = "<OrderBy><FieldRef Name=\"Modified\" Ascending=\"FALSE\"></FieldRef></OrderBy>";
                spList.Views.Add(GlobalConstants.VIEW_AllOpenProjects, viewFields, query, 20, true, false);
                spList.Update();
            }
            view = spList.Views.Cast<SPView>().FirstOrDefault(w => w.Title.ToUpper().Equals(GlobalConstants.VIEW_MyOpenTasksDash.ToUpper()));
            if (view == null)
            {
                var viewFields = new System.Collections.Specialized.StringCollection();
                viewFields.Add(CompassListFields.ProjectNumber);
                viewFields.Add(CompassListFields.SAPItemNumber);
                viewFields.Add(CompassListFields.WorkflowPhase);
                viewFields.Add(CompassListFields.SAPDescription);
                viewFields.Add(CompassListFields.RevisedFirstShipDate);
                viewFields.Add(CompassListFields.FirstProductionDate);
                viewFields.Add(CompassListFields.ProjectTypeSubCategory);
                viewFields.Add(CompassListFields.ProductHierarchyLevel1);
                viewFields.Add(CompassListFields.MaterialGroup1Brand);
                viewFields.Add(CompassListFields.ManufacturingLocation);
                viewFields.Add(CompassListFields.PackingLocation);
                viewFields.Add(CompassListFields.PM);
                viewFields.Add(CompassListFields.PackagingEngineerLead);
                viewFields.Add(CompassListFields.Initiator);
                viewFields.Add(CompassListFields.InitiatorName);
                viewFields.Add(CompassListFields.TimelineType);
                viewFields.Add(CompassListFields.Customer);
                viewFields.Add(CompassListFields.SubmittedDate);
                viewFields.Add(CompassListFields.LikeFGItemDescription);
                viewFields.Add(CompassListFields.ProjectType);

                string query = "<OrderBy><FieldRef Name=\"Modified\" Ascending=\"FALSE\"></FieldRef></OrderBy>";
                spList.Views.Add(GlobalConstants.VIEW_MyOpenTasksDash, viewFields, query, 200, true, false);
                spList.Update();
            }
            #endregion
            //@Fatimah
            /*#region Brand Manager View
            view = spList.Views.Cast<SPView>().FirstOrDefault(w => w.Title.ToUpper().Equals(GlobalConstants.VIEW_BrandManager.ToUpper()));
            if (view == null)
            {
                var viewFields = new System.Collections.Specialized.StringCollection();
                viewFields.Add(CompassListFields.ProjectStatus);
                viewFields.Add(CompassListFields.MaterialGroup1Brand);
                viewFields.Add(CompassListFields.ProductHierarchyLevel1);
                viewFields.Add(CompassListFields.ProductHierarchyLevel2);
                viewFields.Add(CompassListFields.SAPItemNumber);
                viewFields.Add(CompassListFields.SAPDescription);
                viewFields.Add(CompassListFields.Customer);
                viewFields.Add(CompassListFields.FirstShipDate);
                viewFields.Add(CompassListFields.ManufacturingLocation);
                viewFields.Add(CompassListFields.PackingLocation);
                viewFields.Add(CompassListFields.RevisedFirstShipDate);
                viewFields.Add(CompassListFields.AnnualProjectedUnits);
                viewFields.Add(CompassListFields.AnnualProjectedDollars);
                viewFields.Add(CompassListFields.ItemConcept);

                string query = "<OrderBy><FieldRef Name=\"Modified\" Ascending=\"FALSE\"></FieldRef></OrderBy><Where><Eq><FieldRef Name='" + CompassListFields.BrandManager + "' /><Value Type=\"Integer\"><UserID Type=\"Integer\"/></Value></Eq></Where>";
                spList.Views.Add(GlobalConstants.VIEW_BrandManager, viewFields, query, 20, true, false);
                spList.Update();
            }
            #endregion*/

            #region PM View
            view = spList.Views.Cast<SPView>().FirstOrDefault(w => w.Title.ToUpper().Equals(GlobalConstants.VIEW_PM.ToUpper()));
            if (view == null)
            {
                var viewFields = new System.Collections.Specialized.StringCollection();
                viewFields.Add(CompassListFields.ProjectStatus);
                viewFields.Add(CompassListFields.MaterialGroup1Brand);
                viewFields.Add(CompassListFields.ProductHierarchyLevel1);
                viewFields.Add(CompassListFields.ProductHierarchyLevel2);
                viewFields.Add(CompassListFields.SAPItemNumber);
                viewFields.Add(CompassListFields.SAPDescription);
                viewFields.Add(CompassListFields.Customer);
                viewFields.Add(CompassListFields.FirstShipDate);
                viewFields.Add(CompassListFields.ManufacturingLocation);
                viewFields.Add(CompassListFields.PackingLocation);
                //viewFields.Add(CompassListFields.OBM_NLEAPosted);
                //viewFields.Add(CompassListFields.OBM_ProofPosted);
                //viewFields.Add(CompassListFields.OBM_GraphicsFileSent);
                //viewFields.Add(CompassListFields.OBM_ProofApproved);
                //viewFields.Add(CompassListFields.OBM_PlatesToPrinter);
                //viewFields.Add(CompassListFields.OBM_SAPStatus);
                //viewFields.Add(CompassListFields.OBM_PackagingArrivalDate);
                //viewFields.Add(CompassListFields.OBM_EstimatedProductionDate);
                //viewFields.Add(CompassListFields.OBM_PlannedProductionDate);
                //viewFields.Add(CompassListFields.OBM_EstimatedGraphicHours);
                viewFields.Add(CompassListFields.RevisedFirstShipDate);
                //viewFields.Add(CompassListFields.OBM_PEExpectedCompletionDate);
                //viewFields.Add(CompassListFields.OBM_GraphicsExpectedCompletionDate);
                viewFields.Add(CompassListFields.AnnualProjectedUnits);
                viewFields.Add(CompassListFields.AnnualProjectedDollars);
                viewFields.Add(CompassListFields.ItemConcept);

                string query = "<OrderBy><FieldRef Name=\"Modified\" Ascending=\"FALSE\"></FieldRef></OrderBy><Where><Eq><FieldRef Name='" + CompassListFields.PM + "' /><Value Type=\"Integer\"><UserID Type=\"Integer\"/></Value></Eq></Where>";
                spList.Views.Add(GlobalConstants.VIEW_PM, viewFields, query, 20, true, false);
                spList.Update();
            }
            #endregion

            #region OBM Admin View
            view = spList.Views.Cast<SPView>().FirstOrDefault(w => w.Title.ToUpper().Equals(GlobalConstants.VIEW_OBMAdmin.ToUpper()));
            if (view == null)
            {
                var viewFields = new System.Collections.Specialized.StringCollection();
                viewFields.Add(CompassListFields.ProjectStatus);
                viewFields.Add(CompassListFields.MaterialGroup1Brand);
                viewFields.Add(CompassListFields.ProductHierarchyLevel1);
                viewFields.Add(CompassListFields.ProductHierarchyLevel2);
                viewFields.Add(CompassListFields.SAPItemNumber);
                viewFields.Add(CompassListFields.SAPDescription);
                viewFields.Add(CompassListFields.Customer);
                viewFields.Add(CompassListFields.FirstShipDate);
                viewFields.Add(CompassListFields.ManufacturingLocation);
                viewFields.Add(CompassListFields.PackingLocation);
                //viewFields.Add(CompassListFields.OBM_NLEAPosted);
                //viewFields.Add(CompassListFields.OBM_ProofPosted);
                //viewFields.Add(CompassListFields.OBM_GraphicsFileSent);
                //viewFields.Add(CompassListFields.OBM_ProofApproved);
                //viewFields.Add(CompassListFields.OBM_PlatesToPrinter);
                //viewFields.Add(CompassListFields.OBM_SAPStatus);
                //viewFields.Add(CompassListFields.OBM_PackagingArrivalDate);
                //viewFields.Add(CompassListFields.OBM_EstimatedProductionDate);
                //viewFields.Add(CompassListFields.OBM_PlannedProductionDate);
                //viewFields.Add(CompassListFields.OBM_EstimatedGraphicHours);
                viewFields.Add(CompassListFields.RevisedFirstShipDate);
                //viewFields.Add(CompassListFields.OBM_PEExpectedCompletionDate);
                //viewFields.Add(CompassListFields.OBM_GraphicsExpectedCompletionDate);
                viewFields.Add(CompassListFields.AnnualProjectedUnits);
                viewFields.Add(CompassListFields.AnnualProjectedDollars);
                viewFields.Add(CompassListFields.ItemConcept);

                string query = "<OrderBy><FieldRef Name=\"Modified\" Ascending=\"FALSE\"></FieldRef></OrderBy>";
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "OBM Admin View: " + query);
                spList.Views.Add(GlobalConstants.VIEW_OBMAdmin, viewFields, query, 20, true, false);
                spList.Update();
            }
            #endregion

            #region First Ship Date View
            view = spList.Views.Cast<SPView>().FirstOrDefault(w => w.Title.ToUpper().Equals(GlobalConstants.VIEW_FirstShipDate.ToUpper()));
            if (view == null)
            {
                var viewFields = new System.Collections.Specialized.StringCollection();
                viewFields.Add(CompassListFields.Title);
                viewFields.Add(CompassListFields.FirstShipDate);
                //viewFields.Add(CompassListFields.BrandManager);//@Fatimah
                viewFields.Add(CompassListFields.PM);
                viewFields.Add(CompassListFields.ItemConcept);

                string query = "<OrderBy><FieldRef Name=\"IPF_FirstShipDate\" /></OrderBy><Where><And><And><And><And><Geq><FieldRef Name=\"IPF_FirstShipDate\" /><Value Type=\"DateTime\"><Today /></Value></Geq><Leq><FieldRef Name=\"IPF_FirstShipDate\" /><Value Type=\"DateTime\"><Today OffsetDays=\"14\" /></Value></Leq></And><Neq><FieldRef Name=\"OBM_ProjectStatus\" /><Value Type=\"Text\">Green</Value></Neq></And><Neq><FieldRef Name=\"WorkflowStep\" /><Value Type=\"Text\">Cancelled</Value></Neq></And><Neq><FieldRef Name=\"WorkflowStep\" /><Value Type=\"Text\">Completed</Value></Neq></And></Where>";
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "First Ship Date View: " + query);
                spList.Views.Add(GlobalConstants.VIEW_FirstShipDate, viewFields, query, 20, true, false);
                spList.Update();
            }
            #endregion

            #region All Cancelled Projects View
            view = spList.Views.Cast<SPView>().FirstOrDefault(w => w.Title.ToUpper().Equals(GlobalConstants.VIEW_AllCancelledProjects.ToUpper()));
            if (view == null)
            {
                var viewFields = new System.Collections.Specialized.StringCollection();
                viewFields.Add(CompassListFields.ProductHierarchyLevel1);
                viewFields.Add(CompassListFields.ProductHierarchyLevel2);
                viewFields.Add(CompassListFields.ProjectNumber);
                viewFields.Add(CompassListFields.SAPItemNumber);
                viewFields.Add(CompassListFields.SAPDescription);
                viewFields.Add(CompassListFields.Initiator);
                viewFields.Add(CompassListFields.MaterialGroup1Brand);
                viewFields.Add(CompassListFields.Customer);
                viewFields.Add(CompassListFields.FirstShipDate);
                viewFields.Add(CompassListFields.ManufacturingLocation);
                viewFields.Add(CompassListFields.PackingLocation);
                //viewFields.Add(CompassListFields.OBM_NLEAPosted);
                viewFields.Add(CompassListFields.PackagingEngineerLead);
                //viewFields.Add(CompassListFields.OBM_ProofPosted);
                //viewFields.Add(CompassListFields.OBM_GraphicsFileSent);
                //viewFields.Add(CompassListFields.OBM_ProofApproved);
                //viewFields.Add(CompassListFields.OBM_PlatesToPrinter);
                //viewFields.Add(CompassListFields.OBM_SAPStatus);
                //viewFields.Add(CompassListFields.OBM_PackagingArrivalDate);
                //viewFields.Add(CompassListFields.OBM_EstimatedProductionDate);
                viewFields.Add(CompassListFields.RevisedFirstShipDate);
                viewFields.Add(CompassListFields.AnnualProjectedUnits);
                viewFields.Add(CompassListFields.ItemConcept);

                string query = "<OrderBy><FieldRef Name=\"Modified\" Ascending=\"FALSE\"></FieldRef></OrderBy><Where><Eq><FieldRef Name=\"WorkflowStep\" /><Value Type=\"Text\">Cancelled</Value></Eq></Where>";
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "All Cancelled Projects View: " + query);
                spList.Views.Add(GlobalConstants.VIEW_AllCancelledProjects, viewFields, query, 20, true, false);
                spList.Update();
            }
            #endregion

            #region All Completed Projects View
            view = spList.Views.Cast<SPView>().FirstOrDefault(w => w.Title.ToUpper().Equals(GlobalConstants.VIEW_AllCompletedProjects.ToUpper()));
            if (view == null)
            {
                var viewFields = new System.Collections.Specialized.StringCollection();
                viewFields.Add(CompassListFields.ProductHierarchyLevel1);
                viewFields.Add(CompassListFields.ProductHierarchyLevel2);
                viewFields.Add(CompassListFields.ProjectNumber);
                viewFields.Add(CompassListFields.SAPItemNumber);
                viewFields.Add(CompassListFields.SAPDescription);
                viewFields.Add(CompassListFields.Initiator);
                viewFields.Add(CompassListFields.MaterialGroup1Brand);
                viewFields.Add(CompassListFields.Customer);
                viewFields.Add(CompassListFields.FirstShipDate);
                viewFields.Add(CompassListFields.ManufacturingLocation);
                viewFields.Add(CompassListFields.PackingLocation);
                //                viewFields.Add(CompassListFields.OBM_NLEAPosted);
                viewFields.Add(CompassListFields.PackagingEngineerLead);
                //viewFields.Add(CompassListFields.OBM_ProofPosted);
                //viewFields.Add(CompassListFields.OBM_GraphicsFileSent);
                //viewFields.Add(CompassListFields.OBM_ProofApproved);
                //viewFields.Add(CompassListFields.OBM_PlatesToPrinter);
                //viewFields.Add(CompassListFields.OBM_SAPStatus);
                //viewFields.Add(CompassListFields.OBM_PackagingArrivalDate);
                //viewFields.Add(CompassListFields.OBM_EstimatedProductionDate);
                viewFields.Add(CompassListFields.RevisedFirstShipDate);
                viewFields.Add(CompassListFields.AnnualProjectedUnits);
                viewFields.Add(CompassListFields.ItemConcept);

                string query = "<OrderBy><FieldRef Name=\"Modified\" Ascending=\"FALSE\"></FieldRef></OrderBy><Where><Eq><FieldRef Name=\"WorkflowStep\" /><Value Type=\"Text\">Completed</Value></Eq></Where>";
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "All Completed Projects View: " + query);
                spList.Views.Add(GlobalConstants.VIEW_AllCompletedProjects, viewFields, query, 20, true, false);
                spList.Update();
            }
            #endregion

            web.Update();
        }
        private void CreateCompassEmailLoggingList()
        {
            string description = "Compass Email Logging List";
            try
            {
                SPList splist = web.Lists.TryGetList(GlobalConstants.LIST_EmailLoggingListName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, GlobalConstants.LIST_EmailLoggingListName, description);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.CompassListItemId, EmailLoggingListFields.CompassListItemId, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }

                #region Item Proposal Form fields - IPF Form
                #endregion

                #region Sr. PM Initial Approval Form
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.SrOBMApproval_EmailTo, EmailLoggingListFields.SrOBMApproval_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.SrOBMApproval_EmailDate, EmailLoggingListFields.SrOBMApproval_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region 2nd Sr. PM Initial Approval Form
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.SrOBMApproval2_EmailTo, EmailLoggingListFields.SrOBMApproval2_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.SrOBMApproval2_EmailDate, EmailLoggingListFields.SrOBMApproval2_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Initial Costing Review - InitialCosting
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.InitialCosting_EmailTo, EmailLoggingListFields.InitialCosting_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.InitialCosting_EmailDate, EmailLoggingListFields.InitialCosting_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Initial Capacity Review - InitialCapacity
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.InitialCapacity_EmailTo, EmailLoggingListFields.InitialCapacity_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.InitialCapacity_EmailDate, EmailLoggingListFields.InitialCapacity_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Trade Promo Group - TradePromo
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.TradePromo_EmailTo, EmailLoggingListFields.TradePromo_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.TradePromo_EmailDate, EmailLoggingListFields.TradePromo_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region EstimatedPricing
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.EstPricing_EmailTo, EmailLoggingListFields.EstPricing_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.EstPricing_EmailDate, EmailLoggingListFields.EstPricing_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region EstimatedBracketPricing
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.EstBracketPricing_EmailTo, EmailLoggingListFields.EstBracketPricing_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.EstBracketPricing_EmailDate, EmailLoggingListFields.EstBracketPricing_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Distribution Form - Distribution
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.Distribution_EmailTo, EmailLoggingListFields.Distribution_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.Distribution_EmailDate, EmailLoggingListFields.Distribution_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Operations Form - Operations
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.Operations_EmailTo, EmailLoggingListFields.Operations_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.Operations_EmailDate, EmailLoggingListFields.Operations_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region External Manufacturing Form - ExternalMfg
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.ExternalMfg_EmailTo, EmailLoggingListFields.ExternalMfg_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.ExternalMfg_EmailDate, EmailLoggingListFields.ExternalMfg_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region SAP Initial Item Setup Form - SAPInitialSetup
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.SAPInitialSetup_EmailTo, EmailLoggingListFields.SAPInitialSetup_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.SAPInitialSetup_EmailDate, EmailLoggingListFields.SAPInitialSetup_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region Preliminary SAP Initial Item Setup Form - PrelimSAPInitialSetup
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.PrelimSAPInitialSetup_EmailTo, EmailLoggingListFields.PrelimSAPInitialSetup_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.PrelimSAPInitialSetup_EmailDate, EmailLoggingListFields.PrelimSAPInitialSetup_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region QA Form - QA
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.QA_EmailTo, EmailLoggingListFields.QA_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.QA_EmailDate, EmailLoggingListFields.QA_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region PM Review 1 - OBMReview1
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.OBMReview1_EmailTo, EmailLoggingListFields.OBMReview1_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.OBMReview1_EmailDate, EmailLoggingListFields.OBMReview1_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Packaging Form - BOMSetupPE, BOMSetupProc, BOMSetupPE2
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.BOMSetupPE_EmailTo, EmailLoggingListFields.BOMSetupPE_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.BOMSetupPE_EmailDate, EmailLoggingListFields.BOMSetupPE_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.BOMSetupProc_EmailTo, EmailLoggingListFields.BOMSetupProc_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.BOMSetupProc_EmailDate, EmailLoggingListFields.BOMSetupProc_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.BOMSetupPE2_EmailTo, EmailLoggingListFields.BOMSetupPE2_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.BOMSetupPE2_EmailDate, EmailLoggingListFields.BOMSetupPE2_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.BOMSetupPE3_EmailTo, EmailLoggingListFields.BOMSetupPE3_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.BOMSetupPE3_EmailDate, EmailLoggingListFields.BOMSetupPE3_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region SAPBOMSetup
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.SAPBOMSetup_EmailTo, EmailLoggingListFields.SAPBOMSetup_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.SAPBOMSetup_EmailDate, EmailLoggingListFields.SAPBOMSetup_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region BOMSetupWarehouse
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.BOMSetupMaterialWarehouse_EmailTo, EmailLoggingListFields.BOMSetupMaterialWarehouse_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.BOMSetupMaterialWarehouse_EmailDate, EmailLoggingListFields.BOMSetupMaterialWarehouse_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region SAPCompleteItemSetup
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.SAPCompleteItemSetup_EmailTo, EmailLoggingListFields.SAPCompleteItemSetup_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.SAPCompleteItemSetup_EmailDate, EmailLoggingListFields.SAPCompleteItemSetup_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region PM Review 2 - OBMReview2
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.OBMReview2_EmailTo, EmailLoggingListFields.OBMReview2_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.OBMReview2_EmailDate, EmailLoggingListFields.OBMReview2_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Graphics Form - GRAPHICS
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.Graphics_EmailTo, EmailLoggingListFields.Graphics_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.Graphics_EmailDate, EmailLoggingListFields.Graphics_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region FGPackSpec
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.FGPackSpec_EmailTo, EmailLoggingListFields.FGPackSpec_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.FGPackSpec_EmailDate, EmailLoggingListFields.FGPackSpec_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region CostingQuote
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.CostingQuote_EmailTo, EmailLoggingListFields.CostingQuote_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.CostingQuote_EmailDate, EmailLoggingListFields.CostingQuote_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region PM Review 3 - OBMReview3
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.OBMReview3_EmailTo, EmailLoggingListFields.OBMReview3_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.OBMReview3_EmailDate, EmailLoggingListFields.OBMReview3_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region OnHold
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.OnHold_EmailTo, EmailLoggingListFields.OnHold_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.OnHold_EmailDate, EmailLoggingListFields.OnHold_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region PreProduction
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.PreProduction_EmailTo, EmailLoggingListFields.PreProduction_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.PreProduction_EmailDate, EmailLoggingListFields.PreProduction_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Completed
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.Completed_EmailTo, EmailLoggingListFields.Completed_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.Completed_EmailDate, EmailLoggingListFields.Completed_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Cancelled
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.Cancelled_EmailTo, EmailLoggingListFields.Cancelled_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.Cancelled_EmailDate, EmailLoggingListFields.Cancelled_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region ProductionCompleted
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.ProductionCompleted_EmailTo, EmailLoggingListFields.ProductionCompleted_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.ProductionCompleted_EmailDate, EmailLoggingListFields.ProductionCompleted_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                // Notifications
                #region R&D Team Notification
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.RnDNotification_EmailTo, EmailLoggingListFields.RnDNotification_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.RnDNotification_EmailDate, EmailLoggingListFields.RnDNotification_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Zest Pricing Notification
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.ZestPricingNotification_EmailTo, EmailLoggingListFields.ZestPricingNotification_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.ZestPricingNotification_EmailDate, EmailLoggingListFields.ZestPricingNotification_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Demand Planning Notification
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.DemandPlanningNotification_EmailTo, EmailLoggingListFields.DemandPlanningNotification_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.DemandPlanningNotification_EmailDate, EmailLoggingListFields.DemandPlanningNotification_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region International Team Notification
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.InternationalTeamNotification_EmailTo, EmailLoggingListFields.InternationalTeamNotification_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.InternationalTeamNotification_EmailDate, EmailLoggingListFields.InternationalTeamNotification_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region SAP Routing Setup Notification
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.SAPRoutingSetupNotification_EmailTo, EmailLoggingListFields.SAPRoutingSetupNotification_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.SAPRoutingSetupNotification_EmailDate, EmailLoggingListFields.SAPRoutingSetupNotification_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region BOM Active Date Notification
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.BOMActiveDateNotification_EmailTo, EmailLoggingListFields.BOMActiveDateNotification_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.BOMActiveDateNotification_EmailDate, EmailLoggingListFields.BOMActiveDateNotification_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region SAP Costing Details Notification
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.SAPCostingDetailsNotification_EmailTo, EmailLoggingListFields.SAPCostingDetailsNotification_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.SAPCostingDetailsNotification_EmailDate, EmailLoggingListFields.SAPCostingDetailsNotification_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region SAP Warehouse Info Notification
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.SAPWarehouseInfoNotification_EmailTo, EmailLoggingListFields.SAPWarehouseInfoNotification_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.SAPWarehouseInfoNotification_EmailDate, EmailLoggingListFields.SAPWarehouseInfoNotification_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Standard Cost Entry Notification
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.StandardCostEntryNotification_EmailTo, EmailLoggingListFields.StandardCostEntryNotification_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.StandardCostEntryNotification_EmailDate, EmailLoggingListFields.StandardCostEntryNotification_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Cost Finished Good Notification
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.CostFinishedGoodNotification_EmailTo, EmailLoggingListFields.CostFinishedGoodNotification_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.CostFinishedGoodNotification_EmailDate, EmailLoggingListFields.CostFinishedGoodNotification_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Final Costing Review Notification
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.FinalCostingReviewNotification_EmailTo, EmailLoggingListFields.FinalCostingReviewNotification_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.FinalCostingReviewNotification_EmailDate, EmailLoggingListFields.FinalCostingReviewNotification_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Purchased PO Notification
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.PurchasedPONotification_EmailTo, EmailLoggingListFields.PurchasedPONotification_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.PurchasedPONotification_EmailDate, EmailLoggingListFields.PurchasedPONotification_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Remove SAP Blocks
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.RemoveSAPBlocksNotification_EmailTo, EmailLoggingListFields.RemoveSAPBlocksNotification_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.RemoveSAPBlocksNotification_EmailDate, EmailLoggingListFields.RemoveSAPBlocksNotification_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region Customer POs can be Entered Notification
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.CustomerPONotification_EmailTo, EmailLoggingListFields.CustomerPONotification_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.CustomerPONotification_EmailDate, EmailLoggingListFields.CustomerPONotification_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion
                #region BEQRC - BEQRC
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.BEQRC_EmailTo, EmailLoggingListFields.BEQRC_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.BEQRC_EmailDate, EmailLoggingListFields.BEQRC_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                #region BEQRCRequest - BEQRCRequest
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.BEQRCRequest_EmailTo, EmailLoggingListFields.BEQRCRequest_EmailTo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, EmailLoggingListFields.BEQRCRequest_EmailDate, EmailLoggingListFields.BEQRCRequest_EmailDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                #endregion

                if (needsListUpdate)
                    splist.Update();

            }
            catch (Exception ex)
            {

            }
        }
        private void CreateCompassUploadsDocumentLibrary()
        {
            string description = "Compass Upload document library for importing SGS information";
            try
            {
                SPList splist = web.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassUploadsLibraryName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, GlobalConstants.DOCLIBRARY_CompassUploadsLibraryName, description);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                var needsListUpdate = false;

                if (needsListUpdate)
                    splist.Update();

            }
            catch (Exception ex)
            {

            }
        }
        private void CreateTimelineTypeList()
        {
            try
            {
                SPList splist = web.Lists.TryGetList(GlobalConstants.LIST_ProjectTimelineTypeListName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, GlobalConstants.LIST_ProjectTimelineTypeListName, GlobalConstants.LIST_ProjectTimelineTypeListName);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //update Title Field
                var field = splist.Fields[SPBuiltInFieldId.Title];
                field.Title = "WorkflowStep";
                field.Update();

                //Add Fields
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, ProjectTimelineTypeDays.PhaseNumber, ProjectTimelineTypeDays.PhaseNumber_DisplayName, SPFieldType.Integer, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineTypeDays.WorkflowOrder, ProjectTimelineTypeDays.WorkflowOrder_DisplayName, SPFieldType.Integer, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineTypeDays.Standard, ProjectTimelineTypeDays.Standard_DisplayName, SPFieldType.Integer, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineTypeDays.Expedited, ProjectTimelineTypeDays.Expedited_DisplayName, SPFieldType.Integer, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineTypeDays.Ludicrous, ProjectTimelineTypeDays.Ludicrous_DisplayName, SPFieldType.Integer, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineTypeDays.GraphicsInternalStandard, ProjectTimelineTypeDays.GraphicsInternalStandard_DisplayName, SPFieldType.Integer, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineTypeDays.GraphicsInternalExpedited, ProjectTimelineTypeDays.GraphicsInternalExpedited_DisplayName, SPFieldType.Integer, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineTypeDays.GraphicsInternalLudicrous, ProjectTimelineTypeDays.GraphicsInternalLudicrous_DisplayName, SPFieldType.Integer, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineTypeDays.GraphicsExternalStandard, ProjectTimelineTypeDays.GraphicsExternalStandard_DisplayName, SPFieldType.Integer, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineTypeDays.GraphicsExternalExpedited, ProjectTimelineTypeDays.GraphicsExternalExpedited_DisplayName, SPFieldType.Integer, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineTypeDays.GraphicsExternalLudicrous, ProjectTimelineTypeDays.GraphicsExternalLudicrous_DisplayName, SPFieldType.Integer, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineTypeDays.WorkflowQuickStep, ProjectTimelineTypeDays.WorkflowQuickStep_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineTypeDays.WorkflowExceptions, ProjectTimelineTypeDays.WorkflowExceptions_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineTypeDays.WorkflowStacks, ProjectTimelineTypeDays.WorkflowStacks_DisplayName, SPFieldType.Integer, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineTypeDays.GraphicsInternalWorkflowStacks, ProjectTimelineTypeDays.GraphicsInternalWorkflowStacks_DisplayName, SPFieldType.Integer, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineTypeDays.GraphicsExternalWorkflowStacks, ProjectTimelineTypeDays.GraphicsExternalWorkflowStacks_DisplayName, SPFieldType.Integer, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineTypeDays.WorkflowMisc, ProjectTimelineTypeDays.WorkflowMisc_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineTypeDays.GraphicsInternalWorkflowMisc, ProjectTimelineTypeDays.GraphicsInternalWorkflowMisc_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineTypeDays.GraphicsExternalWorkflowMisc, ProjectTimelineTypeDays.GraphicsExternalWorkflowMisc_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (needsListUpdate)
                    splist.Update();
            }
            catch (Exception ex)
            {

            }
        }
        private void CreateProjectTimelineUpdateList()
        {
            try
            {
                SPList splist = web.Lists.TryGetList(GlobalConstants.LIST_ProjectTimelineUpdateName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, GlobalConstants.LIST_ProjectTimelineUpdateName, GlobalConstants.LIST_ProjectTimelineUpdateName);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;
                var needsListUpdate = false;
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.compassListItemId, ProjectTimelineUpdateFields.compassListItemId, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.IPF, ProjectTimelineUpdateFields.IPF_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.SrOBMApproval, ProjectTimelineUpdateFields.SrOBMApproval_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.SrOBMApproval2, ProjectTimelineUpdateFields.SrOBMApproval2_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.TradePromo, ProjectTimelineUpdateFields.TradePromo_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.EstPricing, ProjectTimelineUpdateFields.EstPricing_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.EstBracketPricing, ProjectTimelineUpdateFields.EstBracketPricing_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.Distribution, ProjectTimelineUpdateFields.Distribution_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.Operations, ProjectTimelineUpdateFields.Operations_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.SAPInitialSetup, ProjectTimelineUpdateFields.SAPInitialSetup_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.PrelimSAPInitialSetup, ProjectTimelineUpdateFields.PrelimSAPInitialSetup_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.QA, ProjectTimelineUpdateFields.QA_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.OBMReview1, ProjectTimelineUpdateFields.OBMReview1_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.BOMSetupPE, ProjectTimelineUpdateFields.BOMSetupPE_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.BOMSetupProc, ProjectTimelineUpdateFields.BOMSetupProc_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.BOMSetupPE2, ProjectTimelineUpdateFields.BOMSetupPE2_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.BOMSetupPE3, ProjectTimelineUpdateFields.BOMSetupPE3_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.OBMReview2, ProjectTimelineUpdateFields.OBMReview2_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.GRAPHICS, ProjectTimelineUpdateFields.GRAPHICS_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.CostingQuote, ProjectTimelineUpdateFields.CostingQuote_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.FGPackSpec, ProjectTimelineUpdateFields.FGPackSpec_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.SAPBOMSetup, ProjectTimelineUpdateFields.SAPBOMSetup_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.OBMReview3, ProjectTimelineUpdateFields.OBMReview3_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.FCST, ProjectTimelineUpdateFields.FCST_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.ExternalMfg, ProjectTimelineUpdateFields.ExternalMfg_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.SAPRoutingSetup, ProjectTimelineUpdateFields.SAPRoutingSetup_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.BOMActiveDate, ProjectTimelineUpdateFields.BOMActiveDate_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.SAPCostingDetails, ProjectTimelineUpdateFields.SAPCostingDetails_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.SAPWarehouseInfo, ProjectTimelineUpdateFields.SAPWarehouseInfo_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.StandardCostEntry, ProjectTimelineUpdateFields.StandardCostEntry_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.CostFinishedGood, ProjectTimelineUpdateFields.CostFinishedGood_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.FinalCostingReview, ProjectTimelineUpdateFields.FinalCostingReview_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.PurchasedPO, ProjectTimelineUpdateFields.PurchasedPO_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.RemoveSAPBlocks, ProjectTimelineUpdateFields.RemoveSAPBlocks_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.CustomerPO, ProjectTimelineUpdateFields.CustomerPO_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.MaterialsRcvdChk, ProjectTimelineUpdateFields.MaterialsRcvdChk_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.FirstProductionChk, ProjectTimelineUpdateFields.FirstProductionChk_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.DistributionChk, ProjectTimelineUpdateFields.DistributionChk_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.MaterialWarehouseSetUp, ProjectTimelineUpdateFields.MaterialWarehouseSetUp_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.SAPCompleteItemSetup, ProjectTimelineUpdateFields.SAPCompleteItemSetup_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.BEQRC, ProjectTimelineUpdateFields.BEQRC_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.IPF_Planned, ProjectTimelineUpdateFields.IPF_Planned_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.SrOBMApproval_Planned, ProjectTimelineUpdateFields.SrOBMApproval_Planned_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.SrOBMApproval2_Planned, ProjectTimelineUpdateFields.SrOBMApproval2_Planned_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.TradePromo_Planned, ProjectTimelineUpdateFields.TradePromo_Planned_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.EstPricing_Planned, ProjectTimelineUpdateFields.EstPricing_Planned_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.EstBracketPricing_Planned, ProjectTimelineUpdateFields.EstBracketPricing_Planned_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.Distribution_Planned, ProjectTimelineUpdateFields.Distribution_Planned_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.Operations_Planned, ProjectTimelineUpdateFields.Operations_Planned_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.SAPInitialSetup_Planned, ProjectTimelineUpdateFields.SAPInitialSetup_Planned_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.PrelimSAPInitialSetup_Planned, ProjectTimelineUpdateFields.PrelimSAPInitialSetup_Planned_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.QA_Planned, ProjectTimelineUpdateFields.QA_Planned_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.OBMReview1_Planned, ProjectTimelineUpdateFields.OBMReview1_Planned_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.BOMSetupPE_Planned, ProjectTimelineUpdateFields.BOMSetupPE_Planned_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.BOMSetupProc_Planned, ProjectTimelineUpdateFields.BOMSetupProc_Planned_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.BOMSetupPE2_Planned, ProjectTimelineUpdateFields.BOMSetupPE2_Planned_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.OBMReview2_Planned, ProjectTimelineUpdateFields.OBMReview2_Planned_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.BOMSetupPE3_Planned, ProjectTimelineUpdateFields.BOMSetupPE3_Planned_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.GRAPHICS_Planned, ProjectTimelineUpdateFields.GRAPHICS_Planned_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.SAPBOMSetup_Planned, ProjectTimelineUpdateFields.SAPBOMSetup_Planned_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.OBMReview3_Planned, ProjectTimelineUpdateFields.OBMReview3_Planned_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.ExternalMfg_Planned, ProjectTimelineUpdateFields.ExternalMfg_Planned_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.SAPRoutingSetup_Planned, ProjectTimelineUpdateFields.SAPRoutingSetup_Planned_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.MaterialWarehouseSetUp_Planned, ProjectTimelineUpdateFields.MaterialWarehouseSetUp_Planned_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.SAPCompleteItemSetup_Planned, ProjectTimelineUpdateFields.SAPCompleteItemSetup_Planned_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectTimelineUpdateFields.BEQRC_Planned, ProjectTimelineUpdateFields.BEQRC_Planned_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (needsListUpdate)
                    splist.Update();

            }
            catch (Exception ex)
            {

            }
        }
        private void CreateCompassWorkflowStatusList()
        {
            try
            {
                SPList splist = web.Lists.TryGetList(GlobalConstants.LIST_CompassWorkflowStatusListName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, GlobalConstants.LIST_CompassWorkflowStatusListName, GlobalConstants.LIST_CompassWorkflowStatusListName);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, CompassWorkflowStatusListFields.CompassListItemId, CompassWorkflowStatusListFields.CompassListItemId, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.IPF_Completed, CompassWorkflowStatusListFields.IPF_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.SrOBMApproval_Completed, CompassWorkflowStatusListFields.SrOBMApproval_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.SrOBMApproval2_Completed, CompassWorkflowStatusListFields.SrOBMApproval2_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.InitialCosting_Completed, CompassWorkflowStatusListFields.InitialCosting_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.InitialCapacity_Completed, CompassWorkflowStatusListFields.InitialCapacity_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.TradePromo_Completed, CompassWorkflowStatusListFields.TradePromo_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.EstPricing_Completed, CompassWorkflowStatusListFields.EstPricing_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.EstBracketPricing_Completed, CompassWorkflowStatusListFields.EstBracketPricing_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.Distribution_Completed, CompassWorkflowStatusListFields.Distribution_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.Operations_Completed, CompassWorkflowStatusListFields.Operations_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ExternalMfg_Completed, CompassWorkflowStatusListFields.ExternalMfg_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.SAPInitialSetup_Completed, CompassWorkflowStatusListFields.SAPInitialSetup_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.PrelimSAPInitialSetup_Completed, CompassWorkflowStatusListFields.PrelimSAPInitialSetup_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.QA_Completed, CompassWorkflowStatusListFields.QA_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.OBMReview1_Completed, CompassWorkflowStatusListFields.OBMReview1_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.BOMSetupPE_Completed, CompassWorkflowStatusListFields.BOMSetupPE_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.BOMSetupProc_Completed, CompassWorkflowStatusListFields.BOMSetupProc_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                //Additional Procurement Tasks
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ProcAncillary_Completed, CompassWorkflowStatusListFields.ProcAncillary_Completed, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ProcCorrugated_Completed, CompassWorkflowStatusListFields.ProcCorrugated_Completed, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ProcPurchased_Completed, CompassWorkflowStatusListFields.ProcPurchased_Completed, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ProcFilm_Completed, CompassWorkflowStatusListFields.ProcFilm_Completed, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ProcLabel_Completed, CompassWorkflowStatusListFields.ProcLabel_Completed, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ProcMetal_Completed, CompassWorkflowStatusListFields.ProcMetal_Completed, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ProcOther_Completed, CompassWorkflowStatusListFields.ProcOther_Completed, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ProcPaperboard_Completed, CompassWorkflowStatusListFields.ProcPaperboard_Completed, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ProcRigidPlastic_Completed, CompassWorkflowStatusListFields.ProcRigidPlastic_Completed, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ProcSeasonal_Completed, CompassWorkflowStatusListFields.ProcSeasonal_Completed, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ProcCoMan_Completed, CompassWorkflowStatusListFields.ProcCoMan_Completed, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ProcNovelty_Completed, CompassWorkflowStatusListFields.ProcNovelty_Completed, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ProcExternal_Completed, CompassWorkflowStatusListFields.ProcExternal_Completed, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ProcExtAncillary_Completed, CompassWorkflowStatusListFields.ProcExtAncillary_Completed, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ProcExtCorrugated_Completed, CompassWorkflowStatusListFields.ProcExtCorrugated_Completed, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ProcExtPurchased_Completed, CompassWorkflowStatusListFields.ProcExtPurchased_Completed, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ProcExtFilm_Completed, CompassWorkflowStatusListFields.ProcExtFilm_Completed, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ProcExtLabel_Completed, CompassWorkflowStatusListFields.ProcExtLabel_Completed, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ProcExtMetal_Completed, CompassWorkflowStatusListFields.ProcExtMetal_Completed, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ProcExtOther_Completed, CompassWorkflowStatusListFields.ProcExtOther_Completed, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ProcExtPaperboard_Completed, CompassWorkflowStatusListFields.ProcExtPaperboard_Completed, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ProcExtRigidPlastic_Completed, CompassWorkflowStatusListFields.ProcExtRigidPlastic_Completed, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                //End Procurement Tasks
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.BOMSetupPE2_Completed, CompassWorkflowStatusListFields.BOMSetupPE2_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.BOMSetupPE3_Completed, CompassWorkflowStatusListFields.BOMSetupPE3_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.BOMSetupMWH_Completed, CompassWorkflowStatusListFields.BOMSetupMWH_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.SAPCompleteItemSetup_Completed, CompassWorkflowStatusListFields.SAPCompleteItemSetup_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.BEQRC_Completed, CompassWorkflowStatusListFields.BEQRC_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.SAPBOMSetup_Completed, CompassWorkflowStatusListFields.SAPBOMSetup_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.OBMReview2_Completed, CompassWorkflowStatusListFields.OBMReview2_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.Graphics_Completed, CompassWorkflowStatusListFields.Graphics_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.OBMReview3_Completed, CompassWorkflowStatusListFields.OBMReview3_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.FGPackSpec_Completed, CompassWorkflowStatusListFields.FGPackSpec_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.CompCostSeasonal_Completed, CompassWorkflowStatusListFields.CompCostSeasonal_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.CompCostCorrPaper_Completed, CompassWorkflowStatusListFields.CompCostCorrPaper_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.CompCostFLRP_Completed, CompassWorkflowStatusListFields.CompCostFLRP_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.SAPRoutingSetup_Completed, CompassWorkflowStatusListFields.SAPRoutingSetup_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.BOMActiveDate_Completed, CompassWorkflowStatusListFields.BOMActiveDate_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.SAPCostingDetails_Completed, CompassWorkflowStatusListFields.SAPCostingDetails_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.SAPWarehouseInfo_Completed, CompassWorkflowStatusListFields.SAPWarehouseInfo_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.StandardCostEntry_Completed, CompassWorkflowStatusListFields.StandardCostEntry_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.CostFinishedGood_Completed, CompassWorkflowStatusListFields.CostFinishedGood_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.FinalCostingReview_Completed, CompassWorkflowStatusListFields.FinalCostingReview_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.PurchasedPO_Completed, CompassWorkflowStatusListFields.PurchasedPO_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.RemoveSAPBlocks_Completed, CompassWorkflowStatusListFields.RemoveSAPBlocks_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.CustomerPO_Completed, CompassWorkflowStatusListFields.CustomerPO_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.MaterialsReceivedChk_Completed, CompassWorkflowStatusListFields.MaterialsReceivedChk_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.FirstProductionChk_Completed, CompassWorkflowStatusListFields.FirstProductionChk_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.DistributionCenterChk_Completed, CompassWorkflowStatusListFields.DistributionCenterChk_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.IntCompEmailSent_Completed, CompassWorkflowStatusListFields.IntCompEmailSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.RnDEmailSent_Completed, CompassWorkflowStatusListFields.RnDEmailSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ZestPricingEmailSent_Completed, CompassWorkflowStatusListFields.ZestPricingEmailSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.DemandPlanningEmailSent_Completed, CompassWorkflowStatusListFields.DemandPlanningEmail_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ProjectCancelled, CompassWorkflowStatusListFields.ProjectCancelled_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ProjectCompleted, CompassWorkflowStatusListFields.ProjectCompleted_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ProjectOnHold, CompassWorkflowStatusListFields.ProjectOnHold_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                #region Email Notifications Sent Flags
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.PENotificationSent_Completed, CompassWorkflowStatusListFields.PENotificationSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.DemandForecastReminderSent_Completed, CompassWorkflowStatusListFields.DemandForecastReminderSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.NetMoveAutoEmailSent_Completed, CompassWorkflowStatusListFields.NetMoveAutoEmailSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.IPFSubmissionSent_Completed, CompassWorkflowStatusListFields.IPFSubmissionSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.SrOBMApprovalSent_Completed, CompassWorkflowStatusListFields.SrOBMApprovalSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.IPF_ReqForInfoSent_Completed, CompassWorkflowStatusListFields.IPF_ReqForInfoSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.IPF_RejectedSent_Completed, CompassWorkflowStatusListFields.IPF_RejectedSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.PrlmSAPIntlSetupSent_Completed, CompassWorkflowStatusListFields.PrlmSAPIntlSetupSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.PrlmSAPIntlStupCmpltdSent_Cmpltd, CompassWorkflowStatusListFields.PrlmSAPIntlStupCmpltdSent_Cmpltd_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.DemandPlanEmailSent_Completed, CompassWorkflowStatusListFields.DemandPlanEmailSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.TradePromoSent_Completed, CompassWorkflowStatusListFields.TradePromoSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.EstPricingSent_Completed, CompassWorkflowStatusListFields.EstPricingSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.EstBracketPricingSent_Completed, CompassWorkflowStatusListFields.EstBracketPricingSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.DistributionSent_Completed, CompassWorkflowStatusListFields.DistributionSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.InitialCostingSent_Completed, CompassWorkflowStatusListFields.InitialCostingSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.QASent_Completed, CompassWorkflowStatusListFields.QASent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.SAPInitialSetupSent_Completed, CompassWorkflowStatusListFields.SAPInitialSetupSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.SAPIntlSetupConfSent_Completed, CompassWorkflowStatusListFields.SAPIntlSetupConfSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.NewFGWithRplcmntSent_Completed, CompassWorkflowStatusListFields.NewFGWithRplcmntSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.OperationsSent_Completed, CompassWorkflowStatusListFields.OperationsSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.ExternalMfgSent_Completed, CompassWorkflowStatusListFields.ExternalMfgSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.OBMReview1Sent_Completed, CompassWorkflowStatusListFields.OBMReview1Sent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.SrOBMApproval2Sent_Completed, CompassWorkflowStatusListFields.SrOBMApproval2Sent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.FGPackSpecSent_Completed, CompassWorkflowStatusListFields.FGPackSpecSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.BOMSetupPESent_Completed, CompassWorkflowStatusListFields.BOMSetupPESent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.BOMSetupProcSent_Completed, CompassWorkflowStatusListFields.BOMSetupProcSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.BOMSetupPE2Sent_Completed, CompassWorkflowStatusListFields.BOMSetupPE2Sent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.BOMSetupPE3Sent_Completed, CompassWorkflowStatusListFields.BOMSetupPE3Sent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.BOMSetupPE3SubmittedSent_Completed, CompassWorkflowStatusListFields.BOMSetupPE3SubmittedSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.SAPBOMSetupSent_Completed, CompassWorkflowStatusListFields.SAPBOMSetupSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.BOMSetupWarehouseSent_Completed, CompassWorkflowStatusListFields.BOMSetupWarehouseSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.SAPCompleteItemSetupSent_Completed, CompassWorkflowStatusListFields.SAPCompleteItemSetupSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.OBMReview2Sent_Completed, CompassWorkflowStatusListFields.OBMReview2Sent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.GraphicsSent_Completed, CompassWorkflowStatusListFields.GraphicsSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.StandardCostEntrySent_Completed, CompassWorkflowStatusListFields.StandardCostEntrySent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.SAPWareHouseInfoSent_Completed, CompassWorkflowStatusListFields.SAPWareHouseInfoSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.SAPCostingDetailsSent_Completed, CompassWorkflowStatusListFields.SAPCostingDetailsSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.RemoveSAPBlocksSent_Completed, CompassWorkflowStatusListFields.RemoveSAPBlocksSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.CustPOSCanBeEntrdSent_Completed, CompassWorkflowStatusListFields.CustPOSCanBeEntrdSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.PurchasePOSent_Completed, CompassWorkflowStatusListFields.PurchasePOSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.CompCostSeasonalSent_Completed, CompassWorkflowStatusListFields.CompCostSeasonalSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.CompCostCrrgtdSent_Completed, CompassWorkflowStatusListFields.CompCostCrrgtdSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.CompCostFlmLblSent_Completed, CompassWorkflowStatusListFields.CompCostFlmLblSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.SAPRoutSetUpEmailSent_Completed, CompassWorkflowStatusListFields.SAPRoutSetUpEmailSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.SAPBOMSetupConfSent_Completed, CompassWorkflowStatusListFields.SAPBOMSetupConfSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.CancelledSent_Completed, CompassWorkflowStatusListFields.CancelledSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.RejectedSent_Completed, CompassWorkflowStatusListFields.RejectedSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.BEQRCSent_Completed, CompassWorkflowStatusListFields.BEQRCSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.BEQRCRequestSent_Completed, CompassWorkflowStatusListFields.BEQRCRequestSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, CompassWorkflowStatusListFields.NewMatSAPConfSent_Completed, CompassWorkflowStatusListFields.NewMatSAPConfSent_Completed_DisplayName, false, yesNoChoices))
                {
                    needsListUpdate = true;
                }
                #endregion

                if (needsListUpdate)
                    splist.Update();
            }
            catch (Exception ex)
            {

            }
        }
        private void CreateComponentCostingList()
        {
            try
            {
                SPList splist = web.Lists.TryGetList(GlobalConstants.LIST_ComponentCostingListName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, GlobalConstants.LIST_ComponentCostingListName, GlobalConstants.LIST_ComponentCostingListName);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields - We are adding all dates as strings since there is a limit of 48 dates per list. We will break this limit with the amount of dates
                // being gathered.
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, ComponentCostingListFields.CompassListItemId, ComponentCostingListFields.CompassListItemId_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ComponentCostingListFields.PackagingItemId, ComponentCostingListFields.PackagingItemId_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ComponentCostingListFields.ValidityStartDate, ComponentCostingListFields.ValidityStartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ComponentCostingListFields.ValidityEndDate, ComponentCostingListFields.ValidityEndDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ComponentCostingListFields.SupplierAgreementNumber, ComponentCostingListFields.SupplierAgreementNumber_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ComponentCostingListFields.Subcontracted, ComponentCostingListFields.Subcontracted_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ComponentCostingListFields.ProcurementManager, ComponentCostingListFields.ProcurementManager_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ComponentCostingListFields.BracketPricing, ComponentCostingListFields.BracketPricing_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ComponentCostingListFields.PIRCostPerUOM, ComponentCostingListFields.PIRCostPerUOM_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ComponentCostingListFields.PerUnit, ComponentCostingListFields.PerUnit_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ComponentCostingListFields.DeliveredOrOriginCost, ComponentCostingListFields.DeliveredOrOriginCost_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ComponentCostingListFields.FreightAmount, ComponentCostingListFields.FreightAmount_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ComponentCostingListFields.TransferOfOwnership, ComponentCostingListFields.TransferOfOwnership_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ComponentCostingListFields.PlannedDeliveryTime, ComponentCostingListFields.PlannedDeliveryTime_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ComponentCostingListFields.MinimumOrderQTY, ComponentCostingListFields.MinimumOrderQTY_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ComponentCostingListFields.StandardQuantity, ComponentCostingListFields.StandardQuantity_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ComponentCostingListFields.TolOverDelivery, ComponentCostingListFields.TolOverDelivery_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ComponentCostingListFields.TolUnderDelivery, ComponentCostingListFields.TolUnderDelivery_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ComponentCostingListFields.PurchasingGroup, ComponentCostingListFields.PurchasingGroup_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ComponentCostingListFields.ConversionFactors, ComponentCostingListFields.ConversionFactors_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ComponentCostingListFields.AnnualVolumeEA, ComponentCostingListFields.AnnualVolumeEA_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ComponentCostingListFields.NinetyDayVolume, ComponentCostingListFields.NinetyDayVolume_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ComponentCostingListFields.AnnualVolumeCaseUOM, ComponentCostingListFields.AnnualVolumeCaseUOM_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ComponentCostingListFields.PriceDetermination, ComponentCostingListFields.PriceDetermination_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void CreateAlternateUOMsList()
        {
            try
            {
                SPList splist = web.Lists.TryGetList(GlobalConstants.LIST_AlternateUOMListName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, GlobalConstants.LIST_AlternateUOMListName, GlobalConstants.LIST_AlternateUOMListName);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields - We are adding all dates as strings since there is a limit of 48 dates per list. We will break this limit with the amount of dates
                // being gathered.
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, AlternateUOMFields.PackagingItemId, AlternateUOMFields.PackagingItemId_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, AlternateUOMFields.AlternateUOM, AlternateUOMFields.AlternateUOM_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, AlternateUOMFields.XValue, AlternateUOMFields.XValue_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, AlternateUOMFields.YValue, AlternateUOMFields.YValue_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void CreateWorldSyncRequestList()
        {
            try
            {
                SPList splist = web.Lists.TryGetList(GlobalConstants.LIST_WorldSyncRequestList);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, GlobalConstants.LIST_WorldSyncRequestList, GlobalConstants.LIST_WorldSyncRequestList);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields - We are adding all dates as strings since there is a limit of 48 dates per list. We will break this limit with the amount of dates
                // being gathered.
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, WorldSyncRequestFields.SAPnumber, WorldSyncRequestFields.SAPnumber_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, WorldSyncRequestFields.SAPdescription, WorldSyncRequestFields.SAPdescription_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, WorldSyncRequestFields.RequestType, WorldSyncRequestFields.RequestType_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, WorldSyncRequestFields.RequestStatus, WorldSyncRequestFields.RequestStatus_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, WorldSyncRequestFields.WorkflowStep, WorldSyncRequestFields.WorkflowStep_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (needsListUpdate)
                    splist.Update();
            }
            catch (Exception ex)
            {

            }
        }
        private void CreateMarketingClaimsList()
        {
            try
            {
                SPList splist = web.Lists.TryGetList(GlobalConstants.LIST_MarketingClaimsListName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, GlobalConstants.LIST_MarketingClaimsListName, GlobalConstants.LIST_MarketingClaimsListName);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.CompassListItemId, MarketingClaimsListFields.CompassListItemId_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.SellableUnit, MarketingClaimsListFields.SellableUnit_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.NewNLEAFormat, MarketingClaimsListFields.NewNLEAFormat_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.MadeInUSAClaim, MarketingClaimsListFields.MadeInUSAClaim_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.MadeInUSAClaimDets, MarketingClaimsListFields.MadeInUSAClaimDets_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.Organic, MarketingClaimsListFields.Organic_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.GMOClaim, MarketingClaimsListFields.GMOClaim_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.GlutenFree, MarketingClaimsListFields.GlutenFree_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.FatFree, MarketingClaimsListFields.FatFree_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.Kosher, MarketingClaimsListFields.Kosher_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.NaturalColors, MarketingClaimsListFields.NaturalColors_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.NaturalFlavors, MarketingClaimsListFields.NaturalFlavors_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.PreservativeFree, MarketingClaimsListFields.PreservativeFree_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.LactoseFree, MarketingClaimsListFields.LactoseFree_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.JuiceConcentrate, MarketingClaimsListFields.JuiceConcentrate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.LowSodium, MarketingClaimsListFields.LowSodium_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.GoodSource, MarketingClaimsListFields.GoodSource_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.VitaminAPct, MarketingClaimsListFields.VitaminAPct_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.VitaminB1Pct, MarketingClaimsListFields.VitaminB1Pct_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.VitaminB2Pct, MarketingClaimsListFields.VitaminB2Pct_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.VitaminB3Pct, MarketingClaimsListFields.VitaminB3Pct_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.VitaminB5Pct, MarketingClaimsListFields.VitaminB5Pct_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.VitaminB6Pct, MarketingClaimsListFields.VitaminB6Pct_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.VitaminB12Pct, MarketingClaimsListFields.VitaminB12Pct_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.VitaminCPct, MarketingClaimsListFields.VitaminCPct_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.VitaminDPct, MarketingClaimsListFields.VitaminDPct_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.VitaminEPct, MarketingClaimsListFields.VitaminEPct_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.PotassiumPct, MarketingClaimsListFields.PotassiumPct_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.IronPct, MarketingClaimsListFields.IronPct_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.CalciumPct, MarketingClaimsListFields.CalciumPct_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.AllergenMilk, MarketingClaimsListFields.AllergenMilk_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.AllergenEggs, MarketingClaimsListFields.AllergenEggs_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.AllergenPeanuts, MarketingClaimsListFields.AllergenPeanuts_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.AllergenCoconut, MarketingClaimsListFields.AllergenCoconut_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.AllergenAlmonds, MarketingClaimsListFields.AllergenAlmonds_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.AllergenSoy, MarketingClaimsListFields.AllergenSoy_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.AllergenWheat, MarketingClaimsListFields.AllergenWheat_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.AllergenHazelNuts, MarketingClaimsListFields.AllergenHazelNuts_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.AllergenOther, MarketingClaimsListFields.AllergenOther_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.ClaimsDesired, MarketingClaimsListFields.ClaimsDesired_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.MaterialClaimsCompNumber, MarketingClaimsListFields.MaterialClaimsCompNumber_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.MaterialClaimsCompDesc, MarketingClaimsListFields.MaterialClaimsCompDesc_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.BioEngLabelingAcceptable, MarketingClaimsListFields.BioEngLabelingAcceptable_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, MarketingClaimsListFields.ClaimBioEngineering, MarketingClaimsListFields.ClaimBioEngineering_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (needsListUpdate)
                    splist.Update();

            }
            catch (Exception ex)
            {

            }
        }
        private void CreateCompassTemplatesDocumentLibrary()
        {
            string description = "Compass Templates Document Library";

            try
            {
                SPList splist = web.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassTemplatesName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateDocumentLibrary(web, GlobalConstants.DOCLIBRARY_CompassTemplatesName, description);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

            }
            catch (Exception ex)
            {

            }
        }

        private void CreateDragonflyList(string listName, string description)
        {
            try
            {
                SPList splist = web.Lists.TryGetList(listName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, listName, description);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields
                var needsListUpdate = false;


                if (SetupUtilities.CreateField(splist, DragonflyListFields.CompassProjectNumber, DragonflyListFields.CompassProjectNumber_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyListFields.ItemNumber, DragonflyListFields.ItemNumber_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyListFields.MaterialNumber, DragonflyListFields.MaterialNumber_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyListFields.MaterialStatus, DragonflyListFields.MaterialStatus_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyListFields.TaskName, DragonflyListFields.TaskName_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyListFields.TaskVersionNumber, DragonflyListFields.TaskVersionNumber_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyListFields.ProjUploadedtoDF_ActStart, DragonflyListFields.ProjUploadedtoDF_ActStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyListFields.ProjUploadedtoDF_ActEnd, DragonflyListFields.ProjUploadedtoDF_ActEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyListFields.SGSOnsiteUploadsArt_ActStart, DragonflyListFields.SGSOnsiteUploadsArt_ActStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyListFields.SGSOnsiteUploadsArt_ActEnd, DragonflyListFields.SGSOnsiteUploadsArt_ActEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyListFields.ArtworkApproved_ActStart, DragonflyListFields.ArtworkApproved_ActStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyListFields.ArtworkApproved_ActEnd, DragonflyListFields.ArtworkApproved_ActEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyListFields.ProofCreatedUploaded_ActStart, DragonflyListFields.ProofCreatedUploaded_ActStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyListFields.ProofCreatedUploaded_ActEnd, DragonflyListFields.ProofCreatedUploaded_ActEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyListFields.ProofApproved_ActStart, DragonflyListFields.ProofApproved_ActStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyListFields.ProofApproved_ActEnd, DragonflyListFields.ProofApproved_ActEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyListFields.MakeAndShipPlates_ActStart, DragonflyListFields.MakeAndShipPlates_ActStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyListFields.MakeAndShipPlates_ActEnd, DragonflyListFields.MakeAndShipPlates_ActEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (needsListUpdate)
                    splist.Update();
            }
            catch (Exception ex)
            {

            }
        }

        private void CreateDragonflyErrorList(string listName, string description)
        {
            try
            {
                SPList splist = web.Lists.TryGetList(listName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, listName, description);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, DragonflyErrorListFields.CompassProjectNumber, DragonflyErrorListFields.CompassProjectNumber_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyErrorListFields.ItemNumber, DragonflyErrorListFields.ItemNumber_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyErrorListFields.MaterialNumber, DragonflyErrorListFields.MaterialNumber_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyErrorListFields.MaterialStatus, DragonflyErrorListFields.MaterialStatus_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyErrorListFields.TaskName, DragonflyErrorListFields.TaskName_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyErrorListFields.TaskVersionNumber, DragonflyErrorListFields.TaskVersionNumber_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyErrorListFields.TaskStatus, DragonflyErrorListFields.TaskStatus_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyErrorListFields.ActualStartDate, DragonflyErrorListFields.ActualStartDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyErrorListFields.ActualEndDate, DragonflyErrorListFields.ActualEndDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyErrorListFields.Error, DragonflyErrorListFields.Error_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (needsListUpdate)
                    splist.Update();
            }
            catch (Exception ex)
            {

            }
        }

        #region List View Helper Methods
        private void AddSLTExceptionFields(System.Collections.Specialized.StringCollection viewFields)
        {
            viewFields.Add(CompassListFields.ProjectStatus);
            viewFields.Add(CompassListFields.MaterialGroup1Brand);
            viewFields.Add(CompassListFields.ProjectNumber);
            //viewFields.Add(CompassListFields.OBM_SAPStatus);
            viewFields.Add(CompassListFields.FirstShipDate);
            viewFields.Add(CompassListFields.RevisedFirstShipDate);
            //viewFields.Add(CompassListFields.OBM_EstimatedProductionDate);
            //viewFields.Add(CompassListFields.OBM_PlannedProductionDate);
        }
        private void AddAgendaFields(System.Collections.Specialized.StringCollection viewFields)
        {
            viewFields.Add(CompassListFields.ProductHierarchyLevel1);
            viewFields.Add(CompassListFields.ProductHierarchyLevel2);
            viewFields.Add(CompassListFields.MaterialGroup1Brand);
            viewFields.Add(CompassListFields.SAPItemNumber);
            viewFields.Add(CompassListFields.SAPDescription);
            viewFields.Add(CompassListFields.Customer);
            viewFields.Add(CompassListFields.FirstShipDate);
            viewFields.Add(CompassListFields.ManufacturingLocation);
            viewFields.Add(CompassListFields.PackingLocation);
            //viewFields.Add(CompassListFields.OBM_NLEAPosted);
            //viewFields.Add(CompassListFields.OBM_ProofPosted);
            //viewFields.Add(CompassListFields.OBM_GraphicsFileSent);
            //viewFields.Add(CompassListFields.OBM_ProofApproved);
            //viewFields.Add(CompassListFields.OBM_PlatesToPrinter);
            //viewFields.Add(CompassListFields.OBM_SAPStatus);
            //viewFields.Add(CompassListFields.OBM_PackagingArrivalDate);
            //viewFields.Add(CompassListFields.OBM_EstimatedProductionDate);
            //viewFields.Add(CompassListFields.OBM_PlannedProductionDate);
            //viewFields.Add(CompassListFields.OBM_CustomGrouping);
            viewFields.Add(CompassListFields.ItemConcept);
        }
        #endregion

        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
