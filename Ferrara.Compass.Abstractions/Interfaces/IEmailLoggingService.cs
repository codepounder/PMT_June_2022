using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IEmailLoggingService
    {
        int InsertEmailLoggingItem(int compassListItemId, string title);
        EmailLoggingListItem GetEmailLoggingItem(int itemId);
        EmailLoggingSentVersions GetEmailLoggingVersions(int itemId);

        // Email Sent
        void LogSentEmail(EmailLoggingListItem emailLoggingListItem, string title);
        //EmailLoggingSentVersions GetEmailLoggingHistory(int itemId);
        List<KeyValuePair<string, WorkflowStep>> GetEmailLoggingHistory(int itemId);
        List<string> GetVersionDisplay(List<KeyValuePair<string, WorkflowStep>> versionList, string workflowStep);
        void LogSentEmailUpdate(int CompassItemId, string workflowTask, string projectName);
    }
}
