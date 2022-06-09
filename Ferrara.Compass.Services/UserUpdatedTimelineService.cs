using System;
using System.Collections.Generic;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;

namespace Ferrara.Compass.Services
{
    public class UserUpdatedTimelineService : IUserUpdatedTimelineService
    {
        public List<UserUpdatedTimelineItem> GetUserUpdatedTimeline(String projectNumber)
        {
            var newItem = new List<UserUpdatedTimelineItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_UserUpdatedTimelineListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"Project Number\" /><Value Type=\"String\">" + projectNumber + "</Value></Eq></Where>";
                    SPListItemCollection compassItemCol;
                    compassItemCol = spList.GetItems(spQuery);

                    foreach (SPListItem item in compassItemCol)
                    {
                        if (item != null)
                        {

                            UserUpdatedTimelineItem obUserUpdatedTimelineItem = new UserUpdatedTimelineItem();
                            obUserUpdatedTimelineItem.CompassListItemId = Convert.ToInt32(UserUpdatedTimeline.CompassListItemId);
                            obUserUpdatedTimelineItem.BOMPE = Convert.ToInt32(UserUpdatedTimeline.BOMPE);
                            obUserUpdatedTimelineItem.BOMPE2 = Convert.ToInt32(UserUpdatedTimeline.BOMPE2);
                            obUserUpdatedTimelineItem.BOMPROC = Convert.ToInt32(UserUpdatedTimeline.BOMPROC);
                            obUserUpdatedTimelineItem.Distribution = Convert.ToInt32(UserUpdatedTimeline.Distribution);
                            obUserUpdatedTimelineItem.SrOBMApproval = Convert.ToInt32(UserUpdatedTimeline.SrOBMApproval);
                            obUserUpdatedTimelineItem.SrOBMApproval2 = Convert.ToInt32(UserUpdatedTimeline.SrOBMApproval2);
                            obUserUpdatedTimelineItem.SAPInitialSetup = Convert.ToInt32(UserUpdatedTimeline.SAPInitialSetup);
                            obUserUpdatedTimelineItem.PrelimSAPInitialSetup = Convert.ToInt32(UserUpdatedTimeline.PrelimSAPInitialSetup);
                            obUserUpdatedTimelineItem.InitialCapacity = Convert.ToInt32(UserUpdatedTimeline.InitialCapacity);
                            obUserUpdatedTimelineItem.IPF = Convert.ToInt32(UserUpdatedTimeline.IPF);
                            obUserUpdatedTimelineItem.Operations = Convert.ToInt32(UserUpdatedTimeline.Operations);
                            obUserUpdatedTimelineItem.OBMReview1 = Convert.ToInt32(UserUpdatedTimeline.OBMReview1);
                            obUserUpdatedTimelineItem.OBMReview2 = Convert.ToInt32(UserUpdatedTimeline.OBMReview2);
                            obUserUpdatedTimelineItem.OBMReview3 = Convert.ToInt32(UserUpdatedTimeline.OBMReview3);
                            obUserUpdatedTimelineItem.QA = Convert.ToInt32(UserUpdatedTimeline.QA);
                            obUserUpdatedTimelineItem.RNDFINAL = Convert.ToInt32(UserUpdatedTimeline.RNDFINAL);
                            obUserUpdatedTimelineItem.TradePromo = Convert.ToInt32(UserUpdatedTimeline.TradePromo);
                            obUserUpdatedTimelineItem.ExternalMfg = Convert.ToInt32(UserUpdatedTimeline.ExternalMfg);
                            obUserUpdatedTimelineItem.PACKENG1 = Convert.ToInt32(UserUpdatedTimeline.PACKENG1);
                            obUserUpdatedTimelineItem.PACKENG2 = Convert.ToInt32(UserUpdatedTimeline.PACKENG2);
                            obUserUpdatedTimelineItem.PACKPROC = Convert.ToInt32(UserUpdatedTimeline.PACKPROC);
                            obUserUpdatedTimelineItem.NewXferSemi = Convert.ToInt32(UserUpdatedTimeline.NewXferSemi);
                            obUserUpdatedTimelineItem.GRAPHICS = Convert.ToInt32(UserUpdatedTimeline.GRAPHICS);
                            obUserUpdatedTimelineItem.FCST = Convert.ToInt32(UserUpdatedTimeline.FCST);
                            obUserUpdatedTimelineItem.FGPackSpec = Convert.ToInt32(UserUpdatedTimeline.FGPackSpec);
                            obUserUpdatedTimelineItem.CostingQuote = Convert.ToInt32(UserUpdatedTimeline.CostingQuote);
                            obUserUpdatedTimelineItem.InitialCosting = Convert.ToInt32(UserUpdatedTimeline.InitialCosting);

                            newItem.Add(obUserUpdatedTimelineItem);
                        }
                    }
                }
            }
            return newItem;
        }
        
    }
}
