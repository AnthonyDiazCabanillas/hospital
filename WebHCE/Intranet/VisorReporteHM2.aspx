﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VisorReporteHM2.aspx.cs" Inherits="VisorReporteHM2" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    
    
    
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" HasToggleGroupTreeButton="False" 
            HasToggleParameterPanelButton="False" ToolPanelView="None" BestFitPage="False" Width="100%" />
    
    
    
    </form>
</body>
</html>
