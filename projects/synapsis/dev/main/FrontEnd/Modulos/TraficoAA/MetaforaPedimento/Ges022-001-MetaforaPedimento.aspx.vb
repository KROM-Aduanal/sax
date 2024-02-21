
#Region "├┴┘├┴┘├┴┘├┴┘├┴┘|├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘   DEPENDENCIAS   ├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘"

'RECURSOS DEL CMF
Imports gsol
Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Wma.Exceptions
Imports Syn.Operaciones
Imports gsol.Web.Components
Imports Syn.Documento
Imports Syn.Documento.Componentes.Campo
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Nucleo.RecursosComercioExterior.CamposPedimento
Imports Wma.Exceptions.TagWatcher

'UTILERIAS/RECURSOS ADICIONALES
Imports Sax.Web

'OBJETOS DIMENSIONALES (ODS's) Dependencias en MongoDB
Imports Rec.Globals.Controllers
Imports Rec.Globals.Controllers.ControladorRecursosAduanales

'OBJETOS BIDIMENSIONALES (ODF's.  Dependencias Krombase/SQL Server)
Imports Rec.Globals.Utils
Imports Syn.CustomBrokers.Controllers
Imports Syn.CustomBrokers.Controllers.ControladorRecursosAduanales
Imports Rec.Globals
Imports Syn.CustomBrokers.Controllers.reportes

#End Region

Public Class Ges022_001_MetaforaPedimento
    Inherits ControladorBackend

#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████


    Protected Enum TipoFijo
        SinDefinir = 0
        Prefijo = 1
        Sufijo = 2
        Completo = 3
    End Enum

    Protected Enum TipoSecuencia
        SinDefinir = 0
        Referencia = 1
        Pedimento = 2
    End Enum

#End Region


#Region "██████ Vinculación c/capas inf █████████       SAX      ████████████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Public Overrides Sub Inicializa()

        With Buscador

            .DataObject = New ConstructorPedimentoNormal(True)

            .addFilter(SeccionesPedimento.ANS1, CamposPedimento.CA_NUMERO_PEDIMENTO_COMPLETO, "Pedimento")
            .addFilter(SeccionesPedimento.ANS1, CamposPedimento.CP_REFERENCIA, "Referencia")

        End With

        CargaCatalogos()

        AplicarReglasCampoPedimento()

    End Sub

    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher

        ' ** * ** * Generales * ** * **
        [Set](dbcReferenciaPedimento, CP_REFERENCIA, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](dbcReferenciaPedimento, CA_NUMERO_PEDIMENTO_COMPLETO, propiedadDelControl_:=PropiedadesControl.ValueDetail)
        'scTipoReferencia
        'scPrefijoReferencia

        [Set](scPatente, CP_MODALIDAD_ADUANA_PATENTE)
        [Set](scPatente.Value, CA_PATENTE)

        [Set](scEjecutivoCuenta, CP_EJECUTIVO_CUENTA)
        [Set](IIf(swcTipoOperacion.Checked, 1, 0), CA_TIPO_OPERACION, TiposDato.Entero)
        [Set](scClavePedimento, CA_CVE_PEDIMENTO)
        [Set](scRegimen, CA_REGIMEN)
        [Set](scDestinoMercancia, CA_DESTINO_ORIGEN)
        [Set](icTipoCambio, CA_TIPO_CAMBIO)
        [Set](icPesoBruto, CA_PESO_BRUTO)
        'ver lo del valor a 3 digitos
        [Set](scAduanaEntradaSalida, CA_ADUANA_ENTRADA_SALIDA)
        [Set](scAduanaEntradaSalida.Value, CA_CLAVE_SAD)

        [Set](scTransporteEntradaSalida, CA_MEDIO_TRANSPORTE)
        [Set](scMedioTransporteArribo, CA_MEDIO_TRANSPORTE_ARRIBO)
        [Set](scMedioTransporteSalida, CA_MEDIO_TRANSPORTE_SALIDA)
        [Set](icValorDolares, CA_VALOR_DOLARES)
        [Set](icValorAduana, CA_VALOR_ADUANA)
        [Set](icPrecioPagado, CA_PRECIO_PAGADO_VALOR_COMERCIAL)

        If Not String.IsNullOrWhiteSpace(scAduanaEntradaSalida.Value) Then

            [Set](scAduanaEntradaSalida.Value.Substring(0, 2), CA_ADUANA_SIN_SECCION)

        End If

        Dim informacionAgrupacion_ As InformacionAgrupacion = GetVars("_informacionAgrupacion")

        If informacionAgrupacion_ IsNot Nothing Then

            [Set](Convert.ToInt32(informacionAgrupacion_.numerototalpartidas), CA_NUMERO_TOTAL_PARTIDAS, TiposDato.Entero)
            [Set](informacionAgrupacion_.fechageneracionagrupacion, CA_ANIO_VALIDACION)

        End If

        ' ** * ** * Generales * ** * **

        ' ** * ** * Datos del importador/exportador * ** * **
        [Set](fbcCliente, CA_RAZON_SOCIAL_IOE)
        [Set](fbcCliente, CA_RAZON_SOCIAL_IOE, propiedadDelControl_:=PropiedadesControl.Text, asignarA_:=TiposAsignacion.ValorPresentacion)
        [Set](icRFCCliente, CA_RFC_IOE)
        [Set](icCURP, CA_CURP_IOE)
        [Set](icRFCFacturacion, CA_RFC_AA)
        [Set](icDomicilioCliente, CA_DOMICILIO_IOE)
        [Set](icValorSeguros, CA_VALOR_SEGUROS)
        [Set](icSeguros, CA_SEGUROS)
        [Set](icFletes, CA_FLETES)
        [Set](icEmbalajes, CA_EMBALAJES)
        [Set](icOtrosIncrementables, CA_OTROS_INCREMENTABLES)
        [Set](icTransporteDec, CA_TRANSPORTE_DECREMENTABLES)
        [Set](icSegurosDec, CA_SEGURO_DECREMENTABLES)
        [Set](icCargaDec, CA_CARGA_DECREMENTABLES)
        [Set](icDescargaDec, CA_DESCARGA_DECREMENTABLES)
        [Set](icOtrosDec, CA_OTROS_DECREMENTABLES)
        ' ** * ** * Datos del importador/exportador * ** * **

        ' ** * ** * ValidacionPago * ** * **
        [Set](scValidadorDesignado, CA_VALIDADOR_DESIGNADO)
        [Set](scNumeroSemana, CA_NUMERO_SEMANA)
        [Set](icArchivoValidacion, CA_ARCHIVO_VALIDACION)
        [Set](icAcuseValidación, CA_ACUSE_ELECTRONICO_VALIDACION)
        [Set](icArchivoPago, CA_ARCHIVO_PAGO)
        [Set](icAcusetaPago, CA_ACUSE_ELECTRONICO_PAGO)
        [Set](scValidacionAduanaDespacho, CA_CLAVE_SAD)
        [Set](icMarcasNumeros, CA_MARCAS_NUMEROS_TOTAL_BULTOS)
        [Set](icCertificacion, CA_CERTIFICACION)
        [Set](icFechaValidacion, CA_FECHA_VALIDACION)
        ' ** * ** * ValidacionPago * ** * **

        ' ** * ** * Fechas * ** * **
        [Set](icFechaRegistro, CA_FECHA_REGISTRO)
        [Set](icFechaRevalidacion, CA_FECHA_REVALIDACION)
        [Set](icFechaZarpe, CA_FECHA_ZARPE)
        [Set](icFechaPrevio, CA_FECHA_PREVIO)
        [Set](icFechaFondeo, CA_FECHA_FONDEO)
        [Set](icFechaPago, CA_FECHA_PAGO)
        [Set](icFechaAtraque, CA_FECHA_ATRAQUE)
        [Set](icFechaDespacho, CA_FECHA_DESPACHO)
        [Set](icFechaEstimadaArribo, CA_FECHA_ARRIBO)
        [Set](icFechaEntrega, CA_FECHA_ENTREGA)
        [Set](icFechaEntrada, CA_FECHA_ENTRADA)
        [Set](icFechaPresentacion, CA_FECHA_PRESENTACION)
        [Set](icFechaFacturacion, CA_FECHA_FACTURACION)
        ' ** * ** * Fechas * ** * **

        ' ** * ** * TasasContribuciones * ** * **
        [Set](scTasasContribucion, CA_CONTRIBUCION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scTasasTipoTasa, CA_CVE_TIPO_TASA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icTasasTasa, CA_TASA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ccTasas, Nothing, seccion_:=SeccionesPedimento.ANS6)
        ' ** * ** * TasasContribuciones * ** * **

        ' ** * ** * CatalogoCuadroLiquidacion * ** * **
        [Set](scCuadroLiquidacionConcepto, CA_CONCEPTO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCuadroLiquidacionDescripcion, CA_DESCRIPCION_CONCEPTO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scCuadroLiquidacionFP, CA_FORMA_PAGO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCuadroLiquidacionImporte, CA_IMPORTE, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ccCuadroLiquidacion, Nothing, seccion_:=SeccionesPedimento.ANS55)

        [Set](icCuadroLiquidacionEfectivo, CA_EFECTIVO)
        [Set](icCuadroLiquidacionOtros, CA_OTROS)
        [Set](icCuadroLiquidacionTotal, CA_TOTAL)
        ' ** * ** * CatalogoCuadroLiquidacion * ** * **

        ' ** * ** * LineaCaptura * ** * **
        '[Set](ic_LineaCapturaPatente, CA_PATENTE)
        '[Set](ic_LineaCapturaPedimento, CA_NUM_PEDIMENTO)
        '[Set](ic_LineaCapturaAduana, CA_ADUANA_DESPACHO)
        '[Set](ic_LineaCapturaBanco, CA_NOMBRE_INST_BANCARIA)
        '[Set](ic_LineaCapturaNumero, CA_LINEA_CAPTURA)
        '[Set](ic_LineaCapturaImporte, CA_EFECTIVO)
        '[Set](ic_LineaCapturaPago, CA_FECHA_PAGO)

        '[Set](ic_LineaCapturaNumeroOperacion, CA_NUM_OPERACION_BANCARIA)
        '[Set](ic_LineaCapturaNumeroTransaccion, CA_NUM_TRANSACCION_SAT)
        '[Set](ic_LineaCapturaPresentacion, CA_MEDIO_PRESENTACION)
        '[Set](ic_LineaCapturaRecepsion, CA_MEDIO_RECEPCION_COBRO)
        ' ** * ** * LineaCaptura * ** * **

        ' ** * ** * DatosProveedoresImpo * ** * **
        [Set](icIdFiscalProveedor, CA_ID_FISCAL_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](icNombreProveedor, CA_NOMBRE_DEN_RAZON_SOC_POC, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](fbxProveedor, CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_POC, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](fbxProveedor, CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_POC, propiedadDelControl_:=PropiedadesControl.Ninguno, asignarA_:=TiposAsignacion.ValorPresentacion)
        [Set](icDocimilioProveedor, CA_DOMICILIO_POC, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](icFacturaProveedor, CA_CFDI_FACTURA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scFacturaProveedor, CA_CFDI_FACTURA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icFechaFacturaProveedor, CA_FECHA_FACTURA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icIncontermProveedor, CA_INCOTERM, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scFactorMonedaProveedor, CA_FACTOR_MONEDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icMontoFacturaProveedor, CA_MONTO_MONEDA_FACTURA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icMontoFacturaUSDProveedor, CA_MONTO_USD, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ccFacturas, Nothing, seccion_:=SeccionesPedimento.ANS13, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](pbcProveedores, Nothing, seccion_:=SeccionesPedimento.ANS10)
        ' ** * ** * DatosProveedoresImpo * ** * **

        ' ** * ** * Destinatarios * ** * **
        [Set](icTaxtIDDestinatario, CA_ID_FISCAL_DESTINATARIO)
        [Set](scRazonSocialDestinatario, CA_NOMBRE_RAZON_SOCIAL_DESTINATARIO)
        [Set](icDomicilioDestinatario, CA_DOMICILIO_DESTINATARIO)
        ' ** * ** * Destinatarios * ** * **

        ' ** * ** * DatosTransporte * ** * **
        '[Set](ic_IDTransporte, CA_ID_TRANSPORT, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](sc_PaisTransporte, CA_CVE_PAIS_TRANSP, propiedadDelControl_:=PropiedadesControl.Ninguno)

        ' **** El catalogo dentro de un componente pillbox no se a probado
        '[Set](ic_NumeroCandado, CA_NUM_CANDADO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](ic_CandadoPrimeraRevisión, CA_NUM_CANDADO_1RA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](ic_CandadoSegundaRevision, CA_NUM_CANDADO_2DA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](CatalogoCandados, Nothing, seccion_:=SeccionesPedimento.ANS15)

        '[Set](pb_DatosTransporte, Nothing, seccion_:=SeccionesPedimento.ANS12)
        ' ** * ** * DatosTransporte * ** * **

        ' ** * ** * Guias * ** * **
        [Set](icGuia, CA_GUIA_MANIFIESTO_BL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](swcTipoGuia, CA_MASTER_HOUSE, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ccGuias, Nothing, seccion_:=SeccionesPedimento.ANS16)
        ' ** * ** * Guias * ** * **

        ' ** * ** * Contenedores * ** * **
        [Set](icNumeroContenedor, CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scTipoContenedor, CA_CVE_TIPO_CONTENEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ccContenedores, Nothing, seccion_:=SeccionesPedimento.ANS17)
        ' ** * ** * Contenedores * ** * **

        ' ** * ** * Identificadores * ** * **
        [Set](scIdentificadorPedimento, CA_CVE_IDENTIFICADOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icComplemento1Pedimento, CA_COMPLEMENTO_1, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icComplemento2Pedimento, CA_COMPLEMENTO_2, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icComplemento3Pedimento, CA_COMPLEMENTO_3, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ccIdentificadores, Nothing, seccion_:=SeccionesPedimento.ANS18)
        ' ** * ** * Identificadores * ** * **

        ' ** * ** * CuentasAduaneras * ** * **
        [Set](scCuentaAduanera, CA_CVE_CUENTA_ADUANERA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scTipoCuentaAduanera, CA_CVE_TIPO_GARANTIA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scInstitucionEmisora, CA_INSTITUCION_EMISORA_GARANTIA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icNumeroCOntratoCuentaAduanera, CA_NUMERO_CONTRATO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icFolioConstanciaCuentaAduanera, CA_FOLIO_CONSTANCIA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icImporteCuentaAduanera, CA_IMPORTE_TOTAL_CONSTANCIA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icFechaEmisionCuentaAduanera, CA_FECHA_EMISION_CONSTANCIA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPrecioEstimadoCuentaAduanera, CA_CANTIDAD_UMT_PRECIO_ESTIMADO_PEDIMENTO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icTitulosCuentaAduanera, CA_TITULOS_ASIGNADOS_PEDIMENTO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icValorUnitario, CA_VALOR_UNITARIO_TITULO_PEDIMENTO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ccCuentasAduaneras, Nothing, seccion_:=SeccionesPedimento.ANS19)
        ' ** * ** * CuentasAduaneras * ** * **

        ' ** * ** * Pagosvirtuales * ** * **
        [Set](scPagosVirtualesFormaPago, CA_PAGOS_VIRTUALES_FORMA_PAGO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scPagosVIrtualesEmisora, CA_NOMBRE_INSTITUCION_EMISORA_DOCUMENTO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPagosVirtualesDocumento, CA_NUMERO_DOCUMENTO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPagosVirtualesFechaDocumento, CA_FECHA_EXPOCICION_DOCUMENTO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPagosVirtualesImporteDocumento, CA_IMPORTE_DOCUMENTO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPagosVirtualesSaldo, CA_SALDO_DISPONIBLE_DOCUMENTO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPagosVirtualesImportePedimento, CA_IMPORTE_PAGADO_PEDIMENTO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ccPagosvirtuales, Nothing, seccion_:=SeccionesPedimento.ANS22)
        ' ** * ** * Pagosvirtuales * ** * **

        '' Estas se ejecutaran dependiendo el constructor del pedimento

        '' ** * ** * Rectificaciones * ** * **
        '[Set](ic_RectificacionesFechaPedOriginal, CA_RECTIFICACION_FECHA_PEDIM_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](sc_RectificacionesClavePedOriginal, CA_RECTIFICACION_CVE_PEDIM_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](ic_RectificacionesPatentePedOriginal, CA_RECTIFICACION_PATENTE_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](ic_RectificacionesPedimentoCompleto, CA_RECTIFICACION_NUM_PEDIMENTO_ORIGINAL_7_DIGITOS, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](ic_RectificacionesAñoValidacion2PedOriginal, CA_RECTIFICACION_AÑO_VALIDACION_2_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](ic_RectificacionesAñoValidacionPedOriginal, CA_RECTIFICACION_AÑO_VALIDACION_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](sc_RectificacionesAduanaOriginal, CA_RECTIFICACION_ADUANA_DESPACHO_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](sc_RectificacionesAduana2Original, CA_RECTIFICACION_ADUANA_DESPACHO_ORIGINAL_2, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](sc_RectificacionesClavePedimento, CA_CVE_PEDIM_RECTIF, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](ic_RectificacionesFechaPedimento, CA_FECHA_PAG_PEDIM_RECTIFICACION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](CatalogoRectificaciones, Nothing, seccion_:=SeccionesPedimento.ANS37)
        '' ** * ** * Rectificaciones * ** * **

        '' ** * ** * DiferenciaContribuciones * ** * **
        '[Set](sc_DifConConcepto, CA_CONCEPTO_DIF_CONTRIB, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](sc_DifConFormaPago, CA_FORMA_PAGO_DIFERENCIAS, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](ic_DifConDiferencia, CA_DIFERENCIA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](ic_DifConEfectivo, CA_DIFERENCIA_EFECTIVO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](ic_DifConOtros, CA_DIFERENCIA_OTROS, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](ic_DifConTotal, CA_DIFERENCIA_TOTAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](CatalogoDiferenciasRecti, Nothing, seccion_:=SeccionesPedimento.ANS38)
        '' ** * ** * DiferenciaContribuciones * ** * **

        '' ** * ** * PruebaSuficiente * ** * **
        '[Set](sc_PruebaSuficientePaisDestino, CA_PRUEBASUFICIENTE_CVE_PAIS_EXPORT, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](ic_PruebaSuficientePedEUACAN, CA_NUM_DOCTO_IMP_EU_O_CAN, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](sc_PruebaSuficiente, CA_CVE_PRUEBA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](CatalogoPruebaSuficiente, Nothing, seccion_:=SeccionesPedimento.ANS39)
        '' ** * ** * PruebaSuficiente * ** * **

        '' Estas se ejecutaran dependiendo el constructor del pedimento

        ' ** * ** * CatalogoDescargos * ** * **
        [Set](icDescargosPedCompletoOriginal, CA_NUMERO_PEDIMENTO_ORIGINAL_COMPLETO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icDescargosFechaPedOriginal, CA_FECHA_PEDIMENTO_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scDescargosClavePedOriginal, CA_CVE_PEDIMENTO_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icDescargosValidacionOriginal, CA_ANIO_VALIDACION_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icDescargosValidacion2Original, CA_ANIO_VALIDACION_2_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scDescargosPatenteOriginal, CA_PATENTE_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scDescargosAduanaOriginal, CA_ADUANA_DESPACHO_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scDescargosAduana2Original, CA_ADUANA_DESPACHO_ORIGINAL_2, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icDescargosPedOriginal, CA_NUMERO_PEDIMENTO_ORIGINAL_7_DIGITOS, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icDescargosFraccionOriginal, CA_FRACCION_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icDescargosUMOriginal, CA_UM_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icDescargosUMDescargo, CA_CANTIDAD_MERCANCIA_UMT_DESCARGO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ccDescargos, Nothing, seccion_:=SeccionesPedimento.ANS20)
        ' ** * ** * CatalogoDescargos * ** * **

        ' ** * ** * Compensaciones * ** * **
        [Set](scCompensacionesContribucion, CA_COMPENSACION_CONTRIBUCION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCompensacionesPedCompletoOriginal, CA_COMPENSACION_NUMERO_PEDIMENTO_ORIGINAL_COMPLETO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCompensacionesFechaPagoPedOriginal, CA_FECHA_PAGO_ORIGINAL_PARA_COMPENSACION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCompensacionesGravamen, CA_IMPORTE_GRAVAMEN_COMPENSACION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scCompensacionesConcepto, CA_CVE_CONCEPTO_COMPENSACION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCompensacionesAñoValidacionOriginal, CA_COMPENSACION_ANIO_VALIDACION_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCompensacionesAñoValidacion2Original, CA_COMPENSACION_ANIO_VALIDACION_2_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scCompensacionesPatenteOriginal, CA_COMPENSACION_PATENTE_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scCompensacionesAduanaOriginal, CA_COMPENSACION_ADUANA_DESPACHO_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scCompensacionesAduana2Original, CA_COMPENSACION_ADUANA_DESPACHO_ORIGINAL_2, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCompensacionesPedOriginal, CA_COMPENSACION_NUMERO_PEDIMENTO_ORIGINAL_7_DIGITOS, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ccCompensaciones, Nothing, seccion_:=SeccionesPedimento.ANS21)
        ' ** * ** * Compensaciones * ** * **

        ' ** * ** * Observaciones * ** * **
        [Set](icOnservacionesPedimento, CA_OBSERVACIONES_PEDIMENTO)
        ' ** * ** * Observaciones * ** * **

        ' ** * ** * Partidas * ** * **

        [Set](icFraccionArancelaria, CA_FRACCION_ARANCELARIA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icNico, CA_NICO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](sc_PartidaVinculacion, CA_PARTIDAS_VINCULACION)
        [Set](scPartidaMetodoValoracion, CA_CVE_METODO_VALORACION_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scUMC, CA_CVE_UMC_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCantidadUMC, CA_CANTIDAD_UMC_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scUMT, CA_CVE_UMT_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icCantidadUMT, CA_CANTIDAD_UMT_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scPaisVendedor, CA_CVE_PAIS_VENDEDOR_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scPaisComprador, CA_CVE_PAIS_COMPRADOR_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scPaisOrigen, CA_CVE_PAIS_ORIGEN_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scPaisDestino, CA_CVE_PAIS_DESTINO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icPartidaValorAduana, CA_VALOR_ADUANA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPartidaPrecioPagado, CA_PRECIO_PAGADO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPartidaValorComercial, CA_VALOR_COMERCIAL_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPartidaPrecioUnitario, CA_PRECIO_UNITARIO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPartidaValorAgregado, CA_VALOR_AGREGADO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPartidaMarca, CA_MARCA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPartidaModelo, CA_MODELO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](scPartidaVinculacion, CA_CVE_VINCULACION_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        '[Set](icPartidaCodigoProducto, CA_CODIGO_PRODUCTO_PARTIDA)

        [Set](icPartidaDescripcion, CA_DESCRIPCION_MERCANCIA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scPartidaContribucion, CA_CVE_CONTRIBUCION_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scPartidaTipoTasa, CA_CVE_TIPO_TASA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPartidaTasa, CA_TASA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scFormaPago, CA_FORMA_PAGO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icImporte, CA_IMPORTE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ccPartidaTasas, Nothing, seccion_:=SeccionesPedimento.ANS29, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scClavePermiso, CA_CVE_PERMISO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPermisoNom, CA_NUMERO_PERMISO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icFirmaDescargo, CA_FIRMA_ELECTRONICA_PERMISO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icValorComercialDLS, CA_VALOR_USD_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCantidadUMTC, CA_CANTIDAD_UMT_UMC, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ccPartidasPermisos, Nothing, seccion_:=SeccionesPedimento.ANS26, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scPartidaIdentificador, CA_CVE_IDENTIFICADOR_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPartidaComplemento1, CA_COMPLEMENTO_1_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPartidaComplemento2, CA_COMPLEMENTO_2_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPartidaComplemento3, CA_COMPLEMENTO_3_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ccPartidasIdentificadores, Nothing, seccion_:=SeccionesPedimento.ANS27, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icPartidaObservacion, CA_OBSERVACIONES_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](pbcPartidas, Nothing, seccion_:=SeccionesPedimento.ANS24)


        ' ** * ** * Partidas * ** * **

        Return New TagWatcher(1)

    End Function

    Public Overrides Sub BotoneraClicNuevo()

        If OperacionGenerica IsNot Nothing Then

            '_empresa = Nothing

        End If

        PreparaControles()

        PreparaTarjetero(PillboxControl.ToolbarModality.Simple, pbcPartidas)


    End Sub

    Public Overrides Sub BotoneraClicGuardar()

        'ProcesarOperacion(Of Something)()
        If Not ProcesarTransaccion(Of ConstructorPedimentoNormal)().Status = TypeStatus.Errors Then : End If

    End Sub

    Public Overrides Sub BotoneraClicEditar()

        PreparaTarjetero(PillboxControl.ToolbarModality.Advanced, pbcPartidas)

    End Sub

    Public Overrides Sub BotoneraClicBorrar()


    End Sub

    Public Overrides Sub BotoneraClicOtros(ByVal IndexSelected_ As Integer)

        If IndexSelected_ = 8 Then

            ImprimirPedimentoNormal(dbcReferenciaPedimento.Value)

        End If

        If IndexSelected_ = 10 Then

            'pruebas 
            'MANDAR UNA LISTA
            'RKU23 - 402
            Dim listaObjectID = New List(Of ObjectId) From {
                New ObjectId("658492a31e051f4771122bd8")
            }
            Dim factura_ As IControladorFacturaComercial = New ControladorFacturaComercial(listaObjectID, IControladorFacturaComercial.Modalidades.Externo, 1)
            Dim listafactura_ = New List(Of DocumentoElectronico) From {
                factura_.FacturasComerciales(0)
            }

            Dim documentoElectronico_ = OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

            Dim generador_ As IGeneradorPartidasPedimento = New GeneradorPartidasPedimento(Statements.GetOfficeOnline()._id, listafactura_, documentoElectronico_, OperacionGenerica.Id)
            Dim lista_ As List(Of IGeneradorPartidasPedimento.TipoAgrupaciones) = generador_.GeneraOpcionesAgrupacion(generador_.ItemsFacturaComercial)

            Dim agrupaciones_ = generador_.AgruparItemsFacturaPor(lista_(0), generador_.ItemsFacturaComercial)

            SetVars("_informacionAgrupacion", generador_.InformacionAgrupacion)

            pbcPartidas.ClearRows()

            Dim i = 1

            For Each agrupacion_ As PartidaPedimento In agrupaciones_.ObjectReturned

                pbcPartidas.SetPillbox(Sub(ByVal pillbox_ As PillBox)

                                           pillbox_.SetIndice(pbcPartidas.KeyField, IIf(i = 1, i, 0))
                                           pillbox_.SetIdentity(i)
                                           pillbox_.SetControlValue(icFraccionArancelaria, New SelectOption With {.Value = agrupacion_.FraccionArancelaria, .Text = agrupacion_.DescripcionFraccionArancelaria})
                                           pillbox_.SetControlValue(icNico, New SelectOption With {.Value = agrupacion_.Nico, .Text = agrupacion_.DescripcionNico})
                                           pillbox_.SetControlValue(icPartidaPrecioUnitario, Convert.ToInt64(agrupacion_.PrecioUnitario))
                                           pillbox_.SetControlValue(scPartidaMetodoValoracion, New SelectOption With {.Value = agrupacion_.MetodoValoracion, .Text = agrupacion_.DescripcionMetodoValoracion})
                                           pillbox_.SetControlValue(icCantidadUMC, agrupacion_.CantidadUMC)
                                           pillbox_.SetControlValue(scUMC, New SelectOption With {.Value = agrupacion_.UnidadMedidaComercial, .Text = agrupacion_.DescripcionUnidadMedidaComercial})
                                           pillbox_.SetControlValue(icCantidadUMT, agrupacion_.CantidadUMT)
                                           pillbox_.SetControlValue(scUMT, New SelectOption With {.Value = agrupacion_.UnidadMedidaTarifa, .Text = agrupacion_.DescripcionUnidadMedidaTarifa})
                                           pillbox_.SetControlValue(icPartidaValorAduana, Convert.ToInt64(agrupacion_.ValorAduanal))
                                           pillbox_.SetControlValue(icPartidaValorUSd, Convert.ToInt64(agrupacion_.ValorDolares))
                                           pillbox_.SetControlValue(icPartidaPrecioPagado, Convert.ToInt64(agrupacion_.ImportePrecioPagado))
                                           pillbox_.SetControlValue(icPartidaValorComercial, agrupacion_.ValorComercial)
                                           pillbox_.SetControlValue(scPaisVendedor, New SelectOption With {.Value = agrupacion_.PaisVendedor, .Text = agrupacion_.DescripcionPaisVendedor})
                                           pillbox_.SetControlValue(scPaisComprador, New SelectOption With {.Value = agrupacion_.PaisComprador, .Text = agrupacion_.DescripcionPaisComprador})
                                           pillbox_.SetControlValue(scPaisOrigen, New SelectOption With {.Value = agrupacion_.PaisOrigen, .Text = agrupacion_.DescripcionPaisOrigen})
                                           pillbox_.SetControlValue(scPaisDestino, New SelectOption With {.Value = agrupacion_.PaisDestino, .Text = agrupacion_.DescripcionPaisDestino})
                                           pillbox_.SetControlValue(icPartidaMarca, agrupacion_.Marca)
                                           pillbox_.SetControlValue(icPartidaModelo, agrupacion_.Modelo)
                                           pillbox_.SetControlValue(scPartidaVinculacion, New SelectOption With {.Value = agrupacion_.Vinculacion, .Text = agrupacion_.DescripcionVinculacion})
                                           pillbox_.SetControlValue(icPartidaValorAgregado, agrupacion_.ValorAgregado)
                                           pillbox_.SetControlValue(icPartidaDescripcion, agrupacion_.Descripcion)
                                           pillbox_.SetControlValue(icPartidaObservacion, agrupacion_.Observaciones)

                                       End Sub)

                i += 1

            Next

            pbcPartidas.PillBoxDataBinding()

        End If

    End Sub

    'EVENTOS PARA LA INSERCIÓN DE DATOS
    Public Overrides Sub RealizarInsercion(ByRef documentoElectronico_ As DocumentoElectronico)

        With documentoElectronico_

            .FolioOperacion = dbcReferenciaPedimento.Value

            .FolioDocumento = dbcReferenciaPedimento.ValueDetail

            .IdCliente = 0

            .NombreCliente = fbcCliente.Text

        End With

        'LimpiaFormatos()

    End Sub

    'EVENTOS PARA MODIFICACIÓN DE DATOS

    Public Overrides Function AntesRealizarModificacion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As TagWatcher

        '     ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒

        If session_ IsNot Nothing Then

            '  ██████inicio███████        Logica de negocios local      ████████████████████████

            If GetVars("_informacionAgrupacion") IsNot Nothing Then

                Dim generador_ As IGeneradorPartidasPedimento = New GeneradorPartidasPedimento(Statements.GetOfficeOnline()._id, session_)

                tagwatcher_ = generador_.GuardarInformacionAgrupaciones(session_, GetVars("_informacionAgrupacion"))

            Else

                tagwatcher_ = New TagWatcher

                tagwatcher_.SetOK()

            End If

        Else  '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas sin transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()

        End If

        Return tagwatcher_

    End Function

    Public Overrides Sub RealizarModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

        'LimpiaFormatos()

    End Sub

    'EVENTOS PARA PRESENTACIÓN DE DATOS EN FRONTEND
    Public Overrides Sub PreparaModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

        With documentoElectronico_

            If .Attribute(CamposPedimento.CA_TIPO_OPERACION).Valor = 1 Then

                swcTipoOperacion.Checked = True

            Else

                swcTipoOperacion.Checked = False

            End If

        End With

    End Sub

    Public Overrides Sub DespuesBuquedaGeneralConDatos()


        PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbcPartidas)

    End Sub

    Public Overrides Sub DespuesBuquedaGeneralSinDatos()

        PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbcPartidas)


    End Sub

    'EVENTOS DE MANTENIMIENTO
    Public Overrides Sub LimpiaSesion()

        'Lógica para limpiar sesiones que sean necearías

        'SetVars("ConstructorTarjetas", Nothing)

    End Sub

    Public Overrides Sub Limpiar()

        ccTasas.DataSource = Nothing
        ccCuadroLiquidacion.DataSource = Nothing
        ccGuias.DataSource = Nothing

    End Sub


#End Region

#Region "████████████████  QUINTA CAPA  █████████       Reglas locales         ██████████████████████████████"
    '    ██                                                                                                ██
    '    ██    Pendientes                                                                                  ██
    '    ██      1.                                                                                        ██
    '    ██      2.                                                                                        ██
    '    ██      3.                                                                                        ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Protected Sub sc_TipoReferencia_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub sc_PrefijoReferencia_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub fbc_Cliente_TextChanged(sender As Object, e As EventArgs)

        Dim controlador_ As New ControladorBusqueda(Of ConstructorCliente)

        Dim lista_ As List(Of SelectOption) = controlador_.Buscar(fbcCliente.Text, New Filtro With {.IdSeccion = SeccionesClientes.SCS1, .IdCampo = CamposClientes.CA_RAZON_SOCIAL})

        fbcCliente.DataSource = lista_

    End Sub



#End Region

#Region "██████ Vinculación sexta capa  █████████       SAX      ████████████████████████████████████████████"
    '    ██████   Controladores utilizados                     Documentos por coding para MongoDB      ██████
    '    ██████    1.ControladorClientes                        1. En Empresa:                         ██████
    '    ██████    2.ControladorRecursosAduanales                  a). Domicilios                      ██████
    '    ██████    3.ControladorSecuencias                         b). Contactos                       ██████
    '    ██████                                                                                        ██████
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Protected Sub fbc_Cliente_Click(sender As Object, e As EventArgs)

        Dim controlador_ As New ControladorBusqueda(Of ConstructorCliente)

        Dim TagWatcher_ = controlador_.ObtenerDocumento(fbcCliente.Value)

        If TagWatcher_.ObjectReturned IsNot Nothing Then

            Dim documentoCliente_ = TagWatcher_.ObjectReturned

            CargaCliente(documentoCliente_)

        End If

    End Sub

    Protected Sub sc_Patente_Click(sender As Object, e As EventArgs)

        scPatente.DataSource = CargaPatente()

    End Sub

    Public Function ConsultaPedimento(ByVal FolioOper_ As String) As DocumentoElectronico

        Using enlaceDatos_ As IEnlaceDatos =
            New EnlaceDatos With {.EspacioTrabajo = Session("EspacioTrabajoExtranet")}

            'enlaceDatos_.EspacioTrabajo.DivisionEmpresarial =

            Dim operacionesDB_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)(rootid_:=1)

            Dim filtro_ As BsonDocument = New BsonDocument().Add("FolioOperacion", FolioOper_)

            Dim resultadoDocumentos_ = operacionesDB_.Find(filtro_).ToList

            If resultadoDocumentos_.Count Then

                Dim operacionGenerica_ As OperacionGenerica = resultadoDocumentos_(0)

                Return operacionGenerica_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

            Else

                Return Nothing

            End If

        End Using

    End Function

    Public Sub ImprimirPedimentoNormal(ByVal FolioOperacion_ As String)

        Dim docElectronico_ As DocumentoElectronico = ConsultaPedimento(FolioOperacion_)

        If docElectronico_ IsNot Nothing Then

            Dim constructorPedimento As New RepresentacionPedimento(docElectronico_)

            Dim pdfstring = "data:Application/pdf;base64, " & constructorPedimento.ImprimirPedimentoNormal()

            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "openPDF('" & pdfstring & "','" & FolioOperacion_ & "')", True)

            'Dim iframe = "<iframe src='" & pdfstring & "' name='" & FolioOperacion_ & ".pdf' frameborder='0' style='border:0; top:0px; left:0px; bottom:0px; right:0px; width:100%; height:100%;position:absolute;' allowfullscreen></iframe>"

            'Dim func = "win = window.open() win.document.write(""" & iframe & """) win.document.title=""" & FolioOperacion_ & """"

            'ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", func, True)
            ' HttpUtility.UrlEncode(pdfstring)

        End If

    End Sub

    Private Sub CargaCatalogos()

        scClavePedimento.DataEntity = New Anexo22

        scRegimen.DataEntity = New Anexo22

        scDestinoMercancia.DataEntity = New Anexo22

        scAduanaEntradaSalida.DataEntity = New Anexo22

        scTransporteEntradaSalida.DataEntity = New Anexo22

        scMedioTransporteArribo.DataEntity = New Anexo22

        scMedioTransporteSalida.DataEntity = New Anexo22

        scTasasContribucion.DataEntity = New Anexo22

        scTasasTipoTasa.DataEntity = New Anexo22

        scCuadroLiquidacionConcepto.DataEntity = New Anexo22

        scCuadroLiquidacionFP.DataEntity = New Anexo22

        'scPaisTransporte.DataEntity = New Anexo22

        scTipoContenedor.DataEntity = New Anexo22

        scIdentificadorPedimento.DataEntity = New Anexo22

        scPagosVirtualesFormaPago.DataEntity = New Anexo22

        scRectificacionesClavePedOriginal.DataEntity = New Anexo22

        scRectificacionesAduanaOriginal.DataEntity = New Anexo22

        scRectificacionesAduana2Original.DataEntity = New Anexo22

        scRectificacionesClavePedimento.DataEntity = New Anexo22

        scDifConConcepto.DataEntity = New Anexo22

        scDifConFormaPago.DataEntity = New Anexo22

        scDescargosClavePedOriginal.DataEntity = New Anexo22

        scDescargosAduanaOriginal.DataEntity = New Anexo22

        scDescargosAduana2Original.DataEntity = New Anexo22

        scCompensacionesContribucion.DataEntity = New Anexo22

        scCompensacionesAduanaOriginal.DataEntity = New Anexo22

        scCompensacionesAduana2Original.DataEntity = New Anexo22

        scPruebaSuficientePaisDestino.DataEntity = New Anexo22

        scPartidaMetodoValoracion.DataEntity = New Anexo22

        scUMC.DataEntity = New Anexo22

        scUMT.DataEntity = New Anexo22

        scPaisVendedor.DataEntity = New Anexo22

        scPaisOrigen.DataEntity = New Anexo22

        scPaisComprador.DataEntity = New Anexo22

        scPaisDestino.DataEntity = New Anexo22

        scFormaPago.DataEntity = New Anexo22

        scClavePermiso.DataEntity = New Anexo22

        scPartidaContribucion.DataEntity = New Anexo22

        scPartidaTipoTasa.DataEntity = New Anexo22

        scPartidaIdentificador.DataEntity = New Anexo22

        scEjecutivoCuenta.DataEntity = New krom.Ejecutivos

        scEjecutivoCuenta.FreeClauses = " and i_Cve_DivisionMiEmpresa = " & Statements.GetOfficeOnline()._id

    End Sub

    Private Sub PreparaControles()

        'Inicializa Tipo de Referencia
        scTipoReferencia.Value = 1

        'Inicizliza prefijo
        InicializaPrefijo(scTipoReferencia, scPrefijoReferencia)

        'GeneraPrefijoReferencia
        dbcReferenciaPedimento.Value = GeneraReferenciaPedimento(TipoSecuencia.Referencia, TipoFijo.Completo, scPrefijoReferencia)


    End Sub

    Private Sub CargaCliente(ByVal datosCliente_ As OperacionGenerica)

        icRFCCliente.Value = datosCliente_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Campo(CamposClientes.CA_RFC_CLIENTE).Valor

        icCURP.Value = datosCliente_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Campo(CamposClientes.CA_CURP_CLIENTE).Valor


        icRFCFacturacion.Value = datosCliente_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Campo(CamposClientes.CA_RFC_CLIENTE).Valor

        icDomicilioCliente.Value = datosCliente_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor

    End Sub

    Public Function CargaPatente() As List(Of SelectOption)

        Dim recursos_ As ControladorRecursosAduanales = BuscarRecursosAduanales(ControladorRecursosAduanales.TiposRecurso.Generales)

        Dim patentes_ = From data In recursos_.patentes
                        Where data.archivado = False And data.estado = 1
                        Select data._idpatente, data.agenteaduanal

        If patentes_.Count > 0 Then

            Dim soPatentes_ As New List(Of SelectOption)

            For index_ As Int32 = 0 To patentes_.Count - 1

                soPatentes_.Add(New SelectOption With
                             {.Value = patentes_(index_)._idpatente,
                              .Text = patentes_(index_)._idpatente.ToString & " - " & patentes_(index_).agenteaduanal})

            Next

            Return soPatentes_

        End If

        Return Nothing

    End Function

    Protected Sub icFraccionArancelaria_TextChanged(sender As Object, e As EventArgs)

        Dim controlador_ = New ControladorTIGIE()

        Dim tagwacher_ = controlador_.EnlistarFracciones(icFraccionArancelaria.Text)

        If tagwacher_.Status = TypeStatus.Ok Then

            Dim fracciones_ As List(Of FraccionArancelaria) = tagwacher_.ObjectReturned

            Dim fraccionesData_ = New List(Of SelectOption)

            fracciones_.ForEach(Sub(ByVal fraccion_ As FraccionArancelaria) fraccionesData_.Add(New SelectOption With {.Value = fraccion_.Fraccion, .Text = fraccion_.Fraccion & " | " & fraccion_.DescripcionFraccion}))

            icFraccionArancelaria.DataSource = fraccionesData_

        End If

    End Sub

    Protected Sub icFraccionArancelaria_Click(sender As Object, e As EventArgs)

        Dim controlador_ = New ControladorTIGIE()

        Dim tagwacher_ = controlador_.EnlistarNicosFraccion(icFraccionArancelaria.Value)

        If tagwacher_.Status = TypeStatus.Ok Then

            Dim nicos_ As List(Of NicoFraccionArancelaria) = tagwacher_.ObjectReturned

            Dim nicosData_ = New List(Of SelectOption)

            nicos_.ForEach(Sub(ByVal nico_ As NicoFraccionArancelaria) nicosData_.Add(New SelectOption With {.Value = nico_.Nico, .Text = nico_.Nico & " | " & nico_.DescripcionNico}))

            icNico.DataSource = nicosData_

        End If

    End Sub

    Protected Sub pbcPartidas_CheckedChange(sender As Object, e As EventArgs)

        lbSecuencia.Text = pbcPartidas.PageIndex.ToString()

    End Sub

    Protected Sub pbcPartidas_Click(sender As Object, e As EventArgs)

        lbSecuencia.Text = pbcPartidas.PageIndex.ToString()

    End Sub


    Protected Sub swcTipoOperacion_CheckedChanged(sender As Object, e As EventArgs)

        AplicarReglasCampoPedimento()

    End Sub



    Protected Sub InicializaPrefijo(ByVal tipoReferencia_ As SelectControl, ByVal prefijo_ As SelectControl)

        Dim tipoPrefijo_ As Int16

        Select Case tipoReferencia_.Value

            Case ControladorRecursosAduanales.TiposReferenciasOperativas.Operativas

                tipoPrefijo_ = ControladorRecursosAduanales.TiposPrefijosEnviroment.ReferenciaOperativaNormal

            Case ControladorRecursosAduanales.TiposReferenciasOperativas.Corresponsalias

                tipoPrefijo_ = ControladorRecursosAduanales.TiposPrefijosEnviroment.ReferenciaOperativaCorresponsalia

            Case ControladorRecursosAduanales.TiposReferenciasOperativas.CorresponsaliasTerceros

                tipoPrefijo_ = ControladorRecursosAduanales.TiposPrefijosEnviroment.ReferenciaOperativaCorresponsaliasTerceros

            Case Else

                tipoPrefijo_ = ControladorRecursosAduanales.TiposPrefijosEnviroment.SinDefinir

        End Select

        If tipoPrefijo_ = ControladorRecursosAduanales.TiposReferenciasOperativas.SinDefinir Then

            prefijo_.DataSource = Nothing

        Else

            Dim prefijodefault_ As Int16 = 0

            prefijo_.DataSource = PrefijosReferencia(tipoPrefijo_, prefijodefault_)

            If prefijo_.DataSource IsNot Nothing And prefijodefault_ <> 0 Then

                'Se selecciona el primer elemento que esté por default
                prefijo_.Value = prefijodefault_

            End If

        End If

    End Sub

    Private Function PrefijosReferencia(ByVal tipoPrefijo_ As TiposPrefijosEnviroment, Optional ByRef idprefijoDefault_ As Int16 = 0) As List(Of SelectOption)

        Dim recursos_ As ControladorRecursosAduanales = BuscarRecursosAduanales(ControladorRecursosAduanales.TiposRecurso.Generales)

        Dim prefijos_ = From enviroment In recursos_.prefijosenviroment'.AsQueryable
                        Where enviroment._idenviroment = Statements.GetOfficeOnline._id
                        From prefix In enviroment.prefijosoperativos
                        Where prefix._idtipoprefijo = tipoPrefijo_
                        Select prefix.prefijo, prefix._idprefijo, prefix.default

        If prefijos_.Count > 0 Then

            Dim infoprefijolocal_ As New List(Of SelectOption)
            Dim primerdefault_ As Boolean = True

            For Each dato In prefijos_

                If primerdefault_ And dato.default Then

                    idprefijoDefault_ = dato._idprefijo

                    primerdefault_ = False

                End If

                infoprefijolocal_.Add(New SelectOption With {.Value = dato._idprefijo, .Text = dato.prefijo})

            Next

            Return infoprefijolocal_

        End If

        Return Nothing

    End Function

    Protected Function GeneraReferenciaPedimento(ByVal tipoSecuencia_ As TipoSecuencia,
                                                 ByVal tipoFijo_ As TipoFijo,
                                                 ByVal inputPrefijo_ As SelectControl) As String

        Dim secuencia_ As New Secuencia

        Select Case tipoSecuencia_

            Case TipoSecuencia.Referencia

                Select Case tipoFijo_

                    Case 1 'Prefijo

                        Return inputPrefijo_.Text & Mid(Year(Now).ToString, 3, 2) & "-"

                    Case 2 'Sufijo


                    Case 3 'Completo

                        Return inputPrefijo_.Text &
                                Mid(Year(Now).ToString, 3, 2) & "-" &
                                secuencia_.GeneraSecuencia("Referencias", Statements.GetOfficeOnline._id, Year(Now), 0, 0, 0, inputPrefijo_.Value).ToString.PadLeft(8, "0")

                End Select

            Case TipoSecuencia.Pedimento

                Select Case tipoFijo_

                    Case 1 'Prefijo

                    Case 2 'Sufijo

                    Case 3 'Completo

                End Select

            Case Else

        End Select

        Return Nothing

    End Function

    Protected Sub icFecha_TextChanged(sender As Object, e As EventArgs)

        Dim _icontroladorMonedas = New ControladorMonedas

        Dim factorTipoCambio_ = _icontroladorMonedas.ObtenerFactorTipodeCambio("MXN", Date.Parse(sender.Value))

        If factorTipoCambio_.Status = TypeStatus.Ok Then

            Dim tcvalor As tipodecambioreciente = factorTipoCambio_.ObjectReturned(1)

            icTipoCambio.Value = tcvalor.tipocambio

        End If

    End Sub

    Protected Sub ccFacturas_RowChanged(row_ As Object, e As EventArgs)

        Dim facturasInfo_ As List(Of Dictionary(Of String, String)) = GetVars("facturasInfo_")

        Dim id_ As String = row_.Item("scFacturaProveedor").Item("Value")

        Dim factura_ = facturasInfo_.Where(Function(f) f.ContainsKey("id") AndAlso f.Item("id").ToString = id_).ToList().First

        row_.Item(icFechaFacturaProveedor.ID) = factura_.Item("fechaFactura")
        row_.Item(icIncontermProveedor.ID) = factura_.Item("incoterm")
        row_.Item(scFactorMonedaProveedor.ID) = New Dictionary(Of String, String) From {{"Value", factura_.Item("monedaValor")}, {"Text", factura_.Item("monedaValorPresentacion")}}
        row_.Item(icMontoFacturaProveedor.ID) = factura_.Item("valorFactura")
        row_.Item(icMontoFacturaUSDProveedor.ID) = factura_.Item("valorMercancia")

    End Sub

    Protected Sub scClavePedimento_SelectedIndexChanged(sender As Object, e As EventArgs)

        AplicarReglasCampoPedimento()

    End Sub

    Protected Sub fbxProveedor_TextChanged(sender As Object, e As EventArgs)

        Dim controlador_ As New ControladorBusqueda(Of ConstructorProveedoresOperativos)

        Dim lista_ As List(Of SelectOption) = controlador_.Buscar(fbxProveedor.Text, New Filtro With {.IdSeccion = SeccionesProvedorOperativo.SPRO1, .IdCampo = CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR})

        fbxProveedor.DataSource = lista_

    End Sub

    Protected Sub scFacturaProveedor_Click(sender As Object, e As EventArgs)

        Dim operacion_ = IIf(swcTipoOperacion.Checked = True, IControladorFacturaComercial.TipoOperaciones.Importacion, IControladorFacturaComercial.TipoOperaciones.Exportacion)

        Dim _icontroladorFacturaComercial = New ControladorFacturaComercial(1, True, operacion_)

        If fbxProveedor.Value IsNot Nothing And fbcCliente.Value IsNot Nothing Then

            Dim idProveedor_ = New ObjectId(fbxProveedor.Value)

            Dim idCliente_ = New ObjectId(fbcCliente.Value)

            Dim estado_ = _icontroladorFacturaComercial.ListaFacturasProveedor(idProveedor_, idCliente_)

            If estado_.Status = TypeStatus.Ok Then

                Dim listaFacturas_ As List(Of ConstructorFacturaComercial) = estado_.ObjectReturned

                Dim dataSource_ As New List(Of SelectOption)

                Dim facturasInfo_ As New List(Of Dictionary(Of String, String))

                For Each facturaComercial_ In listaFacturas_

                    Dim seccion_ = facturaComercial_.Seccion(SeccionesFacturaComercial.SFAC1)

                    dataSource_.Add(New SelectOption With {
                        .Value = facturaComercial_.Id,
                        .Text = seccion_.Attribute(CamposFacturaComercial.CA_NUMERO_FACTURA).Valor
                        }
                    )

                    facturasInfo_.Add(
                        New Dictionary(Of String, String) From {
                            {"id", facturaComercial_.Id},
                            {"numeroFactura", seccion_.Attribute(CamposFacturaComercial.CA_NUMERO_FACTURA).Valor},
                            {"fechaFactura", seccion_.Attribute(CamposFacturaComercial.CA_FECHA_FACTURA).Valor},
                            {"incoterm", seccion_.Attribute(CamposFacturaComercial.CA_CVE_INCOTERM).ValorPresentacion},
                            {"monedaValor", seccion_.Attribute(CamposFacturaComercial.CA_MONEDA_FACTURACION).Valor},
                            {"monedaValorPresentacion", seccion_.Attribute(CamposFacturaComercial.CA_MONEDA_FACTURACION).ValorPresentacion},
                            {"valorFactura", seccion_.Attribute(CamposFacturaComercial.CP_VALOR_FACTURA).Valor},
                            {"valorMercancia", seccion_.Attribute(CamposFacturaComercial.CP_VALOR_MERCANCIA).Valor}
                        }
                    )

                Next

                scFacturaProveedor.DataSource = dataSource_

                SetVars("facturasInfo_", facturasInfo_)

            End If

        End If

    End Sub


    'SISTEMA SIN LUGAR APROPIADO DESIGNADO

    Public Function RegimenAutorizadoAutoridad() As Boolean

        Return True

    End Function

    Public Function a() As Boolean

        Return True

    End Function

    Private Function ReglasCamposPedimento() As List(Of ReglasCampoPedimento)

        Dim reglasCampos As New List(Of ReglasCampoPedimento)

        With reglasCampos
            '***GENERALES

            '-NÚM. PEDIMENTO.
            .Add(New ReglasCampoPedimento With {
                 .Campo = dbcReferenciaPedimento,
                 .ReglasImportacion = New ReglasImportacion With {.Editable = False},
                 .ReglasExportacion = New ReglasExportacion With {.Editable = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Editable = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Editable = False}
             })
            '-T. OPER.
            .Add(New ReglasCampoPedimento With {
                 .Campo = swcTipoOperacion,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-CVE. PEDIMENTO.
            .Add(New ReglasCampoPedimento With {
                .Campo = scClavePedimento,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Editable = False},
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Editable = False}
            })
            '-RÉGIMEN.
            .Add(New ReglasCampoPedimento With {
                .Campo = scRegimen,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Editable = RegimenAutorizadoAutoridad()},
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Editable = RegimenAutorizadoAutoridad()}
            })
            '-DESTINO/ORIGEN.
            .Add(New ReglasCampoPedimento With {
                 .Campo = scDestinoMercancia,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-TIPO CAMBIO.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icTipoCambio,
                 .ReglasImportacion = New ReglasImportacion With {.Editable = False},
                 .ReglasExportacion = New ReglasExportacion With {.Editable = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Visible = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            '-PESO BRUTO.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icPesoBruto,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-ADUANA E/S.
            .Add(New ReglasCampoPedimento With {
                 .Campo = scAduanaEntradaSalida,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-MEDIO DE TRANSPORTE.
            .Add(New ReglasCampoPedimento With {
                 .Campo = scTransporteEntradaSalida,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-MEDIO DE TRANSPORTE DE ARRIBO
            .Add(New ReglasCampoPedimento With {
                 .Campo = scMedioTransporteArribo,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-MEDIO DE TRANSPORTE DE SALIDA.
            .Add(New ReglasCampoPedimento With {
                 .Campo = scMedioTransporteSalida,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-VALOR DÓLARES.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icValorDolares,
                 .ReglasImportacion = New ReglasImportacion With {.Editable = False},
                 .ReglasExportacion = New ReglasExportacion With {.Editable = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Editable = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Editable = False}
             })
            '-VALOR ADUANA.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icValorAduana,
                 .ReglasImportacion = New ReglasImportacion With {.Editable = False},
                 .ReglasExportacion = New ReglasExportacion With {.Editable = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Editable = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Editable = False}
             })
            '-PRECIO PAGADO/VALOR COMERCIAL
            .Add(New ReglasCampoPedimento With {
                 .Campo = icPrecioPagado,
                 .ReglasImportacion = New ReglasImportacion With {.Editable = False},
                 .ReglasExportacion = New ReglasExportacion With {.Editable = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Editable = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Editable = False}
             })
            '-no definido
            .Add(New ReglasCampoPedimento With {
                 .Campo = scTipoReferencia,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-no definido
            .Add(New ReglasCampoPedimento With {
                 .Campo = scPatente,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-no definido
            .Add(New ReglasCampoPedimento With {
                 .Campo = scEjecutivoCuenta,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '***DATOS IMPORTADOR

            '-NOMBRE, DENOMINACIÓN O RAZÓN SOCIAL DEL IMPORTADOR/EXPORTADOR
            .Add(New ReglasCampoPedimento With {
                 .Campo = fbcCliente,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Editable = False}, 'Es editable si no ha sido sometido a despacho aduanero, -Que caiga en los supuestos de la regla 6.1.2.
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Editable = False} 'Es editable si no ha sido sometido a despacho aduanero, -Que caiga en los supuestos de la regla 6.1.2.
             })
            '-RFC DEL IMPORTADOR/EXPORTADOR
            .Add(New ReglasCampoPedimento With {
                 .Campo = icRFCCliente,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Editable = False}, '-Es editable si no ha sido sometido a despacho aduanero, -Que caiga en los supuestos de la regla 6.1.2.
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Editable = False} '-Es editable si no ha sido sometido a despacho aduanero, -Que caiga en los supuestos de la regla 6.1.2.
             })
            '-CURP DEL IMPORTADOR/EXPORTADOR
            .Add(New ReglasCampoPedimento With {
                 .Campo = icCURP,
                 .ReglasImportacion = New ReglasImportacion With {.Editable = False},
                 .ReglasExportacion = New ReglasExportacion With {.Editable = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Editable = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Editable = False}
             })
            '-DOMICILIO DEL IMPORTADOR/EXPORTADOR.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icDomicilioCliente,
                 .ReglasImportacion = New ReglasImportacion With {.Editable = False},
                 .ReglasExportacion = New ReglasExportacion With {.Editable = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Editable = False}, 'Solo si la razón social/RFC sufrió cambios
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Editable = False} 'Solo si la razón social/RFC sufrió cambios
             })
            '***INCREMENTABLES

            '-VAL. SEGUROS.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icValorSeguros,
                 .ReglasImportacion = New ReglasImportacion, 'Si se capturan en el módulo de factura,no deben estar disponibles para capturarlos
                 .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion, 'Si se capturan en el módulo de factura,no deben estar disponibles para capturarlos
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            '-SEGUROS
            .Add(New ReglasCampoPedimento With {
                 .Campo = icSeguros,
                 .ReglasImportacion = New ReglasImportacion, 'Si se capturan en el módulo de factura,no deben estar disponibles para capturarlos
                 .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion, 'Si se capturan en el módulo de factura,no deben estar disponibles para capturarlos
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            '-FLETES
            .Add(New ReglasCampoPedimento With {
                 .Campo = icFletes,
                 .ReglasImportacion = New ReglasImportacion, 'Si se capturan en el módulo de factura,no deben estar disponibles para capturarlos
                 .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion, 'Si se capturan en el módulo de factura,no deben estar disponibles para capturarlos
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            '-EMBALAJES
            .Add(New ReglasCampoPedimento With {
                 .Campo = icEmbalajes,
                 .ReglasImportacion = New ReglasImportacion, 'Si se capturan en el módulo de factura,no deben estar disponibles para capturarlos
                 .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion, 'Si se capturan en el módulo de factura,no deben estar disponibles para capturarlos
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            '-OTROS INCREMENTABLES.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icOtrosIncrementables,
                 .ReglasImportacion = New ReglasImportacion, 'Si se capturan en el módulo de factura,no deben estar disponibles para capturarlos
                 .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion, 'Si se capturan en el módulo de factura,no deben estar disponibles para capturarlos
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            '***DECREMENTABLES

            '-TRANSPORTE DECREMENTABLES.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icTransporteDec,
                 .ReglasImportacion = New ReglasImportacion, 'Si se capturan en el módulo de factura,no deben estar disponibles para capturarlos
                 .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion, 'Si se capturan en el módulo de factura,no deben estar disponibles para capturarlos
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            '-SEGURO DECREMENTABLES.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icSegurosDec,
                 .ReglasImportacion = New ReglasImportacion, 'Si se capturan en el módulo de factura,no deben estar disponibles para capturarlos
                 .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion, 'Si se capturan en el módulo de factura,no deben estar disponibles para capturarlos
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            '-CARGA DECREMENTABLES.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icCargaDec,
                 .ReglasImportacion = New ReglasImportacion, 'Si se capturan en el módulo de factura,no deben estar disponibles para capturarlos
                 .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion, 'Si se capturan en el módulo de factura,no deben estar disponibles para capturarlos
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            '-DESCARGA DECREMENTABLES.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icDescargaDec,
                 .ReglasImportacion = New ReglasImportacion, 'Si se capturan en el módulo de factura,no deben estar disponibles para capturarlos
                 .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion, 'Si se capturan en el módulo de factura,no deben estar disponibles para capturarlos
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            '-OTROS DECREMENTABLES.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icOtrosDec,
                 .ReglasImportacion = New ReglasImportacion, 'Si se capturan en el módulo de factura,no deben estar disponibles para capturarlos
                 .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion, 'Si se capturan en el módulo de factura,no deben estar disponibles para capturarlos
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            '-ACUSE ELECTRÓNICO DE VALIDACIÓN.
            '-CÓDIGO DE BARRAS.
            '-CLAVE DE LA SECCIÓN ADUANERA DE DESPACHO.
            '-MARCAS, NÚMEROS Y TOTAL DE BULTOS

            '***VALIDACION Y PAGOS

            '-ACUSE ELECTRÓNICO DE VALIDACIÓN.
            '-CÓDIGO DE BARRAS.
            '-CLAVE DE LA SECCIÓN ADUANERA DE DESPACHO.
            '-MARCAS, NÚMEROS Y TOTAL DE BULTOS
            'scValidadorDesignado
            'scNumeroSemana
            'icArchivoValidacion
            'icAcuseValidación
            'icArchivoPago
            'icAcusetaPago
            'scValidacionAduanaDespacho
            'icMarcasNumeros
            'icCertificacion
            'icFechaValidacion

            '***FECHAS

            .Add(New ReglasCampoPedimento With {
                 .Campo = icFechaRegistro,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Visible = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            .Add(New ReglasCampoPedimento With {
                 .Campo = icFechaRevalidacion,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Visible = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            .Add(New ReglasCampoPedimento With {
                 .Campo = icFechaZarpe,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Visible = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            .Add(New ReglasCampoPedimento With {
                 .Campo = icFechaPrevio,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Visible = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            .Add(New ReglasCampoPedimento With {
                 .Campo = icFechaFondeo,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Visible = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            .Add(New ReglasCampoPedimento With {
                 .Campo = icFechaPago,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Visible = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            .Add(New ReglasCampoPedimento With {
                 .Campo = icFechaAtraque,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Visible = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            .Add(New ReglasCampoPedimento With {
                 .Campo = icFechaDespacho,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Visible = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            .Add(New ReglasCampoPedimento With {
                 .Campo = icFechaEstimadaArribo,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Visible = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            .Add(New ReglasCampoPedimento With {
                 .Campo = icFechaEntrega,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Visible = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            .Add(New ReglasCampoPedimento With {
                 .Campo = icFechaEntrada,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Visible = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            .Add(New ReglasCampoPedimento With {
                 .Campo = icFechaPresentacion,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Visible = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            .Add(New ReglasCampoPedimento With {
                 .Campo = icFechaFacturacion,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Visible = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            '***TASAS Y TOTALES

            '-CONTRIB.
            .Add(New ReglasCampoPedimento With {
                 .Campo = scTasasContribucion,
                 .ReglasImportacion = New ReglasImportacion With {.Editable = False},
                 .ReglasExportacion = New ReglasExportacion With {.Editable = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Editable = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Editable = False}
             })
            '-CVE. T. TASA.
            .Add(New ReglasCampoPedimento With {
                 .Campo = scTasasTipoTasa,
                 .ReglasImportacion = New ReglasImportacion With {.Editable = False},
                 .ReglasExportacion = New ReglasExportacion With {.Editable = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Editable = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Editable = False}
             })
            '-TASA.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icTasasTasa,
                 .ReglasImportacion = New ReglasImportacion With {.Editable = False},
                 .ReglasExportacion = New ReglasExportacion With {.Editable = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Editable = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Editable = False}
             })

            '***CUADRO DE LIQUIDACION

            '-CONCEPTO.
            .Add(New ReglasCampoPedimento With {
                 .Campo = scCuadroLiquidacionConcepto,
                 .ReglasImportacion = New ReglasImportacion With {.Editable = False},
                 .ReglasExportacion = New ReglasExportacion With {.Editable = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Editable = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Editable = False}
             })
            '-F.P.
            .Add(New ReglasCampoPedimento With {
                 .Campo = scCuadroLiquidacionFP,
                 .ReglasImportacion = New ReglasImportacion With {.Editable = False},
                 .ReglasExportacion = New ReglasExportacion With {.Editable = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Editable = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Editable = False}
             })
            '-IMPORTE.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icCuadroLiquidacionImporte,
                 .ReglasImportacion = New ReglasImportacion With {.Editable = False},
                 .ReglasExportacion = New ReglasExportacion With {.Editable = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Editable = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Editable = False}
             })
            'icCuadroLiquidacionDescripcion
            '-EFECTIVO.
            '-OTROS.
            '-TOTAL.

            '***CERTIFICACION
            'no va en el formulario

            '***DEPOSITO REFERENCIADO
            'no va en el formulario

            '***CÓDIGO QR, VERIFICADOR DE PAGO O CUMPLIMIENTO.
            'no va en el formulario 

            '***PROVEEDORES
            'No es necesaria la seccion cuando CVE_PEDIMENTO=E1, E2, G1,C3, K2, E3, E4, G2, K3, F3, V3, F8, F9, G6, G7, V8. 

            '-ID. FISCAL.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icIdFiscalProveedor,
                 .ReglasImportacion = New ReglasImportacion With {.Editable = False},
                 .ReglasExportacion = New ReglasExportacion With {.Editable = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-NOMBRE, DENOMINACIÓN O RAZÓN SOCIAL
            .Add(New ReglasCampoPedimento With {
                 .Campo = fbxProveedor,
                 .ReglasImportacion = New ReglasImportacion With {.Editable = False},
                 .ReglasExportacion = New ReglasExportacion With {.Editable = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-DOMICILIO.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icDocimilioProveedor,
                 .ReglasImportacion = New ReglasImportacion With {.Editable = False},
                 .ReglasExportacion = New ReglasExportacion With {.Editable = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-VINCULACIÓN, 
            '-NÚM. CFDI O DOCUMENTO EQUIVALENTE.
            .Add(New ReglasCampoPedimento With {
                 .Campo = scFacturaProveedor,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-FECHA.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icFechaFacturaProveedor,
                 .ReglasImportacion = New ReglasImportacion With {.Editable = False},
                 .ReglasExportacion = New ReglasExportacion With {.Editable = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-INCOTERM.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icIncontermProveedor,
                 .ReglasImportacion = New ReglasImportacion With {.Editable = False},
                 .ReglasExportacion = New ReglasExportacion With {.Editable = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-MONEDA FACTURA
            '-VAL. MON. FACT.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icMontoFacturaProveedor,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-FACTOR MON. FACT.
            .Add(New ReglasCampoPedimento With {
                 .Campo = scFactorMonedaProveedor,
                 .ReglasImportacion = New ReglasImportacion With {.Editable = False},
                 .ReglasExportacion = New ReglasExportacion With {.Editable = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-VAL. DÓLARES.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icMontoFacturaUSDProveedor,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '***DATOS DEL DESTINATARIO

            '-ID. FISCAL.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icTaxtIDDestinatario,
                 .ReglasImportacion = New ReglasImportacion With {.Visible = False}, 'Solo si T_OPER=TRA
                 .ReglasExportacion = New ReglasExportacion With {.Editable = False}, 'Solo lo llenan cuando el destinatario es distinto del comprador. 
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Visible = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-NOMBRE, DENOMINACIÓN O RAZÓN SOCIAL
            .Add(New ReglasCampoPedimento With {
                 .Campo = scRazonSocialDestinatario,
                 .ReglasImportacion = New ReglasImportacion With {.Visible = False}, 'Solo si T_OPER=TRA
                 .ReglasExportacion = New ReglasExportacion With {.Editable = False}, 'Solo lo llenan cuando el destinatario es distinto del comprador. 
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Visible = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-DOMICILIO.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icDomicilioDestinatario,
                 .ReglasImportacion = New ReglasImportacion With {.Visible = False}, 'Solo si T_OPER=TRA
                 .ReglasExportacion = New ReglasExportacion With {.Editable = False}, 'Solo lo llenan cuando el destinatario es distinto del comprador. 
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Visible = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '***DATOS TRANSPORTE Y TRANSPORTISTA

            '-IDENTIFICACIÓN.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icIDTransporte,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-PAÍS.
            .Add(New ReglasCampoPedimento With {
                 .Campo = scPaisTransporte,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-TRANSPORTISTA.
            .Add(New ReglasCampoPedimento With {
                 .Campo = scTransportista,
                 .ReglasImportacion = New ReglasImportacion With {.Visible = False, .Editable = False}, 'Solo si T_OPER=TRA, excepto pedimento clave T9.
                 .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Visible = False, .Editable = False}, 'Solo si el pedimento original tenía el dato.
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            '-RFC.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icTransportistaRfc,
                 .ReglasImportacion = New ReglasImportacion With {.Visible = False, .Editable = False}, 'Solo si T_OPER=TRA, excepto pedimento clave T9.
                 .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Visible = False, .Editable = False}, 'Solo si el pedimento original tenía el dato.
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            '-CURP.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icTransportistaCurp,
                 .ReglasImportacion = New ReglasImportacion With {.Visible = False, .Editable = False}, 'Solo si T_OPER=TRA, excepto pedimento clave T9.
                 .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Visible = False, .Editable = False}, 'Solo si el pedimento original tenía el dato.
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            '-DOMICILIO/CIUDAD/ESTADO.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icTransportistaDomicilio,
                 .ReglasImportacion = New ReglasImportacion With {.Visible = False, .Editable = False}, 'Solo si T_OPER=TRA, excepto pedimento clave T9.
                 .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Visible = False, .Editable = False}, 'Solo si el pedimento original tenía el dato.
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            '***CANDADOS

            '-NÚMERO DE CANDADO.
            .Add(New ReglasCampoPedimento With {
                 .Campo = ccCandados,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-1RA. REVISIÓN.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icCandadoPrimeraRevisión,
                 .ReglasImportacion = New ReglasImportacion With {.Visible = False},
                 .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Visible = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            '-2DA. REVISIÓN.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icCandadoSegundaRevision,
                 .ReglasImportacion = New ReglasImportacion With {.Visible = False},
                 .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Visible = False},
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
             })
            '***GUIAS, MANIFIESTOS, CONOCIMENTOS DE EMBARQUE O DOCUMENTOS

            '-NÚMERO (GUÍA/CONOCIMIENTO EMBARQUE) DOCUMENTOS DE TRANSPORTE
            .Add(New ReglasCampoPedimento With {
                 .Campo = icGuia,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-ID
            .Add(New ReglasCampoPedimento With {
                 .Campo = swcTipoGuia,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '***CONTENEDORES/EQUIPO FERROCARRIL/NUMERO ECONOMICO DEL VEHICULO

            '-NÚMERO DE CONTENEDOR/EQUIPO FERROCARRIL/NÚMERO ECONÓMICO DEL VEHÍCULO.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icNumeroContenedor,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-TIPO DE CONTENEDOR/EQUIPO FERROCARRIL/NÚMERO ECONÓMICO DEL VEHÍCULO.
            .Add(New ReglasCampoPedimento With {
                 .Campo = scTipoContenedor,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '***IDENTIFICADORES (NIVEL PEDIMENTO)

            '-CLAVE.
            .Add(New ReglasCampoPedimento With {
                 .Campo = scIdentificadorPedimento,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-COMPL. IDENTIFICADOR 1.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icComplemento1Pedimento,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-COMPL. IDENTIFICADOR 2.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icComplemento2Pedimento,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-COMPL. IDENTIFICADOR 3.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icComplemento3Pedimento,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '***CUENTAS ADUANERAS Y CUENTAS ADUANERAS DE GARANTIA (NIVEL PEDIMENTO)

            '-TIPO CUENTA.
            .Add(New ReglasCampoPedimento With {
                 .Campo = scCuentaAduanera,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-CLAVE DE GARANTÍA.
            .Add(New ReglasCampoPedimento With {
                 .Campo = scTipoCuentaAduanera,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-INSTITUCIÓN EMISORA.
            .Add(New ReglasCampoPedimento With {
                 .Campo = scInstitucionEmisora,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-NÚMERO DE CONTRATO.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icNumeroCOntratoCuentaAduanera,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-FOLIO CONSTANCIA.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icFolioConstanciaCuentaAduanera,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-TOTAL DEPÓSITO.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icImporteCuentaAduanera,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-FECHA CONSTANCIA.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icFechaEmisionCuentaAduanera,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            'no definido
            .Add(New ReglasCampoPedimento With {
                 .Campo = icPrecioEstimadoCuentaAduanera,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            'no definido
            .Add(New ReglasCampoPedimento With {
                 .Campo = icTitulosCuentaAduanera,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            'no definido
            .Add(New ReglasCampoPedimento With {
                 .Campo = icValorUnitario,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '***DESCARGOS

            '-NÚM. PEDIMENTO ORIGINAL.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icDescargosPedCompletoOriginal,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-FECHA DE OPERACIÓN ORIGINAL.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icDescargosFechaPedOriginal,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-CVE. PEDIMENTO ORIGINAL.
            .Add(New ReglasCampoPedimento With {
                 .Campo = scDescargosClavePedOriginal,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            'no definido
            .Add(New ReglasCampoPedimento With {
                 .Campo = icDescargosValidacionOriginal,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            'no definido
            .Add(New ReglasCampoPedimento With {
                 .Campo = icDescargosValidacion2Original,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            'no definido
            .Add(New ReglasCampoPedimento With {
                 .Campo = scDescargosPatenteOriginal,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            'no definido
            .Add(New ReglasCampoPedimento With {
                 .Campo = scDescargosAduanaOriginal,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            'no definido
            .Add(New ReglasCampoPedimento With {
                 .Campo = scDescargosAduana2Original,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            'no definido
            .Add(New ReglasCampoPedimento With {
                 .Campo = icDescargosPedOriginal,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            'no definido
            .Add(New ReglasCampoPedimento With {
                 .Campo = icDescargosFraccionOriginal,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            'no definido
            .Add(New ReglasCampoPedimento With {
                 .Campo = icDescargosUMOriginal,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            'no definido
            .Add(New ReglasCampoPedimento With {
                 .Campo = icDescargosUMDescargo,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '***COMPENSACIONES

            'no definido
            .Add(New ReglasCampoPedimento With {
                 .Campo = scCompensacionesContribucion,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-NÚM. PEDIMENTO ORIGINAL.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icCompensacionesPedCompletoOriginal,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-FECHA DE OPERACIÓN ORIGINAL.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icCompensacionesFechaPagoPedOriginal,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-CLAVE DE GRAVAMEN.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icCompensacionesGravamen,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-IMPORTE DEL GRAVAMEN.
            'no definido
            .Add(New ReglasCampoPedimento With {
                 .Campo = scCompensacionesConcepto,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            'no definido
            .Add(New ReglasCampoPedimento With {
                 .Campo = icCompensacionesAñoValidacionOriginal,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            'no definido
            .Add(New ReglasCampoPedimento With {
                 .Campo = icCompensacionesAñoValidacion2Original,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            'no definido
            .Add(New ReglasCampoPedimento With {
                 .Campo = scCompensacionesPatenteOriginal,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            'no definido
            .Add(New ReglasCampoPedimento With {
                 .Campo = scCompensacionesAduanaOriginal,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            'no definido
            .Add(New ReglasCampoPedimento With {
                 .Campo = scCompensacionesAduana2Original,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            'no definido
            .Add(New ReglasCampoPedimento With {
                 .Campo = icCompensacionesPedOriginal,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '***FORMAS DE PAGOS VIRTUALES

            '-FORMAS DE PAGO.
            .Add(New ReglasCampoPedimento With {
                 .Campo = scPagosVirtualesFormaPago,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-DEPENDENCIA O INSTITUCIÓN EMISORA
            .Add(New ReglasCampoPedimento With {
                 .Campo = scPagosVIrtualesEmisora,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-NÚMERO DEL DOCUMENTO.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icPagosVirtualesDocumento,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-FECHA DEL DOCUMENTO.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icPagosVirtualesFechaDocumento,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-IMPORTE DEL DOCUMENTO.
            .Add(New ReglasCampoPedimento With {
                 .Campo = icPagosVirtualesImporteDocumento,
                 .ReglasImportacion = New ReglasImportacion,
                 .ReglasExportacion = New ReglasExportacion,
                 .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                 .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
             })
            '-SALDO DISPONIBLE.
            .Add(New ReglasCampoPedimento With {
                .Campo = icPagosVirtualesSaldo,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-IMPORTE A PAGAR.
            .Add(New ReglasCampoPedimento With {
                .Campo = icPagosVirtualesImportePedimento,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '***OBSERVACIONES

            '-Observaciones (nivel pedimento)
            .Add(New ReglasCampoPedimento With {
                .Campo = fscObservaciones,
                .ReglasImportacion = New ReglasImportacion With {.Editable = False},
                .ReglasExportacion = New ReglasExportacion With {.Editable = False},
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Editable = False},
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Editable = False}
            })
            '***PARTIDAS

            '-SEC
            '-FRACCIÓN.
            .Add(New ReglasCampoPedimento With {
                .Campo = icFraccionArancelaria,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-SUBD. / NÚM. IDENTIFICACIÓN COMERCIAL
            .Add(New ReglasCampoPedimento With {
                .Campo = icNico,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-PRECIO UNIT.
            .Add(New ReglasCampoPedimento With {
                .Campo = icPartidaPrecioUnitario,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-MET. VAL.
            .Add(New ReglasCampoPedimento With {
                .Campo = scPartidaMetodoValoracion,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
            })
            '-CANTIDAD UMC.
            .Add(New ReglasCampoPedimento With {
                .Campo = icCantidadUMC,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-UMC
            .Add(New ReglasCampoPedimento With {
                .Campo = scUMC,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-CANTIDAD UMT
            .Add(New ReglasCampoPedimento With {
                .Campo = icCantidadUMT,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-UMT
            .Add(New ReglasCampoPedimento With {
                .Campo = scUMT,
                .ReglasImportacion = New ReglasImportacion With {.Editable = False},
                .ReglasExportacion = New ReglasExportacion With {.Editable = False},
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Editable = False},
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Editable = False}
            })
            '-VAL. ADU/VAL. USD.
            .Add(New ReglasCampoPedimento With {
                .Campo = icPartidaValorAduana,
                .ReglasImportacion = New ReglasImportacion With {.Editable = False},
                .ReglasExportacion = New ReglasExportacion With {.Editable = False},
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Editable = False},
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Editable = False}
            })
            '-VAL. ADU/VAL. USD.
            .Add(New ReglasCampoPedimento With {
                .Campo = icPartidaValorUSd,
                .ReglasImportacion = New ReglasImportacion With {.Editable = False},
                .ReglasExportacion = New ReglasExportacion With {.Editable = False},
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Editable = False},
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Editable = False}
            })
            '-IMP. PRECIO PAG./VALOR COMERCIAL.
            .Add(New ReglasCampoPedimento With {
                .Campo = icPartidaPrecioPagado,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-IMP. PRECIO PAG./VALOR COMERCIAL.
            .Add(New ReglasCampoPedimento With {
                .Campo = icPartidaValorComercial,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-P. V/C.
            .Add(New ReglasCampoPedimento With {
                .Campo = scPaisVendedor,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-P. V/C.
            .Add(New ReglasCampoPedimento With {
                .Campo = scPaisComprador,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-P. O/D.
            .Add(New ReglasCampoPedimento With {
                .Campo = scPaisOrigen,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-P. O/D.
            .Add(New ReglasCampoPedimento With {
                .Campo = scPaisDestino,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-MARCA
            .Add(New ReglasCampoPedimento With {
                .Campo = icPartidaMarca,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-MODELO
            '-CODIGO PRODUCTO
            .Add(New ReglasCampoPedimento With {
                .Campo = icPartidaModelo,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-VINC
            .Add(New ReglasCampoPedimento With {
                .Campo = scPartidaVinculacion,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Visible = False}
            })
            '-VAL. AGREG.
            .Add(New ReglasCampoPedimento With {
                .Campo = icPartidaValorAgregado,
                .ReglasImportacion = New ReglasImportacion With {.Visible = False},
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Visible = False},
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-DESCRIPCIÓN (RENGLONES VARIABLES SEGÚN SE REQUIERA).
            .Add(New ReglasCampoPedimento With {
                .Campo = icPartidaDescripcion,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-CON
            .Add(New ReglasCampoPedimento With {
                .Campo = scPartidaContribucion,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-TASA
            .Add(New ReglasCampoPedimento With {
                .Campo = icPartidaTasa,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-T.T
            .Add(New ReglasCampoPedimento With {
                .Campo = scPartidaTipoTasa,
                .ReglasImportacion = New ReglasImportacion With {.Editable = False},
                .ReglasExportacion = New ReglasExportacion With {.Editable = False},
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Editable = False},
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Editable = False}
            })
            '-F.P.
            .Add(New ReglasCampoPedimento With {
                .Campo = scFormaPago,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-IMPORTE
            .Add(New ReglasCampoPedimento With {
                .Campo = icImporte,
                .ReglasImportacion = New ReglasImportacion With {.Editable = False},
                .ReglasExportacion = New ReglasExportacion With {.Editable = False},
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion With {.Editable = False},
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion With {.Editable = False}
            })
            '***MERCANCIAS
            'no esta

            '***REGULACIONES Y RESTRICCIONES NO ARANCELARIAS

            '-PERMISO
            .Add(New ReglasCampoPedimento With {
                .Campo = scClavePermiso,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-NÚMERO DE PERMISO.
            .Add(New ReglasCampoPedimento With {
                .Campo = icPermisoNom,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-FIRMA DESCARGO.
            .Add(New ReglasCampoPedimento With {
                .Campo = icFirmaDescargo,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-VAL. COM. DLS.
            .Add(New ReglasCampoPedimento With {
                .Campo = icValorComercialDLS,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-CANTIDAD UMT/C.
            .Add(New ReglasCampoPedimento With {
                .Campo = icCantidadUMTC,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '***IDENTIFICADORES (NIVEL PARTIDA)

            '-IDENTIF.
            .Add(New ReglasCampoPedimento With {
                .Campo = scPartidaIdentificador,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-COMPLEMENTO 1.
            .Add(New ReglasCampoPedimento With {
                .Campo = icPartidaComplemento1,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-COMPLEMENTO 2.
            .Add(New ReglasCampoPedimento With {
                .Campo = icPartidaComplemento2,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '--COMPLEMENTO 3.
            .Add(New ReglasCampoPedimento With {
                .Campo = icPartidaComplemento3,
                .ReglasImportacion = New ReglasImportacion,
                .ReglasExportacion = New ReglasExportacion,
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '***CUENTAS ADUANERAS DE GARANTIA (NIVEL PARTIDA)
            'no esta

            '***RECTIFICACIONES

            'no definido
            .Add(New ReglasCampoPedimento With {
                .Campo = icRectificacionesFechaPedOriginal,
                .ReglasImportacion = New ReglasImportacion With {.Visible = False},
                .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-PEDIMENTO ORIGINAL.
            .Add(New ReglasCampoPedimento With {
                .Campo = scRectificacionesClavePedOriginal,
                .ReglasImportacion = New ReglasImportacion With {.Visible = False},
                .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            'no definido
            .Add(New ReglasCampoPedimento With {
                .Campo = icRectificacionesPatentePedOriginal,
                .ReglasImportacion = New ReglasImportacion With {.Visible = False},
                .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-CVE. PEDIM. ORIGINAL.
            .Add(New ReglasCampoPedimento With {
                .Campo = icRectificacionesPedimentoCompleto,
                .ReglasImportacion = New ReglasImportacion With {.Visible = False},
                .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            'no definido
            .Add(New ReglasCampoPedimento With {
                .Campo = icRectificacionesAñoValidacion2PedOriginal,
                .ReglasImportacion = New ReglasImportacion With {.Visible = False},
                .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            'no definido
            .Add(New ReglasCampoPedimento With {
                .Campo = icRectificacionesAñoValidacionPedOriginal,
                .ReglasImportacion = New ReglasImportacion With {.Visible = False},
                .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            'no definido
            .Add(New ReglasCampoPedimento With {
                .Campo = scRectificacionesAduanaOriginal,
                .ReglasImportacion = New ReglasImportacion With {.Visible = False},
                .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            'no definido
            .Add(New ReglasCampoPedimento With {
                .Campo = scRectificacionesAduana2Original,
                .ReglasImportacion = New ReglasImportacion With {.Visible = False},
                .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-CVE. PEDIM. RECT.
            .Add(New ReglasCampoPedimento With {
                .Campo = scRectificacionesClavePedimento,
                .ReglasImportacion = New ReglasImportacion With {.Visible = False},
                .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-FECHA PAGO RECT.
            .Add(New ReglasCampoPedimento With {
                .Campo = icRectificacionesFechaPedimento,
                .ReglasImportacion = New ReglasImportacion With {.Visible = False},
                .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '***DIFERENCIAS DE CONTRIBUCIONES (NIVEL PEDIMENTO)

            '-CONCEPTO.
            .Add(New ReglasCampoPedimento With {
                .Campo = scDifConConcepto,
                .ReglasImportacion = New ReglasImportacion With {.Visible = False},
                .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-F.P.
            .Add(New ReglasCampoPedimento With {
                .Campo = scDifConFormaPago,
                .ReglasImportacion = New ReglasImportacion With {.Visible = False},
                .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-DIFERENCIA.
            .Add(New ReglasCampoPedimento With {
                .Campo = icDifConDiferencia,
                .ReglasImportacion = New ReglasImportacion With {.Visible = False},
                .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-DIFERENCIA.
            .Add(New ReglasCampoPedimento With {
                .Campo = icDifConEfectivo,
                .ReglasImportacion = New ReglasImportacion With {.Visible = False},
                .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-OTROS.
            .Add(New ReglasCampoPedimento With {
                .Campo = icDifConOtros,
                .ReglasImportacion = New ReglasImportacion With {.Visible = False},
                .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })
            '-DIF. TOTALES.
            .Add(New ReglasCampoPedimento With {
                .Campo = icDifConTotal,
                .ReglasImportacion = New ReglasImportacion With {.Visible = False},
                .ReglasExportacion = New ReglasExportacion With {.Visible = False},
                .ReglasRectificacionImportacion = New ReglasRectificacionImportacion,
                .ReglasRectificacionExportacion = New ReglasRectificacionExportacion
            })

        End With

        Return reglasCampos

    End Function

    Private Sub AplicarReglasCampoPedimento()

        Dim reglasCampos = ReglasCamposPedimento()

        For Each regla_ As ReglasCampoPedimento In reglasCampos

            If swcTipoOperacion.Checked = True Then
                'IMPORTACION

                If scClavePedimento.Value = "13" Then
                    'RECTIFICACION

                    regla_.Campo.Visible = regla_.ReglasRectificacionImportacion.Visible
                    regla_.Campo.Enabled = regla_.ReglasRectificacionImportacion.Editable

                Else
                    'NORMAL

                    regla_.Campo.Visible = regla_.ReglasImportacion.Visible
                    regla_.Campo.Enabled = regla_.ReglasImportacion.Editable

                End If

            Else
                'EXPORTACION

                If scClavePedimento.Value = "13" Then
                    'RECTIFICACION

                    regla_.Campo.Visible = regla_.ReglasRectificacionExportacion.Visible
                    regla_.Campo.Enabled = regla_.ReglasRectificacionExportacion.Editable

                Else
                    'NORMAL

                    regla_.Campo.Visible = regla_.ReglasExportacion.Visible
                    regla_.Campo.Enabled = regla_.ReglasExportacion.Editable

                End If

            End If

        Next

    End Sub

#End Region

End Class

Class ReglasCampoPedimento

    Property Campo As UIControl

    Property ReglasImportacion As ReglasImportacion

    Property ReglasExportacion As ReglasExportacion

    Property ReglasRectificacionImportacion As ReglasRectificacionImportacion

    Property ReglasRectificacionExportacion As ReglasRectificacionExportacion

End Class


Class ReglasImportacion

    Property Visible As Boolean = True

    Property Editable As Boolean = True

End Class

Class ReglasExportacion

    Property Visible As Boolean = True

    Property Editable As Boolean = True

End Class

Class ReglasRectificacionImportacion

    Property Visible As Boolean = True

    Property Editable As Boolean = True

End Class

Class ReglasRectificacionExportacion

    Property Visible As Boolean = True

    Property Editable As Boolean = True

End Class

Class ReglasModalidad

    Property Visible As Boolean = True

    Property Editable As Boolean = True

End Class