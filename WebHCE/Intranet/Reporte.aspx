<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Reporte.aspx.vb" Inherits="WebHCE.Reporte" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
 
        <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" />
 
        <CR:CrystalReportViewer runat="server" ID="VisorReporte" AutoDataBind="true" 
            Height="50px" PrintMode="ActiveX" 
            Width="350px" EnableParameterPrompt="False" SeparatePages="False" 
            ShowAllPageIds="True" />
 
    </div>
    </form>
</body>
</html>