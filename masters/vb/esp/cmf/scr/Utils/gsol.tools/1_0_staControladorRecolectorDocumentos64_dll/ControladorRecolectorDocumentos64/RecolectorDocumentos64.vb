Imports System.Threading
Imports System.ComponentModel
Imports gsol.documento

Public Class RecolectorDocumentos64

#Region "Atributos"

    Private _clave As Int32

    Private _nombre As String

    Private _rutaRaiz As String

    Private _separadorSubdirectorios As String

    Private _divisionMiEmpresa As Int32

    Private _estatus As Boolean

    Private _fileWatcher As FileWatcher64

    Private _procesoEscuchaDirectorio As Thread

    Private _fechaActualizacionLista As DateTime

    Private _procesoRegistro As Thread

    Private _procesoReconocimiento As Thread

    Private _procesoRecoleccion As Thread

    Private _subprocesosRegistro As List(Of Thread)

    Private _subprocesosReconocimiento As List(Of Thread)

    Private _subprocesosRecoleccion As List(Of Thread)

    Private _estatusProcesoEscuchaDirectorio As Boolean

    Private _estatusProcesoRegistro As Boolean

    Private _estatusProcesoReconocimiento As Boolean

    Private _estatusProcesoRecoleccion As Boolean

    Private _operacionesDocumento As OperacionesDocumentoHilos64

    Private _lockRegistro As New Object

    Private _lockReconocimiento As New Object

    Private _lockRecoleccion As New Object

#End Region

#Region "Constructores"

    Sub New()

        _estatus = True

        _estatusProcesoEscuchaDirectorio = False

        _estatusProcesoRegistro = False

        _estatusProcesoReconocimiento = False

        _estatusProcesoRecoleccion = False

    End Sub

#End Region

#Region "Propiedades"

    Public Property Clave As Int32
        Get
            Return _clave
        End Get
        Set(value As Int32)
            _clave = value
        End Set
    End Property

    Public Property Nombre As String
        Get
            Return _nombre
        End Get
        Set(value As String)
            _nombre = value
        End Set
    End Property

    Public Property RutaRaiz As String
        Get
            Return _rutaRaiz
        End Get
        Set(value As String)
            _rutaRaiz = value
        End Set
    End Property

    Public Property SeparadorSubdirectorios As String
        Get
            Return _separadorSubdirectorios
        End Get
        Set(value As String)
            _separadorSubdirectorios = value
        End Set
    End Property

    Public Property DivisionMiEmpresa As Int32
        Get
            Return _divisionMiEmpresa
        End Get
        Set(value As Int32)
            _divisionMiEmpresa = value
        End Set
    End Property

    Public Property FileWatcher As FileWatcher64
        Get
            Return _fileWatcher
        End Get
        Set(value As FileWatcher64)
            _fileWatcher = value
        End Set
    End Property

    Public Property Estatus As Boolean
        Get
            Return _estatus
        End Get
        Set(value As Boolean)
            _estatus = value
        End Set
    End Property

    Public Property FechaActualizaLista As DateTime
        Get
            Return _fechaActualizacionLista
        End Get
        Set(value As DateTime)
            _fechaActualizacionLista = value
        End Set
    End Property

    Public Property ProcesoEscuchaDirectorio As Thread
        Get
            Return _procesoEscuchaDirectorio
        End Get
        Set(value As Thread)
            _procesoEscuchaDirectorio = value
        End Set
    End Property

    Public Property ProcesoRegistro As Thread
        Get
            Return _procesoRegistro
        End Get
        Set(value As Thread)
            _procesoRegistro = value
        End Set
    End Property

    Public Property ProcesoReconocimiento As Thread
        Get
            Return _procesoReconocimiento
        End Get
        Set(value As Thread)
            _procesoReconocimiento = value
        End Set
    End Property

    Public Property ProcesoRecoleccion As Thread
        Get
            Return _procesoRecoleccion
        End Get
        Set(value As Thread)
            _procesoRecoleccion = value
        End Set
    End Property

    Public Property SubProcesosRegistro As List(Of Thread)
        Get
            Return _subprocesosRegistro
        End Get
        Set(value As List(Of Thread))
            _subprocesosRegistro = value
        End Set
    End Property

    Public Property SubProcesosReconocimiento As List(Of Thread)
        Get
            Return _subprocesosReconocimiento
        End Get
        Set(value As List(Of Thread))
            _subprocesosReconocimiento = value
        End Set
    End Property

    Public Property SubProcesosRecoleccion As List(Of Thread)
        Get
            Return _subprocesosRecoleccion
        End Get
        Set(value As List(Of Thread))
            _subprocesosRecoleccion = value
        End Set
    End Property

    Public Property EstatusProcesoEscuchaDirectorio As Boolean
        Get
            Return _estatusProcesoEscuchaDirectorio
        End Get
        Set(value As Boolean)
            _estatusProcesoEscuchaDirectorio = value
        End Set
    End Property

    Public Property EstatusProcesoRegistro As Boolean
        Get
            Return _estatusProcesoRegistro
        End Get
        Set(value As Boolean)
            _estatusProcesoRegistro = value
        End Set
    End Property

    Public Property EstatusProcesoReconocimiento As Boolean
        Get
            Return _estatusProcesoReconocimiento
        End Get
        Set(value As Boolean)
            _estatusProcesoReconocimiento = value
        End Set
    End Property

    Public Property EstatusProcesoRecoleccion As Boolean
        Get
            Return _estatusProcesoRecoleccion
        End Get
        Set(value As Boolean)
            _estatusProcesoRecoleccion = value
        End Set
    End Property

    Public Property OperacionesDocumento As OperacionesDocumentoHilos64
        Get
            Return _operacionesDocumento
        End Get
        Set(value As OperacionesDocumentoHilos64)
            _operacionesDocumento = value
        End Set
    End Property

    Public Property LockRegistro As Object
        Get
            Return _lockRegistro
        End Get
        Set(value As Object)
            _lockRegistro = value
        End Set
    End Property

    Public Property LockReconocimiento As Object
        Get
            Return _lockReconocimiento
        End Get
        Set(value As Object)
            _lockReconocimiento = value
        End Set
    End Property

    Public Property LockRecoleccion As Object
        Get
            Return _lockRecoleccion
        End Get
        Set(value As Object)
            _lockRecoleccion = value
        End Set
    End Property

#End Region

#Region "Metodos"

    Public Sub DetenerProcesos()

        If _procesoEscuchaDirectorio.IsAlive Then

            _procesoEscuchaDirectorio.Abort()

            _estatusProcesoEscuchaDirectorio = False

        End If

        If _procesoRegistro.IsAlive Then

            _procesoRegistro.Abort()

            _estatusProcesoRegistro = False

        End If

        If _procesoReconocimiento.IsAlive Then

            _procesoReconocimiento.Abort()

            _estatusProcesoReconocimiento = False

        End If

        If _procesoRecoleccion.IsAlive Then

            _procesoRecoleccion.Abort()

            _estatusProcesoRecoleccion = False

        End If

        _procesoEscuchaDirectorio = Nothing

        _procesoRegistro = Nothing

        _procesoReconocimiento = Nothing

        _procesoRecoleccion = Nothing

    End Sub

    Public Sub ReiniciarProcesos()

        DetenerProcesos()

        If Not _procesoEscuchaDirectorio.IsAlive Then

            _procesoEscuchaDirectorio.Start()

            _estatusProcesoEscuchaDirectorio = True

        End If

        If Not _procesoRegistro.IsAlive Then

            _procesoRegistro.Start()

            _estatusProcesoRegistro = True

        End If

        If Not _procesoReconocimiento.IsAlive Then

            _procesoReconocimiento.Start()

            _estatusProcesoReconocimiento = True

        End If


        If Not _procesoRecoleccion.IsAlive Then

            _procesoRecoleccion.Start()

            _estatusProcesoRecoleccion = True

        End If

    End Sub

#End Region

#Region "Eventos"

#End Region

End Class
