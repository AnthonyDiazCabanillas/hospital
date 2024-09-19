<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ControlClinicoIndicacionMedica.aspx.vb" Inherits="WebHCE.ControlClinicoIndicacionMedica" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".JIMG-GENERAL").click(function () {
                fn_oculta_popup("fn_CCIM_ENFE");
            });
        });

        function fn_CCIM_ENFE() {
            $.JPopUp("", "PopUp/ControlClinicoIndicacionMedica_Edicion.aspx", "1", "Cerrar", "fn_oculta_popup()", 70);
        }
    </script>
</head>
<body>   
    <div class="JFILA">
        <div class="JCELDA-10">
            <div class="JFILA">
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA">Nombres y Apellidos</span>
                    </div>
                </div>
                <div class="JCELDA-3"> <%--style="border:1px solid Black;"--%>
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" runat="server" id="spDatosNombreApellido_CCIM_E">SUAREZ ACEVEDO ROWEL</span>
                    </div>
                </div>
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA">Edad</span>
                    </div>
                </div>
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" runat="server" id="spDatosEdad_CCIM_E">45 AÑOS 10 MESES 21 DIAS</span>
                    </div>
                </div>
            </div>
            <div class="JFILA">
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA">Diagnostico</span>
                    </div>
                </div>
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" runat="server" id="spDatosDiagnostico_CCIM_E">Colera</span>
                    </div>
                </div>
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA">Código Atencion:</span>
                    </div>
                </div>
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" runat="server" id="spDatosCodigoAtencion_CCIM_E">H1113255</span>
                    </div>
                </div>
            </div>
            <div class="JFILA">
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA">H.C</span>
                    </div>
                </div>
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" runat="server" id="spDatosHD_CCIM_E">397454</span>
                    </div>
                </div>
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA">Fono Contacto</span>
                    </div>
                </div>
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" runat="server" id="spFonoContacto_CCIM_E">5532875</span>
                    </div>
                </div>
            </div>
            <div class="JFILA">
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA">Aseguradora:</span>
                    </div>
                </div>
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" runat="server" id="spDatosAseguradora_CCIM_E">Rimac</span>
                    </div>
                </div>
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA">Especialidad:</span>
                    </div>
                </div>
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" runat="server" id="spDatosEspecialidad_CCIM_E">CARDIOLOGIA</span>
                    </div>
                </div>
            </div>
            <div class="JFILA">
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA">Médico Tratante</span>
                    </div>
                </div>
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" runat="server" id="spDatosMedico_CCIM_E">ALEGRE CHANG</span>
                    </div>
                </div>
            </div>
        </div>
        <div class="JCELDA-2">
            <div class="JFILA">
                <div class="JCELDA-12">
                    <div class="JDIV-CONTROLES">
                        <div style="height:auto; color:Red; font-weight:700; text-align:center; width:100%; border:1px solid Red;">
                            <span id="Span14" runat="server" sp="spDiasHospitalizado">3</span> <span style="color:Red;">Dias Hospitalizados</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="JFILA">
        <div class="JCELDA-12">
            <div class="JDIV-CONTROLES JDIV-GRUPO" style="text-align:center;">
                CONTROL CLINICO - INDICACIONES MEDICAS - ENFERMERAS
            </div>
        </div>
    </div>
    <div class="JFILA">
        <div class="JCELDA-12">
            <div style="border:1px solid Black;width:100%;max-height:200px;overflow-y:auto;">
                <div class="JFILA"> 
                    <div class="JCELDA-1">
                        <img alt="" src="../../Imagenes/Pastilla.png" class="JIMG-GENERAL" />
                    </div>
                    <div class="JCELDA-5">
                        <span class="JETIQUETA" runat="server" id="spDoctorFecha_CCIM1">ALEGRE CHANG | 27/09/2015 10:35 a.m</span>
                    </div>                                                             
                </div>
                <div class="JFILA">
                    <div class="JCELDA-4">
                        <span class="JETIQUETA_2" runat="server" id="spDescripcion1_CCIM1">
                            Ibuprofeno 10MG/2ML x 4 AMP
                        </span>
                    </div>
                    <div class="JCELDA-1">
                        <span class="JETIQUETA_2" runat="server" id="spDescripcion2_CCIM1">
                            500mg
                        </span>
                    </div>
                    <div class="JCELDA-1">
                        <span class="JETIQUETA_2" runat="server" id="spDescripcion3_CCIM1">
                            Oral
                        </span>
                    </div>
                    <div class="JCELDA-1">
                        <span class="JETIQUETA_2" runat="server" id="spDescripcion4_CCIM1">
                            3 horas
                        </span>
                    </div>
                    <div class="JCELDA-1">
                        <span class="JETIQUETA_2" runat="server" id="spDescripcion5_CCIM1">
                            3
                        </span>
                    </div>
                    <div class="JCELDA-1">
                        <span class="JETIQUETA_2" runat="server" id="spDescripcion6_CCIM1">
                            Oral
                        </span>
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-4">
                        <span class="JETIQUETA_2" runat="server" id="Span1">
                            Acarbosa Apotex Comp.
                        </span>
                    </div>
                    <div class="JCELDA-1">
                        <span class="JETIQUETA_2" runat="server" id="Span2">
                            100mg
                        </span>
                    </div>
                    <div class="JCELDA-1">
                        <span class="JETIQUETA_2" runat="server" id="Span3">
                            Oral
                        </span>
                    </div>
                    <div class="JCELDA-1">
                        <span class="JETIQUETA_2" runat="server" id="Span4">
                            4 horas
                        </span>
                    </div>
                    <div class="JCELDA-1">
                        <span class="JETIQUETA_2" runat="server" id="Span5">
                            2
                        </span>
                    </div>
                    <div class="JCELDA-1">
                        <span class="JETIQUETA_2" runat="server" id="Span6">
                            Oral
                        </span>
                    </div>
                </div>
                <div class="JFILA"> 
                    <div class="JCELDA-1">
                        <img alt="" src="../../Imagenes/Pastilla.png" class="JIMG-GENERAL" />
                    </div>
                    <div class="JCELDA-5">
                        <span class="JETIQUETA" runat="server" id="Span13">ALEGRE CHANG | 20/09/2015 09:35 a.m</span>
                    </div>                                                             
                </div>
                <div class="JFILA">
                    <div class="JCELDA-4">
                        <span class="JETIQUETA_2" runat="server" id="Span7">
                            Izitromicina Mabo EFG
                        </span> 
                    </div>
                    <div class="JCELDA-1">
                        <span class="JETIQUETA_2" runat="server" id="Span8">
                            500mg
                        </span>
                    </div>
                    <div class="JCELDA-1">
                        <span class="JETIQUETA_2" runat="server" id="Span9">
                            Oral
                        </span>
                    </div>
                    <div class="JCELDA-1">
                        <span class="JETIQUETA_2" runat="server" id="Span10">
                            8 horas
                        </span>
                    </div>
                    <div class="JCELDA-1">
                        <span class="JETIQUETA_2" runat="server" id="Span11">
                            2
                        </span>
                    </div>
                    <div class="JCELDA-1">
                        <span class="JETIQUETA_2" runat="server" id="Span12">
                            Oral
                        </span>
                    </div>
                </div>
            </div>
        </div>
    </div>

</body>    
</html>
