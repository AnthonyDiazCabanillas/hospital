Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Entidades.PatologiaE

Namespace PatologiaAD
    Public Class RcePatologiaCabAD
        Private CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ToString()

        Public Function Sp_RcePatologiaCab_Insert(ByVal pRcePatologiaCabE As RcePatologiaCabE) As Integer
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RcePatologiaCab_Insert", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@cod_atencion", pRcePatologiaCabE.CodAtencion)
            cmd.Parameters.AddWithValue("@ide_historia", pRcePatologiaCabE.IdeHistoria)
            cmd.Parameters.AddWithValue("@cod_medico", pRcePatologiaCabE.CodMedico)
            cmd.Parameters.AddWithValue("@est_examen", pRcePatologiaCabE.EstExamen)
            cmd.Parameters.AddWithValue("@dsc_muestra", pRcePatologiaCabE.Muestra)
            cmd.Parameters.AddWithValue("@dsc_datosclinico", pRcePatologiaCabE.DatosClinico)

            If IsNothing(pRcePatologiaCabE.FecUltimaRegla) Then
                cmd.Parameters.AddWithValue("@fec_ultimaregla", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@fec_ultimaregla", Format(CDate(pRcePatologiaCabE.FecUltimaRegla), "MM/dd/yyyy h:mm:ss"))
            End If


            'cmd.Parameters.AddWithValue("@fec_ultimaregla", IIf(IsNothing(pRcePatologiaCabE.FecUltimaRegla), DBNull.Value, Format(CDate(pRcePatologiaCabE.FecUltimaRegla), "MM/dd/yyyy h:mm:ss")))
            cmd.Parameters.AddWithValue("@usr_registra", pRcePatologiaCabE.UsrRegistra)

            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim IdPatologiaCab As Integer
            IdPatologiaCab = cmd.ExecuteScalar()
            Return IdPatologiaCab
            'If cmd.ExecuteNonQuery() > 0 Then
            '    Return True
            'Else
            '    Return False
            'End If

            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
        End Function


        

    End Class

End Namespace

