Imports AccesoDatos.MedicoAD
Imports Entidades.MedicoE

Namespace MedicoLN
    Public Class MedicoLN
        Dim oMedicoAD As New MedicoAD

        Public Function Sp_RceMedicos_Consulta(ByVal oMedicoE As MedicoE) As DataTable
            Return oMedicoAD.Sp_RceMedicos_Consulta(oMedicoE)
        End Function

    End Class
End Namespace


