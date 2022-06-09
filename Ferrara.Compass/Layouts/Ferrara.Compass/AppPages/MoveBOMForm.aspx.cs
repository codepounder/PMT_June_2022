using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.ComponentModel;
using Ferrara.Compass.Classes;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Services;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Enum;
using Microsoft.Practices.Unity;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

namespace Ferrara.Compass.Layouts.Ferrara.Compass.AppPages
{
    public partial class MoveBOMForm : LayoutsPageBase
    {
        #region Member Variables
        private IPackagingItemService packagingItemService;
        private IUtilityService utilityService;
        private IExceptionService exceptionService;
        private string webUrl = string.Empty;
        #endregion
      
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            packagingItemService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
        }

        #region Properties
        private int PackagingItemId
        {
            get
            {
                if (Request.QueryString[GlobalConstants.QUERYSTRING_PackagingItemId] != null)
                    return Convert.ToInt32(Request.QueryString[GlobalConstants.QUERYSTRING_PackagingItemId]);
                return 0;
            }
        }
        private int CompassItemId
        {
            get
            {
                if (Request.QueryString[GlobalConstants.QUERYSTRING_CompassItemId] != null)
                    return Convert.ToInt32(Request.QueryString[GlobalConstants.QUERYSTRING_CompassItemId]);
                return 0;
            }
        }
        private int RequestId
        {
            get
            {
                if (Request.QueryString[GlobalConstants.QUERYSTRING_RequestId] != null)
                    return Convert.ToInt32(Request.QueryString[GlobalConstants.QUERYSTRING_RequestId]);
                return 0;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            webUrl = SPContext.Current.Web.Url;

            if (!Page.IsPostBack)
            {
                try
                {
                    this.Form.Page.Title = "Move BOM";
                    moveOptions();
                }
                catch (Exception ex)
                {
                    exceptionService.Handle(LogCategory.CriticalError, ex, "MoveBOM", "Page_Load");
                }
            }
        }

        #region Private Methods
        private void moveOptions()
        {
            List<KeyValuePair<int, string>> TSitems = packagingItemService.GetTransferSemiIDsForProject(CompassItemId);
            List<KeyValuePair<int, string>> PCSitems = packagingItemService.GetPurchasedSemiIDsForProject(CompassItemId);
            List<KeyValuePair<int, string>> allItems = new List<KeyValuePair<int, string>>();
            allItems.AddRange(TSitems);
            allItems.AddRange(PCSitems);
            List<int> idsOnly = new List<int>();
            foreach(KeyValuePair<int, string> items in allItems)
            {
                idsOnly.Add(items.Key);
            }
            
            if (allItems != null && allItems.Count > 0)
            {
                List<int> filteredParents = packagingItemService.filterParents(idsOnly, CompassItemId, PackagingItemId);
                foreach (KeyValuePair<int, string> IDs in allItems)
                {
                    int approvedId = filteredParents.IndexOf(IDs.Key);
                    if (IDs.Key != PackagingItemId && approvedId > -1)
                    {
                        drpMoveOptions.Items.Add(new ListItem(IDs.Value, IDs.Key.ToString()));
                    }
                }
            }
        }
       
        #endregion

        #region Button/Link Methods
        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
                Context.Response.Flush();
            }
            catch (Exception exception)
            {

            }
            finally
            {
                Context.Response.End();
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool error = false;
            try
            {
                if (PackagingItemId != 0)
                {
                    if (CompassItemId > 0)
                    {
                        if (drpMoveOptions.SelectedItem.Value == "-1")
                        {
                            this.lblUploadError.Text = "Please select a component to move to.";
                                error = true;
                                return;
                            
                        }
                        var item = new PackagingItem();
                        item.Id = PackagingItemId;
                        item.ParentID = Convert.ToInt32(drpMoveOptions.SelectedItem.Value);
                        item.CompassListItemId = CompassItemId.ToString();
                        packagingItemService.UpdatePackagingItemParentID(item);
                        
                    }
                }
                Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
                Context.Response.Flush();          
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Move BOM Form: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "Move", "btnSubmit_Click");
            }
            finally
            {
                if (!error) { 
                Context.Response.End();
                }
            }
        }
        #endregion
    }
}
