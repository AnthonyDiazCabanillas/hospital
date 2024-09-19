
Namespace PatologiaE
    Public Class RcePatologiaMaeE

        Private _IdePatologiaMae As Integer = 0
        Private _Orden As Integer
        Private _IdeTipoAtencion As String = ""
        Private _CodMedico As String = ""
        Private _Nombre As String = ""
        Private _IdeOrgano As Integer = 0

        Public Property IdeOrgano() As Integer
            Get
                Return _IdeOrgano
            End Get
            Set(ByVal value As Integer)
                _IdeOrgano = value
            End Set
        End Property
        Public Property IdePatologiaMae() As Integer
            Get
                Return _IdePatologiaMae
            End Get
            Set(ByVal value As Integer)
                _IdePatologiaMae = value
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

        Public Property IdeTipoAtencion() As String
            Get
                Return _IdeTipoAtencion
            End Get
            Set(ByVal value As String)
                _IdeTipoAtencion = value
            End Set
        End Property

    End Class
End Namespace

