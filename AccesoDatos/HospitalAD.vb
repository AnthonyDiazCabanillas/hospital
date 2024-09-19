Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Entidades.HospitalE

Namespace HospitalAD
    Public Class HospitalAD
        Private CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString

        ''' <summary>
        ''' FUNCION PARA CARGAR LOS COMBOS DE PISOS Y PABELLON
        ''' </summary>
        ''' <param name="sTabla"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_Tablas_Consulta(ByVal sTabla As String) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_Tablas_Consulta", cn)

            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codtabla", sTabla)
            cmd.Parameters.AddWithValue("@buscar", "")
            cmd.Parameters.AddWithValue("@key", 0)
            cmd.Parameters.AddWithValue("@numerolineas", 0)
            cmd.Parameters.AddWithValue("@orden", 2)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            Return dt
        End Function

        ''' <summary>
        ''' FUNCION PARA LISTAR/BUSCAR LOS PACIENTES POR PISO Y PABELLON
        ''' </summary>
        ''' <param name="oHospitalE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceHospital_Consulta(ByVal oHospitalE As HospitalE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceHospital_Consulta", cn)
            Dim dt As New DataTable()
            Try
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@busqueda_paciente", oHospitalE.NombrePaciente)
                cmd.Parameters.AddWithValue("@pabellon", oHospitalE.Pabellon)
                cmd.Parameters.AddWithValue("@servicio", oHospitalE.Servicio)
                cmd.Parameters.AddWithValue("@estado", oHospitalE.Estado)
                cmd.Parameters.AddWithValue("@orden", oHospitalE.Orden)
                cn.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
            Catch ex As Exception
                dt = New DataTable()
            Finally
                'dr.Close()
                'dr.Dispose()
                cmd.Dispose()
                cn.Close()
                cn.Dispose()
            End Try
            Return dt
        End Function


        Public Function Sp_HospitalProcedimiento_Consulta(ByVal oHospitalE As HospitalE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_HospitalProcedimiento_Consulta", cn)

            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codatencionproced", oHospitalE.CodAtencion)
            cmd.Parameters.AddWithValue("@orden", oHospitalE.Orden)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            Return dt
        End Function

        Public Function Sp_CIFormatos_Consulta(ByVal oHospitalE As HospitalE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_CIFormatos_Consulta", cn)

            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_plantilla", oHospitalE.IdePlantilla)
            cmd.Parameters.AddWithValue("@orden", oHospitalE.Orden)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            Return dt
        End Function


        Public Function Sp_RceHistoriaClinica_Consulta(ByVal oHospitalE As HospitalE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceHistoriaClinica_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@cod_atencion", oHospitalE.CodAtencion)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            Return dt
        End Function


        Public Function Sp_Hospital_Update(ByVal oHospitalE As HospitalE) As Integer
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_Hospital_Update", cn)
            Dim exito As Integer
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@campo", oHospitalE.Campo)
            cmd.Parameters.AddWithValue("@codigo", oHospitalE.CodAtencion)
            cmd.Parameters.AddWithValue("@nuevovalor", oHospitalE.ValorNuevo)
            cn.Open()
            exito = cmd.ExecuteNonQuery()
            cn.Close()
            Return exito
        End Function

        Public Function Sp_RceHistoriaClinicaCab_Update(ByVal oHospitalE As HospitalE) As Integer
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceHistoriaClinicaCab_Update", cn)
            Dim exito As Integer
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_historia", oHospitalE.IdeHistoria)
            cmd.Parameters.AddWithValue("@cod_atencion", oHospitalE.CodAtencion)
            cmd.Parameters.AddWithValue("@cod_paciente", oHospitalE.CodPaciente)
            cmd.Parameters.AddWithValue("@cod_medico", oHospitalE.CodMedico)
            cmd.Parameters.AddWithValue("@campo", oHospitalE.Campo)
            cmd.Parameters.AddWithValue("@valor_nuevo", oHospitalE.ValorNuevo)
            cmd.Parameters.AddWithValue("@usr_modifica", oHospitalE.CodUser)
            cn.Open()
            exito = cmd.ExecuteNonQuery()
            cn.Close()
            Return exito
        End Function

        Public Function Sp_HospitalDoc_Insert(ByVal oHospitalE As HospitalE) As HospitalE
            Dim cn As New SqlConnection(CnnBD)
            Dim exito As Integer
            Dim cmd As New SqlCommand("Sp_HospitalDoc_Insert", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@cod_atencion", oHospitalE.CodAtencion)
            cmd.Parameters.AddWithValue("@tipo_doc", oHospitalE.TipoDoc)
            cmd.Parameters.AddWithValue("@usuario", oHospitalE.CodUser)
            cmd.Parameters.AddWithValue("@bib_documento", "")
            cmd.Parameters.AddWithValue("@descripcion_doc", oHospitalE.Descripcion)
            cmd.Parameters.AddWithValue("@estado", 1)

            Dim oOutParameter1 As New SqlParameter("@id_documento", SqlDbType.Int, 8, ParameterDirection.InputOutput)
            oOutParameter1.ParameterName = "@id_documento"
            oOutParameter1.SqlDbType = SqlDbType.Int
            oOutParameter1.Size = 8
            oOutParameter1.Direction = ParameterDirection.InputOutput
            oOutParameter1.Value = oHospitalE.IdDocumento
            cmd.Parameters.Add(oOutParameter1)
            cn.Open()
            exito = cmd.ExecuteNonQuery()
            oHospitalE.IdDocumento = cmd.Parameters("@id_documento").Value
            Return oHospitalE
            
        End Function


        Public Function Sp_RceHospitalDoc_Insert(ByVal oHospitalE As HospitalE) As HospitalE
            Dim cn As New SqlConnection(CnnBD)
            Dim exito As Integer
            Dim cmd As New SqlCommand("Sp_RceHospitalDoc_Insert", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_historia", oHospitalE.IdeHistoria)
            cmd.Parameters.AddWithValue("@ide_general", oHospitalE.IdeGeneral)
            cmd.Parameters.AddWithValue("@tipo_doc", oHospitalE.TipoDoc)


            Dim oOutParameter1 As New SqlParameter("@ide_hospitaldoc", SqlDbType.Int, 8, ParameterDirection.InputOutput)
            oOutParameter1.ParameterName = "@ide_hospitaldoc"
            oOutParameter1.SqlDbType = SqlDbType.Int
            oOutParameter1.Size = 8
            oOutParameter1.Direction = ParameterDirection.InputOutput
            oOutParameter1.Value = oHospitalE.IdHospitalDoc
            cmd.Parameters.Add(oOutParameter1)
            cn.Open()
            exito = cmd.ExecuteNonQuery()
            oHospitalE.IdHospitalDoc = cmd.Parameters("@ide_hospitaldoc").Value
            Return oHospitalE
        End Function


        Public Function Sp_RceHospitalDoc_Consulta(ByVal oHospitalE As HospitalE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceHospitalDoc_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_historia", oHospitalE.IdeHistoria)
            cmd.Parameters.AddWithValue("@ide_general", oHospitalE.IdeGeneral)
            cmd.Parameters.AddWithValue("@tipo_doc", oHospitalE.TipoDoc)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            Return dt
        End Function
        'Sp_RceHospitalDoc_Consulta 


        Public Function Sp_RCEReportes_Consulta(ByVal oHospitalE As HospitalE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RCEReportes_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@tipo", oHospitalE.Tipo)
            cmd.Parameters.AddWithValue("@ide_historia", oHospitalE.IdeHistoria)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            Return dt
        End Function

        Public Function Sp_RceValidacion(ByVal oHospitalE As HospitalE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceValidacion", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@id_historia", oHospitalE.IdeHistoria)
            cmd.Parameters.AddWithValue("@id_usuario", oHospitalE.CodUser)
            cmd.Parameters.AddWithValue("@ide_tipoVal", oHospitalE.TipoValidacion)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            Return dt
        End Function

        Public Function Rp_RceHojadeconsentimiento(ByVal oHospitalE As HospitalE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Rp_RceHojadeconsentimiento", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codprocedimiento", oHospitalE.CodProcedimiento)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            Return dt
        End Function





        Public Function Sp_RceEpicrisis_Insert(ByVal oHospitalE As HospitalE) As Integer
            Dim cn As New SqlConnection(CnnBD)
            Dim exito As Integer
            Dim cmd As New SqlCommand("Sp_RceEpicrisis_Insert", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_historia", oHospitalE.IdeHistoria)
            cmd.Parameters.AddWithValue("@cod_atencion", oHospitalE.CodAtencion)

            cmd.Parameters.AddWithValue("@dsc_enfermedad_actual", oHospitalE.dsc_enfermedad_actual)
            cmd.Parameters.AddWithValue("@dsc_examenfisico", oHospitalE.dsc_examenfisico)
            cmd.Parameters.AddWithValue("@dsc_presionarterial", oHospitalE.dsc_presionarterial)
            cmd.Parameters.AddWithValue("@dsc_talla", oHospitalE.dsc_talla)
            cmd.Parameters.AddWithValue("@dsc_frecuenciacardiaca", oHospitalE.dsc_frecuenciacardiaca)
            cmd.Parameters.AddWithValue("@dsc_frecuenciarespiratoria", oHospitalE.dsc_frecuenciarespiratoria)
            cmd.Parameters.AddWithValue("@dsc_peso", oHospitalE.dsc_peso)
            cmd.Parameters.AddWithValue("@dsc_examenauxiliar", oHospitalE.dsc_examenauxiliar)
            cmd.Parameters.AddWithValue("@dsc_evolucion", oHospitalE.dsc_evolucion)
            cmd.Parameters.AddWithValue("@dsc_tratamiento", oHospitalE.dsc_tratamiento)
            cmd.Parameters.AddWithValue("@dsc_observacion", oHospitalE.dsc_observacion)
            cmd.Parameters.AddWithValue("@cod_altamedica", oHospitalE.cod_altamedica)
            cmd.Parameters.AddWithValue("@flg_necropsia", oHospitalE.flg_necropsia)

            cmd.Parameters.AddWithValue("@ide_usuario", oHospitalE.CodUser)

            cn.Open()
            exito = cmd.ExecuteNonQuery()

            Return exito
        End Function



        Public Function Rp_RceInformeMedico(ByVal oHospitalE As HospitalE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Rp_RceInformeMedico", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_historia", oHospitalE.IdeHistoria)
            cmd.Parameters.AddWithValue("@orden", oHospitalE.Orden)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            Return dt
        End Function


        Public Function Sp_RceEpicrisis_Consulta(ByVal oHospitalE As HospitalE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceEpicrisis_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_historia", oHospitalE.IdeHistoria)
            cmd.Parameters.AddWithValue("@cod_atencion", oHospitalE.CodAtencion)
            cmd.Parameters.AddWithValue("@orden", oHospitalE.Orden)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            Return dt
        End Function


        Public Function Sp_Hospital_Traslado_Consulta(ByVal oHospitalE As HospitalE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_Hospital_Traslado_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codatencionorigen", oHospitalE.CodAtencion)
            cmd.Parameters.AddWithValue("@accion", oHospitalE.Orden)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            Return dt
        End Function



        Public Function Sp_CorreosAltaMedica(ByVal oHospitalE As HospitalE) As Integer
            Dim cn As New SqlConnection(CnnBD)
            Dim exito As Integer
            Dim cmd As New SqlCommand("Sp_CorreosAltaMedica", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@atencion", oHospitalE.CodAtencion)
            cmd.Parameters.AddWithValue("@orden", oHospitalE.Orden)
            cn.Open()
            exito = cmd.ExecuteNonQuery()

            Return exito
        End Function

        'INICIO - PROCEDIMIENTOS MEDICOS - 13/08/2020
        Public Function ObtenerServicio(ByVal oHospitalE As HospitalE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("select cama, servicio from camas where cama = @cama", cn)
            cmd.CommandType = CommandType.Text
            cmd.Parameters.AddWithValue("@cama", oHospitalE.Cama)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            Return dt
        End Function


        Public Function Sp_RceProcedimientos_Consulta(ByVal pHospitalE As HospitalE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceProcedimientos_Consulta", cn)
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@atencion", pHospitalE.CodAtencion)
            cmd.Parameters.AddWithValue("@ide_orden", "0")
            cmd.Parameters.AddWithValue("@servicio", pHospitalE.Servicio)
            cmd.Parameters.AddWithValue("@opcion", pHospitalE.Orden)
            cn.Open()
            Dim tabla As New DataTable()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(tabla)
            cn.Close()
            Return tabla
        End Function

        Public Function Sp_ordenprocedimientosCab_Insert(ByVal pHospitalE As HospitalE) As String
            Dim ejecuto As Integer
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_ordenprocedimientosCab_Insert", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@cod_atencion", pHospitalE.CodAtencion)

            Dim oOutParameter1 As New SqlParameter("@codigo", SqlDbType.VarChar, 15, ParameterDirection.InputOutput)
            oOutParameter1.ParameterName = "@codigo"
            oOutParameter1.SqlDbType = SqlDbType.VarChar
            oOutParameter1.Size = 15
            oOutParameter1.Direction = ParameterDirection.InputOutput
            oOutParameter1.Value = pHospitalE.CodigoProcedimientoCab
            cmd.Parameters.Add(oOutParameter1)

            cn.Open()
            ejecuto = cmd.ExecuteNonQuery()
            Dim codigo As String
            codigo = cmd.Parameters("@codigo").Value
            cn.Close()
            Return codigo
        End Function


        Public Function Sp_ordenprocedimientosDet_Insert(ByVal pHospitalE As HospitalE) As String
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_ordenprocedimientosDet_Insert", cn)
            cmd.CommandType = System.Data.CommandType.StoredProcedure

            Dim oOutParameter1 As New SqlParameter()
            oOutParameter1.ParameterName = "@codigo"
            oOutParameter1.SqlDbType = SqlDbType.VarChar
            oOutParameter1.Size = 15
            oOutParameter1.Direction = ParameterDirection.InputOutput
            oOutParameter1.Value = pHospitalE.CodigoProcedimientoDet
            cmd.Parameters.Add(oOutParameter1)
            cmd.Parameters.AddWithValue("@ide_orden", pHospitalE.CodigoProcedimientoCab)
            cmd.Parameters.AddWithValue("@cod_medico", pHospitalE.CodMedico)
            cmd.Parameters.AddWithValue("@cod_prestacion", "")
            cmd.Parameters.AddWithValue("@valor", "0")
            cmd.Parameters.AddWithValue("@cod_empresa", "0")
            cmd.Parameters.AddWithValue("@orden", pHospitalE.Orden)

            cn.Open()
            cmd.ExecuteNonQuery()
            Dim codigo As String
            codigo = cmd.Parameters("@codigo").Value
            cn.Close()
            Return codigo
        End Function


        Public Function Sp_Ordenprocedimientos_Update(ByVal pHospitalE As HospitalE) As String
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_Ordenprocedimientos_Update", cn)
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codigo", pHospitalE.CodigoProcedimientoDet)
            cmd.Parameters.AddWithValue("@campo", pHospitalE.Campo)
            cmd.Parameters.AddWithValue("@nuevovalor", pHospitalE.ValorNuevo)
            cn.Open()
            Dim codigo As Integer
            codigo = cmd.ExecuteNonQuery()
            cn.Close()
            Return codigo
        End Function

        'INICIO - JB - NUEVO SP - 30/11/2020
        Public Function Sp_ordenprocedimientosCab_Update(ByVal pHospitalE As HospitalE) As String
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_ordenprocedimientosCab_Update", cn)
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@campo", pHospitalE.Campo)
            cmd.Parameters.AddWithValue("@codigo", pHospitalE.CodigoProcedimientoCab)
            cmd.Parameters.AddWithValue("@valor_nuevo", pHospitalE.ValorNuevo)
            cn.Open()
            Dim codigo As Integer
            codigo = cmd.ExecuteNonQuery()
            cn.Close()
            Return codigo
        End Function
        'FIN - JB - NUEVO SP - 30/11/2020

        Public Function Sp_RceProcedimientos_ConsultaV2(ByVal pHospitalE As HospitalE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceProcedimientos_ConsultaV2", cn)
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@cod_atencion", pHospitalE.CodAtencion)
            cmd.Parameters.AddWithValue("@ide_orden", pHospitalE.IdeOrden)
            cmd.Parameters.AddWithValue("@fec_orden", pHospitalE.FecOrden)
            cmd.Parameters.AddWithValue("@hor_orden", pHospitalE.HorOrden)
            cmd.Parameters.AddWithValue("@orden", pHospitalE.Orden)
            cn.Open()
            Dim tabla As New DataTable()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(tabla)
            cn.Close()
            Return tabla
        End Function
        'FIN - PROCEDIMIENTOS MEDICOS - 13/08/2020



        Public Function Sp_RceHospital_ConsultaV2(ByVal oHospitalE As HospitalE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceHospital_ConsultaV2", cn)

            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@pabellon", oHospitalE.Pabellon)
            cmd.Parameters.AddWithValue("@servicio", oHospitalE.Servicio)
            cmd.Parameters.AddWithValue("@estado", oHospitalE.Estado)
            cmd.Parameters.AddWithValue("@rce_table", oHospitalE.Tabla)
            cmd.Parameters.AddWithValue("@orden", oHospitalE.Orden)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            Return dt
        End Function








        Public Function Sp_RceSeccioncpt_Consulta(ByVal oHospitalE As HospitalE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceSeccioncpt_Consulta", cn)

            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codseccion", oHospitalE.CodSeccion)
            cmd.Parameters.AddWithValue("@orden", oHospitalE.Orden)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            Return dt
        End Function

        Public Function Sp_RceCpt_Consulta(ByVal oHospitalE As HospitalE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceCpt_Consulta", cn)

            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@cod_cpt", oHospitalE.CodProcedimiento)
            cmd.Parameters.AddWithValue("@codseccion", oHospitalE.CodSeccion)
            cmd.Parameters.AddWithValue("@codsubseccion", oHospitalE.CodSubSeccion)
            cmd.Parameters.AddWithValue("@orden", oHospitalE.Orden)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            Return dt
        End Function


        Public Function Sp_RceCptFavorito_Delete(ByVal pHospitalE As HospitalE) As String
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceCptFavorito_Delete", cn)
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@cod_cpt", pHospitalE.CodProcedimiento)
            cmd.Parameters.AddWithValue("@cod_medico", pHospitalE.CodMedico)
            cmd.Parameters.AddWithValue("@ide_tipoatencion", pHospitalE.TipoAtencion)
            cn.Open()
            Dim codigo As Integer
            codigo = cmd.ExecuteNonQuery()
            cn.Close()
            Return codigo
        End Function

        Public Function Sp_RceCptFavorito_Insert(ByVal pHospitalE As HospitalE) As String
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceCptFavorito_Insert", cn)
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@cod_cpt", pHospitalE.CodProcedimiento)
            cmd.Parameters.AddWithValue("@cod_medico", pHospitalE.CodMedico)
            cmd.Parameters.AddWithValue("@ide_tipoatencion", pHospitalE.TipoAtencion)
            cn.Open()
            Dim codigo As Integer
            codigo = cmd.ExecuteNonQuery()
            cn.Close()
            Return codigo
        End Function

        Public Function Sp_RceCptFavorito_Consulta(ByVal pHospitalE As HospitalE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceCptFavorito_Consulta", cn)
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@cod_cpt", pHospitalE.CodProcedimiento)
            cmd.Parameters.AddWithValue("@cod_medico", pHospitalE.CodMedico)
            cmd.Parameters.AddWithValue("@ide_tipoatencion", pHospitalE.TipoAtencion)
            cmd.Parameters.AddWithValue("@orden", pHospitalE.Orden)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            Return dt
        End Function

        Public Function Sp_OrdenprocedimientosCab_Update_(ByVal pHospitalE As HospitalE) As String
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_OrdenprocedimientosCab_Update", cn)
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codigo", pHospitalE.CodigoProcedimientoCab)
            cmd.Parameters.AddWithValue("@campo", pHospitalE.Campo)
            cmd.Parameters.AddWithValue("@nuevovalor", pHospitalE.ValorNuevo)
            cn.Open()
            Dim codigo As Integer
            codigo = cmd.ExecuteNonQuery()
            cn.Close()
            Return codigo
        End Function





        Public Function Rp_ProcedimientosHM(ByVal pHospitalE As HospitalE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Rp_ProcedimientosHM", cn)
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@FecIni", pHospitalE.FecInicio)
            cmd.Parameters.AddWithValue("@FecFin", pHospitalE.FecFin)
            cmd.Parameters.AddWithValue("@orden", pHospitalE.Orden)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            Return dt
        End Function

        Public Function Rp_GestionClinica(ByVal pHospitalE As HospitalE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Rp_GestionClinica", cn)
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@FecIni", pHospitalE.FecInicio)
            cmd.Parameters.AddWithValue("@FecFin", pHospitalE.FecFin)
            cmd.Parameters.AddWithValue("@orden", pHospitalE.Orden)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            Return dt
        End Function

        Public Function Rp_LibrodeHospitalizacion(ByVal pHospitalE As HospitalE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Rp_LibrodeHospitalizacion", cn)
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@fechainicio", pHospitalE.FecInicio)
            cmd.Parameters.AddWithValue("@fechafin", pHospitalE.FecFin)
            cmd.Parameters.AddWithValue("@codlocal", pHospitalE.Orden)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            Return dt
        End Function

        Public Function Datos_PacienteHospitalizado(codatencio As String) As HospitalE

            Try
                Dim cn As New SqlConnection(CnnBD)
                Dim cmd As New SqlCommand("Sp_Kardex_PacientesHospitalizados_Consulta", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@orden", 2)
                cmd.Parameters.AddWithValue("@n_filas", 10)
                cmd.Parameters.AddWithValue("@cod_atencion", codatencio)
                cmd.Parameters.AddWithValue("@v_Busq", "")
                cn.Open()
                Dim dr As SqlDataReader = cmd.ExecuteReader()
                Dim hh As HospitalE = New HospitalE()

                If (dr.Read()) Then
                    hh.Ide_kardexhospitalizacion = Convert.ToInt32(dr("Ide_kardexhospitalizacion").ToString())
                    hh.PesoPaciente = Convert.ToDecimal(dr("PesoPaciente").ToString())
                End If

                cn.Close()

                Return hh

            Catch ex As Exception
                Return New HospitalE()
            End Try

        End Function

    End Class
End Namespace


