using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Ferrara.Compass.Abstractions.Constants;

namespace Ferrara.Compass.Layouts.Ferrara.Compass.AppPages
{
    public partial class CompassErrorPage : LayoutsPageBase
    {
        #region Member Variables
        private string siteURL;
        #endregion
        
        #region Properties
        private int ErrorId
        {
            get
            {
                if (Page.Request.QueryString["ErrorId"] != null)
                    return Convert.ToInt32(Request.QueryString["ErrorId"]);
                return 0;
            }
        }

        private string ProjectNumber
        {
            get
            {
                if (Page.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo] != null)
                    return Page.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo];
                return string.Empty;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            // Production
            //siteURL = "/sites/CFTS";
            // DEV
            //siteURL = string.Empty;

            if (string.IsNullOrEmpty(ProjectNumber))
            {
                lbCommercializationItem.Visible = false;
                lblCommHeader.Visible = false;
            }

            DisplayErrorMessage();
        }

        private void DisplayErrorMessage()
        {
            switch (ErrorId)
            {
                case 1:
                    lblErrorMessage.Text = "The Project Number you have supplied is not valid. <br>No matching projects were found. Please check that you are using the correct Project Number.";
                    break;
                case 2:
                    lblErrorMessage.Text = "The Project Number you have supplied has already passed the current stage in the workflow.";
                    break;
                case 3:
                    lblErrorMessage.Text = "You do not have sufficient rights to access the page.";
                    break;
                case 4:
                    lblErrorMessage.Text = "An Invalid workflow state has been detected for the current project.";
                    break;
                case 5:
                    lblErrorMessage.Text = "An Invalid User Type has been detected for the current project.";
                    break;
                case 6:
                    lblErrorMessage.Text = "The Project's Packaging Component you have supplied is not valid. <br>No matching components were found. Please check that you are using the correct Component Number.";
                    break;
                default:
                    lblErrorMessage.Text = "An unidentified error has occurred.";
                    break;

            }
        }

        protected void lbCommercializationItem_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(SPContext.Current.Web.Url + siteURL +"/Pages/CommercializationItem.aspx?ProjectNo=" + ProjectNumber, false);
        }

        protected void lbHome_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(SPContext.Current.Web.Url + siteURL, false);
        }
    }
}
