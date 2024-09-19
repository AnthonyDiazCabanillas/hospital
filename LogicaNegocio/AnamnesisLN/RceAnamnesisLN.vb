Imports System.Data
Imports AccesoDatos.AnamnesisAD
Imports Entidades.AnamnesisE


Namespace AnamnesisLN
    Public Class RceAnamnesisLN
        Dim oRceAnamnesisAD As New RceAnamnesisAD()

        Public Function Sp_RceExamenFisicoMae_Consulta2(ByVal oRceAnamnesisE As RceAnamnesisE) As DataTable
            Return oRceAnamnesisAD.Sp_RceExamenFisicoMae_Consulta2(oRceAnamnesisE)
        End Function

        Public Function Sp_RceResultadoExamenFisicoDet_Insert2(ByVal oRceAnamnesisE As RceAnamnesisE) As RceAnamnesisE
            Return oRceAnamnesisAD.Sp_RceResultadoExamenFisicoDet_Insert2(oRceAnamnesisE)
        End Function



        Public Function Sp_RceExamenFisicoMae_Consulta5(ByVal oRceAnamnesisE As RceAnamnesisE) As DataTable
            Return oRceAnamnesisAD.Sp_RceExamenFisicoMae_Consulta5(oRceAnamnesisE)
        End Function
        Public Function Sp_RceResultadoExamenFisicoDet_Insert4(ByVal oRceAnamnesisE As RceAnamnesisE) As Integer
            Return oRceAnamnesisAD.Sp_RceResultadoExamenFisicoDet_Insert4(oRceAnamnesisE)
        End Function



        Public Function Sp_RceExamenFisicoMae_Consulta4(ByVal pRceExamenfisicoMaeE As RceAnamnesisE) As DataTable
            Return oRceAnamnesisAD.Sp_RceExamenFisicoMae_Consulta4(pRceExamenfisicoMaeE)
        End Function

    End Class
End Namespace

