using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class StageGateProjectSummaryItem
    {
        #region Variables
        private string mProjectNumber;
        private int mStageGateProjectId;
        private int mGate;
        private string mStage;
        private string mURL;
        private string mURLText;
        private DateTime mStartDate;
        private List<KeyValuePair<DateTime, string>> mEndDateList;
        private string mEndDate;
        private string mPhaseLabel;
        private string mSubmitter;
        #endregion

        #region Properties
        public int StageGateProjectId { get { return mStageGateProjectId; } set { mStageGateProjectId = value; } }
        public string ProjectNumber { get { return mProjectNumber; } set { mProjectNumber = value; } }
        public int Gate { get { return mGate; } set { mGate = value; } }
        public string Stage { get { return mStage; } set { mStage = value; } }
        public string URL { get { return mURL; } set { mURL = value; } }
        public string URLText { get { return mURLText; } set { mURLText = value; } }
        public string EndDate { get { return mEndDate; } set { mEndDate = value; } }
        public List<KeyValuePair<DateTime, string>> EndDateList { get { return mEndDateList; } set { mEndDateList = value; } }
        public DateTime StartDate { get { return mStartDate; } set { mStartDate = value; } }  
        public string PhaseLabel { get { return mPhaseLabel; } set { mPhaseLabel = value; } }
        public string Submitter { get { return mSubmitter; } set { mSubmitter = value; } }
        #endregion
    }
}
