using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferrara.Compass.ProjectTimelineCalculator.Models
{
    public class ApprovalListItem
    {
        public ApprovalListItem()
        {
        }

        private int approvalListItemId;
        private int compassListItemId;

        public int ApprovalListItemId { get { return approvalListItemId; } set { approvalListItemId = value; } }
        public int CompassListItemId { get { return compassListItemId; } set { compassListItemId = value; } }

        #region Variables
        #region IPF Variables

        // Submitted Users/Dates
        private string mIPFSubmittedDate;
        private string mIPFSubmittedBy;

        private string mIPFResubmittedStartDate;
        private string mIPFResubmittedDate;
        private string mIPFResubmittedBy;
        private string mIPFModifiedDate;
        private string mIPFModifiedBy;

        #endregion

        #region Initial Approval and 2nd Approval Variables
        private string mSrOBMApproval_StartDate;
        private string mSrOBMApproval_ModifiedDate;
        private string mSrOBMApproval_ModifiedBy;
        private string mSrOBMApproval_SubmittedDate;
        private string mSrOBMApproval_SubmittedBy;

        private string mSrOBMApproval2_StartDate;
        private string mSrOBMApproval2_ModifiedDate;
        private string mSrOBMApproval2_ModifiedBy;
        private string mSrOBMApproval2_SubmittedDate;
        private string mSrOBMApproval2_SubmittedBy;
        #endregion

        #region Initial Costing Variables
        private string mInitialCosting_StartDate;
        private string mInitialCosting_ModifiedBy;
        private string mInitialCosting_ModifiedDate;
        private string mInitialCosting_SubmittedBy;
        private string mInitialCosting_SubmittedDate;
        #endregion

        #region Trade Promo Group Variables
        private string mTradePromo_StartDate;
        private string mTradePromo_ModifiedDate;
        private string mTradePromo_ModifiedBy;
        private string mTradePromo_SubmittedDate;
        private string mTradePromo_SubmittedBy;
        #endregion
        #region Estimated Pricing
        private string mEstPricing_ModifiedDate;
        private string mEstPricing_SubmittedDate;
        private string mEstPricing_StartDate;
        private string mEstPricing_ModifiedBy;
        private string mEstPricing_SubmittedBy;
        #endregion
        #region Estimated Bracket Pricing
        private string mEstBracketPricing_ModifiedDate;
        private string mEstBracketPricing_SubmittedDate;
        private string mEstBracketPricing_StartDate;
        private string mEstBracketPricing_ModifiedBy;
        private string mEstBracketPricing_SubmittedBy;
        #endregion
        #region Distribution Variables
        private string mDistribution_StartDate;
        private string mDistribution_ModifiedDate;
        private string mDistribution_ModifiedBy;
        private string mDistribution_SubmittedDate;
        private string mDistribution_SubmittedBy;
        #endregion

        #region Operations Variables
        private string mOperations_StartDate;
        private string mOperations_ModifiedDate;
        private string mOperations_ModifiedBy;
        private string mOperations_SubmittedDate;
        private string mOperations_SubmittedBy;
        #endregion

        #region SAP Item Request Form Variables
        private string mSAPInitialSetup_ModifiedDate;
        private string mSAPInitialSetup_SubmittedDate;
        private string mSAPInitialSetup_StartDate;
        private string mSAPInitialSetup_ModifiedBy;
        private string mSAPInitialSetup_SubmittedBy;
        #endregion

        #region SAP Item Request Form Variables
        private string mPrelimSAPInitialSetup_ModifiedDate;
        private string mPrelimSAPInitialSetup_SubmittedDate;
        private string mPrelimSAPInitialSetup_StartDate;
        private string mPrelimSAPInitialSetup_ModifiedBy;
        private string mPrelimSAPInitialSetup_SubmittedBy;
        #endregion

        #region QA Variables
        private string mQA_StartDate;
        private string mQA_ModifiedDate;
        private string mQA_ModifiedBy;
        private string mQA_SubmittedDate;
        private string mQA_SubmittedBy;
        #endregion

        #region OBM Review 1 Variables
        private string mOBMReview1_StartDate;
        private string mOBMReview1_ModifiedDate;
        private string mOBMReview1_ModifiedBy;
        private string mOBMReview1_SubmittedDate;
        private string mOBMReview1_SubmittedBy;
        #endregion
        
        #region Packaging Variables
        private string mBOMSetupPE_StartDate;
        private string mBOMSetupPE_ModifiedDate;
        private string mBOMSetupPE_ModifiedBy;
        private string mBOMSetupPE_SubmittedDate;
        private string mBOMSetupPE_SubmittedBy;

        private string mBOMSetupProc_StartDate;
        private string mBOMSetupProc_ModifiedDate;
        private string mBOMSetupProc_ModifiedBy;
        private string mBOMSetupProc_SubmittedDate;
        private string mBOMSetupProc_SubmittedBy;

        private string mProcAncillary_SubmittedBy;
        private string mProcCorrugated_SubmittedBy;
        private string mProcPurchased_SubmittedBy;
        private string mProcFilm_SubmittedBy;
        private string mProcLabel_SubmittedBy;
        private string mProcMetal_SubmittedBy;
        private string mProcOther_SubmittedBy;
        private string mProcPaperboard_SubmittedBy;
        private string mProcRigidPlastic_SubmittedBy;
        private string mProcNovelty_SubmittedBy;
        private string mProcCoMan_SubmittedBy;
        private string mProcSeasonal_SubmittedBy;
        private string mProcExternal_SubmittedBy;
        private string mProcExternalAncillary_SubmittedBy;
        private string mProcExternalCorrugated_SubmittedBy;
        private string mProcExternalPurchased_SubmittedBy;
        private string mProcExternalFilm_SubmittedBy;
        private string mProcExternalLabel_SubmittedBy;
        private string mProcExternalMetal_SubmittedBy;
        private string mProcExternalOther_SubmittedBy;
        private string mProcExternalPaperboard_SubmittedBy;
        private string mProcExternalRigidPlastic_SubmittedBy;

        private string mProcExternalAncillary_SubmittedDate;
        private string mProcExternalCorrugated_SubmittedDate;
        private string mProcExternalPurchased_SubmittedDate;
        private string mProcExternalFilm_SubmittedDate;
        private string mProcExternalLabel_SubmittedDate;
        private string mProcExternalMetal_SubmittedDate;
        private string mProcExternalOther_SubmittedDate;
        private string mProcExternalPaperboard_SubmittedDate;
        private string mProcExternalRigidPlastic_SubmittedDate;
        private string mProcAncillary_SubmittedDate;
        private string mProcCorrugated_SubmittedDate;
        private string mProcPurchased_SubmittedDate;
        private string mProcFilm_SubmittedDate;
        private string mProcLabel_SubmittedDate;
        private string mProcMetal_SubmittedDate;
        private string mProcOther_SubmittedDate;
        private string mProcPaperboard_SubmittedDate;
        private string mProcRigidPlastic_SubmittedDate;
        private string mProcNovelty_SubmittedDate;
        private string mProcCoMan_SubmittedDate;
        private string mProcSeasonal_SubmittedDate;
        private string mProcExternal_SubmittedDate;

        private string mBOMSetupPE2_StartDate;
        private string mBOMSetupPE2_ModifiedDate;
        private string mBOMSetupPE2_ModifiedBy;
        private string mBOMSetupPE2_SubmittedDate;
        private string mBOMSetupPE2_SubmittedBy;

        private string mBOMSetupPE3_StartDate;
        private string mBOMSetupPE3_ModifiedDate;
        private string mBOMSetupPE3_ModifiedBy;
        private string mBOMSetupPE3_SubmittedDate;
        private string mBOMSetupPE3_SubmittedBy;

        private string mMatrlWHSetUp_StartDate;
        private string mMatrlWHSetUp_ModifiedDate;
        private string mMatrlWHSetUp_ModifiedBy;
        private string mMatrlWHSetUp_SubmittedDate;
        private string mMatrlWHSetUp_SubmittedBy;

        private string mSAPCompleteItem_StartDate;
        private string mSAPCompleteItem_ModifiedDate;
        private string mSAPCompleteItem_ModifiedBy;
        private string mSAPCompleteItem_SubmittedDate;
        private string mSAPCompleteItem_SubmittedBy;

        private string mBEQRC_StartDate;
        private string mBEQRC_ModifiedDate;
        private string mBEQRC_ModifiedBy;
        private string mBEQRC_SubmittedDate;
        private string mBEQRC_SubmittedBy;
        #endregion

        #region OBM Review 2 Variables
        private string mOBMReview2_StartDate;
        private string mOBMReview2_ModifiedDate;
        private string mOBMReview2_ModifiedBy;
        private string mOBMReview2_SubmittedDate;
        private string mOBMReview2_SubmittedBy;
        #endregion

        #region Graphics Variables
        private string mGRAPHICS_StartDate;
        private string mGRAPHICS_ModifiedDate;
        private string mGRAPHICS_ModifiedBy;
        private string mGRAPHICS_SubmittedDate;
        private string mGRAPHICS_SubmittedBy;
        #endregion

        #region CostingQuote Variables
        private string mCompCostSeasonal_StartDate;
        private string mCompCostSeasonal_ModifiedDate;
        private string mCompCostSeasonal_ModifiedBy;
        private string mCompCostSeasonal_SubmittedDate;
        private string mCompCostSeasonal_SubmittedBy;

        private string mCompCostCorrPaper_StartDate;
        private string mCompCostCorrPaper_ModifiedDate;
        private string mCompCostCorrPaper_ModifiedBy;
        private string mCompCostCorrPaper_SubmittedDate;
        private string mCompCostCorrPaper_SubmittedBy;

        private string mCompCostFLRP_StartDate;
        private string mCompCostFLRP_ModifiedDate;
        private string mCompCostFLRP_ModifiedBy;
        private string mCompCostFLRP_SubmittedDate;
        private string mCompCostFLRP_SubmittedBy;
        #endregion

        #region FGPackSpec Variables
        private string mFGPackSpec_StartDate;
        private string mFGPackSpec_ModifiedDate;
        private string mFGPackSpec_ModifiedBy;
        private string mFGPackSpec_SubmittedDate;
        private string mFGPackSpec_SubmittedBy;
        #endregion

        #region SAPBOMSetup Variables
        private string mSAPBOMSetup_StartDate;
        private string mSAPBOMSetup_ModifiedDate;
        private string mSAPBOMSetup_ModifiedBy;
        private string mSAPBOMSetup_SubmittedDate;
        private string mSAPBOMSetup_SubmittedBy;
        #endregion

        #region OBM Review 3 Variables
        private string mOBMReview3_StartDate;
        private string mOBMReview3_ModifiedDate;
        private string mOBMReview3_ModifiedBy;
        private string mOBMReview3_SubmittedDate;
        private string mOBMReview3_SubmittedBy;
        #endregion

        #region External Mfg Variables
        private string mExternalMfg_StartDate;
        private string mExternalMfg_ModifiedDate;
        private string mExternalMfg_ModifiedBy;
        private string mExternalMfg_SubmittedDate;
        private string mExternalMfg_SubmittedBy;
        #endregion

        #region SAP Routing Setup Variables
        private string mSAPRoutingSetup_StartDate;
        private string mSAPRoutingSetup_ModifiedDate;
        private string mSAPRoutingSetup_ModifiedBy;
        private string mSAPRoutingSetup_SubmittedDate;
        private string mSAPRoutingSetup_SubmittedBy;
        #endregion


        #region Final Costing Variables

        private string mFCST_Comments;
        private string mFCST_StartDate;
        private string mFCST_ModifiedDate;
        private string mFCST_ModifiedBy;
        private string mFCST_SubmittedDate;
        private string mFCST_SubmittedBy;

        #endregion


        #region Other Workflow State Changes Properties
        private string mOnHold_ModifiedDate;
        private string mOnHold_ModifiedBy;

        private string mPreProduction_ModifiedDate;
        private string mPreProduction_ModifiedBy;

        private string mCompleted_ModifiedDate;
        private string mCompleted_ModifiedBy;

        private string mCancelled_ModifiedDate;
        private string mCancelled_ModifiedBy;

        private string mProductionCompleted_ModifiedDate;
        private string mProductionCompleted_ModifiedBy;
        #endregion

        #endregion

        #region Properties
        #region IPF Properties

        public string IPF_SubmittedBy { get { return mIPFSubmittedBy; } set { mIPFSubmittedBy = value; } }
        public string IPF_SubmittedDate { get { return mIPFSubmittedDate; } set { mIPFSubmittedDate = value; } }
        public string IPF_ResubmittedStartDate { get { return mIPFResubmittedStartDate; } set { mIPFResubmittedStartDate = value; } }
        public string IPF_ResubmittedDate { get { return mIPFResubmittedDate; } set { mIPFResubmittedDate = value; } }
        public string IPF_ResubmittedBy { get { return mIPFResubmittedBy; } set { mIPFResubmittedBy = value; } }
        public string IPF_ModifiedDate { get { return mIPFModifiedDate; } set { mIPFModifiedDate = value; } }
        public string IPF_ModifiedBy { get { return mIPFModifiedBy; } set { mIPFModifiedBy = value; } }

        #endregion

        #region Initial Approval Review 1 & 2

        public string SrOBMApproval_ModifiedDate { get { return mSrOBMApproval_ModifiedDate; } set { mSrOBMApproval_ModifiedDate = value; } }
        public string SrOBMApproval_SubmittedDate { get { return mSrOBMApproval_SubmittedDate; } set { mSrOBMApproval_SubmittedDate = value; } }
        public string SrOBMApproval_StartDate { get { return mSrOBMApproval_StartDate; } set { mSrOBMApproval_StartDate = value; } }
        public string SrOBMApproval_ModifiedBy { get { return mSrOBMApproval_ModifiedBy; } set { mSrOBMApproval_ModifiedBy = value; } }
        public string SrOBMApproval_SubmittedBy { get { return mSrOBMApproval_SubmittedBy; } set { mSrOBMApproval_SubmittedBy = value; } }


        public string SrOBMApproval2_ModifiedDate { get { return mSrOBMApproval2_ModifiedDate; } set { mSrOBMApproval2_ModifiedDate = value; } }
        public string SrOBMApproval2_SubmittedDate { get { return mSrOBMApproval2_SubmittedDate; } set { mSrOBMApproval2_SubmittedDate = value; } }
        public string SrOBMApproval2_StartDate { get { return mSrOBMApproval2_StartDate; } set { mSrOBMApproval2_StartDate = value; } }
        public string SrOBMApproval2_ModifiedBy { get { return mSrOBMApproval2_ModifiedBy; } set { mSrOBMApproval2_ModifiedBy = value; } }
        public string SrOBMApproval2_SubmittedBy { get { return mSrOBMApproval2_SubmittedBy; } set { mSrOBMApproval2_SubmittedBy = value; } }
        #endregion

        #region Initial Costing Properties
        public string InitialCosting_StartDate { get { return mInitialCosting_StartDate; } set { mInitialCosting_StartDate = value; } }
        public string InitialCosting_ModifiedBy { get { return mInitialCosting_ModifiedBy; } set { mInitialCosting_ModifiedBy = value; } }
        public string InitialCosting_ModifiedDate { get { return mInitialCosting_ModifiedDate; } set { mInitialCosting_ModifiedDate = value; } }
        public string InitialCosting_SubmittedBy { get { return mInitialCosting_SubmittedBy; } set { mInitialCosting_SubmittedBy = value; } }
        public string InitialCosting_SubmittedDate { get { return mInitialCosting_SubmittedDate; } set { mInitialCosting_SubmittedDate = value; } }

        #endregion

        #region Trade Promo Group Properties
        public string TradePromo_ModifiedDate { get { return mTradePromo_ModifiedDate; } set { mTradePromo_ModifiedDate = value; } }
        public string TradePromo_SubmittedDate { get { return mTradePromo_SubmittedDate; } set { mTradePromo_SubmittedDate = value; } }
        public string TradePromo_StartDate { get { return mTradePromo_StartDate; } set { mTradePromo_StartDate = value; } }
        public string TradePromo_ModifiedBy { get { return mTradePromo_ModifiedBy; } set { mTradePromo_ModifiedBy = value; } }
        public string TradePromo_SubmittedBy { get { return mTradePromo_SubmittedBy; } set { mTradePromo_SubmittedBy = value; } }

        #endregion
        #region Estimated Pricing
        public string EstPricing_ModifiedDate { get { return mEstPricing_ModifiedDate; } set { mEstPricing_ModifiedDate = value; } }
        public string EstPricing_SubmittedDate { get { return mEstPricing_SubmittedDate; } set { mEstPricing_SubmittedDate = value; } }
        public string EstPricing_StartDate { get { return mEstPricing_StartDate; } set { mEstPricing_StartDate = value; } }
        public string EstPricing_ModifiedBy { get { return mEstPricing_ModifiedBy; } set { mEstPricing_ModifiedBy = value; } }
        public string EstPricing_SubmittedBy { get { return mEstPricing_SubmittedBy; } set { mEstPricing_SubmittedBy = value; } }

        #endregion
        #region Estimated Bracket Pricing
        public string EstBracketPricing_ModifiedDate { get { return mEstBracketPricing_ModifiedDate; } set { mEstBracketPricing_ModifiedDate = value; } }
        public string EstBracketPricing_SubmittedDate { get { return mEstBracketPricing_SubmittedDate; } set { mEstBracketPricing_SubmittedDate = value; } }
        public string EstBracketPricing_StartDate { get { return mEstBracketPricing_StartDate; } set { mEstBracketPricing_StartDate = value; } }
        public string EstBracketPricing_ModifiedBy { get { return mEstBracketPricing_ModifiedBy; } set { mEstBracketPricing_ModifiedBy = value; } }
        public string EstBracketPricing_SubmittedBy { get { return mEstBracketPricing_SubmittedBy; } set { mEstBracketPricing_SubmittedBy = value; } }
        #endregion
        #region Distribution Properties

        public string Distribution_ModifiedDate { get { return mDistribution_ModifiedDate; } set { mDistribution_ModifiedDate = value; } }
        public string Distribution_SubmittedDate { get { return mDistribution_SubmittedDate; } set { mDistribution_SubmittedDate = value; } }
        public string Distribution_StartDate { get { return mDistribution_StartDate; } set { mDistribution_StartDate = value; } }
        public string Distribution_ModifiedBy { get { return mDistribution_ModifiedBy; } set { mDistribution_ModifiedBy = value; } }
        public string Distribution_SubmittedBy { get { return mDistribution_SubmittedBy; } set { mDistribution_SubmittedBy = value; } }

        #endregion

        #region Make Pack Properties

        public string Operations_ModifiedDate { get { return mOperations_ModifiedDate; } set { mOperations_ModifiedDate = value; } }
        public string Operations_SubmittedDate { get { return mOperations_SubmittedDate; } set { mOperations_SubmittedDate = value; } }
        public string Operations_StartDate { get { return mOperations_StartDate; } set { mOperations_StartDate = value; } }
        public string Operations_ModifiedBy { get { return mOperations_ModifiedBy; } set { mOperations_ModifiedBy = value; } }
        public string Operations_SubmittedBy { get { return mOperations_SubmittedBy; } set { mOperations_SubmittedBy = value; } }

        #endregion

        #region SAP Item Request Properties

        public string SAPInitialSetup_ModifiedDate { get { return mSAPInitialSetup_ModifiedDate; } set { mSAPInitialSetup_ModifiedDate = value; } }
        public string SAPInitialSetup_SubmittedDate { get { return mSAPInitialSetup_SubmittedDate; } set { mSAPInitialSetup_SubmittedDate = value; } }
        public string SAPInitialSetup_StartDate { get { return mSAPInitialSetup_StartDate; } set { mSAPInitialSetup_StartDate = value; } }
        public string SAPInitialSetup_ModifiedBy { get { return mSAPInitialSetup_ModifiedBy; } set { mSAPInitialSetup_ModifiedBy = value; } }
        public string SAPInitialSetup_SubmittedBy { get { return mSAPInitialSetup_SubmittedBy; } set { mSAPInitialSetup_SubmittedBy = value; } }

        #endregion
        #region Preliminary SAP Item Request Properties

        public string PrelimSAPInitialSetup_ModifiedDate { get { return mPrelimSAPInitialSetup_ModifiedDate; } set { mPrelimSAPInitialSetup_ModifiedDate = value; } }
        public string PrelimSAPInitialSetup_SubmittedDate { get { return mPrelimSAPInitialSetup_SubmittedDate; } set { mPrelimSAPInitialSetup_SubmittedDate = value; } }
        public string PrelimSAPInitialSetup_StartDate { get { return mPrelimSAPInitialSetup_StartDate; } set { mPrelimSAPInitialSetup_StartDate = value; } }
        public string PrelimSAPInitialSetup_ModifiedBy { get { return mPrelimSAPInitialSetup_ModifiedBy; } set { mPrelimSAPInitialSetup_ModifiedBy = value; } }
        public string PrelimSAPInitialSetup_SubmittedBy { get { return mPrelimSAPInitialSetup_SubmittedBy; } set { mPrelimSAPInitialSetup_SubmittedBy = value; } }

        #endregion

        #region QA Properties

        public string QA_ModifiedDate { get { return mQA_ModifiedDate; } set { mQA_ModifiedDate = value; } }
        public string QA_SubmittedDate { get { return mQA_SubmittedDate; } set { mQA_SubmittedDate = value; } }
        public string QA_StartDate { get { return mQA_StartDate; } set { mQA_StartDate = value; } }
        public string QA_ModifiedBy { get { return mQA_ModifiedBy; } set { mQA_ModifiedBy = value; } }
        public string QA_SubmittedBy { get { return mQA_SubmittedBy; } set { mQA_SubmittedBy = value; } }

        #endregion

        #region OBM First Review Variables

        public string OBMReview1_StartDate { get { return mOBMReview1_StartDate; } set { mOBMReview1_StartDate = value; } }
        public string OBMReview1_ModifiedDate { get { return mOBMReview1_ModifiedDate; } set { mOBMReview1_ModifiedDate = value; } }
        public string OBMReview1_ModifiedBy { get { return mOBMReview1_ModifiedBy; } set { mOBMReview1_ModifiedBy = value; } }
        public string OBMReview1_SubmittedDate { get { return mOBMReview1_SubmittedDate; } set { mOBMReview1_SubmittedDate = value; } }
        public string OBMReview1_SubmittedBy { get { return mOBMReview1_SubmittedBy; } set { mOBMReview1_SubmittedBy = value; } }

        #endregion

        #region Packaging Properties

        public string BOMSetupPE_ModifiedDate { get { return mBOMSetupPE_ModifiedDate; } set { mBOMSetupPE_ModifiedDate = value; } }
        public string BOMSetupPE_SubmittedDate { get { return mBOMSetupPE_SubmittedDate; } set { mBOMSetupPE_SubmittedDate = value; } }
        public string BOMSetupPE_StartDate { get { return mBOMSetupPE_StartDate; } set { mBOMSetupPE_StartDate = value; } }
        public string BOMSetupPE_ModifiedBy { get { return mBOMSetupPE_ModifiedBy; } set { mBOMSetupPE_ModifiedBy = value; } }
        public string BOMSetupPE_SubmittedBy { get { return mBOMSetupPE_SubmittedBy; } set { mBOMSetupPE_SubmittedBy = value; } }

        public string BOMSetupProc_ModifiedDate { get { return mBOMSetupProc_ModifiedDate; } set { mBOMSetupProc_ModifiedDate = value; } }
        public string BOMSetupProc_SubmittedDate { get { return mBOMSetupProc_SubmittedDate; } set { mBOMSetupProc_SubmittedDate = value; } }
        public string BOMSetupProc_StartDate { get { return mBOMSetupProc_StartDate; } set { mBOMSetupProc_StartDate = value; } }
        public string BOMSetupProc_ModifiedBy { get { return mBOMSetupProc_ModifiedBy; } set { mBOMSetupProc_ModifiedBy = value; } }
        public string BOMSetupProc_SubmittedBy { get { return mBOMSetupProc_SubmittedBy; } set { mBOMSetupProc_SubmittedBy = value; } }

        public string ProcAncillary_SubmittedDate { get { return mProcAncillary_SubmittedDate; } set { mProcAncillary_SubmittedDate = value; } }
        public string ProcCorrugated_SubmittedDate { get { return mProcCorrugated_SubmittedDate; } set { mProcCorrugated_SubmittedDate = value; } }
        public string ProcPurchased_SubmittedDate { get { return mProcPurchased_SubmittedDate; } set { mProcPurchased_SubmittedDate = value; } }
        public string ProcFilm_SubmittedDate { get { return mProcFilm_SubmittedDate; } set { mProcFilm_SubmittedDate = value; } }
        public string ProcLabel_SubmittedDate { get { return mProcLabel_SubmittedDate; } set { mProcLabel_SubmittedDate = value; } }
        public string ProcMetal_SubmittedDate { get { return mProcMetal_SubmittedDate; } set { mProcMetal_SubmittedDate = value; } }
        public string ProcOther_SubmittedDate { get { return mProcOther_SubmittedDate; } set { mProcOther_SubmittedDate = value; } }
        public string ProcPaperboard_SubmittedDate { get { return mProcPaperboard_SubmittedDate; } set { mProcPaperboard_SubmittedDate = value; } }
        public string ProcRigidPlastic_SubmittedDate { get { return mProcRigidPlastic_SubmittedDate; } set { mProcRigidPlastic_SubmittedDate = value; } }
        public string ProcExternal_SubmittedDate { get { return mProcExternal_SubmittedDate; } set { mProcExternal_SubmittedDate = value; } }
        public string ProcCoMan_SubmittedDate { get { return mProcCoMan_SubmittedDate; } set { mProcCoMan_SubmittedDate = value; } }
        public string ProcSeasonal_SubmittedDate { get { return mProcSeasonal_SubmittedDate; } set { mProcSeasonal_SubmittedDate = value; } }
        public string ProcNovelty_SubmittedDate { get { return mProcNovelty_SubmittedDate; } set { mProcNovelty_SubmittedDate = value; } }
        public string ProcAncillary_SubmittedBy { get { return mProcAncillary_SubmittedBy; } set { mProcAncillary_SubmittedBy = value; } }
        public string ProcCorrugated_SubmittedBy { get { return mProcCorrugated_SubmittedBy; } set { mProcCorrugated_SubmittedBy = value; } }
        public string ProcPurchased_SubmittedBy { get { return mProcPurchased_SubmittedBy; } set { mProcPurchased_SubmittedBy = value; } }
        public string ProcFilm_SubmittedBy { get { return mProcFilm_SubmittedBy; } set { mProcFilm_SubmittedBy = value; } }
        public string ProcLabel_SubmittedBy { get { return mProcLabel_SubmittedBy; } set { mProcLabel_SubmittedBy = value; } }
        public string ProcMetal_SubmittedBy { get { return mProcMetal_SubmittedBy; } set { mProcMetal_SubmittedBy = value; } }
        public string ProcOther_SubmittedBy { get { return mProcOther_SubmittedBy; } set { mProcOther_SubmittedBy = value; } }
        public string ProcPaperboard_SubmittedBy { get { return mProcPaperboard_SubmittedBy; } set { mProcPaperboard_SubmittedBy = value; } }
        public string ProcRigidPlastic_SubmittedBy { get { return mProcRigidPlastic_SubmittedBy; } set { mProcRigidPlastic_SubmittedBy = value; } }
        public string ProcExternal_SubmittedBy { get { return mProcExternal_SubmittedBy; } set { mProcExternal_SubmittedBy = value; } }
        public string ProcCoMan_SubmittedBy { get { return mProcCoMan_SubmittedBy; } set { mProcCoMan_SubmittedBy = value; } }
        public string ProcSeasonal_SubmittedBy { get { return mProcSeasonal_SubmittedBy; } set { mProcSeasonal_SubmittedBy = value; } }
        public string ProcNovelty_SubmittedBy { get { return mProcNovelty_SubmittedBy; } set { mProcNovelty_SubmittedBy = value; } }
        public string ProcExternalAncillary_SubmittedDate { get { return mProcExternalAncillary_SubmittedDate; } set { mProcExternalAncillary_SubmittedDate = value; } }
        public string ProcExternalCorrugated_SubmittedDate { get { return mProcExternalCorrugated_SubmittedDate; } set { mProcExternalCorrugated_SubmittedDate = value; } }
        public string ProcExternalPurchased_SubmittedDate { get { return mProcExternalPurchased_SubmittedDate; } set { mProcExternalPurchased_SubmittedDate = value; } }
        public string ProcExternalFilm_SubmittedDate { get { return mProcExternalFilm_SubmittedDate; } set { mProcExternalFilm_SubmittedDate = value; } }
        public string ProcExternalLabel_SubmittedDate { get { return mProcExternalLabel_SubmittedDate; } set { mProcExternalLabel_SubmittedDate = value; } }
        public string ProcExternalMetal_SubmittedDate { get { return mProcExternalMetal_SubmittedDate; } set { mProcExternalMetal_SubmittedDate = value; } }
        public string ProcExternalOther_SubmittedDate { get { return mProcExternalOther_SubmittedDate; } set { mProcExternalOther_SubmittedDate = value; } }
        public string ProcExternalPaperboard_SubmittedDate { get { return mProcExternalPaperboard_SubmittedDate; } set { mProcExternalPaperboard_SubmittedDate = value; } }
        public string ProcExternalRigidPlastic_SubmittedDate { get { return mProcExternalRigidPlastic_SubmittedDate; } set { mProcExternalRigidPlastic_SubmittedDate = value; } }
        public string ProcExternalAncillary_SubmittedBy { get { return mProcExternalAncillary_SubmittedBy; } set { mProcExternalAncillary_SubmittedBy = value; } }
        public string ProcExternalCorrugated_SubmittedBy { get { return mProcExternalCorrugated_SubmittedBy; } set { mProcExternalCorrugated_SubmittedBy = value; } }
        public string ProcExternalPurchased_SubmittedBy { get { return mProcExternalPurchased_SubmittedBy; } set { mProcExternalPurchased_SubmittedBy = value; } }
        public string ProcExternalFilm_SubmittedBy { get { return mProcExternalFilm_SubmittedBy; } set { mProcExternalFilm_SubmittedBy = value; } }
        public string ProcExternalLabel_SubmittedBy { get { return mProcExternalLabel_SubmittedBy; } set { mProcExternalLabel_SubmittedBy = value; } }
        public string ProcExternalMetal_SubmittedBy { get { return mProcExternalMetal_SubmittedBy; } set { mProcExternalMetal_SubmittedBy = value; } }
        public string ProcExternalOther_SubmittedBy { get { return mProcExternalOther_SubmittedBy; } set { mProcExternalOther_SubmittedBy = value; } }
        public string ProcExternalPaperboard_SubmittedBy { get { return mProcExternalPaperboard_SubmittedBy; } set { mProcExternalPaperboard_SubmittedBy = value; } }
        public string ProcExternalRigidPlastic_SubmittedBy { get { return mProcExternalRigidPlastic_SubmittedBy; } set { mProcExternalRigidPlastic_SubmittedBy = value; } }

        public string BOMSetupPE2_ModifiedDate { get { return mBOMSetupPE2_ModifiedDate; } set { mBOMSetupPE2_ModifiedDate = value; } }
        public string BOMSetupPE2_SubmittedDate { get { return mBOMSetupPE2_SubmittedDate; } set { mBOMSetupPE2_SubmittedDate = value; } }
        public string BOMSetupPE2_StartDate { get { return mBOMSetupPE2_StartDate; } set { mBOMSetupPE2_StartDate = value; } }
        public string BOMSetupPE2_ModifiedBy { get { return mBOMSetupPE2_ModifiedBy; } set { mBOMSetupPE2_ModifiedBy = value; } }
        public string BOMSetupPE2_SubmittedBy { get { return mBOMSetupPE2_SubmittedBy; } set { mBOMSetupPE2_SubmittedBy = value; } }

        public string BOMSetupPE3_ModifiedDate { get { return mBOMSetupPE3_ModifiedDate; } set { mBOMSetupPE3_ModifiedDate = value; } }
        public string BOMSetupPE3_SubmittedDate { get { return mBOMSetupPE3_SubmittedDate; } set { mBOMSetupPE3_SubmittedDate = value; } }
        public string BOMSetupPE3_StartDate { get { return mBOMSetupPE3_StartDate; } set { mBOMSetupPE3_StartDate = value; } }
        public string BOMSetupPE3_ModifiedBy { get { return mBOMSetupPE3_ModifiedBy; } set { mBOMSetupPE3_ModifiedBy = value; } }
        public string BOMSetupPE3_SubmittedBy { get { return mBOMSetupPE3_SubmittedBy; } set { mBOMSetupPE3_SubmittedBy = value; } }

        public string MatrlWHSetUp_ModifiedDate { get { return mMatrlWHSetUp_ModifiedDate; } set { mMatrlWHSetUp_ModifiedDate = value; } }
        public string MatrlWHSetUp_SubmittedDate { get { return mMatrlWHSetUp_SubmittedDate; } set { mMatrlWHSetUp_SubmittedDate = value; } }
        public string MatrlWHSetUp_StartDate { get { return mMatrlWHSetUp_StartDate; } set { mMatrlWHSetUp_StartDate = value; } }
        public string MatrlWHSetUp_ModifiedBy { get { return mMatrlWHSetUp_ModifiedBy; } set { mMatrlWHSetUp_ModifiedBy = value; } }
        public string MatrlWHSetUp_SubmittedBy { get { return mMatrlWHSetUp_SubmittedBy; } set { mMatrlWHSetUp_SubmittedBy = value; } }

        public string SAPCompleteItem_ModifiedDate { get { return mSAPCompleteItem_ModifiedDate; } set { mSAPCompleteItem_ModifiedDate = value; } }
        public string SAPCompleteItem_SubmittedDate { get { return mSAPCompleteItem_SubmittedDate; } set { mSAPCompleteItem_SubmittedDate = value; } }
        public string SAPCompleteItem_StartDate { get { return mSAPCompleteItem_StartDate; } set { mSAPCompleteItem_StartDate = value; } }
        public string SAPCompleteItem_ModifiedBy { get { return mSAPCompleteItem_ModifiedBy; } set { mSAPCompleteItem_ModifiedBy = value; } }
        public string SAPCompleteItem_SubmittedBy { get { return mSAPCompleteItem_SubmittedBy; } set { mSAPCompleteItem_SubmittedBy = value; } }

        public string BEQRC_StartDate { get { return mBEQRC_StartDate; } set { mBEQRC_StartDate = value; } }
        public string BEQRC_ModifiedDate { get { return mBEQRC_ModifiedDate; } set { mBEQRC_ModifiedDate = value; } }
        public string BEQRC_ModifiedBy { get { return mBEQRC_ModifiedBy; } set { mBEQRC_ModifiedBy = value; } }
        public string BEQRC_SubmittedDate { get { return mBEQRC_SubmittedDate; } set { mBEQRC_SubmittedDate = value; } }
        public string BEQRC_SubmittedBy { get { return mBEQRC_SubmittedBy; } set { mBEQRC_SubmittedBy = value; } }
        #endregion

        #region Packaging Engineer Specs Properties
        #endregion

        #region OBM Second Review Variables

        public string OBMReview2_StartDate { get { return mOBMReview2_StartDate; } set { mOBMReview2_StartDate = value; } }
        public string OBMReview2_ModifiedDate { get { return mOBMReview2_ModifiedDate; } set { mOBMReview2_ModifiedDate = value; } }
        public string OBMReview2_ModifiedBy { get { return mOBMReview2_ModifiedBy; } set { mOBMReview2_ModifiedBy = value; } }
        public string OBMReview2_SubmittedDate { get { return mOBMReview2_SubmittedDate; } set { mOBMReview2_SubmittedDate = value; } }
        public string OBMReview2_SubmittedBy { get { return mOBMReview2_SubmittedBy; } set { mOBMReview2_SubmittedBy = value; } }

        #endregion

        #region Graphics Properties

        public string GRAPHICS_ModifiedDate { get { return mGRAPHICS_ModifiedDate; } set { mGRAPHICS_ModifiedDate = value; } }
        public string GRAPHICS_SubmittedDate { get { return mGRAPHICS_SubmittedDate; } set { mGRAPHICS_SubmittedDate = value; } }
        public string GRAPHICS_StartDate { get { return mGRAPHICS_StartDate; } set { mGRAPHICS_StartDate = value; } }
        public string GRAPHICS_ModifiedBy { get { return mGRAPHICS_ModifiedBy; } set { mGRAPHICS_ModifiedBy = value; } }
        public string GRAPHICS_SubmittedBy { get { return mGRAPHICS_SubmittedBy; } set { mGRAPHICS_SubmittedBy = value; } }

        #endregion
        #region External Mfg Properties

        public string ExternalMfg_ModifiedDate { get { return mExternalMfg_ModifiedDate; } set { mExternalMfg_ModifiedDate = value; } }
        public string ExternalMfg_SubmittedDate { get { return mExternalMfg_SubmittedDate; } set { mExternalMfg_SubmittedDate = value; } }
        public string ExternalMfg_StartDate { get { return mExternalMfg_StartDate; } set { mExternalMfg_StartDate = value; } }
        public string ExternalMfg_ModifiedBy { get { return mExternalMfg_ModifiedBy; } set { mExternalMfg_ModifiedBy = value; } }
        public string ExternalMfg_SubmittedBy { get { return mExternalMfg_SubmittedBy; } set { mExternalMfg_SubmittedBy = value; } }

        #endregion

        #region SAP Routing Setup Properties

        public string SAPRoutingSetup_ModifiedDate { get { return mSAPRoutingSetup_ModifiedDate; } set { mSAPRoutingSetup_ModifiedDate = value; } }
        public string SAPRoutingSetup_SubmittedDate { get { return mSAPRoutingSetup_SubmittedDate; } set { mSAPRoutingSetup_SubmittedDate = value; } }
        public string SAPRoutingSetup_StartDate { get { return mSAPRoutingSetup_StartDate; } set { mSAPRoutingSetup_StartDate = value; } }
        public string SAPRoutingSetup_ModifiedBy { get { return mSAPRoutingSetup_ModifiedBy; } set { mSAPRoutingSetup_ModifiedBy = value; } }
        public string SAPRoutingSetup_SubmittedBy { get { return mSAPRoutingSetup_SubmittedBy; } set { mSAPRoutingSetup_SubmittedBy = value; } }

        #endregion

        #region CostingQuote Properties

        public string CompCostSeasonal_StartDate { get { return mCompCostSeasonal_StartDate; } set { mCompCostSeasonal_StartDate = value; } }
        public string CompCostSeasonal_ModifiedDate { get { return mCompCostSeasonal_ModifiedDate; } set { mCompCostSeasonal_ModifiedDate = value; } }
        public string CompCostSeasonal_ModifiedBy { get { return mCompCostSeasonal_ModifiedBy; } set { mCompCostSeasonal_ModifiedBy = value; } }
        public string CompCostSeasonal_SubmittedDate { get { return mCompCostSeasonal_SubmittedDate; } set { mCompCostSeasonal_SubmittedDate = value; } }
        public string CompCostSeasonal_SubmittedBy { get { return mCompCostSeasonal_SubmittedBy; } set { mCompCostSeasonal_SubmittedBy = value; } }

        public string CompCostCorrPaper_StartDate { get { return mCompCostCorrPaper_StartDate; } set { mCompCostCorrPaper_StartDate = value; } }
        public string CompCostCorrPaper_ModifiedDate { get { return mCompCostCorrPaper_ModifiedDate; } set { mCompCostCorrPaper_ModifiedDate = value; } }
        public string CompCostCorrPaper_ModifiedBy { get { return mCompCostCorrPaper_ModifiedBy; } set { mCompCostCorrPaper_ModifiedBy = value; } }
        public string CompCostCorrPaper_SubmittedDate { get { return mCompCostCorrPaper_SubmittedDate; } set { mCompCostCorrPaper_SubmittedDate = value; } }
        public string CompCostCorrPaper_SubmittedBy { get { return mCompCostCorrPaper_SubmittedBy; } set { mCompCostCorrPaper_SubmittedBy = value; } }

        public string CompCostFLRP_StartDate { get { return mCompCostFLRP_StartDate; } set { mCompCostFLRP_StartDate = value; } }
        public string CompCostFLRP_ModifiedDate { get { return mCompCostFLRP_ModifiedDate; } set { mCompCostFLRP_ModifiedDate = value; } }
        public string CompCostFLRP_ModifiedBy { get { return mCompCostFLRP_ModifiedBy; } set { mCompCostFLRP_ModifiedBy = value; } }
        public string CompCostFLRP_SubmittedDate { get { return mCompCostFLRP_SubmittedDate; } set { mCompCostFLRP_SubmittedDate = value; } }
        public string CompCostFLRP_SubmittedBy { get { return mCompCostFLRP_SubmittedBy; } set { mCompCostFLRP_SubmittedBy = value; } }


        #endregion

        #region FGPackSpec Properties

        public string FGPackSpec_ModifiedDate { get { return mFGPackSpec_ModifiedDate; } set { mFGPackSpec_ModifiedDate = value; } }
        public string FGPackSpec_SubmittedDate { get { return mFGPackSpec_SubmittedDate; } set { mFGPackSpec_SubmittedDate = value; } }
        public string FGPackSpec_StartDate { get { return mFGPackSpec_StartDate; } set { mFGPackSpec_StartDate = value; } }
        public string FGPackSpec_ModifiedBy { get { return mFGPackSpec_ModifiedBy; } set { mFGPackSpec_ModifiedBy = value; } }
        public string FGPackSpec_SubmittedBy { get { return mFGPackSpec_SubmittedBy; } set { mFGPackSpec_SubmittedBy = value; } }

        #endregion

        #region SAPBOMSetup Properties

        public string SAPBOMSetup_ModifiedDate { get { return mSAPBOMSetup_ModifiedDate; } set { mSAPBOMSetup_ModifiedDate = value; } }
        public string SAPBOMSetup_SubmittedDate { get { return mSAPBOMSetup_SubmittedDate; } set { mSAPBOMSetup_SubmittedDate = value; } }
        public string SAPBOMSetup_StartDate { get { return mSAPBOMSetup_StartDate; } set { mSAPBOMSetup_StartDate = value; } }
        public string SAPBOMSetup_ModifiedBy { get { return mSAPBOMSetup_ModifiedBy; } set { mSAPBOMSetup_ModifiedBy = value; } }
        public string SAPBOMSetup_SubmittedBy { get { return mSAPBOMSetup_SubmittedBy; } set { mSAPBOMSetup_SubmittedBy = value; } }

        #endregion

        // Routing

        // Material Cost

        // Master Data Routing

        // Accouting

        // Set BOM-Component Active Date

        #region OBM Third Review Variables

        public string OBMReview3_StartDate { get { return mOBMReview3_StartDate; } set { mOBMReview3_StartDate = value; } }
        public string OBMReview3_ModifiedDate { get { return mOBMReview3_ModifiedDate; } set { mOBMReview3_ModifiedDate = value; } }
        public string OBMReview3_ModifiedBy { get { return mOBMReview3_ModifiedBy; } set { mOBMReview3_ModifiedBy = value; } }
        public string OBMReview3_SubmittedDate { get { return mOBMReview3_SubmittedDate; } set { mOBMReview3_SubmittedDate = value; } }
        public string OBMReview3_SubmittedBy { get { return mOBMReview3_SubmittedBy; } set { mOBMReview3_SubmittedBy = value; } }

        #endregion

        #region Final Costing Properties

        public string FCST_Comments { get { return mFCST_Comments; } set { mFCST_Comments = value; } }
        public string FCST_ModifiedBy { get { return mFCST_ModifiedBy; } set { mFCST_ModifiedBy = value; } }
        public string FCST_ModifiedDate { get { return mFCST_ModifiedDate; } set { mFCST_ModifiedDate = value; } }
        public string FCST_SubmittedBy { get { return mFCST_SubmittedBy; } set { mFCST_SubmittedBy = value; } }
        public string FCST_SubmittedDate { get { return mFCST_SubmittedDate; } set { mFCST_SubmittedDate = value; } }
        public string FCST_StartDate { get { return mFCST_StartDate; } set { mFCST_StartDate = value; } }

        #endregion

        #region Other Workflow State Changes Properties
        public string OnHold_ModifiedBy { get { return mOnHold_ModifiedBy; } set { mOnHold_ModifiedBy = value; } }
        public string OnHold_ModifiedDate { get { return mOnHold_ModifiedDate; } set { mOnHold_ModifiedDate = value; } }

        public string PreProduction_ModifiedBy { get { return mPreProduction_ModifiedBy; } set { mPreProduction_ModifiedBy = value; } }
        public string PreProduction_ModifiedDate { get { return mPreProduction_ModifiedDate; } set { mPreProduction_ModifiedDate = value; } }

        public string Completed_ModifiedBy { get { return mCompleted_ModifiedBy; } set { mCompleted_ModifiedBy = value; } }
        public string Completed_ModifiedDate { get { return mCompleted_ModifiedDate; } set { mCompleted_ModifiedDate = value; } }

        public string Cancelled_ModifiedBy { get { return mCancelled_ModifiedBy; } set { mCancelled_ModifiedBy = value; } }
        public string Cancelled_ModifiedDate { get { return mCancelled_ModifiedDate; } set { mCancelled_ModifiedDate = value; } }

        public string ProductionCompleted_ModifiedBy { get { return mProductionCompleted_ModifiedBy; } set { mProductionCompleted_ModifiedBy = value; } }

        public string ProductionCompleted_ModifiedDate { get { return mProductionCompleted_ModifiedDate; } set { mProductionCompleted_ModifiedDate = value; } }

        #endregion

        #endregion
    }
}
