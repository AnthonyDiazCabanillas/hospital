<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Botonera.aspx.vb" Inherits="WebHCE.Botonera" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>    
    <script type="text/javascript">
        $(document).ready(function () {
            fn_CreaTooltip();

            $("#imgHome").click(function ()
            {
                //window.location.replace("ConsultaPacienteHospitalizado.aspx");
                window.location.href = "ConsultaPacienteHospitalizado.aspx";
            });
            $("#imgEditar").click(function ()
            {
                if ($("[id*=hfAdministrativo]").val() == "ADMINISTRATIVOS")
                { //JB - 24/06/2020 - NUEVO CODIGO PARA EVITAR QUE INGRESE A ESTA OPCION UN ADMINISTRATIVO
                    return false;
                }
                $.ajax({ url: "Utilidad/Botonera.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON)
                {
                    if (oOB_JSON.d == "")
                    {
                        let sDescripcionAcordeon = 'HCE_DECLARATORIA_ALERGIA';
                        fn_GuardaLog(sDescripcionAcordeon, "Se ingreso a la opción");
                        var aVA_DEMO = ["#divDatosUsuarioreconciliacion", "Utilidad/DatosUsuarioPopUp.aspx", ""];
                        $.JPopUp("Reconciliación Medicamentosa", "PopUp/RegMedicoReconciliacionMedicamento.aspx", "1", "Salir", "fn_oculta_popup_medicamentosa()", 85, aVA_DEMO); //fn_NuevoReconciliacionMedicamentosa
                    }
                    else
                    {
                        aValores = oOB_JSON.d.toString().split(";");
                        $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                    }
                });
            });

            //DECLARATORIA DE ALERGIA
            $("#imgPastillaA").click(function ()
            {
                if ($("[id*=hfAdministrativo]").val() == "ADMINISTRATIVOS")
                { //JB - 24/06/2020 - NUEVO CODIGO PARA EVITAR QUE INGRESE A ESTA OPCION UN ADMINISTRATIVO
                    return false;
                }
                $.ajax({ url: "Utilidad/Botonera.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON)
                {
                    if (oOB_JSON.d == "")
                    {
                        let sDescripcionAcordeon = 'HCE_DECLARATORIA_ALERGIA';
                        fn_GuardaLog(sDescripcionAcordeon, "Se ingreso a la opción");
                        var aVA_DEMO = ["#DatosUsuarioDeclaratoriaAlergia", "Utilidad/DatosUsuarioPopUp.aspx", "ALERGIA"];
                        $.JPopUp("", "PopUp/DeclaratoriaAlergia.aspx", "2", "Guardar;Salir", "fn_GuardarDeclaratoriaAlergia();fn_CierraPopup()", 85, aVA_DEMO); //fn_EditarDeclaratoriaAlergia
                    }
                    else
                    {
                        aValores = oOB_JSON.d.toString().split(";");
                        $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                    }
                });
            });

            $("#imgImprimir").click(function ()
            {
                if ($.find("#frmGridConsultaPacienteHospitalizado").length > 0)
                {
                    var nFilaSeleccionada = 0;
                    $("#frmGridConsultaPacienteHospitalizado").find(".JSBTABLA tr").each(function ()
                    {
                        if ($(this).find(".JFILA-SELECCIONADA").length != 0) {
                            nFilaSeleccionada = $(this).find(".JFILA-SELECCIONADA").length;
                        }
                    });
                    if (nFilaSeleccionada != 0)
                    {
                        var aValores = ["S"];
                        $.JPopUp("Acceso", "PopUp/Acceso.aspx", "2", "Ingresar;Cerrar", "fn_IngresarLogin();fn_oculta_popup()", 45, "", aValores);
                        //$.JPopUp("Impresion de Reportes", "PopUp/ImpresionReporte.aspx", "2", "Aceptar;Salir", "fn_Imprimir();fn_oculta_popup()", 50); //ImprimirReporte.aspx 31/01/2017
                    } else
                    {
                        $.JMensajePOPUP("Impresion de Reportes", "Es necesario que seleccione un paciente.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()");
                    }
                } else {
                    $.ajax({ url: "Utilidad/Botonera.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) { //validara session
                        if (oOB_JSON.d == "")
                        {
                            let sDescripcionAcordeon = 'HCE_IMPRESION_REPORTES';
                            fn_GuardaLog(sDescripcionAcordeon, "Se ingreso a la opción");
                            $.JPopUp("Impresion de Reportes", "PopUp/ImpresionReporte.aspx", "2", "Aceptar;Salir", "fn_Imprimir();fn_oculta_popup()", 50); //ImprimirReporte.aspx 31/01/2017 
                        }
                    });

                }



                /*$.ajax({ url: "Utilidad/Botonera.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                $.JPopUp("Impresion de Reportes", "PopUp/ImprimirReporte.aspx", "2", "Aceptar;Salir", "fn_Imprimir();fn_oculta_popup()", 40);
                } else {
                var aValorers = oOB_JSON.d.toString().split(";");
                window.location.href = aValorers[1];
                }
                });*/
            });

            $("#imgReceta").click(function ()
            {
                fn_LOAD_VISI();
                if ($("[id*=hfAdministrativo]").val() == "ADMINISTRATIVOS")
                { //JB - 24/06/2020 - NUEVO CODIGO PARA EVITAR QUE INGRESE A ESTA OPCION UN ADMINISTRATIVO
                    return false;
                }
                $.ajax({ url: "Utilidad/Botonera.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON)
                {
                    if (oOB_JSON.d == "")
                    {
                        $.ajax({ url: "Utilidad/Botonera.aspx/Validacion", type: "POST", contentType: "application/json; charset=utf-8", data: JSON.stringify({ Tipo: "1" }), dataType: "json" }).done(function (oOB_JSON)
                        {
                            fn_LOAD_OCUL();
                            if (oOB_JSON.d == "")
                            {
                                let sDescripcionAcordeon = 'HCE_RECETA_ALTA';
                                fn_GuardaLog(sDescripcionAcordeon, "Se ingreso a la opción");
                                var aVA_DEMO = ["#DatosUsuarioReceta", "Utilidad/DatosUsuarioPopUp.aspx", ""];
                                $.JPopUp("Receta", "PopUp/Receta.aspx", "2", "Guardar;Salir", "fn_GuardarReceta();fn_CerrarPopUp()", 85, aVA_DEMO);
                            } else
                            {
                                $.JMensajePOPUP("Mensaje del sistema Clinica San Felipe", oOB_JSON.d, "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()");
                            }
                        });
                    } else {
                        aValores = oOB_JSON.d.toString().split(";");
                        $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                    }
                });
            });

            $("#imgPedidos").click(function ()
            {
                if ($("[id*=hfAdministrativo]").val() == "ADMINISTRATIVOS")
                { //JB - 24/06/2020 - NUEVO CODIGO PARA EVITAR QUE INGRESE A ESTA OPCION UN ADMINISTRATIVO
                    return false;
                }
                $.ajax({ url: "Utilidad/Botonera.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON)
                {
                    if (oOB_JSON.d == "")
                    {
                        let sDescripcionAcordeon = 'HCE_PEDIDOS';
                        fn_GuardaLog(sDescripcionAcordeon, "Se ingreso a la opción");
                        var aVA_DEMO = ["#DatosUsuarioPedido", "Utilidad/DatosUsuarioPopUp.aspx", ""];
                        $.JPopUp("Información", "PopUp/Pedido.aspx", "2", "Imprimir;Salir", "fn_ImprimirPedido();fn_oculta_popup()", 65, aVA_DEMO);
                    } else {
                        aValores = oOB_JSON.d.toString().split(";");
                        $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                    }
                });
            });

            $("#imgAlerta").click(function ()
            {
                if ($("[id*=hfAdministrativo]").val() == "ADMINISTRATIVOS")
                { //JB - 24/06/2020 - NUEVO CODIGO PARA EVITAR QUE INGRESE A ESTA OPCION UN ADMINISTRATIVO
                    return false;
                }
                $.ajax({ url: "Utilidad/Botonera.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON)
                {
                    if (oOB_JSON.d == "")
                    {
                        $.ajax({
                            url: "Utilidad/Botonera.aspx/VerificarAlertas",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            error: function (dato1, datos2, dato3) {
                            }
                        }).done(function (oOB_JSON) {
                            aValoresAlerta = oOB_JSON.d.toString().split(";");
                            if (aValoresAlerta[0] == "ALERTA")
                            {
                                let sDescripcionAcordeon = 'HCE_ALERTA';
                                fn_GuardaLog(sDescripcionAcordeon, "Se ingreso a la opción");

                                var aVA_DEMO = ["#DatosUsuarioAlerta", "Utilidad/DatosUsuarioPopUp.aspx", ""];
                                /*$.JPopUp("Alerta", "PopUp/Alerta.aspx", "1", "Salir", "fn_oculta_popup()", 50, aVA_DEMO);*/
                                $.JPopUp("Alerta", "PopUp/Alerta.aspx", "2", "Verificar;Salir", "fn_VerificarAlerta();fn_oculta_popup()", 50, aVA_DEMO);
                                //$.JPopUp("Alerta", "PopUp/Alerta.aspx", "2", "Verificar;Salir", "fn_VerificarAlerta();fn_oculta_popup()", 55, aVA_DEMO);
                            }
                        });
                    } else {
                        aValores = oOB_JSON.d.toString().split(";");
                        $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                    }
                });
            });

            $("#imgLoginEnfermera").click(function ()
            {
                //window.location.href = "../LoginEnfermera.aspx";
                if ($.find("#frmGridConsultaPacienteHospitalizado").length > 0)
                {
                    var nFilaSeleccionada = 0;
                    $("#frmGridConsultaPacienteHospitalizado").find(".JSBTABLA tr").each(function ()
                    {
                        if ($(this).find(".JFILA-SELECCIONADA").length != 0) {
                            nFilaSeleccionada = $(this).find(".JFILA-SELECCIONADA").length;
                        }
                    });
                    if (nFilaSeleccionada != 0) {
                        var sCodigoAtencion = $(".JFILA-SELECCIONADA").eq(4).html().trim();
                        var sIdHistoria = $(".JFILA-SELECCIONADA").parent().find("td:last").html().trim();
                        var aDEMO = [sCodigoAtencion, sIdHistoria];
                        //$.JPopUp("Acceso Enfermera", "PopUp/AccesoEnfermera.aspx", "2", "Ingresar;Salir", "fn_IngresarLoginEnfermera();fn_oculta_popup()", 45, "", aDEMO); 30/01/2017
                        $.JPopUp("Acceso", "PopUp/Acceso.aspx", "2", "Ingresar;Cerrar", "fn_IngresarLogin();fn_oculta_popup()", 45); //30/01/2017
                    }
                }
            });

            /*ALTA MEDICA - 14/11/2016*/
            $("#imgAltaMedica").click(function ()
            {
                if ($("[id*=hfAdministrativo]").val() == "ADMINISTRATIVOS")
                { //JB - 24/06/2020 - NUEVO CODIGO PARA EVITAR QUE INGRESE A ESTA OPCION UN ADMINISTRATIVO
                    return false;
                }
                $.ajax({ url: "Utilidad/Botonera.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON)
                {
                    if (oOB_JSON.d == "")
                    {
                        $.ajax({ url: "Utilidad/Botonera.aspx/Validacion", type: "POST", contentType: "application/json; charset=utf-8", data: JSON.stringify({ Tipo: "2" }), dataType: "json" }).done(function (oOB_JSON) {
                            if (oOB_JSON.d == "")
                            {
                                //$.JPopUp("Alta Médica", "PopUp/AltaMedica.aspx", "2", "Aceptar;Salir", "fn_AceptarAltaMedica();fn_oculta_popup()", 50); JB - COMENTADO - 27/01/2020
                                if ($("[id*=hfAdministrativo]").val() == "DIRECCION MEDICA")
                                {
                                    let sDescripcionAcordeon = 'HCE_ALTA_MEDICA';
                                    fn_GuardaLog(sDescripcionAcordeon, "Se ingreso a la opción");
                                    fn_InvocaPopUpAltaMedica();
                                }
                                else
                                {
                                    $.ajax({ url: "Utilidad/Botonera.aspx/ValidaAlta", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON)
                                    {
                                        if (oOB_JSON.d != "1")
                                        {
                                            let sDescripcionAcordeon = 'HCE_ALTA_MEDICA';
                                            fn_GuardaLog(sDescripcionAcordeon, "Se ingreso a la opción");
                                            fn_InvocaPopUpAltaMedica();
                                        }
                                        else
                                        {
                                            $.JMensajePOPUP("Mensaje del sistema Clinica San Felipe", "El paciente ya se encuentra de alta", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()");
                                        }
                                    });
                                }                                
                            } else {
                                $.JMensajePOPUP("Mensaje del sistema Clinica San Felipe", "¿Su paciente requiere receta de alta?", "ADVERTENCIA", "Si;No", "fn_AceptaRequiereRecetaAlta();fn_NoAceptaRequiereRecetaAlta()");

                                //$.JMensajePOPUP("Mensaje del sistema Clinica San Felipe", oOB_JSON.d, "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()");
                            }
                        });
                    } else {
                        aValores = oOB_JSON.d.toString().split(";");
                        $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                    }
                });
            });
            /*FIN 14/11/2016*/

            $("#imgRoeLaboratorioB").click(function ()
            {
                $.ajax({ url: "Utilidad/Botonera.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "")
                    {
                        let sDescripcionAcordeon = 'HCE_ACCESO_ROE';
                        fn_GuardaLog(sDescripcionAcordeon, "Se ingreso a la opción");
                        //CODIGO AQUI QUE TRAERA LA URL
                        $.ajax({
                            url: "Utilidad/Botonera.aspx/ObtenerUrlRoe", //obtiene mensaje para el tittle del boton de alerta
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            error: function (dato1, datos2, dato3) {
                            }
                        }).done(function (oOB_JSON)
                        {
                            if (oOB_JSON.d != "") {
                                if (oOB_JSON.d.toString().split("*")[0].trim() == "OK")
                                {
                                    window.open(oOB_JSON.d.toString().split("*")[1].trim(), "_blank");
                                } else
                                {
                                    $.JMensajePOPUP("Error", oOB_JSON.d.toString().split("*")[1].trim(), "ERROR", "Cerrar", "fn_oculta_mensaje()");
                                }
                            }
                        });                        
                    } else
                    {
                        $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                    }
                });
            });

            $("#imgRiskCalculator").click(function ()
            {
                let sDescripcionAcordeon = 'HCE_CALCULADOR_RIEGOS_Q';
                fn_GuardaLog(sDescripcionAcordeon, "Se ingreso a la opción");
                window.open("https://riskcalculator.facs.org/RiskCalculator/Outcome.jsp");
            });

            $("#imgGuiasClinicas").click(function ()
            {
                let sDescripcionAcordeon = 'HCE_GUIAS_PRACTICAS_CLINICAS';
                fn_GuardaLog(sDescripcionAcordeon, "Se ingreso a la opción");
                window.open("https://login.mcg.com/account/login?returnUrl=https%3A%2F%2Fcgi.careguidelines.com%2F&ClientId=Careweb");
                //ttps://login.mcg.com/account/login?returnUrl=https%3A%2F%2Fcgi.careguidelines.com%2F&ClientId=Careweb
                //ttp://192.168.22.251:8775/ed23/
            });

            $("#imgSinadef").click(function ()
            {
                let sDescripcionAcordeon = 'HCE_SINADEF';
                fn_GuardaLog(sDescripcionAcordeon, "Se ingreso a la opción");
                window.open("https://www.minsa.gob.pe/defunciones/");
            });

            $("#imgReporteHM").click(function () {
                var aValores = ["S"];
                $.JPopUp("Acceso", "PopUp/Acceso.aspx", "2", "Ingresar;Cerrar", "fn_IngresarLogin();fn_oculta_popup()", 45, "", aValores);
            });

            $("#imgInterconTumores").click(function () {
                let sDescripcionAcordeon = 'HCE_INTERCONSULTA';
                fn_GuardaLog(sDescripcionAcordeon, "Se ingreso a la opción de comité de tumores");
                window.open("https://docs.google.com/forms/d/e/1FAIpQLSdU2p32v4wcBD65YrW_1F2KSxfwMbTJrRrH_XzoQVOfiaIzOA/viewform");
            });
            
        });

        function fn_InvocaAlertaSuspension()
        {            
            var IdRecetaDet = $("#hfAlerta").val();
            var aVA_DEMO = ["#divDatosUsuarioVerificarMedicamento", "Utilidad/DatosUsuarioPopUp.aspx", ""];
            var aValores = [IdRecetaDet];
            $.JPopUp("Verificación de Suspensión de Medicamentos - Control Clínico", "PopUp/VerificarMedicamento.aspx", "2", "Verificar;Salir", "fn_VerificarSuspension();fn_oculta_popup()", 75, aVA_DEMO, aValores);
        }

        function fn_InvocaExamenCompletado()
        {
            var IdRecetaDet = $("#hfAlerta").val();
            var aVA_DEMO = ["#divDatosExamenCompletado", "Utilidad/DatosUsuarioPopUp.aspx", ""];
            var aValores = [IdRecetaDet];
            $.JPopUp("Examen Completado", "PopUp/ExamenCompletado.aspx", "2", "Verificar;Salir", "fn_VerificarExamen();fn_oculta_popup()", 75, aVA_DEMO, aValores);
        }

        function fn_InvocaLaboratorioCompletado()
        {
            var IdRecetaDet = $("#hfAlerta").val();
            var aVA_DEMO = ["#divDatosLaboratorioCompletado", "Utilidad/DatosUsuarioPopUp.aspx", ""];
            var aValores = [IdRecetaDet];
            $.JPopUp("Examen Completado", "PopUp/LaboratorioCompletado.aspx", "2", "Verificar;Salir", "fn_VerificarLaboratorio();fn_oculta_popup()", 75, aVA_DEMO, aValores);
        }

        function fn_AbreVentanaReporte()
        {            
            $.JPopUp("Reporte", "PopUp/ReporteHM.aspx", "2", "Aceptar;Salir", "fn_ImprimirReporteHM();fn_CierraReporteHM()", 30);
        }

        function fn_InvocaPopUpAltaMedica()
        {
            $("[id*=btnCargarAntecenteAltaMedica]").trigger("click"); 
            $("[id*=btnCargarDatosAltaMedicaEP]").trigger("click");
            $("[id*=btnCargarDiagnosticoAltaMedicaEP]").trigger("click");
            fn_CargarAltaMedica();
            fn_MostrarPopup2("divPopUpAltaMedicaEpicrisis", false);
        }

        function fn_AceptaRequiereRecetaAlta()
        {
            fn_oculta_mensaje_rapido();
            $("#imgReceta").trigger("click");
        }

        function fn_NoAceptaRequiereRecetaAlta()
        {
            fn_oculta_mensaje_rapido();
            fn_InvocaPopUpAltaMedica();
        }
        
    </script>

    <style type="text/css">
        .JIMG-ALERTA
        {
                opacity:0.8; /*0.6*/
				width:40px;
				height:40px;
				-webkit-animation: ANIMACION2 1s infinite linear;
				animation: ANIMACION2 1s infinite linear;
				border-radius: 25px;
				/*box-shadow: 0 0 10px 3px Red;*/
            }
            
            @-webkit-keyframes ANIMACION2 {
				0% { -webkit-transform: scale(0.5); opacity: 0.1}
				100% { -webkit-transform: scale(1.1); opacity: 0.9} /*0.5*/
			}
			@keyframes ANIMACION2 {
				0% { transform: scale(0.5); opacity: 0.1}
				100% { transform: scale(1.1); opacity: 0.9} 
			}
			
			
    </style>
</head>
<body>
    <input type="hidden" id="hfAlerta" />
    <input type="hidden" id="hfOpcionAlerta" />
    <div class="JFILA" style="overflow:initial;">
        <div class="JCELDA-12" style="overflow:initial;">
            <div class="JDIV-CONTROLES" style="border:  15px #8DC73F;">
                <img src="../Imagenes/Enfermera.png" alt="" class="JIMG-GENERAL" id="imgLoginEnfermera" title="Acceder como enfermera" style="display:none !important;" />
                <div class="tooltip">
                    <img src="../Imagenes/Home.png" alt="" class="JIMG-GENERAL" id="imgHome" title="Inicio" />
                    <span tooltip-direccion="abajo">Inicio</span>
                </div>
                <div class="tooltip">
                    <img src="../Imagenes/Edit.png" name="08/01" alt="" class="JIMG-GENERAL" id="imgEditar" title="Registro Médico de Rec. medicamentosa"  /> <%--Edit.png--%>
                    <span tooltip-direccion="abajo">Rec. medicamentosa</span>
                </div>
                <div class="tooltip">
                    <img src="../Imagenes/Pastilla_abierta.png" alt="" name="09/01" class="JIMG-GENERAL" id="imgPastillaA" title="Declaratoria de Alergia"  /> <%--Pastilla_abierta--%>
                    <span tooltip-direccion="abajo">Declaratoria de Alergia</span>
                </div>
                <div class="tooltip">
                    <img src="../Imagenes/Receta.png" alt="" name="10/01" class="JIMG-GENERAL" id="imgReceta" title="Receta de Alta" /> <%--Receta--%>
                    <span tooltip-direccion="abajo">Receta de Alta</span>
                </div>
                <div class="tooltip">
                    <img src="../Imagenes/lista_pedidos.png" alt="" class="JIMG-GENERAL" id="imgPedidos" title="Pedidos" />  <%--lista_pedidos--%>
                    <span tooltip-direccion="abajo">Pedidos</span>
                </div>
                <div class="tooltip">
                    <img src="../Imagenes/ico_alta.png" name="12/01" class="JIMG-GENERAL" id="imgAltaMedica" title="Alta Médica" /> <%--ico_alta--%>
                    <span tooltip-direccion="abajo">Alta Médica</span>
                </div>
                <div class="tooltip">
                    <img src="../Imagenes/Alerta.png" alt="" class="JIMG-GENERAL" id="imgAlerta" title="Alerta" style="border-radius:5px;" />
                    <span tooltip-direccion="abajo">Alerta</span>
                </div>
                <div class="tooltip">
                    <img src="../Imagenes/RiskCalculator.png" alt="" class="JIMG-GENERAL" id="imgRiskCalculator" title="Calculador de Riesgo Quirúrgico" style="border-radius:5px;" width="70px" height="35" />
                    <span tooltip-direccion="abajo">Calculador de Riesgo Quirúrgico</span>
                </div>
                <div class="tooltip">
                    <img src="../Imagenes/GPC.jpg" alt="" class="JIMG-GENERAL" id="imgGuiasClinicas" title="Guías Clínicas" style="border-radius:5px;" width="50px" height="35" />
                    <span tooltip-direccion="abajo">Guías de prácticas clínicas</span>
                </div>
                <div class="tooltip">
                    <img src="../Imagenes/Sinadef.png" alt="" class="JIMG-GENERAL" id="imgSinadef" title="Guías Clínicas" style="border-radius:5px;" width="35px" height="35" />
                    <span tooltip-direccion="abajo">SINADEF</span>
                </div>
                <div class="tooltip">
                    <%--<input type="button" id="" title="ROE" class="JBOTON-IMAGEN" name="05/01/03" style="background-image:url(../Imagenes/ROE2.png);background-size: 30px 30px;width: 30px;height: 30px;" />--%>  <%--Petitorio_Laboratorio.png  CAMBIO IMG  Microscopio1 width:35px;height:35px;background-size: 30px 30px;--%>
                    <img src="../Imagenes/ROE2.png" alt="" class="JIMG-GENERAL" id="imgRoeLaboratorioB" title="ROE" style="border-radius:5px;" width="35px" height="35" />
                    <span tooltip-direccion="arriba">ROE</span> 
                </div>
                <div class="tooltip" style="float:right;">
                    <img src="../Imagenes/imprimir.png" alt="" class="JIMG-GENERAL" style="float:right;margin-right:15px;margin-top:5px;" id="imgImprimir" />                
                    <span tooltip-direccion="abajo">Imprimir</span>
                </div>
                <div class="tooltip" style="float:right;">
                    <img src="../Imagenes/ReporteHM.png" alt="" class="JIMG-GENERAL" style="float:right;margin-right:15px;margin-top:5px;width:35px;height:35px;" id="imgReporteHM" />                
                    <span tooltip-direccion="abajo">Reporte</span>
                </div>
                <div class="tooltip">
                    <img src="../Imagenes/intercon.png" alt="" class="JIMG-GENERAL" id="imgInterconTumores" style="border-radius:5px;" width="35px" height="35" />
                    <span tooltip-direccion="abajo">Interconsulta al Comité de Tumores</span>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
