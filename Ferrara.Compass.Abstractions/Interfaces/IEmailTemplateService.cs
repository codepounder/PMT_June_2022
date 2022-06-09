using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IEmailTemplateService
    {
        void ResetEmailTemplatesCache(string webUrl);
        List<EmailTemplateField> GetEmailTemplates(string webUrl);
        EmailTemplateField GetEmailTemplate(string templateName);
        EmailTemplateField GetEmailTemplateFromList(string key);
    }
}
