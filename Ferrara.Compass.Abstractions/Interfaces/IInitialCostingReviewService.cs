using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IInitialCostingReviewService
    {
        InitialCostingReviewItem GetInitialCostingReviewItem(int itemId);
        void UpdateInitialCostingReviewItem(InitialCostingReviewItem costingItem);

        #region Approval Methods
        void UpdateInitialCostingReviewApprovalItem(ApprovalItem approvalItem, bool bSubmitted);
        void SetInitialCostingStartDate(int compassListItemId, DateTime startDate);
        #endregion
    }
}
