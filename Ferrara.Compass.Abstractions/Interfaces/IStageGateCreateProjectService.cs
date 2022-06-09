using Ferrara.Compass.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IStageGateCreateProjectService
    {
        StageGateCreateProjectItem GetStageGateProjectItem(int itemId);
        bool CheckOnHoldChildProjects(int itemId);
        int InsertStageGateProjectItem(StageGateCreateProjectItem item, bool submitted);
        int UpdateStageGateProjectItem(StageGateCreateProjectItem item, bool submitted);
        int UpdateStageGateProjectSubmittedEmailSent(StageGateCreateProjectItem sgitem);
        int UpdateStageGateProjectCompletedEmailSent(StageGateCreateProjectItem sgitem);
        int UpdateStageGateProjectCancelledEmailSent(StageGateCreateProjectItem sgitem);
    }
}
