using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class BOMSetupProjectSummaryItem
    {
        #region Variables
        private int mCompassListItemId;

        private string mProjectType;
        private string mProjectSubCategory;
        private string mSAPItemNumber;
        private string mSAPDescription;
        private string mPackingLocation;
        private string mWorkCenterAddInfo;
        private string mPegHoleNeed;
        private string mItemConcept;
        private string mFGLikeItem;

        private string mInitiatorName;
        private string mMarketingName;
        private string mInTechManagerName;
        private string mPMName;
        private string mPackagingEngineerName;//?????

        private string mMakeLocation;
        private string mPurchasedIntoLocation;
        private string mProcurementType;
        private string mExternalManufacturer;
        private string mExternalPacker;
        private string mSAPBaseUOM;
        private string mDesignateHUBDC;

        //Graphics Fields
        private string mAllAspectsApprovedFromPEPersp;
        private string mWhatIsIncorrectPE;
        #endregion

        #region Properties
        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }

        public string ProjectType { get { return mProjectType; } set { mProjectType = value; } }
        public string ProjectSubCategory { get { return mProjectSubCategory; } set { mProjectSubCategory = value; } }
        public string SAPItemNumber { get { return mSAPItemNumber; } set { mSAPItemNumber = value; } }
        public string SAPDescription { get { return mSAPDescription; } set { mSAPDescription = value; } }
        public string PackingLocation { get { return mPackingLocation; } set { mPackingLocation = value; } }
        public string WorkCenterAddInfo { get { return mWorkCenterAddInfo; } set { mWorkCenterAddInfo = value; } }
        public string PegHoleNeeded { get { return mPegHoleNeed; } set { mPegHoleNeed = value; } }
        public string ItemConcept { get { return mItemConcept; } set { mItemConcept = value; } }
        public string FGLikeItem { get { return mFGLikeItem; } set { mFGLikeItem = value; } }

        public string InitiatorName { get { return mInitiatorName; } set { mInitiatorName = value; } }
        public string MarketingName { get { return mMarketingName; } set { mMarketingName = value; } }
        public string PMName { get { return mPMName; } set { mPMName = value; } }
        public string InTechManagerName { get { return mInTechManagerName; } set { mInTechManagerName = value; } }
        public string PackagingEngineerName { get { return mPackagingEngineerName; } set { mPackagingEngineerName = value; } }//?????
        public string MakeLocation { get { return mMakeLocation; } set { mMakeLocation = value; } }
        public string PurchasedIntoLocation { get { return mPurchasedIntoLocation; } set { mPurchasedIntoLocation = value; } }
        public string ProcurementType { get { return mProcurementType; } set { mProcurementType = value; } }
        public string ExternalManufacturer { get { return mExternalManufacturer; } set { mExternalManufacturer = value; } }
        public string ExternalPacker { get { return mExternalPacker; } set { mExternalPacker = value; } }
        public string SAPBaseUOM { get { return mSAPBaseUOM; } set { mSAPBaseUOM = value; } }
        public string DesignateHUBDC { get { return mDesignateHUBDC; } set { mDesignateHUBDC = value; } }

        //Graphics Fields
        public string AllAspectsApprovedFromPEPersp { get { return mAllAspectsApprovedFromPEPersp; } set { mAllAspectsApprovedFromPEPersp = value; } }
        public string WhatIsIncorrectPE { get { return mWhatIsIncorrectPE; } set { mWhatIsIncorrectPE = value; } }
        #endregion
    }
}
