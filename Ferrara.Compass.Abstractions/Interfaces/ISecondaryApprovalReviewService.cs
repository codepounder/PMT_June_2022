using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface ISecondaryApprovalReviewService
    {
        SecondaryApprovalReviewItem GetSecondaryApprovalReviewItem(int itemId);
        void UpdateSecondaryApprovalReviewItem(SecondaryApprovalReviewItem capacityItem);

        #region Approval Methods
        void UpdateSecondaryApprovalReviewApprovalItem(ApprovalItem approvalItem, bool bSubmitted);
        void SetSecondaryApprovalReviewStartDate(int compassListItemId, DateTime startDate);
        #endregion
    }
}
