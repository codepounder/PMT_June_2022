using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IGraphicsService
    {
        CompassListItem GetGraphicsItem(int itemId);
        void UpdateGraphicsItem(CompassListItem compassListItem, int itemId);

        #region Approval Methods
        ApprovalItem GetGraphicsApprovalItem(int itemId);
        void UpdateGraphicsApprovalItem(ApprovalItem approvalItem, bool bSubmitted);
        void SetGraphicsStartDate(int compassListItemId, DateTime startDate);
        #endregion
    }
}
