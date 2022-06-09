using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IOPSService
    {
        OPSItem GetOPSItem(int itemId);
        void UpdateOPSItem(OPSItem compassListItem, int itemId);
        bool HasMakePackChanges(CompassListItem newItem, CompassListItem existingItem);
        bool HasDistributionChanges(CompassListItem newItem, CompassListItem existingItem);
        bool HasInternationalComplianceChanges(ApprovalListItem newItem, ApprovalListItem existingItem);
        List<PackagingItem> GetTransferSemiItemsForProject(int stageListItemId);

        #region Approval Methods
        ApprovalItem GetOPSApprovalItem(int itemId);
        void UpdateOPSApprovalItem(ApprovalItem approvalItem, bool bSubmitted);
        void SetOPSStartDate(int compassListItemId, DateTime startDate);
        #endregion
    }
}
