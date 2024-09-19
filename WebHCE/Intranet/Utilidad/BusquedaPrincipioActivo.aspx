<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BusquedaPrincipioActivo.aspx.vb" Inherits="WebHCE.BusquedaPrincipioActivo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="frmBusquedaPrincipioActivo" runat="server">
        <div class="JSBDIV_TABLA">
            <asp:GridView ID="gvBusquedaPrincipioActivo" runat="server" AutoGenerateColumns="False" 
                    ShowHeaderWhenEmpty="true" CssClass="JSBTABLA-1" GridLines="None" 
                PageSize="5" PagerStyle-CssClass="JPAGINADO" >
                <Columns>
                    <asp:BoundField DataField="codgenerico" HeaderText="Código">
                        <ItemStyle Width="20%" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="nombre" HeaderText="Nombre" HtmlEncode="false" HtmlEncodeFormatString="true">
                        <ItemStyle Width="80%" HorizontalAlign="Left" />
                    </asp:BoundField>
                </Columns>
                <PagerStyle CssClass="JPAGINADO" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
