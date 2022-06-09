using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(IEnumerable<string> to, IEnumerable<string> cc, string subject, string body);
        void SendEmail(string to, string subject, string body);
        void SendEmail(IEnumerable<string> to, IEnumerable<string> cc, string subject, string body, IEnumerable<string> attachments);
        void SendEmail(IEnumerable<string> to, IEnumerable<string> cc, string subject, string body, string projectNumber);
        void SendEmail(IEnumerable<string> to, IEnumerable<string> cc, string subject, string body, SPUser user);
        void SendEmailComponentCostingRequest(IEnumerable<string> to, IEnumerable<string> cc, string subject, string body, string projectNumber, int packagingItemId, string docType);
    }
}
