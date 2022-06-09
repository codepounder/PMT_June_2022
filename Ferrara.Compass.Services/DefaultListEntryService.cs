using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Helper;

namespace Ferrara.Compass.Services
{
    public class DefaultListEntryService : IDefaultListEntryService
    {
        public IEnumerable<string> GetDefaultDataFor(XmlFileName fileName)
        {
            var xmlFileData = GetXmlFileData(fileName);
            return XmlSerializationHelper.DeserializeFromXml<List<string>>(xmlFileData);
        }

        public string GetXML(XmlFileName fileName)
        {
            return GetXmlFileData(fileName);
        }

        public IEnumerable<GlobalLookupField> GetGlobalLookupFieldsData(XmlFileName fileName)
        {
            var xmlFileData = GetXmlFileData(fileName);
            return XmlSerializationHelper.DeserializeFromXml<List<GlobalLookupField>>(xmlFileData);
        }

        private static string GetXmlFileData(XmlFileName fileName)
        {
            string xmlFileData = string.Empty;

            switch (fileName)
            {
                case XmlFileName.AllergenLookupData:
                    xmlFileData = Properties.Resources.AllergensLookup;
                    break;
                case XmlFileName.CaseTypesLookupData:
                    xmlFileData = Properties.Resources.CasesTypes;
                    break;
                case XmlFileName.CountryOfOriginLookupData:
                    xmlFileData = Properties.Resources.CountryOfOrigin;
                    break;
                case XmlFileName.CustomersLookupData:
                    xmlFileData = Properties.Resources.Customers;
                    break;
                case XmlFileName.CompPurchasedIntoLocationsLookup:
                    xmlFileData = Properties.Resources.ComponentPurchasedIntoLocations;
                    break;
                case XmlFileName.DistributionCentersLookupData:
                    xmlFileData = Properties.Resources.DistributionCenters;
                    break;
                case XmlFileName.DistributionDeploymentModesLookupData:
                    xmlFileData = Properties.Resources.DistributionDeploymentModes;
                    break;
                case XmlFileName.KosherTypesLookupData:
                    xmlFileData = Properties.Resources.KosherTypes;
                    break;
                case XmlFileName.ManufacturingLocationsLookupData:
                    xmlFileData = Properties.Resources.ManufacturingLocations;
                    break;
                case XmlFileName.TaskCheckboxesLookup:
                    xmlFileData = Properties.Resources.TaskCheckboxesLookup;
                    break;
                case XmlFileName.MaterialGroup1LookupData:
                    xmlFileData = Properties.Resources.MaterialGroup1Lookup;
                    break;
                case XmlFileName.MaterialGroup2LookupData:
                    xmlFileData = Properties.Resources.MaterialGroup2Lookup;
                    break;
                case XmlFileName.MaterialGroup4LookupData:
                    xmlFileData = Properties.Resources.MaterialGroup4Lookup;
                    break;
                case XmlFileName.MaterialGroup5LookupData:
                    xmlFileData = Properties.Resources.MaterialGroup5Lookup;
                    break;
                case XmlFileName.PackagingComponentTypesLookupData:
                    xmlFileData = Properties.Resources.PackagingComponentTypes;
                    break;
                case XmlFileName.PackingLocationsLookupData:
                    xmlFileData = Properties.Resources.PackingLocations;
                    break;
                case XmlFileName.PlantLinesLookupData:
                    xmlFileData = Properties.Resources.PlantLines;
                    break;
                case XmlFileName.ProcurementTypesLookupData:
                    xmlFileData = Properties.Resources.ProcurementTypes;
                    break;
                case XmlFileName.ProductHierarchyLevel1LookupData:
                    xmlFileData = Properties.Resources.ProductHierarchyLevel1;
                    break;
                case XmlFileName.ProductHierarchyLevel2LookupData:
                    xmlFileData = Properties.Resources.ProductHierarchyLevel2;
                    break;
                case XmlFileName.ProjectTypesLookupData:
                    xmlFileData = Properties.Resources.ProjectTypes;
                    break;
                case XmlFileName.PMTProjectTypesLookupData:
                    xmlFileData = Properties.Resources.PMTProjectTypes;
                    break;
                case XmlFileName.RetailPackTypesLookupData:
                    xmlFileData = Properties.Resources.RetailPackTypes;
                    break;
                case XmlFileName.PrintStyleLookupData:
                    xmlFileData = Properties.Resources.PrintStyleTypes;
                    break;
                case XmlFileName.FilmPrintStyleLookupData:
                    xmlFileData = Properties.Resources.FilmPrintStyleTypes;
                    break;
                case XmlFileName.FilmStyleLookupData:
                    xmlFileData = Properties.Resources.FilmStyleTypes;
                    break;
                case XmlFileName.CorrugatedPrintStyleLookupData:
                    xmlFileData = Properties.Resources.CorrugatedPrintStyleTypes;
                    break;
                case XmlFileName.ExternalGraphicsVendorsData:
                    xmlFileData = Properties.Resources.ExternalGraphicsVendorTypes;
                    break;
                case XmlFileName.CriticalInitiativesData:
                    xmlFileData = Properties.Resources.CriticalInitiativesLookup;
                    break;
                case XmlFileName.PrinterSupplierData:
                    xmlFileData = Properties.Resources.PrinterSupplierLookup;
                    break;
                case XmlFileName.ProjectCategoryData:
                    xmlFileData = Properties.Resources.ProjectCategoryLookup;
                    break;
                case XmlFileName.TargetFCCMarginData:
                    xmlFileData = Properties.Resources.TargetFCCMarginLookup;
                    break;
                case XmlFileName.SAPBaseUOMData:
                    xmlFileData = Properties.Resources.SAPBaseUOMLookup;
                    break;
                case XmlFileName.PackUnitData:
                    xmlFileData = Properties.Resources.PackUnitLookup;
                    break;
                case XmlFileName.FlowThroughTypeData:
                    xmlFileData = Properties.Resources.FlowThroughTypesLookup;
                    break;
                case XmlFileName.MRPCData:
                    xmlFileData = Properties.Resources.MRPCLookup;
                    break;
                case XmlFileName.ChannelData:
                    xmlFileData = Properties.Resources.ChannelLookup;
                    break;
                case XmlFileName.SubstrateData:
                    xmlFileData = Properties.Resources.SubstrateLookup;
                    break;
                case XmlFileName.StageLookupData:
                    xmlFileData = Properties.Resources.Stages;
                    break;
                case XmlFileName.MakePackTransfersData:
                    xmlFileData = Properties.Resources.MakePackTransfersLookup;
                    break;
                case XmlFileName.SubstrateColorData:
                    xmlFileData = Properties.Resources.SubstrateColorLookup;
                    break;
                case XmlFileName.PurchasedIntoCenterData:
                    xmlFileData = Properties.Resources.PurchasedIntoCenterLookup;
                    break;
                case XmlFileName.FilmSubstrateData:
                    xmlFileData = Properties.Resources.FilmSubstrateLookup;
                    break;
                case XmlFileName.CoManufacturersData:
                    xmlFileData = Properties.Resources.CoManufacturers;
                    break;
                case XmlFileName.CoManufacturingClassificationsData:
                    xmlFileData = Properties.Resources.CoManufacturingClassificationTypes;
                    break;
                case XmlFileName.CoPackersData:
                    xmlFileData = Properties.Resources.CoPackers;
                    break;
                case XmlFileName.ManufacturerCountryOfOriginData:
                    xmlFileData = Properties.Resources.ManufacturerCountryOfOriginLookup;
                    break;
                case XmlFileName.SupplierLeadTimeData:
                    xmlFileData = Properties.Resources.SupplierLeadTimeTypes;
                    break;
                case XmlFileName.PackTrialResultData:
                    xmlFileData = Properties.Resources.PackTrialResult;
                    break;
                case XmlFileName.SecondaryInitialReviewDecisions:
                    xmlFileData = Properties.Resources.SecondaryInitialReviewDecisions;
                    break;
                case XmlFileName.OrderUnitofMeasure:
                    xmlFileData = Properties.Resources.OrderUnitofMeasureLookup;
                    break;
                case XmlFileName.Incoterms:
                    xmlFileData = Properties.Resources.IncotermsLookup;
                    break;
                case XmlFileName.PRDateCategory:
                    xmlFileData = Properties.Resources.PRDateCategoryLookup;
                    break;
                case XmlFileName.TimelineTypesLookupData:
                    xmlFileData = Properties.Resources.TimelineTypesLookup;
                    break;
                case XmlFileName.BackSeamsData:
                    xmlFileData = Properties.Resources.BackSeamTypes;
                    break;
                case XmlFileName.CostingUnitData:
                    xmlFileData = Properties.Resources.CostingUnitTypes;
                    break;
                case XmlFileName.NutrientTypeData:
                    xmlFileData = Properties.Resources.NutrientTypes;
                    break;
                case XmlFileName.NutrientQuantityContainedTypeData:
                    xmlFileData = Properties.Resources.NutrientQuantityContainedTypes;
                    break;
                case XmlFileName.DailyValueIntakePctMeasPrecCode:
                    xmlFileData = Properties.Resources.DailyValueIntakePctMeasPrecCode;
                    break;
                case XmlFileName.NutrientQuantityUOMdata:
                    xmlFileData = Properties.Resources.NutrientQuantityUOMs;
                    break;
                case XmlFileName.PreparationStateData:
                    xmlFileData = Properties.Resources.PreparationStates;
                    break;
                case XmlFileName.ServingSizeData:
                    xmlFileData = Properties.Resources.ServingSizes;
                    break;
                case XmlFileName.ProductTypeData:
                    xmlFileData = Properties.Resources.ProductTypes;
                    break;
                case XmlFileName.AlternateClassificationSchemeData:
                    xmlFileData = Properties.Resources.AlternateClassificationSchemes;
                    break;
                case XmlFileName.GS1tradeKeyCodeData:
                    xmlFileData = Properties.Resources.GS1tradeKeyCodes;
                    break;
                case XmlFileName.DataCarrierTypeCodeData:
                    xmlFileData = Properties.Resources.DataCarrierTypeCodes;
                    break;
                case XmlFileName.TradeChannelData:
                    xmlFileData = Properties.Resources.TradeChannels;
                    break;
                case XmlFileName.BrandOwnerGLNdata:
                    xmlFileData = Properties.Resources.BrandOwnerGLNs;
                    break;
                case XmlFileName.ProjectTypesSubCategoryLookup:
                    xmlFileData = Properties.Resources.ProjectTypesSubCategoryLookup;
                    break;
                case XmlFileName.PercentagesLookup:
                    xmlFileData = Properties.Resources.PercentagesLookup;
                    break;
                case XmlFileName.GoodSourceLookup:
                    xmlFileData = Properties.Resources.GoodSourceLookup;
                    break;
                case XmlFileName.NaturalColorsLookup:
                    xmlFileData = Properties.Resources.NaturalColorsLookup;
                    break;
                case XmlFileName.NaturalFlavorsLookup:
                    xmlFileData = Properties.Resources.NaturalFlavorsLookup;
                    break;
                case XmlFileName.GlutenFreeLookup:
                    xmlFileData = Properties.Resources.GlutenFreeLookup;
                    break;
                case XmlFileName.GMOClaimLookup:
                    xmlFileData = Properties.Resources.GMOClaimLookup;
                    break;
                case XmlFileName.MadeInUSAClaimLookup:
                    xmlFileData = Properties.Resources.MadeInUSAClaimLookup;
                    break;
                case XmlFileName.FilmUnWindLookup:
                    xmlFileData = Properties.Resources.FilmUnWindLookup;
                    break;
                case XmlFileName.StageGateDesignDelivLookup:
                    xmlFileData = Properties.Resources.StageGateDesignDelivLookup;
                    break;
                case XmlFileName.StageGateDevelopDelivLookup:
                    xmlFileData = Properties.Resources.StageGateDevelopDelivLookup;
                    break;
                case XmlFileName.StageGateIndustrializDelivLookup:
                    xmlFileData = Properties.Resources.StageGateIndustrializDelivLookup;
                    break;
                case XmlFileName.StageGateLaunchDelivLookup:
                    xmlFileData = Properties.Resources.StageGateLaunchDelivLookup;
                    break;
                case XmlFileName.StageGatePostLaunchDelivLookup:
                    xmlFileData = Properties.Resources.StageGatePostLaunchDelivLookup;
                    break;
                case XmlFileName.StageGateValidateDelivLookup:
                    xmlFileData = Properties.Resources.StageGateValidateDelivLookup;
                    break;
                case XmlFileName.StageGateStageStatusLookup:
                    xmlFileData = Properties.Resources.StageGateStageStatusLookup;
                    break;
                case XmlFileName.OverallRiskLookup:
                    xmlFileData = Properties.Resources.OverallRiskLookup;
                    break;
                case XmlFileName.OverallStatusLookup:
                    xmlFileData = Properties.Resources.OverallStatusLookup;
                    break;
                case XmlFileName.BusinessFunctionsLookup:
                    xmlFileData = Properties.Resources.BusinessFunctions;
                    break;
                case XmlFileName.BioEngineeringLabelingcceptableLookup:
                    xmlFileData = Properties.Resources.BioEngineeringLabelingcceptableLookup;
                    break;
                case XmlFileName.BioEngineeringLabelingRequiredLookup:
                    xmlFileData = Properties.Resources.BioEngineeringLabelingRequiredLookup;
                    break;
            }
            return xmlFileData;
        }
    }
}
