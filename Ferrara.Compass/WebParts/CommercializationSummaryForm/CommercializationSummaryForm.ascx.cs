using System;
using System.ComponentModel;
using Microsoft.Practices.Unity;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Constants;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.WebControls;
using Ferrara.Compass.Classes;
using Ferrara.Compass.Abstractions.Models;
using System.Linq;
using System.Text.RegularExpressions;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.WebParts.CommercializationSummaryForm
{
    [ToolboxItemAttribute(false)]
    public partial class CommercializationSummaryForm : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

        private ICommercializationService commercializationService;
        private IUtilityService utilityService;
        private IBOMSetupService bomSetupService;
        private IPackagingItemService packagingItemService;
        private IItemProposalService itemProposalService;
        private IMixesService mixesService;
        private IShipperFinishedGoodService shipperFinishedGoodService;
        private IProjectHeaderService headerService;
        private IExceptionService exceptionService;

        private string ProjectNumber
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo];
                return string.Empty;
            }
        }

        public CommercializationSummaryForm()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            headerService = DependencyResolution.DependencyMapper.Container.Resolve<IProjectHeaderService>();
            commercializationService = DependencyResolution.DependencyMapper.Container.Resolve<ICommercializationService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            itemProposalService = DependencyResolution.DependencyMapper.Container.Resolve<IItemProposalService>();
            mixesService = DependencyResolution.DependencyMapper.Container.Resolve<IMixesService>();
            shipperFinishedGoodService = DependencyResolution.DependencyMapper.Container.Resolve<IShipperFinishedGoodService>();
            bomSetupService = DependencyResolution.DependencyMapper.Container.Resolve<IBOMSetupService>();
            packagingItemService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ItemProposalItem itemProposal;
            List<MixesItem> mixes;
            List<ShipperFinishedGoodItem> shippers;
            int iItemId = Utilities.GetItemIdFromProjectNumber(ProjectNumber);
            if (Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_MasterData))
            {
                List<KeyValuePair<string, string>> masterData = commercializationService.GetMasterDataItem(iItemId);
                Table masterTable = commercialItems(masterData);
                commercializationPanel.Controls.Add(masterTable);
            }
            ProjectHeaderItem headerItem = headerService.GetProjectHeaderItem(iItemId);
            this.lblTitle.Text = SetProjectTitle(headerItem);
            this.lblProjectType.Text = "Project Type: " + headerItem.ProjectType;
            if (headerItem.ProjectTypeSubCategory == "NA")
            {
                divProjectTypeSubCategory.Visible = false;
            }
            else
            {
                this.lblProjectTypeSubCategory.Text = "Project Type SubCategory: " + headerItem.ProjectTypeSubCategory;
            }

            HyperLink projectStatus = new HyperLink();
            projectStatus.NavigateUrl = "/Pages/ProjectStatus.aspx?ProjectNo=" + ProjectNumber;
            projectStatus.Text = "Project Status Report";
            projectStatusLink.Controls.Add(projectStatus);
            if (Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_PackagingEngineer))
            {
                HyperLink bomPE2 = new HyperLink();
                bomPE2.NavigateUrl = "/Pages/BOMSetupPE2.aspx?ProjectNo=" + ProjectNumber;
                bomPE2.Text = "BOM-PE2";
                BOMPE2Link.Controls.Add(bomPE2);
            }
            itemProposal = itemProposalService.GetItemProposalItem(iItemId);
            hddRetailSellingUnitsPerBaseUOM.Value = itemProposal.RetailSellingUnitsBaseUOM == -9999 ? "0" : itemProposal.RetailSellingUnitsBaseUOM.ToString();
            if (itemProposal.MaterialGroup4ProductForm == "MIXES (MIX)")
            {
                mixes = mixesService.GetMixesItems(iItemId);
                if (mixes.Count > 0)
                {
                    rpMixesSummary.DataSource = mixes;
                    rpMixesSummary.DataBind();
                    MixesPanel.Visible = true;
                }
            }
            if (itemProposal.MaterialGroup5PackType.ToLower() == "shipper (shp)" || itemProposal.MaterialGroup5PackType.ToLower() == "shippers (shp)")
            {
                shippers = shipperFinishedGoodService.GetShipperFinishedGoodItems(iItemId);
                if (shippers.Count > 0)
                {
                    rpShipperSummary.DataSource = shippers;
                    rpShipperSummary.DataBind();
                    ShippersPanel.Visible = true;
                }
            }
            List<KeyValuePair<string, string>> tableValues = commercializationService.GetCommercializationItem(iItemId);
            Table comTable = commercialItems(tableValues);
            commercializationPanel.Controls.Add(comTable);

            List<KeyValuePair<string, string>> packComp = commercializationService.GetPackagingItem(iItemId, itemProposal.PLMFlag, itemProposal.ProjectType);

            if (packComp.Count > 0)
            {
                int endFG = packComp.IndexOf(new KeyValuePair<string, string>("endFG", "endFG"));
                if (endFG > 0)
                {
                    int FGindex = packComp.IndexOf(new KeyValuePair<string, string>("new", ""));
                    int count = endFG - FGindex + 1;
                    List<KeyValuePair<string, string>> finishedGood = packComp.GetRange(FGindex - 1, count);
                    Table packTable = packagingItems(finishedGood, "");
                    packTable.CssClass = "packTable";
                    commercializationPanel.Controls.Add(packTable);
                }
            }
            int transferSemiCount = (from key in packComp where key.Key == "newTS" select key.Key).Count();
            if (transferSemiCount > 0)
            {
                int endTS = packComp.LastIndexOf(new KeyValuePair<string, string>("endTS", "endTS"));
                if (endTS != -1)
                {
                    int TSindex = packComp.IndexOf(new KeyValuePair<string, string>("newTS", ""));
                    int TScount = endTS - TSindex;
                    List<KeyValuePair<string, string>> transferSemi = packComp.GetRange(TSindex, TScount);
                    Table packTable = packagingItems(transferSemi, "TS");
                    packTable.CssClass = "packTable";
                    commercializationPanel.Controls.Add(packTable);
                }
            }

            Table attTable = new Table();
            List<FileAttribute> files = utilityService.GetUploadedFiles(ProjectNumber);

            foreach (var file in files)
            {
                int PackagingComponentItemId = file.PackagingComponentItemId;
                var DocType = file.DocType;

                if ((DocType == GlobalConstants.DOCTYPE_Dieline || DocType == GlobalConstants.DOCTYPE_BEQRCodeEPSFile || DocType == GlobalConstants.DOCTYPE_ApprovedGraphicsAsset) && PackagingComponentItemId != 0)
                {
                    try
                    {
                        PackagingItem packItem = packagingItemService.GetPackagingItemByPackagingId(file.PackagingComponentItemId);
                        DocType = DocType + " : " + packItem.MaterialNumber;
                    }
                    catch (Exception ex)
                    {
                        exceptionService.Handle(LogCategory.CriticalError, ex.Message, GlobalConstants.PAGE_CommercializationItemSummary, "GetPackagingItemByPackagingId", file.PackagingComponentItemId.ToString());

                    }
                }
                file.DocType = DocType;
            }

            #region Dieline Attachment(s) :
            List<FileAttribute> PalletSpecfiles = new List<FileAttribute>();
            var packMeasItems = bomSetupService.GetPackMeasurementsItems(iItemId);
            foreach (var packMeasItem in packMeasItems)
            {
                if (!string.IsNullOrEmpty(packMeasItem.PalletSpecLink))
                {
                    FileAttribute PalletSpecfile = new FileAttribute();
                    string title = string.IsNullOrEmpty(packMeasItem.PackagingComponent) ? "Finished Good" : packMeasItem.PackagingComponent;
                    title = title + ": " + packMeasItem.MaterialNumber + ": Pallet Pattern";

                    PalletSpecfile.DocType = title;
                    PalletSpecfile.FileUrl = packMeasItem.PalletSpecLink;
                    PalletSpecfile.FileName = title;
                    PalletSpecfiles.Add(PalletSpecfile);
                }
            }
            #endregion

            #region Hyperlink to Dieline in PLM 
            List<FileAttribute> DielineInPLMfiles = new List<FileAttribute>();
            var PackingItems = packagingItemService.GetGraphicsPackagingItemsForProject(iItemId);
            foreach (var PackingItem in PackingItems)
            {
                if (!string.IsNullOrEmpty(PackingItem.DielineURL))
                {
                    FileAttribute DielineInPLMfile = new FileAttribute();
                    DielineInPLMfile.DocType = generateLinkName(PackingItem.ParentType, PackingItem.MaterialNumber);
                    DielineInPLMfile.FileUrl = PackingItem.DielineURL;
                    DielineInPLMfile.FileName = generateLinkName(PackingItem.ParentType, PackingItem.MaterialNumber);
                    DielineInPLMfiles.Add(DielineInPLMfile);
                }
            }
            #endregion

            if (files.Count > 0)
            {
                TableRow newHeaderRow = new TableRow();
                TableCell newHeaderCell = new TableCell();
                newHeaderCell.Text = "<h2>Attachments</h2><div class='CompassSeparator'>&nbsp;</div>";
                newHeaderCell.CssClass = "ComHeader";
                newHeaderCell.ColumnSpan = 4;
                newHeaderRow.Cells.Add(newHeaderCell);
                attTable.Rows.Add(newHeaderRow);
                TableRow newColHeaderRow = new TableRow();
                TableCell newFTHeaderCell = new TableCell();
                TableCell newAHeaderCell = new TableCell();
                newFTHeaderCell.Text = "<strong>File Type</strong>";
                newFTHeaderCell.CssClass = "ColHeader";
                newColHeaderRow.Cells.Add(newFTHeaderCell);
                newAHeaderCell.Text = "<strong>Attachment</strong>";
                newAHeaderCell.CssClass = "ColHeader";
                newColHeaderRow.Cells.Add(newAHeaderCell);
                attTable.Rows.Add(newColHeaderRow);
                foreach (FileAttribute attributes in files)
                {
                    TableRow newRow = new TableRow();
                    newRow.CssClass = "attachments";
                    TableCell newCell = new TableCell();
                    newCell.Text = attributes.DocType;
                    newRow.Cells.Add(newCell);
                    TableCell newCell2 = new TableCell();
                    newCell2.Text = "<a href='" + attributes.FileUrl + "'>" + attributes.FileName + "</a>";
                    newRow.Cells.Add(newCell2);
                    attTable.Rows.Add(newRow);
                }
            }

            List<FileAttribute> PLMfiles = new List<FileAttribute>();
            PLMfiles.AddRange(PalletSpecfiles);
            PLMfiles.AddRange(DielineInPLMfiles);
            foreach (FileAttribute attributes in PLMfiles)
            {
                TableRow newRow = new TableRow();
                newRow.CssClass = "attachments";
                TableCell newCell = new TableCell();
                newCell.Text = attributes.DocType;
                newRow.Cells.Add(newCell);
                TableCell newCell2 = new TableCell();
                newCell2.Text = "<a href='" + attributes.FileUrl + "'>" + attributes.FileName + "</a>";
                newRow.Cells.Add(newCell2);
                attTable.Rows.Add(newRow);
            }

            commercializationPanel.Controls.Add(attTable);
        }

        private string generateLinkName(string parentName, string matNumber)
        {
            if (parentName == "")
            {
                parentName = "Finished Good";
            }
            if (matNumber == "")
            {
                matNumber = "XXXXX";
            }
            return parentName + ": " + matNumber + ": Dieline Link";
        }
        //private Table
        public Table packagingItems(List<KeyValuePair<string, string>> newTrial, string breaker)
        {

            Table packagingTable = new Table();
            string newBreaker = "new" + breaker;
            int packCompCount = (from key in newTrial where key.Key == newBreaker select key.Key).Count();

            int packTrial1st = newTrial.IndexOf(new KeyValuePair<string, string>("Pack Trial", "subHead"));
            Table subDetailsPackTable = new Table();
            //List<KeyValuePair<string, string>> newTrial = packComp;//.GetRange(packTrial1st, 26);
            int subHeadindex = 0;
            int packageCount = 0;
            Boolean material = false;
            foreach (KeyValuePair<string, string> pair in newTrial.Where(r => r.Key != newBreaker))
            {

                if (pair.Key == "Packaging Type")
                {
                    subHeadindex = 0;
                    if (breaker == "TS")
                    {
                        packageCount++;
                        //packageCount = 0;
                    }
                    else
                    {
                        packageCount++;
                    }
                    material = false;
                }
                if (pair.Key == "Pack Trial" || pair.Key == "Finished Good Specifications" || pair.Key == "Transfer Semi Specifications" || pair.Key == "Purchased Candy Semi Specifications")
                {
                    packageCount = 0;
                    material = true;
                }
                TableRow newRow = new TableRow();
                TableCell newCell = new TableCell();
                TableCell newCell2 = new TableCell();
                int insertLoc = subDetailsPackTable.Rows.Count - 1;
                if (packageCount % 2 != 0)
                {
                    newRow.CssClass = "oddRow";
                }
                if (material == true)
                {
                    newRow.CssClass = "matRow";
                }
                if (pair.Value == "subHead")
                {
                    newCell.Text = "<H3>" + pair.Key + "</H3>";
                    newCell.ColumnSpan = 4;
                    newRow.Cells.Add(newCell);
                    subDetailsPackTable.Rows.Add(newRow);
                    subHeadindex = 0;
                    if (subHeadindex == 0 && material == true && (subDetailsPackTable.Rows[insertLoc].Cells.Count != 4))
                    {
                        TableCell newBlankCell = new TableCell();
                        newBlankCell.ColumnSpan = 2;
                        subDetailsPackTable.Rows[insertLoc].Cells.Add(newBlankCell);
                    }
                }
                else if (pair.Value == "header")
                {
                    newCell.Text = "<H2>" + pair.Key + "</H2><div class='CompassSeparator'>&nbsp;</div>";
                    newCell.ColumnSpan = 4;
                    newRow.Cells.Add(newCell);
                    newRow.CssClass = "";
                    subDetailsPackTable.Rows.Add(newRow);
                    subHeadindex = 0;
                }
                else if (pair.Key == "endTS")
                {
                    continue;
                }
                else
                {
                    newCell.Text = string.IsNullOrEmpty(pair.Key) ? "" : " <strong> " + pair.Key + " :</strong> ";
                    newCell2.Text = pair.Value;


                    if (insertLoc < 0 || subHeadindex == 0 || subHeadindex % 2 == 0)
                    {

                        newRow.Cells.Add(newCell);
                        newRow.Cells.Add(newCell2);
                        subDetailsPackTable.Rows.Add(newRow);

                    }
                    else
                    {
                        subDetailsPackTable.Rows[insertLoc].Cells.Add(newCell);
                        subDetailsPackTable.Rows[insertLoc].Cells.Add(newCell2);
                    }
                    subHeadindex++;
                }
            }
            TableRow packDetailsLastLastRow = new TableRow();
            TableCell packDetailsLastLastCell = new TableCell();
            packDetailsLastLastCell.ColumnSpan = 4;
            packDetailsLastLastCell.Controls.Add(subDetailsPackTable);
            packDetailsLastLastRow.Cells.Add(packDetailsLastLastCell);
            packagingTable.Rows.Add(packDetailsLastLastRow);


            return packagingTable;
        }
        public Table commercialItems(List<KeyValuePair<string, string>> tableValues)
        {
            Table comTable = new Table();
            int index = 0;
            int sectionCount = 0;
            int headerIndex = 0;
            bool qa = false;
            int qaItemCount = 0;
            foreach (KeyValuePair<string, string> pair in tableValues)
            {
                TableRow newRow = new TableRow();
                TableCell newCell = new TableCell();
                TableCell newCell2 = new TableCell();
                if (pair.Value == "header")
                {
                    newCell.ColumnSpan = 4;
                    newCell.CssClass = "ComHeader";
                    newCell.Text = "<h2>" + pair.Key + "</h2><div class='CompassSeparator'>&nbsp;</div>";
                    newRow.Cells.Add(newCell);
                    comTable.Rows.Add(newRow);
                    sectionCount = 0;
                    headerIndex = tableValues.IndexOf(pair);
                    if (pair.Key == "InTech Regulatory - Candy Semi" || pair.Key == "InTech Regulatory - Purchased Candy Semi")
                    {
                        qa = true;
                    }
                    else
                    {
                        qa = false;
                    }
                    continue;
                }
                else if (pair.Value == "subHead")
                {
                    newCell.Text = "<H3>" + pair.Key + "</H3>";
                    newCell.ColumnSpan = 4;
                    newRow.Cells.Add(newCell);
                    comTable.Rows.Add(newRow);
                    sectionCount = 0;
                    headerIndex = tableValues.IndexOf(pair);
                    continue;
                }
                else if (pair.Value == "endTS")
                {
                    newCell.Text = "<div class='CompassSeparator'>&nbsp;</div>";
                    newCell.ColumnSpan = 4;
                    newRow.Cells.Add(newCell);
                    comTable.Rows.Add(newRow);
                    sectionCount = 0;
                    headerIndex = tableValues.IndexOf(pair);
                    continue;
                }
                else if (pair.Key == "Item Description" || pair.Key == "Item Concept" || pair.Key == "Network Move?")
                {
                    newCell.Text = "<strong>" + pair.Key + ":</strong>";
                    newCell.ColumnSpan = 1;
                    newCell2.ColumnSpan = 3;
                    newCell2.Text = pair.Value;
                    newRow.Cells.Add(newCell);
                    newRow.Cells.Add(newCell2);
                    comTable.Rows.Add(newRow);
                    sectionCount = 0;
                    if (((comTable.Rows.Count) % 2 != 0))
                    {
                        comTable.Rows[comTable.Rows.Count - 1].CssClass = "bgColor";
                    }
                    continue;
                }
                else if (qa && pair.Key == "new")
                {
                    qaItemCount++;
                    sectionCount = 0;
                    continue;
                }

                sectionCount++;
                index = tableValues.IndexOf(pair);
                if (pair.Key != "")
                {
                    newCell.Text = "<strong>" + pair.Key + ":</strong>";
                }
                else
                {
                    newCell.Text = "";
                }
                newCell2.Text = pair.Value;
                int insertLoc = comTable.Rows.Count - 1;
                if (((sectionCount) % 2 != 0) || comTable.Rows[insertLoc].Cells[0].CssClass == "ComHeader")
                {
                    newRow.Cells.Add(newCell);
                    newRow.Cells.Add(newCell2);
                    comTable.Rows.Add(newRow);
                }
                else
                {
                    comTable.Rows[insertLoc].Cells.Add(newCell);
                    comTable.Rows[insertLoc].Cells.Add(newCell2);
                }
                if (((comTable.Rows.Count) % 2 != 0) && !qa)
                {
                    comTable.Rows[comTable.Rows.Count - 1].CssClass = "bgColor";
                }
                else if (qa && (qaItemCount % 2 != 0))
                {
                    comTable.Rows[comTable.Rows.Count - 1].CssClass = "oddRow";
                }
            }
            return comTable;
        }
        public string SetProjectTitle(ProjectHeaderItem item)
        {
            string title = item.ProjectNumber + " : ";

            if (string.IsNullOrEmpty(item.SAPItemNumber))
                title = title + "XXXXX : ";
            else
                title = title + item.SAPItemNumber + " : ";

            if (string.IsNullOrEmpty(item.SAPDescription))
                title = title + "(Proposed)";
            else
                title = title + item.SAPDescription;

            return title;
        }
    }
}
