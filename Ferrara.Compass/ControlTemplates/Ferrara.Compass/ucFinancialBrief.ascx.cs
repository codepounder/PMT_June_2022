using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Classes;
using Ferrara.Compass.Services;
using Microsoft.Practices.Unity;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ferrara.Compass.ControlTemplates.Ferrara.Compass
{
    public partial class ucFinancialBrief : UserControl
    {
        #region Member Variables
        private IStageGateFinancialServices stageGateFinancialServices;
        private const string _ucFinancialBriefPath = @"~/_controltemplates/15/Ferrara.Compass/ucFinancialBrief.ascx";
        private string webUrl;

        #endregion
        #region Properties
        public string ProjectNo { get; set; }
        public int StageGateProjectListItemId { get; set; }
        public int GateNo { get; set; }
        public int BriefNo { get; set; }
        public string BriefName { get; set; }

        #endregion
        #region OnInit
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            stageGateFinancialServices = DependencyResolution.DependencyMapper.Container.Resolve<IStageGateFinancialServices>();
            EnsureScriptManager();
            EnsureUpdatePanelFixups();
        }
        #endregion
        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            webUrl = SPContext.Current.Web.Url;
            LoadAllFinacialAnalysis();
        }
        #endregion
        #region Data Transfer Methods
        private void LoadAllFinacialAnalysis()
        {
            List<StageGateFinancialAnalysisItem> dtFinancialAnalysisItems = new List<StageGateFinancialAnalysisItem>();
            dtFinancialAnalysisItems = stageGateFinancialServices.GetAllStageGateFinancialAnalysisItemsByGateAndBriefNumber(StageGateProjectListItemId, GateNo.ToString(), BriefNo.ToString());

            if (dtFinancialAnalysisItems.Count > 0)
            {
                rptFinancialBriefItem.DataSource = dtFinancialAnalysisItems;
                rptFinancialBriefItem.DataBind();
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "UpdateAllSigns();", true);
        }
        public void saveData(bool Submitted = false)
        {
            PlaceHolder phFinancialBriefAnalysis = (PlaceHolder)this.Parent.FindControl("phFinancialBriefAnalysis");
            foreach (UserControl uc in phFinancialBriefAnalysis.Controls)
            {
                var type = (ucFinancialBrief)uc;
                Repeater repeater = type.rptFinancialBriefItem;
                foreach (RepeaterItem item in repeater.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        StageGateFinancialAnalysisItem analysisItem = new StageGateFinancialAnalysisItem();
                        analysisItem.StageGateProjectListItemId = StageGateProjectListItemId;
                        analysisItem.Gate = GateNo.ToString();
                        analysisItem.BriefNumber = BriefNo.ToString();
                        analysisItem.BriefName = BriefName;
                        analysisItem.AnalysisName = ((TextBox)item.FindControl("txtAnalysisName")).Text;
                        analysisItem.FGNumber = ((TextBox)item.FindControl("txtFGNumber")).Text;
                        analysisItem.CustomerChannel = ((DropDownList)item.FindControl("ddlCustomerChannel")).SelectedItem.Text;
                        analysisItem.BrandSeason = ((DropDownList)item.FindControl("ddlBrandSeason")).SelectedItem.Text;
                        analysisItem.ProductForm = ((DropDownList)item.FindControl("ddlProductForm")).SelectedItem.Text;
                        analysisItem.TargetMarginPct = GetDoubleValuefromTextBox(item, "txtTargetMarginPct");
                        analysisItem.PLsinProjectBrief = ((DropDownList)item.FindControl("ddlDisplayPLInProjectBrief")).SelectedItem.Text;
                        analysisItem.PLinConsolidatedFinancials = ((DropDownList)item.FindControl("ddlIncludePLInConsolidatedFinancials")).SelectedItem.Text;

                        analysisItem.VolumeTotal1 = GetDoubleValuefromTextBox(item, "txtTotalVolume1");
                        analysisItem.VolumeIncremental1 = GetDoubleValuefromTextBox(item, "txtIncrementalVolume1");
                        analysisItem.GrossSalesTotal1 = GetDoubleValuefromTextBox(item, "txtTotalGrossSales1");
                        analysisItem.GrossSalesIncremental1 = GetDoubleValuefromTextBox(item, "txtIncrementalGrossSales1");
                        analysisItem.TradeRateTotal1 = GetDoubleValuefromTextBox(item, "txtTotalTradeRate1");
                        analysisItem.TradeRateIncremental1 = GetDoubleValuefromTextBox(item, "txtIncrementalTradeRate1");
                        analysisItem.OGTNTotal1 = GetDoubleValuefromTextBox(item, "txtTotalOGTN1");
                        analysisItem.OGTNIncremental1 = GetDoubleValuefromTextBox(item, "txtIncrementalOGTN1");
                        analysisItem.NetSalesTotal1 = GetDoubleValuefromTextBox(item, "txtTotalNetSales1");
                        analysisItem.NetSalesIncremental1 = GetDoubleValuefromTextBox(item, "txtIncrementalNetSales1");
                        analysisItem.COGSTotal1 = GetDoubleValuefromTextBox(item, "txtTotalCOGS1");
                        analysisItem.COGSIncremental1 = GetDoubleValuefromTextBox(item, "txtIncrementalCOGS1");
                        analysisItem.GrossMarginTotal1 = GetDoubleValuefromTextBox(item, "txtTotalGrossMargin1");
                        analysisItem.GrossMarginIncremental1 = GetDoubleValuefromTextBox(item, "txtIncrementalGrossMargin1");
                        analysisItem.GrossMarginPctTotal1 = GetDoubleValuefromTextBox(item, "txtTotalGrossMarginPct1");
                        analysisItem.GrossMarginPctIncremental1 = GetDoubleValuefromTextBox(item, "txtIncrementalGrossMarginPct1");

                        analysisItem.VolumeTotal2 = GetDoubleValuefromTextBox(item, "txtTotalVolume2");
                        analysisItem.VolumeIncremental2 = GetDoubleValuefromTextBox(item, "txtIncrementalVolume2");
                        analysisItem.GrossSalesTotal2 = GetDoubleValuefromTextBox(item, "txtTotalGrossSales2");
                        analysisItem.GrossSalesIncremental2 = GetDoubleValuefromTextBox(item, "txtIncrementalGrossSales2");
                        analysisItem.TradeRateTotal2 = GetDoubleValuefromTextBox(item, "txtTotalTradeRate2");
                        analysisItem.TradeRateIncremental2 = GetDoubleValuefromTextBox(item, "txtIncrementalTradeRate2");
                        analysisItem.OGTNTotal2 = GetDoubleValuefromTextBox(item, "txtTotalOGTN2");
                        analysisItem.OGTNIncremental2 = GetDoubleValuefromTextBox(item, "txtIncrementalOGTN2");
                        analysisItem.NetSalesTotal2 = GetDoubleValuefromTextBox(item, "txtTotalNetSales2");
                        analysisItem.NetSalesIncremental2 = GetDoubleValuefromTextBox(item, "txtIncrementalNetSales2");
                        analysisItem.COGSTotal2 = GetDoubleValuefromTextBox(item, "txtTotalCOGS2");
                        analysisItem.COGSIncremental2 = GetDoubleValuefromTextBox(item, "txtIncrementalCOGS2");
                        analysisItem.GrossMarginTotal2 = GetDoubleValuefromTextBox(item, "txtTotalGrossMargin2");
                        analysisItem.GrossMarginIncremental2 = GetDoubleValuefromTextBox(item, "txtIncrementalGrossMargin2");
                        analysisItem.GrossMarginPctTotal2 = GetDoubleValuefromTextBox(item, "txtTotalGrossMarginPct2");
                        analysisItem.GrossMarginPctIncremental2 = GetDoubleValuefromTextBox(item, "txtIncrementalGrossMarginPct2");

                        analysisItem.VolumeTotal3 = GetDoubleValuefromTextBox(item, "txtTotalVolume3");
                        analysisItem.VolumeIncremental3 = GetDoubleValuefromTextBox(item, "txtIncrementalVolume3");
                        analysisItem.GrossSalesTotal3 = GetDoubleValuefromTextBox(item, "txtTotalGrossSales3");
                        analysisItem.GrossSalesIncremental3 = GetDoubleValuefromTextBox(item, "txtIncrementalGrossSales3");
                        analysisItem.TradeRateTotal3 = GetDoubleValuefromTextBox(item, "txtTotalTradeRate3");
                        analysisItem.TradeRateIncremental3 = GetDoubleValuefromTextBox(item, "txtIncrementalTradeRate3");
                        analysisItem.OGTNTotal3 = GetDoubleValuefromTextBox(item, "txtTotalOGTN3");
                        analysisItem.OGTNIncremental3 = GetDoubleValuefromTextBox(item, "txtIncrementalOGTN3");
                        analysisItem.NetSalesTotal3 = GetDoubleValuefromTextBox(item, "txtTotalNetSales3");
                        analysisItem.NetSalesIncremental3 = GetDoubleValuefromTextBox(item, "txtIncrementalNetSales3");
                        analysisItem.COGSTotal3 = GetDoubleValuefromTextBox(item, "txtTotalCOGS3");
                        analysisItem.COGSIncremental3 = GetDoubleValuefromTextBox(item, "txtIncrementalCOGS3");
                        analysisItem.GrossMarginTotal3 = GetDoubleValuefromTextBox(item, "txtTotalGrossMargin3");
                        analysisItem.GrossMarginIncremental3 = GetDoubleValuefromTextBox(item, "txtIncrementalGrossMargin3");
                        analysisItem.GrossMarginPctTotal3 = GetDoubleValuefromTextBox(item, "txtTotalGrossMarginPct3");
                        analysisItem.GrossMarginPctIncremental3 = GetDoubleValuefromTextBox(item, "txtIncrementalGrossMarginPct3");


                        analysisItem.NSDollerperLB1 = GetDoubleValuefromTextBox(item, "txtNSPerLB1");
                        analysisItem.NSDollerperLB2 = GetDoubleValuefromTextBox(item, "txtNSPerLB2");
                        analysisItem.NSDollerperLB3 = GetDoubleValuefromTextBox(item, "txtNSPerLB3");
                        analysisItem.COGSperLB1 = GetDoubleValuefromTextBox(item, "txtCOGSPerLBValue1");
                        analysisItem.COGSperLB2 = GetDoubleValuefromTextBox(item, "txtCOGSPerLBValue2");
                        analysisItem.COGSperLB3 = GetDoubleValuefromTextBox(item, "txtCOGSPerLBValue3");
                        analysisItem.TruckldPricePrRtlSllngUt1 = GetDoubleValuefromTextBox(item, "txtTruckLoadPrice1", true);
                        analysisItem.TruckldPricePrRtlSllngUt2 = GetDoubleValuefromTextBox(item, "txtTruckLoadPrice2", true);
                        analysisItem.TruckldPricePrRtlSllngUt3 = GetDoubleValuefromTextBox(item, "txtTruckLoadPrice3", true);
                        analysisItem.Assumptions1 = ((TextBox)item.FindControl("txtAssumptions1")).Text;
                        analysisItem.Assumptions2 = ((TextBox)item.FindControl("txtAssumptions2")).Text;
                        analysisItem.Assumptions3 = ((TextBox)item.FindControl("txtAssumptions3")).Text;
                        //Deleted Status
                        string deletedStatus = ((HiddenField)item.FindControl("hdnDeletedStatus")).Value;
                        if (deletedStatus == "deleted")
                        {
                            analysisItem.Deleted = "Yes";
                        }

                        analysisItem.Id = Convert.ToInt32(((HiddenField)item.FindControl("hdnAnalysisListItemId")).Value.Replace(",", ""));


                        if (analysisItem.Id <= 0 && deletedStatus != "deleted")
                        {
                            int newId = stageGateFinancialServices.InsertStageGateFinancialAnalysisItem(analysisItem, Submitted);
                            HiddenField hdnAnalysisListItemId = ((HiddenField)item.FindControl("hdnAnalysisListItemId"));
                            if (hdnAnalysisListItemId != null) hdnAnalysisListItemId.Value = newId.ToString();
                        }
                        else if (analysisItem.Id > 0 && deletedStatus == "deleted")
                        {
                            stageGateFinancialServices.DeleteStageGateFinancialAnalysisItem(analysisItem.Id);
                        }
                        else
                        {
                            stageGateFinancialServices.UpdateStageGateFinancialAnalysisItem(analysisItem, Submitted);
                        }
                    }
                }
            }
            LoadAllFinacialAnalysis();
        }
        #endregion
        #region Button Click Events
        protected void btnAddNewFinancialBriefAnalysis_Click(object sender, EventArgs e)
        {
            saveData();

            var obj = new StageGateFinancialAnalysisItem();
            obj.StageGateProjectListItemId = StageGateProjectListItemId;
            obj.Gate = GateNo.ToString();
            obj.BriefNumber = BriefNo.ToString();
            int id = stageGateFinancialServices.InsertStageGateFinancialAnalysisItem(obj, false);

            LoadAllFinacialAnalysis();
        }
        #endregion
        #region Repeater Methods
        protected void rptFinancialBriefItem_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                StageGateFinancialAnalysisItem stageGateFinancialAnalysisItem = (StageGateFinancialAnalysisItem)e.Item.DataItem;

                DropDownList ddlIncludePLInConsolidatedFinancials = ((DropDownList)e.Item.FindControl("ddlIncludePLInConsolidatedFinancials"));
                DropDownList ddlDisplayPLInProjectBrief = ((DropDownList)e.Item.FindControl("ddlDisplayPLInProjectBrief"));
                DropDownList ddlCustomerChannel = ((DropDownList)e.Item.FindControl("ddlCustomerChannel"));
                DropDownList ddlBrandSeason = ((DropDownList)e.Item.FindControl("ddlBrandSeason"));
                DropDownList ddlProductForm = ((DropDownList)e.Item.FindControl("ddlProductForm"));

                Utilities.BindDropDownItems(ddlCustomerChannel, GlobalConstants.LIST_CustomersLookup, webUrl);
                AppendChannels(ddlCustomerChannel);
                Utilities.BindDropDownUniqueItemsByTitle(ddlBrandSeason, GlobalConstants.LIST_MaterialGroup1Lookup, webUrl);
                AppendSeasons(ddlBrandSeason);

                Utilities.BindDropDownItems(ddlProductForm, GlobalConstants.LIST_MaterialGroup4Lookup, webUrl);

                if (stageGateFinancialAnalysisItem.PLinConsolidatedFinancials != null && stageGateFinancialAnalysisItem.PLinConsolidatedFinancials != string.Empty)
                {
                    Utilities.SetDropDownValue(stageGateFinancialAnalysisItem.PLinConsolidatedFinancials, ddlIncludePLInConsolidatedFinancials, this.Page);
                }

                if (stageGateFinancialAnalysisItem.PLsinProjectBrief != null && stageGateFinancialAnalysisItem.PLsinProjectBrief != string.Empty)
                {
                    Utilities.SetDropDownValue(stageGateFinancialAnalysisItem.PLsinProjectBrief, ddlDisplayPLInProjectBrief, this.Page);
                }

                if (stageGateFinancialAnalysisItem.CustomerChannel != null && stageGateFinancialAnalysisItem.CustomerChannel != string.Empty)
                {
                    Utilities.SetDropDownValue(stageGateFinancialAnalysisItem.CustomerChannel, ddlCustomerChannel, this.Page);
                }

                if (stageGateFinancialAnalysisItem.BrandSeason != null && stageGateFinancialAnalysisItem.BrandSeason != string.Empty)
                {
                    Utilities.SetDropDownValue(stageGateFinancialAnalysisItem.BrandSeason, ddlBrandSeason, this.Page);
                }

                if (stageGateFinancialAnalysisItem.ProductForm != null && stageGateFinancialAnalysisItem.ProductForm != string.Empty)
                {
                    Utilities.SetDropDownValue(stageGateFinancialAnalysisItem.ProductForm, ddlProductForm, this.Page);
                }
            }
        }
        protected void rptFinancialBriefItem_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SearchByFGNumber")
            {

                string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
                string StageGateProjectListItemitemId = commandArgs[0];

                if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
                {
                    StageGateFinancialAnalysisItem stageGateFinancialAnalysisItem = (StageGateFinancialAnalysisItem)e.Item.DataItem;

                    TextBox txtFGNumber = ((TextBox)e.Item.FindControl("txtFGNumber"));

                    var item = GetItemProposalItemByFGNumber(Convert.ToInt32(StageGateProjectListItemitemId), txtFGNumber.Text);

                    if (item != null)
                    {
                        if (item.CompassListItemId != 0)
                        {
                            DropDownList ddlCustomerChannel = ((DropDownList)e.Item.FindControl("ddlCustomerChannel"));
                            DropDownList ddlBrandSeason = ((DropDownList)e.Item.FindControl("ddlBrandSeason"));
                            DropDownList ddlProductForm = ((DropDownList)e.Item.FindControl("ddlProductForm"));
                            TextBox txtTargetMarginPct = ((TextBox)e.Item.FindControl("txtTargetMarginPct"));

                            if (item.CustomerSpecific == "Customer Specific")
                            {
                                Utilities.SetDropDownValue(item.Customer, ddlCustomerChannel, this.Page);
                            }
                            else if (item.CustomerSpecific == "Channel Specific")
                            {
                                Utilities.SetDropDownValue(item.Channel, ddlCustomerChannel, this.Page);
                            }

                            if (item.ProductHierarchyLevel1 == "Seasonal (000000023)")
                            {
                                Utilities.SetDropDownValue(item.ProductHierarchyLevel2, ddlBrandSeason, this.Page);
                            }
                            else
                            {
                                Utilities.SetDropDownValue(item.MaterialGroup1Brand, ddlBrandSeason, this.Page);
                            }

                            if (!string.IsNullOrEmpty(item.MaterialGroup4ProductForm))
                            {
                                Utilities.SetDropDownValue(item.MaterialGroup4ProductForm, ddlProductForm, this.Page);
                            }

                            txtTargetMarginPct.Text = Convert.ToString(item.ExpectedGrossMarginPercent);
                        }
                    }
                }
            }
        }
        #endregion
        #region Private Methods
        private static double GetDoubleValuefromTextBox(RepeaterItem item, string textboxId, bool RoundOffTwoDecimalPlacess = false)
        {
            TextBox txtBox = ((TextBox)item.FindControl(textboxId));
            string txtValue = txtBox.Text.Replace("$", "").Replace("%", "").Replace(",", "");
            double value = 0;
            if (txtBox != null)
            {
                value = string.IsNullOrWhiteSpace(txtValue) ? 0 : Convert.ToDouble(txtValue);
            }

            if (RoundOffTwoDecimalPlacess) value = Convert.ToDouble(value.ToString("n2"));

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
        private void AppendSeasons(DropDownList ddlBrandSeason)
        {
            // Get the Product Hierarchy Level 1 Value
            string level2 = Utilities.GetLookupValue(GlobalConstants.LIST_ProductHierarchyLevel1Lookup, GlobalConstants.PRODUCT_HIERARCHY1_Seasonal, webUrl);

            if ((!string.IsNullOrEmpty(level2)) && (!string.Equals(level2, "Select...")))
            {
                Utilities.AppendDropDownItemsByValue(ddlBrandSeason, GlobalConstants.LIST_ProductHierarchyLevel2Lookup, level2, webUrl);
            }
        }
        private void AppendChannels(DropDownList ddlCustomerChannel)
        {
            DropDownList ddlChannel = new DropDownList();
            Utilities.BindDropDownItems(ddlChannel, GlobalConstants.LIST_ChannelLookup, webUrl);

            foreach (var item in ddlChannel.Items)
            {
                Utilities.AddItemToDropDown(ddlCustomerChannel, item.ToString(), item.ToString(), true);
            }

        }

        private ItemProposalItem GetItemProposalItemByFGNumber(int StageGateProjectListItemitemId, string FGNumber)
        {
            if (StageGateProjectListItemitemId == 0 || string.IsNullOrEmpty(FGNumber))
            {
                return null;
            }

            var ipfRecord = stageGateFinancialServices.GetItemProposalItemByFGNumber(StageGateProjectListItemitemId, FGNumber);

            return ipfRecord;
        }
        #endregion
    }
}
