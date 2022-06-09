using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Interfaces;
using System;
using System.ComponentModel;
using System.Web;
using Microsoft.Practices.Unity;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Ferrara.Compass.Classes;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Models;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections.Generic;
using Ferrara.Compass.ControlTemplates.Ferrara.Compass;
using System.Linq;
using System.Text.RegularExpressions;

namespace Ferrara.Compass.WebParts.StageGateGenerateIPFsForm
{
    [ToolboxItemAttribute(false)]
    public partial class StageGateGenerateIPFsForm : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        #region Member Variables
        private IDashboardService dashboardService;
        private IStageGateCreateProjectService stageGateCreateProjectService;
        private IStageGateGeneralService stageGateGeneralService;
        private IUtilityService utilityService;
        private IUserManagementService userManagementService;
        private IExceptionService exceptionService;
        private IConfigurationManagementService configurationService;
        private IItemProposalService IPFService;
        private IWorkflowService workflowService;
        private const string _ucSGSProjectInformation = @"~/_controltemplates/15/Ferrara.Compass/ucSGSProjectInformation.ascx";
        private int StageGateItemId = 0;
        private string webUrl;
        #endregion
        #region Properties
        private string ProjectNumber
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo];
                return string.Empty;
            }
        }
        #endregion
        #region Constructor
        public StageGateGenerateIPFsForm()
        {

        }
        #endregion
        #region OnInit
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();

            dashboardService = DependencyResolution.DependencyMapper.Container.Resolve<IDashboardService>();
            stageGateCreateProjectService = DependencyResolution.DependencyMapper.Container.Resolve<IStageGateCreateProjectService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            userManagementService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            configurationService = DependencyResolution.DependencyMapper.Container.Resolve<IConfigurationManagementService>();
            stageGateGeneralService = DependencyResolution.DependencyMapper.Container.Resolve<IStageGateGeneralService>();
            IPFService = DependencyResolution.DependencyMapper.Container.Resolve<IItemProposalService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();

            EnsureScriptManager();
            EnsureUpdatePanelFixups();

        }
        #endregion
        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {

            this.Page.Form.Enctype = "multipart/form-data";
            webUrl = SPContext.Current.Web.Url;

            if (!Page.IsPostBack)
            {
                this.divAccessDenied.Visible = false;
                this.divAccessRequest.Visible = false;

                // Check for a valid project number
                if (!CheckProjectNumber())
                    return;

                StageGateCreateProjectItem stageGateItem = stageGateCreateProjectService.GetStageGateProjectItem(StageGateItemId);
                hdnProjectType.Value = stageGateItem.ProjectType;
                hdnLOB.Value = stageGateItem.LineOfBisiness;
                hdnPMTListItemId.Value = StageGateItemId.ToString();
                hdnStage.Value = stageGateItem.Stage;
                LoadIPFTable();
            }
            else
            {
                StageGateItemId = Convert.ToInt32(hdnPMTListItemId.Value);
            }

            if (string.IsNullOrWhiteSpace(hdnProjectType.Value))
            {
                StageGateCreateProjectItem stageGateItem = stageGateCreateProjectService.GetStageGateProjectItem(StageGateItemId);
                hdnProjectType.Value = stageGateItem.ProjectType;
                hdnLOB.Value = stageGateItem.LineOfBisiness;
            }

            hdnParentProjectNumber.Value = ProjectNumber;
            LoadProjectInformation();
        }
        #endregion
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
        private void LoadIPFTable()
        {

            try
            {
                List<ItemProposalItem> childProjectData = dashboardService.getRequestedChildProjectDetails(StageGateItemId);
                rptChildProjects.DataSource = childProjectData.OrderBy(x => x.GenerateIPFSortOrder);
                rptChildProjects.DataBind();
            }
            catch (Exception error)
            {
                ErrorSummary.AddError(error.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "SGS Generate IPFs Form" + ": " + error.Message);
                exceptionService.Handle(LogCategory.CriticalError, error, "SGS Generate IPFs Form", "addTempIPF_Click");
            }
        }
        private void LoadProjectInformation()
        {
            //Project Information
            ucSGSProjectInformation ctrl2 = (ucSGSProjectInformation)Page.LoadControl(_ucSGSProjectInformation);
            ctrl2.StageGateItemId = StageGateItemId;
            SGSProjectInformation.Controls.Add(ctrl2);

            if (hdnUCLoaded.Value == "true")
            {
                ucSGSItemDetailInfo ctrl = (ucSGSItemDetailInfo)Page.LoadControl(@"~/_controltemplates/15/Ferrara.Compass/ucSGSItemDetailInfo.ascx");
                ctrl.IPFGenerated = Convert.ToBoolean(hdnUCLoadedIPFGenerated.Value);
                ctrl.childProjectNo = hdnUCLoadedchildProjectNo.Value;
                ctrl.StageGateItemId = Convert.ToInt32(hdnUCLoadedPMTItemId.Value);
                ctrl.ID = "ucSGSItemDetailInfo";
                ctrl.StageGateChildItemId = Convert.ToInt32(hdnUCLoadedId.Value);
                ctrl.firstLoad = false;
                phMsg.Controls.Add(ctrl);
            }
        }
        protected void rptChildProjects_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                ItemProposalItem SGSIPFItem = (ItemProposalItem)e.Item.DataItem;

                DropDownList ddlTBDIndicator = ((DropDownList)e.Item.FindControl("ddlTBDIndicator"));
                TextBox txtSAPItemNumber = (TextBox)e.Item.FindControl("txtSAPItemNumber");
                TextBox txtSAPDescription = (TextBox)e.Item.FindControl("txtSAPDescription");
                DropDownList ddlMaterialGroup1Brand = ((DropDownList)e.Item.FindControl("ddlMaterialGroup1Brand"));
                if (string.IsNullOrEmpty(SGSIPFItem.ProductHierarchyLevel2) || SGSIPFItem.ProductHierarchyLevel2 == "Select...")
                {
                    ddlMaterialGroup1Brand.Enabled = false;
                }
                else
                {
                    ReloadBrand(SGSIPFItem.ProductHierarchyLevel2, ddlMaterialGroup1Brand);
                    if (!string.IsNullOrEmpty(SGSIPFItem.MaterialGroup1Brand) && !string.Equals(SGSIPFItem.MaterialGroup1Brand, "Multiple"))
                    {
                        Utilities.SetDropDownValue(SGSIPFItem.MaterialGroup1Brand, ddlMaterialGroup1Brand, this.Page);
                        ddlMaterialGroup1Brand.ToolTip = SGSIPFItem.MaterialGroup1Brand;
                    }
                }
                //Utilities.BindDropDownItems(drpMaterialGroup1Brand, GlobalConstants.LIST_MaterialGroup1Lookup, webUrl);

                DropDownList ddlCustomer = ((DropDownList)e.Item.FindControl("ddlCustomer"));
                Utilities.BindDropDownItemsAddValues(ddlCustomer, GlobalConstants.LIST_CustomersLookup, webUrl);
                HiddenField hdnProjectNumber = (HiddenField)e.Item.FindControl("hdnProjectNumber");
                HiddenField hdnPMTProjectListItemId = (HiddenField)e.Item.FindControl("hdnPMTProjectListItemId");
                HiddenField hdnCompassListItemId = (HiddenField)e.Item.FindControl("hdnCompassListItemId");
                HiddenField hdnPopupValidated = (HiddenField)e.Item.FindControl("hdnPopupValidated");
                HiddenField hdnIPFGenerated = (HiddenField)e.Item.FindControl("hdnIPFGenerated");
                HiddenField hdnSortOrder = (HiddenField)e.Item.FindControl("hdnSortOrder");
                HiddenField hdnProductHierarchyLevel1 = (HiddenField)e.Item.FindControl("hdnProductHierarchyLevel1");
                HiddenField hdnManuallyCreateSAPDescription = (HiddenField)e.Item.FindControl("hdnManuallyCreateSAPDescription");
                HiddenField hdnProductHierarchyLevel2 = (HiddenField)e.Item.FindControl("hdnProductHierarchyLevel2");
                HiddenField hdnMaterialGroup5 = (HiddenField)e.Item.FindControl("hdnMaterialGroup5");
                HiddenField hdnCustomerSpecific = (HiddenField)e.Item.FindControl("hdnCustomerSpecific");
                HiddenField hdnCustomer = (HiddenField)e.Item.FindControl("hdnCustomer");

                HyperLink lnkProjectNumber = (HyperLink)e.Item.FindControl("lnkProjectNumber");
                HyperLink lnkIPFGenerated = (HyperLink)e.Item.FindControl("lnkIPFGenerated");

                Label lblProjectStatus = (Label)e.Item.FindControl("lblProjectStatus");
                Label lblSubmitter = (Label)e.Item.FindControl("lblSubmitter");

                ImageButton imgCreateIPF = (ImageButton)e.Item.FindControl("imgCreateIPF");
                ImageButton imgNeedsNew = (ImageButton)e.Item.FindControl("imgNeedsNew");
                ImageButton imgDeleteIPF = (ImageButton)e.Item.FindControl("imgDeleteIPF");
                ImageButton imgItemDetail = (ImageButton)e.Item.FindControl("imgItemDetail");

                Image imgStatus = (Image)e.Item.FindControl("imgStatus");

                if (!string.IsNullOrEmpty(SGSIPFItem.ProjectStatus))
                {
                    if (SGSIPFItem.ProjectStatus == GlobalConstants.WORKFLOWPHASE_SrOBMInitialReview)
                    {
                        SGSIPFItem.ProjectStatus = GlobalConstants.WORKFLOWPHASE_PMInitialReview;
                    }
                    else
                    {
                        SGSIPFItem.ProjectStatus = SGSIPFItem.ProjectStatus.Replace("OBM", "PM");
                    }
                }

                lblProjectStatus.Text = SGSIPFItem.ProjectStatus;
                lblProjectStatus.ToolTip = SGSIPFItem.ProjectStatus;

                if (SGSIPFItem.SubmittedDate != DateTime.MinValue)
                {
                    lblSubmitter.Text = Utilities.GetPersonFieldForDisplay(SGSIPFItem.Initiator) + ": " + SGSIPFItem.SubmittedDate.ToString("MM/dd/yyyy");
                }
                else
                {
                    lblSubmitter.Text = Utilities.GetPersonFieldForDisplay(SGSIPFItem.Initiator);
                }
                txtSAPItemNumber.Text = SGSIPFItem.SAPItemNumber;
                txtSAPItemNumber.ToolTip = SGSIPFItem.SAPItemNumber;

                txtSAPDescription.Text = SGSIPFItem.SAPDescription;
                txtSAPDescription.ToolTip = SGSIPFItem.SAPDescription;

                bool requCheck = requirementsCheck(SGSIPFItem);

                if (!string.IsNullOrEmpty(SGSIPFItem.Customer))
                {
                    Utilities.SetDropDownValueMatchWithoutCodes(SGSIPFItem.Customer, ddlCustomer, this.Page);
                    ddlCustomer.ToolTip = SGSIPFItem.Customer;
                }

                if (!string.IsNullOrEmpty(SGSIPFItem.TBDIndicator))
                {
                    Utilities.SetDropDownValue(SGSIPFItem.TBDIndicator, ddlTBDIndicator, this.Page);
                }
                if (SGSIPFItem.CreateIPFBtn == true)
                {
                    lnkProjectNumber.Visible = false;
                    lnkIPFGenerated.Visible = false;
                    imgCreateIPF.Visible = true;
                    if (hdnStage.Value == GlobalConstants.WORKFLOWPHASE_OnHold)
                    {
                        imgCreateIPF.Enabled = false;
                    }
                    imgNeedsNew.Visible = false;
                    imgDeleteIPF.Visible = true;
                    imgItemDetail.CommandArgument = SGSIPFItem.ProjectNumber.ToString();
                    if (SGSIPFItem.TBDIndicator == "No")
                    {
                        imgStatus.ImageUrl = "/_layouts/15/Ferrara.Compass/img/greenCircle.png";
                        imgStatus.CssClass = "green";
                    }
                    else if (SGSIPFItem.TBDIndicator == "Yes" && requirementsCheckDT(SGSIPFItem) && requCheck)
                    {
                        imgStatus.ImageUrl = "/_layouts/15/Ferrara.Compass/img/greenCircle.png";
                        imgStatus.CssClass = "green";
                    }
                    else
                    {
                        imgStatus.ImageUrl = "/_layouts/15/Ferrara.Compass/img/redCircle.png";
                        imgStatus.CssClass = "red";
                        imgCreateIPF.CssClass = "createIPF hideItem";
                    }

                    txtSAPDescription.Enabled = false;
                    txtSAPDescription.ReadOnly = true;
                }
                else
                {
                    lnkProjectNumber.Text = SGSIPFItem.ProjectNumber;
                    lnkProjectNumber.NavigateUrl = "/Pages/ProjectStatus.aspx?ProjectNo=" + SGSIPFItem.ProjectNumber;
                    lnkProjectNumber.Target = "_blank";
                    lnkIPFGenerated.Visible = true;
                    lnkIPFGenerated.NavigateUrl = "/Pages/ItemProposal.aspx?ProjectNo=" + SGSIPFItem.ProjectNumber;
                    imgCreateIPF.Visible = false;
                    imgNeedsNew.Visible = true;
                    if (SGSIPFItem.TBDIndicator == "Yes")
                    {
                        if (!SGSIPFItem.NeedsNewBtn)
                        {
                            if (hdnStage.Value == GlobalConstants.WORKFLOWPHASE_OnHold)
                            {
                                imgNeedsNew.Enabled = false;
                            }
                            imgNeedsNew.ImageUrl = "/_layouts/15/Ferrara.Compass/img/Request4.png";
                        }
                        else
                        {
                            imgNeedsNew.ImageUrl = "/_layouts/15/Ferrara.Compass/img/Requested.png";
                            imgNeedsNew.Enabled = false;
                            imgNeedsNew.OnClientClick = "return false;";
                        }
                    }
                    else
                    {
                        imgNeedsNew.Visible = false;
                    }
                    imgDeleteIPF.Visible = false;
                    imgItemDetail.CommandArgument = SGSIPFItem.CompassListItemId.ToString();
                    imgStatus.ImageUrl = "/_layouts/15/Ferrara.Compass/img/greenCircle.png";
                    imgStatus.CssClass = "green";

                    txtSAPItemNumber.Enabled = false;
                    txtSAPDescription.Enabled = false;
                    ddlTBDIndicator.Enabled = false;
                    ddlCustomer.Enabled = false;
                    ddlMaterialGroup1Brand.Enabled = false;
                    txtSAPItemNumber.ReadOnly = true;
                    txtSAPDescription.ReadOnly = true;

                }

                if (hdnProjectType.Value == GlobalConstants.PROJECTTYPE_SimpleNetworkMove || hdnProjectType.Value == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
                {
                    ddlTBDIndicator.Enabled = false;
                    //ddlTBDIndicator.SelectedItem.Value = "N";
                }
                hdnCompassListItemId.Value = SGSIPFItem.CompassListItemId.ToString();
                hdnProjectNumber.Value = SGSIPFItem.ProjectNumber;
                hdnPMTProjectListItemId.Value = SGSIPFItem.StageGateProjectListItemId.ToString();
                hdnPopupValidated.Value = requCheck.ToString().ToLower();
                hdnIPFGenerated.Value = SGSIPFItem.CreateIPFBtn.ToString().ToLower();
                hdnSortOrder.Value = SGSIPFItem.GenerateIPFSortOrder.ToString();

                hdnProductHierarchyLevel1.Value = SGSIPFItem.ProductHierarchyLevel1;
                hdnManuallyCreateSAPDescription.Value = SGSIPFItem.ManuallyCreateSAPDescription;
                hdnProductHierarchyLevel2.Value = SGSIPFItem.ProductHierarchyLevel2;
                hdnMaterialGroup5.Value = SGSIPFItem.MaterialGroup5PackType;
                hdnCustomerSpecific.Value = SGSIPFItem.CustomerSpecific;
                hdnCustomer.Value = SGSIPFItem.Customer;
            }
        }
        protected void rptChildProjects_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {

            saveFields();
            HiddenField hdnPMTProjectListItemId = (HiddenField)e.Item.FindControl("hdnPMTProjectListItemId");
            HiddenField hdnCompassListItemId = (HiddenField)e.Item.FindControl("hdnCompassListItemId");
            HiddenField hdnProjectNumber = (HiddenField)e.Item.FindControl("hdnProjectNumber");
            HyperLink IPFGeneratedLink = (HyperLink)e.Item.FindControl("lnkIPFGenerated");
            bool IPFGenerated = IPFGeneratedLink.Visible;

            int id = 0;

            if (e.CommandName.ToLower() != "sapnomenclature")
            {
                id = Convert.ToInt32(e.CommandArgument.ToString());
            }

            if (e.CommandName.ToLower() == "delete")
            {
                stageGateGeneralService.deleteChildProjectDetails(id);
                LoadIPFTable();
            }
            else if (e.CommandName.ToLower() == "generate")
            {
                ItemProposalItem movingIPF = stageGateGeneralService.GetTempIPFItem(id, StageGateItemId);
                StageGateCreateProjectItem parentItem = stageGateCreateProjectService.GetStageGateProjectItem(StageGateItemId);
                movingIPF = mergeParentChild(movingIPF, parentItem);
                string ChildProjectNo = Utilities.GetNextChildProjectNumber(ProjectNumber);
                movingIPF.ProjectNumber = ChildProjectNo;
                movingIPF.ParentProjectNumber = ProjectNumber;
                movingIPF.ProjectStatus = "IPF In Progress";
                movingIPF.NewIPF = "Yes";
                string PMTWorkflowVersion = configurationService.GetConfiguration(SystemConfiguration.PMTWorkflowVersion);
                movingIPF.PMTWorkflowVersion = string.IsNullOrEmpty(PMTWorkflowVersion) ? 9999 : Convert.ToInt32(PMTWorkflowVersion);
                int compassID = IPFService.InsertItemProposalItem(movingIPF);
                //Compass List 2
                IPFService.InsertCompassList2(movingIPF);
                //Insert Approval Item
                ApprovalItem approvalItem = new ApprovalItem();
                approvalItem.CompassListItemId = compassID;
                approvalItem.ModifiedDate = DateTime.Now.ToString();
                approvalItem.ModifiedBy = SPContext.Current.Web.CurrentUser.Name;
                IPFService.InsertApprovalItem(approvalItem, ChildProjectNo);
                // Insert Project Decision Info
                IPFService.InsertProjectDecisionItem(compassID, ChildProjectNo);
                // Insert Email Logging Info
                IPFService.InsertEmailLoggingItem(compassID, ChildProjectNo);
                // Insert Workflow Status Info
                IPFService.InsertWorkflowStatusItem(compassID, ChildProjectNo);
                stageGateGeneralService.deleteChildProjectDetails(id);
                LoadIPFTable();
                string url = Utilities.RedirectPageForm(GlobalConstants.PAGE_ItemProposal, ChildProjectNo);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('" + url + "','_newtab');", true);

            }
            else if (e.CommandName.ToLower() == "workflow")
            {
                workflowService.StartSpecificWorkflow(id, "2b - Preliminary SAP Initial Item Setup Workflow");
                stageGateGeneralService.SetPrelimStartDate(id, DateTime.Now);
                LoadIPFTable();
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "UpdateImage", "UpdateImage('" + ((ImageButton)e.CommandSource).ClientID + "');", true);

            }
            else if (e.CommandName.ToLower() == "loadcontrol")
            {
                ucSGSItemDetailInfo ctrl = (ucSGSItemDetailInfo)Page.LoadControl(@"~/_controltemplates/15/Ferrara.Compass/ucSGSItemDetailInfo.ascx");
                ctrl.IPFGenerated = IPFGenerated;
                ctrl.childProjectNo = hdnProjectNumber.Value;
                ctrl.StageGateItemId = Convert.ToInt32(hdnPMTProjectListItemId.Value);
                ctrl.ID = "ucSGSItemDetailInfo";
                ctrl.StageGateChildItemId = id;
                ctrl.firstLoad = true;

                hdnUCLoaded.Value = "true";
                hdnUCLoadedchildProjectNo.Value = hdnProjectNumber.Value;
                hdnUCLoadedIPFGenerated.Value = IPFGenerated.ToString();
                hdnUCLoadedchildProjectNo.Value = hdnProjectNumber.Value;
                hdnUCLoadedPMTItemId.Value = hdnPMTProjectListItemId.Value;
                hdnUCLoadedId.Value = id.ToString();
                //var phPage = (PlaceHolder)this.Parent.FindControl("phMsg");
                //phPage.Visible = false;
                // Add messages to page

                phMsg.Controls.Clear();
                phMsg.Controls.Add(ctrl);
            }
            else if (e.CommandName.ToLower() == "sapnomenclature")
            {
                DropDownList ddlTBDIndicator = ((DropDownList)e.Item.FindControl("ddlTBDIndicator"));
                TextBox txtSAPDescription = (TextBox)e.Item.FindControl("txtSAPDescription");
                DropDownList drpMaterialGroup1Brand = ((DropDownList)e.Item.FindControl("ddlMaterialGroup1Brand"));
                DropDownList ddlCustomer = ((DropDownList)e.Item.FindControl("ddlCustomer"));
                HiddenField hdnProductHierarchyLevel1 = (HiddenField)e.Item.FindControl("hdnProductHierarchyLevel1");
                HiddenField hdnManuallyCreateSAPDescription = (HiddenField)e.Item.FindControl("hdnManuallyCreateSAPDescription");
                HiddenField hdnProductHierarchyLevel2 = (HiddenField)e.Item.FindControl("hdnProductHierarchyLevel2");
                HiddenField hdnMaterialGroup5 = (HiddenField)e.Item.FindControl("hdnMaterialGroup5");
                HiddenField hdnCustomerSpecific = (HiddenField)e.Item.FindControl("hdnCustomerSpecific");

                var SAPNomenclature = false;
                if (ddlTBDIndicator.SelectedItem.Text == "Yes")
                {
                    SAPNomenclature = true;
                    if (hdnProductHierarchyLevel1.Value == GlobalConstants.PRODUCT_HIERARCHY1_CoMan)
                    {
                        if (hdnManuallyCreateSAPDescription.Value == "Yes")
                        {
                            SAPNomenclature = false;
                        }
                    }
                }

                if (SAPNomenclature)
                {
                    var TBD = "";
                    var Brand = "";
                    var Season = "";
                    var CustomerSpecific = "";
                    var PkgType = "";

                    //TBD
                    TBD = "TBD ";

                    #region Brand
                    var BrandSelection = drpMaterialGroup1Brand.SelectedItem.Text;

                    if (BrandSelection != "Select...")
                    {
                        Brand = BrandSelection.Substring(BrandSelection.LastIndexOf("(") + 1, (BrandSelection.LastIndexOf(")") - BrandSelection.LastIndexOf("(")) - 1);
                    }

                    Brand = string.IsNullOrEmpty(Brand) ? "" : Brand + " ";
                    #endregion
                    #region Season
                    if (hdnProductHierarchyLevel1.Value == "Seasonal (000000023)")
                    {
                        var SeasonSelection = hdnProductHierarchyLevel2.Value;

                        if (SeasonSelection == "VALENTINE\'S (000000008)" || SeasonSelection == "VALENTINE\'S (000000008)" || SeasonSelection == "VALENTINE\'S DAY (000000008)")
                        {
                            Season = "VDY ";
                        }
                        else if (SeasonSelection == "EASTER (000000003)" || SeasonSelection == "EASTER BULK (000000004)")
                        {
                            Season = "ESR ";
                        }
                        else if (SeasonSelection == "HALLOWEEN (000000005)" || SeasonSelection == "HALLOWEEN BULK (000000006)")
                        {
                            Season = "HWN ";
                        }
                        else if (SeasonSelection == "CHRISTMAS (000000001)" || SeasonSelection == "CHRISTMAS BULK (000000002)")
                        {
                            Season = "HLY ";
                        }
                        else if (SeasonSelection == "HOLIDAY (000000001)")
                        {
                            Season = "HLY ";
                        }
                        else if (SeasonSelection == "SUMMER(000000007)")
                        {
                            Season = "SMR ";
                        }
                    }
                    #endregion
                    #region Customer specific
                    if (hdnCustomerSpecific.Value == "Customer Specific")
                    {
                        var CustomerspecificSelection = ddlCustomer.SelectedItem.Text;

                        if (CustomerspecificSelection != "Select...")
                        {
                            CustomerSpecific = CustomerspecificSelection.Substring(CustomerspecificSelection.LastIndexOf("(") + 1, (CustomerspecificSelection.LastIndexOf(")") - CustomerspecificSelection.LastIndexOf("(")) - 1);
                        }
                    }
                    CustomerSpecific = string.IsNullOrEmpty(CustomerSpecific) ? "" : CustomerSpecific + " ";

                    #endregion
                    #region Pkg Type
                    if (hdnMaterialGroup5.Value.Contains("(DOY)"))
                    {
                        PkgType = " DOY";
                    }
                    else if (hdnMaterialGroup5.Value.Contains("(SHP)"))
                    {
                        PkgType = " SHP";
                    }
                    #endregion

                    var Description = TBD + Brand + Season + CustomerSpecific + PkgType;
                    txtSAPDescription.Text = Description;
                    saveFields();
                    LoadIPFTable();
                }
            }
        }
        private ItemProposalItem mergeParentChild(ItemProposalItem movingIPF, StageGateCreateProjectItem parentItem)
        {
            List<string> users = new List<string>();
            movingIPF.ProjectLeader = parentItem.ProjectLeader;
            users.AddRange(movingIPF.ProjectLeader.Split(';').ToList());
            movingIPF.ProjectLeaderName = parentItem.ProjectLeaderName;
            movingIPF.PM = parentItem.ProjectManager;
            users.AddRange(movingIPF.PM.Split(';').ToList());
            movingIPF.PMName = parentItem.ProjectManagerName;
            movingIPF.Marketing = parentItem.Marketing;
            users.AddRange(movingIPF.Marketing.Split(';').ToList());
            movingIPF.MarketingName = parentItem.MarketingName;
            movingIPF.SrProjectManager = parentItem.SeniorProjectManager;
            users.AddRange(movingIPF.SrProjectManager.Split(';').ToList());
            movingIPF.SrProjectManagerName = parentItem.SeniorProjectManagerName;
            movingIPF.QA = parentItem.QAInnovation;
            users.AddRange(movingIPF.QA.Split(';').ToList());
            movingIPF.QAName = parentItem.QAInnovationName;
            movingIPF.InTech = parentItem.InTech;
            users.AddRange(movingIPF.InTech.Split(';').ToList());
            movingIPF.InTechName = parentItem.InTechName;
            movingIPF.InTechRegulatory = parentItem.InTechRegulatory;
            users.AddRange(movingIPF.InTechRegulatory.Split(';').ToList());
            movingIPF.InTechRegulatoryName = parentItem.InTechRegulatoryName;
            movingIPF.RegulatoryQA = parentItem.RegulatoryQA;
            users.AddRange(movingIPF.RegulatoryQA.Split(';').ToList());
            movingIPF.RegulatoryQAName = parentItem.RegulatoryQAName;
            movingIPF.PackagingEngineering = parentItem.PackagingEngineering;
            users.AddRange(movingIPF.PackagingEngineering.Split(';').ToList());
            movingIPF.PackagingEngineeringName = parentItem.PackagingEngineeringName;
            movingIPF.SupplyChain = parentItem.SupplyChain;
            users.AddRange(movingIPF.SupplyChain.Split(';').ToList());
            movingIPF.SupplyChainName = parentItem.SupplyChainName;
            movingIPF.Finance = parentItem.Finance;
            users.AddRange(movingIPF.Finance.Split(';').ToList());
            movingIPF.FinanceName = parentItem.FinanceName;
            movingIPF.Sales = parentItem.Sales;
            users.AddRange(movingIPF.Sales.Split(';').ToList());
            movingIPF.SalesName = parentItem.SalesName;
            movingIPF.Manufacturing = parentItem.Manufacturing;
            users.AddRange(movingIPF.Manufacturing.Split(';').ToList());
            movingIPF.ManufacturingName = parentItem.ManufacturingName;
            movingIPF.OtherTeamMembers = parentItem.OtherMember;
            users.AddRange(movingIPF.OtherTeamMembers.Split(';').ToList());
            movingIPF.OtherTeamMembersName = parentItem.OtherMemberName;
            movingIPF.LifeCycleManagement = parentItem.LifeCycleManagement;
            users.AddRange(movingIPF.Marketing.Split(';').ToList());
            movingIPF.LifeCycleManagementName = parentItem.LifeCycleManagementName;
            movingIPF.PackagingProcurement = parentItem.PackagingProcurement;
            users.AddRange(movingIPF.PackagingProcurement.Split(';').ToList());
            movingIPF.PackagingProcurementName = parentItem.PackagingProcurementName;
            movingIPF.ExtManufacturingProc = parentItem.ExtMfgProcurement;
            users.AddRange(movingIPF.ExtManufacturingProc.Split(';').ToList());
            movingIPF.ExtManufacturingProcName = parentItem.ExtMfgProcurementName;
            movingIPF.FirstShipDate = parentItem.RevisedShipDate == DateTime.MinValue ? parentItem.DesiredShipDate : parentItem.RevisedShipDate;
            movingIPF.Initiator = SPContext.Current.Web.CurrentUser.ID.ToString() + ";#" + SPContext.Current.Web.CurrentUser.LoginName;
            users.Add(movingIPF.Initiator);
            movingIPF.InitiatorName = SPContext.Current.Web.CurrentUser.Name.ToString();
            movingIPF.TestProject = parentItem.TestProject;
            List<string> finalUsers = new List<string>();
            foreach (string user in users)
            {
                string userTrimmed = Regex.Replace(user, "[^0-9.]", "");
                int id;
                if (int.TryParse(userTrimmed, out id))
                {
                    if (id > 0)
                    {
                        finalUsers.Add(id.ToString());
                    }
                }
            }
            List<string> finalUsersDistinct = finalUsers.Select(x => x).Distinct().ToList<string>();
            movingIPF.AllUsers = "," + String.Join(",", finalUsersDistinct) + ",";

            if (parentItem.ProjectType != "Multiple")
            {
                movingIPF.ProjectType = parentItem.ProjectType;
                movingIPF.NewFormula = parentItem.NewBaseFormula;
                movingIPF.NewShape = parentItem.NewShape;
                movingIPF.NewFlavorColor = parentItem.NewFlavorColor;
                movingIPF.NewNetWeight = parentItem.NewNetWeight;
            }
            if (parentItem.ProjectTypeSubCategory != "Multiple")
            {
                movingIPF.ProjectTypeSubCategory = parentItem.ProjectTypeSubCategory;
            }
            return movingIPF;
        }
        private bool CheckProjectNumber()
        {
            if (!string.IsNullOrWhiteSpace(ProjectNumber))
            {
                StageGateItemId = Utilities.GetItemIdByProjectNumberFromStageGateProjectList(ProjectNumber);
            }

            // Store Id in Hidden field
            this.hdnPMTListItemId.Value = StageGateItemId.ToString();
            return true;
        }
        private bool CheckWriteAccess()
        {
            if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_StageGateCreateProject.ToLower()))
            {
                if (userManagementService.HasWriteAccess(CompassForm.StageGateCreateProject))
                {
                    return true;
                }
            }
            return false;
        }

        protected void UpDateSAPNomenclature(object sender, EventArgs e)
        {
            rptChildProjects_ItemCommand(rptChildProjects, new RepeaterCommandEventArgs((RepeaterItem)((DropDownList)sender).NamingContainer, sender, new CommandEventArgs("SAPNomenclature", null)));

        }
        #endregion
        #region Private Methods
        private void saveFields()
        {
            foreach (RepeaterItem item in rptChildProjects.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField hdnCompassListItemId = (HiddenField)item.FindControl("hdnCompassListItemId");
                    HiddenField hdnPMTProjectListItemId = (HiddenField)item.FindControl("hdnPMTProjectListItemId");
                    HiddenField hdnProductHierarchyLevel1 = (HiddenField)item.FindControl("hdnProductHierarchyLevel1");
                    HiddenField hdnSortOrder = (HiddenField)item.FindControl("hdnSortOrder");
                    int CompassListItemId = Convert.ToInt32(hdnCompassListItemId.Value);
                    int SortOrder = Convert.ToInt32(hdnSortOrder.Value);
                    ItemProposalItem IPF = new ItemProposalItem();
                    IPF.ProjectNumber = ((HiddenField)item.FindControl("hdnProjectNumber")).Value;

                    IPF.StageGateProjectListItemId = Convert.ToInt32(hdnPMTProjectListItemId.Value);
                    IPF.TBDIndicator = ((DropDownList)item.FindControl("ddlTBDIndicator")).SelectedItem.Text;
                    IPF.SAPItemNumber = ((TextBox)item.FindControl("txtSAPItemNumber")).Text;
                    IPF.SAPDescription = ((TextBox)item.FindControl("txtSAPDescription")).Text;
                    IPF.MaterialGroup1Brand = ((DropDownList)item.FindControl("ddlMaterialGroup1Brand")).SelectedItem.Text;
                    var customerList = ((DropDownList)item.FindControl("ddlCustomer")).SelectedItem.Text.Split('(');
                    if (customerList != null)
                    {
                        IPF.Customer = customerList[0].Trim();
                    }

                    IPF.GenerateIPFSortOrder = SortOrder;
                    if (CompassListItemId == 0)
                    {
                        stageGateGeneralService.upsertTempIPFList(IPF);
                    }
                }
            }
        }
        protected void saveData_Click(object sender, EventArgs e)
        {
            saveFields();
        }
        protected void addTempIPF_Click(object sender, EventArgs e)
        {
            try
            {
                saveFields();
                List<ItemProposalItem> childProjectData = dashboardService.getRequestedChildProjectDetails(StageGateItemId);
                if (childProjectData.Count <= 0)
                {
                    dashboardService.setGenerateIPFStartDate(Convert.ToInt32(hdnPMTListItemId.Value));
                }
                StageGateCreateProjectItem stageGateItem = stageGateCreateProjectService.GetStageGateProjectItem(StageGateItemId);
                ItemProposalItem insertItem = getParentDetailedIPF(stageGateItem);
                insertItem.StageGateProjectListItemId = Convert.ToInt32(hdnPMTListItemId.Value);
                insertItem.ProjectNumber = "0";
                insertItem.ProductHierarchyLevel1 = stageGateItem.LineOfBisiness;
                insertItem.ProductHierarchyLevel2 = stageGateItem.PHL2;
                insertItem.GenerateIPFSortOrder = childProjectData.Count;
                int tempItemId = stageGateGeneralService.insertTempIPFList(insertItem);

                insertItem.ProjectNumber = tempItemId.ToString();
                childProjectData.Add(insertItem);
                rptChildProjects.DataSource = childProjectData.OrderBy(x => x.GenerateIPFSortOrder);
                rptChildProjects.DataBind();
            }
            catch (Exception error)
            {
                ErrorSummary.AddError(error.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "SGS Generate IPFs Form" + ": " + error.Message);
                exceptionService.Handle(LogCategory.CriticalError, error, "SGS Generate IPFs Form", "addTempIPF_Click");
            }

        }
        #endregion
        private ItemProposalItem getParentDetailedIPF(StageGateCreateProjectItem stageGateItem)
        {
            ItemProposalItem newIPFItem = new ItemProposalItem();
            //newIPFItem.ProjectType = stageGateItem.ProjectType;
            if (stageGateItem.ProjectType == GlobalConstants.PROJECTTYPE_SimpleNetworkMove || stageGateItem.ProjectType == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
            {
                newIPFItem.TBDIndicator = "No";
            }
            else
            {
                newIPFItem.TBDIndicator = "Select...";
            }
            if (stageGateItem.ProjectType != "Multiple")
            {
                newIPFItem.ProjectType = stageGateItem.ProjectType;
            }
            if (stageGateItem.Brand != "Multiple")
            {
                newIPFItem.MaterialGroup1Brand = stageGateItem.Brand;
            }
            newIPFItem.ProductHierarchyLevel2 = stageGateItem.PHL2;
            if (stageGateItem.LineOfBisiness != "Multiple")
            {
                newIPFItem.ProductHierarchyLevel1 = stageGateItem.LineOfBisiness;
            }
            if (stageGateItem.ProjectTypeSubCategory != "Multiple")
            {
                newIPFItem.ProjectTypeSubCategory = stageGateItem.ProjectTypeSubCategory;
            }
            newIPFItem.RevisedFirstShipDate = stageGateItem.RevisedShipDate;
            newIPFItem.CreateIPFBtn = true;
            newIPFItem.CreateIPFBtn = true;
            newIPFItem.NeedsNewBtn = false;
            return newIPFItem;
        }
        private bool requirementsCheck(ItemProposalItem item)
        {
            bool completed = true;
            //Rules for all components
            if (item.TBDIndicator == "Select...")
            {
                completed = false;
            }
            else if (item.TBDIndicator == "Yes")
            {
                if (string.IsNullOrEmpty(item.SAPItemNumber))
                {
                    completed = false;
                }
                else if (string.IsNullOrEmpty(item.SAPDescription))
                {
                    completed = false;
                }
                else if (item.ProductHierarchyLevel1 == "Select..." || string.IsNullOrEmpty(item.ProductHierarchyLevel1))
                {
                    completed = false;
                }
                else if (item.ProductHierarchyLevel2 == "Select..." || string.IsNullOrEmpty(item.ProductHierarchyLevel2))
                {
                    completed = false;
                }
                else if (item.MaterialGroup1Brand == "Select..." || string.IsNullOrEmpty(item.MaterialGroup1Brand))
                {
                    completed = false;
                }
                else if (item.MaterialGroup4ProductForm == "Select..." || string.IsNullOrEmpty(item.MaterialGroup4ProductForm))
                {
                    completed = false;
                }
                else if (item.MaterialGroup5PackType == "Select..." || string.IsNullOrEmpty(item.MaterialGroup5PackType))
                {
                    completed = false;
                }
                else if (item.RequireNewUPCUCC == "Select..." || string.IsNullOrEmpty(item.RequireNewUPCUCC))
                {
                    completed = false;
                }
                else if (item.SAPBaseUOM == "Select..." || string.IsNullOrEmpty(item.SAPBaseUOM))
                {
                    completed = false;
                }
                /*else if (item.RequireNewUPCUCC == "No")
                {
                    if (string.IsNullOrEmpty(item.UnitUPC))
                    {
                        completed = false;
                    }
                    else if (item.SAPBaseUOM == "Select..." || string.IsNullOrEmpty(item.SAPBaseUOM))
                    {
                        completed = false;
                    }
                    else if (item.SAPBaseUOM == "PAL" && string.IsNullOrEmpty(item.PalletUCC))
                    {
                        completed = false;
                    }
                    else if (item.SAPBaseUOM == "CS" && string.IsNullOrEmpty(item.CaseUCC))
                    {
                        completed = false;
                    }
                }
                else if (item.RequireNewUPCUCC == "Yes")
                {
                    if (item.RequireNewUnitUPC == "Select..." || string.IsNullOrEmpty(item.RequireNewUnitUPC))
                    {
                        completed = false;
                    }
                    else if (item.RequireNewUnitUPC == "No" && string.IsNullOrEmpty(item.UnitUPC))
                    {
                        completed = false;
                    }
                    else if (item.RequireNewDisplayBoxUPC == "Select..." || string.IsNullOrEmpty(item.RequireNewDisplayBoxUPC))
                    {
                        completed = false;
                    }
                    else if (item.RequireNewDisplayBoxUPC == "No" && string.IsNullOrEmpty(item.DisplayBoxUPC))
                    {
                        completed = false;
                    }
                    else if (item.SAPBaseUOM == "Select..." || string.IsNullOrEmpty(item.SAPBaseUOM))
                    {
                        completed = false;
                    }
                    else if (item.SAPBaseUOM == "PAL")
                    {
                        if (item.RequireNewPalletUCC == "Select..." || string.IsNullOrEmpty(item.RequireNewPalletUCC))
                        {
                            completed = false;
                        }
                        else if (item.RequireNewPalletUCC == "No" && string.IsNullOrEmpty(item.PalletUCC))
                        {
                            completed = false;
                        }
                    }
                    else if (item.SAPBaseUOM == "CS")
                    {
                        if (item.RequireNewCaseUCC == "Select..." || string.IsNullOrEmpty(item.RequireNewCaseUCC))
                        {
                            completed = false;
                        }
                        else if (item.RequireNewCaseUCC == "No" && string.IsNullOrEmpty(item.CaseUCC))
                        {
                            completed = false;
                        }
                    }
                }*/
                /*else if (item.CustomerSpecific == "Select..." || string.IsNullOrEmpty(item.CustomerSpecific))
                {
                    completed = false;
                }
                else if (item.CustomerSpecific == "Customer" || item.CustomerSpecific == "Pricelist")
                {
                    if (item.Customer == "Select..." || string.IsNullOrEmpty(item.Customer))
                    {
                        completed = false;
                    }
                }
                else if (item.CustomerSpecific == "Channel")
                {
                    if (item.Channel == "Select..." || string.IsNullOrEmpty(item.Channel))
                    {
                        completed = false;
                    }
                }*/
            }
            return completed;
        }
        private bool requirementsCheckDT(ItemProposalItem item)
        {
            bool completed = true;
            //Rules for all components
            if (item.TBDIndicator == "Select..." || string.IsNullOrEmpty(item.TBDIndicator))
            {
                completed = false;
            }
            else if (string.IsNullOrEmpty(item.SAPItemNumber))
            {
                completed = false;
            }
            else if (string.IsNullOrEmpty(item.SAPDescription))
            {
                completed = false;
            }
            else if (item.MaterialGroup1Brand == "Select..." || string.IsNullOrEmpty(item.MaterialGroup1Brand))
            {
                completed = false;
            }
            else if (item.Customer == "Select..." || string.IsNullOrEmpty(item.Customer))
            {
                completed = false;
            }
            return completed;
        }
        private void ReloadBrand(string productHierarhcyLevel2, DropDownList ddlBrand)
        {
            if ((!string.IsNullOrEmpty(productHierarhcyLevel2)) && (!string.Equals(productHierarhcyLevel2, "Select...")))
            {
                Utilities.BindDropDownItemsByValueAndColumn(ddlBrand, GlobalConstants.LIST_MaterialGroup1Lookup, "ParentPHL2", productHierarhcyLevel2, webUrl);
                ddlBrand.SelectedIndex = -1;
            }
            else
            {
                ddlBrand.Items.Clear();
                ListItem li = new ListItem();
                li.Text = "Select...";
                li.Value = "-1";
                ddlBrand.Items.Add(li);
            }
        }
    }
}
