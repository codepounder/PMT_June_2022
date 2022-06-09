

namespace Ferrara.Compass.ProjectTimelineCalculator.Constants
{
    public class SAPApprovalListFields
    {
        public const string Title = "Title" ;
        public const string CompassListItemId = "CompassListItemId";
  
        // Final Setup Phase Notifications

        #region SAP Routing Setup Notification
        public const string SAPRoutingSetup_StartDate = "SAPROUTINGSETUP_StartDate";
        public const string SAPRoutingSetup_ModifiedDate = "SAPROUTINGSETUP_ModifiedDate";
        public const string SAPRoutingSetup_SubmittedDate = "SAPROUTINGSETUP_SubmittedDate";

        public const string SAPRoutingSetup_StartDate_DisplayName = "SAPRoutingSetup StartDate";
        public const string SAPRoutingSetup_ModifiedDate_DisplayName = "SAPRoutingSetup ModifiedDate";
        public const string SAPRoutingSetup_SubmittedDate_DisplayName = "SAPRoutingSetup SubmittedDate";
        #endregion

        #region SAP Costing Details Notification
        public const string SAPCostingDetails_StartDate = "SAPCOSTINGDETAILS_StartDate";
        public const string SAPCostingDetails_ModifiedDate = "SAPCOSTINGDETAILS_ModifiedDate";
        public const string SAPCostingDetails_SubmittedDate = "SAPCOSTINGDETAILS_SubmittedDate";

        public const string SAPCostingDetails_StartDate_DisplayName = "SAPCostingDetails StartDate";
        public const string SAPCostingDetails_ModifiedDate_DisplayName = "SAPCostingDetails ModifiedDate";
        public const string SAPCostingDetails_SubmittedDate_DisplayName = "SAPCostingDetails SubmittedDate";
        #endregion

        #region SAP Warehouse Info Notification
        public const string SAPWarehouseInfo_StartDate = "SAPWAREHOUSEINFO_StartDate";
        public const string SAPWarehouseInfo_ModifiedDate = "SAPWAREHOUSEINFO_ModifiedDate";
        public const string SAPWarehouseInfo_SubmittedDate = "SAPWAREHOUSEINFO_SubmittedDate";

        public const string SAPWarehouseInfo_StartDate_DisplayName = "SAPWarehouseInfo StartDate";
        public const string SAPWarehouseInfo_ModifiedDate_DisplayName = "SAPWarehouseInfo ModifiedDate";
        public const string SAPWarehouseInfo_SubmittedDate_DisplayName = "SAPWarehouseInfo SubmittedDate";
        #endregion

        #region Standard Cost Entry Notification
        public const string StandardCostEntry_StartDate = "STANDARDCOSTENTRY_StartDate";
        public const string StandardCostEntry_ModifiedDate = "STANDARDCOSTENTRY_ModifiedDate";
        public const string StandardCostEntry_SubmittedDate = "STANDARDCOSTENTRY_SubmittedDate";

        public const string StandardCostEntry_StartDate_DisplayName = "StandardCostEntry StartDate";
        public const string StandardCostEntry_ModifiedDate_DisplayName = "StandardCostEntry ModifiedDate";
        public const string StandardCostEntry_SubmittedDate_DisplayName = "StandardCostEntry SubmittedDate";
        #endregion

        /*Waiting to see if these are included 
        #region Cost Finished Good Notification
        public const string CostFinishedGood_StartDate = "CostFinishedGood_StartDate";
        public const string CostFinishedGood_ModifiedDate = "CostFinishedGood_ModifiedDate";
        public const string CostFinishedGood_ModifiedBy = "CostFinishedGood_ModifiedBy";
        public const string CostFinishedGood_SubmittedDate = "CostFinishedGood_SubmittedDate";
        public const string CostFinishedGood_SubmittedBy = "CostFinishedGood_SubmittedBy";

        public const string CostFinishedGood_StartDate_DisplayName = "CostFinishedGood StartDate";
        public const string CostFinishedGood_ModifiedDate_DisplayName = "CostFinishedGood ModifiedDate";
        public const string CostFinishedGood_ModifiedBy_DisplayName = "CostFinishedGood ModifiedBy";
        public const string CostFinishedGood_SubmittedDate_DisplayName = "CostFinishedGood SubmittedDate";
        public const string CostFinishedGood_SubmittedBy_DisplayName = "CostFinishedGood SubmittedBy";
        #endregion

        #region Final Costing Review Notification
        public const string FinalCostingReview_StartDate = "FinalCostingReview_StartDate";
        public const string FinalCostingReview_ModifiedDate = "FinalCostingReview_ModifiedDate";
        public const string FinalCostingReview_ModifiedBy = "FinalCostingReview_ModifiedBy";
        public const string FinalCostingReview_SubmittedDate = "FinalCostingReview_SubmittedDate";
        public const string FinalCostingReview_SubmittedBy = "FinalCostingReview_SubmittedBy";

        public const string FinalCostingReview_StartDate_DisplayName = "FinalCostingReview StartDate";
        public const string FinalCostingReview_ModifiedDate_DisplayName = "FinalCostingReview ModifiedDate";
        public const string FinalCostingReview_ModifiedBy_DisplayName = "FinalCostingReview ModifiedBy";
        public const string FinalCostingReview_SubmittedDate_DisplayName = "FinalCostingReview SubmittedDate";
        public const string FinalCostingReview_SubmittedBy_DisplayName = "FinalCostingReview SubmittedBy";
        #endregion

        #region Purchased PO Notification
        public const string PurchasedPO_StartDate = "PurchasedPO_StartDate";
        public const string PurchasedPO_ModifiedDate = "PurchasedPO_ModifiedDate";
        public const string PurchasedPO_ModifiedBy = "PurchasedPO_ModifiedBy";
        public const string PurchasedPO_SubmittedDate = "PurchasedPO_SubmittedDate";
        public const string PurchasedPO_SubmittedBy = "PurchasedPO_SubmittedBy";

        public const string PurchasedPO_StartDate_DisplayName = "PurchasedPO StartDate";
        public const string PurchasedPO_ModifiedDate_DisplayName = "PurchasedPO ModifiedDate";
        public const string PurchasedPO_ModifiedBy_DisplayName = "PurchasedPO ModifiedBy";
        public const string PurchasedPO_SubmittedDate_DisplayName = "PurchasedPO SubmittedDate";
        public const string PurchasedPO_SubmittedBy_DisplayName = "PurchasedPO SubmittedBy";
        #endregion

        #region Remove SAP Blocks
        public const string RemoveSAPBlocks_StartDate = "RemoveSAPBlocks_StartDate";
        public const string RemoveSAPBlocks_ModifiedDate = "RemoveSAPBlocks_ModifiedDate";
        public const string RemoveSAPBlocks_ModifiedBy = "RemoveSAPBlocks_ModifiedBy";
        public const string RemoveSAPBlocks_SubmittedDate = "RemoveSAPBlocks_SubmittedDate";
        public const string RemoveSAPBlocks_SubmittedBy = "RemoveSAPBlocks_SubmittedBy";

        public const string RemoveSAPBlocks_StartDate_DisplayName = "RemoveSAPBlocks StartDate";
        public const string RemoveSAPBlocks_ModifiedDate_DisplayName = "RemoveSAPBlocks ModifiedDate";
        public const string RemoveSAPBlocks_ModifiedBy_DisplayName = "RemoveSAPBlocks ModifiedBy";
        public const string RemoveSAPBlocks_SubmittedDate_DisplayName = "RemoveSAPBlocks SubmittedDate";
        public const string RemoveSAPBlocks_SubmittedBy_DisplayName = "RemoveSAPBlocks SubmittedBy";
        #endregion

        #region Customer POs can be Entered Notification
        public const string CustomerPO_StartDate = "CustomerPO_StartDate";
        public const string CustomerPO_ModifiedDate = "CustomerPO_ModifiedDate";
        public const string CustomerPO_ModifiedBy = "CustomerPO_ModifiedBy";
        public const string CustomerPO_SubmittedDate = "CustomerPO_SubmittedDate";
        public const string CustomerPO_SubmittedBy = "CustomerPO_SubmittedBy";

        public const string CustomerPO_StartDate_DisplayName = "CustomerPO StartDate";
        public const string CustomerPO_ModifiedDate_DisplayName = "CustomerPO ModifiedDate";
        public const string CustomerPO_ModifiedBy_DisplayName = "CustomerPO ModifiedBy";
        public const string CustomerPO_SubmittedDate_DisplayName = "CustomerPO SubmittedDate";
        public const string CustomerPO_SubmittedBy_DisplayName = "CustomerPO SubmittedBy";
        #endregion

        #region Materials Received Check
        public const string MaterialsReceivedChk_StartDate = "MaterialsRcvdChk_StartDate";
        public const string MaterialsReceivedChk_SubmittedDate = "MaterialsRcvdChk_SubmittedDate";
        public const string MaterialsReceivedChk_SubmittedBy = "MaterialsRcvdChk_SubmittedBy";

        public const string MaterialsReceivedChk_StartDate_DisplayName = "Materials Rcvd Chk StartDate";
        public const string MaterialsReceivedChk_SubmittedDate_DisplayName = "Materials Rcvd Chk SubmittedDate";
        public const string MaterialsReceivedChk_SubmittedBy_DisplayName = "Materials Rcvd Chk SubmittedBy";
        #endregion

        #region First Production Check
        public const string FirstProductionChk_StartDate = "FirstProductionChk_StartDate";
        public const string FirstProductionChk_SubmittedDate = "FirstProductionChk_SubmittedDate";
        public const string FirstProductionChk_SubmittedBy = "FirstProductionChk_SubmittedBy";

        public const string FirstProductionChk_StartDate_DisplayName = "First Production Chk StartDate";
        public const string FirstProductionChk_SubmittedDate_DisplayName = "First Production Chk SubmittedDate";
        public const string FirstProductionChk_SubmittedBy_DisplayName = "First Production Chk SubmittedBy";
        #endregion

        #region Distribution Center Check
        public const string DistributionCenterChk_StartDate = "DistributionChk_StartDate";
        public const string DistributionCenterChk_SubmittedDate = "DistributionChk_SubmittedDate";
        public const string DistributionCenterChk_SubmittedBy = "DistributionChk_SubmittedBy";

        public const string DistributionCenterChk_StartDate_DisplayName = "Distribution Chk StartDate";
        public const string DistributionCenterChk_SubmittedDate_DisplayName = "Distribution Chk SubmittedDate";
        public const string DistributionCenterChk_SubmittedBy_DisplayName = "Distribution Chk SubmittedBy";
        #endregion
        */
        
    }
}
