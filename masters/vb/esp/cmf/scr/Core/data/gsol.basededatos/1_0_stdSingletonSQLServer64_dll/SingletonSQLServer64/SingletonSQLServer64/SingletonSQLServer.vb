Imports gsol.basededatos

Namespace Gsol.basedatos
	Public Class SingletonSQLServer

#Region "Atributos"

		Private Shared _instancia As SingletonSQLServer = Nothing

		Private _controlador As IConexiones.Controladores
		Private _host As String
		Private _puerto As String
		Private _usuario As String
		Private _basedatos As String
		Private _contrasena As String

		Private _iconexion As IConexiones

		'Status
		Private _estadoconexion As Boolean

		'Load configuration
		Private _configuracion As ILeerArchivo

#End Region

#Region "Constructores"

		Private Sub New()

            _iconexion = New Conexiones

			_configuracion = New LeerArchivoXML32

			_configuracion.Encriptado = ILeerArchivo.TiposCifrado.RijndaelManaged
			_configuracion.Llave = "KcjaSJElOTn2pIXa6qAVufUABuiXSbNb16KfjWjsMEQ="
			_configuracion.LeerXML()
			_iconexion.Usuario = _configuracion.RegresaValor("_UsuarioSQLServerGeneralProduccion")
			_iconexion.Contrasena = _configuracion.RegresaValor("_ClaveSQLServerGeneralProduccion")
			_iconexion.NombreBaseDatos = _configuracion.RegresaValor("_BaseDatosSQLServerProduccion")
			_iconexion.IpServidor = _configuracion.RegresaValor("_DireccionIPServidorSQLServerGeneralProduccion")
			'Configuración
            _iconexion.ControladorBaseDatos = IConexiones.Controladores.SQLServer2008

            _iconexion.ObjetoDatos = IConexiones.TipoConexion.SqlCommand

            'Init first vars
            _estadoconexion = _iconexion.IniciaConexion()

            'Collect data for connection
            _controlador = New IConexiones.Controladores

        End Sub

#End Region

#Region "Propiedades"

        Public ReadOnly Property EstaConectado() As Boolean
            Get
                Return _estadoconexion
            End Get
        End Property

        Public ReadOnly Property SQLServerSingletonConexion As IConexiones
            Get
                Return _iconexion
            End Get
        End Property

#End Region

#Region "Metodos"

        Private Sub IniciarConexion()

            Try
                _estadoconexion = _iconexion.IniciaConexion()

            Catch

                _estadoconexion = False

            End Try


            'If Not _estadoconexion Then
            '    Try
            '        _iconexion.IniciaConexion()
            '        'flag
            '        _estadoconexion = True

            '    Catch

            '        'flag
            '        _estadoconexion = False
            '    End Try

            'End If
        End Sub

        Private Sub TerminaConexion()
            If _estadoconexion Then

                _iconexion.TerminaConexion()

                'flag
                _estadoconexion = False
            End If
        End Sub


        Public Shared Function ObtenerInstancia() As SingletonSQLServer

            If _instancia Is Nothing Then

                _instancia = New SingletonSQLServer()

            End If

            _instancia.IniciarConexion()

            Return _instancia

        End Function

#End Region

#Region "Propiedades"

        Public Property UsuarioCliente As String
            Get
                Return _iconexion.NombreUsuarioCliente
            End Get
            Set(ByVal value As String)
                _iconexion.NombreUsuarioCliente = value
            End Set
        End Property

        Public Property ModuloCliente As String
            Get
                Return _iconexion.ModuloCliente
            End Get
            Set(ByVal value As String)
                _iconexion.ModuloCliente = value
            End Set
        End Property

        Public Property Controlador As IConexiones.Controladores
            Get
                Return _controlador
            End Get
            Set(ByVal value As IConexiones.Controladores)
                _controlador = value
            End Set
        End Property

        Public Property Host As String
            Get
                Return _host
            End Get
            Set(ByVal value As String)
                _host = value
            End Set
        End Property

        Public Property Contrasena As String
            Get
                Return _contrasena
            End Get
            Set(ByVal value As String)
                _contrasena = value
            End Set
        End Property

        Public Property Puerto As String
            Get
                Return _puerto
            End Get
            Set(ByVal value As String)
                _puerto = value
            End Set
        End Property


        Public Property Usuario As String
            Get
                Return _usuario
            End Get
            Set(ByVal value As String)
                _usuario = value
            End Set
        End Property

#End Region

	End Class

End Namespace



