using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    [Serializable]
    public class UserUpdatedTimelineItem
    {
        #region General Variables

        private int mCompassListItemId;
        private int mBOMPE;
        private int mBOMPE2;
        private int mBOMPROC;
        private int mDistribution;
        private int mSrOBMApproval;
        private int mSrOBMApproval2;
        private int mSAPInitialSetup;
        private int mPrelimSAPInitialSetup;
        private int mInitialCapacity;
        private int mIPF;
        private int mOperations;
        private int mOBMReview1;
        private int mOBMReview2;
        private int mOBMReview3;
        private int mQA;
        private int mRNDFINAL;
        private int mTradePromo;
        private int mExternalMfg;
        private int mPACKENG1;
        private int mPACKENG2;
        private int mPACKPROC;
        private int mGRAPHICS;
        private int mFCST;
        private int mFGPackSpec;
        private int mCostingQuote;
        private int mInitialCosting;

        #endregion

        #region General Properties
        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        public int BOMPE { get { return mBOMPE; } set { mBOMPE = value; } }
        public int BOMPE2 { get { return mBOMPE2; } set { mBOMPE2 = value; } }
        public int BOMPROC { get { return mBOMPROC; } set { mBOMPROC = value; } }
        public int Distribution { get { return mDistribution; } set { mDistribution = value; } }
        public int SrOBMApproval { get { return mSrOBMApproval; } set { mSrOBMApproval = value; } }
        public int SrOBMApproval2 { get { return mSrOBMApproval2; } set { mSrOBMApproval2 = value; } }
        public int SAPInitialSetup { get { return mSAPInitialSetup; } set { mSAPInitialSetup = value; } }
        public int PrelimSAPInitialSetup { get { return mPrelimSAPInitialSetup; } set { mPrelimSAPInitialSetup = value; } }
        public int InitialCapacity { get { return mInitialCapacity; } set { mInitialCapacity = value; } }
        public int IPF { get { return mIPF; } set { mIPF = value; } }
        public int Operations { get { return mOperations; } set { mOperations = value; } }
        public int OBMReview1 { get { return mOBMReview1; } set { mOBMReview1 = value; } }
        public int OBMReview2 { get { return mOBMReview2; } set { mOBMReview2 = value; } }
        public int OBMReview3 { get { return mOBMReview3; } set { mOBMReview3 = value; } }
        public int QA { get { return mQA; } set { mQA = value; } }
        public int RNDFINAL { get { return mRNDFINAL; } set { mRNDFINAL = value; } }
        public int TradePromo { get { return mTradePromo; } set { mTradePromo = value; } }
        public int ExternalMfg { get { return mExternalMfg; } set { mExternalMfg = value; } }
        public int PACKENG1 { get { return mPACKENG1; } set { mPACKENG1 = value; } }
        public int PACKENG2 { get { return mPACKENG2; } set { mPACKENG2 = value; } }
        public int PACKPROC { get { return mPACKPROC; } set { mPACKPROC = value; } }
        public int GRAPHICS { get { return mGRAPHICS; } set { mGRAPHICS = value; } }
        public int FCST { get { return mFCST; } set { mFCST = value; } }
        public int FGPackSpec { get { return mFGPackSpec; } set { mFGPackSpec = value; } }
        public int CostingQuote { get { return mCostingQuote; } set { mCostingQuote = value; } }
        public int InitialCosting { get { return mInitialCosting; } set { mInitialCosting = value; } }

        #endregion
    }
}
