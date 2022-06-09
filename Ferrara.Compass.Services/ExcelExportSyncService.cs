using System;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using Microsoft.SharePoint;
using System.Text.RegularExpressions;
using System.Web;
using System.Collections.Generic;
using System.Security.Principal;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;

namespace Ferrara.Compass.Services
{
    public class ExcelExportSyncService : IExcelExportSyncService
    {
        private readonly IExceptionService exceptionService;

        public ExcelExportSyncService(IExceptionService exceptionService)
        {
            this.exceptionService = exceptionService;
        }
        private string DownloadFile(int compassId)
        {
            SPFile file;
            FileStream fs;
            BinaryWriter bw;
            string filename;
            byte[] b;
            string constructedURL = string.Empty;
            filename = GlobalConstants.EXP_SYNC_FILENAME.Replace("{compassId}", compassId.ToString());
            filename = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Temp/"), filename);
            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    file = web.GetFile(web.Url + "/Templates/FUSE_Supplier_Template.xlsx");
                    if (file.Exists)
                    {
                        b = file.OpenBinary();
                        fs = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite);
                        bw = new BinaryWriter(fs);
                        bw.Write(b);
                        bw.Close();
                    }
                }
            }
            return filename;
        }
        public void CopyFile(int compassId, string projectNumber)
        {
            SPDocumentLibrary projectDocLib, templateDocLib;
            SPFolder stageGateProjectFolder;
            SPListItem projectFolder;
            bool success = false;
            string sourceUrl = string.Empty;
            string targetUrl = string.Empty;
            string fileName;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spweb = spsite.OpenWeb())
                    {
                        try
                        {
                            spweb.AllowUnsafeUpdates = true;
                            projectDocLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassLibraryName) as SPDocumentLibrary;
                            templateDocLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassTemplatesName) as SPDocumentLibrary;
                            sourceUrl = string.Concat(templateDocLib.RootFolder.ServerRelativeUrl, "/");
                            targetUrl = string.Concat(projectDocLib.RootFolder.ServerRelativeUrl, "/", projectNumber);
                            if (!spweb.GetFolder(targetUrl).Exists)
                            {
                                projectFolder = projectDocLib.Items.Add("", SPFileSystemObjectType.Folder, projectNumber);
                                projectFolder.Update();
                            }

                            if (spweb.GetFolder(sourceUrl).Exists)
                            {
                                stageGateProjectFolder = spweb.GetFolder(sourceUrl);
                                foreach (SPFile spfile in stageGateProjectFolder.Files)
                                {
                                    if (spfile.Name == GlobalConstants.EXP_SYNC_TEMPLATE_NAME)
                                    {
                                        fileName = GlobalConstants.EXP_SYNC_FILENAME.Replace("{compassId}", compassId.ToString());
                                        spfile.CopyTo(string.Concat(projectDocLib.RootFolder.ServerRelativeUrl, "/", projectNumber, "/", fileName), true);
                                        success = true;
                                        break;
                                    }
                                }
                            }
                            if (!success)
                                throw new Exception("Template file or folder was not found");
                            spweb.AllowUnsafeUpdates = false;
                        }
                        catch (Exception ex)
                        {
                            exceptionService.Handle(LogCategory.CriticalError, ex, "ExcelExportSyncService", "CopyFile", string.Concat("Source: ", sourceUrl, " Destination: ", targetUrl));
                        }
                    }
                }
            });
        }
        public byte[] WriteToFile(string projectNumber, List<Dictionary<string, string>> rowsItem,
            Dictionary<string, string> rowPublish, Dictionary<string, string> rowLink)
        {
            MemoryStream memStr;
            ExcelHandler handler;
            string error = "";
            uint w;
            handler = new ExcelHandler(exceptionService, false);
            handler.SetColumnHeaders(
                GlobalConstants.EXP_SYNC_SHT_ITM, 
                GlobalConstants.EXP_SYNC_ROW_HDR_ITM, 
                new string[] {
                    GlobalConstants.EXP_SYNC_ITM_ItemID,
                    GlobalConstants.EXP_SYNC_ITM_ItemName,
                    GlobalConstants.EXP_SYNC_ITM_BrandName,
                    GlobalConstants.EXP_SYNC_ITM_Depth,
                    GlobalConstants.EXP_SYNC_ITM_Height,
                    GlobalConstants.EXP_SYNC_ITM_Width,
                    GlobalConstants.EXP_SYNC_ITM_GrossWeight,
                    GlobalConstants.EXP_SYNC_ITM_NetWeight,
                    GlobalConstants.EXP_SYNC_ITM_GS1TradeItemIDKeyValue,
                    GlobalConstants.EXP_SYNC_ITM_ShortDescription,
                    GlobalConstants.EXP_SYNC_ITM_ProductDescription,
                    GlobalConstants.EXP_SYNC_ITM_FunctionalName,
                    GlobalConstants.EXP_SYNC_ITM_Volume,
                    GlobalConstants.EXP_SYNC_ITM_QtyofNextLevelItem,
                    GlobalConstants.EXP_SYNC_ITM_NumberofItemsinaCompleteLayerGTINPalletTi,
                    GlobalConstants.EXP_SYNC_ITM_NumberofCompleteLayersContainedinItemGTINPalletHi,
                    GlobalConstants.EXP_SYNC_ITM_AlternateItemIdentificationId,
                    GlobalConstants.EXP_SYNC_ITM_MinProductLifespanfromProduction,
                    GlobalConstants.EXP_SYNC_ITM_StartAvailabilityDate,
                    GlobalConstants.EXP_SYNC_ITM_EffectiveDate });
            try
            {
                w = GlobalConstants.EXP_SYNC_ROW_STRT_ITM;
                foreach(Dictionary<string, string> values in rowsItem)
                {
                    if (!handler.updateRowByHeaders(GlobalConstants.EXP_SYNC_SHT_ITM, values, w, ref error))
                        throw new Exception(error);
                    w++;
                }
                if (!handler.updateRowByHeaders(GlobalConstants.EXP_SYNC_SHT_PUB, rowPublish, GlobalConstants.EXP_SYNC_ROW_STRT_PUB, ref error, GlobalConstants.EXP_SYNC_ROW_HDR_PUB))
                    throw new Exception(error);
                if (!handler.updateRowByHeaders(GlobalConstants.EXP_SYNC_SHEET_LNK, rowLink, GlobalConstants.EXP_SYNC_ROW_STRT_LNK, ref error, GlobalConstants.EXP_SYNC_ROW_HDR_LNK))
                    throw new Exception(error);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                memStr = handler.SaveDocument();
            }
            return memStr.ToArray();
        }
        public void saveFileToDocLibrary(int compassId, string projectNumber, byte[] fileContent)
        {
            SPDocumentLibrary projectDocLib;
            SPFolder projectFolder;
            SPListItem projFolderItem;
            string targetUrl, fileName;
            fileName = GlobalConstants.EXP_SYNC_FILENAME.Replace("{compassId}", compassId.ToString());
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spweb = spsite.OpenWeb())
                    {
                        try
                        {
                            spweb.AllowUnsafeUpdates = true;
                            projectDocLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassLibraryName) as SPDocumentLibrary;
                            targetUrl = string.Concat(projectDocLib.RootFolder.ServerRelativeUrl, "/", projectNumber);
                            if (!spweb.GetFolder(targetUrl).Exists)
                            {
                                projFolderItem = projectDocLib.Items.Add("", SPFileSystemObjectType.Folder, projectNumber);
                                projFolderItem.Update();
                            }
                            projectFolder = spweb.GetFolder(targetUrl);
                            projectFolder.Files.Add(fileName, fileContent, true);
                            projectFolder.Update();
                            spweb.AllowUnsafeUpdates = false;
                        }
                        catch (Exception ex)
                        {
                            exceptionService.Handle(LogCategory.CriticalError, ex, "ExcelExportSyncService", "saveFileToDocLibrary");
                        }
                    }
                }
            });
        }
    }
}