using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Models;
using Microsoft.SharePoint;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IItemProposalService
    {
        Boolean IsExistingProjectNo(string projectNo);

        ItemProposalItem GetItemProposalItem(int itemId);
        List<ItemProposalItem> GetActiveItemProposals();
        List<ItemProposalItem> GetGraphicsReviewProposals();

        int InsertItemProposalItem(ItemProposalItem ipItem);
        int InsertCompassList2(ItemProposalItem ipItem);
        void UpdateItemProposalItem(ItemProposalItem ipItem, bool Submitted);

        SPUser GetItemProposalBrandManager(int itemId);
        void CopyCompassItem(int copyId, int newItemId, string CopyMode, bool CopyTeam = true);
        void CopyCompassList2Item(int odlCompassListItemId, int newCompassListItemId, string newProjectNumber, string CopyMode);
        void CopyFormsFromPreviousprojects(int PreviousItemId, int CurrentItemId, string newProjectNumber);
        void CancelItemProposal(int itemId, string newProjectId, string ProjectRejected);
        void CancelItemProposals(string projectId, string ProjectRejected);

        #region Approval Methods
        ApprovalItem GetItemProposalApprovalItem(int itemId);
        int InsertApprovalItem(ApprovalItem approvalItem, string title);
        void UpdateItemProposalApprovalItem(ApprovalItem approvalItem, bool bSubmitted);
        void UpdateIPF(IPFUpdateItem IPFUpdateItem, bool bSubmitted);
        bool IPFSubmitted(int itemId);
        #endregion

        int InsertProjectDecisionItem(int itemId, string title);
        int InsertEmailLoggingItem(int itemId, string title);
        int InsertWorkflowStatusItem(int itemId, string title);
        List<PackagingItem> GetFinishedPackingComponentsForProject(int stageListItemId);
        List<PackagingItem> GetAllPackagingComponentsForProject(int stageListItemId);
        MarketingClaimsItem GetMarketingClaimsItem(int itemId);
        void InsertMarketingClaimsItem(MarketingClaimsItem ipItem);
        void UpdateMarketingClaimsItem(MarketingClaimsItem ipItem);
        void CopyMarketingClaimsItem(int copyId, int newItemId, string projectNumber, string Mode);
        string GetTBDIndicator(int itemId);
    }
}
