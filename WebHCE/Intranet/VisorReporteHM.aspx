﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VisorReporteHM.aspx.vb" Inherits="WebHCE.VisorReporteHM" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--<script type="text/javascript">
        function fn_CierraReporteHM() {
            $("#" + "<%=btnCerrarReporteHM.ClientID %>").click();
        }
    </script>--%>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin:auto;">
        
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
            AutoDataBind="true" HasToggleGroupTreeButton="False" 
            HasToggleParameterPanelButton="False" ToolPanelView="None" BestFitPage="False" Width="100%" />
    
    </div>
    </form>
</body>
</html>
