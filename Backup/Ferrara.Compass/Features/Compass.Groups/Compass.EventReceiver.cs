using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Constants;
using System.Collections.Generic;
using System.Linq;
using Ferrara.Compass.Classes;

namespace Ferrara.Compass.Features.Compass.Groups
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("90f9ec92-c0ea-4161-b814-728058a7cf92")]
    public class CompassEventReceiver : SPFeatureReceiver
    {
        SPWeb web;

        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            web = (SPWeb)properties.Feature.Parent;
            //DeleteAllGroups();
            CreateUserGroups();
            //AddUsersToAllGroups();
        }
        private void DeleteAllGroups()
        {
            try
            {
                foreach (SPGroup group in web.SiteGroups) {
                    try { 
                        web.SiteGroups.RemoveByID(group.ID);
                    }
                    catch (Exception e) { LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "DeleteAllGroups1: " + group.Name + ": " + e.Message); }
                }
            }
            catch (Exception e) { LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "DeleteAllGroups2: " + e.Message); }
        }
        private void CreateUserGroups()
        {
            var permissions = new List<string> { GlobalConstants.ACCESS_Read };

            CreateUserGroup(GlobalConstants.GROUP_Marketing, permissions);
            CreateUserGroup(GlobalConstants.GROUP_InitialCapacity, permissions);
            CreateUserGroup(GlobalConstants.GROUP_InitialCosting, permissions);
            CreateUserGroup(GlobalConstants.GROUP_InternationalCompliance, permissions);
            CreateUserGroup(GlobalConstants.GROUP_FinalCosting, permissions);
            CreateUserGroup(GlobalConstants.GROUP_TradePromo, permissions);
            CreateUserGroup(GlobalConstants.GROUP_EstimatedPricing, permissions);
            CreateUserGroup(GlobalConstants.GROUP_EstimatedBracketPricing, permissions);
            CreateUserGroup(GlobalConstants.GROUP_DemandPlanning, permissions);
            CreateUserGroup(GlobalConstants.GROUP_Distribution, permissions);
            CreateUserGroup(GlobalConstants.GROUP_ExternalManufacturing, permissions);
            CreateUserGroup(GlobalConstants.GROUP_Graphics, permissions);
            CreateUserGroup(GlobalConstants.GROUP_GraphicsTask, permissions);
            CreateUserGroup(GlobalConstants.GROUP_Operations, permissions);
            CreateUserGroup(GlobalConstants.GROUP_ProjectManagers, permissions);
            CreateUserGroup(GlobalConstants.GROUP_SeniorProjectManager, permissions);
            CreateUserGroup(GlobalConstants.GROUP_PackagingEngineer, permissions);
            CreateUserGroup(GlobalConstants.GROUP_ProcurementCoManufacturingMembers, permissions);
            CreateUserGroup(GlobalConstants.GROUP_ProcurementPackaging, permissions);
            CreateUserGroup(GlobalConstants.GROUP_QualityAssurance, permissions);
            CreateUserGroup(GlobalConstants.GROUP_InTech, permissions);
            CreateUserGroup(GlobalConstants.GROUP_MasterData, permissions); 
            CreateUserGroup(GlobalConstants.GROUP_MaterialWarehouse, permissions); 
             CreateUserGroup(GlobalConstants.GROUP_InnovationReview, permissions);
            CreateUserGroup(GlobalConstants.GROUP_TradeSpending, permissions);
            CreateUserGroup(GlobalConstants.GROUP_SalesPlanning, permissions);

            CreateUserGroup(GlobalConstants.GROUP_Obsolescence, permissions);
            CreateUserGroup(GlobalConstants.GROUP_CustomerService, permissions);
            CreateUserGroup(GlobalConstants.GROUP_PlantForestPark, permissions);
            CreateUserGroup(GlobalConstants.GROUP_PlantBellwood, permissions);
            CreateUserGroup(GlobalConstants.GROUP_PlantPackCenter, permissions);
            CreateUserGroup(GlobalConstants.GROUP_PlantShipLab, permissions);
            CreateUserGroup(GlobalConstants.GROUP_PlantVernell, permissions);
            CreateUserGroup(GlobalConstants.GROUP_PlantReynosa, permissions);

            CreateUserGroup(GlobalConstants.GROUP_PurchasingSeasonal, permissions);
            CreateUserGroup(GlobalConstants.GROUP_PurchasingFilm, permissions);
            CreateUserGroup(GlobalConstants.GROUP_PurchasingCorrugated, permissions);
            CreateUserGroup(GlobalConstants.GROUP_PurchasingOther, permissions);
            CreateUserGroup(GlobalConstants.GROUP_WorldSync, permissions);           

            CreateUserGroup(GlobalConstants.GROUP_IPFSubmissionMembers, permissions);

            CreateUserGroup(GlobalConstants.GROUP_ProjectCancellationMembers, permissions);

            CreateUserGroup(GlobalConstants.GROUP_Developers, permissions);
            CreateUserGroup(GlobalConstants.GROUP_QualityInnovation, permissions);
            CreateUserGroup(GlobalConstants.GROUP_InTechRegulatory, permissions);
            CreateUserGroup(GlobalConstants.GROUP_Sales, permissions);
            CreateUserGroup(GlobalConstants.GROUP_Manufacturing, permissions);
            CreateUserGroup(GlobalConstants.GROUP_SupplyChain, permissions);
            
            CreateUserGroup(GlobalConstants.GROUP_Legal, permissions);
            CreateUserGroup(GlobalConstants.GROUP_LifeCycleMngmt, permissions);
            CreateUserGroup(GlobalConstants.GROUP_NewFGWithReplacementItemMembers, permissions);

            #region Procurement Groups
            CreateUserGroup(GlobalConstants.GROUP_ProcurementCatchAll, permissions);
            CreateUserGroup(GlobalConstants.GROUP_ProcurementCoMan, permissions);
            CreateUserGroup(GlobalConstants.GROUP_ProcurementEBPAncillary, permissions);
            CreateUserGroup(GlobalConstants.GROUP_ProcurementEBPCorrugated, permissions);
            CreateUserGroup(GlobalConstants.GROUP_ProcurementEBPFilm, permissions);
            CreateUserGroup(GlobalConstants.GROUP_ProcurementEBPLabel, permissions);
            CreateUserGroup(GlobalConstants.GROUP_ProcurementEBPMetal, permissions);
            CreateUserGroup(GlobalConstants.GROUP_ProcurementEBPOther, permissions);
            CreateUserGroup(GlobalConstants.GROUP_ProcurementEBPPaperboard, permissions);
            CreateUserGroup(GlobalConstants.GROUP_ProcurementEBPPurchased, permissions);
            CreateUserGroup(GlobalConstants.GROUP_ProcurementEBPRigidPlastic, permissions);
            CreateUserGroup(GlobalConstants.GROUP_ProcurementNovelty, permissions);
            CreateUserGroup(GlobalConstants.GROUP_ProcurementSeasonal, permissions);
            #endregion

            permissions = new List<string> { GlobalConstants.ACCESS_Contribute };
            CreateUserGroup(GlobalConstants.GROUP_OBMAdmins, permissions);
            CreateUserGroup(GlobalConstants.GROUP_WorldSyncGraphics, permissions);
            CreateUserGroup(GlobalConstants.GROUP_ConsumerRelations, permissions);
            CreateUserGroup(GlobalConstants.GROUP_PostLaunchNotificationRnDMembers, permissions);
            CreateUserGroup(GlobalConstants.GROUP_NewPackagingComponentsCreated, permissions);
        }

        private void AddUsersToAllGroups()
        {
            try
            {
                foreach (SPGroup group in web.SiteGroups)
                {
                    try
                    {
                        SPUser spUser = web.EnsureUser(web.CurrentUser.Name);
                        group.AddUser(spUser);
                        web.Update();
                    }
                    catch (Exception e) { LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "AddUsersToAllGroups1: "+group.Name + ": " + e.Message); }
                }
            }
            catch (Exception e) { LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "AddUsersToAllGroups2" + ": " + e.Message); }
        }
        private bool GroupExistsInWebSite(string name)
        {
            return web.AssociatedGroups.OfType<SPGroup>().Count(g => g.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)) > 0;
        }

        private void CreateUserGroup(string groupName, List<string> permissions)
        {
            if (GroupExistsInWebSite(groupName)) return;
            try
            {
                //Add the group to the SPWeb web                    
                web.SiteGroups.Add(groupName, web.AssociatedOwnerGroup, null, groupName);

                var siteGroup = web.SiteGroups[groupName];
                //Associate the group with SPWeb
                web.AssociatedGroups.Add(siteGroup);
                web.Update();

                //Assignment of the roles to the group.
                var assignment = new SPRoleAssignment(siteGroup);

                foreach (var permission in permissions)
                {
                    var role = web.RoleDefinitions[permission];
                    assignment.RoleDefinitionBindings.Add(role);
                    web.RoleAssignments.Add(assignment);
                }
            }
            catch (Exception ex)
            {

            }
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

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
