using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Practices.Unity;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Classes;
using Ferrara.Compass.Abstractions.Enum;
using System.Web.UI;
using System.Reflection;
using System.Linq;

namespace Ferrara.Compass.WebParts.ProjectNotesForm
{
    [ToolboxItemAttribute(false)]
    public partial class ProjectNotesForm : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        #region Member Variables
        private IProjectNotesService notesService;
        private IExceptionService exceptionService;
        private int iItemId = 0;
        private int iParentItemId = 0;
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

        public ProjectNotesForm()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            notesService = DependencyResolution.DependencyMapper.Container.Resolve<IProjectNotesService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();

            EnsureScriptManager();
            EnsureUpdatePanelFixups();
        }
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
        protected void UpdatePanel_Unload(object sender, EventArgs e)
        {
            MethodInfo methodInfo = typeof(ScriptManager).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(i => i.Name.Equals("System.Web.UI.IScriptManagerInternal.RegisterUpdatePanel")).First();
            methodInfo.Invoke(ScriptManager.GetCurrent(Page),
                new object[] { sender as UpdatePanel });
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    if (!CheckProjectNumber())
                    {
                        this.divProjectNotes.Visible = false;
                        this.divProjectNotesParentContainer.Visible = false;
                        return;
                    }

                    LoadFormData();
                }
                catch (Exception exception)
                {
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Project Notes: " + exception.Message);
                    exceptionService.Handle(LogCategory.CriticalError, exception, "Project Notes", "Page_Load");
                }
            }
            else
            {
                if (!CheckProjectNumber())
                {
                    this.divProjectNotes.Visible = false;
                    this.divProjectNotesParentContainer.Visible = false;
                    return;
                }
            }
        }

        #region Private Methods
        private bool CheckProjectNumber()
        {
            try
            {
                //Check In Compass List
                iItemId = Utilities.GetItemIdFromProjectNumber(ProjectNumber);

                if (iItemId == 0)
                {
                    //Check In Stage Gate Project List
                    iItemId = Utilities.GetStageGateProjectListItemIdFromProjectNumber(ProjectNumber);
                    if (iItemId == 0)
                    {
                        return false;
                    }
                    else
                    {
                        hdnProjectType.Value = GlobalConstants.ParentChild_Parent;
                        divProjectNotes.Visible = false;
                    }
                }
                else
                {
                    hdnProjectType.Value = GlobalConstants.ParentChild_Child;
                    divTextBoxNotesParent.Visible = false;
                    divButtonSaveParent.Visible = false;
                    iParentItemId = Utilities.GetStageGateProjectListItemIdFromCompassListItemId(iItemId);
                }
            }
            catch (Exception ex)
            {
                iItemId = 0;
                return false;
            }

            return true;
        }
        #endregion

        #region Data Transfer Methods
        private void LoadFormData()
        {
            if (iItemId != 0)
            {
                if (hdnProjectType.Value == GlobalConstants.ParentChild_Parent)
                {
                    this.lblNotesHistoryParent.Text = notesService.GetStageGateProjectCommentsHistory(iItemId);
                }
                else if (hdnProjectType.Value == GlobalConstants.ParentChild_Child)
                {
                    if (iParentItemId != 0)
                    {
                        this.lblNotesHistoryParent.Text = notesService.GetStageGateProjectCommentsHistory(iParentItemId);
                    }
                    this.lblNotesHistory.Text = notesService.GetProjectCommentsHistory(iItemId);
                }
            }
        }

        #endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string comments = this.txtNotes.Text.Trim();

            if (string.IsNullOrEmpty(comments))
                return;

            notesService.UpdateProjectComments(iItemId, comments);
            this.txtNotes.Text = "";
            this.lblNotesHistory.Text = notesService.GetProjectCommentsHistory(iItemId);
        }

        protected void btnSaveParent_Click(object sender, EventArgs e)
        {
            string comments = this.txtNotesParent.Text.Trim();

            if (string.IsNullOrEmpty(comments))
                return;

            if (hdnProjectType.Value == GlobalConstants.ParentChild_Parent)
            {
                notesService.UpdateStageGateProjectComments(iItemId, comments);
                this.lblNotesHistoryParent.Text = notesService.GetStageGateProjectCommentsHistory(iItemId);
            }
            else if (hdnProjectType.Value == GlobalConstants.ParentChild_Child)
            {
                notesService.UpdateStageGateProjectComments(iParentItemId, comments);
                this.lblNotesHistoryParent.Text = notesService.GetStageGateProjectCommentsHistory(iParentItemId);
            }

            this.txtNotesParent.Text = "";
        }
    }
}
