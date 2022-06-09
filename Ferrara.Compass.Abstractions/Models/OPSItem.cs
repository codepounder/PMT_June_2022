using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferrara.Compass.Abstractions.Models
{
    public class OPSItem
    {
        #region Member Variables
        private int mCompassListItemId;
        private string mFGItemNumber;
        private string mFGItemDescription;
        private string mSAPBaseUOM;
        private string mPackType;
        private string mSimilarUnitWeight;
        private string mCandySemi;
        private string mCommentsfromMarketing;
        private string mMakeLocation;
        private string mCountryOfOrigin;
        private string mPackingLocation;
        private string mExternalManufacturer;
        private string mExternalPacker;
        private string mInternalTransferSemiNeeded;
        private string mPurchasedSemiNeeded;
        private string mLikeItem;
        private string mLikeItemDescription;

        private string mWorkCenterAddInfo;

        private string mProjectType;
        private string mMaterialGroup4ProductForm;
        private string mMaterialGroup5PackType;
        private string mMfgLocationChange;
        private string mImmediateSPKChange;
        private int mRetailSellingUnitsBaseUOM;
        private string mCoManClassification;

        #endregion

        #region Initial Capacity Variables
        private string mLastUpdatedFormName;

        private string mInitialCapacity_CapacityRiskComments;
        private string mInitialCapacity_Decision;
        private string mInitialCapacity_MakeIssues;
        private string mInitialCapacity_PackIssues;
        private string mInitialCapacity_AcceptanceComments;
        private string mSrOBMApproval_CapacityReviewComments;

        private string mSAPItemNumber;
        private string mManufacturingLocation;
        private int mAnnualProjectedUnits;

        private double mRetailUnitWieghtOz;

        private int mMonth1ProjectedUnits;
        private int mMonth2ProjectedUnits;
        private int mMonth3ProjectedUnits;
        private int mMonth1ProjectedCases;
        private int mMonth2ProjectedCases;
        private int mMonth3ProjectedCases;
        private int mMonth1Projectedlbs;
        private int mMonth2Projectedlbs;
        private int mMonth3Projectedlbs;
        private DateTime mFirstProductionDate;
        private DateTime mRevisedFirstShipDate;
        private string mLineOfBusiness;
        private string mWhatNetworkMoveIsRequired;
        private string mProjectApproved;
        private string mReasonForRejection;
        #endregion

        #region Properties
        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        public string FGItemNumber { get { return mFGItemNumber; } set { mFGItemNumber = value; } }
        public string FGItemDescription { get { return mFGItemDescription; } set { mFGItemDescription = value; } }
        public string SAPBaseUOM { get { return mSAPBaseUOM; } set { mSAPBaseUOM = value; } }
        public string PackType { get { return mPackType; } set { mPackType = value; } }
        public string SimilarUnitWeight { get { return mSimilarUnitWeight; } set { mSimilarUnitWeight = value; } }
        public string CandySemi { get { return mCandySemi; } set { mCandySemi = value; } }
        public string CommentsfromMarketing { get { return mCommentsfromMarketing; } set { mCommentsfromMarketing = value; } }
        public string MakeLocation { get { return mMakeLocation; } set { mMakeLocation = value; } }
        public string PackingLocation { get { return mPackingLocation; } set { mPackingLocation = value; } }
        public string ExternalManufacturer { get { return mExternalManufacturer; } set { mExternalManufacturer = value; } }
        public string ExternalPacker { get { return mExternalPacker; } set { mExternalPacker = value; } }
        public string InternalTransferSemiNeeded { get { return mInternalTransferSemiNeeded; } set { mInternalTransferSemiNeeded = value; } }
        public string PurchasedSemiNeeded { get { return mPurchasedSemiNeeded; } set { mPurchasedSemiNeeded = value; } }
        public string LikeItemDescription { get { return mLikeItemDescription; } set { mLikeItemDescription = value; } }
        public string LikeItem { get { return mLikeItem; } set { mLikeItem = value; } }
        public string CountryOfOrigin { get { return mCountryOfOrigin; } set { mCountryOfOrigin = value; } }
        public string ProjectType { get { return mProjectType; } set { mProjectType = value; } }

        public string WorkCenterAddInfo { get { return mWorkCenterAddInfo; } set { mWorkCenterAddInfo = value; } }
        public string MaterialGroup4ProductForm { get { return mMaterialGroup4ProductForm; } set { mMaterialGroup4ProductForm = value; } }
        public string MaterialGroup5PackType { get { return mMaterialGroup5PackType; } set { mMaterialGroup5PackType = value; } }
        public string MfgLocationChange { get { return mMfgLocationChange; } set { mMfgLocationChange = value; } }
        public string ImmediateSPKChange { get { return mImmediateSPKChange; } set { mImmediateSPKChange = value; } }
        public int RetailSellingUnitsBaseUOM { get { return mRetailSellingUnitsBaseUOM; } set { mRetailSellingUnitsBaseUOM = value; } }
        public string CoManClassification { get { return mCoManClassification; } set { mCoManClassification = value; } }

        #endregion

        #region Initial Capacity Properties       
        public string LastUpdatedFormName { get { return mLastUpdatedFormName; } set { mLastUpdatedFormName = value; } }
        #endregion

        public DateTime FirstProductionDate { get { return mFirstProductionDate; } set { mFirstProductionDate = value; } }
        public DateTime RevisedFirstShipDate { get { return mRevisedFirstShipDate; } set { mRevisedFirstShipDate = value; } }
        public int Month1ProjectedUnits { get { return mMonth1ProjectedUnits; } set { mMonth1ProjectedUnits = value; } }
        public int Month2ProjectedUnits { get { return mMonth2ProjectedUnits; } set { mMonth2ProjectedUnits = value; } }
        public int Month3ProjectedUnits { get { return mMonth3ProjectedUnits; } set { mMonth3ProjectedUnits = value; } }
        public int Month1ProjectedCases { get { return mMonth1ProjectedCases; } set { mMonth1ProjectedCases = value; } }
        public int Month2ProjectedCases { get { return mMonth2ProjectedCases; } set { mMonth2ProjectedCases = value; } }
        public int Month3ProjectedCases { get { return mMonth3ProjectedCases; } set { mMonth3ProjectedCases = value; } }
        public int Month1Projectedlbs { get { return mMonth1Projectedlbs; } set { mMonth1Projectedlbs = value; } }
        public int Month2Projectedlbs { get { return mMonth2Projectedlbs; } set { mMonth2Projectedlbs = value; } }
        public int Month3Projectedlbs { get { return mMonth3Projectedlbs; } set { mMonth3Projectedlbs = value; } }

        public string InitialCapacity_Decision { get { return mInitialCapacity_Decision; } set { mInitialCapacity_Decision = value; } }
        public string InitialCapacity_CapacityRiskComments { get { return mInitialCapacity_CapacityRiskComments; } set { mInitialCapacity_CapacityRiskComments = value; } }
        public string InitialCapacity_AcceptanceComments { get { return mInitialCapacity_AcceptanceComments; } set { mInitialCapacity_AcceptanceComments = value; } }
        public string InitialCapacity_MakeIssues { get { return mInitialCapacity_MakeIssues; } set { mInitialCapacity_MakeIssues = value; } }
        public string InitialCapacity_PackIssues { get { return mInitialCapacity_PackIssues; } set { mInitialCapacity_PackIssues = value; } }
        public string SrOBMApproval_CapacityReviewComments { get { return mSrOBMApproval_CapacityReviewComments; } set { mSrOBMApproval_CapacityReviewComments = value; } }

        public string SAPItemNumber { get { return mSAPItemNumber; } set { mSAPItemNumber = value; } }
        public string ManufacturingLocation { get { return mManufacturingLocation; } set { mManufacturingLocation = value; } }
        public int AnnualProjectedUnits { get { return mAnnualProjectedUnits; } set { mAnnualProjectedUnits = value; } }
        public double RetailUnitWieghtOz { get { return mRetailUnitWieghtOz; } set { mRetailUnitWieghtOz = value; } }
        public string LineOfBusiness { get { return mLineOfBusiness; } set { mLineOfBusiness = value; } }
        public string WhatNetworkMoveIsRequired { get { return mWhatNetworkMoveIsRequired; } set { mWhatNetworkMoveIsRequired = value; } }
        public string ProjectApproved { get { return mProjectApproved; } set { mProjectApproved = value; } }
        public string ReasonForRejection { get { return mReasonForRejection; } set { mReasonForRejection = value; } }
    }
}

