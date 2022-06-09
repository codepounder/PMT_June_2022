using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class SAPMaterialMasterListItem
    {
        #region Variables
        private string mSAPItemNumber;
        private string mSAPDescription;
        private string mCaseType;
        private string mTruckLoadPricePerSellingUnit;
        private string mCandySemiNumber;
        private string mLast12MonthSales;
        private string mProductHierarchyLevel1;
        private string mProductHierarchyLevel2;
        private string mMaterialGroup1Brand;
        private string mMaterialGroup4ProductForm;
        private string mMaterialGroup5PackType;
        private string mRetailSellingUnitsBaseUOM;
        private string mRetailUnitWieghtOz;
        private string mUnitUPC;
        private string mDisplayBoxUPC;
        private string mCaseUCC;
        private string mPalletUCC;
        private string mMaterialType;
        private string mMaterialType2;
        #endregion

        #region
        public string CaseType { get { return mCaseType; } set { mCaseType = value; } }
        public string TruckLoadPricePerSellingUnit { get { return mTruckLoadPricePerSellingUnit; } set { mTruckLoadPricePerSellingUnit = value; } }
        public string CandySemiNumber { get { return mCandySemiNumber; } set { mCandySemiNumber = value; } }
        public string Last12MonthSales { get { return mLast12MonthSales; } set { mLast12MonthSales = value; } }
        public string SAPItemNumber { get { return mSAPItemNumber; } set { mSAPItemNumber = value; } }
        public string SAPDescription { get { return mSAPDescription; } set { mSAPDescription = value; } }
        public string ProductHierarchyLevel1 { get { return mProductHierarchyLevel1; } set { mProductHierarchyLevel1 = value; } }
        public string ProductHierarchyLevel2 { get { return mProductHierarchyLevel2; } set { mProductHierarchyLevel2 = value; } }
        public string MaterialGroup1Brand { get { return mMaterialGroup1Brand; } set { mMaterialGroup1Brand = value; } }
        public string MaterialGroup4ProductForm { get { return mMaterialGroup4ProductForm; } set { mMaterialGroup4ProductForm = value; } }
        public string MaterialGroup5PackType { get { return mMaterialGroup5PackType; } set { mMaterialGroup5PackType = value; } }
        public string RetailSellingUnitsBaseUOM { get { return mRetailSellingUnitsBaseUOM; } set { mRetailSellingUnitsBaseUOM = value; } }
        public string RetailUnitWieghtOz { get { return mRetailUnitWieghtOz; } set { mRetailUnitWieghtOz = value; } }
        public string UnitUPC { get { return mUnitUPC; } set { mUnitUPC = value; } }
        public string DisplayBoxUPC { get { return mDisplayBoxUPC; } set { mDisplayBoxUPC = value; } }
        public string CaseUCC { get { return mCaseUCC; } set { mCaseUCC = value; } }
        public string PalletUCC { get { return mPalletUCC; } set { mPalletUCC = value; } }
        public string MaterialType { get { return mMaterialType; } set { mMaterialType = value; } }
        public string MaterialType2 { get { return mMaterialType2; } set { mMaterialType2 = value; } }
        #endregion
    }
}
