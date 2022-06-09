using Ferrara.Compass.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IStageGateGeneralService
    {
        void UpdateStageGateGateListSubmitDetails(StageGateGateItem gateItem);
        List<StageGateNecessaryDeliverablesItem> GetStageGateDeliverables(string listName);
        int UpsertDeliverables(int StageGateListItemId, string stage, List<StageGateNecessaryDeliverablesItem> necessaryDeliverables, string projectNumber);
        List<StageGateNecessaryDeliverablesItem> GetSavedStageGateDeliverables(int StageGateListItemId, string Stage);
        StageGateGateItem GetStageGateGateItem(int StageGateListItemId, int gateNo);
        List<StageGateGateItem> GetStageGateBriefItem(int StageGateListItemId, int gateNo);
        StageGateGateItem GetSingleStageGateBriefItem(int briefID);
        int UpsertGateBriefItem(StageGateGateItem gateItem);
        void UpsertGateDetsItem(StageGateGateItem gateItem);
        List<KeyValuePair<DateTime, string>> GateSubmittedVersions(int StageGateItemId, int gateNo);
        List<KeyValuePair<DateTime, string>> SubmittedVersions(int StageGateItemId, int gateNo, string field, string table, string submitterField);
        List<KeyValuePair<DateTime, string>> Gate0SubmittedVersions(int StageGateItemId, int gateNo, string field, string table, string submitterField);
        void UpdateProjectItem(StageGateCreateProjectItem stageItem);
        void deleteChildProjectDetails(int TempProjectId);
        void deleteDeliverable(int deliverableId);
        int upsertTempIPFList(ItemProposalItem newItem);
        ItemProposalItem GetTempIPFItem(int TempItemId, int StageGateParentProjectId);
        void DeleteProjectGateInfo(int deleteID);
        //int geChildProjectCount(int PMTProjectId);
        bool UploadStageGateDocument(List<FileAttribute> fileList, string projectNo, string docType, string Gate, string BriefNo);
        List<FileAttribute> GetUploadedStageGateFiles(string projectNo, string docType, string Gate, string BriefNo, string webUrl);
        List<FileAttribute> GetStageGateFiles(string projectNo, string webUrl);
        void DeleteStageGateFiles(string folder, string docType, string Gate, string BriefNo);
        void updateChildItemDetails(ItemProposalItem ipfItem);
        void updateStageforPostLaunch(StageGateCreateProjectItem stageItem);
        void updateCurrentStage(StageGateCreateProjectItem stageItem);
        void updateRevisedFirstShip(StageGateCreateProjectItem stageItem);
        void SetPrelimStartDate(int compassListItemId, DateTime startDate);
        List<ItemProposalItem> GetSearchProjectName(string ProjectNo);
        List<ItemProposalItem> GetSearchParentProjectName(string ProjectNo);
        void updateSGSPHL2(ItemProposalItem stageItem);
        void updateReadinessDetails(StageGateGateItem gateItem);
        void cancelProject(int StageGateListItemID);
        void ProjectOnHold(int StageGateListItemID);
        void RemoveProjectOnHold(int StageGateListItemID, int TotalOnHoldDays);
        bool CheckPMTWorkflowUpdateStatuses(int StageGateListItemId, string status);
        List<StageGateGateItem> GetOtherStageGateBriefItem(int StageGateListItemId, int gateNo);
        int insertTempIPFList(ItemProposalItem newItem);
    }
}
