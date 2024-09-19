<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BusquedaProcedimientoMedico.aspx.vb" Inherits="WebHCE.BusquedaProcedimientoMedico" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="frmBusquedaProcedimientoMedico" runat="server">
        <div class="JSBDIV_TABLA" runat="server">
            <asp:GridView ID="gvBusquedaProcedimientoMedico" runat="server" AutoGenerateColumns="False" 
                    ShowHeaderWhenEmpty="true" CssClass="JSBTABLA-1" GridLines="None" 
                PageSize="5" PagerStyle-CssClass="JPAGINADO" >
                <Columns>
                    <asp:BoundField DataField="ide_procedimiento" HeaderText="Id">
                        <ItemStyle Width="20%" HorizontalAlign="Center" />
                        <HeaderStyle Width="20%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="dsc_procedimiento" HeaderText="Procedimiento" HtmlEncode="false" HtmlEncodeFormatString="true">
                        <ItemStyle Width="50%" HorizontalAlign="Left" />
                        <HeaderStyle Width="50%" />
                    </asp:BoundField>                    
                    <asp:BoundField DataField="cod_prestacion" HeaderText="cod_prestacion" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA cod_prestacion"  />
                    </asp:BoundField>
                    <%--<asp:BoundField DataField="servicio" HeaderText="servicio" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA servicio"  />
                    </asp:BoundField>
                    <asp:BoundField DataField="ide_procedimiento_padre" HeaderText="ide_procedimiento_padre" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA ide_procedimiento_padre"  />
                    </asp:BoundField>--%>
                </Columns>
                <PagerStyle CssClass="JPAGINADO" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
