<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Receta.aspx.vb" Inherits="WebHCE.Receta" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        var Habilitado = true;
        $(document).ready(function () {
            $(".JCONTENIDO-POPUP-1").find("footer").find("input[type='button']").eq(0).attr("name", "10/01/01");
            fn_CargaPermiso(); //PERMISO
            $(".JFECHA").datepicker({ dateFormat: 'dd/mm/yy' });
            fn_CampoBase(); //DA FORMATO A ALGUNOS CAMPOS
            $("#divGridReceta").load("GridViewAjax/GridReceta.aspx", { Pagina: "1" }, function () {

            });


            if ($("[id*=gvProductoMedicamentoRA]").find("tr").length == 2) {
                if ($("[id*=gvProductoMedicamentoRA]").find("tr").find("td").eq(0).text() == "\xa0" && $("[id*=gvProductoMedicamentoRA]").find("tr").find("td").eq(7).text() == "\xa0") {
                    fn_LimpiarGridProductoMedicamentoRA();
                }
                //fn_LimpiarGridProductoMedicamentoRA();
            }


            $("[id*=frmReceta]").find(".JCHEK-TABS").click(function () {
                $(this).parent().find("> .JCONTENIDO-TAB").css("display", "none");
                $(this).parent().find("> .JCONTENIDO-TAB").eq($(this).parent().find(".JCHEK-TABS").index(this)).css("display", "block");
            });
            $("[id*=frmReceta]").find(".JTABS").each(function () {
                $(this).find("> .JCHEK-TABS").eq(0).click();
            });

            //OBTENIENDO FECHA VIGENCIA
            $.ajax({
                url: "PopUp/Receta.aspx/ObtenerFechaVigencia",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d != "") {
                    $("#txtVigenciaRecenta").val(oOB_JSON.d);
                } else {

                }
            });



            $("#txtProximaCitaReceta").blur(function () {
                if ($(this).val().trim() != "") {
                    var fecha_actual = new Date();
                    var fecha1; //new Date($("#txtProximaCitaReceta").val().split("/")[2], $("#txtProximaCitaReceta").val().split("/")[1], $("#txtProximaCitaReceta").val().split("/")[0]);
                    fecha1 = Date.parse(($("#txtProximaCitaReceta").val().split("/")[2] + "-" + $("#txtProximaCitaReceta").val().split("/")[1] + "-" + $("#txtProximaCitaReceta").val().split("/")[0]));
                    //fecha_actual.setHours(0,0,0,0);

                    if (fecha1 < fecha_actual) {
                        $.JMensajePOPUP("Aviso", "La fecha de proxima cita debe ser mayor a la fecha actual", "", "Cerrar", "fn_oculta_mensaje()", "frmReceta");
                        $("#txtProximaCitaReceta").val("");
                    }
                }
            });


            $("#txtVigenciaRecenta").blur(function () {
                if ($(this).val().trim() != "") {
                    var fecha_actual = new Date();
                    var fecha1; //new Date($("#txtProximaCitaReceta").val().split("/")[2], $("#txtProximaCitaReceta").val().split("/")[1], $("#txtProximaCitaReceta").val().split("/")[0]);
                    fecha1 = Date.parse(($("#txtVigenciaRecenta").val().split("/")[2] + "-" + $("#txtVigenciaRecenta").val().split("/")[1] + "-" + $("#txtVigenciaRecenta").val().split("/")[0]));
                    //fecha_actual.setHours(0,0,0,0);

                    if (fecha1 < fecha_actual) {
                        $.JMensajePOPUP("Aviso", "La fecha de vigencia debe ser mayor a la fecha actual", "", "Cerrar", "fn_oculta_mensaje()", "frmReceta");
                        $("#txtVigenciaRecenta").val("");
                    }
                }
            });


            $("#txtProductoReceta").keypress(function (e) {
                if (e.which == 13) {
                    $("#imgBusquedaProductoReceta").trigger("click");
                }
            });

            $("#imgBusquedaProductoReceta").click(function () {
                if (Habilitado == false) {
                    return;
                }
                $.ajax({ url: "PopUp/Receta.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "") {
                        if ($("#txtProductoReceta").val().trim().length > 3) {
                            var NroORden = "";
                            if ($("#chkBuscarDciRecetaAlta").prop("checked")) {
                                NroORden = "-1"
                            } else {
                                NroORden = "-2"
                            }


                            $("#divFONDO1").css("display", "inline");
                            $("#divProductoReceta").css("display", "block");
                            $("#divProductoReceta").html("");
                            $("#divProductoReceta").load("Utilidad/BusquedaMedicamento.aspx", { Nombre: $("#txtProductoReceta").val().trim(), Orden: NroORden }, function () {
                                fn_CrearEventoReceta();
                            });
                        } else {
                            $.JMensajePOPUP("Aviso", "Debe ingresar al menos 4 caracteres para realizar una búsqueda.", "", "Cerrar", "fn_oculta_mensaje()", "frmReceta");
                        }

                    } else {
                        aValores = oOB_JSON.d.toString().split(";");
                        $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession_Receta()", "frmReceta");
                    }
                });
            });

            $("#divFONDO1").click(function () {
                $(".JBUSQUEDA-ESPECIAL").css("display", "none");
                $(this).css("display", "none");
            });


            $("#txtDiaReceta").blur(function () {
                if ($("#txtDiaReceta").val().trim() != "" && $("#txtCadaHoraReceta").val().trim() != "") {
                    var Cantidad = (24 / $("#txtCadaHoraReceta").val()) * $("#txtDiaReceta").val();
                    $("#txtCantidadReceta").val(Cantidad.toFixed(2));
                }
            });
            $("#txtCadaHoraReceta").blur(function () {
                if ($("#txtDiaReceta").val().trim() != "" && $("#txtCadaHoraReceta").val().trim() != "") {
                    var Cantidad = (24 / $("#txtCadaHoraReceta").val()) * $("#txtDiaReceta").val();
                    $("#txtCantidadReceta").val(Cantidad.toFixed(2));
                }
            });

            /************ CAMBIOS PEDIDOS EL 02/09/2016 *************/
            $("#imgAgregarReceta").click(function () {
                if (Habilitado == false) {
                    return;
                }
                fn_AgregarProductoRecetaF();
            });

            fn_EventoEliminarRecetaAltaF();


            /************************* PESTAÑA NO FARMACOLOGICO **************************/
            $("#imgAgregrarNoFarmacologicoRA").click(function () {
                fn_AgregarNoFarmacologicoRA();
            });


            /**************************PESTAÑA INFUSIONES *********************************/
            $("#imgAgregarInfusionRA").click(function () {
                fn_AgregarInfusionesRA();
            });
        });


        function fn_EventoEliminarRecetaAltaF() {
            $("[id*=gvProductoMedicamentoRA]").find(".JIMG-ELIMINAR").unbind("click");
            $("[id*=gvProductoMedicamentoRA]").find(".JIMG-ELIMINAR").click(function () {
                var nIndice = $(this).parent().parent().index();
                var objeto = $(this);
                $.ajax({
                    url: "PopUp/Receta.aspx/EliminarRecetaAltaF",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({
                        Indice: nIndice
                    })
                }).done(function (oOB_JSON) {
                    if ($("[id*=gvProductoMedicamentoRA]").find("tr").length == 2) {
                        var row = $("[id*=gvProductoMedicamentoRA] tr:last").clone();
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
                        $("td:nth-child(11)", row).html("");
                        row.css("display", "none");
                        $("[id*=gvProductoMedicamentoRA] tbody").append(row);
                    } else {
                        objeto.parent().parent().remove();
                    }
                });
            });
        }

        function fn_NuevoReceta() {
            //OBTENIENDO FECHA VIGENCIA
            $.ajax({ url: "PopUp/Receta.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    if ($("#hfIdReceta").val() != "") {
                        //$("#hfIdRecetaDet").val("");
                        $.JMensajePOPUP("Mensaje del Sistema de Clínica San Felipe - Advertencia", "No guardo la receta, ¿desea anular y crear una nueva receta?", "ADVERTENCIA", "Si;No", "fn_LimpiarReceta();fn_oculta_mensaje()", "frmReceta");
                    } else {
                        fn_LimpiarReceta();
                    }
                    /*
                    05/09/2016
                    fn_HabilitaControles("txtProximaCitaReceta;txtVigenciaRecenta;txtProductoReceta;txtCadaHoraReceta;txtDosisReceta;txtDiaReceta;txtIndicacionesReceta;chkRecetaAlta");
                    fn_HabilitaControlIMG("imgBusquedaProductoReceta");
                    Habilitado = true;
                    */
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession_Receta()", "frmReceta");
                }
            });
        }

        function fn_AgregarProductoRecetaF() {
            var ValorViaRecetaAlta = "";
            ValorViaRecetaAlta = $("#" + "<%=ddlViaRecetaAlta.ClientID %>" + " option:selected").text().trim() //$("#" + "<= ddlVia_Con.ClientID %>" + " option:selected").text().trim(); //JB - 27/04/2021 - $("#txtVia_ControlClinico").val().trim()
            var CodigoViaRecetaAlta = ""; //JB - 27/04/2021 - nuevo codigo
            CodigoViaRecetaAlta = $("#" + "<%=ddlViaRecetaAlta.ClientID %>").val().trim(); //JB - 27/04/2021 - nuevo codigo

            fn_LOAD_VISI();
            var ValorRecetaAlta = 0;
            ValorRecetaAlta = 1;
            $.ajax({ url: "PopUp/Receta.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $.ajax({
                        url: "PopUp/Receta.aspx/GuardarRecetaF",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            FechaProximaCita: $("#txtProximaCitaReceta").val().trim(),
                            RecetaAlta: ValorRecetaAlta,
                            CodigoProdcuto: $("#hfCodigoProductoReceta").val().trim(),
                            Cantidad: $("#txtCantidadReceta").val().trim(),
                            Frecuencia: $("#txtCadaHoraReceta").val().trim(),
                            Duracion: $("#txtDiaReceta").val().trim(),
                            Dosis: $("#txtDosisReceta").val().trim(),
                            Detalle: $("#txtIndicacionesReceta").val().trim(),
                            DscProducto: $("#txtProductoReceta").val().trim(),
                            FechaVigencia: $("#txtVigenciaRecenta").val().trim(),
                            UnidadMedida: "",
                            ViaReceta: ValorViaRecetaAlta
                        }),
                        dataType: "json",
                        error: function (dato1, datos2, dato3) {
                        }
                    }).done(function (oOB_JSON) {
                        if (oOB_JSON.d != "") {
                            if (oOB_JSON.d.split(";")[0] != "ERROR") {
                                var xmlDoc = $.parseXML(oOB_JSON.d);
                                var xml = $(xmlDoc);
                                var RecetaALtaF = xml.find("TablaRecetaAltaF");
                                var row = $("[id*=gvProductoMedicamentoRA] tr:last").clone();
                                $("[id*=gvProductoMedicamentoRA] tr:gt(0)").remove();
                                $(RecetaALtaF).each(function () {
                                    $("td:nth-child(1)", row).html($(this).find("Codigo").text());
                                    $("td:nth-child(2)", row).html($(this).find("Producto").text());
                                    $("td:nth-child(3)", row).html($(this).find("Via").text());
                                    $("td:nth-child(4)", row).html($(this).find("CadaHora").text());
                                    $("td:nth-child(5)", row).html($(this).find("Dia").text());
                                    $("td:nth-child(6)", row).html($(this).find("Dosis").text());
                                    $("td:nth-child(7)", row).html($(this).find("Cantidad").text());
                                    $("td:nth-child(8)", row).html($(this).find("Indicacion").text());
                                    $("td:nth-child(9)", row).html($(this).find("FechaProximaCita").text());
                                    $("td:nth-child(10)", row).html($(this).find("FechaVigencia").text());
                                    $("td:nth-child(11)", row).html($(this).find("UnidMedida").text());
                                    row.css("display", "table-row");
                                    $("[id*=gvProductoMedicamentoRA] tbody").append(row);
                                    row = $("[id*=gvProductoMedicamentoRA] tr:last").clone();
                                    fn_EventoEliminarRecetaAltaF();
                                });
                                fn_LOAD_OCUL();
                                $("#txtCadaHoraReceta").val("");
                                $("#txtProductoReceta").val("");
                                $("#txtDosisReceta").val("");
                                $("#txtDiaReceta").val("");
                                $("#txtIndicacionesReceta").val("");
                                $("#txtCantidadReceta").val("");
                                $("#hfCodigoProductoReceta").val("");
                                $("#" + "<%= ddlViaRecetaAlta.ClientID %>").val($("#" + "<%= ddlViaRecetaAlta.ClientID %>" + " option:first").val()); //$("#" + "<= ddlVia_Con.ClientID %>").val($("#" + "<= ddlVia_Con.ClientID %>" + " option:first").val()); JB - 14/07/2020 - COMENTADO   //JB - 27/04/2021 - $("#txtVia_ControlClinico").val("");
                            } else {
                                fn_LOAD_OCUL();
                                $.JMensajePOPUP("Aviso", oOB_JSON.d.split(";")[1], "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje_rapido()", "frmReceta");
                            }
                        }
                    });
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession_Receta()", "frmReceta");
                }
            });


            /*      
            if (Habilitado == false) {
            return;
            }
            Habilitado = false; //JB - 08/06/2020
            var ValorRecetaAlta = 0;
            if ($("#chkRecetaAlta").prop("checked")) {
            ValorRecetaAlta = 1
            }
            ValorRecetaAlta = 1; //JB - 16/01/2020 - YA QUE EL CHECKBOX ESTA OCULTO AHORA SE GUARDARA CON VALOR 1 POR DEFECTO...

            var bValidacionReceta;
            //bValidacionReceta = $.JValidaCampoObligatorio("txtProximaCitaReceta;txtVigenciaRecenta;txtProductoReceta;txtCadaHoraReceta;txtDosisReceta;txtIndicacionesReceta;txtDiaReceta");
            //31/10/2016 TMACASSI SE QUITA DE VALIDACIONtxtCadaHoraReceta;txtDosisReceta;txtDiaReceta
            bValidacionReceta = $.JValidaCampoObligatorio("txtVigenciaRecenta;txtIndicacionesReceta"); //"txtProximaCitaReceta;txtVigenciaRecenta;txtProductoReceta;txtIndicacionesReceta"

            if (bValidacionReceta == false) {
            Habilitado = true;
            return false;
            }

            if ($("#hfCodigoProductoReceta").val() == "") {
            //$.JMensajePOPUP("Mensaje del Sistema de Clínica San Felipe", "Seleccione un producto del buscador", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()", "frmReceta");
            //return false;
            }
            
            $.ajax({ url: "PopUp/Receta.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
            if (oOB_JSON.d == "") {
            $.ajax({
            url: "PopUp/Receta.aspx/GuardarReceta",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
            FechaProximaCita: $("#txtProximaCitaReceta").val().trim(),
            RecetaAlta: ValorRecetaAlta,
            CodigoProdcuto: $("#hfCodigoProductoReceta").val().trim(),
            Cantidad: $("#txtCantidadReceta").val().trim(),
            Frecuencia: $("#txtCadaHoraReceta").val().trim(),
            Duracion: $("#txtDiaReceta").val().trim(),
            Dosis: $("#txtDosisReceta").val().trim(),
            Detalle: $("#txtIndicacionesReceta").val().trim(),
            IdRecetaActual: $("#hfIdReceta").val().trim(),
            IdRecetaDetActual: $("#hfIdRecetaDet").val().trim(),
            DscProducto: $("#txtProductoReceta").val().trim(),
            FechaVigencia: $("#txtVigenciaRecenta").val().trim() //tmacassi 03/10/2016
            }),
            dataType: "json",
            error: function (dato1, datos2, dato3) {
            }
            }).done(function (oOB_JSON) {
            if (oOB_JSON.d.split(";")[0] == "OK") {
            $.JMensajePOPUP("Mensaje del Sistema de Clínica San Felipe", "Se guardaron los datos correctamente", "OK", "Cerrar", "fn_oculta_mensaje()", "frmReceta");
            fn_LOAD_GRID_VISI();
            $("#divGridReceta").load("GridViewAjax/GridReceta.aspx", { Pagina: "1", IdReceta: oOB_JSON.d.split(";")[1] }, function () {
            fn_LOAD_GRID_OCUL();
            fn_CentrarPopUp();
            //EVENTO CLICK EN LA FILA DEL GRID
            $("#divGridReceta").find(".JSBTABLA tr td:not(:last)").click(function () {
            if ($(this).parent().find("td").length > 1) {
            $("#txtIndicacionesReceta").val($(this).parent().find("td").eq(6).html().trim());
            $("#hfCodigoProductoReceta").val($(this).parent().find("td").eq(0).html().trim());
            $("#txtProductoReceta").val($(this).parent().find("td").eq(1).html().trim());
            $("#txtCadaHoraReceta").val($(this).parent().find("td").eq(2).html().trim());
            $("#txtDiaReceta").val($(this).parent().find("td").eq(3).html().trim());
            $("#txtDosisReceta").val($(this).parent().find("td").eq(4).html().trim());
            $("#hfIdRecetaDet").val($(this).parent().find("td").eq(7).html().trim());
            $("#txtDiaReceta").trigger("blur");
            }
            });
            });

            $("#hfIdReceta").val(oOB_JSON.d.split(";")[1]);
            //$("#hfIdRecetaDet").val(oOB_JSON.d.split(";")[2]);

            //deshabilitando controles
            //fn_DeshabilitaControles("txtProximaCitaReceta;txtVigenciaRecenta;txtProductoReceta;txtCadaHoraReceta;txtDosisReceta;txtDiaReceta;txtIndicacionesReceta;chkRecetaAlta");
            //fn_DeshabilitaControlIMG("imgBusquedaProductoReceta");
            $("#txtCadaHoraReceta").val("");
            $("#txtProductoReceta").val("");
            $("#txtDosisReceta").val("");
            $("#txtDiaReceta").val("");
            $("#txtIndicacionesReceta").val("");
            $("#txtCantidadReceta").val("");
            $("#hfCodigoProductoReceta").val("");

            Habilitado = true; //JB - 08/06/2020
            } else {
            if (oOB_JSON.d.split(";")[0] == "AVISO") {
            $.JMensajePOPUP("Mensaje del Sistema de Clínica San Felipe - Aviso", oOB_JSON.d.split(";")[1], "", "Cerrar", "fn_oculta_mensaje()", "frmReceta");
            $("#hfCodigoProductoReceta").val("");
            $("#txtProductoReceta").val("");
            } else {
            $.JMensajePOPUP("Mensaje del Sistema de Clínica San Felipe - Error", oOB_JSON.d.split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()", "frmReceta");
            }
            Habilitado = true;  //JB - 08/06/2020
            }
            });
            } else {
            aValores = oOB_JSON.d.toString().split(";");
            $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession_Receta()", "frmReceta");
            }
            });*/
        }


        function fn_AgregarNoFarmacologicoRA() {
            fn_LOAD_VISI();
            $.ajax({ url: "PopUp/Receta.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {

                    $.ajax({
                        url: "PopUp/Receta.aspx/GuardarRecetaN",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            NutricionNoFarmacologicoRA: $("#txtNutricionNoFarmacologicoRA").val().trim(),
                            TerapiaFisRehaNoFarmacologicoRA: $("#txtTerapiaFisRehaNoFarmacologicoRA").val().trim(),
                            CuidadosEnfermeriaNoFarmacologicoRA: $("#txtCuidadosEnfermeriaNoFarmacologicoRA").val().trim(),
                            OtrosNoFarmacologicoRA: $("#txtOtrosNoFarmacologicoRA").val().trim()
                        }),
                        dataType: "json",
                        error: function (dato1, datos2, dato3) {
                        }
                    }).done(function (oOB_JSON) {
                        if (oOB_JSON.d.split(";")[0] != "ERROR") {
                            fn_LOAD_OCUL();
                            var aValores = "";
                            aValores = $("#txtNutricionNoFarmacologicoRA").val().trim().toUpperCase() + ";" + $("#txtTerapiaFisRehaNoFarmacologicoRA").val().trim().toUpperCase() + ";" +
                                $("#txtCuidadosEnfermeriaNoFarmacologicoRA").val().trim().toUpperCase() + ";" + $("#txtOtrosNoFarmacologicoRA").val().trim().toUpperCase() + ";" +
                                "<img alt='' src='../Imagenes/anular.gif' class='JIMG-GENERAL JIMG-ELIMINAR' />";

                            fn_AgregarFila("gvNoFarmacologicoRA", aValores, "fn_EventoEliminarRecetaAltaN");

                            $("#txtNutricionNoFarmacologicoRA").val("");
                            $("#txtTerapiaFisRehaNoFarmacologicoRA").val("");
                            $("#txtCuidadosEnfermeriaNoFarmacologicoRA").val("");
                            $("#txtOtrosNoFarmacologicoRA").val("");

                            
                        } else {
                            fn_LOAD_OCUL();
                            $.JMensajePOPUP("Error", oOB_JSON.d.split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()", "frmReceta");
                        }
                    });
                } else {
                    fn_LOAD_OCUL();
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession_Receta()", "frmReceta");
                }
            });
        }

        function fn_AgregarInfusionesRA() {
            fn_LOAD_VISI();
            $.ajax({ url: "PopUp/Receta.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {

                    $.ajax({
                        url: "PopUp/Receta.aspx/GuardarRecetaI",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            InfusionControlClinicoRA: $("#txtInfusionControlClinicoRA").val().trim()
                        }),
                        dataType: "json",
                        error: function (dato1, datos2, dato3) {
                        }
                    }).done(function (oOB_JSON) {
                        if (oOB_JSON.d.split(";")[0] != "ERROR") {
                            fn_LOAD_OCUL();
                            var aValores = "";
                            aValores = ($("[id*=gvInfusionesRA]").find("tr:gt(0)").length + 1) + ";" + $("#txtInfusionControlClinicoRA").val().trim().toUpperCase() + ";" + "<img alt='' src='../Imagenes/anular.gif' class='JIMG-GENERAL JIMG-ELIMINAR' />";

                            fn_AgregarFila("gvInfusionesRA", aValores, "fn_EventoEliminarRecetaAltaI");
                            $("#txtInfusionControlClinicoRA").val("");
                        } else {
                            fn_LOAD_OCUL();
                            $.JMensajePOPUP("Error", oOB_JSON.d.split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()", "frmReceta");
                        }
                    });
                } else {
                    fn_LOAD_OCUL();
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession_Receta()", "frmReceta");
                }
            });
        }


        function fn_GuardarReceta() {
            fn_LOAD_VISI();
            $.ajax({ url: "PopUp/Receta.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {

                    $.ajax({
                        url: "PopUp/Receta.aspx/GuardarRecetaGeneral",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        error: function (dato1, datos2, dato3) {
                        }
                    }).done(function (oOB_JSON) {
                        if (oOB_JSON.d.split(";")[0] == "OK") {
                            fn_LOAD_OCUL();
                            fn_LimpiarGridProductoMedicamentoRA();
                            $.JMensajePOPUP("Mensaje del Sistema de Clínica San Felipe", "Se guardaron los datos correctamente", "OK", "Cerrar", "fn_oculta_mensaje()", "frmReceta");

                            //deshabilitando controles
                            $("#txtCadaHoraReceta").val("");
                            $("#txtProductoReceta").val("");
                            $("#txtDosisReceta").val("");
                            $("#txtDiaReceta").val("");
                            $("#txtIndicacionesReceta").val("");
                            $("#txtCantidadReceta").val("");
                            $("#hfCodigoProductoReceta").val("");
                            $("#txtProximaCitaReceta").val("");
                            $("#hfIdReceta").val("");
                            $("#hfIdRecetaDet").val("");

                            fn_LimpiarGridNoFarmacologicoRA();
                            fn_LimpiarGridInfusionesRA();
                        } else {
                            $.JMensajePOPUP("Mensaje del Sistema de Clínica San Felipe - Error", oOB_JSON.d.split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()", "frmReceta");
                        }
                    });
                } else {
                    fn_LOAD_OCUL();
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession_Receta()", "frmReceta");
                }
            });

            /*if (Habilitado == false) {
            return;
            }
            Habilitado = false; //JB - 08/06/2020
            //$(".JCONTENIDO-POPUP-2").find("input[type='button']").attr("disabled", "disabled");


            $.ajax({ url: "PopUp/Receta.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
            if (oOB_JSON.d == "") {
            if ($("#hfIdReceta").val().trim() == "") {
            return;
            }

            $.ajax({
            url: "PopUp/Receta.aspx/GuardarRecetaFinal",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
            IdRecetaActual: $("#hfIdReceta").val().trim()
            }),
            dataType: "json",
            error: function (dato1, datos2, dato3) {
            }
            }).done(function (oOB_JSON) {
            if (oOB_JSON.d.split(";")[0] == "OK") {
            $.JMensajePOPUP("Mensaje del Sistema de Clínica San Felipe", "Se guardaron los datos correctamente", "OK", "Cerrar", "fn_oculta_mensaje()", "frmReceta");
            fn_LOAD_GRID_VISI();
            $("#divGridReceta").load("GridViewAjax/GridReceta.aspx", { Pagina: "1", IdReceta: oOB_JSON.d.split(";")[1] }, function () {
            fn_LOAD_GRID_OCUL();
            fn_CentrarPopUp();
            //EVENTO CLICK EN LA FILA DEL GRID
            $("#divGridReceta").find(".JSBTABLA tr:not(:first)").click(function () {
            if ($(this).find("td").length > 1) {
            $("#txtIndicacionesReceta").val($(this).find("td").eq(6).html().trim());
            $("#hfCodigoProductoReceta").val($(this).find("td").eq(0).html().trim());
            $("#txtProductoReceta").val($(this).find("td").eq(1).html().trim());
            $("#txtCadaHoraReceta").val($(this).find("td").eq(2).html().trim());
            $("#txtDiaReceta").val($(this).find("td").eq(3).html().trim());
            $("#txtDosisReceta").val($(this).find("td").eq(4).html().trim());
            $("#hfIdRecetaDet").val($(this).find("td").eq(7).html().trim());
            $("#txtDiaReceta").trigger("blur");
            }
            });
            });

            //deshabilitando controles
            fn_DeshabilitaControles("txtProximaCitaReceta;txtVigenciaRecenta;txtProductoReceta;txtCadaHoraReceta;txtDosisReceta;txtDiaReceta;txtIndicacionesReceta;chkRecetaAlta");
            fn_DeshabilitaControlIMG("imgBusquedaProductoReceta;imgAgregarReceta");
            $("#txtCadaHoraReceta").val("");
            $("#txtProductoReceta").val("");
            $("#txtDosisReceta").val("");
            $("#txtDiaReceta").val("");
            $("#txtIndicacionesReceta").val("");
            $("#txtCantidadReceta").val("");
            $("#hfCodigoProductoReceta").val("");
            $("#txtProximaCitaReceta").val("");
            $("#hfIdReceta").val("");
            $("#hfIdRecetaDet").val("");

            Habilitado = false;
            } else {
            $.JMensajePOPUP("Mensaje del Sistema de Clínica San Felipe - Error", oOB_JSON.d.split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()", "frmReceta");
            }
            });
            } else {
            aValores = oOB_JSON.d.toString().split(";");
            $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession_Receta()", "frmReceta");
            }
            });*/
        }


        function fn_CrearEventoReceta() {
            $("#divProductoReceta").find(".JSBTABLA-1 tr td").click(function () {
                if ($(this).html().trim() != "") {
                    var objeto = $(this);

                    $.ajax({ url: "PopUp/Receta.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                        if (oOB_JSON.d == "") {
                            //VALIDAR SI ES ALERGICO AL PRODUCTO
                            $.ajax({
                                url: "PopUp/Receta.aspx/ValidaAlergiaPaciente",
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
                                    $("#txtProductoReceta").val("");
                                    $("#hfCodigoProductoReceta").val("");
                                    $("#divFONDO1").css("display", "none");
                                    $("#divProductoReceta").css("display", "none");
                                } else {
                                    if (oOB_JSON.d != "") {
                                        $.JMensajePOPUP("Mensaje del Sistema Clinica San Felipe - Error", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_mensaje()");
                                    } else {
                                        $("#txtProductoReceta").val("");
                                        $("#hfCodigoProductoReceta").val("");
                                        $("#hfCodigoProductoReceta").val(objeto.parent().find("td").eq(0).html().trim());
                                        $("#txtProductoReceta").val(objeto.parent().find("td").eq(1).html().trim());

                                        $("#divFONDO1").css("display", "none");
                                        $("#divProductoReceta").css("display", "none");
                                    }
                                }
                            });
                        } else {
                            aValores = oOB_JSON.d.toString().split(";");
                            $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession_Receta()", "frmReceta");
                        }
                    });
                }
            });
        }

        function fn_ExpiraSession_Receta() {
            window.location.href = aValores[1];
        }

        function fn_LimpiarReceta() {
            $.ajax({
                url: "PopUp/Receta.aspx/ObtenerFechaVigencia",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d.split(";").length > 1) {
                    $.JMensajePOPUP("Mensaje del Sistema de Clínica San Felipe - Error", oOB_JSON.d.split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()", "frmReceta");
                } else {
                    $("#txtVigenciaRecenta").val(oOB_JSON.d);
                }
            });
            $("#txtCadaHoraReceta").val("");
            $("#txtProductoReceta").val("");
            $("#txtDosisReceta").val("");
            $("#txtDiaReceta").val("");
            $("#txtIndicacionesReceta").val("");
            $("#txtCantidadReceta").val("");
            $("#hfCodigoProductoReceta").val("");
            $("#hfIdReceta").val("");
            $("#hfIdRecetaDet").val("");
            $("#txtProximaCitaReceta").val("");
            $("#divGridReceta").load("GridViewAjax/GridReceta.aspx", { Pagina: "1" }, function () {
            });
            fn_HabilitaControles("txtProximaCitaReceta;txtVigenciaRecenta;txtProductoReceta;txtCadaHoraReceta;txtDosisReceta;txtDiaReceta;txtIndicacionesReceta;chkRecetaAlta");
            fn_HabilitaControlIMG("imgBusquedaProductoReceta;imgAgregarReceta");
            Habilitado = true;
            fn_oculta_mensaje();
        }

        function fn_CerrarPopUp() {
            //            if ($("#hfIdReceta").val().trim() != "") {
            //                $.JMensajePOPUP("Mensaje del Sistema de Clínica San Felipe - Advertencia", "¿Desea anular la receta?", "ADVERTENCIA", "Si;No", "fn_AceptaSalirReceta();fn_oculta_mensaje()", "frmReceta");
            //            } else {
            //                fn_oculta_popup();
            //            }
            fn_LOAD_VISI();
            $.ajax({
                url: "PopUp/Receta.aspx/VerificaDataReceta",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (dato1, datos2, dato3) {
                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d.split(";")[0] == "OK") {
                    fn_LOAD_OCUL();
                    $.JMensajePOPUP("Mensaje del Sistema de Clínica San Felipe - Advertencia", "¿Desea anular la receta?", "ADVERTENCIA", "Si;No", "fn_AceptaSalirReceta();fn_oculta_mensaje()", "frmReceta");
                } else {
                    fn_LOAD_OCUL();
                    fn_oculta_popup();
                }
            });
            
        }

        function fn_AceptaSalirReceta() {
            $.ajax({
                url: "PopUp/Receta.aspx/AceptaSalirReceta",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    IdRecetaActual: $("#hfIdReceta").val().trim()
                }),
                dataType: "json",
                error: function (dato1, datos2, dato3) {
                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "OK") {
                    fn_oculta_mensaje_rapido();
                    fn_oculta_popup();
                } else {
                    $.JMensajePOPUP("Mensaje del Sistema de Clínica San Felipe - Error", oOB_JSON.d.split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()", "frmReceta");
                }
            });

        }


        function fn_LimpiarGridProductoMedicamentoRA() {
            var row = $("[id*=gvProductoMedicamentoRA] tr:last").clone();
            $("[id*=gvProductoMedicamentoRA] tr:gt(0)").remove();
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
            $("td:nth-child(11)", row).html("");
            row.css("display", "none");
            $("[id*=gvProductoMedicamentoRA] tbody").append(row);
        }


        function fn_EventoEliminarRecetaAltaN() {
            $("[id*=gvNoFarmacologicoRA]").find(".JIMG-ELIMINAR").unbind("click");
            $("[id*=gvNoFarmacologicoRA]").find(".JIMG-ELIMINAR").click(function () {
                fn_LOAD_VISI();
                var nIndice = $(this).parent().parent().index();
                var objeto = $(this);
                $.ajax({
                    url: "PopUp/Receta.aspx/EliminarRecetaAltaN",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({
                        Indice: nIndice
                    })
                }).done(function (oOB_JSON) {
                    objeto.parent().parent().remove();
                    fn_LOAD_OCUL();
                });
            });
        }

        function fn_EventoEliminarRecetaAltaI() {            
            $("[id*=gvInfusionesRA]").find(".JIMG-ELIMINAR").unbind("click");
            $("[id*=gvInfusionesRA]").find(".JIMG-ELIMINAR").click(function () {
                fn_LOAD_VISI();
                var nIndice = $(this).parent().parent().index();
                var objeto = $(this);
                $.ajax({
                    url: "PopUp/Receta.aspx/EliminarRecetaAltaI",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({
                        Indice: nIndice
                    })
                }).done(function (oOB_JSON) {
                    objeto.parent().parent().remove();
                    var cont = 0;
                    
                    $("[id*=gvInfusionesRA]").find("tr:gt(0)").each(function () {                        
                        var ofila = $(this);
                        if (ofila.find("th").length > 0) {
                        } else {
                            cont += 1;
                            ofila.find("td").eq(0).html(cont);
                        }
                    });
                    fn_LOAD_OCUL();
                });
            });
        }


        function fn_LimpiarGridNoFarmacologicoRA() {
            $("[id*=gvNoFarmacologicoRA]").find("tr:gt(0)").remove();
        }

        function fn_LimpiarGridInfusionesRA() {
            $("[id*=gvInfusionesRA]").find("tr:gt(0)").remove();
        }

        function fn_AgregarFila(IdGrid, aValores, funcion) {            
            var FilaHtml = "<tr>";
            for (var i = 0; i < aValores.split(";").length; i++) {
                FilaHtml += "<td>" + aValores.split(";")[i] + "</td>";
            }
            if (aValores.split(";").length < $("[id*=" + IdGrid + "]").find("tr:last").find("td").length) {
                var Inicio = aValores.split(";").length;
                var Fin = $("[id*=" + IdGrid + "]").find("tr:last").find("td").length;
                for (var i = Inicio; i < Fin; i++) {
                    FilaHtml += "<td>" + $("[id*=" + IdGrid + "]").find("tr:last").find("td").eq(i).html() + "</td>";
                }
            }
            FilaHtml += "</tr>";

            $("[id*=" + IdGrid + "]").find("tr:last").after(FilaHtml);
            $("[id*=" + IdGrid + "]").find("tr:last").find(".JIMG-ELIMINAR").parent().css("text-align", "center");

            eval("" + funcion + "()");
        }

        //Cmendez 16/05/2022
        $(".JTEXTO").keypress(function (e) {
            var ValidaTilde = /[|<>'&]/;
            if (ValidaTilde.test(String.fromCharCode(event.which))) {
                event.preventDefault();
            }
        });

        $(".JTEXTO").blur(function () {
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

        $(".JNUMERO").blur(function () {
            var ValidacionNumerica = /[0-9]/;
            for (var i = 0; i < this.value.length; i++) {
                if (!ValidacionNumerica.test($(this).val())) {
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

        //Fin
    </script>
</head>
<body>
    <div id="divFONDO1" style="background-color: transparent; width: 100%; height: 100%;
        position: fixed; z-index:999; top:0;left:0;display:none;">
    </div>
    <form id="frmReceta" runat="server" class="JFORM-CONTENEDOR-POPUP">
        <div class="JFILA">
            <div class="JCELDA-12">
                <div id="DatosUsuarioReceta" class="DatosUsuario">
                    
                </div>
            </div>
        </div>
        <div class="JFILA">
            <div class="JCELDA-6">
               <%-- <div class="JFILA">
                    <div class="JCELDA-2">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2">Paciente</span>
                        </div>
                    </div>
                    <div class="JCELDA-9">
                        <div class="JDIV-CONTROLES">
                            <input type="text" class="JTEXTO" id="txtPacienteReceta" disabled="disabled" />
                        </div>
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-2">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2">Médico</span>
                        </div>
                    </div>
                    <div class="JCELDA-9">
                        <div class="JDIV-CONTROLES">
                            <input type="text" class="JTEXTO" id="txtMedicoReceta"  />
                        </div>
                    </div>
                </div>--%>
                <div class="JFILA">
                    <div class="JCELDA-2">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2">Diagnósticos</span>
                        </div>
                    </div>
                    <div class="JCELDA-10">
                        <div class="JDIV-CONTROLES">
                            <%--<textarea rows="3" cols="1" id="txtDiagnosticoReceta" class="JTEXTO" disabled="disabled"></textarea>--%>
                            <div runat="server" id="divDiagnosticoReceta" style="width:100%;height:100%;border:1px solid #4BACFF;min-height:50px;" class="JETIQUETA_2" >
                                
                            </div>
                        </div>
                    </div>
                </div>
                <%--<div class="JFILA">
                    <div class="JCELDA-2">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2">Alergias</span>
                        </div>
                    </div>
                    <div class="JCELDA-9">
                        <div class="JDIV-CONTROLES">
                            <textarea rows="3" cols="1" id="txtAlergiasReceta" class="JTEXTO"></textarea>
                        </div>
                    </div>
                </div>--%>
                <div class="JFILA">
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <input type="checkbox" id="chkRecetaAlta" checked="checked" style="display:none;" />
                            <%--<span class="JETIQUETA_2">Receta de Alta</span>--%>
                        </div>
                    </div>
                    <div class="JCELDA-2">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2">Proxima Cita</span>
                        </div>
                    </div>
                    <div class="JCELDA-2">
                        <div class="JDIV-CONTROLES">
                            <input type="text" class="JFECHA" id="txtProximaCitaReceta" readonly="readonly" />
                        </div>
                    </div>
                    <div class="JCELDA-2 JESPACIO-IZQ-1">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2">Vigencia</span>
                        </div>
                    </div>
                    <div class="JCELDA-2">
                        <div class="JDIV-CONTROLES">
                            <input type="text" class="JFECHA" id="txtVigenciaRecenta" readonly="readonly" />
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
                
            </div>

            <div class="JCELDA-6">
                <div class="JFILA">
                    <div class="JCELDA-2">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2">Alergias</span>
                        </div>
                    </div>
                    <div class="JCELDA-10">
                        <div class="JDIV-CONTROLES">
                            <%--<textarea rows="3" cols="1" id="txtAlergiasReceta" class="JTEXTO" disabled="disabled"></textarea>--%>
                            <div runat="server" id="divAlergiaReceta" style="width:100%;height:100%;border:1px solid #4BACFF;min-height:50px;" class="JETIQUETA_2">
                                
                            </div>
                        </div>
                    </div>
                </div>
                <%--<div class="JFILA">
                    <div class="JCELDA-12">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2">Recomendaciones</span>
                        </div>
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-12">
                        <div class="JDIV-CONTROLES">
                            <asp:DropDownList runat="server" ID="ddlRecomendaciones" CssClass="JSELECT"></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-12">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2">Otras Recomendaciones</span>
                        </div>
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-12">
                        <div class="JDIV-CONTROLES">
                            <textarea rows="3" cols="1" id="txtOtraRecomendionReceta" class="JTEXTO"></textarea>
                        </div>
                    </div>
                </div>--%>
                <div class="JFILA">
                    <div class="JCELDA-12">
                        <div class="JDIV-CONTROLES">   
                            <br /><br />
                        </div>
                    </div>
                </div>
                
            </div>
        </div>
        <div class="JFILA">
            <div class="JCELDA-12">
                <div class="JTABS" style="width:100%;">
                    <input type="radio" id="TabPrincipalT1_RecetaAlta" name="TabNro3" class="JCHEK-TABS" />
                    <label for="TabPrincipalT1_RecetaAlta" class="JTABS-LABEL">Farmacologico</label>
                    <input type="radio" id="TabPrincipalT2_RecetaAlta" name="TabNro3" class="JCHEK-TABS" />        
                    <label for="TabPrincipalT2_RecetaAlta" class="JTABS-LABEL">No Farmacologico</label>
                    <%--<input type="radio" id="TabPrincipalT3_RecetaAlta" name="TabNro3" class="JCHEK-TABS" />        
                    <label for="TabPrincipalT3_RecetaAlta" class="JTABS-LABEL">Infusiones</label> JB - SE ELIMINA TAB INFUSIONES - 13/05/2017--%>
                    <div class="JCONTENIDO-TAB">
                        <div class="JFILA">
                        <div class="JCELDA-6">
                            <div class="JFILA">
                                <div class="JCELDA-2">
                                    <div class="JDIV-CONTROLES">
                                        <span class="JETIQUETA_2">Producto</span>
                                    </div>
                                </div>
                                <div class="JCELDA-9" style="position:initial;overflow:initial;position:static;">
                                    <div class="JDIV-CONTROLES">
                                        <input type="hidden" id="hfCodigoProductoReceta" />
                                        <input type="text" class="JTEXTO" id="txtProductoReceta" autocomplete="off" />
                                        <div id="divProductoReceta" class="JBUSQUEDA-ESPECIAL" style="max-height:130px;width:75%;">
                                
                                        </div>
                                    </div>
                                </div>
                                <div class="JCELDA-1">
                                    <div class="JDIV-CONTROLES">
                                        <img src="../Imagenes/Buscar.png" id="imgBusquedaProductoReceta" alt="" class="JIMG-BUSQUEDA" />
                                    </div>
                                </div>
                            </div>
                            <div class="JFILA">
                                <div class="JCELDA-2">
                                    <div class="JDIV-CONTROLES">
                                        <span class="JETIQUETA_2">DCI</span>
                                    </div>
                                </div>
                                <div class="JCELDA-10">
                                    <div class="JDIV-CONTROLES">
                                        <%--<input type="text" class="JTEXTO-11" id="txtDciReceta" disabled="disabled" />--%>
                                        <input type="checkbox" id="chkBuscarDciRecetaAlta" checked="checked" />
                                        <label for="chkBuscarDciRecetaAlta" class="JETIQUETA">Buscar por DCI</label>
                                    </div>
                                </div>
                            </div>
                            <div class="JFILA">
                                <%--<div class="JCELDA-2" style="position:initial;">
                                    <div class="JDIV-CONTROLES">
                                        <span class="JETIQUETA_2">Uni. Medida:</span>
                                    </div>
                                </div>
                                <div class="JCELDA-2" style="position:initial;">
                                    <div class="JDIV-CONTROLES">
                                        <input type="text" id="txtUniMedidaReceta" class="JTEXTO" disabled="disabled" />
                                    </div>
                                </div>JB - SE ELIMINA ESTE CAMPO - 13/05/2017--%>
                                <div class="JCELDA-2" style="position:initial;">
                                    <div class="JDIV-CONTROLES">
                                        <span class="JETIQUETA_2">Dosis:</span>
                                    </div>
                                </div>
                                <div class="JCELDA-3" style="position:initial;">
                                    <div class="JDIV-CONTROLES">
                                        <input type="text" id="txtDosisReceta" class="JNUMERO" />
                                    </div>
                                </div>
                                <div class="JCELDA-2 JESPACIO-IZQ-1" style="position:initial;">
                                    <div class="JDIV-CONTROLES">
                                        <span class="JETIQUETA_2">Via:</span>
                                    </div>
                                </div>
                                <div class="JCELDA-3" style="position:initial;">
                                    <div class="JDIV-CONTROLES">
                                        <%--<input type="text" id="txtViaReceta" class="JTEXTO" disabled="disabled" />--%>
                                        <asp:DropDownList ID="ddlViaRecetaAlta" runat="server" CssClass="JTEXTO" >
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="JCELDA-1">
                                    <div class="JDIV-CONTROLES">
                                        <img src="../Imagenes/Agregar.png" id="imgAgregarReceta" alt="" class="JIMG-BUSQUEDA" style="float:right;" />
                                    </div>
                                </div>
                            </div>
                            <div class="JFILA">
                                <div class="JCELDA-2" style="position:initial;">
                                    <div class="JDIV-CONTROLES">
                                        <span class="JETIQUETA_2">Cada (Hrs):</span>
                                    </div>
                                </div>
                                <div class="JCELDA-3" style="position:initial;">
                                    <div class="JDIV-CONTROLES">
                                        <input type="text" id="txtCadaHoraReceta" class="JNUMERO"  />
                                    </div>
                                </div>  

                                
                                <div class="JCELDA-2 JESPACIO-IZQ-1" style="position:initial;">
                                    <div class="JDIV-CONTROLES">
                                        <span class="JETIQUETA_2">Tratamiento (Días):</span>
                                    </div>
                                </div>
                                <div class="JCELDA-3" style="position:initial;">
                                    <div class="JDIV-CONTROLES">
                                        <input type="text" id="txtDiaReceta" class="JNUMERO"  />
                                    </div>
                                </div>  
                            </div>
                            <div class="JFILA">
                                
                                <div class="JCELDA-2" style="position:initial;">
                                    <div class="JDIV-CONTROLES">
                                        <span class="JETIQUETA_2">Cantidad:</span>
                                    </div>
                                </div>
                                <div class="JCELDA-3" style="position:initial;">
                                    <div class="JDIV-CONTROLES">
                                        <input type="text" id="txtCantidadReceta" class="JNUMERO" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="JCELDA-6">
                            <div class="JFILA">
                                <div class="JCELDA-2">
                                    <div class="JDIV-CONTROLES">
                                        <span class="JETIQUETA_2">Indicaciones</span>
                                    </div>
                                </div>
                                <div class="JCELDA-10">
                                    <div class="JDIV-CONTROLES">
                                        <%--Cmendez 25/05/2022 maxlength="5000"--%> 
                                        <textarea rows="3" cols="1" id="txtIndicacionesReceta" class="JTEXTO" maxlength="5000"></textarea>
                                    </div>
                                </div>
                            </div>
                        </div>
                        </div>
                        <div class="JFILA">
                            <div class="JCELDA-12">
                                <div class="JDIV-CONTROLES">
                                    <%--<div id="divGridReceta">                    
                                    </div>--%>
                                    <asp:GridView ID="gvProductoMedicamentoRA" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" CssClass="JSBTABLA" 
                                        GridLines="None" emptydatatext="No hay Productos agregados." >
                                        <Columns>
                                            <asp:BoundField DataField="Codigo" HeaderText="Codigo" >
                                                <HeaderStyle  CssClass="JCOL-OCULTA" />
                                                <ItemStyle  CssClass="JCOL-OCULTA Codigo" />
                                            </asp:BoundField>

                                            <asp:BoundField DataField="Producto" HeaderText="Producto" >
                                                <ItemStyle  CssClass="Producto" Width="20%"  />
                                            </asp:BoundField>                                            
                                            <asp:BoundField DataField="Via" HeaderText="Via" >
                                                <HeaderStyle CssClass="JCOL-OCULTA" />
                                                <ItemStyle  CssClass="JCOL-OCULTA Via" Width="15%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CadaHora" HeaderText="Cada (hrs)" >
                                                <ItemStyle  CssClass="Cada (hrs)" Width="10%"  />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Dia" HeaderText="Tratamiento">                                                
                                                <ItemStyle CssClass="Dia" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Dosis" HeaderText="Dosis" >
                                                <ItemStyle  CssClass="Dosis" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" >                                                
                                                <ItemStyle  CssClass="Cantidad" Width="10%"  />
                                            </asp:BoundField>                                            
                                            <asp:BoundField DataField="Indicacion" HeaderText="Indicacion">                                                
                                                <ItemStyle CssClass="Indicacion" />
                                            </asp:BoundField>
                                            
                                            <asp:BoundField DataField="FechaProximaCita" HeaderText="FechaProximaCita" >
                                                <HeaderStyle CssClass="JCOL-OCULTA" />
                                                <ItemStyle  CssClass="JCOL-OCULTA FechaProximaCita" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FechaVigencia" HeaderText="FechaVigencia" >
                                                <HeaderStyle CssClass="JCOL-OCULTA" />
                                                <ItemStyle  CssClass="JCOL-OCULTA FechaVigencia" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UnidadMedida" HeaderText="Unid. Medida" >
                                                <HeaderStyle CssClass="JCOL-OCULTA" />
                                                <ItemStyle  CssClass="JCOL-OCULTA Unid. Medida" Width="10%" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>                            
                                                    <img alt="" src="../Imagenes/anular.gif" class="JIMG-GENERAL JIMG-ELIMINAR" />                                    
                                                </ItemTemplate>                                                                
                                                <ItemStyle CssClass="Eliminar" Width="5%" />
                                            </asp:TemplateField>                                            
                                        </Columns>
                                        <PagerStyle CssClass="JPAGINADO" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                        
                    <div class="JCONTENIDO-TAB">
                        <div class="JFILA">
                            <div class="JCELDA-2">
                                <span class="JETIQUETA_2">Nutrición:</span>
                            </div>
                            <div class="JCELDA-9">
                                <%--Cmendez 25/05/2022 maxlength="5000"--%> 
                                <input type="text" id="txtNutricionNoFarmacologicoRA" class="JTEXTO" maxlength="5000"/>
                            </div>
                        </div>
                        <div class="JFILA">
                            <div class="JCELDA-2">
                                <span class="JETIQUETA_2">Terapia Física y Rehabilitación:</span>
                            </div>
                            <div class="JCELDA-9">
                                <%--Cmendez 25/05/2022 maxlength="5000"--%> 
                                <input type="text" id="txtTerapiaFisRehaNoFarmacologicoRA" class="JTEXTO" maxlength="5000" />
                            </div>
                        </div>
                        <div class="JFILA">
                            <div class="JCELDA-2">
                                <span class="JETIQUETA_2">Cuidados de enfermeria:</span>
                            </div>
                            <div class="JCELDA-9">
                                <%--Cmendez 25/05/2022 maxlength="5000"--%> 
                                <input type="text" id="txtCuidadosEnfermeriaNoFarmacologicoRA" class="JTEXTO" maxlength="5000" />
                            </div>
                        </div>
                        <div class="JFILA">
                            <div class="JCELDA-2">
                                <span class="JETIQUETA_2">Otros:</span>
                            </div>
                            <div class="JCELDA-9">
                                <%--Cmendez 25/05/2022 maxlength="5000"--%> 
                                <input type="text" id="txtOtrosNoFarmacologicoRA" class="JTEXTO" maxlength="5000" />
                            </div>
                            <div class="JCELDA-1">
                                <div class="JDIV-CONTROLES">
                                    <img src="../Imagenes/Agregar.png" id="imgAgregrarNoFarmacologicoRA" alt="" class="JIMG-BUSQUEDA" style="float:left;" />
                                </div>
                            </div>
                        </div>                        
                        <div class="JFILA">
                            <div class="JCELDA-12">
                                <table id="gvNoFarmacologicoRA" class="JSBTABLA">
                                    <tr>
                                        <th>Nutrición</th>
                                        <th>Terapia Física y Rehabilitacion</th>
                                        <th>Cuidados de Enfermeria</th>
                                        <th>Otros</th>
                                        <th>Eliminar</th>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>

                    <div class="JCONTENIDO-TAB" style="display:none;"> <%--JB - SE ELIMINA TAB INFUSIONES - 13/05/2017--%>
                        <div class="JFILA">
                            <div class="JCELDA-11" >
                                <div class="JDIV-CONTROLES">
                                    <input type="text" id="txtInfusionControlClinicoRA" class="JTEXTO" />
                                </div>
                            </div>
                            <div class="JCELDA-1" style="position:initial;">
                                <div class="JDIV-CONTROLES">
                                    <input type="button" id="imgAgregarInfusionRA" class="JBOTON-IMAGEN" style="background-image:url(../Imagenes/Agregar.png);" />                                     
                                </div>
                            </div>
                        </div>
                        <div class="JFILA">
                            <div class="JCELDA-12">
                                <table id="gvInfusionesRA" class="JSBTABLA">
                                    <tr>
                                        <th>Item</th>
                                        <th>Descripción</th>
                                        <th>Eliminar</th>
                                    </tr>
                                </table>
                                <%--<asp:GridView ID="gvListadoInfusionesRA" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" CssClass="JSBTABLA" GridLines="None" 
                                    AllowPaging="True" PageSize="15" PagerStyle-CssClass="JPAGINADO">
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
                                </asp:GridView>--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <input type="hidden" id="hfIdReceta" />
        <input type="hidden" id="hfIdRecetaDet" />
    </form>
</body>
</html>
