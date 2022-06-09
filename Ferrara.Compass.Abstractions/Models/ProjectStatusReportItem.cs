using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferrara.Compass.Abstractions.Models
{
    public class ProjectStatusReportItem
    {
        #region Member Variables
        private string mProcess;
        private string mStatus;
        private string mSubmittedBy;
        private string mEmail;
        private DateTime mOGStartDay;
        private DateTime mOGEndDay;
        private double mOGDuration;
        private DateTime mActualStartDay;
        private DateTime mActualEndDay;
        private double mActualDuration;
        private double mPixelsFromLeft;
        private double mWidth;
        private string mChecks;
        private string mColor;
        private string mWorflowQuickStep;
        private string mReadOnly;
        #endregion

        #region Properties
        public string Process { get { return mProcess; } set { mProcess = value; } }
        public string Status { get { return mStatus; } set { mStatus = value; } }
        public string SubmittedBy { get { return mSubmittedBy; } set { mSubmittedBy = value; } }
        public string Email { get { return mEmail; } set { mEmail = value; } }
        public DateTime OGStartDay { get { return mOGStartDay; } set { mOGStartDay = value; } }
        public DateTime OGEndDay { get { return mOGEndDay; } set { mOGEndDay = value; } }
        public double OGDuration { get { return mOGDuration; } set { mOGDuration = value; } }
        public DateTime ActualStartDay { get { return mActualStartDay; } set { mActualStartDay = value; } }
        public DateTime ActualEndDay { get { return mActualEndDay; } set { mActualEndDay = value; } }
        public double ActualDuration { get { return mActualDuration; } set { mActualDuration = value; } }
        public double PixelsFromLeft { get { return mPixelsFromLeft; } set { mPixelsFromLeft = value; } }
        public double Width { get { return mWidth; } set { mWidth = value; } }
        public string Checks { get { return mChecks; } set { mChecks = value; } }
        public string Color { get { return mColor; } set { mColor = value; } }
        public string WorflowQuickStep { get { return mWorflowQuickStep; } set { mWorflowQuickStep = value; } }
        public string ReadOnly { get { return mReadOnly; } set { mReadOnly = value; } }
        
        #endregion
    }
}
