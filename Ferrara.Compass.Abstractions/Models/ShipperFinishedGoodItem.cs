using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    [Serializable]
    public class ShipperFinishedGoodItem
    {
        #region Variables
        #region General Variables
        private int itemId;
        private int compassListItemId;
        #endregion

        private string mFGItemNumber;
        private string mFGItemDescription;
        private int mFGItemNumberUnits;
        private double mFGItemOuncesPerUnit;
        private string mFGPackUnit;
        private string mFGShelfLife;
        private string mIngredientsNeedToClaimBioEng;
        private bool mFGDeleted;
        #endregion

        #region Properties
        #region General Properties
        public int ItemId { get { return itemId; } set { itemId = value; } }
        public int CompassListItemId { get { return compassListItemId; } set { compassListItemId = value; } }
        #endregion

        public string FGItemNumber { get { return mFGItemNumber; } set { mFGItemNumber = value; } }
        public string FGItemDescription { get { return mFGItemDescription; } set { mFGItemDescription = value; } }
        public int FGItemNumberUnits { get { return mFGItemNumberUnits; } set { mFGItemNumberUnits = value; } }
        public double FGItemOuncesPerUnit { get { return mFGItemOuncesPerUnit; } set { mFGItemOuncesPerUnit = value; } }
        public string FGPackUnit { get { return mFGPackUnit; } set { mFGPackUnit = value; } }
        public string FGShelfLife { get { return mFGShelfLife; } set { mFGShelfLife = value; } }
        public string IngredientsNeedToClaimBioEng { get { return mIngredientsNeedToClaimBioEng; } set { mIngredientsNeedToClaimBioEng = value; } }        
        public bool FGDeleted { get { return mFGDeleted; } set { mFGDeleted = value; } }

        #endregion

    }
}
