using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferrara.Compass.Abstractions.Constants
{
    public class SystemConfiguration
    {
        public Dictionary<string, string> Configurations { get; set; }
        public SystemConfiguration()
        {
            Configurations = new Dictionary<string, string>();
        }

        public static readonly string PMTWorkflowVersion = "PMTWorkflowVersion";
        public static readonly string SMTPServerName = "SMTPServerName";
        public static readonly string SMTPFromEmailAddress = "SMTPFromEmailAddress";
        public static readonly string SystemLogMaximumNumberOfLogsAllowed = "SystemLog_MaximumNumberOfLogsAllowed";
        public static readonly string SystemLogNumberOfLogsToBeDeleted = "SystemLog_NumberOfLogsToBeDeleted";
        public static readonly string CompassMessage = "CompassMessage";
        public static readonly string RYGAverageCount = "RYGAverageCount";
        public static readonly string HelpDeskEmail = "HelpDeskEmail";
        public static readonly string ManageEngineAPIKey = "ManageEngineAPIKey";
    }
}
