using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Models;


namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IComponentCostingQuoteService
    {
        ComponentCostingQuoteItem GetComponentCostingQuoteItem(int packagingId, int itemId);
        void UpdatePackagingItem(ComponentCostingQuoteItem packagingItem, string webUrl, string componentType);

        #region Approval Methods
        ApprovalItem GetComponentCostingApprovalItem(int itemId);
        void UpdateComponentCostingApprovalItem(ApprovalItem approvalItem, bool bSubmitted, string productHierarchy);
        void SetComponentCostingStartDate(int compassListItemId, DateTime startDate);
        #endregion
        List<KeyValuePair<string, bool>> allCostingQuotesSubmitted(int itemId, string compType);
        List<AlternateUOMItem> GetAlternateUOMConversions(string PackagingItemId);
        void DeleteAlternateUOMItem(int deletedId);
        void UpdateAlternateUOMs(AlternateUOMItem alternateItem, string webUrl);
    }
}
