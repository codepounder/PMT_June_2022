using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    [Serializable]
    public class StageGateIPFItem
    {
        public StageGateIPFItem()
        {
        }

        #region Variables
        private int mStageGateListItemId;
        private int mCompassListItemId;
        private string mProjectNumber;
        private string mTBDIndicator;
        private string mFinishedGood;
        private string mLikeNumber;
        private string mDescription;
        private string mUCC;
        private string mUPC;
        private string mProductHierarchy1;
        private string mProductHierarchy2;
        private string mBrandMaterialGroup1;
        private string mProductMaterialGroup4;
        private string mPackTypeMaterialGroup5;
        private string mProjectStatus;
        private bool mCreateIPFBtn;
        private bool mNeedsNewBtn;
        private bool mDeleteBtn;
        private bool mGenerated;

        #endregion

        public int StageGateListItemId { get { return mStageGateListItemId; } set { mStageGateListItemId = value; } }
        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }

        #region List Properties
        public string ProjectNumber { get { return mProjectNumber; } set { mProjectNumber = value; } }
        public string TBDIndicator { get { return mTBDIndicator; } set { mTBDIndicator = value; } }
        public string FinishedGood { get { return mFinishedGood; } set { mFinishedGood = value; } }
        public string LikeNumber { get { return mLikeNumber; } set { mLikeNumber = value; } }
        public string Description { get { return mDescription; } set { mDescription = value; } }
        public string UCC { get { return mUCC; } set { mUCC = value; } }
        public string UPC { get { return mUPC; } set { mUPC = value; } }
        public string ProductHierarchy1 { get { return mProductHierarchy1; } set { mProductHierarchy1 = value; } }
        public string ProductHierarchy2 { get { return mProductHierarchy2; } set { mProductHierarchy2 = value; } }
        public string BrandMaterialGroup1 { get { return mBrandMaterialGroup1; } set { mBrandMaterialGroup1 = value; } }
        public string ProductMaterialGroup4 { get { return mProductMaterialGroup4; } set { mProductMaterialGroup4 = value; } }
        public string PackTypeMaterialGroup5 { get { return mPackTypeMaterialGroup5; } set { mPackTypeMaterialGroup5 = value; } }
        public string ProjectStatus { get { return mProjectStatus; } set { mProjectStatus = value; } }
        public bool CreateIPFBtn { get { return mCreateIPFBtn; } set { mCreateIPFBtn = value; } }
        public bool NeedsNewBtn { get { return mNeedsNewBtn; } set { mNeedsNewBtn = value; } }
        public bool DeleteBtn { get { return mDeleteBtn; } set { mDeleteBtn = value; } }
        public bool Generated { get { return mGenerated; } set { mGenerated = value; } }
        #endregion
    }
}
