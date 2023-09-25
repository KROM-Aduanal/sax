Imports System.ComponentModel
Imports System.Xml.Serialization
Imports System.Reflection

Namespace gsol.krom

    Public Class ReglaAccesoKrom

        'Public Class DataAccessRules

#Region "Enums"

        Enum TypesDisplay
            ShowAll = 0
            NotDelivered = 1
            Delivered = 2
        End Enum

        Enum ContainerTypes
            Undefined = 0
            OpenTop = 1
            Refrigerated = 2
        End Enum

        Enum ContainerSizes
            Undefined = 0
            C20Inches = 20
            C40Inches = 40
        End Enum

        Enum GuidTypes
            <Description("Undefined")> UND = 0
            <Description("Master")> Master = 1
            <Description("House")> House = 2
        End Enum

        Enum PatentNumbers
            <Description("Undefined")> UND = 0
            <Description("Rolando Reyes Kuri ")> RRK = 3210
            <Description("Luis de la Cruz Reyes")> LDC = 3921
            <Description("Jesús Gomez Reyes")> JGR = 3945
            <Description("Sergio Álvarez Ramírez")> SAR = 3931
            <Description("Marco Antonio Barquín de la Calle")> MBC = 3962
        End Enum

        Enum AccessTypes
            Access = 1
            Deny = 2
        End Enum

        Enum ApplicationTypes
            Undefined = 0
            Desktop = 4
            Mobile = 11
            Web = 12
        End Enum

        Enum SchemaTypes
            OnlyHeaders
            ReferenceType
            Advanced1
        End Enum

        Enum ModalityTypes
            <Description("Undefined")> UND = 0
            <Description("Sea")> SEA = 1
            <Description("Air")> AIR = 2
            <Description("Road")> ROA = 3
            <Description("Rail")> RAI = 4
            <Description("Logística")> MDL = 5
        End Enum

        Enum CustomSections
            <Description("Undefined")> UND = 0
            <Description("México ( Áereo )")> DAI = 470
            <Description("México ( Terrestre )")> PAN = 200
            <Description("Veracruz ( Marítimo )")> RKU = 430
            <Description("Lázaro Cárdenas ( Marítimo )")> ALC = 510
            <Description("Toluca ( Áereo )")> TOL = 650
            <Description("Toluca ( Áereo ) Seccion 1")> TOL1 = 651
            <Description("Altamira ( Marítimo )")> CEG = 810
            <Description("Nuevo Laredo ( Terrestre )")> SFI = 240
            <Description("Colombia N.L. ( Terrestres )")> SFC = 800
            <Description("Tuxpan Veracruz( Marítimo )")> TUX = 420
            <Description("Manzanillo ( Marítimo )")> SAP = 160
        End Enum

        Enum OperationTypes
            <Description("Undefined")> UND = 0
            <Description("Importación")> IMP = 1
            <Description("Exportación")> EXP = 2
        End Enum

        Enum ReferenceStatusTypes
            <Description("Sin definir")> UND = 0
            <Description("Arribo de buque")> ABU = 50
            <Description("Arribo de la mercancía")> AME = 51
            <Description("Arribo de tren")> ATR = 52
            <Description("Arribo dentro de México")> AMX = 53
            <Description("Aterrizaje")> ATER = 54
            <Description("Booking")> BOO = 55
            <Description("Canje de documentos")> CAD = 56
            <Description("Carga de mercancías")> CAM = 57
            <Description("Carga en tránsito")> CAT = 58
            <Description("Cruce de unidad")> CRU = 59
            <Description("Descarga/entrega en planta")> ENP = 60
            <Description("Despegue")> DES = 61
            <Description("Entregado")> ENT = 62
            <Description("Ingreso a EUA")> IEU = 63
            <Description("Inicio de ruta")> INR = 64
            <Description("Llegada a destino")> LLD = 65
            <Description("Pago de cuenta de gastos")> LFAC = 66
            <Description("Pago de pedimento")> PAG = 67
            <Description("Previo")> PRE = 68
            <Description("Recepción de documentos")> DOC = 69
            <Description("Recepción en bodega")> RBO = 70
            <Description("Recolección de guía")> RGU = 71
            <Description("Recolección/Posición en planta")> PPL = 72
            <Description("Reconocimiento aduanero")> RAD = 73
            <Description("Revalidación")> REV = 74
            <Description("Selección automátizada rojo")> SAR = 75
            <Description("Transbordo")> TBD = 76
            <Description("Zarpe de buque")> ZAR = 77
            <Description("Despacho aduanal")> DSP = 49
            <Description("Emisión de cuenta de gastos")> FAC = 38
            <Description("PENDIENTE DE FACTURAR NUEVAMENTE")> PFAC = 41
            <Description("Alta de la referencia")> ALR = 27
            <Description("Anticipo")> ANT = 5
            <Description("Fecha de Entrada")> ATA = 94
            <Description("Solicitud de Servicio")> SSE = 95
        End Enum

#End Region

#Region "Attributes"

        Private _i_Cve_AccesoUsuarioClavesCliente As Int32

        Private _i_Cve_DivisionMiEmpresa As Int32

        Private _t_RFC As String

        Private _i_Cve_ClienteOperativoExterno As Int32

        Private _i_Cve_ClienteEmpresa As Int32

        Private _i_Cve_DivisionEmpresarialCliente As Int32

        Private _i_Cve_Aplicacion As ApplicationTypes

        Private _i_Cve_Accion As AccessTypes

        Private _i_Aduana As CustomSections

        Private _i_Patente As PatentNumbers

        Private _i_TipoOperacion As OperationTypes

        Private _scriptOfRule As String

        'Reglas Administrativas

        Private _i_Cve_ClienteComprobanteFiscal As Int32

        Private _t_RFC_ClienteComprobanteFiscal As String

        Private _i_ExigirCongruenciaImportadorFacturacion As Int32

#End Region

#Region "Builders"

        Sub New()

            _i_Cve_AccesoUsuarioClavesCliente = 0

            _i_Cve_DivisionMiEmpresa = 0

            _t_RFC = Nothing

            _i_Cve_ClienteOperativoExterno = 0

            _i_Cve_ClienteEmpresa = 0

            _i_Cve_DivisionEmpresarialCliente = 0

            _i_Cve_Aplicacion = ApplicationTypes.Mobile

            _i_Cve_Accion = AccessTypes.Deny

            _i_Aduana = CustomSections.UND

            _i_Patente = PatentNumbers.UND

            _i_TipoOperacion = OperationTypes.UND

        End Sub

#End Region

#Region "Properties"

        Property ClaveClienteComprobanteFiscal As Int32

            Get

                Return _i_Cve_ClienteComprobanteFiscal

            End Get

            Set(value As Int32)

                _i_Cve_ClienteComprobanteFiscal = value

            End Set

        End Property

        Property RFC_ClienteComprobanteFiscal As String

            Get

                Return _t_RFC_ClienteComprobanteFiscal

            End Get

            Set(value As String)

                _t_RFC_ClienteComprobanteFiscal = value

            End Set

        End Property

        Property ExigirCongruenciaImportadorFacturacion As Int32

            Get

                Return _i_ExigirCongruenciaImportadorFacturacion

            End Get

            Set(value As Int32)

                _i_ExigirCongruenciaImportadorFacturacion = value

            End Set

        End Property

        Property IDAccesoUsuarioClavesCliente As Int32

            Get

                Return _i_Cve_AccesoUsuarioClavesCliente

            End Get

            Set(value As Int32)

                _i_Cve_AccesoUsuarioClavesCliente = value

            End Set

        End Property

        Property Accion As AccessTypes

            Get

                Return _i_Cve_Accion

            End Get

            Set(value As AccessTypes)

                _i_Cve_Accion = value

            End Set

        End Property

        Property IDDivisionMiEmpresa As Int32

            Get

                Return _i_Cve_DivisionMiEmpresa

            End Get

            Set(value As Int32)

                _i_Cve_DivisionMiEmpresa = value

            End Set

        End Property

        Property RFC As String

            Get

                Return _t_RFC

            End Get

            Set(value As String)

                _t_RFC = value

            End Set

        End Property

        Property ClaveClienteExterno As Int32

            Get

                Return _i_Cve_ClienteOperativoExterno

            End Get

            Set(value As Int32)

                _i_Cve_ClienteOperativoExterno = value

            End Set

        End Property

        Property IDClienteEmpresa As Int32

            Get

                Return _i_Cve_ClienteEmpresa

            End Get

            Set(value As Int32)

                _i_Cve_ClienteEmpresa = value

            End Set

        End Property

        Property IDDivisionEmpresarialCliente As Int32

            Get

                Return _i_Cve_DivisionEmpresarialCliente

            End Get

            Set(value As Int32)

                _i_Cve_DivisionEmpresarialCliente = value

            End Set

        End Property

        Property Aplicacion As ApplicationTypes

            Get

                Return _i_Cve_Aplicacion

            End Get

            Set(value As ApplicationTypes)

                _i_Cve_Aplicacion = value

            End Set

        End Property

        Property Aduana As CustomSections

            Get

                Return _i_Aduana

            End Get

            Set(value As CustomSections)

                _i_Aduana = value

            End Set

        End Property

        Property Patente As PatentNumbers

            Get

                Return _i_Patente

            End Get

            Set(value As PatentNumbers)

                _i_Patente = value

            End Set

        End Property

        Property TipoOperacion As OperationTypes

            Get

                Return _i_TipoOperacion

            End Get

            Set(value As OperationTypes)

                _i_TipoOperacion = value

            End Set

        End Property

#End Region

#Region "Methods"

#End Region

    End Class

End Namespace

