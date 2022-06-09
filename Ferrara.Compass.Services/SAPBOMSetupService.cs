using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;

namespace Ferrara.Compass.Services
{
    public class SAPBOMSetupService : ISAPBOMSetupService
    {
        public SAPBOMSetupItem GetSAPBOMSetupItem(int itemId)
        {
            SAPBOMSetupItem sgItem = new SAPBOMSetupItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem item = spList.GetItemById(itemId);
                    if (item != null)
                    {
                        sgItem.CompassListItemId = item.ID;
                        sgItem.ProjectType = Convert.ToString(item[CompassListFields.ProjectType]);
                        sgItem.SAPDescription = Convert.ToString(item[CompassListFields.SAPDescription]);
                        sgItem.SAPItemNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);

                        sgItem.ImmediateSPKChange = Convert.ToString(item[CompassListFields.ImmediateSPKChange]);
                        sgItem.MakeLocation = Convert.ToString(item[CompassListFields.ManufacturingLocation]);
                        sgItem.PackingLocation = Convert.ToString(item[CompassListFields.PackingLocation]);
                        sgItem.PurchasedIntoLocation = Convert.ToString(item[CompassListFields.PurchasedIntoLocation]);

                        sgItem.SAPBaseUOM = Convert.ToString(item[CompassListFields.SAPBaseUOM]);

                        sgItem.ProductHierarchyLevel1 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]);
                        sgItem.ProductHierarchyLevel2 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel2]);
                        sgItem.MaterialGroup1Brand = Convert.ToString(item[CompassListFields.MaterialGroup1Brand]);
                        sgItem.MaterialGroup2Pricing = Convert.ToString(item[CompassListFields.MaterialGroup2Pricing]);
                        sgItem.MaterialGroup4ProductForm = Convert.ToString(item[CompassListFields.MaterialGroup4ProductForm]);
                        sgItem.MaterialGroup5PackType = Convert.ToString(item[CompassListFields.MaterialGroup5PackType]);
                        sgItem.ProfitCenter = Convert.ToString(item[CompassListFields.ProfitCenter]);
                        sgItem.ProcurementType = Convert.ToString(item[CompassListFields.CoManufacturingClassification]);

                        sgItem.UnitUPC = Convert.ToString(item[CompassListFields.UnitUPC]);
                        sgItem.CaseUCC = Convert.ToString(item[CompassListFields.CaseUCC]);
                        sgItem.DisplayBoxUPC = Convert.ToString(item[CompassListFields.DisplayBoxUPC]);
                        sgItem.PalletUCC = Convert.ToString(item[CompassListFields.PalletUCC]);
                        sgItem.ExternalManufacturer = Convert.ToString(item[CompassListFields.ExternalManufacturer]);
                        sgItem.ExternalPacker = Convert.ToString(item[CompassListFields.ExternalPacker]);
                        sgItem.PLMProject = Convert.ToString(item[CompassListFields.PLMProject]);
                    }
                    #region LIST_CompassList2Name
                    sgItem = GetDistributionItem2(sgItem, spWeb, itemId);
                    #endregion
                    #region LIST_ProjectDecisionsListName
                    spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectDecisionsListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    spQuery.RowLimit = 1;

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        SPListItem decisionItem = compassItemCol[0];
                        if (decisionItem != null)
                        {
                            sgItem.FinishedGoodBOMSetup = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_FinishedGoodBOMSetup]);
                            sgItem.NewMaterialNumbersCreated = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_NewMaterialNumbersCreated]);
                            sgItem.ContBuildFGBOM = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_ContBuildFGBOM]);
                            sgItem.TransferSemiBOMSetup = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_TransferSemiBOMSetup]);
                            sgItem.TransferMatNumCreatd = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_TransferMatNumCreatd]);
                            sgItem.HardSoftTransition = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_HardSoftTransition]);
                            sgItem.TransferSAPSpecsChangeCompleted = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_TSSAPSpecsChangeComp]);
                            sgItem.FGSAPSpecsChangeCompleted = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_FGSAPSpecsChangeComp]);
                            sgItem.TurnkeyFGMMCreated = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_TurnkeyFGMaterialMasterCreated]);
                            sgItem.CompleteFGBOMCreated = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_CompleteFGBOMCreated]);
                            sgItem.CompleteTSBOMCreated = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_CompleteTSBOMCreated]);
                            sgItem.PackMatsCreatedInPackLoc = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_PackMatsCreatedInPackLoc]);
                            sgItem.FGBOMCreatedInNewPackLoc = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_FGBOMCreatedInNewPackLoc]);
                            sgItem.SPKUpdatedInDCsPack = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_SPKUpdatedInDCsPack]);
                            sgItem.TSCompsCreatedInNewMPLoc = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_TSCompsCreatedInNewMPLoc]);
                            sgItem.TSFGBOMCreatedInNewMakeLoc = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_TSFGBOMCreatedInNewMakeLoc]);
                            sgItem.SPKUpdatedInDCsMake = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_SPKUpdatedInDCsMake]);
                            sgItem.ProdVersionCreated = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_ProdVersionCreated]);
                            sgItem.CreateNewPURCNDYSAPMatNum = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_CreateNewPURCNDYSAPMatNum]);
                            sgItem.NewFGBOMCreated = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_NewFGBOMCreated]);
                            sgItem.NewTSMaterialNumbersCreated = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_NewTSMaterialNumbersCreated]);
                            sgItem.NewTSCompPackNumsCreated = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_NewTSComponentPackNumsCreated]);
                            sgItem.InitialFGBOMCreated = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_InitialFGBOMCreated]);
                            sgItem.InitialTSBOMCreated = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_InitialTSBOMCreated]);
                            sgItem.FGSubConBOMCreated = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_FGSubConBOMCreated]);
                            sgItem.ExtendFGToDCs = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_ExtendFGToDCs]);
                            sgItem.VerifyFGBOMInDCs = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_VerifyFGBOMInDCs]);
                            sgItem.GS1Calculator = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_GS1Calculator]);
                            sgItem.FGPrivateLable = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_FGPrivateLable]);
                            sgItem.FGDCFP07 = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_FGDCFP07]);
                            sgItem.FGDCFP13 = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_FGDCFP13]);
                            sgItem.FGSPKOthers = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_FGSPKOthers]);
                            sgItem.VerifyPrivateLabel = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_VerifyPrivateLabel]);
                            sgItem.VerifyFGDCFP07 = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_VerifyFGDCFP07]);
                            sgItem.VerifyFGDCFP13 = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_VerifyFGDCFP13]);
                            sgItem.AddZSTOMatEntry = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_AddZSTOMatEntry]);
                            sgItem.TSCompsExtendedInNewMPLoc = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_TSCompsExtendedInNewMPLoc]);
                            sgItem.SPKsUpdatedPerDeployment = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_SPKsUpdatedPerDeployment]);
                            sgItem.ExtProfitCenterToDC = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_ExtProfitCenterToDC]);
                            sgItem.ClckNewTSPCPrftCntr = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_ClckNewTSPCPrftCntr]);
                            sgItem.OpenSalesFPCO = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.PrelimSAPSetup_OpenSalesFPCO]);
                            sgItem.OpenSalesSELL = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.PrelimSAPSetup_OpenSalesSELL]);
                            sgItem.OpenSalesFERQ = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.PrelimSAPSetup_OpenSalesFERQ]);

                            sgItem.EmptyTurnkeyAtFC01 = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPInitSet_EmptyTurnkeyAtFC01]);
                            sgItem.EmptyTurnkeyPCsAtFC01 = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPInitSet_EmptyTurnkeyPCsAtFC01]);
                            sgItem.ExtPCToSalesOrg0001 = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPInitSet_ExtPCToSalesOrg0001]);
                            sgItem.ExtTSToSalesOrg0001 = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPInitSet_ExtTSToSalesOrg0001]);
                            sgItem.ExtTSToSalesOrg1000 = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPInitSet_ExtTSToSalesOrg1000]);
                            sgItem.ExtPCToSalesOrg1000 = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPInitSet_ExtPCToSalesOrg1000]);
                            sgItem.ExtTSToSalesOrg2000 = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPInitSet_ExtTSToSalesOrg2000]);
                            sgItem.ExtFGToSalesOrg0001 = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPInitSet_ExtFGToSalesOrg0001]);
                            sgItem.ExtFGToSalesOrg1000 = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPInitSet_ExtFGToSalesOrg1000]);
                            sgItem.ExtFGToCompSale2000 = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPInitSet_ExtFGToCompSale2000]);
                            sgItem.ExtFGToSalesOrg2000 = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPInitSet_ExtFGToSalesOrg2000]);

                        }
                    }
                    #endregion
                    #region LIST_CompassPackMeasurementsListName
                    spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassPackMeasurementsListName);
                    SPQuery spQuery2 = new SPQuery();
                    spQuery2.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq><Eq><FieldRef Name=\"ParentComponentId\" /><Value Type=\"Int\">0</Value></Eq></And></Where>";
                    SPListItemCollection compassItemCol2 = spList.GetItems(spQuery2);
                    sgItem.FGSAPSpecsChangePackMeas = "";
                    if (compassItemCol2.Count > 0)
                    {
                        SPListItem decisionItem2 = compassItemCol2[0];
                        if (decisionItem2 != null)
                        {
                            sgItem.DoubleStackable = Convert.ToString(decisionItem2[CompassPackMeasurementsFields.DoubleStackable]);
                            sgItem.FGSAPSpecsChangePackMeas = Convert.ToString(decisionItem2[CompassPackMeasurementsFields.SAPSpecsChange]);
                        }
                    }
                    SPQuery spQuery3 = new SPQuery();
                    spQuery3.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq><Neq><FieldRef Name=\"" + CompassPackMeasurementsFields.ParentComponentId + "\" /><Value Type=\"Int\">0</Value></Neq></And></Where>";
                    SPListItemCollection compassItemCol3 = spList.GetItems(spQuery3);
                    sgItem.TransferSAPSpecsChangePackMeas = "no";
                    if (compassItemCol3.Count > 0)
                    {
                        foreach (SPListItem decisionItem3 in compassItemCol3)
                        {
                            if (decisionItem3 != null)
                            {
                                string SAPSpecsChange = Convert.ToString(decisionItem3[CompassPackMeasurementsFields.SAPSpecsChange]);
                                if (SAPSpecsChange.ToLower() == "yes")
                                {
                                    sgItem.TransferSAPSpecsChangePackMeas = SAPSpecsChange;
                                    break;
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            return sgItem;
        }
        private SAPBOMSetupItem GetDistributionItem2(SAPBOMSetupItem SAPBOMSetupItem, SPWeb spWeb, int itemId)
        {
            var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassList2Fields.CompassListItemId + "\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
            var CompassList2Items = spList.GetItems(spQuery);

            if (CompassList2Items != null && CompassList2Items.Count > 0)
            {
                var ipItem = CompassList2Items[0];
                SAPBOMSetupItem.DesignateHUBDC = Convert.ToString(ipItem[CompassList2Fields.DesignateHUBDC]);
                SAPBOMSetupItem.DeploymentModeofItem = Convert.ToString(ipItem[CompassList2Fields.DeploymentModeofItem]);

                #region SELL DCs
                SAPBOMSetupItem.ExtendtoSL07 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoSL07]);
                SAPBOMSetupItem.SetSL07SPKto = Convert.ToString(ipItem[CompassList2Fields.SetSL07SPKto]);
                SAPBOMSetupItem.ExtendtoSL13 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoSL13]);
                SAPBOMSetupItem.SetSL13SPKto = Convert.ToString(ipItem[CompassList2Fields.SetSL13SPKto]);
                SAPBOMSetupItem.ExtendtoSL18 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoSL18]);
                SAPBOMSetupItem.SetSL18SPKto = Convert.ToString(ipItem[CompassList2Fields.SetSL18SPKto]);
                SAPBOMSetupItem.ExtendtoSL19 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoSL19]);
                SAPBOMSetupItem.SetSL19SPKto = Convert.ToString(ipItem[CompassList2Fields.SetSL19SPKto]);
                SAPBOMSetupItem.ExtendtoSL30 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoSL30]);
                SAPBOMSetupItem.SetSL30SPKto = Convert.ToString(ipItem[CompassList2Fields.SetSL30SPKto]);
                SAPBOMSetupItem.ExtendtoSL14 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoSL14]);
                SAPBOMSetupItem.SetSL14SPKto = Convert.ToString(ipItem[CompassList2Fields.SetSL14SPKto]);
                #endregion

                #region FERQ DCs
                SAPBOMSetupItem.ExtendtoFQ26 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoFQ26]);
                SAPBOMSetupItem.SetFQ26SPKto = Convert.ToString(ipItem[CompassList2Fields.SetFQ26SPKto]);
                SAPBOMSetupItem.ExtendtoFQ27 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoFQ27]);
                SAPBOMSetupItem.SetFQ27SPKto = Convert.ToString(ipItem[CompassList2Fields.SetFQ27SPKto]);
                SAPBOMSetupItem.ExtendtoFQ28 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoFQ28]);
                SAPBOMSetupItem.SetFQ28SPKto = Convert.ToString(ipItem[CompassList2Fields.SetFQ28SPKto]);
                SAPBOMSetupItem.ExtendtoFQ29 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoFQ29]);
                SAPBOMSetupItem.SetFQ29SPKto = Convert.ToString(ipItem[CompassList2Fields.SetFQ29SPKto]);
                SAPBOMSetupItem.ExtendtoFQ34 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoFQ34]);
                SAPBOMSetupItem.SetFQ34SPKto = Convert.ToString(ipItem[CompassList2Fields.SetFQ34SPKto]);
                SAPBOMSetupItem.ExtendtoFQ35 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoFQ35]);
                SAPBOMSetupItem.SetFQ35SPKto = Convert.ToString(ipItem[CompassList2Fields.SetFQ35SPKto]);
                #endregion
            }
            return SAPBOMSetupItem;
        }
        public List<string> getTSSPKDetails(int itemId)
        {
            List<string> sgItems = new List<string>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery3 = new SPQuery();
                    spQuery3.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq><Neq><FieldRef Name=\"" + PackagingItemListFields.Deleted + "\" /><Value Type=\"Text\">Yes</Value></Neq></And></Where>";
                    SPListItemCollection compassItemCol3 = spList.GetItems(spQuery3);
                    if (compassItemCol3.Count > 0)
                    {
                        foreach (SPListItem decisionItem3 in compassItemCol3)
                        {
                            if (decisionItem3 != null)
                            {
                                string materialType = Convert.ToString(decisionItem3[PackagingItemListFields.PackagingComponent]);
                                if (materialType.ToLower() == "transfer semi" || materialType.ToLower() == "purchased candy semi")
                                {
                                    string newExisting = Convert.ToString(decisionItem3[PackagingItemListFields.NewExisting]);
                                    if (newExisting == "Network Move")
                                    {
                                        string MatType = "";
                                        string MatNumber = Convert.ToString(decisionItem3[PackagingItemListFields.MaterialNumber]);
                                        string SPKDec = Convert.ToString(decisionItem3[PackagingItemListFields.ImmediateSPKChange]);
                                        if (materialType.ToLower() == "purchased candy semi")
                                        {
                                            MatType = "PSC";
                                        }
                                        else
                                        {
                                            MatType = "TS";
                                        }
                                        sgItems.Add("Immediate SPK Change for " + MatType + " " + MatNumber + ": " + SPKDec);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return sgItems;
        }
        public void UpdateSAPBOMSetupItem(SAPBOMSetupItem sapBOMSetupItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectDecisionsListName);

                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + sapBOMSetupItem.CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        SPListItem item = compassItemCol[0];
                        if (item != null)
                        {
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_FinishedGoodBOMSetup] = sapBOMSetupItem.FinishedGoodBOMSetup;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_ContBuildFGBOM] = sapBOMSetupItem.ContBuildFGBOM;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_NewMaterialNumbersCreated] = sapBOMSetupItem.NewMaterialNumbersCreated;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_TransferSemiBOMSetup] = sapBOMSetupItem.TransferSemiBOMSetup;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_TransferMatNumCreatd] = sapBOMSetupItem.TransferMatNumCreatd;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_HardSoftTransition] = sapBOMSetupItem.HardSoftTransition;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_TSSAPSpecsChangeComp] = sapBOMSetupItem.TransferSAPSpecsChangeCompleted;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_FGSAPSpecsChangeComp] = sapBOMSetupItem.FGSAPSpecsChangeCompleted;


                            item[CompassProjectDecisionsListFields.SAPBOMSetup_TurnkeyFGMaterialMasterCreated] = sapBOMSetupItem.TurnkeyFGMMCreated;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_CompleteFGBOMCreated] = sapBOMSetupItem.CompleteFGBOMCreated;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_CompleteTSBOMCreated] = sapBOMSetupItem.CompleteTSBOMCreated;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_PackMatsCreatedInPackLoc] = sapBOMSetupItem.PackMatsCreatedInPackLoc;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_FGBOMCreatedInNewPackLoc] = sapBOMSetupItem.FGBOMCreatedInNewPackLoc;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_SPKUpdatedInDCsPack] = sapBOMSetupItem.SPKUpdatedInDCsPack;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_TSCompsCreatedInNewMPLoc] = sapBOMSetupItem.TSCompsCreatedInNewMPLoc;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_TSFGBOMCreatedInNewMakeLoc] = sapBOMSetupItem.TSFGBOMCreatedInNewMakeLoc;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_SPKUpdatedInDCsMake] = sapBOMSetupItem.SPKUpdatedInDCsMake;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_ProdVersionCreated] = sapBOMSetupItem.ProdVersionCreated;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_NewFGBOMCreated] = sapBOMSetupItem.NewFGBOMCreated;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_NewTSMaterialNumbersCreated] = sapBOMSetupItem.NewTSMaterialNumbersCreated;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_NewTSComponentPackNumsCreated] = sapBOMSetupItem.NewTSCompPackNumsCreated;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_VerifyFGBOMInDCs] = sapBOMSetupItem.VerifyFGBOMInDCs;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_VerifyPrivateLabel] = sapBOMSetupItem.VerifyPrivateLabel;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_GS1Calculator] = sapBOMSetupItem.GS1Calculator;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_VerifyFGDCFP07] = sapBOMSetupItem.VerifyFGDCFP07;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_VerifyFGDCFP13] = sapBOMSetupItem.VerifyFGDCFP13;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_VerifyFGSPKOthers] = sapBOMSetupItem.VerifyFGSPKOthers;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_AddZSTOMatEntry] = sapBOMSetupItem.AddZSTOMatEntry;

                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;

                            item.Update();
                        }

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        public void UpdateSAPBOMSetupItemFromInitial(SAPBOMSetupItem sapBOMSetupItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectDecisionsListName);

                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + sapBOMSetupItem.CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        SPListItem item = compassItemCol[0];
                        if (item != null)
                        {
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_InitialFGBOMCreated] = sapBOMSetupItem.InitialFGBOMCreated;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_InitialTSBOMCreated] = sapBOMSetupItem.InitialTSBOMCreated;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_PackMatsCreatedInPackLoc] = sapBOMSetupItem.PackMatsCreatedInPackLoc;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_FGBOMCreatedInNewPackLoc] = sapBOMSetupItem.FGBOMCreatedInNewPackLoc;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_SPKUpdatedInDCsPack] = sapBOMSetupItem.SPKUpdatedInDCsPack;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_TSCompsCreatedInNewMPLoc] = sapBOMSetupItem.TSCompsCreatedInNewMPLoc;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_TSFGBOMCreatedInNewMakeLoc] = sapBOMSetupItem.TSFGBOMCreatedInNewMakeLoc;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_SPKUpdatedInDCsMake] = sapBOMSetupItem.SPKUpdatedInDCsMake;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_ProdVersionCreated] = sapBOMSetupItem.ProdVersionCreated;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_FGSubConBOMCreated] = sapBOMSetupItem.FGSubConBOMCreated;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_ExtendFGToDCs] = sapBOMSetupItem.ExtendFGToDCs;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_FGPrivateLable] = sapBOMSetupItem.FGPrivateLable;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_FGDCFP07] = sapBOMSetupItem.FGDCFP07;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_FGDCFP13] = sapBOMSetupItem.FGDCFP13;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_FGSPKOthers] = sapBOMSetupItem.FGSPKOthers;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_TSCompsExtendedInNewMPLoc] = sapBOMSetupItem.TSCompsExtendedInNewMPLoc;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_SPKsUpdatedPerDeployment] = sapBOMSetupItem.SPKsUpdatedPerDeployment;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_AddZSTOMatEntry] = sapBOMSetupItem.AddZSTOMatEntry;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_ExtProfitCenterToDC] = sapBOMSetupItem.ExtProfitCenterToDC;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_ClckNewTSPCPrftCntr] = sapBOMSetupItem.ClckNewTSPCPrftCntr;

                            item[CompassProjectDecisionsListFields.SAPInitSet_EmptyTurnkeyAtFC01] = sapBOMSetupItem.EmptyTurnkeyAtFC01;
                            item[CompassProjectDecisionsListFields.SAPInitSet_EmptyTurnkeyPCsAtFC01] = sapBOMSetupItem.EmptyTurnkeyPCsAtFC01;
                            item[CompassProjectDecisionsListFields.SAPInitSet_ExtPCToSalesOrg0001] = sapBOMSetupItem.ExtPCToSalesOrg0001;
                            item[CompassProjectDecisionsListFields.SAPInitSet_ExtTSToSalesOrg0001] = sapBOMSetupItem.ExtTSToSalesOrg0001;
                            item[CompassProjectDecisionsListFields.SAPInitSet_ExtTSToSalesOrg1000] = sapBOMSetupItem.ExtTSToSalesOrg1000;
                            item[CompassProjectDecisionsListFields.SAPInitSet_ExtPCToSalesOrg1000] = sapBOMSetupItem.ExtPCToSalesOrg1000;
                            item[CompassProjectDecisionsListFields.SAPInitSet_ExtTSToSalesOrg2000] = sapBOMSetupItem.ExtTSToSalesOrg2000;
                            item[CompassProjectDecisionsListFields.SAPInitSet_ExtFGToSalesOrg0001] = sapBOMSetupItem.ExtFGToSalesOrg0001;
                            item[CompassProjectDecisionsListFields.SAPInitSet_ExtFGToSalesOrg1000] = sapBOMSetupItem.ExtFGToSalesOrg1000;
                            item[CompassProjectDecisionsListFields.SAPInitSet_ExtFGToCompSale2000] = sapBOMSetupItem.ExtFGToCompSale2000;
                            item[CompassProjectDecisionsListFields.SAPInitSet_ExtFGToSalesOrg2000] = sapBOMSetupItem.ExtFGToSalesOrg2000;


                            item[CompassProjectDecisionsListFields.SAPBOMSetup_CreateNewPURCNDYSAPMatNum] = sapBOMSetupItem.CreateNewPURCNDYSAPMatNum;

                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;

                            item.Update();
                        }

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        public void UpdatePrelimSAPInitialItemSetup(SAPBOMSetupItem sapBOMSetupItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectDecisionsListName);

                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + sapBOMSetupItem.CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        SPListItem item = compassItemCol[0];
                        if (item != null)
                        {
                            item[CompassProjectDecisionsListFields.PrelimSAPSetup_OpenSalesFPCO] = sapBOMSetupItem.OpenSalesFPCO;
                            item[CompassProjectDecisionsListFields.PrelimSAPSetup_OpenSalesSELL] = sapBOMSetupItem.OpenSalesSELL;
                            item[CompassProjectDecisionsListFields.PrelimSAPSetup_OpenSalesFERQ] = sapBOMSetupItem.OpenSalesFERQ;

                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;

                            item.Update();
                        }

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        #region Approval Methods
        public void UpdateSAPBOMSetupApprovalItem(ApprovalItem approvalItem, bool bSubmitted)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + approvalItem.CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem appItem = compassItemCol[0];
                            if (appItem != null)
                            {
                                if ((bSubmitted) && (appItem[ApprovalListFields.SAPBOMSetup_SubmittedDate] == null))
                                {
                                    appItem[ApprovalListFields.SAPBOMSetup_SubmittedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.SAPBOMSetup_SubmittedBy] = approvalItem.ModifiedBy;
                                }
                                else
                                {
                                    appItem[ApprovalListFields.SAPBOMSetup_ModifiedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.SAPBOMSetup_ModifiedBy] = approvalItem.ModifiedBy;
                                }
                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void SetSAPBOMSetupStartDate(int compassListItemId, DateTime startDate)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem item = compassItemCol[0];
                            if (item != null)
                            {
                                // Distribution Fields
                                if (item[ApprovalListFields.SAPBOMSetup_StartDate] == null)
                                {
                                    item[ApprovalListFields.SAPBOMSetup_StartDate] = startDate.ToString();
                                    item.Update();
                                }
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        #endregion
    }
}
