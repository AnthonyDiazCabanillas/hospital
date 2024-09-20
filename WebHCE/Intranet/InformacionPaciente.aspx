<%--''====================================================================================================
 @Copyright Clinica San Felipe S.A.C. 2023. Todos los derechos reservados.
====================================================================================================
 MODIFICACIONES:
 Version  Fecha       Autor       Requerimiento
 1.0      18/12/2023  CRODRIGUEZ  REQ 2023-017525 No acceso a escalas de enfermería
 1.1      20/02/2024  FGONZALES   REQ 2023-020511 cambiar campo obligatorio a opcional
 1.2      19/06/2024  FGUEVARA    REQ-2024-011009  RESULTADOS ROE - HC
====================================================================================================--%>
<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="InformacionPaciente.aspx.vb" Inherits="WebHCE.InformacionPaciente" EnableEventValidation="false" %>
<%@ Register Src="ControlesUsuario/PopupProcedimientoConsentimiento.ascx" TagName="cuProcedimientoConsentimiento" TagPrefix="uc1" %>
<%@ Register Src="ControlesUsuario/PopUpAltaMedicaEpicrisis.ascx" TagName="cuAltaMedicaEpicrisis" TagPrefix="uc2" %>
<%@ Register Src="ControlesUsuario/PopUpFechaReceta.ascx" TagName="cuPopUpFechaReceta" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/JTabs.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/JTabs.js" type="text/javascript"></script>
    <link href="../Styles/JTreeview.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/AutocompleteMultiple.js" type="text/javascript"></script>
    <link href="../Styles/AutocompleteMultiple.css" rel="stylesheet" type="text/css" />
    <style type="text/css"> 
        
         .JSTABLASCALA tr:hover td
         {
            background-color: #f6f6f6; /*#E8E8E8 - GRIS*/
            color: #134B8D !important;
         }
        .selectize-input /*estilo autocomplete multiple - JB*/
        {
            border: 1px solid #4BACFF;
            display: inline-block;
            width: 100%;
            overflow: hidden;
            position: relative;
            z-index: 1;
            -webkit-box-sizing: border-box;
            -moz-box-sizing: border-box;
            box-sizing: border-box;
            border-radius: 5px;
            padding: 0.1em;
        }
        
        .ClaseBuscadorPatologia1
        {
            background-color: transparent !important;
            background-image: none !important;
            color: #134B8D !important;
            text-shadow: none !important;
            -webkit-box-shadow: none !important;
            box-shadow: none !important;
        }
        .ClaseBuscadorPatologia2
        {
            background: transparent !important;
            border: none !important;
        }
        .buttonescalaseindicaciones {
        
            position: relative;
            float: right;
            width: 160px;
            height: 40px;
            font-size: 18px;
        }
    </style>
<script type="text/javascript">
    var sIdControlValidado = ""; /*variable usada para la basurita de Internet Explorer que no reconoce una funcion javascript*/
    var sAtencionAnterior = "";
    var sCodigoAtencionActual = "";
    var aValores = "";
    var sOpcionRadio = "";
    var valorFR = 0;
    var valorSAT = 0;
    var valorTEMP = 0;
    var valorFC = 0;
    var gridViewInfusiones = "";
    var rowInfusiones = "";


    function CierraVentanaM() {
        $.ajax({
            url: "InformacionPaciente.aspx/ValidarVentanaMultiple",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (dato1, datos2, dato3) {
            }
        }).done(function (oOB_JSON) {
            if (oOB_JSON.d != "") {
                //$.JMensajePOPUP("Aviso", oOB_JSON.d, "ADVERTENCIA", "Cerrar", "fn_SalirOtraVentanaAbierta()");                
                window.location.href = "ConsultaPacienteHospitalizado.aspx";
            } else {

            }
        });        
        setTimeout(CierraVentanaM, 1000);
    }

    CierraVentanaM();
    

    function pageLoad() {        
        fn_RefrescaEventosGridInfusiones();

        //fn_SetValoresPatologia(); //para setear valores perdidos al cargar la pantalla por algun evento        

        //checkbox otras patologias            
        $("#chkPatologia_otros").unbind("click");
        $("#chkPatologia_otros").click(function () {
            if ($(this).prop("checked") == true) {
                $('#ddlOtrosPatologia')[0].selectize.enable();
            } else {
                $('#ddlOtrosPatologia')[0].selectize.disable();
            }
        });

        //campo otras patologias
        $(".select-otrospatologia").unbind("change");
        $('.select-otrospatologia').change(function () {
            $("#" + "<%=hfIdPatologiaSeleccionado.ClientID %>").val("");
            $("#" + "<%=hfIdPatologiaSeleccionado.ClientID %>").val($(this).val());
        });

        //checkbox de patologias
        $(".JCHECK-PATOLOGIA").find(":input[type='checkbox']").click(function () {
            var IdePatologia = "";
            $(".JCHECK-PATOLOGIA").find(":input[type='checkbox']").each(function () {
                var ObjetoCheck = $(this);
                if ($(this).prop("checked") == true && $(this).prop("id") != "chkPatologia_otros") {
                    //Observaciones Cmendez 02/05/2022 
                    IdePatologia += ObjetoCheck.val() + "|";
                }
            });
            $("#" + "<%=hfCheckPatologiaSeleccionado.ClientID %>").val("");
            $("#" + "<%=hfCheckPatologiaSeleccionado.ClientID %>").val(IdePatologia);            
        });

        //
        $("#" + "<%=TxtFechaUltimaRegla.ClientID %>").unbind("keydown");
        $("#" + "<%=TxtFechaUltimaRegla.ClientID %>").keydown(function (event) {
            if (event.which != 8) {
                event.preventDefault();
            }
        });

        if ($("[id*=gvPatologia]").find("tr").length == 2) {
            fn_LimpiarGridPatologia();
        }
        fn_ListarPatologiaSeleccionadas();
    }


    $(document).ready(function () {
        fn_ConsultaSeccion("", "", 1, "");
        fn_InicializarCombo();

        $(".JCHEK-TABS").click(function () {
            $(this).parent().find("> .JCONTENIDO-TAB").css("display", "none");
            $(this).parent().find("> .JCONTENIDO-TAB").eq($(this).parent().find(".JCHEK-TABS").index(this)).css("display", "block");
        });

        //marcando el primer tab
        $(".JTABS").each(function () {
            $(this).find("> .JCHEK-TABS").eq(0).click();
        });

        /*10/11/2016*/
        $("#divCierraSesion").click(function () {
            $.ajax({
                url: "InformacionPaciente.aspx/CerrarSesion",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (dato1, datos2, dato3) {
                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d.split(";")[0] != "OK") {
                    //alert(oOB_JSON.d.split(";")[1]);
                } else {
                    //window.location.href = "ConsultaPacienteHospitalizado.aspx" JB - 15/10/2020
                    window.close();
                }
            });
        });

        fn_SetValoresPatologia();

        /*CARGANDO DATOS DE LA BOTONERA (BOTONES DE LA PARTE SUPERIOR)*/
        $("#divBotonera").load("Utilidad/Botonera.aspx", function ()
        {
            //VERIFICANDO ALERTAS
            $.ajax({
                url: "InformacionPaciente.aspx/VerificarAlertas", //verifica si hay alertas
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (dato1, datos2, dato3) {
                }
            }).done(function (oOB_JSON) {
                aValoresAlerta = oOB_JSON.d.toString().split(";");
                if (aValoresAlerta[0] == "ALERTA") {

                    /* JB - COMENTADO - 06/08/2019
                    $("#imgAlerta").addClass("JIMG-ALERTA");
                    var aVA_DEMO = ["#DatosUsuarioAlerta", "Utilidad/DatosUsuarioPopUp.aspx", ""];
                    if (aValoresAlerta[1] == "U") { //MOSTRAR ALERTA PARA USUARIO DIFERENTE A MEDICO
                    $.JPopUp("Alerta", "PopUp/Alerta.aspx", "2", "Verificar;Salir", "fn_VerificarAlerta();fn_oculta_popup()", 65, aVA_DEMO);
                    }
                    if (aValoresAlerta[1] == "M") {
                    $.ajax({
                    url: "InformacionPaciente.aspx/VerificarAlertas2",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    error: function (dato1, datos2, dato3) {
                    }
                    }).done(function (oOB_JSON) {
                    if (oOB_JSON.d != "") {
                    $("#imgAlerta").attr("title", oOB_JSON.d);
                    $("#imgAlerta").next().html(oOB_JSON.d);
                    }
                    });
                    }*/
                    //if (aValoresAlerta[1] == "M") {
                    $.ajax({
                        url: "InformacionPaciente.aspx/VerificarAlertas2", //obtiene mensaje para el tittle del boton de alerta
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        error: function (dato1, datos2, dato3) {
                        }
                    }).done(function (oOB_JSON) {
                        if (oOB_JSON.d != "") {
                            $("#imgAlerta").attr("title", oOB_JSON.d);
                            $("#imgAlerta").next().html(oOB_JSON.d);
                            $("#imgAlerta").attr("src", "../Imagenes/AlertaR.png");
                            $("#imgAlerta").addClass("JIMG-ALERTA");
                        }
                    });
                    //}
                } else {
                    $("#imgAlerta").removeClass("JIMG-ALERTA");
                    $("#imgAlerta").attr("title", "Alerta");
                    $("#imgAlerta").next().html("Alerta");
                    $("#imgAlerta").attr("src", "../Imagenes/Alerta.png");
                }
            });
            $("#imgReporteHM").css("display", "none"); //JB - SE OCULTA IMPRIMIR - 17/04/2020
        });

        /*CARGANDO LOS DATOS DEL USUARIO EN LA PARTE SUPERIOR*/
        $("#DatosUsuarioInformacionPaciente").load("Utilidad/DatosUsuario.aspx", function ()
        {
            $("#DatosUsuarioInformacionPaciente").find("#spPresentaAlergia").css("cursor", "pointer");
            $("#DatosUsuarioInformacionPaciente").find("#spPresentaAlergia").click(function ()
            {
                $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "")
                    {
                        let sDescripcionAcordeon = 'DECLARATORIA ALERGIA';
                        fn_GuardaLog(sDescripcionAcordeon, "Se ingreso a la opción para agregar Alergias del paciente ");
                        var aVA_DEMO = ["#DatosUsuarioDeclaratoriaAlergia", "Utilidad/DatosUsuarioPopUp.aspx", "ALERGIA"];
                        $.JPopUp("", "PopUp/DeclaratoriaAlergia.aspx", "2", "Guardar;Salir", "fn_GuardarDeclaratoriaAlergia();fn_CierraPopup()", 85, aVA_DEMO); //fn_EditarDeclaratoriaAlergia
                    } else {
                        aValores = oOB_JSON.d.toString().split(";");
                        $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                    }
                });
            });
        });

        $(".JBUSQUEDA-ESPECIAL").parent().find("input[type='text']").keypress(function (e) {
            var ValidaTilde = /[ÁÉÍÓÚáéíóú|<>'&]/;
            if (ValidaTilde.test(String.fromCharCode(event.which))) {
                event.preventDefault();
            }
        });
        //Cmendez 03/05/2022
        $(".JTEXTO").keypress(function (e) {
            var ValidaTilde = /[|<>'&]/;
            if (ValidaTilde.test(String.fromCharCode(event.which))) {
                event.preventDefault();
            }
        });
        $(".JDIV-CONTROLES").keypress(function (e) {
            var ValidaTilde = /[|<>'&]/;
            if (ValidaTilde.test(String.fromCharCode(event.which))) {
                event.preventDefault();
            }
        });
        //Fin
        $("#divCambiarPassword").click(function () {
            $.JPopUp("Cambiar Contraseña", "PopUp/CambiarPassword.aspx", "2", "Cambiar Contraseña;Salir", "fn_CambiarContraseña();fn_oculta_popup()", 40, "");
        });

        //CARGANDO CONTROLES DINAMICOS
        /*$.ajax({
        url: "InformacionPaciente.aspx/ControlesDinamicos_2",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
        TipoAtencion: ""
        }),
        dataType: "json",
        error: function (dato1, datos2, dato3) {

        }
        }).done(function (oOB_JSON) {
        $("#divContenedorDinamico").append(oOB_JSON.d);
        $("#divContenedorDinamico").find(".JDIV-GRUPO").each(function () {
        fn_CargandoDatosControlesGrupo($(this).prop("id").trim(), "", "");
        });
        fn_RenderizarTabs();
        });*/


        //INICIO - JB - NUEVO CODIGO PARA CARGA DE CONTROLES DINAMICOS, SE COMENTA EL AJAX LINEAS ARRIBA
        fn_CrearEventoControles();
        fn_RenderizarTabs();
        $(".JCAMP-D").attr("disabled", "disabled"); // JB - NUEVO - 15/04/2019
        fn_MostraNewsPews();

        $("[class*=JDECIMAL]").keypress(function (event) {
            //alert(String.fromCharCode(event.which));            
            if (String.fromCharCode(event.which) == ".") {
                if ($(this).val() == "") {
                    event.preventDefault();
                }
                if ($(this).val().indexOf(".") != -1) {
                    event.preventDefault();
                }
            } else {
                var ValidacionNumerica = /[0-9]/;
                if (!ValidacionNumerica.test(String.fromCharCode(event.which))) {
                    event.preventDefault();
                }
            }
        });

        if ($("input[id^='Opt_SiDiferidoAnoRecto']").prop("checked")) {
            $("input[id^='txt_DescripcionEsfinterAnoRecto']").removeAttr("disabled");
            $("input[id^='txt_DescripcionLesionesAnoRecto']").removeAttr("disabled");
            $("input[id^='txt_DescripcionProstataAnoRecto']").removeAttr("disabled");
        }
        //babinsky            
        if ($("input[id^='chk_babinskyD']").prop("checked") == false) {
            $("input[id^='Opt_PositivoBabinskyDerecha']").attr("disabled", "disabled");
            $("input[id^='Opt_NegativoBabinskyDerecha']").attr("disabled", "disabled");
            $("input[id^='Opt_IndiferenteBabinskyDerecha']").attr("disabled", "disabled");
        }
        if ($("input[id^='chk_babinskyI']").prop("checked") == false) {
            $("input[id^='Opt_PositivoBabinskyIzquierda']").attr("disabled", "disabled");
            $("input[id^='Opt_NegativoBabinskyIzquierda']").attr("disabled", "disabled");
            $("input[id^='Opt_IndiferenteBabinskyIzquierda']").attr("disabled", "disabled");
        }
        //FIN - JB - NUEVO CODIGO PARA CARGA DE CONTROLES DINAMICOS

        //INICIO - JB - PARA REEMPLAZAR COMILLA SIMPLE Y COLOCARLE SU VERDADERO VALOR        
        $("[id*=divContenedorDinamico]").find("input[type='text']").each(function () {
            $(this).val($(this).val().replace(/SGL_QUOTE/g, "'"));            
            //$("[id*=divContenedorDinamico]").find("input[type='text']").val().replace(/SGL_QUOTE/g, "'");
        });
        //FIN - JB - PARA REEMPLAZAR COMILLA SIMPLE Y COLOCARLE SU VERDADERO VALOR

        //Comentado Christian Méndez
        //Validar caracter extraño; por palote |
        //No permite ingresar el caracter palote | y apostrofe en campo dinamico
        //Se cambia al campo inputext y al camppo textarea
        $("[id*=divContenedorDinamico]").find("input[type='text']").keypress(function (e) {
            var ValidaTilde = /[|'<>&]/;
            if (ValidaTilde.test(String.fromCharCode(event.which))) {
                event.preventDefault();
            }
        });
        $("[id*=divContenedorDinamico]").find("textarea").keypress(function (e) {
            var ValidaTilde = /[|'<>&]/;
            if (ValidaTilde.test(String.fromCharCode(event.which))) {
                event.preventDefault();
            }
        });
        //Pruebas Cmendez lee el registro y elimina todos los palotes | que fueron copiados
        $(".JTEXTO").blur(function () {
            for (var i = 0; i < this.value.length; i++) {
                if (this.value.includes("|")) {
                    $(this).val($(this).val().replace("|", ""));
                    i--;
                }
                if (this.value.includes("'")) {
                    $(this).val($(this).val().replace("'", ""));
                    i--;
                }
                if (this.value.includes("<")) {
                    $(this).val($(this).val().replace("<", ""));
                    i--;
                }
                if (this.value.includes(">")) {
                    $(this).val($(this).val().replace(">", ""));
                    i--;
                }
                if (this.value.includes("&")) {
                    $(this).val($(this).val().replace("&", ""));
                    i--;
                }
            }
        });
        //CMENDEZ 16/05/2022
        $(".JTEXTO-6").blur(function () {
            for (var i = 0; i < this.value.length; i++) {
                if (this.value.includes("|")) {
                    $(this).val($(this).val().replace("|", ""));
                    i--;
                }
                if (this.value.includes("'")) {
                    $(this).val($(this).val().replace("'", ""));
                    i--;
                }
                if (this.value.includes("<")) {
                    $(this).val($(this).val().replace("<", ""));
                    i--;
                }
                if (this.value.includes(">")) {
                    $(this).val($(this).val().replace(">", ""));
                    i--;
                }
                if (this.value.includes("&")) {
                    $(this).val($(this).val().replace("&", ""));
                    i--;
                }
            }
        });

        $(".JTEXTO-10").blur(function () {
            for (var i = 0; i < this.value.length; i++) {
                if (this.value.includes("|")) {
                    $(this).val($(this).val().replace("|", ""));
                    i--;
                }
                if (this.value.includes("'")) {
                    $(this).val($(this).val().replace("'", ""));
                    i--;
                }
                if (this.value.includes("<")) {
                    $(this).val($(this).val().replace("<", ""));
                    i--;
                }
                if (this.value.includes(">")) {
                    $(this).val($(this).val().replace(">", ""));
                    i--;
                }
                if (this.value.includes("&")) {
                    $(this).val($(this).val().replace("&", ""));
                    i--;
                }
            }
        });

        $(".JDECIMAL").blur(function () {
            var ValorDecimal = /^[0-9]+(.[0-9]+)?$/;
            for (var i = 0; i < this.value.length; i++) {
                if (!ValorDecimal.test($(this).val())) {
                    $(this).val("");
                }
            }
        });

        $(".JHORA").blur(function () {
            var ValidacionHora = /^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$/;
            for (var i = 0; i < this.value.length; i++) {
                if (!ValidacionHora.test($(this).val())) {
                    $(this).val("");
                }
            }
        });

        $(".JDECIMAL-10").blur(function () {
            var ValorDecimal = /^[0-9]+(.[0-9]+)?$/;
            for (var i = 0; i < this.value.length; i++) {
                if (!ValorDecimal.test($(this).val())) {
                    $(this).val("");
                }
            }
        });
         
        $(".JNUMERO").blur(function () {
            var ValidacionNumerica = /[0-9]/;
            for (var i = 0; i < this.value.length; i++) {
                if (!ValidacionNumerica.test($(this).val())) {
                    $(this).val("");
                }
            }
        });
        //Fin

        $("[id*=divContenedorDinamico]").find("input[type='text']").blur(function () {
            for (var i = 0; i < this.value.length; i++) {                
                if (this.value.includes('|')) {
                    $(this).val($(this).val().replace("|", ""));
                    i--;
                }
                if (this.value.includes("'")) {
                    $(this).val($(this).val().replace("'", ""));
                    i--;
                }
                if (this.value.includes("<")) {
                    $(this).val($(this).val().replace("<", ""));
                    i--;
                }
                if (this.value.includes(">")) {
                    $(this).val($(this).val().replace(">", ""));
                    i--;
                }
                if (this.value.includes("&")) {
                    $(this).val($(this).val().replace("&", ""));
                    i--;
                }
            }
        });
        $("[id*=divContenedorDinamico]").find("textarea").blur(function () {
            for (var i = 0; i < this.value.length; i++) {
                if (this.value.includes('|')) {
                    $(this).val($(this).val().replace("|", ""));
                    i--;
                }
                if (this.value.includes("'")) {
                    $(this).val($(this).val().replace("'", ""));
                    i--;
                }
                if (this.value.includes("<")) {
                    $(this).val($(this).val().replace("<", ""));
                    i--;
                }
                if (this.value.includes(">")) {
                    $(this).val($(this).val().replace(">", ""));
                    i--;
                }
                if (this.value.includes("&")) {
                    $(this).val($(this).val().replace("&", ""));
                    i--;
                }
            }
        });

      //Fin

        fn_CargaNotaIngreso();
        //fn_CargarEvolucionClinica();
        fn_CargarEvolucionClinica2("1", "0");
        //fn_CargarControlClinicoIM();
        fn_CargarControlClinicoIM2("1", "", "0");
        fn_CargarPatologias();

        /*CODIGO PARA FUNCIONAMIENTO DEL ACORDEON*/
        $(".JDIV_ACOR_CONTENIDO").hide();
        $(".JDIV_ACOR_CONTENEDOR > label").click(function () {
            var sDescripcionAcordeon = $(this).html().trim();
            if ($(this).next().css("display") != "block" && $(this).next().css("display") != "inline-block") {
                $(".JDIV_ACOR_CONTENIDO").slideUp();
                $(this).next().slideDown();
                fn_GuardaLog(sDescripcionAcordeon, "Se ingreso a la opción");
            } else {
                $(this).next().slideUp();
            }
            $("#divAtencionesAnteriores").css("height", $("#divTituloAtenciones").outerHeight(true) + "px");
        });

        if ($("[id*=hfAcordeonAbierto]").val() != "") {
            if ($("[id*=hfAcordeonAbierto]").val() == "LAB") {
                $(".JDIV_ACOR_CONTENEDOR > label").eq(6).trigger("click");
                $("[id*=hfAcordeonAbierto]").val("");
            }
            if ($("[id*=hfAcordeonAbierto]").val() == "IMG") {
                $(".JDIV_ACOR_CONTENEDOR > label").eq(7).trigger("click");
                $("[id*=hfAcordeonAbierto]").val("");
            }
        }


        fn_CargarAtencionesAnteriores();


        /*opcion 3*/
        $("#divContenedorAtenciones").mouseover(function () {
            $(".JCELDA-0").css("height", $("#divAtencionesAnteriores").outerHeight(true) + 25);
            $(this).stop().animate({
                right: "5px"
            }, 500);
        });
        $("#divContenedorAtenciones").mouseleave(function () {
            $(this).stop().animate({
                right: "-450px"
            }, 500);
        });

        /*JB - 19/06/2020 - YA NO VA 
        if ($("#" + "<=hfHistoriaClinicaMedico.ClientID %>").val() == "DESHABILITAR") {
        $("#" + "<=divContenedorDinamico.ClientID %>" + " :input").attr("disabled", "disabled");
        }*/

        if ($("#" + "<%=hfHistoriaClinicaHoras.ClientID %>").val() != "0" && $("#" + "<%=hfHistoriaClinicaHoras.ClientID %>").val() != "") {
            $("#" + "<%=divContenedorDinamico.ClientID %>" + " :input").attr("disabled", "disabled");
            //attr("enabled", "disabled");
            //Cmendez Pruebas enabled
            //cambio
        }

        /*********************************************************************************************************/
        /************************************************LABOTATORIO**********************************************/
        /*********************************************************************************************************/
        //CARGANDO LISTA ANALISIS SELECCIONADOS
        $("#divGridLaboratorio").load("GridViewAjax/GridLaboratorio.aspx", function () {
        });
        //INICIO - 19/01/2017
        $("#chkProgramarHoraLab").click(function () {
            if ($("#divGridLaboratorio").find(".JSBTABLA .HoraProg").find("input[type='checkbox']").length == 0) {
                return false;
            }
            if ($(this).prop("checked")) {
                $("#txtFechaProgramarHoraLab").removeAttr("disabled");
                $("#txtHoraProgramarHoraLab").removeAttr("disabled");

                $("#divGridLaboratorio").load("GridViewAjax/GridLaboratorio.aspx", function () {
                    $("#divGridLaboratorio").find(".JSBTABLA .HoraProg").find("input[type='checkbox']").each(function () {
                        $(this).removeAttr("disabled");
                        $(this).prop("checked", true);
                    });
                });
            } else {
                $("#txtFechaProgramarHoraLab").attr("disabled", "disabled");
                $("#txtHoraProgramarHoraLab").attr("disabled", "disabled");

                $("#divGridLaboratorio").load("GridViewAjax/GridLaboratorio.aspx", function () {
                    $("#divGridLaboratorio").find(".JSBTABLA .HoraProg").find("input[type='checkbox']").each(function () {
                        $(this).attr("disabled", "disabled");
                        $(this).prop("checked", false);
                    });
                });
            }

        });

        $("#txtFechaProgramarHoraLab").blur(function () {
            if ($(this).val().trim() != "") {
                var fecha_actual = new Date();
                var fecha1;
                if ($("#txtHoraProgramarHoraLab").val().trim() == "") {
                    fecha1 = new Date($("#txtFechaProgramarHoraLab").val().split("/")[2], (parseInt($("#txtFechaProgramarHoraLab").val().split("/")[1]) - 1), $("#txtFechaProgramarHoraLab").val().split("/")[0]);
                    fecha_actual = new Date(fecha_actual.getFullYear(), fecha_actual.getMonth(), fecha_actual.getDate());

                    if (fecha1 < fecha_actual) {
                        $.JMensajePOPUP("Aviso", "La fecha a programar no debe ser menor a la fecha actual", "", "Cerrar", "fn_oculta_mensaje()", "");
                        $("#txtFechaProgramarHoraLab").val("");
                    }
                } else {
                    fecha1 = new Date($("#txtFechaProgramarHoraLab").val().split("/")[2], (parseInt($("#txtFechaProgramarHoraLab").val().split("/")[1]) - 1), $("#txtFechaProgramarHoraLab").val().split("/")[0], $("#txtHoraProgramarHoraLab").val().split(":")[0], $("#txtHoraProgramarHoraLab").val().split(":")[1]);
                    if (fecha1 < fecha_actual) {
                        $.JMensajePOPUP("Aviso", "La fecha y hora a programar no debe ser menor a la fecha actual", "", "Cerrar", "fn_oculta_mensaje()", "");
                        $("#txtFechaProgramarHoraLab").val("");
                        $("#txtHoraProgramarHoraLab").val("");
                    }
                }
            }
        });

        $("#txtHoraProgramarHoraLab").blur(function () {
            if ($(this).val().trim() != "") {
                var fecha_actual = new Date();
                var fecha1;
                if ($("#txtFechaProgramarHoraLab").val().trim() != "") {
                    fecha1 = new Date($("#txtFechaProgramarHoraLab").val().split("/")[2], (parseInt($("#txtFechaProgramarHoraLab").val().split("/")[1]) - 1), $("#txtFechaProgramarHoraLab").val().split("/")[0], $("#txtHoraProgramarHoraLab").val().split(":")[0], $("#txtHoraProgramarHoraLab").val().split(":")[1]);
                    if (fecha1 < fecha_actual) {
                        $.JMensajePOPUP("Aviso", "La fecha y hora a programar no debe ser menor a la fecha actual", "", "Cerrar", "fn_oculta_mensaje()", "");
                        $("#txtFechaProgramarHoraLab").val("");
                        $("#txtHoraProgramarHoraLab").val("");
                    }
                }
            }
        });
        //FIN - 19/01/2017

        //CREANDO TREEVIEW DE ANALISIS
        //fn_CrearTreeViewAnalisis();
        fn_CrearTreeViewAnalisis2("1", "", "0");

        //CLICK EN PETITORIO DE LABORATORIO
        $("#imgPetirorioLaboratorio").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    //AQUI EL CODIGO SI LA SESSION AUN NO EXPIRA
                    window.location.href = "PetitorioLaboratorio.aspx";
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });

        });


        $("#imgRoeLaboratorio").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    //CODIGO AQUI QUE TRAERA LA URL
                    $.ajax({
                        url: "InformacionPaciente.aspx/ObtenerUrlRoe", //obtiene mensaje para el tittle del boton de alerta
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        error: function (dato1, datos2, dato3) {
                        }
                    }).done(function (oOB_JSON) {
                        if (oOB_JSON.d != "") {
                            if (oOB_JSON.d.toString().split("*")[0].trim() == "OK") {
                                window.open(oOB_JSON.d.toString().split("*")[1].trim(), "_blank");
                            } else {
                                $.JMensajePOPUP("Error", oOB_JSON.d.toString().split("*")[1].trim(), "ERROR", "Cerrar", "fn_oculta_mensaje()");
                            }
                        }
                    });
                    //window.location.href = "PetitorioLaboratorio.aspx";
                } else {
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });

        //BUSQUEDA DE ANALISIS
        $("#imgBuscarAnalisisLaboratorio").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    if ($("#txtAnalisisLaboratorio").val().trim().length > 1) {
                        $("#divFONDO").css("display", "inline");
                        $("#divBusquedaLaboratorio").css("display", "block");
                        $("#divBusquedaLaboratorio").load("Utilidad/BusquedaLaboratorio.aspx", { Nombre: $("#txtAnalisisLaboratorio").val().trim(), TipoBusqueda: "G", Orden: "9" }, function () {
                            $(this).find("#hfTipoBusqueda").val("G");
                            fn_CrearEventoLaboratorio();
                        });
                    } else {
                        $.JMensajePOPUP("Aviso", "Debe ingresar al menos 2 caracteres para realizar una búsqueda.", "1", "Cerrar", "fn_oculta_mensaje()");
                    }
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });
        //BUSQUEDA DE FAVORITO ANALISIS
        $("#imgFavoritosAnalisisLaboratorio").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $("#divFONDO").css("display", "inline");
                    $("#divBusquedaLaboratorio").css("display", "block");
                    $("#divBusquedaLaboratorio").load("Utilidad/BusquedaLaboratorio.aspx", { Nombre: $("#txtAnalisisLaboratorio").val().trim(), TipoBusqueda: "F", Orden: "4" }, function () {
                        $(this).find("#hfTipoBusqueda").val("F");
                        fn_CrearEventoLaboratorio();
                    });
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });

        //ACTIVANDO LA BUSQUEDA CUANDO SE PRESION ENTER
        $("#txtAnalisisLaboratorio").keypress(function (e) {
            if (e.which == 13) {
                $("#imgBuscarAnalisisLaboratorio").trigger("click");
            }
        });

        //ELIMINANDO DE FAVORITOS 
        $("#chkAnalisisLaboratorioFavorito").click(function () {
            if ($(this).prop("checked") == false && ($("#hfIdeFavoritoAnalisis").val().trim() != "" && $("#hfIdeFavoritoAnalisis").val().trim() != "0")) {
                $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "") {
                        $.JMensajePOPUP("Aviso", "¿Desea eliminar de Favoritos?", "", "Si;No", "fn_EliminaFavoritoAnalisis();fn_CancelarEliminarFavoritoAnalisis()");
                    } else {
                        aValores = oOB_JSON.d.toString().split(";");
                        $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                    }
                });
            }
        });

        //BOTON AGREGAR ANALISIS
        $("#imgAgregarLaboratorio").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    var bValidacion = false;
                    var bFlgCubierto = true;
                    var bFlgFavoritoLaboratorio = false;

                    //alert($("#divGridDiagnosticos").find(".JSBTABLA tr:not(:first)").length);

                    if ($("#divGridDiagnosticos").find(".JSBTABLA tr:not(:first)").length == 0 || $("#divGridDiagnosticos").find(".JSBTABLA tr:not(:first)").length == -1) {
                        $.JMensajePOPUP("Aviso", "Debe ingresar primero un Diagnóstico.", "", "Cerrar", "fn_oculta_mensaje()");
                        return;
                    }
                    if ($("#divGridDiagnosticos").find(".JSBTABLA tr:not(:first)").length > 0) {
                        if ($("#divGridDiagnosticos").find(".JSBTABLA tr:not(:first)").find("td").length <= 1) {
                            $.JMensajePOPUP("Aviso", "Debe ingresar primero un Diagnóstico.", "", "Cerrar", "fn_oculta_mensaje()");
                            return;
                        }
                    }

                    if ($("#chkAnalisisLaboratorioFavorito").prop("checked") == true && ($("#hfIdeFavoritoAnalisis").val().trim() == "" || $("#hfIdeFavoritoAnalisis").val().trim() == "0")) {
                        bFlgFavoritoLaboratorio = true;
                    }
                    if ($("#spCodigoAnalisisLaboratorioSeleccionado").html().trim() == "") {
                        $.JMensajePOPUP("Aviso", "Seleccione un analisis.", "1", "Cerrar", "fn_oculta_mensaje()");
                        return false;
                    }



                    //*************************
                    $.ajax({
                        url: "GridViewAjax/GridLaboratorio.aspx/ValidaAnalisisExistente_AnalisisDiagnostico",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            CodigoAnalisis: $("#spCodigoAnalisisLaboratorioSeleccionado").html().trim(),
                            DscAnalisis: $("#spAnalisisLaboratorioSeleccionado").html().trim().replace('<b>', '').replace('</b>', '').replace('<span style="color:red;">', '').replace('</span>', '')
                        }),
                        dataType: "json",
                        error: function (dato1, datos2, dato3) {

                        }
                    }).done(function (oOB_JSON) {
                        if (oOB_JSON.d != "") {
                            var aValoresSesion1;
                            aValoresSesion1 = oOB_JSON.d.toString().split("*");
                            if (aValoresSesion1[0] == "EXPIRO") {
                                window.location.href = aValoresSesion1[1];
                            }
                            if (oOB_JSON.d.toString().split("*")[0].trim() == "ERROR") {

                                $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe - Error", oOB_JSON.d.toString().split("*")[1], "ERROR", "Aceptar", "fn_oculta_mensaje()");
                                return false;
                            }

                            if (oOB_JSON.d.toString().split("*")[0].trim() == "VALIDACION1") { //jb - 16/07/2020 - nuevo codigo
                                $.JMensajePOPUP("Aviso", oOB_JSON.d.toString().split("*")[1], "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()");
                                return false;
                            }
                            if (oOB_JSON.d.toString().split("*")[0].trim() == "VALIDACION2") {
                                if (oOB_JSON.d.toString().split("*")[2] == "NO") {
                                    $.JMensajePOPUP("Aviso", oOB_JSON.d.toString().split("*")[1], "ADVERTENCIA", "Si;No", "AgregarAnalisis3();fn_oculta_mensaje()");
                                } else {
                                    $.JMensajePOPUP("Aviso", oOB_JSON.d.toString().split("*")[1], "ADVERTENCIA", "Si;No", "AgregarAnalisis2();fn_oculta_mensaje()");
                                }
                                return false;
                            }


                            /*JB - COMENTADO - 26/08/2019
                            * if (oOB_JSON.d.toString().substring(0, 1) == "2") {
                            if (oOB_JSON.d.toString().split(";").length == 2) {
                            $.JMensajePOPUP("Aviso", oOB_JSON.d.toString().substring(2, oOB_JSON.d.length), "ADVERTENCIA", "Si;No", "AgregarAnalisis();fn_oculta_mensaje()");
                            return false;
                            } else {
                            $.JMensajePOPUP("Aviso", oOB_JSON.d.toString().split(";")[1], "ADVERTENCIA", "Si;No", "AgregarAnalisis3();fn_oculta_mensaje()");
                            return false;
                            }
                            }
                            if (oOB_JSON.d.toString().substring(0, 1) == "1") {
                            $.JMensajePOPUP("Aviso", oOB_JSON.d.toString().substring(2, oOB_JSON.d.length), "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()");
                            return false;
                            }                            
                            if (oOB_JSON.d.toString().substring(0, 1) == "3") {
                            $.JMensajePOPUP("Aviso", oOB_JSON.d.toString().substring(2, oOB_JSON.d.length), "ADVERTENCIA", "Cerrar", "AgregarAnalisis2()");
                            return false;
                            }*/


                            //FUNCION PARA AGREGAR ANALISIS
                            /*JB - COMENTADO - 26/08/2019
                            * fn_LOAD_VISI();
                            $.ajax({
                            url: "InformacionPaciente.aspx/AgregarAnalisis",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({
                            DescripcionReceta: $("#txtObservacionAnalisisLaboratorio").val().trim(),
                            IdAnalisis: $("#spCodigoAnalisisLaboratorioSeleccionado").html().trim(),
                            FlgCubierto: bFlgCubierto,
                            flgFavoritoLaboratorio: bFlgFavoritoLaboratorio,
                            Perfil: $("#spPerfil").html().trim()
                            }),
                            dataType: "json",
                            error: function (dato1, datos2, dato3) {

                            }
                            }).done(function (oOB_JSON_1) {
                            fn_LOAD_OCUL();
                            var aValoresSesion;
                            aValoresSesion = oOB_JSON_1.d.toString().split(";");
                            if (aValoresSesion[0] == "EXPIRO") {
                            window.location.href = aValoresSesion[1];
                            }
                            if (oOB_JSON_1.d.split(";")[0] != "OK") {
                            if (oOB_JSON_1.d.split(";")[0] == "1") {
                            $.JMensajePOPUP("Error", oOB_JSON_1.d.split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()");
                            } else {
                            $.JMensajePOPUP("Aviso", oOB_JSON_1.d.split(";")[1], "", "Cerrar", "fn_oculta_mensaje()");
                            }
                            } else {                                    
                            }
                            $("#txtAnalisisLaboratorio").val("");
                            $("#spCodigoAnalisisLaboratorioSeleccionado").html("");
                            $("#chkAnalisisLaboratorioSeleccionado").prop("checked", false);
                            $("#spAnalisisLaboratorioSeleccionado").html("");
                            $("#chkAnalisisLaboratorioFavorito").prop("checked", false);
                            $("#hfIdeFavoritoAnalisis").val("");
                            $("#spPerfil").html("");
                            $("#divGridLaboratorio").load("GridViewAjax/GridLaboratorio.aspx", function () {
                            if ($("#chkProgramarHoraLab").prop("checked")) {
                            $("#divGridLaboratorio").find(".JSBTABLA .HoraProg").find("input[type='checkbox']").each(function () {
                            $(this).removeAttr("disabled");
                            $(this).prop("checked", true);
                            });
                            }
                            });
                            });*/
                        } else {
                            //FUNCION PARA AGREGAR ANALISIS
                            fn_LOAD_VISI();

                            $.ajax({
                                url: "InformacionPaciente.aspx/AgregarAnalisis",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                data: JSON.stringify({
                                    DescripcionReceta: $("#txtObservacionAnalisisLaboratorio").val().trim(),
                                    IdAnalisis: $("#spCodigoAnalisisLaboratorioSeleccionado").html().trim(),
                                    FlgCubierto: bFlgCubierto,
                                    flgFavoritoLaboratorio: bFlgFavoritoLaboratorio,
                                    Perfil: $("#spPerfil").html().trim()
                                }),
                                dataType: "json",
                                error: function (dato1, datos2, dato3) {

                                }
                            }).done(function (oOB_JSON_1) {
                                fn_LOAD_OCUL();
                                var aValoresSesion;
                                aValoresSesion = oOB_JSON_1.d.toString().split("*");
                                if (aValoresSesion[0] == "EXPIRO") {
                                    window.location.href = aValoresSesion[1];
                                }
                                if (oOB_JSON_1.d.split("*")[0] != "OK") {
                                    if (oOB_JSON_1.d.split("*")[0] == "1") {
                                        $.JMensajePOPUP("Error", oOB_JSON_1.d.split("*")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()");
                                    } else {
                                        $.JMensajePOPUP("Aviso", oOB_JSON_1.d.split("*")[1], "", "Cerrar", "fn_oculta_mensaje()");
                                    }
                                } else {
                                    /*$("#divGridLaboratorio").load("GridViewAjax/GridLaboratorio.aspx", function () {
                                    });*/
                                }
                                $("#txtAnalisisLaboratorio").val("");
                                $("#spCodigoAnalisisLaboratorioSeleccionado").html("");
                                $("#chkAnalisisLaboratorioSeleccionado").prop("checked", false);
                                $("#spAnalisisLaboratorioSeleccionado").html("");
                                $("#chkAnalisisLaboratorioFavorito").prop("checked", false);
                                $("#hfIdeFavoritoAnalisis").val("");
                                $("#spPerfil").html("");
                                $("#divGridLaboratorio").load("GridViewAjax/GridLaboratorio.aspx", function () {
                                    if ($("#chkProgramarHoraLab").prop("checked")) {
                                        $("#divGridLaboratorio").find(".JSBTABLA .HoraProg").find("input[type='checkbox']").each(function () {
                                            $(this).removeAttr("disabled");
                                            $(this).prop("checked", true);
                                        });
                                    }
                                });
                            });
                        }
                    });
                    //************
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });

        //BOTON ENVIAR SOLICITUD DE ANALISIS
        $("#imgEnviarSolicitudAnalisisLaboratorio").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    //ENVIANDO SOLICITUD DE ANALISIS
                    if ($("#divGridLaboratorio").find(".JSBTABLA tr:not(:first)").length == 0) {
                        $.JMensajePOPUP("Aviso", "Debe seleccionar al menos un analisis", "", "Cerrar", "fn_oculta_mensaje()");
                        return;
                    }
                    if ($("#divGridLaboratorio").find(".JSBTABLA tr:not(:first)").length > 0) {
                        if ($("#divGridLaboratorio").find(".JSBTABLA tr:not(:first)").find("td").length <= 1) {
                            $.JMensajePOPUP("Aviso", "Debe seleccionar al menos un analisis", "", "Cerrar", "fn_oculta_mensaje()");
                            return;
                        }
                    }

                    $.JMensajePOPUP("Aviso", "¿Desea enviar la Solicitud del Petitorio de Análisis?", "ADVERTENCIA", "Si;No", "fn_EnviarSolicitudAnalisis();fn_oculta_mensaje()");

                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });

        $("#btnVerInformeAnalisisLaboratorio").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    /*23/07/2020 - JB - COMENTADO
                    var IdRecetaCab = $("#divTreeLaboratorio").find(".JTREE2-SELECCIONADO").next().val();
                    if (IdRecetaCab != undefined && IdRecetaCab != null && IdRecetaCab != "") {
                    } else {
                    IdRecetaCab = $("#divTreeLaboratorio").find(".JTREE2-SELECCIONADO").parent().find("input").eq(0).val();
                    }
                    */


                    //INICIO - 23/07/2020 - JB - NUEVO
                    var IdRecetaCab = $("#divTreeLaboratorio2").find(".JTREE3-SELECCIONADO > input").eq(0).val();
                    if (isNaN(IdRecetaCab)) {
                        return;
                    } else {
                    }
                    //FIN - JB - NUEVO


                    if (IdRecetaCab != undefined && IdRecetaCab != null && IdRecetaCab != "") {
                         $.ajax({
                             url: "InformacionPaciente.aspx/VerInformeAnalisis",
                             type: "POST",
                             contentType: "application/json; charset=utf-8",
                             data: JSON.stringify({
                                 IdRecetaCab: IdRecetaCab
                             }),
                             dataType: "json",
                             error: function (dato1, datos2, dato3) {

                             }
                        }).done(function (oOB_JSON)
                        {
                             fn_LOAD_OCUL();
                             fn_GuardaLog("LABORATORIO", "Se visualizo informe " + IdRecetaCab);
                             if (oOB_JSON.d.toString().split(";").length > 1) {
                                 $.JMensajePOPUP("Aviso", oOB_JSON.d.toString().split(";")[1], "", "Cerrar", "fn_oculta_mensaje()");
                             } else {
                                 window.open("VisorReporte.aspx?OP=ANALISISLABORATORIO&Valor=" + IdRecetaCab.toString()); //1.2
                                 // INI 1.2
                                 //var ventana_popup = window.open(oOB_JSON.d, "_blank");
                                 //if (ventana_popup == null || typeof (ventana_popup) == undefined) {
                                 //    //ventana popup bloqueada
                                 //} else {
                                 //    //ventana_popup.focus();
                                 //}
                                 // FIN 1.2
                             }
                         });                

                        $.ajax({
                            url: "InformacionPaciente.aspx/LaboratorioCompletado",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({
                                IdReceta: IdRecetaCab
                            }),
                            dataType: "json",
                            error: function (dato1, datos2, dato3) {
                            }
                        }).done(function (oOB_JSON) {
                            if (oOB_JSON.d != "OK") {
                                $.JMensajePOPUP("Mensaje de Clinica San Felipe - Error", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_mensaje()");
                            }
                        });

                    } else {
                        $.JMensajePOPUP("Aviso", "Debe seleccionar un análisis.", "", "Cerrar", "fn_oculta_mensaje()");
                    }
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });

        });


        /*********************************************************************************************************/
        /*************************************************IMAGENES************************************************/
        /*********************************************************************************************************/
        $("#divGridImagen").load("GridViewAjax/GridImagen.aspx", function () {
        });

        //CARGANDO TREEVIEW DE IMAGENES
        //fn_CrearTreeViewImagenes();
        fn_CrearTreeViewImagenes2("1", "", "0");

        $("#imgPetitorioImagen").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    //AQUI EL CODIGO SI LA SESSION AUN NO EXPIRA
                    window.location.href = "PetitorioImagen.aspx";
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });

        //BUSQUEDA DE IMAGEN
        $("#imgBusquedaImagenes").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    if ($("#txtImagen").val().trim().length > 3) {
                        $("#divFONDO").css("display", "inline");
                        $("#divBusquedaImagen").css("display", "block");
                        $("#divBusquedaImagen").load("Utilidad/BusquedaImagen.aspx", { Nombre: $("#txtImagen").val().trim(), TipoBusqueda: "G", Orden: "3" }, function () {
                            $(this).find("#hfTipoBusqueda").val("G");
                            fn_CrearEventoImagenes();
                        });
                    } else {
                        $.JMensajePOPUP("Aviso", "Debe ingresar al menos 4 caracteres para realizar una búsqueda.", "", "Cerrar", "fn_oculta_mensaje()");
                    }
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });
        //EVENTO ENTER ACTIVARA LA BUSQUEDA DE IMAGEN
        $("#txtImagen").keypress(function (e) {
            if (e.which == 13) {
                $("#imgBusquedaImagenes").trigger("click");
            }
        });
        //BUSQUEDA DE FAVORITO DE IMAGEN
        $("#imgFavoritoImagenes").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $("#divFONDO").css("display", "inline");
                    $("#divBusquedaImagen").css("display", "block");
                    $("#divBusquedaImagen").load("Utilidad/BusquedaImagen.aspx", { Nombre: $("#txtImagen").val().trim(), TipoBusqueda: "F", Orden: "1" }, function () {
                        $(this).find("#hfTipoBusqueda").val("F");
                        fn_CrearEventoImagenes();
                    });
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });

        //ELIMINANDO DE FAVORITOS 
        $("#chkFavoritoImagen").click(function () {
            if ($(this).prop("checked") == false && ($("#hfFavoritoImagen").val().trim() != "" && $("#hfFavoritoImagen").val().trim() != "0")) {
                $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "") {
                        $.JMensajePOPUP("Aviso", "¿Desea eliminar de Favoritos?", "", "Si;No", "fn_EliminaFavoritoImagenes();fn_CancelarEliminarFavoritoImagenes()");
                    } else {
                        aValores = oOB_JSON.d.toString().split(";");
                        $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                    }
                });
            }
        });

        //AGREGAR IMAGEN
        $("#imgAgregarImagen").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    if ($("#spCodigoImagenSeleccionado").html().trim() == "") {
                        $.JMensajePOPUP("Aviso", "Seleccionar un tipo de imagen.", "", "Cerrar", "fn_oculta_mensaje()");
                        return;
                    }
                    var bFlgFavorito = false;
                    if ($("#chkFavoritoImagen").prop("checked") == true && ($("#hfFavoritoImagen").val().trim() == "" || $("#hfFavoritoImagen").val().trim() == "0")) {
                        bFlgFavorito = true;
                    }

                    fn_LOAD_VISI();
                    $.ajax({
                        url: "InformacionPaciente.aspx/ValidarImagenAgregado",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            IdImagen: $("#spCodigoImagenSeleccionado").html().trim()
                        }),
                        dataType: "json",
                        error: function (dato1, datos2, dato3) {

                        }
                    }).done(function (oOB_JSON_1) {
                        fn_LOAD_OCUL();
                        if (oOB_JSON_1.d == "SI") {
                            $.JMensajePOPUP("Aviso", "La imagen ya se encuentra seleccionada.", "", "Cerrar", "fn_oculta_mensaje()");
                        } else {
                            $.ajax({
                                url: "InformacionPaciente.aspx/AgregarImagen",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                data: JSON.stringify({
                                    DescripcionRecetaImagen: $("#txtObservacionImagen").val().trim(),
                                    IdImagen: $("#spCodigoImagenSeleccionado").html().trim(),
                                    FlgFavorito: bFlgFavorito
                                }),
                                dataType: "json",
                                error: function (dato1, datos2, dato3) {

                                }
                            }).done(function (oOB_JSON) {
                                fn_LOAD_OCUL();
                                if (oOB_JSON.d != "OK") {
                                    $.JMensajePOPUP("Aviso", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_mensaje()");
                                } else {
                                    $("#divGridImagen").load("GridViewAjax/GridImagen.aspx", function () {
                                    });
                                    $("#spCodigoImagenSeleccionado").html("");
                                    $("#chkImagenSeleccionado").prop("checked", false);
                                    $("#spImagenSeleccionado").html("");
                                    $("#chkFavoritoImagen").prop("checked", false);
                                    $("#hfFavoritoImagen").val("");
                                }
                            });
                        }
                    });
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });

        $("#imgEnviarSolicitudImagen").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    //AQUI EL CODIGO SI LA SESSION AUN NO EXPIRA
                    if ($("#divGridImagen").find(".JSBTABLA tr:not(:first)").length == 0) {
                        $.JMensajePOPUP("Aviso", "Seleccionar un tipo de imagen.", "", "Cerrar", "fn_oculta_mensaje()");
                        return;
                    }
                    if ($("#divGridImagen").find(".JSBTABLA tr:not(:first)").length > 0) {
                        if ($("#divGridImagen").find(".JSBTABLA tr:not(:first)").find("td").length <= 1) {
                            $.JMensajePOPUP("Aviso", "Seleccionar un tipo de imagen.", "", "Cerrar", "fn_oculta_mensaje()");
                            return;
                        }
                    }
                    $.JMensajePOPUP("Aviso", "¿Desea enviar la Solicitud del Petitorio de Imágenes?", "ADVERTENCIA", "Si;No", "fn_EnviarSolicitudImagen();fn_oculta_mensaje()");
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });

        $("#btnVerInformeImagen").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    var FecReceta = $("#divTreeViewImagenes2").find(".JTREE3-SELECCIONADO > .FecRegistro").eq(0).val(); //si obtiene valor marco el nodo de fecha
                    if (FecReceta == undefined || FecReceta == null || FecReceta == "") {
                        var IdeImagenDet = $("#divTreeViewImagenes2").find(".JTREE3-SELECCIONADO > .IdeImagenDet").eq(0).val(); //si tiene valor marco detalle                        
                        if (IdeImagenDet == null || IdeImagenDet == undefined || IdeImagenDet == null) {
                            $.JMensajePOPUP("Aviso", "Debe seleccionar una imagen.", "", "Cerrar", "fn_oculta_mensaje()");
                        } else {
                            var PresotorSps = $("#divTreeViewImagenes2").find(".JTREE3-SELECCIONADO").find("> input").eq(0).val(); //
                            var FlgVerificar = $("#divTreeViewImagenes2").find(".JTREE3-SELECCIONADO").find("> .FlgVerificarIma").eq(0).val();

                            if (PresotorSps.trim() != "" && PresotorSps.trim() != "_") {
                                $.ajax({
                                    url: "InformacionPaciente.aspx/VerInformeImagen",
                                    type: "POST",
                                    contentType: "application/json; charset=utf-8",
                                    data: JSON.stringify({
                                        PresotorSps: PresotorSps
                                    }),
                                    dataType: "json",
                                    error: function (dato1, datos2, dato3) {
                                    }
                                }).done(function (oOB_JSON) {
                                    if (oOB_JSON.d.toString().split(";").length > 1) {
                                        $.JMensajePOPUP("Aviso", oOB_JSON.d.toString().split(";")[1], "", "Cerrar", "fn_oculta_mensaje()");
                                    } else {
                                        fn_GuardaLog("IMAGEN", "Se visualizo informe " + PresotorSps);
                                        var ventana_popup_imagen = window.open(oOB_JSON.d, "_blank");
                                        if (ventana_popup_imagen == null || typeof (ventana_popup_imagen) == undefined) {
                                            //ventana popup bloqueada
                                        } else {
                                            //ventana_popup.focus();
                                        }
                                    }
                                });
                                if (FlgVerificar == "S" && PresotorSps != "") { //14/10/2016
                                    $.ajax({
                                        url: "InformacionPaciente.aspx/ExamenCompletado",
                                        type: "POST",
                                        contentType: "application/json; charset=utf-8",
                                        data: JSON.stringify({
                                            IdReceta: IdeImagenDet
                                        }),
                                        dataType: "json",
                                        error: function (dato1, datos2, dato3) {
                                        }
                                    }).done(function (oOB_JSON) {

                                    });
                                }
                            }
                        }
                    } else {
                        $.JMensajePOPUP("Aviso", "Debe seleccionar una imagen.", "", "Cerrar", "fn_oculta_mensaje()");
                    }


                    /*var FecReceta = $("#divTreeViewImagenes").find(".JTREE2-SELECCIONADO").prev().val(); //si obtiene valor marco el nodo de fecha               
                    if (FecReceta == undefined || FecReceta == null || FecReceta == "") {
                    var PresotorSps = $("#divTreeViewImagenes").find(".JTREE2-SELECCIONADO").next().val(); //si tiene valor marco la cabecera
                    if (PresotorSps == "_" || PresotorSps == "") {
                    PresotorSps = $("#divTreeViewImagenes").find(".JTREE2-SELECCIONADO").parent().find("> input").eq(0).val(); //
                    var FlgVerificar = $("#divTreeViewImagenes").find(".JTREE2-SELECCIONADO").parent().find("> .FlgVerificarIma").eq(0).val();
                    var IdeImagenDet = $("#divTreeViewImagenes").find(".JTREE2-SELECCIONADO").parent().find("> .IdeImagenDet").eq(0).val();


                    //return; //DESCOMENTAR LUEGO

                    if (PresotorSps.trim() != "" && PresotorSps.trim() != "_") {
                    $.ajax({
                    url: "InformacionPaciente.aspx/VerInformeImagen",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                    PresotorSps: PresotorSps
                    }),
                    dataType: "json",
                    error: function (dato1, datos2, dato3) {
                    }
                    }).done(function (oOB_JSON) {
                    if (oOB_JSON.d.toString().split(";").length > 1) {
                    $.JMensajePOPUP("Aviso", oOB_JSON.d.toString().split(";")[1], "", "Cerrar", "fn_oculta_mensaje()");
                    } else {
                    fn_GuardaLog("IMAGEN", "Se visualizo informe " + PresotorSps);
                    var ventana_popup_imagen = window.open(oOB_JSON.d, "_blank");
                    if (ventana_popup_imagen == null || typeof (ventana_popup_imagen) == undefined) {
                    //ventana popup bloqueada
                    } else {
                    //ventana_popup.focus();
                    }
                    }
                    });
                    if (FlgVerificar == "S" && PresotorSps != "") { //14/10/2016
                    $.ajax({
                    url: "InformacionPaciente.aspx/ExamenCompletado",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                    IdReceta: IdeImagenDet
                    }),
                    dataType: "json",
                    error: function (dato1, datos2, dato3) {
                    }
                    }).done(function (oOB_JSON) {

                    });
                    }
                    }
                    } else {
                    //seleccione una imagen
                    $.JMensajePOPUP("Aviso", "Debe seleccionar una imagen.", "", "Cerrar", "fn_oculta_mensaje()");
                    }
                    } else {
                    //seleccione una imagen
                    $.JMensajePOPUP("Aviso", "Debe seleccionar una imagen.", "", "Cerrar", "fn_oculta_mensaje()");
                    }*/


                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });

        $("#btnVerImagen").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {


                    var FecReceta = $("#divTreeViewImagenes2").find(".JTREE3-SELECCIONADO > .FecRegistro").eq(0).val(); //si obtiene valor marco el nodo de fecha
                    if (FecReceta == undefined || FecReceta == null || FecReceta == "") {
                        var IdeImagenDet = $("#divTreeViewImagenes2").find(".JTREE3-SELECCIONADO > .IdeImagenDet").eq(0).val(); //si tiene valor marco detalle                        
                        if (IdeImagenDet == null || IdeImagenDet == undefined || IdeImagenDet == null) {
                            $.JMensajePOPUP("Aviso", "Debe seleccionar una imagen.", "", "Cerrar", "fn_oculta_mensaje()");
                        } else {
                            var PresotorSps = $("#divTreeViewImagenes2").find(".JTREE3-SELECCIONADO").find("> input").eq(0).val(); //
                            var FlgVerificar = $("#divTreeViewImagenes2").find(".JTREE3-SELECCIONADO").find("> .FlgVerificarIma").eq(0).val();

                            if (PresotorSps.trim() != "" && PresotorSps.trim() != "_") {
                                $.ajax({
                                    url: "InformacionPaciente.aspx/VerImagen",
                                    type: "POST",
                                    contentType: "application/json; charset=utf-8",
                                    data: JSON.stringify({
                                        PresotorSps: PresotorSps
                                    }),
                                    dataType: "json",
                                    error: function (dato1, datos2, dato3) {
                                    }
                                }).done(function (oOB_JSON) {
                                    if (oOB_JSON.d.toString().split(";").length > 1) {
                                        $.JMensajePOPUP("Aviso", oOB_JSON.d.toString().split(";")[1], "", "Cerrar", "fn_oculta_mensaje()");
                                    } else {
                                        fn_GuardaLog("IMAGEN", "Se visualizo imagen de " + PresotorSps);
                                        //abrira en internet explorer
                                        var ventana_popup_imagen = window.open(oOB_JSON.d, "_blank");
                                        if (ventana_popup_imagen == null || typeof (ventana_popup_imagen) == undefined) {
                                            //ventana popup bloqueada
                                        } else {
                                            ventana_popup_imagen.focus();
                                        }
                                    }
                                });
                            }
                        }
                    } else {
                        $.JMensajePOPUP("Aviso", "Debe seleccionar una imagen.", "", "Cerrar", "fn_oculta_mensaje()");
                    }

                    /*
                    var FecReceta = $("#divTreeViewImagenes").find(".JTREE2-SELECCIONADO").prev().val();
                    if (FecReceta == undefined || FecReceta == null || FecReceta == "") {
                    var PresotorSps = $("#divTreeViewImagenes").find(".JTREE2-SELECCIONADO").next().val(); //si tiene valor marco la cabecera
                    if (PresotorSps == "_" || PresotorSps == "") {
                    PresotorSps = $("#divTreeViewImagenes").find(".JTREE2-SELECCIONADO").parent().find("> input").eq(0).val(); //
                    var FlgVerificar = $("#divTreeViewImagenes").find(".JTREE2-SELECCIONADO").parent().find("> .FlgVerificarIma").eq(0).val();
                    var IdeImagenDet = $("#divTreeViewImagenes").find(".JTREE2-SELECCIONADO").parent().find("> .IdeImagenDet").eq(0).val();

                    //return; //DESCOMENTAR LUEGO
                    if (PresotorSps.trim() != "" && PresotorSps.trim() != "_") {
                    $.ajax({
                    url: "InformacionPaciente.aspx/VerImagen",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                    PresotorSps: PresotorSps
                    }),
                    dataType: "json",
                    error: function (dato1, datos2, dato3) {
                    }
                    }).done(function (oOB_JSON) {
                    if (oOB_JSON.d.toString().split(";").length > 1) {
                    $.JMensajePOPUP("Aviso", oOB_JSON.d.toString().split(";")[1], "", "Cerrar", "fn_oculta_mensaje()");
                    } else {
                    fn_GuardaLog("IMAGEN", "Se visualizo imagen de " + PresotorSps);
                    //abrira en internet explorer
                    var ventana_popup_imagen = window.open(oOB_JSON.d, "_blank");
                    if (ventana_popup_imagen == null || typeof (ventana_popup_imagen) == undefined) {
                    //ventana popup bloqueada
                    } else {
                    ventana_popup_imagen.focus();
                    }
                    }
                    });
                    } else {
                    //no ha seleccionado ninguna imagen para ver el informe   
                    $.JMensajePOPUP("Aviso", "Debe seleccionar una imagen.", "", "Cerrar", "fn_oculta_mensaje()");
                    }

                    }
                    }*/



                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });

        });

        $("#btnCancelarOrden").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {


                    var FecReceta = $("#divTreeViewImagenes2").find(".JTREE3-SELECCIONADO > .FecRegistro").eq(0).val(); //si obtiene valor marco el nodo de fecha
                    if (FecReceta == undefined || FecReceta == null || FecReceta == "") {
                        var IdeImagenDet = $("#divTreeViewImagenes2").find(".JTREE3-SELECCIONADO > .IdeImagenDet").eq(0).val(); //si tiene valor marco detalle                        
                        if (IdeImagenDet == null || IdeImagenDet == undefined || IdeImagenDet == null) {
                            $.JMensajePOPUP("Aviso", "Debe seleccionar una imagen.", "", "Cerrar", "fn_oculta_mensaje()");
                        } else {
                            var PresotorSps = $("#divTreeViewImagenes2").find(".JTREE3-SELECCIONADO").find("> input").eq(0).val();

                            $.ajax({
                                url: "InformacionPaciente.aspx/ValidaCancelarOrden",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                data: JSON.stringify({
                                    PresotorSps: PresotorSps,
                                    IdeImagenDet: IdeImagenDet
                                }),
                                dataType: "json",
                                error: function (dato1, datos2, dato3) {
                                }
                            }).done(function (oOB_JSON) {
                                if (oOB_JSON.d.toString() != "") {
                                    $.JMensajePOPUP("Aviso", oOB_JSON.d.toString(), "", "Cerrar", "fn_oculta_mensaje()");
                                } else {
                                    //ejecutar funcion para eliminar
                                    $.JMensajePOPUP("Aviso", "¿Desea anular la orden de imagen?", "", "Si;Cerrar", "fn_CancelarOrden();fn_oculta_mensaje()");
                                }
                            });
                        }
                    } else {
                        $.JMensajePOPUP("Aviso", "Debe seleccionar una imagen.", "", "Cerrar", "fn_oculta_mensaje()");
                    }


                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });

        //INICIO - JB - 07/01/2020 - SE DESHABILITA LA SECCION DE IMAGENES, SOLO FUNCIONARA EL PETITORIO.
        $("#imgBusquedaImagenes").prop("disabled", "disabled");
        $("#imgFavoritoImagenes").prop("disabled", "disabled");
        $("#txtImagen").prop("disabled", "disabled");
        $("#chkFavoritoImagen").prop("disabled", "disabled");
        $("#imgAgregarImagen").prop("disabled", "disabled");
        $("#txtObservacionImagen").prop("disabled", "disabled");
        $("#imgEnviarSolicitudImagen").prop("disabled", "disabled");

        $("#imgBusquedaImagenes").unbind("click");
        $("#imgFavoritoImagenes").unbind("click");
        $("#imgAgregarImagen").unbind("click");
        $("#imgEnviarSolicitudImagen").unbind("click");
        //FIN - JB - 07/01/2020 - SE DESHABILITA LA SECCION DE IMAGENES, SOLO FUNCIONARA EL PETITORIO.


        /*********************************************************************************************************/
        /******************************************NOTA DE INGRESO************************************************/
        /*********************************************************************************************************/
        $("#btnGuardarNotaIngresoMT").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    if ($("#txtNotaIngresoMedicoTratante").val().trim() != "") {
                        fn_LOAD_VISI();

                        $.ajax({
                            url: "InformacionPaciente.aspx/GuardarNotaIngreso",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({
                                IdControl: "dsc_medico_tratante",
                                ValorControl: $("#txtNotaIngresoMedicoTratante").val()
                            }),
                            dataType: "json",
                            error: function (dato1, datos2, dato3) {

                            }
                        }).done(function (oOB_JSON) {
                            if (oOB_JSON.d != "") {
                                //INICIO - 16/01/2017 - JB
                                if (oOB_JSON.d.split(";")[0] == "V") {
                                    $.JMensajePOPUP("Mensaje del Sistema Clinica San Felipe - Error", oOB_JSON.d.split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()");
                                    fn_LOAD_OCUL();
                                    return;
                                }
                                //FIN - 16/01/2017 - JB
                                $("#divNotaIngreso").html("");
                                $("#divNotaIngreso").append(oOB_JSON.d);
                                $("#txtNotaIngreso").val("");
                                /*$("#btnGuardarNotaIngreso").attr("disabled", "disabled");
                                $("#txtNotaIngreso").val("");
                                $("#txtNotaIngreso").attr("disabled", "disabled");
                                COMENTADO 06/19/2016
                                */
                                $("#txtNotaIngresoMedicoTratante").val("");
                            } else {
                                $.JMensajePOPUP("Aviso", "No se pudo Guardar la Nota de Ingreso", "ERROR", "Cerrar", "fn_oculta_mensaje()");
                                $("#btnGuardarNotaIngresoMT").removeAttr("disabled");
                                $("#txtNotaIngresoMedicoTratante").removeAttr("disabled");
                            }
                            fn_LOAD_OCUL();
                        });
                    }
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });
        $("#btnGuardarNotaIngresoI").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    if ($("#txtNotaIngresoIntensivista").val().trim() != "") {
                        fn_LOAD_VISI();

                        $.ajax({
                            url: "InformacionPaciente.aspx/GuardarNotaIngreso",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({
                                IdControl: "dsc_intensivista",
                                ValorControl: $("#txtNotaIngresoIntensivista").val()
                            }),
                            dataType: "json",
                            error: function (dato1, datos2, dato3) {

                            }
                        }).done(function (oOB_JSON) {
                            if (oOB_JSON.d != "") {
                                //INICIO - 16/01/2017 - JB
                                if (oOB_JSON.d.split(";")[0] == "V") {
                                    $.JMensajePOPUP("Mensaje del Sistema Clinica San Felipe - Error", oOB_JSON.d.split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()");
                                    fn_LOAD_OCUL();
                                    return;
                                }
                                //FIN - 16/01/2017 - JB
                                $("#divNotaIngreso").html("");
                                $("#divNotaIngreso").append(oOB_JSON.d);
                                $("#txtNotaIngreso").val("");
                                /*$("#btnGuardarNotaIngreso").attr("disabled", "disabled");
                                $("#txtNotaIngreso").val("");
                                $("#txtNotaIngreso").attr("disabled", "disabled");
                                COMENTADO 06/19/2016
                                */
                                $("#txtNotaIngresoIntensivista").val("");
                            } else {
                                $.JMensajePOPUP("Aviso", "No se pudo Guardar la Nota de Ingreso", "ERROR", "Cerrar", "fn_oculta_mensaje()");
                                $("#btnGuardarNotaIngresoI").removeAttr("disabled");
                                $("#txtNotaIngresoIntensivista").removeAttr("disabled");
                            }
                            fn_LOAD_OCUL();
                        });
                    }
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });

        /*********************************************************************************************************/
        /******************************************JUNTA MEDICA***************************************************/
        /*********************************************************************************************************/
        fn_CrearTreeViewJuntaMedica("1", "0");

        $("#btnGuardarJuntaMedica").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    if ($("#txtJuntaMedica").val().trim() != "") {
                        fn_LOAD_VISI();

                        $.ajax({
                            url: "InformacionPaciente.aspx/GuardarJuntaMedica",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({
                                DscJuntaMedica: $("#txtJuntaMedica").val()
                            }),
                            dataType: "json",
                            error: function (dato1, datos2, dato3) {

                            }
                        }).done(function (oOB_JSON) {
                            if (oOB_JSON.d != "") {
                                $("#txtJuntaMedica").val("");
                                fn_CrearTreeViewJuntaMedica("1", "0");
                                fn_CargarEvolucionClinica2("1", "0");
                            } else {
                                $.JMensajePOPUP("Aviso", "No se pudo Guardar la Nota de Ingreso", "ERROR", "Cerrar", "fn_oculta_mensaje()");
                            }
                            fn_LOAD_OCUL();
                        });
                    }
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });




        /*********************************************************************************************************/
        /******************************************EVOLUCION CLINICA**********************************************/
        /*********************************************************************************************************/

        $("#btnGuardarEvolucionClinica").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    /*chkEducacionEvolucionClinica
                    chkInformeEvolucionClinica*/
                    var TipoEducacionInforme = "";
                    var TipoEvolucion = "";
                    var VacioTexto = "";
                    var VacioEducacionInforme = "";
                    var VacioEvolucion = "";
                    if ($("#rbEducacionEvolucionClinica").prop("checked")) {
                        TipoEducacionInforme = "E";
                    }
                    if ($("#rbInformeEvolucionClinica").prop("checked")) {
                        TipoEducacionInforme = "I";
                    }

                    if ($("#rbInestable").prop("checked")) {
                        TipoEvolucion = $("#rbInestable").val();
                    }
                    if ($("#rbDeterioro").prop("checked")) {
                        TipoEvolucion = $("#rbDeterioro").val();
                    }
                    if ($("#rbEstacionaria").prop("checked")) {
                        TipoEvolucion = $("#rbEstacionaria").val();
                    }
                    if ($("#rbEstableMejoria").prop("checked")) {
                        TipoEvolucion = $("#rbEstableMejoria").val();
                    }

                    if ($("#txtSubjetiva").val().trim() == "" && $("#txtObjetiva").val().trim() == "" && $("#txtEvolucionClinica").val().trim() == "" && $("#txtPlan").val().trim() == "") {
                        //INICIO - JB - 25/05/2020 - comentado
                        //$.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe", "No ha registrado información.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()");
                        //return;
                        //FIN - JB - 25/05/2020 - comentado
                        VacioTexto = "SI";
                    }

                    if (TipoEducacionInforme == "") {
                        /*JB - COMENTADO - 02/06/2020
                        $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe", "No ha registrado información.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()");
                        return;*/
                        VacioEducacionInforme = "SI";
                    }
                    if (TipoEvolucion == "") {
                        /*JB - COMENTADO - 03/06/2020
                        $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe", "Debe marcar un tipo de evolución.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()");
                        return;*/
                        VacioEvolucion = "SI";
                    }

                    var sRequiereFirma = "";

                    if ($("#chkRequiereFirma").prop("checked")) {
                        sRequiereFirma = "1";
                    }

                    if (VacioTexto == "SI" && VacioEducacionInforme == "SI" && VacioEvolucion == "SI" && sRequiereFirma == "") {
                        $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe", "No ha ingresado ninguna información.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()");
                        return;
                    }

                    fn_LOAD_VISI();
                    $.ajax({
                        url: "InformacionPaciente.aspx/GuardarEvolucionClinica",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            Analitica: $("#txtEvolucionClinica").val().trim(), //ANALITICA
                            TipoEducacionInforme: TipoEducacionInforme,
                            Subjetiva: $("#txtSubjetiva").val().trim(),
                            Objetiva: $("#txtObjetiva").val().trim(),
                            Plan: $("#txtPlan").val().trim(),
                            RequiereFirma: sRequiereFirma,
                            TipoEvolucion: TipoEvolucion
                        }),
                        dataType: "json",
                        error: function (dato1, datos2, dato3) {

                        }
                    }).done(function (oOB_JSON) {
                        fn_LOAD_OCUL();
                        if (oOB_JSON.d.split(";")[0] == "ERROR") {
                            $.JMensajePOPUP("Error", oOB_JSON.d.split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()");
                        } else {
                            if (oOB_JSON.d != "") {
                                //INICIO - 16/01/2017 - JB
                                if (oOB_JSON.d.split(";")[0] == "V") {
                                    $.JMensajePOPUP("Mensaje del Sistema Clinica San Felipe - Error", oOB_JSON.d.split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()");
                                    fn_LOAD_OCUL();
                                    return;
                                }
                                //FIN - 16/01/2017 - JB
                                $("#divEvolucionClinica").html("");
                                $("#divEvolucionClinica").append(oOB_JSON.d);
                                $("#txtEvolucionClinica").val("");
                                $("#txtSubjetiva").val("");
                                $("#txtObjetiva").val("");
                                $("#txtPlan").val("");
                                $("#rbEducacionEvolucionClinica").prop("checked", false);
                                $("#rbInformeEvolucionClinica").prop("checked", false);
                                $("#rbInestable").prop("checked", false);
                                $("#rbDeterioro").prop("checked", false);
                                $("#rbEstacionaria").prop("checked", false);
                                $("#rbEstableMejoria").prop("checked", false);


                                $("#chkRequiereFirma").prop("checked", false);
                                $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe", "Se guardaron los datos correctamente.", "OK", "Cerrar", "fn_oculta_mensaje()");
                                //fn_CargarEvolucionClinica();
                                fn_CargarEvolucionClinica2("1", "0");
                            } else {

                            }
                        }
                    });
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });


        /*********************************************************************************************************/
        /********************************CONTROL CLINICO e INDICACIONES MEDICAS***********************************/
        /*********************************************************************************************************/
        $("#btnCalcularCalculadora").click(function () {
            if ($("#txtPesoCalculadora").val().trim() == "" || $("#" + "<%=ddlFarmacoCalculadora.ClientID %>").val().trim() == "" || $("#txtVelocidadInfusionCalculadora").val().trim() == "") {
                $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe", "Debe ingresar todos los campos", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()");
                return;
            }
            //alert($("#" + "<%=ddlFarmacoCalculadora.ClientID %>" + " option:selected").text());
            var ModificarAgregar = "AGREGAR";
            $("#divContenedorCalculadora").find(".JFILA-ESTILO-ARRIBACERO").each(function () {
                if ($(this).find(".JCELDA-2").eq(0).find(".JETIQUETA_2").text() == $("#" + "<%=ddlFarmacoCalculadora.ClientID %>" + " option:selected").text()) {
                    //&& $(this).find(".JCOL-OCULTA").eq(0).find(".JETIQUETA_2").text() == $("#txtPesoCalculadora").val().trim() - CAMBIO EN VALIDACION
                    ModificarAgregar = "MODIFICAR";
                }
            });


            //SI LLENO TODOS LOS DATOS
            $.ajax({
                url: "InformacionPaciente.aspx/CalcularDosis",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    PesoCalculadora: $("#txtPesoCalculadora").val().trim(),
                    FarmacoCalculadora: $("#" + "<%=ddlFarmacoCalculadora.ClientID %>").val().trim(),
                    VelocidadInfusionCalculadora: $("#txtVelocidadInfusionCalculadora").val().trim(),
                    Tipo: ModificarAgregar
                }),
                dataType: "json",
                error: function (dato1, datos2, dato3) {
                }
            }).done(function (oOB_JSON) {
                if (ModificarAgregar == "MODIFICAR") {
                    $("#divContenedorCalculadora").find(".JFILA-ESTILO-ARRIBACERO").each(function () {
                        var oObjeto = $(this);
                        if ($(this).find(".JCELDA-2").eq(0).find(".JETIQUETA_2").text() == $("#" + "<%=ddlFarmacoCalculadora.ClientID %>" + " option:selected").text()) {
                            //&& $(this).find(".JCOL-OCULTA").eq(0).find(".JETIQUETA_2").text() == $("#txtPesoCalculadora").val().trim() 24/09/2021 - CAMBIO EN VALIDACION
                            oObjeto.replaceWith(oOB_JSON.d);
                            fn_EnumerarCalculosCalculadora();
                        }
                    });
                } else {
                    $("#divContenedorCalculadora").append(oOB_JSON.d);
                    fn_EnumerarCalculosCalculadora();
                }

            });




        });



        var IdVia_ = "<%=ddlVia_Con.ClientID %>";  //"<= ddlVia_Con.ClientID %>";  JB - 14/07/2020 - COMENTADO  //JB - 27/04/2021 - txtVia_ControlClinico
        fn_DeshabilitaControles(IdVia_ + ";txtProducto_Con;txtDosis_Con;txtCadaHora_Con;txtIndicacionProductoMedicamento;imgBusquedaProducto;imgAgregar_Con;chkBuscarDci;txtCantidad_Con;chkPRNControlClinico");
        $("#btnCopiarCC").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $.ajax({
                        url: "InformacionPaciente.aspx/VerificarAlertas", //verifica si hay alertar lab o img
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        error: function (dato1, datos2, dato3) {
                        }
                    }).done(function (oOB_JSON) {
                        aValoresAlerta = oOB_JSON.d.toString().split(";");
                        if (aValoresAlerta[0] == "ALERTA") {
                            //if (aValoresAlerta[1] == "M") { //solo mostrara a los medicos esta alerta
                            $.ajax({
                                url: "InformacionPaciente.aspx/VerificarAlertas3", //obtiene cadena de mensajes
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                error: function (dato1, datos2, dato3) {
                                }
                            }).done(function (oOB_JSON) {
                                if (oOB_JSON.d != "") {
                                    $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe", oOB_JSON.d, "Aceptar", "Cerrar", "fn_AceptaMensajeCopiarCC()");
                                }
                            });
                            //}
                        } else {
                            fn_AceptaMensajeCopiarCC();
                        }
                    });

                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });


        $("#btnSuspenderCC").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    fn_LOAD_OCUL();
                    //var IdReceta = $("#divControlClinicoIndicacionMed").find(".JTREE2-SELECCIONADO").next().val(); SE REEMPLAZA POR LA LINEA DEBAJO
                    var IdReceta = $("#divControlClinicoIndicacionMed2").find(".JTREE3-SELECCIONADO > .IdeRecetaCab").val();
                    if (IdReceta != "" && IdReceta != null && IdReceta != undefined) {
                        fn_GuardaLog("CONTROL CLINICO", "Suspendio la receta nro " + IdReceta);
                        var aVA_DEMO = ["#divDatosUsuarioSuspensionMedicamento", "Utilidad/DatosUsuarioPopUp.aspx", ""];
                        var aValores = [IdReceta];
                        $.JPopUp("Suspensión de Medicamentos - Control Clínico", "PopUp/SuspensionMedicamento.aspx", "2", "Suspender;Salir", "fn_SuspenderSuspension();fn_CancelarSuspension()", 75, aVA_DEMO, aValores); //fn_NuevoReconciliacionMedicamentosa
                    }
                    /*$("#divControlClinicoIndicacionMed > ul > li > a").each(function () {
                    if ($(this).css("color") == "rgb(141, 199, 63)") { //8DC73F
                    var IdRecetaDet = $(this).next().val();
                    fn_GuardaLog("CONTROL CLINICO", "Suspendio la receta nro " + IdRecetaDet);
                    //$.JPopUp("Suspensión de Medicamentos - Control Clínico", "PopUp/SuspensionMedicamento.aspx");
                    var aVA_DEMO = ["#divDatosUsuarioSuspensionMedicamento", "Utilidad/DatosUsuarioPopUp.aspx", ""];
                    var aValores = [IdRecetaDet];
                    $.JPopUp("Suspensión de Medicamentos - Control Clínico", "PopUp/SuspensionMedicamento.aspx", "2", "Suspender;Salir", "fn_SuspenderSuspension();fn_oculta_popup()", 75, aVA_DEMO, aValores); //fn_NuevoReconciliacionMedicamentosa
                    }
                    });*/
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });

        $("#btnVerificarCC").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $("#divControlClinicoIndicacionMed > ul > li > a").each(function () {
                        if ($(this).css("color") == "rgb(141, 199, 63)") { //8DC73F
                            var IdRecetaDet = $(this).next().val();
                            fn_GuardaLog("CONTROL CLINICO", "Se verifico suspensión " + IdRecetaDet);
                            //$.JPopUp("Suspensión de Medicamentos - Control Clínico", "PopUp/SuspensionMedicamento.aspx");
                            var aVA_DEMO = ["#divDatosUsuarioVerificarMedicamento", "Utilidad/DatosUsuarioPopUp.aspx", ""];
                            var aValores = [IdRecetaDet];
                            $.JPopUp("Verificación de Suspensión de Medicamentos - Control Clínico", "PopUp/VerificarMedicamento.aspx", "2", "Verificar;Salir", "fn_VerificarSuspension();fn_oculta_popup()", 75, aVA_DEMO, aValores); //fn_NuevoReconciliacionMedicamentosa
                        }
                    });
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });


        $("#imgBusquedaProducto").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    if ($("#txtProducto_Con").val().trim().length > 3) {
                        var NroORden = "";
                        if ($("#chkBuscarDci").prop("checked")) {
                            NroORden = "-1"
                        } else {
                            NroORden = "-2"
                        }

                        $("#divFONDO").css("display", "inline");
                        $("#divBusquedaMedicamentoCC").css("display", "block");
                        $("#divBusquedaMedicamentoCC").html("");
                        $("#divBusquedaMedicamentoCC").load("Utilidad/BusquedaMedicamento.aspx", { Nombre: $("#txtProducto_Con").val().trim(), Orden: NroORden }, function () {
                            fn_CrearEventoControlClinico();
                        });
                    } else {
                        $.JMensajePOPUP("Aviso", "Debe ingresar al menos 4 caracteres para realizar una búsqueda.", "", "Cerrar", "fn_oculta_mensaje()");
                    }
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });

        //EVENTO ENTER ACTIVARA LA BUSQUEDA DE IMAGEN
        $("#txtProducto_Con").keypress(function (e) {
            if (e.which == 13) {
                $("#imgBusquedaProducto").trigger("click");
            }
        });

        $("#txtProducto_Con").keyup(function () {
            $("#spCodigoProductoSeleccionado").html("");
        });

        //INICIO - JB - 27/04/2021 - nuevo codigo
        $("#" + "<%=ddlVia_Con.ClientID %>").change(function () {
            var CodigoVia = "";
            CodigoVia = $(this).val().trim(); //JB - 27/04/2021 - nuevo codigo
            if (CodigoVia == "01" || CodigoVia == "02" || CodigoVia == "06" || CodigoVia == "07" || CodigoVia == "08" || CodigoVia == "14") {
                $("#txtCantidad_Con").val("1");
                $("#txtCantidad_Con").attr("disabled", "disabled"); //JB - 05/05/2021
                $.JValidaCampoObligatorio("txtCantidad_Con");
            } else {
                $("#txtCantidad_Con").removeAttr("disabled"); //JB - 05/05/2021
                if ($("#chkPRNControlClinico").prop("checked")) {

                } else {
                    $("#txtCadaHora_Con").trigger("blur");
                }

            }
        });

        $("#txtCantidad_Con").blur(function () {
            if ($(this).val().trim() == "" || $(this).val().trim() == "0") {
                $.JMensajePOPUP("Error", "Cantidad debe ser mayor a 0", "ERROR", "Cerrar", "fn_oculta_mensaje()");
            }
        });
        $("#chkPRNControlClinico").change(function () {
            if ($(this).prop("checked")) {
                $("#txtCantidad_Con").val("1");
                $("#txtCadaHora_Con").val("");
                $("#txtCadaHora_Con").attr("disabled", "disabled");
            } else {
                $("#txtCadaHora_Con").val("");
                $("#txtCadaHora_Con").removeAttr("disabled");
            }
        });
        //FIN - JB - 27/04/2021 - nuevo codigo


        $("#divGridProductoMedicamento").load("GridViewAjax/GridProductoMedicamento.aspx");

        //BOTON AGREGAR CONTROL CLINICO
        $("#imgAgregar_Con").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    var bValidacion;
                    var IdVia_ = "<%=ddlVia_Con.ClientID %>"; //jb - 26/04/2021 - nuevo codigo
                    if ($("#chkPRNControlClinico").prop("checked")) { //12/05/2021
                        bValidacion = $.JValidaCampoObligatorio("txtCantidad_Con;txtProducto_Con;txtDosis_Con;" + IdVia_); //txtIndicacionProductoMedicamento
                    } else {
                        bValidacion = $.JValidaCampoObligatorio("txtCantidad_Con;txtProducto_Con;txtDosis_Con;" + IdVia_ + ";txtCadaHora_Con");
                    }


                    if (bValidacion == false) {
                        return false;
                    }
                    var ValorVia = "";
                    ValorVia = $("#" + "<%=ddlVia_Con.ClientID %>" + " option:selected").text().trim() //$("#" + "<= ddlVia_Con.ClientID %>" + " option:selected").text().trim(); //JB - 27/04/2021 - $("#txtVia_ControlClinico").val().trim()
                    var CodigoVia = ""; //JB - 27/04/2021 - nuevo codigo
                    CodigoVia = $("#" + "<%=ddlVia_Con.ClientID %>").val().trim(); //JB - 27/04/2021 - nuevo codigo

                    $.ajax({
                        url: "InformacionPaciente.aspx/ValidarProductoAgregado2",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            CodigoProducto: $("#spCodigoProductoSeleccionado").html().trim()
                        }),
                        dataType: "json",
                        error: function (dato1, datos2, dato3) {

                        }
                    }).done(function (oOB_JSON) {
                        if (oOB_JSON.d == "") {
                            var TipoPedido = "";
                            if ($("#spCodigoProductoSeleccionado").html().trim() != "") {
                                TipoPedido = "02"; //INICIO DE TRATAMIENTO
                            }
                            if ($("#txtCantidad_Con").val().trim() == "" || $("#txtCantidad_Con").val().trim() == "Infinity") { //JB - 19/04/2021 - si cantidad es vacia no se hara pedido
                                TipoPedido = "";
                                if ($("#txtCantidad_Con").val().trim() == "Infinity") {
                                    $("#txtCantidad_Con").val("");
                                }
                            }
                            //INICIO - JB - 27/04/2021 - nuevo codigo
                            if (CodigoVia == "01" || CodigoVia == "02" || CodigoVia == "06" || CodigoVia == "07" || CodigoVia == "08" || CodigoVia == "14") {
                                TipoPedido = "";
                            }
                            if ($("#chkPRNControlClinico").prop("checked")) {
                                TipoPedido = "";
                            }
                            var Prn = "NO";
                            //alert($("#chkPRNControlClinico").prop("checked"));
                            if ($("#chkPRNControlClinico").prop("checked")) {
                                Prn = "SI";
                            }
                            //FIN - JB - 27/04/2021 - nuevo codigo

                            fn_LOAD_GRID_VISI();
                            $("#divGridProductoMedicamento").load("GridViewAjax/GridProductoMedicamento.aspx", { Codigo: $("#spCodigoProductoSeleccionado").html().trim(),
                                Producto: $("#txtProducto_Con").val().trim(),
                                Via: ValorVia,
                                CadaHora: ($("#txtCadaHora_Con").val().trim() == "" ? "PRN" : $("#txtCadaHora_Con").val().trim()),
                                Cantidad: $("#txtCantidad_Con").val().trim(),
                                Dosis: $("#txtDosis_Con").val().trim(),
                                Indicacion: $("#txtIndicacionProductoMedicamento").val().trim(),
                                TipoPedido: TipoPedido,
                                Prn: Prn,
                                /*Dia: $("#txtDiaControlClinico").val().trim(),*/
                                Pagina: "1"
                            }, function () {
                                fn_LOAD_GRID_OCUL();
                                $("#spCodigoProductoSeleccionado").html("");
                                $("#txtProducto_Con").val("");
                                $("#" + "<%= ddlVia_Con.ClientID %>").val($("#" + "<%= ddlVia_Con.ClientID %>" + " option:first").val()); //$("#" + "<= ddlVia_Con.ClientID %>").val($("#" + "<= ddlVia_Con.ClientID %>" + " option:first").val()); JB - 14/07/2020 - COMENTADO   //JB - 27/04/2021 - $("#txtVia_ControlClinico").val("");
                                $("#txtCadaHora_Con").val("");
                                $("#txtCantidad_Con").val("");
                                $("#txtDosis_Con").val("");
                                $("#txtIndicacionProductoMedicamento").val("");
                                $("#chkPRNControlClinico").prop("checked", false);
                                $("#txtCantidad_Con").removeAttr("disabled"); //JB - 05/05/2021
                                $("#txtCadaHora_Con").removeAttr("disabled"); //JB - 05/05/2021
                                $("#txtCadaHora_Con").removeClass("JCAMP-OBLI-ROJO");
                                //$("#txtDiaControlClinico").val("1");
                            });
                        } else {
                            $.JMensajePOPUP("Aviso", "El Producto ya se encuentra agregado", "1", "Cerrar", "fn_oculta_mensaje()");
                        }
                    });
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });

        $("#imgEnviarControlClinico").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    var CodigosProducto = "";
                    //fn_LOAD_VISI();
                    $("#divGridProductoMedicamento").find(".JSBTABLA tr:not(:first)").each(function () {
                        if ($(this).find("td").length > 1) {
                            CodigosProducto += $(this).find("td").eq(0).html().trim() + ";";
                        }
                    });

                    if (CodigosProducto != "") {
                        $.JMensajePOPUP("Aviso", "¿Seguro de registrar las indicaciones seleccionadas?", "ADVERTENCIA", "Si;No", "fn_GuardarControlClinico();fn_CancelaGuardarControlClinico()");
                    } else {
                        fn_LOAD_OCUL();
                        $.JMensajePOPUP("Aviso", "Debe ingresar almenos un producto.", "", "Cerrar", "fn_oculta_mensaje()");
                    }
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });

        $("#imgEnviarNoFarmacologico").click(function () {
            var bValidacion;
            bValidacion = true;  //$.JValidaCampoObligatorio("txtNutricionNoFarmacologico;txtTerapiaFisRehaNoFarmacologico;txtCuidadosEnfermeriaNoFarmacologico;txtOtrosNoFarmacologico"); JB - SE COMENTA
            if ($("#txtNutricionNoFarmacologico").val() == "" && $("#txtTerapiaFisRehaNoFarmacologico").val() == "" && $("#txtCuidadosEnfermeriaNoFarmacologico").val() == "" && $("#txtOtrosNoFarmacologico").val() == "") {
                bValidacion = false;
            }
            if (bValidacion) {
                $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "") {
                        fn_LOAD_VISI();
                        fn_GuardarNoFarmacologico();
                    } else {
                        aValores = oOB_JSON.d.toString().split(";");
                        $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                    }
                });
            } else {
                $.JMensajePOPUP("Aviso", "Debe llenar algun campo.", "", "Cerrar", "fn_oculta_mensaje()");
            }

        });


        $("#imgAgregarInfusion").click(function () {
            fn_LOAD_VISI();
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    var bValidacion;
                    bValidacion = $.JValidaCampoObligatorio("txtInfusionControlClinico");
                    if (bValidacion == false) {
                        fn_LOAD_OCUL();
                        return false;
                    }

                    $.ajax({
                        url: "InformacionPaciente.aspx/AgregarInfusion",
                        type: "POST",
                        data: JSON.stringify({
                            DscInfusion: $("#txtInfusionControlClinico").val()
                        }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        error: function (dato1, datos2, dato3) {
                        }
                    }).done(function (oOB_JSON) {
                        if (oOB_JSON.d.split(";")[0] == "ERROR") {
                            $.JMensajePOPUP("Aviso", oOB_JSON.d.split(";")[1], "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()");
                        } else {
                            if (oOB_JSON.d != "EXISTE") {
                                //$.JMensajePOPUP("Aviso", "Se guardaron los datos correctamente", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");                            
                                //var gridViewInfusiones = $("#" + "<%= gvListadoInfusiones.ClientID %>");
                                //var rowInfusiones = gridViewInfusiones.find("tr").eq(1);
                                //if ($.trim(rowInfusiones.find("td").eq(0).html()) == "" || $.trim(rowInfusiones.find("td").eq(0).html()) == "&nbsp;") {
                                //    gridViewInfusiones.find("tr").eq(1).remove();                           
                                //}
                                //clonar la primera referencia.
                                rowInfusiones = rowInfusiones.clone(true);
                                //agregando valores a las celdas
                                rowInfusiones.find("td").eq(0).html(gridViewInfusiones.find("tr").length);
                                rowInfusiones.find("td").eq(1).html($("#txtInfusionControlClinico").val().toUpperCase());
                                //agregando la fila al gridview
                                gridViewInfusiones.append(rowInfusiones);
                                fn_RefrescaEventosGridInfusiones();
                                $("#txtInfusionControlClinico").val("");
                            } else {
                                $.JMensajePOPUP("Aviso", "Ya existe una infusion con esa descripción", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()");
                            }
                            fn_LOAD_OCUL();
                        }
                    });
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });

        $("#imgEnviarInfusiones").click(function () {
            if (gridViewInfusiones.find("tr").length > 1) {
                fn_LOAD_VISI();
                $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "") {
                        fn_GuardarInfusion();
                    } else {
                        aValores = oOB_JSON.d.toString().split(";");
                        $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                    }
                });
            }
        });



        /*$("#txtDiaControlClinico").blur(function () {
        if ($("#txtDiaControlClinico").val().trim() != "" && $("#txtCadaHora_Con").val().trim() != "") {
        var Cantidad = (24 / $("#txtCadaHora_Con").val()) * $("#txtDiaControlClinico").val();
        $("#txtCantidad_Con").val(Cantidad.toFixed(2));
        }
        });*/
        $("#txtCadaHora_Con").blur(function () {
            var CodigoVia = ""; //JB - 27/04/2021 - nuevo codigo
            CodigoVia = $("#" + "<%=ddlVia_Con.ClientID %>").val().trim(); //JB - 27/04/2021 - nuevo codigo

            if (CodigoVia == "01" || CodigoVia == "02" || CodigoVia == "06" || CodigoVia == "07" || CodigoVia == "08" || CodigoVia == "14") {

            } else {
                if ($("#txtCadaHora_Con").val().trim() != "") {
                    var Cantidad = (24 / $("#txtCadaHora_Con").val()) * 1;
                    $("#txtCantidad_Con").val(Cantidad);
                    $.JValidaCampoObligatorio("txtCantidad_Con");
                }
            }

        });


        /*INICIO - 27/10/2016*/
        $("#imgNuevoControlClinico").click(function () {

            $.ajax({
                url: "InformacionPaciente.aspx/VerificarAlertas", //verifica si hay alertar lab o img
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (dato1, datos2, dato3) {
                }
            }).done(function (oOB_JSON) {
                aValoresAlerta = oOB_JSON.d.toString().split(";");
                if (aValoresAlerta[0] == "ALERTA") {
                    //if (aValoresAlerta[1] == "M") { //solo mostrara a los medicos esta alerta
                    $.ajax({
                        url: "InformacionPaciente.aspx/VerificarAlertas3", //obtiene cadena de mensajes
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        error: function (dato1, datos2, dato3) {
                        }
                    }).done(function (oOB_JSON) {
                        if (oOB_JSON.d != "") {
                            $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe", oOB_JSON.d, "Aceptar", "Cerrar", "fn_AceptaMenjeNuevoCC()");
                        }
                    });
                    //}
                } else {
                    fn_AceptaMenjeNuevoCC();
                }
            });
        });


        $("#btnImprimirCC").click(function () {
            $.ajax(
                {
                    url: "InformacionPaciente.aspx/ValidaSession",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json"
                }).done(function (oOB_JSON)
                {
                if (oOB_JSON.d == "") {
                    //var IdReceta = $("#divControlClinicoIndicacionMed").find(".JTREE2-SELECCIONADO").next().val(); SE REEMPLAZA POR LAS LINEAS DEBAJO
                    var IdReceta = $("#divControlClinicoIndicacionMed2").find(".JTREE3-SELECCIONADO > .IdeRecetaCab").val();
                    if (IdReceta != undefined && IdReceta != null && IdReceta != "") {
                        $.ajax({
                            url: "InformacionPaciente.aspx/VerificarReporteCC", //verifica si data para imprimir en el reporte
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({
                                IdeReceta: IdReceta,
                                FecReceta: ""
                            }),
                            error: function (dato1, datos2, dato3) {
                            }
                        }).done(function (oOB_JSON) {
                            if (oOB_JSON.d == "OK") {
                                fn_GuardaLog("CONTROL CLINICO", "Se imprimio receta " + IdReceta);
                                window.open("VisorReporte.aspx?OP=IM&Valor=ID;" + IdReceta.toString());
                            } else {
                                if (oOB_JSON.d == "") {

                                } else {
                                    $.JMensajePOPUP("Error", oOB_JSON.d.split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()");
                                }
                            }
                        });

                        /*fn_GuardaLog("CONTROL CLINICO", "Se imprimio receta " + IdReceta);                        
                        window.open("VisorReporte.aspx?OP=IM&Valor=ID;" + IdReceta.toString());*/
                    } else {
                        //var FecReceta = $("#divControlClinicoIndicacionMed").find(".JTREE2-SELECCIONADO").prev().val(); SE REEMPLAZA POR LAS LINEAS DEBAJO
                        var FecReceta = $("#divControlClinicoIndicacionMed2").find(".JTREE3-SELECCIONADO > .FecRegistro").val();
                        if (FecReceta != undefined && FecReceta != null && FecReceta != "") {
                            $.ajax({
                                url: "InformacionPaciente.aspx/VerificarReporteCC", //verifica si data para imprimir en el reporte
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: JSON.stringify({
                                    IdeReceta: "0",
                                    FecReceta: FecReceta
                                }),
                                error: function (dato1, datos2, dato3) {
                                }
                            }).done(function (oOB_JSON) {

                                if (oOB_JSON.d == "OK") {
                                    window.open("VisorReporte.aspx?OP=IM&Valor=FE;" + FecReceta.toString());
                                } else {
                                    if (oOB_JSON.d == "") {

                                    } else {
                                        $.JMensajePOPUP("Error", oOB_JSON.d.split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()");
                                    }
                                }
                            });
                            //window.open("VisorReporte.aspx?OP=IM&Valor=FE;" + FecReceta.toString());
                        } else {
                            $.JMensajePOPUP("Aviso", "No hay registro seleccionado para imprimir.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()");
                            return;
                        }
                    }

                    /*$("#divControlClinicoIndicacionMed > ul > li > a").each(function () {
                    if ($(this).css("color") == "rgb(141, 199, 63)") { //8DC73F
                    IdReceta = $(this).next().val().trim();
                    fn_GuardaLog("CONTROL CLINICO", "Se imprimio receta " + IdReceta);
                    //window.open("Reporte.aspx?OP=PE2&IM=" + IdReceta);
                    window.open("VisorReporte.aspx?OP=IM&Valor=")
                    }
                    });*/

                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });

        });


        /*********************************************************************************************************/
        /************************************************DIAGNOSTICO**********************************************/
        /*********************************************************************************************************/
        $("#divGridDiagnosticos").load("GridViewAjax/GridDiagnostico.aspx", function () {

        });

        $("#txtDiagnostico").keypress(function (e) {
            if (e.which == 13) {
                $("#imgBuscarDiagnostico").trigger("click");
            }
        });

        $("#imgBuscarDiagnostico").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    if ($("#rbBuscarCodigoDiagnotico").prop("checked")) {
                        if ($("#txtDiagnostico").val().trim().length > 0) {
                            $("#divFONDO").css("display", "inline");
                            $("#divBusquedaDiagnostico").css("display", "block");
                            var NroOrden = 0;
                            if ($("#rbBuscarCodigoDiagnotico").prop("checked")) {
                                NroOrden = 6;
                            }
                            if ($("#rbBuscarNombreDiagnotico").prop("checked")) {
                                NroOrden = 1;
                            }


                            $("#divBusquedaDiagnostico").load("Utilidad/BusquedaDiagnostico.aspx", { Nombre: $("#txtDiagnostico").val().trim(), TipoBusqueda: "G", Orden: NroOrden }, function () {
                                $(this).find("#hfTipoBusqueda").val("G");
                                fn_CrearEventoDiagnostico();
                            });
                        } else {
                            $.JMensajePOPUP("Aviso", "Debe ingresar al menos 1 caracter para realizar una búsqueda.", "1", "Cerrar", "fn_oculta_mensaje()");
                        }
                    } else {
                        if ($("#txtDiagnostico").val().trim().length > 3) {
                            $("#divFONDO").css("display", "inline");
                            $("#divBusquedaDiagnostico").css("display", "block");
                            var NroOrden = 0;
                            if ($("#rbBuscarCodigoDiagnotico").prop("checked")) {
                                NroOrden = 6;
                            }
                            if ($("#rbBuscarNombreDiagnotico").prop("checked")) {
                                NroOrden = 1;
                            }


                            $("#divBusquedaDiagnostico").load("Utilidad/BusquedaDiagnostico.aspx", { Nombre: $("#txtDiagnostico").val().trim(), TipoBusqueda: "G", Orden: NroOrden }, function () {
                                $(this).find("#hfTipoBusqueda").val("G");
                                fn_CrearEventoDiagnostico();
                            });
                        } else {
                            $.JMensajePOPUP("Aviso", "Debe ingresar al menos 4 caracteres para realizar una búsqueda.", "1", "Cerrar", "fn_oculta_mensaje()");
                        }
                    }



                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });

        $("#imgFavoritoDiagnostico").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $("#divFONDO").css("display", "inline");
                    $("#divBusquedaDiagnostico").css("display", "block");
                    $("#divBusquedaDiagnostico").load("Utilidad/BusquedaDiagnostico.aspx", { Nombre: $("#txtDiagnostico").val().trim(), TipoBusqueda: "F", Orden: "2" }, function () {
                        $(this).find("#hfTipoBusqueda").val("F");
                        fn_CrearEventoDiagnostico();
                    });
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });


        $("#chkFavoritoDiagnostico").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    if ($(this).prop("checked") == false && $("#hfFavoritoDiagnostico").val().trim() != "" && $("#hfFavoritoDiagnostico").val().trim() != "0") {
                        $.JMensajePOPUP("Aviso", "¿Desea Eliminar de Favoritos?", "2", "Si;No", "fn_EliminaFavoritoDiagnostico();fn_CancelarEliminaFavoritoDiagnostico()");
                    }
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });


        $("#imgAgregarDiagnostico").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    var bValidacion = true;
                    var TipoDiagnostico = "";
                    var TipoES = "";

                    if ($("#rbTipoE").prop("checked") == true) {
                        TipoES = "E";
                    }
                    if ($("#rbTipoS").prop("checked") == true) {
                        TipoES = "S";
                    }

                    $.ajax({
                        url: "InformacionPaciente.aspx/ValidarDiagnosticoAgregado",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            CodDiagnostico: $("#spCodigoDiagnosticoSeleccionado").html().trim(),
                            Tipo: TipoES
                        }),
                        dataType: "json",
                        error: function (dato1, datos2, dato3) {

                        }
                    }).done(function (oOB_JSON) {
                        if (oOB_JSON.d == "") {
                            if ($("#spCodigoDiagnosticoSeleccionado").html().trim() == "") {
                                $.JMensajePOPUP("Aviso", "Seleccione un diagnóstico.", "1", "Cerrar", "fn_oculta_mensaje()");
                                return false;
                            }

                            if ($("#rbTipoE").prop("checked") == true) {
                                TipoES = "E"; //E= Entrada  -  I= Ingreso
                            }
                            if ($("#rbTipoS").prop("checked") == true) {
                                TipoES = "S"; //S= Salida      E= Egreso
                            }

                            if ($("#chkPresuntivoDiagnostico").prop("checked") == true) {
                                TipoDiagnostico = "P";
                            }
                            if ($("#chkRepetidoDiagnostico").prop("checked") == true) {
                                TipoDiagnostico = "R";
                            }
                            if ($("#chkDefinitivoDiagnostico").prop("checked") == true) {
                                TipoDiagnostico = "D";
                            }

                            if (TipoDiagnostico == "") {
                                $.JMensajePOPUP("Aviso", "Seleccione un tipo de diagnóstico.", "1", "Cerrar", "fn_oculta_mensaje()");
                                return false;
                            }
                            if (TipoES == "") {
                                $.JMensajePOPUP("Aviso", "Seleccione un tipo de Entrada.", "1", "Cerrar", "fn_oculta_mensaje()");
                                return false;
                            }

                            var Favorito = "";
                            if ($("#chkFavoritoDiagnostico").prop("checked") == true) {
                                Favorito = "SI";
                            }
                            fn_LOAD_VISI();
                            $.ajax({
                                url: "InformacionPaciente.aspx/AgregarDiagnostico",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: JSON.stringify({
                                    TipoDiagnostico: TipoDiagnostico,
                                    CodDiagnostico: $("#spCodigoDiagnosticoSeleccionado").html().trim(),
                                    Favorito: Favorito,
                                    Tipo: TipoES
                                }),
                                error: function (dato1, datos2, dato3) {

                                }
                            }).done(function (oOB_JSON) {
                                fn_LOAD_OCUL();
                                if (oOB_JSON.d == "OK") {
                                    $("#txtDiagnostico").val("");
                                    $("#spCodigoDiagnosticoSeleccionado").html("");
                                    $("#spDiagnosticoSeleccionado").html("");
                                    $("#chkDiagnosticoSeleccionado").prop("checked", false);
                                    $("#chkPresuntivoDiagnostico").prop("checked", false);
                                    $("#chkRepetidoDiagnostico").prop("checked", false);
                                    $("#chkDefinitivoDiagnostico").prop("checked", false);
                                    $("#chkFavoritoDiagnostico").prop("checked", false);
                                    $("#rbTipoE").prop("checked", false);
                                    $("#rbTipoS").prop("checked", false);
                                    $("#divGridDiagnosticos").load("GridViewAjax/GridDiagnostico.aspx", function () {

                                    });
                                    fn_CargarEvolucionClinica2("1", "0");
                                } else {
                                    $.JMensajePOPUP("Error", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_mensaje()");
                                }
                            });
                        } else {
                            $.JMensajePOPUP("Aviso", "El diagnóstico ya se encuentra seleccionado.", "1", "Cerrar", "fn_oculta_mensaje()");
                            return;
                        }
                    });
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });

        /*********************************************************************************************************/
        /**********************************************INTERCONSULTA**********************************************/
        /*********************************************************************************************************/
        $("#divGridInterconsulta").load("GridViewAjax/GridInterconsulta.aspx", function () {
            fn_CreaEventoGridInterconsulta();
        });

        $("#txtEspecialidadInterconsulta").keypress(function (e) {
            if (e.which == 13) {
                $("#imgBusquedaEspecialidadInterconsulta").trigger("click");
            }

        });

        $("#txtEspecialidadInterconsulta").keyup(function () {
            if ($("#txtEspecialidadInterconsulta").val().trim() == "") {
                $("#spCodigoEspecialidadSeleccionado").html("");
            }
        });

        $("#imgBusquedaEspecialidadInterconsulta").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    if ($("#spCodigoMedicoSeleccionado").html().trim() == "") { //****************
                        if ($("#txtEspecialidadInterconsulta").val().trim().length > 3) {
                            $("#divFONDO").css("display", "inline");
                            $("#divBusquedaEspecialidad").css("display", "block");
                            $("#divBusquedaEspecialidad").load("Utilidad/BusquedaEspecialidad.aspx", { Nombre: $("#txtEspecialidadInterconsulta").val().trim(), Medico: $("#spCodigoMedicoSeleccionado").html() }, function () {
                                fn_CrearEventoEspecialidad();
                                /*$("#spCodigoMedicoSeleccionado").html("");
                                $("#txtMedicoInterconsulta").val("");*/
                            });
                        } else {
                            $.JMensajePOPUP("Aviso", "Debe ingresar al menos 4 caracteres para realizar una búsqueda.", "1", "Cerrar", "fn_oculta_mensaje()");
                        }
                    } else {
                        $("#divFONDO").css("display", "inline");
                        $("#divBusquedaEspecialidad").css("display", "block");
                        $("#divBusquedaEspecialidad").load("Utilidad/BusquedaEspecialidad.aspx", { Nombre: $("#txtEspecialidadInterconsulta").val().trim(), Medico: $("#spCodigoMedicoSeleccionado").html() }, function () {
                            fn_CrearEventoEspecialidad();
                            /*$("#spCodigoMedicoSeleccionado").html("");
                            $("#txtMedicoInterconsulta").val("");*/
                        });
                    }

                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });

        $("#txtMedicoInterconsulta").keypress(function (e) {
            if (e.which == 13) {
                $("#imgBusquedaMedicoInterconsulta").trigger("click");
            }
        });
        //EVENTO PARA BUSQUEDA DE INTERCONSULTA
        $("#imgBusquedaMedicoInterconsulta").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    if ($("#txtMedicoInterconsulta").val().trim().length > 3) {
                        $("#divFONDO").css("display", "inline");
                        $("#divBusquedaMedico").css("display", "block");
                        $("#divBusquedaMedico").load("Utilidad/BusquedaMedico.aspx", { Nombre: $("#txtMedicoInterconsulta").val().trim(), Especialidad: $("#spCodigoEspecialidadSeleccionado").html() }, function () {
                            fn_CrearEventoMedico();
                        });
                    } else {
                        $.JMensajePOPUP("Aviso", "Debe ingresar al menos 4 caracteres para realizar una búsqueda.", "1", "Cerrar", "fn_oculta_mensaje()");
                    }
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });

        //EVENTO PARA GUARDAR INTERCONSULTA
        $("#btnGuardarInterconsulta").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    //AQUI EL CODIGO SI LA SESSION AUN NO EXPIRA       
                    if ($("#spCodigoEspecialidadSeleccionado").html().trim() == "") {
                        $.JMensajePOPUP("Validacion", "Debe ingresar el campo especialidad", "", "Cerrar", "fn_oculta_mensaje()");
                        return;
                    }
                    fn_LOAD_VISI();
                    $.ajax({
                        url: "InformacionPaciente.aspx/GuardarInterconsulta",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            IdInterconsulta: $("#hfIdInterconsulta").val().trim(),
                            IdMotivo: $("#" + "<%=ddlMotivo.ClientID %>").val().trim(),
                            Descripcion: $("#txtDescripcionInterconsulta").val().trim(),
                            CodEspecialidad: $("#spCodigoEspecialidadSeleccionado").html().trim(),
                            CodMedico: $("#spCodigoMedicoSeleccionado").html().trim()
                        }),
                        dataType: "json"
                    }).done(function (oOB_JSON) {
                        fn_LOAD_OCUL();
                        if (oOB_JSON.d != "OK") {
                            $.JMensajePOPUP("Error", oOB_JSON.d.toString().split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()");
                        } else {
                            $.JMensajePOPUP("Aviso", "Se guardo los datos de la interconsulta correctamente.", "OK", "Cerrar", "fn_oculta_mensaje()");
                            $("#hfIdInterconsulta").val("");
                            //$("#" + "<%=ddlMotivo.ClientID %>").val("1");
                            $("#" + "<%=ddlMotivo.ClientID %>" + " option:first-child").attr("selected", "selected");
                            $("#txtDescripcionInterconsulta").val("");
                            $("#spCodigoEspecialidadSeleccionado").html("");
                            $("#spCodigoMedicoSeleccionado").html("");
                            $("#txtEspecialidadInterconsulta").val("");
                            $("#txtMedicoInterconsulta").val("");
                            $("#divGridInterconsulta").load("GridViewAjax/GridInterconsulta.aspx", function () { fn_CreaEventoGridInterconsulta(); });
                            //fn_CargarEvolucionClinica(); //CARGARA EVOLUCION CLINICA TMACASSI 07/09/2016
                            fn_CargarEvolucionClinica2("1", "0");
                        }
                    });
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });


        /********************************PROCEDIMIENTO MEDICO************************/
        fn_CrearTreeViewProcedimientoMedico("1", "", "0");
        $("#txtProcedimientoMedico").attr("disabled", "disabled");
        $("#txtProcedimientoMedico").keypress(function (e) {
            if (e.which == 13) {
                $("#imgBusquedaProcedimientoMedico").trigger("click");
            }
        });



        $("#imgBusquedaProcedimientoMedico").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    /*if ($("#txtProcedimientoMedico").val().trim().length > 3) {
                    $("#divFONDO").css("display", "inline");
                    $("#divBusquedaProcedimientoMedico").html("");
                    $("#divBusquedaProcedimientoMedico").css("display", "block");
                    $("#divBusquedaProcedimientoMedico").load("Utilidad/BusquedaProcedimientoMedico.aspx", { Nombre: $("#txtProcedimientoMedico").val().trim(), TipoBusqueda: "N" }, function () {
                    fn_CrearEventoProcedimientoMedico();
                    });
                    } else {
                    $.JMensajePOPUP("Aviso", "Debe ingresar al menos 4 caracteres para realizar una búsqueda.", "1", "Cerrar", "fn_oculta_mensaje()");
                    }*/

                    if ($("#txtProcedimientoMedico").val().trim().length > 2) {
                        $.ajax({
                            url: "InformacionPaciente.aspx/ConsultaCpt",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({
                                CodCpt: $("#txtProcedimientoMedico").val()
                            }),
                            error: function (dato1, datos2, dato3) {

                            }
                        }).done(function (oOB_JSON) {
                            if (oOB_JSON.d != "") {
                                var xmlDoc = $.parseXML(oOB_JSON.d);
                                var xml = $(xmlDoc);
                                var ProcedimientoMedicos = xml.find("TablaProcedimientoMedicoConsulta");

                                $(ProcedimientoMedicos).each(function () {
                                    $("#ddlSeccionProcedimientoMedico").data("codigo", $(this).find("cod_seccion").text());
                                    $("#ddlSeccionProcedimientoMedico").data("descripcion", $(this).find("seccion").text());

                                    $("#ddlSubSeccionProcedimientoMedico").data("codigo", $(this).find("cod_subseccion").text());
                                    $("#ddlSubSeccionProcedimientoMedico").data("descripcion", $(this).find("sub_seccion").text());

                                    $("#ddlDescripcionCPT").data("codigo", $(this).find("cod_cpt").text());
                                    $("#ddlDescripcionCPT").data("descripcion", $(this).find("dsc_cpt").text());

                                    $("#ddlSeccionProcedimientoMedico").find(".JSELECT2-SELECCION").html($(this).find("seccion").text());
                                    $("#ddlSubSeccionProcedimientoMedico").find(".JSELECT2-SELECCION").html(($(this).find("sub_seccion").text() == "" ? "-" : $(this).find("sub_seccion").text()));
                                    $("#ddlDescripcionCPT").find(".JSELECT2-SELECCION").html(($(this).find("dsc_cpt").text() == "" ? "-" : $(this).find("dsc_cpt").text()));

                                    $("#spCodigoProcedimientoSeleccionado").html($(this).find("cod_cpt").text());


                                    fn_InicializarCombo();
                                    $("#ddlSeccionProcedimientoMedico").find(".JSELECT2-SELECCION").unbind("click");
                                    $("#ddlSubSeccionProcedimientoMedico").find(".JSELECT2-SELECCION").unbind("click");
                                    $("#ddlDescripcionCPT").find(".JSELECT2-SELECCION").unbind("click");
                                    //AQUI TMNB VERIFICAR SI ES FAVORITO

                                    $("#ddlSeccionProcedimientoMedico").find(".JSELECT2-ELEMENT").parent().parent().css("position", "relative");
                                    $("#ddlSubSeccionProcedimientoMedico").find(".JSELECT2-ELEMENT").parent().parent().css("position", "relative");
                                    $("#ddlDescripcionCPT").find(".JSELECT2-ELEMENT").parent().parent().css("position", "relative");
                                });

                            }
                        });
                    } else {
                        $.JMensajePOPUP("Aviso", "Debe ingresar al menos 3 caracteres para realizar una búsqueda.", "1", "Cerrar", "fn_oculta_mensaje()");
                    }

                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });

        $("#imgFavoritoProcedimientoMedico").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    if ($("#txtProcedimientoMedico").val().trim().length > -1) {
                        $("#divFONDO").css("display", "inline");
                        $("#divBusquedaProcedimientoMedico").html("");
                        $("#divBusquedaProcedimientoMedico").css("display", "block");
                        $("#divBusquedaProcedimientoMedico").load("Utilidad/BusquedaProcedimientoMedico.aspx", { Nombre: $("#txtProcedimientoMedico").val().trim(), TipoBusqueda: "F" }, function () {
                            fn_CrearEventoProcedimientoMedico("F");
                        });
                    } else {
                        $.JMensajePOPUP("Aviso", "Debe ingresar al menos 3 caracteres para realizar una búsqueda.", "1", "Cerrar", "fn_oculta_mensaje()");
                    }

                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });

        $("#chkBuscarCodProcedimientoMedico").click(function () {

            fn_DeshabilitarHabilitarCombo("ddlSeccionProcedimientoMedico");
            fn_DeshabilitarHabilitarCombo("ddlSubSeccionProcedimientoMedico");
            fn_DeshabilitarHabilitarCombo("ddlDescripcionCPT");
            if ($(this).prop("checked") == true) {
                $("#txtProcedimientoMedico").removeAttr("disabled");
                $("#txtProcedimientoMedico").val(""); //limpiando campo codigo procedimiento

                $("#ddlDescripcionCPT").removeData("descripcion");
                $("#ddlDescripcionCPT").removeData("codigo");
                $("#ddlSeccionProcedimientoMedico").find(".JSELECT2-SELECCION").html("-");
                $("#ddlSubSeccionProcedimientoMedico").find(".JSELECT2-SELECCION").html("-");
                $("#ddlDescripcionCPT").find(".JSELECT2-SELECCION").html("-");
                fn_InicializarCombo();

                fn_ConsultaSeccion("", "", "1", "");
                fn_ConsultaSeccion("", "", "2", "");
                fn_ConsultaSeccion("", "", "3", "");
            } else {
                $("#txtProcedimientoMedico").attr("disabled", "disabled");
                $("#txtProcedimientoMedico").val("");
                //limpiando combos
                $("#ddlDescripcionCPT").removeData("descripcion");
                $("#ddlDescripcionCPT").removeData("codigo");
                $("#ddlSeccionProcedimientoMedico").find(".JSELECT2-SELECCION").html("-");
                $("#ddlSubSeccionProcedimientoMedico").find(".JSELECT2-SELECCION").html("-");
                $("#ddlDescripcionCPT").find(".JSELECT2-SELECCION").html("-");
                fn_InicializarCombo();

                fn_ConsultaSeccion("", "", "1", "");
                fn_ConsultaSeccion("", "", "2", "");
                fn_ConsultaSeccion("", "", "3", "");
            }

        });

        $("#chkFavoritoProcedimientoMedico").click(function () {
            if ($(this).prop("checked")) {
                if ($("#spCodigoProcedimientoSeleccionado").html() != "") {
                    $.ajax({
                        url: "InformacionPaciente.aspx/InsertarFavoritoCpt",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({
                            CodCpt: $("#spCodigoProcedimientoSeleccionado").html()
                        }),
                        error: function (dato1, datos2, dato3) {

                        }
                    }).done(function (oOB_JSON) {
                        if (oOB_JSON.d == "OK") {

                        } else {
                            $.JMensajePOPUP("Aviso", oOB_JSON.d.split(";")[1], "1", "Cerrar", "fn_oculta_mensaje()");
                        }
                    });
                } else {
                    $("#chkFavoritoProcedimientoMedico").prop("checked", false);
                }
            } else {
                if ($("#spCodigoProcedimientoSeleccionado").html() != "") {
                    $.ajax({
                        url: "InformacionPaciente.aspx/EliminaFavoritoCpt",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({
                            CodCpt: $("#spCodigoProcedimientoSeleccionado").html()
                        }),
                        error: function (dato1, datos2, dato3) {

                        }
                    }).done(function (oOB_JSON) {
                        if (oOB_JSON.d == "OK") {

                        } else {
                            $.JMensajePOPUP("Aviso", oOB_JSON.d.split(";")[1], "1", "Cerrar", "fn_oculta_mensaje()");
                        }
                    });
                } else {
                    $("#chkFavoritoProcedimientoMedico").prop("checked", false);
                }
            }
        });


        $("#btnGuardarProcedimientoMedico").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    //AQUI EL CODIGO SI LA SESSION AUN NO EXPIRA                         
                    if ($("#ddlDescripcionCPT").data("codigo") == "" || $("#ddlDescripcionCPT").data("codigo") == undefined) {
                        $.JMensajePOPUP("Validacion", "Debe ingresar el procedimiento", "", "Cerrar", "fn_oculta_mensaje()");
                        return;
                    }
                    if ($("#txtRelatoCpt").val().trim() == "") {
                        $.JMensajePOPUP("Validacion", "Debe ingresar el relato", "", "Cerrar", "fn_oculta_mensaje()");
                        return;
                    }
                    fn_LOAD_VISI();
                    $.ajax({
                        url: "InformacionPaciente.aspx/GuardarProcedimientoMedico",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            Descripcion: $("#ddlDescripcionCPT").data("descripcion"), //$("#txtDescripcionProcedimiento").val().trim(),
                            CodProcedimiento: $("#ddlDescripcionCPT").data("codigo"),  //$("#spCodigoProcedimientoSeleccionado").html().trim(),
                            NomProcedimiento: $("#txtProcedimientoMedico").val().trim(),
                            Relato: $("#txtRelatoCpt").val().trim()
                        }),
                        dataType: "json"
                    }).done(function (oOB_JSON) {
                        fn_LOAD_OCUL();
                        if (oOB_JSON.d != "OK") {
                            $.JMensajePOPUP("Error", oOB_JSON.d.toString().split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()");
                        } else {
                            $.JMensajePOPUP("Aviso", "Se guardo los datos correctamente.", "OK", "Cerrar", "fn_oculta_mensaje()");
                            $("#txtProcedimientoMedico").val("");
                            $("#spCodigoProcedimientoSeleccionado").html("");
                            //$("#txtDescripcionProcedimiento").val("");
                            $("#ddlDescripcionCPT").removeData("descripcion");
                            $("#ddlDescripcionCPT").removeData("codigo");
                            $("#ddlSeccionProcedimientoMedico").find(".JSELECT2-SELECCION").html("-");
                            $("#ddlSubSeccionProcedimientoMedico").find(".JSELECT2-SELECCION").html("-");
                            $("#ddlDescripcionCPT").find(".JSELECT2-SELECCION").html("-");
                            fn_InicializarCombo();
                            fn_ConsultaSeccion("", "", "1", "");
                            fn_ConsultaSeccion("", "", "2", "");
                            fn_ConsultaSeccion("", "", "3", "");
                            fn_CrearTreeViewProcedimientoMedico("1", "", "0");
                            $("#chkFavoritoProcedimientoMedico").prop("checked", false);
                            if ($("#chkBuscarCodProcedimientoMedico").prop("checked")) {
                                $("#chkBuscarCodProcedimientoMedico").trigger("click");
                            }
                            $("#txtRelatoCpt").val("");
                        }
                    });
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });


        /********************************PATOLOGIA***********************************/
        $("#btnVerInformePatologia").click(function () {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    var IdePatologiaDet = "0"
                    IdePatologiaDet = $("#divTreePatologia").find(".JTREE2-SELECCIONADO").parent().find("> input").eq(0).val();

                    if (IdePatologiaDet.trim() == "0" || IdePatologiaDet.trim() == "" || IdePatologiaDet == undefined) {
                        $.JMensajePOPUP("Error", "Seleccione una patologia para visualizar el informe", "ERROR", "Cerrar", "fn_oculta_mensaje()");
                        return;
                    }

                    $.ajax({
                        url: "InformacionPaciente.aspx/VerInformePatologia",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            IdePatologiaDet: IdePatologiaDet
                        }),
                        dataType: "json"
                    }).done(function (oOB_JSON) {
                        fn_LOAD_OCUL();
                        if (oOB_JSON.d.split(";")[0] == "ERROR") {
                            $.JMensajePOPUP("Error", oOB_JSON.d.toString().split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()");
                        } else {
                            var CodigoDocumento = oOB_JSON.d.split(";")[1];
                            for (var i = 0; i < CodigoDocumento.split("_").length; i++) {
                                if (CodigoDocumento.split("_")[i] != "") {
                                    window.open("VisorReporte.aspx?OP=INFORME_PATOLOGIA&Valor=" + CodigoDocumento.split("_")[i]);
                                }
                            }
                        }
                    });


                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });

        fn_CargaPermiso();

    });


    function fn_LimpiarGridPatologia() {
        var row = $("[id*=gvPatologia] tr:last").clone();
        $("[id*=gvPatologia] tr:gt(0)").remove();
        $("td:nth-child(1)", row).html("");
        $("td:nth-child(2)", row).html("");
        $("td:nth-child(3)", row).html("");
        $("td:nth-child(4)", row).html("");
        $("td:nth-child(5)", row).html("");
        $("td:nth-child(6)", row).html("");
        $("td:nth-child(7)", row).html("");
        $("td:nth-child(8)", row).html("");
        $("td:nth-child(9)", row).html("");
        $("td:nth-child(10)", row).html("");
        row.css("display", "none");
        $("[id*=gvPatologia] tbody").append(row);
    }

    function fn_EventosGridPatologia() {
        /*PATOLOGIA*/
        $("#" + "<%=gvPatologia.ClientID %>").find("tr").each(function (e) {
            var ObjetoFila = $(this);
            if (ObjetoFila.find('.select-patologia').length > 0) {
                var $SelectMuestra = ObjetoFila.find('.select-patologia').selectize({
                    SeleccionMaxima: 1,
                    AbrirEnFoco: false,
                    MaxElementos: 10
                });
                if (e > 0) {
                    if (ObjetoFila.find(".ORGANO-OCULTO").html() != undefined) {
                        if (ObjetoFila.find(".ORGANO-OCULTO").html() != "") {
                            //Observaciones Cmendez 02/05/2022
                            var aAR_ORGA = $.trim(ObjetoFila.find(".ORGANO-OCULTO").html().split("|"));
                            var selectize = $SelectMuestra[0].selectize;
                            selectize.setValue(aAR_ORGA);
                        }
                        if (ObjetoFila.find(".CANTIDAD-OCULTO").html() != "") {
                            ObjetoFila.find(".TEXTO-CANTIDAD ").val($.trim(ObjetoFila.find(".CANTIDAD-OCULTO").html()));
                        }
                    }
                }
            }
        });
        $("#" + "<%=gvPatologia.ClientID %>").find(".selectize-control.multi .selectize-input [data-value]").addClass("ClaseBuscadorPatologia1");
        $("#" + "<%=gvPatologia.ClientID %>").find(".selectize-control.multi .selectize-input > div").addClass("ClaseBuscadorPatologia2");



        $(".select-patologia").unbind("change");
        $(".select-patologia").change(function () {
            $("#" + "<%=hfOrganosSeleccionados.ClientID %>").val($(this).parent().parent().index() + "_" + $(this).val());
            //$("#" + "<%=btnActualizarGridPatologias.ClientID %>").trigger("click");
            $.ajax({
                url: "InformacionPaciente.aspx/ActualizarGridPato",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    OrganosSeleccionados: $("#" + "<%=hfOrganosSeleccionados.ClientID %>").val(),
                    CantidadSeleccionada: $("#" + "<%=hfCantidadSeleccionados.ClientID %>").val()
                }),
                dataType: "json",
                error: function (dato1, datos2, dato3) {

                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "OK") {
                    $("#" + "<%=hfOrganosSeleccionados.ClientID %>").val("");
                    $("#" + "<%=hfCantidadSeleccionados.ClientID %>").val("");
                }
            });
            $("#" + "<%=gvPatologia.ClientID %>").find(".selectize-control.multi .selectize-input [data-value]").addClass("ClaseBuscadorPatologia1");
            $("#" + "<%=gvPatologia.ClientID %>").find(".selectize-control.multi .selectize-input > div").addClass("ClaseBuscadorPatologia2");
        });

        $(".TEXTO-CANTIDAD").unbind("click");
        $(".TEXTO-CANTIDAD").click(function () {
            if ($.trim($(this).parent().parent().find("td").eq(1).html()) == "PAPANICOLAU CERVICO VAGINAL") { //esta patologia solo debe permitir cantidad 1
                $(this).val($.trim($(this).val()));
                $(this).attr("readonly", true);
            } else {
                $(this).removeAttr("readonly", true);
                $(this).val($.trim($(this).val()));
                this.select();
            }
        });

        $(".TEXTO-CANTIDAD").unbind("blur");
        $(".TEXTO-CANTIDAD").blur(function () {
            $("#" + "<%=hfCantidadSeleccionados.ClientID %>").val($(this).parent().parent().index() + "_" + $(this).val());
            //$("#" + "<%=btnActualizarGridPatologias.ClientID %>").trigger("click");
            var OrganoSelecc = $("#" + "<%=hfOrganosSeleccionados.ClientID %>").val();
            var CantidadSelecc = $("#" + "<%=hfCantidadSeleccionados.ClientID %>").val();            
            $.ajax({
                url: "InformacionPaciente.aspx/ActualizarGridPato",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    OrganosSeleccionados: $("#" + "<%=hfOrganosSeleccionados.ClientID %>").val(),
                    CantidadSeleccionada: $("#" + "<%=hfCantidadSeleccionados.ClientID %>").val()
                }),
                dataType: "json",
                error: function (dato1, datos2, dato3) {

                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "OK") {
                    $("#" + "<%=hfOrganosSeleccionados.ClientID %>").val("");
                    $("#" + "<%=hfCantidadSeleccionados.ClientID %>").val("");
                }
            });
            $("#" + "<%=gvPatologia.ClientID %>").find(".selectize-control.multi .selectize-input [data-value]").addClass("ClaseBuscadorPatologia1");
            $("#" + "<%=gvPatologia.ClientID %>").find(".selectize-control.multi .selectize-input > div").addClass("ClaseBuscadorPatologia2");
        });
        //la fecha del campo 'FUR' va en el campo de patologia
        $("#" + "<%=TxtFechaUltimaRegla.ClientID %>").val($('[id^=txt_fur-]').val());


        $("[id*=gvPatologia]").find(".JIMG-ELIMINAR").unbind("click");
        $("[id*=gvPatologia]").find(".JIMG-ELIMINAR").click(function () {              
            fn_LOAD_VISI();
            var codigo = $(this).parent().parent().find("td").eq(7).html().trim();
            var objeto = $(this);
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {                    
                    $.ajax({
                        url: "InformacionPaciente.aspx/EliminarPatologia",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({
                            IdePatologiaMae: codigo
                        }),
                        error: function (dato1, datos2, dato3) {

                        }
                    }).done(function (oOB_JSON) {
                        if ($("[id*=gvPatologia]").find("tr").length == 2) {
                            var row = $("[id*=gvPatologia] tr:last").clone();
                            objeto.parent().parent().remove();
                            $("td:nth-child(1)", row).html("");
                            $("td:nth-child(2)", row).html("");
                            $("td:nth-child(3)", row).html("");
                            $("td:nth-child(4)", row).html("");
                            $("td:nth-child(5)", row).html("");
                            $("td:nth-child(6)", row).html("");
                            $("td:nth-child(7)", row).html("");
                            $("td:nth-child(8)", row).html("");
                            $("td:nth-child(9)", row).html("");
                            $("td:nth-child(10)", row).html("");
                            row.css("display", "none");
                            $("[id*=gvPatologia] tbody").append(row);
                        } else {
                            objeto.parent().parent().remove();  
                        }
                        //objeto.parent().parent().remove();   
                        fn_LOAD_OCUL();
                    });
                }
            });            
        });

    }

    function fn_LimpiarCheckOtrosPatologia() {
        $(".JCHECK-PATOLOGIA").find(":input[type='checkbox']").each(function () {
            $(this).prop("checked", false);
        });
        if ($('#ddlOtrosPatologia')[0].selectize != undefined) {
            $('#ddlOtrosPatologia')[0].selectize.disable();
        }        
    }

    

    function fn_AgregarPatologia() {
        //$("#" + "<%=btnAgregarPatologia.ClientID %>").trigger("click");
        if ($("#" + "<%=hfIdPatologiaSeleccionado.ClientID %>").val().trim() == "" && $("#" + "<%=hfCheckPatologiaSeleccionado.ClientID %>").val().trim() == "") {
            fn_MESG_POPU("Seleccionar una patología.");
            return;
        }        
        fn_LOAD_VISI();
        $.ajax({
            url: "InformacionPaciente.aspx/AgregarPatologia",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                IdePatologiaSeleccionado: $("#" + "<%=hfIdPatologiaSeleccionado.ClientID %>").val().trim(),
                CheckPatologiaSeleccionado: $("#" + "<%=hfCheckPatologiaSeleccionado.ClientID %>").val().trim(),
                DatoClinico: $("#" + "<%=txtDatoClinicoPatologia.ClientID %>").val()
            }),
            dataType: "json"
        }).done(function (oOB_JSON) {
            fn_LOAD_OCUL();
            if (oOB_JSON.d.split(";")[0] == "ERROR") {
                $.JMensajePOPUP("Error", oOB_JSON.d.toString().split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()");
            } else {
                fn_LOAD_OCUL();
                //obteniendo el XML que contiene los datos de patologia y agregandolo al grid
                var xmlDoc = $.parseXML(oOB_JSON.d);
                var xml = $(xmlDoc);
                var Patologias = xml.find("TablaPatologia");
                var ContPatologia = 0;
                var row = $("[id*=gvPatologia] tr:last").clone();
                $("[id*=gvPatologia] tr:gt(0)").remove();
                $(Patologias).each(function () {
                    //var row = $("[id*=gvPatologia] tr:last").clone();                    
                    /*if ($(row).find("td").html() == "&nbsp;" || $(row).find("td").html() == "") {                        
                    $("[id*=gvPatologia] tr:last").remove();
                    }*/
                    $("td:nth-child(1)", row).html($(this).find("cod_prestacion").text());
                    $("td:nth-child(2)", row).html($(this).find("dsc_prestacion").text());
                    $("td:nth-child(3)", row).html($(this).find("dsc_muestra").text());
                    $("td:nth-child(4)", row).html($(this).find("dsc_muestra2").text());
                    $("td:nth-child(5)", row).html($(this).find("dsc_datoclinico").text());
                    $("td:nth-child(6)", row).html($(this).find("cnt_examen").text());
                    $("td:nth-child(7)", row).html($(this).find("cnt_examen2").text());
                    $("td:nth-child(8)", row).html($(this).find("ide_patologia_mae").text());
                    $("td:nth-child(9)", row).html($(this).find("cod_patologico").text());
                    $("td:nth-child(10)", row).html($(this).find("cod_presotor").text());
                    row.css("display", "table-row");
                    $("[id*=gvPatologia] tbody").append(row);
                    row = $("[id*=gvPatologia] tr:last").clone();

                    /*rowPatologia = rowPatologia.clone(true);
                    rowPatologia.find("td").eq(0).html($(this).find("cod_prestacion").text());
                    rowPatologia.find("td").eq(1).html($(this).find("dsc_prestacion").text());
                    rowPatologia.find("td").eq(2).html($(this).find("dsc_muestra").text());
                    rowPatologia.find("td").eq(3).html($(this).find("dsc_muestra2").text());
                    rowPatologia.find("td").eq(4).html($(this).find("dsc_datoclinico").text());
                    rowPatologia.find("td").eq(5).html($(this).find("cnt_examen").text());
                    rowPatologia.find("td").eq(6).html($(this).find("cnt_examen2").text());
                    rowPatologia.find("td").eq(7).html($(this).find("ide_patologia_mae").text());
                    rowPatologia.find("td").eq(8).html($(this).find("cod_patologico").text());
                    rowPatologia.find("td").eq(9).html($(this).find("cod_presotor").text());
                    gridViewPatologia.append(rowPatologia);*/
                    ContPatologia += 1;
                    if ($(Patologias).length == ContPatologia) {
                        //fn_SetValoresPatologia();
                        fn_LimpiarCheckOtrosPatologia();
                        fn_EventosGridPatologia();
                        $('#ddlOtrosPatologia')[0].selectize.disable();

                        $("#" + "<%=hfIdPatologiaSeleccionado.ClientID %>").val(""); //JB - 08/01/2020
                        $("#" + "<%=hfCheckPatologiaSeleccionado.ClientID %>").val(""); //JB - 08/01/2020
                        $('#ddlOtrosPatologia')[0].selectize.clear(); //JB - 08/01/2020
                    }

                });
            }
        });
    }

    function fn_ListarPatologiaSeleccionadas() { //JB - ACTUALMENTE NO SE USA PERO PODRIA SERVIR POSTETIORMENTE - 09/01/2020
        $.ajax({
            url: "InformacionPaciente.aspx/ListarPatologiasSeleccionadas1",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).done(function (oOB_JSON) {
            fn_LOAD_OCUL();
            if (oOB_JSON.d.split(";")[0] == "ERROR") {
                $.JMensajePOPUP("Error", oOB_JSON.d.toString().split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()");
            } else {
                fn_LOAD_OCUL();
                var xmlDoc = $.parseXML(oOB_JSON.d);
                var xml = $(xmlDoc);
                var Patologias = xml.find("TablaPatologia");
                var ContPatologia = 0;
                var row = $("[id*=gvPatologia] tr:last").clone();
                $("[id*=gvPatologia] tr:gt(0)").remove();
                $(Patologias).each(function () {
                    $("td:nth-child(1)", row).html($(this).find("cod_prestacion").text());
                    $("td:nth-child(2)", row).html($(this).find("dsc_prestacion").text());
                    $("td:nth-child(3)", row).html($(this).find("dsc_muestra").text());
                    $("td:nth-child(4)", row).html($(this).find("dsc_muestra2").text());
                    $("td:nth-child(5)", row).html($(this).find("dsc_datoclinico").text());
                    $("td:nth-child(6)", row).html($(this).find("cnt_examen").text());
                    $("td:nth-child(7)", row).html($(this).find("cnt_examen2").text());
                    $("td:nth-child(8)", row).html($(this).find("ide_patologia_mae").text());
                    $("td:nth-child(9)", row).html($(this).find("cod_patologico").text());
                    $("td:nth-child(10)", row).html($(this).find("cod_presotor").text());
                    if ($(this).find("cod_prestacion").text().trim() != "") {
                        row.css("display", "table-row");
                    } else {
                        row.css("display", "none");
                    }

                    $("[id*=gvPatologia] tbody").append(row);
                    row = $("[id*=gvPatologia] tr:last").clone();
                    ContPatologia += 1;
                    if ($(Patologias).length == ContPatologia) {
                        fn_LimpiarCheckOtrosPatologia();
                        fn_EventosGridPatologia();
                    }
                });
            }
        });
    }

    function fn_VALI_DIAG() {
        fn_LOAD_VISI();
        if ($("#divGridDiagnosticos").find(".JSBTABLA tr:not(:first)").length == 0 || $("#divGridDiagnosticos").find(".JSBTABLA tr:not(:first)").length == -1) {
            $.JMensajePOPUP("Aviso", "Debe ingresar primero un Diagnóstico.", "", "Cerrar", "fn_oculta_mensaje()");
            fn_LOAD_OCUL();
            return false;
        }
        if ($("#divGridDiagnosticos").find(".JSBTABLA tr:not(:first)").length > 0) {
            if ($("#divGridDiagnosticos").find(".JSBTABLA tr:not(:first)").find("td").length <= 1) {
                $.JMensajePOPUP("Aviso", "Debe ingresar primero un Diagnóstico.", "", "Cerrar", "fn_oculta_mensaje()");
                fn_LOAD_OCUL();
                return false;
            }
        }
        $.ajax({
            url: "InformacionPaciente.aspx/EnviarPatologia",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                DatosClinico: $("#" + "<%=txtDatoClinicoPatologia.ClientID %>").val(),
                FechaUltimaRegla: $("#" + "<%=TxtFechaUltimaRegla.ClientID %>").val().trim()
            }),
            dataType: "json"
        }).done(function (oOB_JSON) {
            if (oOB_JSON.d.split(";")[0] == "ERROR") {
                $.JMensajePOPUP("Error", oOB_JSON.d.toString().split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()");
                fn_LOAD_OCUL();
            } else {
                fn_LimpiarGridPatologia();
                $("#" + "<%=hfCheckPatologiaSeleccionado.ClientID %>").val("");
                $("#" + "<%=hfIdPatologiaSeleccionado.ClientID %>").val("");
                fn_LimpiarCheckOtrosPatologia();
                $('#ddlOtrosPatologia')[0].selectize.disable();

                $.ajax({
                    url: "InformacionPaciente.aspx/CargaPatologias",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json"
                }).done(function (oOB_JSON) {
                    fn_LOAD_OCUL();
                    if (oOB_JSON.d.split(";")[0] == "ERROR") {
                        $.JMensajePOPUP("Error", oOB_JSON.d.toString().split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()");
                        fn_LOAD_OCUL();
                    } else {
                        $("#" + "<%=txtDatoClinicoPatologia.ClientID %>").val("");
                        $("#divTreePatologia").html("");
                        $("#divTreePatologia").html(oOB_JSON.d);
                        fn_RenderizaTreeView2("divTreePatologia", true);
                        fn_LOAD_OCUL();
                    }
                });

            }
        });
    }

    function fn_CargarPatologias() {
        $.ajax({
            url: "InformacionPaciente.aspx/CargaPatologias",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).done(function (oOB_JSON) {
            fn_LOAD_OCUL();
            if (oOB_JSON.d.split(";")[0] == "ERROR") {
                $.JMensajePOPUP("Error", oOB_JSON.d.toString().split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()");
            } else {
                $("#divTreePatologia").html("");
                $("#divTreePatologia").html(oOB_JSON.d);
                fn_RenderizaTreeView2("divTreePatologia", true);                
            }
        });
    }


    function fn_MESG_POPU(Mensaje) {
        $.JMensajePOPUP("Aviso", Mensaje, "", "Cerrar", "fn_oculta_mensaje()");
    }

    function fn_AceptaMenjeNuevoCC() {
        fn_oculta_mensaje_rapido();
        if ($("#TabPrincipalT1").prop("checked")) {
            var IdVia = "<%= ddlVia_Con.ClientID %>";  //"<= ddlVia_Con.ClientID %>";  JB - - 14/07/2020 - COMENTADO  // JB - 27/04/2021 - txtVia_ControlClinico
            $("#" + "<%= ddlVia_Con.ClientID %>").val($("#" + "<%= ddlVia_Con.ClientID %>" + " option:first").val()); // JB - 14/07/2020 - COMENTADO //JB - 27/04/2021 - DESCOMENTADO
            fn_HabilitaControles(IdVia + ";txtProducto_Con;txtDosis_Con;txtCadaHora_Con;txtIndicacionProductoMedicamento;imgBusquedaProducto;imgAgregar_Con;chkBuscarDci;txtCantidad_Con;chkPRNControlClinico");
        }
        if ($("#TabPrincipalT2").prop("checked")) {
            fn_HabilitaControles("txtNutricionNoFarmacologico;txtTerapiaFisRehaNoFarmacologico;txtCuidadosEnfermeriaNoFarmacologico;txtOtrosNoFarmacologico");
        }
        if ($("#TabPrincipalT3").prop("checked")) {
            fn_HabilitaControles("txtInfusionControlClinico");
        }
    }

    function fn_AceptaMensajeCopiarCC() {
        fn_oculta_mensaje_rapido();
        //var IdReceta = $("#divControlClinicoIndicacionMed").find(".JTREE2-SELECCIONADO").next().val();  SE REEMPLAZA POR LAS LINEAS DEBAJO
        //var TipoControlClinico = $("#divControlClinicoIndicacionMed").find(".JTREE2-SELECCIONADO").next().next().val();  SE REEMPLAZA POR LAS LINEAS DEBAJO
        var IdReceta = $("#divControlClinicoIndicacionMed2").find(".JTREE3-SELECCIONADO > .IdeRecetaCab").val();
        var TipoControlClinico = $("#divControlClinicoIndicacionMed2").find(".JTREE3-SELECCIONADO > .TipoRecetaCC").val();
        fn_GuardaLog("CONTROL CLINICO", "Se copio la receta nro " + IdReceta);

        if (TipoControlClinico == "F") {
            fn_LOAD_VISI();
            $.ajax({
                url: "InformacionPaciente.aspx/ValidarProductoAgregado",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    IdeReceta: IdReceta
                }),
                dataType: "json",
                error: function (dato1, datos2, dato3) {

                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "OK") {
                    $("#divGridProductoMedicamento").load("GridViewAjax/GridProductoMedicamento.aspx", {
                        Pagina: "1"
                    }, function () {
                        //fn_LOAD_GRID_OCUL();
                    });
                    $("#TabPrincipalT1").trigger("click");
                    fn_LOAD_OCUL();
                } else {
                    $.JMensajePOPUP("Aviso", oOB_JSON.d, "1", "Cerrar", "fn_oculta_mensaje()");
                    $("#divGridProductoMedicamento").load("GridViewAjax/GridProductoMedicamento.aspx", {
                        Pagina: "1"
                    }, function () {
                        //fn_LOAD_GRID_OCUL();
                    });
                    $("#TabPrincipalT1").trigger("click");
                    fn_LOAD_OCUL();
                }
            });
        }
        if (TipoControlClinico == "N") {
            fn_LOAD_VISI();
            $.ajax({
                url: "InformacionPaciente.aspx/ValidarNoFarmacologico",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    IdeReceta: IdReceta
                }),
                dataType: "json",
                error: function (dato1, datos2, dato3) {

                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d.split(";")["0"] != "ERROR") {
                    var aValores = new Array();
                    aValores = oOB_JSON.d.split(";")
                    for (var i = 0; i < oOB_JSON.d.split("*").length; i++) {
                        if (oOB_JSON.d.split("*")[i] != "") {
                            if (oOB_JSON.d.split("*")[i].split(";")[0] == "NUTRICION") {
                                $("#txtNutricionNoFarmacologico").val(oOB_JSON.d.split("*")[i].split(";")[1]);
                            }
                            if (oOB_JSON.d.split("*")[i].split(";")[0] == "TERAPIA") {
                                $("#txtTerapiaFisRehaNoFarmacologico").val(oOB_JSON.d.split("*")[i].split(";")[1]);
                            }
                            if (oOB_JSON.d.split("*")[i].split(";")[0] == "CUIDADOS") {
                                $("#txtCuidadosEnfermeriaNoFarmacologico").val(oOB_JSON.d.split("*")[i].split(";")[1]);
                            }
                            if (oOB_JSON.d.split("*")[i].split(";")[0] == "OTROS") {
                                $("#txtOtrosNoFarmacologico").val(oOB_JSON.d.split("*")[i].split(";")[1]);
                            }
                        }
                    }
                    fn_HabilitaControles("txtNutricionNoFarmacologico;txtTerapiaFisRehaNoFarmacologico;txtCuidadosEnfermeriaNoFarmacologico;txtOtrosNoFarmacologico");
                    $("#TabPrincipalT2").trigger("click");
                    fn_LOAD_OCUL();
                } else {
                    //$.JMensajePOPUP("Aviso", "El Producto ya se encuentra agregado", "1", "Cerrar", "fn_oculta_mensaje()");
                }
            });
        }
        if (TipoControlClinico == "I") {
            fn_LOAD_VISI();
            $.ajax({
                url: "InformacionPaciente.aspx/ValidarInfusionAgregado",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    IdeReceta: IdReceta
                }),
                dataType: "json",
                error: function (dato1, datos2, dato3) {

                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d != "ERROR") {
                    fn_LimpiarInfusion();
                    for (var i = 0; i < oOB_JSON.d.split(";").length; i++) {
                        if (oOB_JSON.d.split(";")[i] != "") {
                            rowInfusiones = rowInfusiones.clone(true);
                            //agregando valores a las celdas
                            rowInfusiones.find("td").eq(0).html(gridViewInfusiones.find("tr").length);
                            rowInfusiones.find("td").eq(1).html(oOB_JSON.d.split(";")[i]);
                            //agregando la fila al gridview
                            gridViewInfusiones.append(rowInfusiones);
                            fn_RefrescaEventosGridInfusiones();
                        }
                    }
                    fn_HabilitaControles("txtInfusionControlClinico");
                    $("#TabPrincipalT3").trigger("click");

                    fn_LOAD_OCUL();
                } else {
                    //$.JMensajePOPUP("Aviso", "El Producto ya se encuentra agregado", "1", "Cerrar", "fn_oculta_mensaje()");
                }
            });
        }
    }

    function fn_RefrescaEventosGridInfusiones() {
        gridViewInfusiones = $("#" + "<%= gvListadoInfusiones.ClientID %>");
        rowInfusiones = gridViewInfusiones.find("tr").eq(1);
        if ($.trim(rowInfusiones.find("td").eq(0).html()) == "" || $.trim(rowInfusiones.find("td").eq(0).html()) == "&nbsp;") {
            gridViewInfusiones.find("tr").eq(1).remove();
        }                
        $("#" + "<%=gvListadoInfusiones.ClientID %>").find(".JIMG-GENERAL").unbind("click");
        $("#" + "<%=gvListadoInfusiones.ClientID %>").find(".JIMG-GENERAL").click(function () {
            //btnRefrescarGridview_Click
            var codigo = $(this).parent().parent().find("td").eq(0).html().trim();
            var dscitem = $(this).parent().parent().find("td").eq(1).html().trim();
            $(this).parent().parent().remove();            
            $.ajax({
                url: "InformacionPaciente.aspx/EliminarInfusiones",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    CodigoItem: dscitem
                }),
                error: function (dato1, datos2, dato3) {

                }
            }).done(function (oOB_JSON) {
                $("#" + "<%= gvListadoInfusiones.ClientID %>").find("tr").each(function (e) {
                    var ObjetoInfusion = $(this);
                    if (ObjetoInfusion.find("th").length > 0) {
                        
                    } else {
                        ObjetoInfusion.find("td").eq(0).html(e)
                    }
                });
            });
        });


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
                $("#imgNuevoControlClinico").unbind("click");
                $("#btnCopiarCC").unbind("click");
                $("#btnGuardarEvolucionClinica").unbind("click");
                $("#btnGuardarNotaIngreso").unbind("click");
                $("#imgAgregarDiagnostico").unbind("click");
                $("#imgAgregarLaboratorio").unbind("click");
                $("#imgEnviarSolicitudAnalisisLaboratorio").unbind("click");
                $("#imgPetirorioLaboratorio").unbind("click");
                $("#imgAgregarImagen").unbind("click");
                $("#imgEnviarSolicitudImagen").unbind("click");
                $("#imgPetitorioImagen").unbind("click");
                $("#btnGuardarInterconsulta").unbind("click");
                $("#imgPastillaA").unbind("click");
                $("#imgReceta").unbind("click");
                $("#imgAltaMedica").unbind("click");

                $("#btnGuardarInterconsulta").unbind("click"); 
                $("#btnAgregarPatologiax").attr("disabled", "disabled"); //18/06/2020

                $("#imgNuevoControlClinico,#btnCopiarCC,#btnGuardarEvolucionClinica,#btnGuardarNotaIngreso,#imgAgregarDiagnostico,#imgAgregarLaboratorio,#imgEnviarSolicitudAnalisisLaboratorio,#imgPetirorioLaboratorio,#imgAgregarImagen,#imgEnviarSolicitudImagen,#imgPetitorioImagen,#btnGuardarInterconsulta,#imgPastillaA,#imgReceta,#imgAltaMedica").click(function () {
                    $.JMensajePOPUP("Mensaje del Sistema Clinica San Felipe", "El paciente se encuentra de alta.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()");
                });
            }
        });
    }


    function fn_CargaPermiso() {
        //JB - NUEVO CODIGO - SE REEMPLAZA POR EL CODIGO LINEAS ABAJO - 13/05/2020
        if ($("#" + "<%=hfAdministrativo.ClientID %>").val() == "ADMINISTRATIVOS") {
            $(":input").attr("disabled", "disabled");

            $("img").each(function () {
                var objeto = $(this);
                if (objeto.attr("id") != "imgImprimir" && objeto.attr("id") != "imgHome" && objeto.attr("id") != "imgRiskCalculator" && objeto.attr("id") != "imgGuiasClinicas" && objeto.attr("id") != "imgSinadef") { //
                    objeto.attr("disabled", "disabled");
                    objeto.css("opacity", "0.5");
                    objeto.unbind("click");
                }
            });

            $("#btnVerInformeAnalisisLaboratorio").removeAttr("disabled");
            $("#btnVerInformeImagen").removeAttr("disabled");
            $("#btnVerImagen").removeAttr("disabled");
            $("#imgRoeLaboratorio").removeAttr("disabled");
            $("#imgRoeLaboratorioB").removeAttr("disabled");
            $("[id*=tabescalaeintervenciones]").removeAttr("disabled");//1.0
            
            $("#divPopUpFechaReceta").find(":input").removeAttr("disabled");
            $("#divPopUpProcedimientoConsentimiento").find(":input").removeAttr("disabled");            
        }
        if ($("#" + "<%=hfAdministrativo.ClientID %>").val() == "DIRECCION MEDICA") { //JB - 18/06/2020 - SE HABILITARA SIEMPRE PARA DIRECCION MEDICA
            $("[id*=divContenedorDinamico]").find(":input").removeAttr("disabled");

        }

        

<%--        /*JB - COMENTADO - 13/05/2020
        $.ajax({
        url: "InformacionPaciente.aspx/CargaPermiso",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
        IdeModulo: $("#" + "<%=hfCodigoFormulario.ClientID %>").val()
        }),
        dataType: "json",
        error: function (dato1, datos2, dato3) {
        }
        }).done(function (oOB_JSON) {
        if (oOB_JSON.d.split(";")[0] == "ERROR") {
        $.JMensajePOPUP("Aviso", oOB_JSON.d.split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()");
        } else {
        var aControlesPermiso = oOB_JSON.d.split(";");

        $("img").each(function () {
        var bExiste = false;
        var objeto = $(this);
        for (var i = 0; i < aControlesPermiso.length; i++) {
        if (objeto.attr("name") == aControlesPermiso[i]) {
        bExiste = true;
        }
        }
        if (objeto.attr("name") != undefined) { //preguntando si tiene atributo name
        if (bExiste == false && objeto.attr("name").toString().trim() != "") {
        objeto.attr("disabled", "disabled");
        objeto.css("opacity", "0.5");//TMACASSI 26/10/2016
        objeto.unbind("click");
        }
        }
        });
        $("input[type='button']").each(function () {
        var bExiste = false;
        var objeto = $(this);
        for (var i = 0; i < aControlesPermiso.length; i++) {
        if (objeto.attr("name") == aControlesPermiso[i]) {
        bExiste = true;
        }
        }
        if (objeto.attr("name") != undefined) { //preguntando si tiene atributo name
        if (bExiste == false && objeto.attr("name").toString().trim() != "") {
        objeto.attr("disabled", "disabled");
        //objeto.css("opacity", "0.5"); //TMACASSI 26/10/2016
        objeto.unbind("click");
        }
        }
        });
        }
        });*/--%>
    }

    function fn_CreaEventoGridInterconsulta() {
        $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
            if (oOB_JSON.d == "") {
                //EVENTO CLICK EN LA IMAGEN DE LA LISTA DE INTERCONSULTA
                $("#frmGridInterconsulta").find(".JIMG-ESTADO").click(function () {
                    var objeto = $(this);
                    var estado = objeto.parent().parent().find("td").eq(9).html().trim();
                    if (estado != "P") {
                        return;
                    }
                    var CodigoEsp = objeto.parent().parent().find("td").eq(8).html().trim();
                    $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                        if (oOB_JSON.d == "") {
                            //AQUI EL CODIGO SI LA SESSION AUN NO EXPIRA
                            $.ajax({
                                url: "InformacionPaciente.aspx/ValidaEspecialidadInterconsulta",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: JSON.stringify({
                                    CodigoEspecialidad: CodigoEsp
                                }),
                                error: function (dato1, datos2, dato3) {
                                }
                            }).done(function (oOB_JSON) {
                                if (oOB_JSON.d != "") {
                                    if (oOB_JSON.d.toString().split(";").length > 1) {
                                        $.JMensajePOPUP("Error", oOB_JSON.d.toString().split(";")[2], "ERROR", "Cerrar", "fn_oculta_mensaje()");
                                    } else {
                                        $.JMensajePOPUP("Aviso", oOB_JSON.d, "", "Cerrar", "fn_oculta_mensaje()");
                                    }
                                } else {
                                    var IdInterconsultaR = objeto.parent().parent().find("td").eq(10).html().trim();
                                    var IdMotivoR = objeto.parent().parent().find("td").eq(11).html().trim();
                                    var CodEspecialidadR = objeto.parent().parent().find("td").eq(8).html().trim();
                                    var DescripcionSolicitudR = objeto.parent().parent().find("td").eq(12).html().trim();
                                    var DescripcionEspecialidadR = objeto.parent().parent().find("td").eq(5).html().trim();
                                    var NombreMedicoR = objeto.parent().parent().find("td").eq(13).html().trim().replace("&nbsp;", "");   //3  
                                    //Observaciones Cmendez 02/05/2022
                                    var aValores = [IdInterconsultaR + "|", IdMotivoR + "|", CodEspecialidadR + "|", DescripcionSolicitudR + "|", DescripcionEspecialidadR + "|", NombreMedicoR]
                                    fn_GuardaLog("INTERCONSULTA", "Accedio a la opcion de respuesta de la interconsulta nro " + IdInterconsultaR);
                                    $.JPopUp("Respuesta Interconsulta", "PopUp/InterconsultaRespuesta.aspx", "1", "Cerrar", "fn_CierraPopupInterconsulta()", 80, "", aValores);

                                }
                            });


                        } else {
                            aValores = oOB_JSON.d.toString().split(";");
                            $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                        }
                    });
                });
                fn_CargaPermiso();
            } else {
                aValores = oOB_JSON.d.toString().split(";");
                $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
            }
        });



        
    }


    function fn_CrearEventoMedico() {
        $(".JSBTABLA-1 tr td").click(function () {
            if ($(this).html().trim() != "") {
                $("#spCodigoMedicoSeleccionado").html($(this).parent().find("td").eq(0).html().trim());
                $("#txtMedicoInterconsulta").val($(this).parent().find("td").eq(1).html().trim());

                $("#divFONDO").css("display", "none");
                $("#divBusquedaMedico").css("display", "none");
            }
        });
    }


    function fn_CrearEventoEspecialidad() {
        $(".JSBTABLA-1 tr td").click(function () {
            if ($(this).html().trim() != "") {
                $("#spCodigoEspecialidadSeleccionado").html($(this).parent().find("td").eq(0).html().trim());
                $("#txtEspecialidadInterconsulta").val($(this).parent().find("td").eq(1).html().trim());

                $("#divFONDO").css("display", "none");
                $("#divBusquedaEspecialidad").css("display", "none");
            }
        });
    }

    function fn_CrearEventoProcedimientoMedico(TipoBusqueda) {
        $(".JSBTABLA-1 tr td").click(function () {
            if ($(this).html().trim() != "") {
                $("#spCodigoProcedimientoSeleccionado").html($(this).parent().find("td").eq(0).html().trim());
                $("#txtProcedimientoMedico").val($(this).parent().find("td").eq(0).html().trim()); //eq(1) -> descripcion

                $("#divFONDO").css("display", "none");
                $("#divBusquedaProcedimientoMedico").css("display", "none");

                if (TipoBusqueda == "F") {
                    $("#chkFavoritoProcedimientoMedico").prop("checked", true);

                    $.ajax({
                        url: "InformacionPaciente.aspx/ConsultaCpt",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({
                            CodCpt: $("#spCodigoProcedimientoSeleccionado").html()
                        }),
                        error: function (dato1, datos2, dato3) {

                        }
                    }).done(function (oOB_JSON) {
                        if (oOB_JSON.d != "") {
                            var xmlDoc = $.parseXML(oOB_JSON.d);
                            var xml = $(xmlDoc);
                            var ProcedimientoMedicos = xml.find("TablaProcedimientoMedicoConsulta");

                            $(ProcedimientoMedicos).each(function () {
                                $("#ddlSeccionProcedimientoMedico").data("codigo", $(this).find("cod_seccion").text());
                                $("#ddlSeccionProcedimientoMedico").data("descripcion", $(this).find("seccion").text());

                                $("#ddlSubSeccionProcedimientoMedico").data("codigo", $(this).find("cod_subseccion").text());
                                $("#ddlSubSeccionProcedimientoMedico").data("descripcion", $(this).find("sub_seccion").text());

                                $("#ddlDescripcionCPT").data("codigo", $(this).find("cod_cpt").text());
                                $("#ddlDescripcionCPT").data("descripcion", $(this).find("dsc_cpt").text());

                                $("#ddlSeccionProcedimientoMedico").find(".JSELECT2-SELECCION").html($(this).find("seccion").text());
                                $("#ddlSubSeccionProcedimientoMedico").find(".JSELECT2-SELECCION").html(($(this).find("sub_seccion").text() == "" ? "-" : $(this).find("sub_seccion").text()));
                                $("#ddlDescripcionCPT").find(".JSELECT2-SELECCION").html(($(this).find("dsc_cpt").text() == "" ? "-" : $(this).find("dsc_cpt").text()));

                                $("#spCodigoProcedimientoSeleccionado").html($(this).find("cod_cpt").text());


                                fn_InicializarCombo();
                                $("#ddlSeccionProcedimientoMedico").find(".JSELECT2-SELECCION").unbind("click");
                                $("#ddlSubSeccionProcedimientoMedico").find(".JSELECT2-SELECCION").unbind("click");
                                $("#ddlDescripcionCPT").find(".JSELECT2-SELECCION").unbind("click");
                                //AQUI TMNB VERIFICAR SI ES FAVORITO

                                $("#ddlSeccionProcedimientoMedico").find(".JSELECT2-ELEMENT").parent().parent().css("position", "relative"); //JB - 13/05/2021
                                $("#ddlSubSeccionProcedimientoMedico").find(".JSELECT2-ELEMENT").parent().parent().css("position", "relative"); //JB - 13/05/2021
                                $("#ddlDescripcionCPT").find(".JSELECT2-ELEMENT").parent().parent().css("position", "relative"); //JB - 13/05/2021
                            });

                        }
                    });
                }
            }
        });
    }

    

    function fn_EnviarSolicitudImagen() {
        fn_oculta_mensaje_rapido();
        fn_LOAD_VISI();
        $.ajax({
            url: "InformacionPaciente.aspx/GuardarSolicitudImagenes",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                Descripcion: $("#txtObservacionImagen").val().trim()
            }),
            dataType: "json",
            error: function (dato1, datos2, dato3) {

            }
        }).done(function (oOB_JSON) {
            fn_LOAD_OCUL();
            if (oOB_JSON.d != "OK") {
                $.JMensajePOPUP("Error", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_mensaje()");
            } else {
                $("#txtObservacionImagen").val("");
                $("#divGridImagen").load("GridViewAjax/GridImagen.aspx", function () {
                });
                //fn_CrearTreeViewImagenes();
                fn_CrearTreeViewImagenes2("1", "", "0");
                //fn_CargarEvolucionClinica(); //CARGARA EVOLUCION CLINICA
                fn_CargarEvolucionClinica2("1", "0");
            }
        });
    }

    function fn_CrearTreeViewImagenes() {
        $.ajax({
            url: "InformacionPaciente.aspx/TreeViewImagenes",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (dato1, datos2, dato3) {

            }
        }).done(function (oOB_JSON) {
            //imgAgregarImagen
            if (oOB_JSON.d != "") {
                if (oOB_JSON.d.toString().split("*").length > 1) {
                    if (oOB_JSON.d.toString().split("*")[0] == "ERROR") {
                        $.JMensajePOPUP("Error", oOB_JSON.d.toString().split(";")(1), "ERROR", "Cerrar", "fn_oculta_mensaje()");
                    } else {
                        //cargar datos
                        $("#divTreeViewImagenes").html("");
                        $("#divTreeViewImagenes").append(oOB_JSON.d);
                        fn_RenderizaTreeView2("divTreeViewImagenes", true);
                    }
                } else {
                    $("#divTreeViewImagenes").html("");
                    $("#divTreeViewImagenes").append(oOB_JSON.d);
                    fn_RenderizaTreeView2("divTreeViewImagenes", true);
                    //fn_CreaEventosTreeViewAI("divTreeViewImagenes", "hfRecetaTreeViewSeleccionadoImagen");
                    //fn_CrearEventoTreeViewImagenes();                    
                    //$('#divTreeViewImagenes li.parent > a').first().trigger("click");
                    
                }
            }
        });
    }



    function fn_CrearTreeViewImagenes2(OrdenEjecutar, FechaMostrar, IdeRecetaCabMostrar, objeto) {
        $.ajax({
            url: "InformacionPaciente.aspx/TreeViewImagenes2",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({
                orden: OrdenEjecutar,
                fec_receta: FechaMostrar,
                ide_recetacab: IdeRecetaCabMostrar
            }),
            error: function (dato1, datos2, dato3) {

            }
        }).done(function (oOB_JSON) {            
            if (oOB_JSON.d != "") {
                if (OrdenEjecutar == "1") {
                    $("#divTreeViewImagenes2").html("");
                    $("#divTreeViewImagenes2").append(oOB_JSON.d);
                }
                if (OrdenEjecutar == "2") {
                    objeto.parent().find(".JTREE3-HORA").html("");
                    objeto.parent().find(".JTREE3-HORA").append(oOB_JSON.d);
                }
                if (OrdenEjecutar == "3") {
                    objeto.next().html("");
                    objeto.next().append(oOB_JSON.d);
                }

                fn_CrearEventoTreeImagenes2();
            }
        });
    }


    function fn_CrearEventoTreeImagenes2() {
        $("#divTreeViewImagenes2").find(".JFILA-FECHA").unbind("click");
        $("#divTreeViewImagenes2").find(".JFILA-FECHA").click(function () {
            var oObjeto = $(this);

            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $("#divTreeViewImagenes2").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                    $("#divTreeViewImagenes2").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                    $("#divTreeViewImagenes2").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");                    
                    oObjeto.addClass("JTREE3-SELECCIONADO");
                    oObjeto.find(" > .JTREE3-SIGNO").toggleClass("JTREE3-PLUS");
                    var CadenaClase = oObjeto.find(" > .JTREE3-SIGNO").attr("class");
                    var Fecha = oObjeto.find("> input").val();
                    if (CadenaClase.includes("JTREE3-PLUS")) {
                        fn_CrearTreeViewImagenes2("2", Fecha, "0", oObjeto);
                        oObjeto.parent().find(".JTREE3-HORA").css("display", "block");
                    } else {
                        oObjeto.parent().find(".JTREE3-HORA").css("display", "none");
                    }                    
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });
        $("#divTreeViewImagenes2").find(".JFILA-HORA").unbind("click");
        $("#divTreeViewImagenes2").find(".JFILA-HORA").click(function () {
            var oObjeto = $(this);

            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $("#divTreeViewImagenes2").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                    $("#divTreeViewImagenes2").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                    $("#divTreeViewImagenes2").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");
                    
                    oObjeto.addClass("JTREE3-SELECCIONADO");
                    oObjeto.find(" > .JTREE3-SIGNO").toggleClass("JTREE3-PLUS");
                    var CadenaClase = oObjeto.find(" > .JTREE3-SIGNO").attr("class");
                    var IdRecetaCab = oObjeto.find("> input").val();
                    if (CadenaClase.includes("JTREE3-PLUS")) {
                        fn_CrearTreeViewImagenes2("3", "", IdRecetaCab, oObjeto);
                        oObjeto.next().css("display", "block");
                    } else {
                        oObjeto.next().css("display", "none");
                    }
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });
        $("#divTreeViewImagenes2").find(".JTREE3-DETALLE").unbind("click");
        $("#divTreeViewImagenes2").find(".JTREE3-DETALLE").click(function () {
            var oObjeto = $(this);

            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $("#divTreeViewImagenes2").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                    $("#divTreeViewImagenes2").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                    $("#divTreeViewImagenes2").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");
                    
                    oObjeto.addClass("JTREE3-SELECCIONADO");
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });
    }

 
    function fn_EnviarSolicitudAnalisis() {
        fn_oculta_mensaje_rapido();
        fn_LOAD_VISI();
        var Fecha = "";
        var Hora = "";
        if ($("#chkProgramarHoraLab").prop("checked") == true) {
            Fecha = $("#txtFechaProgramarHoraLab").val();
            Hora = $("#txtHoraProgramarHoraLab").val();

            if (Fecha == "" || Hora == "") {
                $.JMensajePOPUP("Mensaje del Sistema Clinica San Felipe - Error", "Debe ingresar una Fecha y Hora", "ERROR", "Cerrar", "fn_oculta_mensaje()");
                fn_LOAD_OCUL();
                return;
            }
        }        

        //validando debe marcar un analisis marcado para programar
        if ($("#chkProgramarHoraLab").prop("checked") == true) {
            var ValidaCheckLab = false;
            $("#divGridLaboratorio").find(".JSBTABLA .HoraProg").find("input[type='checkbox']").each(function () {
                if ($(this).prop("checked")) {
                    ValidaCheckLab = true;
                }
            });
            if (ValidaCheckLab == false) {
                $.JMensajePOPUP("Mensaje del Sistema Clinica San Felipe - Error", "Debe marcar un analisis para programar la hora", "ERROR", "Cerrar", "fn_oculta_mensaje()");
                fn_LOAD_OCUL();
                return;
            }
        }
        var CheckActivo = ""
        $("#divGridLaboratorio").find(".JSBTABLA .HoraProg").find("input[type='checkbox']").each(function () {
            var objetoc = $(this);
            if ($(this).prop("checked")) {
                CheckActivo += objetoc.parent().parent().find(".ide_analisis").html() + ";";
            }
        });

        $.ajax({
            url: "InformacionPaciente.aspx/GuardarSolicitudAnalisis",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                Descripcion: $("#txtObservacionAnalisisLaboratorio").val().trim(),
                Fecha: Fecha,
                Hora: Hora,
                CodigoMarcado: CheckActivo
            }),
            dataType: "json",
            error: function (dato1, datos2, dato3) {

            }
        }).done(function (oOB_JSON) {
            fn_LOAD_OCUL();
            if (oOB_JSON.d != "OK") {
                $.JMensajePOPUP("Error", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_mensaje()");
            } else {
                $("#txtObservacionAnalisisLaboratorio").val("");
                $("#chkProgramarHoraLab").prop("checked", false);
                $("#txtFechaProgramarHoraLab").val("");
                $("#txtHoraProgramarHoraLab").val("");
                $("#txtFechaProgramarHoraLab").attr("disabled", "disabled");
                $("#txtHoraProgramarHoraLab").attr("disabled", "disabled");
                $("#divGridLaboratorio").load("GridViewAjax/GridLaboratorio.aspx", function () {
                });
                //fn_CrearTreeViewAnalisis();
                fn_CrearTreeViewAnalisis2("1", "", "0");
                //fn_CargarEvolucionClinica(); //CARGARA EVOLUCION CLINICA
                fn_CargarEvolucionClinica2("1", "0");
            }
        });
    }

    function fn_CancelarEliminarFavoritoImagenes() {
        fn_oculta_mensaje();
        $("#chkFavoritoImagen").prop("checked", true);
    }

    function fn_EliminaFavoritoImagenes() {
        fn_oculta_mensaje_rapido();
        fn_LOAD_VISI();
        $.ajax({
            url: "InformacionPaciente.aspx/EliminarFavoritoImagenes",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({
                IdeFavoritoImagen: $("#hfFavoritoImagen").val().trim()
            }),
            error: function (dato1, datos2, dato3) {

            }
        }).done(function (oOB_JSON) {
            fn_LOAD_OCUL();
            if (oOB_JSON.d != "OK") {
                $.JMensajePOPUP("Error", oOB_JSON.d, "ERROR", "Aceptar", "fn_oculta_mensaje()");
            } else {
                $("#chkFavoritoImagen").prop("checked", false);
                $("#hfFavoritoImagen").val("");
            }
        });
    }

    function fn_CancelarOrden() {
        fn_oculta_mensaje();        
        var FecReceta = $("#divTreeViewImagenes2").find(".JTREE3-SELECCIONADO > .FecRegistro").eq(0).val(); //si obtiene valor marco el nodo de fecha
        var IdeImagenDet = "";
        var PresotorSps = "";
        if (FecReceta == undefined || FecReceta == null || FecReceta == "") {
            IdeImagenDet = $("#divTreeViewImagenes2").find(".JTREE3-SELECCIONADO > .IdeImagenDet").eq(0).val(); //si tiene valor marco detalle                        
            if (IdeImagenDet == null || IdeImagenDet == undefined || IdeImagenDet == null) {                
                $.JMensajePOPUP("Aviso", "Debe seleccionar una imagen.", "", "Cerrar", "fn_oculta_mensaje()");                
                return;
            } else {
                PresotorSps = $("#divTreeViewImagenes2").find(".JTREE3-SELECCIONADO").find("> input").eq(0).val();                
            }
        } else {            
            $.JMensajePOPUP("Aviso", "Debe seleccionar una imagen.", "", "Cerrar", "fn_oculta_mensaje()");
            return;
        }


        fn_LOAD_VISI();
        $.ajax({
            url: "InformacionPaciente.aspx/AnularOrdenImagen",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({
                PresotorSps: PresotorSps,
                IdeImagenDet: IdeImagenDet
            }),
            error: function (dato1, datos2, dato3) {

            }
        }).done(function (oOB_JSON) {
            fn_LOAD_OCUL();
            if (oOB_JSON.d != "") {
                $.JMensajePOPUP("Error", oOB_JSON.d, "ERROR", "Aceptar", "fn_oculta_mensaje()");
            } else {
                
            }
        });
    }

    function fn_CrearTreeViewAnalisis() {
        $.ajax({
            url: "InformacionPaciente.aspx/TreeViewAnalisis",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (dato1, datos2, dato3) {

            }
        }).done(function (oOB_JSON) {
            if (oOB_JSON.d != "") {  
                
                if (oOB_JSON.d.toString().split("*").length > 0) {
                    if (oOB_JSON.d.toString().split("*")[0] == "ERROR") {
                        $.JMensajePOPUP("Error", oOB_JSON.d.toString().split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()");
                    } else {
                        $("#divTreeLaboratorio").html("");
                        $("#divTreeLaboratorio").append(oOB_JSON.d);
                        fn_RenderizaTreeView2("divTreeLaboratorio", true);
                    }
                } else {
                    $("#divTreeLaboratorio").html("");
                    $("#divTreeLaboratorio").append(oOB_JSON.d);
                    fn_RenderizaTreeView2("divTreeLaboratorio", true);
                    //fn_CreaEventosTreeViewAI("divTreeLaboratorio", "hfRecetaTreeViewSeleccionado"); JB - 04/09/2019 - COMENTADO
                    //fn_CrearEventoTreeViewAnalisis(); //JB - 04/09/2019 - COMENTADO                
                    //$('#divTreeLaboratorio li.parent > a').first().trigger("click"); JB - 04/09/2019 - COMENTADO                    
                }
            }
        });
    }

    function fn_CrearTreeViewAnalisis2(OrdenEjecutar, FechaMostrar, IdeRecetaCabMostrar, objeto) {
        $.ajax({
            url: "InformacionPaciente.aspx/TreeViewAnalisis2",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({
                orden: OrdenEjecutar,
                fec_receta: FechaMostrar,
                ide_recetacab: IdeRecetaCabMostrar
            }),
            error: function (dato1, datos2, dato3) {

            }
        }).done(function (oOB_JSON) {
            if (oOB_JSON.d != "") {
                if (OrdenEjecutar == "1") {
                    $("#divTreeLaboratorio2").html("");
                    $("#divTreeLaboratorio2").append(oOB_JSON.d);
                }
                if (OrdenEjecutar == "2") {
                    objeto.parent().find(".JTREE3-HORA").html("");
                    objeto.parent().find(".JTREE3-HORA").append(oOB_JSON.d);
                }
                if (OrdenEjecutar == "3") {
                    objeto.next().html("");
                    objeto.next().append(oOB_JSON.d);
                }
                
                fn_CrearEventoTreeAnalisis2();
            }
        });
    }

    function fn_CrearEventoTreeAnalisis2() {
        $("#divTreeLaboratorio2").find(".JFILA-FECHA").unbind("click");
        $("#divTreeLaboratorio2").find(".JFILA-FECHA").click(function () {
            var oObjeto = $(this);

            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $("#divTreeLaboratorio2").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                    $("#divTreeLaboratorio2").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                    $("#divTreeLaboratorio2").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");
                    
                    oObjeto.addClass("JTREE3-SELECCIONADO");
                    oObjeto.find(" > .JTREE3-SIGNO").toggleClass("JTREE3-PLUS");
                    var CadenaClase = oObjeto.find(" > .JTREE3-SIGNO").attr("class");
                    var Fecha = oObjeto.find("> input").val();
                    if (CadenaClase.includes("JTREE3-PLUS")) {
                        fn_CrearTreeViewAnalisis2("2", Fecha, "0", oObjeto);
                        oObjeto.parent().find(".JTREE3-HORA").css("display", "block");
                    } else {
                        oObjeto.parent().find(".JTREE3-HORA").css("display", "none");
                    }
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
            
        });
        $("#divTreeLaboratorio2").find(".JFILA-HORA").unbind("click");
        $("#divTreeLaboratorio2").find(".JFILA-HORA").click(function () {
            var oObjeto = $(this);

            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $("#divTreeLaboratorio2").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                    $("#divTreeLaboratorio2").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                    $("#divTreeLaboratorio2").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");
                    
                    oObjeto.addClass("JTREE3-SELECCIONADO");
                    oObjeto.find(" > .JTREE3-SIGNO").toggleClass("JTREE3-PLUS");
                    var CadenaClase = oObjeto.find(" > .JTREE3-SIGNO").attr("class");
                    var IdRecetaCab = oObjeto.find("> input").val();
                    if (CadenaClase.includes("JTREE3-PLUS")) {
                        fn_CrearTreeViewAnalisis2("3", "", IdRecetaCab, oObjeto);
                        oObjeto.next().css("display", "block");
                    } else {
                        oObjeto.next().css("display", "none");
                    }
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });
        $("#divTreeLaboratorio2").find(".JTREE3-DETALLE").unbind("click");
        $("#divTreeLaboratorio2").find(".JTREE3-DETALLE").click(function () {
            var oObjeto = $(this);
            
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $("#divTreeLaboratorio2").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                    $("#divTreeLaboratorio2").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                    $("#divTreeLaboratorio2").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");
                    
                    oObjeto.addClass("JTREE3-SELECCIONADO");  
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });
    }


    function fn_EliminaFavoritoAnalisis() {
        fn_oculta_mensaje();
        fn_LOAD_VISI();
        $.ajax({
            url: "InformacionPaciente.aspx/EliminarFavoritoAnalisis",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({
                IdeFavoritoAnalisisLab: $("#hfIdeFavoritoAnalisis").val().trim()
            }),
            error: function (dato1, datos2, dato3) {

            }
        }).done(function (oOB_JSON) {
            fn_LOAD_OCUL();
            if (oOB_JSON.d != "OK") {
                $.JMensajePOPUP("Error", oOB_JSON.d, "ERROR", "Aceptar", "fn_oculta_mensaje()");
            } else {
                $("#chkAnalisisLaboratorioFavorito").prop("checked", false);
                $("#hfIdeFavoritoAnalisis").val("");
            }
        });
    }

    function fn_CancelarEliminarFavoritoAnalisis() {
        fn_oculta_mensaje();
        $("#chkAnalisisLaboratorioFavorito").prop("checked", true);
    }

    /*function AgregarAnalisis3() {
        fn_oculta_mensaje_rapido();
        $.JMensajePOPUP("Aviso", "Aseguradora no cubre analisis seleccionado", "ADVERTENCIA", "Aceptar", "AgregarAnalisis2()");
    }*/

    function AgregarAnalisis2() {
        AgregarAnalisis(true);
    }
    function AgregarAnalisis3() {
        AgregarAnalisis(true); //JB - 31/07/2020 - SE LE ASIGNA VALOR 1/TRUE ANTES ENVIABA 'false'
    }

    function AgregarAnalisis(pFlgCubierto) {
        fn_oculta_mensaje_rapido();
        var bFlgFavoritoLaboratorio = false;
        if ($("#chkAnalisisLaboratorioFavorito").prop("checked") == true && ($("#hfIdeFavoritoAnalisis").val().trim() == "" || $("#hfIdeFavoritoAnalisis").val().trim() == "0")) {
            bFlgFavoritoLaboratorio = true;
        }

        var bFlgCubierto = false;
        if (pFlgCubierto != undefined && pFlgCubierto != null) {
            bFlgCubierto = pFlgCubierto;
        }
        
        fn_LOAD_VISI();
        $.ajax({
            url: "InformacionPaciente.aspx/AgregarAnalisis",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                DescripcionReceta: $("#txtObservacionAnalisisLaboratorio").val().trim(),
                IdAnalisis: $("#spCodigoAnalisisLaboratorioSeleccionado").html().trim(),
                FlgCubierto: bFlgCubierto,
                flgFavoritoLaboratorio: bFlgFavoritoLaboratorio,
                Perfil: $("#spPerfil").html().trim()
            }),
            dataType: "json",
            error: function (dato1, datos2, dato3) {

            }
        }).done(function (oOB_JSON) {            
            fn_LOAD_OCUL();
            var aValoresSesion;
            aValoresSesion = oOB_JSON.d.toString().split(";");
            if (aValoresSesion[0] == "EXPIRO") {
                window.location.href = aValoresSesion[1];
            }
            if (oOB_JSON.d.split(";")[0] != "OK") {
                if (oOB_JSON.d.split(";")[0] == "1") {
                    $.JMensajePOPUP("Error", oOB_JSON.d.split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()");
                } else {
                    $.JMensajePOPUP("Aviso", oOB_JSON.d.split(";")[1], "1", "Cerrar", "fn_oculta_mensaje()");
                }
            } else {
                /*$("#divGridLaboratorio").load("GridViewAjax/GridLaboratorio.aspx", function () {
                });*/
            }
            $("#txtAnalisisLaboratorio").val("");
            $("#spCodigoAnalisisLaboratorioSeleccionado").html("");
            $("#chkAnalisisLaboratorioSeleccionado").prop("checked", false);
            $("#spAnalisisLaboratorioSeleccionado").html("");
            $("#chkAnalisisLaboratorioFavorito").prop("checked", false);
            $("#hfIdeFavoritoAnalisis").val("");
            $("#divGridLaboratorio").load("GridViewAjax/GridLaboratorio.aspx", function () {
            });
            $("#spPerfil").html("");
        });
    }



    function fn_EliminaFavoritoDiagnostico() {
        fn_oculta_mensaje();
        fn_LOAD_VISI();
        $.ajax({
            url: "InformacionPaciente.aspx/EliminarDiagnosticoFavorito",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({
                IdeFavoritoDiagnostico: $("#hfFavoritoDiagnostico").val().trim()
            }),
            error: function (dato1, datos2, dato3) {

            }
        }).done(function (oOB_JSON) {
            fn_LOAD_OCUL();
            if (oOB_JSON.d == "") {
                $.JMensajePOPUP("Error", "No se pudo eliminar de favoritos", "ERROR", "Aceptar", "fn_oculta_mensaje()");
            } else {
                $("#chkFavoritoDiagnostico").prop("checked", false);
                $("#hfFavoritoDiagnostico").val("");
            }            
        });
    }

    function fn_CancelarEliminaFavoritoDiagnostico() {
        fn_oculta_mensaje();
        $("#chkFavoritoDiagnostico").prop("checked", true);
    }


    function fn_CrearTreeViewProcedimientoMedico(OrdenEjecutar, FechaMostrar, IdeOrdenMostrar, objeto) {
        $.ajax({
            url: "InformacionPaciente.aspx/TreeViewProcedimientoMedico",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({
                orden: OrdenEjecutar,
                fec_orden: FechaMostrar,
                ide_orden: IdeOrdenMostrar
            }),
            error: function (dato1, datos2, dato3) {

            }
        }).done(function (oOB_JSON) {
            if (oOB_JSON.d != "") {
                if (OrdenEjecutar == "1") {
                    $("#divTreeProcedimientoMedicos").html("");
                    $("#divTreeProcedimientoMedicos").append(oOB_JSON.d);
                }
                if (OrdenEjecutar == "2") {
                    objeto.parent().find(".JTREE3-HORA").html("");
                    objeto.parent().find(".JTREE3-HORA").append(oOB_JSON.d);
                }
                if (OrdenEjecutar == "3") {
                    objeto.next().html("");
                    objeto.next().append(oOB_JSON.d);
                }

                fn_CrearEventoTreeProcedimientoMedico();
            }
        });
    }


    function fn_CrearEventoTreeProcedimientoMedico() {
        $("#divTreeProcedimientoMedicos").find(".JFILA-FECHA").unbind("click");
        $("#divTreeProcedimientoMedicos").find(".JFILA-FECHA").click(function () {
            var oObjeto = $(this);

            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $("#divTreeProcedimientoMedicos").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                    $("#divTreeProcedimientoMedicos").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                    $("#divTreeProcedimientoMedicos").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");
                    oObjeto.addClass("JTREE3-SELECCIONADO");
                    oObjeto.find(" > .JTREE3-SIGNO").toggleClass("JTREE3-PLUS");
                    var CadenaClase = oObjeto.find(" > .JTREE3-SIGNO").attr("class");
                    var Fecha = oObjeto.find("> input").val();
                    if (CadenaClase.includes("JTREE3-PLUS")) {
                        fn_CrearTreeViewProcedimientoMedico("2", Fecha, "0", oObjeto);
                        oObjeto.parent().find(".JTREE3-HORA").css("display", "block");
                    } else {
                        oObjeto.parent().find(".JTREE3-HORA").css("display", "none");
                    }
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });
        $("#divTreeProcedimientoMedicos").find(".JFILA-HORA").unbind("click");
        $("#divTreeProcedimientoMedicos").find(".JFILA-HORA").click(function () {
            var oObjeto = $(this);

            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $("#divTreeProcedimientoMedicos").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                    $("#divTreeProcedimientoMedicos").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                    $("#divTreeProcedimientoMedicos").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");

                    oObjeto.addClass("JTREE3-SELECCIONADO");
                    oObjeto.find(" > .JTREE3-SIGNO").toggleClass("JTREE3-PLUS");
                    var CadenaClase = oObjeto.find(" > .JTREE3-SIGNO").attr("class");
                    var IdRecetaCab = oObjeto.find("> input").val();
                    if (CadenaClase.includes("JTREE3-PLUS")) {
                        fn_CrearTreeViewProcedimientoMedico("3", "", IdRecetaCab, oObjeto);
                        oObjeto.next().css("display", "block");
                    } else {
                        oObjeto.next().css("display", "none");
                    }
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });
        $("#divTreeProcedimientoMedicos").find(".JTREE3-DETALLE").unbind("click");
        $("#divTreeProcedimientoMedicos").find(".JTREE3-DETALLE").click(function () {
            var oObjeto = $(this);

            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $("#divTreeProcedimientoMedicos").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                    $("#divTreeProcedimientoMedicos").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                    $("#divTreeProcedimientoMedicos").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");

                    oObjeto.addClass("JTREE3-SELECCIONADO");
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });
    }


    function fn_CrearEventoDiagnostico() { //CREA EVENTO CLICK EN LA BUSQUEDA DE DIAGNOSTICO PARA SELECCIONARLO
        $(".JSBTABLA-1 tr td").click(function () {
            if ($(this).html().trim() != "") {
                $("#txtDiagnostico").val("");
                $("#spCodigoDiagnosticoSeleccionado").html("");
                $("#spDiagnosticoSeleccionado").html("");                
                $("#chkDiagnosticoSeleccionado").prop("checked", false);
                $("#chkPresuntivoDiagnostico").prop("checked", false);
                $("#chkRepetidoDiagnostico").prop("checked", false);
                $("#chkDefinitivoDiagnostico").prop("checked", false);
                $("#chkFavoritoDiagnostico").prop("checked", false);

                if ($(this).parent().find("td").eq(0).html().trim().length == 3) {
                    $.JMensajePOPUP("Aviso", "No puede seleccionar CIE de 3 dígitos.", "", "Cerrar", "fn_oculta_mensaje()");
                    return false;
                }

                if ($("#divBusquedaDiagnostico").find("#hfTipoBusqueda").val().trim() == "G") {
                    $("#spCodigoDiagnosticoSeleccionado").html($(this).parent().find("td").eq(0).html().trim());
                    $("#chkDiagnosticoSeleccionado").prop("checked", true);
                    $("#spDiagnosticoSeleccionado").html($(this).parent().find("td").eq(1).html().trim());

                    if ($(this).parent().find("td").eq(2).html().trim() == 1) {
                        $("#chkFavoritoDiagnostico").prop("checked", true);
                    } else {
                        $("#chkFavoritoDiagnostico").prop("checked", false);
                    }
                    $("#hfFavoritoDiagnostico").val($(this).parent().find("td").eq(3).html().trim());

                    $("#divFONDO").css("display", "none");
                    $("#divBusquedaDiagnostico").css("display", "none");
                } else {
                    $("#spCodigoDiagnosticoSeleccionado").html($(this).parent().find("td").eq(0).html().trim());
                    $("#chkDiagnosticoSeleccionado").prop("checked", true);
                    $("#spDiagnosticoSeleccionado").html($(this).parent().find("td").eq(1).html().trim());

                    $("#chkFavoritoDiagnostico").prop("checked", true);
                    $("#hfFavoritoDiagnostico").val($(this).parent().find("td").eq(2).html().trim());
                    $("#divFONDO").css("display", "none");
                    $("#divBusquedaDiagnostico").css("display", "none");
                }

            }
        });
    }


    function fn_CrearEventoControlClinico() {
        $(".JSBTABLA-1 tr td").click(function () {
            if ($(this).html().trim() != "") {
                var objeto = $(this);

                //VALIDAR SI ES ALERGICO AL PRODUCTO
                $.ajax({
                    url: "InformacionPaciente.aspx/ValidaAlergiaPaciente",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                        CodigoProducto: objeto.parent().find("td").eq(0).html().trim()
                    }),
                    dataType: "json",
                    error: function (dato1, datos2, dato3) {
                    }
                }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "SI") {
                        $.JMensajePOPUP("Mensaje del Sistema Clinica San Felipe - Aviso", "El paciente presenta alergia a este producto.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()");
                        $("#txtProducto_Con").val("");
                        $("#spCodigoProductoSeleccionado").html("");
                        $("#divFONDO").css("display", "none");
                        $("#divBusquedaMedicamentoCC").css("display", "none");
                    } else {
                        if (oOB_JSON.d != "") {
                            $.JMensajePOPUP("Mensaje del Sistema Clinica San Felipe - Error", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_mensaje()");
                        } else {
                            $("#txtProducto_Con").val("");
                            $("#spCodigoProductoSeleccionado").html("");
                            $("#spCodigoProductoSeleccionado").html(objeto.parent().find("td").eq(0).html().trim());
                            $("#txtProducto_Con").val(objeto.parent().find("td").eq(1).html().trim());

                            $("#divFONDO").css("display", "none");
                            $("#divBusquedaMedicamentoCC").css("display", "none");
                        }
                    }
                });

            }
        });
    }

    function fn_CrearEventoImagenes() {
        $(".JSBTABLA-1 tr td").click(function () {
            if ($(this).html().trim() != "") {
                if ($("#divBusquedaImagen").find("#hfTipoBusqueda").val().trim() == "G") {
                    $("#spCodigoImagenSeleccionado").html("");
                    $("#chkImagenSeleccionado").prop("checked", false);
                    $("#spImagenSeleccionado").html("");
                    $("#chkFavoritoImagen").prop("checked", false);
                    $("#hfFavoritoImagen").val("");

                    $("#spCodigoImagenSeleccionado").html($(this).parent().find("td").eq(0).html().trim());
                    $("#chkImagenSeleccionado").prop("checked", true);

                    $("#spImagenSeleccionado").html($(this).parent().find("td").eq(1).html().trim());

                    if ($(this).parent().find("td").eq(2).html().trim() != 0) {
                        $("#chkFavoritoImagen").prop("checked", true);
                    }
                    $("#hfFavoritoImagen").val($(this).parent().find("td").eq(3).html().trim());
                } else {
                    $("#spCodigoImagenSeleccionado").html("");
                    $("#chkImagenSeleccionado").prop("checked", false);
                    $("#spImagenSeleccionado").html("");
                    $("#chkFavoritoImagen").prop("checked", false);
                    $("#hfFavoritoImagen").val("");

                    //cargando datos de la busqueda seleccionada
                    $("#spCodigoImagenSeleccionado").html($(this).parent().find("td").eq(0).html().trim());
                    $("#chkImagenSeleccionado").prop("checked", true);
                    $("#spImagenSeleccionado").html($(this).parent().find("td").eq(1).html().trim());

                    $("#chkFavoritoImagen").prop("checked", true);

                    $("#hfFavoritoImagen").val($(this).parent().find("td").eq(2).html().trim());
                }

                $("#divFONDO").css("display", "none");
                $("#divBusquedaImagen").css("display", "none");
            }
        });
    }



    function fn_CrearEventoLaboratorio() {
        $(".JSBTABLA-1 tr td").click(function () {
            if ($(this).html().trim() != "") {
                if ($("#divBusquedaLaboratorio").find("#hfTipoBusqueda").val().trim() == "G") {
                    //limpiando controles antes de cargarlos
                    $("#spCodigoAnalisisLaboratorioSeleccionado").html("");
                    $("#chkAnalisisLaboratorioSeleccionado").prop("checked", false);
                    $("#spAnalisisLaboratorioSeleccionado").html("");
                    $("#chkAnalisisLaboratorioFavorito").prop("checked", false);
                    $("#hfIdeFavoritoAnalisis").val("");
                    $("#spPerfil").html("");
                    $("#chkAnalisisTipo1").removeClass("JCOL-OCULTA"); $("#chkAnalisisTipo1").addClass("JCOL-OCULTA");
                    $("#spAnalisisTipo1").removeClass("JCOL-OCULTA"); $("#spAnalisisTipo1").addClass("JCOL-OCULTA");
                    $("#chkAnalisisTipo2").removeClass("JCOL-OCULTA"); $("#chkAnalisisTipo2").addClass("JCOL-OCULTA");
                    $("#spAnalisisTipo2").removeClass("JCOL-OCULTA"); $("#spAnalisisTipo2").addClass("JCOL-OCULTA");
                    $("#chkAnalisisTipo3").removeClass("JCOL-OCULTA"); $("#chkAnalisisTipo3").addClass("JCOL-OCULTA");
                    $("#spAnalisisTipo3").removeClass("JCOL-OCULTA"); $("#spAnalisisTipo3").addClass("JCOL-OCULTA");

                    //cargando datos de la busqueda seleccionada
                    $("#spCodigoAnalisisLaboratorioSeleccionado").html($(this).parent().find("td").eq(0).html().trim());
                    $("#chkAnalisisLaboratorioSeleccionado").prop("checked", true);
                    $("#spAnalisisLaboratorioSeleccionado").html($(this).parent().find("td").eq(1).html().trim());

                    if ($(this).parent().find("td").eq(2).html().trim() != 0) {
                        $("#chkAnalisisLaboratorioFavorito").prop("checked", true);
                    }
                    $("#hfIdeFavoritoAnalisis").val($(this).parent().find("td").eq(3).html().trim());
                    
                    $("#spPerfil").html($(this).parent().find("td").eq(10).html().trim());
                } else {
                    //limpiando controles antes de cargarlos
                    $("#spCodigoAnalisisLaboratorioSeleccionado").html("");
                    $("#chkAnalisisLaboratorioSeleccionado").prop("checked", false);
                    $("#spAnalisisLaboratorioSeleccionado").html("");
                    $("#chkAnalisisLaboratorioFavorito").prop("checked", false);
                    $("#hfIdeFavoritoAnalisis").val("");
                    $("#spPerfil").html("");
                    $("#chkAnalisisTipo1").removeClass("JCOL-OCULTA"); $("#chkAnalisisTipo1").addClass("JCOL-OCULTA");
                    $("#spAnalisisTipo1").removeClass("JCOL-OCULTA"); $("#spAnalisisTipo1").addClass("JCOL-OCULTA");
                    $("#chkAnalisisTipo2").removeClass("JCOL-OCULTA"); $("#chkAnalisisTipo2").addClass("JCOL-OCULTA");
                    $("#spAnalisisTipo2").removeClass("JCOL-OCULTA"); $("#spAnalisisTipo2").addClass("JCOL-OCULTA");
                    $("#chkAnalisisTipo3").removeClass("JCOL-OCULTA"); $("#chkAnalisisTipo3").addClass("JCOL-OCULTA");
                    $("#spAnalisisTipo3").removeClass("JCOL-OCULTA"); $("#spAnalisisTipo3").addClass("JCOL-OCULTA");

                    //cargando datos de la busqueda seleccionada
                    $("#spCodigoAnalisisLaboratorioSeleccionado").html($(this).parent().find("td").eq(0).html().trim());
                    $("#chkAnalisisLaboratorioSeleccionado").prop("checked", true);
                    $("#spAnalisisLaboratorioSeleccionado").html($(this).parent().find("td").eq(1).html().trim());

                    $("#chkAnalisisLaboratorioFavorito").prop("checked", true);

                    $("#hfIdeFavoritoAnalisis").val($(this).parent().find("td").eq(2).html().trim());
                                        
                    $("#spPerfil").html($(this).parent().find("td").eq(9).html().trim());
                }

                $("#divFONDO").css("display", "none");
                $("#divBusquedaLaboratorio").css("display", "none");
            }
        });
    }


    function fn_CargaNotaIngreso() {
        $.ajax({
            url: "InformacionPaciente.aspx/ConsultarNotaIngreso",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (dato1, datos2, dato3) {

            }
        }).done(function (oOB_JSON) {
            if (oOB_JSON.d != "") {
                $("#divNotaIngreso").html("");
                $("#divNotaIngreso").append(oOB_JSON.d);                            
                /*$("#btnGuardarNotaIngreso").attr("disabled", "disabled");
                $("#txtNotaIngreso").attr("disabled", "disabled");
                06/09/2016
                */
            } else {
                $("#divNotaIngreso").html("");
                $("#btnGuardarNotaIngreso").removeAttr("disabled");
                $("#txtNotaIngreso").removeAttr("disabled");
            }
        });
    }


    function fn_CargarEvolucionClinica() {
        $.ajax({
            url: "InformacionPaciente.aspx/ConsultarEvolucionClinica",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (dato1, datos2, dato3) {

            }
        }).done(function (oOB_JSON) {
            if (oOB_JSON.d != "") {
                $("#divEvolucionClinica").html("");
                $("#divEvolucionClinica").append(oOB_JSON.d);
                $(".JFILA-SELECCIONEVOLUCION").click(function () {
                    var IdeEvolucion = 0;
                    IdeEvolucion = $(this).find(".JIDE_EVOLUCION").html().trim();
                    fn_LOAD_VISI();
                    $.ajax({
                        url: "InformacionPaciente.aspx/CargarDatosEvolucionClinica",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            IdeEvolucion: IdeEvolucion
                        }),
                        dataType: "json",
                        error: function (dato1, datos2, dato3) {

                        }
                    }).done(function (oOB_JSON) {
                        fn_LOAD_OCUL();
                        if (oOB_JSON.d.split(";")[0] == "ERROR") {

                        } else {
                            $("#txtSubjetiva").val(oOB_JSON.d.split(";")[0]);
                            $("#txtObjetiva").val(oOB_JSON.d.split(";")[1]);
                            $("#txtEvolucionClinica").val(oOB_JSON.d.split(";")[2]);
                            $("#txtPlan").val(oOB_JSON.d.split(";")[3]);
                            if (oOB_JSON.d.split(";")[4] == "E") {
                                $("#rbEducacionEvolucionClinica").prop("checked", true)
                            }
                            if (oOB_JSON.d.split(";")[4] == "I") {
                                $("#rbInformeEvolucionClinica").prop("checked", true);
                            }
                            if (oOB_JSON.d.split(";")[5] == "1") {
                                $("#chkRequiereFirma").prop("checked", true);
                            }
                            if (oOB_JSON.d.split(";")[6] == "01") {
                                $("#rbInestable").prop("checked", true);
                            }
                            if (oOB_JSON.d.split(";")[6] == "02") {
                                $("#rbDeterioro").prop("checked", true);
                            }
                            if (oOB_JSON.d.split(";")[6] == "03") {
                                $("#rbEstacionaria").prop("checked", true);
                            }
                            if (oOB_JSON.d.split(";")[6] == "04") {
                                $("#rbEstableMejoria").prop("checked", true);
                            }                            
                            if (oOB_JSON.d.split(";")[6] == "") {
                                $("#rbInestable").prop("checked", false);
                                $("#rbDeterioro").prop("checked", false);
                                $("#rbEstacionaria").prop("checked", false);
                                $("#rbEstableMejoria").prop("checked", false);
                            }
                        }
                    });
                });                
            } else {
                $("#divEvolucionClinica").html("");
            }
        });
    }


    function fn_CargarEvolucionClinica2(OrdenEjecutar, IdeEvolucionClinicaMostrar, objeto) {
        $.ajax({
            url: "InformacionPaciente.aspx/ConsultarEvolucionClinica2",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({
                Valor: IdeEvolucionClinicaMostrar,
                Orden: OrdenEjecutar   
            }),
            error: function (dato1, datos2, dato3) {

            }
        }).done(function (oOB_JSON) {
            if (oOB_JSON.d != "") {
                if (OrdenEjecutar == "1") {
                    $("#divEvolucionClinica").html("");
                    $("#divEvolucionClinica").append(oOB_JSON.d);
                }
                if (OrdenEjecutar == "2") {
                    objeto.parent().find(".JTREE3-HORA").html("");
                    objeto.parent().find(".JTREE3-HORA").append(oOB_JSON.d);
                }
                if (OrdenEjecutar == "3") {
                    objeto.next().html("");
                    objeto.next().append(oOB_JSON.d);
                }
                fn_CrearTreeViewEvolucionClinica();
            }            
        });
    }


    function fn_CrearTreeViewEvolucionClinica() {
        $("#divEvolucionClinica").find(".JFILA-FECHA").unbind("click");
        $("#divEvolucionClinica").find(".JFILA-FECHA").click(function () {
            var oObjeto = $(this);

            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $("#divEvolucionClinica").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                    $("#divEvolucionClinica").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                    $("#divEvolucionClinica").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");
                    oObjeto.addClass("JTREE3-SELECCIONADO");
                    oObjeto.find(" > .JTREE3-SIGNO").toggleClass("JTREE3-PLUS");
                    var CadenaClase = oObjeto.find(" > .JTREE3-SIGNO").attr("class");
                    var FecRegistro = oObjeto.find("> input").val();
                    if (CadenaClase.includes("JTREE3-PLUS")) {
                        fn_CargarEvolucionClinica2("2", FecRegistro, oObjeto);
                        oObjeto.parent().find(".JTREE3-HORA").css("display", "block");
                    } else {
                        oObjeto.parent().find(".JTREE3-HORA").css("display", "none");
                    }
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });
        $("#divEvolucionClinica").find(".JFILA-HORA").unbind("click");
        $("#divEvolucionClinica").find(".JFILA-HORA").click(function () {
            var oObjeto = $(this);

            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $("#divEvolucionClinica").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                    $("#divEvolucionClinica").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                    $("#divEvolucionClinica").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");

                    oObjeto.addClass("JTREE3-SELECCIONADO");
                    oObjeto.find(" > .JTREE3-SIGNO").toggleClass("JTREE3-PLUS");
                    var CadenaClase = oObjeto.find(" > .JTREE3-SIGNO").attr("class");
                    var IdeEvolucionClinica = oObjeto.find("> input").val();
                    if (CadenaClase.includes("JTREE3-PLUS")) {
                        fn_CargarEvolucionClinica2("3", IdeEvolucionClinica, oObjeto);
                        oObjeto.next().css("display", "block");
                    } else {
                        oObjeto.next().css("display", "none");
                    }
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });
        $("#divEvolucionClinica").find(".JTREE3-DETALLE").unbind("click");
        $("#divEvolucionClinica").find(".JTREE3-DETALLE").click(function () {
            var oObjeto = $(this);

            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $("#divEvolucionClinica").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                    $("#divEvolucionClinica").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                    $("#divEvolucionClinica").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");

                    oObjeto.addClass("JTREE3-SELECCIONADO");
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });
    }



    function fn_CargarControlClinicoIM() {
        $.ajax({
            url: "InformacionPaciente.aspx/ConsultaControlClinico",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (dato1, datos2, dato3) {

            },
            success: function (oOB_JSON) {                
                if (oOB_JSON.d != "") {
                    $("#divControlClinicoIndicacionMed").html("");
                    $("#divControlClinicoIndicacionMed").html(oOB_JSON.d);
                    fn_RenderizaTreeView2("divControlClinicoIndicacionMed", false);
                    //fn_CreaEventosTreeViewAI("divControlClinicoIndicacionMed", "");
                    //fn_CreaEventoCargaEvolucionClinica();
                } else {
                    //$("#divControlClinicoIndicacionMed").html("");
                }                
            }
       
        });

        /*$.ajax({
            url: "InformacionPaciente.aspx/ConsultaControlClinico",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (dato1, datos2, dato3) {

            }
        }).done(function (oOB_JSON) {
            alert(oOB_JSON.d);
            if (oOB_JSON.d != "") {
                $("#divControlClinicoIndicacionMed").html("");
                $("#divControlClinicoIndicacionMed").append(oOB_JSON.d);
                fn_CreaEventosTreeViewAI("divControlClinicoIndicacionMed", "");
                fn_CreaEventoCargaEvolucionClinica(); 
            } else {
                $("#divControlClinicoIndicacionMed").html("");
            }
        });*/
    }

    function fn_CargarControlClinicoIM2(OrdenEjecutar, FechaMostrar, IdeRecetaCabMostrar, objeto) {
        $.ajax({
            url: "InformacionPaciente.aspx/ConsultaControlClinico2",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({
                orden: OrdenEjecutar,
                fec_receta: FechaMostrar,
                ide_recetacab: IdeRecetaCabMostrar
            }),
            error: function (dato1, datos2, dato3) {

            },
            success: function (oOB_JSON) {
                if (oOB_JSON.d != "") {
                    if (OrdenEjecutar == "1") {
                        $("#divControlClinicoIndicacionMed2").html("");
                        $("#divControlClinicoIndicacionMed2").append(oOB_JSON.d);
                    }
                    if (OrdenEjecutar == "2") {
                        objeto.parent().find(".JTREE3-HORA").html("");
                        objeto.parent().find(".JTREE3-HORA").append(oOB_JSON.d);
                    }
                    if (OrdenEjecutar == "3") {
                        objeto.next().html("");
                        objeto.next().append(oOB_JSON.d);
                    }
                    fn_CrearEventoControlClinico2();
                } else {
                    
                }
            }

        });
    }

    function fn_CrearEventoControlClinico2() {
        $("#divControlClinicoIndicacionMed2").find(".JFILA-FECHA").unbind("click");
        $("#divControlClinicoIndicacionMed2").find(".JFILA-FECHA").click(function () {
            var oObjeto = $(this);

            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $("#divControlClinicoIndicacionMed2").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                    $("#divControlClinicoIndicacionMed2").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                    $("#divControlClinicoIndicacionMed2").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");
                    
                    oObjeto.addClass("JTREE3-SELECCIONADO");
                    oObjeto.find(" > .JTREE3-SIGNO").toggleClass("JTREE3-PLUS");
                    var CadenaClase = oObjeto.find(" > .JTREE3-SIGNO").attr("class");
                    var Fecha = oObjeto.find("> input").val();
                    if (CadenaClase.includes("JTREE3-PLUS")) {
                        fn_CargarControlClinicoIM2("2", Fecha, "0", oObjeto);
                        oObjeto.parent().find(".JTREE3-HORA").css("display", "block");
                    } else {
                        oObjeto.parent().find(".JTREE3-HORA").css("display", "none");
                    }
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });
        $("#divControlClinicoIndicacionMed2").find(".JFILA-HORA").unbind("click");
        $("#divControlClinicoIndicacionMed2").find(".JFILA-HORA").click(function () {
            var oObjeto = $(this);

            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $("#divControlClinicoIndicacionMed2").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                    $("#divControlClinicoIndicacionMed2").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                    $("#divControlClinicoIndicacionMed2").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");
                    
                    oObjeto.addClass("JTREE3-SELECCIONADO");
                    oObjeto.find(" > .JTREE3-SIGNO").toggleClass("JTREE3-PLUS");
                    var CadenaClase = oObjeto.find(" > .JTREE3-SIGNO").attr("class");
                    var IdRecetaCab = oObjeto.find("> input").val();
                    if (CadenaClase.includes("JTREE3-PLUS")) {
                        fn_CargarControlClinicoIM2("3", "", IdRecetaCab, oObjeto);
                        oObjeto.next().css("display", "block");
                    } else {
                        oObjeto.next().css("display", "none");
                    }
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });
        $("#divControlClinicoIndicacionMed2").find(".JTREE3-DETALLE").unbind("click");
        $("#divControlClinicoIndicacionMed2").find(".JTREE3-DETALLE").click(function () {
            var oObjeto = $(this);

            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $("#divControlClinicoIndicacionMed2").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                    $("#divControlClinicoIndicacionMed2").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                    $("#divControlClinicoIndicacionMed2").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");
                    
                    oObjeto.addClass("JTREE3-SELECCIONADO");
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });
    }



    function fn_GuardarControlClinico() {
        fn_oculta_mensaje();
        fn_LOAD_VISI();
        $.ajax({
            url: "InformacionPaciente.aspx/GuardarControlClinico",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (dato1, datos2, dato3) {

            }
        }).done(function (oOB_JSON) {
            fn_LOAD_OCUL();
            fn_Load_Page();
            //fn_CargarControlClinicoIM();
            fn_CargarControlClinicoIM2("1", "", "0");
            $("#divGridProductoMedicamento").load("GridViewAjax/GridProductoMedicamento.aspx");
            var IdVia = "<%= ddlVia_Con.ClientID %>"; //"<= ddlVia_Con.ClientID %>";  JB - 14/07/2020 - COMENTADO // JB - 27/04/2021 - txtVia_ControlClinico
            fn_DeshabilitaControles(IdVia + ";txtProducto_Con;txtDosis_Con;txtCadaHora_Con;txtIndicacionProductoMedicamento;imgBusquedaProducto;imgAgregar_Con;chkBuscarDci;txtCantidad_Con");
        });
    }
        
    function fn_GuardarNoFarmacologico() {
        $.ajax({
            url: "InformacionPaciente.aspx/GuardarNoFarmacologico",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({
                Nutricion: $("#txtNutricionNoFarmacologico").val(),
                TerapiaFisica: $("#txtTerapiaFisRehaNoFarmacologico").val(),
                CuidadosEnfermeria: $("#txtCuidadosEnfermeriaNoFarmacologico").val(),
                OtrosNoFarmacologico: $("#txtOtrosNoFarmacologico").val()
            }),
            error: function (dato1, datos2, dato3) {

            }
        }).done(function (oOB_JSON) {
            if (oOB_JSON.d == "OK") {
                fn_LOAD_OCUL();
                fn_oculta_mensaje();
                fn_Load_Page();
                //fn_CargarControlClinicoIM();                
                fn_CargarControlClinicoIM2("1", "", "0");
                $("#txtNutricionNoFarmacologico").val("");
                $("#txtTerapiaFisRehaNoFarmacologico").val("");
                $("#txtCuidadosEnfermeriaNoFarmacologico").val("");
                $("#txtOtrosNoFarmacologico").val("");
                fn_DeshabilitaControles("txtNutricionNoFarmacologico;txtTerapiaFisRehaNoFarmacologico;txtCuidadosEnfermeriaNoFarmacologico;txtOtrosNoFarmacologico");
            } else {
                $.JMensajePOPUP("Mensaje de Clínica San Felipe - Error", oOB_JSON.d.split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()", "");
            }            
        });
    }


    function fn_GuardarInfusion() {
        $.ajax({
            url: "InformacionPaciente.aspx/GuardarInfusion",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (dato1, datos2, dato3) {

            }
        }).done(function (oOB_JSON) {
            fn_LOAD_OCUL();
            fn_Load_Page();
            //fn_CargarControlClinicoIM();
            fn_CargarControlClinicoIM2("1", "", "0");
            fn_DeshabilitaControles("txtInfusionControlClinico");
            fn_LimpiarInfusion();            
        });
    }

    function fn_LimpiarInfusion() {
        var gridViewInfusion1 = $("#" + "<%= gvListadoInfusiones.ClientID %>");
        gridViewInfusion1.find("tr:gt(0)").remove();
    }


    //CANCELA GUARDADO DE CONTROL CLINICO
    function fn_CancelaGuardarControlClinico() {
        fn_LOAD_OCUL();
        fn_oculta_mensaje();
    }



    function fn_CreaEventoCargaEvolucionClinica() {
        $(".CheckCC").click(function () {
            if ($(this).prop("checked")) {
                var objeto = $(this);
                $.ajax({
                    url: "InformacionPaciente.aspx/VerificarMedicamento",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                        IdMedicamentoSuspendido: $(this).prev().html().trim()
                    }),
                    dataType: "json",
                    error: function (dato1, datos2, dato3) {
                    }
                }).done(function (oOB_JSON) {
                    if (oOB_JSON.d != "OK") {
                        $.JMensajePOPUP("Mensaje de Clínica San Felipe - Error", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_mensaje()", "");
                    } else {
                        //fn_CargarControlClinicoIM();
                        fn_CargarControlClinicoIM2("1", "", "0");
                    }
                });
            }
        });


        $(".SELECCIONCC").click(function () {
            //fn_LOAD_GRID_VISI();
            $(this).next().next().find("li").each(function () {
                var objeto = $(this);

                $.ajax({
                    url: "InformacionPaciente.aspx/ValidarProductoAgregado",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                        CodigoProducto: objeto.find("a").eq(6).html().trim()
                    }),
                    dataType: "json",
                    error: function (dato1, datos2, dato3) {

                    }
                }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "") {
                        $("#divGridProductoMedicamento").load("GridViewAjax/GridProductoMedicamento.aspx", { Codigo: objeto.find("a").eq(6).html().trim(),
                            Producto: objeto.find("a").eq(1).html().trim(),                            
                            Via: objeto.find("a").eq(3).html().trim(),
                            CadaHora: objeto.find("a").eq(4).html().trim(),
                            Cantidad: objeto.find("a").eq(5).html().trim(),
                            Dosis: objeto.find("a").eq(7).html().trim(),
                            Indicacion: objeto.find("a").eq(8).html().trim(),
                            Dia: objeto.find("a").eq(9).html().trim(),
                            Pagina: "1"
                        }, function () {
                            //fn_LOAD_GRID_OCUL();
                        });
                    } else {
                        //$.JMensajePOPUP("Aviso", "El Producto ya se encuentra agregado", "1", "Cerrar", "fn_oculta_mensaje()");
                    }
                });
            });
        });


        $(".SELECCIONCS").click(function () {
            var IdRecetaDet = $(this).attr("id");
            //$.JPopUp("Suspensión de Medicamentos - Control Clínico", "PopUp/SuspensionMedicamento.aspx");
            var aVA_DEMO = ["#divDatosUsuarioSuspensionMedicamento", "Utilidad/DatosUsuarioPopUp.aspx", ""];
            var aValores = [IdRecetaDet];
            $.JPopUp("Suspensión de Medicamentos - Control Clínico", "PopUp/SuspensionMedicamento.aspx", "2", "Suspender;Salir", "fn_SuspenderSuspension();fn_oculta_popup()", 75, aVA_DEMO, aValores); //fn_NuevoReconciliacionMedicamentosa
        });
    }


    
    /*var ContGen = 0;
    function fn_PruebaAs(){        
        $("[id*=txt_DetalleCirugia]").each(function () {            
            ContGen += 1;
            if ($(this).val().trim() != "") {                
                $(this).parent().parent().parent().css("display", "inline");
                $(this).parent().parent().parent().prev().css("display", "inline");
                $(this).parent().parent().parent().prev().prev().css("display", "inline");
                $(this).parent().parent().parent().prev().prev().prev().css("display", "inline");
                $(this).parent().parent().parent().next().css("display", "inline");
            }            
        });
    }*/

    function fn_CrearEventoControles() {
        /*let promesa1 = new Promise(function(resolve, reject){
            fn_PruebaAs();
            resolve();
        });

        promesa1.then(function(){
            alert("OK" + ContGen);
        }, function(){
            alert("ERROR");
        });*/
        

        //INICIO - JB - NUEVO CODIGO PARA CIRUGIAS
        $(".JDIV-CIRUGIA").css("display", "none");

        $(".JDIV-CIRUGIA").eq(0).css("display", "inline");
        $(".JDIV-CIRUGIA").eq(1).css("display", "inline");
        $(".JDIV-CIRUGIA").eq(2).css("display", "inline");
        $(".JDIV-CIRUGIA").eq(3).css("display", "inline");
        $(".JDIV-CIRUGIA").eq(4).css("display", "inline");

        $("[id*=imgAgregarCirugia]").click(function () {
            if ($(this).parent().parent().parent().prev().find("input").val() == "" && $(this).parent().parent().parent().prev().prev().find("input").val() == "" &&
                $(this).parent().parent().parent().prev().prev().prev().find("input").val() == "" && $(this).parent().parent().parent().prev().prev().prev().prev().find("input").val() == "") {
                return;
            }
            $(this).parent().parent().parent().next().css("display", "inline");
            //$(this).parent().parent().parent().next().find("input").css("display", "inline");

            $(this).parent().parent().parent().next().next().css("display", "inline");
            //$(this).parent().parent().parent().next().next().find("input").css("display", "inline");

            $(this).parent().parent().parent().next().next().next().css("display", "inline");
            //$(this).parent().parent().parent().next().next().next().find("input").css("display", "inline");

            $(this).parent().parent().parent().next().next().next().next().css("display", "inline");
            //$(this).parent().parent().parent().next().next().next().next().find("input").css("display", "inline");

            $(this).parent().parent().parent().next().next().next().next().next().css("display", "inline");
            $(this).parent().parent().parent().next().next().next().next().next().find("input").css("visibility", "visible");
            $(this).css("visibility", "hidden");
            if ($(this).attr("id").includes("imgAgregarCirugia15")) {
                $(this).css("visibility", "hidden");
            }

        });

        var ContCirugia = 0;
        var ContTotal = 0;
        var ContTotal2 = 0;
        var ContTotal3 = 0;
        var ContTotal4 = 0;
        var IdControl = 0;
        var IdControl2 = 0;
        var IdControl3 = 0;
        var IdControl4 = 0;

        $("[id*=txt_CirugiaCirugia]").each(function () {
            ContTotal += 1;
            if ($(this).val().trim() != "") {
                $(this).parent().parent().parent().css("display", "inline");
                $(this).parent().parent().parent().next().css("display", "inline");
                $(this).parent().parent().parent().next().next().css("display", "inline");
                $(this).parent().parent().parent().next().next().next().css("display", "inline");
                $(this).parent().parent().parent().next().next().next().next().css("display", "inline");
                var Aux = "";
                Aux = $(this).attr("id").split("-")[0];
                IdControl = Aux.substring(Aux.length - 1, Aux.length);
            }
        });
        
        $("[id*=txt_FechaCirugia]").each(function () {
            ContTotal2 += 1;
            if ($(this).val().trim() != "") {                
                $(this).parent().parent().parent().css("display", "inline");
                $(this).parent().parent().parent().prev().css("display", "inline");
                $(this).parent().parent().parent().next().css("display", "inline");
                $(this).parent().parent().parent().next().next().css("display", "inline");
                $(this).parent().parent().parent().next().next().next().css("display", "inline");
                var Aux = "";
                Aux = $(this).attr("id").split("-")[0];
                IdControl2 = Aux.substring(Aux.length - 1, Aux.length);
            }            
        });
        $("[id*=txt_DiagnosticoCirugia]").each(function () {
            ContTotal3 += 1;
            if ($(this).val().trim() != "") {                
                $(this).parent().parent().parent().css("display", "inline");
                $(this).parent().parent().parent().prev().css("display", "inline");
                $(this).parent().parent().parent().prev().prev().css("display", "inline");
                $(this).parent().parent().parent().next().css("display", "inline");
                $(this).parent().parent().parent().next().next().css("display", "inline");
                var Aux = "";
                Aux = $(this).attr("id").split("-")[0];
                IdControl3 = Aux.substring(Aux.length - 1, Aux.length);
            }            
        });
        $("[id*=txt_DetalleCirugia]").each(function () {
            ContTotal4 += 1;
            if ($(this).val().trim() != "") {                
                $(this).parent().parent().parent().css("display", "inline");
                $(this).parent().parent().parent().prev().css("display", "inline");
                $(this).parent().parent().parent().prev().prev().css("display", "inline");
                $(this).parent().parent().parent().prev().prev().prev().css("display", "inline");
                $(this).parent().parent().parent().next().css("display", "inline");
                var Aux = "";
                Aux = $(this).attr("id").split("-")[0];
                IdControl4 = Aux.substring(Aux.length - 1, Aux.length);
            }
        });
        if (ContTotal == 15 && ContTotal2 == 15 && ContTotal3 == 15 && ContTotal4 == 15) {
            var Mayor = 0;
            Mayor = IdControl
            if (IdControl2 > Mayor) {
                Mayor = IdControl2
            }
            if (IdControl3 > Mayor) {
                Mayor = IdControl3
            }
            if (IdControl4 > Mayor) {
                Mayor = IdControl4
            }
            $("[id*=imgAgregarCirugia]").each(function () {
                //$(this).parent().parent().parent().css("display", "none");
                $(this).css("visibility", "hidden");
            });
            //            $("#imgAgregarCirugia " + Mayor).parent().parent().parent().css("display", "inline");
            if (Mayor != 0) {
                $("[id*=imgAgregarCirugia" + Mayor + "]").css("visibility", "visible");
            } else {
                $("[id*=imgAgregarCirugia" + "1" + "]").css("visibility", "visible");
            }
            
        }
        //FIN - JB - NUEVO CODIGO PARA CIRUGIAS


        $("[id*=divContenedorDinamico]").find(".JBOTON").unbind("click");
        $("[id*=divContenedorDinamico]").find(".JBOTON").click(function () {
            var objeto_boton = $(this);
            fn_LOAD_VISI();
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    //fn_LOAD_VISI();
                    var IdControl = ""; var ValorControl = "";
                    /*objeto_boton.parent().parent().parent().parent().find(":input:not(.JBOTON)").each(function () {
                    //CREANDO VARIABLE QUE GUARDARA LOS ID DE LOS CONTROLES A GUARDAR
                    IdControl += $(this).prop("id") + ";";
                    //CREANDO VARIABLE QUE GUARDARA EL VALOR DE LOS CONTROLES A GUARDAR
                    if ($(this).is("input[type='radio']")) {
                    if ($(this).prop("checked")) {
                    ValorControl += "1" + ";";
                    } else {
                    ValorControl += "0" + ";";
                    }
                    }
                    if ($(this).is("input[type='checkbox']")) {
                    if ($(this).prop("checked")) {
                    ValorControl += "1" + ";";
                    } else {
                    ValorControl += "0" + ";";
                    }
                    }
                    if ($(this).is("input[type='text']")) {
                    ValorControl += $(this).val() + ";";
                    }
                    if ($(this).is("textarea")) {
                    ValorControl += $(this).val() + ";";
                    }
                    if ($(this).is("select")) {
                    ValorControl += $(this).val() + ";";
                    } 
                    
                    }); JB - COMENTADO - 27/10/2020*/

                    var IdBoton = $.trim(objeto_boton.attr("id").substring(0, objeto_boton.attr("id").indexOf("_btn")));

                    $("[id*=" + IdBoton + "]").not(".JBOTON").each(function () {
                        var oObjeto = $(this);
                        //IdControl += $(this).prop("id").replace((IdBoton + "_"), "") + ";"; //$(this).prop("id") + ";";

                        //Comentado Christian Méndez
                        //Validar caracter extraño; por palote |
                        //Cambiarlas los ; ´pr los | Palotes
                        //Inicio
                        if ($(this).is("input[type='radio']")) {
                            if ($(this).prop("checked")) {
                                ValorControl += "1" + "|";
                            } else {
                                ValorControl += "0" + "|"; 
                            }
                            IdControl += oObjeto.prop("id").replace((IdBoton + "_"), "") + "|"; //$(this).prop("id") + ";";
                        }
                        if ($(this).is("input[type='checkbox']")) {
                            if ($(this).prop("checked")) {
                                ValorControl += "1" + "|";
                            } else {
                                ValorControl += "0" + "|";
                            }
                            IdControl += oObjeto.prop("id").replace((IdBoton + "_"), "") + "|"; //$(this).prop("id") + ";";
                        }
                        if ($(this).is("input[type='text']")) {
                            ValorControl += $(this).val() + "|";
                            IdControl += oObjeto.prop("id").replace((IdBoton + "_"), "") + "|"; //$(this).prop("id") + ";";
                        }
                        if ($(this).is("textarea")) {
                            ValorControl += $(this).val() + "|";
                            IdControl += oObjeto.prop("id").replace((IdBoton + "_"), "") + "|"; //$(this).prop("id") + ";";
                        }
                        if ($(this).is("select")) {
                            ValorControl += $(this).val() + "|";
                            IdControl += oObjeto.prop("id").replace((IdBoton + "_"), "") + "|"; //$(this).prop("id") + ";";
                        }
                    });

                    IdControl = IdControl.substring(0, IdControl.lastIndexOf("|"));
                    ValorControl = ValorControl.substring(0, ValorControl.lastIndexOf("|"));
                    //Fin
                    $.ajax({
                        url: "InformacionPaciente.aspx/GuardarActualizarDatos",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            IdControles: IdControl,
                            ValorControl: ValorControl
                        }),
                        dataType: "json",
                        error: function (dato1, datos2, dato3) {

                        }
                    }).done(function (oOB_JSON) {
                        //MENSAJE DE GUARDADO
                        if (oOB_JSON.d > 0 || oOB_JSON.d.toString() == "-9") {
                            fn_LOAD_OCUL();
                            $.JMensajePOPUP("Información", "Se guardaron los datos correctamente", "OK", "Aceptar", "fn_oculta_mensaje()");
                        } else {
                            fn_LOAD_OCUL();
                            $.JMensajePOPUP("Información", oOB_JSON.d, "OK", "Aceptar", "fn_oculta_mensaje()");
                        }

                    });
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });


        $("[id*=divContenedorDinamico] :input:not(.JBOTON)").unbind("blur");
        /*$("[id*=divContenedorDinamico] :input:not(.JBOTON)").blur(function () {
            var objeto = $(this);
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    //var demo = $(this).val() + " - " + $(this).attr("id");
                    var IdControl = objeto.attr("id");
                    var ValorControl = objeto.val();
                    if (objeto.is("input[type='radio']")) {
                        if (objeto.prop("checked")) {
                            ValorControl = "1";
                        } else {
                            ValorControl = "0";
                        }
                    }
                    if (objeto.is("input[type='checkbox']")) {
                        if (objeto.prop("checked")) {
                            ValorControl = "1";
                        } else {
                            ValorControl = "0";
                        }
                    }                    
                    $.ajax({
                        url: "InformacionPaciente.aspx/GuardarActualizarDatos",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            IdControles: IdControl,
                            ValorControl: ValorControl
                        }),
                        dataType: "json",
                        error: function (dato1, datos2, dato3) {

                        }
                    }).done(function (oOB_JSON) {
                        if (oOB_JSON.d > 0) {
                            //fn_LOAD_OCUL();
                            //$.JMensajePOPUP("Información", "Se guardaron los datos correctamente", "OK", "Aceptar", "fn_oculta_mensaje()"); //fn_oculta_mensaje_rapido
                        }
                    });

                }
            });
        });JB - COMENTADO - 27/10/2020*/

        /*CLICK  EN OPTION QUE ABRIRA PANTALLA SIMILAR A MEDICAMENTOSA  -  input[id*='opt_ramSi']*/
        $("#opt_ramSi, #opt_transfusionesSi, #opt_hiptensionSi, #opt_diabetesSi, #opt_asmaSi, #opt_enfCardiacaSi, #opt_enfRenalSi, #opt_gastritisSi, #opt_viajes3mSi, #opt_fiebreMaltaSi, #opt_hepatitisViralSi, #opt_tuberculosisSi, #opt_enfTiroideaSi, #opt_otrosSi").unbind("click");
        $("#opt_ramSi, #opt_transfusionesSi, #opt_hiptensionSi, #opt_diabetesSi, #opt_asmaSi, #opt_enfCardiacaSi, #opt_enfRenalSi, #opt_gastritisSi, #opt_viajes3mSi, #opt_fiebreMaltaSi, #opt_hepatitisViralSi, #opt_tuberculosisSi, #opt_enfTiroideaSi, #opt_otrosSi").click(function () {
            var objeto = $(this);
            sOpcionRadio = $(this).attr("id");
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    var IdExamen = objeto.attr("name");
                    $.ajax({
                        url: "InformacionPaciente.aspx/ObtenerPatologia",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            Codigo: IdExamen
                        }),
                        dataType: "json",
                        error: function (dato1, datos2, dato3) {
                        }
                    }).done(function (oOB_JSON) {
                        if (oOB_JSON.d.split(";").length > 1) {
                            var aVA_DEMO = ["#divDatosUsuarioMedicamentos", "Utilidad/DatosUsuarioPopUp.aspx", ""];
                            var aValores = [oOB_JSON.d.split(";")[0], oOB_JSON.d.split(";")[1]];
                            $.JPopUp("", "PopUp/Medicamentos.aspx", "1", "Salir", "fn_oculta_popup_medicamentos()", 85, aVA_DEMO, aValores); //fn_NuevoReconciliacionMedicamentosa
                        } else {
                            if (oOB_JSON.d != "") {
                                $.JMensajePOPUP("Mensaje del Sistema Clinica San Felipe - Error", oOB_JSON.d, "ERROR", "Aceptar", "fn_oculta_mensaje()");
                            }
                        }
                    });
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });

        $(".JIMGRADIO").unbind("click");
        $(".JIMGRADIO").click(function () {            
            $(this).next().trigger("click");
        });

        

        /*CALCULANDO CAMPO ICM*/
        $("[id*=divContenedorDinamico]").find("#TxtTalla").unbind("blur");
        $("[id*=divContenedorDinamico]").find("#TxtPeso").unbind("blur");
        $("[id*=divContenedorDinamico]").find("#TxtPeso").blur(function () {
            if ($("#TxtPeso").val().trim() != "" && $("#TxtTalla").val().trim() != "") {
                $("#TxtICM").val(($("#TxtPeso").val() / (($("#TxtTalla").val() / 100) * ($("#TxtTalla").val() / 100))).toFixed(2));
            }            
        });
        $("[id*=divContenedorDinamico]").find("#TxtTalla").blur(function () {
            if ($("#TxtPeso").val().trim() != "" && $("#TxtTalla").val().trim() != "") {
                $("#TxtICM").val(($("#TxtPeso").val() / (($("#TxtTalla").val() / 100) * ($("#TxtTalla").val() / 100))).toFixed(2));
            }
        });


        /*22/02/2017*/
        $("#txt_talla").blur(function () {
            if ($("#txt_talla").val().indexOf(".") == -1) {

            } else {
                if ($("#txt_talla").val().trim() != "") {
                    $("#txt_talla").val($("#txt_talla").val() * 100);
                }
            }
        });


        /*PATOLOGIA*/
        $('[id^=txt_fur-]').unbind("blur");
        $('[id^=txt_fur-]').blur(function () {            
            $("#" + "<%=TxtFechaUltimaRegla.ClientID %>").val($(this).val());
        });

        $('[id^=txt_fur-]').unbind("change");
        $('[id^=txt_fur-]').change(function () {
            $('[id^=txt_fur-]').removeAttr("readonly", true);
            $("#" + "<%=TxtFechaUltimaRegla.ClientID %>").val($(this).val());
            var IdControl = $(this).attr("id");
            var ValorControl = $(this).val();
            
            $.ajax({
                url: "InformacionPaciente.aspx/GuardarActualizarDatos",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    IdControles: IdControl,
                    ValorControl: ValorControl
                }),
                dataType: "json",
                error: function (dato1, datos2, dato3) {

                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d > 0) {

                }
            });
        });
    }

    function fn_ImagenRadio() {
        /*JB - COMENTADO TEMPORALMENTE IMG...
        if ($("#" + sOpcionRadio).prev().length == 1) {
        if ($("#" + sOpcionRadio).prev().attr("src") == "../Imagenes/ico_vacio.png") {
        $("#" + sOpcionRadio).prev().remove();
        $("#" + sOpcionRadio).before("<img src='../Imagenes/ico_data.png' alt='' class='JIMG-GENERAL JIMGRADIO' style='height:16px;'>");
        }
        }
        $(".JIMGRADIO").unbind("click");
        $(".JIMGRADIO").click(function () {
        $(this).next().trigger("click");
        });*/

        //$("#" + sOpcionRadio).prev().html("<img src='../Imagenes/ico_data.png' alt='' class='JIMG-GENERAL JIMGRADIO' style='height:17px;'>");
    }


    function fn_CalculaICM(IdControl1, IdControl2, IdControl3) {                
        if ($("#" + IdControl1).val().trim() != "" && $("#" + IdControl2).val().trim() != "") {
            $("#" + IdControl3).val(($("#" + IdControl1).val() / (($("#" + IdControl2).val() / 100) * ($("#" + IdControl2).val() / 100))).toFixed(2));
        }

        $("#" + IdControl3).removeAttr("disabled");        
        $("#" + IdControl3).trigger("blur");
        $("#" + IdControl3).attr("disabled", "disabled");
    }

    function fn_MostraNewsPews() {
        $.ajax({
            url: "InformacionPaciente.aspx/VerificarEdadNewPew",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (dato1, datos2, dato3) {

            }
        }).done(function (oOB_JSON) {
            if (oOB_JSON.d.split(";").lenght > 1) {

            } else {
                if (oOB_JSON.d.indexOf("AÑO") != -1) {
                    if (oOB_JSON.d.split(" ")[0] >= 15) {
                        fn_CalculaNews();                        
                        $("[id*=Opt_NormalComportamientoPes-]").parent().parent().css("display", "none");
                        $("[id*=Opt_NormalComportamientoPes-]").parent().parent().prev().css("display", "none");
                        $("[id*=Opt_NormalComportamientoPes-]").parent().parent().prev().prev().css("display", "none");
                        $("[id*=Opt_NormalComportamientoPes-]").parent().parent().prev().prev().prev().css("display", "none"); //prev()
                        $("[id*=Opt_TendenciaSuenoComportamientoPews-]").parent().parent().css("display", "none");
                        $("[id*=Opt_IrritableComportamientoPews-]").parent().parent().css("display", "none");
                        $("[id*=Opt_LetargicoConfusoComportamientoPews-]").parent().parent().css("display", "none");

                        $("[id*=Opt_RosaCardiovascularPews-]").parent().parent().css("display", "none");
                        $("[id*=Opt_RosaCardiovascularPews-]").parent().parent().prev().css("display", "none");
                        $("[id*=Opt_PalidoCardiovascularPews-]").parent().parent().css("display", "none");
                        $("[id*=Opt_Gris4CardiovascularPews-]").parent().parent().css("display", "none");
                        $("[id*=Opt_Gris5CardiovascularPews-]").parent().parent().css("display", "none");

                        $("[id*=Opt_NormalRespiratorioPews-]").parent().parent().css("display", "none");
                        $("[id*=Opt_NormalRespiratorioPews-]").parent().parent().prev().css("display", "none");
                        $("[id*=Opt_FR10RespiratorioPews-]").parent().parent().css("display", "none");
                        $("[id*=Opt_FR10RespiratorioPews-]").parent().parent().css("display", "none");
                        $("[id*=Opt_FR30RespiratorioPews-]").parent().parent().css("display", "none");


                        $("[id*=Opt_NuloNebulizadorInhaladorPews-]").parent().parent().css("display", "none");
                        $("[id*=Opt_NuloNebulizadorInhaladorPews-]").parent().parent().prev().css("display", "none");
                        $("[id*=Opt_Cada15NebulizadorInhaladorPews-]").parent().parent().css("display", "none");

                        $("[id*=Opt_NuloVomitoPostPews-]").parent().parent().css("display", "none");
                        $("[id*=Opt_NuloVomitoPostPews-]").parent().parent().prev().css("display", "none");
                        $("[id*=Opt_VomitoPersistenteVomitoPostPews-]").parent().parent().css("display", "none");

                        $("[id*=txtPuntajeTotalExamFisicoPews-]").parent().parent().css("display", "none");
                        $("[id*=txtPuntajeTotalExamFisicoPews-]").parent().parent().prev().css("display", "none");
                        $("[id*=txtFrecuenciaEvaluacionExamFisicoPews-]").parent().parent().css("display", "none");
                        $("[id*=txtFrecuenciaEvaluacionExamFisicoPews-]").parent().parent().prev().css("display", "none");

                    } else {
                        fn_CalculaPews();                        
                        $("[id*=txtPuntajeTotalExamFisico-]").parent().parent().css("display", "none");
                        $("[id*=txtPuntajeTotalExamFisico-]").parent().parent().prev().css("display", "none");
                        $("[id*=txtPuntajeTotalExamFisico-]").parent().parent().prev().prev().css("display", "none");

                        $("[id*=txtRiesgoClinicoExamFisico-]").parent().parent().css("display", "none");
                        $("[id*=txtRiesgoClinicoExamFisico-]").parent().parent().prev().css("display", "none");
                    }
                }
                if (oOB_JSON.d.indexOf("MES") != -1) {
                    fn_CalculaPews();                    
                    $("[id*=txtPuntajeTotalExamFisico-]").parent().parent().css("display", "none");
                    $("[id*=txtPuntajeTotalExamFisico-]").parent().parent().prev().css("display", "none");
                    $("[id*=txtPuntajeTotalExamFisico-]").parent().parent().prev().prev().css("display", "none");

                    $("[id*=txtRiesgoClinicoExamFisico-]").parent().parent().css("display", "none");
                    $("[id*=txtRiesgoClinicoExamFisico-]").parent().parent().prev().css("display", "none");
                }
            }
        });


        /*if ($("#DatosUsuarioInformacionPaciente").find("#spDatosEdad").length > 0) {            
            var sDatosEdad = $("#DatosUsuarioInformacionPaciente").find("#spDatosEdad").html().trim();            
            if (sDatosEdad.indexOf("AÑO") != -1) {
                if (sDatosEdad.split(" ")[0] >= 15) {
                    fn_CalculaNews();                    
                    $("#Opt_NormalComportamientoPes-692").parent().parent().css("display", "none");
                    $("#Opt_NormalComportamientoPes-692").parent().parent().prev().css("display", "none");
                    $("#Opt_NormalComportamientoPes-692").parent().parent().prev().prev().css("display", "none");
                    $("#Opt_NormalComportamientoPes-692").parent().parent().prev().prev().prev().prev().css("display", "none");
                    $("#Opt_TendenciaSuenoComportamientoPews-693").parent().parent().css("display", "none");
                    $("#Opt_IrritableComportamientoPews-694").parent().parent().css("display", "none");
                    $("#Opt_LetargicoConfusoComportamientoPews-695").parent().parent().css("display", "none");

                    $("#Opt_RosaCardiovascularPews-696").parent().parent().css("display", "none");
                    $("#Opt_RosaCardiovascularPews-696").parent().parent().prev().css("display", "none");
                    $("#Opt_PalidoCardiovascularPews-697").parent().parent().css("display", "none");
                    $("#Opt_Gris4CardiovascularPews-698").parent().parent().css("display", "none");
                    $("#Opt_Gris5CardiovascularPews-699").parent().parent().css("display", "none");

                    $("#Opt_NormalRespiratorioPews-700").parent().parent().css("display", "none");
                    $("#Opt_NormalRespiratorioPews-700").parent().parent().prev().css("display", "none");
                    $("#Opt_FR10RespiratorioPews-701").parent().parent().css("display", "none");
                    $("#Opt_FR10RespiratorioPews-702").parent().parent().css("display", "none");
                    $("#Opt_FR30RespiratorioPews-703").parent().parent().css("display", "none");


                    $("#Opt_NuloNebulizadorInhaladorPews-704").parent().parent().css("display", "none");
                    $("#Opt_NuloNebulizadorInhaladorPews-704").parent().parent().prev().css("display", "none");
                    $("#Opt_Cada15NebulizadorInhaladorPews-705").parent().parent().css("display", "none");

                    $("#Opt_NuloVomitoPostPews-706").parent().parent().css("display", "none");
                    $("#Opt_NuloVomitoPostPews-706").parent().parent().prev().css("display", "none");
                    $("#Opt_VomitoPersistenteVomitoPostPews-707").parent().parent().css("display", "none");

                    $("#txtPuntajeTotalExamFisicoPews-831").parent().parent().css("display", "none");
                    $("#txtPuntajeTotalExamFisicoPews-831").parent().parent().prev().css("display", "none");
                    $("#txtFrecuenciaEvaluacionExamFisicoPews-832").parent().parent().css("display", "none");
                    $("#txtFrecuenciaEvaluacionExamFisicoPews-832").parent().parent().prev().css("display", "none");
                    
                } else {
                    fn_CalculaPews();                    
                    $("#txtPuntajeTotalExamFisico-792").parent().parent().css("display", "none");
                    $("#txtPuntajeTotalExamFisico-792").parent().parent().prev().css("display", "none");
                    $("#txtPuntajeTotalExamFisico-792").parent().parent().prev().prev().css("display", "none");

                    $("#txtRiesgoClinicoExamFisico-793").parent().parent().css("display", "none");
                    $("#txtRiesgoClinicoExamFisico-793").parent().parent().prev().css("display", "none");                  
                }
            }
            if (sDatosEdad.indexOf("MES") != -1) {
                fn_CalculaPews();
                $("#txtPuntajeTotalExamFisico-792").parent().parent().css("display", "none");
                $("#txtPuntajeTotalExamFisico-792").parent().parent().prev().css("display", "none");
                $("#txtPuntajeTotalExamFisico-792").parent().parent().prev().prev().css("display", "none");

                $("#txtRiesgoClinicoExamFisico-793").parent().parent().css("display", "none");
                $("#txtRiesgoClinicoExamFisico-793").parent().parent().prev().css("display", "none"); 
            }

        }  */

        /*var url = "CiePetitorio_2.aspx/FinalizarConsulta";        
        var dato = { "CodigoAtencion": $("#" + "hfCodAtencion.ClientID ").val(), "CodigoPaciente": $("#" + "hfCodPaciente.ClientID").val() };
        fn_FuncionAjax(url, dato, "fn_DatoAjaxFinalizarConsulta");*/

    }

    function fn_CalculaNews() {
        //var valorFR, valorSAT, valorTEMP, valorFC = "0";
        $("[id*=divContenedorDinamico]").find("[id*=txt_fr-]").blur(function () {            
            if ($("[id*=divContenedorDinamico]").find("[id*=txt_fc-]").val().trim() == "" && $("[id*=divContenedorDinamico]").find("[id*=txt_temp-]").val().trim() == "" && $("[id*=divContenedorDinamico]").find("[id*=txt_sat-]").val().trim() == "" && $("[id*=divContenedorDinamico]").find("[id*=txt_fr-]").val().trim() == "") {
                $("[id*=txtPuntajeTotalExamFisico-]").val("");
                $("[id*=txtRiesgoClinicoExamFisico-]").val("");

                $("[id*=txtPuntajeTotalExamFisico-]").removeAttr("disabled");
                $("[id*=txtPuntajeTotalExamFisico-]").trigger("blur");
                $("[id*=txtPuntajeTotalExamFisico-]").attr("disabled", "disabled");

                $("[id*=txtRiesgoClinicoExamFisico-]").removeAttr("disabled");
                $("[id*=txtRiesgoClinicoExamFisico-]").trigger("blur");
                $("[id*=txtRiesgoClinicoExamFisico-]").attr("disabled", "disabled");
                valorTEMP = 0; valorFR = 0; valorSAT = 0; valorFC = 0;
                return false;
            }



            var CampoFR = $(this).val();
            var CampoPuntajeTotal = "";
            CampoPuntajeTotal = $("[id*=txtPuntajeTotalExamFisico-]").val().trim();
            if (CampoPuntajeTotal == "") {
                CampoPuntajeTotal = 0
            }

            if (valorFR != 0) {
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) - valorFR); //limpiando el valor en puntaje si es que lo tiene
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) - valorFR;
            }

            if (CampoFR <= 8) {
                valorFR = 3;
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) + 3);
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) + 3;
                $("[id*=txtRiesgoClinicoExamFisico-]").val("MEDIO - ALTO");
            }
            if (CampoFR > 8 && CampoFR < 12) {
                valorFR = 1;
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) + 1);
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) + 1;
            }
            if (CampoFR > 11 && CampoFR < 21) {
                valorFR = 0;
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) + 0);
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) + 0;
            }
            if (CampoFR > 20 && CampoFR < 25) {
                valorFR = 2;
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) + 2);
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) + 2;
            }
            if (CampoFR > 25) {
                valorFR = 3;
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) + 3);
                $("[id*=txtRiesgoClinicoExamFisico-]").val("MEDIO - ALTO");
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) + 3;
            }
            if (CampoFR == "") {
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) - valorFR);
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) - valorFR;
                valorFR = 0;
            }

            if ($("[id*=txtPuntajeTotalExamFisico-]").val() > 4) { //si es mayor a 4
                $("[id*=txtRiesgoClinicoExamFisico-]").val("MEDIO - ALTO");
            } else {
                $("[id*=txtRiesgoClinicoExamFisico-]").val("BAJO");
            }
            if (valorFR == 3 || valorSAT == 3 || valorTEMP == 3 || valorFC == 3) {
                $("[id*=txtRiesgoClinicoExamFisico-]").val("MEDIO - ALTO");
            }

            $("[id*=txtPuntajeTotalExamFisico-]").removeAttr("disabled");
            $("[id*=txtPuntajeTotalExamFisico-]").trigger("blur");
            $("[id*=txtPuntajeTotalExamFisico-]").attr("disabled", "disabled");

            $("[id*=txtRiesgoClinicoExamFisico-]").removeAttr("disabled");
            $("[id*=txtRiesgoClinicoExamFisico-]").trigger("blur");
            $("[id*=txtRiesgoClinicoExamFisico-]").attr("disabled", "disabled");
        });

        $("[id*=divContenedorDinamico]").find("[id*=txt_sat-]").blur(function () {
            if ($("[id*=divContenedorDinamico]").find("[id*=txt_fc-]").val().trim() == "" && $("[id*=divContenedorDinamico]").find("[id*=txt_temp-]").val().trim() == "" && $("[id*=divContenedorDinamico]").find("[id*=txt_sat-]").val().trim() == "" && $("[id*=divContenedorDinamico]").find("[id*=txt_fr-]").val().trim() == "") {
                $("[id*=txtPuntajeTotalExamFisico-]").val("");
                $("[id*=txtRiesgoClinicoExamFisico-]").val("");

                $("[id*=txtPuntajeTotalExamFisico-]").removeAttr("disabled");
                $("[id*=txtPuntajeTotalExamFisico-]").trigger("blur");
                $("[id*=txtPuntajeTotalExamFisico-]").attr("disabled", "disabled");

                $("[id*=txtRiesgoClinicoExamFisico-]").removeAttr("disabled");
                $("[id*=txtRiesgoClinicoExamFisico-]").trigger("blur");
                $("[id*=txtRiesgoClinicoExamFisico-]").attr("disabled", "disabled");
                valorTEMP = 0; valorFR = 0; valorSAT = 0; valorFC = 0;
                return false;
            }
            

            var CampoSAT = $(this).val();
            var CampoPuntajeTotal = "";
            CampoPuntajeTotal = $("[id*=txtPuntajeTotalExamFisico-]").val().trim();
            if (CampoPuntajeTotal == "") {
                CampoPuntajeTotal = 0
            }

            if (valorSAT != 0) {
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) - valorSAT); //limpiando el valor en puntaje si es que lo tiene
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) - valorSAT;
            }

            if (CampoSAT <= 91) {
                valorSAT = 3;
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) + 3);
                $("[id*=txtRiesgoClinicoExamFisico-]").val("MEDIO - ALTO");
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) + 3;
            }
            if (CampoSAT == 92 || CampoSAT == 93) {
                valorSAT = 2;
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) + 2);
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) + 2;
            }
            if (CampoSAT == 94 || CampoSAT == 95) {
                valorSAT = 1;
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) + 1);
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) + 1;
            }
            if (CampoSAT >= 96) {
                valorSAT = 0;
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) + 0);
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) + 0;
            }
            if (CampoSAT == "") {
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) - valorSAT);
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) - valorSAT;
                valorSAT = 0;
            }

            if ($("[id*=txtPuntajeTotalExamFisico-]").val() > 4) { //si es mayor a 4
                $("[id*=txtRiesgoClinicoExamFisico-]").val("MEDIO - ALTO");
            } else {
                $("[id*=txtRiesgoClinicoExamFisico-]").val("BAJO");
            }
            if (valorFR == 3 || valorSAT == 3 || valorTEMP == 3 || valorFC == 3) {
                $("[id*=txtRiesgoClinicoExamFisico-]").val("MEDIO - ALTO");
            }

            $("[id*=txtPuntajeTotalExamFisico-]").removeAttr("disabled");
            $("[id*=txtPuntajeTotalExamFisico-]").trigger("blur");
            $("[id*=txtPuntajeTotalExamFisico-]").attr("disabled", "disabled");

            $("[id*=txtRiesgoClinicoExamFisico-]").removeAttr("disabled");
            $("[id*=txtRiesgoClinicoExamFisico-]").trigger("blur");
            $("[id*=txtRiesgoClinicoExamFisico-]").attr("disabled", "disabled");
        });


        $("[id*=divContenedorDinamico]").find("[id*=txt_temp-]").blur(function () {
            if ($("[id*=divContenedorDinamico]").find("[id*=txt_fc-]").val().trim() == "" && $("[id*=divContenedorDinamico]").find("[id*=txt_temp-]").val().trim() == "" && $("[id*=divContenedorDinamico]").find("[id*=txt_sat-]").val().trim() == "" && $("[id*=divContenedorDinamico]").find("[id*=txt_fr-]").val().trim() == "") {
                $("[id*=txtPuntajeTotalExamFisico-]").val("");
                $("[id*=txtRiesgoClinicoExamFisico-]").val("");

                $("[id*=txtPuntajeTotalExamFisico-]").removeAttr("disabled");
                $("[id*=txtPuntajeTotalExamFisico-]").trigger("blur");
                $("[id*=txtPuntajeTotalExamFisico-]").attr("disabled", "disabled");

                $("[id*=txtRiesgoClinicoExamFisico-]").removeAttr("disabled");
                $("[id*=txtRiesgoClinicoExamFisico-]").trigger("blur");
                $("[id*=txtRiesgoClinicoExamFisico-]").attr("disabled", "disabled");
                valorTEMP = 0; valorFR = 0; valorSAT = 0; valorFC = 0;
                return false;
            }
            

            var CampoTEMP = $(this).val();
            var CampoPuntajeTotal = "";
            CampoPuntajeTotal = $("[id*=txtPuntajeTotalExamFisico-]").val().trim();
            if (CampoPuntajeTotal == "") {
                CampoPuntajeTotal = 0
            }

            if (valorTEMP != 0) {
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) - valorTEMP); //limpiando el valor en puntaje si es que lo tiene
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) - valorTEMP;
            }

            if (CampoTEMP <= 35) {
                valorTEMP = 3;
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) + 3);
                $("[id*=txtRiesgoClinicoExamFisico-]").val("MEDIO - ALTO");
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) + 3;
            }
            if (CampoTEMP > 35 && CampoTEMP < 37) {
                valorTEMP = 1;
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) + 1);
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) + 1;
            }
            if (CampoTEMP > 36 && CampoTEMP < 39) {
                valorTEMP = 0;
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) + 0);
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) + 0;
            }
            if (CampoTEMP > 38 && CampoTEMP < 40) {
                valorTEMP = 1;
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) + 1);
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) + 1;
            }
            if (CampoTEMP > 39) {
                valorTEMP = 2;
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) + 2);
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) + 2;
            }
            if (CampoTEMP == "") {
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) - valorTEMP);
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) - valorTEMP;
                valorTEMP = 0;
            }

            if ($("[id*=txtPuntajeTotalExamFisico-]").val() > 4) { //si es mayor a 4
                $("[id*=txtRiesgoClinicoExamFisico-]").val("MEDIO - ALTO");
            } else {
                $("[id*=txtRiesgoClinicoExamFisico-]").val("BAJO");
            }
            if (valorFR == 3 || valorSAT == 3 || valorTEMP == 3 || valorFC == 3) {
                $("[id*=txtRiesgoClinicoExamFisico-]").val("MEDIO - ALTO");
            }

            $("[id*=txtPuntajeTotalExamFisico-]").removeAttr("disabled");
            $("[id*=txtPuntajeTotalExamFisico-]").trigger("blur");
            $("[id*=txtPuntajeTotalExamFisico-]").attr("disabled", "disabled");

            $("[id*=txtRiesgoClinicoExamFisico-]").removeAttr("disabled");
            $("[id*=txtRiesgoClinicoExamFisico-]").trigger("blur");
            $("[id*=txtRiesgoClinicoExamFisico-]").attr("disabled", "disabled");
        });


        $("[id*=divContenedorDinamico]").find("[id*=txt_fc-]").blur(function () {
            if ($("[id*=divContenedorDinamico]").find("[id*=txt_fc-]").val().trim() == "" && $("[id*=divContenedorDinamico]").find("[id*=txt_temp-]").val().trim() == "" && $("[id*=divContenedorDinamico]").find("[id*=txt_sat-]").val().trim() == "" && $("[id*=divContenedorDinamico]").find("[id*=txt_fr-]").val().trim() == "") {
                $("[id*=txtPuntajeTotalExamFisico-]").val("");
                $("[id*=txtRiesgoClinicoExamFisico-]").val("");

                $("[id*=txtPuntajeTotalExamFisico-]").removeAttr("disabled");
                $("[id*=txtPuntajeTotalExamFisico-]").trigger("blur");
                $("[id*=txtPuntajeTotalExamFisico-]").attr("disabled", "disabled");

                $("[id*=txtRiesgoClinicoExamFisico-]").removeAttr("disabled");
                $("[id*=txtRiesgoClinicoExamFisico-]").trigger("blur");
                $("[id*=txtRiesgoClinicoExamFisico-]").attr("disabled", "disabled");
                valorTEMP = 0; valorFR = 0; valorSAT = 0; valorFC = 0;
                return false;
            }



            var CampoFC = $(this).val();
            var CampoPuntajeTotal = "";
            CampoPuntajeTotal = $("[id*=txtPuntajeTotalExamFisico-]").val().trim();
            if (CampoPuntajeTotal == "") {
                CampoPuntajeTotal = 0
            }

            if (valorFC != 0) {
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) - valorFC); //limpiando el valor en puntaje si es que lo tiene
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) - valorFC;
            }

            if (CampoFC <= 40) {
                valorFC = 3;
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) + 3);
                $("[id*=txtRiesgoClinicoExamFisico-]").val("MEDIO - ALTO");
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) + 3;
            }
            if (CampoFC > 40 && CampoFC < 51) {
                valorFC = 1;
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) + 1);
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) + 1;
            }
            if (CampoFC > 50 && CampoFC < 91) {
                valorFC = 0;
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) + 0);
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) + 0;
            }
            if (CampoFC > 90 && CampoFC < 111) {
                valorFC = 1;
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) + 1);
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) + 1;
            }
            if (CampoFC > 110 && CampoFC < 131) {
                valorFC = 2;
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) + 2);
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) + 2;
            }
            if (CampoFC >= 131) {
                valorFC = 3;
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) + 3);
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) + 3;
            }
            if (CampoFC == "") {
                $("[id*=txtPuntajeTotalExamFisico-]").val(parseInt(CampoPuntajeTotal) - valorFC);
                CampoPuntajeTotal = parseInt(CampoPuntajeTotal) - valorFC;
                valorFC = 0;
            }

            if ($("[id*=txtPuntajeTotalExamFisico-]").val() > 4) { //si es mayor a 4
                $("[id*=txtRiesgoClinicoExamFisico-]").val("MEDIO - ALTO");
            } else {
                $("[id*=txtRiesgoClinicoExamFisico-]").val("BAJO");
            }
            if (valorFR == 3 || valorSAT == 3 || valorTEMP == 3 || valorFC == 3) {
                $("[id*=txtRiesgoClinicoExamFisico-]").val("MEDIO - ALTO");
            }

            $("[id*=txtPuntajeTotalExamFisico-]").removeAttr("disabled");
            $("[id*=txtPuntajeTotalExamFisico-]").trigger("blur");
            $("[id*=txtPuntajeTotalExamFisico-]").attr("disabled", "disabled");

            $("[id*=txtRiesgoClinicoExamFisico-]").removeAttr("disabled");
            $("[id*=txtRiesgoClinicoExamFisico-]").trigger("blur");
            $("[id*=txtRiesgoClinicoExamFisico-]").attr("disabled", "disabled");
            
        });

    }

    function fn_CalculaPews() {
        fn_SumaPuntajePews();

        //$("#Opt_NormalComportamientoPes-692").prop("checked", false);
        //$("#Opt_NormalRespiratorioPews-700").prop("checked", false);

        $("[id*=Opt_NormalComportamientoPes-], [id*=Opt_TendenciaSuenoComportamientoPews-], [id*=Opt_IrritableComportamientoPews-], [id*=Opt_LetargicoConfusoComportamientoPews-]").click(function () {
            fn_SumaPuntajePews();
        });
        $("[id*=Opt_RosaCardiovascularPews-], [id*=Opt_PalidoCardiovascularPews-], [id*=Opt_Gris4CardiovascularPews-], [id*=Opt_Gris5CardiovascularPews-]").click(function () {
            fn_SumaPuntajePews();
        });
        $("[id*=Opt_NormalRespiratorioPews-], [id*=Opt_FR10RespiratorioPews-], [id*=Opt_FR20RespiratorioPews-], [id*=Opt_FR30RespiratorioPews-]").click(function () {
            fn_SumaPuntajePews();
        });
        $("[id*=Opt_NuloNebulizadorInhaladorPews-], [id*=Opt_Cada15NebulizadorInhaladorPews-]").click(function () {
            fn_SumaPuntajePews();
        });
        $("[id*=Opt_NuloVomitoPostPews-], [id*=Opt_VomitoPersistenteVomitoPostPews-]").click(function () {
            fn_SumaPuntajePews();
        });


        /*$("#Opt_NormalComportamientoPes-692,#Opt_TendenciaSuenoComportamientoPews-693,#Opt_IrritableComportamientoPews-694,#Opt_LetargicoConfusoComportamientoPews-695").click(function () {
            fn_SumaPuntajePews();
        });

        $("#Opt_RosaCardiovascularPews-696,#Opt_PalidoCardiovascularPews-697,#Opt_Gris4CardiovascularPews-698,#Opt_Gris5CardiovascularPews-699").click(function () {
            fn_SumaPuntajePews();
        });

        $("#Opt_NormalRespiratorioPews-700,#Opt_FR10RespiratorioPews-701,#Opt_FR10RespiratorioPews-702,#Opt_FR30RespiratorioPews-703").click(function () {
            fn_SumaPuntajePews();
        });

        $("#Opt_NuloNebulizadorInhaladorPews-704,#Opt_Cada15NebulizadorInhaladorPews-705").click(function () {
            fn_SumaPuntajePews();
        });
        $("#Opt_NuloVomitoPostPews-706,#Opt_VomitoPersistenteVomitoPostPews-707").click(function () {
            fn_SumaPuntajePews();
        });*/

    }

    function fn_SumaPuntajePews() {
        var PuntajeComportamiento = 0;
        var PUntajeCardiovascular = 0;
        var PUntajeRespiratorio = 0;
        var PUntajeNebulizador = 0;
        var PUntajeVomito = 0;
        var PuntajePews = 0;


        if ($("[id*=Opt_NormalComportamientoPes-]").prop("checked")) {
            PuntajeComportamiento = 0;
        }
        if ($("[id*=Opt_TendenciaSuenoComportamientoPews-]").prop("checked")) {
            PuntajeComportamiento = 1;
        }
        if ($("[id*=Opt_IrritableComportamientoPews-]").prop("checked")) {
            PuntajeComportamiento = 2;
        }
        if ($("[id*=Opt_LetargicoConfusoComportamientoPews-]").prop("checked")) {
            PuntajeComportamiento = 3;
        }


        if ($("[id*=Opt_RosaCardiovascularPews-]").prop("checked")) {
            PUntajeCardiovascular = 0;
        }
        if ($("[id*=Opt_PalidoCardiovascularPews-]").prop("checked")) {
            PUntajeCardiovascular = 1;
        }
        if ($("[id*=Opt_Gris4CardiovascularPews-]").prop("checked")) {
            PUntajeCardiovascular = 2;
        }
        if ($("[id*=Opt_Gris5CardiovascularPews-]").prop("checked")) {
            PUntajeCardiovascular = 3;
        }


        if ($("[id*=Opt_NormalRespiratorioPews-]").prop("checked")) {
            PUntajeRespiratorio = 0;
        }
        if ($("[id*=Opt_FR10RespiratorioPews-]").prop("checked")) {
            PUntajeRespiratorio = 1;
        }
        if ($("[id*=Opt_FR20RespiratorioPews-]").prop("checked")) {
            PUntajeRespiratorio = 2;
        }
        if ($("[id*=Opt_FR30RespiratorioPews-]").prop("checked")) {
            PUntajeRespiratorio = 3;
        }

        if ($("[id*=Opt_NuloNebulizadorInhaladorPews-]").prop("checked")) {
            PUntajeNebulizador = 0;
        }
        if ($("[id*=Opt_Cada15NebulizadorInhaladorPews-]").prop("checked")) {
            PUntajeNebulizador = 2;
        }


        if ($("[id*=Opt_NuloVomitoPostPews-]").prop("checked")) {
            PUntajeVomito = 0;
        }
        if ($("[id*=Opt_VomitoPersistenteVomitoPostPews-]").prop("checked")) {
            PUntajeVomito = 2;
        }


        PuntajePews = PuntajeComportamiento + PUntajeCardiovascular + PUntajeRespiratorio + PUntajeNebulizador + PUntajeVomito;



        $("[id*=txtPuntajeTotalExamFisicoPews-]").val(PuntajePews);
        if (PuntajePews == 0) {
            $("[id*=txtFrecuenciaEvaluacionExamFisicoPews-]").val("");
            $("[id*=txtPuntajeTotalExamFisicoPews-]").val("");
        }
        if (PuntajePews == 1) {
            $("[id*=txtFrecuenciaEvaluacionExamFisicoPews-]").val("4 horas");
        }
        if (PuntajePews == 2) {
            $("[id*=txtFrecuenciaEvaluacionExamFisicoPews-]").val("2 - 4 horas");
        }
        if (PuntajePews == 3) {
            $("[id*=txtFrecuenciaEvaluacionExamFisicoPews-]").val("1 hora");
        }
        if (PuntajePews == 4 || PuntajePews == 5) {
            $("[id*=txtFrecuenciaEvaluacionExamFisicoPews-]").val("30 minutos");
        }
        if (PuntajePews == 6) {
            $("[id*=txtFrecuenciaEvaluacionExamFisicoPews-]").val("Continuo");
        }
        if (PuntajePews >= 7) {
            $("[id*=txtFrecuenciaEvaluacionExamFisicoPews-]").val("Continuo");
        }

        $("[id*=txtPuntajeTotalExamFisicoPews-]").removeAttr("disabled");
        $("[id*=txtPuntajeTotalExamFisicoPews-]").trigger("blur");
        $("[id*=txtPuntajeTotalExamFisicoPews-]").attr("disabled", "disabled");

        $("[id*=txtFrecuenciaEvaluacionExamFisicoPews-]").removeAttr("disabled");
        $("[id*=txtFrecuenciaEvaluacionExamFisicoPews-]").trigger("blur");
        $("[id*=txtFrecuenciaEvaluacionExamFisicoPews-]").attr("disabled", "disabled");
    }



//    function fn_SumaPuntajePews() {
//        var PuntajePews = 0;        

//        if ($("#Opt_TendenciaSuenoComportamientoPews-693").prop("checked")) {
//            PuntajePews += 1;
//        }
//        if ($("#Opt_PalidoCardiovascularPews-697").prop("checked")) {
//            PuntajePews += 1;
//        }
//        if ($("#Opt_FR10RespiratorioPews-701").prop("checked")) {
//            PuntajePews += 1;
//        }


//        if ($("#Opt_IrritableComportamientoPews-694").prop("checked")) {
//            PuntajePews += 2;
//        }
//        if ($("#Opt_Gris4CardiovascularPews-698").prop("checked")) {
//            PuntajePews += 2;
//        }
//        if ($("#Opt_FR10RespiratorioPews-702").prop("checked")) {
//            PuntajePews += 2;
//        }
//        if ($("#Opt_Cada15NebulizadorInhaladorPews-705").prop("checked")) {
//            PuntajePews += 2;
//        }
//        if ($("#Opt_VomitoPersistenteVomitoPostPews-707").prop("checked")) {
//            PuntajePews += 2;
//        }


//        if ($("#Opt_LetargicoConfusoComportamientoPews-695").prop("checked")) {
//            PuntajePews += 3;
//        }
//        if ($("#Opt_Gris5CardiovascularPews-699").prop("checked")) {
//            PuntajePews += 3;
//        }
//        if ($("#Opt_FR30RespiratorioPews-703").prop("checked")) {
//            PuntajePews += 3;
//        }

//        $("#txtPuntajeTotalExamFisicoPews-831").val(PuntajePews);
//        if (PuntajePews == 0) {
//            $("#txtFrecuenciaEvaluacionExamFisicoPews-832").val("");
//            $("#txtPuntajeTotalExamFisicoPews-831").val("");
//        }
//        if (PuntajePews == 1) {
//            $("#txtFrecuenciaEvaluacionExamFisicoPews-832").val("4 horas");
//        }
//        if (PuntajePews == 2) {
//            $("#txtFrecuenciaEvaluacionExamFisicoPews-832").val("2 - 4 horas");
//        }
//        if (PuntajePews == 3) {
//            $("#txtFrecuenciaEvaluacionExamFisicoPews-832").val("1 hora");
//        }
//        if (PuntajePews == 4 || PuntajePews == 5) {
//            $("#txtFrecuenciaEvaluacionExamFisicoPews-832").val("30 minutos");
//        }
//        if (PuntajePews == 6) {
//            $("#txtFrecuenciaEvaluacionExamFisicoPews-832").val("Continuo");
//        }
//        if (PuntajePews >= 7) {
//            $("#txtFrecuenciaEvaluacionExamFisicoPews-832").val("Continuo");
//        }

//        $("#txtPuntajeTotalExamFisicoPews-831").removeAttr("disabled");
//        $("#txtPuntajeTotalExamFisicoPews-831").trigger("blur");
//        $("#txtPuntajeTotalExamFisicoPews-831").attr("disabled", "disabled");

//        $("#txtFrecuenciaEvaluacionExamFisicoPews-832").removeAttr("disabled");
//        $("#txtFrecuenciaEvaluacionExamFisicoPews-832").trigger("blur");
//        $("#txtFrecuenciaEvaluacionExamFisicoPews-832").attr("disabled", "disabled");
//    }

    function fn_CalcularStopBang(IdControl, valor, CampoRiesgo) {
        var ValorPuntaje = 0;

        $("input[id^='" + IdControl + "']").parent().parent().parent().parent().find("input[id*='Si']").each(function () {
            var IdCtrl = $(this).attr("id");
            var valorCtrl = 0;
            if ($("input[id^='" + IdCtrl + "']").prop("checked")) {
                //Observaciones Cmendez 02/05/2022
                valorCtrl = $("input[id^='" + IdCtrl + "']").attr("onchange").split(",")[1];
                ValorPuntaje = parseInt(ValorPuntaje) + parseInt(valorCtrl.replace(/"/g, ""));
            }
        });


        if (ValorPuntaje > 0 && ValorPuntaje < 3) {
            $("input[id^='" + CampoRiesgo + "']").val("Bajo riesgo de AOS (Apnea Obstructiva del Sueño)");
        }
        if (ValorPuntaje == 3 || ValorPuntaje == 4) {
            $("input[id^='" + CampoRiesgo + "']").val("Riesgo intermedio de AOS (Apnea Obstructiva del Sueño)");
        }
        if (ValorPuntaje >= 5) {
            $("input[id^='" + CampoRiesgo + "']").val("Alto riesgo de AOS (Apnea Obstructiva del Sueño)");
        }        


        $("input[id^='" + CampoRiesgo + "']").removeAttr("disabled");
        $("input[id^='" + CampoRiesgo + "']").trigger("blur");
        $("input[id^='" + CampoRiesgo + "']").attr("disabled", "disabled");
    }

    function fn_CalcularCaprini(IdControl, valor, CampoPuntaje, CampoRiesgo, CampoRecomendacion) {
        /*var ValorPuntaje = $("input[id^='" + CampoPuntaje + "']").val().trim();
        if (ValorPuntaje == "") {            
            ValorPuntaje = 0;
        }*/

        /*if ($("input[id^='" + IdControl + "']").prop("checked")) {            
            $("input[id^='" + CampoPuntaje + "']").val(parseInt(ValorPuntaje) + parseInt(valor));
        } else {
            if (ValorPuntaje != 0) {
                $("input[id^='" + CampoPuntaje + "']").val(parseInt(ValorPuntaje) - parseInt(valor));            
            }
        }*/

        var ValorPuntaje = "";
        $("input[id^='" + IdControl + "']").parent().parent().parent().parent().parent().parent().find("input[type='checkbox']").each(function () {
            var CampoCheck = $(this).attr("id");
            var valorCtrl = 0;
            if ($(this).prop("checked")) {  //.indexOf(cadena) !== -1
                if (ValorPuntaje == "") {
                    ValorPuntaje = 0;
                }
                /*//Observaciones Cmendez 02/05/2022*/
                valorCtrl = $("#" + CampoCheck).attr("onchange").split(",")[1];
                ValorPuntaje = parseInt(ValorPuntaje) + parseInt(valorCtrl.replace(/"/g, ""));
            }
        });        
        $("input[id^='" + CampoPuntaje + "']").val(ValorPuntaje);
        
        if ($("input[id^='" + CampoPuntaje + "']").val() == 0) {
            $("input[id^='" + CampoRiesgo + "']").val("MUY BAJO");
            $("input[id^='" + CampoRecomendacion + "']").val("Deambulación");
        }
        if ($("input[id^='" + CampoPuntaje + "']").val() == 1 || $("input[id^='" + CampoPuntaje + "']").val() == 2) {
            $("input[id^='" + CampoRiesgo + "']").val("BAJO");
            $("input[id^='" + CampoRecomendacion + "']").val("Media compresivas");
        }
        if ($("input[id^='" + CampoPuntaje + "']").val() == 3 || $("input[id^='" + CampoPuntaje + "']").val() == 4) {
            $("input[id^='" + CampoRiesgo + "']").val("MODERADO");
            $("input[id^='" + CampoRecomendacion + "']").val("Medias compresivas + HBPM");
        }
        if ($("input[id^='" + CampoPuntaje + "']").val() > 4 && $("input[id^='" + CampoPuntaje + "']").val() < 9) {
            $("input[id^='" + CampoRiesgo + "']").val("ALTO");
            $("input[id^='" + CampoRecomendacion + "']").val("HBPM + compresión neumática - Ortopedica rivaroxaban 10g");
        }
        if ($("input[id^='" + CampoPuntaje + "']").val() > 8) {
            $("input[id^='" + CampoRiesgo + "']").val("MUY ALTO");
            $("input[id^='" + CampoRecomendacion + "']").val("HBPM + Media compresivas y Compresión neumatica");
        }
        if ($("input[id^='" + CampoPuntaje + "']").val() == "") {
            $("input[id^='" + CampoRiesgo + "']").val("");
            $("input[id^='" + CampoRecomendacion + "']").val("");
        }

        $("input[id^='" + CampoPuntaje + "']").removeAttr("disabled");
        $("input[id^='" + CampoPuntaje + "']").trigger("blur");
        $("input[id^='" + CampoPuntaje + "']").attr("disabled", "disabled");

        $("input[id^='" + CampoRiesgo + "']").removeAttr("disabled");
        $("input[id^='" + CampoRiesgo + "']").trigger("blur");
        $("input[id^='" + CampoRiesgo + "']").attr("disabled", "disabled");

        $("input[id^='" + CampoRecomendacion + "']").removeAttr("disabled");
        $("input[id^='" + CampoRecomendacion + "']").trigger("blur");
        $("input[id^='" + CampoRecomendacion + "']").attr("disabled", "disabled");
    }

    function fn_CalcularPadua(IdControl, valor, CampoPuntaje, CampoInterpretacion) {
        var ValorPuntaje = 0;

        $("input[id^='" + IdControl + "']").parent().parent().parent().find("input[id*='Si']").each(function () {
            var IdCtrl = $(this).attr("id");
            var valorCtrl = 0;
            if ($("input[id^='" + IdCtrl + "']").prop("checked")) {
               /* //Observaciones Cmendez 02/05/2022*/
                valorCtrl = $("input[id^='" + IdCtrl + "']").attr("onchange").split(",")[1];                
                ValorPuntaje = parseInt(ValorPuntaje) + parseInt(valorCtrl.replace(/"/g, ""));
            }
        });

        if (ValorPuntaje < 4) {
            $("input[id^='" + CampoPuntaje + "']").val(ValorPuntaje);
            $("input[id^='" + CampoInterpretacion + "']").val("BAJO RIESGO");            
        } else {
            $("input[id^='" + CampoPuntaje + "']").val(ValorPuntaje);
            $("input[id^='" + CampoInterpretacion + "']").val("ALTO RIESGO");
        }
        if (ValorPuntaje == 0) {
            $("input[id^='" + CampoPuntaje + "']").val("");
            $("input[id^='" + CampoInterpretacion + "']").val("");
        }
        


        $("input[id^='" + CampoPuntaje + "']").removeAttr("disabled");
        $("input[id^='" + CampoPuntaje + "']").trigger("blur");
        $("input[id^='" + CampoPuntaje + "']").attr("disabled", "disabled");

        $("input[id^='" + CampoInterpretacion + "']").removeAttr("disabled");
        $("input[id^='" + CampoInterpretacion + "']").trigger("blur");
        $("input[id^='" + CampoInterpretacion + "']").attr("disabled", "disabled");
    }

    function fn_CalcularKhorana(IdControl, valor, CampoPuntaje, CampoRiesgo) {
        var ValorPuntaje = 0;

        $("input[id^='" + IdControl + "']").parent().parent().parent().parent().find("input[id*='Si']").each(function () {
            var IdCtrl = $(this).attr("id");
            var valorCtrl = 0;
            if ($("input[id^='" + IdCtrl + "']").prop("checked")) {
                //Observaciones Cmendez 02/05/2022
                valorCtrl = $("input[id^='" + IdCtrl + "']").attr("onchange").split(",")[1];
                ValorPuntaje = parseInt(ValorPuntaje) + parseInt(valorCtrl.replace(/"/g, ""));
            }
        });

        if (ValorPuntaje == 0) {
            $("input[id^='" + CampoPuntaje + "']").val(ValorPuntaje);
            $("input[id^='" + CampoRiesgo + "']").val("BAJO RIESGO");
        }
        if (ValorPuntaje == 1 || ValorPuntaje == 2) {
            $("input[id^='" + CampoPuntaje + "']").val(ValorPuntaje);
            $("input[id^='" + CampoRiesgo + "']").val("INTERMEDIO");
        }
        if (ValorPuntaje >= 3) {
            $("input[id^='" + CampoPuntaje + "']").val(ValorPuntaje);
            $("input[id^='" + CampoRiesgo + "']").val("ALTO RIESGO");
        }        


        $("input[id^='" + CampoPuntaje + "']").removeAttr("disabled");
        $("input[id^='" + CampoPuntaje + "']").trigger("blur");
        $("input[id^='" + CampoPuntaje + "']").attr("disabled", "disabled");

        $("input[id^='" + CampoRiesgo + "']").removeAttr("disabled");
        $("input[id^='" + CampoRiesgo + "']").trigger("blur");
        $("input[id^='" + CampoRiesgo + "']").attr("disabled", "disabled");
    }

    function fn_CalcularKatz(IdControl, IndiceKatz, ValoracionKatz) {
        var Marcado = "";

        $("input[id^='" + IdControl + "']").parent().parent().parent().parent().find("input[type='radio']").each(function () {
            var IdCtrlMarcado = $(this).attr("id");
            if ($("#" + IdCtrlMarcado).prop("checked")) {
                Marcado += IdCtrlMarcado;
            }
        });

        if (Marcado.indexOf("Opt_SeBanaSoloPrecisa") != -1 && Marcado.indexOf("Opt_SacaRopaCajones") != -1 && Marcado.indexOf("Opt_VaWCSolo") != -1
            && Marcado.indexOf("Opt_SeLevantaAcuesta") != -1 && Marcado.indexOf("Opt_ControlCompletoMiccion") != -1 && Marcado.indexOf("Opt_LlevaAlimentoBoca") != -1) {
            $("input[id^='" + IndiceKatz + "']").val("A");
            $("input[id^='" + ValoracionKatz + "']").val("Independiente en alimentación, continencia, movilidad, uso del retrete, vestirse y bañarse");
        }

        if (Marcado.indexOf("Opt_PrecisaAyudaParaLavar") != -1 || Marcado.indexOf("Opt_NoVisteSiMismo") != -1 || Marcado.indexOf("Opt_PrecisaAyudaWC") != -1
            || Marcado.indexOf("Opt_PrecisaAyudaLevantarse") != -1 || Marcado.indexOf("Opt_IncontinenciaParialTotal") != -1 || Marcado.indexOf("Opt_PrecisaAyudaComer") != -1) {
            $("input[id^='" + IndiceKatz + "']").val("B");
            $("input[id^='" + ValoracionKatz + "']").val("Independiente para todas las funciones anteriores excepto una");
        }

        if (Marcado.indexOf("Opt_PrecisaAyudaParaLavar") != -1) {
            if (Marcado.indexOf("Opt_NoVisteSiMismo") != -1 || Marcado.indexOf("Opt_PrecisaAyudaWC") != -1
            || Marcado.indexOf("Opt_PrecisaAyudaLevantarse") != -1 || Marcado.indexOf("Opt_IncontinenciaParialTotal") != -1 || Marcado.indexOf("Opt_PrecisaAyudaComer") != -1) {
                $("input[id^='" + IndiceKatz + "']").val("C");
                $("input[id^='" + ValoracionKatz + "']").val("Independiente para todas, excepto bañarse y otra funcion adicional");
            }
        }

        if (Marcado.indexOf("Opt_PrecisaAyudaParaLavar") != -1 && Marcado.indexOf("Opt_NoVisteSiMismo") != -1) {
            if (Marcado.indexOf("Opt_PrecisaAyudaWC") != -1 || Marcado.indexOf("Opt_PrecisaAyudaLevantarse") != -1 || 
                Marcado.indexOf("Opt_IncontinenciaParialTotal") != -1 || Marcado.indexOf("Opt_PrecisaAyudaComer") != -1) {
                $("input[id^='" + IndiceKatz + "']").val("D");
                $("input[id^='" + ValoracionKatz + "']").val("Independiente para todas, excepto bañarse, vestirse y otra funcion adicional");
            }
        }

        if (Marcado.indexOf("Opt_PrecisaAyudaParaLavar") != -1 && Marcado.indexOf("Opt_NoVisteSiMismo") != -1 && Marcado.indexOf("Opt_PrecisaAyudaWC") != -1) {
            if (Marcado.indexOf("Opt_PrecisaAyudaLevantarse") != -1 ||
                Marcado.indexOf("Opt_IncontinenciaParialTotal") != -1 || Marcado.indexOf("Opt_PrecisaAyudaComer") != -1) {
                $("input[id^='" + IndiceKatz + "']").val("E");
                $("input[id^='" + ValoracionKatz + "']").val("Independiente para todas, excepto bañarse, vestirse, uso del retrete y otra funcion adicional");
            }
        }

        if (Marcado.indexOf("Opt_PrecisaAyudaParaLavar") != -1 && Marcado.indexOf("Opt_NoVisteSiMismo") != -1 && Marcado.indexOf("Opt_PrecisaAyudaWC") != -1 && Marcado.indexOf("Opt_PrecisaAyudaLevantarse") != -1) {
            if (Marcado.indexOf("Opt_IncontinenciaParialTotal") != -1 || Marcado.indexOf("Opt_PrecisaAyudaComer") != -1) {
                $("input[id^='" + IndiceKatz + "']").val("F");
                $("input[id^='" + ValoracionKatz + "']").val("Independiente para todas, excepto bañarse, vestirse, uso del retrete, movilidad y otra funcion adicional");
            }
        }


        if (Marcado.indexOf("Opt_PrecisaAyudaParaLavar") != -1 && Marcado.indexOf("Opt_NoVisteSiMismo") != -1 && Marcado.indexOf("Opt_PrecisaAyudaWC")
            && Marcado.indexOf("Opt_PrecisaAyudaLevantarse") != -1 && Marcado.indexOf("Opt_IncontinenciaParialTotal") != -1 && Marcado.indexOf("Opt_PrecisaAyudaComer") != -1) {
            $("input[id^='" + IndiceKatz + "']").val("G");
            $("input[id^='" + ValoracionKatz + "']").val("Dependiente en las 6 funciones");
        }


        if ($("input[id^='" + IndiceKatz + "']").val() != "C" && $("input[id^='" + IndiceKatz + "']").val() != "D" &&
            $("input[id^='" + IndiceKatz + "']").val() != "E" && $("input[id^='" + IndiceKatz + "']").val() != "F") {
            var ConteoH = 0;
            var array = ["Opt_PrecisaAyudaParaLavar", "Opt_NoVisteSiMismo", "Opt_PrecisaAyudaWC", "Opt_PrecisaAyudaLevantarse", "Opt_IncontinenciaParialTotal", "Opt_PrecisaAyudaComer"];            
            for (var i = 0; i < array.length; i++) {
                var CadenaCtrl = array[i].toString();
                if (Marcado.indexOf(CadenaCtrl.trim()) != -1) {
                    ConteoH += 1;
                }
            }
            if (ConteoH >= 2) {
                $("input[id^='" + IndiceKatz + "']").val("H");
                $("input[id^='" + ValoracionKatz + "']").val("Dependiente en almenos 2 funciones, pero no clasificable como C, D, E o F");
            }
        }


        $("input[id^='" + IndiceKatz + "']").removeAttr("disabled");
        $("input[id^='" + IndiceKatz + "']").trigger("blur");
        $("input[id^='" + IndiceKatz + "']").attr("disabled", "disabled");

        $("input[id^='" + ValoracionKatz + "']").removeAttr("disabled");
        $("input[id^='" + ValoracionKatz + "']").trigger("blur");
        $("input[id^='" + ValoracionKatz + "']").attr("disabled", "disabled");

        /*
        Opt_SeBanaSoloPrecisa
        Opt_PrecisaAyudaParaLavar

        Opt_SacaRopaCajones
        Opt_NoVisteSiMismo

        Opt_VaWCSolo
        Opt_PrecisaAyudaWC

        Opt_SeLevantaAcuesta
        Opt_PrecisaAyudaLevantarse


        Opt_ControlCompletoMiccion
        Opt_IncontinenciaParialTotal

        Opt_LlevaAlimentoBoca
        Opt_PrecisaAyudaComer        
        */
    }

    function fn_DefinicionRankin(ComboRankin, DefinicionRankin) {
        if ($("select[id^='" + ComboRankin + "']").val() == "0") {
            $("input[id^='" + DefinicionRankin + "']").val("");
        }
        if ($("select[id^='" + ComboRankin + "']").val() == "1") {
            $("input[id^='" + DefinicionRankin + "']").val("Puede realizar todas sus obligaciones");
        }
        if ($("select[id^='" + ComboRankin + "']").val() == "2") {
            $("input[id^='" + DefinicionRankin + "']").val("No puede realizar todas las actividades previas, pero puede realizar sus propias cosas sin ayuda");
        }
        if ($("select[id^='" + ComboRankin + "']").val() == "3") {
            $("input[id^='" + DefinicionRankin + "']").val("Requiere ayuda pero es capaz de caminar solo");
        }
        if ($("select[id^='" + ComboRankin + "']").val() == "4") {
            $("input[id^='" + DefinicionRankin + "']").val("No puede caminar sin asistencia. No puede atender sus propias necesidades corporales sin ayuda");
        }
        if ($("select[id^='" + ComboRankin + "']").val() == "5") {
            $("input[id^='" + DefinicionRankin + "']").val("Confinado a la cama. requiere supervision y atencion constante de enfermeria");
        }
        if ($("select[id^='" + ComboRankin + "']").val() == "5") {
            $("input[id^='" + DefinicionRankin + "']").val("");
        }

        $("input[id^='" + DefinicionRankin + "']").removeAttr("disabled");
        $("input[id^='" + DefinicionRankin + "']").trigger("blur");
        $("input[id^='" + DefinicionRankin + "']").attr("disabled", "disabled");
    }

    function fn_MarcarDiferidoAnoRecto(IdControl1, IdControl2, Check1, Campo1, Check2, Campo2, Check3, Campo3) {
        var ValorPuntaje = 0;

        if ($("input[id^='" + IdControl2 + "']").prop("checked") == true) {
            $("input[id^='" + Check1 + "']").prop("checked", true);
            $("input[id^='" + Check2 + "']").prop("checked", true);
            $("input[id^='" + Check3 + "']").prop("checked", true);
            $("input[id^='" + Check1 + "']").trigger("blur");
            $("input[id^='" + Check2 + "']").trigger("blur");
            $("input[id^='" + Check3 + "']").trigger("blur");

            $("input[id^='" + Campo1 + "']").removeAttr("disabled");
            $("input[id^='" + Campo2 + "']").removeAttr("disabled");
            $("input[id^='" + Campo3 + "']").removeAttr("disabled");
        } else {
            $("input[id^='" + Check1 + "']").prop("checked", false);
            $("input[id^='" + Check2 + "']").prop("checked", false);
            $("input[id^='" + Check3 + "']").prop("checked", false);
            $("input[id^='" + Check1 + "']").trigger("blur");
            $("input[id^='" + Check2 + "']").trigger("blur");
            $("input[id^='" + Check3 + "']").trigger("blur"); 

            $("input[id^='" + Campo1 + "']").attr("disabled", "disabled");
            $("input[id^='" + Campo2 + "']").attr("disabled", "disabled");
            $("input[id^='" + Campo3 + "']").attr("disabled", "disabled");
        }
    }

    function fn_MarcarBabinsky(IdControl1, Radio1, Radio2, Radio3) {  
        if ($("input[id^='" + IdControl1 + "']").prop("checked") == true) {          
            $("input[id^='" + Radio1 + "']").removeAttr("disabled");
            $("input[id^='" + Radio2 + "']").removeAttr("disabled");
            $("input[id^='" + Radio3 + "']").removeAttr("disabled");
        } else {
            $("input[id^='" + Radio1 + "']").attr("disabled", "disabled");
            $("input[id^='" + Radio2 + "']").attr("disabled", "disabled");
            $("input[id^='" + Radio3 + "']").attr("disabled", "disabled");
        }
    }    

    /*
     "Opt_SiDiferidoAnoRecto","Opt_NoDiferidoAnoRecto","Opt_SiEsfinterAnoRecto","txt_DescripcionEsfinterAnoRecto",
"Opt_SiLesionesAnoRecto","txt_DescripcionLesionesAnoRecto","Opt_SiProstataAnoRecto","txt_DescripcionProstataAnoRecto"
     */


    /*FUNCION QUE CARGA LOS CONTROLES DINAMICOS Y LOS DATOS QUE ELLOS CONTENGAN*/
    function fn_CargandoDatosControlesGrupo(IdExamenFisico, IdeHistoria, TipoAtencion) {
        var atributo = ""
        atributo = $("#" + IdExamenFisico).attr("name");

        $.ajax({
            url: "InformacionPaciente.aspx/CargarDatosControlesDinamicoGrupo",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                IdeExamenFisico: IdExamenFisico,
                CantidadColumnas: atributo,
                IdeHistoria: IdeHistoria,
                TipoDeAtencion: TipoAtencion
            }),
            dataType: "json",
            error: function (dato1, datos2, dato3) {

            }
        }).done(function (oOB_JSON) {
            $("#" + IdExamenFisico).parent().parent().after("<div class='JGRUPO-CTRL'>" + oOB_JSON.d + "</div>");
            fn_RenderizarTabs();
            fn_CrearEventoControles();
            $(".JCAMP-D").attr("disabled", "disabled"); // JB - NUEVO - 15/04/2019            
            
            fn_MostraNewsPews();

            //INICIO - JB - 23/07/2019
            if ($("input[id^='Opt_SiDiferidoAnoRecto']").prop("checked")) {
                $("input[id^='txt_DescripcionEsfinterAnoRecto']").removeAttr("disabled");
                $("input[id^='txt_DescripcionLesionesAnoRecto']").removeAttr("disabled");
                $("input[id^='txt_DescripcionProstataAnoRecto']").removeAttr("disabled");
            }
            //babinsky            
            if ($("input[id^='chk_babinskyD']").prop("checked") == false) {
                $("input[id^='Opt_PositivoBabinskyDerecha']").attr("disabled", "disabled");
                $("input[id^='Opt_NegativoBabinskyDerecha']").attr("disabled", "disabled");
                $("input[id^='Opt_IndiferenteBabinskyDerecha']").attr("disabled", "disabled");
            }
            if ($("input[id^='chk_babinskyI']").prop("checked") == false) {
                $("input[id^='Opt_PositivoBabinskyIzquierda']").attr("disabled", "disabled");
                $("input[id^='Opt_NegativoBabinskyIzquierda']").attr("disabled", "disabled");
                $("input[id^='Opt_IndiferenteBabinskyIzquierda']").attr("disabled", "disabled");
            }
            //FIN - JB - 23/07/2019

            //CONDICION PARA HABILITAR/DESHABILITAR CONTROLES SI SE HA SELECCIONADO UNA ATENCION ANTERIOR
            if (sAtencionAnterior != "") {
                if (sAtencionAnterior != sCodigoAtencionActual) {
                    fn_DeshabilitaControlesI(); //FUNCION PARA DESHABILITAR TODOS LOS CONTROLES DE LA PANTALLA
                } else {
                    location.reload();
                }
            } else {
                fn_CampoBase();
            }
        });
    }

    /*FUNCION PARA VALIDACION DE LOS CAMPOS (EN CASO TENGA ALGUNA)*/
    function fn_ValidaControl(CantidadMinima, CantidadMaxima, MensajeValidacion, Control) {        
        if ($("#" + Control).val().trim() != "") {
            if ($("#" + Control).val() < CantidadMinima || $("#" + Control).val() > CantidadMaxima) {
                sIdControlValidado = Control;
                $.JMensajePOPUP("Validación", MensajeValidacion, "1", "Aceptar", "fn_FocusControlValidado()");
            }
        }        
    }
    function fn_FocusControlValidado() {
        $("#" + sIdControlValidado).val("");
        $("#" + sIdControlValidado).focus();
        fn_oculta_mensaje();
    }

    function fn_CargarAtencionesAnteriores() {
        $.ajax({
            url: "InformacionPaciente.aspx/CargarAtencionesAnteriores",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (dato1, datos2, dato3) {

            }
        }).done(function (oOB_JSON) {
            /*CREANDO ESTRUCTURA DE ATENCIONES ANTERIORES*/
            var sCA_HTML = "<ul style='list-style-type:none;min-width:780px;max-width:900px;margin-top:5px;'>";
            var sEstiloCursor = "";
            for (var i = 0; i < oOB_JSON.d.length - 1; i++) {
                var NombreImagen = "";
                if (oOB_JSON.d[i].substring(oOB_JSON.d[i].indexOf("_") + 1, oOB_JSON.d[i].length) == "0") {
                    NombreImagen = "S_HC";
                    sEstiloCursor = "cursor:default;";
                } else {
                    NombreImagen = "Datos_Atencion_Medico";
                    sEstiloCursor = "cursor:pointer;";
                }

                sCA_HTML = sCA_HTML + "<li style='margin-left:-30px;font-size:12px;font-family:Arial;color:#134B8D;'> " +
                            "<a style='" + sEstiloCursor + "'>" +
                            "<img alt='' src='../Imagenes/" + NombreImagen + ".png' style='padding-right:10px;vertical-align:middle;' />" +
                            "<span>" + oOB_JSON.d[i].substring(0, oOB_JSON.d[i].indexOf("_")) + "</span>" +
                            "<input type='hidden' value='" + oOB_JSON.d[i].substring(oOB_JSON.d[i].indexOf("_") + 1, oOB_JSON.d[i].length) + "' />" +
                            "</a></li>";
            }
            var sCA_HTML = sCA_HTML + "</ul>";
            $("#divAtencionesAnteriores").append(sCA_HTML);

            /*CREANDO EVENTO CLICK EN ATENCIONES ANTERIORES QUE CARGARA LOS DATOS DE LA ATENCION SELECCIONADA*/
            $("#divAtencionesAnteriores").find("a").click(function () {
                var ObjetoAtencionAnterior = $(this);
                var sIdehistoria = $(this).find("input[type='hidden']").val().trim();
                var sTipoAtencion = $(this).find("span").html().trim().substring(0, 1);
                var sCodigoAtencion = $(this).find("span").html().trim().substring(0, $(this).find("span").html().trim().indexOf("-")).trim();
                sAtencionAnterior = sCodigoAtencion;
                sCodigoAtencionActual = $("#DatosUsuarioInformacionPaciente").find("#spDatosCodigoAtencion").html().trim();

                if (sIdehistoria == "0" && sIdehistoria == "") {
                    return;
                }

                $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "") {
                        fn_LOAD_VISI();
                        $.ajax({
                            url: "InformacionPaciente.aspx/ObtenerCodigoAtencion",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({
                                CodigoAtencion: sCodigoAtencion
                            }),
                            dataType: "json",
                            error: function (dato1, datos2, dato3) {

                            }
                        }).done(function (oOB_JSON) {
                            if (sIdehistoria != "0") {
                                $.ajax({
                                    url: "InformacionPaciente.aspx/ObtenerValoresAtencionAnterior",
                                    type: "POST",
                                    contentType: "application/json; charset=utf-8",
                                    data: JSON.stringify({
                                        TipoDeAtencion: sTipoAtencion,
                                        IdHistoria: sIdehistoria
                                    }),
                                    dataType: "json",
                                    error: function (dato1, datos2, dato3) {

                                    }
                                }).done(function (oOB_JSON) {
                                    $.ajax({
                                        url: "InformacionPaciente.aspx/ControlesDinamicos_2",
                                        type: "POST",
                                        contentType: "application/json; charset=utf-8",
                                        data: JSON.stringify({
                                            TipoAtencion: sTipoAtencion
                                        }),
                                        dataType: "json",
                                        error: function (dato1, datos2, dato3) {

                                        }
                                    }).done(function (oOB_JSON) {
                                        $("[id*=divContenedorDinamico]").html("");
                                        $("[id*=divContenedorDinamico]").append(oOB_JSON.d);
                                        fn_LOAD_OCUL();
                                        //INICIO - JB - NUEVA CONDICION PARA CARGAR LAS ATENCIONES E
                                        if (sTipoAtencion.toString().substring(0, 1) == "E") {
                                            //CONDICION PARA HABILITAR/DESHABILITAR CONTROLES SI SE HA SELECCIONADO UNA ATENCION ANTERIOR                                        

                                            if (sAtencionAnterior != "") {
                                                if (sAtencionAnterior != sCodigoAtencionActual) {
                                                    fn_DeshabilitaControlesI(); //FUNCION PARA DESHABILITAR TODOS LOS CONTROLES DE LA PANTALLA                                                    
                                                } else {
                                                    location.reload();
                                                }
                                            } else {
                                                fn_CampoBase();
                                            }
                                            $(".JCHEK-TABS").removeAttr("disabled");
                                            $(".JCHEK-TABS").click(function () {
                                                $(this).parent().find("> .JCONTENIDO-TAB").css("display", "none");
                                                $(this).parent().find("> .JCONTENIDO-TAB").eq($(this).parent().find(".JCHEK-TABS").index(this)).css("display", "block");
                                            });
                                            //marcando el primer tab
                                            $(".JTABS").each(function () {
                                                $(this).find("> .JCHEK-TABS").eq(0).click();
                                            });

                                        } else {
                                            /*JB - 20/06/2020 - se comenta esta linea y se usa las lineas de abajo para que refresque pantalla
                                            $("[id*=divContenedorDinamico]").find(".JDIV-GRUPO").each(function () {
                                            fn_CargandoDatosControlesGrupo($(this).prop("id").trim(), sIdehistoria, sTipoAtencion);
                                            });*/
                                            //                                            fn_LOAD_VISI(); 30/10/2020
                                            if (sAtencionAnterior != "") {
                                                if (sAtencionAnterior != sCodigoAtencionActual) {
                                                    fn_DeshabilitaControlesI(); //FUNCION PARA DESHABILITAR TODOS LOS CONTROLES DE LA PANTALLA
                                                } else {
                                                    fn_LOAD_VISI();
                                                    location.reload();
                                                }
                                            } else {
                                                fn_CampoBase();
                                            }
                                        }
                                        //FIN - JB - NUEVA CONDICION PARA CARGAR LAS ATENCIONES E

                                        fn_RenderizarTabs();
                                        //$(".DatosUsuario").load("Utilidad/DatosUsuario.aspx"); //CARGANDO DATOS EN LA PARTE SUPERIOR CON LA NUEVA ATENCION SELECCIONADA

                                        /*RECARGANDO TODO EL CONTENIDO DE LA PAGINA*/
                                        fn_CargaNotaIngreso();
                                        //fn_CargarEvolucionClinica();
                                        fn_CargarEvolucionClinica2("1", "0");
                                        //fn_CargarControlClinicoIM();
                                        fn_CargarControlClinicoIM2("1", "", "0");
                                        $("#divGridDiagnosticos").load("GridViewAjax/GridDiagnostico.aspx", function () {
                                        });
                                        $("#divGridLaboratorio").load("GridViewAjax/GridLaboratorio.aspx", function () {
                                        });
                                        //fn_CrearTreeViewAnalisis();
                                        fn_CrearTreeViewAnalisis2("1", "", "0");
                                        $("#divGridImagen").load("GridViewAjax/GridImagen.aspx", function () {
                                        });
                                        //fn_CrearTreeViewImagenes();
                                        fn_CrearTreeViewImagenes2("1", "", "0");
                                        fn_CargarPatologias();

                                        if (sAtencionAnterior != "") {
                                            if (sAtencionAnterior != sCodigoAtencionActual) {
                                                $("#divGridInterconsulta").load("GridViewAjax/GridInterconsulta.aspx");
                                            } else {
                                                $("#divGridInterconsulta").load("GridViewAjax/GridInterconsulta.aspx", function () { fn_CargaPermiso(); });
                                            }
                                        }


                                    });
                                });





                                //                                $.ajax({
                                //                                    url: "InformacionPaciente.aspx/ControlesDinamicos_2",
                                //                                    type: "POST",
                                //                                    contentType: "application/json; charset=utf-8",
                                //                                    data: JSON.stringify({
                                //                                        TipoAtencion: sTipoAtencion
                                //                                    }),
                                //                                    dataType: "json",
                                //                                    error: function (dato1, datos2, dato3) {

                                //                                    }
                                //                                }).done(function (oOB_JSON) {
                                //                                    $("[id*=divContenedorDinamico]").html("");
                                //                                    $("[id*=divContenedorDinamico]").append(oOB_JSON.d);
                                //                                    //INICIO - JB - NUEVA CONDICION PARA CARGAR LAS ATENCIONES E
                                //                                    if (sTipoAtencion.toString().substring(0, 1) == "E") {
                                //                                        //CONDICION PARA HABILITAR/DESHABILITAR CONTROLES SI SE HA SELECCIONADO UNA ATENCION ANTERIOR                                        

                                //                                        if (sAtencionAnterior != "") {
                                //                                            if (sAtencionAnterior != sCodigoAtencionActual) {
                                //                                                fn_DeshabilitaControlesI(); //FUNCION PARA DESHABILITAR TODOS LOS CONTROLES DE LA PANTALLA                                                    
                                //                                            } else {
                                //                                                location.reload();
                                //                                            }
                                //                                        } else {
                                //                                            fn_CampoBase();
                                //                                        }
                                //                                        $(".JCHEK-TABS").removeAttr("disabled");
                                //                                        $(".JCHEK-TABS").click(function () {
                                //                                            $(this).parent().find("> .JCONTENIDO-TAB").css("display", "none");
                                //                                            $(this).parent().find("> .JCONTENIDO-TAB").eq($(this).parent().find(".JCHEK-TABS").index(this)).css("display", "block");
                                //                                        });
                                //                                        //marcando el primer tab
                                //                                        $(".JTABS").each(function () {
                                //                                            $(this).find("> .JCHEK-TABS").eq(0).click();
                                //                                        });
                                //                                        $.ajax({
                                //                                            url: "InformacionPaciente.aspx/ObtenerValoresAtencionAnterior",
                                //                                            type: "POST",
                                //                                            contentType: "application/json; charset=utf-8",
                                //                                            data: JSON.stringify({
                                //                                                TipoDeAtencion: sTipoAtencion,
                                //                                                IdHistoria: sIdehistoria
                                //                                            }),
                                //                                            dataType: "json",
                                //                                            error: function (dato1, datos2, dato3) {

                                //                                            }
                                //                                        }).done(function (oOB_JSON) {
                                //                                            
                                //                                        });

                                //                                    } else {
                                //                                        /*JB - 20/06/2020 - se comenta esta linea y se usa las lineas de abajo para que refresque pantalla
                                //                                        $("[id*=divContenedorDinamico]").find(".JDIV-GRUPO").each(function () {
                                //                                        fn_CargandoDatosControlesGrupo($(this).prop("id").trim(), sIdehistoria, sTipoAtencion);
                                //                                        });*/
                                //                                        fn_LOAD_VISI();
                                //                                        $.ajax({
                                //                                            url: "InformacionPaciente.aspx/ObtenerValoresAtencionAnterior",
                                //                                            type: "POST",
                                //                                            contentType: "application/json; charset=utf-8",
                                //                                            data: JSON.stringify({
                                //                                                TipoDeAtencion: sTipoAtencion,
                                //                                                IdHistoria: sIdehistoria
                                //                                            }),
                                //                                            dataType: "json",
                                //                                            error: function (dato1, datos2, dato3) {

                                //                                            }
                                //                                        }).done(function (oOB_JSON) {
                                //                                            if (sAtencionAnterior != "") {
                                //                                                if (sAtencionAnterior != sCodigoAtencionActual) {
                                //                                                    fn_DeshabilitaControlesI(); //FUNCION PARA DESHABILITAR TODOS LOS CONTROLES DE LA PANTALLA
                                //                                                } else {
                                //                                                    location.reload();
                                //                                                }
                                //                                            } else {
                                //                                                fn_CampoBase();
                                //                                            }
                                //                                        });
                                //                                    }
                                //                                    //FIN - JB - NUEVA CONDICION PARA CARGAR LAS ATENCIONES E

                                //                                    fn_RenderizarTabs();
                                //                                    //$(".DatosUsuario").load("Utilidad/DatosUsuario.aspx"); //CARGANDO DATOS EN LA PARTE SUPERIOR CON LA NUEVA ATENCION SELECCIONADA

                                //                                    /*RECARGANDO TODO EL CONTENIDO DE LA PAGINA*/
                                //                                    fn_CargaNotaIngreso();
                                //                                    fn_CargarEvolucionClinica();
                                //                                    fn_CargarControlClinicoIM();
                                //                                    $("#divGridDiagnosticos").load("GridViewAjax/GridDiagnostico.aspx", function () {
                                //                                    });
                                //                                    $("#divGridLaboratorio").load("GridViewAjax/GridLaboratorio.aspx", function () {
                                //                                    });
                                //                                    fn_CrearTreeViewAnalisis();
                                //                                    $("#divGridImagen").load("GridViewAjax/GridImagen.aspx", function () {
                                //                                    });
                                //                                    fn_CrearTreeViewImagenes();
                                //                                    $("#divGridInterconsulta").load("GridViewAjax/GridInterconsulta.aspx", function () { fn_CargaPermiso(); });
                                //                                    fn_CargarPatologias();
                                //                                });
                            }
                        });
                    } else {
                        aValores = oOB_JSON.d.toString().split(";");
                        $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                    }
                });
            });
        });
    }

    function fn_ExpiraSession() {        
        window.location.href = aValores[1];
    }

    function fn_CierraPopupInterconsulta() {
        fn_oculta_popup();        
        $("#divGridInterconsulta").load("GridViewAjax/GridInterconsulta.aspx", function () {
            fn_CreaEventoGridInterconsulta();
        });
    }

    function fn_GuardaLog(Control1, Mensaje1) {
        $.ajax({
            url: "InformacionPaciente.aspx/GuardarLog",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                Control: Control1,
                Mensaje: Mensaje1
            }),
            dataType: "json",
            error: function (dato1, datos2, dato3) {
            }
        }).done(function (oOB_JSON) {
            if (oOB_JSON.d != "OK") {
                //error
            } else {
                //ok
            }
        });
    }


    function fn_CargarAlergia1() {
        $.ajax({
            url: "InformacionPaciente.aspx/CargarAlergia1",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (dato1, datos2, dato3) {
            }
        }).done(function (oOB_JSON) {
            if (oOB_JSON.d != "OK") {
                //error
            } else {
                $("[id*=divPresentaAlergia]").css("display", "block");
                $("[id*=spPresentaAlergia]").html("Presenta Alergia");
                
            }
        });
    }
    

    function fn_AlertaImgLagPendiente() {
        $.ajax({
            url: "InformacionPaciente.aspx/VerificarAlertas", //verifica si hay alerta de pendiente
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (dato1, datos2, dato3) {
            }
        }).done(function (oOB_JSON) {
            aValoresAlerta = oOB_JSON.d.toString().split(";");
            if (aValoresAlerta[0] == "ALERTA") {
                if (aValoresAlerta[1] == "M") { //solo mostrara a los medicos esta alerta
                    $.ajax({
                        url: "InformacionPaciente.aspx/VerificarAlertas2", //obtiene el mensaje de alerta para el tittle del boton alerta
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        error: function (dato1, datos2, dato3) {
                        }
                    }).done(function (oOB_JSON) {
                        if (oOB_JSON.d != "") {
                            $("#imgAlerta").attr("title", oOB_JSON.d);
                            $("#imgAlerta").next().html(oOB_JSON.d);
                            $("#imgAlerta").addClass("JIMG-ALERTA"); 
                            $("#imgAlerta").attr("src", "../Imagenes/AlertaR.png");
                        }
                    });
                }
            } else {
                $("#imgAlerta").removeClass("JIMG-ALERTA");
                $("#imgAlerta").attr("title", "Alerta");
                $("#imgAlerta").next().html("Alerta"); 
                $("#imgAlerta").attr("src", "../Imagenes/Alerta.png");

                $("#divPresentaExamenVerificarVisualizar").css("display", "none");
            }
        });
        //fn_CrearTreeViewAnalisis();
        fn_CrearTreeViewAnalisis2("1", "", "0");
    }

    function fn_SetValoresPatologia() {
        var $select = $('.select-otrospatologia').selectize({
            SeleccionMaxima: 5,
            AbrirEnFoco: false,
            MaxElementos: 10
        });

        //checkbox seleccionados
        if ($("#" + "<%=hfCheckPatologiaSeleccionado.ClientID %>").val() != "") {
            var aAR_PATO = new Array();
            /*//Observaciones Cmendez 02/05/2022*/
            aAR_PATO = $("#" + "<%=hfCheckPatologiaSeleccionado.ClientID %>").val().split("|");
            $(".JCHECK-PATOLOGIA").find(":input[type='checkbox']").each(function () {
                var ObjetoPatologia = $(this);
                for (var i = 0; i < aAR_PATO.length; i++) {
                    if (ObjetoPatologia.val() == aAR_PATO[i]) {
                        ObjetoPatologia.prop("checked", true);
                    }
                }
            });
        }
        //otras patologias seleccionados
        if ($("#" + "<%=hfIdPatologiaSeleccionado.ClientID %>").val() != "") {
            var aAR_PATO = new Array();
            //Observaciones Cmendez 02/05/2022
            aAR_PATO = $("#" + "<%=hfIdPatologiaSeleccionado.ClientID %>").val().split("|");

            //campo otros patologia 
            var selectize = $select[0].selectize;
            for (var i = 0; i < aAR_PATO.length; i++) {
                selectize.setValue(aAR_PATO);
            }
            $('#ddlOtrosPatologia')[0].selectize.enable();
            $("#chkPatologia_otros").prop("checked", true);
        } else {
            if ($("#chkPatologia_otros").prop("checked")) {
                $('#ddlOtrosPatologia')[0].selectize.enable();
            } else {
                $('#ddlOtrosPatologia')[0].selectize.disable();
            }
        }
        //Grid patologias

        $("#" + "<%=gvPatologia.ClientID %>").find("tr").each(function (e) {
            var ObjetoFila = $(this);
            if (ObjetoFila.find('.select-patologia').length > 0) {
                var $SelectMuestra = ObjetoFila.find('.select-patologia').selectize({
                    SeleccionMaxima: 1,
                    AbrirEnFoco: false,
                    MaxElementos: 10
                });
                if (e > 0) {
                    if (ObjetoFila.find(".ORGANO-OCULTO").html()  != undefined) {
                        if (ObjetoFila.find(".ORGANO-OCULTO").html() != "") {
                            /*//Observaciones Cmendez 02/05/2022*20/05/22*/
                            var aAR_ORGA = $.trim(ObjetoFila.find(".ORGANO-OCULTO").html().split("|"));
                            var selectize = $SelectMuestra[0].selectize;
                            selectize.setValue(aAR_ORGA);
                        }
                        if (ObjetoFila.find(".CANTIDAD-OCULTO").html() != "") {
                            ObjetoFila.find(".TEXTO-CANTIDAD ").val($.trim(ObjetoFila.find(".CANTIDAD-OCULTO").html()));
                        }
                    }                    
                }
            }            
        });
        $("#" + "<%=gvPatologia.ClientID %>").find(".selectize-control.multi .selectize-input [data-value]").addClass("ClaseBuscadorPatologia1");
        $("#" + "<%=gvPatologia.ClientID %>").find(".selectize-control.multi .selectize-input > div").addClass("ClaseBuscadorPatologia2");

        $("[id*=gvPatologia]").find(".JIMG-ELIMINAR").unbind("click");
        $("[id*=gvPatologia]").find(".JIMG-ELIMINAR").click(function () {
            fn_LOAD_VISI();
            var codigo = $(this).parent().parent().find("td").eq(7).html().trim();
            var objeto = $(this);            
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $.ajax({
                        url: "InformacionPaciente.aspx/EliminarPatologia",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({
                            IdePatologiaMae: codigo
                        }),
                        error: function (dato1, datos2, dato3) {

                        }
                    }).done(function (oOB_JSON) {
                        if ($("[id*=gvPatologia]").find("tr").length == 2) {
                            var row = $("[id*=gvPatologia] tr:last").clone();
                            objeto.parent().parent().remove();
                            $("td:nth-child(1)", row).html("");
                            $("td:nth-child(2)", row).html("");
                            $("td:nth-child(3)", row).html("");
                            $("td:nth-child(4)", row).html("");
                            $("td:nth-child(5)", row).html("");
                            $("td:nth-child(6)", row).html("");
                            $("td:nth-child(7)", row).html("");
                            $("td:nth-child(8)", row).html("");
                            $("td:nth-child(9)", row).html("");
                            $("td:nth-child(10)", row).html("");
                            $("[id*=gvPatologia] tbody").append(row);
                        } else {
                            objeto.parent().parent().remove();
                        }
                        //objeto.parent().parent().remove();
                        fn_LOAD_OCUL();
                    });
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });
    }

    function fn_DadoAltaPaciente() {
        fn_oculta_mensaje();
        fn_CancelaAltaMedicaEpicrisis();        
        //window.location.href = "ConsultaPacienteHospitalizado.aspx";  JB - COMENTADO - 20/07/2020
        $.JPopUp("Impresion de Reportes", "PopUp/ImpresionReporte.aspx", "2", "Aceptar;Salir", "fn_Imprimir();fn_FinImpresionReporteAlta()", 50); //ImprimirReporte.aspx 31/01/2017 
    }


    function fn_FinImpresionReporteAlta() {
        fn_oculta_popup();
        fn_LOAD_VISI();
        window.location.href = "ConsultaPacienteHospitalizado.aspx";
    }



    function fn_CrearTreeViewJuntaMedica(OrdenEjecutar, IdeJuntaMedicaMostrar, objeto) {        
        $.ajax({
            url: "InformacionPaciente.aspx/ConsultaJuntaMedica",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({
                Valor: IdeJuntaMedicaMostrar,
                Orden: OrdenEjecutar                
            }),
            error: function (dato1, datos2, dato3) {

            }
        }).done(function (oOB_JSON) {
            if (oOB_JSON.d != "") {
                if (OrdenEjecutar == "1") {
                    $("#divJuntaMedica").html("");
                    $("#divJuntaMedica").append(oOB_JSON.d);
                }
                if (OrdenEjecutar == "2") {
                    objeto.parent().find(".JTREE3-HORA").html("");
                    objeto.parent().find(".JTREE3-HORA").append(oOB_JSON.d);
                }
                if (OrdenEjecutar == "3") {
                    objeto.next().html("");
                    objeto.next().append(oOB_JSON.d);
                }
                fn_CrearEventoTreeJuntaMedica();
            }
        });
    }

    function fn_CrearEventoTreeJuntaMedica() {
        $("#divJuntaMedica").find(".JFILA-FECHA").unbind("click");
        $("#divJuntaMedica").find(".JFILA-FECHA").click(function () {
            var oObjeto = $(this);

            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $("#divJuntaMedica").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                    $("#divJuntaMedica").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                    $("#divJuntaMedica").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");
                    oObjeto.addClass("JTREE3-SELECCIONADO");
                    oObjeto.find(" > .JTREE3-SIGNO").toggleClass("JTREE3-PLUS");
                    var CadenaClase = oObjeto.find(" > .JTREE3-SIGNO").attr("class");
                    var FecJuntaMedica = oObjeto.find("> input").val();
                    if (CadenaClase.includes("JTREE3-PLUS")) {
                        fn_CrearTreeViewJuntaMedica("2", FecJuntaMedica, oObjeto);
                        oObjeto.parent().find(".JTREE3-HORA").css("display", "block");
                    } else {
                        oObjeto.parent().find(".JTREE3-HORA").css("display", "none");
                    }
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });
        $("#divJuntaMedica").find(".JFILA-HORA").unbind("click");
        $("#divJuntaMedica").find(".JFILA-HORA").click(function () {
            var oObjeto = $(this);
            
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $("#divJuntaMedica").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                    $("#divJuntaMedica").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                    $("#divJuntaMedica").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");

                    oObjeto.addClass("JTREE3-SELECCIONADO");
                    oObjeto.find(" > .JTREE3-SIGNO").toggleClass("JTREE3-PLUS");
                    var CadenaClase = oObjeto.find(" > .JTREE3-SIGNO").attr("class");
                    var IdeJuntaMedica = oObjeto.find("> input").val();                    
                    if (CadenaClase.includes("JTREE3-PLUS")) {
                        fn_CrearTreeViewJuntaMedica("3", IdeJuntaMedica, oObjeto);
                        oObjeto.next().css("display", "block");
                    } else {
                        oObjeto.next().css("display", "none");
                    }
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });

        $("#divJuntaMedica").find(".JTREE3-DETALLE").unbind("click");
        $("#divJuntaMedica").find(".JTREE3-DETALLE").click(function () {
            var oObjeto = $(this);

            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $("#divJuntaMedica").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                    $("#divJuntaMedica").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                    $("#divJuntaMedica").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");

                    oObjeto.addClass("JTREE3-SELECCIONADO");
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        });
             
    }



    function fn_ConsultaSeccion(Seccion, SubSeccion, Orden, Codcpt) {
        $.ajax({
            url: "InformacionPaciente.aspx/ConsultaSeccionProcedimiento",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({
                seccion: Seccion,
                subseccion: SubSeccion,
                orden: Orden,
                CodCpt: Codcpt
            }),
            error: function (dato1, datos2, dato3) {

            }
        }).done(function (oOB_JSON) {
            if (oOB_JSON.d != "") {
                var xmlDoc = $.parseXML(oOB_JSON.d);
                var xml = $(xmlDoc);
                var ProcedimientoMedicos = xml.find("TablaProcedimientoMedico");

                if (Orden == 1) {
                    var EstructuraCombo = "";
                    $(ProcedimientoMedicos).each(function () {
                        EstructuraCombo += "<div class='JSELECT2-ITEM JSELECT2-ELEMENT JSELECT2-INVISIBLE'>" + $(this).find("dsc_seccioncpt").text() + "</div>";
                        EstructuraCombo += "<div style='display:none'>" + $(this).find("cod_seccioncpt").text() + "</div>";
                    });

                    $("#ddlSeccionProcedimientoMedico").find(".JSELECT2-CONTENEDOR").html("");
                    $("#ddlSeccionProcedimientoMedico").find(".JSELECT2-CONTENEDOR").html("<div class='JSELECT2-ITEM JSELECT2-ELEMENT JSELECT2-INVISIBLE'>-</div><div style='display:none'></div>" +
                    EstructuraCombo);
                    fn_InicializarCombo();

                    $("#ddlSeccionProcedimientoMedico").find(".JSELECT2-ELEMENT").click(function () {
                        $(this).parent().parent().data("codigo", $(this).next().text());
                        $(this).parent().parent().data("descripcion", $(this).text());
                        $(this).parent().find(".JSELECT2-ELEMENT").removeClass("JSELECT2-VISIBLE");
                        $(this).parent().find(".JSELECT2-ELEMENT").addClass("JSELECT2-INVISIBLE");
                        $(this).parent().parent().find(".JSELECT2-SELECCION").toggleClass("JSELECT2-ACTIVO");
                        $(this).parent().parent().css("position", "relative"); //JB - 03/05/2021

                        $(this).parent().parent().find(".JSELECT2-SELECCION").html($(this).text());
                        fn_ConsultaSeccion($("#ddlSeccionProcedimientoMedico").data("codigo"), "", 2, "");

                        //limpiando los otros combos
                        $("#ddlDescripcionCPT").removeData("descripcion");
                        $("#ddlDescripcionCPT").removeData("codigo");
                        //$("#ddlSeccionProcedimientoMedico").find(".JSELECT2-SELECCION").html("-");
                        $("#ddlSubSeccionProcedimientoMedico").find(".JSELECT2-SELECCION").html("-");
                        $("#ddlDescripcionCPT").find(".JSELECT2-SELECCION").html("-");
                    });
                    fn_InicializarCombo(); //JB - 03/05/2021
                }

                if (Orden == 2) {
                    var EstructuraCombo = "";
                    $(ProcedimientoMedicos).each(function () {
                        EstructuraCombo += "<div class='JSELECT2-ITEM JSELECT2-ELEMENT JSELECT2-INVISIBLE'>" + $(this).find("dsc_seccioncpt").text() + "</div>";
                        EstructuraCombo += "<div style='display:none'>" + $(this).find("cod_seccioncpt").text() + "</div>";
                    });

                    $("#ddlSubSeccionProcedimientoMedico").find(".JSELECT2-CONTENEDOR").html("");
                    $("#ddlSubSeccionProcedimientoMedico").find(".JSELECT2-CONTENEDOR").html("<div class='JSELECT2-ITEM JSELECT2-ELEMENT JSELECT2-INVISIBLE'>-</div><div style='display:none'></div>" +
                    EstructuraCombo);
                    fn_InicializarCombo();

                    $("#ddlSubSeccionProcedimientoMedico").find(".JSELECT2-ELEMENT").click(function () {
                        $(this).parent().parent().data("codigo", $(this).next().text());
                        $(this).parent().parent().data("descripcion", $(this).text());
                        $(this).parent().find(".JSELECT2-ELEMENT").removeClass("JSELECT2-VISIBLE");
                        $(this).parent().find(".JSELECT2-ELEMENT").addClass("JSELECT2-INVISIBLE");
                        $(this).parent().parent().find(".JSELECT2-SELECCION").toggleClass("JSELECT2-ACTIVO");
                        $(this).parent().parent().css("position", "relative"); //JB - 03/05/2021

                        $(this).parent().parent().find(".JSELECT2-SELECCION").html($(this).text());
                        fn_ConsultaSeccion($("#ddlSeccionProcedimientoMedico").data("codigo"), $("#ddlSubSeccionProcedimientoMedico").data("codigo"), 3, "");

                        //limpiando los otros combos
                        $("#ddlDescripcionCPT").removeData("descripcion");
                        $("#ddlDescripcionCPT").removeData("codigo");
                        //$("#ddlSeccionProcedimientoMedico").find(".JSELECT2-SELECCION").html("-");
                        //$("#ddlSubSeccionProcedimientoMedico").find(".JSELECT2-SELECCION").html("-");
                        $("#ddlDescripcionCPT").find(".JSELECT2-SELECCION").html("-");
                    });
                    fn_InicializarCombo(); //JB - 03/05/2021
                }

                if (Orden == 3) {
                    var EstructuraCombo = "";
                    $(ProcedimientoMedicos).each(function () {
                        EstructuraCombo += "<div class='JSELECT2-ITEM JSELECT2-ELEMENT JSELECT2-INVISIBLE'>" + $(this).find("dsc_cpt").text() + "</div>";
                        EstructuraCombo += "<div style='display:none'>" + $(this).find("cod_cpt").text() + "</div>";
                    });

                    $("#ddlDescripcionCPT").find(".JSELECT2-CONTENEDOR").html("");
                    $("#ddlDescripcionCPT").find(".JSELECT2-CONTENEDOR").html("<div class='JSELECT2-ITEM JSELECT2-ELEMENT JSELECT2-INVISIBLE'>-</div><div style='display:none'></div>" +
                    EstructuraCombo);
                    fn_InicializarCombo();

                    $("#ddlDescripcionCPT").find(".JSELECT2-ELEMENT").click(function () {
                        $(this).parent().parent().data("codigo", $(this).next().text());
                        $(this).parent().parent().data("descripcion", $(this).text());
                        $(this).parent().find(".JSELECT2-ELEMENT").removeClass("JSELECT2-VISIBLE");
                        $(this).parent().find(".JSELECT2-ELEMENT").addClass("JSELECT2-INVISIBLE");
                        $(this).parent().parent().find(".JSELECT2-SELECCION").toggleClass("JSELECT2-ACTIVO");
                        $(this).parent().parent().css("position", "relative"); //JB - 03/05/2021

                        $(this).parent().parent().find(".JSELECT2-SELECCION").html($(this).text());

                        $("#txtProcedimientoMedico").val($(this).next().text());  //JB - SEGUN LO SOLICITADO EN DOCUMENTO 09/03/2021
                        $("#spCodigoProcedimientoSeleccionado").html($(this).next().text()); //JB - SEGUN LO SOLICITADO EN DOCUMENTO 09/03/2021
                    });
                    fn_InicializarCombo(); //JB - 03/05/2021
                }

                if ($("#chkBuscarCodProcedimientoMedico").prop("checked")) {
                    $("#ddlSeccionProcedimientoMedico").find(".JSELECT2-SELECCION").unbind("click");
                    $("#ddlSubSeccionProcedimientoMedico").find(".JSELECT2-SELECCION").unbind("click");
                    $("#ddlDescripcionCPT").find(".JSELECT2-SELECCION").unbind("click");
                }
                //ddlDescripcionCPT
            }
        });
    }


    function fn_DeshabilitarHabilitarCombo(IdControlCombo) {
        $("#" + IdControlCombo).toggleClass("JSELECT2-DESHABILITADO");
        
        $("#" + IdControlCombo).find(".JSELECT2-SELECCION").unbind("click");

    }

    function fn_EnumerarCalculosCalculadora() {
        $("#divContenedorCalculadora").find(".JFILA-ESTILO-ARRIBACERO").each(function () {
            $(this).find(".JCELDA-1").eq(0).find(".JETIQUETA_2").html($(this).index() + 1);
        });

        $(".JELIMINAR-CALCULO").unbind("click");
        $(".JELIMINAR-CALCULO").click(function () {
            $(this).parent().parent().remove();
            fn_EnumerarCalculosCalculadora();
        });
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">    
    
    <div class="JCONTENEDOR">
        <%--12/09/2016--%>
        <div style="position:absolute;top:110px;right:1%;width:75%;color:#2074ac;font-family:arial;font-weight:bold;border:1px solid;padding:2px;">
            <div style="float:right;margin-right:10px;cursor:pointer;" id="divCierraSesion"> -  Cerrar Sesion</div>
            <div style="float:right;margin-right:10px;cursor:pointer;" id="divCambiarPassword"> -  Cambiar Contraseña</div>
            <div runat="server" id="divUsuarioConexion" style="float:right;margin-right:5px;"></div>
        </div>
        <%--12/09/2016--%>
        <div class="JFILA" style="position:absolute;top:40px;right:1%;float:right;width:75%;">
            <div class="JCELDA-12">
                <div class="JDIV-CONTROLES" style="border:1px solid Green;padding:0.4em;" id="divBotonera">
                   
                </div>
            </div>
        </div>        
        <div class="JFILA">
            <div class="JCELDA-12">
                <div class="DatosUsuario" id="DatosUsuarioInformacionPaciente">
                           
                </div>
            </div>
        </div>
        <div class="JFILA">
            <div class="JCELDA-12">
                <div class="JDIV-CONTROLES">
                    <div class="JDIV_ACOR_CONTENEDOR">
                        <label class="JDIV-ACOR-TITU" name="labelmedicina">Historia Clínica</label>
                        <div class="JDIV_ACOR_CONTENIDO" name="componentmedicina">
                            <div class="JFILA">
								<div class="JCELDA-12">
                                    <div class="JDIV-CONTROLES">
                                    <div class="JCONTENEDOR-TAB1" id="divContenedorDinamico" runat="server">                                        
                                        
                                    </div>
                                    </div>
                                </div>
                                <div class="JCELDA-0">
                                    
                                    <%--<div style="width: 100%;overflow: hidden;float: left;display: table;padding: 0.35em;">
                                        <div style="position:absolute;right:5px;top:0;" id="divContenedorAtenciones">
                                            <div style="padding:5px;font-size:1.5em;Color:White;cursor:pointer;background:#8DC73F;width:280px;text-align:center; float:right;" >Atenciones Anteriores</div>
                                            <div style="position:absolute;top:2.4em;right:-500px;width:450px;border:1px solid #4BACFF;border-radius:5px;overflow-x:auto;background:white;box-shadow:0 0 25px #4BACFF;" id="divAtencionesAnteriores">
                                               
                                            </div>
                                        </div>
                                    </div>
                                    8DC73F   4BACFF--%>
                                    <div style="width: 100%;overflow: hidden;float: left;display: table;">
                                        <div style="position:absolute;right:-450px;top:0;width:auto;height:auto;min-width:495px;" id="divContenedorAtenciones">
                                            <div id="divTituloAtenciones" style="float:left;right:5px;padding:10px;font-size:1.3em;top:2em;Color:White;cursor:pointer;background:#134B8D;box-shadow:-10px 0 50px #134B8D;width:auto;text-align:center;-ms-writing-mode: tb-rl;-webkit-writing-mode: vertical-rl;-moz-writing-mode: vertical-rl;-ms-writing-mode: vertical-rl;writing-mode: vertical-rl;-webkit-text-orientation: upright;-moz-text-orientation: upright;-ms-text-orientation: upright;text-orientation: upright; border-radius:10px;" >Atenciones&nbsp;Anteriores</div>
                                            <div style="top:2.6em;right:-5px;width:450px;border:1px solid #4BACFF;border-radius:5px;overflow-x:auto;background:white;box-shadow:0 0 25px #4BACFF;" id="divAtencionesAnteriores">                    
                        
                                            </div>
                                        </div>
                                    </div><%----%>
                                </div>
                            </div>                     
                        </div>                        
                        
                        <label class="JDIV-ACOR-TITU" name="labelmedicina">Nota de Ingreso</label>
                        <div class="JDIV_ACOR_CONTENIDO" name="componentmedicina">
                            <div class="JFILA">
                                <div class="JCELDA-12">   
                                    <div style="border: 1px solid #BABABA;padding:5px;width:100%;overflow-y:auto;height:99%;background: #F5F5F5;border-radius: 5px;float: left;">
                                        <div class="JFILA">
                                            <div class="JCELDA-5">
                                                <div class="JTABS" style="width:100%;">                
                                                    <input type="radio" id="TabPrincipalNI1" name="TabPrincipalNI" class="JCHEK-TABS" />
                                                    <label for="TabPrincipalNI1" class="JTABS-LABEL">Médico Tratante</label>
                                                    <input type="radio" id="TabPrincipalNI2" name="TabPrincipalNI" class="JCHEK-TABS" />        
                                                    <label for="TabPrincipalNI2" class="JTABS-LABEL">Intensivista</label>
                                                    <div class="JCONTENIDO-TAB">
                                                        <div class="JFILA">
                                                            <div class="JCELDA-12">
                                                                <div class="JDIV-CONTROLES">
                                                                    <textarea rows="5" cols="1" class="JTEXTO" id="txtNotaIngresoMedicoTratante" ></textarea>      
                                                                </div>
                                                            </div>
                                                        </div>  
                                                        <div class="JFILA">
                                                            <div class="JCELDA-3">
                                                                <input type="button" value="Guardar" id="btnGuardarNotaIngresoMT" name="11/01/01" />
                                                            </div>
                                                        </div>                                              
                                                    </div>
                                                    <div class="JCONTENIDO-TAB">
                                                        <div class="JFILA">
                                                            <div class="JCELDA-12">
                                                                <div class="JDIV-CONTROLES">
                                                                    <textarea rows="5" cols="1" class="JTEXTO" id="txtNotaIngresoIntensivista" ></textarea>      
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="JFILA">
                                                            <div class="JCELDA-3">
                                                                <input type="button" value="Guardar" id="btnGuardarNotaIngresoI" name="11/01/01" />
                                                            </div>
                                                        </div>  
                                                    </div>
                                                </div>
                                             </div>
                                             <div class="JCELDA-7">
                                                <div style="width:100%;height:100%;border:1px solid #4BACFF;min-height:50px;" id="divNotaIngreso"> 
                                        
                                                </div>
                                            </div>
                                        </div>
                                    </div>                            
                                </div>
                            </div>
                                                        
                        </div>
                        <label class="JDIV-ACOR-TITU" name="labelmedicina">Junta Médica</label>
                        <div class="JDIV_ACOR_CONTENIDO" name="componentmedicina">
                            <div class="JFILA">
                                <div class="JCELDA-12">
                                    <div style="border: 1px solid #BABABA;padding:5px;width:100%;overflow-y:auto;height:99%;background: #F5F5F5;border-radius: 5px;float: left;">
                                        <div class="JFILA">
                                            <div class="JCELDA-5">                                                
                                                <div class="JFILA">
                                                    <div class="JCELDA-12">
                                                        <div class="JDIV-CONTROLES">
                                                            <textarea rows="5" cols="1" class="JTEXTO" id="txtJuntaMedica" ></textarea>  
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="JFILA">
                                                    <div class="JCELDA-3">
                                                        <input type="button" value="Guardar" id="btnGuardarJuntaMedica" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="JCELDA-7">
                                                <div style="border:1px solid #4BACFF;width:100%;max-height:360px;overflow-y:auto;" id="divJuntaMedica"> 
                                        
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>                            
                        </div>

                        <label class="JDIV-ACOR-TITU" name="labelmedicina">Diagnóstico</label>
                        <div class="JDIV_ACOR_CONTENIDO" name="componentmedicina">
                            <div class="JFILA">
                                <div class="JCELDA-12">
                                    <div style="border: 1px solid #BABABA;padding:5px;width:100%;overflow-y:auto;height:99%;background: #F5F5F5;border-radius: 5px;float: left;">
                                        <div class="JFILA">
                                            <div class="JCELDA-6" style="height:240px;">
                                                <div class="JDIV-CONTROLES">  
                                                    <div>
                                                        <div class="JCELDA-12">
                                                            <div class="JDIV-CONTROLES">
                                                                <span class="JETIQUETA">Diagnóstico:</span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div>   <%--class="JFILA"--%>
                                                        <div class="JCELDA-4">
                                                            <div class="JDIV-CONTROLES">                  
                                                                <span class="JETIQUETA_2">Buscar por: </span>                                              
                                                                <input type="radio" name="rbTipoDiagnosticoB" id="rbBuscarCodigoDiagnotico" checked="checked" /><span class="JETIQUETA_2">Código</span>
                                                                <input type="radio" name="rbTipoDiagnosticoB" id="rbBuscarNombreDiagnotico" /><span class="JETIQUETA_2">Nombre</span>
                                                            </div>
                                                        </div>
                                                        <div class="JCELDA-5" style="overflow:initial;overflow:initial;position:static;">
                                                            <div class="JDIV-CONTROLES">
                                                                <input type="text" id="txtDiagnostico" class="JTEXTO" />
                                                                <span id="spCodigoDiagnosticoSeleccionado" class="JCOL-OCULTA"></span>
                                                                <div id="divBusquedaDiagnostico" class="JBUSQUEDA-ESPECIAL" style="width:50%;">                                                                                 
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="JCELDA-3">
                                                            <div class="JDIV-CONTROLES">
                                                                <%--<img src="../Imagenes/Buscar.png" id="imgBuscarDiagnostico" alt="" class="JIMG-BUSQUEDA" />--%>
                                                                <input type="button" id="imgBuscarDiagnostico" class="JBOTON-IMAGEN" style="background-image:url(../Imagenes/Buscar.png);" />
                                                                <%--<img src="../Imagenes/Favoritos.png" id="imgFavoritoDiagnostico" alt="" class="JIMG-FAVORITO" />--%>
                                                                <input type="button" id="imgFavoritoDiagnostico" class="JBOTON-IMAGEN" style="background-image:url(../Imagenes/Favoritos.png);" />
                                                                

                                                                <input type="checkbox" id="chkFavoritoDiagnostico" />
                                                                <span class="JETIQUETA_2">Favorito</span>
                                                                <input type="hidden" id="hfFavoritoDiagnostico" />
                                                            </div>
                                                        </div>
                                                    </div> 
                                                    <div> <%--class="JFILA"--%>
                                                        <div class="JCELDA-12" style="position:initial;">
                                                            <div class="JDIV-CONTROLES">
                                                                <input type="checkbox" id="chkDiagnosticoSeleccionado" disabled="disabled" />
                                                                <span class="JETIQUETA_2" id="spDiagnosticoSeleccionado"></span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div>
                                                    <div class="JCELDA-4" style="position:initial;border:1px solid gray;">
                                                        <div class="JDIV-CONTROLES">
                                                            <input type="radio" id="rbTipoE" name="TipoES" />
                                                            <span class="JETIQUETA_2">Ingreso</span>                                                        

                                                            <input type="radio" id="rbTipoS" name="TipoES" />
                                                            <span class="JETIQUETA_2">Egreso</span>                                                        
                                                        </div>
                                                    </div>
                                                    <div class="JCELDA-2">
                                                        <div class="JDIV-CONTROLES">
                                                            <div style="float:right;">    
                                                                <span class="JETIQUETA_2">Clasificación: </span>
                                                            </div>
                                                        </div>                                                        
                                                    </div>
                                                    <div class="JCELDA-5" style="position:initial;border:1px solid gray;">
                                                        <div class="JDIV-CONTROLES">
                                                            <div style="float:right;">                                                           

                                                            <%--<input type="checkbox" id="chkPresuntivoDiagnostico" />--%>
                                                            <input type="radio" id="chkPresuntivoDiagnostico" name="TipoDiag" />
                                                            <span class="JETIQUETA_2">Presuntivo</span>
                                                            <input type="hidden" id="hfPresuntivoDiagnostico" />

                                                            <%--<input type="checkbox" id="chkRepetidoDiagnostico"/>--%>
                                                            <input type="radio" id="chkRepetidoDiagnostico" name="TipoDiag" />
                                                            <span class="JETIQUETA_2">Repetido</span>
                                                            <input type="hidden" id="hfRepetidoDiagnostico" />

                                                            <%--<input type="checkbox" id="chkDefinitivoDiagnostico"/>--%>
                                                            <input type="radio" id="chkDefinitivoDiagnostico" name="TipoDiag" />
                                                            <span class="JETIQUETA_2">Definitivo</span>
                                                            <input type="hidden" id="hfDefinitivoDiagnostico" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="JCELDA-1" style="position:initial;">
                                                        <div class="JDIV-CONTROLES">
                                                            <%--<img src="../Imagenes/Agregar.png" name="04/01/01" id="imgAgregarDiagnostico" alt="" class="JIMG-BUSQUEDA" />--%>
                                                            <input type="button" id="imgAgregarDiagnostico" class="JBOTON-IMAGEN" name="04/01/01" style="background-image:url(../Imagenes/Agregar.png);" /> 
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="JCELDA-6">
                                                <div class="JDIV-CONTROLES">   
                                                    <div id="divGridDiagnosticos">
                    
                                                    </div>                                    
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <label class="JDIV-ACOR-TITU" name="labelmedicina">Evolucion Clinica</label>
                        <div class="JDIV_ACOR_CONTENIDO" name="componentmedicina">
                            <div class="JFILA">
                                <div class="JCELDA-12">
                                    <div style="border: 1px solid #BABABA;padding:5px;width:100%;overflow-y:auto;height:99%;background: #F5F5F5;border-radius: 5px;float: left;">
                                        <div class="JFILA">
                                            <div class="JCELDA-5">                                    
                                                <%--<div class="JFILA"> 
                                                    <div class="JCELDA-1">
                                                        <span class="JETIQUETA_2">Nota:</span>
                                                    </div>
                                                    <div class="JCELDA-11">
                                                        <textarea rows="5" cols="1" class="JTEXTO" id="txtEvolucionClinica" ></textarea>
                                                    </div>
                                                </div>--%>
                                                <div class="JFILA"> 
                                                    <div class="JCELDA-2">
                                                        <span class="JETIQUETA">Subjetiva:</span>
                                                    </div>
                                                    <div class="JCELDA-10">
                                                        <textarea rows="4" cols="1" class="JTEXTO" id="txtSubjetiva" ></textarea>
                                                    </div>
                                                </div>
                                                <div class="JFILA"> 
                                                    <div class="JCELDA-2">
                                                        <span class="JETIQUETA">Objetiva:</span>
                                                    </div>
                                                    <div class="JCELDA-10">
                                                        <textarea rows="4" cols="1" class="JTEXTO" id="txtObjetiva" ></textarea>
                                                    </div>
                                                </div>
                                                <div class="JFILA"> 
                                                    <div class="JCELDA-2">
                                                        <span class="JETIQUETA">Apreciación:</span>
                                                    </div>
                                                    <div class="JCELDA-10">
                                                        <textarea rows="4" cols="1" class="JTEXTO" id="txtEvolucionClinica" ></textarea>
                                                    </div>
                                                </div>
                                                <div class="JFILA"> 
                                                    <div class="JCELDA-2">
                                                        <span class="JETIQUETA">Plan:</span>
                                                    </div>
                                                    <div class="JCELDA-10">
                                                        <textarea rows="4" cols="1" class="JTEXTO" id="txtPlan" ></textarea>
                                                    </div>
                                                </div>
                                                <div class="JFILA"> 
                                                    <div class="JCELDA-3">
                                                        <span class="JETIQUETA">Tipo Evolucion</span>
                                                    </div>

                                                    <div class="JCELDA-3">
                                                        <input type="radio" id="rbEstableMejoria" name="TipoEvolucion" value="04" /><span class="JETIQUETA_2">Estable con mejoria</span>
                                                    </div>
                                                    <div class="JCELDA-2">
                                                        <input type="radio" id="rbEstacionaria" name="TipoEvolucion" value="03" /><span class="JETIQUETA_2">Estacionaria</span>
                                                    </div>
                                                    <div class="JCELDA-2">                                                        
                                                        <input type="radio" id="rbInestable" name="TipoEvolucion" value="02" /><span class="JETIQUETA_2">Inestable</span>
                                                    </div>
                                                    <div class="JCELDA-2">                                                        
                                                        <input type="radio" id="rbDeterioro" name="TipoEvolucion" value="01" /><span class="JETIQUETA_2">Deterioro</span>
                                                    </div>
                                                </div>
                                                <div class="JFILA"> 
                                                    <div class="JCELDA-12">                                                        
                                                        <span class="JETIQUETA">Informacion a Paciente Familiar</span>
                                                    </div>
                                                </div>
                                                <div class="JFILA"> 
                                                    <div class="JCELDA-2 JESPACIO-IZQ-2">
                                                        <%--<input type="checkbox" id="chkEducacionEvolucionClinica" />Educación--%>
                                                        <input type="radio" id="rbEducacionEvolucionClinica" name="TipoEducacionInforme" /><span class="JETIQUETA_2">Educación</span>
                                                    </div>
                                                    <div class="JCELDA-2">
                                                        <%--<input type="checkbox" id="chkInformeEvolucionClinica" />Informe--%>
                                                        <input type="radio" id="rbInformeEvolucionClinica" name="TipoEducacionInforme" /><span class="JETIQUETA_2">Informe</span>
                                                    </div>
                                                    <div class="JCELDA-3">
                                                        <input type="checkbox" id="chkRequiereFirma" /><span class="JETIQUETA_2">Requiere Firma</span>
                                                    </div>
                                                    <div class="JCELDA-3">
                                                        <input type="button" value="Guardar" id="btnGuardarEvolucionClinica" name="02/01/01" />
                                                    </div>
                                                    
                                                </div>
                                            </div>
                                            <div class="JCELDA-7">
                                                <div style="border:1px solid #4BACFF;width:100%;height:100%;min-height:50px;max-height:350px;overflow-y:auto;" id="divEvolucionClinica"> <%--CONTENEDOR DE EVOLUCION CLINICA--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>                            
                            </div>
                        </div>
                        <label class="JDIV-ACOR-TITU" name="labelmedicina">Control Clínico e Indicaciones Médicas</label>
                        <div class="JDIV_ACOR_CONTENIDO" name="componentmedicina">
                            <div class="JFILA">
                                <div class="JCELDA-12">
                                    <div style="border: 1px solid #BABABA;padding:5px;width:100%;overflow-y:auto;height:99%;background: #F5F5F5;border-radius: 5px;float: left;">
                                        <div class="JFILA">
                                            <div class="JCELDA-6">
                                                <div class="JTABS" style="width:100%;">
                                                    <input type="radio" id="TabPrincipalT1" name="TabNro2" class="JCHEK-TABS" />
                                                    <label for="TabPrincipalT1" class="JTABS-LABEL">Farmacologico</label>
                                                    <input type="radio" id="TabPrincipalT2" name="TabNro2" class="JCHEK-TABS" />        
                                                    <label for="TabPrincipalT2" class="JTABS-LABEL">No Farmacologico</label>
                                                    <input type="radio" id="TabPrincipalT3" name="TabNro2" class="JCHEK-TABS" />        
                                                    <label for="TabPrincipalT3" class="JTABS-LABEL">Infusiones</label>
                                                    <input type="radio" id="TabPrincipalT4" name="TabNro2" class="JCHEK-TABS" />        
                                                    <label for="TabPrincipalT4" class="JTABS-LABEL">Calculadora</label>
                                                    <div class="JCONTENIDO-TAB">
                                                        <div class="JFILA">
                                                            <div class="JCELDA-2">
                                                                <div class="JDIV-CONTROLES">
                                                                    <span class="JETIQUETA">Medicamento:</span>
                                                                </div>
                                                            </div>
                                                            <div class="JCELDA-9" style="position:initial;overflow:initial;position:static;">
                                                                <div class="JDIV-CONTROLES">
                                                                    <input type="text" id="txtProducto_Con" class="JTEXTO" />
                                                                    <span id="spCodigoProductoSeleccionado" class="JCOL-OCULTA"></span>
                                                                    <div id="divBusquedaMedicamentoCC" class="JBUSQUEDA-ESPECIAL" style="max-height:185px;width:80%;">                                                                                 
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="JCELDA-1">
                                                                <div class="JDIV-CONTROLES">
                                                                    <%--<img src="../Imagenes/Buscar.png" id="imgBusquedaProducto" alt="" class="JIMG-BUSQUEDA" />--%>
                                                                    <input type="button" id="imgBusquedaProducto" class="JBOTON-IMAGEN" style="background-image:url(../Imagenes/Buscar.png);" /> 
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="JFILA">
                                                            <div class="JCELDA-10">
                                                                <input type="checkbox" id="chkBuscarDci" checked="checked" />
                                                                <label for="chkBuscarDci" class="JETIQUETA">Buscar por DCI</label>
                                                            </div>
                                                        </div>
                                                        <div class="JFILA"> <%--class="JFILA"--%>
                                                            <div class="JCELDA-2" style="position:initial;">
                                                                <div class="JDIV-CONTROLES">
                                                                    <span class="JETIQUETA">Dosis:</span>
                                                                </div>
                                                            </div>
                                                            <div class="JCELDA-8" style="position:initial;">
                                                                <div class="JDIV-CONTROLES">
                                                                    <input type="text" id="txtDosis_Con" class="JTEXTO" maxlength="100" />
                                                                </div>
                                                            </div> 
                                                        </div>
                                                        <div class="JFILA">
                                                            <div class="JCELDA-2" style="position:initial;">
                                                                <div class="JDIV-CONTROLES">
                                                                    <span class="JETIQUETA">Cada (Hrs.)</span>
                                                                </div>
                                                            </div>
                                                            <div class="JCELDA-2" style="position:initial;">
                                                                <div class="JDIV-CONTROLES">
                                                                    <input type="text" id="txtCadaHora_Con" class="JNUMERO" />
                                                                </div>
                                                            </div> 
                                                            <div class="JCELDA-1" style="position:initial;">
                                                                <div class="JDIV-CONTROLES">
                                                                    <input type="checkbox" id="chkPRNControlClinico" />
                                                                    <label for="chkPRNControlClinico" class="JETIQUETA" >PRN</label>
                                                                </div>
                                                            </div>
                                                            <div class="JCELDA-2 JESPACIO-IZQ-2" style="position:initial;">
                                                                <div class="JDIV-CONTROLES">
                                                                    <span class="JETIQUETA">Via:</span>
                                                                </div>
                                                            </div>
                                                            <div class="JCELDA-3" style="position:initial;">
                                                                <div class="JDIV-CONTROLES">
                                                                    <%--<input type="text" id="txtVia_ControlClinico" class="JTEXTO" /> jb - 26/04/2021 - comentado--%>
                                                                    <%--<input type="text" id="txtVia_Con" class="JTEXTO" disabled="disabled" />--%>
                                                                    <asp:DropDownList ID="ddlVia_Con" runat="server" CssClass="JTEXTO" Enabled="false" >
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="JFILA">
                                                            <div class="JCELDA-2" style="position:initial;">
                                                                <div class="JDIV-CONTROLES">
                                                                    <span class="JETIQUETA">Cantidad:</span>
                                                                </div>
                                                            </div>
                                                            <div class="JCELDA-2" style="position:initial;">
                                                                <div class="JDIV-CONTROLES">
                                                                    <input type="text" id="txtCantidad_Con" class="JNUMERO" />
                                                                </div>
                                                            </div>  
                                                        </div>
                                                        <%--<div class="JFILA">   
                                                            <div class="JCELDA-2" style="position:initial;">
                                                                <div class="JDIV-CONTROLES">
                                                                    <span class="JETIQUETA">Dia:</span>
                                                                </div>
                                                            </div>
                                                            <div class="JCELDA-2" style="position:initial;">
                                                                <div class="JDIV-CONTROLES">
                                                                    <input type="text" id="txtDiaControlClinico" class="JNUMERO" value="1"  />
                                                                </div>
                                                            </div>                                                       
                                                            <div class="JCELDA-2 JESPACIO-IZQ-3" style="position:initial;">
                                                                <div class="JDIV-CONTROLES">
                                                                    <span class="JETIQUETA">Cantidad:</span>
                                                                </div>
                                                            </div>
                                                            <div class="JCELDA-2" style="position:initial;">
                                                                <div class="JDIV-CONTROLES">
                                                                    <input type="text" id="txtCantidad_Con" class="JNUMERO" disabled="disabled" />
                                                                </div>
                                                            </div>                                                            
                                                        </div>--%>
                                                        <div class="JFILA">
                                                             <div class="JCELDA-2" >
                                                                <div class="JDIV-CONTROLES">
                                                                    <span class="JETIQUETA">Indicación:</span>
                                                                </div>
                                                             </div>
                                                             <div class="JCELDA-9" >
                                                                <div class="JDIV-CONTROLES">
                                                                    <textarea rows="5" cols="1" maxlength="500" class="JTEXTO" id="txtIndicacionProductoMedicamento" ></textarea>
                                                                </div>
                                                             </div>
                                                             <div class="JCELDA-1" style="position:initial;">
                                                                <div class="JDIV-CONTROLES">
                                                                    <%--<img src="../Imagenes/Agregar.png" id="imgAgregar_Con" alt="" class="JIMG-BUSQUEDA" />--%>
                                                                    <input type="button" id="imgAgregar_Con" class="JBOTON-IMAGEN" style="background-image:url(../Imagenes/Agregar.png);" /> 
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="JFILA"> <%--class="JFILA"--%>
                                                            <div class="JCELDA-12" style="position:initial;">
                                                                <span class="JETIQUETA_3">Indicaciones Selecionadas</span>
                                                            </div>
                                                        </div>
                                                        <div class="JFILA"> <%----%>
                                                            <div class="JCELDA-12">
                                                                <div class="JDIV-CONTROLES">
                                                                    <div id="divGridProductoMedicamento">
                    
                                                                    </div>
                                                                </div>
                                                            </div>                                        
                                                        </div>
                                                        <div class="JFILA"> <%--class="JFILA"--%>
                                                            <div class="JCELDA-2 JESPACIO-IZQ-10" style="position:initial;">
                                                                <div class="JDIV-CONTROLES">                                                            
                                                                    <div class="tooltip">
                                                                        <input type="button" id="imgEnviarControlClinico" class="JBOTON-IMAGEN" name="03/01/01" style="background-image:url(../Imagenes/Enviar_Solicitud.png);" title="Enviar" /> <%--Enviar_Solicitud.png  CAMBIO IMG   Enviar1.png);width:35px;height:35px;background-size:30px 30px;--%>
                                                                        <span tooltip-direccion="arriba">Enviar Control Clinico.</span> 
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="JCELDA-1" style="position:initial;">
                                                                <div class="JDIV-CONTROLES">
                                                                    
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="JCONTENIDO-TAB">
                                                        <div class="JFILA">
                                                            <div class="JCELDA-3">
                                                                <span class="JETIQUETA_2">Nutrición:</span>
                                                            </div>
                                                            <div class="JCELDA-9">
                                                                <input type="text" id="txtNutricionNoFarmacologico" class="JTEXTO" disabled="disabled"  />
                                                            </div>
                                                        </div>
                                                        <div class="JFILA">
                                                            <div class="JCELDA-3">
                                                                <span class="JETIQUETA_2">Terapia Física y Rehabilitación:</span>
                                                            </div>
                                                            <div class="JCELDA-9">
                                                                <input type="text" id="txtTerapiaFisRehaNoFarmacologico" class="JTEXTO"  disabled="disabled"  />
                                                            </div>
                                                        </div>
                                                        <div class="JFILA">
                                                            <div class="JCELDA-3">
                                                                <span class="JETIQUETA_2">Cuidados de enfermeria:</span>
                                                            </div>
                                                            <div class="JCELDA-9">
                                                                <input type="text" id="txtCuidadosEnfermeriaNoFarmacologico" class="JTEXTO"  disabled="disabled" />
                                                            </div>
                                                        </div>
                                                        <div class="JFILA">
                                                            <div class="JCELDA-3">
                                                                <span class="JETIQUETA_2">Otros:</span>
                                                            </div>
                                                            <div class="JCELDA-9">
                                                                <input type="text" id="txtOtrosNoFarmacologico" class="JTEXTO" disabled="disabled"  />
                                                            </div>
                                                        </div>
                                                        <div class="JFILA">
                                                            <div class="JCELDA-2 JESPACIO-IZQ-10" style="position:initial;">
                                                                <div class="JDIV-CONTROLES">                                                            
                                                                    <div class="tooltip">
                                                                        <input type="button" id="imgEnviarNoFarmacologico" class="JBOTON-IMAGEN" name="03/01/01" style="background-image:url(../Imagenes/Enviar_Solicitud.png);" title="Enviar" />  <%--Enviar_Solicitud.png  CAMBIO IMG   Enviar1.png);width:35px;height:35px;background-size:30px 30px;--%>
                                                                        <span tooltip-direccion="arriba">Enviar No Farmacologico.</span> 
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                    <div class="JCONTENIDO-TAB"> 
                                                        <div class="JFILA">
                                                            <div class="JCELDA-10" >
                                                                <div class="JDIV-CONTROLES">
                                                                    <input type="text" id="txtInfusionControlClinico" class="JTEXTO" disabled="disabled" />
                                                                </div>
                                                            </div>
                                                            <div class="JCELDA-1" style="position:initial;">
                                                                <div class="JDIV-CONTROLES">                                                                
                                                                    <input type="button" id="imgAgregarInfusion" class="JBOTON-IMAGEN" style="background-image:url(../Imagenes/Agregar.png);" /> 
                                                                    <%--<asp:ImageButton runat="server" ImageUrl="~/Imagenes/Agregar.png" CssClass="JBOTON-IMAGEN" ID="imgAgregarInfusion_1" OnClientClick="return GuardarAnalista();" />--%>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="JFILA">
                                                            <div class="JCELDA-12">
                                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:GridView ID="gvListadoInfusiones" runat="server" AutoGenerateColumns="False" 
                                                                            ShowHeaderWhenEmpty="True" CssClass="JSBTABLA" GridLines="None" 
                                                                            AllowPaging="True" PageSize="15" PagerStyle-CssClass="JPAGINADO" OnPageIndexChanging="gvListadoInfusiones_PageIndexChanging" >
                                                                            <Columns>
                                                                                <asp:BoundField DataField="Item" HeaderText="Item" >
                                                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="Infusion"  HeaderText="Descripción" >
                                                                                    <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" Width="85%" />
                                                                                </asp:BoundField>
                                                                                <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>                            
                                                                                        <img alt="" src="../Imagenes/anular.gif" class="JIMG-GENERAL" />                                    
                                                                                    </ItemTemplate>                                                                
                                                                                    <ItemStyle CssClass="Eliminar" Width="5%" />
                                                                                </asp:TemplateField>
                                                                            </Columns>                                                                            
                                                                        </asp:GridView>
                                                                        <%--<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                                                            <ProgressTemplate>                                                                                    
                                                                                <div class='JLOADER-3'><div class='JARO-1'></div><div class='JARO-2'></div></div>
                                                                            </ProgressTemplate>
                                                                        </asp:UpdateProgress>--%>
                                                                        <%--OnClick="btnAgregarInfusion_Click"--%>                                                                                                                                                
                                                                        <asp:Button runat="server" ID="btnRefrescarGridview" style="display:none;"/>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                                
                                                            </div>
                                                        </div>
                                                        <div class="JFILA">
                                                            <div class="JCELDA-2 JESPACIO-IZQ-10" style="position:initial;">
                                                                <div class="JDIV-CONTROLES">                                                            
                                                                    <div class="tooltip">
                                                                        <input type="button" id="imgEnviarInfusiones" class="JBOTON-IMAGEN" name="03/01/01" style="background-image:url(../Imagenes/Enviar_Solicitud.png);" title="Enviar Infusiones." />  <%--Enviar_Solicitud.png  CAMBIO IMG   Enviar1.png);width:35px;height:35px;background-size:30px 30px;--%>
                                                                        <span tooltip-direccion="arriba">Enviar Infusiones.</span> 
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <%--<div class="tooltip">
                                                                <input type="button" id="Button1" title="Enviar Patología." class="JBOTON-IMAGEN" style="background-image:url(../Imagenes/Enviar1.png);width:35px;height:35px;background-size:30px 30px;" onclick="fn_VALI_DIAG();" />                                                                   
                                                                <span tooltip-direccion="derecha">Enviar la Orden del Petitorio de Laboratorio.</span>                                                                     
                                                            </div>--%>



                                                            <div class="JCELDA-1" style="position:initial;">
                                                                <div class="JDIV-CONTROLES">
                                                                    
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="JCONTENIDO-TAB">
                                                        <div class="JFILA">
                                                            <div class="JCELDA-8 JESPACIO-IZQ-2">
                                                                <div style="width:100%;height:100%;border:1px solid #8DC73F;">
                                                                    <div class="JFILA">
                                                                        <div class="JCELDA-5 JESPACIO-IZQ-4">
                                                                            <span class="JETIQUETA">DATOS DEL PACIENTE</span><br /><br />
                                                                        </div>
                                                                    </div>
                                                                    <div class="JFILA">
                                                                        <div class="JCELDA-4" style="border: 1px solid #8DC73F;border-right:0;border-left:0;text-align:center;">
                                                                            <span class="JETIQUETA">Peso en Kg. </span><br /><br />
                                                                        </div>
                                                                        <div class="JCELDA-4" style="border: 1px solid #8DC73F;border-right:0;text-align:center;">
                                                                            <span class="JETIQUETA">Fármaco</span><br /><br />
                                                                        </div>
                                                                        <div class="JCELDA-4" style="border: 1px solid #8DC73F;border-right:0;text-align:center;">
                                                                            <span class="JETIQUETA">Velocidad de Infusión CC/Hr.</span><br /><br />
                                                                        </div>
                                                                    </div>
                                                                    <div class="JFILA">
                                                                        <div class="JCELDA-4">
                                                                            <input type="text" id="txtPesoCalculadora" class="JDECIMAL" />
                                                                        </div>
                                                                        <div class="JCELDA-4">
                                                                            <asp:DropDownList runat="server" ID="ddlFarmacoCalculadora" CssClass="JSELECT"></asp:DropDownList>
                                                                        </div>
                                                                        <div class="JCELDA-4">
                                                                            <input type="text" id="txtVelocidadInfusionCalculadora" class="JDECIMAL"  />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="JFILA">
                                                            <div class="JCELDA-8 JESPACIO-IZQ-2" style="text-align:center;">
                                                                <input type="button" id="btnCalcularCalculadora" value="Calcular" />
                                                            </div>
                                                        </div>
                                                        <div class="JFILA">
                                                            <div class="JCELDA-12" style="text-align:center;">
                                                                <span class="JETIQUETA_3">CALCULO DE DOSIS Y PRESCRIPCION</span>
                                                            </div>
                                                        </div>
                                                        <div class="JFILA" style="border: 1px solid #8DC73F;">
                                                            <div class="JCELDA-1" style="border-right: 1px solid #8DC73F;">
                                                                <span class="JETIQUETA">N° </span>
                                                            </div>
                                                            <div class="JCELDA-2" style="border-right: 1px solid #8DC73F;">
                                                                <span class="JETIQUETA">Farmaco </span>
                                                            </div>
                                                            <div class="JCELDA-2" style="border-right: 1px solid #8DC73F;">
                                                                <span class="JETIQUETA">Dosis por Kg por Hora</span>
                                                            </div>
                                                            <div class="JCELDA-2" style="border-right: 1px solid #8DC73F;">
                                                                <span class="JETIQUETA">Dosis por Dia</span>
                                                            </div>
                                                            <div class="JCELDA-2" style="border-right: 1px solid #8DC73F;">
                                                                <span class="JETIQUETA">Prescripcion</span>
                                                            </div>
                                                            <div class="JCELDA-2" style="border-right: 1px solid #8DC73F;">
                                                                <span class="JETIQUETA">Semáforo</span>
                                                            </div>
                                                            <div class="JCELDA-1">
                                                                <span class="JETIQUETA">Eliminar</span>
                                                            </div>
                                                        </div>
                                                        <div id="divContenedorCalculadora">
                                                            <%--<div class="JFILA JFILA-ESTILO-ARRIBACERO">
                                                                <div class="JCELDA-1 JCELDA-ESTILO-DERECHA">
                                                                    <span class="JETIQUETA">1 </span>
                                                                </div>
                                                                <div class="JCELDA-2 JCELDA-ESTILO-DERECHA">
                                                                    <span class="JETIQUETA">Fentanilo </span>
                                                                </div>
                                                                <div class="JCELDA-2 JCELDA-ESTILO-DERECHA">
                                                                    <span class="JETIQUETA">1.250 </span>
                                                                    <span class="JETIQUETA">MCG </span>
                                                                </div>
                                                                <div class="JCELDA-2 JCELDA-ESTILO-DERECHA">
                                                                    <span class="JETIQUETA">2400</span>
                                                                    <span class="JETIQUETA">MCG</span>
                                                                </div>
                                                                <div class="JCELDA-2 JCELDA-ESTILO-DERECHA">
                                                                    <span class="JETIQUETA">4.8 Ampollas</span>
                                                                </div>
                                                                <div class="JCELDA-2 JCELDA-ESTILO-DERECHA">
                                                                    <span class="JETIQUETA">Dosis Correcta</span>
                                                                </div>
                                                                <div class="JCELDA-1" >
                                                                    <img alt="" src="../Imagenes/anular.gif" class="JIMG-GENERAL" style="height:14px;">
                                                                </div>
                                                            </div>--%>
                                                        </div>
                                                        <div class="JFILA">
                                                            <div class="JCELDA-12">
                                                                <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:GridView ID="gvCalculoDosis" runat="server" AutoGenerateColumns="False" 
                                                                            ShowHeaderWhenEmpty="True" CssClass="JSBTABLA" GridLines="None" 
                                                                            AllowPaging="false" PageSize="15" PagerStyle-CssClass="JPAGINADO" >
                                                                            <Columns>
                                                                                <asp:BoundField DataField="Item" HeaderText="N°" >
                                                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="dsc_dci"  HeaderText="Farmaco" >
                                                                                    <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" Width="30%" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="dosis_kg_hora"  HeaderText="Dosis por Kg por Hora" >
                                                                                    <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" Width="20%" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="dosis_dia"  HeaderText="Dosis por Día" >
                                                                                    <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" Width="20%" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="dsc_prescripcion"  HeaderText="Prescripción" >
                                                                                    <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" Width="20%" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="semaforo"  HeaderText="Semáforo" HtmlEncode="false" HtmlEncodeFormatString="true" >
                                                                                    <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" Width="10%" />
                                                                                </asp:BoundField>
                                                                                <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>                            
                                                                                        <img alt="" src="../Imagenes/anular.gif" class="JIMG-GENERAL" />                                    
                                                                                    </ItemTemplate>                                                                
                                                                                    <ItemStyle CssClass="Eliminar" Width="5%" />
                                                                                </asp:TemplateField>
                                                                            </Columns>                                                                            
                                                                        </asp:GridView>                                                                                                                                                                                                                         
                                                                        <asp:Button runat="server" ID="Button1" style="display:none;"/>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>--%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                
                                                
                                            </div>
                                            <div class="JCELDA-6">
                                                <div class="JFILA">
                                                    <div class="JCELDA-12">
                                                        <%--<div style="border:1px solid #4BACFF;width:100%;max-height:360px;overflow-y:auto;" id="divControlClinicoIndicacionMed"> 
                                                            
                                                        </div>--%>
                                                        <div style="border:1px solid #4BACFF;width:100%;max-height:360px;overflow-y:auto;" id="divControlClinicoIndicacionMed2"> <%--CONTENEDOR DE CONTROL CLINICO E INDICACIOENS MEDICAS--%>

                                                        </div>
                                                    </div>
                                                </div>
                                                <%--<div class="JFILA">
                                                    <div class="JCELDA-12">
                                                        <div style="border:1px solid #4BACFF;width:100%;max-height:360px;overflow-y:auto;" id="divControlClinicoIndicacionMed2"> 
                                                            <ul class="JTreeView"><li><span class="nudo"><img alt="" src="../Imagenes/Pastilla.png"><span style="padding-top:5px;">19/08/2019</span></span><ul class="anidado"><li><span class="nudo">04.41 PM</span><ul class="anidado"><li class="JTree-Element">DOXIUM 500 X100CAPS ORAL SOLID 24 horas UNA VEZ AL DIA EN LA MAÑANA</li></ul></li><li><span class="nudo">12.55 PM</span><ul class="anidado"><li class="JTree-Element">NUTRICION NUTRICION</li><li class="JTree-Element">TERAPIA FISICA Y REHABILITACION TERAPIA</li><li class="JTree-Element">CUIDADOS DE ENFERMERIA CUIDADO</li><li class="JTree-Element">OTROS OTROS</li></ul></li><li><span class="nudo">12.35 PM</span><ul class="anidado"><li class="JTree-Element">PANADOL JARABE x 60ML. ORAL LIQUIDA_ 12 horas DESPUES DE CADA COMIDA</li><li class="JTree-Element">PARACETAMOL 500MG/50ML IV UND ORAL LIQUIDA 24 horas 1 VEZ AL DIA</li></ul></li></ul></li><li><span class="nudo"><img alt="" src="../Imagenes/Pastilla.png"><span style="padding-top:5px;">21/08/2019</span></span><ul class="anidado"><li><span class="nudo">11.26 AM</span><ul class="anidado"><li class="JTree-Element">NUTRICION GELATINAS</li><li class="JTree-Element">TERAPIA FISICA Y REHABILITACION EJERCICIOS</li><li class="JTree-Element">OTROS MANTENERLO SEDADO</li></ul></li><li><span class="nudo">11.04 AM</span><ul class="anidado"><li class="JTree-Element">AMOXIL 500 MG x 100 CAPS. ORAL SOLIDA 24 horas TOMAR DESPUES DE LAS COMIDAS</li><li class="JTree-Element">CLOROALERGAN GOTAS X 20 ML OFTALMIC 24 horas AL DESPERTAR</li></ul></li></ul></li></ul>
                                                                                                                        
                                                        </div>
                                                    </div>
                                                </div>--%>
                                                <div class="JFILA">
                                                    <div class="JCELDA-12">
                                                        <div class="JDIV-CONTROLES">
                                                            <input type="button" id="imgNuevoControlClinico" name="03/01/05" value="Nuevo" />
                                                            <input type="button" id="btnCopiarCC" name="03/01/02" value="Copiar" />
                                                            <input type="button" id="btnSuspenderCC" name="03/01/03" value="Suspender" />
                                                            <input type="button" id="btnVerificarCC" name="03/01/04" value="Verificar" style="display:none;" />
                                                            <input type="button" id="btnImprimirCC" value="Imprimir" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <label class="JDIV-ACOR-TITU" name="labelenfermeria" id="kardexEnfermerialabel"  >Kardex Enfermería</label>
                        <div class="JDIV_ACOR_CONTENIDO"  name="componentenfermeria">
                            <div class="JFILA">
                                <div class="JCELDA-12">
                                    <div style="border: 1px solid #BABABA;padding:5px;width:100%;overflow-y:auto;height:99%;background: #F5F5F5;border-radius: 5px;float: left;">
                                        <div class="JFILA" id="contenidokardexhospitalario">
                                            <div class="JCELDA-12">  
                                                <div class="JTABS" style="width:100%" id="tablaDatosKardexEnfermeria">
                                                   
                                                </div>
                                            </div>  
                                            <div class="JCELDA-12">
                                               <div class="JCELDA-12" style="height: 40px;margin-top: 10px;">
                                                   <label style="font-size:18px; font-weight:bold;">Datos históricos</label>
                                               </div>
                                                <div class="JCELDA-12"> 
                                                    <div class="JCELDA-3"><label style="font-size:18px; font-weight:bold;">DE: &nbsp;&nbsp;</label> <input type="date" style=" width: 160px; height: 40px; font-size: 18px;"   id="fechaInicioKardexenfermeria"/></div>
                                                    <div class="JCELDA-3"><label style="font-size:18px; font-weight:bold;">HASTA: &nbsp;&nbsp;</label> <input type="date"  style=" width: 160px; height: 40px; font-size: 18px;" id="fechaFINKardexenfermeria"/></div>
                                                    <div class="JCELDA-2"><input type="button" value="Buscar" style="position:absolute; left:1px;" class="buttonescalaseindicaciones" onclick="Listar_datos_historicosKardexemfermeria()"/></div>
                                                 </div>
                                                 <div class="JCELDA-12" style="margin-top: 10px; margin-bottom: 10px;"> 
                                                     <label style="font-size:18px; font-weight:bold;">Resultado de búsqueda</label>
                                                 </div>
                                                <div class="JCELDA-12" id="tablaDatosHistoricoskardexEnfermeria"></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <label class="JDIV-ACOR-TITU" name="labelenfermeria" id="escalaseintervencionlabel">Escalas e Intervenciones Enfermería</label>
                        <div class="JDIV_ACOR_CONTENIDO" name="componentenfermeria">
                            <div class="JFILA">
                                <div class="JCELDA-12">
                                    <div style="border: 1px solid #BABABA;padding:5px;width:100%;overflow-y:auto;height:99%;background: #F5F5F5;border-radius: 5px;float: left;">
                                        <div class="JFILA" id="contenidoescalahospitalario">
                                            <div class="JCELDA-12">
                                                <div class="JTABS" style="width:100%">
                                                   <asp:ListView runat="server" ID="lvEscalaeIntervenciones">
                                                       <ItemTemplate>
                                                           <input type="radio" id="tabescalaeintervenciones<%# Eval("groupcab")%>" name="TabNro2" class="JCHEK-TABS" />
                                                           <label for="tabescalaeintervenciones<%# Eval("groupcab")%>" class="JTABS-LABEL"><%#Eval("dsc_detalle") %></label>   
                                                       </ItemTemplate>
                                                   </asp:ListView>

                                                  <asp:ListView runat="server" ID="lvEscalaeIntervencionedetalle" OnItemDataBound="lvEscalaeIntervencionedetalle_ItemDataBound">
                                                            <ItemTemplate>
                                                                <div class="JCONTENIDO-TAB">
                                                                    <div class="JFILA">
                                                                        <div class="JCELDA-12">
                                                                            <asp:Label runat="server" ID="lblIdEscalas" Visible="false" Text='<%#Eval("groupcab") %>'></asp:Label>     
                                                                            <asp:ListView runat="server" ID="lvdetalleEscalaEIndicaciones" >
                                                                                <ItemTemplate>
                                                                                    <%#Eval("descripcion_det") %>
                                                                                </ItemTemplate>
                                                                            </asp:ListView>
                                                                        </div>     
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:ListView>
                                                </div>
                                              
                                            </div>
                                            <div class="JCELDA-12">
                                               <div class="JCELDA-12" style="height: 40px;margin-top: 10px;">
                                                   <label style="font-size:18px; font-weight:bold;">Datos históricos</label>
                                               </div>
                                                <div class="JCELDA-12"> 
                                                    <div class="JCELDA-3"><label style="font-size:18px; font-weight:bold;">DE: &nbsp;&nbsp;</label> <input type="date" style=" width: 160px; height: 40px; font-size: 18px;"   id="fechaInicioEscalaEIntervencionesenfermeria"/></div>
                                                    <div class="JCELDA-3"><label style="font-size:18px; font-weight:bold;">HASTA: &nbsp;&nbsp;</label> <input type="date"  style=" width: 160px; height: 40px; font-size: 18px;" id="fechaFINEscalaEIntervencionesenfermeria"/></div>
                                                    <div class="JCELDA-2"><input type="button" value="Buscar" style="position:absolute; left:1px;" class="buttonescalaseindicaciones" onclick="Listar_datos_historicosEscalaeIntervenciones()"/></div>
                                                 </div>
                                                 <div class="JCELDA-12" style="margin-top: 10px; margin-bottom: 10px;"> 
                                                     <label style="font-size:18px; font-weight:bold;">Resultado de búsqueda</label>
                                                 </div>
                                                <div class="JCELDA-12" id="tablaDatosHistoricosEscalaeindicacionesEnfermeria"></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div> 

                        <label class="JDIV-ACOR-TITU" name="labelmedicina">Laboratorio</label>
                        <div class="JDIV_ACOR_CONTENIDO" name="componentmedicina">
                            <div class="JFILA">
                                <div class="JCELDA-12">
                                    <div style="border: 1px solid #BABABA;padding:5px;width:100%;overflow-y:auto;height:99%;background: #F5F5F5;border-radius: 5px;float: left;">
                                        <div class="JFILA">
                                            <div class="JCELDA-6">
                                                <div class="JDIV-CONTROLES">                                        
                                                    <div > <%--class="JFILA"--%>
                                                        <div class="JCELDA-3">
                                                            <div class="JDIV-CONTROLES">
                                                                <span class="JETIQUETA">Análisis:</span>
                                                            </div>
                                                        </div>
                                                        <div class="JCELDA-7" style="overflow:initial;overflow:initial;position:static;">
                                                            <div class="JDIV-CONTROLES">
                                                                <input type="text" id="txtAnalisisLaboratorio" class="JTEXTO" />
                                                                <span id="spCodigoAnalisisLaboratorioSeleccionado" class="JCOL-OCULTA"></span>
                                                                <span id="spPerfil" class="JCOL-OCULTA"></span>
                                                                <div id="divBusquedaLaboratorio" class="JBUSQUEDA-ESPECIAL" style="width:65%">  
                                                                                                                                
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="JCELDA-2">
                                                            <div class="JDIV-CONTROLES">
                                                                <%--<img src="../Imagenes/Buscar.png" id="imgBuscarAnalisisLaboratorio" alt="" class="JIMG-BUSQUEDA" />--%>
                                                                <input type="button" id="imgBuscarAnalisisLaboratorio" class="JBOTON-IMAGEN" style="background-image:url(../Imagenes/Buscar.png);" />
                                                                <%--<img src="../Imagenes/Favoritos.png" id="imgFavoritosAnalisisLaboratorio" alt="" class="JIMG-FAVORITO" />--%>
                                                                <input type="button" id="imgFavoritosAnalisisLaboratorio" class="JBOTON-IMAGEN" style="background-image:url(../Imagenes/Favoritos.png);" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div > <%--class="JFILA"--%>
                                                        <div class="JCELDA-4" style="position:initial;">
                                                            <div class="JDIV-CONTROLES">
                                                                <input type="checkbox" id="chkAnalisisLaboratorioSeleccionado" disabled="disabled" />
                                                                <span class="JETIQUETA_2" id="spAnalisisLaboratorioSeleccionado"></span>
                                                            </div>
                                                        </div>
                                                        <div class="JCELDA-7" style="position:initial;">
                                                            <div class="JDIV-CONTROLES">
                                                                <div style="float:right;">
                                                                <input type="checkbox" id="chkAnalisisLaboratorioFavorito" />
                                                                <span class="JETIQUETA_2">Favorito</span>                                                    
                                                                <input type="hidden" id="hfIdeFavoritoAnalisis" />

                                                                <input type="checkbox" id="chkAnalisisTipo1" class="JCOL-OCULTA" />
                                                                <span class="JETIQUETA_2 JCOL-OCULTA" id="spAnalisisTipo1"></span>
                                                                <input type="hidden" id="hdfIdeTipoAnalisis1" />

                                                                <input type="checkbox" id="chkAnalisisTipo2" class="JCOL-OCULTA" />
                                                                <span class="JETIQUETA_2 JCOL-OCULTA" id="spAnalisisTipo2"></span>
                                                                <input type="hidden" id="hdfIdeTipoAnalisis2" />

                                                                <input type="checkbox" id="chkAnalisisTipo3" class="JCOL-OCULTA" />
                                                                <span class="JETIQUETA_2 JCOL-OCULTA" id="spAnalisisTipo3"></span>
                                                                <input type="hidden" id="hdfIdeTipoAnalisis3" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="JCELDA-1" style="position:initial;">
                                                            <div class="JDIV-CONTROLES">
                                                                <%--<img src="../Imagenes/Agregar.png" name="05/01/01" id="imgAgregarLaboratorio" alt="" class="JIMG-BUSQUEDA" />--%>
                                                                <input type="button" id="imgAgregarLaboratorio" class="JBOTON-IMAGEN" name="05/01/01" style="background-image:url(../Imagenes/Agregar.png);" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div > <%--class="JFILA"--%>
                                                        <div class="JCELDA-12" style="position:initial;">
                                                            <div class="JDIV-CONTROLES">
                                                                <span class="JETIQUETA_3">Analisis Seleccionados</span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div > <%--class="JFILA"--%>
                                                        <div class="JCELDA-12">
                                                            <div class="JDIV-CONTROLES">
                                                                <div id="divGridLaboratorio" style="max-height:205px;overflow-y:auto;">
                                                    
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div > <%--class="JFILA"--%>
                                                        <div class="JCELDA-2" style="position:initial;">
                                                            <div class="JDIV-CONTROLES">
                                                                <span class="JETIQUETA_3">Programar Hora</span>
                                                            </div>
                                                        </div>
                                                        <div class="JCELDA-1" style="position:initial;">
                                                            <div class="JDIV-CONTROLES">
                                                                <input type="checkbox" id="chkProgramarHoraLab" />
                                                            </div>
                                                        </div>
                                                        <div class="JCELDA-2" style="position:initial;">
                                                            <div class="JDIV-CONTROLES">
                                                                <input type="text" id="txtFechaProgramarHoraLab" disabled="disabled" class="JFECHA" />
                                                            </div>
                                                        </div>
                                                        <div class="JCELDA-3" style="position:initial;">
                                                            <div class="JDIV-CONTROLES">
                                                                <input type="text" id="txtHoraProgramarHoraLab" disabled="disabled" class="JHORA" placeholder="23:59" />
                                                                <span class="JETIQUETA_4">Formato 24hrs</span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div > <%--class="JFILA"--%>
                                                        <div class="JCELDA-12" style="position:initial;">
                                                            <div class="JDIV-CONTROLES">
                                                                <span class="JETIQUETA_3">Observación</span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div > <%--class="JFILA"--%>
                                                        <div class="JCELDA-12" style="position:initial;">
                                                            <div class="JDIV-CONTROLES">
                                                                <textarea rows="3" cols="1" id="txtObservacionAnalisisLaboratorio" class="JTEXTO" ></textarea>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div > <%--class="JFILA"--%>
                                                        <div class="JCELDA-2" style="position:initial;overflow:initial;position:static;">
                                                            <div class="JDIV-CONTROLES">
                                                                <div class="tooltip">
                                                                    <input type="button" id="imgEnviarSolicitudAnalisisLaboratorio" title="Enviar la Orden del Petitorio de Laboratorio." class="JBOTON-IMAGEN" name="05/01/02" style="background-image:url(../Imagenes/Enviar_Solicitud.png);" /> <%--Enviar_Solicitud.png CAMBIO IMG   Enviar1 width:35px;height:35px;background-size: 30px 30px;--%>
                                                                    <span tooltip-direccion="derecha">Enviar la Orden del Petitorio de Laboratorio.</span> 
                                                                </div>                                                                
                                                                <div class="tooltip">
                                                                    <input type="button" id="imgPetirorioLaboratorio" title="Mostrar el Petitorio de Laboratorio." class="JBOTON-IMAGEN" name="05/01/03" style="background-image:url(../Imagenes/Petitorio_Laboratorio.png);" />  <%--Petitorio_Laboratorio.png  CAMBIO IMG  Microscopio1 width:35px;height:35px;background-size: 30px 30px;--%>
                                                                    <span tooltip-direccion="arriba">Mostrar el Petitorio de Laboratorio.</span> 
                                                                </div>
                                                                
                                                            </div>
                                                        </div>


                                                        <%--<div class="JCELDA-1" style="position:initial;overflow:initial;position:static;">
                                                            <div class="JDIV-CONTROLES">
                                                                
                                                            </div>
                                                        </div>--%>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="JCELDA-6">
                                                <div class="JDIV-CONTROLES">                                        
                                                    <div class="JFILA">
                                                        <div class="JCELDA-12">
                                                            <%--<div style="border:1px solid #4BACFF;width:100%;height:100%; min-height:250px;max-height:450px;overflow:auto;" id="divTreeLaboratorio">
                                                    
                                                            </div>--%>
                                                            <div style="border:1px solid #4BACFF;width:100%;height:100%; min-height:250px;max-height:450px;overflow:auto;" id="divTreeLaboratorio2">
                                                                <%--<div id="divFecha1" class="JTREE3-FECHA">
                                                                    <div class="JTREE3-SIGNO"></div><div class="JLAB-ROJO"></div><div class="JVALOR-FECHA">19/07/2020</div>
                                                                    <div class="JTREE3-HORA">
                                                                        <div class="JFILA-HORA">
                                                                            <div class="JTREE3-SIGNO"></div><div class="JLAB-ROJO"></div><div class="JVALOR-HORA"> 167254 - DIANA ELVIA | 01:15 PM</div><input type="hidden" id="Hidden8" value="167254" />
                                                                        </div>
                                                                        <div class="JFILA-DETALLE">
                                                                            <div class="JTREE3-DETALLE">
                                                                                <div class="JTREE3-SIGNO"></div><div class="JLAB-VERDE"></div><div class="JVALOR-HORA">UROCULTIVO</div><input type="hidden" id="Hidden9" value="167254"  /><input type="hidden" id="Hidden10" value="" />
                                                                            </div>
                                                                            <div class="JTREE3-DETALLE">
                                                                                <div class="JTREE3-SIGNO"></div><div class="JLAB-VERDE"></div><div class="JVALOR-HORA">GASES ARTERIALES</div><input type="hidden" id="Hidden11" value="167254"  /><input type="hidden" id="Hidden12" value="" />
                                                                            </div>
                                                                        </div>                                                                        
                                                                    </div>                                                                     
                                                                </div>
                                                                <div id="divFecha2" class="JTREE3-FECHA">
                                                                    <div class="JFILA-FECHA">
                                                                        <div class="JTREE3-SIGNO"></div><div class="JLAB-ROJO"></div><div class="JVALOR-FECHA">18/07/2020</div>
                                                                    </div> 
                                                                    <div class="JTREE3-HORA">
                                                                        <div class="JFILA-HORA">
                                                                            <div class="JTREE3-SIGNO"></div><div class="JLAB-ROJO"></div><div class="JVALOR-HORA"> 167254 - DIANA ELVIA | 01:15 PM</div><input type="hidden" id="Hidden3" value="167254" />
                                                                        </div>
                                                                        <div class="JFILA-DETALLE">
                                                                            <div class="JTREE3-DETALLE">
                                                                                <div class="JTREE3-SIGNO"></div><div class="JLAB-VERDE"></div><div class="JVALOR-HORA">UROCULTIVO</div><input type="hidden" id="Hidden4" value="167254"  /><input type="hidden" id="Hidden5" value="" />
                                                                            </div>
                                                                            <div class="JTREE3-DETALLE">
                                                                                <div class="JTREE3-SIGNO"></div><div class="JLAB-VERDE"></div><div class="JVALOR-HORA">GASES ARTERIALES</div><input type="hidden" id="Hidden6" value="167254"  /><input type="hidden" id="Hidden7" value="" />
                                                                            </div>
                                                                        </div>                                                                        
                                                                    </div>                                                                                                                                      
                                                                </div>
                                                                <div id="divFecha3" class="JTREE3-FECHA">
                                                                    <div class="JFILA-FECHA">
                                                                        <div class="JTREE3-SIGNO"></div><div class="JLAB-ROJO"></div><div class="JVALOR-FECHA">16/07/2020</div>
                                                                    </div>                                                                    
                                                                    <div class="JTREE3-HORA">
                                                                        <div class="JFILA-HORA">
                                                                            <div class="JTREE3-SIGNO"></div><div class="JLAB-ROJO"></div><div class="JVALOR-HORA"> 167254 - DIANA ELVIA | 01:15 PM</div><input type="hidden" id="idRecetaCab3" value="167254" />
                                                                        </div>
                                                                        <div class="JFILA-DETALLE">
                                                                            <div class="JTREE3-DETALLE">
                                                                                <div class="JTREE3-SIGNO"></div><div class="JLAB-VERDE"></div><div class="JVALOR-HORA">UROCULTIVO</div><input type="hidden" id="idRecetaCab4" value="167254"  /><input type="hidden" id="FlgVerificarLaboratorio" value="" />
                                                                            </div>
                                                                            <div class="JTREE3-DETALLE">
                                                                                <div class="JTREE3-SIGNO"></div><div class="JLAB-VERDE"></div><div class="JVALOR-HORA">GASES ARTERIALES</div><input type="hidden" id="Hidden1" value="167254"  /><input type="hidden" id="Hidden2" value="" />
                                                                            </div>
                                                                        </div>                                                                        
                                                                    </div>                                                                    
                                                                </div>--%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="JFILA">
                                                        <div class="JCELDA-12" style="text-align:right;">
                                                            <input type="hidden" id="hfRecetaTreeViewSeleccionado" />
                                                            <input type="hidden" id="hfFlgVerificadoLab" />
                                                            <input type="hidden" id="hfEstadoAnalisis" />
                                                            <div class="tooltip">
                                                                <input type="button" id="imgRoeLaboratorio" title="ROE" class="JBOTON-IMAGEN" name="05/01/03" style="background-image:url(../Imagenes/ROE2.png);" style="background-size: 30px 30px;width: 30px;height: 30px;"/>  <%--Petitorio_Laboratorio.png  CAMBIO IMG  Microscopio1 width:35px;height:35px;background-size: 30px 30px;--%>
                                                                <span tooltip-direccion="arriba">ROE</span> 
                                                            </div>
                                                            <input type="button" value="Ver Informe" name="05/01/04" id="btnVerInformeAnalisisLaboratorio" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <label class="JDIV-ACOR-TITU" name="labelmedicina">Imágenes</label>
                        <div class="JDIV_ACOR_CONTENIDO" name="componentmedicina">
                            <div class="JFILA">
                                <div class="JCELDA-12">
                                    <div style="border: 1px solid #BABABA;padding:5px;width:100%;overflow-y:auto;height:99%;background: #F5F5F5;border-radius: 5px;float: left;">
                                        <div class="JFILA">
                                            <div class="JCELDA-6">
                                                <div class="JDIV-CONTROLES">                                        
                                                    <div > <%--class="JFILA"--%>
                                                        <div class="JCELDA-3">
                                                            <div class="JDIV-CONTROLES">
                                                                <span class="JETIQUETA">Imágenes:</span>
                                                            </div>
                                                        </div>
                                                        <div class="JCELDA-7" style="overflow:initial;overflow:initial;position:static;">
                                                            <div class="JDIV-CONTROLES">
                                                                <input type="text" id="txtImagen" class="JTEXTO" />
                                                                <span id="spCodigoImagenSeleccionado" class="JCOL-OCULTA"></span>
                                                                <div id="divBusquedaImagen" class="JBUSQUEDA-ESPECIAL" style="width:65%;" >                                                                                 
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="JCELDA-2">
                                                            <div class="JDIV-CONTROLES">
                                                                <%--<img src="../Imagenes/Buscar.png" id="imgBusquedaImagenes" alt="" class="JIMG-BUSQUEDA" />--%>
                                                                <input type="button" id="imgBusquedaImagenes" class="JBOTON-IMAGEN" style="background-image:url(../Imagenes/Buscar.png);" />
                                                                <%--<img src="../Imagenes/Favoritos.png" id="imgFavoritoImagenes" alt="" class="JIMG-FAVORITO" />--%>
                                                                <input type="button" id="imgFavoritoImagenes" class="JBOTON-IMAGEN" style="background-image:url(../Imagenes/Favoritos.png);" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div > <%--class="JFILA"--%>
                                                        <div class="JCELDA-3" style="position:initial;">
                                                            <div class="JDIV-CONTROLES">
                                                                <input type="checkbox"  id="chkImagenSeleccionado" disabled="disabled" />
                                                                <span class="JETIQUETA_2" id="spImagenSeleccionado"></span>
                                                            </div>
                                                        </div>
                                                        <div class="JCELDA-2 JESPACIO-IZQ-5" style="position:initial;">
                                                            <div class="JDIV-CONTROLES">
                                                                <input type="checkbox" id="chkFavoritoImagen" />
                                                                <span class="JETIQUETA_2">Favorito</span>
                                                                <input type="hidden" id="hfFavoritoImagen" />
                                                            </div>
                                                        </div>
                                                        <div class="JCELDA-2" style="position:initial;">
                                                            <div class="JDIV-CONTROLES">
                                                                <%--<img src="../Imagenes/Agregar.png" name="06/01/01" id="imgAgregarImagen" alt="" class="JIMG-BUSQUEDA" />--%>
                                                                <input type="button" id="imgAgregarImagen" name="06/01/01" class="JBOTON-IMAGEN" style="background-image:url(../Imagenes/Agregar.png);" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div > <%--class="JFILA"--%>
                                                        <div class="JCELDA-12" style="position:initial;">
                                                            <div class="JDIV-CONTROLES">
                                                                <span class="JETIQUETA_3">Examenes Seleccionados</span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div > <%--class="JFILA"--%>
                                                        <div class="JCELDA-12">
                                                            <div class="JDIV-CONTROLES">
                                                                <div id="divGridImagen">    
                                                                                                
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div > <%--class="JFILA"--%>
                                                        <div class="JCELDA-12" style="position:initial;">
                                                            <div class="JDIV-CONTROLES">
                                                                <span class="JETIQUETA_3">Observación</span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div > <%--class="JFILA"--%>
                                                        <div class="JCELDA-12" style="position:initial;">
                                                            <div class="JDIV-CONTROLES">
                                                                <textarea rows="3" cols="1" id="txtObservacionImagen" class="JTEXTO" ></textarea>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div > <%--class="JFILA"--%>
                                                        <div class="JCELDA-2" style="position:initial;overflow:initial;position:static;">
                                                            <div class="JDIV-CONTROLES">
                                                                <div class="tooltip">
                                                                    <input type="button" id="imgEnviarSolicitudImagen" name="06/01/02" title="Enviar la Orden del Petitorio de Imágenes." class="JBOTON-IMAGEN" style="background-image:url(../Imagenes/Enviar_Solicitud.png);" />   <%--Enviar_Solicitud.png  CAMBIO IMG   Enviar1.png);width:35px;height:35px;background-size:30px 30px;--%>
                                                                    <span tooltip-direccion="derecha">Enviar la Orden del Petitorio de Imágenes.</span> 
                                                                </div>

                                                                
                                                                <%--<img src="../Imagenes/Petitorio_Imagenes.png" alt=""  name="06/01/03" id="imgPetitorioImagen" title="Mostrar el Petitorio de Imágenes." class="JIMG-GENERAL JIMG-IMAGEN" />--%>
                                                                <div class="tooltip">
                                                                    <input type="button" id="imgPetitorioImagen" name="06/01/03" title="Mostrar el Petitorio de Imágenes." class="JBOTON-IMAGEN" style="background-image:url(../Imagenes/Petitorio_Imagenes.png);" />  <%--Petitorio_Imagenes.png CAMBIO IMG   Imagen4.png);width:35px;height:35px;background-size:30px 30px;--%>
                                                                    <span tooltip-direccion="arriba">Mostrar el Petitorio de Imágenes.</span> 
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <%--<div class="JCELDA-1" style="position:initial;overflow:initial;position:static;">
                                                            <div class="JDIV-CONTROLES">
                                                                
                                                            </div>
                                                        </div>--%>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="JCELDA-6">
                                                <div class="JDIV-CONTROLES">                                        
                                                    <div class="JFILA">
                                                        <div class="JCELDA-12">
                                                            <%--<div style="border:1px solid #4BACFF;width:100%;height:255px;overflow:auto;" id="divTreeViewImagenes">
                                                    
                                                            </div>--%>

                                                            <div style="border:1px solid #4BACFF;width:100%;height:255px;overflow:auto;" id="divTreeViewImagenes2">
                                                                <%--<div id="divFecha2" class="JTREE3-FECHA">
                                                                    <div class="JFILA-FECHA">
                                                                        <div class="JTREE3-SIGNO"></div><div class="JIMAG-ROJO"></div><div class="JVALOR-FECHA">18/07/2020</div><input type="hidden" value="18/07/2020">
                                                                    </div> 
                                                                    <div class="JTREE3-HORA">
                                                                        <div class="JFILA-HORA">
                                                                            <div class="JTREE3-SIGNO"></div><div class="JIMAG-ROJO"></div><div class="JVALOR-HORA"> 167254 - DIANA ELVIA | 01:15 PM</div><input type="hidden" value="H01764470469_0" /><input type="hidden" class="FlgVerificarIma" value="" /><input type="hidden" id="Hidden3" value="167254" />
                                                                        </div>
                                                                        <div class="JFILA-DETALLE">
                                                                            <div class="JTREE3-DETALLE">
                                                                                <div class="JTREE3-SIGNO"></div><div class="JIMAG-VERDE"></div><div class="JVALOR-HORA">003440808 - 1 PORTATIL CADA VIAJE - PISO</div><input type="hidden" value="H01764470469_0" /><input type="hidden" class="FlgVerificarIma" value="" /><input type="hidden" id="Hidden4" value="167254"  />
                                                                            </div>
                                                                            <div class="JTREE3-DETALLE">
                                                                                <div class="JTREE3-SIGNO"></div><div class="JIMAG-VERDE"></div><div class="JVALOR-HORA">003440808 - 1 PORTATIL CADA VIAJE - PISO</div><input type="hidden" value="H01764470469_0" /><input type="hidden" class="FlgVerificarIma" value="" /><input type="hidden" id="Hidden6" value="167254"  />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>--%>
                                                            </div>
                                                            
                                                        </div>
                                                    </div>
                                                    <div class="JFILA">
                                                        <div class="JCELDA-12" style="text-align:right;">
                                                            <input type="hidden" id="hfRecetaTreeViewSeleccionadoImagen" />
                                                            <input type="hidden" id="hfFlgVerificadoIma" />
                                                            <input type="hidden" id="IdeImagenDet" />
                                                            <input type="button" value="Ver Informe" id="btnVerInformeImagen" name="06/01/04" />
                                                            <input type="button" value="Ver Imagen" id="btnVerImagen" name="06/01/05" />
                                                            <input type="button" value="Cancelar Orden" id="btnCancelarOrden" style="display:none;" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <label class="JDIV-ACOR-TITU" name="labelmedicina">Interconsulta</label>
                        <div class="JDIV_ACOR_CONTENIDO" name="componentmedicina">
                            <div class="JFILA">
                                <div class="JCELDA-12">
                                    <div style="border: 1px solid #BABABA;padding:5px;width:100%;overflow-y:auto;height:99%;background: #F5F5F5;border-radius: 5px;float: left;">
                                         <div class="JFILA">
                                            <div class="JCELDA-6">
                                                <div class="JDIV-CONTROLES">
                                                    <div class="JFILA">
                                                        <div class="JCELDA-3">
                                                            <div class="JDIV-CONTROLES">
                                                                <span class="JETIQUETA">Motivo:</span>
                                                            </div>                                                
                                                        </div>
                                                        <div class="JCELDA-4">
                                                            <div class="JDIV-CONTROLES">
                                                                <asp:DropDownList runat="server" ID="ddlMotivo" CssClass="JSELECT"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div ><%--class="JFILA"--%>
                                                        <div class="JCELDA-3">
                                                            <div class="JDIV-CONTROLES">
                                                                <span class="JETIQUETA">Especialidad:</span>
                                                            </div>                                                
                                                        </div>
                                                        <div class="JCELDA-8" style="overflow:initial;overflow:initial;position:static;">
                                                            <div class="JDIV-CONTROLES">
                                                                <input type="text" id="txtEspecialidadInterconsulta" class="JTEXTO" />
                                                                <span id="spCodigoEspecialidadSeleccionado" class="JCOL-OCULTA"></span>
                                                                <div id="divBusquedaEspecialidad" class="JBUSQUEDA-ESPECIAL" style="width:65%;">                                                                                 
                                                                </div>
                                                            </div>                                                
                                                        </div>
                                                        <div class="JCELDA-1">
                                                            <div class="JDIV-CONTROLES">
                                                                <%--<img src="../Imagenes/Buscar.png" id="imgBusquedaEspecialidadInterconsulta" alt="" class="JIMG-BUSQUEDA" />--%>
                                                                <input type="button" id="imgBusquedaEspecialidadInterconsulta" class="JBOTON-IMAGEN" style="background-image:url(../Imagenes/Buscar.png);" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div><%--class="JFILA"--%>
                                                        <div class="JCELDA-3">
                                                            <div class="JDIV-CONTROLES">
                                                                <span class="JETIQUETA">Medico Interconsulta:</span>
                                                            </div>                                                
                                                        </div>
                                                        <div class="JCELDA-8" style="overflow:initial;overflow:initial;position:static;">
                                                            <div class="JDIV-CONTROLES">                                                    
                                                                <input type="text" id="txtMedicoInterconsulta" class="JTEXTO" />
                                                                <span id="spCodigoMedicoSeleccionado" class="JCOL-OCULTA"></span>
                                                                <div id="divBusquedaMedico" class="JBUSQUEDA-ESPECIAL" style="width:65%;">                                                                                 
                                                                </div>
                                                            </div>                                                
                                                        </div>
                                                        <div class="JCELDA-1">
                                                            <div class="JDIV-CONTROLES">
                                                                <%--<img src="../Imagenes/Buscar.png" id="imgBusquedaMedicoInterconsulta" alt="" class="JIMG-BUSQUEDA" />--%>
                                                                <input type="button" id="imgBusquedaMedicoInterconsulta" class="JBOTON-IMAGEN" style="background-image:url(../Imagenes/Buscar.png);" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div> <%--class="JFILA"--%>
                                                        <div class="JCELDA-12">
                                                            <div class="JDIV-CONTROLES">
                                                                <textarea rows="5" cols="1" id="txtDescripcionInterconsulta" class="JTEXTO" ></textarea>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div> <%--class="JFILA"--%>
                                                        <div class="JCELDA-12">
                                                            <div class="JDIV-CONTROLES">
                                                                <input type="hidden" id="hfIdInterconsulta" />
                                                                <input type="button" value="Aceptar" id="btnGuardarInterconsulta" name="07/01/01" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="JFILA">
                                                        <div class="JCELDA-12">
                                                            <div class="JDIV-CONTROLES">
                                                                <br /><br />
                                                            </div>
                                                        </div>
                                                    </div>                                        
                                                </div>
                                            </div>
                                            <div class="JCELDA-6">
                                                <div class="JDIV-CONTROLES">
                                                    <div class="JFILA">
                                                        <div class="JCELDA-3">
                                                            <div class="JDIV-CONTROLES">
                                                                <span class="JETIQUETA_3">Lista de Interconsultas</span>
                                                            </div>                                                
                                                        </div>
                                                    </div>
                                                    <div class="JFILA">
                                                        <div class="JCELDA-12">
                                                            <div class="JDIV-CONTROLES">
                                                                <div id="divGridInterconsulta">
                                                    
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                
                                         </div>
                                    </div>
                                </div>
                            </div>                             
                        </div>

                        <label class="JDIV-ACOR-TITU" name="labelmedicina">Procedimientos Médicos</label>
                        <div class="JDIV_ACOR_CONTENIDO" name="componentmedicina">
                            <div class="JFILA">
                                <div class="JCELDA-12">
                                    <div style="border: 1px solid #BABABA;padding:5px;width:100%;overflow-y:auto;height:99%;background: #F5F5F5;border-radius: 5px;float: left;">
                                        <div class="JFILA">
                                            <div class="JCELDA-6">
                                                <div class="JDIV-CONTROLES">
                                                    <div class="JFILA">
                                                        <div class="JCELDA-3">
                                                            <div class="JDIV-CONTROLES">
                                                                <span class="JETIQUETA">Procedimiento:</span>
                                                            </div>                                                
                                                        </div>
                                                        <div class="JCELDA-6" style="overflow:initial;overflow:initial;position:static;">
                                                            <div class="JDIV-CONTROLES">
                                                                <input type="text" id="txtProcedimientoMedico" class="JTEXTO" />
                                                                <span id="spCodigoProcedimientoSeleccionado" class="JCOL-OCULTA"></span>
                                                                <div id="divBusquedaProcedimientoMedico" class="JBUSQUEDA-ESPECIAL" style="width:65%;overflow-x:hidden;">                                                                                 
                                                                </div>
                                                            </div>                                                
                                                        </div>                                                        
                                                        <div class="JCELDA-3">
                                                            <div class="JDIV-CONTROLES">
                                                                <%--<img src="../Imagenes/Buscar.png" id="imgBusquedaEspecialidadInterconsulta" alt="" class="JIMG-BUSQUEDA" />--%>
                                                                <input type="button" id="imgBusquedaProcedimientoMedico" class="JBOTON-IMAGEN" style="background-image:url(../Imagenes/Buscar.png);" />
                                                                <input type="button" id="imgFavoritoProcedimientoMedico" class="JBOTON-IMAGEN" style="background-image:url(../Imagenes/Favoritos.png);" />

                                                                <input type="checkbox" id="chkFavoritoProcedimientoMedico" />
                                                                <span class="JETIQUETA_2">Favorito</span>
                                                                <input type="hidden" id="hfFavoritoProcedimientoMedico" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="JFILA">
                                                        <div class="JCELDA-3">
                                                            <input type="checkbox" id="chkBuscarCodProcedimientoMedico" /><label for="chkBuscarCodProcedimientoMedico" class="JETIQUETA">Codigo CPT</label>
                                                        </div>
                                                    </div>
                                                    <div class="JFILA" style="margin:5px 0">
                                                        <div class="JCELDA-3"> <%--style="position:initial;"--%>
                                                            <div class="JDIV-CONTROLES">
                                                                <%--<textarea rows="3" cols="1" id="txtDescripcionProcedimiento" class="JTEXTO" ></textarea>--%>
                                                                <span class="JETIQUETA">Sección:</span>
                                                            </div>
                                                        </div>
                                                        <div class="JCELDA-8">
                                                            <div class="JDIV-CONTROLES">                                                                
                                                                <%--<asp:DropDownList runat="server" ID="DropDownList1" CssClass="JSELECT"></asp:DropDownList>--%>
                                                                <div class="JSELECT2" id="ddlSeccionProcedimientoMedico" style="padding:3px;">  
                                                                    <span class="JSELECT2-SELECCION">-</span>
                                                                    <div class="JSELECT2-CONTENEDOR">
                                                                        <%--<div class="JSELECT2-ITEM JSELECT2-ELEMENT JSELECT2-INVISIBLE">seleccione seccion:</div><div style="display:none">0</div>
                                                                        <div class="JSELECT2-ITEM JSELECT2-ELEMENT JSELECT2-INVISIBLE">Audi</div>       <div style="display:none">1</div>
                                                                        <div class="JSELECT2-ITEM JSELECT2-ELEMENT JSELECT2-INVISIBLE">BMW</div>        <div style="display:none">2</div>
                                                                        <div class="JSELECT2-ITEM JSELECT2-ELEMENT JSELECT2-INVISIBLE">Citroen</div>    <div style="display:none">3</div>
                                                                        <div class="JSELECT2-ITEM JSELECT2-ELEMENT JSELECT2-INVISIBLE">Ford</div>       <div style="display:none">4</div>
                                                                        <div class="JSELECT2-ITEM JSELECT2-ELEMENT JSELECT2-INVISIBLE">Honda</div>      <div style="display:none">5</div>
                                                                        <div class="JSELECT2-ITEM JSELECT2-ELEMENT JSELECT2-INVISIBLE">Jaguar</div>     <div style="display:none">6</div>
                                                                        <div class="JSELECT2-ITEM JSELECT2-ELEMENT JSELECT2-INVISIBLE">Land Rover</div> <div style="display:none">7</div>
                                                                        <div class="JSELECT2-ITEM JSELECT2-ELEMENT JSELECT2-INVISIBLE">Mercedes</div>   <div style="display:none">8</div>
                                                                        <div class="JSELECT2-ITEM JSELECT2-ELEMENT JSELECT2-INVISIBLE">Mini</div>       <div style="display:none">9</div>         
                                                                        <div class="JSELECT2-ITEM JSELECT2-ELEMENT JSELECT2-INVISIBLE">Toyota</div>     <div style="display:none">10</div>
                                                                        <div class="JSELECT2-ITEM JSELECT2-ELEMENT JSELECT2-INVISIBLE">Autoinjerto epidérmico en tronco y extremidades; cada 100 cm cuadrados adicionales o 1% de superficie corporal adicional en lactantes y niños. Registrar por separado adicionalmente al código del procedimiento primario </div><div style="display:none">11</div>--%>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="JFILA" style="margin:5px 0">
                                                        <div class="JCELDA-3"> <%--style="position:initial;"--%>
                                                            <div class="JDIV-CONTROLES">
                                                                <span class="JETIQUETA">Sub división Anatómica:</span>
                                                            </div>
                                                        </div>
                                                        <div class="JCELDA-8">
                                                            <div class="JDIV-CONTROLES">
                                                                <div class="JSELECT2" id="ddlSubSeccionProcedimientoMedico" style="padding:3px;">  
                                                                    <span class="JSELECT2-SELECCION">-</span>
                                                                    <div class="JSELECT2-CONTENEDOR">
                                                                        
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="JFILA" style="margin:5px 0">
                                                        <div class="JCELDA-3"> <%--style="position:initial;"--%>
                                                            <div class="JDIV-CONTROLES">
                                                                <span class="JETIQUETA">Descripcion CPT:</span>
                                                            </div>
                                                        </div>
                                                        <div class="JCELDA-8">
                                                            <div class="JDIV-CONTROLES">
                                                                <div class="JSELECT2" id="ddlDescripcionCPT" style="padding:3px;">
                                                                    <span class="JSELECT2-SELECCION">-</span>  
                                                                    <div class="JSELECT2-CONTENEDOR">
                                                                        
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="JFILA" style="margin:5px 0"> <%--JB - 03/05/2021 - NUEVO CAMPO RELATO--%>
                                                        <div class="JCELDA-3"> 
                                                            <div class="JDIV-CONTROLES">
                                                                <span class="JETIQUETA">Relato:</span>
                                                            </div>
                                                        </div>
                                                        <div class="JCELDA-8">
                                                            <div class="JDIV-CONTROLES">
                                                                <textarea id="txtRelatoCpt" rows="5" cols="1" maxlength="500" class="JTEXTO"></textarea>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="JFILA">
                                                        <div class="JCELDA-12">
                                                            <div class="JDIV-CONTROLES">  
                                                                <br />                                                              
                                                                <br />          
                                                                <input type="button" value="Aceptar" id="btnGuardarProcedimientoMedico" />                                                                
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="JCELDA-6">
                                                <div class="JDIV-CONTROLES">                                        
                                                    <div class="JFILA">
                                                        <div class="JCELDA-12">                                                           

                                                            <div style="border:1px solid #4BACFF;width:100%;height:255px;overflow:auto;" id="divTreeProcedimientoMedicos">
                                                                
                                                            </div>
                                                            
                                                        </div>
                                                    </div>                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <label class="JDIV-ACOR-TITU" name="labelmedicina">Patología</label>
                        <div class="JDIV_ACOR_CONTENIDO" name="componentmedicina">
                            <div class="JFILA">
                                <div class="JCELDA-12">
                                    <div style="border: 1px solid #BABABA;padding:5px;width:100%;overflow-y:auto;height:99%;background: #F5F5F5;border-radius: 5px;float: left;">
                                        
                                                <div class="JFILA">
                                                    <div class="JCELDA-7">
                                                        <div class="JFILA">
                                                            <div class="JCELDA-12 JCHECK-PATOLOGIA" id="DivTabs" runat="server">
                                                            </div>
                                                        </div>
                                                        <div class="JFILA">
                                                            <div class="JCELDA-12">
                                                                <button style="width: 15%; background-color: #8DC73F; border: none; color: white;
                                                                    float: right; height: 25px; cursor: pointer" onclick="fn_AgregarPatologia();return false;">
                                                                    <span style="display: inline-block; text-align: left; margin-left: 10%; vertical-align: middle;
                                                                        font-weight: bold;">Añadir </span>
                                                                    <img src="../Imagenes/Agregar2.png" class="del" style="width: 20px; height: 20px;
                                                                        display: inline-block; cursor: pointer; margin-left: 10px; position: relative;
                                                                        vertical-align: middle;" />
                                                                </button>
                                                                <asp:ImageButton ID="imgAgregarPatologia" runat="server" ImageUrl="../Imagenes/Agregar.png"
                                                                    ToolTip="Agregar Patología" OnClientClick="return false;" Style="display: none;" />
                                                            </div>
                                                            <asp:HiddenField ID="hfIdPatologiaSeleccionado" runat="server" />
                                                            <asp:HiddenField ID="hfCheckPatologiaSeleccionado" runat="server" />
                                                            <asp:Button runat="server" ID="btnAgregarPatologia" Style="display: none;" />
                                                        </div>                                                        
                                                        <div class="JFILA">
                                                            <div class="JCELDA-12">
                                                                <span class="JETIQUETA_3">Exámenes Patológicos</span>
                                                            </div>
                                                        </div>
                                                        <div class="JFILA">
                                                            <div class="JCELDA-12">
                                                                <div class="JSBDIV_TABLA" id="divGridPatologia">
                                                                    <asp:GridView ID="gvPatologia" runat="server" AlternatingRowStyle-CssClass="altrowstyle"
                                                                        AutoGenerateColumns="False" CssClass="JSBTABLA" ShowHeaderWhenEmpty="True" TabIndex="100"
                                                                        Width="100%" DataKeyNames="ide_patologia_mae">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="cod_prestacion" HeaderText="Código">
                                                                                <ItemStyle Width="15%" HorizontalAlign="Center" />
                                                                                <HeaderStyle Width="15%" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="dsc_prestacion" HeaderText="Descripción">
                                                                                <ItemStyle HorizontalAlign="Left" Width="50%" />
                                                                                <HeaderStyle Width="50%" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="dsc_muestra" HeaderText="Organo" HtmlEncode="false" HtmlEncodeFormatString="true">
                                                                                <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                                                <HeaderStyle Width="25%" />
                                                                            </asp:BoundField>
                                                                            <%--<asp:TemplateField HeaderText="Organo2">
                                                                                <ItemStyle HorizontalAlign="Left" Width="25%" CssClass="JCOL-OCULTA" />
                                                                                <HeaderStyle Width="25%" CssClass="JCOL-OCULTA" />
                                                                                <ItemTemplate>
                                                                                    <span id="txtMuestraPatologia" runat="server" class="ORGANO-OCULTO">
                                                                                        <%# Eval("dsc_muestra2")%></span>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>--%>
                                                                            <asp:BoundField DataField="dsc_muestra2" HeaderText="dsc_muestra2">
                                                                                <ItemStyle HorizontalAlign="Left" Width="25%" CssClass="JCOL-OCULTA ORGANO-OCULTO" />
                                                                                <HeaderStyle Width="25%" CssClass="JCOL-OCULTA" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="dsc_datoclinico" HeaderText="Datos Clinicos">
                                                                                <ItemStyle HorizontalAlign="Left" Width="5%" CssClass="JCOL-OCULTA" />
                                                                                <HeaderStyle Width="25%" CssClass="JCOL-OCULTA" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="cnt_examen" HeaderText="Cantidad" HtmlEncode="false" HtmlEncodeFormatString="true">
                                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                                                <HeaderStyle Width="10%" />
                                                                            </asp:BoundField>
                                                                            <%--<asp:TemplateField HeaderText="Cantidad2">
                                                                                <ItemStyle HorizontalAlign="Left" Width="25%" CssClass="JCOL-OCULTA" />
                                                                                <HeaderStyle Width="25%" CssClass="JCOL-OCULTA" />
                                                                                <ItemTemplate>
                                                                                    <span id="txtCantidadPatologia" runat="server" class="CANTIDAD-OCULTO">
                                                                                        <%# Eval("cnt_examen2").ToString().Trim()%></span>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>--%>
                                                                            <asp:BoundField DataField="cnt_examen2" HeaderText="cnt_examen2">
                                                                                <ItemStyle HorizontalAlign="Left" CssClass="JCOL-OCULTA CANTIDAD-OCULTO" />
                                                                                <HeaderStyle CssClass="JCOL-OCULTA" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="ide_patologia_mae" HeaderText="ide_patologia_mae">
                                                                                <ItemStyle HorizontalAlign="Left" CssClass="JCOL-OCULTA" />
                                                                                <HeaderStyle CssClass="JCOL-OCULTA" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="cod_patologico" HeaderText="cod_patologico">
                                                                                <ItemStyle HorizontalAlign="Left" CssClass="JCOL-OCULTA" />
                                                                                <HeaderStyle CssClass="JCOL-OCULTA" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="cod_presotor" HeaderText="cod_presotor">
                                                                                <ItemStyle HorizontalAlign="Left" CssClass="JCOL-OCULTA" />
                                                                                <HeaderStyle CssClass="JCOL-OCULTA" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>                            
                                                                                    <img alt="" src="../Imagenes/anular.gif" class="JIMG-GENERAL JIMG-ELIMINAR" />                                    
                                                                                </ItemTemplate>                                                                
                                                                                <ItemStyle CssClass="Eliminar" Width="5%" />
                                                                            </asp:TemplateField>
                                                                            <%--<asp:CommandField ButtonType="Image" DeleteImageUrl="~/Imagenes/anular.gif" ShowDeleteButton="True"
                                                                                HeaderText="Quitar">
                                                                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                                                <HeaderStyle Width="10%" />
                                                                            </asp:CommandField>--%>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                    <asp:HiddenField ID="hfOrganosSeleccionados" runat="server" />
                                                                    <asp:HiddenField ID="hfCantidadSeleccionados" runat="server" />
                                                                    <asp:Button runat="server" ID="btnActualizarGridPatologias" Style="display: none;" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <br />
                                                        <div class="JFILA">
                                                            <div class="JCELDA-2">
                                                                <div class="JDIV-CONTROLES" style="padding-top: 5px;">
                                                                    <span class="JETIQUETA_2">Dato Clínico</span>
                                                                </div>
                                                            </div>
                                                            <div class="JCELDA-12">
                                                                <asp:TextBox runat="server" ID="txtDatoClinicoPatologia" CssClass="JTEXTO"
                                                                    TextMode="MultiLine" Rows="3" Columns="1"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="JFILA">
                                                            <div class="JCELDA-2">
                                                                <div class="JDIV-CONTROLES" style="padding-top: 5px;">
                                                                    <span class="JETIQUETA_2">Fecha ultima Regla</span>
                                                                </div>
                                                            </div>
                                                            <div class="JCELDA-3">
                                                                <asp:TextBox ID="TxtFechaUltimaRegla" runat="server" CssClass="JFECHA" MaxLength="10"
                                                                    autocomplete="off" ></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="JFILA" style="overflow: initial;">
                                                            <div class="JCELDA-12" style="overflow: initial;">
                                                                <div class="tooltip">
                                                                    <input type="button" id="imgEnviarPatologia" title="Enviar Patología." class="JBOTON-IMAGEN" style="background-image:url(../Imagenes/Enviar_Solicitud.png);" onclick="fn_VALI_DIAG();" />  <%--Enviar_Solicitud.png  CAMBIO IMG   Enviar1.png);width:35px;height:35px;background-size:30px 30px;--%>
                                                                    <span tooltip-direccion="derecha">Enviar Patología.</span>
                                                                     <%--<asp:ImageButton ID="imgEnviarPatologia" runat="server" ImageUrl="~/Imagenes/Enviar_Solicitud.png"
                                                                        CssClass="JBOTON-IMAGEN BotonesImagenes" Enabled="false" ToolTip="Enviar Patología."
                                                                        Style="width: auto; height: auto;" OnClientClick="fn_VALI_DIAG();return false;" />--%>
                                                                </div>
                                                                &nbsp;
                                                                <div class="tooltip" style="display: none;">
                                                                    <asp:ImageButton ID="imgPetitorioPatolog" runat="server" ImageUrl="~/Imagenes/Resultados.png"
                                                                        CssClass="JBOTON-IMAGEN BotonesImagenes" ToolTip="Mostrar el Petitorio de Patología."
                                                                        Style="width: auto; height: auto;" OnClientClick="return fn_FechaFUR();" />
                                                                    <span tooltip-direccion="derecha">Mostrar el Petitorio de Patología.</span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="JCELDA-5">                                                        
                                                            <div class="JFILA">
                                                                <div class="JCELDA-12">
                                                                    <div style="min-height: 100px; max-height: 400px; overflow: auto; border: 1px solid #4BACFF;width: 100%;" id="divTreePatologia">
                                                                        <%--<div class="JDIV-CONTROLES" style="position: relative; max-width: 1000px;">
                                                                            <asp:TreeView ID="tvPatologia" runat="server">
                                                                                <RootNodeStyle Font-Bold="True" ForeColor="#134B8D" />
                                                                                <NodeStyle ForeColor="#134B8D" />
                                                                                <SelectedNodeStyle CssClass="SeleccionarArbol" Font-Bold="true" />
                                                                            </asp:TreeView>
                                                                        </div>--%>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="JFILA">
                                                                <div class="JCELDA-2 JESPACIO-IZQ-10">
                                                                    <input type="button" value="Ver Informe" id="btnVerInformePatologia" />
                                                                    <%--<asp:Button ID="btnVerInformePatologia" runat="server" Text="Ver Informe" CssClass="BotonGeneral" />--%>
                                                                    <%--OnClientClick="fn_AbrirInformeCardiologia();return false;"--%>
                                                                    <asp:HiddenField runat="server" ID="hfIdePatologiaDet" />
                                                                </div>
                                                            </div>
                                                            <%--
                                                            <div style="border:1px solid #4BACFF;width:100%;height:100%; min-height:250px;max-height:450px;overflow:auto;" id="divTreePatologia">
                                                                <ul class="JTreeView">                                                                              
                                                                    <li>
                                                                        <span class="nudo nudo-down JTREE2-SELECCIONADO"><img alt="" src="../Imagenes/Res_Laboratorio_Rojo.png"> 120959 - LITA | 12:51 PM</span>
                                                                        <input type="hidden" value="120959">
                                                                        <ul class="anidado active">
                                                                            <li class="JTree-Element">
                                                                                <input type="hidden" value="120959">
                                                                                <input type="hidden" class="FlgVerificarLab" value="">
                                                                                <img alt="" src="../Imagenes/Res_Laboratorio_Rojo.png">
                                                                                <span class="JETIQUETA_TREE0">ARSENICO SANGRE ORINA</span>
                                                                                <span class="JETIQUETA_TREE2"> </span> 
                                                                            </li>
                                                                            <li class="JTree-Element">
                                                                                <input type="hidden" value="120959">
                                                                                <input type="hidden" class="FlgVerificarLab" value="">
                                                                                <img alt="" src="../Imagenes/Res_Laboratorio_Rojo.png">
                                                                                <span class="JETIQUETA_TREE0">COLESTEROL TOTAL</span>
                                                                                <span class="JETIQUETA_TREE2"> </span>
                                                                            </li>
                                                                        </ul>
                                                                    </li>                                                                                                                                                                  
                                                                </ul>
                                                            </div>
                                                            --%>
                                                            <%--<div class="JFILA">
                                                                <div class="JCELDA-12">
                                                                    <div style="min-height: 100px; max-height: 400px; overflow: auto; border: 1px solid #4BACFF;
                                                                        font-size: 11px; width: 100%;">
                                                                        <div class="JDIV-CONTROLES" style="position: relative; max-width: 1000px;">
                                                                            <asp:TreeView ID="tvPatologiaAnterior" runat="server">
                                                                                <RootNodeStyle Font-Bold="True" ForeColor="#134B8D" />
                                                                                <NodeStyle ForeColor="#134B8D" />
                                                                                <SelectedNodeStyle CssClass="SeleccionarArbol" Font-Bold="true" />
                                                                            </asp:TreeView>
                                                                        </div>
                                                                    </div>                                                                        
                                                                </div>
                                                            </div>
                                                            <div class="JFILA">
                                                                <div class="JCELDA-2 JESPACIO-IZQ-10">
                                                                    <asp:Button ID="btnVerInformePatologiaAnterior" runat="server" Text="Ver Informe" CssClass="BotonGeneral" />                                                                        
                                                                    <asp:HiddenField runat="server" ID="hfIdePatologiaDetAnterior" />
                                                                </div>
                                                            </div>--%>                                                            
                                                    </div>
                                                </div>
                                            
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>        
        </div>


        <%--POPUP MEDICAMENTOSA--%>
        <div class="JCONTENIDO-POPUP-2" id="divPopUpProcedimientoConsentimiento" style="width:60%;z-index:9999;display:none;">
            <header>Seleccionar Procedimiento</header>
            <div class="JCUERPO-POPUP-2">
                <uc1:cuProcedimientoConsentimiento ID="cuProcedimientoConsentimiento1" runat="server" />
            </div>
            <footer>
                <input type="button" value="Aceptar" onclick="fn_AceptarSeleccionConsentimiento()" />
                <input type="button" value="Salir" onclick="fn_CancelaSeleccionConsentimiento()" />
            </footer>
        </div>                
                

        <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>--%>
                <%--POPUP ALTA MEDICA - EPICRISIS --%>
                <div class="JCONTENIDO-POPUP-2" id="divPopUpAltaMedicaEpicrisis" style="width:65%;z-index:9999;display:none;">
                    <header>Alta Médica</header>
                    <div class="JCUERPO-POPUP-2" style="max-height:550px;overflow-y:auto;">
                        <uc2:cuAltaMedicaEpicrisis ID="cuAltaMedicaEpicrisis1" runat="server" />
                    </div>
                    <footer>
                        <input type="button" value="Guardar" onclick="fn_AceptarAltaMedicaEpicrisis()" />
                        <input type="button" value="Salir" onclick="fn_CancelaAltaMedicaEpicrisis()" />
                    </footer>
                </div>                
            <%--</ContentTemplate>
        </asp:UpdatePanel>--%>

        <div class="JCONTENIDO-POPUP-2" id="divPopUpFechaReceta" style="width:30%;z-index:9999;display:none;">
            <header>Seleccionar Procedimiento</header>
            <div class="JCUERPO-POPUP-2" style="max-height:550px;overflow-y:auto;">
                <uc3:cuPopUpFechaReceta ID="cuPopUpFechaReceta1" runat="server" />                
            </div>
            <footer>
                <input type="button" value="Aceptar" onclick="fn_AceptarSeleccionFechaReceta()" />
                <input type="button" value="Salir" onclick="fn_CancelarSeleccionFechaReceta()" />
            </footer>
        </div>   
    </div>
    
    <script>   

        function clickDolor(value){
            var _value = value.value;
            var chk = value.checked;
            Activar_desactivarCheckDolor(_value, chk);
        }
     
        function clickGlascow(value) {
            // check if checkbox is checked
            var _value = value.value;
            var chk = value.checked;
            Activar_desactivarCheckGlascow(_value, chk);
        }

        function clickMaddox(value){
            // check if checkbox is checked
            var _value = value.value;
            var chk = value.checked;
            Activar_desactivarCheckMaddox(_value, chk);
        }  

        function clickBradden(value){
            var _value = value.value;
            var chk = value.checked;
            Activar_desactivarCheckBradden(_value, chk);
        }

        function clickRiegoCaida(value) {
          //  ObternerEdad();
            var _value = value.value;
            var chk = value.checked;
            Activar_desactivarRiegoCaida(_value, chk);
        }

        function Activar_desactivarRiegoCaida(_value, check)
        {
            var _val = _value.split("_");
            var itemcab = _val[1];
            var puntaje = _val[3];
            var order_ = _val[2];
            if (itemcab == 1)
            {
                document.getElementById('checkriesgocaida_5_1_1_0').checked = false;
                document.getElementById('checkriesgocaida_5_1_2_1').checked = false;
            }
            if (itemcab == 2) {
                document.getElementById('checkriesgocaida_5_2_1_0').checked = false;
                document.getElementById('checkriesgocaida_5_2_2_1').checked = false;
                document.getElementById('checkriesgocaida_5_2_3_1').checked = false;
                document.getElementById('checkriesgocaida_5_2_4_1').checked = false;
                document.getElementById('checkriesgocaida_5_2_5_1').checked = false;
                document.getElementById('checkriesgocaida_5_2_6_1').checked = false;
                document.getElementById('checkriesgocaida_5_2_7_1').checked = false;
            }
            if (itemcab == 3)
            {
                document.getElementById('checkriesgocaida_5_3_1_0').checked = false;
                document.getElementById('checkriesgocaida_5_3_2_1').checked = false;
                document.getElementById('checkriesgocaida_5_3_3_1').checked = false;
                document.getElementById('checkriesgocaida_5_3_4_1').checked = false;
            }
            if (itemcab == 4) {
                document.getElementById('checkriesgocaida_5_4_1_0').checked = false;
                document.getElementById('checkriesgocaida_5_4_2_1').checked = false;
            }
            if (itemcab == 5) {
                document.getElementById('checkriesgocaida_5_5_1_0').checked = false;
                document.getElementById('checkriesgocaida_5_5_2_1').checked = false;
                document.getElementById('checkriesgocaida_5_5_3_1').checked = false;
                document.getElementById('checkriesgocaida_5_5_4_1').checked = false;
                document.getElementById('checkriesgocaida_5_5_5_1').checked = false;
            }
            if (itemcab == 6) {
                document.getElementById('checkriesgocaida_5_6_1_0').checked = false;
                document.getElementById('checkriesgocaida_5_6_2_1').checked = false;
                document.getElementById('checkriesgocaida_5_6_3_1').checked = false;

            }
            //if (itemcab == 7) {
            //    document.getElementById('checkriesgocaida_5_7_1_1').checked = false;
            //    document.getElementById('checkriesgocaida_5_7_2_1').checked = false;
            //
            //}
            document.getElementById('checkriesgocaida_' + _value.toString()).checked = check;
            var nombretext = "lblfilatotalRiesgoCaida_" + itemcab.toString();
            var nombretext2 = "lblfilatotalRiesgoCaida2_" + itemcab.toString();

            if (check == true) {
                if (itemcab == 7) {
                    if (document.getElementById('checkriesgocaida_5_7_1_1').checked == true && document.getElementById('checkriesgocaida_5_7_2_1').checked == true) {
                        document.getElementById(nombretext).value = "2";
                    }
                    else {
                        document.getElementById(nombretext).value = "1";
                        
                    }
                }
                else {
                    document.getElementById(nombretext).value = puntaje.toString();
                    document.getElementById(nombretext2).value = order_.toString();
                }                
            }
            else {
                if (itemcab == 7)
                {
                    if (document.getElementById('checkriesgocaida_5_7_1_1').checked == false && document.getElementById('checkriesgocaida_5_7_2_1').checked == false) {
                        document.getElementById(nombretext).value = '';
                    }
                    else if (document.getElementById('checkriesgocaida_5_7_1_1').checked == true && document.getElementById('checkriesgocaida_5_7_2_1').checked == false)
                    {
                        document.getElementById(nombretext).value = "1";
                    }
                    else if (document.getElementById('checkriesgocaida_5_7_1_1').checked == false && document.getElementById('checkriesgocaida_5_7_2_1').checked == true) {
                        document.getElementById(nombretext).value = "1";
                    }
                    else {
                        document.getElementById(nombretext).value = '';
                    }
                }
                else {
                    document.getElementById(nombretext).value = '';
                }   
               
            }

            var _puntaje1 = document.getElementById("lblfilatotalRiesgoCaida_1").value;
            var _puntaje2 = document.getElementById("lblfilatotalRiesgoCaida_2").value;
            var _puntaje3 = document.getElementById("lblfilatotalRiesgoCaida_3").value;
            var _puntaje4 = document.getElementById("lblfilatotalRiesgoCaida_4").value;
            var _puntaje5 = document.getElementById("lblfilatotalRiesgoCaida_5").value;
            var _puntaje6 = document.getElementById("lblfilatotalRiesgoCaida_6").value;
            var _puntaje7 = document.getElementById("lblfilatotalRiesgoCaida_7").value;           


            if (_puntaje1 == "") { _puntaje1 = "0"; }
            if (_puntaje2 == "") { _puntaje2 = "0"; }
            if (_puntaje3 == "") { _puntaje3 = "0"; }
            if (_puntaje4 == "") { _puntaje4 = "0"; }
            if (_puntaje5 == "") { _puntaje5 = "0"; }
            if (_puntaje6 == "") { _puntaje6 = "0"; }
            if (_puntaje7 == "") { _puntaje7 = "0"; }
            document.getElementById("divtotalriesgocaida").value = (parseInt(_puntaje1) + parseInt(_puntaje2) + parseInt(_puntaje3) + parseInt(_puntaje4) + parseInt(_puntaje5) + parseInt(_puntaje6) + parseInt(_puntaje7)).toString();

            /*para detalle actividaees*/

            var _1_puntaje1 = document.getElementById("lblfilatotalRiesgoCaida_1").value.toString();
            var _1_puntaje2 = document.getElementById("lblfilatotalRiesgoCaida_2").value.toString();
            var _1_puntaje3 = document.getElementById("lblfilatotalRiesgoCaida_3").value.toString();
            var _1_puntaje4 = document.getElementById("lblfilatotalRiesgoCaida_4").value.toString();
            var _1_puntaje5 = document.getElementById("lblfilatotalRiesgoCaida_5").value.toString();
            var _1_puntaje6 = document.getElementById("lblfilatotalRiesgoCaida_6").value.toString();
            var _1_puntaje7 = document.getElementById("lblfilatotalRiesgoCaida_7").value.toString();

            if (_1_puntaje1 != "" && _1_puntaje2 != "" && _1_puntaje3 != "" && _1_puntaje4 != "" && _1_puntaje5 != "" && _1_puntaje6 != "") {
                var _valor = document.getElementById("divtotalriesgocaida").value;
                Activar_busqueda_Actividades_riesgocaida("26", _valor.toString());
            }
            else {
                document.getElementById("tablaActividadesRiesgoCaida").style.visibility = "hidden";
                $("#bodyactividadRiesgoCaida").html('');
            }


        }

        function Activar_busqueda_Actividades_riesgocaida(codigo, valor)
        {
            var obj = JSON.stringify({ _Order: "4", _Codigo: codigo, _valor: valor });
            $.ajax({
                type: "POST",
                url: "InformacionPaciente.aspx/ActividaesEscalaEIndicaciones",
                data: obj,
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {
                    console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
                },
                success: function (data) {
                    DataActividadriesgocaida(data.d)
                }
            });
        }

        function DataActividadriesgocaida(data) {
            $("#bodyactividadRiesgoCaida").html('');
            $.each(data, function (index, value) {
                $("#bodyactividadRiesgoCaida").append("<tr><td>" + value.Item + "</td><td>" + value.Codigo + "</td><td>" + value.Actividad + "</td><td style='text-align: center;'><input type='checkbox' style='width: 25px; height: 25px;' name='checkactividadriesgocaida' /></td></tr>")
            });
            document.getElementById("tablaActividadesRiesgoCaida").style.visibility = "visible";
        }

        function Activar_desactivarCheckBradden(_value, check)
        {
            var _val = _value.split("_");
            var itemcab = _val[1];
            var puntaje = _val[2];
            //checkbraden_
            if(itemcab == 1)
            {
                document.getElementById('checkbraden_4_1_1').checked = false;
                document.getElementById('checkbraden_4_1_2').checked = false;
                document.getElementById('checkbraden_4_1_3').checked = false;
                document.getElementById('checkbraden_4_1_4').checked = false;
            }

            if(itemcab == 2)
            {
                document.getElementById('checkbraden_4_2_1').checked = false;
                document.getElementById('checkbraden_4_2_2').checked = false;
                document.getElementById('checkbraden_4_2_3').checked = false;
                document.getElementById('checkbraden_4_2_4').checked = false;
            }

            if(itemcab == 3)
            {
                document.getElementById('checkbraden_4_3_1').checked = false;
                document.getElementById('checkbraden_4_3_2').checked = false;
                document.getElementById('checkbraden_4_3_3').checked = false;
                document.getElementById('checkbraden_4_3_4').checked = false;
            }

            if(itemcab == 4)
            {
                document.getElementById('checkbraden_4_4_1').checked = false;
                document.getElementById('checkbraden_4_4_2').checked = false;
                document.getElementById('checkbraden_4_4_3').checked = false;
                document.getElementById('checkbraden_4_4_4').checked = false;
            }                

            if(itemcab == 5)
            {
                document.getElementById('checkbraden_4_5_1').checked = false;
                document.getElementById('checkbraden_4_5_2').checked = false;
                document.getElementById('checkbraden_4_5_3').checked = false;
                document.getElementById('checkbraden_4_5_4').checked = false;
            }
            
            if(itemcab == 6)
            {
                document.getElementById('checkbraden_4_6_1').checked = false;
                document.getElementById('checkbraden_4_6_2').checked = false;
                document.getElementById('checkbraden_4_6_3').checked = false;
            }
            document.getElementById('checkbraden_' + _value.toString()).checked = check;
            var nombretext = "lblfilatotalbradden_" + itemcab.toString();
           
            if (check == true)
            {                
                 document.getElementById(nombretext).value = puntaje.toString();
             }
            else {
               document.getElementById(nombretext).value = '0';
            }
            var _puntaje1= document.getElementById("lblfilatotalbradden_1").value;
            var _puntaje2= document.getElementById("lblfilatotalbradden_2").value;
            var _puntaje3= document.getElementById("lblfilatotalbradden_3").value;
            var _puntaje4= document.getElementById("lblfilatotalbradden_4").value;
            var _puntaje5= document.getElementById("lblfilatotalbradden_5").value;
            var _puntaje6= document.getElementById("lblfilatotalbradden_6").value;
            
             document.getElementById("divtotalbradden").value = (parseInt(_puntaje1) + parseInt(_puntaje2) + parseInt(_puntaje3) + parseInt(_puntaje4)+ parseInt(_puntaje5)+parseInt(_puntaje6)).toString();
 
            $("#traltoriesgo").css("background-color", "#ffffff");
            $("#trriesgomoderado").css("background-color", "#ffffff");
            $("#trriesgobajo").css("background-color", "#ffffff");

          

            if (parseInt(_puntaje1) > 0 && parseInt(_puntaje2) > 0 && parseInt(_puntaje3) > 0 && parseInt(_puntaje4) > 0 && parseInt(_puntaje5) > 0 && parseInt(_puntaje6) > 0) {
                var _valor = document.getElementById("divtotalbradden").value;
                Activar_busqueda_Actividades_bradden("19", _valor.toString());

                if (parseInt(_valor.toString()) <= 12) {
                     $("#traltoriesgo").css("background-color", "#d5b2b2");
                }
                else if (parseInt(_valor.toString()) > 12 && parseInt(_valor.toString()) <= 14) {
                    $("#trriesgomoderado").css("background-color", "#d5b2b2");
                }
                else if (parseInt(_valor.toString()) > 14 && parseInt(_valor.toString()) <= 30)
                {
                    $("#trriesgobajo").css("background-color", "#d5b2b2");
                }
            }
            else {
                document.getElementById("tablaActividadesBradden").style.visibility = "hidden"; 
                $("#bodyactividadBradden").html('');
            }

        }

        function Activar_busqueda_Actividades_bradden(codigo, valor) {
            var obj = JSON.stringify({ _Order: "3", _Codigo: codigo, _valor: valor });
            $.ajax({
                type: "POST",
                url: "InformacionPaciente.aspx/ActividaesEscalaEIndicaciones",
                data: obj,
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {
                    console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
                },
                success: function (data) {
                    DataActividadbradden(data.d)
                }
            });
        }

        function DataActividadbradden(data) {
            $("#bodyactividadBradden").html('');
            $.each(data, function (index, value) {
                $("#bodyactividadBradden").append("<tr><td>" + value.Item + "</td><td>" + value.Codigo + "</td><td>" + value.Actividad + "</td><td style='text-align: center;'><input type='checkbox' style='width: 25px; height: 25px;' name='checkactividadbradden' /></td></tr>")
            });
            document.getElementById("tablaActividadesBradden").style.visibility = "visible";
        }

        function Activar_desactivarCheckMaddox(_value, check){
            var _val = _value.split("_");
            var itemcab = _val[1];
            var puntaje = _val[2];
            document.getElementById('checkMaddox_3_1_0').checked = false;
            document.getElementById('checkMaddox_3_2_1').checked = false;
            document.getElementById('checkMaddox_3_3_2').checked = false;
            document.getElementById('checkMaddox_3_4_3').checked = false;
            document.getElementById('checkMaddox_3_5_4').checked = false;
            document.getElementById('checkMaddox_3_6_5').checked = false;
             

            document.getElementById('checkMaddox_' + _value.toString()).checked = check;
            //var nombretext = "lblfilatotal_" + itemcab.toString();
           
            //document.getElementById('checDolor_2_6_10').checked = false;

            //document.getElementById('checDolor_' + _value.toString()).checked = check;
            //var nombretext = "lblfilatotal_" + itemcab.toString();
            if (check == true){
                document.getElementById("divtotalMaddox").value = puntaje.toString();
                //
                 
                var valor_Busqueda =  puntaje.toString() ;
                Activar_busqueda_Actividades_Maddox("12",valor_Busqueda );
            }
            else {
                document.getElementById("divtotalMaddox").value = "";
                document.getElementById("tablaActividadesMaddox").style.visibility = "hidden"; 
                $("#tablaactividadMaddox").html('');
            }  
             
         
        }

        function Activar_busqueda_Actividades_Maddox(codigo, valor) {
            var obj = JSON.stringify({ _Order:"2" ,_Codigo: codigo, _valor: valor });
            $.ajax({
                type: "POST",
                url: "InformacionPaciente.aspx/ActividaesEscalaEIndicaciones",
                data: obj,
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {
                    console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
                },
                success: function (data) {                    
                    DataActividadMaddox(data.d)
                }
            });
        }

        function DataActividadMaddox(data)
        {
            //var tabla = $("#tablaactividadMaddox")
            $("#tablaactividadMaddox").html('');
            $.each(data, function (index, value) {
                $("#tablaactividadMaddox").append("<tr><td>" + value.Item + "</td><td>" + value.Codigo + "</td><td>" + value.Actividad + "</td><td style='text-align: center;'><input type='checkbox' style='width: 25px; height: 25px;' name='checkactividadMaddox' /></td></tr>")
            });
            document.getElementById("tablaActividadesMaddox").style.visibility = "visible";
        }

        function Activar_desactivarCheckDolor(_value, check)
        {
            var _val = _value.split("_");
            var itemcab = _val[1];
            var puntaje = _val[2];
            document.getElementById('checDolor_2_1_0').checked = false;
            document.getElementById('checDolor_2_2_1').checked = false;
            document.getElementById('checDolor_2_2_2').checked = false;
            document.getElementById('checDolor_2_3_3').checked = false;
            document.getElementById('checDolor_2_3_4').checked = false;
            document.getElementById('checDolor_2_4_5').checked = false;
            document.getElementById('checDolor_2_4_6').checked = false;
            document.getElementById('checDolor_2_5_7').checked = false;
            document.getElementById('checDolor_2_5_8').checked = false;
            document.getElementById('checDolor_2_6_9').checked = false;
            document.getElementById('checDolor_2_6_10').checked = false;

            document.getElementById('checDolor_'+_value.toString()).checked = check;
             if (check == true){
                document.getElementById("divtotalDolor").value =puntaje.toString();
            }
            else{ document.getElementById("divtotalDolor").value = "";  }  
        }

        function Activar_desactivarCheckGlascow(_value, check)
        {
            var _val = _value.split("_");
            var itemcab = _val[1];
            var puntaje = _val[2];
            if (itemcab == 1)
            {
                document.getElementById('checkGlascow_1_1_1').checked = false;
                document.getElementById('checkGlascow_1_1_2').checked = false;
                document.getElementById('checkGlascow_1_1_3').checked = false;
                document.getElementById('checkGlascow_1_1_4').checked = false;
            }
            if (itemcab == 2)
            {
                document.getElementById('checkGlascow_1_2_1').checked = false;
                document.getElementById('checkGlascow_1_2_2').checked = false;
                document.getElementById('checkGlascow_1_2_3').checked = false;
                document.getElementById('checkGlascow_1_2_4').checked = false;
                document.getElementById('checkGlascow_1_2_5').checked = false;
                document.getElementById('checkGlascow_1_2_6').checked = false;
            }
            if (itemcab == 3)
            {
                document.getElementById('checkGlascow_1_3_1').checked = false;
                document.getElementById('checkGlascow_1_3_2').checked = false;
                document.getElementById('checkGlascow_1_3_3').checked = false;
                document.getElementById('checkGlascow_1_3_4').checked = false;
                document.getElementById('checkGlascow_1_3_5').checked = false;
            }            

            document.getElementById('checkGlascow_' + _value.toString()).checked = check;
            var nombretext = "lblfilatotal_" + itemcab.toString();
           
            if (check == true)
            {                
                 document.getElementById(nombretext).value = puntaje.toString();
             }
            else {
               document.getElementById(nombretext).value = '0';
            }
            
            var _puntaje1= document.getElementById("lblfilatotal_1").value;
            var _puntaje2= document.getElementById("lblfilatotal_2").value;
            var _puntaje3= document.getElementById("lblfilatotal_3").value;
            //divtotalglascow 
            document.getElementById("divtotalglascow").value = (parseInt(_puntaje1) + parseInt(_puntaje2) + parseInt(_puntaje3)).toString();
        }


        /*Evento de Guardardo*/
        function btnGuardarGlascowClick()
        {
            var _puntaje1 = document.getElementById("lblfilatotal_1").value;
            var _puntaje2 = document.getElementById("lblfilatotal_2").value;
            var _puntaje3 = document.getElementById("lblfilatotal_3").value;

            if (parseInt(_puntaje1.toString()) == 0 || parseInt(_puntaje2.toString()) == 0 || parseInt(_puntaje3.toString()) == 0) {
                $.JMensajePOPUP("Aviso", "campo Obligatorio; seleccionar un valor de cada fila. \n\n El total por fila debe ser mayor a cero", "", "Cerrar", "fn_oculta_mensaje()", "");
            }
            else {
                //$.JPopUp("Cambiar Contraseña", "PopUp/CambiarPassword.aspx", "2", "Aceptar;Salir", "fn_GuardarRegistroGlacow();fn_oculta_popup()", 40, "");
                $.JMensajePOPUP("Confirmación", "¿Está seguro de guardar el registro?", "2", "Guardar;Cerrar", "fn_GuardarRegistroGlascow();fn_oculta_mensaje()", "");
            }
        }

        function fn_GuardarRegistroGlascow() {
            var _puntaje1 = parseInt(document.getElementById("lblfilatotal_1").value.toString());
            var _puntaje2 = parseInt(document.getElementById("lblfilatotal_2").value.toString());
            var _puntaje3 = parseInt(document.getElementById("lblfilatotal_3").value.toString());
            var _total = parseInt(document.getElementById("divtotalglascow").value.toString());

            var obj = JSON.stringify({
                Tipo: 1,
                puntaje1: _puntaje1,
                puntaje2: _puntaje2,
                puntaje3: _puntaje3,
                total: _total 
            });

            $.ajax({
                type: "POST",
                url: "InformacionPaciente.aspx/RegistrowebGlascow",
                data: obj,
                dataType: "json",
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {
                    $.JMensajePOPUP("Aviso", xhr.status + " \n" + xhr.responseText, "\n" + thrownError, "", "Cerrar", "fn_oculta_mensaje()", "");    
                     
                },
                success: function (response) {
                    $.JMensajePOPUP("Aviso", response.d, "", "Cerrar", "fn_oculta_mensaje()", "");
                    limpiartablaGlascow();
                }
            });
        }

        function limpiartablaGlascow() {
            document.getElementById('checkGlascow_1_1_1').checked = false;
            document.getElementById('checkGlascow_1_1_2').checked = false;
            document.getElementById('checkGlascow_1_1_3').checked = false;
            document.getElementById('checkGlascow_1_1_4').checked = false;
            document.getElementById('checkGlascow_1_2_1').checked = false;
            document.getElementById('checkGlascow_1_2_2').checked = false;
            document.getElementById('checkGlascow_1_2_3').checked = false;
            document.getElementById('checkGlascow_1_2_4').checked = false;
            document.getElementById('checkGlascow_1_2_5').checked = false;
            document.getElementById('checkGlascow_1_2_6').checked = false;
            document.getElementById('checkGlascow_1_3_1').checked = false;
            document.getElementById('checkGlascow_1_3_2').checked = false;
            document.getElementById('checkGlascow_1_3_3').checked = false;
            document.getElementById('checkGlascow_1_3_4').checked = false;
            document.getElementById('checkGlascow_1_3_5').checked = false;

            document.getElementById("lblfilatotal_1").value = "0";
            document.getElementById("lblfilatotal_2").value = "0";
            document.getElementById("lblfilatotal_3").value = "0";
            document.getElementById("divtotalglascow").value  = "0"
        }

        function btnGuardarEscalaDolor()
        {
            var _dolor = document.getElementById("divtotalDolor").value.toString();
            if (_dolor == "") {
                $.JMensajePOPUP("Aviso", "campo Obligatorio;  Seleccionar un elemento de la lista", "", "Cerrar", "fn_oculta_mensaje()", "");
            }
            else {
                $.JMensajePOPUP("Confirmación", "¿Está seguro de guardar el registro?", "2", "Guardar;Cerrar", "fn_GuardarRegistroDolor();fn_oculta_mensaje()", "");
            }
        }

        function fn_GuardarRegistroDolor() {
            var _puntaje = parseInt(document.getElementById("divtotalDolor").value.toString());
            var obj = JSON.stringify({
                Tipo: 2,
                puntaje1: _puntaje,
                total:_puntaje
            });

            $.ajax({
                type: "POST",
                url: "InformacionPaciente.aspx/RegistrowebDolor",
                data: obj,
                dataType: "json",
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {
                    $.JMensajePOPUP("Aviso", xhr.status + " \n" + xhr.responseText, "\n" + thrownError, "", "Cerrar", "fn_oculta_mensaje()", "");

                },
                success: function (response) {
                    $.JMensajePOPUP("Aviso", response.d, "", "Cerrar", "fn_oculta_mensaje()", "");
                    limpiartablaDolor();
                }
            });
        }

        function limpiartablaDolor() {
            document.getElementById('checDolor_2_1_0').checked = false;
            document.getElementById('checDolor_2_2_1').checked = false;
            document.getElementById('checDolor_2_2_2').checked = false;
            document.getElementById('checDolor_2_3_3').checked = false;
            document.getElementById('checDolor_2_3_4').checked = false;
            document.getElementById('checDolor_2_4_5').checked = false;
            document.getElementById('checDolor_2_4_6').checked = false;
            document.getElementById('checDolor_2_5_7').checked = false;
            document.getElementById('checDolor_2_5_8').checked = false;
            document.getElementById('checDolor_2_6_9').checked = false;
            document.getElementById('checDolor_2_6_10').checked = false;

            document.getElementById("divtotalDolor").value = "";
        }

        ///*function btnGuardarMaddoxClick()
        //{
        //    var _maddox = document.getElementById("divtotalMaddox").value.toString();
        //    if (_maddox == "") {
        //        $.JMensajePOPUP("Aviso", "campo Obligatorio;  Seleccionar un elemento de la lista", "", "Cerrar", "fn_oculta_mensaje()", "");
        //    }
        //    else {
        //        //checkactividadMaddox
        //        var checkboxs = $("input[name='checkactividadMaddox']");
        //        var todos = checkboxs.length === checkboxs.filter(":checked").length;
        //        if (todos == true) {
        //            //$.JMensajePOPUP("Aviso", "campos Obligatorio;  hecho", "", "Cerrar", "fn_oculta_mensaje()", "");
        //            $.JMensajePOPUP("Confirmación", "¿Está seguro de guardar el registro?", "2", "Guardar;Cerrar", "fn_GuardarRegistroMaddox();fn_oculta_mensaje()", "");
        //        }
        //        else
        //        {
        //            $.JMensajePOPUP("Aviso", "campos Obligatorio;  Debes seleccionar todas las actividades de la escala Maddox", "", "Cerrar", "fn_oculta_mensaje()", "");
        //        }
        //    }
        //}*/

        //INICIO 1.1 FGONZALES 19/02/2024|
        function btnGuardarMaddoxClick() {
            var _maddox = document.getElementById("divtotalMaddox").value.toString();
            if (_maddox == "") {
                $.JMensajePOPUP("Aviso", "Campo Obligatorio; Seleccionar un elemento de la lista", "", "Cerrar", "fn_oculta_mensaje()", "");
            } else {
                // checkactividadMaddox
                var checkboxs = $("input[name='checkactividadMaddox']");
                var alMenosUnoSeleccionado = checkboxs.is(":checked");

                if (alMenosUnoSeleccionado) {
                    $.JMensajePOPUP("Confirmación", "¿Está seguro de guardar el registro?", "2", "Guardar;Cerrar", "fn_GuardarRegistroMaddox();fn_oculta_mensaje()", "");
                } else {
                    $.JMensajePOPUP("Aviso", "Campos Obligatorios; Debes seleccionar al menos una actividad de la escala Maddox", "", "Cerrar", "fn_oculta_mensaje()", "");
                }
            }
        }
        //FIN 1.1 FGONZALES 19/02/2024

        function fn_GuardarRegistroMaddox() {
            var _puntaje = parseInt(document.getElementById("divtotalMaddox").value.toString());
            var obj = JSON.stringify({
                Tipo: 3,
                puntaje1: _puntaje,
                total: _puntaje
            });

            $.ajax({
                type: "POST",
                url: "InformacionPaciente.aspx/RegistrowebDolor",
                data: obj,
                dataType: "json",
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {
                    $.JMensajePOPUP("Aviso", xhr.status + " \n" + xhr.responseText, "\n" + thrownError, "", "Cerrar", "fn_oculta_mensaje()", "");

                },
                success: function (response) {
                    $.JMensajePOPUP("Aviso", response.d, "", "Cerrar", "fn_oculta_mensaje()", "");
                    limpiartablaMaddox();
                }
            });
        }

        function limpiartablaMaddox() {
            document.getElementById('checkMaddox_3_1_0').checked = false;
            document.getElementById('checkMaddox_3_2_1').checked = false;
            document.getElementById('checkMaddox_3_3_2').checked = false;
            document.getElementById('checkMaddox_3_4_3').checked = false;
            document.getElementById('checkMaddox_3_5_4').checked = false;
            document.getElementById('checkMaddox_3_6_5').checked = false;
            document.getElementById("divtotalMaddox").value = "";
            document.getElementById("tablaActividadesMaddox").style.visibility = "hidden";
            $("#tablaactividadMaddox").html('');
        }

        ///*function btnGuardarEscalaBradden2()
        //{
        //    var _puntaje1 = document.getElementById("lblfilatotalbradden_1").value;
        //    var _puntaje2 = document.getElementById("lblfilatotalbradden_2").value;
        //    var _puntaje3 = document.getElementById("lblfilatotalbradden_3").value;
        //    var _puntaje4 = document.getElementById("lblfilatotalbradden_4").value;
        //    var _puntaje5 = document.getElementById("lblfilatotalbradden_5").value;
        //    var _puntaje6 = document.getElementById("lblfilatotalbradden_6").value;
        //    if (parseInt(_puntaje1) > 0 && parseInt(_puntaje2) > 0 && parseInt(_puntaje3) > 0 && parseInt(_puntaje4) > 0 && parseInt(_puntaje5) > 0 && parseInt(_puntaje6) > 0) {

        //        var checkboxs = $("input[name='checkactividadbradden']");
        //        var todos = checkboxs.length === checkboxs.filter(":checked").length;
        //        if (todos == true) {
        //            $.JMensajePOPUP("Confirmación", "¿Está seguro de guardar el registro?", "2", "Guardar;Cerrar", "fn_GuardarRegistrobradden();fn_oculta_mensaje()", "");
        //        } else {

        //            $.JMensajePOPUP("Aviso", "campos Obligatorio;  Debes seleccionar todas las actividades de la escala bradden", "", "Cerrar", "fn_oculta_mensaje()", "");
        //        }
                
        //    }
        //    else {
        //        $.JMensajePOPUP("Aviso", "campo Obligatorio; seleccionar un valor de cada fila. \n\n El total por fila debe ser mayor a cero", "", "Cerrar", "fn_oculta_mensaje()", "");
        //    }
        //}*/
        //inicio 1.1 fgonzales
        function btnGuardarEscalaBradden2() {
            var _puntaje1 = document.getElementById("lblfilatotalbradden_1").value;
            var _puntaje2 = document.getElementById("lblfilatotalbradden_2").value;
            var _puntaje3 = document.getElementById("lblfilatotalbradden_3").value;
            var _puntaje4 = document.getElementById("lblfilatotalbradden_4").value;
            var _puntaje5 = document.getElementById("lblfilatotalbradden_5").value;
            var _puntaje6 = document.getElementById("lblfilatotalbradden_6").value;

            var alMenosUnPuntajeMayorQueCero = parseInt(_puntaje1) > 0 || parseInt(_puntaje2) > 0 || parseInt(_puntaje3) > 0 || parseInt(_puntaje4) > 0 || parseInt(_puntaje5) > 0 || parseInt(_puntaje6) > 0;

            if (alMenosUnPuntajeMayorQueCero) {
                $.JMensajePOPUP("Confirmación", "¿Está seguro de guardar el registro?", "2", "Guardar;Cerrar", "fn_GuardarRegistrobradden();fn_oculta_mensaje()", "");
            } else {
                $.JMensajePOPUP("Aviso", "Campo Obligatorio; seleccionar un valor de al menos una fila. \n\n El total por fila debe ser mayor a cero", "", "Cerrar", "fn_oculta_mensaje()", "");
            }
        }
        //fin 1.1 fgonzales

        function fn_GuardarRegistrobradden()
        {
            var _puntaje1 = parseInt(document.getElementById("lblfilatotalbradden_1").value.toString());
            var _puntaje2 = parseInt(document.getElementById("lblfilatotalbradden_2").value.toString());
            var _puntaje3 = parseInt(document.getElementById("lblfilatotalbradden_3").value.toString());
            var _puntaje4 = parseInt(document.getElementById("lblfilatotalbradden_4").value.toString());
            var _puntaje5 = parseInt(document.getElementById("lblfilatotalbradden_5").value.toString());
            var _puntaje6 = parseInt(document.getElementById("lblfilatotalbradden_6").value.toString());
           
            var _total = parseInt(document.getElementById("divtotalbradden").value.toString());

            var obj = JSON.stringify({
                Tipo: 4,
                puntaje1: _puntaje1,
                puntaje2: _puntaje2,
                puntaje3: _puntaje3,
                puntaje4: _puntaje4,
                puntaje5: _puntaje5,
                puntaje6: _puntaje6,
                total: _total
            });

            $.ajax({
                type: "POST",
                url: "InformacionPaciente.aspx/RegistrowebBradden",
                data: obj,
                dataType: "json",
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {
                    $.JMensajePOPUP("Aviso", xhr.status + " \n" + xhr.responseText, "\n" + thrownError, "", "Cerrar", "fn_oculta_mensaje()", "");

                },
                success: function (response) {
                    $.JMensajePOPUP("Aviso", response.d, "", "Cerrar", "fn_oculta_mensaje()", "");
                    limpiartablabradden();
                }
            });
        }

        function limpiartablabradden() {
            document.getElementById('checkbraden_4_1_1').checked = false;
            document.getElementById('checkbraden_4_1_2').checked = false;
            document.getElementById('checkbraden_4_1_3').checked = false;
            document.getElementById('checkbraden_4_1_4').checked = false;
            document.getElementById('checkbraden_4_2_1').checked = false;
            document.getElementById('checkbraden_4_2_2').checked = false;
            document.getElementById('checkbraden_4_2_3').checked = false;
            document.getElementById('checkbraden_4_2_4').checked = false;
            document.getElementById('checkbraden_4_3_1').checked = false;
            document.getElementById('checkbraden_4_3_2').checked = false;
            document.getElementById('checkbraden_4_3_3').checked = false;
            document.getElementById('checkbraden_4_3_4').checked = false;
            document.getElementById('checkbraden_4_4_1').checked = false;
            document.getElementById('checkbraden_4_4_2').checked = false;
            document.getElementById('checkbraden_4_4_3').checked = false;
            document.getElementById('checkbraden_4_4_4').checked = false;

            document.getElementById('checkbraden_4_5_1').checked = false;
            document.getElementById('checkbraden_4_5_2').checked = false;
            document.getElementById('checkbraden_4_5_3').checked = false;
            document.getElementById('checkbraden_4_5_4').checked = false;

            document.getElementById('checkbraden_4_6_1').checked = false;
            document.getElementById('checkbraden_4_6_2').checked = false;
            document.getElementById('checkbraden_4_6_3').checked = false;


            document.getElementById("lblfilatotalbradden_1").value="0";
            document.getElementById("lblfilatotalbradden_2").value="0";
            document.getElementById("lblfilatotalbradden_3").value="0";
            document.getElementById("lblfilatotalbradden_4").value="0";
            document.getElementById("lblfilatotalbradden_5").value="0";
            document.getElementById("lblfilatotalbradden_6").value="0";
            document.getElementById("divtotalbradden").value = "0";

            $("#traltoriesgo").css("background-color", "#ffffff");
            $("#trriesgomoderado").css("background-color", "#ffffff");
            $("#trriesgobajo").css("background-color", "#ffffff");

            document.getElementById("tablaActividadesBradden").style.visibility = "hidden";
            $("#bodyactividadBradden").html('');
        }

        ///*function btnGuardarEscalariesgocaida()
        //{
        //    var _1_puntaje1 = document.getElementById("lblfilatotalRiesgoCaida_1").value.toString();
        //    var _1_puntaje2 = document.getElementById("lblfilatotalRiesgoCaida_2").value.toString();
        //    var _1_puntaje3 = document.getElementById("lblfilatotalRiesgoCaida_3").value.toString();
        //    var _1_puntaje4 = document.getElementById("lblfilatotalRiesgoCaida_4").value.toString();
        //    var _1_puntaje5 = document.getElementById("lblfilatotalRiesgoCaida_5").value.toString();
        //    var _1_puntaje6 = document.getElementById("lblfilatotalRiesgoCaida_6").value.toString();
        //    var _1_puntaje7 = document.getElementById("lblfilatotalRiesgoCaida_7").value.toString();

        //    if (_1_puntaje1 != "" && _1_puntaje2 != "" && _1_puntaje3 != "" && _1_puntaje4 != "" && _1_puntaje5 != "" && _1_puntaje6 != "") {
        //        var checkboxs = $("input[name='checkactividadriesgocaida']");
        //        var todos = checkboxs.length === checkboxs.filter(":checked").length;
        //        if (todos == true) {
        //            $.JMensajePOPUP("Confirmación", "¿Está seguro de guardar el registro?", "2", "Guardar;Cerrar", "fn_GuardarRegistroRiesgocaida();fn_oculta_mensaje()", "");
        //        } else {

        //            $.JMensajePOPUP("Aviso", "campos Obligatorio;  Debes seleccionar todas las actividades de la escala bradden", "", "Cerrar", "fn_oculta_mensaje()", "");
        //        }
        //    }
        //    else {
        //        $.JMensajePOPUP("Aviso", "campo Obligatorio; seleccionar un valor de cada fila. \n\n el total por fila debe ser diferente a vacío('')", "", "Cerrar", "fn_oculta_mensaje()", "");
        //    }
        //}*/
        //inicio 1.1 fgonzales
        function btnGuardarEscalariesgocaida() {
            var _1_puntaje1 = document.getElementById("lblfilatotalRiesgoCaida_1").value.toString();
            var _1_puntaje2 = document.getElementById("lblfilatotalRiesgoCaida_2").value.toString();
            var _1_puntaje3 = document.getElementById("lblfilatotalRiesgoCaida_3").value.toString();
            var _1_puntaje4 = document.getElementById("lblfilatotalRiesgoCaida_4").value.toString();
            var _1_puntaje5 = document.getElementById("lblfilatotalRiesgoCaida_5").value.toString();
            var _1_puntaje6 = document.getElementById("lblfilatotalRiesgoCaida_6").value.toString();
            var _1_puntaje7 = document.getElementById("lblfilatotalRiesgoCaida_7").value.toString();

            //if (_1_puntaje1 != "" && _1_puntaje2 != "" && _1_puntaje3 != "" && _1_puntaje4 != "" && _1_puntaje5 != "" && _1_puntaje6 != "") {
            if (_1_puntaje1 !== "" || _1_puntaje2 !== "" || _1_puntaje3 !== "" || _1_puntaje4 !== "" || _1_puntaje5 !== "" || _1_puntaje6 !== "" || _1_puntaje7 !== "") {
                var checkboxs = $("input[name='checkactividadriesgocaida']");
                var checkedCount = checkboxs.filter(":checked").length;

                if (checkedCount >= 0 && checkedCount <= 6) {
                    $.JMensajePOPUP("Confirmación", "¿Está seguro de guardar el registro?", "2", "Guardar;Cerrar", "fn_GuardarRegistroRiesgocaida();fn_oculta_mensaje()", "");
                } else {
                    $.JMensajePOPUP("Aviso", "Debe seleccionar de 0 a 2 actividades de la escala Bradden", "", "Cerrar", "fn_oculta_mensaje()", "");
                }
            } else {
                $.JMensajePOPUP("Aviso", "Debes seleccionar un valor de cada fila. El total por fila no debe ser vacío ('')", "", "Cerrar", "fn_oculta_mensaje()", "");
            }
        }
        //fin 1.1 fgonzales


        function fn_GuardarRegistroRiesgocaida()
        {
            ///*var _puntaje1 = parseInt(document.getElementById("lblfilatotalRiesgoCaida2_1").value.toString());
            //var _puntaje2 = parseInt(document.getElementById("lblfilatotalRiesgoCaida2_2").value.toString());
            //var _puntaje3 = parseInt(document.getElementById("lblfilatotalRiesgoCaida2_3").value.toString());
            //var _puntaje4 = parseInt(document.getElementById("lblfilatotalRiesgoCaida2_4").value.toString());
            //var _puntaje5 = parseInt(document.getElementById("lblfilatotalRiesgoCaida2_5").value.toString());
            //var _puntaje6 = parseInt(document.getElementById("lblfilatotalRiesgoCaida2_6").value.toString());*/
            //incio 1.1 fgonzales
            var _puntaje1 = parseInt(document.getElementById("lblfilatotalRiesgoCaida2_1").value);
            var _puntaje2 = parseInt(document.getElementById("lblfilatotalRiesgoCaida2_2").value);
            var _puntaje3 = parseInt(document.getElementById("lblfilatotalRiesgoCaida2_3").value);
            var _puntaje4 = parseInt(document.getElementById("lblfilatotalRiesgoCaida2_4").value);
            var _puntaje5 = parseInt(document.getElementById("lblfilatotalRiesgoCaida2_5").value);
            var _puntaje6 = parseInt(document.getElementById("lblfilatotalRiesgoCaida2_6").value);
            //fin 1.1 fgonzales

            var _puntaje7 = 0;
            var _valor = "";
            //inicio 1.1 fgonzales
            // Verificar si los campos opcionales están vacíos y asignarles un valor predeterminado
            if (isNaN(_puntaje1)) _puntaje1 = 0;
            if (isNaN(_puntaje2)) _puntaje2 = 0;
            if (isNaN(_puntaje3)) _puntaje3 = 0;
            if (isNaN(_puntaje4)) _puntaje4 = 0;
            if (isNaN(_puntaje5)) _puntaje5 = 0;
            if (isNaN(_puntaje6)) _puntaje6 = 0;
            //fin 1.1 fgonzales
            if (document.getElementById('checkriesgocaida_5_7_1_1').checked == true && document.getElementById('checkriesgocaida_5_7_2_1').checked == true) {
                _puntaje7 = 2;
                _valor = "0";
            }
            else if (document.getElementById('checkriesgocaida_5_7_1_1').checked == false && document.getElementById('checkriesgocaida_5_7_2_1').checked == true) {
                _puntaje7 = 1;
                _valor = "2";
            }
            else if (document.getElementById('checkriesgocaida_5_7_1_1').checked == true && document.getElementById('checkriesgocaida_5_7_2_1').checked == false)
            {
                _puntaje7 = 1;
                _valor = "1";
            }


            var _total = parseInt(document.getElementById("divtotalriesgocaida").value.toString());



            var obj = JSON.stringify({
                Tipo: 5,
                puntaje1: _puntaje1,
                puntaje2: _puntaje2,
                puntaje3: _puntaje3,
                puntaje4: _puntaje4,
                puntaje5: _puntaje5,
                puntaje6: _puntaje6,
                puntaje7: _puntaje7,
                valor : _valor,
                total: _total
            });

            $.ajax({
                type: "POST",
                url: "InformacionPaciente.aspx/RegistrowebRiesgocaida",
                data: obj,
                dataType: "json",
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {
                    $.JMensajePOPUP("Aviso", xhr.status + " \n" + xhr.responseText, "\n" + thrownError, "", "Cerrar", "fn_oculta_mensaje()", "");

                },
                success: function (response) {
                    $.JMensajePOPUP("Aviso", response.d, "", "Cerrar", "fn_oculta_mensaje()", "");
                    limpiartablariesgocaida();
                }
            });

        }

        function limpiartablariesgocaida() {

            document.getElementById('checkriesgocaida_5_1_1_0').checked = false;
            document.getElementById('checkriesgocaida_5_1_2_1').checked = false;

            document.getElementById('checkriesgocaida_5_2_1_0').checked = false;
            document.getElementById('checkriesgocaida_5_2_2_1').checked = false;
            document.getElementById('checkriesgocaida_5_2_3_1').checked = false;
            document.getElementById('checkriesgocaida_5_2_4_1').checked = false;
            document.getElementById('checkriesgocaida_5_2_5_1').checked = false;
            document.getElementById('checkriesgocaida_5_2_6_1').checked = false;
            document.getElementById('checkriesgocaida_5_2_7_1').checked = false;

            document.getElementById('checkriesgocaida_5_3_1_0').checked = false;
            document.getElementById('checkriesgocaida_5_3_2_1').checked = false;
            document.getElementById('checkriesgocaida_5_3_3_1').checked = false;
            document.getElementById('checkriesgocaida_5_3_4_1').checked = false;

            document.getElementById('checkriesgocaida_5_4_1_0').checked = false;
            document.getElementById('checkriesgocaida_5_4_2_1').checked = false;

            document.getElementById('checkriesgocaida_5_5_1_0').checked = false;
            document.getElementById('checkriesgocaida_5_5_2_1').checked = false;
            document.getElementById('checkriesgocaida_5_5_3_1').checked = false;
            document.getElementById('checkriesgocaida_5_5_4_1').checked = false;
            document.getElementById('checkriesgocaida_5_5_5_1').checked = false;

            document.getElementById('checkriesgocaida_5_6_1_0').checked = false;
            document.getElementById('checkriesgocaida_5_6_2_1').checked = false;
            document.getElementById('checkriesgocaida_5_6_3_1').checked = false;


            document.getElementById('checkriesgocaida_5_7_1_1').checked = false;
            document.getElementById('checkriesgocaida_5_7_2_1').checked = false;

            document.getElementById("lblfilatotalRiesgoCaida_1").value = "";
            document.getElementById("lblfilatotalRiesgoCaida_2").value = "";
            document.getElementById("lblfilatotalRiesgoCaida_3").value = "";
            document.getElementById("lblfilatotalRiesgoCaida_4").value = "";
            document.getElementById("lblfilatotalRiesgoCaida_5").value = "";
            document.getElementById("lblfilatotalRiesgoCaida_6").value = "";
            document.getElementById("lblfilatotalRiesgoCaida_7").value = "";
            document.getElementById("divtotalriesgocaida").value = "";
            document.getElementById("tablaActividadesRiesgoCaida").style.visibility = "hidden";
            $("#bodyactividadRiesgoCaida").html('');
        }

        /*Listado Historico*/

        function Listar_datos_historicosEscalaeIntervenciones()
        {

            // <div class="JCELDA-2"><input type="date" style=" width: 160px; height: 40px; font-size: 18px;"   id="fechaInicioEscalaEIntervencionesenfermeria"/></div>
            //  <div class="JCELDA-2"><input type="date"  style=" width: 160px; height: 40px; font-size: 18px;" id="fechaFINEscalaEIntervencionesenfermeria"/></div>
            var _valor1 = document.getElementById("fechaInicioEscalaEIntervencionesenfermeria").value.toString();
            var _valor2 = document.getElementById("fechaFINEscalaEIntervencionesenfermeria").value.toString();
             
            $("#tablaDatosHistoricosEscalaeindicacionesEnfermeria").html('');

            var obj = JSON.stringify({ Valor1: _valor1, Valor2: _valor2 });
            $.ajax({
                type: "POST",
                url: "InformacionPaciente.aspx/Listado_datos_historicos_escala",
                data: obj,
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {
                    console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
                },
                success: function (data) {
                    //DataActividadriesgocaida(data.d)
                    //Log.d(data.d);
                    //console.log(data.d);
                    $("#tablaDatosHistoricosEscalaeindicacionesEnfermeria").append(data.d);
                }
            });

        }
        //Listar_datos_historicosEscalaeIntervenciones();       

        /*Ver resumen*/

        function EscalaEIntervencionVerResumen(valor, ID, total)
        {
            var _IDE = valor.toString();
            var obj = JSON.stringify({ _variable: _IDE, IDEscala : ID, Total :total});
            $.ajax({
                type: "POST",
                url: "InformacionPaciente.aspx/Listado_datos_historicos_escalaDetallado",
                data: obj,
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {
                    console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
                },
                success: function (data) {
                    $.JMensajePOPUP("Aviso:  Detalle de registro", data.d, "", "Cerrar", "fn_oculta_mensaje()", "");                  
                }
            });
        }

         
        $('#tabescalaeintervenciones5').click(function () {
            
            ObternerEdad();
        });

      
        function ObternerEdad()
        {
            var edad = $('#spDatosEdad').html();
            if (edad != "") {
                var split = edad.toString().split(" ");
                var i_edad = parseInt(split[0].toString());
                var Mes = split[1].toString();
                var validar = Mes.toUpperCase().includes("MES")
                var validarDia = Mes.toUpperCase().includes("DIA")
                var validarDIA2 = Mes.toUpperCase().includes("DÍA")


                if (validar == true)
                {
                    i_edad = i_edad / 12;
                }
                if (validarDia == true || validarDIA2 == true)
                {
                    i_edad = (i_edad / 12)/12;
                }
                //if (split[1].String().includes("Mes"))
                //{
                //    i_edad = i_edad/12
                //}
                
                document.getElementById('checkriesgocaida_5_6_1_0').checked = false;
                document.getElementById('checkriesgocaida_5_6_2_1').checked = false;
                document.getElementById('checkriesgocaida_5_6_3_1').checked = false;

                if (i_edad < 6) {
                    document.getElementById('checkriesgocaida_5_6_2_1').checked = true;
                    
                    Activar_desactivarRiegoCaida("5_6_2_1", true);
                }
                else if (i_edad > 66) {
                    document.getElementById('checkriesgocaida_5_6_3_1').checked = true;
                    Activar_desactivarRiegoCaida("5_6_3_1", true);
                }
                else {
                    document.getElementById('checkriesgocaida_5_6_1_0').checked = true;
                    Activar_desactivarRiegoCaida("5_6_1_0", true);
                }


                
                //document.getElementById('checkriesgocaida_5_6_1_0').checked = false;
                //
                //
            }            
        }   

        var date = new Date(Date.now()).toLocaleDateString("en-CA");
        document.getElementById("fechaInicioEscalaEIntervencionesenfermeria").value = date;
        document.getElementById("fechaInicioEscalaEIntervencionesenfermeria").setAttribute("max", date);
        document.getElementById("fechaFINEscalaEIntervencionesenfermeria").value = date;
        document.getElementById("fechaFINEscalaEIntervencionesenfermeria").setAttribute("max", date);
        $("#tabescalaeintervenciones1").attr('checked', true);
        document.getElementById("tabescalaeintervenciones1").checked = true;
    </script>
    <script>
        /*Script para el Kardex*/

        var Checkseleccionado = "";
       // mostrar_iniciochek();

        function MostrarDatosKardexEnfermeria() {
            $("#tablaDatosKardexEnfermeria").html('');

            $.ajax({
                type: "POST",
                url: "InformacionPaciente.aspx/Listar_Kardex_Hospitalarios",
                data: {},
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {
                    console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
                },
                success: function (data) {
                    $("#tablaDatosKardexEnfermeria").append(data.d);  
                    
                    try { var check1 = document.getElementById("tabclinico1").checked;  } catch { check1 = false;  }
                    try { var check2 = document.getElementById("tabclinico2").checked; } catch { check2 = false; }
                    try { var check3 = document.getElementById("tabclinico3").checked; } catch { check3 = false; }
                    try { var check4 = document.getElementById("tabclinico4").checked; } catch { check4 = false; }
                   
                    
                   
                   if (check1 == false && check2 == false && check3 == false && check4 == false)
                   {
                       mostrar_iniciochek();
                   }
                }
            });
        }

        function mostrar_iniciochek()
        {
            if (Checkseleccionado == "") {


                try {
                    var check1 = document.getElementById("tabclinico1").checked;

                    $("#tabclinico1").attr('checked', true);
                    document.getElementById("tabclinico1").checked = true;
                    document.getElementById("Contenidotabclinico1").style.display = "block";
                } catch
                {
                    try {
                        var check2 = document.getElementById("tabclinico2").checked;

                        $("#tabclinico2").attr('checked', true);
                        document.getElementById("tabclinico2").checked = true;
                        document.getElementById("Contenidotabclinico2").style.display = "block";
                    }
                    catch
                    {
                        try {
                            var check3 = document.getElementById("tabclinico3").checked;

                            $("#tabclinico3").attr('checked', true);
                            document.getElementById("tabclinico3").checked = true;
                            document.getElementById("Contenidotabclinico3").style.display = "block";
                        }
                        catch
                        {
                            try {
                                var check1 = document.getElementById("tabclinico4").checked;

                                $("#tabclinico4").attr('checked', true);
                                document.getElementById("tabclinico4").checked = true;
                                document.getElementById("Contenidotabclinico4").style.display = "block";
                            }
                            catch
                            {

                            }
                        }
                    }
                }
            }
            else {
                var _namecheckinput = "tabclinico" + Checkseleccionado.toString();
                var _contenedorinput = "Contenidotabclinico" + Checkseleccionado.toString();

                $(_namecheckinput).attr('checked', true);
                document.getElementById(_namecheckinput).checked = true;
                document.getElementById(_contenedorinput).style.display = "block";
            }
        }


        function clickInputKardexValidation(value)
        {
            var variable = value.value;
            var check = value.checked

            try { document.getElementById("Contenidotabclinico1").style.display = "none"; } catch { }
            try { document.getElementById("Contenidotabclinico2").style.display = "none"; } catch { }
            try { document.getElementById("Contenidotabclinico3").style.display = "none"; } catch { }
            try { document.getElementById("Contenidotabclinico4").style.display = "none"; } catch { }

            var namecheck = "Contenidotabclinico" + variable.toString();
            document.getElementById(namecheck).style.display = "block";

            Checkseleccionado = variable.toString();


        }

        function ProgramarHorarios(_val)
        {
            var detalle = _val.split("_");
            var ide_medicamentorec = detalle[0];
            var dsc_producto = detalle[1];
            var NumeracionFrecuencia = detalle[2];
            var num_frecuencia = detalle[3];
            $("#listadohoraprogramadadetallado").html('');

            var html = "<table><tbody><tr><td><table><tbody><tr><td><select id='selectprogramarthora' style='width: 80px;height: 30px;'><option value='1'>1</option><option>2</option><option>3</option><option>4</option><option>5</option><option>6</option><option>7</option><option>8</option><option>9</option><option>10</option><option>11</option><option>12</option> </select > </td><td><select id='selectprogramartdia'  value = 'AM'   style='width: 80px;height: 30px;'><option  value='AM'>AM</option> <option>PM</option> </select ></td><td><input style='width: 25px;height: 25px;' type='button' value='Calcular'  onclick = 'ProgramarHorariosenfermera(" + ide_medicamentorec + ", " + NumeracionFrecuencia + " , " + num_frecuencia + ")' /> </td> </tr> </tbody></table></td></tr><tr><td><table class='JSBTABLA' style='font-size: 18px;'><thead><tr><th>Horario</th></tr></thead><tbody id='listadohoraprogramadadetallado'></tbody></table> </td></tr></tbody></table>" ;


            $.JMensajePOPUP("Programar horario de "+ dsc_producto, html.toString(), "", "Guardar;Cerrar", "fn_grabar_ProgramacionHorario(" + ide_medicamentorec + ");fn_oculta_mensaje()", "");
           
        }

        function ProgramarHorariosenfermera(idemedicamentorec, numeracionfrecuencia, num_frecuencia)
        {
            var horainicio = document.getElementById("selectprogramarthora").value;
            var tipohora = document.getElementById("selectprogramartdia").value;
            $("#listadohoraprogramadadetallado").html('');

            var obj = JSON.stringify({
                _numeracionfrecuencia: parseInt(numeracionfrecuencia),
                _num_frecuencia: parseInt(num_frecuencia),
                _horainicio: parseInt(horainicio),
                _tipohora: tipohora
            });


            $.ajax({
                type: "POST",
                url: "InformacionPaciente.aspx/obtenerfechasprogramadas",
                data: obj,
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {
                    console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
                },
                success: function (data) {
                    $("#listadohoraprogramadadetallado").append(data.d);
                  
                }
            });

        }

        function fn_grabar_ProgramacionHorario(ide_medicamentorec)
        {
            var IDE = document.getElementById("ide_KardexHospitalarioItem").value;
            var horainicio = document.getElementById("selectprogramarthora").value;
            var tipohora = document.getElementById("selectprogramartdia").value;
            if (tipohora == "PM")
            {
                var horaInicial = parseInt(horainicio);
                if (horaInicial == 12)
                {
                    horainicio = 12;
                }
                else {
                    horainicio = parseInt(horainicio) + 12;
                }
            }
            if (tipohora == "AM")
            {
                var horaInicial = parseInt(horainicio);

                if (horaInicial == 12)
                {
                    horainicio = 0;
                }
            }

            if (parseInt(IDE) == 0) {
                fn_oculta_mensaje();
                IngresarPesoPacienteKardex();
            }
            else {
                $.JMensajePOPUP("Confirmación", "Está seguro de confirmar esta acción", "", "Procesar;Cerrar", "fn_procesar_programacion_horario(" + IDE + "," + horainicio + "," + ide_medicamentorec + ");fn_oculta_mensaje()", "");
            }
        }

        function IngresarPesoPacienteKardex()
        {
            var pesoo = document.getElementById("ide_pesopaciente").value;
            var Message = "<input type='text' style='width: 150px; font-size: 20px; '   id='inputpesopacientekardex'   minlength = '1'  maxlength = '5' size = '10' step='0.00' value='"+pesoo.toString()+"' onkeypress='return validateoinputdecimal(event)'/><label style='font-weight:bold;font-size:20px;'>KG</label>" ;
            //  ''
            $.JMensajePOPUP("Ingresar Peso del Paciente", Message, "", "Guardar;Cerrar", "fn_guardar_peso_paciente();fn_Desactivar_checkInput()", "");
            //document.getElementById("ide_pesopaciente").value = pesoo

        } 

        function validateoinputdecimal(evt)
        {
            cadena = document.getElementById("inputpesopacientekardex").value;
            var x = event.charCode;
            if (cadena.indexOf('.') == -1)
            {
                if (x == 46) { return true; }
                else { return true; }
               
            }
            else {
                if (x == 46 || x== 45) return false;
                var charCode = (evt.which) ? evt.which : event.keyCode
                if (charCode < 48 || charCode > 57)
                    return false;
                //if (evt.key.indexOf('.') == -1) return false;
                return true;
            }
            
        }

        function fn_guardar_peso_paciente()
        {
            var decimal_in = parseFloat(document.getElementById("inputpesopacientekardex").value.toString());
            if (decimal_in == 0 || document.getElementById("inputpesopacientekardex").value.toString() == "") {
                fn_oculta_mensaje();
                $.JMensajePOPUP("Aviso", "El peso debe ser mayor a cero", "", "Cerrar", "fn_Desactivar_checkInput()", "");
            }
            else if (decimal_in > 550) {
                fn_oculta_mensaje();
                $.JMensajePOPUP("Aviso", "Actualmente se maneja un rango de hasta 550 kg, por favor ingrese in peso válido", "", "Cerrar", "fn_Desactivar_checkInput()", "");
            }
            else
            {
               var obj = JSON.stringify({
                   peso:decimal_in
               });


               $.ajax({
                   type: "POST",
                   url: "InformacionPaciente.aspx/registrar_horas_programadas",
                   data: obj,
                   contentType: 'application/json; charset=utf-8',
                   error: function (xhr, ajaxOptions, thrownError) {
                       console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
                   },
                   success: function (data) {
                       fn_oculta_mensaje();
                       $.JMensajePOPUP("Aviso", data.d, "", "Cerrar", "fn_oculta_mensaje()", "");
                       MostrarDatosKardexEnfermeria();

                   }
               });
            }

        }

        function fn_procesar_programacion_horario(idekardex, horainicio, ide_medicamentorec)
        {
            var _ide = parseInt(idekardex)
            var _horainicio = parseInt(horainicio)
            var ide_meducamnto = parseInt(ide_medicamentorec)

            var obj = JSON.stringify({
                ide_kardexhospitalario : _ide,
                horaInicio : _horainicio ,
                ide_MedicamentoRec : ide_meducamnto
            });
            $.ajax({
                type: "POST",
                url: "InformacionPaciente.aspx/registrar_programacion_horas_N",
                data: obj,
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {
                    //console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
                    $.JMensajePOPUP("Aviso", xhr.status + " \n" + xhr.responseText, "\n" + thrownError, "", "Cerrar", "fn_oculta_mensaje()", "");
                },
                success: function (data) {
                    fn_oculta_mensaje();
                    $.JMensajePOPUP("Aviso", data.d, "", "Cerrar", "fn_oculta_mensaje()", "");
                    MostrarDatosKardexEnfermeria();

                }
            });

        }

        var data_valor;

        function Kardex_RegistrosDetalle(_val)
        {

            var IDE = document.getElementById("ide_KardexHospitalarioItem").value;

            var _check = _val.checked;
            if (_check == true)
            {
                data_valor = "";
                if (parseInt(IDE) == 0) {
                    fn_oculta_mensaje();
                    IngresarPesoPacienteKardex();
                }
                else {
                    data_valor = _val.value;

                    $.JMensajePOPUP("Confirmación", "Está seguro de confirmar esta acción?", "", "Procesar;Cerrar", "fn_procesar_kardex_hospitalario_hora();fn_Desactivar_checkInput()", "");

                }
            }
            
        }

        function fn_procesar_kardex_hospitalario_hora()
        {
            var IDE = document.getElementById("ide_KardexHospitalarioItem").value;

            var detalle = data_valor.split("_");

            var ide_medicamentorec = parseInt(detalle[0]);
            var ide_receta = parseInt(detalle[1]);
            var cod_atencion = detalle[2].toString();
            var IdTipo = parseInt(detalle[3]);
            var NombreTipo = detalle[4];
            var idekardexhospitalario = parseInt(IDE);



            var obj = JSON.stringify({
                IdeMedicamentorec: ide_medicamentorec,
                IdeReceta: ide_receta,
                CodAtencion: cod_atencion,
                IdeTipo: IdTipo,
                NombreTipos: NombreTipo,
                IdeKardexHospitalario: idekardexhospitalario
            });

            $.ajax({
                type: "POST",
                url: "InformacionPaciente.aspx/RegistrarSeleccionKardexHospitalario",
                data: obj,
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {
                    //console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
                    $.JMensajePOPUP("Aviso", xhr.status + " \n" + xhr.responseText, "\n" + thrownError, "", "Cerrar", "fn_oculta_mensaje()", "");
                },
                success: function (data) {
                    fn_oculta_mensaje();
                    $.JMensajePOPUP("Aviso", data.d, "", "Cerrar", "fn_oculta_mensaje()", "");
                    MostrarDatosKardexEnfermeria();
                    ////$("input[name='InputKardexhospitalarioenfermeria']").checked = false;

                }
            });

        }

        function fn_Desactivar_checkInput()
        {
            //$("input[name='InputKardexhospitalarioenfermeria']").checked = false;
            document.querySelectorAll("input[name='InputKardexhospitalarioenfermeria']").forEach(function (checkelement) { checkelement.checked = false; });
            fn_oculta_mensaje();

        }

        function Listar_datos_historicosKardexemfermeria() {
            var FechaInicio = document.getElementById("fechaInicioKardexenfermeria").value.toString();
            var FechaFin = document.getElementById("fechaFINKardexenfermeria").value.toString();
            $("#tablaDatosHistoricoskardexEnfermeria").html('');

            var obj = JSON.stringify({ Valor1: FechaInicio, Valor2: FechaFin });

            $.ajax({
                type: "POST",
                url: "InformacionPaciente.aspx/MostrarDastosHistoricosKardexEnfermeria",
                data: obj,
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {
                    console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
                },
                success: function (data) {                    
                    $("#tablaDatosHistoricoskardexEnfermeria").append(data.d);
                }
            });

        }


        MostrarDatosKardexEnfermeria();

        var fdate = new Date(Date.now()).toLocaleDateString("en-CA");
        document.getElementById("fechaInicioKardexenfermeria").value = fdate;
        document.getElementById("fechaInicioKardexenfermeria").setAttribute("max", fdate);// = fdate;
        document.getElementById("fechaFINKardexenfermeria").value = fdate;
        document.getElementById("fechaFINKardexenfermeria").setAttribute("max", fdate)

        //




    </script>

     <script>

         window.onload = function ()
         {
             var test = document.querySelectorAll('.tabs_historiaclinica');
             for (var i = 0; i < test.length; i++)
             {
                 test[i].addEventListener("click", function () {
                     let sDescripcionAcordeon = 'Historia Clínica';
                     fn_GuardaLog(sDescripcionAcordeon, "Se ingreso a la opción de " + this.id);
                     
                 });
             }
         }

         $("#kardexEnfermerialabel").click(function ()
         {
             fn_cargar_permisos_tableta();
         });

         $("#escalaseintervencionlabel").click(function () {
             fn_cargar_permisos_tableta();
         });

         function fn_cargar_permisos_tableta() {
             try {
                 var obj = JSON.stringify({ IdeModulo: $("#" + "<%=hfCodigoFormulario.ClientID %>").val() });
                 $.ajax({
                     type: "POST",
                     url: "InformacionPaciente.aspx/CargaPermiso",
                     data: obj,
                     contentType: 'application/json; charset=utf-8',
                     error: function (xhr, ajaxOptions, thrownError) {
                         console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
                     },
                     success: function (data) {
                         validar_permisosSIC(data.d);
                         data_permiso = data.d;
                         //console.log(data.d);

                     }
                 });
             } catch { }      
         }

         function validar_permisosSIC(data)
         {     
             if ( data == "OTROS")
             {
                    
             }
             //  $(":input").attr("disabled", "disabled"); contenidokardexhospitalario

             if (data == "Medico" || data == null || data== '')
             {
                 $("[id*=tablaescala1]").find(":input").attr("disabled", "disabled");
                 $("[id*=tablaescala2]").find(":input").attr("disabled", "disabled");
                 $("[id*=tablaescala3]").find(":input").attr("disabled", "disabled");
                 $("[id*=tablaescala4]").find(":input").attr("disabled", "disabled");
                 $("[id*=tablaescala5]").find(":input").attr("disabled", "disabled");

                 $("[id*=btnbuttonescala1]").find(":input").attr("disabled", "disabled");
                 $("[id*=btnbuttonescala2]").find(":input").attr("disabled", "disabled");
                 $("[id*=btnbuttonescala3]").find(":input").attr("disabled", "disabled");
                 $("[id*=btnbuttonescala4]").find(":input").attr("disabled", "disabled");
                 $("[id*=btnbuttonescala5]").find(":input").attr("disabled", "disabled");

                 try { $("[id*=tablekardexenfermeria1]").find(":input").attr("disabled", "disabled"); } catch { }
                 try { $("[id*=tablekardexenfermeria2]").find(":input").attr("disabled", "disabled"); } catch { }
                 try { $("[id*=tablekardexenfermeria3]").find(":input").attr("disabled", "disabled"); } catch { }
                 try { $("[id*=tablekardexenfermeria4]").find(":input").attr("disabled", "disabled"); } catch { }
                 
             }

             if (data == "Desarrollo" || data == "Enfermera" || data == "" )
             {
                 $("[id*=contenidokardexhospitalario]").find(":input").removeAttr("disabled");
                 $("[id*=contenidokardexhospitalario]").find("img").removeAttr("disabled");

                 $("[id*=contenidoescalahospitalario]").find(":input").removeAttr("disabled");
                

                 $("[id*=contenidoescalahospitalario]").find("img").css("opacity", "1");
                 $("[id*=contenidoescalahospitalario]").find("img").removeAttr("disabled");

                 $("[id*=tablaescala1]").find(":input").removeAttr("disabled");
                 $("[id*=tablaescala2]").find(":input").removeAttr("disabled");
                 $("[id*=tablaescala3]").find(":input").removeAttr("disabled");
                 $("[id*=tablaescala4]").find(":input").removeAttr("disabled");
                 $("[id*=tablaescala5]").find(":input").removeAttr("disabled");

                 $("[id*=btnbuttonescala1]").find(":input").removeAttr("disabled");
                 $("[id*=btnbuttonescala2]").find(":input").removeAttr("disabled");
                 $("[id*=btnbuttonescala3]").find(":input").removeAttr("disabled");
                 $("[id*=btnbuttonescala4]").find(":input").removeAttr("disabled");
                 $("[id*=btnbuttonescala5]").find(":input").removeAttr("disabled");

                 try { $("[id*=tablekardexenfermeria1]").find(":input").removeAttr("disabled"); } catch { }
                 try { $("[id*=tablekardexenfermeria2]").find(":input").removeAttr("disabled"); } catch { }
                 try { $("[id*=tablekardexenfermeria3]").find(":input").removeAttr("disabled"); } catch { }
                 try { $("[id*=tablekardexenfermeria4]").find(":input").removeAttr("disabled"); } catch { }

                 
             }           
         }

        $(".JDIV_ACOR_CONTENEDOR > label").click(function () {
            var sDescripcionAcordeon = $(this.id);
            var detalle = sDescripcionAcordeon.selector;
            if(detalle=="kardexEnfermerialabel" || detalle=="escalaseintervencionlabel")
            {
                validar_permisosSIC(data_permiso);    
            }
        });
         function fn_Load_Page() {
             MostrarDatosKardexEnfermeria();
         }
         fn_cargar_permisos_tableta();
    

        var data_permiso;
     </script>

  <script>
      let timeoutHandle;

      var Minutos = 15;//se pone por default se va a agregar valor desde la base de datos
      var timeOutPeriodo = Minutos * 60 * 1000;//Convenimos a Molisegundos
      ObtenerTimeOutBD();

      function ObtenerTimeOutBD() {
          try {
              $.ajax({
                  url: "InformacionPaciente.aspx/MostrarTimeOutInactividad",
                  type: "POST",
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  error: function (dato1, datos2, dato3) {
                  }
              }).done(function (oOB_JSON) {
                  let dto = oOB_JSON.d;
                  if (dto > 0) {
                      timeOutPeriodo = dto * 60 * 1000;
                  }
              });
          }
          catch (error) {

          }
      }

      function ResetTimeOut() {
          clearTimeout(timeoutHandle);
          timeoutHandle = setTimeout(LogoutTimeOut, timeOutPeriodo);
      }

      function LogoutTimeOut() {
          try {
              $.ajax({
                  url: "InformacionPaciente.aspx/CerrarSesion",
                  type: "POST",
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  error: function (dato1, datos2, dato3) {
                  }
              }).done(function (oOB_JSON) {
                  window.location.href = "ConsultaPacienteHospitalizado.aspx";
              });
          }
          catch (error) {
              window.location.href = "ConsultaPacienteHospitalizado.aspx";
          }
      }

      document.addEventListener('mousemove', ResetTimeOut);
      document.addEventListener('keypress', ResetTimeOut);
      document.addEventListener('click', ResetTimeOut);
      ResetTimeOut();

  </script>
  
    <input type="hidden" id="hfCodigoFormulario" value="86" runat="server" />
    <input type="hidden" id="hfAcordeonAbierto" value="" runat="server" />
    <input type="hidden" id="hfHistoriaClinicaHoras" value="" runat="server" />
    <input type="hidden" id="hfHistoriaClinicaMedico" value="" runat="server" />
    <input type="hidden" id="hfAdministrativo" value="" runat="server" />

   
</asp:Content>