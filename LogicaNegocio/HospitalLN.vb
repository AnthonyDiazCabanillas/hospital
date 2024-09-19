Imports System.Data
Imports Entidades.HospitalE
Imports AccesoDatos.HospitalAD

Namespace HospitalLN
    Public Class HospitalLN
        Dim oHospitalAD As New HospitalAD()

        Public Function Sp_Tablas_Consulta(ByVal sTabla As String) As DataTable
            Return oHospitalAD.Sp_Tablas_Consulta(sTabla)
        End Function

        Public Function Sp_RceHospital_Consulta(ByVal oHospitalE As HospitalE) As DataTable
            Return oHospitalAD.Sp_RceHospital_Consulta(oHospitalE)
        End Function

        Public Function Sp_HospitalProcedimiento_Consulta(ByVal oHospitalE As HospitalE) As DataTable
            Return oHospitalAD.Sp_HospitalProcedimiento_Consulta(oHospitalE)
        End Function

        Public Function Sp_CIFormatos_Consulta(ByVal oHospitalE As HospitalE) As DataTable
            Return oHospitalAD.Sp_CIFormatos_Consulta(oHospitalE)
        End Function

        Public Function Sp_RceHistoriaClinica_Consulta(ByVal oHospitalE As HospitalE) As DataTable
            Return oHospitalAD.Sp_RceHistoriaClinica_Consulta(oHospitalE)
        End Function

        Public Function Sp_Hospital_Update(ByVal oHospitalE As HospitalE) As Integer
            Return oHospitalAD.Sp_Hospital_Update(oHospitalE)
        End Function

        Public Function Sp_RceHistoriaClinicaCab_Update(ByVal oHospitalE As HospitalE) As Integer
            Return oHospitalAD.Sp_RceHistoriaClinicaCab_Update(oHospitalE)
        End Function

        Public Function Sp_HospitalDoc_Insert(ByVal oHospitalE As HospitalE) As HospitalE
            Return oHospitalAD.Sp_HospitalDoc_Insert(oHospitalE)
        End Function

        Public Function Sp_RceHospitalDoc_Insert(ByVal oHospitalE As HospitalE) As HospitalE
            Return oHospitalAD.Sp_RceHospitalDoc_Insert(oHospitalE)
        End Function

        Public Function Sp_RceHospitalDoc_Consulta(ByVal oHospitalE As HospitalE) As DataTable
            Return oHospitalAD.Sp_RceHospitalDoc_Consulta(oHospitalE)
        End Function

        Public Function Sp_RCEReportes_Consulta(ByVal oHospitalE As HospitalE) As DataTable
            Return oHospitalAD.Sp_RCEReportes_Consulta(oHospitalE)
        End Function

        Public Function Sp_RceValidacion(ByVal oHospitalE As HospitalE) As DataTable
            Return oHospitalAD.Sp_RceValidacion(oHospitalE)
        End Function


        Public Function Rp_RceHojadeconsentimiento(ByVal oHospitalE As HospitalE) As DataTable
            Return oHospitalAD.Rp_RceHojadeconsentimiento(oHospitalE)
        End Function

        Public Function Sp_RceEpicrisis_Insert(ByVal oHospitalE As HospitalE) As Integer
            Return oHospitalAD.Sp_RceEpicrisis_Insert(oHospitalE)
        End Function

        Public Function Rp_RceInformeMedico(ByVal oHospitalE As HospitalE) As DataTable
            Return oHospitalAD.Rp_RceInformeMedico(oHospitalE)
        End Function

        Public Function Sp_RceEpicrisis_Consulta(ByVal oHospitalE As HospitalE) As DataTable
            Return oHospitalAD.Sp_RceEpicrisis_Consulta(oHospitalE)
        End Function

        Public Function Sp_Hospital_Traslado_Consulta(ByVal oHospitalE As HospitalE) As DataTable
            Return oHospitalAD.Sp_Hospital_Traslado_Consulta(oHospitalE)
        End Function



        Public Function Sp_CorreosAltaMedica(ByVal oHospitalE As HospitalE) As Integer
            Return oHospitalAD.Sp_CorreosAltaMedica(oHospitalE)
        End Function



        Public Function ObtenerServicio(ByVal oHospitalE As HospitalE) As DataTable
            Return oHospitalAD.ObtenerServicio(oHospitalE)
        End Function
        Public Function Sp_RceProcedimientos_Consulta(ByVal oHospitalE As HospitalE) As DataTable
            Return oHospitalAD.Sp_RceProcedimientos_Consulta(oHospitalE)
        End Function
        Public Function Sp_ordenprocedimientosCab_Insert(ByVal oHospitalE As HospitalE) As String
            Return oHospitalAD.Sp_ordenprocedimientosCab_Insert(oHospitalE)
        End Function
        Public Function Sp_ordenprocedimientosDet_Insert(ByVal oHospitalE As HospitalE) As String
            Return oHospitalAD.Sp_ordenprocedimientosDet_Insert(oHospitalE)
        End Function
        Public Function Sp_Ordenprocedimientos_Update(ByVal oHospitalE As HospitalE) As String
            Return oHospitalAD.Sp_Ordenprocedimientos_Update(oHospitalE)
        End Function

        Public Function Sp_ordenprocedimientosCab_Update(ByVal pHospitalE As HospitalE) As String
            Return oHospitalAD.Sp_ordenprocedimientosCab_Update(pHospitalE)
        End Function


        Public Function Sp_RceProcedimientos_ConsultaV2(ByVal oHospitalE As HospitalE) As DataTable
            Return oHospitalAD.Sp_RceProcedimientos_ConsultaV2(oHospitalE)
        End Function


        Public Function Sp_RceHospital_ConsultaV2(ByVal oHospitalE As HospitalE) As DataTable
            Return oHospitalAD.Sp_RceHospital_ConsultaV2(oHospitalE)
        End Function


        Public Function Sp_RceSeccioncpt_Consulta(ByVal oHospitalE As HospitalE) As DataTable
            Return oHospitalAD.Sp_RceSeccioncpt_Consulta(oHospitalE)
        End Function

        Public Function Sp_RceCpt_Consulta(ByVal oHospitalE As HospitalE) As DataTable
            Return oHospitalAD.Sp_RceCpt_Consulta(oHospitalE)
        End Function


        Public Function Sp_RceCptFavorito_Delete(ByVal pHospitalE As HospitalE) As String
            Return oHospitalAD.Sp_RceCptFavorito_Delete(pHospitalE)
        End Function
        Public Function Sp_RceCptFavorito_Insert(ByVal pHospitalE As HospitalE) As String
            Return oHospitalAD.Sp_RceCptFavorito_Insert(pHospitalE)
        End Function

        Public Function Sp_RceCptFavorito_Consulta(ByVal pHospitalE As HospitalE) As DataTable
            Return oHospitalAD.Sp_RceCptFavorito_Consulta(pHospitalE)
        End Function

        Public Function Sp_OrdenprocedimientosCab_Update_(ByVal pHospitalE As HospitalE) As String
            Return oHospitalAD.Sp_OrdenprocedimientosCab_Update_(pHospitalE)
        End Function



        Public Function Rp_ProcedimientosHM(ByVal pHospitalE As HospitalE) As DataTable
            Return oHospitalAD.Rp_ProcedimientosHM(pHospitalE)
        End Function

        Public Function Rp_GestionClinica(ByVal pHospitalE As HospitalE) As DataTable
            Return oHospitalAD.Rp_GestionClinica(pHospitalE)
        End Function

        Public Function Rp_LibrodeHospitalizacion(ByVal pHospitalE As HospitalE) As DataTable
            Return oHospitalAD.Rp_LibrodeHospitalizacion(pHospitalE)
        End Function
        Public Function Datos_PacienteHospitalizado(codatencio As String) As HospitalE
            Return oHospitalAD.Datos_PacienteHospitalizado(codatencio)
        End Function
    End Class
End Namespace


