using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class DashboardDetailsItem
    {
        #region Variables
        private int mCompassListItemId;
        private string mProjectNumber;
        private string mTimelineType;
        private string mFirstShipDate;
        private string mFirstProductionDate;
        private string mProjectType;
        private string mWorkflowPhase;
        private string mProjectName;
        private string mCompassProjectType;
        private string mProjectTypeSubCategory;
        private string mParentProjectNumber;
        private string mStageGateProjectListItemId;
        private string mPLMProject;
        private bool mExistingItem;
        private string mExternalItem;
        private string mNeedSExpeditedWorkflowWithSGS;
        private string mSGSExpeditedWorkflowApproved;
        private bool mPMReview2Submitted;

        #endregion

        #region Properties
        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        public string ProjectNumber { get { return mProjectNumber; } set { mProjectNumber = value; } }
        public string TimelineType { get { return mTimelineType; } set { mTimelineType = value; } }
        public string FirstShipDate { get { return mFirstShipDate; } set { mFirstShipDate = value; } }
        public string FirstProductionDate { get { return mFirstProductionDate; } set { mFirstProductionDate = value; } }
        public string ProjectType { get { return mProjectType; } set { mProjectType = value; } }
        public string CompassProjectType { get { return mCompassProjectType; } set { mCompassProjectType = value; } }
        public string ProjectTypeSubCategory { get { return mProjectTypeSubCategory; } set { mProjectTypeSubCategory = value; } }
        public string WorkflowPhase { get { return mWorkflowPhase; } set { mWorkflowPhase = value; } }
        public string ProjectName { get { return mProjectName; } set { mProjectName = value; } }
        public string ParentProjectNumber { get { return mParentProjectNumber; } set { mParentProjectNumber = value; } }
        public string StageGateProjectListItemId { get { return mStageGateProjectListItemId; } set { mStageGateProjectListItemId = value; } }
        public string PLMProject { get { return mPLMProject; } set { mPLMProject = value; } }
        public string ExternalItem { get { return mExternalItem; } set { mExternalItem = value; } }
        public bool ExistingItem { get { return mExistingItem; } set { mExistingItem = value; } }
        public string NeedSExpeditedWorkflowWithSGS { get { return mNeedSExpeditedWorkflowWithSGS; } set { mNeedSExpeditedWorkflowWithSGS = value; } }
        public string SGSExpeditedWorkflowApproved { get { return mSGSExpeditedWorkflowApproved; } set { mSGSExpeditedWorkflowApproved = value; } }
        public bool PMReview2Submitted { get { return mPMReview2Submitted; } set { mPMReview2Submitted = value; } }
        #endregion
    }
}
