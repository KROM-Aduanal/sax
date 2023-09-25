Imports Wma.Exceptions
Imports gsol.monitoreo
Imports gsol.BaseDatos.Operaciones

Namespace gsol.basededatos
    Public Interface IConexiones

#Region "Atributos"

        Enum Controladores

            SinDefinir = 0
            MySQL51 = 1
            SQLServer2008 = 2
            VFP5 = 3
            MongoDB = 4
            Automatico = 5

        End Enum

        Enum TipoConexion

            SinDefinir = 0
            MySQLCommand = 1
            SqlCommand = 2
            OdbcCommand = 3
            DirectMongoDB = 4
            Automatico = 5

        End Enum

        Enum TiposRepositorio
            SinDefinir = 0
            DataSetObject = 1
            DataTableObject = 2
            BSONDocumentObject = 3
            Automatico = 4
            DocumentoElectronico = 5 'Nuevo MOP 23/11/2021
        End Enum

#End Region

#Region "Constructores"

#End Region

#Region "Propiedades"

        'Solo lectura
        ReadOnly Property DataSetReciente As DataSet

        ReadOnly Property EstadoConexion As String

        ReadOnly Property ConjuntoDataSet As Dictionary(Of Int32, DataSet)

        'Los datos de entrada y salida
        Property IpServidor As String

        Property PuertoConexion As String

        Property NombreBaseDatos As String

        Property Usuario As String

        Property Contrasena As String

        Property ControladorBaseDatos As Controladores

        Property ObjetoDatos As TipoConexion

        Property NombreUsuarioCliente As String

        Property ModuloCliente As String

        'Adicionales

        Property RepositorioDatos As TiposRepositorio

        ReadOnly Property GetTagWatcherObject As TagWatcher

        ReadOnly Property DataTableReciente As DataTable


        Property IDUsuario As Int32

        Property ClaveDivisionMiEmpresa As Int32

        Property IDAplicacion As IBitacoras.ClaveTiposAplicacion

        Property TipoInstrumentacion As IBitacoras.TiposInstrumentacion

        Property ModalidadConsulta As IOperacionesCatalogo.ModalidadesConsulta

        Property IDRecursoSolicitante As Int32


        Property NombreEnsamblado As String 't_NombreEnsamblado
        Property NombreVT As String 't_NombreVT
        Property NombreCompletoUsuario As String 't_NombreUsuario
        Property CuentaPublicaUsuario As String 't_CuentaUsuario

        'Continuidad de bitacoras

        Property ActivarBitacoraAvanzada As Boolean

        Property ActivarBitacoraSimple As Boolean

#End Region

#Region "Métodos"

        Function IniciaConexion() As Boolean

        Function TerminaConexion() As Boolean

        Function EjecutaConsultaIndividual(ByVal _sentencia As String) As Boolean

        Function EjecutaConsulta(ByVal _sentencia As String) As Boolean

        Function EjecutaConsultaSinRespuesta(ByVal _sentencia As String) As Boolean

        Function EjecutaSentenciaIndividual(ByVal _sentencia As String) As Boolean

        Function EjecutaSentencia(ByVal _sentencia As String) As Boolean

        '16/Nov/2018, implementando hiperbitacora, pbm, ª_ª
        Function EjecutaConsultaDirectaEstandarizada(ByVal _sentencia As String) As TagWatcher

        Function EjecutaConsultaDirectaEstandarizadaOpenClose(ByVal _sentencia As String) As TagWatcher

        Function EjecutaConsultaDirectaEstandarizadaDataTable(ByVal datadapter_ As IDbDataAdapter, ByVal command_ As IDbCommand, ByVal _sentencia As String) As TagWatcher

        Function EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(ByVal _sentencia As String) As TagWatcher

        '08/Ago/2019
        Property TiempoEspera As Int32

        '25/Feb/2021, IEnlace MongoDB.


#End Region

    End Interface

End Namespace