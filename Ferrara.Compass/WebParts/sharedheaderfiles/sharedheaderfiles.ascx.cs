using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Classes;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Models;
using Microsoft.Practices.Unity;
using System.Web.UI.HtmlControls;

namespace Ferrara.Compass.WebParts.sharedheaderfiles
{
    [ToolboxItemAttribute(false)]
    public partial class sharedheaderfiles : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

        #region Member Variables
        public string versionNumber = "74";
        #endregion


        public sharedheaderfiles()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        private void InitializeScreen()
        {

        }

    }
}
