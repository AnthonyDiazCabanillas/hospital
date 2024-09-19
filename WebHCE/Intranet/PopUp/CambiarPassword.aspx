<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CambiarPassword.aspx.vb" Inherits="WebHCE.CambiarPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        $(document).ready(function () {

        });

        function fn_CambiarContraseña() {
            if ($("#txtNuevoPassword").val().trim() == "") {
                $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe", "Escribir la nueva contraseña.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()", "frmCambiarPassword");
                return;
            }
            if ($("#txtRepetirPassword").val().trim() == "") {
                $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe", "Escribir la verficación de la contraseña.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()", "frmCambiarPassword");
                return;
            }
            if ($("#txtNuevoPassword").val().length < 4) {
                $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe", "La nueva contraseña debe tener como minimo 4 caracteres.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()", "frmCambiarPassword");
                return;
            }
            if ($("#txtNuevoPassword").val().trim() != $("#txtRepetirPassword").val().trim()) {
                $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe", "La Nueva Contraseña y la Contraseña de Verificación no coinciden.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()", "frmCambiarPassword");
                return;
            }


            $.ajax({
                url: "PopUp/CambiarPassword.aspx/CambiarPassword",
                type: "POST",
                data: JSON.stringify({
                    NuevaPassword: $("#txtNuevoPassword").val(),
                    RepetirPassword: $("#txtRepetirPassword").val(),
                    AnteriorPassword: $("#txtAnteriorPassword").val()
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (error, abc, def) {
                    //alert(def);
                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d != "") {
                    $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_mensaje()", "frmCambiarPassword");
                } else {
                    $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe", "Se realizo el cambio correctamente", "OK", "Cerrar", "fn_OcultarCambiarPass()", "frmCambiarPassword");
                }
            });

            /*
            If TxtClaveNueva1.Text.Trim = "" Then
                retorna = "Escribir la nueva contraseña"
                Return retorna
            End If

            If TxtClaveNueva2.Text.Trim = "" Then
                retorna = "Escribir la verficación de la contraseña"
                Return retorna
            End If

            If TxtClaveNueva1.Text.Length < 4 Then
                retorna = "La nueva contraseña debe tener como minimo 4 caracteres"
                Return retorna
            End If

            If TxtClaveNueva1.Text.Trim <> TxtClaveNueva2.Text.Trim Then
                retorna = "La Nueva Contraseña y la Contraseña de Verificación no coinciden"
                Return retorna
            End If
            */
        }

        function fn_OcultarCambiarPass() {
            fn_oculta_mensaje();
            fn_oculta_popup();
        }

    </script>
</head>
<body>
    <form id="frmCambiarPassword" runat="server">
        <div class="JFILA">
            <div class="JCELDA-4">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA_2">Anterior Contraseña</span>
                </div>
            </div>
            <div class="JCELDA-7">
                <div class="JDIV-CONTROLES">
                    <input type="password" class="JTEXTO" id="txtAnteriorPassword" />
                </div>
            </div>
        </div>
        <div class="JFILA">
            <div class="JCELDA-4">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA_2">Nueva Contraseña</span>
                </div>
            </div>
            <div class="JCELDA-7">
                <div class="JDIV-CONTROLES">
                    <input type="password" class="JTEXTO" id="txtNuevoPassword" />
                </div>
            </div>
        </div>
        <div class="JFILA">
            <div class="JCELDA-4">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA_2">Repetir Contraseña</span>
                </div>
            </div>
            <div class="JCELDA-7">
                <div class="JDIV-CONTROLES">
                    <input type="password" class="JTEXTO" id="txtRepetirPassword" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
