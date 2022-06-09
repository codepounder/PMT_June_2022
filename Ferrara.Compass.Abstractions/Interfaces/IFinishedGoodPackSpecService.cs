using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IFinishedGoodPackSpecService
    {
        #region Approval Methods
        ApprovalItem GetFinishedGoodPackSpecApprovalItem(int itemId);
        void UpdateFinishedGoodPackSpecApprovalItem(ApprovalItem approvalItem, bool bSubmitted);
        void SetFinishedGoodPackSpecStartDate(int compassListItemId, DateTime startDate);
        #endregion
    }
}
