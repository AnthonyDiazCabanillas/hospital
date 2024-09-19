<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LaboratorioCompletado.aspx.vb" Inherits="WebHCE.LaboratorioCompletado" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#divGridLaboratorioCompletado").load("GridViewAjax/GridLaboratorioCompletado.aspx", {
                Pagina: "1",
                IdReceta: $("#" + "<%=hfIdReceta.ClientID %>").val()
            }, function () {

            });
        });


        function fn_VerificarLaboratorio() {
            $.ajax({ url: "PopUp/LaboratorioCompletado.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    $("#frmGridLaboratorioCompletado").find(".JSBTABLA").find("tr").each(function () {
                        var objeto = $(this);
                        if (objeto.find(".ide_recetadet").length > 0) { //si selecciona alguna fila que no sea la cabecera del listado
                            var bRegistroMarcado = false;

                            $("#frmGridLaboratorioCompletado").find(".JSBTABLA").find("tr").find(".Verificar").find("input[type='checkbox']").each(function () {
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
                                url: "Popup/LaboratorioCompletado.aspx/LaboratorioCompletado",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                data: JSON.stringify({
                                    IdReceta: objeto.find(".ide_recetadet").html().trim()
                                }),
                                dataType: "json",
                                error: function (dato1, datos2, dato3) {

                                }
                            }).done(function (oOB_JSON) {
                                if (oOB_JSON.d != "OK") {
                                    $.JMensajePOPUP("Mensaje de Clínica San Felipe - Error", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_mensaje()", "frmLaboratorioCompletado");
                                } else {
                                    fn_LOAD_GRID_VISI();
                                    $("#divGridLaboratorioCompletado").load("GridViewAjax/GridLaboratorioCompletado.aspx", {
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
                    var aValorers = oOB_JSON.d.toString().split(";");
                    window.location.href = aValorers[1];
                }
            });
        }
    </script>
</head>
<body>
    <form id="frmLaboratorioCompletado" runat="server">
        <input type="hidden" runat="server" id="hfIdReceta" />
        <div class="JFILA"> 
            <div class="JCELDA-12">
                <div class="DatosUsuario" id="divDatosLaboratorioCompletado">
                                
                </div>
            </div>
        </div>
        <div class="JFILA">
            <div class="JCELDA-12">
                <div class="JDIV-CONTROLES">
                    <div id="divGridLaboratorioCompletado">
                    
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
