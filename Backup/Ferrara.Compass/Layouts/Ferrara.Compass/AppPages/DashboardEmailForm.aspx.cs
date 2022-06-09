using System;
using Microsoft.SharePoint.WebControls;
using System.Web;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Interfaces;
using Microsoft.Practices.Unity;
using Ferrara.Compass.Classes;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Models;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Linq;

namespace Ferrara.Compass.Layouts.Ferrara.Compass.AppPages
{
    public partial class DashboardEmailForm : LayoutsPageBase
    {
        #region Member Variables
        private INotificationService notificationService;
        private IExceptionService exceptionService;
        private IEmailLoggingService emailLoggingService;
        private IVersionHistoryService versionHistoryService;
        private IWorkflowService workflowService;
        private string webUrl = string.Empty;
        #endregion
        private string ProjectNumber
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo];
                return string.Empty;
            }
        }
        private string WFQuickStep
        {
            get
            {
                if (HttpContext.Current.Request.QueryString["WFQuickStep"] != null)
                    return HttpContext.Current.Request.QueryString["WFQuickStep"];
                return string.Empty;
            }
        }
        private string pageName
        {
            get
            {
                if (HttpContext.Current.Request.QueryString["pageName"] != null)
                    return HttpContext.Current.Request.QueryString["pageName"] + ".aspx";
                return string.Empty;
            }
        }
        private int compassId
        {
            get
            {
                if (HttpContext.Current.Request.QueryString["compassId"] != null)
                    return Convert.ToInt32(HttpContext.Current.Request.QueryString["compassId"]);
                return 0;
            }
        }
        public DashboardEmailForm()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            emailLoggingService = DependencyResolution.DependencyMapper.Container.Resolve<IEmailLoggingService>();
            versionHistoryService = DependencyResolution.DependencyMapper.Container.Resolve<IVersionHistoryService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            webUrl = SPContext.Current.Web.Url;

            if (!Page.IsPostBack)
            {
                try
                {
                    Page.Title = "Dashboard Email Form";
                    List<KeyValuePair<string, WorkflowStep>> emailHistory = emailLoggingService.GetEmailLoggingHistory(compassId);
                    List<string> historyDetails = emailLoggingService.GetVersionDisplay(emailHistory, WFQuickStep);
                    HtmlTable historyTable = new HtmlTable();
                    foreach (string log in historyDetails)
                    {
                        HtmlTableRow historyRow = new HtmlTableRow();
                        HtmlTableCell historyItem = new HtmlTableCell();
                        historyItem.InnerText = log;
                        historyRow.Cells.Add(historyItem);
                        historyTable.Rows.Add(historyRow);
                    }
                    emailHistoryPanel.Controls.Add(historyTable);
                    if (WFQuickStep == "SAPRoutingSetup")
                    {
                        lblExtraMessage.Visible = false;
                        txtExtraMessage.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    exceptionService.Handle(LogCategory.CriticalError, ex, "DashboardEmailForm", "Page_Load");
                }
            }
        }
        private string hasError
        {
            get
            {
                if (HttpContext.Current.Request.QueryString["error"] != null)
                    return HttpContext.Current.Request.QueryString["error"];
                return string.Empty;
            }
        }
        private string errorText
        {
            get
            {
                if (HttpContext.Current.Request.QueryString["errorText"] != null)
                    return HttpContext.Current.Request.QueryString["errorText"];
                return string.Empty;
            }
        }
        protected void sendEmail(object sender, EventArgs e)
        {
            try
            {
                string newMessage = txtExtraMessage.Text;
                string tempWFQuickStep = WFQuickStep;
                if (tempWFQuickStep.Contains("BOMSetupProc"))
                {
                    if (!tempWFQuickStep.Contains("Seasonal") && !tempWFQuickStep.Contains("External") && !tempWFQuickStep.Contains("CoMan") && !tempWFQuickStep.Contains("Novelty"))
                    {
                        tempWFQuickStep = tempWFQuickStep.Replace("BOMSetupProc", "BOMSetupProcEBP");
                    }
                }

                bool results = false;
                if (WFQuickStep == "SAPRoutingSetup")
                {
                    notificationService.resetSentStatus(compassId);
                    workflowService.StartSpecificWorkflow(compassId, "7f - SAP BOM Setup Automated Emails");
                    this.lblSendEmailCompleted.Text = "The email will be sent shortly";
                    this.emailHistoryPanel.Controls.Clear();
                    List<KeyValuePair<string, WorkflowStep>> emailHistory = emailLoggingService.GetEmailLoggingHistory(compassId);
                    List<string> historyDetails = emailLoggingService.GetVersionDisplay(emailHistory, WFQuickStep);
                    HtmlTable historyTable = new HtmlTable();
                    foreach (string log in historyDetails)
                    {
                        HtmlTableRow historyRow = new HtmlTableRow();
                        HtmlTableCell historyItem = new HtmlTableCell();
                        historyItem.InnerText = log;
                        historyRow.Cells.Add(historyItem);
                        historyTable.Rows.Add(historyRow);
                    }
                    this.emailHistoryPanel.Controls.Add(historyTable);
                    Context.Response.Write("<script type='text/javascript'>setTimeout(function() { window.frameElement.commitPopup();}, 5000);</script>");
                    return;
                }
                else
                {
                    if (WFQuickStep == "MatrlWHSetUp")
                    {
                        tempWFQuickStep = "BOMSetupMaterialWarehouse";
                    }
                    else if (WFQuickStep == "SAPCompleteItem")
                    {
                        tempWFQuickStep = "SAPCompleteItemSetup";
                    }

                    results = notificationService.EmailWFStep(tempWFQuickStep, pageName, compassId, ProjectNumber, newMessage);

                    if (WFQuickStep == "MatrlWHSetUp")
                    {
                        tempWFQuickStep = "BOMSetupMH";
                    }

                    if (results)
                    {
                        this.lblSendEmailCompleted.Text = "Email has been successfully sent!";
                        this.emailHistoryPanel.Controls.Clear();
                        List<KeyValuePair<string, WorkflowStep>> emailHistory = emailLoggingService.GetEmailLoggingHistory(compassId);
                        List<string> historyDetails = emailLoggingService.GetVersionDisplay(emailHistory, tempWFQuickStep);
                        HtmlTable historyTable = new HtmlTable();
                        foreach (string log in historyDetails)
                        {
                            HtmlTableRow historyRow = new HtmlTableRow();
                            HtmlTableCell historyItem = new HtmlTableCell();
                            historyItem.InnerText = log;
                            historyRow.Cells.Add(historyItem);
                            historyTable.Rows.Add(historyRow);
                        }
                        this.emailHistoryPanel.Controls.Add(historyTable);
                        Context.Response.Write("<script type='text/javascript'>setTimeout(function() { window.frameElement.commitPopup();}, 5000);</script>");
                        return;

                    }
                    else
                    {
                        this.lblSendEmailCompleted.Text = "An Error has occured, please try again.";
                        Context.Response.Write("<script type='text/javascript'>setTimeout(function() { window.frameElement.commitPopup();}, 5000);</script>");
                    }
                }
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "DashboardEmailForm: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "DashboardEmailForm", "sendEmail");
            }
            finally
            {
                //Context.Response.Flush();
                //Context.Response.End();
            }

        }
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
    }
}
