Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports gsol
Imports gsol.Web.Components
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Rec.Globals
Imports Rec.Globals.Controllers
Imports Rec.Globals.Utils
Imports Sax.Web
Imports Syn.CustomBrokers.Controllers
Imports Syn.Documento
Imports Syn.Documento.Componentes.Campo
Imports Syn.Nucleo.Recursos.CamposClientes
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Nucleo.RecursosComercioExterior.CamposAcuseValor
Imports Syn.Nucleo.RecursosComercioExterior.CamposFacturaComercial
Imports Syn.Operaciones
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher
Imports Wma.Exceptions.TagWatcher.TypeStatus

Public Class Ges003_001_FacturasComerciales
    Inherits ControladorBackend

#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Private _monedas As Object

    Private _pais As Pais

    Private _controladorPaises As New ControladorPaises

    Private _controladorProveedor As New CtrlProveedoresOperativos

    Private _sistema As New Syn.Utils.Organismo

#End Region

#Region "██████ Vinculación c/capas inf █████████       SAX      ████████████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    'INICIALIZADOR Y BOTONERA
    Public Overrides Sub Inicializa()

        With Buscador

            .DataObject = New ConstructorFacturaComercial(True)

            .addFilter(SeccionesFacturaComercial.SFAC1, CamposFacturaComercial.CA_NUMERO_FACTURA, "Número factura")
            .addFilter(SeccionesFacturaComercial.SFAC1, CamposAcuseValor.CA_NUMERO_ACUSEVALOR, "Acuse de Valor")
            .addFilter(SeccionesFacturaComercial.SFAC2, CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, "Proveedor")

        End With

        If Not Page.IsPostBack Then

            Session("_pbPartidas") = PillboxControl.ToolbarModality.Default

        End If

        pbPartidas.Modality = Session("_pbPartidas")

        '  Generales

        fbcIncoterm.DataEntity = New krom.Anexo22()



        ' Datos del proveedor

        scMetodoValoracion.DataEntity = New krom.Anexo22()

        ' Partidas

        scMetodoValoracionPartida.DataEntity = New krom.Anexo22()

    End Sub

    Public Overrides Sub BotoneraClicNuevo()

        If OperacionGenerica IsNot Nothing Then

            '_empresa = Nothing

        End If

        PreparaTarjetero(PillboxControl.ToolbarModality.Simple, pbPartidas)

        Habilitar()

    End Sub

    Public Overrides Sub BotoneraClicGuardar()

        If Not ProcesarTransaccion(Of ConstructorFacturaComercial)().Status = TypeStatus.Errors Then : End If

    End Sub

    Public Overrides Sub BotoneraClicEditar()

        PreparaTarjetero(PillboxControl.ToolbarModality.Advanced, pbPartidas)
        Habilitar()

    End Sub

    Public Overrides Sub BotoneraClicBorrar()



    End Sub

    Public Overrides Sub BotoneraClicOtros(ByVal IndexSelected_ As Integer)



    End Sub

    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher

        'Datos Generales
        'Case SeccionesFacturaComercial.SFAC1
        '                                     Item(CamposFacturaComercial.CA_NUMERO_FACTURA, Texto, longitud_:=40)
        [Set](dbcNumFacturaCOVE, CA_NUMERO_FACTURA, propiedadDelControl_:=PropiedadesControl.Valor)
        '                                     Item(CamposCOVE.CA_NUMERO_COVE, Texto, longitud_:=40)
        [Set](dbcNumFacturaCOVE, CA_NUMERO_AcuseValor, propiedadDelControl_:=PropiedadesControl.ValueDetail)
        '                                     Item(CamposFacturaComercial.CA_FECHA_FACTURA, Fecha)
        [Set](icFechaFactura, CA_FECHA_FACTURA)
        '                                     Item(CamposCOVE.CA_FECHA_COVE, Fecha)
        [Set](icFechaCOVE, CA_FECHA_AcuseValor)
        '                                     Item(CamposClientes.CA_RAZON_SOCIAL, Texto, longitud_:=120)
        [Set](fbcCliente, CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Text)
        '                                     Item(CamposClientes.CA_TAX_ID, Texto, longitud_:=11)
        '                                     Item(CamposClientes.CA_RFC_CLIENTE, Texto, longitud_:=13)
        '                                     Item(CamposDomicilio.CA_DOMICILIO_FISCAL, Texto, longitud_:=450)
        '                                     Item(CamposDomicilio.CA_CALLE, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_NUMERO_EXTERIOR, Texto, longitud_:=10)
        '                                     Item(CamposDomicilio.CA_NUMERO_INTERIOR, Texto, longitud_:=10)
        '                                     Item(CamposDomicilio.CA_NUMERO_EXT_INT, Texto, longitud_:=20)
        '                                     Item(CamposDomicilio.CA_CODIGO_POSTAL, Texto, longitud_:=10)
        '                                     Item(CamposDomicilio.CA_COLONIA, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_LOCALIDAD, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_CIUDAD, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_MUNICIPIO, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA, Texto, longitud_:=3)
        '                                     Item(CamposDomicilio.CA_ENTIDAD_FEDERATIVA, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_ENTIDAD_MUNICIPIO, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_CVE_PAIS, Texto, longitud_:=3)
        '                                     Item(CamposDomicilio.CA_PAIS, Texto, longitud_:=80)
        '                                     Item(CamposFacturaComercial.CA_CVE_PAIS_FACTURACION, Texto, longitud_:=3)
        [Set](fbcPais, CA_CVE_PAIS_FACTURACION)
        '                                     Item(CamposFacturaComercial.CA_PAIS_FACTURACION, Texto, longitud_:=80)
        [Set](fbcPais, CA_PAIS_FACTURACION, propiedadDelControl_:=PropiedadesControl.Text)
        '                                     Item(CamposFacturaComercial.CP_TIPO_OPERACION, Texto, longitud_:=11)
        [Set](IIf(swcTipoOperacion.Checked, "Importación", "Exportación"), CamposFacturaComercial.CP_TIPO_OPERACION)
        '                                     Item(CamposFacturaComercial.CA_CVE_INCOTERM, Texto, longitud_:=3)
        [Set](fbcIncoterm, CA_CVE_INCOTERM)

        [Set](fbcIncoterm, CA_CVE_INCOTERM, propiedadDelControl_:=PropiedadesControl.Text, asignarA_:=TiposAsignacion.ValorPresentacion)
        '                                     Item(CamposFacturaComercial.CP_VALOR_FACTURA, Real, cantidadEnteros_:=14, cantidadDecimales_:=2)
        [Set](icValorFactura, CP_VALOR_FACTURA)
        '                                     Item(CamposFacturaComercial.CA_MONEDA_FACTURACION, Texto, longitud_:=3)
        [Set](scMonedaFactura, CA_MONEDA_FACTURACION)
        '                                     Item(CamposFacturaComercial.CP_VALOR_MERCANCIA, Real, cantidadEnteros_:=14, cantidadDecimales_:=2)
        [Set](icValorMercancia, CP_VALOR_MERCANCIA)
        '                                     Item(CamposFacturaComercial.CP_MONEDA_VALOR_MERCANCIA, Texto, longitud_:=3)
        [Set](scMonedaMercancia, CP_MONEDA_VALOR_MERCANCIA)
        '                                     Item(CamposFacturaComercial.CP_PESO_TOTAL, Real, cantidadEnteros_:=14, cantidadDecimales_:=3)
        [Set](icPesoTotal, CP_PESO_TOTAL)
        '                                     Item(CamposFacturaComercial.CP_APLICA_ENAJENACION, Booleano)
        [Set](IIf(swcEnajenacion.Checked, True, False), CP_APLICA_ENAJENACION)
        '                                     Item(CamposFacturaComercial.CA_APLICA_SUBDIVISION, Entero), ' Preguntar que significa el siguiente paramentro en los Enteros
        [Set](IIf(swcSubdivision.Checked, 2, 1), CA_APLICA_SUBDIVISION, tipoDato_:=TiposDato.Entero)
        '                                     Item(CamposFacturaComercial.CP_SERIE_FOLIO_FACTURA, Texto, longitud_:=100)
        [Set](icSerieFolioFactura, CP_SERIE_FOLIO_FACTURA)
        '                                     Item(CamposFacturaComercial.CP_ORDEN_COMPRA, Texto, longitud_:=60)
        [Set](fbcOrdenCompra, CP_ORDEN_COMPRA, propiedadDelControl_:=PropiedadesControl.Text)

        'Datos del proveedor
        'Case SeccionesFacturaComercial.SFAC2
        '                                     Item(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, Texto, longitud_:=120)
        [Set](fbcProveedor, CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Text)
        '                                     Item(CamposDomicilio.CA_DOMICILIO_FISCAL, Texto, longitud_:=450)
        [Set](scDomiciliosProveedor, CamposDomicilio.CA_DOMICILIO_FISCAL)
        '                                     Item(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR, Texto, longitud_:=11)
        '                                     Item(CamposProveedorOperativo.CA_RFC_PROVEEDOR, Texto, longitud_:=13)
        '                                     Item(CamposDomicilio.CA_CALLE, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_NUMERO_EXTERIOR, Texto, longitud_:=10)
        '                                     Item(CamposDomicilio.CA_NUMERO_INTERIOR, Texto, longitud_:=10)
        '                                     Item(CamposDomicilio.CA_NUMERO_EXT_INT, Texto, longitud_:=20)
        '                                     Item(CamposDomicilio.CA_CODIGO_POSTAL, Texto, longitud_:=10)
        '                                     Item(CamposDomicilio.CA_COLONIA, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_LOCALIDAD, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_CIUDAD, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_MUNICIPIO, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA, Texto, longitud_:=3)
        '                                     Item(CamposDomicilio.CA_ENTIDAD_FEDERATIVA, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_CVE_PAIS, Texto, longitud_:=3)
        '                                     Item(CamposDomicilio.CA_PAIS, Texto, longitud_:=80)
        '                                     Item(CamposFacturaComercial.CA_CVE_VINCULACION, Entero)
        [Set](scVinculacion, CA_CVE_VINCULACION)
        '                                     Item(CamposFacturaComercial.CP_CVE_METODO_VALORACION, Entero)
        [Set](scMetodoValoracion, CP_CVE_METODO_VALORACION)
        '                                     Item(CamposFacturaComercial.CA_APLICA_CERTIFICADO, Entero)
        [Set](IIf(swcFungeCertificado.Checked, 2, 1), CA_APLICA_SUBDIVISION, tipoDato_:=TiposDato.Entero)
        '                                     Item(CamposFacturaComercial.CP_NOMBRE_CERTIFICADOR, Texto, longitud_:=120)
        [Set](fbcProveedorCertificado, CP_NOMBRE_CERTIFICADOR, propiedadDelControl_:=PropiedadesControl.Text)

        'Datos del destinatario
        'Case SeccionesFacturaComercial.SFAC3
        '                                     Item(CamposDestinatario.CA_RAZON_SOCIAL, Texto, longitud_:=120)
        [Set](fbcDestinatario, CamposDestinatario.CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Text)
        '                                     Item(CamposDestinatario.CA_TAX_ID, Texto, longitud_:=11)
        '                                     Item(CamposDestinatario.CA_RFC_DESTINATARIO, Texto, longitud_:=13)
        '                                     Item(CamposDomicilio.CA_DOMICILIO_FISCAL, Texto, longitud_:=450)
        '                                     Item(CamposDomicilio.CA_CALLE, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_NUMERO_EXTERIOR, Texto, longitud_:=10)
        '                                     Item(CamposDomicilio.CA_NUMERO_INTERIOR, Texto, longitud_:=10)
        '                                     Item(CamposDomicilio.CA_CODIGO_POSTAL, Texto, longitud_:=10)
        '                                     Item(CamposDomicilio.CA_COLONIA, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_LOCALIDAD, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_CIUDAD, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_MUNICIPIO, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA, Texto, longitud_:=3)
        '                                     Item(CamposDomicilio.CA_ENTIDAD_FEDERATIVA, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_CVE_PAIS, Texto, longitud_:=3)
        '                                     Item(CamposDomicilio.CA_PAIS, Texto, longitud_:=80)

        ' Partidas - Factura
        'Case SeccionesFacturaComercial.SFAC5
        '                                     Item(CamposFacturaComercial.CP_NUMERO_PARTIDA, Entero)
        '[Set](lbNumero, CP_NUMERO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        If pbPartidas.PageIndex > 0 Then

            lbNumero.Text = pbPartidas.PageIndex.ToString()

        End If
        '                                     Item(CamposFacturaComercial.CA_NUMERO_PARTE_PARTIDA, Texto, longitud_:=20)
        [Set](fbcProducto, CA_NUMERO_PARTE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_VALOR_FACTURA_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5)
        [Set](icValorfacturaPartida, CA_VALOR_FACTURA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CP_MONEDA_FACTURA_PARTIDA, Texto, longitud_:=3)
        [Set](scMonedaFacturaPartida, CP_MONEDA_FACTURA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_VALOR_MERCANCIA_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5)
        [Set](icValorMercanciaPartida, CA_VALOR_MERCANCIA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_MONEDA_MERCANCIA_PARTIDA, Texto, longitud_:=3)
        [Set](scMonedaMercanciaPartida, CA_MONEDA_MERCANCIA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_CVE_METODO_VALORACION_PARTIDA, Texto, longitud_:=1)
        [Set](scMetodoValoracionPartida, CA_CVE_METODO_VALORACION_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_PESO_NETO_PARTIDA, Real, cantidadEnteros_:=14, cantidadDecimales_:=3)
        [Set](icPesoNeto, CA_PESO_NETO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_PRECIO_UNITARIO_PARTIDA, Real, cantidadEnteros_:=15, cantidadDecimales_:=5)
        [Set](icPrecioUnitario, CA_PRECIO_UNITARIO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CP_MONEDA_PRECIO_UNITARIO, Texto, longitud_:=3)
        [Set](scMonedaPrecioUnitarioPartida, CP_MONEDA_PRECIO_UNITARIO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_PAIS_ORIGEN_PARTIDA, Texto, longitud_:=3)
        [Set](fbcPaisPartida, CA_PAIS_ORIGEN_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CP_CANTIDAD_FACTURA_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5)
        '[Set](icCantidadComercial, CP_CANTIDAD_FACTURA_PARTIDA)
        '                                     Item(CamposFacturaComercial.CP_UNIDAD_MEDIDA_FACTURA_PARTIDA, Entero)
        '[Set](scUnidadMedidaComercial, CP_UNIDAD_MEDIDA_FACTURA_PARTIDA)
        '                                     Item(CamposFacturaComercial.CA_DESCRIPCION_PARTE_PARTIDA, Texto, longitud_:=250)
        [Set](icDescripcionPartida, CA_DESCRIPCION_PARTE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CP_APLICA_DESCRIPCION_COVE_PARTIDA, Booleano)
        [Set](swcAplicaCOVE, CP_APLICA_DESCRIPCION_COVE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_CANTIDAD_COMERCIAL_PARTIDA, Real, cantidadEnteros_:=15, cantidadDecimales_:=3)
        [Set](icCantidadComercial, CA_CANTIDAD_COMERCIAL_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_UNIDAD_MEDIDA_COMERCIAL_PARTIDA, Entero)
        [Set](scUnidadMedidaComercial, CA_UNIDAD_MEDIDA_COMERCIAL_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_DESCRIPCION_COVE_PARTIDA, Texto, longitud_:=250)
        [Set](icDescripcionCOVE, CA_DESCRIPCION_COVE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CP_ORDEN_COMPRA_PARTIDA, Texto, longitud_:=60)
        [Set](fbcOrdenCompraPartida, CP_ORDEN_COMPRA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)


        ' Partida - clasificación
        'Case SeccionesFacturaComercial.SFAC6
        '                                     Item(CamposFacturaComercial.CA_FRACCION_ARANCELARIA_PARTIDA, Texto, longitud_:=8)
        [Set](fbcFraccionArancelaria, CA_FRACCION_ARANCELARIA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_CANTIDAD_TARIFA_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5)
        [Set](icCantidadTarifa, CA_CANTIDAD_TARIFA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_UNIDAD_MEDIDA_TARIFA_PARTIDA, Entero)
        [Set](scUnidadMedidaTarifa, CA_UNIDAD_MEDIDA_TARIFA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_FRACCION_NICO_PARTIDA, Texto, longitud_:=2)
        [Set](fbcFraccionNico, CA_FRACCION_NICO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        ' Partida - detalle mercancía
        'Case SeccionesFacturaComercial.SFAC7
        '                                     Item(CamposFacturaComercial.CA_LOTE_PARTIDA, Texto, longitud_:=80)
        [Set](icLote, CA_LOTE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_NUMERO_SERIE_PARTIDA, Texto, longitud_:=25)
        [Set](icNumeroSerie, CA_NUMERO_SERIE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_MARCA_PARTIDA, Texto, longitud_:=80)
        [Set](icMarca, CA_MARCA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_MODELO_PARTIDA, Texto, longitud_:=80)
        [Set](icModelo, CA_MODELO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_SUBMODELO_PARTIDA, Texto, longitud_:=80)
        [Set](icSubmodelo, CA_SUBMODELO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_KILOMETRAJE_PARTIDA, Entero)
        [Set](icKilometraje, CA_KILOMETRAJE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](pbPartidas, Nothing, seccion_:=SeccionesFacturaComercial.SFAC4)

        ' Incrementables
        'Case SeccionesFacturaComercial.SFAC8
        '                                     Item(CamposFacturaComercial.CA_FLETES, Real, cantidadEnteros_:=18, cantidadDecimales_:=5)
        [Set](icFletes, CA_FLETES)
        '                                     Item(CamposFacturaComercial.CA_MONEDA_FLETES, Texto, longitud_:=3)
        [Set](scMonedaFletes, CA_MONEDA_FLETES)
        '                                     Item(CamposFacturaComercial.CA_SEGURO, Real, cantidadEnteros_:=18, cantidadDecimales_:=5)
        [Set](icSeguros, CA_SEGURO)
        '                                     Item(CamposFacturaComercial.CA_MONEDA_SEGUROS, Texto, longitud_:=3)
        [Set](scMonedaSeguros, CA_MONEDA_SEGUROS)
        '                                     Item(CamposFacturaComercial.CA_EMBALAJES, Real, cantidadEnteros_:=18, cantidadDecimales_:=5)
        [Set](icEmbalajes, CA_EMBALAJES)
        '                                     Item(CamposFacturaComercial.CA_MONEDA_EMBALAJES, Texto, longitud_:=3)
        [Set](scMonedaEmbalajes, CA_MONEDA_EMBALAJES)
        '                                     Item(CamposFacturaComercial.CA_OTROS_INCREMENTABLES, Real, cantidadEnteros_:=18, cantidadDecimales_:=5)
        [Set](icOtrosIncrementables, CA_OTROS_INCREMENTABLES)
        '                                     Item(CamposFacturaComercial.CA_MONEDA_OTROS_INCREMENTABLES, Texto, longitud_:=3)
        [Set](scMonedaOtrosIncrementables, CA_MONEDA_OTROS_INCREMENTABLES)
        '                                     Item(CamposFacturaComercial.CA_DESCUENTOS, Real, cantidadEnteros_:=18, cantidadDecimales_:=5)
        [Set](icDescuentos, CA_DESCUENTOS)
        '                                     Item(CamposFacturaComercial.CA_MONEDA_DESCUENTOS, Texto, longitud_:=3)
        [Set](scMonedaDescuentos, CA_MONEDA_DESCUENTOS)

        Return New TagWatcher(1)

    End Function

    'EVENTOS PARA LA INSERCIÓN DE DATOS
    Public Overrides Function AntesRealizarInsercion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As TagWatcher

        '      ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒
        If session_ IsNot Nothing Then

            '  ██████inicio███████        Logica de negocios local      ████████████████████████



            '  ████████fin█████████       Logica de negocios local       ███████████████████████

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()


        Else  '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas sin transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()

        End If

        Return tagwatcher_

    End Function

    Public Overrides Sub RealizarInsercion(ByRef documentoElectronico_ As DocumentoElectronico)

        Dim secuencia_ As New Secuencia _
              With {.anio = 0,
                    .environment = Statements.GetOfficeOnline()._id,
                    .mes = 0,
                    .nombre = "FacturasComerciales",
                    .tiposecuencia = 1,
                    .subtiposecuencia = 0
                    }

        Dim respuesta_ As TagWatcher = secuencia_.Generar().Result

        Dim sec_ As Int32 = 0

        Select Case respuesta_.Status

            Case TypeStatus.Ok

                sec_ = respuesta_.ObjectReturned.sec

            Case Else

        End Select

        With documentoElectronico_

            .FolioDocumento = dbcNumFacturaCOVE.Value

            .FolioOperacion = sec_

            .IdCliente = 0

            .NombreCliente = fbcCliente.Text

        End With

    End Sub

    Public Overrides Function DespuesRealizarInsercion() As TagWatcher

        Return New TagWatcher(Ok)

    End Function

    'EVENTOS PARA LA MODIFICACIÓN DE DATOS
    Public Overrides Function AntesRealizarModificacion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As TagWatcher

        '     ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒

        If session_ IsNot Nothing Then

            '  ██████inicio███████        Logica de negocios local      ████████████████████████

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()


        Else  '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas sin transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()

        End If

        Return tagwatcher_

    End Function

    Public Overrides Sub RealizarModificacion(ByRef documentoElectronico_ As DocumentoElectronico)



    End Sub

    Public Overrides Function DespuesRealizarModificacion() As TagWatcher

        ' Acciones después de realizar la modificación exitosamente

        Return New TagWatcher(Ok)

    End Function

    'EVENTOS PARA PRESENTACIÓN DE DATOS EN FRONTEND
    Public Overrides Sub PreparaModificacion(ByRef documentoElectronico_ As DocumentoElectronico)



    End Sub

    'EVENTO PARA BÚSQUEDA
    Public Overrides Sub DespuesBuquedaGeneralConDatos()

        PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbPartidas)

    End Sub

    Public Overrides Sub DespuesBuquedaGeneralSinDatos()

        PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbPartidas)

    End Sub

    'EVENTOS DE MANTENIMIENTO
    Public Overrides Sub LimpiaSesion()

        'Lógica para limpiar sesiones que sean necearías

    End Sub

    Public Overrides Sub Limpiar()



    End Sub

#End Region

#Region "████████████████  QUINTA CAPA  █████████       Reglas locales         ██████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    'EVENTO PARA CONTROLAR QUE PASA CON EL TIPO DE OPERACIÓN
    Protected Sub swcTipoOperacion_CheckedChanged(sender As Object, e As EventArgs)

        If swcTipoOperacion.Checked Then ' Importación

            fscDestinatario.Visible = False

        Else ' Exportación

            fscDestinatario.Visible = True

        End If

    End Sub

#End Region

#Region "██████ Vinculación sexta capa  █████████       SAX      ████████████████████████████████████████████"
    '    ██████   Controladores utilizados                     Documentos por coding para MongoDB      ██████
    '    ██████    1.ControladorRecursosAduanales                                                      ██████
    '    ██████    2.ControladorSecuencias                                                             ██████
    '    ██████                                                                                        ██████
    '    ██████                                                                                        ██████
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Protected Sub fbcCliente_TextChanged(sender As Object, e As EventArgs)

        Dim controlador_ As New ControladorBusqueda(Of ConstructorCliente)

        Dim lista_ As List(Of SelectOption) = controlador_.Buscar(fbcCliente.Text, New Filtro With {.IdSeccion = SeccionesClientes.SCS1, .IdCampo = CamposClientes.CA_RAZON_SOCIAL})

        fbcCliente.DataSource = lista_

    End Sub

    Protected Sub fbcCliente_Click(sender As Object, e As EventArgs)

        'aquí no se que va tal vez la preparación de los datos internos a guardar del cliente

    End Sub

    Protected Sub pbPartidas_CheckedChange(sender As Object, e As EventArgs)

    End Sub

    Protected Sub pbPartidas_Click(sender As Object, e As EventArgs)

        Select Case pbPartidas.ToolbarAction

            Case PillboxControl.ToolbarActions.Nuevo
                If GetVars("_pais") IsNot Nothing Then

                    Dim monedas_ = ControladorPaises.BuscarMonedasOficiales(GetVars("_pais"))

                    scMonedaFacturaPartida.DataSource = monedas_
                    scMonedaMercanciaPartida.DataSource = monedas_
                    scMonedaPrecioUnitarioPartida.DataSource = monedas_

                    scMonedaFacturaPartida.Value = monedas_(0).Value
                    scMonedaMercanciaPartida.Value = monedas_(0).Value
                    scMonedaPrecioUnitarioPartida.Value = monedas_(0).Value

                End If


            Case PillboxControl.ToolbarActions.Borrar

            Case PillboxControl.ToolbarActions.Archivar

            Case Else

        End Select

    End Sub

    Protected Sub fbcProveedor_TextChanged(sender As Object, e As EventArgs)

        Dim controlador_ As New CtrlProveedoresOperativos()

        controlador_._tipoOperacion = IIf(swcTipoOperacion.Checked, CtrlProveedoresOperativos.TipoOperacion.Importacion, CtrlProveedoresOperativos.TipoOperacion.Exportacion)

        Dim tagwatcher_ = controlador_.BuscarProveedores(sender.Text, False)

        Dim lista_ = controlador_.ToSelectOption(tagwatcher_)

        sender.DataSource = lista_

    End Sub

    Protected Sub fbcProveedor_Click(sender As Object, e As EventArgs)

        _controladorProveedor = New CtrlProveedoresOperativos()

        _controladorProveedor._tipoOperacion = IIf(swcTipoOperacion.Checked,
                                                   CtrlProveedoresOperativos.TipoOperacion.Importacion, CtrlProveedoresOperativos.TipoOperacion.Exportacion)

        Dim tagwatcher_ = _controladorProveedor.BuscarProveedor(New ObjectId(sender.Value.ToString))

        SetVars("_proveedor", tagwatcher_)

        'Aquí buscamos al proveedor y su dirección
        Select Case sender.ID
            Case "fbcProveedor"

                scDomiciliosProveedor.DataSource = _controladorProveedor.BuscarDomicilios(tagwatcher_.ObjectReturned.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente)

            Case "fbcProveedorCertificado"


            Case "fbcDestinatario"

                Dim domicilio_ = _controladorProveedor.BuscarDomicilios(tagwatcher_.ObjectReturned.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente)

                If domicilio_ IsNot Nothing Then

                    Dim domicilio As String = Mid(domicilio_(0).Text, InStrRev(domicilio_(0).Text, "|") + 2,
                                                  domicilio_(0).Text.ToString().Length - InStrRev(domicilio_(0).Text, "|"))

                    icDomicilioDestinario.Value = domicilio

                End If

            Case Else

        End Select

    End Sub

    Protected Sub scVinculacion_Click(sender As Object, e As EventArgs)

        scVinculacion.DataSource = Vinculacion()

    End Sub

    Private Function Vinculacion() As List(Of SelectOption)

        Dim recursos_ As ControladorRecursosAduanalesGral = ControladorRecursosAduanalesGral.Buscar(ControladorRecursosAduanalesGral.TiposRecurso.Anexo22)

        Dim vinculaciones_ = From data In recursos_.tiposvinculacion
                             Where data.archivado = False And data.estado = 1
                             Select data._idvinculacion, data.descripcion, data.descripcioncorta

        If vinculaciones_.Count > 0 Then

            Dim dataSource1_ As New List(Of SelectOption)

            For index_ As Int32 = 0 To vinculaciones_.Count - 1

                dataSource1_.Add(New SelectOption With
                             {.Value = vinculaciones_(index_)._idvinculacion,
                              .Text = vinculaciones_(index_)._idvinculacion.ToString & " - " & vinculaciones_(index_).descripcioncorta})

            Next

            Return dataSource1_

        End If

        Return Nothing

    End Function

    Sub Habilitar()

        icValorfacturaPartida.Enabled = True
        scMonedaFacturaPartida.Enabled = True
        icValorMercanciaPartida.Enabled = True
        scMonedaMercanciaPartida.Enabled = True
        scMetodoValoracionPartida.Enabled = True
        icPesoNeto.Enabled = True
        icPrecioUnitario.Enabled = True
        scMonedaPrecioUnitarioPartida.Enabled = True
        icDescripcionPartida.Enabled = True
        swcAplicaCOVE.Enabled = True
        icCantidadComercial.Enabled = True
        scUnidadMedidaComercial.Enabled = True
        icDescripcionCOVE.Enabled = True
        icCantidadTarifa.Enabled = True
        scUnidadMedidaTarifa.Enabled = True
        icLote.Enabled = True
        icNumeroSerie.Enabled = True
        icMarca.Enabled = True
        icModelo.Enabled = True
        icSubmodelo.Enabled = True
        icKilometraje.Enabled = True

    End Sub

    Function CargaPaises(ByRef control_ As FindboxControl) As List(Of SelectOption)

        Dim paisesTemporales_ As New List(Of Pais)

        Dim lista_ As List(Of SelectOption) = ControladorPaises.BuscarPaises(paisesTemporales_, control_.Text)

        SetVars("_paisesTemporal", paisesTemporales_)

        control_.DataSource = lista_

        Return lista_

    End Function

    Protected Sub dbcNumFacturaCOVE_Click(sender As Object, e As EventArgs)

        'dbcNumFacturaCOVE.ValueDetail = "pollito"
        DisplayMessage("Aquí usted vera su COVE o eso creo", StatusMessage.Info)

    End Sub

    Protected Sub fbcPais_TextChanged(sender As Object, e As EventArgs)

        If Not fbcPais.Text.Contains("-") Then

            Dim lista_ = CargaPaises(sender)

            fbcPaisPartida.DataSource = lista_

        End If

    End Sub

    Protected Sub fbcPaisPartida_TextChanged(sender As Object, e As EventArgs)

        CargaPaises(sender)

    End Sub

    Protected Sub fbcPais_Click(sender As Object, e As EventArgs)

        If fbcPais.Value <> "" Then

            _pais = ControladorPaises.BuscarPais(New ObjectId(fbcPais.Value))

            If _pais IsNot Nothing Then

                SetVars("_pais", _pais)

                Dim monedas_ = ControladorPaises.BuscarMonedasOficiales(_pais)

                scMonedaFactura.DataSource = monedas_
                scMonedaMercancia.DataSource = monedas_
                scMonedaFacturaPartida.DataSource = monedas_
                scMonedaMercanciaPartida.DataSource = monedas_
                scMonedaPrecioUnitarioPartida.DataSource = monedas_
                scMonedaFletes.DataSource = monedas_
                scMonedaSeguros.DataSource = monedas_
                scMonedaEmbalajes.DataSource = monedas_
                scMonedaOtrosIncrementables.DataSource = monedas_
                scMonedaDescuentos.DataSource = monedas_

                If monedas_.Count > 0 Then

                    scMonedaFactura.Value = monedas_(0).Value
                    scMonedaMercancia.Value = monedas_(0).Value
                    scMonedaFacturaPartida.Value = monedas_(0).Value
                    scMonedaMercanciaPartida.Value = monedas_(0).Value
                    scMonedaPrecioUnitarioPartida.Value = monedas_(0).Value
                    scMonedaFletes.Value = monedas_(0).Value
                    scMonedaSeguros.Value = monedas_(0).Value
                    scMonedaEmbalajes.Value = monedas_(0).Value
                    scMonedaOtrosIncrementables.Value = monedas_(0).Value
                    scMonedaDescuentos.Value = monedas_(0).Value

                End If

            End If

        Else

            scMonedaFactura.Options.Clear()
            scMonedaMercancia.Options.Clear()
            scMonedaFletes.Options.Clear()
            scMonedaSeguros.Options.Clear()
            scMonedaEmbalajes.Options.Clear()
            scMonedaOtrosIncrementables.Options.Clear()
            scMonedaDescuentos.Options.Clear()

            scMonedaFactura.DataSource = Nothing
            scMonedaMercancia.DataSource = Nothing
            scMonedaFletes.DataSource = Nothing
            scMonedaSeguros.DataSource = Nothing
            scMonedaEmbalajes.DataSource = Nothing
            scMonedaOtrosIncrementables.DataSource = Nothing
            scMonedaDescuentos.DataSource = Nothing

        End If

    End Sub

    Protected Sub fbcPaisPartida_Click(sender As Object, e As EventArgs)

        If fbcPaisPartida.Value <> "" Then

            _pais = ControladorPaises.BuscarPais(New ObjectId(fbcPaisPartida.Value))

            If _pais IsNot Nothing Then

                SetVars("_pais", _pais)

            End If

        End If

    End Sub

    Function BusquedaMonedas(ByRef control_ As SelectControl) As List(Of SelectOption)

        Dim listaMonedas_ = ControladorPaises.BuscarTodasMonedas(control_.SuggestedText)

        If listaMonedas_.Count > 0 Then

            control_.DataSource = listaMonedas_

        End If

        Return listaMonedas_

    End Function

    Protected Sub SeleccionarMoneda_Click(sender As Object, e As EventArgs)

        'If fbcPais.Value IsNot Nothing Then

        '    If sender.Value <> "" Then

        '        Dim valor_ = sender.Value

        '    End If

        'End If

    End Sub

    Protected Sub swcFungeCertificado_CheckedChanged(sender As Object, e As EventArgs)

        If swcFungeCertificado.Checked Then

            fbcProveedorCertificado.Enabled = False
            fbcProveedorCertificado.Value = ""

        Else

            fbcProveedorCertificado.Enabled = True

        End If

    End Sub

    Protected Sub swcAplicaCOVE_CheckedChanged(sender As Object, e As EventArgs)

        If swcAplicaCOVE.Checked And icDescripcionPartida.Value <> "" Then

            icDescripcionCOVE.Enabled = False

            icDescripcionCOVE.Value = icDescripcionPartida.Value

        Else

            icDescripcionCOVE.Enabled = True

        End If

    End Sub

    Protected Sub CargaMoneda_TextChanged(sender As Object, e As EventArgs)

        If fbcPais.Value IsNot Nothing Then

            BusquedaMonedas(sender)

        End If

        'sender.DataSource = _monedas

    End Sub

    Protected Sub scUnidadMedidaComercial_TextChanged(sender As Object, e As EventArgs)

        CargaUnidades(scUnidadMedidaComercial, ControladorUnidadesMedida.TiposUnidad.Comercial)

    End Sub

    Sub CargaUnidades(ByRef control_ As SelectControl, ByVal tipoUnidad_ As ControladorUnidadesMedida.TiposUnidad, Optional ByVal top_ As Int32 = 0)

        Dim lista_ As List(Of UnidadMedida) = ControladorUnidadesMedida.BuscarUnidades(tipoUnidad_, control_.SuggestedText, top_)

        If lista_.Count > 0 Then

            control_.DataSource = ControladorUnidadesMedida.ToSelectOption(lista_, ControladorUnidadesMedida.TipoSelectOption.CveMXnombreoficiales)

        End If

    End Sub

    Protected Sub scUnidadMedidaTarifa_Click(sender As Object, e As EventArgs)

        CargaUnidades(scUnidadMedidaTarifa, ControladorUnidadesMedida.TiposUnidad.Comercial, 5)

    End Sub

    Protected Sub scUnidadMedidaTarifa_TextChanged(sender As Object, e As EventArgs)

        CargaUnidades(scUnidadMedidaTarifa, ControladorUnidadesMedida.TiposUnidad.Comercial)

    End Sub

    Protected Sub scUnidadMedidaComercial_Click(sender As Object, e As EventArgs)

        CargaUnidades(scUnidadMedidaComercial, ControladorUnidadesMedida.TiposUnidad.Comercial, 5)

    End Sub

    Protected Sub scDomiciliosProveedor_SelectedIndexChanged(sender As Object, e As EventArgs)

        Dim tagwatcher_ = GetVars("_proveedor")

        _controladorProveedor = New CtrlProveedoresOperativos()

        _controladorProveedor._tipoOperacion = IIf(swcTipoOperacion.Checked, CtrlProveedoresOperativos.TipoOperacion.Importacion, CtrlProveedoresOperativos.TipoOperacion.Exportacion)

        If fbcCliente.Value IsNot Nothing And fbcCliente.Value <> "" Then

            '++++++++++++++++++++++
            'Aquí buscamos las vinculaciones de este proveedor
            Dim vinculaciones_ = _controladorProveedor.BuscarVinculaciones(tagwatcher_.ObjectReturned.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente, scDomiciliosProveedor.Value, New ObjectId(fbcCliente.Value))

            If vinculaciones_ IsNot Nothing Then

                scVinculacion.DataSource = vinculaciones_

                If scVinculacion.DataSource.Count() > 0 Then

                    scVinculacion.Value = vinculaciones_(0).Value

                End If

            End If

            '++++++++++++++++++++++
            'Aquí buscamos la configuración de este proveedor
            Dim metodoValoración_ = _controladorProveedor.BuscarConfiguraciones(tagwatcher_.ObjectReturned.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente, scDomiciliosProveedor.Value, New ObjectId(fbcCliente.Value))

            If metodoValoración_ IsNot Nothing Then

                scMetodoValoracion.DataSource = metodoValoración_

                If scMetodoValoracion.DataSource.Count() > 0 Then

                    scMetodoValoracion.Value = metodoValoración_(0).Value

                End If

            End If
        End If

    End Sub

#End Region

End Class