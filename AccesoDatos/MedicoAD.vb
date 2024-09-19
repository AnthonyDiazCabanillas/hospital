Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Entidades.MedicoE

Namespace MedicoAD
    Public Class MedicoAD
        Private CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString

        ''' <summary>
        ''' FUNCION PARA CONSUTAR LOS DATOS DEL MEDICO DE LA ATENCION
        ''' </summary>
        ''' <param name="oMedicoE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceMedicos_Consulta(ByVal oMedicoE As MedicoE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceMedicos_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codmedico", oMedicoE.CodMedico)
            cmd.Parameters.AddWithValue("@codatencion", oMedicoE.Atencion)
            cmd.Parameters.AddWithValue("@orden", oMedicoE.Orden)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)

            Return dt
        End Function

    End Class
End Namespace

