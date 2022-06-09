using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class InitialCostingReviewItem
    {
        #region Variables
        private int mCompassListItemId;
        private string mLastUpdatedFormName;

        private string mInitialCosting_Comments;
        private string mInitialCosting_Decision;
        private string mInitialCosting_GrossMarginAccurate;

        private string mSrOBMApproval_CostingReviewComments;
        private double mExpectedGrossMarginPercent;
        private double mRevisedGrossMarginPercent;
        #endregion

        #region Properties       
        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        public string LastUpdatedFormName { get { return mLastUpdatedFormName; } set { mLastUpdatedFormName = value; } }

        public string InitialCosting_Comments { get { return mInitialCosting_Comments; } set { mInitialCosting_Comments = value; } }
        public string InitialCosting_GrossMarginAccurate { get { return mInitialCosting_GrossMarginAccurate; } set { mInitialCosting_GrossMarginAccurate = value; } }
        public string InitialCosting_Decision { get { return mInitialCosting_Decision; } set { mInitialCosting_Decision = value; } }

        public string SrOBMApproval_CostingReviewComments { get { return mSrOBMApproval_CostingReviewComments; } set { mSrOBMApproval_CostingReviewComments = value; } }
        public double ExpectedGrossMarginPercent { get { return mExpectedGrossMarginPercent; } set { mExpectedGrossMarginPercent = value; } }
        public double RevisedGrossMarginPercent { get { return mRevisedGrossMarginPercent; } set { mRevisedGrossMarginPercent = value; } }
        #endregion

    }
}
