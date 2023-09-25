Imports System.Windows.Forms
Imports Gsol.krom
Imports Gsol.monitoreo
Imports Gsol.basededatos
'Imports MongoDB.Driver
'Imports MongoDB.Bson
Imports System.Threading.Tasks
Imports System.Data
Imports MongoDB.Bson

Namespace Gsol.BaseDatos.Operaciones
    Public Interface IOperacionesCatalogo


#Region "Atributos"

        Enum EstadoOperacion
            CError = -1
            CSinDefinir = 0
            CVacio = 1
            COk = 2
        End Enum

        Enum TiposOperacionSQL
            SinDefinir = -1
            ConsultaCaracteristicas = 0
            Consulta = 1
            Insercion = 2
            Modificar = 3
            Eliminar = 4

            InsertMany = 5
            InsertManyAsync = 6
            InsertOne = 7
            InsertOneAsync = 8
            UpdateMany = 9
            UpdateManyAsync = 10
            UpdateOne = 11
            UpdateOneAsync = 12
        End Enum

        Enum TiposOperacionNoSQL
            SinDefinir = -1
            ConsultaCaracteristicas = 0
            Consulta = 1
            Insercion = 2
            Modificar = 3
            Eliminar = 4
            Guardar = 5

        End Enum

        Enum TiposEscritura
            Inmediata = 0
            MemoriaIntermedia = 1
            MemoriaIntermediaBeta = 2
            TransaccionBajoDemanda = 3
            SinDefinir = 4
        End Enum

        Enum OrdenConsulta
            ASC = 0
            DESC = 1
            INDEFINIDO = 2
        End Enum

        Enum TiposAccesoCampo
            NombreTecnico = 0
            NombreDesplegado = 1
            SinDefinir = 2
        End Enum

        Enum ModosClonacion
            Basica = 1
            Completa = 2

        End Enum

        Enum ComplexTypes
            Undefined = 0
            ComplexA1A1 = 1
            ComplexA1B1A1 = 2
            ComplexA2A2 = 3
            ComplexA1B2A2 = 4
            ComplexA2B2B2A2 = 5
            ComplexA2B2B2nA2 = 6
            ComplexA2B2A2 = 7
        End Enum

        Enum ModalidadesBusqueda
            SinDefinir = 0
            HiloAsincrono = 1
            HiloSincrono = 2
        End Enum

        Enum ModalidadesConsulta 'Conectividad
            SinDefinir = 0
            Singleton = 1
            ConexionLibre = 2
        End Enum


        Enum TiposVisualizacionCampos
            PresentarTodos
            PresentarVisibles
            PresentarVisiblesAcarreo
            PresentarAcarreo
            PresentarImpresion
            RegenerarVistaEntorno
        End Enum

#End Region

#Region "Constructores"


#End Region

#Region "Destructores"

#End Region

#Region "Propiedades"

        Property EstructuraConsulta As IEntidadDatos

        Property VisualizacionCamposConfigurada As TiposVisualizacionCampos

        Property CadenaEncabezados As String

        Property Nombre As String

        Property NombreToken As String

        'Numero de serie del catálogo
        Property SerieCatalogo As Int32

        Property IdentificadorCatalogo As String

        Property IdentificadorEmpresa As String


        Property Caracteristicas As Dictionary(Of Integer, ICaracteristica)

        'Property BulkCaracteristicas As List(Of RegistroIOperaciones)

        ReadOnly Property Vista As DataSet

        Property CantidadVisibleRegistros As Int32

        Property IndicePaginacion As Int32

        'Capa 3
        Property VistaEncabezados As String

        Property TablaEdicion As String

        Property OperadorCatalogoConsulta As String

        Property OperadorCatalogoInsercion As String

        Property OperadorCatalogoBorrado As String

        Property OperadorCatalogoModificacion As String

        'Avanzada
        Property ValorIndice As String

        'Espacio de trabajo
        Property EspacioTrabajo As IEspacioTrabajo

        Property ClausulasLibres As String

        Property ClausulasAutoFiltros As String

        Property TipoEscritura As TiposEscritura

        'Tipo de escritura ( Inmediata, MemoriaIntermedia )
        Property RegistrosTemporales As Dictionary(Of Int32, String)

        Property RegistrosTemporalesDataTable As DataTable


        Property CampoPorNombre(ByVal nombrecampo_ As String) As String

        Property CampoPorNombreAvanzado(ByVal nombrecampo_ As String,
                                        Optional ByVal tipoBusquedaCampo_ As TiposAccesoCampo = TiposAccesoCampo.NombreTecnico) As String

        'Control de insercion repetitiva
        Property SalidaLlaves() As String

        Property EntradaLlaves() As String

        Property NoMostrarRegistrosInsertados As Boolean

        'Enlace
        Property OperacionAnterior As IOperacionesCatalogo

        'Ordenar resultados
        WriteOnly Property OrdenarResultados(ByVal columnaNumero_ As Int32) As OrdenConsulta

        'Coleccion de instrucciones
        ReadOnly Property ColeccionInstrucciones As Dictionary(Of String, IOperacionesCatalogo)

        'SQL Transaccion
        Property SQLTransaccion As Dictionary(Of String, String)

        'Transacciones

        Property IDNivelTransaccional As String

        Property IDObjetoTransaccional As String

        Property IndiceTablaTemporal As Int32

        Property IndiceTablaTemporalLlamante As String

        Property TipoOperacionSQL As TiposOperacionSQL

        Property AdvertenciasIndicador As Boolean

        WriteOnly Property SetVista As DataSet

        Property ObjetoRepositorio As Object



        Property DeclaracionesAdicionalesUsuario As String

        Property ComplejoTransaccional As ComplexTypes

        Property EjecutarPlanEjecucionTransaccional As Boolean

        Property PlanEjecucionSQL As String

        Property InstruccionesSQLAntesIniciarTransaccion As String

        Property InstruccionesAdicionalesPieTransaccion As String




        'Caracteristicas para bitacora especial

        ''Private _i_Cve_Usuario As Int32 'IDUsuario = _i_Cve_Usuario

        Property IDUsuario As Int32

        ''Private _i_Cve_DivisionMiEmpresa As Int32 'conexion_.ClaveDivisionMiEmpresa = _i_Cve_DivisionMiEmpresa
        Property ClaveDivisionMiEmpresa As Int32

        ''Private _i_Cve_Aplicacion As Int32 'conexion_.IDAplicacion = _i_Cve_Aplicacion
        Property IDAplicacion As IBitacoras.ClaveTiposAplicacion


        ''Private TipoInstrumentacion As IBitacoras.TiposInstrumentacion 'conexion_.TipoInstrumentacion = IBitacoras.TiposInstrumentacion.GestorIOperaciones
        Property TipoInstrumentacion As IBitacoras.TiposInstrumentacion


        ''Private _i_Cve_RecursoSolicitante As Int32 'conexion_.IDRecursoSolicitante = _i_Clave_Modulo
        Property IDRecursoSolicitante As Int32

        'Propiedades nuevas opcionales
        Property Version As String
        Property Granularidad As String
        Property Entidad As String
        Property Dimension As String

        Property NombreClaveUpsert As String

        Property ActivarComodinesDeConsulta As Boolean

        'Propiedades nuevas para IEnlace, MOP 24/02/2021

        Property KeyValues As List(Of String)

        Property ReflejarEn As IEnlaceDatos.DestinosParaReplicacion

        Property ObjetoDatos As IConexiones.TiposRepositorio 'Implements IEnlaceDatos.ObjetoDatos

        Property ObjetoDatosAlternativo As IConexiones.TiposRepositorio 'Implements IEnlaceDatos.ObjetoDatos

        Property OrigenDatos As IConexiones.Controladores 'Implements IEnlaceDatos.OrigenDatos

        Property OrigenDatosAlternativo As IConexiones.Controladores 'Implements IEnlaceDatos.OrigenDatos

        Property TipoConexion As IConexiones.TipoConexion 'Implements IEnlaceDatos.TipoConexion

        Property TipoConexionAlternativa As IConexiones.TipoConexion 'Implements IEnlaceDatos.TipoConexion

        Property BSONDocumentResult As List(Of BsonDocument) 'MongoCursor(Of BsonDocument)

        Property ModalidadConsulta As ModalidadesConsulta

        Property ModalidadConsultaAlternativa As ModalidadesConsulta

        Property ActivarLecturaSuciaSQL As Boolean

#End Region

#Region "Métodos"

        Function EjecutaPlanSQL() As Boolean

        Function EjecutaInstrucciones() As Boolean

        ReadOnly Property Vista(ByVal numeroFila_ As Int32,
                                ByVal columna_ As String,
                                Optional ByVal tipoAccesoCampo_ As TiposAccesoCampo = TiposAccesoCampo.NombreTecnico) As String

        Function Clone() As Object

        Function EditaCampoPorNombre(ByVal nombrecampo_ As String) As ICaracteristica

        Sub EliminaCampoPorNombre(ByVal nombrecampo_ As String,
                                  Optional ByVal esVisible_ As Boolean = False)

        'Grabar datos ( Bajar datos de memoria Intermedia a HD ) solo aplica para escritura tipo Memoria Intermedia
        Function GrabarDatosEnDisco(Optional ByVal pilallaves_ As Dictionary(Of Int32, ICaracteristica) = Nothing,
                                    Optional ByVal tipoescritura_ As IOperacionesCatalogo.TiposEscritura = IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia) As Boolean

        Function ValidaValoresCampos(ByVal tipooperacion_ As IOperacionesCatalogo.TiposOperacionSQL) As String

        Function PreparaCatalogo(Optional ByVal operacion_ As IOperacionesCatalogo.TiposOperacionSQL = TiposOperacionSQL.SinDefinir) As IOperacionesCatalogo.EstadoOperacion

        Function GenerarVista(Optional ByVal valordefault_ As String = "%",
                              Optional ByVal filtrarcampo_ As String = Nothing,
                              Optional ByVal rigor_ As String = "like") As EstadoOperacion

        Function GenerarVistaAsync(Optional ByVal valordefault_ As String = "%",
                              Optional ByVal filtrarcampo_ As String = Nothing,
                              Optional ByVal rigor_ As String = "like") As Task(Of EstadoOperacion)

        Function Agregar(ByVal grabarPlanEjecucion_ As Boolean) As EstadoOperacion

        Function Agregar() As EstadoOperacion

        Sub AgregaRegistroBulk(ByVal registroNuevo_ As RegistroIOperaciones)

        Function Eliminar(ByVal llaveprimaria_ As String) As EstadoOperacion

        Function EliminarBulk(ByVal llavesprimarias_ As List(Of String)) As EstadoOperacion

        Function Modificar(ByVal llaveprimaria_ As String) As EstadoOperacion

        Function ModificarBulk(ByVal llavesprimarias_ As List(Of String)) As EstadoOperacion

        Sub OcultaCamposGridView(ByRef gridview_ As DataGridView)
        'Function GenerarVistaAsync(Optional valordefaultbuscado_ As String = Nothing, Optional filtrarcampo_ As String = Nothing, Optional rigor_ As String = Nothing) As Task(Of EstadoOperacion)

#End Region

    End Interface

    Public Class RegistroIOperaciones

#Region "Atributos"

        Private _cantidadCampos As Int32

        Private _listaCaracteristicas As List(Of ICaracteristica)

#End Region


#Region "Constructores"

        Sub New()

            _cantidadCampos = 0

            _listaCaracteristicas = New List(Of ICaracteristica)

        End Sub

#End Region


#Region "Propiedades"

        ReadOnly Property Caracteristicas As List(Of ICaracteristica)

            Get
                Return _listaCaracteristicas

            End Get

        End Property

        'Property Add(ByVal idCampo_ As Int32) As ICaracteristica

        '    Get
        '        Return _listaCaracteristicas(idCampo_)

        '    End Get
        '    Set(value As ICaracteristica)

        '        _listaCaracteristicas.Add(value)

        '        _cantidadCampos += 1

        '    End Set

        'End Property

#End Region


#Region "Métodos"

        Sub Add(ByVal caracteristica_ As ICaracteristica)

            _listaCaracteristicas.Add(caracteristica_)

            _cantidadCampos += 1

        End Sub


#End Region

    End Class

End Namespace


