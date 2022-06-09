using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Data;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IReportService
    {
        DataTable GetWFReport();
        DataTable GetWFReportCached(ref string lastCachedDateTime);

        DataTable GetReportByView(string list, string query, StringCollection columns);

        Dictionary<string, int> GetSeasonalProjectTotalsReport();
        DataTable GetEverydayProjectTotalsReport();

        DataTable GetGraphicsProgressReport(string projectNumber);
        DataTable GetGraphicsProgressReportBySAPNumber(string sapNumber);
        DateTime GetLatestGraphicsImportDate();
    }
}
