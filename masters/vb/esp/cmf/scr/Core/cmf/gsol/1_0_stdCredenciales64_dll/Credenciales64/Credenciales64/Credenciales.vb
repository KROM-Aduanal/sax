Namespace gsol

	Public Class Credenciales
        Implements ICredenciales


#Region "Atributos"

        'Basicos
        Private _claveUsuario As Int32
        Private _usuario As String
        Private _contrasenia As String
        Private _grupoEmpresarial As Integer
        Private _divisionEmpresarial As Integer
        Private _aplicacion As Integer
        'Seguridad
        Private _metodoCredencializacion As ICredenciales.MetodosCredencializacion
        Private _cifradoCredencializacion As ICredenciales.TipoCifrado
        Private _llaveCifrado As String
        'Inicia sesion
        Private _iniciaSesion As IIniciaSesion
        'Nombre autenticacion
        Private _nombreAutenticacion As String

        Private _claveEjecutivo As Int32


#End Region

#Region "Constructores"

        Sub New()
            _claveUsuario = Nothing
            _usuario = Nothing
            _contrasenia = Nothing
            _grupoEmpresarial = Nothing
            _divisionEmpresarial = Nothing
            _aplicacion = Nothing
            _metodoCredencializacion = ICredenciales.MetodosCredencializacion.Basico
            _cifradoCredencializacion = ICredenciales.TipoCifrado.SinDefinir
            _llaveCifrado = Nothing
            'Inicia sesion
            _iniciaSesion = New IniciaSesion
            'Nombre de autenticacion
            _nombreAutenticacion = Nothing
        End Sub

#End Region

#Region "Propiedades"

        Public ReadOnly Property EstadoAutenticacion As IIniciaSesion.StatusAutenticacion _
            Implements ICredenciales.EstadoAutenticacion
            Get
                Return _iniciaSesion.EstadoAutenticacion
            End Get
        End Property

        Public ReadOnly Property NombreAutenticacion As String _
            Implements ICredenciales.NombreAutenticacion
            Get
                Return _iniciaSesion.NombreAutenticacion
            End Get
        End Property

        Public Property CifradoCredencializacion As ICredenciales.TipoCifrado _
            Implements ICredenciales.CifradoCredencializacion
            Get
                Return _cifradoCredencializacion
            End Get
            Set(ByVal value As ICredenciales.TipoCifrado)
                _cifradoCredencializacion = value
            End Set
        End Property

        Public Property ContrasenaUsuario As String Implements _
            ICredenciales.ContraseniaUsuario
            Get
                Return _contrasenia
            End Get
            Set(ByVal value As String)
                _contrasenia = value
            End Set
        End Property

        Public Property CredencialUsuario As String _
            Implements ICredenciales.CredencialUsuario
            Get
                Return _usuario
            End Get
            Set(ByVal value As String)
                _usuario = value
            End Set
        End Property

        Public Property LlaveCifrado As String _
            Implements ICredenciales.LlaveCifrado
            Get
                Return _llaveCifrado
            End Get
            Set(ByVal value As String)
                _llaveCifrado = value
            End Set
        End Property

        Public Property MetodoCredencializacion As ICredenciales.MetodosCredencializacion _
            Implements ICredenciales.MetodoCredencializacion
            Get
                Return _metodoCredencializacion
            End Get
            Set(ByVal value As ICredenciales.MetodosCredencializacion)
                _metodoCredencializacion = value
            End Set
        End Property

        Public Property DivisionEmpresaria As Integer _
            Implements ICredenciales.DivisionEmpresaria
            Get
                Return _divisionEmpresarial
            End Get
            Set(ByVal value As Integer)
                _divisionEmpresarial = value
            End Set
        End Property

        Public Property GrupoEmpresarial As Integer _
            Implements ICredenciales.GrupoEmpresarial
            Get
                Return _grupoEmpresarial
            End Get
            Set(ByVal value As Integer)
                _grupoEmpresarial = value
            End Set
        End Property

        Public Property Aplicacion As Integer _
            Implements ICredenciales.Aplicacion
            Get
                Return _aplicacion
            End Get
            Set(ByVal value As Integer)
                _aplicacion = value
            End Set
        End Property

        Public ReadOnly Property GruposEmpresariales As DataSet _
            Implements ICredenciales.GruposEmpresariales
            Get
                Return _iniciaSesion.GruposEmpresariales
            End Get
        End Property

        Public ReadOnly Property ClaveUsuario As Integer _
            Implements ICredenciales.ClaveUsuario
            Get
                Return _iniciaSesion.IniciaSesionCalveUsuario
            End Get

        End Property

        Public ReadOnly Property ClaveEjecutivo As Integer Implements ICredenciales.ClaveEjecutivo
            Get
                Return _iniciaSesion.ClaveEjecutivo
            End Get

        End Property





        Public Property AsignarClaveUsuario As Integer Implements ICredenciales.AsignarClaveUsuario

            Set(value As Integer)

                _iniciaSesion.AsignaIniciaSesionClaveUsuario = value
            End Set

            Get

                Return _iniciaSesion.IniciaSesionCalveUsuario

            End Get

        End Property

#End Region

#Region "Metodos"

        Public Sub CargarGruposEmpresariales() Implements ICredenciales.CargarGruposEmpresariales
            _iniciaSesion = New IniciaSesion(_usuario, _contrasenia)
        End Sub

        Public Sub IniciarSesion() Implements ICredenciales.IniciarSesion
            _iniciaSesion = New IniciaSesion(_usuario, _contrasenia, _grupoEmpresarial, _divisionEmpresarial, _aplicacion)
        End Sub

#End Region



    End Class

End Namespace

