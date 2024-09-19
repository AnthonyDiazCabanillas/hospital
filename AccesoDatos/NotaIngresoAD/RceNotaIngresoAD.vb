Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Entidades.NotaIngresoE

Namespace NotaIngresoAD
    Public Class RceNotaIngresoAD
        Private CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString

        Public Function Sp_RceNotaIngreso_Consulta(ByVal oRceNotaIngresoE As RceNotaIngresoE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceNotaIngreso_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_historia", oRceNotaIngresoE.IdHistoria)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()

            Return dt
        End Function

        Public Function Sp_RceNotaIngreso_Insert(ByVal oRceNotaIngresoE As RceNotaIngresoE) As Integer
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceNotaIngreso_Insert", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_historia", oRceNotaIngresoE.IdHistoria)
            cmd.Parameters.AddWithValue("@cod_medico", oRceNotaIngresoE.CodMedico)
            cmd.Parameters.AddWithValue("@ide_usuario", oRceNotaIngresoE.IdUsuario)
            'cmd.Parameters.AddWithValue("@ide_notaingreso", oRceNotaIngresoE.IdNotaIngreso)

            Dim ParametroSalida As New SqlParameter()
            ParametroSalida.ParameterName = "@ide_notaingreso"
            ParametroSalida.Direction = ParameterDirection.InputOutput
            ParametroSalida.SqlDbType = SqlDbType.Int
            ParametroSalida.Size = 8
            ParametroSalida.Value = oRceNotaIngresoE.IdNotaIngreso
            cmd.Parameters.Add(ParametroSalida)

            Dim insert As Integer
            Dim update As Integer = 0
            cn.Open()
            insert = cmd.ExecuteNonQuery()
            oRceNotaIngresoE.IdNotaIngreso = cmd.Parameters("@ide_notaingreso").Value
            If insert > 0 Then
                Dim cmd_update As New SqlCommand("Sp_RceNotaIngreso_Update", cn)
                cmd_update.CommandType = CommandType.StoredProcedure
                cmd_update.Parameters.AddWithValue("@ide_notaingreso", oRceNotaIngresoE.IdNotaIngreso)
                cmd_update.Parameters.AddWithValue("@valor_nuevo", oRceNotaIngresoE.ValorCampo)
                cmd_update.Parameters.AddWithValue("@campo", oRceNotaIngresoE.IdCampo)
                update = cmd_update.ExecuteNonQuery()
            End If
            cn.Close()

            Return update
        End Function



        Public Function Rp_NotaIngreso(ByVal oRceNotaIngresoE As RceNotaIngresoE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Rp_NotaIngreso", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_historia", oRceNotaIngresoE.IdHistoria)
            cmd.Parameters.AddWithValue("@orden", oRceNotaIngresoE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()

            Return dt
        End Function
    End Class
End Namespace


