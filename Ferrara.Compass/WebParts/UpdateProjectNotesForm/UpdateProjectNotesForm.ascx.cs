using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using System.ComponentModel;
using Ferrara.Compass.Classes;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Interfaces;
using System.Web;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using Microsoft.SharePoint;

namespace Ferrara.Compass.WebParts.UpdateProjectNotesForm
{
    [ToolboxItemAttribute(false)]
    public partial class UpdateProjectNotesForm : WebPart
    {

        private IUpdateNotesService updateNotesService;

        private string webUrl;
        private string CompassItemId
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_CompassItemId] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_CompassItemId];
                return string.Empty;
            }
        }
        private string projectNotes
        {
            get
            {
                if (HttpContext.Current.Request.QueryString["projectNotes"] != null)
                    return HttpContext.Current.Request.QueryString["projectNotes"];
                return string.Empty;
            }
        }

        public UpdateProjectNotesForm()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            updateNotesService = DependencyResolution.DependencyMapper.Container.Resolve<IUpdateNotesService>();

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CompassItemId != null && CompassItemId != "")
            {
                int compassID = Convert.ToInt32(CompassItemId);
                if (!Page.IsPostBack)
                {
                    updateNotesService.UpdateProjectNotes(compassID, projectNotes);
                }
            }
        }
    }
}
