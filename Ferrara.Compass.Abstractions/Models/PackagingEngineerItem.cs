using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferrara.Compass.Abstractions.Models
{
    public class PackagingEngineerItem
    {
        #region Member Variables
        private int mVersion;
        private string mPreCostComments;

        private DateTime mCreatedDate;
        private string mCreatedBy;
        private DateTime mModifiedDate;
        private string mModifiedBy;
        #endregion

        #region Properties
        public int Version { get { return mVersion; } set { mVersion = value; } }
        public string PreCostComments { get { return mPreCostComments; } set { mPreCostComments = value; } }
        public DateTime CreatedDate { get { return mCreatedDate; } set { mCreatedDate = value; } }
        public string CreatedBy { get { return mCreatedBy; } set { mCreatedBy = value; } }
        public DateTime ModifiedDate { get { return mModifiedDate; } set { mModifiedDate = value; } }
        public string ModifiedBy { get { return mModifiedBy; } set { mModifiedBy = value; } }
        #endregion
    }
}
