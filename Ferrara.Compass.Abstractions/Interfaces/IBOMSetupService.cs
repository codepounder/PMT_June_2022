using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IBOMSetupService
    {
        string GetPLMFlag(int itemId);
        BOMSetupProjectSummaryItem GetProjectSummaryDetails(int compassListItemId);
        bool DeleteBOMSetupItem(int packagingItemId);
        bool DeleteBOMSetupItems(List<int> packagingItemIds);
        int InsertBOMSetupItem(BOMSetupItem packagingItem);
        List<BOMSetupItem> GetAllBOMSetupItemsForProject(int compassListItemId);
        string GetReviewPrinterSupplierProcDets(int itemId);
        BOMSetupItem GetBOMSetupItemByComponentId(int packagingId);
        void UpdateBOMSetupItem(BOMSetupItem packagingItem);
        List<FileAttribute> GetUploadedFiles(string projectNo, int packagingItemId, string docType);
        List<FileAttribute> GetUploadedFiles(string projectNo, int packagingItemId, string docType, string webUrl);
        List<FileAttribute> GetUploadedFiles(string projectNo, string docType);
        void UpsertPackMeasurementsItem(BOMSetupItem pmItem, string projectNumber);
        void UpsertPackMeasurementsItem(List<BOMSetupItem> pmItems, string projectNumber);
        void UpdateTransferSemiMakePackLocations(int id, string TransferSEMIMakePackLocations);
        void UpdateTransferSemiMakePackLocations(List<BOMSetupItem> TSMakePackLocationsitems);
        BOMSetupItem GetPackMeasurementsItem(int itemId, int parentId);
        List<BOMSetupItem> GetPackMeasurementsItems(int itemId);
        void UpdateBillOfMaterialsItem(BillofMaterialsItem materialItem, string pageName);
        void UpdateBillofMaterialsApprovalItem(ApprovalItem approvalItem, string BOMType, bool bSubmitted);
        SAPBOMSetupItem GetSAPBOMSetupItem(int itemId);
        void UpdateSAPBOMSetupItem(SAPBOMSetupItem sapBOMSetupItem);
    }
}
