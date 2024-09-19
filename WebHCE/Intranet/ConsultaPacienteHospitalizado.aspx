<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ConsultaPacienteHospitalizado.aspx.vb" Inherits="WebHCE.ConsultaPacienteHospitalizado" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        var Pabellon = "";
        var Servicio = "";
        var CheckActivo = "";
        var CheckEstado = ""; //nuevo valor 15/06/2020

        $(document).ready(function () {
            //CODIGO QUE CARGA LA BOTONERA
            $("#divBotonera").load("Utilidad/Botonera.aspx", function () {
                $("#imgHome, #imgEditar, #imgClinica, #imgPastillaA, #imgReceta, #imgPedidos, #imgAlerta, #imgAltaMedica, #imgImprimir, #imgRoeLaboratorioB").css("display", "none"); //JB - SE OCULTA IMPRIMIR - 17/04/2020
                //$("#imgLoginEnfermera").css("display", "inline");
            });


            //CODIGO QUE SE EJECUTA SI SE MARCA/DESMARCA EL CHECKBOX
            if ($("#chkBusqueda").prop("checked") == true) {
                CheckActivo = "S";
                $("#" + "<%=ddlPabellonConsultaPaciente.ClientID %>").prop("disabled", true);
                $("#" + "<%=ddlPisosConsultaPaciente.ClientID %>").prop("disabled", true);
                $("#" + "<%=txtBuscarPacienteHospitalizado.ClientID %>").prop("disabled", false);
                $("#chkBuscarNoActivo").prop("disabled", false);    
            } else {
                CheckActivo = "N";
                $("#" + "<%=txtBuscarPacienteHospitalizado.ClientID %>").val("");
                $("#" + "<%=ddlPabellonConsultaPaciente.ClientID %>").prop("disabled", false);
                $("#" + "<%=ddlPisosConsultaPaciente.ClientID %>").prop("disabled", false);
                $("#" + "<%=txtBuscarPacienteHospitalizado.ClientID %>").prop("disabled", true);
                $("#chkBuscarNoActivo").prop("checked", false);     
                $("#chkBuscarNoActivo").prop("disabled", true);
            }

            //CODIGO QUE CARGA LA GRILLA
            if ($("#chkBuscarNoActivo").prop("checked")) {
                CheckEstado = "N";
            } else {
                CheckEstado = "S";
            }
            Pabellon = $("#" + "<%=ddlPabellonConsultaPaciente.ClientID %>").val();
            Servicio = $("#" + "<%=ddlPisosConsultaPaciente.ClientID %>").val();
            $("#divGrid").load("GridViewAjax/GridConsultaPacienteHospitalizado.aspx?Pagina=1&NombrePacienteBuscar=" + $("#" + "<%=txtBuscarPacienteHospitalizado.ClientID %>").val().trim().replace("*", "").replace(" ", "*") +
                "&Pabellon=" + Pabellon.trim() + "&Servicio=" + Servicio.trim() + "&CheckActivo=" + CheckActivo + "&CheckEstado=" + CheckEstado, function () {
                    fn_LOAD_GRID_OCUL();
                });

            $("#" + "<%=ddlPabellonConsultaPaciente.ClientID %>").change(function () {
                CargarGrilla(1);
            });

            $("#" + "<%=ddlPisosConsultaPaciente.ClientID %>").change(function () {
                CargarGrilla(1);
            });

            $("#imgBuscarPacienteHospitalizado").click(function () {
                CargarGrilla(1);
            });


            //CLICK EN CHECKBOX PARA LA BUSQUEDA
            $("#chkBusqueda").click(function () {
                if ($(this).prop("checked") == true) {
                    CheckActivo = "S";
                    $("#" + "<%=ddlPabellonConsultaPaciente.ClientID %>").prop("disabled", true);
                    $("#" + "<%=ddlPisosConsultaPaciente.ClientID %>").prop("disabled", true);
                    $("#" + "<%=txtBuscarPacienteHospitalizado.ClientID %>").prop("disabled", false);
                    $("#chkBuscarNoActivo").prop("disabled", false);                    
                    CargarGrilla(1);
                } else {
                    CheckActivo = "N";
                    $("#" + "<%=txtBuscarPacienteHospitalizado.ClientID %>").val("");
                    $("#" + "<%=ddlPabellonConsultaPaciente.ClientID %>").prop("disabled", false);
                    $("#" + "<%=ddlPisosConsultaPaciente.ClientID %>").prop("disabled", false);
                    $("#" + "<%=txtBuscarPacienteHospitalizado.ClientID %>").prop("disabled", true);
                    $("#chkBuscarNoActivo").prop("checked", false);     
                    $("#chkBuscarNoActivo").prop("disabled", true);
                    CargarGrilla(1);
                }
            });

            $("#chkBuscarNoActivo").click(function () { //JB - 15/06/2020 - NUEVO CODIGO PARA BUSCAR NO ACTIVOS
                if ($(this).prop("checked") == true) {
                    CheckEstado = "N";
                } else {
                    CheckEstado = "S";
                }
                if ($("#" + "<%=txtBuscarPacienteHospitalizado.ClientID %>").val().trim() != "") {
                    CargarGrilla(1);
                }
            });

            $("#" + "<%=txtBuscarPacienteHospitalizado.ClientID %>").keypress(function (e) { //JB - 15/06/2020 - NUEVO CODIGO PARA EVITAR EL SUBMIT DEL ENTER EN EL CONTROL
                if (e.which == 13) {
                    event.preventDefault();
                    CargarGrilla(1);
                }
            });

        });

        function CargarGrilla(NumeroPag) {
            //INICIO - JB - 15/06/2020 - NUEVO CODIGO PARA BUSCAR NO ACTIVOS
            if ($("#chkBuscarNoActivo").prop("checked")) {
                CheckEstado = "N";
            } else {
                CheckEstado = "S";
            }            
            //FIN - JB - 15/06/2020 - NUEVO CODIGO PARA BUSCAR NO ACTIVOS

            Pabellon = $("#" + "<%=ddlPabellonConsultaPaciente.ClientID %>").val();
            Servicio = $("#" + "<%=ddlPisosConsultaPaciente.ClientID %>").val();
            //.split("*").join("")
            fn_LOAD_GRID_VISI();
            $("#divGrid").load("GridViewAjax/GridConsultaPacienteHospitalizado.aspx?Pagina=" + NumeroPag + "&NombrePacienteBuscar=" + $("#" + "<%=txtBuscarPacienteHospitalizado.ClientID %>").val().trim().replace(/ /g, "*") +
                    "&Pabellon=" + Pabellon.trim() + "&Servicio=" + Servicio.trim() + "&CheckActivo=" + CheckActivo + "&CheckEstado=" + CheckEstado, function () {
                        fn_LOAD_GRID_OCUL();
                    });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="JCONTENEDOR">
        <div class="JFILA" style="position:absolute;top:40px;right:1%;float:right;width:75%;">
            <div class="JCELDA-12">
                <div class="JDIV-CONTROLES" style="border:1px solid Green;padding:0.4em;" id="divBotonera">
                    
                </div>
            </div>
        </div>
        <br />
        <div class="JFILA">
            <div class="JCELDA-2"> <%--JESPACIO-IZQ-1--%>
                <div class="JDIV-CONTROLES">
                     <span class="JETIQUETA">Buscar Pacientes Hospitalizados</span>
                     <%--<asp:Button runat="server" ID="btnDEMO_CLICK" Text="Click" />--%>
                </div>
            </div>
            <div class="JCELDA-3"> <%--JESPACIO-IZQ-2--%>
                <div class="JDIV-CONTROLES">
                     <input type="text" class="JTEXTO" runat="server" id="txtBuscarPacienteHospitalizado" />
                </div>
            </div>            
            <div class="JCELDA-1">
                <div class="JDIV-CONTROLES">
                    <img src="../Imagenes/Buscar.png" class="JIMG-GENERAL" alt="" id="imgBuscarPacienteHospitalizado" />
                    <input type="checkbox" id="chkBusqueda" checked="checked" />
                </div>
            </div>
            <div class="JCELDA-1">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA">Pabellon</span>
                </div>
            </div>
            <div class="JCELDA-2">
                <div class="JDIV-CONTROLES">
                    <asp:DropDownList runat="server" ID="ddlPabellonConsultaPaciente" CssClass="JSELECT"></asp:DropDownList>
                </div>                
            </div>
            <div class="JCELDA-1">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA">Servicio</span>
                </div>
            </div>
            <div class="JCELDA-2">
                <div class="JDIV-CONTROLES">
                    <asp:DropDownList runat="server" ID="ddlPisosConsultaPaciente" CssClass="JSELECT"></asp:DropDownList>
                </div>                
            </div>                        
        </div>
        <div class="JFILA">
            <div class="JCELDA-2">
                <div class="JDIV-CONTROLES">                    
                    <input type="checkbox" id="chkBuscarNoActivo" />
                    <span class="JETIQUETA_2">Buscar No Activos</span>
                </div>
            </div>
        </div>
        <div class="JFILA">
            <br />
        </div>
        <div class="JFILA">
            <div class="JCELDA-12">
                <div class="JDIV-CONTROLES" style="text-align:center;font-size:1.5em;">                    
                    <span class="JETIQUETA_3">Relacion de Pacientes Hospitalizados</span>
                </div>
            </div>
        </div>        
        <div class="JFILA">
            <div class="JCELDA-12">
                <div class="JDIV-CONTROLES">
                    <div class="JSBDIV_TABLA">                    
                    <asp:GridView ID="gvConsultaPaciente" runat="server" AutoGenerateColumns="False" 
                            ShowHeaderWhenEmpty="true" CssClass="JSBTABLA" GridLines="None" 
                            AllowPaging="True" >
                        <Columns>
                            <asp:BoundField DataField="codpaciente" HeaderText="ID" >
                                <ItemStyle CssClass="ID" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Paciente">
                                <ItemTemplate>                                    
                                    <img alt="" id="imgPacienteTabla" runat="server" src="" class="JIMG-GENERAL" />
                                    <span id="Span1" runat="server"><%# Eval("nombres")%></span>
                                </ItemTemplate>
                                <ItemStyle CssClass="Paciente"/>
                            </asp:TemplateField>
                            <asp:BoundField DataField="edad" HeaderText="Edad" >
                                <ItemStyle  CssClass="Edad" />
                            </asp:BoundField>
                            <asp:BoundField DataField="cama" HeaderText="Cama" >
                                <ItemStyle  CssClass="Cama" />
                            </asp:BoundField>
                            <asp:BoundField DataField="codatencion" HeaderText="Atención" >
                                <ItemStyle  CssClass="Atención" />
                            </asp:BoundField>
                            <asp:BoundField DataField="fechainicio" HeaderText="Fecha Ingreso" >
                                <ItemStyle  CssClass="Fecha Ingreso" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Médico Tratante">
                                <ItemTemplate>                                    
                                    <img alt="" id="imgMedicoTratanteTabla" runat="server" src="~/Imagenes/Doctor.png" class="JIMG-GENERAL" />
                                    <span runat="server"><%# Eval("NombreMedico")%></span>
                                </ItemTemplate>
                                <ItemStyle CssClass="Médico Tratante"/>
                            </asp:TemplateField>                        
                            <asp:TemplateField HeaderText="Lab">
                                <ItemTemplate>                                    
                                    <img alt="" id="imgLaboratorioTabla" runat="server" src="" class="JIMG-GENERAL" />                                    
                                </ItemTemplate>                                                                
                                <ItemStyle CssClass="Lab"/>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Img">
                                <ItemTemplate>                                    
                                    <img alt="" id="imgImagenTabla" runat="server" src="" class="JIMG-GENERAL" />
                                </ItemTemplate>
                                <ItemStyle CssClass="Img"/>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Intercon">
                                <ItemTemplate>                                    
                                    <img alt="" id="imgInterconTabla" runat="server" src="" class="JIMG-GENERAL" />                                    
                                </ItemTemplate>                                                                
                                <ItemStyle CssClass="Intercon"/>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CCIM">
                                <ItemTemplate>                                    
                                    <img alt="" id="imgCCIMTabla" runat="server" src="~/Imagenes/Pastilla.png" class="JIMG-GENERAL" />                                    
                                </ItemTemplate>                                                                
                                <ItemStyle CssClass="CCIM"/>
                            </asp:TemplateField>
                            <asp:BoundField DataField="NombreAseguradora" HeaderText="Aseguradora" >
                                <ItemStyle  CssClass="Aseguradora" />
                            </asp:BoundField>
                            <asp:BoundField DataField="fechaaltamedica" HeaderText="Alta" >
                                <ItemStyle  CssClass="Alta" />
                            </asp:BoundField>
                            <asp:BoundField DataField="sexo" HeaderText="Sexo" HeaderStyle-CssClass="JCOL-OCULTA" >
                                <ItemStyle  CssClass="JCOL-OCULTA sexo"  />
                            </asp:BoundField>
                            <asp:BoundField DataField="est_analisis" HeaderText="est_analisis" HeaderStyle-CssClass="JCOL-OCULTA" >
                                <ItemStyle  CssClass="JCOL-OCULTA est_analisis"  />
                            </asp:BoundField>
                            <asp:BoundField DataField="est_imagen" HeaderText="est_imagen" HeaderStyle-CssClass="JCOL-OCULTA" >
                                <ItemStyle  CssClass="JCOL-OCULTA est_imagen"  />
                            </asp:BoundField>
                            <asp:BoundField DataField="codmedico" HeaderText="codmedico" HeaderStyle-CssClass="JCOL-OCULTA" >
                                <ItemStyle  CssClass="JCOL-OCULTA codmedico"  />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        

        <div class="JFILA">
            <div class="JCELDA-12">
                <div class="JDIV-CONTROLES">                
                    <div id="divGrid">
                    
                    </div>
                </div>
            </div>
        </div>
        
    </div>
     <asp:HiddenField ID="StatedInputData" runat="server"  ClientIDMode="Static"/>

 </asp:Content>
