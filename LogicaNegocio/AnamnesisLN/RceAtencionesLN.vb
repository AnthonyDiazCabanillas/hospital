Imports System.Data
Imports AccesoDatos.AnamnesisAD
Imports Entidades.AnamnesisE

Namespace AnamnesisLN
    Public Class RceAtencionesLN
        Dim oRceAtencionesAD As New RceAtencionesAD()

        Public Function Sp_Hospital_ConsultaV2RCE(ByVal oRceAtencionesE As RceAtencionesE) As DataTable
            Return oRceAtencionesAD.Sp_Hospital_ConsultaV2RCE(oRceAtencionesE)
        End Function

    End Class
End Namespace

