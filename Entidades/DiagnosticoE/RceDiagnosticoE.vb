
Namespace DiagnosticoE
    Public Class RceDiagnosticoE
        Private _Orden As Integer
        Private _TipoDeAtencion As String
        Private _CodMedico As String
        Private _Nombre As String

        Private _CodAtencion As String = ""
        Private _Tipo As String
        Private _CodDiagnostico As String
        Private _IdeDiagnosticoFavorito As Integer
        Private _IdeHistoria As Integer

        Private _Campo As String
        Private _NuevoValor As String
        Private _TipoDianostico As String
        Private _UsrRegistra As Integer

        Public Property IdeHistoria() As Integer
            Get
                Return _IdeHistoria
            End Get
            Set(ByVal value As Integer)
                _IdeHistoria = value
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
        Public Property TipoDianostico() As String
            Get
                Return _TipoDianostico
            End Get
            Set(ByVal value As String)
                _TipoDianostico = value
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
        Public Property NuevoValor() As String
            Get
                Return _NuevoValor
            End Get
            Set(ByVal value As String)
                _NuevoValor = value
            End Set
        End Property
        Public Property IdeDiagnosticoFavorito() As Integer
            Get
                Return _IdeDiagnosticoFavorito
            End Get
            Set(ByVal value As Integer)
                _IdeDiagnosticoFavorito = value
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
        Public Property Tipo() As String
            Get
                Return _Tipo
            End Get
            Set(ByVal value As String)
                _Tipo = value
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

