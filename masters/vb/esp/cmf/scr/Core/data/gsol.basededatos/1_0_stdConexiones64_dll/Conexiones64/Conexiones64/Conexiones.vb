Imports System.Data.SqlClient
Imports System.Data
Imports MySql.Data.MySqlClient
Imports gsol.monitoreo
Imports ADODB
Imports Wma.Exceptions
Imports gsol.BaseDatos.Operaciones
Imports Wma.Operations
Imports gsol.monitoreo.SingletonBitacoras64

Namespace gsol.basededatos

    Public Class Conexiones
        Implements IConexiones

#Region "Constantes"

#End Region

#Region "Atributos"

        ' Singleton bitacóra
        Private _singletonBitacora As SingletonBitacoras

        '_singletonBitacora = SingletonBitacoras.ObtenerInstancia(4)

        'Parametros
        Public _cadenaconexion As String

        Private _ipservidor As String

        Private _puerto As String

        Private _usuario As String

        Private _nombrebasedatos As String

        Private _contrasena As String

        Public _estadoconexion As Boolean

        'Indices
        Private _indiceconjuntodataset As Int32

        'Gestion
        Private _controladorbasedatos As IConexiones.Controladores

        Private _objetodatos As IConexiones.TipoConexion

        Private _datasetreciente As DataSet

        Private _dataTableReciente As DataTable

        Private _conjuntodataset As Dictionary(Of Int32, DataSet)

        Private _conexion As IDbConnection

        Private _datadapter As IDbDataAdapter

        Private _command As IDbCommand

        'Obtejo para documentacion de errores 
        Private _bitacoras As IBitacoras

        Private _nombreUsuarioCliente As String

        Private _moduloCliente As String

        'Adaptaciones 2016

        Private _repositorioDatos As IConexiones.TiposRepositorio

        Private _tagWatcherObject As TagWatcher

        Private _bitacoraAvanzada As IBitacoras

        Private _i_Cve_Usuario As Int32 = 0 'IDUsuario = _i_Cve_Usuario
        Private _i_Cve_DivisionMiEmpresa As Int32 = 0 'conexion_.ClaveDivisionMiEmpresa = _i_Cve_DivisionMiEmpresa
        Private _i_Cve_Aplicacion As IBitacoras.ClaveTiposAplicacion = IBitacoras.ClaveTiposAplicacion.AplicacionEscritorioKrombase ' = 4 'conexion_.IDAplicacion = _i_Cve_Aplicacion


        Private _i_TipoInstrumentacion As IBitacoras.TiposInstrumentacion = IBitacoras.TiposInstrumentacion.GestorIOperaciones 'conexion_.TipoInstrumentacion = IBitacoras.TiposInstrumentacion.GestorIOperaciones
        Private _i_ModalidadConsulta As IOperacionesCatalogo.ModalidadesConsulta = IOperacionesCatalogo.ModalidadesConsulta.Singleton 'conexion_.ModalidadConsulta = _modalidadConsulta ' IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre
        Private _i_Cve_RecursoSolicitante As Int32 = 0 'conexion_.IDRecursoSolicitante = _i_Clave_Modulo

        Private _t_NombreEnsamblado As String

        Private _t_NombreVT As String

        Private _t_NombreUsuario As String

        Private _t_CuentaUsuario As String

        'Adaptaciones 2019

        Private _i_TiempoEspera As Int32 = 0

        'Continuidad de las bitacoras

        Private _activarBitacoraAvanzada As Boolean

        Private _activarBitacoraSimple As Boolean

        Private _saxappid As Int32? = Nothing


#End Region

#Region "Constructor"

        Sub New()

            _nombreUsuarioCliente = Nothing

            _moduloCliente = Nothing

            _indiceconjuntodataset = 0

            _ipservidor = Nothing

            _puerto = 0

            _nombrebasedatos = Nothing

            _usuario = Nothing

            _contrasena = Nothing

            _estadoconexion = False

            '_controladorbasedatos = IConexiones.Controladores.MySQL51
            _controladorbasedatos = IConexiones.Controladores.SQLServer2008

            '_objetodatos = IConexiones.TipoConexion.MySQLCommand
            _objetodatos = IConexiones.TipoConexion.SqlCommand

            _cadenaconexion = vbNullString

            _datasetreciente = New DataSet

            _dataTableReciente = New DataTable

            _conjuntodataset = New Dictionary(Of Int32, DataSet)

            _bitacoras = New BitacoraCapaDatos

            _repositorioDatos = IConexiones.TiposRepositorio.DataSetObject

            _tagWatcherObject = New TagWatcher


            'Continuidad de las bitacoras

            _singletonBitacora = SingletonBitacoras.ObtenerInstancia(4)

            _activarBitacoraAvanzada = _singletonBitacora.ActivarBitacoraAvanzada

            _activarBitacoraSimple = _singletonBitacora.ActivarBitacoraSimple

        End Sub

#End Region

#Region "Propiedades"

        Public Property SaxAppId As Int32? Implements IConexiones.SaxAppId

            Get

                Return _saxappid

            End Get

            Set(value As Int32?)

                _saxappid = value

            End Set

        End Property

        Public Property ActivarBitacoraAvanzada As Boolean _
    Implements IConexiones.ActivarBitacoraAvanzada

            Get
                Return _activarBitacoraAvanzada
            End Get
            Set(value As Boolean)
                _activarBitacoraAvanzada = value
            End Set

        End Property

        Public Property ActivarBitacoraSimple As Boolean _
    Implements IConexiones.ActivarBitacoraSimple

            Get
                Return _activarBitacoraSimple
            End Get
            Set(value As Boolean)
                _activarBitacoraSimple = value
            End Set

        End Property

        Public Property TiempoEspera As Integer _
            Implements IConexiones.TiempoEspera

            Get
                Return _i_TiempoEspera
            End Get
            Set(value As Integer)
                _i_TiempoEspera = value
            End Set

        End Property

        Public Property CuentaPublicaUsuario As String Implements IConexiones.CuentaPublicaUsuario
            Get
                Return _t_CuentaUsuario
            End Get
            Set(value As String)
                _t_CuentaUsuario = value
            End Set
        End Property

        Public Property NombreCompletoUsuario As String Implements IConexiones.NombreCompletoUsuario
            Get
                Return _t_NombreUsuario
            End Get
            Set(value As String)
                _t_NombreUsuario = value
            End Set
        End Property

        Public Property NombreEnsamblado As String Implements IConexiones.NombreEnsamblado
            Get
                Return _t_NombreEnsamblado
            End Get
            Set(value As String)
                _t_NombreEnsamblado = value
            End Set
        End Property

        Public Property NombreVT As String Implements IConexiones.NombreVT
            Get
                Return _t_NombreVT
            End Get
            Set(value As String)
                _t_NombreVT = value
            End Set
        End Property

        Public Property IDUsuario As Int32 Implements IConexiones.IDUsuario
            Get
                Return _i_Cve_Usuario
            End Get
            Set(value As Int32)
                _i_Cve_Usuario = value
            End Set
        End Property

        Public Property ClaveDivisionMiEmpresa As Int32 Implements IConexiones.ClaveDivisionMiEmpresa
            Get
                Return _i_Cve_DivisionMiEmpresa
            End Get
            Set(value As Int32)
                _i_Cve_DivisionMiEmpresa = value
            End Set
        End Property

        Public Property IDAplicacion As IBitacoras.ClaveTiposAplicacion Implements IConexiones.IDAplicacion
            Get
                Return _i_Cve_Aplicacion
            End Get
            Set(value As IBitacoras.ClaveTiposAplicacion)

                Dim i_Cve_Aplicacion_ As Int32 = 4

                Select Case value

                    Case IBitacoras.ClaveTiposAplicacion.AplicacionEscritorioKrombase

                        i_Cve_Aplicacion_ = 4

                    Case IBitacoras.ClaveTiposAplicacion.AplicacionMovilTracking

                        i_Cve_Aplicacion_ = 11

                    Case IBitacoras.ClaveTiposAplicacion.AplicacionWebExtranet

                        i_Cve_Aplicacion_ = 12

                    Case IBitacoras.ClaveTiposAplicacion.SinDefinir

                        i_Cve_Aplicacion_ = 4

                    Case Else

                        i_Cve_Aplicacion_ = value

                End Select

                _singletonBitacora = SingletonBitacoras.ObtenerInstancia(i_Cve_Aplicacion_)

                _activarBitacoraAvanzada = _singletonBitacora.ActivarBitacoraAvanzada

                _activarBitacoraSimple = _singletonBitacora.ActivarBitacoraSimple

                _i_Cve_Aplicacion = value

            End Set

        End Property

        Public Property TipoInstrumentacion As IBitacoras.TiposInstrumentacion Implements IConexiones.TipoInstrumentacion
            Get
                Return _i_TipoInstrumentacion
            End Get
            Set(value As IBitacoras.TiposInstrumentacion)
                _i_TipoInstrumentacion = value
            End Set
        End Property

        Public Property ModalidadConsulta As IOperacionesCatalogo.ModalidadesConsulta Implements IConexiones.ModalidadConsulta
            Get
                Return _i_ModalidadConsulta
            End Get
            Set(value As IOperacionesCatalogo.ModalidadesConsulta)
                _i_ModalidadConsulta = value
            End Set
        End Property

        Public Property IDRecursoSolicitante As Int32 Implements IConexiones.IDRecursoSolicitante
            Get
                Return _i_Cve_RecursoSolicitante
            End Get
            Set(value As Int32)
                _i_Cve_RecursoSolicitante = value
            End Set
        End Property

        Public Property RepositorioDatos As IConexiones.TiposRepositorio _
            Implements IConexiones.RepositorioDatos
            Get
                Return _repositorioDatos
            End Get
            Set(value As IConexiones.TiposRepositorio)
                _repositorioDatos = value
            End Set

        End Property

        Public ReadOnly Property GetTagWatcherObject As Wma.Exceptions.TagWatcher _
            Implements IConexiones.GetTagWatcherObject
            Get
                Return _tagWatcherObject
            End Get
        End Property

        Public Property NombreUsuarioCliente As String _
        Implements IConexiones.NombreUsuarioCliente

            Set(value As String)

                _nombreUsuarioCliente = value

            End Set

            Get
                Return _nombreUsuarioCliente

            End Get

        End Property

        Public Property ModuloCliente As String _
        Implements IConexiones.ModuloCliente

            Set(value As String)

                _moduloCliente = value

            End Set

            Get
                Return _moduloCliente

            End Get

        End Property

        Public ReadOnly Property EstadoConexion As String _
            Implements IConexiones.EstadoConexion
            Get
                Return _estadoconexion
            End Get
        End Property

        Public ReadOnly Property ConjuntoDataSet As System.Collections.Generic.Dictionary(Of Integer, System.Data.DataSet) _
            Implements IConexiones.ConjuntoDataSet
            Get
                Return _conjuntodataset
            End Get
        End Property

        Public ReadOnly Property DataSetReciente As System.Data.DataSet _
           Implements IConexiones.DataSetReciente
            Get
                Return _datasetreciente
            End Get
        End Property

        Public ReadOnly Property DataTableReciente As DataTable _
            Implements IConexiones.DataTableReciente
            Get
                Return _dataTableReciente
            End Get
        End Property

        Public Property Contrasena As String _
            Implements IConexiones.Contrasena
            Get
                Return _contrasena
            End Get
            Set(ByVal value As String)
                _contrasena = value
            End Set
        End Property

        Public Property ControladorBaseDatos As IConexiones.Controladores _
            Implements IConexiones.ControladorBaseDatos
            Get
                Return _controladorbasedatos
            End Get
            Set(ByVal value As IConexiones.Controladores)

                _controladorbasedatos = value

                Select Case _controladorbasedatos

                    Case IConexiones.Controladores.MySQL51
                        _conexion = New MySqlConnection
                        _datadapter = New MySqlDataAdapter
                        _command = New MySql.Data.MySqlClient.MySqlCommand

                        If _i_TiempoEspera = 0 Then : _i_TiempoEspera = 900 : End If

                        _command.CommandTimeout = _i_TiempoEspera

                    Case IConexiones.Controladores.SQLServer2008
                        _conexion = New SqlConnection
                        _datadapter = New SqlDataAdapter
                        _command = New SqlCommand


                        If _i_TiempoEspera = 0 Then : _i_TiempoEspera = 900 : End If

                        _command.CommandTimeout = _i_TiempoEspera


                    Case IConexiones.Controladores.VFP5
                        _conexion = New Odbc.OdbcConnection
                        _datadapter = New Odbc.OdbcDataAdapter
                        _command = New Odbc.OdbcCommand


                        If _i_TiempoEspera = 0 Then : _i_TiempoEspera = 900 : End If

                        _command.CommandTimeout = _i_TiempoEspera


                End Select

            End Set

        End Property

        Public Property IpServidor As String _
            Implements IConexiones.IpServidor
            Get
                Return _ipservidor
            End Get
            Set(ByVal value As String)
                _ipservidor = value
            End Set
        End Property

        Public Property NombreBaseDatos As String _
            Implements IConexiones.NombreBaseDatos
            Get
                Return _nombrebasedatos
            End Get
            Set(ByVal value As String)
                _nombrebasedatos = value
            End Set
        End Property

        Public Property ObjetoDatos As IConexiones.TipoConexion _
            Implements IConexiones.ObjetoDatos
            Get
                Return _objetodatos
            End Get
            Set(ByVal value As IConexiones.TipoConexion)
                _objetodatos = value
                _cadenaconexion = CadenaConexion()
            End Set
        End Property

        Public Property PuertoConexion As String _
            Implements IConexiones.PuertoConexion
            Get
                Return _puerto
            End Get
            Set(ByVal value As String)
                _puerto = value
            End Set
        End Property

        Public Property Usuario As String _
            Implements IConexiones.Usuario
            Get
                Return _usuario
            End Get
            Set(ByVal value As String)
                _usuario = value
            End Set
        End Property

        ' Propiedades de Conexiones
        Public ReadOnly Property CadenaConexion As String
            Get
                Select Case _objetodatos

                    Case IConexiones.TipoConexion.MySQLCommand

                        'MasterOfPuppets
                        If _i_TiempoEspera = 0 Then : _i_TiempoEspera = 5 : End If

                        _cadenaconexion = "Server=10.66.1.150;" &
                                       "Database=arkam3001;" &
                                       "UID=root;" &
                                       "Pwd=modelotx30;" &
                                       "Port=3306;" &
                                       "Connect Timeout=" & _i_TiempoEspera.ToString & ";"


                    Case IConexiones.TipoConexion.SqlCommand

                        If _i_TiempoEspera = 0 Then : _i_TiempoEspera = 2200 : End If

                        _cadenaconexion = " Server=" & _ipservidor & ";" &
                         " Database=" & _nombrebasedatos & ";" &
                         " Uid=" & _usuario & "; " &
                         " Pwd=" & _contrasena & ";Connection Timeout=" & _i_TiempoEspera.ToString & ";Connection Lifetime=0;"

                    Case IConexiones.Controladores.MySQL51

                        If _i_TiempoEspera = 0 Then : _i_TiempoEspera = 5 : End If

                        _cadenaconexion = "Server=" & _ipservidor & ";" &
                                          "Database=" & _nombrebasedatos & ";" &
                                          "UID=" & _usuario & ";" &
                                          "Pwd=" & _contrasena & ";" &
                                          "Port=" & _puerto & ";" &
                                          "Connect Timeout=" & _i_TiempoEspera.ToString & ";"

                    Case IConexiones.Controladores.SQLServer2008

                        If _i_TiempoEspera = 0 Then : _i_TiempoEspera = 2200 : End If

                        _cadenaconexion = " Server=" & _ipservidor & ";" &
                                     " Database=" & _nombrebasedatos & ";" &
                                     " Uid=" & _usuario & "; " &
                                     " Pwd=" & _contrasena & ";Connection Timeout=" & _i_TiempoEspera.ToString & ";Connection Lifetime=0;"

                    Case IConexiones.TipoConexion.DirectMongoDB

                        Dim _statements As Sax.SaxStatements = Sax.SaxStatements.GetInstance()

                        Dim ipservidor_ As String
                        Dim stringoptions_ As String
                        Dim usuario_ As String
                        Dim contrasena_ As String
                        Dim puerto_ As String

                        With _statements

                            If Not _saxappid Is Nothing Then

                                ipservidor_ = .GetEndPoint("project", .GetRol("project", "bigdataops", "nosql", "mongodb", _saxappid).endpointId, _saxappid).ip
                                stringoptions_ = .GetEndPoint("project", .GetRol("project", "bigdataops", "nosql", "mongodb", _saxappid).endpointId, _saxappid).stringoptions
                                puerto_ = .GetEndPoint("project", .GetRol("project", "bigdataops", "nosql", "mongodb", _saxappid).endpointId, _saxappid).port

                                usuario_ = .GetCredentials("project", .GetRol("project", "bigdataops", "nosql", "mongodb", _saxappid).credentialId, _saxappid).user
                                contrasena_ = .GetCredentials("project", .GetRol("project", "bigdataops", "nosql", "mongodb", _saxappid).credentialId, _saxappid).password
                            Else

                                ipservidor_ = .GetEndPoint("project", .GetRol("project", "bigdataops", "nosql", "mongodb").endpointId).ip
                                stringoptions_ = .GetEndPoint("project", .GetRol("project", "bigdataops", "nosql", "mongodb").endpointId).stringoptions
                                puerto_ = .GetEndPoint("project", .GetRol("project", "bigdataops", "nosql", "mongodb").endpointId).port

                                usuario_ = .GetCredentials("project", .GetRol("project", "bigdataops", "nosql", "mongodb").credentialId).user
                                contrasena_ = .GetCredentials("project", .GetRol("project", "bigdataops", "nosql", "mongodb").credentialId).password

                            End If


                        End With

                        _cadenaconexion = "mongodb://" & IIf(Not usuario_ Is Nothing, usuario_ & ":" & contrasena_ & "@", Nothing) &
                                                        Trim(ipservidor_) &
                                                        IIf(Not puerto_ Is Nothing, ":" & puerto_, Nothing) &
                                                        IIf(Not stringoptions_ Is Nothing, "/" & stringoptions_, Nothing)

                        '    _cadenaconexion = "mongodb://" & Trim(ipservidor_) & ":" & puerto_


                    Case IConexiones.Controladores.VFP5

                        _cadenaconexion = "Driver={Microsoft Visual FoxPro Driver};SourceType=DBF;SourceDB=" & System.IO.Path.GetDirectoryName(_ipservidor) & ";"

                End Select

                Return _cadenaconexion
            End Get
        End Property

#End Region

#Region "Métodos"

        Public Function EjecutaConsultaDirectaEstandarizadaDataTable(
                        ByVal datadapter_ As IDbDataAdapter,
                        ByVal command_ As IDbCommand,
                        ByVal _sentencia As String) As TagWatcher _
                        Implements IConexiones.EjecutaConsultaDirectaEstandarizadaDataTable

            Dim ejecucion_ As Boolean = False

            Dim dataTable_ As New DataTable

            Dim tagWatcher_ As New TagWatcher

            Try

                tagWatcher_.SetError(TagWatcher.ErrorTypes.C2_000_2000, "Inicio de consulta")

                command_.CommandText = _sentencia

                datadapter_.SelectCommand = command_

                Select Case _controladorbasedatos

                    Case IConexiones.Controladores.MySQL51

                        DirectCast(datadapter_, MySqlDataAdapter).Fill(dataTable_)

                    Case IConexiones.Controladores.SQLServer2008

                        DirectCast(datadapter_, SqlDataAdapter).Fill(dataTable_)

                    Case IConexiones.Controladores.VFP5

                        DirectCast(datadapter_, Odbc.OdbcDataAdapter).Fill(dataTable_)

                End Select

                ejecucion_ = True

                tagWatcher_.ObjectReturned = dataTable_

                tagWatcher_.SetOK()

            Catch ex2 As System.Data.SqlClient.SqlException

                ejecucion_ = False

                tagWatcher_.SetError(TagWatcher.ErrorTypes.C2_000_2000, ex2.Message)

                LogsInspector("EjecutaConsultaDirectaEstandarizadaDataTable", "SqlException", ex2.Message, _sentencia)

            End Try

            Return tagWatcher_

        End Function

        'Mecanismos libres de bitácota.
        Public Function EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(ByVal _sentencia As String) As TagWatcher _
            Implements IConexiones.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable

            Dim tagWatcher_ As New TagWatcher

            Dim conexion_ As IDbConnection = New SqlConnection

            Dim datadapter_ As IDbDataAdapter = New SqlDataAdapter

            Dim command_ As IDbCommand = New SqlCommand

            'Abrir conexión
            Try

                Select Case _controladorbasedatos

                    Case IConexiones.Controladores.MySQL51

                        conexion_ = New MySqlConnection

                        datadapter_ = New MySqlDataAdapter

                        command_ = New MySql.Data.MySqlClient.MySqlCommand

                        ObjetoDatos = IConexiones.TipoConexion.MySQLCommand

                    Case IConexiones.Controladores.SQLServer2008

                        conexion_ = New SqlConnection

                        datadapter_ = New SqlDataAdapter

                        command_ = New SqlCommand

                        ObjetoDatos = IConexiones.TipoConexion.SqlCommand

                    Case IConexiones.Controladores.VFP5

                        conexion_ = New Odbc.OdbcConnection

                        datadapter_ = New Odbc.OdbcDataAdapter

                        command_ = New Odbc.OdbcCommand

                End Select

                command_.CommandTimeout = 900

                command_.Connection = conexion_

                conexion_.ConnectionString = _cadenaconexion

                conexion_.Open()

            Catch ex As Exception

                LogsInspector("EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable", "Exception", ex.Message, _sentencia)

            End Try

            tagWatcher_ = EjecutaConsultaDirectaEstandarizadaDataTable(datadapter_, command_, _sentencia)

            'Cerrar conexión

            Try

                command_.Dispose()

                conexion_.Close()

                conexion_.Dispose()

            Catch ex As Exception

            End Try

            Return tagWatcher_

        End Function

        'Sin bitacora
        Public Function EjecutaConsultaDirectaEstandarizada(ByVal _sentencia As String) As TagWatcher _
                        Implements IConexiones.EjecutaConsultaDirectaEstandarizada

            Dim ejecucion_ As Boolean = False

            Dim dataSetAuxiliar_ As New DataSet

            Try

                _command.CommandText = _sentencia

                _datadapter.SelectCommand = _command

                _datadapter.Fill(dataSetAuxiliar_)

                ejecucion_ = True

                _tagWatcherObject.ObjectReturned = dataSetAuxiliar_

                _tagWatcherObject.SetOK()

            Catch ex2 As System.Data.SqlClient.SqlException

                ejecucion_ = False

                _tagWatcherObject.SetError(TagWatcher.ErrorTypes.C2_000_2000, ex2.Message)

                LogsInspector("EjecutaConsultaDirectaEstandarizada", "SqlException", ex2.Message, _sentencia)

            End Try

            Return _tagWatcherObject

        End Function

        'Sin bitacora
        Private Function ObtenerMilisegundos(ByVal fecha_ As DateTime) As Long

            Dim respuesta_ As Long = 0

            Dim fechaAuxiliar_ As Date = New Date(1970, 1, 1, 0, 0, 0, 0)

            respuesta_ = DateDiff(DateInterval.Second, fechaAuxiliar_, fecha_) * 1000 + fecha_.Millisecond

            Return respuesta_

        End Function

        'Sin bitacora
        Public Function EjecutaConsultaDirectaEstandarizadaOpenClose(ByVal _sentencia As String) As TagWatcher _
            Implements IConexiones.EjecutaConsultaDirectaEstandarizadaOpenClose

            IniciaConexion()

            EjecutaConsultaDirectaEstandarizada(_sentencia)

            TerminaConexion()

            Return _tagWatcherObject

        End Function

        'Sin bitacora
        Public Function TerminaConexion() As Boolean _
            Implements IConexiones.TerminaConexion

            Try

                _command.Dispose()

                _conexion.Close()

                _conexion.Dispose()

                _estadoconexion = False

            Catch ex As Exception

                ' ibitacoras_.DocumentaError(ibitacoras_.NombreArchivo, 0, ex)
                LogsInspector("TerminaConexion", "Exception", ex.Message, Nothing)

            End Try

            Return _estadoconexion

        End Function

        'Usa bitácora
        Public Function EjecutaConsultaIndividual(ByVal _sentencia As String) As Boolean _
            Implements IConexiones.EjecutaConsultaIndividual

            Dim ejecucion_ As Boolean = False

            '::::::::::::::::: I N I C I O ::: B I T Á C O R A ::::::::::::::::::::::::::::::::::::::::

            If ActivarBitacoraAvanzada Then

                Dim idTransaccion_ As String = Nothing

                Dim tiempoInicialL_ As Long = Nothing

                Dim tiempoFinalL_ As Long = Nothing

                Dim tiempoTranscurridoL_ As Long = 0

                Dim f_FechaHoraInicial_ As Date = DateTime.Now

                tiempoInicialL_ = ObtenerMilisegundos(f_FechaHoraInicial_)

                _bitacoraAvanzada = New BitacoraCapaDatos(Nothing,
                    Nothing,
                    118,
                    Nothing,
                    Nothing,
                    IBitacoras.TiposBitacora.Informacion,
                     IBitacoras.TiposSucesos.Consultar,
                    _i_Cve_Usuario,
                    _i_Cve_DivisionMiEmpresa,
                    0,
                    f_FechaHoraInicial_,
                    _i_Cve_Aplicacion,
                    _i_TipoInstrumentacion,
                    _i_ModalidadConsulta,
                    _i_Cve_RecursoSolicitante
                   )

                _bitacoraAvanzada.NombreEnsamblado = _t_NombreEnsamblado
                _bitacoraAvanzada.NombreVT = _t_NombreVT
                _bitacoraAvanzada.NombreCompletoUsuario = _t_NombreUsuario
                _bitacoraAvanzada.CuentaPublicaUsuario = _t_CuentaUsuario


                _bitacoraAvanzada.DocumentaBitacoraCapaDatos(_sentencia,
                                                             IBitacoras.EstadosConsultaAvanzada.EnProceso,
                                                             f_FechaHoraInicial_,
                                                             0,
                                                             Nothing)

                idTransaccion_ = _bitacoraAvanzada.IDTransaccionAbierta

                ':::::::::::::::::::::::::::::::::::::::::::::::::::::::::

                IniciaConexion()

                ejecucion_ = EjecutaConsultaEspecial(_sentencia)

                TerminaConexion()

                ':::::::::::::::::::::::::::::::::::::::::::::::::::::::::


                _bitacoraAvanzada.IDTransaccionAbierta = idTransaccion_

                Dim f_FechaHorafinal_ As Date = DateTime.Now

                tiempoFinalL_ = ObtenerMilisegundos(f_FechaHorafinal_)

                tiempoTranscurridoL_ = tiempoFinalL_ - tiempoInicialL_

                _bitacoraAvanzada.DocumentaBitacoraCapaDatos(Nothing,
                                                             IBitacoras.EstadosConsultaAvanzada.Concluida,
                                                             f_FechaHorafinal_,
                                                             tiempoTranscurridoL_,
                                                             _tagWatcherObject)

            Else

                ':::::::::::::::::::::::::::::::::::::::::::::::::::::::::

                IniciaConexion()

                ejecucion_ = EjecutaConsultaEspecial(_sentencia)

                TerminaConexion()

                ':::::::::::::::::::::::::::::::::::::::::::::::::::::::::

            End If

            '::::::::::::::::: C I E R R E ::: B I T Á C O R A ::::::::::::::::::::::::::::::::::::::::

            Return ejecucion_

        End Function

        'Usa bitácora
        Public Function EjecutaConsultaSinRespuesta(ByVal _sentencia As String) As Boolean _
            Implements IConexiones.EjecutaConsultaSinRespuesta

            Dim ejecucion_ As Boolean = False

            'Dim dialogo_ As New gsol.Componentes.SistemaBase.GsDialogo

            Try

                If ActivarBitacoraAvanzada Then


                    '::::::::::::::::: I N I C I O ::: B I T Á C O R A ::::::::::::::::::::::::::::::::::::::::

                    Dim idTransaccion_ As String = Nothing

                    Dim tiempoInicialL_ As Long = Nothing

                    Dim tiempoFinalL_ As Long = Nothing

                    Dim tiempoTranscurridoL_ As Long = 0

                    Dim f_FechaHoraInicial_ As Date = DateTime.Now

                    tiempoInicialL_ = ObtenerMilisegundos(f_FechaHoraInicial_)

                    _bitacoraAvanzada = New BitacoraCapaDatos(Nothing,
                        Nothing,
                        118,
                        Nothing,
                        Nothing,
                        IBitacoras.TiposBitacora.Informacion,
                         IBitacoras.TiposSucesos.Consultar,
                        _i_Cve_Usuario,
                        _i_Cve_DivisionMiEmpresa,
                        0,
                        f_FechaHoraInicial_,
                        _i_Cve_Aplicacion,
                        _i_TipoInstrumentacion,
                        _i_ModalidadConsulta,
                        _i_Cve_RecursoSolicitante
                       )

                    _bitacoraAvanzada.NombreEnsamblado = _t_NombreEnsamblado
                    _bitacoraAvanzada.NombreVT = _t_NombreVT
                    _bitacoraAvanzada.NombreCompletoUsuario = _t_NombreUsuario
                    _bitacoraAvanzada.CuentaPublicaUsuario = _t_CuentaUsuario


                    _bitacoraAvanzada.DocumentaBitacoraCapaDatos(_sentencia,
                                                                 IBitacoras.EstadosConsultaAvanzada.EnProceso,
                                                                 f_FechaHoraInicial_,
                                                                 0,
                                                                 Nothing)

                    idTransaccion_ = _bitacoraAvanzada.IDTransaccionAbierta

                    ':::::::::::::::::::::::::::::::::::::::::::::::::::::::::


                    ejecucion_ = False

                    _command.CommandText = _sentencia

                    _datadapter.SelectCommand = _command

                    ejecucion_ = True


                    ':::::::::::::::::::::::::::::::::::::::::::::::::::::::::

                    _bitacoraAvanzada.IDTransaccionAbierta = idTransaccion_

                    Dim f_FechaHorafinal_ As Date = DateTime.Now

                    tiempoFinalL_ = ObtenerMilisegundos(f_FechaHorafinal_)

                    tiempoTranscurridoL_ = tiempoFinalL_ - tiempoInicialL_

                    _bitacoraAvanzada.DocumentaBitacoraCapaDatos(Nothing,
                                                                 IBitacoras.EstadosConsultaAvanzada.Concluida,
                                                                 f_FechaHorafinal_,
                                                                 tiempoTranscurridoL_,
                                                                 _tagWatcherObject)


                    '::::::::::::::::: C I E R R E ::: B I T Á C O R A ::::::::::::::::::::::::::::::::::::::::

                Else

                    ':::::::::::::::::::::::::::::::::::::::::::::::::::::::::


                    ejecucion_ = False

                    _command.CommandText = _sentencia

                    _datadapter.SelectCommand = _command

                    ejecucion_ = True


                    ':::::::::::::::::::::::::::::::::::::::::::::::::::::::::


                End If


            Catch ex1 As System.InvalidOperationException

                ejecucion_ = False

                LogsInspector("EjecutaConsultaSinRespuesta", "InvalidOperationException", ex1.Message, _sentencia)

                If ActivarBitacoraSimple Then

                    _bitacoras = New BitacoraCapaDatos("SQLCommand Was{EjecutaConsultaSinRespuesta}[System.InvalidOperationException]: " & vbNewLine &
                                              " ID Error: " & Err.Number.ToString & vbNewLine &
                                              "   " & _sentencia & ", " & vbNewLine &
                                             vbNewLine &
                                              "Mensaje excepción:" &
                                              "   " & ex1.Message, IBitacoras.TiposBitacora.Errores, IBitacoras.TiposSucesos.Consultar, _nombreUsuarioCliente, _moduloCliente)
                End If




            Catch ex2 As System.Data.SqlClient.SqlException

                ejecucion_ = False

                _tagWatcherObject.SetError(TagWatcher.ErrorTypes.C2_000_2000, ex2.Message)

                LogsInspector("EjecutaConsultaSinRespuesta", "SqlException", ex2.Message, _sentencia)

                Select Case Err.Number

                    Case 5

                        'dialogo_.Invocar(
                        '       "Atención usuario Krom." & vbNewLine &
                        '       "Código de protección SQL Server: ID: " & Err.Number.ToString & vbNewLine &
                        '       "Posiblemente las tablas/vistas indicadas en esta consulta no existan" & vbNewLine &
                        '       "Consulta: {" & _sentencia & "}" & vbNewLine &
                        '       "Mensaje:" & ex2.Message, Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)

                    Case 2627

                        'dialogo_.Invocar("Atención usuario Krom." & vbNewLine & _
                        '       "- Código de protección SQL Server: ID: " & Err.Number.ToString & vbNewLine & _
                        '       "- El registro que está intentando guardar ya existe, esta acción no será permitirá para evitar duplicados" & vbNewLine & _
                        '       "- Mensaje:" & ex2.Message, Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)


                    Case Else

                        If ActivarBitacoraSimple Then

                            _bitacoras = New BitacoraCapaDatos("SQLCommand Was{EjecutaConsulta*}[System.Data.SqlClient.SqlException]: " & vbNewLine &
                                                                               " ..ID Error: " & Err.Number.ToString & vbNewLine &
                                                                               "   " & _sentencia & ", " & vbNewLine &
                                                                              vbNewLine &
                                                                               "Mensaje excepción:" &
                                                                               "   " & ex2.Message, IBitacoras.TiposBitacora.Errores, IBitacoras.TiposSucesos.Consultar, _nombreUsuarioCliente, _moduloCliente)

                        End If


                End Select

            Catch ex As Exception

                ejecucion_ = False

                LogsInspector("EjecutaConsultaSinRespuesta", "Exception", ex.Message, _sentencia)

                If ActivarBitacoraSimple Then

                    _bitacoras = New BitacoraCapaDatos("SQLCommand Was{EjecutaConsulta*}: " & vbNewLine &
                                                                    " ..ID Error: " & Err.Number.ToString & vbNewLine &
                                                                    "   " & _sentencia & ", " & vbNewLine &
                                                                   vbNewLine &
                                                                    "Mensaje excepción:" &
                                                                    "   " & ex.Message, IBitacoras.TiposBitacora.Errores, IBitacoras.TiposSucesos.Consultar, _nombreUsuarioCliente, _moduloCliente)

                End If

            End Try

            Return ejecucion_

        End Function

        'Usa bitácora ambas
        Public Function EjecutaConsulta(ByVal _sentencia As String) As Boolean _
            Implements IConexiones.EjecutaConsulta

            Dim ejecucion_ As Boolean = False


            Try

                If ActivarBitacoraAvanzada Then

                    '::::::::::::::::: I N I C I O ::: B I T Á C O R A ::::::::::::::::::::::::::::::::::::::::

                    Dim idTransaccion_ As String = Nothing

                    Dim tiempoInicialL_ As Long = Nothing

                    Dim tiempoFinalL_ As Long = Nothing

                    Dim tiempoTranscurridoL_ As Long = 0

                    Dim f_FechaHoraInicial_ As Date = DateTime.Now

                    tiempoInicialL_ = ObtenerMilisegundos(f_FechaHoraInicial_)

                    _bitacoraAvanzada = New BitacoraCapaDatos(Nothing,
                        Nothing,
                        118,
                        Nothing,
                        Nothing,
                        IBitacoras.TiposBitacora.Informacion,
                         IBitacoras.TiposSucesos.Consultar,
                        _i_Cve_Usuario,
                        _i_Cve_DivisionMiEmpresa,
                        0,
                        f_FechaHoraInicial_,
                        _i_Cve_Aplicacion,
                        _i_TipoInstrumentacion,
                        _i_ModalidadConsulta,
                        _i_Cve_RecursoSolicitante
                       )

                    _bitacoraAvanzada.NombreEnsamblado = _t_NombreEnsamblado
                    _bitacoraAvanzada.NombreVT = _t_NombreVT
                    _bitacoraAvanzada.NombreCompletoUsuario = _t_NombreUsuario
                    _bitacoraAvanzada.CuentaPublicaUsuario = _t_CuentaUsuario

                    _bitacoraAvanzada.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.Singleton

                    _bitacoraAvanzada.DocumentaBitacoraCapaDatos(_sentencia,
                                                                 IBitacoras.EstadosConsultaAvanzada.EnProceso,
                                                                 f_FechaHoraInicial_,
                                                                 0,
                                                                 Nothing)

                    idTransaccion_ = _bitacoraAvanzada.IDTransaccionAbierta

                    ':::::::::::::::::::::::::::::::::::::::::::::::::::::::::

                    _command.CommandText = _sentencia

                    _datadapter.SelectCommand = _command

                    _datadapter.Fill(_datasetreciente)

                    ejecucion_ = True

                    ':::::::::::::::::::::::::::::::::::::::::::::::::::::::::

                    _bitacoraAvanzada.IDTransaccionAbierta = idTransaccion_

                    Dim f_FechaHorafinal_ As Date = DateTime.Now

                    tiempoFinalL_ = ObtenerMilisegundos(f_FechaHorafinal_)

                    tiempoTranscurridoL_ = tiempoFinalL_ - tiempoInicialL_

                    _bitacoraAvanzada.DocumentaBitacoraCapaDatos(Nothing,
                                                                 IBitacoras.EstadosConsultaAvanzada.Concluida,
                                                                 f_FechaHorafinal_,
                                                                 tiempoTranscurridoL_,
                                                                 _tagWatcherObject)

                    '::::::::::::::::: C I E R R E ::: B I T Á C O R A ::::::::::::::::::::::::::::::::::::::::

                Else

                    ':::::::::::::::::::::::::::::::::::::::::::::::::::::::::

                    _command.CommandText = _sentencia

                    _datadapter.SelectCommand = _command

                    _datadapter.Fill(_datasetreciente)

                    ejecucion_ = True



                    ':::::::::::::::::::::::::::::::::::::::::::::::::::::::::

                End If


            Catch ex1 As System.InvalidOperationException

                'ejecucion_ = False

                LogsInspector("EjecutaConsulta", "InvalidOperationException", ex1.Message, _sentencia)

                If ActivarBitacoraSimple Then

                    _bitacoras = New BitacoraCapaDatos("SQLCommand Was{EjecutaConsulta*}[System.InvalidOperationException]: " & vbNewLine &
                               " ID Error: " & Err.Number.ToString & vbNewLine &
                               "   " & _sentencia & ", " & vbNewLine &
                            vbNewLine &
                               "Mensaje excepción:" &
                               "   " & ex1.Message, IBitacoras.TiposBitacora.Errores, IBitacoras.TiposSucesos.Consultar, _nombreUsuarioCliente, _moduloCliente)

                End If


            Catch ex2 As System.Data.SqlClient.SqlException

                'Dim dialogo_ As New gsol.Componentes.SistemaBase.GsDialogo

                ' ejecucion_ = False

                _tagWatcherObject.SetError(TagWatcher.ErrorTypes.C2_000_2000, ex2.Message)

                LogsInspector("EjecutaConsulta", "SqlException", ex2.Message, _sentencia)

                Select Case Err.Number

                    Case 5 'System.Data.SqlClient.SqlErrorCollection

                        'dialogo_.Invocar("Operación no permitida" & vbNewLine &
                        '                " Está intentando duplicar un registro." & Err.Number.ToString & vbNewLine &
                        '                " Mensaje: " & ex2.Message & vbNewLine, Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)
                        ''"Consulta: {" & _sentencia & "}" & vbNewLine & _
                        ''Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)

                    Case 2627

                        'dialogo_.Invocar("Atención usuario Krom." & vbNewLine & _
                        '       "- Código de protección SQL Server: ID: " & Err.Number.ToString & vbNewLine & _
                        '       "- El registro que está intentando guardar ya existe, esta acción no será permitirá para evitar duplicados" & vbNewLine & _
                        '       "- Mensaje:" & ex2.Message, Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)


                    Case Else

                        'dialogo_.Invocar("Atención usuario Krom." & vbNewLine & _
                        '     "- Código de protección SQL Server: ID: " & Err.Number.ToString & vbNewLine & _
                        '     "- El registro que está intentando guardar ya existe, esta acción no será permitirá para evitar duplicados" & vbNewLine & _
                        '     "- Mensaje:" & ex2.Message, Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)

                End Select

                If ActivarBitacoraSimple Then

                    _bitacoras = New BitacoraCapaDatos("SQLCommand Was{EjecutaConsulta*}[System.Data.SqlClient.SqlException]: " & vbNewLine &
                                                                    " ..ID Error: " & Err.Number.ToString & vbNewLine &
                                                                    "   " & _sentencia & ", " & vbNewLine &
                                                                   vbNewLine &
                                                                    "Mensaje excepción:" &
                                                                    "   " & ex2.Message, IBitacoras.TiposBitacora.Errores, IBitacoras.TiposSucesos.Consultar, _nombreUsuarioCliente, _moduloCliente)


                End If


            Catch ex As Exception


                LogsInspector("EjecutaConsulta", "Exception", ex.Message, _sentencia)

                ' ejecucion_ = False

                If ActivarBitacoraSimple Then

                    _bitacoras = New BitacoraCapaDatos("SQLCommand Was{EjecutaConsulta*}: " & vbNewLine &
                                                                       " ..ID Error: " & Err.Number.ToString & vbNewLine &
                                                                       "   " & _sentencia & ", " & vbNewLine &
                                                                      vbNewLine &
                                                                       "Mensaje excepción:" &
                                                                       "   " & ex.Message, IBitacoras.TiposBitacora.Errores, IBitacoras.TiposSucesos.Consultar, _nombreUsuarioCliente, _moduloCliente)


                End If

            End Try

            Return ejecucion_

        End Function

        'Usa bitácora antigua
        Public Overridable Function EjecutaSentencia(ByVal _sentencia As String) As Boolean _
            Implements IConexiones.EjecutaSentencia

            Dim ejecucion_ As Boolean = False

            Try
                _command.CommandText = _sentencia

                _command.ExecuteNonQuery()
                ejecucion_ = True

            Catch ex As Exception

                LogsInspector("EjecutaSentencia", "Exception", ex.Message, _sentencia)

                ejecucion_ = False

                If ActivarBitacoraSimple Then

                    _bitacoras = New BitacoraCapaDatos("SQLCommand Was{EjecutaSentencia}:" & Chr(13) &
                                                       "   " & _sentencia & ", " & Chr(13) &
                                                       Chr(13) &
                                                       "Mensaje excepción:" &
                                                       "   " & ex.Message, IBitacoras.TiposBitacora.Errores, IBitacoras.TiposSucesos.Consultar, _nombreUsuarioCliente, _moduloCliente)

                End If


            End Try

            Return ejecucion_

        End Function

        'Usa bitácora antigua
        Public Function EjecutaSentenciaIndividual(ByVal _sentencia As String) As Boolean _
            Implements IConexiones.EjecutaSentenciaIndividual

            Dim ejecucion_ As Boolean = False

            IniciaConexion()

            Try
                _command.CommandText = _sentencia
                _command.ExecuteNonQuery()
                ejecucion_ = True

            Catch ex As Exception


                LogsInspector("EjecutaSentenciaIndividual", "Exception", ex.Message, _sentencia)

                ejecucion_ = False

                If ActivarBitacoraSimple Then

                    _bitacoras = New BitacoraCapaDatos("SQLCommand Was{EjecutaSentenciaIndividual}:" & Chr(13) &
                                     "   " & _sentencia & ", " & Chr(13) &
                                     Chr(13) &
                                     "Mensaje excepción:" &
                                     "   " & ex.Message, IBitacoras.TiposBitacora.Errores, IBitacoras.TiposSucesos.Consultar, _nombreUsuarioCliente, _moduloCliente)

                End If


            Finally

                TerminaConexion()

            End Try

            Return ejecucion_

        End Function

        'Usa bitácora antigua
        Public Function IniciaConexion() As Boolean _
            Implements IConexiones.IniciaConexion

            Try

                If _conexion.State = ConnectionState.Closed Then

                    _command.Connection = _conexion

                    _conexion.ConnectionString = _cadenaconexion

                    _conexion.Open()

                    _estadoconexion = True


                End If

            Catch ex As Exception

                LogsInspector("IniciaConexion", "Exception", ex.Message, Nothing)

                _estadoconexion = False

                If ActivarBitacoraSimple Then

                    If _nombreUsuarioCliente Is Nothing Then : _nombreUsuarioCliente = "IConexiones" : End If

                    _moduloCliente = "Conexiones64.dll"

                    _bitacoras = New BitacoraCapaDatos("SQLCommand Was{Inicio Conexión}:" & Chr(13) &
                       Chr(13) &
                       "Mensaje excepción:" &
                       "   " & ex.Message, IBitacoras.TiposBitacora.Errores, IBitacoras.TiposSucesos.Consultar, _nombreUsuarioCliente, _moduloCliente)

                End If

            End Try

            Return _estadoconexion

        End Function

        'Usa bitácora antigua
        Private Function EjecutaConsultaEspecial(ByVal _sentencia As String) As Boolean _
            ' Implements IConexiones.EjecutaConsulta

            Dim ejecucion_ As Boolean = False

            'Dim dialogo_ As New gsol.Componentes.SistemaBase.GsDialogo

            Try

                '::::::::::::::::: I N I C I O ::: B I T Á C O R A ::::::::::::::::::::::::::::::::::::::::

                ':::::::::::::::::::::::::::::::::::::::::::::::::::::::::

                _command.CommandText = _sentencia

                _datadapter.SelectCommand = _command

                _datadapter.Fill(_datasetreciente)

                ejecucion_ = True

                ':::::::::::::::::::::::::::::::::::::::::::::::::::::::::

                '::::::::::::::::: C I E R R E ::: B I T Á C O R A ::::::::::::::::::::::::::::::::::::::::

            Catch ex1 As System.InvalidOperationException

                ejecucion_ = False

                LogsInspector("EjecutaConsultaEspecial", "InvalidOperationException", ex1.Message, _sentencia)

                If ActivarBitacoraSimple Then

                    _bitacoras = New BitacoraCapaDatos("SQLCommand Was{EjecutaConsulta*}[System.InvalidOperationException]: " & vbNewLine &
                                              " ID Error: " & Err.Number.ToString & vbNewLine &
                                              "   " & _sentencia & ", " & vbNewLine &
                                           vbNewLine &
                                              "Mensaje excepción:" &
                                              "   " & ex1.Message, IBitacoras.TiposBitacora.Errores, IBitacoras.TiposSucesos.Consultar, _nombreUsuarioCliente, _moduloCliente)

                End If

            Catch ex2 As System.Data.SqlClient.SqlException

                ejecucion_ = False

                LogsInspector("EjecutaConsultaEspecial", "SqlException", ex2.Message, _sentencia)

                _tagWatcherObject.SetError(TagWatcher.ErrorTypes.C2_000_2000, ex2.Message)

                Select Case Err.Number

                    Case 5 'System.Data.SqlClient.SqlErrorCollection

                        'dialogo_.Invocar("Operación no permitida" & vbNewLine & _
                        '                " Está intentando duplicar un registro." & Err.Number.ToString & vbNewLine & _
                        '                " Mensaje: " & ex2.Message & vbNewLine, Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)

                    Case 2627

                        'dialogo_.Invocar("Atención usuario Krom." & vbNewLine & _
                        '       "- Código de protección SQL Server: ID: " & Err.Number.ToString & vbNewLine & _
                        '       "- El registro que está intentando guardar ya existe, esta acción no será permitirá para evitar duplicados" & vbNewLine & _
                        '       "- Mensaje:" & ex2.Message, Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)


                    Case Else

                        'dialogo_.Invocar("Atención usuario Krom." & vbNewLine & _
                        '     "- Código de protección SQL Server: ID: " & Err.Number.ToString & vbNewLine & _
                        '     "- El registro que está intentando guardar ya existe, esta acción no será permitirá para evitar duplicados" & vbNewLine & _
                        '     "- Mensaje:" & ex2.Message, Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)

                End Select

                If ActivarBitacoraSimple Then

                    _bitacoras = New BitacoraCapaDatos("SQLCommand Was{EjecutaConsulta*}[System.Data.SqlClient.SqlException]: " & vbNewLine &
                                                                     " ..ID Error: " & Err.Number.ToString & vbNewLine &
                                                                     "   " & _sentencia & ", " & vbNewLine &
                                                                    vbNewLine &
                                                                     "Mensaje excepción:" &
                                                                     "   " & ex2.Message, IBitacoras.TiposBitacora.Errores, IBitacoras.TiposSucesos.Consultar, _nombreUsuarioCliente, _moduloCliente)

                End If


            Catch ex As Exception

                ejecucion_ = False

                LogsInspector("EjecutaConsultaEspecial", "Exception", ex.Message, _sentencia)

                If ActivarBitacoraSimple Then

                    _bitacoras = New BitacoraCapaDatos("SQLCommand Was{EjecutaConsulta*}: " & vbNewLine &
                                                                      " ..ID Error: " & Err.Number.ToString & vbNewLine &
                                                                      "   " & _sentencia & ", " & vbNewLine &
                                                                     vbNewLine &
                                                                      "Mensaje excepción:" &
                                                                      "   " & ex.Message, IBitacoras.TiposBitacora.Errores, IBitacoras.TiposSucesos.Consultar, _nombreUsuarioCliente, _moduloCliente)

                End If

            End Try

            Return ejecucion_

        End Function

        Public Function ActivarBitacora() As Boolean

            Dim ev_ As New EnvironmentViews()

            Dim messaje_ As New TagWatcher

            If messaje_.Status = TagWatcher.TypeStatus.Ok Then

                Dim datasetEstatico_ As New DataSet

                datasetEstatico_.Tables.Add(messaje_.ObjectReturned)

                Return True

            Else

                Return False

            End If

        End Function


        Public Sub LogsInspector(ByVal from As String, ByVal level As String, ByVal message As String, sentence As String)

            Dim _organismo = New Organismo()
            _organismo.GeneraLog("\\10.66.2.117\c$\temp\logPruebaCOnexiones.txt", from & " (" & level & ") " & sentence & vbNewLine & vbCrLf)
            _organismo.GeneraLog("\\10.66.2.117\c$\temp\logPruebaCOnexiones.txt", from & " (" & level & ") " & message & vbNewLine & vbCrLf)

        End Sub

#End Region

    End Class

End Namespace
