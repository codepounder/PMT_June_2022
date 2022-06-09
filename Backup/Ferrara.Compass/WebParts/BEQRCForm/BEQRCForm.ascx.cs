using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.ControlTemplates.Ferrara.Compass;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Practices.Unity;
using Ferrara.Compass.Classes;
using System.Web.UI;
using Ferrara.Compass.Abstractions.Enum;
using System.Linq;

namespace Ferrara.Compass.WebParts.BEQRCForm
{
    [ToolboxItemAttribute(false)]
    public partial class BEQRCForm : WebPart
    {
        #region Member Variables
        private IBEQRCService BEQRCService;
        private IItemProposalService itemProposalService;
        private IPackagingItemService packagingItemService;
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private IWorkflowService workflowService;
        private IUserManagementService userMgmtService;
        private INotificationService notificationService;
        private string webUrl;
        private int iItemId = 0;
        private string SAPDescription;
        private string SAPItemNumber;
        private string currentProjectNumber;
        private List<PackagingItem> packagingItems = new List<PackagingItem>();
        private List<string> UPCAssociated = new List<string>();
        private const string _ucBEQRCPath = @"~/_controltemplates/15/Ferrara.Compass/ucBEQRC.ascx";
        #endregion
        #region Properties
        private string ProjectNumber
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo] != null)
                    currentProjectNumber = HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo];
                else
                    currentProjectNumber = string.Empty;
                return currentProjectNumber;
            }
        }
        #endregion
        public BEQRCForm()
        {
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            // Page.EnableEventValidation = false;

            BEQRCService = DependencyResolution.DependencyMapper.Container.Resolve<IBEQRCService>();
            itemProposalService = DependencyResolution.DependencyMapper.Container.Resolve<IItemProposalService>();
            packagingItemService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
            userMgmtService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
            EnsureScriptManager();
            EnsureUpdatePanelFixups();
        }
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
                {
                    btnSave.Style.Add("opacity", "0");
                    return;

                }
                else
                {
                    btnSave.Style.Add("opacity", "1");
                }
                InitializeScreen();
                LoadFormData();
            }
            else
            {
                iItemId = Convert.ToInt32(hiddenItemId.Value);
            }
            hdnIItemId.Value = iItemId.ToString();
            hdnProjectNumber.Value = ProjectNumber;
            hdnPageName.Value = GlobalConstants.PAGE_BEQRC;
            if (iItemId > 0)
            {
                LoadBOMItems();
            }
        }
        #region Data Transfer Methods
        private void LoadFormData()
        {
            // Load the Original Item Proposal
            BEQRCItem item = BEQRCService.GetBEQRCItem(iItemId);
            hdnTBDIndicator.Value = item.TBDIndicator;
            txtProjectType.Text = item.ProjectType;
            txtLineOfBusiness.Text = item.ProductHierarchyLevel1;
            txtProductHierarchy2.Text = item.ProductHierarchyLevel2;
            txtBrand.Text = item.MaterialGroup1Brand;
            txtFirstShipDate.Text = Utilities.GetDateForDisplay(item.RevisedFirstShipDate);

            try
            {
                TimeSpan timeSpan = item.RevisedFirstShipDate.Subtract(DateTime.Now);
                int dateDiff = timeSpan.Days;
                int totalWeeks = (int)dateDiff / 7;
                txtWeeksUntilShip.Text = Convert.ToString(totalWeeks);
            }
            catch (Exception ex)
            {
            }
            txtAnnualDollars.Text = Utilities.FormatDecimal(item.AnnualProjectedDollars, 0);
            txtAnnualUnits.Text = Utilities.FormatNumber(item.AnnualProjectedUnits);
            txtCustomer.Text = item.Customer;
            txtExpectedGrossMarginPct.Text = Utilities.FormatDecimal(item.ExpectedGrossMarginPercent, 2);
            if (item.ProjectType == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
            {
                lblComments.Text = "Change Notes:";
            }
            else
            {
                lblComments.Text = "Item Concept:";
            }

            txtItemConcept.Text = item.ItemConcept;
            txtConsumerFacingProdDesc.Text = item.ConsumerFacingProdDesc;
            hdnProductForm.Value = string.IsNullOrEmpty(item.MaterialGroup4ProductForm) ? "" : item.MaterialGroup4ProductForm;
            hdnPackType.Value = string.IsNullOrEmpty(item.MaterialGroup5PackType) ? "" : item.MaterialGroup5PackType;
            hdnBrand.Value = string.IsNullOrEmpty(item.MaterialGroup1Brand) ? "" : item.MaterialGroup1Brand;

            packagingItems = BEQRCService.GetAllProjectItems(iItemId);

            var CandySemis =
                (
                    from
                        packagingItem in packagingItems
                    where
                        packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_CandySemi
                    select
                       packagingItem
                ).ToList();
            if (CandySemis.Count > 0)
            {
                divCandySemi.Visible = true;
                rptCandySemi.DataSource = CandySemis;
                rptCandySemi.DataBind();
            }

            var purchasedCandyItems =
               (
                   from
                       packagingItem in packagingItems
                   where
                       packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi
                   select
                      packagingItem
               ).ToList();
            if (purchasedCandyItems.Count > 0)
            {
                divPurchasedCandy.Visible = true;
                rptPurchasedCandy.DataSource = purchasedCandyItems;
                rptPurchasedCandy.DataBind();
            }

            var finishedGoodItems =
              (
                  from
                      packagingItem in packagingItems
                  where
                      packagingItem.PackagingComponent.Contains("Finished Good")
                  select
                     packagingItem
              ).ToList();
            if (finishedGoodItems.Count > 0)
            {
                divFG.Visible = true;
                rptFG.DataSource = finishedGoodItems;
                rptFG.DataBind();
            }
        }
        private ApprovalItem ConstructApprovalData()
        {
            var item = new ApprovalItem();

            try
            {
                item.CompassListItemId = iItemId;
                item.ModifiedDate = DateTime.Now.ToString();
                item.ModifiedBy = SPContext.Current.Web.CurrentUser.Name;
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError("Unexpected Error Occurred: ConstructApprovalData", this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BEQRC.ToString() + ": " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.BEQRC.ToString(), "ConstructApprovalData");
                return null;
            }

            return item;
        }
        #endregion
        #region Button Methods
        private Boolean SaveBEQRC(bool bSubmitted)
        {
            ApprovalItem approvalItem;
            Boolean bSuccess = true;
            try
            {
                // Set the approval data
                approvalItem = ConstructApprovalData();
                BEQRCService.UpdateBEQRCItem(new BEQRCItem() { CompassListItemId = iItemId, ConsumerFacingProdDesc = txtConsumerFacingProdDesc.Text });
                BEQRCService.UpdateBEQRCApprovalItem(approvalItem, bSubmitted);
            }
            catch (Exception exception)
            {
                bSuccess = false;
                ErrorSummary.AddError(exception.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BEQRC.ToString() + ": SaveBEQRC(main): " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.BEQRC.ToString(), "SaveBEQRC(main)");
            }

            return bSuccess;
        }
        protected void btnReloadAttachment_Click(object sender, EventArgs e)
        {
            btnSave_Click(sender, e);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveBEQRC(false);
                SaveAllRepeaterItems();
                ViewState["Changes Saved"] = "Changes Saved: " + DateTime.Now.ToString();
                lblSavedMessage.Text = "Changes Saved: " + DateTime.Now.ToString();
            }
            catch (Exception exception)
            {
                ErrorSummary.AddError("Error occurred while saving: " + exception.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BEQRC.ToString() + ": btnSave_Click: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.BEQRC.ToString(), "btnSave_Click");
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                SaveBEQRC(true);
                SaveAllRepeaterItems();
                // Complete the workflow task
                workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.BEQRC);

                // Disable Submit to only allow 1 submit
                this.btnSubmit.Enabled = false;

                // Redirect to Home page after successfull Submit                        
                Page.Response.Redirect(Utilities.RedirecttoHomePage(), false);
            }
            catch (Exception exception)
            {
                ErrorSummary.AddError(exception.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BEQRC.ToString() + ": btnSubmit_Click: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.BEQRC.ToString(), "btnSubmit_Click");
            }
        }
        protected void btnRequestQRCodes_Click(object sender, EventArgs e)
        {
            try
            {
                BEQRCService.UpdateBEQRCItem(new BEQRCItem() { CompassListItemId = iItemId, ConsumerFacingProdDesc = txtConsumerFacingProdDesc.Text });
                SaveAllRepeaterItems();

                string PackagingComponentsQRCodesTable = BEQRCService.GetPackagingComponentsWithQRCodeForEmail(iItemId);

                if (notificationService.EmailBEQRCRequest(WorkflowStep.BEQRCRequest.ToString(), iItemId, PackagingComponentsQRCodesTable))
                {
                    lblSavedMessage.Text = "Email to Request QR Codes has been sent successfully.";
                    BEQRCService.UpdateBEQRCRequestEmailSent(iItemId);
                }
                else
                {
                    lblSavedMessage.Text = "Sending Email to Request QR Codes failed. Please Try again.";
                }
            }
            catch (Exception exception)
            {
                ErrorSummary.AddError("Error occurred while sending BE QRC Codes request email: " + exception.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BEQRC.ToString() + ": btnRequestQRCodes_Click: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.BEQRC.ToString(), "btnRequestQRCodes_Click");
            }

        }
        protected void lbHelpDeskEmail_Click(object sender, EventArgs e)
        {
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "InTech Regulatory");
            this.divAccessDenied.Visible = false;
            this.divAccessRequest.Visible = true;
        }
        protected void hdnbtnDeleteApprovedGraphicsAsset_Click(object sender, EventArgs e)
        {
            var hdnDeleteApprovedGraphicsAssetUrl = (HiddenField)this.FindControl("hdnDeleteApprovedGraphicsAssetUrl");
            if (hdnDeleteApprovedGraphicsAssetUrl != null && !string.IsNullOrEmpty(hdnDeleteApprovedGraphicsAssetUrl.Value))
            {
                utilityService.DeleteAttachment(hdnDeleteApprovedGraphicsAssetUrl.Value);
                SaveAllRepeaterItems();
            }
        }
        protected void btnhdnDeleteVisualreferenceUrl_Click(object sender, EventArgs e)
        {
            var hdnDeleteVisualreferenceUrl = (HiddenField)this.FindControl("hdnDeleteVisualreferenceUrl");
            if (hdnDeleteVisualreferenceUrl != null && !string.IsNullOrEmpty(hdnDeleteVisualreferenceUrl.Value))
            {
                utilityService.DeleteAttachment(hdnDeleteVisualreferenceUrl.Value);
                SaveAllRepeaterItems();
            }
        }
        protected void btnhdnDeleteBEQRCEPSFileUrl_Click(object sender, EventArgs e)
        {
            var hdnDeleteBEQRCEPSFileUrl = (HiddenField)this.FindControl("hdnDeleteBEQRCEPSFileUrl");
            if (hdnDeleteBEQRCEPSFileUrl != null && !string.IsNullOrEmpty(hdnDeleteBEQRCEPSFileUrl.Value))
            {
                utilityService.DeleteAttachment(hdnDeleteBEQRCEPSFileUrl.Value);
                SaveAllRepeaterItems();
            }
        }
        #endregion
        #region Repeater Methods
        protected void rptCandySemi_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }
        protected void rptPurchasedCandy_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }
        protected void rptFG_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }
        #endregion
        #region Private Methods
        private bool CheckProjectNumber()
        {
            iItemId = Utilities.GetItemIdFromProjectNumber(ProjectNumber);

            if ((iItemId == 0) && (!string.IsNullOrEmpty(ProjectNumber)))
            {
                // Invalid project Id supplied
                Page.Response.Redirect("/_layouts/Compass/AppPages/CompassErrorPage.aspx?ErrorId=1&ProjectNo=" + ProjectNumber, false);
                return false;
            }
            else if (iItemId == 0)
            {
                this.hiddenItemId.Value = iItemId.ToString();
                return false;
            }
            // Store Id in Hidden field
            this.hiddenItemId.Value = iItemId.ToString();
            return true;
        }
        private void EnsureScriptManager()
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);

            if (scriptManager == null)
            {
                scriptManager = new ScriptManager();
                scriptManager.EnablePartialRendering = true;
                scriptManager.AsyncPostBackTimeout = 900;


                if (Page.Form != null)
                {
                    Page.Form.Controls.AddAt(0, scriptManager);
                }
            }
            else
            {
                scriptManager.AsyncPostBackTimeout = 900;
            }
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
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "EnsureUpdatePanelFixups", "SAPNomenclature()", false);
        }
        private Boolean SaveAllRepeaterItems()
        {
            GetuserControls();
            LoadBOMItems();

            return true;
        }
        private void LoadBOMItems()
        {
            List<PackagingItem> allPIs = new List<PackagingItem>();
            List<FileAttribute> allFiles = new List<FileAttribute>();
            allPIs = packagingItemService.GetAllPackagingItemsForProject(iItemId);
            allFiles = packagingItemService.GetRenderingUploadedFiles(ProjectNumber);
            allFiles.AddRange(packagingItemService.GetApprovedGraphicsAssetUploadedFiles(ProjectNumber));
            allFiles.AddRange(BEQRCService.GetBEQRCodeEPSFileUploadedFiles(ProjectNumber));
            ucBEQRC ctrl = (ucBEQRC)Page.LoadControl(_ucBEQRCPath);

            ctrl.ParentId = 0;
            ctrl.MaterialNumber = "";
            ctrl.MaterialDesc = "";
            ctrl.ParentComponentType = "";
            ctrl.ProjectNumber = ProjectNumber;
            ctrl.projectAttachments = allFiles;
            ctrl.AllPIs = allPIs;
            ctrl.CompassItemId = iItemId;
            ctrl.ID = "FinishedGood";
            ctrl.UPCAssociated = UPCAssociated;
            phFG.Controls.Clear();
            phTS.Controls.Clear();
            phFG.Controls.Add(ctrl);
        }
        public void GetuserControls()
        {
            //foreach (var ctrl in phFG.Controls)
            //{
            //    if (ctrl is System.Web.UI.UserControl)
            //    {
            //        var type = (ucBEQRC)ctrl;

            //        type.saveData();

            //    }
            //}
            saveData();
        }
        public void saveData()
        {
            List<PlaceHolder> ipfUC = new List<PlaceHolder>();
            ipfUC.Add(phFG);
            ipfUC.Add(phTS);
            List<PackagingItem> PIsToSave = new List<PackagingItem>();
            foreach (PlaceHolder phUC in ipfUC)
            {
                foreach (UserControl uc in phUC.Controls)
                {
                    var type = (ucBEQRC)uc;
                    Repeater repeater = (Repeater)type.FindControl("rptTSItem");
                    foreach (RepeaterItem item in repeater.Items)
                    {
                        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                        {
                            PackagingItem packagingItem = new PackagingItem();

                            packagingItem.PackagingComponent = ((DropDownList)item.FindControl("drpComponent")).SelectedItem.Text;
                            packagingItem.NewExisting = ((DropDownList)item.FindControl("drpNew")).SelectedItem.Text;
                            packagingItem.PackQuantity = ((TextBox)item.FindControl("txtPackQty")).Text;
                            packagingItem.PackUnit = ((DropDownList)item.FindControl("drpUnitOfMeasure")).SelectedItem.Text;
                            packagingItem.GraphicsChangeRequired = ((DropDownList)item.FindControl("drpGraphicsNeeded")).SelectedItem.Text;
                            packagingItem.ComponentContainsNLEA = ((DropDownList)item.FindControl("drpComponentContainsNLEA")).SelectedItem.Text;
                            packagingItem.ExternalGraphicsVendor = ((DropDownList)item.FindControl("ddlGraphicsVendor")).SelectedItem.Text;
                            packagingItem.Flowthrough = ((DropDownList)item.FindControl("ddlFlowthrough")).SelectedItem.Text;
                            packagingItem.MaterialNumber = ((TextBox)item.FindControl("txtMaterial")).Text;
                            packagingItem.MaterialDescription = ((TextBox)item.FindControl("txtMaterialDesc")).Text;
                            packagingItem.CurrentLikeItem = ((TextBox)item.FindControl("txtLikeMaterial")).Text;
                            packagingItem.CurrentLikeItemDescription = ((TextBox)item.FindControl("txtLikeMaterialDesc")).Text;
                            packagingItem.CurrentOldItem = ((TextBox)item.FindControl("txtOldMaterial")).Text;
                            packagingItem.CurrentOldItemDescription = ((TextBox)item.FindControl("txtOldMaterialDesc")).Text;
                            if (packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi || packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi)
                            {
                                packagingItem.PHL1 = ((DropDownList)item.FindControl("ddlPHL1")).SelectedItem.Text;
                                packagingItem.PHL2 = ((DropDownList)item.FindControl("ddlPHL2")).SelectedItem.Text;
                                packagingItem.Brand = ((DropDownList)item.FindControl("ddlBrand")).SelectedItem.Text;
                                packagingItem.ProfitCenter = ((TextBox)item.FindControl("txtProfitCenterUC")).Text;
                                packagingItem.ProfitCenter = ((HiddenField)item.FindControl("hdnProfitCenterUC")).Value;
                            }
                            packagingItem.CurrentLikeItemReason = ((TextBox)item.FindControl("txtLikeReason")).Text;
                            packagingItem.GraphicsBrief = ((TextBox)item.FindControl("txtGraphicsBrief")).Text;
                            packagingItem.UPCAssociated = ((DropDownList)item.FindControl("ddlUPCAssociated")).SelectedItem.Text;
                            packagingItem.UPCAssociatedManualEntry = ((TextBox)item.FindControl("txtUPCAssociated")).Text;
                            packagingItem.BioEngLabelingRequired = ((DropDownList)item.FindControl("ddlBioEngLabelingRequired")).SelectedItem.Text;
                            packagingItem.FlowthroughMaterialsSpecs = ((TextBox)item.FindControl("txtFlowthroughMaterialsSpecs")).Text;

                            var CompId = ((HiddenField)item.FindControl("hdnItemID")).Value.Replace(",", "");
                            packagingItem.Id = string.IsNullOrEmpty(CompId) ? 0 : Convert.ToInt32(CompId);
                            packagingItem.CompassListItemId = iItemId.ToString();

                            int moveId = Convert.ToInt32(((DropDownList)item.FindControl("ddlMoveTS")).SelectedItem.Value);
                            string movedPackType = ((DropDownList)item.FindControl("ddlMoveTS")).SelectedItem.Text;
                            if (moveId != -1)
                            {
                                packagingItem.ParentID = moveId;
                            }
                            else
                            {
                                var hdnParentId = ((HiddenField)item.FindControl("hdnParentId")).Value;
                                if (!string.IsNullOrEmpty(hdnParentId))
                                {
                                    packagingItem.ParentID = Convert.ToInt32(hdnParentId);
                                }
                            }

                            string deletedStatus = ((HiddenField)item.FindControl("hdnDeletedStatus")).Value;
                            if (deletedStatus == "deleted")
                            {
                                packagingItem.Deleted = "Yes";
                            }


                            if (packagingItem.Id <= 0 && deletedStatus != "deleted")
                            {
                                int newId = packagingItemService.InsertPackagingItem(packagingItem, iItemId);
                                HiddenField hidId = ((HiddenField)item.FindControl("hdnItemID"));
                                if (hidId != null)
                                    hidId.Value = newId.ToString();
                            }
                            else if (packagingItem.Id <= 0 && deletedStatus == "deleted")
                            {

                            }
                            else if (packagingItem.Id > 0 && deletedStatus == "deleted")
                            {
                                packagingItemService.DeletePackagingItem(packagingItem.Id);
                            }
                            else
                            {
                                PIsToSave.Add(packagingItem);
                                //packagingItemService.UpdateIPFPackagingItem(packagingItem);
                            }
                        }
                    }
                }
            }
            if (PIsToSave.Count > 0)
            {
                BEQRCService.UpdateBEQRCPackagingItems(PIsToSave, iItemId);
            }

        }
        private void InitializeScreen()
        {
            // If user does not belong to a valid group for the page, inform them that they do not have access rights
            if (!userMgmtService.HasReadAccess(CompassForm.BEQRC))
            {
                this.divAccessDenied.Visible = true;
            }

            // If user does not have rights to save/submit the page, disable the Save and Submit buttons
            if (!userMgmtService.HasWriteAccess(CompassForm.BEQRC))
            {
                this.btnSave.Enabled = false;
                this.btnSubmit.Enabled = false;
            }

            string workflowPhase = utilityService.GetWorkflowPhase(iItemId);
            if ((string.Equals(workflowPhase, WorkflowStep.OnHold.ToString())) || (string.Equals(workflowPhase, WorkflowStep.Cancelled.ToString())) ||
                (string.Equals(workflowPhase, WorkflowStep.Completed.ToString())))
            {
                this.btnSave.Enabled = false;
                this.btnSubmit.Enabled = false;
            }
        }
        #endregion
    }
}
