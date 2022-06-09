using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    [Serializable]
    public class TimelineTypeItem
    {
        #region General Variables

        private string mWorkflowStep;
        private int mPhaseNumber;
        private int mWorkflowOrder;
        private double mStandard;
        private double mExpedited;
        private double mLudicrous;
        private double mHolder;
        private string mWorkflowQuickStep;
        private string mWorkflowExceptions;
        private int mWorkflowStacks;
        private string mWorkflowMisc;
        #endregion

        #region General Properties

        public string WorkflowStep { get { return mWorkflowStep; } set { mWorkflowStep = value; } }
        public int PhaseNumber { get { return mPhaseNumber; } set { mPhaseNumber = value; } }
        public int WorkflowOrder { get { return mWorkflowOrder; } set { mWorkflowOrder = value; } }
        public double Standard { get { return mStandard; } set { mStandard = value; } }
        public double Expedited { get { return mExpedited; } set { mExpedited = value; } }
        public double Ludicrous { get { return mLudicrous; } set { mLudicrous = value; } }
        public double Holder { get { return mHolder; } set { mHolder = value; } }
        public string WorkflowQuickStep { get { return mWorkflowQuickStep; } set { mWorkflowQuickStep = value; } }
        public string WorkflowExceptions { get { return mWorkflowExceptions; } set { mWorkflowExceptions = value; } }
        public int WorkflowStacks { get { return mWorkflowStacks; } set { mWorkflowStacks = value; } }
        public string WorkflowMisc { get { return mWorkflowMisc; } set { mWorkflowMisc = value; } }


        #endregion
    }
}
