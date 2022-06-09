using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferrara.Compass.Abstractions.Models
{
    public class ProcessDataItem
    {
        public ProcessDataItem()
        {
            mMakePackRequired = "Yes";
            mDistributionRequired = "Yes";
            mInternationalComplianceRequired = "Yes";
            mCoManRequired = "Yes";
            mQARequired = "Yes";
            mCustomerMarketingRequired = "Yes";
            mSemiRequestRequired = "Yes";
        }

        #region Member Variables
        private int mId;
        private int mCompassListItemId;
        private string mMakePackRequired;
        private string mDistributionRequired;
        private string mInternationalComplianceRequired;
        private string mCoManRequired;
        private string mQARequired;
        private string mCustomerMarketingRequired;
        private string mSemiRequestRequired;
        #endregion

        #region Properties
        public int Id { get { return mId; } set { mId = value; } }
        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        public string MakePackRequired { get { return mMakePackRequired; } set { mMakePackRequired = value; } }
        public string DistributionRequired { get { return mDistributionRequired; } set { mDistributionRequired = value; } }
        public string InternationalComplianceRequired { get { return mInternationalComplianceRequired; } set { mInternationalComplianceRequired = value; } }
        public string CoManRequired { get { return mCoManRequired; } set { mCoManRequired = value; } }
        public string QARequired { get { return mQARequired; } set { mQARequired = value; } }
        public string CustomerMarketingRequired { get { return mCustomerMarketingRequired; } set { mCustomerMarketingRequired = value; } }
        public string SemiRequestRequired { get { return mSemiRequestRequired; } set { mSemiRequestRequired = value; } }
        #endregion
    }
}
