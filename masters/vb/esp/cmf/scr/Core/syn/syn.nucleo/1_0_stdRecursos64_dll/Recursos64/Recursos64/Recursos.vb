Imports System.ComponentModel
Imports System.Reflection
Imports System.Runtime.Serialization

Namespace Syn.Nucleo

    Public Enum TiposProcesamientoDocumental

        SinDefinir = 0
        NoRequerido = 3
        PDF_aDocumentoElectronico = 1 'OCR para facturas comerciales | Proviene de los clientes, importadores y exportadores
        DocumentoElectronico_aPDF = 2 'FormaCompleja(DocumentoElectonico) a representaciones impresas | Proviene de nuestros procesos normales
        OtraFuente_aDocumentoElectronico = 3 'Archivos M3 a FormaCompleja(DocumentoElectronico) | proviene de corresponsales
    End Enum

    <Serializable()>
    Public Class Recursos

#Region "Enums"

        Public Enum SeccionesGenericas
            <EnumMember> <Description("Sin definir")> SGS0 = 0
            <EnumMember> <Description("Encabezado principal")> SGS1 = 1
            <EnumMember> <Description("Sección")> SGS2 = 2
            <EnumMember> <Description("Sección")> SGS3 = 3
            <EnumMember> <Description("Sección")> SGS4 = 4
            <EnumMember> <Description("Sección")> SGS5 = 5
            <EnumMember> <Description("Sección")> SGS6 = 6
            <EnumMember> <Description("Cuerpo o contenido")> SGS7 = 7
            <EnumMember> <Description("Sección")> SGS8 = 8
            <EnumMember> <Description("Sección")> SGS9 = 9
            <EnumMember> <Description("Sección")> SGS10 = 10
            <EnumMember> <Description("Sección")> SGS12 = 12
            <EnumMember> <Description("Sección")> SGS13 = 13
            <EnumMember> <Description("Pie de página")> SGS14 = 14
        End Enum

        Public Enum SeccionesClientes
            <EnumMember> <Description("Sin definir")> SCS0 = 0
            <EnumMember> <Description("Generales")> SCS1 = 1
            <EnumMember> <Description("Aduana por defecto")> SCS2 = 2
            <EnumMember> <Description("Contactos del imp/exp")> SCS3 = 3
            <EnumMember> <Description("Configuración")> SCS4 = 4
            <EnumMember> <Description("Sellos digitales")> SCS5 = 5
            <EnumMember> <Description("Encargo conferido")> SCS6 = 6
            <EnumMember> <Description("Pago electrónico")> SCS7 = 7
            <EnumMember> <Description("Expediente legal")> SCS8 = 8
        End Enum

        Public Enum CamposGlobales
            'Región del 10000 - 10100
            <EnumMember> <Description("SIN DEFINIR")> SIN_DEFINIR = 10000
            <EnumMember> <Description("Identificador único por tarjeta")> CP_IDENTITY = 10001
        End Enum

        Public Enum CamposDomicilio
            'Región del 1000 - 1999

            <EnumMember> <Description("SIN DEFINIR")> SIN_DEFINIR = 1000
            <EnumMember> <Description("Domicilio fiscal")> CA_DOMICILIO_FISCAL = 1001
            <EnumMember> <Description("Calle")> CA_CALLE = 1002
            <EnumMember> <Description("Número exterior")> CA_NUMERO_EXTERIOR = 1003
            <EnumMember> <Description("Número interior")> CA_NUMERO_INTERIOR = 1004
            <EnumMember> <Description("Numero exterior/interior")> CA_NUMERO_EXT_INT = 1005
            <EnumMember> <Description("Código postal")> CA_CODIGO_POSTAL = 1006
            <EnumMember> <Description("Colonia")> CA_COLONIA = 1007
            <EnumMember> <Description("Localidad")> CA_LOCALIDAD = 1008
            <EnumMember> <Description("Ciudad")> CA_CIUDAD = 1009
            <EnumMember> <Description("Municipio")> CA_MUNICIPIO = 1010
            <EnumMember> <Description("Clave entidad federativa")> CA_CVE_ENTIDAD_FEDERATIVA = 1011
            <EnumMember> <Description("Nombre entidad federativa")> CA_ENTIDAD_FEDERATIVA = 1012
            <EnumMember> <Description("Entidad/Municipio")> CA_ENTIDAD_MUNICIPIO = 1013
            <EnumMember> <Description("Clave del país")> CA_CVE_PAIS = 1014
            <EnumMember> <Description("Nombre del país")> CA_PAIS = 1015
            <EnumMember> <Description("ObjectID Domicilio")> CP_ID_DOMICILIO = 1016
            <EnumMember> <Description("Secuencia Domicilio")> CP_SEC_DOMICILIO = 1017

        End Enum

        Public Enum CamposClientes
            'Región del 2000 - 2999

            <EnumMember> <Description("Identificador de empresa")> CP_CVE_EMPRESA = 2001
            <EnumMember> <Description("Nombre o razón social")> CA_RAZON_SOCIAL = 2002
            <EnumMember> <Description("Clave división mi empresa")> CP_CVE_DIVISION_MI_EMPRESA = 2003
            <EnumMember> <Description("RFC")> CA_RFC_CLIENTE = 2004
            <EnumMember> <Description("Tax id")> CA_TAX_ID = 2005
            <EnumMember> <Description("Curp (OPCIONAL)")> CA_CURP_CLIENTE = 2006
            <EnumMember> <Description("Tipo de persona")> CP_TIPO_PERSONA = 2007
            <EnumMember> <Description("Cliente habilitado")> CP_CLIENTE_HABILITADO = 2008
            <EnumMember> <Description("Cliente extranjero")> CP_CLIENTE_EXTRANJERO = 2009
            <EnumMember> <Description("Lista de aduanas")> CP_ADUANA_PATENTE = 2010
            <EnumMember> <Description("Identificador de la aduana sección")> CP_CVE_ADUANA_SECCION = 2011
            <EnumMember> <Description("Modalidad Aduana sección")> CP_MODALIDAD_ADUANA_SECCION = 2012
            <EnumMember> <Description("Identificador de la patente aduanal")> CP_CVE_PATENTE_ADUANAL = 2013
            <EnumMember> <Description("Patente Aduanal")> CP_PATENTE_ADUANAL = 2014
            <EnumMember> <Description("Tipo de operación")> CP_CVE_TIPO_OPERACION = 2015
            <EnumMember> <Description("Nombre tipo de operación")> CP_TIPO_OPERACION = 2016

            <EnumMember> <Description("Lista de Contactos")> CP_CONTACTO = 2017
            <EnumMember> <Description("Identificador de la persona")> CP_CVE_PERSONA = 2018
            <EnumMember> <Description("Nombre de contacto")> CP_NOMBRE_CONTACTO = 2019
            <EnumMember> <Description("RFC del contacto")> CP_RFC_CONTACTO = 2020
            <EnumMember> <Description("puesto del contacto")> CP_NOMBRE_PUESTO = 2021

            <EnumMember> <Description("Permiso para usar los sellos del agente aduanal")> CP_SELLOS_AGENTE_ADUANAL = 2022
            <EnumMember> <Description("Ruta del archivo .SER")> CP_RUTA_ARCHIVO_SER_SELLOS = 2023
            <EnumMember> <Description("Ruta del archivo .KEY")> CP_RUTA_ARCHIVO_KEY_SELLOS = 2024
            <EnumMember> <Description("Fecha de vigencia del sello")> CP_FECHA_VIGENCIA_SELLOS = 2025
            <EnumMember> <Description("Contraseña del sello")> CP_CONTRASENIA_SELLOS = 2026
            <EnumMember> <Description("Clave del web services")> CP_CVE_WEB_SERVICES_SELLOS = 2027

            <EnumMember> <Description("Lista de encargos conferidos")> CP_ENCARGO_CONFERIDO = 2028
            <EnumMember> <Description("Patentes Aduanales")> CP_PATENTES_ADUANALES_ENCARGO = 2029
            <EnumMember> <Description("Identificador de la patente aduanal")> CP_CVE_PATENTE_ADUANAL_ENCARGO = 2030
            <EnumMember> <Description("Patente Aduanal del encargo")> CP_PATENTE_ADUANAL_ENCARGO = 2031
            <EnumMember> <Description("Encargo activo")> CP_ACTIVO_ENCARGO = 2032
            <EnumMember> <Description("Fecha de inicio del encargo")> CP_FECHA_INICIO_ENCARGO = 2033
            <EnumMember> <Description("Fecha de fin del encargo")> CP_FECHA_FIN_ENCARGO = 2034

            <EnumMember> <Description("Lista pago electrónico")> CP_PAGO_ELECTRONICO = 2035
            <EnumMember> <Description("Nombre tipo de operación")> CP_TIPO_OPERACION_PAGO = 2037
            <EnumMember> <Description("patente/Aduana")> CP_PATENTE_ADUANA_SECCION_PAGO = 2038
            <EnumMember> <Description("Identificador de la clave de documento")> CP_CVE_DOCUMENTO = 2039
            <EnumMember> <Description("Clave de documento")> CP_CVE_DOCUMENTO_PAGO = 2040
            <EnumMember> <Description("Identificador del banco")> CP_CVE_BANCO = 2041
            <EnumMember> <Description("Clave del banco")> CP_CVE_BANCO_PAGO = 2042
            <EnumMember> <Description("Número de cuenta")> CP_ID_CUENTA_PAGO = 2043
            <EnumMember> <Description("Lista de rango de pago")> CP_RANGO_PAGO = 2044
            <EnumMember> <Description("Rango mínimo")> CP_RANGO_MINIMO_PAGO = 2045
            <EnumMember> <Description("Rango máximo")> CP_RANGO_MAXIMO_PAGO = 2046
            <EnumMember> <Description("Cuenta por defecto")> CP_CUENTA_DEFAULT_PAGO = 2047

            <EnumMember> <Description("Lista de expediente legal")> CP_EXPEDIENTE_LEGAL = 2048
            <EnumMember> <Description("Identificador de la plantilla")> CP_CVE_NOMBRE_PLANTILLA = 2049
            <EnumMember> <Description("Plantilla")> CP_NOMBRE_PLANTILLA = 2050
            <EnumMember> <Description("Ruta del documento")> CP_RUTA_DOCUMENTO = 2051
            <EnumMember> <Description("Vigencia del documento")> CP_VIGENCIA_DOCUMENTO = 2052

            <EnumMember> <Description("ObjectID Empresa")> CP_ID_EMPRESA = 2053

            <EnumMember> <Description("Informacióm del contacto")> CP_INFO_CONTACTO = 2054
            <EnumMember> <Description("Telefono1 del contacto")> CP_TELEFONO1_CONTACTO = 2055
            <EnumMember> <Description("Móvil del contacto")> CP_MOVIL_CONTACTO = 2056
            <EnumMember> <Description("email del contacto")> CP_EMAIL_CONTACTO = 2057

            <EnumMember> <Description("Estatus del pago")> CP_ACTIVO_PAGO = 2058

            <EnumMember> <Description("ObjectId Cliente")> CP_OBJECTID_CLIENTE = 2059
            <EnumMember> <Description("Secuencia de Cliente")> CP_CVE_CLIENTE = 2060

            'CP_ACTIVO_PAGO


        End Enum

        Public Enum EventosGenericos
            SinDefinir = 0
            Insercion = 1
            AntesInsertar = 2
            DespuesInsertar = 3
            Actualizacion = 4
            AntesDeActualizar = 5
            DespuesDeDeActualizar = 6
            Eliminacion = 7
            AntesDeEliminar = 8
            DespuesDeEliminar = 9
            Consulta = 10
            ErrorGenerado = 11
        End Enum

        'Tipos vinculación
        Public Enum TiposVinculacion
            SinDefinir = 0
            AgenciaAduanal
            ProveedorDeAgenciaAduanal
            SucursalAgenciaAduanal
            CorresponsalAgenciaAduanal
            ImportadorOExportador
            ProveedorExtranjeroDelIOE
            ProveedorNacionalDelIOE
            AutoridadNacional
        End Enum

        'Tipos bloque
        Public Enum TiposBloque
            SinDefinir = 0
            Encabezado = 1
            EncabezadoPaginasSecundarias = 2
            Cuerpo = 3
            PiePagina = 4
        End Enum

        'Documentos electrónicos /VOCE
        Enum TiposDocumentoElectronico

            SinDefinir = 0

            'Relacionados con el pedimento aduanal
            PedimentoNormal = 1
            PedimentoRectificacion = 2
            PedimentoComplementario = 3
            PedimentoGlobalComplementario = 4
            PedimentoTransitoInterno = 5
            PedimentoTransitoInternacional = 6

            'Otros formatos electronicos
            XMLCFDiNacional = 11
            XMLDODA = 12
            XMLCOVE = 13

            'Otros

            Referencia = 14

            FacturaComercial = 15

            Clientes = 16

            ProveedoresOperativos = 17

            Revalidacion = 18

            Viajes = 19

            Productos = 20

            TarifaArancelaria = 21

            AcuseValor = 22

            ManifestacionValor = 23
        End Enum

        'Para representaciones impresas/ formatos PDF
        Enum TiposDocumentoDigital
            SinDefinir = 0
            'Formas del pedimento
            PedimentoNormalPDF = 1
            PedimentoComplementarioPDF = 4
            RectificacionPDF = 9

            'Partes II
            PedimentoImportacionParteIIPDF = 11
            PedimentoExportacionParteIIPDF = 12

            'Avisos electrónicos
            FormatoAvisoConsolidado = 13

            'Otros documentos digitales
            FacturaComercialExtranjera = 14
            FacturaComercialNacional = 15
            ConocimientoDeEmbarque = 16
            CertificadoDeOrigen = 17
            PDFDODA = 18
            PDFAcuseValor = 19

        End Enum

        'Para el tamaño de la fuente en la creación del PDF
        Public Enum Fuentes
            Pequenio = 8
            Medio = 9
            Mediano = 10
            Grande = 11
            Normal = 12
        End Enum

        'Para el estilo de una celda en la creación de un PDF
        Public Enum Bordes

            SinBordes = 0
            BordeSuperior = 1
            BordeInferior = 2
            Bordes = 3
            BordeIzquierdo = 4
            BordeIzqInf = 5
            BordeDerInf = 6
            BordeDerIzq = 7
            BordeDerecho = 8
            BordeDerIzqInf = 9

        End Enum

        'Para la alineación del texto dentro de una celdas en la creación de un PDF
        Public Enum Alineaciones
            AlIzquierdo = 0
            AlCentro = 1
            AlDerecha = 2
            AlJustificado = 3
        End Enum

#End Region

#Region "Funciones"
        Public Shared Function GetEnumDescription(ByVal EnumConstant As [Enum]) As String

            Dim fi As FieldInfo = EnumConstant.GetType().GetField(EnumConstant.ToString())

            Dim attr() As DescriptionAttribute =
                          DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute),
                          False), DescriptionAttribute())


            If attr.Length > 0 Then

                Return attr(0).Description

            Else

                Return EnumConstant.ToString()

            End If

        End Function

#End Region

    End Class

End Namespace