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
    public class FinalCostingService : IFinalCostingService
    {
        public CompassListItem GetFinalCostingItem(int itemId)
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
                        //// SAP Item Request Fields
                        //sgItem.SIR_SAPItemNumber = Convert.ToString(item[CompassListFields.SIR_SAPItemNumber]);
                        //sgItem.SIR_SAPDescription = Convert.ToString(item[CompassListFields.SIR_SAPDescription]);

                        //sgItem.REPORT_CriticalInitiative = Convert.ToString(item[CompassListFields.REPORT_CriticalInitiative]);

                        //sgItem.FCF_BrandManagerReview = Convert.ToString(item[CompassListFields.FCF_BrandManagerReview]);
                        //sgItem.FCF_BrandManagerReviewComments = Convert.ToString(item[CompassListFields.FCF_BrandManagerReviewComments]);
                        //sgItem.FCF_CapacityCapabilityReview = Convert.ToString(item[CompassListFields.FCF_CapacityCapabilityReview]);
                        //sgItem.FCF_CapacityCapabilityReviewComments = Convert.ToString(item[CompassListFields.ICF_CapacityCapabilityReviewComments]);
                        //sgItem.FCF_FinalCostingReview = Convert.ToString(item[CompassListFields.FCF_FinalCostingReview]);
                        //sgItem.FCF_FinalCostingReviewComments = Convert.ToString(item[CompassListFields.FCF_FinalCostingReviewComments]);
                        //sgItem.FCF_SeniorLeadershipDecision = Convert.ToString(item[CompassListFields.FCF_SeniorLeadershipDecision]);
                        //sgItem.FCF_SeniorLeadershipDecisionComments = Convert.ToString(item[CompassListFields.FCF_SeniorLeadershipDecisionComments]);
                        //// Modified By
                        //sgItem.FCF_SLTModifiedBy = Convert.ToString(item[CompassListFields.FCF_SLTModifiedBy]);
                        //sgItem.FCF_SLTModifiedDate = Convert.ToDateTime(item[CompassListFields.FCF_SLTModifiedDate]);
                    }
                }
            }
            return sgItem;
        }

        public void UpdateFinalCostingItem(CompassListItem compassListItem, int itemId)
        {
            // Get the current user
            SPUser currentUser = SPContext.Current.Web.CurrentUser;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        SPListItem item = spList.GetItemById(itemId);
                        if (item != null)
                        {
                            //item[StageGateListFields.FCF_BrandManagerReview] = stageGateListItem.FCF_BrandManagerReview;
                            //item[StageGateListFields.FCF_BrandManagerReviewComments] = stageGateListItem.FCF_BrandManagerReviewComments;
                            //item[StageGateListFields.FCF_CapacityCapabilityReview] = stageGateListItem.FCF_CapacityCapabilityReview;
                            //item[StageGateListFields.FCF_CapacityCapabilityReviewComments] = stageGateListItem.FCF_CapacityCapabilityReviewComments;
                            //item[StageGateListFields.FCF_FinalCostingReview] = stageGateListItem.FCF_FinalCostingReview;
                            //item[StageGateListFields.FCF_FinalCostingReviewComments] = stageGateListItem.FCF_FinalCostingReviewComments;
                            //item[StageGateListFields.FCF_SeniorLeadershipDecision] = stageGateListItem.FCF_SeniorLeadershipDecision;
                            //item[StageGateListFields.FCF_SeniorLeadershipDecisionComments] = stageGateListItem.FCF_SeniorLeadershipDecisionComments;
                            // Modified Users
                            //item[StageGateListFields.FCF_SLTModifiedBy] = stageGateListItem.FCF_SLTModifiedBy;
                            //item[StageGateListFields.FCF_SLTModifiedDate] = stageGateListItem.FCF_SLTModifiedDate;


                            // Set Modified By to current user NOT System Account
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
