using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IFinalCostingService
    {
        CompassListItem GetFinalCostingItem(int itemId);
        void UpdateFinalCostingItem(CompassListItem compassListItem, int itemId);
    }
}
