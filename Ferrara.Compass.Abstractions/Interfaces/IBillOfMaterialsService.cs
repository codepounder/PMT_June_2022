using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IBillOfMaterialsService
    {
        void UpdateBillOfMaterialsItem(BillofMaterialsItem item, string pageName);
        BillofMaterialsItem GetBillOfMaterialsItem(int itemId);
        void UpdatePackagingNumbers(string packagingNumbers, int iItemId);

        CompassPackMeasurementsItem GetPackMeasurementsItem(int itemId, int parentId);
        string getProjectNewExisting(int iItemId);
        int InsertPackMeasurementItem(int compassListItemId, string title);
        void UpsertPackMeasurementsItem(CompassPackMeasurementsItem pmItem, string projectNumber);
        void UpsertPackMeasurementsPackTrial(int compassListItemId, string packTrial, string projectNumber);
        ApprovalItem GetBillofMaterialsApprovalItem(int itemId, string BOMType);
        void UpdateBillofMaterialsApprovalItem(ApprovalItem approvalItem, string BOMType, bool bSubmitted);
        void SetBillofMaterialsStartDate(int compassListItemId, DateTime startDate, string BOMType);
        ItemProposalItem getIPFItem(int itemId);
    }
}
