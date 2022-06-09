using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class AlternateUOMItem
    {
        #region Variables
        private int mId;
        private int mPackagingItemId;
        private string mAlternateUOM;
        private string mXValue;
        private string mYValue;
        #endregion

        #region Properties
        public int Id { get { return mId; } set { mId = value; } }
        public int PackagingItemId { get { return mPackagingItemId; } set { mPackagingItemId = value; } }
        public string AlternateUOM { get { return mAlternateUOM; } set { mAlternateUOM = value; } }
        public string XValue { get { return mXValue; } set { mXValue = value; } }
        public string YValue { get { return mYValue; } set { mYValue = value; } }  
        #endregion
    }
}
