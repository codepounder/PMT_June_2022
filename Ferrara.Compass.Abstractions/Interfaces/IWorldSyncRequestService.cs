using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IWorldSyncRequestService
    {
        int InsertRequest(WorldSyncRequestItem request);
        void UpdateRequest(WorldSyncRequestItem request);
        List<WorldSyncRequestItem> GetRequestItems(string SAPnumber);
        List<WorldSyncRequestItem> GetRequestItems();
        List<WorldSyncRequestItem> GetRequestItems(string SAPnumber, string requestType);
        List<WorldSyncRequestItem> GetRequestItems(string SAPnumber, string requestType, string requestStatus);
        WorldSyncRequestItem GetRequestItemById(int requestId);
        List<FileAttribute> GetUploadedFilesByRequestId(string SAPnumber, int requestId);
    }
}
