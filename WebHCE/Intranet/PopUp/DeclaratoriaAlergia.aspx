<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DeclaratoriaAlergia.aspx.vb" Inherits="WebHCE.DeclaratoriaAlergia" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        var Editar = true;

        $(document).ready(function () {
            fn_CampoBase();
            $(".JCONTENIDO-POPUP-1").find("footer").find("input[type='button']").eq(0).attr("name", "09/01/01");
            $(".JCONTENIDO-POPUP-1").find("footer").find("input[type='button']").eq(1).attr("name", "09/01/02");
            fn_CargaPermiso(); //PERMISO
            //            $("#frmDeclaratoriaAlergia").find(".DatosUsuario").load("Utilidad/DatosUsuario.aspx", function () {
            //                $("#frmDeclaratoriaAlergia").find("#spDatosDNIPaciente").css("display", "inline");
            //                $("#frmDeclaratoriaAlergia").find("#spSpanDNI").css("display", "inline");
            //            });

            $("#divGridDeclaratoriaAlergia").load("GridViewAjax/GridDeclaratoriaAlergia.aspx", { Pagina: "1" }, function () {
                //$("#divGridDeclaratoriaAlergia").find(".JIMG-ELIMINAR").unbind("click"); 27/10/2016
            });

            $("#frmDeclaratoriaAlergia").parent().parent().find("footer").find("input[type='button']").eq(1).removeAttr("disabled"); //JB - 12/05/2021            

            //CARGANDO DATOS
            $.ajax({
                url: "PopUp/DeclaratoriaAlergia.aspx/CargarDatos",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (dato1, datos2, dato3) {
                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d != "") {
                    var aValores = "";
                    aValores = oOB_JSON.d.toString().split(";");
                    if (aValores[0] == "True") {
                        $("#rbSI_Dec").prop("checked", true);
                        //$("#rbSI_Dec").trigger("click");
                    } else {
                        $("#rbNo_Dec").prop("checked", true);
                        fn_DeshabilitaCtrlAlergia();
                        //$("#rbNo_Dec").trigger("click");
                    }
                    if (aValores[1] == "True") {
                        $("#chkNombreRepresentado_Dec").prop("checked", true);
                    } else {
                        $("#chkNombreRepresentado_Dec").prop("checked", false);
                    }
                    $("#txtNroDni").val(aValores[2]);
                    $("#txtNombreRepresentado_Dec").val(aValores[3]);
                    $("#txtAlimento_Dec").val(aValores[4]);
                    $("#txtOtros_Dec").val(aValores[5]);
                    if ($("#chkNombreRepresentado_Dec").prop("checked")) {
                        $.JValidaCampoObligatorio("txtNroDni;txtNombreRepresentado_Dec");
                        $("#txtNroDni").removeAttr("disabled");
                        $("#txtNombreRepresentado_Dec").removeAttr("disabled");
                    }
                    //DESHABILITANDO CONTROLES Y EL BOTON ELIMINAR DEL LISTADO
                    /* 
                    fn_DeshabilitaControles("rbNo_Dec;rbSI_Dec;txtPrincipioActivo;txtAlimento_Dec;txtOtros_Dec;chkNombreRepresentado_Dec;txtNroDni;txtNombreRepresentado_Dec");
                    $("#frmDeclaratoriaAlergia").find(".JIMG-BUSQUEDA").css("opacity", "0.6");
                    Editar = false;
                    */
                    //$("#rbSI_Dec").trigger("click");
                } else {

                }
            });


            $("#divFONDO1").click(function () {
                $(".JBUSQUEDA-ESPECIAL").css("display", "none");
                $(this).css("display", "none");
            });

            $("#rbSI_Dec").click(function () {
                if ($(this).prop("checked")) {
                    fn_HabilitaControles("txtPrincipioActivo;txtAlimento_Dec;txtOtros_Dec;chkNombreRepresentado_Dec");
                    $("#frmDeclaratoriaAlergia").find(".JIMG-BUSQUEDA").css("opacity", "1");
                    Editar = true;
                    if ($("#chkNombreRepresentado_Dec").prop("checked")) {
                        $.JValidaCampoObligatorio("txtNroDni;txtNombreRepresentado_Dec");
                        $("#txtNroDni").removeAttr("disabled");
                        $("#txtNombreRepresentado_Dec").removeAttr("disabled");
                    }
                }
            });
            $("#rbNo_Dec").click(function () {
                if ($(this).prop("checked")) {
                    //Esta opción eliminará los datos ingresados, ¿Desea continuar?  
                    //txtPrincipioActivo  txtAlimento_Dec  txtOtros_Dec  txtNombreRepresentado_Dec  txtNroDni
                    if ($("#txtPrincipioActivo").val().trim() != "" || $("#txtAlimento_Dec").val().trim() != "" || $("#txtOtros_Dec").val().trim() != "" || $("#txtNombreRepresentado_Dec").val().trim() != "" || $("#txtNroDni").val().trim() != "") {
                        $.JMensajePOPUP("Advertencia", "Esta opción eliminará los datos ingresados, ¿Desea continuar?", "ADVERTENCIA", "Si;No", "fn_DeshabilitaCtrlAlergia();fn_CancelarNo()", "frmDeclaratoriaAlergia");
                    } else {
                        fn_DeshabilitaCtrlAlergia();
                    }
                }
            });

            $("#chkNombreRepresentado_Dec").click(function () {
                if ($(this).prop("checked")) {
                    $.JValidaCampoObligatorio("txtNroDni;txtNombreRepresentado_Dec");
                    $("#txtNroDni").removeAttr("disabled");
                    $("#txtNombreRepresentado_Dec").removeAttr("disabled");
                } else {
                    $("#txtNroDni").prop("disabled", "disabled");
                    $("#txtNroDni").val("");
                    $("#txtNombreRepresentado_Dec").prop("disabled", "disabled");
                    $("#txtNombreRepresentado_Dec").val("");
                }
            });
            $("#txtPrincipioActivo").keypress(function (e) {
                if (e.which == 13) {
                    $("#imgBuscarPrincipioActivo").trigger("click");
                }
            });

            //FUNCION PARA BUSCAR UN PRINCIPIO ACTIVO
            $("#imgBuscarPrincipioActivo").click(function () {
                if (Editar == false) {
                    return;
                }
                $.ajax({ url: "PopUp/DeclaratoriaAlergia.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "") {
                        if ($("#rbSI_Dec").prop("checked")) {
                            $("#divFONDO1").css("display", "inline");
                            $("#divPrincipioActivo").css("display", "block");
                            $("#divPrincipioActivo").load("Utilidad/BusquedaPrincipioActivo.aspx", { Buscar: $("#txtPrincipioActivo").val().trim() }, function () {
                                fn_CreaEventoBusquedaPrincipioActivo();
                            });
                            /*if ($("#txtPrincipioActivo").val().trim().length > 3) {
                            $("#divPrincipioActivo").load("Utilidad/BusquedaPrincipioActivo.aspx", { Buscar: $("#txtPrincipioActivo").val().trim() }, function () {
                            });
                            } else {
                            $.JMensajePOPUP("Aviso", "Debe ingresar al menos 4 caracteres para realizar una búsqueda.", "", "Cerrar", "fn_oculta_mensaje()", "frmDeclaratoriaAlergia");
                            }*/
                        }
                    } else {
                        aValores = oOB_JSON.d.toString().split(";");
                        $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession_DeclatoriaAlergia()", "frmDeclaratoriaAlergia");
                    }
                });
            });

            //FUNCION PARA AÑADIR UN PRINCIPIO ACTIVO
            $("#imgAgregarPrincipioActivo").click(function () {
                if (Editar == false) {
                    return;
                }
                $.ajax({ url: "PopUp/DeclaratoriaAlergia.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "") {
                        if ($("#hfCodigoPrincipioActivo").val().trim() == "") {
                            return;
                        }
                        $.ajax({
                            url: "PopUp/DeclaratoriaAlergia.aspx/ValidaPrincipioActivoAgregado",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({
                                CodigoPrincipioActivo: $("#hfCodigoPrincipioActivo").val().trim()
                            }),
                            dataType: "json"
                        }).done(function (oOB_JSON) {
                            if (oOB_JSON.d == "EXISTE") {
                                $.JMensajePOPUP("Validación", "El principio activo ya se encuentra agregado.", "", "Cerrar", "fn_oculta_mensaje()", "frmDeclaratoriaAlergia");
                            } else {
                                if ($("#rbSI_Dec").prop("checked")) {
                                    $("#divGridDeclaratoriaAlergia").load("GridViewAjax/GridDeclaratoriaAlergia.aspx", { Codigo: $("#hfCodigoPrincipioActivo").val().trim(), Nombre: $("#txtPrincipioActivo").val().trim(), Pagina: "1" }, function () {
                                    });
                                    $("#hfCodigoPrincipioActivo").val(""); //06/09/2016
                                    $("#txtPrincipioActivo").val(""); //06/09/2016
                                }                                
                            }
                        });
                    } else {
                        aValores = oOB_JSON.d.toString().split(";");
                        $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession_DeclatoriaAlergia()", "frmDeclaratoriaAlergia");
                    }
                });
            });

        });


        function fn_CreaEventoBusquedaPrincipioActivo() {
            $("#divPrincipioActivo").find(".JSBTABLA-1 tr td").click(function () {                
                if ($(this).html().trim() != "") {
                    $("#hfCodigoPrincipioActivo").val($(this).parent().find("td").eq(0).html().trim());
                    $("#txtPrincipioActivo").val($(this).parent().find("td").eq(1).html().trim());

                    $("#divFONDO1").css("display", "none");
                    $("#divPrincipioActivo").css("display", "none");
                }
            });
        }


        function fn_CierraPopup() {
            $.ajax({
                url: "PopUp/DeclaratoriaAlergia.aspx/EliminaListadoAlergia",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (dato1, datos2, dato3) {
                }
            }).done(function (oOB_JSON) {
                fn_oculta_popup();
            });

        }

        function fn_EditarDeclaratoriaAlergia() {
            $.ajax({ url: "PopUp/DeclaratoriaAlergia.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    if ($("#rbNo_Dec").prop("checked")) {
                        $("#rbNo_Dec").removeAttr("disabled");
                        $("#rbSI_Dec").removeAttr("disabled");
                    } else {
                        if ($("#chkNombreRepresentado_Dec").prop("checked")) {
                            fn_HabilitaControles("rbNo_Dec;rbSI_Dec;txtPrincipioActivo;txtAlimento_Dec;txtOtros_Dec;chkNombreRepresentado_Dec;txtNroDni;txtNombreRepresentado_Dec");
                            $.JValidaCampoObligatorio("txtNroDni;txtNombreRepresentado_Dec");
                        } else {
                            fn_HabilitaControles("rbNo_Dec;rbSI_Dec;txtPrincipioActivo;txtAlimento_Dec;txtOtros_Dec;chkNombreRepresentado_Dec");
                        }
                        $("#frmDeclaratoriaAlergia").find(".JIMG-BUSQUEDA").css("opacity", "1");
                        $("#divGridDeclaratoriaAlergia").load("GridViewAjax/GridDeclaratoriaAlergia.aspx", { Pagina: "1" }, function () {
                        });
                        Editar = true;
                    }
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession_DeclatoriaAlergia()", "frmDeclaratoriaAlergia");
                }
            });
        }

        function fn_GuardarDeclaratoriaAlergia() {
            $.ajax({ url: "PopUp/DeclaratoriaAlergia.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
//                    if (Editar == false) {
//                        return;
//                    }

                    var PresentaAlergia = false;
                    if ($("#rbSI_Dec").prop("checked")) {
                        PresentaAlergia = true;
                    }
                    var RepresentanteAlergia = false;
                    if ($("#chkNombreRepresentado_Dec").prop("checked")) {
                        RepresentanteAlergia = true;
                    }
                    if (RepresentanteAlergia == true && $("#rbSI_Dec").prop("checked")) { //&&
                        if ($.JValidaCampoObligatorio("txtNroDni;txtNombreRepresentado_Dec") == false) {
                            $.JMensajePOPUP("Validación", "Ingrese los campos en rojo", "", "Cerrar", "fn_ValidaCampoObli()", "frmDeclaratoriaAlergia");
                            return;
                        }
                    }
                    $.ajax({
                        url: "PopUp/DeclaratoriaAlergia.aspx/GuardarDeclaratoriaAlergia",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            OtrosDeclaratoria: $("#txtOtros_Dec").val(),
                            AlimentosDeclaratoria: $("#txtAlimento_Dec").val(),
                            PresentaAlergiaDeclaratoria: PresentaAlergia.toString(),
                            RepresentanteDeclaratoria: RepresentanteAlergia.toString(),
                            NroDocumentoDeclaratoria: $("#txtNroDni").val(),
                            NombreRepresentanteDeclaratoria: $("#txtNombreRepresentado_Dec").val()
                        }),
                        dataType: "json",
                        error: function (dato1, datos2, dato3) {
                        }
                    }).done(function (oOB_JSON) {
                        if (oOB_JSON.d != "OK") {
                            $.JMensajePOPUP("Error", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_mensaje()", "frmDeclaratoriaAlergia");
                        } else {
                            fn_ExportarBD();
                            fn_CargarAlergia1(); //15/01/2020 - JB - ESTA FUNCION ESTA EN EL FORMULARIO INFORMACIONPACIENTE.ASPX Y SERVIRA PARA ACTUALIZAR EL ESTADO DE 'ALERGIA' 
                            $.JMensajePOPUP("Aviso", "Se guardaron los datos correctamente", "OK", "Cerrar", "fn_oculta_mensaje()", "frmDeclaratoriaAlergia");
                            $("#divGridDeclaratoriaAlergia").load("GridViewAjax/GridDeclaratoriaAlergia.aspx", { Pagina: "1" }, function () {
                            });
                        }
                    });
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession_DeclatoriaAlergia()", "frmDeclaratoriaAlergia");
                }
            });
        }

        function fn_ValidaCampoObli() {
            $("#txtNroDni").focus();
            fn_oculta_mensaje();
        }


        function fn_DeshabilitaCtrlAlergia() {
            fn_DeshabilitaControles("txtPrincipioActivo;txtAlimento_Dec;txtOtros_Dec;txtNroDni;txtNombreRepresentado_Dec"); //chkNombreRepresentado_Dec
            $("#txtPrincipioActivo").val("");
            $("#txtAlimento_Dec").val("");
            $("#txtOtros_Dec").val("");
            //$("#txtNroDni").val("");
            //$("#txtNombreRepresentado_Dec").val("");
            $.ajax({
                url: "PopUp/DeclaratoriaAlergia.aspx/LimpiaListadoAlergia",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            }).done(function (oOB_JSON) {
                $("#divGridDeclaratoriaAlergia").load("GridViewAjax/GridDeclaratoriaAlergia.aspx", { Pagina: "1" }, function () {
                    //$("#divGridDeclaratoriaAlergia").find(".JIMG-ELIMINAR").unbind("click");
                });
            });
            Editar = false;
            $("#frmDeclaratoriaAlergia").find(".JIMG-BUSQUEDA").css("opacity", "0.6");
            fn_oculta_mensaje();
        }

        function fn_CancelarNo() {
            $("#rbSI_Dec").prop("checked", true);
            fn_oculta_mensaje();
        }


        function fn_ExpiraSession_DeclatoriaAlergia() {
            window.location.href = aValores[1];
        }
        
        function fn_ExportarBD() {
            //window.open("Reporte.aspx?OP=DA");
            window.open("Reporte.aspx?OP=DA&EX=1");
        }

        //Cmendez 25/05/2022 
        $(".JTEXTO").keypress(function (e) {
            var ValidaTilde = /[|'><&]/;
            if (ValidaTilde.test(String.fromCharCode(event.which))) {
                event.preventDefault();
            }
        });
        $(".JTEXTO").blur(function () {
            for (var i = 0; i < this.value.length; i++) {
                if (this.value.includes('|')) {
                    $(this).val($(this).val().replace("|", ""));
                }
                if (this.value.includes("'")) {
                    $(this).val($(this).val().replace("'", ""));
                }
                if (this.value.includes("<")) {
                    $(this).val($(this).val().replace("<", ""));
                }
                if (this.value.includes(">")) {
                    $(this).val($(this).val().replace(">", ""));
                }
                if (this.value.includes("&")) {
                    $(this).val($(this).val().replace("&", ""));
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
    <form id="frmDeclaratoriaAlergia" runat="server" class="JFORM-CONTENEDOR-POPUP">        
        <div class="JFILA">
            <div class="JCELDA-12">
                <div class="DatosUsuario" id="DatosUsuarioDeclaratoriaAlergia">
                                
                </div>
            </div>
        </div>
        <div class="JFILA">
            <div class="JCELDA-12">
                <div class="JDIV-CONTROLES JDIV-GRUPO" style="text-align:center;">
                    Declaratoria de Alergia
                </div>
            </div>
        </div>
        <div class="JFILA">
            <div class="JCELDA-7">
                <div class="JFILA">
                    <div class="JCELDA-12">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA">Presenta el Paciente algun tipo de alergia</span>
                        </div>    
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-2">
                        <div class="JDIV-CONTROLES">
                            <input type="radio" id="rbSI_Dec" name="rbAlergiaSINO" /><span class="JETIQUETA_4">SI</span>
                        </div>
                    </div>
                    <div class="JCELDA-2">
                        <div class="JDIV-CONTROLES">
                            <input type="radio" id="rbNo_Dec" name="rbAlergiaSINO" /><span class="JETIQUETA_4">No</span>
                        </div>
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2">Representante (DNI / Nombre)</span>
                        </div>
                    </div>
                    <div class="JCELDA-1">
                        <div class="JDIV-CONTROLES">
                            <input type="checkbox" id="chkNombreRepresentado_Dec" />
                        </div>
                    </div>
                    <div class="JCELDA-2">
                        <div class="JDIV-CONTROLES">
                            <input type="text"  id="txtNroDni" class="JNUMERO" disabled="disabled" maxlength="8" />
                        </div>
                    </div>
                    <div class="JCELDA-5">
                        <div class="JDIV-CONTROLES">
                            <input type="text"  id="txtNombreRepresentado_Dec" class="JTEXTO JTEXTO-C" disabled="disabled" />
                        </div>
                    </div>
                    <div class="JCELDA-1">
                       
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-4">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2">Principio Activo</span>
                        </div>
                    </div>
                    <div class="JCELDA-7" style="position:initial;overflow:initial;position:static;">
                        <div class="JDIV-CONTROLES">
                            <input type="hidden" id="hfCodigoPrincipioActivo" />
                            <input type="text" id="txtPrincipioActivo" class="JTEXTO" />
                            <div id="divPrincipioActivo" class="JBUSQUEDA-ESPECIAL" style="max-height:130px;width:57%;">
                                <%--BUSQUEDA PRICIPIO ACTIVO--%>
                            </div>
                        </div>
                    </div>
                    <div class="JCELDA-1">
                        <div class="JDIV-CONTROLES">
                            <img src="../Imagenes/Buscar.png" id="imgBuscarPrincipioActivo" alt="" class="JIMG-BUSQUEDA" />
                        </div>
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-4">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2">Alimentos</span>
                        </div>
                    </div>
                    <div class="JCELDA-7">
                        <div class="JDIV-CONTROLES">
                            <%--<input type="text" runat="server" id="txtAlimento_Dec" class="JTEXTO" />--%>
                            <textarea rows="3" cols="1" id="txtAlimento_Dec" class="JTEXTO"></textarea>
                        </div>
                    </div>
                    <div class="JCELDA-1">
                        <div class="JDIV-CONTROLES">
                            <img src="../Imagenes/Agregar.png" id="imgAgregarPrincipioActivo" alt="" class="JIMG-BUSQUEDA" />
                        </div>
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-4">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2">Otros</span>
                        </div>
                    </div>
                    <div class="JCELDA-7">
                        <div class="JDIV-CONTROLES">
                            <%--<input type="text" runat="server" id="txtAlimentos_Dec" class="JTEXTO" />--%>
                            <textarea rows="3" cols="1" id="txtOtros_Dec" class="JTEXTO"></textarea>
                        </div>
                    </div>                    
                </div>
            </div>
            <div class="JCELDA-5">
                <div class="JFILA">
                    <div class="JCELDA-12">
                        <span class="JETIQUETA_3">Principios Activos Seleccionados</span>
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-12">
                        <div class="JDIV-CONTROLES">
                            <div id="divGridDeclaratoriaAlergia">   
                                
                            </div>
                        </div>
                    </div>                    
                </div>
            </div>

        </div>
    </form>
</body>
</html>
