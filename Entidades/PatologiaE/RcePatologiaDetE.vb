
Namespace PatologiaE
    Public Class RcePatologiaDetE
        Private _IdePatologiaCab As Integer
        Private _IdePatologiaMae As Integer
        Private _CodPrestacion As String = ""
        Private _CodPatologia As Integer = 0
        Private _CntExamen As Integer = 0
        Private _CodPresotor As String = ""
        Private _FlgEstado As String = ""
        Private _CodAtencion As String = ""
        Private _Orden As Integer
        Private _IdePatologiaDet As Integer

        'JB - nuevos valores muestra-datoclinica - 07/11/2018
        Private _Muestra As String = ""
        Private _DatoClinico As String = ""

        Public Property Muestra() As String
            Get
                Return _Muestra
            End Get
            Set(ByVal value As String)
                _Muestra = value
            End Set
        End Property
        Public Property DatoClinico() As String
            Get
                Return _DatoClinico
            End Get
            Set(ByVal value As String)
                _DatoClinico = value
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
        Public Property CodAtencion() As String
            Get
                Return _CodAtencion
            End Get
            Set(ByVal value As String)
                _CodAtencion = value
            End Set
        End Property
        Public Property FlgEstado() As String
            Get
                Return _FlgEstado
            End Get
            Set(ByVal value As String)
                _FlgEstado = value
            End Set
        End Property
        Public Property CodPresotor() As String
            Get
                Return _CodPresotor
            End Get
            Set(ByVal value As String)
                _CodPresotor = value
            End Set
        End Property
        Public Property CntExamen() As Integer
            Get
                Return _CntExamen
            End Get
            Set(ByVal value As Integer)
                _CntExamen = value
            End Set
        End Property
        Public Property CodPatologia() As Integer
            Get
                Return _CodPatologia
            End Get
            Set(ByVal value As Integer)
                _CodPatologia = value
            End Set
        End Property
        Public Property CodPrestacion() As String
            Get
                Return _CodPrestacion
            End Get
            Set(ByVal value As String)
                _CodPrestacion = value
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
        Public Property IdePatologiaCab() As Integer
            Get
                Return _IdePatologiaCab
            End Get
            Set(ByVal value As Integer)
                _IdePatologiaCab = value
            End Set
        End Property

        Public Property IdePatologiaDet() As Integer
            Get
                Return _IdePatologiaDet
            End Get
            Set(ByVal value As Integer)
                _IdePatologiaDet = value
            End Set
        End Property

        Private _NuevoValor As String
        Public Property NuevoValor() As String
            Get
                Return _NuevoValor
            End Get
            Set(ByVal value As String)
                _NuevoValor = value
            End Set
        End Property

        Private _Campo As String
        Public Property Campo() As String
            Get
                Return _Campo
            End Get
            Set(ByVal value As String)
                _Campo = value
            End Set
        End Property



        'Extensiones Otros 
        'Public CodAtencion As String
        Public FechaAdmision As DateTime
        Public HistorialPaciente As String
        Public Procedimiento As String
        Public Organo As String
        Public PacienteApPaterno As String
        Public PacienteApMaterno As String
        Public PacienteNombre As String
        Public PacienteTelefono As String
        Public Sexo As String
        Public EspecialidadNombres As String
        Public TipoDocumento As String
        Public Documento As String
        Public PacienteMail As String
        Public DxPresuntivo As String
        Public CMP As String
        Public MedicoAPPaterno As String
        Public MedicoAPMaterno As String
        Public UnidadReplicacion As String
        Public FechaLimiteAtencion As String
        Public CodigoHC As String
        Public NumeroCama As String
        Public Servicio As String
        Public Tarifario As String
        Public CodigoOA As String
        Public TipoOrden As String
        Public MedicoNombres As String
        Public IdOrdenAtencion As String
        Public CantidadSolicitada As String
        Public TipoAtencion As String
        Public Linea As String
        Public UsuarioCreacion As String
        Public FechaCreacion As String
        Public IpCreacion As String
        Public Componente As String
        Public FechaNacimiento As String
        Public ComponenteNombre As String
        Public Empleadora_Nombre As String
        Public Empleadora_Ruc As String
        Public Aseguradora_Nombre As String
        Public Aseguradora_Ruc As String
        Public Acceso As String
        Public EmpresaProveedor As String

    End Class
End Namespace

