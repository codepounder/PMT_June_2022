using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using Ferrara.Compass.Services;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Microsoft.Practices.Unity;
using Ferrara.Compass.Abstractions.Constants;

namespace Ferrara.Compass.ControlTemplates.Ferrara.Compass
{
    public partial class ucTransferSemiBOMs : UserControl
    {
        private IPackagingItemService packagingService;
        private int mCompassId = 0;
        public int CompassId { get { return mCompassId; } set { mCompassId = value; } }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            packagingService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            List<PackagingItem> p = packagingService.GetTransferPurchasedSemiItemsForProject(mCompassId,GlobalConstants.COMPONENTTYPE_TransferSemi);
            p.AddRange(packagingService.GetTransferPurchasedSemiItemsForProject(mCompassId, GlobalConstants.COMPONENTTYPE_PurchasedSemi));
            rptTransferSemi.DataSource = p;
            rptTransferSemi.DataBind();
        }
        protected void rptTransferSemi_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rptTransferSemiChildren;
            PackagingItem data;
            Label lblNoComponentsfound;
            int transferSemiId;
            data = (PackagingItem)e.Item.DataItem;
            string HeaderTitle = "";
            if(data.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
            {
                HeaderTitle = GlobalConstants.COMPONENTTYPE_PurchasedSemi;
            }
            else
            {
                HeaderTitle = GlobalConstants.COMPONENTTYPE_TransferSemi;
            }
            Label lblHeaderText = (Label)e.Item.FindControl("lblHeaderText");
            lblHeaderText.Text = HeaderTitle;
            transferSemiId = data.Id;
            rptTransferSemiChildren = (Repeater)e.Item.FindControl("rptTransferSemiChildren");
            lblNoComponentsfound = (Label)e.Item.FindControl("lblNoComponentsfound");
            rptTransferSemiChildren.DataSource = packagingService.GetPackagingChildren(transferSemiId);
            rptTransferSemiChildren.DataBind();
            if (rptTransferSemiChildren.Items.Count == 0)
            {
                lblNoComponentsfound.Visible = true;
                rptTransferSemiChildren.Visible = false;
            }
        }
    }
}