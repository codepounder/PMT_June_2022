using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IDashboardService
    {
        List<KeyValuePair<int, Dictionary<string, string>>> GetAllPackagingNumbersForProject(List<int> compassListItemId);
        Task<List<KeyValuePair<int, Dictionary<string, string>>>> GetAllPackagingNumbersForProjectAsync(List<int> compassListItemId);
        ProjectDetailsItem getProjectDetails(int iItemId);
        List<ProjectDetailsItem> getProjectDetailsbyIDs(List<int> iItemIds);
        Task<ProjectDetailsItem> getProjectDetailsAsync(int iItemId);
        WorldSyncReqTask getWorldSyncRequestItem(int requestId);
        List<ProjectDetailsItem> getRequestedProjectDetails(string queryDetails);
        List<ProjectDetailsItem> getRequestedProjectDetails2(string queryDetails);
        Task<List<ProjectDetailsItem>> getRequestedProjectDetailsAsync(string queryDetails);
        List<ProjectDetailsItem> getStageGateRequestedProjectDetails(string queryDetails);
        List<ItemProposalItem> getRequestedChildProjectDetails(int parentID);
        List<ProjectDetailsItem> getAllProjectDetails(List<int> ids);
        List<ProjectTimelineItem> GetProjectsItem(List<int> ids);
        List<TimelineTypeItem> GetWorkflowStepItems();
        List<WorldSyncReqTask> getWorldSyncRequestItems(string queryDetails);
        List<WorkflowTaskDetailsItem> getWorkflowTaskItems(string userId);
        List<WorkflowTaskDetailsItem> getWorldSyncWorkflowTaskItems(string userId);
        List<KeyValuePair<int, Dictionary<string, string>>> GetNewPackagingNumbersForProject(List<int> compassListItemIds);
        List<ItemProposalItem> getChildProjectsByProjectTeam(string queryDetails);
        void moveChildProject(ItemProposalItem IPFItem);
        void setGenerateIPFStartDate(int StageGateListItemId);
    }
}
