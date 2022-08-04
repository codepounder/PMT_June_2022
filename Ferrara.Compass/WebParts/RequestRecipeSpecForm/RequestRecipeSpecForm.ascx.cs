using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;

namespace Ferrara.Compass.WebParts.RequestRecipeSpecForm
{
    [ToolboxItemAttribute(false)]
    public partial class RequestRecipeSpecForm : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public RequestRecipeSpecForm()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string requestType = Page.Request.QueryString["RequestType"];
            if (!Page.IsPostBack)
            {
                if (requestType == null || requestType == "FG")
                {
                    this.divSemiSection.Visible = false;
                    this.divFGSection.Visible = true;

                }
                else
                {
                    this.divSemiSection.Visible = true;
                    this.divFGSection.Visible = false;
                }
                // comment by mahipal
            }
        }

        protected void btnClick()
        {
            //test
        }
    }
}
