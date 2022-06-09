using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Microsoft.SharePoint;

namespace Ferrara.Compass.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly ICacheManagementService cacheManagementService;
        private readonly IExceptionService exceptionService;
        private static object _lock = new object();

        public EmailTemplateService(ICacheManagementService cacheManagementService, IExceptionService exService)
        {
            this.cacheManagementService = cacheManagementService;
            this.exceptionService = exService;
        }

        public void ResetEmailTemplatesCache(string webUrl)
        {
            cacheManagementService.DeleteCache(CacheKeys.EmailTemplateList);
            GetEmailTemplates(webUrl);
        }

        public List<EmailTemplateField> GetEmailTemplates(string webUrl)
        {
            List<EmailTemplateField> emailTemplates = cacheManagementService.GetFromCache<List<EmailTemplateField>>(CacheKeys.EmailTemplateList);
            if (emailTemplates == null)
            {
                emailTemplates = new List<EmailTemplateField>();
                lock (_lock)
                {
                    using (SPSite site = new SPSite(webUrl))
                    {
                        using (SPWeb oWeb = site.OpenWeb())
                        {
                            SPList oList = oWeb.Lists.TryGetList(GlobalConstants.LIST_EmailTemplates);
                            if (oList.Items.Count > 0)
                            {
                                foreach (SPListItem item in oList.Items)
                                {
                                    EmailTemplateField field = new EmailTemplateField
                                    {
                                        Title = (string)item[EmailTemplateFieldName.Title],
                                        Body = (string)item[EmailTemplateFieldName.Body],                                        
                                        Subject = (string)item[EmailTemplateFieldName.Subject]
                                    };
                                    emailTemplates.Add(field);
                                }
                            }
                            cacheManagementService.AddToCache<List<EmailTemplateField>>(CacheKeys.EmailTemplateList, emailTemplates, new TimeSpan(24, 0, 0));
                        }
                    }
                }
            }
            return emailTemplates;
        }

        public EmailTemplateField GetEmailTemplate(string templateName)
        {
            // Get the list of available email templates
            List<EmailTemplateField> emailTemplateList = GetEmailTemplates(SPContext.Current.Web.Url).ToList();

            // Locate the template required
            EmailTemplateField emailTemplate = emailTemplateList.FirstOrDefault(x => x.Title.ToUpper().Equals(templateName));

            if (emailTemplate != null)
            {
                return emailTemplate;
            }
            else
            {
                // Couldn't find the template... reloading cache and getting again
                ResetEmailTemplatesCache(SPContext.Current.Web.Url);
                emailTemplate = GetEmailTemplateFromList(templateName);

                exceptionService.Handle(Abstractions.Enum.LogCategory.CriticalError, "Unable to locate key", "EmailTemplateService", "GetEmailTemplate", "Key: " + templateName);
                if (emailTemplate == null)
                    exceptionService.Handle(Abstractions.Enum.LogCategory.CriticalError, "Unable to load key", "EmailTemplateService", "GetEmailTemplate", "Key: " + templateName);
            }

            return null;
        }

        public EmailTemplateField GetEmailTemplateFromList(string key)
        {
            EmailTemplateField emailTemplate = new EmailTemplateField();

            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var oWeb = spSite.OpenWeb())
                {
                    SPList oList = oWeb.Lists.TryGetList(GlobalConstants.LIST_EmailTemplates);
                    var oItem = oList.Items.OfType<SPListItem>().FirstOrDefault(x => x["Title"].Equals(key));
                    if (oItem != null)
                    {
                        if (oItem["Value"] != null)
                        {
                            emailTemplate.Body = oItem[EmailTemplateFieldName.Body].ToString();
                            emailTemplate.Subject = oItem[EmailTemplateFieldName.Subject].ToString();
                            emailTemplate.Title = oItem[EmailTemplateFieldName.Title].ToString();
                            return emailTemplate;
                        }
                    }
                }
            }

            return null;
        }
    }
}
