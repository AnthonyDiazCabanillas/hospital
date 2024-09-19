<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Medicamentos.aspx.vb" Inherits="WebHCE.Medicamentos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        $(document).ready(function () {
            fn_CampoBase();
            //$(".DatosUsuario").load("Utilidad/DatosUsuario.aspx");
            $("#divGridReconciliacionMedicamentosa").load("GridViewAjax/GridReconciliacionMedicamentosa.aspx?Medicamentos=3&IdPatologia=" + $("#" + "<%=IdExamenFisicoCores.ClientID %>").val().trim(), function () {
                fn_CreaEventoGridReconciliacion();
            });

            if ($("#" + "<%=IdExamenFisicoCores.ClientID %>").val().trim() != "") {
                $("#hfCodigoAntecedentePatologico").val($("#" + "<%=IdExamenFisicoCores.ClientID %>").val().trim());
            }
            if ($("#" + "<%=DescripcionExamenFisicoCore.ClientID %>").val().trim() != "") {
                $("#txtAntecedentesPatologicos").val($("#" + "<%=DescripcionExamenFisicoCore.ClientID %>").val().trim());
            }

            $.JValidaCampoObligatorio("txtMedicamentoRegMedico;txtDosisRegMedico;txtViaRegMedico;txtFrecuenciaRegMedico;txtAntecedentesPatologicos;txtFechaUltima;txtHoraUltima");

            $("#divFONDO1").click(function () {
                $(".JBUSQUEDA-ESPECIAL").css("display", "none");
                $(this).css("display", "none");
            });
            //BUSQUEDA ANTECEDENTES PATOLOGICOS
            $("#txtAntecedentesPatologicos").keypress(function (e) {
                if (e.which == 13) {
                    $("#imgBusquedaAntecedetesPatologicos").trigger("click");
                }
            });
            $("#imgBusquedaAntecedetesPatologicos").click(function () {
                $.ajax({ url: "PopUp/Medicamentos.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "") {
                        $("#divFONDO1").css("display", "inline");
                        $("#divAntecedentePatologicos").css("display", "inline");
                        $("#divAntecedentePatologicos").load("Utilidad/BusquedaAntecedentesPatologicos.aspx", { Buscar: $("#txtAntecedentesPatologicos").val().trim() }, function () {
                            fn_CreaEventoBusquedaAntecedentePatologico();
                        });
                    } else {
                        aValores = oOB_JSON.d.toString().split(";");
                        $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession_Medicamentos()", "frmMedicamentos");
                    }
                });
            });

            //BUSQUEDA MEDICAMENTO
            $("#imgBusquedaMedicamentosa").click(function () {
                $.ajax({ url: "PopUp/Medicamentos.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "") {
                        if ($("#txtMedicamentoRegMedico").val().trim().length > 3) {
                            $("#divFONDO1").css("display", "inline");
                            $("#divBusquedaMedicamentoRegistroMedicamentosa").css("display", "block");
                            $("#divBusquedaMedicamentoRegistroMedicamentosa").html("");
                            $("#divBusquedaMedicamentoRegistroMedicamentosa").load("Utilidad/BusquedaMedicamento.aspx", { Nombre: $("#txtMedicamentoRegMedico").val().trim() }, function () {
                                fn_CrearEventoMedicamentoReconciliacionMedicamentosa();
                            });
                        } else {
                            $.JMensajePOPUP("Aviso", "Debe ingresar al menos 4 caracteres para realizar una búsqueda.", "", "Cerrar", "fn_oculta_mensaje()", "frmMedicamentos");
                        }
                    } else {
                        aValores = oOB_JSON.d.toString().split(";");
                        $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession_Medicamentos()", "frmMedicamentos");
                    }
                });
            });

            $("#txtMedicamentoRegMedico").keypress(function (e) {
                if (e.which == 13) {
                    $("#imgBusquedaMedicamentosa").trigger("click");
                }
            });

            $("#imgAgregarRegMedico").click(function () {
                fn_GuardarReconciliacionMedicamentosa();
            });
        });

        function fn_oculta_popup_medicamentos() {
            var nFilas = 0;
            $("#divGridReconciliacionMedicamentosa").find(".JSBTABLA > tbody > tr").each(function () {
                if ($(this).find("td").length > 1) {
                    nFilas += 1;
                }
            });
            $.ajax({
                url: "PopUp/Medicamentos.aspx/GenerarPDF",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (dato1, datos2, dato3) {

                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "OK") {
                    fn_oculta_popup();
                    //$.JValidaCampoObligatorio("txtMedicamentoRegMedico;txtDosisRegMedico;txtViaRegMedico;txtFrecuenciaRegMedico;txtAntecedentesPatologicos;txtFechaUltima;txtHoraUltima");
                } else {
                    $.JMensajePOPUP("Mensaje del Sistema Clinica San Felipe - Error", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_mensaje()", "frmRegMedicoReconciliacion");
                }
            });

        }


        function fn_GuardarReconciliacionMedicamentosa() {
            var bValidacion = false;

            bValidacion = $.JValidaCampoObligatorio("txtMedicamentoRegMedico;txtDosisRegMedico;txtViaRegMedico;txtFrecuenciaRegMedico;txtAntecedentesPatologicos;txtFechaUltima;txtHoraUltima");
            if (bValidacion == false) {
                $.JMensajePOPUP("Mensaje del Sistema Clinica San Felipe - Aviso", "Ingrese los campos en rojo.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()", "frmMedicamentos");
                return false;
            }

            if ($("#hfCodigoAntecedentePatologico").val().trim() == "") {
                $("#txtAntecedentesPatologicos").val("");
                $.JValidaCampoObligatorio("txtAntecedentesPatologicos");
                $.JMensajePOPUP("Mensaje del Sistema Clinica San Felipe - Aviso", "Ingrese los campos en rojo.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()", "frmMedicamentos");
                return false;
            }

            var Accion;
            if ($("#rbContinuaRegMedico").prop("checked")) {
                Accion = 0;
            }
            if ($("#rbSuspendeRegMedico").prop("checked")) {
                Accion = 1;
            }
            if ($("#rbModificaRegMedico").prop("checked")) {
                Accion = 2;
            }
            var PortaMed = "";
            if ($("#chkPortaMedicamento").prop("checked")) {
                PortaMed = "S";
            } else {
                PortaMed = "N";
            }

            $.ajax({ url: "PopUp/Medicamentos.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    //INSERTA NUEVO REGISTRO
                    $.ajax({
                        url: "PopUp/Medicamentos.aspx/GuardarReconciliacionMedicamentosa",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            CodigoProducto: $("#hfCodigoMedicamentoRegMedico").val().trim(),
                            NombreProducto: $("#txtMedicamentoRegMedico").val().trim(),
                            Dosis: $("#txtDosisRegMedico").val().trim(),
                            Via: $("#txtViaRegMedico").val().trim(),
                            Frecuencia: $("#txtFrecuenciaRegMedico").val().trim(),
                            IdeExamenFisicoCore: $("#hfCodigoAntecedentePatologico").val().trim(),
                            CodigoAccion: Accion,
                            FechaUltima: $("#txtFechaUltima").val().trim(),
                            HoraUltima: $("#txtHoraUltima").val().trim(),
                            IdMedicamentosaDet: $("#IdMedicamentosaDet").val().trim(),
                            PortaMedicamento: PortaMed
                        }),
                        dataType: "json",
                        error: function (dato1, datos2, dato3) {

                        }
                    }).done(function (oOB_JSON) {
                        if (oOB_JSON.d.split(";")[0] == "ERROR") {
                            $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe - Error", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_mensaje()", "frmMedicamentos");
                            $.JValidaCampoObligatorio("txtMedicamentoRegMedico;txtDosisRegMedico;txtViaRegMedico;txtFrecuenciaRegMedico;txtAntecedentesPatologicos;txtFechaUltima;txtHoraUltima");
                        } else {
                            if (oOB_JSON.d.split(";")[0] == "VALIDACION") {
                                $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe - Validación", oOB_JSON.d, "VALIDACION", "Cerrar", "fn_oculta_mensaje()", "frmMedicamentos");
                            } else {
                                $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe", "Se guardaron los datos satisfactoriamente.", "OK", "Cerrar", "fn_oculta_mensaje()", "frmMedicamentos");
                                //$("#IdMedicamentosaDet").val(oOB_JSON.d.split(";")[1]);

                                $("#IdMedicamentosaDet").val("");
                                $("#hfCodigoMedicamentoRegMedico").val("");
                                $("#txtMedicamentoRegMedico").val("");
                                $("#txtDosisRegMedico").val("");
                                $("#txtViaRegMedico").val("");
                                $("#txtFrecuenciaRegMedico").val("");
                                /*$("#hfCodigoAntecedentePatologico").val("");
                                $("#txtAntecedentesPatologicos").val("");*/
                                $("#txtFechaUltima").val("");
                                $("#txtHoraUltima").val("");
                                $("#rbContinuaRegMedico").prop("checked", true);
                                $("#chkPortaMedicamento").prop("checked", false);

                                /*CARGA TABLA CON LOS NUEVOS REGISTROS AGREGADOS*/
                                $("#divGridReconciliacionMedicamentosa").load("GridViewAjax/GridReconciliacionMedicamentosa.aspx?Medicamentos=3&IdPatologia=" + $("#" + "<%=IdExamenFisicoCores.ClientID %>").val().trim(), function () {
                                    fn_CreaEventoGridReconciliacion();
                                });

                                fn_ImagenRadio(); //MOSTRAR LA IMAGEN VERDE AL COSTADO DEL RADIO BUTTON
                            }
                        }
                    });
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession_Medicamentos()", "frmMedicamentos");
                }
            });
        }

        //EVENTO AL SELECCIONAR UN REGISTRO
        function fn_CreaEventoGridReconciliacion() {
            $("#divGridReconciliacionMedicamentosa").find(".JSBTABLA tr td").css("cursor", "pointer");

            $("#divGridReconciliacionMedicamentosa").find(".JSBTABLA > tbody > tr > td").click(function () {
                var objeto = $(this);
                if (objeto.find(".JIMG-GENERAL").length == 1) { //SI HACE CLICK EN EL BOTON ELIMINAR CANCELAR EL EVENTO
                    return;
                }
                if (objeto.parent().prop("class").trim() == "JPAGINADO") { //SI HACE CLICK EN EL BOTON ELIMINAR CANCELAR EL EVENTO
                    return;
                }
                if ($(this).parent().find("td").length > 1) {
                    //CAPTURAR CODIGO
                    var IdMedicamentosaDet = 0;
                    IdMedicamentosaDet = objeto.parent().find("td").eq(0).html().trim();

                    $.ajax({ url: "PopUp/Medicamentos.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                        if (oOB_JSON.d == "") {
                            $.ajax({
                                url: "PopUp/Medicamentos.aspx/ObtenerMedicamentosaDet",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                data: JSON.stringify({
                                    IdMedicamentosaDet: IdMedicamentosaDet
                                }),
                                dataType: "json",
                                error: function (dato1, datos2, dato3) {

                                }
                            }).done(function (oOB_JSON) {
                                if (oOB_JSON.d != "") {
                                    //ide_medicamentosa_cab;ide_medicamentosa_det;cod_medico;medico;cod_producto;dsc_producto;dsc_via;num_dosis;num_frecuencia;fec_ultima;hor_ultima;cod_accion;dsc_accion;fec_modifica
                                    var aValores = oOB_JSON.d.split(";");
                                    $("#IdMedicamentosaDet").val(aValores[1]);
                                    $("#hfCodigoMedicamentoRegMedico").val(aValores[4]);
                                    $("#txtMedicamentoRegMedico").val(aValores[5]);
                                    $("#txtDosisRegMedico").val(aValores[7]);
                                    $("#txtViaRegMedico").val(aValores[6]);
                                    $("#txtFrecuenciaRegMedico").val(aValores[8]);

                                    $("#hfCodigoAntecedentePatologico").val(aValores[13]);
                                    $("#txtAntecedentesPatologicos").val(aValores[15]);

                                    $("#txtFechaUltima").val(aValores[9]);
                                    $("#txtHoraUltima").val(aValores[10]);

                                    if (aValores[11].trim() == "0") {
                                        $("#rbContinuaRegMedico").prop("checked", true);
                                    }
                                    if (aValores[11].trim() == "1") {
                                        $("#rbSuspendeRegMedico").prop("checked", true);
                                    }
                                    if (aValores[11].trim() == "2") {
                                        $("#rbModificaRegMedico").prop("checked", true);
                                    }
                                    if (aValores[16] == "S") {
                                        $("#chkPortaMedicamento").prop("checked", true);
                                    } else {
                                        $("#chkPortaMedicamento").prop("checked", false);
                                    }
                                    $.JValidaCampoObligatorio("txtMedicamentoRegMedico;txtDosisRegMedico;txtViaRegMedico;txtFrecuenciaRegMedico;txtAntecedentesPatologicos;txtFechaUltima;txtHoraUltima");
                                } else {
                                    $.JMensajePOPUP("Mensaje del Sistema Clinica San Felipe - Error", "No se encontro el registro seleccionado", "ERROR", "Cerrar", "fn_oculta_mensaje()", "frmMedicamentos");
                                }
                            });
                        } else {
                            aValores = oOB_JSON.d.toString().split(";");
                            $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession_Medicamentos()", "frmMedicamentos");
                        }
                    });

                }
            });
        }


        function fn_NuevoReconciliacionMedicamentosa() {
            $("#IdMedicamentosaDet").val("");
            $("#hfCodigoMedicamentoRegMedico").val("");
            $("#txtMedicamentoRegMedico").val("");
            $("#txtDosisRegMedico").val("");
            $("#txtViaRegMedico").val("");
            $("#txtFrecuenciaRegMedico").val("");

//            $("#hfCodigoAntecedentePatologico").val("");
//            $("#txtAntecedentesPatologicos").val(""); 
            $("#txtFechaUltima").val("");
            $("#txtHoraUltima").val("");


            $("#rbContinuaRegMedico").prop("checked", true);
            $.JValidaCampoObligatorio("txtMedicamentoRegMedico;txtDosisRegMedico;txtViaRegMedico;txtFrecuenciaRegMedico;txtAntecedentesPatologicos;txtFechaUltima;txtHoraUltima");
        }

        //EVENTO BUSQUEDA DE ANTECEDENTE PATOLOGICO AL SELECCIONAR
        function fn_CreaEventoBusquedaAntecedentePatologico() {
            $("#divAntecedentePatologicos").find(".JSBTABLA-1 tr td").click(function () {
                if ($(this).html().trim() != "") {
                    var objeto = $(this);

                    $("#txtAntecedentesPatologicos").val("");
                    $("#hfCodigoAntecedentePatologico").val("");
                    $("#hfCodigoAntecedentePatologico").val(objeto.parent().find("td").eq(0).html().trim());
                    $("#txtAntecedentesPatologicos").val(objeto.parent().find("td").eq(1).html().trim());

                    $("#divFONDO1").css("display", "none");
                    $("#divAntecedentePatologicos").css("display", "none");
                    $("#txtAntecedentesPatologicos").trigger("blur");
                }
            });
        }

        //EVENTO BUSQUEDA DE MEDICAMENTO AL SELECCIONAR
        function fn_CrearEventoMedicamentoReconciliacionMedicamentosa() {
            $("#divBusquedaMedicamentoRegistroMedicamentosa").find(".JSBTABLA-1 tr td").click(function () {
                if ($(this).html().trim() != "") {
                    var objeto = $(this);

                    $.ajax({ url: "PopUp/Medicamentos.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                        if (oOB_JSON.d == "") {
                            //VALIDAR SI ES ALERGICO AL PRODUCTO
                            $.ajax({
                                url: "PopUp/Medicamentos.aspx/ValidaAlergiaPaciente",
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
                                    $.JMensajePOPUP("Mensaje del Sistema Clinica San Felipe - Aviso", "El paciente presenta alergia a este producto.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()", "frmMedicamentos");
                                    $("#txtMedicamentoRegMedico").val("");
                                    $("#hfCodigoMedicamentoRegMedico").val("");
                                    $("#divFONDO1").css("display", "none");
                                    $("#divBusquedaMedicamentoRegistroMedicamentosa").css("display", "none");
                                } else {
                                    if (oOB_JSON.d != "") {
                                        $.JMensajePOPUP("Mensaje del Sistema Clinica San Felipe - Error", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_mensaje()", "frmMedicamentos");
                                    } else {
                                        $("#txtMedicamentoRegMedico").val("");
                                        $("#hfCodigoMedicamentoRegMedico").val("");
                                        $("#hfCodigoMedicamentoRegMedico").val(objeto.parent().find("td").eq(0).html().trim());
                                        $("#txtMedicamentoRegMedico").val(objeto.parent().find("td").eq(1).html().trim());

                                        $("#divFONDO1").css("display", "none");
                                        $("#divBusquedaMedicamentoRegistroMedicamentosa").css("display", "none");
                                    }
                                }
                            });
                        } else {
                            aValores = oOB_JSON.d.toString().split(";");
                            $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession_Medicamentos()", "frmMedicamentos");
                        }
                    });
                }
            });
        }

        function fn_ExpiraSession_Medicamentos() {
            window.location.href = aValores[1];
        }
    </script>
</head>
<body>
    <div id="divFONDO1" style="background-color: transparent; width: 100%; height: 100%;
        position: fixed; z-index:999; top:0;left:0;display:none;">
    </div>
    <form id="frmMedicamentos" runat="server">
        <input type="hidden" id="IdMedicamentosaDet" />
        <input type="hidden" id="IdExamenFisicoCores" runat="server" />
        <input type="hidden" id="DescripcionExamenFisicoCore" runat="server" />
        <div class="JFILA"> 
            <div class="JCELDA-12">
                <div class="DatosUsuario" id="divDatosUsuarioMedicamentos">
                                
                </div>
            </div>
        </div>
        <div class="JFILA">
            <div class="JCELDA-12">
                <div class="JDIV-CONTROLES JDIV-GRUPO" style="text-align:center;">
                    MEDICAMENTOS
                </div>
            </div>
        </div>
        <div class="JFILA">
            <div class="JCELDA-2">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA_2">Medicamento:</span>
                </div>
            </div>
            <div class="JCELDA-3" style="position:initial;overflow:initial;position:static;"> <%--YYY--%>
                <div class="JDIV-CONTROLES">
                    <input type="text" id="txtMedicamentoRegMedico" class="JTEXTO" />
                    <input type="hidden" id="hfCodigoMedicamentoRegMedico" />
                    <div id="divBusquedaMedicamentoRegistroMedicamentosa" class="JBUSQUEDA-ESPECIAL" style="max-height:150px;width:25%;">                                                                                 
                    </div><%--YYY--%>
                </div>
            </div>
            <div class="JCELDA-1">
                <div class="JDIV-CONTROLES">
                    <img src="../Imagenes/Buscar.png" id="imgBusquedaMedicamentosa" alt="" class="JIMG-BUSQUEDA" />
                </div>
            </div> <%--YYY--%>
            <div class="JCELDA-2">   <%--JESPACIO-IZQ-1 YYY--%>
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA_2">Dosis:</span>
                </div>
            </div>
            <div class="JCELDA-3">
                <div class="JDIV-CONTROLES">
                    <input type="text" id="txtDosisRegMedico" class="JTEXTO" />
                </div>
            </div>
        </div>
        <div class="JFILA">
            <div class="JCELDA-2">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA_2">Via:</span>
                </div>
            </div>
            <div class="JCELDA-3">
                <div class="JDIV-CONTROLES">
                    <input type="text" id="txtViaRegMedico" class="JTEXTO" />
                </div>
            </div>
            <div class="JCELDA-2 JESPACIO-IZQ-1">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA_2">Frecuencia:</span>
                </div>
            </div>
            <div class="JCELDA-3">
                <div class="JDIV-CONTROLES">
                    <input type="text" id="txtFrecuenciaRegMedico" class="JTEXTO" />
                </div>
            </div>
        </div>
        <div class="JFILA">
            <div class="JCELDA-2">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA_2">Antecedentes Patológicos</span>
                </div>
            </div>
            <div class="JCELDA-3" style="position:initial;">
                <div class="JDIV-CONTROLES">
                    <input type="text" id="txtAntecedentesPatologicos" class="JTEXTO" disabled="disabled" />
                    <input type="hidden" id="hfCodigoAntecedentePatologico"  />
                    <%--<div id="divAntecedentePatologicos" class="JBUSQUEDA-ESPECIAL" style="max-height:100px;width:25%;">
                        
                    </div>--%>
                </div>
            </div>
            <div class="JCELDA-1">
                <div class="JDIV-CONTROLES">
                    <%--<img src="../Imagenes/Buscar.png" id="imgBusquedaAntecedetesPatologicos" alt="" class="JIMG-BUSQUEDA" />--%>
                </div>
            </div>
            <div class="JCELDA-2">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA_2">Fecha Ultima:</span>
                </div>
            </div>
            <div class="JCELDA-1">
                <div class="JDIV-CONTROLES">
                    <input type="text" id="txtFechaUltima" class="JFECHA" />
                </div>
            </div>
            <div class="JCELDA-1">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA_2">Hora Ultima:</span>
                </div>
            </div>
            <div class="JCELDA-1">
                <div class="JDIV-CONTROLES">
                    <input type="text" id="txtHoraUltima" class="JHORA" />
                </div>
            </div>
        </div>
        <div class="JFILA">
            <%--<div class="JCELDA-2">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA_2">Hora Ultima:</span>
                </div>
            </div>
            <div class="JCELDA-3">
                <div class="JDIV-CONTROLES">
                    <input type="text" id="txtHoraUltima" class="JHORA" />
                </div>
            </div>--%>
        </div>
        <div class="JFILA">
            <div class="JCELDA-2">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA_2">Acción médica:</span>
                </div>
            </div>
            <div class="JCELDA-3">
                <div class="JDIV-CONTROLES">
                    
                        <div class="JFILA" style="border:1px solid gray;">
                        <div class="JCELDA-4">
                            <div class="JDIV-CONTROLES">
                                <input id="rbContinuaRegMedico" type="radio" name="rbTipoRegMedico" checked="checked" /><span class="JETIQUETA_2">Continua</span>
                            </div>
                        </div>
                        <div class="JCELDA-4">
                            <div class="JDIV-CONTROLES">
                                <input id="rbSuspendeRegMedico" type="radio" name="rbTipoRegMedico" /><span class="JETIQUETA_2">Suspende</span>
                            </div>
                        </div>
                        <div class="JCELDA-4">
                            <div class="JDIV-CONTROLES">
                                <input id="rbModificaRegMedico" type="radio" name="rbTipoRegMedico" /><span class="JETIQUETA_2">Modifica</span>
                            </div>
                        </div>
                        </div>
                    
                </div>
            </div>
            
            <div class="JCELDA-2 JESPACIO-IZQ-2">
                <div class="JDIV-CONTROLES">
                    <input type="checkbox" id="chkPortaMedicamento" /><span class="JETIQUETA_2">Paciente trae medicamento</span>
                </div>
            </div>
            <div class="JCELDA-1">
                <div class="JDIV-CONTROLES">
                    <img src="../Imagenes/Agregar.png" id="imgAgregarRegMedico" alt="" class="JIMG-BUSQUEDA" />
                </div>
            </div>            
        </div>
        <div class="JFILA">
            <div class="JCELDA-2">
                <div class="JDIV-CONTROLES">
                    <br />
                </div>
            </div>
        </div>
        <div class="JFILA">
            <div class="JCELDA-12">
                <div class="JDIV-CONTROLES">
                    <div id="divGridReconciliacionMedicamentosa">
                    
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
