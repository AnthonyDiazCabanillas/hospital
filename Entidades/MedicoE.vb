Namespace MedicoE
    Public Class MedicoE
        Private _CodMedico As String
        Private _Orden As Integer
        Private _Atencion As String

        Public Property Atencion() As String
            Get
                Return _Atencion
            End Get
            Set(ByVal value As String)
                _Atencion = value
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

        Public Property Orden() As String
            Get
                Return _Orden
            End Get
            Set(ByVal value As String)
                _Orden = value
            End Set
        End Property
    End Class
End Namespace


