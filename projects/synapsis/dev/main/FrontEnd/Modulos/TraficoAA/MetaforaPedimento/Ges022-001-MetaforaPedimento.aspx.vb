
#Region "├┴┘├┴┘├┴┘├┴┘├┴┘|├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘   DEPENDENCIAS   ├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘"

'RECURSOS DEL CMF
Imports gsol
Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Wma.Exceptions
Imports Syn.Operaciones
Imports Gsol.Web.Components
Imports Syn.Documento
Imports Syn.Documento.Componentes.Campo
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Nucleo.RecursosComercioExterior.CamposPedimento
Imports Wma.Exceptions.TagWatcher

'UTILERIAS/RECURSOS ADICIONALES
Imports Sax.Web

'OBJETOS DIMENSIONALES (ODS's) Dependencias en MongoDB
Imports Rec.Globals.Controllers

'OBJETOS BIDIMENSIONALES (ODF's.  Dependencias Krombase/SQL Server)
Imports Rec.Globals.Utils
Imports Syn.CustomBrokers.Controllers
Imports Syn.CustomBrokers.Controllers.ControladorRecursosAduanales
Imports System.Net

#End Region

Public Class Ges022_001_MetaforaPedimento
    Inherits ClaseSinNombre

#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Dim _manifestacionValor As IControladorManifestacionValor

#End Region

#Region "██████ Vinculación c/capas inf █████████       SAX      ████████████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Public Overrides Sub Inicializa()

        With Buscador

            .DataObject = New ConstructorPedimentoNormal(True)

            .addFilter(SeccionesPedimento.ANS1, CamposPedimento.CA_NUM_PEDIMENTO_COMPLETO, "Pedimento")
            .addFilter(SeccionesPedimento.ANS1, CamposPedimento.CP_REFERENCIA, "Referencia")

        End With

        CargaCatalogos()

    End Sub

    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher

        ' ** * ** * Generales * ** * **
        [Set](dbc_ReferenciaPedimento, CP_REFERENCIA, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](dbc_ReferenciaPedimento, CA_NUM_PEDIMENTO_COMPLETO, propiedadDelControl_:=PropiedadesControl.ValueDetail)
        [Set](sc_Patente, CP_MODALIDAD_ADUANA_PATENTE)
        [Set](sc_EjecutivoCuenta, CP_EJECUTIVO_DE_CUENTA)
        [Set](Convert.ToInt32(IIf(sch_TipoOperacion.Checked, 1, 2)), CA_T_OPER, TiposDato.Entero)
        [Set](sc_ClavePedimento, CA_CVE_PEDIMENTO)
        [Set](sc_Regimen, CA_REGIMEN)
        [Set](sc_DestinoMercancia, CA_DESTINO_ORIGEN)
        [Set](ic_TipoCambio, CA_TIPO_CAMBIO)
        [Set](ic_PesoBruto, CA_PESO_BRUTO)
        [Set](sc_AduanaEntradaSalida, CA_ADUANA_E_S)
        [Set](sc_TransporteEntradaSalida, CA_MEDIO_DE_TRANSPORTE)
        [Set](sc_MedioTransporteArribo, CA_MEDIO_DE_TRANSPORTE_DE_ARRIBO)
        [Set](sc_MedioTransporteSalida, CA_MEDIO_DE_TRANSPORTE_DE_SALIDA)
        [Set](ic_ValorDolares, CA_VALOR_DOLARES)
        [Set](ic_ValorAduana, CA_VALOR_ADUANA)
        [Set](ic_PrecioPagado, CA_PRECIO_PAGADO_O_VALOR_COMERCIAL)
        ' ** * ** * Generales * ** * **

        ' ** * ** * Datos del importador/exportador * ** * **
        [Set](fbc_Cliente, CA_RAZON_SOCIAL_IOE)
        [Set](fbc_Cliente, CA_RAZON_SOCIAL_IOE, propiedadDelControl_:=PropiedadesControl.Text, asignarA_:=TiposAsignacion.ValorPresentacion)
        [Set](ic_RFCCliente, CA_RFC_DEL_IOE)
        [Set](ic_CURP, CA_CURP_DEL_IOE)
        [Set](ic_RFCFacturacion, CA_RFC_AA)
        [Set](ic_DomicilioCliente, CA_DOMICILIO_IOE)
        [Set](ic_ValorSeguros, CA_VAL_SEGUROS)
        [Set](ic_Seguros, CA_SEGUROS)
        [Set](ic_Fletes, CA_FLETES)
        [Set](ic_Embalajes, CA_EMBALAJES)
        [Set](ic_OtrosIncrementables, CA_OTROS_INCREMENTABLES)
        [Set](ic_TransporteDec, CA_TRANSPORTE_DECREMENTABLES)
        [Set](ic_SegurosDec, CA_SEGURO_DECREMENTABLES)
        [Set](ic_CargaDec, CA_CARGA_DECREMENTABLES)
        [Set](ic_DescargaDec, CA_DESCARGA_DECREMENTABLES)
        [Set](ic_OtrosDec, CA_OTROS_DECREMENTABLES)
        ' ** * ** * Datos del importador/exportador * ** * **

        ' ** * ** * ValidacionPago * ** * **
        [Set](sc_ValidadorDesignado, CA_VALIDADOR_DESIGNADO)
        [Set](sc_NumeroSemana, CA_NUMERO_SEMANA)
        [Set](ic_ArchivoValidacion, CA_ARCHIVO_VALIDACION)
        [Set](ic_AcuseValidación, CA_ACUSE_ELECTONICO_DE_VALIDACION)
        [Set](ic_ArchivoPago, CA_ARCHIVO_PAGO)
        [Set](ic_AcusetaPago, CA_ACUSE_ELECTONICO_DE_PAGO)
        [Set](sc_ValidacionAduanaDespacho, CA_ADUANA_DESPACHO)
        [Set](ic_MarcasNumeros, CA_MARCAS_NUMEROS_TOTAL_BULTOS)
        [Set](ic_Certificacion, CA_CERTIFICACION)
        [Set](ic_FechaValidacion, CA_FECHA_VALIDACION)
        ' ** * ** * ValidacionPago * ** * **

        ' ** * ** * Fechas * ** * **
        [Set](ic_FechaRegistro, CA_FECHA_REGISTRO)
        [Set](ic_FechaRevalidacion, CA_FECHA_REVALIDACION)
        [Set](ic_FechaZarpe, CA_FECHA_ZARPE)
        [Set](ic_FechaPrevio, CA_FECHA_PREVIO)
        [Set](ic_FechaFondeo, CA_FECHA_FONDEO)
        [Set](ic_FechaPago, CA_FECHA_PAGO)
        [Set](ic_FechaAtraque, CA_FECHA_ATRAQUE)
        [Set](ic_FechaDespacho, CA_FECHA_DESPACHO)
        [Set](ic_FechaEstimadaArribo, CA_FECHA_ARRIBO)
        [Set](ic_FechaEntrega, CA_FECHA_ENTREGA)
        [Set](ic_FechaEntrada, CA_FECHA_ENTRADA)
        [Set](ic_FechaFacturacion, CA_FECHA_FACTURACION)
        ' ** * ** * Fechas * ** * **

        ' ** * ** * TasasContribuciones * ** * **
        [Set](sc_TasasContribucion, CA_CONTRIBUCION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](sc_TasasTipoTasa, CA_CVE_T_TASA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_TasasTasa, CA_TASA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](CatalogoTasas, Nothing, seccion_:=SeccionesPedimento.ANS6)
        ' ** * ** * TasasContribuciones * ** * **

        ' ** * ** * CatalogoCuadroLiquidacion * ** * **
        [Set](sc_CuadroLiquidacionConcepto, CA_CONCEPTO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_CuadroLiquidacionDescripcion, CA_DESCRIPCION_CONCEPTO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](sc_CuadroLiquidacionFP, CA_FP, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_CuadroLiquidacionImporte, CA_IMPORTE, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](CatalogoCuadroLiquidacion, Nothing, seccion_:=SeccionesPedimento.ANS55)

        [Set](ic_CuadroLiquidacionEfectivo, CA_EFECTIVO)
        [Set](ic_CuadroLiquidacionOtros, CA_OTROS)
        [Set](ic_CuadroLiquidacionTotal, CA_TOTAL)
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
        '[Set](ic_IdFiscalProveedor, CA_ID_FISCAL_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](ic_NombreProveedor, CA_NOMBRE_DEN_RAZON_SOC_POC, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](ic_DocimilioProveedor, CA_DOMICILIO_POC, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](pb_Proveedores, Nothing, seccion_:=SeccionesPedimento.ANS10)

        '[Set](ic_FacturaProveedor, CA_CFDI_O_FACT, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](ic_FechaFacturaProveedor, CA_FECHA_FACT, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](ic_IncontermProveedor, CA_INCOTERM, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](sc_FactorMonedaProveedor, CA_FACTOR_MONEDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](ic_MontoFacturaProveedor, CA_MONTO_MONEDA_FACT, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](ic_MontoFacturaUSDProveedor, CA_MONTO_USD, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](CatalogoFacturas, Nothing, seccion_:=SeccionesPedimento.ANS13)
        ' ** * ** * DatosProveedoresImpo * ** * **

        ' ** * ** * Destinatarios * ** * **
        [Set](ic_TaxtIDDestinatario, CA_ID_FISCAL_DESTINATARIO)
        [Set](sc_RazonSocialDestinatario, CA_NOMBRE_RAZON_SOC_DESTINATARIO)
        [Set](ic_DomicilioDestinatario, CA_DOMICILIO_DESTINATARIO)
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
        [Set](ic_Guia, CA_GUIA_O_MANIF_O_BL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](sc_TipoGuia, CA_MASTER_O_HOUSE, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](CatalogoGuias, Nothing, seccion_:=SeccionesPedimento.ANS16)
        ' ** * ** * Guias * ** * **

        ' ** * ** * Contenedores * ** * **
        [Set](ic_NumeroContenedor, CA_NUM_CONTENEDOR_FERRO_NUM_ECON, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](sc_TipoContenedor, CA_CVE_TIPO_CONTENEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](CatalogoContenedores, Nothing, seccion_:=SeccionesPedimento.ANS17)
        ' ** * ** * Contenedores * ** * **

        ' ** * ** * Identificadores * ** * **
        [Set](sc_IdentificadorPedimento, CA_CVE_IDENTIFICADOR_G, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_Complemento1Pedimento, CA_COMPL_1, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_Complemento2Pedimento, CA_COMPL_2, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_Complemento3Pedimento, CA_COMPL_3, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](CatalogoIdentificadores, Nothing, seccion_:=SeccionesPedimento.ANS18)
        ' ** * ** * Identificadores * ** * **

        ' ** * ** * CuentasAduaneras * ** * **
        [Set](sc_CuentaAduanera, CA_CVE_CTA_ADUANERA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](sc_TipoCuentaAduanera, CA_CVE_TIPO_GARANTIA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](sc_InstitucionEmisora, CA_NOMBRE_INST_EMISORA_CTA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_NumeroCOntratoCuentaAduanera, CA_NUM_CONTRATO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_FolioConstanciaCuentaAduanera, CA_FOLIO_CONSTANCIA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_ImporteCuentaAduanera, CA_IMPORTE_TOTAL_CONSTANCIA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_FechaEmisionCuentaAduanera, CA_FECHA_EMISION_CONSTANCIA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_PrecioEstimadoCuentaAduanera, CA_CANTIDAD_UMT_PRECIO_ESTIMADO_PEDIMENTO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_TitulosCuentaAduanera, CA_TITULOS_ASIGNADOS_PEDIMENTO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_ValorUnitario, CA_VALOR_UNITARIO_TITULO_PEDIMENTO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](CatalogoCuentasAduaneras, Nothing, seccion_:=SeccionesPedimento.ANS19)
        ' ** * ** * CuentasAduaneras * ** * **

        ' ** * ** * Pagosvirtuales * ** * **
        [Set](sc_PagosVirtualesFormaPago, CA_PV_FP, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](sc_PagosVIrtualesEmisora, CA_NOMBRE_INST_EMISORA_DOCTO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_PagosVirtualesDocumento, CA_NUM_DOCTO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_PagosVirtualesFechaDocumento, CA_FECHA_EXP_DOCTO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_PagosVirtualesImporteDocumento, CA_MONTO_DOCTO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_PagosVirtualesSaldo, CA_SALDO_DISP_DOCTO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_PagosVirtualesImportePedimento, CA_MONTO_PAG_PEDIM, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](CatalogoPagosvirtuales, Nothing, seccion_:=SeccionesPedimento.ANS22)
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
        [Set](ic_DescargosPedCompletoOriginal, CA_NUM_PEDIM_ORIGINAL_COMPLETO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_DescargosFechaPedOriginal, CA_FECHA_PEDIM_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](sc_DescargosClavePedOriginal, CA_CVE_PEDIM_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_DescargosValidacionOriginal, CA_AÑO_VALIDACION_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_DescargosValidacion2Original, CA_AÑO_VALIDACION_2_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](sc_DescargosPatenteOriginal, CA_PATENTE_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](sc_DescargosAduanaOriginal, CA_ADUANA_DESPACHO_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](sc_DescargosAduana2Original, CA_ADUANA_DESPACHO_ORIGINAL_2, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_DescargosPedOriginal, CA_NUM_PEDIMENTO_ORIGINAL_7_DIGITOS, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_DescargosFraccionOriginal, CA_FRACCION_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_DescargosUMOriginal, CA_UM_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_DescargosUMDescargo, CA_CANT_MERCANCIA_UMT_DESCARGO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](CatalogoDescargos, Nothing, seccion_:=SeccionesPedimento.ANS20)
        ' ** * ** * CatalogoDescargos * ** * **

        ' ** * ** * Compensaciones * ** * **
        [Set](sc_CompensacionesContribucion, CA_COMPENSACION_CONTRIBUCION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_CompensacionesPedCompletoOriginal, CA_COMPENSACION_NUM_PEDIM_ORIGINAL_COMPLETO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_CompensacionesFechaPagoPedOriginal, CA_FECHA_PAGO_ORIG_PARA_COMPENSAC, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_CompensacionesGravamen, CA_IMPORTE_GRAVAMEN_COMPENSACION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](sc_CompensacionesConcepto, CA_CVE_CONCEPTO_COMPENSACION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_CompensacionesAñoValidacionOriginal, CA_COMPENSACION_AÑO_VALIDACION_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_CompensacionesAñoValidacion2Original, CA_COMPENSACION_AÑO_VALIDACION_2_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](sc_CompensacionesPatenteOriginal, CA_COMPENSACION_PATENTE_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](sc_CompensacionesAduanaOriginal, CA_COMPENSACION_ADUANA_DESPACHO_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](sc_CompensacionesAduana2Original, CA_COMPENSACION_ADUANA_DESPACHO_ORIGINAL_2, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_CompensacionesPedOriginal, CA_COMPENSACION_NUM_PEDIMENTO_ORIGINAL_7_DIGITOS, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](CatalogoCompensaciones, Nothing, seccion_:=SeccionesPedimento.ANS21)
        ' ** * ** * Compensaciones * ** * **

        ' ** * ** * Observaciones * ** * **
        [Set](ic_OnservacionesPedimento, CA_OBSERV_PEDIM)
        ' ** * ** * Observaciones * ** * **

        ' ** * ** * Partidas * ** * **
        [Set](ic_FraccionArancelaria, CA_FRACC_ARANC_PARTIDA)
        [Set](ic_Nico, CA_NICO_PARTIDA)
        '[Set](sc_PartidaVinculacion, CA_PARTIDAS_VINCULACION)
        [Set](sc_PartidaMetodoValoracion, CA_CVE_MET_VALOR_PARTIDA)
        [Set](sc_UMC, CA_CVE_UMC_PARTIDA)
        [Set](ic_CantidadUMC, CA_CANT_UMC_PARTIDA)
        [Set](sc_UMT, CA_CVE_UMT_PARTIDA)

        [Set](ic_CantidadUMT, CA_CANT_UMT_PARTIDA)
        [Set](sc_PaisVC, CA_CVE_PAIS_VEND_O_COMP_PARTIDA)
        [Set](sc_PaisOD, CA_CVE_PAIS_ORIGEN_O_DEST_PARTIDA)
        [Set](ic_PartidaValorAduana, CA_VAL_ADU_O_VAL_USD_PARTIDA)
        [Set](ic_PartidaPrecioPagado, CA_IMP_PRECIO_PAG_O_VAL_COMER_PARTIDA)
        [Set](ic_PartidaPrecioUnitario, CA_MONTO_PRECIO_UNITARIO_PARTIDA)
        [Set](ic_PartidaValorAgregado, CA_MONTO_VALOR_AGREG_PARTIDA)
        [Set](ic_PartidaMarca, CA_NOMBRE_MARCA_PARTIDA)
        [Set](ic_PartidaModelo, CA_CVE_MODELO_PARTIDA)
        [Set](ic_PartidaCodigoProducto, CA_CODIGO_PRODUCTO_PARTIDA)

        [Set](ic_PartidaDescripcion, CA_DESCRIP_MERC_PARTIDA)

        [Set](sc_PartidaContribucion, CA_CONTRIBUCION_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](sc_PartidaTipoTasa, CA_CVE_T_TASA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_PartidaTasa, CA_TASA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](catalogoPartidaTasas, Nothing, seccion_:=SeccionesPedimento.ANS29)

        [Set](sc_PartidaIdentificador, CA_CVE_IDENTIF_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_PartidaComplemento1, CA_COMPL_1_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_PartidaComplemento2, CA_COMPL_2_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ic_PartidaComplemento3, CA_COMPL_3_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](CatalogoPartidasIdentificadores, Nothing, seccion_:=SeccionesPedimento.ANS27)

        [Set](ic_PartidaObservacion, CA_OBSERV_PARTIDA)
        ' ** * ** * Partidas * ** * **

        Return New TagWatcher(1)

    End Function

    Public Overrides Sub BotoneraClicNuevo()

        If OperacionGenerica IsNot Nothing Then

            '_empresa = Nothing

        End If

        PreparaControles()

    End Sub

    Public Overrides Sub BotoneraClicGuardar()

        'ProcesarOperacion(Of Something)()
        If Not ProcesarTransaccion(Of ConstructorPedimentoNormal)().Status = TypeStatus.Errors Then : End If

    End Sub

    Public Overrides Sub BotoneraClicEditar()


    End Sub

    Public Overrides Sub BotoneraClicBorrar()


    End Sub

    Public Overrides Sub BotoneraClicOtros(ByVal IndexSelected_ As Integer)

        Select Case IndexSelected_
            Case 7

                ImprimirPedimentoNormal(dbc_ReferenciaPedimento.Value)

            Case 9

                _manifestacionValor = New ControladorManifestacionValor(1)

                Dim tagWatcher = _manifestacionValor.Generar(OperacionGenerica.Id)

                DisplayMessage(tagWatcher.LastMessage, StatusMessage.Fail)

            Case 10

                ImprimirMV(dbc_ReferenciaPedimento.Value)

        End Select

    End Sub

    'EVENTOS PARA LA INSERCIÓN DE DATOS
    Public Overrides Sub RealizarInsercion(ByRef documentoElectronico_ As DocumentoElectronico)

        With documentoElectronico_

            .FolioOperacion = dbc_ReferenciaPedimento.Value

            .FolioDocumento = dbc_ReferenciaPedimento.ValueDetail

            .IdCliente = 0

            .NombreCliente = fbc_Cliente.Text

        End With

        LimpiaFormatos()

    End Sub

    'EVENTOS PARA MODIFICACIÓN DE DATOS

    Public Overrides Sub RealizarModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

        LimpiaFormatos()

    End Sub

    'EVENTOS PARA PRESENTACIÓN DE DATOS EN FRONTEND
    Public Overrides Sub PreparaModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

        With documentoElectronico_

            If .Attribute(CamposPedimento.CA_T_OPER).Valor = 1 Then

                sch_TipoOperacion.Checked = True

            Else

                sch_TipoOperacion.Checked = False

            End If

        End With

    End Sub

    'EVENTOS DE MANTENIMIENTO
    Public Overrides Sub LimpiaSesion()

        'Lógica para limpiar sesiones que sean necearías

        'SetVars("ConstructorTarjetas", Nothing)

    End Sub

    Public Overrides Sub Limpiar()

        CatalogoTasas.DataSource = Nothing
        CatalogoCuadroLiquidacion.DataSource = Nothing

        CatalogoGuias.DataSource = Nothing

    End Sub

    Private Sub LimpiaFormatos()

        ' ** * ** * Generales * ** * **
        ic_TipoCambio.Value = ic_TipoCambio.Value.Replace("$", "")
        ic_ValorDolares.Value = ic_ValorDolares.Value.Replace("$", "")
        ic_ValorAduana.Value = ic_ValorAduana.Value.Replace("$", "")
        ic_PrecioPagado.Value = ic_PrecioPagado.Value.Replace("$", "")
        ' ** * ** * Generales * ** * **

        ' ** * ** * Datos del importador/exportador * ** * **
        ic_ValorSeguros.Value = ic_ValorSeguros.Value.Replace("$", "")
        ic_Seguros.Value = ic_Seguros.Value.Replace("$", "")
        ic_Fletes.Value = ic_Fletes.Value.Replace("$", "")
        ic_Embalajes.Value = ic_Embalajes.Value.Replace("$", "")
        ic_OtrosIncrementables.Value = ic_OtrosIncrementables.Value.Replace("$", "")

        ic_TransporteDec.Value = ic_TransporteDec.Value.Replace("$", "")
        ic_SegurosDec.Value = ic_SegurosDec.Value.Replace("$", "")
        ic_CargaDec.Value = ic_CargaDec.Value.Replace("$", "")
        ic_DescargaDec.Value = ic_DescargaDec.Value.Replace("$", "")
        ic_OtrosDec.Value = ic_OtrosDec.Value.Replace("$", "")
        ' ** * ** * Datos del importador/exportador * ** * **

        ' ** * ** * DatosProveedoresImpo * ** * **
        ic_MontoFacturaProveedor.Value = ic_MontoFacturaProveedor.Value.Replace("$", "")
        ic_MontoFacturaUSDProveedor.Value = ic_MontoFacturaUSDProveedor.Value.Replace("$", "")
        ' ** * ** * DatosProveedoresImpo * ** * **

        ' ** * ** * CuadroLiquidacion * ** * **
        ic_CuadroLiquidacionEfectivo.Value = ic_CuadroLiquidacionEfectivo.Value.Replace("$", "")
        ic_CuadroLiquidacionOtros.Value = ic_CuadroLiquidacionOtros.Value.Replace("$", "")
        ic_CuadroLiquidacionTotal.Value = ic_CuadroLiquidacionTotal.Value.Replace("$", "")
        ' ** * ** * CuadroLiquidacion * ** * **

        ' ** * ** * CuentasAduaneras * ** * **
        ic_ImporteCuentaAduanera.Value = ic_ImporteCuentaAduanera.Value.Replace("$", "")
        ic_PrecioEstimadoCuentaAduanera.Value = ic_PrecioEstimadoCuentaAduanera.Value.Replace("$", "")
        ic_ValorUnitario.Value = ic_ValorUnitario.Value.Replace("$", "")
        ' ** * ** * CuentasAduaneras * ** * **

        ' ** * ** * Partidas * ** * **
        ic_PartidaValorAduana.Value = ic_PartidaValorAduana.Value.Replace("$", "")
        ic_PartidaPrecioPagado.Value = ic_PartidaPrecioPagado.Value.Replace("$", "")
        ic_PartidaPrecioUnitario.Value = ic_PartidaPrecioUnitario.Value.Replace("$", "")
        ic_PartidaValorAgregado.Value = ic_PartidaValorAgregado.Value.Replace("$", "")
        ' ** * ** * Partidas * ** * **

        ic_LineaCapturaImporte.Value = ic_LineaCapturaImporte.Value.Replace("$", "")

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

        Dim lista_ As List(Of SelectOption) = controlador_.Buscar(fbc_Cliente.Text, New Filtro With {.IdSeccion = SeccionesClientes.SCS1, .IdCampo = CamposClientes.CA_RAZON_SOCIAL})

        fbc_Cliente.DataSource = lista_

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

        Dim TagWatcher_ = controlador_.ObtenerDocumento(fbc_Cliente.Value)

        If TagWatcher_.ObjectReturned IsNot Nothing Then

            Dim documentoCliente_ = TagWatcher_.ObjectReturned

            CargaCliente(documentoCliente_)

        End If

    End Sub

    Protected Sub sc_Patente_Click(sender As Object, e As EventArgs)
        sc_Patente.DataSource = CargaPatente()
    End Sub

    Public Function ConsultaPedimentito(ByVal FolioOper_ As String) As DocumentoElectronico

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

        Dim docElectronico_ As DocumentoElectronico = ConsultaPedimentito(FolioOperacion_)

        If docElectronico_ IsNot Nothing Then

            Using constructorPedimento = New ConstructorPedimentoNormalPDF(TiposDocumentoDigital.PedimentoNormalPDF,
                                                                        True, docElectronico_)

                Dim pdfstring = "data:Application/pdf;base64, " & constructorPedimento.ImpresionEncabezado(docElectronico_)

                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "openPDF('" & pdfstring & "','" & FolioOperacion_ & "')", True)

                'Dim iframe = "<iframe src='" & pdfstring & "' name='" & FolioOperacion_ & ".pdf' frameborder='0' style='border:0; top:0px; left:0px; bottom:0px; right:0px; width:100%; height:100%;position:absolute;' allowfullscreen></iframe>"

                'Dim func = "win = window.open() win.document.write(""" & iframe & """) win.document.title=""" & FolioOperacion_ & """"

                'ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", func, True)
                ' HttpUtility.UrlEncode(pdfstring)
            End Using

        End If



        'constructorPedimento.CrearArchivo(docElectronico)

        'Using _entidadDatos As IEntidadDatos =
        '            New ConstructorImpresionDocumento(TiposDocumentoDigital.PedimentoNormalPDF,
        '                                            True, docElectronico)

        'End Using

    End Sub

    Public Sub ImprimirMV(ByVal FolioOperacion_ As String)

        Dim docElectronico_ As DocumentoElectronico = ConsultaPedimentito(FolioOperacion_)

        If docElectronico_ IsNot Nothing Then

            Dim i = 1

            _manifestacionValor = New ControladorManifestacionValor(1)

            Dim _manifestacionesValor = _manifestacionValor.DescargarPDF(FolioOperacion_)

            For Each _manifestacionValorPDF As String In _manifestacionesValor

                Dim pdfstring = "data:Application/pdf;base64, " + _manifestacionValorPDF

                If i = 1 Then
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "openPDF('" & pdfstring & "','" & FolioOperacion_ & "HC" & "')", True)
                    i = 3
                Else
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "openPDF('" & pdfstring & "','" & FolioOperacion_ & "MV" & "')", True)
                End If

            Next

        End If


    End Sub

    Private Sub CargaCatalogos()

        sc_ClavePedimento.DataEntity = New Anexo22

        sc_Regimen.DataEntity = New Anexo22

        sc_DestinoMercancia.DataEntity = New Anexo22

        sc_AduanaEntradaSalida.DataEntity = New Anexo22

        sc_TransporteEntradaSalida.DataEntity = New Anexo22

        sc_MedioTransporteArribo.DataEntity = New Anexo22

        sc_MedioTransporteSalida.DataEntity = New Anexo22

        sc_TasasContribucion.DataEntity = New Anexo22

        sc_TasasTipoTasa.DataEntity = New Anexo22

        sc_CuadroLiquidacionConcepto.DataEntity = New Anexo22

        sc_CuadroLiquidacionFP.DataEntity = New Anexo22

        'sc_PaisTransporte.DataEntity = New Anexo22

        sc_TipoContenedor.DataEntity = New Anexo22

        sc_IdentificadorPedimento.DataEntity = New Anexo22

        sc_PagosVirtualesFormaPago.DataEntity = New Anexo22

        sc_RectificacionesClavePedOriginal.DataEntity = New Anexo22

        sc_RectificacionesAduanaOriginal.DataEntity = New Anexo22

        sc_RectificacionesAduana2Original.DataEntity = New Anexo22

        sc_RectificacionesClavePedimento.DataEntity = New Anexo22

        sc_DifConConcepto.DataEntity = New Anexo22

        sc_DifConFormaPago.DataEntity = New Anexo22

        sc_DescargosClavePedOriginal.DataEntity = New Anexo22

        sc_DescargosAduanaOriginal.DataEntity = New Anexo22

        sc_DescargosAduana2Original.DataEntity = New Anexo22

        sc_CompensacionesContribucion.DataEntity = New Anexo22

        sc_CompensacionesAduanaOriginal.DataEntity = New Anexo22

        sc_CompensacionesAduana2Original.DataEntity = New Anexo22

        sc_PruebaSuficientePaisDestino.DataEntity = New Anexo22

        sc_PartidaMetodoValoracion.DataEntity = New Anexo22

        sc_UMC.DataEntity = New Anexo22

        sc_UMT.DataEntity = New Anexo22

        sc_PaisVC.DataEntity = New Anexo22

        sc_PaisOD.DataEntity = New Anexo22

        sc_PartidaContribucion.DataEntity = New Anexo22

        sc_PartidaTipoTasa.DataEntity = New Anexo22

        sc_PartidaIdentificador.DataEntity = New Anexo22

        sc_EjecutivoCuenta.DataEntity = New krom.Ejecutivos

        sc_EjecutivoCuenta.FreeClauses = " and i_Cve_DivisionMiEmpresa = " & Statements.GetOfficeOnline()._id

    End Sub

    Private Sub PreparaControles()

        'Inicializa Tipo de Referencia
        sc_TipoReferencia.Value = 1

        'Inicizliza prefijo
        InicializaPrefijo(sc_TipoReferencia, sc_PrefijoReferencia)

        'GeneraPrefijoReferencia
        dbc_ReferenciaPedimento.Value = GeneraReferenciaPedimento(TipoSecuencia.Referencia, TipoFijo.Completo, sc_PrefijoReferencia)


    End Sub

    Private Sub CargaCliente(ByVal datosCliente_ As OperacionGenerica)

        ic_RFCCliente.Value = datosCliente_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Campo(CamposClientes.CA_RFC_CLIENTE).Valor

        ic_CURP.Value = datosCliente_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Campo(CamposClientes.CA_CURP_CLIENTE).Valor


        ic_RFCFacturacion.Value = datosCliente_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Campo(CamposClientes.CA_RFC_CLIENTE).Valor

        ic_DomicilioCliente.Value = datosCliente_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor

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

#End Region

End Class

Public MustInherit Class ClaseSinNombre
    Inherits ControladorBackend

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

End Class