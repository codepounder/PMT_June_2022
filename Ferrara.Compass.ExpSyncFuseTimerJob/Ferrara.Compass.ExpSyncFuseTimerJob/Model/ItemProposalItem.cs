using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferrara.Compass.ExpSyncFuseTimerJob.Models
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
        private string mCommercializationLink;
        private string mChangeLink;
        private string mCopyLink;
        private string mWorkflowStatusLink;
        private string mLastUpdatedFormName;

        #region IPF Variables
        private string mItemDescription;
        private DateTime mSubmittedDate;
        private string mProjectNumber;
        private string mInitiator;
        private string mBrandManager;
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
        private double mTruckLoadPricePerSellingUnit;
        private string mCandySemiNumber;
        private string mMarketClaimsLabelingRequirements;
        private string mCountryOfSale;
        private string mProjectType;
        private string mTBDIndicator;
        private string mResearchDevelopmentLead;
        private double mLast12MonthSales;
        private string mSAPBaseUOM;
        private double mBaseUOMNetWeightLbs;
        private string mOrganic;
        private string mChannel;
        private string mSAPItemNumber;
        private string mSAPDescription;
        private string mProductHierarchyLevel1;
        private string mProductHierarchyLevel2;
        private string mProjectNotes;
        private string mItemConcept;
        private string mOBM;
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
        private int mRetailSellingUnitsBaseUOM;
        private double mRetailUnitWieghtOz;
        private string mFilmSubstrate;
        private string mPegHoleNeeded;
        private DateTime mExpectedPackagingSwitchDate;
        private string mReplacementForItemNumber;
        private string mReasonForChange;
        private string mInitiatorName;
        private string mBrandManagerName;
        private string mResearchDevelopmentLeadName;
        private string mOBMName;
        private string mGraphicsRequired;
        private string mNewComponentRequired;
        private string mNewTransferSemiRequired;
        private string mProjectTypeSubCategory;
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
        public string ItemDescription { get { return mItemDescription; } set { mItemDescription = value; } }
        public DateTime SubmittedDate { get { return mSubmittedDate; } set { mSubmittedDate = value; } }
        public string ProjectNumber { get { return mProjectNumber; } set { mProjectNumber = value; } }
        public string Initiator { get { return mInitiator; } set { mInitiator = value; } }
        public string BrandManager { get { return mBrandManager; } set { mBrandManager = value; } }
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
        public double TruckLoadPricePerSellingUnit { get { return mTruckLoadPricePerSellingUnit; } set { mTruckLoadPricePerSellingUnit = value; } }
        public string CandySemiNumber { get { return mCandySemiNumber; } set { mCandySemiNumber = value; } }
        public string MarketClaimsLabelingRequirements { get { return mMarketClaimsLabelingRequirements; } set { mMarketClaimsLabelingRequirements = value; } }
        public string CountryOfSale { get { return mCountryOfSale; } set { mCountryOfSale = value; } }
        public string ProjectType { get { return mProjectType; } set { mProjectType = value; } }
        public string TBDIndicator { get { return mTBDIndicator; } set { mTBDIndicator = value; } }
        public string ResearchDevelopmentLead { get { return mResearchDevelopmentLead; } set { mResearchDevelopmentLead = value; } }
        public double Last12MonthSales { get { return mLast12MonthSales; } set { mLast12MonthSales = value; } }
        public string SAPBaseUOM { get { return mSAPBaseUOM; } set { mSAPBaseUOM = value; } }
        public double BaseUOMNetWeightLbs { get { return mBaseUOMNetWeightLbs; } set { mBaseUOMNetWeightLbs = value; } }
        public string Organic { get { return mOrganic; } set { mOrganic = value; } }
        public string Channel { get { return mChannel; } set { mChannel = value; } }
        public string SAPItemNumber { get { return mSAPItemNumber; } set { mSAPItemNumber = value; } }
        public string SAPDescription { get { return mSAPDescription; } set { mSAPDescription = value; } }
        public string ProductHierarchyLevel1 { get { return mProductHierarchyLevel1; } set { mProductHierarchyLevel1 = value; } }
        public string ProductHierarchyLevel2 { get { return mProductHierarchyLevel2; } set { mProductHierarchyLevel2 = value; } }
        public string ProjectNotes { get { return mProjectNotes; } set { mProjectNotes = value; } }
        public string ItemConcept { get { return mItemConcept; } set { mItemConcept = value; } }
        public string OBM { get { return mOBM; } set { mOBM = value; } }
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
        public int RetailSellingUnitsBaseUOM { get { return mRetailSellingUnitsBaseUOM; } set { mRetailSellingUnitsBaseUOM = value; } }
        public double RetailUnitWieghtOz { get { return mRetailUnitWieghtOz; } set { mRetailUnitWieghtOz = value; } }
        public string FilmSubstrate { get { return mFilmSubstrate; } set { mFilmSubstrate = value; } }
        public string PegHoleNeeded { get { return mPegHoleNeeded; } set { mPegHoleNeeded = value; } }
        public DateTime ExpectedPackagingSwitchDate { get { return mExpectedPackagingSwitchDate; } set { mExpectedPackagingSwitchDate = value; } }
        public string ReplacementForItemNumber { get { return mReplacementForItemNumber; } set { mReplacementForItemNumber = value; } }
        public string ReasonForChange { get { return mReasonForChange; } set { mReasonForChange = value; } }
        public string InitiatorName { get { return mInitiatorName; } set { mInitiatorName = value; } }
        public string BrandManagerName { get { return mBrandManagerName; } set { mBrandManagerName = value; } }
        public string ResearchDevelopmentLeadName { get { return mResearchDevelopmentLeadName; } set { mResearchDevelopmentLeadName = value; } }
        public string OBMName { get { return mOBMName; } set { mOBMName = value; } }
        public string NewTransferSemiRequired { get { return mNewTransferSemiRequired; } set { mNewTransferSemiRequired = value; } }
        public string GraphicsRequired { get { return mGraphicsRequired; } set { mGraphicsRequired = value; } }
        public string NewComponentRequired { get { return mNewComponentRequired; } set { mNewComponentRequired = value; } }
        public string ProjectTypeSubCategory { get { return mProjectTypeSubCategory; } set { mProjectTypeSubCategory = value; } }
        #endregion

        #endregion
    }
}
