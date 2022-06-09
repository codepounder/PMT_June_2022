using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IOBMSecondReviewService
    {
        OBMSecondReviewItem GetPMSecondReviewItem(int itemId);
        void UpdatePMSecondReviewItem(OBMSecondReviewItem reviewItem);

        #region Approval Methods
        ApprovalItem GetPMSecondReviewApprovalItem(int itemId);
        void UpdatePMSecondReviewApprovalItem(ApprovalItem approvalItem, bool bSubmitted);
        void SetPMSecondReviewStartDate(int compassListItemId, DateTime startDate);
        #endregion
    }
}
