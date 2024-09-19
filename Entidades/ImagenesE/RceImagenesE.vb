Namespace ImagenesE
    Public Class RceImagenesE
        Private _Orden As Integer
        Private _TipoDeAtencion As String
        Private _CodMedico As String
        Private _Nombre As String
        Private _CodAtencion As String = ""
        Private _IdeRecetaCab As Integer = 0
        Private _IdeRecetaDet As Integer = 0
        Private _IdeImagen As Integer = 0
        Private _UsrRegistra As Integer
        Private _DscReceta As String
        Private _EstImagen As String = ""
        Private _ValorNuevo As String
        Private _Campo As String
        Private _CodPresotor As String = ""
        Private _CodPrestacion As String = ""
        Private _IdImagenFavorito As Integer = 0
        Private _CodPaciente As String = ""
        Private _TipoExamen As Integer = 0

        Private _CodTabla As String = ""
        Private _Buscar As String = ""
        Private _Key As Integer = 0
        Private _NumeroLineas As Integer = 0
        Private _IdeImagenPadre As Integer = 0
        Private _IdeImagenTitulo As Integer = 0
        Private _CodLocal As String = "0"

        'TMACASSI 31/10/2016
        Private _Codigo As String = ""
        Private _valor As Boolean = True
        Private _estado As String = ""
        'TMACASSI 31/10/2016

        Private _IdeUsr As Integer = 0

        'WS
        Private _ORACLE As String = ""
        Private _X_TIPOMSG As String = ""
        Private _T_COD_EMPRESA As Integer = 0
        Private _T_COD_SUCURSAL As Integer = 0
        Private _T_EVENT_ID As String = ""
        Private _T_EVENT_DATETIME As String = ""
        Private _T_EVENT_TYPE_ID As String = ""
        Private _X_ID_PACIENTE As String = ""
        Private _X_RUT_PACIENTE As String = ""
        Private _X_TIPO_PACIENTE As String = ""
        Private _X_DEATH_INDICATOR As String = ""
        Private _X_CAT_NAME As String = ""
        Private _X_LAST_NAME As String = ""
        Private _X_FIRST_NAME As String = ""
        Private _X_BIRTH_DATE As String = ""
        Private _X_GENDER_KEY As String = ""
        Private _X_LAST_UPDATED As String = ""
        Private _X_STREET_ADDRESS As String = ""
        Private _X_CITY As String = ""
        Private _X_COUNTRY As String = ""
        Private _X_PHONE_NUMBER As String = ""
        Private _X_VISIT_NUMBER As String = ""
        Private _X_START_DATETIME As String = ""
        Private _X_DURATION As String = ""
        Private _X_STATUS_KEY As String = ""
        Private _X_STATUS As String = ""
        Private _X_PROCEDURE_CODE As String = ""
        Private _X_PROCEDURE_DESCRIPTION As String = ""
        Private _X_ROOM_CODE As String = ""
        Private _X_REQUESTED_BY As String = ""
        Private _X_MESSAGE_TYPE As String = ""
        Private _X_PACS_SPS_ID As String = ""

        Private _MSG_STATUS As String = ""


        Private _FecReceta As String = ""
        Private _HorReceta As String = ""
        Private _CodPresotorNew As String = ""
        Private _DocIdentidad As String = ""


        Public Property DocIdentidad() As String
            Get
                Return _DocIdentidad
            End Get
            Set(ByVal value As String)
                _DocIdentidad = value
            End Set
        End Property

        Public Property CodPresotorNew() As String
            Get
                Return _CodPresotorNew
            End Get
            Set(ByVal value As String)
                _CodPresotorNew = value
            End Set
        End Property

        Public Property FecReceta() As String
            Get
                Return _FecReceta
            End Get
            Set(ByVal value As String)
                _FecReceta = value
            End Set
        End Property
        Public Property HorReceta() As String
            Get
                Return _HorReceta
            End Get
            Set(ByVal value As String)
                _HorReceta = value
            End Set
        End Property

        Public Property TipoExamen() As String
            Get
                Return _TipoExamen
            End Get
            Set(ByVal value As String)
                _TipoExamen = value
            End Set
        End Property

        Public Property MSG_STATUS() As String
            Get
                Return _MSG_STATUS
            End Get
            Set(ByVal value As String)
                _MSG_STATUS = value
            End Set
        End Property

        Public Property IdeUsr() As Integer
            Get
                Return _IdeUsr
            End Get
            Set(ByVal value As Integer)
                _IdeUsr = value
            End Set
        End Property

        Public Property ORACLE() As String
            Get
                Return _ORACLE
            End Get
            Set(ByVal value As String)
                _ORACLE = value
            End Set
        End Property

        Public Property X_PACS_SPS_ID() As String
            Get
                Return _X_PACS_SPS_ID
            End Get
            Set(ByVal value As String)
                _X_PACS_SPS_ID = value
            End Set
        End Property

        Public Property X_MESSAGE_TYPE() As String
            Get
                Return _X_MESSAGE_TYPE
            End Get
            Set(ByVal value As String)
                _X_MESSAGE_TYPE = value
            End Set
        End Property

        Public Property X_REQUESTED_BY() As String
            Get
                Return _X_REQUESTED_BY
            End Get
            Set(ByVal value As String)
                _X_REQUESTED_BY = value
            End Set
        End Property

        Public Property X_ROOM_CODE() As String
            Get
                Return _X_ROOM_CODE
            End Get
            Set(ByVal value As String)
                _X_ROOM_CODE = value
            End Set
        End Property

        Public Property X_PROCEDURE_DESCRIPTION() As String
            Get
                Return _X_PROCEDURE_DESCRIPTION
            End Get
            Set(ByVal value As String)
                _X_PROCEDURE_DESCRIPTION = value
            End Set
        End Property

        Public Property X_PROCEDURE_CODE() As String
            Get
                Return _X_PROCEDURE_CODE
            End Get
            Set(ByVal value As String)
                _X_PROCEDURE_CODE = value
            End Set
        End Property

        Public Property X_STATUS() As String
            Get
                Return _X_STATUS
            End Get
            Set(ByVal value As String)
                _X_STATUS = value
            End Set
        End Property

        Public Property X_STATUS_KEY() As String
            Get
                Return _X_STATUS_KEY
            End Get
            Set(ByVal value As String)
                _X_STATUS_KEY = value
            End Set
        End Property

        Public Property X_DURATION() As String
            Get
                Return _X_DURATION
            End Get
            Set(ByVal value As String)
                _X_DURATION = value
            End Set
        End Property

        Public Property X_START_DATETIME() As String
            Get
                Return _X_START_DATETIME
            End Get
            Set(ByVal value As String)
                _X_START_DATETIME = value
            End Set
        End Property

        Public Property X_VISIT_NUMBER() As String
            Get
                Return _X_VISIT_NUMBER
            End Get
            Set(ByVal value As String)
                _X_VISIT_NUMBER = value
            End Set
        End Property

        Public Property X_PHONE_NUMBER() As String
            Get
                Return _X_PHONE_NUMBER
            End Get
            Set(ByVal value As String)
                _X_PHONE_NUMBER = value
            End Set
        End Property

        Public Property X_COUNTRY() As String
            Get
                Return _X_COUNTRY
            End Get
            Set(ByVal value As String)
                _X_COUNTRY = value
            End Set
        End Property

        Public Property X_CITY() As String
            Get
                Return _X_CITY
            End Get
            Set(ByVal value As String)
                _X_CITY = value
            End Set
        End Property

        Public Property X_STREET_ADDRESS() As String
            Get
                Return _X_STREET_ADDRESS
            End Get
            Set(ByVal value As String)
                _X_STREET_ADDRESS = value
            End Set
        End Property

        Public Property X_LAST_UPDATED() As String
            Get
                Return _X_LAST_UPDATED
            End Get
            Set(ByVal value As String)
                _X_LAST_UPDATED = value
            End Set
        End Property

        Public Property X_GENDER_KEY() As String
            Get
                Return _X_GENDER_KEY
            End Get
            Set(ByVal value As String)
                _X_GENDER_KEY = value
            End Set
        End Property

        Public Property X_BIRTH_DATE() As String
            Get
                Return _X_BIRTH_DATE
            End Get
            Set(ByVal value As String)
                _X_BIRTH_DATE = value
            End Set
        End Property

        Public Property X_FIRST_NAME() As String
            Get
                Return _X_FIRST_NAME
            End Get
            Set(ByVal value As String)
                _X_FIRST_NAME = value
            End Set
        End Property

        Public Property X_LAST_NAME() As String
            Get
                Return _X_LAST_NAME
            End Get
            Set(ByVal value As String)
                _X_LAST_NAME = value
            End Set
        End Property

        Public Property X_CAT_NAME() As String
            Get
                Return _X_CAT_NAME
            End Get
            Set(ByVal value As String)
                _X_CAT_NAME = value
            End Set
        End Property

        Public Property X_DEATH_INDICATOR() As String
            Get
                Return _X_DEATH_INDICATOR
            End Get
            Set(ByVal value As String)
                _X_DEATH_INDICATOR = value
            End Set
        End Property

        Public Property X_TIPO_PACIENTE() As String
            Get
                Return _X_TIPO_PACIENTE
            End Get
            Set(ByVal value As String)
                _X_TIPO_PACIENTE = value
            End Set
        End Property

        Public Property X_RUT_PACIENTE() As String
            Get
                Return _X_RUT_PACIENTE
            End Get
            Set(ByVal value As String)
                _X_RUT_PACIENTE = value
            End Set
        End Property

        Public Property X_ID_PACIENTE() As String
            Get
                Return _X_ID_PACIENTE
            End Get
            Set(ByVal value As String)
                _X_ID_PACIENTE = value
            End Set
        End Property

        Public Property T_EVENT_TYPE_ID() As String
            Get
                Return _T_EVENT_TYPE_ID
            End Get
            Set(ByVal value As String)
                _T_EVENT_TYPE_ID = value
            End Set
        End Property

        Public Property T_EVENT_DATETIME() As String
            Get
                Return _T_EVENT_DATETIME
            End Get
            Set(ByVal value As String)
                _T_EVENT_DATETIME = value
            End Set
        End Property

        Public Property T_EVENT_ID() As String
            Get
                Return _T_EVENT_ID
            End Get
            Set(ByVal value As String)
                _T_EVENT_ID = value
            End Set
        End Property

        Public Property T_COD_SUCURSAL() As Integer
            Get
                Return _T_COD_SUCURSAL
            End Get
            Set(ByVal value As Integer)
                _T_COD_SUCURSAL = value
            End Set
        End Property

        Public Property T_COD_EMPRESA() As Integer
            Get
                Return _T_COD_EMPRESA
            End Get
            Set(ByVal value As Integer)
                _T_COD_EMPRESA = value
            End Set
        End Property

        Public Property X_TIPOMSG() As String
            Get
                Return _X_TIPOMSG
            End Get
            Set(ByVal value As String)
                _X_TIPOMSG = value
            End Set
        End Property







        Public Property CodLocal() As String
            Get
                Return _CodLocal
            End Get
            Set(ByVal value As String)
                _CodLocal = value
            End Set
        End Property

        Public Property IdeImagenTitulo() As Integer
            Get
                Return _IdeImagenTitulo
            End Get
            Set(ByVal value As Integer)
                _IdeImagenTitulo = value
            End Set
        End Property
        Public Property IdeImagenPadre() As Integer
            Get
                Return _IdeImagenPadre
            End Get
            Set(ByVal value As Integer)
                _IdeImagenPadre = value
            End Set
        End Property

        Public Property NumeroLineas() As Integer
            Get
                Return _NumeroLineas
            End Get
            Set(ByVal value As Integer)
                _NumeroLineas = value
            End Set
        End Property
        Public Property Key() As Integer
            Get
                Return _Key
            End Get
            Set(ByVal value As Integer)
                _Key = value
            End Set
        End Property
        Public Property Buscar() As String
            Get
                Return _Buscar
            End Get
            Set(ByVal value As String)
                _Buscar = value
            End Set
        End Property
        Public Property CodTabla() As String
            Get
                Return _CodTabla
            End Get
            Set(ByVal value As String)
                _CodTabla = value
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
        Public Property IdImagenFavorito() As Integer
            Get
                Return _IdImagenFavorito
            End Get
            Set(ByVal value As Integer)
                _IdImagenFavorito = value
            End Set
        End Property
        Public Property CodPrestacion() As String
            Get
                Return _CodPrestacion
            End Get
            Set(ByVal value As String)
                _CodPrestacion = value
            End Set
        End Property
        Public Property CodPresotor() As String
            Get
                Return _CodPresotor
            End Get
            Set(ByVal value As String)
                _CodPresotor = value
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
        Public Property EstImagen() As String
            Get
                Return _EstImagen
            End Get
            Set(ByVal value As String)
                _EstImagen = value
            End Set
        End Property
        Public Property DscReceta() As String
            Get
                Return _DscReceta
            End Get
            Set(ByVal value As String)
                _DscReceta = value
            End Set
        End Property
        Public Property UsrRegistra() As Integer
            Get
                Return _UsrRegistra
            End Get
            Set(ByVal value As Integer)
                _UsrRegistra = value
            End Set
        End Property
        Public Property IdeImagen() As Integer
            Get
                Return _IdeImagen
            End Get
            Set(ByVal value As Integer)
                _IdeImagen = value
            End Set
        End Property
        Public Property IdeRecetaDet() As Integer
            Get
                Return _IdeRecetaDet
            End Get
            Set(ByVal value As Integer)
                _IdeRecetaDet = value
            End Set
        End Property
        Public Property IdeRecetaCab() As Integer
            Get
                Return _IdeRecetaCab
            End Get
            Set(ByVal value As Integer)
                _IdeRecetaCab = value
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
        Public Property TipoDeAtencion() As String
            Get
                Return _TipoDeAtencion
            End Get
            Set(ByVal value As String)
                _TipoDeAtencion = value
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
        Public Property Nombre() As String
            Get
                Return _Nombre
            End Get
            Set(ByVal value As String)
                _Nombre = value
            End Set
        End Property

        'TMACASSI 31/10/2016
        Public Property Codigo() As String
            Get
                Return _Codigo
            End Get
            Set(ByVal value As String)
                _Codigo = value
            End Set
        End Property

        Public Property estado() As String
            Get
                Return _estado
            End Get
            Set(ByVal value As String)
                _estado = value
            End Set
        End Property

        Public Property valor() As Integer
            Get
                Return _valor
            End Get
            Set(ByVal value As Integer)
                _valor = value
            End Set
        End Property
        'TMACASSI 31/10/2016
    End Class
End Namespace


