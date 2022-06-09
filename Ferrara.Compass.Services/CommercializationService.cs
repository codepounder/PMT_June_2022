using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using System.Text.RegularExpressions;

namespace Ferrara.Compass.Services
{
    public class CommercializationService : ICommercializationService
    {
        public List<KeyValuePair<string, string>> GetCommercializationItem(int itemId)
        {
            List<KeyValuePair<string, string>> sgItem = new List<KeyValuePair<string, string>>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    //Compass List Query
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem item = spList.GetItemById(itemId);

                    //Compass List 2 Query
                    SPList spCompassList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
                    SPQuery spCompassList2Query = new SPQuery();
                    spCompassList2Query.Query = "<Where><Eq><FieldRef Name=\"" + CompassListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    SPListItemCollection itemCollection = spCompassList2.GetItems(spCompassList2Query);
                    SPListItem item2 = null;
                    if (itemCollection != null && itemCollection.Count > 0)
                    {
                        item2 = itemCollection[0];
                    }
                    else
                    {
                        item2 = AddCompassList2Item(itemId, Convert.ToString(item[CompassListFields.ProjectNumber]));
                    }

                    //Compass Team List Query
                    SPList spCompassTeamList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTeamListName);
                    SPQuery spCompassTeamQuery = new SPQuery();
                    spCompassTeamQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    SPListItemCollection CompassTeamListItems = spCompassTeamList.GetItems(spCompassTeamQuery);

                    //Decisions Query
                    SPList spOpsList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectDecisionsListName);
                    SPQuery spDecQuery = new SPQuery();
                    spDecQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    SPListItemCollection ColItem = spOpsList.GetItems(spDecQuery);

                    //Marketing Claims Query
                    SPList spClaimsList = spWeb.Lists.TryGetList(GlobalConstants.LIST_MarketingClaimsListName);
                    SPQuery spClaimsQuery = new SPQuery();
                    spClaimsQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Text\">" + itemId + "</Value></Eq></Where>";
                    SPListItemCollection ClaimsItem = spClaimsList.GetItems(spClaimsQuery);

                    List<KeyValuePair<string, string>> hideCurrentRow = hideRow(spWeb, itemId);

                    if (item != null)
                    {
                        string PLMFlag = Convert.ToString(item[CompassListFields.PLMProject]);
                        #region Project Team
                        //Assigned Users
                        sgItem.Add(new KeyValuePair<string, string>("Project Team", "header"));
                        #region Initiator
                        string initiator = Convert.ToString(item[CompassListFields.Initiator]);
                        int index = initiator.IndexOf("#");
                        string cleanPath = (index < 0)
                            ? initiator
                            : initiator.Substring(index + 1);
                        sgItem.Add(new KeyValuePair<string, string>("Initiator", cleanPath));
                        #endregion
                        #region Project Leader (PL)
                        string PL = string.Empty;
                        if (CompassTeamListItems != null)
                        {
                            if (CompassTeamListItems.Count > 0)
                            {
                                PL = Convert.ToString(CompassTeamListItems[0][CompassTeamListFields.ProjectLeaderName]);
                            }
                        }
                        sgItem.Add(new KeyValuePair<string, string>("Project Leader", PL));
                        #endregion
                        #region Project Manager
                        sgItem.Add(new KeyValuePair<string, string>("Project Manager (PM)", Convert.ToString(item[CompassListFields.PMName])));
                        #endregion
                        #region Sr. Project Manager (Sr. PM)
                        string SrPM = string.Empty;
                        if (CompassTeamListItems != null)
                        {
                            if (CompassTeamListItems.Count > 0)
                            {
                                SrPM = Convert.ToString(CompassTeamListItems[0][CompassTeamListFields.SeniorProjectManagerName]);
                            }
                        }
                        sgItem.Add(new KeyValuePair<string, string>("Sr. Project Manager (Sr. PM)", SrPM));
                        #endregion
                        #region Marketing
                        string Marketing = string.Empty;
                        if (CompassTeamListItems != null)
                        {
                            if (CompassTeamListItems.Count > 0)
                            {
                                Marketing = Convert.ToString(CompassTeamListItems[0][CompassTeamListFields.MarketingName]);
                            }
                        }
                        sgItem.Add(new KeyValuePair<string, string>("Marketing", Marketing));
                        #endregion
                        #region InTech
                        string InTech = string.Empty;
                        if (CompassTeamListItems != null)
                        {
                            if (CompassTeamListItems.Count > 0)
                            {
                                InTech = Convert.ToString(CompassTeamListItems[0][CompassTeamListFields.InTechName]);
                            }
                        }
                        sgItem.Add(new KeyValuePair<string, string>("InTech", InTech));
                        #endregion
                        #region Quality Innovation
                        string QA = string.Empty;
                        if (CompassTeamListItems != null)
                        {
                            if (CompassTeamListItems.Count > 0)
                            {
                                QA = Convert.ToString(CompassTeamListItems[0][StageGateProjectListFields.QAInnovationName]);
                            }
                        }
                        sgItem.Add(new KeyValuePair<string, string>("Quality Innovation", QA));
                        #endregion
                        #region InTech Regulatory
                        string InTechRegulatory = string.Empty;
                        if (CompassTeamListItems != null)
                        {
                            if (CompassTeamListItems.Count > 0)
                            {
                                InTechRegulatory = Convert.ToString(CompassTeamListItems[0][StageGateProjectListFields.InTechRegulatoryName]);
                            }
                        }
                        sgItem.Add(new KeyValuePair<string, string>("InTech Regulatory", InTechRegulatory));
                        #endregion
                        #region Regulatory QA
                        string QARegulatory = string.Empty;
                        if (CompassTeamListItems != null)
                        {
                            if (CompassTeamListItems.Count > 0)
                            {
                                QARegulatory = Convert.ToString(CompassTeamListItems[0][StageGateProjectListFields.RegulatoryQAName]);
                            }
                        }
                        sgItem.Add(new KeyValuePair<string, string>("Regulatory QA", QARegulatory));
                        #endregion
                        #region Packaging Engineering 
                        string PE = string.Empty;
                        if (CompassTeamListItems != null)
                        {
                            if (CompassTeamListItems.Count > 0)
                            {
                                PE = Convert.ToString(CompassTeamListItems[0][StageGateProjectListFields.PackagingEngineeringName]);
                            }
                        }

                        if (string.IsNullOrEmpty(PE))
                        {
                            PE = "";
                        }

                        sgItem.Add(new KeyValuePair<string, string>("Packaging Engineering", PE));
                        #endregion
                        #region Supply Chain (SC)
                        string SC = string.Empty;
                        if (CompassTeamListItems != null)
                        {
                            if (CompassTeamListItems.Count > 0)
                            {
                                SC = Convert.ToString(CompassTeamListItems[0][StageGateProjectListFields.SupplyChainName]);
                            }
                        }
                        sgItem.Add(new KeyValuePair<string, string>("Supply Chain (SC)", SC));
                        #endregion
                        #region Finance
                        string Finance = string.Empty;
                        if (CompassTeamListItems != null)
                        {
                            if (CompassTeamListItems.Count > 0)
                            {
                                Finance = Convert.ToString(CompassTeamListItems[0][StageGateProjectListFields.FinanceName]);
                            }
                        }
                        sgItem.Add(new KeyValuePair<string, string>("Finance", Finance));
                        #endregion
                        #region Sales
                        string Sales = string.Empty;
                        if (CompassTeamListItems != null)
                        {
                            if (CompassTeamListItems.Count > 0)
                            {
                                Sales = Convert.ToString(CompassTeamListItems[0][StageGateProjectListFields.SalesName]);
                            }
                        }
                        sgItem.Add(new KeyValuePair<string, string>("Sales (SSCM)", Sales));
                        #endregion
                        #region Manufacturing
                        string Manufacturing = string.Empty;
                        if (CompassTeamListItems != null)
                        {
                            if (CompassTeamListItems.Count > 0)
                            {
                                Manufacturing = Convert.ToString(CompassTeamListItems[0][StageGateProjectListFields.ManufacturingName]);
                            }
                        }
                        sgItem.Add(new KeyValuePair<string, string>("Manufacturing", Manufacturing));
                        #endregion
                        #region External Mfg - Procurement
                        string ExtMfg = string.Empty;
                        if (CompassTeamListItems != null)
                        {
                            if (CompassTeamListItems.Count > 0)
                            {
                                ExtMfg = Convert.ToString(CompassTeamListItems[0][StageGateProjectListFields.ExtMfgProcurementName]);
                            }
                        }
                        sgItem.Add(new KeyValuePair<string, string>("External Mfg - Procurement", ExtMfg));
                        #endregion
                        #region Packaging Procurement
                        string PackagingProcurement = string.Empty;
                        if (CompassTeamListItems != null)
                        {
                            if (CompassTeamListItems.Count > 0)
                            {
                                PackagingProcurement = Convert.ToString(CompassTeamListItems[0][StageGateProjectListFields.PackagingProcurementName]);
                            }
                        }
                        sgItem.Add(new KeyValuePair<string, string>("Packaging Procurement", PackagingProcurement));
                        #endregion
                        #region Life Cycle Management
                        string LifeCycleManagement = string.Empty;
                        if (CompassTeamListItems != null)
                        {
                            if (CompassTeamListItems.Count > 0)
                            {
                                LifeCycleManagement = Convert.ToString(CompassTeamListItems[0][StageGateProjectListFields.LifeCycleManagementName]);
                            }
                        }
                        sgItem.Add(new KeyValuePair<string, string>("Life Cycle Management", LifeCycleManagement));
                        #endregion
                        #region Other Team Members
                        string OtherTeamMembers = string.Empty;
                        if (CompassTeamListItems != null)
                        {
                            if (CompassTeamListItems.Count > 0)
                            {
                                OtherTeamMembers = Convert.ToString(CompassTeamListItems[0][StageGateProjectListFields.OtherMemberName]);
                            }
                        }
                        sgItem.Add(new KeyValuePair<string, string>("Other Team Members", OtherTeamMembers));
                        #endregion 
                        #endregion
                        //sgItem.Add(new KeyValuePair<string, string>("Graphics", Convert.ToString(item[CompassListFields.????????])));
                        DateTime RevisedFirstShipDate = Convert.ToDateTime(item[CompassListFields.RevisedFirstShipDate]);
                        string month1 = "Month 1", month2 = "Month 2", month3 = "Month 3";
                        if (RevisedFirstShipDate != null || RevisedFirstShipDate.ToString() != "")
                        {
                            string[] months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

                            int selectedMonth = RevisedFirstShipDate.Month - 1;
                            month1 = months[selectedMonth];
                            if (selectedMonth + 1 > 11)
                                selectedMonth = -1;
                            month2 = months[selectedMonth + 1];
                            if (selectedMonth + 2 > 11)
                                selectedMonth = -2;
                            month3 = months[selectedMonth + 2];
                        }
                        #region Item Proposal Details
                        //Item Proposal Details
                        sgItem.Add(new KeyValuePair<string, string>("Item Proposal Details", "header"));

                        sgItem.Add(new KeyValuePair<string, string>("Network Move?", IsProjectNetworkMove(item)));

                        sgItem.Add(new KeyValuePair<string, string>("Project Type", Convert.ToString(item[CompassListFields.ProjectType])));
                        sgItem.Add(new KeyValuePair<string, string>("Project Type SubCategory", Convert.ToString(item[CompassListFields.ProjectTypeSubCategory])));

                        if (Convert.ToString(item[CompassListFields.ProjectType]) == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
                        {
                            sgItem.Add(new KeyValuePair<string, string>("Do you want to copy forms from previous project?", Convert.ToString(item2[CompassList2Fields.CopyFormsForGraphicsProject])));
                            sgItem.Add(new KeyValuePair<string, string>("Is this item external/contain external semis?", Convert.ToString(item2[CompassList2Fields.ExternalSemisItem])));
                        }

                        sgItem.Add(new KeyValuePair<string, string>("Revised First Ship Date", RevisedFirstShipDate.ToString("MM/dd/yyyy")));
                        sgItem.Add(new KeyValuePair<string, string>("Item Concept", Convert.ToString(item[CompassListFields.ItemConcept])));

                        sgItem.Add(new KeyValuePair<string, string>("Is a New FG Item # Being Used?", Convert.ToString(item[CompassListFields.TBDIndicator])));
                        sgItem.Add(new KeyValuePair<string, string>("Finished Good Item #", Convert.ToString(item[CompassListFields.SAPItemNumber])));
                        sgItem.Add(new KeyValuePair<string, string>("Finished Good Item Description", Convert.ToString(item[CompassListFields.SAPDescription])));
                        sgItem.Add(new KeyValuePair<string, string>("\"Like\" Finished Good Item #", Convert.ToString(item[CompassListFields.LikeFGItemNumber])));
                        sgItem.Add(new KeyValuePair<string, string>("\"Like\" Finished Good Item Description", Convert.ToString(item[CompassListFields.LikeFGItemDescription])));
                        sgItem.Add(new KeyValuePair<string, string>("Old Finished Good Item #", Convert.ToString(item[CompassListFields.OldFGItemNumber])));
                        sgItem.Add(new KeyValuePair<string, string>("Old Finished Good Item Description", Convert.ToString(item[CompassListFields.OldFGItemDescription])));

                        sgItem.Add(new KeyValuePair<string, string>("New Formula?", Convert.ToString(item[CompassListFields.NewFormula])));
                        sgItem.Add(new KeyValuePair<string, string>("Organic?", Convert.ToString(item[CompassListFields.Organic])));

                        if (Convert.ToString(item[CompassListFields.ProjectType]) != GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
                        {
                            sgItem.Add(new KeyValuePair<string, string>("List Price B1 per Selling Unit ", formatText(Convert.ToString(item[CompassListFields.TruckLoadPricePerSellingUnit]), "money")));
                            sgItem.Add(new KeyValuePair<string, string>("FCC Expected Gross Margin %", formatText(Convert.ToString(item[CompassListFields.ExpectedGrossMarginPercent]), "decimal")));
                            sgItem.Add(new KeyValuePair<string, string>("12 Month Projected $", formatText(Convert.ToString(item[CompassListFields.AnnualProjectedDollars]), "money")));
                            sgItem.Add(new KeyValuePair<string, string>("Annual Projected Retail Selling Units", formatText(Convert.ToString(item[CompassListFields.AnnualProjectedUnits]), "number")));
                            sgItem.Add(new KeyValuePair<string, string>(month1 + " Projected $", formatText(Convert.ToString(item[CompassListFields.Month1ProjectedDollars]), "money")));
                            sgItem.Add(new KeyValuePair<string, string>(month1 + " Projected Retail Selling Units", formatText(Convert.ToString(item[CompassListFields.Month1ProjectedUnits]), "number")));

                            sgItem.Add(new KeyValuePair<string, string>(month2 + " Projected $", formatText(Convert.ToString(item[CompassListFields.Month2ProjectedDollars]), "money")));
                            sgItem.Add(new KeyValuePair<string, string>(month2 + " Projected Retail Selling Units", formatText(Convert.ToString(item[CompassListFields.Month2ProjectedUnits]), "number")));
                            sgItem.Add(new KeyValuePair<string, string>(month3 + " Projected $", formatText(Convert.ToString(item[CompassListFields.Month3ProjectedDollars]), "money")));
                            sgItem.Add(new KeyValuePair<string, string>(month3 + " Projected Retail Selling Units", formatText(Convert.ToString(item[CompassListFields.Month3ProjectedUnits]), "number")));
                        }
                        string specific = Convert.ToString(item[CompassListFields.CustomerSpecific]);
                        sgItem.Add(new KeyValuePair<string, string>("Customer/Channel Specific", specific));
                        if (specific == "Customer Specific")
                        {
                            sgItem.Add(new KeyValuePair<string, string>("Customer", Convert.ToString(item[CompassListFields.Customer])));
                            sgItem.Add(new KeyValuePair<string, string>("Customer/Specific Lot Code", Convert.ToString(item[CompassListFields.CustomerSpecificLotCode])));
                        }
                        else if (specific == "Channel Specific")
                        {
                            sgItem.Add(new KeyValuePair<string, string>("Channel", Convert.ToString(item[CompassListFields.Channel])));
                        }
                        string soldoutsideofusa = Convert.ToString(item[CompassListFields.SoldOutsideUSA]);
                        sgItem.Add(new KeyValuePair<string, string>("Sold outside of USA?", soldoutsideofusa));
                        if (soldoutsideofusa == "Yes")
                        {
                            sgItem.Add(new KeyValuePair<string, string>("Country of Sale", Convert.ToString(item[CompassListFields.CountryOfSale])));
                        }

                        sgItem.Add(new KeyValuePair<string, string>("Product Hierarchy Level 1", Convert.ToString(item[CompassListFields.ProductHierarchyLevel1])));

                        var SAPNomenclature = false;
                        var CoMan = false;

                        if (Convert.ToString(item[CompassListFields.NewIPF]) == "Yes" && Convert.ToString(item[CompassListFields.TBDIndicator]) == "Yes")
                        {
                            SAPNomenclature = true;
                            if (Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]) == GlobalConstants.PRODUCT_HIERARCHY1_CoMan)
                            {
                                CoMan = true;
                                if (Convert.ToString(item[CompassListFields.ManuallyCreateSAPDescription]) != "No")
                                {
                                    SAPNomenclature = false;
                                }
                            }
                        }

                        if (CoMan)
                        {
                            sgItem.Add(new KeyValuePair<string, string>("Manually Create SAP Description:", Convert.ToString(item[CompassListFields.ManuallyCreateSAPDescription])));
                        }

                        sgItem.Add(new KeyValuePair<string, string>("Product Hierarchy Level 2", Convert.ToString(item[CompassListFields.ProductHierarchyLevel2])));
                        sgItem.Add(new KeyValuePair<string, string>("Material Group 1 (Brand)", Convert.ToString(item[CompassListFields.MaterialGroup1Brand])));
                        sgItem.Add(new KeyValuePair<string, string>("Profit Center)", Convert.ToString(item[CompassListFields.ProfitCenter])));
                        sgItem.Add(new KeyValuePair<string, string>("Material Group 4 (Product Form)", Convert.ToString(item[CompassListFields.MaterialGroup4ProductForm])));
                        sgItem.Add(new KeyValuePair<string, string>("Material Group 5 (Pack Type)", Convert.ToString(item[CompassListFields.MaterialGroup5PackType])));
                        if (SAPNomenclature)
                        {
                            sgItem.Add(new KeyValuePair<string, string>("Product Form Description", Convert.ToString(item[CompassListFields.ProductFormDescription])));
                        }
                        sgItem.Add(new KeyValuePair<string, string>("Is this project considered to be Novelty?", Convert.ToString(item[CompassListFields.NoveltyProject])));

                        string newUPCUCC = Convert.ToString(item[CompassListFields.RequireNewUPCUCC]);
                        sgItem.Add(new KeyValuePair<string, string>("Do we need any new UPCs (12 digit GTIN) or UCCs (14 digit GTIN)?", newUPCUCC));
                        if (newUPCUCC == "Yes")
                        {
                            sgItem.Add(new KeyValuePair<string, string>("Do we need a New Consumer Unit UPC (12 digit GTIN) – EACH?", Convert.ToString(item[CompassListFields.RequireNewUnitUPC])));
                            sgItem.Add(new KeyValuePair<string, string>("Existing Consumer Unit UPC (12 digit GTIN) – EACH", Convert.ToString(item[CompassListFields.UnitUPC])));/////??????????????? Confirm Scenario
                            sgItem.Add(new KeyValuePair<string, string>("Do we need a New Consumer Unit UPC (12 digit GTIN) – Display/Carton/Display Tray?", Convert.ToString(item[CompassListFields.RequireNewDisplayBoxUPC])));
                            sgItem.Add(new KeyValuePair<string, string>("Existing Consumer Unit UPC (12 digit GTIN) – Display/Carton/Display Tray", Convert.ToString(item[CompassListFields.DisplayBoxUPC])));/////??????????????? Confirm Scenario
                        }
                        else if (newUPCUCC == "No")
                        {
                            sgItem.Add(new KeyValuePair<string, string>("Existing Consumer Unit UPC (12 digit GTIN) – EACH", Convert.ToString(item[CompassListFields.UnitUPC])));
                            sgItem.Add(new KeyValuePair<string, string>("Existing Consumer Unit UPC (12 digit GTIN) – Display/Carton/Display Tray", Convert.ToString(item[CompassListFields.DisplayBoxUPC])));
                        }
                        string sapbaseuom = Convert.ToString(item[CompassListFields.SAPBaseUOM]);
                        sgItem.Add(new KeyValuePair<string, string>("SAP Base UOM", sapbaseuom));
                        if (sapbaseuom == "PAL")
                        {
                            sgItem.Add(new KeyValuePair<string, string>("Do we need a new Pallet UCC (14 digit GTIN)?", Convert.ToString(item[CompassListFields.RequireNewPalletUCC])));
                            sgItem.Add(new KeyValuePair<string, string>("Existing Pallet UCC (14 digit GTIN)", Convert.ToString(item[CompassListFields.PalletUCC])));/////??????????????? Confirm Scenario
                        }
                        else if (sapbaseuom == "CS")
                        {
                            sgItem.Add(new KeyValuePair<string, string>("Do we need a new Case UCC (14 digit GTIN)?", Convert.ToString(item[CompassListFields.RequireNewCaseUCC])));
                            sgItem.Add(new KeyValuePair<string, string>("Existing Case UCC (14 digit GTIN)", Convert.ToString(item[CompassListFields.CaseUCC])));/////??????????????? Confirm Scenario
                        }

                        //sgItem.Add(new KeyValuePair<string, string>("Candy SEMI #", Convert.ToString(item[CompassListFields.CandySemiNumber])));
                        sgItem.Add(new KeyValuePair<string, string>("CaseType", Convert.ToString(item[CompassListFields.CaseType])));
                        sgItem.Add(new KeyValuePair<string, string>("Film Substrate", Convert.ToString(item[CompassListFields.FilmSubstrate])));
                        sgItem.Add(new KeyValuePair<string, string>("Peg Hole Needed?", Convert.ToString(item[CompassListFields.PegHoleNeeded])));
                        if (SAPNomenclature)
                        {
                            sgItem.Add(new KeyValuePair<string, string>("Does this project involve a carton/mixed bag/tray or other product form that goes within a case but also contains individual units within it?", Convert.ToString(item[CompassListFields.InvolvesCarton])));
                            if (Convert.ToString(item[CompassListFields.InvolvesCarton]) == "Yes")
                            {
                                sgItem.Add(new KeyValuePair<string, string>("Number of Units Inside of Carton/Mixed Bag/Tray/Etc", Convert.ToString(item[CompassListFields.UnitsInsideCarton])));
                                sgItem.Add(new KeyValuePair<string, string>("Individual Pouch Weight (oz)", Convert.ToString(item[CompassListFields.IndividualPouchWeight])));
                                sgItem.Add(new KeyValuePair<string, string>("Number of Trays Per Base UOM", Convert.ToString(item[CompassListFields.NumberofTraysPerBaseUOM])));
                            }
                        }
                        sgItem.Add(new KeyValuePair<string, string>("Retail Selling Units Per Base UOM", formatText(Convert.ToString(item[CompassListFields.RetailSellingUnitsBaseUOM]), "decimal")));
                        sgItem.Add(new KeyValuePair<string, string>("Retail Unit Weight (oz)", formatText(Convert.ToString(item[CompassListFields.RetailUnitWieghtOz]), "decimal")));
                        sgItem.Add(new KeyValuePair<string, string>("Base UOM Net Weight (lbs)", formatText(Convert.ToString(item[CompassListFields.BaseUOMNetWeightLbs]), "decimal")));
                        sgItem.Add(new KeyValuePair<string, string>("Project Notes", Convert.ToString(item[CompassListFields.ProjectNotes])));
                        #endregion
                        #region Marketing Claims
                        sgItem.Add(new KeyValuePair<string, string>("Marketing Claims", "subHead"));
                        sgItem.Add(new KeyValuePair<string, string>("Marketing Claims/Labeling Requirements", Convert.ToString(item[CompassListFields.MarketClaimsLabelingRequirements])));
                        if (ClaimsItem.Count > 0)
                        {
                            SPListItem ClaimItem = ClaimsItem[0];

                            sgItem.Add(new KeyValuePair<string, string>("Sellable Unit", Convert.ToString(ClaimItem[MarketingClaimsListFields.SellableUnit])));
                            sgItem.Add(new KeyValuePair<string, string>("Are there any desired claims", Convert.ToString(ClaimItem[MarketingClaimsListFields.ClaimsDesired])));
                            sgItem.Add(new KeyValuePair<string, string>("New NLEA Format", Convert.ToString(ClaimItem[MarketingClaimsListFields.NewNLEAFormat])));
                            sgItem.Add(new KeyValuePair<string, string>("Is Bio-Engineering Labeling Acceptable for this item?", Convert.ToString(ClaimItem[MarketingClaimsListFields.BioEngLabelingAcceptable])));
                            sgItem.Add(new KeyValuePair<string, string>("Made In USA Claim", Convert.ToString(ClaimItem[MarketingClaimsListFields.MadeInUSAClaim])));
                            sgItem.Add(new KeyValuePair<string, string>("Made In USA Claim Details", Convert.ToString(ClaimItem[MarketingClaimsListFields.MadeInUSAClaimDets])));
                            sgItem.Add(new KeyValuePair<string, string>("Organic", Convert.ToString(ClaimItem[MarketingClaimsListFields.Organic])));
                            sgItem.Add(new KeyValuePair<string, string>("GMO Claim", Convert.ToString(ClaimItem[MarketingClaimsListFields.GMOClaim])));
                            sgItem.Add(new KeyValuePair<string, string>("Gluten Free", Convert.ToString(ClaimItem[MarketingClaimsListFields.GlutenFree])));
                            sgItem.Add(new KeyValuePair<string, string>("Fat Free", Convert.ToString(ClaimItem[MarketingClaimsListFields.FatFree])));
                            sgItem.Add(new KeyValuePair<string, string>("Kosher", Convert.ToString(ClaimItem[MarketingClaimsListFields.Kosher])));
                            sgItem.Add(new KeyValuePair<string, string>("Natural Colors", Convert.ToString(ClaimItem[MarketingClaimsListFields.NaturalColors])));
                            sgItem.Add(new KeyValuePair<string, string>("Natural / Real Flavors Claims", Convert.ToString(ClaimItem[MarketingClaimsListFields.NaturalFlavors])));
                            sgItem.Add(new KeyValuePair<string, string>("Preservative Free", Convert.ToString(ClaimItem[MarketingClaimsListFields.PreservativeFree])));
                            sgItem.Add(new KeyValuePair<string, string>("Lactose Free", Convert.ToString(ClaimItem[MarketingClaimsListFields.LactoseFree])));
                            sgItem.Add(new KeyValuePair<string, string>("Juice Concentrate", Convert.ToString(ClaimItem[MarketingClaimsListFields.JuiceConcentrate])));
                            sgItem.Add(new KeyValuePair<string, string>("Low Sodium", Convert.ToString(ClaimItem[MarketingClaimsListFields.LowSodium])));
                            sgItem.Add(new KeyValuePair<string, string>("Good / Excellent Source", Convert.ToString(ClaimItem[MarketingClaimsListFields.GoodSource])));
                            //sgItem.Add(new KeyValuePair<string, string>("Do any ingredients within this item need to claim Bio-Engineering?", Convert.ToString(ClaimItem[MarketingClaimsListFields.ClaimBioEngineering])));
                        }
                        #endregion
                        #region PM Initial Review
                        sgItem.Add(new KeyValuePair<string, string>("PM Initial Review", "header"));

                        if (ColItem.Count > 0)
                        {
                            SPListItem decisionItem = ColItem[0];
                            if (decisionItem != null)
                            {
                                string decision = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SrOBMApproval_Decision]);
                                sgItem.Add(new KeyValuePair<string, string>("Is the Project Approved?", decision));
                                if (decision == "Approved")
                                {
                                    string costDec = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SrOBMApproval_CostingDecision]);
                                    string capDec = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SrOBMApproval_CapacityDecision]);
                                    sgItem.Add(new KeyValuePair<string, string>("Request Initial Costing Review?", costDec));
                                    if (costDec == "Yes")
                                    {
                                        sgItem.Add(new KeyValuePair<string, string>("Costing Review Comments", Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SrOBMApproval_CostingReviewComments])));
                                    }
                                    sgItem.Add(new KeyValuePair<string, string>("Request Initial Capacity Review?", capDec));
                                    if (capDec == "Yes")
                                    {
                                        sgItem.Add(new KeyValuePair<string, string>("Capacity Review Comments", Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SrOBMApproval_CapacityReviewComments])));
                                    }
                                }
                                else if (decision == "Request IPF Update")
                                {
                                    sgItem.Add(new KeyValuePair<string, string>("Requested Update", Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SrOBMApproval_Comments])));
                                }
                                else if (decision == "Rejected")
                                {
                                    sgItem.Add(new KeyValuePair<string, string>("Reason for Rejection", Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SrOBMApproval_Comments])));
                                }
                                sgItem.Add(new KeyValuePair<string, string>("Initial Time Table", Convert.ToString(item[CompassListFields.TimelineType])));
                                sgItem.Add(new KeyValuePair<string, string>("Do you anticipate needing an Expedited Workflow with SGS?", Convert.ToString(item2[CompassList2Fields.NeedSExpeditedWorkflowWithSGS])));

                                //PACKTRIAL
                                //sgItem.Add(new KeyValuePair<string, string>("Is a pack trial needed?", Convert.ToString(item[CompassListFields.pac])));
                                //PACKTRIAL

                                //Trade Promo Group
                                int result = hideCurrentRow.Where(a => a.Key == "PTradePromo" && a.Value != "Graphics Changes/Internal Adjustments").Count();
                                if (result > 0)
                                {
                                    sgItem.Add(new KeyValuePair<string, string>("Trade Promo Group", "header"));
                                    sgItem.Add(new KeyValuePair<string, string>("Material Group 2 (Trade Promo Group)", Convert.ToString(item[CompassListFields.MaterialGroup2Pricing])));

                                    sgItem.Add(new KeyValuePair<string, string>("Initial Pricing", "header"));
                                    sgItem.Add(new KeyValuePair<string, string>("Initial Pricing", Convert.ToString(item2[CompassList2Fields.InitialEstimatedPricing])));

                                    sgItem.Add(new KeyValuePair<string, string>("Estimated Bracket Pricing", "header"));
                                    sgItem.Add(new KeyValuePair<string, string>("Initial Bracket Pricing", Convert.ToString(item2[CompassList2Fields.InitialEstimatedBracketPricing])));
                                }
                                //Primary Distribution Center
                                result = hideCurrentRow.Where(a => a.Key == "Distribution" && a.Value == "Yes").Count();
                                int resultP = hideCurrentRow.Where(a => a.Key == "PDistribution" && a.Value != "Graphics Changes/Internal Adjustments").Count();
                                if (resultP > 0 && result > 0 && item2 != null)
                                {
                                    sgItem.Add(new KeyValuePair<string, string>("Distribution", "header"));
                                    sgItem.Add(new KeyValuePair<string, string>("Designate HUB DC (aka Material: Delivery Plant)", Convert.ToString(item2[CompassList2Fields.DesignateHUBDC])));
                                    sgItem.Add(new KeyValuePair<string, string>("What is the Deployment Mode of Item?", Convert.ToString(item2[CompassList2Fields.DeploymentModeofItem])));
                                    #region SELLDCs
                                    if (Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]) != GlobalConstants.PRODUCT_HIERARCHY1_LBB)
                                    {
                                        sgItem.Add(new KeyValuePair<string, string>("Extend to SL07 (Dallas)", Convert.ToString(item2[CompassList2Fields.ExtendtoSL07])));
                                        if (Convert.ToString(item2[CompassList2Fields.ExtendtoSL13]) == "Yes")
                                        {
                                            sgItem.Add(new KeyValuePair<string, string>("Set SL07 (Dallas) SPK to", Convert.ToString(item2[CompassList2Fields.SetSL07SPKto])));
                                        }

                                        sgItem.Add(new KeyValuePair<string, string>("Extend to SL13 (Bolingbrook)", Convert.ToString(item2[CompassList2Fields.ExtendtoSL13])));
                                        if (Convert.ToString(item2[CompassList2Fields.ExtendtoSL13]) == "Yes")
                                        {
                                            sgItem.Add(new KeyValuePair<string, string>("Set SL13 (Bolingbrook) SPK to", Convert.ToString(item2[CompassList2Fields.SetSL13SPKto])));
                                        }

                                        sgItem.Add(new KeyValuePair<string, string>("Extend to SL18 (Jonestown)", Convert.ToString(item2[CompassList2Fields.ExtendtoSL18])));
                                        if (Convert.ToString(item2[CompassList2Fields.ExtendtoSL18]) == "Yes")
                                        {
                                            sgItem.Add(new KeyValuePair<string, string>("Set SL18 (Jonestown) SPK to", Convert.ToString(item2[CompassList2Fields.SetSL18SPKto])));
                                        }

                                        sgItem.Add(new KeyValuePair<string, string>("Extend to SL19 (Phoenix)", Convert.ToString(item2[CompassList2Fields.ExtendtoSL19])));
                                        if (Convert.ToString(item2[CompassList2Fields.ExtendtoSL19]) == "Yes")
                                        {
                                            sgItem.Add(new KeyValuePair<string, string>("Set SL19 (Phoenix) SPK to", Convert.ToString(item2[CompassList2Fields.SetSL19SPKto])));
                                        }

                                        sgItem.Add(new KeyValuePair<string, string>("Extend to SL30 (Atlanta)", Convert.ToString(item2[CompassList2Fields.ExtendtoSL30])));
                                        if (Convert.ToString(item2[CompassList2Fields.ExtendtoSL30]) == "Yes")
                                        {
                                            sgItem.Add(new KeyValuePair<string, string>("Set SL30 (Atlanta) SPK to", Convert.ToString(item2[CompassList2Fields.SetSL30SPKto])));
                                        }

                                        sgItem.Add(new KeyValuePair<string, string>("Extend to SL41 (DeKalb)", Convert.ToString(item2[CompassList2Fields.ExtendtoSL14])));
                                        if (Convert.ToString(item2[CompassList2Fields.ExtendtoSL14]) == "Yes")
                                        {
                                            sgItem.Add(new KeyValuePair<string, string>("Set SL41 (DeKalb) SPK to", Convert.ToString(item2[CompassList2Fields.SetSL14SPKto])));
                                        }

                                    }
                                    #endregion
                                    #region FERQDCs
                                    else
                                    {
                                        sgItem.Add(new KeyValuePair<string, string>("Extend to FQ26 (Louisville)", Convert.ToString(item2[CompassList2Fields.ExtendtoFQ26])));
                                        if (Convert.ToString(item2[CompassList2Fields.ExtendtoFQ26]) == "Yes")
                                        {
                                            sgItem.Add(new KeyValuePair<string, string>("Set FQ26 (Louisville) SPK to", Convert.ToString(item2[CompassList2Fields.SetFQ26SPKto])));
                                        }

                                        sgItem.Add(new KeyValuePair<string, string>("Extend to FQ27 (Louisville)", Convert.ToString(item2[CompassList2Fields.ExtendtoFQ27])));
                                        if (Convert.ToString(item2[CompassList2Fields.ExtendtoFQ27]) == "Yes")
                                        {
                                            sgItem.Add(new KeyValuePair<string, string>("Set FQ27 (Louisville) SPK to", Convert.ToString(item2[CompassList2Fields.SetFQ27SPKto])));
                                        }

                                        sgItem.Add(new KeyValuePair<string, string>("Extend to FQ28 (Triways)", Convert.ToString(item2[CompassList2Fields.ExtendtoFQ28])));
                                        if (Convert.ToString(item2[CompassList2Fields.ExtendtoFQ28]) == "Yes")
                                        {
                                            sgItem.Add(new KeyValuePair<string, string>("Set FQ28 (Triways) SPK to", Convert.ToString(item2[CompassList2Fields.SetFQ28SPKto])));
                                        }

                                        sgItem.Add(new KeyValuePair<string, string>("Extend to FQ29 (Digital Orders/Evans)", Convert.ToString(item2[CompassList2Fields.ExtendtoFQ29])));
                                        if (Convert.ToString(item2[CompassList2Fields.ExtendtoFQ29]) == "Yes")
                                        {
                                            sgItem.Add(new KeyValuePair<string, string>("Set FQ29 (Digital Orders/Evans) SPK to", Convert.ToString(item2[CompassList2Fields.SetFQ29SPKto])));
                                        }

                                        sgItem.Add(new KeyValuePair<string, string>("Extend to FQ34 (Port Logistics)", Convert.ToString(item2[CompassList2Fields.ExtendtoFQ34])));
                                        if (Convert.ToString(item2[CompassList2Fields.ExtendtoFQ34]) == "Yes")
                                        {
                                            sgItem.Add(new KeyValuePair<string, string>("Set FQ34 (Port Logistics) SPK to", Convert.ToString(item2[CompassList2Fields.SetFQ34SPKto])));
                                        }

                                        sgItem.Add(new KeyValuePair<string, string>("Extend to FQ35 (Advance)", Convert.ToString(item2[CompassList2Fields.ExtendtoFQ35])));
                                        if (Convert.ToString(item2[CompassList2Fields.ExtendtoFQ35]) == "Yes")
                                        {
                                            sgItem.Add(new KeyValuePair<string, string>("Set FQ35 (Advance) SPK to", Convert.ToString(item2[CompassList2Fields.SetFQ35SPKto])));
                                        }
                                    }
                                }
                                #endregion
                                #region Operations & Initial Capacity Review
                                //Operations & Initial Capacity Review
                                sgItem.Add(new KeyValuePair<string, string>("Operations & Initial Capacity Review", "header"));
                                sgItem.Add(new KeyValuePair<string, string>("Make Location", Convert.ToString(item[CompassListFields.ManufacturingLocation])));
                                sgItem.Add(new KeyValuePair<string, string>("Make Country of Origin", Convert.ToString(item[CompassListFields.ManufacturerCountryOfOrigin])));
                                sgItem.Add(new KeyValuePair<string, string>("Finished Good Pack Location", Convert.ToString(item[CompassListFields.PackingLocation])));
                                sgItem.Add(new KeyValuePair<string, string>("Manufacturing Location Change (Network Move)", Convert.ToString(item[CompassListFields.MfgLocationChange])));
                                if (Convert.ToString(item[CompassListFields.MfgLocationChange]) == "Yes")
                                {
                                    sgItem.Add(new KeyValuePair<string, string>("What Network Move is Required?", Convert.ToString(item2[CompassList2Fields.WhatNetworkMoveIsRequired])));
                                }
                                sgItem.Add(new KeyValuePair<string, string>("Immediate SPK Change", Convert.ToString(item[CompassListFields.ImmediateSPKChange])));
                                sgItem.Add(new KeyValuePair<string, string>("Line/Workcenter Additional Info", Convert.ToString(item[CompassListFields.WorkCenterAdditionalInfo])));
                                if (Convert.ToString(item[CompassListFields.ProjectType]) == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
                                {
                                    sgItem.Add(new KeyValuePair<string, string>("Project Approved?", Convert.ToString(item2[CompassList2Fields.ProjectApproved])));
                                    if (Convert.ToString(item2[CompassList2Fields.ProjectApproved]) == "Yes")
                                    {
                                        sgItem.Add(new KeyValuePair<string, string>("Reason for rejection", Convert.ToString(item2[CompassList2Fields.ReasonForRejection])));
                                    }
                                }
                                //string newTranSemi = Convert.ToString(item[CompassListFields.InternalTransferSemiNeeded]);
                                //sgItem.Add(new KeyValuePair<string, string>("Are Internal Transfer SEMI #s Needed?", newTranSemi));
                                //if (newTranSemi == "Yes")
                                //{
                                //    sgItem.Add(new KeyValuePair<string, string>("'Like' Finished Good Item #", Convert.ToString(item[CompassListFields.LikeFGItemNumber])));
                                //    sgItem.Add(new KeyValuePair<string, string>("'Like' Finished Good Item Description", Convert.ToString(item[CompassListFields.LikeFGItemDescription])));

                                //}
                                DateTime FirstProductionDate = Convert.ToDateTime(item[CompassListFields.FirstProductionDate]);
                                sgItem.Add(new KeyValuePair<string, string>("Recommended First Production Date", (FirstProductionDate == DateTime.MinValue) ? string.Empty : FirstProductionDate.ToString("MM/dd/yyyy")));
                                sgItem.Add(new KeyValuePair<string, string>("Will there be any capacity issues at the Make Location?", Convert.ToString(decisionItem[CompassProjectDecisionsListFields.InitialCapacity_MakeIssues])));
                                sgItem.Add(new KeyValuePair<string, string>("Will there be any capacity issues at the Pack Location?", Convert.ToString(decisionItem[CompassProjectDecisionsListFields.InitialCapacity_PackIssues])));
                                sgItem.Add(new KeyValuePair<string, string>("Comments on Capacity/Risk", Convert.ToString(decisionItem[CompassProjectDecisionsListFields.InitialCapacity_CapacityRiskComments])));
                                sgItem.Add(new KeyValuePair<string, string>("Recommendations on Project Acceptance", Convert.ToString(decisionItem[CompassProjectDecisionsListFields.InitialCapacity_Decision])));
                                sgItem.Add(new KeyValuePair<string, string>("Comments on Project Acceptance", Convert.ToString(decisionItem[CompassProjectDecisionsListFields.InitialCapacity_AcceptanceComments])));

                                result = hideCurrentRow.Where(a => a.Key == "ExternalMfgML" && a.Value == "Externally Manufactured").Count();
                                int resultEMl = hideCurrentRow.Where(a => a.Key == "ExternalMfgPL" && a.Value == "Externally Packed").Count();
                                int resultEPl = hideCurrentRow.Where(a => a.Key == "ExternalMfgSPL" && a.Value == "Externally Packed").Count();
                                if (resultEMl > 0 || result > 0 || resultEPl > 0)
                                {
                                    sgItem.Add(new KeyValuePair<string, string>("External Manufacturing", "header"));
                                    string ExternalMfgProjectLead = Convert.ToString(item[CompassListFields.ExternalMfgProjectLead]);
                                    index = ExternalMfgProjectLead.LastIndexOf("#");
                                    cleanPath = (index < 0)
                                        ? ExternalMfgProjectLead
                                        : ExternalMfgProjectLead.Substring(index + 1);
                                    sgItem.Add(new KeyValuePair<string, string>("External Manufacturing Lead", cleanPath));
                                    string procType = Convert.ToString(item[CompassListFields.CoManufacturingClassification]);
                                    sgItem.Add(new KeyValuePair<string, string>("Procurement Type", procType));
                                    if (procType == "External Turnkey Semi")
                                    {
                                        string inhouse = Convert.ToString(item[CompassListFields.DoesBulkSemiExistToBringInHouse]);
                                        sgItem.Add(new KeyValuePair<string, string>("Does Bulk Semi Exist to Bring In-House?", inhouse));
                                        if (inhouse == "Yes")
                                        {
                                            sgItem.Add(new KeyValuePair<string, string>("What is the Existing Bulk Semi #?", Convert.ToString(item[CompassListFields.ExistingBulkSemiNumber])));
                                            sgItem.Add(new KeyValuePair<string, string>("Bulk Semi Description", Convert.ToString(item[CompassListFields.BulkSemiDescription])));
                                        }
                                        else if (inhouse == "No")
                                        {
                                            sgItem.Add(new KeyValuePair<string, string>("Bulk Semi Description", Convert.ToString(item[CompassListFields.BulkSemiDescription])));
                                        }
                                        sgItem.Add(new KeyValuePair<string, string>("External Manufacturer", Convert.ToString(item[CompassListFields.ExternalManufacturer])));
                                        sgItem.Add(new KeyValuePair<string, string>("Manufacturer Country of Origin", Convert.ToString(item[CompassListFields.ManufacturerCountryOfOrigin])));
                                        sgItem.Add(new KeyValuePair<string, string>("Purchased Into Location", Convert.ToString(item[CompassListFields.PurchasedIntoLocation])));

                                    }
                                    else if (procType == "External Turnkey FG")
                                    {
                                        sgItem.Add(new KeyValuePair<string, string>("External Manufacturer", Convert.ToString(item[CompassListFields.ExternalManufacturer])));
                                        sgItem.Add(new KeyValuePair<string, string>("Manufacturer Country of Origin", Convert.ToString(item[CompassListFields.ManufacturerCountryOfOrigin])));
                                        sgItem.Add(new KeyValuePair<string, string>("External Packer", Convert.ToString(item[CompassListFields.ExternalPacker])));
                                        sgItem.Add(new KeyValuePair<string, string>("Purchased Into Location", Convert.ToString(item[CompassListFields.PurchasedIntoLocation])));
                                    }
                                    else
                                    {
                                        sgItem.Add(new KeyValuePair<string, string>("External Packer", Convert.ToString(item[CompassListFields.ExternalPacker])));
                                        sgItem.Add(new KeyValuePair<string, string>("Purchased Into Location", Convert.ToString(item[CompassListFields.PurchasedIntoLocation])));
                                    }
                                    sgItem.Add(new KeyValuePair<string, string>("Is Current Timeline Acceptable?", Convert.ToString(item[CompassListFields.CurrentTimelineAcceptable])));
                                    sgItem.Add(new KeyValuePair<string, string>("Lead Time From Supplier", Convert.ToString(item[CompassListFields.LeadTimeFromSupplier])));
                                    sgItem.Add(new KeyValuePair<string, string>("Final Artwork Due to Supplier", Convert.ToString(item[CompassListFields.FinalArtworkDueToSupplier])));
                                    sgItem.Add(new KeyValuePair<string, string>("Confirm packaging supplier & dieline has stayed the same", Convert.ToString(item2[CompassList2Fields.PackSupplierAndDielineSame])));
                                    if (Convert.ToString(item2[CompassList2Fields.PackSupplierAndDielineSame]) == "Yes")
                                    {
                                        sgItem.Add(new KeyValuePair<string, string>("What change is required?", Convert.ToString(item2[CompassList2Fields.WhatChangeIsRequiredExtMfg])));
                                    }
                                }
                                #endregion
                            }
                        }
                        #endregion
                        #region SAP Initial Item Setup
                        //SAP Initial Item Setup
                        int resultN = hideCurrentRow.Where(a => a.Key == "SAPInitialSetup" && a.Value == "Yes").Count();
                        int resultPS = hideCurrentRow.Where(a => a.Key == "PSAPInitialSetup" && a.Value != "Graphics Changes/Internal Adjustments").Count();
                        if (resultPS > 0 && resultN > 0)
                        {
                            sgItem.Add(new KeyValuePair<string, string>("SAP Initial Item Setup", "header"));
                            sgItem.Add(new KeyValuePair<string, string>("SAP Item #", Convert.ToString(item[CompassListFields.SAPItemNumber])));
                            sgItem.Add(new KeyValuePair<string, string>("SAP Description", Convert.ToString(item[CompassListFields.SAPDescription])));
                            sgItem.Add(new KeyValuePair<string, string>("Unit UPC", Convert.ToString(item[CompassListFields.UnitUPC])));
                            sgItem.Add(new KeyValuePair<string, string>("Jar/Display UPC", Convert.ToString(item[CompassListFields.DisplayBoxUPC])));
                            sgItem.Add(new KeyValuePair<string, string>("Case UCC", Convert.ToString(item[CompassListFields.CaseUCC])));
                        }
                        #endregion
                        //New Transfer Semi
                        //Waiting on answers
                        #region QA - InTech Regulatory
                        //QA - InTech Regulatory
                        //sgItem.Add(new KeyValuePair<string, string>("InTech Regulatory - Regulatory Labeling Considerations", "header"));
                        //if (ClaimsItem.Count > 0)
                        //{
                        //    SPListItem ClaimItem = ClaimsItem[0];

                        //    sgItem.Add(new KeyValuePair<string, string>("Do any ingredients within this item need to claim Bio-Engineering?", Convert.ToString(ClaimItem[MarketingClaimsListFields.ClaimBioEngineering])));
                        //}

                        sgItem.Add(new KeyValuePair<string, string>("InTech Regulatory - Allergens", "header"));
                        if (ClaimsItem.Count > 0)
                        {
                            SPListItem ClaimItem = ClaimsItem[0];

                            sgItem.Add(new KeyValuePair<string, string>("Milk", Convert.ToString(ClaimItem[MarketingClaimsListFields.AllergenMilk])));
                            sgItem.Add(new KeyValuePair<string, string>("Eggs", Convert.ToString(ClaimItem[MarketingClaimsListFields.AllergenEggs])));
                            sgItem.Add(new KeyValuePair<string, string>("Peanuts", Convert.ToString(ClaimItem[MarketingClaimsListFields.AllergenPeanuts])));
                            sgItem.Add(new KeyValuePair<string, string>("Coconut", Convert.ToString(ClaimItem[MarketingClaimsListFields.AllergenCoconut])));
                            sgItem.Add(new KeyValuePair<string, string>("Almonds", Convert.ToString(ClaimItem[MarketingClaimsListFields.AllergenAlmonds])));
                            sgItem.Add(new KeyValuePair<string, string>("Soy", Convert.ToString(ClaimItem[MarketingClaimsListFields.AllergenSoy])));
                            sgItem.Add(new KeyValuePair<string, string>("Wheat", Convert.ToString(ClaimItem[MarketingClaimsListFields.AllergenWheat])));
                            sgItem.Add(new KeyValuePair<string, string>("Hazelnuts", Convert.ToString(ClaimItem[MarketingClaimsListFields.AllergenHazelNuts])));
                            sgItem.Add(new KeyValuePair<string, string>("Other Allergen(s)", Convert.ToString(ClaimItem[MarketingClaimsListFields.AllergenOther])));
                        }

                        sgItem.Add(new KeyValuePair<string, string>("Is Regulatory information correct?", Convert.ToString(item2[CompassList2Fields.IsRegulatoryinformationCorrect])));
                        if (Convert.ToString(item[CompassListFields.ProjectType]) == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
                        {
                            if (Convert.ToString(item2[CompassList2Fields.IsRegulatoryinformationCorrect]) == "No")
                            {
                                sgItem.Add(new KeyValuePair<string, string>("What Regulatory information is incorrect?", Convert.ToString(item2[CompassList2Fields.WhatRegulatoryInfoIsIncorrect])));
                            }
                            sgItem.Add(new KeyValuePair<string, string>("Do you approve this project to proceed?", Convert.ToString(item2[CompassList2Fields.DoYouApproveThisProjectToProceed])));

                        }
                        sgItem.Add(new KeyValuePair<string, string>("InTech Regulatory - Candy Semi", "header"));
                        List<KeyValuePair<string, string>> qaItems = GetQAItems(itemId, spWeb, GlobalConstants.COMPONENTTYPE_CandySemi);
                        if (qaItems.Count() > 0)
                        {
                            sgItem.AddRange(qaItems);
                        }

                        sgItem.Add(new KeyValuePair<string, string>("InTech Regulatory - Purchased Candy Semi", "header"));
                        List<KeyValuePair<string, string>> qaPurCandyItems = GetQAItems(itemId, spWeb, GlobalConstants.COMPONENTTYPE_PurchasedSemi);
                        if (qaPurCandyItems.Count() > 0)
                        {
                            sgItem.AddRange(qaPurCandyItems);
                        }
                        #endregion
                        #region PM First Review
                        //PM First Review
                        sgItem.Add(new KeyValuePair<string, string>("PM First Review", "header"));
                        string OBMFirstCheck = Convert.ToString(item[CompassListFields.OBMFirstReviewCheck]);
                        sgItem.Add(new KeyValuePair<string, string>("Is all of the information above complete?", OBMFirstCheck));
                        if (OBMFirstCheck == "No")
                        {
                            sgItem.Add(new KeyValuePair<string, string>("Which sections are not complete?", Convert.ToString(item[CompassListFields.SectionsOfConcern])));
                            sgItem.Add(new KeyValuePair<string, string>("Comments", Convert.ToString(item[CompassListFields.OBMFirstReviewComments])));
                        }
                        string OBMFirstStatusCheck = Convert.ToString(item[CompassListFields.DoesFirstShipNeedRevision]);
                        sgItem.Add(new KeyValuePair<string, string>("Does First Ship Date need to be revised?", OBMFirstStatusCheck));
                        if (OBMFirstStatusCheck == "Yes")
                        {
                            DateTime RevisedFirstShipDateN = Convert.ToDateTime(item[CompassListFields.RevisedFirstShipDate]);
                            sgItem.Add(new KeyValuePair<string, string>("Revised First Ship Date", RevisedFirstShipDateN.ToString("MM/dd/yyyy")));
                            sgItem.Add(new KeyValuePair<string, string>("Comments on First Ship Revision", Convert.ToString(item[CompassListFields.RevisedFirstShipDateComments])));
                        }
                        #endregion
                        #region Proc
                        //PM Second Review

                        #endregion
                        #region PE2
                        if (Convert.ToString(item[CompassListFields.ProjectType]) == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
                        {
                            sgItem.Add(new KeyValuePair<string, string>("Bill of Materials Creation Form - PE2", "header"));
                            sgItem.Add(new KeyValuePair<string, string>("All Aspects approved from PE Perspective?", Convert.ToString(item2[CompassList2Fields.AllAspectsApprovedFromPEPersp])));
                            if (Convert.ToString(item2[CompassList2Fields.AllAspectsApprovedFromPEPersp]) == "No")
                            {
                                sgItem.Add(new KeyValuePair<string, string>("What is incorrect?", Convert.ToString(item2[CompassList2Fields.WhatIsIncorrectPE])));
                            }
                        }
                        #endregion
                        #region PM Second Review
                        //PM Second Review
                        if (Convert.ToString(item[CompassListFields.ProjectType]) == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
                        {
                            sgItem.Add(new KeyValuePair<string, string>("PM Second Review", "header"));
                            sgItem.Add(new KeyValuePair<string, string>("SGS Expedited Workflow Approved?", Convert.ToString(item2[CompassList2Fields.SGSExpeditedWorkflowApproved])));
                        }
                        #endregion
                    }
                }
            }
            return sgItem;
        }

        private static SPListItem AddCompassList2Item(int itemId, string ProjectNumber)
        {
            // Get the current user
            SPUser currentUser = SPContext.Current.Web.CurrentUser;
            SPListItem CompassList2Item = null;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassList2Fields.CompassListItemId + "\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                        SPListItemCollection CompassList2Items = spList.GetItems(spQuery);
                        if (CompassList2Items != null && CompassList2Items.Count > 0)
                        {
                            CompassList2Item = CompassList2Items[0];
                        }
                        else
                        {
                            CompassList2Item = spList.AddItem();

                            CompassList2Item["Title"] = ProjectNumber;
                            CompassList2Item[CompassList2Fields.CompassListItemId] = itemId;
                        }
                        CompassList2Item[CompassList2Fields.ModifiedBy] = SPContext.Current.Web.CurrentUser.ToString();
                        CompassList2Item[CompassList2Fields.ModifiedDate] = DateTime.Now.ToString();
                        CompassList2Item.Update();

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });

            return CompassList2Item;
        }

        public List<KeyValuePair<string, string>> GetQAItems(int itemId, SPWeb spWeb, string CompType)
        {
            List<KeyValuePair<string, string>> sgItem = new List<KeyValuePair<string, string>>();

            SPList spQAList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq><Eq><FieldRef Name=\"PackagingComponent\" /><Value Type=\"Text\">" + CompType + "</Value></Eq></And></Where>";
            SPListItemCollection ColItem = spQAList.GetItems(spQuery);

            if (ColItem.Count > 0)
            {
                int count = 0;
                //QA Packaging Items

                foreach (SPListItem item in ColItem)
                {
                    if (Convert.ToString(item[PackagingItemListFields.Deleted]).ToLower() == "yes")
                    {
                        continue;
                    }

                    sgItem.Add(new KeyValuePair<string, string>("new", ""));
                    sgItem.Add(new KeyValuePair<string, string>("Candy Semi #", Convert.ToString(item[PackagingItemListFields.MaterialNumber])));
                    sgItem.Add(new KeyValuePair<string, string>("Candy Semi Description", Convert.ToString(item[PackagingItemListFields.MaterialDescription])));
                    sgItem.Add(new KeyValuePair<string, string>("New or Existing?", Convert.ToString(item[PackagingItemListFields.NewExisting])));
                    sgItem.Add(new KeyValuePair<string, string>("Trials Completed?", Convert.ToString(item[PackagingItemListFields.TrialsCompleted])));
                    sgItem.Add(new KeyValuePair<string, string>("New Formula?", Convert.ToString(item[PackagingItemListFields.NewFormula])));
                    sgItem.Add(new KeyValuePair<string, string>("Shelf Life", Convert.ToString(item[PackagingItemListFields.ShelfLife]) + " Days"));
                    sgItem.Add(new KeyValuePair<string, string>("Kosher", Convert.ToString(item[PackagingItemListFields.Kosher])));
                    sgItem.Add(new KeyValuePair<string, string>("Allergens", Convert.ToString(item[PackagingItemListFields.Allergens])));
                    sgItem.Add(new KeyValuePair<string, string>("Do any ingredients within this item need to claim Bio-Engineering?:", Convert.ToString(item[PackagingItemListFields.IngredientsNeedToClaimBioEng])));
                    sgItem.Add(new KeyValuePair<string, string>("", ""));
                    count++;
                }
            }
            return sgItem;
        }
        public List<KeyValuePair<string, string>> GetPackagingItem(int itemId, string PLMFlag, string ProjectType)
        {
            List<KeyValuePair<string, string>> sgItem = new List<KeyValuePair<string, string>>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spOpsList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();

                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq><Neq><FieldRef Name=\"Deleted\" /><Value Type=\"Choice\">Yes</Value></Neq></And></Where>";
                    SPListItemCollection ColItem = spOpsList.GetItems(spQuery);

                    SPList spMatsList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassPackMeasurementsListName);
                    SPQuery spMatsQuery = new SPQuery();
                    spMatsQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq><Neq><FieldRef Name=\"Deleted\" /><Value Type=\"Choice\">Yes</Value></Neq></And></Where>";
                    SPListItemCollection MatsCol = spMatsList.GetItems(spMatsQuery);

                    List<SPListItem> subList = new List<SPListItem>();
                    List<SPListItem> AllTransferSemis = new List<SPListItem>();
                    List<string> idListTS = new List<string>();
                    List<string> idListPC = new List<string>();
                    bool endFG = false;
                    #region FG Summary
                    if (ColItem != null)
                    {
                        if (ColItem.Count > 0)
                        {
                            int count = 0;
                            //Packaging Items
                            sgItem.Add(new KeyValuePair<string, string>("Finished Good BOM Summary", "header"));
                            foreach (SPListItem item in ColItem)
                            {
                                string parentID = Convert.ToString(item[PackagingItemListFields.ParentID]);
                                string packagingComponent = Convert.ToString(item[PackagingItemListFields.PackagingComponent]);
                                string newExisting = Convert.ToString(item[PackagingItemListFields.NewExisting]);
                                string id = Convert.ToString(item.ID);
                                if (parentID != "0" && parentID != "")
                                {
                                    subList.Add(item);
                                    continue;
                                }
                                if (packagingComponent == "Transfer Semi")
                                {
                                    AllTransferSemis.Add(item);
                                    idListTS.Add(string.Concat(Convert.ToString(item.ID), ",", Convert.ToString(item[PackagingItemListFields.MaterialNumber]), ",", Convert.ToString(item[PackagingItemListFields.MaterialDescription]), ",", "TS"));
                                }
                                if (packagingComponent == "Purchased Candy Semi")
                                {
                                    idListPC.Add(string.Concat(Convert.ToString(item.ID), ",", Convert.ToString(item[PackagingItemListFields.MaterialNumber]), ",", Convert.ToString(item[PackagingItemListFields.MaterialDescription]), ",", "PC"));
                                }
                                sgItem.Add(new KeyValuePair<string, string>("new", ""));
                                sgItem.Add(new KeyValuePair<string, string>("Packaging Type", Convert.ToString(item[PackagingItemListFields.PackagingComponent])));
                                sgItem.Add(new KeyValuePair<string, string>("New or Existing?", newExisting));
                                sgItem.Add(new KeyValuePair<string, string>("Pack Qty", Convert.ToString(item[PackagingItemListFields.PackQuantity])));
                                sgItem.Add(new KeyValuePair<string, string>("UOM", Convert.ToString(item[PackagingItemListFields.PackUnit])));
                                sgItem.Add(new KeyValuePair<string, string>("Graphics Required?", Convert.ToString(item[PackagingItemListFields.GraphicsChangeRequired])));
                                sgItem.Add(new KeyValuePair<string, string>("Graphics Vendor", Convert.ToString(item[PackagingItemListFields.ExternalGraphicsVendor])));
                                sgItem.Add(new KeyValuePair<string, string>("Component #", Convert.ToString(item[PackagingItemListFields.MaterialNumber])));
                                sgItem.Add(new KeyValuePair<string, string>("Component Description", Convert.ToString(item[PackagingItemListFields.MaterialDescription])));
                                sgItem.Add(new KeyValuePair<string, string>("Like/Old Component #", Convert.ToString(item[PackagingItemListFields.CurrentLikeItem])));
                                sgItem.Add(new KeyValuePair<string, string>("Like/Old Component Description", Convert.ToString(item[PackagingItemListFields.CurrentLikeItemDescription])));
                                sgItem.Add(new KeyValuePair<string, string>("How is it a Like Component #", Convert.ToString(item[PackagingItemListFields.CurrentLikeItemReason])));
                                sgItem.Add(new KeyValuePair<string, string>("Graphics Brief", Convert.ToString(item[PackagingItemListFields.GraphicsBrief])));
                                if (packagingComponent != "Purchased Candy Semi" && packagingComponent != "Transfer Semi" && !packagingComponent.ToLower().Contains("finished good"))
                                {
                                    sgItem.Add(new KeyValuePair<string, string>("Printer/Supplier", Convert.ToString(item[PackagingItemListFields.PrinterSupplier])));
                                }
                                sgItem.Add(new KeyValuePair<string, string>("Component requires consumer facing labeling?", Convert.ToString(item[PackagingItemListFields.ComponentContainsNLEA])));
                                sgItem.Add(new KeyValuePair<string, string>("Flowthrough", Convert.ToString(item[PackagingItemListFields.Flowthrough])));
                                sgItem.Add(new KeyValuePair<string, string>("Review Printer-Supplier (Proc)?", Convert.ToString(item[PackagingItemListFields.ReviewPrinterSupplier])));
                                if (packagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi || packagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi)
                                {
                                    sgItem.Add(new KeyValuePair<string, string>("Product Hierarchy Level 1", Convert.ToString(item[PackagingItemListFields.PHL1])));
                                    sgItem.Add(new KeyValuePair<string, string>("Product Hierarchy Level 2", Convert.ToString(item[PackagingItemListFields.PHL2])));
                                    sgItem.Add(new KeyValuePair<string, string>("Material Group 1 (Brand)", Convert.ToString(item[PackagingItemListFields.Brand])));
                                    sgItem.Add(new KeyValuePair<string, string>("Profit Center", Convert.ToString(item[PackagingItemListFields.ProfitCenter])));
                                }
                                if (ProjectType == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
                                {
                                    if (Convert.ToString(item[PackagingItemListFields.NewExisting]) == "Yes")
                                    {
                                        sgItem.Add(new KeyValuePair<string, string>("Is all procurement information correct?", Convert.ToString(item[PackagingItemListFields.IsAllProcInfoCorrect])));
                                        if (Convert.ToString(item[PackagingItemListFields.IsAllProcInfoCorrect]) == "No")
                                        {
                                            sgItem.Add(new KeyValuePair<string, string>("What procument information has changed?", Convert.ToString(item[PackagingItemListFields.WhatProcInfoHasChanged])));
                                        }
                                    }
                                }
                                sgItem.Add(new KeyValuePair<string, string>("UPC Associated with this Packaging Component", Convert.ToString(item[PackagingItemListFields.UPCAssociated])));
                                if (Convert.ToString(item[PackagingItemListFields.UPCAssociated]) == "Manual Entry")
                                {
                                    sgItem.Add(new KeyValuePair<string, string>("UPC Associated with this Packaging Component - Manual Entry", Convert.ToString(item[PackagingItemListFields.UPCAssociatedManualEntry])));
                                }
                                sgItem.Add(new KeyValuePair<string, string>("Is Bio-Eng. labeling required or desired on this material?", Convert.ToString(item[PackagingItemListFields.BioEngLabelingRequired])));
                                sgItem.Add(new KeyValuePair<string, string>("Flowthrough materials specs", Convert.ToString(item[PackagingItemListFields.FlowthroughMaterialsSpecs])));


                                sgItem.Add(new KeyValuePair<string, string>("", ""));
                                endFG = true;
                                count++;
                            }
                            if (MatsCol != null && PLMFlag != "Yes")
                            {
                                #region Pack Meas
                                if (MatsCol.Count > 0)
                                {
                                    foreach (SPListItem matRows in MatsCol)
                                    {
                                        string parentCompId = Convert.ToString(matRows[CompassPackMeasurementsFields.ParentComponentId]);
                                        if (parentCompId == "0" || parentCompId == "")
                                        {
                                            sgItem.Add(new KeyValuePair<string, string>("Pack Trial", "subHead"));
                                            sgItem.Add(new KeyValuePair<string, string>("Is a pack trial required? ", Convert.ToString(matRows[CompassPackMeasurementsFields.PackTrialNeeded])));

                                            sgItem.Add(new KeyValuePair<string, string>("Unit Measurements", "subHead"));
                                            sgItem.Add(new KeyValuePair<string, string>("Net Unit Weight (oz)", formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.NetUnitWeight]), "decimal")));
                                            string unitDims = formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.UnitDimensionLength]), "decimal") + " in. X " + formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.UnitDimensionWidth]), "decimal") + " in. X " + formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.UnitDimensionHeight]), "decimal") + " in.";
                                            if (unitDims == " in. X  in. X  in." || unitDims == "-9999.00 in. X -9999.00 in. X -9999.00 in.") { unitDims = ""; }
                                            sgItem.Add(new KeyValuePair<string, string>("Unit Dimensions (LXWXH)", unitDims));

                                            sgItem.Add(new KeyValuePair<string, string>("Case Measurements", "subHead"));
                                            sgItem.Add(new KeyValuePair<string, string>("Case Pack", formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.CasePack]), "number")));
                                            unitDims = formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.CaseDimensionLength]), "decimal") + " in. X " + formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.CaseDimensionWidth]), "decimal") + " in. X " + formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.CaseDimensionHeight]), "decimal") + " in.";
                                            if (unitDims == " in. X  in. X  in." || unitDims == "-9999.00 in. X -9999.00 in. X -9999.00 in.") { unitDims = ""; }
                                            sgItem.Add(new KeyValuePair<string, string>("Case Dimensions (LXWXH)", unitDims));
                                            sgItem.Add(new KeyValuePair<string, string>("Case Cube (cubic ft)", formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.CaseCube]), "decimal")));
                                            sgItem.Add(new KeyValuePair<string, string>("Case Net Weight (lbs)", formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.CaseNetWeight]), "decimal")));
                                            sgItem.Add(new KeyValuePair<string, string>("Case Gross Weight (lbs)", formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.CaseGrossWeight]), "number")));

                                            sgItem.Add(new KeyValuePair<string, string>("Pallet Measurements", "subHead"));
                                            sgItem.Add(new KeyValuePair<string, string>("Cases per Pallet", formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.CasesPerPallet]), "number")));
                                            sgItem.Add(new KeyValuePair<string, string>("Double Stackable?", Convert.ToString(matRows[CompassPackMeasurementsFields.DoubleStackable])));
                                            unitDims = formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.PalletDimensionsLength]), "decimal") + " in. X " + formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.PalletDimensionsWidth]), "decimal") + " in. X " + formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.PalletDimensionsHeight]), "decimal") + " in.";
                                            if (unitDims == " in. X  in. X  in." || unitDims == "-9999.00 in. X -9999.00 in. X -9999.00 in.") { unitDims = ""; }
                                            sgItem.Add(new KeyValuePair<string, string>("Case Dimensions (LXWXH)", unitDims));
                                            sgItem.Add(new KeyValuePair<string, string>("Pallet Cube (sq. ft)", formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.PalletCube]), "decimal")));
                                            sgItem.Add(new KeyValuePair<string, string>("Pallet Weight", formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.PalletWeight]), "number")));
                                            sgItem.Add(new KeyValuePair<string, string>("Pallet Gross Weight (lbs)", formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.PalletGrossWeight]), "decimal")));
                                            sgItem.Add(new KeyValuePair<string, string>("Cases per Layer", formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.CasesPerLayer]), "number")));
                                            sgItem.Add(new KeyValuePair<string, string>("Layers per Pallet", formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.LayersPerPallet]), "number")));

                                            sgItem.Add(new KeyValuePair<string, string>("Sales Dimensions", "subHead"));
                                            unitDims = formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.SalesUnitDimensionsLength]), "decimal") + " in. X " + formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.SalesUnitDimensionsWidth]), "decimal") + " in. X " + formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.SalesUnitDimensionsHeight]), "decimal") + " in.";
                                            if (unitDims == " in. X  in. X  in." || unitDims == "-9999.00 in. X -9999.00 in. X -9999.00 in.") { unitDims = ""; }
                                            sgItem.Add(new KeyValuePair<string, string>("Unit Dimensions (LXWXH)", unitDims));
                                            unitDims = formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.SalesCaseDimensionsLength]), "decimal") + " in. X " + formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.SalesCaseDimensionsWidth]), "decimal") + " in. X " + formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.SalesCaseDimensionsHeight]), "decimal") + " in.";
                                            if (unitDims == " in. X  in. X  in." || unitDims == "-9999.00 in. X -9999.00 in. X -9999.00 in.") { unitDims = ""; }
                                            sgItem.Add(new KeyValuePair<string, string>("Case Dimensions (LXWXH)", unitDims));

                                            sgItem.Add(new KeyValuePair<string, string>("Display Dimensions", "subHead"));
                                            unitDims = formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.DisplayDimensionsLength]), "decimal") + " in. X " + formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.DisplayDimensionsWidth]), "decimal") + " in. X " + formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.DisplayDimensionsHeight]), "decimal") + " in.";
                                            if (unitDims == " in. X  in. X  in." || unitDims == "-9999.00 in. X -9999.00 in. X -9999.00 in.") { unitDims = ""; }
                                            sgItem.Add(new KeyValuePair<string, string>("Display Dimensions (LXWXH)", unitDims));
                                            unitDims = formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.SetUpDimensionsLength]), "decimal") + " in. X " + formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.SetUpDimensionsWidth]), "decimal") + " in. X " + formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.SetUpDimensionsHeight]), "decimal") + " in.";
                                            if (unitDims == " in. X  in. X  in." || unitDims == "-9999.00 in. X -9999.00 in. X -9999.00 in.") { unitDims = ""; }
                                            sgItem.Add(new KeyValuePair<string, string>("Set Up Dimensions (LXWXH)", unitDims));
                                            sgItem.Add(new KeyValuePair<string, string>("endFG", "endFG"));
                                            endFG = false;
                                            break;
                                        }
                                    }
                                }
                                #endregion
                            }
                            else if (MatsCol != null && PLMFlag == "Yes")
                            {
                                #region Pack Meas
                                if (MatsCol.Count > 0)
                                {
                                    foreach (SPListItem matRows in MatsCol)
                                    {
                                        string parentCompId = Convert.ToString(matRows[CompassPackMeasurementsFields.ParentComponentId]);
                                        if (parentCompId == "0" || parentCompId == "")
                                        {
                                            sgItem.Add(new KeyValuePair<string, string>("Finished Good Specifications", "subHead"));
                                            sgItem.Add(new KeyValuePair<string, string>("Is this a new Finished Good or is it an existing Finished Good with a change to its specification? ", Convert.ToString(matRows[CompassPackMeasurementsFields.SAPSpecsChange])));
                                            sgItem.Add(new KeyValuePair<string, string>("If this is an existing specification with changes that don't constitute a new material number, please describe those changes ", Convert.ToString(matRows[CompassPackMeasurementsFields.NotesSpec])));
                                            sgItem.Add(new KeyValuePair<string, string>("Finished Good Finished Good (Pack only) Specification Number ", Convert.ToString(matRows[CompassPackMeasurementsFields.PackSpecNumber])));
                                            sgItem.Add(new KeyValuePair<string, string>("Finished Good Pallet Specification Number ", Convert.ToString(matRows[CompassPackMeasurementsFields.PalletSpecNumber])));
                                            sgItem.Add(new KeyValuePair<string, string>("endFG", "endFG"));
                                            endFG = false;
                                            break;
                                        }
                                    }
                                }
                                #endregion

                            }

                            if (endFG)
                            {
                                sgItem.Add(new KeyValuePair<string, string>("endFG", "endFG"));
                            }
                        }
                    }
                    #endregion

                    #region TS and PCss
                    List<string> idList = new List<string>();
                    idList.AddRange(idListTS);
                    idList.AddRange(idListPC);
                    var EligibleTransferSemis = new List<SPListItem>();
                    var EligibleTransferSemi = new List<SPListItem>();

                    if (AllTransferSemis.Count > 0)
                    {
                        EligibleTransferSemis =
                            (
                                 from packgingItem in AllTransferSemis
                                 where
                                 Convert.ToString(packgingItem[PackagingItemListFields.PackagingComponent]).ToLower().Contains("transfer") &&
                                 (Convert.ToString(packgingItem[PackagingItemListFields.PackLocation]).Contains("FQ22") || Convert.ToString(packgingItem[PackagingItemListFields.PackLocation]).Contains("FQ25"))
                                 select packgingItem
                            ).ToList();
                    }

                    foreach (var item in idList)
                    {
                        string[] itemDetails = item.Split(',');
                        sgItem.Add(new KeyValuePair<string, string>("newTS", ""));
                        string Type = ((itemDetails[3] == "TS") ? "Transfer Semi" : "Purchased Candy Semi");
                        string TSHeader = ((itemDetails[3] == "TS") ? "Transfer Semi : " : "Purchased Candy Semi : ") + itemDetails[1] + " - " + itemDetails[2];
                        sgItem.Add(new KeyValuePair<string, string>(TSHeader, "header"));
                        var endTS = false;
                        foreach (SPListItem childItem in subList)
                        {
                            if (Convert.ToString(childItem[PackagingItemListFields.ParentID]) == itemDetails[0])
                            {
                                string packagingComponent = Convert.ToString(childItem[PackagingItemListFields.PackagingComponent]);
                                sgItem.Add(new KeyValuePair<string, string>("Packaging Type", Convert.ToString(childItem[PackagingItemListFields.PackagingComponent])));
                                sgItem.Add(new KeyValuePair<string, string>("New or Existing?", Convert.ToString(childItem[PackagingItemListFields.NewExisting])));
                                sgItem.Add(new KeyValuePair<string, string>("Pack Qty", Convert.ToString(childItem[PackagingItemListFields.PackQuantity])));
                                sgItem.Add(new KeyValuePair<string, string>("UOM", Convert.ToString(childItem[PackagingItemListFields.PackUnit])));
                                sgItem.Add(new KeyValuePair<string, string>("Graphics Required?", Convert.ToString(childItem[PackagingItemListFields.GraphicsChangeRequired])));
                                sgItem.Add(new KeyValuePair<string, string>("Graphics Vendor", Convert.ToString(childItem[PackagingItemListFields.ExternalGraphicsVendor])));
                                sgItem.Add(new KeyValuePair<string, string>("Component #", Convert.ToString(childItem[PackagingItemListFields.MaterialNumber])));
                                sgItem.Add(new KeyValuePair<string, string>("Component Description", Convert.ToString(childItem[PackagingItemListFields.MaterialDescription])));
                                sgItem.Add(new KeyValuePair<string, string>("Like/Old Component #", Convert.ToString(childItem[PackagingItemListFields.CurrentLikeItem])));
                                sgItem.Add(new KeyValuePair<string, string>("Like/Old Component Description", Convert.ToString(childItem[PackagingItemListFields.CurrentLikeItemDescription])));
                                sgItem.Add(new KeyValuePair<string, string>("How is it a Like Component #", Convert.ToString(childItem[PackagingItemListFields.CurrentLikeItemReason])));
                                sgItem.Add(new KeyValuePair<string, string>("Graphics Brief", Convert.ToString(childItem[PackagingItemListFields.GraphicsBrief])));
                                if (packagingComponent != "Purchased Candy Semi" && packagingComponent != "Transfer Semi" && !packagingComponent.ToLower().Contains("finished good"))
                                {
                                    sgItem.Add(new KeyValuePair<string, string>("Printer/Supplier", Convert.ToString(childItem[PackagingItemListFields.PrinterSupplier])));
                                }
                                sgItem.Add(new KeyValuePair<string, string>("Component requires consumer facing labeling?", Convert.ToString(childItem[PackagingItemListFields.ComponentContainsNLEA])));
                                sgItem.Add(new KeyValuePair<string, string>("Flowthrough", Convert.ToString(childItem[PackagingItemListFields.Flowthrough])));
                                sgItem.Add(new KeyValuePair<string, string>("Review Printer-Supplier (Proc)?", Convert.ToString(childItem[PackagingItemListFields.ReviewPrinterSupplier])));
                                if (packagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi || packagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi)
                                {
                                    sgItem.Add(new KeyValuePair<string, string>("Product Hierarchy Level 1", Convert.ToString(childItem[PackagingItemListFields.PHL1])));
                                    sgItem.Add(new KeyValuePair<string, string>("Product Hierarchy Level 2", Convert.ToString(childItem[PackagingItemListFields.PHL2])));
                                    sgItem.Add(new KeyValuePair<string, string>("Material Group 1 (Brand)", Convert.ToString(childItem[PackagingItemListFields.Brand])));
                                    sgItem.Add(new KeyValuePair<string, string>("Profit Center", Convert.ToString(childItem[PackagingItemListFields.ProfitCenter])));
                                }

                                sgItem.Add(new KeyValuePair<string, string>("UPC Associated with this Packaging Component", Convert.ToString(childItem[PackagingItemListFields.UPCAssociated])));
                                if (Convert.ToString(childItem[PackagingItemListFields.UPCAssociated]) == "Manual Entry")
                                {
                                    sgItem.Add(new KeyValuePair<string, string>("UPC Associated with this Packaging Component - Manual Entry", Convert.ToString(childItem[PackagingItemListFields.UPCAssociatedManualEntry])));
                                }
                                sgItem.Add(new KeyValuePair<string, string>("Is Bio-Eng. labeling required or desired on this material?", Convert.ToString(childItem[PackagingItemListFields.BioEngLabelingRequired])));
                                sgItem.Add(new KeyValuePair<string, string>("Flowthrough materials specs", Convert.ToString(childItem[PackagingItemListFields.FlowthroughMaterialsSpecs])));

                                try
                                {
                                    EligibleTransferSemi =
                                       (
                                           from packgingItem in EligibleTransferSemis
                                           where Convert.ToInt32(childItem[PackagingItemListFields.ParentID]) == packgingItem.ID
                                           select packgingItem
                                      ).ToList();
                                }
                                catch (Exception ex)
                                {
                                }

                                if (EligibleTransferSemi.Count > 0 && Convert.ToString(childItem[PackagingItemListFields.PackagingComponent]).ToLower().Contains("corrugated") && Convert.ToString(childItem[PackagingItemListFields.NewExisting]).ToLower() == "new")
                                {
                                    sgItem.Add(new KeyValuePair<string, string>("14 Digit Barcode", Convert.ToString(childItem[PackagingItemListFields.FourteenDigitBarcode])));
                                    sgItem.Add(new KeyValuePair<string, string>("", ""));
                                }
                            }
                            endTS = true;
                        }

                        if (MatsCol != null && PLMFlag == "Yes")
                        {
                            if (MatsCol.Count > 0)
                            {
                                foreach (SPListItem matRows in MatsCol)
                                {
                                    string parentCompId = Convert.ToString(matRows[CompassPackMeasurementsFields.ParentComponentId]);

                                    if (parentCompId != "" && parentCompId != "0")
                                    {
                                        if (Convert.ToString(matRows[CompassPackMeasurementsFields.ParentComponentId]) == itemDetails[0])
                                        {
                                            sgItem.Add(new KeyValuePair<string, string>(string.Concat(Type, " Specifications"), "subHead"));
                                            sgItem.Add(new KeyValuePair<string, string>("Is this a new Finished Good or is it an existing Finished Good with a change to its specification?", Convert.ToString(matRows[CompassPackMeasurementsFields.SAPSpecsChange])));
                                            sgItem.Add(new KeyValuePair<string, string>("If this is an existing specification with changes that don't constitute a new material number, please describe those changes", Convert.ToString(matRows[CompassPackMeasurementsFields.NotesSpec])));
                                            sgItem.Add(new KeyValuePair<string, string>(string.Concat(Type, " Finished Good (Pack only) Specification Number"), Convert.ToString(matRows[CompassPackMeasurementsFields.PackSpecNumber])));
                                            sgItem.Add(new KeyValuePair<string, string>(string.Concat(Type, " Pallet Specification Number"), Convert.ToString(matRows[CompassPackMeasurementsFields.PalletSpecNumber])));
                                            sgItem.Add(new KeyValuePair<string, string>("endTS", "endTS"));
                                            endTS = false;
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        if (endTS)
                        {
                            sgItem.Add(new KeyValuePair<string, string>("endTS", "endTS"));
                        }
                    }
                    #endregion
                }
            }
            return sgItem;
        }

        public List<KeyValuePair<string, string>> GetMasterDataItem(int itemId)
        {
            List<KeyValuePair<string, string>> sgItem = new List<KeyValuePair<string, string>>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    //Compass List Query
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem item = spList.GetItemById(itemId);

                    #region LIST_CompassList2Name
                    spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
                    SPQuery spCompassList2Query = new SPQuery();
                    spCompassList2Query.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    spCompassList2Query.RowLimit = 1;

                    SPListItemCollection compassList2ItemCol = spList.GetItems(spCompassList2Query);
                    SPListItem compassList2Item = null;
                    if (spCompassList2Query != null && compassList2ItemCol.Count > 0)
                    {
                        compassList2Item = compassList2ItemCol[0];
                    }
                    else
                    {
                        compassList2Item = AddCompassList2Item(itemId, Convert.ToString(item[CompassListFields.ProjectNumber]));
                    }
                    #endregion

                    SPList spPackList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spPackQuery = new SPQuery();
                    spPackQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq><Eq><FieldRef Name=\"PackagingComponent\" /><Value Type=\"Text\">Transfer Semi</Value></Eq></And></Where>";
                    SPListItemCollection PackCol = spPackList.GetItems(spPackQuery);
                    List<SPListItem> newTransSemiPackDets = new List<SPListItem>();
                    List<int> newTransSemiIds = new List<int>();

                    SPList spMatsList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassPackMeasurementsListName);
                    SPQuery spMatsQuery = new SPQuery();
                    spMatsQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq><Neq><FieldRef Name=\"Deleted\" /><Value Type=\"Choice\">Yes</Value></Neq></And></Where>";
                    SPListItemCollection MatsCol = spMatsList.GetItems(spMatsQuery);
                    SPListItem matRows = null;
                    List<SPListItem> newTransSemi = new List<SPListItem>();

                    if (PackCol != null && item != null)
                    {
                        string PLMFlag = Convert.ToString(item[CompassListFields.PLMProject]);
                        foreach (SPListItem tempRows in PackCol)
                        {
                            if (Convert.ToString(tempRows[PackagingItemListFields.Deleted]).ToLower() == "yes")
                            {
                                continue;
                            }
                            newTransSemiIds.Add(tempRows.ID);
                            newTransSemiPackDets.Add(tempRows);
                        }
                        if (MatsCol != null)
                        {
                            foreach (SPListItem tempRows in MatsCol)
                            {
                                int parentCompID = Convert.ToInt32(tempRows[CompassPackMeasurementsFields.ParentComponentId]);
                                if (parentCompID == 0)
                                {
                                    matRows = tempRows;
                                }
                                else if (newTransSemiIds.Contains(parentCompID))
                                {
                                    string tsMatNum = Convert.ToString((from ts in newTransSemiPackDets where ts.ID == parentCompID select ts[PackagingItemListFields.MaterialNumber]).FirstOrDefault());
                                    string tsMatDesc = Convert.ToString((from ts in newTransSemiPackDets where ts.ID == parentCompID select ts[PackagingItemListFields.MaterialDescription]).FirstOrDefault());
                                    tempRows[CompassPackMeasurementsFields.PackTrialComments] = tsMatNum;
                                    tempRows[CompassPackMeasurementsFields.PackTrialResult] = tsMatDesc;
                                    newTransSemi.Add(tempRows);
                                }
                            }
                        }
                        //Master Data Details
                        sgItem.Add(new KeyValuePair<string, string>("Master Data Details", "header"));

                        sgItem.Add(new KeyValuePair<string, string>("SAP Item #", Convert.ToString(item[CompassListFields.SAPItemNumber])));
                        sgItem.Add(new KeyValuePair<string, string>("SAP Item Description", Convert.ToString(item[CompassListFields.SAPDescription])));
                        sgItem.Add(new KeyValuePair<string, string>("\"Like\" FG Item #", Convert.ToString(item[CompassListFields.LikeFGItemNumber])));
                        sgItem.Add(new KeyValuePair<string, string>("SAP Base UofM", Convert.ToString(item[CompassListFields.SAPBaseUOM])));
                        sgItem.Add(new KeyValuePair<string, string>("Manufacturing Location", Convert.ToString(item[CompassListFields.ManufacturingLocation])));
                        sgItem.Add(new KeyValuePair<string, string>("Designate HUB DC (aka Material: Delivery Plant)", (compassList2Item == null) ? "" : Convert.ToString(compassList2Item[CompassList2Fields.DesignateHUBDC])));
                        sgItem.Add(new KeyValuePair<string, string>("Purchased Into Center", Convert.ToString(item[CompassListFields.PurchasedIntoLocation])));
                        sgItem.Add(new KeyValuePair<string, string>("Packing Location", Convert.ToString(item[CompassListFields.PackingLocation])));
                        if (matRows != null && PLMFlag != "Yes")
                        {
                            sgItem.Add(new KeyValuePair<string, string>("Case Pack", formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.CasePack]), "number")));
                            sgItem.Add(new KeyValuePair<string, string>("Net Unit Weight (oz)", formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.NetUnitWeight]), "decimal")));
                            sgItem.Add(new KeyValuePair<string, string>("Gross Case Weight (lbs)", formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.CaseGrossWeight]), "number")));
                            sgItem.Add(new KeyValuePair<string, string>("Net Case Weight (lbs)", formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.CaseNetWeight]), "decimal")));
                            string unitDims = formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.UnitDimensionLength]), "decimal") + " in. X " + formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.UnitDimensionWidth]), "decimal") + " in. X " + formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.UnitDimensionHeight]), "decimal") + " in.";
                            if (unitDims == " in. X  in. X  in." || unitDims == "-9999.00 in. X -9999.00 in. X -9999.00 in.") { unitDims = ""; }
                            sgItem.Add(new KeyValuePair<string, string>("Unit Dimensions (LXWXH)", unitDims));
                            string caseDims = formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.CaseDimensionLength]), "decimal") + " in. X " + formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.CaseDimensionWidth]), "decimal") + " in. X " + formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.CaseDimensionHeight]), "decimal") + " in.";
                            if (caseDims == " in. X  in. X  in." || caseDims == "-9999.00 in. X -9999.00 in. X -9999.00 in.") { caseDims = ""; }
                            sgItem.Add(new KeyValuePair<string, string>("Case Dimensions (LXWXH)", caseDims));
                            sgItem.Add(new KeyValuePair<string, string>("Cases/Pallet", formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.CasesPerPallet]), "number")));
                            sgItem.Add(new KeyValuePair<string, string>("Cases per Layer", formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.CasesPerLayer]), "number")));
                            sgItem.Add(new KeyValuePair<string, string>("Layers per Pallet", formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.LayersPerPallet]), "number")));
                        }
                        sgItem.Add(new KeyValuePair<string, string>("Case UCC", Convert.ToString(item[CompassListFields.CaseUCC])));
                        sgItem.Add(new KeyValuePair<string, string>("Unit UPC", Convert.ToString(item[CompassListFields.UnitUPC])));
                        sgItem.Add(new KeyValuePair<string, string>("Pallet UCC", Convert.ToString(item[CompassListFields.PalletUCC])));
                        sgItem.Add(new KeyValuePair<string, string>("Display UPC", Convert.ToString(item[CompassListFields.DisplayBoxUPC])));
                        if (matRows != null && PLMFlag != "Yes")
                        {
                            string palletDims = formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.PalletDimensionsLength]), "decimal") + " in. X " + formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.PalletDimensionsWidth]), "decimal") + " in. X " + formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.PalletDimensionsHeight]), "decimal") + " in.";
                            if (palletDims == " in. X  in. X  in." || palletDims == "-9999.00 in. X -9999.00 in. X -9999.00 in.") { palletDims = ""; }
                            sgItem.Add(new KeyValuePair<string, string>("Pallet Dimensions (LXWXH)", palletDims));
                            sgItem.Add(new KeyValuePair<string, string>("Gross Pallet Weight (lbs)", formatText(Convert.ToString(matRows[CompassPackMeasurementsFields.PalletGrossWeight]), "decimal")));
                        }
                        sgItem.Add(new KeyValuePair<string, string>("Product Hierarchy Level 1", Convert.ToString(item[CompassListFields.ProductHierarchyLevel1])));
                        sgItem.Add(new KeyValuePair<string, string>("Product Hierarchy Level 2", Convert.ToString(item[CompassListFields.ProductHierarchyLevel2])));
                        sgItem.Add(new KeyValuePair<string, string>("Material Group 1 (Brand)", Convert.ToString(item[CompassListFields.MaterialGroup1Brand])));
                        sgItem.Add(new KeyValuePair<string, string>("Material Group 2 (Trade Promo Group)", Convert.ToString(item[CompassListFields.MaterialGroup2Pricing])));
                        if (matRows != null && PLMFlag != "Yes")
                        {
                            sgItem.Add(new KeyValuePair<string, string>("Double Stackable?", Convert.ToString(matRows[CompassPackMeasurementsFields.DoubleStackable])));
                        }
                        sgItem.Add(new KeyValuePair<string, string>("Material Group 4 (Product Form)", Convert.ToString(item[CompassListFields.MaterialGroup4ProductForm])));
                        sgItem.Add(new KeyValuePair<string, string>("Material Group 5 (Pack Type)", Convert.ToString(item[CompassListFields.MaterialGroup5PackType])));

                        if (newTransSemi.Count > 0 && PLMFlag != "Yes")
                        {
                            sgItem.Add(new KeyValuePair<string, string>("Transfer Semi Measurements", "subHead"));
                            foreach (SPListItem packMeas in newTransSemi)
                            {
                                sgItem.Add(new KeyValuePair<string, string>("Material Number", Convert.ToString(packMeas[CompassPackMeasurementsFields.PackTrialComments])));
                                sgItem.Add(new KeyValuePair<string, string>("Material Description", Convert.ToString(packMeas[CompassPackMeasurementsFields.PackTrialResult])));
                                sgItem.Add(new KeyValuePair<string, string>("Case Pack", formatText(Convert.ToString(packMeas[CompassPackMeasurementsFields.CasePack]), "number")));
                                sgItem.Add(new KeyValuePair<string, string>("Net Unit Weight (oz)", formatText(Convert.ToString(packMeas[CompassPackMeasurementsFields.NetUnitWeight]), "decimal")));
                                sgItem.Add(new KeyValuePair<string, string>("Gross Case Weight (lbs)", formatText(Convert.ToString(packMeas[CompassPackMeasurementsFields.CaseGrossWeight]), "number")));
                                sgItem.Add(new KeyValuePair<string, string>("Net Case Weight (lbs)", formatText(Convert.ToString(packMeas[CompassPackMeasurementsFields.CaseNetWeight]), "decimal")));
                                string unitDims2 = formatText(Convert.ToString(packMeas[CompassPackMeasurementsFields.UnitDimensionLength]), "decimal") + " in. X " + formatText(Convert.ToString(packMeas[CompassPackMeasurementsFields.UnitDimensionWidth]), "decimal") + " in. X " + formatText(Convert.ToString(packMeas[CompassPackMeasurementsFields.UnitDimensionHeight]), "decimal") + " in.";
                                if (unitDims2 == " in. X  in. X  in." || unitDims2 == "-9999.00 in. X -9999.00 in. X -9999.00 in.") { unitDims2 = ""; }
                                sgItem.Add(new KeyValuePair<string, string>("Unit Dimensions (LXWXH)", unitDims2));
                                string caseDims2 = formatText(Convert.ToString(packMeas[CompassPackMeasurementsFields.CaseDimensionLength]), "decimal") + " in. X " + formatText(Convert.ToString(packMeas[CompassPackMeasurementsFields.CaseDimensionWidth]), "decimal") + " in. X " + formatText(Convert.ToString(packMeas[CompassPackMeasurementsFields.CaseDimensionHeight]), "decimal") + " in.";
                                if (caseDims2 == " in. X  in. X  in." || caseDims2 == "-9999.00 in. X -9999.00 in. X -9999.00 in.") { caseDims2 = ""; }
                                sgItem.Add(new KeyValuePair<string, string>("Case Dimensions (LXWXH)", caseDims2));
                                sgItem.Add(new KeyValuePair<string, string>("Cases/Pallet", formatText(Convert.ToString(packMeas[CompassPackMeasurementsFields.CasesPerPallet]), "number")));
                                sgItem.Add(new KeyValuePair<string, string>("Cases per Layer", formatText(Convert.ToString(packMeas[CompassPackMeasurementsFields.CasesPerLayer]), "number")));
                                sgItem.Add(new KeyValuePair<string, string>("Layers per Pallet", formatText(Convert.ToString(packMeas[CompassPackMeasurementsFields.LayersPerPallet]), "number")));
                                string palletDims2 = formatText(Convert.ToString(packMeas[CompassPackMeasurementsFields.PalletDimensionsLength]), "decimal") + " in. X " + formatText(Convert.ToString(packMeas[CompassPackMeasurementsFields.PalletDimensionsWidth]), "decimal") + " in. X " + formatText(Convert.ToString(packMeas[CompassPackMeasurementsFields.PalletDimensionsHeight]), "decimal") + " in.";
                                if (palletDims2 == " in. X  in. X  in." || palletDims2 == "-9999.00 in. X -9999.00 in. X -9999.00 in.") { palletDims2 = ""; }
                                sgItem.Add(new KeyValuePair<string, string>("Pallet Dimensions (LXWXH)", palletDims2));
                                sgItem.Add(new KeyValuePair<string, string>("Gross Pallet Weight (lbs)", formatText(Convert.ToString(packMeas[CompassPackMeasurementsFields.PalletGrossWeight]), "decimal")));
                                sgItem.Add(new KeyValuePair<string, string>("Double Stackable?", Convert.ToString(packMeas[CompassPackMeasurementsFields.DoubleStackable])));
                                sgItem.Add(new KeyValuePair<string, string>("endTS", "endTS"));

                            }
                        }
                    }
                }
            }
            return sgItem;
        }
        public List<KeyValuePair<string, string>> hideRow(SPWeb spWeb, int compassID)
        {
            List<KeyValuePair<string, string>> showHideParams = new List<KeyValuePair<string, string>>();
            SPList spDecisionList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectDecisionsListName);
            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Integer\">" + compassID.ToString() + "</Value></Eq></Where>";
            SPListItemCollection itemDecision = spDecisionList.GetItems(spQuery);

            if (itemDecision.Count > 0)
            {
                foreach (SPListItem decisions in itemDecision)
                {

                    if (decisions != null)
                    {
                        showHideParams.Add(new KeyValuePair<string, string>("InitialCosting", Convert.ToString(decisions[CompassProjectDecisionsListFields.SrOBMApproval_CostingDecision])));
                        showHideParams.Add(new KeyValuePair<string, string>("InitialCapacity", Convert.ToString(decisions[CompassProjectDecisionsListFields.SrOBMApproval_CapacityDecision])));
                    }
                }
            }

            SPList spCompassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
            SPListItem itemCompass = spCompassList.GetItemById(compassID);
            if (itemCompass != null)
            {

                showHideParams.Add(new KeyValuePair<string, string>("ExternalMfgML", Convert.ToString(itemCompass[CompassListFields.ManufacturingLocation])));
                showHideParams.Add(new KeyValuePair<string, string>("ExternalMfgPL", Convert.ToString(itemCompass[CompassListFields.PackingLocation])));
                //showHideParams.Add(new KeyValuePair<string, string>("QANF", Convert.ToString(itemCompass[CompassListFields.NewFormula])));
                //showHideParams.Add(new KeyValuePair<string, string>("QATBD", Convert.ToString(itemCompass[CompassListFields.TBDIndicator])));
                showHideParams.Add(new KeyValuePair<string, string>("Distribution", Convert.ToString(itemCompass[CompassListFields.TBDIndicator])));
                showHideParams.Add(new KeyValuePair<string, string>("SAPInitialSetup", Convert.ToString(itemCompass[CompassListFields.TBDIndicator])));

                //For Hiding By Project Type
                string projectType = Convert.ToString(itemCompass[CompassListFields.ProjectType]);
                showHideParams.Add(new KeyValuePair<string, string>("PTradePromo", projectType));
                showHideParams.Add(new KeyValuePair<string, string>("PDistribution", projectType));
                showHideParams.Add(new KeyValuePair<string, string>("PSAPInitialSetup", projectType));
            }
            return showHideParams;
        }
        private string formatText(string text, string format)
        {
            string formattedText = "";
            if (text != "" && text != "-9,999" && text != "-9999")
            {
                double number = Convert.ToDouble(text);
                if (format == "number")
                {
                    formattedText = number.ToString("n0");
                }
                else if (format == "percent")
                {
                    formattedText = number.ToString("P2");
                }
                else if (format == "money")
                {
                    formattedText = number.ToString("C2");
                }
                else if (format == "decimal")
                {
                    formattedText = number.ToString("F2");
                }
            }
            return formattedText;
        }
        public string IsProjectNetworkMove(SPListItem item)
        {
            string IsProjectNetworkMove = GlobalConstants.CONST_No;

            string ProjectType = Convert.ToString(item[CompassListFields.ProjectType]);
            string ProjectTypeSubCategory = Convert.ToString(item[CompassListFields.ProjectTypeSubCategory]);
            string MfgLocationChange = Convert.ToString(item[CompassListFields.MfgLocationChange]);

            if (ProjectType == GlobalConstants.PROJECTTYPE_SimpleNetworkMove ||
                ProjectTypeSubCategory == GlobalConstants.PROJECTTYPESUBCATEGORY_ComplexNetworkMove ||
                ProjectTypeSubCategory == GlobalConstants.PROJECTTYPESUBCATEGORY_NetworkMove ||
                MfgLocationChange == GlobalConstants.CONST_Yes)
            {
                IsProjectNetworkMove = GlobalConstants.CONST_Yes;
            }

            return IsProjectNetworkMove;
        }

    }
}
