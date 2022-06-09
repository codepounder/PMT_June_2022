using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IApprovalService
    {
        ApprovalListItem GetApprovalItem(int itemId);
        
        // Other Workflow States
        void UpdateOnHoldApprovalItem(int compassListItemId);
        void UpdatePreProductionApprovalItem(int compassListItemId);
        void UpdateCompletedApprovalItem(int compassListItemId);
        void UpdateCancelledApprovalItem(int compassListItemId);
    }
}
