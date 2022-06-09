using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class SAPBOMListItem
    {
        #region Variables
        private string mSAPItemNumber;
        private string mMaterialNumber;
        private string mMaterialType;
        private string mMaterialDescription;
        private string mPackQuantity;
        private string mPackUnit;
        #endregion

        #region Properties
        public string SAPItemNumber { get { return mSAPItemNumber; } set { mSAPItemNumber = value; } }
        public string MaterialNumber { get { return mMaterialNumber; } set { mMaterialNumber = value; } }
        public string MaterialType { get { return mMaterialType; } set { mMaterialType = value; } }
        public string MaterialDescription { get { return mMaterialDescription; } set { mMaterialDescription = value; } }
        public string PackQuantity { get { return mPackQuantity; } set { mPackQuantity = value; } }
        public string PackUnit { get { return mPackUnit; } set { mPackUnit = value; } }

        #endregion
    }
}
