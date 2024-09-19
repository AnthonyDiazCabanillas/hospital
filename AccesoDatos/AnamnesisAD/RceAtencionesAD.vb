Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Entidades.AnamnesisE

Namespace AnamnesisAD
    Public Class RceAtencionesAD
        Private CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString

        Public Function Sp_Hospital_ConsultaV2RCE(ByVal oRceAtencionesE As RceAtencionesE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_Hospital_ConsultaV2RCE", cn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@fec_atencion", "")
            cmd.Parameters.AddWithValue("@cod_medico", "")
            cmd.Parameters.AddWithValue("@cod_atencion", "")
            cmd.Parameters.AddWithValue("@cod_paciente", oRceAtencionesE.CodPaciente)
            cmd.Parameters.AddWithValue("@orden", oRceAtencionesE.Orden)
            cmd.Parameters.AddWithValue("@sede", oRceAtencionesE.Sede)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            cn.Open()

            da.Fill(dt)
            Return dt

        End Function

    End Class
End Namespace
