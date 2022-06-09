using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface ITradePromoGroupService
    {
        TradePromoGroupItem GetTradePromoGroupItem(int itemId);
        void UpdateTradePromoGroupItem(TradePromoGroupItem compassListItem);
        void UpdateEstimatedBracketPricingItem(TradePromoGroupItem compassListItem);
        void UpdateEstimatedPricingItem(TradePromoGroupItem compassListItem);

        #region Approval Methods
        ApprovalItem GetTradePromoGroupApprovalItem(int itemId);
        void UpdateTradePromoGroupApprovalItem(ApprovalItem approvalItem, bool bSubmitted);
        void UpdateEstimatedBracketPricingApprovalItem(ApprovalItem approvalItem, bool bSubmitted);
        void UpdateEstimatedPricingApprovalItem(ApprovalItem approvalItem, bool bSubmitted);
        void SetTradePromoGroupStartDate(int compassListItemId, DateTime startDate);
        #endregion
    }
}
