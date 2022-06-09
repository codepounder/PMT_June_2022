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
    public class ConfigurationListEventReceiver : SPItemEventReceiver
    {
        /// <summary>
        /// An item was updated.
        /// </summary>

        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);
            ClearCache(properties);
        }
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);
            ClearCache(properties);
        }
        public override void ItemDeleted(SPItemEventProperties properties)
        {
            base.ItemDeleted(properties);
            ClearCache(properties);
        }
        private static void ClearCache(SPItemEventProperties properties)
        {
            var cacheManagementService = FerraraUnityContainer.Container.Resolve<ICacheManagementService>();
            cacheManagementService.DeleteCache(CacheKeys.Configurations);
            cacheManagementService.DeleteCache(CacheKeys.EmailTemplateList);
        }
    }
}
