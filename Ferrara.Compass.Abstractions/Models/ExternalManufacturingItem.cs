using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class ExternalManufacturingItem
    {
        #region Variables
        private int mCompassListItemId;
        private string mLastUpdatedFormName;

        // IPF Fields
        private string mProjectType;
        private DateTime mRevisedFirstShipDate;
        private int mAnnualProjectedUnits;
        private string mSAPBaseUOM;
        private int mRetailSellingUnitsBaseUOM;
        private double mRetailUnitWieghtOz;
        private double mTruckLoadPricePerSellingUnit;

        private string mExternalMfgProjectLead;
        private string mCoManufacturingClassification;
        private string mDoesBulkSemiExistToBringInHouse;
        private string mExistingBulkSemiNumber;
        private string mBulkSemiDescription;
        private string mExternalManufacturer;
        private string mDoesSupplierHaveMakeCapacity;
        private string mManufacturerCountryOfOrigin;
        private string mExternalPacker;
        private string mDoesSupplierHavePackCapacity;
        private string mPurchasedIntoLocation;
        private string mCurrentTimelineAcceptable;
        private string mLeadTimeFromSupplier;
        private DateTime mFinalArtworkDueToSupplier;
        private double mExpectedGrossMarginPercent;
        private string mMakeLocation;
        private string mPackingLocation;
        private int mMonth1ProjectedUnits;
        private int mMonth2ProjectedUnits;
        private int mMonth3ProjectedUnits;
        private int mMonth1ProjectedCases;
        private int mMonth2ProjectedCases;
        private int mMonth3ProjectedCases;
        private int mMonth1Projectedlbs;
        private int mMonth2Projectedlbs;
        private int mMonth3Projectedlbs;
        private decimal mCaseCostTarget;
        private decimal mLbsCostTarget;
        private string mInitialCosting_GrossMarginAccurate;
        private double mRevisedGrossMarginPercent;
        private string mMaterialNumber;
        private string mMaterialDescriptiom;
        private string mNoveltyProject;
        private string mPHL1;
        private string mPLMFlag;
        private string mDesignateHUBDC;
        private string mPackSupplierAndDielineSame;
        private string mWhatChangeIsRequiredExtMfg;
        private string mItemConcept;
        #endregion

        #region Properties




        public string ProjectType { get { return mProjectType; } set { mProjectType = value; } }
        public string MakeLocation { get { return mMakeLocation; } set { mMakeLocation = value; } }
        public string MfgLocationChange { get { return mMakeLocation; } set { mMakeLocation = value; } }
        public string PackingLocation { get { return mPackingLocation; } set { mPackingLocation = value; } }
        public int Month1ProjectedUnits { get { return mMonth1ProjectedUnits; } set { mMonth1ProjectedUnits = value; } }
        public int Month2ProjectedUnits { get { return mMonth2ProjectedUnits; } set { mMonth2ProjectedUnits = value; } }
        public int Month3ProjectedUnits { get { return mMonth3ProjectedUnits; } set { mMonth3ProjectedUnits = value; } }
        public int Month1ProjectedCases { get { return mMonth1ProjectedCases; } set { mMonth1ProjectedCases = value; } }
        public int Month2ProjectedCases { get { return mMonth2ProjectedCases; } set { mMonth2ProjectedCases = value; } }
        public int Month3ProjectedCases { get { return mMonth3ProjectedCases; } set { mMonth3ProjectedCases = value; } }
        public int Month1Projectedlbs { get { return mMonth1Projectedlbs; } set { mMonth1Projectedlbs = value; } }
        public int Month2Projectedlbs { get { return mMonth2Projectedlbs; } set { mMonth2Projectedlbs = value; } }
        public int Month3Projectedlbs { get { return mMonth3Projectedlbs; } set { mMonth3Projectedlbs = value; } }
        public decimal CaseCostTarget { get { return mCaseCostTarget; } set { mCaseCostTarget = value; } }
        public decimal LbsCostTarget { get { return mLbsCostTarget; } set { mLbsCostTarget = value; } }


        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        public string LastUpdatedFormName { get { return mLastUpdatedFormName; } set { mLastUpdatedFormName = value; } }

        public DateTime RevisedFirstShipDate { get { return mRevisedFirstShipDate; } set { mRevisedFirstShipDate = value; } }
        public int AnnualProjectedUnits { get { return mAnnualProjectedUnits; } set { mAnnualProjectedUnits = value; } }
        public string SAPBaseUOM { get { return mSAPBaseUOM; } set { mSAPBaseUOM = value; } }
        public int RetailSellingUnitsBaseUOM { get { return mRetailSellingUnitsBaseUOM; } set { mRetailSellingUnitsBaseUOM = value; } }
        public double RetailUnitWieghtOz { get { return mRetailUnitWieghtOz; } set { mRetailUnitWieghtOz = value; } }
        public double TruckLoadPricePerSellingUnit { get { return mTruckLoadPricePerSellingUnit; } set { mTruckLoadPricePerSellingUnit = value; } }
        public double ExpectedGrossMarginPercent { get { return mExpectedGrossMarginPercent; } set { mExpectedGrossMarginPercent = value; } }
        public double RevisedGrossMarginPercent { get { return mRevisedGrossMarginPercent; } set { mRevisedGrossMarginPercent = value; } }
        public string ExternalMfgProjectLead { get { return mExternalMfgProjectLead; } set { mExternalMfgProjectLead = value; } }
        public string CoManufacturingClassification { get { return mCoManufacturingClassification; } set { mCoManufacturingClassification = value; } }
        public string DoesBulkSemiExistToBringInHouse { get { return mDoesBulkSemiExistToBringInHouse; } set { mDoesBulkSemiExistToBringInHouse = value; } }
        public string ExistingBulkSemiNumber { get { return mExistingBulkSemiNumber; } set { mExistingBulkSemiNumber = value; } }
        public string BulkSemiDescription { get { return mBulkSemiDescription; } set { mBulkSemiDescription = value; } }
        public string ExternalManufacturer { get { return mExternalManufacturer; } set { mExternalManufacturer = value; } }
        public string DoesSupplierHaveMakeCapacity { get { return mDoesSupplierHaveMakeCapacity; } set { mDoesSupplierHaveMakeCapacity = value; } }
        public string ManufacturerCountryOfOrigin { get { return mManufacturerCountryOfOrigin; } set { mManufacturerCountryOfOrigin = value; } }
        public string ExternalPacker { get { return mExternalPacker; } set { mExternalPacker = value; } }
        public string DoesSupplierHavePackCapacity { get { return mDoesSupplierHavePackCapacity; } set { mDoesSupplierHavePackCapacity = value; } }
        public string PurchasedIntoLocation { get { return mPurchasedIntoLocation; } set { mPurchasedIntoLocation = value; } }
        public string CurrentTimelineAcceptable { get { return mCurrentTimelineAcceptable; } set { mCurrentTimelineAcceptable = value; } }
        public string LeadTimeFromSupplier { get { return mLeadTimeFromSupplier; } set { mLeadTimeFromSupplier = value; } }
        public DateTime FinalArtworkDueToSupplier { get { return mFinalArtworkDueToSupplier; } set { mFinalArtworkDueToSupplier = value; } }
        public string InitialCosting_GrossMarginAccurate { get { return mInitialCosting_GrossMarginAccurate; } set { mInitialCosting_GrossMarginAccurate = value; } }
        public string MaterialNumber { get { return mMaterialNumber; } set { mMaterialNumber = value; } }
        public string MaterialDescriptiom { get { return mMaterialDescriptiom; } set { mMaterialDescriptiom = value; } }
        public string NoveltyProject { get { return mNoveltyProject; } set { mNoveltyProject = value; } }
        public string PHL1 { get { return mPHL1; } set { mPHL1 = value; } }
        public string PLMFlag { get { return mPLMFlag; } set { mPLMFlag = value; } }
        public string DesignateHUBDC { get { return mDesignateHUBDC; } set { mDesignateHUBDC = value; } }
        public string PackSupplierAndDielineSame { get { return mPackSupplierAndDielineSame; } set { mPackSupplierAndDielineSame = value; } }
        public string WhatChangeIsRequiredExtMfg { get { return mWhatChangeIsRequiredExtMfg; } set { mWhatChangeIsRequiredExtMfg = value; } }
        public string ItemConcept { get { return mItemConcept; } set { mItemConcept = value; } }
        #endregion
    }
}
