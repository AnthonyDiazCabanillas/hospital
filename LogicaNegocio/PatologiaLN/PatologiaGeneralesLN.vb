Imports Entidades.PatologiaE 'JCAICEDO 20/12/2018 - Se agrego la entidad de patologia
Imports LogicaNegocio.WsAriasStella

Namespace PatologiaLN

    Public Class PatologiaGeneralesLN

#Region "EnviarWebPatologia(pIdePatologiaDet)" 'Inicio - Web Services
        'Public Sub EnviarWebPatologia(ByVal pIdePatologiaDet As Integer)
        '    Dim oList As DataTable
        '    Dim oRcePatologiaDetE As New RcePatologiaDetE()
        '    Dim oRcePatologiaDetLN As New RcePatologiaDetLN()

        '    Dim oWsUnilabsPatologia As New SoaServiceSoapClient '.SoaServiceSoapClient
        '    Dim objSecured As New SecuredTockenwebservice
        '    Dim objPreAdmision As New ADBE_PreAdmision

        '    Dim objRcePatologiaDetE As New RcePatologiaDetE()
        '    Dim xRpta As String

        '    oRcePatologiaDetE.CodAtencion = ""
        '    oRcePatologiaDetE.IdePatologiaDet = pIdePatologiaDet
        '    oRcePatologiaDetE.Orden = 4
        '    oList = oRcePatologiaDetLN.Sp_RcePatologiaDet_ConsultaV1(oRcePatologiaDetE)

        '    For i As Integer = 0 To oList.Rows.Count - 1

        '        objRcePatologiaDetE = New RcePatologiaDetE
        '        objRcePatologiaDetE.IdePatologiaDet = pIdePatologiaDet
        '        objRcePatologiaDetE.NuevoValor = ""
        '        objRcePatologiaDetE.Campo = "flg_enviarexamen"
        '        oRcePatologiaDetLN.Sp_RcePatologiaDet_Update(objRcePatologiaDetE)

        '        objSecured.UserName = oList.Rows(i).Item("UserName") '"20100162742"
        '        objSecured.Password = oList.Rows(i).Item("Password") '"pmMpLQiMkeo"
        '        objSecured.Sucursal = oList.Rows(i).Item("Sucursal") '"CSFJ"

        '        'objRcePatologiaDetE = oList.Rows(0).Table

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



        '        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        '        'objPreAdmision.EmpresaProveedor = "20100162742"
        '        'objPreAdmision.Acceso = "fYpiB/d+oH+R7gq+ePfWNQ=="
        '        'objPreAdmision.Servicio = "PATOLOGIA"
        '        'objPreAdmision.UnidadReplicacion = "CSFJ"

        '        'objPreAdmision.CodigoOA = "0000001329"
        '        'objPreAdmision.Tarifario = 0
        '        'objPreAdmision.Documento = "08068398"
        '        'objPreAdmision.TipoDocumento = "01"
        '        'objPreAdmision.PacienteNombres = "ALFREDO"
        '        'objPreAdmision.PacienteAPMaterno = "LAY"
        '        'objPreAdmision.PacienteAPPaterno = "LIPE"
        '        'objPreAdmision.PacienteMail = ""
        '        'objPreAdmision.PacienteTelefono = "2139422"
        '        'objPreAdmision.Sexo = "M"
        '        'objPreAdmision.HistorialPaciente = "NINGUN EXAMEN"
        '        'objPreAdmision.Aseguradora_Nombre = "NO ASEGURADO"
        '        'objPreAdmision.Aseguradora_RUC = "0"
        '        'objPreAdmision.Componente = "210201"
        '        'objPreAdmision.ComponenteNombre = "BIOPSIA POR UNIDAD TISULAR"
        '        'objPreAdmision.CantidadSolicitada = 1
        '        'objPreAdmision.CMP = "5861"
        '        'objPreAdmision.MedicoAPMaterno = "MAURTUA"
        '        'objPreAdmision.MedicoAPPaterno = "SAFRA"
        '        'objPreAdmision.MedicoNombres = "CHRISTIAN"
        '        'objPreAdmision.Numerocama = ""
        '        'objPreAdmision.TipoAtencion = 1
        '        'objPreAdmision.IdOrdenAtencion = 501
        '        'objPreAdmision.Linea = 1
        '        'objPreAdmision.Organo = "PRÓSTATA"
        '        'objPreAdmision.Empleadora_RUC = ""
        '        'objPreAdmision.Empleadora_Nombre = ""
        '        'objPreAdmision.DxPresuntivo = "DESCARTE"
        '        'objPreAdmision.Especialidad_Nombre = "SIN ESPECIALIDAD"
        '        'objPreAdmision.Procedimiento = "TERCIO MEDIO DERECHO."
        '        'objPreAdmision.TipoOrden = "URG"
        '        'objPreAdmision.CodigoHC = "08068398"
        '        'objPreAdmision.FechaNacimiento = Convert.ToDateTime("1946/05/13")
        '        'objPreAdmision.FechaLimiteAtencion = Convert.ToDateTime("2018/10/30")

        '        'Enviar IP real del Servidor
        '        xRpta = oWsUnilabsPatologia.PreAdmisionRegistro(objSecured, objSecured.UserName, "192.168.22.47", objSecured.Sucursal, objPreAdmision)
        '    Next i

        'End Sub
        'Fin - Web Services
#End Region

    End Class

End Namespace