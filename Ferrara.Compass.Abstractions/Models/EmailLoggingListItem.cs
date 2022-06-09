using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferrara.Compass.Abstractions.Models
{
    public class EmailLoggingListItem
    {
        private int emailLoggingListItemId;
        private int compassListItemId;

        public int EmailLoggingListItemId { get { return emailLoggingListItemId; } set { emailLoggingListItemId = value; } }
        public int CompassListItemId { get { return compassListItemId; } set { compassListItemId = value; } }

        #region Variables
        #region IPF Variables
        #endregion

        #region SAP Item Request Form Variables
 
        private string mSIR_SAP_EmailTo;
        private DateTime mSIR_SAP_EmailDate;
        
        #endregion

        #region OPS Variables
   
        private string mOPS_MFG_EmailTo;
        private DateTime mOPS_MFG_EmailDate;

        private string mOPS_RT_EmailTo;
        private DateTime mOPS_RT_EmailDate;

        private string mOPS_DIST_EmailTo;
        private DateTime mOPS_DIST_EmailDate;

        private string mOPS_ICR_EmailTo;
        private DateTime mOPS_ICR_EmailDate;
        
        #endregion

        #region QA Variables
 
        private string mQA_QA_EmailTo;
        private DateTime mQA_QA_EmailDate;
        
        #endregion

        #region COMAN Variables
  
        private string mCOMAN_COMAN_EmailTo;
        private DateTime mCOMAN_COMAN_EmailDate;
        
        #endregion

        #region Packaging Engineering Variables
        #endregion

        #region Customer Marketing Variables
  
        private string mCMF_CM_EmailTo;
        private DateTime mCMF_CM_EmailDate;
        
        #endregion

        #region PM First Review Variables
        
        private string mOBMREV1_OBM_EmailTo;
        private DateTime mOBMREV1_OBM_EmailDate;
        
        #endregion

        #region TBD Variables
        private string mTBD_SAP_EmailTo;
        private DateTime mTBD_SAP_EmailDate;
        #endregion

        #region Initial Costing Variables
        private string mICF_CC_EmailTo;
        private DateTime mICF_CC_EmailDate;

        private string mICF_CST_EmailTo;
        private DateTime mICF_CST_EmailDate;

        private string mICF_SLT_EmailTo;
        private DateTime mICF_SLT_EmailDate;
        #endregion

        #region Item Request Form Variables
        #endregion

        #region MaterialNumber Variables
        private string mMN_PE_EmailTo;
        private DateTime mMN_PE_EmailDate;

        private string mMN_PUR_EmailTo;
        private DateTime mMN_PUR_EmailDate;

        private string mMN_PE2_EmailTo;
        private DateTime mMN_PE2_EmailDate;

        private string mMN_BM_EmailTo;
        private DateTime mMN_BM_EmailDate;

        private string mMN_SAP_EmailTo;
        private DateTime mMN_SAP_EmailDate;
        #endregion

        #region Graphics Variables
        private string mRGF_OBM_EmailTo;
        private DateTime mRGF_OBM_EmailDate;

        private string mRGF_GR_EmailTo;
        private DateTime mRGF_GR_EmailDate;
        #endregion

        #region SAP Item Setup Variables
        
        private string mSIS_SAP_EmailTo;
        private DateTime mSIS_SAP_EmailDate;
        
        #endregion
        
        #region Final Costing Variables
        #endregion              

        #region Obsolescence Variables
        #endregion              
         
        #region Plates Purchased Variables

        private string mGPP_GR_EmailTo;
        private DateTime mGPP_GR_EmailDate;

        #endregion

        #region PM Third Review Variables

        private string mOBMREV3_OBM_EmailTo;
        private DateTime mOBMREV3_OBM_EmailDate;

        #endregion

        #region SAP Semi Request Variables

        private string mSSR_SAP_EmailTo;
        private DateTime mSSR_SAP_EmailDate;

        #endregion


        #region Trand Spending Variables

        private string mTrade_Spending_EmailTo;
        private DateTime mTrade_Spending_EmailDate;

        #endregion

        #region Demand Planning Variables

        private string mDemand_Planning_EmailTo;
        private DateTime mDemand_Planning_EmailDate;

        #endregion

        //#region Sales Planning Variables

        //private string mSales_Planning_EmailTo;
        //private DateTime mSales_Planning_EmailDate;

        //#endregion

        #region TBDSAP Notification Variables

        private string mTBDSAP_Notification_EmailTo;
        private DateTime mTBDSAP_Notification_EmailDate;

        #endregion

        #endregion

        #region Properties
        #region IPF Properties
        #endregion

        #region SAP Item Request Form

        public string SIR_SAP_EmailTo { get { return mSIR_SAP_EmailTo; } set { mSIR_SAP_EmailTo = value; } }
        public DateTime SIR_SAP_EmailDate { get { return mSIR_SAP_EmailDate; } set { mSIR_SAP_EmailDate = value; } }

        #endregion

        #region OPS Properties

        public string OPS_MFG_EmailTo { get { return mOPS_MFG_EmailTo; } set { mOPS_MFG_EmailTo = value; } }
        public DateTime OPS_MFG_EmailDate { get { return mOPS_MFG_EmailDate; } set { mOPS_MFG_EmailDate = value; } }

        public string OPS_RT_EmailTo { get { return mOPS_RT_EmailTo; } set { mOPS_RT_EmailTo = value; } }
        public DateTime OPS_RT_EmailDate { get { return mOPS_RT_EmailDate; } set { mOPS_RT_EmailDate = value; } }

        public string OPS_DIST_EmailTo { get { return mOPS_DIST_EmailTo; } set { mOPS_DIST_EmailTo = value; } }
        public DateTime OPS_DIST_EmailDate { get { return mOPS_DIST_EmailDate; } set { mOPS_DIST_EmailDate = value; } }

        public string OPS_ICR_EmailTo { get { return mOPS_ICR_EmailTo; } set { mOPS_ICR_EmailTo = value; } }
        public DateTime OPS_ICR_EmailDate { get { return mOPS_ICR_EmailDate; } set { mOPS_ICR_EmailDate = value; } }

        #endregion

        #region COMAN Properties

        public string COMAN_COMAN_EmailTo { get { return mCOMAN_COMAN_EmailTo; } set { mCOMAN_COMAN_EmailTo = value; } }
        public DateTime COMAN_COMAN_EmailDate { get { return mCOMAN_COMAN_EmailDate; } set { mCOMAN_COMAN_EmailDate = value; } }

        #endregion

        #region QA Properties

        public string QA_QA_EmailTo { get { return mQA_QA_EmailTo; } set { mQA_QA_EmailTo = value; } }
        public DateTime QA_QA_EmailDate { get { return mQA_QA_EmailDate; } set { mQA_QA_EmailDate = value; } }

        #endregion

        #region Packaging Engineering Properties
        #endregion

        #region Customer Marketing Properties

        public string CMF_CM_EmailTo { get { return mCMF_CM_EmailTo; } set { mCMF_CM_EmailTo = value; } }
        public DateTime CMF_CM_EmailDate { get { return mCMF_CM_EmailDate; } set { mCMF_CM_EmailDate = value; } }

        #endregion

        #region TBD Properties

        public string TBD_SAP_EmailTo { get { return mTBD_SAP_EmailTo; } set { mTBD_SAP_EmailTo = value; } }
        public DateTime TBD_SAP_EmailDate { get { return mTBD_SAP_EmailDate; } set { mTBD_SAP_EmailDate = value; } }

        #endregion

        #region Initial Costing Properties

        public string ICF_CC_EmailTo { get { return mICF_CC_EmailTo; } set { mICF_CC_EmailTo = value; } }
        public DateTime ICF_CC_EmailDate { get { return mICF_CC_EmailDate; } set { mICF_CC_EmailDate = value; } }

        public string ICF_CST_EmailTo { get { return mICF_CST_EmailTo; } set { mICF_CST_EmailTo = value; } }
        public DateTime ICF_CST_EmailDate { get { return mICF_CST_EmailDate; } set { mICF_CST_EmailDate = value; } }

        public string ICF_SLT_EmailTo { get { return mICF_SLT_EmailTo; } set { mICF_SLT_EmailTo = value; } }
        public DateTime ICF_SLT_EmailDate { get { return mICF_SLT_EmailDate; } set { mICF_SLT_EmailDate = value; } }

        #endregion

        #region Item Request Form
        #endregion

        #region Material Number Properties

        public string MN_PE_EmailTo { get { return mMN_PE_EmailTo; } set { mMN_PE_EmailTo = value; } }
        public DateTime MN_PE_EmailDate { get { return mMN_PE_EmailDate; } set { mMN_PE_EmailDate = value; } }

        public string MN_PUR_EmailTo { get { return mMN_PUR_EmailTo; } set { mMN_PUR_EmailTo = value; } }
        public DateTime MN_PUR_EmailDate { get { return mMN_PUR_EmailDate; } set { mMN_PUR_EmailDate = value; } }

        public string MN_PE2_EmailTo { get { return mMN_PE2_EmailTo; } set { mMN_PE2_EmailTo = value; } }
        public DateTime MN_PE2_EmailDate { get { return mMN_PE2_EmailDate; } set { mMN_PE2_EmailDate = value; } }

        public string MN_SAP_EmailTo { get { return mMN_SAP_EmailTo; } set { mMN_SAP_EmailTo = value; } }
        public DateTime MN_SAP_EmailDate { get { return mMN_SAP_EmailDate; } set { mMN_SAP_EmailDate = value; } }

        #endregion

        #region Graphics Properties

        public string RGF_OBM_EmailTo { get { return mRGF_OBM_EmailTo; } set { mRGF_OBM_EmailTo = value; } }
        public DateTime RGF_OBM_EmailDate { get { return mRGF_OBM_EmailDate; } set { mRGF_OBM_EmailDate = value; } }

        public string RGF_GR_EmailTo { get { return mRGF_GR_EmailTo; } set { mRGF_GR_EmailTo = value; } }
        public DateTime RGF_GR_EmailDate { get { return mRGF_GR_EmailDate; } set { mRGF_GR_EmailDate = value; } }

        #endregion

        #region SAP Item Setup Properties

        public string SIS_SAP_EmailTo { get { return mSIS_SAP_EmailTo; } set { mSIS_SAP_EmailTo = value; } }
        public DateTime SIS_SAP_EmailDate { get { return mSIS_SAP_EmailDate; } set { mSIS_SAP_EmailDate = value; } }

        #endregion

        #region Final Costing Properties

        #endregion

        #region Obsolescence Properties

        #endregion

        #region PM First Review Properties

        public string OBMREV1_OBM_EmailTo { get { return mOBMREV1_OBM_EmailTo; } set { mOBMREV1_OBM_EmailTo = value; } }
        public DateTime OBMREV1_OBM_EmailDate { get { return mOBMREV1_OBM_EmailDate; } set { mOBMREV1_OBM_EmailDate = value; } }

        #endregion

        #region Plates Purchased Properties

        public string GPP_GR_EmailTo { get { return mGPP_GR_EmailTo; } set { mGPP_GR_EmailTo = value; } }
        public DateTime GPP_GR_EmailDate { get { return mGPP_GR_EmailDate; } set { mGPP_GR_EmailDate = value; } }

        #endregion

        #region PM Third Review Properties

        public string OBMREV3_OBM_EmailTo { get { return mOBMREV3_OBM_EmailTo; } set { mOBMREV3_OBM_EmailTo = value; } }
        public DateTime OBMREV3_OBM_EmailDate { get { return mOBMREV3_OBM_EmailDate; } set { mOBMREV3_OBM_EmailDate = value; } }

        #endregion

        #region SAP Semi Request Properties

        public string SSR_SAP_EmailTo { get { return mSSR_SAP_EmailTo; } set { mSSR_SAP_EmailTo = value; } }
        public DateTime SSR_SAP_EmailDate { get { return mSSR_SAP_EmailDate; } set { mSSR_SAP_EmailDate = value; } }

        #endregion

        #region Trade Spending Properties

        public string Trade_Spending_EmailTo { get { return mTrade_Spending_EmailTo; } set { mTrade_Spending_EmailTo = value; } }
        public DateTime Trade_Spending_EmailDate { get { return mTrade_Spending_EmailDate; } set { mTrade_Spending_EmailDate = value; } }

        #endregion

        #region Demand Planning Properties

        public string Demand_Planning_EmailTo { get { return mDemand_Planning_EmailTo; } set { mDemand_Planning_EmailTo = value; } }
        public DateTime Demand_Planning_EmailDate { get { return mDemand_Planning_EmailDate; } set { mDemand_Planning_EmailDate = value; } }

        #endregion

        //#region Sales Planning Properties

        //public string Sales_Planning_EmailTo { get { return mSales_Planning_EmailTo; } set { mSales_Planning_EmailTo = value; } }
        //public DateTime Sales_Planning_EmailDate { get { return mSales_Planning_EmailDate; } set { mSales_Planning_EmailDate = value; } }

        //#endregion

        #region TBDSAP Notification Properties

        public string TBDSAP_Notification_EmailTo { get { return mTBDSAP_Notification_EmailTo; } set { mTBDSAP_Notification_EmailTo = value; } }
        public DateTime TBDSAP_Notification_EmailDate { get { return mTBDSAP_Notification_EmailDate; } set { mTBDSAP_Notification_EmailDate = value; } }

        #endregion

        #endregion
    }
}
