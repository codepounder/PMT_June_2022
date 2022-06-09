using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IExcelService
    {
        string ImportSGSPrepPlatesFile(string filename);
        string ImportSGSCorrugateFile(string filename);
    }
}
