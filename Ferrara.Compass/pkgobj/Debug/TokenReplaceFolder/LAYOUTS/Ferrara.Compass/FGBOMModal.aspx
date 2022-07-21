<%@ Assembly Name="Ferrara.Compass, Version=1.0.0.0, Culture=neutral, PublicKeyToken=04ae2c9e0ea4efe6" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FGBOMModal.aspx.cs" Inherits="Ferrara.Compass.Layouts.Ferrara.Compass.FGBOMModal" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
     <div class="row">
             
              <div class="row">
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="form-group">
                                <label class="control-label"><span class="markrequired">*</span>Component Type</label>
                                <select name="ctl00$ctl40$g_2c83c192_5d5a_4da0_91e1_8436e99db50c$FinishedGood$rptTSItem$ctl00$drpComponent" id="ctl00_ctl40_g_2c83c192_5d5a_4da0_91e1_8436e99db50c_FinishedGood_rptTSItem_ctl00_drpComponent" class="PCBOMrequired form-control drpComponentType VerifySAPNumbersType" onchange="drpCompType_changed(this);">
										<option value="-1">Select...</option>
										<option value="1">Ancillary Packaging</option>
										<option value="2">Candy Semi</option>
										<option value="3">Corrugated - Carton</option>
										<option value="4">Corrugated - Coman</option>
										<option value="5">Corrugated - Corner Posts</option>
										<option value="6">Corrugated - Display (Base)</option>
										<option value="7">Corrugated - Display (Body)</option>
										<option value="8">Corrugated - Display (Header)</option>
										<option value="9">Corrugated - Display (Shelves)</option>
										<option value="10">Corrugated - Display RSC</option>
										<option value="11">Corrugated - Display Tray</option>
										<option value="12">Corrugated - Divider</option>
										<option selected="selected" value="13">Corrugated - DRC</option>
										<option value="14">Corrugated - Generic RSC</option>
										<option value="15">Corrugated - HSC</option>
										<option value="16">Corrugated - Inserts</option>
										<option value="17">Corrugated - Pallet Caps/Tray</option>
										<option value="18">Corrugated - RSC</option>
										<option value="19">Corrugated – Shield DRC</option>
										<option value="20">Corrugated - Shroud</option>
										<option value="21">Corrugated - Slip Sheets</option>
										<option value="22">Corrugated - Tote</option>
										<option value="23">Corrugated - Tote Cap</option>
										<option value="24">Corrugated - Wraparound Display Tray</option>
										<option value="25">Corrugated - Wraparound RSC</option>
										<option value="26">Film - Clear Overwrap</option>
										<option value="27">Film - DOY</option>
										<option value="28">Film - Gusseted (Non Reseal)</option>
										<option value="29">Film - Gusseted (Reseal)</option>
										<option value="30">Film - Lay Down</option>
										<option value="31">Film - Other</option>
										<option value="32">Film - Peg Bag</option>
										<option value="33">Film - Piece Flow Wraps</option>
										<option value="34">Film - Piece Twist Wraps</option>
										<option value="35">Film - Pouch</option>
										<option value="37">Film - Printed Overwrap (Non Reseal)</option>
										<option value="38">Film - Printed Overwrap (Reseal)</option>
										<option value="36">Film - Sleeve (over U-Board)</option>
										<option value="39">Film - Slug</option>
										<option value="40">Film - Zipper</option>
										<option value="41">Finished Good (Internal)</option>
										<option value="42">Finished Good (Purchased)</option>
										<option value="43">Label - Case</option>
										<option value="44">Label - Pallet</option>
										<option value="45">Label - Pie Crust</option>
										<option value="46">Label - PricePoint Sticker</option>
										<option value="48">Label - Reinforcement Material</option>
										<option value="47">Label - Tamper Evident</option>
										<option value="49">Label - Tub (Back)</option>
										<option value="50">Label - Tub (Bottom)</option>
										<option value="51">Label - Tub (Front)</option>
										<option value="52">Label - Tub (Side)</option>
										<option value="53">Label - Tub (Top)</option>
										<option value="54">Metal - Cookie Tin</option>
										<option value="55">Metal - Pie Tin</option>
										<option value="56">Other - Ancillary Display Parts</option>
										<option value="57">Other - Half Pallet</option>
										<option value="58">Other - Pallet</option>
										<option value="59">Paperboard - 25c Box</option>
										<option value="60">Paperboard - Autobottom Tray</option>
										<option value="61">Paperboard - Brightwood Tray</option>
										<option value="62">Paperboard - Candy Cane Tray</option>
										<option value="63">Paperboard - Carton Brand</option>
										<option value="64">Paperboard - Carton Misc.</option>
										<option value="65">Paperboard - Carton Private</option>
										<option value="66">Paperboard - Coman</option>
										<option value="67">Paperboard - Mini Boxes</option>
										<option value="68">Paperboard - Pre-glued Tray</option>
										<option value="69">Paperboard - Theater Boxes</option>
										<option value="70">Paperboard - U-Board</option>
										<option value="71">Purchased Candy Semi</option>
										<option value="72">Rigid Plastic - Bottle</option>
										<option value="73">Rigid Plastic - Bottle Cap</option>
										<option value="74">Rigid Plastic - Cups</option>
										<option value="75">Rigid Plastic – Foam Clamshell</option>
										<option value="76">Rigid Plastic - Jars</option>
										<option value="77">Rigid Plastic - Other</option>
										<option value="78">Rigid Plastic - Paperboard Cups</option>
										<option value="79">Rigid Plastic – Pie Dome</option>
										<option value="80">Rigid Plastic - Tray</option>
										<option value="81">Rigid Plastic - Tub Sealing Film</option>
										<option value="82">Rigid Plastic - Tubs (Bottom)</option>
										<option value="83">Rigid Plastic - Tubs (Lids)</option>
										<option value="84">Rigid Plastic- Shrink Band Film</option>
										<option value="85">Transfer Semi</option>

									</select>
                                <input type="hidden" name="ctl00$ctl40$g_2c83c192_5d5a_4da0_91e1_8436e99db50c$FinishedGood$rptTSItem$ctl00$hdnCompassListItemId" id="hdnCompassListItemId" value="1">
                                <input type="hidden" name="ctl00$ctl40$g_2c83c192_5d5a_4da0_91e1_8436e99db50c$FinishedGood$rptTSItem$ctl00$hdnItemID" id="hdnItemID" value="3651">
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="form-group">
                                <label class="control-label"><span class="markrequired">*</span>New or Existing?:</label>
                                <select name="ctl00$ctl40$g_2c83c192_5d5a_4da0_91e1_8436e99db50c$FinishedGood$rptTSItem$ctl00$drpNew" id="ctl00_ctl40_g_2c83c192_5d5a_4da0_91e1_8436e99db50c_FinishedGood_rptTSItem_ctl00_drpNew" class="PCBOMrequired drpNewClass form-control VerifySAPNewExisting" onchange="drpNew_changed(this.id, true);">
										<option value="-1">Select...</option>
										<option selected="selected" value="New">New</option>
										<option value="Existing">Existing</option>
										

									</select>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="form-group">
                                <label class="control-label"><span class="markrequired">*</span>Base UOM Qty:</label>
                                <input name="ctl00$ctl40$g_2c83c192_5d5a_4da0_91e1_8436e99db50c$FinishedGood$rptTSItem$ctl00$txtPackQty" type="text" value="12345" id="ctl00_ctl40_g_2c83c192_5d5a_4da0_91e1_8436e99db50c_FinishedGood_rptTSItem_ctl00_txtPackQty" class="PCBOMrequired numericDecimal3 form-control">
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="form-group">
                                <label class="control-label"><span class="markrequired">*</span>UOM:</label>
                                <select name="ctl00$ctl40$g_2c83c192_5d5a_4da0_91e1_8436e99db50c$FinishedGood$rptTSItem$ctl00$drpUnitOfMeasure" id="ctl00_ctl40_g_2c83c192_5d5a_4da0_91e1_8436e99db50c_FinishedGood_rptTSItem_ctl00_drpUnitOfMeasure" class="PCBOMrequired form-control">
										<option value="-1">Select...</option>
										<option value="1">CS</option>
										<option selected="selected" value="2">EA</option>
										<option value="5">FT</option>
										<option value="7">GRAMS</option>
										<option value="6">IN</option>
										<option value="3">LB</option>

									</select>
                            </div>
                        </div>
                    </div>
          
        </div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Application Page
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
 
</asp:Content>
