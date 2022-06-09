using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Classes;
using Microsoft.Practices.Unity;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ferrara.Compass.ControlTemplates.Ferrara.Compass
{
    public partial class ucConsolidatedFinancialSummary : UserControl
    {
        #region Member Variables
        private IStageGateFinancialServices stageGateFinancialServices;
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private INotificationService notificationService;
        private string webUrl;
        private int iItemId = 0;
        private List<PackagingItem> packagingItems = new List<PackagingItem>();
        private List<ShipperFinishedGoodItem> shipperFGItems = new List<ShipperFinishedGoodItem>();

        private const string _ucConsolidatedFinancialSummary = @"~/_controltemplates/15/Ferrara.Compass/ucConsolidatedFinancialSummary.ascx";
        #endregion

        #region Properties
        public string ProjectNo { get; set; }
        public int StageGateProjectListItemId { get; set; }
        public int GateNo { get; set; }
        public int BriefNo { get; set; }
        public string BriefName { get; set; }
        public List<StageGateConsolidatedFinancialSummaryItem> SummaryItem { get; set; }
        #endregion

        #region OnInit
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            // Page.EnableEventValidation = false;

            stageGateFinancialServices = DependencyResolution.DependencyMapper.Container.Resolve<IStageGateFinancialServices>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
            EnsureScriptManager();
            EnsureUpdatePanelFixups();
        }
        #endregion
        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            webUrl = SPContext.Current.Web.Url;
            LoadAllFinacialSummary();
            LoadFinanceBriefFromPreviousGate();
        }
        #endregion
        #region Data Transfer Methods
        private void LoadAllFinacialSummary()
        {
            this.hdnStageGateProjectListItemId.Value = StageGateProjectListItemId.ToString();
            this.hdnGate.Value = GateNo.ToString();
            this.hdnBriefNumber.Value = BriefNo.ToString();

            var dtFinancialSummaryItems = stageGateFinancialServices.GetStageGateConsolidatedFinancialSummaryItem(StageGateProjectListItemId, GateNo.ToString(), BriefNo.ToString());

            if (dtFinancialSummaryItems != null)
            {
                this.hdnSummaryListItemId.Value = dtFinancialSummaryItems.Id.ToString();
                this.txtBriefSummary.Text = dtFinancialSummaryItems.BriefSummary;
                //this.txtBriefName.Text = dtFinancialSummaryItems.Name;
                this.txtAvgTargetMarginPct.Text = dtFinancialSummaryItems.AverageTargetMargin.ToString();
                Utilities.SetDropDownValue(dtFinancialSummaryItems.DispConsFinInProjBrief, ddlDisplayConsolidatedFinancialsInProjectBrief, this.Page);
                //Volume
                this.txtTotalVolume1.Text = dtFinancialSummaryItems.VolumeTotal1.ToString();
                this.txtTotalVolume2.Text = dtFinancialSummaryItems.VolumeTotal2.ToString();
                this.txtTotalVolume3.Text = dtFinancialSummaryItems.VolumeTotal3.ToString();
                this.txtIncrementalVolume1.Text = dtFinancialSummaryItems.VolumeIncremental1.ToString();
                this.txtIncrementalVolume2.Text = dtFinancialSummaryItems.VolumeIncremental2.ToString();
                this.txtIncrementalVolume3.Text = dtFinancialSummaryItems.VolumeIncremental3.ToString();
                //Gross Sales
                this.txtTotalGrossSales1.Text = dtFinancialSummaryItems.GrossSalesTotal1.ToString();
                this.txtTotalGrossSales2.Text = dtFinancialSummaryItems.GrossSalesTotal2.ToString();
                this.txtTotalGrossSales3.Text = dtFinancialSummaryItems.GrossSalesTotal3.ToString();
                this.txtIncrementalGrossSales1.Text = dtFinancialSummaryItems.GrossSalesIncremental1.ToString();
                this.txtIncrementalGrossSales2.Text = dtFinancialSummaryItems.GrossSalesIncremental2.ToString();
                this.txtIncrementalGrossSales3.Text = dtFinancialSummaryItems.GrossSalesIncremental3.ToString();
                //Net Sales
                this.txtTotalNetSales1.Text = dtFinancialSummaryItems.NetSalesTotal1.ToString();
                this.txtTotalNetSales2.Text = dtFinancialSummaryItems.NetSalesTotal2.ToString();
                this.txtTotalNetSales3.Text = dtFinancialSummaryItems.NetSalesTotal3.ToString();
                this.txtIncrementalNetSales1.Text = dtFinancialSummaryItems.NetSalesIncremental1.ToString();
                this.txtIncrementalNetSales2.Text = dtFinancialSummaryItems.NetSalesIncremental2.ToString();
                this.txtIncrementalNetSales3.Text = dtFinancialSummaryItems.NetSalesIncremental3.ToString();
                //COGS
                this.txtTotalCOGS1.Text = dtFinancialSummaryItems.COGSTotal1.ToString();
                this.txtTotalCOGS2.Text = dtFinancialSummaryItems.COGSTotal2.ToString();
                this.txtTotalCOGS3.Text = dtFinancialSummaryItems.COGSTotal3.ToString();
                this.txtIncrementalCOGS1.Text = dtFinancialSummaryItems.COGSIncremental1.ToString();
                this.txtIncrementalCOGS2.Text = dtFinancialSummaryItems.COGSIncremental2.ToString();
                this.txtIncrementalCOGS3.Text = dtFinancialSummaryItems.COGSIncremental3.ToString();
                //GrossMargin
                this.txtTotalGrossMargin1.Text = dtFinancialSummaryItems.GrossMarginTotal1.ToString();
                this.txtTotalGrossMargin2.Text = dtFinancialSummaryItems.GrossMarginTotal2.ToString();
                this.txtTotalGrossMargin3.Text = dtFinancialSummaryItems.GrossMarginTotal3.ToString();
                this.txtIncrementalGrossMargin1.Text = dtFinancialSummaryItems.GrossMarginIncremental1.ToString();
                this.txtIncrementalGrossMargin2.Text = dtFinancialSummaryItems.GrossMarginIncremental2.ToString();
                this.txtIncrementalGrossMargin3.Text = dtFinancialSummaryItems.GrossMarginIncremental3.ToString();
                //GrossMarginPct
                this.txtTotalGrossMarginPct1.Text = dtFinancialSummaryItems.GrossMarginPctTotal1.ToString();
                this.txtTotalGrossMarginPct2.Text = dtFinancialSummaryItems.GrossMarginPctTotal2.ToString();
                this.txtTotalGrossMarginPct3.Text = dtFinancialSummaryItems.GrossMarginPctTotal3.ToString();
                this.txtIncrementalGrossMarginPct1.Text = dtFinancialSummaryItems.GrossMarginPctIncremental1.ToString();
                this.txtIncrementalGrossMarginPct2.Text = dtFinancialSummaryItems.GrossMarginPctIncremental2.ToString();
                this.txtIncrementalGrossMarginPct3.Text = dtFinancialSummaryItems.GrossMarginPctIncremental3.ToString();

                this.txtNSPerLB1.Text = dtFinancialSummaryItems.NSDollerperLB1.ToString();
                this.txtNSPerLB2.Text = dtFinancialSummaryItems.NSDollerperLB2.ToString();
                this.txtNSPerLB3.Text = dtFinancialSummaryItems.NSDollerperLB3.ToString();
                this.txtCOGSPerLB1.Text = dtFinancialSummaryItems.COGSperLB1.ToString();
                this.txtCOGSPerLB2.Text = dtFinancialSummaryItems.COGSperLB2.ToString();
                this.txtCOGSPerLB3.Text = dtFinancialSummaryItems.COGSperLB3.ToString();
                this.lblAnalysesIncludedInSummary.Text = dtFinancialSummaryItems.Analysesincluded.ToString();
                this.hdnAnalysesIncludedInSummary.Value = dtFinancialSummaryItems.Analysesincluded.ToString();
            }
        }
        private void LoadFinanceBriefFromPreviousGate()
        {
            var FinanceBriefs = new List<StageGateFinancialAnalysisItem>();
            var FinanceBriefListWithoutCurrentBrief = new List<StageGateFinancialAnalysisItem>();
            FinanceBriefs.AddRange(stageGateFinancialServices.GetAllStageGateFinancialAnalysisItems(StageGateProjectListItemId));

            foreach (var FinanceBrief in FinanceBriefs)
            {
                if (!(FinanceBrief.Gate == GateNo.ToString() && FinanceBrief.BriefNumber == BriefNo.ToString()))
                {
                    FinanceBriefListWithoutCurrentBrief.Add(FinanceBrief);
                }
            }

            var UniqueFinanceBriefs = FinanceBriefListWithoutCurrentBrief.GroupBy(o => new { o.Gate, o.BriefNumber }).Select(o => o.FirstOrDefault()).ToList();

            if (UniqueFinanceBriefs.Count > 0)
            {
                LoadFinancialBriefSection.Visible = true;
                lblFinanceBrieffromPreviousGateNoData.Visible = false;
                rptPrevoiusFinancialBriefItem.DataSource = UniqueFinanceBriefs;
                rptPrevoiusFinancialBriefItem.DataBind();
            }
            else
            {
                lblFinanceBrieffromPreviousGateNoData.Visible = true;
                LoadFinancialBriefSection.Visible = false;
            }
        }
        public void saveData(bool submitted = false)
        {

            StageGateConsolidatedFinancialSummaryItem summaryItem = new StageGateConsolidatedFinancialSummaryItem();
            summaryItem.StageGateProjectListItemId = StageGateProjectListItemId;
            summaryItem.Gate = GateNo.ToString();
            summaryItem.BriefNumber = BriefNo.ToString();
            summaryItem.BriefName = BriefName;

            summaryItem.BriefSummary = this.txtBriefSummary.Text;
            summaryItem.Name = this.txtBriefSummaryName.Text;
            summaryItem.AverageTargetMargin = GetDoubleValuefromTextBox(this.txtAvgTargetMarginPct);
            summaryItem.DispConsFinInProjBrief = this.ddlDisplayConsolidatedFinancialsInProjectBrief.SelectedItem.Text;
            //Volume
            summaryItem.VolumeTotal1 = GetDoubleValuefromTextBox(this.txtTotalVolume1);
            summaryItem.VolumeTotal2 = GetDoubleValuefromTextBox(this.txtTotalVolume2);
            summaryItem.VolumeTotal3 = GetDoubleValuefromTextBox(this.txtTotalVolume3);
            summaryItem.VolumeIncremental1 = GetDoubleValuefromTextBox(this.txtIncrementalVolume1);
            summaryItem.VolumeIncremental2 = GetDoubleValuefromTextBox(this.txtIncrementalVolume2);
            summaryItem.VolumeIncremental3 = GetDoubleValuefromTextBox(this.txtIncrementalVolume3);
            //Gross Sales  
            summaryItem.GrossSalesTotal1 = GetDoubleValuefromTextBox(this.txtTotalGrossSales1);
            summaryItem.GrossSalesTotal2 = GetDoubleValuefromTextBox(this.txtTotalGrossSales2);
            summaryItem.GrossSalesTotal3 = GetDoubleValuefromTextBox(this.txtTotalGrossSales3);
            summaryItem.GrossSalesIncremental1 = GetDoubleValuefromTextBox(this.txtIncrementalGrossSales1);
            summaryItem.GrossSalesIncremental2 = GetDoubleValuefromTextBox(this.txtIncrementalGrossSales2);
            summaryItem.GrossSalesIncremental3 = GetDoubleValuefromTextBox(this.txtIncrementalGrossSales3);
            //Net Sales  
            summaryItem.NetSalesTotal1 = GetDoubleValuefromTextBox(this.txtTotalNetSales1);
            summaryItem.NetSalesTotal2 = GetDoubleValuefromTextBox(this.txtTotalNetSales2);
            summaryItem.NetSalesTotal3 = GetDoubleValuefromTextBox(this.txtTotalNetSales3);
            summaryItem.NetSalesIncremental1 = GetDoubleValuefromTextBox(this.txtIncrementalNetSales1);
            summaryItem.NetSalesIncremental2 = GetDoubleValuefromTextBox(this.txtIncrementalNetSales2);
            summaryItem.NetSalesIncremental3 = GetDoubleValuefromTextBox(this.txtIncrementalNetSales3);
            //COGS  
            summaryItem.COGSTotal1 = GetDoubleValuefromTextBox(this.txtTotalCOGS1);
            summaryItem.COGSTotal2 = GetDoubleValuefromTextBox(this.txtTotalCOGS2);
            summaryItem.COGSTotal3 = GetDoubleValuefromTextBox(this.txtTotalCOGS3);
            summaryItem.COGSIncremental1 = GetDoubleValuefromTextBox(this.txtIncrementalCOGS1);
            summaryItem.COGSIncremental2 = GetDoubleValuefromTextBox(this.txtIncrementalCOGS2);
            summaryItem.COGSIncremental3 = GetDoubleValuefromTextBox(this.txtIncrementalCOGS3);
            //GrossMargin  
            summaryItem.GrossMarginTotal1 = GetDoubleValuefromTextBox(this.txtTotalGrossMargin1);
            summaryItem.GrossMarginTotal2 = GetDoubleValuefromTextBox(this.txtTotalGrossMargin2);
            summaryItem.GrossMarginTotal3 = GetDoubleValuefromTextBox(this.txtTotalGrossMargin3);
            summaryItem.GrossMarginIncremental1 = GetDoubleValuefromTextBox(this.txtIncrementalGrossMargin1);
            summaryItem.GrossMarginIncremental2 = GetDoubleValuefromTextBox(this.txtIncrementalGrossMargin2);
            summaryItem.GrossMarginIncremental3 = GetDoubleValuefromTextBox(this.txtIncrementalGrossMargin3);
            //GrossMarginPct  
            summaryItem.GrossMarginPctTotal1 = GetDoubleValuefromTextBox(this.txtTotalGrossMarginPct1);
            summaryItem.GrossMarginPctTotal2 = GetDoubleValuefromTextBox(this.txtTotalGrossMarginPct2);
            summaryItem.GrossMarginPctTotal3 = GetDoubleValuefromTextBox(this.txtTotalGrossMarginPct3);
            summaryItem.GrossMarginPctIncremental1 = GetDoubleValuefromTextBox(this.txtIncrementalGrossMarginPct1);
            summaryItem.GrossMarginPctIncremental2 = GetDoubleValuefromTextBox(this.txtIncrementalGrossMarginPct2);
            summaryItem.GrossMarginPctIncremental3 = GetDoubleValuefromTextBox(this.txtIncrementalGrossMarginPct3);

            summaryItem.NSDollerperLB1 = GetDoubleValuefromTextBox(this.txtNSPerLB1);
            summaryItem.NSDollerperLB2 = GetDoubleValuefromTextBox(this.txtNSPerLB2);
            summaryItem.NSDollerperLB3 = GetDoubleValuefromTextBox(this.txtNSPerLB3);
            summaryItem.COGSperLB1 = GetDoubleValuefromTextBox(this.txtCOGSPerLB1);
            summaryItem.COGSperLB2 = GetDoubleValuefromTextBox(this.txtCOGSPerLB2);
            summaryItem.COGSperLB3 = GetDoubleValuefromTextBox(this.txtCOGSPerLB3);
            summaryItem.Analysesincluded = this.hdnAnalysesIncludedInSummary.Value;


            summaryItem.Id = string.IsNullOrWhiteSpace(hdnSummaryListItemId.Value.Replace(",", "")) ? 0 : Convert.ToInt32(hdnSummaryListItemId.Value.Replace(",", ""));


            if (summaryItem.Id <= 0)
            {
                int newId = stageGateFinancialServices.InsertStageGateConsolidatedFinancialSummaryItem(summaryItem, submitted);
                hdnSummaryListItemId.Value = newId.ToString();
            }
            else
            {
                stageGateFinancialServices.UpdateStageGateConsolidatedFinancialSummaryItem(summaryItem, submitted);
            }
        }
        #endregion
        #region Button Click Events
        protected void btnCopyFinanceBrief_Click(object sender, EventArgs e)
        {
            saveData(false);
            var phFinancialBriefAnalysis = (PlaceHolder)this.Parent.FindControl("phFinancialBriefAnalysis");
            foreach (var ctrl in phFinancialBriefAnalysis.Controls)
            {
                if (ctrl is System.Web.UI.UserControl)
                {
                    var type = (ucFinancialBrief)ctrl;
                    type.saveData(false);
                }
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "DialogLoadFinanceBrieffromPreviousGate();", true);
        }
        protected void btnGetFinanceBrieffromPreviousGate_Click(object sender, EventArgs e)
        {
            bool onechecked = false;
            foreach (RepeaterItem item in rptPrevoiusFinancialBriefItem.Items)
            {
                CheckBox ckbox = (CheckBox)item.FindControl("chkFinanceBrief");

                if (ckbox.Checked)
                {
                    onechecked = true;
                    HiddenField hdnGate = (HiddenField)item.FindControl("hdnGate");
                    HiddenField hdnBriefNumber = (HiddenField)item.FindControl("hdnBriefNumber");

                    //Get the brief 
                    List<StageGateFinancialAnalysisItem> FinanceBriefs = new List<StageGateFinancialAnalysisItem>();
                    FinanceBriefs.AddRange(stageGateFinancialServices.GetAllStageGateFinancialAnalysisItemsByGateAndBriefNumber(StageGateProjectListItemId, hdnGate.Value, hdnBriefNumber.Value));

                    //insert brief
                    foreach (StageGateFinancialAnalysisItem FinanceBrief in FinanceBriefs)
                    {
                        FinanceBrief.Gate = GateNo.ToString();
                        FinanceBrief.BriefNumber = BriefNo.ToString();
                        FinanceBrief.BriefName = BriefName;
                        stageGateFinancialServices.InsertStageGateFinancialAnalysisItem(FinanceBrief, false);
                    }
                }
            }

            if (onechecked)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "DialogStageGateFinanciaBriefsCopiedMessage();", true);
            }
            else
            {
                lblFinanceBrieffromPreviousGateValidationMsg.Text = "Please select atleast one Brief.";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "GetFinanceBrieffromPreviousGateCancel();", true);
            }
        }

        protected void btnRedirect_Click(object sender, EventArgs e)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                   new KeyValuePair<string, string>(GlobalConstants.QUERYSTRING_ProjectNo, ProjectNo),
                   new KeyValuePair<string, string>(GlobalConstants.QUERYSTRING_Gate, GateNo.ToString()),
                   new KeyValuePair<string, string>(GlobalConstants.QUERYSTRING_FinancialBrief, BriefNo.ToString())
                };

            Page.Response.Redirect(Utilities.RedirectPageValue(GlobalConstants.PAGE_StageGateFinancialBrief, parameters), false);

        }
        #endregion
        #region Repeater Methods
        protected void rptPrevoiusFinancialBriefItem_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                StageGateFinancialAnalysisItem stageGateFinancialAnalysisItem = (StageGateFinancialAnalysisItem)e.Item.DataItem;

                TextBox txtBriefName = ((TextBox)e.Item.FindControl("txtBriefName"));

                if (txtBriefName != null)
                {
                    txtBriefName.Text = "Finance Brief G" + stageGateFinancialAnalysisItem.Gate.ToString() + " # " + stageGateFinancialAnalysisItem.BriefNumber.ToString() + ": " + stageGateFinancialAnalysisItem.BriefName;
                }
            }
        }
        protected void rptPrevoiusFinancialBriefItem_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "CheckBoxClick")
            {

                string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
                string SelectedGate = commandArgs[0];
                string SelectedBriefNo = commandArgs[1];
            }
        }
        #endregion
        #region Private Methods
        private static double GetDoubleValuefromTextBox(TextBox textbox, bool RoundOffTwoDecimalPlacess = false)
        {
            double value = 0;
            if (textbox != null)
            {
                string txtValue = textbox.Text.Replace("$", "").Replace("%", "").Replace(",", "");
                value = string.IsNullOrWhiteSpace(txtValue) ? 0 : Convert.ToDouble(txtValue);
            }

            if (RoundOffTwoDecimalPlacess) value = Math.Round(value, 2);

            return value;
        }
        private static double GetDoubleValuefromTextBox(Label label)
        {
            double value = 0;
            if (label != null)
            {
                string txtValue = label.Text.Replace("$", "").Replace("%", "").Replace(",", "");
                value = string.IsNullOrWhiteSpace(label.Text) ? 0 : Convert.ToDouble(label.Text);
            }
            return value;
        }
        private void EnsureScriptManager()
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);

            if (scriptManager == null)
            {
                scriptManager = new ScriptManager();
                scriptManager.EnablePartialRendering = true;


                if (Page.Form != null)
                {
                    Page.Form.Controls.AddAt(0, scriptManager);
                }
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
        }
        #endregion
    }
}
