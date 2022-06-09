using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.Practices.Unity;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Models;
using Microsoft.SharePoint.WebControls;
using System.Collections.Specialized;
using System.Collections;
using System.Text;
using Ferrara.Compass.Abstractions.Enum;
using System.Threading;
using System.IO;
using System.Xml.Serialization;

namespace Ferrara.Compass.Classes
{
    public class Utilities
    {
        /// <summary>
        /// Method to return unique number based on ddmmyyyyhhssmm as a negetive number to be used
        /// when creating new list items and avoiding duplication of the id for the current session
        /// </summary>
        /// <returns>int - unique negative number</returns>
        public static int GetUniqueId()
        {
            int intUniqueId = -1;
            try
            {

                Thread.Sleep(10);
                DateTime currentDateTime = DateTime.Now;
                string strDateTimeStamp = currentDateTime.Hour.ToString() + currentDateTime.Minute.ToString() + currentDateTime.Second.ToString() + currentDateTime.Millisecond.ToString();
                intUniqueId = intUniqueId * int.Parse(strDateTimeStamp);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.Write(ex.Message);
            }
            return intUniqueId;
        }
        public static string GetRandomDigit()
        {
            Random r = new Random();

            return r.Next(0, 10).ToString();
        }

        #region Formatting Methods
        public static string FormatNumber(int value)
        {
            if (value == -9999)
                return string.Empty;

            return value.ToString("N0");
        }
        public static string FormatNumber(double value)
        {
            if (value == -9999)
                return string.Empty;

            return value.ToString("N0");
        }
        public static string FormatDecimal(double value, int numPlaces)
        {
            if (value == -9999)
                return string.Empty;

            return value.ToString("N" + numPlaces.ToString());
        }
        public static string FormatPercentage(double value, int numPlaces)
        {
            if (value == -9999)
                return string.Empty;

            return value.ToString("N" + numPlaces.ToString()) + "%";
        }
        public static string FormatCurrency(double value)
        {
            if (value == -9999)
                return string.Empty;

            return value.ToString("C").Replace("$", "");
        }
        public static string RemoveFormatting(string value)
        {
            value = value.Replace("%", "");
            value = value.Replace("$", "");
            value = value.Replace(",", "");

            return value;
        }

        public static int GetNumber(string value)
        {
            int newValue = 0;
            value = RemoveFormatting(value);
            try
            {
                newValue = Convert.ToInt32(value);
            }
            catch
            {
                newValue = -9999;
            }

            return newValue;
        }
        public static double GetDecimal(string value)
        {
            double newValue = 0;
            value = RemoveFormatting(value);
            try
            {
                newValue = Convert.ToDouble(value);
            }
            catch
            {
                newValue = -9999;
            }

            return newValue;
        }
        public static double GetCurrency(string value)
        {
            return GetDecimal(value);
        }
        public static List<Int32> GetIntegerArrayFromDelimittedString(string value, char delimitter)
        {
            var integerList = new List<int>();
            if (!string.IsNullOrEmpty(value))
            {
                var stringList = value.Split(delimitter);
                integerList = stringList
                            .Select(s => { int i; return int.TryParse(s, out i) ? i : (int?)null; })
                            .Where(i => i.HasValue)
                            .Select(i => i.Value)
                            .ToList();
            }
            return integerList;
        }

        #endregion

        #region Project Number Methods
        public static int GetStageGateProjectListItemIdFromCompassListItemId(int compassItemId)
        {
            var utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            var StageGateProjectListItemId = utilityService.GetStageGateProjectListItemIdFromCompassListItemId(compassItemId);
            return StageGateProjectListItemId;
        }
        public static int GetStageGateProjectListItemIdFromProjectNumber(string projectNumber)
        {
            var utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            var itemId = utilityService.GetStageGateProjectListItemIdFromProjectNumber(projectNumber);
            return itemId;
        }
        public static int GetItemIdFromProjectNumber(string projectNumber)
        {
            var utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            var itemId = utilityService.GetItemIdFromProjectNumber(projectNumber);
            return itemId;
        }
        public static string GetOnHoldWorkFlowPhase(int CompassListItemId)
        {
            var utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            return utilityService.GetOnHoldWorkFlowPhase(CompassListItemId);
        }

        public static int GetItemIdByProjectNumberFromStageGateProjectList(string projectNumber)
        {
            var utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            var itemId = utilityService.GetItemIdByProjectNumberFromStageGateProjectList(projectNumber);
            return itemId;
        }
        public static string DetermineChangeRequestProjectNumber(string existingProjectNumber)
        {
            string newProjectNumber = string.Empty;
            string lastCharacter = string.Empty;

            // Check if the last character is A-Z
            lastCharacter = existingProjectNumber.Substring(existingProjectNumber.Length - 1);

            byte[] asciiValue = Encoding.ASCII.GetBytes(lastCharacter);
            int value = asciiValue[0];

            // Convert to upper case if the last character is a-Z
            if ((value >= 97) && (value <= 122))
            {
                lastCharacter = lastCharacter.ToUpper();
                asciiValue = Encoding.ASCII.GetBytes(lastCharacter);
                value = asciiValue[0];
            }

            if ((value >= 65) && (value <= 90))
            {
                // Check if we are on Z currently
                if (value == 90)
                {
                    Boolean bDone = false;
                    // Keep checking previous characters for Z or non-alphabetic
                    newProjectNumber = "A";
                    while (!bDone)
                    {
                        // Since this was a Z, we will create next value with A and possibly increment previous letter. 
                        // (Z goes to AA, AZ goes to BA, etc)
                        existingProjectNumber = existingProjectNumber.Substring(0, existingProjectNumber.Length - 1);
                        lastCharacter = existingProjectNumber.Substring(existingProjectNumber.Length - 1);
                        asciiValue = Encoding.ASCII.GetBytes(lastCharacter);
                        value = asciiValue[0];
                        // Convert to upper case if the last character is a-Z
                        if ((value >= 97) && (value <= 122))
                        {
                            lastCharacter = lastCharacter.ToUpper();
                            asciiValue = Encoding.ASCII.GetBytes(lastCharacter);
                            value = asciiValue[0];
                        }

                        if ((value >= 65) && (value <= 90))
                        {
                            if (value == 90)
                            {
                                // Found another Z
                                newProjectNumber = newProjectNumber + "A";
                            }
                            else
                            {
                                // Found another alphabetic... so increment
                                newProjectNumber = existingProjectNumber.Substring(0, existingProjectNumber.Length - 1) + Convert.ToChar(value + 1).ToString() + newProjectNumber;
                                bDone = true;
                            }
                        }
                        else
                        {
                            // No Previous letter found
                            newProjectNumber = existingProjectNumber + "A" + newProjectNumber;
                            bDone = true;
                        }
                    }
                }
                else
                {
                    newProjectNumber = existingProjectNumber.Substring(0, existingProjectNumber.Length - 1) + Convert.ToChar(value + 1).ToString();
                }
            }
            else
            {
                // This is the first Change Request so append "A" to the existing project
                newProjectNumber = existingProjectNumber + "A";
            }

            return newProjectNumber;
        }

        internal static void BindDropDownItems(object drpNutrientType, string lIST_PackagingComponentTypesLookup, object url)
        {
            throw new NotImplementedException();
        }

        public static Boolean IsChangeRequestProjectNumber(string existingProjectNumber)
        {
            string lastCharacter = string.Empty;

            if (string.IsNullOrEmpty(existingProjectNumber))
                return false;

            // Check if the last character is A-Z
            lastCharacter = existingProjectNumber.Substring(existingProjectNumber.Length - 1);
            byte[] asciiValue = Encoding.ASCII.GetBytes(lastCharacter);
            int value = asciiValue[0];
            if ((value >= 65) && (value <= 90))
                return true;

            return false;
        }
        #endregion

        #region People / People Picker Methods
        public IEnumerable<string> GetUserGroups()
        {
            List<string> groups = new List<string>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPUser spUser = spWeb.CurrentUser;

                    if (spUser != null)
                    {
                        if (spUser.Groups != null)
                        {
                            groups.AddRange(from SPGroup g in spUser.Groups select g.Name);
                        }
                    }
                }
            }
            return groups;
        }
        public static SPFieldUserValueCollection GetPeopleFromPickerControl(PeopleEditor people, SPWeb web)
        {
            SPFieldUserValueCollection values = new SPFieldUserValueCollection();
            var users = people.CommaSeparatedAccounts.Split(',');
            foreach (string user in users)
            {
                var spUser = web.EnsureUser(user);
                spUser = web.SiteUsers[user];
                var userName = new SPFieldUserValue(web, spUser.ID, spUser.LoginName);
                values.Add(userName);
            }
            #region -- This doesnt work
            /*
            if (people.ResolvedEntities.Count > 0)
            {
                for (int i = 0; i < people.ResolvedEntities.Count; i++)
                {
                    PickerEntity user = (PickerEntity)people.ResolvedEntities[i];
                    switch ((string)user.EntityData["PrincipalType"])
                    {
                        case "User":
                            SPUser webUser = web.EnsureUser(user.Key);
                            SPFieldUserValue userValue = new SPFieldUserValue(web, webUser.ID, webUser.Name);
                            values.Add(userValue);
                            break;

                        case "SharePointGroup":
                            SPGroup siteGroup = web.SiteGroups[user.EntityData["AccountName"].ToString()];
                            SPFieldUserValue groupValue = new SPFieldUserValue(web, siteGroup.ID, siteGroup.Name);
                            values.Add(groupValue);
                            break;
                    }
                }
            }**/
            #endregion
            return values;
        }
        public static string GetNamesFromPickerControl(PeopleEditor people, SPWeb web)
        {
            string values = string.Empty;

            var users = people.CommaSeparatedAccounts.Split(',');
            foreach (string user in users)
            {
                var spUser = web.EnsureUser(user);
                spUser = web.SiteUsers[user];
                values = spUser.Name;
            }

            return values;
        }
        public static SPFieldUserValueCollection GetPeopleFromPickerControl(string LopinName, SPWeb web)
        {
            SPFieldUserValueCollection values = new SPFieldUserValueCollection();
            //SPUser user = (
            //    from SPUser c in web.Users
            //               where c.LoginName == LopinName
            //            select c).First();
            var spUser = web.EnsureUser(LopinName);
            spUser = web.SiteUsers[LopinName];
            var userName = new SPFieldUserValue(web, spUser.ID, spUser.LoginName);
            values.Add(userName);
            return values;
        }
        public static string SetPeoplePickerValue(string loginName, SPWeb web)
        {
            var users = string.Empty;
            SPFieldUserValueCollection userCol = new SPFieldUserValueCollection(web, loginName);
            foreach (SPFieldUserValue usrItm in userCol)
            {
                users += usrItm.User.ToString() + ",";
            }
            return users;
        }
        public static string GetPersonFieldForDisplay(string person)
        {
            if (string.IsNullOrEmpty(person))
                return string.Empty;
            if (person.IndexOf("#") < 0)
                return person;

            return person.Substring(person.IndexOf("#") + 1);
        }
        public static string GetPersonFieldForSummary(string person)
        {
            string name = string.Empty;
            if (!string.IsNullOrEmpty(person))
            {
                var str = person.Split('\\');
                name = str[1];

            }
            return name;
        }
        public static Boolean CheckIfCurrentUserInGroup(string groups)
        {
            bool bFound = false;
            string[] groupArray = groups.Split(',');

            foreach (string group in groupArray)
            {
                if (SPContext.Current.Web.SiteGroups[group].ContainsCurrentUser)
                {
                    bFound = true;
                    break;
                }
            }
            return bFound;
        }

        public static List<string> GetAllUsersOfGroup(string groupName, Boolean IncludeNA = false)
        {
            SPUserCollection GroupUSers = SPContext.Current.Web.SiteGroups[groupName].Users;
            List<string> GroupMembers = new List<string>();
            try
            {
                if (IncludeNA)
                {
                    GroupMembers.Add("NA");
                }
                foreach (SPUser GroupUSer in GroupUSers)
                {
                    GroupMembers.Add(GroupUSer.Name);
                }
            }
            catch (Exception ex)
            {
            }
            return GroupMembers;
        }
        public static Boolean CheckIfUserInGroup(string groupName, string userName)
        {
            bool bFound = false;

            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPUser user = spWeb.EnsureUser(userName);
                    if (user.Groups.Cast<SPGroup>().Any(g => g.Name.Equals(groupName)))
                    {
                        bFound = true;
                    }
                }
            }

            return bFound;

        }
        #endregion

        #region Dropdown Methods
        public static void AddItemToDropDown(DropDownList drp, string Text, string value, bool enabled)
        {
            drp.Items.Add(new ListItem(Text, value, enabled));
        }
        public static void AddItemToDropDownWithAttributes(DropDownList drp, string Text, string value, string AttrKey, string AttrValue)
        {
            var NewListItem = new ListItem(Text, value, true);
            NewListItem.Attributes.Add(AttrKey, AttrValue);
            drp.Items.Add(NewListItem);
        }
        public static void AppendDropDownItemsByValue(DropDownList drp, string listName, string value, string webUrl)
        {
            using (var site = new SPSite(webUrl))
            {
                using (SPWeb spweb = site.OpenWeb())
                {
                    SPList spList = spweb.Lists.TryGetList(listName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where>" +
                                        "<And>" +
                                            "<Eq><FieldRef Name=\"Compass_Value\" /><Value Type=\"Text\">" + value + "</Value></Eq>" +
                                            "<Eq><FieldRef Name=\"RoutingEnabled\" /><Value Type=\"Boolean\">1</Value></Eq>" +
                                        "</And>" +
                                    "</Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        var fields = new string[] { GlobalLookupFieldConstants.Id, GlobalLookupFieldConstants.Title, GlobalLookupFieldConstants.Active };
                        foreach (SPListItem enumItems in compassItemCol)
                        {
                            drp.Items.Add(new ListItem(enumItems.Title, Convert.ToString("5" + enumItems[GlobalLookupFieldConstants.Id])));
                        }
                    }
                }
            }
        }
        public static void BindGroupMembersToDropDown(DropDownList drp, string GroupName, Boolean IncludeNA = false)
        {
            SPUserCollection GroupUSers = SPContext.Current.Web.SiteGroups[GroupName].Users;
            try
            {
                drp.Items.Clear();
                drp.Items.Add(new ListItem("Select...", "-1"));
                if (IncludeNA)
                {
                    drp.Items.Add(new ListItem("NA", "NA"));
                }
                foreach (SPUser GroupUSer in GroupUSers)
                {
                    drp.Items.Add(new ListItem(GroupUSer.Name, Convert.ToString(GroupUSer.LoginName)));
                }
            }
            catch (Exception ex)
            {
            }
        }
        public static void BindDropDownItems(DropDownList drp, string listName, string webUrl)
        {
            using (var site = new SPSite(webUrl))
            {
                using (var spweb = site.OpenWeb())
                {
                    var spList = spweb.Lists.TryGetList(listName);
                    if (spList != null)
                    {
                        var fields = new string[] { GlobalLookupFieldConstants.Id, GlobalLookupFieldConstants.Title, GlobalLookupFieldConstants.Active };
                        var items = spList.GetItems(fields).Cast<SPListItem>().Where(x => Convert.ToBoolean(x[GlobalLookupFieldConstants.Active]).Equals(true)).OrderBy(y => y.Title);
                        var enumItems = items.GetEnumerator();
                        drp.ClearSelection();
                        while (enumItems.MoveNext())
                        {
                            drp.Items.Add(new ListItem(enumItems.Current.Title, Convert.ToString(enumItems.Current[GlobalLookupFieldConstants.Id])));
                        }
                    }
                }
            }
        }

        public static void BindDropDownItemsWithClass(DropDownList drp, string listName, string webUrl)
        {
            using (var site = new SPSite(webUrl))
            {
                using (var spweb = site.OpenWeb())
                {
                    var spList = spweb.Lists.TryGetList(listName);
                    if (spList != null)
                    {
                        var fields = new string[] { GlobalLookupFieldConstants.Id, GlobalLookupFieldConstants.Title, GlobalLookupFieldConstants.Active, "PrintType" };
                        var items = spList.GetItems(fields).Cast<SPListItem>().Where(x => Convert.ToBoolean(x[GlobalLookupFieldConstants.Active]).Equals(true)).OrderBy(y => y.Title);
                        var enumItems = items.GetEnumerator();
                        drp.ClearSelection();
                        while (enumItems.MoveNext())
                        {
                            var NewListItem = new ListItem(enumItems.Current.Title, Convert.ToString(enumItems.Current[GlobalLookupFieldConstants.Id]));
                            NewListItem.Attributes.Add("class", string.Concat("PrintStyleType ", Convert.ToString(enumItems.Current["PrintType"])));
                            drp.Items.Add(NewListItem);
                        }
                    }
                }
            }
        }
        public static void BindDropDownItemsPHL1(DropDownList drp, string listName, string webUrl)
        {
            using (var site = new SPSite(webUrl))
            {
                using (var spweb = site.OpenWeb())
                {
                    var spList = spweb.Lists.TryGetList(listName);
                    if (spList != null)
                    {
                        var fields = new string[] { GlobalLookupFieldConstants.Id, GlobalLookupFieldConstants.Title, GlobalLookupFieldConstants.Active, GlobalLookupFieldConstants.LookupValue };
                        drp.Items.Clear();
                        drp.Items.Add(new ListItem("Select...", "-1"));
                        var items = spList.GetItems(fields).Cast<SPListItem>().Where(x => Convert.ToBoolean(x[GlobalLookupFieldConstants.Active]).Equals(true)).OrderBy(y => y.Title);
                        var enumItems = items.GetEnumerator();
                        drp.ClearSelection();

                        while (enumItems.MoveNext())
                        {
                            ListItem newListItem = new ListItem(enumItems.Current.Title, Convert.ToString(enumItems.Current[GlobalLookupFieldConstants.LookupValue]));
                            newListItem.Attributes.Add("class", string.Concat("PHLOptions ", Convert.ToString(enumItems.Current[GlobalLookupFieldConstants.LookupValue])));
                            drp.Items.Add(newListItem);
                            //drp.Items.Add(new ListItem(enumItems.Current.Title, Convert.ToString(enumItems.Current[GlobalLookupFieldConstants.Id])));
                        }
                    }
                }
            }
        }
        public static void BindDropDownItemsPHL2(DropDownList drp, string listName, string webUrl)
        {
            using (var site = new SPSite(webUrl))
            {
                using (var spweb = site.OpenWeb())
                {
                    var spList = spweb.Lists.TryGetList(listName);
                    if (spList != null)
                    {
                        var fields = new string[] { GlobalLookupFieldConstants.Id, GlobalLookupFieldConstants.Title, GlobalLookupFieldConstants.Active, GlobalLookupFieldConstants.LookupValue };
                        drp.Items.Clear();
                        drp.Items.Add(new ListItem("Select...", "-1"));
                        var items = spList.GetItems(fields).Cast<SPListItem>().Where(x => Convert.ToBoolean(x[GlobalLookupFieldConstants.Active]).Equals(true)).OrderBy(y => y.Title);
                        var enumItems = items.GetEnumerator();
                        drp.ClearSelection();

                        while (enumItems.MoveNext())
                        {
                            ListItem newListItem = new ListItem(enumItems.Current.Title, Convert.ToString(enumItems.Current[GlobalLookupFieldConstants.Id]));
                            newListItem.Attributes.Add("class", string.Concat("PHLOptions ", Convert.ToString(enumItems.Current[GlobalLookupFieldConstants.LookupValue])));
                            drp.Items.Add(newListItem);
                            //drp.Items.Add(new ListItem(enumItems.Current.Title, Convert.ToString(enumItems.Current[GlobalLookupFieldConstants.Id])));
                        }

                    }
                }
            }
        }
        public static void BindDropDownItemsBrand(DropDownList drp, string listName, string webUrl)
        {
            using (var site = new SPSite(webUrl))
            {
                using (var spweb = site.OpenWeb())
                {
                    var spList = spweb.Lists.TryGetList(listName);
                    if (spList != null)
                    {
                        var fields = new string[] { GlobalLookupFieldConstants.Id, GlobalLookupFieldConstants.Title, GlobalLookupFieldConstants.Active, GlobalLookupFieldConstants.LookupValue, "ParentPHL2" };
                        drp.Items.Clear();
                        drp.Items.Add(new ListItem("Select...", "-1"));
                        var items = spList.GetItems(fields).Cast<SPListItem>().Where(x => Convert.ToBoolean(x[GlobalLookupFieldConstants.Active]).Equals(true)).OrderBy(y => y.Title);
                        var enumItems = items.GetEnumerator();
                        drp.ClearSelection();

                        while (enumItems.MoveNext())
                        {
                            ListItem newListItem = new ListItem(enumItems.Current.Title, Convert.ToString(enumItems.Current[GlobalLookupFieldConstants.Id]));
                            newListItem.Attributes.Add("class", string.Concat("PHLOptions ",
                                Convert.ToString(enumItems.Current["ParentPHL2"])
                                .Replace(" ", "")
                                .Replace("(", "")
                                .Replace(")", "")
                                .Replace("'", "")
                                .Replace("-", "")
                                .Replace("&", "")
                                .Replace("/", "")
                                ));
                            drp.Items.Add(newListItem);
                        }
                    }
                }
            }
        }
        public static void BindDropDownUniqueItemsByTitle(DropDownList drp, string listName, string webUrl)
        {
            using (var site = new SPSite(webUrl))
            {
                using (var spweb = site.OpenWeb())
                {
                    var spList = spweb.Lists.TryGetList(listName);
                    if (spList != null)
                    {
                        var fields = new string[] { GlobalLookupFieldConstants.Id, GlobalLookupFieldConstants.Title, GlobalLookupFieldConstants.Active };
                        var items = spList.GetItems(fields).Cast<SPListItem>().Where(x => Convert.ToBoolean(x[GlobalLookupFieldConstants.Active]).Equals(true)).OrderBy(y => y.Title);
                        var uniqueItems = items.GroupBy(x => x.Title).Select(y => y.First());
                        var enumItems = uniqueItems.GetEnumerator();
                        drp.ClearSelection();
                        while (enumItems.MoveNext())
                        {
                            drp.Items.Add(new ListItem(enumItems.Current.Title, Convert.ToString(enumItems.Current[GlobalLookupFieldConstants.Id])));
                        }
                    }
                }
            }
        }
        public static void BindDropDownPassColumnNames(DropDownList drp, string listName, string webUrl, string idColumn, string textColumn)
        {
            using (var site = new SPSite(webUrl))
            {
                using (var spweb = site.OpenWeb())
                {
                    var spList = spweb.Lists.TryGetList(listName);
                    if (spList != null)
                    {
                        var fields = new string[] { idColumn, textColumn, GlobalLookupFieldConstants.Active };
                        var items = spList.GetItems(fields).Cast<SPListItem>().Where(x => Convert.ToBoolean(x[GlobalLookupFieldConstants.Active]).Equals(true)).OrderBy(y => y.Title);
                        var uniqueItems = items.GroupBy(x => x.Title).Select(y => y.First());
                        var enumItems = uniqueItems.GetEnumerator();
                        drp.ClearSelection();
                        while (enumItems.MoveNext())
                        {
                            drp.Items.Add(new ListItem(Convert.ToString(enumItems.Current[textColumn]), Convert.ToString(enumItems.Current[idColumn])));
                        }
                    }
                }
            }
        }
        public static void BindDropDownItemsAddValues(DropDownList drp, string listName, string webUrl)
        {
            using (var site = new SPSite(webUrl))
            {
                using (var spweb = site.OpenWeb())
                {
                    var spList = spweb.Lists.TryGetList(listName);
                    if (spList != null)
                    {
                        var fields = new string[] { GlobalLookupFieldConstants.Id, GlobalLookupFieldConstants.Title, GlobalLookupFieldConstants.Active, GlobalLookupFieldConstants.LookupValue };
                        var items = spList.GetItems(fields).Cast<SPListItem>().Where(x => Convert.ToBoolean(x[GlobalLookupFieldConstants.Active]).Equals(true)).OrderBy(y => y.Title);
                        var enumItems = items.GetEnumerator();
                        drp.ClearSelection();
                        while (enumItems.MoveNext())
                        {
                            drp.Items.Add(new ListItem(enumItems.Current.Title + " (" + Convert.ToString(enumItems.Current[GlobalLookupFieldConstants.LookupValue]) + ")", Convert.ToString(enumItems.Current[GlobalLookupFieldConstants.Id])));
                        }
                    }
                }
            }
        }
        public static void BindDropDownWithTitleFilter(DropDownList drp, string listName, string webUrl, bool has, string filter)
        {
            using (var site = new SPSite(webUrl))
            {
                using (var spweb = site.OpenWeb())
                {
                    var spList = spweb.Lists.TryGetList(listName);
                    if (spList != null)
                    {
                        var fields = new string[] { GlobalLookupFieldConstants.Id, GlobalLookupFieldConstants.Title, GlobalLookupFieldConstants.Active };
                        var items = spList.GetItems(fields).Cast<SPListItem>().Where(x => Convert.ToBoolean(x[GlobalLookupFieldConstants.Active]).Equals(true)).OrderBy(y => y.Title);
                        var enumItems = items.GetEnumerator();
                        drp.ClearSelection();
                        while (enumItems.MoveNext())
                        {
                            if (has)
                            {
                                if (enumItems.Current.Title.ToLower().Contains(filter.ToLower()))
                                {
                                    drp.Items.Add(new ListItem(enumItems.Current.Title, Convert.ToString(enumItems.Current[GlobalLookupFieldConstants.Id])));
                                }
                            }
                            else
                            {
                                if (!enumItems.Current.Title.ToLower().Contains(filter.ToLower()))
                                {
                                    drp.Items.Add(new ListItem(enumItems.Current.Title, Convert.ToString(enumItems.Current[GlobalLookupFieldConstants.Id])));
                                }
                            }
                        }
                    }
                }
            }
        }
        public static void BindDropDownWithColumnFilter(DropDownList drp, string listName, string webUrl, string ColumnName, string filterValue = "Yes")
        {
            using (var site = new SPSite(webUrl))
            {
                using (var spweb = site.OpenWeb())
                {
                    var spList = spweb.Lists.TryGetList(listName);
                    if (spList != null)
                    {
                        var fields = new string[] { GlobalLookupFieldConstants.Id, GlobalLookupFieldConstants.Title, GlobalLookupFieldConstants.Active, ColumnName };
                        var items = spList.GetItems(fields).Cast<SPListItem>().Where(x => Convert.ToBoolean(x[GlobalLookupFieldConstants.Active]).Equals(true) && x[ColumnName].Equals("Yes")).OrderBy(y => y.Title);
                        var enumItems = items.GetEnumerator();
                        drp.ClearSelection();
                        while (enumItems.MoveNext())
                        {
                            drp.Items.Add(new ListItem(enumItems.Current.Title, Convert.ToString(enumItems.Current[GlobalLookupFieldConstants.Id])));
                        }
                    }
                }
            }
        }
        public static void BindDropDownValuesWithColumnFilter(DropDownList drp, string listName, string webUrl, string ColumnName, string filterValue = "Yes")
        {
            using (var site = new SPSite(webUrl))
            {
                using (var spweb = site.OpenWeb())
                {
                    var spList = spweb.Lists.TryGetList(listName);
                    if (spList != null)
                    {
                        var fields = new string[] { GlobalLookupFieldConstants.Id, GlobalLookupFieldConstants.Title, GlobalLookupFieldConstants.Active, GlobalLookupFieldConstants.LookupValue, ColumnName };
                        var items = spList.GetItems(fields).Cast<SPListItem>().Where(x => Convert.ToBoolean(x[GlobalLookupFieldConstants.Active]).Equals(true) && x[ColumnName].Equals("Yes")).OrderBy(y => y.Title);
                        var enumItems = items.GetEnumerator();
                        drp.ClearSelection();
                        while (enumItems.MoveNext())
                        {
                            drp.Items.Add(new ListItem(enumItems.Current.Title, Convert.ToString(enumItems.Current[GlobalLookupFieldConstants.LookupValue])));
                        }
                    }
                }
            }
        }
        public static void BindListBoxItems(ListBox lb, string listName, string webUrl)
        {
            using (var site = new SPSite(webUrl))
            {
                using (var spweb = site.OpenWeb())
                {
                    var spList = spweb.Lists.TryGetList(listName);
                    if (spList != null)
                    {
                        var fields = new string[] { GlobalLookupFieldConstants.Id, GlobalLookupFieldConstants.Title, GlobalLookupFieldConstants.Active };
                        var items = spList.GetItems(fields).Cast<SPListItem>().Where(x => Convert.ToBoolean(x[GlobalLookupFieldConstants.Active]).Equals(true)).OrderBy(y => y.Title);
                        var enumItems = items.GetEnumerator();
                        lb.ClearSelection();
                        while (enumItems.MoveNext())
                        {
                            lb.Items.Add(new ListItem(enumItems.Current.Title, Convert.ToString(enumItems.Current[GlobalLookupFieldConstants.Id])));
                        }
                    }
                }
            }
        }
        public static void BindDropDownItemsById(DropDownList drp, string listName, string webUrl)
        {
            using (var site = new SPSite(webUrl))
            {
                using (var spweb = site.OpenWeb())
                {
                    var spList = spweb.Lists.TryGetList(listName);
                    if (spList != null)
                    {
                        var fields = new string[] { GlobalLookupFieldConstants.Id, GlobalLookupFieldConstants.Title, GlobalLookupFieldConstants.Active };
                        var items = spList.GetItems(fields).Cast<SPListItem>().Where(x => Convert.ToBoolean(x[GlobalLookupFieldConstants.Active]).Equals(true)).OrderBy(y => y.ID);
                        var enumItems = items.GetEnumerator();
                        drp.ClearSelection();
                        while (enumItems.MoveNext())
                        {
                            drp.Items.Add(new ListItem(enumItems.Current.Title, Convert.ToString(enumItems.Current[GlobalLookupFieldConstants.Id])));
                        }
                    }
                }
            }
        }
        public static void BindDropDownItemsByIdSortByValue(DropDownList drp, string listName, string webUrl)
        {
            using (var site = new SPSite(webUrl))
            {
                using (var spweb = site.OpenWeb())
                {
                    var spList = spweb.Lists.TryGetList(listName);
                    if (spList != null)
                    {
                        var fields = new string[] { GlobalLookupFieldConstants.Id, GlobalLookupFieldConstants.Title, GlobalLookupFieldConstants.Active, GlobalLookupFieldConstants.LookupValue };
                        var items = spList.GetItems(fields).Cast<SPListItem>().Where(x => Convert.ToBoolean(x[GlobalLookupFieldConstants.Active]).Equals(true)).OrderBy(y => y.ID);
                        var enumItems = items.GetEnumerator();
                        drp.ClearSelection();
                        while (enumItems.MoveNext())
                        {
                            drp.Items.Add(new ListItem(enumItems.Current.Title, Convert.ToString(enumItems.Current[GlobalLookupFieldConstants.LookupValue])));
                        }
                    }
                }
            }
        }
        public static void BindDropDownItemsByValueAndColumn(DropDownList drp, string listName, string columnName, string value, string webUrl)
        {
            using (var site = new SPSite(webUrl))
            {
                using (SPWeb spweb = site.OpenWeb())
                {
                    SPList spList = spweb.Lists.TryGetList(listName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where>" +
                                        "<And>" +
                                            "<Eq><FieldRef Name=\"" + columnName + "\" /><Value Type=\"Text\">" + value + "</Value></Eq>" +
                                            "<Eq><FieldRef Name=\"RoutingEnabled\" /><Value Type=\"Boolean\">1</Value></Eq>" +
                                        "</And>" +
                                    "</Where>" +
                                    "<OrderBy>" +
                                        "<FieldRef Name=\"Title\" Type=\"Text\"/>" +
                                    "</OrderBy>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        var fields = new string[] { GlobalLookupFieldConstants.Id, GlobalLookupFieldConstants.Title, GlobalLookupFieldConstants.Active };
                        drp.Items.Clear();
                        drp.Items.Add(new ListItem("Select...", "-1"));
                        foreach (SPListItem enumItems in compassItemCol)
                        {
                            drp.Items.Add(new ListItem(enumItems.Title, Convert.ToString(enumItems[GlobalLookupFieldConstants.Id])));
                        }
                    }
                }
            }
        }
        public static void BindDropDownItemsByValue(DropDownList drp, string listName, string value, string webUrl)
        {
            using (var site = new SPSite(webUrl))
            {
                using (SPWeb spweb = site.OpenWeb())
                {
                    SPList spList = spweb.Lists.TryGetList(listName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where>" +
                                        "<And>" +
                                            "<Eq><FieldRef Name=\"Compass_Value\" /><Value Type=\"Text\">" + value + "</Value></Eq>" +
                                            "<Eq><FieldRef Name=\"RoutingEnabled\" /><Value Type=\"Boolean\">1</Value></Eq>" +
                                        "</And>" +
                                    "</Where>" +
                                    "<OrderBy>" +
                                        "<FieldRef Name=\"Title\" Type=\"Text\"/>" +
                                    "</OrderBy>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        var fields = new string[] { GlobalLookupFieldConstants.Id, GlobalLookupFieldConstants.Title, GlobalLookupFieldConstants.Active };
                        drp.Items.Clear();
                        drp.Items.Add(new ListItem("Select...", "-1"));
                        foreach (SPListItem enumItems in compassItemCol)
                        {
                            //if ((enumItems.Current[GlobalLookupFieldConstants.LookupValue] != null) && (string.IsNullOrEmpty(Convert.ToString(enumItems.Current[GlobalLookupFieldConstants.LookupValue]))))
                            //    drp.Items.Add(new ListItem(enumItems.Current.Title, Convert.ToString(enumItems.Current[GlobalLookupFieldConstants.LookupValue])));
                            //else
                            drp.Items.Add(new ListItem(enumItems.Title, Convert.ToString(enumItems[GlobalLookupFieldConstants.Id])));
                        }
                    }
                }
            }
        }
        public static Boolean SetDropDownValue(string value, DropDownList drp, System.Web.UI.Page page)
        {
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    drp.ClearSelection();
                    drp.CssClass = String.Join(" ", drp.CssClass.Split(' ').Where(x => x != "WrongItemControl").ToArray());

                    ListItem listItem = drp.Items.Cast<ListItem>().First(i => i.Text.Equals(value, StringComparison.InvariantCultureIgnoreCase));
                    //drp.Items.FindByValue(listItem.Value);
                    drp.SelectedIndex = drp.Items.IndexOf(listItem);
                    //drp.Items.FindByText(value).Selected = true;
                }
                catch (Exception ex)
                {
                    drp.CssClass += " WrongItemControl";
                    drp.Items.Add(new ListItem(value, "-9999"));
                    drp.Items.FindByText(value).Selected = true;
                    drp.BackColor = System.Drawing.Color.Pink;
                    string strErrors = "'" + value + "' is not a valid value! Please select a new one.";
                    ErrorSummary.AddError(strErrors, page);
                    return false;
                }
            }

            return true;
        }
        public static Boolean SetDropDownValueMatchWithoutCodes(string value, DropDownList drp, System.Web.UI.Page page)
        {
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    drp.ClearSelection();
                    drp.CssClass = String.Join(" ", drp.CssClass.Split(' ').Where(x => x != "WrongItemControl").ToArray());
                    ListItem listItem = drp.Items.Cast<ListItem>().First(i => i.Text.Equals(value, StringComparison.InvariantCultureIgnoreCase) || i.Text.ToLower().Contains(value.ToLower()));
                    //drp.Items.FindByValue(listItem.Value);
                    drp.SelectedIndex = drp.Items.IndexOf(listItem);
                    //drp.Items.FindByText(value).Selected = true;
                }
                catch (Exception ex)
                {
                    drp.CssClass += " WrongItemControl";
                    drp.Items.Add(new ListItem(value, "-9999"));
                    drp.Items.FindByText(value).Selected = true;
                    drp.BackColor = System.Drawing.Color.Pink;
                    string strErrors = "'" + value + "' is not a valid value! Please select a new one.";
                    ErrorSummary.AddError(strErrors, page);
                    return false;
                }
            }

            return true;
        }
        public static Boolean SetDropDownValueById(string id, DropDownList drp, System.Web.UI.Page page)
        {
            try
            {
                drp.ClearSelection();
                drp.SelectedValue = id;
            }
            catch (Exception ex)
            {
                drp.Items.Add(id);
                drp.Items.FindByText(id).Selected = true;
                drp.BackColor = System.Drawing.Color.Pink;
                string strErrors = "'" + id + "' is not a valid value! Please select a new one.";
                ErrorSummary.AddError(strErrors, page);
                return false;
            }
            return true;
        }
        public static string GetDropDownDisplayValue(string realValue)
        {
            if (string.Equals(realValue, GlobalConstants.LIST_NoSelectionText))
                return string.Empty;
            else
                return realValue;
        }
        #endregion
        #region Date Methods
        static readonly DateTime DATETIME_MIN = new DateTime(1900, 1, 1);

        /// <summary>
        /// Setter for setting the DateTime and dealing with a string interface to that date.
        /// Deals with null dates.
        /// </summary>
        /// <param name="oldValue">the value of the current date</param>
        /// <param name="newValue">the value of the new date in a string</param>
        /// <returns>The DateTime representation of the passed string date</returns>
        public static DateTime SetDate(DateTime oldValue, string newValue)
        {
            try
            {
                if (newValue == null || newValue == string.Empty)
                    return (Convert.ToDateTime(null));
                else
                {
                    DateTime dt = DateTime.Parse(newValue);
                    if (dt < DateTime.MinValue)
                        dt = DateTime.MinValue;
                    return dt;
                }
            }
            catch (Exception)
            {
                return (Convert.ToDateTime(null));
            }
        }

        /// <summary>
        /// Getter for getting the DateTime as a string and dealing with null dates.
        /// </summary>
        /// <param name="currentValue">the current value date</param>
        /// <returns>The DateTime returned as a string</returns>
        public static string GetDateForDisplay(DateTime currentValue)
        {
            if (currentValue == Convert.ToDateTime(null))
                return string.Empty;
            else if (currentValue.Equals(DATETIME_MIN))
                return string.Empty;
            else if (currentValue.Equals("1/1/0001"))
                return string.Empty;
            else
                return currentValue.ToShortDateString();
        }
        public static DateTime GetDateFromField(string currentValue)
        {
            if (!string.IsNullOrEmpty(currentValue))
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(currentValue);
                    return dt;
                }
                catch (Exception ex)
                {
                    return Utilities.GetMinDate();
                }
            }

            return Utilities.GetMinDate();
        }
        public static DateTime GetMinDate()
        {
            // Sharepoint only accepts value between 1/1/1900 and 12/31/8900
            return DATETIME_MIN;
        }
        #endregion
        public static bool GetConfigurationValue(SPWeb web, string keyName)
        {
            var lists = web.Lists;
            var list = lists.TryGetList(GlobalConstants.LIST_Configurations);

            if (list != null)
            {
                foreach (SPListItem spListItem in list.Items)
                {
                    if (spListItem.Title.Trim().ToUpperInvariant() == keyName.Trim().ToUpperInvariant() &&
                        (spListItem["Value"].ToString().Trim().ToLowerInvariant() == "yes" || spListItem["Value"].ToString().Trim().ToLowerInvariant() == "1" || spListItem["Value"].ToString().Trim().ToLowerInvariant() == "true"))
                        return true;
                }
            }
            return false;
        }
        #region Redirect Methods
        public static string RedirectPageValue(string formName)
        {
            return RedirectPageValue(formName, null);
        }
        public static string RedirectPageValue(string formName, List<KeyValuePair<string, string>> parameters)
        {
            StringBuilder parametersStr = null;
            if (parameters != null)
            {
                parametersStr = new StringBuilder();
                foreach (KeyValuePair<string, string> kvp in parameters)
                    if (parametersStr.Length == 0)
                        parametersStr.Append("?" + kvp.Key + "=" + kvp.Value);
                    else
                        parametersStr.Append("&" + kvp.Key + "=" + kvp.Value);
            }
            if (parameters == null)
                return string.Format(string.Concat(SPContext.Current.Web.Url, "/Pages/{0}"), formName);
            else
                return string.Format(string.Concat(SPContext.Current.Web.Url, "/Pages/{0}{1}"), formName, parametersStr.ToString());
        }
        public static string RedirectPageForm(string formName, string projectNo)
        {
            var redirectUrl = string.Format(string.Concat(SPContext.Current.Web.Url, "/Pages/{0}?", GlobalConstants.QUERYSTRING_ProjectNo, "=", projectNo), formName);
            return redirectUrl;
        }
        public static string RedirectPageValueFirstSave(string formName, string projectNo)
        {
            var redirectUrl = string.Format(string.Concat(SPContext.Current.Web.Url, "/Pages/{0}?", GlobalConstants.QUERYSTRING_ProjectNo, "=", projectNo, "&Status=Saved"), formName);
            return redirectUrl;
        }
        public static string GetCurrentPageName()
        {
            return Path.GetFileName(System.Web.HttpContext.Current.Request.FilePath);
        }
        public static string RedirecttoHomePage()
        {
            string redirectUrl = string.Concat(SPContext.Current.Web.Url, "/Pages/TaskDashboard.aspx");

            return redirectUrl;
        }
        public static string RedirectChangeRequest(string projectNo)
        {
            var redirectUrl = string.Format(string.Concat(SPContext.Current.Web.Url, "/Pages/{0}?", GlobalConstants.QUERYSTRING_ProjectNo, "=", projectNo, "&IPFForm=Change"), GlobalConstants.PAGE_ItemProposal);
            return redirectUrl;
        }
        public static string RedirecttoProjectStatusPage(string projectNo)
        {
            string redirectUrl = string.Format(string.Concat(SPContext.Current.Web.Url, "/Pages/{0}?", GlobalConstants.QUERYSTRING_ProjectNo, "=", projectNo), GlobalConstants.PAGE_ProjectStatus);
            return redirectUrl;
        }
        #endregion
        #region Get Lookup
        public static string GetLookupValue(string listName, string title, string webUrl)
        {
            using (var site = new SPSite(webUrl))
            {
                using (var spweb = site.OpenWeb())
                {
                    var spList = spweb.Lists.TryGetList(listName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where>" +
                                        "<And>" +
                                            "<Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + title + "</Value></Eq>" +
                                            "<Eq><FieldRef Name=\"RoutingEnabled\" /><Value Type=\"Boolean\">1</Value></Eq>" +
                                        "</And>" +
                                     "</Where>";
                    spQuery.RowLimit = 1;

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        SPListItem item = compassItemCol[0];
                        return Convert.ToString(item[GlobalLookupFieldConstants.LookupValue]);
                    }
                }
            }

            return string.Empty;
        }
        public static string GetLookupDetailsByValueAndColumn(string receivingColumn, string listName, string passingColumn, string value, string webUrl)
        {
            string result = "";
            using (var site = new SPSite(webUrl))
            {
                using (SPWeb spweb = site.OpenWeb())
                {
                    SPList spList = spweb.Lists.TryGetList(listName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where>" +
                                        "<And>" +
                                            "<Eq><FieldRef Name=\"" + passingColumn + "\" /><Value Type=\"Text\">" + value + "</Value></Eq>" +
                                            "<Eq><FieldRef Name=\"RoutingEnabled\" /><Value Type=\"Boolean\">1</Value></Eq>" +
                                        "</And>" +
                                     "</Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        SPListItem item = compassItemCol[0];
                        result = Convert.ToString(item[receivingColumn]);
                    }
                }
            }
            return result;
        }
        public static string GetLookupDetailsByValueAndColumn(string receivingColumn, string listName, string passingColumn1, string value1, string passingColumn2, string value2, string webUrl)
        {
            string result = "";
            using (var site = new SPSite(webUrl))
            {
                using (SPWeb spweb = site.OpenWeb())
                {
                    SPList spList = spweb.Lists.TryGetList(listName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where>" +
                                        "<And>" +
                                            "<And>" +
                                                "<Eq><FieldRef Name=\"" + passingColumn1 + "\" /><Value Type=\"Text\">" + value1 + "</Value></Eq>" +
                                                "<Eq><FieldRef Name=\"" + passingColumn2 + "\" /><Value Type=\"Text\">" + value2 + "</Value></Eq>" +
                                            "</And>" +
                                            "<Eq><FieldRef Name=\"RoutingEnabled\" /><Value Type=\"Boolean\">1</Value></Eq>" +
                                        "</And>" +
                                    "</Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        SPListItem item = compassItemCol[0];
                        result = Convert.ToString(item[receivingColumn]);
                    }
                }
            }
            return result;
        }
        #endregion
        public static string DetermineWeeksToShip(DateTime shipDate)
        {
            string strWeeks = string.Empty;

            try
            {
                // Calculate the Weeks til Ship
                double days = (shipDate - DateTime.Now).TotalDays;
                days = Math.Round(days, 0);
                double weeks = days / 7;

                if (weeks <= 0)
                    strWeeks = "0";
                else
                    strWeeks = weeks.ToString("N1");
            }
            catch (Exception ex)
            {
            }

            return strWeeks;
        }
        #region Next Project Numbers
        public static string GetNextProjectNumber()
        {
            string projectNumber = string.Empty;
            string projectYear = DateTime.Now.Year.ToString();
            string value;

            var configurationService = DependencyResolution.DependencyMapper.Container.Resolve<IConfigurationManagementService>();
            SystemConfiguration configuration = configurationService.GetConfigurations();
            if (configuration.Configurations.TryGetValue("Projects-" + projectYear, out value))
            {
                projectNumber = projectYear + "-" + configurationService.GetConfigurationFromList("Projects-" + projectYear);
                // Update to next number
                try
                {
                    int nextNumber = Convert.ToInt32(configurationService.GetConfigurationFromList("Projects-" + projectYear));
                    nextNumber++;
                    configurationService.UpdateConfiguration("Projects-" + projectYear, nextNumber.ToString());
                }
                catch (Exception exception)
                {
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.IPF.ToString() + ": GetNextProjectNumber: " + exception.Message);
                }
            }
            else
            {
                // Configuration not found, need to add for current year
                projectNumber = projectYear + "-1";
                configurationService.CreateConfiguration("Projects-" + projectYear, "2");
            }

            return projectNumber;
        }
        public static string GetNextChildProjectNumber(string ParentProjectNumber)
        {
            string projectNumber = string.Empty;
            string projectYear = DateTime.Now.Year.ToString();
            string value;

            var configurationService = DependencyResolution.DependencyMapper.Container.Resolve<IConfigurationManagementService>();
            SystemConfiguration configuration = configurationService.GetConfigurations();
            if (configuration.Configurations.TryGetValue("Projects-" + ParentProjectNumber, out value))
            {
                projectNumber = ParentProjectNumber + "-" + configurationService.GetConfigurationFromList("Projects-" + ParentProjectNumber);
                // Update to next number
                try
                {
                    int nextNumber = Convert.ToInt32(configurationService.GetConfigurationFromList("Projects-" + ParentProjectNumber));
                    nextNumber++;
                    configurationService.UpdateConfiguration("Projects-" + ParentProjectNumber, nextNumber.ToString());
                }
                catch (Exception exception)
                {
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateGenerateIPF.ToString() + ": GetNextProjectNumber: " + exception.Message);
                }
            }
            else
            {
                // Configuration not found, need to add for current year
                projectNumber = ParentProjectNumber + "-1";
                configurationService.CreateConfiguration("Projects-" + ParentProjectNumber, "2");
            }

            return projectNumber;
        }
        #endregion
        #region deserialization
        // deserialization
        public static object Deserialize(Type type, string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    return xmldes.Deserialize(sr);
                }
            }
            catch (Exception e)
            {

                return null;
            }
        }
        // deserialization
        public static object Deserialize(Type type, Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(type);
            return xmldes.Deserialize(stream);
        }
        #endregion
        #region serialization
        // serialization
        public static string Serializer(Type type, object obj)
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);
            try
            {
                //Serialized object
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }
        #endregion
        #region Graphics Change Request Methods
        public static void UpdateProjectRejectionReason(int CompassListItemId, string ProjectCancelReasson, string Function)
        {
            // Get the current user
            SPUser currentUser = SPContext.Current.Web.CurrentUser;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectDecisionsListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem appItem = compassItemCol[0];
                            if (appItem != null)
                            {
                                appItem[CompassProjectDecisionsListFields.FunctionRejected] = Function;
                                appItem[CompassProjectDecisionsListFields.ReasonForRejection] = ProjectCancelReasson;
                                appItem[CompassProjectDecisionsListFields.RejectedBy] = SPContext.Current.Web.CurrentUser;
                                appItem[CompassProjectDecisionsListFields.RejectedByName] = SPContext.Current.Web.CurrentUser.Name.ToString(); ;
                                appItem[CompassProjectDecisionsListFields.CancellationReasons] = ProjectCancelReasson;
                                appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                appItem.Update();
                            }
                        }

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        #endregion
    }
}
