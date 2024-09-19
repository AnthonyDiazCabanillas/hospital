''====================================================================================================
'' @Copyright Clinica San Felipe S.A.C. 2024. Todos los derechos reservados.
''====================================================================================================
'' MODIFICACIONES:
'' Version  Fecha       Autor       Requerimiento
'' 1.0      15/01/2024  CRODRIGUEZ  REQ 2023-012268 Filtrar envío correos para interconsultas
''====================================================================================================
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Entidades.InterconsultaE

Namespace InterconsultaAD
    Public Class InterconsultaAD
        Private CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString

        ''' <summary>
        ''' FUNCION QUE OBTIENE LOS DATOS DE INTERCONSULTA POR ATENCION
        ''' </summary>
        ''' <param name="oInterconsultaE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceInterconsulta_Consulta(ByVal oInterconsultaE As InterconsultaE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceInterconsulta_Consulta2", cn)

            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codatencion", oInterconsultaE.Atencion)
            cmd.Parameters.AddWithValue("@orden", oInterconsultaE.Orden)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            Return dt
        End Function


        ''' <summary>
        ''' FUNCION QUE INSERTA REGISTRO DATOS EN LA HISTORIA CLINICA DEL PACIENTE (CAB)
        ''' </summary>
        ''' <param name="oInterconsultaE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceHistoriaClinica_Insert(ByVal oInterconsultaE As InterconsultaE) As InterconsultaE
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceHistoriaClinica_Insert", cn)
            Dim exito As Integer
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@cod_atencion", oInterconsultaE.Atencion)

            Dim parametro As New SqlParameter()
            parametro.ParameterName = "@ide_historia"
            parametro.SqlDbType = SqlDbType.Int
            parametro.Size = 8
            parametro.Direction = ParameterDirection.InputOutput
            parametro.Value = oInterconsultaE.IdeHistoria
            cmd.Parameters.Add(parametro)

            cn.Open()
            exito = cmd.ExecuteNonQuery()
            oInterconsultaE.IdeHistoria = cmd.Parameters("@ide_historia").Value
            cn.Close()

            Return oInterconsultaE
        End Function

        ''' <summary>
        ''' FUNCION PARA LA BUSQUEDA DE INTERCONSULTA
        ''' </summary>
        ''' <param name="oInterconsultaE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceBuscar_Consulta(ByVal oInterconsultaE As InterconsultaE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceBuscar_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@buscar", oInterconsultaE.Nombre)
            cmd.Parameters.AddWithValue("@ide_tipoatencion", oInterconsultaE.TipoDeAtencion)
            cmd.Parameters.AddWithValue("@cod_medico", oInterconsultaE.CodMedico)
            cmd.Parameters.AddWithValue("@cod_atencion", oInterconsultaE.Atencion)
            cmd.Parameters.AddWithValue("@orden", oInterconsultaE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()

            Return dt
        End Function


        ''' <summary>
        ''' FUNCION PARA INSERTAR CABECERA DE INTERCONSULTA
        ''' </summary>
        ''' <param name="oInterconsultaE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceInterconsulta_Insert(ByVal oInterconsultaE As InterconsultaE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceInterconsulta_Insert", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_historia", oInterconsultaE.IdeHistoria)
            cmd.Parameters.AddWithValue("@cod_medicosl", oInterconsultaE.IdeSolicitante)
            cmd.Parameters.AddWithValue("@cod_medicoat", oInterconsultaE.IdeSolicitado)

            Dim oOutParameter1 As New SqlParameter()
            oOutParameter1.ParameterName = "@ide_interconsulta"
            oOutParameter1.SqlDbType = SqlDbType.Int
            oOutParameter1.Size = 8
            oOutParameter1.Direction = ParameterDirection.InputOutput
            oOutParameter1.Value = oInterconsultaE.IdeInterConsulta
            cmd.Parameters.Add(oOutParameter1)
            cn.Open()
            If cmd.ExecuteNonQuery() >= 1 Then
                oInterconsultaE.IdeInterConsulta = cmd.Parameters("@ide_interconsulta").Value
                Return True
            Else
                Return False
            End If
            cn.Close()
        End Function

        ''' <summary>
        ''' FUNCION PARA ACTUALIZAR INTERCONSULTA
        ''' </summary>
        ''' <param name="oInterconsultaE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceInterconsulta_Update(ByVal oInterconsultaE As InterconsultaE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceInterconsulta_Update", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codigo", oInterconsultaE.IdeInterConsulta)
            cmd.Parameters.AddWithValue("@campo", oInterconsultaE.Campo)
            cmd.Parameters.AddWithValue("@valor_nuevo", oInterconsultaE.ValorNuevo)
            cn.Open()
            If cmd.ExecuteNonQuery() >= 1 Then
                Return True
            Else
                Return False
            End If
            cn.Close()
        End Function

        ''' <summary>
        ''' FUNCION PARA ELIMINAR INTERCONSULTA
        ''' </summary>
        ''' <param name="oInterconsultaE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceInterconsulta_Delete(ByVal oInterconsultaE As InterconsultaE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceInterconsulta_Delete", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ideintercon", oInterconsultaE.IdeInterConsulta)
            cn.Open()
            If cmd.ExecuteNonQuery() >= 1 Then
                Return True
            Else
                Return False
            End If
            cn.Close()
        End Function




        Public Function Sp_Medicos_Consulta(ByVal oInterconsultaE As InterconsultaE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_Medicos_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@buscar", oInterconsultaE.Buscar)
            cmd.Parameters.AddWithValue("@key", oInterconsultaE.Key)
            cmd.Parameters.AddWithValue("@numerolineas", oInterconsultaE.Numerolineas)
            cmd.Parameters.AddWithValue("@orden", oInterconsultaE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt
        End Function

        Public Function Ut_EnviarCorreov2(ByVal oInterconsultaE As InterconsultaE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Ut_EnviarCorreov2", cn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@enviara", oInterconsultaE.Enviara)
            cmd.Parameters.AddWithValue("@copiara", oInterconsultaE.Copiara)
            cmd.Parameters.AddWithValue("@copiarh", oInterconsultaE.Copiarh)
            cmd.Parameters.AddWithValue("@asunto", oInterconsultaE.Asunto)
            cmd.Parameters.AddWithValue("@cuerpo", oInterconsultaE.Cuerpo)
            cmd.Parameters.AddWithValue("@file", oInterconsultaE.File)

            cn.Open()
            If cmd.ExecuteNonQuery() >= 1 Then
                Return True
            Else
                Return False
            End If
            cn.Close()
        End Function


        Public Function Ut_EnviarCorreov3(ByVal oInterconsultaE As InterconsultaE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Ut_EnviarCorreov3", cn)

            '--1.0 INI
            'Dim coreAgrupados As String = ""
            'If Trim(oInterconsultaE.Copiara) = "" Then
            '    coreAgrupados = "intensivistasadulto@clinicasanfelipe.com;oquinones@clinicasanfelipe.com;mcontardo@clinicasanfelipe.com"
            'Else
            '    coreAgrupados = oInterconsultaE.Copiara & ";intensivistasadulto@clinicasanfelipe.com;oquinones@clinicasanfelipe.com;mcontardo@clinicasanfelipe.com"
            'End If
            '--1.0 FIN

            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@profile_name", "PostMaster")
            cmd.Parameters.AddWithValue("@recipients", Replace(oInterconsultaE.Enviara, "/", ";"))
            cmd.Parameters.AddWithValue("@copy_recipients", oInterconsultaE.Copiara) '1.0
            cmd.Parameters.AddWithValue("@blind_copy_recipients", oInterconsultaE.Copiarh)
            cmd.Parameters.AddWithValue("@subject", oInterconsultaE.Asunto)
            cmd.Parameters.AddWithValue("@body", oInterconsultaE.Cuerpo)
            cmd.Parameters.AddWithValue("@body_format", "HTML")
            cmd.Parameters.AddWithValue("@importance", "")
            cmd.Parameters.AddWithValue("@sensitivity", "")
            cmd.Parameters.AddWithValue("@file_attachments", "")

            'cmd.Parameters.AddWithValue("@query", oInterconsultaE.File)
            'cmd.Parameters.AddWithValue("@execute_query_database", oInterconsultaE.File)
            'cmd.Parameters.AddWithValue("@attach_query_result_as_file", oInterconsultaE.File)
            'cmd.Parameters.AddWithValue("@query_attachment_filename", oInterconsultaE.File)
            'cmd.Parameters.AddWithValue("@query_result_header", oInterconsultaE.File)
            'cmd.Parameters.AddWithValue("@query_result_width", oInterconsultaE.File)
            'cmd.Parameters.AddWithValue("@query_result_separator", oInterconsultaE.File)
            'cmd.Parameters.AddWithValue("@exclude_query_output", oInterconsultaE.File)
            'cmd.Parameters.AddWithValue("@append_query_error", oInterconsultaE.File)
            'cmd.Parameters.AddWithValue("@query_no_truncate", oInterconsultaE.File)
            'cmd.Parameters.AddWithValue("@query_result_no_padding", oInterconsultaE.File)
            'cmd.Parameters.AddWithValue("@mailitem_id", oInterconsultaE.File)
            'cmd.Parameters.AddWithValue("@from_address", oInterconsultaE.File)
            'cmd.Parameters.AddWithValue("@reply_to", oInterconsultaE.File)
            cn.Open()
            If cmd.ExecuteNonQuery() >= 1 Then
                Return True
            Else
                Return False
            End If
            cn.Close()
        End Function




        Public Function Rp_InterconsultaHM(ByVal oInterconsultaE As InterconsultaE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Rp_InterconsultaHM", cn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@FecIni", oInterconsultaE.FecInicio)
            cmd.Parameters.AddWithValue("@FecFin", oInterconsultaE.FecFin)
            cmd.Parameters.AddWithValue("@orden", oInterconsultaE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt
        End Function
    End Class
End Namespace


