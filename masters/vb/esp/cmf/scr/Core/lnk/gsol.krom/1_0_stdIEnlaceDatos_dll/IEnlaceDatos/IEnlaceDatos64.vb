Imports Wma.Exceptions
Imports gsol.BaseDatos.Operaciones

Imports gsol.basededatos
Imports Syn.Documento
Imports MongoDB.Driver
Imports System.Threading.Tasks
Imports MongoDB.Bson
Imports Syn.Operaciones

Namespace gsol.krom

    Public Interface IEnlaceDatos : Inherits IDisposable

#Region "Atributos"
        Enum RolNames
            undefined = 0
            bigdata = 1
            bigdataglobals = 2
            bigdatainferencemod = 4
            bigdatasettings = 5
            replicaDocs = 6
            master = 7
            dimensional = 8
            log = 9
            Catalogs = 10
        End Enum

#End Region

#Region "Constantes"

#End Region

#Region "Enum"

        'ok
        Enum TiposGestionOperativa
            AccesoCuentaGastos
            AccesoOperativo
            AccesoOperativoYCuentaGastos
        End Enum

        'ok
        Enum TiposDimension
            Seguimiento = 1
            ReporteIndicadoresLDAHidrocarburos = 2
            Vt025SeguimientoPendientesKBW = 3
            SeguimientoKBW = 4
            LDAHidrocarburos = 5
            ReporteLDAHidrocarburos = 6
            vt025EstadoCuentaOperaciones = 7
            Vt022ReporteEstandarHidrocarburos = 8
            CopiasSimples = 9
            SinDefinir = 10
            CuentaGastos = 11
            Facturas = 12
            Mercancias = 13
            Referencias = 14
            ExtranetGeneralOperaciones = 15
            Ext022ReferenciasAgenciasAduanales = 16
            Pedimentos = 17
            Contenedores = 18
            Fracciones = 19
            ImpuestosPedimento = 20
            FacturacionAgenciaAduanal = 21
            PagosDeTercerosAddendas = 22
            CostosDirectos = 23
            OtrosCostos = 24
            CostosExtemporaneos = 25
            ReporteEstandarPedimentos = 26
            Vt022ConsultaCuentaGastos = 27
            Vt022ConsultaOperaciones = 28
            ReporteEstandarFracciones = 29
            ReporteEstandarFacturas = 30
            ReporteEstandarMercancias = 31
            ReporteEstandarCuentaGastos = 32
            ConsultaOperacionesFreightForwarder = 33
            ReporteEstandarOperacionesFreightForwarder = 34
            KPIOperaciones = 35
            ConsultaOperacionesVivas = 36
            ConsultaCuentaGastosFreightForwarder = 37
            ReporteEstandarCuentaGastosFreightForwarder = 38
            Responsables = 39
            Causales = 40
            Vt026CausalesTracking = 41
            Vt026Responsables = 42
            RegistroCausales = 43
            Vt026GeneralCausales = 44
            BancosSATTokenDimencion = 45
            BancosSAT2 = 46
            CodigoAgrupadorSATTokenDimencion = 47
            CodigoAgrupadorSAT = 48
            Vt009MaestroOperacionesOtrosCampos = 49
            Vt009MaestroOperaciones = 50
            ConfiguracionBaseDeDatos = 51
            VT000CamposBaseDatos = 52
            Vt014CuentaGastosOtrosCampos = 53
            FormulariosFeedBack = 54
            CategoriasFormularios = 55
            SubCategoriasFormularios = 56
            TiposPreguntas = 57
            FormatosValidaciones = 58
            ReglasValidaciones = 59
            Ejecutivos = 60
            EjecutivosMiEmpresa = 61
            Anexo22 = 62
            Vt022AduanaSeccionA01 = 63
            Vt022ClavesPedimentoA02 = 64
            Vt022MediosTransporteA03 = 65
            Vt022PaisesA04 = 66
            Vt022MonedasA05 = 67
            Vt022RecintosFiscalizadosA06 = 68
            Vt022UnidadesMedidaA07 = 69
            Vt022IdentificadoresA08 = 70
            Vt022RegulacionesRestriccionesNoArancelariasA09 = 71
            Vt022TiposContenedoresVehiculosTransporteA10 = 72
            Vt022MetodosValoracionA11 = 73
            Vt022ContribucionesA12 = 74
            Vt022FormasPagoA13 = 75
            Vt022TerminosFacturacionA14 = 76
            Vt022DestinosMercanciasA15 = 77
            Vt022RegimenesA16 = 78
            Vt022TiposTasasA18 = 79
            Vt022RecintosFiscalizadosEstrategicosA21 = 80
            Vt000Domicilios = 81
            Vt000Personas = 82
            Vt022FacturaComercialOtrosCampos = 83
            Vt022MercanciasOtrosCampos = 84
            Clientes = 85
            Vt001ClientesEmpresaOtrosCampos = 86
            EtiquetasRoles = 87
            Vt000EtiquetasRoles = 88
            DocumentoElectronico = 600
        End Enum

        'ok
        Enum FormatosRespuesta
            Automatico
            ObjetoSimple
            IOperaciones
            ObjetoXML
            ObjetoJson
            ObjetoXLS
        End Enum

        'NA
        Enum ModalidadPresentacionEncabezados
            SinDefinir = 0  'Nuevo MOP 23/11/2021
            DescripcionesTecnicasCampo = 1
            DescripcionesInforme = 2
            DescripcionesAutomaticas = 3
        End Enum

        'Área de pruebas para NoSQL
        Enum DestinosParaReplicacion
            SinDefinir = 0
            RDDMSSQLServer = 1
            NoSQLMongoDB = 2
        End Enum

#End Region

#Region "Estructuras"

#End Region

#Region "Constructores"

#End Region

#Region "Propiedades"

        'Área nueva temporal, para evaluar
        Property OrigenDatos As IConexiones.Controladores 'Ok

        Property TipoConexion As IConexiones.TipoConexion 'Ok

        Property ObjetoDatos As IConexiones.TiposRepositorio 'Ok

        Property TipoGestionOperativa As TiposGestionOperativa

        Property FiltrosAvanzados As String '?

        Property LimiteRegistrosColeccionMuestral As Int32 '?

        ReadOnly Property ObtenerListaRegistros As List(Of Object) 'Ok

        ReadOnly Property ObtenerTablaResumen As DataTable '?

        ReadOnly Property ObtenerListaResumen As List(Of Object) 'Ok

        Property ModalidadConsulta As IOperacionesCatalogo.ModalidadesConsulta 'Ok conexionLibre

        Property IOperaciones As IOperacionesCatalogo

        Property ClausulasLibres As String

        Property ModalidadPresentacion As ModalidadPresentacionEncabezados

        ReadOnly Property TiempoTranscurridoMilisegundos As Long 'Ok

        WriteOnly Property EspacioTrabajo As IEspacioTrabajo 'Ok

        WriteOnly Property LimiteResultados As Int32 'Ok

        WriteOnly Property ConvertirCamposNulos As Int32 'Ok

        WriteOnly Property MilisegundosLatenciaMaxima As Int32 'Ok

        WriteOnly Property TipoRespuestaRequerida As FormatosRespuesta 'Ok

        WriteOnly Property Granularidad As IEnlaceDatos.TiposDimension

        Property MensajeTagWatcher As TagWatcher 'Ok

        Property Registros As List(Of IEntidadDatos)

        ReadOnly Property Tabla As DataTable

        'Área de pruebas para NoSQL
        Property ReflejarEn As DestinosParaReplicacion 'Ok

        ' Property CompararActualizar(ByVal documentoOriginal_ As IEnlaceDatos, ByVal documentoNuevo_ As IEnlaceDatos) As TagWatcher



#End Region

#Region "Metodos"

        ' Function GetMongoCollection(Of T)(ByVal rolname_ As String) As IMongoCollection(Of T)

        '--

        'Function GetMongoCollection(Of T)(Optional ByVal resourceName_ As String = Nothing) As IMongoCollection(Of T)

        Function GetMongoCollection(Of T)(Optional ByVal resourceName_ As String = Nothing,
                                          Optional ByVal rootid_ As Int32? = Nothing) As IMongoCollection(Of T)

        Function GetMongoCollection(Of T)(ByRef imongoClient_ As IMongoClient,
                                          Optional ByVal resorceName_ As String = Nothing,
                                          Optional ByVal rootid_ As Int32? = Nothing) As IMongoCollection(Of T)

        'Function GetMongoCollection(Of T)(ByVal collectiontype_ As Sax.SaxStatements.CollectionTypes,
        '                                         Optional ByVal rolname_ As IEnlaceDatos.RolNames = IEnlaceDatos.RolNames.bigdata) As IMongoCollection(Of T)
        'Function GetMongoCollection(Of T)(ByRef imongoClient_ As IMongoClient,
        '                                         ByVal collectionTypes_ As Sax.SaxStatements.CollectionTypes,
        '                                         Optional ByVal rolname_ As RolNames = RolNames.bigdata) As IMongoCollection(Of T)
        'Function GetMongoClient(ByVal rolname_ As RolNames,
        '                          Optional ByVal settingsType_ As Sax.SaxStatements.SettingTypes = Sax.SaxStatements.SettingTypes.Projects) As IMongoClient

        Function GetMongoClient(Optional ByVal settingsType_ As Sax.SaxStatements.SettingTypes = Sax.SaxStatements.SettingTypes.Projects) As IMongoClient

        Function CompararActualizar(Of T)(ByVal id_ As ObjectId,
                                          ByVal documentoOriginal_ As DocumentoElectronico,
                                          ByVal documentoClonado_ As DocumentoElectronico,
                                          Optional ByVal collectionType_ As Sax.SaxStatements.CollectionTypes = Sax.SaxStatements.CollectionTypes.OO,
                                          Optional ByVal session_ As IClientSessionHandle = Nothing) As Wma.Exceptions.TagWatcher

        'Sub CambiosDetectadosAsync(Of T)(ByVal listaActualizaciones_ As List(Of String),
        '                                 ByVal referencia_ As String,
        '                                 Optional ByVal collectionType_ As Sax.SaxStatements.CollectionTypes = Sax.SaxStatements.CollectionTypes.OO)

        Sub CambiosDetectadosAsync(Of T)(ByVal listaActualizaciones_ As List(Of DocumentoElectronicoObjetoActualizador),
                                         ByVal referencia_ As String,
                                         ByVal id_ As ObjectId,
                                         ByVal resourceName_ As String,
                                         Optional ByVal session_ As IClientSessionHandle = Nothing)

        Function ComparaDocumentosAsync(Of tipo_)(ByVal object1_ As Object,
                                                  ByVal iEnlaceDatos_ As IEnlaceDatos,
                                                  ByVal documentoElectronico_ As DocumentoElectronico) As Task(Of TagWatcher)

        Function AnalizaDiferencias(ByVal documentoNuevo_ As DocumentoElectronico,
                                    ByVal documentoOriginal_ As DocumentoElectronico) As List(Of DocumentoElectronicoObjetoActualizador)
        Function GeneraTransaccion(ByVal estructuraRequerida_ As IEntidadDatos,
                    Optional ByVal clausulas_ As List(Of String) = Nothing,
                    Optional ByVal espacioTrabajo_ As IEspacioTrabajo = Nothing,
                    Optional ByVal limiteResultados_ As Int32 = 1000,
                    Optional ByVal convertirCamposNulos_ As Boolean = True,
                    Optional ByVal milisegundosLatenciaMaxima_ As Int32 = 5000,
                    Optional ByVal tipoRespuestaRequerida_ As FormatosRespuesta = FormatosRespuesta.ObjetoSimple
                    ) As Object 'Ok, pero revisar

        Function ObtieneEstructuraResultados(ByRef estructuraRequerida_ As IEntidadDatos,
                                Optional espacioTrabajo_ As IEspacioTrabajo = Nothing) As TagWatcher

        Function ObtieneEstructuraCompleta(ByVal estructuraRequerida_ As IEntidadDatos,
                                           ByVal nombreEntidad_ As String,
                                           Optional espacioTrabajo_ As IEspacioTrabajo = Nothing) As TagWatcher

        Function CrearTablaResumen(Optional ByVal tabla_ As DataTable = Nothing,
                                   Optional ByVal estructuraResumen_ As IEntidadDatos = Nothing) As DataTable

        Function CrearListaResumen(Optional ByVal tabla_ As DataTable = Nothing,
                                          Optional ByVal estructuraRequerida_ As IEntidadDatos = Nothing) As List(Of Object)

        '1 partida
        Function AgregarDatos(ByVal objetoDatos_ As IEntidadDatos,
                              Optional ByVal espacioTrabajo_ As IEspacioTrabajo = Nothing,
                              Optional ByVal session_ As IClientSessionHandle = Nothing) As TagWatcher 'Ok, pero revisar

        Function AgregarDatosDocumento(ByVal objetoDatos_ As DocumentoElectronico,
                                       Optional ByVal espacioTrabajo_ As IEspacioTrabajo = Nothing,
                                       Optional ByVal session_ As IClientSessionHandle = Nothing) As Threading.Tasks.Task(Of TagWatcher)

        Function ActualizarDocumento(Of T)(ByVal id_ As ObjectId,
                                           ByVal enlaceDatos_ As IEnlaceDatos,
                                           ByVal entidadDatos_ As IEntidadDatos,
                                           ByVal datosCargados_ As IEntidadDatos,
                                           Optional ByVal session_ As IClientSessionHandle = Nothing) As Task(Of TagWatcher)

        Function AgregarDatos(ByVal objetoDatos_ As DocumentoElectronico,
                              Optional ByVal espacioTrabajo_ As IEspacioTrabajo = Nothing) As TagWatcher 'Ok, pero revisar

        'N Partidas
        Function AgregarDatos(ByVal bulkDatos_ As List(Of IEntidadDatos),
                      Optional ByVal espacioTrabajo_ As IEspacioTrabajo = Nothing) As TagWatcher 'Ok, pero revisar



        Function ModificarDatos(ByVal objetoDatos_ As IEntidadDatos,
                                Optional ByVal espacioTrabajo_ As IEspacioTrabajo = Nothing) As TagWatcher 'Ok, pero revisar


        Function EliminarDatos(ByVal objetoDatos_ As IEntidadDatos,
                               Optional ByVal espacioTrabajo_ As IEspacioTrabajo = Nothing) As TagWatcher 'Ok, pero revisar

        Function Consultar(ByVal estructuraRequerida_ As IEntidadDatos,
                    Optional ByVal clausulas_ As List(Of String) = Nothing,
                    Optional ByVal espacioTrabajo_ As IEspacioTrabajo = Nothing,
                    Optional ByVal limiteResultados_ As Int32 = 1000,
                    Optional ByVal convertirCamposNulos_ As Boolean = True,
                    Optional ByVal milisegundosLatenciaMaxima_ As Int32 = 5000,
                    Optional objetoDatos_ As IConexiones.TiposRepositorio = IConexiones.TiposRepositorio.Automatico
                    ) As TagWatcher 'Ok, pero revisar
        Function BusquedaGeneralDocumento(ByVal objetoDatos_ As DocumentoElectronico,
                                  ByVal IdUnicoSeccion_ As Integer,
                                  ByVal IdUnicoCampo_ As Integer,
                                  ByVal valor_ As String) As TagWatcher


        Function NotificarSubscriptores(ByVal recurso_ As String,
                                        ByVal iddocumento_ As ObjectId,
                                        ByVal documentoelectronico_ As DocumentoElectronico,
                                        Optional ByVal session_ As IClientSessionHandle = Nothing) As TagWatcher

        Function EliminarSuscripciones(ByVal iddocumento_ As ObjectId,
                                       ByVal subscriptionsgroup_ As List(Of subscriptionsgroup),
                                       Optional ByVal session_ As IClientSessionHandle = Nothing) As TagWatcher

        Function FirmarDocumento(ByVal recurso_ As String,
                                 ByVal iddocumento_ As ObjectId,
                                 ByVal claveusuario_ As String,
                                 Optional ByVal session_ As IClientSessionHandle = Nothing) As TagWatcher

#End Region

    End Interface

End Namespace