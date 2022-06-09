using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.Practices.Unity;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Unity;
using Ferrara.Compass.Abstractions.Constants;

namespace Ferrara.Compass.Services.EventReceivers
{
    public class WorkflowStepListEventReceiver : SPItemEventReceiver
    {
        private ICacheManagementService cacheManagementService;

        private void Init()
        {
            this.cacheManagementService = FerraraUnityContainer.Container.Resolve<ICacheManagementService>();
        }

        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);
            Init();
            cacheManagementService.DeleteCache(CacheKeys.WFStepList);
        }

        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);
            Init();
            cacheManagementService.DeleteCache(CacheKeys.WFStepList);
        }

        public override void ItemDeleting(SPItemEventProperties properties)
        {
            base.ItemDeleted(properties);
            Init();
            cacheManagementService.DeleteCache(CacheKeys.WFStepList);
        }
    }
}
