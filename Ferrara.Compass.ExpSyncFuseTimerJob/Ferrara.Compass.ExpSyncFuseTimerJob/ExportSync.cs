using System;
using System.Collections.Generic;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;
using Ferrara.Compass.ExpSyncFuseTimerJob.Services;
using Ferrara.Compass.ExpSyncFuseTimerJob.Constants;
using Ferrara.Compass.ExpSyncFuseTimerJob.Models;

namespace Ferrara.Compass.ExpSyncFuseTimerJob
{
    public class ExportSync : SPJobDefinition
    {
        LoggerService logger;
        HashSet<string> seenUids = new HashSet<string>();
        SPWeb web = null;
        public ExportSync() { }
        public ExportSync(string jobName, SPWebApplication webapp) : base(jobName, webapp, null, SPJobLockType.Job)
        {
            this.Title = jobName;
        }
        public ExportSync(string jobName, SPService service, SPServer server, SPJobLockType targetType) : base(jobName, service, server, targetType)
        {
            this.Title = jobName;
        }
        public override void Execute(Guid targetInstanceId)
        {
            byte[] fileContent;
            string projectNumber;
            Dictionary<string, string> publishRow, linkRow;
            List<Dictionary<string, string>> itemRows;
            List<int> projectIds;
            SPSite site;
            SPWebApplication webApp = this.Parent as SPWebApplication;
            ItemProposalService proposalService;
            BillOfMaterialsService BOMservice;
            PackagingItemService packagingService;
            SAPmaterialMaster sapMM;
            ExcelExportSyncService excelService;

            site = webApp.Sites[0];
            logger = new LoggerService(site);
            proposalService = new ItemProposalService(site);
            BOMservice = new BillOfMaterialsService(site);
            packagingService = new PackagingItemService(site);
            sapMM = new SAPmaterialMaster(site);
            excelService = new ExcelExportSyncService(site, logger);
            publishRow = new Dictionary<string, string>();
            linkRow = new Dictionary<string, string>();
            itemRows = new List<Dictionary<string, string>>();
            projectIds = sapMM.GetProjectIds();
            foreach (int projectId in projectIds)
            {
                projectNumber = GetExportData(proposalService, BOMservice, packagingService, projectId, ref itemRows, ref publishRow, ref linkRow);
                fileContent = excelService.WriteToFile(itemRows, publishRow, linkRow);
                excelService.saveFileToDocLibrary(projectId, projectNumber, fileContent);
            }
        }
        private string GetExportData(ItemProposalService proposalService, BillOfMaterialsService BOMservice,
            PackagingItemService packagingService, int compassId, ref List<Dictionary<string, string>> itemRows,
            ref Dictionary<string, string> publishRow, ref Dictionary<string, string> linkRow)
        {
            ItemProposalItem proposalItem;
            CompassPackMeasurementsItem measure;
            Dictionary<string, string> values;
            LogEntry entry;
            string todayDate, parentDescription, childDescription = "", onz = "";
            todayDate = DateTime.Now.ToString("s");
            try
            {
                proposalItem = proposalService.GetItemProposalItem(compassId);
                measure = BOMservice.GetPackMeasurementsItem(compassId, 0, GlobalConstants.PACKAGINGTYPE_FGBOM);
                parentDescription = proposalItem.SAPDescription;
                GetDescriptions(35, ref parentDescription, ref childDescription, ref onz);
                itemRows = new List<Dictionary<string, string>>();
                values = new Dictionary<string, string>();
                values.Add(GlobalConstants.EXP_SYNC_ITM_ItemID, proposalItem.CaseUCC);
                values.Add(GlobalConstants.EXP_SYNC_ITM_ItemName, parentDescription);
                values.Add(GlobalConstants.EXP_SYNC_ITM_BrandName, proposalItem.MaterialGroup1Brand);
                values.Add(GlobalConstants.EXP_SYNC_ITM_Depth, FormatDecimal(measure.CaseDimensionLength, 2));
                values.Add(GlobalConstants.EXP_SYNC_ITM_Height, FormatDecimal(measure.CaseDimensionHeight, 2));
                values.Add(GlobalConstants.EXP_SYNC_ITM_Width, FormatDecimal(measure.CaseDimensionWidth, 2));
                values.Add(GlobalConstants.EXP_SYNC_ITM_GrossWeight, FormatDecimal(measure.CaseGrossWeight, 2));
                values.Add(GlobalConstants.EXP_SYNC_ITM_NetWeight, FormatDecimal(measure.CaseNetWeight, 2));
                values.Add(GlobalConstants.EXP_SYNC_ITM_GS1TradeItemIDKeyValue, proposalItem.CaseUCC);
                values.Add(GlobalConstants.EXP_SYNC_ITM_ShortDescription, parentDescription);
                values.Add(GlobalConstants.EXP_SYNC_ITM_ProductDescription, parentDescription);
                values.Add(GlobalConstants.EXP_SYNC_ITM_FunctionalName, parentDescription);
                values.Add(GlobalConstants.EXP_SYNC_ITM_Volume, FormatDecimal(measure.CaseCube, 2));
                values.Add(GlobalConstants.EXP_SYNC_ITM_QtyofNextLevelItem, FormatDecimal(proposalItem.RetailSellingUnitsBaseUOM, 2));
                values.Add(GlobalConstants.EXP_SYNC_ITM_NumberofItemsinaCompleteLayerGTINPalletTi, FormatDecimal(measure.CasesPerLayer, 2));
                values.Add(GlobalConstants.EXP_SYNC_ITM_NumberofCompleteLayersContainedinItemGTINPalletHi, FormatDecimal(measure.LayersPerPallet, 2));
                values.Add(GlobalConstants.EXP_SYNC_ITM_AlternateItemIdentificationId, proposalItem.SAPItemNumber);
                values.Add(GlobalConstants.EXP_SYNC_ITM_MinProductLifespanfromProduction, GetShelfLife(packagingService, compassId));
                values.Add(GlobalConstants.EXP_SYNC_ITM_StartAvailabilityDate, todayDate);
                values.Add(GlobalConstants.EXP_SYNC_ITM_EffectiveDate, todayDate);
                itemRows.Add(values);
                values = new Dictionary<string, string>(1);
                values.Add(GlobalConstants.EXP_SYNC_ITM_ItemID, string.Empty);
                itemRows.Add(values);
                values = new Dictionary<string, string>();
                values.Add(GlobalConstants.EXP_SYNC_ITM_ItemID, proposalItem.UnitUPC);
                values.Add(GlobalConstants.EXP_SYNC_ITM_ItemName, childDescription);
                values.Add(GlobalConstants.EXP_SYNC_ITM_BrandName, proposalItem.MaterialGroup1Brand);
                values.Add(GlobalConstants.EXP_SYNC_ITM_Depth, FormatDecimal(measure.UnitDimensionLength, 2));
                values.Add(GlobalConstants.EXP_SYNC_ITM_Height, FormatDecimal(measure.UnitDimensionHeight, 2));
                values.Add(GlobalConstants.EXP_SYNC_ITM_Width, FormatDecimal(measure.UnitDimensionWidth, 2));
                values.Add(GlobalConstants.EXP_SYNC_ITM_GrossWeight, onz);
                values.Add(GlobalConstants.EXP_SYNC_ITM_NetWeight, FormatDecimal(measure.NetUnitWeight, 2));
                values.Add(GlobalConstants.EXP_SYNC_ITM_GS1TradeItemIDKeyValue, proposalItem.UnitUPC);
                values.Add(GlobalConstants.EXP_SYNC_ITM_ShortDescription, childDescription);
                values.Add(GlobalConstants.EXP_SYNC_ITM_ProductDescription, childDescription);
                values.Add(GlobalConstants.EXP_SYNC_ITM_FunctionalName, childDescription);
                values.Add(GlobalConstants.EXP_SYNC_ITM_StartAvailabilityDate, todayDate);
                values.Add(GlobalConstants.EXP_SYNC_ITM_EffectiveDate, todayDate);
                itemRows.Add(values);
                publishRow = new Dictionary<string, string>();
                publishRow.Add(GlobalConstants.EXP_SYNC_PUB_ItemID, proposalItem.CaseUCC);
                publishRow.Add(GlobalConstants.EXP_SYNC_PUB_PublishDate, todayDate);
                linkRow = new Dictionary<string, string>();
                linkRow.Add(GlobalConstants.EXP_SYNC_LKN_ParentItemID, proposalItem.CaseUCC);
                linkRow.Add(GlobalConstants.EXP_SYNC_LKN_ChildItemID, proposalItem.UnitUPC);
                linkRow.Add(GlobalConstants.EXP_SYNC_LKN_QtyofChildItem, FormatDecimal(proposalItem.RetailSellingUnitsBaseUOM, 2));
                return proposalItem.ProjectNumber;
            }
            catch(Exception ex)
            {
                entry = new LogEntry();
                entry.Category = "CriticalError";
                entry.Form = GlobalConstants.JOBNAME;
                entry.Message = ex.StackTrace.ToString();
                entry.Method = "saveFileToDocLibrary";
                entry.Title = ex.ToString();
                logger.InsertLog(entry);
                throw ex;
            }
        }
        public static string FormatDecimal(double value, int numPlaces)
        {
            if (value == -9999)
                return string.Empty;

            return value.ToString("N" + numPlaces.ToString());
        }
        private string GetShelfLife(PackagingItemService packagingService, int compassId)
        {
            List<PackagingItem> packagingItems;
            int current, shelfLife = int.MaxValue;
            packagingItems = packagingService.GetCandySemiItemsForProject(compassId);
            foreach (PackagingItem packItem in packagingItems)
            {
                if (packItem.ShelfLife == "")
                    continue;
                current = int.Parse(packItem.ShelfLife);
                if (current < shelfLife)
                    shelfLife = current;
            }
            return shelfLife == int.MaxValue ? "" : shelfLife.ToString();
        }
        private string GetChildDescription(string description, ref string onz)
        {
            int slashIdx, ozIdx, w;
            char ch;
            bool otherCharsFound;
            otherCharsFound = false;
            slashIdx = description.IndexOf('/');
            if (slashIdx <= 0 || slashIdx == description.Length - 1)
                return description;
            for (w = slashIdx - 1; w > 0; w--)
            {
                ch = description[w];
                if (ch == ' ')
                {
                    if (otherCharsFound)
                        break;
                }
                else
                    otherCharsFound = true;
            }
            if (w > 0)
            {
                ozIdx = description.ToLower().IndexOf("oz", slashIdx);
                if (ozIdx > -1)
                    onz = description.Substring(slashIdx + 1, ozIdx - slashIdx - 1).Trim();
                return description.Substring(0, w).Trim() + " " + description.Substring(slashIdx + 1);
            }
            return description;
        }
        private void GetDescriptions(int maxLength, ref string parentDescription, ref string childDescription, ref string onz)
        {
            int slashIdx, ozIdx, endNameIdx, w;
            char ch;
            bool digitsFound;
            string nameOnly, quantity, postfixParent, postfixChild;
            digitsFound = false;
            slashIdx = parentDescription.LastIndexOf('/');
            ozIdx = parentDescription.ToLower().LastIndexOf("oz");
            onz = slashIdx > -1 && ozIdx > -1 && ozIdx > slashIdx ? parentDescription.Substring(slashIdx + 1, ozIdx - slashIdx - 1).Trim() : "";
            if (slashIdx <= 0)
                slashIdx = parentDescription.Length;
            for (w = slashIdx - 1; w > 0; w--)
            {
                ch = parentDescription[w];
                if (ch == ' ')
                {
                    if (digitsFound)
                        break;
                }
                else
                    if (ch >= '0' && ch <= '9')
                    digitsFound = true;
            }
            endNameIdx = w;
            if (endNameIdx > 0)
            {
                nameOnly = parentDescription.Substring(0, endNameIdx).Trim();
                quantity = parentDescription.Substring(endNameIdx, slashIdx - endNameIdx).Trim();
                postfixParent = " " + quantity + '/' + onz + " oz";
                postfixChild = " " + onz + " oz";
                parentDescription = ShrinkName(nameOnly, postfixParent, maxLength) + postfixParent;
                childDescription = ShrinkName(nameOnly, postfixChild, maxLength) + postfixChild;
            }
            else
            {
                parentDescription = ShrinkName(parentDescription, "", maxLength);
                childDescription = ShrinkName(parentDescription, "", maxLength);
            }
        }
        private string ShrinkName(string nameOnly, string postfix, int maxLength)
        {
            int w, previousLength;
            do
            {
                previousLength = nameOnly.Length;
                nameOnly = nameOnly.Replace("  ", " ");
            }
            while (nameOnly.Length < previousLength);
            maxLength -= postfix.Length;
            for (w = nameOnly.Length - 1; w > 0 && nameOnly.Length > maxLength; w--)
                if ("AEIOU".IndexOf(nameOnly[w]) > -1 || "aeiou".IndexOf(nameOnly[w]) > -1)
                    nameOnly = nameOnly.Remove(w, 1);
            if (nameOnly.Length > maxLength)
                return nameOnly.Substring(0, maxLength);
            return nameOnly;
        }
    }
}
