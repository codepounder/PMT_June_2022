using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IProjectTimelineTypeService
    {
        List<TimelineTypeItem> GetPhases();
        List<TimelineTypeItem> GetWorkflowStepItems(DashboardDetailsItem dashboardDetails);
        int GetSingleWorkflowStepItem(string timelineType, string task);
        List<TimelineTypeItem> GetAllWorkflowStepItems();
        List<List<string>> GetWorkflowTasksStart(int compassId);
        List<List<string>> GetWorkflowTasksEnd(int compassId);
        DashboardDetailsItem dashboardDetails(int compassID);
        int GetStackedItems(int exception, string timelineType);
        List<DateTime> GetHolidays();
        void dashboardUpdateDates(int CompassListItemId, string whichDate, string date);
        void workflowStatusUpdate(int CompassListItemId, string status);
        string GetTimelineType(string compassID);
        Tuple<List<ProjectStatusReportItem>, List<ProjectStatusReportItem>, int, int, DateTime, List<DateTime>, bool> actualTimeLine(DashboardDetailsItem dashboardDetails, bool pageName);
        List<ProjectStatusReportItem> originalTimeLine(DateTime firstShipDate, DateTime ipfSubmit, DateTime newIPFStart, List<TimelineTypeItem> phases, List<TimelineTypeItem> tasks, Dictionary<string, string> hideRows, List<DateTime> holidaysList);
        double totalDaysCalc(DateTime start, DateTime end, List<DateTime> holidays);
        double weekends(DateTime start, DateTime end, List<DateTime> holidays, Boolean leftSpace);
        int exclDays(DateTime start, DateTime end);
        int exclHolidays(DateTime start, DateTime end, List<DateTime> holidays);
        double getWidth(DateTime start, DateTime end, List<DateTime> weekendList, Boolean change, Boolean leftSpace);
    }
}
