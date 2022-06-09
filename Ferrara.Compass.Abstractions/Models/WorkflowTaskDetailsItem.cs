using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class WorkflowTaskDetailsItem
    {
        private int mCompassId;
        private string mFirstProductionDate;
        private string mTitle;
        private string mProjectNumber;
        private string mProjectType;
        private string mProjectTypeSubcategory;
        private string mMaterialGroup1Brand;
        private string mInitiator;
        private string mInitiatorName;
        private string mFormUrl;
        private string mDueDate;
        private string mFirstShipDate;
        private string mItemDescription;
        private string mRequestedDate;
        private string mNewPackagingComponents;
        private string mPackagingCompInitialSetup;
        private string mPackagingCompSAPsetupBOM;
        private string mAssignedToId;
        private string mStatus;
        private string mWorkflowStep;
        private string mTaskName;
        private string mProductHierarchyLevel1;
        private int mDueDateCalc;

        public int CompassId { get { return mCompassId; } set { mCompassId = value; } }
        public string FirstProductionDate { get { return mFirstProductionDate; } set { mFirstProductionDate = value; } }
        public string Title { get { return mTitle; } set { mTitle = value; } }
        public string ProjectNumber { get { return mProjectNumber; } set { mProjectNumber = value; } }
        public string ProjectType { get { return mProjectType; } set { mProjectType = value; } }
        public string ProjectTypeSubcategory { get { return mProjectTypeSubcategory; } set { mProjectTypeSubcategory = value; } }
        public string MaterialGroup1Brand { get { return mMaterialGroup1Brand; } set { mMaterialGroup1Brand = value; } }
        public string Initiator { get { return mInitiator; } set { mInitiator = value; } }
        public string InitiatorName { get { return mInitiatorName; } set { mInitiatorName = value; } }
        public string FormUrl { get { return mFormUrl; } set { mFormUrl = value; } }
        public string DueDate { get { return mDueDate; } set { mDueDate = value; } }
        public string FirstShipDate { get { return mFirstShipDate; } set { mFirstShipDate = value; } }
        public string ItemDescription { get { return mItemDescription; } set { mItemDescription = value; } }
        public string RequestedDate { get { return mRequestedDate; } set { mRequestedDate = value; } }
        public string NewPackagingComponents { get { return mNewPackagingComponents; } set { mNewPackagingComponents = value; } }
        public string PackagingCompInitialSetup { get { return mPackagingCompInitialSetup; } set { mPackagingCompInitialSetup = value; } }
        public string PackagingCompSAPsetupBOM { get { return mPackagingCompSAPsetupBOM; } set { mPackagingCompSAPsetupBOM = value; } }
        public string AssignedToId { get { return mAssignedToId; } set { mAssignedToId = value; } }
        public string Status { get { return mStatus; } set { mStatus = value; } }
        public string WorkflowStep { get { return mWorkflowStep; } set { mWorkflowStep = value; } }
        public string TaskName { get { return mTaskName; } set { mTaskName = value; } }
        public string ProductHierarchyLevel1 { get { return mProductHierarchyLevel1; } set { mProductHierarchyLevel1 = value; } }
        public int DueDateCalc { get { return mDueDateCalc; } set { mDueDateCalc = value; } }
        
    }
}
