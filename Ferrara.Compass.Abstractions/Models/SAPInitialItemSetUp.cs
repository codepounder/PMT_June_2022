using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class SAPInitialItemSetUp
    {
        public SAPInitialItemSetUp()
        {
            mRetailSellingUnitsBaseUOM = -9999;
            mRetailUnitWieghtOz = -9999;
        }

        private int mCompassListItemId;
        private string mNewIPF;
        private string mLastUpdatedFormName;
        private string mProjectType;

        private string mItemDescription;
        private string mSAPItemNumber;
        private string mSAPDescription;
        private string mProfitCenter;
        private string mSAPBaseUOM;
        private double mRetailUnitWieghtOz;
        private int mRetailSellingUnitsBaseUOM;
        private string mProductHierarchyLevel1;
        private string mProductHierarchyLevel2;
        private string mMaterialGroup1Brand;
        private string mMaterialGroup4ProductForm;
        private string mMaterialGroup5PackType;
        private string mUnitUPC;
        private string mDisplayBoxUPC;
        private string mCaseUCC;
        private string mPalletUCC;
        private string mMaterialGroup2Pricing;
        private string mRequireNewDisplayBoxUPC;
        private string mNewTransferSemi;
        private string mMfgLocationChange;
        private string mImmediateSPKChange;
        private string mPurchasedIntoLocation;
        private string mNoveltyProject;
        #region Deployment Information
        private string mDesignateHUBDC;
        private string mDeploymentModeofItem;
        #region SELL DCs
        private string mExtendtoSL07;
        private string mSetSL07SPKto;
        private string mExtendtoSL13;
        private string mSetSL13SPKto;
        private string mExtendtoSL18;
        private string mSetSL18SPKto;
        private string mExtendtoSL19;
        private string mSetSL19SPKto;
        private string mExtendtoSL30;
        private string mSetSL30SPKto;
        private string mExtendtoSL14;
        private string mSetSL14SPKto;
        #endregion
        #region FERQ DCs
        private string mExtendtoFQ26;
        private string mSetFQ26SPKto;
        private string mExtendtoFQ27;
        private string mSetFQ27SPKto;
        private string mExtendtoFQ28;
        private string mSetFQ28SPKto;
        private string mExtendtoFQ29;
        private string mSetFQ29SPKto;
        private string mExtendtoFQ34;
        private string mSetFQ34SPKto;
        private string mExtendtoFQ35;
        private string mSetFQ35SPKto;
        #endregion
        #endregion
        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        public string NewIPF { get { return mNewIPF; } set { mNewIPF = value; } }
        public string LastUpdatedFormName { get { return mLastUpdatedFormName; } set { mLastUpdatedFormName = value; } }
        public string ProjectType { get { return mProjectType; } set { mProjectType = value; } }

        public string ItemDescription { get { return mItemDescription; } set { mItemDescription = value; } }
        public string SAPItemNumber { get { return mSAPItemNumber; } set { mSAPItemNumber = value; } }
        public string SAPDescription { get { return mSAPDescription; } set { mSAPDescription = value; } }
        public string ProfitCenter { get { return mProfitCenter; } set { mProfitCenter = value; } }
        public string SAPBaseUOM { get { return mSAPBaseUOM; } set { mSAPBaseUOM = value; } }
        public double RetailUnitWieghtOz { get { return mRetailUnitWieghtOz; } set { mRetailUnitWieghtOz = value; } }
        public int RetailSellingUnitsBaseUOM { get { return mRetailSellingUnitsBaseUOM; } set { mRetailSellingUnitsBaseUOM = value; } }
        public string ProductHierarchyLevel1 { get { return mProductHierarchyLevel1; } set { mProductHierarchyLevel1 = value; } }
        public string ProductHierarchyLevel2 { get { return mProductHierarchyLevel2; } set { mProductHierarchyLevel2 = value; } }
        public string MaterialGroup1Brand { get { return mMaterialGroup1Brand; } set { mMaterialGroup1Brand = value; } }
        public string MaterialGroup2Pricing { get { return mMaterialGroup2Pricing; } set { mMaterialGroup2Pricing = value; } }
        public string MaterialGroup4ProductForm { get { return mMaterialGroup4ProductForm; } set { mMaterialGroup4ProductForm = value; } }
        public string MaterialGroup5PackType { get { return mMaterialGroup5PackType; } set { mMaterialGroup5PackType = value; } }
        public string UnitUPC { get { return mUnitUPC; } set { mUnitUPC = value; } }
        public string DisplayBoxUPC { get { return mDisplayBoxUPC; } set { mDisplayBoxUPC = value; } }
        public string CaseUCC { get { return mCaseUCC; } set { mCaseUCC = value; } }
        public string PalletUCC { get { return mPalletUCC; } set { mPalletUCC = value; } }
        public string RequireNewDisplayBoxUPC { get { return mRequireNewDisplayBoxUPC; } set { mRequireNewDisplayBoxUPC = value; } }
        public string NewTransferSemi { get { return mNewTransferSemi; } set { mNewTransferSemi = value; } }
        public string MfgLocationChange { get { return mMfgLocationChange; } set { mMfgLocationChange = value; } }
        public string ImmediateSPKChange { get { return mImmediateSPKChange; } set { mImmediateSPKChange = value; } }
        public string PurchasedIntoLocation { get { return mPurchasedIntoLocation; } set { mPurchasedIntoLocation = value; } }
        public string NoveltyProject { get { return mNoveltyProject; } set { mNoveltyProject = value; } }

        #region Deployment Information
        public string DesignateHUBDC { get { return mDesignateHUBDC; } set { mDesignateHUBDC = value; } }
        public string DeploymentModeofItem { get { return mDeploymentModeofItem; } set { mDeploymentModeofItem = value; } }
        #region SELL DCs
        public string ExtendtoSL07 { get { return mExtendtoSL07; } set { mExtendtoSL07 = value; } }
        public string SetSL07SPKto { get { return mSetSL07SPKto; } set { mSetSL07SPKto = value; } }
        public string ExtendtoSL13 { get { return mExtendtoSL13; } set { mExtendtoSL13 = value; } }
        public string SetSL13SPKto { get { return mSetSL13SPKto; } set { mSetSL13SPKto = value; } }
        public string ExtendtoSL18 { get { return mExtendtoSL18; } set { mExtendtoSL18 = value; } }
        public string SetSL18SPKto { get { return mSetSL18SPKto; } set { mSetSL18SPKto = value; } }
        public string ExtendtoSL19 { get { return mExtendtoSL19; } set { mExtendtoSL19 = value; } }
        public string SetSL19SPKto { get { return mSetSL19SPKto; } set { mSetSL19SPKto = value; } }
        public string ExtendtoSL30 { get { return mExtendtoSL30; } set { mExtendtoSL30 = value; } }
        public string SetSL30SPKto { get { return mSetSL30SPKto; } set { mSetSL30SPKto = value; } }
        public string ExtendtoSL14 { get { return mExtendtoSL14; } set { mExtendtoSL14 = value; } }
        public string SetSL14SPKto { get { return mSetSL14SPKto; } set { mSetSL14SPKto = value; } }
        #endregion
        #region FERQ DCs
        public string ExtendtoFQ26 { get { return mExtendtoFQ26; } set { mExtendtoFQ26 = value; } }
        public string SetFQ26SPKto { get { return mSetFQ26SPKto; } set { mSetFQ26SPKto = value; } }
        public string ExtendtoFQ27 { get { return mExtendtoFQ27; } set { mExtendtoFQ27 = value; } }
        public string SetFQ27SPKto { get { return mSetFQ27SPKto; } set { mSetFQ27SPKto = value; } }
        public string ExtendtoFQ28 { get { return mExtendtoFQ28; } set { mExtendtoFQ28 = value; } }
        public string SetFQ28SPKto { get { return mSetFQ28SPKto; } set { mSetFQ28SPKto = value; } }
        public string ExtendtoFQ29 { get { return mExtendtoFQ29; } set { mExtendtoFQ29 = value; } }
        public string SetFQ29SPKto { get { return mSetFQ29SPKto; } set { mSetFQ29SPKto = value; } }
        public string ExtendtoFQ34 { get { return mExtendtoFQ34; } set { mExtendtoFQ34 = value; } }
        public string SetFQ34SPKto { get { return mSetFQ34SPKto; } set { mSetFQ34SPKto = value; } }
        public string ExtendtoFQ35 { get { return mExtendtoFQ35; } set { mExtendtoFQ35 = value; } }
        public string SetFQ35SPKto { get { return mSetFQ35SPKto; } set { mSetFQ35SPKto = value; } }
        #endregion
        #endregion

    }
}
