using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Practices.Unity;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.DependencyResolution;
using Ferrara.Compass.Classes;
using Microsoft.SharePoint;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Web;
using System.Xml;
using System.Net;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.WebParts.CompassAdministrationForm
{
    public class XLSDets
    {
        public string SAPNumber = "";
        public string oldPHL1 = "";
        public string newPHL1 = "";
        public string oldPHL2 = "";
        public string newPHL2 = "";
        public string oldBrand = "";
        public string newBrand = "";
        public string newProfitCenter = "";
        public string oldPackType = "";
        public string newPackType = "";
        public string profitCenter = "";
        public string projectNumber = "";
        public string newTradePromo = "";
        public string oldTradePromo = "";
        public string oldProfitCenter = "";
        public string PM = "";
        public string oldSingleColumn = "";
        public string newSingleColumn = "";
    }
    public partial class CompassAdministrationFormUserControl : UserControl
    {

        private IUtilityService utilityService;
        private IBillOfMaterialsService materialService;
        private IPackagingItemService packagingItemService;
        private IApprovalService approvalService;
        private ILoggerService exceptionService;
        private IExcelExportSyncService excelService;
        private IConfigurationManagementService configurationManagementService;
        private System.Collections.Generic.List<WFStepField> wfAllSteps;
        private string totalRecords;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            materialService = DependencyResolution.DependencyMapper.Container.Resolve<IBillOfMaterialsService>();
            packagingItemService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            approvalService = DependencyResolution.DependencyMapper.Container.Resolve<IApprovalService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<ILoggerService>();
            excelService = DependencyResolution.DependencyMapper.Container.Resolve<IExcelExportSyncService>();
            configurationManagementService = DependencyResolution.DependencyMapper.Container.Resolve<IConfigurationManagementService>();

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Get the list of all Workflow Steps
            //wfAllSteps = utilityService.GetWorkflowSteps();

            lblUpdateCancelledDate.Visible = false;
            lblUpdatePreProductionDate.Visible = false;
        }
        protected void btnDeleteParentTeam_Click(object sender, EventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList TeamList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);
                        if (TeamList == null)
                        {
                            ErrorSummary.AddError(string.Concat("Unable to find List: ", GlobalConstants.LIST_StageGateProjectListName), this.Page);
                            return;
                        }

                        SPList CompassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);

                        // Get all the old Logs
                        SPQuery spQuery2 = new SPQuery();
                        spQuery2.Query = "<Where><IsNotNull><FieldRef Name=\"" + CompassListFields.AllProjectUsers + "\" /></IsNotNull></Where>";
                        SPListItemCollection compassItemCol2 = CompassList.GetItems(spQuery2);
                        if (compassItemCol2.Count > 0)
                        {
                            foreach (SPListItem item in compassItemCol2)
                            {
                                if (item != null)
                                {
                                    item[CompassListFields.AllProjectUsers] = string.Empty;
                                    item.Update();
                                }
                            }
                        }
                    }
                }
            });
        }
        protected void btnDeleteChildTeam_Click(object sender, EventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList TeamList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTeamListName);
                        if (TeamList == null)
                        {
                            ErrorSummary.AddError(string.Concat("Unable to find List: ", GlobalConstants.LIST_CompassTeamListName), this.Page);
                            return;
                        }

                        SPList CompassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        // Get all the old Logs
                        SPQuery spQuery2 = new SPQuery();
                        spQuery2.Query = "<Where><IsNotNull><FieldRef Name=\"" + CompassListFields.AllProjectUsers + "\" /></IsNotNull></Where>";
                        SPListItemCollection compassItemCol2 = CompassList.GetItems(spQuery2);
                        if (compassItemCol2.Count > 0)
                        {
                            foreach (SPListItem item in compassItemCol2)
                            {
                                if (item != null)
                                {
                                    item[CompassListFields.AllProjectUsers] = string.Empty;
                                    item.Update();
                                }
                            }
                        }
                    }
                }
            });
        }
        protected void btnCopyChildTeam_Click(object sender, EventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList TeamList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTeamListName);
                        if (TeamList == null)
                        {
                            ErrorSummary.AddError(string.Concat("Unable to find List: ", GlobalConstants.LIST_CompassTeamListName), this.Page);
                            return;
                        }

                        SPList CompassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        // Get all the old Logs
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><IsNull><FieldRef Name=\"" + CompassListFields.AllProjectUsers + "\" /></IsNull></Where>";
                        spQuery.RowLimit = 200;
                        SPListItemCollection compassItemCol = CompassList.GetItems(spQuery);
                        SPView TeamlistView = TeamList.Views["All Items"];
                        SPViewFieldCollection TeamviewfieldCollection = TeamlistView.ViewFields;

                        SPView CompasslistView = CompassList.Views["All Items"];
                        SPViewFieldCollection CompassviewfieldCollection = CompasslistView.ViewFields;

                        foreach (SPListItem compassItem in compassItemCol)
                        {
                            List<string> userIDs = new List<string>();
                            foreach (string viewFieldName in CompassviewfieldCollection)
                            {
                                try
                                {
                                    SPField columnDetails = CompassList.Fields.GetField(viewFieldName);
                                    SPFieldType fieldType = columnDetails.Type;
                                    if (fieldType == SPFieldType.User && !viewFieldName.Contains("Modified"))
                                    {
                                        string users = Convert.ToString(compassItem[columnDetails.InternalName]);
                                        if (string.IsNullOrEmpty(users)) { continue; }
                                        userIDs.AddRange(LoadProjectTeamMembers(users));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    continue;
                                }
                            }
                            try
                            {
                                string itemId = compassItem.ID.ToString();
                                spQuery = new SPQuery();
                                spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                                spQuery.RowLimit = 1;
                                SPListItemCollection teamItemCol = TeamList.GetItems(spQuery);
                                if (teamItemCol.Count > 0)
                                {
                                    if (teamItemCol[0] != null)
                                    {
                                        SPListItem team = teamItemCol[0];
                                        foreach (string viewFieldName in TeamviewfieldCollection)
                                        {
                                            //get fields based on the columns you need, might get an exception if the field doesn't exist
                                            try
                                            {
                                                SPField columnDetails = TeamList.Fields.GetField(viewFieldName);
                                                SPFieldType fieldType = columnDetails.Type;
                                                if (fieldType == SPFieldType.User && !viewFieldName.Contains("Modified"))
                                                {
                                                    string users = Convert.ToString(team[columnDetails.InternalName]);
                                                    userIDs.AddRange(LoadProjectTeamMembers(users));
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                continue;
                                            }
                                        }

                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ErrorSummary.AddError(ex.Message, this.Page);
                            }
                            List<string> finalIds = userIDs.Distinct().ToList();
                            compassItem[CompassListFields.AllProjectUsers] = "," + string.Join(",", finalIds) + ",";
                            compassItem.Update();
                        }

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        protected void btnCopyParentTeam_Click(object sender, EventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList SGSList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);

                        // Get all the old Logs
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><IsNull><FieldRef Name=\"" + CompassListFields.AllProjectUsers + "\" /></IsNull></Where>";
                        spQuery.RowLimit = 200;
                        SPListItemCollection compassItemCol = SGSList.GetItems(spQuery);

                        SPView CompasslistView = SGSList.Views["All Items"];
                        SPViewFieldCollection CompassviewfieldCollection = CompasslistView.ViewFields;

                        foreach (SPListItem compassItem in compassItemCol)
                        {
                            List<string> userIDs = new List<string>();
                            foreach (string viewFieldName in CompassviewfieldCollection)
                            {
                                try
                                {
                                    /*SPField columnDetails = SGSList.Fields[viewFieldName];
                                    SPFieldType fieldType = columnDetails.Type;
                                    if (fieldType == SPFieldType.User)
                                    {
                                        string users = Convert.ToString(compassItem[columnDetails.InternalName]);
                                        if (string.IsNullOrEmpty(users)) { continue; }
                                        foreach (string user in users.Split(',').ToList())
                                        {
                                            int number;
                                            bool isNumber = Int32.TryParse(user.Split(';').ToList()[0], out number);
                                            if (isNumber)
                                            {
                                                userIDs.Add(number.ToString());
                                            }
                                        }
                                    }*/
                                    SPField columnDetails = SGSList.Fields.GetField(viewFieldName);
                                    SPFieldType fieldType = columnDetails.Type;
                                    if (fieldType == SPFieldType.User && !viewFieldName.Contains("Modified"))
                                    {
                                        string users = Convert.ToString(compassItem[columnDetails.InternalName]);
                                        if (string.IsNullOrEmpty(users)) { continue; }
                                        userIDs.AddRange(LoadProjectTeamMembers(users));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    continue;
                                }
                            }
                            List<string> finalIds = userIDs.Distinct().ToList();
                            compassItem[StageGateProjectListFields.AllProjectUsers] = "," + string.Join(",", finalIds) + ",";
                            compassItem.Update();
                        }

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        protected void btnInsertApprovalList2Item_Click(object sender, EventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        List<KeyValuePair<int, string>> activeProject = new List<KeyValuePair<int, string>>();

                        // Get all Active Projects
                        SPList compassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><And><Neq><FieldRef Name=\"" + CompassListFields.WorkflowPhase + "\" /><Value Type=\"Text\">Cancelled</Value></Neq><Neq><FieldRef Name=\"" + CompassListFields.WorkflowPhase + "\" /><Value Type=\"Text\">Completed</Value></Neq></And></Where>";

                        SPListItemCollection compassItemCol = compassList.GetItems(spQuery);

                        foreach (SPListItem compassItem in compassItemCol)
                        {
                            if (compassItem != null)
                            {
                                activeProject.Add(new KeyValuePair<int, string>(compassItem.ID, Convert.ToString(compassItem[CompassListFields.ProjectNumber])));
                            }
                        }

                        //Add items to ApprovalList
                        foreach (KeyValuePair<int, string> project in activeProject)
                        {
                            SPList approvalList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalList2Name);
                            spQuery = new SPQuery();
                            spQuery.ViewFields = string.Concat(
                                       "<FieldRef Name='" + ApprovalListFields.CompassListItemId + "' />",
                                       "<FieldRef Name='Title' />");
                            spQuery.ViewFieldsOnly = true;
                            spQuery.Query = "<Where><Eq><FieldRef Name=\"" + ApprovalListFields.CompassListItemId + "\" /><Value Type=\"Text\">" + project.Key + "</Value></Eq></Where>";
                            spQuery.RowLimit = 1;
                            SPListItemCollection approvalItems = approvalList.GetItems(spQuery);
                            if (approvalItems.Count > 0)
                            {
                                continue;
                            }
                            else
                            {
                                var item = approvalList.AddItem();

                                item["Title"] = project.Value;
                                item[ApprovalListFields.CompassListItemId] = project.Key;

                                item.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        protected void btnInsertCancelledCompletedApprovalList2Item_Click(object sender, EventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        List<KeyValuePair<int, string>> activeProject = new List<KeyValuePair<int, string>>();

                        // Get all Active Projects
                        SPList compassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Or><Eq><FieldRef Name=\"" + CompassListFields.WorkflowPhase + "\" /><Value Type=\"Text\">Cancelled</Value></Eq><Eq><FieldRef Name=\"" + CompassListFields.WorkflowPhase + "\" /><Value Type=\"Text\">Completed</Value></Eq></Or></Where>";

                        SPListItemCollection compassItemCol = compassList.GetItems(spQuery);

                        foreach (SPListItem compassItem in compassItemCol)
                        {
                            if (compassItem != null)
                            {
                                activeProject.Add(new KeyValuePair<int, string>(compassItem.ID, Convert.ToString(compassItem[CompassListFields.ProjectNumber])));
                            }
                        }

                        //Add items to ApprovalList
                        foreach (KeyValuePair<int, string> project in activeProject)
                        {
                            SPList approvalList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalList2Name);
                            spQuery = new SPQuery();
                            spQuery.ViewFields = string.Concat(
                                       "<FieldRef Name='" + ApprovalListFields.CompassListItemId + "' />",
                                       "<FieldRef Name='Title' />");
                            spQuery.ViewFieldsOnly = true;
                            spQuery.Query = "<Where><Eq><FieldRef Name=\"" + ApprovalListFields.CompassListItemId + "\" /><Value Type=\"Text\">" + project.Key + "</Value></Eq></Where>";
                            spQuery.RowLimit = 1;
                            SPListItemCollection approvalItems = approvalList.GetItems(spQuery);
                            if (approvalItems.Count > 0)
                            {
                                continue;
                            }
                            else
                            {
                                var item = approvalList.AddItem();

                                item["Title"] = project.Value;
                                item[ApprovalListFields.CompassListItemId] = project.Key;

                                item.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        protected void btnPopulateApprovalList2AllFieldsItem_Click(object sender, EventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        List<ApprovalListItem> activeProject = new List<ApprovalListItem>();

                        // Get all Active Projects
                        SPList compassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><IsNotNull><FieldRef Name=\"" + ApprovalListFields.BOMSetupProc_SubmittedDate + "\" /></IsNotNull></Where>";

                        SPListItemCollection compassItemCol = compassList.GetItems(spQuery);
                        ErrorSummary.AddError("Looping Through Approval List", this.Page);
                        try
                        {
                            foreach (SPListItem compassItem in compassItemCol)
                            {
                                if (compassItem != null)
                                {
                                    ApprovalListItem item = new ApprovalListItem();
                                    item.CompassListItemId = Convert.ToInt32(compassItem[ApprovalListFields.CompassListItemId]);
                                    item.ProjectNumber = Convert.ToString(compassItem["Title"]);
                                    item.BOMSetupProc_SubmittedDate = Convert.ToString(compassItem[ApprovalListFields.BOMSetupProc_SubmittedDate]);
                                    item.BOMSetupProc_SubmittedBy = "System Account";
                                    activeProject.Add(item);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorSummary.AddError("Error in Approval Loop: " + ex.Message, this.Page);
                        }
                        SPList approvalList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalList2Name);
                        SPView CompasslistView = approvalList.Views["All Items"];
                        SPViewFieldCollection CompassviewfieldCollection = CompasslistView.ViewFields;
                        ErrorSummary.AddError("Looping Through Active Projects ", this.Page);
                        //Add items to ApprovalList
                        foreach (ApprovalListItem project in activeProject)
                        {

                            spQuery = new SPQuery();
                            spQuery.Query = "<Where><Eq><FieldRef Name=\"" + ApprovalListFields.CompassListItemId + "\" /><Value Type=\"Text\">" + project.CompassListItemId + "</Value></Eq></Where>";
                            spQuery.RowLimit = 1;
                            SPListItemCollection approvalItems = approvalList.GetItems(spQuery);
                            SPListItem item;
                            if (approvalItems.Count > 0)
                            {
                                item = approvalItems[0];
                            }
                            else
                            {
                                item = approvalList.AddItem();

                                item["Title"] = project.ProjectNumber;
                                item[ApprovalListFields.CompassListItemId] = project.CompassListItemId;

                                item.Update();
                            }

                            foreach (string viewFieldName in CompassviewfieldCollection)
                            {
                                try
                                {
                                    if (viewFieldName.Contains("SubmittedBy"))
                                    {
                                        SPField columnDetails = approvalList.Fields.GetField(viewFieldName);
                                        item[columnDetails.InternalName] = "System Account";
                                    }
                                    else if (viewFieldName.Contains("SubmittedDate"))
                                    {
                                        SPField columnDetails = approvalList.Fields.GetField(viewFieldName);
                                        item[columnDetails.InternalName] = project.BOMSetupProc_SubmittedDate;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ErrorSummary.AddError("Error in CompassviewfieldCollection Loop: " + ex.Message, this.Page);
                                    continue;
                                }
                            }
                            item.Update();

                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        #region Group Cleanup
        protected void btnOBMToPM_Click(object sender, EventArgs e)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                  {
                      using (var spSite = new SPSite(SPContext.Current.Web.Url))
                      {
                          using (var spWeb = spSite.OpenWeb())
                          {
                              spWeb.AllowUnsafeUpdates = true;
                              List<CompassListItem> compassProjects = new List<CompassListItem>();

                              // Get all  Projects
                              SPList compassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                              SPQuery spQuery = new SPQuery();

                              //Update PM from OBM
                              int UpdateCount = 0;
                              spWeb.AllowUnsafeUpdates = true;
                              SPList CompasslList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                              spQuery = new SPQuery();
                              spQuery.ViewFields = string.Concat(
                                             "<FieldRef Name='" + "ID" + "' />",
                                             "<FieldRef Name='" + "OBM" + "' />",
                                             "<FieldRef Name='" + "OBMName" + "' />",
                                             "<FieldRef Name='" + CompassListFields.PM + "' />",
                                             "<FieldRef Name='" + CompassListFields.PMName + "' />"
                                         );
                              spQuery.RowLimit = 1000;
                              spQuery.Query = "<Where>" +
                                                "<And>" +
                                                    "<IsNotNull><FieldRef Name='" + "OBMName" + "' /></IsNotNull>" +
                                                    "<IsNull><FieldRef Name='" + CompassListFields.PMName + "' /></IsNull>" +
                                                "</And>" +
                                              "</Where>";
                              SPListItemCollection compassItemCol = compassList.GetItems(spQuery);
                              foreach (SPListItem CompasslListItem in compassItemCol)
                              {
                                  if (CompasslListItem != null)
                                  {
                                      CompasslListItem[CompassListFields.PM] = CompasslListItem["OBM"];
                                      CompasslListItem[CompassListFields.PMName] = CompasslListItem["OBMName"];

                                      CompasslListItem.Update();
                                      UpdateCount++;
                                  }
                              }
                              spWeb.AllowUnsafeUpdates = false;

                              lblOBMToPMComplete.Visible = true;
                              lblOBMToPMComplete.Text = UpdateCount + " rows updated successfully!";
                          }
                      }
                  });
            }
            catch (Exception ex)
            {

                lblOBMToPMComplete.Visible = true;
                lblOBMToPMComplete.Text = ex.Message;
            }
        }
        protected void btnIntechStageGateMove_Click(object sender, EventArgs e)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                   {
                       using (var spSite = new SPSite(SPContext.Current.Web.Url))
                       {
                           using (var spWeb = spSite.OpenWeb())
                           {
                               spWeb.AllowUnsafeUpdates = true;
                               List<StageGateCreateProjectItem> compassProjects = new List<StageGateCreateProjectItem>();

                               // Get all  Projects
                               SPList compassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);

                               SPQuery spQuery = new SPQuery();

                               //Update InTech from R&D
                               int UpdateCount = 0;

                               SPList StageGatelList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);
                               spQuery = new SPQuery();
                               spQuery.ViewFields = string.Concat(
                                          "<FieldRef Name='" + "ID" + "' />",
                                          "<FieldRef Name='" + StageGateProjectListFields.InTech + "' />",
                                          "<FieldRef Name='" + StageGateProjectListFields.InTechName + "' />",
                                          "<FieldRef Name='" + "RnD" + "' />",
                                          "<FieldRef Name='" + "RnDName" + "' />"
                                         );
                               spQuery.RowLimit = 1000;
                               spQuery.Query = "<Where>" +
                                            "<And>" +
                                                "<IsNotNull><FieldRef Name='" + "RnDName" + "' /></IsNotNull>" +
                                                "<IsNull><FieldRef Name='" + StageGateProjectListFields.InTechName + "' /></IsNull>" +
                                            "</And>" +
                                          "</Where>";

                               SPListItemCollection CompasslListItems = StageGatelList.GetItems(spQuery);
                               foreach (SPListItem CompasslListItem in CompasslListItems)
                               {
                                   if (CompasslListItem != null)
                                   {
                                       CompasslListItem[StageGateProjectListFields.InTech] = CompasslListItem["RnD"];
                                       CompasslListItem[StageGateProjectListFields.InTechName] = CompasslListItem["RnDName"];

                                       CompasslListItem.Update();
                                       UpdateCount++;
                                   }
                               }
                               spWeb.AllowUnsafeUpdates = false;

                               lblIntechStageGateMoveMove.Visible = true;
                               lblIntechStageGateMoveMove.Text = UpdateCount + " rows updated successfully!";
                           }
                       }
                   });
            }
            catch (Exception ex)
            {

                lblIntechStageGateMoveMove.Visible = true;
                lblIntechStageGateMoveMove.Text = ex.Message;
            }
        }
        protected void btnInTechRegStageGateMove_Click(object sender, EventArgs e)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (var spSite = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (var spWeb = spSite.OpenWeb())
                        {
                            spWeb.AllowUnsafeUpdates = true;
                            List<StageGateCreateProjectItem> compassProjects = new List<StageGateCreateProjectItem>();

                            // Get all  Projects
                            SPList compassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);

                            //Update InTech Regulatory from R&D Regulatory
                            SPQuery spQuery = new SPQuery();
                            int UpdateCount = 0;
                            SPList StageGatelList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);
                            spQuery = new SPQuery();
                            spQuery.ViewFields = string.Concat(
                                       "<FieldRef Name='" + "ID" + "' />",
                                       "<FieldRef Name='" + StageGateProjectListFields.InTechRegulatory + "' />",
                                       "<FieldRef Name='" + StageGateProjectListFields.InTechRegulatoryName + "' />",
                                       "<FieldRef Name='" + "RegulatoryRnD" + "' />",
                                       "<FieldRef Name='" + "RegulatoryRnDName" + "' />"
                                      );
                            spQuery.RowLimit = 1000;
                            spQuery.Query = "<Where>" +
                                          "<And>" +
                                             "<IsNotNull><FieldRef Name='" + "RegulatoryRnDName" + "' /></IsNotNull>" +
                                             "<IsNull><FieldRef Name='" + StageGateProjectListFields.InTechRegulatoryName + "' /></IsNull>" +
                                         "</And>" +
                                       "</Where>";

                            SPListItemCollection CompasslListItems = StageGatelList.GetItems(spQuery);
                            foreach (SPListItem CompasslListItem in CompasslListItems)
                            {
                                if (CompasslListItem != null)
                                {
                                    CompasslListItem[StageGateProjectListFields.InTechRegulatory] = CompasslListItem["RegulatoryRnD"];
                                    CompasslListItem[StageGateProjectListFields.InTechRegulatoryName] = CompasslListItem["RegulatoryRnDName"];

                                    CompasslListItem.Update();
                                    UpdateCount++;
                                }
                            }
                            spWeb.AllowUnsafeUpdates = false;

                            lblInTechRegStageGateMove.Visible = true;
                            lblInTechRegStageGateMove.Text = UpdateCount + " rows updated successfully!";
                        }
                    }
                });
            }
            catch (Exception ex)
            {

                lblInTechRegStageGateMove.Visible = true;
                lblInTechRegStageGateMove.Text = ex.Message;
            }
        }
        protected void btnIntechCompassteameMove_Click(object sender, EventArgs e)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                   {
                       using (var spSite = new SPSite(SPContext.Current.Web.Url))
                       {
                           using (var spWeb = spSite.OpenWeb())
                           {
                               spWeb.AllowUnsafeUpdates = true;
                               List<ItemProposalItem> compassProjects = new List<ItemProposalItem>();

                               int UpdateCount = 0;
                               SPQuery spQuery = new SPQuery();
                               SPList CompassTeamList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTeamListName);
                               spQuery = new SPQuery();
                               spQuery.ViewFields = string.Concat(
                                          "<FieldRef Name='" + "ID" + "' />",
                                          "<FieldRef Name='" + CompassTeamListFields.InTechRegulatory + "' />",
                                          "<FieldRef Name='" + CompassTeamListFields.InTechRegulatoryName + "' />",
                                          "<FieldRef Name='" + "RegulatoryRnD" + "' />",
                                          "<FieldRef Name='" + "RegulatoryRnDName" + "' />"
                                          );
                               spQuery.Query = "<Where>" +
                                          "<And>" +
                                             "<IsNotNull><FieldRef Name='" + "RegulatoryRnDName" + "' /></IsNotNull>" +
                                             "<IsNull><FieldRef Name='" + CompassTeamListFields.InTechRegulatoryName + "' /></IsNull>" +
                                         "</And>" +
                                       "</Where>";
                               spQuery.RowLimit = 1000;

                               SPListItemCollection CompassTeamListItems = CompassTeamList.GetItems(spQuery);
                               foreach (SPListItem CompassTeamListItem in CompassTeamListItems)
                               {
                                   if (CompassTeamListItem != null)
                                   {
                                       CompassTeamListItem[CompassTeamListFields.InTechRegulatory] = CompassTeamListItem["RegulatoryRnD"];
                                       CompassTeamListItem[CompassTeamListFields.InTechRegulatoryName] = CompassTeamListItem["RegulatoryRnDName"];

                                       CompassTeamListItem.Update();
                                       UpdateCount++;
                                   }
                               }
                               spWeb.AllowUnsafeUpdates = false;

                               lblbtnIntechCompassteameMove.Visible = true;
                               lblbtnIntechCompassteameMove.Text = UpdateCount + " rows updated successfully!";
                           }
                       }
                   });
            }
            catch (Exception ex)
            {
                lblbtnIntechCompassteameMove.Visible = true;
                lblbtnIntechCompassteameMove.Text = ex.Message;
            }
        }
        #endregion

        private List<string> LoadProjectTeamMembers(string Members)
        {
            List<string> listTeamMembers = new List<string>();
            if (!string.IsNullOrEmpty(Members))
            {
                listTeamMembers = Members.Split(';').ToList();
            }
            List<string> ids = new List<string>();
            foreach (string member in listTeamMembers)
            {
                string userId = Regex.Replace(member, "[^0-9.]", "");
                int num;
                bool isNum = Int32.TryParse(userId, out num);
                if (isNum)
                {
                    ids.Add(num.ToString());
                }
            }
            return ids;
        }
        protected void btnDeleteLogsList_Click(object sender, EventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var list = spWeb.Lists.TryGetList(GlobalConstants.LIST_LogsListName);
                        if (list != null)
                        {
                            spWeb.Lists.Delete(list.ID);
                        }
                    }
                }
            });
        }
        protected void btnDeleteLogsPart2_Click(object sender, EventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList logsList = spWeb.Lists.TryGetList(GlobalConstants.LIST_LogsListName);
                        if (logsList == null)
                        {
                            ErrorSummary.AddError(string.Concat("Unable to find List: ", GlobalConstants.LIST_LogsListName), this.Page);
                            return;
                        }

                        // Get all the old Logs
                        SPQuery spQuery = new SPQuery();
                        spQuery.ViewAttributes = "Scope='Recursive'";
                        spQuery.ViewFields = "";
                        spQuery.ViewFieldsOnly = true;
                        spQuery.RowLimit = Convert.ToUInt32(txtRowLimit.Text);
                        SPListItemCollection compassItemCol = logsList.GetItems(spQuery);

                        totalRecords = compassItemCol.Count.ToString();

                        foreach (SPListItem logItem in compassItemCol)
                        {
                            try
                            {
                                logItem.Delete();
                            }
                            catch (Exception ex)
                            {
                                ErrorSummary.AddError(ex.Message, this.Page);
                            }
                        }

                        this.txtCurrentStatus.Text = string.Concat(totalRecords, " Records Deleted! ", DateTime.Now.ToString());
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        protected void btnDeleteLogs_Click(object sender, EventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList logsList = spWeb.Lists.TryGetList(GlobalConstants.LIST_LogsListName);
                        if (logsList == null)
                        {
                            ErrorSummary.AddError(string.Concat("Unable to find List: ", GlobalConstants.LIST_LogsListName), this.Page);
                            return;
                        }

                        // Get all the old Logs
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Leq><FieldRef Name=\"Created\" /><Value Type=\"DateTime\" IncludeTimeValue=\"FALSE\"><Today OffsetDays=\"-7\" /></Value></Leq></Where>";
                        spQuery.ViewAttributes = "Scope='Recursive'";
                        spQuery.ViewFields = "";
                        spQuery.ViewFieldsOnly = true;
                        spQuery.RowLimit = Convert.ToUInt32(txtRowLimit.Text);
                        SPListItemCollection compassItemCol = logsList.GetItems(spQuery);

                        totalRecords = compassItemCol.Count.ToString();

                        // Create a string to batch delete
                        StringBuilder sbDelete = new StringBuilder();
                        sbDelete.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                        sbDelete.Append("<Batch>");

                        string buildQuery = "<Method><SetList Scope=\"Request\">" + logsList.ID + "</SetList><SetVar Name=\"ID\">{0}</SetVar><SetVar Name=\"Cmd\">Delete</SetVar></Method>";

                        // Loop thru all the logs and add to the delete batch
                        foreach (SPListItem logItem in compassItemCol)
                        {
                            try
                            {
                                sbDelete.Append(string.Format(buildQuery, logItem.ID.ToString()));
                            }
                            catch (Exception ex)
                            {
                                ErrorSummary.AddError(ex.Message, this.Page);
                            }
                        }

                        sbDelete.Append("</Batch>");
                        spWeb.ProcessBatchData(sbDelete.ToString());

                        this.txtCurrentStatus.Text = string.Concat(totalRecords, " Records Deleted! ", DateTime.Now.ToString());
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        protected void btnDeleteAllLogs_Click(object sender, EventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList logsList = spWeb.Lists.TryGetList(GlobalConstants.LIST_LogsListName);
                        if (logsList == null)
                        {
                            ErrorSummary.AddError(string.Concat("Unable to find List: ", GlobalConstants.LIST_LogsListName), this.Page);
                            return;
                        }

                        // Create a string to batch delete
                        StringBuilder sbDelete = new StringBuilder();
                        sbDelete.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                        sbDelete.Append("<Batch>");

                        string buildQuery = "<Method><SetList Scope=\"Request\">" + logsList.ID + "</SetList><SetVar Name=\"ID\">{0}</SetVar><SetVar Name=\"Cmd\">Delete</SetVar></Method>";
                        int recordCount = 0;
                        // Loop thru all the logs and add to the delete batch
                        foreach (SPListItem logItem in logsList.Items)
                        {
                            recordCount++;
                            try
                            {
                                sbDelete.Append(string.Format(buildQuery, logItem.ID.ToString()));
                            }
                            catch (Exception ex)
                            {
                                ErrorSummary.AddError(ex.Message, this.Page);
                            }
                            if (recordCount > 999)
                                break;
                        }

                        sbDelete.Append("</Batch>");
                        spWeb.ProcessBatchData(sbDelete.ToString());

                        this.txtCurrentStatus.Text = string.Concat(recordCount.ToString(), " Records Deleted! ", DateTime.Now.ToString());
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        protected void btnMoveFieldsFromXMLActive_Click(object sender, EventArgs e)
        {
            List<XLSDets> xlsDets = new List<XLSDets>();
            using (StreamReader sr = new StreamReader(docUpload.FileContent))
            {
                string line;
                string[] columns = null;

                while ((line = sr.ReadLine()) != null)
                {
                    columns = line.Split(',');
                    string sapItemNumber = columns[0];
                    int sap;
                    bool success = Int32.TryParse(sapItemNumber, out sap);
                    if (success)
                    {
                        string PHL1 = columns[11].Trim() + " (" + columns[12].Trim() + ")";
                        string PHL2 = ""; try { PHL2 = columns[13].ToUpper().Trim() + " (" + columns[14].Trim().Substring(columns[14].Length - 9) + ")"; } catch (Exception ex) { PHL2 = "error"; }
                        string ProfitCenter = columns[19].Trim();
                        string packType = "";
                        try { packType = columns[17].Trim() + " (" + columns[18].Trim() + ")"; } catch (Exception ex) { packType = "error"; }
                        string brand = columns[15].Trim() + " (" + columns[16].Trim() + ")";
                        string projectdets = PHL1 + PHL2 + ProfitCenter + packType + brand;
                        XLSDets lineDets = new XLSDets();
                        lineDets.newBrand = brand;
                        lineDets.newPackType = packType;
                        lineDets.newPHL1 = PHL1;
                        lineDets.newPHL2 = PHL2;
                        lineDets.profitCenter = ProfitCenter;
                        lineDets.SAPNumber = sapItemNumber;
                        xlsDets.Add(lineDets);
                    }
                }
            }
            List<XLSDets> copiedDets = new List<XLSDets>();
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList compassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><And><Neq><FieldRef Name=\"" + CompassListFields.WorkflowPhase + "\" /><Value Type=\"Text\">Cancelled</Value></Neq><Neq><FieldRef Name=\"" + CompassListFields.WorkflowPhase + "\" /><Value Type=\"Text\">Completed</Value></Neq></And></Where>";
                        SPListItemCollection compassItemCol = compassList.GetItems(spQuery);

                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem item in compassItemCol)
                            {
                                if (item != null)
                                {
                                    string currentSAPNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                                    XLSDets query = (from detail in xlsDets where detail.SAPNumber == currentSAPNumber select detail).FirstOrDefault();
                                    if (query != null)
                                    {
                                        XLSDets oldDetails = new XLSDets();
                                        oldDetails.oldPHL1 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]);
                                        oldDetails.oldPHL2 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel2]);
                                        oldDetails.oldBrand = Convert.ToString(item[CompassListFields.MaterialGroup1Brand]);
                                        oldDetails.oldPackType = Convert.ToString(item[CompassListFields.MaterialGroup5PackType]);
                                        oldDetails.newPHL1 = query.newPHL1;
                                        oldDetails.newPHL2 = query.newPHL2;
                                        oldDetails.newBrand = query.newBrand;
                                        oldDetails.newPackType = query.newPackType;
                                        oldDetails.profitCenter = query.profitCenter;
                                        oldDetails.projectNumber = Convert.ToString(item[CompassListFields.ProjectNumber]);
                                        oldDetails.SAPNumber = query.SAPNumber;
                                        copiedDets.Add(oldDetails);
                                        item[CompassListFields.ProductHierarchyLevel1] = query.newPHL1;
                                        item[CompassListFields.ProductHierarchyLevel2] = query.newPHL2;
                                        item[CompassListFields.MaterialGroup1Brand] = query.newBrand;
                                        item[CompassListFields.MaterialGroup5PackType] = query.newPackType;
                                        item[CompassListFields.ProfitCenter] = query.profitCenter;
                                        item.Update();
                                    }
                                }
                            }
                        }
                    }
                }
            });
            var data = copiedDets.ToArray();

            string delimiter = ",";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join(delimiter, new string[] { "SAPNumber", "ProjectNumber", "oldPHL1", "oldPHL2", "oldBrand", "oldPackType", "newPHL1", "newPHL2", "newBrand", "newPackType", "profitCenter" }));

            foreach (XLSDets details in copiedDets)
            {
                sb.AppendLine(string.Join(delimiter, new string[] { details.SAPNumber, details.projectNumber, details.oldPHL1, details.oldPHL2, details.oldBrand, details.oldPackType, details.newPHL1, details.newPHL2, details.newBrand, details.newPackType, details.profitCenter }));
            }
            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(sb.ToString());
            List<FileAttribute> uploadFile = new List<FileAttribute>();
            FileAttribute file = new FileAttribute();
            file.FileName = "btnMoveFieldsFromXMLActive" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".csv";
            file.FileContent = byteArray;
            file.DocType = GlobalConstants.DOCTYPE_StageGateOthers;
            uploadFile.Add(file);
            utilityService.UploadCompassAttachment(uploadFile, "2019-9999-1");
            //File.WriteAllText(filePath, sb.ToString());

            //excelService.saveFileToDocLibrary(2268, "2019-9999-1", byteArray);
            LogEntry logEntry = new LogEntry();
            logEntry.Title = "btnMoveFieldsFromXMLActive_Click Completed";
            logEntry.Message = "btnMoveFieldsFromXMLActive_Click Completed";
            logEntry.Category = "General";
            logEntry.Form = "PMTAdmin";
            logEntry.Method = "btnMoveFieldsFromXMLActive_Click";
            logEntry.AdditionalInfo = "btnMoveFieldsFromXMLActive_Click Completed";
            exceptionService.InsertLog(logEntry);
        }
        protected void btnMoveFieldsFromXMLCancelled_Click(object sender, EventArgs e)
        {
            List<XLSDets> xlsDets = new List<XLSDets>();
            using (StreamReader sr = new StreamReader(FileUpload1.FileContent))
            {
                string line;
                string[] columns = null;

                while ((line = sr.ReadLine()) != null)
                {
                    columns = line.Split(',');
                    string sapItemNumber = columns[0];
                    int sap;
                    bool success = Int32.TryParse(sapItemNumber, out sap);
                    if (success)
                    {
                        string PHL1 = columns[11].Trim() + " (" + columns[12].Trim() + ")";
                        string PHL2 = ""; try { PHL2 = columns[13].ToUpper().Trim() + " (" + columns[14].Trim().Substring(columns[14].Length - 9) + ")"; } catch (Exception ex) { PHL2 = "error"; }
                        string ProfitCenter = columns[19].Trim();
                        string packType = "";
                        try { packType = columns[17].Trim() + " (" + columns[18].Trim() + ")"; } catch (Exception ex) { packType = "error"; }
                        string brand = columns[15].Trim() + " (" + columns[16].Trim() + ")";
                        string projectdets = PHL1 + PHL2 + ProfitCenter + packType + brand;
                        XLSDets lineDets = new XLSDets();
                        lineDets.newBrand = brand;
                        lineDets.newPackType = packType;
                        lineDets.newPHL1 = PHL1;
                        lineDets.newPHL2 = PHL2;
                        lineDets.profitCenter = ProfitCenter;
                        lineDets.SAPNumber = sapItemNumber;
                        xlsDets.Add(lineDets);
                    }
                }
            }
            List<XLSDets> copiedDets = new List<XLSDets>();
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList compassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Or><Eq><FieldRef Name=\"" + CompassListFields.WorkflowPhase + "\" /><Value Type=\"Text\">Cancelled</Value></Eq><Eq><FieldRef Name=\"" + CompassListFields.WorkflowPhase + "\" /><Value Type=\"Text\">Completed</Value></Eq></Or></Where>";
                        SPListItemCollection compassItemCol = compassList.GetItems(spQuery);

                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem item in compassItemCol)
                            {
                                if (item != null)
                                {
                                    string currentSAPNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                                    XLSDets query = (from detail in xlsDets where detail.SAPNumber == currentSAPNumber select detail).FirstOrDefault();
                                    if (query != null)
                                    {
                                        XLSDets oldDetails = new XLSDets();
                                        oldDetails.oldPHL1 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]);
                                        oldDetails.oldPHL2 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel2]);
                                        oldDetails.oldBrand = Convert.ToString(item[CompassListFields.MaterialGroup1Brand]);
                                        oldDetails.oldPackType = Convert.ToString(item[CompassListFields.MaterialGroup5PackType]);
                                        oldDetails.newPHL1 = query.newPHL1;
                                        oldDetails.newPHL2 = query.newPHL2;
                                        oldDetails.newBrand = query.newBrand;
                                        oldDetails.newPackType = query.newPackType;
                                        oldDetails.profitCenter = query.profitCenter;
                                        oldDetails.projectNumber = Convert.ToString(item[CompassListFields.ProjectNumber]);
                                        oldDetails.SAPNumber = query.SAPNumber;
                                        copiedDets.Add(oldDetails);
                                        item[CompassListFields.ProductHierarchyLevel1] = query.newPHL1;
                                        item[CompassListFields.ProductHierarchyLevel2] = query.newPHL2;
                                        item[CompassListFields.MaterialGroup1Brand] = query.newBrand;
                                        item[CompassListFields.MaterialGroup5PackType] = query.newPackType;
                                        item[CompassListFields.ProfitCenter] = query.profitCenter;
                                        item.Update();
                                    }
                                }
                            }
                        }
                    }
                }
            });
            var data = copiedDets.ToArray();

            string filePath = @"C:\Temp\updatedCancelledProjects.csv";
            string delimiter = ",";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join(delimiter, new string[] { "SAPNumber", "ProjectNumber", "oldPHL1", "oldPHL2", "oldBrand", "oldPackType", "newPHL1", "newPHL2", "newBrand", "newPackType", "profitCenter" }));

            foreach (XLSDets details in copiedDets)
            {
                sb.AppendLine(string.Join(delimiter, new string[] { details.SAPNumber, details.projectNumber, details.oldPHL1, details.oldPHL2, details.oldBrand, details.oldPackType, details.newPHL1, details.newPHL2, details.newBrand, details.newPackType, details.profitCenter }));
            }

            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(sb.ToString());
            List<FileAttribute> uploadFile = new List<FileAttribute>();
            FileAttribute file = new FileAttribute();
            file.FileName = "updatedCancelledProjects" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".csv";
            file.FileContent = byteArray;
            file.DocType = GlobalConstants.DOCTYPE_StageGateOthers;
            uploadFile.Add(file);
            utilityService.UploadCompassAttachment(uploadFile, "2019-9999-1");
            LogEntry logEntry = new LogEntry();
            logEntry.Title = "btnMoveFieldsFromXMLCancelled_Click Completed";
            logEntry.Message = "btnMoveFieldsFromXMLCancelled_Click Completed";
            logEntry.Category = "General";
            logEntry.Form = "PMTAdmin";
            logEntry.Method = "btnMoveFieldsFromXMLCancelled_Click";
            logEntry.AdditionalInfo = "btnMoveFieldsFromXMLCancelled_Click Completed";
            exceptionService.InsertLog(logEntry);
        }
        protected void btnMoveTradePromoFieldsFromXMLActive_Click(object sender, EventArgs e)
        {
            List<XLSDets> xlsDets = new List<XLSDets>();
            using (StreamReader sr = new StreamReader(TradePromoUpload.FileContent))
            {
                string line;
                string[] columns = null;

                while ((line = sr.ReadLine()) != null)
                {
                    columns = line.Split(',');
                    string sapItemNumber = columns[0];
                    int sap;
                    bool success = Int32.TryParse(sapItemNumber, out sap);
                    if (success)
                    {
                        string tradePromo = columns[7].Trim() + " (" + columns[8].Trim() + ")";
                        XLSDets lineDets = new XLSDets();
                        lineDets.newTradePromo = tradePromo;
                        lineDets.SAPNumber = sapItemNumber;
                        xlsDets.Add(lineDets);
                    }
                }
            }
            List<XLSDets> copiedDets = new List<XLSDets>();
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList compassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><And><Neq><FieldRef Name=\"" + CompassListFields.WorkflowPhase + "\" /><Value Type=\"Text\">Cancelled</Value></Neq><Neq><FieldRef Name=\"" + CompassListFields.WorkflowPhase + "\" /><Value Type=\"Text\">Completed</Value></Neq></And></Where>";
                        SPListItemCollection compassItemCol = compassList.GetItems(spQuery);

                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem item in compassItemCol)
                            {
                                if (item != null)
                                {
                                    string currentSAPNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                                    XLSDets query = (from detail in xlsDets where detail.SAPNumber == currentSAPNumber select detail).FirstOrDefault();
                                    if (query != null)
                                    {
                                        XLSDets oldDetails = new XLSDets();
                                        oldDetails.oldTradePromo = Convert.ToString(item[CompassListFields.MaterialGroup2Pricing]);
                                        oldDetails.newTradePromo = query.newTradePromo;
                                        oldDetails.projectNumber = Convert.ToString(item[CompassListFields.ProjectNumber]);
                                        oldDetails.SAPNumber = query.SAPNumber;
                                        copiedDets.Add(oldDetails);
                                        item[CompassListFields.MaterialGroup2Pricing] = query.newTradePromo;
                                        item.Update();
                                    }
                                }
                            }
                        }
                    }
                }
            });
            var data = copiedDets.ToArray();

            string filePath = @"C:\Temp\updatedTradePromoActiveProjects.csv";
            string delimiter = ",";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join(delimiter, new string[] { "SAPNumber", "ProjectNumber", "oldTradePromo", "newTradePromo" }));

            foreach (XLSDets details in copiedDets)
            {
                sb.AppendLine(string.Join(delimiter, new string[] { details.SAPNumber, details.projectNumber, details.oldTradePromo, details.newTradePromo }));
            }

            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(sb.ToString());
            List<FileAttribute> uploadFile = new List<FileAttribute>();
            FileAttribute file = new FileAttribute();
            file.FileName = "updatedTradePromoActiveProjects" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".csv";
            file.FileContent = byteArray;
            file.DocType = GlobalConstants.DOCTYPE_StageGateOthers;
            uploadFile.Add(file);
            utilityService.UploadCompassAttachment(uploadFile, "2019-9999-1");
            LogEntry logEntry = new LogEntry();
            logEntry.Title = "btnMoveTradePromoFieldsFromXMLActive_Click Completed";
            logEntry.Message = "btnMoveTradePromoFieldsFromXMLActive_Click Completed";
            logEntry.Category = "General";
            logEntry.Form = "PMTAdmin";
            logEntry.Method = "btnMoveTradePromoFieldsFromXMLActive_Click";
            logEntry.AdditionalInfo = "btnMoveTradePromoFieldsFromXMLActive_Click Completed";
            exceptionService.InsertLog(logEntry);
        }
        protected void btnMoveTradePromoFieldsFromXMLCancelled_Click(object sender, EventArgs e)
        {
            List<XLSDets> xlsDets = new List<XLSDets>();
            using (StreamReader sr = new StreamReader(TradePromoUploadCancelled.FileContent))
            {
                string line;
                string[] columns = null;

                while ((line = sr.ReadLine()) != null)
                {
                    columns = line.Split(',');
                    string sapItemNumber = columns[0];
                    int sap;
                    bool success = Int32.TryParse(sapItemNumber, out sap);
                    if (success)
                    {
                        string tradePromo = columns[7].Trim() + " (" + columns[8].Trim() + ")";
                        XLSDets lineDets = new XLSDets();
                        lineDets.newTradePromo = tradePromo;
                        lineDets.SAPNumber = sapItemNumber;
                        xlsDets.Add(lineDets);
                    }
                }
            }
            List<XLSDets> copiedDets = new List<XLSDets>();
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList compassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Or><Eq><FieldRef Name=\"" + CompassListFields.WorkflowPhase + "\" /><Value Type=\"Text\">Cancelled</Value></Eq><Eq><FieldRef Name=\"" + CompassListFields.WorkflowPhase + "\" /><Value Type=\"Text\">Completed</Value></Eq></Or></Where>";
                        SPListItemCollection compassItemCol = compassList.GetItems(spQuery);

                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem item in compassItemCol)
                            {
                                if (item != null)
                                {
                                    string currentSAPNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                                    XLSDets query = (from detail in xlsDets where detail.SAPNumber == currentSAPNumber select detail).FirstOrDefault();
                                    if (query != null)
                                    {
                                        XLSDets oldDetails = new XLSDets();
                                        oldDetails.oldTradePromo = Convert.ToString(item[CompassListFields.MaterialGroup2Pricing]);
                                        oldDetails.newTradePromo = query.newTradePromo;
                                        oldDetails.projectNumber = Convert.ToString(item[CompassListFields.ProjectNumber]);
                                        oldDetails.SAPNumber = query.SAPNumber;
                                        copiedDets.Add(oldDetails);
                                        item[CompassListFields.MaterialGroup2Pricing] = query.newTradePromo;
                                        item.Update();
                                    }
                                }
                            }
                        }
                    }
                }
            });
            var data = copiedDets.ToArray();

            string filePath = @"C:\Temp\updatedTradePromoCancelledProjects.csv";
            string delimiter = ",";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join(delimiter, new string[] { "SAPNumber", "ProjectNumber", "oldTradePromo", "newTradePromo" }));

            foreach (XLSDets details in copiedDets)
            {
                sb.AppendLine(string.Join(delimiter, new string[] { details.SAPNumber, details.projectNumber, details.oldTradePromo, details.newTradePromo }));
            }

            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(sb.ToString());
            List<FileAttribute> uploadFile = new List<FileAttribute>();
            FileAttribute file = new FileAttribute();
            file.FileName = "updatedTradePromoCancelledProjects" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".csv";
            file.FileContent = byteArray;
            file.DocType = GlobalConstants.DOCTYPE_StageGateOthers;
            uploadFile.Add(file);
            utilityService.UploadCompassAttachment(uploadFile, "2019-9999-1");
            LogEntry logEntry = new LogEntry();
            logEntry.Title = "updatedTradePromoCancelledProjects Completed";
            logEntry.Message = "updatedTradePromoCancelledProjects Completed";
            logEntry.Category = "General";
            logEntry.Form = "PMTAdmin";
            logEntry.Method = "updatedTradePromoCancelledProjects";
            logEntry.AdditionalInfo = "updatedTradePromoCancelledProjects Completed";
            exceptionService.InsertLog(logEntry);
        }
        protected void btnResetProcurementWorkflowData_Click(object sender, EventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList ApprovalList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);
                        SPList ApprovalList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalList2Name);
                        SPList WorkflowStatusList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassWorkflowStatusListName);

                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + projectNumbertoRest.Text + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;
                        SPListItemCollection ApprovalCol = ApprovalList.GetItems(spQuery);
                        SPListItemCollection Approval2Col = ApprovalList2.GetItems(spQuery);
                        SPListItemCollection WorkflowCol = WorkflowStatusList.GetItems(spQuery);

                        SPListItem item = ApprovalCol[0];
                        if (item != null)
                        {
                            item[ApprovalListFields.BOMSetupProc_StartDate] = "";
                            item[ApprovalListFields.BOMSetupProc_SubmittedBy] = "";
                            item[ApprovalListFields.BOMSetupProc_SubmittedDate] = "";
                            item.Update();
                        }

                        SPListItem item2 = WorkflowCol[0];
                        if (item2 != null)
                        {
                            SPView WorkflowStatusListView = WorkflowStatusList.Views["All Items"];
                            SPViewFieldCollection WorkflowStatusListViewfieldCollection = WorkflowStatusListView.ViewFields;
                            List<string> userIDs = new List<string>();
                            foreach (string viewFieldName in WorkflowStatusListViewfieldCollection)
                            {
                                try
                                {
                                    SPField columnDetails = WorkflowCol.Fields.GetField(viewFieldName);
                                    string name = columnDetails.InternalName;
                                    if (name.Contains("Proc"))
                                    {
                                        item2[name] = "No";

                                    }
                                }
                                catch (Exception ex)
                                {
                                    continue;
                                }
                            }
                            item2.Update();
                        }

                        SPListItem item3 = Approval2Col[0];
                        if (item3 != null)
                        {
                            SPView ApprovalList2View = ApprovalList2.Views["All Items"];
                            SPViewFieldCollection ApprovalList2ViewfieldCollection = ApprovalList2View.ViewFields;
                            List<string> userIDs = new List<string>();
                            foreach (string viewFieldName in ApprovalList2ViewfieldCollection)
                            {
                                try
                                {
                                    SPField columnDetails = Approval2Col.Fields.GetField(viewFieldName);
                                    string name = columnDetails.InternalName;
                                    if (name.Contains("Proc"))
                                    {
                                        item3[name] = "";

                                    }
                                }
                                catch (Exception ex)
                                {
                                    continue;
                                }
                            }
                            item3.Update();
                        }

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        protected void btnHolidayEasterTrasnfer_Click(object sender, EventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        List<int> CompassListIds = new List<int>();

                        // Get all  Projects
                        SPList compassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        SPQuery spQuery = new SPQuery();
                        spQuery.ViewFields = string.Concat(
                                   "<FieldRef Name='ID' />");
                        spQuery.Query = "<Where>" +
                                            "<And>" +
                                                "<And>" +
                                                    "<Neq><FieldRef Name=\"WorkflowPhase\" /><Value Type=\"Text\">Completed</Value></Neq>" +
                                                    "<Neq><FieldRef Name=\"WorkflowPhase\" /><Value Type=\"Text\">Cancelled</Value></Neq>" +
                                                 "</And>" +
                                                 "<Or>" +
                                                    "<Eq><FieldRef Name=\"" + CompassListFields.ProductHierarchyLevel2 + "\" /><Value Type=\"Text\">" + "CHRISTMAS (000000001)" + "</Value></Eq>" +
                                                        "<Or>" +
                                                            "<Eq><FieldRef Name=\"" + CompassListFields.ProductHierarchyLevel2 + "\" /><Value Type=\"Text\">" + "CHRISTMAS BULK (000000002)" + "</Value></Eq>" +
                                                            "<Or>" +
                                                                "<Eq><FieldRef Name=\"" + CompassListFields.ProductHierarchyLevel2 + "\" /><Value Type=\"Text\">" + "EASTER (000000003)" + "</Value></Eq>" +
                                                                "<Eq><FieldRef Name=\"" + CompassListFields.ProductHierarchyLevel2 + "\" /><Value Type=\"Text\">" + "EASTER BULK (000000004)" + "</Value></Eq>" +
                                                            "</Or>" +
                                                        "</Or>" +
                                                "</Or>" +
                                            "</And>" +
                                        "</Where>";
                        spQuery.ViewFieldsOnly = true;
                        SPListItemCollection compassItemCol = compassList.GetItems(spQuery);

                        foreach (SPListItem compassItem in compassItemCol)
                        {
                            if (compassItem != null)
                            {
                                CompassListIds.Add(compassItem.ID);
                            }
                        }

                        var user = "Zilleox, Gina Nicole";
                        SPFieldUserValueCollection values = new SPFieldUserValueCollection();
                        SPUser spUser = spWeb.EnsureUser(user);
                        // spUser = spWeb.SiteUsers[user];
                        var userName = new SPFieldUserValue(spWeb, spUser.ID, spUser.LoginName);
                        values.Add(userName);

                        //Update Compass Team list
                        int UpdateCount = 0;
                        foreach (var CompassListId in CompassListIds)
                        {
                            SPList CompassTeamList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTeamListName);
                            spQuery = new SPQuery();
                            spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassTeamListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + CompassListId + "</Value></Eq></Where>";
                            spQuery.RowLimit = 1;
                            SPListItemCollection compassTeamListItems = CompassTeamList.GetItems(spQuery);
                            if (compassTeamListItems.Count > 0)
                            {
                                var compassTeamListItem = compassTeamListItems[0];
                                compassTeamListItem[CompassTeamListFields.SeniorProjectManager] = values;
                                compassTeamListItem[CompassTeamListFields.SeniorProjectManagerName] = spUser.Name;

                                compassTeamListItem.Update();

                                UpdateCount++;
                            }
                            else
                            {
                                var compassTeamListItem = CompassTeamList.AddItem();

                                compassTeamListItem[CompassList2Fields.CompassListItemId] = CompassListId;
                                compassTeamListItem[CompassTeamListFields.SeniorProjectManager] = values;
                                compassTeamListItem[CompassTeamListFields.SeniorProjectManagerName] = spUser.Name;

                                compassTeamListItem.Update();
                                UpdateCount++;
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;

                        lblHolidayEasterTrasnfer.Visible = true;
                        lblHolidayEasterTrasnfer.Text = "Zilleox, Gina Nicole has been updated as Sr. PM successfully on " + UpdateCount + " projects";
                    }
                }
            });
        }

        protected void btnHalloweenVday_Click(object sender, EventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        List<int> CompassListIds = new List<int>();

                        // Get all  Projects
                        SPList compassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        SPQuery spQuery = new SPQuery();
                        spQuery.ViewFields = string.Concat(
                                   "<FieldRef Name='ID' />");
                        spQuery.Query = "<Where>" +
                                            "<And>" +
                                                "<And>" +
                                                     "<Neq><FieldRef Name=\"WorkflowPhase\" /><Value Type=\"Text\">Completed</Value></Neq>" +
                                                     "<Neq><FieldRef Name=\"WorkflowPhase\" /><Value Type=\"Text\">Cancelled</Value></Neq>" +
                                                "</And>" +
                                                "<Or>" +
                                                    "<Eq><FieldRef Name=\"" + CompassListFields.ProductHierarchyLevel2 + "\" /><Value Type=\"Text\">" + "HALLOWEEN (000000005)" + "</Value></Eq>" +
                                                        "<Or>" +
                                                            "<Eq><FieldRef Name=\"" + CompassListFields.ProductHierarchyLevel2 + "\" /><Value Type=\"Text\">" + "HALLOWEEN BULK (000000006)" + "</Value></Eq>" +
                                                            "<Or>" +
                                                                "<Eq><FieldRef Name=\"" + CompassListFields.ProductHierarchyLevel2 + "\" /><Value Type=\"Text\">" + "VALENTINE'S (000000008)" + "</Value></Eq>" +
                                                                "<Eq><FieldRef Name=\"" + CompassListFields.ProductHierarchyLevel2 + "\" /><Value Type=\"Text\">" + "VALENTINE'S BULK (000000009)" + "</Value></Eq>" +
                                                            "</Or>" +
                                                        "</Or>" +
                                                "</Or>" +
                                            "</And>" +
                                        "</Where>";
                        spQuery.ViewFieldsOnly = true;
                        SPListItemCollection compassItemCol = compassList.GetItems(spQuery);

                        foreach (SPListItem compassItem in compassItemCol)
                        {
                            if (compassItem != null)
                            {
                                CompassListIds.Add(compassItem.ID);
                            }
                        }

                        var user = "Kielma, Phil";
                        SPFieldUserValueCollection values = new SPFieldUserValueCollection();
                        SPUser spUser = spWeb.EnsureUser(user);
                        // spUser = spWeb.SiteUsers[user];
                        var userName = new SPFieldUserValue(spWeb, spUser.ID, spUser.LoginName);
                        values.Add(userName);

                        //Update Compass Team list
                        int UpdateCount = 0;
                        foreach (var CompassListId in CompassListIds)
                        {
                            SPList CompassTeamList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTeamListName);
                            spQuery = new SPQuery();
                            spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassTeamListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + CompassListId + "</Value></Eq></Where>";
                            spQuery.RowLimit = 1;
                            SPListItemCollection compassTeamListItems = CompassTeamList.GetItems(spQuery);
                            if (compassTeamListItems.Count > 0)
                            {
                                var compassTeamListItem = compassTeamListItems[0];
                                compassTeamListItem[CompassTeamListFields.SeniorProjectManager] = values;
                                compassTeamListItem[CompassTeamListFields.SeniorProjectManagerName] = spUser.Name;

                                compassTeamListItem.Update();

                                UpdateCount++;
                            }
                            else
                            {

                                var compassTeamListItem = CompassTeamList.AddItem();

                                compassTeamListItem[CompassList2Fields.CompassListItemId] = CompassListId;
                                compassTeamListItem[CompassTeamListFields.SeniorProjectManager] = values;
                                compassTeamListItem[CompassTeamListFields.SeniorProjectManagerName] = spUser.Name;

                                compassTeamListItem.Update();
                                UpdateCount++;
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;

                        lblHalloweenVday.Visible = true;
                        lblHalloweenVday.Text = "Kielma, Phil has been updated as Sr. PM successfully on " + UpdateCount + " projects";
                    }
                }
            });
        }
        protected void btnBadUpdatesReport_Click(object sender, EventArgs e)
        {
            List<XLSDets> copiedDets = new List<XLSDets>();
            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    SPList PHL1List = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProductHierarchyLevel1Lookup);
                    SPList PHL2List = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProductHierarchyLevel2Lookup);
                    SPList BrandList = spWeb.Lists.TryGetList(GlobalConstants.LIST_MaterialGroup1Lookup);
                    SPList PackTypeList = spWeb.Lists.TryGetList(GlobalConstants.LIST_MaterialGroup5Lookup);
                    SPList TradePromoList = spWeb.Lists.TryGetList(GlobalConstants.LIST_MaterialGroup2Lookup);
                    SPList compassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                    SPQuery lookupQuery = new SPQuery();
                    lookupQuery.Query = "<Where><IsNotNull><FieldRef Name=\"Title\" /></IsNotNull></Where>";

                    SPListItemCollection PHL1Col = PHL1List.GetItems(lookupQuery);
                    SPListItemCollection PHL2Col = PHL2List.GetItems(lookupQuery);
                    SPListItemCollection BrandCol = BrandList.GetItems(lookupQuery);
                    SPListItemCollection PackTypeCol = PackTypeList.GetItems(lookupQuery);
                    SPListItemCollection TradePromoCol = TradePromoList.GetItems(lookupQuery);

                    List<string> PHL1s = (from SPListItem cols in PHL1Col select cols.Title).ToList();
                    List<string> PHL2s = (from SPListItem cols in PHL2Col select cols.Title).ToList();
                    List<string> Brands = (from SPListItem cols in BrandCol select cols.Title).ToList();
                    List<string> PackTypes = (from SPListItem cols in PackTypeCol select cols.Title).ToList();
                    List<string> TradePromos = (from SPListItem cols in TradePromoCol select cols.Title).ToList();
                    List<string> ProfitCenters = (from SPListItem cols in BrandCol select Convert.ToString(cols["ProfitCenter"])).ToList();

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Neq><FieldRef Name=\"" + CompassListFields.WorkflowPhase + "\" /><Value Type=\"Text\">Cancelled</Value></Neq><Neq><FieldRef Name=\"" + CompassListFields.WorkflowPhase + "\" /><Value Type=\"Text\">Completed</Value></Neq></And></Where>";
                    SPListItemCollection compassItemCol = compassList.GetItems(spQuery);

                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            if (item != null)
                            {
                                XLSDets oldDetails = new XLSDets();
                                string SetPHL1 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]);
                                string SetPHL2 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel2]);
                                string SetBrand = Convert.ToString(item[CompassListFields.MaterialGroup1Brand]);
                                string SetPackType = Convert.ToString(item[CompassListFields.MaterialGroup5PackType]);
                                string SetTradePromo = Convert.ToString(item[CompassListFields.MaterialGroup2Pricing]);
                                string SetProfitCeneter = Convert.ToString(item[CompassListFields.ProfitCenter]);
                                SPListItem brandRow = (from SPListItem brandListCol in BrandCol where Convert.ToString(brandListCol["ParentPHL2"]) == SetPHL2 && Convert.ToString(brandListCol.Title) == SetBrand select brandListCol).FirstOrDefault();
                                oldDetails.oldPHL1 = (from phl1 in PHL1s where phl1 == SetPHL1 select phl1).Count() > 0 || SetPHL1 == "Select..." ? "" : SetPHL1;
                                oldDetails.oldPHL2 = (from phl2 in PHL2s where phl2 == SetPHL2 select phl2).Count() > 0 || SetPHL2 == "Select..." ? "" : SetPHL2;
                                oldDetails.oldBrand = (from brand in Brands where brand == SetBrand select brand).Count() > 0 || SetBrand == "Select..." ? "" : SetBrand;
                                oldDetails.oldPackType = (from packType in PackTypes where packType == SetPackType select packType).Count() > 0 || SetPackType == "Select..." ? "" : SetPackType;
                                oldDetails.oldTradePromo = (from tradePromo in TradePromos where tradePromo == SetTradePromo select tradePromo).Count() > 0 || SetTradePromo == "Select..." ? "" : SetTradePromo;
                                if (brandRow != null)
                                {
                                    oldDetails.oldProfitCenter = Convert.ToString(brandRow["ProfitCenter"]) == SetProfitCeneter ? "" : SetProfitCeneter;
                                }
                                oldDetails.projectNumber = Convert.ToString(item[CompassListFields.ProjectNumber]);
                                oldDetails.PM = Convert.ToString(item[CompassListFields.PMName]).Replace(",", " ");
                                oldDetails.SAPNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                                if (string.IsNullOrEmpty(oldDetails.oldPHL1) && string.IsNullOrEmpty(oldDetails.oldPHL2) && string.IsNullOrEmpty(oldDetails.oldBrand) && string.IsNullOrEmpty(oldDetails.oldPackType) && string.IsNullOrEmpty(oldDetails.oldTradePromo) && string.IsNullOrEmpty(oldDetails.oldProfitCenter)) { }
                                else { copiedDets.Add(oldDetails); }
                            }
                        }
                    }
                }
            }
            var data = copiedDets.ToArray();

            string delimiter = ",";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join(delimiter, new string[] { "SAPNumber", "ProjectNumber", "BrokenPHL1", "BrokenPHL2", "BrokenBrand", "BrokenPackType", "BrokenTradePromo", "BrokenProfitCenter", "PM" }));

            foreach (XLSDets details in copiedDets)
            {
                sb.AppendLine(string.Join(delimiter, new string[] { details.SAPNumber, details.projectNumber, details.oldPHL1, details.oldPHL2, details.oldBrand, details.oldPackType, details.oldTradePromo, details.oldProfitCenter, details.PM }));
            }
            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(sb.ToString());
            List<FileAttribute> uploadFile = new List<FileAttribute>();
            FileAttribute file = new FileAttribute();
            file.FileName = "badUpdates" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".csv";
            file.FileContent = byteArray;
            file.DocType = GlobalConstants.DOCTYPE_StageGateOthers;
            uploadFile.Add(file);
            utilityService.UploadCompassAttachment(uploadFile, "2019-9999-1");
            //File.WriteAllText(filePath, sb.ToString());

            //excelService.saveFileToDocLibrary(2268, "2019-9999-1", byteArray);
            LogEntry logEntry = new LogEntry();
            logEntry.Title = "bad Updates report Completed";
            logEntry.Message = "bad Updates report Completed";
            logEntry.Category = "General";
            logEntry.Form = "PMTAdmin";
            logEntry.Method = "bad Updates report";
            logEntry.AdditionalInfo = "bad Updates report Completed";
            exceptionService.InsertLog(logEntry);
        }
        protected void btnMovePHL1_click(object sender, EventArgs e)
        {
            moveColumn(CompassListFields.ProductHierarchyLevel1);
        }
        protected void btnMovePHL2_click(object sender, EventArgs e)
        {
            moveColumn(CompassListFields.ProductHierarchyLevel2);
        }
        protected void btnMoveBrand_click(object sender, EventArgs e)
        {
            moveColumn(CompassListFields.MaterialGroup1Brand);
        }
        protected void btnMovePackType_click(object sender, EventArgs e)
        {
            moveColumn(CompassListFields.MaterialGroup5PackType);
        }
        protected void btnMoveTradePromo_click(object sender, EventArgs e)
        {
            moveColumn(CompassListFields.MaterialGroup2Pricing);
        }
        private void moveColumn(string columnName)
        {
            List<XLSDets> xlsDets = new List<XLSDets>();
            using (StreamReader sr = new StreamReader(docUploadSingleColumn.FileContent))
            {
                string line;
                string[] columns = null;

                while ((line = sr.ReadLine()) != null)
                {
                    columns = line.Split(',');
                    string sapItemNumber = columns[0];
                    int sap;
                    bool success = Int32.TryParse(sapItemNumber, out sap);
                    if (success)
                    {
                        string singleColumn = columns[1].Trim();
                        XLSDets lineDets = new XLSDets();
                        lineDets.newSingleColumn = singleColumn;
                        lineDets.SAPNumber = sapItemNumber;
                        xlsDets.Add(lineDets);
                    }
                }
            }
            List<XLSDets> copiedDets = new List<XLSDets>();
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList compassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><And><Neq><FieldRef Name=\"" + CompassListFields.WorkflowPhase + "\" /><Value Type=\"Text\">Cancelled</Value></Neq><Neq><FieldRef Name=\"" + CompassListFields.WorkflowPhase + "\" /><Value Type=\"Text\">Completed</Value></Neq></And></Where>";
                        SPListItemCollection compassItemCol = compassList.GetItems(spQuery);

                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem item in compassItemCol)
                            {
                                if (item != null)
                                {
                                    string currentSAPNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                                    XLSDets query = (from detail in xlsDets where detail.SAPNumber == currentSAPNumber select detail).FirstOrDefault();
                                    if (query != null)
                                    {
                                        XLSDets oldDetails = new XLSDets();
                                        oldDetails.oldSingleColumn = Convert.ToString(item[columnName]);
                                        oldDetails.newSingleColumn = query.newSingleColumn;
                                        oldDetails.projectNumber = Convert.ToString(item[CompassListFields.ProjectNumber]);
                                        oldDetails.SAPNumber = query.SAPNumber;
                                        copiedDets.Add(oldDetails);
                                        item[columnName] = query.newSingleColumn;
                                        item.Update();
                                    }
                                }
                            }
                        }
                    }
                }
            });
            var data = copiedDets.ToArray();

            string delimiter = ",";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join(delimiter, new string[] { "SAPNumber", "ProjectNumber", "Old " + columnName, "New " + columnName }));

            foreach (XLSDets details in copiedDets)
            {
                sb.AppendLine(string.Join(delimiter, new string[] { details.SAPNumber, details.projectNumber, details.oldSingleColumn, details.newSingleColumn }));
            }
            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(sb.ToString());
            List<FileAttribute> uploadFile = new List<FileAttribute>();
            FileAttribute file = new FileAttribute();
            file.FileName = "btnMoveFieldsFromXMLActiveSingleColumn" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".csv";
            file.FileContent = byteArray;
            file.DocType = GlobalConstants.DOCTYPE_StageGateOthers;
            uploadFile.Add(file);
            utilityService.UploadCompassAttachment(uploadFile, "2019-9999-1");
            //File.WriteAllText(filePath, sb.ToString());

            //excelService.saveFileToDocLibrary(2268, "2019-9999-1", byteArray);
            LogEntry logEntry = new LogEntry();
            logEntry.Title = columnName + " has been updated";
            logEntry.Message = columnName + " has been updated";
            logEntry.Category = "General";
            logEntry.Form = "PMTAdmin";
            logEntry.Method = columnName + " has been updated";
            logEntry.AdditionalInfo = columnName + " has been updated";
            exceptionService.InsertLog(logEntry);
        }
        protected void btnCheckProfitCenter_Click(object sender, EventArgs e)
        {
            List<XLSDets> xlsDets = new List<XLSDets>();

            List<XLSDets> copiedDets = new List<XLSDets>();
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList compassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPList BrandList = spWeb.Lists.TryGetList(GlobalConstants.LIST_MaterialGroup1Lookup);

                        SPQuery lookupQuery = new SPQuery();
                        lookupQuery.Query = "<Where><IsNotNull><FieldRef Name=\"Title\" /></IsNotNull></Where>";

                        SPListItemCollection BrandCol = BrandList.GetItems(lookupQuery);

                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><And><Neq><FieldRef Name=\"" + CompassListFields.WorkflowPhase + "\" /><Value Type=\"Text\">Cancelled</Value></Neq><Neq><FieldRef Name=\"" + CompassListFields.WorkflowPhase + "\" /><Value Type=\"Text\">Completed</Value></Neq></And></Where>";
                        SPListItemCollection compassItemCol = compassList.GetItems(spQuery);

                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem item in compassItemCol)
                            {
                                if (item != null)
                                {
                                    string SetPHL1 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]);
                                    string SetPHL2 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel2]);
                                    string SetBrand = Convert.ToString(item[CompassListFields.MaterialGroup1Brand]);
                                    string SetProfitCeneter = Convert.ToString(item[CompassListFields.ProfitCenter]);
                                    string currentSAPNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                                    SPListItem brandRow = (from SPListItem brandListCol in BrandCol where Convert.ToString(brandListCol["ParentPHL2"]) == SetPHL2 && brandListCol.Title == SetBrand select brandListCol).FirstOrDefault();


                                    if (SetProfitCeneter != "" && brandRow != null)
                                    {

                                        if (Convert.ToString(brandRow["ProfitCenter"]) != SetProfitCeneter)
                                        {
                                            XLSDets oldDetails = new XLSDets();
                                            oldDetails.oldProfitCenter = SetProfitCeneter;
                                            oldDetails.newProfitCenter = Convert.ToString(brandRow["ProfitCenter"]);
                                            oldDetails.projectNumber = Convert.ToString(item[CompassListFields.ProjectNumber]);
                                            copiedDets.Add(oldDetails);

                                            item[CompassListFields.ProfitCenter] = Convert.ToString(brandRow["ProfitCenter"]);
                                            item.Update();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            });
            var data = copiedDets.ToArray();

            string delimiter = ",";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join(delimiter, new string[] { "OldProfitCenter", "NewProfitCenter", "ProjectNumber" }));

            foreach (XLSDets details in copiedDets)
            {
                sb.AppendLine(string.Join(delimiter, new string[] { details.oldProfitCenter, details.newProfitCenter, details.projectNumber }));
            }

            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(sb.ToString());
            List<FileAttribute> uploadFile = new List<FileAttribute>();
            FileAttribute file = new FileAttribute();
            file.FileName = "btnCheckProfitCenter_Click" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".csv";
            file.FileContent = byteArray;
            file.DocType = GlobalConstants.DOCTYPE_StageGateOthers;
            uploadFile.Add(file);
            utilityService.UploadCompassAttachment(uploadFile, "2019-9999-1");
            LogEntry logEntry = new LogEntry();
            logEntry.Title = "btnCheckProfitCenter_Click Completed";
            logEntry.Message = "btnCheckProfitCenter_Click Completed";
            logEntry.Category = "General";
            logEntry.Form = "PMTAdmin";
            logEntry.Method = "btnCheckProfitCenter_Click";
            logEntry.AdditionalInfo = "btnCheckProfitCenter_Click Completed";
            exceptionService.InsertLog(logEntry);
        }

        protected void btnUpdateCancelledDate_click(object sender, EventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        int count = 500;
                        try
                        {
                            count = Convert.ToInt32(configurationManagementService.GetConfiguration("BatchSize"));
                        }
                        catch (Exception)
                        {
                        }
                        SPList compassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where>" +
                                            "<And>" +
                                                "<Eq><FieldRef Name=\"" + CompassListFields.WorkflowPhase + "\" /><Value Type=\"Text\">Cancelled</Value></Eq>" +
                                                "<Neq><FieldRef Name=\"" + CompassListFields.PMTWorkflowUpdateStatus + "\" /><Value Type=\"Text\">CancelledDateUpdated</Value></Neq>" +
                                            "</And>" +
                                        "</Where>";
                        spQuery.RowLimit = Convert.ToUInt32(count);
                        SPListItemCollection compassItemCol = compassList.GetItems(spQuery);

                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem item in compassItemCol)
                            {
                                if (item != null)
                                {
                                    // SPListItem item1 = compassList.GetItemById(492);
                                    string CancelledDate = "";
                                    string CancelledBy = "";
                                    int CompassListItemId = item.ID;
                                    SPListItemVersionCollection versionCol = item.Versions;

                                    SPListItemVersion CancelledVersion = (
                                          from
                                           SPListItemVersion version in versionCol
                                          where
                                            Convert.ToString(version[CompassListFields.WorkflowPhase]) == "Cancelled"
                                          orderby
                                            version.Created ascending
                                          select
                                            version
                                        ).FirstOrDefault();

                                    CancelledDate = Convert.ToString(CancelledVersion.Created);
                                    CancelledBy = Convert.ToString(CancelledVersion.CreatedBy);
                                    CancelledBy = Convert.ToString(CancelledVersion["Editor"]);

                                    //update approval item
                                    //ApprovalList
                                    SPList spApprovalList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);
                                    SPQuery spApprovalListQuery = new SPQuery();
                                    spApprovalListQuery.Query = "<Where><Eq><FieldRef Name=\"" + ApprovalListFields.CompassListItemId + "\" /><Value Type=\"Integer\">" + CompassListItemId + "</Value></Eq></Where>";

                                    SPListItemCollection ApprovalListItemCol = spApprovalList.GetItems(spApprovalListQuery);
                                    if (ApprovalListItemCol.Count > 0)
                                    {
                                        foreach (SPListItem appItem in ApprovalListItemCol)
                                        {
                                            if (appItem != null)
                                            {
                                                if (string.IsNullOrEmpty(Convert.ToString(appItem[ApprovalListFields.Cancelled_ModifiedDate])))
                                                {
                                                    appItem[ApprovalListFields.Cancelled_ModifiedDate] = CancelledDate;
                                                    appItem[ApprovalListFields.Cancelled_ModifiedBy] = CancelledBy;
                                                    appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                                    appItem.Update();
                                                }
                                            }
                                        }
                                    }

                                    //Update PMT Workflow Update Column
                                    SPList spCompassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                                    SPQuery spCompassListQuery = new SPQuery();
                                    SPListItem compassItem = spCompassList.GetItemById(CompassListItemId);
                                    if (compassItem != null)
                                    {
                                        compassItem[CompassListFields.PMTWorkflowUpdateStatus] = "CancelledDateUpdated";
                                        compassItem.Update();
                                    }
                                }
                            }
                            lblUpdateCancelledDate.Visible = true;
                            lblUpdateCancelledDate.Text = "Cancelled Date for " + count.ToString() + " projects updated successfully. " + DateTime.Now.ToString("MM-dd-yyyy hh.mm.ss tt");
                        }
                    }
                }
            });

        }
        protected void btnUpdatePreProductionDate_click(object sender, EventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        int count = 500;
                        try
                        {
                            count = Convert.ToInt32(configurationManagementService.GetConfiguration("BatchSize"));
                        }
                        catch (Exception)
                        {
                        }

                        SPList compassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where>" +
                                           "<And>" +
                                                "<Eq><FieldRef Name=\"" + CompassListFields.WorkflowPhase + "\" /><Value Type=\"Text\">Pre-Production</Value></Eq>" +
                                                "<Neq><FieldRef Name=\"" + CompassListFields.PMTWorkflowUpdateStatus + "\" /><Value Type=\"Text\">PreProductionDateUpdated</Value></Neq>" +
                                           "</And>" +
                                        "</Where>";
                        spQuery.RowLimit = Convert.ToUInt32(count);
                        SPListItemCollection compassItemCol = compassList.GetItems(spQuery);

                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem item in compassItemCol)
                            {
                                if (item != null)
                                {
                                    //SPListItem item1 = compassList.GetItemById(492);
                                    string PreProductionDate = "";
                                    string PreProductionBy = "";
                                    int CompassListItemId = item.ID;
                                    SPListItemVersionCollection versionCol = item.Versions;

                                    SPListItemVersion PreProductionVersion = (
                                          from
                                           SPListItemVersion version in versionCol
                                          where
                                            Convert.ToString(version[CompassListFields.WorkflowPhase]) == GlobalConstants.WORKFLOWPHASE_PreProduction
                                          orderby
                                            version.Created ascending
                                          select
                                            version
                                        ).FirstOrDefault();

                                    PreProductionDate = Convert.ToString(PreProductionVersion.Created);
                                    PreProductionBy = Convert.ToString(PreProductionVersion.CreatedBy);
                                    PreProductionBy = Convert.ToString(PreProductionVersion["Editor"]);

                                    //update approval item
                                    //ApprovalList
                                    SPList spApprovalList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);
                                    SPQuery spApprovalListQuery = new SPQuery();
                                    spApprovalListQuery.Query = "<Where><Eq><FieldRef Name=\"" + ApprovalListFields.CompassListItemId + "\" /><Value Type=\"Integer\">" + CompassListItemId + "</Value></Eq></Where>";

                                    SPListItemCollection ApprovalListItemCol = spApprovalList.GetItems(spApprovalListQuery);
                                    if (ApprovalListItemCol.Count > 0)
                                    {
                                        foreach (SPListItem appItem in ApprovalListItemCol)
                                        {
                                            if (appItem != null)
                                            {
                                                if (string.IsNullOrEmpty(Convert.ToString(appItem[ApprovalListFields.PreProduction_ModifiedDate])))
                                                {
                                                    appItem[ApprovalListFields.PreProduction_ModifiedDate] = PreProductionDate;
                                                    appItem[ApprovalListFields.PreProduction_ModifiedBy] = PreProductionBy
                                                    ;
                                                    appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                                    appItem.Update();
                                                }
                                            }
                                        }
                                    }

                                    //Update PMT Workflow Update Column
                                    SPList spCompassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                                    SPQuery spCompassListQuery = new SPQuery();
                                    SPListItem compassItem = spCompassList.GetItemById(CompassListItemId);
                                    if (compassItem != null)
                                    {
                                        compassItem[CompassListFields.PMTWorkflowUpdateStatus] = "PreProductionDateUpdated";
                                        compassItem.Update();
                                    }
                                }
                            }
                            lblUpdatePreProductionDate.Visible = true;
                            lblUpdatePreProductionDate.Text = "Pre-Production Date for " + count.ToString() + " projects updated successfully. " + DateTime.Now.ToString("MM-dd-yyyy hh.mm.ss tt");
                        }
                    }
                }
            });
        }

        protected void btnPopulateList_Click(object sender, EventArgs e)
        {
            SPListItem newItem;
            SPList spList;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectDecisionsListName);
                        var compassItem = spList.GetItemById(138);
                        for (int i = 0; i < 200; i++)
                        {
                            newItem = spList.AddItem();
                            foreach (SPField field in compassItem.Fields)
                            {
                                if (!field.ReadOnlyField && compassItem[field.InternalName] != null && !field.InternalName.Equals("Attachments"))
                                {
                                    newItem[field.InternalName] = compassItem[field.InternalName];
                                }
                            }
                            //if (SPContext.Current.Item != null)
                            //{
                            //    //newItem[CompassProjectDecisionsListFields.for] = SPContext.Current.Item["Title"];
                            //}
                            newItem.Update();
                        }

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
    }
}
