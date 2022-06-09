using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class BillofMaterialsItem
    {
        #region Variables
        private int mCompassListItemId;
        private string mLastUpdatedFormName;
        private string mProjectType;
        private string mProjectSubcat;
        // Read-only fields
        private string mPackingLocation;
        private string mInitiator;
        private string mMarketing;
        private string mMarketingName;
        private string mInTech;
        private string mInTechName;
        private string mPM;
        private string mPMName;
        private string mWorkCenterAddInfo;

        // Bill of Materails fields
        private string mPackagingEngineerLead;
        private string mPackagingEngineerLeadName;
        private string mPackagingEngineering;
        private string mPackagingEngineeringName;
        private string mPackagingNumbers;
        private string mSAPItemNumber;
        private string mSAPDescription;
        private string mPegHoleNeed;
        private string mItemConcept;
        private string mFGLikeItem;
        private string mNewExistingItem;

        //Graphics Fields
        private string mAllAspectsApprovedFromPEPersp;
        private string mWhatIsIncorrectPE;
        private string mIsAllProcInfoCorrect;
        private string mWhatProcInfoHasChanged;

        private string mPLMProject;
        #endregion

        #region Properties
        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        public string LastUpdatedFormName { get { return mLastUpdatedFormName; } set { mLastUpdatedFormName = value; } }
        public string ProjectType { get { return mProjectType; } set { mProjectType = value; } }
        public string ProjectSubcat { get { return mProjectSubcat; } set { mProjectSubcat = value; } }
        
        public string PackingLocation { get { return mPackingLocation; } set { mPackingLocation = value; } }
        public string Initiator { get { return mInitiator; } set { mInitiator = value; } }
        public string Marketing { get { return mMarketing; } set { mMarketing = value; } }
        public string MarketingName { get { return mMarketingName; } set { mMarketingName = value; } }
        public string PM { get { return mPM; } set { mPM = value; } }
        public string PMName { get { return mPMName; } set { mPMName = value; } }
        public string InTech { get { return mInTech; } set { mInTech = value; } }
        public string InTechName { get { return mInTechName; } set { mInTechName = value; } }
        public string PackagingEngineerLead { get { return mPackagingEngineerLead; } set { mPackagingEngineerLead = value; } }
        public string PackagingEngineerLeadName { get { return mPackagingEngineerLeadName; } set { mPackagingEngineerLeadName = value; } }
        public string PackagingEngineering { get { return mPackagingEngineering; } set { mPackagingEngineering = value; } }
        public string PackagingEngineeringName { get { return mPackagingEngineeringName; } set { mPackagingEngineeringName = value; } }
        public string PackagingNumbers { get { return mPackagingNumbers; } set { mPackagingNumbers = value; } }
        public string SAPItemNumber { get { return mSAPItemNumber; } set { mSAPItemNumber = value; } }
        public string SAPDescription { get { return mSAPDescription; } set { mSAPDescription = value; } }
        public string WorkCenterAddInfo { get { return mWorkCenterAddInfo; } set { mWorkCenterAddInfo = value; } }
        public string PegHoleNeeded { get { return mPegHoleNeed; } set { mPegHoleNeed = value; } }
        public string ItemConcept { get { return mItemConcept; } set { mItemConcept = value; } }
        public string FGLikeItem { get { return mFGLikeItem; } set { mFGLikeItem = value; } }
        public string NewExistingItem { get { return mNewExistingItem; } set { mNewExistingItem = value; } }
        public string PLMProject { get { return mPLMProject; } set { mPLMProject = value; } }
        //Graphics Fields
        public string AllAspectsApprovedFromPEPersp { get { return mAllAspectsApprovedFromPEPersp; } set { mAllAspectsApprovedFromPEPersp = value; } }
        public string WhatIsIncorrectPE { get { return mWhatIsIncorrectPE; } set { mWhatIsIncorrectPE = value; } }

        #endregion
    }
}
