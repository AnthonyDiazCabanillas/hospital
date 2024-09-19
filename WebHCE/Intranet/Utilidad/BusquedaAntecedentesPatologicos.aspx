<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BusquedaAntecedentesPatologicos.aspx.vb" Inherits="WebHCE.BusquedaAntecedentesPatologicos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="frmBusquedaAntecedetePatologico" runat="server">
        <div class="JSBDIV_TABLA">
            <asp:GridView ID="gvBusquedaAntecedentePatologico" runat="server" AutoGenerateColumns="False" 
                    ShowHeaderWhenEmpty="true" CssClass="JSBTABLA-1" GridLines="None" 
                PagerStyle-CssClass="JPAGINADO" >
                <Columns>
                    <asp:BoundField DataField="ide_patologia" HeaderText="Código">
                        <ItemStyle Width="20%" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="dsc_patologia" HeaderText="Nombre" HtmlEncode="false" HtmlEncodeFormatString="true">
                        <ItemStyle Width="80%" HorizontalAlign="Left" />
                    </asp:BoundField>
                </Columns>                
            </asp:GridView>
        </div>
    </form>
</body>
</html>
