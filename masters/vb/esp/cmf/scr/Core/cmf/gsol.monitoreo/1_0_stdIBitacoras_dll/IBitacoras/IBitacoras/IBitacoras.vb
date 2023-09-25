Imports Wma.Exceptions
Imports gsol.BaseDatos.Operaciones

'Namespace gsol.monitoreo

Public Interface IBitacoras

#Region "Atributos"

    Enum TiposBitacora
        Alertas = 10
        Errores = 11
        Adbertencias = 12
        Informacion = 14
        Otros = 15
    End Enum

    Enum TiposSucesos
        Consultar = 1
        Insertar = 2
        Modificar = 3
        Eliminar = 4
        Otros = 5
    End Enum

    Enum EstatusInsercion
        SinEstatus
        Correcto
        Incorrecto
    End Enum

    'Confirmar
    Enum ClaveTiposAplicacion
        SinDefinir = 0
        AplicacionEscritorioKrombase = 1
        AplicacionMovilTracking = 2
        AplicacionWebExtranet = 3
    End Enum

    Enum EstadosConsultaAvanzada
        EnProceso = 0
        Concluida = 1
        AbortadaFallo = 2
    End Enum

    Enum TiposInstrumentacion
        SinDefinir = 0
        ConsultaLibre = 1
        GestorIOperaciones = 2
        GestorIEnlaceDatos = 3
    End Enum

#End Region

#Region "Constructores"

#End Region

#Region "Destructores"

#End Region

#Region "Propiedades"

    'Sección básica

    Property TipoBitacora As TiposBitacora

    Property TipoSuceso As TiposSucesos

    Property MisCredenciales As ICredenciales

    Property Disparo As String

    Property Permiso As Int32

    Property Mensaje As String

    Property Parametros As String

    ReadOnly Property Estatus As EstatusInsercion

    Property Modulo As String

    Property NombreUsuario As String

    'Sección avanzada, extensión del contrato 15/11/2018, Pedro Bautista tu servilleta!

    'i_Cve_BitacoraAvanzadaConsulta
    Property ClaveUsuario As Int32 'i_Cve_Usuario
    Property ClaveDivisionMiEmpresa As Int32 'i_Cve_DivisionMiEmpresa
    'Property TipoSuceso As Int32 'i_Cve_Suceso = TipoSuceso
    Property DiaSemana As Int16 'i_DiaSemana 1,2...7
    Property Mes As Int16 'i_Mes 1,2...12
    Property Anio As Int32 'i_Anio 2018
    Property FechaHoraInicio As DateTime 'f_FechaHoraInicio
    Property FechaHoraFinal As DateTime 'f_FechaHoraFinal
    'Property Permiso As Int32 'i_Cve_Permiso
    'Property Mensaje As Int32 't_Mensaje
    'Property i_Cve_InformacionSQL As Int32 'i_Cve_InformacionSQL Interno
    Property ClaveAplicacion As ClaveTiposAplicacion 'i_Cve_Aplicacion
    'Property EstadoConsulta As Int32 'i_Cve_Estado
    Property EstadoConsulta As EstadosConsultaAvanzada 'i_Cve_Estatus= 1 Concluida, 2 Sin concluir
    'Property i_Cve_TipoMensaje As Int32 'i_Cve_TipoMensaje
    Property MensajeTagWatcher As TagWatcher 'i_Cve_TagWatcher
    Property TiempoRespuestaTotal As Double 'i_TiempoRespuestaTotal
    Property TipoInstrumentacion As TiposInstrumentacion 'i_Instrumentacion = 1=IOperaciones, 2= IEnlaceDatos
    Property ModalidadConsulta As IOperacionesCatalogo.ModalidadesConsulta    'i_Conectividad = 1= Singleton, 2= ConexionLibre
    Property MemoriaRAMDisponibleGB As Double 'i_MemoriaRAMDisponibleGB
    Property MemoriaRAMTotalGB As Double 'i_MemoriaRAMTotalGB
    Property RecursoSolicitante As Int32 'i_Cve_RecursoSolicitante
    Property NumeroIP As String 't_IP
    Property PaisSolicitud As String 't_Pais
    Property EstadoCiudad As String 't_EstadoCiudad

    Sub DocumentaBitacoraCapaDatos(ByVal consulta_ As String,
                                 ByVal estatus_ As IBitacoras.EstadosConsultaAvanzada,
                                 ByVal f_Fecha_ As DateTime,
                                 Optional ByVal tiempoTotalRespuesta_ As Double = 0,
                                 Optional ByVal tagWatcherMensaje_ As TagWatcher = Nothing)

    Property IDTransaccionAbierta As String

    '._t_NombreEnsamblado = _
    Property NombreEnsamblado As String

    '._t_NombreVT()
    Property NombreVT As String

    '._t_NombreUsuario()
    Property NombreCompletoUsuario As String

    '._t_CuentaUsuario()
    Property CuentaPublicaUsuario As String

#End Region

#Region "Metodos"


#End Region

End Interface

'End Namespace

