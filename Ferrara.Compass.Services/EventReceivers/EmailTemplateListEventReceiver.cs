using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.Practices.Unity;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Unity;

namespace Ferrara.Compass.Services.EventReceivers
{
    public class EmailTemplateListEventReceiver : SPItemEventReceiver
    {

        private IEmailTemplateService emailTemplateService;
        private void Init()
        {
            this.emailTemplateService = FerraraUnityContainer.Container.Resolve<IEmailTemplateService>();
        }


        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);
            Init();
            emailTemplateService.ResetEmailTemplatesCache(properties.Web.Url);
        }

        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);
            Init();
            emailTemplateService.ResetEmailTemplatesCache(properties.Web.Url);

        }

        public override void ItemDeleting(SPItemEventProperties properties)
        {
            base.ItemDeleted(properties);
            Init();
            emailTemplateService.ResetEmailTemplatesCache(properties.Web.Url);
        }



    }
}
