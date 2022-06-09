using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ferrara.Compass.Services
{
    public class StageGateCreateProjectService : IStageGateCreateProjectService
    {
        #region Member Variables
        private readonly IExceptionService exceptionService;
        private readonly IWorkflowService workflowServices;
        #endregion
        #region Constructor
        public StageGateCreateProjectService(IExceptionService exceptionService, IWorkflowService workflowServices)
        {
            this.exceptionService = exceptionService;
            this.workflowServices = workflowServices;
        }
        #endregion
        public StageGateCreateProjectItem GetStageGateProjectItem(int itemId)
        {
            var newItem = new StageGateCreateProjectItem();
            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);
                    var item = spList.GetItemById(itemId);
                    if (item != null)
                    {
                        newItem.StageGateProjectListItemId = item.ID;
                        newItem.ProjectNumber = Convert.ToString(item[StageGateProjectListFields.ProjectNumber]);
                        newItem.LineOfBisiness = Convert.ToString(item[StageGateProjectListFields.LineOfBusiness]);
                        newItem.ProjectTier = Convert.ToString(item[StageGateProjectListFields.ProjectTier]);
                        newItem.NumberofNoveltySKUs = Convert.ToString(item[StageGateProjectListFields.NumberofNoveltySKUs]);
                        newItem.DesiredShipDate = Convert.ToDateTime(item[StageGateProjectListFields.DesiredShipDate]);
                        newItem.RevisedShipDate = Convert.ToDateTime(item[StageGateProjectListFields.RevisedShipDate]);
                        newItem.ProjectName = Convert.ToString(item[StageGateProjectListFields.ProjectName]);
                        newItem.Gate0ApprovedDate = Convert.ToDateTime(item[StageGateProjectListFields.Gate0ApprovedDate]);

                        //Project Information
                        newItem.PHL2 = Convert.ToString(item[StageGateProjectListFields.ProductHierarchyL2]);
                        newItem.Brand = Convert.ToString(item[StageGateProjectListFields.Brand]);
                        newItem.SKUs = Convert.ToString(item[StageGateProjectListFields.SKUs]);
                        newItem.ProjectType = Convert.ToString(item[StageGateProjectListFields.ProjectType]);
                        newItem.ProjectTypeSubCategory = Convert.ToString(item[StageGateProjectListFields.ProjectTypeSubCategory]);
                        newItem.BusinessFunction = Convert.ToString(item[StageGateProjectListFields.BusinessFunction]);
                        newItem.BusinessFunctionOther = Convert.ToString(item[StageGateProjectListFields.BusinessFunctionOther]);
                        newItem.NewFinishedGood = Convert.ToString(item[StageGateProjectListFields.NewFinishedGood]);
                        newItem.NewBaseFormula = Convert.ToString(item[StageGateProjectListFields.NewBaseFormula]);
                        newItem.NewShape = Convert.ToString(item[StageGateProjectListFields.NewShape]);
                        newItem.NewPackType = Convert.ToString(item[StageGateProjectListFields.NewPackType]);
                        newItem.NewNetWeight = Convert.ToString(item[StageGateProjectListFields.NewNetWeight]);
                        newItem.NewGraphics = Convert.ToString(item[StageGateProjectListFields.NewGraphics]);
                        newItem.NewFlavorColor = Convert.ToString(item[StageGateProjectListFields.NewFlavorColor]);
                        newItem.ProjectConceptOverview = Convert.ToString(item[StageGateProjectListFields.ProjectConceptOverview]);
                        //Stage
                        newItem.Stage = Convert.ToString(item[StageGateProjectListFields.Stage]);
                        newItem.TotalOnHoldDays = Convert.ToInt16(item[StageGateProjectListFields.TotalOnHoldDays]);
                        newItem.OnHoldStartDate = Convert.ToDateTime(item[StageGateProjectListFields.OnHoldStartDate]);
                        newItem.PostLaunchActive = Convert.ToString(item[StageGateProjectListFields.PostLaunchActive]);
                        //Project Team
                        newItem.ProjectLeader = Convert.ToString(item[StageGateProjectListFields.ProjectLeader]);
                        newItem.ProjectLeaderName = Convert.ToString(item[StageGateProjectListFields.ProjectLeaderName]);
                        newItem.ProjectManager = Convert.ToString(item[StageGateProjectListFields.ProjectManager]);
                        newItem.ProjectManagerName = Convert.ToString(item[StageGateProjectListFields.ProjectManagerName]);
                        newItem.SeniorProjectManager = Convert.ToString(item[StageGateProjectListFields.SeniorProjectManager]);
                        newItem.SeniorProjectManagerName = Convert.ToString(item[StageGateProjectListFields.SeniorProjectManagerName]);
                        newItem.Marketing = Convert.ToString(item[StageGateProjectListFields.Marketing]);
                        newItem.MarketingName = Convert.ToString(item[StageGateProjectListFields.MarketingName]);
                        newItem.InTech = Convert.ToString(item[StageGateProjectListFields.InTech]);
                        newItem.InTechName = Convert.ToString(item[StageGateProjectListFields.InTechName]);
                        newItem.QAInnovation = Convert.ToString(item[StageGateProjectListFields.QAInnovation]);
                        newItem.QAInnovationName = Convert.ToString(item[StageGateProjectListFields.QAInnovationName]);
                        newItem.InTechRegulatory = Convert.ToString(item[StageGateProjectListFields.InTechRegulatory]);
                        newItem.InTechRegulatoryName = Convert.ToString(item[StageGateProjectListFields.InTechRegulatoryName]);
                        newItem.RegulatoryQA = Convert.ToString(item[StageGateProjectListFields.RegulatoryQA]);
                        newItem.RegulatoryQAName = Convert.ToString(item[StageGateProjectListFields.RegulatoryQAName]);
                        newItem.PackagingEngineering = Convert.ToString(item[StageGateProjectListFields.PackagingEngineering]);
                        newItem.PackagingEngineeringName = Convert.ToString(item[StageGateProjectListFields.PackagingEngineeringName]);
                        newItem.SupplyChain = Convert.ToString(item[StageGateProjectListFields.SupplyChain]);
                        newItem.SupplyChainName = Convert.ToString(item[StageGateProjectListFields.SupplyChainName]);
                        newItem.Finance = Convert.ToString(item[StageGateProjectListFields.Finance]);
                        newItem.FinanceName = Convert.ToString(item[StageGateProjectListFields.FinanceName]);
                        newItem.Sales = Convert.ToString(item[StageGateProjectListFields.Sales]);
                        newItem.SalesName = Convert.ToString(item[StageGateProjectListFields.SalesName]);
                        newItem.Manufacturing = Convert.ToString(item[StageGateProjectListFields.Manufacturing]);
                        newItem.ManufacturingName = Convert.ToString(item[StageGateProjectListFields.ManufacturingName]);
                        newItem.TeamMembers = Convert.ToString(item[StageGateProjectListFields.TeamMembers]);
                        newItem.TeamMembersNames = Convert.ToString(item[StageGateProjectListFields.TeamMembersNames]);
                        newItem.ExtMfgProcurement = Convert.ToString(item[StageGateProjectListFields.ExtMfgProcurement]);
                        newItem.ExtMfgProcurementName = Convert.ToString(item[StageGateProjectListFields.ExtMfgProcurementName]);
                        newItem.PackagingProcurement = Convert.ToString(item[StageGateProjectListFields.PackagingProcurement]);
                        newItem.PackagingProcurementName = Convert.ToString(item[StageGateProjectListFields.PackagingProcurementName]);
                        newItem.LifeCycleManagement = Convert.ToString(item[StageGateProjectListFields.LifeCycleManagement]);
                        newItem.LifeCycleManagementName = Convert.ToString(item[StageGateProjectListFields.LifeCycleManagementName]);
                        newItem.Legal = Convert.ToString(item[StageGateProjectListFields.Legal]);
                        newItem.LegalName = Convert.ToString(item[StageGateProjectListFields.LegalName]);
                        newItem.OtherMember = Convert.ToString(item[StageGateProjectListFields.OtherMember]);
                        newItem.OtherMemberName = Convert.ToString(item[StageGateProjectListFields.OtherMemberName]);
                        newItem.IPFStartDate = Convert.ToString(item[StageGateProjectListFields.IPFStartDate]);
                        newItem.IPFSubmitter = Convert.ToString(item[StageGateProjectListFields.IPFSubmitter]);

                        //Email Sent Status
                        newItem.ProjectSubmittedSent = Convert.ToString(item[StageGateProjectListFields.ProjectSubmittedSent]);
                        newItem.ProjectCacnelledSent = Convert.ToString(item[StageGateProjectListFields.ProjectCancelledSent]);
                        newItem.ProjectCompletedSent = Convert.ToString(item[StageGateProjectListFields.ProjectCompletedSent]);

                        //Test Project
                        newItem.TestProject = Convert.ToString(item[StageGateProjectListFields.TestProject]);

                        newItem.FormSubmittedDate = Convert.ToDateTime(item[StageGateProjectListFields.FormSubmittedDate]);
                        newItem.ModifiedDate = Convert.ToDateTime(item[StageGateProjectListFields.ModifiedDate]);
                        newItem.FormSubmittedBy = Convert.ToString(item[StageGateProjectListFields.FormSubmittedBy]);
                        newItem.ModifiedBy = Convert.ToString(item[StageGateProjectListFields.ModifiedBy]);
                        newItem.CreatedDate = Convert.ToDateTime(item["Created"]);
                    }
                }
            }
            return newItem;
        }
        public bool CheckOnHoldChildProjects(int itemId)
        {
            bool CheckOnHoldChildProjects = false;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"StageGateProjectListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem item2 in compassItemCol)
                            {
                                if (item2 != null)
                                {
                                    string wfPhase = Convert.ToString(item2[CompassListFields.WorkflowPhase]);

                                    if (wfPhase.ToLower() == "on hold")
                                    {
                                        CheckOnHoldChildProjects = true;
                                        break;
                                    }
                                }
                            }
                        }

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return CheckOnHoldChildProjects;
        }
        public int InsertStageGateProjectItem(StageGateCreateProjectItem sgitem, bool submitted)
        {
            // Get the current user
            SPUser currentUser = SPContext.Current.Web.CurrentUser;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);

                        var item = spList.AddItem();

                        item["Title"] = sgitem.ProjectNumber;

                        //Project Information
                        item[StageGateProjectListFields.ProjectNumber] = sgitem.ProjectNumber;
                        item[StageGateProjectListFields.ProjectName] = sgitem.ProjectName;
                        if ((sgitem.Gate0ApprovedDate != null) && (sgitem.Gate0ApprovedDate != DateTime.MinValue))
                        {
                            item[StageGateProjectListFields.Gate0ApprovedDate] = sgitem.Gate0ApprovedDate;
                        }
                        item[StageGateProjectListFields.LineOfBusiness] = sgitem.LineOfBisiness;
                        item[StageGateProjectListFields.ProjectTier] = sgitem.ProjectTier;
                        item[StageGateProjectListFields.NumberofNoveltySKUs] = sgitem.NumberofNoveltySKUs;
                        if ((sgitem.DesiredShipDate != null) && (sgitem.DesiredShipDate != DateTime.MinValue))
                        {
                            item[StageGateProjectListFields.DesiredShipDate] = sgitem.DesiredShipDate;
                        }
                        if ((sgitem.RevisedShipDate != null) && (sgitem.RevisedShipDate != DateTime.MinValue))
                        {
                            item[StageGateProjectListFields.RevisedShipDate] = sgitem.RevisedShipDate;
                        }
                        item[StageGateProjectListFields.ProductHierarchyL2] = sgitem.PHL2;
                        item[StageGateProjectListFields.Brand] = sgitem.Brand;
                        item[StageGateProjectListFields.SKUs] = sgitem.SKUs;
                        item[StageGateProjectListFields.ProjectType] = sgitem.ProjectType;
                        item[StageGateProjectListFields.ProjectTypeSubCategory] = sgitem.ProjectTypeSubCategory;
                        item[StageGateProjectListFields.BusinessFunction] = sgitem.BusinessFunction;
                        item[StageGateProjectListFields.NewFinishedGood] = sgitem.NewFinishedGood;
                        item[StageGateProjectListFields.NewBaseFormula] = sgitem.NewBaseFormula;
                        item[StageGateProjectListFields.NewShape] = sgitem.NewShape;
                        item[StageGateProjectListFields.NewPackType] = sgitem.NewPackType;
                        item[StageGateProjectListFields.NewNetWeight] = sgitem.NewNetWeight;
                        item[StageGateProjectListFields.NewGraphics] = sgitem.NewGraphics;
                        item[StageGateProjectListFields.NewFlavorColor] = sgitem.NewFlavorColor;
                        item[StageGateProjectListFields.ProjectConceptOverview] = sgitem.ProjectConceptOverview;
                        //Stage
                        item[StageGateProjectListFields.Stage] = sgitem.Stage;

                        //Project Team
                        item[StageGateProjectListFields.ProjectLeader] = sgitem.ProjectLeader;
                        item[StageGateProjectListFields.ProjectLeaderName] = sgitem.ProjectLeaderName;
                        item[StageGateProjectListFields.ProjectManager] = sgitem.ProjectManager;
                        item[StageGateProjectListFields.ProjectManagerName] = sgitem.ProjectManagerName;
                        item[StageGateProjectListFields.SeniorProjectManager] = sgitem.SeniorProjectManager;
                        item[StageGateProjectListFields.SeniorProjectManagerName] = sgitem.SeniorProjectManagerName;
                        item[StageGateProjectListFields.Marketing] = sgitem.Marketing;
                        item[StageGateProjectListFields.MarketingName] = sgitem.MarketingName;
                        item[StageGateProjectListFields.InTech] = sgitem.InTech;
                        item[StageGateProjectListFields.InTechName] = sgitem.InTechName;
                        item[StageGateProjectListFields.QAInnovation] = sgitem.QAInnovation;
                        item[StageGateProjectListFields.QAInnovationName] = sgitem.QAInnovationName;
                        item[StageGateProjectListFields.InTechRegulatory] = sgitem.InTechRegulatory;
                        item[StageGateProjectListFields.InTechRegulatoryName] = sgitem.InTechRegulatoryName;
                        item[StageGateProjectListFields.RegulatoryQA] = sgitem.RegulatoryQA;
                        item[StageGateProjectListFields.RegulatoryQAName] = sgitem.RegulatoryQAName;
                        item[StageGateProjectListFields.PackagingEngineering] = sgitem.PackagingEngineering;
                        item[StageGateProjectListFields.PackagingEngineeringName] = sgitem.PackagingEngineeringName;
                        item[StageGateProjectListFields.SupplyChain] = sgitem.SupplyChain;
                        item[StageGateProjectListFields.SupplyChainName] = sgitem.SupplyChainName;
                        item[StageGateProjectListFields.Finance] = sgitem.Finance;
                        item[StageGateProjectListFields.FinanceName] = sgitem.FinanceName;
                        item[StageGateProjectListFields.Sales] = sgitem.Sales;
                        item[StageGateProjectListFields.SalesName] = sgitem.SalesName;
                        item[StageGateProjectListFields.Manufacturing] = sgitem.Manufacturing;
                        item[StageGateProjectListFields.ManufacturingName] = sgitem.ManufacturingName;
                        item[StageGateProjectListFields.TeamMembers] = sgitem.TeamMembers;
                        item[StageGateProjectListFields.TeamMembersNames] = sgitem.TeamMembersNames;
                        item[StageGateProjectListFields.ExtMfgProcurement] = sgitem.ExtMfgProcurement;
                        item[StageGateProjectListFields.ExtMfgProcurementName] = sgitem.ExtMfgProcurementName;
                        item[StageGateProjectListFields.PackagingProcurement] = sgitem.PackagingProcurement;
                        item[StageGateProjectListFields.PackagingProcurementName] = sgitem.PackagingProcurementName;
                        item[StageGateProjectListFields.LifeCycleManagement] = sgitem.LifeCycleManagement;
                        item[StageGateProjectListFields.LifeCycleManagementName] = sgitem.LifeCycleManagementName;
                        item[StageGateProjectListFields.Legal] = sgitem.Legal;
                        item[StageGateProjectListFields.LegalName] = sgitem.LegalName;
                        item[StageGateProjectListFields.OtherMember] = sgitem.OtherMember;
                        item[StageGateProjectListFields.OtherMemberName] = sgitem.OtherMemberName;
                        item[StageGateProjectListFields.AllProjectUsers] = sgitem.AllUsers;

                        item["Editor"] = SPContext.Current.Web.CurrentUser;
                        item[StageGateProjectListFields.LastUpdatedFormName] = sgitem.LastUpdatedFormName;

                        if (submitted)
                        {
                            item[StageGateProjectListFields.FormSubmittedDate] = DateTime.Now.ToString();
                            item[StageGateProjectListFields.FormSubmittedBy] = SPContext.Current.Web.CurrentUser;
                        }
                        else
                        {
                            item[StageGateProjectListFields.ModifiedDate] = DateTime.Now.ToString();
                            item[StageGateProjectListFields.ModifiedBy] = SPContext.Current.Web.CurrentUser;
                        }
                        item[StageGateProjectListFields.LastUpdatedFormName] = sgitem.LastUpdatedFormName;

                        item.Update();

                        sgitem.StageGateProjectListItemId = item.ID;
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return sgitem.StageGateProjectListItemId;
        }
        public int UpdateStageGateProjectItem(StageGateCreateProjectItem sgitem, bool submitted)
        {
            //Get the current user
            SPUser currentUser = SPContext.Current.Web.CurrentUser;
            int id = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);
                        if (sgitem.StageGateProjectListItemId != 0)
                        {
                            var item = spList.GetItemById(sgitem.StageGateProjectListItemId);
                            item["Title"] = sgitem.ProjectNumber;

                            //Project Information
                            item[StageGateProjectListFields.ProjectNumber] = sgitem.ProjectNumber;
                            item[StageGateProjectListFields.ProjectName] = sgitem.ProjectName;
                            item[StageGateProjectListFields.LineOfBusiness] = sgitem.LineOfBisiness;
                            item[StageGateProjectListFields.ProjectTier] = sgitem.ProjectTier;
                            item[StageGateProjectListFields.NumberofNoveltySKUs] = sgitem.NumberofNoveltySKUs;
                            if ((sgitem.Gate0ApprovedDate != null) && (sgitem.Gate0ApprovedDate != DateTime.MinValue))
                            {
                                item[StageGateProjectListFields.Gate0ApprovedDate] = sgitem.Gate0ApprovedDate;
                            }
                            if ((sgitem.DesiredShipDate != null) && (sgitem.DesiredShipDate != DateTime.MinValue))
                            {
                                item[StageGateProjectListFields.DesiredShipDate] = sgitem.DesiredShipDate;
                            }

                            item[StageGateProjectListFields.Brand] = sgitem.Brand;
                            item[StageGateProjectListFields.ProductHierarchyL2] = sgitem.PHL2;
                            item[StageGateProjectListFields.SKUs] = sgitem.SKUs;
                            item[StageGateProjectListFields.ProjectType] = sgitem.ProjectType;
                            item[StageGateProjectListFields.ProjectTypeSubCategory] = sgitem.ProjectTypeSubCategory;
                            item[StageGateProjectListFields.BusinessFunction] = sgitem.BusinessFunction;
                            item[StageGateProjectListFields.BusinessFunctionOther] = sgitem.BusinessFunctionOther;
                            item[StageGateProjectListFields.NewFinishedGood] = sgitem.NewFinishedGood;
                            item[StageGateProjectListFields.NewBaseFormula] = sgitem.NewBaseFormula;
                            item[StageGateProjectListFields.NewShape] = sgitem.NewShape;
                            item[StageGateProjectListFields.NewPackType] = sgitem.NewPackType;
                            item[StageGateProjectListFields.NewNetWeight] = sgitem.NewNetWeight;
                            item[StageGateProjectListFields.NewGraphics] = sgitem.NewGraphics;
                            item[StageGateProjectListFields.NewFlavorColor] = sgitem.NewFlavorColor;
                            item[StageGateProjectListFields.ProjectConceptOverview] = sgitem.ProjectConceptOverview;
                            //Stage
                            item[StageGateProjectListFields.Stage] = sgitem.Stage;

                            //Project Team
                            item[StageGateProjectListFields.ProjectLeader] = sgitem.ProjectLeader;
                            item[StageGateProjectListFields.ProjectLeaderName] = sgitem.ProjectLeaderName;
                            item[StageGateProjectListFields.ProjectManager] = sgitem.ProjectManager;
                            item[StageGateProjectListFields.ProjectManagerName] = sgitem.ProjectManagerName;
                            item[StageGateProjectListFields.SeniorProjectManager] = sgitem.SeniorProjectManager;
                            item[StageGateProjectListFields.SeniorProjectManagerName] = sgitem.SeniorProjectManagerName;
                            item[StageGateProjectListFields.Marketing] = sgitem.Marketing;
                            item[StageGateProjectListFields.MarketingName] = sgitem.MarketingName;
                            item[StageGateProjectListFields.InTech] = sgitem.InTech;
                            item[StageGateProjectListFields.InTechName] = sgitem.InTechName;
                            item[StageGateProjectListFields.QAInnovation] = sgitem.QAInnovation;
                            item[StageGateProjectListFields.QAInnovationName] = sgitem.QAInnovationName;
                            item[StageGateProjectListFields.InTechRegulatory] = sgitem.InTechRegulatory;
                            item[StageGateProjectListFields.InTechRegulatoryName] = sgitem.InTechRegulatoryName;
                            item[StageGateProjectListFields.RegulatoryQA] = sgitem.RegulatoryQA;
                            item[StageGateProjectListFields.RegulatoryQAName] = sgitem.RegulatoryQAName;
                            item[StageGateProjectListFields.PackagingEngineering] = sgitem.PackagingEngineering;
                            item[StageGateProjectListFields.PackagingEngineeringName] = sgitem.PackagingEngineeringName;
                            item[StageGateProjectListFields.SupplyChain] = sgitem.SupplyChain;
                            item[StageGateProjectListFields.SupplyChainName] = sgitem.SupplyChainName;
                            item[StageGateProjectListFields.Finance] = sgitem.Finance;
                            item[StageGateProjectListFields.FinanceName] = sgitem.FinanceName;
                            item[StageGateProjectListFields.Sales] = sgitem.Sales;
                            item[StageGateProjectListFields.SalesName] = sgitem.SalesName;
                            item[StageGateProjectListFields.Manufacturing] = sgitem.Manufacturing;
                            item[StageGateProjectListFields.ManufacturingName] = sgitem.ManufacturingName;
                            item[StageGateProjectListFields.TeamMembers] = sgitem.TeamMembers;
                            item[StageGateProjectListFields.TeamMembersNames] = sgitem.TeamMembersNames;
                            item[StageGateProjectListFields.ExtMfgProcurement] = sgitem.ExtMfgProcurement;
                            item[StageGateProjectListFields.ExtMfgProcurementName] = sgitem.ExtMfgProcurementName;
                            item[StageGateProjectListFields.PackagingProcurement] = sgitem.PackagingProcurement;
                            item[StageGateProjectListFields.PackagingProcurementName] = sgitem.PackagingProcurementName;
                            item[StageGateProjectListFields.LifeCycleManagement] = sgitem.LifeCycleManagement;
                            item[StageGateProjectListFields.LifeCycleManagementName] = sgitem.LifeCycleManagementName;
                            item[StageGateProjectListFields.Legal] = sgitem.Legal;
                            item[StageGateProjectListFields.LegalName] = sgitem.LegalName;
                            item[StageGateProjectListFields.OtherMember] = sgitem.OtherMember;
                            item[StageGateProjectListFields.OtherMemberName] = sgitem.OtherMemberName;
                            item[StageGateProjectListFields.AllProjectUsers] = sgitem.AllUsers;

                            if (submitted)
                            {
                                item[StageGateProjectListFields.FormSubmittedDate] = DateTime.Now.ToString();
                                item[StageGateProjectListFields.FormSubmittedBy] = SPContext.Current.Web.CurrentUser;
                            }
                            else
                            {
                                item[StageGateProjectListFields.ModifiedDate] = DateTime.Now.ToString();
                                item[StageGateProjectListFields.ModifiedBy] = SPContext.Current.Web.CurrentUser;
                            }

                            item[StageGateProjectListFields.LastUpdatedFormName] = CompassForm.StageGateCreateProject.ToString();

                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            item[StageGateProjectListFields.LastUpdatedFormName] = sgitem.LastUpdatedFormName;

                            item.Update();
                            spWeb.AllowUnsafeUpdates = false;
                            id = sgitem.StageGateProjectListItemId;
                        }
                        else
                        {
                            id = InsertStageGateProjectItem(sgitem, submitted);
                        }
                    }
                }
            });
            return id;
        }
        public int UpdateStageGateProjectSubmittedEmailSent(StageGateCreateProjectItem sgitem)
        {
            int id = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);
                        if (sgitem.StageGateProjectListItemId != 0)
                        {
                            var item = spList.GetItemById(sgitem.StageGateProjectListItemId);
                            item["Title"] = sgitem.ProjectNumber;

                            //Project Information
                            item[StageGateProjectListFields.ProjectSubmittedSent] = sgitem.ProjectSubmittedSent;

                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            item[StageGateProjectListFields.LastUpdatedFormName] = sgitem.LastUpdatedFormName;

                            item.Update();
                            spWeb.AllowUnsafeUpdates = false;
                            id = sgitem.StageGateProjectListItemId;
                        }
                    }
                }
            });
            return id;
        }
        public int UpdateStageGateProjectCancelledEmailSent(StageGateCreateProjectItem sgitem)
        {
            int id = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);
                        if (sgitem.StageGateProjectListItemId != 0)
                        {
                            var item = spList.GetItemById(sgitem.StageGateProjectListItemId);
                            item["Title"] = sgitem.ProjectNumber;

                            //Project Information
                            item[StageGateProjectListFields.ProjectCancelledSent] = sgitem.ProjectCacnelledSent;

                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            item[StageGateProjectListFields.LastUpdatedFormName] = sgitem.LastUpdatedFormName;

                            item.Update();
                            spWeb.AllowUnsafeUpdates = false;
                            id = sgitem.StageGateProjectListItemId;
                        }
                    }
                }
            });
            return id;
        }
        public int UpdateStageGateProjectCompletedEmailSent(StageGateCreateProjectItem sgitem)
        {
            int id = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);
                        if (sgitem.StageGateProjectListItemId != 0)
                        {
                            var item = spList.GetItemById(sgitem.StageGateProjectListItemId);

                            //Project Information
                            item[StageGateProjectListFields.ProjectCompletedSent] = sgitem.ProjectCompletedSent;

                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            item[StageGateProjectListFields.LastUpdatedFormName] = sgitem.LastUpdatedFormName;

                            item.Update();
                            spWeb.AllowUnsafeUpdates = false;
                            id = sgitem.StageGateProjectListItemId;
                        }
                    }
                }
            });
            return id;
        }
    }
}
