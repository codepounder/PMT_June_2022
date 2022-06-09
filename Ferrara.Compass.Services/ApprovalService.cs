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
    public class ApprovalService : IApprovalService
    {
        public ApprovalListItem GetApprovalItem(int itemId)
        {
            ApprovalListItem appItem = new ApprovalListItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    spQuery.RowLimit = 1;

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        SPListItem item = compassItemCol[0];

                        if (item != null)
                        {
                            appItem.ApprovalListItemId = item.ID;
                            appItem.CompassListItemId = Convert.ToInt32(item[ApprovalListFields.CompassListItemId]);
                            // IPF Fields
                            appItem.IPF_StartDate = Convert.ToString(item[ApprovalListFields.IPF_StartDate]);
                            appItem.IPF_SubmittedBy = Convert.ToString(item[ApprovalListFields.IPF_SubmittedBy]);
                            appItem.IPF_SubmittedDate = Convert.ToString(item[ApprovalListFields.IPF_SubmittedDate]);
                            appItem.IPF_ModifiedBy = Convert.ToString(item[ApprovalListFields.IPF_ModifiedBy]);
                            appItem.IPF_ModifiedDate = Convert.ToString(item[ApprovalListFields.IPF_ModifiedDate]);
                            // SrOBMApproval, SrOBMApproval2 Fields
                            appItem.SrOBMApproval_StartDate = Convert.ToString(item[ApprovalListFields.SrOBMApproval_StartDate]);
                            appItem.SrOBMApproval_ModifiedDate = Convert.ToString(item[ApprovalListFields.SrOBMApproval_ModifiedDate]);
                            appItem.SrOBMApproval_ModifiedBy = Convert.ToString(item[ApprovalListFields.SrOBMApproval_ModifiedBy]);
                            appItem.SrOBMApproval_SubmittedDate = Convert.ToString(item[ApprovalListFields.SrOBMApproval_SubmittedDate]);
                            appItem.SrOBMApproval_SubmittedBy = Convert.ToString(item[ApprovalListFields.SrOBMApproval_SubmittedBy]);

                            appItem.SrOBMApproval2_StartDate = Convert.ToString(item[ApprovalListFields.SrOBMApproval2_StartDate]);
                            appItem.SrOBMApproval2_ModifiedDate = Convert.ToString(item[ApprovalListFields.SrOBMApproval2_ModifiedDate]);
                            appItem.SrOBMApproval2_ModifiedBy = Convert.ToString(item[ApprovalListFields.SrOBMApproval2_ModifiedBy]);
                            appItem.SrOBMApproval2_SubmittedDate = Convert.ToString(item[ApprovalListFields.SrOBMApproval2_SubmittedDate]);
                            appItem.SrOBMApproval2_SubmittedBy = Convert.ToString(item[ApprovalListFields.SrOBMApproval2_SubmittedBy]);
                            // Initial Costing Fields
                            appItem.InitialCosting_StartDate = Convert.ToString(item[ApprovalListFields.InitialCosting_StartDate]);
                            appItem.InitialCosting_ModifiedBy = Convert.ToString(item[ApprovalListFields.InitialCosting_ModifiedBy]);
                            appItem.InitialCosting_ModifiedDate = Convert.ToString(item[ApprovalListFields.InitialCosting_ModifiedDate]);
                            appItem.InitialCosting_SubmittedBy = Convert.ToString(item[ApprovalListFields.InitialCosting_SubmittedBy]);
                            appItem.InitialCosting_SubmittedDate = Convert.ToString(item[ApprovalListFields.InitialCosting_SubmittedDate]);
                            // Distribution Fields
                            appItem.Distribution_StartDate = Convert.ToString(item[ApprovalListFields.Distribution_StartDate]);
                            appItem.Distribution_ModifiedDate = Convert.ToString(item[ApprovalListFields.Distribution_ModifiedDate]);
                            appItem.Distribution_ModifiedBy = Convert.ToString(item[ApprovalListFields.Distribution_ModifiedBy]);
                            appItem.Distribution_SubmittedDate = Convert.ToString(item[ApprovalListFields.Distribution_SubmittedDate]);
                            appItem.Distribution_SubmittedBy = Convert.ToString(item[ApprovalListFields.Distribution_SubmittedBy]);
                            // Operations Make Pack Fields
                            appItem.Operations_StartDate = Convert.ToString(item[ApprovalListFields.Operations_StartDate]);
                            appItem.Operations_ModifiedDate = Convert.ToString(item[ApprovalListFields.Operations_ModifiedDate]);
                            appItem.Operations_ModifiedBy = Convert.ToString(item[ApprovalListFields.Operations_ModifiedBy]);
                            appItem.Operations_SubmittedDate = Convert.ToString(item[ApprovalListFields.Operations_SubmittedDate]);
                            appItem.Operations_SubmittedBy = Convert.ToString(item[ApprovalListFields.Operations_SubmittedBy]);
                            // SAP Item Request Fields
                            appItem.SAPInitialSetup_StartDate = Convert.ToString(item[ApprovalListFields.SAPInitialSetup_StartDate]);
                            appItem.SAPInitialSetup_ModifiedDate = Convert.ToString(item[ApprovalListFields.SAPInitialSetup_ModifiedDate]);
                            appItem.SAPInitialSetup_ModifiedBy = Convert.ToString(item[ApprovalListFields.SAPInitialSetup_ModifiedBy]);
                            appItem.SAPInitialSetup_SubmittedDate = Convert.ToString(item[ApprovalListFields.SAPInitialSetup_SubmittedDate]);
                            appItem.SAPInitialSetup_SubmittedBy = Convert.ToString(item[ApprovalListFields.SAPInitialSetup_SubmittedBy]);
                            // Preliminary SAP Item Request Fields
                            appItem.PrelimSAPInitialSetup_StartDate = Convert.ToString(item[ApprovalListFields.PrelimSAPInitialSetup_StartDate]);
                            appItem.PrelimSAPInitialSetup_ModifiedDate = Convert.ToString(item[ApprovalListFields.PrelimSAPInitialSetup_ModifiedDate]);
                            appItem.PrelimSAPInitialSetup_ModifiedBy = Convert.ToString(item[ApprovalListFields.PrelimSAPInitialSetup_ModifiedBy]);
                            appItem.PrelimSAPInitialSetup_SubmittedDate = Convert.ToString(item[ApprovalListFields.PrelimSAPInitialSetup_SubmittedDate]);
                            appItem.PrelimSAPInitialSetup_SubmittedBy = Convert.ToString(item[ApprovalListFields.PrelimSAPInitialSetup_SubmittedBy]);
                            // QA Fields
                            appItem.QA_StartDate = Convert.ToString(item[ApprovalListFields.QA_StartDate]);
                            appItem.QA_ModifiedDate = Convert.ToString(item[ApprovalListFields.QA_ModifiedDate]);
                            appItem.QA_ModifiedBy = Convert.ToString(item[ApprovalListFields.QA_ModifiedBy]);
                            appItem.QA_SubmittedDate = Convert.ToString(item[ApprovalListFields.QA_SubmittedDate]);
                            appItem.QA_SubmittedBy = Convert.ToString(item[ApprovalListFields.QA_SubmittedBy]);
                            // PM First Review Fields
                            appItem.OBMReview1_StartDate = Convert.ToString(item[ApprovalListFields.OBMReview1_StartDate]);
                            appItem.OBMReview1_ModifiedDate = Convert.ToString(item[ApprovalListFields.OBMReview1_ModifiedDate]);
                            appItem.OBMReview1_ModifiedBy = Convert.ToString(item[ApprovalListFields.OBMReview1_ModifiedBy]);
                            appItem.OBMReview1_SubmittedDate = Convert.ToString(item[ApprovalListFields.OBMReview1_SubmittedDate]);
                            appItem.OBMReview1_SubmittedBy = Convert.ToString(item[ApprovalListFields.OBMReview1_SubmittedBy]);
                            // Material Numbers Fields
                            appItem.BOMSetupPE_StartDate = Convert.ToString(item[ApprovalListFields.BOMSetupPE_StartDate]);
                            appItem.BOMSetupPE_ModifiedDate = Convert.ToString(item[ApprovalListFields.BOMSetupPE_ModifiedDate]);
                            appItem.BOMSetupPE_ModifiedBy = Convert.ToString(item[ApprovalListFields.BOMSetupPE_ModifiedBy]);
                            appItem.BOMSetupPE_SubmittedDate = Convert.ToString(item[ApprovalListFields.BOMSetupPE_SubmittedDate]);
                            appItem.BOMSetupPE_SubmittedBy = Convert.ToString(item[ApprovalListFields.BOMSetupPE_SubmittedBy]);
                            appItem.BOMSetupProc_StartDate = Convert.ToString(item[ApprovalListFields.BOMSetupProc_StartDate]);
                            appItem.BOMSetupProc_ModifiedDate = Convert.ToString(item[ApprovalListFields.BOMSetupProc_ModifiedDate]);
                            appItem.BOMSetupProc_ModifiedBy = Convert.ToString(item[ApprovalListFields.BOMSetupProc_ModifiedBy]);
                            appItem.BOMSetupProc_SubmittedDate = Convert.ToString(item[ApprovalListFields.BOMSetupProc_SubmittedDate]);
                            appItem.BOMSetupProc_SubmittedBy = Convert.ToString(item[ApprovalListFields.BOMSetupProc_SubmittedBy]);
                            appItem.BOMSetupPE2_StartDate = Convert.ToString(item[ApprovalListFields.BOMSetupPE2_StartDate]);
                            appItem.BOMSetupPE2_ModifiedDate = Convert.ToString(item[ApprovalListFields.BOMSetupPE2_ModifiedDate]);
                            appItem.BOMSetupPE2_ModifiedBy = Convert.ToString(item[ApprovalListFields.BOMSetupPE2_ModifiedBy]);
                            appItem.BOMSetupPE2_SubmittedDate = Convert.ToString(item[ApprovalListFields.BOMSetupPE2_SubmittedDate]);
                            appItem.BOMSetupPE2_SubmittedBy = Convert.ToString(item[ApprovalListFields.BOMSetupPE2_SubmittedBy]);
                            appItem.BOMSetupPE3_StartDate = Convert.ToString(item[ApprovalListFields.BOMSetupPE3_StartDate]);
                            appItem.BOMSetupPE3_ModifiedDate = Convert.ToString(item[ApprovalListFields.BOMSetupPE3_ModifiedDate]);
                            appItem.BOMSetupPE3_ModifiedBy = Convert.ToString(item[ApprovalListFields.BOMSetupPE3_ModifiedBy]);
                            appItem.BOMSetupPE3_SubmittedDate = Convert.ToString(item[ApprovalListFields.BOMSetupPE3_SubmittedDate]);
                            appItem.BOMSetupPE3_SubmittedBy = Convert.ToString(item[ApprovalListFields.BOMSetupPE3_SubmittedBy]);

                            appItem.MatrlWHSetUp_StartDate = Convert.ToString(item[ApprovalListFields.BOMSetupMaterialWarehouse_StartDate]);
                            appItem.MatrlWHSetUp_ModifiedDate = Convert.ToString(item[ApprovalListFields.BOMSetupMaterialWarehouse_ModifiedDate]);
                            appItem.MatrlWHSetUp_ModifiedBy = Convert.ToString(item[ApprovalListFields.BOMSetupMaterialWarehouse_ModifiedBy]);
                            appItem.MatrlWHSetUp_SubmittedDate = Convert.ToString(item[ApprovalListFields.BOMSetupMaterialWarehouse_SubmittedDate]);
                            appItem.MatrlWHSetUp_SubmittedBy = Convert.ToString(item[ApprovalListFields.BOMSetupMaterialWarehouse_SubmittedBy]);

                            appItem.SAPCompleteItem_StartDate = Convert.ToString(item[ApprovalListFields.SAPCompleteItemSetup_StartDate]);
                            appItem.SAPCompleteItem_ModifiedDate = Convert.ToString(item[ApprovalListFields.SAPCompleteItemSetup_ModifiedDate]);
                            appItem.SAPCompleteItem_ModifiedBy = Convert.ToString(item[ApprovalListFields.SAPCompleteItemSetup_ModifiedBy]);
                            appItem.SAPCompleteItem_SubmittedDate = Convert.ToString(item[ApprovalListFields.SAPCompleteItemSetup_SubmittedDate]);
                            appItem.SAPCompleteItem_SubmittedBy = Convert.ToString(item[ApprovalListFields.SAPCompleteItemSetup_SubmittedBy]);

                            // SAP Item Setup Fields
                            // PM Second Review Fields
                            appItem.OBMReview2_StartDate = Convert.ToString(item[ApprovalListFields.OBMReview2_StartDate]);
                            appItem.OBMReview2_ModifiedDate = Convert.ToString(item[ApprovalListFields.OBMReview2_ModifiedDate]);
                            appItem.OBMReview2_ModifiedBy = Convert.ToString(item[ApprovalListFields.OBMReview2_ModifiedBy]);
                            appItem.OBMReview2_SubmittedDate = Convert.ToString(item[ApprovalListFields.OBMReview2_SubmittedDate]);
                            appItem.OBMReview2_SubmittedBy = Convert.ToString(item[ApprovalListFields.OBMReview2_SubmittedBy]);
                            // Request For Graphics Fields
                            appItem.GRAPHICS_StartDate = Convert.ToString(item[ApprovalListFields.GRAPHICS_StartDate]);
                            appItem.GRAPHICS_ModifiedDate = Convert.ToString(item[ApprovalListFields.GRAPHICS_ModifiedDate]);
                            appItem.GRAPHICS_ModifiedBy = Convert.ToString(item[ApprovalListFields.GRAPHICS_ModifiedBy]);
                            appItem.GRAPHICS_SubmittedDate = Convert.ToString(item[ApprovalListFields.GRAPHICS_SubmittedDate]);
                            appItem.GRAPHICS_SubmittedBy = Convert.ToString(item[ApprovalListFields.GRAPHICS_SubmittedBy]);
                            // SAPBOMSetup Fields
                            appItem.SAPBOMSetup_StartDate = Convert.ToString(item[ApprovalListFields.SAPBOMSetup_StartDate]);
                            appItem.SAPBOMSetup_ModifiedDate = Convert.ToString(item[ApprovalListFields.SAPBOMSetup_ModifiedDate]);
                            appItem.SAPBOMSetup_ModifiedBy = Convert.ToString(item[ApprovalListFields.SAPBOMSetup_ModifiedBy]);
                            appItem.SAPBOMSetup_SubmittedDate = Convert.ToString(item[ApprovalListFields.SAPBOMSetup_SubmittedDate]);
                            appItem.SAPBOMSetup_SubmittedBy = Convert.ToString(item[ApprovalListFields.SAPBOMSetup_SubmittedBy]);
                            // FGPackSpec Fields
                            appItem.FGPackSpec_StartDate = Convert.ToString(item[ApprovalListFields.FGPackSpec_StartDate]);
                            appItem.FGPackSpec_ModifiedDate = Convert.ToString(item[ApprovalListFields.FGPackSpec_ModifiedDate]);
                            appItem.FGPackSpec_ModifiedBy = Convert.ToString(item[ApprovalListFields.FGPackSpec_ModifiedBy]);
                            appItem.FGPackSpec_SubmittedDate = Convert.ToString(item[ApprovalListFields.FGPackSpec_SubmittedDate]);
                            appItem.FGPackSpec_SubmittedBy = Convert.ToString(item[ApprovalListFields.FGPackSpec_SubmittedBy]);
                            // CostingQuote Fields
                            appItem.CompCostFLRP_StartDate = Convert.ToString(item[ApprovalListFields.CompCostFLRP_StartDate]);
                            appItem.CompCostFLRP_ModifiedDate = Convert.ToString(item[ApprovalListFields.CompCostFLRP_ModifiedDate]);
                            appItem.CompCostFLRP_ModifiedBy = Convert.ToString(item[ApprovalListFields.CompCostFLRP_ModifiedBy]);
                            appItem.CompCostFLRP_SubmittedDate = Convert.ToString(item[ApprovalListFields.CompCostFLRP_SubmittedDate]);
                            appItem.CompCostFLRP_SubmittedBy = Convert.ToString(item[ApprovalListFields.CompCostFLRP_SubmittedBy]);

                            appItem.CompCostSeasonal_StartDate = Convert.ToString(item[ApprovalListFields.CompCostSeasonal_StartDate]);
                            appItem.CompCostSeasonal_ModifiedDate = Convert.ToString(item[ApprovalListFields.CompCostSeasonal_ModifiedDate]);
                            appItem.CompCostSeasonal_ModifiedBy = Convert.ToString(item[ApprovalListFields.CompCostSeasonal_ModifiedBy]);
                            appItem.CompCostSeasonal_SubmittedDate = Convert.ToString(item[ApprovalListFields.CompCostSeasonal_SubmittedDate]);
                            appItem.CompCostSeasonal_SubmittedBy = Convert.ToString(item[ApprovalListFields.CompCostSeasonal_SubmittedBy]);

                            appItem.CompCostCorrPaper_StartDate = Convert.ToString(item[ApprovalListFields.CompCostCorrPaper_StartDate]);
                            appItem.CompCostCorrPaper_ModifiedDate = Convert.ToString(item[ApprovalListFields.CompCostCorrPaper_ModifiedDate]);
                            appItem.CompCostCorrPaper_ModifiedBy = Convert.ToString(item[ApprovalListFields.CompCostCorrPaper_ModifiedBy]);
                            appItem.CompCostCorrPaper_SubmittedDate = Convert.ToString(item[ApprovalListFields.CompCostCorrPaper_SubmittedDate]);
                            appItem.CompCostCorrPaper_SubmittedBy = Convert.ToString(item[ApprovalListFields.CompCostCorrPaper_SubmittedBy]);
                            //ExternalMfg
                            appItem.ExternalMfg_StartDate = Convert.ToString(item[ApprovalListFields.ExternalMfg_StartDate]);
                            appItem.ExternalMfg_ModifiedDate = Convert.ToString(item[ApprovalListFields.ExternalMfg_ModifiedDate]);
                            appItem.ExternalMfg_ModifiedBy = Convert.ToString(item[ApprovalListFields.ExternalMfg_ModifiedBy]);
                            appItem.ExternalMfg_SubmittedDate = Convert.ToString(item[ApprovalListFields.ExternalMfg_SubmittedDate]);
                            appItem.ExternalMfg_SubmittedBy = Convert.ToString(item[ApprovalListFields.ExternalMfg_SubmittedBy]);
                            //SAP Routing Setup
                            appItem.SAPRoutingSetup_StartDate = Convert.ToString(item[ApprovalListFields.SAPRoutingSetup_StartDate]);
                            appItem.SAPRoutingSetup_ModifiedDate = Convert.ToString(item[ApprovalListFields.SAPRoutingSetup_ModifiedDate]);
                            appItem.SAPRoutingSetup_ModifiedBy = Convert.ToString(item[ApprovalListFields.SAPRoutingSetup_ModifiedBy]);
                            appItem.SAPRoutingSetup_SubmittedDate = Convert.ToString(item[ApprovalListFields.SAPRoutingSetup_SubmittedDate]);
                            appItem.SAPRoutingSetup_SubmittedBy = Convert.ToString(item[ApprovalListFields.SAPRoutingSetup_SubmittedBy]);
                            //Trade Promo
                            appItem.TradePromo_StartDate = Convert.ToString(item[ApprovalListFields.TradePromo_StartDate]);
                            appItem.TradePromo_ModifiedDate = Convert.ToString(item[ApprovalListFields.TradePromo_ModifiedDate]);
                            appItem.TradePromo_ModifiedBy = Convert.ToString(item[ApprovalListFields.TradePromo_ModifiedBy]);
                            appItem.TradePromo_SubmittedDate = Convert.ToString(item[ApprovalListFields.TradePromo_SubmittedDate]);
                            appItem.TradePromo_SubmittedBy = Convert.ToString(item[ApprovalListFields.TradePromo_SubmittedBy]);
                            // Other Fields
                            appItem.OnHold_ModifiedDate = Convert.ToString(item[ApprovalListFields.OnHold_ModifiedDate]);
                            appItem.OnHold_ModifiedBy = Convert.ToString(item[ApprovalListFields.OnHold_ModifiedBy]);
                            appItem.PreProduction_ModifiedDate = Convert.ToString(item[ApprovalListFields.PreProduction_ModifiedDate]);
                            appItem.PreProduction_ModifiedBy = Convert.ToString(item[ApprovalListFields.PreProduction_ModifiedBy]);
                            appItem.Completed_ModifiedDate = Convert.ToString(item[ApprovalListFields.Completed_ModifiedDate]);
                            appItem.Completed_ModifiedBy = Convert.ToString(item[ApprovalListFields.Completed_ModifiedBy]);
                            appItem.Cancelled_ModifiedDate = Convert.ToString(item[ApprovalListFields.Cancelled_ModifiedDate]);
                            appItem.Cancelled_ModifiedBy = Convert.ToString(item[ApprovalListFields.Cancelled_ModifiedBy]);
                            appItem.ProductionCompleted_ModifiedDate = Convert.ToString(item[ApprovalListFields.ProductionCompleted_ModifiedDate]);
                            appItem.ProductionCompleted_ModifiedBy = Convert.ToString(item[ApprovalListFields.ProductionCompleted_ModifiedBy]);
                        }
                    }
                    try
                    {
                        SPList spList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalList2Name);

                        SPQuery spQuery2 = new SPQuery();
                        spQuery2.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                        spQuery2.RowLimit = 1;

                        SPListItemCollection compassItemCol2 = spList2.GetItems(spQuery2);
                        if (compassItemCol2.Count > 0)
                        {
                            SPListItem item2 = compassItemCol2[0];

                            if (item2 != null)
                            {
                                appItem.ProcAncillary_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcAncillary_SubmittedDate]);
                                appItem.ProcCorrugated_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcCorrugated_SubmittedDate]);
                                appItem.ProcPurchased_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcPurchased_SubmittedDate]);
                                appItem.ProcFilm_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcFilm_SubmittedDate]);
                                appItem.ProcLabel_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcLabel_SubmittedDate]);
                                appItem.ProcMetal_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcMetal_SubmittedDate]);
                                appItem.ProcOther_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcOther_SubmittedDate]);
                                appItem.ProcPaperboard_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcPaperboard_SubmittedDate]);
                                appItem.ProcRigidPlastic_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcRigidPlastic_SubmittedDate]);
                                appItem.ProcExternal_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcExternal_SubmittedDate]);
                                appItem.ProcSeasonal_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcSeasonal_SubmittedDate]);
                                appItem.ProcCoMan_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcCoMan_SubmittedDate]);
                                appItem.ProcNovelty_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcNovelty_SubmittedDate]);
                                appItem.ProcAncillary_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcAncillary_SubmittedBy]);
                                appItem.ProcCorrugated_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcCorrugated_SubmittedBy]);
                                appItem.ProcPurchased_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcPurchased_SubmittedBy]);
                                appItem.ProcFilm_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcFilm_SubmittedBy]);
                                appItem.ProcLabel_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcLabel_SubmittedBy]);
                                appItem.ProcMetal_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcMetal_SubmittedBy]);
                                appItem.ProcOther_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcOther_SubmittedBy]);
                                appItem.ProcPaperboard_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcPaperboard_SubmittedBy]);
                                appItem.ProcRigidPlastic_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcRigidPlastic_SubmittedBy]);
                                appItem.ProcExternal_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcExternal_SubmittedBy]);
                                appItem.ProcExternalAncillary_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcAncillary_SubmittedDate]);
                                appItem.ProcExternalCorrugated_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcCorrugated_SubmittedDate]);
                                appItem.ProcExternalPurchased_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcPurchased_SubmittedDate]);
                                appItem.ProcExternalFilm_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcFilm_SubmittedDate]);
                                appItem.ProcExternalLabel_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcLabel_SubmittedDate]);
                                appItem.ProcExternalMetal_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcMetal_SubmittedDate]);
                                appItem.ProcExternalOther_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcOther_SubmittedDate]);
                                appItem.ProcExternalPaperboard_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcPaperboard_SubmittedDate]);
                                appItem.ProcExternalRigidPlastic_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcRigidPlastic_SubmittedDate]);
                                appItem.ProcExternalAncillary_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcAncillary_SubmittedBy]);
                                appItem.ProcExternalCorrugated_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcCorrugated_SubmittedBy]);
                                appItem.ProcExternalPurchased_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcPurchased_SubmittedBy]);
                                appItem.ProcExternalFilm_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcFilm_SubmittedBy]);
                                appItem.ProcExternalLabel_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcLabel_SubmittedBy]);
                                appItem.ProcExternalMetal_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcMetal_SubmittedBy]);
                                appItem.ProcExternalOther_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcOther_SubmittedBy]);
                                appItem.ProcExternalPaperboard_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcPaperboard_SubmittedBy]);
                                appItem.ProcExternalRigidPlastic_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcRigidPlastic_SubmittedBy]);
                                string procSeasonalSubmittedBy = "";
                                if (!string.IsNullOrEmpty(Convert.ToString(item2[ApprovalListFields.ProcAncillary_SubmittedBy])))
                                {
                                    procSeasonalSubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcAncillary_SubmittedBy]);
                                }
                                else if (!string.IsNullOrEmpty(Convert.ToString(item2[ApprovalListFields.ProcCorrugated_SubmittedBy])))
                                {
                                    procSeasonalSubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcCorrugated_SubmittedBy]);
                                }
                                else if (!string.IsNullOrEmpty(Convert.ToString(item2[ApprovalListFields.ProcPurchased_SubmittedBy])))
                                {
                                    procSeasonalSubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcPurchased_SubmittedBy]);
                                }
                                else if (!string.IsNullOrEmpty(Convert.ToString(item2[ApprovalListFields.ProcFilm_SubmittedBy])))
                                {
                                    procSeasonalSubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcFilm_SubmittedBy]);
                                }
                                else if (!string.IsNullOrEmpty(Convert.ToString(item2[ApprovalListFields.ProcLabel_SubmittedBy])))
                                {
                                    procSeasonalSubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcLabel_SubmittedBy]);
                                }
                                else if (!string.IsNullOrEmpty(Convert.ToString(item2[ApprovalListFields.ProcMetal_SubmittedBy])))
                                {
                                    procSeasonalSubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcMetal_SubmittedBy]);
                                }
                                else if (!string.IsNullOrEmpty(Convert.ToString(item2[ApprovalListFields.ProcOther_SubmittedBy])))
                                {
                                    procSeasonalSubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcOther_SubmittedBy]);
                                }
                                else if (!string.IsNullOrEmpty(Convert.ToString(item2[ApprovalListFields.ProcPaperboard_SubmittedBy])))
                                {
                                    procSeasonalSubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcPaperboard_SubmittedBy]);
                                }
                                else if (!string.IsNullOrEmpty(Convert.ToString(item2[ApprovalListFields.ProcRigidPlastic_SubmittedBy])))
                                {
                                    procSeasonalSubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcRigidPlastic_SubmittedBy]);
                                }
                                else if (!string.IsNullOrEmpty(Convert.ToString(item2[ApprovalListFields.ProcSeasonal_SubmittedBy])))
                                {
                                    procSeasonalSubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcSeasonal_SubmittedBy]);
                                }
                                if (!string.IsNullOrEmpty(procSeasonalSubmittedBy) && procSeasonalSubmittedBy != DateTime.MinValue.ToString())
                                {
                                    appItem.ProcSeasonal_SubmittedBy = procSeasonalSubmittedBy;
                                }

                                if (string.IsNullOrEmpty(appItem.ProcExternal_SubmittedBy))
                                {
                                    string procExternalSubmittedBy = "";
                                    if (!string.IsNullOrEmpty(Convert.ToString(item2[ApprovalListFields.ProcExternalAncillary_SubmittedBy])))
                                    {
                                        procExternalSubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcExternalAncillary_SubmittedBy]);
                                    }
                                    else if (!string.IsNullOrEmpty(Convert.ToString(item2[ApprovalListFields.ProcExternalCorrugated_SubmittedBy])))
                                    {
                                        procExternalSubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcExternalCorrugated_SubmittedBy]);
                                    }
                                    else if (!string.IsNullOrEmpty(Convert.ToString(item2[ApprovalListFields.ProcExternalPurchased_SubmittedBy])))
                                    {
                                        procExternalSubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcExternalPurchased_SubmittedBy]);
                                    }
                                    else if (!string.IsNullOrEmpty(Convert.ToString(item2[ApprovalListFields.ProcExternalFilm_SubmittedBy])))
                                    {
                                        procExternalSubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcExternalFilm_SubmittedBy]);
                                    }
                                    else if (!string.IsNullOrEmpty(Convert.ToString(item2[ApprovalListFields.ProcExternalLabel_SubmittedBy])))
                                    {
                                        procExternalSubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcExternalLabel_SubmittedBy]);
                                    }
                                    else if (!string.IsNullOrEmpty(Convert.ToString(item2[ApprovalListFields.ProcExternalMetal_SubmittedBy])))
                                    {
                                        procExternalSubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcExternalMetal_SubmittedBy]);
                                    }
                                    else if (!string.IsNullOrEmpty(Convert.ToString(item2[ApprovalListFields.ProcExternalOther_SubmittedBy])))
                                    {
                                        procExternalSubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcExternalOther_SubmittedBy]);
                                    }
                                    else if (!string.IsNullOrEmpty(Convert.ToString(item2[ApprovalListFields.ProcExternalPaperboard_SubmittedBy])))
                                    {
                                        procExternalSubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcExternalPaperboard_SubmittedBy]);
                                    }
                                    else if (!string.IsNullOrEmpty(Convert.ToString(item2[ApprovalListFields.ProcExternalRigidPlastic_SubmittedBy])))
                                    {
                                        procExternalSubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcExternalRigidPlastic_SubmittedBy]);
                                    }
                                    else if (!string.IsNullOrEmpty(Convert.ToString(item2[ApprovalListFields.ProcExternal_SubmittedBy])))
                                    {
                                        procExternalSubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcExternal_SubmittedBy]);
                                    }
                                    if (!string.IsNullOrEmpty(procExternalSubmittedBy))
                                    {
                                        appItem.ProcExternal_SubmittedBy = procExternalSubmittedBy;
                                    }
                                }

                                appItem.ProcCoMan_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcCoMan_SubmittedBy]);
                                appItem.ProcNovelty_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcNovelty_SubmittedBy]);

                                //BE QRC
                                appItem.BEQRC_StartDate = Convert.ToString(item2[ApprovalListFields.BEQRC_StartDate]);
                                appItem.BEQRC_ModifiedDate = Convert.ToString(item2[ApprovalListFields.BEQRC_ModifiedDate]);
                                appItem.BEQRC_ModifiedBy = Convert.ToString(item2[ApprovalListFields.BEQRC_ModifiedBy]);
                                appItem.BEQRC_SubmittedDate = Convert.ToString(item2[ApprovalListFields.BEQRC_SubmittedDate]);
                                appItem.BEQRC_SubmittedBy = Convert.ToString(item2[ApprovalListFields.BEQRC_SubmittedBy]);
                                //Estimated Pricing
                                appItem.EstPricing_StartDate = Convert.ToString(item2[ApprovalListFields.EstPricing_StartDate]);
                                appItem.EstPricing_ModifiedDate = Convert.ToString(item2[ApprovalListFields.EstPricing_ModifiedDate]);
                                appItem.EstPricing_ModifiedBy = Convert.ToString(item2[ApprovalListFields.EstPricing_ModifiedBy]);
                                appItem.EstPricing_SubmittedDate = Convert.ToString(item2[ApprovalListFields.EstPricing_SubmittedDate]);
                                appItem.EstPricing_SubmittedBy = Convert.ToString(item2[ApprovalListFields.EstPricing_SubmittedBy]);
                                //Estimated Bracket Pricing
                                appItem.EstBracketPricing_StartDate = Convert.ToString(item2[ApprovalListFields.EstBracketPricing_StartDate]);
                                appItem.EstBracketPricing_ModifiedDate = Convert.ToString(item2[ApprovalListFields.EstBracketPricing_ModifiedDate]);
                                appItem.EstBracketPricing_ModifiedBy = Convert.ToString(item2[ApprovalListFields.EstBracketPricing_ModifiedBy]);
                                appItem.EstBracketPricing_SubmittedDate = Convert.ToString(item2[ApprovalListFields.EstBracketPricing_SubmittedDate]);
                                appItem.EstBracketPricing_SubmittedBy = Convert.ToString(item2[ApprovalListFields.EstBracketPricing_SubmittedBy]);
                            }
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            return appItem;
        }

        public void UpdateAllApprovalItem(ApprovalListItem approvalListItem, string title)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);
                        SPListItem appItem = spList.GetItemById(approvalListItem.ApprovalListItemId);
                        if (appItem != null)
                        {
                            // IPF Fields
                            appItem[ApprovalListFields.IPF_SubmittedBy] = approvalListItem.IPF_SubmittedBy;
                            appItem[ApprovalListFields.IPF_SubmittedDate] = approvalListItem.IPF_SubmittedDate;

                            appItem.Update();
                            spWeb.AllowUnsafeUpdates = false;
                        }
                    }
                }
            });
        }

        #region IPF Approvals
        public int InsertApprovalItem(ApprovalListItem approvalListItem, string title)
        {
            int id = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);

                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + approvalListItem.CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;
                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                            return;

                        SPListItem appItem = spList.AddItem();

                        appItem["Title"] = title;
                        appItem[ApprovalListFields.CompassListItemId] = approvalListItem.CompassListItemId;
                        // IPF Fields
                        appItem[ApprovalListFields.IPF_SubmittedBy] = approvalListItem.IPF_SubmittedBy;
                        appItem[ApprovalListFields.IPF_SubmittedDate] = approvalListItem.IPF_SubmittedDate;

                        appItem.Update();
                        spWeb.AllowUnsafeUpdates = false;

                        id = appItem.ID;
                    }
                }
            });
            return id;
        }
        #endregion

        #region Other Workflow State Changes
        public void UpdateOnHoldApprovalItem(int compassListItemId)
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
                            SPListItem appItem = compassItemCol[0];
                            if (appItem != null)
                            {
                                // On Hold Fields
                                appItem[ApprovalListFields.OnHold_ModifiedDate] = DateTime.Now;
                                appItem[ApprovalListFields.OnHold_ModifiedBy] = SPContext.Current.Web.CurrentUser.Name;

                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        public void UpdatePreProductionApprovalItem(int compassListItemId)
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
                            SPListItem appItem = compassItemCol[0];
                            if (appItem != null)
                            {
                                // Pre-Production Fields
                                appItem[ApprovalListFields.PreProduction_ModifiedDate] = DateTime.Now;
                                appItem[ApprovalListFields.PreProduction_ModifiedBy] = SPContext.Current.Web.CurrentUser.Name;

                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        public void UpdateCompletedApprovalItem(int compassListItemId)
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
                            SPListItem appItem = compassItemCol[0];
                            if (appItem != null)
                            {
                                // Completed Fields
                                appItem[ApprovalListFields.Completed_ModifiedDate] = DateTime.Now;
                                appItem[ApprovalListFields.Completed_ModifiedBy] = SPContext.Current.Web.CurrentUser.Name;

                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        public void UpdateCancelledApprovalItem(int compassListItemId)
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
                            SPListItem appItem = compassItemCol[0];
                            if (appItem != null)
                            {
                                // Cancelled Fields
                                appItem[ApprovalListFields.Cancelled_ModifiedDate] = DateTime.Now;
                                appItem[ApprovalListFields.Cancelled_ModifiedBy] = SPContext.Current.Web.CurrentUser.Name;
                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        #endregion

        #region Helper Methods
        private void SetStartDate(ApprovalListItem approvalItem, string title)
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
                        SPListItem item = null;
                        bool bUpdated = false;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            item = compassItemCol[0];
                            if (item == null)
                            {
                                // Couldn't find a logging item, so insert a new record!
                                approvalItem.IPF_SubmittedDate = DateTime.Now.ToString();
                                int id = InsertApprovalItem(approvalItem, title);
                                item = spList.GetItemById(id);
                            }
                        }
                        else
                        {
                            // Couldn't find a logging item, so insert a new record!
                            approvalItem.IPF_SubmittedDate = DateTime.Now.ToString();
                            int id = InsertApprovalItem(approvalItem, title);
                            item = spList.GetItemById(id);
                        }

                        if (item != null)
                        {
                            // Update any start dates that have been set
                            if ((!string.IsNullOrEmpty(approvalItem.GRAPHICS_StartDate)) && (item[ApprovalListFields.GRAPHICS_StartDate] == null))
                            {
                                item[ApprovalListFields.GRAPHICS_StartDate] = approvalItem.GRAPHICS_StartDate;
                                bUpdated = true;
                            }

                            if ((!string.IsNullOrEmpty(approvalItem.InitialCosting_StartDate)) && (item[ApprovalListFields.InitialCosting_StartDate] == null))
                            {
                                item[ApprovalListFields.InitialCosting_StartDate] = approvalItem.InitialCosting_StartDate;
                                bUpdated = true;
                            }

                            if ((!string.IsNullOrEmpty(approvalItem.BOMSetupPE_StartDate)) && (item[ApprovalListFields.BOMSetupPE_StartDate] == null))
                            {
                                item[ApprovalListFields.BOMSetupPE_StartDate] = approvalItem.BOMSetupPE_StartDate;
                                bUpdated = true;
                            }

                            if ((!string.IsNullOrEmpty(approvalItem.BOMSetupPE2_StartDate)) && (item[ApprovalListFields.BOMSetupPE2_StartDate] == null))
                            {
                                item[ApprovalListFields.BOMSetupPE2_StartDate] = approvalItem.BOMSetupPE2_StartDate;
                                bUpdated = true;
                            }

                            if ((!string.IsNullOrEmpty(approvalItem.BOMSetupProc_StartDate)) && (item[ApprovalListFields.BOMSetupProc_StartDate] == null))
                            {
                                item[ApprovalListFields.BOMSetupProc_StartDate] = approvalItem.BOMSetupProc_StartDate;
                                bUpdated = true;
                            }

                            if ((!string.IsNullOrEmpty(approvalItem.OBMReview1_StartDate)) && (item[ApprovalListFields.OBMReview1_StartDate] == null))
                            {
                                item[ApprovalListFields.OBMReview1_StartDate] = approvalItem.OBMReview1_StartDate;
                                bUpdated = true;
                            }

                            if ((!string.IsNullOrEmpty(approvalItem.Distribution_StartDate)) && (item[ApprovalListFields.Distribution_StartDate] == null))
                            {
                                item[ApprovalListFields.Distribution_StartDate] = approvalItem.Distribution_StartDate;
                                bUpdated = true;
                            }

                            if ((!string.IsNullOrEmpty(approvalItem.Operations_StartDate)) && (item[ApprovalListFields.Operations_StartDate] == null))
                            {
                                item[ApprovalListFields.Operations_StartDate] = approvalItem.Operations_StartDate;
                                bUpdated = true;
                            }

                            if ((!string.IsNullOrEmpty(approvalItem.QA_StartDate)) && (item[ApprovalListFields.QA_StartDate] == null))
                            {
                                item[ApprovalListFields.QA_StartDate] = approvalItem.QA_StartDate;
                                bUpdated = true;
                            }

                            if ((!string.IsNullOrEmpty(approvalItem.SAPInitialSetup_StartDate)) && (item[ApprovalListFields.SAPInitialSetup_StartDate] == null))
                            {
                                item[ApprovalListFields.SAPInitialSetup_StartDate] = approvalItem.SAPInitialSetup_StartDate;
                                bUpdated = true;
                            }
                            if ((!string.IsNullOrEmpty(approvalItem.PrelimSAPInitialSetup_StartDate)) && (item[ApprovalListFields.PrelimSAPInitialSetup_StartDate] == null))
                            {
                                item[ApprovalListFields.PrelimSAPInitialSetup_StartDate] = approvalItem.PrelimSAPInitialSetup_StartDate;
                                bUpdated = true;
                            }

                            if ((!string.IsNullOrEmpty(approvalItem.OBMReview2_StartDate)) && (item[ApprovalListFields.OBMReview2_StartDate] == null))
                            {
                                item[ApprovalListFields.OBMReview2_StartDate] = approvalItem.OBMReview2_StartDate;
                                bUpdated = true;
                            }

                            if ((!string.IsNullOrEmpty(approvalItem.TradePromo_StartDate)) && (item[ApprovalListFields.TradePromo_StartDate] == null))
                            {
                                item[ApprovalListFields.TradePromo_StartDate] = approvalItem.TradePromo_StartDate;
                                bUpdated = true;
                            }
                            if ((!string.IsNullOrEmpty(approvalItem.EstPricing_StartDate)) && (item[ApprovalListFields.EstPricing_StartDate] == null))
                            {
                                item[ApprovalListFields.EstPricing_StartDate] = approvalItem.EstPricing_StartDate;
                                bUpdated = true;
                            }
                            if ((!string.IsNullOrEmpty(approvalItem.EstBracketPricing_StartDate)) && (item[ApprovalListFields.EstBracketPricing_StartDate] == null))
                            {
                                item[ApprovalListFields.EstBracketPricing_StartDate] = approvalItem.EstBracketPricing_StartDate;
                                bUpdated = true;
                            }

                            if (item[ApprovalListFields.OBMReview3_StartDate] == null)
                            {
                                item[ApprovalListFields.OBMReview3_StartDate] = approvalItem.OBMReview3_StartDate;
                                bUpdated = true;
                            }

                            if (item[ApprovalListFields.SAPBOMSetup_StartDate] == null)
                            {
                                item[ApprovalListFields.SAPBOMSetup_StartDate] = approvalItem.SAPBOMSetup_StartDate;
                                bUpdated = true;
                            }

                            if (item[ApprovalListFields.FGPackSpec_StartDate] == null)
                            {
                                item[ApprovalListFields.FGPackSpec_StartDate] = approvalItem.FGPackSpec_StartDate;
                                bUpdated = true;
                            }

                            if (item[ApprovalListFields.CompCostCorrPaper_StartDate] == null)
                            {
                                item[ApprovalListFields.CompCostCorrPaper_StartDate] = approvalItem.CompCostCorrPaper_StartDate;
                                bUpdated = true;
                            }

                            if (item[ApprovalListFields.CompCostFLRP_StartDate] == null)
                            {
                                item[ApprovalListFields.CompCostFLRP_StartDate] = approvalItem.CompCostFLRP_StartDate;
                                bUpdated = true;
                            }

                            if (item[ApprovalListFields.CompCostSeasonal_StartDate] == null)
                            {
                                item[ApprovalListFields.CompCostSeasonal_StartDate] = approvalItem.CompCostSeasonal_StartDate;
                                bUpdated = true;
                            }

                            if (bUpdated)
                                item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        #endregion
    }
}
