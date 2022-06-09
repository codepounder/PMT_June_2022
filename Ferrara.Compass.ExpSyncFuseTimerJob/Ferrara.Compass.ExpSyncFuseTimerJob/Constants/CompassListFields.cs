using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.ExpSyncFuseTimerJob.Constants
{
    public class CompassListFields
    {
        public const string Title = "Title" ;
        public const string CompassListItemId = "CompassListItemId";
        public const string ChangeLink = "ChangeLink";
        public const string CopyLink = "CopyLink";
        public const string WorkflowStatusLink = "WorkflowStatuLink";
        public const string CommercializationLink = "CommercializationLink";
        public const string LastUpdatedFormName = "LastUpdatedFormName";
        public const string WorkflowPhase = "WorkflowPhase";
        public const string ProjectDashboardComments = "ProjectDashboardComments";
        public const string TimelineType = "TimelineType";
        public const string LastUpdatedSAPTaskName = "LastUpdatedSAPTaskName";

        public const string ChangeLink_DisplayName = "Change Link";
        public const string CopyLink_DisplayName = "Copy Link";
        public const string WorkflowStatusLink_DisplayName = "Workflow Status Link";
        public const string CommercializationLink_DisplayName = "Commercialization Link";
        public const string LastUpdatedFormName_DisplayName = "Last Updated Form";
        public const string WorkflowPhase_DisplayName = "Workflow Phase";
        public const string ProjectDashboardComments_DisplayName = "Project Dashboard Comments";
        public const string TimelineType_DisplayName = "Timeline Type";
        public const string LastUpdatedSAPTaskName_DisplayName = "Last Updated SAP Task";

        #region IPF Fields
        public const string SubmittedDate = "SubmittedDate";
        public const string ProjectNumber = "ProjectNumber";
        public const string SAPItemNumber = "SAPItemNumber";
        public const string SAPDescription = "SAPDescription";
        public const string ProjectType = "ProjectType";
        public const string FirstShipDate = "FirstShipDate";
        public const string RevisedFirstShipDate = "RevisedFirstShipDate";
        public const string ProductHierarchyLevel1 = "ProductHierarchyLevel1";
        public const string ProductHierarchyLevel2 = "ProductHierarchyLevel2";
        public const string ProjectNotes = "ProjectNotes";
        public const string ItemConcept = "ItemConcept";

        public const string Initiator = "Initiator";
        public const string InitiatorName = "InitiatorName";
        public const string BrandManager = "BrandManager";
        public const string BrandManagerName = "BrandManagerName";
        public const string OBM = "OBM";
        public const string OBMName = "OBMName";
        public const string ResearchDevelopmentLead = "ResearchDevelopmentLead";
        public const string ResearchDevelopmentLeadName = "ResearchDevelopmentLeadName";

        public const string TBDIndicator = "TBDIndicator";

        public const string NewFormula = "NewFormula";
        public const string Organic = "Organic";
        public const string DTVProject = "DTVProject";
        public const string MfgLocationChange = "MfgLocationChange";
        public const string ServingSizeWeightChange = "ServingSizeWeightChange";
        public const string ReplacementForItemNumber = "ReplacementForItemNumber";

        public const string Last12MonthSales = "Last12MonthSales";
        public const string AnnualProjectedDollars = "AnnualProjectedDollars";
        public const string Month1ProjectedDollars = "Month1ProjectedDollars";
        public const string Month2ProjectedDollars = "Month2ProjectedDollars";
        public const string Month3ProjectedDollars = "Month3ProjectedDollars";
        public const string ExpectedGrossMarginPercent = "ExpectedGrossMarginPercent";
        public const string RevisedGrossMarginPercent = "RevisedGrossMarginPercent";
        
        public const string TruckLoadPricePerSellingUnit = "TruckLoadPricePerSellingUnit";
        public const string AnnualProjectedUnits = "AnnualProjectedUnits";
        public const string Month1ProjectedUnits = "Month1ProjectedUnits";
        public const string Month2ProjectedUnits = "Month2ProjectedUnits";
        public const string Month3ProjectedUnits = "Month3ProjectedUnits";

        public const string CustomerSpecific = "CustomerSpecific";
        public const string Customer = "Customer";
        public const string CustomerSpecificLotCode = "CustomerSpecificLotCode";
        public const string Channel = "Channel";
        public const string SoldOutsideUSA = "SoldOutsideUSA";
        public const string CountryOfSale = "CountryOfSale";
        public const string MakeCountryOfOrigin = "MakeCountryOfOrigin";

        public const string MaterialGroup1Brand = "MaterialGroup1Brand";
        public const string MaterialGroup4ProductForm = "MaterialGroup4ProductForm";
        public const string MaterialGroup5PackType = "MaterialGroup5PackType";

        public const string TotalQuantityUnitsInDisplay = "TotalQuantityUnitsInDisplay";
        public const string FGItemNumberInDisplay = "FGItemNumberInDisplay";
        public const string FGItemDescription = "FGItemDescription";
        public const string FGItemQuantity = "FGItemQuantity";

        public const string RequireNewUPCUCC = "RequireNewUPCUCC";
        public const string RequireNewUnitUPC = "RequireNewUnitUPC";
        public const string UnitUPC = "UnitUPC";
        public const string RequireNewDisplayBoxUPC = "RequireNewDisplayBoxUPC";
        public const string DisplayBoxUPC = "DisplayBoxUPC";
        public const string RequireNewCaseUCC = "RequireNewCaseUCC";
        public const string CaseUCC = "CaseUCC";
        public const string RequireNewPalletUCC = "RequireNewPalletUCC";
        public const string PalletUCC = "PalletUCC";
        public const string Flowthrough = "Flowthrough";

        public const string CandySemiNumber = "CandySemiNumber";
        public const string LikeFGItemNumber = "LikeFGItemNumber";
        public const string LikeFGItemDescription = "LikeFGItemDescription";
        public const string CaseType = "CaseType";
        public const string MarketClaimsLabelingRequirements = "MarketClaimsLabelingRequirements";
        public const string SAPBaseUOM = "SAPBaseUOM";
        public const string RetailSellingUnitsBaseUOM = "RetailSellingUnitsBaseUOM";
        public const string RetailUnitWieghtOz = "RetailUnitWieghtOz";
        public const string BaseUOMNetWeightLbs = "BaseUOMNetWeight";
        public const string FilmSubstrate = "FilmSubstrate";
        public const string ExpectedPackagingSwitchDate = "ExpectedPackagingSwitchDate";
        public const string PegHoleNeeded = "PegHoleNeeded";
        public const string ReasonForChange = "ReasonForChange";
        public const string GraphicsRequired = "GraphicsRequired";
        public const string NewComponentRequired = "NewComponentRequired";
        public const string ProjectTypeSubCategory = "ProjectTypeSubCategory";

        public const string SubmittedDate_DisplayName = "Submitted Date";
        public const string ProjectNumber_DisplayName = "Project Number";
        public const string SAPItemNumber_DisplayName = "SAP Item #";
        public const string SAPDescription_DisplayName = "SAP Description";
        public const string ProjectType_DisplayName = "Project Type";
        public const string FirstShipDate_DisplayName = "First Ship Date";
        public const string RevisedFirstShipDate_DisplayName = "Revised First Ship Date";
        public const string ProductHierarchyLevel1_DisplayName = "Product Hierarchy Level1";
        public const string ProductHierarchyLevel2_DisplayName = "Product Hierarchy Level2";
        public const string ProjectNotes_DisplayName = "Project Notes";
        public const string ItemConcept_DisplayName = "Item Concept";
        public const string Initiator_DisplayName = "Initiator";
        public const string BrandManager_DisplayName = "Brand Manager";
        public const string OBM_DisplayName = "OBM";
        public const string ResearchDevelopmentLead_DisplayName = "Research Development Lead";
        public const string InitiatorName_DisplayName = "Initiator Name";
        public const string BrandManagerName_DisplayName = "Brand Manager Name";
        public const string OBMName_DisplayName = "OBM Name";
        public const string ResearchDevelopmentLeadName_DisplayName = "Research Development Lead Name";
        public const string TBDIndicator_DisplayName = "TBD Indicator";
        public const string NewFormula_DisplayName = "New Formula?";
        public const string Organic_DisplayName = "Organic?";
        public const string DTVProject_DisplayName = "DTVProject?";
        public const string MfgLocationChange_DisplayName = "Mfg Location Change?";
        public const string ServingSizeWeightChange_DisplayName = "Serving Size-Weight Change?";
        public const string ReplacementForItemNumber_DisplayName = "Replacement For Item #";

        public const string Last12MonthSales_DisplayName = "Last 12 Month Sales";
        public const string AnnualProjectedDollars_DisplayName = "Annual Projected $";
        public const string ExpectedGrossMarginPercent_DisplayName = "Expected Gross Margin %";
        public const string RevisedGrossMarginPercent_DisplayName = "Revised Gross Margin %";
        public const string TruckLoadPricePerSellingUnit_DisplayName = "TruckLoad Price Per Selling Unit";
        public const string AnnualProjectedUnits_DisplayName = "Annual Projected Units";
        public const string CustomerSpecific_DisplayName = "Customer Specific?";
        public const string Customer_DisplayName = "Customer";
        public const string CustomerSpecificLotCode_DisplayName = "Customer Specific Lot Code";
        public const string Channel_DisplayName = "Channel";
        public const string SoldOutsideUSA_DisplayName = "Sold Outside USA?";
        public const string CountryOfSale_DisplayName = "Country of Sale";
        public const string MakeCountryOfOrigin_DisplayName = "Make Country Of Origin";
        public const string MaterialGroup1Brand_DisplayName = "Material Group1 Brand";
        public const string MaterialGroup4ProductForm_DisplayName = "Material Group4 ProductForm";
        public const string MaterialGroup5PackType_DisplayName = "Material Group5 PackType";
        public const string TotalQuantityUnitsInDisplay_DisplayName = "Total Quantity Units in Display";
        public const string FGItemNumberInDisplay_DisplayName = "FG Item Number in Display";
        public const string FGItemDescription_DisplayName = "FG Item Description";
        public const string FGItemQuantity_DisplayName = "FG Item Quantity";
        public const string RequireNewUPCUCC_DisplayName = "Require New UPC-UCC?";
        public const string RequireNewUnitUPC_DisplayName = "Require New Unit UPC?";
        public const string UnitUPC_DisplayName = "Unit UPC";
        public const string RequireNewDisplayBoxUPC_DisplayName = "Require New Display Box UPC?";
        public const string DisplayBoxUPC_DisplayName = "Display Box UPC";
        public const string RequireNewCaseUCC_DisplayName = "Require New Case UCC?";
        public const string CaseUCC_DisplayName = "Case UCC";
        public const string RequireNewPalletUCC_DisplayName = "Require New Pallet UCC?";
        public const string PalletUCC_DisplayName = "Pallet UCC";
        public const string Flowthrough_DisplayName = "Flowthrough";
        public const string CandySemiNumber_DisplayName = "Candy Semi #";
        public const string LikeFGItemNumber_DisplayName = "Like FG Item #";
        public const string LikeFGItemDescription_DisplayName = "Like FG Item Description";
        public const string CaseType_DisplayName = "Case Type";
        public const string MarketClaimsLabelingRequirements_DisplayName = "Market Claims-Labeling Requirements";
        public const string SAPBaseUOM_DisplayName = "SAP Base UOM";
        public const string RetailSellingUnitsBaseUOM_DisplayName = "Retail Selling Units Base UOM";
        public const string RetailUnitWieghtOz_DisplayName = "Retail Unit Wieght (oz)";
        public const string BaseUOMNetWeightLbs_DisplayName = "Base UOM Net Weight (lbs)";
        public const string FilmSubstrate_DisplayName = "Film Substrate";
        public const string PegHoleNeeded_DisplayName = "Peg Hole Needed?";
        public const string ExpectedPackagingSwitchDate_DisplayName = "Expected Packaging Switch Date";
        public const string Month1ProjectedDollars_DisplayName = "Month1 Projected $";
        public const string Month2ProjectedDollars_DisplayName = "Month2 Projected $";
        public const string Month3ProjectedDollars_DisplayName = "Month3 Projected $";
        public const string Month1ProjectedUnits_DisplayName = "Month1 Projected Units";
        public const string Month2ProjectedUnits_DisplayName = "Month2 Projected Units";
        public const string Month3ProjectedUnits_DisplayName = "Month3 Projected Units";
        public const string ReasonForChange_DisplayName = "Reason For Change";
        public const string GraphicsRequired_DisplayName = "Graphics Required";
        public const string NewComponentRequired_DisplayName = "New Component Required";
        public const string ProjectTypeSubCategory_DisplayName = "Project Type SubCategory";

        #endregion

        #region Initial Approval Review Fields
        #endregion

        #region Trade Promo Group Fields
        public const string MaterialGroup2Pricing = "MaterialGroup2Pricing";

        public const string MaterialGroup2Pricing_DisplayName = "Material Group2 Pricing";
        #endregion

        #region OPS Fields
        public const string ManufacturingLocation = "ManufacturingLocation";
        public const string SecondaryManufacturingLocation = "SecondaryManufacturingLoc";
        public const string PackingLocation = "PackingLocation";
        public const string SecondaryPackingLocation = "SecondaryPackingLocation";
        public const string DistributionCenter = "DistributionCenter";
        public const string SecondaryDistributionCenter = "SecondaryDistributionCenter";
        public const string InternalTransferSemiNeeded = "InternalTransferSemiNeeded";
        public const string WorkCenterAdditionalInfo = "WorkCenterAdditionalInfo";
        public const string ItemConceptandFG = "ItemConceptandFG";


        public const string ManufacturingLocation_DisplayName = "Manufacturing Location";
        public const string SecondaryManufacturingLocation_DisplayName = "Secondary Manufacturing Location";
        public const string PackingLocation_DisplayName = "Packing Location";
        public const string SecondaryPackingLocation_DisplayName = "Secondary Packing Location";
        public const string DistributionCenter_DisplayName = "Distribution Center";
        public const string SecondaryDistributionCenter_DisplayName = "Secondary Distribution Center";
        public const string InternalTransferSemiNeeded_DisplayName = "Internal Transfer Semi Needed?";
        public const string WorkCenterAdditionalInfo_DisplayName = "Work Center Additional Info";
        public const string ItemConceptandFG_DisplayName = "Item Concept and FG";

        #endregion

        #region External Manufacturing Form - EXTMFG
        public const string ExternalMfgProjectLead = "ExternalMfgProjectLead";
        public const string CoManufacturingClassification = "CoManufacturingClassification";
        public const string DoesBulkSemiExistToBringInHouse = "DoesBulkSemiExistToBringInHouse";
        public const string ExistingBulkSemiNumber = "ExistingBulkSemiNumber";
        public const string BulkSemiDescription = "BulkSemiDescription";
        public const string ExternalManufacturer = "ExternalManufacturer";
        public const string DoesSupplierHaveMakeCapacity = "DoesSupplierHaveMakeCapacity";
        public const string ManufacturerCountryOfOrigin = "ManufacturerCountryOfOrigin";
        public const string ExternalPacker = "ExternalPacker";
        public const string DoesSupplierHavePackCapacity = "DoesSupplierHavePackCapacity";
        public const string PurchasedIntoLocation = "PurchasedIntoLocation";
        public const string CurrentTimelineAcceptable = "CurrentTimelineAcceptable";
        public const string LeadTimeFromSupplier = "LeadTimeFromSupplier";
        public const string FinalArtworkDueToSupplier = "FinalArtworkDueToSupplier";
        public const string MakePackTransferLocation = "MakePackTransferLocation";

        public const string ExternalMfgProjectLead_DisplayName = "External Mfg Project Lead";
        public const string CoManufacturingClassification_DisplayName = "Co-Manufacturing Classification";
        public const string DoesBulkSemiExistToBringInHouse_DisplayName = "Bulk Semi Exist For In House?";
        public const string ExistingBulkSemiNumber_DisplayName = "Existing Bulk Semi#";
        public const string BulkSemiDescription_DisplayName = "Bulk Semi Description";
        public const string ExternalManufacturer_DisplayName = "External Manufacturer";
        public const string DoesSupplierHaveMakeCapacity_DisplayName = "Supplier Have Make Capacity?";
        public const string ManufacturerCountryOfOrigin_DisplayName = "Manufacturer Country Of Origin";
        public const string ExternalPacker_DisplayName = "External Packer";
        public const string DoesSupplierHavePackCapacity_DisplayName = "Supplier HavePackCapacity";
        public const string PurchasedIntoLocation_DisplayName = "PurchasedIntoLocation";
        public const string CurrentTimelineAcceptable_DisplayName = "CurrentTimelineAcceptable";
        public const string LeadTimeFromSupplier_DisplayName = "LeadTimeFromSupplier";
        public const string FinalArtworkDueToSupplier_DisplayName = "FinalArtworkDueToSupplier";
        public const string MakePackTransferLocation_DisplayName = "Make Pack Transfer Location";
        #endregion

        #region QA Fields - QA

        public const string ShelfLife = "ShelfLife";
        public const string Kosher = "Kosher";
        public const string Allergen = "Allergen";
        public const string CandySemiDescription = "CandySemiDescription";
        public const string TrialsCompleted = "TrialsCompleted";

        public const string ShelfLife_DisplayName = "Shelf Life";
        public const string Kosher_DisplayName = "Kosher";
        public const string Allergen_DisplayName = "Allergen";
        public const string CandySemiDescription_DisplayName = "Candy Semi Description";
        public const string NewSemiRequired_DisplayName = "New Semi Required";
        public const string NewTransferSemiRequired_DisplayName = "New Transfer Semi Required";
        public const string NewCandySemiRequired_DisplayName = "New Candy Semi Required";
        public const string TrialsCompleted_DisplayName = "Trials Completed";

        #endregion

        #region Bill of Materials Fields 
        public const string PackagingEngineerLead = "PackagingEngineerLead";
        public const string PackagingNumbers = "PackagingNumbers";

        public const string PackagingEngineerLead_DisplayName = "Packaging Engineer Lead";
        public const string PackagingNumbers_DisplayName = "Packaging #s";
        public const string FGLikeItem = "FG Like Item";
        public const string FGLikeItemDescription = "FGLikeItemDescription";
        public const string FGLikeItem_DisplayName = "FG Like Item:";

        #endregion

        #region OBM First Review
        public const string ProjectStatus = "ProjectStatus";
        public const string OBMFirstReviewCheck = "OBMFirstReviewCheck";
        public const string SectionsOfConcern = "SectionsOfConcern";
        public const string OBMFirstReviewComments = "OBMFirstReviewComments";
        public const string DoesFirstShipNeedRevision = "DoesFirstShipNeedRevision";
        public const string RevisedFirstShipDateComments = "RevisedFirstShipDateComments";
        public const string FirstProductionDate = "FirstProductionDate";
        public const string OBMSecondReviewCheck = "OBMSecondReviewCheck";
        public const string NewSemiRequired = "NewSemiRequired";
        public const string NewTransferSemiRequired = "NewTransferSemiRequired";
        public const string NewCandySemiRequired = "NewCandySemiRequired";

        public const string ProjectStatus_DisplayName = "Project Status";
        public const string OBMFirstReviewCheck_DisplayName = "OBM 1st Review Check";
        public const string SectionsOfConcern_DisplayName = "OBM 1st Rev Sections of Concern";
        public const string OBMFirstReviewComments_DisplayName = "OBM 1st Review Comments";
        public const string DoesFirstShipNeedRevision_DisplayName = "Does 1st Ship Need Revision?";
        public const string RevisedFirstShipDateComments_DisplayName = "Revised 1st Ship Date Comments";
        public const string FirstProductionDate_DisplayName = "1st Production Date";
        public const string OBMSecondReviewCheck_DisplayName = "OBM 2nd Review Check";
        #endregion

        #region Document Library Fields

        public const string DOCLIBRARY_PackagingComponentItemId = "PackagingComponentItemId";
        public const string DOCLIBRARY_PackagingComponentItemId_DisplayName = "Packaging Component ItemId";
        public const string DOCLIBRARY_CompassDocType = "CompassDocType";
        public const string DOCLIBRARY_CompassDocType_DisplayName = "Compass Document Type";

        public const string DOCLIBRARY_DisplayFileName = "DisplayFileName";
        public const string DOCLIBRARY_DisplayFileName_DisplayName = "Display File Name";
        #endregion

    }
}
