Namespace LaboratorioE
    Public Class RceLaboratioE
        Private _Orden As Integer
        Private _TipoDeAtencion As String
        Private _CodMedico As String
        Private _Nombre As String = ""
        Private _IdAnalisisLaboratorio As Integer
        Private _IdlaboratorioTitulo As Integer = 0

        Private _CodAtencion As String = ""
        Private _IdeRecetaCab As Integer = 0
        Private _IdeRecetaDet As Integer = 0

        Private _UsrRegistra As Integer
        Private _IdAnalisisFavorito As Integer
        Private _EstAnalisis As String = ""
        Private _DscReceta As String
        Private _ValorNuevo As String
        Private _Campo As String
        Private _FlgCubierto As Boolean = False
        Private _CodDiagnostico As String
        Private _IdHistoria As Integer

        Private _IdeUsr As Integer = 0
        Private _FlgAnticipado As String = ""


        Private _FechaReceta As String = ""
        Private _HoraReceta As String = ""


        Public Property HoraReceta() As String
            Get
                Return _HoraReceta
            End Get
            Set(ByVal value As String)
                _HoraReceta = value
            End Set
        End Property
        Public Property FechaReceta() As String
            Get
                Return _FechaReceta
            End Get
            Set(ByVal value As String)
                _FechaReceta = value
            End Set
        End Property


        Public Property FlgAnticipado() As String
            Get
                Return _FlgAnticipado
            End Get
            Set(ByVal value As String)
                _FlgAnticipado = value
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

        Public Property IdHistoria() As String
            Get
                Return _IdHistoria
            End Get
            Set(ByVal value As String)
                _IdHistoria = value
            End Set
        End Property
        Public Property CodDiagnostico() As String
            Get
                Return _CodDiagnostico
            End Get
            Set(ByVal value As String)
                _CodDiagnostico = value
            End Set
        End Property

        Public Property FlgCubierto() As Boolean
            Get
                Return _FlgCubierto
            End Get
            Set(ByVal value As Boolean)
                _FlgCubierto = value
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
        Public Property DscReceta() As String
            Get
                Return _DscReceta
            End Get
            Set(ByVal value As String)
                _DscReceta = value
            End Set
        End Property
        Public Property EstAnalisis() As String
            Get
                Return _EstAnalisis
            End Get
            Set(ByVal value As String)
                _EstAnalisis = value
            End Set
        End Property
        Public Property IdAnalisisFavorito() As Integer
            Get
                Return _IdAnalisisFavorito
            End Get
            Set(ByVal value As Integer)
                _IdAnalisisFavorito = value
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

        Public Property IdlaboratorioTitulo() As Integer
            Get
                Return _IdlaboratorioTitulo
            End Get
            Set(ByVal value As Integer)
                _IdlaboratorioTitulo = value
            End Set
        End Property
        Public Property IdAnalisisLaboratorio() As Integer
            Get
                Return _IdAnalisisLaboratorio
            End Get
            Set(ByVal value As Integer)
                _IdAnalisisLaboratorio = value
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

    End Class
End Namespace

