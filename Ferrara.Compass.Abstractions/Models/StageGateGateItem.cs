using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    [Serializable]
    public class StageGateGateItem
    {
        public StageGateGateItem()
        {
        }

        #region Variables
        private int mID;
        private int mStageGateListItemId;
        private string mProjectNumber;
        private string mGate;
        private int mBriefNo;

        private string mMarketingColor;
        private string mSalesColor;
        private string mFinanceColor;
        private string mRDColor;
        private string mQAColor;
        private string mPEColor;
        private string mManuColor;
        private string mSupplyChainColor;

        private string mMarketingComments;
        private string mSalesComments;
        private string mFinanceComments;
        private string mRDComments;
        private string mQAComments;
        private string mPEComments;
        private string mManuComments;
        private string mSupplyChainComments;

        private string mSGMeetingStatus;
        private DateTime mSGMeetingDate;
        private DateTime mActualSGMeetingDate;
        private string mFormSubmittedDate;
        private string mFormSubmittedBy;
        private DateTime mModifiedDate;
        private DateTime mCreatedDate;
        private string mCreatedBy;
        private string mModifiedBy;
        private string mReadinessPct;
        private int mDeliverablesApplicable;
        private int mDeliverablesCompleted;

        private string mBriefName;
        private string mProductFormats;
        private string mRetailExecution;
        private string mOtherKeyInfo;
        private string mOverallRisk;
        private string mOverallStatus;
        private string mMilestones;
        private string mImpactProjectHealth;
        private string mTeamRecommendation;
        private string mOverallRiskReason;
        private string mOverallStatusReason;
        private string mGateReadiness;
        private string mFinanceBriefInGateBrief;
        private string mDeleted;
        #endregion

        public int StageGateListItemId { get { return mStageGateListItemId; } set { mStageGateListItemId = value; } }
        public int ID { get { return mID; } set { mID = value; } }

        #region List Properties
        public string ProjectNumber { get { return mProjectNumber; } set { mProjectNumber = value; } }
        public string Gate { get { return mGate; } set { mGate = value; } }
        public int BriefNo { get { return mBriefNo; } set { mBriefNo = value; } }
        public string MarketingColor { get { return mMarketingColor; } set { mMarketingColor = value; } }
        public string SalesColor { get { return mSalesColor; } set { mSalesColor = value; } }
        public string FinanceColor { get { return mFinanceColor; } set { mFinanceColor = value; } }
        public string RDColor { get { return mRDColor; } set { mRDColor = value; } }
        public string QAColor { get { return mQAColor; } set { mQAColor = value; } }
        public string PEColor { get { return mPEColor; } set { mPEColor = value; } }
        public string ManuColor { get { return mManuColor; } set { mManuColor = value; } }
        public string SupplyChainColor { get { return mSupplyChainColor; } set { mSupplyChainColor = value; } }

        public string MarketingComments { get { return mMarketingComments; } set { mMarketingComments = value; } }
        public string SalesComments { get { return mSalesComments; } set { mSalesComments = value; } }
        public string FinanceComments { get { return mFinanceComments; } set { mFinanceComments = value; } }
        public string RDComments { get { return mRDComments; } set { mRDComments = value; } }
        public string QAComments { get { return mQAComments; } set { mQAComments = value; } }
        public string PEComments { get { return mPEComments; } set { mPEComments = value; } }
        public string ManuComments { get { return mManuComments; } set { mManuComments = value; } }
        public string SupplyChainComments { get { return mSupplyChainComments; } set { mSupplyChainComments = value; } }

        public string SGMeetingStatus { get { return mSGMeetingStatus; } set { mSGMeetingStatus = value; } }
        public DateTime SGMeetingDate { get { return mSGMeetingDate; } set { mSGMeetingDate = value; } }
        public DateTime ActualSGMeetingDate { get { return mActualSGMeetingDate; } set { mActualSGMeetingDate = value; } }
        public string FormSubmittedDate { get { return mFormSubmittedDate; } set { mFormSubmittedDate = value; } }
        public string FormSubmittedBy { get { return mFormSubmittedBy; } set { mFormSubmittedBy = value; } }
        public DateTime ModifiedDate { get { return mModifiedDate; } set { mModifiedDate = value; } }
        public DateTime CreatedDate { get { return mCreatedDate; } set { mModifiedDate = value; } }
        public string CreatedBy { get { return mCreatedBy; } set { mCreatedBy = value; } }
        public string ModifiedBy { get { return mModifiedBy; } set { mModifiedBy = value; } }
        public string ReadinessPct { get { return mReadinessPct; } set { mReadinessPct = value; } }
        public int DeliverablesApplicable { get { return mDeliverablesApplicable; } set { mDeliverablesApplicable = value; } }
        public int DeliverablesCompleted { get { return mDeliverablesCompleted; } set { mDeliverablesCompleted = value; } }
        public string BriefName { get { return mBriefName; } set { mBriefName = value; } }
        public string ProductFormats { get { return mProductFormats; } set { mProductFormats = value; } }
        public string RetailExecution { get { return mRetailExecution; } set { mRetailExecution = value; } }
        public string OtherKeyInfo { get { return mOtherKeyInfo; } set { mOtherKeyInfo = value; } }
        public string OverallRisk { get { return mOverallRisk; } set { mOverallRisk = value; } }
        public string OverallStatus { get { return mOverallStatus; } set { mOverallStatus = value; } }
        public string Milestones { get { return mMilestones; } set { mMilestones = value; } }
        public string ImpactProjectHealth { get { return mImpactProjectHealth; } set { mImpactProjectHealth = value; } }
        public string TeamRecommendation { get { return mTeamRecommendation; } set { mTeamRecommendation = value; } }
        public string OverallRiskReason { get { return mOverallRiskReason; } set { mOverallRiskReason = value; } }
        public string OverallStatusReason { get { return mOverallStatusReason; } set { mOverallStatusReason = value; } }
        public string GateReadiness { get { return mGateReadiness; } set { mGateReadiness = value; } }
        public string FinanceBriefInGateBrief { get { return mFinanceBriefInGateBrief; } set { mFinanceBriefInGateBrief = value; } }
        public string Deleted { get { return mDeleted; } set { mDeleted = value; } }
        #endregion
    }
}
