using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Classes;
using Microsoft.Practices.Unity;
using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;

namespace Ferrara.Compass.WebParts.StageGateGenerateBriefPDF
{
    [ToolboxItemAttribute(false)]
    public partial class StageGateGenerateBriefPDF : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        #region Member Variables
        private IPDFService pdfService;
        #endregion

        #region Properties
        private string StageGateListItemId
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_PMTListItemId] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_PMTListItemId];
                return string.Empty;
            }
        }

        private string Gate
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_Gate] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_Gate];
                return string.Empty;
            }
        }

        private string BriefNo
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_FinancialBrief] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_FinancialBrief];
                return string.Empty;
            }
        }
        private string BriefType
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_BriefType] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_BriefType];
                return string.Empty;
            }
        }
        #endregion

        public StageGateGenerateBriefPDF()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();

            pdfService = DependencyResolution.DependencyMapper.Container.Resolve<IPDFService>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                FileAttribute pdfFile = new FileAttribute();
                pdfFile = pdfService.StageGateGenerateBriefPDF("", Convert.ToInt32(StageGateListItemId), Convert.ToInt32(Gate), Convert.ToInt32(BriefNo), true);
                Page.Response.ContentType = "application/pdf";
                Page.Response.AppendHeader("Content-Disposition", "inline;filename=data.pdf");
                Page.Response.BufferOutput = true;
                Page.Response.AddHeader("Content-Length", pdfFile.FileContent.Length.ToString());
                Page.Response.BinaryWrite(pdfFile.FileContent);
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateGenerateBriefPDF.ToString() + " : Gate = " + Gate + "& PMTProjectListId: " + StageGateListItemId + "& BriefNo: " + BriefNo + " : btnCreatePDF_Click: " + ex.Message);
            }
        }
    }
}
