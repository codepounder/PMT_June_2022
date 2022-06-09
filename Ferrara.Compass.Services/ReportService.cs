using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
//using Microsoft.Office.Server.SetupUtilities;

namespace Ferrara.Compass.Services
{
    public class ReportService : IReportService
    {
        private IUserManagementService userService;
        private readonly ICacheManagementService cacheService;
        private IPackagingItemService packagingItemService;
        private IVersionHistoryService versionHistoryService;
        private DataTable items;

        public ReportService(IUserManagementService userMgmtService, ICacheManagementService cacheManagementService, IPackagingItemService packItemService, IVersionHistoryService vhService)
        {
            userService = userMgmtService;
            cacheService = cacheManagementService;
            packagingItemService = packItemService;
            versionHistoryService = vhService;
        }

        #region Workflow Status Report Methods
        public DataTable GetWFReportCached(ref string lastCachedDateTime)
        {   
            items = cacheService.GetFromCache<DataTable>(CacheKeys.WFStatusReport);
            if (items == null)
            {
                items = new DataTable();
                AddColumns(items);       
         
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb spWeb = spSite.OpenWeb())
                        {
                            // Get all the information from the Compass List
                            ProcessCompassListItems(spWeb);

                            if (items.Rows.Count > 0)
                            {
                                // Get all the information from the Approval List
                                ProcessApprovalListItems(spWeb);

                                items.Columns.Remove("CompassId");
                            }
                            items.AcceptChanges();                            

                            cacheService.AddToCache<DataTable>(CacheKeys.WFStatusReport, items, new TimeSpan(1, 0, 0));

                            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                            DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cstZone);
                            cacheService.AddToCache<string>(CacheKeys.WFStatusReportCacheDate, string.Concat(cstTime.ToLongDateString(), " ", cstTime.ToLongTimeString()), new TimeSpan(1, 0, 0));
                        }
                    }
                });                
            }

            if (cacheService.GetFromCache<string>(CacheKeys.WFStatusReportCacheDate) != null)
            {
                lastCachedDateTime = cacheService.GetFromCache<string>(CacheKeys.WFStatusReportCacheDate);
            }            
            return items;
        }

        public DataTable GetWFReport()
        {
            items = new DataTable();
            AddColumns(items);
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        // Get all the information from the Compass List
                        ProcessCompassListItems(spWeb);
                        
                        if (items.Rows.Count > 0)
                        {
                            ProcessApprovalListItems(spWeb);
                            items.Columns.Remove("CompassId");
                        }
                        items.AcceptChanges();                        
                    }
                }
            });            
            return items;
        }

        private void ProcessCompassListItems(SPWeb spWeb)
        {
            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<OrderBy><FieldRef Name=\"ProjectNumber\" /></OrderBy><Where><And><Neq><FieldRef Name=\"WorkflowStep\" /><Value Type=\"Text\">Completed</Value></Neq><Neq><FieldRef Name=\"WorkflowStep\" /><Value Type=\"Text\">Cancelled</Value></Neq></And></Where>";
            SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
            SPListItemCollection itemCol = spList.GetItems(spQuery);

            foreach (SPListItem item in itemCol)
            {
                DataRow newRow = items.NewRow();
                newRow["CompassId"] = Convert.ToString(item.ID);
                newRow["Project"] = Convert.ToString(item[CompassListFields.ProjectNumber]);
                newRow["Season"] = Convert.ToString(item[CompassListFields.ProductHierarchyLevel2]);
                //newRow["Workflow Step"] = Convert.ToString(item[CompassListFields.WORKFLOW_Step]);
                newRow["SAP Item #"] = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                newRow["Item Description"] = Convert.ToString(item[CompassListFields.SAPDescription]);

                StringBuilder opsString = new StringBuilder();
                opsString.Append(GetCompassData(CompassListFields.ManufacturingLocation, item, "Make"));
                opsString.Append(GetCompassData(CompassListFields.PackingLocation, item, "Pack"));
                opsString.Append(GetCompassData(CompassListFields.DistributionCenter, item, "Dist"));
                newRow["OPS"] = opsString.ToString();

                //newRow["Notes"] = Convert.ToString(item[CompassListFields.OBM_Comments]);
                if (!string.IsNullOrEmpty(CompassListFields.PM))
                {
                    newRow["OBM"] = userService.GetUserNameFromPersonField(Convert.ToString(item[CompassListFields.PM]));
                }
                //@Fatimah
                /*if (!string.IsNullOrEmpty(CompassListFields.BrandManager))
                {
                    newRow["Brand Mgr"] = userService.GetUserNameFromPersonField(Convert.ToString(item[CompassListFields.BrandManager]));
                }*/
                items.Rows.Add(newRow);
            }
        }

        private void ProcessApprovalListItems(SPWeb spWeb)
        {
            SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);
            foreach (DataRow dRow in items.Rows)
            {
                string query = @"<Where><Eq><FieldRef Name='{0}' /><Value Type='Number'>{1}</Value></Eq></Where>";
                query = string.Format(query, ApprovalListFields.CompassListItemId, Convert.ToInt32(dRow["CompassId"]));
                SPQuery spQuery = new SPQuery();
                spQuery.Query = query;
                spQuery.RowLimit = 1;
                SPListItemCollection approvalItems = spList.GetItems(spQuery);
                if (approvalItems.Count > 0)
                {
                    SPListItem item = approvalItems[0];
                    //var item = spList.Items.OfType<SPListItem>().FirstOrDefault(x => Convert.ToInt32(x[ApprovalListFields.CompassListItemId]).Equals(dRow["CompassId"]));
                    if (item != null)
                    {
                        dRow["International Compliance"] = string.Concat((Convert.ToString(item[ApprovalListFields.Operations_ModifiedBy])), " ", Convert.ToString(item[ApprovalListFields.Operations_ModifiedDate]));
                        dRow["QA"] = string.Concat((Convert.ToString(item[ApprovalListFields.QA_ModifiedBy])), " ", Convert.ToString(item[ApprovalListFields.QA_ModifiedDate]));
                        dRow["Customer Marketing"] = string.Concat((Convert.ToString(item[ApprovalListFields.Distribution_ModifiedBy])), " ", Convert.ToString(item[ApprovalListFields.Distribution_ModifiedDate]));
                        dRow["TBD"] = string.Concat((Convert.ToString(item[ApprovalListFields.SAPInitialSetup_ModifiedBy])), " ", Convert.ToString(item[ApprovalListFields.SAPInitialSetup_ModifiedDate]));

                        StringBuilder initialReviewString = new StringBuilder();
                        initialReviewString.Append(GetApprovalData(ApprovalListFields.InitialCosting_SubmittedBy, ApprovalListFields.InitialCosting_SubmittedDate, item, "CST"));
                        initialReviewString.Append(GetApprovalData(ApprovalListFields.SrOBMApproval_ModifiedBy, ApprovalListFields.SrOBMApproval_ModifiedDate, item, "APP"));
                        dRow["Initial Review"] = initialReviewString.ToString();
                        //dRow["Initial Costing Review"] = string.Concat((Convert.ToString(item[ApprovalListFields.ICF_CST_ModifiedBy])), " ", Convert.ToString(item[ApprovalListFields.ICF_CST_ModifiedDate]));
                        //dRow["Initial Approver Review"] = string.Concat((Convert.ToString(item[ApprovalListFields.ICF_SLT_ModifiedBy])), " ", Convert.ToString(item[ApprovalListFields.ICF_SLT_ModifiedDate]), " ", Convert.ToString(item[ApprovalListFields.ICF_SLT_Decision]));
                        //dRow["Item Request Form"] = string.Concat((Convert.ToString(item[ApprovalListFields.IRF_SAP_ModifiedBy])), " ", Convert.ToString(item[ApprovalListFields.IRF_SAP_ModifiedDate]));

                        StringBuilder matNumString = new StringBuilder();
                        matNumString.Append(GetApprovalData(ApprovalListFields.BOMSetupPE_ModifiedBy, ApprovalListFields.BOMSetupPE_ModifiedDate, item, "BOMSetupPE"));
                        matNumString.Append(GetApprovalData(ApprovalListFields.BOMSetupProc_ModifiedBy, ApprovalListFields.BOMSetupProc_ModifiedDate, item, "BOMSetupProc"));
                        matNumString.Append(GetApprovalData(ApprovalListFields.BOMSetupPE2_ModifiedBy, ApprovalListFields.BOMSetupPE2_ModifiedDate, item, "BOMSetupPE2"));
                        dRow["Material Confirmation"] = matNumString.ToString();

                        dRow["Graphics Request"] = string.Concat((Convert.ToString(item[ApprovalListFields.GRAPHICS_ModifiedBy])), " ", Convert.ToString(item[ApprovalListFields.GRAPHICS_ModifiedDate]));
                        dRow.AcceptChanges();
                    }
                }
            }
        }

        private string GetApprovalData(string user,string date, SPListItem item, string type)
        {
            StringBuilder str = new StringBuilder();
            string modifiedUser = (Convert.ToString(item[user]));
            if (!string.IsNullOrEmpty(modifiedUser))
            {
                str.Append(string.Concat("<p>", type, " : ", modifiedUser, " ", Convert.ToString(item[date]), "</p>"));
            }
            return str.ToString();
        }

        private string GetCompassData(string data, SPListItem item, string type)
        {
            StringBuilder str = new StringBuilder();
            string dataString = (Convert.ToString(item[data]));
            if (!string.IsNullOrEmpty(dataString))
            {
                str.Append(string.Concat("<p>", type, " : ", dataString, "</p>"));
            }
            return str.ToString();
        }

        private void AddColumns(DataTable items)
        {
            items.Columns.Add(new DataColumn("CompassId", typeof(int)));
            items.Columns.Add(new DataColumn("OBM"));
            items.Columns.Add(new DataColumn("Season"));
            items.Columns.Add(new DataColumn("Workflow Step"));
            items.Columns.Add(new DataColumn("Project"));
            items.Columns.Add(new DataColumn("SAP Item #"));
            items.Columns.Add(new DataColumn("Item Description"));
            items.Columns.Add(new DataColumn("Initial Review"));
            items.Columns.Add(new DataColumn("OPS"));            
            items.Columns.Add(new DataColumn("International Compliance"));
            items.Columns.Add(new DataColumn("QA"));
            items.Columns.Add(new DataColumn("Customer Marketing"));
            items.Columns.Add(new DataColumn("TBD"));
            items.Columns.Add(new DataColumn("Material Confirmation"));
            items.Columns.Add(new DataColumn("Graphics Request"));
            items.Columns.Add(new DataColumn("SAP Item Setup"));            
            items.Columns.Add(new DataColumn("Brand Mgr"));
            items.Columns.Add(new DataColumn("Notes"));            
        }
        #endregion

        #region Report By View Methods
        private string FixWhereClause(string query)
        {
            SPUser user = SPContext.Current.Web.CurrentUser;

            if (query.IndexOf("<UserID Type=\"Integer\" />") > 0)
            {
                // Find the previous FieldRef Name and add the 'LookupId=\"TRUE\"'
                string temp = query.Substring(0, query.IndexOf("<UserID Type=\"Integer\" />"));
                int index = temp.LastIndexOf("FieldRef Name");
                temp = temp.Substring(index);
                index = temp.IndexOf("/>");
                temp = temp.Substring(0, index);

                query = query.Replace(temp, temp + " LookupId=\"TRUE\" ");

                //query = query.Replace("<FieldRef Name=\"OBM_OBM\" />", "<FieldRef Name=\"OBM_OBM\" LookupId=\"TRUE\" />");
                //query = query.Replace("<FieldRef Name=\"Author\" />", "<FieldRef Name=\"Author\" LookupId=\"TRUE\" />");

                // Replace the UserID with the actual user id of the currently logged in user
                query = query.Replace("<UserID Type=\"Integer\" />", user.ID.ToString());
            }

            return query;
        }

        public DataTable GetReportByView(string list, string query, System.Collections.Specialized.StringCollection columns)
        {
            DataTable reportItems = new DataTable();                     
            
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        SPList spList = spWeb.Lists.TryGetList(list);
                        query = FixWhereClause(query);
                        foreach (string sCol in columns)
                        {   
                            var spField = spList.Fields.GetFieldByInternalName(sCol);
                            DataColumn column = new DataColumn(spField.Title, typeof(string));
                            column.Caption = spField.TypeAsString.ToUpper();
                            reportItems.Columns.Add(column);
                        }

                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = query;
                        SPListItemCollection itemCol = spList.GetItems(spQuery);
                        string value;

                        foreach (SPListItem item in itemCol)
                        {
                            DataRow newRow = reportItems.NewRow();
                            foreach (DataColumn col in reportItems.Columns)
                            {
                                if (col.Caption.Equals("USER"))
                                {
                                    newRow[col] = userService.GetUserNameFromPersonField(Convert.ToString(item[col.ColumnName]));                                    
                                }
                                else if (col.Caption.Equals("URL"))
                                {
                                    value = Convert.ToString(item[col.ColumnName]);
                                    if (value.IndexOf(',') > 0)
                                        newRow[col] = string.Concat("<a href='", value.Substring(0, value.IndexOf(',')), "'>", value.Substring(value.IndexOf(',') + 1), "</a>");
                                    else
                                        newRow[col] = string.Empty;
                                    //newRow[col] = string.Concat("<a href='", SPContext.Current.Web.Url, "/pages/", GlobalConstants.PAGE_CommercializationItem, "?ProjectNo=", Convert.ToString(item[CompassListFields.IPF_ProjectNumber]), "'>", Convert.ToString(item[CompassListFields.IPF_ProjectNumber]), "</a>");
                                }
                                else if (col.Caption.Equals("DATETIME"))
                                {
                                    // If the time component is "12:00:00 AM", then just remove it
                                    newRow[col] = Convert.ToString(item[col.ColumnName]).Replace(" 12:00:00 AM", "");
                                }
                                //else if (col.Caption.Equals("NOTE"))
                                //{
                                //    //var noteValue = Convert.ToString(item[col.ColumnName]).Replace("\r\n", "<br>");
                                //    newRow[col] = Convert.ToString(item[col.ColumnName]).Replace("\r\n", "<br>");
                                //}
                                else
                                {
                                    newRow[col] = Convert.ToString(item[col.ColumnName]);
                                }
                            }
                            reportItems.Rows.Add(newRow);
                        }                        
                        reportItems.AcceptChanges();
                    }
                }
            });

            return reportItems;
        }
        #endregion

        #region Project Totals Report
        public Dictionary<string, int> GetSeasonalProjectTotalsReport()
        {
            //items = cacheService.GetFromCache<DataTable>(CacheKeys.TotalProjectsReport);
            //if (items != null)
            //    return items;

            //items = new DataTable();
            //items.Columns.Add(new DataColumn("Category"));
            //items.Columns.Add(new DataColumn("Total Projects", typeof(int)));
            //items.Columns.Add(new DataColumn("Red", typeof(int)));
            //items.Columns.Add(new DataColumn("Yellow", typeof(int)));
            //items.Columns.Add(new DataColumn("Green", typeof(int)));

            SPListItemCollection itemCol = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<OrderBy><FieldRef Name=\"ProjectNumber\" /></OrderBy><Where><And><Neq><FieldRef Name=\"WorkflowStep\" /><Value Type=\"Text\">Completed</Value></Neq><Neq><FieldRef Name=\"WorkflowStep\" /><Value Type=\"Text\">Cancelled</Value></Neq></And></Where>";
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        itemCol = spList.GetItems(spQuery);
                    }
                }
            });

            // Calculate totals
            Dictionary<string, int> reportTotals = new Dictionary<string,int>();
            if (itemCol == null)
                return reportTotals;
            
            string season;
            string projectStatus;
            //string brand;
            int value;
            foreach (SPListItem item in itemCol)
            {
                season = Convert.ToString(item[CompassListFields.ProductHierarchyLevel2]);
                projectStatus = Convert.ToString(item[CompassListFields.ProjectStatus]);
//                brand = Convert.ToString(item[CompassListFields.IPF_Brand]);

                // Collect Season Totals
                if ((string.Equals(season, TotalProjectsFields.Holiday2014)) || (string.Equals(season, TotalProjectsFields.Holiday2014)))
                {
                    if (reportTotals.TryGetValue(TotalProjectsFields.SeasonalTotal, out value))
                        reportTotals[TotalProjectsFields.SeasonalTotal] = reportTotals[TotalProjectsFields.SeasonalTotal] + 1;
                    else
                        reportTotals.Add(TotalProjectsFields.SeasonalTotal, 1);
                    if (reportTotals.TryGetValue(TotalProjectsFields.ChristmasTotal, out value))
                        reportTotals[TotalProjectsFields.ChristmasTotal] = reportTotals[TotalProjectsFields.ChristmasTotal] + 1;
                    else
                        reportTotals.Add(TotalProjectsFields.ChristmasTotal, 1);

                    DetermineProjectStatus(projectStatus, TotalProjectsFields.ChristmasTotalRed, TotalProjectsFields.ChristmasTotalYellow, TotalProjectsFields.ChristmasTotalGreen, reportTotals);
                    DetermineProjectStatus(projectStatus, TotalProjectsFields.SeasonalTotalRed, TotalProjectsFields.SeasonalTotalYellow, TotalProjectsFields.SeasonalTotalGreen, reportTotals);
                }
                else if ((string.Equals(season, TotalProjectsFields.Valentines2014)) || (string.Equals(season, TotalProjectsFields.Valentines2015)))
                {
                    if (reportTotals.TryGetValue(TotalProjectsFields.SeasonalTotal, out value))
                        reportTotals[TotalProjectsFields.SeasonalTotal] = reportTotals[TotalProjectsFields.SeasonalTotal] + 1;
                    else
                        reportTotals.Add(TotalProjectsFields.SeasonalTotal, 1);
                    if (reportTotals.TryGetValue(TotalProjectsFields.ValentinesTotal, out value))
                        reportTotals[TotalProjectsFields.ValentinesTotal] = reportTotals[TotalProjectsFields.ValentinesTotal] + 1;
                    else
                        reportTotals.Add(TotalProjectsFields.ValentinesTotal, 1);

                    DetermineProjectStatus(projectStatus, TotalProjectsFields.ValentinesTotalRed, TotalProjectsFields.ValentinesTotalYellow, TotalProjectsFields.ValentinesTotalGreen, reportTotals);
                    DetermineProjectStatus(projectStatus, TotalProjectsFields.SeasonalTotalRed, TotalProjectsFields.SeasonalTotalYellow, TotalProjectsFields.SeasonalTotalGreen, reportTotals);
                }
                else if ((string.Equals(season, TotalProjectsFields.Easter2014)) || (string.Equals(season, TotalProjectsFields.Easter2015)))
                {
                    if (reportTotals.TryGetValue(TotalProjectsFields.SeasonalTotal, out value))
                        reportTotals[TotalProjectsFields.SeasonalTotal] = reportTotals[TotalProjectsFields.SeasonalTotal] + 1;
                    else
                        reportTotals.Add(TotalProjectsFields.SeasonalTotal, 1);
                    if (reportTotals.TryGetValue(TotalProjectsFields.EasterTotal, out value))
                        reportTotals[TotalProjectsFields.EasterTotal] = reportTotals[TotalProjectsFields.EasterTotal] + 1;
                    else
                        reportTotals.Add(TotalProjectsFields.EasterTotal, 1);

                    DetermineProjectStatus(projectStatus, TotalProjectsFields.EasterTotalRed, TotalProjectsFields.EasterTotalYellow, TotalProjectsFields.EasterTotalGreen, reportTotals);
                    DetermineProjectStatus(projectStatus, TotalProjectsFields.SeasonalTotalRed, TotalProjectsFields.SeasonalTotalYellow, TotalProjectsFields.SeasonalTotalGreen, reportTotals);
                }
                else if ((string.Equals(season, TotalProjectsFields.Summer2014)) || (string.Equals(season, TotalProjectsFields.Summer2015)))
                {
                    if (reportTotals.TryGetValue(TotalProjectsFields.SeasonalTotal, out value))
                        reportTotals[TotalProjectsFields.SeasonalTotal] = reportTotals[TotalProjectsFields.SeasonalTotal] + 1;
                    else
                        reportTotals.Add(TotalProjectsFields.SeasonalTotal, 1);
                    if (reportTotals.TryGetValue(TotalProjectsFields.SummerTotal, out value))
                        reportTotals[TotalProjectsFields.SummerTotal] = reportTotals[TotalProjectsFields.SummerTotal] + 1;
                    else
                        reportTotals.Add(TotalProjectsFields.SummerTotal, 1);

                    DetermineProjectStatus(projectStatus, TotalProjectsFields.SummerTotalRed, TotalProjectsFields.SummerTotalYellow, TotalProjectsFields.SummerTotalGreen, reportTotals);
                    DetermineProjectStatus(projectStatus, TotalProjectsFields.SeasonalTotalRed, TotalProjectsFields.SeasonalTotalYellow, TotalProjectsFields.SeasonalTotalGreen, reportTotals);
                }
                else if ((string.Equals(season, TotalProjectsFields.Halloween2014)) || (string.Equals(season, TotalProjectsFields.Halloween2014)))
                {
                    if (reportTotals.TryGetValue(TotalProjectsFields.SeasonalTotal, out value))
                        reportTotals[TotalProjectsFields.SeasonalTotal] = reportTotals[TotalProjectsFields.SeasonalTotal] + 1;
                    else
                        reportTotals.Add(TotalProjectsFields.SeasonalTotal, 1);
                    if (reportTotals.TryGetValue(TotalProjectsFields.HalloweenTotal, out value))
                        reportTotals[TotalProjectsFields.HalloweenTotal] = reportTotals[TotalProjectsFields.HalloweenTotal] + 1;
                    else
                        reportTotals.Add(TotalProjectsFields.HalloweenTotal, 1);

                    DetermineProjectStatus(projectStatus, TotalProjectsFields.HalloweenTotalRed, TotalProjectsFields.HalloweenTotalYellow, TotalProjectsFields.HalloweenTotalGreen, reportTotals);
                    DetermineProjectStatus(projectStatus, TotalProjectsFields.SeasonalTotalRed, TotalProjectsFields.SeasonalTotalYellow, TotalProjectsFields.SeasonalTotalGreen, reportTotals);
                }

                // Collect Brand Totals
                //if (reportTotals.TryGetValue(brand, out value))
                //    reportTotals[brand] = reportTotals[brand] + 1;
                //else
                //    reportTotals.Add(brand, 1);
                //DetermineProjectStatus(projectStatus, brand+"Red", brand+"Yellow", brand+"Green", reportTotals);

                        //var newRow = items.NewRow();
                            //newRow["CompassId"] = Convert.ToString(item.ID);
                            //newRow["Project"] = Convert.ToString(item[CompassListFields.IPF_ProjectNumber]);
                            //newRow["Season"] = Convert.ToString(item[CompassListFields.IPF_Season]);
                            ///newRow["Workflow Step"] = Convert.ToString(item[CompassListFields.WORKFLOW_Step]);
                           // newRow["Master Data"] = Convert.ToString(item[CompassListFields.SIR_SAPItemNumber]);
                            //newRow["Item Description"] = Convert.ToString(item[CompassListFields.SIR_SAPDescription]);
                            //newRow["OPS"] = Convert.ToString(item[CompassListFields.OPS_ManufacturingLocation]);
                            //newRow["Distribution"] = Convert.ToString(item[CompassListFields.OPS_DistributionCenter]);

                            //items.Rows.Add(newRow);
            }
            //items.AcceptChanges();

            // Add data to the cache
            //cacheService.AddToCache<DataTable>(CacheKeys.TotalProjectsReport, reportTotals, new TimeSpan(24, 0, 0));

            return reportTotals;
        }

        public DataTable GetEverydayProjectTotalsReport()
        {
            //items = cacheService.GetFromCache<DataTable>(CacheKeys.TotalProjectsReport);
            //if (items != null)
            //    return items;

            items = new DataTable();
            items.Columns.Add(new DataColumn("Brand"));
            items.Columns.Add(new DataColumn("Total Projects", typeof(int)));
            items.Columns.Add(new DataColumn("Red", typeof(int)));
            items.Columns.Add(new DataColumn("Yellow", typeof(int)));
            items.Columns.Add(new DataColumn("Green", typeof(int)));

            SPListItemCollection itemCol = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<OrderBy><FieldRef Name=\"ProjectNumber\" /></OrderBy><Where><And><And><Neq><FieldRef Name=\"WorkflowStep\" /><Value Type=\"Text\">Completed</Value></Neq><Neq><FieldRef Name=\"WorkflowStep\" /><Value Type=\"Text\">Cancelled</Value></Neq></And><Eq><FieldRef Name=\"IPF_ProjectSeasonType\" /><Value Type=\"Text\">EVERYDAY</Value></Eq></And></Where>";
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        itemCol = spList.GetItems(spQuery);
                    }
                }
            });

            // Calculate totals
            Dictionary<string, int> reportTotals = new Dictionary<string, int>();
            List<string> brandsList = new List<string>();

            if (itemCol == null)
                return null;

            brandsList.Add(TotalProjectsFields.EverydayTotal);
            string projectStatus;
            string brand;
            int value;

            foreach (SPListItem item in itemCol)
            {
                projectStatus = Convert.ToString(item[CompassListFields.ProjectStatus]);
                brand = Convert.ToString(item[CompassListFields.MaterialGroup1Brand]);
                if (brand.Contains(TotalProjectsFields.PrivateLabel))
                    brand = TotalProjectsFields.PrivateLabel;

                // Collect Everyday Total
                if (reportTotals.TryGetValue(TotalProjectsFields.EverydayTotal, out value))
                    reportTotals[TotalProjectsFields.EverydayTotal] = reportTotals[TotalProjectsFields.EverydayTotal] + 1;
                else
                    reportTotals.Add(TotalProjectsFields.EverydayTotal, 1);

                // Collect Brand Totals
                if (reportTotals.TryGetValue(brand, out value))
                    reportTotals[brand] = reportTotals[brand] + 1;
                else
                    reportTotals.Add(brand, 1);
                DetermineProjectStatus(projectStatus, brand + "Red", brand + "Yellow", brand + "Green", reportTotals);
                DetermineProjectStatus(projectStatus, TotalProjectsFields.EverydayTotal + "Red", TotalProjectsFields.EverydayTotal + "Yellow", TotalProjectsFields.EverydayTotal + "Green", reportTotals);

                if (!brandsList.Contains(brand))
                    brandsList.Add(brand);
            }

            DataRow newRow;
            foreach (string brands in brandsList)
            {
                newRow = items.NewRow();
                newRow["Brand"] = Convert.ToString(brands);
                newRow["Total Projects"] = reportTotals[brands];
                
                if (reportTotals.TryGetValue(brands + "Red", out value))
                    newRow["Red"] = reportTotals[brands + "Red"];
                else
                    newRow["Red"] = 0;

                if (reportTotals.TryGetValue(brands + "Yellow", out value))
                    newRow["Yellow"] = reportTotals[brands + "Yellow"];
                else
                    newRow["Yellow"] = 0;

                if (reportTotals.TryGetValue(brands + "Green", out value))
                    newRow["Green"] = reportTotals[brands + "Green"];
                else
                    newRow["Green"] = 0;

                items.Rows.Add(newRow);
            }
            items.AcceptChanges();

            // Add data to the cache
            //cacheService.AddToCache<DataTable>(CacheKeys.TotalProjectsReport, reportTotals, new TimeSpan(24, 0, 0));

            return items;
        }
        
        private void DetermineProjectStatus(string projectStatus, string red, string yellow, string green, Dictionary<string, int> reportTotals)
        {
            int value;
            if (string.Equals(projectStatus, "Red"))
            {
                if (reportTotals.TryGetValue(red, out value))
                    reportTotals[red] = reportTotals[red] + 1;
                else
                    reportTotals.Add(red, 1);
            }
            else if (string.Equals(projectStatus, "Yellow"))
            {
                if (reportTotals.TryGetValue(yellow, out value))
                    reportTotals[yellow] = reportTotals[yellow] + 1;
                else
                    reportTotals.Add(yellow, 1);
            }
            else
            {
                if (reportTotals.TryGetValue(green, out value))
                    reportTotals[green] = reportTotals[green] + 1;
                else
                    reportTotals.Add(green, 1);
            }
        }
        #endregion

        #region Graphics Progress Report
        public DataTable GetGraphicsProgressReport(string projectNumber)
        {
            items = new DataTable();
            AddGraphicsProgressColumns(items);
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        // Get all the information from the Compass List
                        if (string.IsNullOrEmpty(projectNumber))
                            ProcessGraphicsProgressItems(spWeb, "<OrderBy><FieldRef Name=\"RevisedFirstShipDate\" Type='DateTime' Ascending='TRUE'/></OrderBy><Where><Eq><FieldRef Name=\"WorkflowStep\" /><Value Type=\"Text\">GPP</Value></Eq></Where>", true);
                        else
                            ProcessGraphicsProgressItems(spWeb, "<OrderBy><FieldRef Name=\"RevisedFirstShipDate\" Type='DateTime' Ascending='TRUE'/></OrderBy><Where><Eq><FieldRef Name=\"ProjectNumber\" /><Value Type=\"Text\">" + projectNumber + "</Value></Eq></Where>", false);
                    }
                }
            });
            return items;
        }

        public DataTable GetGraphicsProgressReportBySAPNumber(string sapNumber)
        {
            items = new DataTable();
            AddGraphicsProgressColumns(items);
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        // Get all the information from the Compass List
                        ProcessGraphicsProgressItems(spWeb, "<OrderBy><FieldRef Name=\"RevisedFirstShipDate\" Type='DateTime' Ascending='TRUE'/></OrderBy><Where><Eq><FieldRef Name=\"SAPItemNumber\" /><Value Type=\"Text\">" + sapNumber + "</Value></Eq></Where>", false);

                        //items.AcceptChanges();
                    }
                }
            });
            return items;
        }

        public DateTime GetLatestGraphicsImportDate()
        {
            DateTime date = DateTime.MinValue;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<OrderBy><FieldRef Name=\"Created\" Type='DateTime' Ascending='FALSE'/></OrderBy>";
                        spQuery.RowLimit = 1;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_GraphicsLogsListName);
                        SPListItemCollection itemCol = spList.GetItems(spQuery);

                        if ((itemCol != null) && (itemCol.Count > 0))
                        {
                            SPListItem item = itemCol[0];
                            date = Convert.ToDateTime(item["Created"].ToString());
                        }
                    }
                }
            });

            return date;
        }

        private void AddGraphicsProgressColumns(DataTable items)
        {
            items.Columns.Add(new DataColumn("CompassId", typeof(int)));
            items.Columns.Add(new DataColumn("ProjectView"));
            items.Columns.Add(new DataColumn("MaterialNumber"));
            items.Columns.Add(new DataColumn("Season"));
            items.Columns.Add(new DataColumn("Brand"));
            items.Columns.Add(new DataColumn("ProjectNumber"));
            items.Columns.Add(new DataColumn("SAPNumber"));
            items.Columns.Add(new DataColumn("SAPDescription"));
            items.Columns.Add(new DataColumn("Notes"));
            items.Columns.Add(new DataColumn("SubmittedDate"));
            items.Columns.Add(new DataColumn("SubmittedBy"));
            items.Columns.Add(new DataColumn("Routing"));
            items.Columns.Add(new DataColumn("RoutingReleased"));
            items.Columns.Add(new DataColumn("PDFApproved"));
            items.Columns.Add(new DataColumn("PlatesShipped"));
            items.Columns.Add(new DataColumn("RoutingHistory"));
            items.Columns.Add(new DataColumn("RevisedFirstShipDate"));
            items.Columns.Add(new DataColumn("CriticalInitiative"));
            items.Columns.Add(new DataColumn("TBDIndicator"));
        }

        private void ProcessGraphicsProgressItems(SPWeb spWeb, string query, bool allProjects)
        {
            SPQuery spQuery = new SPQuery();
            spQuery.Query = query;
            SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
            SPListItemCollection itemCol = spList.GetItems(spQuery);

            foreach (SPListItem item in itemCol)
            {
                // Get all the Graphics items for this project
                List<PackagingItem> packagingItems = packagingItemService.GetGraphicsProgressPackagingItemsForProject(item.ID);
                foreach (PackagingItem packItem in packagingItems)
                {
                    DataRow newRow = items.NewRow();
                    if (allProjects)
                        newRow["ProjectView"] = "No";
                    else
                        newRow["ProjectView"] = "Yes";
                    newRow["CompassId"] = Convert.ToString(packItem.Id);
                    //newRow["ProjectNumber"] = Convert.ToString(item[CompassListFields.IPF_ProjectNumber]);
                    //newRow["Season"] = Convert.ToString(item[CompassListFields.IPF_Season]);
                    //newRow["Brand"] = Convert.ToString(item[CompassListFields.CMF_MaterialGroup1Brand]);
                    //newRow["SAPNumber"] = Convert.ToString(item[CompassListFields.SIR_SAPItemNumber]);
                    //newRow["SAPDescription"] = Convert.ToString(item[CompassListFields.SIR_SAPDescription]);
                    //newRow["SubmittedDate"] = Convert.ToDateTime(item[CompassListFields.IPF_SubmittedDate]).ToShortDateString();
                    //newRow["SubmittedBy"] = GetPersonFieldForDisplay(Convert.ToString(item[CompassListFields.IPF_Initiator]));
                    //newRow["RevisedFirstShipDate"] = Convert.ToDateTime(item[CompassListFields.OBM_RevisedFirstShipDate]).ToShortDateString();
                    //newRow["CriticalInitiative"] = Convert.ToString(item[CompassListFields.REPORT_CriticalInitiative]);
                    //newRow["TBDIndicator"] = Convert.ToString(item[CompassListFields.IPF_TBDIndicator]);

                    newRow["MaterialNumber"] = Convert.ToString(packItem.MaterialNumber);
                    newRow["Routing"] = GetDateForDisplay(packItem.GraphicsRoutingDate);
                    newRow["RoutingReleased"] = GetDateForDisplay(packItem.GraphicsRoutingReleasedDate);
                    newRow["PDFApproved"] = GetDateForDisplay(packItem.GraphicsPDFApprovedDate);
                    newRow["PlatesShipped"] = GetDateForDisplay(packItem.GraphicsPlatesShippedDate);
                    newRow["Notes"] = Convert.ToString(packItem.GraphicsNotes);
                    if (!allProjects)
                        newRow["RoutingHistory"] = versionHistoryService.GetVersionDisplay(versionHistoryService.GetGraphicsRoutingVersionHistory(packItem.Id));
                    else
                        newRow["RoutingHistory"] = string.Empty;

                    items.Rows.Add(newRow);
                }
            }
        }

        private string GetPersonFieldForDisplay(string person)
        {
            if (string.IsNullOrEmpty(person))
                return string.Empty;
            if (person.IndexOf("#") < 0)
                return person;

            return person.Substring(person.IndexOf("#") + 1);
        }

        private string GetDateForDisplay(DateTime currentValue)
        {
            if (currentValue == Convert.ToDateTime(null))
                return string.Empty;
            else if (currentValue.Equals("1/1/0001"))
                return string.Empty;
            else
                return currentValue.ToShortDateString();
        }

        
        #endregion

    }
}
