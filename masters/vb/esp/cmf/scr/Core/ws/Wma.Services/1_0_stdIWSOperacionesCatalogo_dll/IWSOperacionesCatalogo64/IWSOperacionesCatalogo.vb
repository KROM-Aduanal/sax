' NOTA: puede usar el comando "Cambiar nombre" del menú contextual para cambiar el nombre de interfaz "IService1" en el código y en el archivo de configuración a la vez.

'Imports System.Windows.Forms

Namespace Gsol.BaseDatos.Operaciones

    <ServiceContract()>
    Public Interface IWSOperacionesCatalogo

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


        End Enum

        Enum ModalidadesBusqueda
            SinDefinir = 0
            HiloAsincrono = 1
            HiloSincrono = 2
        End Enum

#End Region

#Region "Constructores"

#End Region

#Region "Destructores"

#End Region

#Region "Propiedades"


        <DataMember()>
        Property Nombre As String

        'Numero de serie del catálogo
        Property SerieCatalogo As Int32

        Property IdentificadorCatalogo As String

        Property IdentificadorEmpresa As String


        Property Caracteristicas As Dictionary(Of Integer, ICaracteristica)

        ReadOnly Property Vista As DataSet

        Property CantidadVisibleRegistros As Int32

        <DataMember()>
        Property IndicePaginacion As Int32

        'Capa 3
        Property VistaEncabezados As String

        <DataMember()>
        Property TablaEdicion As String

        <DataMember()>
        Property OperadorCatalogoConsulta As String

        <DataMember()>
        Property OperadorCatalogoInsercion As String

        <DataMember()>
        Property OperadorCatalogoBorrado As String

        <DataMember()>
        Property OperadorCatalogoModificacion As String

        'Avanzada
        <DataMember()>
        Property ValorIndice As String

        'Espacio de trabajo
        <DataMember()>
        Property EspacioTrabajo As IEspacioTrabajo

        <DataMember()>
        Property ClausulasLibres As String

        <DataMember()>
        Property ClausulasAutoFiltros As String

        <DataMember()>
        Property TipoEscritura As TiposEscritura

        'Tipo de escritura ( Inmediata, MemoriaIntermedia )
        <DataMember()>
        Property RegistrosTemporales As Dictionary(Of Int32, String)

        <DataMember()>
        Property RegistrosTemporalesDataTable As DataTable

        <DataMember()>
        Property CampoPorNombre(ByVal nombrecampo_ As String) As String

        <DataMember()>
        Property CampoPorNombreAvanzado(ByVal nombrecampo_ As String, _
                                        Optional ByVal tipoBusquedaCampo_ As TiposAccesoCampo = TiposAccesoCampo.NombreTecnico) As String

        'Control de insercion repetitiva
        <DataMember()>
        Property SalidaLlaves() As String

        <DataMember()>
        Property EntradaLlaves() As String

        <DataMember()>
        Property NoMostrarRegistrosInsertados As Boolean

        'Enlace
        <DataMember()>
        Property OperacionAnterior As IWSOperacionesCatalogo

        'Ordenar resultados
        WriteOnly Property OrdenarResultados(ByVal columnaNumero_ As Int32) As OrdenConsulta

        'Coleccion de instrucciones

        ReadOnly Property ColeccionInstrucciones As Dictionary(Of String, IWSOperacionesCatalogo)

        'SQL Transaccion
        <DataMember()>
        Property SQLTransaccion As Dictionary(Of String, String)

        'Transacciones
        <DataMember()>
        Property IDNivelTransaccional As String

        <DataMember()>
        Property IDObjetoTransaccional As String

        <DataMember()>
        Property IndiceTablaTemporal As Int32

        <DataMember()>
        Property IndiceTablaTemporalLlamante As String

        <DataMember()>
        Property TipoOperacionSQL As TiposOperacionSQL

        <DataMember()>
        Property AdvertenciasIndicador As Boolean

        <DataMember()>
        WriteOnly Property SetVista As DataSet

        <DataMember()>
        Property ObjetoRepositorio As Object


        <DataMember()>
        Property DeclaracionesAdicionalesUsuario As String

        <DataMember()>
        Property ComplejoTransaccional As ComplexTypes

        <DataMember()>
        Property EjecutarPlanEjecucionTransaccional As Boolean

        <DataMember()>
        Property PlanEjecucionSQL As String

        <DataMember()>
        Property InstruccionesSQLAntesIniciarTransaccion As String

        <DataMember()>
        Property InstruccionesAdicionalesPieTransaccion As String

#End Region


#Region "Métodos"

        <OperationContract()>
        Function EjecutaPlanSQL() As Boolean

        <OperationContract()>
        Function EjecutaInstrucciones() As Boolean


        ReadOnly Property Vista(ByVal numeroFila_ As Int32, _
                                ByVal columna_ As String, _
                                Optional ByVal tipoAccesoCampo_ As TiposAccesoCampo = TiposAccesoCampo.NombreTecnico) As String

        '<OperationContract()>
        Function Clone() As Object

        '<OperationContract()>
        Function EditaCampoPorNombre(ByVal nombrecampo_ As String) As ICaracteristica

        '<OperationContract()>
        Sub EliminaCampoPorNombre(ByVal nombrecampo_ As String, _
                                  Optional ByVal esVisible_ As Boolean = False)

        'Grabar datos ( Bajar datos de memoria Intermedia a HD ) solo aplica para escritura tipo Memoria Intermedia
        '<OperationContract()>
        Function GrabarDatosEnDisco(Optional ByVal pilallaves_ As Dictionary(Of Int32, ICaracteristica) = Nothing, _
                                    Optional ByVal tipoescritura_ As IWSOperacionesCatalogo.TiposEscritura = IWSOperacionesCatalogo.TiposEscritura.MemoriaIntermedia) As Boolean

        <OperationContract()>
        Function ValidaValoresCampos(ByVal tipooperacion_ As IWSOperacionesCatalogo.TiposOperacionSQL) As String

        <OperationContract()>
        Function PreparaCatalogo(Optional ByVal operacion_ As IWSOperacionesCatalogo.TiposOperacionSQL = TiposOperacionSQL.SinDefinir) As IWSOperacionesCatalogo.EstadoOperacion

        <OperationContract()>
        Function GenerarVista(Optional ByVal valordefault_ As String = "%", _
                              Optional ByVal filtrarcampo_ As String = Nothing, _
                              Optional ByVal rigor_ As String = "like") As EstadoOperacion

        <OperationContract()>
        Function Agregar(ByVal grabarPlanEjecucion_ As Boolean) As EstadoOperacion

        <OperationContract()>
        Function Agregar3() As EstadoOperacion

        <OperationContract()>
        Function Eliminar(ByVal llaveprimaria_ As String) As EstadoOperacion

        <OperationContract()>
        Function Modificar(ByVal llaveprimaria_ As String) As EstadoOperacion

        'Sub OcultaCamposGridView(ByRef gridview_ As DataGridView)

#End Region

    End Interface

End Namespace

