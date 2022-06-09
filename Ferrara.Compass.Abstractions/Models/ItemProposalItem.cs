using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferrara.Compass.Abstractions.Models
{
    [Serializable]
    public class ItemProposalItem
    {
        public ItemProposalItem()
        {
            mAnnualProjectedUnits = -9999;
            mMonth1ProjectedUnits = -9999;
            mMonth2ProjectedUnits = -9999;
            mMonth3ProjectedUnits = -9999;
            mAnnualProjectedDollars = -9999;
            mMonth1ProjectedDollars = -9999;
            mMonth3ProjectedDollars = -9999;
            mTruckLoadPricePerSellingUnit = -9999;
            mLast12MonthSales = -9999;
            mBaseUOMNetWeightLbs = -9999;
            mExpectedGrossMarginPercent = -9999;
            mRevisedGrossMarginPercent = -9999;
            mRetailSellingUnitsBaseUOM = -9999;
            mRetailUnitWieghtOz = -9999;
        }

        #region Variables
        private int mCompassListItemId;
        private int mStageGateProjectListItemId;
        private string mParentProjectNumber;
        private string mCommercializationLink;
        private string mChangeLink;
        private string mCopyLink;
        private string mWorkflowStatusLink;
        private string mLastUpdatedFormName;

        #region IPF Variables
        private int mPMTWorkflowVersion;
        private string mNewIPF;
        private DateTime mSubmittedDate;
        private string mProjectNumber;
        private string mInitiator;
        private DateTime mFirstShipDate;
        private DateTime mRevisedFirstShipDate;
        private string mCustomer;
        private string mCaseType;
        private int mAnnualProjectedUnits;
        private int mMonth1ProjectedUnits;
        private int mMonth2ProjectedUnits;
        private int mMonth3ProjectedUnits;
        private double mAnnualProjectedDollars;
        private double mMonth1ProjectedDollars;
        private double mMonth2ProjectedDollars;
        private double mMonth3ProjectedDollars;
        private string mCustomerSpecific;
        private string mCustomerSpecificLotCode;
        private string mLikeFGItemNumber;
        private string mLikeFGItemDescription;
        private string mOldFGItemNumber;
        private string mOldFGItemDescription;
        private double mTruckLoadPricePerSellingUnit;
        private string mMarketClaimsLabelingRequirements;
        private string mCountryOfSale;
        private string mProjectType;
        private string mTBDIndicator;
        private double mLast12MonthSales;
        private string mSAPBaseUOM;
        private double mBaseUOMNetWeightLbs;
        private string mOrganic;
        private string mChannel;
        private int mGenerateIPFSortOrder;
        private string mSAPItemNumber;
        private string mSAPDescription;
        private string mProductHierarchyLevel1;
        private string mManuallyCreateSAPDescription;
        private string mProductHierarchyLevel2;
        private string mNovelyProject;
        private string mCoManClassification;
        private string mProjectNotes;
        private string mItemConcept;
        private string mCopyFormsForGraphicsProject;
        private string mExternalSemisItem;
        private int mIPFCopiedFromCompassListItemId;
        private string mIsDisplayBoxConsumerUnit;
        private string mPM;
        private string mNewFormula;
        private string mNewShape;
        private string mNewFlavorColor;
        private string mNewNetWeight;
        private string mDTVProject;
        private string mMfgLocationChange;
        private string mServingSizeWeightChange;
        private double mExpectedGrossMarginPercent;
        private double mRevisedGrossMarginPercent;
        private string mSoldOutsideUSA;
        private string mMaterialGroup1Brand;
        private string mMaterialGroup4ProductForm;
        private string mMaterialGroup5PackType;
        private string mProductFormDescription;
        private string mTotalQuantityUnitsInDisplay;
        private string mRequireNewUPCUCC;
        private string mRequireNewUnitUPC;
        private string mUnitUPC;
        private string mRequireNewDisplayBoxUPC;
        private string mDisplayBoxUPC;
        private string mRequireNewCaseUCC;
        private string mCaseUCC;
        private string mRequireNewPalletUCC;
        private string mPalletUCC;
        private string mFlowthrough;
        private int mNumberofTraysPerBaseUOM;
        private int mRetailSellingUnitsBaseUOM;
        private double mRetailUnitWieghtOz;
        private string mFilmSubstrate;
        private string mPegHoleNeeded;
        private string mInvolvesCarton;
        private string mUnitsInsideCarton;
        private double mIndividualPouchWeight;
        private DateTime mExpectedPackagingSwitchDate;
        private string mReasonForChange;
        private string mInitiatorName;
        private string mPMName;
        private string mProjectTypeSubCategory;
        private string mTestProject;
        private bool mNeedsNewBtn;
        private bool mCreateIPFBtn;
        private string mProjectStatus;
        private string mManufacturingLocation;
        private string mPackingLocation;
        //new SGS Fields
        private string mProjectLeader;
        private string mProjectLeaderName;
        private string mSrProjectManager;
        private string mSrProjectManagerName;
        private string mQA;
        private string mQAName;
        private string mInTech;
        private string mInTechName;
        private string mInTechRegulatory;
        private string mInTechRegulatoryName;
        private string mRegulatoryQA;
        private string mRegulatoryQAName;
        private string mPackagingEngineering;
        private string mPackagingEngineeringName;
        private string mSupplyChain;
        private string mSupplyChainName;
        private string mFinance;
        private string mFinanceName;
        private string mSales;
        private string mSalesName;
        private string mManufacturing;
        private string mManufacturingName;
        private string mOtherTeamMembers;
        private string mOtherTeamMembersName;
        private string mLifeCycleManagement;
        private string mLifeCycleManagementName;
        private string mPackagingProcurement;
        private string mPackagingProcurementName;
        private string mExtManufacturingProc;
        private string mExtManufacturingProcName;
        private string mMarketing;
        private string mMarketingName;
        private string mFlowthroughDets;
        private string mWorkflowPhase;
        private string mAllUsers;

        private string mProfitCenter;
        private string mPLMFlag;

        private string mFGReplacingAnExistingFG;
        private string mIsThisAnLTOItem;
        private string mRequestChangeToFGNumForSameUCC;
        private DateTime mLTOTransitionStartWindowRDD;
        private DateTime mLTOTransitionEndWindowRDD;
        private string mLTOEndDateFlexibility;
        #endregion

        #endregion

        #region Properties
        #region General Properties
        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        public int StageGateProjectListItemId { get { return mStageGateProjectListItemId; } set { mStageGateProjectListItemId = value; } }
        public string ParentProjectNumber { get { return mParentProjectNumber; } set { mParentProjectNumber = value; } }
        public string CommercializationLink { get { return mCommercializationLink; } set { mCommercializationLink = value; } }
        public string ChangeLink { get { return mChangeLink; } set { mChangeLink = value; } }
        public string CopyLink { get { return mCopyLink; } set { mCopyLink = value; } }
        public string WorkflowStatusLink { get { return mWorkflowStatusLink; } set { mWorkflowStatusLink = value; } }
        public string LastUpdatedFormName { get { return mLastUpdatedFormName; } set { mLastUpdatedFormName = value; } }
        public bool CreateIPFBtn { get { return mCreateIPFBtn; } set { mCreateIPFBtn = value; } }
        public bool NeedsNewBtn { get { return mNeedsNewBtn; } set { mNeedsNewBtn = value; } }
        public string ProjectStatus { get { return mProjectStatus; } set { mProjectStatus = value; } }
        #endregion

        #region IPF Properties
        public int PMTWorkflowVersion { get { return mPMTWorkflowVersion; } set { mPMTWorkflowVersion = value; } }
        public string NewIPF { get { return mNewIPF; } set { mNewIPF = value; } }
        public DateTime SubmittedDate { get { return mSubmittedDate; } set { mSubmittedDate = value; } }
        public string ProjectNumber { get { return mProjectNumber; } set { mProjectNumber = value; } }
        public string Initiator { get { return mInitiator; } set { mInitiator = value; } }
        public DateTime FirstShipDate { get { return mFirstShipDate; } set { mFirstShipDate = value; } }
        public DateTime RevisedFirstShipDate { get { return mRevisedFirstShipDate; } set { mRevisedFirstShipDate = value; } }
        public string Customer { get { return mCustomer; } set { mCustomer = value; } }
        public string CaseType { get { return mCaseType; } set { mCaseType = value; } }
        public int AnnualProjectedUnits { get { return mAnnualProjectedUnits; } set { mAnnualProjectedUnits = value; } }
        public int Month1ProjectedUnits { get { return mMonth1ProjectedUnits; } set { mMonth1ProjectedUnits = value; } }
        public int Month2ProjectedUnits { get { return mMonth2ProjectedUnits; } set { mMonth2ProjectedUnits = value; } }
        public int Month3ProjectedUnits { get { return mMonth3ProjectedUnits; } set { mMonth3ProjectedUnits = value; } }
        public double AnnualProjectedDollars { get { return mAnnualProjectedDollars; } set { mAnnualProjectedDollars = value; } }
        public double Month1ProjectedDollars { get { return mMonth1ProjectedDollars; } set { mMonth1ProjectedDollars = value; } }
        public double Month2ProjectedDollars { get { return mMonth2ProjectedDollars; } set { mMonth2ProjectedDollars = value; } }
        public double Month3ProjectedDollars { get { return mMonth3ProjectedDollars; } set { mMonth3ProjectedDollars = value; } }
        public string CustomerSpecific { get { return mCustomerSpecific; } set { mCustomerSpecific = value; } }
        public string CustomerSpecificLotCode { get { return mCustomerSpecificLotCode; } set { mCustomerSpecificLotCode = value; } }
        public string LikeFGItemNumber { get { return mLikeFGItemNumber; } set { mLikeFGItemNumber = value; } }
        public string LikeFGItemDescription { get { return mLikeFGItemDescription; } set { mLikeFGItemDescription = value; } }
        public string OldFGItemNumber { get { return mOldFGItemNumber; } set { mOldFGItemNumber = value; } }
        public string OldFGItemDescription { get { return mOldFGItemDescription; } set { mOldFGItemDescription = value; } }
        public double TruckLoadPricePerSellingUnit { get { return mTruckLoadPricePerSellingUnit; } set { mTruckLoadPricePerSellingUnit = value; } }
        public string MarketClaimsLabelingRequirements { get { return mMarketClaimsLabelingRequirements; } set { mMarketClaimsLabelingRequirements = value; } }
        public string CountryOfSale { get { return mCountryOfSale; } set { mCountryOfSale = value; } }
        public string ProjectType { get { return mProjectType; } set { mProjectType = value; } }
        public string TBDIndicator { get { return mTBDIndicator; } set { mTBDIndicator = value; } }
        public double Last12MonthSales { get { return mLast12MonthSales; } set { mLast12MonthSales = value; } }
        public string SAPBaseUOM { get { return mSAPBaseUOM; } set { mSAPBaseUOM = value; } }
        public double BaseUOMNetWeightLbs { get { return mBaseUOMNetWeightLbs; } set { mBaseUOMNetWeightLbs = value; } }
        public string Organic { get { return mOrganic; } set { mOrganic = value; } }
        public string Channel { get { return mChannel; } set { mChannel = value; } }
        public int GenerateIPFSortOrder { get { return mGenerateIPFSortOrder; } set { mGenerateIPFSortOrder = value; } }
        public string SAPItemNumber { get { return mSAPItemNumber; } set { mSAPItemNumber = value; } }
        public string SAPDescription { get { return mSAPDescription; } set { mSAPDescription = value; } }
        public string ProductHierarchyLevel1 { get { return mProductHierarchyLevel1; } set { mProductHierarchyLevel1 = value; } }
        public string ManuallyCreateSAPDescription { get { return mManuallyCreateSAPDescription; } set { mManuallyCreateSAPDescription = value; } }
        public string ProductHierarchyLevel2 { get { return mProductHierarchyLevel2; } set { mProductHierarchyLevel2 = value; } }
        public string NovelyProject { get { return mNovelyProject; } set { mNovelyProject = value; } }
        public string CoManClassification { get { return mCoManClassification; } set { mCoManClassification = value; } }
        public string ProjectNotes { get { return mProjectNotes; } set { mProjectNotes = value; } }
        public string ItemConcept { get { return mItemConcept; } set { mItemConcept = value; } }
        public string CopyFormsForGraphicsProject { get { return mCopyFormsForGraphicsProject; } set { mCopyFormsForGraphicsProject = value; } }
        public string ExternalSemisItem { get { return mExternalSemisItem; } set { mExternalSemisItem = value; } }
        public int IPFCopiedFromCompassListItemId { get { return mIPFCopiedFromCompassListItemId; } set { mIPFCopiedFromCompassListItemId = value; } }
        public string IsDisplayBoxConsumerUnit { get { return mIsDisplayBoxConsumerUnit; } set { mIsDisplayBoxConsumerUnit = value; } }
        public string PM { get { return mPM; } set { mPM = value; } }
        public string NewFormula { get { return mNewFormula; } set { mNewFormula = value; } }
        public string NewShape { get { return mNewShape; } set { mNewShape = value; } }
        public string NewFlavorColor { get { return mNewFlavorColor; } set { mNewFlavorColor = value; } }
        public string NewNetWeight { get { return mNewNetWeight; } set { mNewNetWeight = value; } }
        public string DTVProject { get { return mDTVProject; } set { mDTVProject = value; } }
        public string MfgLocationChange { get { return mMfgLocationChange; } set { mMfgLocationChange = value; } }
        public string ServingSizeWeightChange { get { return mServingSizeWeightChange; } set { mServingSizeWeightChange = value; } }
        public double ExpectedGrossMarginPercent { get { return mExpectedGrossMarginPercent; } set { mExpectedGrossMarginPercent = value; } }
        public double RevisedGrossMarginPercent { get { return mRevisedGrossMarginPercent; } set { mRevisedGrossMarginPercent = value; } }
        public string SoldOutsideUSA { get { return mSoldOutsideUSA; } set { mSoldOutsideUSA = value; } }
        public string MaterialGroup1Brand { get { return mMaterialGroup1Brand; } set { mMaterialGroup1Brand = value; } }
        public string MaterialGroup4ProductForm { get { return mMaterialGroup4ProductForm; } set { mMaterialGroup4ProductForm = value; } }
        public string MaterialGroup5PackType { get { return mMaterialGroup5PackType; } set { mMaterialGroup5PackType = value; } }
        public string ProductFormDescription { get { return mProductFormDescription; } set { mProductFormDescription = value; } }
        public string TotalQuantityUnitsInDisplay { get { return mTotalQuantityUnitsInDisplay; } set { mTotalQuantityUnitsInDisplay = value; } }
        public string RequireNewUPCUCC { get { return mRequireNewUPCUCC; } set { mRequireNewUPCUCC = value; } }
        public string RequireNewUnitUPC { get { return mRequireNewUnitUPC; } set { mRequireNewUnitUPC = value; } }
        public string UnitUPC { get { return mUnitUPC; } set { mUnitUPC = value; } }
        public string RequireNewDisplayBoxUPC { get { return mRequireNewDisplayBoxUPC; } set { mRequireNewDisplayBoxUPC = value; } }
        public string DisplayBoxUPC { get { return mDisplayBoxUPC; } set { mDisplayBoxUPC = value; } }
        public string RequireNewCaseUCC { get { return mRequireNewCaseUCC; } set { mRequireNewCaseUCC = value; } }
        public string CaseUCC { get { return mCaseUCC; } set { mCaseUCC = value; } }
        public string RequireNewPalletUCC { get { return mRequireNewPalletUCC; } set { mRequireNewPalletUCC = value; } }
        public string PalletUCC { get { return mPalletUCC; } set { mPalletUCC = value; } }
        public string Flowthrough { get { return mFlowthrough; } set { mFlowthrough = value; } }
        public int NumberofTraysPerBaseUOM { get { return mNumberofTraysPerBaseUOM; } set { mNumberofTraysPerBaseUOM = value; } }
        public int RetailSellingUnitsBaseUOM { get { return mRetailSellingUnitsBaseUOM; } set { mRetailSellingUnitsBaseUOM = value; } }
        public double RetailUnitWieghtOz { get { return mRetailUnitWieghtOz; } set { mRetailUnitWieghtOz = value; } }
        public string FilmSubstrate { get { return mFilmSubstrate; } set { mFilmSubstrate = value; } }
        public string PegHoleNeeded { get { return mPegHoleNeeded; } set { mPegHoleNeeded = value; } }
        public string InvolvesCarton { get { return mInvolvesCarton; } set { mInvolvesCarton = value; } }
        public string UnitsInsideCarton { get { return mUnitsInsideCarton; } set { mUnitsInsideCarton = value; } }
        public double IndividualPouchWeight { get { return mIndividualPouchWeight; } set { mIndividualPouchWeight = value; } }
        public DateTime ExpectedPackagingSwitchDate { get { return mExpectedPackagingSwitchDate; } set { mExpectedPackagingSwitchDate = value; } }
        public string ReasonForChange { get { return mReasonForChange; } set { mReasonForChange = value; } }
        public string InitiatorName { get { return mInitiatorName; } set { mInitiatorName = value; } }
        public string PMName { get { return mPMName; } set { mPMName = value; } }
        public string ProjectTypeSubCategory { get { return mProjectTypeSubCategory; } set { mProjectTypeSubCategory = value; } }
        public string ManufacturingLocation { get { return mManufacturingLocation; } set { mManufacturingLocation = value; } }
        public string PackingLocation { get { return mPackingLocation; } set { mPackingLocation = value; } }
        public string TestProject { get { return mTestProject; } set { mTestProject = value; } }

        //new SGS Fields
        public string ProjectLeader { get { return mProjectLeader; } set { mProjectLeader = value; } }
        public string ProjectLeaderName { get { return mProjectLeaderName; } set { mProjectLeaderName = value; } }
        public string SrProjectManager { get { return mSrProjectManager; } set { mSrProjectManager = value; } }
        public string SrProjectManagerName { get { return mSrProjectManagerName; } set { mSrProjectManagerName = value; } }
        public string QA { get { return mQA; } set { mQA = value; } }
        public string QAName { get { return mQAName; } set { mQAName = value; } }
        public string InTechRegulatory { get { return mInTechRegulatory; } set { mInTechRegulatory = value; } }
        public string InTechRegulatoryName { get { return mInTechRegulatoryName; } set { mInTechRegulatoryName = value; } }
        public string InTech { get { return mInTech; } set { mInTech = value; } }
        public string InTechName { get { return mInTechName; } set { mInTechName = value; } }
        public string RegulatoryQA { get { return mRegulatoryQA; } set { mRegulatoryQA = value; } }
        public string RegulatoryQAName { get { return mRegulatoryQAName; } set { mRegulatoryQAName = value; } }
        public string PackagingEngineering { get { return mPackagingEngineering; } set { mPackagingEngineering = value; } }
        public string PackagingEngineeringName { get { return mPackagingEngineeringName; } set { mPackagingEngineeringName = value; } }
        public string SupplyChain { get { return mSupplyChain; } set { mSupplyChain = value; } }
        public string SupplyChainName { get { return mSupplyChainName; } set { mSupplyChainName = value; } }
        public string Finance { get { return mFinance; } set { mFinance = value; } }
        public string FinanceName { get { return mFinanceName; } set { mFinanceName = value; } }
        public string Sales { get { return mSales; } set { mSales = value; } }
        public string SalesName { get { return mSalesName; } set { mSalesName = value; } }
        public string Manufacturing { get { return mManufacturing; } set { mManufacturing = value; } }
        public string ManufacturingName { get { return mManufacturingName; } set { mManufacturingName = value; } }
        public string OtherTeamMembers { get { return mOtherTeamMembers; } set { mOtherTeamMembers = value; } }
        public string OtherTeamMembersName { get { return mOtherTeamMembersName; } set { mOtherTeamMembersName = value; } }
        public string LifeCycleManagement { get { return mLifeCycleManagement; } set { mLifeCycleManagement = value; } }
        public string LifeCycleManagementName { get { return mLifeCycleManagementName; } set { mLifeCycleManagementName = value; } }
        public string PackagingProcurement { get { return mPackagingProcurement; } set { mPackagingProcurement = value; } }
        public string PackagingProcurementName { get { return mPackagingProcurementName; } set { mPackagingProcurementName = value; } }
        public string ExtManufacturingProc { get { return mExtManufacturingProc; } set { mExtManufacturingProc = value; } }
        public string ExtManufacturingProcName { get { return mExtManufacturingProcName; } set { mExtManufacturingProcName = value; } }
        public string Marketing { get { return mMarketing; } set { mMarketing = value; } }
        public string MarketingName { get { return mMarketingName; } set { mMarketingName = value; } }
        public string FlowthroughDets { get { return mFlowthroughDets; } set { mFlowthroughDets = value; } }
        public string WorkflowPhase { get { return mWorkflowPhase; } set { mWorkflowPhase = value; } }
        public string AllUsers { get { return mAllUsers; } set { mAllUsers = value; } }
        public string ProfitCenter { get { return mProfitCenter; } set { mProfitCenter = value; } }
        public string PLMFlag { get { return mPLMFlag; } set { mPLMFlag = value; } }
        public string FGReplacingAnExistingFG { get { return mFGReplacingAnExistingFG; } set { mFGReplacingAnExistingFG = value; } }
        public string IsThisAnLTOItem { get { return mIsThisAnLTOItem; } set { mIsThisAnLTOItem = value; } }
        public string RequestChangeToFGNumForSameUCC { get { return mRequestChangeToFGNumForSameUCC; } set { mRequestChangeToFGNumForSameUCC = value; } }
        public DateTime LTOTransitionStartWindowRDD { get { return mLTOTransitionStartWindowRDD; } set { mLTOTransitionStartWindowRDD = value; } }
        public DateTime LTOTransitionEndWindowRDD { get { return mLTOTransitionEndWindowRDD; } set { mLTOTransitionEndWindowRDD = value; } }
        public string LTOEndDateFlexibility { get { return mLTOEndDateFlexibility; } set { mLTOEndDateFlexibility = value; } }
        #endregion

        #endregion
    }
}
