<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AltaMedica.aspx.vb" Inherits="WebHCE.AltaMedica" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <script type="text/javascript">
        $(document).ready(function () {
            $("#divNecropcia").css("display", "none");

            $("#" + "<%=cbCondicionAltaMedica.ClientID %>").change(function () {
                if ($(this).val().trim() == "F") {
                    $("#divNecropcia").css("display", "block");
                } else {
                    $("#divNecropcia").css("display", "none");
                }
            });

        });

        function fn_AceptarAltaMedica() {
            if ($("#" + "<%=cbDestinoAltaMedica.ClientID %>").val() == "" || $("#" + "<%=cbCondicionAltaMedica.ClientID %>").val() == "") {
                $.JMensajePOPUP("Aviso", "Debe ingresar todos los datos.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()", "frmAltaMedica");
                return;
            }
            if ($("#" + "<%=cbDestinoAltaMedica.ClientID %>").val() == "") {
                $.JMensajePOPUP("Aviso", "Debe ingresar destino.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()", "frmAltaMedica");
                return;
            }
            if ($("#" + "<%=cbCondicionAltaMedica.ClientID %>").val() == "") {
                $.JMensajePOPUP("Aviso", "Debe ingresar todos los datos.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()", "frmAltaMedica");
                return;
            }
            $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe", "¿Seguro de dar de alta al paciente? ¿Desea continuar?.", "ADVERTENCIA", "Si;No", "fn_AceptaAltaSi();fn_oculta_mensaje()", "frmAltaMedica");
        }

        function fn_AceptaAltaSi() {
            //PopUp/DeclaratoriaAlergia.aspx/CargarDatos
            var Necro = "";
            if ($("#rbSiNecropcia").prop("checked")) {
                Necro = "1";
            }
            if ($("#rbNoNecropcia").prop("checked")) {
                Necro = "0";
            } 

            $.ajax({
                url: "PopUp/AltaMedica.aspx/DarAltaMedica",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    CodigoDestino: $("#" + "<%=cbDestinoAltaMedica.ClientID %>").val(),
                    CondicionAlta: $("#" + "<%=cbCondicionAltaMedica.ClientID %>").val(),
                    Necropcia: Necro
                }),
                dataType: "json",
                error: function (dato1, datos2, dato3) {

                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d != "OK") {
                    $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe - Error", oOB_JSON.d.toString(), "ADVERTENCIA", "Aceptar", "fn_oculta_mensaje()", "frmAltaMedica");
                } else {
                    $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe", "Se guardaron los datos correctamente.", "OK", "Aceptar", "fn_oculta_mensaje()", "frmAltaMedica");
                }
            });

        }

    </script>

</head>
<body>
    <form id="frmAltaMedica" runat="server">
    
    <div class="JFILA">
        <div class="JCELDA-12">
            <div class="DatosUsuario">
                <div class="JFILA">
                    <div class="JCELDA-2">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA">Paciente :</span>
                        </div>
                    </div>
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2" runat="server" id="spDatosNombreApellido"></span>
                        </div>
                    </div>
                    <div class="JCELDA-2">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA">Médico :</span> </div>
                    </div>
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2" runat="server" id="spNombreMedico"></span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    

    <div class="JFILA">
        <div class="JCELDA-2">
            <div class="JDIV-CONTROLES">
                <span class="JETIQUETA">Destino :</span>
            </div>
        </div>
        <div class="JCELDA-3">
            <div class="JDIV-CONTROLES">
                <asp:DropDownList runat="server" CssClass="JSELECT" ID="cbDestinoAltaMedica"></asp:DropDownList>
            </div>
        </div>
        <div class="JCELDA-2 JESPACIO-IZQ-1">
            <div class="JDIV-CONTROLES">
                <span class="JETIQUETA">Condición de Alta :</span> </div>
        </div>
        <div class="JCELDA-3">
            <div class="JDIV-CONTROLES">
                <asp:DropDownList runat="server" CssClass="JSELECT" ID="cbCondicionAltaMedica"></asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="JFILA" id="divNecropcia">
        <div class="JCELDA-2">
            <div class="JDIV-CONTROLES">
                <span class="JETIQUETA">Necropsia :</span>
            </div>
        </div>
        <div class="JCELDA-3">
            <div class="JDIV-CONTROLES">
                <input type="radio" id="rbSiNecropcia" name="rbNecropcia" /><span class="JETIQUETA_2">Si</span>
                <input type="radio" id="rbNoNecropcia" name="rbNecropcia" checked="checked" /><span class="JETIQUETA_2">No</span>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
