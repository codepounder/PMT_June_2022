using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class ProjectHeaderItem
    {
        #region Variables
        private string mNewIPF;
        private string mProjectNumber;
        private string mProjectType;
        private string mProjectTypeSubCategory;
        private string mSAPItemNumber;
        private string mSAPDescription;
        private string mCriticalInitiative;
        private string mWorkflowPhase;
        private string mTestProject;
        #endregion

        #region Properties
        public string NewIPF { get { return mNewIPF; } set { mNewIPF = value; } }
        public string ProjectNumber { get { return mProjectNumber; } set { mProjectNumber = value; } }
        public string ProjectType { get { return mProjectType; } set { mProjectType = value; } }
        public string ProjectTypeSubCategory { get { return mProjectTypeSubCategory; } set { mProjectTypeSubCategory = value; } }
        public string SAPItemNumber { get { return mSAPItemNumber; } set { mSAPItemNumber = value; } }
        public string SAPDescription { get { return mSAPDescription; } set { mSAPDescription = value; } }
        public string CriticalInitiative { get { return mCriticalInitiative; } set { mCriticalInitiative = value; } }
        public string WorkflowPhase { get { return mWorkflowPhase; } set { mWorkflowPhase = value; } }
        public string TestProject { get { return mTestProject; } set { mTestProject = value; } }
        #endregion
    }
}
