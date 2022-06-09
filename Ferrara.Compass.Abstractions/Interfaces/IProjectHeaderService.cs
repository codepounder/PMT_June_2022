using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IProjectHeaderService
    {
        ProjectHeaderItem GetProjectHeaderItem(int itemId);
        StageGateProjectHeaderItem GetStagegateProjectHeaderItem(int itemId);
        void updateDebugMode(int iItemId, string debugMode);
        void updateSGSDebugMode(int SGSItemId, string debugMode);
    }
}
