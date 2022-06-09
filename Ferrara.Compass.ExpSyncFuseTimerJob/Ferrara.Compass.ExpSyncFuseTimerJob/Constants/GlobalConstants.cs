using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.ExpSyncFuseTimerJob.Constants
{
    public class GlobalConstants
    {
        public const string JOBNAME = "PMT Export Sync Fuse Task Timer";
        public const string DOCLIBRARY_CompassLibraryName = "Compass Documents";
        public const string DOCLIBRARYTemplatesName = "Templates";
        public const string PACKAGINGTYPE_FGBOM = "FGBOM";
        public const string COMPONENTTYPE_CandySemi = "Candy Semi";
        public const string WORKFLOWPHASE_Cancelled = "Cancelled";

        public const string LIST_CompassListName = "Compass List";
        public const string LIST_CompassPackMeasurementsListName = "Compass Pack Measurements List";
        public const string LIST_PackagingItemListName = "Compass Packaging Item List";
        public const string LIST_SAPMaterialMasterListName = "SAP Material Master List";
        public const string LIST_LogsListName = "Logs List";

        public const string EXP_SYNC_TEMPLATE_NAME = "FUSE_Supplier_Template.xlsx";
        public const string EXP_SYNC_FILENAME = "FUSE_Supplier{compassId}.xlsx";
        public const int EXP_SYNC_ROW_HDR_ITM = 3;
        public const int EXP_SYNC_ROW_HDR_PUB = 2;
        public const int EXP_SYNC_ROW_HDR_LNK = 2;
        public const int EXP_SYNC_ROW_STRT_ITM = 4;
        public const int EXP_SYNC_ROW_STRT_PUB = 3;
        public const int EXP_SYNC_ROW_STRT_LNK = 3;
        public const string EXP_SYNC_SHT_ITM = "FS_Item";
        public const string EXP_SYNC_SHT_PUB = "FS_Publication";
        public const string EXP_SYNC_SHEET_LNK = "FS_Link";
        
        public const string EXP_SYNC_ITM_ItemID = "gtin";
        public const string EXP_SYNC_ITM_ItemName = "gtinName";
        public const string EXP_SYNC_ITM_BrandName = "brandName";
        public const string EXP_SYNC_ITM_Depth = "depth";
        public const string EXP_SYNC_ITM_Height = "height";
        public const string EXP_SYNC_ITM_Width = "width";
        public const string EXP_SYNC_ITM_GrossWeight = "grossWeight";
        public const string EXP_SYNC_ITM_NetWeight = "netWeight";
        public const string EXP_SYNC_ITM_GS1TradeItemIDKeyValue = "gs1TradeItemIdentificationKey/value";
        public const string EXP_SYNC_ITM_ShortDescription = "shortDescription";
        public const string EXP_SYNC_ITM_ProductDescription = "productDescription";
        public const string EXP_SYNC_ITM_FunctionalName = "functionalName";
        public const string EXP_SYNC_ITM_Volume = "volume";
        public const string EXP_SYNC_ITM_QtyofNextLevelItem = "totalQuantityOfNextLowerTradeItem";
        public const string EXP_SYNC_ITM_NumberofItemsinaCompleteLayerGTINPalletTi = "ti";
        public const string EXP_SYNC_ITM_NumberofCompleteLayersContainedinItemGTINPalletHi = "hi";
        public const string EXP_SYNC_ITM_AlternateItemIdentificationId = "alternateItemIdentification/id";
        public const string EXP_SYNC_ITM_MinProductLifespanfromProduction = "minimumTradeItemLifespanFromProduction";
        public const string EXP_SYNC_ITM_StartAvailabilityDate = "startAvailabilityDate";
        public const string EXP_SYNC_ITM_EffectiveDate = "effectiveDate";

        public const string EXP_SYNC_PUB_ItemID = "Item ID";
        public const string EXP_SYNC_PUB_PublishDate = "Publish Date";

        public const string EXP_SYNC_LKN_ParentItemID = "Parent Item ID";
        public const string EXP_SYNC_LKN_ChildItemID = "Child Item ID";
        public const string EXP_SYNC_LKN_QtyofChildItem = "Qty of Child Item";
    }
}
