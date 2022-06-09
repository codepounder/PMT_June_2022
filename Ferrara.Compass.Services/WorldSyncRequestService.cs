using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Microsoft.SharePoint;

namespace Ferrara.Compass.Services
{
    public class WorldSyncRequestService : IWorldSyncRequestService
    {
        public int InsertRequest(WorldSyncRequestItem request)
        {
            int itemId = 0;
            SPList spList;
            SPListItem appItem = null;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_WorldSyncRequestList);
                        appItem = spList.AddItem();
                        appItem["Title"] = request.SAPnumber;
                        appItem[WorldSyncRequestFields.SAPnumber] = request.SAPnumber;
                        appItem[WorldSyncRequestFields.SAPdescription] = request.SAPdescription;
                        appItem[WorldSyncRequestFields.RequestType] = request.RequestType;
                        appItem[WorldSyncRequestFields.RequestStatus] = request.RequestStatus;
                        appItem[WorldSyncRequestFields.WorkflowStep] = request.WorkflowStep;
                        appItem.Update();
                        spWeb.AllowUnsafeUpdates = false;
                        itemId = appItem.ID;
                    }
                }
            });
            return itemId;
        }
        public void UpdateRequest(WorldSyncRequestItem request)
        {
            SPList spList;
            SPListItem appItem = null;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_WorldSyncRequestList);
                        appItem = spList.GetItemById(request.RequestId);
                        appItem["Title"] = request.SAPnumber;
                        appItem[WorldSyncRequestFields.SAPnumber] = request.SAPnumber;
                        appItem[WorldSyncRequestFields.SAPdescription] = request.SAPdescription;
                        appItem[WorldSyncRequestFields.RequestType] = request.RequestType;
                        appItem[WorldSyncRequestFields.RequestStatus] = request.RequestStatus;
                        appItem[WorldSyncRequestFields.WorkflowStep] = request.WorkflowStep;
                        appItem.Update();
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public List<WorldSyncRequestItem> GetRequestItems(string SAPnumber)
        {
            return GetRequestItems(SAPnumber, null);
        }
        public List<WorldSyncRequestItem> GetRequestItems(string SAPnumber, string requestType)
        {
            return GetRequestItems(SAPnumber, null, null);
        }
        public List<WorldSyncRequestItem> GetRequestItems(string SAPnumber, string requestType, string requestStatus)
        {
            string query;
            if (string.IsNullOrEmpty(requestType))
                query = "<Where><Eq><FieldRef Name=\"" + WorldSyncRequestFields.SAPnumber + "\" /><Value Type=\"Text\">" + SAPnumber +
                "</Value></Eq></Where>";
            else
                if (string.IsNullOrEmpty(requestStatus))
                    query = "<Where><And><Eq><FieldRef Name=\"" + WorldSyncRequestFields.SAPnumber + "\" /><Value Type=\"Text\">" + SAPnumber +
                        "</Value></Eq><Eq><FieldRef Name=\"" + WorldSyncRequestFields.RequestType + "\" /><Value Type=\"Text\">" + requestType +
                        "</Value></Eq></And></Where>";
                else
                    query = "<Where><And><And><Eq><FieldRef Name=\"" + WorldSyncRequestFields.SAPnumber + "\" /><Value Type=\"Text\">" + SAPnumber +
                        "</Value></Eq><Eq><FieldRef Name=\"" + WorldSyncRequestFields.RequestType + "\" /><Value Type=\"Text\">" + requestType +
                        "</Value></Eq></And><Eq><FieldRef Name=\"" + WorldSyncRequestFields.RequestStatus + "\" /><Value Type=\"Text\">" + requestStatus +
                        "</Value></Eq></And></Where>";
            return GetRequestItemsByQuery(query);
        }
        private List<WorldSyncRequestItem> GetRequestItemsByQuery(string query)
        {
            SPList spList;
            SPQuery spQuery;
            SPListItemCollection compassItemCol;
            WorldSyncRequestItem request;
            var newItem = new List<WorldSyncRequestItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_WorldSyncRequestList);
                    spQuery = new SPQuery();
                    spQuery.Query = query;
                    compassItemCol = spList.GetItems(spQuery);
                    foreach (SPListItem item in compassItemCol)
                    {
                        if (item == null)
                            continue;
                        request = new WorldSyncRequestItem();
                        request.RequestId = item.ID;
                        request.SAPnumber = Convert.ToString(item[WorldSyncRequestFields.SAPnumber]);
                        request.SAPdescription = Convert.ToString(item[WorldSyncRequestFields.SAPdescription]);
                        request.RequestType = Convert.ToString(item[WorldSyncRequestFields.RequestType]);
                        request.RequestStatus = Convert.ToString(item[WorldSyncRequestFields.RequestStatus]);
                        request.WorkflowStep = Convert.ToString(item[WorldSyncRequestFields.WorkflowStep]);
                        newItem.Add(request);
                    }
                }
            }
            return newItem;
        }
        public WorldSyncRequestItem GetRequestItemById(int requestId)
        {
            SPList spList;
            SPListItem item;
            WorldSyncRequestItem request = null;
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_WorldSyncRequestList);
                    item = spList.GetItemById(requestId);
                    if (item != null)
                    {
                        request = new WorldSyncRequestItem();
                        request.RequestId = item.ID;
                        request.SAPnumber = Convert.ToString(item[WorldSyncRequestFields.SAPnumber]);
                        request.SAPdescription = Convert.ToString(item[WorldSyncRequestFields.SAPdescription]);
                        request.RequestType = Convert.ToString(item[WorldSyncRequestFields.RequestType]);
                        request.RequestStatus = Convert.ToString(item[WorldSyncRequestFields.RequestStatus]);
                        request.WorkflowStep = Convert.ToString(item[WorldSyncRequestFields.WorkflowStep]);
                    }
                }
            }
            return request;
        }
        public List<WorldSyncRequestItem> GetRequestItems()
        {
            SPList spList;
            SPListItemCollection compassItemCol;
            WorldSyncRequestItem request;
            var newItem = new List<WorldSyncRequestItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_WorldSyncRequestList);
                    compassItemCol = spList.GetItems();
                    foreach (SPListItem item in compassItemCol)
                    {
                        if (item == null)
                            continue;
                        request = new WorldSyncRequestItem();
                        request.RequestId = item.ID;
                        request.SAPnumber = Convert.ToString(item[WorldSyncRequestFields.SAPnumber]);
                        request.SAPdescription = Convert.ToString(item[WorldSyncRequestFields.SAPdescription]);
                        request.RequestType = Convert.ToString(item[WorldSyncRequestFields.RequestType]);
                        request.RequestStatus = Convert.ToString(item[WorldSyncRequestFields.RequestStatus]);
                        request.WorkflowStep = Convert.ToString(item[WorldSyncRequestFields.WorkflowStep]);
                        newItem.Add(request);
                    }
                }
            }
            return newItem;
        }
        public List<FileAttribute> GetUploadedFilesByRequestId(string SAPnumber, int requestId)
        {
            SPDocumentLibrary documentLib;
            SPFolder stageGateProjectFolder;
            List<FileAttribute> files = new List<FileAttribute>();
            using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spweb = spsite.OpenWeb())
                {
                    documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_WorldSyncRequestName) as SPDocumentLibrary;
                    var folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", SAPnumber);
                    if (spweb.GetFolder(folderUrl).Exists)
                    {
                        stageGateProjectFolder = spweb.GetFolder(folderUrl);
                        try
                        {
                            var spFiles = stageGateProjectFolder.Files.OfType<SPFile>().Where(x => x.Item[WorldSyncRequestFields.DOCLIBRARY_RequestId].Equals(requestId));

                            foreach (SPFile spfile in spFiles)
                            {
                                var file = new FileAttribute();
                                if (string.IsNullOrEmpty(spfile.Title))
                                {
                                    file.FileName = spfile.Name;
                                }
                                else
                                {
                                    file.FileName = spfile.Title;
                                }
                                file.FileUrl = string.Concat(spweb.Url, "/", spfile.Url);
                                file.DocType = spfile.Item[WorldSyncRequestFields.DOCLIBRARY_RequestType].ToString();
                                files.Add(file);
                            }
                        }
                        catch (Exception ex)
                        {
                            //exceptionService.Handle(LogCategory.CriticalError, ex, "WorldSyncRequestService", "GetUploadedFilesByRequestId(string SAPnumber, int requestId)", string.Concat(SAPnumber, "-", requestId));
                        }
                    }
                }
            }
            return files;
        }
    }
}
