using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;

namespace Ferrara.Compass.Services
{
    public class UserAssignmentService : IUserAssignmentService
    {
        public CompassListItem GetUserAssignmentItem(int itemId)
        {
            CompassListItem sgItem = new CompassListItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem item = spList.GetItemById(itemId);
                    if (item != null)
                    {
                        sgItem.CompassListItemId = item.ID;

                        // IPF Fields
                        //sgItem.IPF_ProjectNumber = Convert.ToString(item[CompassListFields.IPF_ProjectNumber]);
                        ////sgItem.IPF_ProposedItem = Convert.ToString(item[CompassListFields.IPF_ProposedItem]);
                        //// SAP Item Request Fields
                        //sgItem.SIR_SAPItemNumber = Convert.ToString(item[CompassListFields.SIR_SAPItemNumber]);
                        //sgItem.SIR_SAPDescription = Convert.ToString(item[CompassListFields.SIR_SAPDescription]);

                        //sgItem.OBM_PackagingEngineerUser = Convert.ToString(item[CompassListFields.OBM_PackageEngineer]);
                        //sgItem.OBM_OBMUser = Convert.ToString(item[CompassListFields.OBM_OBM]);
                        //sgItem.IPF_BrandManager = Convert.ToString(item[CompassListFields.IPF_BrandManager]);
                        //sgItem.OBM_GraphicsLeadUser = Convert.ToString(item[CompassListFields.OBM_GraphicsLead]);
                    }
                }
            }
            return sgItem;
        }

        public void UpdatePackagingEngineer(BillofMaterialsItem compassListItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        SPListItem item = spList.GetItemById(compassListItem.CompassListItemId);
                        if (item != null)
                        {
                            item[CompassListFields.PackagingEngineerLead] = compassListItem.PackagingEngineerLead;

                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        public void UpdateOBM(CompassListItem compassListItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        SPListItem item = spList.GetItemById(compassListItem.CompassListItemId);
                        if (item != null)
                        {
                            item[CompassListFields.PM] = compassListItem.PM;
                            item[CompassListFields.OBM] = compassListItem.PM;

                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        public void UpdateBrandManager(CompassListItem compassListItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        SPListItem item = spList.GetItemById(compassListItem.CompassListItemId);
                        if (item != null)
                        {
                            //item[CompassListFields.BrandManager] = stageGateListItem.IPF_BrandManager;

                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        public void UpdateGraphicsUser(CompassListItem compassListItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        SPListItem item = spList.GetItemById(compassListItem.CompassListItemId);
                        if (item != null)
                        {
                            //item[CompassListFields.OBM_GraphicsLead] = compassListItem.OBM_GraphicsLeadUser;

                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

    }
}
