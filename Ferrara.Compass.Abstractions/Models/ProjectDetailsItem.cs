using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferrara.Compass.Abstractions.Models
{
    public class ProjectDetailsItem
    {
        #region Member Variables
        private int mCompassItemId;
        private int mStageGateProjectListItemId;
        private string mProjectNumber;
        private string mParentProjectNumber;
        private string mProjectType;
        private string mProjectTypeSubCategory;
        private string mCommercializationLink;
        private string mSAPItemNumber;
        private string mSAPDescription;
        private string mRevisedFirstShipDate;
        private string mPackagingNumbers;
        private string mProjectWorkflowLink;
        private string mWorkflowPhase;
        private string mProductHierarchyLevel1;
        private string mMaterialGroup1Brand;
        private string mTimelineType;
        private string mManufacturingLocation;
        private string mPackingLocation;
        private string mPM;
        private string mPackagingEngineer;
        private string mInitiator;
        private string mInitiatorName;
        private string mCustomer;
        private string mSubmittedDate;
        private string mProjectTitle;
        private string mRequestedDate;
        private string mFirstProduction;
        private string mDueDate;
        private string mParent;
        private string mDesiredShipDate;
        private string mGate0ApprovedDate;
        private string mBrand;
        private string mSKUs;
        private string mStage;
        private string mProjectLeader;
        private string mParentProjectNumberLink;

        #endregion

        #region Properties
        public int CompassItemId { get { return mCompassItemId; } set { mCompassItemId = value; } }
        public int StageGateProjectListItemId { get { return mStageGateProjectListItemId; } set { mStageGateProjectListItemId = value; } }
        public string ProjectNumber { get { return mProjectNumber; } set { mProjectNumber = value; } }
        public string ParentProjectNumber { get { return mParentProjectNumber; } set { mParentProjectNumber = value; } }
        public string ProjectType { get { return mProjectType; } set { mProjectType = value; } }
        public string ProjectTypeSubCategory { get { return mProjectTypeSubCategory; } set { mProjectTypeSubCategory = value; } }
        public string CommercializationLink { get { return mCommercializationLink; } set { mCommercializationLink = value; } }
        public string SAPItemNumber { get { return mSAPItemNumber; } set { mSAPItemNumber = value; } }
        public string SAPDescription { get { return mSAPDescription; } set { mSAPDescription = value; } }
        public string RevisedFirstShipDate { get { return mRevisedFirstShipDate; } set { mRevisedFirstShipDate = value; } }
        public string PackagingNumbers { get { return mPackagingNumbers; } set { mPackagingNumbers = value; } }
        public string ProjectWorkflowLink { get { return mProjectWorkflowLink; } set { mProjectWorkflowLink = value; } }
        public string WorkflowPhase { get { return mWorkflowPhase; } set { mWorkflowPhase = value; } }
        public string ProductHierarchyLevel1 { get { return mProductHierarchyLevel1; } set { mProductHierarchyLevel1 = value; } }
        public string MaterialGroup1Brand { get { return mMaterialGroup1Brand; } set { mMaterialGroup1Brand = value; } }
        public string TimelineType { get { return mTimelineType; } set { mTimelineType = value; } }
        public string ManufacturingLocation { get { return mManufacturingLocation; } set { mManufacturingLocation = value; } }
        public string PackingLocation { get { return mPackingLocation; } set { mPackingLocation = value; } }
        public string PM { get { return mPM; } set { mPM = value; } }
        public string PackagingEngineer { get { return mPackagingEngineer; } set { mPackagingEngineer = value; } }
        public string Initiator { get { return mInitiator; } set { mInitiator = value; } }
        public string InitiatorName { get { return mInitiatorName; } set { mInitiatorName = value; } }
        public string Customer { get { return mCustomer; } set { mCustomer = value; } }
        public string SubmittedDate { get { return mSubmittedDate; } set { mSubmittedDate = value; } }
        public string ProjectTitle { get { return mProjectTitle; } set { mProjectTitle = value; } }
        public string RequestedDate { get { return mRequestedDate; } set { mRequestedDate = value; } }
        public string FirstProduction { get { return mFirstProduction; } set { mFirstProduction = value; } }
        public string DueDate { get { return mDueDate; } set { mDueDate = value; } }
        public string Parent { get { return mParent; } set { mParent = value; } }
        public string DesiredShipDate { get { return mDesiredShipDate; } set { mDesiredShipDate = value; } }
        public string Gate0ApprovedDate { get { return mGate0ApprovedDate; } set { mGate0ApprovedDate = value; } }
        public string Brand { get { return mBrand; } set { mBrand = value; } }
        public string SKUs { get { return mSKUs; } set { mSKUs = value; } }
        public string Stage { get { return mStage; } set { mStage = value; } }
        public string ProjectLeader { get { return mProjectLeader; } set { mProjectLeader = value; } }
        public string ParentProjectNumberLink { get { return mParentProjectNumberLink; } set { mParentProjectNumberLink = value; } }
        #endregion
    }
}
