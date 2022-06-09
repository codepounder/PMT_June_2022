<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorldSyncNutritionalsForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.WorldSyncNutritionalsForm.WorldSyncNutritionalsForm" %>

<div class="container">
    <div class="row">
        <div id="divAccessDenied" runat="server" class="col-xs-12 col-sm-12 col-md-12 AccessDenied">
            WARNING: You do not have access to update this page. Save functionality will be disabled! <br />If you require access, please email the
            <asp:LinkButton ID="lbHelpDeskEmail" runat="server" CausesValidation="False" onclick="lbHelpDeskEmail_Click">HelpDesk</asp:LinkButton>.<br />
        </div>
    </div>
    <div class="row">
        <div id="divAccessRequest" runat="server" class="col-xs-12 col-sm-12 col-md-12 AccessRequest">
            Your request for access has been sent!
        </div>
    </div>
    <asp:HiddenField ID="hddCompassItemId" runat="server" />
    <asp:HiddenField ID="hddNutritionalId" runat="server" Value="0" />
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h1>Nutrient Information</h1>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 ValidationSummary">
            <asp:ValidationSummary ID="ItemValidationSummary" runat="server" Enabled="True" HeaderText="<strong><p style='font-size:medium'>Submission Failed!</p></strong><br/>Please correct the errors below:<br/>" />
            <div id="dverror_message" style="display:none;"> <ul id="error_message"></ul></div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Preparation State:</label>
                <asp:DropDownList ID="ddlPreparationState" runat="server" ClientIDMode="Static" CssClass="form-control required">
                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Nutrient Basis Quantity Type:</label>
                <asp:DropDownList ID="ddlNutrientBasisQuantityType" runat="server" ClientIDMode="Static" CssClass="form-control required">
                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <label class="control-label"><span class="markrequired">*</span>Serving Size:</label>
            <asp:TextBox ID="txtServingSize" runat="server" CssClass="form-control required numericNoMask" MaxLength="6" ClientIDMode="Static" ToolTip="Serving Size"></asp:TextBox>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <label class="control-label"><span class="markrequired">*</span>Serving Size UOM:</label>
            <asp:DropDownList ID="ddlServingSizeUOM" runat="server" ClientIDMode="Static" CssClass="form-control required" ToolTip="Serving Size">
                <asp:ListItem Value="-1">Select...</asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <label class="control-label"><span class="markrequired">*</span>Nutrient Basis Qty:</label>
            <asp:TextBox ID="txtNutrientBasisQty" runat="server" ClientIDMode="Static" CssClass="form-control required numericNoMask"  MaxLength="6" ToolTip="Nutrient Basis Quantity"></asp:TextBox>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <label class="control-label"><span class="markrequired">*</span>Nutrient Basis Qty UOM:</label>
            <asp:DropDownList ID="ddlNutrientBasisQuantityUOM" runat="server" ClientIDMode="Static" ToolTip="Nutrient Basis UOM" CssClass="form-control required">
                <asp:ListItem Value="-1">Select...</asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-9">
            <label class="control-label"><span class="markrequired">*</span>Serving Size Description:</label>
            <asp:TextBox ID="txtServingSizeDescription" runat="server" ClientIDMode="Static" CssClass="form-control required" ToolTip="Serving Size Description"></asp:TextBox>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <label class="control-label"><span class="markrequired">*</span>Number of Servings per Package:</label>
            <asp:TextBox ID="txtServingsPerPackage" runat="server" ClientIDMode="Static" CssClass="form-control required numericNoMask" ToolTip="Number of Servings per Package"></asp:TextBox>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <label class="control-label"><span class="markrequired">*</span>Ingredient Statement:</label>
            <asp:TextBox ID="txtIngredientStatement" runat="server" ClientIDMode="Static" CssClass="form-control required" ToolTip="Ingredient Statement"></asp:TextBox>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <label class="control-label"><span class="markrequired">*</span>Allergen Specification Agency:</label>
            <asp:TextBox ID="txtAllergenSpecificationAgency" runat="server" ClientIDMode="Static" CssClass="form-control required" ToolTip="Allergen Specification Agency"></asp:TextBox>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <label class="control-label"><span class="markrequired">*</span>Allergen Specification Name:</label>
            <asp:TextBox ID="txtAllergenSpecificationName" runat="server" ClientIDMode="Static" CssClass="form-control required" ToolTip="Allergen Specification Name"></asp:TextBox>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <label class="control-label"><span class="markrequired">*</span>Allergen Statement:</label>
            <asp:TextBox ID="txtAllergenStatement" runat="server" ClientIDMode="Static" CssClass="form-control required" ToolTip="Allergen Statement"></asp:TextBox>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>New Item Nutritionals</h2>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <table>
                <asp:Repeater ID="rptNutrientInfo" runat="server" OnItemDataBound="rptNutrientInfo_ItemDataBound" OnItemCommand="rptNutrientInfo_ItemCommand" >
                    <HeaderTemplate>
                        <tr>
                            <th>
                                <label class="control-label"><span class="markrequired">*</span>Nutrient Type Code</label>
                            </th>
                            <th>
                                <label class="control-label"><span class="markrequired">*</span>Nutrient Quantity Contained Measurement Precision</label>
                            </th>
                            <th>
                                <label class="control-label"><span class="markrequired">*</span>Nutrient Quantity Contained</label>
                            </th>
                            <th>
                                <label class="control-label"><span class="markrequired">*</span>Nutrient Quantity Contained UOM</label>
                            </th>
                            <th>
                                <label class="control-label"><span class="markrequired">*</span>Daily Value Intake Percent Measurement Precision Code</label>
                            </th>
                            <th>
                                <label class="control-label"><span class="markrequired">*</span>Percentage of Daily Value Intake</label>
                            </th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:DropDownList ID="drpNutrientType" CssClass="form-control NewNutRequired" Title="Nutrient Type Code" runat="server">
                                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                                </asp:DropDownList>
                                <asp:HiddenField ID="hdnItemID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />    
                            </td>
                            <td>
                                <asp:DropDownList ID="drpNutrientQtyContainedMeasPerc" CssClass="form-control NewNutRequired" Title="Nutrient Quantity Contained Measurement Precision" runat="server">
                                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNutrientQtyContained" runat="server" CssClass="form-control NewNutRequired" Title="Nutrient Quantity Contained" value='<%# DataBinder.Eval(Container.DataItem, "NutrientQtyContained") %>' ClientIDMode="Static"></asp:TextBox>
                            </td>
                            <td>
                                <asp:DropDownList ID="drpNutrientQtyContainedUOM" CssClass="form-control NewNutRequired" runat="server" Title="Nutrient Quantity Contained UOM">
                                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                                </asp:DropDownList>             
                            </td>
                            <td>
                                <asp:DropDownList ID="drpDailyValueIntakePct" CssClass="form-control NewNutRequired" runat="server" Title="Daily Value Intake Percent Measurement Precision Code">
                                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPctDailyValue" runat="server" CssClass="form-control NewNutRequired" Title="Percentage of Daily Value Intake" value='<%# DataBinder.Eval(Container.DataItem, "PctDailyValue") %>' ClientIDMode="Static"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btndelete" CausesValidation="false" CssClass="button" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' runat="server" Text="Delete" />  
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <asp:Button id="btnAddDetail" runat="server" onClick="addNutritionalDetail" OnClientClick="return ValidateNutritionals()" Text="Add" CssClass="btn" />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <asp:Label ID="lblSavedMessage" CssClass="SuccessMessage justifyRight" runat="server"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-1 col-md-1">
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn" OnClientClick="return ValidateData()" />
        </div>
        <div class="col-xs-12 col-sm-1 col-md-1">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSave_Click" CssClass="btn" OnClientClick="return ValidateData()" />
        </div>
    </div>
</div>
<script>
    var hddNutritionalDetailId = "hddNutritionalDetailId";
    var cmbNutrientType = "cmbNutrientType";
    var txtNutrientQuantity = "txtNutrientQuantity";
    var cmbNutrientQuantityType = "cmbNutrientQuantityType";
    var txtPercentageDailyValue = "txtPercentageDailyValue";
    var trHeaderNutritional = "trHeaderNutritional";
    var trAddNutritional = "trAddNutritional";
    var newNutritionalDetailId = "0";   
</script>