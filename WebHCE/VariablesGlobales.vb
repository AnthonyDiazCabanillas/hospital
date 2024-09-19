Module VariablesGlobales
    Public Const sIdeSesion = "IdeSesion"
    Public Const sNombreUsuario = "NombreUsuario"
    Public Const sCodMedico = "CodMedico"
    Public Const sCodUser = "CodUser"
    Public Const sCodigoAtencion = "CodigoAtencion"
    Public Const sIdeHistoria = "IdeHistoria"
    Public Const sCodPaciente = "CodPaciente"

    Public Const sIdRecetaCab = "IdRecetaCab" 'laboratorio
    Public Const sIdeRecetaImagenCab = "IdeRecetaImagenCab" 'imagen
    Public Const sCodEnfermera = "CodEnfermera"
    Public Const ScodigoAccesoLogin = "login"
    Public Const sTipoAtencion = "TipoAtencion"
    Public Const sCodigoAtencion_Auxiliar = "CodigoAtencion_Auxiliar" '21/06/2016
    Public Const sTablaProductoMedicamento = "TablaProductoMedicamento"
    Public Const sTablaAnalisisLaboratorio = "TablaAnalisisLaboratorio"
    Public Const sTablaImagenes = "TablaImagenes"
    Public Const sTablaDeclaratoriaAlergia = "TablaDeclaratoriaAlergia"
    Public Const sTablaReconciliacionMedicamentosa = "TablaReconciliacionMedicamentosa"
    Public Const sTipoD = "E" 'E o S
    Public Const sDscPcName = "DscPcName"
    Public Const sSede = "Sede"
    Public Const sTablaInfusiones = "TablaInfusiones"
    Public Const sCodEspecialidad = "CodEspecialidad"
    Public Const Ide_usuario = "Ide_usuario"

    'Variables para mensajes de guardar, actualizar y eliminar
    Public Const sMensajeGuardarError = "MENSAJE_GUARDAR_ERROR"
    Public Const sMensajeActualizarError = "MENSAJE_ACTUALIZAR_ERROR"
    Public Const sMensajeEliminarError = "MENSAJE_ELIMINAR_ERROR"

    Public Const sRutaArchivos = "\\JMEUSIS113\Archivos\"

    Public Const sRutaTemp = "c:\Temp"

    'Variable pantalla default
    Public Const sPantallaDefault = "PANTALLA_DEFAULT"
    Public Const sPerfilUsuario = "PerfilUsuario"

    Public Const sTablaFarmacologicoRA = "TablaFarmacologicoRA"
    Public Const sTablaNoFarmacologicoRA = "TablaNoFarmacologicoRA"
    Public Const sTablaInfusionesRA = "TablaInfusionesRA"

    'Nuevos - 2019
    Public Const sRutaServicioRes = "http://172.16.1.64/CSPublicQueryService/CSPublicQueryService.svc/json/EncryptQS?"
    Public RutaWebServicesRis As String = ""
    Public Const sEntidadImagenes = "EntidadImagenes"
    Public Const sTablaPatologiasSeleccionados = "TablaPatologiasSeleccionados"
    '2021
    Public Const sTablaMedicamentosSuspender = "TablaMedicamentosSuspender"

End Module
