using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IVersionHistoryService
    {
        List<string> GetGraphicsRoutingVersionHistory(int itemId);

        string GetVersionDisplay(List<string> versionList);
    }
}
