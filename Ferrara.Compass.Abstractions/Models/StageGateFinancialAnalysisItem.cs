using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class StageGateFinancialAnalysisItem
    {
        public StageGateFinancialAnalysisItem()
        {

        }

        private int mId;
        private int mStageGateProjectListItemId;
        private string mGate;
        private string mBriefNumber;
        private string mBriefName;
        private string mAnalysisName;
        private string mFGNumber;
        private string mCustomerChannel;
        private string mBrandSeason;
        private string mProductForm;
        private double mTargetMarginPct;
        private string mPLsinProjectBrief;
        private string mPLinConsolidatedFinancials;
        private double mVolumeTotal1;
        private double mVolumeIncremental1;
        private double mGrossSalesTotal1;
        private double mGrossSalesIncremental1;
        private double mTradeRateTotal1;
        private double mTradeRateIncremental1;
        private double mOGTNTotal1;
        private double mOGTNIncremental1;
        private double mNetSalesTotal1;
        private double mNetSalesIncremental1;
        private double mCOGSTotal1;
        private double mCOGSIncremental1;
        private double mGrossMarginTotal1;
        private double mGrossMarginIncremental1;
        private double mGrossMarginPctTotal1;
        private double mGrossMarginPctIncremental1;
        private double mVolumeTotal2;
        private double mVolumeIncremental2;
        private double mGrossSalesTotal2;
        private double mGrossSalesIncremental2;
        private double mTradeRateTotal2;
        private double mTradeRateIncremental2;
        private double mOGTNTotal2;
        private double mOGTNIncremental2;
        private double mNetSalesTotal2;
        private double mNetSalesIncremental2;
        private double mCOGSTotal2;
        private double mCOGSIncremental2;
        private double mGrossMarginTotal2;
        private double mGrossMarginIncremental2;
        private double mGrossMarginPctTotal2;
        private double mGrossMarginPctIncremental2;
        private double mVolumeTotal3;
        private double mVolumeIncremental3;
        private double mGrossSalesTotal3;
        private double mGrossSalesIncremental3;
        private double mTradeRateTotal3;
        private double mTradeRateIncremental3;
        private double mOGTNTotal3;
        private double mOGTNIncremental3;
        private double mNetSalesTotal3;
        private double mNetSalesIncremental3;
        private double mCOGSTotal3;
        private double mCOGSIncremental3;
        private double mGrossMarginTotal3;
        private double mGrossMarginIncremental3;
        private double mGrossMarginPctTotal3;
        private double mGrossMarginPctIncremental3;
        private double mNSDollerperLB1;
        private double mNSDollerperLB2;
        private double mNSDollerperLB3;
        private double mCOGSperLB1;
        private double mCOGSperLB2;
        private double mCOGSperLB3;
        private double mTruckldPricePrRtlSllngUt1;
        private double mTruckldPricePrRtlSllngUt2;
        private double mTruckldPricePrRtlSllngUt3;
        private string mAssumptions1;
        private string mAssumptions2;
        private string mAssumptions3;
        private string mDeleted;

        private DateTime mFormSubmittedDate;
        private string mFormSubmittedBy;
        private DateTime mModifiedDate;
        private string mModifiedBy;
        private string mTestProject;

        public int Id { get { return mId; } set { mId = value; } }
        public int StageGateProjectListItemId { get { return mStageGateProjectListItemId; } set { mStageGateProjectListItemId = value; } }
        public string Gate { get { return mGate; } set { mGate = value; } }
        public string BriefNumber { get { return mBriefNumber; } set { mBriefNumber = value; } }
        public string BriefName { get { return mBriefName; } set { mBriefName = value; } }
        public string AnalysisName { get { return mAnalysisName; } set { mAnalysisName = value; } }
        public string FGNumber { get { return mFGNumber; } set { mFGNumber = value; } }
        public string CustomerChannel { get { return mCustomerChannel; } set { mCustomerChannel = value; } }
        public string BrandSeason { get { return mBrandSeason; } set { mBrandSeason = value; } }
        public string ProductForm { get { return mProductForm; } set { mProductForm = value; } }
        public double TargetMarginPct { get { return mTargetMarginPct; } set { mTargetMarginPct = value; } }
        public string PLsinProjectBrief { get { return mPLsinProjectBrief; } set { mPLsinProjectBrief = value; } }
        public string PLinConsolidatedFinancials { get { return mPLinConsolidatedFinancials; } set { mPLinConsolidatedFinancials = value; } }
        public double VolumeTotal1 { get { return mVolumeTotal1; } set { mVolumeTotal1 = value; } }
        public double VolumeIncremental1 { get { return mVolumeIncremental1; } set { mVolumeIncremental1 = value; } }
        public double GrossSalesTotal1 { get { return mGrossSalesTotal1; } set { mGrossSalesTotal1 = value; } }
        public double GrossSalesIncremental1 { get { return mGrossSalesIncremental1; } set { mGrossSalesIncremental1 = value; } }
        public double TradeRateTotal1 { get { return mTradeRateTotal1; } set { mTradeRateTotal1 = value; } }
        public double TradeRateIncremental1 { get { return mTradeRateIncremental1; } set { mTradeRateIncremental1 = value; } }
        public double OGTNTotal1 { get { return mOGTNTotal1; } set { mOGTNTotal1 = value; } }
        public double OGTNIncremental1 { get { return mOGTNIncremental1; } set { mOGTNIncremental1 = value; } }
        public double NetSalesTotal1 { get { return mNetSalesTotal1; } set { mNetSalesTotal1 = value; } }
        public double NetSalesIncremental1 { get { return mNetSalesIncremental1; } set { mNetSalesIncremental1 = value; } }
        public double COGSTotal1 { get { return mCOGSTotal1; } set { mCOGSTotal1 = value; } }
        public double COGSIncremental1 { get { return mCOGSIncremental1; } set { mCOGSIncremental1 = value; } }
        public double GrossMarginTotal1 { get { return mGrossMarginTotal1; } set { mGrossMarginTotal1 = value; } }
        public double GrossMarginIncremental1 { get { return mGrossMarginIncremental1; } set { mGrossMarginIncremental1 = value; } }
        public double GrossMarginPctTotal1 { get { return mGrossMarginPctTotal1; } set { mGrossMarginPctTotal1 = value; } }
        public double GrossMarginPctIncremental1 { get { return mGrossMarginPctIncremental1; } set { mGrossMarginPctIncremental1 = value; } }
        public double VolumeTotal2 { get { return mVolumeTotal2; } set { mVolumeTotal2 = value; } }
        public double VolumeIncremental2 { get { return mVolumeIncremental2; } set { mVolumeIncremental2 = value; } }
        public double GrossSalesTotal2 { get { return mGrossSalesTotal2; } set { mGrossSalesTotal2 = value; } }
        public double GrossSalesIncremental2 { get { return mGrossSalesIncremental2; } set { mGrossSalesIncremental2 = value; } }
        public double TradeRateTotal2 { get { return mTradeRateTotal2; } set { mTradeRateTotal2 = value; } }
        public double TradeRateIncremental2 { get { return mTradeRateIncremental2; } set { mTradeRateIncremental2 = value; } }
        public double OGTNTotal2 { get { return mOGTNTotal2; } set { mOGTNTotal2 = value; } }
        public double OGTNIncremental2 { get { return mOGTNIncremental2; } set { mOGTNIncremental2 = value; } }
        public double NetSalesTotal2 { get { return mNetSalesTotal2; } set { mNetSalesTotal2 = value; } }
        public double NetSalesIncremental2 { get { return mNetSalesIncremental2; } set { mNetSalesIncremental2 = value; } }
        public double COGSTotal2 { get { return mCOGSTotal2; } set { mCOGSTotal2 = value; } }
        public double COGSIncremental2 { get { return mCOGSIncremental2; } set { mCOGSIncremental2 = value; } }
        public double GrossMarginTotal2 { get { return mGrossMarginTotal2; } set { mGrossMarginTotal2 = value; } }
        public double GrossMarginIncremental2 { get { return mGrossMarginIncremental2; } set { mGrossMarginIncremental2 = value; } }
        public double GrossMarginPctTotal2 { get { return mGrossMarginPctTotal2; } set { mGrossMarginPctTotal2 = value; } }
        public double GrossMarginPctIncremental2 { get { return mGrossMarginPctIncremental2; } set { mGrossMarginPctIncremental2 = value; } }
        public double VolumeTotal3 { get { return mVolumeTotal3; } set { mVolumeTotal3 = value; } }
        public double VolumeIncremental3 { get { return mVolumeIncremental3; } set { mVolumeIncremental3 = value; } }
        public double GrossSalesTotal3 { get { return mGrossSalesTotal3; } set { mGrossSalesTotal3 = value; } }
        public double GrossSalesIncremental3 { get { return mGrossSalesIncremental3; } set { mGrossSalesIncremental3 = value; } }
        public double TradeRateTotal3 { get { return mTradeRateTotal3; } set { mTradeRateTotal3 = value; } }
        public double TradeRateIncremental3 { get { return mTradeRateIncremental3; } set { mTradeRateIncremental3 = value; } }
        public double OGTNTotal3 { get { return mOGTNTotal3; } set { mOGTNTotal3 = value; } }
        public double OGTNIncremental3 { get { return mOGTNIncremental3; } set { mOGTNIncremental3 = value; } }
        public double NetSalesTotal3 { get { return mNetSalesTotal3; } set { mNetSalesTotal3 = value; } }
        public double NetSalesIncremental3 { get { return mNetSalesIncremental3; } set { mNetSalesIncremental3 = value; } }
        public double COGSTotal3 { get { return mCOGSTotal3; } set { mCOGSTotal3 = value; } }
        public double COGSIncremental3 { get { return mCOGSIncremental3; } set { mCOGSIncremental3 = value; } }
        public double GrossMarginTotal3 { get { return mGrossMarginTotal3; } set { mGrossMarginTotal3 = value; } }
        public double GrossMarginIncremental3 { get { return mGrossMarginIncremental3; } set { mGrossMarginIncremental3 = value; } }
        public double GrossMarginPctTotal3 { get { return mGrossMarginPctTotal3; } set { mGrossMarginPctTotal3 = value; } }
        public double GrossMarginPctIncremental3 { get { return mGrossMarginPctIncremental3; } set { mGrossMarginPctIncremental3 = value; } }
        public double NSDollerperLB1 { get { return mNSDollerperLB1; } set { mNSDollerperLB1 = value; } }
        public double NSDollerperLB2 { get { return mNSDollerperLB2; } set { mNSDollerperLB2 = value; } }
        public double NSDollerperLB3 { get { return mNSDollerperLB3; } set { mNSDollerperLB3 = value; } }
        public double COGSperLB1 { get { return mCOGSperLB1; } set { mCOGSperLB1 = value; } }
        public double COGSperLB2 { get { return mCOGSperLB2; } set { mCOGSperLB2 = value; } }
        public double COGSperLB3 { get { return mCOGSperLB3; } set { mCOGSperLB3 = value; } }
        public double TruckldPricePrRtlSllngUt1 { get { return mTruckldPricePrRtlSllngUt1; } set { mTruckldPricePrRtlSllngUt1 = value; } }
        public double TruckldPricePrRtlSllngUt2 { get { return mTruckldPricePrRtlSllngUt2; } set { mTruckldPricePrRtlSllngUt2 = value; } }
        public double TruckldPricePrRtlSllngUt3 { get { return mTruckldPricePrRtlSllngUt3; } set { mTruckldPricePrRtlSllngUt3 = value; } }
        public string Assumptions1 { get { return mAssumptions1; } set { mAssumptions1 = value; } }
        public string Assumptions2 { get { return mAssumptions2; } set { mAssumptions2 = value; } }
        public string Assumptions3 { get { return mAssumptions3; } set { mAssumptions3 = value; } }
        public string Deleted { get { return mDeleted; } set { mDeleted = value; } }

        public DateTime FormSubmittedDate { get { return mFormSubmittedDate; } set { mFormSubmittedDate = value; } }
        public string FormSubmittedBy { get { return mFormSubmittedBy; } set { mFormSubmittedBy = value; } }
        public DateTime ModifiedDate { get { return mModifiedDate; } set { mModifiedDate = value; } }
        public string ModifiedBy { get { return mModifiedBy; } set { mModifiedBy = value; } }
        public string TestProject { get { return mTestProject; } set { mTestProject = value; } }

    }
}
