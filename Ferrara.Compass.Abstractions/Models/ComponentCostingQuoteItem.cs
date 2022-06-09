using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
   public class ComponentCostingQuoteItem
    {
        #region Variables  
        private int mId;
        private int mCompassListItemId;
        private string mLastUpdatedFormName;
        private string mProjectType;

        private string mVendorNumber;
        private string mMaterialNumber;
        private string mSKU;
        private string mPrintStyle;
        private string mStyle;

        private string mStructure;
        private string mWebWidth;

        private string mExactCutOff;
        private string mUnwind;

        private string mCoreSize;

        private string mMaxDiameter;
        private string mReceivingPlant;
        private string mQuantityQuote;
        private string mFirst90Days;
        private string mCommentForecast;
        private string mRequestedDueDate;
        private string mNumberColors;
        private string mInkCoveragePercentage;

        private string mStandardOrderingQuantity;
        private string mOrderUOM;
        private string mIncoterms;
        private string mXferOfOwnership;

        private string mPRDateCategory;
        private string mVendorMaterialNumber;
        private string mCostingCondition;
        private string mCostingUnit;
        private string mEachesPerCostingUnit;
        private string mLBPerCostingUnit;
        private string mCostingUnitPerPallet;
        private string mStandardCost;
        private double mRetailUnitWieghtOz;
        private string mFilmSubstrate;
        private DateTime mExpectedPackagingSwitchDate;
        private string mComponentType;
        private decimal mMonth1ProjectedDollar;
        private decimal mMonth2ProjectedDollar;
        private decimal mMonth3ProjectedDollar;
        private int mRetailSellingUnitsBaseUOM;
        private string mPackQty;
        private string mPackagingItemId;
        private string mCostingQuoteDate;
        private string mForecastComments;
        private string mProductHierarchyLevel1;
        private string mMaterialDescription;
        private decimal mMonth1ProjectedUnits;
        private decimal mMonth2ProjectedUnits;
        private decimal mMonth3ProjectedUnits;
        private string mBaseUOM;
        private DateTime mCompCostSubmittedDate;
        private string mPrinterSupplier;

        //New Fields
        private DateTime mValidityStartDate;
        private DateTime mValidityEndDate;
        private string mSupplierAgreementNumber;
        private string mSubcontracted;
        private string mProcurementManager;
        private string mBracketPricing;
        private string mPIRCostPerUOM;
        private string mPerUnit;
        private string mDeliveredOrOriginCost;
        private string mFreightAmount;
        private string mTransferOfOwnership;
        private string mPlannedDeliveryTime;
        private string mMinimumOrderQTY;
        private string mStandardQuantity;
        private string mTolOverDelivery;
        private string mTolUnderDelivery;
        private string mPurchasingGroup;
        private string mConversionFactors;
        private string mAnnualVolumeEA;
        private string mNinetyDayVolume;
        private string mAnnualVolumeCaseUOM;
        private string mPriceDetermination;





        #endregion

        #region Properties
        public int Id { get { return mId; } set { mId = value; } }
        public string ComponentType { get { return mComponentType; } set { mComponentType = value; } }
        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        public string LastUpdatedFormName { get { return mLastUpdatedFormName; } set { mLastUpdatedFormName = value; } }
        public string ProjectType { get { return mProjectType; } set { mProjectType = value; } }
        public string VendorNumber { get { return mVendorNumber; } set { mVendorNumber = value; } }
        public string MaterialNumber { get { return mMaterialNumber; } set { mMaterialNumber = value; } }
        public string SKU { get { return mSKU; } set { mSKU = value; } }
        public string Style { get { return mStyle; } set { mStyle = value; } }
        public string PrintStyle { get { return mPrintStyle; } set { mPrintStyle = value; } }
        public string Structure { get { return mStructure; } set { mStructure = value; } }
        public string WebWidth { get { return mWebWidth; } set { mWebWidth = value; } }
        public string ExactCutOff { get { return mExactCutOff; } set { mExactCutOff = value; } }
        public string Unwind { get { return mUnwind; } set { mUnwind = value; } }
        public string CoreSize { get { return mCoreSize; } set { mCoreSize = value; } }
        public string MaxDiameter { get { return mMaxDiameter; } set { mMaxDiameter = value; } }
        public string ReceivingPlant { get { return mReceivingPlant; } set { mReceivingPlant = value; } }
        public string QuantityQuote { get { return mQuantityQuote; } set { mQuantityQuote = value; } }
        public string First90Days { get { return mFirst90Days; } set { mFirst90Days = value; } }
        public string CommentForecast { get { return mCommentForecast; } set { mCommentForecast = value; } }
        public string RequestedDueDate { get { return mRequestedDueDate; } set { mRequestedDueDate = value; } }
        public string NumberColors { get { return mNumberColors; } set { mNumberColors = value; } }
        public string InkCoveragePercentage { get { return mInkCoveragePercentage; } set { mInkCoveragePercentage = value; } }
        public string StandardOrderingQuantity { get { return mStandardOrderingQuantity; } set { mStandardOrderingQuantity = value; } }
        public string OrderUOM { get { return mOrderUOM; } set { mOrderUOM = value; } }
        public string Incoterms { get { return mIncoterms; } set { mIncoterms = value; } }
        public string XferOfOwnership { get { return mXferOfOwnership; } set { mXferOfOwnership = value; } }
        public string PRDateCategory { get { return mPRDateCategory; } set { mPRDateCategory = value; } }
        public string VendorMaterialNumber { get { return mVendorMaterialNumber; } set { mVendorMaterialNumber = value; } }
        public string CostingCondition { get { return mCostingCondition; } set { mCostingCondition = value; } }
        public string CostingUnit { get { return mCostingUnit; } set { mCostingUnit = value; } }
        public string EachesPerCostingUnit { get { return mEachesPerCostingUnit; } set { mEachesPerCostingUnit = value; } }
        public string LBPerCostingUnit { get { return mLBPerCostingUnit; } set { mLBPerCostingUnit = value; } }
        public string CostingUnitPerPallet { get { return mCostingUnitPerPallet; } set { mCostingUnitPerPallet = value; } }
        public string StandardCost { get { return mStandardCost; } set { mStandardCost = value; } }
        public double RetailUnitWieghtOz { get { return mRetailUnitWieghtOz; } set { mRetailUnitWieghtOz = value; } }
        public string FilmSubstrate { get { return mFilmSubstrate; } set { mFilmSubstrate = value; } }
        public DateTime ExpectedPackagingSwitchDate { get { return mExpectedPackagingSwitchDate; } set { mExpectedPackagingSwitchDate = value; } }
        public decimal Month1ProjectedDollar { get { return mMonth1ProjectedDollar; } set { mMonth1ProjectedDollar = value; } }
        public decimal Month2ProjectedDollar { get { return mMonth2ProjectedDollar; } set { mMonth2ProjectedDollar = value; } }
        public decimal Month3ProjectedDollar { get { return mMonth3ProjectedDollar; } set { mMonth3ProjectedDollar = value; } }
        public int RetailSellingUnitsBaseUOM { get { return mRetailSellingUnitsBaseUOM; } set { mRetailSellingUnitsBaseUOM = value; } }
        public string PackQty { get { return mPackQty; } set { mPackQty = value; } }
        public string PackagingItemId { get { return mPackagingItemId; } set { mPackagingItemId = value; } }
        public string CostingQuoteDate { get { return mCostingQuoteDate; } set { mCostingQuoteDate = value; } }
        public string ForecastComments { get { return mForecastComments; } set { mForecastComments = value; } }
        public string ProductHierarchyLevel1 { get { return mProductHierarchyLevel1; } set { mProductHierarchyLevel1 = value; } }
        public string MaterialDescription { get { return mMaterialDescription; } set { mMaterialDescription = value; } }
        public decimal Month1ProjectedUnits { get { return mMonth1ProjectedUnits; } set { mMonth1ProjectedUnits = value; } }
        public decimal Month2ProjectedUnits { get { return mMonth2ProjectedUnits; } set { mMonth2ProjectedUnits = value; } }
        public decimal Month3ProjectedUnits { get { return mMonth3ProjectedUnits; } set { mMonth3ProjectedUnits = value; } }
        public DateTime CompCostSubmittedDate { get { return mCompCostSubmittedDate; } set { mCompCostSubmittedDate = value; } }
        public string PrinterSupplier { get { return mPrinterSupplier; } set { mPrinterSupplier = value; } }

        public DateTime ValidityStartDate { get { return mValidityStartDate; } set { mValidityStartDate = value; } }
        public DateTime ValidityEndDate { get { return mValidityEndDate; } set { mValidityEndDate = value; } }
        public string SupplierAgreementNumber { get { return mSupplierAgreementNumber; } set { mSupplierAgreementNumber = value; } }
        public string Subcontracted { get { return mSubcontracted; } set { mSubcontracted = value; } }
        public string ProcurementManager { get { return mProcurementManager; } set { mProcurementManager = value; } }
        public string BracketPricing { get { return mBracketPricing; } set { mBracketPricing = value; } }
        public string PIRCostPerUOM { get { return mPIRCostPerUOM; } set { mPIRCostPerUOM = value; } }
        public string PerUnit { get { return mPerUnit; } set { mPerUnit = value; } }
        public string DeliveredOrOriginCost { get { return mDeliveredOrOriginCost; } set { mDeliveredOrOriginCost = value; } }
        public string FreightAmount { get { return mFreightAmount; } set { mFreightAmount = value; } }
        public string TransferOfOwnership { get { return mTransferOfOwnership; } set { mTransferOfOwnership = value; } }
        public string PlannedDeliveryTime { get { return mPlannedDeliveryTime; } set { mPlannedDeliveryTime = value; } }
        public string MinimumOrderQTY { get { return mMinimumOrderQTY; } set { mMinimumOrderQTY = value; } }
        public string StandardQuantity { get { return mStandardQuantity; } set { mStandardQuantity = value; } }
        public string TolOverDelivery { get { return mTolOverDelivery; } set { mTolOverDelivery = value; } }
        public string TolUnderDelivery { get { return mTolUnderDelivery; } set { mTolUnderDelivery = value; } }
        public string PurchasingGroup { get { return mPurchasingGroup; } set { mPurchasingGroup = value; } }
        public string ConversionFactors { get { return mConversionFactors; } set { mConversionFactors = value; } }
        public string AnnualVolumeEA { get { return mAnnualVolumeEA; } set { mAnnualVolumeEA = value; } }
        public string NinetyDayVolume { get { return mNinetyDayVolume; } set { mNinetyDayVolume = value; } }
        public string AnnualVolumeCaseUOM { get { return mAnnualVolumeCaseUOM; } set { mAnnualVolumeCaseUOM = value; } }
        public string PriceDetermination { get { return mPriceDetermination; } set { mPriceDetermination = value; } }
        public string BaseUOM { get { return mBaseUOM; } set { mBaseUOM = value; } }

        #endregion

    }
}

