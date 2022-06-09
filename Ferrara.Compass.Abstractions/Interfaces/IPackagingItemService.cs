using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IPackagingItemService
    {
        bool CheckIfPackagingItemsExistForProject(int stageListItemId);
        PackagingItem GetPackagingItemByPackagingId(int packagingId);
        string GetReviewPrinterSupplierProcDets(int itemId);
        PackagingItem GetGraphicsPackagingItemByPackagingId(int packagingId);
        List<PackagingItem> GetAllPackagingItemsForProject(int stageListItemId);
        List<PackagingItem> GetFinishedGoodItemsForProject(int stageListItemId);
        List<PackagingItem> GetSemiChildTSBOMItems(int stageListItemId, int parentID, string componentType);
        List<PackagingItem> GetSemiChildTSBOMItemsForBOMSetup(int stageListItemId, int parentID, string componentType);
        void CopyPackagingItems(int sourceCompassId, string sourceProjectNumber, ItemProposalItem targetProposalItem);
        void CopyIPFPackagingItems(int sourceCompassId, string sourceProjectNumber, ItemProposalItem targetProposalItem);
        List<PackagingItem> GetPackagingParents(int compassId);
        List<PackagingItem> GetPackagingChildren(int parentID);
        List<PackagingItem> GetTransferSemiItemsForProject(int stageListItemId);
        List<PackagingItem> GetTransferPurchasedSemiItemsForProject(int stageListItemId, string semiType);
        List<KeyValuePair<int, string>> GetTransferSemiIDsForProject(int stageListItemId);
        List<KeyValuePair<int, string>> GetPurchasedSemiIDsForProject(int stageListItemId);
        List<FileAttribute> GetRenderingUploadedFiles(string projectNo);
        List<FileAttribute> GetApprovedGraphicsAssetUploadedFiles(string projectNo);
        List<PackagingItem> GetCandySemiItemsForProject(int stageListItemId);
        List<PackagingItem> GetCandyAndPurchasedSemisForProject(int CompassListItemId);
        List<PackagingItem> GetNewTransferSemiItemsForProject(int stageListItemId);
        List<PackagingItem> GetNewPURCANDYSemiItemsForProject(int stageListItemId);
        List<PackagingItem> GetGraphicsPackagingItemsForProject(int stageListItemId);

        // Component Costing Packaging Items For Project
        List<PackagingItem> GetAllComponentCostingPackagingItemsForProject(int stageListItemId);
        List<PackagingItem> GetFilmLabelRigidPlasticComponentCostingPackagingItemsForProject(int stageListItemId);
        List<PackagingItem> GetCorrugatedPaperboardComponentCostingPackagingItemsForProject(int stageListItemId);
        // 
        List<PackagingItem> GetGraphicsProgressPackagingItemsForProject(int stageListItemId);
        int CheckForExistingPackagingItem(int stageListItemId, string bulkSemiNumber, string materialDescription);

        int InsertPackagingItem(PackagingItem packagingItem, int stageListItemId);
        List<PackagingItem> InsertPackagingItems(List<PackagingItem> packagingItems, int compassListItemId, string ProjectNumber);
        void UpdatePackagingItem(PackagingItem packagingItem);
        void UpdateOPSPackagingItem(PackagingItem packagingItem);
        void UpdateIPFPackagingItem(PackagingItem packagingItem);
        void UpdateIPFPackagingItems(List<PackagingItem> packagingItems, int compassId);
        void UpdatePackagingGraphicsBrief(PackagingItem packagingItem);
        void UpdateGraphicsDevelopmentPackagingItem(PackagingItem packagingItem);
        void UpdateGraphicsPackagingItem(PackagingItem packagingItem);
        bool DeletePackagingItem(int packagingItemId);
        void UpdateGraphicsProgressReportItem(int compassId, string routingDate, string routingReleasedDate, string PDFApprovedDate, string platesShippedDate, string notes);
        void UpdateGraphicsProgressReportItem(string form, int materialNumber, string routingDate, string routingReleasedDate, string PDFApprovedDate, string platesShippedDate);
        void UpdatePackagingItemPlatesShipped(PackagingItem packagingItem);
        void UpdatePackagingItemMaterialDescription(PackagingItem packagingItem);
        List<FileAttribute> GetUploadedFiles(string projectNo, int packagingItemId, string docType, string webUrl);
        List<FileAttribute> GetUploadedFiles(string projectNo, int packagingItemId, string docType);
        List<PackagingItem> GetSemiBOMItems(int stageListItemId, int parentID);
        void UpdateTransferSemiMakePackLocations(int id, string TransferSEMIMakePackLocations);
        string GetTransferSemiMakePackLocations(int id);
        void updateCompletedItems(string ComponentIds, string pageName);
        List<CompassPackMeasurementsItem> GetPackMeasurements(int itemId);
        string getParentComponentType(int packagingItemId);
        bool BOMAttachmentsExist(string projectNo, int packagingItemId);
        List<PackagingItem> GetClassMissMatchedBOMList(int compassListItemId);
        void UpdatePackagingItemParentID(PackagingItem packagingItem);
        List<int> filterParents(List<int> IDs, int compassItemID, int movindId);
    }
}
