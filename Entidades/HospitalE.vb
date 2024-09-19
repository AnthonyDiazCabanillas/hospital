Namespace HospitalE
    Public Class HospitalE
        Private _NombrePaciente As String
        Private _Pabellon As String
        Private _Servicio As String
        Private _Orden As Integer
        Private _Estado As String = "" 'JB - 15/06/2020 - nuevo parametro

        Private _CodAtencion As String = ""
        Private _IdePlantilla As String = ""

        Private _ValorNuevo As String = ""
        Private _Campo As String = ""
        Private _CodPaciente As String = "0"
        Private _CodMedico As String = "0"
        Private _IdeHistoria As Integer
        Private _CodUser As Integer

        Private _IdDocumento As Integer '27/01/2017 - Nuevo valor
        Private _IdHospitalDoc As Integer '27/01/2017 - Nuevo valor
        Private _TipoDoc As Integer '27/01/2017 - Nuevo valor
        Private _IdeGeneral As Integer
        Private _Descripcion As String
        Private _Tipo As String

        Private _TipoValidacion As Integer
        Private _CodProcedimiento As String


        Private _dsc_enfermedad_actual As String
        Private _dsc_examenfisico As String
        Private _dsc_presionarterial As String '13/07/2020 - SE CAMBIO DE DECIMAL A STRING
        Private _dsc_talla As Decimal
        Private _dsc_frecuenciacardiaca As Decimal
        Private _dsc_frecuenciarespiratoria As Decimal
        Private _dsc_peso As Decimal

        Private _dsc_examenauxiliar As String
        Private _dsc_evolucion As String
        Private _dsc_tratamiento As String
        Private _dsc_observacion As String
        Private _cod_altamedica As String
        Private _flg_necropsia As Integer


        Private _Cama As String = ""
        Private _CodigoProcedimientoCab As String = ""
        Private _CodigoProcedimientoDet As String = ""

        Private _FecOrden As String = ""
        Private _HorOrden As String = ""
        Private _IdeOrden As Integer = 0

        Private _CodSeccion As String = ""
        Private _CodSubSeccion As String = ""
        Private _TipoAtencion As String = ""
        Private _Tabla As DataTable


        Private _FecInicio As String = ""
        Private _FecFin As String = ""
        Private _PesoPaciente As Decimal? = 0
        Private _Ide_kardexhospitalizacion As Integer? = 0

        Public Property FecInicio() As String
            Get
                Return _FecInicio
            End Get
            Set(ByVal value As String)
                _FecInicio = value
            End Set
        End Property
        Public Property FecFin() As String
            Get
                Return _FecFin
            End Get
            Set(ByVal value As String)
                _FecFin = value
            End Set
        End Property

        Public Property Tabla() As DataTable
            Get
                Return _Tabla
            End Get
            Set(ByVal value As DataTable)
                _Tabla = value
            End Set
        End Property


        Public Property TipoAtencion() As String
            Get
                Return _TipoAtencion
            End Get
            Set(ByVal value As String)
                _TipoAtencion = value
            End Set
        End Property
        Public Property CodSubSeccion() As String
            Get
                Return _CodSubSeccion
            End Get
            Set(ByVal value As String)
                _CodSubSeccion = value
            End Set
        End Property
        Public Property CodSeccion() As String
            Get
                Return _CodSeccion
            End Get
            Set(ByVal value As String)
                _CodSeccion = value
            End Set
        End Property

        Public Property IdeOrden() As Integer
            Get
                Return _IdeOrden
            End Get
            Set(ByVal value As Integer)
                _IdeOrden = value
            End Set
        End Property
        Public Property FecOrden() As String
            Get
                Return _FecOrden
            End Get
            Set(ByVal value As String)
                _FecOrden = value
            End Set
        End Property
        Public Property HorOrden() As String
            Get
                Return _HorOrden
            End Get
            Set(ByVal value As String)
                _HorOrden = value
            End Set
        End Property


        Public Property CodigoProcedimientoDet() As String
            Get
                Return _CodigoProcedimientoDet
            End Get
            Set(ByVal value As String)
                _CodigoProcedimientoDet = value
            End Set
        End Property

        Public Property CodigoProcedimientoCab() As String
            Get
                Return _CodigoProcedimientoCab
            End Get
            Set(ByVal value As String)
                _CodigoProcedimientoCab = value
            End Set
        End Property
        Public Property Cama() As String
            Get
                Return _Cama
            End Get
            Set(ByVal value As String)
                _Cama = value
            End Set
        End Property

        Public Property Estado() As String
            Get
                Return _Estado
            End Get
            Set(ByVal value As String)
                _Estado = value
            End Set
        End Property

        Public Property flg_necropsia() As Integer
            Get
                Return _flg_necropsia
            End Get
            Set(ByVal value As Integer)
                _flg_necropsia = value
            End Set
        End Property

        Public Property cod_altamedica() As String
            Get
                Return _cod_altamedica
            End Get
            Set(ByVal value As String)
                _cod_altamedica = value
            End Set
        End Property

        Public Property dsc_observacion() As String
            Get
                Return _dsc_observacion
            End Get
            Set(ByVal value As String)
                _dsc_observacion = value
            End Set
        End Property

        Public Property dsc_tratamiento() As String
            Get
                Return _dsc_tratamiento
            End Get
            Set(ByVal value As String)
                _dsc_tratamiento = value
            End Set
        End Property

        Public Property dsc_evolucion() As String
            Get
                Return _dsc_evolucion
            End Get
            Set(ByVal value As String)
                _dsc_evolucion = value
            End Set
        End Property

        Public Property dsc_examenauxiliar() As String
            Get
                Return _dsc_examenauxiliar
            End Get
            Set(ByVal value As String)
                _dsc_examenauxiliar = value
            End Set
        End Property


        Public Property dsc_peso() As Decimal
            Get
                Return _dsc_peso
            End Get
            Set(ByVal value As Decimal)
                _dsc_peso = value
            End Set
        End Property

        Public Property dsc_frecuenciarespiratoria() As Decimal
            Get
                Return _dsc_frecuenciarespiratoria
            End Get
            Set(ByVal value As Decimal)
                _dsc_frecuenciarespiratoria = value
            End Set
        End Property

        Public Property dsc_frecuenciacardiaca() As Decimal
            Get
                Return _dsc_frecuenciacardiaca
            End Get
            Set(ByVal value As Decimal)
                _dsc_frecuenciacardiaca = value
            End Set
        End Property

        Public Property dsc_talla() As Decimal
            Get
                Return _dsc_talla
            End Get
            Set(ByVal value As Decimal)
                _dsc_talla = value
            End Set
        End Property

        Public Property dsc_presionarterial() As String
            Get
                Return _dsc_presionarterial
            End Get
            Set(ByVal value As String)
                _dsc_presionarterial = value
            End Set
        End Property

        Public Property dsc_examenfisico() As String
            Get
                Return _dsc_examenfisico
            End Get
            Set(ByVal value As String)
                _dsc_examenfisico = value
            End Set
        End Property

        Public Property dsc_enfermedad_actual() As String
            Get
                Return _dsc_enfermedad_actual
            End Get
            Set(ByVal value As String)
                _dsc_enfermedad_actual = value
            End Set
        End Property






        Public Property CodProcedimiento() As String
            Get
                Return _CodProcedimiento
            End Get
            Set(ByVal value As String)
                _CodProcedimiento = value
            End Set
        End Property


        Public Property TipoValidacion() As Integer
            Get
                Return _TipoValidacion
            End Get
            Set(ByVal value As Integer)
                _TipoValidacion = value
            End Set
        End Property

        Public Property Tipo() As String
            Get
                Return _Tipo
            End Get
            Set(ByVal value As String)
                _Tipo = value
            End Set
        End Property

        Public Property IdeGeneral() As Integer
            Get
                Return _IdeGeneral
            End Get
            Set(ByVal value As Integer)
                _IdeGeneral = value
            End Set
        End Property

        Public Property TipoDoc() As Integer
            Get
                Return _TipoDoc
            End Get
            Set(ByVal value As Integer)
                _TipoDoc = value
            End Set
        End Property

        Public Property IdHospitalDoc() As Integer
            Get
                Return _IdHospitalDoc
            End Get
            Set(ByVal value As Integer)
                _IdHospitalDoc = value
            End Set
        End Property

        Public Property IdDocumento() As Integer
            Get
                Return _IdDocumento
            End Get
            Set(ByVal value As Integer)
                _IdDocumento = value
            End Set
        End Property


        Public Property CodUser() As Integer
            Get
                Return _CodUser
            End Get
            Set(ByVal value As Integer)
                _CodUser = value
            End Set
        End Property
        Public Property IdeHistoria() As Integer
            Get
                Return _IdeHistoria
            End Get
            Set(ByVal value As Integer)
                _IdeHistoria = value
            End Set
        End Property
        Public Property CodPaciente() As String
            Get
                Return _CodPaciente
            End Get
            Set(ByVal value As String)
                _CodPaciente = value
            End Set
        End Property
        Public Property CodMedico() As String
            Get
                Return _CodMedico
            End Get
            Set(ByVal value As String)
                _CodMedico = value
            End Set
        End Property

        Public Property Descripcion() As String
            Get
                Return _Descripcion
            End Get
            Set(ByVal value As String)
                _Descripcion = value
            End Set
        End Property

        Public Property Campo() As String
            Get
                Return _Campo
            End Get
            Set(ByVal value As String)
                _Campo = value
            End Set
        End Property
        Public Property ValorNuevo() As String
            Get
                Return _ValorNuevo
            End Get
            Set(ByVal value As String)
                _ValorNuevo = value
            End Set
        End Property

        Public Property IdePlantilla() As String
            Get
                Return _IdePlantilla
            End Get
            Set(ByVal value As String)
                _IdePlantilla = value
            End Set
        End Property
        Public Property CodAtencion() As String
            Get
                Return _CodAtencion
            End Get
            Set(ByVal value As String)
                _CodAtencion = value
            End Set
        End Property

        Public Property Orden() As Integer
            Get
                Return _Orden
            End Get
            Set(ByVal value As Integer)
                _Orden = value
            End Set
        End Property

        Public Property Servicio() As String
            Get
                Return _Servicio
            End Get
            Set(ByVal value As String)
                _Servicio = value
            End Set
        End Property

        Public Property Pabellon() As String
            Get
                Return _Pabellon
            End Get
            Set(ByVal value As String)
                _Pabellon = value
            End Set
        End Property

        Public Property NombrePaciente() As String
            Get
                Return _NombrePaciente
            End Get
            Set(ByVal value As String)
                _NombrePaciente = value
            End Set
        End Property

        Public Property PesoPaciente As String
            Get
                Return _PesoPaciente
            End Get
            Set(value As String)
                _PesoPaciente = value
            End Set
        End Property

        Public Property Ide_kardexhospitalizacion As Integer
            Get
                Return _Ide_kardexhospitalizacion
            End Get
            Set(value As Integer)
                _Ide_kardexhospitalizacion = value
            End Set
        End Property
    End Class
End Namespace


