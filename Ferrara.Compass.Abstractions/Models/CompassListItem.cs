using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Constants;

namespace Ferrara.Compass.Abstractions.Models
{
    public class CompassListItem
    {
        public CompassListItem()
        {
            TBDIndicator = GlobalConstants.TBDINDICATOR_Existing;
            mRetailUnitWieghtOz = -9999;
        }

        #region Variables
        private int mCompassListItemId;
        private string mCommercializationLink;
        private string mChangeLink;
        private string mCopyLink;
        private string mWorkflowStatusLink;
        private string mLastUpdatedFormName;

        #region IPF Variables
        private DateTime mSubmittedDate;
        private string mProjectNumber;
        private string mInitiator;
        private string  mProjectLeader;  
        private string  mProjectLeaderName;  
        private string mMarketing;
        private DateTime mFirstShipDate;
        private DateTime mRevisedFirstShipDate;
        private string mCustomer;
        private string mCaseType;
        private string mAnnualProjectedUnits;
        private string mAnnualProjectedDollars;
        private string mCustomerSpecific;
        private string mCustomerSpecificLotCode;
        private string mLikeFGItemNumber;
        private string mLikeFGItemDescription;
        private string mTruckLoadPricePerSellingUnit;
        private string mMarketClaimsLabelingRequirements;
        private string mCountryOfSale;
        private string mProjectType;
        private string mProjectTypeSubCategory;
        private string mTBDIndicator;
        private string mResearchDevelopmentLead;
        private string mLast12MonthSales;
        private string mSAPBaseUOM;
        private string mBaseUOMNetWeightLbs;
        private string mOrganic;
        private string mChannel;
        private string mSAPItemNumber;
        private string mSAPDescription;
        private string mProductHierarchyLevel1;
        private string mProductHierarchyLevel2;
        private string mProjectNotes;
        private string mItemConcept;
        private string mPM;
        private string mNewFormula;
        private string mDTVProject;
        private string mMfgLocationChange;
        private string mServingSizeWeightChange;
        private double mExpectedGrossMarginPercent;
        private double mRevisedGrossMarginPercent;
        private string mSoldOutsideUSA;
        private string mMaterialGroup1Brand;
        private string mMaterialGroup4ProductForm;
        private string mMaterialGroup5PackType;
        private string mTotalQuantityUnitsInDisplay;
        private string mFGItemNumberInDisplay;
        private string mFGItemDescription;
        private string mFGItemQuantity;
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
        private string mRetailSellingUnitsBaseUOM;
        private double mRetailUnitWieghtOz;
        private string mFilmSubstrate;
        private DateTime mExpectedPackagingSwitchDate;
        private string mPMName;
        private string mMarketingName;
        private string mRnDLeadName;
        private string mInitiatorName;

        private int mMonth1ProjectedUnits;
        private int mMonth2ProjectedUnits;
        private int mMonth3ProjectedUnits;
        private double mMonth1ProjectedDollars;
        private double mMonth2ProjectedDollars;
        private double mMonth3ProjectedDollars;
        private string mManufacturerCountryOfOrigin;

        private string mTestProject;
        #endregion

        #region OPS Variables
        private string mManufacturingLocation;
        private string mSecondaryManufacturingLocation;
        private string mPackingLocation;
        private string mDistributionCenter;
        //private string mSecondaryDistributionCenter;
        #endregion

        #region Trade Promo Group Variables

        private string mMaterialGroup2Pricing;

        #endregion

        #endregion

        #region Properties
        #region General Properties
        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        public string CommercializationLink { get { return mCommercializationLink; } set { mCommercializationLink = value; } }
        public string ChangeLink { get { return mChangeLink; } set { mChangeLink = value; } }
        public string CopyLink { get { return mCopyLink; } set { mCopyLink = value; } }
        public string WorkflowStatusLink { get { return mWorkflowStatusLink; } set { mWorkflowStatusLink = value; } }
        public string LastUpdatedFormName { get { return mLastUpdatedFormName; } set { mLastUpdatedFormName = value; } }
        #endregion

        #region IPF Properties
        public DateTime SubmittedDate { get { return mSubmittedDate; } set { mSubmittedDate = value; } }
        public string ProjectNumber { get { return mProjectNumber; } set { mProjectNumber = value; } }
        public string Initiator { get { return mInitiator; } set { mInitiator = value; } }
        public string Marketing { get { return mMarketing; } set { mMarketing = value; } }
        public string ProjectLeader { get { return mProjectLeader; } set { mProjectLeader = value; } }
        public string ProjectLeaderName { get { return mProjectLeaderName; } set { mProjectLeaderName = value; } }
        public DateTime FirstShipDate { get { return mFirstShipDate; } set { mFirstShipDate = value; } }
        public DateTime RevisedFirstShipDate { get { return mRevisedFirstShipDate; } set { mRevisedFirstShipDate = value; } }
        public string Customer { get { return mCustomer; } set { mCustomer = value; } }
        public string CaseType { get { return mCaseType; } set { mCaseType = value; } }
        public string AnnualProjectedUnits { get { return mAnnualProjectedUnits; } set { mAnnualProjectedUnits = value; } }
        public string AnnualProjectedDollars { get { return mAnnualProjectedDollars; } set { mAnnualProjectedDollars = value; } }
        public string CustomerSpecific { get { return mCustomerSpecific; } set { mCustomerSpecific = value; } }
        public string CustomerSpecificLotCode { get { return mCustomerSpecificLotCode; } set { mCustomerSpecificLotCode = value; } }
        public string LikeFGItemNumber { get { return mLikeFGItemNumber; } set { mLikeFGItemNumber = value; } }
        public string LikeFGItemDescription { get { return mLikeFGItemDescription; } set { mLikeFGItemDescription = value; } }
        public string TruckLoadPricePerSellingUnit { get { return mTruckLoadPricePerSellingUnit; } set { mTruckLoadPricePerSellingUnit = value; } }
        public string MarketClaimsLabelingRequirements { get { return mMarketClaimsLabelingRequirements; } set { mMarketClaimsLabelingRequirements = value; } }
        public string CountryOfSale { get { return mCountryOfSale; } set { mCountryOfSale = value; } }
        public string ProjectType { get { return mProjectType; } set { mProjectType = value; } }
        public string ProjectTypeSubCategory { get { return mProjectTypeSubCategory; } set { mProjectTypeSubCategory = value; } }
        public string TBDIndicator { get { return mTBDIndicator; } set { mTBDIndicator = value; } }
        public string ResearchDevelopmentLead { get { return mResearchDevelopmentLead; } set { mResearchDevelopmentLead = value; } }
        public string Last12MonthSales { get { return mLast12MonthSales; } set { mLast12MonthSales = value; } }
        public string SAPBaseUOM { get { return mSAPBaseUOM; } set { mSAPBaseUOM = value; } }
        public string BaseUOMNetWeightLbs { get { return mBaseUOMNetWeightLbs; } set { mBaseUOMNetWeightLbs = value; } }
        public string Organic { get { return mOrganic; } set { mOrganic = value; } }
        public string Channel { get { return mChannel; } set { mChannel = value; } }
        public string SAPItemNumber { get { return mSAPItemNumber; } set { mSAPItemNumber = value; } }
        public string SAPDescription { get { return mSAPDescription; } set { mSAPDescription = value; } }
        public string ProductHierarchyLevel1 { get { return mProductHierarchyLevel1; } set { mProductHierarchyLevel1 = value; } }
        public string ProductHierarchyLevel2 { get { return mProductHierarchyLevel2; } set { mProductHierarchyLevel2 = value; } }
        public string ProjectNotes { get { return mProjectNotes; } set { mProjectNotes = value; } }
        public string ItemConcept { get { return mItemConcept; } set { mItemConcept = value; } }
        public string PM { get { return mPM; } set { mPM = value; } }
        public string NewFormula { get { return mNewFormula; } set { mNewFormula = value; } }
        public string DTVProject { get { return mDTVProject; } set { mDTVProject = value; } }
        public string MfgLocationChange { get { return mMfgLocationChange; } set { mMfgLocationChange = value; } }
        public string ServingSizeWeightChange { get { return mServingSizeWeightChange; } set { mServingSizeWeightChange = value; } }
        public double ExpectedGrossMarginPercent { get { return mExpectedGrossMarginPercent; } set { mExpectedGrossMarginPercent = value; } }
        public double RevisedGrossMarginPercent { get { return mRevisedGrossMarginPercent; } set { mRevisedGrossMarginPercent = value; } }
        public string SoldOutsideUSA { get { return mSoldOutsideUSA; } set { mSoldOutsideUSA = value; } }
        public string MaterialGroup1Brand { get { return mMaterialGroup1Brand; } set { mMaterialGroup1Brand = value; } }
        public string MaterialGroup4ProductForm { get { return mMaterialGroup4ProductForm; } set { mMaterialGroup4ProductForm = value; } }
        public string MaterialGroup5PackType { get { return mMaterialGroup5PackType; } set { mMaterialGroup5PackType = value; } }
        public string TotalQuantityUnitsInDisplay { get { return mTotalQuantityUnitsInDisplay; } set { mTotalQuantityUnitsInDisplay = value; } }
        public string FGItemNumberInDisplay { get { return mFGItemNumberInDisplay; } set { mFGItemNumberInDisplay = value; } }
        public string FGItemDescription { get { return mFGItemDescription; } set { mFGItemDescription = value; } }
        public string FGItemQuantity { get { return mFGItemQuantity; } set { mFGItemQuantity = value; } }
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
        public string RetailSellingUnitsBaseUOM { get { return mRetailSellingUnitsBaseUOM; } set { mRetailSellingUnitsBaseUOM = value; } }
        public double RetailUnitWieghtOz { get { return mRetailUnitWieghtOz; } set { mRetailUnitWieghtOz = value; } }
        public string FilmSubstrate { get { return mFilmSubstrate; } set { mFilmSubstrate = value; } }
        public DateTime ExpectedPackagingSwitchDate { get { return mExpectedPackagingSwitchDate; } set { mExpectedPackagingSwitchDate = value; } }
        public string PMName { get { return mPMName; } set { mPMName = value; } }
        public string MarketingName { get { return mMarketingName; } set { mMarketingName = value; } }
        public string RnDLeadName { get { return mRnDLeadName; } set { mRnDLeadName = value; } }
        public string InitiatorName { get { return mInitiatorName; } set { mInitiatorName = value; } }
        public int Month1ProjectedUnits { get { return mMonth1ProjectedUnits; } set { mMonth1ProjectedUnits = value; } }
        public int Month2ProjectedUnits { get { return mMonth2ProjectedUnits; } set { mMonth2ProjectedUnits = value; } }
        public int Month3ProjectedUnits { get { return mMonth3ProjectedUnits; } set { mMonth3ProjectedUnits = value; } }
        public double Month1ProjectedDollars { get { return mMonth1ProjectedDollars; } set { mMonth1ProjectedDollars = value; } }
        public double Month2ProjectedDollars { get { return mMonth2ProjectedDollars; } set { mMonth2ProjectedDollars = value; } }
        public double Month3ProjectedDollars { get { return mMonth3ProjectedDollars; } set { mMonth3ProjectedDollars = value; } }
        public string ManufacturerCountryOfOrigin { get { return mManufacturerCountryOfOrigin; } set { mManufacturerCountryOfOrigin = value; } }
        public string TestProject { get { return mTestProject; } set { mTestProject = value; } }

        #endregion

        #region OPS Properties
        public string ManufacturingLocation { get { return mManufacturingLocation; } set { mManufacturingLocation = value; } }
        public string SecondaryManufacturingLocation { get { return mSecondaryManufacturingLocation; } set { mSecondaryManufacturingLocation = value; } }
        public string PackingLocation { get { return mPackingLocation; } set { mPackingLocation = value; } }
        public string DistributionCenter { get { return mDistributionCenter; } set { mDistributionCenter = value; } }
        #endregion

        #region Trade Promo Group Properties
        public string MaterialGroup2Pricing { get { return mMaterialGroup2Pricing; } set { mMaterialGroup2Pricing = value; } }
        #endregion


        #endregion



        //OLD STUFF
        //**********************************************************************
        #region Variables
        #region General Variables
        private DateTime mNLEAPosted;
        private string mPackagingEngineerUser;
        private string mGraphicsLeadUser;
        private string mPackagingNumbers;
        private string mActionGroup;
        private List<FileAttribute> fileAttachments = new List<FileAttribute>();
        private List<string> emailAddresses = new List<string>();
        #endregion


        #region OPS Variables

        private string mProcurementType;
        private string mPlantLine;
        private string mPurchasedIntoCenter;

        #endregion

        #region Packaging Engineering Variables

        private string mGrossCaseWeight;
        private string mCaseCube;

        #endregion


        #region SAP Semi Request Variables
        private string mSemiDescription;
        #endregion

        #region MaterialNumber Variables

        private List<PackagingItem> packingItem = new List<PackagingItem>();

        #endregion

        #region Graphics Variables

        #endregion

        #region Final Costing Variables

        #endregion

        #region Workflow Variables

        private Ferrara.Compass.Abstractions.Enum.WorkflowStep mWorkflowStep;
        private string mWorkflowPreviousUrl;
        private string mWorkflowCurrentUrl;
        private string mWorkflowCommercializationUrl;
        private string mWorkflowTBDUrl;
        #endregion

        #region COMAN Variables
        private string mFCCMakeCapacity;
        private string mFCCPackCapacity;
        private string mComanNotes;
        private string mTrademarkIssues;
        private string mCoBranded;
        private string mCoBrandedNotes;
        private string mAlternativeCaseTypes;
        private string mShippingReq;
        private string mSpecialTransReq;
        private string mRemainingDaysUponDelivery;
        private string mCustomerExtShelfLife;
        private string mFCCSpecificationOwned;
        private string mConfidentialityAgreement;
        private string mBOMDetailedRecipe;
        private string mSupplierApprovalProcess;
        private string mQAApprovalProcess;
        private string mSiteAuditProcess;
        private string mSupplierMakeCapacity;
        private string mSupplierPackCapacity;
        private string mProcurementNotes;
        private string mCurrentOrNew;
        private string mBusinessThreshold;
        private string mIngredientPackagingComponents;

        #endregion

        #region Report Variables
        private string mCriticalInitiative = string.Empty;
        #endregion

        #endregion

        #region Properties
        #region General Properties

        public List<FileAttribute> FileAttachments { get { return fileAttachments; } set { fileAttachments = value; } }

        public List<string> EmailAddresses { get { return emailAddresses; } set { emailAddresses = value; } }

        public DateTime OBM_NLEAPosted { get { return mNLEAPosted; } set { mNLEAPosted = value; } }

        public string OBM_PackagingEngineerUser { get { return mPackagingEngineerUser; } set { mPackagingEngineerUser = value; } }

        public string OBM_GraphicsLeadUser { get { return mGraphicsLeadUser; } set { mGraphicsLeadUser = value; } }

        public string OBM_PackagingNumbers { get { return mPackagingNumbers; } set { mPackagingNumbers = value; } }

        public string OBM_ActionGroup { get { return mActionGroup; } set { mActionGroup = value; } }
        #endregion


        #region OPS Properties

        public string OPS_ProcurementType { get { return mProcurementType; } set { mProcurementType = value; } }

        public string OPS_PlantLine { get { return mPlantLine; } set { mPlantLine = value; } }

        public string OPS_PurchasedIntoCenter { get { return mPurchasedIntoCenter; } set { mPurchasedIntoCenter = value; } }

        #endregion


        #region Packaging Engineering Properties

        public string PE_GrossCaseWeight { get { return mGrossCaseWeight; } set { mGrossCaseWeight = value; } }

        public string PE_CaseCube { get { return mCaseCube; } set { mCaseCube = value; } }

        #endregion

        #region SAP Semi Request

        public string SSR_SemiDescription { get { return mSemiDescription; } set { mSemiDescription = value; } }

        #endregion

        #region Material Number Properties

        public List<PackagingItem> MatNum_PackingItem { get { return packingItem; } set { packingItem = value; } }

        #endregion


        #region Workflow Properties

        public Ferrara.Compass.Abstractions.Enum.WorkflowStep WorkflowStep { get { return mWorkflowStep; } set { mWorkflowStep = value; } }

        public string WorkflowPreviousUrl { get { return mWorkflowPreviousUrl; } set { mWorkflowPreviousUrl = value; } }

        public string WorkflowCurrentUrl { get { return mWorkflowCurrentUrl; } set { mWorkflowCurrentUrl = value; } }

        public string WorkflowCommercializationUrl { get { return mWorkflowCommercializationUrl; } set { mWorkflowCommercializationUrl = value; } }

        public string WorkflowTBDUrl { get { return mWorkflowTBDUrl; } set { mWorkflowTBDUrl = value; } }

        #endregion

        #region COMAN Properties

        public string COMAN_FCCMakeCapacity { get { return mFCCMakeCapacity; } set { mFCCMakeCapacity = value; } }
        public string COMAN_FCCPackCapacity { get { return mFCCPackCapacity; } set { mFCCPackCapacity = value; } }
        public string COMAN_ComanNotes { get { return mComanNotes; } set { mComanNotes = value; } }
        public string COMAN_TrademarkIssues { get { return mTrademarkIssues; } set { mTrademarkIssues = value; } }
        public string COMAN_CoBranded { get { return mCoBranded; } set { mCoBranded = value; } }
        public string COMAN_CoBrandedNotes { get { return mCoBrandedNotes; } set { mCoBrandedNotes = value; } }
        public string COMAN_AlternativeCaseTypes { get { return mAlternativeCaseTypes; } set { mAlternativeCaseTypes = value; } }
        public string COMAN_ShippingReq { get { return mShippingReq; } set { mShippingReq = value; } }
        public string COMAN_SpecialTransReq { get { return mSpecialTransReq; } set { mSpecialTransReq = value; } }
        public string COMAN_RemainingDaysUponDelivery { get { return mRemainingDaysUponDelivery; } set { mRemainingDaysUponDelivery = value; } }
        public string COMAN_CustomerExtShelfLife { get { return mCustomerExtShelfLife; } set { mCustomerExtShelfLife = value; } }
        public string COMAN_FCCSpecificationOwned { get { return mFCCSpecificationOwned; } set { mFCCSpecificationOwned = value; } }
        public string COMAN_ConfidentialityAgreement { get { return mConfidentialityAgreement; } set { mConfidentialityAgreement = value; } }
        public string COMAN_BOMDetailedRecipe { get { return mBOMDetailedRecipe; } set { mBOMDetailedRecipe = value; } }
        public string COMAN_SupplierApprovalProcess { get { return mSupplierApprovalProcess; } set { mSupplierApprovalProcess = value; } }
        public string COMAN_QAApprovalProcess { get { return mQAApprovalProcess; } set { mQAApprovalProcess = value; } }
        public string COMAN_SiteAuditProcess { get { return mSiteAuditProcess; } set { mSiteAuditProcess = value; } }
        public string COMAN_SupplierMakeCapacity { get { return mSupplierMakeCapacity; } set { mSupplierMakeCapacity = value; } }
        public string COMAN_SupplierPackCapacity { get { return mSupplierPackCapacity; } set { mSupplierPackCapacity = value; } }
        public string COMAN_ProcurementNotes { get { return mProcurementNotes; } set { mProcurementNotes = value; } }
        public string COMAN_CurrentOrNew { get { return mCurrentOrNew; } set { mCurrentOrNew = value; } }
        public string COMAN_BusinessThreshold { get { return mBusinessThreshold; } set { mBusinessThreshold = value; } }
        public string COMAN_IngredientPackagingComponents { get { return mIngredientPackagingComponents; } set { mIngredientPackagingComponents = value; } }
        #endregion

        #region Report Properties
        public string REPORT_CriticalInitiative { get { return mCriticalInitiative; } set { mCriticalInitiative = value; } }
        #endregion

        #endregion
    }
}
