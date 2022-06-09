using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferrara.Compass.Abstractions.Models
{
    public class MarketingItem
    {
        #region Member Variables
        private int mVersion;
        private string mCustomerSpecific;
        private string mCustomerSpecificLotCode;
        private string mFGLikeItemNumber;
        private string mTruckLoadSellingPrice;
        private string mExistingSemiOrBulkNumber;
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
        private string mShipReadyMonth;
        private DateTime mCreatedDate;
        private string mCreatedBy;
        private DateTime mModifiedDate;
        private string mModifiedBy;

        #endregion

        #region Properties
        public int Version { get { return mVersion; } set { mVersion = value; } }
        public string CustomerSpecific { get { return mCustomerSpecific; } set { mCustomerSpecific = value; } }
        public string CustomerSpecificLotCode { get { return mCustomerSpecificLotCode; } set { mCustomerSpecificLotCode = value; } }
        public string FGLikeItemNumber { get { return mFGLikeItemNumber; } set { mFGLikeItemNumber = value; } }
        public string TruckLoadSellingPrice { get { return mTruckLoadSellingPrice; } set { mTruckLoadSellingPrice = value; } }
        public string ExistingSemiOrBulkNumber { get { return mExistingSemiOrBulkNumber; } set { mExistingSemiOrBulkNumber = value; } }
        public string ShipReadyMonth { get { return mShipReadyMonth; } set { mShipReadyMonth = value; } }
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
        public DateTime CreatedDate { get { return mCreatedDate; } set { mCreatedDate = value; } }
        public string CreatedBy { get { return mCreatedBy; } set { mCreatedBy = value; } }
        public DateTime ModifiedDate { get { return mModifiedDate; } set { mModifiedDate = value; } }
        public string ModifiedBy { get { return mModifiedBy; } set { mModifiedBy = value; } }

        public int TotalForecastedCases()
        {
            return mJanuaryForecast + mFebruaryForecast + mMarchForecast + mAprilForecast + mMayForecast + mJuneForecast + mJulyForecast + mAugustForecast +
                    mSeptemberForecast + mOctoberForecast + mNovemberForecast + mDecemberForecast;
        }
        #endregion
    }
}
