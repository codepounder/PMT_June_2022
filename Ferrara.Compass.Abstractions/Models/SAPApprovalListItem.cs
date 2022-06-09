using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferrara.Compass.Abstractions.Models
{
    public class SAPApprovalListItem
    {
        public SAPApprovalListItem()
        {
        }

        private int approvalListItemId;
        private int compassListItemId;

        public int ApprovalListItemId { get { return approvalListItemId; } set { approvalListItemId = value; } }
        public int CompassListItemId { get { return compassListItemId; } set { compassListItemId = value; } }

        #region Variables
        private string mSAPRoutingSetup_StartDate;
        private string mSAPRoutingSetup_ModifiedDate;
        private string mSAPRoutingSetup_SubmittedDate;
        private string mSAPCostingDetails_StartDate;
        private string mSAPCostingDetails_ModifiedDate;
        private string mSAPCostingDetails_SubmittedDate;
        private string mSAPWarehouseInfo_StartDate;
        private string mSAPWarehouseInfo_ModifiedDate;
        private string mSAPWarehouseInfo_SubmittedDate;
        private string mStandardCostEntry_StartDate;
        private string mStandardCostEntry_ModifiedDate;
        private string mStandardCostEntry_SubmittedDate;
        #endregion

        #region Properties

        public string SAPRoutingSetup_StartDate { get { return mSAPRoutingSetup_StartDate; } set { mSAPRoutingSetup_StartDate = value; } }
        public string SAPRoutingSetup_ModifiedDate { get { return mSAPRoutingSetup_ModifiedDate; } set { mSAPRoutingSetup_ModifiedDate = value; } }
        public string SAPRoutingSetup_SubmittedDate { get { return mSAPRoutingSetup_SubmittedDate; } set { mSAPRoutingSetup_SubmittedDate = value; } }
        public string SAPCostingDetails_StartDate { get { return mSAPCostingDetails_StartDate; } set { mSAPCostingDetails_StartDate = value; } }
        public string SAPCostingDetails_ModifiedDate { get { return mSAPCostingDetails_ModifiedDate; } set { mSAPCostingDetails_ModifiedDate = value; } }
        public string SAPCostingDetails_SubmittedDate { get { return mSAPCostingDetails_SubmittedDate; } set { mSAPCostingDetails_SubmittedDate = value; } }
        public string SAPWarehouseInfo_StartDate { get { return mSAPWarehouseInfo_StartDate; } set { mSAPWarehouseInfo_StartDate = value; } }
        public string SAPWarehouseInfo_ModifiedDate { get { return mSAPWarehouseInfo_ModifiedDate; } set { mSAPWarehouseInfo_ModifiedDate = value; } }
        public string SAPWarehouseInfo_SubmittedDate { get { return mSAPWarehouseInfo_SubmittedDate; } set { mSAPWarehouseInfo_SubmittedDate = value; } }
        public string StandardCostEntry_StartDate { get { return mStandardCostEntry_StartDate; } set { mStandardCostEntry_StartDate = value; } }
        public string StandardCostEntry_ModifiedDate { get { return mStandardCostEntry_ModifiedDate; } set { mStandardCostEntry_ModifiedDate = value; } }
        public string StandardCostEntry_SubmittedDate { get { return mStandardCostEntry_SubmittedDate; } set { mStandardCostEntry_SubmittedDate = value; } }


        #endregion
    }
}
