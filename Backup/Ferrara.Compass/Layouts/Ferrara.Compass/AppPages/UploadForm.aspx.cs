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
    public partial class UploadForm : LayoutsPageBase
    {
        #region Member Variables
        private IPackagingItemService packagingItemService;
        private IUtilityService utilityService;
        private IStageGateGeneralService SGSGeneralService;
        private IExceptionService exceptionService;
        private bool isFileExist = false;
        private string webUrl = string.Empty;
        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            packagingItemService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            SGSGeneralService = DependencyResolution.DependencyMapper.Container.Resolve<IStageGateGeneralService>();
        }

        #region Properties
        private int PackagingItemId
        {
            get
            {
                if (Request.QueryString[GlobalConstants.QUERYSTRING_PackagingItemId] != null)
                    return Convert.ToInt32(Request.QueryString[GlobalConstants.QUERYSTRING_PackagingItemId]);
                return 0;
            }
        }
        private int CompassItemId
        {
            get
            {
                if (Request.QueryString[GlobalConstants.QUERYSTRING_CompassItemId] != null)
                    return Convert.ToInt32(Request.QueryString[GlobalConstants.QUERYSTRING_CompassItemId]);
                return 0;
            }
        }
        private string DocType
        {
            get
            {
                if (Request.QueryString[GlobalConstants.QUERYSTRING_DoctType] != null)
                    return Request.QueryString[GlobalConstants.QUERYSTRING_DoctType];
                return string.Empty;
            }
        }
        private string SAPnumber
        {
            get
            {
                if (Request.QueryString[GlobalConstants.QUERYSTRING_SAPNo] != null)
                    return Request.QueryString[GlobalConstants.QUERYSTRING_SAPNo];
                return string.Empty;
            }
        }
        private int RequestId
        {
            get
            {
                if (Request.QueryString[GlobalConstants.QUERYSTRING_RequestId] != null)
                    return Convert.ToInt32(Request.QueryString[GlobalConstants.QUERYSTRING_RequestId]);
                return 0;
            }
        }
        private string ProjectNumber
        {
            get
            {
                if (Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo] != null)
                    return Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo];
                return string.Empty;
            }
        }
        private string GateNo
        {
            get
            {
                if (Request.QueryString[GlobalConstants.QUERYSTRING_Gate] != null)
                    return Request.QueryString[GlobalConstants.QUERYSTRING_Gate];
                return string.Empty;
            }
        }
        private string BriefNo
        {
            get
            {
                if (Request.QueryString[GlobalConstants.QUERYSTRING_BriefNo] != null)
                    return Request.QueryString[GlobalConstants.QUERYSTRING_BriefNo];
                return string.Empty;
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
                    this.Form.Page.Title = "Upload Documents";
                }
                catch (Exception ex)
                {
                    exceptionService.Handle(LogCategory.CriticalError, ex, "UploadForm", "Page_Load");
                }
            }
            if (!string.IsNullOrEmpty(DocType) && DocType == GlobalConstants.DOCTYPE_StageGateBriefImage)
            {
                docUpload.Attributes.Add("accept", ".png,.jpg,.jpeg,.gif");
            }
        }

        #region Private Methods
        private void AddAttachments(PackagingItem packagingItem, FileUpload fileUpload, string docType)
        {
            if (fileUpload.HasFile)
            {
                isFileExist = true;
                var file = new FileAttribute();

                file.FileName = System.IO.Path.GetFileName(fileUpload.PostedFile.FileName);
                file.FileContent = fileUpload.FileBytes;
                file.DocType = docType;
                packagingItem.FileCADAttachments.Add(file);
            }
        }
        public List<FileAttribute> ConstructBriefImage(FileUpload fileUpload, string doctype)
        {
            List<FileAttribute> files = new List<FileAttribute>();
            if (fileUpload.HasFile)
            {
                isFileExist = true;
                var file = new FileAttribute();

                file.FileName = System.IO.Path.GetFileName(fileUpload.PostedFile.FileName);
                file.FileContent = fileUpload.FileBytes;
                file.DocType = doctype;
                files.Add(file);

            }
            return files;
        }
        private CompassListItem ConstructProjectAttachment()
        {
            CompassListItem item = new CompassListItem();

            if (DocType == GlobalConstants.DOCTYPE_StageGateProjectBrief || DocType == GlobalConstants.DOCTYPE_StageGateOthers || DocType == GlobalConstants.DOCTYPE_StageGateBriefImage || DocType == GlobalConstants.DOCTYPE_StageGateGateDocument)
            {
                item.ProjectNumber = ProjectNumber;
            }
            else
            {
                item.ProjectNumber = utilityService.GetProjectNumberFromItemId(CompassItemId, webUrl);
            }
            HttpFileCollection uploads = HttpContext.Current.Request.Files;
            for (int i = 0; i < uploads.Count; i++)
            {
                HttpPostedFile uploadedFile = uploads[i];
                if (uploadedFile.ContentLength > 0)
                {
                    var file = new FileAttribute();
                    file.FileName = System.IO.Path.GetFileName(docUpload.PostedFile.FileName);
                    file.FileContentLength = uploadedFile.ContentLength;
                    file.FileStream = uploadedFile.InputStream;
                    file.DocType = DocType;
                    item.FileAttachments.Add(file);
                }
            }
            return item;
        }
        private WorldSyncRequestItem ConstructRequestAttachment()
        {
            WorldSyncRequestItem request = null;
            HttpFileCollection uploads = HttpContext.Current.Request.Files;
            for (int i = 0; i < uploads.Count; i++)
            {
                HttpPostedFile uploadedFile = uploads[i];
                if (uploadedFile.ContentLength > 0)
                {
                    var file = new FileAttribute();
                    file.FileName = System.IO.Path.GetFileName(docUpload.PostedFile.FileName);
                    file.FileContentLength = uploadedFile.ContentLength;
                    file.FileStream = uploadedFile.InputStream;
                    file.DocType = DocType;
                    request = new WorldSyncRequestItem();
                    request.SAPnumber = SAPnumber;
                    request.RequestType = DocType;
                    request.RequestId = RequestId;
                    request.RequestStatus = GlobalConstants.WORLDSYNCREQ_InProcess;
                    request.FileAttachment = file;
                }
            }
            return request;
        }
        #endregion

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
                if (docUpload.HasFile)
                {

                    if (PackagingItemId != 0)
                    {
                        if (CompassItemId > 0)
                        {
                            var item = new PackagingItem();
                            item.Id = PackagingItemId;
                            AddAttachments(item, docUpload, DocType);

                            if (isFileExist)
                            {
                                var projectNo = utilityService.GetProjectNumberFromItemId(CompassItemId, webUrl);
                                if (!utilityService.UploadPackagingAttachment(item.FileCADAttachments, projectNo, item.Id))
                                {
                                    this.lblUploadError.Text = "Error uploading files...<br>Ensure your filename does not contain any of the following characters: ~, #, %, & , *, {, }, \\, :, <, >, ?, /, |, “";
                                    error = true;
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (DocType == GlobalConstants.DOCTYPE_StageGateProjectBrief || DocType == GlobalConstants.DOCTYPE_StageGateOthers)
                        {
                            if (string.IsNullOrWhiteSpace(ProjectNumber))
                            {
                                this.lblUploadError.Text = "Not a valid project to upload files.";
                                error = true;
                                return;
                            }
                        }

                        if (CompassItemId > 0 && DocType != GlobalConstants.DOCTYPE_StageGateBriefImage && DocType != GlobalConstants.DOCTYPE_StageGateGateDocument)
                        {
                            // Retrieve the data from the form
                            CompassListItem item = ConstructProjectAttachment();
                            if (!utilityService.UploadAttachment(item))
                            {
                                this.lblUploadError.Text = "Error uploading files...<br>Ensure your filename does not contain any of the following characters: ~, #, %, & , *, {, }, \\, :, <, >, ?, /, |, “";
                                error = true;
                                return;
                            }
                        }
                        else if (CompassItemId > 0 && (DocType == GlobalConstants.DOCTYPE_StageGateBriefImage || DocType == GlobalConstants.DOCTYPE_StageGateGateDocument))
                        {
                            if (DocType == GlobalConstants.DOCTYPE_StageGateBriefImage)
                            {
                                string fileExtn = System.IO.Path.GetExtension(docUpload.FileName);
                                fileExtn = fileExtn.ToLower();
                                if (!(fileExtn == ".png" || fileExtn == ".jpg" || fileExtn == ".jpeg" || fileExtn == ".gif"))
                                {
                                    this.lblUploadError.Text = "Only .png, .jpg, .jpeg and .gif files can be uploaded.";
                                    error = true;
                                    return;
                                }
                            }

                            List<FileAttribute> item = ConstructBriefImage(docUpload, DocType);
                            if (!SGSGeneralService.UploadStageGateDocument(item, ProjectNumber, DocType, GateNo.ToString(), BriefNo.ToString()))
                            {
                                this.lblUploadError.Text = "Error uploading files...<br>Ensure your filename does not contain any of the following characters: ~, #, %, & , *, {, }, \\, :, <, >, ?, /, |, “";
                                error = true;
                                return;
                            }
                            else
                            {
                                string location = Utilities.RedirectPageForm(GlobalConstants.PAGE_StageGateGenerateIPFs, ProjectNumber);
                                Context.Response.Write("<script type='text/javascript'>window.top.location.href = " + location + ";</script>");
                            }
                        }
                        else if (!string.IsNullOrEmpty(SAPnumber))
                        {
                            // Retrieve the data from the form
                            WorldSyncRequestItem request = ConstructRequestAttachment();
                            if (!utilityService.UploadAttachment(request))
                            {
                                this.lblUploadError.Text = "Error uploading files...<br>Ensure your filename does not contain any of the following characters: ~, #, %, & , *, {, }, \\, :, <, >, ?, /, |, “";
                                error = true;
                                return;
                            }
                        }
                    }
                    Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
                    Context.Response.Flush();
                }
                else
                {
                    this.lblUploadError.Text = "Please select a file to upload.";
                    error = true;

                }

            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "UploadForm: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "UploadForm", "btnSubmit_Click");
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
