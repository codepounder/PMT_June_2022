using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Constants;

namespace Ferrara.Compass.Abstractions.Models
{
    public class CompassPackMeasurementsItem
    {

        public CompassPackMeasurementsItem()
        {
            this.mDeleted = "No";

            mNetUnitWeight = -9999;
            mUnitDimensionLength = -9999;
            mUnitDimensionWidth = -9999;
            mUnitDimensionHeight = -9999;

            mCasePack = -9999;
            mCaseDimensionLength = -9999;
            mCaseDimensionWidth = -9999;
            mCaseDimensionHeight = -9999;
            mCaseCube = -9999;
            mCaseNetWeight = -9999;
            mCaseGrossWeight = -9999;

            mCasesPerPallet = -9999;
            mPalletDimensionsLength = -9999;
            mPalletDimensionsWidth = -9999;
            mPalletDimensionsHeight = -9999;
            mPalletCube = -9999;
            mPalletWeight = -9999;
            mPalletGrossWeight = -9999;
            mCasesPerLayer = -9999;
            mLayersPerPallet = -9999;

            mSalesUnitDimensionsLength = -9999;
            mSalesUnitDimensionsWidth = -9999;
            mSalesUnitDimensionsHeight = -9999;
            mSalesCaseDimensionsLength = -9999;
            mSalesCaseDimensionsWidth = -9999;
            mSalesCaseDimensionsHeight = -9999;

            mDisplayDimensionsLength = -9999;
            mDisplayDimensionsWidth = -9999;
            mDisplayDimensionsHeight = -9999;
            mSetUpDimensionsLength = -9999;
            mSetUpDimensionsWidth = -9999;
            mSetUpDimensionsHeight = -9999;
            
        }

        #region Variables
        #region General Variables
        private int mItemId;
        private int mCompassListItemId;
        private int mParentComponentId;
        private string mBOMType;
        private string mDeleted;
        #endregion

        #region Pack Measurement Variables
        private string mPackTrialNeeded;
        private string mPackTrialResult;
        private string mPackTrialComments;

        private double mNetUnitWeight;
        private double mUnitDimensionLength;
        private double mUnitDimensionWidth;
        private double mUnitDimensionHeight;

        private double mCasePack;
        private double mCaseDimensionLength;
        private double mCaseDimensionWidth;
        private double mCaseDimensionHeight;
        private double mCaseCube;
        private double mCaseNetWeight;
        private double mCaseGrossWeight;

        private double mCasesPerPallet;
        private string mDoubleStackable;
        private double mPalletDimensionsLength;
        private double mPalletDimensionsWidth;
        private double mPalletDimensionsHeight;
        private double mPalletCube;
        private double mPalletWeight;
        private double mPalletGrossWeight;
        private double mCasesPerLayer;
        private double mLayersPerPallet;

        private double mSalesUnitDimensionsLength;
        private double mSalesUnitDimensionsWidth;
        private double mSalesUnitDimensionsHeight;
        private double mSalesCaseDimensionsLength;
        private double mSalesCaseDimensionsWidth;
        private double mSalesCaseDimensionsHeight;

        private double mDisplayDimensionsLength;
        private double mDisplayDimensionsWidth;
        private double mDisplayDimensionsHeight;
        private double mSetUpDimensionsLength;
        private double mSetUpDimensionsWidth;
        private double mSetUpDimensionsHeight;
        private string mPalletPatternChange;

        private string mSAPSpecsChange;
        #endregion
        #endregion

        #region Properties
        #region General Properties
        public int ItemId { get { return mItemId; } set { mItemId = value; } }
        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        public int ParentComponentId { get { return mParentComponentId; } set { mParentComponentId = value; } }
        public string BOMType { get { return mBOMType; } set { mBOMType = value; } }
        public string Deleted { get { return mDeleted; } set { mDeleted = value; } }

        #endregion

        #region Pack Measurement Properties
        public string PackTrialNeeded { get { return mPackTrialNeeded; } set { mPackTrialNeeded = value; } }
        public string PackTrialResult { get { return mPackTrialResult; } set { mPackTrialResult = value; } }
        public string PackTrialComments { get { return mPackTrialComments; } set { mPackTrialComments = value; } }
        public double NetUnitWeight { get { return mNetUnitWeight; } set { mNetUnitWeight = value; } }
        public double UnitDimensionLength { get { return mUnitDimensionLength; } set { mUnitDimensionLength = value; } }
        public double UnitDimensionWidth { get { return mUnitDimensionWidth; } set { mUnitDimensionWidth = value; } }
        public double UnitDimensionHeight { get { return mUnitDimensionHeight; } set { mUnitDimensionHeight = value; } }
        public double CasePack { get { return mCasePack; } set { mCasePack = value; } }
        public double CaseDimensionLength { get { return mCaseDimensionLength; } set { mCaseDimensionLength = value; } }
        public double CaseDimensionWidth { get { return mCaseDimensionWidth; } set { mCaseDimensionWidth = value; } }
        public double CaseDimensionHeight { get { return mCaseDimensionHeight; } set { mCaseDimensionHeight = value; } }
        public double CaseCube { get { return mCaseCube; } set { mCaseCube = value; } }
        public double CaseNetWeight { get { return mCaseNetWeight; } set { mCaseNetWeight = value; } }
        public double CaseGrossWeight { get { return mCaseGrossWeight; } set { mCaseGrossWeight = value; } }
        public double CasesPerPallet { get { return mCasesPerPallet; } set { mCasesPerPallet = value; } }
        public string DoubleStackable { get { return mDoubleStackable; } set { mDoubleStackable = value; } }
        public double PalletDimensionsLength { get { return mPalletDimensionsLength; } set { mPalletDimensionsLength = value; } }
        public double PalletDimensionsWidth { get { return mPalletDimensionsWidth; } set { mPalletDimensionsWidth = value; } }
        public double PalletDimensionsHeight { get { return mPalletDimensionsHeight; } set { mPalletDimensionsHeight = value; } }
        public double PalletCube { get { return mPalletCube; } set { mPalletCube = value; } }
        public double PalletWeight { get { return mPalletWeight; } set { mPalletWeight = value; } }
        public double PalletGrossWeight { get { return mPalletGrossWeight; } set { mPalletGrossWeight = value; } }
        public double CasesPerLayer { get { return mCasesPerLayer; } set { mCasesPerLayer = value; } }
        public double LayersPerPallet { get { return mLayersPerPallet; } set { mLayersPerPallet = value; } }
        public double SalesUnitDimensionsLength { get { return mSalesUnitDimensionsLength; } set { mSalesUnitDimensionsLength = value; } }
        public double SalesUnitDimensionsWidth { get { return mSalesUnitDimensionsWidth; } set { mSalesUnitDimensionsWidth = value; } }
        public double SalesUnitDimensionsHeight { get { return mSalesUnitDimensionsHeight; } set { mSalesUnitDimensionsHeight = value; } }
        public double SalesCaseDimensionsLength { get { return mSalesCaseDimensionsLength; } set { mSalesCaseDimensionsLength = value; } }
        public double SalesCaseDimensionsWidth { get { return mSalesCaseDimensionsWidth; } set { mSalesCaseDimensionsWidth = value; } }
        public double SalesCaseDimensionsHeight { get { return mSalesCaseDimensionsHeight; } set { mSalesCaseDimensionsHeight = value; } }
        public double DisplayDimensionsLength { get { return mDisplayDimensionsLength; } set { mDisplayDimensionsLength = value; } }
        public double DisplayDimensionsWidth { get { return mDisplayDimensionsWidth; } set { mDisplayDimensionsWidth = value; } }
        public double DisplayDimensionsHeight { get { return mDisplayDimensionsHeight; } set { mDisplayDimensionsHeight = value; } }
        public double SetUpDimensionsLength { get { return mSetUpDimensionsLength; } set { mSetUpDimensionsLength = value; } }
        public double SetUpDimensionsWidth { get { return mSetUpDimensionsWidth; } set { mSetUpDimensionsWidth = value; } }
        public double SetUpDimensionsHeight { get { return mSetUpDimensionsHeight; } set { mSetUpDimensionsHeight = value; } }
        public string PalletPatternChange { get { return mPalletPatternChange; } set { mPalletPatternChange = value; } }
        public string SAPSpecsChange { get { return mSAPSpecsChange; } set { mSAPSpecsChange = value; } }
        
        #endregion
        #endregion
    }
}
