using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Classes;
using Ferrara.Compass.ControlTemplates.Ferrara.Compass;
using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Practices.Unity;
using Ferrara.Compass.Abstractions.Models;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using System.Web.UI.HtmlControls;
using System.Linq;

namespace Ferrara.Compass.WebParts.StageGateFinanceSummaryForm
{
    [ToolboxItemAttribute(false)]

    public partial class StageGateFinancialSummaryForm : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        #region Member Variables
        private IStageGateGeneralService stageGateGeneralService;
        private IUserManagementService userManagementService;
        private IStageGateFinancialServices stageGateFinancialServices;
        private IPDFService pdfService;
        private int StageGateProjectListItemId;
        private const string _ucSGSProjectInformation = @"~/_controltemplates/15/Ferrara.Compass/ucSGSProjectInformation.ascx";
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
        private string GateNo
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_Gate] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_Gate];
                return string.Empty;
            }
        }
        #endregion
        public StageGateFinancialSummaryForm()
        {
        }

        #region OnInit
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            stageGateFinancialServices = DependencyResolution.DependencyMapper.Container.Resolve<IStageGateFinancialServices>();
            stageGateGeneralService = DependencyResolution.DependencyMapper.Container.Resolve<IStageGateGeneralService>();
            userManagementService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            pdfService = DependencyResolution.DependencyMapper.Container.Resolve<IPDFService>();
        }
        #endregion

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

                if (StageGateProjectListItemId > 0)
                {
                    LoadFormData();
                }
            }
            else
            {
                StageGateProjectListItemId = Convert.ToInt32(hdnStageGateListItemId.Value);
            }

            LoadUserControls();
        }

        #region DataTransfer Methods
        private void LoadFormData()
        {
            List<StageGateGateItem> gateBriefItems = stageGateGeneralService.GetStageGateBriefItem(StageGateProjectListItemId, Convert.ToInt32(GateNo));
            List<StageGateGateItem> filterdedgateBriefItems = new List<StageGateGateItem>();

            foreach (var item in gateBriefItems)
            {
                if (item.Deleted != "Yes")
                {
                    filterdedgateBriefItems.Add(item);
                }
            }
            if (filterdedgateBriefItems.Count > 0)
            {
                rptFinanceBriefs.DataSource = filterdedgateBriefItems;
                rptFinanceBriefs.DataBind();
            }
        }
        private void LoadUserControls()
        {
            SGSProjectInformation.Controls.Clear();

            //Project Information
            ucSGSProjectInformation ctrl = (ucSGSProjectInformation)Page.LoadControl(_ucSGSProjectInformation);
            ctrl.StageGateItemId = StageGateProjectListItemId;
            SGSProjectInformation.Controls.Add(ctrl);
        }
        #endregion

        #region Button Click Methods
        #endregion

        #region Repeater Methods
        protected void rptFinanceBriefs_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                StageGateGateItem stageGateGateItem = (StageGateGateItem)e.Item.DataItem;
                Label lblBriefName = (Label)e.Item.FindControl("lblBriefName");
                lblBriefName.Text = "Brief G" + GateNo.ToString() + " # " + stageGateGateItem.BriefNo.ToString() + ": " + stageGateGateItem.BriefName;
                var dtFinancialSummaryItems = stageGateFinancialServices.GetStageGateConsolidatedFinancialSummaryItem(StageGateProjectListItemId, GateNo.ToString(), stageGateGateItem.BriefNo.ToString());
                var dtFinancialAnalysisItems = stageGateFinancialServices.GetAllStageGateFinancialAnalysisItemsByGateAndBriefNumber(StageGateProjectListItemId, GateNo.ToString(), stageGateGateItem.BriefNo.ToString());

                HtmlGenericControl divGrossMargin = (HtmlGenericControl)e.Item.FindControl("divGrossMargin");
                divGrossMargin.Visible = true;

                var FinancialAnalysisIncluded = dtFinancialAnalysisItems.Where(item => item.PLinConsolidatedFinancials == "Yes");
                if (FinancialAnalysisIncluded.Count() == 0)
                {
                    divGrossMargin.Visible = false;
                }

                if (dtFinancialSummaryItems != null)
                {
                    TextBox txtTotalGrossMarginPct1 = (TextBox)e.Item.FindControl("txtTotalGrossMarginPct1");
                    TextBox txtTotalGrossMarginPct2 = (TextBox)e.Item.FindControl("txtTotalGrossMarginPct2");
                    TextBox txtTotalGrossMarginPct3 = (TextBox)e.Item.FindControl("txtTotalGrossMarginPct3");
                    TextBox txtIncrementalGrossMarginPct1 = (TextBox)e.Item.FindControl("txtIncrementalGrossMarginPct1");
                    TextBox txtIncrementalGrossMarginPct2 = (TextBox)e.Item.FindControl("txtIncrementalGrossMarginPct2");
                    TextBox txtIncrementalGrossMarginPct3 = (TextBox)e.Item.FindControl("txtIncrementalGrossMarginPct3");

                    txtTotalGrossMarginPct1.Text = dtFinancialSummaryItems.GrossMarginPctTotal1.ToString();
                    txtTotalGrossMarginPct2.Text = dtFinancialSummaryItems.GrossMarginPctTotal2.ToString();
                    txtTotalGrossMarginPct3.Text = dtFinancialSummaryItems.GrossMarginPctTotal3.ToString();
                    txtIncrementalGrossMarginPct1.Text = dtFinancialSummaryItems.GrossMarginPctIncremental1.ToString();
                    txtIncrementalGrossMarginPct2.Text = dtFinancialSummaryItems.GrossMarginPctIncremental2.ToString();
                    txtIncrementalGrossMarginPct3.Text = dtFinancialSummaryItems.GrossMarginPctIncremental3.ToString();
                }


                HtmlTextArea txtOtherKeyInfo = (HtmlTextArea)e.Item.FindControl("txtOtherKeyInfo");
                HtmlTextArea txtProductFormats = (HtmlTextArea)e.Item.FindControl("txtProductFormats");

                txtOtherKeyInfo.InnerHtml = stageGateGateItem.OtherKeyInfo;
                txtProductFormats.InnerHtml = stageGateGateItem.ProductFormats;

                var FinacialBriefDocument = stageGateGeneralService.GetUploadedStageGateFiles(ProjectNumber, GlobalConstants.DOCTYPE_StageGateFinanceBriefPDF, GateNo.ToString(), stageGateGateItem.BriefNo.ToString(), webUrl);
                if (FinacialBriefDocument.Count > 0)
                {
                    HtmlAnchor ancFinancePDF = ((HtmlAnchor)e.Item.FindControl("ancFinancePDF"));
                    if (ancFinancePDF != null)
                    {
                        string fileName = FinacialBriefDocument[0].FileName;
                        fileName = fileName.Replace("_", " ");
                        ancFinancePDF.InnerText = "Finance Brief G" + GateNo.ToString() + " # " + stageGateGateItem.BriefNo.ToString() + ": " + stageGateGateItem.BriefName + ".pdf";
                        ancFinancePDF.HRef = FinacialBriefDocument[0].FileUrl;
                        ancFinancePDF.Visible = true;
                    }
                }

                HyperLink btnFinanceBrief = ((HyperLink)e.Item.FindControl("btnFinanceBrief"));
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                   new KeyValuePair<string, string>(GlobalConstants.QUERYSTRING_ProjectNo, ProjectNumber),
                   new KeyValuePair<string, string>(GlobalConstants.QUERYSTRING_Gate, GateNo.ToString()),
                   new KeyValuePair<string, string>(GlobalConstants.QUERYSTRING_FinancialBrief, stageGateGateItem.BriefNo.ToString())
                };

                btnFinanceBrief.NavigateUrl = Utilities.RedirectPageValue(GlobalConstants.PAGE_StageGateFinancialBrief, parameters);
            }
        }
        protected void rptFinanceBriefs_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "GenerateFinancePDF")
            {
                StageGateGateItem stageGateGateItem = (StageGateGateItem)e.Item.DataItem;
                string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
                string BriefNo = commandArgs[0];
                HtmlAnchor ancFinancePDF = ((HtmlAnchor)e.Item.FindControl("ancFinancePDF"));

                if (ancFinancePDF.Visible)
                {
                    stageGateGeneralService.DeleteStageGateFiles(ProjectNumber, GlobalConstants.DOCTYPE_StageGateFinanceBriefPDF, GateNo.ToString(), BriefNo.ToString());
                }

                var FinacialBriefDocumentName = pdfService.StageGateGenerateFinanceBriefPDF(ProjectNumber, Convert.ToInt32(StageGateProjectListItemId), Convert.ToInt32(GateNo), Convert.ToInt32(BriefNo));
                if (!string.IsNullOrEmpty(FinacialBriefDocumentName))
                {
                    var FinacialBriefDocument = stageGateGeneralService.GetUploadedStageGateFiles(ProjectNumber, GlobalConstants.DOCTYPE_StageGateFinanceBriefPDF, GateNo.ToString(), BriefNo.ToString(), webUrl);
                    if (FinacialBriefDocument.Count > 0)
                    {
                        Label lblBriefName = (Label)e.Item.FindControl("lblBriefName");
                        if (ancFinancePDF != null)
                        {
                            ancFinancePDF.Visible = true;
                            ancFinancePDF.InnerText = "Finance " + lblBriefName.Text + ".pdf";
                            ancFinancePDF.HRef = FinacialBriefDocument[0].FileUrl;
                        }
                    }
                }
            }
        }
        #endregion

        #region Private Methods
        private bool CheckProjectNumber()
        {
            if (!string.IsNullOrWhiteSpace(ProjectNumber))
            {
                StageGateProjectListItemId = Utilities.GetItemIdByProjectNumberFromStageGateProjectList(ProjectNumber);
            }

            // Store Id in Hidden field
            this.hdnStageGateListItemId.Value = StageGateProjectListItemId.ToString();
            return true;
        }
        private bool CheckWriteAccess()
        {
            if (userManagementService.HasWriteAccess(CompassForm.StageGateFinancialSummary))
            {
                return true;
            }
            return false;
        }
        private Boolean ValidateForm()
        {
            Boolean bValid = true;


            return bValid;
        }
        #endregion
    }
}
