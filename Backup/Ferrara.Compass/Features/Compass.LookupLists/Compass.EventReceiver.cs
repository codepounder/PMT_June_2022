using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.DependencyResolution;
using System.Collections.Generic;
using System.Linq;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Classes;
using Microsoft.Practices.Unity;
using System.Xml;
using System.Data;
using System.Xml.Linq;


namespace Ferrara.Compass.Features.Compass.LookupLists
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("4f17c0a6-1e62-41ce-b6db-1019e3d5660d")]
    public class CompassEventReceiver : SPFeatureReceiver
    {
        SPWeb web;
        private IDefaultListEntryService defaultListEntryService;
        List<string> yesNoStates = new List<string>{
                    "Y",
                    "N"
                };

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            web = properties.Feature.Parent as SPWeb;
            defaultListEntryService = DependencyMapper.Container.Resolve<IDefaultListEntryService>();

            web.AllowUnsafeUpdates = true;

            CreateSiteColumns();
            CreateSiteContentType();
            AddSitecolumnsToCtype();

            //DeleteAllLists();

            CreateListAndLoadXmlData(GlobalConstants.LIST_TimelineTypesLookup, XmlFileName.TimelineTypesLookupData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_AllergensLookup, XmlFileName.AllergenLookupData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_CaseTypesLookup, XmlFileName.CaseTypesLookupData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_CountryOfOriginLookup, XmlFileName.CountryOfOriginLookupData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_CustomersLookup, XmlFileName.CustomersLookupData);
            CreateListAndLoadXmlDataGeneric(GlobalConstants.LIST_DistributionLookup, XmlFileName.DistributionCentersLookupData);
            CreateListAndLoadXmlDataGeneric(GlobalConstants.LIST_DistributionDeploymentModesLookup, XmlFileName.DistributionDeploymentModesLookupData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_KosherTypesLookup, XmlFileName.KosherTypesLookupData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_ManufacturingLocationsLookup, XmlFileName.ManufacturingLocationsLookupData);
            CreateListAndLoadXmlDataGeneric(GlobalConstants.LIST_MaterialGroup1Lookup, XmlFileName.MaterialGroup1LookupData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_MaterialGroup2Lookup, XmlFileName.MaterialGroup2LookupData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_MaterialGroup4Lookup, XmlFileName.MaterialGroup4LookupData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_MaterialGroup5Lookup, XmlFileName.MaterialGroup5LookupData);
            CreateListAndLoadXmlDataGeneric(GlobalConstants.LIST_PackagingComponentTypesLookup, XmlFileName.PackagingComponentTypesLookupData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_PackingLocationsLookup, XmlFileName.PackingLocationsLookupData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_PlantLinesLookup, XmlFileName.PlantLinesLookupData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_ProcurementTypesLookup, XmlFileName.ProcurementTypesLookupData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_ProductHierarchyLevel1Lookup, XmlFileName.ProductHierarchyLevel1LookupData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_ProductHierarchyLevel2Lookup, XmlFileName.ProductHierarchyLevel2LookupData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_ProjectTypesLookup, XmlFileName.ProjectTypesLookupData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_StageGateProjectTypesLookup, XmlFileName.PMTProjectTypesLookupData);
            CreateListAndLoadXmlDataGeneric(GlobalConstants.LIST_StageLookup, XmlFileName.StageLookupData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_RetailPackTypesLookup, XmlFileName.RetailPackTypesLookupData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_FilmPrintStyleLookup, XmlFileName.FilmPrintStyleLookupData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_FilmStyleLookup, XmlFileName.FilmStyleLookupData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_CorrugatedPrintStyleLookup, XmlFileName.CorrugatedPrintStyleLookupData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_ExternalGraphicsVendorLookup, XmlFileName.ExternalGraphicsVendorsData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_CriticalInitiativesLookup, XmlFileName.CriticalInitiativesData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_PrinterSupplierLookup, XmlFileName.PrinterSupplierData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_ProjectCategoryLookup, XmlFileName.ProjectCategoryData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_TargetFCCMarginLookup, XmlFileName.TargetFCCMarginData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_SAPBaseUOMLookup, XmlFileName.SAPBaseUOMData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_PackUnitLookup, XmlFileName.PackUnitData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_FlowThroughTypeLookup, XmlFileName.FlowThroughTypeData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_MRPCLookup, XmlFileName.MRPCData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_ChannelLookup, XmlFileName.ChannelData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_SubstrateLookup, XmlFileName.SubstrateData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_FilmSubstrate, XmlFileName.FilmSubstrateData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_CoManufacturers, XmlFileName.CoManufacturersData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_CoManufacturingClassifications, XmlFileName.CoManufacturingClassificationsData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_CoPackers, XmlFileName.CoPackersData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_ManufacturerCountryOfOrigin, XmlFileName.ManufacturerCountryOfOriginData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_SupplierLeadTime, XmlFileName.SupplierLeadTimeData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_PurchasedIntoCenterLookup, XmlFileName.PurchasedIntoCenterData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_MakePackTransfersLookup, XmlFileName.MakePackTransfersData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_SubstrateColorLookup, XmlFileName.SubstrateColorData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_PackTrialResultLookUp, XmlFileName.PackTrialResultData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_SecondaryInitialReviewDecisionsLookup, XmlFileName.SecondaryInitialReviewDecisions);
            CreateListAndLoadXmlData(GlobalConstants.LIST_BackSeamsLookup, XmlFileName.BackSeamsData);

            CreateListAndLoadXmlData(GlobalConstants.LIST_OrderUnitofMeasureLookup, XmlFileName.OrderUnitofMeasure);
            CreateListAndLoadXmlData(GlobalConstants.LIST_IncotermsLookup, XmlFileName.Incoterms);
            CreateListAndLoadXmlData(GlobalConstants.LIST_PRDateCategoryLookup, XmlFileName.PRDateCategory);
            CreateListAndLoadXmlData(GlobalConstants.LIST_CostingUnitLookup, XmlFileName.CostingUnitData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_NutrientTypeLookup, XmlFileName.NutrientTypeData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_NutrientQuantityContainedTypeLookup, XmlFileName.NutrientQuantityContainedTypeData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_NutrientQuantityUOMlookup, XmlFileName.NutrientQuantityUOMdata);
            CreateListAndLoadXmlData(GlobalConstants.LIST_DailyValueIntakePctMeasPrecCodeLookup, XmlFileName.DailyValueIntakePctMeasPrecCode);

            CreateListAndLoadXmlData(GlobalConstants.LIST_PreparationStateLookup, XmlFileName.PreparationStateData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_ServingSizeLookup, XmlFileName.ServingSizeData);

            CreateListAndLoadXmlData(GlobalConstants.LIST_ProductTypeLookup, XmlFileName.ProductTypeData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_AlternateClassificationSchemeLookup, XmlFileName.AlternateClassificationSchemeData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_GS1tradeKeyCodeLookup, XmlFileName.GS1tradeKeyCodeData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_DataCarrierTypeCodeLookup, XmlFileName.DataCarrierTypeCodeData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_TradeChannelLookup, XmlFileName.TradeChannelData);
            CreateListAndLoadXmlData(GlobalConstants.LIST_BrandOwnerGLNlookup, XmlFileName.BrandOwnerGLNdata);
            CreateListAndLoadXmlData(GlobalConstants.LIST_ProjectTypesSubCategoryLookup, XmlFileName.ProjectTypesSubCategoryLookup);
            CreateListAndLoadXmlData(GlobalConstants.LIST_PercentagesLookup, XmlFileName.PercentagesLookup);
            CreateListAndLoadXmlData(GlobalConstants.LIST_GoodSourceLookup, XmlFileName.GoodSourceLookup);
            CreateListAndLoadXmlData(GlobalConstants.LIST_NaturalColorsLookup, XmlFileName.NaturalColorsLookup);
            CreateListAndLoadXmlData(GlobalConstants.LIST_NaturalFlavorsLookup, XmlFileName.NaturalFlavorsLookup);
            CreateListAndLoadXmlData(GlobalConstants.LIST_GlutenFreeLookup, XmlFileName.GlutenFreeLookup);
            CreateListAndLoadXmlData(GlobalConstants.LIST_GMOClaimLookup, XmlFileName.GMOClaimLookup);
            CreateListAndLoadXmlData(GlobalConstants.LIST_MadeInUSAClaimLookup, XmlFileName.MadeInUSAClaimLookup);
            CreateListAndLoadXmlData(GlobalConstants.LIST_FilmUnWindLookup, XmlFileName.FilmUnWindLookup);
            CreateListAndLoadXmlData(GlobalConstants.LIST_StageGateStageStatus, XmlFileName.StageGateStageStatusLookup);
            CreateListAndLoadXmlData(GlobalConstants.LIST_GateDetailColorsLookup, XmlFileName.GateDetailColorsLookup);

            CreateListAndLoadXmlData(GlobalConstants.LIST_StageGateDesignDeliverables, XmlFileName.StageGateDesignDelivLookup);
            CreateListAndLoadXmlData(GlobalConstants.LIST_StageGateDevelopDeliverables, XmlFileName.StageGateDevelopDelivLookup);
            CreateListAndLoadXmlData(GlobalConstants.LIST_StageGateIndustrializDeliverables, XmlFileName.StageGateIndustrializDelivLookup);
            CreateListAndLoadXmlData(GlobalConstants.LIST_StageGateLaunchDeliverables, XmlFileName.StageGateLaunchDelivLookup);
            CreateListAndLoadXmlData(GlobalConstants.LIST_StageGatePostLaunchDeliverables, XmlFileName.StageGatePostLaunchDelivLookup);
            CreateListAndLoadXmlData(GlobalConstants.LIST_StageGateValidateDeliverables, XmlFileName.StageGateValidateDelivLookup);
            CreateListAndLoadXmlData(GlobalConstants.LIST_StageGateSGMeetingLookup, XmlFileName.StageGateSGMeetingLookup);
            CreateListAndLoadXmlData(GlobalConstants.LIST_OverallRiskLookup, XmlFileName.OverallRiskLookup);
            CreateListAndLoadXmlData(GlobalConstants.LIST_OverallStatusLookup, XmlFileName.OverallStatusLookup);
            CreateListAndLoadXmlData(GlobalConstants.LIST_CompPurchasedIntoLocationsLookup, XmlFileName.CompPurchasedIntoLocationsLookup);
            CreateListAndLoadXmlDataGeneric(GlobalConstants.LIST_BusinessFunctionsLookup, XmlFileName.BusinessFunctionsLookup);
            CreateListAndLoadXmlDataGeneric(GlobalConstants.LIST_TaskCheckboxesLookup, XmlFileName.TaskCheckboxesLookup);
            CreateListAndLoadXmlData(GlobalConstants.LIST_BioEngineeringLabelingcceptableLookup, XmlFileName.BioEngineeringLabelingcceptableLookup);
            CreateListAndLoadXmlData(GlobalConstants.LIST_BioEngineeringLabelingRequiredLookup, XmlFileName.BioEngineeringLabelingRequiredLookup);
            CreateListAndLoadXmlDataGeneric(GlobalConstants.LIST_PrintStyleLookup, XmlFileName.PrintStyleLookupData);

            CreateSAPMaterialMasterList();
            CreateSAPBOMList();
            CreatePLMSpecificationsList();
            CreateSAPHanaStatusList();
            CreateDragonflyStatusList();

            web.AllowUnsafeUpdates = false;
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            web = properties.Feature.Parent as SPWeb;
            //DeleteAllLists();
        }

        private void CreateListAndLoadXmlData(string listName, XmlFileName fileName)
        {
            if (CreateLookupList(listName))
            {
                var list = web.Lists.TryGetList(listName);
                if (list != null)
                {
                    var globalLookupFields = defaultListEntryService.GetGlobalLookupFieldsData(fileName);

                    foreach (var lookupField in globalLookupFields)
                    {
                        AddDefaultItems(list, lookupField);
                    }
                }
            }
        }
        #region CreateListAndLoadXmlDataDets
        private void CreateListAndLoadXmlDataDets(string listName, XmlFileName fileName)
        {
            SPList splist = web.Lists.TryGetList(listName);
            var needsListUpdate = false;
            //Creating the List if List does not exist 
            if (splist == null)
            {
                splist = SetupUtilities.CreateList(web, listName, "This is the lookup list for the Compass " + listName.Substring(0, listName.IndexOf(" Lookup")));

                XmlDocument doc = new XmlDocument();
                var xmlFileData = defaultListEntryService.GetDefaultDataFor(fileName);

                doc.Load(xmlFileData.ToString());

                foreach (XmlNode xn in doc.ChildNodes[0])
                {
                    string tagName = xn.Name;
                    if (SetupUtilities.CreateField(splist, tagName, tagName, SPFieldType.Text, false))
                    {
                        needsListUpdate = true;
                    }
                }
                if (needsListUpdate)
                    splist.Update();
            }
        }
        #endregion

        private void CreateListAndLoadXmlDataGeneric(string listName, XmlFileName fileName)
        {
            var needsListUpdate = false;

            string description = "This is the lookup list for the Compass " + listName.Substring(0, listName.IndexOf(" Lookup"));

            SPList splist = web.Lists.TryGetList(listName);

            //Creating the List if List does not exist 
            if (splist == null)
            {
                splist = SetupUtilities.CreateList(web, listName, description);

                splist.OnQuickLaunch = false;
                splist.EnableVersioning = false;
                splist.EnableMinorVersions = false;
                splist.ContentTypesEnabled = true;
                splist.Update();
                LinkContentTypeToList(splist);
                SetDefaultContentType(splist);

                web.Update();

                XmlDocument doc = new XmlDocument();
                XDocument xdoc = new XDocument();

                var xmlFileData1 = defaultListEntryService.GetXML(fileName);
                xdoc = XDocument.Parse(xmlFileData1);
                var elements = xdoc.Element("ArrayOfGlobalLookupField").Elements("GlobalLookupField");

                var firstElement = elements.First();

                foreach (var attribute in firstElement.Attributes())
                {
                    if (attribute.Name.ToString() != "Id" &&
                        attribute.Name.ToString() != "Title" &&
                        attribute.Name.ToString() != "Active" &&
                        attribute.Name.ToString() != "LookupFieldValue")
                    {
                        if (SetupUtilities.CreateField(splist, attribute.Name.ToString(), attribute.Name.ToString(), SPFieldType.Text, false))
                        {
                            needsListUpdate = true;
                        }
                    }
                }

                if (needsListUpdate)
                    splist.Update();

                foreach (var element in elements)
                {
                    SPListItem item = splist.Items.Add();
                    foreach (var attribute in element.Attributes())
                    {
                        if (attribute.Name.ToString() != "Id")
                        {
                            if (attribute.Name.ToString() == "LookupFieldValue")
                            {
                                item[GlobalLookupFieldConstants.LookupValue] = attribute.Value.ToString();
                            }
                            else if (attribute.Name.ToString() == "Active")
                            {
                                item[GlobalLookupFieldConstants.Active] = attribute.Value.ToString();
                            }
                            else
                            {
                                item[attribute.Name.ToString()] = attribute.Value.ToString();
                            }
                        }
                    }
                    item.Update();
                }
            }
        }
        private void CreateSiteColumns()
        {
            if (!web.Fields.ContainsField(GlobalLookupFieldConstants.LookupValue))
            {
                SetupUtilities.CreateFieldInWeb(web, GlobalLookupFieldConstants.LookupValue, GlobalLookupFieldConstants.LookupValue_DisplayName, SPFieldType.Text.ToString(), false);
            }

            if (!web.Fields.ContainsField(GlobalLookupFieldConstants.Active))
            {
                SetupUtilities.CreateFieldInWeb(web, GlobalLookupFieldConstants.Active, GlobalLookupFieldConstants.Active_DisplayName, SPFieldType.Boolean.ToString(), false);
            }
        }

        private void CreateSiteContentType()
        {
            SPContentTypeId itemContentTypeId = new SPContentTypeId("0x01");

            if (web.AvailableContentTypes[GlobalConstants.CONTENTTYPE_CompassLookup] == null)
            {
                SPContentType itemCType = web.AvailableContentTypes[itemContentTypeId];
                SPContentType contentType =
                    new SPContentType(itemCType, web.ContentTypes, GlobalConstants.CONTENTTYPE_CompassLookup) { Group = "Compass" };
                web.ContentTypes.Add(contentType);
                contentType.Update();

                web.Update();
            }
        }

        private void SetDefaultContentType(SPList list)
        {
            IList<SPContentType> cTypes = new List<SPContentType>();
            SPFolder root = list.RootFolder;
            cTypes = root.ContentTypeOrder;
            SPContentType cType = cTypes.SingleOrDefault(hd => hd.Name == GlobalConstants.CONTENTTYPE_CompassLookup);
            int j = cTypes.IndexOf(cType);
            cTypes.RemoveAt(j);
            cTypes.Insert(0, cType);
            root.UniqueContentTypeOrder = cTypes;
            root.Update();
        }

        private void AddSitecolumnsToCtype()
        {
            if (web.AvailableContentTypes[GlobalConstants.CONTENTTYPE_CompassLookup] != null)
            {
                SPContentType contentType = web.ContentTypes[GlobalConstants.CONTENTTYPE_CompassLookup];

                if (!contentType.Fields.ContainsField(GlobalLookupFieldConstants.LookupValue))
                {
                    SPField fld = web.Fields.GetFieldByInternalName(GlobalLookupFieldConstants.LookupValue);
                    if (fld != null)
                    {
                        SPFieldLink fieldLink = new SPFieldLink(fld);
                        contentType.FieldLinks.Add(fieldLink);
                        contentType.Update();
                    }
                }

                if (!contentType.Fields.ContainsField(GlobalLookupFieldConstants.Active))
                {
                    SPField fld = web.Fields.GetFieldByInternalName(GlobalLookupFieldConstants.Active);
                    if (fld != null)
                    {
                        SPFieldLink fieldLink = new SPFieldLink(fld);
                        contentType.FieldLinks.Add(fieldLink);
                        contentType.Update();
                    }
                }
            }
        }

        private void DeleteAllLists()
        {

            DeleteList(GlobalConstants.LIST_TimelineTypesLookup);
            DeleteList(GlobalConstants.LIST_AllergensLookup);
            DeleteList(GlobalConstants.LIST_CaseTypesLookup);
            DeleteList(GlobalConstants.LIST_CountryOfOriginLookup);
            DeleteList(GlobalConstants.LIST_CustomersLookup);
            DeleteList(GlobalConstants.LIST_DistributionLookup);
            DeleteList(GlobalConstants.LIST_DistributionDeploymentModesLookup);
            DeleteList(GlobalConstants.LIST_KosherTypesLookup);
            DeleteList(GlobalConstants.LIST_ManufacturingLocationsLookup);
            DeleteList(GlobalConstants.LIST_MaterialGroup1Lookup);
            DeleteList(GlobalConstants.LIST_MaterialGroup2Lookup);
            DeleteList(GlobalConstants.LIST_MaterialGroup4Lookup);
            DeleteList(GlobalConstants.LIST_MaterialGroup5Lookup);
            DeleteList(GlobalConstants.LIST_PackagingComponentTypesLookup);
            DeleteList(GlobalConstants.LIST_PackingLocationsLookup);
            DeleteList(GlobalConstants.LIST_PlantLinesLookup);
            DeleteList(GlobalConstants.LIST_ProcurementTypesLookup);
            DeleteList(GlobalConstants.LIST_ProductHierarchyLevel1Lookup);
            DeleteList(GlobalConstants.LIST_ProductHierarchyLevel2Lookup);
            DeleteList(GlobalConstants.LIST_ProjectTypesLookup);
            DeleteList(GlobalConstants.LIST_StageGateProjectTypesLookup);
            DeleteList(GlobalConstants.LIST_StageLookup);
            DeleteList(GlobalConstants.LIST_RetailPackTypesLookup);
            DeleteList(GlobalConstants.LIST_FilmPrintStyleLookup);
            DeleteList(GlobalConstants.LIST_FilmStyleLookup);
            DeleteList(GlobalConstants.LIST_CorrugatedPrintStyleLookup);
            DeleteList(GlobalConstants.LIST_CriticalInitiativesLookup);
            DeleteList(GlobalConstants.LIST_PackTrialResultLookUp);
            DeleteList(GlobalConstants.LIST_SecondaryInitialReviewDecisionsLookup);
            DeleteList(GlobalConstants.LIST_OrderUnitofMeasureLookup);
            DeleteList(GlobalConstants.LIST_IncotermsLookup);
            DeleteList(GlobalConstants.LIST_PRDateCategoryLookup);
            DeleteList(GlobalConstants.LIST_StageGateDesignDeliverables);
            DeleteList(GlobalConstants.LIST_StageGateStageStatus);
        }

        private void DeleteList(string listName)
        {
            var list = web.Lists.TryGetList(listName);
            if (list != null)
            {
                web.Lists.Delete(list.ID);
            }
        }

        private static void AddDefaultItems(SPList list, GlobalLookupField globalLookupField)
        {
            if (list.Items.Cast<SPListItem>().Any(existingItem => globalLookupField.Title.ToUpperInvariant().Trim() == existingItem[GlobalLookupFieldConstants.Title].ToString().ToUpperInvariant().Trim() &&
                globalLookupField.LookupFieldValue.ToUpperInvariant().Trim() == existingItem[GlobalLookupFieldConstants.LookupValue].ToString().ToUpperInvariant().Trim()))
            {
                return;
            }

            SPListItem item = list.Items.Add();
            item[GlobalLookupFieldConstants.Title] = globalLookupField.Title;
            item[GlobalLookupFieldConstants.LookupValue] = globalLookupField.LookupFieldValue;
            item[GlobalLookupFieldConstants.Active] = globalLookupField.Active;
            item.Update();
        }

        private bool CreateLookupList(string list)
        {
            string description = "This is the lookup list for the Compass " + list.Substring(0, list.IndexOf(" Lookup"));

            SPList splist = web.Lists.TryGetList(list);

            //Creating the List if List does not exist 
            if (splist == null)
            {
                splist = SetupUtilities.CreateList(web, list, description);

                splist.OnQuickLaunch = false;
                splist.EnableVersioning = false;
                splist.EnableMinorVersions = false;
                splist.ContentTypesEnabled = true;
                splist.Update();
                LinkContentTypeToList(splist);
                SetDefaultContentType(splist);

                var view = splist.Views.Cast<SPView>().FirstOrDefault(w => w.Title.ToUpper().Equals(GlobalConstants.VIEW_AllItems.ToUpper()));
                if (view != null)
                {
                    view.ViewFields.DeleteAll();
                    view.ViewFields.Add(web.Fields[SPBuiltInFieldId.LinkTitle]);
                    view.ViewFields.Add(GlobalLookupFieldConstants.LookupValue_DisplayName);
                    view.ViewFields.Add(GlobalLookupFieldConstants.Active_DisplayName);
                    view.Update();
                    splist.Update();
                }
                web.Update();
                return true;
            }
            return false;
        }

        /// <summary>
        /// LinkContentTypeToList - this method will add the content type to the list
        /// </summary>
        /// <param name="web">spWeb for the site</param>
        /// <param name="contentTypeName">Name of the content type to add to the list</param>
        /// <param name="list">List to have the content type added to</param>
        private void LinkContentTypeToList(SPList list)
        {
            try
            {
                // Check to ensure the content type exists
                if (web.AvailableContentTypes[GlobalConstants.CONTENTTYPE_CompassLookup] != null)
                {
                    SPContentType contentType = web.ContentTypes[GlobalConstants.CONTENTTYPE_CompassLookup];
                    list.ContentTypes.Add(contentType);
                    list.Update();
                }
            }
            catch (Exception ex)
            {

            }
        }

        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}

        #region Other Lists
        private void CreateSAPMaterialMasterList()
        {
            try
            {
                SPList splist = web.Lists.TryGetList(GlobalConstants.LIST_SAPMaterialMasterListName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, GlobalConstants.LIST_SAPMaterialMasterListName, GlobalConstants.LIST_SAPMaterialMasterListName);
                }

                splist.EnableVersioning = false;
                splist.EnableMinorVersions = false;

                //Add Fields
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.HanaKey, SAPMaterialMasterListFields.HanaKey_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.SAPItemNumber, SAPMaterialMasterListFields.SAPItemNumber_DisplayName, SPFieldType.Integer, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.SAPDescription, SAPMaterialMasterListFields.SAPDescription_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.CaseType, SAPMaterialMasterListFields.CaseType_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.CandySemiNumber, SAPMaterialMasterListFields.CandySemiNumber_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.TruckLoadPricePerSellingUnit, SAPMaterialMasterListFields.TruckLoadPricePerSellingUnit_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateCurrencyField(splist, SAPMaterialMasterListFields.Last12MonthSales, SAPMaterialMasterListFields.Last12MonthSales_DisplayName, false, SPNumberFormatTypes.TwoDecimals))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.MaterialGroup1BrandCode, SAPMaterialMasterListFields.MaterialGroup1BrandCode_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.MaterialGroup1Brand, SAPMaterialMasterListFields.MaterialGroup1Brand_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.MaterialGroup4ProductFormCode, SAPMaterialMasterListFields.MaterialGroup4ProductFormCode_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.MaterialGroup4ProductForm, SAPMaterialMasterListFields.MaterialGroup4ProductForm_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.MaterialGroup5PackTypeCode, SAPMaterialMasterListFields.MaterialGroup5PackTypeCode_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.MaterialGroup5PackType, SAPMaterialMasterListFields.MaterialGroup5PackType_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.ProductHierarchyLevel1Code, SAPMaterialMasterListFields.ProductHierarchyLevel1Code_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.ProductHierarchyLevel1, SAPMaterialMasterListFields.ProductHierarchyLevel1_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.ProductHierarchyLevel2Code, SAPMaterialMasterListFields.ProductHierarchyLevel2Code_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.ProductHierarchyLevel2, SAPMaterialMasterListFields.ProductHierarchyLevel2_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.RetailSellingUnitsBaseUOM, SAPMaterialMasterListFields.RetailSellingUnitsBaseUOM_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.RetailUnitWieghtOz, SAPMaterialMasterListFields.RetailUnitWieghtOz_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.CaseUCC, SAPMaterialMasterListFields.CaseUCC_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.DisplayBoxUPC, SAPMaterialMasterListFields.DisplayBoxUPC_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.PalletUCC, SAPMaterialMasterListFields.PalletUCC_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.UnitUPC, SAPMaterialMasterListFields.UnitUPC_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.MaterialType, SAPMaterialMasterListFields.MaterialType_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.MaterialType2, SAPMaterialMasterListFields.MaterialType2_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.BaseQuantity, SAPMaterialMasterListFields.BaseQuantity_DisplayName, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.ListPriceB1PerUnit, SAPMaterialMasterListFields.ListPriceB1PerUnit_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPMaterialMasterListFields.ZestPricingPerUnit, SAPMaterialMasterListFields.ZestPricingPerUnit_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (needsListUpdate)
                    splist.Update();
            }
            catch (Exception ex)
            {

            }
        }
        private void CreatePLMSpecificationsList()
        {
            try
            {
                SPList splist = web.Lists.TryGetList(GlobalConstants.LIST_PLMSpecificationsListName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, GlobalConstants.LIST_PLMSpecificationsListName, GlobalConstants.LIST_PLMSpecificationsListName);
                    var field = splist.Fields[PLMSpecificationsListFields.Title];
                    field.Title = PLMSpecificationsListFields.MaterialNumber_DisplayName;
                    field.Update();
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, PLMSpecificationsListFields.Specification, PLMSpecificationsListFields.Specification_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PLMSpecificationsListFields.SpecType, PLMSpecificationsListFields.SpecType_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PLMSpecificationsListFields.SpecDescription, PLMSpecificationsListFields.SpecDescription_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PLMSpecificationsListFields.Identifier, PLMSpecificationsListFields.Identifier_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PLMSpecificationsListFields.Status, PLMSpecificationsListFields.Status_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, PLMSpecificationsListFields.StatusDescription, PLMSpecificationsListFields.StatusDescription_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (needsListUpdate)
                    splist.Update();
            }
            catch (Exception ex)
            {

            }
        }
        private void CreateSAPBOMList()
        {
            try
            {
                SPList splist = web.Lists.TryGetList(GlobalConstants.LIST_SAPBOMListName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, GlobalConstants.LIST_SAPBOMListName, GlobalConstants.LIST_SAPBOMListName);
                    var field = splist.Fields[SPBuiltInFieldId.Title];
                    field.Title = SAPBOMListFields.SAPItemNumber;
                    field.Update();
                }

                splist.EnableVersioning = false;
                splist.EnableMinorVersions = false;

                //Add Fields
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, SAPBOMListFields.HanaKey, SAPBOMListFields.HanaKey_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPBOMListFields.MaterialNumber, SAPBOMListFields.MaterialNumber_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPBOMListFields.MaterialDescription, SAPBOMListFields.MaterialDescription_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPBOMListFields.PackQuantity, SAPBOMListFields.PackQuantity_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPBOMListFields.PackUnit, SAPBOMListFields.PackUnit_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPBOMListFields.Plant, SAPBOMListFields.Plant_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                var choices = new List<string>{
                    "CANDY SEMI",
                    "PACK",
                    "TRANSFER SEMI",
                    "RAW"
                };
                if (SetupUtilities.CreateFieldChoice(splist, SAPBOMListFields.MaterialType, SAPBOMListFields.MaterialType_DisplayName, false, choices))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPBOMListFields.MaterialLevel, SAPBOMListFields.MaterialLevel_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPBOMListFields.MaterialParent, SAPBOMListFields.MaterialParent_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPBOMListFields.ItemOrder, SAPBOMListFields.ItemOrder_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (needsListUpdate)
                    splist.Update();
            }
            catch (Exception ex)
            {

            }
        }

        private void CreateSAPHanaStatusList()
        {
            try
            {
                SPList splist = web.Lists.TryGetList(GlobalConstants.LIST_SAPHanaStatusListName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, GlobalConstants.LIST_SAPHanaStatusListName, GlobalConstants.LIST_SAPHanaStatusListName);
                    var field = splist.Fields[SPBuiltInFieldId.Title];
                    field.Title = "Material";
                    field.Update();
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, SAPHanaStatusListFields.HanaKey, SAPHanaStatusListFields.HanaKey_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPHanaStatusListFields.Plant, SAPHanaStatusListFields.Plant, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, SAPHanaStatusListFields.BBlockOnItem, SAPHanaStatusListFields.BBlockOnItem_DisplayName, false, yesNoStates))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPHanaStatusListFields.MRPType, SAPHanaStatusListFields.MRPType, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, SAPHanaStatusListFields.POExists, SAPHanaStatusListFields.POExists_DisplayName, false, yesNoStates))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, SAPHanaStatusListFields.SourceListComplete, SAPHanaStatusListFields.SourceListComplete_DisplayName, false, yesNoStates))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, SAPHanaStatusListFields.StandardCostSet, SAPHanaStatusListFields.StandardCostSet_DisplayName, false, yesNoStates))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, SAPHanaStatusListFields.ZBlocksComplete, SAPHanaStatusListFields.ZBlocksComplete_DisplayName, false, yesNoStates))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateFieldChoice(splist, SAPHanaStatusListFields.SAPRoutings, SAPHanaStatusListFields.SAPRoutings_DisplayName, false, yesNoStates))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPHanaStatusListFields.CurrentAvailableQuantity, SAPHanaStatusListFields.CurrentAvailableQuantity_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPHanaStatusListFields.DateofFirstProduction, SAPHanaStatusListFields.DateofFirstProduction_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPHanaStatusListFields.QuantityofFirstProduction, SAPHanaStatusListFields.QuantityofFirstProduction_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPHanaStatusListFields.DateofOrder, SAPHanaStatusListFields.DateofOrder_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, SAPHanaStatusListFields.QuantityofOrder, SAPHanaStatusListFields.QuantityofOrder_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (needsListUpdate)
                    splist.Update();
            }
            catch (Exception ex)
            {

            }
        }

        private void CreateDragonflyStatusList()
        {
            try
            {
                SPList splist = web.Lists.TryGetList(GlobalConstants.LIST_DragonflyStatusListName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(web, GlobalConstants.LIST_DragonflyStatusListName, GlobalConstants.LIST_DragonflyStatusListName);
                    var field = splist.Fields[SPBuiltInFieldId.Title];
                    field.Title = "Project #";
                    field.Update();
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;

                //Add Fields
                var needsListUpdate = false;

                if (SetupUtilities.CreateField(splist, DragonflyStatusListFields.ItemNumber, DragonflyStatusListFields.ItemNumber_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyStatusListFields.MaterialNumber, DragonflyStatusListFields.MaterialNumber_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyStatusListFields.MaterialStatus, DragonflyStatusListFields.MaterialStatus_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyStatusListFields.ProductionArtStartedPlanned, DragonflyStatusListFields.ProductionArtStartedPlanned_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyStatusListFields.ProductionArtStartedEstimated, DragonflyStatusListFields.ProductionArtStartedEstimated_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyStatusListFields.ProductionArtStartedActual, DragonflyStatusListFields.ProductionArtStartedActual_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyStatusListFields.ProductionArtUploadedPlanned, DragonflyStatusListFields.ProductionArtUploadedPlanned_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyStatusListFields.ProductionArtUploadedEstimated, DragonflyStatusListFields.ProductionArtUploadedEstimated_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyStatusListFields.ProductionArtUploadedActual, DragonflyStatusListFields.ProductionArtUploadedActual_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyStatusListFields.RoutingCompletePlanned, DragonflyStatusListFields.RoutingCompletePlanned_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyStatusListFields.RoutingCompleteEstimated, DragonflyStatusListFields.RoutingCompleteEstimated_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyStatusListFields.RoutingCompleteActual, DragonflyStatusListFields.RoutingCompleteActual_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyStatusListFields.ProofingStartedPlanned, DragonflyStatusListFields.ProofingStartedPlanned_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyStatusListFields.ProofingStartedEstimated, DragonflyStatusListFields.ProofingStartedEstimated_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyStatusListFields.ProofingStartedActual, DragonflyStatusListFields.ProofingStartedActual_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyStatusListFields.ProofApprovedPlanned, DragonflyStatusListFields.ProofApprovedPlanned_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyStatusListFields.ProofApprovedEstimated, DragonflyStatusListFields.ProofApprovedEstimated_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyStatusListFields.ProofApprovedActual, DragonflyStatusListFields.ProofApprovedActual_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyStatusListFields.FinalFilesPlatesShippedPlanned, DragonflyStatusListFields.FinalFilesPlatesShippedPlanned_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyStatusListFields.FinalFilesPlatesShippedEstimated, DragonflyStatusListFields.FinalFilesPlatesShippedEstimated_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, DragonflyStatusListFields.FinalFilesPlatesShippedActual, DragonflyStatusListFields.FinalFilesPlatesShippedActual_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (needsListUpdate)
                    splist.Update();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
