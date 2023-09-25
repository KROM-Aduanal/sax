Imports System.ComponentModel
Imports System.Reflection
Imports System.Runtime.Serialization

Namespace Syn.Nucleo

    <Serializable()>
    Public Class RecursosComercioExterior
        Inherits Recursos

#Region "Attributes"

#End Region

        'UN = Sin definir
        'AN = Anexo22
        'VO = VOCE
        'SG = Generico
        'SCS = Clientes
        'SFAC = Factura Comercial
        'SPRO = Secciones Proveedor Operativo

        Enum SeccionesPedimento

            <EnumMember> <Description("Sin definir")> UNS00 = 0

            '#############################  SECCIONES ÚNICAS DEL ANEXO 22 ##################################
            <EnumMember> <Description("Encabezado principal del pedimento")> ANS1 = 1
            <EnumMember> <Description("Encabezado para páginas secundarias del pedimento")> ANS2 = 2
            <EnumMember> <Description("Datos del importador/exportador")> ANS3 = 3
            <EnumMember> <Description("Datos generales del pedimento complementario")> ANS4 = 4
            <EnumMember> <Description("Prueba suficiente")> ANS5 = 5
            <EnumMember> <Description("Tasas a nivel pedimento")> ANS6 = 6
            <EnumMember> <Description("Cuadro de liquidación")> ANS7 = 7
            <EnumMember> <Description("Informe de la industria automotriz")> ANS8 = 8
            <EnumMember> <Description("Deposito referenciado - línea de captura - información del pago")> ANS9 = 9
            <EnumMember> <Description("Datos del proveedor o comprador")> ANS10 = 10
            <EnumMember> <Description("Datos del destinatario")> ANS11 = 11
            <EnumMember> <Description("Datos del transporte y transportista")> ANS12 = 12
            <EnumMember> <Description("CFDi o ducumento equivalente ")> ANS13 = 13
            <EnumMember> <Description("Fechas")> ANS14 = 14
            <EnumMember> <Description("Candados")> ANS15 = 15
            <EnumMember> <Description("Guias, Manifiestos, conocimientos de embarque o documentos de transporte")> ANS16 = 16
            <EnumMember> <Description("Contenedores/Equipo ferrocarril/Número economico del vehiculo.")> ANS17 = 17
            <EnumMember> <Description("Identificadores (Nivel pedimento)")> ANS18 = 18
            <EnumMember> <Description("Cuentas aduaneras y cuentas aduaneras de garantia (Nivel pedimento)")> ANS19 = 19
            <EnumMember> <Description("Descargos")> ANS20 = 20
            <EnumMember> <Description("Compensaciones")> ANS21 = 21
            <EnumMember> <Description("Documentos que amparan las formas de pago distintas a efectivo….")> ANS22 = 22
            <EnumMember> <Description("Observaciones ( nivel pedimento)")> ANS23 = 23
            <EnumMember> <Description("Partidas")> ANS24 = 24
            <EnumMember> <Description("Mercancias")> ANS25 = 25
            <EnumMember> <Description("Regulaciones y restricciones no arancelarias")> ANS26 = 26
            <EnumMember> <Description("Identificadores ( Nivel partida )")> ANS27 = 27
            <EnumMember> <Description("Cuentas aduaneras de garantia ( Nivel partida)")> ANS28 = 28
            <EnumMember> <Description("Tasas y contribuciones a nivel partida")> ANS29 = 29
            <EnumMember> <Description("Contribuciones a nivel partida")> ANS30 = 30
            <EnumMember> <Description("Partidas del informe de la industria automotriz")> ANS31 = 31
            <EnumMember> <Description("Determinación de contribuciones a nivel partida al amparo del Art 2.5 del T-MEC")> ANS32 = 32
            <EnumMember> <Description("Detalle de importación a EUA/CAN al amparo del Art. 2.5 del T-MEC")> ANS33 = 33
            <EnumMember> <Description("Determinación y/o pago de contribuciones por aplicación del art 2.5 del TMEC en el pedimento de exporación(Retorno)")> ANS34 = 34
            <EnumMember> <Description("Pago de contribuciones a nivel partida por aplicación del Art. 2.5 del T-MEC.")> ANS35 = 35
            <EnumMember> <Description("Observaciones ( Nivel partida )")> ANS36 = 36
            <EnumMember> <Description("Rectificaciones")> ANS37 = 37
            <EnumMember> <Description("Diferencias de contribuciones ( Nivel pedimento )")> ANS38 = 38
            <EnumMember> <Description("Prueba suficiente")> ANS39 = 39
            <EnumMember> <Description("Encabezado para determinacion de contribuciones a nivel partdida para pedimentos complementarios al amparo del art. T-Mec.")> ANS40 = 40
            <EnumMember> <Description("Encabezado para determinación de contribuciones a nivel partida para pedimentos complementarios al amparo del los articulos 14 de la decision o 15 del TLCAELC")> ANS41 = 41
            <EnumMember> <Description("Instructivo de llenado del pedimento de tránsito para el transbordo")> ANS42 = 42
            <EnumMember> <Description("Fin de pedimento")> ANS43 = 43
            <EnumMember> <Description("Pie de pagina del pedimento")> ANS44 = 44
            <EnumMember> <Description("Desglose de contribuciones del cuadro de liquidación")> ANS55 = 55
            <EnumMember> <Description("Desglose de diferencias del cuadro de diferencias de contribuciones")> ANS56 = 56
            '#############################  SECCIONES ÚNICAS DEL ANEXO 22 ##################################

        End Enum

        Public Enum SeccionesFacturaComercial
            <EnumMember> <Description("Sin definir")> SFAC0 = 0

            '#############################  SECCIONES ÚNICAS DE LA FACTURA COMERCIAL ##################################
            <EnumMember> <Description("Generales")> SFAC1 = 1
            <EnumMember> <Description("Datos del proveedor")> SFAC2 = 2
            <EnumMember> <Description("Datos del destinatario")> SFAC3 = 3
            <EnumMember> <Description("Partidas")> SFAC4 = 4
            <EnumMember> <Description("Incrementables")> SFAC5 = 5
            <EnumMember> <Description("Subdivisión")> SFAC6 = 6
            'Se comenta para que un futuro se pueda dividir la forma de guardar los item de la factura (si es que es optimo)
            '<EnumMember> <Description("Partida - factura")> SFAC7 = 7
            '<EnumMember> <Description("Partida - clasificación")> SFAC8 = 8
            '<EnumMember> <Description("Partida - detalle mercancía")> SFAC9 = 9
            '#############################  SECCIONES ÚNICAS  DE LA FACTURA COMERCIAL ##################################

        End Enum

        Public Enum SeccionesAcuseValor
            <EnumMember> <Description("Sin definir")> SAcuseValor0 = 0

            '#############################  SECCIONES ÚNICAS DE MODULO DE ACUSE DE VALOR ##################################
            <EnumMember> <Description("Generales")> SAcuseValor1 = 1
            <EnumMember> <Description("Datos del proveedor")> SAcuseValor2 = 2
            <EnumMember> <Description("Datos del destinatario")> SAcuseValor3 = 3
            <EnumMember> <Description("Partidas y Detalles")> SAcuseValor4 = 4
            <EnumMember> <Description("Configuración")> SAcuseValor5 = 5

            '#############################  SECCIONES ÚNICAS  DE MÓDULO DE ACUSE DE VALOR ##################################

        End Enum

        Public Enum SeccionesReferencias
            <EnumMember> <Description("Sin definir")> SREF0 = 0
            <EnumMember> <Description("Generales")> SREF1 = 1
            <EnumMember> <Description("Cliente")> SREF2 = 2
            <EnumMember> <Description("Detalles Cliente")> SREF3 = 3
            <EnumMember> <Description("Seguimiento")> SREF4 = 4
            <EnumMember> <Description("Datos Adicionales")> SREF5 = 5
        End Enum

        Public Enum SeccionesProvedorOperativo
            <EnumMember> <Description("Sin definir")> SPRO0 = 0

            '#############################  SECCIONES ÚNICAS DEL PROVEEDOR OPERATIVO ##################################
            <EnumMember> <Description("Generales")> SPRO1 = 1
            <EnumMember> <Description("DetalleProveedor")> SPRO2 = 2
            <EnumMember> <Description("DomiciliosFiscales")> SPRO3 = 3
            <EnumMember> <Description("VinculacionesClientes")> SPRO4 = 4
            <EnumMember> <Description("ConfiguracionAdicional")> SPRO5 = 5
            '#############################  SECCIONES ÚNICAS  DEL PROVEEDOR OPERATIVO ##################################
        End Enum

        Public Enum SeccionesRevalidacion
            <EnumMember> <Description("Sin definir")> SREV0 = 0
            <EnumMember> <Description("Generales")> SREV1 = 1
            <EnumMember> <Description("DatosRevalidacion")> SREV2 = 2
            <EnumMember> <Description("CargaSuelta")> SREV3 = 3
            <EnumMember> <Description("Contenedores")> SREV4 = 4
        End Enum

        Public Enum SeccionesViajes
            <EnumMember> <Description("Sin definir")> SVIA0 = 0
            <EnumMember> <Description("Generales")> SVIA1 = 1
            <EnumMember> <Description("DatosOperativos")> SVIA2 = 2
            <EnumMember> <Description("DatosAdicionales")> SVIA3 = 3
            <EnumMember> <Description("Referencias")> SVIA4 = 4
        End Enum

        Public Enum SeccionesProducto
            <EnumMember> <Description("Sin definir")> SPTO0 = 0
            <EnumMember> <Description("Generales")> SPTO1 = 1
            <EnumMember> <Description("Clasificacion")> SPTO2 = 2
            <EnumMember> <Description("DescipcionesFacturas")> SPTO3 = 3
            <EnumMember> <Description("Historico Clasificacion")> SPTO4 = 4
            <EnumMember> <Description("Catalogo DescripcionesFacturas")> SPTO5 = 5
        End Enum

        Public Enum SeccionesTarifaArancelaria
            <EnumMember> <Description("Sin definir")> TIGIE0 = 0
            <EnumMember> <Description("Generales")> TIGIE1 = 1
            <EnumMember> <Description("Importacion")> TIGIE2 = 2
            <EnumMember> <Description("Exportacion")> TIGIE3 = 3
            <EnumMember> <Description("Regulaciones Arancelarias")> TIGIE4 = 4
            <EnumMember> <Description("Regulaciones no Arancelarias")> TIGIE5 = 5

            <EnumMember> <Description("Tratados comerciales")> TIGIE6 = 6
            <EnumMember> <Description("Paised afiliados")> TIGIE7 = 7
            <EnumMember> <Description("Cupos arancel")> TIGIE8 = 8
            <EnumMember> <Description("IEPS")> TIGIE9 = 9
            <EnumMember> <Description("Cuotas compensatorias")> TIGIE10 = 10
            <EnumMember> <Description("Precios estimados")> TIGIE11 = 11

            <EnumMember> <Description("Permisos instituciones")> TIGIE12 = 12
            <EnumMember> <Description("Permisos")> TIGIE13 = 13
            <EnumMember> <Description("Normas oficiales mexicanas")> TIGIE14 = 14
            <EnumMember> <Description("Anexos")> TIGIE15 = 15
            <EnumMember> <Description("Embargos")> TIGIE16 = 16
            <EnumMember> <Description("Cupos Mínimos")> TIGIE17 = 17
            <EnumMember> <Description("Padron Sectorial")> TIGIE18 = 18

            <EnumMember> <Description("Impuestos")> TIGIE19 = 19

        End Enum

        Public Enum SeccionesManifestacionValor
            <EnumMember> <Description("Sin definir")> SMV0 = 0



            '#############################  SECCIONES ÚNICAS DE LA MANIFESTACIÓN DE VALOR ##################################
            <EnumMember> <Description("Generales")> SMV1 = 1
            <EnumMember> <Description("Datos del proveedor")> SMV2 = 2
            <EnumMember> <Description("Datos del importador")> SMV3 = 3
            <EnumMember> <Description("facturas")> SMV4 = 4
            <EnumMember> <Description("Incrementables")> SMV5 = 5
            <EnumMember> <Description("Anexos")> SMV6 = 6
            <EnumMember> <Description("Valor de tansacción")> SMV7 = 7
            <EnumMember> <Description("Anexa Doc art 66")> SMV8 = 8
            <EnumMember> <Description("No anexa Doc art 66")> SMV9 = 9
            <EnumMember> <Description("Anexa Doc art 65")> SMV10 = 10
            <EnumMember> <Description("No anexa Doc art 65")> SMV11 = 11
            '#############################  SECCIONES ÚNICAS  DE LA MANIFESTACIÓN DE VALOR ##################################



        End Enum
        Public Enum CamposPedimento
            'Región del 1 - 999

            'Abreviaciones genenerales
            'IOE = IMPORTADOR O EXPORTADOR
            'POC = PROVEEDOR O COMPRADOR
            'AAD = AGENTE ADUANAL
            'SAD = SECCIÓN ADUANA DE DESPACHO
            'SAE = SECCIÓN ADUANA DE ENTRADA
            'SES = SECCIÓN ADUANA DE ENTRADA O SALIDA

            <EnumMember> <Description("SIN DEFINIR")> SIN_DEFINIR = 0

            '#############################  CAMPOS ÚNICOS DE LA AUTORIDAD ##################################
            <EnumMember> <Description("NUM. PEDIMENTO. COMPLETO")> CA_NUM_PEDIMENTO_COMPLETO = 1
            <EnumMember> <Description("T.OPER.")> CA_T_OPER = 2
            <EnumMember> <Description("CVE. PEDIMENTO.")> CA_CVE_PEDIMENTO = 3
            <EnumMember> <Description("REGIMEN")> CA_REGIMEN = 4
            <EnumMember> <Description("DESTINO/ORIGEN")> CA_DESTINO_ORIGEN = 5
            <EnumMember> <Description("TIPO CAMBIO")> CA_TIPO_CAMBIO = 6
            <EnumMember> <Description("PESO BRUTO")> CA_PESO_BRUTO = 7
            <EnumMember> <Description("ADUANA E/S")> CA_ADUANA_E_S = 8
            <EnumMember> <Description("MEDIO DE TRANSPORTE")> CA_MEDIO_DE_TRANSPORTE = 9
            <EnumMember> <Description("MEDIO DE TRANSPORTE DE ARRIBO")> CA_MEDIO_DE_TRANSPORTE_DE_ARRIBO = 10
            <EnumMember> <Description("MEDIO DE TRANSPORTE DE SALIDA")> CA_MEDIO_DE_TRANSPORTE_DE_SALIDA = 11
            <EnumMember> <Description("VALOR DOLARES")> CA_VALOR_DOLARES = 12
            <EnumMember> <Description("VALOR ADUANA")> CA_VALOR_ADUANA = 13
            <EnumMember> <Description("PRECIO PAGAGO/VALOR COMERCIAL")> CA_PRECIO_PAGADO_O_VALOR_COMERCIAL = 14
            <EnumMember> <Description("RFC DEL IMPORTADOR/EXPORTADOR")> CA_RFC_DEL_IOE = 15
            <EnumMember> <Description("CURP DEL IMPORTADOR/EXPORTADOR")> CA_CURP_DEL_IOE = 16
            <EnumMember> <Description("NOMBRE, DENOMINACIÓN SOCIAL DEL IMPORTADOR/EXPORTADOR")> CA_RAZON_SOCIAL_IOE = 17
            <EnumMember> <Description("DOMICILIO DEL IMPORTADOR / EXPORTADOR")> CA_DOMICILIO_IOE = 18
            <EnumMember> <Description("VAL. SEGUROS")> CA_VAL_SEGUROS = 19
            <EnumMember> <Description("SEGUROS")> CA_SEGUROS = 20
            <EnumMember> <Description("FLETES")> CA_FLETES = 21
            <EnumMember> <Description("EMBALAJES")> CA_EMBALAJES = 22
            <EnumMember> <Description("OTROS INCREMENTABLES")> CA_OTROS_INCREMENTABLES = 23
            <EnumMember> <Description("TRANSPORTE DECREMENTABLES")> CA_TRANSPORTE_DECREMENTABLES = 24
            <EnumMember> <Description("SEGURO DECREMENTABLES")> CA_SEGURO_DECREMENTABLES = 25
            <EnumMember> <Description("CARGA DECREMENTABLES")> CA_CARGA_DECREMENTABLES = 26
            <EnumMember> <Description("DESCARGA DECREMENTABLES")> CA_DESCARGA_DECREMENTABLES = 27
            <EnumMember> <Description("OTROS DECREMENTABLES")> CA_OTROS_DECREMENTABLES = 28
            <EnumMember> <Description("ACUSE ELECTRÓNICO DE VALIDACIÓN")> CA_ACUSE_ELECTONICO_DE_VALIDACION = 29
            <EnumMember> <Description("CÓDIGO DE BARRAS")> CA_CODIGO_DE_BARRAS = 30
            <EnumMember> <Description("CLAVE DE LA SECCIÓN ADUANERA DE DESPACHO")> CA_CLAVE_SAD = 31
            <EnumMember> <Description("MARCAS, NÚMEROS Y TOTAL DE BULTOS")> CA_MARCAS_NUMEROS_TOTAL_BULTOS = 32
            <EnumMember> <Description("FECHA ENTRADA")> CA_FECHA_ENTRADA = 33
            <EnumMember> <Description("FECHA PAGO")> CA_FECHA_PAGO = 34
            <EnumMember> <Description("FECHA EXTRACCIÓN")> CA_FECHA_EXTRACCION = 35
            <EnumMember> <Description("FECHA PRESENTACION")> CA_FECHA_PRESENTACION = 36
            <EnumMember> <Description("FECHA IMP.EUA/CAN")> CA_FECHA_IMP_EUA_CAN = 37
            <EnumMember> <Description("FECHA ORIGINAL")> CA_FECHA_ORIGINAL = 38
            <EnumMember> <Description("CONTRIB")> CA_CONTRIBUCION = 39
            <EnumMember> <Description("CLAVE T. TASA.")> CA_CVE_T_TASA = 40
            <EnumMember> <Description("TASA")> CA_TASA = 41
            <EnumMember> <Description("CONCEPTO")> CA_CONCEPTO = 42
            <EnumMember> <Description("FORMA DE PAGO")> CA_FP = 43
            <EnumMember> <Description("IMPORTE")> CA_IMPORTE = 44
            <EnumMember> <Description("EFECTIVO")> CA_EFECTIVO = 45
            <EnumMember> <Description("OTROS")> CA_OTROS = 46
            <EnumMember> <Description("TOTAL")> CA_TOTAL = 47
            <EnumMember> <Description("CERTIFICACIÓN")> CA_CERTIFICACION = 48
            <EnumMember> <Description("DEPÓSITO REFERENCIADO Y EN SU CASO LA IMPRESIÓN DEL PAGO ELECTRÓNICO CONFORME AL APENDICE 23")> CA_DEP_REFERENCIADO = 49
            <EnumMember> <Description("NOMBRE, DENOMINACIÓN O RAZ. SOC.")> CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA = 50
            <EnumMember> <Description("RFC.")> CA_RFC_AA = 51
            <EnumMember> <Description("CURP.")> CA_CURP_AA_O_REP_LEGAL = 52
            <EnumMember> <Description("NOMBRE")> CA_NOMBRE_MAND_REP_AA = 53
            <EnumMember> <Description("RFC.")> CA_RFC_MAND_O_AGAD_REP_ALMACEN = 54
            <EnumMember> <Description("CURP.")> CA_CURP_MAND_O_AGAD_REP_ALMACEN = 55
            <EnumMember> <Description("PATENTE O AUTORIZACION.")> CA_PATENTE = 56
            <EnumMember> <Description("FIRMA ELECTRONICA AVANZADA.")> CA_EFIRMA = 57
            <EnumMember> <Description("NUM. DE SERIE DEL CERTIFICADO.")> CA_CERTIFICADO_FIRMA = 58
            <EnumMember> <Description("FIN DEL PEDIMENTO.")> CA_FIN_PEDIMENTO = 59
            <EnumMember> <Description("ID. FISCAL.")> CA_ID_FISCAL_PROVEEDOR = 60
            <EnumMember> <Description("NOMBRE, DENOMINACION O RAZON SOCIAL.")> CA_NOMBRE_DEN_RAZON_SOC_POC = 61
            <EnumMember> <Description("DOMICILIO.")> CA_DOMICILIO_POC = 62
            <EnumMember> <Description("VINCULACION.")> CA_VINCULACION = 63
            <EnumMember> <Description("NUM. CFDI O DOCUMENTO EQUIVALENTE.")> CA_CFDI_O_FACT = 64
            <EnumMember> <Description("FECHA.")> CA_FECHA_FACT = 65
            <EnumMember> <Description("INCOTERM.")> CA_INCOTERM = 66
            <EnumMember> <Description("MONEDA FACT.")> CA_CVE_MONEDA_FACT = 67
            <EnumMember> <Description("VAL. MON. FACT.")> CA_MONTO_MONEDA_FACT = 68
            <EnumMember> <Description("FACTOR MON. FACT.")> CA_FACTOR_MONEDA = 69
            <EnumMember> <Description("VAL. DOLARES.")> CA_MONTO_USD = 70
            <EnumMember> <Description("ID. FISCAL.")> CA_ID_FISCAL_DESTINATARIO = 71
            <EnumMember> <Description("NOMBRE, DENOMINACION O RAZON SOCIAL.")> CA_NOMBRE_RAZON_SOC_DESTINATARIO = 72
            <EnumMember> <Description("DOMICILIO.")> CA_DOMICILIO_DESTINATARIO = 73
            <EnumMember> <Description("IDENTIFICACION.")> CA_ID_TRANSPORT = 74
            <EnumMember> <Description("PAIS.")> CA_CVE_PAIS_TRANSP = 75
            <EnumMember> <Description("TRANSPORTISTA.")> CA_NOMBRE_RAZON_SOC_TRANSP = 76
            <EnumMember> <Description("RFC.")> CA_CVE_RFC_TRANSP = 77
            <EnumMember> <Description("CURP.")> CA_CURP_TRANSP_PERSONA_FISICA = 78
            <EnumMember> <Description("DOMICILIO/CIUDAD/ESTADO.")> CA_DOMICILIO_TRANSP = 79
            <EnumMember> <Description("NUMERO DE CANDADO.")> CA_NUM_CANDADO = 80
            <EnumMember> <Description("1RA. REVISION.")> CA_NUM_CANDADO_1RA = 81
            <EnumMember> <Description("2DA. REVISION.")> CA_NUM_CANDADO_2DA = 82
            <EnumMember> <Description("NUMERO (GUIA/CONOCIMIENTO EMBARQUE) DOCUMENTOS DE TRANSPORTE")> CA_GUIA_O_MANIF_O_BL = 83
            <EnumMember> <Description("ID.")> CA_MASTER_O_HOUSE = 84
            <EnumMember> <Description("NUMERO DE CONTENEDOR/EQUIPO FERROCARRIL/NUMERO ECONOMICO DEL VEHICULO.")> CA_NUM_CONTENEDOR_FERRO_NUM_ECON = 85
            <EnumMember> <Description("TIPO DE CONTENEDOR/EQUIPO FERROCARRIL/NUMERO ECONOMICO DEL VEHICULO.")> CA_CVE_TIPO_CONTENEDOR = 86
            <EnumMember> <Description("CLAVE.")> CA_CVE_IDENTIFICADOR_G = 87
            <EnumMember> <Description("COMPL. IDENTIFICADOR 1.")> CA_COMPL_1 = 88
            <EnumMember> <Description("COMPL. IDENTIFICADOR 2.")> CA_COMPL_2 = 89
            <EnumMember> <Description("COMPL. IDENTIFICADOR 3.")> CA_COMPL_3 = 90
            <EnumMember> <Description("TIPO CUENTA.")> CA_CVE_CTA_ADUANERA = 91
            <EnumMember> <Description("CLAVE DE GARANTIA.")> CA_CVE_TIPO_GARANTIA = 92
            <EnumMember> <Description("INSTITUCION EMISORA.")> CA_NOMBRE_INST_EMISORA_CTA = 93
            <EnumMember> <Description("NUMERO DE CONTRATO.")> CA_NUM_CONTRATO = 94
            <EnumMember> <Description("FOLIO CONSTANCIA.")> CA_FOLIO_CONSTANCIA = 95
            <EnumMember> <Description("TOTAL DEPOSITO.")> CA_IMPORTE_TOTAL_CONSTANCIA = 96
            <EnumMember> <Description("FECHA CONSTANCIA.")> CA_FECHA_EMISION_CONSTANCIA = 97
            <EnumMember> <Description("NUM. PEDIMENTO ORIGINAL COMPLETO")> CA_NUM_PEDIM_ORIGINAL_COMPLETO = 98
            <EnumMember> <Description("FECHA DE OPERACION ORIGINAL.")> CA_FECHA_PEDIM_ORIGINAL = 99
            <EnumMember> <Description("CVE. PEDIMENTO ORIGINAL.")> CA_CVE_PEDIM_ORIGINAL = 100
            <EnumMember> <Description("FECHA DE OPERACION ORIGINAL.")> CA_FECHA_PAGO_ORIG_PARA_COMPENSAC = 101
            <EnumMember> <Description("DEPENDENCIA O INSTITUCION EMISORA.")> CA_NOMBRE_INST_EMISORA_DOCTO = 102
            <EnumMember> <Description("NUMERO DEL DOCUMENTO.")> CA_NUM_DOCTO = 103
            <EnumMember> <Description("FECHA DEL DOCUMENTO.")> CA_FECHA_EXP_DOCTO = 104
            <EnumMember> <Description("IMPORTE DEL DOCUMENTO.")> CA_MONTO_DOCTO = 105
            <EnumMember> <Description("SALDO DISPONIBLE.")> CA_SALDO_DISP_DOCTO = 106
            <EnumMember> <Description("IMPORTE A PAGAR.")> CA_MONTO_PAG_PEDIM = 107
            <EnumMember> <Description("OBSERVACIONES.")> CA_OBSERV_PEDIM = 108
            <EnumMember> <Description("SEC.")> CA_NUM_SEC_PARTIDA = 109
            <EnumMember> <Description("FRACCION.")> CA_FRACC_ARANC_PARTIDA = 110
            <EnumMember> <Description("SUBD. / NÚM. IDENTIFICACIÓN COMERCIAL")> CA_NICO_PARTIDA = 111
            <EnumMember> <Description("MET. VAL.")> CA_CVE_MET_VALOR_PARTIDA = 112
            <EnumMember> <Description("UMC.")> CA_CVE_UMC_PARTIDA = 113
            <EnumMember> <Description("CANTIDAD UMC.")> CA_CANT_UMC_PARTIDA = 114
            <EnumMember> <Description("UMT.")> CA_CVE_UMT_PARTIDA = 115
            <EnumMember> <Description("CANTIDAD UMT")> CA_CANT_UMT_PARTIDA = 116
            <EnumMember> <Description("P. V/C.")> CA_CVE_PAIS_VEND_O_COMP_PARTIDA = 117
            <EnumMember> <Description("P. O/D.")> CA_CVE_PAIS_ORIGEN_O_DEST_PARTIDA = 118
            <EnumMember> <Description("DESCRIPCION (RENGLONES VARIABLES SEGUN SE REQUIERA).")> CA_DESCRIP_MERC_PARTIDA = 119
            <EnumMember> <Description("VAL. ADU/VAL. USD.")> CA_VAL_ADU_O_VAL_USD_PARTIDA = 120
            <EnumMember> <Description("IMP. PRECIO PAG./VALOR COMERCIAL.")> CA_IMP_PRECIO_PAG_O_VAL_COMER_PARTIDA = 121
            <EnumMember> <Description("PRECIO UNIT.")> CA_MONTO_PRECIO_UNITARIO_PARTIDA = 122
            <EnumMember> <Description("VAL. AGREG.")> CA_MONTO_VALOR_AGREG_PARTIDA = 123
            <EnumMember> <Description("MARCA.")> CA_NOMBRE_MARCA_PARTIDA = 124
            <EnumMember> <Description("MODELO.")> CA_CVE_MODELO_PARTIDA = 125
            <EnumMember> <Description("CODIGO PRODUCTO.")> CA_CODIGO_PRODUCTO_PARTIDA = 126
            <EnumMember> <Description("CONTRIB A NIVEL PARTIDA")> CA_CONTRIBUCION_PARTIDA = 127
            <EnumMember> <Description("NIV/NUM. SERIE.")> CA_VIN_O_NUM_SERIE_MERCA = 128
            <EnumMember> <Description("KILOMETRAJE.")> CA_KILOM = 129
            <EnumMember> <Description("PERMISO.")> CA_CVE_PERMISO = 130
            <EnumMember> <Description("NUMERO DE PERMISO.")> CA_NUM_PERMISO = 131
            <EnumMember> <Description("FIRMA DESCARGO.")> CA_FIRM_ELECTRON_PERMISO = 132
            <EnumMember> <Description("VAL. COM. DLS.")> CA_MONTO_USD_VAL_COM = 133
            <EnumMember> <Description("CANTIDAD UMT/C.")> CA_CANT_UMT_O_UMC = 134
            <EnumMember> <Description("IDENTIF.")> CA_CVE_IDENTIF_PARTIDA = 135
            <EnumMember> <Description("COMPLEMENTO 1.")> CA_COMPL_1_PARTIDA = 136
            <EnumMember> <Description("COMPLEMENTO 2.")> CA_COMPL_2_PARTIDA = 137
            <EnumMember> <Description("COMPLEMENTO 3.")> CA_COMPL_3_PARTIDA = 138
            <EnumMember> <Description("CVE. GAR.")> CA_CVE_TIPO_GARANTIA_PARTIDA = 139
            <EnumMember> <Description("INST. EMISORA.")> CA_INST_EMISORA_GARANTIA_PARTIDA = 140
            <EnumMember> <Description("FECHA C.")> CA_FECHA_EXP_CONSTANCIA_PARTIDA = 141
            <EnumMember> <Description("NUMERO DE CUENTA.")> CA_NUM_CTA_GARANTIA_PARTIDA = 142
            <EnumMember> <Description("FOLIO CONSTANCIA.")> CA_FOLIO_CONSTANCIA_PARTIDA = 143
            <EnumMember> <Description("TOTAL DEPOSITO.")> CA_MONTO_TOTAL_CONSTANCIA_PARTIDA = 144
            <EnumMember> <Description("PRECIO ESTIMADO.")> CA_PRECIO_ESTIMADO_PARTIDA = 145
            <EnumMember> <Description("CANT. U.M. PRECIO EST.")> CA_CANT_UMT_PRECIO_ESTIMADO_PARTIDA = 146
            <EnumMember> <Description("VALOR MERCANCIAS NO ORIGINARIAS.")> CA_MONTO_MERC_NO_ORIGIN_PARTIDA = 147
            <EnumMember> <Description("MONTO IGI.")> CA_MONTO_IGI_PARTIDA = 148
            <EnumMember> <Description("OBSERVACIONES A NIVEL PARTIDA.")> CA_OBSERV_PARTIDA = 149
            <EnumMember> <Description("CVE. PEDIM. RECT.")> CA_CVE_PEDIM_RECTIF = 150
            <EnumMember> <Description("FECHA PAGO RECT.")> CA_FECHA_PAG_PEDIM_RECTIFICACION = 151
            <EnumMember> <Description("DIFERENCIA.")> CA_DIFERENCIA = 152
            <EnumMember> <Description("EFECTIVO.")> CA_DIFERENCIA_EFECTIVO = 153
            <EnumMember> <Description("OTROS.")> CA_DIFERENCIA_OTROS = 154
            <EnumMember> <Description("DIF. TOTALES.")> CA_DIFERENCIA_TOTAL = 155
            <EnumMember> <Description("PAIS DESTINO.")> CA_CVE_PAIS_EXPORT = 156
            <EnumMember> <Description("NUM. PEDIMENTO EUA/CAN.")> CA_NUM_DOCTO_IMP_EU_O_CAN = 157
            <EnumMember> <Description("PRUEBA SUFICIENTE.")> CA_CVE_PRUEBA = 158
            <EnumMember> <Description("TOTAL ARAN. EUA/CAN.")> CA_MONTO_IMP_PAGADO_EU_O_CAN = 159
            <EnumMember> <Description("MONTO EXENTO.")> CA_MONTO_IMP_CONFORME_CAMPO_4 = 160
            <EnumMember> <Description("FRACC. EUA/CAN.")> CA_FRACC_ARANC_BIEN_FINAL_EU_O_CAN = 161
            <EnumMember> <Description("TASA EUA/CAN.")> CA_TASA_IMP_PAGADO_EN_EU_O_CAN = 162
            <EnumMember> <Description("ARAN. EUA/CAN.")> CA_MONTO_IMP_PAGADO_EN_EU_O_CAN = 163
            <EnumMember> <Description("Aduana")> CA_ADUANA_SIN_SECCION = 164
            <EnumMember> <Description("Llenar con 0")> CA_0 = 165
            <EnumMember> <Description("IMPORTE DE DERECHO DE TRAMITE ADUANERO")> CA_IMPORTE_DTA = 166
            <EnumMember> <Description("PARA OPERACIONES AL AMPARO DE LA REGLA 3.1.40., DECLARAR: 3")> CA_REGLA_3_1_40 = 167
            <EnumMember> <Description("CLAVE DEL RECINTO FISCALIZADO, CONFORME AL APENDICE 6Ley Aduanera DE ESTE ANEXO")> CA_RECINTO_FISCALIZADO = 168
            <EnumMember> <Description("PARA OPERACIONES CONFORME AL TERCER PARRAFO DE LA REGLA 2.3.8.Ley Aduanera, DECLARAR EL NUMERO DEL CONTENEDOR QUE CONTIENE LAS MERCANCIAS")> CA_REGLA_2_3_8_CONTENEDOR = 169
            <EnumMember> <Description("PARA OPERACIONES CONFORME A LA REGLA 1.9.12.Ley Aduanera, DECLARAR EL NUMERO DE IDENTIFICACION DEL EQUIPO FERROVIARIO O NUMERO DE CONTENEDOR")> CA_REGLA_1_9_12_ID_EQ_FERROVIARIO_O_CONTENEDOR = 170
            <EnumMember> <Description("LA CANTIDAD DE MERCANCIA EN UNIDADES DE COMERCIALIZACION AMPARADA POR LA PARTE II")> CA_CANTIDAD_UMC_PARTE_II = 171
            <EnumMember> <Description("PARA OPERACIONES AL AMPARO DE LA REGLA 1.9.12.Ley Aduanera, SE DEBERA DECLARAR EL TOTAL DE GUIAS DE EMBARQUE (NIUS) AMPARADAS POR LA PARTE II")> CA_REGLA_1_9_12_TOTAL_GUIAS_PARTE_II = 172
            <EnumMember> <Description("PARA OPERACIONES AL AMPARO DE LA REGLA. 1.9.12.Ley Aduanera, SE DEBERA DECLARAR EL NUMERO DE IDENTIFICACION UNICO (NIU) DE LA GUIA DE EMBARQUE")> CA_REGLA_1_9_12_NIU_GUIA = 173
            <EnumMember> <Description("NUMERO CONSECUTIVO QUE EL AGENTE ADUANAL O LA AGENCIA ADUANAL ASIGNE A LA PARTE II")> CA_NUM_CONSECUTIVO_PARTE_II = 174
            <EnumMember> <Description("LA CANTIDAD DE MERCANCIA EN UNIDADES TIGIE AMPARADA POR LA COPIA SIMPLE")> CA_CANTIDAD_UMC_COPIA_SIMPLE = 175
            <EnumMember> <Description("PARA OPERACIONES AL AMPARO DE LA REGLA 1.9.12.Ley Aduanera, SE DEBERA DECLARAR EL TOTAL DE GUIAS DE EMBARQUE (NIUS) AMPARADAS POR LA COPIA SIMPLE")> CA_REGLA_1_9_12_TOTAL_GUIAS_COPIA_SIMPLE = 176
            <EnumMember> <Description("CONSECUTIVO QUE EL AGENTE ADUANAL O LA AGENCIA ADUANAL ASIGNE A LA COPIA SIMPLE.")> CA_NUM_CONSECUTIVO_COPIA_SIMPLE = 177
            <EnumMember> <Description("PARA OPERACIONES DE LA REGLA 3.1.21.Ley Aduanera, FRACCIÓN II, INCISO d), DECLARAR EL PESO BRUTO DE LA MERCANCÍA AMPARADA POR LA COPIA SIMPLE")> CA_REGLA_3_1_21_PESO_BRUTO_MERCA_COPIA_SIMPLE = 178
            <EnumMember> <Description("NUMERO DEL ACUSE DE VALOR EMITIDO POR VENTANILLA DIGITAL")> CA_NUM_ACUSE_VUCEM = 179
            <EnumMember> <Description("LA CANTIDAD DE MERCANCIA EN UNIDADES DE COMERCIALIZACION AMPARADA EN LA REMESA")> CA_CANT_UMC_REMESA = 180
            <EnumMember> <Description("PARA OPERACIONES AL AMPARO DE LA REGLA 1.9.12.Ley Aduanera, SE DEBERA DECLARAR EL TOTAL DE GUIAS DE EMBARQUE (NIUS) AMPARADAS POR LA REMESA")> CA_REGLA_1_9_12_TOTAL_GUIAS_REMESA = 181
            <EnumMember> <Description("NUMERO CONSECUTIVO QUE EL AGENTE ADUANAL O LA AGENCIA ADUANAL ASIGNE A LA REMESA DEL PEDIMENTO CONSOLIDADO")> CA_NUM_CONSECUTIVO_REMESA = 182
            <EnumMember> <Description("NUMERO DEL ACUSE DE VALOR DE LA RELACION DE CFDI O DOCUMENTOS EQUIVALENTES EMITIDO POR VENTANILLA DIGITAL")> CA_NUM_ACUSE_RELACION_CFDI_O_DOCTO_EQUIVALENTE = 183
            <EnumMember> <Description("VALOR EN DOLARES DEL TOTAL DE CDFI O DOCUMENTOS EQUIVALENTES AMPARADOS EN LA RELACION DE CDFI O DOCUMENTOS EQUIVALENTES")> CA_VALOR_DOLARES_TOTAL_CFDI_O_DOCTO_EQUIVALENTE = 184
            <EnumMember> <Description("PAGO ELECTRONICO")> CA_PAGO_ELECTRONICO = 185
            <EnumMember> <Description("NOMBRE DE LA INSTITUCIÓN BANCARIA")> CA_NOMBRE_INST_BANCARIA = 186
            <EnumMember> <Description("LINEA DE CAPTURA")> CA_LINEA_CAPTURA = 187
            <EnumMember> <Description("NÚMERO DE OPERACIÓN BANCARIA")> CA_NUM_OPERACION_BANCARIA = 188
            <EnumMember> <Description("NÚMERO DE TRANSACCIÓN SAT")> CA_NUM_TRANSACCION_SAT = 189
            <EnumMember> <Description("MEDIO DE PRESENTACIÓN")> CA_MEDIO_PRESENTACION = 190
            <EnumMember> <Description("MEDIO DE RECEPCIÓN/COBRO")> CA_MEDIO_RECEPCION_COBRO = 191
            <EnumMember> <Description("CALLE IMPORTADOR/EXPORTADOR")> CA_CALLE_IOE = 192
            <EnumMember> <Description("NUMERO INTERIOR")> CA_NUM_INTER_IOE = 193
            <EnumMember> <Description("NUMERO EXTERIOR")> CA_NUM_EXTERIOR_IOE = 194
            <EnumMember> <Description("CODIGO POSTAL")> CA_CODIGO_POSTAL_IOE = 195
            <EnumMember> <Description("MUNICIPIO/CIUDAD")> CA_MUNICIPIO_CIUDAD_IOE = 196
            <EnumMember> <Description("ENTIDAD FEDERATIVA")> CA_ENTIDAD_FEDERATIVA_IOE = 197
            <EnumMember> <Description("PAÍS DEL IMPORTADOR O EXPORTADOR")> CA_PAIS_IOE = 198
            <EnumMember> <Description("CALLE PROVEEDOR/COMPRADOR")> CA_CALLE_POC = 199
            <EnumMember> <Description("NÚMERO INTERIOR PROVEEDOR/COMPRADOR")> CA_NUMERO_INT_POC = 200
            <EnumMember> <Description("NUMERO EXTERIOR PROVEEDOR/COMPRADOR")> CA_NUMERO_EXTER_POC = 201
            <EnumMember> <Description("CODIGO POSTAL PROVEEDOR/COMPRADOR")> CA_CODIGO_POSTAL_POC = 202
            <EnumMember> <Description("MUNICIPIO/CIUDAD PROVEEDOR/COMPRADOR")> CA_MUNICIPIO_CIUDAD_POC = 203
            <EnumMember> <Description("ENTIDAD FEDERATIVA PROVEEDOR/COMPRADOR")> CA_ENTIDAD_FEDERATIVA_POC = 204
            <EnumMember> <Description("PAIS PROVEEDOR/COMPRADOR")> CA_PAIS_POC = 205
            <EnumMember> <Description("CALLE DESTINATARIO")> CA_CALLE_DESTINATARIO = 206
            <EnumMember> <Description("NÚMERO INTERIOR DESTINATARIO")> CA_NUMERO_INT_DESTINATARIO = 207
            <EnumMember> <Description("NÚMERO EXTERIOR DESTINATARIO")> CA_NUMERO_EXTER_DESTINATARIO = 208
            <EnumMember> <Description("CÓDIGO POSTAL DESTINATARIO")> CA_CODIGO_POSTAL_DESTINATARIO = 209
            <EnumMember> <Description("MUNICIPIO/CIUDAD DESTINATARIO")> CA_MUNICIPIO_CIUDAD_DESTINATARIO = 210
            <EnumMember> <Description("PAÍS DE DESTINATARIO")> CA_PAIS_DESTINATARIO = 211
            <EnumMember> <Description("CANTIDAD EN UNIDADES DE MEDIDA DE PRECIO ESTIMADO PEDIMENTO")> CA_CANTIDAD_UMT_PRECIO_ESTIMADO_PEDIMENTO = 212
            <EnumMember> <Description("TÍTULOS ASIGNADOS")> CA_TITULOS_ASIGNADOS_PEDIMENTO = 213
            <EnumMember> <Description("TOTAL DE BULTOS")> CA_TOTAL_BULTOS = 214
            <EnumMember> <Description("FRACCIÓN ORIGINAL")> CA_FRACCION_ORIGINAL = 215
            <EnumMember> <Description("UNIDAD DE MEDIDA ORIGINAL")> CA_UM_ORIGINAL = 216
            <EnumMember> <Description("CANTIDAD DE MERCANCÍA EN UMT DE DESCARGO")> CA_CANT_MERCANCIA_UMT_DESCARGO = 217
            <EnumMember> <Description("FECHA DE DOCUMENTO DE PRUEBA SUFICIENTE")> CA_FECHA_DOCUMENTO_PRUEBA_SUFICIENTE = 218
            <EnumMember> <Description("ENTIDAD FEDERATIVA DE ORIGEN")> CA_ENTIDAD_FEDER_ORIGEN = 219
            <EnumMember> <Description("ENTIDAD FEDERATIVA DE DESTINO")> CA_ENTIDAD_FEDER_DESTINO = 220
            <EnumMember> <Description("ENTIDAD FEDERATIVA DEL COMPRADOR")> CA_ENTIDAD_FEDER_COMPRADOR = 221
            <EnumMember> <Description("ENTIDAD FEDERATIVA DEL VENDEDOR")> CA_ENTIDAD_FEDER_VENDEDOR = 222
            <EnumMember> <Description("MONEDA")> CA_MONEDA_IGI_PARTIDA = 223
            <EnumMember> <Description("UNIDAD MEDIDA TARIFA EUA/CAN")> CA_UMT_EUA_CAN = 224
            <EnumMember> <Description("CANTIDAD DE MERCANCÍA EN UMT EUA/CAN")> CA_CANT_MERCA_UMT_EUA_CAN = 225
            <EnumMember> <Description("TIPO DE FIGURA")> CA_TIPO_FIGURA = 226
            <EnumMember> <Description("NOMBRE DEL ARCHIVO")> CA_NOMBRE_ARCHIVO = 227
            <EnumMember> <Description("CANTIDAD DE PEDIMENTOS")> CA_CANTIDAD_PEDIMENTOS = 228
            <EnumMember> <Description("CANTIDAD DE REGISTROS")> CA_CANTIDAD_REGISTROS = 229
            <EnumMember> <Description("CLAVE DEL PREVALIDADOR")> CA_CVE_PREVAL = 230
            <EnumMember> <Description("NUMERO DE PEDIMENTO 7 DIGITOS")> CA_NUM_PEDIMENTO = 231
            <EnumMember> <Description("AÑO VALIDACIÓN A 2 DÍGITOS")> CA_AÑO_VALIDACION_2 = 232
            <EnumMember> <Description("VALOR UNITARIO DEL TITULO")> CA_VALOR_UNITARIO_TITULO_PEDIMENTO = 233
            <EnumMember> <Description("PATENTE ORIGINAL")> CA_PATENTE_ORIGINAL = 234
            <EnumMember> <Description("IMPORTE DEL GRAVAMEN")> CA_IMPORTE_GRAVAMEN_COMPENSACION = 235
            <EnumMember> <Description("CLAVE T. TASA. PARTIDA")> CA_CVE_T_TASA_PARTIDA = 236
            <EnumMember> <Description("TASA PARTIDA")> CA_TASA_PARTIDA = 237
            <EnumMember> <Description("FORMA DE PAGO PARTIDA")> CA_FP_PARTIDA = 238
            <EnumMember> <Description("IMPORTE PARTIDA")> CA_IMPORTE_PARTIDA = 239
            <EnumMember> <Description("NUMERO TOTAL DE PARTIDAS")> CA_NUMERO_TOTAL_PARTIDAS = 240
            <EnumMember> <Description("CLAVE TIPO DE OPERACIÓN")> CA_CVE_TIPO_OPERACION = 241
            <EnumMember> <Description("AÑO VALIDACION")> CA_AÑO_VALIDACION = 242
            <EnumMember> <Description("FECHA DE VALIDACION")> CA_FECHA_VALIDACION = 243
            <EnumMember> <Description("CLAVE CONCEPTO PARA COMPENSACION")> CA_CVE_CONCEPTO_COMPENSACION = 244
            <EnumMember> <Description("CLAVE CONCEPTO NIVEL PEDIMENTO")> CA_CVE_CONCEPTO_NIVEL_PEDIMENTO = 245
            <EnumMember> <Description("CLAVE CONTRIBUCION A NIVEL PARTIDA")> CA_CVE_CONTRIBUCION_NIVEL_PARTIDA = 246
            <EnumMember> <Description("NUMERO DE ACUSE DE VALOR")> CA_NUMERO_ACUSE_DE_VALOR = 247
            <EnumMember> <Description("CLAVE INSTITUCION EMISORA CUENTA ADUANERA PEDIMENTO")> CA_CVE_INSTITUCION_EMISORA_CTA_ADUANERA_PEDIMENTO = 248
            <EnumMember> <Description("CLAVE VINCULACION")> CA_CVE_VINCULACION = 249
            <EnumMember> <Description("CLAVE INSTITUCION EMISORA CUENTA ADUANERA PARTIDA")> CA_CVE_INSTITUCION_EMISORA_CTA_ADUANERA_PARTIDA = 250
            <EnumMember> <Description("ADUANA DE DESPACHO")> CA_ADUANA_DESPACHO = 251
            <EnumMember> <Description("NUMERO DE PEDIMENTO ORIGINAL 7 DIGITOS")> CA_NUM_PEDIMENTO_ORIGINAL_7_DIGITOS = 252
            <EnumMember> <Description("AÑO VALIDACION 2 DIGITOS ORIGINAL")> CA_AÑO_VALIDACION_2_ORIGINAL = 253
            <EnumMember> <Description("AÑO VALIDACION ORIGINAL")> CA_AÑO_VALIDACION_ORIGINAL = 254
            <EnumMember> <Description("ADUANA DESPACHO ORIGINAL")> CA_ADUANA_DESPACHO_ORIGINAL = 255
            <EnumMember> <Description("ADUANA DESPACHO ORIGINAL 2 DIGITOS")> CA_ADUANA_DESPACHO_ORIGINAL_2 = 256
            <EnumMember> <Description("CONCEPTO DIFERENCIA DE CONTRIBUCIONES")> CA_CONCEPTO_DIF_CONTRIB = 257
            <EnumMember> <Description("CLAVE CONCEPTO DIFERENCIA DE CONTRIBUCIONES")> CA_CVE_CONCEPTO_DIF_CONTRIB = 258
            <EnumMember> <Description("FORMA DE PAGO DIFERENCIAS")> CA_FORMA_PAGO_DIFERENCIAS = 259
            <EnumMember> <Description("NOMBRE PREVALIDADOR")> CA_VALIDADOR_DESIGNADO = 260
            <EnumMember> <Description("Número de la semana")> CA_NUMERO_SEMANA = 261
            <EnumMember> <Description("ARCHIVO VALIDACIÓN")> CA_ARCHIVO_VALIDACION = 262
            <EnumMember> <Description("ACUSE ELECTRÓNICO DE PAGO")> CA_ACUSE_ELECTONICO_DE_PAGO = 263
            <EnumMember> <Description("ARCHIVO DE PAGO")> CA_ARCHIVO_PAGO = 264
            <EnumMember> <Description("FECHA REGISTRO")> CA_FECHA_REGISTRO = 265

            <EnumMember> <Description("FECHA REVALIDACIÓN")> CA_FECHA_REVALIDACION = 266
            <EnumMember> <Description("FECHA ZARPE")> CA_FECHA_ZARPE = 267
            <EnumMember> <Description("FECHA PREVIO")> CA_FECHA_PREVIO = 268
            <EnumMember> <Description("FECHA FONDEO")> CA_FECHA_FONDEO = 269
            <EnumMember> <Description("FECHA ATRAQUE")> CA_FECHA_ATRAQUE = 270
            <EnumMember> <Description("FECHA DESPACHO")> CA_FECHA_DESPACHO = 271
            <EnumMember> <Description("FECHA ARRIBO")> CA_FECHA_ARRIBO = 272
            <EnumMember> <Description("FECHA ENTREGA")> CA_FECHA_ENTREGA = 273
            <EnumMember> <Description("FECHA FACTURACIÓN")> CA_FECHA_FACTURACION = 274

            <EnumMember> <Description("DESCRIPCIÓN CONCEPTO")> CA_DESCRIPCION_CONCEPTO = 275
            <EnumMember> <Description("COVE")> CA_COVE = 276

            <EnumMember> <Description("PAGOS VIRTUALES - FORMA DE PAGO")> CA_PV_FP = 277
            <EnumMember> <Description("RECTIFICACIONES - FECHA DE OPERACION ORIGINAL.")> CA_RECTIFICACION_FECHA_PEDIM_ORIGINAL = 278
            <EnumMember> <Description("RECTIFICACIONES - CVE. PEDIMENTO ORIGINAL.")> CA_RECTIFICACION_CVE_PEDIM_ORIGINAL = 279
            <EnumMember> <Description("RECTIFICACIONES - PATENTE ORIGINAL")> CA_RECTIFICACION_PATENTE_ORIGINAL = 280
            <EnumMember> <Description("RECTIFICACIONES - NUMERO DE PEDIMENTO ORIGINAL 7 DIGITOS")> CA_RECTIFICACION_NUM_PEDIMENTO_ORIGINAL_7_DIGITOS = 281
            <EnumMember> <Description("RECTIFICACIONES - AÑO VALIDACION 2 DIGITOS ORIGINAL")> CA_RECTIFICACION_AÑO_VALIDACION_2_ORIGINAL = 282
            <EnumMember> <Description("RECTIFICACIONES - AÑO VALIDACION ORIGINAL")> CA_RECTIFICACION_AÑO_VALIDACION_ORIGINAL = 283
            <EnumMember> <Description("RECTIFICACIONES - ADUANA DESPACHO ORIGINAL")> CA_RECTIFICACION_ADUANA_DESPACHO_ORIGINAL = 284
            <EnumMember> <Description("RECTIFICACIONES - ADUANA DESPACHO ORIGINAL 2 DIGITOS")> CA_RECTIFICACION_ADUANA_DESPACHO_ORIGINAL_2 = 285
            <EnumMember> <Description("COMPENSACIONES - CONTRIB")> CA_COMPENSACION_CONTRIBUCION = 286
            <EnumMember> <Description("COMPENSACIONES - NUM. PEDIMENTO ORIGINAL COMPLETO")> CA_COMPENSACION_NUM_PEDIM_ORIGINAL_COMPLETO = 287
            <EnumMember> <Description("COMPENSACIONES - AÑO VALIDACION ORIGINAL")> CA_COMPENSACION_AÑO_VALIDACION_ORIGINAL = 288
            <EnumMember> <Description("COMPENSACIONES - AÑO VALIDACION 2 DIGITOS ORIGINAL")> CA_COMPENSACION_AÑO_VALIDACION_2_ORIGINAL = 289
            <EnumMember> <Description("COMPENSACIONES - PATENTE ORIGINAL")> CA_COMPENSACION_PATENTE_ORIGINAL = 290
            <EnumMember> <Description("COMPENSACIONES - ADUANA DESPACHO ORIGINAL")> CA_COMPENSACION_ADUANA_DESPACHO_ORIGINAL = 291
            <EnumMember> <Description("COMPENSACIONES - ADUANA DESPACHO ORIGINAL 2 DIGITOS")> CA_COMPENSACION_ADUANA_DESPACHO_ORIGINAL_2 = 292
            <EnumMember> <Description("COMPENSACIONES - NUMERO DE PEDIMENTO ORIGINAL 7 DIGITOS")> CA_COMPENSACION_NUM_PEDIMENTO_ORIGINAL_7_DIGITOS = 293
            <EnumMember> <Description("PRUEBA SUFICIENTE - PAIS DESTINO.")> CA_PRUEBASUFICIENTE_CVE_PAIS_EXPORT = 294
            <EnumMember> <Description("PARTIDAS - VINCULACION.")> CA_PARTIDAS_VINCULACION = 295
            '#############################  CAMPOS ÚNICOS DE LA AUTORIDAD ##################################




            '#############################  CAMPOS ÚNICOS PROPIOS ##################################
            <EnumMember> <Description("REFERENCIA")> CP_REFERENCIA = 1000
            <EnumMember> <Description("MODALIDAD/ADUANA/PATENTE")> CP_MODALIDAD_ADUANA_PATENTE = 1001
            <EnumMember> <Description("MODALIDAD")> CP_MODALIDAD = 1002
            <EnumMember> <Description("EJECUTIVO DE CUENTA")> CP_EJECUTIVO_DE_CUENTA = 1003
            <EnumMember> <Description("NÚMERO DE CLIENTE")> CP_NUMERO_CLIENTE = 1004
            '#############################  CAMPOS ÚNICOS PROPIOS ##################################3


        End Enum

        '<DataContract()>
        Public Enum CamposVOCE

            '#############################  CAMPOS ÚNICOS DEL VOCE ##################################
            'Abreviaciones genenerales
            'IOE = IMPORTADOR O EXPORTADOR
            'PRO = PROVEEDOR
            'AAD = AGENTE ADUANAL
            'SAD = SECCIÓN ADUANA DE DESPACHO
            'SAE = SECCIÓN ADUANA DE ENTRADA
            'SAS = SECCIÓN ADUANA DE SALIDA
            'SES = SECCIÓN ADUANA DE ENTRADA O SALIDA
            '******************************************
            '500 = INICIO DEL PEDIMENTO
            <EnumMember> <Description("Sin definir")> SinDefinir = 0
            <EnumMember> <Description("Clave del tipo de registro.")> ClaveDelTipoDeRegistro = 1
            <EnumMember> <Description("Tipo de Movimiento.")> TipoDeMovimientoS0VC002 = 2
            <EnumMember> <Description("Patente o autorización")> PatenteOAutorizacion = 3
            <EnumMember> <Description("Número de pedimento.")> NumeroDePedimento = 4
            <EnumMember> <Description("Aduana-sección de despacho.")> AduanaSeccionDeDespacho = 5
            <EnumMember> <Description("Acuse electrónico de validación")> AcuseElectronicoDeValidacion = 6
            '501 = DATOS GENERALES
            <EnumMember> <Description("Tipo de operación.")> TipoDeOperacion = 7
            <EnumMember> <Description("Clave de pedimento.")> ClaveDePedimento = 8
            <EnumMember> <Description("Aduana-sección de entrada o salida.")> AduanaSeccionDeEntradaOSalida = 9
            <EnumMember> <Description("CURP del importador o exportador")> CURPDelIOE = 10
            <EnumMember> <Description("RFC del importador o exportador")> RFCDelIOE = 11
            <EnumMember> <Description("CURP del agente aduanal, representante legal, apodarado o mandatario.")> CURPAgenteAduanalUOtro = 12
            <EnumMember> <Description("Tipo de cambio")> TipoDeCambio = 13
            <EnumMember> <Description("Importe del pago de fletes")> ImporteDelPagoDeFletes = 14
            <EnumMember> <Description("Importe del pago de primas de seguros")> ImporteDelPagoDePrimasDeSeguros = 15
            <EnumMember> <Description("Importe del pago de embalajes")> ImporteDelPagoDeEmbalajes = 16
            <EnumMember> <Description("Importe del pago de otros incrementables")> ImporteDelPagoDeOtrosIncrementables = 17

            <EnumMember> <Description("Uso futuro")> UsoFuturo = 18
            <EnumMember> <Description("Peso bruto total de la mercancía")> PesoBrutoTotalDeLaMercancia = 19
            <EnumMember> <Description("Medio de transporte de salida de la aduana-sección de salida")> MedioDeTransporteDeSalidaDeLaSAS = 20
            <EnumMember> <Description("Medio de transporte de arribo a la aduana-sección de arribo")> MedioDeTransporteDeArriboALaAduanaSeccionDeArribo = 21
            <EnumMember> <Description("Medio de transporte utilizado a la entrada o salida de la mercancía a territorio nacional")> MedioDeTransporteUtilizadoALaEntradaOSalidaDeLaMercanciaATerritorioNacional = 22
            <EnumMember> <Description("Origen o destino de la mercancía")> OrigenODestinoDeLaMercancia = 23

            <EnumMember> <Description("Nombre del importador o exportador")> NombreDelIOE = 24
            <EnumMember> <Description("Calle del domicilio del importador o exportador")> CalleDelDomicilioDelIOE = 25
            <EnumMember> <Description("Número interior del domicilio del importador o exportador")> NumeroInteriorDelDomicilioDelIOE = 26
            <EnumMember> <Description("Número exterior del domicilio del importador o exportador")> NumeroExteriorDelDomicilioDelIOE = 27
            <EnumMember> <Description("Código postal del domicilio fiscal del importador o exportador")> CodigoPostalDelDomicilioFiscalDelIOE = 28
            <EnumMember> <Description("Municipio del domicilio fiscal del importador o exportador")> MunicipioDelDomicilioFiscalDelIOE = 29
            <EnumMember> <Description("Entidad federativa del domicilio del importador o exportador")> EntidadFederativaDelIOE = 30
            <EnumMember> <Description("País del domicilio fiscal del importador o exportador")> PaisDelDomicilioFiscalDelIOE = 31
            <EnumMember> <Description("RFC de quien emite el CFDi o documento equivalente de los servicios de operación")> RFCDelEmisorCFDI = 32

            <EnumMember> <Description("Decrementables por fletes")> DecrementablesPorFletes = 33
            <EnumMember> <Description("Decrementables por seguros")> DecrementablesPorSeguros = 34
            <EnumMember> <Description("Decrementables por carga")> DecrementablesPorCarga = 35
            <EnumMember> <Description("Decrementables por descarga")> DecrementablesPorDescarga = 36
            <EnumMember> <Description("Otros decrementables")> OtrosDecrementables = 37
            '502 = TRANSPORTE
            '<EnumMember> <Description("Clave del tipo de registro")> ClaveDelTipoDeRegistro = 1
            '<EnumMember> <Description("Número de pedimento.")> NumeroDePedimento = 4
            <EnumMember> <Description("RFC del transportista")> RFCDelTransportista = 38
            <EnumMember> <Description("CURP del transportista")> CURPDelTransportista = 39
            <EnumMember> <Description("Nombre del transportista")> NombreDelTransportista = 40
            <EnumMember> <Description("País del medio de transporte")> PaisDelMedioDeTransporte = 41
            <EnumMember> <Description("Identificador del transporte")> IdentificadorDelTransporte = 42
            <EnumMember> <Description("Total del bultos")> TotalDeBultos = 43
            <EnumMember> <Description("Domicilio fiscal del transportista")> DomicilioFiscalDelTransportista = 44
            '503 = GUÍAS ( para Array )
            '<EnumMember> <Description("Clave del tipo de registro")> ClaveDelTipoDeRegistro = 1
            '<EnumMember> <Description("Número de pedimento.")> NumeroDePedimento = 4
            <EnumMember> <Description("Número de guia, manifiesto o conocimiento de embarque")> NumeroDeGuia = 45
            <EnumMember> <Description("Identificador de la guía")> IdentificadorDeLaGuia = 46
            '504 = CONTENEDORES ( para Array )
            '<EnumMember> <Description("Clave del tipo de registro")> ClaveDelTipoDeRegistro = 1
            '<EnumMember> <Description("Número de pedimento.")> NumeroDePedimento = 4
            <EnumMember> <Description("Número de contenedor")> NumeroDeContenedor = 47
            <EnumMember> <Description("Tipo de contenedor")> TipoDeContenedor = 48

        End Enum

        'Campos referencia
        '        <JsonConverter(TypeOf (StringEnumConverter))>
        '<BsonRepresentation(BsonType.String)>
        '<BsonRepresentation(BsonType.String)>
        Public Enum CamposReferencia
            'Región del 2000 - 2999 esta deberia ser su numeracion

            '#############################  CAMPOS ÚNICOS DE LA REFERENCIA ##################################
            'Abreviaciones genenerales
            'IOE = IMPORTADOR O EXPORTADOR
            'PRO = PROVEEDOR
            'AAD = AGENTE ADUANAL
            'SAD = SECCIÓN ADUANA DE DESPACHO
            'SAE = SECCIÓN ADUANA DE ENTRADA
            'SAS = SECCIÓN ADUANA DE SALIDA
            'SES = SECCIÓN ADUANA DE ENTRADA O SALIDA
            '******************************************
            'GENERALES
            <EnumMember> <Description("SIN DEFINIR")> SIN_DEFINIR = 1100

            '#############################  CAMPOS ÚNICOS DE LA AUTORIDAD ##################################
            <EnumMember> <Description("REFERENCIA")> CP_REFERENCIA = 1101
            <EnumMember> <Description("NUMERO DE PEDIMENTO")> CP_PEDIMENTO = 1102
            <EnumMember> <Description("ORIGINAL")> CP_ORIGINAL = 1103
            <EnumMember> <Description("TIPO DE OPERACION")> CP_TIPO_OPERACION = 1104
            <EnumMember> <Description("TIPO DE REFERENCIA")> CP_TIPO_REFERENCIA = 1105
            <EnumMember> <Description("MATERIAL PELIGROSO")> CP_MATERIAL_PELIROSO = 1106
            <EnumMember> <Description("RECTIFICACION")> CP_RECTIFICACION = 1107
            <EnumMember> <Description("PREFIJO")> CP_PREFIJO = 1108
            <EnumMember> <Description("TIPO DE CARGA/LOTE")> CP_TIPO_CARGA = 1109
            <EnumMember> <Description("MODALIDAD/ADUANA/PATENTE")> CP_MODALIDAD_ADUANA_PATENTE2 = 1110
            <EnumMember> <Description("REGIMEN")> CP_REGIMEN = 1111
            <EnumMember> <Description("TIPO DE DOCUMENTO")> CA_TIPO_DOCUMENTO = 1112
            <EnumMember> <Description("CLAVE DEL DOCUMENTO")> CP_CLAVE_DOCUMENTO = 1113
            <EnumMember> <Description("ADUANA DE ENTRADA/SALIDA")> CP_ADUANA_ENTRADA_SALIDA = 1114
            <EnumMember> <Description("ADUANA DE DESPACHO")> CP_ADUANA_DESPACHO = 1115
            <EnumMember> <Description("DESTINO MERCANCIA")> CP_DESTINO_MERCANCIA = 1116
            <EnumMember> <Description("EJECUTIVO DE CUENTA")> CP_EJECUTIVO_CUENTA = 1117
            <EnumMember> <Description("CLAVE DEL IMPORTADOR/EXPORTADOR")> CP_ID_IOE = 1118
            <EnumMember> <Description("NOMBRE, DENOMINACIÓN SOCIAL DEL IMPORTADOR/EXPORTADOR")> CA_RAZON_SOCIAL_IOE = 1119
            <EnumMember> <Description("RFC DEL IMPORTADOR/EXPORTADOR")> CA_RFC_DEL_IOE = 1120
            <EnumMember> <Description("CURP DEL IMPORTADOR/EXPORTADOR")> CA_CURP_DEL_IOE = 1121
            <EnumMember> <Description("RFC FACTURACION.")> CP_RFC_FACTURACION_IOE = 1122
            <EnumMember> <Description("BANCO DE PAGO DEL IMPORTADOR/EXPORTADOR")> CA_BANCO_PAGO_IOE = 1123
            <EnumMember> <Description("DESCRIPCION_DETALLE")> CP_DESCRIPCION_DETALLE = 1124
            <EnumMember> <Description("TIPO_DETALLE")> CP_TIPO_DETALLE = 1125
            <EnumMember> <Description("FECHA ETA")> CP_FECHA_ETA = 1126
            <EnumMember> <Description("ES ENTRADA")> CP_ES_ENTRADA = 1127
            <EnumMember> <Description("ES PAGO ANTICIPADO")> CP_ES_PAGO_ANTICIPADO = 1128
            <EnumMember> <Description("FECHA RECEPCIÓN DE DOCUMENTOS")> CP_FECHA_RECEPCION_DOCUMENTOS = 1129

            'MODALIDAD
            '<EnumMember><Description("REFERENCIA")> CP_REFERENCIA = 1
            '<EnumMember><Description("NUM. PEDIMENTO. COMPLETO")> CA_NUM_PEDIMENTO_COMPLETO = 2
            '<EnumMember><Description("TIPO DOCUMENTO")> CA_TIPO_DOCUMENTO = 3
            '<EnumMember><Description("T.OPER.")> CA_T_OPER = 4
            '<EnumMember><Description("MODALIDAD")> CA_MODALIDAD = 5 'Q
            '<EnumMember><Description("DESTINO/ORIGEN")> CA_DESTINO_ORIGEN = 6 'P
            '<EnumMember><Description("ADUANA E/S")> CA_ADUANA_E_S = 7
            '<EnumMember><Description("MATERIAL PELIGROSO")> CA_MATERIAL_PELIGROSO = 8
            '<EnumMember><Description("RECTIFICACION")> CA_RECTIFICACION = 9
            '<EnumMember><Description("ORIGEN")> CA_ORIGEN = 10 'Que es?
            '<EnumMember><Description("TIPO REFERENCIA")> CA_TIPO_REFERENCIA = 11
            '<EnumMember><Description("CVE. PEDIMENTO.")> CA_CVE_PEDIMENTO = 12
            '<EnumMember><Description("REGIMEN")> CA_REGIMEN = 13
            '<EnumMember><Description("EJECUTIVO DE CUENTA")> CP_EJECUTIVO_DE_CUENTA = 14
            '<EnumMember><Description("DOMICILIO DEL IMPORTADOR / EXPORTADOR")> CA_DOMICILIO_IOE = 18 'F
            '<EnumMember><Description("CLAVE DE LA SECCIÓN ADUANERA DE DESPACHO")> CA_CLAVE_SAD = 19 'Que es?
            '<EnumMember><Description("TIPO DE CARGA/LOTE")> CA_TIPO_CARGA = 20

            '<EnumMember><Description("NÚMERO DE CLIENTE")> CP_NUMERO_CLIENTE = 27
            '<EnumMember><Description("NOMBRE, DENOMINACIÓN O RAZ. SOC.")> CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA = 28
            '<EnumMember><Description("CURP.")> CA_CURP_AA_O_REP_LEGAL = 30
            '<EnumMember><Description("RFC IMPORTADOR/EXPORTADOR.")> CA_RFC_IOE = 31
            '<EnumMember><Description("MODALIDAD/ADUANA/PATENTE")> CP_MODALIDAD_ADUANA_PATENTE = 32
            '<EnumMember><Description("MODALIDAD")> CP_MODALIDAD = 33

            '<EnumMember><Description("FECHA ENTRADA")> CA_FECHA_ENTRADA = 21
            '<EnumMember><Description("FECHA PAGO")> CA_FECHA_PAGO = 22
            '<EnumMember><Description("FECHA EXTRACCIÓN")> CA_FECHA_EXTRACCION = 23
            '<EnumMember><Description("FECHA PRESENTACION")> CA_FECHA_PRESENTACION = 24
            '<EnumMember><Description("FECHA IMP.EUA/CAN")> CA_FECHA_IMP_EUA_CAN = 25
            '<EnumMember><Description("FECHA ORIGINAL")> CA_FECHA_ORIGINAL = 26

            '<EnumMember><Description("ATA")> CP_ATA = 35
            '<EnumMember><Description("REVALIDACION")> CP_REVALIDACION = 36
            '<EnumMember><Description("PAGO PEDIMENTO")> CP_PAGO_PEDIMENTO = 37
            '<EnumMember><Description("DESPACHO")> CP_DESPACHO = 38
            '<EnumMember><Description("REGISTRO")> CP_REGISTRO = 39
            '<EnumMember><Description("RECEPCION DE DOCS")> CP_RECEPCION_DOCS = 40
            '<EnumMember><Description("PREVIO")> CP_PREVIO = 41

        End Enum

        Public Enum CamposFacturaComercial
            'Región del 3000 - 3999
            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 3000

            <EnumMember> <Description("Número de factura/Folio fiscal")> CA_NUMERO_FACTURA = 3001
            <EnumMember> <Description("Fecha de factura")> CA_FECHA_FACTURA = 3002
            <EnumMember> <Description("Orden de compra")> CP_ORDEN_COMPRA = 3003
            <EnumMember> <Description("Clave país de facturación")> CA_CVE_PAIS_FACTURACION = 3004
            <EnumMember> <Description("País de facturación")> CA_PAIS_FACTURACION = 3005
            <EnumMember> <Description("Tipo de operación")> CP_TIPO_OPERACION = 3006
            <EnumMember> <Description("Clave Incoterm")> CA_CVE_INCOTERM = 3007
            <EnumMember> <Description("Valor factura")> CP_VALOR_FACTURA = 3008
            <EnumMember> <Description("Moneda factura")> CA_MONEDA_FACTURACION = 3009
            <EnumMember> <Description("Valor mercancía")> CP_VALOR_MERCANCIA = 3010
            <EnumMember> <Description("Moneda valor mercancia")> CP_MONEDA_VALOR_MERCANCIA = 3011
            <EnumMember> <Description("Peso total (kg)")> CP_PESO_TOTAL = 3012
            <EnumMember> <Description("Enajenación")> CP_APLICA_ENAJENACION = 3013
            <EnumMember> <Description("Subdivisión")> CA_APLICA_SUBDIVISION = 3014
            <EnumMember> <Description("Serie/folio de la factura")> CP_SERIE_FOLIO_FACTURA = 3015
            <EnumMember> <Description("Clave vinculación")> CA_CVE_VINCULACION = 3016
            <EnumMember> <Description("Método de valoración")> CP_CVE_METODO_VALORACION = 3017
            <EnumMember> <Description("Funge como certificado")> CA_APLICA_CERTIFICADO = 3018
            <EnumMember> <Description("Nombre del certificador")> CP_NOMBRE_CERTIFICADOR = 3019

            'Partidas de Factura
            <EnumMember> <Description("Número de partida")> CP_NUMERO_PARTIDA = 3020
            <EnumMember> <Description("Número de parte")> CA_NUMERO_PARTE_PARTIDA = 3021
            <EnumMember> <Description("Valor factura")> CA_VALOR_FACTURA_PARTIDA = 3022
            <EnumMember> <Description("Moneda factura")> CP_MONEDA_FACTURA_PARTIDA = 3023
            <EnumMember> <Description("Valor mercancía")> CA_VALOR_MERCANCIA_PARTIDA = 3024
            <EnumMember> <Description("Moneda mercancía")> CA_MONEDA_MERCANCIA_PARTIDA = 3025
            <EnumMember> <Description("Clave método de valoración")> CA_CVE_METODO_VALORACION_PARTIDA = 3026
            <EnumMember> <Description("Peso neto (Kg)")> CA_PESO_NETO_PARTIDA = 3027
            <EnumMember> <Description("Precio unitario")> CA_PRECIO_UNITARIO_PARTIDA = 3028
            <EnumMember> <Description("País de origen")> CA_PAIS_ORIGEN_PARTIDA = 3029
            <EnumMember> <Description("Cantidad factura")> CP_CANTIDAD_FACTURA_PARTIDA = 3030
            <EnumMember> <Description("Unidad de medida factura")> CP_UNIDAD_MEDIDA_FACTURA_PARTIDA = 3031
            <EnumMember> <Description("Descripción")> CA_DESCRIPCION_PARTE_PARTIDA = 3032
            <EnumMember> <Description("Aplica para COVE")> CP_APLICA_DESCRIPCION_COVE_PARTIDA = 3033
            <EnumMember> <Description("Cantidad comercial")> CA_CANTIDAD_COMERCIAL_PARTIDA = 3034
            <EnumMember> <Description("Unidad de medida comercial")> CA_UNIDAD_MEDIDA_COMERCIAL_PARTIDA = 3035
            <EnumMember> <Description("Descripción COVE")> CA_DESCRIPCION_COVE_PARTIDA = 3036
            <EnumMember> <Description("Fracción arancelaria")> CA_FRACCION_ARANCELARIA_PARTIDA = 3037
            <EnumMember> <Description("Cantidad tarifa")> CA_CANTIDAD_TARIFA_PARTIDA = 3038
            <EnumMember> <Description("Unidad de medida tarifa")> CA_UNIDAD_MEDIDA_TARIFA_PARTIDA = 3039
            <EnumMember> <Description("Nico")> CA_FRACCION_NICO_PARTIDA = 3040
            <EnumMember> <Description("Lote")> CA_LOTE_PARTIDA = 3041
            <EnumMember> <Description("Número de serie")> CA_NUMERO_SERIE_PARTIDA = 3042
            <EnumMember> <Description("Marca")> CA_MARCA_PARTIDA = 3043
            <EnumMember> <Description("Modelo")> CA_MODELO_PARTIDA = 3044
            <EnumMember> <Description("Submodelo")> CA_SUBMODELO_PARTIDA = 3045
            <EnumMember> <Description("Kilometraje")> CA_KILOMETRAJE_PARTIDA = 3046
            <EnumMember> <Description("Fletes")> CA_FLETES = 3047
            <EnumMember> <Description("Moneda fletes")> CA_MONEDA_FLETES = 3048
            <EnumMember> <Description("Seguros")> CA_SEGURO = 3049
            <EnumMember> <Description("Moneda seguros")> CA_MONEDA_SEGUROS = 3050
            <EnumMember> <Description("Embalajes")> CA_EMBALAJES = 3051
            <EnumMember> <Description("Moneda embalajes")> CA_MONEDA_EMBALAJES = 3052
            <EnumMember> <Description("Otros incrementables")> CA_OTROS_INCREMENTABLES = 3053
            <EnumMember> <Description("Moneda otros")> CA_MONEDA_OTROS_INCREMENTABLES = 3054
            <EnumMember> <Description("Descuentos")> CA_DESCUENTOS = 3055
            <EnumMember> <Description("Moneda descuentos")> CA_MONEDA_DESCUENTOS = 3056
            <EnumMember> <Description("Orden de compra partida")> CP_ORDEN_COMPRA_PARTIDA = 3057
            <EnumMember> <Description("Moneda precio unitario")> CP_MONEDA_PRECIO_UNITARIO = 3058

        End Enum

        Public Enum CamposAcuseValor
            'Región del 4000 - 4999
            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 4000
            <EnumMember> <Description("ObjectID FacturaComercial")> CP_ID_FACTURA_ACUSEVALOR = 4001
            <EnumMember> <Description("Número COVE SYSTEM")> CP_NUMERO_SYSTEM_ACUSEVALOR = 4002
            <EnumMember> <Description("FOLIO COVE")> CA_NUMERO_ACUSEVALOR = 4003
            <EnumMember> <Description("Tipo de Documento")> CP_TIPO_DOCUMENTO_ACUSEVALOR = 4004
            <EnumMember> <Description("Fecha COVE")> CA_FECHA_ACUSEVALOR = 4005
            <EnumMember> <Description("Relación de Facturas")> CA_RELACION_FACTURA_ACUSEVALOR = 4006
            <EnumMember> <Description("Número del exportador autorizado")> CA_NUMERO_EXPORTADOR_ACUSEVALOR = 4007
            <EnumMember> <Description("Observaciones")> CA_OBSERVACIONES_ACUSEVALOR = 4008
            <EnumMember> <Description("ObjectID Proveedor")> CP_ID_Proveedor_ACUSEVALOR = 4009
            <EnumMember> <Description("ObjectID Destinatario")> CP_ID_Destinatario_ACUSEVALOR = 4010

            'Partidas de ACUSEVALOR
            <EnumMember> <Description("Descripción COVE")> CA_DESCRIPCION_PARTIDA_ACUSEVALOR = 4011
            <EnumMember> <Description("Unidad de medida COVE")> CA_UNIDAD_MEDIDA_FACTURA_PARTIDA_ACUSEVALOR = 4012
            <EnumMember> <Description("Valor mercancía dólares")> CA_VALOR_MERCANCIA_PARTIDA_DOLARES_ACUSEVALOR = 4013

            'Detalles Partida ACUSEVALOR
            'TODOS ESTOS DATOS ESTÄN EN LA FACTURA SON NUMERO DE SERIE, MARCA, MODELO Y SUBMODELO

            'Configuración ACUSEVALOR
            <EnumMember> <Description("Sello del Importador")> CA_SELLO_ACUSEVALOR = 4014
            <EnumMember> <Description("Patente del Agente Aduanal")> CA_PATENTE_ACUSEVALOR = 4015
            <EnumMember> <Description("RFC'S de Consulta")> CA_RFC_CONSULTA_ACUSEVALOR = 4016
            <EnumMember> <Description("E-mail de Consulta")> CP_EMAIL_CONSULTA_ACUSEVALOR = 4017

        End Enum

        Public Enum CamposProveedorOperativo
            'Región del 5000 - 5999
            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 5000

            <EnumMember> <Description("Identificador de proveedor operativo")> CP_CVE_PROVEEDOR = 5001
            <EnumMember> <Description("Identificador de empresa")> CP_CVE_EMPRESA = 5002
            <EnumMember> <Description("Nombre o razón social")> CA_RAZON_SOCIAL_PROVEEDOR = 5003
            <EnumMember> <Description("Tipo de uso")> CP_TIPO_USO = 5004

            <EnumMember> <Description("Lista detalle proveedor operativo")> CP_DETALLE_PROVEEDOR = 5005
            <EnumMember> <Description("Identificador detalle proveedor operativo")> CP_SECUENCIA_PROVEEDOR = 5006
            <EnumMember> <Description("Tax id")> CA_TAX_ID_PROVEEDOR = 5007
            <EnumMember> <Description("RFC")> CA_RFC_PROVEEDOR = 5008
            <EnumMember> <Description("Curp (OPCIONAL)")> CA_CURP_PROVEEDOR = 5009

            <EnumMember> <Description("Lista de domicilios fiscales")> CP_DOMICILIOS_FISCALES = 5010
            <EnumMember> <Description("Identificador proveedor operativo del domicilio")> CP_SECUENCIA_PROVEEDOR_DOMICILIO = 5011
            <EnumMember> <Description("Tax id del domicilio")> CP_TAX_ID_DOMICILIO = 5012
            <EnumMember> <Description("RFC del domicilio")> CP_RFC_PROVEEDOR_DOMICILIO = 5013
            <EnumMember> <Description("ObjectID Domicilio")> CP_ID_DOMICILIO = 5014
            <EnumMember> <Description("Domicilio fiscal")> CA_DOMICILIO_FISCAL = 5015
            <EnumMember> <Description("Domicilio archivado")> CP_ARCHIVADO_DOMICILIO = 5016

            <EnumMember> <Description("Lista de vinculaciones con clientes")> CP_VINCULACIONES = 5017
            <EnumMember> <Description("Identificador del cliente a vincular")> CP_ID_CLIENTE_VINCULACION = 5018
            <EnumMember> <Description("Tax id vinculado")> CP_TAX_ID_VINCULACION = 5019
            <EnumMember> <Description("RFC vinculado")> CP_RFC_PROVEEDOR_VINCULACION = 5020
            <EnumMember> <Description("Identificador de la vinculación")> CA_CVE_VINCULACION = 5021
            <EnumMember> <Description("Vinculación")> CP_VINCULACION = 5022
            <EnumMember> <Description("Porcetanje de la vinculación")> CP_PORCENTAJE_VINCULACION = 5023

            <EnumMember> <Description("Lista de configuraciones")> CP_CONFIGURACIONES = 5024
            <EnumMember> <Description("Identificador del cliente a configurar")> CP_ID_CLIENTE_CONFIGURACION = 5025
            <EnumMember> <Description("Tax id configurado")> CP_TAX_ID_CONFIGURACION = 5026
            <EnumMember> <Description("RFC configurado")> CP_RFC_PROVEEDOR_CONFIGURACION = 5026
            <EnumMember> <Description("Identificador del metódo de valoración")> CA_CVE_METODO_VALORACION = 5027
            <EnumMember> <Description("Metódo de valoración")> CP_METODO_VALORACION = 5028
            <EnumMember> <Description("Identificador del termino de facturación (INCOTERM)")> CA_CVE_INCOTERM = 5029
            <EnumMember> <Description("Termino de facturación (INCOTERM)")> CP_INCOTERM = 5030

            <EnumMember> <Description("ObjectID Empresa")> CP_ID_EMPRESA = 5031


        End Enum

        Public Enum CamposDestinatario
            'Región del 6000 - 6999
            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 6000

            <EnumMember> <Description("Nombre o razón social")> CA_RAZON_SOCIAL = 6001
            <EnumMember> <Description("Tax ID")> CA_TAX_ID = 6002
            <EnumMember> <Description("RFC")> CA_RFC_DESTINATARIO = 6003

            <EnumMember> <Description("ObjectID Empresa")> CP_ID_EMPRESA = 6004

        End Enum

        Public Enum CamposRevalidacion
            'Región del 7000 - 7999
            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 7000
            '<EnumMember> <Description("Referencia")> CP_REFERENCIA = 7001
            '<EnumMember> <Description("Nombre o razón social")> CA_RAZON_SOCIAL = 7002
            <EnumMember> <Description("No. Guia Master")> CP_NO_GUIA_MASTER = 7003 ' <----
            <EnumMember> <Description("Revalidado")> CP_REVALIDADO = 7004
            <EnumMember> <Description("Fecha Revalidacion")> CP_FECHA_REVALIDACION = 7005
            <EnumMember> <Description("Tipo de Carga")> CP_TIPO_CARGA = 7006 '<--- ControladorAtipico
            <EnumMember> <Description("BL Revalidado")> CP_ID_BLREVALIDADO = 7007 'Tipo ObjectID

            <EnumMember> <Description("Clase Carga")> CP_CLASE_CARGA = 7008 '<--- evualar...
            <EnumMember> <Description("Cantidad Carga")> CP_CANTIDAD_CARGA = 7009
            <EnumMember> <Description("Peso Carga")> CP_PESO_CARGA = 7010

            <EnumMember> <Description("No. Contenedor")> CP_CONTENEDOR = 7011 'MARCASY NUMEROS <--
            <EnumMember> <Description("Tamaño Contenedor")> CP_TAMANO_CONTENEDOR = 7012 'TAMAÑO <--
            <EnumMember> <Description("Peso Contenedor")> CP_PESO_CONTENEDOR = 7013 '????
        End Enum

        Public Enum CamposViajes
            'Región del 8000 - 8999
            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 8000
            <EnumMember> <Description("Tipo de Transporte")> CP_TIPO_TRANSPORTE = 8001 '
            <EnumMember> <Description("Tipo de Operación")> CP_TIPO_OPERACION = 8002
            <EnumMember> <Description("Nave/Buque")> CP_NAVE_BUQUE = 8003
            <EnumMember> <Description("Naviera/Aereolinea")> CP_NAVIERA_AEREOLINEA = 8004
            <EnumMember> <Description("Reexpedidora/Forwarding")> CP_REEXPEDIDORA_FORWARDING = 8005
            <EnumMember> <Description("Folio de Capitania")> CP_FOLIO_CAPITANIA = 8006
            <EnumMember> <Description("Número de Viaje")> CP_NUMERO_VIAJE = 8007
            <EnumMember> <Description("Puerto Extranjero")> CP_PUERTO_EXTRANGERO = 8008
            <EnumMember> <Description("Fecha de Salida Origen")> CP_FECHA_SALIDA_ORIGEN = 8009
            <EnumMember> <Description("Fecha ETA")> CP_FECHA_ETA = 8010
            <EnumMember> <Description("Fecha ETD")> CP_FECHA_ETD = 8011
            <EnumMember> <Description("Fecha de Fondeo")> CP_FECHA_FONDEO = 8012
            <EnumMember> <Description("Fecha de Atraque")> CP_FECHA_ATRAQUE = 8013
            <EnumMember> <Description("Fecha de Cieere de Documento")> CP_FECHA_CIERRE_DOCUMENTO = 8014
            <EnumMember> <Description("Fecha de Presentación")> CP_FECHA_PRESENTACION = 8015
            '<EnumMember> <Description("Referencia")> CP_REFERENCIA = 8012
            '<EnumMember> <Description("Operación")> CP_OPERACION = 8013
            '<EnumMember> <Description("Cliente")> CA_RAZON_SOCIAL = 8014
            '<EnumMember> <Description("Estatus")> CP_ESTATUS = 8015
            '<EnumMember> <Description("Ejecutivo")> CP_EJECUTIVO = 8016
        End Enum

        Public Enum CamposProducto
            'Región del 9000 - 9999
            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 9000
            <EnumMember> <Description("Nombre Comercial")> CP_NOMBRE_COMERCIAL = 9001
            <EnumMember> <Description("Habilitado")> CP_HABILITADO = 9002
            <EnumMember> <Description("Fracción Arancelaria")> CP_FRACCION_ARANCELARIA = 9003
            <EnumMember> <Description("Nico")> CP_NICO = 9004
            <EnumMember> <Description("Fecha de Registro")> CP_FECHA_REGISTRO = 9005
            <EnumMember> <Description("Estatus")> CP_ESTATUS = 9006
            <EnumMember> <Description("Observaciones")> CP_OBSERVACION = 9007
            <EnumMember> <Description("Motivo de Archivado")> CP_MOTIVO = 9008
            <EnumMember> <Description("Id Krom")> CP_IDKROM = 9009
            <EnumMember> <Description("Número de Parte")> CP_NUMERO_PARTE = 9010
            <EnumMember> <Description("Alias")> CP_ALIAS = 9011
            <EnumMember> <Description("Descripción")> CP_DESCRIPCION = 9012
            <EnumMember> <Description("Aplica COVE")> CP_APLICACOVE = 9013
            <EnumMember> <Description("Descripcion COVE")> CP_DESCRIPCION_COVE = 9014
            <EnumMember> <Description("Fecha Modificacion")> CP_FECHA_MODIFICACION = 9015
            <EnumMember> <Description("Fecha Modificacion")> CP_TIPO_ALIAS = 9016
        End Enum

        Public Enum CamposTarifaArancelaria
            'Región del 10100 - 11000
            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 10100
            <EnumMember> <Description("Numero fraccion arancelaria")> CA_NUMERO_FRACCION_ARANCELARIA = 10101
            <EnumMember> <Description("Numero nico")> CA_NUMERO_NICO = 10102
            <EnumMember> <Description("Fraccion arancelaria")> CA_FRACCION_ARANCELARIA = 10103
            <EnumMember> <Description("Nico")> CA_NICO = 10104
            <EnumMember> <Description("Material peligroso")> CA_MATERIAL_PELIGROSO = 10105
            <EnumMember> <Description("Material vulnerable")> CA_MATERIAL_VULNERABLE = 10106
            <EnumMember> <Description("Material sensible")> CA_MATERIAL_SENSIBLE = 10107
            <EnumMember> <Description("Seccion")> CA_SECCION = 10108
            <EnumMember> <Description("Capitulo")> CA_CAPITULO = 10109
            <EnumMember> <Description("Partida")> CA_PARTIDA = 10110
            <EnumMember> <Description("Subpartida")> CA_SUBPARTIDA = 10111
            <EnumMember> <Description("Fecha publicación")> CA_FECHA_PUBLICACION = 10112
            <EnumMember> <Description("Fecha entrada en vigor")> CA_FECHA_ENTRADA_VIGOR = 10113
            <EnumMember> <Description("Fecha fin")> CA_FECHA_FIN = 10114
            <EnumMember> <Description("Fecha actualización")> PA_ACTUALIZACION = 10115
            <EnumMember> <Description("Unidad de medida")> CA_UNIDAD_MEDIDA = 10116
            <EnumMember> <Description("Nombre impuesto")> CA_NOMBRE_IMPUESTO = 10117
            <EnumMember> <Description("Valor impuesto")> CA_VALOR_IMPUESTO = 10118
            <EnumMember> <Description("Nombre tratado")> CA_NOMBRE_TRATADO = 10119
            <EnumMember> <Description("Nombre corto tratado")> CA_NOMBRE_CORTO_TRATADO = 10120
            <EnumMember> <Description("Pais")> CA_PAIS = 10121
            <EnumMember> <Description("Sector")> CA_SECTOR = 10122
            <EnumMember> <Description("Arancel")> CA_ARANCEL = 10123
            <EnumMember> <Description("Preferencia")> CA_PREFERENCIA = 10124
            <EnumMember> <Description("Observacion")> CA_OBSERVACION = 10125
            <EnumMember> <Description("Icono pais")> CA_ICONO_PAIS = 10126
            <EnumMember> <Description("Total cupo")> CA_TOTAL_CUPO = 10127
            <EnumMember> <Description("Arancel fuera")> CA_ARANCEL_FUERA = 10128
            <EnumMember> <Description("Nota")> CA_NOTA = 10129
            <EnumMember> <Description("Categoria")> CA_CATEGORIA = 10130
            <EnumMember> <Description("Tipo")> CA_TIPO = 10131
            <EnumMember> <Description("Acotacion")> CA_ACOTACION = 10132
            <EnumMember> <Description("Precio")> CA_PRECIO = 10133
            <EnumMember> <Description("Descripcion")> CA_DESCRIPCION = 10134
            <EnumMember> <Description("Nombre institucion")> CA_NOMBRE_INSTITUCION = 10135
            <EnumMember> <Description("Nombre corto institucion")> CA_NOMBRE_CORTO_INSTITUCION = 10136
            <EnumMember> <Description("Clave")> CA_CLAVE = 10137
            <EnumMember> <Description("Permiso")> CA_PERMISO = 10138
            <EnumMember> <Description("Norma")> CA_NORMA = 10139
            <EnumMember> <Description("Dato omitido inexacto")> CA_DATO_OMITIDO_INEXACTO = 10140
            <EnumMember> <Description("Numero")> CA_NUMERO = 10141
            <EnumMember> <Description("Nombre")> CA_NOMBRE = 10142
            <EnumMember> <Description("Aplicacion")> CA_APLICACION = 10143
            <EnumMember> <Description("Mercancia")> CA_MERCANCIA = 10144
            <EnumMember> <Description("Cupo")> CA_CUPO = 10145
            <EnumMember> <Description("Anexo")> CA_ANEXO = 10146
            <EnumMember> <Description("Tasa")> CA_TASA = 10147
            <EnumMember> <Description("Cuota")> CA_CUOTA = 10148
            <EnumMember> <Description("Empresa")> CA_EMPRESA = 10149
        End Enum

        Public Enum CamposManifestacionValor
            'Región del 11000 - 11200
            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 11000



            <EnumMember> <Description("Número de manifestación")> CA_NUMERO_MANIFESTACION = 11001
            <EnumMember> <Description("Fecha de manifestación")> CA_FECHA_MANIFESTACION = 11002
            <EnumMember> <Description("Presenta anexos")> CA_HAY_ANEXOS = 11003
            <EnumMember> <Description("Precio pagado")> CA_VALOR_PAGADO = 11004
            <EnumMember> <Description("Conceptos que no integran valor de transaccion")> CA_CONCEPTOS_NO_VALOR = 11005
            <EnumMember> <Description("Anexa documentos Art 66")> CA_ANEXA_DOC_66 = 11006
            <EnumMember> <Description("Numero del anexo")> CA_NUM_ANEXO = 11007
            <EnumMember> <Description("Factura o documento anexado")> CP_FACTURA_DOCUMENTO_66 = 11008
            <EnumMember> <Description("Mercancía")> CA_MERCANCIA_66 = 11009
            <EnumMember> <Description("Factura o documento comercial")> CA_FAC_DOC_COMERCIAL_66 = 11010
            <EnumMember> <Description("Importe y moneda")> CA_IMPORTE_MONEDA_66 = 11011
            <EnumMember> <Description("Concepto del cargo")> CA_CONCEPTO_CARGO_66 = 11012
            <EnumMember> <Description("Precio pagado comprende conceptos señalados en art 65")> CA_PRECIO_COMPRENDE_ART_65 = 11013
            <EnumMember> <Description("Acompañar las facturas")> CA_ACOMPANA_FACT = 11014
            <EnumMember> <Description("Anexa documentos Art 65")> CA_ANEXA_DOC_65 = 11015
            <EnumMember> <Description("Periodicidad")> CA_PERIODICIDAD = 11016
            <EnumMember> <Description("Representante legal")> CA_REPRESENTANTE_LEGAL = 11017
            <EnumMember> <Description("Fecha manifestación")> CA_FECHA_MANIFESTACION_66 = 11018
            <EnumMember> <Description("Numero del anexo")> CA_NUM_ANEXO_65 = 110019
            <EnumMember> <Description("Factura o documento anexado")> CP_FACTURA_DOCUMENTO_65 = 11020
            <EnumMember> <Description("Mercancía")> CA_MERCANCIA_65 = 11021
            <EnumMember> <Description("Factura o documento comercial")> CA_FAC_DOC_COMERCIAL_65 = 11022
            <EnumMember> <Description("Importe y moneda")> CA_IMPORTE_MONEDA_65 = 11023
            <EnumMember> <Description("Concepto del cargo")> CA_CONCEPTO_CARGO_65 = 11024
        End Enum

#Region "Builders"
        Sub New()

        End Sub

#End Region


#Region "Methods"


#End Region

    End Class

End Namespace
