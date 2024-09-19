Public Class IndicacionesMedicasE
	Private _IdTipo As Integer? = 0
	Private _NombreTipo As String = ""
	Private _dsc_producto As String = ""
	Private _num_dosis As String = ""
	Private _dsc_via As String = ""
	Private _ide_receta As Integer? = 0
	Private _ide_medicamentorec As Integer? = 0
	Private _flg_suspendido As String = ""
	Private _txt_detalle As String = ""
	Private _num_duracion As String = ""
	Private _num_frecuencia As String = ""
	Private _num_cantidad As Decimal? = 0
	Private _HOR_REGISTRO As String = ""
	Private _HOR_REGISTRO2 As String = ""
	Private _Fecha As String = ""
	Private _NombreMedico As String = ""
	Private _cod_atencion As String = ""
	Private Seleccion As Boolean? = False
	Private _UserRegistro As String = ""
	Private _Observacion As String = ""
	Private _horaAtencion As String = ""
	Private _ide_KardexHospitalario As Integer? = 0
	Private Item As Integer? = 0
	Private _HoraSecundaria As Integer? = 0
	Private _UltimoFechaRegistro As String = ""
	Private _UltimoUserRegistro As String = ""
	Private _i_horasugerida As Integer? = 0
	Private _Icons As String = ""
	Private _SiguienteHora As String = ""
	Private _NumeracionFrecuencia As Integer? = 0
	'Public int? TotDetalle { Get; Set; } = 0;
	'Public int? SumEstado { Get; Set; } = 0;
	Private _TotDetalle As Integer? = 0
	Private _SumEstado As Integer? = 0

	Public Property NumeracionFrecuencia() As Integer
		Get
			Return _NumeracionFrecuencia
		End Get
		Set(value As Integer)
			_NumeracionFrecuencia = value
		End Set
	End Property

	Public Property SiguienteHora() As String
		Get
			Return _SiguienteHora
		End Get
		Set(value As String)
			_SiguienteHora = value
		End Set
	End Property
	Public Property Icons() As String
		Get
			Return _Icons
		End Get
		Set(value As String)
			_Icons = value
		End Set
	End Property

	Public Property i_horasugerida() As Integer
		Get
			Return _i_horasugerida
		End Get
		Set(value As Integer)
			_i_horasugerida = value
		End Set
	End Property

	Public Property UltimoUserRegistro() As String
		Get
			Return _UltimoUserRegistro
		End Get
		Set(value As String)
			_UltimoUserRegistro = value
		End Set
	End Property

	Public Property UltimoFechaRegistro() As String
		Get
			Return _UltimoFechaRegistro
		End Get
		Set(value As String)
			_UltimoFechaRegistro = value
		End Set
	End Property

	Public Property HoraSecundaria() As Integer
		Get
			Return _HoraSecundaria
		End Get
		Set(value As Integer)
			_HoraSecundaria = value
		End Set
	End Property

	Public Property _item() As Integer
		Get
			Return Item
		End Get
		Set(value As Integer)
			Item = value
		End Set
	End Property

	Public Property ide_KardexHospitalario() As Integer
		Get
			Return _ide_KardexHospitalario
		End Get
		Set(value As Integer)
			_ide_KardexHospitalario = value
		End Set
	End Property


	Public Property horaAtencion() As String
		Get
			Return _horaAtencion
		End Get
		Set(value As String)
			_horaAtencion = value
		End Set
	End Property

	Public Property Observacion() As String
		Get
			Return _Observacion
		End Get
		Set(value As String)
			_Observacion = value
		End Set
	End Property


	Public Property UserRegistro() As String
		Get
			Return _UserRegistro
		End Get
		Set(value As String)
			_UserRegistro = value
		End Set
	End Property

	Public Property _Seleccion() As Boolean
		Get
			Return Seleccion
		End Get
		Set(value As Boolean)
			Seleccion = value
		End Set
	End Property

	Public Property cod_atencion() As String
		Get
			Return _cod_atencion
		End Get
		Set(value As String)
			_cod_atencion = value
		End Set
	End Property

	Public Property NombreMedico() As String
		Get
			Return _NombreMedico
		End Get
		Set(value As String)
			_NombreMedico = value
		End Set
	End Property

	Public Property Fecha() As String
		Get
			Return _Fecha
		End Get
		Set(value As String)
			_Fecha = value
		End Set
	End Property

	Public Property HOR_REGISTRO2() As String
		Get
			Return _HOR_REGISTRO2
		End Get
		Set(value As String)
			_HOR_REGISTRO2 = value
		End Set
	End Property

	Public Property HOR_REGISTRO() As String
		Get
			Return _HOR_REGISTRO
		End Get
		Set(value As String)
			_HOR_REGISTRO = value
		End Set
	End Property


	Public Property num_cantidad() As Decimal
		Get
			Return _num_cantidad
		End Get
		Set(value As Decimal)
			_num_cantidad = value
		End Set
	End Property

	Public Property num_frecuencia() As String
		Get
			Return _num_frecuencia
		End Get
		Set(value As String)
			_num_frecuencia = value
		End Set
	End Property

	Public Property num_duracion() As String
		Get
			Return _num_duracion
		End Get
		Set(value As String)
			_num_duracion = value
		End Set
	End Property

	Public Property txt_detalle() As String
		Get
			Return _txt_detalle
		End Get
		Set(value As String)
			_txt_detalle = value
		End Set
	End Property

	Public Property flg_suspendido() As String
		Get
			Return _flg_suspendido
		End Get
		Set(value As String)
			_flg_suspendido = value
		End Set
	End Property

	Public Property ide_medicamentorec() As Integer
		Get
			Return _ide_medicamentorec
		End Get
		Set(value As Integer)
			_ide_medicamentorec = value
		End Set
	End Property

	Public Property ide_receta() As Integer
		Get
			Return _ide_receta
		End Get
		Set(value As Integer)
			_ide_receta = value
		End Set
	End Property

	Public Property dsc_via() As String
		Get
			Return _dsc_via
		End Get
		Set(ByVal value As String)
			_dsc_via = value
		End Set
	End Property

	Public Property num_dosis() As String
		Get
			Return _num_dosis
		End Get
		Set(ByVal value As String)
			_num_dosis = value
		End Set
	End Property

	Public Property IdTipo() As Integer
		Get
			Return _IdTipo
		End Get
		Set(ByVal value As Integer)
			_IdTipo = value
		End Set
	End Property

	Public Property NombreTipo() As String
		Get
			Return _NombreTipo
		End Get
		Set(value As String)
			_NombreTipo = value
		End Set
	End Property

	Public Property dsc_producto() As String
		Get
			Return _dsc_producto
		End Get
		Set(value As String)
			_dsc_producto = value
		End Set
	End Property

	Public Property TotDetalle As Integer?
		Get
			Return _TotDetalle
		End Get
		Set(value As Integer?)
			_TotDetalle = value
		End Set
	End Property

	Public Property SumEstado As Integer?
		Get
			Return _SumEstado
		End Get
		Set(value As Integer?)
			_SumEstado = value
		End Set
	End Property
End Class

Public Class IndicacionesMedicaDetalleE

	Private _ide_medicamentorec As Integer? = 0
	Private _Fecha As DateTime? = DateTime.Now
	Private _i_Idtipo As Integer? = 0
	Private _dsc_tipo As String = ""
	Private _dsc_producto As String = ""
	Private _usr_registra As String = ""
	Private _FInsert As String = ""
	Private _HInsert As String = ""
	Private _i_Correlativo As Integer = 0
	Private _Icons As String = ""
	Private _peso As String = ""
	Private _fechaprogramada As String = ""
	Private _usr_adminstra As String = ""
	Private _fechaadministrada As String = ""
	Private _dsc_tipoAdminstracio As String = ""
	Private _i_estadoadministrado As Integer? = 0

	Public Property ide_medicamentorec() As Integer?
		Get
			Return _ide_medicamentorec
		End Get
		Set(value As Integer?)
			_ide_medicamentorec = value
		End Set
	End Property

	Public Property Fecha() As Date
		Get
			Return _Fecha
		End Get
		Set(value As Date)
			_Fecha = value
		End Set
	End Property

	Public Property i_Idtipo() As Integer?
		Get
			Return _i_Idtipo
		End Get
		Set(value As Integer?)
			_i_Idtipo = value
		End Set
	End Property

	Public Property dsc_tipo() As String
		Get
			Return _dsc_tipo
		End Get
		Set(value As String)
			_dsc_tipo = value
		End Set
	End Property

	Public Property dsc_producto() As String
		Get
			Return _dsc_producto
		End Get
		Set(value As String)
			_dsc_producto = value
		End Set
	End Property

	Public Property usr_registra() As String
		Get
			Return _usr_registra
		End Get
		Set(value As String)
			_usr_registra = value
		End Set
	End Property

	Public Property FInsert() As String
		Get
			Return _FInsert
		End Get
		Set(value As String)
			_FInsert = value
		End Set
	End Property

	Public Property HInsert() As String
		Get
			Return _HInsert
		End Get
		Set(value As String)
			_HInsert = value
		End Set
	End Property

	Public Property i_Correlativo() As Integer
		Get
			Return _i_Correlativo
		End Get
		Set(value As Integer)
			_i_Correlativo = value
		End Set
	End Property

	Public Property Icons() As String
		Get
			Return _Icons
		End Get
		Set(value As String)
			_Icons = value
		End Set
	End Property

	Public Property peso() As String
		Get
			Return _peso
		End Get
		Set(value As String)
			_peso = value
		End Set
	End Property

	Public Property fechaprogramada As String
		Get
			Return _fechaprogramada
		End Get
		Set(value As String)
			_fechaprogramada = value
		End Set
	End Property

	Public Property usr_adminstra As String
		Get
			Return _usr_adminstra
		End Get
		Set(value As String)
			_usr_adminstra = value
		End Set
	End Property

	Public Property fechaadministrada As String
		Get
			Return _fechaadministrada
		End Get
		Set(value As String)
			_fechaadministrada = value
		End Set
	End Property

	Public Property dsc_tipoAdminstracio As String
		Get
			Return _dsc_tipoAdminstracio
		End Get
		Set(value As String)
			_dsc_tipoAdminstracio = value
		End Set
	End Property

	Public Property i_estadoadministrado As Integer?
		Get
			Return _i_estadoadministrado
		End Get
		Set(value As Integer?)
			_i_estadoadministrado = value
		End Set
	End Property
End Class

Public Class ProgramacionKardexE

	Private _item As Integer?
	Private _codatencion As String
	Private _ide_medicamentorec As Integer
	Private _i_Correlativo As Integer
	Private _i_estadoadministrado As Integer
	Private _fechaprogramada As String
	Private _horaprogramada As String
	Private _FechaAdministrada As String
	Private _usr_adminstra As String

	Public Property Item As Integer?
		Get
			Return _item
		End Get
		Set(value As Integer?)
			_item = value
		End Set
	End Property

	Public Property Codatencion As String
		Get
			Return _codatencion
		End Get
		Set(value As String)
			_codatencion = value
		End Set
	End Property

	Public Property Ide_medicamentorec As Integer
		Get
			Return _ide_medicamentorec
		End Get
		Set(value As Integer)
			_ide_medicamentorec = value
		End Set
	End Property

	Public Property I_Correlativo As Integer
		Get
			Return _i_Correlativo
		End Get
		Set(value As Integer)
			_i_Correlativo = value
		End Set
	End Property

	Public Property I_estadoadministrado As Integer
		Get
			Return _i_estadoadministrado
		End Get
		Set(value As Integer)
			_i_estadoadministrado = value
		End Set
	End Property

	Public Property Fechaprogramada As String
		Get
			Return _fechaprogramada
		End Get
		Set(value As String)
			_fechaprogramada = value
		End Set
	End Property

	Public Property Horaprogramada As String
		Get
			Return _horaprogramada
		End Get
		Set(value As String)
			_horaprogramada = value
		End Set
	End Property

	Public Property FechaAdministrada As String
		Get
			Return _FechaAdministrada
		End Get
		Set(value As String)
			_FechaAdministrada = value
		End Set
	End Property

	Public Property Usr_adminstra As String
		Get
			Return _usr_adminstra
		End Get
		Set(value As String)
			_usr_adminstra = value
		End Set
	End Property
End Class

Public Class KardexHospitalarioE
	'	Public int?  { Get; Set; }
	'        Public String?  { Get; Set; }
	'        Public String?  { Get; Set; }
	'        Public Decimal?  { Get; Set; }
	'        Public String?  { Get; Set; }
	'        Public String?  { Get; Set; }
	'        Public int?  { Get; Set; }
	'        Public DateTime?  { Get; Set; }
	'        Public DateTime?  { Get; Set; }
	'        Public DateTime?  { Get; Set; }
	'        Public int?  { Get; Set; }
	Private _ide_kardexhospitalario As Integer?
	Private _codatencion			As String
	Private _codpaciente As String
	Private _peso As Decimal?
	Private _usr_registra As String
	Private _estado As String
	Private _eliminado As Integer
	Private _fecharegistro As Date?
	Private _fechainicio As Date?
	Private _fechafin As Date?
	Private _order As Integer

	Public Property ide_kardexhospitalario As Integer?
		Get
			Return _ide_kardexhospitalario
		End Get
		Set(value As Integer?)
			_ide_kardexhospitalario = value
		End Set
	End Property

	Public Property codatencion As String
		Get
			Return _codatencion
		End Get
		Set(value As String)
			_codatencion = value
		End Set
	End Property

	Public Property codpaciente As String
		Get
			Return _codpaciente
		End Get
		Set(value As String)
			_codpaciente = value
		End Set
	End Property

	Public Property peso As Decimal?
		Get
			Return _peso
		End Get
		Set(value As Decimal?)
			_peso = value
		End Set
	End Property

	Public Property usr_registra As String
		Get
			Return _usr_registra
		End Get
		Set(value As String)
			_usr_registra = value
		End Set
	End Property

	Public Property estado As String
		Get
			Return _estado
		End Get
		Set(value As String)
			_estado = value
		End Set
	End Property

	Public Property eliminado As Integer
		Get
			Return _eliminado
		End Get
		Set(value As Integer)
			_eliminado = value
		End Set
	End Property

	Public Property fecharegistro As Date?
		Get
			Return _fecharegistro
		End Get
		Set(value As Date?)
			_fecharegistro = value
		End Set
	End Property

	Public Property fechainicio As Date?
		Get
			Return _fechainicio
		End Get
		Set(value As Date?)
			_fechainicio = value
		End Set
	End Property

	Public Property fechafin As Date?
		Get
			Return _fechafin
		End Get
		Set(value As Date?)
			_fechafin = value
		End Set
	End Property

	Public Property order As Integer
		Get
			Return _order
		End Get
		Set(value As Integer)
			_order = value
		End Set
	End Property
End Class