using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Ferrara.Compass.Services;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Classes;
using Microsoft.Practices.Unity;

namespace Ferrara.Compass.ControlTemplates.Ferrara.Compass
{
    public partial class TransferSemiBOMs : UserControl
    {
        private IPackagingItemService packagingService;
        private int iItemId = 0;
        private string ProjectNumber
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo];
                return string.Empty;
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            packagingService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            iItemId = Utilities.GetItemIdFromProjectNumber(ProjectNumber);
            rptTransferSemi.DataSource = packagingService.GetTransferSemiItemsForProject(iItemId);
            rptTransferSemi.DataBind();
        }
        protected void rptTransferSemi_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rptTransferSemiChildren;
            PackagingItem data;
            Label lblNoComponentsfound;
            int transferSemiId;
            data = (PackagingItem)e.Item.DataItem;
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
