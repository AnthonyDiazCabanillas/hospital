Namespace ControlClinicoE
    Public Class RceRecetaMedicamentoE

        Private _IdReceta As Integer = 0
        Private _IdHistoria As Integer = 0
        Private _IdUsuario As Integer
        Private _IdRecetaDet As Integer = 0
        Private _CodProducto As String
        Private _Campo As String
        Private _ValorNuevo As String
        Private _Orden As Integer
        Private _CodMedico As String


        Private _CodAtencion As String
        Private _FecReceta As String
        Private _HorReceta As String

        Private _IdFarmaco As Integer = 0

        Public Property IdFarmaco() As Integer
            Get
                Return _IdFarmaco
            End Get
            Set(ByVal value As Integer)
                _IdFarmaco = value
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
        Public Property CodAtencion() As String
            Get
                Return _CodAtencion
            End Get
            Set(ByVal value As String)
                _CodAtencion = value
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
        Public Property Orden() As Integer
            Get
                Return _Orden
            End Get
            Set(ByVal value As Integer)
                _Orden = value
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
        Public Property Campo() As String
            Get
                Return _Campo
            End Get
            Set(ByVal value As String)
                _Campo = value
            End Set
        End Property
        Public Property CodProducto() As String
            Get
                Return _CodProducto
            End Get
            Set(ByVal value As String)
                _CodProducto = value
            End Set
        End Property
        Public Property IdRecetaDet() As Integer
            Get
                Return _IdRecetaDet
            End Get
            Set(ByVal value As Integer)
                _IdRecetaDet = value
            End Set
        End Property
        Public Property IdUsuario() As Integer
            Get
                Return _IdUsuario
            End Get
            Set(ByVal value As Integer)
                _IdUsuario = value
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
        Public Property IdReceta() As Integer
            Get
                Return _IdReceta
            End Get
            Set(ByVal value As Integer)
                _IdReceta = value
            End Set
        End Property

    End Class
End Namespace

