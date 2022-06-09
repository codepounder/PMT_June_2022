using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using System.Collections.Generic;
using Microsoft.SharePoint.Security;
using Ferrara.Compass.Classes;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Enum;
using System.Text;
using Microsoft.Practices.Unity;
using Ferrara.Compass.DependencyResolution;
using Ferrara.Compass.Abstractions.Interfaces;

namespace Ferrara.Compass.Features.Compass.SupportLists
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("ace0f6e5-3c78-4998-a00c-0c155756c847")]
    public class CompassEventReceiver : SPFeatureReceiver
    {
        #region Variables
        private SPWeb currentWeb;
        private IDefaultListEntryService defaultListEntryService;

        List<string> yesNoChoices = new List<string>{
                    "Yes",
                    "No"
                };

        #endregion

        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            Init(properties);
            defaultListEntryService = DependencyMapper.Container.Resolve<IDefaultListEntryService>();

            CreateSupportLists();
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        {
            Init(properties);

            CreateSupportLists();
        }
        private void CreateSupportLists()
        {
            if (!SetupUtilities.ListExists(currentWeb, GlobalConstants.LIST_Configurations))
            {
                CreateConfigurationList();
                AddUpdateEventReceiver();
            }
            if (!SetupUtilities.ListExists(currentWeb, GlobalConstants.LIST_EmailTemplates))
            {
                CreateEmailTemplateList();
                AttachEmailTemplateListEventReceiver();
            }

            if (!SetupUtilities.ListExists(currentWeb, GlobalConstants.LIST_CompassTaskAssignmentListName))
            {
                CreateCompassTaskAssignmentList();
                AttachWFListEventReceiver();
            }
            else
            {
                // Update any new fields added to the list
                CreateCompassTaskAssignmentList();
            }

            if (!SetupUtilities.ListExists(currentWeb, GlobalConstants.LIST_LogsListName))
            {
                CreateLogList();
            }

            if (!SetupUtilities.ListExists(currentWeb, GlobalConstants.LIST_FormAccessListName))
            {
                CreateFormAccessList();
                AttachFormAccessListEventReceiver();
            }
            else
            {
                // Update any new fields added to the list
                CreateFormAccessList();
            }

            if (!SetupUtilities.ListExists(currentWeb, GlobalConstants.LIST_OBMBrandManagerLookupListName))
            {
                CreateOBMBrandManagerLookupList();
                AttachOBMBrandManagerLookupListEventReceiver();
            }
            else
            {
                // Update any new fields added to the list
                CreateOBMBrandManagerLookupList();
            }

            if (!SetupUtilities.ListExists(currentWeb, GlobalConstants.LIST_HolidayLookup))
            {
                CreateHolidayList();
                AttachHolidayListEventReceiver();
            }
            else
            {
                // Update any new fields added to the list
                CreateHolidayList();
            }

            CreateCompassNewsList();
            CreateProjectTimelineDetailsList();
            CreateCompassGraphicsLogsList();
            CreateWorldSyncNutritionalsList();
            CreateWorldSyncNutritionalsDetailList();
            CreateCompassWorldSyncList();

            PopulateConfigurationListData();
            PopulatingEmailTemplateListData();
            PopulatingWFListData();
            PopulatingFormAccessListData();
            PopulateOBMBrandManagerLookupData();
        }

        #region Helpers
        private void Init(SPFeatureReceiverProperties properties)
        {
            currentWeb = properties.Feature.Parent as SPWeb;
        }

        private void DeleteList(string listName)
        {
            var lists = currentWeb.Lists;
            var List = lists.TryGetList(listName);
            if (List != null)
            {
                List.Delete();
            }
        }

        private void DeleteEventReceiver(SPList spList)
        {
            for (int i = 0; i < spList.EventReceivers.Count; i++)
            {
                spList.EventReceivers[i].Delete();
            }
            spList.Update();
        }
        #endregion

        #region Configuration List

        private void CreateConfigurationList()
        {
            string description = "Configuration List to store Compass values";
            try
            {
                Guid listId = currentWeb.Lists.Add(GlobalConstants.LIST_Configurations, description, SPListTemplateType.GenericList);
                SPList list = currentWeb.Lists[listId];
                if (list == null) return;

                // Add Fields
                var strNewField = "<Field Type=\"Text\" " +
                                    "DisplayName=\"Value\" " +
                                    "Required=\"TRUE\" Name=\"Value\">" +
                                    "</Field>";
                list.Fields.AddFieldAsXml(strNewField, true, SPAddFieldOptions.AddFieldInternalNameHint);
                var titleField = list.Fields[SPBuiltInFieldId.Title];
                titleField.Indexed = true;
                titleField.EnforceUniqueValues = true;
                titleField.Update();

                list.Update();
            }
            catch (Exception ex)
            {
            }
        }
        private void AddUpdateEventReceiver()
        {
            SPList splist = currentWeb.Lists.TryGetList(GlobalConstants.LIST_Configurations);
            if (splist != null)
            {
                DeleteEventReceiver(splist);

                splist.EventReceivers.Add(SPEventReceiverType.ItemUpdated,
                    GlobalConstants.SharePointServiceAssemblyName,
                    GlobalConstants.ConfigurationListEventReceiverClassName);

                splist.EventReceivers.Add(SPEventReceiverType.ItemAdded,
                    GlobalConstants.SharePointServiceAssemblyName,
                    GlobalConstants.ConfigurationListEventReceiverClassName);

                splist.EventReceivers.Add(SPEventReceiverType.ItemDeleting,
                    GlobalConstants.SharePointServiceAssemblyName,
                    GlobalConstants.ConfigurationListEventReceiverClassName);

                splist.Update();
            }
        }

        private void PopulateConfigurationListData()
        {
            try
            {
                var configurationList = currentWeb.Lists.TryGetList(GlobalConstants.LIST_Configurations);
                if (configurationList == null) return;

                AddDefaultItems(configurationList, SystemConfiguration.SMTPServerName, "mail.ferrarausa.com");
                AddDefaultItems(configurationList, SystemConfiguration.SMTPFromEmailAddress, "SharePoint@ferrarausa.com");

                AddDefaultItems(configurationList, SystemConfiguration.SystemLogMaximumNumberOfLogsAllowed, "1000");
                AddDefaultItems(configurationList, SystemConfiguration.SystemLogNumberOfLogsToBeDeleted, "100");

                AddDefaultItems(configurationList, SystemConfiguration.CompassMessage, "NA");

                AddDefaultItems(configurationList, SystemConfiguration.RYGAverageCount, "1000");
                AddDefaultItems(configurationList, SystemConfiguration.HelpDeskEmail, "helpdesk@ferrarausa.com");
                AddDefaultItems(configurationList, SystemConfiguration.ManageEngineAPIKey, "43EF7062-6F89-45F4-8BA4-7660639D7235");
            }
            catch (Exception exception)
            {

            }
        }

        private static void AddDefaultItems(SPList configurationList, string sKey, string sValue)
        {
            var keyFieldName = "Title";
            var valueFieldName = "Value";
            foreach (SPListItem existingItem in configurationList.Items)
            {
                if (sKey == existingItem[keyFieldName].ToString())
                    return;
            }
            var item = configurationList.Items.Add();
            item[keyFieldName] = sKey;
            item[valueFieldName] = sValue;
            item.Update();
        }
        #endregion

        #region Email Template List
        private void CreateEmailTemplateList()
        {
            if (currentWeb != null)
            {
                SPList emailTemplateList = currentWeb.Lists.TryGetList(GlobalConstants.LIST_EmailTemplates);

                //Creating the List if List does not exist 
                if (emailTemplateList == null)
                {
                    emailTemplateList = SetupUtilities.CreateList(currentWeb, GlobalConstants.LIST_EmailTemplates, GlobalConstants.LIST_EmailTemplates);
                }

                bool needsListUpdate = false;

                bool fieldExist = emailTemplateList.Fields.ContainsFieldWithStaticName(EmailTemplateFieldName.Subject);
                if (!fieldExist)
                {
                    emailTemplateList.Fields.Add(EmailTemplateFieldName.Subject, SPFieldType.Note, true);

                    //Adding the Field to the Default View
                    SPView view = emailTemplateList.DefaultView;
                    view.ViewFields.Add(EmailTemplateFieldName.Subject);
                    view.Update();

                    needsListUpdate = true;
                }


                fieldExist = emailTemplateList.Fields.ContainsFieldWithStaticName(EmailTemplateFieldName.Body);
                if (!fieldExist)
                {
                    emailTemplateList.Fields.Add(EmailTemplateFieldName.Body, SPFieldType.Note, true);
                    SPView view = emailTemplateList.DefaultView;
                    view.ViewFields.Add(EmailTemplateFieldName.Body);
                    view.Update();
                    needsListUpdate = true;
                }


                if (needsListUpdate)
                    emailTemplateList.Update();
            }
        }
        private void AttachEmailTemplateListEventReceiver()
        {
            SPList emailTemplateList = currentWeb.Lists.TryGetList(GlobalConstants.LIST_EmailTemplates);
            if (emailTemplateList != null)
            {
                DeleteEventReceiver(emailTemplateList);

                emailTemplateList.EventReceivers.Add(SPEventReceiverType.ItemUpdated,
                    GlobalConstants.SharePointServiceAssemblyName,
                    GlobalConstants.EmailTemplateListEventReceiverClassName);

                emailTemplateList.EventReceivers.Add(SPEventReceiverType.ItemAdded,
                    GlobalConstants.SharePointServiceAssemblyName,
                    GlobalConstants.EmailTemplateListEventReceiverClassName);

                emailTemplateList.EventReceivers.Add(SPEventReceiverType.ItemDeleting,
                    GlobalConstants.SharePointServiceAssemblyName,
                    GlobalConstants.EmailTemplateListEventReceiverClassName);

                emailTemplateList.Update();
            }
        }
        private void DeleteEmailTemplateListData()
        {
            var allLists = currentWeb.Lists;
            var emailTemplateList = allLists.TryGetList(GlobalConstants.LIST_EmailTemplates);
            if (emailTemplateList != null)
            {
                for (int i = emailTemplateList.Items.Count - 1; i >= 0; i--)
                    emailTemplateList.Items.Delete(i);

                emailTemplateList.Update();
            }
        }
        private void PopulatingEmailTemplateListData()
        {
            var emailTemplateList = currentWeb.Lists.TryGetList(GlobalConstants.LIST_EmailTemplates);
            if (emailTemplateList == null) return;

            // Workflow Step Notification Templates
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BEQRC, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for BE QRC Form – Marketing Confirmation", GetHtmlBody(EmailTemplateKey.BEQRC.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BEQRCRequest, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - BE QRC Request – <#NameOfTheRequester#>", GetHtmlBody(EmailTemplateKey.BEQRCRequest.ToString()));

            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMACTIVEDATE_NOTIFICATION, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Set BOM Active Date", GetHtmlBody(EmailTemplateKey.BOMACTIVEDATE_NOTIFICATION.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupPE, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Packaging Engineer (1)", GetHtmlBody(EmailTemplateKey.BOMSetupPE.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupPE2, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Packaging Engineer (2)", GetHtmlBody(EmailTemplateKey.BOMSetupPE2.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProc, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProc.ToString()));

            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.CANCELLED, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Project Cancelled", GetHtmlBody(EmailTemplateKey.CANCELLED.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.COMPLETED, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Project Completed", GetHtmlBody(EmailTemplateKey.COMPLETED.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.ComponentCostingCorrugatedPaperboard, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Component Costing", GetHtmlBody(EmailTemplateKey.ComponentCostingCorrugatedPaperboard.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.ComponentCostingFilmLabelRigidPlastic, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Component Costing", GetHtmlBody(EmailTemplateKey.ComponentCostingFilmLabelRigidPlastic.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.ComponentCostingSeasonal, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Component Costing", GetHtmlBody(EmailTemplateKey.ComponentCostingSeasonal.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.COSTFINISHEDGOOD_NOTIFICATION, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Cost Finished Good", GetHtmlBody(EmailTemplateKey.COSTFINISHEDGOOD_NOTIFICATION.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.CUSTOMERPOSCANBEENTERED_NOTIFICATION, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Customer POs can be Entered", GetHtmlBody(EmailTemplateKey.CUSTOMERPOSCANBEENTERED_NOTIFICATION.ToString()));

            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.DEMANDFORECAST_NOTIFICATION, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Please Review and Update Forecast", GetHtmlBody(EmailTemplateKey.DEMANDFORECAST_NOTIFICATION.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.DEMANDPLANNING_NOTIFICATION, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Create CVCs in APO DP", GetHtmlBody(EmailTemplateKey.DEMANDPLANNING_NOTIFICATION.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.Distribution, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Request for Distribution Information", GetHtmlBody(EmailTemplateKey.Distribution.ToString()));

            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.ExternalMfg, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Request for External Manufacturing Information", GetHtmlBody(EmailTemplateKey.ExternalMfg.ToString()));

            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.FGPackSpec, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Finished Good Pack Spec", GetHtmlBody(EmailTemplateKey.FGPackSpec.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.FINALCOSTINGREVIEW_NOTIFICATION, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Final Costing Review", GetHtmlBody(EmailTemplateKey.FINALCOSTINGREVIEW_NOTIFICATION.ToString()));

            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.Graphics, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Graphics", GetHtmlBody(EmailTemplateKey.Graphics.ToString()));

            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.HELPDESK_ACCESS, "PMT Access Request: <#User#>-<#Form#>", GetHtmlBody(EmailTemplateKey.HELPDESK_ACCESS.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.HELPDESK_LOOKUP_REQUEST, "PMT Lookup Request: <#User#>-<#LookupList#>", GetHtmlBody(EmailTemplateKey.HELPDESK_LOOKUP_REQUEST.ToString()));

            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.INTERNATIONALCOMP_NOTIFICATION, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Notification of international Sale", GetHtmlBody(EmailTemplateKey.INTERNATIONALCOMP_NOTIFICATION.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.IPF, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Item Proposal", GetHtmlBody(EmailTemplateKey.IPF.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.IPF_REJECTED, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – REJECTED", GetHtmlBody(EmailTemplateKey.IPF_REJECTED.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.IPF_REQUESTFORINFORMATION, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Item Proposal: Request for Information", GetHtmlBody(EmailTemplateKey.IPF_REQUESTFORINFORMATION.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.IPF_Submission, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Item Proposal Submitted", GetHtmlBody(EmailTemplateKey.IPF_Submission.ToString()));

            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.NewFGWithReplacementItem, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Project <#ProjectNo#> FG <#SAP FG##> has been entered by <#Initiator#> with replacement item# : <#OldFinishedGoodItem##>", GetHtmlBody(EmailTemplateKey.NewFGWithReplacementItem.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.NewPackagingComponentsCreated, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – New Packaging Components Created", GetHtmlBody(EmailTemplateKey.NewPackagingComponentsCreated.ToString()));

            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.OBMReview1, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for PM First Review", GetHtmlBody(EmailTemplateKey.OBMReview1.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.OBMReview2, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Review of Graphics", GetHtmlBody(EmailTemplateKey.OBMReview2.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.OBMReview3, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Request for PM Third Review", GetHtmlBody(EmailTemplateKey.OBMReview3.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.ONHOLD, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Project Placed On-Hold", GetHtmlBody(EmailTemplateKey.ONHOLD.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.Operations, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Request for Operations and Initial Capacity", GetHtmlBody(EmailTemplateKey.Operations.ToString()));

            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.PE_NOTIFICATION, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Packaging Trial May be Required", GetHtmlBody(EmailTemplateKey.PE_NOTIFICATION.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.PRE_PRODUCTION, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Project Moved to Pre-Production", GetHtmlBody(EmailTemplateKey.PRE_PRODUCTION.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.PrelimSAPInitialSetup, "(SIRSAP) <#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Request for Preliminary Initial Item Setup", GetHtmlBody(EmailTemplateKey.PrelimSAPInitialSetup.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.PrelimSAPInitialSetupCompleted, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Preliminary Initial Item Setup Completed", GetHtmlBody(EmailTemplateKey.PrelimSAPInitialSetupCompleted.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.ProjectRejected, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Project Cancelled", GetHtmlBody(EmailTemplateKey.ProjectRejected.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.PURCHASEPO_NOTIFICATION, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Purchase POs", GetHtmlBody(EmailTemplateKey.PURCHASEPO_NOTIFICATION.ToString()));


            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.QA, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Request for InTech Regulatory Information", GetHtmlBody(EmailTemplateKey.QA.ToString()));

            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.REMOVESAPBLOCKS_NOTIFICATION, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Remove Sales Block TBD", GetHtmlBody(EmailTemplateKey.REMOVESAPBLOCKS_NOTIFICATION.ToString()));

            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPBOMSetup, "(SISSAP) <#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - SAP BOM Confirmation", GetHtmlBody(EmailTemplateKey.SAPBOMSetup.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPBOMSetupConfirmationExternalSemis, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Complete SAP BOM Setup", GetHtmlBody(EmailTemplateKey.SAPBOMSetupConfirmationExternalSemis.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPBOMSetupConfirmationExternalSubconFG, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Complete SAP BOM Setup", GetHtmlBody(EmailTemplateKey.SAPBOMSetupConfirmationExternalSubconFG.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPBOMSetupConfirmationExternalTurnkeyFG, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Complete SAP BOM Setup", GetHtmlBody(EmailTemplateKey.SAPBOMSetupConfirmationExternalTurnkeyFG.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPBOMSetupConfirmationExternalTurnkeyExistingFG, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Complete SAP BOM Setup", GetHtmlBody(EmailTemplateKey.SAPBOMSetupConfirmationExternalTurnkeyExistingFG.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPBOMSetupConfirmationInternalFG, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – SAP BOM Confirmation", GetHtmlBody(EmailTemplateKey.SAPBOMSetupConfirmationInternalFG.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPBOMSetupConfirmationNetworkMove, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Network Move Complete BOM Setup", GetHtmlBody(EmailTemplateKey.SAPBOMSetupConfirmationNetworkMove.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPBOMSetupConfirmationNetworkMoveTSs, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Network Move Transfer Semi Complete BOM Setup", GetHtmlBody(EmailTemplateKey.SAPBOMSetupConfirmationNetworkMoveTSs.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPBOMSetupConfirmationNewTSs, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – SAP BOM Confirmation", GetHtmlBody(EmailTemplateKey.SAPBOMSetupConfirmationNewTSs.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPBOMSetupConfirmationSubConNetworkMove, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – SubCon Network Move Complete BOM Setup", GetHtmlBody(EmailTemplateKey.SAPBOMSetupConfirmationSubConNetworkMove.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPBOMSetupConfirmationTurnkeyNetworkMove, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Turnkey Network Move Complete BOM Setup", GetHtmlBody(EmailTemplateKey.SAPBOMSetupConfirmationTurnkeyNetworkMove.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPCOSTINGDETAILS_NOTIFICATION, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Request Component Costing Quote", GetHtmlBody(EmailTemplateKey.SAPCOSTINGDETAILS_NOTIFICATION.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPInitialItemSetupConfirmationNewFG, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – TBD Initial Item Setup", GetHtmlBody(EmailTemplateKey.SAPInitialItemSetupConfirmationNewFG.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPInitialItemSetupConfirmationNewFGExternalTurnkeyFG, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – TBD Initial Item Setup", GetHtmlBody(EmailTemplateKey.SAPInitialItemSetupConfirmationNewFGExternalTurnkeyFG.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPInitialItemSetupConfirmationNewFGExternalSubconFG, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – TBD Initial Item Setup", GetHtmlBody(EmailTemplateKey.SAPInitialItemSetupConfirmationNewFGExternalSubconFG.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPInitialItemSetupConfirmationNewTSs, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – TBD Initial Transfer Semi Setup", GetHtmlBody(EmailTemplateKey.SAPInitialItemSetupConfirmationNewTSs.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPInitialItemSetupConfirmationNetworkMoveTSs, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Network Move TS Initial Item Setup", GetHtmlBody(EmailTemplateKey.SAPInitialItemSetupConfirmationNetworkMoveTSs.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPInitialItemSetupConfirmationExstNetworkMove, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Network Move Finished Good Initial Item Setup", GetHtmlBody(EmailTemplateKey.SAPInitialItemSetupConfirmationExstNetworkMove.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPInitialItemSetupConfirmationExstNetworkMoveTurnkey, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Network Move Initial Item Setup", GetHtmlBody(EmailTemplateKey.SAPInitialItemSetupConfirmationExstNetworkMoveTurnkey.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPInitialItemSetupConfirmationExstNetworkMoveSubCon, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Network Move Initial Item Setup", GetHtmlBody(EmailTemplateKey.SAPInitialItemSetupConfirmationExstNetworkMoveSubCon.ToString()));

            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPInitialItemSetupConfirmationNewFGTurnkeyFC01, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Turnkey FG FC01 Initial Item Setup", GetHtmlBody(EmailTemplateKey.SAPInitialItemSetupConfirmationNewFGTurnkeyFC01.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPInitialItemSetupConfirmationNewPCFC01, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – New Purchased Semi FC01 Setup", GetHtmlBody(EmailTemplateKey.SAPInitialItemSetupConfirmationNewPCFC01.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPInitialItemSetupConfirmationNMPCFC01, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Network Move Purchased Semi FC01 Initial Item Setup", GetHtmlBody(EmailTemplateKey.SAPInitialItemSetupConfirmationNMPCFC01.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPInitialItemSetupConfirmationExistingFGNMFC01, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Turnkey FG FC01 Network Move Initial Item Setup", GetHtmlBody(EmailTemplateKey.SAPInitialItemSetupConfirmationExistingFGNMFC01.ToString()));

            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPInitialSetup, "(SIRSAP) <#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – (TBD/New FG#) Request for Initial Item Setup", GetHtmlBody(EmailTemplateKey.SAPInitialSetup.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPROUTINGSETUP_NOTIFICATION, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – SAP Final Routing Setup", GetHtmlBody(EmailTemplateKey.SAPROUTINGSETUP_NOTIFICATION.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPWAREHOUSEINFO_NOTIFICATION, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Warehouse Information into SAP", GetHtmlBody(EmailTemplateKey.SAPWAREHOUSEINFO_NOTIFICATION.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SrOBMApproval, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Request for PM Initial Leadership Review", GetHtmlBody(EmailTemplateKey.SrOBMApproval.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.STANDARDCOSTENTRY_NOTIFICATION, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> – Standard Cost Entry", GetHtmlBody(EmailTemplateKey.STANDARDCOSTENTRY_NOTIFICATION.ToString()));

            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.TradePromo, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> –Request for Trade Input and Apply ZEST Pricing", GetHtmlBody(EmailTemplateKey.TradePromo.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.EstBracketPricing, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> –Request for Initial Bracket Pricing", GetHtmlBody(EmailTemplateKey.EstBracketPricing.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.EstPricing, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> –Request for Initial Pricing", GetHtmlBody(EmailTemplateKey.EstPricing.ToString()));

            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.WorldSyncReqImageRequested, "A new worldsync image has been requested", GetHtmlBody(EmailTemplateKey.WorldSyncReqImageRequested.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.WorldSyncReqNutritionalRequested, "A new worldsync nutritional has been requested", GetHtmlBody(EmailTemplateKey.WorldSyncReqNutritionalRequested.ToString()));

            //Stage Gae Post launch Notification
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.StageGatePostLaunchNotification, "REMINDER: PROJECT: <#PARENTPROJECTNUMBER#>- <#PARENTPROJECTNAME#> - Post Launch <#NOTIFICATIONTYPE#> Preparation", GetHtmlBody(EmailTemplateKey.StageGatePostLaunchNotification.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.ParentProjectSubmitted, "<#PARENTPROJECTNUMBER#>- <#PARENTPROJECTNAME#> - Parent Project Submitted", GetHtmlBody(EmailTemplateKey.ParentProjectSubmitted.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.ParentProjectCompleted, "<#PARENTPROJECTNUMBER#>- <#PARENTPROJECTNAME#> - Parent Project Completed", GetHtmlBody(EmailTemplateKey.ParentProjectCompleted.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.ParentProjectCancelled, "<#PARENTPROJECTNUMBER#>- <#PARENTPROJECTNAME#> - Parent Project Cancelled", GetHtmlBody(EmailTemplateKey.ParentProjectCancelled.ToString()));

            //Procurement Tasks
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcSeasonal, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcSeasonal.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcNovelty, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcNovelty.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcCoMan, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcCoMan.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcEBPCorrugated, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcEBPCorrugated.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcEBPOther, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcEBPOther.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcEBPAncillary, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcEBPAncillary.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcEBPPaperboard, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcEBPPaperboard.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcEBPFilm, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcEBPFilm.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcEBPPurchased, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcEBPPurchased.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcEBPLabel, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcEBPLabel.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcEBPRigidPlastic, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcEBPRigidPlastic.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcEBPMetal, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcEBPMetal.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcSeasonalCorrugated, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcSeasonalCorrugated.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcSeasonalOther, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcSeasonalOther.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcSeasonalAncillary, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcSeasonalAncillary.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcSeasonalPaperboard, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcSeasonalPaperboard.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcSeasonalFilm, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcSeasonalFilm.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcSeasonalPurchased, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcSeasonalPurchased.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcSeasonalLabel, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcSeasonalLabel.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcSeasonalRigidPlastic, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcSeasonalRigidPlastic.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcSeasonalMetal, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcSeasonalMetal.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcExternalCorrugated, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcExternalCorrugated.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcExternalOther, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcExternalOther.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcExternalAncillary, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcExternalAncillary.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcExternalPaperboard, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcExternalPaperboard.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcExternalFilm, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcExternalFilm.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcExternalPurchased, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcExternalPurchased.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcExternalLabel, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcExternalLabel.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcExternalRigidPlastic, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcExternalRigidPlastic.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcExternalMetal, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcExternalMetal.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcExternal, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcExternal.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcCorrugated, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcCorrugated.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcOther, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcOther.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcAncillary, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcAncillary.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcPaperboard, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcPaperboard.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcFilm, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcFilm.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcPurchased, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcPurchased.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcLabel, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcLabel.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcRigidPlastic, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcRigidPlastic.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupProcMetal, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation from Procurement (Co-Man and Packaging)", GetHtmlBody(EmailTemplateKey.BOMSetupProcMetal.ToString()));

            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupMaterialWarehouse, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Warehouse Setup Completion", GetHtmlBody(EmailTemplateKey.BOMSetupMaterialWarehouse.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupPE3, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for Material Confirmation (PE3)", GetHtmlBody(EmailTemplateKey.BOMSetupPE3.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.BOMSetupPE3Submitted, "<#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Material Confirmation (PE3) Submitted", GetHtmlBody(EmailTemplateKey.BOMSetupPE3Submitted.ToString()));
            AddEmailTemplateToList(emailTemplateList, EmailTemplateKey.SAPCompleteItemSetup, "(SISSAP) <#ProjectNo#> - <#SAP FG##> - <#SAP DESCRIPTION#> - Request for SAP Complete Item Setup (Master Data)", GetHtmlBody(EmailTemplateKey.SAPCompleteItemSetup.ToString()));

            // Email Notification Templates

            emailTemplateList.Update();
        }
        private static void AddEmailTemplateToList(SPList emailTemplateList, EmailTemplateKey keyName, string subject, string body)
        {
            foreach (SPListItem existingItem in emailTemplateList.Items)
            {
                if (keyName.ToString() == existingItem[EmailTemplateFieldName.Title].ToString())
                    return;
            }

            var spListItem = emailTemplateList.Items.Add();
            spListItem[EmailTemplateFieldName.Title] = keyName;
            spListItem[EmailTemplateFieldName.Subject] = subject;
            spListItem[EmailTemplateFieldName.Body] = body;
            spListItem.Update();
        }

        #region Email HTML Body
        private static string GetHtmlBody(string emailTemplateKey)
        {
            var sb = new StringBuilder();
            if (string.Equals(emailTemplateKey, EmailTemplateKey.BEQRC.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for BE QRC Form – Marketing Confirmation<br><br> ");
                sb.AppendLine("Please input any BE QRC direction for any and all packaging components within this Finished Good.");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>BE QRC Form – Marketing Confirmation</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.BEQRCRequest.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for BE QRC Form – Marketing Confirmation<br><br> ");
                sb.AppendLine("<br>");
                sb.AppendLine("<U><B>Key Project Information:</B></U>");
                sb.AppendLine("Project Leader : <#ProjectLeader#>");
                sb.AppendLine("Marketer : <#Marketer#>");
                sb.AppendLine("Project Manager : <#OBM#>");
                sb.AppendLine("UPC : <#UPC#>");
                sb.AppendLine("Display UPC: <#DisplayUPC#>");
                sb.AppendLine("UCC: <#UCC#>");
                sb.AppendLine("<br>");
                sb.AppendLine("The below item requires QR codes to be assigned for the following packaging components");
                sb.AppendLine("<#PackagingComponentsQRCodesTable#>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>BE QRC Form – Marketing Confirmation</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.BOMACTIVEDATE_NOTIFICATION.ToString()))
            {
                sb.AppendLine("Set BOM Active Date");
                sb.AppendLine("<br>");
                sb.AppendLine("You are receiving this email as a notification of this upcoming project.<br>");
                sb.AppendLine("If you feel you have received this email in error, please contact the PM.<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("First Ship Date: <#FIRST SHIP DATE#>");
                sb.AppendLine("Flowthrough: <#NEEDS INPUT#>");
                sb.AppendLine("PM: <#OBM#>");
                sb.AppendLine("Brand Manager: <#BRAND MGR#>");
                sb.AppendLine("New Materials:<br>");
                sb.AppendLine("<#NEW MATERIALS#>");
                sb.AppendLine("<br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.BOMSetupPE.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for Material Confirmation (PE1)<br><br> ");
                sb.AppendLine("<B>* Packaging Engineer *</B><br>");
                sb.AppendLine("Please verify all necessary packaging components.  If existing materials can be used, please input the existing material number in the Material Number Column. If new material numbers are necessary, please enter 'NEEDS NEW'.");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>Bill of Materials (PE) Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.BOMSetupPE2.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for Material Confirmation (PE2)<br><br>");
                sb.AppendLine("<B>* Packaging Engineer *</B><br>");
                sb.AppendLine("Please input all needed packaging components information and details and add pallet pattern information.");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>Bill of Materials (PE2) Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (emailTemplateKey.Contains(EmailTemplateKey.BOMSetupProc.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for Material Confirmation (Procurement)<br><br>");
                sb.AppendLine("<B>* Packaging Procurement *</B><br>");
                sb.AppendLine("Please input all needed packaging components information and details.");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>Bill of Materials (Proc) Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.CANCELLED.ToString()))
            {
                sb.AppendLine("***NOTIFICATION ONLY***");
                sb.AppendLine("<br><br>");
                sb.AppendLine("PROJECT CANCELLED");
                sb.AppendLine("<br>");
                sb.AppendLine("Cancelled By: <#ModifiedBy#>");
                sb.AppendLine("<br>");
                sb.AppendLine("Cancellation Reason: <#CANCELREASON#>");
                sb.AppendLine("<br>");
                sb.AppendLine("<#CHANGEREQUESTNEWPROJECTLINK#>");
                sb.AppendLine("<br><br>");
                sb.AppendLine("You are receiving this email as a notification that this project has been cancelled. No further action is required at this time.<br><br>");
                sb.AppendLine("If you feel you have received this email in error, please contact the project's PM.<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>.");
                sb.AppendLine("<br>");
                sb.AppendLine("SAP Item #: <#SAP FG##>.");
                sb.AppendLine("<br>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>.");
                sb.AppendLine("<br>");
                sb.AppendLine("PM: <#OBM#>.");
                sb.AppendLine("<br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#>");
                sb.AppendLine("<br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#>");
                sb.AppendLine("<br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.ProjectRejected.ToString()))
            {
                sb.AppendLine("***NOTIFICATION ONLY***");
                sb.AppendLine("<br><br>");
                sb.AppendLine("PROJECT REJECTED");
                sb.AppendLine("<br>");
                sb.AppendLine("Rejected By: <#REJECTEDBY#>");
                sb.AppendLine("<br>");
                sb.AppendLine("Function Rejected: <#FUNCTIONREJECTED#>");
                sb.AppendLine("<br>");
                sb.AppendLine("Reason for Rejection: <#REASONFORREJECTION#>");
                sb.AppendLine("<br>");
                sb.AppendLine("<#CHANGEREQUESTNEWPROJECTLINK#>");
                sb.AppendLine("<br><br>");
                sb.AppendLine("You are receiving this email as a notification that this project has been rejected. No further action is required at this time for Form Submitters.<br>");
                sb.AppendLine("<B>INITIATOR</B>: MUST re-classify & re-submit project.<br><br>");
                sb.AppendLine("If you feel you have received this email in error, please contact the project's PM.<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>.");
                sb.AppendLine("<br>");
                sb.AppendLine("SAP Item #: <#SAP FG##>.");
                sb.AppendLine("<br>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>.");
                sb.AppendLine("<br>");
                sb.AppendLine("PM: <#OBM#>.");
                sb.AppendLine("<br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#>");
                sb.AppendLine("<br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#>");
                sb.AppendLine("<br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.COMPLETED.ToString()))
            {
                sb.AppendLine("<#ProjectNo#>.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.ComponentCostingCorrugatedPaperboard.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for Corrugated/Paperboard Component Costing - Review/Submit Costing Quote<br><br>");
                sb.AppendLine("<B>* <#OBM#> *</B><br><br>");
                sb.AppendLine("The appropriate PMT tasks have been completed in order to proceed with the Component Costing Request for this project.");
                sb.AppendLine("<br><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>Component Costing Summary Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.ComponentCostingFilmLabelRigidPlastic.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for Film/Label/Rigid Plastic Component Costing - Review/Submit Costing Quote<br><br>");
                sb.AppendLine("<B>* <#OBM#> *</B><br><br>");
                sb.AppendLine("The appropriate PMT tasks have been completed in order to proceed with the Component Costing Request for this project.");
                sb.AppendLine("<br><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>Component Costing Summary Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.ComponentCostingSeasonal.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for Seasonal Component Costing - Review/Submit Costing Quote<br><br>");
                sb.AppendLine("<B>* <#OBM#> *</B><br><br>");
                sb.AppendLine("The appropriate PMT tasks have been completed in order to proceed with the Component Costing Request for this project.");
                sb.AppendLine("<br><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>Component Costing Summary Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.COSTFINISHEDGOOD_NOTIFICATION.ToString()))
            {
                sb.AppendLine("Cost Finished Good");
                sb.AppendLine("<br>");
                sb.AppendLine("You are receiving this email as a notification of this upcoming project.<br>");
                sb.AppendLine("If you feel you have received this email in error, please contact the PM.<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("New Materials:<br>");
                sb.AppendLine("<#NEW MATERIALS#>");
                sb.AppendLine("<br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.CUSTOMERPOSCANBEENTERED_NOTIFICATION.ToString()))
            {
                sb.AppendLine("Customer POs can be Entered");
                sb.AppendLine("<br>");
                sb.AppendLine("You are receiving this email as a notification of this upcoming project.<br>");
                sb.AppendLine("If you feel you have received this email in error, please contact the PM.<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("New Materials:<br>");
                sb.AppendLine("<#NEW MATERIALS#>");
                sb.AppendLine("<br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.DEMANDFORECAST_NOTIFICATION.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Review Demand Forecast<br><br>");
                sb.AppendLine("<B>* Demand Planning *</B><br>");
                sb.AppendLine("You are receiving this email as a notification of this upcoming project. Please Review the Demand Forecast for this item at this time and update if any values are incorrect.<br>");
                sb.AppendLine("If you feel you have received this email in error, please contact the PM.<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("1st Month Demand Forecast: <#FORECAST1#>");
                sb.AppendLine("2nd Month Demand Forecast: <#FORECAST2#>");
                sb.AppendLine("3rd Month Demand Forecast: <#FORECAST3#>");
                sb.AppendLine("Annualized Demand Forecast: <#ANNUAL FORECAST#>");
                sb.AppendLine("<br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.DEMANDPLANNING_NOTIFICATION.ToString()))
            {
                sb.AppendLine("Create CVCs in APO DP");
                sb.AppendLine("<br>");
                sb.AppendLine("You are receiving this email as a notification of this upcoming project. Please Create CVCs in APO DP for this item at this time.<br>");
                sb.AppendLine("If you feel you have received this email in error, please contact the PM.<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("Designate HUB DC (aka Material: Delivery Plant): <#DESIGNATEHUBDC#>");
                sb.AppendLine("Customer Specific: <#CUSTOMER SPECIFIC#>");
                sb.AppendLine("Customer: <#CUSTOMER#>");
                sb.AppendLine("<br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.Distribution.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for Distribution Information<br><br>");
                sb.AppendLine("<B>* Distribution Team *<br></B>");
                sb.AppendLine("Please provide the Distribution locations for this project.<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>Distribution Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.ExternalMfg.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for External Manufacturing Information<br>");
                sb.AppendLine("<B>* External Manufacturing *</B>");
                sb.AppendLine("<br><br>");
                sb.AppendLine("Please provide the Manufacturing and Packing locations.<br><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>External Manufacturing Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.FGPackSpec.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Finshed Good Pack Spec<br><br>");
                sb.AppendLine("<B>* Packaging Engineer *</B><br>");
                sb.AppendLine("Please input all needed information and details.");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>Finshed Good Pack Spec Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.FINALCOSTINGREVIEW_NOTIFICATION.ToString()))
            {
                sb.AppendLine("Final Costing Review");
                sb.AppendLine("<br>");
                sb.AppendLine("The cost has been created for the below items:<br>");
                sb.AppendLine("Please review the BOM, Routing, Scrap Rates, Overweight, etc., and respond back to Glenn Fink ");
                sb.AppendLine("<b><u>as soon as possible within 24 hours</u></b> that everything is okay (Approved) or ");
                sb.AppendLine("if changes are needed. Once it is approved, cost will be released.<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("<br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("New Materials:<br>");
                sb.AppendLine("<#NEW MATERIALS#>");
                sb.AppendLine("<br>PMT Links:<br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.Graphics.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for Graphics<br><br>");
                sb.AppendLine("<B>* Graphics *</B><br>");
                sb.AppendLine("Please see graphics request form for all needed information and details for each component.");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Initiator: <#Initiator#><br>");
                sb.AppendLine("PM: <#OBM#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>Graphics Request Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.HELPDESK_ACCESS.ToString()))
            {
                sb.AppendLine("Dear Helpdesk:");
                sb.AppendLine("<br><br>");
                sb.AppendLine("<#User#> is requesting access to PMT.");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Request was intiated from the <#Form#> form.");
                sb.AppendLine("<br><br>");
                sb.AppendLine("Please follow up with this user to determine what access rights need to be granted.<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.HELPDESK_LOOKUP_REQUEST.ToString()))
            {
                sb.AppendLine("Dear Helpdesk:");
                sb.AppendLine("<br><br>");
                sb.AppendLine("<#User#> is requesting a new lookup value in PMT.");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Request was intiated for the <#LookupList#> lookup list with the new value of '<#Value#>'.<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.INTERNATIONALCOMP_NOTIFICATION.ToString()))
            {
                sb.AppendLine("International Compliance Team");
                sb.AppendLine("<br>");
                sb.AppendLine("You are receiving this email as a notification of this upcoming project.<br>");
                sb.AppendLine("If you feel you have received this email in error, please contact the PM.<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("PM: <#OBM#>");
                sb.AppendLine("Make Location: <#MAKE LOCATION#>");
                sb.AppendLine("Make Location Country: <#MAKE COUNTRY#>");
                sb.AppendLine("1st Pack Location: <#PACK LOCATION1#>");
                sb.AppendLine("Organic?: <#ORGANIC#>");
                sb.AppendLine("First Ship Date: <#FIRST SHIP DATE#>");
                sb.AppendLine("<br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.IPF.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Item Proposal in Process<br><br>");
                sb.AppendLine("<br><br>");
                sb.AppendLine("Please be advised that project <#ProjectNo#>, has not been submitted yet. Please complete the item proposal and submit.");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>Item Proposal Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.IPF_REJECTED.ToString()))
            {
                sb.AppendLine("Dear Brand Lead/PM:");
                sb.AppendLine("<br><br>");
                sb.AppendLine("*** ATTENTION ***");
                sb.AppendLine("<br><br>");
                sb.AppendLine("Project #: <#ProjectNo#>.<br>");
                sb.AppendLine("Proposed Item: <#Proposed Item#>.<br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("<br>");
                sb.AppendLine("You are receiving this email because the project has been REJECTED. <br>");
                sb.AppendLine("Please find the comments below supplied by the reviewer as to why this project was rejected:<br><br>");
                sb.AppendLine("Approver Comments: <br>");
                sb.AppendLine("<#Approver Comments#>.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.IPF_REQUESTFORINFORMATION.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for Information");
                sb.AppendLine("<br><br>");
                sb.AppendLine("Please be advised that project <#ProjectNo#>, has been resubmitted for more information. Please provide the requested information.<br>");
                sb.AppendLine("Requested Update Comments: <#Approver Comments#>");
                sb.AppendLine("<br><br>");
                sb.AppendLine("Project #: <#ProjectNo#>.");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:");
                sb.AppendLine("<a href=\"<#FormLink#>\">Item Proposal Form</a><br>");
                sb.AppendLine("<a href=\"<#CommFormLink#>\">Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'.Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.IPF_Submission.ToString()))
            {
                sb.AppendLine("<br><br>");
                sb.AppendLine("Please be advised that an Item Proposal for the Finished Good <#SAP FG##>, Project <#ProjectNo#> has been submitted.");
                sb.AppendLine("<br><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=\"<#FormLink#>\">Item Proposal Form</a><br>");
                sb.AppendLine("<a href=\"<#CommFormLink#>\">Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.NewFGWithReplacementItem.ToString()))
            {
                sb.AppendLine("<br><br>");
                sb.AppendLine("Project <#ProjectNo#> FG <#SAP FG##> has been entered by <#Initiator#> with replacement item# : <#OldFinishedGoodItem##>.<br>");
                sb.AppendLine("<br><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=\"<#FormLink#>\">SAP Initial Item Setup Form</a><br>");
                sb.AppendLine("<a href=\"<#CommFormLink#>\">Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Thank you.<br><br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.NewPackagingComponentsCreated.ToString()))
            {
                sb.AppendLine("<br><br>Finished Good BOM has been updated with the New Packaging Components assigned to <span style=\"color:blue;font-weight:bold\"><U><big><#PACK LOCATION1#></big></U></span>. Please see below for details on if any materials will be flowed through (Hard vs Soft Transition).<br><br><br>");
                sb.AppendLine("<h3><u>Current Material Numbers</u> :</h3><br>");
                sb.AppendLine("<#Packaging Components SAPsetupBOM#><br><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#><br>");
                sb.AppendLine("SAP Item #: <#SAP FG##><br>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>SAP BOM Setup Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Thank you.<br><br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.OBMReview1.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for PM First Review<br>");
                sb.AppendLine("<B>* <#OBM#> *</B><br>");
                sb.AppendLine("Please request all necessary information required for PMREV1 and ensure appropriate tasks/forms are complete before sending this project to Material Confirmation.<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>PM First Review Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click ‘submit’. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.OBMReview2.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for PM 2nd Review - Review/Submit Graphics Request<br><br>");
                sb.AppendLine("<B>* <#OBM#> *</B><br><br>");
                sb.AppendLine("The appropriate PMT tasks have been completed in order to proceed with the Graphics Request for this project - please review and select the appropriate Graphics vendor(s).");
                sb.AppendLine("<br><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#><br>");
                sb.AppendLine("SAP Item #: <#SAP FG##><br>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("PM: <#OBM#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>PM Second Review Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.OBMReview3.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for PM Third Review<br><br>");
                sb.AppendLine("<B>* <#OBM#> *</B><br><br>");
                sb.AppendLine("Please ensure all routing and costs have been submitted and sales blocks are removed prior to moving this project to Pre-Production.<br><br>");
                sb.AppendLine("You can access the PM Third Review Form here: <#FormLink#>.");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#><br>");
                sb.AppendLine("SAP Item #: <#SAP FG##><br>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>PM Third Review Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click ‘submit’. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.ONHOLD.ToString()))
            {
                sb.AppendLine("Project #: <#ProjectNo#><br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.Operations.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for Operations and Initial Capacity<br><br>");
                sb.AppendLine("<B>* Operations and Initial Capacity *<br></B>");
                sb.AppendLine("Please provide the Manufacturing and Packing locations.<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>Operations & Initial Capacity Review Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.PE_NOTIFICATION.ToString()))
            {
                sb.AppendLine("NO ACTION REQUIRED: PE Notification<br><br>");
                sb.AppendLine("<B>* Packaging Engineers *<br></B>");
                sb.AppendLine("You are receiving this email as a notification of this upcoming project that may require a pack trial. No further action is required at this time.<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("*** ATTENTION ***<br>");
                sb.AppendLine("Project #: <#ProjectNo#>.");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("Make Location: <#MAKE LOCATION#>");
                sb.AppendLine("1st Pack Location: <#PACK LOCATION1#>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>Item Proposal Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("If you feel you have received this email in error, please contact the project's PM.<br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.PRE_PRODUCTION.ToString()))
            {
                sb.AppendLine("Project #: <#ProjectNo#><br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.PrelimSAPInitialSetup.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for Preliminary Initial Item Setup<br><br>");
                sb.AppendLine("<B>* Master Data *</B><br>");
                sb.AppendLine("The appropriate PMT tasks have been completed in order to proceed with the Preliminary Initial Item Setup for this project.");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>> Preliminary SAP Initial Item Setup Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.PrelimSAPInitialSetupCompleted.ToString()))
            {
                sb.AppendLine("<br>The Preliminary SAP Setup form has been completed. Please click on the link below to reference the new numbers generated.<br><br>");
                sb.AppendLine("<br>PRELIMINARY SAP SETUP COMPLETE <br><br>");
                sb.AppendLine("<h2 style=\"border-bottom:1px solid;color:#084C61\">Intial Item Setup</h2><br>");
                sb.AppendLine("<table style='width: 100%;'>");
                sb.AppendLine("    <tr>");
                sb.AppendLine("        <td>");
                sb.AppendLine("            <table style='width: 100%;'>");
                sb.AppendLine("                <tr><td style=\"font-weight:700\">SAP Item #:</td></tr>");
                sb.AppendLine("                <tr><td style=\"border: 1px solid #ccc;border-radius:4px; padding:6px 12px\"><#SAP FG##></td></tr>");
                sb.AppendLine("            </table>");
                sb.AppendLine("        </td>");
                sb.AppendLine("        <td>");
                sb.AppendLine("            <table style='width: 100%;'>");
                sb.AppendLine("                <tr><td style=\"font-weight:700\">SAP Description:</td></tr>");
                sb.AppendLine("                <tr><td style=\"border: 1px solid #ccc;border-radius:4px; padding:6px 12px\"><#SAP DESCRIPTION#></td></tr>");
                sb.AppendLine("            </table>");
                sb.AppendLine("        </td>");
                sb.AppendLine("    </tr>");
                sb.AppendLine("    <tr>");
                sb.AppendLine("        <td>");
                sb.AppendLine("            <table style='width: 100%;'>");
                sb.AppendLine("                <tr><td style=\"font-weight:700\">Unit UPC:</td></tr>");
                sb.AppendLine("                <tr><td style=\"border: 1px solid #ccc;border-radius:4px; padding:6px 12px\"><#UnitUPC#></td></tr>");
                sb.AppendLine("            </table>");
                sb.AppendLine("        </td>");
                sb.AppendLine("        <td></td>");
                sb.AppendLine("    </tr>");
                sb.AppendLine("    <tr>");
                sb.AppendLine("        <td>");
                sb.AppendLine("            <table style='width: 100%;'>");
                sb.AppendLine("                <tr><td style=\"font-weight:700\">Jar/Display UPC:</td></tr>");
                sb.AppendLine("                <tr><td style=\"border: 1px solid #ccc;border-radius:4px; padding:6px 12px\"><#JarDisplayUPC#></td></tr>");
                sb.AppendLine("            </table>");
                sb.AppendLine("        </td>");
                sb.AppendLine("        <td></td>");
                sb.AppendLine("    </tr>");
                sb.AppendLine("    <tr>");
                sb.AppendLine("        <td>");
                sb.AppendLine("            <table style='width: 100%;'>");
                sb.AppendLine("                <tr><td style=\"font-weight:700\">Case UCC:</td></tr>");
                sb.AppendLine("                <tr><td style=\"border: 1px solid #ccc;border-radius:4px; padding:6px 12px\"><#CaseUCC#></td></tr>");
                sb.AppendLine("            </table>");
                sb.AppendLine("        </td>");
                sb.AppendLine("        <td></td>");
                sb.AppendLine("    </tr>");
                sb.AppendLine("</table>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>> Preliminary SAP Initial Item Setup Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.PURCHASEPO_NOTIFICATION.ToString()))
            {
                sb.AppendLine("Purchase POs");
                sb.AppendLine("<br>");
                sb.AppendLine("You are receiving this email as a notification of this upcoming project.<br>");
                sb.AppendLine("If you feel you have received this email in error, please contact the PM.<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("New Materials:<br>");
                sb.AppendLine("<#NEW MATERIALS#>");
                sb.AppendLine("<br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.QA.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for NLEAs and InTech Regulatory Information<br><br>");
                sb.AppendLine("<B>* InTech Regulatory Team *<br></B>");
                sb.AppendLine("Please provide the NLEAs and InTech Regulatory information for this project.<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>InTech Regulatory Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.REMOVESAPBLOCKS_NOTIFICATION.ToString()))
            {
                sb.AppendLine("Remove Sales Blocks TBD");
                sb.AppendLine("<br>");
                sb.AppendLine("You are receiving this email as a notification of this upcoming project.<br>");
                sb.AppendLine("If you feel you have received this email in error, please contact the PM.<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("New Materials:<br>");
                sb.AppendLine("<#NEW MATERIALS#>");
                sb.AppendLine("<br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPBOMSetup.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for BOM Confirmation (Master Data)<br><br>");
                sb.AppendLine("<B>* Master Data *</B><br>");
                sb.AppendLine("Please input all needed packaging components information and details, create new packaging material numbers, and add new packaging materials in to the BOM.");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>SAP BOM Setup Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPBOMSetupConfirmationExternalSemis.ToString()))
            {
                sb.AppendLine("<br><br>The below New PUR CANDY has been set-up to be purchased into following locations. <span style=\"background-color:#FFFF00\">Item needs purchased price/PIR.</span><br><br><br>");
                sb.AppendLine("<#NEW PURCANDY SEMIS#><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#><br>");
                sb.AppendLine("SAP Item #: <#SAP FG##><br>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>SAP BOM Setup Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPBOMSetupConfirmationExternalSubconFG.ToString()))
            {
                sb.AppendLine("<br><br>SubCon Item <span style=\"color:blue;font-weight:bold\"><U><big><#SAP FG##></big></U></span> has been set-up in the pack location has <span style=\"color:blue;font-weight:bold\"><U><big><#EXTERNAL FINISHED GOOD PACKER#></big></U></span> and purchased into <span style=\"color:blue;font-weight:bold\"><U><big><#HUB DC#></big></U></span>. <span style=\"background-color:#FFFF00\">Item needs purchased price/PIR</span>.");
                sb.AppendLine("<br><br><span style=\"font-weight:bold\"><big>Procurement:</big></span>");
                sb.AppendLine("<ul>");
                sb.AppendLine("     <li>Please confirm the purchased price and create the vendor PIR to be set up in the Purchased Into Location indicated above.</li>");
                sb.AppendLine("     <li>Send to HQ Costing Team the SUBK PIR for costing.</li>");
                sb.AppendLine("</ul>");
                sb.AppendLine("<br><br><span style=\"font-weight:bold\"><big>HQ Costing Team:</big></span>");
                sb.AppendLine("<br>First cost in the Subcontract Plant, then follow the path below:");
                sb.AppendLine("<ul>");
                sb.AppendLine("     <li>If FG or Transfer Semi and <span style=\"font-weight:bold\"><big>Confections/Fruit Snacks</big></span>, please extend costing to FPCO and all FP and FX DCs.</li>");
                sb.AppendLine("     <li>If FG or Transfer Semi and <span style=\"font-weight:bold\"><big>CCC</big></span>, please extend costing to FPCO and all SELL DCs.</li>");
                sb.AppendLine("     <li>If FG or Transfer Semi and <span style=\"font-weight:bold\"><big>LBB</big></span>, please extend to FPCO and all FERQ DCs.</li>");
                sb.AppendLine("</ul>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#><br>");
                sb.AppendLine("SAP Item #: <#SAP FG##><br>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("Deployment Mode: <#DEPLOYMENT MODE#><br>");
                sb.AppendLine("HUB DC: <#HUB DC#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>SAP BOM Setup Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPBOMSetupConfirmationExternalTurnkeyFG.ToString()))
            {
                sb.AppendLine("<br><br>Turnkey Item <span style=\"color:blue;font-weight:bold\"><U><big><#SAP FG##></big></U></span> has been set-up in the pack location <span style=\"color:blue;font-weight:bold\"><U><big><#PACK LOCATION1#></big></U></span> and purchased into <span style=\"color:blue;font-weight:bold\"><U><big><#HUB DC#></big></U></span> for capacity and planning purposes. Demand can now be entered. If it should be different, please advise. Please refer to the SharePoint link below for item details.");
                sb.AppendLine("<br><br><span style=\"font-weight:bold\"><big>Procurement:</big></span>");
                sb.AppendLine("<ul>");
                sb.AppendLine("     <li>Please confirm the purchased price and create the vendor PIR to be set up in the Purchased Into Location indicated above.</li>");
                sb.AppendLine("     <li>Please send to HQ Costing Team to enter the Planned Price.</li>");
                sb.AppendLine("</ul>");
                sb.AppendLine("<br><br><span style=\"font-weight:bold\"><big>HQ Costing Team:</big></span>");
                sb.AppendLine("<ul>");
                sb.AppendLine("     <li>If FG or Transfer Semi and <span style=\"font-weight:bold\"><big>Confections/Fruit Snacks</big></span>, please cost in FPCO and all FP and FX DCs.</li>");
                sb.AppendLine("     <li>If FG or Transfer Semi and <span style=\"font-weight:bold\"><big>CCC</big></span>, please cost in FPCO and all SL DCs.</li>");
                sb.AppendLine("     <li>If FG or Transfer Semi and <span style=\"font-weight:bold\"><big>LBB</big></span>, please cost in FPCO and all FERQ DCs.</li>");
                sb.AppendLine("</ul>");
                sb.AppendLine("<br>----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#><br>");
                sb.AppendLine("SAP Item #: <#SAP FG##><br>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("HUB DC: <#HUB DC#><br>");
                sb.AppendLine("Deployment Mode: <#DEPLOYMENT MODE#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>SAP BOM Setup Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPBOMSetupConfirmationExternalTurnkeyExistingFG.ToString()))
            {
                sb.AppendLine("<br><br>Item <span style=\"color:blue;font-weight:bold\"><U><big><#SAP FG##></big></U></span> has been set-up to be purchased into <span style=\"color:blue;font-weight:bold\"><U><big><#HUB DC#></big></U></span>. This item is turnkey there is no SAP BOM. <span style=\"background-color:#FFFF00\">Item needs a purchased price/PIR.</span>");
                sb.AppendLine("<br><br><span style=\"font-weight:bold\"><big>Procurement:</big></span>");
                sb.AppendLine("<ul>");
                sb.AppendLine("     <li>Please confirm the purchased price and create the vendor PIR to be set up in the Purchased Into Location indicated above.</li>");
                sb.AppendLine("     <li>Please send to HQ Costing Team to enter the Planned Price.</li>");
                sb.AppendLine("</ul>");
                sb.AppendLine("<br><br><span style=\"font-weight:bold\"><big>HQ Costing Team:</big></span>");
                sb.AppendLine("<ul>");
                sb.AppendLine("     <li>If FG or Transfer Semi and <span style=\"font-weight:bold\"><big>Confections/Fruit Snacks</big></span>, please cost in FPCO and all FP and FX DCs.</li>");
                sb.AppendLine("     <li>If FG or Transfer Semi and <span style=\"font-weight:bold\"><big>CCC</big></span>, please cost in FPCO and all SL DCs.</li>");
                sb.AppendLine("     <li>If FG or Transfer Semi and <span style=\"font-weight:bold\"><big>LBB</big></span>, please cost in FPCO and all FERQ DCs.</li>");
                sb.AppendLine("</ul>");
                sb.AppendLine("<br>----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#><br>");
                sb.AppendLine("SAP Item #: <#SAP FG##><br>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("HUB DC: <#HUB DC#><br>");
                sb.AppendLine("Deployment Mode: <#DEPLOYMENT MODE#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>SAP BOM Setup Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPBOMSetupConfirmationInternalFG.ToString()))
            {
                sb.AppendLine("<br><br>New item <span style=\"color:blue;font-weight:bold\"><U><big><#SAP FG##></big></U></span> has been setup in <span style=\"color:blue;font-weight:bold\"><U><big><#PACK LOCATION1#></big></U></span>.");
                sb.AppendLine("<br><span style=\"background-color:#FFFF00\"> Item needs final routing and cost.</span>");
                sb.AppendLine("<br><span style=\"font-weight:bold\"><big>Plants</big></span> - Please enter routings within 48 hours and reply to this email after final routings have been entered.");
                sb.AppendLine("<br><span style=\"font-weight:bold\"><big>Packaging Procurement</big></span> - Please confirm all new material costs for this FG have been sent to HQ Costing Team.");
                sb.AppendLine("<br><br><span style=\"font-weight:bold\"><big>HQ Costing Team:</big></span>");
                sb.AppendLine("<br><br>First cost in the Manufacturing Plant, then follow the path below:");
                sb.AppendLine("<ul>");
                sb.AppendLine("     <li>If <span style=\"font-weight:bold\"><big>Confections/Fruit Snacks</big></span>:</li>");
                sb.AppendLine("     <ul>");
                sb.AppendLine("             <li>Extend cost to the following Plant Codes: FPCO and all FX and FP DC's");
                sb.AppendLine("     </ul>");
                sb.AppendLine("     <li>If <span style=\"font-weight:bold\"><big>CCC</big></span>:</li> ");
                sb.AppendLine("     <ul>");
                sb.AppendLine("             <li>Extend cost to the following Plant Codes: FPCO and all SELL DC's");
                sb.AppendLine("     </ul>");
                sb.AppendLine("     <li>If <span style=\"font-weight:bold\"><big>LBB</big></span>:</li> ");
                sb.AppendLine("     <ul>");
                sb.AppendLine("             <li>Extend cost to the following Plant Codes: FPCO and all FERQ DC's");
                sb.AppendLine("     </ul>");
                sb.AppendLine("</ul>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#><br>");
                sb.AppendLine("SAP Item #: <#SAP FG##><br>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("Deployment Mode: <#DEPLOYMENT MODE#><br>");
                sb.AppendLine("HUB DC: <#HUB DC#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>SAP BOM Setup Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPBOMSetupConfirmationNetworkMove.ToString()))
            {
                sb.AppendLine("<br><br><U><B>Existing</B></U> item <span style=\"color:blue;font-weight:bold\"><U><big><#SAP FG##></big></U></span> below has been setup in <span style=\"color:blue;font-weight:bold\"><U><big><#PACK LOCATION1#></big></U></span> for capacity and planning purposes. If it should be different, please advise. Please refer to the SharePoint link below for item details.<br><br>");
                sb.AppendLine("<span style=\"background-color:#FFFF00\">Item needs final routing and cost.</span><br><br>");
                sb.AppendLine("<br><br><span style=\"font-weight:bold\"><big>Plants</big></span> - Please enter final routings within 48 hours and reply to this email after final routings have been entered.>");
                sb.AppendLine("<br><br><span style=\"font-weight:bold\"><big>Packaging Procurement</big></span> - Please confirm all new material costs for this FG have been sent to HQ Costing Team.</span>");
                sb.AppendLine("<br><br><span style=\"font-weight:bold\"><big>HQ Costing Team</big></span> - Please update Special Procurement Costing Keys in FPCO.</span>");
                sb.AppendLine("<ul>");
                sb.AppendLine("     <il>Do not change DC standards unless specific approval received from Director or VP of Supply Chain Finance.</il>");
                sb.AppendLine("</ul>");

                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#><br>");
                sb.AppendLine("SAP Item #: <#SAP FG##><br>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("Deployment Mode: <#DEPLOYMENT MODE#><br>");
                sb.AppendLine("HUB DC: <#HUB DC#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>SAP BOM Setup Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPBOMSetupConfirmationNetworkMoveTSs.ToString()))
            {
                sb.AppendLine("<br><br><span style=\"background-color: #FFFF00\">The below Transfer Semi(s) has been set up in the following pack location(s). Please update to final routings and cost.</span>");
                sb.AppendLine("<br><br><span style=\"font-weight:bold\"><big>Plants</big></span> - Please enter final routings within 48 hours and reply to this email after final routings have been entered.>");
                sb.AppendLine("<br><br><span style=\"font-weight:bold\"><big>HQ Costing Team</big></span> - Please update Special Procurement Costing Keys in FPCO.</span>");
                sb.AppendLine("<ul>");
                sb.AppendLine("     <il>Do not change DC standards unless specific approval received from Director or VP of Supply Chain Finance.</il>");
                sb.AppendLine("</ul>");
                sb.AppendLine("<#NETWORK MOVE TRANSFER SEMIS#><br><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#><br>");
                sb.AppendLine("SAP Item #: <#SAP FG##><br>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>SAP BOM Setup Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPBOMSetupConfirmationNewTSs.ToString()))
            {
                sb.AppendLine("<span style=\"background-color:#FFFF00\">The below Transfer Semi has been set up in the following pack location. Plant please update to final routings and cost. Please enter within 48 hours and reply to this email after final routings have been entered.</span><br><br>");

                sb.AppendLine("<br><br><span style=\"font-weight:bold\"><big>HQ Costing Team:</big></span>");
                sb.AppendLine("<ul>");
                sb.AppendLine("     <li>If <span style=\"font-weight:bold\"><big>Confections/Fruit Snacks</big></span>:</li>");
                sb.AppendLine("     <ul>");
                sb.AppendLine("             <li>Extend cost to the following DC's: FPCO and all FX and FP DC's");
                sb.AppendLine("     </ul>");
                sb.AppendLine("     <li>If <span style=\"font-weight:bold\"><big>CCC</big></span>:</li> ");
                sb.AppendLine("     <ul>");
                sb.AppendLine("             <li>Extend cost to the following DC's: FPCO and all SELL DC's");
                sb.AppendLine("     </ul>");
                sb.AppendLine("     <li>If <span style=\"font-weight:bold\"><big>LBB</big></span>:</li> ");
                sb.AppendLine("     <ul>");
                sb.AppendLine("             <li>Extend cost to the following DC's: FPCO and all FERQ DC's");
                sb.AppendLine("     </ul>");
                sb.AppendLine("</ul>");

                sb.AppendLine("<#NEW TS PACK LOCATIONS#><br><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>SAP BOM Setup Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPBOMSetupConfirmationSubConNetworkMove.ToString()))
            {
                sb.AppendLine("<br><br><U><B>Existing</B></U> SubCon item <span style=\"color:blue;font-weight:bold\"><U><big><#SAP FG##></big></U></span> has been set-up in the new pack location <span style=\"color:blue;font-weight:bold\"><U><big><#EXTERNAL FINISHED GOOD PACKER#></big></U></span> and to be purchased into <span style=\"color:blue;font-weight:bold\"><U><big><#HUB DC#></big></U></span>. <span style=\"background-color:#FFFF00\">Item needs purchased price/PIR.</span><br><br>");
                sb.AppendLine("<br><br><span style=\"font-weight:bold\"><big>Procurement:</big></span>");
                sb.AppendLine("<ul>");
                sb.AppendLine("     <li>Please confirm the purchased price and create the vendor PIR for the location above.</li>");
                sb.AppendLine("     <li>Send to HQ Costing Team the SUBK PIR for costing.</li>");
                sb.AppendLine("</ul>");
                sb.AppendLine("<br><br><span style=\"font-weight:bold\"><big>HQ Costing Team:</big></span>");
                sb.AppendLine("<br><br>First cost in the Subcontract Plant, then follow the path below:");
                sb.AppendLine("<ul>");
                sb.AppendLine("     <li>If FG or Transfer Semi and <span style=\"font-weight:bold\"><big>Confections/Fruit Snacks</big></span>, please extend costing to FPCO and all FP and FX DCs.</li>");
                sb.AppendLine("     <li>If FG or Transfer Semi and <span style=\"font-weight:bold\"><big>CCC</big></span>, please extend costing to FPCO and all SELL DCs.</li>");
                sb.AppendLine("     <li>If FG or Transfer Semi and <span style=\"font-weight:bold\"><big>LBB</big></span>, please extend to FPCO and all FERQ DCs.</li>");
                sb.AppendLine("</ul>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#><br>");
                sb.AppendLine("SAP Item #: <#SAP FG##><br>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("Deployment Mode: <#DEPLOYMENT MODE#><br>");
                sb.AppendLine("HUB DC: <#HUB DC#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>SAP BOM Setup Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPBOMSetupConfirmationTurnkeyNetworkMove.ToString()))
            {
                sb.AppendLine("<br><br><U><B>Existing</B></U> item <span style=\"color:blue;font-weight:bold\"><U><big><#SAP FG##></big></U></span> has been set-up in the new pack location <span style=\"color:blue;font-weight:bold\"><U><big><#EXTERNAL FINISHED GOOD PACKER#></big></U></span> and to be purchased into <span style=\"color:blue;font-weight:bold\"><U><big><#HUB DC#></big></U></span>. This item is turnkey there is no SAP BOM. <span style=\"background-color:#FFFF00\">Item needs a purchased price/PIR.</span>");

                sb.AppendLine("<br><br><span style=\"font-weight:bold\"><big>Procurement:</big></span>");
                sb.AppendLine("<ul>");
                sb.AppendLine("     <li>Please confirm the purchased price and create the vendor PIR to be set up in the Purchased Into Location indicated above.</li>");
                sb.AppendLine("     <li>Please send to HQ Costing Team to enter the Planned Price.</li>");
                sb.AppendLine("</ul>");
                sb.AppendLine("<br><br><span style=\"font-weight:bold\"><big>HQ Costing Team:</big></span>");
                sb.AppendLine("<ul>");
                sb.AppendLine("     <li>If FG or Transfer Semi and <span style=\"font-weight:bold\"><big>Confections/Fruit Snacks</big></span>, please cost in FPCO and all FP and FX DCs.</li>");
                sb.AppendLine("     <li>If FG or Transfer Semi and <span style=\"font-weight:bold\"><big>CCC</big></span>, please cost in FPCO and all SL DCs.</li>");
                sb.AppendLine("     <li>If FG or Transfer Semi and <span style=\"font-weight:bold\"><big>LBB</big></span>, please cost in FPCO and all FERQ DCs.</li>");
                sb.AppendLine("</ul>");
                sb.AppendLine("<br><br>----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#><br>");
                sb.AppendLine("SAP Item #: <#SAP FG##><br>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("Deployment Mode: <#DEPLOYMENT MODE#><br>");
                sb.AppendLine("HUB DC: <#HUB DC#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>SAP BOM Setup Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPCOSTINGDETAILS_NOTIFICATION.ToString()))
            {
                sb.AppendLine("Costing Details to SAP");
                sb.AppendLine("<br>");
                sb.AppendLine("You are receiving this email as a notification of this upcoming project.<br>");
                sb.AppendLine("If you feel you have received this email in error, please contact the PM.<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("PM: <#OBM#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("New Materials:<br>");
                sb.AppendLine("<#COSTING DETAILS#>");
                sb.AppendLine("<br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPInitialItemSetupConfirmationNewFG.ToString()))
            {
                sb.AppendLine("The <U><B>TBD</B></U> item <span style=\"color:blue;font-weight:bold\"><U><big><#SAP FG##></big></U></span> below has been setup in <span style=\"color:blue;font-weight:bold\"><U><big><#PACK LOCATION1#> (Packaging Plant)</big></U></span> for capacity and planning purposes. Planning Team, please note that safety time was entered at 28 days. If it should be different, please advise. Please refer to the SharePoint link below for item details.<br><br>");
                sb.AppendLine("<span style=\"background-color:#FFFF00\">Plant please enter initial routing and production version. Please enter within 48 hours and reply to this e-mail after routings and production versions have been entered.</span><br><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("Deployment Mode: <#DEPLOYMENT MODE#><br>");
                sb.AppendLine("HUB DC: <#HUB DC#><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPInitialItemSetupConfirmationNewFGExternalTurnkeyFG.ToString()))
            {
                sb.AppendLine("Turnkey <U><B>TBD</B></U> Item <span style=\"color:blue;font-weight:bold\"><U><big><#SAP FG##></big></U></span> has been set-up in the pack location <span style=\"color:blue;font-weight:bold\"><U><big><#EXTERNAL PACKER#></big></U></span> and purchased into <span style=\"color:blue;font-weight:bold\"><U><big><#HUB DC#></big></U></span> for capacity and planning purposes. If it should be different, please advise. Please refer to the SharePoint link below for item details.<br><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("Deployment Mode: <#DEPLOYMENT MODE#><br>");
                sb.AppendLine("HUB DC: <#HUB DC#><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPInitialItemSetupConfirmationNewFGExternalSubconFG.ToString()))
            {
                sb.AppendLine("SubCon <U><B>TBD</B></U> Item <span style=\"color:blue;font-weight:bold\"><U><big><#SAP FG##></big></U></span> has been set-up in the pack location <span style=\"color:blue;font-weight:bold\"><U><big><#EXTERNAL PACKER#></big></U></span> and purchased into <span style=\"color:blue;font-weight:bold\"><U><big><#HUB DC#></big></U></span> for capacity and planning purposes. Production Versions have been entered. If it should be different, please advise. Please refer to the SharePoint link below for item details.<br><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("Deployment Mode: <#DEPLOYMENT MODE#><br>");
                sb.AppendLine("HUB DC: <#HUB DC#><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPInitialItemSetupConfirmationNewTSs.ToString()))
            {
                sb.AppendLine("The below <U><B>TBD</B></U> Transfer Semi has been setup in manufacturing plants for capacity and planning purposes. Please refer to the SharePoint link below for item details.<br><br>");
                sb.AppendLine("<span style=\"background-color:#FFFF00\">Plant please enter initial routings and production versions. Please enter within 48 hours and reply to this e-mail after routings and production versions have been entered.</span><br><br>");
                sb.AppendLine("<#NEW TS PACK LOCATIONS#><br><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("Deployment Mode: <#DEPLOYMENT MODE#><br>");
                sb.AppendLine("HUB DC: <#HUB DC#><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPInitialItemSetupConfirmationNetworkMoveTSs.ToString()))
            {
                sb.AppendLine("<br><br>The below <span style=\"color:#00e6e6;font-weight:bold\"><U>Existing</U></span> Transfer Semi has been setup in manufacturing plants for capacity and planning purposes. Please refer to the SharePoint link below for item details.<br><br>");
                sb.AppendLine("<span style=\"background-color:#FFFF00\">Plant please enter initial routings and production versions. Please enter within 48 hours and reply to this e-mail after routings and production versions have been entered.</span><br><br>");
                sb.AppendLine("<br><br>");
                sb.AppendLine("<#NETWORK MOVE TRANSFER SEMIS#><br><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#><br>");
                sb.AppendLine("SAP Item #: <#SAP FG##><br>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("Deployment Mode: <#DEPLOYMENT MODE#><br>");
                sb.AppendLine("HUB DC: <#HUB DC#><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPInitialItemSetupConfirmationExstNetworkMove.ToString()))
            {
                sb.AppendLine("The <span style=\"color:#00e6e6;font-weight:bold\"><U>Existing</U></span> item <span style=\"color:blue;font-weight:bold\"><U><big><#SAP FG##></big></U></span> below has been setup in <span style=\"color:blue;font-weight:bold\"><U><big><#PACK LOCATION1#></big></U></span> for capacity and planning purposes. Planning Team, please note that safety time was entered at 28 days. If it should be different, please advise. Please refer to the SharePoint link below for item details.<br><br>");
                sb.AppendLine("<span style=\"background-color:#FFFF00\">Plant please enter initial routing and production version. Please enter within 48 hours and reply to this e-mail after routings and production versions have been entered.</span><br><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("Deployment Mode: <#DEPLOYMENT MODE#><br>");
                sb.AppendLine("HUB DC: <#HUB DC#><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPInitialItemSetupConfirmationExstNetworkMoveTurnkey.ToString()))
            {
                sb.AppendLine("Turnkey <span style=\"color:#00e6e6;font-weight:bold\"><U>Existing</U></span> item <span style=\"color:blue;font-weight:bold\"><U><big><#SAP FG##></big></U></span> below has been setup in the pack location <span style=\"color:blue;font-weight:bold\"><U><big><#EXTERNAL FINISHED GOOD PACKER#></big></U></span> and purchased into <span style=\"color:blue;font-weight:bold\"><U><big><#HUB DC#></big></U></span> for capacity and planning purposes. If it should be different, please advise. Please refer to the SharePoint link below for item details.<br><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("Deployment Mode: <#DEPLOYMENT MODE#><br>");
                sb.AppendLine("HUB DC: <#HUB DC#><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPInitialItemSetupConfirmationExstNetworkMoveSubCon.ToString()))
            {
                sb.AppendLine("SubCon <span style=\"color:#00e6e6;font-weight:bold\"><U>Existing</U></span> item <span style=\"color:blue;font-weight:bold\"><U><big><#SAP FG##></big></U></span> below has been setup in the pack location <span style=\"color:blue;font-weight:bold\"><U><big><#EXTERNAL FINISHED GOOD PACKER#></big></U></span> and purchased into <span style=\"color:blue;font-weight:bold\"><U><big><#HUB DC#></big></U></span> for capacity and planning purposes. Production Versions have been entered. If it should be different, please advise. Please refer to the SharePoint link below for item details.<br><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("Deployment Mode: <#DEPLOYMENT MODE#><br>");
                sb.AppendLine("HUB DC: <#HUB DC#><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPInitialItemSetupConfirmationNewFGTurnkeyFC01.ToString()))
            {
                sb.AppendLine("Turnkey Item <span style=\"color:blue;font-weight:bold\"><U><big><#SAP FG##></big></U></span> has been setup in the pack location <span style=\"color:blue;font-weight:bold\"><U><big><#EXTERNAL FINISHED GOOD PACKER#></big></U></span> and purchased into <span style=\"color:blue;font-weight:bold\"><U><big><#HUB DC#></big></U></span> for capacity and planning purposes. If it should be different, please advise. Please refer to the SharePoint link below for item details.<br>");
                sb.AppendLine("<br>");
                sb.AppendLine("<span style=\"background-color:#FFFF00;font-weight:bold\">Supply Planning:</span><br><br>");
                sb.AppendLine("<span style=\"background-color:#FFFF00\">•	Please enter final routing and production versions within 48 hours and reply all  when this task is complete.</span><br><br>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("Deployment Mode: <#DEPLOYMENT MODE#><br>");
                sb.AppendLine("HUB DC: <#HUB DC#><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPInitialItemSetupConfirmationNewPCFC01.ToString()))
            {
                sb.AppendLine("<br><br>The below <U><B>TBD</B></U> Purchased Semi has been setup in manufacturing plants for capacity and planning purposes. Please refer to the SharePoint link below for item details.<br><br>");
                sb.AppendLine("<span style=\"background-color:#FFFF00;font-weight:bold\">Supply Planning:</span><br><br>");
                sb.AppendLine("<span style=\"background-color:#FFFF00\">•	Please enter final routings and production versions within 48 hours and reply all  when this task is complete.</span><br><br>");
                sb.AppendLine("<#NEW PURCANDY SEMIS PACK LOCATIONS#><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#><br>");
                sb.AppendLine("SAP Item #: <#SAP FG##><br>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>SAP BOM Setup Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPInitialItemSetupConfirmationNMPCFC01.ToString()))
            {
                sb.AppendLine("<br><br>The below <U><B>Existing</B></U> Purchased Semi has been setup in manufacturing plants for capacity and planning purposes. Please refer to the SharePoint link below for item details.<br><br>");
                sb.AppendLine("<span style=\"background-color:#FFFF00;font-weight:bold\">Supply Planning:</span><br><br>");
                sb.AppendLine("<span style=\"background-color:#FFFF00\">•	Please enter final routings and production versions within 48 hours and reply all  when this task is complete.</span><br><br>");
                sb.AppendLine("<#NM PURCANDY SEMIS PACK LOCATIONS#><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#><br>");
                sb.AppendLine("SAP Item #: <#SAP FG##><br>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>SAP BOM Setup Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPInitialItemSetupConfirmationExistingFGNMFC01.ToString()))
            {
                sb.AppendLine("<U>Existing</U> item <span style=\"color:blue;font-weight:bold\"><U><big><#SAP FG##></big></U></span> has been setup in the new pack location <span style=\"color:blue;font-weight:bold\"><U><big><#EXTERNAL FINISHED GOOD PACKER#></big></U></span> and to be purchased into <span style=\"color:blue;font-weight:bold\"><U><big><#HUB DC#></big></U></span>. This item is turnkey.<br>");
                sb.AppendLine("<br>");
                sb.AppendLine("<span style=\"background-color:#FFFF00;font-weight:bold\">Supply Planning:</span><br><br>");
                sb.AppendLine("<span style=\"background-color:#FFFF00\">•	Please enter final routings and production versions within 48 hours and reply all  when this task is complete.</span><br><br>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("Deployment Mode: <#DEPLOYMENT MODE#><br>");
                sb.AppendLine("HUB DC: <#HUB DC#><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPInitialSetup.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for Initial Item Setup<br><br>");
                sb.AppendLine("<B>* Master Data *</B><br>");
                sb.AppendLine("The appropriate PMT tasks have been completed in order to proceed with the Initial Item Setup for this project.");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>SAP Initial Item Setup Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPROUTINGSETUP_NOTIFICATION.ToString()))
            {
                sb.AppendLine("SAP Routing Setup");
                sb.AppendLine("<br>");
                sb.AppendLine("You are receiving this email as a notification of this upcoming project.<br>");
                sb.AppendLine("If you feel you have received this email in error, please contact the PM.<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("PM: <#OBM#>");
                sb.AppendLine("Make Location: <#MAKE LOCATION#>");
                sb.AppendLine("1st Pack Location: <#PACK LOCATION1#>");
                sb.AppendLine("Pack Spec: <#NEEDS INPUT#>");
                sb.AppendLine("<br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPWAREHOUSEINFO_NOTIFICATION.ToString()))
            {
                sb.AppendLine("Warehouse Information into SAP");
                sb.AppendLine("<br>");
                sb.AppendLine("You are receiving this email as a notification of this upcoming project.<br>");
                sb.AppendLine("If you feel you have received this email in error, please contact the PM.<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("PM: <#OBM#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("New Materials:<br>");
                sb.AppendLine("<#WAREHOUSE DETAILS#>");
                sb.AppendLine("<br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SrOBMApproval.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for PM Initial Review<br><br>");
                sb.AppendLine("<B>* PM Initial Approver *</B><br>");
                sb.AppendLine("A new PMT Item Proposal Form has been submitted.  Please complete the Initial Review for approval or rejection.");
                sb.AppendLine("<br><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#><br>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>PM Initial Review Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click ‘submit’. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.STANDARDCOSTENTRY_NOTIFICATION.ToString()))
            {
                sb.AppendLine("Standard Cost Entry");
                sb.AppendLine("<br>");
                sb.AppendLine("You are receiving this email as a notification of this upcoming project.<br>");
                sb.AppendLine("If you feel you have received this email in error, please contact the PM.<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#><br>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#><br>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br><br>");
                sb.AppendLine("New Materials:<br>");
                sb.AppendLine("<#STDCOST DETAILS#>");
                sb.AppendLine("<br><br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.TradePromo.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for Trade Input and Apply ZEST Pricing<br><br>");
                sb.AppendLine("<B>* Trade Input and Apply ZEST Pricing *<br></B>");
                sb.AppendLine("You are receiving this email as a notification of this upcoming project. Please enter in Trade Input and Apply ZEST Pricing for this item at this time.");
                sb.AppendLine("If you feel you have received this email in error, please contact the PM.<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("PM: <#OBM#>");
                //sb.AppendLine("List Price B1 per Selling Unit: <#TRUCKLOAD PRICE UNIT#>");
                sb.AppendLine("List Price B1 per Selling Unit: <#TRUCKLOAD PRICE CASE#>");
                //sb.AppendLine("Truckload Price per Case: <#TRUCKLOAD PRICE CASE#>");
                sb.AppendLine("Retail Selling unit per base UOM: <#RETAIL SELLING UNIT PER BASE UOM#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("<br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>Trade Promo Group Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.EstBracketPricing.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for Bracket Pricing Input<br><br>");
                sb.AppendLine("<B>* Bracket Pricing Input *<br></B>");
                sb.AppendLine("You are receiving this email as a notification of this upcoming project. Please enter in Trade Input and Apply ZEST Pricing for this item at this time.");
                sb.AppendLine("If you feel you have received this email in error, please contact the PM.<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("PM: <#OBM#>");
                sb.AppendLine("List Price B1 per Selling Unit: <#TRUCKLOAD PRICE CASE#>");
                //sb.AppendLine("Truckload Price per Case: <#TRUCKLOAD PRICE CASE#>");
                sb.AppendLine("Retail Selling unit per base UOM: <#RETAIL SELLING UNIT PER BASE UOM#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("<br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>Estimated Bracket Pricing Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.EstPricing.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for Pricing Input Input<br><br>");
                sb.AppendLine("<B>* Pricing Input *<br></B>");
                sb.AppendLine("You are receiving this email as a notification of this upcoming project. Please enter in Trade Input and Apply ZEST Pricing for this item at this time.");
                sb.AppendLine("If you feel you have received this email in error, please contact the PM.<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("PM: <#OBM#>");
                sb.AppendLine("List Price B1 per Selling Unit: <#TRUCKLOAD PRICE CASE#>");
                //sb.AppendLine("Truckload Price per Case: <#TRUCKLOAD PRICE CASE#>");
                sb.AppendLine("Retail Selling unit per base UOM: <#RETAIL SELLING UNIT PER BASE UOM#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("<br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>Est Pricing Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.WorldSyncReqImageRequested.ToString()))
            {
                sb.AppendLine("<br><br>");
                sb.AppendLine("A new worldsync image has been requested for SAP number <#SAP FG##> and description <#SAP DESCRIPTION#>.<br>");
                sb.AppendLine("<a href=<#FormLink#>>Upload new image</a><br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.WorldSyncReqNutritionalRequested.ToString()))
            {
                sb.AppendLine("A new worldsync nutritional has been requested for SAP number <#SAP FG##> and description <#SAP DESCRIPTION#>.<br>");
                sb.AppendLine("<a href=<#FormLink#>>Upload new nutritionale</a><br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.ParentProjectSubmitted.ToString()))
            {
                sb.AppendLine("<br>");
                sb.AppendLine("Please be advised that a Parent Project, Project <#PARENTPROJECTNUMBER#> has been submitted. If you are receiving this notification, then you have been assigned as a role in the project team.");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#PARENTPROJECTNUMBER#>");
                sb.AppendLine("Project Manager: <#PARENTPROJECTMANAGER#>");
                sb.AppendLine("Project Leader: <#PARENTPROJECTLEADER#>");
                sb.AppendLine("Parent Description: <#PARENTPROJECTNAME#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#>");
                sb.AppendLine("Material Group 1(Brand): <#Material Group 1#>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#>");
                sb.AppendLine("<br>");
                sb.AppendLine("PMT Links:");
                sb.AppendLine("<a href=<#ProjectInformationFormLink#>>Project Information Form</a>");
                sb.AppendLine("<a href=<#ProjectSummaryFormLink#>>Project Summary Form</a>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.ParentProjectCompleted.ToString()))
            {
                sb.AppendLine("<br>");
                sb.AppendLine("Please be advised that a Parent Project, Project <#PARENTPROJECTNUMBER#> has been completed. If you are receiving this notification and believe this is a mistake please contact the Project Manager indicated below.");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#PARENTPROJECTNUMBER#>");
                sb.AppendLine("Project Manager: <#PARENTPROJECTMANAGER#>");
                sb.AppendLine("Project Leader: <#PARENTPROJECTLEADER#>");
                sb.AppendLine("Parent Description: <#PARENTPROJECTNAME#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#>");
                sb.AppendLine("Material Group 1(Brand): <#Material Group 1#>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#>");
                sb.AppendLine("<br>");
                sb.AppendLine("PMT Links:");
                sb.AppendLine("<a href=<#ProjectInformationFormLink#>>Project Information Form</a>");
                sb.AppendLine("<a href=<#ProjectSummaryFormLink#>>Project Summary Form</a>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.ParentProjectCancelled.ToString()))
            {
                sb.AppendLine("<br>");
                sb.AppendLine("Please be advised that a Parent Project, Project <#PARENTPROJECTNUMBER#> has been cancelled. If you are receiving this notification and believe this is a mistake please contact the Project Manager indicated below.");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#PARENTPROJECTNUMBER#>");
                sb.AppendLine("Project Manager: <#PARENTPROJECTMANAGER#>");
                sb.AppendLine("Project Leader: <#PARENTPROJECTLEADER#>");
                sb.AppendLine("Parent Description: <#PARENTPROJECTNAME#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#>");
                sb.AppendLine("Material Group 1(Brand): <#Material Group 1#>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#>");
                sb.AppendLine("<br>");
                sb.AppendLine("PMT Links:");
                sb.AppendLine("<a href=<#ProjectInformationFormLink#>>Project Information Form</a>");
                sb.AppendLine("<a href=<#ProjectSummaryFormLink#>>Project Summary Form</a>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.BOMSetupMaterialWarehouse.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for Material Warehouse Setup Completion<br><br>");
                sb.AppendLine("<B>* Material Warehouse Setup *<br></B>");
                sb.AppendLine("Please indicate the “Purchase-into” location for all new materials in the BOM. <br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("Make Location: <#MAKE LOCATION#>");
                sb.AppendLine("Pack Location: <#PACK LOCATION1#>");
                sb.AppendLine("HUB DC: <#HUB DC#><br>");
                sb.AppendLine("<br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>Material Warehouse Setup Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.BOMSetupPE3.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for Material Confirmation (PE3)<br><br>");
                sb.AppendLine("<B>* Packaging Engineer *<br></B>");
                sb.AppendLine("Please input all needed finished good, transfer semi, and purchased candy semi specification information. <br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("<br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>Bill of Materials (PE3) Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.BOMSetupPE3Submitted.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Material Confirmation (PE3) Submitted<br><br>");
                sb.AppendLine("<B>* Packaging Engineer *<br></B>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("<br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>Bill of Materials (PE3) Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.SAPCompleteItemSetup.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: Request for SAP Complete Item Setup (Master Data)<br><br>");
                sb.AppendLine("<B>* Master Data *<br></B>");
                sb.AppendLine("Please complete item setup in SAP.<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br>");
                sb.AppendLine("Project #: <#ProjectNo#>");
                sb.AppendLine("SAP Item #: <#SAP FG##>");
                sb.AppendLine("SAP Description: <#SAP DESCRIPTION#>");
                sb.AppendLine("Product Hierarchy Level 1: <#PROD HIER LVL1#>");
                sb.AppendLine("Product Hierarchy Level 2: <#PROD HIER LVL2#>");
                sb.AppendLine("Material Group 1 (Brand): <#BRAND#><br>");
                sb.AppendLine("Project Type: <#PRJECTTYPE#><br>");
                sb.AppendLine("<#PRJECTTYPESUBCATEGORY#><br>");
                sb.AppendLine("<br>");
                sb.AppendLine("PMT Links:<br>");
                sb.AppendLine("<a href=<#FormLink#>>SAP Complete Item Setup Form</a><br>");
                sb.AppendLine("<a href=<#CommFormLink#>>Commercialization Form</a><br>");
                sb.AppendLine("<br>");
                sb.AppendLine("----------------------------------------------------------------------------------------------------<br><br>");
                sb.AppendLine("Once completed, please click 'submit'. Thank you.");
            }
            else if (string.Equals(emailTemplateKey, EmailTemplateKey.StageGatePostLaunchNotification.ToString()))
            {
                sb.AppendLine("ACTION REQUIRED: To initiate Post Launch preparation<br><br>");
                sb.AppendLine("<B>The POST LAUNCH <#NOTIFICATIONTYPECODE#> for the Parent Project <#PARENTPROJECTNUMBER#> has been initiated.<br></B>");
                sb.AppendLine("<B>Please start to prepare data for reporting on your assigned topic described below<br><br><br></B>");
                sb.AppendLine("<#CHILDPROJECTTABLE#>");
                sb.AppendLine("<br><br>");
                sb.AppendLine("    <table style='border:1px solid black; width:80%;'>");
                sb.AppendLine("        <tr>");
                sb.AppendLine("            <td></td>");
                sb.AppendLine("            <td style=\"background-color:#00008b;color:white;border:1px solid black; \">Data Owner</td>");
                sb.AppendLine("            <td style=\"background-color:#00008b;color:white;border:1px solid black; \">Reporting</td>");
                sb.AppendLine("        </tr>");
                sb.AppendLine("        <tr>");
                sb.AppendLine("            <td></td>");
                sb.AppendLine("            <td style='border:1px solid black;'>Customer Relations</td>");
                sb.AppendLine("            <td style='border:1px solid black;'>");
                sb.AppendLine("                    <li>Complaint data (trends, and top drivers of complaints)</li>");
                sb.AppendLine("                    <li>Praise data</li>");
                sb.AppendLine("                    <li>Other consumer data requests, if any</li>");
                sb.AppendLine("            </td>");
                sb.AppendLine("        </tr>");
                sb.AppendLine("        <tr>");
                sb.AppendLine("            <td><B>Post Launch Data</B></td>");
                sb.AppendLine("            <td  style='border:1px solid black;'>R&D</td>");
                sb.AppendLine("            <td  style='border:1px solid black;'>");
                sb.AppendLine("                    <li>Formula updates, if any</li>");
                sb.AppendLine("                    <li>Shelf Life update</li>");
                sb.AppendLine("            </td>");
                sb.AppendLine("        </tr>");
                sb.AppendLine("        <tr>");
                sb.AppendLine("            <td></td>");
                sb.AppendLine("            <td  style='border:1px solid black;'>Innovation Quality</td>");
                sb.AppendLine("            <td  style='border:1px solid black;'>");
                sb.AppendLine("                As proxy provide :");
                sb.AppendLine("                    <li>Quality updates: post launch production quality issues on materials and/or semis-wips or finished goodsy</li>");
                sb.AppendLine("                    <li>Updates, if any on change management.</li>");
                sb.AppendLine("                    <li>Review output in comparison to established PES criteria</li>");
                sb.AppendLine("            </td>");
                sb.AppendLine("        </tr>");
                sb.AppendLine("        <tr>");
                sb.AppendLine("            <td></td>");
                sb.AppendLine("            <td  style='border:1px solid black;'>Marketing</td>");
                sb.AppendLine("            <td  style='border:1px solid black;'>");
                sb.AppendLine("                <li>Business updates</li>");
                sb.AppendLine("                <li>Any monitoring desired from FAAR meeting.</li>");
                sb.AppendLine("            </td>");
                sb.AppendLine("        </tr>");
                sb.AppendLine("    </table>");
            }
            return sb.ToString();
        }

        #endregion
        #endregion

        #region Compass Task Assignment List
        private void CreateCompassTaskAssignmentList()
        {
            if (currentWeb != null)
            {
                SPList splist = currentWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTaskAssignmentListName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(currentWeb, GlobalConstants.LIST_CompassTaskAssignmentListName, GlobalConstants.LIST_CompassTaskAssignmentListName);
                    var field = splist.Fields[SPBuiltInFieldId.Title];
                    field.Title = "WorkflowStep";
                    field.Update();
                }

                bool needsListUpdate = false;

                bool fieldExist = splist.Fields.ContainsFieldWithStaticName(TaskAssignmentFieldName.TaskGroups);
                if (!fieldExist)
                {
                    splist.Fields.Add(TaskAssignmentFieldName.TaskGroups, SPFieldType.Text, true);
                    SPView view = splist.DefaultView;
                    view.ViewFields.Add(TaskAssignmentFieldName.TaskGroups);
                    view.Update();
                    needsListUpdate = true;
                }

                fieldExist = splist.Fields.ContainsFieldWithStaticName(TaskAssignmentFieldName.CompassPageName);
                if (!fieldExist)
                {
                    splist.Fields.Add(TaskAssignmentFieldName.CompassPageName, SPFieldType.Text, true);
                    SPView view = splist.DefaultView;
                    view.ViewFields.Add(TaskAssignmentFieldName.CompassPageName);
                    view.Update();
                    needsListUpdate = true;
                }

                fieldExist = splist.Fields.ContainsFieldWithStaticName(TaskAssignmentFieldName.EmailGroups);
                if (!fieldExist)
                {
                    splist.Fields.Add(TaskAssignmentFieldName.EmailGroups, SPFieldType.Text, true);
                    SPView view = splist.DefaultView;
                    view.ViewFields.Add(TaskAssignmentFieldName.EmailGroups);
                    view.Update();
                    needsListUpdate = true;
                }

                fieldExist = splist.Fields.ContainsFieldWithStaticName(TaskAssignmentFieldName.TaskTitle);
                if (!fieldExist)
                {
                    splist.Fields.Add(TaskAssignmentFieldName.TaskTitle, SPFieldType.Text, true);
                    SPView view = splist.DefaultView;
                    view.ViewFields.Add(TaskAssignmentFieldName.TaskTitle);
                    view.Update();
                    needsListUpdate = true;
                }

                fieldExist = splist.Fields.ContainsFieldWithStaticName(TaskAssignmentFieldName.TaskDescription);
                if (!fieldExist)
                {
                    splist.Fields.Add(TaskAssignmentFieldName.TaskDescription, SPFieldType.Text, true);
                    SPView view = splist.DefaultView;
                    view.ViewFields.Add(TaskAssignmentFieldName.TaskDescription);
                    view.Update();
                    needsListUpdate = true;
                }
                if (needsListUpdate)
                    splist.Update();
            }
        }
        private void AttachWFListEventReceiver()
        {
            SPList wfList = currentWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTaskAssignmentListName);
            if (wfList != null)
            {
                DeleteEventReceiver(wfList);

                wfList.EventReceivers.Add(SPEventReceiverType.ItemUpdated,
                    GlobalConstants.SharePointServiceAssemblyName,
                    GlobalConstants.WorkflowStepListEventReceiverClassName);

                wfList.EventReceivers.Add(SPEventReceiverType.ItemAdded,
                    GlobalConstants.SharePointServiceAssemblyName,
                    GlobalConstants.WorkflowStepListEventReceiverClassName);

                wfList.EventReceivers.Add(SPEventReceiverType.ItemDeleting,
                    GlobalConstants.SharePointServiceAssemblyName,
                    GlobalConstants.WorkflowStepListEventReceiverClassName);

                wfList.Update();
            }
        }
        private void PopulatingWFListData()
        {
            var list = currentWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTaskAssignmentListName);
            if (list == null) return;

            // Workflow Steps
            AddTaskAssignmentItemToList(list, WorkflowStep.IPF, "Initial Proposal Pending", "Initial Proposal Pending", GlobalConstants.GROUP_Marketing, GlobalConstants.GROUP_Marketing, "ItemProposal");
            AddTaskAssignmentItemToList(list, WorkflowStep.IPFR, "Request for Initial Proposal Review", "Initial Proposal Review", GlobalConstants.GROUP_Marketing, GlobalConstants.GROUP_Marketing, "ItemProposal");
            AddTaskAssignmentItemToList(list, WorkflowStep.SrOBMApproval, "Request for PM Initial Approval", "Request for PM Initial Approval", string.Concat(GlobalConstants.GROUP_IndividualPM, ";", GlobalConstants.GROUP_IndividualSeniorPM), string.Concat(GlobalConstants.GROUP_IndividualPM, ";", GlobalConstants.GROUP_IndividualSeniorPM), "InitialApprovalReview");
            AddTaskAssignmentItemToList(list, WorkflowStep.Distribution, "Request for Distribution Center", "Request for Distribution", GlobalConstants.GROUP_Distribution, GlobalConstants.GROUP_Distribution, "Distribution");
            AddTaskAssignmentItemToList(list, WorkflowStep.TradePromo, "Request for Trade Promo Group", "Request for Trade Promo Group", GlobalConstants.GROUP_TradePromo, GlobalConstants.GROUP_TradePromo, "TradePromoGroup");
            AddTaskAssignmentItemToList(list, WorkflowStep.EstBracketPricing, "Request for Initial Bracket Pricing", "Request for Initial Bracket Pricing", GlobalConstants.GROUP_EstimatedBracketPricing, GlobalConstants.GROUP_EstimatedBracketPricing, "EstBracketPricing");
            AddTaskAssignmentItemToList(list, WorkflowStep.EstPricing, "Request for Initial Pricing", "Request for Initial Pricing", GlobalConstants.GROUP_EstimatedPricing, GlobalConstants.GROUP_EstimatedPricing, "EstPricing");
            AddTaskAssignmentItemToList(list, WorkflowStep.Operations, "Request for Operations and Initial Capacity", "Request for Operations and Initial Capacity", GlobalConstants.GROUP_Operations, GlobalConstants.GROUP_Operations, "Ops");

            AddTaskAssignmentItemToList(list, WorkflowStep.ExternalMfg, "Request for Extermal Manufacturing", "Request for Extermal Manufacturing", GlobalConstants.GROUP_ExternalManufacturing, GlobalConstants.GROUP_ExternalManufacturing, "ExternalMfg");
            AddTaskAssignmentItemToList(list, WorkflowStep.PrelimSAPInitialSetup, "Request for Preliminary SAP Initial Item Setup", "Request for Preliminary SAP Initial Item Setup", string.Concat("helpdesk@ferrarausa.com", "; ", GlobalConstants.GROUP_IndividualPM, ";", GlobalConstants.GROUP_IndividualInitiator), GlobalConstants.GROUP_MasterData, "PrelimSAPInitialItemSetup");
            AddTaskAssignmentItemToList(list, WorkflowStep.PrelimSAPInitialSetupCompleted, "Preliminary Initial Item Setup Completed", "Preliminary Initial Item Setup Completed", string.Concat(GlobalConstants.GROUP_IndividualInitiator), GlobalConstants.GROUP_IndividualInitiator, "PrelimSAPInitialItemSetup");
            AddTaskAssignmentItemToList(list, WorkflowStep.SAPInitialSetup, "Request for SAP Initial Item Setup", "Request for SAP Initial Item Setup", GlobalConstants.GROUP_MasterData, GlobalConstants.GROUP_MasterData, "SAPInitialItemSetup");
            AddTaskAssignmentItemToList(list, WorkflowStep.SAPInitialItemSetupConfirmationNewFG, "TBD Item Setup", "TBD Item Setup", string.Concat("Materials@ferrarausa.com", ";", GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), string.Concat(GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), "SAPInitialItemSetup");
            AddTaskAssignmentItemToList(list, WorkflowStep.SAPInitialItemSetupConfirmationNewFGExternalTurnkeyFG, "TBD Item Setup", "TBD Item Setup", string.Concat("Materials@ferrarausa.com", ";", GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), string.Concat(GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), "SAPInitialItemSetup");
            AddTaskAssignmentItemToList(list, WorkflowStep.SAPInitialItemSetupConfirmationNewFGExternalSubconFG, "TBD Item Setup", "TBD Item Setup", string.Concat("Materials@ferrarausa.com", ";", GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), string.Concat(GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), "SAPInitialItemSetup");
            AddTaskAssignmentItemToList(list, WorkflowStep.SAPInitialItemSetupConfirmationNewTSs, "TBD Initial Transfer Semi Setup", "TBD Initial Transfer Semi Setup", string.Concat("Materials@ferrarausa.com", ";", GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), string.Concat(GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), "SAPInitialItemSetup");
            AddTaskAssignmentItemToList(list, WorkflowStep.SAPInitialItemSetupConfirmationNetworkMoveTSs, "TBD Initial Transfer Semi Setup", "TBD Initial Transfer Semi Setup", string.Concat("Materials@ferrarausa.com", ";", GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), string.Concat(GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), "SAPInitialItemSetup");
            AddTaskAssignmentItemToList(list, WorkflowStep.SAPInitialItemSetupConfirmationExstNetworkMove, "TBD Initial Transfer Semi Setup", "TBD Initial Transfer Semi Setup", string.Concat("Materials@ferrarausa.com", ";", GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), string.Concat(GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), "SAPInitialItemSetup");
            AddTaskAssignmentItemToList(list, WorkflowStep.SAPInitialItemSetupConfirmationExstNetworkMoveTurnkey, "TBD Initial Transfer Semi Setup", "TBD Initial Transfer Semi Setup", string.Concat("Materials@ferrarausa.com", ";", GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), string.Concat(GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), "SAPInitialItemSetup");
            AddTaskAssignmentItemToList(list, WorkflowStep.SAPInitialItemSetupConfirmationExstNetworkMoveSubCon, "TBD Initial Transfer Semi Setup", "TBD Initial Transfer Semi Setup", string.Concat("Materials@ferrarausa.com", ";", GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), string.Concat(GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), "SAPInitialItemSetup");

            AddTaskAssignmentItemToList(list, WorkflowStep.SAPInitialItemSetupConfirmationNewFGTurnkeyFC01, "TBD Initial Transfer Semi Setup", "TBD Initial Transfer Semi Setup", string.Concat("Materials@ferrarausa.com", ";", GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), string.Concat(GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), "SAPInitialItemSetup");
            AddTaskAssignmentItemToList(list, WorkflowStep.SAPInitialItemSetupConfirmationNewPCFC01, "TBD Initial Transfer Semi Setup", "TBD Initial Transfer Semi Setup", string.Concat("Materials@ferrarausa.com", ";", GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), string.Concat(GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), "SAPInitialItemSetup");
            AddTaskAssignmentItemToList(list, WorkflowStep.SAPInitialItemSetupConfirmationNMPCFC01, "TBD Initial Transfer Semi Setup", "TBD Initial Transfer Semi Setup", string.Concat("Materials@ferrarausa.com", ";", GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), string.Concat(GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), "SAPInitialItemSetup");
            AddTaskAssignmentItemToList(list, WorkflowStep.SAPInitialItemSetupConfirmationExistingFGNMFC01, "TBD Initial Transfer Semi Setup", "TBD Initial Transfer Semi Setup", string.Concat("Materials@ferrarausa.com", ";", GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), string.Concat(GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), "SAPInitialItemSetup");


            AddTaskAssignmentItemToList(list, WorkflowStep.DEMANDPLANNING_NOTIFICATION, "NA", "NA", string.Concat(GlobalConstants.GROUP_DemandPlanning), "NA", "NA");
            AddTaskAssignmentItemToList(list, WorkflowStep.INTERNATIONALCOMP_NOTIFICATION, "NA", "NA", string.Concat(GlobalConstants.GROUP_InternationalCompliance), "NA", "NA");
            AddTaskAssignmentItemToList(list, WorkflowStep.QA, "Request for InTech Regulatory Form", "Request for InTech Regulatory Form", GlobalConstants.GROUP_InTechRegulatory, GlobalConstants.GROUP_InTechRegulatory, "QA");

            AddTaskAssignmentItemToList(list, WorkflowStep.OBMReview1, "Request for PM First Review", "PM First Review", "", "", "OBMFirstReview");

            AddTaskAssignmentItemToList(list, WorkflowStep.BEQRC, "Complete BE QRC Form", "Request for BE QRC Form – Marketing Confirmation", GlobalConstants.GROUP_IndividualBrandManager, GlobalConstants.GROUP_IndividualBrandManager, "BEQRC");
            AddTaskAssignmentItemToList(list, WorkflowStep.BEQRCRequest, "Request for  BE QRC Codes", "Request for  BE QRC Codes", "BE_QRCODE_REQUEST@ferrarausa.com", "BE_QRCODE_REQUEST@ferrarausa.com", "BEQRCRequest");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupPE, "Packaging Engineer (PE1)", "Request for Packaging", GlobalConstants.GROUP_PackagingEngineer, GlobalConstants.GROUP_PackagingEngineer, "BOMSetupPE");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProc, "Procurement", "Request for Printer-Supplier", GlobalConstants.GROUP_ProcurementPackaging, GlobalConstants.GROUP_ProcurementPackaging, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupPE2, "Packaging Enginneer (PE2)", "Request for Packaging Review", GlobalConstants.GROUP_PackagingEngineer, GlobalConstants.GROUP_PackagingEngineer, "BOMSetupPE2");
            AddTaskAssignmentItemToList(list, WorkflowStep.DEMANDFORECAST_NOTIFICATION, "NA", "NA", string.Concat(GlobalConstants.GROUP_DemandPlanning, ";", GlobalConstants.GROUP_IndividualPM, ";", GlobalConstants.GROUP_IndividualBrandManager), "NA", "NA");
            AddTaskAssignmentItemToList(list, WorkflowStep.SAPBOMSetup, "Request for SAP BOM Setup", "Request for SAP BOM Setup", GlobalConstants.GROUP_MasterData, GlobalConstants.GROUP_MasterData, "SAPBOMSetup");
            AddTaskAssignmentItemToList(list, WorkflowStep.SAPBOMSetupConfirmationExternalSemis, "Complete SAP BOM Setup", "SAP BOM Setup Confirmation", string.Concat("Materials@ferrarausa.com", ";", GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), string.Concat(GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), "SAPBOMSetup");
            AddTaskAssignmentItemToList(list, WorkflowStep.SAPBOMSetupConfirmationExternalSubconFG, "Complete SAP BOM Setup", "SAP BOM Setup Confirmation", string.Concat("Materials@ferrarausa.com", ";", GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), string.Concat(GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), "SAPBOMSetup");
            AddTaskAssignmentItemToList(list, WorkflowStep.SAPBOMSetupConfirmationExternalTurnkeyFG, "Complete SAP BOM Setup", "SAP BOM Setup Confirmation", string.Concat("Materials@ferrarausa.com", ";", GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), string.Concat(GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), "SAPBOMSetup");
            AddTaskAssignmentItemToList(list, WorkflowStep.SAPBOMSetupConfirmationInternalFG, "SAP BOM Setup Confirmation", "SAP BOM Setup Confirmation", string.Concat("Materials@ferrarausa.com", ";", GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), string.Concat(GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), "SAPBOMSetup");
            AddTaskAssignmentItemToList(list, WorkflowStep.SAPBOMSetupConfirmationExternalTurnkeyExistingFG, "SAP BOM Setup Confirmation", "SAP BOM Setup Confirmation", string.Concat("Materials@ferrarausa.com", ";", GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), string.Concat(GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), "SAPBOMSetup");
            AddTaskAssignmentItemToList(list, WorkflowStep.SAPBOMSetupConfirmationNewTSs, "SAP BOM Setup Confirmation", "SAP BOM Setup Confirmation", string.Concat("Materials@ferrarausa.com", ";", GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), string.Concat(GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), "SAPBOMSetup");
            AddTaskAssignmentItemToList(list, WorkflowStep.NewPackagingComponentsCreated, "New Packaging Components Created", "New Packaging Components Created", string.Concat("Materials@ferrarausa.com", ";", "michel.sabourin@ferrarausa.com", ";", "James.Mcvady@ferrarausa.com", ";", "Tye.Key@ferrarausa.com", ";", "Robert.Sammons@ferrarausa.com", ";", GlobalConstants.GROUP_IndividualBrandManager, "; ", GlobalConstants.GROUP_IndividualPM), string.Concat(GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPM), "SAPBOMSetup");
            AddTaskAssignmentItemToList(list, WorkflowStep.FGPackSpec, "Request for Finished Good Pack Spec", "Request for Finshed Good Pack Spec", GlobalConstants.GROUP_IndividualPackagingEngineer, GlobalConstants.GROUP_IndividualPackagingEngineer, "FinishedGoodPackSpec");

            AddTaskAssignmentItemToList(list, WorkflowStep.SAPCompleteItemSetup, "Request for SAP Complete Item Setup", "Request for SAP Complete Item Setup", GlobalConstants.GROUP_MasterData, "helpdesk@ferrarausa.com", "SAPCompleteItemSetup");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupMaterialWarehouse, "Request for Material Warehouse SetUp", "Request for Material Warehouse SetUp", GlobalConstants.GROUP_MaterialWarehouse, GlobalConstants.GROUP_MaterialWarehouse, "BOMSetupMaterialWarehouse");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupPE3, "Packaging Engineer(PE3)", "Request for Packaging", GlobalConstants.GROUP_IndividualPackagingEngineer, GlobalConstants.GROUP_IndividualPackagingEngineer, "BOMSetupPE3");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupPE3Submitted, "Packaging Engineer(PE3)", "PE3 Submitted", GlobalConstants.GROUP_IndividualPackagingEngineer, GlobalConstants.GROUP_IndividualPackagingEngineer, "BOMSetupPE3");


            AddTaskAssignmentItemToList(list, WorkflowStep.OBMReview2, "PM Second Review", "PM Second Review", "", "", "OBMSecondReview");
            AddTaskAssignmentItemToList(list, WorkflowStep.ComponentCostingSeasonal, "Seasonal Component Costing", "Seasonal Component Costing", "", "", "ComponentCostingSummary");
            AddTaskAssignmentItemToList(list, WorkflowStep.ComponentCostingCorrugatedPaperboard, "Corrugated/Paperboard Component Costing", "Corrugated/Paperboard Component Costing", "", "", "ComponentCostingSummary");
            AddTaskAssignmentItemToList(list, WorkflowStep.ComponentCostingFilmLabelRigidPlastic, "Film/Label/Rigid Plastic Component Costing", "Film/Label/Rigid Plastic Component Costing", "", "", "ComponentCostingSummary");
            AddTaskAssignmentItemToList(list, WorkflowStep.Graphics, "Graphics Request", "Graphics Request", GlobalConstants.GROUP_Graphics, GlobalConstants.GROUP_GraphicsTask, "GraphicsRequest");

            AddTaskAssignmentItemToList(list, WorkflowStep.SAPROUTINGSETUP_NOTIFICATION, "Plants Enter Routing into SAP", "Plants Enter Routing into SAP", string.Concat(GlobalConstants.GROUP_PlantForestPark), GlobalConstants.GROUP_PlantForestPark, "NA");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMACTIVEDATE_NOTIFICATION, "Set BOM/Component Active Date", "Set BOM/Component Active Date", string.Concat(GlobalConstants.GROUP_Obsolescence), GlobalConstants.GROUP_Obsolescence, "NA");
            AddTaskAssignmentItemToList(list, WorkflowStep.SAPCOSTINGDETAILS_NOTIFICATION, "Costing Details Entered into SAP", "Costing Details Entered into SAP", string.Concat(GlobalConstants.GROUP_ProcurementPackaging), GlobalConstants.GROUP_ProcurementPackaging, "NA");
            AddTaskAssignmentItemToList(list, WorkflowStep.SAPWAREHOUSEINFO_NOTIFICATION, "Warehouse info Entered into SAP", "Warehouse info Entered into SAP", string.Concat(GlobalConstants.GROUP_MasterData), GlobalConstants.GROUP_MasterData, "NA");
            AddTaskAssignmentItemToList(list, WorkflowStep.STANDARDCOSTENTRY_NOTIFICATION, "Standard Cost Entry into SAP", "Standard Cost Entry into SAP", string.Concat(GlobalConstants.GROUP_FinalCosting), GlobalConstants.GROUP_FinalCosting, "NA");
            AddTaskAssignmentItemToList(list, WorkflowStep.COSTFINISHEDGOOD_NOTIFICATION, "Cost Finished Good", "Cost Finished Good", string.Concat(GlobalConstants.GROUP_FinalCosting), GlobalConstants.GROUP_FinalCosting, "NA");
            AddTaskAssignmentItemToList(list, WorkflowStep.FINALCOSTINGREVIEW_NOTIFICATION, "Final Costing Review", "Final Costing Review", string.Concat(GlobalConstants.GROUP_PlantForestPark), GlobalConstants.GROUP_PlantForestPark, "NA");
            AddTaskAssignmentItemToList(list, WorkflowStep.PURCHASEPO_NOTIFICATION, "Purchase POs", "Purchase POs", string.Concat(GlobalConstants.GROUP_DemandPlanning), GlobalConstants.GROUP_IndividualPM, "NA");
            AddTaskAssignmentItemToList(list, WorkflowStep.REMOVESAPBLOCKS_NOTIFICATION, "Remove SAP Blocks", "Remove SAP Blocks", string.Concat(GlobalConstants.GROUP_MasterData), GlobalConstants.GROUP_MasterData, "NA");
            AddTaskAssignmentItemToList(list, WorkflowStep.CUSTOMERPOSCANBEENTERED_NOTIFICATION, "Customer POs can be Entered", "Customer POs can be Entered", string.Concat(GlobalConstants.GROUP_CustomerService), GlobalConstants.GROUP_CustomerService, "NA");

            AddTaskAssignmentItemToList(list, WorkflowStep.PLATES_SHIPPED_NOTIFICATION, "Waiting for Plates to Ship", "Waiting for Plates to Ship", string.Concat(GlobalConstants.GROUP_Graphics), GlobalConstants.GROUP_Graphics, "NA");

            AddTaskAssignmentItemToList(list, WorkflowStep.MaterialsReceivedCheck, "Materials Received Check", "Materials Received Check", "", GlobalConstants.GROUP_IndividualPM, "NA");
            AddTaskAssignmentItemToList(list, WorkflowStep.FirstProductionCheck, "First Production Check", "First Production Check", "", GlobalConstants.GROUP_IndividualPM, "NA");
            AddTaskAssignmentItemToList(list, WorkflowStep.DistributionCenterCheck, "Distribution Center Check", "Distribution Center Check", "", GlobalConstants.GROUP_IndividualPM, "NA");

            //Stage Gate Post Launch Notification
            AddTaskAssignmentItemToList(list, WorkflowStep.StageGatePostLaunchNotification, "Post Launch Notification", "Post Launch Notification", GlobalConstants.GROUP_PostLaunchNotificationRnDMembers, GlobalConstants.GROUP_PostLaunchNotificationRnDMembers, "NA");
            AddTaskAssignmentItemToList(list, WorkflowStep.ParentProjectSubmitted, "Parent Project Submitted", "Parent Project Submitted", "NA", "NA", "NA");
            AddTaskAssignmentItemToList(list, WorkflowStep.ParentProjectCompleted, "Parent Project Completed", "Parent Project Completed", "NA", "NA", "NA");
            AddTaskAssignmentItemToList(list, WorkflowStep.ParentProjectCancelled, "Parent Project Cancelled", "Parent Project Cacnelled", "NA", "NA", "NA");

            // All other Notifications
            AddTaskAssignmentItemToList(list, WorkflowStep.IPF_REJECTED, "Rejected", "Rejected", string.Concat(GlobalConstants.GROUP_IndividualBrandManager), "NA", "NA");
            AddTaskAssignmentItemToList(list, WorkflowStep.IPF_Submission, "NA", "NA", GlobalConstants.GROUP_IPFSubmissionMembers, "NA", "ItemProposal");
            AddTaskAssignmentItemToList(list, WorkflowStep.IPF_REQUESTFORINFORMATION, "NA", "NA", string.Concat(GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualInitiator), "NA", "NA");

            AddTaskAssignmentItemToList(list, WorkflowStep.PE_NOTIFICATION, "NA", "NA", string.Concat(GlobalConstants.GROUP_PackagingEngineer), "NA", "NA");
            AddTaskAssignmentItemToList(list, WorkflowStep.OBM_NOTIFICATION, "NA", "NA", string.Concat(GlobalConstants.GROUP_IndividualPM), "NA", "NA");
            AddTaskAssignmentItemToList(list, WorkflowStep.MATERIAL_NOTIFICATION, "NA", "NA", string.Concat(GlobalConstants.GROUP_PackagingEngineer), "NA", "NA");
            AddTaskAssignmentItemToList(list, WorkflowStep.TBDSAP_NOTIFICATION, "NA", "NA", string.Concat(GlobalConstants.GROUP_MasterData), "NA", "NA");

            AddTaskAssignmentItemToList(list, WorkflowStep.Cancelled, "Cancelled", "Cancelled", string.Concat(GlobalConstants.GROUP_ProcurementPackaging, ";", GlobalConstants.GROUP_MasterData, ";", GlobalConstants.GROUP_Operations, ";", GlobalConstants.GROUP_IndividualPM, ";", GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPackagingEngineer, ";", GlobalConstants.GROUP_Graphics, ";", GlobalConstants.GROUP_ProjectCancellationMembers, ";", GlobalConstants.GROUP_DemandPlanning), GlobalConstants.GROUP_ProcurementPackaging, "Cancelled");
            AddTaskAssignmentItemToList(list, WorkflowStep.ProjectRejected, "ProjectRejected", "ProjectRejected", string.Concat(GlobalConstants.GROUP_ProcurementPackaging, ";", GlobalConstants.GROUP_MasterData, ";", GlobalConstants.GROUP_Operations, ";", GlobalConstants.GROUP_IndividualPM, ";", GlobalConstants.GROUP_IndividualBrandManager, ";", GlobalConstants.GROUP_IndividualPackagingEngineer, ";", GlobalConstants.GROUP_Graphics, ";", GlobalConstants.GROUP_ProjectCancellationMembers, ";", GlobalConstants.GROUP_DemandPlanning), GlobalConstants.GROUP_ProcurementPackaging, "ProjectRejected");
            AddTaskAssignmentItemToList(list, WorkflowStep.NewFGWithReplacementItem, "NA", "NA", GlobalConstants.GROUP_NewFGWithReplacementItemMembers, GlobalConstants.GROUP_NewFGWithReplacementItemMembers, "NA");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcSeasonal, "Procurement", "Request for Printer-Supplier - Seasonal", GlobalConstants.GROUP_ProcurementSeasonal, GlobalConstants.GROUP_ProcurementSeasonal, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcNovelty, "Procurement", "Request for Printer-Supplier - Novelty", GlobalConstants.GROUP_ProcurementNovelty, GlobalConstants.GROUP_ProcurementNovelty, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcCoMan, "Procurement", "Request for Printer-Supplier - Co-Man", GlobalConstants.GROUP_ProcurementCoMan, GlobalConstants.GROUP_ProcurementCoMan, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcEBPCorrugated, "Procurement", "Request for Printer-Supplier - Corrugated", GlobalConstants.GROUP_ProcurementEBPCorrugated, GlobalConstants.GROUP_ProcurementEBPCorrugated, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcEBPOther, "Procurement", "Request for Printer-Supplier - Other", GlobalConstants.GROUP_ProcurementEBPOther, GlobalConstants.GROUP_ProcurementEBPOther, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcEBPAncillary, "Procurement", "Request for Printer-Supplier - Ancillary", GlobalConstants.GROUP_ProcurementEBPAncillary, GlobalConstants.GROUP_ProcurementEBPAncillary, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcEBPPaperboard, "Procurement", "Request for Printer-Supplier - Paperboard", GlobalConstants.GROUP_ProcurementEBPPaperboard, GlobalConstants.GROUP_ProcurementEBPPaperboard, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcEBPFilm, "Procurement", "Request for Printer-Supplier - Film", GlobalConstants.GROUP_ProcurementEBPFilm, GlobalConstants.GROUP_ProcurementEBPFilm, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcEBPPurchased, "Procurement", "Request for Printer-Supplier - FG Purchased", GlobalConstants.GROUP_ExternalManufacturing, GlobalConstants.GROUP_ExternalManufacturing, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcEBPLabel, "Procurement", "Request for Printer-Supplier - Label", GlobalConstants.GROUP_ProcurementEBPLabel, GlobalConstants.GROUP_ProcurementEBPLabel, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcEBPRigidPlastic, "Procurement", "Request for Printer-Supplier - Rigid Plastic", GlobalConstants.GROUP_ProcurementEBPRigidPlastic, GlobalConstants.GROUP_ProcurementEBPRigidPlastic, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcEBPMetal, "Procurement", "Request for Printer-Supplier - Metal", GlobalConstants.GROUP_ProcurementEBPMetal, GlobalConstants.GROUP_ProcurementEBPMetal, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcExternal, "Procurement", "Request for Printer-Supplier - External", GlobalConstants.GROUP_ExternalManufacturing, GlobalConstants.GROUP_ExternalManufacturing, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcSeasonalCorrugated, "Procurement", "Request for Printer-Supplier - Seasonal Corrugated", GlobalConstants.GROUP_ProcurementSeasonal, GlobalConstants.GROUP_ProcurementSeasonal, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcSeasonalOther, "Procurement", "Request for Printer-Supplier - Seasonal Other", GlobalConstants.GROUP_ProcurementSeasonal, GlobalConstants.GROUP_ProcurementSeasonal, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcSeasonalAncillary, "Procurement", "Request for Printer-Supplier - Seasonal Ancillary", GlobalConstants.GROUP_ProcurementSeasonal, GlobalConstants.GROUP_ProcurementSeasonal, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcSeasonalPaperboard, "Procurement", "Request for Printer-Supplier - Seasonal Paperboard", GlobalConstants.GROUP_ProcurementSeasonal, GlobalConstants.GROUP_ProcurementSeasonal, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcSeasonalFilm, "Procurement", "Request for Printer-Supplier - Seasonal Film", GlobalConstants.GROUP_ProcurementSeasonal, GlobalConstants.GROUP_ProcurementSeasonal, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcSeasonalPurchased, "Procurement", "Request for Printer-Supplier - Seasonal FG Purchased", GlobalConstants.GROUP_ExternalManufacturing, GlobalConstants.GROUP_ExternalManufacturing, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcSeasonalLabel, "Procurement", "Request for Printer-Supplier - Seasonal Label", GlobalConstants.GROUP_ProcurementSeasonal, GlobalConstants.GROUP_ProcurementSeasonal, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcSeasonalRigidPlastic, "Procurement", "Request for Printer-Supplier - Seasonal Rigid Plastic", GlobalConstants.GROUP_ProcurementSeasonal, GlobalConstants.GROUP_ProcurementSeasonal, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcSeasonalMetal, "Procurement", "Request for Printer-Supplier - Seasonal Metal", GlobalConstants.GROUP_ProcurementSeasonal, GlobalConstants.GROUP_ProcurementSeasonal, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcExternalCorrugated, "Procurement", "Request for Printer-Supplier - External Corrugated", GlobalConstants.GROUP_ExternalManufacturing, GlobalConstants.GROUP_ExternalManufacturing, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcExternalOther, "Procurement", "Request for Printer-Supplier - External Other", GlobalConstants.GROUP_ExternalManufacturing, GlobalConstants.GROUP_ExternalManufacturing, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcExternalAncillary, "Procurement", "Request for Printer-Supplier - External Ancillary", GlobalConstants.GROUP_ExternalManufacturing, GlobalConstants.GROUP_ExternalManufacturing, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcExternalPaperboard, "Procurement", "Request for Printer-Supplier - External Paperboard", GlobalConstants.GROUP_ExternalManufacturing, GlobalConstants.GROUP_ExternalManufacturing, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcExternalFilm, "Procurement", "Request for Printer-Supplier - External Film", GlobalConstants.GROUP_ExternalManufacturing, GlobalConstants.GROUP_ExternalManufacturing, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcExternalPurchased, "Procurement", "Request for Printer-Supplier - External FG Purchased", GlobalConstants.GROUP_ExternalManufacturing, GlobalConstants.GROUP_ExternalManufacturing, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcExternalLabel, "Procurement", "Request for Printer-Supplier - External Label", GlobalConstants.GROUP_ExternalManufacturing, GlobalConstants.GROUP_ExternalManufacturing, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcExternalRigidPlastic, "Procurement", "Request for Printer-Supplier - External Rigid Plastic", GlobalConstants.GROUP_ExternalManufacturing, GlobalConstants.GROUP_ExternalManufacturing, "BOMSetupProc");
            AddTaskAssignmentItemToList(list, WorkflowStep.BOMSetupProcExternalMetal, "Procurement", "Request for Printer-Supplier - External Metal", GlobalConstants.GROUP_ExternalManufacturing, GlobalConstants.GROUP_ExternalManufacturing, "BOMSetupProc");
            list.Update();
        }
        private static void AddTaskAssignmentItemToList(SPList list, WorkflowStep keyName, string taskTitle, string taskDescription, string emailGroups, string taskGroups, string compassPageName)
        {
            foreach (SPListItem existingItem in list.Items)
            {
                if (keyName.ToString() == existingItem[TaskAssignmentFieldName.WorkflowStep].ToString())
                    return;
            }

            var spListItem = list.Items.Add();
            spListItem[TaskAssignmentFieldName.WorkflowStep] = keyName.ToString();
            spListItem[TaskAssignmentFieldName.TaskGroups] = taskGroups;
            spListItem[TaskAssignmentFieldName.CompassPageName] = compassPageName;

            if (string.IsNullOrEmpty(emailGroups))
                spListItem[TaskAssignmentFieldName.EmailGroups] = GlobalConstants.GROUP_IndividualPM;
            else
                spListItem[TaskAssignmentFieldName.EmailGroups] = string.Concat(emailGroups, ";", GlobalConstants.GROUP_IndividualPM);

            spListItem[TaskAssignmentFieldName.TaskTitle] = taskTitle.ToString();
            spListItem[TaskAssignmentFieldName.TaskDescription] = taskDescription.ToString();
            spListItem.Update();
        }

        #endregion

        #region WorldSyncNutritionalsList
        private void CreateWorldSyncNutritionalsList()
        {
            bool needsListUpdate;
            SPList splist;
            SPView view;
            string description = "World Sync Nutritionals List";
            try
            {
                splist = currentWeb.Lists.TryGetList(GlobalConstants.LIST_WorldSyncNutritionals);

                //Creating the List if List does not exist 
                if (splist == null)
                    splist = SetupUtilities.CreateList(currentWeb, GlobalConstants.LIST_WorldSyncNutritionals, description);

                needsListUpdate = false;
                if (SetupUtilities.CreateField(splist, WorldSyncNutritionalsFields.CompassListItemId, WorldSyncNutritionalsFields.CompassListItemId_DisplayName, SPFieldType.Integer, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, WorldSyncNutritionalsFields.NutrientBasisQty, WorldSyncNutritionalsFields.NutrientBasisQty_DisplayName, SPFieldType.Integer, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, WorldSyncNutritionalsFields.NutrientBasisQtyType, WorldSyncNutritionalsFields.NutrientBasisQtyType_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, WorldSyncNutritionalsFields.NutrientBasisQtyUOM, WorldSyncNutritionalsFields.NutrientBasisQtyUOM_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, WorldSyncNutritionalsFields.ServingSizeDescription, WorldSyncNutritionalsFields.ServingSizeDescription_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, WorldSyncNutritionalsFields.ServingSize, WorldSyncNutritionalsFields.ServingSize_DisplayName, SPFieldType.Integer, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, WorldSyncNutritionalsFields.ServingSizeUOM, WorldSyncNutritionalsFields.ServingSizeUOM_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, WorldSyncNutritionalsFields.PreparationState, WorldSyncNutritionalsFields.PreparationState_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, WorldSyncNutritionalsFields.AllergenSpecificationAgency, WorldSyncNutritionalsFields.AllergenSpecificationAgency_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, WorldSyncNutritionalsFields.AllergenSpecificationName, WorldSyncNutritionalsFields.AllergenSpecificationName_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, WorldSyncNutritionalsFields.AllergenStatement, WorldSyncNutritionalsFields.AllergenStatement_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, WorldSyncNutritionalsFields.IngredientStatement, WorldSyncNutritionalsFields.IngredientStatement_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, WorldSyncNutritionalsFields.ServingsPerPackage, WorldSyncNutritionalsFields.ServingsPerPackage_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;

                if (needsListUpdate)
                    splist.Update();

                view = splist.Views["All Items"];
                view.Query = string.Format("<OrderBy><FieldRef Name='{0}' Ascending='FALSE' /></OrderBy>", WorldSyncNutritionalsFields.CompassListItemId);
                view.Update();
            }
            catch (Exception ex)
            {

            }
        }
        private void CreateWorldSyncNutritionalsDetailList()
        {
            bool needsListUpdate;
            SPList splist;
            SPView view;
            string description = "World Sync Nutritionals Detail List";
            try
            {
                splist = currentWeb.Lists.TryGetList(GlobalConstants.LIST_WorldSyncNutritionalsDetail);

                //Creating the List if List does not exist 
                if (splist == null)
                    splist = SetupUtilities.CreateList(currentWeb, GlobalConstants.LIST_WorldSyncNutritionalsDetail, description);

                needsListUpdate = false;
                if (SetupUtilities.CreateField(splist, WorldSyncNutritionalsDetailFields.CompassListItemId, WorldSyncNutritionalsDetailFields.CompassListItemId_DisplayName, SPFieldType.Integer, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, WorldSyncNutritionalsDetailFields.NutrientQtyContained, WorldSyncNutritionalsDetailFields.NutrientQtyContained_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, WorldSyncNutritionalsDetailFields.NutrientQtyContainedUOM, WorldSyncNutritionalsDetailFields.NutrientQtyContainedUOM_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, WorldSyncNutritionalsDetailFields.NutrientType, WorldSyncNutritionalsDetailFields.NutrientType_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, WorldSyncNutritionalsDetailFields.PctDailyValue, WorldSyncNutritionalsDetailFields.PctDailyValue_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, WorldSyncNutritionalsDetailFields.NutrientQtyContainedMeasPerc, WorldSyncNutritionalsDetailFields.NutrientQtyContainedMeasPerc_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, WorldSyncNutritionalsDetailFields.DailyValueIntakePct, WorldSyncNutritionalsDetailFields.DailyValueIntakePct_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;

                if (needsListUpdate)
                    splist.Update();

                view = splist.Views["All Items"];
                view.Query = string.Format("<OrderBy><FieldRef Name='{0}' Ascending='FALSE' /></OrderBy>", WorldSyncNutritionalsDetailFields.CompassListItemId);
                view.Update();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region CompassWorldSyncList
        private void CreateCompassWorldSyncList()
        {
            bool needsListUpdate;
            SPList splist;
            SPView view;
            string description = "Compass World Sync List";
            try
            {
                splist = currentWeb.Lists.TryGetList(GlobalConstants.LIST_CompassWorldSyncList);

                //Creating the List if List does not exist 
                if (splist == null)
                    splist = SetupUtilities.CreateList(currentWeb, GlobalConstants.LIST_CompassWorldSyncList, description);

                needsListUpdate = false;
                if (SetupUtilities.CreateField(splist, CompassWorldSyncListFields.CompassListItemId, CompassWorldSyncListFields.CompassListItemId_DisplayName, SPFieldType.Integer, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, CompassWorldSyncListFields.ParentGlobalId, CompassWorldSyncListFields.ParentGlobalId_DisplayName, SPFieldType.Integer, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, CompassWorldSyncListFields.TargetMarket, CompassWorldSyncListFields.TargetMarket_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, CompassWorldSyncListFields.ProductType, CompassWorldSyncListFields.ProductType_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, CompassWorldSyncListFields.GPCClassification, CompassWorldSyncListFields.GPCClassification_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, CompassWorldSyncListFields.BrandOwnerGLN, CompassWorldSyncListFields.BrandOwnerGLN_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, CompassWorldSyncListFields.BaseUnitIndicator, CompassWorldSyncListFields.BaseUnitIndicator_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, CompassWorldSyncListFields.ConsumerUnitIndicator, CompassWorldSyncListFields.ConsumerUnitIndicator_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, CompassWorldSyncListFields.AlternateClassificationScheme, CompassWorldSyncListFields.AlternateClassificationScheme_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, CompassWorldSyncListFields.Code, CompassWorldSyncListFields.Code_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, CompassWorldSyncListFields.AlternateItemIdAgency, CompassWorldSyncListFields.AlternateItemIdAgency_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, CompassWorldSyncListFields.GS1TradeItemsIDKeyCode, CompassWorldSyncListFields.GS1TradeItemsIDKeyCode_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, CompassWorldSyncListFields.OrderingUnitIndicator, CompassWorldSyncListFields.OrderingUnitIndicator_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, CompassWorldSyncListFields.DispatchUnitIndicator, CompassWorldSyncListFields.DispatchUnitIndicator_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, CompassWorldSyncListFields.InvoiceUnitIndicator, CompassWorldSyncListFields.InvoiceUnitIndicator_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, CompassWorldSyncListFields.DataCarrierTypeCode, CompassWorldSyncListFields.DataCarrierTypeCode_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, CompassWorldSyncListFields.TradeChannel, CompassWorldSyncListFields.TradeChannel_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, CompassWorldSyncListFields.TemperatureQualitiferCode, CompassWorldSyncListFields.TemperatureQualitiferCode_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, CompassWorldSyncListFields.CustomerBrandName, CompassWorldSyncListFields.CustomerBrandName_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, CompassWorldSyncListFields.NetContent, CompassWorldSyncListFields.NetContent_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;
                if (SetupUtilities.CreateField(splist, CompassWorldSyncListFields.QtyOfNextLevelItems, CompassWorldSyncListFields.QtyOfNextLevelItems_DisplayName, SPFieldType.Text, false))
                    needsListUpdate = true;

                if (needsListUpdate)
                    splist.Update();

                view = splist.Views["All Items"];
                view.Query = string.Format("<OrderBy><FieldRef Name='{0}' Ascending='FALSE' /></OrderBy>", CompassWorldSyncListFields.CompassListItemId);
                view.Update();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Logs list
        private void CreateLogList()
        {
            try
            {
                string description = "Compass System Logs";

                SPList splist = currentWeb.Lists.TryGetList(GlobalConstants.LIST_LogsListName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(currentWeb, GlobalConstants.LIST_LogsListName, description);
                }

                var needsListUpdate = false;
                if (SetupUtilities.CreateField(splist, LogListNames.Category, LogListNames.Category, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, LogListNames.Message, LogListNames.Message, SPFieldType.Note, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, LogListNames.Form, LogListNames.Form, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, LogListNames.Method, LogListNames.Method, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, LogListNames.CreatedDate, LogListNames.CreatedDate, SPFieldType.DateTime, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, LogListNames.AdditionalInfo, LogListNames.AdditionalInfo_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (needsListUpdate)
                    splist.Update();

                SPView view = splist.Views["All Items"];
                view.ViewFields.DeleteAll();
                view.ViewFields.Add(LogListNames.Category);
                view.ViewFields.Add(LogListNames.Message);
                view.ViewFields.Add(LogListNames.CreatedBy);
                view.ViewFields.Add(LogListNames.CreatedDate);

                view.Query = string.Format("<OrderBy><FieldRef Name='{0}' Ascending='FALSE' /></OrderBy>", LogListNames.CreatedDate);
                view.Update();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Form Access List
        private void CreateFormAccessList()
        {
            if (currentWeb != null)
            {
                SPList splist = currentWeb.Lists.TryGetList(GlobalConstants.LIST_FormAccessListName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(currentWeb, GlobalConstants.LIST_FormAccessListName, GlobalConstants.LIST_FormAccessListName);
                }

                bool needsListUpdate = false;

                bool fieldExist = splist.Fields.ContainsFieldWithStaticName(FormAccessListFields.AccessGroups);
                if (!fieldExist)
                {
                    splist.Fields.Add(FormAccessListFields.AccessGroups, SPFieldType.Text, true);
                    SPView view = splist.DefaultView;
                    view.ViewFields.Add(FormAccessListFields.AccessGroups);
                    view.Update();
                    needsListUpdate = true;
                }

                fieldExist = splist.Fields.ContainsFieldWithStaticName(FormAccessListFields.EditGroups);
                if (!fieldExist)
                {
                    splist.Fields.Add(FormAccessListFields.EditGroups, SPFieldType.Text, true);
                    SPView view = splist.DefaultView;
                    view.ViewFields.Add(FormAccessListFields.EditGroups);
                    view.Update();
                    needsListUpdate = true;
                }

                if (needsListUpdate)
                    splist.Update();
            }
        }
        private void AttachFormAccessListEventReceiver()
        {
            SPList wfList = currentWeb.Lists.TryGetList(GlobalConstants.LIST_FormAccessListName);
            if (wfList != null)
            {
                DeleteEventReceiver(wfList);

                wfList.EventReceivers.Add(SPEventReceiverType.ItemUpdated,
                    GlobalConstants.SharePointServiceAssemblyName,
                    GlobalConstants.FormAccessListEventReceiverClassName);

                wfList.EventReceivers.Add(SPEventReceiverType.ItemAdded,
                    GlobalConstants.SharePointServiceAssemblyName,
                    GlobalConstants.FormAccessListEventReceiverClassName);

                wfList.EventReceivers.Add(SPEventReceiverType.ItemDeleting,
                    GlobalConstants.SharePointServiceAssemblyName,
                    GlobalConstants.FormAccessListEventReceiverClassName);

                wfList.Update();
            }
        }
        private void PopulatingFormAccessListData()
        {
            var list = currentWeb.Lists.TryGetList(GlobalConstants.LIST_FormAccessListName);
            if (list == null) return;

            AddFormAccessItemToList(list, CompassForm.IPF,
                string.Concat(GlobalConstants.GROUP_Marketing),
                string.Concat(GlobalConstants.GROUP_Marketing));

            AddFormAccessItemToList(list, CompassForm.SrOBMApproval,
                string.Concat(GlobalConstants.GROUP_ProjectManagers, ",", GlobalConstants.GROUP_SeniorProjectManager),
                string.Concat(GlobalConstants.GROUP_ProjectManagers, ",", GlobalConstants.GROUP_SeniorProjectManager));

            AddFormAccessItemToList(list, CompassForm.TradePromo,
                string.Concat(GlobalConstants.GROUP_TradePromo),
                string.Concat(GlobalConstants.GROUP_TradePromo));

            AddFormAccessItemToList(list, CompassForm.EstPricing,
                string.Concat(GlobalConstants.GROUP_EstimatedPricing),
                string.Concat(GlobalConstants.GROUP_EstimatedPricing));

            AddFormAccessItemToList(list, CompassForm.EstBracketPricing,
                string.Concat(GlobalConstants.GROUP_EstimatedBracketPricing),
                string.Concat(GlobalConstants.GROUP_EstimatedBracketPricing));

            AddFormAccessItemToList(list, CompassForm.Distribution,
                string.Concat(GlobalConstants.GROUP_Distribution),
                string.Concat(GlobalConstants.GROUP_Distribution, ",", GlobalConstants.GROUP_ProjectManagers));

            AddFormAccessItemToList(list, CompassForm.SAPInitialSetup,
                string.Concat(GlobalConstants.GROUP_MasterData),
                GlobalConstants.GROUP_MasterData);
            AddFormAccessItemToList(list, CompassForm.PrelimSAPInitialSetup,
                string.Concat(GlobalConstants.GROUP_MasterData),
                GlobalConstants.GROUP_MasterData);

            AddFormAccessItemToList(list, CompassForm.Operations,
                string.Concat(GlobalConstants.GROUP_Operations),
                string.Concat(GlobalConstants.GROUP_Operations, ",", GlobalConstants.GROUP_ProjectManagers));

            AddFormAccessItemToList(list, CompassForm.ExternalMfg,
                string.Concat(GlobalConstants.GROUP_ExternalManufacturing, ",", GlobalConstants.GROUP_Marketing, ",", GlobalConstants.GROUP_Distribution, ",", GlobalConstants.GROUP_QualityAssurance),
                GlobalConstants.GROUP_ExternalManufacturing);

            AddFormAccessItemToList(list, CompassForm.QA,
                GlobalConstants.GROUP_QualityAssurance,
                GlobalConstants.GROUP_QualityAssurance);

            AddFormAccessItemToList(list, CompassForm.OBMReview1,
                string.Concat(GlobalConstants.GROUP_ProjectManagers),
                string.Concat(GlobalConstants.GROUP_ProjectManagers));

            AddFormAccessItemToList(list, CompassForm.BOMSetupPE,
                string.Concat(GlobalConstants.GROUP_PackagingEngineer),
                string.Concat(GlobalConstants.GROUP_PackagingEngineer));

            AddFormAccessItemToList(list, CompassForm.BOMSetupPE2,
                string.Concat(GlobalConstants.GROUP_PackagingEngineer),
                string.Concat(GlobalConstants.GROUP_PackagingEngineer));

            AddFormAccessItemToList(list, CompassForm.BOMSetupPE3,
                string.Concat(GlobalConstants.GROUP_PackagingEngineer),
                string.Concat(GlobalConstants.GROUP_PackagingEngineer));

            AddFormAccessItemToList(list, CompassForm.BOMSetupProc,
                string.Concat(GlobalConstants.GROUP_ProcurementPackaging, ",", GlobalConstants.GROUP_ProcSeasonal, ",", GlobalConstants.GROUP_ProcEverydayCorrugatePPBRD, ",", GlobalConstants.GROUP_ProcEverydayFilmLabelTubs, ",", GlobalConstants.GROUP_ExternalManTurnKeyFG),
                string.Concat(GlobalConstants.GROUP_ProcurementPackaging, ",", GlobalConstants.GROUP_ProcSeasonal, ",", GlobalConstants.GROUP_ProcEverydayCorrugatePPBRD, ",", GlobalConstants.GROUP_ProcEverydayFilmLabelTubs, ",", GlobalConstants.GROUP_ExternalManTurnKeyFG));

            AddFormAccessItemToList(list, CompassForm.SAPBOMSetup,
                string.Concat(GlobalConstants.GROUP_MasterData),
                GlobalConstants.GROUP_MasterData);

            AddFormAccessItemToList(list, CompassForm.BOMSetupMaterialWarehouse,
                string.Concat(GlobalConstants.GROUP_MaterialWarehouse),
                GlobalConstants.GROUP_MaterialWarehouse);

            AddFormAccessItemToList(list, CompassForm.SAPCompleteItemSetup,
               string.Concat(GlobalConstants.GROUP_MasterData),
               GlobalConstants.GROUP_MasterData);

            AddFormAccessItemToList(list, CompassForm.OBMReview2,
                string.Concat(GlobalConstants.GROUP_ProjectManagers),
                string.Concat(GlobalConstants.GROUP_ProjectManagers));

            AddFormAccessItemToList(list, CompassForm.FGPackSpec,
               string.Concat(GlobalConstants.GROUP_PackagingEngineer),
               GlobalConstants.GROUP_PackagingEngineer);

            AddFormAccessItemToList(list, CompassForm.Graphics,
                string.Concat(GlobalConstants.GROUP_Graphics),
                string.Concat(GlobalConstants.GROUP_Graphics));

            AddFormAccessItemToList(list, CompassForm.OBMReview3,
                string.Concat(GlobalConstants.GROUP_ProjectManagers),
                string.Concat(GlobalConstants.GROUP_ProjectManagers));

            AddFormAccessItemToList(list, CompassForm.ComponentCosting,
                string.Concat(GlobalConstants.GROUP_FinalCosting),
                string.Concat(GlobalConstants.GROUP_FinalCosting));

            AddFormAccessItemToList(list, CompassForm.WORKFLOW,
                string.Concat(GlobalConstants.GROUP_Marketing, ",", GlobalConstants.GROUP_InitialCapacity, ",", GlobalConstants.GROUP_TradePromo, ",", GlobalConstants.GROUP_Distribution,
                     ",", GlobalConstants.GROUP_Graphics, ",", GlobalConstants.GROUP_InitialCosting),
                string.Concat(GlobalConstants.GROUP_ProjectManagers));

            AddFormAccessItemToList(list, CompassForm.PACKAGINGENTRY,
                string.Concat(GlobalConstants.GROUP_Marketing, ",", GlobalConstants.GROUP_MasterData, ",", GlobalConstants.GROUP_PackagingEngineer, ",", GlobalConstants.GROUP_ProcurementPackaging),
                string.Concat(GlobalConstants.GROUP_Marketing, ",", GlobalConstants.GROUP_MasterData, ",", GlobalConstants.GROUP_PackagingEngineer, ",", GlobalConstants.GROUP_ProcurementPackaging));

            list.Update();
        }
        private static void AddFormAccessItemToList(SPList list, CompassForm formName, string accessGroups, string editGroups)
        {
            foreach (SPListItem existingItem in list.Items)
            {
                if (formName.ToString() == existingItem[TaskAssignmentFieldName.WorkflowStep].ToString())
                    return;
            }

            SPListItem spListItem = list.Items.Add();

            spListItem[FormAccessListFields.Title] = formName.ToString();
            // PMs and OBM Admins have read access to every form
            if (string.IsNullOrEmpty(accessGroups))
                spListItem[FormAccessListFields.AccessGroups] = string.Concat(GlobalConstants.GROUP_ProjectManagers, ",", GlobalConstants.GROUP_OBMAdmins);
            else
                spListItem[FormAccessListFields.AccessGroups] = string.Concat(accessGroups, ",", GlobalConstants.GROUP_ProjectManagers, ",", GlobalConstants.GROUP_OBMAdmins);
            // OBM Admins have edit access to every form
            if (string.IsNullOrEmpty(editGroups))
                spListItem[FormAccessListFields.EditGroups] = string.Concat(GlobalConstants.GROUP_OBMAdmins);
            else
                spListItem[FormAccessListFields.EditGroups] = string.Concat(editGroups, ",", GlobalConstants.GROUP_OBMAdmins);
            spListItem.Update();
        }

        #endregion

        #region Compass News List
        private void CreateCompassNewsList()
        {
            if (currentWeb == null)
                return;

            SPList splist = currentWeb.Lists.TryGetList(GlobalConstants.LIST_LatestNewsListName);

            //Creating the List if List does not exist 
            if (splist == null)
            {
                splist = SetupUtilities.CreateList(currentWeb, GlobalConstants.LIST_LatestNewsListName, GlobalConstants.LIST_LatestNewsListName);
            }

        }
        #endregion
        #region Compass Project Status List
        private void CreateProjectTimelineDetailsList()
        {
            try
            {
                if (currentWeb == null)
                    return;

                SPList splist = currentWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectTimelineDetailsList);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(currentWeb, GlobalConstants.LIST_ProjectTimelineDetailsList, GlobalConstants.LIST_ProjectTimelineDetailsList);
                }

                splist.EnableVersioning = true;
                splist.EnableMinorVersions = false;
                var needsListUpdate = false;
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.compassListItemId, ProjectStatusDatesFields.compassListItemId, SPFieldType.Number, false))
                {
                    needsListUpdate = true;
                }
                var RowTypes = new List<string>{
                    "Original",
                    "Actual"
                };
                if (SetupUtilities.CreateFieldChoice(splist, ProjectStatusDatesFields.RowType, ProjectStatusDatesFields.RowType_DisplayName, false, RowTypes))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.SrOBMApprovalStart, ProjectStatusDatesFields.SrOBMApprovalStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.SrOBMApprovalEnd, ProjectStatusDatesFields.SrOBMApprovalEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.SrOBMApprovalDuration, ProjectStatusDatesFields.SrOBMApprovalDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.PrelimSAPInitialSetupStart, ProjectStatusDatesFields.PrelimSAPInitialSetupStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.PrelimSAPInitialSetupEnd, ProjectStatusDatesFields.PrelimSAPInitialSetupEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.PrelimSAPInitialSetupDuration, ProjectStatusDatesFields.PrelimSAPInitialSetupDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.TradePromoStart, ProjectStatusDatesFields.TradePromoStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.TradePromoEnd, ProjectStatusDatesFields.TradePromoEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.TradePromoDuration, ProjectStatusDatesFields.TradePromoDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.EstPricingStart, ProjectStatusDatesFields.EstPricingStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.EstPricingEnd, ProjectStatusDatesFields.EstPricingEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.EstPricingDuration, ProjectStatusDatesFields.EstPricingDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.EstBracketPricingStart, ProjectStatusDatesFields.EstBracketPricingStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.EstBracketPricingEnd, ProjectStatusDatesFields.EstBracketPricingEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.EstBracketPricingDuration, ProjectStatusDatesFields.EstBracketPricingDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.EstPricingStart, ProjectStatusDatesFields.EstPricingStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.EstPricingEnd, ProjectStatusDatesFields.EstPricingEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.EstPricingDuration, ProjectStatusDatesFields.EstPricingDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.EstBracketPricingStart, ProjectStatusDatesFields.EstBracketPricingStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.EstBracketPricingEnd, ProjectStatusDatesFields.EstBracketPricingEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.EstBracketPricingDuration, ProjectStatusDatesFields.EstBracketPricingDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.DistributionStart, ProjectStatusDatesFields.DistributionStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.DistributionEnd, ProjectStatusDatesFields.DistributionEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.DistributionDuration, ProjectStatusDatesFields.DistributionDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.OperationsStart, ProjectStatusDatesFields.OperationsStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.OperationsEnd, ProjectStatusDatesFields.OperationsEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.OperationsDuration, ProjectStatusDatesFields.OperationsDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ExternalMfgStart, ProjectStatusDatesFields.ExternalMfgStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ExternalMfgEnd, ProjectStatusDatesFields.ExternalMfgEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ExternalMfgDuration, ProjectStatusDatesFields.ExternalMfgDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.QAStart, ProjectStatusDatesFields.QAStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.QAEnd, ProjectStatusDatesFields.QAEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.QADuration, ProjectStatusDatesFields.QADuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.OBMReview1Start, ProjectStatusDatesFields.OBMReview1Start_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.OBMReview1End, ProjectStatusDatesFields.OBMReview1End_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.OBMReview1Duration, ProjectStatusDatesFields.OBMReview1Duration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.SAPInitialSetupStart, ProjectStatusDatesFields.SAPInitialSetupStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.SAPInitialSetupEnd, ProjectStatusDatesFields.SAPInitialSetupEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.SAPInitialSetupDuration, ProjectStatusDatesFields.SAPInitialSetupDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.BOMSetupPEStart, ProjectStatusDatesFields.BOMSetupPEStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.BOMSetupPEEnd, ProjectStatusDatesFields.BOMSetupPEEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.BOMSetupPEDuration, ProjectStatusDatesFields.BOMSetupPEDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.BOMSetupProcStart, ProjectStatusDatesFields.BOMSetupProcStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.BOMSetupProcEnd, ProjectStatusDatesFields.BOMSetupProcEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.BOMSetupProcDuration, ProjectStatusDatesFields.BOMSetupProcDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcAncillaryStart, ProjectStatusDatesFields.ProcAncillaryStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcCorrugatedStart, ProjectStatusDatesFields.ProcCorrugatedStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcFilmStart, ProjectStatusDatesFields.ProcFilmStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcLabelStart, ProjectStatusDatesFields.ProcLabelStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcMetalStart, ProjectStatusDatesFields.ProcMetalStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcOtherStart, ProjectStatusDatesFields.ProcOtherStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcPaperboardStart, ProjectStatusDatesFields.ProcPaperboardStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcFGPurchasedStart, ProjectStatusDatesFields.ProcFGPurchasedStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcRigidPlasticStart, ProjectStatusDatesFields.ProcRigidPlasticStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcExternalStart, ProjectStatusDatesFields.ProcExternalStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcAncillaryDuration, ProjectStatusDatesFields.ProcAncillaryDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcCorrugatedDuration, ProjectStatusDatesFields.ProcCorrugatedDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcFilmDuration, ProjectStatusDatesFields.ProcFilmDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcLabelDuration, ProjectStatusDatesFields.ProcLabelDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcMetalDuration, ProjectStatusDatesFields.ProcMetalDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcOtherDuration, ProjectStatusDatesFields.ProcOtherDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcPaperboardDuration, ProjectStatusDatesFields.ProcPaperboardDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcFGPurchasedDuration, ProjectStatusDatesFields.ProcFGPurchasedDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcRigidPlasticDuration, ProjectStatusDatesFields.ProcRigidPlasticDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcExternalDuration, ProjectStatusDatesFields.ProcExternalDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcAncillaryEnd, ProjectStatusDatesFields.ProcAncillaryEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcCorrugatedEnd, ProjectStatusDatesFields.ProcCorrugatedEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcFilmEnd, ProjectStatusDatesFields.ProcFilmEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcLabelEnd, ProjectStatusDatesFields.ProcLabelEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcMetalEnd, ProjectStatusDatesFields.ProcMetalEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcOtherEnd, ProjectStatusDatesFields.ProcOtherEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcPaperboardEnd, ProjectStatusDatesFields.ProcPaperboardEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcFGPurchasedEnd, ProjectStatusDatesFields.ProcFGPurchasedEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcRigidPlasticEnd, ProjectStatusDatesFields.ProcRigidPlasticEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcExternalEnd, ProjectStatusDatesFields.ProcExternalEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.BOMSetupPE2Start, ProjectStatusDatesFields.BOMSetupPE2Start_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.BOMSetupPE2End, ProjectStatusDatesFields.BOMSetupPE2End_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.BOMSetupPE2Duration, ProjectStatusDatesFields.BOMSetupPE2Duration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.BOMSetupPE3Start, ProjectStatusDatesFields.BOMSetupPE3Start_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.BOMSetupPE3End, ProjectStatusDatesFields.BOMSetupPE3End_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.BOMSetupPE3Duration, ProjectStatusDatesFields.BOMSetupPE3Duration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.SAPBOMSetupStart, ProjectStatusDatesFields.SAPBOMSetupStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.SAPBOMSetupEnd, ProjectStatusDatesFields.SAPBOMSetupEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.SAPBOMSetupDuration, ProjectStatusDatesFields.SAPBOMSetupDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.OBMReview2Start, ProjectStatusDatesFields.OBMReview2Start_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.OBMReview2End, ProjectStatusDatesFields.OBMReview2End_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.OBMReview2Duration, ProjectStatusDatesFields.OBMReview2Duration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.FGPackSpecStart, ProjectStatusDatesFields.FGPackSpecStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.FGPackSpecEnd, ProjectStatusDatesFields.FGPackSpecEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.FGPackSpecDuration, ProjectStatusDatesFields.FGPackSpecDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.GRAPHICSStart, ProjectStatusDatesFields.GRAPHICSStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.GRAPHICSEnd, ProjectStatusDatesFields.GRAPHICSEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.GRAPHICSDuration, ProjectStatusDatesFields.GRAPHICSDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.FCSTStart, ProjectStatusDatesFields.FCSTStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.FCSTEnd, ProjectStatusDatesFields.FCSTEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.FCSTDuration, ProjectStatusDatesFields.FCSTDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.SAPRoutingSetupStart, ProjectStatusDatesFields.SAPRoutingSetupStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.SAPRoutingSetupEnd, ProjectStatusDatesFields.SAPRoutingSetupEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.SAPRoutingSetupDuration, ProjectStatusDatesFields.SAPRoutingSetupDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.CustomerPOStart, ProjectStatusDatesFields.CustomerPOStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.CustomerPOEnd, ProjectStatusDatesFields.CustomerPOEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.CustomerPODuration, ProjectStatusDatesFields.CustomerPODuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.MaterialsRcvdChkStart, ProjectStatusDatesFields.MaterialsRcvdChkStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.MaterialsRcvdChkEnd, ProjectStatusDatesFields.MaterialsRcvdChkEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.MaterialsRcvdChkDuration, ProjectStatusDatesFields.MaterialsRcvdChkDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.BOMSetupPE3Status, ProjectStatusDatesFields.BOMSetupPE3Status_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.BOMSetupPE2Status, ProjectStatusDatesFields.BOMSetupPE2Status_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.BOMSetupPEStatus, ProjectStatusDatesFields.BOMSetupPEStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.BOMSetupProcStatus, ProjectStatusDatesFields.BOMSetupProcStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.CustomerPOStatus, ProjectStatusDatesFields.CustomerPOStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.DistributionStatus, ProjectStatusDatesFields.DistributionStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcExternalStatus, ProjectStatusDatesFields.ProcExternalStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ExternalMfgStatus, ProjectStatusDatesFields.ExternalMfgStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.FCSTStatus, ProjectStatusDatesFields.FCSTStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.FGPackSpecStatus, ProjectStatusDatesFields.FGPackSpecStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.GRAPHICSStatus, ProjectStatusDatesFields.GRAPHICSStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.MaterialsRcvdChkStatus, ProjectStatusDatesFields.MaterialsRcvdChkStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.OBMReview1Status, ProjectStatusDatesFields.OBMReview1Status_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.OBMReview2Status, ProjectStatusDatesFields.OBMReview2Status_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.OperationsStatus, ProjectStatusDatesFields.OperationsStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.PrelimSAPInitialSetupStatus, ProjectStatusDatesFields.PrelimSAPInitialSetupStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcAncillaryStatus, ProjectStatusDatesFields.ProcAncillaryStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcCorrugatedStatus, ProjectStatusDatesFields.ProcCorrugatedStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcFGPurchasedStatus, ProjectStatusDatesFields.ProcFGPurchasedStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcFilmStatus, ProjectStatusDatesFields.ProcFilmStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcLabelStatus, ProjectStatusDatesFields.ProcLabelStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcMetalStatus, ProjectStatusDatesFields.ProcMetalStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcOtherStatus, ProjectStatusDatesFields.ProcOtherStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcPaperboardStatus, ProjectStatusDatesFields.ProcPaperboardStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcRigidPlasticStatus, ProjectStatusDatesFields.ProcRigidPlasticStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.QAStatus, ProjectStatusDatesFields.QAStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.SAPBOMSetupStatus, ProjectStatusDatesFields.SAPBOMSetupStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.SAPInitialSetupStatus, ProjectStatusDatesFields.SAPInitialSetupStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.SAPRoutingSetupStatus, ProjectStatusDatesFields.SAPRoutingSetupStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.SrOBMApprovalStatus, ProjectStatusDatesFields.SrOBMApprovalStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.TradePromoStatus, ProjectStatusDatesFields.TradePromoStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.EstPricingStatus, ProjectStatusDatesFields.EstPricingStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.EstBracketPricingStatus, ProjectStatusDatesFields.EstBracketPricingStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }

                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcSeasonalStart, ProjectStatusDatesFields.ProcSeasonalStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcSeasonalEnd, ProjectStatusDatesFields.ProcSeasonalEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcSeasonalDuration, ProjectStatusDatesFields.ProcSeasonalDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcSeasonalStatus, ProjectStatusDatesFields.ProcSeasonalStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcCoManStart, ProjectStatusDatesFields.ProcCoManStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcCoManEnd, ProjectStatusDatesFields.ProcCoManEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcCoManDuration, ProjectStatusDatesFields.ProcCoManDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcCoManStatus, ProjectStatusDatesFields.ProcCoManStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }

                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcNoveltyStart, ProjectStatusDatesFields.ProcNoveltyStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcNoveltyEnd, ProjectStatusDatesFields.ProcNoveltyEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcNoveltyDuration, ProjectStatusDatesFields.ProcNoveltyDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.ProcNoveltyStatus, ProjectStatusDatesFields.ProcNoveltyStatus_DisplayName, SPFieldType.Text, false)) { needsListUpdate = true; }

                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.MaterialWarehouseSetUpStart, ProjectStatusDatesFields.MaterialWarehouseSetUpStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.MaterialWarehouseSetUpEnd, ProjectStatusDatesFields.MaterialWarehouseSetUpEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.MaterialWarehouseSetUpDuration, ProjectStatusDatesFields.MaterialWarehouseSetUpDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.MaterialWarehouseSetUpStatus, ProjectStatusDatesFields.MaterialWarehouseSetUpStatus_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.SAPCompleteItemSetupStart, ProjectStatusDatesFields.SAPCompleteItemSetupStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.BEQRCStart, ProjectStatusDatesFields.BEQRCStart_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.SAPCompleteItemSetupEnd, ProjectStatusDatesFields.SAPCompleteItemSetupEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.BEQRCEnd, ProjectStatusDatesFields.BEQRCEnd_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.SAPCompleteItemSetupDuration, ProjectStatusDatesFields.SAPCompleteItemSetupDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.BEQRCDuration, ProjectStatusDatesFields.BEQRCDuration_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.SAPCompleteItemSetupStatus, ProjectStatusDatesFields.SAPCompleteItemSetupStatus_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.BEQRCStatus, ProjectStatusDatesFields.BEQRCStatus_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.IPFSubmitted, ProjectStatusDatesFields.IPFSubmitted_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.FirstShipDate, ProjectStatusDatesFields.FirstShipDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.RevisedFirstShipDate, ProjectStatusDatesFields.RevisedFirstShipDate_DisplayName, SPFieldType.Text, false))
                {
                    needsListUpdate = true;
                }
                if (SetupUtilities.CreateField(splist, ProjectStatusDatesFields.FirstProductionDate, ProjectStatusDatesFields.FirstProductionDate_DisplayName, SPFieldType.Text, false))
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
        #region Compass Graphics Logs List
        private void CreateCompassGraphicsLogsList()
        {
            if (currentWeb == null)
                return;

            SPList splist = currentWeb.Lists.TryGetList(GlobalConstants.LIST_GraphicsLogsListName);

            //Creating the List if List does not exist 
            if (splist == null)
            {
                splist = SetupUtilities.CreateList(currentWeb, GlobalConstants.LIST_GraphicsLogsListName, GlobalConstants.LIST_GraphicsLogsListName);
            }

            bool needsListUpdate = false;

            if (SetupUtilities.CreateField(splist, LogListNames.Category, LogListNames.Category, SPFieldType.Text, false))
            {
                needsListUpdate = true;
            }
            if (SetupUtilities.CreateField(splist, LogListNames.Message, LogListNames.Message, SPFieldType.Note, false))
            {
                needsListUpdate = true;
            }
            if (SetupUtilities.CreateField(splist, LogListNames.Form, LogListNames.Form, SPFieldType.Text, false))
            {
                needsListUpdate = true;
            }

            if (needsListUpdate)
                splist.Update();
        }
        #endregion

        #region OBM Brand Manager Lookup List
        private void CreateOBMBrandManagerLookupList()
        {
            if (currentWeb != null)
            {
                SPList splist = currentWeb.Lists.TryGetList(GlobalConstants.LIST_OBMBrandManagerLookupListName);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(currentWeb, GlobalConstants.LIST_OBMBrandManagerLookupListName, GlobalConstants.LIST_OBMBrandManagerLookupListName);
                }

                bool needsListUpdate = false;

                var globalLookupFields = defaultListEntryService.GetGlobalLookupFieldsData(XmlFileName.ProductHierarchyLevel1LookupData);
                var prodHierarchyLevel1Types = new List<string>();
                foreach (var lookupField in globalLookupFields)
                {
                    prodHierarchyLevel1Types.Add(lookupField.Title);
                }
                if (SetupUtilities.CreateFieldChoice(splist, OBMBrandManagerListFields.ProductHierarchyLevel1, OBMBrandManagerListFields.ProductHierarchyLevel1_DisplayName, false, prodHierarchyLevel1Types))
                {
                    needsListUpdate = true;
                }

                globalLookupFields = defaultListEntryService.GetGlobalLookupFieldsData(XmlFileName.MaterialGroup1LookupData);
                var materialGroup1Types = new List<string>();
                foreach (var lookupField in globalLookupFields)
                {
                    materialGroup1Types.Add(lookupField.Title);
                }
                if (SetupUtilities.CreateFieldChoice(splist, OBMBrandManagerListFields.MaterialGroup1Brand, OBMBrandManagerListFields.MaterialGroup1Brand_DisplayName, false, materialGroup1Types))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, OBMBrandManagerListFields.PM, OBMBrandManagerListFields.PM_DisplayName, SPFieldType.User, false))
                {
                    needsListUpdate = true;
                }

                if (SetupUtilities.CreateField(splist, OBMBrandManagerListFields.BrandManager, OBMBrandManagerListFields.BrandManager_DisplayName, SPFieldType.User, false))
                {
                    needsListUpdate = true;
                }

                if (needsListUpdate)
                    splist.Update();
            }
        }

        private void AttachOBMBrandManagerLookupListEventReceiver()
        {
            SPList wfList = currentWeb.Lists.TryGetList(GlobalConstants.LIST_OBMBrandManagerLookupListName);
            if (wfList != null)
            {
                DeleteEventReceiver(wfList);

                wfList.EventReceivers.Add(SPEventReceiverType.ItemUpdated,
                    GlobalConstants.SharePointServiceAssemblyName,
                    GlobalConstants.OBMBrandManagerLookupListEventReceiverClassName);

                wfList.EventReceivers.Add(SPEventReceiverType.ItemAdded,
                    GlobalConstants.SharePointServiceAssemblyName,
                    GlobalConstants.OBMBrandManagerLookupListEventReceiverClassName);

                wfList.EventReceivers.Add(SPEventReceiverType.ItemDeleting,
                    GlobalConstants.SharePointServiceAssemblyName,
                    GlobalConstants.OBMBrandManagerLookupListEventReceiverClassName);

                wfList.Update();
            }
        }

        private void PopulateOBMBrandManagerLookupData()
        {
            var list = currentWeb.Lists.TryGetList(GlobalConstants.LIST_OBMBrandManagerLookupListName);
            if (list == null) return;

            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Bulk, "ATOMIC FIREBALL (AF)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Bulk, "BOSTON B BEAN (BB)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Bulk, "BLACK FOREST (BF)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Bulk, "BOB'S (BO)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Bulk, "CHUCKLES (CH)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Bulk, "FARLEY'S (FA)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Bulk, "FERRARA (FC)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Bulk, "JAWBUSTER/JAWBREAKER (JB)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Bulk, "LEMONHEAD (LH)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Bulk, "MISC (MC)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Bulk, "NOW &amp; LATER (NL)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Bulk, "PRIVATE LABEL (PL)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Bulk, "REDHOT (RH)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Bulk, "SATHERS (SA)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Bulk, "SUPER BUBBLE (SE)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Bulk, "TROLLI (TR)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Bulk, "TREE TOP (TT)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Bulk, "BLACK FOREST GUMMI (BG)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Bulk, "DREAMWORKS (DW)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Bulk, "STONYFIELD (SF)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Bulk, "BLACK FOREST ORGANIC (OG)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Bulk, "NORTHERN LIGHTS (NO)");

            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Everyday, "ATOMIC FIREBALL (AF)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Everyday, "BOSTON B BEAN (BB)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Everyday, "BLACK FOREST (BF)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Everyday, "BOB'S (BO)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Everyday, "CHUCKLES (CH)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Everyday, "FARLEY'S (FA)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Everyday, "FERRARA (FC)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Everyday, "FRUIT STRIPE (FR)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Everyday, "JAWBUSTER/JAWBREAKER (JB)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Everyday, "JUJY FRUIT (JF)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Everyday, "LEMONHEAD (LH)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Everyday, "MISC (MC)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Everyday, "NOW &amp; LATER (NL)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Everyday, "RAINBLO (RA)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Everyday, "R.A.P. (RC)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Everyday, "REDHOT (RH)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Everyday, "SATHERS (SA)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Everyday, "SUPER BUBBLE (SE)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Everyday, "TROLLI (TR)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Everyday, "TREE TOP (TT)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Everyday, "BLACK FOREST GUMMI (BG)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Everyday, "DREAMWORKS (DW)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Everyday, "STONYFIELD (SF)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_Everyday, "BLACK FOREST ORGANIC (OG)");

            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_PrivateLabel, "PRIVATE LABEL (PL)");

            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_CoMan, "CONTRACT MANUFACTURING (CM)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_CoMan, "MISC (MC)");
            AddOBMBrandManagerLookupItemToList(list, GlobalConstants.PRODUCT_HIERARCHY1_CoMan, "PRIVATE LABEL (PL)");

            list.Update();
        }
        private static void AddOBMBrandManagerLookupItemToList(SPList list, string hierarchy, string brand)
        {
            foreach (SPListItem existingItem in list.Items)
            {
                if ((hierarchy.ToString() == existingItem[OBMBrandManagerListFields.ProductHierarchyLevel1].ToString()) &&
                    (brand.ToString() == existingItem[OBMBrandManagerListFields.MaterialGroup1Brand].ToString()))
                    return;
            }

            SPListItem spListItem = list.Items.Add();

            spListItem["Title"] = hierarchy.ToString();
            spListItem[OBMBrandManagerListFields.ProductHierarchyLevel1] = hierarchy.ToString();
            spListItem[OBMBrandManagerListFields.MaterialGroup1Brand] = brand.ToString();
            spListItem.Update();
        }

        #endregion

        #region Logs list
        private void CreateHolidayList()
        {
            try
            {
                string description = "Ferrara Holidays";

                SPList splist = currentWeb.Lists.TryGetList(GlobalConstants.LIST_HolidayLookup);

                //Creating the List if List does not exist 
                if (splist == null)
                {
                    splist = SetupUtilities.CreateList(currentWeb, GlobalConstants.LIST_HolidayLookup, description);
                    var field = splist.Fields[SPBuiltInFieldId.Title];
                    field.Title = HolidayListFields.Holiday_Display;
                    field.Update();
                }

                var needsListUpdate = false;
                if (SetupUtilities.CreateFieldDateTime(splist, HolidayListFields.HolidayDate, HolidayListFields.HolidayDate_Display, false, false))
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

        private void AttachHolidayListEventReceiver()
        {
            SPList wfList = currentWeb.Lists.TryGetList(GlobalConstants.LIST_HolidayLookup);
            if (wfList != null)
            {
                DeleteEventReceiver(wfList);

                wfList.EventReceivers.Add(SPEventReceiverType.ItemUpdated,
                    GlobalConstants.SharePointServiceAssemblyName,
                    GlobalConstants.WorkflowStepListEventReceiverClassName);

                wfList.EventReceivers.Add(SPEventReceiverType.ItemAdded,
                    GlobalConstants.SharePointServiceAssemblyName,
                    GlobalConstants.WorkflowStepListEventReceiverClassName);

                wfList.EventReceivers.Add(SPEventReceiverType.ItemDeleting,
                    GlobalConstants.SharePointServiceAssemblyName,
                    GlobalConstants.WorkflowStepListEventReceiverClassName);

                wfList.Update();
            }
        }
        #endregion
    }
}