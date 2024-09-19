Imports Entidades.ImagenesE
Imports AccesoDatos.ImagenesAD

Namespace ImagenLN
    Public Class RceImagenLN
        Dim oRceImagenesAD As New RceImagenesAD()

        Public Function Sp_RceBuscar_Consulta(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Return oRceImagenesAD.Sp_RceBuscar_Consulta(oRceImagenesE)
        End Function

        Public Function Sp_RceImagenFavoritoMae_Consulta(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Return oRceImagenesAD.Sp_RceImagenFavoritoMae_Consulta(oRceImagenesE)
        End Function

        Public Function Sp_RceRecetaImagenCab_Consulta(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Return oRceImagenesAD.Sp_RceRecetaImagenCab_Consulta(oRceImagenesE)
        End Function

        Public Function Sp_RceRecetaImagenDet_Consulta(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Return oRceImagenesAD.Sp_RceRecetaImagenDet_Consulta(oRceImagenesE)
        End Function


        Public Function Sp_RceRecetaImagenCab_InsertV2(ByVal oRceImagenesE As RceImagenesE) As RceImagenesE
            Return oRceImagenesAD.Sp_RceRecetaImagenCab_InsertV2(oRceImagenesE)
        End Function

        Public Function Sp_RceRecetaImagenDet_Update(ByVal oRceImagenesE As RceImagenesE) As Boolean
            Return oRceImagenesAD.Sp_RceRecetaImagenDet_Update(oRceImagenesE)
        End Function

        Public Function Sp_RceRecetaImagenCab_Update(ByVal oRceImagenesE As RceImagenesE) As Boolean
            Return oRceImagenesAD.Sp_RceRecetaImagenCab_Update(oRceImagenesE)
        End Function

        Public Function Sp_RceRecetaImagenDet_InsertV2(ByVal oRceImagenesE As RceImagenesE) As Boolean
            Return oRceImagenesAD.Sp_RceRecetaImagenDet_InsertV2(oRceImagenesE)
        End Function

        Public Function Sp_RceImagenFavoritoMae_Insert(ByVal oRceImagenesE As RceImagenesE) As Boolean
            Return oRceImagenesAD.Sp_RceImagenFavoritoMae_Insert(oRceImagenesE)
        End Function

        Public Function Sp_RceImagenFavoritoMae_Delete(ByVal oRceImagenesE As RceImagenesE) As Boolean
            Return oRceImagenesAD.Sp_RceImagenFavoritoMae_Delete(oRceImagenesE)
        End Function

        Public Function Sp_RceRecetaImagenDet_Delete(ByVal oRceImagenesE As RceImagenesE) As Boolean
            Return oRceImagenesAD.Sp_RceRecetaImagenDet_Delete(oRceImagenesE)
        End Function

        Public Function Sp_Presotor_Pdf_Consulta(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Return oRceImagenesAD.Sp_Presotor_Pdf_Consulta(oRceImagenesE)
        End Function

        Public Function Sp_Tablas_Consulta(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Return oRceImagenesAD.Sp_Tablas_Consulta(oRceImagenesE)
        End Function

        Public Function Sp_RceImagenMae_ConsultaV2(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Return oRceImagenesAD.Sp_RceImagenMae_ConsultaV2(oRceImagenesE)
        End Function





        Public Function Sp_Ris_EvaluaAtencion(ByVal oRceImagenesE As RceImagenesE) As String
            Return oRceImagenesAD.Sp_Ris_EvaluaAtencion(oRceImagenesE)
        End Function

        Public Function Sp_Ris_ListaDatosPresotor(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Return oRceImagenesAD.Sp_Ris_ListaDatosPresotor(oRceImagenesE)
        End Function

        Public Function Sp_Ris_Sala_Consulta(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Return oRceImagenesAD.Sp_Ris_Sala_Consulta(oRceImagenesE)
        End Function

        Public Function Sp_Ris_Consulta_WS(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Return oRceImagenesAD.Sp_Ris_Consulta_WS(oRceImagenesE)
        End Function

        Public Function Sp_Ris_Oracle_His_Xml_Events(ByVal oRceImagenesE As RceImagenesE) As Boolean
            Return oRceImagenesAD.Sp_Ris_Oracle_His_Xml_Events(oRceImagenesE)
        End Function
        'TMACASSI 31/10/2016
        Public Function Sp_Ris_Consulta_RisPacs(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Return oRceImagenesAD.Sp_Ris_Consulta_RisPacs(oRceImagenesE)
        End Function
        'TMACASSI 31/10/2016

        Public Function Sp_PresotorImagen_Insert(ByVal oRceImagenesE As RceImagenesE) As String
            Return oRceImagenesAD.Sp_PresotorImagen_Insert(oRceImagenesE)
        End Function


        Public Function RIS_PACS_WS() As DataTable
            Return oRceImagenesAD.RIS_PACS_WS()
        End Function

        Public Function Sp_RceRecetaImagenDet_ConsultaV2(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Return oRceImagenesAD.Sp_RceRecetaImagenDet_ConsultaV2(oRceImagenesE)
        End Function


        Public Function Sp_Presotor_Consulta2(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Return oRceImagenesAD.Sp_Presotor_Consulta2(oRceImagenesE)
        End Function

        Public Function Sp_Presotor_Consulta(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Return oRceImagenesAD.Sp_Presotor_Consulta(oRceImagenesE)
        End Function

        Public Function Sp_Presotor_DeleteNewv3(ByVal oRceImagenesE As RceImagenesE) As Boolean
            Return oRceImagenesAD.Sp_Presotor_DeleteNewv3(oRceImagenesE)
        End Function

        Public Function Sp_Ris_AgendamientoAmbulatorio_Delete(ByVal oRceImagenesE As RceImagenesE) As Boolean
            Return oRceImagenesAD.Sp_Ris_AgendamientoAmbulatorio_Delete(oRceImagenesE)
        End Function

        Public Function ConsultaRisOracle(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Return oRceImagenesAD.ConsultaRisOracle(oRceImagenesE)
        End Function

        Public Function Sp_Pacientes_Consulta(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Return oRceImagenesAD.Sp_Pacientes_Consulta(oRceImagenesE)
        End Function
    End Class
End Namespace

