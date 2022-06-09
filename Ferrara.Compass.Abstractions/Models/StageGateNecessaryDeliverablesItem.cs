using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class StageGateNecessaryDeliverablesItem
    {
        #region Variables
        private int mDelivId;
        private int mProjectId;
        private string mProjectNumber;
        private string mStage;
        private string mNecessaryDeliverables;
        private string mOwner;
        private string mApplicable;
        private string mStatus;
        private string mSubtask;
        private string mComments;
        private string mModifiedBy;
        private DateTime mModifiedDate;
        private bool mAdded;
        #endregion

        #region Properties
        public int DelivId { get { return mDelivId; } set { mDelivId = value; } }
        public int ProjectId { get { return mProjectId; } set { mProjectId = value; } }
        public string ProjectNumber { get { return mProjectNumber; } set { mProjectNumber = value; } }
        public string Stage { get { return mStage; } set { mStage = value; } }
        public string NecessaryDeliverables { get { return mNecessaryDeliverables; } set { mNecessaryDeliverables = value; } }
        public string Owner { get { return mOwner; } set { mOwner = value; } }
        public string Applicable { get { return mApplicable; } set { mApplicable = value; } }
        public string Status { get { return mStatus; } set { mStatus = value; } }
        public string Subtask { get { return mSubtask; } set { mSubtask = value; } }
        public string Comments { get { return mComments; } set { mComments = value; } }
        public string ModifiedBy { get { return mModifiedBy; } set { mModifiedBy = value; } }
        public DateTime ModifiedDate { get { return mModifiedDate; } set { mModifiedDate = value; } }
        public bool Added { get { return mAdded; } set { mAdded = value; } }
        #endregion
    }
}
