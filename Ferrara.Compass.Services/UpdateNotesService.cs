using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Constants;

namespace Ferrara.Compass.Services
{
    public class UpdateNotesService : IUpdateNotesService
    {
        
        public void UpdateProjectNotes(int CompassListItemId, string notes)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPListItem compassItemCol = spList.GetItemById(CompassListItemId);

                        if (compassItemCol != null)
                        {
                            compassItemCol[CompassListFields.ProjectNotes] = notes;

                            compassItemCol.Update();

                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }      
    }
}

