<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VerificarMedicamento.aspx.vb" Inherits="WebHCE.VerificarMedicamento" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#divGridVerificarMedicamento").load("GridViewAjax/GridVerificarMedicamento.aspx", {
                Pagina: "1",
                IdReceta: $("#" + "<%=hfIdReceta.ClientID %>").val()
            }, function () {

            });
        });


        function fn_VerificarSuspension() {
            $.ajax({ url: "PopUp/VerificarMedicamento.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $("#frmGridVerificarMedicamento").find(".JSBTABLA").find("tr").each(function () {
                        var objeto = $(this);
                        if (objeto.find(".ide_medicamento_suspension").length > 0) { //si selecciona alguna fila que no sea la cabecera del listado
                            var bRegistroMarcado = false;

                            $("#frmGridVerificarMedicamento").find(".JSBTABLA").find("tr").find(".Verificar").find("input[type='checkbox']").each(function () {
                                if ($(this).prop("checked")) {
                                    bRegistroMarcado = true
                                }
                            });

                            if (bRegistroMarcado == false) {
                                return;
                            } else {
                            }

                            if (objeto.find(".Verificar").find("input[type='checkbox']").prop("checked") == false) { //si la fila no esta con el check marcado no realizara ninguna accion
                                return
                            }

                            $.ajax({
                                url: "Popup/VerificarMedicamento.aspx/VerificarMedicamento",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                data: JSON.stringify({
                                    IdMedicamentoSuspendido: objeto.find(".ide_medicamento_suspension").html().trim()
                                }),
                                dataType: "json",
                                error: function (dato1, datos2, dato3) {

                                }
                            }).done(function (oOB_JSON) {
                                if (oOB_JSON.d != "OK") {
                                    $.JMensajePOPUP("Mensaje de Clínica San Felipe - Error", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_mensaje()", "frmVerificarMedicamento");
                                } else {
                                    fn_LOAD_GRID_VISI();
                                    $("#divGridVerificarMedicamento").load("GridViewAjax/GridVerificarMedicamento.aspx", {
                                        Pagina: "1",
                                        IdReceta: $("#" + "<%=hfIdReceta.ClientID %>").val()
                                    }, function () {
                                        fn_LOAD_GRID_OCUL();
                                    });
                                }
                            });
                        }
                    });
                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });
        }
    </script>
</head>
<body>
    <form id="frmVerificarMedicamento" runat="server">
        <input type="hidden" runat="server" id="hfIdReceta" />
        <div class="JFILA"> 
            <div class="JCELDA-12">
                <div class="DatosUsuario" id="divDatosUsuarioVerificarMedicamento">
                                
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
                    <div id="divGridVerificarMedicamento">
                    
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
