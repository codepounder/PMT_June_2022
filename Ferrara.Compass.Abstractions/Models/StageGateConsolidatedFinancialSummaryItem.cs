using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class StageGateConsolidatedFinancialSummaryItem
    {
        public StageGateConsolidatedFinancialSummaryItem()
        {

        }

        private int mId;
        private int mStageGateProjectListItemId;
        private string mGate;
        private string mBriefNumber;
        private string mBriefName;
        private string mBriefSummary;
        private string mName;
        private double mAverageTargetMargin;
        private string mDispConsFinInProjBrief;
        private double mVolumeTotal1;
        private double mVolumeIncremental1;
        private double mGrossSalesTotal1;
        private double mGrossSalesIncremental1;
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
        private string mAnalysesincluded;

        private DateTime mFormSubmittedDate;
        private string mFormSubmittedBy;
        private DateTime mModifiedDate;
        private string mModifiedBy;
        private DateTime mCreatedDate;
        private string mCreatedBy;
        private string mTestProject;

        public int Id { get { return mId; } set { mId = value; } }

        public int StageGateProjectListItemId { get { return mStageGateProjectListItemId; } set { mStageGateProjectListItemId = value; } }
        public string Gate { get { return mGate; } set { mGate = value; } }
        public string BriefNumber { get { return mBriefNumber; } set { mBriefNumber = value; } }
        public string BriefName { get { return mBriefName; } set { mBriefName = value; } }
        public string BriefSummary { get { return mBriefSummary; } set { mBriefSummary = value; } }
        public string Name { get { return mName; } set { mName = value; } }
        public double AverageTargetMargin { get { return mAverageTargetMargin; } set { mAverageTargetMargin = value; } }
        public string DispConsFinInProjBrief { get { return mDispConsFinInProjBrief; } set { mDispConsFinInProjBrief = value; } }
        public double VolumeTotal1 { get { return mVolumeTotal1; } set { mVolumeTotal1 = value; } }
        public double VolumeIncremental1 { get { return mVolumeIncremental1; } set { mVolumeIncremental1 = value; } }
        public double GrossSalesTotal1 { get { return mGrossSalesTotal1; } set { mGrossSalesTotal1 = value; } }
        public double GrossSalesIncremental1 { get { return mGrossSalesIncremental1; } set { mGrossSalesIncremental1 = value; } }
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
        public string Analysesincluded { get { return mAnalysesincluded; } set { mAnalysesincluded = value; } }

        public DateTime FormSubmittedDate { get { return mFormSubmittedDate; } set { mFormSubmittedDate = value; } }
        public string FormSubmittedBy { get { return mFormSubmittedBy; } set { mFormSubmittedBy = value; } }
        public DateTime ModifiedDate { get { return mModifiedDate; } set { mModifiedDate = value; } }
        public string ModifiedBy { get { return mModifiedBy; } set { mModifiedBy = value; } }
        public DateTime CreatedDate { get { return mCreatedDate; } set { mCreatedDate = value; } }
        public string CreatedBy { get { return mCreatedBy; } set { mCreatedBy = value; } }
        public string TestProject { get { return mTestProject; } set { mTestProject = value; } }
    }
}
