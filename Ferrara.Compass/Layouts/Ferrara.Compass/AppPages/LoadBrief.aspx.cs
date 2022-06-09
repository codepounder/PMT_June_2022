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

namespace Ferrara.Compass.Layouts.Ferrara.Compass.AppPages
{
    public partial class LoadBrief : LayoutsPageBase
    {
        #region Member Variables
        private IStageGateGeneralService stageGateGeneralService;
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
        }

        #region Properties
        private int PMTListItemId
        {
            get
            {
                if (Request.QueryString[GlobalConstants.QUERYSTRING_PMTListItemId] != null)
                    return Convert.ToInt32(Request.QueryString[GlobalConstants.QUERYSTRING_PMTListItemId]);
                return 0;
            }
        }
        private string ProjectNo
        {
            get
            {
                if (Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo] != null)
                    return Convert.ToString(Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo]);
                return string.Empty;
            }
        }
        private string URLStage
        {
            get
            {
                if (Request.QueryString["URLStage"] != null)
                    return Convert.ToString(Request.QueryString["URLStage"]);
                return string.Empty;
            }
        }    
        private int Gate
        {
            get
            {
                if (Request.QueryString[GlobalConstants.QUERYSTRING_Gate] != null)
                    return Convert.ToInt32(Request.QueryString[GlobalConstants.QUERYSTRING_Gate]);
                return 0;
            }
        }
        private int BriefCount
        {
            get
            {
                if (Request.QueryString["BriefCount"] != null)
                    return Convert.ToInt32(Request.QueryString["BriefCount"]);
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
                    this.Form.Page.Title = "Load Brief from Previous Gate";
                    LoadBriefList();
                }
                catch (Exception ex)
                {
                    exceptionService.Handle(LogCategory.CriticalError, ex, "LoadPreviousGatePopup", "Page_Load");
                }
            }
        }
        private void LoadBriefList()
        {
            List<StageGateGateItem> searchResults = new List<StageGateGateItem>();
            searchResults = stageGateGeneralService.GetOtherStageGateBriefItem(PMTListItemId, Gate);
            rblRadioButtonList.Items.Clear();
            if (searchResults.Count <= 0)
            {
                lblUploadError.Text = "No results found.";
                lblBriefName.Visible = false;
            }
            else
            {
                lblUploadError.Text = "";
                lblBriefName.Visible = true;
                foreach (StageGateGateItem briefItem in searchResults)
                {
                    ListItem rb = new ListItem();
                    rb.Text = "Gate "+briefItem.Gate +": " + briefItem.BriefName;
                    rb.Value = briefItem.ID.ToString();
                    rblRadioButtonList.Items.Add(rb);
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
                foreach (ListItem item in rblRadioButtonList.Items)
                {
                    if (item.Selected)
                    {
                        int oldBriefId = Convert.ToInt32(item.Value);
                        StageGateGateItem BriefItem = stageGateGeneralService.GetSingleStageGateBriefItem(oldBriefId);
                        var files = stageGateGeneralService.GetUploadedStageGateFiles(ProjectNo, GlobalConstants.DOCTYPE_StageGateBriefImage, BriefItem.Gate.ToString(), BriefItem.BriefNo.ToString(), webUrl);
                        BriefItem.Gate = Gate.ToString();
                        BriefItem.BriefNo = BriefCount + 1;
                        BriefItem.ID = 0;
                        int newBriefId = stageGateGeneralService.UpsertGateBriefItem(BriefItem);
                        error = stageGateGeneralService.UploadStageGateDocument(files, ProjectNo, GlobalConstants.DOCTYPE_StageGateBriefImage, Gate.ToString(), BriefItem.BriefNo.ToString());
                        Context.Response.Write("<script type='text/javascript'>window.top.location.assign('" + Utilities.RedirectPageForm(URLStage, ProjectNo) + "');</script>");
                    }
                }
                if(rblRadioButtonList.SelectedValue == "")
                {
                    lblUploadError.Text = "Please select a brief to load.";
                    error = true;
                }
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Move BOM Form: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "Move", "btnSubmit_Click");
            }
            finally
            {
                if (!error) {
                    Context.Response.End();
                    
                }
            }
        }
        #endregion
    }
}
