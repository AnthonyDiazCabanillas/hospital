Namespace ControlClinicoE
    Public Class RceProductoE

        Private _Producto As String
        Private _CodAlmacen As String
        Private _Orden As Integer

        Private _Nombre As String
        Private _Key As String
        Private _NumeroDeLineas As String
        Private _CodAtencion As String '10/06/2021

        Public Property CodAtencion() As String
            Get
                Return _CodAtencion
            End Get
            Set(ByVal value As String)
                _CodAtencion = value
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
        Public Property Key() As String
            Get
                Return _Key
            End Get
            Set(ByVal value As String)
                _Key = value
            End Set
        End Property
        Public Property NumeroDeLineas() As String
            Get
                Return _NumeroDeLineas
            End Get
            Set(ByVal value As String)
                _NumeroDeLineas = value
            End Set
        End Property


        Public Property Producto() As String
            Get
                Return _Producto
            End Get
            Set(ByVal value As String)
                _Producto = value
            End Set
        End Property
        Public Property CodAlmacen() As String
            Get
                Return _CodAlmacen
            End Get
            Set(ByVal value As String)
                _CodAlmacen = value
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


