Namespace OtrosE
    Public Class TablasE
        Private _CodTabla As String
        Private _Codigo As String
        Private _Nombre As String
        Private _Valor As String
        Private _Estado As String

        'Extensiones
        Private _Buscar As String = ""
        Private _Key As Integer
        Private _NumeroLineas As Integer
        Private _Orden As Integer

        Private _IdeHistoria As Integer = 0
        Private _IdeExamen As Integer = 0
        Private _IdeAlerta As Integer = 0
        Private _IdeUsuario As Integer = 0

        Public Property IdeUsuario() As Integer
            Get
                Return _IdeUsuario
            End Get
            Set(ByVal value As Integer)
                _IdeUsuario = value
            End Set
        End Property

        Public Property IdeAlerta() As Integer
            Get
                Return _IdeAlerta
            End Get
            Set(ByVal value As Integer)
                _IdeAlerta = value
            End Set
        End Property

        Public Property IdeExamen() As Integer
            Get
                Return _IdeExamen
            End Get
            Set(ByVal value As Integer)
                _IdeExamen = value
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

        Public Property CodTabla() As String
            Get
                Return _CodTabla
            End Get
            Set(ByVal value As String)
                _CodTabla = value
            End Set
        End Property

        Public Property Codigo() As String
            Get
                Return _Codigo
            End Get
            Set(ByVal value As String)
                _Codigo = value
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

        Public Property Valor() As String
            Get
                Return _Valor
            End Get
            Set(ByVal value As String)
                _Valor = value
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

        'Extensiones
        Public Property Buscar() As String
            Get
                Return _Buscar
            End Get
            Set(ByVal value As String)
                _Buscar = value
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

        Public Property NumeroLineas() As Integer
            Get
                Return _NumeroLineas
            End Get
            Set(ByVal value As Integer)
                _NumeroLineas = value
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
    End Class
End Namespace

