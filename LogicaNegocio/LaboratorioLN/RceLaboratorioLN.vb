Imports System.Data
Imports AccesoDatos.LaboratorioAD
Imports Entidades.LaboratorioE

Namespace LaboratorioLN
    Public Class RceLaboratorioLN
        Dim oRceLaboratioAD As New RceLaboratioAD()

        Public Function Sp_RceBuscar_Consulta(ByVal oRceLaboratioE As RceLaboratioE) As DataTable
            Return oRceLaboratioAD.Sp_RceBuscar_Consulta(oRceLaboratioE)
        End Function

        Public Function Sp_RceAnalisisFavoritoMae_Consulta(ByVal oRceLaboratioE As RceLaboratioE) As DataTable
            Return oRceLaboratioAD.Sp_RceAnalisisFavoritoMae_Consulta(oRceLaboratioE)
        End Function

        Public Function Sp_RceAnalisisEmergenciaMae_Consulta(ByVal oRceLaboratioE As RceLaboratioE) As DataTable
            Return oRceLaboratioAD.Sp_RceAnalisisEmergenciaMae_Consulta(oRceLaboratioE)
        End Function

        Public Function Sp_RceRecetaAnalisisCab_Consulta(ByVal oRceLaboratioE As RceLaboratioE) As DataTable
            Return oRceLaboratioAD.Sp_RceRecetaAnalisisCab_Consulta(oRceLaboratioE)
        End Function

        Public Function Sp_RceRecetaAnalisisDet_Consulta(ByVal oRceLaboratioE As RceLaboratioE) As DataTable
            Return oRceLaboratioAD.Sp_RceRecetaAnalisisDet_Consulta(oRceLaboratioE)
        End Function

        Public Function Sp_RceRecetaAnalisisDet_Delete(ByVal oRceLaboratioE As RceLaboratioE) As Boolean
            Return oRceLaboratioAD.Sp_RceRecetaAnalisisDet_Delete(oRceLaboratioE)
        End Function

        Public Function Sp_RceAnalisisFavoritoMae_Delete(ByVal oRceLaboratioE As RceLaboratioE) As Boolean
            Return oRceLaboratioAD.Sp_RceAnalisisFavoritoMae_Delete(oRceLaboratioE)
        End Function

        Public Function Sp_RceAnalisisFavoritoMae_Insert(ByVal oRceLaboratioE As RceLaboratioE) As Boolean
            Return oRceLaboratioAD.Sp_RceAnalisisFavoritoMae_Insert(oRceLaboratioE)
        End Function

        Public Function Sp_RceRecetaAnalisisCab_InsertV2(ByVal oRceLaboratioE As RceLaboratioE) As Boolean
            Return oRceLaboratioAD.Sp_RceRecetaAnalisisCab_InsertV2(oRceLaboratioE)
        End Function

        Public Function Sp_RceRecetaAnalisisCab_Update(ByVal oRceLaboratioE As RceLaboratioE) As Boolean
            Return oRceLaboratioAD.Sp_RceRecetaAnalisisCab_Update(oRceLaboratioE)
        End Function

        Public Function Sp_RceRecetaAnalisisDet_Update(ByVal oRceLaboratioE As RceLaboratioE) As Boolean
            Return oRceLaboratioAD.Sp_RceRecetaAnalisisDet_Update(oRceLaboratioE)
        End Function

        Public Function Sp_RceRecetaAnalisisDet_Insert(ByVal oRceLaboratioE As RceLaboratioE) As Boolean
            Return oRceLaboratioAD.Sp_RceRecetaAnalisisDet_Insert(oRceLaboratioE)
        End Function

        Public Function Sp_RceAnalisisxDiagnostico_Consulta(ByVal oRceLaboratioE As RceLaboratioE) As Boolean
            Return oRceLaboratioAD.Sp_RceAnalisisxDiagnostico_Consulta(oRceLaboratioE)
        End Function


        Public Function Sp_RceResultadoAnalisisCab_Consulta(ByVal oRceLaboratioE As RceLaboratioE) As DataTable
            Return oRceLaboratioAD.Sp_RceResultadoAnalisisCab_Consulta(oRceLaboratioE)
        End Function

        Public Function Sp_RceRecetaAnalisis_Consulta(ByVal oRceLaboratioE As RceLaboratioE) As DataTable
            Return oRceLaboratioAD.Sp_RceRecetaAnalisis_Consulta(oRceLaboratioE)
        End Function


        Public Function Sp_RceAnalisisxDiagnostico_Valida(ByVal oRceLaboratioE As RceLaboratioE) As DataTable
            Return oRceLaboratioAD.Sp_RceAnalisisxDiagnostico_Valida(oRceLaboratioE)
        End Function


        Public Function Sp_RceRecetaAnalisisDet_ConsultaV2(ByVal oRceLaboratioE As RceLaboratioE) As DataTable
            Return oRceLaboratioAD.Sp_RceRecetaAnalisisDet_ConsultaV2(oRceLaboratioE)
        End Function

    End Class
End Namespace


