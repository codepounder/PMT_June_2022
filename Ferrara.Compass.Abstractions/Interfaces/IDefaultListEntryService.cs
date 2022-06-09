using System.Collections.Generic;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IDefaultListEntryService
    {
        IEnumerable<string> GetDefaultDataFor(XmlFileName fileName);
        string GetXML(XmlFileName fileName);
        IEnumerable<GlobalLookupField> GetGlobalLookupFieldsData(XmlFileName fileName);
    }
}
