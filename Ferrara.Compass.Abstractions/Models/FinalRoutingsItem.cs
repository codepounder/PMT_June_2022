using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Constants;

namespace Ferrara.Compass.Abstractions.Models
{
    [Serializable]
    public class FinalRoutingsItem
    {

        #region Member Variables
        private int miItemId;
        private string mTitle; // Contains the ID before being reset by a Copy or Change Request
        private string mMaterial;
        private string mPlant;
        private string mBBlockOnItem;
        private string mMRPType;
        private string mPOExists;
        private string mSourceListComplete;
        private string mStandardCostSet;
        private string mZBlocksComplete;
        private string mSAPRoutings;
        private string mCurrentAvailableQuantity;
        private string mDateofFirstProduction;
        private string mQuantityofFirstProduction;
        private string mDateofOrder;
        private string mQuantityofOrder;
        private string mHANAKey;
        private string mNewExisting;
        private string mCreated;
        #endregion

        #region Properties
        public int iItemId { get { return miItemId; } set { miItemId = value; } }
        public string HANAKey { get { return mHANAKey; } set { mHANAKey = value; } }
        public string QuantityofOrder { get { return mQuantityofOrder; } set { mQuantityofOrder = value; } }
        public string DateofOrder { get { return mDateofOrder; } set { mDateofOrder = value; } }
        public string QuantityofFirstProduction { get { return mQuantityofFirstProduction; } set { mQuantityofFirstProduction = value; } }
        public string DateofFirstProduction { get { return mDateofFirstProduction; } set { mDateofFirstProduction = value; } }
        public string CurrentAvailableQuantity { get { return mCurrentAvailableQuantity; } set { mCurrentAvailableQuantity = value; } }
        public string NewExisting { get { return mNewExisting; } set { mNewExisting = value; } }
        public string SAPRoutings { get { return mSAPRoutings; } set { mSAPRoutings = value; } }
        public string ZBlocksComplete { get { return mZBlocksComplete; } set { mZBlocksComplete = value; } }
        public string StandardCostSet { get { return mStandardCostSet; } set { mStandardCostSet = value; } }
        public string SourceListComplete { get { return mSourceListComplete; } set { mSourceListComplete = value; } }
        public string POExists { get { return mPOExists; } set { mPOExists = value; } }
        public string MRPType { get { return mMRPType; } set { mMRPType = value; } }
        public string BBlockOnItem { get { return mBBlockOnItem; } set { mBBlockOnItem = value; } }
        public string Plant { get { return mPlant; } set { mPlant = value; } }
        public string Material { get { return mMaterial; } set { mMaterial = value; } }
        public string Title { get { return mTitle; } set { mTitle = value; } }
        public string Created { get { return mCreated; } set { mCreated = value; } }

        #endregion
    }
}
