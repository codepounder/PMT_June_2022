<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPackagingEngineering.ascx.cs" Inherits="Ferrara.Compass.ControlTemplates.Ferrara.Compass.ucPackagingEngineering" %>

 <h3 class="accordion">Packing Component Details</h3>
    <div class="panel">
<div  class="container ucmain" id="dvMain" style="border:1px solid black;" ClientIDMode="Static" runat="server" >
 
   
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 ValidationSummary">
            <asp:ValidationSummary ID="ItemValidationSummary"  ClientIDMode="Static" CssClass="error" runat="server"  Enabled="True" HeaderText="<strong><p style='font-size:medium'>Submission Failed!</p></strong><br/>Please correct the errors below:<br/>" />
            <div id="dverror_messageuc" class="error" style="display:none;"> <ul id="error_messageuc"><li>Que tal, loko, loko</li></ul></div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <span class="markrequired">* </span><label class="control-label">Component Type:</label>
                 <asp:DropDownList ID="drpPkgComponent" CssClass="readOnly form-control requireduc" onchange="NewPackagingItem();" ClientIDMode="Static" runat="server" >
                      <asp:ListItem Text="--Select Packaging Component--" Value="-1"></asp:ListItem>
                 </asp:DropDownList>
                <asp:HiddenField ID="hdnSAPMatGroup"  ClientIDMode="Static" runat="server" />
                <asp:HiddenField ID="hdnComponentType"  ClientIDMode="Static" runat="server" />
                <asp:HiddenField ID="hdnPackagingType"  ClientIDMode="Static" runat="server" />
                 <asp:HiddenField ID="hdnParentID"  ClientIDMode="Static" runat="server" />
                
            </div>
        </div>
         <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
              <span class="markrequired">* </span>  <label class="control-label">New/Existing:</label>
                    <asp:DropDownList ID="drpNew"  onchange="BOMNewCondition(this);" ClientIDMode="Static" runat="server" ViewStateMode="Enabled"  CssClass="readOnly form-control requireduc">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="New" Value="New"></asp:ListItem>
                    <asp:ListItem Text="Existing" Value="Existing"></asp:ListItem>
                    </asp:DropDownList>
            </div>
        </div>
          <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
               <span class="markrequired">* </span> <label class="control-label">Pack Unit:</label>
                <asp:DropDownList ID="ddlPackUnit"  ClientIDMode="Static" CssClass="readOnly form-control requireduc" runat="server"  >       
                       <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
              <span class="markrequired">* </span>  <label class="control-label">Pack Quantity:</label>
                    <asp:TextBox ID="txtPackQty" CssClass="readOnly form-control minimumlength decimaltwoplaces requireduc numericDecimal3"  ClientIDMode="Static" runat="server" ></asp:TextBox>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanelBOMPE" runat="server"  >
<ContentTemplate>
     <div class="row">
          <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <asp:Label id="lblM" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                </div>
              </div>
         </div>
    <div class="row">
          <div class="col-xs-12 col-sm-6 col-md-2">
            <div class="form-group">
              <span class="markrequired">* </span> <label class="control-label">Component #:</label>
                    <asp:TextBox ID="txtMaterial" CssClass=" form-control minimumlength alphanumericToUpper1 requireduc Component" ViewStateMode="Enabled" MaxLength="20" ClientIDMode="Static" runat="server" ></asp:TextBox>
             </div>
        </div>
         <div class="col-xs-12 col-sm-6 col-md-2">
            <div class="form-group">
                <label class="control-label"></label>
                  <asp:Button ID="btnLookupSAPItemNumber" CssClass="ButtonControl hidebtn" runat="server" Text="Find" OnClientClick="getCompDescriptionBySAPBOMList('txtMaterial','txtMaterialDescription');return false;"  />
            </div>
        </div>
         <div class="col-xs-12 col-sm-6 col-md-8">
            <div class="form-group">
             <span class="markrequired">* </span>   <label class="control-label">Component Description:</label>
                  <asp:TextBox ID="txtMaterialDescription" CssClass=" form-control requireduc ComponentDesc"  ClientIDMode="Static" MaxLength="40" runat="server" ></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
       <div class="col-xs-12 col-sm-6 col-md-2">
            <div class="form-group">
               <span id="spanOldComponent" class="markrequired spanOldComponent">* </span> <label class="control-label">Like/Old Component #:</label>
                <asp:TextBox ID="txtLikeItem" CssClass="form-control minimumlength alphanumericToUpper1 requireduc OldComponent" ViewStateMode="Enabled" MaxLength="20"  ClientIDMode="Static" runat="server" ></asp:TextBox>
               
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-2">
            <div class="form-group">
                <label class="control-label"></label>
                 <asp:Button ID="btnOldComponet" CssClass="ButtonControl hidebtn" runat="server" Text="Find" OnClientClick="getCompDescriptionBySAPBOMList('txtLikeItem','txtLikeDescription');return false;"  />
            </div>
        </div>
          <div class="col-xs-12 col-sm-6 col-md-8">
            <div class="form-group">
             <span id="spanOldComponentDesc" class="markrequired spanOldComponentDesc">* </span>   <label class="control-label">Like/Old Component Description:</label>
                <asp:TextBox ID="txtLikeDescription" CssClass="form-control requireduc OldComponentDesc"  ClientIDMode="Static" runat="server" ></asp:TextBox>
            </div>
        </div>
    </div>
    </ContentTemplate>
        </asp:UpdatePanel>
    <div class="row transferSemi">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Make Location</h2>
        </div>
    </div>
    <div class="row transferSemi">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>

     <div class="row transferSemi">
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Make Location:</label>
                <asp:DropDownList ID="drpTSMakeLocation" ClientIDMode="Static" onChange="enableDisableCountry()" CssClass="form-control requireduc" runat="server">
                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <span class="markrequired COO">*</span><label class="control-label">Country of Origin:</label>
                <asp:DropDownList ID="drpTSCountryOfOrigin" ClientIDMode="Static" CssClass="form-control" runat="server">
                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Pack Location:</label>
                <asp:DropDownList ID="drpTSPackLocation" CssClass="form-control requireduc" runat="server">
                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
     </div>
    <div class="row candySemi">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Quality Assurance</h2>
        </div>
    </div>
    <div class="row candySemi">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>

     <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Trials completed and product set aside for pack trials?:</label>
                <asp:DropDownList ID="drpTrialsCompleted" ClientIDMode="Static" CssClass="form-control" runat="server">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">New Formula:</label>
                <asp:DropDownList ID="ddlNewFormula" ClientIDMode="Static" CssClass="requireduc form-control" runat="server">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row candySemi">
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Shelf Life:</label>
                <div class="input-group"><asp:TextBox ID="txtShelfLife" ClientIDMode="Static"  runat="server" CssClass="required form-control numericDecimal3"></asp:TextBox><span class="input-group-addon"> Days</span></div>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Kosher:</label>
                <asp:DropDownList ID="ddlKosher" CssClass="form-control required" runat="server">
                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Allergens:</label>
                <asp:TextBox ID="txtAllergen" ClientIDMode="Static" runat="server" CssClass="required form-control"></asp:TextBox>
            </div>
        </div>
     </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Notes</h2>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>

     <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <label class="control-label">Pack Location:</label>
              <asp:TextBox ID="TextBox2" TextMode="MultiLine" Rows="3"  ClientIDMode="Static" runat="server"  CssClass="readOnly form-control"></asp:TextBox>
            </div>
        </div>
      </div>
    <div class="row pe1">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row proc pe1">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group ">
               <span class="requiredpe2spanCond markrequired procprinter">* </span> <label class="control-label">Printer/Supplier:</label>
                <asp:DropDownList ID="ddlPrinter" ClientIDMode="Static" runat="server" Width="96%" CssClass="requiredproc requiredpe2Cond readOnly form-control" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                </asp:DropDownList>


            </div>
        </div>
         
        <div class="col-xs-12 col-sm-6 col-md-4" id="dvAddPrinter">
            <div class="form-group">
                <label class="control-label">Request new Printer/Supplier:</label>
                <asp:TextBox ID="txtNewPrinter" CssClass="readOnly form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
                
                <br />
                <asp:Label ID="lblPrinterRequestSent" ClientIDMode="Static" Visible="false" runat="server" CssClass="AccessRequest">Your request has been sent!</asp:Label>
                <asp:Label ID="lblPrinterError" ClientIDMode="Static" runat="server" CssClass="Error"></asp:Label>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-2">
             <label class="control-label"></label>
            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button readOnly" OnClientClick="return chkRequest('printer');" OnClick="btnAdd_Click"  />
        </div>
    </div>

    <div class="row proc pe1">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group ">
                <label class="control-label">Lead Time for Component:</label>
                <div class="input-group">
                <asp:TextBox ID="txtLeadTimeMaterial" CssClass="readOnly form-control alphanumericToUpper1" ClientIDMode="Static" runat="server"></asp:TextBox><span class="input-group-addon">Days</span>
            </div>
                </div>
        </div>
    </div>

     <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Notes</h2>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>

     <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <label class="control-label">  Notes:</label>
              <asp:TextBox ID="txtNotes" TextMode="MultiLine" Rows="3"  ClientIDMode="Static" runat="server"  CssClass="readOnly form-control"></asp:TextBox>
            </div>
        </div>
      </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
     <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-9" id="dvDimension">
            <div class="form-group">
                    <span class="requiredpe2spanCond markrequired spanDimension">* </span><label class="control-label">Dimensions in inches:</label>
                    <div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">L</span><asp:TextBox ID="txtFilmLength" Width="105" CssClass="Dimension requiredpe2Cond decimalfourplaces"  ClientIDMode="Static" runat="server"  ></asp:TextBox><span class="input-group-addon">in.</span>
                           <span class="dimension">X</span>
                                  </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">W</span><asp:TextBox ID="txtFilmWidth" Width="105"   ClientIDMode="Static" runat="server"  CssClass="Dimension requiredpe2Cond decimalfourplaces"></asp:TextBox><span class="input-group-addon">in.</span>
                             <span class="dimension">X</span>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">H</span><asp:TextBox ID="txtFilmHeight" Width="105"  ClientIDMode="Static" runat="server"  CssClass="Dimension requiredpe2Cond decimalfourplaces"></asp:TextBox><span class="input-group-addon">in.</span>
                            </div>
                        </div>
                    </div>
                </div>
        </div>
         <div id="dvBackSeam" class="col-xs-12 col-sm-6 col-md-3 labeluc pe2">
              <div class="form-group ">
            <span class="requiredpe2spanCond markrequired">* </span>    <label class="control-label">Back Seam:</label>
                  <asp:DropDownList ID="drpFilmBackSeam" ClientIDMode="Static" runat="server"  CssClass="requiredpe2Cond readOnly form-control">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                </asp:DropDownList>
             </div>
         </div>
    </div>

    <div class="row labeluc">
        <div class="col-xs-12 col-sm-6 col-md-6 pe2">
            <div class="form-group ">
               <span class="requiredpe2spanCond markrequired">* </span> <label class="control-label"> Web Width (in.):</label>
                <asp:TextBox ID="txtFilmWebWidth" CssClass="requiredpe2Cond decimaltwoplaces readOnly form-control" ClientIDMode="Static" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6 pe2">
           <div class="form-group ">
              <span class="requiredpe2spanCond markrequired">* </span>  <label class="control-label">Exact Cut Off (in.):</label>
               <asp:TextBox ID="txtFilmExactCutoff" CssClass="requiredpe2Cond decimaltwoplaces readOnly form-control" ClientIDMode="Static" runat="server" ></asp:TextBox>
            </div>
        </div>
    </div>
     <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group pe2">
              <span class="requiredpe2spanCond markrequired">* </span>  <label class="control-label">UnWind (in.):</label>
                 <asp:TextBox ID="txtFilmUnWind" CssClass="requiredpe2Cond decimaltwoplaces readOnly form-control" ClientIDMode="Static" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div id="dvBagFace" class="col-xs-12 col-sm-6 col-md-6 labeluc pe2">
           <div class="form-group ">
              <span class="requiredpe2spanCond markrequired">* </span>  <label class="control-label">Bag Face (in.):</label>
                 <asp:TextBox ID="txtFilmBagFace" CssClass="requiredpe2Cond decimaltwoplaces readOnly form-control" ClientIDMode="Static" runat="server" ></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6 pe2 labeluc">
            <div class="form-group ">
              <span class="requiredpe2spanCond markrequired">* </span>  <label class="control-label">Clear/White Substrate:</label>
                <asp:DropDownList ID="ddlFilmSubstrate" ClientIDMode="Static" CssClass="requiredpe2Cond readOnly form-control" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6 pe2 rigid">
           <div class="form-group ">
               <span class="requiredpe2spanCond markrequired">* </span><asp:Label ID="idPackComponent" CssClass="control-label" Font-Bold="true" runat="server" ClientIDMode="Static"></asp:Label> 
                <asp:DropDownList ID="ddlFilmStructure" runat="server" CssClass="requiredpe2Cond readOnly form-control" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>   
            </div>
        </div>
    </div>
   
     <div class="row pe2 rigid">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group ">
               <label class="control-label">Request new Substrate:</label>
                <asp:TextBox ID="txtNewSubstrate" CssClass="readOnly form-control" ClientIDMode="Static" runat="server"></asp:TextBox><br />
                                    <asp:Label ID="lblSubstrateRequestSent" runat="server" Visible="false" CssClass="AccessRequest">Your request has been sent!</asp:Label>
                                    <asp:Label ID="lblSubstrateError" ClientIDMode="Static" runat="server" CssClass="Error"></asp:Label>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-2">
            <div class="form-group ">
                 <label class="control-label"></label>
                 <asp:Button ID="btnRequestNewSubstrate" runat="server" Text="Add" CssClass="button readOnly" OnClientClick="return chkRequest('substrate');" OnClick="lbRequestNewSubstrate_Click"  />
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6" id="dvStructureColor">
           <div class="form-group ">
              <span class="requiredpe2spanCond markrequired">* </span>  <asp:Label id="idComponentColor" CssClass="control-label" Font-Bold="true" runat="server" ClientIDMode="Static">(<i>Packaging Component</i>) Structure Color:</asp:Label>
                <asp:DropDownList ID="ddlStructureColor"  ClientIDMode="Static" runat="server"  Width="96%" CssClass="requiredpe2Cond readOnly form-control" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>

    <div class="row pe2 labeluc">
        <div class="col-xs-12 col-sm-6 col-md-6 ">
            <div class="form-group">
             <span class="requiredpe2spanCond markrequired">* </span>   <label class="control-label">Max Roll (OD) (in.):</label>
                 <asp:TextBox ID="txtFilmMaxRollOD" CssClass="requiredpe2Cond decimaltwoplaces readOnly form-control" ClientIDMode="Static" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
           <div class="form-group">
              <span class="requiredpe2spanCond markrequired">* </span>  <label class="control-label">Roll (ID) (in.):</label>
              <asp:TextBox ID="txtFilmRollID" CssClass="requiredpe2Cond decimaltwoplaces readOnly form-control" ClientIDMode="Static" runat="server" ></asp:TextBox>
            </div>
        </div>
    </div>
     <div class="row labeluc">
        <div id="dvPrintStyle" class="col-xs-12 col-sm-6 col-md-6 ">
            <div class="form-group">
                <span class="markrequired">* </span> <label class="control-label">Print Style:</label>
                <asp:DropDownList ID="ddlFilmPrintStyle"  ClientIDMode="Static" runat="server"  Width="96%" CssClass="requireduc form-control" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6 pe2">
           <div class="form-group">
            <span class="requiredpe2spanCond markrequired">* </span> <label class="control-label">Style:</label>
                  <asp:DropDownList ID="ddlFilmStyle" runat="server" Width="96%" CssClass="requiredpe2Cond readOnly form-control" AppendDataBoundItems="true">
                     <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                 </asp:DropDownList>
            </div>
        </div>
    </div>

     <div class="row attachment">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Attachments</h2>
        </div>
    </div>
    <div class="row attachment">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row attachment">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">CAD Drawing Name:</label>
                <asp:TextBox ID="txtCadDrawingName" CssClass="readOnly form-control"  ClientIDMode="Static" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
           <div class="form-group">
              
            </div>
        </div>
    </div>

    <div  class="row attachment">
         <div class="col-xs-12 col-sm-3 col-md-4">
            <label class="control-label">CAD Drawing:</label>
        </div>
        <div class="col-xs-12 col-sm-9 col-md-4">  
            <input id="btnUploadCAD" runat="server" type="button" class="ButtonControlAutoSize readOnly hidebtn" value="Upload CAD Drawing" onclick="OpenDialog('Upload CAD Drawing', 'CADDrawing'); return false;" />  
        </div>
    </div>
    <div class="row attachment">
        <div class="col-xs-12 col-sm-12 col-md-6">
            <table width="50%">
                <asp:Repeater ID="rptCADDrawing"  ClientIDMode="Static" runat="server" >
                    <HeaderTemplate>
                        <tr>
                            <th>Action</th>
                            <th>Document Name</th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnkNLEADelete"  ClientIDMode="Static" runat="server"  Text="Delete" CommandName='<%#Eval("FileUrl") %>' OnClientClick='javascript:return confirm("Are you sure you want to delete?")' OnClick="lnkDeleteAttachment_Click" CausesValidation="false"></asp:LinkButton>
                            </td>
                            <td>
                                <a target='_blank' href='<%#Eval("FileUrl") %>'><%#Eval("DisplayFileName")%></a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <p class="comment-block"><label id="lblItemNote" class="comment-block">Maximum two documents.</label></p>
        </div>
          
    </div>

    
    <div class="row attachment">
         <div class="col-xs-12 col-sm-3 col-md-4">
            <label class="control-label">Visual Reference/Rendering:</label>
        </div>
        <div class="col-xs-12 col-sm-9 col-md-4">  
            <input id="btnReference" type="button" runat="server" class="ButtonControlAutoSize readOnly hidebtn" value="Upload Visual Reference/Rendering" onclick="OpenDialog('Upload Visual Reference/Rendering', 'Rendering'); return false;" />  
        </div>
    </div>

    <div class="row attachment">
        <div class="col-xs-12 col-sm-12 col-md-6">
            <table width="50%">
                <asp:Repeater ID="rptVisualReference"  ClientIDMode="Static" runat="server" >
                    <HeaderTemplate>
                        <tr>
                            <th>Action</th>
                            <th>Document Name</th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnkNLEADelete"  ClientIDMode="Static" runat="server"  Text="Delete" CommandName='<%#Eval("FileUrl") %>' OnClientClick='javascript:return confirm("Are you sure you want to delete?")' OnClick="lnkDeleteAttachment_Click" CausesValidation="false"></asp:LinkButton>
                            </td>
                            <td>
                                <a target='_blank' href='<%#Eval("FileUrl") %>'><%#Eval("DisplayFileName")%></a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <asp:Button ID="btnReloadAttachment" runat="server" Text="Load Attachments" CssClass="ReloadAttachment HiddenButton readOnly hidebtn" OnClick="btnReloadAttachment_Click"  ></asp:Button>
               <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
              <asp:HiddenField ID="hdnPackagingItemId" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnRequiredCheck" ClientIDMode="Static" runat="server" />
        </div>
    </div>
    
      <div class="row">
        <asp:UpdatePanel ID="UpdatePanel4" runat="server"  >
        <ContentTemplate>
            <div class="col-xs-12 col-sm-6 col-md-8">
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button justifyRight" OnClick="btnCancel_Click" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
             <asp:Button ID="btnSaveValidate" OnClientClick="return validateuc('#dverror_messageuc', '#error_messageuc');" runat="server"  CssClass="ButtonControlAutoSize justifyRight hidebtn" Text="Save and Validate" OnClick="btnSaveValidate_Click"  />
            </div>
            <div class="colId-xs-12 col-sm-6 col-md-2">
                <asp:Button ID="btnSave" runat="server" OnClientClick="return checkrequired();"  CssClass="clickSave button justifyRight hidebtn"  Text="Save" OnClick="btnSave_Click" />
            </div>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
        </div>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        pageLoadPackagingItem();
        var componentType = $("#drpPkgComponent option:selected").text();
        showHideMakeLocationSection();
        if (componentType.indexOf('Candy Semi') != -1) {
            $(".candySemi").show();
            $(".candySemi").find("select,input").addClass("requireduc");
        } else {
            $(".candySemi").hide();
            $(".candySemi").find(".requireduc").removeClass("requireduc");
        }
        $(".numericNoMask").keypress(function (event) {
            // Backspace, tab, enter, end, home, left, right
            // We don't support the del key in Opera because del == . == 46.
            var controlKeys = [8, 9, 13, 35, 36, 37, 39];
            // IE doesn't support indexOf
            var isControlKey = controlKeys.join(",").match(new RegExp(event.which));
            // Some browsers just don't raise events for control keys. Easy.
            // e.g. Safari backspace.
            if (!event.which || // Control keys in most browsers. e.g. Firefox tab is 0
                (48 <= event.which && event.which <= 57) || // Always 1 through 9
                //    (48 == event.which && $(this).attr("value")) || // No 0 first digit
                (isControlKey || $(this).val().indexOf('.') != -1)) { // Opera assigns values for control keys.
                return;
            } else {
                event.preventDefault();
            }
        });
        /******* Following line is to disable Enter **************/
        document.onkeypress = stopRKey;
    });

    
    function chkLength(ctrlId, lengthReq)
    {
        return checkNumberLength(ctrlId, lengthReq);
    }

    function checkrequired() {
        var chkRequired = validateucSave();
        if (chkRequired)
            $("#hdnRequiredCheck").val("Completed");
        else
            $("#hdnRequiredCheck").val("Waiting");

   //     alert($("#hdnRequiredCheck").val());
        return true;
    }
    
    function BOMNewCondition() {
        var url = window.location.href.toLocaleLowerCase();
        
        if ($("#drpNew option:selected").text() == 'Select...') {
            BOMNewConditionIfNoSelection();

            if (url.indexOf('bomsetuppe2.aspx') != -1) {
                $('.requiredpe2spanCond').each(function (i, obj) {
                    $(this).addClass("hideItem");
                });

                $('.requiredpe2Cond').each(function (i, obj) {
                    $(this).removeClass("requiredpe2");
                });
            }
        }
        else if ($("#drpNew option:selected").text() == 'New') {

            $(".procprinter").show();
            $("#ddlPrinter").addClass("requiredproc");

            BOMNewConditionIfNew(true);

            if (url.indexOf('bomsetuppe2.aspx') != -1) {
                $('.requiredpe2spanCond').each(function (i, obj) {
                    $(this).removeClass("hideItem");
                });
                $('.requiredpe2Cond').each(function (i, obj) {
                    $(this).addClass("requiredpe2");
                });
            }
            if (url.indexOf('bomsetupproc.aspx') != -1) {
                pageLoadPackagingItem();
                var componentType = $("#drpPkgComponent option:selected").text();
                showHideMakeLocationSection();
                if (componentType.indexOf('Candy Semi') != -1) {
                    $(".candySemi").show();
                    $(".candySemi").find("select,input").addClass("requireduc");
                } else {
                    $(".candySemi").hide();
                    $(".candySemi").find(".requireduc").removeClass("requireduc");
                }
            }
        }
        else if ($("#drpNew option:selected").text() == 'Existing') {
            BOMNewConditionIfExisting(true);
            $(".procprinter").hide();
            $("#ddlPrinter").removeClass("requiredproc");

            if (url.indexOf('bomsetuppe2.aspx') != -1) {
                $('.requiredpe2spanCond').each(function (i, obj) {
                    $(this).addClass("hideItem");
                });

                $('.requiredpe2Cond').each(function (i, obj) {
                    $(this).removeClass("requiredpe2");
                });
            }
        }
    }

    function OpenDialog(arg1, arg2)
    {
       // alert('in');
        var packId = $("#hdnPackagingItemId").val();
        //alert(packId);
        openBasicDialogWithpackagingItemId(arg1, arg2, packId);
    }
</script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Format.js"></script>