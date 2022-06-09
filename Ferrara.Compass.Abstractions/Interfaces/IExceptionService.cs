using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IExceptionService
    {
        bool Handle(LogCategory category, Exception exception);
        bool Handle(LogCategory category, Exception exception, string form, string method);
        bool Handle(LogCategory category, Exception exception, string form, string method, string additionalInfo);
        bool Handle(LogCategory category, Exception exception, string form, string method, string additionalInfo, string webUrl);
        bool Handle(LogCategory category, string error, string form, string method, string additionalInfo);

        bool HandleGraphicsImport(GraphicsLogCategory category, string error, string form, string materialNumber);
    }
}
