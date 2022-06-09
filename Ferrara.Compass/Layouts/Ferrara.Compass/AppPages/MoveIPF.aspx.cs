using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.ComponentModel;
using Ferrara.Compass.Classes;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Services;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Enum;
using Microsoft.Practices.Unity;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ferrara.Compass.Layouts.Ferrara.Compass.AppPages
{
    public partial class MoveIPF : LayoutsPageBase
    {
        #region Member Variables
        private IStageGateGeneralService stageGateGeneralService;
        private IStageGateCreateProjectService stageGateCreateProjectService;
        private IDashboardService dashboardService;
        private IItemProposalService IPFService;
        private IUtilityService utilityService;
        private IExceptionService exceptionService;
        private string webUrl = string.Empty;
        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            stageGateGeneralService = DependencyResolution.DependencyMapper.Container.Resolve<IStageGateGeneralService>();
            IPFService = DependencyResolution.DependencyMapper.Container.Resolve<IItemProposalService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            stageGateCreateProjectService = DependencyResolution.DependencyMapper.Container.Resolve<IStageGateCreateProjectService>();
            dashboardService = DependencyResolution.DependencyMapper.Container.Resolve<IDashboardService>();
        }

        #region Properties
        
        private int CompassListItemId
        {
            get
            {
                if (Request.QueryString["CompassItemId"] != null)
                    return Convert.ToInt32(Request.QueryString["CompassItemId"]);
                return 0;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            webUrl = SPContext.Current.Web.Url;

            if (!Page.IsPostBack)
            {
                try
                {
                    this.Form.Page.Title = "MoveIPF";
                    hdnChildProjectNo.Value = CompassListItemId.ToString();
                }
                catch (Exception ex)
                {
                    exceptionService.Handle(LogCategory.CriticalError, ex, "MoveIPF", "Page_Load");
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
        protected void btnLookupProjectNumber_Click(object sender, EventArgs e)
        {
            try
            {
                List<ItemProposalItem> searchResults = stageGateGeneralService.GetSearchParentProjectName(txtSearchProjectNo.Text);
                
                rblRadioButtonList.Items.Clear();
                if (searchResults.Count <= 0)
                {
                    lblUploadError.Text = "No results found.";
                    lblSelect.Visible = false;
                    lblProjectName.Visible = false;
                }
                else
                {
                    lblUploadError.Text = "";
                    lblSelect.Visible = true;
                    lblProjectName.Visible = true;
                    foreach (ItemProposalItem ipfItem in searchResults)
                    {
                        ListItem rb = new ListItem();
                        rb.Text = ipfItem.ProjectNumber + ": " + ipfItem.SAPItemNumber;
                        rb.Value = ipfItem.ProjectNumber;
                        
                        rblRadioButtonList.Items.Add(rb);
                    }
                }
            }
            catch (Exception exception)
            {

            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool error = false;
            try
            {
                foreach (ListItem item in rblRadioButtonList.Items)
                {
                    if (item.Selected)
                    {
                        string parentProjectNo = item.Value;
                        string newProjectNo = Utilities.GetNextChildProjectNumber(parentProjectNo);
                        ItemProposalItem IPFItem = new ItemProposalItem();
                        int childCount = newProjectNo.LastIndexOf("-");
                        string sortOrder = Regex.Replace(newProjectNo.Substring(childCount + 1), "[^0-9.]", "");
                        int id;
                        if (int.TryParse(sortOrder, out id))
                        {
                            if (id > 0)
                            {
                                IPFItem.GenerateIPFSortOrder = id+1;
                            }
                        }
                        
                        IPFItem.ProjectNumber = newProjectNo;
                        IPFItem.ParentProjectNumber = parentProjectNo;
                        IPFItem.StageGateProjectListItemId = Utilities.GetItemIdByProjectNumberFromStageGateProjectList(parentProjectNo);
                        
                        IPFItem.CompassListItemId = Convert.ToInt32(hdnChildProjectNo.Value);

                        dashboardService.moveChildProject(IPFItem);
                        if (string.IsNullOrEmpty(parentProjectNo))
                        {
                            lblUploadError.Text = "Please select a project to copy.";
                            error = true;
                        }
                        else
                        {
                            string location = Utilities.RedirectPageForm(GlobalConstants.PAGE_StageGateGenerateIPFs, parentProjectNo);
                            Context.Response.Write("<script type='text/javascript'>window.top.location.href = " + location + ";</script>");
                        }
                    }
                }
                if (rblRadioButtonList.SelectedValue == "")
                {
                    lblUploadError.Text = "Please select a project to copy.";
                    error = true;
                }
                Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
                Context.Response.Flush();
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Move BOM Form: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "Move", "btnSubmit_Click");
            }
            finally
            {
                if (!error)
                {
                    Context.Response.End();

                }
            }
        }
        #endregion
    }
}
