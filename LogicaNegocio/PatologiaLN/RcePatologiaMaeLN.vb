Imports AccesoDatos.PatologiaAD
Imports Entidades.PatologiaE

Namespace PatologiaLN
    Public Class RcePatologiaMaeLN

        Dim oRcePatologiaMaeAD As New RcePatologiaMaeAD()

        Public Function Sp_RcePatologiaMae_Consulta(ByVal pRcePatologiaMaeE As RcePatologiaMaeE) As DataTable
            Return oRcePatologiaMaeAD.Sp_RcePatologiaMae_Consulta(pRcePatologiaMaeE)
        End Function

        Public Function Sp_RcePatologiaOrganosMae_Consulta(ByVal pRcePatologiaMaeE As RcePatologiaMaeE) As DataTable
            Return oRcePatologiaMaeAD.Sp_RcePatologiaOrganosMae_Consulta(pRcePatologiaMaeE)
        End Function

    End Class
End Namespace


