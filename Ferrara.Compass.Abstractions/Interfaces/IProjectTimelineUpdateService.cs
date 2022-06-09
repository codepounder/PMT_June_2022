using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IProjectTimelineUpdateService
    {
        int GetProjectTimelineItem(int compassListId);
        void UpdateProjectTimelineItem(ProjectTimelineItem projectTimelineItem, string title);
        int InsertProjectTimelineItem(ProjectTimelineItem projectTimelineItem, string title);
        List<List<string>> GetProjectItem(int compassListId);
        List<KeyValuePair<string, string>> GetCompletedItems(int compassListId);
        void UpdateSingleProjectTimelineItem(String column, string value, int compassId, string timelineType, string ProjectNo);
        int GetSingleProjectItem(int compassListId, string task);
        List<Tuple<int, string, int>> GetProjectItems(List<WorkflowTaskDetailsItem> taskItems);
        double getEstProjectTotalDays(int compassId, string timelineType);
    }
}
