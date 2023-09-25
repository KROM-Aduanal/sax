Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.Text
Imports Gsol.basededatos
Imports Gsol.BaseDatos.Operaciones
Imports System.IO
Imports System.Drawing
Imports Gsol.seguridad
Imports Gsol.krom
Imports Gsol.monitoreo
Imports Wma.Operations
'Imports Wma.Exceptions

'Imports MongoDB.Bson
'Imports MongoDB.Driverf
Imports MongoDB.Driver.Core
Imports MongoDB.Driver.Builders
Imports MongoDB.Driver.Linq

Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Driver
Imports MongoDB.Driver.MongoDBRefSerializer
Imports Wma.Components
Imports MongoDB.Bson.Serialization
Imports System.Threading.Tasks
Imports System.Globalization
Imports Wma.Exceptions
Imports Sax


'Imports MongoDB.Bson.Serialization
'Imports MongoDB.Driver.Builders
'Imports MongoDB.Driver.GridFS
'Imports MongoDB.Driver.Linq


Namespace Gsol.BaseDatos.Operaciones

    Public Class LineaBaseCatalogos
        Inherits Organismo

#Region "Atributos"

        Private Enum TipoOperacion
            SinDefinir = -1
            Consulta = 0
            Insercion = 1
            Modificacion = 2
            Borrado = 3
            InsercionDinamica = 4
            InsercionDinamicaExposicion = 5
        End Enum

        Structure LlaveOrdenValor
            Public _llaveInt32 As Int32
            Public _llaveString As String
            Public _indiceSuperiorString As String
            Public _indiceSuperiorInt32 As Int32
            Public _activa As Boolean
        End Structure

        Private _nombreToken As String

        Private _estructuraPresentacion As IEntidadDatos

        Private _visualizacionCamposConfigurada As IOperacionesCatalogo.TiposVisualizacionCampos

        Private _tablaedicion As String

        Private _identificadorcatalogo As String

        Private _operadorcatalogoconsultas As String

        Private _operadorcatalogoinsercion As String

        Private _operadorcatalogoborrado As String

        Private _operadorcatalogomodificacion As String

        Private _vistaencabezados As String

        Private _icaracteristicas As Dictionary(Of Int32, ICaracteristica)

        Private _cantidadvisibleregistros As Int32

        Private _cadenaencabezados As String

        Private _valorindiceseleccionado As String

        Private _valoresIndiceInsertados As List(Of Int64)

        Private _envolturacampollave As ICaracteristica.TiposCaracteristica

        Private _iespaciotrabajo As IEspacioTrabajo

        Private _sistema As Organismo

        Private _clausulaslibres As String

        Private _tipoescritura As IOperacionesCatalogo.TiposEscritura

        Private _registrostemporales As Dictionary(Of Int32, String)

        Private _registrostemporalespresentacion As Dictionary(Of Int32, String)

        Private _registrostemporalesDataTable As DataTable

        Private _identificadorempresa As String

        Private _entradallaves As String

        Private _salidallaves As String

        Private _filtraduplicados As Boolean

        Private _clausulasAutoFiltro As String

        Private _ordernarResultadosPorColumna As String

        Private _coleccionInstrucciones As Dictionary(Of String, IOperacionesCatalogo)

        Private _sqlTransaccionList As Dictionary(Of String, String)

        Private _indiceTablaTemporal As Int32

        Private _identificadorObjetoTransaccional As String

        Private _identificadorNivelTransaccional As String

        Private _identificadorCompletoLlamante As String

        Private _indiceTablaTemporalLlamante As String

        Private _tipoOperacionSQL As IOperacionesCatalogo.TiposOperacionSQL

        Private _complejoTransaccional As IOperacionesCatalogo.ComplexTypes

        Private _listaSQLNivel1 As Dictionary(Of LlaveOrdenValor, String)

        Private _listaSQLNivel2 As Dictionary(Of LlaveOrdenValor, String)

        Private _listaSQLNivel3 As Dictionary(Of LlaveOrdenValor, String)



        Public _llavesDeclaradas As List(Of String)

        Public _declaracionesAdicionalesUsuario As String

        Public _instruccionesTransaccionCometida As String = Nothing

        Public _ejecutarPlanEjecucion As Boolean

        Public _planEjecucionSQL As String

        Public _instruccionesSQLAntesIniciarTransaccion As String

        Public _declaracionesAdicionalesPieTransaccion As String

        Public _agrupacionesLibres As String


        'Bitacora especial

        Private _i_Cve_Usuario As Int32
        Private _i_Cve_DivisionMiEmpresa As Int32
        Private _i_Cve_Aplicacion As IBitacoras.ClaveTiposAplicacion

        Private _i_TipoInstrumentacion As IBitacoras.TiposInstrumentacion = IBitacoras.TiposInstrumentacion.GestorIOperaciones

        Private _i_Clave_Modulo As Int32 = 0

        Private _dimension As String

        Private _entidad As String

        Private _granularidad As String

        Private _version As String

        Private _nombreClaveUpsert As String

        Private _activarComodinesDeConsulta As Boolean

        'Lista de registros o bulk
        Private _listaRegistros As List(Of RegistroIOperaciones)

        'Atributos para enlace
        Private _keyValues As List(Of String)

        'Área temporal para MongoDB y replicacion de datos
        Private _reflejarEn As IEnlaceDatos.DestinosParaReplicacion

        Private _objetoDatos As IConexiones.TiposRepositorio

        Private _objetoDatosAlternativo As IConexiones.TiposRepositorio
        Property _origenDatos As IConexiones.Controladores

        Property _origenDatosAlternativo As IConexiones.Controladores

        Property _tipoConexion As IConexiones.TipoConexion

        Property _tipoConexionAlternativa As IConexiones.TipoConexion

        Private _modalidadConsulta As IOperacionesCatalogo.ModalidadesConsulta

        Private _modalidadConsultaAlternativa As IOperacionesCatalogo.ModalidadesConsulta

        Private _bsonDocumentListResult As List(Of BsonDocument) ' MongoCursor(Of BsonDocument)

        Private _activarLecturaSucia As Boolean

#End Region

#Region "Constructores"

        Sub New()

            _visualizacionCamposConfigurada = IOperacionesCatalogo.TiposVisualizacionCampos.PresentarTodos

            _declaracionesAdicionalesPieTransaccion = Nothing

            _declaracionesAdicionalesUsuario = Nothing

            _instruccionesSQLAntesIniciarTransaccion = Nothing

            _planEjecucionSQL = Nothing

            _ejecutarPlanEjecucion = True

            _nombreToken = Nothing

            _tablaedicion = Nothing

            _identificadorcatalogo = Nothing

            _operadorcatalogoconsultas = Nothing

            _operadorcatalogoinsercion = Nothing

            _operadorcatalogoborrado = Nothing

            _operadorcatalogomodificacion = Nothing

            _vistaencabezados = Nothing

            _icaracteristicas = New Dictionary(Of Int32, ICaracteristica)

            _cantidadvisibleregistros = 200

            _cadenaencabezados = Nothing

            _valorindiceseleccionado = Nothing

            _valoresIndiceInsertados = New List(Of Int64)

            _iespaciotrabajo = New EspacioTrabajo

            _clausulaslibres = Nothing

            _sistema = New Organismo

            _tipoescritura = IOperacionesCatalogo.TiposEscritura.Inmediata

            _registrostemporales = New Dictionary(Of Int32, String)

            _registrostemporalespresentacion = New Dictionary(Of Int32, String)

            _registrostemporalesDataTable = New DataTable

            _registrostemporalesDataTable.Clear()

            _identificadorempresa = "-1"

            _salidallaves = "-1"

            _entradallaves = "-1"

            _filtraduplicados = False

            _clausulasAutoFiltro = Nothing

            _ordernarResultadosPorColumna = " ORDER BY 1 DESC"

            _coleccionInstrucciones = New Dictionary(Of String, IOperacionesCatalogo)

            _sqlTransaccionList = New Dictionary(Of String, String)

            _indiceTablaTemporal = 1

            _identificadorObjetoTransaccional = "###"

            _identificadorNivelTransaccional = "1"

            _identificadorCompletoLlamante = Nothing

            _indiceTablaTemporalLlamante = Nothing

            _tipoOperacionSQL = IOperacionesCatalogo.TiposOperacionSQL.SinDefinir

            _complejoTransaccional = IOperacionesCatalogo.ComplexTypes.ComplexA2B2B2A2

            _listaSQLNivel1 = New Dictionary(Of LlaveOrdenValor, String)

            _listaSQLNivel2 = New Dictionary(Of LlaveOrdenValor, String)

            _listaSQLNivel3 = New Dictionary(Of LlaveOrdenValor, String)

            _llavesDeclaradas = New List(Of String)


            _activarComodinesDeConsulta = True

            'Batería para lista de registros o bulk
            _listaRegistros = New List(Of RegistroIOperaciones)

            _objetoDatos = IConexiones.TiposRepositorio.Automatico

            _objetoDatosAlternativo = IConexiones.TiposRepositorio.Automatico

            _origenDatos = IConexiones.Controladores.SinDefinir

            _origenDatosAlternativo = IConexiones.Controladores.SinDefinir

            _tipoConexion = IConexiones.TipoConexion.Automatico

            _tipoConexionAlternativa = IConexiones.TipoConexion.Automatico

            _modalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.SinDefinir

            _modalidadConsultaAlternativa = IOperacionesCatalogo.ModalidadesConsulta.SinDefinir

            'Activar lectura sucia, default=True, MOP 05/07/2021
            _activarLecturaSucia = True

        End Sub

#End Region

#Region "Destructores"

#End Region

#Region "Propiedades"

        'Activar lectura sucia, default=True, MOP 05/07/2021
        Property ActivarLecturaSuciaSQL As Boolean
            Get
                Return _activarLecturaSucia
            End Get
            Set(value As Boolean)
                _activarLecturaSucia = value
            End Set
        End Property

        Property BSONDocumentResult As List(Of BsonDocument) 'MongoCursor(Of BsonDocument)
            Get
                Return _bsonDocumentListResult
            End Get
            Set(value As List(Of BsonDocument)) 'MongoCursor(Of BsonDocument))
                _bsonDocumentListResult = value
            End Set
        End Property

        Property ObjetoDatos As IConexiones.TiposRepositorio
            Get
                Return _objetoDatos
            End Get
            Set(value As IConexiones.TiposRepositorio)
                _objetoDatos = value
            End Set
        End Property

        Property ObjetoDatosAlternativo As IConexiones.TiposRepositorio
            Get
                Return _objetoDatosAlternativo
            End Get
            Set(value As IConexiones.TiposRepositorio)
                _objetoDatosAlternativo = value
            End Set
        End Property

        Property OrigenDatos As IConexiones.Controladores
            Get
                Return _origenDatos
            End Get
            Set(value As IConexiones.Controladores)
                _origenDatos = value
            End Set
        End Property

        Property OrigenDatosAlternativo As IConexiones.Controladores
            Get
                Return _origenDatosAlternativo
            End Get
            Set(value As IConexiones.Controladores)
                _origenDatosAlternativo = value
            End Set
        End Property

        Property TipoConexion As IConexiones.TipoConexion
            Get
                Return _tipoConexion
            End Get
            Set(value As IConexiones.TipoConexion)
                _tipoConexion = value
            End Set
        End Property

        Property TipoConexionAlternativa As IConexiones.TipoConexion
            Get
                Return _tipoConexionAlternativa
            End Get
            Set(value As IConexiones.TipoConexion)
                _tipoConexionAlternativa = value
            End Set
        End Property
        Property ReflejarEn As IEnlaceDatos.DestinosParaReplicacion
            Get
                Return _reflejarEn
            End Get
            Set(value As IEnlaceDatos.DestinosParaReplicacion)
                _reflejarEn = value
            End Set
        End Property

        Property KeyVales As List(Of String)
            Get
                Return _keyValues
            End Get
            Set(value As List(Of String))
                _keyValues = value
            End Set
        End Property


        ReadOnly Property ListaRegistrosIOperaciones As List(Of RegistroIOperaciones)

            Get
                Return _listaRegistros

            End Get

            'Set(value As List(Of RegistroIOperaciones))

            '    _listaRegistros = value

            'End Set

        End Property

        Property ActivarComodinesDeConsulta As Boolean
            Get
                Return _activarComodinesDeConsulta
            End Get
            Set(value As Boolean)
                _activarComodinesDeConsulta = value
            End Set
        End Property

        Property NombreClaveUpsert As String
            Get
                Return _nombreClaveUpsert
            End Get
            Set(value As String)
                _nombreClaveUpsert = value
            End Set
        End Property



        Public Property Dimension As String
            Get
                Return _dimension
            End Get
            Set(value As String)
                _dimension = value
            End Set
        End Property

        Public Property Entidad As String
            Get
                Return _entidad
            End Get
            Set(value As String)
                _entidad = value
            End Set
        End Property

        Public Property Granularidad As String
            Get
                Return _granularidad
            End Get
            Set(value As String)
                _granularidad = value
            End Set
        End Property

        Public Property Version As String
            Get
                Return _version
            End Get
            Set(value As String)
                _version = value
            End Set
        End Property

        'Private _i_Cve_Usuario As Int32 'IDUsuario = _i_Cve_Usuario
        Public Property IDUsuario As Int32
            Get
                Return _i_Cve_Usuario
            End Get
            Set(value As Int32)
                _i_Cve_Usuario = value
            End Set
        End Property

        'Private _i_Cve_DivisionMiEmpresa As Int32 'conexion_.ClaveDivisionMiEmpresa = _i_Cve_DivisionMiEmpresa
        Public Property ClaveDivisionMiEmpresa As Int32
            Get
                Return _i_Cve_DivisionMiEmpresa
            End Get
            Set(value As Int32)
                _i_Cve_DivisionMiEmpresa = value
            End Set
        End Property

        'Private _i_Cve_Aplicacion As Int32 'conexion_.IDAplicacion = _i_Cve_Aplicacion
        Public Property IDAplicacion As IBitacoras.ClaveTiposAplicacion
            Get
                Return _i_Cve_Aplicacion
            End Get
            Set(value As IBitacoras.ClaveTiposAplicacion)
                _i_Cve_Aplicacion = value
            End Set
        End Property

        'Private TipoInstrumentacion As IBitacoras.TiposInstrumentacion 'conexion_.TipoInstrumentacion = IBitacoras.TiposInstrumentacion.GestorIOperaciones
        Public Property TipoInstrumentacion As IBitacoras.TiposInstrumentacion
            Get
                Return _i_TipoInstrumentacion
            End Get
            Set(value As IBitacoras.TiposInstrumentacion)
                _i_TipoInstrumentacion = value
            End Set
        End Property

        'Private _i_Cve_RecursoSolicitante As Int32 'conexion_.IDRecursoSolicitante = _i_Clave_Modulo
        Public Property IDRecursoSolicitante As Int32
            Get
                Return _i_Clave_Modulo
            End Get
            Set(value As Int32)
                _i_Clave_Modulo = value
            End Set
        End Property

        Public Property EstructuraConsulta As IEntidadDatos

            Get

                Return _estructuraPresentacion

            End Get

            Set(value As IEntidadDatos)

                _estructuraPresentacion = value

            End Set

        End Property

        Public Property VisualizacionCamposConfigurada As IOperacionesCatalogo.TiposVisualizacionCampos

            Get

                Return _visualizacionCamposConfigurada

            End Get

            Set(value As IOperacionesCatalogo.TiposVisualizacionCampos)

                _visualizacionCamposConfigurada = value

            End Set

        End Property

        Public Property CadenaEncabezados As String

            Get
                Return _cadenaencabezados
            End Get

            Set(value As String)
                _cadenaencabezados = value
            End Set

        End Property


        Public Property ComplejoTransaccional As IOperacionesCatalogo.ComplexTypes

            Get

                Return _complejoTransaccional

            End Get

            Set(value As IOperacionesCatalogo.ComplexTypes)

                _complejoTransaccional = value

            End Set

        End Property

        Public Property TipoOperacionSQL As IOperacionesCatalogo.TiposOperacionSQL

            Get

                Return _tipoOperacionSQL

            End Get

            Set(value As IOperacionesCatalogo.TiposOperacionSQL)

                _tipoOperacionSQL = value

            End Set

        End Property

        Public Property IndiceTablaTemporalLLamante As String

            Get

                Return _indiceTablaTemporalLlamante

            End Get

            Set(value As String)

                _indiceTablaTemporalLlamante = value

            End Set

        End Property

        Public Property IDCompletoLlamante As String
            Get
                Return _identificadorCompletoLlamante
            End Get
            Set(value As String)
                _identificadorCompletoLlamante = value
            End Set
        End Property

        Public Property IndiceTablaTemporal As Int32
            Get

                Return _indiceTablaTemporal

            End Get
            Set(value As Int32)

                _indiceTablaTemporal = value

            End Set

        End Property

        Public Property IDObjectoTransaccional As String
            Get

                Return _identificadorObjetoTransaccional

            End Get
            Set(value As String)

                _identificadorObjetoTransaccional = value

            End Set

        End Property

        Public Property IDNivelTransaccional As String
            Get

                Return _identificadorNivelTransaccional

            End Get
            Set(value As String)

                _identificadorNivelTransaccional = value

            End Set

        End Property

        Public ReadOnly Property ColeccionInstrucciones As Dictionary(Of String, IOperacionesCatalogo)
            Get
                Return _coleccionInstrucciones
            End Get
        End Property

        Public Property SQLTransacccion As Dictionary(Of String, String)
            Get
                Return _sqlTransaccionList
            End Get
            Set(value As Dictionary(Of String, String))
                _sqlTransaccionList = value
            End Set
        End Property

        Public Property OrdenarResultadosPorColumna As String

            Get
                Return _ordernarResultadosPorColumna

            End Get

            Set(value As String)

                _ordernarResultadosPorColumna = value
            End Set

        End Property

        Public Property ClausulasAutoFiltro As String
            Get
                Return _clausulasAutoFiltro
            End Get
            Set(value As String)
                _clausulasAutoFiltro = value
            End Set

        End Property

        Public Property NoMostrarRegistrosInsertados As Boolean
            Get
                Return _filtraduplicados
            End Get
            Set(value As Boolean)
                _filtraduplicados = value
            End Set
        End Property

        Public Property EntradaLlaves As String
            Get
                Return _entradallaves
            End Get
            Set(value As String)
                _entradallaves = value
            End Set

        End Property

        Public Property SalidaLlaves As String
            Get
                Return _salidallaves
            End Get
            Set(value As String)
                _salidallaves = value
            End Set

        End Property

        Public Property IdentificadorEmpresa() As String
            Get
                Return _identificadorempresa
            End Get
            Set(value As String)
                _identificadorempresa = value
            End Set
        End Property

        Public Property RegistrosTemporales As Dictionary(Of Int32, String)
            Get
                Return _registrostemporales
            End Get
            Set(value As Dictionary(Of Int32, String))
                _registrostemporales = value
            End Set
        End Property

        Public Property RegistrosTemporalesDataTable As DataTable
            Get
                Return _registrostemporalesDataTable
            End Get
            Set(value As DataTable)
                _registrostemporalesDataTable = value
            End Set
        End Property

        Public Property TipoEscritura As IOperacionesCatalogo.TiposEscritura
            Get
                Return _tipoescritura
            End Get
            Set(value As IOperacionesCatalogo.TiposEscritura)
                _tipoescritura = value
            End Set
        End Property

        Public Property ClausulasLibres As String
            Get
                Return _clausulaslibres
            End Get
            Set(value As String)
                _clausulaslibres = value
            End Set
        End Property

        Public Property EspacioTrabajo As IEspacioTrabajo
            Get
                Return _iespaciotrabajo
            End Get
            Set(value As IEspacioTrabajo)
                _iespaciotrabajo = value
            End Set
        End Property

        Public Property ValorIndice As String
            Get
                Return _valorindiceseleccionado
            End Get
            Set(value As String)
                _valorindiceseleccionado = value
            End Set
        End Property

        Public Property ValoresIndiceInsertados() As List(Of Int64)

            Get

                Return _valoresIndiceInsertados

            End Get

            Set(ByVal value_ As List(Of Int64))

                _valoresIndiceInsertados = value_

            End Set

        End Property

        Public Property IdentificadorUnico As String
            Get
                Return _identificadorcatalogo

            End Get
            Set(value As String)
                _identificadorcatalogo = value
            End Set
        End Property

        Public Property NombreToken() As String

            Get

                Return _nombreToken

            End Get

            Set(ByVal value As String)

                _nombreToken = value

            End Set

        End Property

        Public Property TablaEdicion As String
            Get
                Return _tablaedicion
            End Get
            Set(value As String)
                _tablaedicion = value
            End Set
        End Property


        Public Property CantidadVisibleRegistros As Int32
            Get
                Return _cantidadvisibleregistros
            End Get
            Set(value As Int32)
                _cantidadvisibleregistros = value
            End Set
        End Property

        Public Property Caracteristicas As Dictionary(Of Int32, ICaracteristica)
            Get
                Return _icaracteristicas
            End Get
            Set(value As Dictionary(Of Int32, ICaracteristica))
                _icaracteristicas = value
            End Set
        End Property

        Public Property OperadorCatalogoConsultas As String
            Get
                Return _operadorcatalogoconsultas
            End Get
            Set(value As String)
                _operadorcatalogoconsultas = value
            End Set
        End Property

        Public Property OperadorCatalogoInsercion As String
            Get
                Return _operadorcatalogoinsercion
            End Get
            Set(value As String)
                _operadorcatalogoinsercion = value
            End Set
        End Property

        Public Property OperadorCatalogoBorrado As String
            Get
                Return _operadorcatalogoborrado
            End Get
            Set(value As String)
                _operadorcatalogoborrado = value
            End Set
        End Property

        Public Property OperadorCatalogoModificacion As String
            Get
                Return _operadorcatalogomodificacion
            End Get
            Set(value As String)
                _operadorcatalogomodificacion = value
            End Set
        End Property

        Public Property VistaEncabezados As String
            Get
                Return _vistaencabezados
            End Get
            Set(value As String)
                _vistaencabezados = value
            End Set
        End Property


        Public Property ModalidadConsulta As String
            Get
                Return _modalidadConsulta
            End Get
            Set(value As String)
                _modalidadConsulta = value
            End Set
        End Property

        Public Property ModalidadConsultaAlternativa As String
            Get
                Return _modalidadConsultaAlternativa
            End Get
            Set(value As String)
                _modalidadConsultaAlternativa = value
            End Set
        End Property


#End Region

#Region "Métodos"

        Private Sub AplicaLlaves(ByVal llaveanterior_ As String,
                                 ByVal llaveactual As String)

            Dim registronuevo_ As New Dictionary(Of Int32, String)

            Dim registrolisto_ As String = Nothing

            For Each registro_ As String In _registrostemporales.Values

                registrolisto_ = registro_.Replace(llaveanterior_, llaveactual)

                For Each _campoexcluido As ICaracteristica In _icaracteristicas.Values

                    If _campoexcluido.PuedeInsertar = ICaracteristica.TiposRigorDatos.No Then

                        Dim patron_ As String =
                        "([^,'[\.]*'[\s]*[a|A]s[\s]*'" &
                        _campoexcluido.NombreMostrar &
                        "']*[\s]*[\,]*[\s]*[\']*)"

                        registrolisto_ = Regex.Replace(registrolisto_, patron_, String.Empty)

                    Else

                        Continue For

                    End If

                Next

                registronuevo_.Add(registronuevo_.Count + 1, registrolisto_)

            Next

            _registrostemporales.Clear()

            _registrostemporales = registronuevo_

        End Sub

        Public Function GrabarDatosEnDisco(Optional ByVal pilallaves_ As Dictionary(Of Int32, ICaracteristica) = Nothing,
                                           Optional ByVal tipoescritura_ As IOperacionesCatalogo.TiposEscritura = IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia) As Boolean

            Select Case tipoescritura_

                Case IOperacionesCatalogo.TiposEscritura.MemoriaIntermediaBeta

                    If _registrostemporales.Count > 0 And
                        _icaracteristicas.Count > 0 Then

                        Dim sqltexto_ As String = Nothing

                        Dim campos_ As String = Nothing

                        Dim respuestasingleton_ As Boolean = False

                        If Not pilallaves_ Is Nothing Then
                            For Each llave_ As CaracteristicaCatalogo In pilallaves_.Values
                                Dim llaveanterior_ As String = Nothing
                                Dim llaveactual As String = Nothing

                                llaveanterior_ = "'-1' as '" & llave_.NombreMostrar & "'"

                                llaveactual = "'" & llave_.ValorAsignado & "' as '" & llave_.NombreMostrar & "'"

                                AplicaLlaves(llaveanterior_,
                                             llaveactual)

                            Next

                        End If

                        For Each registro_ As String In _registrostemporales.Values

                            If sqltexto_ Is Nothing Then

                                sqltexto_ = "select " & registro_

                            Else

                                sqltexto_ = sqltexto_ & " union all select " & registro_

                            End If

                        Next

                        Dim contadorcaracteristicas_ As Int32 = 0

                        For Each icaracteristica_ As CaracteristicaCatalogo In _icaracteristicas.Values

                            If Not icaracteristica_.PuedeInsertar = ICaracteristica.TiposRigorDatos.Si Then
                                Continue For
                            End If

                            If campos_ Is Nothing Then
                                campos_ = icaracteristica_.Nombre
                            Else
                                campos_ = campos_ & "," & icaracteristica_.Nombre

                            End If

                            contadorcaracteristicas_ += 1
                        Next

                        sqltexto_ = " insert into " & _tablaedicion & " ( " & campos_ & ")" &
                                    "(" & sqltexto_ & " );"

                        If Not _iespaciotrabajo Is Nothing Then

                            If Not _iespaciotrabajo.MisCredenciales Is Nothing Then

                                ConexionSingleton.UsuarioCliente = _iespaciotrabajo.MisCredenciales.CredencialUsuario

                            End If

                        Else
                            ConexionSingleton.UsuarioCliente = _sistema.ConexionSingleton.UsuarioCliente
                        End If


                        ConexionSingleton.ModuloCliente = _operadorcatalogoconsultas

                        respuestasingleton_ = ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(sqltexto_)

                        If respuestasingleton_ Then

                            _registrostemporales.Clear()

                        End If

                        Return respuestasingleton_

                    Else

                        Return False

                    End If

                Case IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda

                    If _registrostemporalesDataTable.Rows.Count > 0 And
                        _icaracteristicas.Count > 0 Then

                        Dim sqltexto_ As String = Nothing

                        Dim campos_ As String = Nothing

                        Dim respuestasingleton_ As Boolean = False

                        If Not pilallaves_ Is Nothing Then

                            For Each llave_ As CaracteristicaCatalogo In pilallaves_.Values

                                Dim llaveanterior_ As String = Nothing

                                Dim llaveactual As String = Nothing

                                For Each registrotemp_ As DataRow In RegistrosTemporalesDataTable.Rows

                                    registrotemp_(llave_.NombreMostrar) = llave_.ValorAsignado

                                Next

                            Next

                        End If

                        Dim contadorcaracteristicas_ As Int32 = 0

                        For Each icaracteristica_ As CaracteristicaCatalogo In _icaracteristicas.Values

                            If Not (icaracteristica_.PuedeInsertar = ICaracteristica.TiposRigorDatos.Si Or
                                    icaracteristica_.PuedeInsertar = ICaracteristica.TiposRigorDatos.Opcional) Then

                                Continue For

                            End If

                            If campos_ Is Nothing Then

                                campos_ = icaracteristica_.Nombre

                            Else

                                campos_ = campos_ & "," & icaracteristica_.Nombre

                            End If

                            contadorcaracteristicas_ += 1
                        Next

                        SQLTransacccion.Clear()

                        For Each registror_ As DataRow In _registrostemporalesDataTable.Rows

                            Dim registrotexto_ As String

                            Dim valores_ As String = Nothing

                            registrotexto_ = ConversionRegistroTexto(registror_)


                            valores_ =
                                "select " & registrotexto_.ToString

                            Dim idLlamante_ As String = Nothing

                            Dim llaveSQL_ As String = Nothing

                            If Not _indiceTablaTemporalLlamante Is Nothing _
                                And _indiceTablaTemporalLlamante <> "0" Then

                                idLlamante_ = _indiceTablaTemporalLlamante & "."

                            Else

                                MsgBox("No se encontró el llamante!,ID= " &
                                IDObjectoTransaccional & "." & IDNivelTransaccional & "." & idLlamante_ & registror_.Item("Id").ToString)

                            End If

                            llaveSQL_ = IDObjectoTransaccional & "." & IDNivelTransaccional & "." & idLlamante_ & registror_.Item("Id").ToString

                            valores_ =
                                Chr(13) &
                                Chr(13) &
                                Chr(9) & "/******* " & IDObjectoTransaccional & "." & IDNivelTransaccional & "." & idLlamante_ & registror_.Item("Id").ToString & " *******/" & Chr(13) &
                                Chr(9) & " insert into " & _tablaedicion & " ( " & campos_ & ")" & Chr(13) &
                                Chr(9) & " (" & valores_ & " );"

                            If SQLTransacccion.ContainsKey(llaveSQL_) Then

                                SQLTransacccion.Remove(llaveSQL_)

                            End If

                            SQLTransacccion.Add(llaveSQL_, valores_)

                            IndiceTablaTemporal = 1

                        Next

                        respuestasingleton_ = True

                        If respuestasingleton_ Then

                            _registrostemporales.Clear()

                        End If

                        Return respuestasingleton_

                    Else

                        Return False

                    End If

                Case IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia

                    If _registrostemporalesDataTable.Rows.Count > 0 And
                        _icaracteristicas.Count > 0 Then

                        Dim sqltexto_ As String = Nothing

                        Dim campos_ As String = Nothing

                        Dim respuestasingleton_ As Boolean = False

                        If Not pilallaves_ Is Nothing Then

                            For Each llave_ As CaracteristicaCatalogo In pilallaves_.Values

                                Dim llaveanterior_ As String = Nothing

                                Dim llaveactual As String = Nothing

                                For Each registrotemp_ As DataRow In RegistrosTemporalesDataTable.Rows

                                    registrotemp_(llave_.NombreMostrar) = llave_.ValorAsignado

                                Next

                            Next

                        End If

                        For Each registror_ As DataRow In _registrostemporalesDataTable.Rows

                            Dim registrotexto_ As String

                            registrotexto_ = ConversionRegistroTexto(registror_)

                            If sqltexto_ Is Nothing Then

                                sqltexto_ = "select " & registrotexto_.ToString

                            Else

                                sqltexto_ = sqltexto_ & " union all select " & registrotexto_.ToString

                            End If

                        Next

                        Dim contadorcaracteristicas_ As Int32 = 0

                        For Each icaracteristica_ As CaracteristicaCatalogo In _icaracteristicas.Values

                            If Not (icaracteristica_.PuedeInsertar = ICaracteristica.TiposRigorDatos.Si Or
                                    icaracteristica_.PuedeInsertar = ICaracteristica.TiposRigorDatos.Opcional) Then

                                Continue For

                            End If

                            If campos_ Is Nothing Then

                                campos_ = icaracteristica_.Nombre

                            Else

                                campos_ = campos_ & "," & icaracteristica_.Nombre

                            End If

                            contadorcaracteristicas_ += 1

                        Next

                        sqltexto_ = " insert into " & _tablaedicion & " ( " & campos_ & ")" &
                                    "(" & sqltexto_ & " );"

                        If Not _iespaciotrabajo Is Nothing Then

                            If Not _iespaciotrabajo.MisCredenciales Is Nothing Then

                                ConexionSingleton.UsuarioCliente = _iespaciotrabajo.MisCredenciales.CredencialUsuario

                            End If

                        Else
                            ConexionSingleton.UsuarioCliente = _sistema.ConexionSingleton.UsuarioCliente
                        End If


                        ConexionSingleton.ModuloCliente = _operadorcatalogoconsultas

                        respuestasingleton_ = ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(sqltexto_)

                        If respuestasingleton_ Then

                            'Grabando bitacora
                            _sistema.Log("Se ha insertado correctamente el detalle ",
                             monitoreo.IBitacoras.TiposBitacora.Informacion,
                            monitoreo.IBitacoras.TiposSucesos.Insertar,
                            _iespaciotrabajo.MisCredenciales,
                            0,
                            sqltexto_.Replace("'", Nothing).Replace(Chr(34), Nothing).Replace("char(39)", Nothing).Replace("+", Nothing),
                            OperadorCatalogoConsultas,
                            EspacioTrabajo.MisCredenciales.NombreAutenticacion)


                            If Not _registrostemporales Is Nothing Then
                                _registrostemporales.Clear()
                            End If

                            If Not _registrostemporalesDataTable Is Nothing Then
                                _registrostemporalesDataTable.Clear()
                            End If


                            _indiceTablaTemporal = 1

                        End If

                        Return respuestasingleton_

                    Else

                        Return False

                    End If

                Case Else

            End Select

            Return False
        End Function

        Private Function EncabezadoTransacciones() As String

            Dim datos_ As String = Nothing

            datos_ = " BEGIN TRANSACTION Transaccion " & Chr(13) &
                     Chr(13) &
                     " BEGIN TRY " & Chr(13) & Chr(13) & Chr(13)

            Return datos_

        End Function

        '" SET TRANSACTION ISOLATION LEVEL SERIALIZABLE " & Chr(13) & Chr(13) & _


        Private Function ConfiguracionTransacciones() As String

            Dim datos_ As String = Nothing

            If _activarLecturaSucia Then

                datos_ = " SET NOCOUNT ON " & Chr(13) &
                     " SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & Chr(13) & Chr(13) &
                     _instruccionesSQLAntesIniciarTransaccion
            Else

                Return _instruccionesSQLAntesIniciarTransaccion

            End If

            Return datos_

        End Function

        Private Function PieTransacciones() As String

            Dim datos_ As String = Nothing

            datos_ = Chr(13) &
                "COMMIT TRANSACTION Transaccion" & Chr(13) &
                "PRINT 'Se ha insertado correctamente'" & Chr(13) &
                _instruccionesTransaccionCometida & Chr(13) &
                "END TRY" & Chr(13) &
                "BEGIN CATCH" & Chr(13) &
                " ROLLBACK TRANSACTION Transaccion" & Chr(13) &
                " SELECT ERROR_NUMBER() AS ErrorNumber," & Chr(13) &
                " ERROR_SEVERITY() AS ErrorSeverity," & Chr(13) &
                " ERROR_STATE() AS ErrorState," & Chr(13) &
                " ERROR_PROCEDURE() AS ErrorProcedure," & Chr(13) &
                " ERROR_LINE() AS ErrorLine," & Chr(13) &
                " ERROR_MESSAGE() AS ErrorMessage" & Chr(13) &
                "END CATCH"

            Return datos_

        End Function

        Public Function FormaPlan(ByVal tipoComplejo_ As IOperacionesCatalogo.ComplexTypes) As String

            Dim transaccionCompleta_ As String = Nothing

            Dim transaccionCompletaFinal_ As String = Nothing

            Dim llaveNivel1_ As String = Nothing

            Dim llavesNivel2_ As New List(Of String)

            Dim llavesNivel3_ As New List(Of String)

            Dim listaMarcadores_ As New List(Of String)

            Dim encabezadoTransaccion_ As String = EncabezadoTransacciones()

            Dim declaracionesTransaccion_ As String = Nothing

            Dim configuracionTransaccion_ As String = ConfiguracionTransacciones()

            Dim pieTransaccion_ As String = PieTransacciones()

            Select Case tipoComplejo_

                Case IOperacionesCatalogo.ComplexTypes.ComplexA2B2B2nA2,
                     IOperacionesCatalogo.ComplexTypes.ComplexA2B2B2A2

                    Dim nivelIndice_ As Int32 = 1

                    _listaSQLNivel1.Clear()

                    _listaSQLNivel2.Clear()

                    _listaSQLNivel3.Clear()

                    If _coleccionInstrucciones.Count >= 1 Then

                        Dim keys_ As List(Of String) = _coleccionInstrucciones.Keys.ToList

                        keys_.Sort()

                        Dim index_ As String

                        For Each index_ In keys_

                            ProcesaNivelesTransaccionales(
                                _coleccionInstrucciones.Item(index_),
                                nivelIndice_,
                                index_,
                                1)

                        Next

                        DepuraInstrucciones()

                        transaccionCompleta_ = GeneraInstrucciones()

                        For Each llaveN2_ As String In _llavesDeclaradas

                            declaracionesTransaccion_ = declaracionesTransaccion_ &
                              "DECLARE " & llaveN2_ & " as integer;" & Chr(13)
                        Next

                    End If

                Case IOperacionesCatalogo.ComplexTypes.ComplexA2B2A2
                    '18/03/2020 MOP, 
                    'Hoy hace coronavirus allá afuera, no tenemos miedo, sólo no tenemos información confiable.
                    'El mundo sigue hablando después que todo se ha dicho sólo por no callar.

                    Dim nivelIndice_ As Int32 = 1

                    _listaSQLNivel1.Clear()

                    _listaSQLNivel2.Clear()

                    _listaSQLNivel3.Clear()

                    If _coleccionInstrucciones.Count >= 1 Then

                        Dim keys_ As List(Of String) = _coleccionInstrucciones.Keys.ToList

                        keys_.Sort()

                        Dim index_ As String

                        For Each index_ In keys_

                            ProcesaNivelesTransaccionales(
                                _coleccionInstrucciones.Item(index_),
                                nivelIndice_,
                                index_,
                                1)

                        Next

                        DepuraInstrucciones()

                        transaccionCompleta_ = GeneraInstrucciones()

                        For Each llaveN2_ As String In _llavesDeclaradas

                            declaracionesTransaccion_ = declaracionesTransaccion_ &
                              "DECLARE " & llaveN2_ & " as integer;" & Chr(13)
                        Next

                    End If


                Case Else
                    'NOT IMPLEMENTED

            End Select

            transaccionCompleta_ =
                encabezadoTransaccion_ &
                declaracionesTransaccion_ &
                _declaracionesAdicionalesUsuario &
                configuracionTransaccion_ &
                transaccionCompleta_ &
                _declaracionesAdicionalesPieTransaccion &
                pieTransaccion_

            ' MsgBox("Trnasacción:" & transaccionCompleta_)

            Return transaccionCompleta_

        End Function

        Private Function GeneraInstrucciones() As String

            Dim instrucciones_ As String = Nothing

            Dim datos_ As New List(Of String)

            Dim registro_ As String = Nothing

            Dim llaveEntradaN1_ As String = Nothing

            Dim llaveSalidaN1_ As String = Nothing

            Dim iteracionNivel1_ As Int32 = 1

            Dim iteracionNivel2_ As Int32 = 1

            For Each parDatos_ As KeyValuePair(Of LlaveOrdenValor, String) In _listaSQLNivel1

                Dim itemAuxiliar_ As String() = parDatos_.Key._llaveString.Split(".")

                Dim itemInicial_ As String = itemAuxiliar_(itemAuxiliar_.Length - 1)

                datos_.Add(itemInicial_)

                Select Case iteracionNivel1_

                    Case 1

                        registro_ = parDatos_.Value

                        llaveSalidaN1_ = "@Key_N1_" & itemInicial_

                        llaveEntradaN1_ = llaveSalidaN1_

                    Case Else

                        registro_ = parDatos_.Value.Replace("-0.00019810210", llaveEntradaN1_)

                End Select

                llaveSalidaN1_ = "@Key_N1_" & itemInicial_

                If Not _llavesDeclaradas.Contains(llaveSalidaN1_) Then

                    _llavesDeclaradas.Add(llaveSalidaN1_)

                End If

                instrucciones_ =
                instrucciones_ &
                 Chr(13) &
                "/******** " & parDatos_.Key._llaveString & " **********/" &
                Chr(13) &
                registro_ &
                Chr(13) &
                "Set " & llaveSalidaN1_ & " = @@IDENTITY;" &
                Chr(13)

                For Each parDatos2_ As KeyValuePair(Of LlaveOrdenValor, String) In _listaSQLNivel2

                    Dim itemAuxiliar2_ As String() = parDatos2_.Key._indiceSuperiorString.Split(".")

                    If itemAuxiliar2_(itemAuxiliar2_.Length - 1) = itemInicial_ Then

                        registro_ = parDatos2_.Value.Replace("-0.00019810210", llaveSalidaN1_)

                        instrucciones_ =
                            instrucciones_ &
                            Chr(13) &
                            Chr(9) & registro_ &
                            Chr(13)

                    End If

                Next

                iteracionNivel1_ += 1

            Next

            Return instrucciones_

        End Function

        Private Sub DepuraInstrucciones()

            Dim datos_ As New List(Of String)

            Dim _listaSQLNivel2Temporal As New Dictionary(Of LlaveOrdenValor, String)

            For Each parDatos_ As KeyValuePair(Of LlaveOrdenValor, String) In _listaSQLNivel1

                Dim itemAuxiliar_ As String() = parDatos_.Key._llaveString.Split(".")

                datos_.Add(itemAuxiliar_(itemAuxiliar_.Length - 1))

            Next

            Dim parDatos2_ As New KeyValuePair(Of LlaveOrdenValor, String)

            For Each parDatos2_ In _listaSQLNivel2

                Dim item2_ As String() = Split(parDatos2_.Key._indiceSuperiorString, ".")

                Dim eliminar_ As Boolean = True

                For Each item1_ As String In datos_

                    If item1_.Contains(item2_(item2_.Length - 1)) Then : eliminar_ = False : End If

                Next

                If Not eliminar_ Then

                    _listaSQLNivel2Temporal.Add(parDatos2_.Key, parDatos2_.Value)

                End If

            Next

            _listaSQLNivel2.Clear()

            _listaSQLNivel2 = _listaSQLNivel2Temporal

        End Sub

        Private Function BuscaLlaveStringColeccion(ByRef lista_ As Dictionary(Of LlaveOrdenValor, String), ByVal llave_ As String) As Boolean

            Dim parDatos_ As New KeyValuePair(Of LlaveOrdenValor, String)

            For Each parDatos_ In lista_

                If parDatos_.Key._llaveString = llave_ Then

                    Return True

                End If

            Next

            Return False

        End Function

        Private Function ExisteRegistroTransaccion(ByVal id_ As LlaveOrdenValor,
                                                   ByVal valor_ As String,
                                                   ByVal nivel_ As Int32) As Boolean

            Select Case nivel_

                Case 1
                    If BuscaLlaveStringColeccion(_listaSQLNivel1, id_._llaveString) Or
                       _listaSQLNivel1.ContainsValue(valor_) Then

                        Return True

                    End If

                Case 2
                    'NOT IMPLEMENTED

                Case 3
                    If BuscaLlaveStringColeccion(_listaSQLNivel3, id_._llaveString) Or
                       _listaSQLNivel3.ContainsValue(valor_) Then

                        Return True

                    End If

            End Select



            Return False

        End Function

        Private Sub AgregaItemsNivel(ByVal instruccionesSQL_ As Dictionary(Of String, String),
                                     ByVal idNivel_ As Int32,
                                     ByVal indiceSuperiorString_ As String,
                                     ByVal indiceSuperiorInt32_ As Int32)

            Dim parCadenas_ As KeyValuePair(Of String, String)

            For Each parCadenas_ In instruccionesSQL_

                Dim llaveOrdenaValor_ As New LlaveOrdenValor

                llaveOrdenaValor_._activa = True

                llaveOrdenaValor_._llaveInt32 = 0

                llaveOrdenaValor_._llaveString = parCadenas_.Key

                llaveOrdenaValor_._indiceSuperiorString = indiceSuperiorString_

                llaveOrdenaValor_._indiceSuperiorInt32 = indiceSuperiorInt32_

                If Not ExisteRegistroTransaccion(llaveOrdenaValor_, parCadenas_.Value, idNivel_) Then

                    Select Case idNivel_

                        Case 1 : llaveOrdenaValor_._llaveInt32 = _listaSQLNivel1.Count + 1

                            _listaSQLNivel1.Add(llaveOrdenaValor_,
                                                parCadenas_.Value)
                        Case 2 : llaveOrdenaValor_._llaveInt32 = _listaSQLNivel2.Count + 1

                            _listaSQLNivel2.Add(llaveOrdenaValor_,
                                                parCadenas_.Value)
                        Case 3 : llaveOrdenaValor_._llaveInt32 = _listaSQLNivel3.Count + 1

                            _listaSQLNivel3.Add(llaveOrdenaValor_,
                                                parCadenas_.Value)

                        Case Else

                            _sistema.GsDialogo("Este nivel no es soportado aún por el framework!")

                    End Select

                End If

            Next

        End Sub

        Private Function VericaSubNivel(ByRef operacionActual_ As IOperacionesCatalogo) As Boolean

            If Not operacionActual_.ColeccionInstrucciones Is Nothing Then

                If operacionActual_.ColeccionInstrucciones.Count >= 1 Then

                    Return True

                Else

                    Return False

                End If

            Else

                Return False

            End If

        End Function

        Private Sub ProcesaNivelesTransaccionales(ByVal operacionActual_ As IOperacionesCatalogo,
                                                  ByVal nivelIndice_ As Int32,
                                                  ByVal indiceSuperiorString_ As String,
                                                  ByVal indiceSuperiorInt32_ As Int32)

            AgregaItemsNivel(
                operacionActual_.SQLTransaccion,
                nivelIndice_,
                indiceSuperiorString_,
                indiceSuperiorInt32_)

            If VericaSubNivel(operacionActual_) Then

                nivelIndice_ += 1

                Dim keys_ As List(Of String) = operacionActual_.ColeccionInstrucciones.Keys.ToList

                Dim index_ As String

                keys_.Sort()

                For Each index_ In keys_

                    indiceSuperiorString_ = index_

                    indiceSuperiorInt32_ = nivelIndice_

                    ProcesaNivelesTransaccionales(
                        operacionActual_.ColeccionInstrucciones.Item(index_),
                        nivelIndice_,
                        indiceSuperiorString_,
                       indiceSuperiorInt32_)

                Next

            End If

        End Sub


        Public Function EjecutaPlanSQL() As Boolean

            Dim respuesta_ As Boolean = False

            If _ejecutarPlanEjecucion Then

                If Not _planEjecucionSQL Is Nothing Then

                    If Not _iespaciotrabajo Is Nothing Then

                        If Not _iespaciotrabajo.MisCredenciales Is Nothing Then

                            ConexionSingleton.UsuarioCliente = _iespaciotrabajo.MisCredenciales.CredencialUsuario

                        End If

                    Else
                        ConexionSingleton.UsuarioCliente = _sistema.ConexionSingleton.UsuarioCliente
                    End If


                    ConexionSingleton.ModuloCliente = _operadorcatalogoconsultas

                    respuesta_ = ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(_planEjecucionSQL)

                End If

            End If

            Return respuesta_

        End Function


        Function EjecutaInstrucciones() As Boolean

            Dim respuesta_ As Boolean = False

            Dim scriptSQL_ As String = Nothing

            If ComplejoTransaccional <> IOperacionesCatalogo.ComplexTypes.Undefined Then

                Select Case ComplejoTransaccional

                    Case IOperacionesCatalogo.ComplexTypes.ComplexA2B2B2A2

                        scriptSQL_ = FormaPlan(ComplejoTransaccional)

                        'If Not scriptSQL_ Is Nothing Then

                        '    respuesta_ = ConexionSingleton.SQLServerSingletonConexion.EjecutaSentencia(scriptSQL_)

                        'End If

                    Case IOperacionesCatalogo.ComplexTypes.ComplexA2B2B2nA2

                        scriptSQL_ = FormaPlan(ComplejoTransaccional)

                        'If Not scriptSQL_ Is Nothing Then

                        '    respuesta_ = ConexionSingleton.SQLServerSingletonConexion.EjecutaSentencia(scriptSQL_)

                        'End If

                    Case IOperacionesCatalogo.ComplexTypes.ComplexA2B2A2

                        scriptSQL_ = FormaPlan(ComplejoTransaccional)


                    Case Else

                        _sistema.GsDialogo("El Complejo transaccional " & ComplejoTransaccional.ToString & " no está implementado")

                End Select

            End If

            If _ejecutarPlanEjecucion Then

                If Not scriptSQL_ Is Nothing Then

                    If Not _iespaciotrabajo Is Nothing Then

                        If Not _iespaciotrabajo.MisCredenciales Is Nothing Then

                            ConexionSingleton.UsuarioCliente = _iespaciotrabajo.MisCredenciales.CredencialUsuario

                        End If

                    Else
                        ConexionSingleton.UsuarioCliente = _sistema.ConexionSingleton.UsuarioCliente
                    End If


                    ConexionSingleton.ModuloCliente = _operadorcatalogoconsultas

                    respuesta_ = ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(scriptSQL_)

                End If

            Else

                _planEjecucionSQL = scriptSQL_

            End If

            TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

            Return respuesta_

        End Function


        Private Function ConversionRegistroTexto(ByVal registrorow_ As DataRow) As String

            Dim registrotexto_ As String = Nothing

            Dim contador_ As Int32 = 0

            If Not _icaracteristicas Is Nothing Then

                For Each caracteristica_ As CaracteristicaCatalogo In _icaracteristicas.Values

                    If caracteristica_.PuedeInsertar = ICaracteristica.TiposRigorDatos.No Then

                        Continue For

                    End If

                    If contador_ = 0 Then

                        If Not (IsDBNull(registrorow_(caracteristica_.NombreMostrar))) Then

                            If Not (Trim(registrorow_(caracteristica_.NombreMostrar)) = "") Then
                                'If Not (Trim(registrorow_(caracteristica_.ValorAsignado)) = "") Then

                                If Not (Trim(registrorow_(caracteristica_.NombreMostrar)) = "") And
                                    Not ((registrorow_(caracteristica_.NombreMostrar)) Is Nothing) Then

                                    'bautismen
                                    registrotexto_ =
                                    EnvolturaDirecta(registrorow_(caracteristica_.NombreMostrar), caracteristica_.TipoDato, "'")

                                Else

                                    registrotexto_ =
                                    EnvolturaDirecta("NULL", ICaracteristica.TiposCaracteristica.cInt32, "")

                                End If

                            Else

                                registrotexto_ =
                                EnvolturaDirecta("NULL", ICaracteristica.TiposCaracteristica.cInt32, "")

                            End If

                        Else

                            registrotexto_ =
                            EnvolturaDirecta("NULL", ICaracteristica.TiposCaracteristica.cInt32, "")

                        End If

                        'registrotexto_ = _
                        'EnvolturaDirecta(registrorow_(caracteristica_.NombreMostrar), caracteristica_.TipoDato, "'")

                    Else

                        'If Not IsDBNull(registrorow_(caracteristica_.NombreMostrar)) Then
                        If Not (IsDBNull(registrorow_(caracteristica_.NombreMostrar))) Then

                            ' If Not (Trim(registrorow_(caracteristica_.NombreMostrar)) = "") Then

                            If Not (Trim(registrorow_(caracteristica_.NombreMostrar)) = "") Then

                                If IsNumeric((registrorow_(caracteristica_.NombreMostrar))) Then

                                    registrotexto_ =
                                    registrotexto_ & " , " & EnvolturaDirecta(registrorow_(caracteristica_.NombreMostrar), caracteristica_.TipoDato, "'")

                                Else

                                    If Not ((registrorow_(caracteristica_.NombreMostrar)) Is Nothing) Then

                                        registrotexto_ =
                                        registrotexto_ & " , " & EnvolturaDirecta(registrorow_(caracteristica_.NombreMostrar), caracteristica_.TipoDato, "'")

                                    Else
                                        registrotexto_ =
                                        registrotexto_ & " , " & EnvolturaDirecta("NULL", ICaracteristica.TiposCaracteristica.cInt32, "")

                                    End If

                                End If

                            Else

                                registrotexto_ =
                                registrotexto_ & " , " & EnvolturaDirecta("NULL", ICaracteristica.TiposCaracteristica.cInt32, "")

                            End If

                        Else

                            registrotexto_ =
                             registrotexto_ & " , " & EnvolturaDirecta("NULL", ICaracteristica.TiposCaracteristica.cInt32, "")

                        End If

                    End If

                    contador_ += 1

                Next
            End If

            Return registrotexto_

        End Function

        Public Sub OcultaCamposGridView(ByRef gridview_ As DataGridView) 'As DataGridView

            If gridview_.RowCount > 0 Then

                For Each caracteristica_ As ICaracteristica In _icaracteristicas.Values

                    If caracteristica_.Visible = ICaracteristica.TiposVisible.Si Or
                        caracteristica_.Visible = ICaracteristica.TiposVisible.Informacion Then

                        If gridview_.Columns.Contains(caracteristica_.NombreMostrar) Then

                            If caracteristica_.TipoDato = ICaracteristica.TiposCaracteristica.cString Then

                                'gridview_.Columns.Item(caracteristica_.NombreMostrar).AutoSizeMode = _
                                'DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader


                                gridview_.Columns.Item(caracteristica_.NombreMostrar).AutoSizeMode =
                                        DataGridViewAutoSizeColumnMode.DisplayedCells


                                ''Se crea la segunda columna 
                                'Dim Col1 As New TextAndImageColumn
                                'Col1.Image = Image.FromFile("C:\svn\svn qa\configuracion\recursos\cross.png")
                                'gridview_.Columns.RemoveAt().Insert(1, Col1)

                                ''Para cambiar el texto y la imagen de una celda, debeis utilizar este codigo (yo modifico la celda 
                                ''que esta en la primera fila y segunda columna): 

                                'DirectCast(gridview_.Columns.Item(caracteristica_.NombreMostrar), TextAndImageCell).Value = "Nuevo texto"
                                'DirectCast(gridview_.Item(0, 0), TextAndImageCell).Image = Image.FromFile("C:\svn\SVN QA\Configuracion\Recursos\cross.png")


                            Else

                                gridview_.Columns.Item(caracteristica_.NombreMostrar).AutoSizeMode =
                                DataGridViewAutoSizeColumnMode.ColumnHeader

                            End If



                            gridview_.Columns.Item(caracteristica_.NombreMostrar).DefaultCellStyle.BackColor = Drawing.Color.WhiteSmoke

                            gridview_.Columns.Item(caracteristica_.NombreMostrar).ReadOnly = True

                            If caracteristica_.PuedeModificar = ICaracteristica.TiposRigorDatos.Si Then

                                gridview_.Columns.Item(caracteristica_.NombreMostrar).ReadOnly = False

                                gridview_.Columns.Item(caracteristica_.NombreMostrar).DefaultCellStyle.BackColor = Drawing.Color.White

                            End If

                        End If

                        Continue For

                    End If

                    'Try
                    If gridview_.Columns.Contains(caracteristica_.NombreMostrar) Then

                        gridview_.Columns.Item(caracteristica_.NombreMostrar).Visible = False

                    End If


                    'Catch ex As Exception

                    'End Try

                Next

            End If





        End Sub

        'bautismen
        Private Function ReplicarCaracteristica(ByRef caracteristicas_ As Dictionary(Of Int32, ICaracteristica),
                                                ByVal nombre_ As String) As ICaracteristica

            Dim campoNuevo_ As ICaracteristica = New CaracteristicaCatalogo


            For Each parDatos_ As KeyValuePair(Of Int32, ICaracteristica) In caracteristicas_

                If parDatos_.Value.Nombre = nombre_ Then

                    campoNuevo_ = parDatos_.Value

                    Return campoNuevo_

                    Exit For

                End If



                'Dim itemAuxiliar_ As String() = parDatos_.Key._llaveString.Split(".")

                'Dim itemInicial_ As String = itemAuxiliar_(itemAuxiliar_.Length - 1)

                'datos_.Add(itemInicial_)

                'Select Case iteracionNivel1_

                '    Case 1

                '        registro_ = parDatos_.Value

                '        llaveSalidaN1_ = "@Key_N1_" & itemInicial_

                '        llaveEntradaN1_ = llaveSalidaN1_

                '    Case Else

                '        registro_ = parDatos_.Value.Replace("-0.00019810210", llaveEntradaN1_)

                'End Select

            Next

            _sistema.GsDialogo("No se encontró ninguna caracteristica con el nombre '" & nombre_ & "' en la VE por defecto")

            Return campoNuevo_

        End Function


        Private Sub ReconfiguraCaracteristicas(ByRef caracteristicas_ As Dictionary(Of Int32, ICaracteristica),
                                               ByVal estructuraPresentacion_ As IEntidadDatos)

            'Creamos un nuevo diccionario para caracteristicas marcadas
            Dim diccionarioCaracteristicasnuevas_ As New Dictionary(Of Int32, ICaracteristica)

            'Eliminamos los datos en las caracteristicas excedentes
            Dim indice_ As Int32 = 1

            For Each campo_ As CampoVirtual In estructuraPresentacion_.Atributos

                'Dim caracteristicaNueva_ As ICaracteristica = New CaracteristicaCatalogo

                'With caracteristicaNueva_
                '    .Llave = ICaracteristica.TipoLlave.SinLlave 'ObtenerValorCaracteristica(caracteristicas_, campo_.Atributo.ToString)
                '    .Nombre = Nothing
                '    .NombreMostrar = Nothing
                '    .TipoDato = Nothing
                '    .TipoFiltro = Nothing
                '    .ValorAsignado = Nothing
                '    .Visible = Nothing
                'End With

                diccionarioCaracteristicasnuevas_.Add(indice_, ReplicarCaracteristica(caracteristicas_, campo_.Atributo.ToString))

                indice_ += 1

            Next


            caracteristicas_ = diccionarioCaracteristicasnuevas_


        End Sub

        Private Function ExtractorEncabezados(ByRef queryDocument_ As QueryDocument, Optional ByVal tipooperacion As TipoOperacion = TipoOperacion.Consulta)


            'Dim filter = Builders(Of BsonDocument).Filter.And(
            '                       Builders(Of BsonDocument).Filter.In(Of String)("CustomerNo", customerList.Select(Function(m) m.CustomerNo)),
            '                       Builders(Of BsonDocument).Filter.Eq(Of Integer)("SupplierId", 410787))



            'Dim filter = Builders(Of BsonDocument).Filter.Eq(Of String)("name", "Neumann")

            'For Each item As BsonDocument In Collection.Find(filter).ToList

            'Next
        End Function

        Private Function ExtractorEncabezados(Optional ByVal tipooperacion As TipoOperacion = TipoOperacion.Consulta) As String

            Dim cadenacampos_ As String = Nothing

            Select Case tipooperacion

                Case LineaBaseCatalogos.TipoOperacion.Consulta

                    'Si no es la presentación generica de caracteristicas se procede a reorganizar en función de la estructura enviada
                    If _visualizacionCamposConfigurada = IOperacionesCatalogo.TiposVisualizacionCampos.RegenerarVistaEntorno Then

                        _cadenaencabezados = Nothing


                    ElseIf Not _visualizacionCamposConfigurada = IOperacionesCatalogo.TiposVisualizacionCampos.PresentarTodos Then

                        If Not _estructuraPresentacion Is Nothing Then

                            ReconfiguraCaracteristicas(_icaracteristicas, _estructuraPresentacion)

                        Else

                            _sistema.GsDialogo("No ha establecido la estructura para reorganizar la visualizacion de los datos",
                                               Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)

                        End If

                    End If

                    If _cadenaencabezados Is Nothing Then

                        For Each nombrecolumna_ As ICaracteristica In _icaracteristicas.Values

                            If nombrecolumna_.Llave = ICaracteristica.TipoLlave.Primaria Then

                                _envolturacampollave = nombrecolumna_.TipoDato

                            End If

                            'bautismen 08/09/2018, reduciendo la densidad de las consultas

                            If _visualizacionCamposConfigurada = IOperacionesCatalogo.TiposVisualizacionCampos.PresentarTodos Or
                                _visualizacionCamposConfigurada = IOperacionesCatalogo.TiposVisualizacionCampos.RegenerarVistaEntorno Then

                                If cadenacampos_ = Nothing Then

                                    cadenacampos_ = nombrecolumna_.Nombre &
                                                         " as '" & nombrecolumna_.NombreMostrar & "'"

                                Else

                                    cadenacampos_ = cadenacampos_ & "," & nombrecolumna_.Nombre &
                                                        " as '" & nombrecolumna_.NombreMostrar & "'"

                                End If

                            ElseIf _visualizacionCamposConfigurada = IOperacionesCatalogo.TiposVisualizacionCampos.PresentarVisibles Then

                                If nombrecolumna_.Visible = ICaracteristica.TiposVisible.Si Then

                                    If cadenacampos_ = Nothing Then

                                        cadenacampos_ = nombrecolumna_.Nombre &
                                                             " as '" & nombrecolumna_.NombreMostrar & "'"

                                    Else

                                        cadenacampos_ = cadenacampos_ & "," & nombrecolumna_.Nombre &
                                                            " as '" & nombrecolumna_.NombreMostrar & "'"

                                    End If

                                End If

                            ElseIf _visualizacionCamposConfigurada = IOperacionesCatalogo.TiposVisualizacionCampos.PresentarAcarreo Or
                                   _visualizacionCamposConfigurada = IOperacionesCatalogo.TiposVisualizacionCampos.PresentarImpresion Or
                                   _visualizacionCamposConfigurada = IOperacionesCatalogo.TiposVisualizacionCampos.PresentarVisiblesAcarreo Then

                                _sistema.GsDialogo("La modalidad de visualización ha sido implementada, confirme con Desarrollo de sistemas", Componentes.SistemaBase.GsDialogo.TipoDialogo.Err)

                            End If
                        Next

                        _cadenaencabezados = cadenacampos_

                    Else



                        '::::::::: Ajuste peligroso, Pedro BM, 06/09/2018, se forzará a reformular los encabezados.

                        'For Each nombrecolumna_ As ICaracteristica In _icaracteristicas.Values

                        '    If nombrecolumna_.Llave = ICaracteristica.TipoLlave.Primaria Then

                        '        _envolturacampollave = nombrecolumna_.TipoDato

                        '    End If

                        '    If nombrecolumna_.Visible = ICaracteristica.TiposVisible.No Then

                        '        Continue For

                        '    End If

                        '    If cadenacampos_ = Nothing Then

                        '        cadenacampos_ = nombrecolumna_.Nombre & _
                        '                             " as '" & nombrecolumna_.NombreMostrar & "'"

                        '    Else

                        '        cadenacampos_ = cadenacampos_ & "," & nombrecolumna_.Nombre & _
                        '                            " as '" & nombrecolumna_.NombreMostrar & "'"

                        '    End If

                        'Next

                        '::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

                        cadenacampos_ = _cadenaencabezados

                    End If

                Case LineaBaseCatalogos.TipoOperacion.Insercion

                    For Each nombrecolumna_ As ICaracteristica In _icaracteristicas.Values

                        If nombrecolumna_.PuedeInsertar = ICaracteristica.TiposRigorDatos.No Then

                            Continue For

                        End If

                        If cadenacampos_ = Nothing Then

                            cadenacampos_ = nombrecolumna_.Nombre

                        Else
                            cadenacampos_ = cadenacampos_ & "," & nombrecolumna_.Nombre

                        End If

                    Next

                Case LineaBaseCatalogos.TipoOperacion.InsercionDinamica

                    Dim indice_ As Int32 = 0

                    For Each nombrecolumna_ As ICaracteristica In _icaracteristicas.Values

                        If (nombrecolumna_.PuedeInsertar = ICaracteristica.TiposRigorDatos.No) Then

                            Continue For

                        End If

                        If cadenacampos_ = Nothing Then

                            cadenacampos_ = "'" & nombrecolumna_.ValorAsignado & "' as '" & nombrecolumna_.NombreMostrar & "' "

                        Else

                            cadenacampos_ = cadenacampos_ & "," & "'" & nombrecolumna_.ValorAsignado & "' as '" & nombrecolumna_.NombreMostrar & "' "

                        End If

                        indice_ += 1

                    Next

                Case LineaBaseCatalogos.TipoOperacion.InsercionDinamicaExposicion

                    For Each nombrecolumna_ As ICaracteristica In _icaracteristicas.Values

                        If (nombrecolumna_.PuedeInsertar = ICaracteristica.TiposRigorDatos.Si Or
                            nombrecolumna_.Visible = ICaracteristica.TiposVisible.Si) Then

                            If cadenacampos_ = Nothing Then

                                cadenacampos_ = "'" & nombrecolumna_.ValorAsignado & "' as '" & nombrecolumna_.NombreMostrar & "' "

                            Else

                                cadenacampos_ = cadenacampos_ & "," & "'" & nombrecolumna_.ValorAsignado & "' as '" & nombrecolumna_.NombreMostrar & "' "

                            End If

                        Else

                            Continue For

                        End If

                    Next

                Case Else

            End Select

            Return cadenacampos_

        End Function

        Private Function EnvolturaDato(ByVal dato_ As String, tipodato_ As ICaracteristica.TiposCaracteristica) As String
            Dim datoenvoltura As String = dato_

            Select Case tipodato_

                Case ICaracteristica.TiposCaracteristica.cString


                    dato_ = "char(39)+cast('" & dato_ & "' as nvarchar(max))+char(39)"

                Case ICaracteristica.TiposCaracteristica.cDateTime

                    dato_ = "char(39)+'" & dato_ & "'+char(39)"

                    'Case ICaracteristica.TiposCaracteristica.cString, _
                    '    ICaracteristica.TiposCaracteristica.cDateTime

                    '    dato_ = "char(39)+'" & dato_ & "'+char(39)"

                Case ICaracteristica.TiposCaracteristica.cBoolean,
                    ICaracteristica.TiposCaracteristica.cInt32,
                    ICaracteristica.TiposCaracteristica.cReal,
                    ICaracteristica.TiposCaracteristica.cUndefined

                    dato_ = "'" & dato_ & "'"
                Case Else

                    dato_ = "'" & dato_ & "'"

            End Select

            Return dato_

        End Function

        Private Function CreaAsignacionPorValor(ByVal tipooperacion_ As TipoOperacion) As String

            Dim valorescampos_ As String = Nothing

            Select Case tipooperacion_

                Case TipoOperacion.Insercion

                    'Si existe una petición de inserción múltiple le dará preferencia
                    If Not _listaRegistros Is Nothing And _listaRegistros.Count > 0 Then

                        Dim partidas_ As Int32 = _listaRegistros.Count

                        Dim iteracion_ As Int32 = 0

                        Dim nuevoRegistro_ As Boolean = False

                        For Each registroIOperaciones_ As RegistroIOperaciones In _listaRegistros

                            'For Each nombrecolumna_ As ICaracteristica In _icaracteristicas.Values
                            For Each nombrecolumna_ As ICaracteristica In registroIOperaciones_.Caracteristicas

                                If nombrecolumna_.PuedeInsertar = ICaracteristica.TiposRigorDatos.No Then

                                    Continue For

                                End If

                                'sólo para el comienzo del query, el primer valor
                                If valorescampos_ = Nothing Then

                                    If Not nombrecolumna_.ValorAsignado Is Nothing Then

                                        valorescampos_ = EnvolturaDato(nombrecolumna_.ValorAsignado,
                                                                       nombrecolumna_.TipoDato)

                                    Else

                                        If Not (nombrecolumna_.ValorDefault Is Nothing Or
                                            nombrecolumna_.ValorDefault = "") Then

                                            valorescampos_ = EnvolturaDato(nombrecolumna_.ValorDefault,
                                                                      nombrecolumna_.TipoDato)
                                        Else

                                            valorescampos_ = EnvolturaDato("NULL",
                                                      ICaracteristica.TiposCaracteristica.cInt32)
                                        End If


                                    End If

                                Else

                                    'Los siguientes valores
                                    If Not (nombrecolumna_.ValorAsignado Is Nothing Or
                                        nombrecolumna_.ValorAsignado = "") Then

                                        If nuevoRegistro_ = True Then ' nuevo

                                            valorescampos_ = valorescampos_ & Nothing & EnvolturaDato(nombrecolumna_.ValorAsignado,
                                                                                    nombrecolumna_.TipoDato)

                                            nuevoRegistro_ = False

                                        Else

                                            valorescampos_ = valorescampos_ & "+','+" & EnvolturaDato(nombrecolumna_.ValorAsignado,
                                                                                    nombrecolumna_.TipoDato)
                                        End If

                                    Else

                                        If nuevoRegistro_ = True Then ' nuevo

                                            If Not (nombrecolumna_.ValorDefault Is Nothing Or
                                                    nombrecolumna_.ValorDefault = "") Then

                                                valorescampos_ = valorescampos_ & Nothing & EnvolturaDato(nombrecolumna_.ValorDefault,
                                                                                        nombrecolumna_.TipoDato)
                                            Else

                                                valorescampos_ = valorescampos_ & Nothing & EnvolturaDato("NULL",
                                                                                        ICaracteristica.TiposCaracteristica.cInt32)
                                            End If

                                            nuevoRegistro_ = False

                                        Else

                                            If Not (nombrecolumna_.ValorDefault Is Nothing Or
                                                    nombrecolumna_.ValorDefault = "") Then

                                                valorescampos_ = valorescampos_ & "+','+" & EnvolturaDato(nombrecolumna_.ValorDefault,
                                                                                        nombrecolumna_.TipoDato)
                                            Else

                                                valorescampos_ = valorescampos_ & "+','+" & EnvolturaDato("NULL",
                                                                                        ICaracteristica.TiposCaracteristica.cInt32)

                                            End If

                                        End If


                                    End If

                                End If

                            Next

                            If partidas_ > 0 Then

                                If iteracion_ >= 0 And iteracion_ < partidas_ - 1 Then

                                    valorescampos_ = valorescampos_ & "+'),('+"

                                    nuevoRegistro_ = True

                                End If

                            End If

                            iteracion_ += 1

                        Next

                    Else

                        For Each nombrecolumna_ As ICaracteristica In _icaracteristicas.Values

                            If nombrecolumna_.PuedeInsertar = ICaracteristica.TiposRigorDatos.No Then

                                Continue For

                            End If

                            If valorescampos_ = Nothing Then

                                If Not nombrecolumna_.ValorAsignado Is Nothing Then

                                    valorescampos_ = EnvolturaDato(nombrecolumna_.ValorAsignado,
                                                                   nombrecolumna_.TipoDato)

                                Else

                                    If Not (nombrecolumna_.ValorDefault Is Nothing Or
                                        nombrecolumna_.ValorDefault = "") Then

                                        valorescampos_ = EnvolturaDato(nombrecolumna_.ValorDefault,
                                                                  nombrecolumna_.TipoDato)
                                    Else

                                        valorescampos_ = EnvolturaDato("NULL",
                                                  ICaracteristica.TiposCaracteristica.cInt32)
                                    End If


                                End If

                            Else

                                'Hay que evaluar esto, jamás entra a este else; arriba está determinado.
                                If Not (nombrecolumna_.ValorAsignado Is Nothing Or
                                    nombrecolumna_.ValorAsignado = "") Then

                                    valorescampos_ = valorescampos_ & "+','+" & EnvolturaDato(nombrecolumna_.ValorAsignado,
                                                                            nombrecolumna_.TipoDato)
                                Else

                                    If Not (nombrecolumna_.ValorDefault Is Nothing Or
                                        nombrecolumna_.ValorDefault = "") Then

                                        valorescampos_ = valorescampos_ & "+','+" & EnvolturaDato(nombrecolumna_.ValorDefault,
                                                                                nombrecolumna_.TipoDato)
                                    Else

                                        valorescampos_ = valorescampos_ & "+','+" & EnvolturaDato("NULL",
                                                                                ICaracteristica.TiposCaracteristica.cInt32)
                                    End If

                                End If

                            End If

                        Next

                    End If

                Case TipoOperacion.Modificacion 'NOT IMPLEMENTED, no suporta bulk aún

                    For Each nombrecolumna_ As ICaracteristica In _icaracteristicas.Values

                        If nombrecolumna_.PuedeModificar = ICaracteristica.TiposRigorDatos.No Then

                            Continue For

                        End If

                        If valorescampos_ = Nothing Then

                            If Not nombrecolumna_.ValorAsignado Is Nothing Then

                                valorescampos_ = "'" & nombrecolumna_.Nombre & "'+'='+" & EnvolturaDato(nombrecolumna_.ValorAsignado,
                                                               nombrecolumna_.TipoDato)

                            Else

                                If Not (nombrecolumna_.ValorDefault Is Nothing Or
                                  nombrecolumna_.ValorDefault = "") Then

                                    valorescampos_ = "'" & nombrecolumna_.Nombre & "'+'='+" & EnvolturaDato(nombrecolumna_.ValorDefault,
                                                                   nombrecolumna_.TipoDato)

                                Else

                                    valorescampos_ = "'" & nombrecolumna_.Nombre & "'+'='+" & EnvolturaDato("NULL",
                                                                   ICaracteristica.TiposCaracteristica.cInt32)
                                End If

                            End If

                        Else

                            If Not (nombrecolumna_.ValorAsignado Is Nothing Or
                                    nombrecolumna_.ValorAsignado = "") Then

                                valorescampos_ = valorescampos_ & "+','+'" & nombrecolumna_.Nombre & "'+'='+" & EnvolturaDato(nombrecolumna_.ValorAsignado,
                                                                                                          nombrecolumna_.TipoDato)
                            Else

                                If Not (nombrecolumna_.ValorDefault Is Nothing Or
                                    nombrecolumna_.ValorDefault = "") Then

                                    valorescampos_ = valorescampos_ & "+','+'" & nombrecolumna_.Nombre & "'+'='+" & EnvolturaDato(nombrecolumna_.ValorDefault,
                                                                                                              nombrecolumna_.TipoDato)
                                Else

                                    valorescampos_ = valorescampos_ & "+','+'" & nombrecolumna_.Nombre & "'+'='+" & EnvolturaDato("NULL",
                                                                                                              ICaracteristica.TiposCaracteristica.cInt32)

                                End If

                            End If

                        End If

                    Next

                Case Else

            End Select

            Return valorescampos_

        End Function

        Private Sub LimpiaAsignaciones()

            For Each campo_ As ICaracteristica In _icaracteristicas.Values

                If campo_.PuedeInsertar = ICaracteristica.TiposRigorDatos.No Then
                    Continue For
                End If

                campo_.ValorAsignado = Nothing

            Next

        End Sub

        Public Function ModificarProduccion(ByVal valorindice_ As String) As Boolean

            Dim valoresparamodificacion_ As String = Nothing

            Dim disparocapadedatos_ As String = Nothing

            Dim respuesta_ As Boolean = False

            '###################### Ajustamos la configuración de las conexiones  ###########################


            ConexionSingleton.Controlador = _origenDatos
            ConexionSingleton.SQLServerSingletonConexion.ObjetoDatos = _tipoConexion
            ConexionSingleton.SQLServerSingletonConexion.ControladorBaseDatos = _origenDatos
            ConexionSingleton.SQLServerSingletonConexion.RepositorioDatos = _objetoDatos


            '###################### Ajustamos la configuración de las conexiones  ###########################

            Select Case _tipoescritura

                Case IOperacionesCatalogo.TiposEscritura.Inmediata

                    Dim registroDatos_ As BsonDocument = New BsonDocument()

                    ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

                    If Not _iespaciotrabajo Is Nothing Then

                        If Not _iespaciotrabajo.MisCredenciales Is Nothing Then

                            ConexionSingleton.UsuarioCliente = _iespaciotrabajo.MisCredenciales.CredencialUsuario

                        End If

                    Else
                        ConexionSingleton.UsuarioCliente = _sistema.ConexionSingleton.UsuarioCliente
                    End If


                    ConexionSingleton.ModuloCliente = _operadorcatalogoconsultas

                    valoresparamodificacion_ = CreaAsignacionPorValor(TipoOperacion.Modificacion)

                    'Aquí verificaremos si valorIndice_ viene con nothing, en ese caso buscaremos si hay que aplicar lista de valores con sus tipos.

                    If Not valorindice_ Is Nothing Then

                        valorindice_ = EnvolturaDirecta(Trim(valorindice_), _envolturacampollave)

                    Else

                        If Not _keyValues Is Nothing Then

                            If Not OperadorCatalogoModificacion = "Sp000OperadorCatalogosModificacionesMaxMultipleValor" Then

                                'Por defecto deberá tomar este operador en caso de aplicación de multiples valores de llave primaria
                                OperadorCatalogoModificacion = "Sp000OperadorCatalogosModificacionesMaxMultipleValor"

                                Dim tagWatcher_ As New TagWatcher

                                'Warning
                                _sistema.Watcher.SetError(IRecursosSistemas.RecursosCMF.Gsol_BaseDatos_Operaciones_LineaBaseCatalogos64,
                                                          TagWatcher.ErrorTypes.C3_001_3000)


                            End If

                            Dim contadorLlaves_ As Int32 = 1

                            For Each valor_ As String In _keyValues

                                If contadorLlaves_ = 1 Then

                                    valorindice_ = EnvolturaDirecta(valor_, _envolturacampollave)

                                Else

                                    valorindice_ = valorindice_ & "," & EnvolturaDirecta(Trim(valor_), _envolturacampollave)

                                End If

                                contadorLlaves_ += 1

                            Next

                        End If

                    End If

                    If Not (valoresparamodificacion_ Is Nothing And valorindice_ Is Nothing) Then

                        Select Case ConexionSingleton.SQLServerSingletonConexion.ObjetoDatos

                            Case IConexiones.TipoConexion.MySQLCommand

                                valoresparamodificacion_ = valoresparamodificacion_.Replace("'", Chr(34)).Replace("+", ",").Replace("char(39)", Chr(34) & "'" & Chr(34))

                                disparocapadedatos_ = "  " &
                                            " set @valores = concat(" & valoresparamodificacion_ & "); " &
                                            " call " &
                                            _operadorcatalogomodificacion.Replace("dbo.", Nothing) &
                                            "('" & _tablaedicion & "'," &
                                            "@valores," &
                                            "'" & _identificadorcatalogo & "'," &
                                            "'" & valorindice_ & "');"
                                '"'" & EnvolturaDirecta(valorindice_, _envolturacampollave) & "');"

                            Case IConexiones.TipoConexion.SqlCommand, IConexiones.TipoConexion.Automatico

                                Dim cabeceraOperadorModificacion_ As String = Nothing

                                Select Case OperadorCatalogoModificacion

                                    Case "Sp000OperadorCatalogosModificaciones"
                                        cabeceraOperadorModificacion_ = " declare @valores as nvarchar(4000); "

                                    Case "Sp000OperadorCatalogosModificacionesMax"
                                        cabeceraOperadorModificacion_ = " declare @valores as nvarchar(MAX); "

                                    Case "Sp000OperadorCatalogosModificacionesMaxMultipleValor"
                                        cabeceraOperadorModificacion_ = " declare @valores as nvarchar(MAX); "

                                    Case Else
                                        cabeceraOperadorModificacion_ = " declare @valores as nvarchar(4000); "

                                End Select

                                disparocapadedatos_ = cabeceraOperadorModificacion_ &
                                        " set @valores = " & valoresparamodificacion_ & "; " &
                                        " exec " &
                                        _operadorcatalogomodificacion &
                                        " '" & _tablaedicion & "'," &
                                        "@valores," &
                                        "'" & _identificadorcatalogo & "'," &
                                        "'" & valorindice_ & "';"
                                '"'" & EnvolturaDirecta(valorindice_, _envolturacampollave) & "';"
                            Case Else

                        End Select

                        If Not _iespaciotrabajo Is Nothing Then

                            If Not _iespaciotrabajo.MisCredenciales Is Nothing Then

                                ConexionSingleton.UsuarioCliente = _iespaciotrabajo.MisCredenciales.CredencialUsuario

                            End If

                        Else

                            ConexionSingleton.UsuarioCliente = _sistema.ConexionSingleton.UsuarioCliente

                        End If

                        ConexionSingleton.ModuloCliente = _operadorcatalogoconsultas
                        ' bautismen()

                        _valorindiceseleccionado = "-1"

                        respuesta_ = ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(disparocapadedatos_)

                        Dim tipobitacora_ As monitoreo.IBitacoras.TiposBitacora =
                            monitoreo.IBitacoras.TiposBitacora.Informacion

                        Dim mensajebitacora_ As String = Nothing

                        If respuesta_ Then

                            mensajebitacora_ = "Modificó exitosamente."

                            tipobitacora_ = monitoreo.IBitacoras.TiposBitacora.Informacion

                            _valorindiceseleccionado = valorindice_

                            registroDatos_ = RefinarCaracteristicas()

                            Select Case _reflejarEn

                                Case IEnlaceDatos.DestinosParaReplicacion.NoSQLMongoDB

                                    If Not _valorindiceseleccionado Is Nothing AndAlso _valorindiceseleccionado <> "" Then

                                        Using conexionesNoSQL_ As IConexionesNoSQL = New ConexionesNoSQL

                                            Dim apuntador_ As New Dictionary(Of String, Object) From {{"Llave", _identificadorcatalogo}, {"Valor", valorindice_}}

                                            ' JCCS - 2021/10/22 - Para no afectar _tipoOperacionSQL que existe se crea una variable para controldar esta petición
                                            Dim tipoOperacionSQL_ As IOperacionesCatalogo.TiposOperacionSQL

                                            If _keyValues IsNot Nothing Then

                                                If _keyValues.Count > 1 Then

                                                    tipoOperacionSQL_ = IOperacionesCatalogo.TiposOperacionSQL.UpdateManyAsync

                                                Else

                                                    tipoOperacionSQL_ = IOperacionesCatalogo.TiposOperacionSQL.UpdateOneAsync

                                                End If

                                            Else

                                                tipoOperacionSQL_ = IOperacionesCatalogo.TiposOperacionSQL.UpdateOneAsync

                                            End If

                                            conexionesNoSQL_.EjecutaSentencia(_operadorcatalogoconsultas, registroDatos_, tipoOperacionSQL_, apuntador_)

                                        End Using

                                    Else

                                        _sistema.Watcher.SetError(IRecursosSistemas.RecursosCMF.Gsol_BaseDatos_Operaciones_LineaBaseCatalogos64,
                                                                  TagWatcher.ErrorTypes.C3_001_3008)

                                    End If

                                Case Else

                                    'NOT IMPLEMENTED

                            End Select

                        Else

                            mensajebitacora_ = "Error al modificar"

                            tipobitacora_ = monitoreo.IBitacoras.TiposBitacora.Errores

                        End If

                        LimpiaAsignaciones()

                        Dim nombreUsuario_ As String = "Sin sesión completa"

                        If Not EspacioTrabajo Is Nothing Then


                            If Not EspacioTrabajo.MisCredenciales Is Nothing Then
                                nombreUsuario_ = EspacioTrabajo.MisCredenciales.NombreAutenticacion

                                _sistema.Log(mensajebitacora_,
                                tipobitacora_,
                                monitoreo.IBitacoras.TiposSucesos.Modificar,
                                _iespaciotrabajo.MisCredenciales,
                                0,
                                valoresparamodificacion_.Replace("'", Nothing).Replace(Chr(34), Nothing).Replace("char(39)", Nothing).Replace("+", Nothing),
                                disparocapadedatos_.Replace("'", Nothing).Replace(Chr(34), Nothing).Replace("char(39)", Nothing).Replace("+", Nothing),
                                OperadorCatalogoConsultas,
                                nombreUsuario_
                                )

                            Else

                                'PENDIENTE IMPLEMENTAR
                                '_sistema.Log(mensajebitacora_, _
                                'tipobitacora_, _
                                'monitoreo.IBitacoras.TiposSucesos.Modificar)

                            End If

                        Else
                            'PENDIENTE IMPLEMENTAR
                            '_sistema.Log(mensajebitacora_, _
                            'tipobitacora_, _
                            'monitoreo.IBitacoras.TiposSucesos.Modificar)

                        End If

                    Else

                        respuesta_ = False

                    End If

                Case IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda

                    Dim foundRows() As Data.DataRow

                    foundRows = EncuentraRegistro(_registrostemporalesDataTable, "Id", valorindice_)
                    '_registrostemporalesDataTable.Select("Id=" & valorindice_)

                    Try

                        For Each nombrecolumna_ As ICaracteristica In _icaracteristicas.Values

                            Dim valorcampo_ As String = Nothing

                            If nombrecolumna_.PuedeModificar = ICaracteristica.TiposRigorDatos.Si Or
                                nombrecolumna_.Visible = ICaracteristica.TiposVisible.Si Or
                                nombrecolumna_.Visible = ICaracteristica.TiposVisible.Acarreo Then

                                If Not nombrecolumna_.ValorAsignado Is Nothing Then

                                    valorcampo_ = nombrecolumna_.ValorAsignado

                                Else

                                    valorcampo_ = nombrecolumna_.ValorDefault

                                End If

                                foundRows(0)(nombrecolumna_.NombreMostrar) = valorcampo_

                            Else

                                Continue For

                            End If

                        Next

                        respuesta_ = True

                    Catch ex As Exception

                        respuesta_ = False

                    End Try

                Case IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia

                    Dim foundRows() As Data.DataRow

                    'foundRows = _registrostemporalesDataTable.Select("Id=" & valorindice_)

                    foundRows = EncuentraRegistro(_registrostemporalesDataTable, "Id", valorindice_) '_registrostemporalesDataTable.Select("Id=" & valorindice_)

                    Try

                        For Each nombrecolumna_ As ICaracteristica In _icaracteristicas.Values

                            Dim valorcampo_ As String = Nothing

                            If nombrecolumna_.PuedeModificar = ICaracteristica.TiposRigorDatos.Si Or
                                nombrecolumna_.Visible = ICaracteristica.TiposVisible.Si Or
                                nombrecolumna_.Visible = ICaracteristica.TiposVisible.Acarreo Then

                                If Not nombrecolumna_.ValorAsignado Is Nothing Then

                                    valorcampo_ = nombrecolumna_.ValorAsignado

                                Else

                                    valorcampo_ = nombrecolumna_.ValorDefault

                                End If

                                If foundRows.Length > 0 Then

                                    foundRows(0)(nombrecolumna_.NombreMostrar) = valorcampo_

                                Else

                                    For Each registro_ As DataRow In _registrostemporalesDataTable.Rows

                                        If registro_.Item("Id").ToString = valorindice_ Then

                                            registro_.Item(nombrecolumna_.NombreMostrar) = valorcampo_

                                        End If

                                    Next

                                End If

                            Else

                                Continue For

                            End If

                        Next

                        respuesta_ = True

                    Catch ex As Exception

                        respuesta_ = False

                    End Try

                Case Else

            End Select

            Return respuesta_

        End Function

        Public Function EncuentraRegistro(ByVal tabla_ As DataTable, ByVal campo_ As String, ByVal valor_ As String) As Data.DataRow()
            Dim registro_(3) As DataRow

            For Each fila_ As DataRow In tabla_.Rows

                If fila_(campo_) = valor_ Then

                    registro_(0) = fila_

                    Return registro_

                End If

            Next

            Return registro_

        End Function

        Private Sub EliminaRegistroGridView(ByRef gridView_ As DataGridView,
                                       ByVal campo_ As String,
                                       ByVal valor_ As String)


            Dim coleccionFilas_ As New List(Of DataGridViewRow)


            For Each fila_ As DataGridViewRow In gridView_.Rows

                If fila_.Cells(campo_).Value = valor_ Then

                    coleccionFilas_.Add(fila_)

                End If

            Next

            For Each item_ As DataGridViewRow In coleccionFilas_

                gridView_.Rows.Remove(item_)

            Next

        End Sub

        Public Function InsertarProduccion(Optional ByVal identificadorempresa_ As String = "-1",
                                           Optional ByVal grabarPlanEjecucion_ As Boolean = False) As Boolean

            Dim valoresparainsercion_ As String = Nothing

            Dim camposparainsercion_ As String = Nothing

            Dim disparocapadedatos_ As String = Nothing

            Dim respuesta_ As Boolean = False

            ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

            '###################### Ajustamos la configuración de las conexiones  ###########################


            ConexionSingleton.Controlador = _origenDatos
            ConexionSingleton.SQLServerSingletonConexion.ObjetoDatos = _tipoConexion
            ConexionSingleton.SQLServerSingletonConexion.ControladorBaseDatos = _origenDatos
            ConexionSingleton.SQLServerSingletonConexion.RepositorioDatos = _objetoDatos


            '###################### Ajustamos la configuración de las conexiones  ###########################

            If Not _iespaciotrabajo Is Nothing Then

                If Not _iespaciotrabajo.MisCredenciales Is Nothing Then

                    ConexionSingleton.UsuarioCliente = _iespaciotrabajo.MisCredenciales.CredencialUsuario

                End If

            Else
                ConexionSingleton.UsuarioCliente = _sistema.ConexionSingleton.UsuarioCliente
            End If


            ConexionSingleton.ModuloCliente = _operadorcatalogoconsultas

            Select Case _tipoescritura

                Case IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda

                    If (PraparaTablaTemporal()) Then

                        Dim registro_ As DataRow = _registrostemporalesDataTable.NewRow

                        registro_ = ExtractorEncabezadosDataTable(TipoOperacion.InsercionDinamica)

                        _registrostemporalesDataTable.Rows.Add(registro_)

                        'If _registrostemporales.Count = 0 Then

                        '    _registrostemporales.Add(1, "OK")

                        'End If

                        If _registrostemporales Is Nothing Then

                            _registrostemporales = New Dictionary(Of Int32, String)

                            _registrostemporales.Add(1, "OK")

                        Else
                            If _registrostemporales.Count >= 0 Then

                                If Not _registrostemporales.ContainsKey(1) Then
                                    _registrostemporales.Add(1, "OK")
                                End If



                            End If

                        End If

                        disparocapadedatos_ = Nothing

                        respuesta_ = True
                    Else

                        respuesta_ = False

                    End If

                    If grabarPlanEjecucion_ Then

                        valoresparainsercion_ = CreaAsignacionPorValor(TipoOperacion.Insercion)

                        camposparainsercion_ = ExtractorEncabezados(TipoOperacion.Insercion)

                        If Not (valoresparainsercion_ Is Nothing And
                            camposparainsercion_ Is Nothing) Then
                            Dim llaveSQL_ As String = Nothing
                            Dim idLlamante_ As String = Nothing

                            If Not _indiceTablaTemporalLlamante Is Nothing And _indiceTablaTemporalLlamante <> "0" Then

                                idLlamante_ = _indiceTablaTemporalLlamante & "."

                            End If

                            llaveSQL_ = IDObjectoTransaccional & "." & IDNivelTransaccional & "." & idLlamante_

                            Select Case ConexionSingleton.SQLServerSingletonConexion.ObjetoDatos
                                Case IConexiones.TipoConexion.MySQLCommand

                                    valoresparainsercion_ = valoresparainsercion_.Replace("'", Chr(34)).Replace("+", ",").Replace("char(39)", Chr(34) & "'" & Chr(34))


                                    disparocapadedatos_ = "  " &
                                                " set @valores = concat(" & valoresparainsercion_ & "); " &
                                                " call " &
                                                _operadorcatalogoinsercion.Replace("dbo.", Nothing) &
                                                "('" & _tablaedicion & "'," &
                                                "'" & _identificadorcatalogo & "'," &
                                                "'" & camposparainsercion_ & "'," &
                                                "@valores);"

                                Case IConexiones.TipoConexion.SqlCommand, IConexiones.TipoConexion.Automatico

                                    Dim cabeceraOperadorInsercion_ As String = Nothing

                                    Select Case OperadorCatalogoInsercion

                                        Case "Sp000OperadorCatalogosInserciones"
                                            cabeceraOperadorInsercion_ = " declare @valores as nvarchar(4000); "

                                        Case "Sp000OperadorCatalogosInsercionesMax"
                                            cabeceraOperadorInsercion_ = " declare @valores as nvarchar(MAX); "

                                        Case Else
                                            cabeceraOperadorInsercion_ = " declare @valores as nvarchar(4000); "

                                    End Select


                                    disparocapadedatos_ = cabeceraOperadorInsercion_ & Chr(13) &
                                                          " set @valores = " & valoresparainsercion_ & "; " & Chr(13) &
                                                          " exec " & Chr(13) &
                                                          _operadorcatalogoinsercion & Chr(13) &
                                                          " '" & _tablaedicion & "'," & Chr(13) &
                                                          "'" & _identificadorcatalogo & "'," & Chr(13) &
                                                          "'" & camposparainsercion_ & "'," & Chr(13) &
                                                          "@valores;"

                                Case Else

                            End Select

                            LimpiaAsignaciones()

                            _valorindiceseleccionado = "-1"

                            If _sqlTransaccionList.ContainsKey(llaveSQL_) Then

                                _sqlTransaccionList.Remove(llaveSQL_)

                            End If

                            _sqlTransaccionList.Add(llaveSQL_, disparocapadedatos_)

                            respuesta_ = True

                        Else

                            respuesta_ = False

                        End If

                    End If

                    Return respuesta_

                Case IOperacionesCatalogo.TiposEscritura.Inmediata

                    valoresparainsercion_ = CreaAsignacionPorValor(TipoOperacion.Insercion)

                    camposparainsercion_ = ExtractorEncabezados(TipoOperacion.Insercion)

                    '-------------temporal MONGO
                    Dim registroDatos_ As BsonDocument = New BsonDocument()
                    '-------------temporal MONGO

                    If Not (valoresparainsercion_ Is Nothing And
                        camposparainsercion_ Is Nothing) Then

                        Select Case ConexionSingleton.SQLServerSingletonConexion.ObjetoDatos
                            Case IConexiones.TipoConexion.MySQLCommand

                                valoresparainsercion_ = valoresparainsercion_.Replace("'", Chr(34)).Replace("+", ",").Replace("char(39)", Chr(34) & "'" & Chr(34))


                                disparocapadedatos_ = "  " &
                                            " set @valores = concat(" & valoresparainsercion_ & "); " &
                                            " call " &
                                            _operadorcatalogoinsercion.Replace("dbo.", Nothing) &
                                            "('" & _tablaedicion & "'," &
                                            "'" & _identificadorcatalogo & "'," &
                                            "'" & camposparainsercion_ & "'," &
                                            "@valores);"

                            Case IConexiones.TipoConexion.SqlCommand, IConexiones.TipoConexion.Automatico

                                Dim cabeceraOperadorInsercion_ As String = Nothing

                                Select Case OperadorCatalogoInsercion

                                    Case "Sp000OperadorCatalogosInserciones"
                                        cabeceraOperadorInsercion_ = " declare @valores as nvarchar(4000); "

                                    Case "Sp000OperadorCatalogosInsercionesMax"
                                        cabeceraOperadorInsercion_ = " declare @valores as nvarchar(MAX); "

                                    Case Else
                                        cabeceraOperadorInsercion_ = " declare @valores as nvarchar(4000); "

                                End Select

                                disparocapadedatos_ = cabeceraOperadorInsercion_ &
                                            " set @valores = " & valoresparainsercion_ & "; " &
                                            " exec " &
                                            _operadorcatalogoinsercion &
                                            " '" & _tablaedicion & "'," &
                                            "'" & _identificadorcatalogo & "'," &
                                            "'" & camposparainsercion_ & "'," &
                                            "@valores;"

                                '#################### INSERCION SQL SERVER ##############################

                                _valorindiceseleccionado = "-1"

                                If Not _iespaciotrabajo Is Nothing Then

                                    If Not _iespaciotrabajo.MisCredenciales Is Nothing Then

                                        ConexionSingleton.UsuarioCliente = _iespaciotrabajo.MisCredenciales.CredencialUsuario

                                    End If

                                Else

                                    ConexionSingleton.UsuarioCliente = _sistema.ConexionSingleton.UsuarioCliente

                                End If

                                ConexionSingleton.ModuloCliente = _operadorcatalogoconsultas

                                respuesta_ = ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(disparocapadedatos_)

                                Dim tipobitacora_ As monitoreo.IBitacoras.TiposBitacora =
                                    monitoreo.IBitacoras.TiposBitacora.Informacion

                                Dim mensajebitacora_ As String = Nothing

                                If respuesta_ Then

                                    mensajebitacora_ = "Inserto exitosamente."

                                    tipobitacora_ = monitoreo.IBitacoras.TiposBitacora.Informacion

                                    '_valorindiceseleccionado = ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)(0).ToString

                                    With ConexionSingleton.SQLServerSingletonConexion

                                        If .DataSetReciente IsNot Nothing Then

                                            With .DataSetReciente

                                                If .Tables IsNot Nothing And .Tables.Count >= 1 Then

                                                    With .Tables(0)

                                                        If .Rows.Count > 1 Then

                                                            For Each row_ As DataRow In .Rows

                                                                _valoresIndiceInsertados.Add(row_(0))

                                                            Next

                                                        End If

                                                        _valorindiceseleccionado = .Rows(.Rows.Count - 1)(0).ToString

                                                    End With

                                                End If

                                            End With

                                        End If

                                    End With

                                    Select Case _reflejarEn

                                        Case IEnlaceDatos.DestinosParaReplicacion.NoSQLMongoDB

                                            If Not _valorindiceseleccionado Is Nothing AndAlso _valorindiceseleccionado <> "" Then

                                                Using conexionesNoSQL_ As IConexionesNoSQL = New ConexionesNoSQL

                                                    'Incluimos el indica de RBMS
                                                    'EnvolturaDirectaTipo(_identificadorcatalogo, _valorindiceseleccionado, ICaracteristica.TiposCaracteristica.cInt32, registroDatos_)

                                                    ' JCCS - 2021/10/22 - Para no afectar _tipoOperacionSQL que existe se crea una variable para controldar esta petición
                                                    Dim tipoOperacionSQL_ As IOperacionesCatalogo.TiposOperacionSQL

                                                    If Not _listaRegistros Is Nothing And _listaRegistros.Count > 0 Then

                                                        Dim inserManyRegistroDatos_ As New List(Of BsonDocument)

                                                        inserManyRegistroDatos_ = ArmaBulkParaTransaccion()

                                                        tipoOperacionSQL_ = IOperacionesCatalogo.TiposOperacionSQL.InsertManyAsync

                                                        If conexionesNoSQL_.EjecutaSentencia(_operadorcatalogoconsultas, inserManyRegistroDatos_, tipoOperacionSQL_) Then

                                                            'MsgBox("Hola replica de mongo, hicimos la logración...:'V")

                                                        End If

                                                    Else

                                                        Dim insertOneRegistroDatos_ As New BsonDocument

                                                        insertOneRegistroDatos_ = RefinarCaracteristicas()

                                                        tipoOperacionSQL_ = IOperacionesCatalogo.TiposOperacionSQL.InsertOneAsync

                                                        If conexionesNoSQL_.EjecutaSentencia(_operadorcatalogoconsultas, insertOneRegistroDatos_, tipoOperacionSQL_) Then

                                                            'MsgBox("Hola replica de mongo, hicimos la logración...:'V")

                                                        End If

                                                    End If

                                                End Using

                                            Else

                                                _sistema.Watcher.SetError(IRecursosSistemas.RecursosCMF.Gsol_BaseDatos_Operaciones_LineaBaseCatalogos64,
                                                                          TagWatcher.ErrorTypes.C3_001_3008)

                                            End If

                                        Case Else

                                            'NOT IMPLEMENTED

                                    End Select

                                    '-------------temporal MONGO

                                Else

                                    mensajebitacora_ = "Error al Insertar"

                                    tipobitacora_ = monitoreo.IBitacoras.TiposBitacora.Errores

                                End If

                                LimpiaAsignaciones()

                                Dim nombreUsuario_ As String = "Sin sesión completa"

                                If Not EspacioTrabajo.MisCredenciales Is Nothing Then
                                    nombreUsuario_ = EspacioTrabajo.MisCredenciales.NombreAutenticacion

                                    _sistema.Log(mensajebitacora_,
                                                 tipobitacora_,
                                                 monitoreo.IBitacoras.TiposSucesos.Insertar,
                                                _iespaciotrabajo.MisCredenciales,
                                                 0,
                                                 valoresparainsercion_.Replace("'", Nothing).Replace(Chr(34), Nothing).Replace("char(39)", Nothing).Replace("+", Nothing),
                                                 disparocapadedatos_.Replace("'", Nothing).Replace(Chr(34), Nothing).Replace("char(39)", Nothing).Replace("+", Nothing), OperadorCatalogoConsultas, EspacioTrabajo.MisCredenciales.NombreAutenticacion)


                                End If

                                '#################### INSERCION SQL SERVER ##############################

                            Case IConexiones.TipoConexion.DirectMongoDB

                                For Each caracteristica_ As ICaracteristica In _icaracteristicas.Values

                                    EnvolturaDirectaTipo(caracteristica_.Nombre, caracteristica_.ValorAsignado, caracteristica_.TipoDato, registroDatos_)

                                Next


                                '#################### INSERCION EN MONGODB ##############################

                                LimpiaAsignaciones()

                                _valorindiceseleccionado = "-1"

                                'If Not _iespaciotrabajo Is Nothing Then

                                '    If Not _iespaciotrabajo.MisCredenciales Is Nothing Then

                                '        ConexionSingleton.UsuarioCliente = _iespaciotrabajo.MisCredenciales.CredencialUsuario

                                '    End If

                                'Else
                                '    ConexionSingleton.UsuarioCliente = _sistema.ConexionSingleton.UsuarioCliente
                                'End If

                                ConexionSingleton.ModuloCliente = _operadorcatalogoconsultas

                                'respuesta_ = ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(disparocapadedatos_)

                                'Dim tipobitacora_ As monitoreo.IBitacoras.TiposBitacora =
                                '    monitoreo.IBitacoras.TiposBitacora.Informacion

                                'Dim mensajebitacora_ As String = Nothing

                                ' If respuesta_ Then

                                'mensajebitacora_ = "Inserto exitosamente."

                                '    tipobitacora_ = monitoreo.IBitacoras.TiposBitacora.Informacion

                                '    _valorindiceseleccionado =
                                '        ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)(0).ToString


                                '-------------temporal MONGO

                                ' Select Case _reflejarEn

                                'Case IEnlaceDatos.DestinosParaReplicacion.NoSQLMongoDB

                                'If Not _valorindiceseleccionado Is Nothing AndAlso _valorindiceseleccionado <> "" Then

                                Using conexionesNoSQL_ As IConexionesNoSQL = New ConexionesNoSQL

                                    ' JCCS - 2021/10/22 - Para no afectar _tipoOperacionSQL que existe se crea una variable para controldar esta petición
                                    Dim tipoOperacionSQL_ As IOperacionesCatalogo.TiposOperacionSQL = IOperacionesCatalogo.TiposOperacionSQL.InsertOneAsync

                                    'If Not conexionesNoSQL_.EjecutaSentencia(_operadorcatalogoconsultas, registroDatos_, tipoOperacionSQL_) = True Then
                                    If Not conexionesNoSQL_.EjecutaSentencia(_operadorcatalogoconsultas, registroDatos_, tipoOperacionSQL_) = True Then

                                        _sistema.Watcher.SetError(IRecursosSistemas.RecursosCMF.Gsol_BaseDatos_Operaciones_LineaBaseCatalogos64,
                                                                      TagWatcher.ErrorTypes.C3_001_3008)

                                    Else

                                        respuesta_ = True

                                    End If


                                End Using

                                'Else



                                'End If

                                '    Case Else

                                '        'NOT IMPLEMENTED

                                'End Select

                                '-------------temporal MONGO

                                'Else

                                '    mensajebitacora_ = "Error al Insertar"

                                '    tipobitacora_ = monitoreo.IBitacoras.TiposBitacora.Errores

                                'End If

                                'Dim nombreUsuario_ As String = "Sin sesión completa"

                                'If Not EspacioTrabajo.MisCredenciales Is Nothing Then
                                '    nombreUsuario_ = EspacioTrabajo.MisCredenciales.NombreAutenticacion

                                '    _sistema.Log(mensajebitacora_,
                                '                 tipobitacora_,
                                '                 monitoreo.IBitacoras.TiposSucesos.Insertar,
                                '                _iespaciotrabajo.MisCredenciales,
                                '                 0,
                                '                 valoresparainsercion_.Replace("'", Nothing).Replace(Chr(34), Nothing).Replace("char(39)", Nothing).Replace("+", Nothing),
                                '                 disparocapadedatos_.Replace("'", Nothing).Replace(Chr(34), Nothing).Replace("char(39)", Nothing).Replace("+", Nothing), OperadorCatalogoConsultas, EspacioTrabajo.MisCredenciales.NombreAutenticacion)


                                'End If

                                '#################### INSERCION MONGODB ##############################



                            Case Else

                        End Select

                        Return respuesta_

                    Else

                        Return False

                    End If

                Case IOperacionesCatalogo.TiposEscritura.MemoriaIntermediaBeta

                    camposparainsercion_ = ExtractorEncabezados(TipoOperacion.InsercionDinamica)

                    If Not (camposparainsercion_ Is Nothing) Then

                        _registrostemporales.Add(_registrostemporales.Count, camposparainsercion_)

                        camposparainsercion_ = ExtractorEncabezados(TipoOperacion.InsercionDinamicaExposicion)

                        _registrostemporalespresentacion.Add(_registrostemporalespresentacion.Count, camposparainsercion_)

                        disparocapadedatos_ = Nothing

                        Return True

                    Else

                        Return False

                    End If

                Case IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia

                    If (PraparaTablaTemporal()) Then

                        Dim registro_ As DataRow = _registrostemporalesDataTable.NewRow

                        registro_ = ExtractorEncabezadosDataTable(TipoOperacion.InsercionDinamica)

                        _registrostemporalesDataTable.Rows.Add(registro_)

                        If _registrostemporales Is Nothing Then

                            _registrostemporales = New Dictionary(Of Int32, String)

                            _registrostemporales.Add(1, "OK")

                        Else
                            If _registrostemporales.Count >= 0 Then

                                If Not _registrostemporales.ContainsKey(1) Then

                                    _registrostemporales.Add(1, "OK")

                                End If

                            End If

                        End If

                        disparocapadedatos_ = Nothing

                        Return True

                    Else

                        Return False

                    End If

                Case Else

                    Return False

            End Select

        End Function

        Private Function PraparaTablaTemporal() As Boolean

            Try

                If _registrostemporalesDataTable.Columns.Count = 0 Then

                    If Not _icaracteristicas Is Nothing Then

                        _registrostemporalesDataTable.Clear()

                        _registrostemporalesDataTable.Columns.Add("Id")

                        If Not _tipoescritura = IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda Then

                            _registrostemporalesDataTable.Columns(0).AutoIncrement = True

                            '_registrostemporalesDataTable.Columns(0).AutoIncrementStep = True

                            _registrostemporalesDataTable.Columns(0).Unique = True

                        End If

                        For Each nombrecolumna_ As ICaracteristica In _icaracteristicas.Values

                            If (nombrecolumna_.Visible = ICaracteristica.TiposVisible.Si Or
                           nombrecolumna_.Visible = ICaracteristica.TiposVisible.Acarreo Or
                           nombrecolumna_.PuedeInsertar = ICaracteristica.TiposRigorDatos.Si) Then

                                _registrostemporalesDataTable.Columns.Add(nombrecolumna_.NombreMostrar)

                            Else

                                Continue For

                            End If

                        Next

                        Return True

                    End If

                Else

                    Return True

                End If

            Catch ex As Exception

                Return False

            End Try

            Return False

        End Function

        Private Function ExtractorEncabezadosDataTable(ByVal tipooperacion_ As TipoOperacion) As DataRow

            Dim _registro As DataRow = _registrostemporalesDataTable.NewRow

            If _tipoescritura = IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda Then

                _registro("Id") = _indiceTablaTemporal

                _indiceTablaTemporal += 1

            End If

            Select Case tipooperacion_

                Case LineaBaseCatalogos.TipoOperacion.InsercionDinamica

                    For Each nombrecolumna_ As ICaracteristica In _icaracteristicas.Values

                        If (nombrecolumna_.Visible = ICaracteristica.TiposVisible.Si Or
                          nombrecolumna_.Visible = ICaracteristica.TiposVisible.Acarreo Or
                          nombrecolumna_.PuedeInsertar = ICaracteristica.TiposRigorDatos.Si) Then

                            '_registro(nombrecolumna_.NombreMostrar) = nombrecolumna_.ValorAsignado

                            If Not (nombrecolumna_.ValorAsignado Is Nothing And
                                    Trim(nombrecolumna_.ValorAsignado) = "") Then

                                _registro(nombrecolumna_.NombreMostrar) = nombrecolumna_.ValorAsignado

                            Else

                                _registro(nombrecolumna_.NombreMostrar) = DBNull.Value

                            End If

                        Else

                            Continue For

                        End If

                    Next

                Case Else

            End Select

            Return _registro

        End Function

        Private Sub EnvolturaDirectaTipo(ByVal nombreCaracteristica_ As String,
                                         ByVal valor_ As String,
                                         ByVal tipodato_ As ICaracteristica.TiposCaracteristica,
                                  ByRef objetoBSONDocument_ As BsonDocument)

            If nombreCaracteristica_ Is Nothing Then

                Exit Sub

            End If

            With objetoBSONDocument_

                Select Case tipodato_

                    Case ICaracteristica.TiposCaracteristica.cDateTime

                        If IsDate(valor_) Then

                            Dim tipoFecha_ As Date = Now

                            tipoFecha_ = valor_

                            .Add(nombreCaracteristica_, tipoFecha_)

                        End If

                    Case ICaracteristica.TiposCaracteristica.cString

                        If Not valor_ Is Nothing Then
                            .Add(nombreCaracteristica_, valor_)
                        Else
                            .Add(nombreCaracteristica_, "")
                        End If

                    Case ICaracteristica.TiposCaracteristica.cBoolean

                        Dim tipoBoolean_ As Boolean = False
                        'NOT IMPLEMENTED
                        .Add(nombreCaracteristica_, valor_)

                    Case ICaracteristica.TiposCaracteristica.cInt32

                        If IsNumeric(valor_) Then

                            Dim tipoEntero_ As Int32 = Convert.ToInt32(valor_)

                            .Add(nombreCaracteristica_, tipoEntero_)

                        Else
                            'NOT IMPLEMENTED
                        End If

                    Case ICaracteristica.TiposCaracteristica.cReal

                        If IsNumeric(valor_) Then

                            Dim tipoDoble_ As Double = Convert.ToDouble(valor_)

                            .Add(nombreCaracteristica_, tipoDoble_)

                        Else
                            'NOT IMPLEMENTED
                        End If

                    Case ICaracteristica.TiposCaracteristica.cUndefined

                        .Add(nombreCaracteristica_, valor_)

                    Case Else

                        .Add(nombreCaracteristica_, valor_)

                End Select

            End With


        End Sub

        Private Function EnvolturaDirecta(ByVal valor_ As String,
                                          ByVal tipodato_ As ICaracteristica.TiposCaracteristica,
                                          Optional ByVal token_ As Char = Chr(34))
            Select Case tipodato_

                Case ICaracteristica.TiposCaracteristica.cDateTime, ICaracteristica.TiposCaracteristica.cString

                    Return token_ & valor_ & token_

                Case Else

                    Return valor_

            End Select

        End Function

        'Public Function BorrarProduccionX(ByVal indice_ As String, _
        '                               ByVal identificadorempresa_ As String) As Boolean

        '    Dim respuesta_ As Boolean = Nothing


        '    'MsgBox("kk")


        '    Select Case _tipoescritura

        '        Case IOperacionesCatalogo.TiposEscritura.Inmediata

        '            ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

        '            Dim sentencia_ As String = Nothing


        '            Select Case ConexionSingleton.SQLServerSingletonConexion.ObjetoDatos
        '                Case IConexiones.TipoConexion.MySQLCommand

        '                    sentencia_ = "call " & _
        '                                _operadorcatalogoborrado.Replace("dbo.", Nothing) & _
        '                                "('" & _tablaedicion & "'," & _
        '                                "'" & _identificadorcatalogo & "'," & _
        '                                "'" & EnvolturaDirecta(indice_, _
        '                                      _envolturacampollave) & "'," & _
        '                                "'" & identificadorempresa_ & "');"

        '                Case IConexiones.TipoConexion.SqlCommand

        '                    sentencia_ = "exec " & _
        '                                _operadorcatalogoborrado & _
        '                                "'" & _tablaedicion & "'," & _
        '                                "'" & _identificadorcatalogo & "'," & _
        '                                "'" & EnvolturaDirecta(indice_, _
        '                                      _envolturacampollave) & "'," & _
        '                                "'" & identificadorempresa_ & "'"

        '                Case Else

        '            End Select



        '            respuesta_ = ConexionSingleton.SQLServerSingletonConexion.EjecutaSentencia(sentencia_)

        '            'Registro de la bitacora
        '            Dim tipobitacora_ As monitoreo.IBitacoras.TiposBitacora = _
        '                monitoreo.IBitacoras.TiposBitacora.Informacion
        '            Dim mensajebitacora_ As String = Nothing

        '            If respuesta_ Then
        '                mensajebitacora_ = "Borró exitosamente."
        '                tipobitacora_ = monitoreo.IBitacoras.TiposBitacora.Informacion
        '            Else
        '                mensajebitacora_ = "Error al borrar"
        '                tipobitacora_ = monitoreo.IBitacoras.TiposBitacora.Errores
        '            End If

        '            _sistema.Log(mensajebitacora_, _
        '                        tipobitacora_, _
        '                        monitoreo.IBitacoras.TiposSucesos.Modificar, _
        '                        _iespaciotrabajo.MisCredenciales, _
        '                        0, _
        '                        sentencia_.Replace("'", Nothing).Replace(Chr(34), Nothing).Replace("char(39)", Nothing).Replace("+", Nothing), _
        '                        sentencia_.Replace("'", Nothing).Replace(Chr(34), Nothing).Replace("char(39)", Nothing).Replace("+", Nothing))


        '        Case IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia

        '            'camposparainsercion_ = ExtractorEncabezados(TipoOperacion.InsercionDinamicaExposicion)

        '            If Not (indice_ Is Nothing) Then

        '                '_registrostemporales.ContainsKey(indice_)

        '                'MsgBox("rem1")


        '                '_registrostemporales.Remove(indice_)

        '                '_sistema.GsDialogo("Esta función no ha sido implementada")


        '                Dim foundRows() As Data.DataRow

        '                foundRows = RegistrosTemporalesDataTable.Select("Id=" & indice_)


        '                RegistrosTemporalesDataTable.Rows.Remove(foundRows(0))


        '                'Ingresa el registro reciente en la pila
        '                '_registrostemporales.Add(_registrostemporales.Count + 1, camposparainsercion_)

        '                'limpiamos la consulta dinamica actual
        '                'disparocapadedatos_ = Nothing
        '                'MsgBox("rem2")

        '                Return True


        '            Else

        '                Return False

        '            End If

        '    End Select


        '    Return respuesta_


        'End Function

        Public Function BorrarProduccion(ByVal indice_ As String,
                                         ByVal identificadorempresa_ As String) As Boolean

            '###################### Ajustamos la configuración de las conexiones  ###########################


            ConexionSingleton.Controlador = _origenDatos
            ConexionSingleton.SQLServerSingletonConexion.ObjetoDatos = _tipoConexion
            ConexionSingleton.SQLServerSingletonConexion.ControladorBaseDatos = _origenDatos
            ConexionSingleton.SQLServerSingletonConexion.RepositorioDatos = _objetoDatos


            '###################### Ajustamos la configuración de las conexiones  ###########################
            Dim registroDatos_ As BsonDocument = New BsonDocument()

            Dim respuesta_ As Boolean = Nothing

            Select Case _tipoescritura

                Case IOperacionesCatalogo.TiposEscritura.Inmediata

                    If Not indice_ Is Nothing Then

                        indice_ = EnvolturaDirecta(Trim(indice_), _envolturacampollave)

                    Else

                        If Not _keyValues Is Nothing Then

                            Dim contadorLlaves_ As Int32 = 1

                            For Each valor_ As String In _keyValues

                                If contadorLlaves_ = 1 Then

                                    indice_ = EnvolturaDirecta(valor_, _envolturacampollave)

                                Else

                                    indice_ = indice_ & "," & EnvolturaDirecta(Trim(valor_), _envolturacampollave)

                                End If

                                contadorLlaves_ += 1

                            Next

                            If Not _operadorcatalogoborrado = "Sp000OperadorCatalogosMaxBorradoMultipleValor" Then

                                _operadorcatalogoborrado = "Sp000OperadorCatalogosMaxBorradoMultipleValor"

                                Dim tagWatcher_ As New TagWatcher

                                _sistema.Watcher.SetError(IRecursosSistemas.RecursosCMF.Gsol_BaseDatos_Operaciones_LineaBaseCatalogos64,
                                                          TagWatcher.ErrorTypes.C3_001_3000)

                            End If

                        End If

                    End If

                    ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

                    Dim sentencia_ As String = Nothing

                    Select Case ConexionSingleton.SQLServerSingletonConexion.ObjetoDatos
                        Case IConexiones.TipoConexion.MySQLCommand

                            sentencia_ = "call " &
                                        _operadorcatalogoborrado.Replace("dbo.", Nothing) &
                                        "('" & _tablaedicion & "'," &
                                        "'" & _identificadorcatalogo & "'," &
                                        "'" & indice_ & "'," &
                                        "'" & identificadorempresa_ & "');"

                        Case IConexiones.TipoConexion.SqlCommand, IConexiones.TipoConexion.Automatico

                            'sentencia_ = "exec " & _
                            '            _operadorcatalogoborrado & _
                            '            "'" & _tablaedicion & "'," & _
                            '            "'" & _identificadorcatalogo & "'," & _
                            '            "'" & EnvolturaDirecta(indice_, _
                            '                  _envolturacampollave) & "'," & _
                            '            "'" & identificadorempresa_ & "'"

                            sentencia_ = "exec " &
                                _operadorcatalogoborrado &
                                "'" & _tablaedicion & "'," &
                                "'" & _identificadorcatalogo & "'," &
                                "'" & indice_ & "'," &
                                "'" & identificadorempresa_ & "'"

                        Case Else

                    End Select

                    If Not _iespaciotrabajo Is Nothing Then

                        If Not _iespaciotrabajo.MisCredenciales Is Nothing Then

                            ConexionSingleton.UsuarioCliente = _iespaciotrabajo.MisCredenciales.CredencialUsuario

                        End If

                    Else
                        ConexionSingleton.UsuarioCliente = _sistema.ConexionSingleton.UsuarioCliente
                    End If


                    ConexionSingleton.ModuloCliente = _operadorcatalogoconsultas

                    respuesta_ = ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(sentencia_)

                    Dim tipobitacora_ As monitoreo.IBitacoras.TiposBitacora =
                        monitoreo.IBitacoras.TiposBitacora.Informacion

                    Dim mensajebitacora_ As String = Nothing

                    If respuesta_ Then

                        mensajebitacora_ = "Borró exitosamente."

                        tipobitacora_ = monitoreo.IBitacoras.TiposBitacora.Informacion
                        'indice_ > 0 AndAlso indice_ <> ""

                        Select Case _reflejarEn

                            Case IEnlaceDatos.DestinosParaReplicacion.NoSQLMongoDB

                                Using conexionesNoSQL_ As IConexionesNoSQL = New ConexionesNoSQL

                                    Dim apuntador_ As New Dictionary(Of String, Object) From {{"Llave", _identificadorcatalogo}, {"Valor", indice_}}

                                    ' JCCS - 2021/10/22 - Para no afectar _tipoOperacionSQL que existe se crea una variable para controldar esta petición
                                    Dim tipoOperacionSQL_ As IOperacionesCatalogo.TiposOperacionSQL

                                    If _keyValues IsNot Nothing Then

                                        If _keyValues.Count > 1 Then

                                            tipoOperacionSQL_ = IOperacionesCatalogo.TiposOperacionSQL.UpdateManyAsync

                                        Else

                                            tipoOperacionSQL_ = IOperacionesCatalogo.TiposOperacionSQL.UpdateOneAsync

                                        End If

                                    Else

                                        tipoOperacionSQL_ = IOperacionesCatalogo.TiposOperacionSQL.UpdateOneAsync

                                    End If

                                    registroDatos_.Add("i_Cve_Estado", 0)

                                    conexionesNoSQL_.EjecutaSentencia(_operadorcatalogoconsultas, registroDatos_, tipoOperacionSQL_, apuntador_)

                                End Using

                            Case Else

                                'NOT IMPLEMENTED

                        End Select

                    Else

                        mensajebitacora_ = "Error al borrar"

                        tipobitacora_ = monitoreo.IBitacoras.TiposBitacora.Errores

                    End If

                    Dim nombreUsuariolocal_ As String = "Sin definir"

                    If Not EspacioTrabajo.MisCredenciales Is Nothing Then

                        nombreUsuariolocal_ = EspacioTrabajo.MisCredenciales.NombreAutenticacion

                    End If

                    _sistema.Log(mensajebitacora_,
                     tipobitacora_,
                     monitoreo.IBitacoras.TiposSucesos.Modificar,
                     _iespaciotrabajo.MisCredenciales,
                     0,
                     sentencia_.Replace("'", Nothing).Replace(Chr(34), Nothing).Replace("char(39)", Nothing).Replace("+", Nothing),
                     sentencia_.Replace("'", Nothing).Replace(Chr(34), Nothing).Replace("char(39)", Nothing).Replace("+", Nothing),
                     OperadorCatalogoConsultas, nombreUsuariolocal_)


                Case IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda

                    If Not (indice_ Is Nothing) Then

                        Dim foundRows() As Data.DataRow

                        foundRows = EncuentraRegistro(_registrostemporalesDataTable, "Id", indice_)

                        'RegistrosTemporalesDataTable.Select("Id=" & indice_)


                        RegistrosTemporalesDataTable.Rows.Remove(foundRows(0))

                        'If (_registrostemporalesDataTable.Rows.Count = 0) Then

                        '    _registrostemporales.Clear()

                        'End If

                        If _registrostemporales Is Nothing Then

                            _registrostemporales = New Dictionary(Of Int32, String)

                            _registrostemporales.Add(1, "OK")

                        Else
                            If _registrostemporales.Count >= 0 Then

                                If Not _registrostemporales.ContainsKey(1) Then
                                    _registrostemporales.Add(1, "OK")
                                End If



                            End If

                        End If


                        Return True

                    Else

                        Return False

                    End If

                Case IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia

                    If Not (indice_ Is Nothing) Then

                        Dim foundRows() As Data.DataRow

                        foundRows = EncuentraRegistro(_registrostemporalesDataTable, "Id", indice_)
                        'RegistrosTemporalesDataTable.Select("Id=" & indice_)

                        RegistrosTemporalesDataTable.Rows.Remove(foundRows(0))

                        If (_registrostemporalesDataTable.Rows.Count = 0) Then

                            _registrostemporales.Clear()

                        End If

                        Return True

                    Else

                        Return False

                    End If

            End Select

            Return respuesta_

        End Function

        Public Sub CortaConexion()

            If Not _iespaciotrabajo Is Nothing Then

                If Not _iespaciotrabajo.MisCredenciales Is Nothing Then

                    ConexionSingleton.UsuarioCliente = _iespaciotrabajo.MisCredenciales.CredencialUsuario

                End If

            Else
                ConexionSingleton.UsuarioCliente = _sistema.ConexionSingleton.UsuarioCliente
            End If


            ConexionSingleton.ModuloCliente = _operadorcatalogoconsultas

            ConexionSingleton.SQLServerSingletonConexion.TerminaConexion()

        End Sub

        Private Function AplicaClausulas(ByRef queryDocument_ As MongoDB.Driver.QueryDocument,
                                         ByVal valorindice_ As String,
                                         Optional ByVal filtrarcampo_ As String = Nothing,
                                         Optional ByVal rigor_ As String = "like") As String

            'Dim lteValue_ As BsonDocument = New BsonDocument("$lte", 0)


            'queryDocument_.Add("i_Cve_Estado", lteValue_)
            'queryDocument_.Add("i_Cve_Estado", 0)
            'queryDocument_.Add("i_Cve_DivisionMiEmpresa", 7785)

            'Equivalencia Like en MongoDB
            'Mayúsculas y minusculas
            'db.tu_coleccion.find({"campo": /.*busqueda.*/i});
            'db.tu_coleccion.find({"campo": /.*busqueda.*/});
            Dim filtros_ As String = Nothing

            Dim char_ As Char = Nothing

            If _activarComodinesDeConsulta Then

                If rigor_ = "like" Then

                    'char_ = "%"

                    char_ = ".*/i"

                End If

            Else
                rigor_ = "="
                char_ = Nothing
                valorindice_ = valorindice_.Replace("%", Nothing)
            End If

            If filtrarcampo_ Is Nothing Then

                Dim campoindicedefault_ As String = Nothing

                For Each caracteristicas_ As ICaracteristica In _icaracteristicas.Values

                    If caracteristicas_.Llave = ICaracteristica.TipoLlave.Primaria Then

                        campoindicedefault_ = caracteristicas_.Nombre

                    End If

                    If Not caracteristicas_.TipoFiltro = ICaracteristica.TiposFiltro.PorDefecto Then

                        Continue For

                    Else

                        If filtros_ Is Nothing Then

                            If caracteristicas_.TipoFiltro = ICaracteristica.TiposFiltro.PorDefecto Then

                                If Trim(valorindice_) <> "%" Then

                                    'db.tu_coleccion.find({"campo": /.*busqueda.*/i});
                                    filtros_ = " " & caracteristicas_.Nombre & " " & rigor_ & " '" & valorindice_ & char_ & "' "

                                    queryDocument_.Add(caracteristicas_.Nombre, Trim(valorindice_) & char_)

                                End If


                            Else
                                If Trim(valorindice_) <> "%" Then

                                    filtros_ = " " & caracteristicas_.Nombre & " " & rigor_ & " '" & caracteristicas_.ValorFiltro & char_ & "' "

                                    queryDocument_.Add(caracteristicas_.Nombre, Trim(valorindice_) & char_)

                                End If

                            End If

                        Else

                            If caracteristicas_.TipoFiltro = ICaracteristica.TiposFiltro.PorDefecto Then

                                If Trim(valorindice_) <> "%" Then

                                    filtros_ = filtros_ & " and " & caracteristicas_.Nombre & " " & rigor_ & " '" & valorindice_ & char_ & "' "

                                    queryDocument_.Add(caracteristicas_.Nombre, Trim(valorindice_) & char_)

                                End If

                            Else

                                If Trim(valorindice_) <> "%" Then

                                    filtros_ = filtros_ & " and " & caracteristicas_.Nombre & " " & rigor_ & " '" & caracteristicas_.ValorFiltro & char_ & "' "

                                    queryDocument_.Add(caracteristicas_.Nombre, Trim(valorindice_) & char_)

                                End If

                            End If

                        End If

                    End If

                Next

                If filtros_ Is Nothing Then

                    If Not campoindicedefault_ Is Nothing Then

                        If Trim(valorindice_) <> "%" Then

                            filtros_ = " " & campoindicedefault_ & " " & rigor_ & " '" & valorindice_ & char_ & "' "

                            queryDocument_.Add(campoindicedefault_, Trim(valorindice_) & char_)

                        End If

                    End If

                End If

            Else

                If Trim(valorindice_) <> "%" Then

                    filtros_ = " " & filtrarcampo_ & " " & rigor_ & " '" & valorindice_ & char_ & "' "

                    queryDocument_.Add(filtrarcampo_, Trim(valorindice_) & char_)

                End If

            End If

            UmbralEmpresarial(queryDocument_)

            'Obsoleto MOP 03/03/2021
            'If _filtraduplicados Then

            '    If _identificadorcatalogo <> "-1" Then

            '        filtros_ = filtros_ & umbralempresarial_ & " and " & _identificadorcatalogo & " not in(" & _entradallaves & ") "

            '        queryDocument_.Add(umbralempresarial_, Trim(valorindice_) & char_)

            '    Else

            '        _sistema.GsDialogo("Es necesario ingresar la propiedad IdentificadorCatalogo para filtrar duplicados")

            '        filtros_ = filtros_ & umbralempresarial_ & " "

            '    End If

            'Else

            '    filtros_ = filtros_ & umbralempresarial_

            'End If

            'filtros_ = LTrim(LCase(filtros_ & _
            '                 _clausulasAutoFiltro & _
            '                 _clausulaslibres))

            'PENDIENTE GeneradorAutofiltrosNoSQL()
            'PENDIENTE GeneradorClausulaslibresfiltrosNoSQL()

            'If Not filtros_ Is Nothing And filtros_ <> "" Then

            '    If filtros_.Length >= 4 Then

            '        If filtros_(0) = "a" And filtros_(1) = "n" And filtros_(2) = "d" And filtros_(3) = " " Then

            '            filtros_ = " where " & Mid(filtros_, 4)

            '        Else

            '            filtros_ = " where " & filtros_

            '        End If

            '    Else

            '        filtros_ = " where " & filtros_

            '    End If

            'End If

            Return filtros_

        End Function

        Private Function ValidaSoprteConversion(ByVal palabras_ As String) As Boolean
            Return True
        End Function

        Sub ProcesaTipoDatoYMapeo(ByRef queryDocument_ As QueryDocument,
                                       ByVal campo_ As String,
                                       ByVal valor_ As String,
                                       ByVal criterio_ As ComponentCriteriaDCKR.TypesCriteria)

            Dim commandString_ As String = Nothing

            For Each campoColeccion_ As ICaracteristica In _icaracteristicas.Values

                If Trim(LCase(campoColeccion_.Nombre)) = Trim(LCase(campo_)) Then

                    Select Case campoColeccion_.TipoDato

                        Case ICaracteristica.TiposCaracteristica.cInt32

                            Dim valorNuevo_ As Int32 = Convert.ToInt32(valor_)

                            Select Case criterio_

                                Case ComponentCriteriaDCKR.TypesCriteria.BetweenValues
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.Contains
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.EndsWith
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.StartWith
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.EqualsTo

                                    queryDocument_.Add(campoColeccion_.Nombre, valorNuevo_)

                                Case ComponentCriteriaDCKR.TypesCriteria.GreaterThan

                                    Dim gtValue_ As BsonDocument = New BsonDocument("$gt", valorNuevo_)

                                    queryDocument_.Add(campoColeccion_.Nombre, gtValue_)

                                Case ComponentCriteriaDCKR.TypesCriteria.MinorThan

                                    Dim gtValue_ As BsonDocument = New BsonDocument("$lt", valorNuevo_)

                                    queryDocument_.Add(campoColeccion_.Nombre, gtValue_)

                                Case ComponentCriteriaDCKR.TypesCriteria.IsDifferentOf
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoBetweenValues
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoContains
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoEndsWith
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoStartWith
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoEqualsTo
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoGreaterThan
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoMinorThan
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoVoid
                                    'NOT IMPLEMENTED
                            End Select

                        Case ICaracteristica.TiposCaracteristica.cReal

                            Dim valorNuevo_ As Double = Convert.ToDouble(valor_)

                            Select Case criterio_

                                Case ComponentCriteriaDCKR.TypesCriteria.BetweenValues
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.Contains
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.EndsWith
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.StartWith
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.EqualsTo

                                    queryDocument_.Add(campoColeccion_.Nombre, valorNuevo_)

                                Case ComponentCriteriaDCKR.TypesCriteria.GreaterThan

                                    Dim gtValue_ As BsonDocument = New BsonDocument("$gt", valorNuevo_)

                                    queryDocument_.Add(campoColeccion_.Nombre, gtValue_)

                                Case ComponentCriteriaDCKR.TypesCriteria.MinorThan

                                    Dim gtValue_ As BsonDocument = New BsonDocument("$lt", valorNuevo_)

                                    queryDocument_.Add(campoColeccion_.Nombre, gtValue_)

                                Case ComponentCriteriaDCKR.TypesCriteria.IsDifferentOf
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoBetweenValues
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoContains
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoEndsWith
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoStartWith
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoEqualsTo
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoGreaterThan
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoMinorThan
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoVoid
                                    'NOT IMPLEMENTED
                            End Select

                        Case ICaracteristica.TiposCaracteristica.cString

                            Select Case criterio_

                                Case ComponentCriteriaDCKR.TypesCriteria.BetweenValues
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.Contains

                                    commandString_ = "{" & campoColeccion_.Nombre & ":/.*" & valor_ & ".*/i}"
                                    queryDocument_.AddRange(BsonSerializer.Deserialize(Of BsonDocument)(commandString_))

                                Case ComponentCriteriaDCKR.TypesCriteria.EndsWith

                                    commandString_ = "{" & campoColeccion_.Nombre & ":/" & valor_ & "$/i}"
                                    queryDocument_.AddRange(BsonSerializer.Deserialize(Of BsonDocument)(commandString_))

                                Case ComponentCriteriaDCKR.TypesCriteria.StartWith

                                    commandString_ = "{" & campoColeccion_.Nombre & ":/^" & valor_ & "/i}"
                                    queryDocument_.AddRange(BsonSerializer.Deserialize(Of BsonDocument)(commandString_))

                                Case ComponentCriteriaDCKR.TypesCriteria.EqualsTo

                                    commandString_ = "{" & campoColeccion_.Nombre & ":/" & valor_ & "/i}"

                                    queryDocument_.AddRange(BsonSerializer.Deserialize(Of BsonDocument)(commandString_))

                                Case ComponentCriteriaDCKR.TypesCriteria.GreaterThan

                                    Dim gtValue_ As BsonDocument = New BsonDocument("$gt", valor_)

                                    queryDocument_.Add(campoColeccion_.Nombre, gtValue_)

                                Case ComponentCriteriaDCKR.TypesCriteria.MinorThan

                                    Dim gtValue_ As BsonDocument = New BsonDocument("$lt", valor_)

                                    queryDocument_.Add(campoColeccion_.Nombre, gtValue_)

                                Case ComponentCriteriaDCKR.TypesCriteria.IsDifferentOf
                                    'NOT IMPLEMENTED
                                    '{ $not: /TST/i }
                                    commandString_ = "{" & campoColeccion_.Nombre & ":{ $not:/" & valor_ & "/i}}"

                                    queryDocument_.AddRange(BsonSerializer.Deserialize(Of BsonDocument)(commandString_))

                                Case ComponentCriteriaDCKR.TypesCriteria.NoBetweenValues
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoContains
                                    'NOT IMPLEMENTED
                                    commandString_ = "{" & campoColeccion_.Nombre & ":{ $not:/.*" & valor_ & ".*/i}}"

                                    queryDocument_.AddRange(BsonSerializer.Deserialize(Of BsonDocument)(commandString_))

                                Case ComponentCriteriaDCKR.TypesCriteria.NoEndsWith
                                    'NOT IMPLEMENTED
                                    '{ $not: /TST$/i }

                                    commandString_ = "{" & campoColeccion_.Nombre & ":{ $not:/" & valor_ & "$/i}}"

                                    queryDocument_.AddRange(BsonSerializer.Deserialize(Of BsonDocument)(commandString_))

                                Case ComponentCriteriaDCKR.TypesCriteria.NoStartWith
                                    'NOT IMPLEMENTED
                                    '{ $not: /^TST/i }

                                    commandString_ = "{" & campoColeccion_.Nombre & ":{ $not:/^" & valor_ & "/i}}"

                                    queryDocument_.AddRange(BsonSerializer.Deserialize(Of BsonDocument)(commandString_))

                                Case ComponentCriteriaDCKR.TypesCriteria.NoEqualsTo
                                    'NOT IMPLEMENTED
                                    '{ $not: /TST/ }
                                    commandString_ = "{" & campoColeccion_.Nombre & ":{ $not:/" & valor_ & "/}}"

                                    queryDocument_.AddRange(BsonSerializer.Deserialize(Of BsonDocument)(commandString_))

                                Case ComponentCriteriaDCKR.TypesCriteria.NoGreaterThan
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoMinorThan
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoVoid
                                    'NOT IMPLEMENTED
                                    '{$or:[{"t_EstatusReferencia":{$exists:false},"t_EstatusReferencia":{$eq:null},"t_EstatusReferencia":{$eq:""}}]}
                                    '{$nor:[{"t_EstatusReferencia":{$exists:false},"t_EstatusReferencia":{$eq:null},"t_EstatusReferencia":{$eq:""}}]}
                                    '{$or:[{"t_EstatusReferencia":{$not : {$exists:false}},"t_EstatusReferencia":{$not :{$eq:null}},"t_EstatusReferencia":{$not:{$eq:""}}}]} 434867
                                    commandString_ = "{$nor:[{" & campoColeccion_.Nombre & ":{$exists:false}," & campoColeccion_.Nombre & ":{$eq:null}," & campoColeccion_.Nombre & ":{$eq:''}}]}"

                                    queryDocument_.AddRange(BsonSerializer.Deserialize(Of BsonDocument)(commandString_))

                                    'Case ComponentCriteriaDCKR.TypesCriteria.IsNullOrBlank

                                    '    commandString_ = "{$or:[{" & campoColeccion_.Nombre & ":{$exists:false}," & campoColeccion_.Nombre & ":{$eq:null}," & campoColeccion_.Nombre & ":{$eq:''}}]}"

                                    '    queryDocument_.AddRange(BsonSerializer.Deserialize(Of BsonDocument)(commandString_))

                            End Select

                        Case ICaracteristica.TiposCaracteristica.cDateTime

                            Dim valorNuevo_ As DateTime = Convert.ToDateTime(valor_).ToUniversalTime()

                            Select Case criterio_

                                Case ComponentCriteriaDCKR.TypesCriteria.BetweenValues

                                    'Dim gtValue_ As BsonDocument = New BsonDocument("$gt", valorNuevo_)

                                    'queryDocument_.Add(campoColeccion_.Nombre, gtValue_)

                                    'Dim lteValue2_ As BsonDocument = New BsonDocument("$lt", valorNuevo_)

                                    'queryDocument_.Add(campoColeccion_.Nombre, lteValue2_)

                                Case ComponentCriteriaDCKR.TypesCriteria.Contains
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.EndsWith
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.StartWith
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.EqualsTo

                                    queryDocument_.Add(campoColeccion_.Nombre, valorNuevo_)

                                    'Dim command As String = "{f_FechaModificacion : {$eq: new Date('23/03/2021')}}"

                                    commandString_ = "{" & campoColeccion_.Nombre & ":{$eq: new Date('" & valor_ & "')}}"

                                    queryDocument_.AddRange(BsonSerializer.Deserialize(Of BsonDocument)(commandString_))

                                Case ComponentCriteriaDCKR.TypesCriteria.GreaterThan

                                    commandString_ = "{" & campoColeccion_.Nombre & ":{$gt: new Date('" & valor_ & "')}}"

                                    queryDocument_.AddRange(BsonSerializer.Deserialize(Of BsonDocument)(commandString_))

                                Case ComponentCriteriaDCKR.TypesCriteria.MinorThan

                                    commandString_ = "{" & campoColeccion_.Nombre & ":{$lt: new Date('" & valor_ & "')}}"

                                    queryDocument_.AddRange(BsonSerializer.Deserialize(Of BsonDocument)(commandString_))

                                Case ComponentCriteriaDCKR.TypesCriteria.IsDifferentOf

                                    commandString_ = "{" & campoColeccion_.Nombre & ":{$neq: new Date('" & valor_ & "')}}"

                                    queryDocument_.AddRange(BsonSerializer.Deserialize(Of BsonDocument)(commandString_))

                                Case ComponentCriteriaDCKR.TypesCriteria.GreaterThanOrEquals

                                    commandString_ = "{" & campoColeccion_.Nombre & ":{$gte: new Date('" & valor_ & "')}}"

                                    queryDocument_.AddRange(BsonSerializer.Deserialize(Of BsonDocument)(commandString_))

                                Case ComponentCriteriaDCKR.TypesCriteria.LessThanOrEquals

                                    commandString_ = "{" & campoColeccion_.Nombre & ":{$lte: new Date('" & valor_ & "')}}"

                                    queryDocument_.AddRange(BsonSerializer.Deserialize(Of BsonDocument)(commandString_))

                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoBetweenValues
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoContains
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoEndsWith
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoStartWith
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoEqualsTo
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoGreaterThan
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoMinorThan
                                    'NOT IMPLEMENTED
                                Case ComponentCriteriaDCKR.TypesCriteria.NoVoid
                                    'NOT IMPLEMENTED
                            End Select

                        Case ICaracteristica.TiposCaracteristica.cBoolean

                            Dim valorNuevo_ As Boolean = Convert.ToBoolean(valor_)

                            queryDocument_.Add(campoColeccion_.Nombre, valorNuevo_)

                        Case Else

                            queryDocument_.Add(campoColeccion_.Nombre, valor_)

                    End Select

                    Exit Sub

                End If

            Next

        End Sub

        Private Sub AplicaClausulasNoSQL(ByRef queryDocument_ As MongoDB.Driver.QueryDocument)

            'Dim lteValue_ As BsonDocument = New BsonDocument("$lte", 0)
            'queryDocument_.Add("i_Cve_Estado", lteValue_)
            'queryDocument_.Add("i_Cve_Estado", 0)
            'queryDocument_.Add("i_Cve_DivisionMiEmpresa", 7785)

            'Equivalencia Like en MongoDB
            'Mayúsculas y minusculas
            'db.tu_coleccion.find({"campo": /.*busqueda.*/i});
            'db.tu_coleccion.find({"campo": /.*busqueda.*/});
            'Dim filtros_ As String = Nothing
            '******************************************************

            Dim filtrosCompletos_ As String =
                (Trim(_clausulasAutoFiltro & _clausulaslibres))
            '& UmbralEmpresarial()))

            filtrosCompletos_ =
                filtrosCompletos_.Replace("   ", " ").Replace("   ", " ").Replace("'", "")

            'Aquí revisaremos si soportaremos la transformación, es decir si no hay signos ni simbolos no soportados, agrupaciones, subqueries, etc.

            If Not ValidaSoprteConversion(filtrosCompletos_) Then

                'MsgBox("No soportado")

                Exit Sub

            End If

            Dim palabras_ As String() = filtrosCompletos_.Split(" ")

            Dim indice_ As Int32 = 0

            'Limpieza de palabras, blancos y apostrofes

            'For Each palabra_ As String In palabras_

            '    If palabra_(0) = "'" Then : palabra_.Replace() : End If

            '    If palabra_(palabra_.Length - 1) = "'" Then : palabra_.Remove(palabra_.Length - 1) : End If

            'Next

            For Each palabra_ As String In palabras_

                If indice_ = 0 And
                    (LCase(Trim(palabra_)) = "and" Or
                    LCase(Trim(palabra_)) = "or") Then

                    indice_ += 1

                    Continue For

                End If

                If (LCase(Trim(palabras_(palabras_.Length - 1))) = "and" _
                        Or LCase(Trim(palabras_(palabras_.Length - 1))) = "or" _
                        Or (Trim(palabras_(palabras_.Length - 1))) = "=" _
                        Or (Trim(palabras_(palabras_.Length - 1))) = "<>" _
                        Or (Trim(palabras_(palabras_.Length - 1))) = ">" _
                        Or (Trim(palabras_(palabras_.Length - 1))) = "<" _
                        Or (Trim(palabras_(palabras_.Length - 1))) = ">=" _
                        Or (Trim(palabras_(palabras_.Length - 1))) = "<=") Then

                    Exit For
                    'Genramos una notificación de la estructura erronea

                End If

                If indice_ > 0 And ((palabra_) = "=" Or LCase(palabra_) = "like" Or (palabra_) = ">" Or (palabra_) = "<" Or (palabra_) = ">=" Or (palabra_) = "<=" Or (palabra_) = "<>") Then

                    If (Not palabras_(indice_ - 1) Is Nothing And Not palabras_(indice_ + 1) Is Nothing) And
                        (palabras_(indice_ - 1) <> "=" And LCase(palabras_(indice_ + 1)) <> "like" And palabras_(indice_ + 1) <> ">" And palabras_(indice_ + 1) <> "<" And palabras_(indice_ + 1) <> ">=" And palabras_(indice_ + 1) <> "<=" And palabras_(indice_ + 1) <> "<>") Then
                        'Generamos el objeto binario
                        Select Case palabra_
                            Case "="

                                ProcesaTipoDatoYMapeo(queryDocument_,
                                                      palabras_(indice_ - 1),
                                                      palabras_(indice_ + 1),
                                                      ComponentCriteriaDCKR.TypesCriteria.EqualsTo)

                            Case "<>"

                                ProcesaTipoDatoYMapeo(queryDocument_,
                                                      palabras_(indice_ - 1),
                                                      palabras_(indice_ + 1),
                                                      ComponentCriteriaDCKR.TypesCriteria.IsDifferentOf)
                            Case ">"


                                ProcesaTipoDatoYMapeo(queryDocument_,
                                                      palabras_(indice_ - 1),
                                                      palabras_(indice_ + 1),
                                                      ComponentCriteriaDCKR.TypesCriteria.GreaterThan)

                            Case "<"

                                ProcesaTipoDatoYMapeo(queryDocument_,
                                                      palabras_(indice_ - 1),
                                                      palabras_(indice_ + 1),
                                                      ComponentCriteriaDCKR.TypesCriteria.MinorThan)

                            Case ">="

                                ProcesaTipoDatoYMapeo(queryDocument_,
                                                     palabras_(indice_ - 1),
                                                     palabras_(indice_ + 1),
                                                     ComponentCriteriaDCKR.TypesCriteria.GreaterThanOrEquals)

                            Case "<="

                                ProcesaTipoDatoYMapeo(queryDocument_,
                                                     palabras_(indice_ - 1),
                                                     palabras_(indice_ + 1),
                                                     ComponentCriteriaDCKR.TypesCriteria.LessThanOrEquals)

                            Case "like"

                                Dim token_ As String() = palabras_(indice_ + 1).Split("%")

                                Dim criterio_ As ComponentCriteriaDCKR.TypesCriteria =
                                    ComponentCriteriaDCKR.TypesCriteria.EqualsTo

                                If token_.Length > 1 Then

                                    If token_(0) = "" And token_(token_.Length - 1) <> "" Then

                                        criterio_ = ComponentCriteriaDCKR.TypesCriteria.EndsWith

                                    ElseIf token_(token_.Length - 1) <> "" And token_(token_.Length - 1) = "" Then

                                        criterio_ = ComponentCriteriaDCKR.TypesCriteria.StartWith

                                    ElseIf token_(token_.Length - 1) = "" And token_(token_.Length - 1) = "" Then

                                        criterio_ = ComponentCriteriaDCKR.TypesCriteria.Contains

                                    End If

                                ElseIf token_.Length = 1 Then

                                    criterio_ = ComponentCriteriaDCKR.TypesCriteria.EqualsTo

                                End If

                                ProcesaTipoDatoYMapeo(queryDocument_,
                                                         palabras_(indice_ - 1),
                                                         palabras_(indice_ + 1).Replace("%", Nothing),
                                                         criterio_)

                        End Select

                    Else

                        'Genramos una notificación de la estructura erronea

                    End If

                End If

                indice_ += 1

            Next

            'Agregamos la division empresarial

            UmbralEmpresarial(queryDocument_)

            '******************************************************

        End Sub

        'RDBMS MOP 03/03/2021
        Private Function AplicaClausulas(ByVal valorindice_ As String,
                                         Optional ByVal filtrarcampo_ As String = Nothing,
                                         Optional ByVal rigor_ As String = "like") As String

            Dim filtros_ As String = Nothing

            Dim char_ As Char = Nothing

            If _activarComodinesDeConsulta Then
                If rigor_ = "like" Then : char_ = "%" : End If
            Else
                rigor_ = "="
                char_ = Nothing
                valorindice_ = valorindice_.Replace("%", Nothing)
            End If

            'If rigor_ = "like" Then : char_ = "%" : End If

            If filtrarcampo_ Is Nothing Then

                Dim campoindicedefault_ As String = Nothing

                For Each caracteristicas_ As ICaracteristica In _icaracteristicas.Values

                    If caracteristicas_.Llave = ICaracteristica.TipoLlave.Primaria Then

                        campoindicedefault_ = caracteristicas_.Nombre

                    End If

                    If Not caracteristicas_.TipoFiltro = ICaracteristica.TiposFiltro.PorDefecto Then

                        Continue For

                    Else

                        If filtros_ Is Nothing Then

                            If caracteristicas_.TipoFiltro = ICaracteristica.TiposFiltro.PorDefecto Then

                                If Trim(valorindice_) <> "%" Then
                                    'filtros_ = " where " & caracteristicas_.Nombre & " " & rigor_ & " '" & valorindice_ & char_ & "' "
                                    filtros_ = " " & caracteristicas_.Nombre & " " & rigor_ & " '" & valorindice_ & char_ & "' "

                                End If


                            Else
                                If Trim(valorindice_) <> "%" Then
                                    'filtros_ = " where " & caracteristicas_.Nombre & " " & rigor_ & " '" & caracteristicas_.ValorFiltro & char_ & "' "
                                    filtros_ = " " & caracteristicas_.Nombre & " " & rigor_ & " '" & caracteristicas_.ValorFiltro & char_ & "' "

                                End If

                            End If

                        Else

                            If caracteristicas_.TipoFiltro = ICaracteristica.TiposFiltro.PorDefecto Then

                                If Trim(valorindice_) <> "%" Then

                                    filtros_ = filtros_ & " and " & caracteristicas_.Nombre & " " & rigor_ & " '" & valorindice_ & char_ & "' "

                                End If

                            Else

                                If Trim(valorindice_) <> "%" Then

                                    filtros_ = filtros_ & " and " & caracteristicas_.Nombre & " " & rigor_ & " '" & caracteristicas_.ValorFiltro & char_ & "' "

                                End If

                            End If

                        End If

                    End If

                Next

                If filtros_ Is Nothing Then

                    If Not campoindicedefault_ Is Nothing Then

                        'nueva MOP 22/10/2020
                        If Trim(valorindice_) <> "%" Then
                            'filtros_ = " where " & campoindicedefault_ & " " & rigor_ & " '" & valorindice_ & char_ & "' "
                            filtros_ = " " & campoindicedefault_ & " " & rigor_ & " '" & valorindice_ & char_ & "' "

                        End If

                    Else
                        'filtros_ = " where 1=1 "
                        ' filtros_ = " 1=1 "

                    End If

                End If

            Else

                'nueva MOP 22/10/2020
                If Trim(valorindice_) <> "%" Then

                    'filtros_ = " where " & filtrarcampo_ & " " & rigor_ & " '" & valorindice_ & char_ & "' "
                    filtros_ = " " & filtrarcampo_ & " " & rigor_ & " '" & valorindice_ & char_ & "' "

                End If


            End If

            Dim umbralempresarial_ As String = Nothing

            umbralempresarial_ = UmbralEmpresarial()

            If _filtraduplicados Then

                If _identificadorcatalogo <> "-1" Then

                    'filtros_ = filtros_ & umbralempresarial_ & " and " & _identificadorcatalogo & " not in(" & _entradallaves & ") " & _
                    '           _clausulasAutoFiltro & _
                    '           _clausulaslibres
                    filtros_ = filtros_ & umbralempresarial_ & " and " & _identificadorcatalogo & " not in(" & _entradallaves & ") "

                Else

                    _sistema.GsDialogo("Es necesario ingresar la propiedad IdentificadorCatalogo para filtrar duplicados")

                    'filtros_ = filtros_ & umbralempresarial_ & " " & _
                    '           _clausulasAutoFiltro & _
                    '           _clausulaslibres

                    filtros_ = filtros_ & umbralempresarial_ & " "

                End If

            Else

                'filtros_ = filtros_ & umbralempresarial_ & _
                '             _clausulasAutoFiltro & _
                '             _clausulaslibres

                filtros_ = filtros_ & umbralempresarial_

            End If

            filtros_ = LTrim(LCase(filtros_ &
                             _clausulasAutoFiltro &
                             _clausulaslibres))

            If Not filtros_ Is Nothing And filtros_ <> "" Then

                If filtros_.Length >= 4 Then

                    If filtros_(0) = "a" And filtros_(1) = "n" And filtros_(2) = "d" And filtros_(3) = " " Then

                        filtros_ = " where " & Mid(filtros_, 4)

                    Else

                        filtros_ = " where " & filtros_

                    End If

                Else

                    filtros_ = " where " & filtros_

                End If



            End If

            Return filtros_

        End Function

        Private Function UmbralEmpresarial() As String

            Dim umbral_ As String = Nothing

            If Not _identificadorempresa Is Nothing And
                _identificadorempresa <> "-1" Then

                If Not _iespaciotrabajo.MisCredenciales Is Nothing Then

                    umbral_ = " and i_Cve_DivisionMiEmpresa = " & _iespaciotrabajo.MisCredenciales.DivisionEmpresaria & " "

                End If


            End If

            Return umbral_

        End Function

        Private Sub UmbralEmpresarial(ByRef queryDocument_ As QueryDocument)

            Dim umbral_ As String = Nothing

            If Not _identificadorempresa Is Nothing And
                _identificadorempresa <> "-1" Then

                If Not _iespaciotrabajo.MisCredenciales Is Nothing Then

                    If Not queryDocument_.ContainsValue(_iespaciotrabajo.MisCredenciales.DivisionEmpresaria) Then

                        queryDocument_.Add("i_Cve_DivisionMiEmpresa", _iespaciotrabajo.MisCredenciales.DivisionEmpresaria)

                    End If

                End If

            End If

        End Sub

        Public Async Function ConsultaProduccionNoSQLAsync(Optional ByVal valordefaultbuscado_ As String = "%",
                                           Optional ByVal filtrarcampo_ As String = Nothing,
                                           Optional ByVal rigor_ As String = "like") As Threading.Tasks.Task(Of List(Of BsonDocument))

            Dim bsonDocumentAnswer_ As List(Of BsonDocument) = Nothing

            Select Case _tipoescritura

                Case IOperacionesCatalogo.TiposEscritura.MemoriaIntermediaBeta,
                     IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia,
                     IOperacionesCatalogo.TiposEscritura.SinDefinir,
                     IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda

                    _sistema.Watcher.SetError(TagWatcher.ErrorTypes.C2_000_2000)

                    Return bsonDocumentAnswer_

                Case IOperacionesCatalogo.TiposEscritura.Inmediata

                    Select Case _origenDatos

                        Case IConexiones.Controladores.Automatico,
                            IConexiones.Controladores.SQLServer2008,
                            IConexiones.Controladores.MySQL51


                            _sistema.Watcher.SetError(TagWatcher.ErrorTypes.C2_000_2000)

                            Return bsonDocumentAnswer_

                        Case IConexiones.TipoConexion.DirectMongoDB

                            'NOT IMPLEMENTED

                    End Select

            End Select

            Select Case _modalidadConsulta

                Case IOperacionesCatalogo.ModalidadesConsulta.Singleton,
                     IOperacionesCatalogo.ModalidadesConsulta.SinDefinir

                    _sistema.Watcher.SetError(TagWatcher.ErrorTypes.C2_000_2000)

                Case IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre



                    Dim queryDocument_ As New QueryDocument

                    Dim stringProjection_ As String = Nothing

                    For Each campoVirtual_ As CampoVirtual In _estructuraPresentacion.Atributos

                        stringProjection_ = stringProjection_ & "," & campoVirtual_.Atributo.ToString & ":1"

                    Next

                    Dim projectionString_ As ProjectionDefinition(Of BsonDocument) = "{_id: 1" & stringProjection_ & "}"

                    'Using cursor_ = Await collectionData_.
                    'Find(queryDocument_).
                    'Project(projectionDefinition_).
                    'Limit(collectionLimit_).
                    'ToCursorAsync

                    '    While (Await cursor_.MoveNextAsync())

                    '        For Each document_ In cursor_.Current

                    '            listBSONDocuments_.Add(document_)

                    '        Next

                    '    End While

                    'End Using

                    'Se colocan el registro del filtro por defecto en caso que haya sido requerido
                    AplicaClausulas(queryDocument_, valordefaultbuscado_, filtrarcampo_, rigor_)

                    'Se traducen los valores de filtro sql a mongodb
                    AplicaClausulasNoSQL(queryDocument_)

                    'var task = camarero.Servicio("Bla");
                    'task.Start();
                    'HablandoDeLaVida();
                    '//Mi amigo pide el brindis. Nos esperamos

                    'var cerveza = Await task;
                    '//start = DateTime.Now;

                    ' Dim task_ As Task = conexionesNoSQL_.EjecutaConsultaAsync(_operadorcatalogoconsultas, queryDocument_, projectionString_, _cantidadvisibleregistros)
                    'task_.Start()

                    'HablandoDeLaVida();

                    'bsonDocumentAnswer_ = Await task_

                    Using conexionesNoSQL_ As IConexionesNoSQL = New ConexionesNoSQL

                        bsonDocumentAnswer_ = Await conexionesNoSQL_.EjecutaConsultaAsync(_operadorcatalogoconsultas, queryDocument_, projectionString_, _cantidadvisibleregistros)

                        If Not bsonDocumentAnswer_ Is Nothing Then

                            Return bsonDocumentAnswer_

                        End If

                    End Using

            End Select

            Return bsonDocumentAnswer_

        End Function

        Public Function ConsultaProduccionNoSQL(Optional ByVal valordefaultbuscado_ As String = "%",
                                           Optional ByVal filtrarcampo_ As String = Nothing,
                                           Optional ByVal rigor_ As String = "like") As List(Of BsonDocument) ' MongoCursor(Of BsonDocument)

            Dim bsonDocumentAnswer_ As List(Of BsonDocument) = Nothing

            Select Case _tipoescritura

                Case IOperacionesCatalogo.TiposEscritura.MemoriaIntermediaBeta,
                     IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia,
                     IOperacionesCatalogo.TiposEscritura.SinDefinir,
                     IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda

                    _sistema.Watcher.SetError(TagWatcher.ErrorTypes.C1_000_0001)

                    'Dim algo As New TagWatcher

                    Return bsonDocumentAnswer_

                Case IOperacionesCatalogo.TiposEscritura.Inmediata

                    Select Case _origenDatos

                        Case IConexiones.Controladores.Automatico,
                            IConexiones.Controladores.SQLServer2008,
                            IConexiones.Controladores.MySQL51


                            _sistema.Watcher.SetError(TagWatcher.ErrorTypes.C2_000_2000)

                            Return bsonDocumentAnswer_

                            '        sqltexto_ = "select top(" & _cantidadvisibleregistros & ") " & _
                            '        ExtractorEncabezados() & _
                            '        " from  " & _
                            '        _operadorcatalogoconsultas & _
                            '        " " & AplicaClausulas(valordefaultbuscado_, _
                            '        filtrarcampo_, _
                            '        rigor_)

                        Case IConexiones.TipoConexion.DirectMongoDB

                            'NOT IMPLEMENTED

                    End Select

            End Select

            Select Case _modalidadConsulta

                Case IOperacionesCatalogo.ModalidadesConsulta.Singleton,
                     IOperacionesCatalogo.ModalidadesConsulta.SinDefinir

                    _sistema.Watcher.SetError(TagWatcher.ErrorTypes.C2_000_2000)

                Case IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                    Using conexionesNoSQL_ As IConexionesNoSQL = New ConexionesNoSQL

                        Dim queryDocument_ As New QueryDocument

                        Dim stringProjection_ As String = Nothing

                        For Each campoVirtual_ As CampoVirtual In _estructuraPresentacion.Atributos

                            stringProjection_ = stringProjection_ & "," & campoVirtual_.Atributo.ToString & ":1"

                        Next

                        Dim projectionString_ As ProjectionDefinition(Of BsonDocument) = "{_id: 1" & stringProjection_ & "}"

                        'Se colocan el registro del filtro por defecto en caso que haya sido requerido
                        AplicaClausulas(queryDocument_, valordefaultbuscado_, filtrarcampo_, rigor_)

                        'Se traducen los valores de filtro sql a mongodb
                        AplicaClausulasNoSQL(queryDocument_)

                        bsonDocumentAnswer_ = conexionesNoSQL_.EjecutaConsulta(_operadorcatalogoconsultas, queryDocument_, projectionString_, _cantidadvisibleregistros)

                        'bsonDocumentAnswer_ = Await conexionesNoSQL_.EjecutaConsultaAsync(_operadorcatalogoconsultas, queryDocument_, projectionString_, _cantidadvisibleregistros)

                        If Not bsonDocumentAnswer_ Is Nothing Then

                            Return bsonDocumentAnswer_

                        End If

                    End Using

            End Select

            Return bsonDocumentAnswer_

        End Function

        Public Sub BSONDocumentToDataSet(ByRef datasetvista_ As DataSet, ByRef bsonDocumentListResult_ As List(Of BsonDocument))

            datasetvista_.Clear()

            Dim dataTableObject_ As New DataTable(_operadorcatalogoconsultas)

            For Each campoVirtual_ As CampoVirtual In EstructuraConsulta.Atributos

                dataTableObject_.Columns.Add(campoVirtual_.Atributo.ToString)

            Next

            For Each item As BsonDocument In bsonDocumentListResult_

                Dim dataRow_ As DataRow

                dataRow_ = dataTableObject_.NewRow

                For Each campo_ As ICaracteristica In Caracteristicas.Values

                    If Not campo_.Visible = ICaracteristica.TiposVisible.No And
                      Not campo_.Visible = ICaracteristica.TiposVisible.Virtual And
                       Not campo_.Visible = ICaracteristica.TiposVisible.Informacion And
                       Not campo_.Visible = ICaracteristica.TiposVisible.Impresion Then

                        If item.Contains(campo_.Nombre) Then

                            Dim campoElement_ As BsonElement = Nothing

                            campoElement_ = item.GetElement(campo_.Nombre)

                            Select Case campo_.TipoDato
                                Case ICaracteristica.TiposCaracteristica.cInt32,
                                     ICaracteristica.TiposCaracteristica.cReal

                                    dataRow_(campo_.Nombre) = campoElement_.Value

                                Case ICaracteristica.TiposCaracteristica.cString,
                                     ICaracteristica.TiposCaracteristica.cBoolean

                                    dataRow_(campo_.Nombre) = campoElement_.Value.ToString

                                Case ICaracteristica.TiposCaracteristica.cDateTime

                                    Dim fecha_ As DateTime = DateTime.Parse(campoElement_.Value.ToString, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)

                                    dataRow_(campo_.Nombre) = fecha_.ToString("dd/MM/yyyy")

                            End Select

                        End If

                    End If

                Next

                dataTableObject_.Rows.Add(dataRow_)

            Next

            datasetvista_.Tables.Add(dataTableObject_)

        End Sub

        Public Function ConsultaProduccion(Optional ByVal valordefaultbuscado_ As String = "%",
                                           Optional ByVal filtrarcampo_ As String = Nothing,
                                           Optional ByVal rigor_ As String = "like") As DataSet

            Dim dataset_ As New DataSet

            '###################### Ajustamos la configuración de las conexiones  ###########################


            'ConexionSingleton.Controlador = _origenDatos
            'ConexionSingleton.SQLServerSingletonConexion.ObjetoDatos = _tipoConexion
            'ConexionSingleton.SQLServerSingletonConexion.ControladorBaseDatos = _origenDatos
            'ConexionSingleton.SQLServerSingletonConexion.RepositorioDatos = _objetoDatos

            Select Case _origenDatos
                Case IConexiones.Controladores.MongoDB

                    BSONDocumentResult = ConsultaProduccionNoSQL(valordefaultbuscado_, filtrarcampo_, rigor_)

                    BSONDocumentToDataSet(dataset_, BSONDocumentResult)

                    Return dataset_

                Case IConexiones.Controladores.SQLServer2008
                    'Continue

                Case Else
                    'NOT IMPLEMENTED
                    'Throw New System.Exception("Not implemented")


            End Select

            '###################### Ajustamos la configuración de las conexiones  ###########################

            Dim sqltexto_ As String = Nothing



            Select Case _tipoescritura

                Case IOperacionesCatalogo.TiposEscritura.Inmediata

                    Select Case ConexionSingleton.SQLServerSingletonConexion.ObjetoDatos
                        Case IConexiones.TipoConexion.MySQLCommand

                            Select Case _cantidadvisibleregistros

                                Case 0

                                    sqltexto_ = "select " &
                                    ExtractorEncabezados() &
                                    " from  " &
                                    _operadorcatalogoconsultas &
                                    " " & AplicaClausulas(valordefaultbuscado_,
                                    filtrarcampo_,
                                    rigor_)

                                Case Else

                                    sqltexto_ = "select " &
                                     ExtractorEncabezados() &
                                     " from  " &
                                    _operadorcatalogoconsultas &
                                    " " & AplicaClausulas(valordefaultbuscado_,
                                    filtrarcampo_,
                                    rigor_) & " limit " & _cantidadvisibleregistros

                            End Select

                        Case IConexiones.TipoConexion.SqlCommand,
                             IConexiones.TipoConexion.Automatico,
                             IConexiones.TipoConexion.OdbcCommand

                            Select Case _cantidadvisibleregistros

                                Case 0

                                    sqltexto_ = "select " &
                                    ExtractorEncabezados() &
                                    " from  " &
                                    _operadorcatalogoconsultas &
                                    " " & AplicaClausulas(valordefaultbuscado_,
                                    filtrarcampo_,
                                    rigor_)


                                Case Else

                                    sqltexto_ = "select top(" & _cantidadvisibleregistros & ") " &
                                    ExtractorEncabezados() &
                                    " from  " &
                                    _operadorcatalogoconsultas &
                                    " " & AplicaClausulas(valordefaultbuscado_,
                                    filtrarcampo_,
                                    rigor_)


                            End Select


                        Case IConexiones.TipoConexion.DirectMongoDB

                            'NOT IMPLEMENTED

                    End Select

                    _salidallaves = "select " &
                                                _identificadorcatalogo &
                                                " from  " &
                                                _operadorcatalogoconsultas


                Case IOperacionesCatalogo.TiposEscritura.MemoriaIntermediaBeta

                    Dim indice_ As Int32 = 0

                    For Each registro_ As String In _registrostemporalespresentacion.Values

                        If sqltexto_ Is Nothing Then

                            sqltexto_ = "select " & indice_ & " as clave, " & registro_

                        Else

                            sqltexto_ = sqltexto_ & " union all select " & indice_ & " as clave, " & registro_

                        End If

                        indice_ += 1

                    Next


            End Select

            If Not sqltexto_ Is Nothing Then

                ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

                If Not _iespaciotrabajo Is Nothing Then

                    If Not _iespaciotrabajo.MisCredenciales Is Nothing Then

                        ConexionSingleton.UsuarioCliente = _iespaciotrabajo.MisCredenciales.CredencialUsuario

                    End If

                Else
                    ConexionSingleton.UsuarioCliente = _sistema.ConexionSingleton.UsuarioCliente
                End If


                ConexionSingleton.ModuloCliente = _operadorcatalogoconsultas


                '::::::. Modificaciones Webservices

                Select Case _modalidadConsulta

                    Case IOperacionesCatalogo.ModalidadesConsulta.Singleton,
                         IOperacionesCatalogo.ModalidadesConsulta.SinDefinir

                        Dim i_Cve_UsuarioAuxiliar_ As Int32 = 0
                        Dim i_cve_DivisionMiEmpresaAuxiliar_ As Int32 = 0
                        Dim i_cve_aplicacionAuxiliar_ As Int32 = 0
                        Dim t_nombreUsuarioAuxiliar_ As String = Nothing
                        Dim t_nombreModuloEnsambladoAuxiliar_ As String = Nothing
                        Dim t_cuentaPublicaUsuarioAuxiliar_ As String = Nothing

                        If Not _iespaciotrabajo Is Nothing Then

                            If Not _iespaciotrabajo.MisCredenciales Is Nothing Then

                                i_Cve_UsuarioAuxiliar_ = _iespaciotrabajo.MisCredenciales.ClaveUsuario
                                t_cuentaPublicaUsuarioAuxiliar_ = _iespaciotrabajo.MisCredenciales.CredencialUsuario
                                i_cve_DivisionMiEmpresaAuxiliar_ = _iespaciotrabajo.MisCredenciales.DivisionEmpresaria
                                i_cve_aplicacionAuxiliar_ = _iespaciotrabajo.MisCredenciales.Aplicacion
                                t_nombreUsuarioAuxiliar_ = _iespaciotrabajo.MisCredenciales.NombreAutenticacion

                            End If

                        End If

                        If Not _operadorcatalogoconsultas Is Nothing Then
                            t_nombreModuloEnsambladoAuxiliar_ = _operadorcatalogoconsultas
                        End If

                        If _i_Cve_Aplicacion <> 0 Then
                            ConexionSingleton.SQLServerSingletonConexion.IDAplicacion = _i_Cve_Aplicacion
                        ElseIf i_cve_aplicacionAuxiliar_ <> 0 Then
                            ConexionSingleton.SQLServerSingletonConexion.IDAplicacion = i_cve_aplicacionAuxiliar_
                        Else
                            ConexionSingleton.SQLServerSingletonConexion.IDAplicacion = IBitacoras.ClaveTiposAplicacion.SinDefinir
                        End If

                        If _i_Cve_DivisionMiEmpresa <> 0 Then
                            ConexionSingleton.SQLServerSingletonConexion.ClaveDivisionMiEmpresa = _i_Cve_DivisionMiEmpresa
                        ElseIf i_cve_DivisionMiEmpresaAuxiliar_ <> 0 Then
                            ConexionSingleton.SQLServerSingletonConexion.ClaveDivisionMiEmpresa = i_cve_DivisionMiEmpresaAuxiliar_
                        Else
                            ConexionSingleton.SQLServerSingletonConexion.ClaveDivisionMiEmpresa = 0
                        End If

                        If _i_Cve_Usuario <> 0 Then
                            ConexionSingleton.SQLServerSingletonConexion.IDUsuario = _i_Cve_Usuario
                        ElseIf i_Cve_UsuarioAuxiliar_ <> 0 Then
                            ConexionSingleton.SQLServerSingletonConexion.IDUsuario = i_Cve_UsuarioAuxiliar_
                        Else
                            ConexionSingleton.SQLServerSingletonConexion.IDUsuario = 0
                        End If


                        ConexionSingleton.SQLServerSingletonConexion.NombreEnsamblado = _vistaencabezados
                        ConexionSingleton.SQLServerSingletonConexion.NombreVT = t_nombreModuloEnsambladoAuxiliar_
                        ConexionSingleton.SQLServerSingletonConexion.NombreUsuarioCliente = t_nombreUsuarioAuxiliar_
                        ConexionSingleton.SQLServerSingletonConexion.CuentaPublicaUsuario = t_cuentaPublicaUsuarioAuxiliar_ ' _iespaciotrabajo.MisCredenciales.ClaveUsuario


                        ConexionSingleton.SQLServerSingletonConexion.TipoInstrumentacion = _i_TipoInstrumentacion
                        ConexionSingleton.SQLServerSingletonConexion.IDRecursoSolicitante = _i_Clave_Modulo


                        Dim scriptLecturaSuciaSQl_ As String = Nothing

                        If _activarLecturaSucia Then

                            scriptLecturaSuciaSQl_ =
                                "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; " &
                                "SET ARITHABORT ON; "

                        End If

                        Dim saxidapp = Sax.SaxStatements.GetInstance()

                        If saxidapp.SaxSettings(1).about = "KromBaseWeb" Then

                            ConexionSingleton.SQLServerSingletonConexion.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                            _modalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre  'Conectividad = 2 (Libre) ... IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                            ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaIndividual(
                            scriptLecturaSuciaSQl_ &
                            sqltexto_ & _ordernarResultadosPorColumna)

                            dataset_ = ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Copy()

                        Else

                            ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(
                            scriptLecturaSuciaSQl_ &
                            sqltexto_ & _ordernarResultadosPorColumna)

                            'ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(
                            '    "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; " &
                            '    "SET ARITHABORT ON;" &
                            '    sqltexto_ & _ordernarResultadosPorColumna)

                            dataset_ = ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Copy()

                        End If

                    Case IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                        Dim configuracionaux_ As Configuracion

                        configuracionaux_ = Configuracion.ObtenerInstancia()

                        'If configuracionaux_.Status.Status = Wma.Exceptions.TagWatcher.TypeStatus.Ok Then

                        Dim conexion_ As IConexiones = New Conexiones

                        conexion_.ControladorBaseDatos = IConexiones.Controladores.SQLServer2008

                        conexion_.Contrasena = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.ClaveSQLServerGeneralProduccion)

                        conexion_.Usuario = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.UsuarioSQLServerGeneralProduccion)

                        conexion_.NombreBaseDatos = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.BaseDatosSQLServerProduccion)

                        conexion_.IpServidor = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.DireccionIPServidorSQLServerGeneralProduccion)

                        conexion_.ObjetoDatos = IConexiones.TipoConexion.SqlCommand

                        conexion_.DataSetReciente.Tables.Clear()

                        Dim i_Cve_UsuarioAuxiliar_ As Int32 = 0
                        Dim i_cve_DivisionMiEmpresaAuxiliar_ As Int32 = 0
                        Dim i_cve_aplicacionAuxiliar_ As Int32 = 0
                        Dim t_nombreUsuarioAuxiliar_ As String = Nothing
                        Dim t_nombreModuloEnsambladoAuxiliar_ As String = Nothing
                        Dim t_cuentaPublicaUsuarioAuxiliar_ As String = Nothing

                        If Not _iespaciotrabajo Is Nothing Then

                            If Not _iespaciotrabajo.MisCredenciales Is Nothing Then

                                i_Cve_UsuarioAuxiliar_ = _iespaciotrabajo.MisCredenciales.ClaveUsuario
                                t_cuentaPublicaUsuarioAuxiliar_ = _iespaciotrabajo.MisCredenciales.CredencialUsuario
                                i_cve_DivisionMiEmpresaAuxiliar_ = _iespaciotrabajo.MisCredenciales.DivisionEmpresaria
                                i_cve_aplicacionAuxiliar_ = _iespaciotrabajo.MisCredenciales.Aplicacion
                                t_nombreUsuarioAuxiliar_ = _iespaciotrabajo.MisCredenciales.NombreAutenticacion

                            End If

                        End If

                        If Not _operadorcatalogoconsultas Is Nothing Then
                            t_nombreModuloEnsambladoAuxiliar_ = _operadorcatalogoconsultas
                        End If

                        If _i_Cve_Aplicacion <> 0 Then
                            conexion_.IDAplicacion = _i_Cve_Aplicacion
                        ElseIf i_cve_aplicacionAuxiliar_ <> 0 Then
                            conexion_.IDAplicacion = i_cve_aplicacionAuxiliar_
                        Else
                            conexion_.IDAplicacion = IBitacoras.ClaveTiposAplicacion.SinDefinir
                        End If

                        If _i_Cve_DivisionMiEmpresa <> 0 Then
                            conexion_.ClaveDivisionMiEmpresa = _i_Cve_DivisionMiEmpresa
                        ElseIf i_cve_DivisionMiEmpresaAuxiliar_ <> 0 Then
                            conexion_.ClaveDivisionMiEmpresa = i_cve_DivisionMiEmpresaAuxiliar_
                        Else
                            conexion_.ClaveDivisionMiEmpresa = 0
                        End If

                        If _i_Cve_Usuario <> 0 Then
                            conexion_.IDUsuario = _i_Cve_Usuario
                        ElseIf i_Cve_UsuarioAuxiliar_ <> 0 Then
                            conexion_.IDUsuario = i_Cve_UsuarioAuxiliar_
                        Else
                            conexion_.IDUsuario = 0
                        End If


                        conexion_.NombreUsuarioCliente = t_nombreUsuarioAuxiliar_
                        conexion_.ModuloCliente = t_nombreModuloEnsambladoAuxiliar_

                        conexion_.NombreEnsamblado = _vistaencabezados
                        conexion_.NombreVT = t_nombreModuloEnsambladoAuxiliar_
                        conexion_.NombreUsuarioCliente = t_nombreUsuarioAuxiliar_
                        conexion_.CuentaPublicaUsuario = t_cuentaPublicaUsuarioAuxiliar_ '_iespaciotrabajo.MisCredenciales.ClaveUsuario


                        conexion_.TipoInstrumentacion = _i_TipoInstrumentacion
                        conexion_.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre
                        _modalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre  'Conectividad = 2 (Libre) ... IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre
                        conexion_.IDRecursoSolicitante = _i_Clave_Modulo

                        Dim scriptLecturaSuciaSQl_ As String = Nothing

                        If _activarLecturaSucia Then

                            scriptLecturaSuciaSQl_ =
                                "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; " &
                                "SET ARITHABORT ON; "

                        End If

                        conexion_.EjecutaConsultaIndividual(
                            scriptLecturaSuciaSQl_ &
                            sqltexto_ & _ordernarResultadosPorColumna)
                        dataset_ = conexion_.DataSetReciente.Copy()

                        'Else

                        '    MsgBox("error en la lectura del archivo de configuracion fmk")

                        'End If

                End Select

            Else

                Select Case _tipoescritura

                    Case IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia

                        dataset_.Tables.Add(_registrostemporalesDataTable.Copy)

                    Case IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda

                        dataset_.Tables.Add(_registrostemporalesDataTable.Copy())

                    Case Else

                End Select

            End If

            Return dataset_

        End Function

        'MOP Obsoleta 03/03/2021
        'Public Function ConsultaProduccionObsoleta(Optional ByVal valordefaultbuscado_ As String = "%", _
        '                                   Optional ByVal filtrarcampo_ As String = Nothing, _
        '                                   Optional ByVal rigor_ As String = "like") As DataSet

        '    Dim sqltexto_ As String = Nothing

        '    Dim dataset_ As New DataSet

        '    Select Case _tipoescritura

        '        Case IOperacionesCatalogo.TiposEscritura.Inmediata

        '            Select Case ConexionSingleton.SQLServerSingletonConexion.ObjetoDatos
        '                Case IConexiones.TipoConexion.MySQLCommand

        '                    Select Case _cantidadvisibleregistros

        '                        Case 0

        '                            sqltexto_ = "select " & _
        '                            ExtractorEncabezados() & _
        '                            " from  " & _
        '                            _operadorcatalogoconsultas & _
        '                            " " & AplicaClausulas(valordefaultbuscado_, _
        '                            filtrarcampo_, _
        '                            rigor_)

        '                        Case Else

        '                            sqltexto_ = "select " & _
        '                             ExtractorEncabezados() & _
        '                             " from  " & _
        '                            _operadorcatalogoconsultas & _
        '                            " " & AplicaClausulas(valordefaultbuscado_, _
        '                            filtrarcampo_, _
        '                            rigor_) & " limit " & _cantidadvisibleregistros

        '                    End Select

        '                Case IConexiones.TipoConexion.SqlCommand

        '                    Select Case _cantidadvisibleregistros

        '                        Case 0

        '                            sqltexto_ = "select " & _
        '                            ExtractorEncabezados() & _
        '                            " from  " & _
        '                            _operadorcatalogoconsultas & _
        '                            " " & AplicaClausulas(valordefaultbuscado_, _
        '                            filtrarcampo_, _
        '                            rigor_)


        '                        Case Else

        '                            sqltexto_ = "select top(" & _cantidadvisibleregistros & ") " & _
        '                            ExtractorEncabezados() & _
        '                            " from  " & _
        '                            _operadorcatalogoconsultas & _
        '                            " " & AplicaClausulas(valordefaultbuscado_, _
        '                            filtrarcampo_, _
        '                            rigor_)


        '                    End Select

        '            End Select

        '            _salidallaves = "select " & _
        '                                        _identificadorcatalogo & _
        '                                        " from  " & _
        '                                        _operadorcatalogoconsultas


        '        Case IOperacionesCatalogo.TiposEscritura.MemoriaIntermediaBeta

        '            Dim indice_ As Int32 = 0

        '            For Each registro_ As String In _registrostemporalespresentacion.Values

        '                If sqltexto_ Is Nothing Then

        '                    sqltexto_ = "select " & indice_ & " as clave, " & registro_

        '                Else

        '                    sqltexto_ = sqltexto_ & " union all select " & indice_ & " as clave, " & registro_

        '                End If

        '                indice_ += 1

        '            Next


        '    End Select

        '    If Not sqltexto_ Is Nothing Then

        '        ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

        '        If Not _iespaciotrabajo Is Nothing Then

        '            If Not _iespaciotrabajo.MisCredenciales Is Nothing Then

        '                ConexionSingleton.UsuarioCliente = _iespaciotrabajo.MisCredenciales.CredencialUsuario

        '            End If

        '        Else
        '            ConexionSingleton.UsuarioCliente = _sistema.ConexionSingleton.UsuarioCliente
        '        End If


        '        ConexionSingleton.ModuloCliente = _operadorcatalogoconsultas


        '        '::::::. Modificaciones Webservices

        '        Select Case _modalidadConsulta

        '            Case IOperacionesCatalogo.ModalidadesConsulta.Singleton,
        '                 IOperacionesCatalogo.ModalidadesConsulta.SinDefinir



        '                Dim i_Cve_UsuarioAuxiliar_ As Int32 = 0
        '                Dim i_cve_DivisionMiEmpresaAuxiliar_ As Int32 = 0
        '                Dim i_cve_aplicacionAuxiliar_ As Int32 = 0
        '                Dim t_nombreUsuarioAuxiliar_ As String = Nothing
        '                Dim t_nombreModuloEnsambladoAuxiliar_ As String = Nothing
        '                Dim t_cuentaPublicaUsuarioAuxiliar_ As String = Nothing

        '                If Not _iespaciotrabajo Is Nothing Then

        '                    If Not _iespaciotrabajo.MisCredenciales Is Nothing Then

        '                        'conexion_.IDUsuario = _iespaciotrabajo.MisCredenciales.ClaveUsuario
        '                        'conexion_.ClaveDivisionMiEmpresa = _iespaciotrabajo.MisCredenciales.DivisionEmpresaria

        '                        i_Cve_UsuarioAuxiliar_ = _iespaciotrabajo.MisCredenciales.ClaveUsuario
        '                        t_cuentaPublicaUsuarioAuxiliar_ = _iespaciotrabajo.MisCredenciales.CredencialUsuario
        '                        i_cve_DivisionMiEmpresaAuxiliar_ = _iespaciotrabajo.MisCredenciales.DivisionEmpresaria
        '                        i_cve_aplicacionAuxiliar_ = _iespaciotrabajo.MisCredenciales.Aplicacion
        '                        t_nombreUsuarioAuxiliar_ = _iespaciotrabajo.MisCredenciales.NombreAutenticacion

        '                    End If

        '                End If

        '                If Not _operadorcatalogoconsultas Is Nothing Then
        '                    t_nombreModuloEnsambladoAuxiliar_ = _operadorcatalogoconsultas
        '                End If

        '                If _i_Cve_Aplicacion <> 0 Then
        '                    ConexionSingleton.SQLServerSingletonConexion.IDAplicacion = _i_Cve_Aplicacion
        '                ElseIf i_cve_aplicacionAuxiliar_ <> 0 Then
        '                    ConexionSingleton.SQLServerSingletonConexion.IDAplicacion = i_cve_aplicacionAuxiliar_
        '                Else
        '                    ConexionSingleton.SQLServerSingletonConexion.IDAplicacion = IBitacoras.ClaveTiposAplicacion.SinDefinir
        '                End If

        '                If _i_Cve_DivisionMiEmpresa <> 0 Then
        '                    ConexionSingleton.SQLServerSingletonConexion.ClaveDivisionMiEmpresa = _i_Cve_DivisionMiEmpresa
        '                ElseIf i_cve_DivisionMiEmpresaAuxiliar_ <> 0 Then
        '                    ConexionSingleton.SQLServerSingletonConexion.ClaveDivisionMiEmpresa = i_cve_DivisionMiEmpresaAuxiliar_
        '                Else
        '                    ConexionSingleton.SQLServerSingletonConexion.ClaveDivisionMiEmpresa = 0
        '                End If

        '                If _i_Cve_Usuario <> 0 Then
        '                    ConexionSingleton.SQLServerSingletonConexion.IDUsuario = _i_Cve_Usuario
        '                ElseIf i_Cve_UsuarioAuxiliar_ <> 0 Then
        '                    ConexionSingleton.SQLServerSingletonConexion.IDUsuario = i_Cve_UsuarioAuxiliar_
        '                Else
        '                    ConexionSingleton.SQLServerSingletonConexion.IDUsuario = 0
        '                End If


        '                'ConexionSingleton.SQLServerSingletonConexion.NombreUsuarioCliente = t_nombreUsuarioAuxiliar_
        '                'ConexionSingleton.SQLServerSingletonConexion.ModuloCliente = t_nombreModuloEnsambladoAuxiliar_

        '                ConexionSingleton.SQLServerSingletonConexion.NombreEnsamblado = _vistaencabezados
        '                ConexionSingleton.SQLServerSingletonConexion.NombreVT = t_nombreModuloEnsambladoAuxiliar_
        '                ConexionSingleton.SQLServerSingletonConexion.NombreUsuarioCliente = t_nombreUsuarioAuxiliar_
        '                ConexionSingleton.SQLServerSingletonConexion.CuentaPublicaUsuario = t_cuentaPublicaUsuarioAuxiliar_ ' _iespaciotrabajo.MisCredenciales.ClaveUsuario


        '                ConexionSingleton.SQLServerSingletonConexion.TipoInstrumentacion = _i_TipoInstrumentacion
        '                ConexionSingleton.SQLServerSingletonConexion.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre
        '                _modalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre  'Conectividad = 2 (Libre) ... IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre
        '                ConexionSingleton.SQLServerSingletonConexion.IDRecursoSolicitante = _i_Clave_Modulo

        '                ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(
        '                    "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; " & _
        '                    "SET ARITHABORT ON;" & _
        '                    sqltexto_ & _ordernarResultadosPorColumna)

        '                dataset_ = ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Copy()

        '            Case IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

        '                ':::::::::::::::::::REGISTRO DE BITÁCORA::::::::::::::::::

        '                'BitacoraCapaDatos
        '                '       Sub New(ByVal disparo_ As String, _ 
        '                '               ByVal misCredenciales_ As ICredenciales, _
        '                '               ByVal permiso_ As Int32, _
        '                '               ByVal mensaje_ As String, _
        '                '               ByVal parametro_ As String, _
        '                '               ByVal tipoBitacora_ As IBitacoras.TiposBitacora, _
        '                '               ByVal tipoSuceso_ As IBitacoras.TiposSucesos, _
        '                '_
        '                '               ByVal i_Cve_Usuario_ As Int32, _
        '                '               ByVal i_Cve_DivisionMiEmpresa_ As Int32, _
        '                '               ByVal i_Cve_Estatus_ As IBitacoras.EstadosConsultaAvanzada, _
        '                '               ByVal f_FechaInicio_ As DateTime, _
        '                '               ByVal i_Cve_TipoAplicacion_ As IBitacoras.ClaveTiposAplicacion, _
        '                '               ByVal i_Instrumentacion_ As IBitacoras.TiposInstrumentacion, _
        '                '               ByVal modalidadConsulta_ As IOperacionesCatalogo.ModalidadesConsulta, _
        '                '               ByVal i_Cve_RecursoSolicitante_ As Int32
        '                '               )

        '                '::::::::::::::::Inserción inicial:::::::::::::::::::::::::::::

        '                'Public Sub DocumentaBitacoraCapaDatos(ByVal estatus_ As IBitacoras.EstadosConsultaAvanzada,
        '                '              ByVal f_Fecha_ As DateTime,
        '                '              Optional ByVal tiempoTotalRespuesta_ As Double = 0,
        '                '              Optional ByVal tagWatcherMensaje_ As TagWatcher = Nothing)

        '                ':::::::::::::::::::::Actualización de transacción al finalizar::::::::::::

        '                'Public Sub DocumentaBitacoraCapaDatos(ByVal estatus_ As IBitacoras.EstadosConsultaAvanzada,
        '                '              ByVal f_Fecha_ As DateTime,
        '                '              Optional ByVal tiempoTotalRespuesta_ As Double = 0,
        '                '              Optional ByVal tagWatcherMensaje_ As TagWatcher = Nothing)
        '                'IBitacoras.EstadosConsultaAvanzada.EnProceso
        '                '._f_FechaHoraFinal = f_Fecha_

        '                '._i_TiempoRespuestaTotal = tiempoTotalRespuesta_

        '                '._tagWatcherMensaje = tagWatcherMensaje_

        '                ':::::::::::::::::::::::::::::::::::::::::::::::::::::::::

        '                Dim configuracionaux_ As Configuracion

        '                configuracionaux_ = Configuracion.ObtenerInstancia()
        '                'Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.ClaveSQLServerGeneralProduccion)

        '                'Dim configuracionaux_ As New Configuracion

        '                ' conexion_.Contrasena = configuracionaux_.ConstanteGlobal(Configuracion.DatosGlobalesSistema.ClaveSQLServerGeneralProduccion)

        '                If configuracionaux_.Status.Status = Wma.Exceptions.TagWatcher.TypeStatus.Ok Then

        '                    Dim conexion_ As IConexiones = New Conexiones

        '                    conexion_.ControladorBaseDatos = IConexiones.Controladores.SQLServer2008

        '                    conexion_.Contrasena = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.ClaveSQLServerGeneralProduccion)

        '                    conexion_.Usuario = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.UsuarioSQLServerGeneralProduccion)

        '                    conexion_.NombreBaseDatos = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.BaseDatosSQLServerProduccion)

        '                    conexion_.IpServidor = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.DireccionIPServidorSQLServerGeneralProduccion)

        '                    conexion_.ObjetoDatos = IConexiones.TipoConexion.SqlCommand

        '                    conexion_.DataSetReciente.Tables.Clear()

        '                    'Dim i_Cve_Usuario_ As Int32 = 139
        '                    'Dim i_Cve_DivisionMiEmpresa_ As Int32 = 1
        '                    'Dim i_Cve_TipoAplicacion_ As IBitacoras.ClaveTiposAplicacion = IBitacoras.ClaveTiposAplicacion.AplicacionWebExtranet
        '                    'Dim i_TipoInstrumentacion_ As IBitacoras.TiposInstrumentacion = IBitacoras.TiposInstrumentacion.GestorIOperaciones
        '                    'Dim i_ModalidaConsulta_ As basedatos.Operaciones.IOperacionesCatalogo.ModalidadesConsulta = basedatos.Operaciones.IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre
        '                    'Dim i_Cve_RecursoSolicitante_ As Int32 = 33

        '                    Dim i_Cve_UsuarioAuxiliar_ As Int32 = 0
        '                    Dim i_cve_DivisionMiEmpresaAuxiliar_ As Int32 = 0
        '                    Dim i_cve_aplicacionAuxiliar_ As Int32 = 0
        '                    Dim t_nombreUsuarioAuxiliar_ As String = Nothing
        '                    Dim t_nombreModuloEnsambladoAuxiliar_ As String = Nothing
        '                    Dim t_cuentaPublicaUsuarioAuxiliar_ As String = Nothing

        '                    If Not _iespaciotrabajo Is Nothing Then

        '                        If Not _iespaciotrabajo.MisCredenciales Is Nothing Then

        '                            'conexion_.IDUsuario = _iespaciotrabajo.MisCredenciales.ClaveUsuario
        '                            'conexion_.ClaveDivisionMiEmpresa = _iespaciotrabajo.MisCredenciales.DivisionEmpresaria

        '                            i_Cve_UsuarioAuxiliar_ = _iespaciotrabajo.MisCredenciales.ClaveUsuario
        '                            t_cuentaPublicaUsuarioAuxiliar_ = _iespaciotrabajo.MisCredenciales.CredencialUsuario
        '                            i_cve_DivisionMiEmpresaAuxiliar_ = _iespaciotrabajo.MisCredenciales.DivisionEmpresaria
        '                            i_cve_aplicacionAuxiliar_ = _iespaciotrabajo.MisCredenciales.Aplicacion
        '                            t_nombreUsuarioAuxiliar_ = _iespaciotrabajo.MisCredenciales.NombreAutenticacion

        '                        End If

        '                    End If

        '                    If Not _operadorcatalogoconsultas Is Nothing Then
        '                        t_nombreModuloEnsambladoAuxiliar_ = _operadorcatalogoconsultas
        '                    End If

        '                    If _i_Cve_Aplicacion <> 0 Then
        '                        conexion_.IDAplicacion = _i_Cve_Aplicacion
        '                    ElseIf i_cve_aplicacionAuxiliar_ <> 0 Then
        '                        conexion_.IDAplicacion = i_cve_aplicacionAuxiliar_
        '                    Else
        '                        conexion_.IDAplicacion = IBitacoras.ClaveTiposAplicacion.SinDefinir
        '                    End If

        '                    If _i_Cve_DivisionMiEmpresa <> 0 Then
        '                        conexion_.ClaveDivisionMiEmpresa = _i_Cve_DivisionMiEmpresa
        '                    ElseIf i_cve_DivisionMiEmpresaAuxiliar_ <> 0 Then
        '                        conexion_.ClaveDivisionMiEmpresa = i_cve_DivisionMiEmpresaAuxiliar_
        '                    Else
        '                        conexion_.ClaveDivisionMiEmpresa = 0
        '                    End If

        '                    If _i_Cve_Usuario <> 0 Then
        '                        conexion_.IDUsuario = _i_Cve_Usuario
        '                    ElseIf i_Cve_UsuarioAuxiliar_ <> 0 Then
        '                        conexion_.IDUsuario = i_Cve_UsuarioAuxiliar_
        '                    Else
        '                        conexion_.IDUsuario = 0
        '                    End If


        '                    conexion_.NombreUsuarioCliente = t_nombreUsuarioAuxiliar_
        '                    conexion_.ModuloCliente = t_nombreModuloEnsambladoAuxiliar_

        '                    conexion_.NombreEnsamblado = _vistaencabezados
        '                    conexion_.NombreVT = t_nombreModuloEnsambladoAuxiliar_
        '                    conexion_.NombreUsuarioCliente = t_nombreUsuarioAuxiliar_
        '                    conexion_.CuentaPublicaUsuario = t_cuentaPublicaUsuarioAuxiliar_ '_iespaciotrabajo.MisCredenciales.ClaveUsuario


        '                    conexion_.TipoInstrumentacion = _i_TipoInstrumentacion
        '                    conexion_.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre
        '                    _modalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre  'Conectividad = 2 (Libre) ... IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre
        '                    conexion_.IDRecursoSolicitante = _i_Clave_Modulo


        '                    conexion_.EjecutaConsultaIndividual(
        '                        "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; " & _
        '                        "SET ARITHABORT ON;" & _
        '                        sqltexto_ & _ordernarResultadosPorColumna)
        '                    dataset_ = conexion_.DataSetReciente.Copy()

        '                Else

        '                    MsgBox("error en la lectura del archivo de configuracion fmk")

        '                End If

        '        End Select
        '    Else

        '        Select Case _tipoescritura

        '            Case IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia

        '                '_registrostemporalesDataTable.filter()

        '                'Dim foundRows() As Data.DataRow

        '                'foundRows = _registrostemporalesDataTable.Select("Id=" & indice_)

        '                'RegistrosTemporalesDataTable.Rows.Remove(foundRows(0))

        '                dataset_.Tables.Add(_registrostemporalesDataTable.Copy)

        '            Case IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda

        '                dataset_.Tables.Add(_registrostemporalesDataTable.Copy())

        '            Case Else

        '        End Select

        '    End If

        '    Return dataset_

        'End Function

        Public Function GeneraEncabezadosCatalogo() As DataSet

            ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

            If Not _iespaciotrabajo Is Nothing Then

                If Not _iespaciotrabajo.MisCredenciales Is Nothing Then

                    ConexionSingleton.UsuarioCliente = _iespaciotrabajo.MisCredenciales.CredencialUsuario

                End If

            Else

                ConexionSingleton.UsuarioCliente = _sistema.ConexionSingleton.UsuarioCliente

            End If

            ConexionSingleton.ModuloCliente = _operadorcatalogoconsultas

            'Cambio importante 08/01/2019, Pedro Bautista. ( A prueba se mueve esta linea a otro contexto) ._.
            'ObtenerInstanciaSingleton()

            '::::::::::::Modificaciones Webservices

            Dim dataset_ As New DataSet

            Select Case _modalidadConsulta

                Case IOperacionesCatalogo.ModalidadesConsulta.Singleton,
                     IOperacionesCatalogo.ModalidadesConsulta.SinDefinir

                    '::::::::::: FIN DE LA OPTIMIZACIÓN

                    Dim ev_ As New EnvironmentViews()

                    Dim messaje_ As New TagWatcher

                    messaje_ = ev_.GetEnvironmentViewAsJSON(_vistaencabezados)

                    'Buscamos la vista en modalidad estática, para acelerar el proceso
                    If messaje_.Status = TagWatcher.TypeStatus.Ok Then

                        Dim datasetEstatico_ As New DataSet

                        datasetEstatico_.Tables.Add(messaje_.ObjectReturned)

                        dataset_ = datasetEstatico_

                    Else

                        ObtenerInstanciaSingleton()

                        'Aquí podemos solicitar se genere para que solo la primera vez sea necesario extraer de la base de datos
                        ev_.ConvertEVTableToStatic(_vistaencabezados) 'Then : MsgBox("done!") : End If

                        Dim i_Cve_UsuarioAuxiliar_ As Int32 = 0
                        Dim i_cve_DivisionMiEmpresaAuxiliar_ As Int32 = 0
                        Dim i_cve_aplicacionAuxiliar_ As Int32 = 0
                        Dim t_nombreUsuarioAuxiliar_ As String = Nothing
                        Dim t_nombreModuloEnsambladoAuxiliar_ As String = Nothing
                        Dim t_cuentaPublicaUsuarioAuxiliar_ As String = Nothing

                        If Not _iespaciotrabajo Is Nothing Then

                            If Not _iespaciotrabajo.MisCredenciales Is Nothing Then

                                'conexion_.IDUsuario = _iespaciotrabajo.MisCredenciales.ClaveUsuario
                                'conexion_.ClaveDivisionMiEmpresa = _iespaciotrabajo.MisCredenciales.DivisionEmpresaria

                                i_Cve_UsuarioAuxiliar_ = _iespaciotrabajo.MisCredenciales.ClaveUsuario
                                t_cuentaPublicaUsuarioAuxiliar_ = _iespaciotrabajo.MisCredenciales.CredencialUsuario
                                i_cve_DivisionMiEmpresaAuxiliar_ = _iespaciotrabajo.MisCredenciales.DivisionEmpresaria
                                i_cve_aplicacionAuxiliar_ = _iespaciotrabajo.MisCredenciales.Aplicacion
                                t_nombreUsuarioAuxiliar_ = _iespaciotrabajo.MisCredenciales.NombreAutenticacion

                            End If

                        End If

                        If Not _operadorcatalogoconsultas Is Nothing Then
                            t_nombreModuloEnsambladoAuxiliar_ = _operadorcatalogoconsultas
                        End If

                        If _i_Cve_Aplicacion <> 0 Then
                            ConexionSingleton.SQLServerSingletonConexion.IDAplicacion = _i_Cve_Aplicacion
                        ElseIf i_cve_aplicacionAuxiliar_ <> 0 Then
                            ConexionSingleton.SQLServerSingletonConexion.IDAplicacion = i_cve_aplicacionAuxiliar_
                        Else
                            ConexionSingleton.SQLServerSingletonConexion.IDAplicacion = IBitacoras.ClaveTiposAplicacion.SinDefinir
                        End If

                        If _i_Cve_DivisionMiEmpresa <> 0 Then
                            ConexionSingleton.SQLServerSingletonConexion.ClaveDivisionMiEmpresa = _i_Cve_DivisionMiEmpresa
                        ElseIf i_cve_DivisionMiEmpresaAuxiliar_ <> 0 Then
                            ConexionSingleton.SQLServerSingletonConexion.ClaveDivisionMiEmpresa = i_cve_DivisionMiEmpresaAuxiliar_
                        Else
                            ConexionSingleton.SQLServerSingletonConexion.ClaveDivisionMiEmpresa = 0
                        End If

                        If _i_Cve_Usuario <> 0 Then
                            ConexionSingleton.SQLServerSingletonConexion.IDUsuario = _i_Cve_Usuario
                        ElseIf i_Cve_UsuarioAuxiliar_ <> 0 Then
                            ConexionSingleton.SQLServerSingletonConexion.IDUsuario = i_Cve_UsuarioAuxiliar_
                        Else
                            ConexionSingleton.SQLServerSingletonConexion.IDUsuario = 0
                        End If



                        ConexionSingleton.SQLServerSingletonConexion.IDUsuario = _i_Cve_Usuario
                        ConexionSingleton.SQLServerSingletonConexion.IDAplicacion = _i_Cve_Aplicacion
                        ConexionSingleton.SQLServerSingletonConexion.IDRecursoSolicitante = _i_Clave_Modulo
                        ConexionSingleton.SQLServerSingletonConexion.ClaveDivisionMiEmpresa = _i_Cve_DivisionMiEmpresa
                        ConexionSingleton.SQLServerSingletonConexion.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.Singleton
                        ConexionSingleton.SQLServerSingletonConexion.TipoInstrumentacion = _i_TipoInstrumentacion


                        ConexionSingleton.SQLServerSingletonConexion.NombreEnsamblado = _vistaencabezados
                        ConexionSingleton.SQLServerSingletonConexion.NombreVT = _operadorcatalogoconsultas
                        ConexionSingleton.SQLServerSingletonConexion.NombreUsuarioCliente = t_nombreUsuarioAuxiliar_
                        ConexionSingleton.SQLServerSingletonConexion.CuentaPublicaUsuario = t_cuentaPublicaUsuarioAuxiliar_ '_iespaciotrabajo.MisCredenciales.ClaveUsuario


                        ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta("select * from " &
                                                                                     _vistaencabezados)

                        dataset_ = ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Copy()


                    End If

                Case IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                    '::::::::::: FIN DE LA OPTIMIZACIÓN

                    Dim ev_ As New EnvironmentViews()

                    Dim messaje_ As New TagWatcher

                    messaje_ = ev_.GetEnvironmentViewAsJSON(_vistaencabezados)

                    'Buscamos la vista en modalidad estática, para acelerar el proceso
                    If messaje_.Status = TagWatcher.TypeStatus.Ok Then

                        Dim datasetEstatico_ As New DataSet

                        datasetEstatico_.Tables.Add(messaje_.ObjectReturned)

                        dataset_ = datasetEstatico_

                    Else
                        'Aquí podemos solicitar se genere para que solo la primera vez sea necesario extraer de la base de datos
                        ev_.ConvertEVTableToStatic(_vistaencabezados) 'Then : MsgBox("done!") : End If

                        Dim configuracionaux_ As New Configuracion

                        Dim conexion_ As IConexiones = New Conexiones
                        conexion_.ControladorBaseDatos = IConexiones.Controladores.SQLServer2008
                        conexion_.Contrasena = configuracionaux_.ConstanteGlobal(Configuracion.DatosGlobalesSistema.ClaveSQLServerGeneralProduccion)
                        conexion_.Usuario = configuracionaux_.ConstanteGlobal(Configuracion.DatosGlobalesSistema.UsuarioSQLServerGeneralProduccion)
                        conexion_.NombreBaseDatos = configuracionaux_.ConstanteGlobal(Configuracion.DatosGlobalesSistema.BaseDatosSQLServerProduccion)
                        conexion_.IpServidor = configuracionaux_.ConstanteGlobal(Configuracion.DatosGlobalesSistema.DireccionIPServidorSQLServerGeneralProduccion)

                        Dim i_Cve_UsuarioAuxiliar_ As Int32 = 0
                        Dim i_cve_DivisionMiEmpresaAuxiliar_ As Int32 = 0
                        Dim i_cve_aplicacionAuxiliar_ As Int32 = 0
                        Dim t_nombreUsuarioAuxiliar_ As String = Nothing
                        Dim t_nombreModuloEnsambladoAuxiliar_ As String = Nothing
                        Dim t_cuentaPublicaUsuarioAuxiliar_ As String = Nothing

                        If Not _iespaciotrabajo Is Nothing Then

                            If Not _iespaciotrabajo.MisCredenciales Is Nothing Then
                                i_Cve_UsuarioAuxiliar_ = _iespaciotrabajo.MisCredenciales.ClaveUsuario
                                t_cuentaPublicaUsuarioAuxiliar_ = _iespaciotrabajo.MisCredenciales.CredencialUsuario
                                i_cve_DivisionMiEmpresaAuxiliar_ = _iespaciotrabajo.MisCredenciales.DivisionEmpresaria
                                i_cve_aplicacionAuxiliar_ = _iespaciotrabajo.MisCredenciales.Aplicacion
                                t_nombreUsuarioAuxiliar_ = _iespaciotrabajo.MisCredenciales.NombreAutenticacion

                            End If

                        End If

                        If Not _operadorcatalogoconsultas Is Nothing Then
                            t_nombreModuloEnsambladoAuxiliar_ = _operadorcatalogoconsultas
                        End If

                        If _i_Cve_Aplicacion <> 0 Then
                            ConexionSingleton.SQLServerSingletonConexion.IDAplicacion = _i_Cve_Aplicacion
                        ElseIf i_cve_aplicacionAuxiliar_ <> 0 Then
                            ConexionSingleton.SQLServerSingletonConexion.IDAplicacion = i_cve_aplicacionAuxiliar_
                        Else
                            ConexionSingleton.SQLServerSingletonConexion.IDAplicacion = IBitacoras.ClaveTiposAplicacion.SinDefinir
                        End If

                        If _i_Cve_DivisionMiEmpresa <> 0 Then
                            ConexionSingleton.SQLServerSingletonConexion.ClaveDivisionMiEmpresa = _i_Cve_DivisionMiEmpresa
                        ElseIf i_cve_DivisionMiEmpresaAuxiliar_ <> 0 Then
                            ConexionSingleton.SQLServerSingletonConexion.ClaveDivisionMiEmpresa = i_cve_DivisionMiEmpresaAuxiliar_
                        Else
                            ConexionSingleton.SQLServerSingletonConexion.ClaveDivisionMiEmpresa = 0
                        End If

                        If _i_Cve_Usuario <> 0 Then
                            ConexionSingleton.SQLServerSingletonConexion.IDUsuario = _i_Cve_Usuario
                        ElseIf i_Cve_UsuarioAuxiliar_ <> 0 Then
                            ConexionSingleton.SQLServerSingletonConexion.IDUsuario = i_Cve_UsuarioAuxiliar_
                        Else
                            ConexionSingleton.SQLServerSingletonConexion.IDUsuario = 0
                        End If

                        conexion_.IDUsuario = _i_Cve_Usuario
                        conexion_.IDAplicacion = _i_Cve_Aplicacion
                        conexion_.IDRecursoSolicitante = _i_Clave_Modulo
                        conexion_.ClaveDivisionMiEmpresa = _i_Cve_DivisionMiEmpresa
                        conexion_.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre
                        conexion_.TipoInstrumentacion = _i_TipoInstrumentacion

                        conexion_.NombreEnsamblado = _vistaencabezados
                        conexion_.NombreVT = t_nombreModuloEnsambladoAuxiliar_
                        conexion_.NombreUsuarioCliente = t_nombreUsuarioAuxiliar_
                        conexion_.CuentaPublicaUsuario = t_cuentaPublicaUsuarioAuxiliar_ '_iespaciotrabajo.MisCredenciales.ClaveUsuario

                        conexion_.ObjetoDatos = IConexiones.TipoConexion.SqlCommand

                        conexion_.DataSetReciente.Tables.Clear()

                        conexion_.EjecutaConsultaIndividual("select * from " & _vistaencabezados)

                        dataset_ = conexion_.DataSetReciente.Copy()

                    End If

                    '::::::::::: FIN DE LA OPTIMIZACIÓN

            End Select

            Return dataset_

        End Function

        Public Sub ListaRegistrosIOperacionesAdd(ByVal registroIOperacion_ As RegistroIOperaciones)

            _listaRegistros.Add(registroIOperacion_)

        End Sub

        ' Actualiza al valor real de las caracteristicas que se configuran para reflejarse, se hace por el tema de los campos que se deben replicar y estan en otros contextos
        Private Function RefinarCaracteristicas()

            Dim registroDatos_ As BsonDocument = New BsonDocument()

            'If _valorindiceseleccionado = "-1" Then

            '    Return registroDatos_

            'End If

            Dim caracteristicasRefinadas_ As Dictionary(Of Integer, ICaracteristica) = _icaracteristicas

            Dim listaCaracteristicasReflejar_ As New List(Of String)

            For Each caracteristica_ As ICaracteristica In _icaracteristicas.Values

                If caracteristica_.Reflejar = 1 Then

                    listaCaracteristicasReflejar_.Add(caracteristica_.Nombre)

                End If

            Next

            Dim iOperaciones_ As IOperacionesCatalogo

            iOperaciones_ = _sistema.EnsamblaModulo(_nombreToken)

            With iOperaciones_

                .ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.Singleton

                .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Consulta)

                .EspacioTrabajo = _iespaciotrabajo

                'Se evalua el tipo de operación para ejecutar el tipo de clausula libre
                Select Case _tipoOperacionSQL

                    Case IOperacionesCatalogo.TiposOperacionSQL.Insercion

                        '' Este segmento se iba a consumir para la insercicón masiva
                        'If _valoresIndiceInsertados.Count > 1 Then

                        '    .ClausulasLibres = " AND " & .IdentificadorCatalogo & " IN (" & String.Join(",", _valoresIndiceInsertados) & ")"

                        'Else

                        '    .ClausulasLibres = " AND " & .IdentificadorCatalogo & " = " & _valorindiceseleccionado

                        'End If

                        .ClausulasLibres = " AND " & .IdentificadorCatalogo & " = " & _valorindiceseleccionado

                    Case IOperacionesCatalogo.TiposOperacionSQL.Modificar

                        If _keyValues IsNot Nothing Then

                            If _keyValues.Count > 1 Then

                                .ClausulasLibres = " AND " & .IdentificadorCatalogo & " IN (" & _valorindiceseleccionado & ")"

                            Else

                                .ClausulasLibres = " AND " & .IdentificadorCatalogo & " = " & _valorindiceseleccionado

                            End If


                        End If

                    Case Else

                        'NOT IMPLEMENTED

                End Select

                .GenerarVista()

            End With

            If _sistema.TieneResultados(iOperaciones_) Then

                ' Asigna los valores a los campos con la propiedad de reflejo
                For Each caracteristica_ As ICaracteristica In caracteristicasRefinadas_.Values

                    If listaCaracteristicasReflejar_.Contains(caracteristica_.Nombre) Then

                        caracteristica_.ValorAsignado = iOperaciones_.Vista(0, caracteristica_.Nombre, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                    End If

                Next

                Select Case _tipoOperacionSQL

                    Case IOperacionesCatalogo.TiposOperacionSQL.Insercion

                        ' Echar un ojo aquí 
                        For Each caracteristica_ As ICaracteristica In caracteristicasRefinadas_.Values

                            If caracteristica_.Llave = ICaracteristica.TipoLlave.Primaria Then

                                caracteristica_.ValorAsignado = iOperaciones_.Vista(0, caracteristica_.Nombre, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                            End If

                        Next

                        Select Case _reflejarEn

                            Case IEnlaceDatos.DestinosParaReplicacion.NoSQLMongoDB

                                For Each caracteristica_ As ICaracteristica In caracteristicasRefinadas_.Values

                                    If caracteristica_.PuedeInsertar = ICaracteristica.TiposRigorDatos.Si Or
                                        caracteristica_.PuedeInsertar = ICaracteristica.TiposRigorDatos.Opcional Or
                                        caracteristica_.Llave = ICaracteristica.TipoLlave.Primaria Or
                                        caracteristica_.Reflejar = 1 Then

                                        EnvolturaDirectaTipo(caracteristica_.Nombre, caracteristica_.ValorAsignado, caracteristica_.TipoDato, registroDatos_)

                                    End If

                                Next

                            Case Else

                                'NOT IMPLEMENTED

                        End Select

                    Case IOperacionesCatalogo.TiposOperacionSQL.Modificar

                        Select Case _reflejarEn

                            Case IEnlaceDatos.DestinosParaReplicacion.NoSQLMongoDB

                                For Each caracteristica_ As ICaracteristica In caracteristicasRefinadas_.Values

                                    If caracteristica_.PuedeModificar = ICaracteristica.TiposRigorDatos.Si Or
                                        caracteristica_.PuedeModificar = ICaracteristica.TiposRigorDatos.Opcional Or
                                        caracteristica_.Llave = ICaracteristica.TipoLlave.Primaria Or
                                        caracteristica_.Reflejar = 1 Then

                                        EnvolturaDirectaTipo(caracteristica_.Nombre, caracteristica_.ValorAsignado, caracteristica_.TipoDato, registroDatos_)

                                    End If

                                Next

                            Case Else

                                'NOT IMPLEMENTED

                        End Select

                    Case Else

                        ' Not implemented

                End Select

            End If

            Return registroDatos_

        End Function

        Private Function ArmaBulkParaTransaccion()

            Dim listaRegistroDatos_ As New List(Of BsonDocument)

            Select Case _tipoOperacionSQL

                Case IOperacionesCatalogo.TiposOperacionSQL.Insercion

                    ' La variable _listaRegistros solo se encuentra llena cuando viene la petición a traves del enlace
                    If Not _listaRegistros Is Nothing And _listaRegistros.Count > 0 Then

                        For Each registroIOperaciones_ As RegistroIOperaciones In _listaRegistros

                            Dim bsonDocument_ As New BsonDocument

                            Dim claveInsertada_ As Int64 = _valoresIndiceInsertados.Item(_listaRegistros.IndexOf(registroIOperaciones_))

                            EnvolturaDirectaTipo(_identificadorcatalogo, claveInsertada_, ICaracteristica.TiposCaracteristica.cInt32, bsonDocument_)

                            For Each caracteristica_ As ICaracteristica In registroIOperaciones_.Caracteristicas

                                EnvolturaDirectaTipo(caracteristica_.Nombre, caracteristica_.ValorAsignado, caracteristica_.TipoDato, bsonDocument_)

                            Next

                            listaRegistroDatos_.Add(bsonDocument_)

                        Next

                    End If

                Case IOperacionesCatalogo.TiposOperacionSQL.Modificar

                    ' Not implemented

                Case Else

                    ' Not implemented

            End Select

            Return listaRegistroDatos_

        End Function

#End Region

    End Class

End Namespace
