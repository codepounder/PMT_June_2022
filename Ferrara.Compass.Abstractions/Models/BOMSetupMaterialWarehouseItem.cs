using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class BOMSetupMaterialWarehouseItem
    {
        #region Variables
        private int mCompassListItemId;
        private string mLastUpdatedFormName;
        private string mProjectType;
        private string mProjectTypeSubCategory;
        private string mItemConcept;
        private string mSAPItemNumber;
        private string mSAPDescription;
        private string mLineWorkcenterAdditionalInfo;
        private string mFinishedGoodPackLocation;
        private string mIsPegHoleNeeded;
        private string mFGLikeItem;
        private string mProjectInitiator;
        private string mProjectInitiatorName;
        private string mBrandManager;
        private string mBrandManagerName;
        private string mPM;
        private string mPMName;
        private string mInTechManager;
        private string mInTechManagerName;
        private string mPackagingEngineer;
        private string mPackagingEngineerName;
        private string mMakeLocation;
        private string mPackingLocation;
        private string mPurchasedIntoLocation;
        private string mProcurementType;
        private string mExternalManufacturer;
        private string mExternalPacker;
        private string mSAPBaseUOM;
        private string mDesignateHUBDC;
        
        #endregion

        #region Properties
        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        public string LastUpdatedFormName { get { return mLastUpdatedFormName; } set { mLastUpdatedFormName = value; } }
        public string ProjectType { get { return mProjectType; } set { mProjectType = value; } }
        public string ProjectTypeSubCategory { get { return mProjectTypeSubCategory; } set { mProjectTypeSubCategory = value; } }
        public string ItemConcept { get { return mItemConcept; } set { mItemConcept = value; } }
        public string SAPItemNumber { get { return mSAPItemNumber; } set { mSAPItemNumber = value; } }
        public string SAPDescription { get { return mSAPDescription; } set { mSAPDescription = value; } }
        public string LineWorkcenterAdditionalInfo { get { return mLineWorkcenterAdditionalInfo; } set { mLineWorkcenterAdditionalInfo = value; } }
        public string FinishedGoodPackLocation { get { return mFinishedGoodPackLocation; } set { mFinishedGoodPackLocation = value; } }
        public string IsPegHoleNeeded { get { return mIsPegHoleNeeded; } set { mIsPegHoleNeeded = value; } }
        public string FGLikeItem { get { return mFGLikeItem; } set { mFGLikeItem = value; } }
        public string ProjectInitiator { get { return mProjectInitiator; } set { mProjectInitiator = value; } }
        public string ProjectInitiatorName { get { return mProjectInitiatorName; } set { mProjectInitiatorName = value; } }
        public string BrandManager { get { return mBrandManager; } set { mBrandManager = value; } }
        public string BrandManagerName { get { return mBrandManagerName; } set { mBrandManagerName = value; } }
        public string PM { get { return mPM; } set { mPM = value; } }
        public string PMName { get { return mPMName; } set { mPMName = value; } }
        public string InTechManager { get { return mInTechManager; } set { mInTechManager = value; } }
        public string InTechManagerName { get { return mInTechManagerName; } set { mInTechManagerName = value; } }
        public string PackagingEngineer { get { return mPackagingEngineer; } set { mPackagingEngineer = value; } }
        public string PackagingEngineerName { get { return mPackagingEngineerName; } set { mPackagingEngineerName = value; } }
        public string MakeLocation { get { return mMakeLocation; } set { mMakeLocation = value; } }
        public string PackingLocation { get { return mPackingLocation; } set { mPackingLocation = value; } }
        public string PurchasedIntoLocation { get { return mPurchasedIntoLocation; } set { mPurchasedIntoLocation = value; } }
        public string ProcurementType { get { return mProcurementType; } set { mProcurementType = value; } }
        public string ExternalManufacturer { get { return mExternalManufacturer; } set { mExternalManufacturer = value; } }
        public string ExternalPacker { get { return mExternalPacker; } set { mExternalPacker = value; } }
        public string SAPBaseUOM { get { return mSAPBaseUOM; } set { mSAPBaseUOM = value; } }
        public string DesignateHUBDC { get { return mDesignateHUBDC; } set { mDesignateHUBDC = value; } }
        
        #endregion
    }
}
