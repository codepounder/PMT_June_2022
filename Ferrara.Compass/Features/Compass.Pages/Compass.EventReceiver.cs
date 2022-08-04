using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Publishing;
using Ferrara.Compass.Abstractions.Constants;
using System.Web.UI.WebControls.WebParts;
using Ferrara.Compass.WebParts;
using Ferrara.Compass.Classes;

namespace Ferrara.Compass.Features.Compass.Pages
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("49de0541-8f88-4934-85bb-0ccc53bf1431")]
    public class CompassEventReceiver : SPFeatureReceiver
    {
        private string pagesDocumentLibrary = "Pages";
        private PageLayout pageLayout = null;

        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            if (!DoesPagesLibraryExist((SPWeb)properties.Feature.Parent))
            {
                CreatePagesLibrary((SPWeb)properties.Feature.Parent);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_ItemProposal, "Item Proposal Form"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.ItemProposalForm.ItemProposalForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Item Proposal Form", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_ItemProposal, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_ItemProposal, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_ItemProposal, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_ItemProposal);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_ItemProposal2, "Item Proposal Form"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.ItemProposalForm2.ItemProposalForm2())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Item Proposal Form", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_ItemProposal2, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_ItemProposal2, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_ItemProposal2, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_ItemProposal2);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_ItemProposal3, "Item Proposal Form"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.ItemProposalForm2.ItemProposalForm2())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Item Proposal Form", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_ItemProposal2, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_ItemProposal2, "Header", 0);
                }
                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_ItemProposal2);
            }
            #region Stage Pages
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_StageGateCreateProject, "Stage Gate Create Project"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.StageGateCreateProjectForm.StageGateCreateProjectForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Stage Gate Create Project", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateCreateProject, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.PMTHeaderForm.PMTHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "SGS Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateCreateProject, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateCreateProject, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_StageGateCreateProject);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_StageGateDesignDeliverables, "Stage Gate Design Deliverables"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.StageGateNecessaryDeliverables.StageGateNecessaryDeliverables())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Stage Gate Design Deliverables", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateDesignDeliverables, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.PMTHeaderForm.PMTHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateDesignDeliverables, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateDesignDeliverables, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_StageGateDesignDeliverables);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_StageGateGenerateIPFs, "Stage Gate Generate IPFs"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateGenerateIPFs, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.StageGateGenerateIPFsForm.StageGateGenerateIPFsForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Stage Gate Generate IPFs", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateGenerateIPFs, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.PMTHeaderForm.PMTHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateGenerateIPFs, "Header", 0);
                }
                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_StageGateGenerateIPFs);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_StageGateDevelopDeliverables, "Stage Gate Develop Deliverables"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.StageGateNecessaryDeliverables.StageGateNecessaryDeliverables())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Stage Gate Develop Deliverables", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateDevelopDeliverables, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.PMTHeaderForm.PMTHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateDevelopDeliverables, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateDevelopDeliverables, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_StageGateDevelopDeliverables);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_StageGateValidateDeliverables, "Stage Gate Validate Deliverables"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.StageGateNecessaryDeliverables.StageGateNecessaryDeliverables())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Stage Gate Validate Deliverables", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateValidateDeliverables, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.PMTHeaderForm.PMTHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateValidateDeliverables, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateValidateDeliverables, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_StageGateValidateDeliverables);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_StageGateIndustrializeDeliverables, "Stage Gate Industrialize Deliverables"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.StageGateNecessaryDeliverables.StageGateNecessaryDeliverables())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Stage Gate Industrialize Deliverables", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateIndustrializeDeliverables, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.PMTHeaderForm.PMTHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateIndustrializeDeliverables, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateIndustrializeDeliverables, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_StageGateIndustrializeDeliverables);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_StageGateLaunchDeliverables, "Stage Gate Launch Deliverables"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.StageGateNecessaryDeliverables.StageGateNecessaryDeliverables())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Stage Gate Launch Deliverables", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateLaunchDeliverables, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.PMTHeaderForm.PMTHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateLaunchDeliverables, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateLaunchDeliverables, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_StageGateLaunchDeliverables);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_StageGatePostLaunchDeliverables, "Stage Gate Post Launch Deliverables"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.StageGateNecessaryDeliverables.StageGateNecessaryDeliverables())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Stage Gate Post Launch Deliverables", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGatePostLaunchDeliverables, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.PMTHeaderForm.PMTHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGatePostLaunchDeliverables, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGatePostLaunchDeliverables, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_StageGatePostLaunchDeliverables);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_StageGateProjectPanel, "Stage Gate Project Panel"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.StageGateProjectPanelForm.StageGateProjectPanelForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Stage Gate Project Panel", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateProjectPanel, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.PMTHeaderForm.PMTHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateProjectPanel, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateProjectPanel, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_StageGateProjectPanel);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_StageGateGenerateBriefPDF, "Stage Gate Generate Brief PDF"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.StageGateGenerateBriefPDF.StageGateGenerateBriefPDF())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Stage Gate Generate Brief PDF", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateGenerateBriefPDF, "Header", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_StageGateGenerateBriefPDF);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_StageGateFinancialSummary, "Stage Gate Financial Summary"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.StageGateFinanceSummaryForm.StageGateFinancialSummaryForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Stage Gate Financial Summary", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateFinancialSummary, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.PMTHeaderForm.PMTHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "SGS Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateFinancialSummary, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateFinancialSummary, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_StageGateFinancialSummary);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_StageGateFinancialBrief, "Stage Gate Financial Brief"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.StageGateFinancialBriefListForm.StageGateFinancialBriefForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Stage Gate Financial Brief List", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateFinancialBrief, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.PMTHeaderForm.PMTHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "SGS Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateFinancialBrief, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_StageGateFinancialBrief, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_StageGateFinancialBrief);
            }
            #endregion
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_InitialApprovalReview, "Initial Approval Review Form"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.InitialApprovalReviewForm.InitialApprovalReviewForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Initial Approval Review Form", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_InitialApprovalReview, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_InitialApprovalReview, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_InitialApprovalReview, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_InitialApprovalReview);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_InitialCapacityReview, "Initial Capacity Review Form"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.InitialCapacityReviewForm.InitialCapacityReviewForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Initial Capacity Review Form", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_InitialCapacityReview, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_InitialCapacityReview, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_InitialCapacityReview, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_InitialCapacityReview);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_InitialCostingReview, "Initial Costing Review Form"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.InitialCostingReviewForm.InitialCostingReviewForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Initial Costing Review Form", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_InitialCostingReview, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_InitialCostingReview, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_InitialCostingReview, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_InitialCostingReview);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_TradePromoGroup, "Trade Promo Group Form"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.TradePromoGroupForm.TradePromoGroupForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Trade Promo Group Form", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_TradePromoGroup, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_TradePromoGroup, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_TradePromoGroup, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_TradePromoGroup);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_RequestRecipeSpec, "Request Recipe Spec Form"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.RequestRecipeSpecForm.RequestRecipeSpecForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Request Recipe Spec Form", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_RequestRecipeSpec, "Header", 0);
                }

                using (WebPart webPart = new Ferrara.Compass.WebParts.PMTHeaderForm.PMTHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_RequestRecipeSpec, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_RequestRecipeSpec, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_RequestRecipeSpec);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_EstPricing, "Initial Pricing Form"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.EstPricingForm.EstPricingForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Initial Pricing Form", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_EstPricing, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_EstPricing, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_EstPricing, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_EstPricing);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_EstBracketPricing, "Initial Bracket Pricing Form"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.EstBracketPricingForm.EstBracketPricingForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Initial Bracket Pricing Form", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_EstBracketPricing, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_EstBracketPricing, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_EstBracketPricing, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_EstBracketPricing);
            }


            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_Distribution, "Distribution Form"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.DistributionForm.DistributionForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Distribution Form", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_Distribution, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_Distribution, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_Distribution, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_Distribution);
            }

            /*if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_DistributionNew, "Distribution Form"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.DistributionNewForm.DistributionNewForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Distribution Form", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_DistributionNew, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_DistributionNew, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_DistributionNew, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_DistributionNew);
            }*/

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_OPS, "Operations Form"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.OpsForm.OpsForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Operations Form", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_OPS, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_OPS, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_OPS, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_OPS);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_ExternalManufacturing, "External Manufacturing Form"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.ExternalManufacturingForm.ExternalManufacturingForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "External Manufacturing Form", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_ExternalManufacturing, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_ExternalManufacturing, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_ExternalManufacturing, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_ExternalManufacturing);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_SAPInitialItemSetup, "SAP Intial Item Setup Form"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.SAPInitialItemSetupForm.SAPInitialItemSetupForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "SAP Intial Item Setup Form", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_SAPInitialItemSetup, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_SAPInitialItemSetup, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_SAPInitialItemSetup, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_SAPInitialItemSetup);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_PrelimSAPInitialItemSetup, "Preliminary SAP Intial Item Setup Form"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.PrelimSAPInitialItemSetupForm.PrelimSAPInitialItemSetupForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Preliminary SAP Intial Item Setup Form", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_PrelimSAPInitialItemSetup, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_PrelimSAPInitialItemSetup, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_PrelimSAPInitialItemSetup, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_PrelimSAPInitialItemSetup);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_QA, "InTech Regulatory Form"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.QAForm.QAForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "InTech Regulatory Form", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_QA, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_QA, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_QA, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_QA);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_BEQRC, "BE QRC Form – Marketing Confirmation Form"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.BEQRCForm.BEQRCForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "BE QRC Form – Marketing Confirmation Form", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_BEQRC, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_BEQRC, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_BEQRC, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_BEQRC);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_PE, "Bill of Material SetUp Form (PE)"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.BillOfMaterialsSetUpForm.BillOfMaterialsSetUpForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Bill of Material SetUp Form (PE)", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_PE, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_PE, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_PE, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_PE);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_PE2, "Bill of Material SetUp Form (PE2)"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.BillOfMaterialsSetUpForm.BillOfMaterialsSetUpForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Bill of Material SetUp Form (PE2)", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_PE2, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_PE2, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_PE2, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_PE2);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_Proc, "Bill of Material SetUp Form (Proc)"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.BillOfMaterialsSetUpForm.BillOfMaterialsSetUpForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Bill of Material SetUp Form (Proc)", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_Proc, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_Proc, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_Proc, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_Proc);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_CommercializationItemSummary, "Commercialization Item Summary"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.CommercializationSummaryForm.CommercializationSummaryForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Commercialization Item Summary", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_CommercializationItemSummary, "Header", 0);
                }
                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_CommercializationItemSummary);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_OBMFirstReview, "PM First Review Form"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.OBMFirstReviewForm.OBMFirstReviewForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "PM First Review Form", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_OBMFirstReview, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_OBMFirstReview, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_OBMFirstReview, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_OBMFirstReview);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_PMFirstReview, "PM First Review Form"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.PMFirstReviewForm.PMFirstReviewForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "PM First Review Form", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_PMFirstReview, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.PMTProjectHeader2.PMTProjectHeader2())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_PMFirstReview, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_PMFirstReview, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_PMFirstReview);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_OBMSecondReview, "PM Second Review Form"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.OBMSecondReviewForm.OBMSecondReviewForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "PM Second Review Form", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_OBMSecondReview, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_OBMSecondReview, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_OBMSecondReview, "Right", 0);
                }
                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_OBMSecondReview);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_PMSecondReview, "PM Second Review Form"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.PMSecondReview.PMSecondReview())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "PM Second Review Form", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_PMSecondReview, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.PMTProjectHeader2.PMTProjectHeader2())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_PMSecondReview, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_PMSecondReview, "Right", 0);
                }
                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_PMSecondReview);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_PMSecondReview2, "PM Second Review Form"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.PMSecondReview.PMSecondReview())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "PM Second Review Form", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_PMSecondReview, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.PMTProjectHeader2.PMTProjectHeader2())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_PMSecondReview, "Header", 0);
                }
                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_PMSecondReview);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_SecondaryApprovalReview, "Secondary Approval Review"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.SecondaryApprovalReviewForm.SecondaryApprovalReviewForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Secondary Approval Review", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_SecondaryApprovalReview, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_SecondaryApprovalReview, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_SecondaryApprovalReview, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_SecondaryApprovalReview);
            }

            #region BOMSetupSAP
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_BOMSetupSAP, "SAP BOM Setup"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.SAPBOMSetupForm.SAPBOMSetupForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "SAP BOM Setup", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_BOMSetupSAP, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_BOMSetupSAP, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_BOMSetupSAP, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_BOMSetupSAP);
            }
            #endregion

            #region SAPBOMSetup
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_SAPBOMSetup, "SAP BOM Setup"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.BOMSetupSAPForm.BOMSetupSAPForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "SAP BOM Setup", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_SAPBOMSetup, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm_New.ProjectHeaderForm_New())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_SAPBOMSetup, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_SAPBOMSetup, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_SAPBOMSetup);
            }
            #endregion

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_SAPCompleteItemSetup, "SAP Complete Item Setup Form"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.SAPCompleteItemSetupForm.SAPCompleteItemSetupForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "SAP Complete Item Setup Form", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_SAPCompleteItemSetup, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm_New.ProjectHeaderForm_New())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_SAPCompleteItemSetup, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_SAPCompleteItemSetup, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_SAPCompleteItemSetup);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_BOMSetupMaterialWarehouse, "Material Warehouse Setup Form"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.BOMSetupMaterialWarehouseForm.BOMSetupMaterialWarehouseForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Material Warehouse Setup Form", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_BOMSetupMaterialWarehouse, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm_New.ProjectHeaderForm_New())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_BOMSetupMaterialWarehouse, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_BOMSetupMaterialWarehouse, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_BOMSetupMaterialWarehouse);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_FinishedGoodPackSpec, "Finished Good Pack Spec"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.FinishedGoodPackSpecForm.FinishedGoodPackSpecForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Finished Good Pack Spec", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_FinishedGoodPackSpec, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_FinishedGoodPackSpec, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_FinishedGoodPackSpec, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_FinishedGoodPackSpec);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_GraphicsRequest_New, "Graphics Request"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.GraphicsRequestForm.GraphicsRequestForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Graphics Request", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_GraphicsRequest_New, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_GraphicsRequest_New, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_GraphicsRequest_New, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_GraphicsRequest_New);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_GraphicsRequest, "Graphics Request"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.GraphicsRequestForm_New.GraphicsRequestForm_New())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Graphics Request", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_GraphicsRequest, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm_New.ProjectHeaderForm_New())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_GraphicsRequest, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_GraphicsRequest, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_GraphicsRequest);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_GraphicsRequestDetail, "Graphics Request Detail"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.GraphicsRequestDetailForm.GraphicsRequestDetailForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Graphics Request Detail", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_GraphicsRequestDetail, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_GraphicsRequestDetail, "Header", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_GraphicsRequestDetail);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_GraphicsRequestDetail_New, "Graphics Request Detail"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.GraphicsRequestDetailForm_New.GraphicsRequestDetailForm_New())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Graphics Request Detail", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_GraphicsRequestDetail_New, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm_New.ProjectHeaderForm_New())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_GraphicsRequestDetail_New, "Header", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_GraphicsRequestDetail_New);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_ComponentCosting, "Component Costing"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.ComponentCostingQuoteForm.ComponentCostingQuoteForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Component Costing", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_ComponentCosting, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_ComponentCosting, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_ComponentCosting, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_ComponentCostingSummary);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_ComponentCostingSummary, "Component Costing Summary"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.ComponentCostingSummaryForm.ComponentCostingSummaryForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Component Costing Summary", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_ComponentCostingSummary, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_ComponentCostingSummary, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_ComponentCostingSummary, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_ComponentCosting);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_MaterialsReceivedCheck, "Materials Received Check"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.MaterialsReceivedCheckForm.MaterialsReceivedCheckForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Materials Received Check", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_MaterialsReceivedCheck, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_MaterialsReceivedCheck, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_MaterialsReceivedCheck, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_MaterialsReceivedCheck);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_FirstProductionCheck, "First Production Check"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.FirstProductionCheckForm.FirstProductionCheckForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "First Production Check", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_FirstProductionCheck, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_FirstProductionCheck, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_FirstProductionCheck, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_FirstProductionCheck);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_DistributionCenterCheck, "Distribution Center Check"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.DistributionCenterCheckForm.DistributionCenterCheckForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Distribution Center Check", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_DistributionCenterCheck, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_DistributionCenterCheck, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_DistributionCenterCheck, "Right", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_DistributionCenterCheck);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_Nutritionals, "Project Nutritionals"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.WorldSyncNutritionalsForm.WorldSyncNutritionalsForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Nutritionals", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_Nutritionals, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_Nutritionals, "Header", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_Nutritionals);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_WorldSyncGlobal, "Project World Sync"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.CompassWorldSyncMainForm.CompassWorldSyncMainForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project World Sync", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_WorldSyncGlobal, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_WorldSyncGlobal, "Header", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_WorldSyncGlobal);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_FinalRoutingsSummary, GlobalConstants.PAGE_NAME_FinalRoutingsSummary))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.SAPFinalRoutingsSummaryForm.SAPFinalRoutingsSummaryForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, GlobalConstants.PAGE_FinalRoutingsSummary, pagesDocumentLibrary + "/" + GlobalConstants.PAGE_FinalRoutingsSummary, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm.ProjectHeaderForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_FinalRoutingsSummary, "Header", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_FinalRoutingsSummary);
            }
            #region Dashboards
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_ProjectStatus, "Project Status"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectDashboard2.ProjectDashboard2())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Status", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_ProjectStatus, "Header", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_ProjectStatus);
            }


            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_TaskDashboard, "PMT Dashboard"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.MyWorkflowTasksForm3.MyWorkflowTasksForm3())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "My Open Tasks", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_TaskDashboard, "Header", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_TaskDashboard);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_TaskDashboard_New, "PMT Dashboard"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.MyOpenProjectsForm.MyOpenProjectsForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "My Open Projects", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_TaskDashboard_New, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.MyWorkflowTasksForm_New.MyWorkflowTasksForm_New())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "My Open Tasks", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_TaskDashboard_New, "Header", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_TaskDashboard_New);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_ProjectTimelineUpdate, "Update Project Timeline"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectTimelineUpdateForm.ProjectTimelineUpdateForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Update Project Timeline", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_ProjectTimelineUpdate, "Header", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_ProjectTimelineUpdate);
            }

            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_HomeOBM, GlobalConstants.PAGE_NAME_HomeOBM))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.MyWorkflowTasksForm.MyWorkflowTasksForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, GlobalConstants.PAGE_NAME_HomeOBM, pagesDocumentLibrary + "/" + GlobalConstants.PAGE_HomeOBM, "Header", 0);
                }
                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_HomeOBM);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_AllProjectDashboard, GlobalConstants.PAGE_NAME_AllProjectDashboard))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.AllOpenProjectsForm.AllOpenProjectsForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, GlobalConstants.PAGE_NAME_AllProjectDashboard, pagesDocumentLibrary + "/" + GlobalConstants.PAGE_AllProjectDashboard, "Header", 0);
                }
                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_AllProjectDashboard);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_AllProjectDetailsDashboard, GlobalConstants.PAGE_NAME_AllProjectDetailsDashboard))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.AllProjectDetailsDashboard.AllProjectDetailsDashboard())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, GlobalConstants.PAGE_NAME_AllProjectDetailsDashboard, pagesDocumentLibrary + "/" + GlobalConstants.PAGE_AllProjectDetailsDashboard, "Header", 0);
                }
                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_AllProjectDetailsDashboard);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_AllProjectDetailsDashboard2, GlobalConstants.PAGE_NAME_AllProjectDetailsDashboard))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.AllProjectDetailsDashboard2.AllProjectDetailsDashboard2())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, GlobalConstants.PAGE_NAME_AllProjectDetailsDashboard, pagesDocumentLibrary + "/" + GlobalConstants.PAGE_AllProjectDetailsDashboard2, "Header", 0);
                }
                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_AllProjectDetailsDashboard2);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_AllParentProjectDetailsDashboard, GlobalConstants.PAGE_NAME_AllParentProjectDetailsDashboard))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.AllParentProjectDetailsDashboard.AllParentProjectDetailsDashboard())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, GlobalConstants.PAGE_NAME_AllParentProjectDetailsDashboard, pagesDocumentLibrary + "/" + GlobalConstants.PAGE_AllParentProjectDetailsDashboard, "Header", 0);
                }
                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_AllParentProjectDetailsDashboard);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_UpdateProjectNotes, GlobalConstants.PAGE_NAME_UpdateProjectNotes))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.UpdateProjectNotesForm.UpdateProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, GlobalConstants.PAGE_NAME_UpdateProjectNotes, pagesDocumentLibrary + "/" + GlobalConstants.PAGE_UpdateProjectNotes, "Header", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_UpdateProjectNotes);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_PMTAdministration, "PMT Administration"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.CompassAdministrationForm.CompassAdministrationForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "PMT Administration", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_PMTAdministration, "Header", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_PMTAdministration);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_WorldSyncRequestDashboard, "World Sync Request Dashboard"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.WorldSyncRequestDashboard.WorldSyncRequestDashboard())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "World Sync Request Dashboard", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_WorldSyncRequestDashboard, "Header", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_WorldSyncRequestDashboard);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_WorldSyncRequestUpload, "World Sync Request Upload"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.WorldSyncRequestUpload.WorldSyncRequestUpload())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "World Sync Request Upload", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_WorldSyncRequestUpload, "Header", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_WorldSyncRequestUpload);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_WorldSyncRequestFile, "World Sync Request File"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.sharedheaderfiles.sharedheaderfiles())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_WorldSyncRequestFile, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.WorldSyncRequestFile.WorldSyncRequestFile())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "World Sync Request File", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_WorldSyncRequestFile, "Header", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_WorldSyncRequestFile);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_WorldSyncFuseFile, "World Sync Fuse File"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.sharedheaderfiles.sharedheaderfiles())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_WorldSyncFuseFile, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.WorldSyncFuseFile.WorldSyncFuseFile())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "World Sync Fuse File", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_WorldSyncFuseFile, "Header", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_WorldSyncFuseFile);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_WorldSyncRequestReceipt, "World Sync Receive File"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.WorldSyncRequestReceipt.WorldSyncRequestReceipt())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "World Sync Receive File", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_WorldSyncRequestReceipt, "Header", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_WorldSyncRequestReceipt);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, "MyOpenProjectsOriginal.aspx", "My Open Projects Original"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.MyOpenProjects2.MyOpenProjects2())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "My Open Projects Original", pagesDocumentLibrary + "/" + "MyOpenProjectsOriginal.aspx", "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.MyWorkflowTasksForm2.MyWorkflowTasksForm2())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "My Open Projects Original", pagesDocumentLibrary + "/" + "MyOpenProjectsOriginal.aspx", "Header", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, "MyOpenProjectsOriginal.aspx");
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, "MyOpenProjectsNew.aspx", "My Open Projects New"))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.MyOpenProjects2.MyOpenProjects2())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "My Open Projects New", pagesDocumentLibrary + "/" + "MyOpenProjectsNew.aspx", "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.MyWorkflowTasksForm3.MyWorkflowTasksForm3())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "My Open Projects New", pagesDocumentLibrary + "/" + "MyOpenProjectsNew.aspx", "Header", 0);
                }

                ApprovePages((SPWeb)properties.Feature.Parent, "MyOpenProjectsNew.aspx");
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_BOMSetupPE, GlobalConstants.PAGE_NAME_BOMSetupPE))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.BOMSetupPEForm.BOMSetupPEForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, GlobalConstants.PAGE_NAME_BOMSetupPE, pagesDocumentLibrary + "/" + GlobalConstants.PAGE_BOMSetupPE, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_BOMSetupPE, "Right", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm_New.ProjectHeaderForm_New())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_BOMSetupPE, "Header", 0);
                }
                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_BOMSetupPE);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_BOMSetupProc, GlobalConstants.PAGE_NAME_BOMSetupProc))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.BOMSetupProcForm.BOMSetupProcForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, GlobalConstants.PAGE_NAME_BOMSetupProc, pagesDocumentLibrary + "/" + GlobalConstants.PAGE_BOMSetupProc, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_BOMSetupProc, "Right", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm_New.ProjectHeaderForm_New())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_BOMSetupProc, "Header", 0);
                }
                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_BOMSetupProc);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_BOMSetupPE2, GlobalConstants.PAGE_NAME_BOMSetupPE2))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.BOMSetupPE2Form.BOMSetupPE2Form())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, GlobalConstants.PAGE_NAME_BOMSetupPE2, pagesDocumentLibrary + "/" + GlobalConstants.PAGE_BOMSetupPE2, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_BOMSetupPE2, "Right", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm_New.ProjectHeaderForm_New())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_BOMSetupPE2, "Header", 0);
                }
                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_BOMSetupPE2);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_BOMSetupPE3, GlobalConstants.PAGE_NAME_BOMSetupPE3))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.BOMSetupPE3Form.BOMSetupPE3Form())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, GlobalConstants.PAGE_NAME_BOMSetupPE3, pagesDocumentLibrary + "/" + GlobalConstants.PAGE_BOMSetupPE3, "Header", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Project Notes", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_BOMSetupPE3, "Right", 0);
                }
                using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm_New.ProjectHeaderForm_New())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_BOMSetupPE3, "Header", 0);
                }
                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_BOMSetupPE3);
            }
            if (CreateWebPartPage((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_RegulatoryComments, GlobalConstants.PAGE_RegulatoryComments))
            {
                using (WebPart webPart = new Ferrara.Compass.WebParts.RegulatoryComments.RegulatoryComments())
                {
                    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, GlobalConstants.PAGE_RegulatoryComments, pagesDocumentLibrary + "/" + GlobalConstants.PAGE_RegulatoryComments, "Header", 0);
                }
                //using (WebPart webPart = new Ferrara.Compass.WebParts.ProjectHeaderForm_New.ProjectHeaderForm_New())
                //{
                //    CreateFormWebPart((SPWeb)properties.Feature.Parent, webPart, "Compass Header", pagesDocumentLibrary + "/" + GlobalConstants.PAGE_RegulatoryComments, "Header", 0);
                //}
                ApprovePages((SPWeb)properties.Feature.Parent, GlobalConstants.PAGE_RegulatoryComments);
            }
            #endregion
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

        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
            DeletePages(properties);
        }

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}

        private void DeletePages(SPFeatureReceiverProperties properties)
        {
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_TradePromoGroup);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_RequestRecipeSpec);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_ItemProposal);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_OPS);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_QA);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_BillofMaterialSetUpPE);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_BillofMaterialSetUpPE2);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_BillofMaterialSetUpProc);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_CommercializationItemSummary);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_SAPInitialItemSetup);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_ExternalManufacturing);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_OBMSecondReview);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_ComponentCosting);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_ComponentCostingSummary);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_FinishedGoodPackSpec);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_GraphicsRequest);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_Nutritionals);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_WorldSyncGlobal);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_WorldSyncRequestDashboard);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_WorldSyncRequestUpload);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_WorldSyncRequestFile);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_WorldSyncFuseFile);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_WorldSyncRequestReceipt);

            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_LISTVIEW_SLTExceptionChristmas);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_LISTVIEW_SLTExceptionCOMAN);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_LISTVIEW_SLTExceptionEaster);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_LISTVIEW_SLTExceptionEveryday);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_LISTVIEW_SLTExceptionHalloween);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_LISTVIEW_SLTExceptionPrivateLabel);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_LISTVIEW_SLTExceptionSummer);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_LISTVIEW_SLTExceptionValentine);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_LISTVIEW_SLTSummary);

            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_LISTVIEW_AgendaViewEveryday);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_LISTVIEW_AgendaViewSeasonal);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_LISTVIEW_AllOpenProjects);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_LISTVIEW_BrandManager);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_LISTVIEW_OBM);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_LISTVIEW_OBMAdmin);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_LISTVIEW_FirstShipDate);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_LISTVIEW_AllCompletedProjects);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_LISTVIEW_AllCancelledProjects);
            DeleteWebPartPage((SPWeb)properties.Feature.Parent, pagesDocumentLibrary, GlobalConstants.PAGE_LISTVIEW_RndView);
        }

        #region Generic Page Library Methods
        private void CreatePagesLibrary(SPWeb web)
        {
            var listId = web.Lists.Add(pagesDocumentLibrary, "Pages library", SPListTemplateType.WebPageLibrary);
            var list = web.Lists[listId];
            list.OnQuickLaunch = true;
            list.EnableModeration = true;
            list.Update();
        }

        private bool DoesPagesLibraryExist(SPWeb web)
        {
            var pageLibrary = web.Lists.TryGetList(pagesDocumentLibrary);
            return pageLibrary != null;
        }
        #endregion

        #region Generic Page Methods
        private void ApprovePages(SPWeb web, string pageUrl)
        {
            var pubWeb = PublishingWeb.GetPublishingWeb(web);
            var page = pubWeb.GetPublishingPage(pubWeb.Url + "/pages/" + pageUrl);
            if (page != null)
            {
                var item = page.ListItem;
                if (item.File.Level == SPFileLevel.Checkout)
                {
                    item.File.CheckIn("Checked in by Page Deployment Feature");
                }
                //item.File.Publish("Published by Page Deployment Feature");
                //item.File.Approve("Approved by Page Deployment Feature");
            }

            pubWeb.Close();
        }

        private Boolean CreateWebPartPage(SPWeb web, string pageName, string pageTitle)
        {
            Boolean pageCreated = false;
            try
            {
                if (PublishingWeb.IsPublishingWeb(web))
                {
                    var pubWeb = PublishingWeb.GetPublishingWeb(web);
                    var layouts = pubWeb.GetAvailablePageLayouts().ToList();
                    if (pageLayout == null)
                    {
                        GetPagelayout(layouts);
                    }

                    var page = pubWeb.GetPublishingPages().ToList().FirstOrDefault(x => x.Name.Equals(pageName));
                    if (page == null)
                    {
                        var newPage = pubWeb.GetPublishingPages().Add(pageName, pageLayout);
                        newPage.Title = pageTitle;
                        newPage.IncludeInCurrentNavigation = false;
                        newPage.IncludeInGlobalNavigation = false;
                        newPage.Update();
                        pageCreated = true;
                    }
                    pubWeb.Close();
                }
                else
                {
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, string.Concat("CreateWebPartPage - The SPWeb must be a PublishingWeb to allow create of webpart pages."));
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, string.Concat("CreateWebPartPage: ", pageName, " ", pageTitle, " ", ex.Message));
            }

            return pageCreated;
        }

        private string CreateFormWebPart(SPWeb web, WebPart webPart, string title, string pageUrl, string zoneId, int zoneIndex)
        {
            webPart.Title = title;
            using (var manager = web.GetLimitedWebPartManager(pageUrl, PersonalizationScope.Shared))
            {
                webPart.ChromeType = PartChromeType.None;
                manager.AddWebPart(webPart, zoneId, zoneIndex);
                return webPart.ID;
            }
        }
        private void GetPagelayout(List<PageLayout> layouts)
        {
            pageLayout = layouts.FirstOrDefault(x => x.Name.ToLower().Equals("blankwebpartpage.aspx"));
        }

        private void DeleteWebPartPage(SPWeb web, string pageFolder, string pageName)
        {
            try
            {
                var libraryFolder = web.GetFolder(pageFolder);
                var files = libraryFolder.Files;
                if (files.Count > 0)
                {
                    var file = web.GetFile(string.Format("{0}/{1}", libraryFolder.Url, pageName));
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, string.Concat("DeleteWebPartPage: ", pageName, " ", pageFolder, " ", ex.Message));
            }
        }

        #endregion
    }
}
