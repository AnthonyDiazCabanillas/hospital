Imports Entidades.PatologiaE 'JCAICEDO 20/12/2018 - Se agrego la entidad de patologia
'Imports LogicaNegocio.WsPatologia

Namespace PatologiaLN
    Public Class RcePatologiaGeneralesLN

#Region "EnviarWebPatologia" 'Inicio - Web Services
        'Public Sub EnviarWebPatologia()
        '    Dim oList As DataTable
        '    Dim oRcePatologiaDetE As New RcePatologiaDetE()
        '    Dim oRcePatologiaDetLN As New RcePatologiaDetLN()

        '    Dim oWsUnilabsPatologia As New WsAriasStella.SoaServiceSoapClient '.SoaServiceSoapClient
        '    Dim objSecured As New WsAriasStella.SecuredTockenwebservice
        '    Dim objPreAdmision As New WsAriasStella.ADBE_PreAdmision

        '    'Dim objRcePatologiaDetE As New RcePatologiaDetE()
        '    Dim objRcePatologiaDetPresotorE As New RcePatologiaDetPresotorE
        '    Dim xRpta As String

        '    oRcePatologiaDetE.CodAtencion = ""
        '    oRcePatologiaDetE.Orden = 4
        '    oList = oRcePatologiaDetLN.Sp_RcePatologiaDet_ConsultaV1(oRcePatologiaDetE)

        '    For i As Integer = 0 To oList.Rows.Count - 1
        '        objSecured.UserName = oList.Rows(i).Item("UserName") '"20100162742"
        '        objSecured.Password = oList.Rows(i).Item("Password") '"pmMpLQiMkeo"
        '        objSecured.Sucursal = oList.Rows(i).Item("Sucursal") '"CSFJ"

        '        objPreAdmision.EmpresaProveedor = oList.Rows(i).Item("EmpresaProveedor")
        '        objPreAdmision.UnidadReplicacion = oList.Rows(i).Item("UnidadReplicacion")
        '        objPreAdmision.Servicio = oList.Rows(i).Item("Servicio")

        '        objPreAdmision.FechaAdmision = oList.Rows(i).Item("FechaAdmision")
        '        objPreAdmision.HistorialPaciente = oList.Rows(i).Item("HistorialPaciente")
        '        objPreAdmision.Procedimiento = oList.Rows(i).Item("Procedimiento")
        '        objPreAdmision.Organo = oList.Rows(i).Item("Organo")
        '        objPreAdmision.PacienteAPPaterno = oList.Rows(i).Item("PacienteAPPaterno")
        '        objPreAdmision.PacienteAPMaterno = oList.Rows(i).Item("PacienteAPMaterno")
        '        objPreAdmision.PacienteNombres = oList.Rows(i).Item("PacienteNombre")

        '        objPreAdmision.PacienteTelefono = oList.Rows(i).Item("PacienteTelefono")
        '        objPreAdmision.Sexo = oList.Rows(i).Item("Sexo")
        '        objPreAdmision.Especialidad_Nombre = oList.Rows(i).Item("EspecialidadNombres")
        '        objPreAdmision.TipoDocumento = oList.Rows(i).Item("TipoDocumento")
        '        objPreAdmision.Documento = oList.Rows(i).Item("Documento")
        '        objPreAdmision.PacienteMail = oList.Rows(i).Item("PacienteMail")
        '        objPreAdmision.DxPresuntivo = oList.Rows(i).Item("DxPresuntivo")
        '        objPreAdmision.CMP = oList.Rows(i).Item("CMP")

        '        objPreAdmision.MedicoAPPaterno = oList.Rows(i).Item("MedicoAPPaterno")
        '        objPreAdmision.MedicoAPMaterno = oList.Rows(i).Item("MedicoAPMaterno")

        '        objPreAdmision.FechaLimiteAtencion = oList.Rows(i).Item("FechaLimiteAtencion")
        '        objPreAdmision.CodigoHC = oList.Rows(i).Item("CodigoHC")
        '        objPreAdmision.Numerocama = oList.Rows(i).Item("Numerocama")
        '        objPreAdmision.Tarifario = Convert.ToDecimal(oList.Rows(i).Item("Tarifario"))

        '        objPreAdmision.CodigoOA = oList.Rows(i).Item("CodigoOA")

        '        objPreAdmision.CodigoProsotor = oList.Rows(i).Item("CodPresotor")

        '        objPreAdmision.TipoOrden = oList.Rows(i).Item("TipoOrden")

        '        objPreAdmision.MedicoNombres = oList.Rows(i).Item("MedicoNombres")
        '        objPreAdmision.IdOrdenAtencion = oList.Rows(i).Item("IdOrdenAtencion")
        '        objPreAdmision.CantidadSolicitada = oList.Rows(i).Item("CantidadSolicitada")
        '        objPreAdmision.TipoAtencion = oList.Rows(i).Item("TipoAtencion")
        '        objPreAdmision.Linea = oList.Rows(i).Item("Linea")
        '        objPreAdmision.UsuarioCreacion = oList.Rows(i).Item("UsuarioCreacion")
        '        objPreAdmision.FechaCreacion = oList.Rows(i).Item("FechaCreacion")
        '        objPreAdmision.IpCreacion = oList.Rows(i).Item("IpCreacion")
        '        objPreAdmision.Componente = oList.Rows(i).Item("Componente")
        '        objPreAdmision.FechaNacimiento = oList.Rows(i).Item("FechaNacimiento")
        '        objPreAdmision.ComponenteNombre = oList.Rows(i).Item("ComponenteNombre")

        '        objPreAdmision.Empleadora_Nombre = oList.Rows(i).Item("Empleadora_Nombre")
        '        objPreAdmision.Empleadora_RUC = oList.Rows(i).Item("Empleadora_RUC")
        '        objPreAdmision.Aseguradora_Nombre = oList.Rows(i).Item("Aseguradora_Nombre")
        '        objPreAdmision.Aseguradora_RUC = oList.Rows(i).Item("Aseguradora_RUC")
        '        objPreAdmision.EmpresaProveedor = oList.Rows(i).Item("EmpresaProveedor")

        '        'Enviar IP real del Servidor
        '        xRpta = oWsUnilabsPatologia.PreAdmisionRegistro(objSecured, objSecured.UserName, "192.168.22.47", objSecured.Sucursal, objPreAdmision)

        '        If xRpta.Substring(0, 1) = 0 Then
        '            objRcePatologiaDetPresotorE = New RcePatologiaDetPresotorE
        '            objRcePatologiaDetPresotorE.CodPresotor = oList.Rows(i).Item("CodPresotor")
        '            objRcePatologiaDetPresotorE.NuevoValor = "E"
        '            objRcePatologiaDetPresotorE.Campo = "flg_envioexamen_presotor"
        '            Call New RcePatologiaDetPresotorLN().Sp_RcePatologiaDetPresotor_Update(objRcePatologiaDetPresotorE)
        '        End If
        '    Next i

        'End Sub
        'Fin - Web Services
#End Region

    End Class
End Namespace