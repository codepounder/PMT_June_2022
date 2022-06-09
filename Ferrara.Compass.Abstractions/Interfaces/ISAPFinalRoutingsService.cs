using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface ISAPFinalRoutingsService
    {
        List<FinalRoutingsItem> GetSAPFinalRoutingsItems(string sapItemNumber);
        FinalRoutingsItem GetSingleSAPFinalRoutingsItem(string sapItemNumber, string packPlant);
        string getProjectPackPlant(int iItemId);
        SAPApprovalListItem getSAPApprovalItem(int iItemId);
        PackagingItem getCompassItem(int iItemId);

    }
}
