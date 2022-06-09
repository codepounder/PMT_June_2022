using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using System.ComponentModel;
using Ferrara.Compass.Classes;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Interfaces;
using System.Web;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using Microsoft.SharePoint;

namespace Ferrara.Compass.WebParts.ProjectTimelineUpdateForm
{
    [ToolboxItemAttribute(false)]
    public partial class ProjectTimelineUpdateForm : WebPart
    {

        private IProjectTimelineUpdateService projectTimelineUpdateService;
        private static IProjectTimelineTypeService timelineNumbers;
        private string webUrl;
        private int compassId = 0;
        private string ProjectNumber
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo];
                return string.Empty;
            }
        }

        public ProjectTimelineUpdateForm()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            timelineNumbers = DependencyResolution.DependencyMapper.Container.Resolve<IProjectTimelineTypeService>();

            projectTimelineUpdateService = DependencyResolution.DependencyMapper.Container.Resolve<IProjectTimelineUpdateService>();


        }
        protected void Page_Load(object sender, EventArgs e)
        {
            compassId = Utilities.GetItemIdFromProjectNumber(ProjectNumber);
            if (!Page.IsPostBack)
            {
                if (!existingEntry(compassId))
                {
                    String timelineType = timelineNumbers.GetTimelineType(compassId.ToString());
                    DashboardDetailsItem dashboardDetails = new DashboardDetailsItem()
                    {
                        TimelineType = timelineType,
                        CompassListItemId = compassId
                    };
                    List<TimelineTypeItem> tasks = timelineNumbers.GetWorkflowStepItems(dashboardDetails);
                    foreach (TimelineTypeItem task in tasks)
                    {
                        if (task.WorkflowQuickStep != "Notification" && task.WorkflowQuickStep != "")
                        {
                            TextBox tb = (TextBox)FindControl("txt" + task.WorkflowQuickStep);
                            tb.Text = task.Holder.ToString();
                        }
                    }
                }
                else
                {
                    List<List<string>> existingProject = projectTimelineUpdateService.GetProjectItem(compassId);
                    List<KeyValuePair<string, string>> completedTasks = projectTimelineUpdateService.GetCompletedItems(compassId);
                    foreach (List<string> task in existingProject)
                    {
                        TextBox tb = (TextBox)FindControl("txt" + task[0]);
                        tb.Text = task[1];

                        foreach (KeyValuePair<string, string> completed in completedTasks.Where(r => r.Key == task[0]))
                        {
                            if (completed.Value != "" && completed.Value != null)
                            {
                                tb.ReadOnly = true;
                            }
                        }
                    }
                }
            }
        }


        private string ValidateForm()
        {
            string bValid = "";

            foreach (TextBox tb in this.Controls.OfType<TextBox>())
            {
                if (tb.Text == "")
                {
                    bValid = tb.ID;
                    break;
                }
            }

            return bValid;
        }

        private ProjectTimelineItem ConstructFormData()
        {
            ProjectTimelineItem item = new ProjectTimelineItem();


            try
            {
                item.CompassListItemId = compassId;
                item.IPF = Convert.ToInt32(txtIPF.Text);
                item.PrelimSAPInitialSetup = Convert.ToInt32(txtPrelimSAPInitialSetup.Text);
                item.SrOBMApproval = Convert.ToInt32(txtSrOBMApproval.Text);
                item.SrOBMApproval2 = Convert.ToInt32(txtSrOBMApproval2.Text);
                item.InitialCosting = Convert.ToInt32(txtInitialCosting.Text);
                item.InitialCapacity = Convert.ToInt32(txtInitialCapacity.Text);
                item.TradePromo = Convert.ToInt32(txtTradePromo.Text);
                item.Distribution = Convert.ToInt32(txtDistribution.Text);
                item.Operations = Convert.ToInt32(txtOperations.Text);
                item.SAPInitialSetup = Convert.ToInt32(txtSAPInitialSetup.Text);
                item.QA = Convert.ToInt32(txtQA.Text);
                item.OBMReview1 = Convert.ToInt32(txtOBMReview1.Text);
                item.BOMSetupPE = Convert.ToInt32(txtBOMSetupPE.Text);
                item.BOMSetupProc = Convert.ToInt32(txtBOMSetupProc.Text);
                item.BOMSetupPE2 = Convert.ToInt32(txtBOMSetupPE2.Text);
                item.OBMReview2 = Convert.ToInt32(txtOBMReview2.Text);
                item.GRAPHICS = Convert.ToInt32(txtGRAPHICS.Text);
                item.CostingQuote = Convert.ToInt32(txtCostingQuote.Text);
                item.FGPackSpec = Convert.ToInt32(txtFGPackSpec.Text);
                item.SAPBOMSetup = Convert.ToInt32(txtSAPBOMSetup.Text);
                item.ExternalMfg = Convert.ToInt32(txtExternalMfg.Text);

            }
            catch (Exception ex)
            {
                ErrorSummary.AddError("Unexpected Error Occurred: ConstructFormData", this.Page);
                return null;
            }


            return item;
        }


        #region Button Methods
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm() == "")
            {
                ProjectTimelineItem item = ConstructFormData();
                if (existingEntry(item.CompassListItemId))
                {
                    projectTimelineUpdateService.UpdateProjectTimelineItem(item, ProjectNumber);
                }
                else
                {
                    projectTimelineUpdateService.InsertProjectTimelineItem(item, ProjectNumber);
                }
                Page.Response.Redirect("/Pages/ProjectStatus.aspx?ProjectNo=" + ProjectNumber, false);
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "alert('Please enter a value for all fields.');", true);
            }

        }
        protected Boolean existingEntry(int compassId)
        {
            Boolean exists = false;
            try
            {
                int projectCount = projectTimelineUpdateService.GetProjectTimelineItem(compassId);
                if (projectCount > 0)
                {
                    exists = true;
                }
            }
            catch (Exception exception)
            {
                ErrorSummary.AddError(exception.Message, this.Page);
            }
            return exists;
        }
        #endregion

    }
}
