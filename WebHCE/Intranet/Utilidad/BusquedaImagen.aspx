<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BusquedaImagen.aspx.vb" Inherits="WebHCE.BusquedaImagen" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="frmBusquedaImagen" runat="server">
        <input type="hidden" id="hfTipoBusqueda" />
        <div class="JSBDIV_TABLA" runat="server" id="divImagenes">                    
            <asp:GridView ID="gvBusquedaImagen" runat="server" AutoGenerateColumns="False" 
                    ShowHeaderWhenEmpty="true" CssClass="JSBTABLA-1" GridLines="None" 
                PageSize="5" PagerStyle-CssClass="JPAGINADO" >
                <Columns>
                    <asp:BoundField DataField="Codigo" HeaderText="Cod. Prest">
                    <ItemStyle Width="20%" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Nombre" HeaderText="Prestación" HtmlEncode="false" HtmlEncodeFormatString="true">
                        <ItemStyle Width="61%" HorizontalAlign="Left" />
                    </asp:BoundField>
                    
                    <asp:BoundField DataField="flg_favorito" HeaderText="FlgFavorito" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA FlgFavorito"  />
                    </asp:BoundField>   
                    <asp:BoundField DataField="ide_favorito" HeaderText="IdeFavorito" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA IdeFavorito"  />
                    </asp:BoundField>   
                    <asp:BoundField DataField="DscTipo1" HeaderText="d1" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA DscTipo1"  />
                    </asp:BoundField>
                    <asp:BoundField DataField="idetipo1" HeaderText="idetipo1" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA idetipo1"  />
                    </asp:BoundField>

                    <asp:BoundField DataField="DscTipo2" HeaderText="d2" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA DscTipo2"  />
                    </asp:BoundField>
                    <asp:BoundField DataField="idetipo2" HeaderText="idetipo2" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA idetipo2"  />
                    </asp:BoundField>

                    <asp:BoundField DataField="DscTipo3" HeaderText="d3" HeaderStyle-CssClass="JCOL-OCULTA" > 
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA DscTipo3" />
                    </asp:BoundField>             
                    <asp:BoundField DataField="idetipo3" HeaderText="idetipo3" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA idetipo3"  />
                    </asp:BoundField>
                </Columns>
                <PagerStyle CssClass="JPAGINADO" />
            </asp:GridView>
        </div>
        <div class="JSBDIV_TABLA" runat="server" id="divImagenesFavorito">        
            <asp:GridView ID="gvBusquedaImagenFavorito" runat="server" AutoGenerateColumns="False" 
                    ShowHeaderWhenEmpty="true" CssClass="JSBTABLA-1" GridLines="None" 
                PageSize="5" PagerStyle-CssClass="JPAGINADO" >
                <Columns>
                    <asp:BoundField DataField="ide_imagen" HeaderText="Cod. Prest">
                    <ItemStyle Width="20%" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="dsc_imagen" HeaderText="Prestación" HtmlEncode="false" HtmlEncodeFormatString="true">
                        <ItemStyle Width="61%" HorizontalAlign="Left" />
                    </asp:BoundField>                    
                    
                    <asp:BoundField DataField="ide_imagenfav" HeaderText="IdeImagenFav" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA IdeImagenFav"  />
                    </asp:BoundField>   
                    <asp:BoundField DataField="grupo" HeaderText="Grupo" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA Grupo"  />
                    </asp:BoundField>   
                </Columns>
                <PagerStyle CssClass="JPAGINADO" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
