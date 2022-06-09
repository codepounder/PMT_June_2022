using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.Services
{
    public class VersionHistoryService : IVersionHistoryService
    {
        public List<string> GetGraphicsRoutingVersionHistory(int id)
        {
            List<string> versionList = new List<string>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPListItem item = spList.GetItemById(id);

                    if ((item != null) && (item.Versions != null) && (item.Versions.Count > 0))
                    {
                        SPListItemVersionCollection versions = item.Versions;
                        foreach (SPListItemVersion ver in versions)
                        {
                            if (ver[PackagingItemListFields.Graphics_Routing_ModifiedDate] != null)
                                versionList.Add(GetLocalDate(ver[PackagingItemListFields.Graphics_Routing_ModifiedDate].ToString()));
                        }
                    }
                }
            }

            return versionList;
        }

        public string GetVersionDisplay(List<string> versionList)
        {
            if (versionList == null)
                return string.Empty;

            string versions = string.Empty;
            string lastVersion = string.Empty;
            foreach (string str in versionList)
            {
                if (!string.Equals(str, lastVersion))
                    versions = string.Concat(versions, str, "<br>");
                lastVersion = str;
            }
            return versions;
        }

        #region Private Methods
        private string GetLocalDateTime(string datetime)
        {
            DateTime result;
            if (DateTime.TryParse(datetime, out result))
            {
                return result.ToLocalTime().ToString();
            }

            return datetime;
        }

        private string GetLocalDate(string datetime)
        {
            DateTime result;
            if (DateTime.TryParse(datetime, out result))
            {
                return result.ToLocalTime().ToShortDateString();
            }

            return datetime;
        }
        #endregion
    }
}
