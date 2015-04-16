<%@ Page Language="C#" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
   <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
   <title>Core Features</title>
   <link href="style.css" type="text/css" rel="stylesheet" />
   <script type="text/javascript">

      function colorChanged(sender, eventArgs) {
         if (sender.get_selectedColor() && sender.get_selectedColor().get_hex()) {
            document.getElementById('chip').style.backgroundColor = '#' + sender.get_selectedColor().get_hex();
            document.getElementById('hexoutput').value = '#' + sender.get_selectedColor().get_hex();
         }
      }

      function load() {
         if (!document.getElementById('chip')) {
            setTimeout('load()', 250);
            return;
         }

         if (ColorPicker1.get_selectedColor() && ColorPicker1.get_selectedColor().get_hex()) {
            document.getElementById('hexoutput').value = '#' + ColorPicker1.get_selectedColor().get_hex();
            document.getElementById('chip').style.backgroundColor = document.getElementById('hexoutput').value;
         }
      }

   </script>
</head>
<body>
   <form id="Form1" method="post" runat="server">
   <div class="DemoArea">
      <table cellspacing="0" cellpadding="0" width="100%">
         <tr>
            <td>
               <ComponentArt:ColorPicker ID="ColorPicker1" GridColumns="8" Mode="Slider" CssClass="colorpicker"
                  ColorCssClass="swatch" ColorHoverCssClass="swatch-h" ColorActiveCssClass="swatch-a"
                  CustomColorCssClass="swatch" CustomColorHoverCssClass="swatch-h" CustomColorActiveCssClass="swatch-a"
                  ColorGridCssClass="swatches" CustomColorGridCssClass="customswatches" runat="server"
                  ColorGridLabel="Standard Colors" CustomColorGridLabel="Custom Colors" ColorPlaneCssClass="plane"
                  SliderCssClass="slider" BaseImageUrl="images/" CrosshairOffsetX="1" CrosshairOffsetY="1">
                  <clientevents>
                    <ColorChanged EventHandler="colorChanged" />
                    <Load EventHandler="load" />
                </clientevents>
                  <colors>
                    <ComponentArt:ColorPickerColor Name="Red" />
                    <ComponentArt:ColorPickerColor Name="Orange" />
                    <ComponentArt:ColorPickerColor Name="Yellow" />
                    <ComponentArt:ColorPickerColor Name="Green" />
                    <ComponentArt:ColorPickerColor Name="Blue" />
                    <ComponentArt:ColorPickerColor Name="Purple" />
                    <ComponentArt:ColorPickerColor Name="Black" />
                    <ComponentArt:ColorPickerColor Name="White" />
                </colors>
                  <customcolors>
                    <ComponentArt:ColorPickerColor Name="White" />
                    <ComponentArt:ColorPickerColor Name="White" />
                    <ComponentArt:ColorPickerColor Name="White" />
                    <ComponentArt:ColorPickerColor Name="White" />
                    <ComponentArt:ColorPickerColor Name="White" />
                    <ComponentArt:ColorPickerColor Name="White" />
                    <ComponentArt:ColorPickerColor Name="White" />
                    <ComponentArt:ColorPickerColor Name="White" />
                </customcolors>
               </ComponentArt:ColorPicker>
            </td>
         </tr>
         <tr>
            <td height="100">
               <div>
                  <div class="br">
                  </div>
                  <div class="info">
                     <div id="chip">
                     </div>
                     <div class="hex">
                        <div class="lbl">
                           Hex value</div>
                        <div class="val">
                           <input type="text" style="width: 96px" id="hexoutput" /></div>
                     </div>
                  </div>
                  <br />
                  <br />
                  <asp:Button ID="SelectColorButton" Text="Select" OnClick="SelectColor" Style="width: 102px;" runat="server" />
               </div>
            </td>
         </tr>
      </table>
   </div>
   </form>
</body>
