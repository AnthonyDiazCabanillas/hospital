
Namespace AlergiaE
    Public Class RceAlergiaE
        Private _Buscar As String
        Private _Key As Integer
        Private _NumeroLineas As Integer
        Private _Orden As Integer
        Private _IdHistoria As Integer

        Private _Campo As String = ""
        Private _ValorNuevo As String = ""
        Private _Atencion As String
        Private _CodProducto As String


        Public Property CodProducto() As String
            Get
                Return _CodProducto
            End Get
            Set(ByVal value As String)
                _CodProducto = value
            End Set
        End Property

        Public Property IdHistoria() As Integer
            Get
                Return _IdHistoria
            End Get
            Set(ByVal value As Integer)
                _IdHistoria = value
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

        Public Property Atencion() As String
            Get
                Return _Atencion
            End Get
            Set(ByVal value As String)
                _Atencion = value
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

