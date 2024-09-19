
Namespace InicioSesionE
    Public Class RceInicioSesionE
        Private _DocIdentidad As String = ""
        Private _CodigoUsuario As String = ""
        Private _Clave As String = ""
        Private _DscIpPC As String = ""
        Private _DscPcName As String = ""

        Private _IdeSesion As String = ""
        Private _CodMedico As String = ""
        Private _CodUser As Integer
        Private _Mensaje As String = ""
        Private _Login As String = ""
        Private _Orden As Integer
        Private _IdeOpcionSupe As Integer
        Private _IdeModulo As Integer
        Private _CodOpcion As String

        Private _Campo As String = ""
        Private _Valor As String = ""

        Private _IdeLog As Integer
        Private _IdeHistoria As Integer
        Private _Formulario As String
        Private _Control As String
        Private _DscLog As String

        Private _Sede As String

        Public Property Sede() As String
            Get
                Return _Sede
            End Get
            Set(ByVal value As String)
                _Sede = value
            End Set
        End Property
        Public Property DscLog() As String
            Get
                Return _DscLog
            End Get
            Set(ByVal value As String)
                _DscLog = value
            End Set
        End Property
        Public Property Control() As String
            Get
                Return _Control
            End Get
            Set(ByVal value As String)
                _Control = value
            End Set
        End Property
        Public Property Formulario() As String
            Get
                Return _Formulario
            End Get
            Set(ByVal value As String)
                _Formulario = value
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
        Public Property IdeLog() As Integer
            Get
                Return _IdeLog
            End Get
            Set(ByVal value As Integer)
                _IdeLog = value
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
        Public Property Campo() As String
            Get
                Return _Campo
            End Get
            Set(ByVal value As String)
                _Campo = value
            End Set
        End Property


        Public Property CodOpcion() As String
            Get
                Return _CodOpcion
            End Get
            Set(ByVal value As String)
                _CodOpcion = value
            End Set
        End Property

        Public Property IdeModulo() As Integer
            Get
                Return _IdeModulo
            End Get
            Set(ByVal value As Integer)
                _IdeModulo = value
            End Set
        End Property

        Public Property IdeOpcionSupe() As Integer
            Get
                Return _IdeOpcionSupe
            End Get
            Set(ByVal value As Integer)
                _IdeOpcionSupe = value
            End Set
        End Property

        Public Property CodigoUsuario() As String
            Get
                Return _CodigoUsuario
            End Get
            Set(ByVal value As String)
                _CodigoUsuario = value
            End Set
        End Property

        Public Property Clave() As String
            Get
                Return _Clave
            End Get
            Set(ByVal value As String)
                _Clave = value
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

        Public Property IdeSesion() As String
            Get
                Return _IdeSesion
            End Get
            Set(ByVal value As String)
                _IdeSesion = value
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

        Public Property CodUser() As Integer
            Get
                Return _CodUser
            End Get
            Set(ByVal value As Integer)
                _CodUser = value
            End Set
        End Property
        Public Property DscIpPC() As String
            Get
                Return _DscIpPC
            End Get
            Set(ByVal value As String)
                _DscIpPC = value
            End Set
        End Property

        Public Property DscPcName() As String
            Get
                Return _DscPcName
            End Get
            Set(ByVal value As String)
                _DscPcName = value
            End Set
        End Property
        Public Property DocIdentidad() As String
            Get
                Return _DocIdentidad
            End Get
            Set(ByVal value As String)
                _DocIdentidad = value
            End Set
        End Property
        

        Public Property Mensaje() As String
            Get
                Return _Mensaje
            End Get
            Set(ByVal value As String)
                _Mensaje = value
            End Set
        End Property
        Public Property Login() As String
            Get
                Return _Login
            End Get
            Set(ByVal value As String)
                _Login = value
            End Set
        End Property
    End Class
End Namespace

