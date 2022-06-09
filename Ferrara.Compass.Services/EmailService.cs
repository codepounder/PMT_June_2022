using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Constants;
using System.Net.Mail;
using Microsoft.SharePoint;
using System.IO;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.Services
{
    public class EmailService : IEmailService
    {
        private readonly IExceptionService exceptionService;
        private readonly IConfigurationManagementService configurationManagementService;

        public EmailService(IConfigurationManagementService configurationManagementService, IExceptionService exceptionService)
        {
            this.exceptionService = exceptionService;
            this.configurationManagementService = configurationManagementService;
        }

        public void SendEmail(string to, string subject, string body)
        {
            SendEmail(new List<string> { to }, new List<string>(), subject, body);
        }

        public void SendEmail(IEnumerable<string> to, IEnumerable<string> cc, string subject, string body)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient { Host = configurationManagementService.GetConfiguration(SystemConfiguration.SMTPServerName) };
                using (MailMessage mailMessage = new MailMessage())
                {
                    foreach (string email in to.Where(email => !string.IsNullOrEmpty(email)))
                    {
                        mailMessage.To.Add(new MailAddress(email));
                    }

                    if (cc != null)
                    {
                        foreach (string email in cc.Where(email => !string.IsNullOrEmpty(email)))
                        {
                            mailMessage.CC.Add(new MailAddress(email));
                        }
                    }

                    mailMessage.From = new MailAddress(configurationManagementService.GetConfiguration(SystemConfiguration.SMTPFromEmailAddress));
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;

                    if (mailMessage.To.Count == 0)
                        throw new ArgumentNullException("Missing recipients ('To').");

                    smtpClient.Send(mailMessage);
                }
            }
            catch (SmtpException ex)
            {
                exceptionService.Handle(Abstractions.Enum.LogCategory.CriticalError, ex);
            }
        }

        public void SendEmail(IEnumerable<string> to, IEnumerable<string> cc, string subject, string body, IEnumerable<string> attachments)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient { Host = configurationManagementService.GetConfiguration(SystemConfiguration.SMTPServerName) };
                using (MailMessage mailMessage = new MailMessage())
                {
                    foreach (string email in to.Where(email => !string.IsNullOrEmpty(email)))
                    {
                        mailMessage.To.Add(new MailAddress(email));
                    }

                    if (cc != null)
                    {
                        foreach (string email in cc.Where(email => !string.IsNullOrEmpty(email)))
                        {
                            mailMessage.To.Add(new MailAddress(email));
                        }
                    }

                    if (attachments != null)
                    {
                        string fileName;
                        foreach (string emailAttachment in attachments.Where(email => !string.IsNullOrEmpty(email)))
                        {
                            fileName = emailAttachment.Substring(emailAttachment.LastIndexOf("/") + 1, emailAttachment.Length - emailAttachment.LastIndexOf("/") - 1);
                            System.Net.Mail.Attachment attach = System.Net.Mail.Attachment.CreateAttachmentFromString(emailAttachment, fileName);
                            mailMessage.Attachments.Add(attach);
                        }
                    }

                    mailMessage.From = new MailAddress(configurationManagementService.GetConfiguration(SystemConfiguration.SMTPFromEmailAddress));
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;

                    if (mailMessage.To.Count == 0)
                        throw new ArgumentNullException("Missing recipients ('To').");

                    smtpClient.Send(mailMessage);
                }
            }
            catch (SmtpException ex)
            {
                exceptionService.Handle(Abstractions.Enum.LogCategory.CriticalError, ex);
            }
        }

        public void SendEmail(IEnumerable<string> to, IEnumerable<string> cc, string subject, string body, string projectNumber)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient { Host = configurationManagementService.GetConfiguration(SystemConfiguration.SMTPServerName) };
                using (MailMessage mailMessage = new MailMessage())
                {
                    foreach (string email in to.Where(email => !string.IsNullOrEmpty(email)))
                    {
                        mailMessage.To.Add(new MailAddress(email));
                    }

                    if (cc != null)
                    {
                        foreach (string email in cc.Where(email => !string.IsNullOrEmpty(email)))
                        {
                            mailMessage.To.Add(new MailAddress(email));
                        }
                    }

                    // Get all the attachments on this project
                    using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb spweb = spsite.OpenWeb())
                        {
                            SPDocumentLibrary documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassLibraryName) as SPDocumentLibrary;
                            var folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", projectNumber);
                            if (spweb.GetFolder(folderUrl).Exists)
                            {
                                SPFolder stageGateProjectFolder = spweb.GetFolder(folderUrl);
                                Stream contentStream;
                                System.Net.Mail.Attachment attachment;
                                byte[] content;
                                try
                                {
                                    var spFiles = stageGateProjectFolder.Files.OfType<SPFile>().Where(x => x.Item[CompassListFields.DOCLIBRARY_CompassDocType].Equals(GlobalConstants.DOCTYPE_NLEA));
                                    foreach (SPFile spfile in spFiles)
                                    {
                                        content = spfile.OpenBinary();
                                        contentStream = new MemoryStream(content);
                                        attachment = new Attachment(contentStream, spfile.Name);
                                        mailMessage.Attachments.Add(attachment);
                                    }

                                    spFiles = stageGateProjectFolder.Files.OfType<SPFile>().Where(x => x.Item[CompassListFields.DOCLIBRARY_CompassDocType].Equals(GlobalConstants.DOCTYPE_PalletPattern));
                                    foreach (SPFile spfile in spFiles)
                                    {
                                        content = spfile.OpenBinary();
                                        contentStream = new MemoryStream(content);
                                        attachment = new Attachment(contentStream, spfile.Name);
                                        mailMessage.Attachments.Add(attachment);
                                    }

                                    spFiles = stageGateProjectFolder.Files.OfType<SPFile>().Where(x => x.Item[CompassListFields.DOCLIBRARY_CompassDocType].Equals(GlobalConstants.DOCTYPE_GraphicsRequest));
                                    foreach (SPFile spfile in spFiles)
                                    {
                                        content = spfile.OpenBinary();
                                        contentStream = new MemoryStream(content);
                                        attachment = new Attachment(contentStream, spfile.Name);
                                        mailMessage.Attachments.Add(attachment);
                                    }

                                    spFiles = stageGateProjectFolder.Files.OfType<SPFile>().Where(x => x.Item[CompassListFields.DOCLIBRARY_CompassDocType].Equals(GlobalConstants.DOCTYPE_CADDrawing));
                                    foreach (SPFile spfile in spFiles)
                                    {
                                        content = spfile.OpenBinary();
                                        contentStream = new MemoryStream(content);
                                        attachment = new Attachment(contentStream, spfile.Name);
                                        mailMessage.Attachments.Add(attachment);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    exceptionService.Handle(LogCategory.CriticalError, ex, "EmailService", "SendEmail(IEnumerable<string> to, IEnumerable<string> cc, string subject, string body, string projectNumber)", projectNumber);
                                }
                            }
                        }
                    }

                    mailMessage.From = new MailAddress(configurationManagementService.GetConfiguration(SystemConfiguration.SMTPFromEmailAddress));
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;

                    if (mailMessage.To.Count == 0)
                        throw new ArgumentNullException("Missing recipients ('To').");

                    smtpClient.Send(mailMessage);
                }
            }
            catch (SmtpException ex)
            {
                exceptionService.Handle(Abstractions.Enum.LogCategory.CriticalError, ex);
            }
        }


        public void SendEmailComponentCostingRequest(IEnumerable<string> to, IEnumerable<string> cc, string subject, string body, string projectNumber, int packagingItemId, string docType)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient { Host = configurationManagementService.GetConfiguration(SystemConfiguration.SMTPServerName) };
                using (MailMessage mailMessage = new MailMessage())
                {
                    foreach (string email in to.Where(email => !string.IsNullOrEmpty(email)))
                    {
                        mailMessage.To.Add(new MailAddress(email));
                    }

                    if (cc != null)
                    {
                        foreach (string email in cc.Where(email => !string.IsNullOrEmpty(email)))
                        {
                            mailMessage.To.Add(new MailAddress(email));
                        }
                    }

                    // Get all the attachments on this project
                    using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb spweb = spsite.OpenWeb())
                        {
                            SPDocumentLibrary documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassLibraryName) as SPDocumentLibrary;
                            var folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", projectNumber);
                            if (spweb.GetFolder(folderUrl).Exists)
                            {
                                SPFolder stageGateProjectFolder = spweb.GetFolder(folderUrl);
                                Stream contentStream;
                                System.Net.Mail.Attachment attachment;
                                byte[] content;
                                try
                                {
                                    var spFiles = stageGateProjectFolder.Files.OfType<SPFile>().Where(x => Convert.ToInt32(x.Item[CompassListFields.DOCLIBRARY_PackagingComponentItemId]).Equals(packagingItemId) && x.Item[CompassListFields.DOCLIBRARY_CompassDocType].Equals(docType)).ToList();
                                    foreach (SPFile spfile in spFiles)
                                    {
                                        content = spfile.OpenBinary();
                                        contentStream = new MemoryStream(content);
                                        attachment = new Attachment(contentStream, spfile.Name);
                                        mailMessage.Attachments.Add(attachment);
                                    }


                                }
                                catch (Exception ex)
                                {
                                    exceptionService.Handle(LogCategory.CriticalError, ex, "EmailService", "SendEmailComponentCostingRequest(IEnumerable<string> to, IEnumerable<string> cc, string subject, string body, string projectNumber, int packagingItemId, string docType)", projectNumber);
                                }
                            }
                        }
                    }

                    mailMessage.From = new MailAddress(configurationManagementService.GetConfiguration(SystemConfiguration.SMTPFromEmailAddress));
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;

                    if (mailMessage.To.Count == 0)
                        throw new ArgumentNullException("Missing recipients ('To').");

                    smtpClient.Send(mailMessage);
                }
            }
            catch (SmtpException ex)
            {
                exceptionService.Handle(Abstractions.Enum.LogCategory.CriticalError, ex);
            }
        }

        public void SendEmail(IEnumerable<string> to, IEnumerable<string> cc, string subject, string body, SPUser user)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient { Host = configurationManagementService.GetConfiguration(SystemConfiguration.SMTPServerName) };
                using (MailMessage mailMessage = new MailMessage())
                {
                    foreach (string email in to.Where(email => !string.IsNullOrEmpty(email)))
                    {
                        mailMessage.To.Add(new MailAddress(email));
                    }

                    if (cc != null)
                    {
                        foreach (string email in cc.Where(email => !string.IsNullOrEmpty(email)))
                        {
                            mailMessage.To.Add(new MailAddress(email));
                        }
                    }

                    mailMessage.From = new MailAddress(user.Email);
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;

                    if (mailMessage.To.Count == 0)
                        throw new ArgumentNullException("Missing recipients ('To').");

                    smtpClient.Send(mailMessage);
                }
            }
            catch (SmtpException ex)
            {
                exceptionService.Handle(Abstractions.Enum.LogCategory.CriticalError, ex);
            }
        }
    }
}
