namespace Ferrara.Compass.ProjectTimelineCalculator.Models
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
        private bool mExistingItem;
        private string mRevisedFirstShipDate;
        private string mIPFSubmitted;

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
        public bool ExistingItem { get { return mExistingItem; } set { mExistingItem = value; } }
        public string IPFSubmitted { get { return mIPFSubmitted; } set { mIPFSubmitted = value; } }
        public string RevisedFirstShipDate { get { return mRevisedFirstShipDate; } set { mRevisedFirstShipDate = value; } }

        #endregion
    }
}
