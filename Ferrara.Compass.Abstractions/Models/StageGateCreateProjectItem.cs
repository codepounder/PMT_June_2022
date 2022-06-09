using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    [Serializable]
    public class StageGateCreateProjectItem
    {
        public StageGateCreateProjectItem()
        {
        }

        #region Variables
        private int mStageGateProjectListItemId;
        private string mProjectName;
        private string mLineOfBusiness;
        private string mProjectTier;
        private string mNumberofNoveltySKUs;
        private DateTime mDesiredShipDate;
        private DateTime mRevisedShipDate;
        private string mPHL2;
        private string mBrand;
        private string mSKUs;
        private string mProjectType;
        private string mProjectTypeSubCategory;
        private string mBusinessFunction;
        private string mBusinessFunctionOther;
        private string mNewFinishedGood;
        private string mNewBaseFormula;
        private string mNewShape;
        private string mNewPackType;
        private string mNewNetWeight;
        private string mNewGraphics;
        private string mNewFlavorColor;
        private string mProjectConceptOverview;
        private string mStage;
        private int mTotalOnHoldDays;
        private DateTime mOnHoldStartDate;
        private string mPostLaunchActive;
        private string mProjectLeader;
        private string mProjectLeaderName;
        private string mProjectManager;
        private string mProjectManagerName;
        private string mSeniorProjectManager;
        private string mSeniorProjectManagerName;
        private string mMarketing;
        private string mMarketingName;
        private string mInTech;
        private string mInTechName;
        private string mQAInnovation;
        private string mQAInnovationName;
        private string mInTechRegulatory;
        private string mInTechRegulatoryName;
        private string mRegulatoryQA;
        private string mRegulatoryQAName;
        private string mPackagingEngineering;
        private string mPackagingEngineeringName;
        private string mSupplyChain;
        private string mSupplyChainName;
        private string mFinance;
        private string mFinanceName;
        private string mSales;
        private string mSalesName;
        private string mManufacturing;
        private string mManufacturingName;
        private string mTeamMembers;
        private string mTeamMembersNames;
        private string mExtMfgProcurement;
        private string mExtMfgProcurementName;
        private string mPackagingProcurement;
        private string mPackagingProcurementName;
        private string mLifeCycleManagement;
        private string mLifeCycleManagementName;
        private string mLegal;
        private string mLegalName;
        private string mOtherMember;
        private string mOtherMemberName;
        private string mAllUsers;
        private List<string> mEmailAddresses;


        private string mUploadProjectBrief;
        private string mOtherAttachments;
        private string mProjectNotes;
        private string mProjectNumber;
        private string mChangeLink;
        private string mCopyLink;
        private string mWorkflowStatusLink;
        private string mCommercializationLink;
        private DateTime mGate0ApprovedDate;
        private DateTime mFormSubmittedDate;
        private string mFormSubmittedBy;
        private DateTime mModifiedDate;
        private string mModifiedBy;
        private string mLastUpdatedFormName;
        private string mTestProject;
        private string mIPFStartDate;
        private string mIPFSubmitter;
        private string mProjectSubmittedSent;
        private string mProjectCompletedSent;
        private string mProjectCacnelledSent;
        private DateTime mCreatedDate;

        #endregion

        public int StageGateProjectListItemId { get { return mStageGateProjectListItemId; } set { mStageGateProjectListItemId = value; } }

        #region List Properties
        public string ProjectName { get { return mProjectName; } set { mProjectName = value; } }
        public string LineOfBisiness { get { return mLineOfBusiness; } set { mLineOfBusiness = value; } }
        public string ProjectTier { get { return mProjectTier; } set { mProjectTier = value; } }
        public string NumberofNoveltySKUs { get { return mNumberofNoveltySKUs; } set { mNumberofNoveltySKUs = value; } }
        public DateTime Gate0ApprovedDate { get { return mGate0ApprovedDate; } set { mGate0ApprovedDate = value; } }
        public DateTime DesiredShipDate { get { return mDesiredShipDate; } set { mDesiredShipDate = value; } }
        public DateTime RevisedShipDate { get { return mRevisedShipDate; } set { mRevisedShipDate = value; } }
        public string PHL2 { get { return mPHL2; } set { mPHL2 = value; } }
        public string Brand { get { return mBrand; } set { mBrand = value; } }
        public string SKUs { get { return mSKUs; } set { mSKUs = value; } }
        public string ProjectType { get { return mProjectType; } set { mProjectType = value; } }
        public string ProjectTypeSubCategory { get { return mProjectTypeSubCategory; } set { mProjectTypeSubCategory = value; } }
        public string BusinessFunction { get { return mBusinessFunction; } set { mBusinessFunction = value; } }
        public string BusinessFunctionOther { get { return mBusinessFunctionOther; } set { mBusinessFunctionOther = value; } }
        public string NewFinishedGood { get { return mNewFinishedGood; } set { mNewFinishedGood = value; } }
        public string NewBaseFormula { get { return mNewBaseFormula; } set { mNewBaseFormula = value; } }
        public string NewShape { get { return mNewShape; } set { mNewShape = value; } }
        public string NewPackType { get { return mNewPackType; } set { mNewPackType = value; } }
        public string NewNetWeight { get { return mNewNetWeight; } set { mNewNetWeight = value; } }
        public string NewGraphics { get { return mNewGraphics; } set { mNewGraphics = value; } }
        public string NewFlavorColor { get { return mNewFlavorColor; } set { mNewFlavorColor = value; } }
        public string ProjectConceptOverview { get { return mProjectConceptOverview; } set { mProjectConceptOverview = value; } }
        public string Stage { get { return mStage; } set { mStage = value; } }
        public int TotalOnHoldDays { get { return mTotalOnHoldDays; } set { mTotalOnHoldDays = value; } }
        public DateTime OnHoldStartDate { get { return mOnHoldStartDate; } set { mOnHoldStartDate = value; } }
        public string PostLaunchActive { get { return mPostLaunchActive; } set { mPostLaunchActive = value; } }
        public string ProjectLeader { get { return mProjectLeader; } set { mProjectLeader = value; } }
        public string ProjectLeaderName { get { return mProjectLeaderName; } set { mProjectLeaderName = value; } }
        public string ProjectManager { get { return mProjectManager; } set { mProjectManager = value; } }
        public string ProjectManagerName { get { return mProjectManagerName; } set { mProjectManagerName = value; } }
        public string SeniorProjectManager { get { return mSeniorProjectManager; } set { mSeniorProjectManager = value; } }
        public string SeniorProjectManagerName { get { return mSeniorProjectManagerName; } set { mSeniorProjectManagerName = value; } }
        public string Marketing { get { return mMarketing; } set { mMarketing = value; } }
        public string MarketingName { get { return mMarketingName; } set { mMarketingName = value; } }
        public string InTech { get { return mInTech; } set { mInTech = value; } }
        public string InTechName { get { return mInTechName; } set { mInTechName = value; } }
        public string QAInnovation { get { return mQAInnovation; } set { mQAInnovation = value; } }
        public string QAInnovationName { get { return mQAInnovationName; } set { mQAInnovationName = value; } }
        public string InTechRegulatory { get { return mInTechRegulatory; } set { mInTechRegulatory = value; } }
        public string InTechRegulatoryName { get { return mInTechRegulatoryName; } set { mInTechRegulatoryName = value; } }
        public string RegulatoryQA { get { return mRegulatoryQA; } set { mRegulatoryQA = value; } }
        public string RegulatoryQAName { get { return mRegulatoryQAName; } set { mRegulatoryQAName = value; } }
        public string PackagingEngineering { get { return mPackagingEngineering; } set { mPackagingEngineering = value; } }
        public string PackagingEngineeringName { get { return mPackagingEngineeringName; } set { mPackagingEngineeringName = value; } }
        public string SupplyChain { get { return mSupplyChain; } set { mSupplyChain = value; } }
        public string SupplyChainName { get { return mSupplyChainName; } set { mSupplyChainName = value; } }
        public string Finance { get { return mFinance; } set { mFinance = value; } }
        public string FinanceName { get { return mFinanceName; } set { mFinanceName = value; } }
        public string Sales { get { return mSales; } set { mSales = value; } }
        public string SalesName { get { return mSalesName; } set { mSalesName = value; } }
        public string Manufacturing { get { return mManufacturing; } set { mManufacturing = value; } }
        public string ManufacturingName { get { return mManufacturingName; } set { mManufacturingName = value; } }
        public string TeamMembers { get { return mTeamMembers; } set { mTeamMembers = value; } }
        public string TeamMembersNames { get { return mTeamMembersNames; } set { mTeamMembersNames = value; } }
        public string ExtMfgProcurement { get { return mExtMfgProcurement; } set { mExtMfgProcurement = value; } }
        public string ExtMfgProcurementName { get { return mExtMfgProcurementName; } set { mExtMfgProcurementName = value; } }
        public string PackagingProcurement { get { return mPackagingProcurement; } set { mPackagingProcurement = value; } }
        public string PackagingProcurementName { get { return mPackagingProcurementName; } set { mPackagingProcurementName = value; } }
        public string LifeCycleManagement { get { return mLifeCycleManagement; } set { mLifeCycleManagement = value; } }
        public string LifeCycleManagementName { get { return mLifeCycleManagementName; } set { mLifeCycleManagementName = value; } }
        public string Legal { get { return mLegal; } set { mLegal = value; } }
        public string LegalName { get { return mLegalName; } set { mLegalName = value; } }
        public string OtherMember { get { return mOtherMember; } set { mOtherMember = value; } }
        public string OtherMemberName { get { return mOtherMemberName; } set { mOtherMemberName = value; } }
        public string AllUsers { get { return mAllUsers; } set { mAllUsers = value; } }
        public List<string> EmailAddresses { get { return mEmailAddresses; } set { mEmailAddresses = value; } }
        public string UploadProjectBrief { get { return mUploadProjectBrief; } set { mUploadProjectBrief = value; } }
        public string OtherAttachments { get { return mOtherAttachments; } set { mOtherAttachments = value; } }
        public string ProjectNotes { get { return mProjectNotes; } set { mProjectNotes = value; } }
        public string ProjectNumber { get { return mProjectNumber; } set { mProjectNumber = value; } }
        public string IPFStartDate { get { return mIPFStartDate; } set { mIPFStartDate = value; } }
        public string IPFSubmitter { get { return mIPFSubmitter; } set { mIPFSubmitter = value; } }
        public string ProjectSubmittedSent { get { return mProjectSubmittedSent; } set { mProjectSubmittedSent = value; } }
        public string ProjectCompletedSent { get { return mProjectCompletedSent; } set { mProjectCompletedSent = value; } }
        public string ProjectCacnelledSent { get { return mProjectCacnelledSent; } set { mProjectCacnelledSent = value; } }

        #endregion

        #region FormLinks
        public string CommercializationLink { get { return mCommercializationLink; } set { mCommercializationLink = value; } }
        public string ChangeLink { get { return mChangeLink; } set { mChangeLink = value; } }
        public string CopyLink { get { return mCopyLink; } set { mCopyLink = value; } }
        public string WorkflowStatusLink { get { return mWorkflowStatusLink; } set { mWorkflowStatusLink = value; } }
        public string LastUpdatedFormName { get { return mLastUpdatedFormName; } set { mLastUpdatedFormName = value; } }
        public DateTime CreatedDate { get { return mCreatedDate; } set { mCreatedDate = value; } }
        #endregion

        public DateTime FormSubmittedDate { get { return mFormSubmittedDate; } set { mFormSubmittedDate = value; } }
        public string FormSubmittedBy { get { return mFormSubmittedBy; } set { mFormSubmittedBy = value; } }
        public DateTime ModifiedDate { get { return mModifiedDate; } set { mModifiedDate = value; } }
        public string ModifiedBy { get { return mModifiedBy; } set { mModifiedBy = value; } }
        public string TestProject { get { return mTestProject; } set { mTestProject = value; } }

    }
}
