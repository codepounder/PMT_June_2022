using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface ICommercializationService
    {
        List<KeyValuePair<string, string>> GetCommercializationItem(int itemId);
        List<KeyValuePair<string, string>> GetPackagingItem(int itemId, string PLMFlag, string ProjectType);
        List<KeyValuePair<string, string>> GetMasterDataItem(int itemId);
    }
}
