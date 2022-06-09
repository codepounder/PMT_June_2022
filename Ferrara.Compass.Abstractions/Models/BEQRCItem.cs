using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    [Serializable]
    public class BEQRCItem
    {

        #region Variables
        private int mCompassListItemId;

        #region IPF Variables
        private string mProjectNumber;
        private string mTBDIndicator;
        private string mCustomer;
        private string mProjectType;
        private string mSAPItemNumber;
        private string mSAPDescription;
        private string mProductHierarchyLevel1;
        private string mProductHierarchyLevel2;
        private string mMaterialGroup1Brand;
        private string mMaterialGroup4ProductForm;
        private string mMaterialGroup5PackType;
        private string mConsumerFacingProdDesc;
        private DateTime mFirstShipDate;
        private DateTime mRevisedFirstShipDate;
        private double mAnnualProjectedDollars;
        private int mAnnualProjectedUnits;
        private double mExpectedGrossMarginPercent;
        private string mUnitUPC;
        private string mDisplayBoxUPC;

        private string mItemConcept;

        #endregion

        #endregion

        #region Properties
        #region General Properties
        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        #endregion

        #region IPF Properties
        public string ProjectNumber { get { return mProjectNumber; } set { mProjectNumber = value; } }
        public string TBDIndicator { get { return mTBDIndicator; } set { mTBDIndicator = value; } }
        public DateTime FirstShipDate { get { return mFirstShipDate; } set { mFirstShipDate = value; } }
        public DateTime RevisedFirstShipDate { get { return mRevisedFirstShipDate; } set { mRevisedFirstShipDate = value; } }
        public string Customer { get { return mCustomer; } set { mCustomer = value; } }
        public double AnnualProjectedDollars { get { return mAnnualProjectedDollars; } set { mAnnualProjectedDollars = value; } }
        public int AnnualProjectedUnits { get { return mAnnualProjectedUnits; } set { mAnnualProjectedUnits = value; } }
        public string ProjectType { get { return mProjectType; } set { mProjectType = value; } }
        public string SAPItemNumber { get { return mSAPItemNumber; } set { mSAPItemNumber = value; } }
        public string SAPDescription { get { return mSAPDescription; } set { mSAPDescription = value; } }
        public string ProductHierarchyLevel1 { get { return mProductHierarchyLevel1; } set { mProductHierarchyLevel1 = value; } }
        public string ProductHierarchyLevel2 { get { return mProductHierarchyLevel2; } set { mProductHierarchyLevel2 = value; } }
        public string ItemConcept { get { return mItemConcept; } set { mItemConcept = value; } }
        public double ExpectedGrossMarginPercent { get { return mExpectedGrossMarginPercent; } set { mExpectedGrossMarginPercent = value; } }
        public string MaterialGroup1Brand { get { return mMaterialGroup1Brand; } set { mMaterialGroup1Brand = value; } }
        public string MaterialGroup4ProductForm { get { return mMaterialGroup4ProductForm; } set { mMaterialGroup4ProductForm = value; } }
        public string MaterialGroup5PackType { get { return mMaterialGroup5PackType; } set { mMaterialGroup5PackType = value; } }
        public string ConsumerFacingProdDesc { get { return mConsumerFacingProdDesc; } set { mConsumerFacingProdDesc = value; } }

        public string UnitUPC { get { return mUnitUPC; } set { mUnitUPC = value; } }
        public string DisplayBoxUPC { get { return mDisplayBoxUPC; } set { mDisplayBoxUPC = value; } }
        #endregion

        #endregion
    }
}
