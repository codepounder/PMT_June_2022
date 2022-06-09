using System;
using System.Linq;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Constants;

namespace Ferrara.Compass.Services
{
    public class ConfigurationManagementService : IConfigurationManagementService
    {        
        private SystemConfiguration configuration;
        private readonly ICacheManagementService cacheManagementService;
        private readonly IExceptionService exceptionService;
        private static object lockObj = new object();

        public ConfigurationManagementService(ICacheManagementService cacheManagementService, IExceptionService exService)
        {            
            this.cacheManagementService = cacheManagementService;
            this.exceptionService = exService;
        }

        public SystemConfiguration GetConfigurations()
        {
            configuration = cacheManagementService.GetFromCache<SystemConfiguration>(CacheKeys.Configurations);
            if ((configuration == null) || (configuration.Configurations == null) || (configuration.Configurations.Count < 1))
            {
                configuration = new SystemConfiguration();
                configuration.Configurations.Clear();
                string value = string.Empty;

                lock (lockObj)
                {
                    SPUserToken systemAcctToken = SPContext.Current.Site.SystemAccount.UserToken;
                    using (var spSite = new SPSite(SPContext.Current.Web.Url, systemAcctToken))
                    {
                        using (var oWeb = spSite.OpenWeb())
                        {
                            SPList oList = oWeb.Lists.TryGetList(GlobalConstants.LIST_Configurations);
                            if (oList.Items.Count > 0)
                            {
                                foreach (SPListItem oItem in oList.Items)
                                {
                                    value = string.Empty;
                                    if (oItem["Value"] != null)
                                        value = oItem["Value"].ToString();
                                    configuration.Configurations.Add(oItem["Title"].ToString(), value);
                                }
                            }
                            cacheManagementService.AddToCache<SystemConfiguration>(CacheKeys.Configurations, configuration, new TimeSpan(24, 0, 0));
                        }
                    }
                }
            }
            return configuration;
        }

        public Boolean UpdateConfiguration(string configuration, string value)
        {
            // Remove from cache
            cacheManagementService.DeleteCache(CacheKeys.Configurations);

            Boolean bSuccessful = false;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_Configurations);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + configuration + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem item = compassItemCol[0];
                            if (item != null)
                            {
                                item["Value"] = value;
                                item.Update();
                                bSuccessful = true;
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });

            return bSuccessful;
        }

        public Boolean CreateConfiguration(string configuration, string value)
        {
            // Remove from cache
            cacheManagementService.DeleteCache(CacheKeys.Configurations);
            
            Boolean bSuccessful = false;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_Configurations);

                        var item = spList.AddItem();

                        item["Title"] = configuration;
                        item["Value"] = value;

                        item.Update();
                        bSuccessful = true;
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });

            return bSuccessful;
        }

        public string GetConfiguration(string key)
        {
            SystemConfiguration configuration = GetConfigurations();
            string value = string.Empty;

            if (configuration.Configurations.ContainsKey(key))
            {
                configuration.Configurations.TryGetValue(key, out value);
            }
            else
            {
                // Key was not found, reset cache and get value directly
                ResetConfiguration();
                value = GetConfigurationFromList(key);
                exceptionService.Handle(Abstractions.Enum.LogCategory.CriticalError, "Unable to locate key", "ConfigurationManagementService", "GetConfiguration", "Key: " + key);
                if (string.IsNullOrEmpty(value))
                    exceptionService.Handle(Abstractions.Enum.LogCategory.CriticalError, "Unable to load key", "ConfigurationManagementService", "GetConfiguration", "Key: " + key);
            }

            return value;
        }

        

        public string GetConfigurationFromList(string key)
        {
            string configuration = string.Empty;

            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var oWeb = spSite.OpenWeb())
                {
                    SPList oList = oWeb.Lists.TryGetList(GlobalConstants.LIST_Configurations);
                    var oItem = oList.Items.OfType<SPListItem>().FirstOrDefault(x => x["Title"].Equals(key));
                    if (oItem != null)
                    {
                        if (oItem["Value"] != null)
                            configuration = oItem["Value"].ToString();
                    }
                }
            }

            return configuration;
        }

        public void ResetConfiguration()
        {
            // Remove from cache
            cacheManagementService.DeleteCache(CacheKeys.Configurations);
        }
    }
}
