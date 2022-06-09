using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Models;
using System.IO;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IPDFService
    {
        void CreateGraphicsRequestPDF(int itemId, int packagingId);
        void CreateGraphicsRequestPDF_New(int itemId, int packagingId);
        void CreateComponentCostingRequestPDF(int itemId, string packagingId, string StructureLabel);
        FileAttribute StageGateGenerateBriefPDF(string ProjectNumber, int StageGateListItemId, int GateNo, int BriefNo, bool IncludeFinanceBriefInGateBrief);
        string StageGateGenerateFinanceBriefPDF(string ProjectNo, int StageGateListItemId, int GateNo, int BriefNo);
    }
}
