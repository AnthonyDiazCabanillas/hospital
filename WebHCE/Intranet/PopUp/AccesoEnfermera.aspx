<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AccesoEnfermera.aspx.vb" Inherits="WebHCE.AccesoEnfermera" %>

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
            $("#txtPasswordEnfermera").keypress(function (e) {
                if (e.which == 13) {
                    fn_IngresarLoginEnfermera();
                }
            });

            $("#btnIngresar").click(function () {
                if ($("#txtUsuarioEnfermera").val().trim() == "" || $("#txtPasswordEnfermera").val().trim() == "") {
                    $.JMensajePOPUP("Validación", "Debe ingresar Usuario y contraseña.", "1", "Cerrar", "fn_oculta_mensaje()");
                    return false;
                }

                var Mensaje = "";
                fn_LOAD_VISI();
                $.ajax({
                    url: "PopUp/AccesoEnfermera.aspx/ValidaSesion",
                    type: "POST",
                    data: JSON.stringify({
                        Usuario: $("#txtUsuarioEnfermera").val(),
                        clave: $("#txtPasswordEnfermera").val()
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    error: function (error, abc, def) {

                    }
                }).done(function (oOB_JSON) {
                    fn_LOAD_OCUL();
                    Mensaje = oOB_JSON.d;
                    if (Mensaje != "") {
                        $.JMensajePOPUP("Validación", Mensaje, "ERROR", "Cerrar", "SetearFocus()");
                    } else {
                        window.location.href = "InformacionPaciente.aspx";
                    }
                });
            });
        });

        function SetearFocus() {
            fn_oculta_mensaje();
            $("#txtUsuarioEnfermera").focus();
        }


        function fn_IngresarLoginEnfermera() {
            if ($("#txtUsuarioEnfermera").val().trim() == "" || $("#txtPasswordEnfermera").val().trim() == "") {
                $.JMensajePOPUP("Validación", "Debe ingresar Usuario y contraseña.", "1", "Cerrar", "fn_oculta_mensaje()");
                return false;
            }

            var Mensaje = "";
            fn_LOAD_VISI();
            $.ajax({
                url: "PopUp/AccesoEnfermera.aspx/ValidaSesion",
                type: "POST",
                data: JSON.stringify({
                    Usuario: $("#txtUsuarioEnfermera").val(),
                    clave: $("#txtPasswordEnfermera").val()
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (error, abc, def) {

                }
            }).done(function (oOB_JSON) {
                fn_LOAD_OCUL();
                Mensaje = oOB_JSON.d;
                if (Mensaje != "") {
                    $.JMensajePOPUP("Validación", Mensaje, "ERROR", "Cerrar", "SetearFocus()");
                } else {
                    window.location.href = "InformacionPaciente.aspx";
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="JDIV_FORM_LOGU">
            <span style="float:left;"><h4>Iniciar Sesión RCE(Hospitalización)</h4></span>
            <div class="JDIV_IMAGEN_LOGIN">
                <img src="../Imagenes/Logo_Login.png" style="width:140px; height:110px;" />
            </div>
            <div class="JDIV_CONTROLES_LOGIN">
                <span class="JETIQUETA_2" style="float:left;">LOGIN</span>
                <input id="txtUsuarioEnfermera" type="text" class="JTEXTO" placeholder="LOGIN SIC" maxlength="20" />
                <span class="JETIQUETA_2" style="float:left;">Contraseña</span>
                <input id="txtPasswordEnfermera" type="password" class="JTEXTO" placeholder="Contraseña" maxlength="25"  />
            </div>            
        </div>

    <%--<div class="JDIV_FORM_LOGU">
        <h4>Iniciar Sesión (Control Clínico)</h4>
        <div class="JDIV_IMAGEN_LOGIN">
            <img src="Imagenes/Logo_Login.png" style="width:140px; height:110px;" />
        </div>
        <div class="JDIV_CONTROLES_LOGIN">            
            <input id="txtUsuarioEnfermera" type="text" class="JTEXTO" placeholder="Usuario" maxlength="8" />
            <input id="txtPasswordEnfermera" type="password" class="JTEXTO" placeholder="Contraseña" maxlength="25"  />
            <a href="#">¿Olvidaste tu Contraseña?</a>
            <input type="button" value="Ingresar" id="btnIngresar" />
        </div>            
    </div>--%>

    </form>
</body>
</html>
