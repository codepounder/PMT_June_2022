using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;

namespace Ferrara.Compass.Classes
{
    public class SetupUtilities
    {
        public static bool CreateFieldInWeb(SPWeb web, string fieldName, string displayFieldName, string fieldType, bool isRequired)
        {

            var fieldExist = web.Fields.ContainsFieldWithStaticName(fieldName);
            if (!fieldExist)
            {

                var strNewField = "<Field Type=\"" + fieldType + "\" " +
                                     "DisplayName=\"" + displayFieldName + "\" " +
                                     "Group=\"" + "Ferrara Columns" + "\" " +
                                     "Required=\"" + isRequired + "\" Name=\"" + fieldName + "\">" +
                                     "</Field>";

                web.Fields.AddFieldAsXml(strNewField, true, SPAddFieldOptions.AddFieldInternalNameHint);
                web.Update();
                return true;
            }
            return false;
        }

        public static bool CreateFieldDateTime(SPList spList, string fieldName, string displayFieldName, bool isRequired, bool isDateOnly)
        {

            bool fieldExist = spList.Fields.ContainsFieldWithStaticName(fieldName);
            if (!fieldExist)
            {

                var strNewField = "<Field Type=\"" + SPFieldType.DateTime + "\" ";

                if (isDateOnly)
                    strNewField += "Format=\"DateOnly\" ";

                strNewField += "DisplayName=\"" + displayFieldName + "\" " +
                                     "Required=\"" + isRequired + "\" Name=\"" + fieldName + "\">" +
                                     "</Field>";

                spList.Fields.AddFieldAsXml(strNewField, true, SPAddFieldOptions.AddFieldInternalNameHint);
                return true;
            }
            return false;
        }

        public static bool CreateField(SPList spList, string fieldName, string displayFieldName, SPFieldType fieldType, bool isRequired)
        {

            var fieldExist = spList.Fields.ContainsFieldWithStaticName(fieldName);
            if (!fieldExist)
            {

                var strNewField = "<Field Type=\"" + fieldType + "\" " +
                                     "DisplayName=\"" + displayFieldName + "\" " +
                                     "Required=\"" + isRequired + "\" Name=\"" + fieldName + "\">" +
                                     "</Field>";

                spList.Fields.AddFieldAsXml(strNewField, true, SPAddFieldOptions.AddFieldInternalNameHint);
                return true;
            }
            return false;
        }

        public static bool CreateFieldNote(SPList spList, string fieldName, string displayFieldName, bool isRequired, bool isAppendOnly, bool isRichText)
        {

            bool fieldExist = spList.Fields.ContainsFieldWithStaticName(fieldName);
            if (!fieldExist)
            {

                var strNewField = "<Field Type=\"Note\" " +
                                     "DisplayName=\"" + displayFieldName + "\" " +
                                     "Required=\"" + isRequired + "\" Name=\"" + fieldName + "\">" +
                                     "</Field>";

                spList.Fields.AddFieldAsXml(strNewField, true, SPAddFieldOptions.AddFieldInternalNameHint);

                var field = (SPFieldMultiLineText)spList.Fields[displayFieldName];
                field.AppendOnly = isAppendOnly;
                if (isRichText)
                {
                    field.RichTextMode = SPRichTextMode.FullHtml;
                }
                field.Update();
                return true;
            }
            return false;
        }

        public static bool CreateFieldChoice(SPList spList, string fieldName, string displayFieldName, bool isRequired, List<string> choiceValues)
        {

            var fieldExist = spList.Fields.ContainsFieldWithStaticName(fieldName);
            if (!fieldExist)
            {
                var strNewField = "<Field Type=\"Choice\" " +
                                     "DisplayName=\"" + displayFieldName + "\" " +
                                     "Required=\"" + isRequired + "\" Name=\"" + fieldName + "\">" +
                                     "</Field>";

                spList.Fields.AddFieldAsXml(strNewField, true, SPAddFieldOptions.AddFieldInternalNameHint);

                var choiceField = (SPFieldChoice)spList.Fields[displayFieldName];
                choiceValues.ForEach(c => choiceField.Choices.Add(c));
                choiceField.Update();

                return true;
            }
            return false;
        }

        public static bool CreateFieldGenericLookupField(SPList spList, string fieldName, string displayFieldName, string mapto)
        {

            var fieldExist = spList.Fields.ContainsFieldWithStaticName(fieldName);
            if (!fieldExist)
            {

                var strNewField = "<Field Type=\"GenericLookupField\" " +
                                     "DisplayName=\"" + displayFieldName + "\" " +
                                     "Required=\"FALSE\" Name=\"" + fieldName + "\">" +
                                      "<CHOICES />" +
                                      "<Customization>" +
                                     "<ArrayOfProperty>" +
                                     "<Property>" +
                                     "<Name>GenericLookupSource</Name>" +
                                     "<Value xmlns:q1=\"http://www.w3.org/2001/XMLSchema\" p4:type=\"q1:string\" xmlns:p4=\"http://www.w3.org/2001/XMLSchema-instance\">"
                                     + mapto + "</Value>" +
                                     "</Property>" +
                                     "</ArrayOfProperty>" +
                                     "</Customization>" +
                                      "</Field>";

                spList.Fields.AddFieldAsXml(strNewField, false, SPAddFieldOptions.AddFieldInternalNameHint);
                return true;
            }
            return false;
        }

        public static bool CreateLookupField(SPList spList, SPList lookupList, string fieldName, string displayFieldName, string lookupColumn, string mapto)
        {
            var fieldExist = spList.Fields.ContainsFieldWithStaticName(fieldName);
            if (!fieldExist)
            {

                var strNewField = "<Field Type=\"GenericLookupField\" " +
                                     "DisplayName=\"" + displayFieldName + "\" " +
                                     "Required=\"FALSE\" Name=\"" + fieldName + "\">" +
                                      "<CHOICES />" +
                                      "<Customization>" +
                                     "<ArrayOfProperty>" +
                                     "<Property>" +
                                     "<Name>GenericLookupSource</Name>" +
                                     "<Value xmlns:q1=\"http://www.w3.org/2001/XMLSchema\" p4:type=\"q1:string\" xmlns:p4=\"http://www.w3.org/2001/XMLSchema-instance\">"
                                     + mapto + "</Value>" +
                                     "</Property>" +
                                     "</ArrayOfProperty>" +
                                     "</Customization>" +
                                      "</Field>";

                spList.Fields.AddFieldAsXml(strNewField, false, SPAddFieldOptions.AddFieldInternalNameHint);
                return true;
            }
            return false;
        }

        public static bool CreateFieldCalculated(SPList spList, string fieldName, string displayFieldName, SPFieldType resultType, string formula, string fieldRefInternalName)
        {

            var fieldExist = spList.Fields.ContainsFieldWithStaticName(fieldName);
            if (!fieldExist)
            {
                var strNewField = "<Field Type=\"Calculated\" " +
                           "Name=\"" + fieldName + "\" " +
                           "DisplayName=\"" + displayFieldName + "\" " +
                           "ResultType=\"" + resultType + "\" " +
                           "ReadOnly=\"TRUE\" > " +
                           "<Formula>" + formula + "</Formula>" +
                           "<FieldRefs><FieldRef Name=\"" + fieldRefInternalName + "\" /></FieldRefs>" +
                           "</Field>";

                spList.Fields.AddFieldAsXml(strNewField, false, SPAddFieldOptions.AddFieldInternalNameHint);
                return true;
            }
            return false;
        }

        public static bool CreateFieldGenericLookupChoiceField(SPList spList, string fieldName, string displayFieldName, string mapto)
        {

            var fieldExist = spList.Fields.ContainsFieldWithStaticName(fieldName);
            if (!fieldExist)
            {

                var strNewField = "<Field Type=\"GenericLookupFieldChoice\" " +
                                     "DisplayName=\"" + displayFieldName + "\" " +
                                     "Required=\"FALSE\" Name=\"" + fieldName + "\">" +
                                      "<CHOICES />" +
                                      "<Customization>" +
                                     "<ArrayOfProperty>" +
                                     "<Property>" +
                                     "<Name>GenericLookupChoiceSource</Name>" +
                                     "<Value xmlns:q1=\"http://www.w3.org/2001/XMLSchema\" p4:type=\"q1:string\" xmlns:p4=\"http://www.w3.org/2001/XMLSchema-instance\">"
                                     + mapto + "</Value>" +
                                     "</Property>" +
                                     "</ArrayOfProperty>" +
                                     "</Customization>" +
                                      "</Field>";

                spList.Fields.AddFieldAsXml(strNewField, false, SPAddFieldOptions.AddFieldInternalNameHint);
                return true;
            }
            return false;
        }

        public static bool CreateNumberField(SPList spList, string fieldName, string displayFieldName, bool isRequired, SPNumberFormatTypes formatType)
        {

            var fieldExist = spList.Fields.ContainsFieldWithStaticName(fieldName);
            if (!fieldExist)
            {

                var strNewField = "<Field Type=\"" + SPFieldType.Number + "\" " +
                                     "DisplayName=\"" + displayFieldName + "\" " +
                                     "Required=\"" + isRequired + "\" Name=\"" + fieldName + "\">" +
                                     "</Field>";

                spList.Fields.AddFieldAsXml(strNewField, true, SPAddFieldOptions.AddFieldInternalNameHint);

                var field = (SPFieldNumber)spList.Fields[displayFieldName];
                field.DisplayFormat = formatType;
                field.Update();
                return true;
            }
            return false;
        }

        public static bool CreateCurrencyField(SPList spList, string fieldName, string displayFieldName, bool isRequired, SPNumberFormatTypes formatType)
        {

            var fieldExist = spList.Fields.ContainsFieldWithStaticName(fieldName);
            if (!fieldExist)
            {

                var strNewField = "<Field Type=\"" + SPFieldType.Currency + "\" " +
                                     "DisplayName=\"" + displayFieldName + "\" " +
                                     "Required=\"" + isRequired + "\" Name=\"" + fieldName + "\">" +
                                     "</Field>";

                spList.Fields.AddFieldAsXml(strNewField, true, SPAddFieldOptions.AddFieldInternalNameHint);

                var field = (SPFieldCurrency)spList.Fields[displayFieldName];
                field.DisplayFormat = formatType;
                //field.CurrencyLocaleId = currencyType;
                field.Update();
                return true;
            }
            return false;
        }

        public static SPList CreateList(SPWeb currentWeb, string listName, string description)
        {
            string cleanListName = listName.Replace(" ", "");
            currentWeb.Lists.Add(cleanListName, description, SPListTemplateType.GenericList);
            SPList newList = currentWeb.Lists.TryGetList(cleanListName);
            newList.Title = listName;
            newList.Update();
            return newList;
        }

        public static SPList CreateDocumentLibrary(SPWeb currentWeb, string listName, string description)
        {
            string cleanListName = listName.Replace(" ", "");
            currentWeb.Lists.Add(cleanListName, description, SPListTemplateType.DocumentLibrary);
            SPList newList = currentWeb.Lists.TryGetList(cleanListName);
            newList.Title = listName;
            newList.Update();
            return newList;
        }
        public static bool ListExists(SPWeb web, string listName)
        {
            var lists = web.Lists;
            var list = lists.TryGetList(listName);
            return list != null;
        }
    }
}
