Namespace NotaIngresoE
    Public Class RceNotaIngresoE
        Private _IdHistoria As Integer
        Private _CodMedico As String
        Private _IdUsuario As Integer
        Private _IdNotaIngreso As Integer
        Private _IdCampo As String
        Private _ValorCampo As String

        Private _Orden As Integer
        Public Property Orden() As Integer
            Get
                Return _Orden
            End Get
            Set(ByVal value As Integer)
                _Orden = value
            End Set
        End Property


        Public Property ValorCampo() As String
            Get
                Return _ValorCampo
            End Get
            Set(ByVal value As String)
                _ValorCampo = value
            End Set
        End Property
        Public Property IdCampo() As String
            Get
                Return _IdCampo
            End Get
            Set(ByVal value As String)
                _IdCampo = value
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

        Public Property IdHistoria() As Integer
            Get
                Return _IdHistoria
            End Get
            Set(ByVal value As Integer)
                _IdHistoria = value
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

        Public Property IdNotaIngreso() As Integer
            Get
                Return _IdNotaIngreso
            End Get
            Set(ByVal value As Integer)
                _IdNotaIngreso = value
            End Set
        End Property

    End Class
End Namespace


