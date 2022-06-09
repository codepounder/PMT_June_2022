using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Microsoft.SharePoint;

namespace Ferrara.Compass.Services
{
    public class SAPBOMService : ISAPBOMService
    {
        public List<SAPBOMListItem> GetSAPBOMItems(string sapItemNumber, string materialType)
        {
            List<SAPBOMListItem> bomItems = new List<SAPBOMListItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_SAPBOMListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + sapItemNumber + "</Value></Eq><Eq><FieldRef Name=\"MaterialType\" /><Value Type=\"Choice\">" + materialType + "</Value></Eq></And></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            SAPBOMListItem newItem = new SAPBOMListItem();
                            //item.Id = item.ID;
                            newItem.MaterialNumber = Convert.ToString(item[SAPBOMListFields.MaterialNumber]);
                            newItem.MaterialDescription = Convert.ToString(item[SAPBOMListFields.MaterialDescription]);
                            newItem.PackQuantity = Convert.ToString(item[SAPBOMListFields.PackQuantity]);
                            newItem.PackUnit = Convert.ToString(item[SAPBOMListFields.PackUnit]);

                            bomItems.Add(newItem);
                        }
                    }
                }
            }
            return bomItems;
        }
        public List<SAPBOMListItem> GetSAPBOMItemsIPF(string sapItemNumber)
        {
            List<SAPBOMListItem> bomItems = new List<SAPBOMListItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_SAPBOMListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + sapItemNumber + "</Value></Eq><In><FieldRef Name=\"MaterialType\" /><Values><Value Type=\"Choice\">FG PACK</Value><Value Type=\"Choice\">PACK</Value><Value Type=\"Choice\">TRANSFER</Value><Value Type=\"Choice\">CANDY</Value><Value Type=\"Choice\">PURCHASED</Value></Values></In></And></Where>";
                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            SAPBOMListItem newItem = new SAPBOMListItem();
                            //item.Id = item.ID;
                            newItem.MaterialNumber = Convert.ToString(item[SAPBOMListFields.MaterialNumber]);
                            newItem.MaterialType = Convert.ToString(item[SAPBOMListFields.MaterialType]);
                            newItem.MaterialDescription = Convert.ToString(item[SAPBOMListFields.MaterialDescription]);
                            newItem.PackQuantity = Convert.ToString(item[SAPBOMListFields.PackQuantity]);
                            newItem.PackUnit = Convert.ToString(item[SAPBOMListFields.PackUnit]);

                            bomItems.Add(newItem);
                        }
                    }
                }
            }
            return bomItems;
        }
        public List<SAPBOMListItem> GetIngredients(string sapItemNumber, string plant)
        {
            List<SAPBOMListItem> bomItems = new List<SAPBOMListItem>();
            string plantCode = string.Empty;

            try
            {
                // Locate Plant code
                plantCode = plant.Substring(plant.IndexOf('(') + 1, (plant.IndexOf(')') - plant.IndexOf('(')) - 1);
            }
            catch
            {
                return bomItems;
            }

            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_SAPBOMListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><And><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + sapItemNumber + "</Value></Eq><Eq><FieldRef Name=\"MaterialType\" /><Value Type=\"Choice\">RAW</Value></Eq></And><Eq><FieldRef Name=\"Plant\" /><Value Type=\"Text\">" + plantCode + "</Value></Eq></And></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            SAPBOMListItem newItem = new SAPBOMListItem();
                            //item.Id = item.ID;
                            newItem.MaterialNumber = Convert.ToString(item[SAPBOMListFields.MaterialNumber]);
                            newItem.MaterialDescription = Convert.ToString(item[SAPBOMListFields.MaterialDescription]);
                            newItem.PackQuantity = Convert.ToString(item[SAPBOMListFields.PackQuantity]);
                            newItem.PackUnit = Convert.ToString(item[SAPBOMListFields.PackUnit]);

                            bomItems.Add(newItem);
                        }
                    }
                }
            }
            return bomItems;
        }
        public List<SAPBOMListItem> GetIngredients(string sapItemNumber)
        {
            List<SAPBOMListItem> bomItems = new List<SAPBOMListItem>();

            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_SAPBOMListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + sapItemNumber +
                        "</Value></Eq><Or><Eq><FieldRef Name=\"MaterialType\" /><Value Type=\"Choice\">RAW</Value></Eq>" +
                        "<IsNull><FieldRef Name=\"MaterialType\" /></IsNull></Or></Eq></And></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            SAPBOMListItem newItem = new SAPBOMListItem();
                            //item.Id = item.ID;
                            newItem.MaterialNumber = Convert.ToString(item[SAPBOMListFields.MaterialNumber]);
                            newItem.MaterialDescription = Convert.ToString(item[SAPBOMListFields.MaterialDescription]);
                            newItem.PackQuantity = Convert.ToString(item[SAPBOMListFields.PackQuantity]);
                            newItem.PackUnit = Convert.ToString(item[SAPBOMListFields.PackUnit]);

                            bomItems.Add(newItem);
                        }
                    }
                }
            }
            return bomItems;
        }
        public List<SAPBOMListItem> GetCandySemis(string sapItemNumber)
        {
            List<SAPBOMListItem> bomItems = new List<SAPBOMListItem>();

            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_SAPBOMListName);
                    SPQuery spQuery = new SPQuery();
                    //spQuery.Query = "<Where><And><Eq><FieldRef Name=\"MaterialNumber\" /><Value Type=\"Text\">" + sapItemNumber + "</Value></Eq><Eq><FieldRef Name=\"MaterialType\" /><Value Type=\"Choice\">CANDY</Value></Eq></And></Where>";
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + sapItemNumber + "</Value></Eq><Eq><FieldRef Name=\"MaterialType\" /><Value Type=\"Choice\">CANDY</Value></Eq></And></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            SAPBOMListItem newItem = new SAPBOMListItem();
                            //item.Id = item.ID;
                            newItem.MaterialNumber = Convert.ToString(item[SAPBOMListFields.MaterialNumber]);
                            newItem.MaterialDescription = Convert.ToString(item[SAPBOMListFields.MaterialDescription]);
                            newItem.PackQuantity = Convert.ToString(item[SAPBOMListFields.PackQuantity]);
                            newItem.PackUnit = Convert.ToString(item[SAPBOMListFields.PackUnit]);

                            bomItems.Add(newItem);
                        }
                    }
                }
            }
            return bomItems;
        }
    }

}





