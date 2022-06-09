using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferrara.Compass.Abstractions.Models
{
    public class InnovationListItem
    {
        #region Variables

        private int mInnovationListItemId;
        private DateTime mTargetLaunchTimingDate;
        private string mProductType;
        private string mBrand;
        private string mPM;
        private string mBrandManager;
        private string mSeasonal;
        private string mCoManDevPartner;
        private string mStrategicRationale;
        private string mCustomerComm;
        private string mCustomerExplanation;
        private string mMarketType;
        private double mAnnualMarketGrowthPercent;
        private double mMarketSize;
        private string mMarketDataSource;
        private double mMarketCapture;
        private string mTargetFCCMargin;
        private string mCompetitiveProduct;
        private string mCompetitiveProduct2;
        private string mCompetitiveProduct3;
        private string mRnDNotes;
        private DateTime mSalesSamplesNeededBy;
        //RnD Fields
        private string mProjectType;
        private string mProjectCategory;
        private string mDegreeOfDifficulty;
        private string mRnDResource;
        private string mExpectedTimeRequired;
        private string mOpsConsiderations;
        // Review Fields
        private string mCriticalInitiative;
        // Link Fields
        private string mInnovationLink;
        private string mRnDLink;
        private string mReviewLink;

        private List<FileAttribute> mFileAttachments = new List<FileAttribute>();
        #endregion

        #region Properties

        public int InnovationListItemId { get { return mInnovationListItemId; } set { mInnovationListItemId = value; } }
        public DateTime TargetLaunchTimingDate { get { return mTargetLaunchTimingDate; } set { mTargetLaunchTimingDate = value; } }
        public string ProductType { get { return mProductType; } set { mProductType = value; } }
        public string Brand { get { return mBrand; } set { mBrand = value; } }
        public string PM { get { return mPM; } set { mPM = value; } }
        public string BrandManager { get { return mBrandManager; } set { mBrandManager = value; } }
        public string Seasonal { get { return mSeasonal; } set { mSeasonal = value; } }
        public string CoManDevPartner { get { return mCoManDevPartner; } set { mCoManDevPartner = value; } }
        public string StrategicRationale { get { return mStrategicRationale; } set { mStrategicRationale = value; } }
        public string CustomerComm { get { return mCustomerComm; } set { mCustomerComm = value; } }
        public string CustomerExplanation { get { return mCustomerExplanation; } set { mCustomerExplanation = value; } }
        public string MarketType { get { return mMarketType; } set { mMarketType = value; } }
        public double AnnualMarketGrowthPercent { get { return mAnnualMarketGrowthPercent; } set { mAnnualMarketGrowthPercent = value; } }
        public double MarketSize { get { return mMarketSize; } set { mMarketSize = value; } }
        public string MarketDataSource { get { return mMarketDataSource; } set { mMarketDataSource = value; } }
        public double MarketCapture { get { return mMarketCapture; } set { mMarketCapture = value; } }
        public string TargetFCCMargin { get { return mTargetFCCMargin; } set { mTargetFCCMargin = value; } }
        public string CompetitiveProduct { get { return mCompetitiveProduct; } set { mCompetitiveProduct = value; } }
        public string CompetitiveProduct2 { get { return mCompetitiveProduct2; } set { mCompetitiveProduct2 = value; } }
        public string CompetitiveProduct3 { get { return mCompetitiveProduct3; } set { mCompetitiveProduct3 = value; } }
        public string RnDNotes { get { return mRnDNotes; } set { mRnDNotes = value; } }
        public DateTime SalesSamplesNeededBy { get { return mSalesSamplesNeededBy; } set { mSalesSamplesNeededBy = value; } }
        //RnD Fields
        public string ProjectType { get { return mProjectType; } set { mProjectType = value; } }
        public string ProjectCategory { get { return mProjectCategory; } set { mProjectCategory = value; } }
        public string DegreeOfDifficulty { get { return mDegreeOfDifficulty; } set { mDegreeOfDifficulty = value; } }
        public string RnDResource { get { return mRnDResource; } set { mRnDResource = value; } }
        public string ExpectedTimeRequired { get { return mExpectedTimeRequired; } set { mExpectedTimeRequired = value; } }
        public string OpsConsiderations { get { return mOpsConsiderations; } set { mOpsConsiderations = value; } }
        // Review Fields
        public string CriticalInitiative { get { return mCriticalInitiative; } set { mCriticalInitiative = value; } }
        // Link Fields
        public string InnovationLink { get { return mInnovationLink; } set { mInnovationLink = value; } }
        public string RnDLink { get { return mRnDLink; } set { mRnDLink = value; } }
        public string ReviewLink { get { return mReviewLink; } set { mReviewLink = value; } }

        public List<FileAttribute> FileAttachments { get { return mFileAttachments; } set { mFileAttachments = value; } }

        #endregion
    }
}
