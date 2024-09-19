Namespace JuntaMedicaE
    Public Class RceJuntaMedicaE
        Private _CodAtencion As String
        Private _IdeJuntaMedica As Integer = 0
        Private _Orden As Integer
        Private _CodMedico As String
        Private _IdUsuario As Integer
        Private _DscJuntaMedica As String = ""
        Private _FecJuntaMedica As String = ""



        Public Property FecJuntaMedica() As String
            Get
                Return _FecJuntaMedica
            End Get
            Set(ByVal value As String)
                _FecJuntaMedica = value
            End Set
        End Property

        Public Property DscJuntaMedica() As String
            Get
                Return _DscJuntaMedica
            End Get
            Set(ByVal value As String)
                _DscJuntaMedica = value
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

        Public Property IdeJuntaMedica() As Integer
            Get
                Return _IdeJuntaMedica
            End Get
            Set(ByVal value As Integer)
                _IdeJuntaMedica = value
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
    End Class
End Namespace


