using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Constants;

namespace Ferrara.Compass.Abstractions.Models
{
    [Serializable]
    public class MaterialsReceivedItem
    {

        #region Member Variables
        private string mMaterialNumber;
        private string mMaterialDescription;
        private string mNewExisting;
        private string mPlant;
        private string mCurrentAvailQuantity;
        private string mDateOfOrder;
        private string mQuantityOfOrder;
        
        #endregion

        #region Properties
        public string MaterialNumber { get { return mMaterialNumber; } set { mMaterialNumber = value; } }
        public string MaterialDescription { get { return mMaterialDescription; } set { mMaterialDescription = value; } }
        public string NewExisting { get { return mNewExisting; } set { mNewExisting = value; } }
        public string Plant { get { return mPlant; } set { mPlant = value; } }
        public string CurrentAvailQuantity { get { return mCurrentAvailQuantity; } set { mCurrentAvailQuantity = value; } }
        public string DateOfOrder { get { return mDateOfOrder; } set { mDateOfOrder = value; } }
        public string QuantityOfOrder { get { return mQuantityOfOrder; } set { mQuantityOfOrder = value; } }       
        #endregion
    }
}
