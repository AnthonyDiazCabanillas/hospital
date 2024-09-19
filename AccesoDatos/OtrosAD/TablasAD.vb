Imports Entidades.OtrosE
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Namespace OtrosAD
    Public Class TablasAD
        Private CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString

        Public Function Sp_RceResultadoExamenFisico_Consulta(ByVal oTablasE As TablasE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceResultadoExamenFisico_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_examen", oTablasE.IdeExamen)
            cmd.Parameters.AddWithValue("@ide_historia", oTablasE.IdeHistoria)
            cmd.Parameters.AddWithValue("@orden", oTablasE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt
        End Function


        Public Function Sp_Tablas_Consulta(ByVal oTablasE As TablasE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_Tablas_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codtabla", oTablasE.CodTabla)
            cmd.Parameters.AddWithValue("@buscar", oTablasE.Buscar)
            cmd.Parameters.AddWithValue("@key", oTablasE.Key)
            cmd.Parameters.AddWithValue("@numerolineas", oTablasE.NumeroLineas)
            cmd.Parameters.AddWithValue("@orden", oTablasE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt
        End Function

        ''' <summary>
        ''' FUNCION PARA MOSTRAR ALERTAS
        ''' </summary>
        ''' <param name="oTablasE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceAlerta(ByVal oTablasE As TablasE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceAlerta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@id_rce", oTablasE.IdeHistoria)
            cmd.Parameters.AddWithValue("@id_alerta", oTablasE.IdeAlerta)
            cmd.Parameters.AddWithValue("@id_usuario", oTablasE.IdeUsuario) 'TMACASSI 12/10/2016
            cmd.Parameters.AddWithValue("@orden", oTablasE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt
        End Function



    End Class
End Namespace

