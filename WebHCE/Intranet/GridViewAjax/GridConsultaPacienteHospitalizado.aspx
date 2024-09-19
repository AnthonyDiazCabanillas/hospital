<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GridConsultaPacienteHospitalizado.aspx.vb" Inherits="WebHCE.GridConsultaPacienteHospitalizado" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        var AtencionInter = "";
        var EspecialidadInter = "";

        var IdInterconsultaR = "";
        var IdMotivoR = "";
        var CodEspecialidadR = "";
        var DescripcionSolicitudR = "";
        var DescripcionEspecialidadR = "";
        var NombreMedicoR = "";
        var aValores = "";

        var codigox = "";

        $(document).ready(function ()
        {
            if ($("#" + "<%=hfMensajeError.ClientID %>").val() != "") {
                $.JMensajePOPUP("Mensaje del Sistema Clinica San Felipe", $("#" + "<%=hfMensajeError.ClientID %>").val(), "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()");
                $("#" + "<%=hfMensajeError.ClientID %>").val("");
            }



            /*PARA EL BOTON IMPRIMIR SE DEBE CAPTURAR UNA FILA*/
            $("#frmGridConsultaPacienteHospitalizado").find(".JSBTABLA > tbody > tr").click(function () {
                $("#divGrid").find(".JSBTABLA tr").each(function () {
                    $(this).find("td").removeClass("JFILA-SELECCIONADA");
                });
                if ($(this).attr("class") != undefined && $(this).attr("class") != "") { //18/01/2017
                    return;
                }
                $(this).find("td").addClass("JFILA-SELECCIONADA");

                var CodigoAtencion = $(this).find(".Atención").html().trim();
                /*$.ajax({  JB - COMENTADO - 17/04/2020
                url: "GridViewAjax/GridConsultaPacienteHospitalizado.aspx/RegistraHistoriaClinica",
                type: "POST",
                data: JSON.stringify({
                CodigoAtencion: CodigoAtencion
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (error, abc, def) {
                //alert(def);
                }
                }).done(function (oOB_JSON) {
                Mensaje = oOB_JSON.d;
                if (Mensaje != "") {
                $.JMensajePOPUP("Error", Mensaje, "ERROR", "Cerrar", "fn_oculta_mensaje()");
                } else {
                }

                });*/
            });

            //EVENTO CLICK EN LA IMAGEN DE LA GRILLA
            $("#frmGridConsultaPacienteHospitalizado").find(".JIMG-GENERAL").click(function ()
            {
                var sDE_IMAG_CLIC = $(this).parent().prop("class");
                if (sDE_IMAG_CLIC.search("Paciente") != -1) {
                    var CodigoAtencion = $(this).parent().parent().find(".Atención").html().trim();
                    fn_LOAD_VISI();
                    
                    $.ajax({
                        url: "ConsultaPacienteHospitalizado.aspx/ValidaAlta",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            CodigoAtencion: CodigoAtencion
                        }),
                        dataType: "json",
                        error: function (dato1, datos2, dato3) {
                        }
                    }).done(function (oOB_JSON)
                    {                        
                        $.ajax({
                            url: "GridViewAjax/GridConsultaPacienteHospitalizado.aspx/RegistraHistoriaClinica",
                            type: "POST",
                            data: JSON.stringify({
                                CodigoAtencion: CodigoAtencion
                            }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            error: function (error, abc, def) {
                            }
                        }).done(function (oOB_JSON)
                        {
                            fn_LOAD_OCUL();
                            Mensaje = oOB_JSON.d;
                            if (Mensaje != "")
                            {
                                $.JMensajePOPUP("Error", Mensaje, "ERROR", "Cerrar", "fn_oculta_mensaje()");
                            } else {
                                //window.location.href = "../Login.aspx";
                                $.JPopUp("Acceso", "PopUp/Acceso.aspx", "2", "Ingresar;Cerrar", "fn_IngresarLogin();fn_oculta_popup()", 45);
                            }
                        });
                        /*
                        if (oOB_JSON.d == "1")
                        {
                        $.JMensajePOPUP("Mensaje del Sistema Clinica San Felipe", "El paciente se encuentra de alta.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()");
                        } else {
                        fn_LOAD_VISI();
                        $.ajax({
                        url: "GridViewAjax/GridConsultaPacienteHospitalizado.aspx/RegistraHistoriaClinica",
                        type: "POST",
                        data: JSON.stringify({
                        CodigoAtencion: CodigoAtencion
                        }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        error: function (error, abc, def) {
                        }
                        }).done(function (oOB_JSON) {
                        fn_LOAD_OCUL();
                        Mensaje = oOB_JSON.d;
                        if (Mensaje != "") {
                        $.JMensajePOPUP("Error", Mensaje, "ERROR", "Cerrar", "fn_oculta_mensaje()");
                        } else {
                        //window.location.href = "../Login.aspx";
                        $.JPopUp("Acceso", "PopUp/Acceso.aspx", "2", "Ingresar;Cerrar", "fn_IngresarLogin();fn_oculta_popup()", 45);
                        }
                        });
                        }JB - COMENTADO-26/05/2020*/
                    });
                }
                if (sDE_IMAG_CLIC.search("Médico Tratante") != -1)
                {
                    var sCO_MEDI = $(this).parent().parent().find(".codmedico").html().trim();
                    var sCO_ATEN = $(this).parent().parent().find(".Atención").html().trim();

                    $.JPopUp("Médico Atención", "PopUp/MedicoAtencion.aspx?Codigo=" + sCO_MEDI + "&Atencion=" + sCO_ATEN, "1", "Cerrar", "fn_oculta_popup()", 35);
                }
                if (sDE_IMAG_CLIC.search("Lab") != -1)
                {
                    var CodigoAtencion = $(this).parent().parent().find(".Atención").html().trim();
                    var CodigoMedico = $(this).parent().parent().find(".codmedico").html().trim();

                    fn_LOAD_VISI();
                    $.JPopUp("Laboratorio", "PopUp/Laboratorio.aspx?CodigoAtencion=" + CodigoAtencion, "2", "Ver Informe;Cerrar", "fn_VerInformeLaboratorio();fn_oculta_popup()", 45); //fn_VerInformeLabPopUp
                    /*$.ajax({ JB - COMENTADO - 04/08/2020
                    url: "ConsultaPacienteHospitalizado.aspx/ValidaAlta",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                    CodigoAtencion: CodigoAtencion
                    }),
                    dataType: "json",
                    error: function (dato1, datos2, dato3) {
                    }
                    }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "1") {
                    $.JMensajePOPUP("Mensaje del Sistema Clinica San Felipe", "El paciente se encuentra de alta.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()");
                    } else {
                    fn_LOAD_VISI();
                    $.JPopUp("Laboratorio", "PopUp/Laboratorio.aspx?CodigoAtencion=" + CodigoAtencion, "2", "Ver Informe;Cerrar", "fn_VerInformeLaboratorio();fn_oculta_popup()", 45); //fn_VerInformeLabPopUp
                    }
                    });*/
                }
                if (sDE_IMAG_CLIC.search("Img") != -1)
                {
                    var CodigoAtencion = $(this).parent().parent().find(".Atención").html().trim();

                    fn_LOAD_VISI();
                    $.JPopUp("Imagenes", "PopUp/Imagenes.aspx?CodigoAtencion=" + CodigoAtencion, "3", "Ver Informe;Ver Imagen;Cerrar", "fn_EventoVerInforme();fn_EventoVerImagen();fn_oculta_popup()", 45); //2   fn_VerInformeImgPopUp
                    /*$.ajax({   JB - COMENTADO - 04/08/2020
                    url: "ConsultaPacienteHospitalizado.aspx/ValidaAlta",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                    CodigoAtencion: CodigoAtencion
                    }),
                    dataType: "json",
                    error: function (dato1, datos2, dato3) {
                    }
                    }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "1") {
                    $.JMensajePOPUP("Mensaje del Sistema Clinica San Felipe", "El paciente se encuentra de alta.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()");
                    } else {
                    fn_LOAD_VISI();
                    $.JPopUp("Imagenes", "PopUp/Imagenes.aspx?CodigoAtencion=" + CodigoAtencion, "3", "Ver Informe;Ver Imagen;Cerrar", "fn_EventoVerInforme();fn_EventoVerImagen();fn_oculta_popup()", 45); //2   fn_VerInformeImgPopUp
                    }
                    });*/
                }
                if (sDE_IMAG_CLIC.search("Enfermera") != -1)
                {
                    var CodigoAtencion = $(this).parent().parent().find(".Atención").html().trim();
                    //window.location.href = "../LoginEnfermera.aspx";

                    $("#hfAuxiliar").val(CodigoAtencion);
                    $("#frmGridConsultaPacienteHospitalizado").prop("action", "../LoginEnfermera.aspx");
                    $("#frmGridConsultaPacienteHospitalizado").prop("method", "POST");
                    $("#frmGridConsultaPacienteHospitalizado").submit();
                }
                if (sDE_IMAG_CLIC.search("Inter") != -1)
                {
                    var sCO_ATEN = $(this).parent().parent().find(".Atención").html().trim();
                    $.ajax({
                        url: "ConsultaPacienteHospitalizado.aspx/ValidaAlta",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            CodigoAtencion: sCO_ATEN
                        }),
                        dataType: "json",
                        error: function (dato1, datos2, dato3) {
                        }
                    }).done(function (oOB_JSON)
                    {
                        if (oOB_JSON.d == "1") {
                            //$.JMensajePOPUP("Mensaje del Sistema Clinica San Felipe", "El paciente se encuentra de alta.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()");
                            $.JPopUp("Interconsultas", "PopUp/Interconsulta.aspx?Atencion=" + sCO_ATEN, "1", "Cerrar", "fn_oculta_popup()", 60);
                        } else {
                            $.JPopUp("Interconsultas", "PopUp/Interconsulta.aspx?Atencion=" + sCO_ATEN, "1", "Cerrar", "fn_oculta_popup()", 60);
                        }
                    });
                }
            });

            /*CAMBIO 12/07/2016 - JB*/
            $("#frmGridConsultaPacienteHospitalizado").find(".JNOMBRE-MEDICO").click(function ()
            {
                var sCO_MEDI = $(this).parent().parent().find(".codmedico").html().trim();
                var sCO_ATEN = $(this).parent().parent().find(".Atención").html().trim();

                $.JPopUp("Médico Atención", "PopUp/MedicoAtencion.aspx?Codigo=" + sCO_MEDI + "&Atencion=" + sCO_ATEN, "1", "Cerrar", "fn_oculta_popup()", 35);
            });
            /*FIN CAMBIO 12/07/2016*/


            //EVENTO CLICK EN EL BOTON DE PAGINADO
            $("#frmGridConsultaPacienteHospitalizado").find(".JSBTABLA").find("tr:last").find("a").click(function (event) {
                event.preventDefault();
                var sNumeroPagina = $(this).prop("href").substring(($(this).prop("href").indexOf("$") + 1), $(this).prop("href").lastIndexOf("'"));
                CargarGrilla(sNumeroPagina);
            });


        });

        //funcion para abrir la ventana de login. Esta funcion es llamada desde el popup de interconsulta.
        function fn_AbreVentanaAcceso()
        {
         
            $.JPopUp("Acceso", "PopUp/AccesoInterconsulta.aspx", "2", "Ingresar;Cerrar", "fn_IngresaInterconsulta();fn_oculta_popup()", 50, "", aValores);
        }
        //FUNCION PARA ABRIR EL POPUP DE RESPUESTA INTERCONSULTA, ESTE FUNCION ES LLAMADA DESDE EL POPUP DE ACCESOINTERCONSULTA
        function fn_AbreVentanaRespuestaInterconsulta() {
            $.JPopUp("Respuesta Interconsulta", "PopUp/InterconsultaRespuesta.aspx", "1", "Cerrar", "fn_CierraDetalle()", 80, "", aValores);
        }

        function fn_AbreOlvidoPassword() {
            $.JPopUp("Recordar Contraseña", "PopUp/OlvidoPassword.aspx", "2", "Recuperar;Cerrar", "fn_RecuperaPassword();fn_oculta_popup()", 30, "", aValores);
        }

        function fn_AbreDetalle() {             
            var aValores = [codigox, "D"];
            $.JPopUp("Detalle Interconsulta", "PopUp/DetalleInterconsulta.aspx", "1", "Cerrar", "fn_oculta_popup();", 50, "", aValores);
        }

        /*function fn_ValidaAlta() {
            $.ajax({
                url: "InformacionPaciente.aspx/ValidaAlta",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (dato1, datos2, dato3) {
                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "1") {

                    $("#frmGridConsultaPacienteHospitalizado").find(".JIMG-GENERAL").unbind("click");

                    $("#frmGridConsultaPacienteHospitalizado").find(".JIMG-GENERAL").click(function () {
                        $.JMensajePOPUP("Mensaje del Sistema Clinica San Felipe", "El paciente se encuentra de alta.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()");
                    });
                }
            });
        }*/

    </script>
</head>
<body>
    <form runat="server" id="frmGridConsultaPacienteHospitalizado" class="JFORM-CONTENEDOR-GRID" > 
        <input type="hidden" id="hfAuxiliar" name="hfAuxiliar" runat="server"  />
        <input type="hidden" id="hfMensajeError" name="hfMensajeError" runat="server"  />
        <div class="JFILA">
            <div class="JCELDA-12">
                <div class="JDIV-CONTROLES">
                    <div class="JSBDIV_TABLA">                    
                    <asp:GridView ID="gvConsultaPaciente" runat="server" AutoGenerateColumns="False" 
                            ShowHeaderWhenEmpty="True" CssClass="JSBTABLA" GridLines="None" 
                            AllowPaging="True" PageSize="15" PagerStyle-CssClass="JPAGINADO" >
                        <Columns>
                            <asp:BoundField DataField="codpaciente" HeaderText="H.C." >
                                <ItemStyle CssClass="H.C." />
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
                                    <%--<img alt="" id="imgMedicoTratanteTabla" runat="server" src="../Imagenes/Doctor.png" class="JIMG-GENERAL" />--%>
                                    <span runat="server" class="JNOMBRE-MEDICO" style="cursor:pointer;"><%# Eval("NombreMedico")%></span>
                                </ItemTemplate>
                                <ItemStyle CssClass="Médico Tratante"/>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Lab">
                                <ItemTemplate>
                                    <%--<asp:ImageButton ID="imgLaboratorioTabla" runat="server" CausesValidation="False" CommandName="Interconsulta"/>--%>
                                    <img alt="" id="imgLaboratorioTabla" runat="server" src="" class="JIMG-GENERAL" />                                    
                                </ItemTemplate>                                                                
                                <ItemStyle CssClass="Lab" HorizontalAlign="Center"/>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Img">
                                <ItemTemplate>                                    
                                    <img alt="" id="imgImagenTabla" runat="server" src="" class="JIMG-GENERAL" />
                                </ItemTemplate>
                                <ItemStyle CssClass="Img" HorizontalAlign="Center"/>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Inter">
                                <ItemTemplate>                                    
                                    <img alt="" id="imgInterconTabla" runat="server" src="" class="JIMG-GENERAL" />                                    
                                </ItemTemplate>                                                                
                                <ItemStyle CssClass="Inter" HorizontalAlign="Center"/>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Enfermera">
                                <ItemTemplate>                                    
                                    <img alt="" id="imgEnfermetaTabla" runat="server" src="" class="JIMG-GENERAL" />                                    
                                </ItemTemplate>                                                                
                                <ItemStyle CssClass="JCOL-OCULTA" />
                                <HeaderStyle CssClass="JCOL-OCULTA" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="NombreAseguradora" HeaderText="Aseguradora" >
                                <ItemStyle  CssClass="Aseguradora" />
                            </asp:BoundField>
                            <asp:BoundField DataField="fechaaltamedica" HeaderText="Alta" >
                                <ItemStyle CssClass="JCOL-OCULTA" />
                                <HeaderStyle CssClass="JCOL-OCULTA" />
                            </asp:BoundField>
                            <asp:BoundField DataField="sexo" HeaderText="Sexo" HeaderStyle-CssClass="JCOL-OCULTA" >
                                <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                                <ItemStyle  CssClass="JCOL-OCULTA sexo"  />
                            </asp:BoundField>
                            <asp:BoundField DataField="est_analisis" HeaderText="est_analisis" HeaderStyle-CssClass="JCOL-OCULTA" >
                                <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                                <ItemStyle  CssClass="JCOL-OCULTA est_analisis"  />
                            </asp:BoundField>
                            <asp:BoundField DataField="est_imagen" HeaderText="est_imagen" HeaderStyle-CssClass="JCOL-OCULTA" >
                                <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                                <ItemStyle  CssClass="JCOL-OCULTA est_imagen"  />
                            </asp:BoundField>
                            <asp:BoundField DataField="codmedico" HeaderText="codmedico" HeaderStyle-CssClass="JCOL-OCULTA" >
                                <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                                <ItemStyle  CssClass="JCOL-OCULTA codmedico"  />
                            </asp:BoundField>
                            <asp:BoundField DataField="est_interconsulta" HeaderText="Inter." 
                                HeaderStyle-CssClass="JCOL-OCULTA" >
                                <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                                <ItemStyle  CssClass="JCOL-OCULTA est_interconsulta"  />
                            </asp:BoundField>  
                            <asp:BoundField DataField="ide_historia" HeaderText="ide_historia" HeaderStyle-CssClass="JCOL-OCULTA" >
                                <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                                <ItemStyle  CssClass="JCOL-OCULTA ide_historia"  />
                            </asp:BoundField>                          
                        </Columns>
                        <PagerStyle CssClass="JPAGINADO" />
                    </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
