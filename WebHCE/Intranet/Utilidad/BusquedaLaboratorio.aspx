<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BusquedaLaboratorio.aspx.vb" Inherits="WebHCE.BusquedaLaboratorio" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="frmBusquedaLaboratorio" runat="server">
        <input type="hidden" id="hfTipoBusqueda" />
        <div class="JSBDIV_TABLA" runat="server" id="divAnalisis">                    
            <asp:GridView ID="gvBusquedaAnalisisLaboratorio" runat="server" AutoGenerateColumns="False" 
                    ShowHeaderWhenEmpty="true" CssClass="JSBTABLA-1" GridLines="None" 
                PageSize="5" PagerStyle-CssClass="JPAGINADO" >
                <Columns>
                    <asp:BoundField DataField="Codigo" HeaderText="Cod. Prest">
                    <ItemStyle Width="15%" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Nombre" HeaderText="Prestación" HtmlEncode="false" HtmlEncodeFormatString="true">
                        <ItemStyle Width="61%" HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="titulo" HeaderText="Titulo">
                        <ItemStyle Width="24%" HorizontalAlign="Left" />
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
                    <asp:BoundField DataField="perfil" HeaderText="perfil" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA perfil" />
                    </asp:BoundField>
                </Columns>
                <PagerStyle CssClass="JPAGINADO" />
            </asp:GridView>
        </div>
        <div class="JSBDIV_TABLA" runat="server" id="divAnalisisFavorito">
            <asp:GridView ID="gvBusquedaAnalisisFavorito" runat="server" AutoGenerateColumns="False" 
                ShowHeaderWhenEmpty="true" CssClass="JSBTABLA-1" GridLines="None" 
                PageSize="5" PagerStyle-CssClass="JPAGINADO" >
                <Columns>
                    <asp:BoundField DataField="ide_analisis" HeaderText="Cod. Prest">
                       <ItemStyle Width="15%" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="dsc_analisis" HtmlEncode="false" HeaderText="Prestación">
                        <ItemStyle Width="61%" HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="titulo" HeaderText="Titulo">
                        <ItemStyle Width="24%" HorizontalAlign="Left" />
                    </asp:BoundField>

                    <asp:BoundField DataField="ide_analisisfav" HeaderText="ide_analisisfav" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA ide_analisisfav"  />
                    </asp:BoundField>
                    <asp:BoundField DataField="dsc_tipoanalisis1" HeaderText="dsc_tipoanalisis1" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA dsc_tipoanalisis1"  />
                    </asp:BoundField>
                    <asp:BoundField DataField="idetipoanalisis1" HeaderText="idetipoanalisis1" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA idetipoanalisis1"  />
                    </asp:BoundField>
                    <asp:BoundField DataField="dsc_tipoanalisis2" HeaderText="dsc_tipoanalisis2" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA dsc_tipoanalisis2"  />
                    </asp:BoundField>
                    <asp:BoundField DataField="idetipoanalisis2" HeaderText="idetipoanalisis2" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA idetipoanalisis2"  />
                    </asp:BoundField>
                    <asp:BoundField DataField="dsc_tipoanalisis3" HeaderText="dsc_tipoanalisis3" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA dsc_tipoanalisis3"  />
                    </asp:BoundField>
                    <asp:BoundField DataField="idetipoanalisis3" HeaderText="idetipoanalisis3" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA idetipoanalisis3"  />
                    </asp:BoundField>

                    <asp:BoundField DataField="perfil" HeaderText="perfil" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA perfil" />
                    </asp:BoundField>
                     <asp:BoundField DataField="Grupo" HeaderText="d3" Visible="False" />
                    </Columns>
                <PagerStyle CssClass="JPAGINADO" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
