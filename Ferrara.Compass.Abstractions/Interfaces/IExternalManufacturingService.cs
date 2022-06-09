using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IExternalManufacturingService
    {
        ExternalManufacturingItem GetExternalManufacturingItem(int itemId);
        void UpdateExternalManufacturingItem(ExternalManufacturingItem item);
        void updateProcPrinterSupplier(PackagingItem packagingItem);
        #region Approval Methods
        ApprovalItem GetExternalManufacturingApprovalItem(int itemId);
        void UpdateExternalManufacturingApprovalItem(ApprovalItem approvalItem, bool bSubmitted);
        void SetExternalManufacturingStartDate(int compassListItemId, DateTime startDate);
        #endregion
    }
}
