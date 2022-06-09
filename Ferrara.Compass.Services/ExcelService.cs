using System;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using Microsoft.SharePoint;
using System.Text.RegularExpressions;
using System.Web;
using System.Security.Principal;

namespace Ferrara.Compass.Services
{
    public class ExcelService : IExcelService
    {
        private IPackagingItemService packagingItemService;
        private IExceptionService exceptionService;

        #region Const
        const string URL_Root = "/_vti_bin/ExcelRest.aspx/" + GlobalConstants.DOCLIBRARY_CompassUploadsLibraryName + "/";
        const string URL_ModelRanges = "/Model/Ranges(";
        const string URL_Format = ")?$format=atom";
        // SGS Graphics Progress - Prep Plates
        const string URL_PrepPlates = "'''Ferrara Prep.plates''!Print_Area'";
        const string FORM_SGS_PrepPlates = "SGS Prep Plates";

        const string COLUMN_PrepPlates_MaterialNumber = "Material#";
        const string COLUMN_PrepPlates_PAApproved = "PAApproved(Date)";
        const string COLUMN_PrepPlates_ProofApproved = "ProofApproved";
        const string COLUMN_PrepPlates_PlatesCompleted = "Files/PlatesCompleted&amp;shipped";

        // SGS Graphics Progress - Corrugates
        const string URL_FCCCorrugate = "'''FCC Corrugate''!Print_Area'";
        const string FORM_SGS_FCCCorrugate = "SGS FCC Corrugate";

        const string COLUMN_Corrugate_MaterialNumber = "Material#";
        const string COLUMN_Corrugate_PAApproved = "PAApproved";
        const string COLUMN_Corrugate_ProofApproved = "ProofApproved";
        const string COLUMN_Corrugate_PlatesCompleted = "PlatesCompletedandShipped";
        #endregion

        #region Member Variables
        int currentColumn = 0;
        string value = string.Empty;
        bool dataCaptured = false;
        string error = string.Empty;
        string returnMessage = string.Empty;
        string responseValue = string.Empty;
        string header = string.Empty;
        // SGS Graphics Progress 
        string strPAApproved = string.Empty;
        string strProofApproved = string.Empty;
        string strPlatesCompleted = string.Empty;
        string strMaterialNumber = string.Empty;
        int colPAApproved = 0;
        int colProofApproved = 0;
        int colPlatesCompleted = 0;
        int colMaterialNumber = 0;

        #endregion

        public ExcelService(IPackagingItemService packItemService, IExceptionService exService)
        {
            this.packagingItemService = packItemService;
            this.exceptionService = exService;
        }

        #region SGS Prep Plates
        public string ImportSGSPrepPlatesFile(string filename)
        {
            Initialize();
            string constructedURL = string.Empty;

            try
            {
                // Open the XLS with the REST API
                constructedURL = SPContext.Current.Web.Url + URL_Root + filename + URL_ModelRanges + URL_PrepPlates + URL_Format;
                constructedURL = HttpUtility.UrlPathEncode(constructedURL);
                // Replace ' with %27 because UrlPathEncode doesn't change them
                constructedURL = constructedURL.Replace("'''", "'%27%27");
                constructedURL = constructedURL.Replace("''", "%27%27");
                responseValue = OpenExcel(constructedURL);
            }
            catch (Exception ex)
            {
                returnMessage = " Unable to locate excel workbook: " + constructedURL;
                exceptionService.Handle(LogCategory.CriticalError, ex);
                return returnMessage;
            }

            // Create a new XML document to parse
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(responseValue);
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(doc.NameTable);
            nsMgr.AddNamespace("x", "http://schemas.microsoft.com/office/2008/07/excelservices/rest");

            // Loop thru each row and parse for data
            foreach (XmlNode row in doc.SelectNodes("//x:row", nsMgr))
            {
                currentColumn = 0;
                dataCaptured = false;
                strPAApproved = string.Empty;
                strPlatesCompleted = string.Empty;
                strProofApproved = string.Empty;
                strMaterialNumber = string.Empty;
                
                // Loop thru each column
                foreach (XmlNode col in row.SelectNodes("x:c", nsMgr))
                {
                    currentColumn++;
                    // Try to read data for the column
                    if (col.SelectSingleNode("x:fv", nsMgr) == null)
                        value = string.Empty;
                    else
                        value = col.SelectSingleNode("x:fv", nsMgr).InnerXml;

                    // Try to locate the columns
                    if ((colPAApproved == 0) ||
                        (colProofApproved == 0) ||
                        (colPlatesCompleted == 0) ||
                        (colMaterialNumber == 0)) 
                        LocateSGSPrepPlates(value, currentColumn);

                    // If the current column matches one of our headers then get the data
                    if ((currentColumn == colPAApproved) ||
                        (currentColumn == colProofApproved) ||
                        (currentColumn == colPlatesCompleted) ||
                        (currentColumn == colMaterialNumber))
                    {
                        if (currentColumn == colPAApproved)
                        {
                            strPAApproved = value;
                            dataCaptured = true;
                        }
                        if (currentColumn == colProofApproved)
                        {
                            strProofApproved = value;
                            dataCaptured = true;
                        }
                        if (currentColumn == colPlatesCompleted)
                        {
                            strPlatesCompleted = value;
                            dataCaptured = true;
                        }
                        if (currentColumn == colMaterialNumber)
                        {
                            strMaterialNumber = value;
                            dataCaptured = true;
                        }
                    }
                }

                // A row has been completed, try to update
                if (dataCaptured)
                    UpdateSGSGraphicsProgress(FORM_SGS_PrepPlates);
            }

            // Log any columns that were not found
            if (colPAApproved == 0)
                exceptionService.HandleGraphicsImport(GraphicsLogCategory.MissingColumn, COLUMN_PrepPlates_PAApproved, FORM_SGS_PrepPlates, "Header Not Found");
            if (colProofApproved == 0)
                exceptionService.HandleGraphicsImport(GraphicsLogCategory.MissingColumn, COLUMN_PrepPlates_ProofApproved, FORM_SGS_PrepPlates, "Header Not Found");
            if (colPlatesCompleted == 0)
                exceptionService.HandleGraphicsImport(GraphicsLogCategory.MissingColumn, COLUMN_PrepPlates_PlatesCompleted, FORM_SGS_PrepPlates, "Header Not Found");
            if (colMaterialNumber == 0)
                exceptionService.HandleGraphicsImport(GraphicsLogCategory.MissingColumn, COLUMN_PrepPlates_MaterialNumber, FORM_SGS_PrepPlates, "Header Not Found");

            return returnMessage;
        }

        private void LocateSGSPrepPlates(string columnHeader, int current)
        {
            string header = Regex.Replace(value, @"\n| ", "");
            header = header.ToLower().Trim();
            if (string.Equals(header, COLUMN_PrepPlates_PAApproved.ToLower()))
            {
                colPAApproved = current;
            }
            if (string.Equals(header, COLUMN_PrepPlates_ProofApproved.ToLower()))
            {
                colProofApproved = current;
            }
            if (string.Equals(header, COLUMN_PrepPlates_PlatesCompleted.ToLower()))
            {
                colPlatesCompleted = current;
            }
            if (string.Equals(header, COLUMN_PrepPlates_MaterialNumber.ToLower()))
            {
                colMaterialNumber = current;
            }
        }

        #endregion

        #region SGS Corrugate
        public string ImportSGSCorrugateFile(string filename)
        {
            Initialize();
            string constructedURL = string.Empty;
            try
            {
                // Open the XLS with the REST API
                constructedURL = SPContext.Current.Web.Url + URL_Root + filename + URL_ModelRanges + URL_FCCCorrugate + URL_Format;
                constructedURL = HttpUtility.UrlPathEncode(constructedURL);
                // Replace ' with %27 because UrlPathEncode doesn't change them
                constructedURL = constructedURL.Replace("'''", "'%27%27");
                constructedURL = constructedURL.Replace("''", "%27%27");
                responseValue = OpenExcel(constructedURL);
            }
            catch (Exception ex)
            {
                returnMessage = " Unable to locate excel workbook: " + constructedURL;
                exceptionService.Handle(LogCategory.CriticalError, ex);
                return returnMessage;
            }

            // Create a new XML document to parse
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(responseValue);
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(doc.NameTable);
            nsMgr.AddNamespace("x", "http://schemas.microsoft.com/office/2008/07/excelservices/rest");

            // Loop thru each row and parse for data
            foreach (XmlNode row in doc.SelectNodes("//x:row", nsMgr))
            {
                currentColumn = 0;
                dataCaptured = false;
                strPAApproved = string.Empty;
                strPlatesCompleted = string.Empty;
                strProofApproved = string.Empty;
                strMaterialNumber = string.Empty;

                // Loop thru each column
                foreach (XmlNode col in row.SelectNodes("x:c", nsMgr))
                {
                    currentColumn++;
                    // Try to read data for the column
                    if (col.SelectSingleNode("x:fv", nsMgr) == null)
                        value = string.Empty;
                    else
                        value = col.SelectSingleNode("x:fv", nsMgr).InnerXml;

                    // Try to locate the columns
                    if ((colPAApproved == 0) ||
                        (colProofApproved == 0) ||
                        (colPlatesCompleted == 0) ||
                        (colMaterialNumber == 0))
                        LocateSGSCorrugateHeaders(value, currentColumn);

                    // If the current column matches one of our headers then get the data
                    if ((currentColumn == colPAApproved) ||
                        (currentColumn == colProofApproved) ||
                        (currentColumn == colPlatesCompleted) ||
                        (currentColumn == colMaterialNumber))
                    {
                        if (currentColumn == colPAApproved)
                        {
                            strPAApproved = value;
                            dataCaptured = true;
                        }
                        if (currentColumn == colProofApproved)
                        {
                            strProofApproved = value;
                            dataCaptured = true;
                        }
                        if (currentColumn == colPlatesCompleted)
                        {
                            strPlatesCompleted = value;
                            dataCaptured = true;
                        }
                        if (currentColumn == colMaterialNumber)
                        {
                            strMaterialNumber = value;
                            dataCaptured = true;
                        }
                    }
                }

                // A row has been completed, try to update
                if (dataCaptured)
                    UpdateSGSGraphicsProgress(FORM_SGS_FCCCorrugate);
            }

            // Log any columns that were not found
            if (colPAApproved == 0)
                exceptionService.HandleGraphicsImport(GraphicsLogCategory.MissingColumn, COLUMN_Corrugate_PAApproved, FORM_SGS_FCCCorrugate, "Header Not Found");
            if (colProofApproved == 0)
                exceptionService.HandleGraphicsImport(GraphicsLogCategory.MissingColumn, COLUMN_Corrugate_ProofApproved, FORM_SGS_FCCCorrugate, "Header Not Found");
            if (colPlatesCompleted == 0)
                exceptionService.HandleGraphicsImport(GraphicsLogCategory.MissingColumn, COLUMN_Corrugate_PlatesCompleted, FORM_SGS_FCCCorrugate, "Header Not Found");
            if (colMaterialNumber == 0)
                exceptionService.HandleGraphicsImport(GraphicsLogCategory.MissingColumn, COLUMN_Corrugate_MaterialNumber, FORM_SGS_FCCCorrugate, "Header Not Found");

            return returnMessage;
        }

        private void LocateSGSCorrugateHeaders(string columnHeader, int current)
        {
            header = Regex.Replace(value, @"\n| ", "");
            header = header.ToLower().Trim();

            if (string.Equals(header, COLUMN_Corrugate_PAApproved.ToLower()))
            {
                colPAApproved = current;
            }
            if (string.Equals(header, COLUMN_Corrugate_ProofApproved.ToLower()))
            {
                colProofApproved = current;
            }
            if (string.Equals(header, COLUMN_Corrugate_PlatesCompleted.ToLower()))
            {
                colPlatesCompleted = current;
            }
            if (string.Equals(header, COLUMN_Corrugate_MaterialNumber.ToLower()))
            {
                colMaterialNumber = current;
            }
        }

        #endregion

        #region Private Methods
        private string OpenExcel(string url)
        {
            StringBuilder sb = new StringBuilder();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.UseDefaultCredentials = true;
            request.Method = "GET";
            request.ContentLength = 0;
            request.ContentType = "text/xml";
            request.PreAuthenticate = true;
            //request.Credentials = System.Net.CredentialCache.DefaultCredentials;
            request.Credentials = new NetworkCredential("SPTestOBM", "Candy841");
            //SPContext.Current.Web.CurrentUser

            //WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            //using (identity.Impersonate())
            //{
            //    request.Credentials = CredentialCache.DefaultNetworkCredentials;
            //    request.UseDefaultCredentials = true;
            //}
            var responseValue = string.Empty;

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    string message = string.Format("GET Failed. Received HTTP {0}", response.StatusCode);
                    throw new ApplicationException(message);
                }

                // Grab the response
                using (var responseStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        responseValue = reader.ReadToEnd();
                    }
                }

                return responseValue;
            }
        }

        private void Initialize()
        {
            returnMessage = "Success";
            colPAApproved = 0;
            colProofApproved = 0;
            colPlatesCompleted = 0;
            colMaterialNumber = 0;
            currentColumn = 0;
            value = string.Empty;
            dataCaptured = false;
            error = string.Empty;
            responseValue = string.Empty;
            header = string.Empty;
            strPAApproved = string.Empty;
            strProofApproved = string.Empty;
            strPlatesCompleted = string.Empty;
            strMaterialNumber = string.Empty;
        }

        private void UpdateSGSGraphicsProgress(string form)
        {
            try
            {
                error = string.Empty;
                strPAApproved = ParseDate(strPAApproved, "PA Approved");
                strProofApproved = ParseDate(strProofApproved, "Proof Approved");
                strPlatesCompleted = ParseDate(strPlatesCompleted, "Plates Completed");

                // Material Number can be a semicolon seperated list of multiple material numbers
                string[] matNums = strMaterialNumber.Split(';');
                foreach (string matNum in matNums)
                {
                    int materialNumber = ParseNumber(matNum, "Material Number");

                    // Try to update row
                    if (materialNumber != 0)
                        packagingItemService.UpdateGraphicsProgressReportItem(form, materialNumber, "", strPAApproved, strProofApproved, strPlatesCompleted);
                }

                if (!string.IsNullOrEmpty(error))
                {
                    if (!string.IsNullOrEmpty(strMaterialNumber) && !string.Equals(strMaterialNumber, "Material #"))
                    {
                        error = "Material#: " + strMaterialNumber + " - " + error;
                        exceptionService.HandleGraphicsImport(GraphicsLogCategory.DataError, error, form, strMaterialNumber);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private string ParseDate(string date, string field)
        {
            try
            {
                if (!string.IsNullOrEmpty(date))
                    date = Convert.ToDateTime(date).ToString();
            }
            catch (Exception ex)
            {
                error = error + field + " contains an invalid date (" + date + "); ";
                date = string.Empty;
            }

            return date;
        }

        private int ParseNumber(string number, string field)
        {
            int num = 0;
            try
            {
                num = Convert.ToInt32(number);
            }
            catch (Exception ex)
            {
                num = 0;
                error = error + field + " contains an invalid number (" + number + "); ";
            }

            return num;
        }
        #endregion
    }
}
