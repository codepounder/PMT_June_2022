using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    [Serializable]
    public class MixesItem
    {
        #region Variables
        #region General Variables
        private int itemId;
        private int compassListItemId;
        #endregion

        private string mItemNumber;
        private string mItemDescription;
        private double mNumberOfPieces;
        private double mOuncesPerPiece;
        private bool mMixDeleted;
        #endregion

        #region Properties
        #region General Properties
        public int ItemId { get { return itemId; } set { itemId = value; } }
        public int CompassListItemId { get { return compassListItemId; } set { compassListItemId = value; } }
        #endregion

        public string ItemNumber { get { return mItemNumber; } set { mItemNumber = value; } }
        public string ItemDescription { get { return mItemDescription; } set { mItemDescription = value; } }
        public double NumberOfPieces { get { return mNumberOfPieces; } set { mNumberOfPieces = value; } }
        public double OuncesPerPiece { get { return mOuncesPerPiece; } set { mOuncesPerPiece = value; } }
        public bool MixDeleted { get { return mMixDeleted; } set { mMixDeleted = value; } }

        #endregion
    }
}
