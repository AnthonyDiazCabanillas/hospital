<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BusquedaMedicamento.aspx.vb" Inherits="WebHCE.BusquedaMedicamento" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="frmBusquedaMedicamento" runat="server">
        <div class="JSBDIV_TABLA">                    
            <asp:GridView ID="gvBusquedaProducto" runat="server" AutoGenerateColumns="False" 
                    ShowHeaderWhenEmpty="true" CssClass="JSBTABLA-1" GridLines="None" 
                    AllowPaging="false" PageSize="5" PagerStyle-CssClass="JPAGINADO" >
                <Columns>
                    <asp:BoundField DataField="codpro" HeaderText="Código" >
                        <ItemStyle CssClass="Código" />
                    </asp:BoundField>
                    <asp:BoundField DataField="despro" HeaderText="Producto" >
                        <ItemStyle CssClass="Producto" />
                    </asp:BoundField>                    
                    <asp:BoundField DataField="codpro" HeaderText="codpro" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA codpro"  />
                    </asp:BoundField>                          
                </Columns>
                <PagerStyle CssClass="JPAGINADO" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
