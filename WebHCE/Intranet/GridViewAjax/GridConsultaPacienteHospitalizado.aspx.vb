Imports Entidades.HospitalE
Imports System.Drawing
Imports LogicaNegocio.HospitalLN

Imports Entidades.InterconsultaE
Imports LogicaNegocio.InterconsultaLN

Public Class GridConsultaPacienteHospitalizado
    Inherits System.Web.UI.Page
    Dim oHospitalLN As New HospitalLN
    Dim oHospitalE As New HospitalE
    Dim oInterconsultaE As New InterconsultaE
    Dim oInterconsultaLN As New InterconsultaLN
    Dim nNumeroPagina As Integer
    Dim sPabellon As String
    Dim sPiso As String
    Dim sNombrePacienteBuscar As String
    Dim LaboratorioR As String = ConfigurationManager.AppSettings("LABORATORIO_ROJO").Trim()
    Dim LaboratorioA As String = ConfigurationManager.AppSettings("LABORATORIO_AMARILLO").Trim()
    Dim LaboratorioV As String = ConfigurationManager.AppSettings("LABORATORIO_VERDE").Trim()

    Dim ImagenR As String = ConfigurationManager.AppSettings("IMAGEN_ROJO").Trim()
    Dim ImagenA As String = ConfigurationManager.AppSettings("IMAGEN_AMARILLO").Trim()
    Dim ImagenV As String = ConfigurationManager.AppSettings("IMAGEN_VERDE").Trim()

    Dim InterconsultaR As String = ConfigurationManager.AppSettings("INTERCONSULTA_ROJO").Trim()
    Dim InterconsultaA As String = ConfigurationManager.AppSettings("INTERCONSULTA_AMARILLO").Trim()
    Dim InterconsultaV As String = ConfigurationManager.AppSettings("INTERCONSULTA_VERDE").Trim()

    Dim UsuarioF As String = ConfigurationManager.AppSettings("USUARIO_FEMENINO").Trim()
    Dim UsuarioM As String = ConfigurationManager.AppSettings("USUARIO_MASCULINO").Trim()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ListaPacienteHospitalizado()
        End If
    End Sub

    Public Sub ListaPacienteHospitalizado()
        Try
            If Request.Params("Pabellon") <> Nothing Then
                nNumeroPagina = CType(Request.Params("Pagina").ToString().Trim(), Integer)
                Dim checkA As String = Request.Params("CheckActivo").ToString().Trim()
                If checkA = "S" Then
                    gvConsultaPaciente.PageIndex = (nNumeroPagina - 1)
                    oHospitalE.NombrePaciente = Request.Params("NombrePacienteBuscar").ToString().Trim().ToUpper().Replace("*", " ")
                    oHospitalE.Pabellon = ""
                    oHospitalE.Servicio = ""
                    oHospitalE.Estado = Request.Params("CheckEstado").ToString().Trim() 'JB - 15/06/2020 - NUEVO PARAMETRO PARA MOSTRAR NO ACTIVOS
                    oHospitalE.Orden = 1
                    gvConsultaPaciente.DataSource = oHospitalLN.Sp_RceHospital_Consulta(oHospitalE)
                    gvConsultaPaciente.DataBind()
                Else
                    'gvConsultaPaciente.PageIndex = (nNumeroPagina - 1)
                    'oHospitalE.NombrePaciente = ""
                    'oHospitalE.Pabellon = Request.Params("Pabellon").ToString().Trim()
                    'oHospitalE.Servicio = Request.Params("Servicio").ToString().Trim()
                    'oHospitalE.Estado = Request.Params("CheckEstado").ToString().Trim() 'JB - 15/06/2020 - NUEVO PARAMETRO PARA MOSTRAR NO ACTIVOS
                    'oHospitalE.Orden = 2
                    'gvConsultaPaciente.DataSource = oHospitalLN.Sp_RceHospital_Consulta(oHospitalE)
                    'gvConsultaPaciente.DataBind()

                    gvConsultaPaciente.PageIndex = (nNumeroPagina - 1)
                    oHospitalE.NombrePaciente = ""
                    oHospitalE.Pabellon = Request.Params("Pabellon").ToString().Trim()
                    oHospitalE.Servicio = Request.Params("Servicio").ToString().Trim()
                    oHospitalE.Estado = Request.Params("CheckEstado").ToString().Trim() 'JB - 15/06/2020 - NUEVO PARAMETRO PARA MOSTRAR NO ACTIVOS
                    Dim dt As New DataTable()
                    dt.Columns.Add("codatencion")
                    oHospitalE.Tabla = dt
                    oHospitalE.Orden = 1
                    Dim TablaGen, TablaAten As New DataTable()
                    TablaGen = oHospitalLN.Sp_RceHospital_ConsultaV2(oHospitalE)
                    TablaAten.Columns.Add("codatencion")

                    If TablaGen.Rows.Count > 0 Then

                        For index = 0 To TablaGen.Rows.Count - 1
                            TablaAten.Rows.Add(TablaGen.Rows(index)("codatencion").ToString().Trim())
                        Next

                        oHospitalE.Tabla = TablaAten
                        oHospitalE.Orden = 2
                        gvConsultaPaciente.DataSource = oHospitalLN.Sp_RceHospital_ConsultaV2(oHospitalE)
                        gvConsultaPaciente.DataBind()
                    Else
                        oHospitalE.Tabla = TablaAten
                        oHospitalE.Orden = 2
                        gvConsultaPaciente.DataSource = oHospitalLN.Sp_RceHospital_ConsultaV2(oHospitalE)
                        gvConsultaPaciente.DataBind()
                    End If
                    
                End If
            Else

            End If
        Catch ex As Exception
            hfMensajeError.Value = ex.Message.ToString()
        End Try

    End Sub

    Protected Sub gvConsultaPaciente_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvConsultaPaciente.PageIndexChanging
        gvConsultaPaciente.PageIndex = e.NewPageIndex
        ListaPacienteHospitalizado()
    End Sub
    ''' <summary>
    ''' APLICANDO COLORES E IMAGENES DEPENDIENDO DE LOS ESTADOS.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvConsultaPaciente_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvConsultaPaciente.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(12).Text.Trim().Replace("&nbsp;", "") <> "" Then
                e.Row.Cells(1).ForeColor = System.Drawing.ColorTranslator.FromHtml("#FA0000")
                e.Row.Cells(1).ForeColor = System.Drawing.ColorTranslator.FromHtml("#FA0000")
                e.Row.Cells(2).ForeColor = System.Drawing.ColorTranslator.FromHtml("#FA0000")
                e.Row.Cells(3).ForeColor = System.Drawing.ColorTranslator.FromHtml("#FA0000")
                e.Row.Cells(4).ForeColor = System.Drawing.ColorTranslator.FromHtml("#FA0000")
                e.Row.Cells(5).ForeColor = System.Drawing.ColorTranslator.FromHtml("#FA0000")
                e.Row.Cells(6).ForeColor = System.Drawing.ColorTranslator.FromHtml("#FA0000")
                e.Row.Cells(7).ForeColor = System.Drawing.ColorTranslator.FromHtml("#FA0000")
                e.Row.Cells(8).ForeColor = System.Drawing.ColorTranslator.FromHtml("#FA0000")
                e.Row.Cells(9).ForeColor = System.Drawing.ColorTranslator.FromHtml("#FA0000")
                e.Row.Cells(10).ForeColor = System.Drawing.ColorTranslator.FromHtml("#FA0000")
                e.Row.Cells(11).ForeColor = System.Drawing.ColorTranslator.FromHtml("#FA0000")
                e.Row.Cells(12).ForeColor = System.Drawing.ColorTranslator.FromHtml("#FA0000")
            End If

            Dim imgPaciente As New System.Web.UI.HtmlControls.HtmlImage
            If e.Row.Cells(13).Text.Trim() = "M" Then
                imgPaciente = CType(e.Row.Cells(1).FindControl("imgPacienteTabla"), System.Web.UI.HtmlControls.HtmlImage)
                imgPaciente.Src = "../Imagenes/" + UsuarioM + ""
                imgPaciente.Width = 35
                imgPaciente.Height = 35
            Else
                imgPaciente = CType(e.Row.Cells(1).FindControl("imgPacienteTabla"), System.Web.UI.HtmlControls.HtmlImage)
                imgPaciente.Src = "../Imagenes/" + UsuarioF + ""
                imgPaciente.Width = 35
                imgPaciente.Height = 35
            End If

            'when 'T' then '3'		-- Verde
            'when 'P' then '2'		-- Ambar
            'when 'G' then '1'		-- Rojo
            Dim img As New System.Web.UI.HtmlControls.HtmlImage
            If e.Row.Cells(14).Text.Trim() = "T" Then
                img = CType(e.Row.Cells(7).FindControl("imgLaboratorioTabla"), System.Web.UI.HtmlControls.HtmlImage)
                img.Src = "../Imagenes/" + LaboratorioV + ""
                img.Width = 35
                img.Height = 35
            ElseIf e.Row.Cells(14).Text.Trim() = "N" Then 'P
                img = CType(e.Row.Cells(7).FindControl("imgLaboratorioTabla"), System.Web.UI.HtmlControls.HtmlImage)
                img.Src = "../Imagenes/" + LaboratorioA + ""
                img.Width = 35
                img.Height = 35
            ElseIf e.Row.Cells(14).Text.Trim() = "A" Or e.Row.Cells(14).Text.Trim() = "G" Then 'jb - 13/07/2020 - se cambia de estado G -> A
                img = CType(e.Row.Cells(7).FindControl("imgLaboratorioTabla"), System.Web.UI.HtmlControls.HtmlImage)
                img.Src = "../Imagenes/" + LaboratorioR + ""
                img.Width = 35
                img.Height = 35
            Else
                img = CType(e.Row.Cells(7).FindControl("imgLaboratorioTabla"), System.Web.UI.HtmlControls.HtmlImage)
                img.Src = ""
            End If
            

            Dim imgImagen As New System.Web.UI.HtmlControls.HtmlImage
            If e.Row.Cells(15).Text.Trim() = "T" Then
                imgImagen = CType(e.Row.Cells(8).FindControl("imgImagenTabla"), System.Web.UI.HtmlControls.HtmlImage)
                imgImagen.Src = "../Imagenes/" + ImagenV + ""
                imgImagen.Width = 35
                imgImagen.Height = 35
            ElseIf e.Row.Cells(15).Text.Trim() = "P" Then
                imgImagen = CType(e.Row.Cells(8).FindControl("imgImagenTabla"), System.Web.UI.HtmlControls.HtmlImage)
                imgImagen.Src = "../Imagenes/" + ImagenA + ""
                imgImagen.Width = 35
                imgImagen.Height = 35
            ElseIf e.Row.Cells(15).Text.Trim() = "G" Then
                imgImagen = CType(e.Row.Cells(8).FindControl("imgImagenTabla"), System.Web.UI.HtmlControls.HtmlImage)
                imgImagen.Src = "../Imagenes/" + ImagenR + ""
                imgImagen.Width = 35
                imgImagen.Height = 35
            Else
                imgImagen = CType(e.Row.Cells(8).FindControl("imgImagenTabla"), System.Web.UI.HtmlControls.HtmlImage)
                imgImagen.Src = ""
            End If
            
            'P - ROJO
            'T - VERDE
            'A- AMARILLO
            Dim imgIntercon As New System.Web.UI.HtmlControls.HtmlImage
            If e.Row.Cells(17).Text.Trim() = "P" Then
                imgIntercon = CType(e.Row.Cells(9).FindControl("imgInterconTabla"), System.Web.UI.HtmlControls.HtmlImage)
                imgIntercon.Src = "../Imagenes/" + InterconsultaR + ""
                imgIntercon.Width = 35
                imgIntercon.Height = 35
            ElseIf e.Row.Cells(17).Text.Trim() = "T" Then
                imgIntercon = CType(e.Row.Cells(9).FindControl("imgInterconTabla"), System.Web.UI.HtmlControls.HtmlImage)
                imgIntercon.Src = "../Imagenes/" + InterconsultaV + ""
                imgIntercon.Width = 35
                imgIntercon.Height = 35
            ElseIf e.Row.Cells(17).Text.Trim() = "A" Then
                imgIntercon = CType(e.Row.Cells(9).FindControl("imgInterconTabla"), System.Web.UI.HtmlControls.HtmlImage)
                imgIntercon.Src = "../Imagenes/" + InterconsultaA + ""
                imgIntercon.Width = 35
                imgIntercon.Height = 35
            Else
                imgIntercon = CType(e.Row.Cells(9).FindControl("imgInterconTabla"), System.Web.UI.HtmlControls.HtmlImage)
                imgIntercon.Src = ""
            End If
            

            Dim imgEnfermera As New System.Web.UI.HtmlControls.HtmlImage
            If e.Row.Cells(18).Text.Trim().Replace("&nbsp;", "") <> "" And e.Row.Cells(12).Text.Trim().Replace("&nbsp;", "") = "" Then
                imgImagen = CType(e.Row.Cells(10).FindControl("imgEnfermetaTabla"), System.Web.UI.HtmlControls.HtmlImage)
                imgImagen.Src = "../Imagenes/Enfermera.png"
            Else
                imgImagen = CType(e.Row.Cells(10).FindControl("imgEnfermetaTabla"), System.Web.UI.HtmlControls.HtmlImage)
                imgImagen.Src = ""
            End If

        End If
    End Sub


    ''' <summary>
    ''' EVENTO QUE SE DISPARA CUANDO SELECCIONA UN PACIENTE, GUARDA LOS DATOS EN HistoriaClinicaCab
    ''' </summary>
    ''' <param name="CodigoAtencion"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function RegistraHistoriaClinica(ByVal CodigoAtencion As String) As String
        Dim pagina As New GridConsultaPacienteHospitalizado()
        Dim resultado As String
        resultado = pagina.RegistrarHistoriaClinica(CodigoAtencion)
        Return resultado
    End Function

    Public Function RegistrarHistoriaClinica(ByVal CodigoAtencion As String) As String
        Try
            Dim dt, dt2 As New DataTable()
            oHospitalE.NombrePaciente = CodigoAtencion
            oHospitalE.Pabellon = ""
            oHospitalE.Servicio = ""
            oHospitalE.Estado = ""
            oHospitalE.Orden = 3
            dt = oHospitalLN.Sp_RceHospital_Consulta(oHospitalE)

            If dt.Rows.Count > 0 Then
                If dt.Rows(0)("activo").ToString().Trim() = "N" Or dt.Rows(0)("fechaaltamedica").ToString().Trim() <> "" Then 'si ya fue dado de alta o esta como "no activo"

                    oHospitalE.Orden = 4
                    dt2 = oHospitalLN.Sp_RceHospital_Consulta(oHospitalE) 'traere datos si tiene historia clinica
                    If dt2.Rows.Count = 0 Then 'si no trae nada mostrara mensaje
                        Return "Este registro no tiene historia clinica"
                    Else 'sino traere el id historia

                        If (String.IsNullOrWhiteSpace(Session(sCodigoAtencion))) Then
                        Else
                            Return "Se ha detectado una sesion abierta. Cierre las otras pestañas y/o actualice la pestaña (F5)"
                        End If
                        If (String.IsNullOrWhiteSpace(Session(sCodigoAtencion_Auxiliar))) Then
                        Else
                            Return "Se ha detectado una sesion abierta. Cierre las otras pestañas y/o actualice la pestaña (F5)"
                        End If

                        Session(sCodigoAtencion) = CodigoAtencion
                        Session(sCodigoAtencion_Auxiliar) = CodigoAtencion '21/06/2016
                        Session(sIdeHistoria) = dt2.Rows(0)("ide_historia").ToString().Trim()
                        Return ""
                    End If
                Else

                    If (String.IsNullOrWhiteSpace(Session(sCodigoAtencion))) Then
                    Else
                        Return "Se ha detectado una sesion abierta. Cierre las otras pestañas y/o actualice la pestaña (F5)"
                    End If
                    If (String.IsNullOrWhiteSpace(Session(sCodigoAtencion_Auxiliar))) Then
                    Else
                        Return "Se ha detectado una sesion abierta. Cierre las otras pestañas y/o actualice la pestaña (F5)"
                    End If

                    oInterconsultaE.Atencion = CodigoAtencion
                    Session(sCodigoAtencion) = CodigoAtencion

                    Session(sCodigoAtencion_Auxiliar) = CodigoAtencion '21/06/2016
                    oInterconsultaE = oInterconsultaLN.Sp_RceHistoriaClinica_Insert(oInterconsultaE)

                        If oInterconsultaE.IdeHistoria <> 0 And oInterconsultaE.IdeHistoria <> Nothing And oInterconsultaE.IdeHistoria <> -1 Then
                            Session(sIdeHistoria) = oInterconsultaE.IdeHistoria
                            Return ""
                        ElseIf oInterconsultaE.IdeHistoria = -1 Then
                            Return "La atencion no tiene especialidad."
                        Else
                            Return "No se pudo Iniciar el acceso al sistema - Sp_RceHistoriaClinica_Insert"
                        End If
                    End If
                End If
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try



        



        'oInterconsultaE.Atencion = CodigoAtencion
        'Session(sCodigoAtencion) = CodigoAtencion
        'Session(sCodigoAtencion_Auxiliar) = CodigoAtencion '21/06/2016
        'oInterconsultaE = oInterconsultaLN.Sp_RceHistoriaClinica_Insert(oInterconsultaE)

        'If oInterconsultaE.IdeHistoria <> 0 And oInterconsultaE.IdeHistoria <> Nothing Then
        '    Session(sIdeHistoria) = oInterconsultaE.IdeHistoria
        '    Return ""
        'Else
        '    Return "No se pudo Iniciar el acceso al sistema - Sp_RceHistoriaClinica_Insert"
        'End If

    End Function

End Class