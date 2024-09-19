Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Entidades.JuntaMedicaE

Namespace JuntaMedicaAD
    Public Class RceJuntaMedicaAD
        Private CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString

        Public Function Sp_RceJuntaMedica_Insert(ByVal oRceJuntaMedicaE As RceJuntaMedicaE) As Integer
            Dim cn As New SqlConnection(CnnBD)
            Dim exito As Integer
            Dim cmd As New SqlCommand("Sp_RceJuntaMedica_Insert", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@cod_atencion", oRceJuntaMedicaE.CodAtencion)
            cmd.Parameters.AddWithValue("@cod_medico", oRceJuntaMedicaE.CodMedico)
            cmd.Parameters.AddWithValue("@dsc_juntamedica", oRceJuntaMedicaE.DscJuntaMedica)
            cmd.Parameters.AddWithValue("@usr_registra", oRceJuntaMedicaE.IdUsuario)
            cn.Open()
            exito = cmd.ExecuteScalar()

            Return exito
        End Function

        Public Function Sp_RceJuntaMedica_Consulta(ByVal oRceJuntaMedicaE As RceJuntaMedicaE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceJuntaMedica_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@cod_atencion", oRceJuntaMedicaE.CodAtencion)
            cmd.Parameters.AddWithValue("@ide_juntamedica", oRceJuntaMedicaE.IdeJuntaMedica)
            cmd.Parameters.AddWithValue("@fec_juntamedica", oRceJuntaMedicaE.FecJuntaMedica)
            cmd.Parameters.AddWithValue("@orden", oRceJuntaMedicaE.Orden)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            Return dt
        End Function


        Public Function Rp_JuntaMedica(ByVal oRceJuntaMedicaE As RceJuntaMedicaE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Rp_JuntaMedica", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@cod_atencion", oRceJuntaMedicaE.CodAtencion)
            cmd.Parameters.AddWithValue("@orden", oRceJuntaMedicaE.Orden)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            Return dt
        End Function

    End Class
End Namespace


