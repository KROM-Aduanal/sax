Imports Wma.Exceptions
Imports gsol.BaseDatos.Operaciones
Imports gsol.basededatos

Namespace gsol.krom

    Public Interface IAdaptadorDatos

        Enum TiposFormatoSalida
            ObjetoAutomatico
            XML
            CSV
            XLSX
            PDF
        End Enum

        Property ModalidadPresentacion As IEnlaceDatos.ModalidadPresentacionEncabezados

        Property IOperaciones As IOperacionesCatalogo

        Property LimiteRegistros As Int32

        ReadOnly Property Registros As List(Of IEntidadDatos)

        ReadOnly Property Tabla As DataTable

        Property ProcesamientoAsincrono As Boolean

        Property EspacioTrabajo As IEspacioTrabajo

        Property ExportarResultadosAutomaticamente As TiposFormatoSalida

        Property ActivarBitacoras As Boolean

        Property ActivarReplicacion As Boolean

        ReadOnly Property TiempoTotalTranscurrido As Int32

        ReadOnly Property Estatus As TagWatcher

        Property LimiteRegistrosPresentacion As Int32

        Property FiltrosAvanzados As String

        Function ProcesaConsulta(ByVal estructuraRequerida_ As IEntidadDatos, ByVal clausulasLibres_ As String,
                                 ByVal modalidadConsultas_ As IOperacionesCatalogo.ModalidadesConsulta) As TagWatcher

        Function OperacionDatos(ByVal estructuraRequerida_ As IEntidadDatos,
                                       ByVal tipoOperacionDatos_ As IOperacionesCatalogo.TiposOperacionSQL,
                                       ByVal clausulasLibres_ As String,
                                       ByVal modalidadConsultas_ As IOperacionesCatalogo.ModalidadesConsulta) As TagWatcher

        Function OperacionDatos(ByVal bulkRequerida_ As List(Of IEntidadDatos),
                               ByVal tipoOperacionDatos_ As IOperacionesCatalogo.TiposOperacionSQL,
                               ByVal clausulasLibres_ As String,
                               ByVal modalidadConsultas_ As IOperacionesCatalogo.ModalidadesConsulta) As TagWatcher


        Function GeneraEstructuraResultados(ByRef estructuraRequerida_ As IEntidadDatos,
                                            ByVal clausulasLibres_ As String,
                                            ByVal modalidadConsultas_ As IOperacionesCatalogo.ModalidadesConsulta) As TagWatcher

        'Function GeneraEstructuraResultadosEspecial(ByRef estructuraRequerida_ As IEntidadDatos,
        '                            ByVal clausulasLibres_ As String,
        '                            ByVal modalidadConsultas_ As IOperacionesCatalogo.ModalidadesConsulta) As TagWatcher


        Function GeneraEstructuraCompleta(ByVal estructuraRequerida_ As IEntidadDatos,
                                          ByVal nombreEntidas_ As String,
                                    ByVal clausulasLibres_ As String,
                                    ByVal modalidadConsultas_ As IOperacionesCatalogo.ModalidadesConsulta) As TagWatcher


        Property ObjetoDatos As IConexiones.TiposRepositorio 'Implements IEnlaceDatos.ObjetoDatos

        Property OrigenDatos As IConexiones.Controladores 'Implements IEnlaceDatos.OrigenDatos

        Property TipoConexion As IConexiones.TipoConexion 'Implements IEnlaceDatos.TipoConexion

    End Interface

End Namespace


