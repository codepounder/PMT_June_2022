using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IExcelExportSyncService
    {
        void CopyFile(int compassId, string projectNumber);
        byte[] WriteToFile(string projectNumber, List<Dictionary<string, string>> rows, Dictionary<string, string> rowPublish, Dictionary<string, string> rowLink);
        void saveFileToDocLibrary(int compassId, string projectNumber, byte[] fileContent);
    }
}
