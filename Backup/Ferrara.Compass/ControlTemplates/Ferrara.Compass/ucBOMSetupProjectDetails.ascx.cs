using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Ferrara.Compass.Classes;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Interfaces;
using System.Web;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.ControlTemplates.Ferrara.Compass
{
    public partial class ucBOMSetupProjectDetails : UserControl
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

        #region Member Variables
        private IBOMSetupService BOMSetupService;
        private IUtilityService utilities;
        #endregion

        #region Properties
        public int CompassListItemId { get; set; }
        #endregion
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeScreen();

            BOMSetupService = DependencyResolution.DependencyMapper.Container.Resolve<IBOMSetupService>();
            utilities = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();

            EnsureScriptManager();
            EnsureUpdatePanelFixups();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadProjectData();
        }

        #region Private Methods       
        private void EnsureScriptManager()
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);

            if (scriptManager == null)
            {
                scriptManager = new ScriptManager();
                scriptManager.EnablePartialRendering = true;


                if (Page.Form != null)
                {
                    Page.Form.Controls.AddAt(0, scriptManager);
                }
            }
        }
        private void EnsureUpdatePanelFixups()
        {
            if (this.Page.Form != null)
            {
                String formOnSubmitAtt = this.Page.Form.Attributes["onsubmit"];
                if (formOnSubmitAtt == "return _spFormOnSubmitWrapper ();")
                {
                    this.Page.Form.Attributes["onsubmit"] = "_spFormOnSubmitWrapper();";
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                "UpdatePanelFixup", "_spOriginalFormAction = document.forms[0].action; _spSuppressFormOnSubmitWrapper = true;", true);
        }
        private void InitializeScreen()
        {

        }
        #endregion


        #region Data Transfer Methods
        private void LoadProjectData()
        {
            BOMSetupProjectSummaryItem projectSummaryItem = BOMSetupService.GetProjectSummaryDetails(CompassListItemId);

            lblProjectType.Text = projectSummaryItem.ProjectType;
            lblProjectSubcategory.Text = projectSummaryItem.ProjectSubCategory;
            if(projectSummaryItem.PackingLocation == GlobalConstants.EXTERNAL_PACKER)
            {
                lblPackLocation.Text = projectSummaryItem.ExternalPacker;
            }
            else
            {
                lblPackLocation.Text = projectSummaryItem.PackingLocation;
            }
            
            txtWorkCenterAddInfo.Text = projectSummaryItem.WorkCenterAddInfo;
            lblPegHoleNeeded.Text = projectSummaryItem.PegHoleNeeded;
            lblItemConcept.Text = projectSummaryItem.ItemConcept;
            lblFGLikeItem.Text = projectSummaryItem.FGLikeItem;

            lblInitiatorName.Text = projectSummaryItem.InitiatorName;
            lblMarketingName.Text = projectSummaryItem.MarketingName;
            lblInTechManagerName.Text = projectSummaryItem.InTechManagerName;
            lblPMName.Text = projectSummaryItem.PMName;
            lblPackagingEngineerName.Text = utilities.GetPersonFieldForDisplay(projectSummaryItem.PackagingEngineerName);


            if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BOMSetupMaterialWarehouse.ToLower()))
            {
                divLogisticsInformation.Visible = true;
                #region Logistics Information
                txtMakeLocation.Text = projectSummaryItem.MakeLocation;
                txtPackLocation1.Text = projectSummaryItem.PackingLocation;
                //Procurement Type
                if (!string.IsNullOrEmpty(projectSummaryItem.ProcurementType) && !string.Equals(projectSummaryItem.ProcurementType, "Select..."))
                {
                    divProcurementType.Visible = true;
                    txtProcurementType.Text = projectSummaryItem.ProcurementType;
                }
                else
                {
                    divProcurementType.Visible = false;
                }
                //External Manufacturer
                if (!string.IsNullOrEmpty(projectSummaryItem.ExternalManufacturer) && !string.Equals(projectSummaryItem.ExternalManufacturer, "Select..."))
                {
                    divExternalManufacturer.Visible = true;
                    txtExternalManufacturer.Text = projectSummaryItem.ExternalManufacturer;
                }
                else
                {
                    divExternalManufacturer.Visible = false;
                }
                //External Packer
                if (!string.IsNullOrEmpty(projectSummaryItem.ExternalPacker) && !string.Equals(projectSummaryItem.ExternalPacker, "Select..."))
                {
                    dvPackLocation.Visible = true;
                    txtExternalPacker.Text = projectSummaryItem.ExternalPacker;
                }
                else
                {
                    dvPackLocation.Visible = false;
                }
                //Purchased Into
                if (!string.IsNullOrEmpty(projectSummaryItem.PurchasedIntoLocation) && !string.Equals(projectSummaryItem.PurchasedIntoLocation, "Select..."))
                {
                    divPurchaseIntoLocation.Visible = true;
                    txtPurchaseIntoLocation.Text = projectSummaryItem.PurchasedIntoLocation;
                }
                else
                {
                    divPurchaseIntoLocation.Visible = false;
                }
                //SAP Base UOM
                txtSAPBaseUOM.Text = projectSummaryItem.SAPBaseUOM;

                //Designate HUBDC
                txtDesignateHUBDC.Text = projectSummaryItem.DesignateHUBDC;
                #endregion
            }
        }
        #endregion   
    }
}
