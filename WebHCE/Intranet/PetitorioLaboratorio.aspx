<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="PetitorioLaboratorio.aspx.vb" Inherits="WebHCE.PetitorioLaboratorio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/JTabs.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/JTabs.js" type="text/javascript"></script>
    <link href="../Styles/JTreeview.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/JGeneral.js" type="text/javascript"></script>
    <style type="text/css">
        @media (max-width:768px) { 
            .JDIVCHECK
            {
                width:100% !important;
                }
       }
       
       
        .JSBTABS ul 
        {
            padding:8px 0;
            }
            
        .JSBTABS ul li{
            margin: 0 2px;
            padding: 10px 25px;
            border-radius: 0;
        }
    </style>
    <script type="text/javascript">
        var ValoresCheck = "";
        var TextoCheck = "";
        var aValores = "";
        var aCheckPerfil;

        $(document).ready(function () {
            /*CARGANDO LOS DATOS DEL USUARIO EN LA PARTE SUPERIOR*/
            $("#divDatosUsuario").load("Utilidad/DatosUsuarioPopUp.aspx", function () {
            });

            fn_ValidaCheck();
            $(".JCONTENEDOR-TAB1").find("input[type='checkbox']").click(function () {
                var objeto = $(this);
                $.ajax({ url: "PetitorioLaboratorio.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "") {
                        //fn_ValidaCheck();
                        fn_LOAD_VISI();
                        //en caso sea perfil - 18/01/2016
                        if (objeto.attr("name") != "0") {
                            if (objeto.prop("checked") == false) { //JB - EN CASO DESMARQUE EL PERFIL                                
                                var CodigoC = objeto.val();
                                $.ajax({
                                    url: "PetitorioLaboratorio.aspx/VerificarPerfil",
                                    type: "POST",
                                    contentType: "application/json; charset=utf-8",
                                    data: JSON.stringify({
                                        Codigo: CodigoC
                                    }),
                                    dataType: "json",
                                    error: function (dato1, datos2, dato3) {
                                    }
                                }).done(function (oOB_JSON) {
                                    fn_LOAD_OCUL();
                                    if (oOB_JSON.d.split(";")[0] != "ERROR") {
                                        $(".JCONTENEDOR-TAB1").find("input[type='checkbox']").each(function () {
                                            var objeto = $(this);
                                            var IdControlCheck = $(this).val();
                                            for (var i = 0; i < oOB_JSON.d.split(";").length; i++) {
                                                if (oOB_JSON.d.split(";")[i].trim() == IdControlCheck) {
                                                    objeto.prop("checked", false);
                                                    fn_ValidaCheck();
                                                }
                                            }
                                        });
                                    }
                                });
                                return;
                            }
                            var CodigoC = objeto.val();
                            $.ajax({
                                url: "PetitorioLaboratorio.aspx/VerificarPerfil",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                data: JSON.stringify({
                                    Codigo: CodigoC
                                }),
                                dataType: "json",
                                error: function (dato1, datos2, dato3) {
                                }
                            }).done(function (oOB_JSON) {
                                fn_LOAD_OCUL();
                                if (oOB_JSON.d.split(";")[0] != "ERROR") {
                                    $(".JCONTENEDOR-TAB1").find("input[type='checkbox']").each(function () {
                                        var objeto = $(this);
                                        var IdControlCheck = $(this).val();
                                        for (var i = 0; i < oOB_JSON.d.split(";").length; i++) {
                                            if (oOB_JSON.d.split(";")[i].trim() == IdControlCheck) {
                                                objeto.prop("checked", true);
                                                fn_ValidaCheck();
                                            }
                                        }
                                    });
                                }
                            });
                        } else {
                            fn_ValidaCheck();
                            fn_LOAD_OCUL();
                        }                        
                    } else {
                        aValores = oOB_JSON.d.toString().split(";");
                        $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                    }
                });
            });

            //BOTON ENVIAR SOLICITUD
            $("#btnEnviarSolicitud").click(function () {
                $.ajax({ url: "PetitorioLaboratorio.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "") {
                        /*fn_ValidaCheck(); INICIO - 19/01/2017
                        $.JMensajePOPUP("Confirmación", "¿Desea enviar los Análisis seleccionados? <br /><br />" + TextoCheck, "ADVERTENCIA", "Si;No", "fn_EnviaPetitorioLaboratorio();fn_oculta_mensaje()");*/
                        //INICIO - 19/01/2017
                        fn_ValidaCheck();
                        if (ValoresCheck.trim() != "") {
                            var ValorEnviar = ValoresCheck + "|" + TextoCheck + "|" + $("#txtObservacionPetitorioLaboratorio").val();
                            var aValores = [ValorEnviar];
                            $.JPopUp("Petitorio Laboratorio", "PopUp/PetitorioLab.aspx", "2", "Aceptar;Salir", "fn_AceptarPetitorioLab();fn_cerrar_petitoriolab()", 50, "", aValores);
                        }
                        //FIN - 19/01/2017
                    } else {
                        aValores = oOB_JSON.d.toString().split(";");
                        $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                    }
                });
            });

            $("#btnVolverAtencion").click(function () {
                window.history.back();
            });

            fn_ValidaAlta();
        });

        function fn_ExpiraSession() {
            window.location.href = aValores[1];
        }

        function fn_EnviaPetitorioLaboratorio() {
            fn_oculta_mensaje_rapido();
            if (ValoresCheck.trim() != "") {
                fn_LOAD_VISI();
                $.ajax({
                    url: "PetitorioLaboratorio.aspx/EnviarSolicitudPetitorio",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                        CodigosPetitorioLaboratorio: ValoresCheck,
                        Descripcion: $("#txtObservacionPetitorioLaboratorio").val()
                    }),
                    dataType: "json",
                    error: function (dato1, datos2, dato3) {
                    }
                }).done(function (oOB_JSON) {
                    fn_LOAD_OCUL();
                    if (oOB_JSON.d != "OK") {
                        $.JMensajePOPUP("Error", oOB_JSON.d, "ERROR", "Aceptar", "fn_oculta_mensaje()");
                    } else {
                        $.JMensajePOPUP("Exito", "Se Envió la solicitud de los análisis de laboratorio seleccionados.", "OK", "Aceptar", "fn_oculta_mensaje()");
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
                if ($(this).attr("name") != "0") { //si es perfil
                    if ($(this).prop("checked")) {
                        CheckActivo = true;
                    }
                } else { //si no es perfil se enviara
                    if ($(this).prop("checked")) {
                        ValoresCheck += $(this).val() + ";";
                        TextoCheck += $(this).next().html().trim() + ";" //"<br />"; 19/01/2017 - REEAMPLAZADO POR ";"
                        CheckActivo = true;
                    }
                }
            });
            ValoresCheck = ValoresCheck.substring(0, ValoresCheck.lastIndexOf(";"));
            if (CheckActivo == false) {
                $("#btnEnviarSolicitud").prop("disabled", "disabled");
            } else {
                $("#btnEnviarSolicitud").removeAttr("disabled");
            }
        }

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


        function fn_OCultaMensajeLimpiaChech() {
            $.ajax({
                url: "InformacionPaciente.aspx/VolverPantallaAbrirAcordeon",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    SeccionAbrir: "LAB"                    
                }),
                error: function (dato1, datos2, dato3) {
                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "OK") {
                    fn_oculta_mensaje();
                    fn_LimpiaCheck();
                    fn_LOAD_VISI();
                    window.location.href = "InformacionPaciente.aspx";
                }
            });            
        }


        function fn_LimpiaCheck() {
            $(".JCONTENEDOR-TAB1").find("input[type='checkbox']").each(function () {
                $(this).prop("checked", false);
            });
            fn_ValidaCheck();
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
        <%--<div class="JSBTABS">
            <label for="chkTABS" class="JSBMOSTRAR_TABS"></label>
            <input type="checkbox" id="chkTABS" class="chkTAB-CHECK" />
            <ul>
                <li class="JSBTAB_ACTIVO">
                    <a>GENERALES</a>
                </li>                
            </ul>
        </div>
        <div class="JCUERPO">
            <div class="JFILA" style="overflow:auto;width:2500px;">
                <div class="JCELDA-2" style="width:200px;">
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <span class="JETIQUETA_5">PERFILES</span>
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" /><span class="JETIQUETA_4">Laboratorio 1</span>
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" /><span class="JETIQUETA_4">Laboratorio 1</span>
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" /><span class="JETIQUETA_4">Laboratorio 1</span>
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" /><span class="JETIQUETA_4">Laboratorio 1</span>
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                </div>
                <div class="JCELDA-2" style="width:200px;">
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>                    
                </div>
                <div class="JCELDA-2" style="width:200px;">
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>                    
                </div>
                <div class="JCELDA-2" style="width:200px;">
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>                    
                </div>
                <div class="JCELDA-2" style="width:200px;">
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>                    
                </div>
                <div class="JCELDA-2" style="width:200px;">
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>                    
                </div>
                <div class="JCELDA-2" style="width:200px;">
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 9999
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <input type="checkbox" />Laboratorio 1
                        </div>
                    </div>                    
                </div>
            </div>
        </div>--%>
    </div>
    <div class="JFILA">
        <div class="JCELDA-12">
            <div class="JDIV-CONTROLES">
                <span class="JETIQUETA_3">OBSERVACIÓN</span>
            </div>        
        </div>
    </div>
    <div class="JFILA">
        <div class="JCELDA-12">
            <div class="JDIV-CONTROLES">
                <textarea rows="5" cols="1" id="txtObservacionPetitorioLaboratorio" class="JTEXTO"></textarea>
            </div>
        </div>
    </div>
    <div class="JFILA">
        <div class="JCELDA-1">
            <div class="JDIV-CONTROLES">
                <input type="button" value="Enviar Solicitud" id="btnEnviarSolicitud" />
            </div>
        </div>
        <div class="JCELDA-2">
            <div class="JDIV-CONTROLES">
                <input type="button" value="Volver a la Atencion..." id="btnVolverAtencion" />
            </div>
        </div>
    </div>
    
</asp:Content>

