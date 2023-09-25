Imports Wma.Exceptions
Imports System.ComponentModel
Imports Syn.Documento
Imports System.Threading.Tasks

Namespace gsol.krom

    Public Interface IEntidadDatos : Inherits IDisposable : Inherits ICloneable

#Region "Enum"

        Enum EstilossCCS

            SinDefinir = 0
            TipoOperacion = 1
            Modalidad = 2
            Aduana = 3
            AgenciaAduanal = 4
            ArchivoEspecial_PDF = 5
            CG_PDF = 6
            CG_XML = 7
            Soportes_CG_ZIP = 8
            EstatusFactura = 9
            TipoComprobante = 10
            OficinaLogistica = 11
            TipoMovimientoLogistica = 12
            AtlasReferenciaAgenciaAduanal = 13
            EstatusTracking = 14
            EstatusTrackingLogistica = 15
            ReferenciaTracking = 16
            DivisionEmpresa = 17
            MostrarDocumentos = 18
            ComplementosPago_ZIP = 19
            ComplementosPagoTerceros_ZIP = 20
            SeparacionMiles = 21
            ClaveDocumentoPDF = 22
            EditarDatos = 23
            MostrarDatos = 24
            EstatusSeguimiento = 25
            Check = 26
            MostrarDocumentosFF = 27
            ArchivoEspecialFF_PDF = 28
        End Enum

        Enum TiposDatosHTML
            <Description("SinDefinir")> SinDefinir = 0
            <Description("String")> Texto = 1
            <Description("Int")> Entero = 2
            <Description("Date")> Fecha = 3
            <Description("Decimal")> Real = 4
            <Description("Bool")> Booleno = 5
            <Description("Select")> Lista = 6
        End Enum

        Enum TiposDatos
            SinDefinir
            Entero
            Real
            Booleno
            Fecha
            Texto
        End Enum

        Enum TiposRigorDato
            SinDefinir
            Opcional
            Obligatorio
        End Enum

        Enum TiposLlave
            SinDefinir = 0
            Primaria = 1
            Foranea = 2
        End Enum

        Enum TiposFuncionesAgregacion
            <Description("SinDefinir")> SinDefinir = 0
            <Description("Sum")> Suma = 1
            <Description("Avg")> Promedio = 2
            <Description("Min")> Minimo = 3
            <Description("Max")> Maximo = 4
            <Description("Count")> Recuento = 5
            <Description("StDev")> Desviacion = 6
            <Description("Var")> Varianza = 7
        End Enum

        Enum TiposOrdenamiento
            <Description("ASC")> Asc = 0
            <Description("DESC")> Desc = 1
            <Description("SinDefinir")> SinDefinir = 2
        End Enum

        Enum MostrarCampo
            No = 0
            Si = 1
        End Enum

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
            DocumentoElectronico = 600
            'Clientes = 83
            'CatalogoClientes = 84
            'DivisionesEmpresariales = 85
            'Prefijos = 86
            'ModalidadAduanaSeccion = 87
            'Personas = 88
            'Contacto = 89
            'Puestos = 90
            'Puesto = 91
            'Domicilio = 92
            'DomicilioFiscal = 93

        End Enum

#End Region

#Region "Atributos"

#End Region

#Region "Propiedades"

        Property NewRow As DataRow

        Property Dimension As IEnlaceDatos.TiposDimension 'Ok, ConstructorPedimentoNormal

        ReadOnly Property CampoPorNombre(ByVal atributoDimension_ As Object) As Object 'Ok

        Property Atributos As List(Of CampoVirtual)

        Property Clausulas As String

        Property Status As TagWatcher 'Ok

        Property Attribute(ByVal attributeName_ As Object) As Object 'Ok

        Property UpdateOnKeyValues As List(Of String)

        Property DeteleOnKeyValues As List(Of String)

        'Property OperacionGenerica As OperacionGenerica


#End Region

#Region "Metodos"

        Sub Array(ByVal arrayOfDataEntity_ As List(Of IEntidadDatos))

        Sub cmp(ByVal nombreCampo_ As Object) 'Ok

        Sub cmp(ByVal nombreCampo_ As Object,
                ByVal descripcion_ As String) 'Ok

        Sub cmp(ByVal nombreCampo_ As Object,
                Optional ByVal descripcion_ As String = Nothing,
                Optional ByVal estiloCCS_ As EstilossCCS = EstilossCCS.SinDefinir,
                Optional ByVal nombreCampoFiltro_ As Object = Nothing,
                Optional ByVal esAgrupador_ As Boolean = False,
                Optional ByVal funcionAgregacion_ As TiposFuncionesAgregacion = TiposFuncionesAgregacion.SinDefinir,
                Optional ByVal tipoOrdenamiento_ As TiposOrdenamiento = TiposOrdenamiento.SinDefinir,
                Optional ByVal mostrarCampo_ As MostrarCampo = MostrarCampo.Si)

        'Documents methods
        'Function UpsertSubscriptions(ByVal toResource_ As String,
        '                             ByVal fromResource_ As String,
        '                             ByVal sgroup_ As subscriptionsgroup) As Task(Of Boolean)

        'Property SubscriptionsGroup As subscriptionsgroup

#End Region

    End Interface

End Namespace