Imports gsol.monitoreo
Imports gsol.basededatos

Namespace gsol

	Public Class Organismo

#Region "Atributos"

		'Elementos de identificación de apps
		Private _nombreObjeto As String
		Private _numeroParte As Int32
		'Documentación de observaciones y errores
		Private _bitacora As IBitacoras
		'Cadena de conexión
		Private _conexion As IConexiones
		'Nombre de la pila
		Private _trazaPila As New System.Diagnostics.StackTrace()

#End Region

#Region "Constructores"

		Sub New()
			'Bitacora
			_bitacora = New BitacoraExcepciones
			'Capa de datos
			_conexion = New Conexiones
			'Inicializacion de identificación
			_nombreObjeto = NombreMetodo(_trazaPila.GetFrame(0))
			_numeroParte = 0
			'Configuración por defecto
			_bitacora.DirectorioBitacoras = "C:\logs"
			_bitacora.NombreArchivo = "SinNombre "
		End Sub

#End Region

#Region "Propiedades"

		Property Conexion As IConexiones
			Get
				Return _conexion
			End Get
			Set(ByVal value As IConexiones)
				_conexion = value
			End Set
		End Property

		Property NombreObjeto() As String
			Get
				Return _nombreObjeto
			End Get
			Set(ByVal value As String)
				_nombreObjeto = value
			End Set
		End Property

		Property NumeroParte() As Int32
			Get
				Return _nombreObjeto
			End Get
			Set(ByVal value As Int32)
				_numeroParte = value
			End Set
		End Property

		Property DirectorioBitacoras() As String
			Get
				Return _bitacora.DirectorioBitacoras
			End Get
			Set(ByVal value As String)
				_bitacora.DirectorioBitacoras = value
			End Set
		End Property

		Property NombreArchivo() As String
			Get
				Return _bitacora.NombreArchivo
			End Get
			Set(ByVal value As String)
				_bitacora.NombreArchivo = value
			End Set
		End Property

#End Region

#Region "Methods"

		Public Sub IniciarConexionBaseDatos()
			'Conecta base de datos
			_conexion.IpServidor = "localhost"
			_conexion.Usuario = "root"
			_conexion.Contrasena = "123"
			_conexion.PuertoConexion = 3306
			_conexion.NombreBaseDatos = "none"
		End Sub

		Public Sub LogError(
			ByVal excepcion_ As Exception
		)
			_bitacora.DocumentaError(
				_nombreObjeto, _
				_numeroParte, _
				excepcion_
			)
		End Sub

		Public Sub LogObservacion(
			ByVal mensaje_ As String
		)
			_bitacora.DocumentaObservacion(
				_nombreObjeto, _
				mensaje_
			)
		End Sub

		Private Function NombreMetodo(
			ByVal pila_ As StackFrame
		) As String
			' Regresa nombre de la función o metodo
			Dim nombreMetodo_ As String = pila_.GetMethod().Name

			Return nombreMetodo_
		End Function

#End Region

	End Class

End Namespace


