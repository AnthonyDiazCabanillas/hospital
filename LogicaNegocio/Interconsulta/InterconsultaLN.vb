Imports AccesoDatos.InterconsultaAD
Imports Entidades.InterconsultaE
Imports System.Data

Namespace InterconsultaLN
    Public Class InterconsultaLN
        Dim oInterconsultaAD As New InterconsultaAD

        Public Function Sp_RceInterconsulta_Consulta(ByVal oInterconsultaE As InterconsultaE) As DataTable
            Return oInterconsultaAD.Sp_RceInterconsulta_Consulta(oInterconsultaE)
        End Function

        Public Function Sp_RceHistoriaClinica_Insert(ByVal oInterconsultaE As InterconsultaE) As InterconsultaE
            Return oInterconsultaAD.Sp_RceHistoriaClinica_Insert(oInterconsultaE)
        End Function

        Public Function Sp_RceBuscar_Consulta(ByVal oInterconsultaE As InterconsultaE) As DataTable
            Return oInterconsultaAD.Sp_RceBuscar_Consulta(oInterconsultaE)
        End Function

        Public Function Sp_RceInterconsulta_Insert(ByVal oInterconsultaE As InterconsultaE) As Boolean
            Return oInterconsultaAD.Sp_RceInterconsulta_Insert(oInterconsultaE)
        End Function

        Public Function Sp_RceInterconsulta_Update(ByVal oInterconsultaE As InterconsultaE) As Boolean
            Return oInterconsultaAD.Sp_RceInterconsulta_Update(oInterconsultaE)
        End Function

        Public Function Sp_RceInterconsulta_Delete(ByVal oInterconsultaE As InterconsultaE) As Boolean
            Return oInterconsultaAD.Sp_RceInterconsulta_Delete(oInterconsultaE)
        End Function


        Public Function Sp_Medicos_Consulta(ByVal oInterconsultaE As InterconsultaE) As DataTable
            Return oInterconsultaAD.Sp_Medicos_Consulta(oInterconsultaE)
        End Function

        Public Function Ut_EnviarCorreov2(ByVal oInterconsultaE As InterconsultaE) As Boolean
            Return oInterconsultaAD.Ut_EnviarCorreov2(oInterconsultaE)
        End Function

        Public Function Ut_EnviarCorreov3(ByVal oInterconsultaE As InterconsultaE) As Boolean
            Return oInterconsultaAD.Ut_EnviarCorreov3(oInterconsultaE)
        End Function


        Public Function Rp_InterconsultaHM(ByVal oInterconsultaE As InterconsultaE) As DataTable
            Return oInterconsultaAD.Rp_InterconsultaHM(oInterconsultaE)
        End Function
    End Class
End Namespace


