Imports System.Data
Imports AccesoDatos.MedicamentosAD
Imports Entidades.MedicamentosE

Namespace MedicamentosLN
    Public Class RceMedicamentosLN
        Dim oRceMedicamentosAD As New RceMedicamentosAD()

        Public Function Sp_RceMedicamentosaCab_Consulta(ByVal oRceMedicamentosE As RceMedicamentosE) As DataTable
            Return oRceMedicamentosAD.Sp_RceMedicamentosaCab_Consulta(oRceMedicamentosE)
        End Function

        Public Function Sp_RceMedicamentosaCab_Insert(ByVal oRceMedicamentosE As RceMedicamentosE) As Integer
            Return oRceMedicamentosAD.Sp_RceMedicamentosaCab_Insert(oRceMedicamentosE)
        End Function

        Public Function Sp_RceMedicamentosaDet_Insert(ByVal oRceMedicamentosE As RceMedicamentosE) As Integer
            Return oRceMedicamentosAD.Sp_RceMedicamentosaDet_Insert(oRceMedicamentosE)
        End Function

        Public Function Sp_RceMedicamentosaDet_Update(ByVal oRceMedicamentosE As RceMedicamentosE) As Integer
            Return oRceMedicamentosAD.Sp_RceMedicamentosaDet_Update(oRceMedicamentosE)
        End Function


        Public Function Sp_Buscar(ByVal oRceMedicamentosE As RceMedicamentosE) As DataTable
            Return oRceMedicamentosAD.Sp_Buscar(oRceMedicamentosE)
        End Function

        Public Function Rp_RceRecetaMedicamento1(ByVal oRceMedicamentosE As RceMedicamentosE) As DataTable
            Return oRceMedicamentosAD.Rp_RceRecetaMedicamento1(oRceMedicamentosE)
        End Function

        Public Function Sp_RceResultadoDocumentoDet_InsertV3(ByVal oRceMedicamentosE As RceMedicamentosE) As Integer
            Return oRceMedicamentosAD.Sp_RceResultadoDocumentoDet_InsertV3(oRceMedicamentosE)
        End Function

        Public Function Rp_RceRecetaAlta(ByVal oRceMedicamentosE As RceMedicamentosE) As DataTable
            Return oRceMedicamentosAD.Rp_RceRecetaAlta(oRceMedicamentosE)
        End Function

        Public Function Rp_RceRecetaTratamiento(ByVal oRceMedicamentosE As RceMedicamentosE) As DataTable
            Return oRceMedicamentosAD.Rp_RceRecetaTratamiento(oRceMedicamentosE)
        End Function
    End Class
End Namespace

