using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferrara.Compass.Abstractions.Models
{
    public class ProjectTimelineItem
    {
        public ProjectTimelineItem()
        {
        }
        private string Title;
        private int mProjectTimelineItemId;
        private int compassListItemId;
        private double mIPF;
        private double mSrOBMApproval;
        private double mSrOBMApproval2;
        private double mInitialCosting;
        private double mInitialCapacity;
        private double mTradePromo;
        private double mEstPricing;
        private double mEstBracketPricing;
        private double mDistribution;
        private double mOperations;
        private double mPrelimSAPInitialSetup;
        private double mSAPInitialSetup;
        private double mQA;
        private double mOBMReview1;
        private double mBOMSetupPE;
        private double mBOMSetupProc;
        private double mBOMSetupPE2;
        private double mBOMSetupPE3;
        private double mOBMReview2;
        private double mGRAPHICS;
        private double mCostingQuote;
        private double mFGPackSpec;
        private double mSAPBOMSetup;
        private double mOBMReview3;
        private double mFCST;
        private double mExternalMfg;
        private double mSAPRoutingSetup;
        private double mBOMActiveDate;
        private double mSAPCostingDetails;
        private double mSAPWarehouseInfo;
        private double mStandardCostEntry;
        private double mCostFinishedGood;
        private double mFinalCostingReview;
        private double mPurchasedPO;
        private double mRemoveSAPBlocks;
        private double mCustomerPO;
        private double mMaterialsRcvdChk;
        private double mFirstProductionChk;
        private double mDistributionChk;
        private double mMaterialWarehouseSetUp;
        private double mSAPCompleteItemSetup;
        private double mBEQRC;

        public int ProjectTimelineItemId { get { return mProjectTimelineItemId; } set { mProjectTimelineItemId = value; } }
        public int CompassListItemId { get { return compassListItemId; } set { compassListItemId = value; } }
        public double IPF { get { return mIPF; } set { mIPF = value; } }
        public double SrOBMApproval { get { return mSrOBMApproval; } set { mSrOBMApproval = value; } }
        public double SrOBMApproval2 { get { return mSrOBMApproval2; } set { mSrOBMApproval2 = value; } }
        public double InitialCosting { get { return mInitialCosting; } set { mInitialCosting = value; } }
        public double InitialCapacity { get { return mInitialCapacity; } set { mInitialCapacity = value; } }
        public double TradePromo { get { return mTradePromo; } set { mTradePromo = value; } }
        public double EstPricing { get { return mEstPricing; } set { mEstBracketPricing = value; } }
        public double EstBracketPricing { get { return mEstBracketPricing; } set { mEstBracketPricing = value; } }
        public double Distribution { get { return mDistribution; } set { mDistribution = value; } }
        public double Operations { get { return mOperations; } set { mOperations = value; } }
        public double SAPInitialSetup { get { return mSAPInitialSetup; } set { mSAPInitialSetup = value; } }
        public double PrelimSAPInitialSetup { get { return mPrelimSAPInitialSetup; } set { mPrelimSAPInitialSetup = value; } }
        public double QA { get { return mQA; } set { mQA = value; } }
        public double OBMReview1 { get { return mOBMReview1; } set { mOBMReview1 = value; } }
        public double BOMSetupPE { get { return mBOMSetupPE; } set { mBOMSetupPE = value; } }
        public double BOMSetupProc { get { return mBOMSetupProc; } set { mBOMSetupProc = value; } }
        public double BOMSetupPE2 { get { return mBOMSetupPE2; } set { mBOMSetupPE2 = value; } }
        public double BOMSetupPE3 { get { return mBOMSetupPE3; } set { mBOMSetupPE3 = value; } }
        public double OBMReview2 { get { return mOBMReview2; } set { mOBMReview2 = value; } }
        public double GRAPHICS { get { return mGRAPHICS; } set { mGRAPHICS = value; } }
        public double CostingQuote { get { return mCostingQuote; } set { mCostingQuote = value; } }
        public double FGPackSpec { get { return mFGPackSpec; } set { mFGPackSpec = value; } }
        public double SAPBOMSetup { get { return mSAPBOMSetup; } set { mSAPBOMSetup = value; } }
        public double OBMReview3 { get { return mOBMReview3; } set { mOBMReview3 = value; } }
        public double FCST { get { return mFCST; } set { mFCST = value; } }
        public double ExternalMfg { get { return mExternalMfg; } set { mExternalMfg = value; } }
        public double SAPRoutingSetup { get { return mSAPRoutingSetup; } set { mSAPRoutingSetup = value; } }
        public double BOMActiveDate { get { return mBOMActiveDate; } set { mBOMActiveDate = value; } }
        public double SAPCostingDetails { get { return mSAPCostingDetails; } set { mSAPCostingDetails = value; } }
        public double SAPWarehouseInfo { get { return mSAPWarehouseInfo; } set { mSAPWarehouseInfo = value; } }
        public double StandardCostEntry { get { return mStandardCostEntry; } set { mStandardCostEntry = value; } }
        public double CostFinishedGood { get { return mCostFinishedGood; } set { mCostFinishedGood = value; } }
        public double FinalCostingReview { get { return mFinalCostingReview; } set { mFinalCostingReview = value; } }
        public double PurchasedPO { get { return mPurchasedPO; } set { mPurchasedPO = value; } }
        public double RemoveSAPBlocks { get { return mRemoveSAPBlocks; } set { mRemoveSAPBlocks = value; } }
        public double CustomerPO { get { return mCustomerPO; } set { mCustomerPO = value; } }
        public double MaterialsRcvdChk { get { return mMaterialsRcvdChk; } set { mMaterialsRcvdChk = value; } }
        public double FirstProductionChk { get { return mFirstProductionChk; } set { mFirstProductionChk = value; } }
        public double DistributionChk { get { return mDistributionChk; } set { mDistributionChk = value; } }
        public double MaterialWarehouseSetUp { get { return mMaterialWarehouseSetUp; } set { mMaterialWarehouseSetUp = value; } }
        public double SAPCompleteItemSetup { get { return mSAPCompleteItemSetup; } set { mSAPCompleteItemSetup = value; } }
        public double BEQRC { get { return mBEQRC; } set { mBEQRC = value; } }
    }
}
