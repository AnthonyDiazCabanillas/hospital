<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OlvidoPassword.aspx.vb" Inherits="WebHCE.OlvidoPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        $(document).ready(function () {

        });

        function fn_RecuperaPassword() {
            if ($("#txtCorreoRecuperarPass").val().trim() == "") {
                $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe", "Ingresar el correo.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()", "frmOlvidoPassword");
                return;
            }

            $.ajax({
                url: "PopUp/OlvidoPassword.aspx/RecuperaPassword",
                type: "POST",
                data: JSON.stringify({
                    Correo: $("#txtCorreoRecuperarPass").val()                    
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (error, abc, def) {
                    //alert(def);
                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d != "") {
                    $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe", oOB_JSON.d, "1", "Cerrar", "fn_CerrarOlvidoPass()", "frmOlvidoPassword");
                } else {
                    $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe", "Se envio la Clave a su Correo.", "1", "Cerrar", "fn_CerrarOlvidoPass()", "frmOlvidoPassword");                    
                }
            });

        }


        function fn_CerrarOlvidoPass() {
            fn_oculta_popup();
            fn_oculta_mensaje();
        }
    </script>

</head>
<body>
    <form id="frmOlvidoPassword" runat="server">
        <div class="JFILA">
            <div class="JCELDA-3">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA_2">Correo</span>
                </div>
            </div>
            <div class="JCELDA-8">
                <div class="JDIV-CONTROLES">
                    <input type="text" class="JTEXTO" id="txtCorreoRecuperarPass" placeholder="Ingrese Correo Personal" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
