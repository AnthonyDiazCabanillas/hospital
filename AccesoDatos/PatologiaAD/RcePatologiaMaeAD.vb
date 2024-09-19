Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Entidades.PatologiaE

Namespace PatologiaAD
    Public Class RcePatologiaMaeAD
        Private CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ToString()

        Public Function Sp_RcePatologiaMae_Consulta(ByVal pRcePatologiaMaeE As RcePatologiaMaeE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RcePatologiaMae_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_patologia_mae", pRcePatologiaMaeE.IdePatologiaMae)
            cmd.Parameters.AddWithValue("@cod_tipoatencion", pRcePatologiaMaeE.IdeTipoAtencion)
            cmd.Parameters.AddWithValue("@cod_medico", pRcePatologiaMaeE.CodMedico)
            cmd.Parameters.AddWithValue("@buscar", pRcePatologiaMaeE.Nombre)
            cmd.Parameters.AddWithValue("@orden", pRcePatologiaMaeE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()

            Return dt
        End Function

        Public Function Sp_RcePatologiaOrganosMae_Consulta(ByVal pRcePatologiaMaeE As RcePatologiaMaeE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RcePatologiaOrganosMae_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_organos_mae", pRcePatologiaMaeE.IdePatologiaMae)
            cmd.Parameters.AddWithValue("@orden", pRcePatologiaMaeE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()

            Return dt
        End Function

    End Class
End Namespace

