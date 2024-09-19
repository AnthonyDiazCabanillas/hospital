<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="RespuestaInterconsulta.aspx.vb" Inherits="WebHCE.RespuestaInterconsulta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".JFECHA").datepicker();
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="JFILA">
    <div class="JCELDA-6">
        <fieldset>
            <legend>Anterior</legend>
            <div class="JDIV-CONTROLES">
                <div class="JFILA">
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2">Motivo:</span>
                        </div>                                                
                    </div>
                    <div class="JCELDA-4">
                        <div class="JDIV-CONTROLES">
                            <asp:DropDownList runat="server" ID="ddlMotivo" CssClass="JSELECT"></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2">Especialidad:</span>
                        </div>                                                
                    </div>
                    <div class="JCELDA-8">
                        <div class="JDIV-CONTROLES">
                            <input type="text" runat="server" id="txtEspecialidadInterconsulta" class="JFECHA" />
                        </div>                                                
                    </div>
                    <div class="JCELDA-1">
                        <div class="JDIV-CONTROLES">
                            <img src="../Imagenes/Buscar.png" id="imgBusquedaEspecialidadInterconsulta" alt="" class="JIMG-BUSQUEDA" />
                        </div>
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2">Medico Interconsulta:</span>
                        </div>                                                
                    </div>
                    <div class="JCELDA-8">
                        <div class="JDIV-CONTROLES">
                            <input type="text" runat="server" id="txtMedicoInterconsulta" class="JTEXTO" />
                        </div>                                                
                    </div>
                    <div class="JCELDA-1">
                        <div class="JDIV-CONTROLES">
                            <img src="../Imagenes/Buscar.png" id="imgBusquedaMedicoInterconsulta" alt="" class="JIMG-BUSQUEDA" />
                        </div>
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-12">
                        <div class="JDIV-CONTROLES">
                            <textarea rows="5" cols="1" runat="server" id="txtDescripcionInterconsulta" class="JTEXTO"></textarea>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <fieldset>
            <legend>Responder</legend>
            <div class="JDIV-CONTROLES">
                <div class="JFILA">
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2">Especialidad:</span>
                        </div>                                                
                    </div>
                    <div class="JCELDA-8">
                        <div class="JDIV-CONTROLES">
                            <input type="text" runat="server" id="Text1" class="JTEXTO" />
                        </div>                                                
                    </div>
                    <div class="JCELDA-1">
                        <div class="JDIV-CONTROLES">
                            <%--<img src="../Imagenes/Buscar.png" id="img1" alt="" class="JIMG-BUSQUEDA" />--%>
                        </div>
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2">Medico Interconsulta:</span>
                        </div>                                                
                    </div>
                    <div class="JCELDA-8">
                        <div class="JDIV-CONTROLES">
                            <input type="text" runat="server" id="Text2" class="JTEXTO" />
                        </div>                                                
                    </div>
                    <div class="JCELDA-1">
                        <div class="JDIV-CONTROLES">
                            <%--<img src="../Imagenes/Buscar.png" id="img2" alt="" class="JIMG-BUSQUEDA" />--%>
                        </div>
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-12">
                        <div class="JDIV-CONTROLES">
                            <textarea rows="5" cols="1" runat="server" id="Textarea1" class="JTEXTO"></textarea>
                        </div>
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-12">
                        <div class="JDIV-CONTROLES">
                            <input type="button" value="Responder" id="btnResponderInterconsulta" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        
    </div>
    <div class="JCELDA-6">
        <div class="JDIV-CONTROLES">
            <div class="JFILA">
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_3">Historial Interconsultas</span>
                    </div>                                                
                </div>
            </div>
            <div class="JFILA">
                <div class="JCELDA-12">
                    <div class="JDIV-CONTROLES">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" CssClass="JSBTABLA" GridLines="None" >
                            <Columns>
                                <asp:TemplateField HeaderText="Pendiente">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imbInfo" runat="server" CausesValidation="False" CommandName="Interconsulta"
                                            ImageUrl="~/Imagenes/Punto_Rojo.png" />
                                    </ItemTemplate>                                                                
                                </asp:TemplateField>
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha Sol." >
                                    <ItemStyle CssClass="Fecha Sol" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha Resp." >
                                    <ItemStyle CssClass="Fecha Resp." />
                                </asp:BoundField>
                                <asp:BoundField DataField="Medico" HeaderText="Médico" >
                                    <ItemStyle CssClass="Médico" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" >
                                    <ItemStyle  CssClass="Especialidad" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Motivo" HeaderText="Motivo" >
                                    <ItemStyle  CssClass="Motivo" />
                                </asp:BoundField>                                                        
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="JFILA">
    <div class="JCELDA-2 JESPACIO-IZQ-10">
        <div class="JDIV-CONTROLES">
            <input type="button" value="Salir" id="btnSalirInterconsulta" />
        </div>
    </div>
</div>

</asp:Content>
