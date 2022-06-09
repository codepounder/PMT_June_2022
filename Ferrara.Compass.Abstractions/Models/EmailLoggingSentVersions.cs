using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.Abstractions.Models
{
    public class EmailLoggingSentVersions
    {
        private int emailLoggingListItemId;
        private int compassListItemId;

        public int EmailLoggingListItemId { get { return emailLoggingListItemId; } set { emailLoggingListItemId = value; } }
        public int CompassListItemId { get { return compassListItemId; } set { compassListItemId = value; } }

        private List<string> mSIRSAPEmailSentVersions = new List<string>();
        private List<string> mPrelimSIRSAPEmailSentVersions = new List<string>();
        private List<string> mOPSMFGEmailSentVersions = new List<string>();
        private List<string> mDISTEmailSentVersions = new List<string>();
        private List<string> mOPSICREmailSentVersions = new List<string>();
        private List<string> mQAQAEmailSentVersions = new List<string>();
        private List<string> mCOMANCOMANEmailSentVersions = new List<string>();
        private List<string> mCMFCMEmailSentVersions = new List<string>();
        private List<string> mSSRSAPEmailSentVersions = new List<string>();
        private List<string> mOBMREV1EmailSentVersions = new List<string>();
        private List<string> mTBDSAPEmailSentVersions = new List<string>();
        private List<string> mICAPEmailSentVersions = new List<string>();
        private List<string> mICSTEmailSentVersions = new List<string>();
        private List<string> mIAPPEmailSentVersions = new List<string>();
        private List<string> mMNPEEmailSentVersions = new List<string>();
        private List<string> mMNPUREmailSentVersions = new List<string>();
        private List<string> mMNPE2EmailSentVersions = new List<string>();
        private List<string> mMNSAPEmailSentVersions = new List<string>();
        private List<string> mRGFOBMREV2EmailSentVersions = new List<string>();
        private List<string> mRGFGREmailSentVersions = new List<string>();
        private List<string> mSISSAPEmailSentVersions = new List<string>();
        private List<string> mGPPGREmailSentVersions = new List<string>();

        public List<string> SIRSAPEmailSentVersions() { return mSIRSAPEmailSentVersions; }
        public List<string> PrelimSIRSAPEmailSentVersions() { return mPrelimSIRSAPEmailSentVersions; }
        public List<string> OPSMFGEmailSentVersions() { return mOPSMFGEmailSentVersions; } 
        public List<string> DISTEmailSentVersions() { return mDISTEmailSentVersions; } 
        public List<string> OPSICREmailSentVersions() { return mOPSICREmailSentVersions; } 
        public List<string> QAQAEmailSentVersions() { return mQAQAEmailSentVersions; } 
        public List<string> COMANCOMANEmailSentVersions() { return mCOMANCOMANEmailSentVersions; }
        public List<string> CMFCMEmailSentVersions() { return mCMFCMEmailSentVersions; }
        public List<string> SSRSAPEmailSentVersions() { return mSSRSAPEmailSentVersions; } 
        public List<string> OBMREV1EmailSentVersions() { return mOBMREV1EmailSentVersions; } 
        public List<string> TBDSAPEmailSentVersions() { return mTBDSAPEmailSentVersions; } 
        public List<string> ICAPEmailSentVersions() { return mICAPEmailSentVersions; } 
        public List<string> ICSTEmailSentVersions() { return mICSTEmailSentVersions; } 
        public List<string> IAPPEmailSentVersions() { return mIAPPEmailSentVersions; } 
        public List<string> MNPEEmailSentVersions() { return mMNPEEmailSentVersions; } 
        public List<string> MNPUREmailSentVersions() { return mMNPUREmailSentVersions; } 
        public List<string> MNPE2EmailSentVersions() { return mMNPE2EmailSentVersions; } 
        public List<string> MNSAPEmailSentVersions() { return mMNSAPEmailSentVersions; } 
        public List<string> RGFOBMREV2EmailSentVersions() { return mRGFOBMREV2EmailSentVersions; } 
        public List<string> RGFGREmailSentVersions() { return mRGFGREmailSentVersions; } 
        public List<string> SISSAPEmailSentVersions() { return mSISSAPEmailSentVersions; }
        public List<string> GPPGREmailSentVersions() { return mGPPGREmailSentVersions; } 

        public void AddVersion(string version, WorkflowStep wfStep)
        {
            if (string.Equals(wfStep.ToString(), WorkflowStep.SrOBMApproval.ToString()))
                mIAPPEmailSentVersions.Add(version);
            else if (string.Equals(wfStep.ToString(), WorkflowStep.SAPInitialSetup.ToString()))
                mSIRSAPEmailSentVersions.Add(version);
            else if (string.Equals(wfStep.ToString(), WorkflowStep.PrelimSAPInitialSetup.ToString()))
                mPrelimSIRSAPEmailSentVersions.Add(version);
            else if (string.Equals(wfStep.ToString(), WorkflowStep.Operations.ToString()))
                mOPSMFGEmailSentVersions.Add(version);
            else if (string.Equals(wfStep.ToString(), WorkflowStep.Distribution.ToString()))
                mDISTEmailSentVersions.Add(version);
            else if (string.Equals(wfStep.ToString(), WorkflowStep.QA.ToString()))
                mQAQAEmailSentVersions.Add(version);
            else if (string.Equals(wfStep.ToString(), WorkflowStep.ExternalMfg.ToString()))
                mCOMANCOMANEmailSentVersions.Add(version);
            else if (string.Equals(wfStep.ToString(), WorkflowStep.TradePromo.ToString()))
                mCMFCMEmailSentVersions.Add(version);
            else if (string.Equals(wfStep.ToString(), WorkflowStep.OBMReview1.ToString()))
                mOBMREV1EmailSentVersions.Add(version);
            else if (string.Equals(wfStep.ToString(), WorkflowStep.BOMSetupPE.ToString()))
                mMNPEEmailSentVersions.Add(version);
            else if (string.Equals(wfStep.ToString(), WorkflowStep.BOMSetupProc.ToString()))
                mMNPUREmailSentVersions.Add(version);
            else if (string.Equals(wfStep.ToString(), WorkflowStep.BOMSetupPE2.ToString()))
                mMNPE2EmailSentVersions.Add(version);
            else if (string.Equals(wfStep.ToString(), WorkflowStep.SAPBOMSetup.ToString()))
                mMNSAPEmailSentVersions.Add(version);
        }

        public string GetVersionDisplay(List<string> versionList)
        {
            if (versionList == null)
                return string.Empty;

            string versions = string.Empty;
            string lastVersion = string.Empty;
            foreach (string str in versionList)
            {
                if (!string.Equals(str, lastVersion))
                    versions = string.Concat(versions, str, "<br>");
                lastVersion = str;
            }
            return versions;
        }

    }
}
