Namespace gsol

    <Serializable()>
    Public Class SectorEntorno
        Implements ISectorEntorno



#Region "Atributos"

        Private _idenficador As Int32
        Private _nombreSector As String
        'Credenciales
        Private _usuario As String
        Private _password As String
        Private _fechaBinario As Date
        Private _divisionEmpresarial As Int32

        Private _grupoEmpresarial As Int32

        Private _aplicacion As Int32
        Private _permisos As Dictionary(Of Int32, IPermisos)
        Private _idioma As Int32

#End Region

#Region "Constructores"

        Sub New()
            _idenficador = 0
            _nombreSector = Nothing
            'Credenciales 
            _usuario = Nothing
            _idioma = 1
            _grupoEmpresarial = -1
            _divisionEmpresarial = -1
            _aplicacion = -1
            _permisos = New Dictionary(Of Int32, IPermisos)
        End Sub

        Sub New(
            ByVal usuario_ As String, _
            ByVal grupoEmpresarial_ As Int32, _
            ByVal divisionEmpresarial_ As Int32, _
            ByVal aplicacion_ As Int32, _
            ByVal identificador_ As Int32, _
            ByVal sector_ As String
        )
            _idenficador = identificador_
            _nombreSector = sector_
            _usuario = usuario_
            _grupoEmpresarial = grupoEmpresarial_
            _divisionEmpresarial = divisionEmpresarial_
            _aplicacion = aplicacion_
            _permisos = New Dictionary(Of Int32, IPermisos)

        End Sub

#End Region

#Region "Destructores"

#End Region

#Region "Propiedades"

        Public Property Identitificador As Integer _
            Implements ISectorEntorno.Identitificador
            Get
                Return Me._idenficador
            End Get
            Set(value As Integer)
                Me._idenficador = value
            End Set
        End Property

        Public ReadOnly Property NombreSector As String _
            Implements ISectorEntorno.NombreSector
            Get
                Return Me._nombreSector
            End Get
        End Property

        Public Property Permisos As Dictionary(Of Int32, IPermisos) _
            Implements ISectorEntorno.Permisos
            Get
                Return Me._permisos
            End Get
            Set(ByVal value As Dictionary(Of Int32, IPermisos))
                Me._permisos = value
            End Set
        End Property

        Public Property FechaBinario As Date _
    Implements ISectorEntorno.FechaBinario
            Get
                Return Me._fechaBinario
            End Get
            Set(ByVal value As Date)
                Me._fechaBinario = value
            End Set
        End Property

        Public Property Usuario As String _
        Implements ISectorEntorno.Usuario
            Get
                Return Me._usuario
            End Get
            Set(ByVal value As String)
                Me._usuario = value
            End Set
        End Property

        Public Property Password As String _
    Implements ISectorEntorno.Password
            Get
                Return Me._password
            End Get
            Set(ByVal value As String)
                Me._password = value
            End Set
        End Property

        Public Property DivisionEmpresarial As Int32 _
Implements ISectorEntorno.DivisionEmpresarial
            Get
                Return Me._divisionEmpresarial
            End Get
            Set(ByVal value As Int32)
                Me._divisionEmpresarial = value
            End Set
        End Property


        Public Property Idioma As Integer Implements ISectorEntorno.Idioma
            Get
                Return _idioma
            End Get
            Set(value As Integer)
                _idioma = value
            End Set
        End Property
        'Private _usuario As String
        'Private _password As String
        'Private _fechaBinario As Date
        'Private _divisionEmpresarial As Int32

#End Region

#Region "Metodos"

#End Region

    End Class

End Namespace


