using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Enum;
using Microsoft.Practices.Unity;
using Ferrara.Compass.Classes;

namespace Ferrara.Compass.Layouts.Ferrara.Compass.AppPages
{
    public partial class CancelProject : LayoutsPageBase
    {
        #region Member Variables
        private string webUrl = string.Empty;
        private static IProjectTimelineTypeService timelineNumbers;
        private IWorkflowService workflowService;
        private IExceptionService exceptionService;
        private IProjectNotesService notesService;
        #endregion

        #region Properties
        private int CompassItemId
        {
            get
            {
                if (Request.QueryString[GlobalConstants.QUERYSTRING_CompassItemId] != null)
                    return Convert.ToInt32(Request.QueryString[GlobalConstants.QUERYSTRING_CompassItemId]);
                return 0;
            }
        }
        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            timelineNumbers = DependencyResolution.DependencyMapper.Container.Resolve<IProjectTimelineTypeService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
            notesService = DependencyResolution.DependencyMapper.Container.Resolve<IProjectNotesService>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            webUrl = SPContext.Current.Web.Url;

            if (!Page.IsPostBack)
            {
                try
                {
                    this.Form.Page.Title = "Cancel Project";
                }
                catch (Exception ex)
                {
                    exceptionService.Handle(LogCategory.CriticalError, ex, "CancelProject", "Page_Load");
                }
            }
        }


        #region Button/Link Methods
        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
                Context.Response.Flush();
            }
            catch (Exception exception)
            {

            }
            finally
            {
                Context.Response.End();
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool error = false;
            try
            {
                if (CompassItemId > 0)
                {
                    if (txtProjectCancelReasson.Text.Length > 1024)
                    {
                        this.lblStatusUpdateError.Text = "Maximum 1024 charatcters are allowed.";
                    }
                    else
                    {
                        UpdateProjectCanellatitonReason(CompassItemId);
                        timelineNumbers.workflowStatusUpdate(CompassItemId, GlobalConstants.WORKFLOWPHASE_Cancelled);
                        workflowService.UpdateWorkflowPhase(CompassItemId, GlobalConstants.WORKFLOWPHASE_Cancelled);
                        notesService.UpdateProjectComments(CompassItemId, string.Concat("Reason for cancellation:", txtProjectCancelReasson.Text));
                        Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
                        Context.Response.Flush();
                    }
                }
                else
                {
                    this.lblStatusUpdateError.Text = "Status update failed. Please try again.";
                    error = true;
                }

            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "CancelProject: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "CancelProject", "btnSubmit_Click");
            }
            finally
            {
                if (!error)
                {
                    Context.Response.End();
                }
            }
        }


        public void UpdateProjectCanellatitonReason(int CompassListItemId)
        {
            // Get the current user
            SPUser currentUser = SPContext.Current.Web.CurrentUser;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectDecisionsListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem appItem = compassItemCol[0];
                            if (appItem != null)
                            {
                                appItem[CompassProjectDecisionsListFields.CancellationReasons] = txtProjectCancelReasson.Text;
                                appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                appItem.Update();
                            }
                        }

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        #endregion
    }
}
