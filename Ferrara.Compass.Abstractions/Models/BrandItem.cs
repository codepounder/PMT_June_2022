using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class BrandItem
    {
        private string mProductHierarchyLevel1;
        private string mMaterialGroup1Brand;
        private string mPM;
        private string mBrandManager;

        public string ProductHierarchyLevel1 { get { return mProductHierarchyLevel1; } set { mProductHierarchyLevel1 = value; } }
        public string MaterialGroup1Brand { get { return mMaterialGroup1Brand; } set { mMaterialGroup1Brand = value; } }
        public string PM { get { return mPM; } set { mPM = value; } }
        public string BrandManager { get { return mBrandManager; } set { mBrandManager = value; } }
    }
}
