<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SuspensionMedicamento.aspx.vb" Inherits="WebHCE.SuspensionMedicamento" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#divGridSuspensionMedicamento").load("GridViewAjax/GridSuspensionMedicamento.aspx", {
                Pagina: "1",
                IdReceta: $("#" + "<%=hfIdReceta.ClientID %>").val()
            }, function () {
                
            });
        });

        function fn_SuspenderSuspension() {
            $.ajax({ url: "PopUp/SuspensionMedicamento.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    //JB - COMENTADO - 05/05/2021
                    //                    $("#frmGridSuspensionMedicamento").find(".JSBTABLA").find("tr").each(function () {
                    //                        var objeto = $(this);
                    //                        if (objeto.find(".ide_medicamentorec").length > 0) { //si selecciona alguna fila que no sea la cabecera del listado
                    //                            var bRegistroMarcado = false;

                    //                            $("#frmGridSuspensionMedicamento").find(".JSBTABLA").find("tr").find(".Suspender").find("input[type='checkbox']").each(function () {
                    //                                if ($(this).prop("checked")) {
                    //                                    bRegistroMarcado = true
                    //                                }
                    //                            });
                    //                            if (bRegistroMarcado == false) {
                    //                                return;
                    //                            } else {
                    //                            }
                    //                            if (objeto.find(".Suspender").find("input[type='checkbox']").prop("checked") == false) { //si la fila no esta con el check marcado no realizara ninguna accion
                    //                                return
                    //                            }

                    //                            $.ajax({
                    //                                url: "Popup/SuspensionMedicamento.aspx/SuspenderMedicamento",
                    //                                type: "POST",
                    //                                contentType: "application/json; charset=utf-8",
                    //                                data: JSON.stringify({
                    //                                    IdMedicamentoRec: objeto.find(".ide_medicamentorec").html().trim()
                    //                                }),
                    //                                dataType: "json",
                    //                                error: function (dato1, datos2, dato3) {

                    //                                }
                    //                            }).done(function (oOB_JSON) {
                    //                                if (oOB_JSON.d != "OK") {
                    //                                    $.JMensajePOPUP("Mensaje de Clínica San Felipe - Error", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_mensaje()", "frmSuspensionMedicamento");
                    //                                } else {
                    //                                    fn_LOAD_GRID_VISI();
                    //                                    $("#divGridSuspensionMedicamento").load("GridViewAjax/GridSuspensionMedicamento.aspx", {
                    //                                        Pagina: "1",
                    //                                        IdReceta: $("#" + "<%=hfIdReceta.ClientID %>").val()
                    //                                    }, function () {
                    //                                        fn_LOAD_GRID_OCUL();
                    //                                        //fn_CargarControlClinicoIM(); //JB - SE INVOCA ESTA FUNCION PARA REFRESCAR EL LISTADO
                    //                                        fn_CargarControlClinicoIM2("1", "", "0");
                    //                                    });
                    //                                }
                    //                            });
                    //                        }
                    //                    });

                    fn_LOAD_VISI();
                    $.ajax({
                        url: "Popup/SuspensionMedicamento.aspx/SuspenderMedicamento",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        error: function (dato1, datos2, dato3) {

                        }
                    }).done(function (oOB_JSON) {
                        fn_LOAD_OCUL();
                        if (oOB_JSON.d != "OK") {
                            $.JMensajePOPUP("Mensaje de Clínica San Felipe - Error", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_mensaje()", "frmSuspensionMedicamento");
                        } else {
                            fn_LOAD_GRID_VISI();
                            $("#divGridSuspensionMedicamento").load("GridViewAjax/GridSuspensionMedicamento.aspx", {
                                Pagina: "1",
                                IdReceta: $("#" + "<%=hfIdReceta.ClientID %>").val()
                            }, function () {
                                fn_LOAD_GRID_OCUL();
                                fn_Load_Page();
                                //fn_CargarControlClinicoIM(); //JB - SE INVOCA ESTA FUNCION PARA REFRESCAR EL LISTADO
                                fn_CargarControlClinicoIM2("1", "", "0");
                            });
                        }
                    });
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        }

        /*OCULTA POPUP DE SUSPENSION DE MEDICAMENTOS Y ELIMINA LA SESSION USADA EN ELLA*/
        function fn_CancelarSuspension() {
            $.ajax({
                url: "Popup/SuspensionMedicamento.aspx/CancelaSuspensinMedicamento",
                type: "POST",
                contentType: "application/json; charset=utf-8",                
                dataType: "json",
                error: function (dato1, datos2, dato3) {

                }
            }).done(function (oOB_JSON) {
                fn_oculta_popup();    
            });
            
        }

    </script>
</head>
<body>
    <form id="frmSuspensionMedicamento" runat="server">
        <input type="hidden" runat="server" id="hfIdReceta" />
        <div class="JFILA"> 
            <div class="JCELDA-12">
                <div class="DatosUsuario" id="divDatosUsuarioSuspensionMedicamento">
                                
                </div>
            </div>
        </div>
        <div class="JFILA">
            <div class="JCELDA-12">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA" id="spDatosCabecera" runat="server"></span>
                </div>
            </div>
        </div>
        <div class="JFILA">
            <div class="JCELDA-12">
                <div class="JDIV-CONTROLES">
                    <div id="divGridSuspensionMedicamento">
                    
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
