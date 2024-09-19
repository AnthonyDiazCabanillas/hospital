Namespace AnamnesisE
    Public Class RceAtencionesE
        Private _Orden As Integer
        Private _CodPaciente As String = ""
        Private _Sede As String = ""

        Public Property Sede() As String
            Get
                Return _Sede
            End Get
            Set(ByVal value As String)
                _Sede = value
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
        Public Property CodPaciente() As String
            Get
                Return _CodPaciente
            End Get
            Set(ByVal value As String)
                _CodPaciente = value
            End Set
        End Property

    End Class
End Namespace
