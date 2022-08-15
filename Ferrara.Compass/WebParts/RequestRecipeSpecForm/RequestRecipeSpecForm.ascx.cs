using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Classes;
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
        #endregion 
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            webUrl = SPContext.Current.Web.Url;
            string requestType = Page.Request.QueryString["RequestType"];
           
            if (!Page.IsPostBack)
            {
                Utilities.BindDropDownItems(drpMakeLocation, GlobalConstants.LIST_ManufacturingLocationsLookup, webUrl);
                Utilities.BindDropDownItems(ddlMakePackSemi, GlobalConstants.LIST_ManufacturingLocationsLookup, webUrl);
                

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
                // comment by mahipal
            }
        }

        protected void btnClick()
        {
            //test
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
            int Counter = 0;
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
            int Counter = 0;
            List<ListMembersForSemiAdd> listMembers = new List<ListMembersForSemiAdd>();
            string selectedPlants = string.Empty;
            foreach (RepeaterItem item in rptMakePackSemi.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox checkValue =(TextBox)item.FindControl("txtMakePackNameSemi");
                    selectedPlants += "," + checkValue.Text;
                }
            }
            listMembers.Add(new ListMembersForSemiAdd()
            {
                Description  = txtSemiDescription.Text,
                DescriptionValue = txtSemiDescription.Text,
                ExistingNo = txtLikeSemiExistingNo.Text,
                ExistingNoValue = txtLikeSemiExistingNo.Text,
                RequestFor = ddlRequestNewExistingSemi.SelectedItem.Text,
                RequestForValue = ddlRequestNewExistingSemi.SelectedItem.Text,
                LikeSemiNo = txtLikeSemiNo.Text,
                LikeSemiNoValue = txtLikeSemiNo.Text,
                Location = selectedPlants,
                 LocationValue = selectedPlants, 
            }) ;
            txtSemiDescription.Text = "";
            txtLikeSemiExistingNo.Text = "";
            txtLikeSemiNo.Text = "";
            rptMakePackSemi.DataSource = null;
            rptMakePackSemi.DataBind();
            ddlRequestNewExistingSemi.SelectedIndex = -1;
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
                                Description = ((TextBox)item.FindControl("txtDescriptionForSemi")).Text,
                                DescriptionValue = ((TextBox)item.FindControl("txtDescriptionForSemiValue")).Text,
                                ExistingNo = ((TextBox)item.FindControl("txtExistingNoValue")).Text,
                                ExistingNoValue = ((TextBox)item.FindControl("txtExistingNo")).Text,
                                RequestFor = ((TextBox)item.FindControl("txtRequestForSemi")) .Text,
                                RequestForValue = ((TextBox)item.FindControl("txtRequestForSemiValue")) .Text,
                                LikeSemiNo = ((TextBox)item.FindControl("txtLikeSemiNoValue")).Text,
                                LikeSemiNoValue = ((TextBox)item.FindControl("txtLikeSemiNo")).Text,
                                Location = ((TextBox)item.FindControl("txtLocation")).Text,
                                LocationValue = ((TextBox)item.FindControl("txtLocationValue")).Text ,
                            });                         
                             
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
            rptNewSemiComponent.DataSource = listMembers;
            rptNewSemiComponent.DataBind();
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
        private static bool CheckForNA(string stateGateItemName)
        {
            return (stateGateItemName.ToUpper() == "NA" || stateGateItemName.ToUpper() == "NOT APPLICABLE" || stateGateItemName.ToUpper() == "N/A");
        }
    }
}
