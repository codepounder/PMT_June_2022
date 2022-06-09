using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Microsoft.SharePoint;

namespace Ferrara.Compass.Services
{
    public class ProjectHeaderService : IProjectHeaderService
    {
        public ProjectHeaderItem GetProjectHeaderItem(int itemId)
        {
            var newItem = new ProjectHeaderItem();
            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    var item = spList.GetItemById(itemId);
                    if (item != null)
                    {
                        newItem.NewIPF = Convert.ToString(item[CompassListFields.NewIPF]);
                        newItem.ProjectNumber = Convert.ToString(item[CompassListFields.ProjectNumber]);
                        newItem.ProjectType = Convert.ToString(item[CompassListFields.ProjectType]);
                        newItem.ProjectTypeSubCategory = Convert.ToString(item[CompassListFields.ProjectTypeSubCategory]);
                        newItem.SAPItemNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                        newItem.SAPDescription = Convert.ToString(item[CompassListFields.SAPDescription]);
                        newItem.WorkflowPhase = Convert.ToString(item[CompassListFields.WorkflowPhase]);
                        newItem.TestProject = Convert.ToString(item[CompassListFields.TestProject]);
                    }
                }
            }
            return newItem;
        }
        public StageGateProjectHeaderItem GetStagegateProjectHeaderItem(int itemId)
        {
            var newItem = new StageGateProjectHeaderItem();
            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);
                    var item = spList.GetItemById(itemId);
                    if (item != null)
                    {
                        newItem.ProjectNumber = Convert.ToString(item[StageGateProjectListFields.ProjectNumber]);
                        newItem.ProjectStage = Convert.ToString(item[StageGateProjectListFields.Stage]);
                        newItem.ProjectName = Convert.ToString(item[StageGateProjectListFields.ProjectName]);
                        newItem.ProjectType = Convert.ToString(item[StageGateProjectListFields.ProjectType]);
                        newItem.ProjectTypeSubCategory = Convert.ToString(item[StageGateProjectListFields.ProjectTypeSubCategory]);
                        newItem.TestProject = Convert.ToString(item[StageGateProjectListFields.TestProject]);
                    }
                }
            }
            return newItem;
        }
        public void updateDebugMode(int iItemId, string debugMode)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPListItem item = spList.GetItemById(iItemId);
                        if (item != null)
                        {
                            item[CompassListFields.TestProject] = debugMode;
                            item["Editor"] = SPContext.Current.Web.CurrentUser;

                            item.Update();
                        }

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });

        }
        public void updateSGSDebugMode(int SGSItemId, string debugMode)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);
                        SPListItem item = spList.GetItemById(SGSItemId);
                        if (item != null)
                        {
                            item[StageGateProjectListFields.TestProject] = debugMode;
                            item["Editor"] = SPContext.Current.Web.CurrentUser;

                            item.Update();
                        }

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });

        }
    }
}
