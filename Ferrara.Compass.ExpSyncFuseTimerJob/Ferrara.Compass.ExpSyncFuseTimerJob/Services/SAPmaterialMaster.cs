using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Ferrara.Compass.ExpSyncFuseTimerJob.Constants;

namespace Ferrara.Compass.ExpSyncFuseTimerJob.Services
{
    public class SAPmaterialMaster
    {
        SPSite site;
        public SAPmaterialMaster(SPSite site)
        {
            this.site = site;
        }
        public List<int> GetProjectIds()
        {
            SPListItemCollection mmItemCol, compassItemCol;
            SPList listMM, listIPF;
            List<int> mmIds;
            SPQuery query;
            string SAPnumber;
            mmIds = new List<int>();
            using (var spWeb = site.OpenWeb())
            {
                listMM = spWeb.Lists.TryGetList(GlobalConstants.LIST_SAPMaterialMasterListName);
                mmItemCol = listMM.GetItems();
                foreach (SPItem smm in mmItemCol)
                {
                    SAPnumber = smm[SAPMaterialMasterListFields.HanaKey].ToString();
                    listIPF = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    query = new SPQuery();
                    query.Query = "<Where><And><Eq><FieldRef Name=\"" + CompassListFields.SAPItemNumber + "\" /><Value Type=\"Text\">" + SAPnumber + "</Value></Eq>" +
                        "<Neq><FieldRef Name=\"" + CompassListFields.WorkflowPhase + "\" /><Value Type=\"Text\">" + GlobalConstants.WORKFLOWPHASE_Cancelled +
                        "</Value></Neq></And></Where>";
                    compassItemCol = listIPF.GetItems(query);
                    foreach (SPItem ipf in compassItemCol)
                            mmIds.Add(ipf.ID);
                }
            }
            return mmIds;
        }
    }
}
