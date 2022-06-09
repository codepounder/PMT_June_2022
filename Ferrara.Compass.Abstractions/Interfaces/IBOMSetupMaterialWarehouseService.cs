using Ferrara.Compass.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IBOMSetupMaterialWarehouseService
    {
        BOMSetupMaterialWarehouseItem GetBOMSetupMaterialWarehouseItem(int itemId);
        void UpdateBOMSetupMaterialWarehouseItem(BOMSetupMaterialWarehouseItem bomSetupMaterialWarehouseItem);
        #region Approval Methods
        void UpdateBOMSetupMaterialWarehouseApprovalItem(ApprovalItem approvalItem, bool bSubmitted);
        #endregion
    }
}
