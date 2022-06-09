using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IQAService
    {
        QAItem GetQAItem(int itemId);
        void UpdateMarketingClaimsItem(MarketingClaimsItem ipItem);
        void UpdateRegulatoryItem(QAItem qaItem);
        List<PackagingItem> GetCandySemiForProject(int stageListItemId);
        List<PackagingItem> GetPurchasedCandySemiForProject(int stageListItemId);
        List<PackagingItem> GetTransferSemiItemsForProject(int stageListItemId);
        List<PackagingItem> GetFinishedGoodItems(int ItemId);
        void UpdateFinishedGoodShelfLife(List<PackagingItem> FGItems, string projectNumber);
        int InsertPackagingItem(PackagingItem packagingItem, int compassListItemId, string projectNumber);
        void UpdatePackagingItem(PackagingItem packagingItem);
        #region Approval Methods
        void UpdateQAApprovalItem(ApprovalItem approvalItem, bool bSubmitted);
        void SetQAStartDate(int compassListItemId, DateTime startDate, string title);
        #endregion
    }
}
