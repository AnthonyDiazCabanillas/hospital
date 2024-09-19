Imports AccesoDatos.PatologiaAD
Imports Entidades.PatologiaE

Namespace PatologiaLN
    Public Class RcePatologiaDetLN
        Dim oRcePatologiaDetAD As New RcePatologiaDetAD()

        Public Function Sp_RcePatologiaDet_Insert(ByVal pRcePatologiaMaeE As RcePatologiaDetE) As Boolean
            Return oRcePatologiaDetAD.Sp_RcePatologiaDet_Insert(pRcePatologiaMaeE)
        End Function

        Public Function Sp_RcePatologiaDet_Consulta(ByVal pRcePatologiaDetE As RcePatologiaDetE) As DataTable
            Return oRcePatologiaDetAD.Sp_RcePatologiaDet_Consulta(pRcePatologiaDetE)
        End Function

        Public Function Sp_RcePatologiaDet_ConsultaV1(ByVal pRcePatologiaDetE As RcePatologiaDetE) As DataTable
            Return oRcePatologiaDetAD.Sp_RcePatologiaDet_ConsultaV1(pRcePatologiaDetE)
        End Function

        Public Function Sp_RcePatologiaDet_Update(ByVal pRcePatologiaDetE As RcePatologiaDetE) As Integer
            Return oRcePatologiaDetAD.Sp_RcePatologiaDet_Update(pRcePatologiaDetE)
        End Function

        Public Function Sp_RcePatologiaDetPresotor_Consulta(ByVal pRcePatologiaDetE As RcePatologiaDetE) As DataTable
            Return oRcePatologiaDetAD.Sp_RcePatologiaDetPresotor_Consulta(pRcePatologiaDetE)
        End Function

        Public Function Sp_RceResultadoDocumentoDet_Consulta(ByVal pRcePatologiaDetE As RcePatologiaDetE) As DataTable
            Return oRcePatologiaDetAD.Sp_RceResultadoDocumentoDet_Consulta(pRcePatologiaDetE)
        End Function

    End Class
End Namespace

