Imports AccesoDatos.JuntaMedicaAD
Imports Entidades.JuntaMedicaE

Namespace JuntaMedicaLN
    Public Class RceJuntaMedicaLN
        Dim oRceJuntaMedicaAD As New RceJuntaMedicaAD

        Public Function Sp_RceJuntaMedica_Insert(ByVal oRceJuntaMedicaE As RceJuntaMedicaE) As Integer
            Return oRceJuntaMedicaAD.Sp_RceJuntaMedica_Insert(oRceJuntaMedicaE)
        End Function

        Public Function Sp_RceJuntaMedica_Consulta(ByVal oRceJuntaMedicaE As RceJuntaMedicaE) As DataTable
            Return oRceJuntaMedicaAD.Sp_RceJuntaMedica_Consulta(oRceJuntaMedicaE)
        End Function


        Public Function Rp_JuntaMedica(ByVal oRceJuntaMedicaE As RceJuntaMedicaE) As DataTable
            Return oRceJuntaMedicaAD.Rp_JuntaMedica(oRceJuntaMedicaE)
        End Function
    End Class
End Namespace


