<%@ Page Title="Página principal" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false"
    CodeBehind="Login.aspx.vb" Inherits="WebHCE._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
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
        $("#btnIngresar").click(function () {
            if ($("#txtUsuario").val().trim() == "" || $("#txtPassword").val().trim() == "") {
                $.JMensajePOPUP("Validación", "Debe ingresar DNI y contraseña.", "1", "Cerrar", "fn_oculta_mensaje()");
                return false;
            }

            var Mensaje = "";
            fn_LOAD_VISI();
            $.ajax({
                url: "Login.aspx/ValidaSesion",
                type: "POST",
                data: JSON.stringify({
                    Usuario: $("#txtUsuario").val(),
                    clave: $("#txtPassword").val()
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                /*async: false,*/
                error: function (error, abc, def) {
                    //alert(def);
                }
            }).done(function (oOB_JSON) {
                fn_LOAD_OCUL();
                Mensaje = oOB_JSON.d;
                if (Mensaje != "") {
                    $.JMensajePOPUP("Validación", Mensaje, "1", "Cerrar", "SetearFocus()");
                    //$.post("About.aspx", { nombre: "John", time: "2pm" });
                    /*HttpContext.Current.Request.Form("nombre").Trim()*/
                } else {       
                    if ($("#" + "<%=hfInterconsulta.ClientID %>").val() != "") {
                        window.location.href = "Intranet/RespuestaInterconsulta.aspx";
                    } else {
                        window.location.href = "Intranet/InformacionPaciente.aspx";                        
                        //window.location.href = "Intranet/ConsultaPacienteHospitalizado.aspx";
                    }

                }
            });

        });
    });

    function SetearFocus() {
        fn_oculta_mensaje();
        $("#txtUsuario").focus();
    }
</script>

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <input type="hidden" id="hfInterconsulta" name="hfInterconsulta" runat="server" /> 
    <div class="JDIV_FORM_LOGU">
        <span style="float:left;"><h4>Iniciar Sesión RCE(Hospitalización)</h4></span>
        <div class="JDIV_IMAGEN_LOGIN">
            <img src="Imagenes/Logo_Login.png" style="width:140px; height:110px;" />
        </div>
        <div class="JDIV_CONTROLES_LOGIN">
            <span class="JETIQUETA_2" style="float:left;">DNI</span>
            <input id="txtUsuario" type="text" class="JTEXTO" placeholder="DNI" maxlength="8" />
            <span class="JETIQUETA_2" style="float:left;">Contraseña</span>
            <input id="txtPassword" type="password" class="JTEXTO" placeholder="Contraseña" maxlength="25"  />
            <a href="#">¿Olvidaste tu Contraseña?</a>
            <input type="button" value="Ingresar" id="btnIngresar" />
        </div>            
    </div>
</asp:Content>
