Imports System.Data
Imports Entidades.OtrosE
Imports AccesoDatos.OtrosAD

Namespace OtrosLN
    Public Class TablasLN
        Dim oTablasAD As New TablasAD()

        Public Function Sp_RceResultadoExamenFisico_Consulta(ByVal oTablasE As TablasE) As DataTable
            Return oTablasAD.Sp_RceResultadoExamenFisico_Consulta(oTablasE)
        End Function

        Public Function Sp_Tablas_Consulta(ByVal oTablasE As TablasE) As DataTable
            Return oTablasAD.Sp_Tablas_Consulta(oTablasE)
        End Function

        Public Function Sp_RceAlerta(ByVal oTablasE As TablasE) As DataTable
            Return oTablasAD.Sp_RceAlerta(oTablasE)
        End Function

    End Class
End Namespace


