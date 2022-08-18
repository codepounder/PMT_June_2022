using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Classes;
using Microsoft.Practices.Unity;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Ferrara.Compass.WebParts.RequestRecipeSpecForm
{
    [ToolboxItemAttribute(false)]
    public partial class RequestRecipeSpecForm : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public RequestRecipeSpecForm()
        {
        }
        #region member variables
        private string webUrl;
        public string requestTypeForForm;
        public string selectedSemiItems;
        private IExceptionService exceptionService;
            
        private IUtilityService utilityService;
        private IWorkflowService workflowService;
        private INotificationService notificationService;
        private IUserManagementService userMgmtService;
      
        private int StageGateListItemId = 0;
        #endregion
        #region Properties

        private string ProjectNumber
        {
            get
            {
                if (Page.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo] != null)
                    return Page.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo];
                return string.Empty;
            }
        }

        #endregion
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            userMgmtService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
        }
        #region Private Methods
        
        private void InitializeScreen()
        {
            // If user does not belong to a valid group for the page, inform them that they do not hvae access rights
            if (!userMgmtService.HasReadAccess(CompassForm.RequestRecipe))
            {
                this.divAccessDenied.Visible = true;
            }

            // If user does not have rights to save/submit the page, disable the Save and Submit buttons
            if (!userMgmtService.HasWriteAccess(CompassForm.RequestRecipe))
            {
                this.btnSave.Enabled = false;
                this.btnSubmit.Enabled = false;
            }

            //string workflowPhase = utilityService.GetWorkflowPhase(StageGateListItemId);
            //if ((string.Equals(workflowPhase, WorkflowStep.OnHold.ToString())) || (string.Equals(workflowPhase, WorkflowStep.Cancelled.ToString())) ||
            //    (string.Equals(workflowPhase, WorkflowStep.Completed.ToString())))
            //{
            //    this.btnSave.Enabled = false;
            //    this.btnSubmit.Enabled = false;
            //}
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
           
            webUrl = SPContext.Current.Web.Url;
          
            string requestType = Page.Request.QueryString["RequestType"];
            if (requestType == null || requestType.ToLower() == "fg")
            {
               
                requestTypeForForm = "FG";

            }
            else
            {
                
                requestTypeForForm = "Transfer Semi";
            }
            try
            {
                this.divAccessDenied.Visible = false;
                this.divAccessRequest.Visible = false;

                // Check for a valid project number
                if (!CheckProjectNumber())
                    return;

                Utilities.BindDropDownItems(drpMakeLocation, GlobalConstants.LIST_ManufacturingLocationsLookup, webUrl);
                Utilities.BindDropDownItems(ddlMakePackSemi, GlobalConstants.LIST_ManufacturingLocationsLookup, webUrl);

              //  LoadFormData();
                InitializeScreen();
                if (hdnProjectType.Value.Contains("Renovations"))
                {
                    dvMain.Visible = false;
                    dvMsg.Visible = true;
                }

                if (requestType == null || requestType.ToLower() == "fg")
                {
                    this.divSemiSection.Visible = false;
                    this.divFGSection.Visible = true;
                    requestTypeForForm = "FG";

                }
                else
                {
                    this.divSemiSection.Visible = true;
                    this.divFGSection.Visible = false;
                    requestTypeForForm = "Transfer Semi";
                }
            }
            catch (Exception exception)
            {
                ErrorSummary.AddError(exception.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.TradePromo.ToString() + ": " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.TradePromo.ToString(), "Page_Load");
            } 
        }

        protected void btnClick()
        {
            //test
        }
        private bool CheckProjectNumber()
        {
            if (!string.IsNullOrWhiteSpace(ProjectNumber))
            {
                StageGateListItemId = Utilities.GetItemIdByProjectNumberFromStageGateProjectList(ProjectNumber);
            }

            // Store Id in Hidden field
            this.hdnStageGateProjectListItemId.Value = StageGateListItemId.ToString();
            return true;
        }
        protected void btnAddMakePackPlantFG_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptMakePackFGRepeater, "MakePackName", drpMakeLocation);
        }
        protected void btnAddMakePackPlantSemi_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptMakePackSemi, "MakePackNameSemi", ddlMakePackSemi);
        }
        private void AddTeamMemberButtonClick(Repeater repeaterName, string MemberName, DropDownList ddlName)
        {
            if (ddlName.SelectedItem.Value != "-1")
            {
                AddMembers_New(repeaterName, string.Concat("txt", MemberName), ddlName.SelectedItem.Text, ddlName.SelectedItem.Value, string.Concat("hdnDeletedStatusFor", MemberName));
                ddlName.SelectedIndex = -1;
               // CallUpdatePeopleEditorScriptFunction();
            }
        }
        private void AddMembers_New(Repeater rptMembers, string txtBoxName, string NewMember, string NewMemberLoginName, string hiddenStatusFieldName)
        {
            SPFieldUserValueCollection Members = new SPFieldUserValueCollection();
            List<int> NAList = new List<int>();
            Dictionary<int, string> BadNamesList = new Dictionary<int, string>();
            int Counter = 1;
            List<ListMembers> listMembers = new List<ListMembers>();
            listMembers.Add(new ListMembers() { MakePackName = NewMember, MakePackNameValue= NewMemberLoginName });

            foreach (RepeaterItem item in rptMembers.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox txtMember = (TextBox)item.FindControl(txtBoxName);
                    HiddenField hiddenStatusField = (HiddenField)item.FindControl(hiddenStatusFieldName);
                    TextBox txtMembersLoginName = (TextBox)item.FindControl(string.Concat(txtBoxName, "Value"));

                    if (hiddenStatusField.Value != "true")
                    {
                        try
                        {
                            if (CheckForNA(txtMember.Text))
                            {
                                listMembers.Add(new ListMembers() { MakePackName = "NA", MakePackNameValue = "NA" });
                            }
                            else
                            {
                                listMembers.Add(new ListMembers() { MakePackName = txtMember.Text, MakePackNameValue = txtMembersLoginName.Text });
                            }
                        }
                        catch (Exception exception)
                        {

                            ErrorSummary.AddError("Error occurred while adding new member: " + exception.Message, this.Page);
                            LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.IPF.ToString() + ": AddMembers_New: " + exception.Message);
                            exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.IPF.ToString(), "AddMembers_New");
                        }
                    }
                }
                Counter++;
            }

            rptMembers.DataSource = listMembers;
            rptMembers.DataBind();
        }
        // add new semi items to table : Mahipal 
        protected void btnAddSemi_Click(object sender, EventArgs e)
        {
            int Counter = 1;
            List<ListMembersForSemiAdd> listMembers = new List<ListMembersForSemiAdd>();
            List<SelectedMakePlants> selectedPlants = new List<SelectedMakePlants>();
            bool IsFormValid = true;

            if (ddlRequestNewExistingSemi.SelectedIndex == -1 || ddlRequestNewExistingSemi.SelectedIndex ==0)
            {
                ErrorSummary.AddError("Select Request for Transfer Semi Type ", this.Page);
                IsFormValid = false;
                return;
            } else 
            if(ddlRequestNewExistingSemi.SelectedItem.Text == "New")
            {
                if(string.IsNullOrEmpty(txtSemiDescription.Text))
                    {
                    ErrorSummary.AddError("Description for New Transfer Semi is Mandatory ", this.Page);
                   
                    //lblSemiDescriptionError.Text = "Description for New Transfer Semi is Mandtory";
                    IsFormValid = false;
                    return;
                }
            } else if(ddlRequestNewExistingSemi.SelectedItem.Text == "Existing")
            {
                if (string.IsNullOrEmpty(txtLikeSemiExistingNo.Text))
                {
                    ErrorSummary.AddError("Existing  Transfer Semi number is mandatory", this.Page);
                    IsFormValid = false;
                    return;
                }
            }
            foreach (RepeaterItem item in rptNewSemiComponent.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField hiddenStatusField = (HiddenField)item.FindControl("hdnDeletedStatusForAddNewSemi");
                    if (hiddenStatusField.Value != "true")
                    {
                        try
                        {
                            listMembers.Add(new ListMembersForSemiAdd()
                            {
                                SNo = Counter,
                                Description = ((Label)item.FindControl("txtDescriptionForSemi")).Text,
                                DescriptionValue = ((Label)item.FindControl("txtDescriptionForSemiValue")).Text,
                                ExistingNo = ((Label)item.FindControl("txtExistingNoValue")).Text,
                                ExistingNoValue = ((Label)item.FindControl("txtExistingNo")).Text,
                                RequestFor = ((Label)item.FindControl("txtRequestForSemi")).Text,
                                RequestForValue = ((Label)item.FindControl("txtRequestForSemiValue")).Text,
                                LikeSemiNo = ((Label)item.FindControl("txtLikeSemiNoValue")).Text,
                                LikeSemiNoValue = ((Label)item.FindControl("txtLikeSemiNo")).Text,
                                Location = ((Label)item.FindControl("txtLocation")).Text,
                                LocationValue = ((Label)item.FindControl("txtLocationValue")).Text,
                            });

                        }
                        catch (Exception exception)
                        {

                            ErrorSummary.AddError("Error occurred while adding new member: " + exception.Message, this.Page);
                            LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.IPF.ToString() + ": AddMembers_New: " + exception.Message);
                            exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.IPF.ToString(), "AddMembers_New");
                        }
                        Counter++;
                    }
                }

            }
            if (IsFormValid)
            {                
                if (ddlMakePackSemi.SelectedItem.Value != "-1")
                {
                    selectedPlants.Add(new SelectedMakePlants
                    {
                        SelectedPlantId = ddlMakePackSemi.SelectedItem.Value,
                         SelectedPlantName= ddlMakePackSemi.SelectedItem.Text
                    }) ;
                }
                foreach (RepeaterItem item in rptMakePackSemi.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        TextBox checkName = (TextBox)item.FindControl("txtMakePackNameSemi");
                        TextBox checkValue = (TextBox)item.FindControl("txtMakePackNameSemiValue");
                        selectedPlants.Add(new SelectedMakePlants
                        {
                             SelectedPlantName= checkName.Text,
                              SelectedPlantId= checkValue.Text
                        });
                    }
                }
                if (selectedPlants.Count > 0)
                {
                    foreach (var sp in selectedPlants)
                    {
                        listMembers.Add(new ListMembersForSemiAdd()
                        {
                            SNo = Counter,
                            Description = txtSemiDescription.Text,
                            DescriptionValue = txtSemiDescription.Text,
                            ExistingNo = txtLikeSemiExistingNo.Text,
                            ExistingNoValue = txtLikeSemiExistingNo.Text,
                            RequestFor = ddlRequestNewExistingSemi.SelectedItem.Text,
                            RequestForValue = ddlRequestNewExistingSemi.SelectedItem.Value,
                            LikeSemiNo = txtLikeSemiNo.Text,
                            LikeSemiNoValue = txtLikeSemiNo.Text,
                            Location = sp.SelectedPlantName,
                            LocationValue = sp.SelectedPlantId,
                        });
                        Counter++;
                    }
                }
                else
                {
                    listMembers.Add(new ListMembersForSemiAdd()
                    {
                        SNo = Counter,
                        Description = txtSemiDescription.Text,
                        DescriptionValue = txtSemiDescription.Text,
                        ExistingNo = txtLikeSemiExistingNo.Text,
                        ExistingNoValue = txtLikeSemiExistingNo.Text,
                        RequestFor = ddlRequestNewExistingSemi.SelectedItem.Text,
                        RequestForValue = ddlRequestNewExistingSemi.SelectedItem.Text,
                        LikeSemiNo = txtLikeSemiNo.Text,
                        LikeSemiNoValue = txtLikeSemiNo.Text
                    });

                }  
            }
            rptNewSemiComponent.DataSource = listMembers;
            rptNewSemiComponent.DataBind();
            txtSemiDescription.Text = "";
            txtLikeSemiExistingNo.Text = "";
            txtLikeSemiNo.Text = "";
            rptMakePackSemi.DataSource = null;
            rptMakePackSemi.DataBind();
            ddlRequestNewExistingSemi.SelectedIndex = -1;
            ddlMakePackSemi.SelectedIndex = -1;

        }
        protected void btnSemiRow_Click(object sender, EventArgs e)
        {
            int Counter = 1;
            List<ListMembersForSemiAdd> listMembers = new List<ListMembersForSemiAdd>();
            List<string> selectedPlants = new List<string>();
            bool IsFormValid = true;

             
                foreach (RepeaterItem item in rptNewSemiComponent.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        HiddenField hiddenStatusField = (HiddenField)item.FindControl("hdnDeletedStatusForAddNewSemi");
                        if (hiddenStatusField.Value != "true")
                        {
                            try
                            {
                                listMembers.Add(new ListMembersForSemiAdd()
                                {
                                    SNo = Counter,
                                    Description = ((Label)item.FindControl("txtDescriptionForSemi")).Text,
                                    DescriptionValue = ((Label)item.FindControl("txtDescriptionForSemiValue")).Text,
                                    ExistingNo = ((Label)item.FindControl("txtExistingNoValue")).Text,
                                    ExistingNoValue = ((Label)item.FindControl("txtExistingNo")).Text,
                                    RequestFor = ((Label)item.FindControl("txtRequestForSemi")).Text,
                                    RequestForValue = ((Label)item.FindControl("txtRequestForSemiValue")).Text,
                                    LikeSemiNo = ((Label)item.FindControl("txtLikeSemiNoValue")).Text,
                                    LikeSemiNoValue = ((Label)item.FindControl("txtLikeSemiNo")).Text,
                                    Location = ((Label)item.FindControl("txtLocation")).Text,
                                    LocationValue = ((Label)item.FindControl("txtLocationValue")).Text,
                                });

                            }
                            catch (Exception exception)
                            {

                                ErrorSummary.AddError("Error occurred while adding new member: " + exception.Message, this.Page);
                                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.IPF.ToString() + ": AddMembers_New: " + exception.Message);
                                exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.IPF.ToString(), "AddMembers_New");
                            }
                            Counter++;
                        }
                    }

                }
                

                rptNewSemiComponent.DataSource = listMembers;
                rptNewSemiComponent.DataBind();
                txtSemiDescription.Text = "";
                txtLikeSemiExistingNo.Text = "";
                txtLikeSemiNo.Text = "";
                rptMakePackSemi.DataSource = null;
                rptMakePackSemi.DataBind();
                ddlRequestNewExistingSemi.SelectedIndex = -1;
                ddlMakePackSemi.SelectedIndex = -1;
            

        }
        public class ListMembers
        {
            private string mMakePackName;
            private string mMakePackNameValue;
            public string MakePackName { get { return mMakePackName; } set { mMakePackName = value; } }
            public string MakePackNameValue { get { return mMakePackNameValue; } set { mMakePackNameValue = value; } }
        }

        public class ListMembersForSemiAdd
        {
            private string mRequestFor;
            private string mDescription;
            private string mLikeSemiNo;
            private string mExistingNo;
            private string mLocation;
            private string mRequestForValue;
            private string mDecriptionValue;
            private string mLikeSemiNoValue;
            private string mExistingNoValue;
            private string mLocationValue;
            private int mSNo;
            public int SNo { get { return mSNo; } set { mSNo = value; } }
            public string RequestFor { get { return mRequestFor; } set { mRequestFor = value; } }
            public string Description { get { return mDescription; } set { mDescription = value; } }
            public string LikeSemiNo { get { return mLikeSemiNo; } set { mLikeSemiNo = value; } }
            public string Location { get { return mLocation; } set { mLocation = value; } }
            public string ExistingNo { get { return mExistingNo; } set { mExistingNo = value; } }
            public string RequestForValue { get { return mRequestForValue; } set { mRequestForValue = value; } }
            public string DescriptionValue { get { return mDecriptionValue; } set { mRequestForValue = value; } }
            public string LikeSemiNoValue { get { return mLikeSemiNoValue; } set { mLikeSemiNoValue = value; } }
            public string ExistingNoValue { get { return mExistingNoValue; } set { mExistingNoValue = value; } }
            public string LocationValue { get { return mLocationValue; } set { mLocationValue = value; } }
        }
        public class SelectedMakePlants{
            private string mSelectedPlantName;
            private string mSelectedPlantId;
            public  string SelectedPlantName { get { return mSelectedPlantName; } set { mSelectedPlantName = value; } }
            public string SelectedPlantId { get { return mSelectedPlantId; } set { mSelectedPlantId = value; } }
       
        }
        private static bool CheckForNA(string stateGateItemName)
        {
            return (stateGateItemName.ToUpper() == "NA" || stateGateItemName.ToUpper() == "NOT APPLICABLE" || stateGateItemName.ToUpper() == "N/A");
        }
    }
}
