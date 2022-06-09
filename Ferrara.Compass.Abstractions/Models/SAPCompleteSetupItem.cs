using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class SAPCompleteSetupItem
    {
        #region Variables
        private int mCompassListItemId;
        private string mLastUpdatedFormName;
        private string mProjectType;
        private string mSAPItemNumber;
        private string mSAPDescription;
        private string mImmediateSPKChange;
        private string mMakeLocation;
        private string mPackingLocation;
        private string mProcurementType;
        private string mDesignateHUBDC;
        private string mProductHierarchyLevel1;
        private string mProductHierarchyLevel2;
        private string mMaterialGroup1Brand;
        private string mMaterialGroup2Pricing;
        private string mMaterialGroup4ProductForm;
        private string mMaterialGroup5PackType;
        private string mProfitCenter;
        private string mPurchasedIntoLocation;
        private string mUnitUPC;
        private string mDisplayBoxUPC;
        private string mCaseUCC;
        private string mPalletUCC;
        private string mSAPBaseUOM;
        private string mDoubleStackable;
        private string mExternalManufacturer;
        private string mExternalPacker;
        private string mTransferSAPSpecsChangePackMeas;

        private string mCmpltFGMtrlMaster;
        private string mTurnkeyFGMMCrtd;
        private string mFGSAPSpChCmpltd;
        private string mCompleteTSBOM;
        private string mFGSAPSpecsChangePackMeas;
        #endregion

        #region Properties
        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        public string LastUpdatedFormName { get { return mLastUpdatedFormName; } set { mLastUpdatedFormName = value; } }
        public string ProjectType { get { return mProjectType; } set { mProjectType = value; } }
        public string SAPItemNumber { get { return mSAPItemNumber; } set { mSAPItemNumber = value; } }
        public string SAPDescription { get { return mSAPDescription; } set { mSAPDescription = value; } }
        public string ImmediateSPKChange { get { return mImmediateSPKChange; } set { mImmediateSPKChange = value; } }
        public string MakeLocation { get { return mMakeLocation; } set { mMakeLocation = value; } }
        public string PackingLocation { get { return mPackingLocation; } set { mPackingLocation = value; } }
        public string ProcurementType { get { return mProcurementType; } set { mProcurementType = value; } }
        public string DesignateHUBDC { get { return mDesignateHUBDC; } set { mDesignateHUBDC = value; } }
        public string MaterialGroup2Pricing { get { return mMaterialGroup2Pricing; } set { mMaterialGroup2Pricing = value; } }
        public string ProductHierarchyLevel1 { get { return mProductHierarchyLevel1; } set { mProductHierarchyLevel1 = value; } }
        public string ProductHierarchyLevel2 { get { return mProductHierarchyLevel2; } set { mProductHierarchyLevel2 = value; } }
        public string MaterialGroup1Brand { get { return mMaterialGroup1Brand; } set { mMaterialGroup1Brand = value; } }
        public string MaterialGroup4ProductForm { get { return mMaterialGroup4ProductForm; } set { mMaterialGroup4ProductForm = value; } }
        public string MaterialGroup5PackType { get { return mMaterialGroup5PackType; } set { mMaterialGroup5PackType = value; } }
        public string ProfitCenter { get { return mProfitCenter; } set { mProfitCenter = value; } }
        public string PurchasedIntoLocation { get { return mPurchasedIntoLocation; } set { mPurchasedIntoLocation = value; } }
        public string UnitUPC { get { return mUnitUPC; } set { mUnitUPC = value; } }
        public string DisplayBoxUPC { get { return mDisplayBoxUPC; } set { mDisplayBoxUPC = value; } }
        public string CaseUCC { get { return mCaseUCC; } set { mCaseUCC = value; } }
        public string PalletUCC { get { return mPalletUCC; } set { mPalletUCC = value; } }
        public string SAPBaseUOM { get { return mSAPBaseUOM; } set { mSAPBaseUOM = value; } }
        public string DoubleStackable { get { return mDoubleStackable; } set { mDoubleStackable = value; } }
        public string ExternalManufacturer { get { return mExternalManufacturer; } set { mExternalManufacturer = value; } }
        public string ExternalPacker { get { return mExternalPacker; } set { mExternalPacker = value; } }
        public string TransferSAPSpecsChangePackMeas { get { return mTransferSAPSpecsChangePackMeas; } set { mTransferSAPSpecsChangePackMeas = value; } }

        public string CmpltFGMtrlMaster { get { return mCmpltFGMtrlMaster; } set { mCmpltFGMtrlMaster = value; } }
        public string TurnkeyFGMMCrtd { get { return mTurnkeyFGMMCrtd; } set { mTurnkeyFGMMCrtd = value; } }
        public string FGSAPSpChCmpltd { get { return mFGSAPSpChCmpltd; } set { mFGSAPSpChCmpltd = value; } }
        public string CompleteTSBOM { get { return mCompleteTSBOM; } set { mCompleteTSBOM = value; } }
        public string FGSAPSpecsChangePackMeas { get { return mFGSAPSpecsChangePackMeas; } set { mFGSAPSpecsChangePackMeas = value; } }

        #endregion
    }
}
