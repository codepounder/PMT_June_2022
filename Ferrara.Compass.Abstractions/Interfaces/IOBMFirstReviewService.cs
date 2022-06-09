using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IOBMFirstReviewService
    {
        OBMFirstReviewItem GetPMFirstReviewItem(int itemId);
        void UpdatePMFirstReviewItem(OBMFirstReviewItem compassListItem);

        #region Approval Methods
        ApprovalItem GetPMFirstReviewApprovalItem(int itemId);
        void UpdatePMFirstReviewApprovalItem(ApprovalItem approvalItem, bool bSubmitted);
        void SetPMFirstReviewStartDate(int compassListItemId, DateTime startDate);
        ApprovalListItem GetCompletedApprovalInfo(int itemId);
        #endregion

    }
}
