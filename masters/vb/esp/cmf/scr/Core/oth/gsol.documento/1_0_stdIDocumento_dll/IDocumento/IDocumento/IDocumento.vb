Imports Wma.Exceptions
Imports System.ComponentModel
Imports System.Runtime.Serialization
Imports System.Reflection
Imports System.Xml.Serialization


Namespace gsol.documento

    Public Interface IDocumento

#Region "Atributos"
        Enum TiposDocumento
            INDEFINIDO = 0
            XML = 1
            PDF = 2
            JPG = 3
            JPEG = 4
            PNG = 5
            XLS = 6
            XLSX = 7
            DOC = 8
            DOCX = 9
            RAR = 10
            ZIP = 11
            REG = 12
            CER = 13
            KEY = 14
        End Enum

        Enum TiposProcesable

            INDEFINIDO = 0
            XML = 1
            XMLCFDI = 2
            XMLCFDIComplementoPago = 3

        End Enum
        Enum EstatusDocumentos
            Undefined = 0
            Ok = 1
            Errores = 2

        End Enum



        Enum TiposDocumentos

            <EnumMember> <Description("Comprobante Fiscal Digital por Internet")> CFDIXML = 3
            <EnumMember> <Description("Comprobante Fiscal Digital por Internet")> CFDIPDF = 4
            <EnumMember> <Description("Comprobante de pago a proveedor extranjero (PDF)")> CompExtPDF = 5
            <EnumMember> <Description("Comprobante de pago a proveedor extranjero (JPG)")> CompExtJPG = 6
            <EnumMember> <Description("Comprobante de pago a proveedor extranjero (JPEG)")> CompExtJPEG = 7
            <EnumMember> <Description("Comprobante de pago a proveedor extranjero (PNG)")> CompExtPNG = 8
            <EnumMember> <Description("Comprobante de pago a proveedor extranjero (TIFF)")> CompExtTIFF = 9
            <EnumMember> <Description("Documentación general para operaciones (XML)")> EMAILXML = 10
            <EnumMember> <Description("Documentación general para operaciones (PDF)")> EMAILPDF = 11
            <EnumMember> <Description("Documentación general para operaciones (JPEG)")> EMAILJPEG = 12
            <EnumMember> <Description("Documentación general para operaciones (PNG)")> EMAILPNG = 13
            <EnumMember> <Description("Documentación general para operaciones (TIFF)")> EMAILTIFF = 14
            <EnumMember> <Description("Documentación general para operaciones (DOC)")> EMAILDOC = 15
            <EnumMember> <Description("Documentación general para operaciones (DOCX)")> EMAILDOCX = 16
            <EnumMember> <Description("Documentación general para operaciones (XLS)")> EMAILXLS = 17
            <EnumMember> <Description("Documentación general para operaciones (XLSX)")> EMAILXLSC = 18
            <EnumMember> <Description("Documentación general para operaciones (PPT)")> EMAILPPT = 19
            <EnumMember> <Description("Documentación general para operaciones (PPS)")> EMAILPPS = 20
            <EnumMember> <Description("Documentación general para operaciones (PPTX)")> EMAILPPTX = 21
            <EnumMember> <Description("Documentación general para operaciones (PPSX)")> EMAILPPSX = 22
            <EnumMember> <Description("Documentación general para operaciones (CSV)")> EMAILCSV = 23
            <EnumMember> <Description("Documentación general para operaciones (JPG)")> EMAILJPG = 24
            <EnumMember> <Description("Factura Comercial")> FAC = 25
            <EnumMember> <Description("Cuenta de Gastos")> CGT = 26
            <EnumMember> <Description("Avisos")> AVS = 27
            <EnumMember> <Description("Otros")> OTR = 28
            <EnumMember> <Description("Comprobante de Gastos")> CPG = 29
            <EnumMember> <Description("Calca o fotografía digital del NIV del vehículo.")> NIV = 30
            <EnumMember> <Description("Documento con el que se acredite la propiedad de la mercancía")> PRM = 31
            <EnumMember> <Description("Contratos")> CON = 32
            <EnumMember> <Description("Documentación relacionada con la garantía otorgada en términos de los artículos 84-A y 86-A de la L.A")> DGO = 33
            <EnumMember> <Description("Identificación Oficial")> IDO = 34
            <EnumMember> <Description("Comprobante de domicilio")> COM = 35
            <EnumMember> <Description("Documento que ampara el avalúo de las mercancías")> AVM = 36
            <EnumMember> <Description("Documentos de adjudicación judicial de las mercancías")> AJM = 37
            <EnumMember> <Description("Solicitud de retiro de mercancías que causaron abandono")> RMA = 38
            <EnumMember> <Description("Actas")> ACT = 39
            <EnumMember> <Description("Escritos")> ESC = 40
            <EnumMember> <Description("Certificado de peso o volumen")> CPV = 41
            <EnumMember> <Description("Comprobante de la importación temporal de la embarcación debidamente formalizado")> CIT = 42
            <EnumMember> <Description("Comprobante expedido por donataria")> CED = 43
            <EnumMember> <Description("Consulta en la que conste que el vehículo no se encuentra reportado como robado, siniestrado, restringido o prohibido para su circulación en el país de procedencia")> VOK = 44
            <EnumMember> <Description("Clave Unica del Registro de Población")> CUR = 45
            <EnumMember> <Description("Declaración de internación o extracción de cantidades en efectivo y/o documentos por cobrar")> IEC = 46
            <EnumMember> <Description("Declaración de operaciones que no confieren origen en países no parte de acuerdo al TLCI")> ONT = 47
            <EnumMember> <Description("Declaración en la que señalen los motivos por los que efectúa la devolución de mercancías en los términos, de la regla 3.8.4")> MDV = 48
            <EnumMember> <Description("Documentación con información que permita la identificación, análisis y control en términos del artículo 36 de la L.A")> IAC = 49
            <EnumMember> <Description("Documentación que acredite que acepta y subsana la irregularidad")> ASI = 50
            <EnumMember> <Description("Documentación que ampare la importación temporal del vehículo de que se trate")> AIT = 51
            <EnumMember> <Description("Documentación que compruebe que la adquisición de las mercancías fue efectuada cuando se contaba con autorización para operar bajo un Programa IMMEX")> IMM = 52
            <EnumMember> <Description("Documento con base en el cual se determine la procedencia y el origen de las mercancías")> OPM = 53
            <EnumMember> <Description("Documento con que se acredite el reintegro del IVA, en caso de que el contribuyente hubiere obtenido la devolución, o efectuado el acreditamiento de los saldos a favor declarados con motivo de la exportación")> RIV = 54
            <EnumMember> <Description("Documentos previstos en la regla 8.7., fracciones I a IV de la Resolución del TLCAN")> DTL = 55
            <EnumMember> <Description("El Documento que compruebe el cumplimiento de las regulaciones y restricciones no arancelarias")> CRR = 56
            <EnumMember> <Description("Formato denominado Relación de documentos")> FDR = 57
            <EnumMember> <Description("Guía aérea, conocimiento de embarque o carta de porte")> GUI = 58
            <EnumMember> <Description("Hoja con los datos de la matrícula y nombre del barco, el lugar donde se localiza y se indique que la mercancía se encuentra almacenada en los depósitos para combustible del barco para su propio consumo")> INB = 59
            <EnumMember> <Description("Manifiesto de carga")> MAC = 60
            <EnumMember> <Description("Oficios emitidos por autoridad")> OEA = 61
            <EnumMember> <Description("Pedimentos")> PDI = 62
            <EnumMember> <Description("Programa IMMEX")> PIM = 63
            <EnumMember> <Description("Relación de candados")> RCA = 64
            <EnumMember> <Description("Relación de certificados de origen")> RCO = 65
            <EnumMember> <Description("Cuenta de Gastos Complementarias")> CGTC = 66
            <EnumMember> <Description("Comprobante de Gastos Complementario")> CPGC = 67
            <EnumMember> <Description("Manifestacion de Valor Firmada")> MVA = 68
            <EnumMember> <Description("Hoja de Cálculo")> HCA = 69
            <EnumMember> <Description("Comprobante de Valor Electronico")> COV = 70
            <EnumMember> <Description("Carta de instrucciones")> CIN = 71
            <EnumMember> <Description("Factura Comercial para Ventanilla")> FAV = 72
            <EnumMember> <Description("Packing List")> PKL = 73
            <EnumMember> <Description("Sagarpa")> SGP = 74
            <EnumMember> <Description("Certificado de Calidad")> CEC = 75
            <EnumMember> <Description("Certificado de Análisis")> CEA = 76
            <EnumMember> <Description("Certificado MTC")> CEMTC = 77
            <EnumMember> <Description("Permisos previos")> PP = 78
            <EnumMember> <Description("Certificados de Origen ")> CEO = 79
            <EnumMember> <Description("Certificados de Inspección")> CEI = 80
            <EnumMember> <Description("Nota de Entrega")> NE = 81
            <EnumMember> <Description("Carta Técnica")> CT = 82
            <EnumMember> <Description("Certificado Fitosanitario")> CF = 83
            <EnumMember> <Description("Hojas de Requisitos")> HR = 84
            <EnumMember> <Description("Certificado de Salud")> CS = 85
            <EnumMember> <Description("Cicoplafest")> CI = 86
            <EnumMember> <Description("Certificado de Fumigacion (nom-144)")> CEF = 87
            <EnumMember> <Description("Certificado de Estufado")> CEE = 88
            <EnumMember> <Description("Hojas de Seguridad")> HS = 89
            <EnumMember> <Description("Carta Aclaratoria ")> CAL = 90
            <EnumMember> <Description("Permiso sanitario de importacion (cofepris) ")> PES = 91
            <EnumMember> <Description("UVA")> UVA = 92
            <EnumMember> <Description("Cronológico")> CRO = 93
            <EnumMember> <Description("Certificados de Seguridad (NYCE / ANCE)")> CES = 94
            <EnumMember> <Description("Conocimiento de Embarque")> CE = 95
            <EnumMember> <Description("Cuenta Aduanera")> CA = 96
            <EnumMember> <Description("Comprobantes de pago propios XML")> XML_COMPROBANTESPAGOPROPIOS = 101
            <EnumMember> <Description("Comprobantes de pago propios PDF")> PDF_COMPROBANTESPAGOPROPIOS = 102
            <EnumMember> <Description("Comprobantes de pago de terceros XML")> XML_COMPROBANTESPAGOTERCEROS = 103
            <EnumMember> <Description("Comprobantes de pago de terceros PDF")> PDF_COMPROBANTESPAGOTERCEROS = 104
            <EnumMember> <Description("Facturas Propias XML")> XML_FACTURASPROPIAS = 105
            <EnumMember> <Description("Facturas Propias PDF")> PDF_FACTURASPROPIAS = 106
            <EnumMember> <Description("Facturas terceros XML")> XML_FACTURASTERCEROS = 107
            <EnumMember> <Description("Facturas terceros PDF")> PDF_FACTURASTERCEROS = 108

        End Enum

#End Region


#Region "Propiedades"

        Property NombreDocumento As String

        Property EstatusDocumento As EstatusDocumentos

        Property TipoDocumento As TiposDocumento

        WriteOnly Property CargarDesdeRuta As String

        ReadOnly Property DocumentoBytes As Dictionary(Of Int32, Byte)

        ReadOnly Property GetDocumento As Object

        ReadOnly Property AtributosProcesados As Dictionary(Of String, String)

        Property TagWatcherActive As TagWatcher


#End Region


#Region "Metodos"

        Function ProcesarDocumento(ByVal tipoProcesable_ As TiposProcesable) As Object

#End Region

    End Interface

End Namespace
