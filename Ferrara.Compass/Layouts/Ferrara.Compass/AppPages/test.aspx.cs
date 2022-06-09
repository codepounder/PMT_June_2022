using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.Services;

namespace Ferrara.Compass.Layouts.Compass.AppPages
{
    public partial class test : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [WebMethod]
        public static string Save(Test obj)
        {
            return "1";
        }



        [WebMethod]
        public static string LoadShipper()
        {
            return "1";
        }
        public class Test
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
}
