<%@ Page AspCompat="true" Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="PetitorioImagen.aspx.vb" Inherits="WebHCE.PetitorioImagen" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/JTabs.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/JTabs.js" type="text/javascript"></script>
    <link href="../Styles/JTreeview.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/JLoader.js" type="text/javascript"></script>
    <script type="text/javascript">
        var ValoresCheck = "";
        var TextoCheck = "";

        var EjecutaEnviaSolicitud = false;

        $(document).ready(function () {
            $("#divDatosUsuario").load("Utilidad/DatosUsuarioPopUp.aspx", function () {
            });

            $(".JCHEK-TABS").click(function () {
                $(this).parent().find("> .JCONTENIDO-TAB").css("display", "none");
                //$(".JCHEK-TABS").index(this)
                $(this).parent().find("> .JCONTENIDO-TAB").eq($(this).parent().find(".JCHEK-TABS").index(this)).css("display", "block");
            });

            //marcando el primer tab
            $(".JTABS").each(function () {
                $(this).find("> .JCHEK-TABS").eq(0).click();
            });
            //mostrando la primera observacion
            $("#" + "<%=divObservacion.ClientID %>").find("textarea").eq(0).css("display", "inline");




           fn_ValidaCheck();
            $(".JCONTENEDOR-TAB1").find("input[type='checkbox']").click(function () {
                fn_ValidaCheck();
            });

            $("#btnEnviarSolicitud").click(function () {
                $.ajax({
                    url: "PetitorioImagen.aspx/ValidaDiagnostico",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    error: function (dato1, datos2, dato3) {

                    }
                }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "OK") {
                        fn_DeshabilitaControles("btnEnviarSolicitud"); //se deshabilita para que no haga click 2 veces
                        fn_oculta_mensaje_rapido();
                        var AnalisisMarcados = "";
                        $(".JTABS-LABEL").each(function () {
                            var objeto = $(this);
                            var marcado = false;
                            $("#" + "<%=divContenedorPetitorio.ClientID %>").find(":input[type='checkbox']").each(function () {
                                var ObjetoCheck = $(this);
                                if (ObjetoCheck.is(":checked")) {
                                    if (objeto.attr("for") == ObjetoCheck.attr("id").split("_")[1].toString().split("-")[1]) {
                                        if (marcado == false) {
                                            AnalisisMarcados += "<strong>" + objeto.html() + "</strong>" + "<br />";
                                            marcado = true;
                                        }
                                        AnalisisMarcados += "&nbsp;&nbsp;" + ObjetoCheck.next().html() + "<br />";
                                    }
                                }
                            });
                        });
                        $.JMensajePOPUP("Información", "¿Desea enviar los Exámenes de Imágenes seleccionados? <br/><br/>" + AnalisisMarcados, "ADVERTENCIA", "Si;No", "fn_EnviaPetitorioImagen2();fn_CerrarMensaje()", "");
                    } else {
                        $.JMensajePOPUP("Validación", "Debe ingresar primero un diagnóstico", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()", "");
                    }
                });


                //fn_ValidaCheck();
                //$.JMensajePOPUP("Mensaje del Sistema de Clínica San Felipe", "¿Desea enviar los Exámenes de Imágenes seleccionados? <br /><br />" + TextoCheck, "ADVERTENCIA", "Si;No", "fn_EnviaPetitorioImagen();fn_oculta_mensaje()");
            });

            $("#btnVolverAtencion").click(function () {
                //window.location.href = "InformacionPaciente.aspx";
                window.history.back();
            });

            fn_ValidaAlta();



            $(".JTABS-LABEL").click(function () {
                $("#" + "<%=divObservacion.ClientID %>").find("textarea").css("display", "none");
                $("#" + "TxtObservacion-" + $(this).attr("for")).css("display", "inline");
            });
        });



        function fn_CerrarMensaje() {
            fn_HabilitaControles("btnEnviarSolicitud");
            fn_oculta_mensaje();
        }

        function fn_EnviaPetitorioImagen2() {
            $("[class*='JCONTENIDO-POPUP']").find("input[type='button']").prop("disabled", true); //deshabilitando botones del popup para que no haga click por segunda vez            
            fn_oculta_mensaje();
            fn_LOAD_VISI();
            var CodigoImagen = "";
            var CodAtencion = "";
            var Observacion = "";
            
            //Obteniendo imagenes marcadas
            $("#" + "<%=divContenedorPetitorio.ClientID %>").find(":input[type='checkbox']").each(function () {
                if ($(this).prop("checked") == true) {
                    CodigoImagen += $(this).attr("id").split("_")[1] + "@";
                }
            });
            //obteniendo todas las descripciones
            $(".JTABS-LABEL").each(function () {
                Observacion += $("#" + "TxtObservacion-" + $(this).attr("for")).val().replace(/@/g, "*").replace(/-/g, " ") + "-" + $(this).attr("for") + "@";
            });


            $.ajax({
                url: "PetitorioImagen.aspx/EnviarSolicitudPetitorio2",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    CodigoImagen: CodigoImagen,
                    Observacion: Observacion,
                    CodigoAtencion: CodAtencion,
                    IdeHistoria: "",
                    CodMedico: "",
                    CodPaciente: ""
                }),
                dataType: "json",
                error: function (dato1, datos2, dato3) {

                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "OK") {
                    fn_HabilitaControles("btnEnviarSolicitud");
                    $.JMensajePOPUP("Validación", "Se Envio la solicitud de los examenes seleccionados.", "OK", "Cerrar", "fn_OCultaMensajeLimpiaChech()", "");
                } else {
                    fn_HabilitaControles("btnEnviarSolicitud");
                    fn_LOAD_OCUL();
                    $.JMensajePOPUP("Validación", oOB_JSON.d.split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()", "");
                    return false;
                }
            });
        }


        function fn_OCultaMensajeLimpiaChech() {
            $.ajax({
                url: "InformacionPaciente.aspx/VolverPantallaAbrirAcordeon",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    SeccionAbrir: "IMG"
                }),
                error: function (dato1, datos2, dato3) {
                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "OK") {
                    fn_LOAD_OCUL();
                    fn_oculta_mensaje();
                    fn_LimpiaCheck();
                    fn_LOAD_VISI();
                    window.location.href = "InformacionPaciente.aspx";
                }
            });
        }

        function fn_LimpiaCheck() {
            $("#" + "<%=divContenedorPetitorio.ClientID %>").find("input[type='checkbox']").each(function () {
                $(this).prop("checked", false);
            });
            fn_ValidaCheck();
        }



        function fn_EnviaPetitorioImagen() {
            fn_oculta_mensaje_rapido();
            if (ValoresCheck.trim() != "") {
                fn_LOAD_VISI();
                $.ajax({
                    url: "PetitorioImagen.aspx/EnviarSolicitudPetitorio",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                        CodigosPetitorioImagen: ValoresCheck,
                        Descripcion: $("#txtObservacionPetitorioLaboratorio").val()
                    }),
                    dataType: "json",
                    error: function (dato1, datos2, dato3) {
                    }
                }).done(function (oOB_JSON) {
                    fn_LOAD_OCUL();                    
                    if (oOB_JSON.d != "OK") {
                        $.JMensajePOPUP("Mensaje del Sistema de Clínica San Felipe - Error", oOB_JSON.d, "ERROR", "Aceptar", "fn_oculta_mensaje()");
                    } else {
                        $.JMensajePOPUP("Mensaje del Sistema de Clínica San Felipe.", "Se Envió la solicitud de los examenes seleccionados.", "OK", "Aceptar", "fn_oculta_mensaje()");
                        ValoresCheck = "";
                        $(".JCONTENEDOR-TAB1").find("input[type='checkbox']").each(function () {
                            $(this).prop("checked", false);
                        });
                        $("#btnEnviarSolicitud").prop("disabled", "disabled"); //15/09/2016
                    }
                });
            }
        }

        //FUNCION PARA VALIDAR SI HAY ALGUN CHECK MARCADO, SI HAY ALGUNO SELECCIONADO SE HABILITA EL BOTON EN ENVIAR SOLICITUD
        function fn_ValidaCheck() {
            var CheckActivo = false;
            ValoresCheck = "";
            TextoCheck = "";
            $(".JCONTENEDOR-TAB1").find("input[type='checkbox']").each(function () {
                if ($(this).prop("checked")) {
                    ValoresCheck += $(this).val() + ";";
                    TextoCheck += $(this).next().html().trim() + "<br />";
                    CheckActivo = true;
                }
            });
            ValoresCheck = ValoresCheck.substring(0, ValoresCheck.lastIndexOf(";"));            
            if (CheckActivo == false) {
                $("#btnEnviarSolicitud").prop("disabled", "disabled");
                $("#btnEnviarSolicitud").prop("disabled", "disabled"); //15/09/2016
            } else {
                $("#btnEnviarSolicitud").removeAttr("disabled");
                $("#btnEnviarSolicitud").removeAttr("disabled"); //15/09/2016
            }
        }
    
//        function fn_EnviaPetitorio() {
//            fn_ValidaCheck();

//            //$.JMensajePOPUP("Mensaje del Sistema de Clínica San Felipe", "¿Desea enviar los Exámenes de Imágenes seleccionados? <br /><br />" + TextoCheck, "ADVERTENCIA", "Si;No", "fn_EnviaPetitorioImagen();fn_oculta_mensaje()");
//            $.JMensajePOPUP("Mensaje del Sistema de Clínica San Felipe", "¿Desea enviar los Exámenes de Imágenes seleccionadosss? <br /><br />" + TextoCheck, "ADVERTENCIA", "Si;No", "fn_AceptaEnviarPetitorio();fn_oculta_mensaje()");
//            /*if (EjecutaEnviaSolicitud == false) {
//                return false;
//            } else {
//                if (ValoresCheck.trim() == "") {
//                    return false;
//                }
//                return true;
//            }*/
//        }
//        function fn_AceptaEnviarPetitorio() {            
//            fn_oculta_mensaje_rapido();
//            EjecutaEnviaSolicitud = true;
//            $("#" + "<%=hfValoresCheck.ClientID %>").val(ValoresCheck);
//            //$("#" + "<=hfObservacionPetitorio.ClientID %>").val($("#txtObservacionPetitorioLaboratorio").val());
//            fn_EnviaPetitorioImagen()
//        }

        function fn_ValidaAlta() {
            $.ajax({
                url: "InformacionPaciente.aspx/ValidaAlta",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (dato1, datos2, dato3) {
                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "1") {
                    $("#btnEnviarSolicitud").unbind("click");

                    $("#btnEnviarSolicitud").click(function () {
                        $.JMensajePOPUP("Mensaje del Sistema Clinica San Felipe", "El paciente se encuentra de alta.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()");
                    });
                }
            });
        }

        //Cmendez 13/05/2022 
        $(".JTEXTO").keypress(function (e) {
            var ValidaTilde = /[|'><]/;
            if (ValidaTilde.test(String.fromCharCode(event.which))) {
                event.preventDefault();
            }
        });
        $(".JTEXTO").blur(function () {
            for (var i = 0; i < this.value.length; i++) {
                if (this.value.includes('|')) {
                    $(this).val($(this).val().replace("|", " "));
                    i--;
                }
                if (this.value.includes("'")) {
                    $(this).val($(this).val().replace("'", " "));
                    i--;
                }
                if (this.value.includes("<")) {
                    $(this).val($(this).val().replace("<", " "));
                    i--;
                }
                if (this.value.includes(">")) {
                    $(this).val($(this).val().replace(">", " "));
                    i--;
                }
                if (this.value.includes("&")) {
                    $(this).val($(this).val().replace("&", ""));
                    i--;
                }
            }
        });
        //Fin
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="JFILA">
        <div class="JCELDA-12">
            <div class="DatosUsuario" id="divDatosUsuario">
                           
            </div>
        </div>
    </div>
    <div class="JCONTENEDOR-TAB1" id="divContenedorPetitorio" runat="server">
        
    </div>
        <div class="JFILA">
        <div class="JCELDA-12">
            <div class="JDIV-CONTROLES">
                <span class="JETIQUETA_3">OBSERVACIÓN</span>
            </div>        
        </div>
    </div>
    <div class="JFILA">
        <div class="JCELDA-12" runat="server" id="divObservacion">
            
        </div>
    </div>
    <%--<div class="JFILA">
        <div class="JCELDA-12">
            <div class="JDIV-CONTROLES">
                <textarea rows="5" cols="1" id="txtObservacionPetitorioLaboratorio" class="JTEXTO"></textarea>
                <input type="hidden" runat="server" id="hfObservacionPetitorio" />
            </div>
        </div>
    </div>--%>
    <div class="JFILA">
        <div class="JCELDA-1">
            <div class="JDIV-CONTROLES">
                <input type="button" value="Enviar Solicitud" id="btnEnviarSolicitud" />
                <%--<asp:Button ID="btnEnviarSolicitud" runat="server" Text="Enviar Solicitud"  />--%> <%--OnClientClick="return fn_EnviaPetitorio();"--%>
                <input type="hidden" id="hfValoresCheck" runat="server" />
            </div>
        </div>
        <div class="JCELDA-2">
            <div class="JDIV-CONTROLES">
                <input type="button" value="Volver a la Atencion..." id="btnVolverAtencion" />
            </div>
        </div>
    </div>
</asp:Content>
