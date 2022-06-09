using System;
using System.ComponentModel;
using Microsoft.SharePoint;
using System.Web.UI.WebControls.WebParts;
using System.Web.Services;
using Ferrara.Compass.Services;

public partial class WebMethods : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    //[WebMethod]
    //public static string Save(ItemProposalService obj)
    //{
    //    return "1";
    //}
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