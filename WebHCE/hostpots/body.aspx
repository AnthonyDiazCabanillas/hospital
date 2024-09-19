<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="body.aspx.vb" Inherits="WebHCE.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ImageMap ID="ImageMap1" runat="server" Height="305px" 
        HotSpotMode="Navigate" ImageUrl="~/Imagenes/body.png" Width="250px">
        <asp:CircleHotSpot AlternateText="cabeza" Radius="10" TabIndex="1" X="125" 
            Y="30" />
        <asp:CircleHotSpot AlternateText="cuello" Radius="10" TabIndex="2" X="125" 
            Y="60" />
        <asp:CircleHotSpot AlternateText="barriga" Radius="10" TabIndex="3" X="125" 
            Y="150" />
        <asp:CircleHotSpot AlternateText="Hombro derecho" Radius="10" X="100" Y="60" />
        <asp:CircleHotSpot AlternateText="Hombro izquierdo" Radius="10" X="150" 
            Y="60" />
    </asp:ImageMap>
    </form>
</body>
</html>
