using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface ICompassWorldSyncService
    {
        CompassWorldSyncListItem GetCompassWorldSyncListItem(int compassItemId, int childId);
        List<CompassWorldSyncListItem> GetCompassWorldSyncChildListItems(int compassItemId);
        int UpsertCompassWorldSyncListItem(CompassWorldSyncListItem worldSync);
        void DeleteWorldSyncDetailItem(int deletedId);
    }
}
