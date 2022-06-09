using Ferrara.Compass.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IBEQRCService
    {
        BEQRCItem GetBEQRCItem(int iItemId);
        List<PackagingItem> GetAllProjectItems(int ItemId);
        string GetPackagingComponentsWithQRCodeForEmail(int compassListItemId);
        List<FileAttribute> GetBEQRCodeEPSFileUploadedFiles(string projectNo);
        void UpdateBEQRCPackagingItems(List<PackagingItem> packagingItems, int compassId);
        void UpdateBEQRCItem(BEQRCItem BEQRCItem);
        void UpdateBEQRCApprovalItem(ApprovalItem approvalItem, bool bSubmitted);
        void UpdateBEQRCRequestEmailSent(int compassListItem);
        List<PackagingItem> InsertPackagingItems(List<PackagingItem> packagingItems, int compassListItemId, string ProjectNumber);
    }
}
