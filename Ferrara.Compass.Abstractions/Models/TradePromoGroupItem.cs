using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class TradePromoGroupItem
    {
        #region Variables
        private int mCompassListItemId;
        private string mLastUpdatedFormName;
        private string mProjectType;

        private string mItemConcept;
        private string mProductHierarchyLevel1;
        private string mProductHierarchyLevel2;
        private string mMaterialGroup1Brand;
        private string mMaterialGroup4ProductForm;
        private string mMaterialGroup5PackType;
        private string mMaterialGroup2Pricing;
        #endregion

        #region Properties
        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        public string LastUpdatedFormName { get { return mLastUpdatedFormName; } set { mLastUpdatedFormName = value; } }
        public string ProjectType { get { return mProjectType; } set { mProjectType = value; } }

        private double mTruckLoadPricePerSellingUnit;
        private double mTruckloadPriceperBaseUOM;
        private int mRetailSellingUnitsBaseUOM;
        private string mSAPBaseUOM;
        private string mCustomerSpecific;
        private string mCustomer;
        private string mChannel;
        private string mCaseType;
        private string mUnitsInsideCarton;
        private double mIndividualPouchWeight;
        private int mNumberofTraysPerBaseUOM;
        private double mRetailUnitWieghtOz;
        private double mBaseUOMNetWeightLbs;
        private string mInitialEstimatedBracketPricing;
        private string mInitialEstimatedPricing;

        public string ItemConcept { get { return mItemConcept; } set { mItemConcept = value; } }
        public double TruckLoadPricePerSellingUnit { get { return mTruckLoadPricePerSellingUnit; } set { mTruckLoadPricePerSellingUnit = value; } }
        public int RetailSellingUnitsBaseUOM { get { return mRetailSellingUnitsBaseUOM; } set { mRetailSellingUnitsBaseUOM = value; } }
        public string SAPBaseUOM { get { return mSAPBaseUOM; } set { mSAPBaseUOM = value; } }
        public string CustomerSpecific { get { return mCustomerSpecific; } set { mCustomerSpecific = value; } }
        public string Customer { get { return mCustomer; } set { mCustomer = value; } }
        public string Channel { get { return mChannel; } set { mChannel = value; } }
        public double TruckloadPriceperBaseUOM { get { return mTruckloadPriceperBaseUOM; } set { mTruckloadPriceperBaseUOM = value; } }


        public string MaterialGroup2Pricing { get { return mMaterialGroup2Pricing; } set { mMaterialGroup2Pricing = value; } }
        public string ProductHierarchyLevel1 { get { return mProductHierarchyLevel1; } set { mProductHierarchyLevel1 = value; } }
        public string ProductHierarchyLevel2 { get { return mProductHierarchyLevel2; } set { mProductHierarchyLevel2 = value; } }
        public string MaterialGroup1Brand { get { return mMaterialGroup1Brand; } set { mMaterialGroup1Brand = value; } }
        public string MaterialGroup4ProductForm { get { return mMaterialGroup4ProductForm; } set { mMaterialGroup4ProductForm = value; } }
        public string MaterialGroup5PackType { get { return mMaterialGroup5PackType; } set { mMaterialGroup5PackType = value; } }

        public string CaseType { get { return mCaseType; } set { mCaseType = value; } }
        public string UnitsInsideCarton { get { return mUnitsInsideCarton; } set { mUnitsInsideCarton = value; } }
        public double IndividualPouchWeight { get { return mIndividualPouchWeight; } set { mIndividualPouchWeight = value; } }
        public int NumberofTraysPerBaseUOM { get { return mNumberofTraysPerBaseUOM; } set { mNumberofTraysPerBaseUOM = value; } }
        public double RetailUnitWieghtOz { get { return mRetailUnitWieghtOz; } set { mRetailUnitWieghtOz = value; } }
        public double BaseUOMNetWeightLbs { get { return mBaseUOMNetWeightLbs; } set { mBaseUOMNetWeightLbs = value; } }
        public string InitialEstimatedBracketPricing { get { return mInitialEstimatedBracketPricing; } set { mInitialEstimatedBracketPricing = value; } }
        public string InitialEstimatedPricing { get { return mInitialEstimatedPricing; } set { mInitialEstimatedPricing = value; } }      

        #endregion
    }
}
