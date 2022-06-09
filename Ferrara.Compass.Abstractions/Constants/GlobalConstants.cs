using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferrara.Compass.Abstractions.Constants
{
    public class GlobalConstants
    {
        public const int CONST_LogCount = 1000;
        public const string CONST_NeedsNew = "NEEDS NEW";
        public const string CONST_Yes = "Yes";
        public const string CONST_No = "No";
        public const string CONST_NotApplicable = "Not Applicable";
        public const string OPTION_Separator = "|";

        #region LIST Constants
        public const string LIST_CompassListName = "Compass List";
        public const string LIST_CompassList2Name = "Compass List 2";
        public const string LIST_CompassTeamListName = "Compass Team List";
        public const string LIST_StageGateProjectListName = "Stage Gate Project List";
        public const string LIST_StageGateConsolidatedFinancialSummaryListName = "Stage Gate Consolidated Financial Summary List";
        public const string LIST_StageGateFinancialAnalysisListName = "Stage Gate Financial Analysis List";
        public const string LIST_PMTNecessaryDeliverablesListName = "Stage Gate Deliverables List";
        public const string LIST_PMTRAListName = "StageGate Gate List";
        public const string LIST_SGSGateBriefList = "StageGate Gate Brief List";
        public const string LIST_SGSChildProjectTempList = "SGS Child Project Temp List";
        public const string LIST_PackagingItemListName = "Compass Packaging Item List";
        public const string LIST_PLMSpecificationsListName = "PLM Specifications List";
        public const string LIST_ApprovalListName = "Compass Approval List";
        public const string LIST_ApprovalList2Name = "Compass Approval List 2";
        public const string LIST_SAPApprovalListName = "Compass SAP Approval List";
        public const string LIST_ProjectDecisionsListName = "Compass Project Decisions List";
        public const string LIST_CompassTaskAssignmentListName = "Compass Task Assignment";

        public const string LIST_LogsListName = "Logs List";
        public const string LIST_FormAccessListName = "Compass Form Access";
        public const string LIST_CompassPackMeasurementsListName = "Compass Pack Measurements List";
        public const string LIST_CompassShipperFinishedGoodListName = "Compass Shipper Finished Good List";
        public const string LIST_CompassMixesListName = "Compass Mixes List";
        public const string LIST_CompassSAPStatusListName = "Compass SAP Status List";
        public const string LIST_CompassWorkflowStatusListName = "Compass Workflow Status List";
        public const string LIST_CompassProjectStatusList = "Compass Project Status List";

        public const string LIST_EmailLoggingListName = "Compass Email Logging List";
        public const string LIST_LatestNewsListName = "Compass Latest News List";
        public const string LIST_GraphicsLogsListName = "Compass Graphics Logs List";
        public const string LIST_OBMBrandManagerLookupListName = "Compass OBM BrandMgr Lookup";

        public const string LIST_NoSelectionValue = "-1";
        public const string LIST_NoSelectionText = "Select...";

        //public const string LIST_WorkflowTaskListName = "WorkflowTasks";
        public const string LIST_WorkflowTaskListName = "Workflow Tasks";
        public const string LIST_WorkflowTaskListName1 = "Compass Workflow Tasks 1";
        public const string LIST_WorkflowTaskListName2 = "Compass Workflow Tasks 2";
        public const string LIST_WorkflowTaskListName3 = "Compass Workflow Tasks 3";
        public const string LIST_WorldSyncRequestTasks = "WorldSync Request Tasks";
        public const string LIST_ProjectTimelineTypeListName = "Project Timeline Type Days List";
        public const string LIST_ProjectTimelineUpdateName = "Update Project Timeline";
        public const string LIST_ProjectTimelineDetailsList = "Compass Timeline Details List";
        public const string LIST_HolidayLookup = "Holiday Lookup";
        public const string LIST_TimelineTypesLookup = "Timeline Types Lookup";

        // SAP Lookup Lists
        public const string LIST_SAPMaterialMasterListName = "SAP Material Master List";
        public const string LIST_SAPBOMListName = "SAP BOM List";
        public const string LIST_SAPHanaStatusListName = "SAP Status List";

        // Dragonfly Lookup Lists        
        public const string LIST_DragonflyStatusListName = "Dragonfly Status List";
        public const string LIST_ComponentCostingListName = "Component Costing List";
        public const string LIST_CompassDragonflyListName = "Compass Dragonfly List";
        public const string LIST_CompassDragonflyErrorListName = "Compass Dragonfly Error List";

        // Lookup Lists
        public const string LIST_AllergensLookup = "Allergens Lookup";
        public const string LIST_CaseTypesLookup = "Case Types Lookup";

        public const string LIST_CountryOfOriginLookup = "Country Of Origin Lookup";
        public const string LIST_CustomersLookup = "Customers Lookup";
        public const string LIST_DistributionLookup = "Distribution Lookup";
        public const string LIST_DistributionDeploymentModesLookup = "Deployment Modes Lookup";

        public const string LIST_KosherTypesLookup = "Kosher Types Lookup";
        public const string LIST_ManufacturingLocationsLookup = "Manufacturing Locations Lookup";
        public const string LIST_MaterialGroup1Lookup = "Material Group1 Lookup";

        public const string LIST_PackTrialResultLookUp = "Pack Trial Result Lookup";

        public const string LIST_MaterialGroup2Lookup = "Material Group2 Lookup";
        public const string LIST_MaterialGroup4Lookup = "Material Group4 Lookup";
        public const string LIST_MaterialGroup5Lookup = "Material Group5 Lookup";

        public const string LIST_PackagingComponentTypesLookup = "Packaging Component Types Lookup";
        public const string LIST_PackingLocationsLookup = "Packing Locations Lookup";
        public const string LIST_PlantLinesLookup = "Plant Lines Lookup";

        public const string LIST_ProcurementTypesLookup = "Procurement Types Lookup";
        public const string LIST_ProductHierarchyLevel1Lookup = "Product Hierarchy Level 1 Lookup";
        public const string LIST_ProductHierarchyLevel2Lookup = "Product Hierarchy Level 2 Lookup";
        public const string LIST_ProjectTypesLookup = "Project Types Lookup";
        public const string LIST_StageGateProjectTypesLookup = "Stage Gate Project Types Lookup";
        public const string LIST_StageLookup = "Stage Lookup";
        public const string LIST_StageGateDesignDeliverables = "Stage Gate Design Deliv Lookup";
        public const string LIST_StageGateDevelopDeliverables = "Stage Gate Develop Deliv Lookup";
        public const string LIST_StageGateIndustrializDeliverables = "Stage Gate Industrialize Deliv Lookup";
        public const string LIST_StageGateLaunchDeliverables = "Stage Gate Launch Deliv Lookup";
        public const string LIST_StageGatePostLaunchDeliverables = "Stage Gate Post Launch Deliv Lookup";
        public const string LIST_StageGateValidateDeliverables = "Stage Gate Validate Deliv Lookup";
        public const string LIST_StageGateStageStatus = "Stage Gate Stage Status Lookup";
        public const string LIST_StageGateSGMeetingLookup = "Stage Gate SG Meeting Lookup";
        public const string LIST_GateDetailColorsLookup = "Stage Gate Color Lookup";
        public const string LIST_OverallRiskLookup = "Overall Risk Lookup";
        public const string LIST_OverallStatusLookup = "Overall Status Lookup";

        public const string LIST_RetailPackTypesLookup = "Retail Pack Types Lookup";
        public const string LIST_ExternalGraphicsVendorLookup = "External Graphics Vendor Lookup";

        public const string LIST_PrintStyleLookup = "Print Style Lookup";
        public const string LIST_FilmPrintStyleLookup = "Film Print Style Lookup";
        public const string LIST_CorrugatedPrintStyleLookup = "Corrugated Print Style Lookup";
        public const string LIST_FilmStyleLookup = "Film Style Lookup";

        public const string LIST_FilmUnWindLookup = "Film UnWind Lookup";

        public const string LIST_EmailTemplates = "Email Templates";
        public const string LIST_Configurations = "Configurations";

        public const string LIST_CriticalInitiativesLookup = "Critical Initiatives Lookup";
        public const string LIST_PrinterSupplierLookup = "Printer Supplier Lookup";
        public const string LIST_ProjectCategoryLookup = "Project Category Lookup";
        public const string LIST_TargetFCCMarginLookup = "Target FCC Margin Lookup";
        public const string LIST_SAPBaseUOMLookup = "SAP Base UOM Lookup";
        public const string LIST_PackUnitLookup = "Pack Unit Lookup";
        public const string LIST_FlowThroughTypeLookup = "Flow Through Type Lookup";
        public const string LIST_MRPCLookup = "MRPC Lookup";
        public const string LIST_ChannelLookup = "Channel Lookup";
        public const string LIST_SubstrateLookup = "Substrate Lookup";
        public const string LIST_MakePackTransfersLookup = "MakePackTransfers Lookup";
        public const string LIST_SubstrateColorLookup = "Substrate Color Lookup";
        public const string LIST_PurchasedIntoCenterLookup = "Purchased Into Center Lookup";
        public const string LIST_FilmSubstrate = "Film Substrate Lookup";

        public const string LIST_OrderUnitofMeasureLookup = "Order Unit of Measure Lookup";
        public const string LIST_IncotermsLookup = "Incoterms Lookup";
        public const string LIST_PRDateCategoryLookup = "PR Date Category Lookup";

        public const string LIST_CoManufacturers = "Co-Manufacturers Lookup";
        public const string LIST_CoManufacturingClassifications = "Co-Mfg Classifications Lookup";
        public const string LIST_CoPackers = "Co-Packers Lookup";
        public const string LIST_ManufacturerCountryOfOrigin = "Mfg Country Of Origin Lookup";
        public const string LIST_SupplierLeadTime = "Supplier Lead Time Lookup";
        public const string LIST_SecondaryInitialReviewDecisionsLookup = "2nd Init Review Decision Lookup";
        public const string LIST_BackSeamsLookup = "Back Seams Lookup";
        public const string LIST_CostingUnitLookup = "Costing Unit Lookup";
        public const string LIST_ProjectTypesSubCategoryLookup = "Project Types SubCategory Lookup";
        public const string LIST_CompPurchasedIntoLocationsLookup = "Comp Purchased Into Locations Lookup";
        public const string LIST_BusinessFunctionsLookup = "Business Functions Lookup";
        public const string LIST_TaskCheckboxesLookup = "Task Checkboxes Lookup";
        public const string LIST_BioEngineeringLabelingcceptableLookup = "BioEngineering Labeling Acceptable Lookup";
        public const string LIST_BioEngineeringLabelingRequiredLookup = "BioEngineering Labeling required Lookup";

        //World Sync Nutritionals
        public const string LIST_WorldSyncNutritionals = "World Sync Nutritionals";
        public const string LIST_WorldSyncNutritionalsDetail = "World Sync Nutritionals Detail";
        public const string LIST_NutrientTypeLookup = "Nutrient Type Lookup";
        public const string LIST_NutrientQuantityContainedTypeLookup = "Nutrient Quantity Contained Type Lookup";
        public const string LIST_NutrientQuantityUOMlookup = "Nutrient Quantity UOM Lookup";
        public const string LIST_DailyValueIntakePctMeasPrecCodeLookup = "DV Intake % Measmnt Code Lookup";
        public const string LIST_PreparationStateLookup = "Preparation State Lookup";
        public const string LIST_ServingSizeLookup = "Serving Size Lookup";

        //World Sync Global
        public const string LIST_CompassWorldSyncList = "Compass World Sync List";
        public const string LIST_ProductTypeLookup = "Product Type Lookup";
        public const string LIST_AlternateClassificationSchemeLookup = "Alternate Classification Scheme Lookup";
        public const string LIST_GS1tradeKeyCodeLookup = "GS1 Trade Key Code Lookup";
        public const string LIST_DataCarrierTypeCodeLookup = "Data Carrier Type Code Lookup";
        public const string LIST_TradeChannelLookup = "Trade Channel Lookup";
        public const string LIST_BrandOwnerGLNlookup = "Brand Owner GLN Lookup";
        public const string LIST_WorldSyncRequestList = "World Sync Request List";

        public const string LIST_AlternateUOMListName = "Alternate UOM List";
        //Marketing Claims List and Lookups
        public const string LIST_MarketingClaimsListName = "Marketing Claims List";
        public const string LIST_PercentagesLookup = "Percentages Lookup";
        public const string LIST_GoodSourceLookup = "Good Source Lookup";
        public const string LIST_NaturalFlavorsLookup = "Natural Flavors Lookup";
        public const string LIST_NaturalColorsLookup = "Natural Colors Lookup";
        public const string LIST_GlutenFreeLookup = "Gluten Free Lookup";
        public const string LIST_GMOClaimLookup = "GMO Claim Lookup";
        public const string LIST_MadeInUSAClaimLookup = "Made In USA Claim Lookup";

        #endregion

        #region DOCUMENT LIBRARY Constants
        public const string DOCLIBRARY_StageGateLibraryName = "Stage Gate Documents";
        public const string DOCLIBRARY_CompassLibraryName = "Compass Documents";
        public const string DOCLIBRARY_InnovationLibraryName = "Innovation Documents";
        public const string DOCLIBRARY_CompassUploadsLibraryName = "Compass Upload Documents";
        public const string DOCLIBRARY_WorldSyncRequestName = "World Sync Request Documents";
        public const string DOCLIBRARY_CompassTemplatesName = "Compass Templates";
        public const string DOCLIBRARY_WorldSyncFUSELibraryName = "World Sync FUSE Documents";
        public const string DOCLIBRARY_CompassDragonflyLibraryName = "Compass Dragonfly Documents";
        #endregion

        #region QUERYSTRING Constants
        public const string QUERYSTRING_CompId = "CompId";
        public const string QUERYSTRING_ProjectNo = "ProjectNo";
        public const string QUERYSTRING_ComponentTab = "ComponentTab";
        public const string QUERYSTRING_CopyMode = "CopyMode";
        public const string QUERYSTRING_Save = "Status";
        public const string QUERYSTRING_SAPNo = "SAPNo";
        public const string QUERYSTRING_RequestId = "RequestId";
        public const string QUERYSTRING_IPFMode = "IPFMode";
        public const string QUERYSTRING_ProjectRejected = "ProjectRejected";
        public const string QUERYSTRING_CompassItemId = "CompassItemId";
        public const string QUERYSTRING_PackagingItemId = "PackagingItemId";
        public const string QUERYSTRING_WorkflowStep = "WFS";
        public const string QUERYSTRING_UserType = "UserType";
        public const string QUERYSTRING_InnovationItemId = "InnovationItemId";
        public const string QUERYSTRING_PackType = "PackType";
        public const string QUERYSTRING_DoctType = "DocType";
        public const string QUERYSTRING_SAPTask = "SAPTask";
        public const string QUERYSTRING_PMTListItemId = "PMTListItemId";
        public const string QUERYSTRING_Gate = "Gate";
        public const string QUERYSTRING_FinancialBrief = "FinancialBrief";
        public const string QUERYSTRING_BriefType = "BriefType";
        public const string QUERYSTRING_BriefNo = "BriefNo";
        public const string QUERYSTRING_FinancialBriefName = "FinancialBriefName";
        public const string QUERYSTRING_ImpersonationId = "ImpersonationId";
        //SGS Generate IPF
        public const string QUERYSTRING_ParentId = "ParentId";
        public const string QUERYSTRING_SGSFinishedGood = "SGSFinishedGood";
        public const string QUERYSTRING_SGSLikeNumber = "SGSLikeNumber";
        public const string QUERYSTRING_SGSDescription = "SGSDescription";
        public const string QUERYSTRING_SGSUCC = "SGSUCC";
        public const string QUERYSTRING_SGSUPC = "SGSUPC";
        public const string QUERYSTRING_SGSProductHierarchy1 = "SGSProductHierarchy1";
        public const string QUERYSTRING_SGSProductHierarchy2 = "SGSProductHierarchy2";
        public const string QUERYSTRING_SGSBrandMaterialGroup1 = "SGSBrand";
        public const string QUERYSTRING_SGSProductFormMaterialGroup4 = "SGSProductForm";
        public const string QUERYSTRING_SGSPackTypeMaterialGroup5 = "SGSPackType";
        public const string QUERYSTRING_SGSParentProjectNo = "SGSParentProjectNo";
        public const string QUERYSTRING_SGSSaveData = "SGSSaveData";
        public const string QUERYSTRING_IPFProjectStatus = "IPFProjectStatus";
        public const string QUERYSTRING_Action = "Action";

        public const string QUERYSTRINGVALUE_IPFCopy = "COPY";
        public const string QUERYSTRINGVALUE_IPFChange = "CHANGE";
        public const string QUERYSTRINGVALUE_PackTypeSemi = "Semi";

        // User Assignment Constants
        public const string QUERYSTRINGVALUE_PackagingEngineer = "PE";
        public const string QUERYSTRINGVALUE_Graphics = "GR";
        public const string QUERYSTRINGVALUE_PM = "PM";
        public const string QUERYSTRINGVALUE_BrandManager = "BM";
        public const string QUERYSTRINGVALUE_TestProject = "TestProject";

        //SAP Tasks
        public const string QUERYSTRINGVALUE_FinalRoutings = "FR";
        public const string QUERYSTRINGVALUE_CostingDetails = "CD";
        public const string QUERYSTRINGVALUE_WarehousInfo = "WI";
        public const string QUERYSTRINGVALUE_StandardCost = "SC";

        #endregion

        #region PAGE Constants
        public const string PAGE_StageGateCreateProject = "StageGateCreateProject.aspx";
        public const string PAGE_StageGateProjectPanel = "StageGateProjectPanel.aspx";
        public const string PAGE_StageGateDesignDeliverables = "StageGateDesignDeliverables.aspx";
        public const string PAGE_StageGateDevelopDeliverables = "StageGateDevelopDeliverables.aspx";
        public const string PAGE_StageGateValidateDeliverables = "StageGateValidateDeliverables.aspx";
        public const string PAGE_StageGateIndustrializeDeliverables = "StageGateIndustrializeDeliverables.aspx";
        public const string PAGE_StageGateLaunchDeliverables = "StageGateLaunchDeliverables.aspx";
        public const string PAGE_StageGatePostLaunchDeliverables = "StageGatePostLaunchDeliverables.aspx";
        public const string PAGE_StageGateGenerateIPFs = "StageGateGenerateIPFs.aspx";
        public const string PAGE_StageGateGenerateBriefPDF = "StageGateGenerateBriefPDF.aspx";
        public const string PAGE_StageGateFinancialSummary = "StageGateFinancialSummary.aspx";
        public const string PAGE_StageGateFinancialBrief = "StageGateFinancialBrief.aspx";
        public const string PAGE_ItemProposal = "ItemProposal.aspx";
        public const string PAGE_ItemProposal2 = "ItemProposal2.aspx";
        public const string PAGE_ItemProposal3 = "ItemProposal3.aspx";

        public const string PAGE_InitialCostingReview = "InitialCostingReview.aspx";
        public const string PAGE_InitialCapacityReview = "InitialCapacityReview.aspx";
        public const string PAGE_InitialApprovalReview = "InitialApprovalReview.aspx";
        public const string PAGE_TradePromoGroup = "TradePromoGroup.aspx";
        public const string PAGE_EstPricing = "EstPricing.aspx";
        public const string PAGE_EstBracketPricing = "EstBracketPricing.aspx";
        public const string PAGE_Distribution = "Distribution.aspx";
        public const string PAGE_OPS = "OPS.aspx";
        public const string PAGE_ExternalManufacturing = "ExternalMfg.aspx";
        public const string PAGE_NewTransferSemi = "NewTransferSemi.aspx";
        public const string PAGE_SAPInitialItemSetup = "SAPInitialItemSetup.aspx";
        public const string PAGE_PrelimSAPInitialItemSetup = "PrelimSAPInitialItemSetup.aspx";

        public const string PAGE_QA = "QA.aspx";

        public const string PAGE_OBMFirstReview = "OBMFirstReview.aspx";
        public const string PAGE_PMFirstReview = "PMFirstReview.aspx";
        public const string PAGE_BillofMaterialSetUpPE = "BOMSetupPE.aspx";
        public const string PAGE_BillofMaterialSetUpProc = "BOMSetupProc.aspx";
        public const string PAGE_BillofMaterialSetUpPE2 = "BOMSetupPE2.aspx";
        public const string PAGE_SAPBOMSetup = "SAPBOMSetup.aspx";
        public const string PAGE_BOMSetupSAP = "BOMSetupSAP.aspx";
        public const string PAGE_SAPCompleteItemSetup = "SAPCompleteItemSetup.aspx";
        public const string PAGE_BOMSetupMaterialWarehouse = "BOMSetupMaterialWarehouse.aspx";
        public const string PAGE_SecondaryApprovalReview = "SecondaryApprovalReview.aspx";

        public const string PAGE_OBMSecondReview = "OBMSecondReview.aspx";
        public const string PAGE_PMSecondReview = "PMSecondReview.aspx";
        public const string PAGE_PMSecondReview2 = "PMSecondReview2.aspx";
        public const string PAGE_FinishedGoodPackSpec = "FinishedGoodPackSpec.aspx";
        public const string PAGE_GraphicsRequest = "GraphicsRequest.aspx";
        public const string PAGE_GraphicsRequest_New = "GraphicsRequest_New.aspx";
        public const string PAGE_GraphicsRequestDetail = "GraphicsRequestDetail.aspx";
        public const string PAGE_GraphicsRequestDetail_New = "GraphicsRequestDetail_New.aspx";
        public const string PAGE_ComponentCosting = "ComponentCosting.aspx";
        public const string PAGE_ComponentCostingSummary = "ComponentCostingSummary.aspx";
        public const string PAGE_FinalRoutingsSummary = "FinalRoutingsSummary.aspx";
        public const string PAGE_NAME_FinalRoutingsSummary = "Final Routings Summary";

        public const string PAGE_CommercializationItemSummary = "CommercializationItem.aspx";
        public const string PAGE_ProjectStatus = "ProjectStatus.aspx";
        public const string PAGE_ProjectTimelineUpdate = "ProjectTimelineUpdate.aspx";
        public const string PAGE_PMTAdministration = "PMTAdmin.aspx";

        public const string PAGE_MaterialsReceivedCheck = "MaterialsReceivedCheck.aspx";
        public const string PAGE_FirstProductionCheck = "FirstProductionCheck.aspx";
        public const string PAGE_DistributionCenterCheck = "DistributionCenterCheck.aspx";

        public const string PAGE_Nutritionals = "Nutritionals.aspx";
        public const string PAGE_WorldSyncGlobal = "WorldSyncGlobal.aspx";
        public const string PAGE_WorldSyncRequestDashboard = "WorldSyncRequestDashboard.aspx";
        public const string PAGE_WorldSyncRequestUpload = "WorldSyncRequestUpload.aspx";
        public const string PAGE_WorldSyncRequestFile = "WorldSyncRequestFile.aspx";
        public const string PAGE_WorldSyncFuseFile = "WorldSyncFuseFile.aspx";
        public const string PAGE_WorldSyncRequestReceipt = "WorldSyncRequestReceipt.aspx";

        public const string PAGE_BEQRC = "BEQRC.aspx";
        public const string PAGE_RegulatoryComments = "RegulatoryComments.aspx";

        // Dashboards
        public const string PAGE_TaskDashboard = "TaskDashboard.aspx";
        public const string PAGE_TaskDashboard_New = "TaskDashboard_New.aspx";
        public const string PAGE_AllProjectDashboard = "AllProjectDashboard.aspx";
        public const string PAGE_NAME_AllProjectDashboard = "All Project Dashboard";
        public const string PAGE_AllProjectDetailsDashboard = "AllProjectDetailsDashboard.aspx";
        public const string PAGE_AllProjectDetailsDashboard2 = "AllProjectDetailsDashboard2.aspx";
        public const string PAGE_NAME_AllProjectDetailsDashboard = "All Project Details Dashboard";
        public const string PAGE_AllParentProjectDetailsDashboard = "AllParentProjectDetailsDashboard.aspx";
        public const string PAGE_NAME_AllParentProjectDetailsDashboard = "All Parent Project Details Dashboard";
        public const string PAGE_HomeOBM = "HomeOBM.aspx";
        public const string PAGE_NAME_HomeOBM = "PM Dashboard";
        public const string PAGE_HomeBrandManager = "HomeBM.aspx";
        public const string PAGE_NAME_HomeBrandManager = "Brand Manager Dashboard";
        public const string PAGE_HomePackagingEngineer = "HomePE.aspx";
        public const string PAGE_NAME_HomePackagingEngineer = "Packaging Engineer Dashboard";
        public const string PAGE_HomeProcurement = "HomeProcurement.aspx";
        public const string PAGE_NAME_HomeProcurement = "Procurement Dashboard";
        public const string PAGE_HomeGraphics = "HomeGraphics.aspx";
        public const string PAGE_NAME_HomeGraphics = "Graphics Dashboard";
        public const string PAGE_HomeManufacturing = "HomeManufacturing.aspx";
        public const string PAGE_NAME_HomeManufacturing = "Manufacturing Dashboard";
        public const string PAGE_HomeQA = "HomeQA.aspx";
        public const string PAGE_NAME_HomeQA = "QA Dashboard";
        public const string PAGE_HomeMasterData = "HomeMasterData.aspx";
        public const string PAGE_NAME_HomeMasterData = "Master Data Dashboard";
        public const string PAGE_HomeCOMAN = "HomeCOMAN.aspx";
        public const string PAGE_NAME_HomeCOMAN = "COMAN Dashboard";
        public const string PAGE_HomeInternationalCompliance = "HomeIntComp.aspx";
        public const string PAGE_NAME_HomeInternationalCompliance = "International Compliance Dashboard";
        public const string PAGE_HomeInitialCosting = "HomeInitialCosting.aspx";
        public const string PAGE_NAME_HomeInitialCosting = "Initial Costing Dashboard";
        public const string PAGE_HomeInitialCapacity = "HomeInitialCapacity.aspx";
        public const string PAGE_NAME_HomeInitialCapacity = "Initial Capacity Dashboard";
        public const string PAGE_HomeTradePromoGroup = "HomeTradePromo.aspx";
        public const string PAGE_NAME_HomeTradePromoGroup = "Trade Promo Dashboard";
        public const string PAGE_HomeDistribution = "HomeDistribution.aspx";
        public const string PAGE_NAME_HomeDistribution = "Distribution Dashboard";
        public const string PAGE_HomeResearchDevelopment = "HomeRnD.aspx";
        public const string PAGE_NAME_HomeResearchDevelopment = "Research & Development Dashboard";
        public const string PAGE_HomeInitialApprover = "HomeInitialApprover.aspx";
        public const string PAGE_NAME_HomeInitialApprover = "Initial Approver Dashboard";

        public const string PAGE_ProjectStatusDashboard = "ProjectStatus.aspx";
        public const string PAGE_NAME_ProjectStatusDashboard = "Project Status Dashboard";
        public const string PAGE_UpdateProjectNotes = "UpdateProjectNotes.aspx";
        public const string PAGE_NAME_UpdateProjectNotes = "Update Project Notes";
        public const string PAGE_SGSGenerateIPF = "SGSGenerateIPF.aspx";
        public const string PAGE_NAME_SGSGenerateIPF = "Stage Gate Generate IPFs";

        public const string PAGE_BOMSetupPE = "BOMSetupPE.aspx";
        public const string PAGE_NAME_BOMSetupPE = "Bill of Materials Creation Form - PE1";
        public const string PAGE_BOMSetupProc = "BOMSetupProc.aspx";
        public const string PAGE_NAME_BOMSetupProc = "Bill of Materials Creation Form - Proc";
        public const string PAGE_BOMSetupPE2 = "BOMSetupPE2.aspx";
        public const string PAGE_NAME_BOMSetupPE2 = "Bill of Materials Creation Form - PE2";
        public const string PAGE_BOMSetupPE3 = "BOMSetupPE3.aspx";
        public const string PAGE_NAME_BOMSetupPE3 = "Bill of Materials Creation Form - PE3";

        //New BOM Setup Pages
        //For Flag Setup
        public const string PAGE_PE = "PE1.aspx";
        public const string PAGE_Proc = "PROC.aspx";
        public const string PAGE_PE2 = "PE2.aspx";
        public const string PAGE_PE3 = "PE3.aspx";
        public const string PAGE_NAME_PE = "Bill of Materials Creation Form - PE1";
        public const string PAGE_NAME_Proc = "Bill of Materials Creation Form - PROC";
        public const string PAGE_NAME_PE2 = "Bill of Materials Creation Form - PE2";
        public const string PAGE_NAME_PE3 = "Bill of Materials Creation Form - PE3";

        public const string PAGE_LISTVIEW_SLTSummary = "SLTSummary.aspx";
        public const string PAGE_LISTVIEW_SLTExceptionEveryday = "SLTExceptionEveryday.aspx";
        public const string PAGE_LISTVIEW_SLTExceptionCOMAN = "SLTExceptionCOMAN.aspx";
        public const string PAGE_LISTVIEW_SLTExceptionPrivateLabel = "SLTExceptionPrivateLabel.aspx";
        public const string PAGE_LISTVIEW_SLTExceptionEaster = "SLTExceptionEaster.aspx";
        public const string PAGE_LISTVIEW_SLTExceptionHalloween = "SLTExceptionHalloween.aspx";
        public const string PAGE_LISTVIEW_SLTExceptionSummer = "SLTExceptionSummer.aspx";
        public const string PAGE_LISTVIEW_SLTExceptionValentine = "SLTExceptionValentine.aspx";
        public const string PAGE_LISTVIEW_SLTExceptionChristmas = "SLTExceptionChristmas.aspx";

        public const string PAGE_LISTVIEW_AllOpenProjects = "AllOpenProjects.aspx";
        public const string PAGE_LISTVIEW_AgendaViewSeasonal = "AgendaViewSeasonal.aspx";
        public const string PAGE_LISTVIEW_AgendaViewEveryday = "AgendaViewEveryday.aspx";
        public const string PAGE_LISTVIEW_OBM = "OBM.aspx";
        public const string PAGE_LISTVIEW_OBMAdmin = "OBMAdmin.aspx";
        public const string PAGE_LISTVIEW_BrandManager = "BrandManager.aspx";
        public const string PAGE_LISTVIEW_FirstShipDate = "FirstShipDate.aspx";
        public const string PAGE_LISTVIEW_AllCompletedProjects = "AllCompletedProjects.aspx";
        public const string PAGE_LISTVIEW_AllCancelledProjects = "AllCancelledProjects.aspx";
        public const string PAGE_LISTVIEW_RndView = "RndView.aspx";

        public const string PAGE_REPORT_ProjectTotals = "ProjectTotals.aspx";
        public const string PAGE_NAME_ProjectTotals = "Project Totals";

        public const string PAGE_NAME_ItemProposal = "Item Proposal";
        public const string PAGE_NAME_OPS = "OPS";
        public const string PAGE_NAME_QA = "QA";
        public const string PAGE_NAME_PackagingEngineer = "Packaging Engineer Pre-Cost";
        public const string PAGE_NAME_FinalCosting = "Final Costing";
        public const string PAGE_NAME_InitialCosting = "Initial Costing";
        public const string PAGE_NAME_CustomerMarketing = "Customer Marketing";
        public const string PAGE_NAME_RequestforItem = "Request for Item";
        public const string PAGE_NAME_MaterialNumbers = "Material Numbers";
        public const string PAGE_NAME_GraphicsDevelopment = "Graphics Development";
        public const string PAGE_NAME_ItemRequestSetUp = "Item Request SetUp";
        public const string PAGE_NAME_SAPItemRequest = "SAP Item Request";
        public const string PAGE_NAME_CommercializationItem = "Commercialization Item";
        public const string PAGE_NAME_SAPItemSetup = "SAP Item Setup";
        public const string PAGE_NAME_TBDView = "TBD View";
        public const string PAGE_NAME_PMFirstReview = "PM First Review";
        public const string PAGE_NAME_InnovationProposal = "Innovation Proposal";
        public const string PAGE_NAME_InnovationRnD = "Innovation: Research and Development";
        public const string PAGE_NAME_InnovationReview = "Innovation: Review";
        public const string PAGE_NAME_CompassAdministration = "Compass Administration";
        public const string PAGE_NAME_MaterialReview = "Material Review";
        public const string PAGE_NAME_MaterialReviewEdit = "Material Review Edit";
        public const string PAGE_NAME_PlatesShipped = "Plates Shipped";
        public const string PAGE_NAME_GraphicsProgress = "Graphics Progress";
        public const string PAGE_NAME_SAPSemiRequest = "SAP Transfer Semi Request";

        #endregion

        #region GROUP Constants

        public const string GROUP_OBMAdmins = "OBM Administrators";

        public const string GROUP_ProjectManagers = "Project Manager Members";
        public const string GROUP_SeniorProjectManager = "Senior Project Managers";

        public const string GROUP_IPFSubmissionMembers = "IPF Submission Members";

        public const string GROUP_InTech = "InTech Members";
        public const string GROUP_InitialCosting = "Initial Costing Members";
        public const string GROUP_InitialCapacity = "Initial Capacity Members";
        public const string GROUP_TradePromo = "Trade Promo Members";
        public const string GROUP_EstimatedPricing = "Estimated Pricing Members";
        public const string GROUP_EstimatedBracketPricing = "Estimated Bracket Pricing Members";
        public const string GROUP_Distribution = "Distribution Members";
        public const string GROUP_Operations = "Operations Members";
        public const string GROUP_ExternalManufacturing = "External Manufacturing Members";
        public const string GROUP_QualityAssurance = "Quality Assurance Members";
        public const string GROUP_MasterData = "Master Data Members";
        public const string GROUP_MaterialWarehouse = "Material Warehouse Members";

        public const string GROUP_DemandPlanning = "Demand Planning Members";
        public const string GROUP_Graphics = "Graphics Members";
        public const string GROUP_GraphicsTask = "Graphics Task Members";
        public const string GROUP_InternationalCompliance = "International Compliance Members";
        public const string GROUP_PackagingEngineer = "Packaging Engineer Members";
        public const string GROUP_ProcurementPackaging = "Procurement Packaging Members";
        public const string GROUP_ProcurementCoManufacturingMembers = "Procurement Co-Manufacturing Members";
        public const string GROUP_FinalCosting = "Final Costing Members";
        public const string GROUP_InnovationReview = "Innovation Review Members";
        public const string GROUP_TradeSpending = "Trade Spending Members";
        public const string GROUP_SalesPlanning = "Sales Planning Members";
        public const string GROUP_Obsolescence = "Obsolescence Members";
        public const string GROUP_CustomerService = "Customer Service Members";

        public const string GROUP_PlantForestPark = "Plant-Forest Park Members";
        public const string GROUP_PlantBellwood = "Plant-Bellwood Members";
        public const string GROUP_PlantPackCenter = "Plant-Pack Center Members";
        public const string GROUP_PlantShipLab = "Plant-Ship Lab Members";
        public const string GROUP_PlantVernell = "Plant-Vernell Members";
        public const string GROUP_PlantReynosa = "Plant-Reynosa Members";

        public const string GROUP_PurchasingSeasonal = "Purchasing-Seasonal Members";
        public const string GROUP_PurchasingFilm = "Purchasing-Film Members";
        public const string GROUP_PurchasingCorrugated = "Purchasing-Corrugated Members";
        public const string GROUP_PurchasingOther = "Purchasing-Other Members";

        public const string GROUP_ProjectCancellationMembers = "Project Cancellation Members";

        public const string GROUP_Developers = "Developer Members";
        public const string GROUP_Sales = "Sales Members";
        public const string GROUP_Manufacturing = "Manufacturing Members";
        public const string GROUP_SupplyChain = "Supply Chain Members";
        public const string GROUP_Marketing = "Marketing Members";
        public const string GROUP_Legal = "Legal Members";
        public const string GROUP_LifeCycleMngmt = "Life Cycle Management Members";
        /// <summary>
        public const string GROUP_QualityInnovation = "Quality Innovation Members";
        public const string GROUP_InTechRegulatory = "InTech Regulatory Members";
        public const string GROUP_RegulatoryQA = "Regulatory QA Members";
        public const string GROUP_Finance = "Finance Members";
        /// </summary>

        public const string GROUP_NewFGWithReplacementItemMembers = "New FG With Replacement Item Members";
        //Procurement Groups
        public const string GROUP_ProcurementEBPFilm = "Procurement EBP Film";
        public const string GROUP_ProcurementEBPPaperboard = "Procurement EBP Paperboard";
        public const string GROUP_ProcurementEBPPurchased = "Procurement EBP Purchased";
        public const string GROUP_ProcurementEBPCorrugated = "Procurement EBP Corrugated";
        public const string GROUP_ProcurementEBPRigidPlastic = "Procurement EBP RigidPlastic";
        public const string GROUP_ProcurementEBPLabel = "Procurement EBP Label";
        public const string GROUP_ProcurementEBPMetal = "Procurement EBP Metal";
        public const string GROUP_ProcurementEBPAncillary = "Procurement EBP Ancillary";
        public const string GROUP_ProcurementEBPOther = "Procurement EBP Other";
        public const string GROUP_ProcurementSeasonal = "ProcurementSeasonal";
        public const string GROUP_ProcurementNovelty = "ProcurementNovelty";
        public const string GROUP_ProcurementCoMan = "ProcurementCoMan";
        public const string GROUP_ProcurementCatchAll = "ProcurementCatchAll";

        // These are individual indicators to send to a particular user as opposed to a Group
        public const string GROUP_IndividualBrandManager = "BrandMgr User";
        public const string GROUP_IndividualPackagingEngineer = "Packaging Engineer User";
        public const string GROUP_IndividualGraphics = "Graphics User";
        public const string GROUP_IndividualPM = "PM User";
        public const string GROUP_IndividualOBM = "OBM User";
        public const string GROUP_IndividualSeniorPM = "SrPM User";
        public const string GROUP_IndividualInTechLead = "InTech User";
        public const string GROUP_IndividualInitiator = "IPF Initiator";
        public const string GROUP_IndividualProjectLeader = "Project Lead User";
        public const string GROUP_GraphicsLead = "Graphics Lead";
        public const string GROUP_WorldSync = "World Sync Members";

        public const string GROUP_ProcSeasonal = "Procurement Seasonal";
        public const string GROUP_ProcEverydayCorrugatePPBRD = "Proc Everyday Corrugate PPBD";
        public const string GROUP_ProcEverydayFilmLabelTubs = "Proc Everyday Film Label Tubs";
        public const string GROUP_ExternalManTurnKeyFG = "External Man Turnkey FG";
        public const string GROUP_WorldSyncGraphics = "WorldSync Graphics Members";
        public const string GROUP_NewPackagingComponentsCreated = "New Packaging Components Created Members";

        public const string GROUP_ConsumerRelations = "Consumer Relations Members";
        public const string GROUP_PostLaunchNotificationRnDMembers = "Post Launch Notification RnD Members";
        #endregion

        #region ACCESS Constants
        public const string ACCESS_Read = "Read";
        public const string ACCESS_Contribute = "Contribute";
        public const string ACCESS_Full = "Full";
        #endregion

        #region FILEEXTENSION Constants
        public static IEnumerable<string> FileExtentions
        {
            get
            {
                return new List<string>
                                         {
                                             "doc",
                                             "docx",
                                             "txt",
                                             "xls",
                                             "xlsx",
                                             "ppt",
                                             "pptx",
                                             "pdf",
                                             "jpg",
                                             "jpeg",
                                             "png",
                                             "bmp",
                                             "gif"
                                         };

            }
        }
        #endregion

        #region DOCUMENT TYPE Constants
        public const string DOCTYPE_StageGateProjectBrief = "Stage Gate Project Brief";
        public const string DOCTYPE_StageGateOthers = "Stage Gate Others";
        public const string DOCTYPE_StageGateBriefPDF = "Stage Gate Brief PDF";
        public const string DOCTYPE_StageGateBriefImage = "Stage Gate Brief Image";
        public const string DOCTYPE_StageGateGateDocument = "Stage Gate Gate Document";
        public const string DOCTYPE_StageGateFinanceBriefPDF = "Stage Gate Finance Brief PDF";
        public const string DOCTYPE_Formulation = "Formulation";
        public const string DOCTYPE_Graphics = "Graphics";
        public const string DOCTYPE_NLEA = "NLEA";
        public const string DOCTYPE_CADDrawing = "CADDrawing";
        public const string DOCTYPE_PalletPattern = "PalletPattern";
        public const string DOCTYPE_SemiPalletPattern = "SemiPalletPattern";
        public const string DOCTYPE_GraphicsRequest = "GraphicsRequest";
        public const string DOCTYPE_CAPACITY = "Capacity";
        public const string DOCTYPE_COSTING = "Costing";
        public const string DOCTYPE_INNOVATION = "Innovation";
        public const string DOCTYPE_PreliminarySpecs = "PreliminarySpec";
        public const string DOCTYPE_Rendering = "Rendering";
        public const string DOCTYPE_ApprovedGraphicsAsset = "ApprovedGraphicsAsset";
        public const string DOCTYPE_BEQRCodeEPSFile = "BEQRCodeEPSFile";
        public const string DOCTYPE_ExternalComponentDieline = "ExternalComponentDieline";
        public const string DOCTYPE_MaterialReview = "MaterialReview";
        public const string DOCTYPE_Dieline = "Dieline";
        public const string DOCTYPE_PackTrial = "PackTrial";
        public const string DOCTYPE_BRACKETPRICING = "BracketPricing";
        public const string DOCTYPE_LineExtension = "Line Extension";
        public const string DOCTYPE_RequestImage = "RequestImage";
        public const string DOCTYPE_RequestNutritional = "RequestNutritional";
        public const string DOCTYPE_PACKSPECS = "PackagingSpecifications";
        public const string DOCTYPE_FUSEFILE = "FUSEFile";
        #endregion

        #region LIST VIEW Constants
        public const string VIEW_SLTSummary = "SLT Summary";
        public const string VIEW_SLTExceptionEveryday = "SLT Exception Everyday";
        public const string VIEW_SLTExceptionCOMAN = "SLT Exception COMAN";
        public const string VIEW_SLTExceptionPrivateLabel = "SLT Exception PrivateLabel";

        public const string VIEW_SLTExceptionEaster = "SLT Exception Easter";
        public const string VIEW_SLTExceptionHalloween = "SLT Exception Halloween";
        public const string VIEW_SLTExceptionSummer = "SLT Exception Summer";
        public const string VIEW_SLTExceptionValentine = "SLT Exception Valentine";
        public const string VIEW_SLTExceptionChristmas = "SLT Exception Christmas";

        public const string VIEW_AllOpenProjects = "All Open Projects";
        public const string VIEW_AgendaViewSeasonal = "Agenda View - Seasonal";
        public const string VIEW_AgendaViewEveryday = "Agenda View - Everyday";
        public const string VIEW_PM = "PM View";
        public const string VIEW_OBMAdmin = "OBM Admin View";
        public const string VIEW_BrandManager = "Brand Manager View";
        public const string VIEW_AllItems = "All Items";
        public const string VIEW_FirstShipDate = "First Ship Date";
        public const string VIEW_AllCompletedProjects = "All Completed Projects";
        public const string VIEW_AllCancelledProjects = "All Cancelled Projects";
        public const string VIEW_RND = "RnD Project View";
        public const string VIEW_MyOpenTasksDash = "MyOpenTask sDashboard Details";

        public const string VIEW_OBM_Summary = "OBM Summary View";

        public const string VIEW_Graphics_Current_Tasks = "Graphics Current Tasks";
        #endregion

        #region Project Status Constants
        public const string PROJECTSTATUS_Green = "Green";
        public const string PROJECTSTATUS_Red = "Red";
        public const string PROJECTSTATUS_Yellow = "Yellow";
        #endregion

        #region EventReceiver Contants
        public static readonly string SharePointServiceAssemblyName = "Ferrara.Compass.Services, Version=1.0.0.0, Culture=neutral, PublicKeyToken=04ae2c9e0ea4efe6";
        public static readonly string ConfigurationListEventReceiverClassName = "Ferrara.Compass.Services.EventReceivers.ConfigurationListEventReceiver";
        public static readonly string EmailTemplateListEventReceiverClassName = "Ferrara.Compass.Services.EventReceivers.EmailTemplateListEventReceiver";
        public static readonly string WorkflowStepListEventReceiverClassName = "Ferrara.Compass.Services.EventReceivers.WorkflowStepListEventReceiver";
        public static readonly string FormAccessListEventReceiverClassName = "Ferrara.Compass.Services.EventReceivers.FormAccessListEventReceiver";
        public static readonly string OBMBrandManagerLookupListEventReceiverClassName = "Ferrara.Compass.Services.EventReceivers.OBMBrandManagerLookupListEventReceiver";
        #endregion

        #region Content Types
        public const string CONTENTTYPE_CompassLookup = "Compass Lookup";
        #endregion

        #region Project Type Constants
        public const string PROJECTTYPE_Innovation = "Innovation";
        public const string PROJECTTYPE_LineExtension = "Line Extenstion";
        public const string PROJECTTYPE_DownweightTransition = "Downweight/Transitions";
        public const string PROJECTTYPE_GraphicsChangesInternalAdjustments = "Graphics Changes/Internal Adjustments";
        public const string PROJECTTYPE_GraphicsChangeOnly = "Graphics Change Only";
        public const string PROJECTTYPE_SimpleNetworkMove = "Simple Network Move";
        #endregion

        #region StageLookup Values
        public const string StageLookup_Design = "Design";
        public const string StageLookup_Develop = "Develop";
        public const string StageLookup_Validate = "Validate";
        public const string StageLookup_Industrialize = "Industrialize";
        #endregion

        public const string PROJECTTYPESUBCATEGORY_ComplexNetworkMove = "Complex Network Move";
        public const string PROJECTTYPESUBCATEGORY_NetworkMove = "Network Move";

        #region Packaging Type Constants
        public const string PACKAGINGTYPE_FGBOM = "FGBOM";
        public const string PACKAGINGTYPE_SEMIBOM = "SEMIBOM";
        public const string PACKAGINGTYPE_CANDYSEMIBOM = "CANDYSEMIBOM";
        public const string PACKAGINGTYPE_PURCHASEDSEMIBOM = "PURCHASEDSEMIBOM";
        public const string PACKAGINGTYPE_SEMIBOM_Display = "TRANSFER SEMI BOM";
        public const string PACKAGINGTYPE_PURCHASEDSEMIBOM_Display = "PUR CANDY BOM";
        #endregion

        #region SAP BBM List MaterialTypes Constants
        public const string SAPBOMLIST_TRANSFERSEMI = "TRANSFER";
        public const string SAPBOMLIST_CANDYSEMI = "CANDY";
        public const string SAPBOMLIST_PURCHASEDSEMI = "PURCHASED";
        #endregion

        #region Packaging New or Existing
        public const string PACKAGINGNEWEXISTING_NEW = "New";
        public const string PACKAGINGNEWEXISTINGE_EXISTING = "Existing";
        #endregion

        #region TBD Indicator Constants
        public const string TBDINDICATOR_New = "New";
        public const string TBDINDICATOR_Existing = "Existing";
        #endregion

        #region
        public const string PRODUCT_HIERARCHY1_Bulk = "Bulk (000000024)";
        public const string PRODUCT_HIERARCHY1_CoMan = "Co-Manufacturing (000000027)";
        public const string PRODUCT_HIERARCHY1_Everyday = "Everyday (000000025)";
        public const string PRODUCT_HIERARCHY1_PrivateLabel = "Private Label (000000026)";
        public const string PRODUCT_HIERARCHY1_Seasonal = "Seasonal (000000023)";
        public const string PRODUCT_HIERARCHY1_LBB = "LBB (000000030)";
        #endregion
        #region
        public const string CompanyCode_FPCODCs = "FPCO - Fruit Snack/Confections Business";
        public const string CompanyCode_SELLDCs = "SELL - Cookies, Cones, Crust Business";
        public const string CompanyCode_FERQDCs = "FERQ - LBB Business";
        #endregion

        #region Stage Gate Approval Decision Constants
        public const string APPROVER_DECISION_Approved = "Approved";
        public const string APPROVER_DECISION_Rejected = "Rejected";
        public const string APPROVER_DECISION_RequestIPFupdate = "Request IPF Update";
        #endregion

        #region Workflow Phases
        public const string WORKFLOWPHASE_IPF = "IPF Phase";
        public const string WORKFLOWPHASE_SrOBMInitialReview = "Sr. OBM Review Phase";
        public const string WORKFLOWPHASE_PMInitialReview = "PM Initial Review Phase";
        public const string WORKFLOWPHASE_PrelimSAPInitialItemSetup = "Preliminary SAPSetup Phase";
        public const string WORKFLOWPHASE_Coordination = "Coordination Phase";
        public const string WORKFLOWPHASE_OBMFirstReview = "OBM 1st Review";
        public const string WORKFLOWPHASE_BOMCreation = "BOM Creation Phase";
        public const string WORKFLOWPHASE_OBMSecondReview = "OBM 2nd Review";
        public const string WORKFLOWPHASE_FinalSetup = "Final Setup Phase";
        public const string WORKFLOWPHASE_OBMThirdReview = "OBM 3rd Review";
        public const string WORKFLOWPHASE_ProductionReadiness = "Production Readiness Phase";
        public const string WORKFLOWPHASE_OnHold = "On Hold";
        public const string WORKFLOWPHASE_Completed = "Completed";
        public const string WORKFLOWPHASE_Cancelled = "Cancelled";
        public const string WORKFLOWPHASE_PreProduction = "Pre-Production";
        public const string WORKFLOWPHASE_GraphicsOnlyPhase = "Graphics Only Phase";
        public const string WORKFLOWPHASE_ExtGraphicsOnlyPhase = "Ext Graphics Only Phase";
        #endregion

        public const string DETAILFORMSTATUS_Completed = "Completed";
        public const string DETAILFORMSTATUS_Partial = "Waiting";

        public const string WORLDSYNCREQ_InProcess = "InProcess";
        public const string WORLDSYNCREQ_Completed = "Completed";
        public const string WORLDSYNCREQ_STEP_RequestFile = "RequestFile";
        public const string WORLDSYNCREQ_STEP_ReceiveFile = "ReceiveFile";

        #region Packaging Component Types
        public const string COMPONENTTYPE_PurchasedSemi = "Purchased Candy Semi";
        public const string COMPONENTTYPE_TransferSemi = "Transfer Semi";
        public const string COMPONENTTYPE_CandySemi = "Candy Semi";

        #endregion

        #region External Manufacturing
        public const string EXTERNAL_MANUFACTURER = "Externally Manufactured";
        public const string EXTERNAL_PACKER = "Externally Packed";
        #endregion

        #region Material Types
        public const string MaterialType_Pack = "PACK";
        public const string MaterialType_TransferSemi = "TRANSFER";
        public const string MaterialType_PurchasedSemi = "PURCANDY";
        public const string MaterialType_CandySemi = "CANDY";
        public const string MaterialType_Raw = "RAW";
        #endregion

        #region Export Sync Constants
        public const string EXP_SYNC_TEMPLATE_NAME = "FUSE_Supplier_Template.xlsx";
        public const string EXP_SYNC_FILENAME = "FUSE_Supplier{compassId}.xlsx";
        public const string WorldSyncFuse_FILENAME = "MassFUSEExport_{YYYY}_{MM}_{DD}_{HH}_{MMM}_{SS}.xlsx";
        public const int EXP_SYNC_ROW_HDR_ITM = 3;
        public const int EXP_SYNC_ROW_HDR_PUB = 2;
        public const int EXP_SYNC_ROW_HDR_LNK = 2;
        public const int EXP_SYNC_ROW_STRT_ITM = 4;
        public const int EXP_SYNC_ROW_STRT_PUB = 3;

        public const int EXP_SYNC_ROW_STRT_LNK = 3;
        public const string EXP_SYNC_SHT_ITM = "FS_Item";
        public const string EXP_SYNC_SHT_PUB = "FS_Publication";
        public const string EXP_SYNC_SHEET_LNK = "FS_Link";

        public const string EXP_SYNC_ITM_ItemID = "gtin";
        public const string EXP_SYNC_ITM_ItemName = "gtinName";
        public const string EXP_SYNC_ITM_BrandName = "brandName";
        public const string EXP_SYNC_ITM_Depth = "depth";
        public const string EXP_SYNC_ITM_Height = "height";
        public const string EXP_SYNC_ITM_Width = "width";
        public const string EXP_SYNC_ITM_GrossWeight = "grossWeight";
        public const string EXP_SYNC_ITM_NetWeight = "netWeight";
        public const string EXP_SYNC_ITM_GS1TradeItemIDKeyValue = "gs1TradeItemIdentificationKey/value";
        public const string EXP_SYNC_ITM_ShortDescription = "shortDescription";
        public const string EXP_SYNC_ITM_ProductDescription = "productDescription";
        public const string EXP_SYNC_ITM_FunctionalName = "functionalName";
        public const string EXP_SYNC_ITM_Volume = "volume";
        public const string EXP_SYNC_ITM_QtyofNextLevelItem = "totalQuantityOfNextLowerTradeItem";
        public const string EXP_SYNC_ITM_NumberofItemsinaCompleteLayerGTINPalletTi = "ti";
        public const string EXP_SYNC_ITM_NumberofCompleteLayersContainedinItemGTINPalletHi = "hi";
        public const string EXP_SYNC_ITM_AlternateItemIdentificationId = "alternateItemIdentification/id";
        public const string EXP_SYNC_ITM_MinProductLifespanfromProduction = "minimumTradeItemLifespanFromProduction";
        public const string EXP_SYNC_ITM_StartAvailabilityDate = "startAvailabilityDate";
        public const string EXP_SYNC_ITM_EffectiveDate = "effectiveDate";

        public const string EXP_SYNC_PUB_ItemID = "Item ID";
        public const string EXP_SYNC_PUB_PublishDate = "Publish Date";

        public const string EXP_SYNC_LKN_ParentItemID = "Parent Item ID";
        public const string EXP_SYNC_LKN_ChildItemID = "Child Item ID";
        public const string EXP_SYNC_LKN_QtyofChildItem = "Qty of Child Item";
        #endregion

        #region PrentChild Constants
        public const string ParentChild_Parent = "Parent";
        public const string ParentChild_Child = "Child";
        #endregion

    }
}
