using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IDistributionService
    {
        DistributionItem GetDistributionItem(int itemId);
        void UpdateDistributionItem(DistributionItem item);

        #region Approval Methods
        ApprovalItem GetDistributionApprovalItem(int itemId);
        void UpdateDistributionApprovalItem(ApprovalItem approvalItem, bool bSubmitted);
        void SetDistributionStartDate(int compassListItemId, DateTime startDate);
        #endregion
    }
}
