
using Ferrara.Compass.Abstractions.Models;
using System;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface ILoggerService
    {
        void InsertLog(LogEntry logentry);
        void InsertLog(LogEntry logentry, string webUrl);
        void InsertGraphicsImportLog(LogEntry logentry);
    }
}
