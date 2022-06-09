using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class OBMFirstReviewItem
    {
        #region Variables
        private int mCompassListItemId;
        private string mLastUpdatedFormName;

        // OBM First Review fields
        private string mOBMFirstReviewCheck;
        private string mSectionsOfConcern;
        private string mOBMFirstReviewComments;
        private string mProjectStatus;
        private string mDoesFirstShipNeedRevision;
        private DateTime mRevisedFirstShipDate;
        private string mRevisedFirstShipDateComments;
        private DateTime mFirstProductionDate;

        // Read-only fields
        private string mProjectType;
        private string mProductHierarchyLevel1;
        private string mProductHierarchyLevel2;
        private string mMaterialGroup1Brand;
        private DateTime mFirstShipDate;
        private double mAnnualProjectedDollars;
        private int mAnnualProjectedUnits;
        private string mCustomer;
        private double mExpectedGrossMarginPercent;
        private double mRevisedGrossMarginPercent;
        private string mItemConcept;

        private string mMakeLocation;
        private string mPackingLocation;
        private string mDesignateHUBDC;
        private string mInternalTransferSemiNeeded;

        private string mCoManufacturingClassification;
        private string mExternalManufacturer;
        private string mExternalPacker;

        private string mUnitUPC;
        private string mDisplayBoxUPC;
        private string mCaseUCC;
        private string mPalletUCC;
        private string mChannel;
        private string mInitialCosting_GrossMarginAccurate;

        private string mMaterialNumber;
        private string mMaterialDescription;
        #endregion

        #region Properties
        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        public string LastUpdatedFormName { get { return mLastUpdatedFormName; } set { mLastUpdatedFormName = value; } }


        public string OBMFirstReviewCheck { get { return mOBMFirstReviewCheck; } set { mOBMFirstReviewCheck = value; } }
        public string SectionsOfConcern { get { return mSectionsOfConcern; } set { mSectionsOfConcern = value; } }
        public string OBMFirstReviewComments { get { return mOBMFirstReviewComments; } set { mOBMFirstReviewComments = value; } }
        public string ProjectStatus { get { return mProjectStatus; } set { mProjectStatus = value; } }
        public string DoesFirstShipNeedRevision { get { return mDoesFirstShipNeedRevision; } set { mDoesFirstShipNeedRevision = value; } }
        public DateTime RevisedFirstShipDate { get { return mRevisedFirstShipDate; } set { mRevisedFirstShipDate = value; } }
        public string RevisedFirstShipDateComments { get { return mRevisedFirstShipDateComments; } set { mRevisedFirstShipDateComments = value; } }
        public DateTime FirstProductionDate { get { return mFirstProductionDate; } set { mFirstProductionDate = value; } }

        public string ProjectType { get { return mProjectType; } set { mProjectType = value; } }
        public string ProductHierarchyLevel1 { get { return mProductHierarchyLevel1; } set { mProductHierarchyLevel1 = value; } }
        public string ProductHierarchyLevel2 { get { return mProductHierarchyLevel2; } set { mProductHierarchyLevel2 = value; } }
        public string MaterialGroup1Brand { get { return mMaterialGroup1Brand; } set { mMaterialGroup1Brand = value; } }
        public DateTime FirstShipDate { get { return mFirstShipDate; } set { mFirstShipDate = value; } }
        public double AnnualProjectedDollars { get { return mAnnualProjectedDollars; } set { mAnnualProjectedDollars = value; } }
        public int AnnualProjectedUnits { get { return mAnnualProjectedUnits; } set { mAnnualProjectedUnits = value; } }
        public string Customer { get { return mCustomer; } set { mCustomer = value; } }
        public double ExpectedGrossMarginPercent { get { return mExpectedGrossMarginPercent; } set { mExpectedGrossMarginPercent = value; } }
        public double RevisedGrossMarginPercent { get { return mRevisedGrossMarginPercent; } set { mRevisedGrossMarginPercent = value; } }
        public string ItemConcept { get { return mItemConcept; } set { mItemConcept = value; } }

        public string MakeLocation { get { return mMakeLocation; } set { mMakeLocation = value; } }
        public string PackingLocation { get { return mPackingLocation; } set { mPackingLocation = value; } }
        public string DesignateHUBDC { get { return mDesignateHUBDC; } set { mDesignateHUBDC = value; } }
        public string InternalTransferSemiNeeded { get { return mInternalTransferSemiNeeded; } set { mInternalTransferSemiNeeded = value; } }

        public string CoManufacturingClassification { get { return mCoManufacturingClassification; } set { mCoManufacturingClassification = value; } }
        public string ExternalManufacturer { get { return mExternalManufacturer; } set { mExternalManufacturer = value; } }
        public string ExternalPacker { get { return mExternalPacker; } set { mExternalPacker = value; } }

        public string UnitUPC { get { return mUnitUPC; } set { mUnitUPC = value; } }
        public string DisplayBoxUPC { get { return mDisplayBoxUPC; } set { mDisplayBoxUPC = value; } }
        public string CaseUCC { get { return mCaseUCC; } set { mCaseUCC = value; } }
        public string PalletUCC { get { return mPalletUCC; } set { mPalletUCC = value; } }
        public string Channel { get { return mChannel; } set { mChannel = value; } }
        public string InitialCosting_GrossMarginAccurate { get { return mInitialCosting_GrossMarginAccurate; } set { mInitialCosting_GrossMarginAccurate = value; } }
        public string MaterialNumber { get { return mMaterialNumber; } set { mMaterialNumber = value; } }
        public string MaterialDescriptiom { get { return mMaterialDescription; } set { mMaterialDescription = value; } }


        #endregion

    }
}
