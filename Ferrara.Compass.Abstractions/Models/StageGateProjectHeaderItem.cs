using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class StageGateProjectHeaderItem
    {
        #region Variables
        private string mProjectNumber;
        private string mProjectStage;
        private string mProjectType;
        private string mProjectTypeSubCategory;
        #endregion
        private string mProjectName;
        private string mPageName;
        private string mTestProject;

        #region Properties
        public string ProjectNumber { get { return mProjectNumber; } set { mProjectNumber = value; } }
        public string ProjectStage { get { return mProjectStage; } set { mProjectStage = value; } }
        public string ProjectName { get { return mProjectName; } set { mProjectName = value; } }
        public string PageName { get { return mPageName; } set { mPageName = value; } }
        public string ProjectType { get { return mProjectType; } set { mProjectType = value; } }
        public string ProjectTypeSubCategory { get { return mProjectTypeSubCategory; } set { mProjectTypeSubCategory = value; } }
        public string TestProject { get { return mTestProject; } set { mTestProject = value; } }
        #endregion
    }
}
