using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferrara.Compass.ExpSyncFuseTimerJob.Models
{
    public class LogEntry
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Category { get; set; }
        public string Form { get; set; }
        public string Method { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
