using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class InitialApprovalReviewItem
    {
        #region Variables
        private int mCompassListItemId;
        private string mLastUpdatedFormName;

        private string mSrOBMApproval_Comments;
        private string mSrOBMApproval_CostingReviewComments;
        private string mSrOBMApproval_CapacityReviewComments;
        private string mSrOBMApproval_Decision;
        private string mSrOBMApproval_CostingDecision;
        private string mSrOBMApproval_CapacityDecision;

        private double mExpectedGrossMarginPercent;
        private string mSAPItemNumber;
        private int mAnnualProjectedUnits;
        private string mProjectNumber;
        private string mProjectType;
        private DateTime mRevisedFirstShipDate;
        private string mProductHierarchyLevel1;
        private string mProductHierarchyLevel2;
        private string mMaterialGroup1Brand;
        private string mCustomer;
        private string mItemConcept;
        private string mPM;
        private string mPMName;
        private double mAnnualProjectedDollars;
        private string mChannel;
        private string mPackTrialNeeded;
        private string mInitialTimeTable;
        private string mNeedSExpeditedWorkflowWithSGS;
        private DateTime mProjectStartDate;
        #endregion

        #region Properties       
        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        public string LastUpdatedFormName { get { return mLastUpdatedFormName; } set { mLastUpdatedFormName = value; } }

        public string SrOBMApproval_Comments { get { return mSrOBMApproval_Comments; } set { mSrOBMApproval_Comments = value; } }
        public string SrOBMApproval_CostingReviewComments { get { return mSrOBMApproval_CostingReviewComments; } set { mSrOBMApproval_CostingReviewComments = value; } }
        //public string SrOBMApproval_CapacityReviewComments { get { return mSrOBMApproval_CapacityReviewComments; } set { mSrOBMApproval_CapacityReviewComments = value; } }
        public string SrOBMApproval_Decision { get { return mSrOBMApproval_Decision; } set { mSrOBMApproval_Decision = value; } }
        public string SrOBMApproval_CostingDecision { get { return mSrOBMApproval_CostingDecision; } set { mSrOBMApproval_CostingDecision = value; } }
       // public string SrOBMApproval_CapacityDecision { get { return mSrOBMApproval_CapacityDecision; } set { mSrOBMApproval_CapacityDecision = value; } }

        public double ExpectedGrossMarginPercent { get { return mExpectedGrossMarginPercent; } set { mExpectedGrossMarginPercent = value; } }
        public string SAPItemNumber { get { return mSAPItemNumber; } set { mSAPItemNumber = value; } }
        public int AnnualProjectedUnits { get { return mAnnualProjectedUnits; } set { mAnnualProjectedUnits = value; } }
        public string ProjectNumber { get { return mProjectNumber; } set { mProjectNumber = value; } }
        public string ProjectType { get { return mProjectType; } set { mProjectType = value; } }
        public DateTime RevisedFirstShipDate { get { return mRevisedFirstShipDate; } set { mRevisedFirstShipDate = value; } }
        public DateTime ProjectStartDate { get { return mProjectStartDate; } set { mProjectStartDate = value; } }
        public string ProductHierarchyLevel1 { get { return mProductHierarchyLevel1; } set { mProductHierarchyLevel1 = value; } }
        public string ProductHierarchyLevel2 { get { return mProductHierarchyLevel2; } set { mProductHierarchyLevel2 = value; } }
        public string MaterialGroup1Brand { get { return mMaterialGroup1Brand; } set { mMaterialGroup1Brand = value; } }
        public string Customer { get { return mCustomer; } set { mCustomer = value; } }
        public string ItemConcept { get { return mItemConcept; } set { mItemConcept = value; } }
        public string PM { get { return mPM; } set { mPM = value; } }
        public string PMName { get { return mPMName; } set { mPMName = value; } }
        public double AnnualProjectedDollars { get { return mAnnualProjectedDollars; } set { mAnnualProjectedDollars = value; } }
        public string Channel { get { return mChannel; } set { mChannel = value; } }

        public string PackTrialNeeded { get { return mPackTrialNeeded; } set { mPackTrialNeeded = value; } }

        public string InitialTimeTable { get { return mInitialTimeTable; } set { mInitialTimeTable = value; } }
        public string NeedSExpeditedWorkflowWithSGS { get { return mNeedSExpeditedWorkflowWithSGS; } set { mNeedSExpeditedWorkflowWithSGS = value; } }
        
        #endregion
    }
}
