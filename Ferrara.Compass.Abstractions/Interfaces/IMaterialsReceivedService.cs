using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IMaterialsReceivedService
    {
        List<MaterialsReceivedItem> getMaterialsReceivedItem(int compassID);
    }
}
