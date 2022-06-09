using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferrara.Compass.Abstractions.Models
{
    public class QAItem
    {
        public QAItem()
        {
        }

        #region Member Variables
        private int mCompassListItemId;
        private string mLastUpdatedFormName;
        private string mProjectType;

        private string mNewFormula;

        private string mPackingLocation;
        private string mManufacturingLocation;
        private string mMakeCountryOfOrigin;
        private string mCoManufacturingClassification;
        private string mExistingBulkSemiNumber;
        private string mBulkSemiDescription;
        private string mTrialsCompleted;
        private string mTBDIndicator;
        private string mItemConcept;
        private string mCaseType;
        private string mPM;
        private string mPMName;
        private string mMarketing;
        private string mMarketingName;
        private string mMarketingClaimsLabeling;
        private string mSAPItemNumber;
        private string mSAPDescription;
        private string mIsRegulatoryinformationCorrect;
        private string mWhatRegulatoryInfoIsIncorrect;
        private string mDoYouApproveThisProjectToProceed;
        #endregion

        #region Properties
        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        public string LastUpdatedFormName { get { return mLastUpdatedFormName; } set { mLastUpdatedFormName = value; } }
        public string ProjectType { get { return mProjectType; } set { mProjectType = value; } }

        public string NewFormula { get { return mNewFormula; } set { mNewFormula = value; } }
        public string CoManufacturingClassification { get { return mCoManufacturingClassification; } set { mCoManufacturingClassification = value; } }
        public string ExistingBulkSemiNumber { get { return mExistingBulkSemiNumber; } set { mExistingBulkSemiNumber = value; } }
        public string BulkSemiDescription { get { return mBulkSemiDescription; } set { mBulkSemiDescription = value; } }
        public string TrialsCompleted { get { return mTrialsCompleted; } set { mTrialsCompleted = value; } }

        #endregion

        public string ManufacturingLocation { get { return mManufacturingLocation; } set { mManufacturingLocation = value; } }
        public string MakeCountryOfOrigin { get { return mMakeCountryOfOrigin; } set { mMakeCountryOfOrigin = value; } }
        public string PackingLocation { get { return mPackingLocation; } set { mPackingLocation = value; } }
        public string TBDIndicator { get { return mTBDIndicator; } set { mTBDIndicator = value; } }
        public string ItemConcept { get { return mItemConcept; } set { mItemConcept = value; } }
        public string CaseType { get { return mCaseType; } set { mCaseType = value; } }
        public string PM { get { return mPM; } set { mPM = value; } }
        public string PMName { get { return mPMName; } set { mPMName = value; } }
        public string Marketing { get { return mMarketing; } set { mMarketing = value; } }
        public string MarketingName { get { return mMarketingName; } set { mMarketingName = value; } }
        public string MarketingClaimsLabeling { get { return mMarketingClaimsLabeling; } set { mMarketingClaimsLabeling = value; } }
        public string SAPItemNumber { get { return mSAPItemNumber; } set { mSAPItemNumber = value; } }
        public string SAPDescription { get { return mSAPDescription; } set { mSAPDescription = value; } }
        public string IsRegulatoryinformationCorrect { get { return mIsRegulatoryinformationCorrect; } set { mIsRegulatoryinformationCorrect = value; } }
        public string WhatRegulatoryInfoIsIncorrect { get { return mWhatRegulatoryInfoIsIncorrect; } set { mWhatRegulatoryInfoIsIncorrect = value; } }
        public string DoYouApproveThisProjectToProceed { get { return mDoYouApproveThisProjectToProceed; } set { mDoYouApproveThisProjectToProceed = value; } }

    }
}

