using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IUserAssignmentService
    {
        CompassListItem GetUserAssignmentItem(int itemId);
        void UpdatePackagingEngineer(BillofMaterialsItem compassListItem);
        void UpdateOBM(CompassListItem compassListItem);
        void UpdateBrandManager(CompassListItem compassListItem);
        void UpdateGraphicsUser(CompassListItem compassListItem);
    }
}
