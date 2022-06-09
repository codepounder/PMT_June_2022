using Microsoft.Practices.Unity;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Services;

namespace Ferrara.Compass.DependencyResolution
{
    public static class DependencyMapper
    {
        private static IUnityContainer container;
        private static ContainerControlledLifetimeManager SpContextlifeTimeManager;
        public static IUnityContainer Container
        {
            get
            {
                if (container == null)
                    RegisterContainer();
                return container;
            }
        }
        private static void RegisterContainer()
        {
            container = new UnityContainer();
            Abstractions.Unity.FerraraUnityContainer.Container = container;

            SpContextlifeTimeManager = new ContainerControlledLifetimeManager();
            container.RegisterType<IUtilityService, UtilityService>(SpContextlifeTimeManager);

            container.RegisterType<IConfigurationManagementService, ConfigurationManagementService>();
            container.RegisterType<ICacheManagementService, CacheManagementService>();
            container.RegisterType<IUserManagementService, UserManagementService>();
            container.RegisterType<IWorkflowService, WorkflowService>();
            container.RegisterType<IDefaultListEntryService, DefaultListEntryService>();
            container.RegisterType<IEmailTemplateService, EmailTemplateService>();
            container.RegisterType<IEmailService, EmailService>();
            container.RegisterType<INotificationService, NotificationService>();
            container.RegisterType<ILoggerService, LoggerService>();
            container.RegisterType<IExceptionService, ExceptionService>();
            container.RegisterType<IPDFService, PDFService>();
            container.RegisterType<IEmailLoggingService, EmailLoggingService>();
            container.RegisterType<IExcelService, ExcelService>();
            container.RegisterType<IVersionHistoryService, VersionHistoryService>();

            container.RegisterType<IProjectNotesService, ProjectNotesService>();
            container.RegisterType<IProjectHeaderService, ProjectHeaderService>();
            container.RegisterType<ISAPMaterialMasterService, SAPMaterialMasterService>();
            container.RegisterType<ISAPBOMService, SAPBOMService>();

            container.RegisterType<IStageGateCreateProjectService, StageGateCreateProjectService>();
            container.RegisterType<IItemProposalService, ItemProposalService>();
            container.RegisterType<IInitialApprovalReviewService, InitialApprovalReviewService>();
            container.RegisterType<IInitialCostingReviewService, InitialCostingReviewService>();
            container.RegisterType<IInitialCapacityReviewService, InitialCapacityReviewService>();
            container.RegisterType<ITradePromoGroupService, TradePromoGroupService>();
            container.RegisterType<IDistributionService, DistributionService>();
            container.RegisterType<IOPSService, OPSService>();
            container.RegisterType<IExternalManufacturingService, ExternalManufacturingService>();
            container.RegisterType<IQAService, QAService>();

            container.RegisterType<IBillOfMaterialsService, BillOfMaterialsService>();
            container.RegisterType<IPackagingItemService, PackagingItemService>();
            container.RegisterType<IShipperFinishedGoodService, ShipperFinishedGoodService>();
            container.RegisterType<IFinalCostingService, FinalCostingService>();
            container.RegisterType<ICommercializationService, CommercializationService>();
            container.RegisterType<IApprovalService, ApprovalService>();
            container.RegisterType<IReportService, ReportService>();
            container.RegisterType<IUserAssignmentService, UserAssignmentService>();
            container.RegisterType<IOBMFirstReviewService, OBMFirstReviewService>();
            container.RegisterType<IGraphicsService, GraphicsService>();
            container.RegisterType<ISAPInitialItemSetUpService, SAPInitialItemSetUpService>();
            container.RegisterType<IMixesService, MixesService>();
            container.RegisterType<ISAPBOMSetupService, SAPBOMSetupService>();
            container.RegisterType<IProjectTimelineTypeService, ProjectTimelineTypeService>();
            container.RegisterType<IProjectTimelineUpdateService, ProjectTimelineUpdateService>();
            container.RegisterType<ISecondaryApprovalReviewService, SecondaryApprovalReviewService>();
            container.RegisterType<IOBMSecondReviewService, OBMSecondReviewService>();
            container.RegisterType<IComponentCostingQuoteService, ComponentCostingQuoteService>();
            container.RegisterType<IUpdateNotesService, UpdateNotesService>();
            container.RegisterType<IFinishedGoodPackSpecService, FinishedGoodPackSpecService>();
            container.RegisterType<IWorldSyncNutritionalService, WorldSyncNutritionalService>();
            container.RegisterType<ICompassWorldSyncService, CompassWorldSyncService>();
            container.RegisterType<IMaterialsReceivedService, MaterialsReceivedService>();
            container.RegisterType<IExcelExportSyncService, ExcelExportSyncService>();
            container.RegisterType<IDashboardService, DashboardService>();
            container.RegisterType<IWorldSyncRequestService, WorldSyncRequestService>();
            container.RegisterType<ISAPFinalRoutingsService, SAPFinalRoutingsService>();
            container.RegisterType<IStageGateGeneralService, StageGateGeneralService>();
            container.RegisterType<IStageGateFinancialServices, StageGateFinancialServices>();
            container.RegisterType<ISAPCompleteItemSetupService, SAPCompleteItemSetupService>();
            container.RegisterType<IBOMSetupMaterialWarehouseService, BOMSetupMaterialWarehouseService>();
            container.RegisterType<IBOMSetupService, BOMSetupService>();
            container.RegisterType<IBEQRCService, BEQRCService>();
        }
    }
}

