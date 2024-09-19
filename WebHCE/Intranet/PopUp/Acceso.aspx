<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Acceso.aspx.vb" Inherits="WebHCE.Acceso" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .JDIV_FORM_LOGU{                
            max-width: 400px;
            padding: 10px;
            margin: auto;        
            /*background: linear-gradient(#E1E1E1, #CCCCCC, #848484);*/
            /*background: linear-gradient(#E3E3E3, #D0D0D0, #D0D0D0, #E3E3E3);*/
            overflow: hidden;
            text-align: center !important;
            /*box-shadow: 0 0 5px Black; TMACASSI*/
            margin-top: 5px;
            margin-bottom: 5px;
            /*background: linear-gradient(#46627f, #46627f, #46627f, #344657);*/
        }
            
        .JDIV_FORM_LOGU input[type="text"], .JDIV_FORM_LOGU input[type="password"]  {
            width: 98%;
            padding: 5px;  
            outline: 0;
            border-radius: 5px;
            margin-top: 5px;
            margin-bottom: 5px;
            font-size: 1em;
        }       
        
            
        .JDIV_FORM_LOGU input[type="text"]:focus{
            box-shadow: 0 0 5px #33B333;
        }
               
        .JDIV_FORM_LOGU input[type="button"]
        {
            float:right;
            margin-top: 15px;
            }
            
            
        .JDIV_FORM_LOGU h4{
            color: #33B333;                
            font-family:Arial;
            font-size:15px;
            font-weight:bold;  
            margin-left:0px;
        }
    
        .JDIV_FORM_LOGU a 
        {
            margin-top:10px;
            color:#134B8D;
            font-family:Arial;
            font-size:11px;
            font-weight:bold;
            }
        
        .JDIV_IMAGEN_LOGIN
        {
            float:left; 
            width:150px;
            text-align:center;
            }
        .JDIV_CONTROLES_LOGIN
        {
            float:left; 
            width:200px; 
            margin-left:10px;
         }
         /*TMACASSI*/
        .JDIV_FORM_LOGU btnIngresar 
        {
            margin-top:10px;
            color:#134B8D;
            font-family:Arial;
            font-size:11px;
            font-weight:bold;
            } 
    
        
         @media (max-width:768px)
         {
             .JDIV_IMAGEN_LOGIN
             {
                 float:none;
                 width: auto;
                 margin-bottom: 15px;
             }
             
              .JDIV_CONTROLES_LOGIN
              {
                  width: auto;
              }
          }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtPassword").keypress(function (e) {
                if (e.which == 13) {
                    fn_IngresarLogin();
                }
            });
            $("#txtUsuario").keypress(function (e) {
                if ($("#rbTipoLoginMedico").prop("checked")) {
                    if ($(this).val().trim().length == 7) {
                        $("#txtPassword").focus();
                    }
                }
            });
            $("#txtUsuario").change(function (e) {
                if ($("#rbTipoLoginMedico").prop("checked")) {
                    if ($(this).val().trim().length == 8) {
                        $("#txtPassword").focus();
                    }
                }                
            });


            $("#rbTipoLoginMedico").click(function () {
                if ($(this).prop("checked")) {
                    $("#spEtiquetaTipoLogin").html("DNI");
                }
            });
            $("#rbTipoLoginAdministrativo").click(function () {
                if ($(this).prop("checked")) {
                    $("#spEtiquetaTipoLogin").html("LOGIN");
                }
            });

            $("#btnIngresar").click(function () {                
                if ($("#txtUsuario").val().trim() == "" || $("#txtPassword").val().trim() == "") {
                    if ($("#rbTipoLoginMedico").prop("checked")) {
                        $.JMensajePOPUP("Validación", "Debe ingresar DNI y contraseña.", "1", "Cerrar", "fn_oculta_mensaje()");
                    } else {
                        $.JMensajePOPUP("Validación", "Debe ingresar Usuario y contraseña.", "1", "Cerrar", "fn_oculta_mensaje()");
                    }
                    return false;
                }

                var Mensaje = "";
                fn_LOAD_POPU_VISI();
                if ($("#rbTipoLoginMedico").prop("checked")) {
                    $.ajax({
                        url: "PopUp/Acceso.aspx/ValidaSesion",
                        type: "POST",
                        data: JSON.stringify({
                            Usuario: $("#txtUsuario").val(),
                            clave: $("#txtPassword").val()
                        }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        error: function (error, abc, def) {
                            //alert(def);
                        }
                    }).done(function (oOB_JSON) {
                        Mensaje = oOB_JSON.d;
                        if (Mensaje != "") {
                            fn_LOAD_POPU_OCUL();
                            if (Mensaje == "EXPIRO_ATENCION") {
                                fn_oculta_popup();
                                return;
                            }
                            $.JMensajePOPUP("Validación", Mensaje, "1", "Cerrar", "SetearFocus()");
                            //$.post("About.aspx", { nombre: "John", time: "2pm" });
                            /*HttpContext.Current.Request.Form("nombre").Trim()*/
                        } else {
                            /*if ($("#" + "<=hfInterconsulta.ClientID %>").val() != "") {
                            window.location.href = "Intranet/RespuestaInterconsulta.aspx";
                            } else {
                            window.location.href = "InformacionPaciente.aspx";
                            }*/
                            if ($("#" + "<%= hfParametro.ClientID %>").val() == "S") { //06/02/2017
                                fn_oculta_popup("fn_AbreVentanaReporte");
                            } else {
                                window.location.href = "InformacionPaciente.aspx";
                            }

                        }
                    });
                } else {
                    $.ajax({
                        url: "PopUp/AccesoEnfermera.aspx/ValidaSesion",
                        type: "POST",
                        data: JSON.stringify({
                            Usuario: $("#txtUsuario").val(),
                            clave: $("#txtPassword").val()
                        }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        error: function (error, abc, def) {

                        }
                    }).done(function (oOB_JSON) {
                        fn_LOAD_POPU_OCUL();
                        Mensaje = oOB_JSON.d;
                        if (Mensaje != "") {
                            if (Mensaje == "EXPIRO_ATENCION") {
                                fn_oculta_popup();
                                return;
                            }
                            $.JMensajePOPUP("Validación", Mensaje, "ERROR", "Cerrar", "SetearFocus()");
                        } else {
                            if ($("#" + "<%= hfParametro.ClientID %>").val() == "S") { //06/02/2017
                                fn_oculta_popup("fn_AbreVentanaReporte");
                            } else {
                                window.location.href = "InformacionPaciente.aspx";
                            }
                        }
                    });
                }

            });


            $("#lnkOlvidoPassword").click(function () {
                fn_oculta_popup("fn_AbreOlvidoPassword");
            });

        });

        function SetearFocus() {
            fn_oculta_mensaje();
            $("#txtUsuario").focus();
        }

        function fn_IngresarLogin() { 
            if ($("#txtUsuario").val().trim() == "" || $("#txtPassword").val().trim() == "") {
                if ($("#rbTipoLoginMedico").prop("checked")) {
                    $.JMensajePOPUP("Validación", "Debe ingresar DNI y contraseña.", "1", "Cerrar", "fn_oculta_mensaje()");
                } else {
                    $.JMensajePOPUP("Validación", "Debe ingresar Usuario y contraseña.", "1", "Cerrar", "fn_oculta_mensaje()");
                }
                return false;
            }

            var Mensaje = "";
            var sede = "";
            sede = "0";  //$("#" + "=ddlSede.ClientID").val();            
            fn_LOAD_POPU_VISI();
            if ($("#rbTipoLoginMedico").prop("checked"))
            {
                $.ajax({
                    url: "PopUp/Acceso.aspx/ValidaSesion",
                    type: "POST",
                    data: JSON.stringify({
                        Usuario: $("#txtUsuario").val(),
                        clave: $("#txtPassword").val(),
                        sede: sede
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    error: function (error, abc, def) {
                        //alert(def);
                    }
                }).done(function (oOB_JSON)
                {
                    Mensaje = oOB_JSON.d;
                    if (Mensaje != "")
                    {
                        if ($("#" + "<%= hfParametro.ClientID %>").val() == "S" && Mensaje == "EXPIRO_ATENCION") { //15/02/2021 - JB - se agrega esta condicion, si no cumple se ejecuta el codigo usuado anteriormente
                            fn_oculta_popup("fn_AbreVentanaReporte");
                        } else {
                            fn_LOAD_POPU_OCUL();
                            if (Mensaje == "EXPIRO_ATENCION") {
                                fn_oculta_popup();
                                return;
                            }
                            $.JMensajePOPUP("Validación", Mensaje, "1", "Cerrar", "SetearFocus()");
                        }
                        
                    } else
                    {
                        if ($("#" + "<%= hfParametro.ClientID %>").val() == "S") { //06/02/2017

                            fn_oculta_popup("fn_AbreVentanaReporte");
                        } else {
                            window.location.href = "InformacionPaciente.aspx";
                            /*window.open("InformacionPaciente.aspx", "InformacionPaciente1");
                            fn_oculta_popup();
                            fn_LOAD_POPU_OCUL();*/
                        }
                    }
                });
            } else
            {
                $.ajax({
                    url: "PopUp/AccesoEnfermera.aspx/ValidaSesion",
                    type: "POST",
                    data: JSON.stringify({
                        Usuario: $("#txtUsuario").val(),
                        clave: $("#txtPassword").val()
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    error: function (error, abc, def) {

                    }
                }).done(function (oOB_JSON)
                {
                    Mensaje = oOB_JSON.d;
                    if (Mensaje != "") {
                        if ($("#" + "<%= hfParametro.ClientID %>").val() == "S" && Mensaje == "EXPIRO_ATENCION") { //15/02/2021 - JB - se agrega esta condicion, si no cumple se ejecuta el codigo usuado anteriormente
                            fn_oculta_popup("fn_AbreVentanaReporte");
                        } else {
                            fn_LOAD_POPU_OCUL();
                            if (Mensaje == "EXPIRO_ATENCION") {
                                fn_oculta_popup();
                                return;
                            }
                            $.JMensajePOPUP("Validación", Mensaje, "ERROR", "Cerrar", "SetearFocus()");
                        }                    
                    } else
                    {
                        if ($("#" + "<%= hfParametro.ClientID %>").val() == "S") { //06/02/2017
                            fn_oculta_popup("fn_AbreVentanaReporte");
                        } else {
                            window.location.href = "InformacionPaciente.aspx";
                            /*window.open("InformacionPaciente.aspx", "InformacionPaciente1");
                            fn_oculta_popup();
                            fn_LOAD_POPU_OCUL();*/
                        }
                    }
                });
            }
            
        }
    </script>
</head>
<body>
    <form id="frmAcceso" runat="server" class="JFORM-CONTENEDOR-POPUP">
        <input type="hidden" id="hfInterconsulta" name="hfInterconsulta" runat="server" /> 
        <div class="JDIV_FORM_LOGU">
            <span style="float:left;"><h4>Iniciar Sesión RCE(Hospitalización)</h4></span>
            <div class="JDIV_IMAGEN_LOGIN">
                <img src="../Imagenes/Logo_Login.png" style="width:140px; height:110px;" />
            </div>
            <div class="JDIV_CONTROLES_LOGIN">
                <div style="float:left;overflow:hidden;">
                    <input type="radio" name="rbTipoLogin" id="rbTipoLoginMedico" checked="checked" /><span class="JETIQUETA">Medico</span>
                    <input type="radio" name="rbTipoLogin" id="rbTipoLoginAdministrativo" /><span class="JETIQUETA">Asistencial</span>
                </div>
                <br /><br />
                <span class="JETIQUETA_2" style="float:left;" id="spEtiquetaTipoLogin">DNI</span>
                <input id="txtUsuario" type="text" class="JTEXTO" placeholder="DNI" maxlength="25" />
                <span class="JETIQUETA_2" style="float:left;">Contraseña</span>
                <input id="txtPassword" type="password" class="JTEXTO" placeholder="Contraseña" maxlength="25"  />
                <%--<span class="JETIQUETA_2" style="float:left;">Sede</span>
                <asp:DropDownList runat="server" ID="ddlSede" CssClass="JSELECT">                                                
                </asp:DropDownList>--%>

                <a href="#" id="lnkOlvidoPassword">¿Olvidaste tu Contraseña?</a>
                <%--<input type="button" value="Ingresar" id="btnIngresar" />--%>
                <input type="hidden" runat="server" id="hfParametro" />
            </div>            
        </div>
    </form>
</body>
</html>
