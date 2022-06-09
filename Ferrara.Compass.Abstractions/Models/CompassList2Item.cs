using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Constants;

namespace Ferrara.Compass.Abstractions.Models
{
    public class CompassList2Item
    {

        private int mCompassListItemId;

        private string mDesignateHUBDC;
        private string mDeploymentModeofItem;       
        private string mModifiedBy;
        private string mModifiedDate;
        //Graphics Fields
        private string mAllAspectsApprovedFromPEPersp;
        private string mWhatIsIncorrectPE;
        private string mIsAllProcInfoCorrect;
        private string mWhatProcInfoHasChanged;

        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        public string DesignateHUBDC { get { return mDesignateHUBDC; } set { mDesignateHUBDC = value; } }
        public string DeploymentModeofItem { get { return mDeploymentModeofItem; } set { mDeploymentModeofItem = value; } }    
        public string ModifiedBy { get { return mModifiedBy; } set { mModifiedBy = value; } }
        public string ModifiedDate { get { return mModifiedDate; } set { mModifiedDate = value; } }
        //Graphics Fields
        public string IsAllProcInfoCorrect { get { return mIsAllProcInfoCorrect; } set { mIsAllProcInfoCorrect = value; } }
        public string WhatProcInfoHasChanged { get { return mWhatProcInfoHasChanged; } set { mWhatProcInfoHasChanged = value; } }
        public string AllAspectsApprovedFromPEPersp { get { return mAllAspectsApprovedFromPEPersp; } set { mAllAspectsApprovedFromPEPersp = value; } }
        public string WhatIsIncorrectPE { get { return mWhatIsIncorrectPE; } set { mWhatIsIncorrectPE = value; } }
    }
}
