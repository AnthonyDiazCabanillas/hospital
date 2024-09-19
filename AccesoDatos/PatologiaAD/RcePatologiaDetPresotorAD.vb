Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Entidades.PatologiaE

Namespace PatologiaAD
    Public Class RcePatologiaDetPresotorAD
        'Cadena de Conexión para el Acceso a la BD
        Private CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString

        Public Function Sp_RcePatologiaDetPresotor_Update(ByVal pRcePatologiaDetPresotorE As RcePatologiaDetPresotorE) As Integer
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RcePatologiaDetPresotor_Update", cn)
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_patologiadet_pres", pRcePatologiaDetPresotorE.IdePatologiaDetPres)
            cmd.Parameters.AddWithValue("@codpresotor", pRcePatologiaDetPresotorE.CodPresotor)
            cmd.Parameters.AddWithValue("@nuevovalor", pRcePatologiaDetPresotorE.NuevoValor)
            cmd.Parameters.AddWithValue("@campo", pRcePatologiaDetPresotorE.Campo)
            cn.Open()
            Dim valor As Integer = cmd.ExecuteNonQuery()
            cn.Close()
            cn.Dispose()
            Return valor

        End Function

    End Class
End Namespace