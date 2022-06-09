using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class SAPBOMSetupItem
    {
        #region Variables
        private int mCompassListItemId;
        private string mLastUpdatedFormName;
        private string mProjectType;
        private string mSAPItemNumber;
        private string mSAPDescription;
        private string mTBDIndicator;
        private string mProjectTypeSubCategory;
        private string mMfgLocationChange;
        private string mItemConcept;

        private string mNewMaterialNumbersCreated;
        private string mContBuildFGBOM;
        private string mTransferSemiBOMSetup;
        private string mFinishedGoodBOMSetup;
        private string mTransferMatNumCreatd;
        private string mHardSoftTransition;
        private string mTransferSAPSpecsChangePackMeas;
        private string mFGSAPSpecsChangePackMeas;
        private string mTransferSAPSpecsChangeCompleted;
        private string mFGSAPSpecsChangeCompleted;
        private string mTurnkeyFGMMCreated;
        private string mCompleteFGBOMCreated;
        private string mCompleteTSBOMCreated;
        private string mPackMatsCreatedInPackLoc;
        private string mFGBOMCreatedInNewPackLoc;
        private string mSPKUpdatedInDCsPack;
        private string mTSCompsCreatedInNewMPLoc;
        private string mTSFGBOMCreatedInNewMakeLoc;
        private string mSPKUpdatedInDCsMake;
        private string mProdVersionCreated;
        private string mCreateNewPURCNDYSAPMatNum;
        private string mNewFGBOMCreated;
        private string mNewTSMaterialNumbersCreated;
        private string mNewTSCompPackNumsCreated;
        private string mInitialFGBOMCreated;
        private string mInitialTSBOMCreated;
        private string mFGSubConBOMCreated;
        private string mExtendFGToDCs;
        private string mVerifyFGBOMInDCs;
        private string mGS1Calculator;

        private string mImmediateSPKChange;
        private string mMakeLocation;
        private string mPackingLocation;
        private string mProcurementType;
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
        private string mFGPrivateLable;
        private string mFGDCFP07;
        private string mFGDCFP13;
        private string mFGSPKOthers;
        private string mVerifyPrivateLabel;
        private string mVerifyFGDCFP07;
        private string mVerifyFGDCFP13;
        private string mVerifyFGSPKOthers;
        private string mAddZSTOMaterialsEntry;
        private string mCmpltFGMtrlMaster;
        private string mOpenSalesFPCO;
        private string mOpenSalesSELL;
        private string mOpenSalesFERQ;
        private string mTSCompsExtendedInNewMPLoc;
        private string mSPKsUpdatedPerDeployment;
        private string mExtProfitCenterToDC;
        private string mClckNewTSPCPrftCntr;
        private string mTurnkeyFGMMCrtd;
        private string mPLMProject;
        private string mExtendFGHL12Brand;
        private string mApplySemiHL12Brand;
        private string mAddOldComp;
        private string mEmptyTurnkeyAtFC01;
        private string mEmptyTurnkeyPCsAtFC01;
        private string mExtPCToSalesOrg0001;
        private string mExtTSToSalesOrg0001;
        private string mExtTSToSalesOrg1000;
        private string mExtPCToSalesOrg1000;
        private string mExtTSToSalesOrg2000;
        private string mExtFGToSalesOrg0001;
        private string mExtFGToSalesOrg1000;
        private string mExtFGToCompSale2000;
        private string mExtFGToSalesOrg2000;
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
        #endregion

        #region Properties
        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        public string LastUpdatedFormName { get { return mLastUpdatedFormName; } set { mLastUpdatedFormName = value; } }
        public string ProjectType { get { return mProjectType; } set { mProjectType = value; } }
        public string SAPItemNumber { get { return mSAPItemNumber; } set { mSAPItemNumber = value; } }
        public string SAPDescription { get { return mSAPDescription; } set { mSAPDescription = value; } }
        public string TBDIndicator { get { return mTBDIndicator; } set { mTBDIndicator = value; } }
        public string ProjectTypeSubCategory { get { return mProjectTypeSubCategory; } set { mProjectTypeSubCategory = value; } }
        public string MfgLocationChange { get { return mMfgLocationChange; } set { mMfgLocationChange = value; } }
        public string ItemConcept { get { return mItemConcept; } set { mItemConcept = value; } }
        public string NewMaterialNumbersCreated { get { return mNewMaterialNumbersCreated; } set { mNewMaterialNumbersCreated = value; } }
        public string ContBuildFGBOM { get { return mContBuildFGBOM; } set { mContBuildFGBOM = value; } }
        public string TransferSemiBOMSetup { get { return mTransferSemiBOMSetup; } set { mTransferSemiBOMSetup = value; } }
        public string FinishedGoodBOMSetup { get { return mFinishedGoodBOMSetup; } set { mFinishedGoodBOMSetup = value; } }
        public string TransferMatNumCreatd { get { return mTransferMatNumCreatd; } set { mTransferMatNumCreatd = value; } }
        public string TransferSAPSpecsChangeCompleted { get { return mTransferSAPSpecsChangeCompleted; } set { mTransferSAPSpecsChangeCompleted = value; } }
        public string FGSAPSpecsChangeCompleted { get { return mFGSAPSpecsChangeCompleted; } set { mFGSAPSpecsChangeCompleted = value; } }
        public string TransferSAPSpecsChangePackMeas { get { return mTransferSAPSpecsChangePackMeas; } set { mTransferSAPSpecsChangePackMeas = value; } }
        public string FGSAPSpecsChangePackMeas { get { return mFGSAPSpecsChangePackMeas; } set { mFGSAPSpecsChangePackMeas = value; } }
        public string HardSoftTransition { get { return mHardSoftTransition; } set { mHardSoftTransition = value; } }
        public string ImmediateSPKChange { get { return mImmediateSPKChange; } set { mImmediateSPKChange = value; } }
        public string MakeLocation { get { return mMakeLocation; } set { mMakeLocation = value; } }
        public string PackingLocation { get { return mPackingLocation; } set { mPackingLocation = value; } }
        public string ProcurementType { get { return mProcurementType; } set { mProcurementType = value; } }
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
        public string TurnkeyFGMMCreated { get { return mTurnkeyFGMMCreated; } set { mTurnkeyFGMMCreated = value; } }
        public string CompleteFGBOMCreated { get { return mCompleteFGBOMCreated; } set { mCompleteFGBOMCreated = value; } }
        public string CompleteTSBOMCreated { get { return mCompleteTSBOMCreated; } set { mCompleteTSBOMCreated = value; } }
        public string PackMatsCreatedInPackLoc { get { return mPackMatsCreatedInPackLoc; } set { mPackMatsCreatedInPackLoc = value; } }
        public string FGBOMCreatedInNewPackLoc { get { return mFGBOMCreatedInNewPackLoc; } set { mFGBOMCreatedInNewPackLoc = value; } }
        public string SPKUpdatedInDCsPack { get { return mSPKUpdatedInDCsPack; } set { mSPKUpdatedInDCsPack = value; } }
        public string TSCompsCreatedInNewMPLoc { get { return mTSCompsCreatedInNewMPLoc; } set { mTSCompsCreatedInNewMPLoc = value; } }
        public string TSFGBOMCreatedInNewMakeLoc { get { return mTSFGBOMCreatedInNewMakeLoc; } set { mTSFGBOMCreatedInNewMakeLoc = value; } }
        public string SPKUpdatedInDCsMake { get { return mSPKUpdatedInDCsMake; } set { mSPKUpdatedInDCsMake = value; } }
        public string ProdVersionCreated { get { return mProdVersionCreated; } set { mProdVersionCreated = value; } }
        public string CreateNewPURCNDYSAPMatNum { get { return mCreateNewPURCNDYSAPMatNum; } set { mCreateNewPURCNDYSAPMatNum = value; } }
        public string NewFGBOMCreated { get { return mNewFGBOMCreated; } set { mNewFGBOMCreated = value; } }
        public string NewTSMaterialNumbersCreated { get { return mNewTSMaterialNumbersCreated; } set { mNewTSMaterialNumbersCreated = value; } }
        public string NewTSCompPackNumsCreated { get { return mNewTSCompPackNumsCreated; } set { mNewTSCompPackNumsCreated = value; } }
        public string InitialFGBOMCreated { get { return mInitialFGBOMCreated; } set { mInitialFGBOMCreated = value; } }
        public string InitialTSBOMCreated { get { return mInitialTSBOMCreated; } set { mInitialTSBOMCreated = value; } }
        public string FGSubConBOMCreated { get { return mFGSubConBOMCreated; } set { mFGSubConBOMCreated = value; } }
        public string ExtendFGToDCs { get { return mExtendFGToDCs; } set { mExtendFGToDCs = value; } }
        public string VerifyFGBOMInDCs { get { return mVerifyFGBOMInDCs; } set { mVerifyFGBOMInDCs = value; } }
        public string GS1Calculator { get { return mGS1Calculator; } set { mGS1Calculator = value; } }
        public string FGPrivateLable { get { return mFGPrivateLable; } set { mFGPrivateLable = value; } }
        public string FGDCFP07 { get { return mFGDCFP07; } set { mFGDCFP07 = value; } }
        public string FGDCFP13 { get { return mFGDCFP13; } set { mFGDCFP13 = value; } }
        public string FGSPKOthers { get { return mFGSPKOthers; } set { mFGSPKOthers = value; } }
        public string VerifyPrivateLabel { get { return mVerifyPrivateLabel; } set { mVerifyPrivateLabel = value; } }
        public string VerifyFGDCFP07 { get { return mVerifyFGDCFP07; } set { mVerifyFGDCFP07 = value; } }
        public string VerifyFGDCFP13 { get { return mVerifyFGDCFP13; } set { mVerifyFGDCFP13 = value; } }
        public string VerifyFGSPKOthers { get { return mVerifyFGSPKOthers; } set { mVerifyFGSPKOthers = value; } }
        public string AddZSTOMatEntry { get { return mAddZSTOMaterialsEntry; } set { mAddZSTOMaterialsEntry = value; } }
        public string CmpltFGMtrlMaster { get { return mCmpltFGMtrlMaster; } set { mCmpltFGMtrlMaster = value; } }
        public string OpenSalesFPCO { get { return mOpenSalesFPCO; } set { mOpenSalesFPCO = value; } }
        public string OpenSalesSELL { get { return mOpenSalesSELL; } set { mOpenSalesSELL = value; } }
        public string OpenSalesFERQ { get { return mOpenSalesFERQ; } set { mOpenSalesFERQ = value; } }

        public string TSCompsExtendedInNewMPLoc { get { return mTSCompsExtendedInNewMPLoc; } set { mTSCompsExtendedInNewMPLoc = value; } }
        public string SPKsUpdatedPerDeployment { get { return mSPKsUpdatedPerDeployment; } set { mSPKsUpdatedPerDeployment = value; } }
        public string ExtProfitCenterToDC { get { return mExtProfitCenterToDC; } set { mExtProfitCenterToDC = value; } }
        public string ClckNewTSPCPrftCntr { get { return mClckNewTSPCPrftCntr; } set { mClckNewTSPCPrftCntr = value; } }
        public string TurnkeyFGMMCrtd { get { return mTurnkeyFGMMCrtd; } set { mTurnkeyFGMMCrtd = value; } }
        public string PLMProject { get { return mPLMProject; } set { mPLMProject = value; } }
        public string DesignateHUBDC { get { return mDesignateHUBDC; } set { mDesignateHUBDC = value; } }
        public string DeploymentModeofItem { get { return mDeploymentModeofItem; } set { mDeploymentModeofItem = value; } }
        public string ExtendFGHL12Brand { get { return mExtendFGHL12Brand; } set { mExtendFGHL12Brand = value; } }
        public string ApplySemiHL12Brand { get { return mApplySemiHL12Brand; } set { mApplySemiHL12Brand = value; } }
        public string AddOldComp { get { return mAddOldComp; } set { mAddOldComp = value; } }

        public string EmptyTurnkeyAtFC01 { get { return mEmptyTurnkeyAtFC01; } set { mEmptyTurnkeyAtFC01 = value; } }
        public string EmptyTurnkeyPCsAtFC01 { get { return mEmptyTurnkeyPCsAtFC01; } set { mEmptyTurnkeyPCsAtFC01 = value; } }
        public string ExtPCToSalesOrg0001 { get { return mExtPCToSalesOrg0001; } set { mExtPCToSalesOrg0001 = value; } }
        public string ExtTSToSalesOrg0001 { get { return mExtTSToSalesOrg0001; } set { mExtTSToSalesOrg0001 = value; } }
        public string ExtTSToSalesOrg1000 { get { return mExtTSToSalesOrg1000; } set { mExtTSToSalesOrg1000 = value; } }
        public string ExtPCToSalesOrg1000 { get { return mExtPCToSalesOrg1000; } set { mExtPCToSalesOrg1000 = value; } }
        public string ExtTSToSalesOrg2000 { get { return mExtTSToSalesOrg2000; } set { mExtTSToSalesOrg2000 = value; } }
        public string ExtFGToSalesOrg0001 { get { return mExtFGToSalesOrg0001; } set { mExtFGToSalesOrg0001 = value; } }
        public string ExtFGToSalesOrg1000 { get { return mExtFGToSalesOrg1000; } set { mExtFGToSalesOrg1000 = value; } }
        public string ExtFGToCompSale2000 { get { return mExtFGToCompSale2000; } set { mExtFGToCompSale2000 = value; } }
        public string ExtFGToSalesOrg2000 { get { return mExtFGToSalesOrg2000; } set { mExtFGToSalesOrg2000 = value; } }

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
