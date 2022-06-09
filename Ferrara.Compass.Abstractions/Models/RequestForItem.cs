using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferrara.Compass.Abstractions.Models
{
    public class RequestForItem
    {
        #region Member Variables
        private int mVersion;
        private string mSAPItemNumber;
        private string mUnitUPC;
        private string mJarDisplayBoxUPC;
        private string mCaseUCC;
        private string mPalletUCC;
        private string mSAPDescription;

        private DateTime mCreatedDate;
        private string mCreatedBy;
        private DateTime mModifiedDate;
        private string mModifiedBy;
        #endregion

        #region Properties
        public int Version { get { return mVersion; } set { mVersion = value; } }
        public string SAPItemNumber { get { return mSAPItemNumber; } set { mSAPItemNumber = value; } }
        public string UnitUPC { get { return mUnitUPC; } set { mUnitUPC = value; } }
        public string JarDisplayBoxUPC { get { return mJarDisplayBoxUPC; } set { mJarDisplayBoxUPC = value; } }
        public string CaseUCC { get { return mCaseUCC; } set { mCaseUCC = value; } }
        public string PalletUCC { get { return mPalletUCC; } set { mPalletUCC = value; } }
        public string SAPDescription { get { return mSAPDescription; } set { mSAPDescription = value; } }
        public DateTime CreatedDate { get { return mCreatedDate; } set { mCreatedDate = value; } }
        public string CreatedBy { get { return mCreatedBy; } set { mCreatedBy = value; } }
        public DateTime ModifiedDate { get { return mModifiedDate; } set { mModifiedDate= value; } }
        public string ModifiedBy { get { return mModifiedBy; } set { mModifiedBy = value; } }
        #endregion
    }
}
