Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Entidades.PatologiaE

Namespace PatologiaAD
    Public Class RcePatologiaFavoritoMaeAD
        Private CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ToString()

        Public Function Sp_RcePatologiaFavoritoMae_Delete(ByVal oRcePatologiaFavoritoMaeE As RcePatologiaFavoritoMaeE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RcePatologiaFavoritoMae_Delete", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_patologiafavorito", oRcePatologiaFavoritoMaeE.IdePatologiaFavorito)

            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If

            If cmd.ExecuteNonQuery() > 0 Then
                If cn.State = ConnectionState.Open Then
                    cn.Close()
                End If
                Return True
            Else
                If cn.State = ConnectionState.Open Then
                    cn.Close()
                End If
                Return False
            End If


        End Function

        Public Function Sp_RcePatologiaFavoritoMae_Insert(ByVal oRcePatologiaFavoritoMaeE As RcePatologiaFavoritoMaeE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RcePatologiaFavoritoMae_Insert", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_patologia_mae", oRcePatologiaFavoritoMaeE.IdePatologiaMae)
            cmd.Parameters.AddWithValue("@cod_medico", oRcePatologiaFavoritoMaeE.CodMedico)
            cmd.Parameters.AddWithValue("@flg_estado", oRcePatologiaFavoritoMaeE.FlgEstado)

            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If

            If cmd.ExecuteNonQuery() > 0 Then
                If cn.State = ConnectionState.Open Then
                    cn.Close()
                End If
                Return True
            Else
                If cn.State = ConnectionState.Open Then
                    cn.Close()
                End If
                Return False
            End If

            

        End Function

        Public Function Sp_RcePatologiaFavoritoMae_Consulta(ByVal oRcePatologiaFavoritoMaeE As RcePatologiaFavoritoMaeE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RcePatologiaFavoritoMae_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_patologiafavorito", oRcePatologiaFavoritoMaeE.IdePatologiaFavorito)
            cmd.Parameters.AddWithValue("@cod_medico", oRcePatologiaFavoritoMaeE.CodMedico)
            cmd.Parameters.AddWithValue("@orden", oRcePatologiaFavoritoMaeE.Orden)

            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If

            Return dt

        End Function



    End Class
End Namespace

