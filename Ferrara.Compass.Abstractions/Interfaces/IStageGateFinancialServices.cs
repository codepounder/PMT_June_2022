using Ferrara.Compass.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IStageGateFinancialServices
    {
        #region Financial Summary
        StageGateConsolidatedFinancialSummaryItem GetStageGateConsolidatedFinancialSummaryItem(int StageGateProjectListItemitemId, string Gate, string BriefNumber);
        int InsertStageGateConsolidatedFinancialSummaryItem(StageGateConsolidatedFinancialSummaryItem item, bool submitted);
        int UpdateStageGateConsolidatedFinancialSummaryItem(StageGateConsolidatedFinancialSummaryItem item, bool submitted);
        List<KeyValuePair<DateTime, string>> GetAllStageGateCreatedFinancialSummaryItems(int StageGateProjectListItemitemId, string Gate);
        #endregion
        #region Financial Analysis
        List<StageGateFinancialAnalysisItem> GetAllStageGateFinancialAnalysisItems(int StageGateProjectListItemitemId);
        List<StageGateFinancialAnalysisItem> GetAllStageGateFinancialAnalysisItemsByGate(int StageGateProjectListItemitemId, string Gate);
        List<StageGateFinancialAnalysisItem> GetAllStageGateFinancialAnalysisItemsByGateAndBriefNumber(int StageGateProjectListItemitemId, string Gate, string BriefNumber);
        int InsertStageGateFinancialAnalysisItem(StageGateFinancialAnalysisItem item, bool submitted);
        List<int> InsertStageGateFinancialAnalysisItems(List<StageGateFinancialAnalysisItem> item, bool submitted);
        int UpdateStageGateFinancialAnalysisItem(StageGateFinancialAnalysisItem item, bool submitted);
        int UpdateAllStageGateFinancialAnalysisItems(List<StageGateFinancialAnalysisItem> item, bool submitted);
        bool DeleteStageGateFinancialAnalysisItem(int StageGateFinancialAnalysisItemId);

        ItemProposalItem GetItemProposalItemByFGNumber(int StageGateProjectListItemitemId, string FGNumber);
        #endregion
    }
}
