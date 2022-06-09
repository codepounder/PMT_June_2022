using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Linq;
using System.ComponentModel;
using Ferrara.Compass.Classes;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Services;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Enum;
using System.Web;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.IO;
using Ferrara.Compass.ControlTemplates.Ferrara.Compass;
using System.Text;

namespace Ferrara.Compass.WebParts.BillOfMaterialsSetUpForm
{
    [ToolboxItemAttribute(false)]
    public partial class BillOfMaterialsSetUpForm : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

        #region Member Variables
        private IPackagingItemService packagingItemService;
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private IUserManagementService userManagementService;
        private INotificationService notificationService;
        private IBillOfMaterialsService billOfMaterialsService;
        private IWorkflowService workflowService;
        private IConfigurationManagementService configurationService;
        private ISAPBOMService sapBOMService;
        private ISAPMaterialMasterService sapMMService;
        private string webUrl;
        private int iItemId = 0;
        private List<PackagingItem> packagingItems = new List<PackagingItem>();
        private List<ShipperFinishedGoodItem> shipperFGItems = new List<ShipperFinishedGoodItem>();
        private const string _ucBOMPackMeas = @"~/_controltemplates/15/Ferrara.Compass/ucBOMPackMeas.ascx";
        private const string _ucBOMEditable = @"~/_controltemplates/15/Ferrara.Compass/ucBOMEditable.ascx";
        private const string _ucBOMGrid = @"~/_controltemplates/15/Ferrara.Compass/ucBOMGrid.ascx";

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
        #endregion
        public BillOfMaterialsSetUpForm()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();

            packagingItemService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            userManagementService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
            configurationService = DependencyResolution.DependencyMapper.Container.Resolve<IConfigurationManagementService>();
            sapBOMService = DependencyResolution.DependencyMapper.Container.Resolve<ISAPBOMService>();
            sapMMService = DependencyResolution.DependencyMapper.Container.Resolve<ISAPMaterialMasterService>();
            billOfMaterialsService = DependencyResolution.DependencyMapper.Container.Resolve<IBillOfMaterialsService>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            setTitle();

            this.Page.Form.Enctype = "multipart/form-data";
            webUrl = SPContext.Current.Web.Url;

            if (!Page.IsPostBack)
            {
                this.divAccessDenied.Visible = false;
                this.divAccessRequest.Visible = false;

                // Check for a valid project number
                if (!CheckProjectNumber())
                    return;

                LoadFormData();
            }
            else
            {
                iItemId = Convert.ToInt32(hiddenItemId.Value);
            }
            hdnComponentStatusChangeIds.Value = "";
            LoadBOMGrid();
        }

        #region Private Methods
        private bool CheckProjectNumber()
        {
            iItemId = Utilities.GetItemIdFromProjectNumber(ProjectNumber);

            if (iItemId == 0)
            {
                // Invalid project Id supplied
                Page.Response.Redirect("/_layouts/Compass/AppPages/CompassErrorPage.aspx?ErrorId=1&ProjectNo=" + ProjectNumber, false);
                this.hiddenItemId.Value = iItemId.ToString();
                return false;
            }

            // Store Id in Hidden field
            this.hiddenItemId.Value = iItemId.ToString();
            return true;
        }

        private void setTitle()
        {
            if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_PE.ToLower()))
            {
                pageHead.InnerText = "Bill of Materials Setup: PE1";
            }
            else if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_PE2.ToLower()))
            {
                pageHead.InnerText = "Bill of Materials Setup: PE2";
            }
            else if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_Proc.ToLower()))
            {
                pageHead.InnerText = "Bill of Materials Setup: PROC";
            }
        }
        private bool CheckWriteAccess()
        {
            if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_PE.ToLower()))
            {
                if (userManagementService.HasWriteAccess(CompassForm.BOMSetupPE))
                {
                    return true;
                }
            }
            else if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_PE2.ToLower()))
            {
                if (userManagementService.HasWriteAccess(CompassForm.BOMSetupPE2))
                {
                    return true;
                }
            }
            else if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_Proc.ToLower()))
            {
                if (userManagementService.HasWriteAccess(CompassForm.BOMSetupProc))
                {
                    return true;
                }
            }

            return false;
        }
        private void CompleteWorkflowTask()
        {
            if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_PE.ToLower()))
            {
                workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.BOMSetupPE);
            }
            else if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_PE2.ToLower()))
            {
                workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.BOMSetupPE2);
            }
            else if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_Proc.ToLower()))
            {
                try
                {
                    List<string> wftasks = hdnWorkflowSteps.Value.Split(';').ToList();
                    foreach (string task in wftasks)
                    {
                        workflowService.CompleteWorkflowTask(iItemId, (WorkflowStep)Enum.Parse(typeof(WorkflowStep), task));
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.BOMSetupProc);
                    }
                    catch (Exception e)
                    {

                    }
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupProc.ToString() + ": CompleteWorkflowTask: " + ex.Message);
                    exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.BOMSetupProc.ToString(), "CompleteWorkflowTask");
                }
            }
        }
        private bool RequiredFieldCheckForPackagingComponent()
        {
            List<string> completedCompIds = new List<string>();
            List<string> lstrequiredItesms = new List<string>();
            List<PackagingItem> totalPackingItem = new List<PackagingItem>();
            List<PackagingItem> dtPackingItem = new List<PackagingItem>();
            string taskAssigned = "";
            bool iserror = false;
            if (hdnComponentStatusChangeIds.Value != "")
            {
                completedCompIds = hdnComponentStatusChangeIds.Value.Split(',').ToList();
            }
            dtPackingItem = packagingItemService.GetFinishedGoodItemsForProject(iItemId);

            totalPackingItem.AddRange(dtPackingItem);

            foreach (PackagingItem item in dtPackingItem)
            {
                if (string.Equals(item.PackagingComponent.ToLower(), GlobalConstants.COMPONENTTYPE_TransferSemi.ToLower()) || string.Equals(item.PackagingComponent.ToLower(), GlobalConstants.COMPONENTTYPE_PurchasedSemi.ToLower()))
                {
                    List<PackagingItem> dtSemiPackingItem = new List<PackagingItem>();
                    dtSemiPackingItem = packagingItemService.GetSemiBOMItems(iItemId, item.Id);
                    totalPackingItem.AddRange(dtSemiPackingItem);
                }
            }
            bool NonPROCPage = !string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_Proc.ToLower());
            try
            {
                taskAssigned = hdnWorkflowSteps.Value.Split(';').ToList()[0];
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupProc.ToString() + ": hdnWorkflowSteps.Value.Split(';').ToList()[0]: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.BOMSetupProc.ToString(), "hdnWorkflowSteps.Value.Split(';').ToList()[0]");
            }
            if (NonPROCPage)
            {
                completedCompIds = completedCompIds.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                foreach (var item in totalPackingItem)
                {
                    int isCompleted = (from id in completedCompIds where Convert.ToInt32(id) == item.Id select id).Count();
                    if (isCompleted <= 0)
                    {
                        ErrorSummary.AddError("Please complete component information: " + item.PackagingComponent, this.Page);
                        iserror = true;
                    }
                    if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_PE2.ToLower()))
                    {
                        if (!(item.PackagingComponent.ToLower().Contains("transfer")) && !(item.PackagingComponent.ToLower().Contains("candy")) && !(item.PackagingComponent.ToLower().Contains("finished")))
                        {
                            // CAD Drawings are only required for New Components
                            if (item.NewExisting.ToLower().Contains("new"))
                            {
                                var filesCAD = packagingItemService.GetUploadedFiles(ProjectNumber, item.Id, GlobalConstants.DOCTYPE_CADDrawing);

                                if (filesCAD.Count == 0)
                                {
                                    ErrorSummary.AddError("Please upload CAD Drawing documents: " + item.PackagingComponent, this.Page);
                                    iserror = true;
                                }
                            }
                        }
                    }
                }
            }
            else if (!NonPROCPage && (taskAssigned == "BOMSetupProcCoMan" || taskAssigned == "BOMSetupProcNovelty" || taskAssigned == "BOMSetupProcSeasonal"))// || taskAssigned == "BOMSetupProcExternal"))
            {
                completedCompIds = completedCompIds.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                List<int> newCompletedCompIds = new List<int>();
                List<string> newCompletedCompRevPrinter = new List<string>();
                foreach (string compString in completedCompIds)
                {
                    int colonIndex = compString.IndexOf(":");
                    int semiIndex = compString.IndexOf(";");
                    string compId = compString.Substring(0, colonIndex);
                    int result;
                    if (int.TryParse(compId, out result))
                    {
                        newCompletedCompIds.Add(result);
                    }
                    string revPrinterSupplier = compString.Substring(semiIndex + 1);
                    newCompletedCompRevPrinter.Add(revPrinterSupplier);
                }
                foreach (var item in totalPackingItem)
                {
                    int isCompleted = (from id in newCompletedCompIds where Convert.ToInt32(id) == item.Id select id).Count();
                    int currentID = newCompletedCompIds[0];
                    if (isCompleted <= 0)
                    {
                        ErrorSummary.AddError("Please complete component information: " + currentID, this.Page);
                        iserror = true;
                    }
                    if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_PE2.ToLower()))
                    {
                        if (!(item.PackagingComponent.ToLower().Contains("transfer")) && !(item.PackagingComponent.ToLower().Contains("candy")) && !(item.PackagingComponent.ToLower().Contains("finished")))
                        {
                            // CAD Drawings are only required for New Components
                            if (item.NewExisting.ToLower().Contains("new"))
                            {
                                var filesCAD = packagingItemService.GetUploadedFiles(ProjectNumber, item.Id, GlobalConstants.DOCTYPE_CADDrawing);

                                if (filesCAD.Count == 0)
                                {
                                    ErrorSummary.AddError("Please upload CAD Drawing documents: " + item.PackagingComponent, this.Page);
                                    iserror = true;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                string projectType = lblProjectType.Text;
                string subCat = lblProjectSubcat.Text;
                try
                {
                    List<string> currentTasks = hdnWorkflowSteps.Value.Split(';').ToList();
                    List<PackagingItem> applicableItems = new List<PackagingItem>();
                    foreach (string task in currentTasks)
                    {
                        if (!string.IsNullOrEmpty(task))
                        {
                            string taskType = "";
                            if (task.Contains("BOMSetupProcSeasonal"))
                            {
                                taskType = task.Substring(20);
                            }
                            else if (task.Contains("BOMSetupProcExternal"))
                            {
                                taskType = task.Substring(20);
                            }
                            else if (task.Contains("BOMSetupProcEBP"))
                            {
                                taskType = task.Substring(15);
                            }
                            List<PackagingItem> taskItems = new List<PackagingItem>();

                            if (projectType == GlobalConstants.PROJECTTYPE_SimpleNetworkMove || subCat == GlobalConstants.PROJECTTYPESUBCATEGORY_ComplexNetworkMove)
                            {
                                taskItems = (from PI in totalPackingItem where PI.PackagingComponent.ToLower().Contains(taskType.ToLower()) select PI).ToList<PackagingItem>();
                            }
                            else
                            {
                                taskItems = (from PI in totalPackingItem where PI.PackagingComponent.ToLower().Contains(taskType.ToLower()) && (PI.NewExisting == "New" || PI.NewExisting == "Network Move") select PI).ToList<PackagingItem>();
                            }
                            if (task.Contains("BOMSetupProcExternal"))
                            {
                                taskItems = (from TI in taskItems where TI.ReviewPrinterSupplier == "Yes" select TI).ToList<PackagingItem>();
                            }
                            else
                            {
                                taskItems = (from TI in taskItems where TI.ReviewPrinterSupplier != "Yes" select TI).ToList<PackagingItem>();
                            }

                            applicableItems.AddRange(taskItems);
                        }
                    }
                    completedCompIds = completedCompIds.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                    List<KeyValuePair<int, string>> newCompletedCompIds = new List<KeyValuePair<int, string>>();
                    foreach (string compString in completedCompIds)
                    {
                        int colonIndex = compString.IndexOf(":");
                        int semiIndex = compString.IndexOf(";");
                        string compId = compString.Substring(0, colonIndex);
                        int result;
                        if (int.TryParse(compId, out result))
                        {
                            string revPrinterSupplier = compString.Substring(semiIndex + 1);
                            newCompletedCompIds.Add(new KeyValuePair<int, string>(result, revPrinterSupplier));
                        }

                    }
                    foreach (PackagingItem item in applicableItems)
                    {

                        int isCompleted = (from id in newCompletedCompIds where Convert.ToInt32(id.Key) == item.Id select id).Count();
                        if (isCompleted <= 0)
                        {
                            ErrorSummary.AddError("Please complete component information: " + item.PackagingComponent, this.Page);
                            iserror = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupProc.ToString() + ": RequiredFieldCheckForPackagingComponent: " + ex.Message);
                    exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.BOMSetupProc.ToString(), "RequiredFieldCheckForPackagingComponent");
                }
            }
            if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_PE2.ToLower()))
            {
                CompassPackMeasurementsItem compassPackMeasurementsItem = billOfMaterialsService.GetPackMeasurementsItem(iItemId, 0);

                if (compassPackMeasurementsItem.PackTrialNeeded.ToLower() != "no")
                {
                    var files = packagingItemService.GetUploadedFiles(ProjectNumber, 99999, GlobalConstants.DOCTYPE_PackTrial);
                    if (files.Count == 0)
                    {
                        ErrorSummary.AddError("Please upload pack trial documents", this.Page);
                        iserror = true;
                    }
                }
            }
            return iserror;
        }
        #endregion

        #region Data Transfer Methods
        private void LoadFormData()
        {
            hdnPageName.Value = Utilities.GetCurrentPageName();
            hdnCompassListItemId.Value = iItemId.ToString();

            BillofMaterialsItem billofMaterialsItem = billOfMaterialsService.GetBillOfMaterialsItem(iItemId);
            hdnMaterialDesc.Value = billofMaterialsItem.SAPDescription;
            hdnMaterialNumber.Value = billofMaterialsItem.SAPItemNumber;
            txtWorkCenterAddInfo.Text = billofMaterialsItem.WorkCenterAddInfo;
            lblPegHoleNeeded.Text = billofMaterialsItem.PegHoleNeeded;
            lblItemConcept.Text = billofMaterialsItem.ItemConcept;
            lblFGLikeItem.Text = billofMaterialsItem.FGLikeItem;

            CompassPackMeasurementsItem compassPackMeasurementsItem = billOfMaterialsService.GetPackMeasurementsItem(iItemId, 0);

            // Project Team tab
            // Set the Initiator people picker
            var users = Utilities.SetPeoplePickerValue(billofMaterialsItem.Initiator, SPContext.Current.Web);
            if (!string.IsNullOrEmpty(users))
            {
                peInitiator.Text = users.Remove(users.LastIndexOf(","), 1);
                peInitiator.Text = Utilities.GetPersonFieldForDisplay(billofMaterialsItem.Initiator);
            }

            // Set the Brand Manager people picker
            peBrandManager.Text = billofMaterialsItem.MarketingName;

            // Set the PM people picker
            peOBM.Text = billofMaterialsItem.PMName;

            // Set Research-Development Members
            peResearch.Text = billofMaterialsItem.InTechName;

            // Set PackagingEngineer Members
            lblPackagingEngineerValue.Text = billofMaterialsItem.PackagingEngineeringName;

            //Set hidden PE Lead for Submission
            hdnPELead.Value = billofMaterialsItem.PackagingEngineerLead;
            lblProjectType.Text = billofMaterialsItem.ProjectType;
            lblProjectSubcat.Text = billofMaterialsItem.ProjectSubcat;
            lblPackLocation.Text = billofMaterialsItem.PackingLocation;
        }

        private void LoadBOMGrid()
        {
            List<PackagingItem> dtPackingItem = new List<PackagingItem>();

            phBOM.Controls.Clear();
            dtPackingItem = packagingItemService.GetFinishedGoodItemsForProject(iItemId);

            foreach (PackagingItem item in dtPackingItem)
            {

                if (item.PackagingComponent.ToLower().Contains("transfer") || item.PackagingComponent.ToLower().Contains("purchased candy"))
                {
                    ucBOMGrid ctrl2 = (ucBOMGrid)Page.LoadControl(_ucBOMGrid);
                    ctrl2.openBtnSave = openBtnSave;
                    ctrl2.PackagingComponent = item.PackagingComponent;
                    ctrl2.ParentId = item.Id;
                    ctrl2.MaterialNumber = item.MaterialNumber;
                    ctrl2.MaterialDesc = item.MaterialDescription;
                    ctrl2.SemiXferMakeLocation = item.TransferSEMIMakePackLocations;
                    phBOM.Controls.Add(ctrl2);

                    List<PackagingItem> dtChildPackingItem = new List<PackagingItem>();
                    dtChildPackingItem = packagingItemService.GetSemiChildTSBOMItems(iItemId, item.Id, item.PackagingComponent);
                    int newItemCountChild = dtChildPackingItem.Select(r => r.NewExisting.ToLower() == "new").Count();

                    ucBOMPackMeas packMeas2 = (ucBOMPackMeas)Page.LoadControl(_ucBOMPackMeas);
                    packMeas2.MaterialDesc = item.MaterialDescription;
                    packMeas2.MaterialNumber = item.MaterialNumber;
                    packMeas2.PackagingComponent = item.PackagingComponent;
                    packMeas2.ParentId = item.Id;
                    packMeas2.NewExisting = item.NewExisting;
                    packMeas2.SemiXferMakeLocation = item.TransferSEMIMakePackLocations;
                    packMeas2.NewComponentCount = newItemCountChild;
                    phBOM.Controls.Add(packMeas2);


                    foreach (PackagingItem childItem in dtChildPackingItem)
                    {
                        ucBOMGrid ctrl3 = (ucBOMGrid)Page.LoadControl(_ucBOMGrid);
                        ctrl3.openBtnSave = openBtnSave;
                        ctrl3.PackagingComponent = childItem.PackagingComponent;
                        ctrl3.ParentId = childItem.Id;
                        ctrl3.MaterialNumber = childItem.MaterialNumber;
                        ctrl3.MaterialDesc = childItem.MaterialDescription;
                        ctrl3.SemiXferMakeLocation = childItem.TransferSEMIMakePackLocations;
                        ctrl3.isChildItem = true;
                        phBOM.Controls.Add(ctrl3);


                        ucBOMPackMeas packMeas3 = (ucBOMPackMeas)Page.LoadControl(_ucBOMPackMeas);
                        packMeas3.MaterialDesc = childItem.MaterialDescription;
                        packMeas3.MaterialNumber = childItem.MaterialNumber;
                        packMeas3.PackagingComponent = childItem.PackagingComponent;
                        packMeas3.ParentId = childItem.Id;
                        packMeas3.SemiXferMakeLocation = childItem.TransferSEMIMakePackLocations;
                        packMeas3.NewExisting = childItem.NewExisting;
                        ctrl3.isChildItem = true;
                        phBOM.Controls.Add(packMeas3);
                    }
                }
            }
            ucBOMGrid ctrl = (ucBOMGrid)Page.LoadControl(_ucBOMGrid);
            ctrl.PackagingComponent = "";
            ctrl.ParentId = 0;
            ctrl.MaterialNumber = hdnMaterialNumber.Value;
            ctrl.MaterialDesc = hdnMaterialDesc.Value;
            ctrl.openBtnSave = openBtnSave;
            phBOM.Controls.Add(ctrl);

            int newItemCount = dtPackingItem.Select(r => r.NewExisting.ToLower() == "new").Count();
            string NewExistingFG = billOfMaterialsService.getProjectNewExisting(iItemId);
            ucBOMPackMeas packMeas = (ucBOMPackMeas)Page.LoadControl(_ucBOMPackMeas);
            packMeas.MaterialDesc = hdnMaterialDesc.Value;
            packMeas.MaterialNumber = hdnMaterialNumber.Value;
            packMeas.PackagingComponent = "";
            packMeas.ParentId = 0;
            packMeas.NewExisting = NewExistingFG;
            packMeas.NewComponentCount = newItemCount;
            phBOM.Controls.Add(packMeas);


            if (hdnPageState.Value == "PE")
            {
                var ctrlPE = (ucBOMEditable)Page.LoadControl(_ucBOMEditable);
                ctrlPE.PackagingComponent = hdnPackagingComponent.Value;
                ctrlPE.PackagingItemId = Convert.ToInt32(hdnPackagingID.Value);
                ctrlPE.ParentID = hdnParentID.Value;
                // Add messages to page
                phMsg.Controls.Add(ctrlPE);
            }
        }
        public void GetuserControls()
        {
            foreach (var ctrl in phBOM.Controls)
            {
                if (ctrl is ucBOMPackMeas)
                {
                    var type = (ucBOMPackMeas)ctrl;

                    type.saveData();

                }
                if (ctrl is ucBOMGrid)
                {
                    var type2 = (ucBOMGrid)ctrl;

                    type2.saveData();

                }
            }
        }

        private BillofMaterialsItem ConstructFormData(bool submitted)
        {
            BillofMaterialsItem bomItem = new BillofMaterialsItem();
            try
            {
                bomItem.CompassListItemId = iItemId;
                bomItem.PackagingNumbers = hdnPackagingNumbers.Value;
                if (submitted)
                {
                    if (string.IsNullOrEmpty(hdnPELead.Value))
                    {
                        if (Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_PackagingEngineer))
                        {
                            bomItem.PackagingEngineerLead = SPContext.Current.Web.CurrentUser.ID.ToString() + ";#" + SPContext.Current.Web.CurrentUser.LoginName;
                        }
                        else
                        {
                            bomItem.PackagingEngineerLead = GlobalConstants.GROUP_PackagingEngineer;
                        }
                    }
                    else
                    {
                        bomItem.PackagingEngineerLead = hdnPELead.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupPE.ToString() + ": ConstructFormData: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.BOMSetupPE.ToString(), "ConstructFormData");
            }
            return bomItem;
        }
        private ApprovalItem ConstructApprovalData()
        {
            var item = new ApprovalItem();

            item.CompassListItemId = iItemId;
            item.ModifiedBy = SPContext.Current.Web.CurrentUser.Name;
            item.ModifiedDate = DateTime.Now.ToString();

            return item;
        }
        #endregion
        public void openBtnSave()
        {
            try
            {
                if (!CheckWriteAccess())
                {
                    ErrorSummary.AddError("You do not have proper access rights to submit this page!", this.Page);
                    return;
                }
                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }
                GetuserControls();
                BillofMaterialsItem item = ConstructFormData(false);
                billOfMaterialsService.UpdateBillOfMaterialsItem(item, Utilities.GetCurrentPageName());

                ApprovalItem approvalItem = ConstructApprovalData();
                billOfMaterialsService.UpdateBillofMaterialsApprovalItem(approvalItem, Utilities.GetCurrentPageName(), false);

                lblSaved.Text = "Changes Saved: " + DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupPE.ToString() + ": btnSave_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.BOMSetupPE.ToString(), "btnSave_Click");
            }
        }
        #region Button Methods
        protected void btnSave_Click(object sender, EventArgs e)
        {
            openBtnSave();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckWriteAccess())
                {
                    ErrorSummary.AddError("You do not have proper access rights to submit this page!", this.Page);
                    return;
                }
                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }

                GetuserControls();

                var iserror = RequiredFieldCheckForPackagingComponent();

                if (iserror)
                {
                    ItemValidationSummary.HeaderText = "<strong><p style='font-size:medium'>Submit Failed:</p></strong><br/>";
                    return;
                }

                BillofMaterialsItem item = ConstructFormData(true);
                billOfMaterialsService.UpdateBillOfMaterialsItem(item, Utilities.GetCurrentPageName());

                ApprovalItem approvalItem = ConstructApprovalData();
                if (!string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_Proc.ToLower()))
                {
                    billOfMaterialsService.UpdateBillofMaterialsApprovalItem(approvalItem, Utilities.GetCurrentPageName(), true);
                }
                // Complete the workflow task
                CompleteWorkflowTask();

                // Redirect to Home page after successfull Submit 
                Page.Response.Redirect(Utilities.RedirecttoHomePage(), false);
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupPE.ToString() + ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.BOMSetupPE.ToString(), "btnSubmit_Click");
            }
        }
        #endregion
    }
}
