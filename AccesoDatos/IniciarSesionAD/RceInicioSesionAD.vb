'----------------------------------------------------------------
'Version    Fecha		    Autor		REQUERIMIENTO			Comentario
'1.0        11/11/2024  	GLLUNCOR	REQ 2024-026424			Restringir acceso a los HC Hospital por médico
'----------------------------------------------------------------
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Entidades.InicioSesionE


Namespace InicioSesionAD
    Public Class RceInicioSesionAD
        Private CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString

        ''' <summary>
        ''' FUNCION PARA VALIDAR EL INICIO DE SESSION
        ''' </summary>
        ''' <param name="oRceInicioSesionE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RCEAmbulatorio_IniciarSesion(ByVal oRceInicioSesionE As RceInicioSesionE) As RceInicioSesionE
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RCEAmbulatorio_IniciarSesion", cn)
            Dim exito As Integer

            cmd.CommandType = System.Data.CommandType.StoredProcedure
            'Parametros del Store
            cmd.Parameters.AddWithValue("@doc_identidad", oRceInicioSesionE.DocIdentidad)
            cmd.Parameters.AddWithValue("@clave", oRceInicioSesionE.Clave)
            cmd.Parameters.AddWithValue("@dsc_ip_pc", oRceInicioSesionE.DscIpPC)
            cmd.Parameters.AddWithValue("@dsc_pc_name", oRceInicioSesionE.DscPcName)
            cmd.Parameters.AddWithValue("@sede", oRceInicioSesionE.Sede)

            Dim oOutParameter1 As New SqlParameter("@mensaje", SqlDbType.VarChar, 100, ParameterDirection.InputOutput)
            oOutParameter1.ParameterName = "@mensaje"
            oOutParameter1.SqlDbType = SqlDbType.VarChar
            oOutParameter1.Size = 100
            oOutParameter1.Direction = ParameterDirection.InputOutput
            oOutParameter1.Value = oRceInicioSesionE.Mensaje
            cmd.Parameters.Add(oOutParameter1)

            Dim oOutParameter2 As New SqlParameter()
            oOutParameter2.ParameterName = "@ide_sesion"
            oOutParameter2.Direction = ParameterDirection.InputOutput
            oOutParameter2.SqlDbType = SqlDbType.VarChar
            oOutParameter2.Size = 25
            oOutParameter2.Value = oRceInicioSesionE.IdeSesion
            cmd.Parameters.Add(oOutParameter2)

            Dim oOutParameter3 As New SqlParameter()
            oOutParameter3.ParameterName = "@login"
            oOutParameter3.Direction = ParameterDirection.InputOutput
            oOutParameter3.SqlDbType = SqlDbType.VarChar
            oOutParameter3.Size = 25
            oOutParameter3.Value = oRceInicioSesionE.Login
            cmd.Parameters.Add(oOutParameter3)

            Dim oOutParameter4 As New SqlParameter()
            oOutParameter4.ParameterName = "@cod_user"
            oOutParameter4.Direction = ParameterDirection.InputOutput
            oOutParameter4.SqlDbType = SqlDbType.Int
            oOutParameter4.Size = 8
            oOutParameter4.Value = oRceInicioSesionE.CodUser
            cmd.Parameters.Add(oOutParameter4)

            Dim oOutParameter5 As New SqlParameter()
            oOutParameter5.ParameterName = "@cod_medico"
            oOutParameter5.Direction = ParameterDirection.InputOutput
            oOutParameter5.SqlDbType = SqlDbType.Char
            oOutParameter5.Size = 8
            oOutParameter5.Value = oRceInicioSesionE.CodMedico
            cmd.Parameters.Add(oOutParameter5)

            cn.Open()
            exito = cmd.ExecuteNonQuery()
            'Obtener Resultados del Store
            oRceInicioSesionE.Mensaje = cmd.Parameters("@mensaje").Value
            oRceInicioSesionE.IdeSesion = cmd.Parameters("@ide_sesion").Value
            oRceInicioSesionE.Login = cmd.Parameters("@login").Value
            oRceInicioSesionE.CodUser = cmd.Parameters("@cod_user").Value
            oRceInicioSesionE.CodMedico = cmd.Parameters("@cod_medico").Value
            cn.Close()

            Return oRceInicioSesionE
        End Function

        ''' <summary>
        ''' FUNCION PARA EL ACCESO A ENFERMERAS
        ''' </summary>
        ''' <param name="oRceInicioSesionE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_Usuarios_IniciarSesion(ByVal oRceInicioSesionE As RceInicioSesionE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_Usuarios_IniciarSesion", cn)

            cmd.CommandType = System.Data.CommandType.StoredProcedure
            'Parametros del Store
            cmd.Parameters.AddWithValue("@CodigoUsuario", oRceInicioSesionE.CodigoUsuario)
            cmd.Parameters.AddWithValue("@Password", oRceInicioSesionE.Clave)
            cmd.Parameters.AddWithValue("@dsc_ip_pc", oRceInicioSesionE.DscIpPC)
            cmd.Parameters.AddWithValue("@dsc_pc_name", oRceInicioSesionE.DscPcName)
            cmd.Parameters.AddWithValue("@Orden", oRceInicioSesionE.Orden)
            cn.Open()
            Dim tabla As New DataTable()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(tabla)

            cn.Close()

            Return tabla
        End Function


        ''' <summary>
        ''' FUNCION QUE CARGA LOS PERMISOS
        ''' </summary>
        ''' <param name="oRceInicioSesionE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceValidaPermiso(ByVal oRceInicioSesionE As RceInicioSesionE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceValidaPermiso", cn)

            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@cod_user", oRceInicioSesionE.CodUser)
            cmd.Parameters.AddWithValue("@ide_opcion_sup", oRceInicioSesionE.IdeOpcionSupe)
            cmd.Parameters.AddWithValue("@ide_modulo", oRceInicioSesionE.IdeModulo)
            cmd.Parameters.AddWithValue("@cod_opcion", oRceInicioSesionE.CodOpcion)
            cmd.Parameters.AddWithValue("@orden", oRceInicioSesionE.Orden)
            cn.Open()
            Dim tabla As New DataTable()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(tabla)
            cn.Close()
            Return tabla

            '@cod_user = 337 (usuario) 
            '@ide_opcion_sup = 0 
            '@ide_modulo = 84 (modulo o formulario) 
            '@cod_opcion = '' 
            '@orden = 3 
        End Function

        Public Function Sp_RceInicioSesion_Update(ByVal oRceInicioSesionE As RceInicioSesionE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceInicioSesion_Update", cn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@idesesion", oRceInicioSesionE.IdeSesion)
            cmd.Parameters.AddWithValue("@campo", oRceInicioSesionE.Campo)
            cmd.Parameters.AddWithValue("@valor", oRceInicioSesionE.Valor)
            cn.Open()
            If cmd.ExecuteNonQuery() >= 1 Then
                Return True
            Else
                Return False
            End If
            cn.Close()
        End Function


        Public Function Sp_RCEAmbulatorio_ObtenerClave(ByVal oRceInicioSesionE As RceInicioSesionE) As RceInicioSesionE
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RCEAmbulatorio_ObtenerClave", cn)
            Dim exito As Integer

            cmd.CommandType = System.Data.CommandType.StoredProcedure
            'Parametros del Store
            cmd.Parameters.AddWithValue("@login", oRceInicioSesionE.Login)

            Dim oOutParameter1 As New SqlParameter("@mensaje", SqlDbType.VarChar, 200, ParameterDirection.InputOutput)
            oOutParameter1.ParameterName = "@mensaje"
            oOutParameter1.SqlDbType = SqlDbType.VarChar
            oOutParameter1.Size = 200
            oOutParameter1.Direction = ParameterDirection.InputOutput
            oOutParameter1.Value = oRceInicioSesionE.Mensaje
            cmd.Parameters.Add(oOutParameter1)

            cn.Open()
            exito = cmd.ExecuteNonQuery()
            'Obtener Resultados del Store
            oRceInicioSesionE.Mensaje = cmd.Parameters("@mensaje").Value
            cn.Close()

            Return oRceInicioSesionE
        End Function

        Public Function Sp_RCEAmbulatorio_ObtenerRutaApiPassword() As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RCEAmbulatorio_ObtenerRutaApiPassword", cn)

            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cn.Open()
            Dim tabla As New DataTable()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(tabla)
            cn.Close()
            Return tabla
        End Function

        Public Function Sp_RCESegUsuario_Update(ByVal oRceInicioSesionE As RceInicioSesionE) As RceInicioSesionE
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RCESegUsuario_Update", cn)
            Dim exito As Integer

            cmd.CommandType = System.Data.CommandType.StoredProcedure
            'Parametros del Store
            cmd.Parameters.AddWithValue("@ide_sesion", oRceInicioSesionE.IdeSesion)
            cmd.Parameters.AddWithValue("@campo", oRceInicioSesionE.Campo)
            cmd.Parameters.AddWithValue("@valor", oRceInicioSesionE.Valor)
            cmd.Parameters.AddWithValue("@clave", oRceInicioSesionE.Clave)
            cmd.Parameters.AddWithValue("@tipoSistema", oRceInicioSesionE.TipoSistema)

            Dim oOutParameter1 As New SqlParameter("@mensaje", SqlDbType.VarChar, 200, ParameterDirection.InputOutput)
            oOutParameter1.ParameterName = "@mensaje"
            oOutParameter1.SqlDbType = SqlDbType.VarChar
            oOutParameter1.Size = 200
            oOutParameter1.Direction = ParameterDirection.InputOutput
            oOutParameter1.Value = oRceInicioSesionE.Mensaje
            cmd.Parameters.Add(oOutParameter1)

            cn.Open()
            exito = cmd.ExecuteNonQuery()
            'Obtener Resultados del Store
            oRceInicioSesionE.Mensaje = cmd.Parameters("@mensaje").Value
            cn.Close()

            Return oRceInicioSesionE
        End Function


        Public Function Sp_Rcelogs_Insert(ByVal oRceInicioSesionE As RceInicioSesionE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_Rcelogs_Insert", cn)
            cmd.CommandType = CommandType.StoredProcedure
            Dim oOutParameter1 As New SqlParameter("@ide_logs", SqlDbType.Int, 8, ParameterDirection.InputOutput)
            oOutParameter1.ParameterName = "@ide_logs"
            oOutParameter1.SqlDbType = SqlDbType.Int
            oOutParameter1.Size = 8
            oOutParameter1.Direction = ParameterDirection.InputOutput
            oOutParameter1.Value = oRceInicioSesionE.IdeLog
            cmd.Parameters.Add(oOutParameter1)
            cmd.Parameters.AddWithValue("@ide_historia", oRceInicioSesionE.IdeHistoria)
            cmd.Parameters.AddWithValue("@ide_usuario", oRceInicioSesionE.CodUser)
            cmd.Parameters.AddWithValue("@formulario", oRceInicioSesionE.Formulario)
            cmd.Parameters.AddWithValue("@control", oRceInicioSesionE.Control)
            cmd.Parameters.AddWithValue("@sesion", oRceInicioSesionE.IdeSesion)
            cmd.Parameters.AddWithValue("@dsc_pc", oRceInicioSesionE.DscPcName)
            cmd.Parameters.AddWithValue("@dsc_log", oRceInicioSesionE.DscLog)

            cn.Open()
            If cmd.ExecuteNonQuery() >= 1 Then
                Return True
            Else
                Return False
            End If
            cn.Close()
        End Function



        Public Function Sp_Usuarios_IniciarSesion2(ByVal oRceInicioSesionE As RceInicioSesionE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_Usuarios_IniciarSesion2", cn)

            cmd.CommandType = System.Data.CommandType.StoredProcedure
            'Parametros del Store
            cmd.Parameters.AddWithValue("@doc_identidad", oRceInicioSesionE.DocIdentidad)
            cmd.Parameters.AddWithValue("@CodigoUsuario", oRceInicioSesionE.CodigoUsuario)
            cmd.Parameters.AddWithValue("@clave", oRceInicioSesionE.Clave)
            cmd.Parameters.AddWithValue("@orden", oRceInicioSesionE.Orden)
            '1.0 INI
            cmd.Parameters.AddWithValue("@idhistoria", oRceInicioSesionE.IdeHistoria)
            '1.0 FIN
            cn.Open()
            Dim tabla As New DataTable()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(tabla)

            cn.Close()

            Return tabla
        End Function

        Public Function sp_seglogclave_sel_clinica(ByVal oRceInicioSesionE As RceInicioSesionE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("sp_seglogclave_sel_clinica", cn)

            cmd.CommandType = System.Data.CommandType.StoredProcedure
            'Parametros del Store
            cmd.Parameters.AddWithValue("@ideusuario", oRceInicioSesionE.Ide_Usuario)
            cmd.Parameters.AddWithValue("@txtclave", oRceInicioSesionE.Txtclave)
            cmd.Parameters.AddWithValue("@tipoSistema", oRceInicioSesionE.TipoSistema)

            cn.Open()
            Dim tabla As New DataTable()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(tabla)

            cn.Close()

            Return tabla
        End Function

        Public Function Sp_ParamSeguridad_Sel() As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_ParamSeguridad_Sel", cn)

            cmd.CommandType = System.Data.CommandType.StoredProcedure
            'Parametros del Store

            cn.Open()
            Dim tabla As New DataTable()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(tabla)

            cn.Close()

            Return tabla
        End Function

        '1.1 INI
        Public Function Sp_RCEAmbulatorio_ActualizaSesionBloqueo(ByVal oRceInicioSesionE As RceInicioSesionE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RCEAmbulatorio_updatesesionlboqueo", cn)

            cmd.CommandType = System.Data.CommandType.StoredProcedure
            'Parametros del Store
            cmd.Parameters.AddWithValue("@cod_user", oRceInicioSesionE.CodUser)
            cmd.Parameters.AddWithValue("@cod_medico", oRceInicioSesionE.CodMedico)

            cn.Open()
            If cmd.ExecuteNonQuery() >= 1 Then
                Return True
            Else
                Return False
            End If
            cn.Close()
        End Function
        '1.1 FIN

        '1.1 INI
        Public Function Sp_RCEAmbulatorio_bloqueoAsistencial(ByVal oRceInicioSesionE As RceInicioSesionE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RCEAmbulatorio_bloqueoAsistencial", cn)

            cmd.CommandType = System.Data.CommandType.StoredProcedure
            'Parametros del Store
            cmd.Parameters.AddWithValue("@ide_usuario", oRceInicioSesionE.Ide_Usuario)

            cn.Open()
            If cmd.ExecuteNonQuery() >= 1 Then
                Return True
            Else
                Return False
            End If
            cn.Close()
        End Function
        '1.1 FIN

    End Class
End Namespace


