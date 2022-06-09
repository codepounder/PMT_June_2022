using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferrara.Compass.Abstractions.Models
{
    public class CommercializationItem
    {
        #region Member Variables
        private int mVersion;
        private int mId;
        private string mProposedItem;
        private string mProjectNumber;
        private string mInitiator;
        private string mBrandManager;
        private string mItemConcept;
        private string mBrand;
        private string mUnitWeight;
        private string mFirstShipDate;
        private string mRetailPackType;
        private string mCustomer;
        private string mCaseType;
        private string mLikeItemDesription;
        private string mAnnualProjectedUnits;
        private string mAnnualProjectedDollars;
        private string mBOYProjectedUnits;
        private string mBOYProjectedDollars;
        private string mMSRP;
        private string mSeason;
        private string mRetailMarginPercent;
        private string mCustomerSpecific;
        private string mCustomerSpecificLotCode;
        private string mFGLikeItemNumber;
        private string mTruckLoadSellingPrice;
        private string mLikeSemiNumber;
        private int mJanuaryForecast;
        private int mFebruaryForecast;
        private int mMarchForecast;
        private int mAprilForecast;
        private int mMayForecast;
        private int mJuneForecast;
        private int mJulyForecast;
        private int mAugustForecast;
        private int mSeptemberForecast;
        private int mOctoberForecast;
        private int mNovemberForecast;
        private int mDecemberForecast;

        private DateTime mSubmittedDate;
        private string mSubmittedBy;
        private string mApprovedBy;
        private DateTime mApprovedDate;
        private string mApproverNotes;

        private List<PackagingItem> mPackagingItemList;
        //private OPSData mOPSData;
        //private QAData mQAData;
        //private PackagingEngineerData mPackagingEngineerData;
        //private CostingData mCostingData;
        //private RequestForItemData mRequestForItemData;
        //private CustomerMarketingData mCustomerMarketingData;
        #endregion

        #region Properties
        public int Version { get { return mVersion; } set { mVersion = value; } }
        public int Id { get { return mId; } set { mId = value; } }
        public string ProposedItem { get { return mProposedItem; } set { mProposedItem = value; } }
        public string ProjectNumber { get { return mProjectNumber; } set { mProjectNumber = value; } }
        public string Initiator { get { return mInitiator; } set { mInitiator = value; } }
        public string BrandManager { get { return mBrandManager; } set { mBrandManager = value; } }
        public string ItemConcept { get { return mItemConcept; } set { mItemConcept = value; } }
        public string Brand { get { return mBrand; } set { mBrand = value; } }
        public string UnitWieght { get { return mUnitWeight; } set { mUnitWeight = value; } }
        public string FirstShipDate { get { return mFirstShipDate; } set { mFirstShipDate = value; } }
        public string RetailPackType { get { return mRetailPackType; } set { mRetailPackType = value; } }
        public string Customer { get { return mCustomer; } set { mCustomer = value; } }
        public string CaseType { get { return mCaseType; } set { mCaseType = value; } }
        public string LikeItemDesription { get { return mLikeItemDesription; } set { mLikeItemDesription = value; } }
        public string AnnualProjectedUnits { get { return mAnnualProjectedUnits; } set { mAnnualProjectedUnits = value; } }
        public string AnnualProjectedDollars { get { return mAnnualProjectedDollars; } set { mAnnualProjectedDollars = value; } }
        public string BOYProjectedUnits { get { return mBOYProjectedUnits; } set { mBOYProjectedUnits = value; } }
        public string BOYProjectedDollars { get { return mBOYProjectedDollars; } set { mBOYProjectedDollars = value; } }
        public string MSRP { get { return mMSRP; } set { mMSRP = value; } }
        public string Season { get { return mSeason; } set { mSeason = value; } }
        public string RetailMarginPercent { get { return mRetailMarginPercent; } set { mRetailMarginPercent = value; } }
        public string SubmittedBy { get { return mSubmittedBy; } set { mSubmittedBy = value; } }
        public DateTime SubmittedDate { get { return mSubmittedDate; } set { mSubmittedDate= value; } }
        public string ApprovedBy { get { return mApprovedBy; } set { mApprovedBy = value; } }
        public DateTime ApprovedDate { get { return mApprovedDate; } set { mApprovedDate= value; } }
        public string ApproverNotes { get { return mApproverNotes; } set { mApproverNotes = value; } }
        public string CustomerSpecific { get { return mCustomerSpecific; } set { mCustomerSpecific = value; } }
        public string CustomerSpecificLotCode { get { return mCustomerSpecificLotCode; } set { mCustomerSpecificLotCode = value; } }
        public string FGLikeItemNumber { get { return mFGLikeItemNumber; } set { mFGLikeItemNumber = value; } }
        public string TruckLoadSellingPrice { get { return mTruckLoadSellingPrice; } set { mTruckLoadSellingPrice = value; } }
        public string LikeSemiNumber { get { return mLikeSemiNumber; } set { mLikeSemiNumber = value; } }
        public int JanuaryForecast { get { return mJanuaryForecast; } set { mJanuaryForecast = value; } }
        public int FebruaryForecast { get { return mFebruaryForecast; } set { mFebruaryForecast = value; } }
        public int MarchForecast { get { return mMarchForecast; } set { mMarchForecast = value; } }
        public int AprilForecast { get { return mAprilForecast; } set { mAprilForecast = value; } }
        public int MayForecast { get { return mMayForecast; } set { mMayForecast = value; } }
        public int JuneForecast { get { return mJuneForecast; } set { mJuneForecast = value; } }
        public int JulyForecast { get { return mJulyForecast; } set { mJulyForecast = value; } }
        public int AugustForecast { get { return mAugustForecast; } set { mAugustForecast = value; } }
        public int SeptemberForecast { get { return mSeptemberForecast; } set { mSeptemberForecast = value; } }
        public int OctoberForecast { get { return mOctoberForecast; } set { mOctoberForecast = value; } }
        public int NovemberForecast { get { return mNovemberForecast; } set { mNovemberForecast = value; } }
        public int DecemberForecast { get { return mDecemberForecast; } set { mDecemberForecast = value; } }

        //public OPSData OPS { get { return mOPSData; } set { mOPSData = value; } }
        //public QAData QA { get { return mQAData; } set { mQAData = value; } }
        //public PackagingEngineerData PackagingEngineer { get { return mPackagingEngineerData; } set { mPackagingEngineerData = value; } }
        //public CostingData Costing { get { return mCostingData; } set { mCostingData = value; } }
        //public RequestForItemData RequestForItem { get { return mRequestForItemData; } set { mRequestForItemData = value; } }
        //public CustomerMarketingData CustomerMarketing { get { return mCustomerMarketingData; } set { mCustomerMarketingData = value; } }

        public List<PackagingItem> PackagingItemList
        {
            get { return mPackagingItemList; }
            set { mPackagingItemList = value; }
        }
        #endregion
    }
}
