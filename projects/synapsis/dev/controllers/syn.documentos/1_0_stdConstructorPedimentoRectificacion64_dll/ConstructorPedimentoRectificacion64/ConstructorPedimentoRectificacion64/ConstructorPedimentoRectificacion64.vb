Imports System.Runtime.Serialization.Formatters.Binary
Imports gsol.krom
Imports Syn.Documento.Componentes
Imports Syn.Documento.Componentes.Campo.TiposDato
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Nucleo.RecursosComercioExterior.CamposPedimento
Imports Syn.Operaciones
Imports Wma.Exceptions

Namespace Syn.Documento
    <Serializable()>
    Public Class ConstructorPedimentoRectificacion
        Inherits EntidadDatosDocumento
        Implements ICloneable

#Region "Attributes"

#End Region

#Region "Builders"
        Sub New(ByVal tipoDocumento_ As TiposDocumentoElectronico,
                ByVal construir_ As Boolean,
                Optional ByVal documentoElectronico_ As DocumentoElectronico = Nothing)

            inicializa(documentoElectronico_,
                            tipoDocumento_,
                            construir_)

        End Sub
        Public Sub New(ByVal tipoDocumento_ As TiposDocumentoElectronico,
                           ByVal folioDocumento_ As String,
                           ByVal folioOperacion_ As String,
                           ByVal idCliente_ As Int32,
                           ByVal nombreCliente_ As String
                           )

            Inicializa(folioDocumento_,
                             folioOperacion_,
                             idCliente_,
                             nombreCliente_,
                             tipoDocumento_)

        End Sub

#End Region

#Region "Methods"
        Public Overrides Sub ConstruyeEncabezado()

            _estructuraDocumento(TiposBloque.Encabezado) = New List(Of Nodo)

            ' Encabezado principal de la referencia
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS1,
                             tipoBloque_:=TiposBloque.Encabezado,
                             conCampos_:=True)

        End Sub
        Public Overrides Sub ConstruyeEncabezadoPaginasSecundarias()

            _estructuraDocumento(TiposBloque.EncabezadoPaginasSecundarias) = New List(Of Nodo)

            ' Encabezado principal del pedimento
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS2,
                                 tipoBloque_:=TiposBloque.EncabezadoPaginasSecundarias,
                                 conCampos_:=True)

        End Sub
        Public Overrides Sub ConstruyeCuerpo()

            _estructuraDocumento(TiposBloque.Cuerpo) = New List(Of Nodo)

            'Datos del importador/exportador
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS3,
                                 tipoBloque_:=TiposBloque.Cuerpo,
                                 conCampos_:=True)

            'Prueba suficiente
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS5,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Tasas a nivel pedimento
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS6,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Cuadro de liquidación
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS7,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=True)

            'Deposito referenciado - línea de captura - información del pago
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS9,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=True)

            'Datos del proveedor o comprador
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS10,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Datos del destinatario
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS11,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Datos del transporte y transportista
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS12,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Fechas
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS14,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=True)

            'Candados
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS15,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Guias, Manifiestos, conocimientos de embarque o documentos de transporte
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS16,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Contenedores/Equipo ferrocarril/Número economico del vehiculo.
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS17,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)


            'Identificadores (Nivel pedimento)
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS18,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Cuentas aduaneras y cuentas aduaneras de garantia (Nivel pedimento)
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS19,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Descargos
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS20,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Compensaciones
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS21,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Documentos que amparan las formas de pago distintas a efectivo….
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS22,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Rectificaciones
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS37,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Diferencias de contribuciones ( Nivel pedimento )
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS38,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Observaciones ( nivel pedimento)
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS23,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=True)

            'Partidas
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS24,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Fin del pedimento
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS43,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=True)
        End Sub
        Public Overrides Sub ConstruyePiePagina()

            _estructuraDocumento(TiposBloque.PiePagina) = New List(Of Nodo)

            With _estructuraDocumento(TiposBloque.PiePagina)

                'Pie de pagina del pedimento
                ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS44,
                                 tipoBloque_:=TiposBloque.PiePagina,
                                 conCampos_:=True)

            End With

        End Sub

#End Region

#Region "Funciones"

        Public Overrides Function ObtenerCamposSeccion(ByVal idSeccion_ As Integer) As List(Of Nodo)

            Select Case idSeccion_

                'Encabezado principal del pedimento
                Case SeccionesPedimento.ANS1
                    Return New List(Of Nodo) From {
                                                    Item(CA_NUM_PEDIMENTO_COMPLETO, Texto, 21),
                                                    Item(CA_T_OPER, Texto, 3),
                                                    Item(CA_CVE_PEDIMENTO, Texto, 2),
                                                    Item(CA_REGIMEN, Texto, 3),
                                                    Item(CA_DESTINO_ORIGEN, Entero, 2),
                                                    Item(CA_TIPO_CAMBIO, Real, cantidadEnteros_:=4, cantidadDecimales_:=5),
                                                    Item(CA_PESO_BRUTO, Real, cantidadEnteros_:=11, cantidadDecimales_:=3),
                                                    Item(CA_ADUANA_E_S, Texto, 3),
                                                    Item(CA_MEDIO_DE_TRANSPORTE, Entero, 2),
                                                    Item(CA_MEDIO_DE_TRANSPORTE_DE_ARRIBO, Entero, 2),
                                                    Item(CA_MEDIO_DE_TRANSPORTE_DE_SALIDA, Entero, 2),
                                                    Item(CA_VALOR_DOLARES, Real, cantidadEnteros_:=12, cantidadDecimales_:=2),
                                                    Item(CA_VALOR_ADUANA, Entero, 12),
                                                    Item(CA_PRECIO_PAGADO_O_VALOR_COMERCIAL, Entero, 12),
                                                    Item(CA_VAL_SEGUROS, Entero, 12),
                                                    Item(CA_SEGUROS, Entero, 12),
                                                    Item(CA_FLETES, Entero, 12),
                                                    Item(CA_EMBALAJES, Entero, 12),
                                                    Item(CA_OTROS_INCREMENTABLES, Entero, 12),
                                                    Item(CA_TRANSPORTE_DECREMENTABLES, Entero, 12),
                                                    Item(CA_SEGURO_DECREMENTABLES, Entero, 12),
                                                    Item(CA_CARGA_DECREMENTABLES, Entero, 12),
                                                    Item(CA_DESCARGA_DECREMENTABLES, Entero, 12),
                                                    Item(CA_OTROS_DECREMENTABLES, Entero, 12),
                                                    Item(CA_ACUSE_ELECTONICO_DE_VALIDACION, Texto, 8),
                                                    Item(CA_CODIGO_DE_BARRAS, Texto, 6), ' Revisar captura
                                                    Item(CA_CLAVE_SAD, Texto, 3),
                                                    Item(CA_MARCAS_NUMEROS_TOTAL_BULTOS, Texto),
                                                    Item(CA_CERTIFICACION, Texto, 6), 'Revisar captura
                                                    Item(CA_ADUANA_DESPACHO, Texto, 3),
                                                    Item(CA_AÑO_VALIDACION_2, Texto, 2),
                                                    Item(CA_ADUANA_SIN_SECCION, Texto, 2),
                                                    Item(CA_CVE_TIPO_OPERACION, Entero, 1),
                                                    Item(CA_FECHA_VALIDACION, Fecha),
                                                    Item(CP_REFERENCIA, Texto, 8),
                                                    Item(CP_MODALIDAD_ADUANA_PATENTE, Entero, 3),
                                                    Item(CP_MODALIDAD, Entero, 1),
                                                    Item(CP_EJECUTIVO_DE_CUENTA, Entero, 5),
                                                    Item(CP_NUMERO_CLIENTE, Entero, 6)
                    }

                ' Encabezado para páginas secundarias del pedimento
                Case SeccionesPedimento.ANS2
                    Return New List(Of Nodo) From {
                                                    Item(CA_TIPO_CAMBIO, Real, cantidadEnteros_:=4, cantidadDecimales_:=5),
                                                    Item(CA_NUM_PEDIMENTO_COMPLETO, Texto, 21),
                                                    Item(CA_T_OPER, Texto, 3),
                                                    Item(CA_CVE_PEDIMENTO, Texto, 2),
                                                    Item(CA_RFC_DEL_IOE, Texto, 13),
                                                    Item(CA_CURP_DEL_IOE, Texto, 18)
                    }

                ' Datos del importador/exportador
                Case SeccionesPedimento.ANS3
                    Return New List(Of Nodo) From {
                                                    Item(CA_RFC_DEL_IOE, Texto, 13),
                                                    Item(CA_CURP_DEL_IOE, Texto, 18),
                                                    Item(CA_RAZON_SOCIAL_IOE, Texto, 120),
                                                    Item(CA_DOMICILIO_IOE, Texto, 196),
                                                    Item(CA_CALLE_IOE, Texto, 80),
                                                    Item(CA_NUM_INTER_IOE, Texto, 10),
                                                    Item(CA_NUM_EXTERIOR_IOE, Texto, 10),
                                                    Item(CA_CODIGO_POSTAL_IOE, Texto, 10),
                                                    Item(CA_MUNICIPIO_CIUDAD_IOE, Texto, 80),
                                                    Item(CA_ENTIDAD_FEDERATIVA_IOE, Texto, 3),
                                                    Item(CA_PAIS_IOE, Texto, 3)
                    }

                ' Datos generales del pedimento complementario
                Case SeccionesPedimento.ANS4
                    Return New List(Of Nodo)

                ' Prueba suficiente
                Case SeccionesPedimento.ANS5
                    Return New List(Of Nodo) From {
                                                    Item(CA_CONCEPTO, Texto, 7),
                                                    Item(CA_FP, Entero, 3),
                                                    Item(CA_IMPORTE, Entero, 12),
                                                    Item(CA_CVE_CONCEPTO_NIVEL_PEDIMENTO, Entero, 2),
                                                    Item(CA_CVE_PAIS_EXPORT, Texto, 3)
                    }

                ' Tasas a nivel pedimento
                Case SeccionesPedimento.ANS6
                    Return New List(Of Nodo) From {
                                                    Item(CA_CONTRIBUCION, Texto, 7),
                                                    Item(CA_CVE_T_TASA, Entero, 2),
                                                    Item(CA_TASA, Real, cantidadEnteros_:=5, cantidadDecimales_:=10),
                                                    Item(CA_CVE_CONCEPTO_NIVEL_PEDIMENTO, Entero, 2)
                    }

                ' Cuadro de liquidación
                Case SeccionesPedimento.ANS7
                    Return New List(Of Nodo) From {
                                                    Item(CA_EFECTIVO, Entero, 12),
                                                    Item(CA_OTROS, Entero, 12),
                                                    Item(CA_TOTAL, Entero, 12),
                                                    Item(SeccionesPedimento.ANS55, False)
                    }

                'Partidas del cuadro de liquidación
                Case SeccionesPedimento.ANS55
                    Return New List(Of Nodo) From {
                                                    Item(CA_CONCEPTO, Texto, 7),
                                                    Item(CA_FP, Entero, 3),
                                                    Item(CA_IMPORTE, Entero, 12),
                                                    Item(CA_CVE_CONCEPTO_NIVEL_PEDIMENTO, Entero, 2)
                    }

                ' Informe de la industria automotriz
                Case SeccionesPedimento.ANS8
                    Return New List(Of Nodo)

                ' Deposito referenciado - línea de captura - información del pago
                Case SeccionesPedimento.ANS9
                    Return New List(Of Nodo) From {
                                                    Item(CA_DEP_REFERENCIADO, Texto),
                                                    Item(CA_NUM_PEDIMENTO, Texto, 7),
                                                    Item(CA_PATENTE, Texto, 4),
                                                    Item(CA_PAGO_ELECTRONICO, Texto, 22),
                                                    Item(CA_NOMBRE_INST_BANCARIA, Texto, 30),
                                                    Item(CA_LINEA_CAPTURA, Texto, 20),
                                                    Item(CA_NUM_OPERACION_BANCARIA, Texto, 20),
                                                    Item(CA_NUM_TRANSACCION_SAT, Texto, 25),
                                                    Item(CA_MEDIO_PRESENTACION, Texto, 23),
                                                    Item(CA_MEDIO_RECEPCION_COBRO, Texto, 26),
                                                    Item(CA_ADUANA_DESPACHO, Texto, 3),
                                                    Item(CA_EFECTIVO, Entero, 12),
                                                    Item(CA_FECHA_PAGO, Fecha)
                    }

                ' Datos del proveedor o comprador
                Case SeccionesPedimento.ANS10
                    Return New List(Of Nodo) From {
                                                    Item(CA_ID_FISCAL_PROVEEDOR, Texto, 30),
                                                    Item(CA_NOMBRE_DEN_RAZON_SOC_POC, Texto, 120),
                                                    Item(CA_DOMICILIO_POC, Texto, 190),
                                                    Item(CA_VINCULACION, Texto, 2),
                                                    Item(CA_CVE_VINCULACION, Entero, 1),
                                                    Item(CA_CALLE_POC, Texto, 80),
                                                    Item(CA_NUMERO_INT_POC, Texto, 10),
                                                    Item(CA_NUMERO_EXTER_POC, Texto, 10),
                                                    Item(CA_CODIGO_POSTAL_POC, Texto, 10),
                                                    Item(CA_MUNICIPIO_CIUDAD_POC, Texto, 80),
                                                    Item(CA_ENTIDAD_FEDERATIVA_POC, Texto, 3),
                                                    Item(CA_PAIS_POC, Texto, 3),
                                                    Item(SeccionesPedimento.ANS13, False)
                    }

                ' Datos del destinatario
                Case SeccionesPedimento.ANS11
                    Return New List(Of Nodo) From {
                                                    Item(CA_ID_FISCAL_DESTINATARIO, Texto, 17),
                                                    Item(CA_NOMBRE_RAZON_SOC_DESTINATARIO, Texto, 120),
                                                    Item(CA_DOMICILIO_DESTINATARIO, Texto, 193),
                                                    Item(CA_CALLE_DESTINATARIO, Texto, 80),
                                                    Item(CA_NUMERO_INT_DESTINATARIO, Texto, 10),
                                                    Item(CA_NUMERO_EXTER_DESTINATARIO, Texto, 10),
                                                    Item(CA_CODIGO_POSTAL_DESTINATARIO, Texto, 10),
                                                    Item(CA_MUNICIPIO_CIUDAD_DESTINATARIO, Texto, 80),
                                                    Item(CA_PAIS_DESTINATARIO, Texto, 3)
                    }

                ' Datos del transporte y transportista
                Case SeccionesPedimento.ANS12
                    Return New List(Of Nodo) From {
                                                    Item(CA_ID_TRANSPORT, Texto, 30),
                                                    Item(CA_CVE_PAIS_TRANSP, Texto, 3),
                                                    Item(CA_NOMBRE_RAZON_SOC_TRANSP, Texto, 120),
                                                    Item(CA_CVE_RFC_TRANSP, Texto, 13),
                                                    Item(CA_CURP_TRANSP_PERSONA_FISICA, Texto, 18),
                                                    Item(CA_DOMICILIO_TRANSP, Texto, 150),
                                                    Item(CA_TOTAL_BULTOS, Entero, 5)
                    }

                ' CFDi o ducumento equivalente
                Case SeccionesPedimento.ANS13
                    Return New List(Of Nodo) From {
                                                    Item(CA_CFDI_O_FACT, Texto, 40),
                                                    Item(CA_FECHA_FACT, Fecha),
                                                    Item(CA_INCOTERM, Texto, 3),
                                                    Item(CA_CVE_MONEDA_FACT, Texto, 3),
                                                    Item(CA_MONTO_MONEDA_FACT, Real, cantidadEnteros_:=12, cantidadDecimales_:=2),
                                                    Item(CA_FACTOR_MONEDA, Real, cantidadEnteros_:=2, cantidadDecimales_:=5),
                                                    Item(CA_MONTO_USD, Real, cantidadEnteros_:=12, cantidadDecimales_:=2),
                                                    Item(CA_NUMERO_ACUSE_DE_VALOR, Texto, 40)
                    }

                ' Fechas
                Case SeccionesPedimento.ANS14
                    Return New List(Of Nodo) From {
                                                    Item(CA_FECHA_ENTRADA, Fecha),
                                                    Item(CA_FECHA_PAGO, Fecha),
                                                    Item(CA_FECHA_EXTRACCION, Fecha),
                                                    Item(CA_FECHA_PRESENTACION, Fecha),
                                                    Item(CA_FECHA_IMP_EUA_CAN, Fecha),
                                                    Item(CA_FECHA_ORIGINAL, Fecha)
                    }

                ' Candados
                Case SeccionesPedimento.ANS15
                    Return New List(Of Nodo) From {
                                                    Item(CA_NUM_CANDADO, Texto, 21),
                                                    Item(CA_NUM_CANDADO_1RA, Texto),
                                                    Item(CA_NUM_CANDADO_2DA, Texto)
                    }

                ' Guias, Manifiestos, conocimientos de embarque o documentos de transporte
                Case SeccionesPedimento.ANS16
                    Return New List(Of Nodo) From {
                                                    Item(CA_GUIA_O_MANIF_O_BL, Texto, 20),
                                                    Item(CA_MASTER_O_HOUSE, Texto, 1)
                    }

                ' Contenedores/Equipo ferrocarril/Número economico del vehiculo
                Case SeccionesPedimento.ANS17
                    Return New List(Of Nodo) From {
                                                    Item(CA_NUM_CONTENEDOR_FERRO_NUM_ECON, Texto, 12),
                                                    Item(CA_CVE_TIPO_CONTENEDOR, Entero, 2)
                    }

                ' Identificadores (Nivel pedimento)
                Case SeccionesPedimento.ANS18
                    Return New List(Of Nodo) From {
                                                    Item(CA_CVE_IDENTIFICADOR_G, Texto, 2),
                                                    Item(CA_COMPL_1, Texto, 20),
                                                    Item(CA_COMPL_2, Texto, 30),
                                                    Item(CA_COMPL_3, Texto, 40)
                    }

                ' Cuentas aduaneras y cuentas aduaneras de garantia (Nivel pedimento)
                Case SeccionesPedimento.ANS19
                    Return New List(Of Nodo) From {
                                                    Item(CA_CVE_CTA_ADUANERA, Entero, 1),
                                                    Item(CA_CVE_TIPO_GARANTIA, Entero, 1),
                                                    Item(CA_NOMBRE_INST_EMISORA_CTA, Texto, 120),
                                                    Item(CA_NUM_CONTRATO, Entero, 17),
                                                    Item(CA_FOLIO_CONSTANCIA, Entero, 17),
                                                    Item(CA_IMPORTE_TOTAL_CONSTANCIA, Real, cantidadEnteros_:=12, cantidadDecimales_:=2),
                                                    Item(CA_FECHA_EMISION_CONSTANCIA, Fecha),
                                                    Item(CA_CANTIDAD_UMT_PRECIO_ESTIMADO_PEDIMENTO, Real, cantidadEnteros_:=10, cantidadDecimales_:=4),
                                                    Item(CA_TITULOS_ASIGNADOS_PEDIMENTO, Real, cantidadEnteros_:=12, cantidadDecimales_:=2),
                                                    Item(CA_VALOR_UNITARIO_TITULO_PEDIMENTO, Real, cantidadEnteros_:=10, cantidadDecimales_:=4),
                                                    Item(CA_CVE_INSTITUCION_EMISORA_CTA_ADUANERA_PEDIMENTO, Entero, 1)
                    }

                ' Descargos
                Case SeccionesPedimento.ANS20
                    Return New List(Of Nodo) From {
                                                    Item(CA_NUM_PEDIM_ORIGINAL_COMPLETO, Texto, 21),
                                                    Item(CA_FECHA_PEDIM_ORIGINAL, Fecha),
                                                    Item(CA_CVE_PEDIM_ORIGINAL, Texto, 2),
                                                    Item(CA_AÑO_VALIDACION_ORIGINAL, Texto, 4),
                                                    Item(CA_AÑO_VALIDACION_2_ORIGINAL, Texto, 2),
                                                    Item(CA_PATENTE_ORIGINAL, Texto, 4),
                                                    Item(CA_ADUANA_DESPACHO_ORIGINAL, Texto, 3),
                                                    Item(CA_ADUANA_DESPACHO_ORIGINAL_2, Texto, 2),
                                                    Item(CA_NUM_PEDIMENTO_ORIGINAL_7_DIGITOS, Texto, 7),
                                                    Item(CA_FRACCION_ORIGINAL, Texto, 8),
                                                    Item(CA_UM_ORIGINAL, Entero, 2),
                                                    Item(CA_CANT_MERCANCIA_UMT_DESCARGO, Real, cantidadEnteros_:=12, cantidadDecimales_:=5)
                    }

                ' Compensaciones
                Case SeccionesPedimento.ANS21
                    Return New List(Of Nodo) From {
                                                    Item(CA_CONTRIBUCION, Texto, 7),
                                                    Item(CA_NUM_PEDIM_ORIGINAL_COMPLETO, Texto, 2),
                                                    Item(CA_FECHA_PAGO_ORIG_PARA_COMPENSAC, Fecha),
                                                    Item(CA_IMPORTE_GRAVAMEN_COMPENSACION, Entero, 12),
                                                    Item(CA_CVE_CONCEPTO_COMPENSACION, Entero, 2),
                                                    Item(CA_AÑO_VALIDACION_ORIGINAL, Texto, 4),
                                                    Item(CA_AÑO_VALIDACION_2_ORIGINAL, Texto, 2),
                                                    Item(CA_PATENTE_ORIGINAL, Texto, 4),
                                                    Item(CA_ADUANA_DESPACHO_ORIGINAL, Texto, 3),
                                                    Item(CA_ADUANA_DESPACHO_ORIGINAL_2, Texto, 2),
                                                    Item(CA_NUM_PEDIMENTO_ORIGINAL_7_DIGITOS, Texto, 7)
                    }

                ' Documentos que amparan las formas de pago distintas a efectivo
                Case SeccionesPedimento.ANS22

                    Return New List(Of Nodo) From {
                                                    Item(CA_FP, Entero, 3),
                                                    Item(CA_NOMBRE_INST_EMISORA_DOCTO, Texto, 120),
                                                    Item(CA_NUM_DOCTO, Texto, 40),
                                                    Item(CA_FECHA_EXP_DOCTO, Fecha),
                                                    Item(CA_MONTO_DOCTO, Real, cantidadEnteros_:=12, cantidadDecimales_:=2),
                                                    Item(CA_SALDO_DISP_DOCTO, Real, cantidadEnteros_:=12, cantidadDecimales_:=2),
                                                    Item(CA_MONTO_PAG_PEDIM, Entero, 14)
                    }

                ' Observaciones ( nivel pedimento)
                Case SeccionesPedimento.ANS23
                    Return New List(Of Nodo) From {
                                                    Item(CA_OBSERV_PEDIM, Texto)
                    }

                ' Partidas
                Case SeccionesPedimento.ANS24
                    Return New List(Of Nodo) From {
                                                    Item(CA_NUM_SEC_PARTIDA, Entero, 5),
                                                    Item(CA_FRACC_ARANC_PARTIDA, Texto, 8),
                                                    Item(CA_NICO_PARTIDA, Texto, 2),
                                                    Item(CA_CVE_MET_VALOR_PARTIDA, Entero, 2),
                                                    Item(CA_CVE_UMC_PARTIDA, Entero, 2),
                                                    Item(CA_CANT_UMC_PARTIDA, Real, cantidadEnteros_:=12, cantidadDecimales_:=3),
                                                    Item(CA_CVE_UMT_PARTIDA, Entero, 2),
                                                    Item(CA_CANT_UMT_PARTIDA, Real, cantidadEnteros_:=13, cantidadDecimales_:=5),
                                                    Item(CA_CVE_PAIS_VEND_O_COMP_PARTIDA, Texto, 3),
                                                    Item(CA_CVE_PAIS_ORIGEN_O_DEST_PARTIDA, Texto, 3),
                                                    Item(CA_DESCRIP_MERC_PARTIDA, Texto, 250),
                                                    Item(CA_VAL_ADU_O_VAL_USD_PARTIDA, Entero, 12),
                                                    Item(CA_IMP_PRECIO_PAG_O_VAL_COMER_PARTIDA, Entero, 12),
                                                    Item(CA_MONTO_PRECIO_UNITARIO_PARTIDA, Real, cantidadEnteros_:=10, cantidadDecimales_:=5),
                                                    Item(CA_MONTO_VALOR_AGREG_PARTIDA, Entero, 12),
                                                    Item(CA_NOMBRE_MARCA_PARTIDA, Texto, 80),
                                                    Item(CA_CVE_MODELO_PARTIDA, Texto, 80),
                                                    Item(CA_CODIGO_PRODUCTO_PARTIDA, Texto, 20),
                                                    Item(CA_VINCULACION, Texto, 2),
                                                    Item(CA_CVE_VINCULACION, Entero, 1),
                                                    Item(CA_ENTIDAD_FEDER_ORIGEN, Texto, 3),
                                                    Item(CA_ENTIDAD_FEDER_DESTINO, Texto, 3),
                                                    Item(CA_ENTIDAD_FEDER_COMPRADOR, Texto, 3),
                                                    Item(CA_ENTIDAD_FEDER_VENDEDOR, Texto, 3),
                                                    Item(SeccionesPedimento.ANS25, False),
                                                    Item(SeccionesPedimento.ANS26, False),
                                                    Item(SeccionesPedimento.ANS27, False),
                                                    Item(SeccionesPedimento.ANS28, False),
                                                    Item(SeccionesPedimento.ANS29, False),
                                                    Item(SeccionesPedimento.ANS32, False),
                                                    Item(SeccionesPedimento.ANS33, False),
                                                    Item(SeccionesPedimento.ANS34, False),
                                                    Item(SeccionesPedimento.ANS35, False),
                                                    Item(SeccionesPedimento.ANS36, False)
                    }

                ' Mercancias
                Case SeccionesPedimento.ANS25
                    Return New List(Of Nodo) From {
                                                    Item(CA_VIN_O_NUM_SERIE_MERCA, Texto, 25),
                                                    Item(CA_KILOM, Entero, 6)
                    }

                ' Regulaciones y restricciones no arancelarias
                Case SeccionesPedimento.ANS26
                    Return New List(Of Nodo) From {
                                                    Item(CA_CVE_PERMISO, Texto, 3),
                                                    Item(CA_NUM_PERMISO, Texto, 50),
                                                    Item(CA_FIRM_ELECTRON_PERMISO, Texto, 32),
                                                    Item(CA_MONTO_USD_VAL_COM, Real, cantidadEnteros_:=12, cantidadDecimales_:=2),
                                                    Item(CA_CANT_UMT_O_UMC, Real, cantidadEnteros_:=13, cantidadDecimales_:=5)
                    }

                ' Identificadores ( Nivel partida )
                Case SeccionesPedimento.ANS27
                    Return New List(Of Nodo) From {
                                                    Item(CA_CVE_IDENTIF_PARTIDA, Texto, 2),
                                                    Item(CA_COMPL_1_PARTIDA, Texto, 20),
                                                    Item(CA_COMPL_2_PARTIDA, Texto, 50),
                                                    Item(CA_COMPL_3_PARTIDA, Texto, 40)
                    }

                ' Cuentas aduaneras de garantia ( Nivel partida)
                Case SeccionesPedimento.ANS28
                    Return New List(Of Nodo) From {
                                                    Item(CA_CVE_TIPO_GARANTIA_PARTIDA, Entero, 2),
                                                    Item(CA_INST_EMISORA_GARANTIA_PARTIDA, Texto, 120),
                                                    Item(CA_FECHA_EXP_CONSTANCIA_PARTIDA, Fecha),
                                                    Item(CA_NUM_CTA_GARANTIA_PARTIDA, Entero, 17),
                                                    Item(CA_FOLIO_CONSTANCIA_PARTIDA, Entero, 17),
                                                    Item(CA_MONTO_TOTAL_CONSTANCIA_PARTIDA, Real, cantidadEnteros_:=12, cantidadDecimales_:=2),
                                                    Item(CA_PRECIO_ESTIMADO_PARTIDA, Real, cantidadEnteros_:=10, cantidadDecimales_:=4),
                                                    Item(CA_CANT_UMT_PRECIO_ESTIMADO_PARTIDA, Real, cantidadEnteros_:=10, cantidadDecimales_:=4),
                                                    Item(CA_CVE_INSTITUCION_EMISORA_CTA_ADUANERA_PARTIDA, Entero, 1)
                    }

                ' Tasas y contribuciones a nivel partida
                Case SeccionesPedimento.ANS29
                    Return New List(Of Nodo) From {
                                                    Item(CA_CONTRIBUCION_PARTIDA, Texto, 7),
                                                    Item(CA_TASA_PARTIDA, Real, cantidadEnteros_:=5, cantidadDecimales_:=10),
                                                    Item(CA_CVE_T_TASA_PARTIDA, Entero, 2),
                                                    Item(CA_FP_PARTIDA, Entero, 3),
                                                    Item(CA_IMPORTE_PARTIDA, Entero, 12),
                                                    Item(CA_CVE_CONTRIBUCION_NIVEL_PARTIDA, Entero, 2)
                    }

                ' Contribuciones a nivel partida
                Case SeccionesPedimento.ANS30
                    Return New List(Of Nodo)

                ' Partidas del informe de la industria automotriz
                Case SeccionesPedimento.ANS31
                    Return New List(Of Nodo)

                ' Determinación de contribuciones a nivel partida al amparo del Art 2.5 del T-MEC
                Case SeccionesPedimento.ANS32
                    Return New List(Of Nodo) From {
                                                    Item(CA_MONTO_MERC_NO_ORIGIN_PARTIDA, Entero, 12),
                                                    Item(CA_MONTO_IGI_PARTIDA, Entero, 12)
                    }

                ' Detalle de importación a EUA/CAN al amparo del Art. 2.5 del T-MEC
                Case SeccionesPedimento.ANS33
                    Return New List(Of Nodo) From {
                                                    Item(CA_NUM_SEC_PARTIDA, Entero, 5),
                                                    Item(CA_FRACC_ARANC_PARTIDA, Entero, 8),
                                                    Item(CA_NUM_PEDIMENTO, Texto, 7),
                                                    Item(CA_CVE_PAIS_EXPORT, Texto, 3)
                    }

                ' Determinación y/o pago de contribuciones por aplicación del art 2.5 del TMEC en el pedimento de exporación(Retorno)
                Case SeccionesPedimento.ANS34
                    Return New List(Of Nodo) From {
                                                    Item(CA_NUM_SEC_PARTIDA, Entero, 5),
                                                    Item(CA_FRACC_ARANC_PARTIDA, Texto, 8),
                                                    Item(CA_MONTO_MERC_NO_ORIGIN_PARTIDA, Entero, 12),
                                                    Item(CA_MONTO_IGI_PARTIDA, Entero, 12),
                                                    Item(CA_NUM_PEDIMENTO, Texto, 7)
                    }

                ' Pago de contribuciones a nivel partida por aplicación del Art. 2.5 del T-MEC
                Case SeccionesPedimento.ANS35
                    Return New List(Of Nodo) From {
                                                    Item(CA_NUM_SEC_PARTIDA, Entero, 5),
                                                    Item(CA_FRACC_ARANC_PARTIDA, Texto, 8),
                                                    Item(CA_CONTRIBUCION_PARTIDA, Entero, 12),
                                                    Item(CA_FP_PARTIDA, Entero, 3),
                                                    Item(CA_IMPORTE_PARTIDA, Entero, 12),
                                                    Item(CA_NUM_PEDIMENTO, Texto, 7),
                                                    Item(CA_CVE_PAIS_EXPORT, Texto, 3)
                    }

                ' Observaciones ( Nivel partida )
                Case SeccionesPedimento.ANS36
                    Return New List(Of Nodo) From {
                                                    Item(CA_OBSERV_PARTIDA, Texto)
                    }

                ' Rectificaciones
                Case SeccionesPedimento.ANS37
                    Return New List(Of Nodo) From {
                                                    Item(CA_FECHA_PEDIM_ORIGINAL, Fecha),
                                                    Item(CA_CVE_PEDIM_ORIGINAL, Texto, 2),
                                                    Item(CA_PATENTE_ORIGINAL, Texto, 4),
                                                    Item(CA_NUM_PEDIMENTO_ORIGINAL_7_DIGITOS, Texto, 7),
                                                    Item(CA_AÑO_VALIDACION_2_ORIGINAL, Texto, 2),
                                                    Item(CA_AÑO_VALIDACION_ORIGINAL, Texto, 4),
                                                    Item(CA_ADUANA_DESPACHO_ORIGINAL, Texto, 3),
                                                    Item(CA_ADUANA_DESPACHO_ORIGINAL_2, Texto, 2),
                                                    Item(CA_CVE_PEDIM_RECTIF, Texto, 72),
                                                    Item(CA_FECHA_PAG_PEDIM_RECTIFICACION, Fecha)
                    }

                ' Diferencias de contribuciones ( Nivel pedimento )
                Case SeccionesPedimento.ANS38
                    Return New List(Of Nodo) From {
                                                    Item(CA_DIFERENCIA, Entero, 7),
                                                    Item(CA_DIFERENCIA_EFECTIVO, Entero, 7),
                                                    Item(CA_DIFERENCIA_OTROS, Entero, 7),
                                                    Item(CA_DIFERENCIA_TOTAL, Entero, 7),
                                                    Item(CA_CONCEPTO_DIF_CONTRIB, Texto, 7),
                                                    Item(CA_CVE_CONCEPTO_DIF_CONTRIB, Entero, 2),
                                                    Item(CA_FORMA_PAGO_DIFERENCIAS, Entero, 2)
                    }

                ' Prueba suficiente
                Case SeccionesPedimento.ANS39
                    Return New List(Of Nodo)

                ' Encabezado para determinacion de contribuciones a nivel partdida para pedimentos complementarios al amparo del art. T-Mec
                Case SeccionesPedimento.ANS40
                    Return New List(Of Nodo)

                ' Encabezado para determinación de contribuciones a nivel partida para pedimentos complementarios al amparo del los articulos 14 de la decision o 15 del TLCAELC
                Case SeccionesPedimento.ANS41
                    Return New List(Of Nodo)

                ' Instructivo de llenado del pedimento de tránsito para el transbordo
                Case SeccionesPedimento.ANS42
                    Return New List(Of Nodo)

                ' Fin de pedimento
                Case SeccionesPedimento.ANS43
                    Return New List(Of Nodo) From {
                                                        Item(CA_FIN_PEDIMENTO, Texto, 17),
                                                        Item(CA_NUMERO_TOTAL_PARTIDAS, Entero, 4),
                                                        Item(CA_CVE_PREVAL, Texto, 3)
                    }

                ' Pie de pagina del pedimento
                Case SeccionesPedimento.ANS44
                    Return New List(Of Nodo) From {
                                                        Item(CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA, Texto, 120),
                                                        Item(CA_RFC_AA, Texto, 13),
                                                        Item(CA_CURP_AA_O_REP_LEGAL, Texto, 18),
                                                        Item(CA_NOMBRE_MAND_REP_AA, Texto, 120),
                                                        Item(CA_RFC_MAND_O_AGAD_REP_ALMACEN, Texto, 13),
                                                        Item(CA_CURP_MAND_O_AGAD_REP_ALMACEN, Texto, 18),
                                                        Item(CA_PATENTE, Texto, 4),
                                                        Item(CA_EFIRMA, Texto, 360),
                                                        Item(CA_CERTIFICADO_FIRMA, Texto, 25),
                                                        Item(CA_TIPO_FIGURA, Entero, 1)
                    }

                Case Else

                    _tagWatcher.SetError(Me, "La sección con clave:" & idSeccion_ & " no esta configurada.")

            End Select

            Return Nothing

        End Function

#End Region

    End Class

End Namespace