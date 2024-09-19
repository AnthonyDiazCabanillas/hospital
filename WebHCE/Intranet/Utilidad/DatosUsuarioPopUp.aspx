<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DatosUsuarioPopUp.aspx.vb" Inherits="WebHCE.DatosUsuarioPopUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
     <div class="JFILA">
        <div class="JCELDA-11">
            <div class="JFILA">
                <div class="JCELDA-1">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA">Paciente :</span>
                    </div>
                </div>
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" runat="server" id="spDatosNombreApellido"></span>
                    </div>
                </div>
                <div class="JCELDA-1">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA">Médico :</span> </div>
                </div>
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" runat="server" id="spNombreMedico"></span>
                    </div>
                </div>
                <div class="JCELDA-2">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA">Ingreso x Emergencia :</span> </div>
                </div>
                <div class="JCELDA-2">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" runat="server" id="spDatosIngresoxEmergencia"></span>
                    </div>
                </div>
            </div>

            <%--
            <div class="JFILA">
                <div class="JCELDA-2">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA">Nombres y Apellidos :</span>
                    </div>
                </div>
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" runat="server" id="spDatosNombreApellido"></span>
                    </div>
                </div>
                <div class="JCELDA-2">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA">Edad :</span> </div>
                </div>
                <div class="JCELDA-1">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" runat="server" id="spDatosEdad"></span>
                    </div>
                </div>
                <div class="JCELDA-2">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA">Ingreso x Emergencia :</span> </div>
                </div>
                <div class="JCELDA-2">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" runat="server" id="spDatosIngresoxEmergencia"></span>
                    </div>
                </div>
            </div>

            <div class="JFILA">
                <div class="JCELDA-2">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA" id="Span16">HC:</span>
                    </div>
                </div>
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" runat="server" id="spDatosHC"></span>
                    </div>
                </div>
                <div class="JCELDA-2">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA">Código Atencion:</span>
                    </div>
                </div>
                <div class="JCELDA-1">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" runat="server" id="spDatosCodigoAtencion"></span>
                    </div>
                </div>
                <div class="JCELDA-2">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA">Ingreso Habitación:</span> </div>
                </div>
                <div class="JCELDA-2">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" runat="server" id="spDatosIngresoHabitacion"></span>
                    </div>
                </div>
            </div>
            <div class="JFILA">
                <div class="JCELDA-2">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA">Aseguradora:</span>
                    </div>
                </div>
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" runat="server" id="spDatosAseguradora"></span>
                    </div>
                </div>
                <div class="JCELDA-2">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA">Fono Contacto:</span>
                    </div>
                </div>
                <div class="JCELDA-1">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" runat="server" id="spDatosFonoContacto"></span>
                    </div>
                </div>
                <div class="JCELDA-2">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA" runat="server" id="spSpanDNI" visible="false">DNI:</span>
                    </div>
                </div>
                <div class="JCELDA-1">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" runat="server" id="spDatosDNIPaciente" visible="false"></span>
                    </div>
                </div>
            </div>
            <div class="JFILA">
                <div class="JCELDA-2">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA">Médico:</span>
                    </div>
                </div>
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" runat="server" id="spDatosMedico"></span>
                    </div>
                </div>
                <div class="JCELDA-2">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA">Especialidad:</span>
                    </div>
                </div>
                <div class="JCELDA-4">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" runat="server" id="spDatosEspecialidad"></span>
                    </div>
                </div>
            </div>
            <div class="JFILA">
                <div class="JCELDA-2">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA">Sexo:</span>
                    </div>
                </div>
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" runat="server" id="spDatosSexo"></span>
                    </div>
                </div>
                <div class="JCELDA-2">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA">Lugar de Nacimiento:</span>
                    </div>
                </div>
                <div class="JCELDA-1">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" runat="server" id="spDatosLugarNacimiento"></span>
                    </div>
                </div>
            </div>
        --%>
        </div>
        <%--<div class="JCELDA-1">
            <div class="JFILA">
                <div class="JCELDA-12" style="padding:0">
                    <div class="JDIV-CONTROLES">
                        <div style="height:auto; color:Red; font-weight:700; text-align:center; width:100%; border:1px solid Red;">
                            <span runat="server" id="spDiasHospitalizado"></span> <span style="color:Red;">Dias Hospitalizado</span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="JFILA">
                <div class="JCELDA-12" style="padding:0">
                    <div class="JDIV-CONTROLES">
                        <br />
                        <div style="height:auto; color:Red; font-weight:700; text-align:center; width:100%; border:1px solid Red;" runat="server" visible="false" id="divPresentaAlergia">
                            <span runat="server" id="spPresentaAlergia"></span>
                        </div>
                    </div>
                </div>
            </div>
        </div>--%>
    </div>
</body>
</html>
