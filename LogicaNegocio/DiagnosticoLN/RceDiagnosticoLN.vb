Imports System.Data
Imports AccesoDatos.DiagnosticoAD
Imports Entidades.DiagnosticoE

Namespace DiagnosticoLN
    Public Class RceDiagnosticoLN
        Dim oRceDiagnosticoAD As New RceDiagnosticoAD()

        Public Function Sp_RceBuscar_Consulta(ByVal oRceDiagnosticoE As RceDiagnosticoE) As DataTable
            Return oRceDiagnosticoAD.Sp_RceBuscar_Consulta(oRceDiagnosticoE)
        End Function

        Public Function Sp_RceDiagnosticoFavoritoMae_Consulta(ByVal oRceDiagnosticoE As RceDiagnosticoE) As DataTable
            Return oRceDiagnosticoAD.Sp_RceDiagnosticoFavoritoMae_Consulta(oRceDiagnosticoE)
        End Function

        Public Function Sp_Diagxhospital_Consulta1(ByVal oRceDiagnosticoE As RceDiagnosticoE) As DataTable
            Return oRceDiagnosticoAD.Sp_Diagxhospital_Consulta1(oRceDiagnosticoE)
        End Function

        Public Function Sp_RceDiagnosticoFavoritoMae_Delete(ByVal oRceDiagnosticoE As RceDiagnosticoE) As Boolean
            Return oRceDiagnosticoAD.Sp_RceDiagnosticoFavoritoMae_Delete(oRceDiagnosticoE)
        End Function

        Public Function Sp_RceDiagnosticoFavoritoMae_Insert(ByVal oRceDiagnosticoE As RceDiagnosticoE) As Boolean
            Return oRceDiagnosticoAD.Sp_RceDiagnosticoFavoritoMae_Insert(oRceDiagnosticoE)
        End Function

        Public Function Sp_Diagxhospital_Insert(ByVal oRceDiagnosticoE As RceDiagnosticoE) As Boolean
            Return oRceDiagnosticoAD.Sp_Diagxhospital_Insert(oRceDiagnosticoE)
        End Function

        Public Function Sp_Diagxhospital_Update(ByVal oRceDiagnosticoE As RceDiagnosticoE) As Boolean
            Return oRceDiagnosticoAD.Sp_Diagxhospital_Update(oRceDiagnosticoE)
        End Function

        Public Function Sp_Diagxhospital_Delete(ByVal oRceDiagnosticoE As RceDiagnosticoE) As Boolean
            Return oRceDiagnosticoAD.Sp_Diagxhospital_Delete(oRceDiagnosticoE)
        End Function

        Public Function Sp_RceDiagnostico_Consulta(ByVal oRceDiagnosticoE As RceDiagnosticoE) As DataTable
            Return oRceDiagnosticoAD.Sp_RceDiagnostico_Consulta(oRceDiagnosticoE)
        End Function

        Public Function Rp_Diagxhospital_Consulta(ByVal oRceDiagnosticoE As RceDiagnosticoE) As DataTable
            Return oRceDiagnosticoAD.Rp_Diagxhospital_Consulta(oRceDiagnosticoE)
        End Function

    End Class
End Namespace
