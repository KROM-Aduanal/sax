Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports gsol
Imports gsol.Web.Components
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Rec.Globals
Imports Rec.Globals.Controllers
Imports Rec.Globals.Controllers.Empresas
Imports Rec.Globals.Empresas
Imports Rec.Globals.FacturaComercial
Imports Rec.Globals.ProcessingAnalysis
Imports Rec.Globals.Utils
Imports Rec.Globals.Utils.Secuencias
Imports Sax.Web
Imports Syn.CustomBrokers.Controllers
Imports Syn.Documento
Imports Syn.Documento.Componentes
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
        icFechaCOVE.Enabled = False
        icFraccionArancelaria.Enabled = False
        icFraccionNico.Enabled = False
    End Sub

    Public Overrides Sub BotoneraClicNuevo()
        lbModoCapturaManual.Visible = False
        lbModoCapturaManualNuevo.Visible = True
        If OperacionGenerica IsNot Nothing Then
        End If
        PreparaTarjetero(PillboxControl.ToolbarModality.Simple, pbPartidas)
        MonedasUsd()
    End Sub

    Public Overrides Sub BotoneraClicGuardar()
        If Not ProcesarTransaccion(Of ConstructorFacturaComercial)().Status = TypeStatus.Errors Then : End If
    End Sub

    Public Overrides Sub BotoneraClicEditar()
        PreparaTarjetero(PillboxControl.ToolbarModality.Advanced, pbPartidas)
    End Sub

    Public Overrides Sub BotoneraClicBorrar()



    End Sub

    Public Overrides Sub BotoneraClicOtros(ByVal IndexSelected_ As Integer)



    End Sub

    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher
        'Datos Generales
        [Set](dbcNumFacturaCOVE, CA_NUMERO_FACTURA, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](dbcNumFacturaCOVE, CA_NUMERO_ACUSEVALOR, propiedadDelControl_:=PropiedadesControl.ValueDetail)
        [Set](icSerieFolioFactura, CP_SERIE_FOLIO_FACTURA)
        [Set](icFechaFactura, CA_FECHA_FACTURA)
        [Set](icFechaCOVE, CA_FECHA_ACUSEVALOR)
        [Set](icPesoTotal, CP_PESO_TOTAL)
        [Set](icBultos, CP_BULTOS)
        [Set](icOrdenCompra, CP_ORDEN_COMPRA)
        [Set](icReferenciaCliente, CP_REFERENCIA_CLIENTE)
        [Set](swcEnajenacion, CP_APLICA_ENAJENACION, propiedadDelControl_:=PropiedadesControl.Checked)
        [Set](swcSubdivision, CA_APLICA_SUBDIVISION, propiedadDelControl_:=PropiedadesControl.Checked)
        'Cliente
        [Set](fbcCliente, CamposClientes.CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](fbcCliente, CamposClientes.CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Text, asignarA_:=TiposAsignacion.ValorPresentacion)
        'Datos factura
        [Set](fbcPais, CA_CVE_PAIS_FACTURACION, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](fbcPais, CA_PAIS_FACTURACION, propiedadDelControl_:=PropiedadesControl.Text)
        [Set](fbcIncoterm, CA_CVE_INCOTERM, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](fbcIncoterm, CA_CVE_INCOTERM, propiedadDelControl_:=PropiedadesControl.Text, asignarA_:=TiposAsignacion.ValorPresentacion)
        [Set](icValorFactura, CP_VALOR_FACTURA)
        [Set](scMonedaFactura, CA_MONEDA_FACTURACION)
        [Set](icValorMercancia, CP_VALOR_MERCANCIA)
        [Set](scMonedaMercancia, CP_MONEDA_VALOR_MERCANCIA)
        'Datos del proveedor
        [Set](fbcProveedor, CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](fbcProveedor, CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Text, asignarA_:=TiposAsignacion.ValorPresentacion)
        [Set](scDomiciliosProveedor, CamposDomicilio.CP_ID_DOMICILIO, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](scVinculacion, CA_CVE_VINCULACION, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](scMetodoValoracion, CP_CVE_METODO_VALORACION, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](swcFungeCertificado, CA_APLICA_CERTIFICADO, propiedadDelControl_:=PropiedadesControl.Checked)
        [Set](fbcProveedorCertificado, CP_NOMBRE_CERTIFICADOR, propiedadDelControl_:=PropiedadesControl.Text)

        If pbPartidas.PageIndex > 0 Then
            lbNumero.Text = pbPartidas.PageIndex.ToString()
        End If

        [Set](fbcProducto, CA_NUMERO_PARTE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCantidadComercial, CA_CANTIDAD_COMERCIAL_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scUnidadMedidaComercial, CA_UNIDAD_MEDIDA_COMERCIAL_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icDescripcionPartida, CA_DESCRIPCION_PARTE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icValorfacturaPartida, CA_VALOR_FACTURA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scMonedaFacturaPartida, CP_MONEDA_FACTURA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icValorMercanciaPartida, CA_VALOR_MERCANCIA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scMonedaMercanciaPartida, CA_MONEDA_MERCANCIA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPrecioUnitario, CA_PRECIO_UNITARIO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scMonedaPrecioUnitarioPartida, CP_MONEDA_PRECIO_UNITARIO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPesoNeto, CA_PESO_NETO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icDescripcionCOVE, CA_DESCRIPCION_COVE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](fbcPaisPartida, CA_PAIS_ORIGEN_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](swcAplicaCOVE, CP_APLICA_DESCRIPCION_COVE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scMetodoValoracionPartida, CA_CVE_METODO_VALORACION_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](fbcOrdenCompraPartida, CP_ORDEN_COMPRA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        ' Partida - clasificación
        [Set](icFraccionArancelaria, CA_FRACCION_ARANCELARIA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icFraccionNico, CA_FRACCION_NICO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCantidadTarifa, CA_CANTIDAD_TARIFA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scUnidadMedidaTarifa, CA_UNIDAD_MEDIDA_TARIFA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        ' Partida - detalle mercancía
        [Set](icLote, CA_LOTE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icNumeroSerie, CA_NUMERO_SERIE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icMarca, CA_MARCA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icModelo, CA_MODELO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icSubmodelo, CA_SUBMODELO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icKilometraje, CA_KILOMETRAJE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](pbPartidas, Nothing, seccion_:=SeccionesFacturaComercial.SFAC4)
        ' Incrementables
        [Set](icFletes, CA_FLETES)
        [Set](scMonedaFletes, CA_MONEDA_FLETES)
        [Set](icSeguros, CA_SEGURO)
        [Set](scMonedaSeguros, CA_MONEDA_SEGUROS)
        [Set](icEmbalajes, CA_EMBALAJES)
        [Set](scMonedaEmbalajes, CA_MONEDA_EMBALAJES)
        [Set](icOtrosIncrementables, CA_OTROS_INCREMENTABLES)
        [Set](scMonedaOtrosIncrementables, CA_MONEDA_OTROS_INCREMENTABLES)
        [Set](icDescuentos, CA_DESCUENTOS)
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
        Dim controladorSecuencias_ As New ControladorSecuencia
        Dim loginUsuario_ As Dictionary(Of String, String) = Session("DatosUsuario")
        Dim datosCliente_ As New Dictionary(Of String, String)
        If GetVars("_datosCliente") IsNot Nothing Then
            datosCliente_ = DirectCast(GetVars("_datosCliente"), Dictionary(Of String, String))
        End If
        Dim tagwatcher_ As TagWatcher = controladorSecuencias_.Generar(SecuenciasComercioExterior.FacturasComerciales.ToString, 1, 1, 1, 1, Statements.GetOfficeOnline()._id)
        Dim secuencia_ As Rec.Globals.Utils.Secuencias.Secuencia = DirectCast(tagwatcher_.ObjectReturned, Rec.Globals.Utils.Secuencias.Secuencia)
        With documentoElectronico_
            Dim constructorCliente_ As New ConstructorCliente
            If GetVars("_ConstructorCliente") IsNot Nothing Then
                constructorCliente_ = DirectCast(GetVars("_ConstructorCliente"), ConstructorCliente)
            End If
            .Campo(CamposFacturaComercial.CP_TIPO_OPERACION).Valor = 1
            .Campo(CamposFacturaComercial.CP_TIPO_OPERACION).ValorPresentacion = "Importacion"
            If lbModoCapturaIA.Visible = True Then
                .Campo(CP_TIPO_CARGA_DATOS).Valor = 1
                .Campo(CP_TIPO_CARGA_DATOS).ValorPresentacion = "Carga IA"
            Else
                .Campo(CP_TIPO_CARGA_DATOS).Valor = 2
                .Campo(CP_TIPO_CARGA_DATOS).ValorPresentacion = "Carga manual"
            End If
            .UsuarioGenerador = loginUsuario_("Nombre")
            .Id = secuencia_._id.ToString
            .IdDocumento = secuencia_.sec
            .FolioDocumento = dbcNumFacturaCOVE.Value
            .FolioOperacion = secuencia_.sec
            .TipoPropietario = SecuenciasComercioExterior.FacturasComerciales.ToString
            .NombrePropietario = fbcCliente.Text
            .IdPropietario = datosCliente_("cveEmpresaCliente") 'se debe agregar desde el cliente
            .ObjectIdPropietario = New ObjectId(fbcCliente.Value)
        End With
    End Sub

    Public Overrides Function DespuesRealizarInsercion() As TagWatcher
        lbModoCapturaManual.Visible = True
        lbModoCapturaManualNuevo.Visible = False
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
        DatosCliente(fbcCliente.Value)
    End Sub

    Public Overrides Sub DespuesOperadorDatosProcesar(ByRef documentoElectronico_ As DocumentoElectronico)
        With documentoElectronico_
            Dim datosCliente_ As New Dictionary(Of String, String)
            If GetVars("_datosCliente") IsNot Nothing Then
                datosCliente_ = DirectCast(GetVars("_datosCliente"), Dictionary(Of String, String))
                Dim domicilioCliente_ As New Rec.Globals.Empresas.Domicilio

                If GetVars("_domicilioCliente") IsNot Nothing Then
                    domicilioCliente_ = DirectCast(GetVars("_domicilioCliente"), Rec.Globals.Empresas.Domicilio)
                End If

                With .Seccion(SeccionesFacturaComercial.SFAC1)
                    .Campo(CamposClientes.CP_OBJECTID_CLIENTE).Valor = datosCliente_("idCliente")
                    .Campo(CamposClientes.CP_CVE_CLIENTE).Valor = datosCliente_("cveEmpresaCliente")
                    .Campo(CamposClientes.CA_RFC_CLIENTE).Valor = datosCliente_("RfcCliente")
                    .Campo(CamposClientes.CA_CURP_CLIENTE).Valor = datosCliente_("Curp")
                    .Campo(CamposClientes.CA_TAX_ID).Valor = datosCliente_("Taxid")
                    .Campo(CamposDomicilio.CA_PAIS).Valor = datosCliente_("Pais")
                    .Campo(CamposDomicilio.CA_CVE_PAIS).Valor = datosCliente_("CvePais")
                    .Campo(CamposDomicilio.CP_ID_DOMICILIO).Valor = domicilioCliente_._iddomicilio.ToString
                    .Campo(CamposDomicilio.CP_SEC_DOMICILIO).Valor = domicilioCliente_.sec
                    .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor = domicilioCliente_.domicilioPresentacion
                    .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).ValorPresentacion = Nothing
                    .Campo(CamposDomicilio.CA_CALLE).Valor = domicilioCliente_.calle
                    .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor = domicilioCliente_.numeroexterior
                    .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor = domicilioCliente_.numerointerior
                    .Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor = domicilioCliente_.numeroexterior + " - " + domicilioCliente_.numerointerior
                    .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor = domicilioCliente_.codigopostal
                    .Campo(CamposDomicilio.CA_COLONIA).Valor = domicilioCliente_.colonia
                    .Campo(CamposDomicilio.CA_LOCALIDAD).Valor = domicilioCliente_.localidad
                    .Campo(CamposDomicilio.CA_CIUDAD).Valor = domicilioCliente_.ciudad
                    .Campo(CamposDomicilio.CA_MUNICIPIO).Valor = domicilioCliente_.municipio
                    .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor = domicilioCliente_.cveEntidadfederativa
                    .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor = domicilioCliente_.entidadfederativa
                    .Campo(CamposDomicilio.CA_ENTIDAD_MUNICIPIO).Valor = domicilioCliente_.municipio
                End With
            End If

            Dim domicilioProveedor_ As List(Of Rec.Globals.Empresas.Domicilio) = Nothing
            If GetVars("_listaDomiciliosProveedores") IsNot Nothing Then
                domicilioProveedor_ = DirectCast(GetVars("_listaDomiciliosProveedores"),
                                      List(Of Rec.Globals.Empresas.Domicilio))

                Dim datosReceptorProveedor_ As New List(Of Dictionary(Of String, String))
                If GetVars("_datosReceptorProveedor") IsNot Nothing Then
                    datosReceptorProveedor_ = DirectCast(GetVars("_datosReceptorProveedor"), List(Of Dictionary(Of String, String)))
                End If
                domicilioProveedor_.Where(Function(x) x._iddomicilio.
                                Equals(New ObjectId(scDomiciliosProveedor.Value))).
                                AsEnumerable().
                                ToList().
                                ForEach(Sub(item_)
                                            With .Seccion(SeccionesFacturaComercial.SFAC2)
                                                For Each data_ In datosReceptorProveedor_
                                                    If data_("ObjectIdDomicilio_") = scDomiciliosProveedor.Value Then
                                                        '.Campo(CamposProveedorOperativo.CP_ID_PROVEEDOR).Valor = "333LKLDKDLK3333"
                                                        .Campo(CamposProveedorOperativo.CP_CVE_PROVEEDOR).Valor = data_("Cve_")
                                                        .Campo(CamposDomicilio.CA_CVE_PAIS).Valor = data_("CvePais_")
                                                        .Campo(CamposDomicilio.CA_PAIS).Valor = data_("Pais_")
                                                        .Campo(CamposProveedorOperativo.CA_RFC_PROVEEDOR).Valor = data_("RFC_")
                                                        .Campo(CamposProveedorOperativo.CA_CURP_PROVEEDOR).Valor = data_("CURP_")
                                                    End If
                                                Next
                                                .Campo(CamposDomicilio.CP_ID_DOMICILIO).Valor = item_._iddomicilio.ToString
                                                .Campo(CamposDomicilio.CP_SEC_DOMICILIO).Valor = item_.sec
                                                .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor = item_.domicilioPresentacion
                                                .Campo(CamposDomicilio.CA_CALLE).Valor = item_.calle
                                                .Campo(CamposDomicilio.CA_CIUDAD).Valor = item_.ciudad
                                                .Campo(CamposDomicilio.CA_COLONIA).Valor = item_.colonia
                                                .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor = item_.codigopostal
                                                .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor = item_.numeroexterior
                                                .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor = item_.numerointerior
                                                .Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor = item_.numeroexterior & " - " & item_.numerointerior
                                                .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor = item_.codigopostal
                                                .Campo(CamposDomicilio.CA_LOCALIDAD).Valor = item_.localidad
                                                .Campo(CamposDomicilio.CA_MUNICIPIO).Valor = item_.municipio
                                                .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor = item_.cveEntidadfederativa
                                                .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor = item_.entidadfederativa
                                                .Campo(CamposDomicilio.CA_ENTIDAD_MUNICIPIO).Valor = item_.municipio
                                            End With
                                        End Sub)
            End If

        End With
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
        SetVars("isEditing", Nothing)
        SetVars("_datosCliente", Nothing)
        SetVars("_domicilioCliente", Nothing)
        SetVars("_listaDomiciliosProveedores", Nothing)
        SetVars("_datosReceptorProveedor", Nothing)
        SetVars("_listaDomiciliosDestinatario", Nothing)
        SetVars("_datosDestinatario", Nothing)
        SetVars("_listaConstructorProductos", Nothing)
        Statements.ObjectSession = Nothing
    End Sub

    Public Overrides Sub Limpiar()
    End Sub

#End Region

#Region "████████████████  QUINTA CAPA  █████████       Reglas locales         ██████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Protected Sub fbcCliente_TextChanged(sender As Object, e As EventArgs)
        Dim constructorCliente_ As New ControladorBusqueda(Of ConstructorCliente)
        Dim listaClientes_ As List(Of SelectOption) = constructorCliente_.Buscar(fbcCliente.Text,
                                                                              New Filtro With {.IdSeccion = SeccionesClientes.SCS1, .IdCampo = CamposClientes.CA_RAZON_SOCIAL})
        fbcCliente.DataSource = listaClientes_
    End Sub
    Protected Sub fbcCliente_Click(sender As Object, e As EventArgs)
        DatosCliente(fbcCliente.Value)
    End Sub

    Protected Sub fbcProveedor_TextChanged(sender As Object, e As EventArgs)
        Dim constructorProveedorOperativo_ As New ControladorBusqueda(Of ConstructorProveedoresOperativos)
        Dim listaCompradorReceptor_ As List(Of SelectOption) = constructorProveedorOperativo_.Buscar(fbcProveedor.Text,
                                                                              New Filtro With {.IdSeccion = SeccionesProvedorOperativo.SPRO1, .IdCampo = CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR})
        fbcProveedor.DataSource = listaCompradorReceptor_
    End Sub

    Protected Sub fbcProveedor_Click(sender As Object, e As EventArgs)
        Dim buscarProveedor_ As New ControladorBusqueda(Of ConstructorProveedoresOperativos)
        Dim tagwatcher_ As TagWatcher = buscarProveedor_.ObtenerDocumento(fbcProveedor.Value)
        If tagwatcher_.Status = TypeStatus.Ok Then
            Dim constructorProveedor_ As ConstructorProveedoresOperativos = DirectCast(tagwatcher_.ObjectReturned.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente, ConstructorProveedoresOperativos)
            Dim domiciliosProveedores_ = constructorProveedor_.Seccion(SeccionesProvedorOperativo.SPRO2).Nodos
            Dim datosReceptorProveedor_ As New List(Of Dictionary(Of String, String))
            Dim listaDomiciliosProveedores_ As List(Of Rec.Globals.Empresas.Domicilio) = ListarDomicilios(domiciliosProveedores_)
            Dim esDestinatario_ As Boolean = True
            For Each nodo_ In domiciliosProveedores_
                Dim listaProveedores_ As New Dictionary(Of String, String)
                For Each item_ In nodo_.Nodos
                    Dim campo As Campo = DirectCast(item_.Nodos(0), Campo)
                    Select Case campo.IDUnico
                        Case CamposProveedorOperativo.CA_RFC_PROVEEDOR
                            listaProveedores_.Add("RFC_", campo.Valor)
                        Case CamposProveedorOperativo.CA_CURP_PROVEEDOR
                            listaProveedores_.Add("CURP_", campo.Valor)
                        Case CamposDomicilio.CA_CVE_PAIS
                            listaProveedores_.Add("CvePais_", campo.Valor)
                        Case CamposDomicilio.CA_PAIS
                            listaProveedores_.Add("Pais_", campo.Valor)
                        Case CamposDomicilio.CP_ID_DOMICILIO
                            listaProveedores_.Add("ObjectIdDomicilio_", campo.Valor)
                        Case CamposProveedorOperativo.CP_DESTINATARIO_PROVEEDOR
                            esDestinatario_ = IIf(campo.Valor = "1", True, False)
                    End Select
                Next
                With constructorProveedor_.Seccion(SeccionesProvedorOperativo.SPRO1)
                    listaProveedores_.Add("RazonSocial_", .Campo(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR).Valor)
                    listaProveedores_.Add("ObjectId_", .Campo(CamposProveedorOperativo.CP_ID_EMPRESA).Valor.ToString)
                    listaProveedores_.Add("Cve_", .Campo(CamposProveedorOperativo.CP_CVE_PROVEEDOR).Valor)
                End With
                datosReceptorProveedor_.Add(listaProveedores_)
            Next
            SetVars("_listaDomiciliosProveedores", listaDomiciliosProveedores_)
            SetVars("_datosReceptorProveedor", datosReceptorProveedor_)
            Dim dataSource_ As New List(Of SelectOption)
            For index_ As Int32 = 0 To listaDomiciliosProveedores_.Count - 1
                dataSource_.Add(New SelectOption With
                             {.Value = listaDomiciliosProveedores_(index_)._iddomicilio.ToString,
                              .Text = listaDomiciliosProveedores_(index_).domicilioPresentacion})
            Next
            scDomiciliosProveedor.DataSource = dataSource_
        End If
    End Sub

    Protected Sub fbcProveedorCertificado_TextChanged(sender As Object, e As EventArgs)
        ''QUIZA HAGA FALTA EL CONTROLADOR DE PROVEEDORES
        If fbcProveedorCertificado.Text <> "" Then
            Dim constructorProveedorOperativo_ As New ControladorBusqueda(Of ConstructorProveedoresOperativos)
            Dim listaCompradorReceptor_ As List(Of SelectOption) = constructorProveedorOperativo_.Buscar(fbcProveedorCertificado.Text,
                                                                                  New Filtro With {.IdSeccion = SeccionesProvedorOperativo.SPRO1,
                                                                                                   .IdCampo = CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR})
            fbcProveedorCertificado.DataSource = listaCompradorReceptor_
        End If
    End Sub

    Protected Sub scVinculacion_Click(sender As Object, e As EventArgs)
        scVinculacion.DataSource = Vinculacion()
    End Sub
    Protected Sub fbcPaisPartida_TextChanged(sender As Object, e As EventArgs)
        CargaPaises(sender)
    End Sub

    Protected Sub fbcPaisPartida_Click(sender As Object, e As EventArgs)
        MonedasUsd()
    End Sub

    Protected Sub scUnidadMedidaComercial_TextChanged(sender As Object, e As EventArgs)
        CargaUnidades(scUnidadMedidaComercial, ControladorUnidadesMedida.TiposUnidad.Comercial, 10)
    End Sub

    Protected Sub scUnidadMedidaComercial_Click(sender As Object, e As EventArgs)
        CargaUnidades(scUnidadMedidaComercial, ControladorUnidadesMedida.TiposUnidad.Comercial, 10)
    End Sub

    Protected Sub scUnidadMedidaTarifa_TextChanged(sender As Object, e As EventArgs)
        CargaUnidades(scUnidadMedidaTarifa, ControladorUnidadesMedida.TiposUnidad.Comercial, 10)
    End Sub

    Protected Sub scUnidadMedidaTarifa_Click(sender As Object, e As EventArgs)
        CargaUnidades(scUnidadMedidaTarifa, ControladorUnidadesMedida.TiposUnidad.Comercial, 10)
    End Sub

    Protected Sub pbPartidas_Click(sender As Object, e As EventArgs)
        Select Case pbPartidas.ToolbarAction
            Case PillboxControl.ToolbarActions.Nuevo
                lbNumero.Text = pbPartidas.PageIndex.ToString()
                MonedasUsd()
        End Select
    End Sub

    Protected Sub scMonedaFactura_Click(sender As Object, e As EventArgs)
        BusquedaMonedas(sender)
    End Sub

    Protected Sub scMonedaMercancia_Click(sender As Object, e As EventArgs)
        BusquedaMonedas(sender)
    End Sub

    Protected Sub scMonedaFacturaPartida_Click(sender As Object, e As EventArgs)
        BusquedaMonedas(sender)
    End Sub

    Protected Sub scMonedaMercanciaPartida_Click(sender As Object, e As EventArgs)
        BusquedaMonedas(sender)
    End Sub

    Protected Sub scMonedaPrecioUnitarioPartida_Click(sender As Object, e As EventArgs)
        BusquedaMonedas(sender)
    End Sub

    Protected Sub scMonedaSeguros_Click(sender As Object, e As EventArgs)
        BusquedaMonedas(sender)
    End Sub

    Protected Sub scMonedaEmbalajes_Click(sender As Object, e As EventArgs)
        BusquedaMonedas(sender)
    End Sub

    Protected Sub scMonedaOtrosIncrementables_Click(sender As Object, e As EventArgs)
        BusquedaMonedas(sender)
    End Sub

    Protected Sub scMonedaDescuentos_Click(sender As Object, e As EventArgs)
        BusquedaMonedas(sender)
    End Sub

    Protected Sub scMonedaFletes_Click(sender As Object, e As EventArgs)
        BusquedaMonedas(sender)
    End Sub

    Protected Sub swcAplicaCOVE_CheckedChanged(sender As Object, e As EventArgs)
        If swcAplicaCOVE.Checked Then
            icDescripcionCOVE.Value = icDescripcionPartida.Value
        Else
            icDescripcionCOVE.Value = Nothing
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

    Protected Sub fbcProducto_TextChanged(sender As Object, e As EventArgs)
        Dim controladorProductos_ As IControladorProductos =
            New ControladorProductos(IControladorProductos.ListaBusquedas.Cliente)
        Dim consulta_ = fbcProducto.Text
        Dim filtroCliente_ = fbcCliente.Value
        Dim dataSource_ As New List(Of SelectOption)
        Dim tagWatcher_ As TagWatcher = controladorProductos_.Consultar(consulta_, filtroCliente_)
        If tagWatcher_.Status = TypeStatus.Ok Then
            Dim listaProductos_ = tagWatcher_.ObjectReturned
            If listaProductos_.Count > 0 Then
                For index_ As Int32 = 0 To listaProductos_.Count - 1
                    dataSource_.Add(New SelectOption With
                                 {.Value = listaProductos_(index_).id.ToString,
                                  .Text = listaProductos_(index_)._idKrom & " | " & listaProductos_(index_)._numeroParte & " - " & listaProductos_(index_)._alias})
                Next
            End If
        End If
        fbcProducto.DataSource = dataSource_
    End Sub

    Protected Sub fbcProducto_Click(sender As Object, e As EventArgs)
        Dim controladorProductos_ As IControladorProductos = New ControladorProductos()
        Dim idProducto_ = fbcProducto.Value
        If idProducto_ <> "" Then
            Dim productoText_ = fbcProducto.Text.Split("|")
            Dim idKrom_ = Integer.Parse(productoText_(0))
            Dim tagWatcher_ As TagWatcher = controladorProductos_.ConsultarOne(New ObjectId(idProducto_))
            If tagWatcher_.Status = TypeStatus.Ok Then
                Dim producto_ = DirectCast(tagWatcher_.ObjectReturned, AuxiliarProducto)
                For Each item_ In producto_._historicoDescripciones
                    With item_
                        If ._idKrom = idKrom_ Then
                            icDescripcionPartida.Value = ._descripcion
                        End If
                    End With
                Next
                With producto_
                    icFraccionArancelaria.Value = ._fraccionArancelaria
                    '    'MostrarDescripciones(.Campo(CamposProducto.CP_DESCRIPCION_FRACCION_ARANCELARIA).Valor)
                    icFraccionNico.Value = ._nico
                    '    'MostrarDescripciones(.Campo(CamposProducto.CP_DESCRIPCION_NICO).Valor)
                    AvisoFraccion(._status)
                End With
            End If
        Else
            icDescripcionPartida.Value = Nothing
            icFraccionArancelaria.Value = Nothing
            icFraccionNico.Value = Nothing
            controladorProductos_.ReiniciarControlador()
        End If
    End Sub

    Private Function Vinculacion() As List(Of SelectOption)
        Dim recursos_ As ControladorRecursosAduanalesGral =
            ControladorRecursosAduanalesGral.Buscar(ControladorRecursosAduanalesGral.TiposRecurso.Anexo22)
        Dim vinculaciones_ = From data In recursos_.tiposvinculacion
                             Where data.archivado = False And data.estado = 1
                             Select data._idvinculacion, data.descripcion, data.descripcioncorta
        Dim dataSource_ As New List(Of SelectOption)
        If vinculaciones_.Count > 0 Then
            For index_ As Int32 = 0 To vinculaciones_.Count - 1
                dataSource_.Add(New SelectOption With
                             {.Value = vinculaciones_(index_)._idvinculacion,
                              .Text = vinculaciones_(index_)._idvinculacion.ToString & " - " & vinculaciones_(index_).descripcioncorta})
            Next
            Return dataSource_
        End If
        Return Nothing
    End Function

    Function CargaPaises(ByRef control_ As FindboxControl) As List(Of SelectOption)
        Dim paisesTemporales_ As New List(Of Pais)
        Dim lista_ As List(Of SelectOption) = ControladorPaises.BuscarPaises(paisesTemporales_, control_.Text)
        control_.DataSource = lista_
        Return lista_
    End Function

    Protected Sub dbcNumFacturaCOVE_Click(sender As Object, e As EventArgs)
    End Sub

    Function BusquedaMonedas(ByRef control_ As SelectControl) As List(Of SelectOption)
        Dim listaMonedas_ = ControladorPaises.BuscarTodasMonedas(control_.SuggestedText)
        If listaMonedas_.Count > 0 Then
            control_.DataSource = listaMonedas_
        End If
        Return listaMonedas_
    End Function

    Protected Sub swcFungeCertificado_CheckedChanged(sender As Object, e As EventArgs)
        If swcFungeCertificado.Checked Then
            fbcProveedorCertificado.Enabled = False
            fbcProveedorCertificado.Text = Nothing
            fbcProveedorCertificado.Value = Nothing
        Else
            fbcProveedorCertificado.Enabled = True
        End If
    End Sub

    Protected Sub CargaMoneda_Click(sender As Object, e As EventArgs)
        BusquedaMonedas(sender)
    End Sub

    Sub CargaUnidades(ByRef control_ As SelectControl,
                                   ByVal tipoUnidad_ As ControladorUnidadesMedida.TiposUnidad,
                                    Optional ByVal top_ As Int32 = 0)
        Dim lista_ As List(Of UnidadMedida) = ControladorUnidadesMedida.BuscarUnidades(tipoUnidad_,
                                                                                       control_.SuggestedText, top_)
        If lista_.Count > 0 Then
            control_.DataSource = ControladorUnidadesMedida.ToSelectOption(lista_,
                                                                           ControladorUnidadesMedida.TipoSelectOption.CveMXnombreoficiales)
        End If
    End Sub

    Protected Sub scDomiciliosProveedor_Click(sender As Object, e As EventArgs)
        If fbcProveedor.Text = "" Then
            MsgValidacionRazonsocial()
        Else
            BuscarDomiciliosProveedor()
        End If
    End Sub

    Protected Sub scDomiciliosProveedor_SelectedIndexChanged(sender As Object, e As EventArgs)
        If fbcProveedor.Text = "" Then
            MsgValidacionRazonsocial()
        Else
            BuscarDomiciliosProveedor()
        End If
    End Sub

    Protected Sub scDomiciliosProveedor_OnTextChanged(sender As Object, e As EventArgs)
        If fbcProveedor.Text = "" Then
            MsgValidacionRazonsocial()
        Else
            BuscarDomiciliosProveedor()
        End If
    End Sub

    Protected Sub BuscarDomiciliosProveedor()
        Dim buscarProveedor_ As New ControladorBusqueda(Of ConstructorProveedoresOperativos)
        Dim tagwatcher_ As TagWatcher = buscarProveedor_.ObtenerDocumento(fbcProveedor.Value)
        If tagwatcher_.Status = TypeStatus.Ok Then
            Dim constructorProveedor_ As ConstructorProveedoresOperativos = DirectCast(tagwatcher_.ObjectReturned.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente, ConstructorProveedoresOperativos)
            Dim domiciliosProveedores_ = constructorProveedor_.Seccion(SeccionesProvedorOperativo.SPRO2).Nodos
            Dim datosReceptorProveedor_ As New List(Of Dictionary(Of String, String))
            Dim listaDomiciliosProveedores_ As List(Of Rec.Globals.Empresas.Domicilio) = ListarDomicilios(domiciliosProveedores_)

            For Each nodo_ In domiciliosProveedores_
                Dim listaProveedores_ As New Dictionary(Of String, String)
                datosReceptorProveedor_.Add(listaProveedores_)
            Next

            SetVars("_listaDomiciliosProveedores", listaDomiciliosProveedores_)

            Dim dataSource_ As New List(Of SelectOption)
            For index_ As Int32 = 0 To listaDomiciliosProveedores_.Count - 1
                dataSource_.Add(New SelectOption With
                             {.Value = listaDomiciliosProveedores_(index_)._iddomicilio.ToString,
                              .Text = listaDomiciliosProveedores_(index_).domicilioPresentacion})
            Next
            scDomiciliosProveedor.DataSource = dataSource_
            scDomiciliosProveedor.Value = dataSource_.Last.Value
        End If
    End Sub

    Protected Sub MsgValidacionRazonsocial()
        fbcProveedor.ToolTip = "Debes indicar una razón social. "
        fbcProveedor.ToolTipExpireTime = 4
        fbcProveedor.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
        fbcProveedor.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        fbcProveedor.ShowToolTip()
    End Sub

    Protected Sub DatosCliente(ByVal razonsocialCliente As String)
        Dim buscarCliente_ As New ControladorBusqueda(Of ConstructorCliente)
        Dim tagwatcher_ As TagWatcher = buscarCliente_.ObtenerDocumento(razonsocialCliente)
        If tagwatcher_.Status = TypeStatus.Ok Then
            Dim constructorCliente_ As ConstructorCliente = DirectCast(tagwatcher_.ObjectReturned.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente, ConstructorCliente)
            Dim domicilioCliente_ As New Rec.Globals.Empresas.Domicilio
            Dim datosCliente_ As New Dictionary(Of String, String)()
            With constructorCliente_
                datosCliente_.Add("cveEmpresaCliente", .Campo(CamposClientes.CP_CVE_EMPRESA).Valor)
                datosCliente_.Add("RfcCliente", .Campo(CamposClientes.CA_RFC_CLIENTE).Valor)
                datosCliente_.Add("Taxid", .Campo(CamposClientes.CA_TAX_ID).Valor)
                datosCliente_.Add("Curp", .Campo(CamposClientes.CA_CURP_CLIENTE).Valor)
                datosCliente_.Add("idCliente", .Campo(CamposClientes.CP_ID_EMPRESA).Valor.ToString)
                datosCliente_.Add("CvePais", .Campo(CamposDomicilio.CA_CVE_PAIS).Valor)
                datosCliente_.Add("Pais", .Campo(CamposDomicilio.CA_PAIS).Valor)
                domicilioCliente_.domicilioPresentacion = .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor
                domicilioCliente_.calle = .Campo(CamposDomicilio.CA_CALLE).Valor
                domicilioCliente_.numeroexterior = .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor
                domicilioCliente_.numerointerior = .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor
                domicilioCliente_.ciudad = .Campo(CamposDomicilio.CA_CIUDAD).Valor
                domicilioCliente_.localidad = .Campo(CamposDomicilio.CA_LOCALIDAD).Valor
                domicilioCliente_.codigopostal = .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor
                domicilioCliente_.localidad = .Campo(CamposDomicilio.CA_LOCALIDAD).Valor
                domicilioCliente_.colonia = .Campo(CamposDomicilio.CA_COLONIA).Valor
                domicilioCliente_.entidadfederativa = .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor
                domicilioCliente_.cveEntidadfederativa = .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor
                domicilioCliente_.cveMunicipio = .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor
                domicilioCliente_.municipio = .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor
                domicilioCliente_._iddomicilio = .Campo(CamposDomicilio.CP_ID_DOMICILIO).Valor
                domicilioCliente_.sec = .Campo(CamposDomicilio.CP_SEC_DOMICILIO).Valor
            End With
            SetVars("_datosCliente", datosCliente_)
            SetVars("_domicilioCliente", domicilioCliente_)
        End If
    End Sub

    Protected Sub MonedasUsd()
        Dim cveUSD_ = "635acf25a8210bfa0d58434e"
        Dim nombre_ = "USD"
        Dim dataSourceUSD_ = New List(Of SelectOption) From {New SelectOption With {.Value = cveUSD_, .Text = nombre_}}
        scMonedaFactura.DataSource = dataSourceUSD_
        scMonedaFactura.Value = cveUSD_
        scMonedaMercancia.DataSource = dataSourceUSD_
        scMonedaMercancia.Value = cveUSD_
        scMonedaFacturaPartida.DataSource = dataSourceUSD_
        scMonedaFacturaPartida.Value = cveUSD_
        scMonedaMercanciaPartida.DataSource = dataSourceUSD_
        scMonedaMercanciaPartida.Value = cveUSD_
        scMonedaPrecioUnitarioPartida.DataSource = dataSourceUSD_
        scMonedaPrecioUnitarioPartida.Value = cveUSD_
        scMonedaFletes.DataSource = dataSourceUSD_
        scMonedaFletes.Value = cveUSD_
        scMonedaSeguros.DataSource = dataSourceUSD_
        scMonedaSeguros.Value = cveUSD_
        scMonedaEmbalajes.DataSource = dataSourceUSD_
        scMonedaEmbalajes.Value = cveUSD_
        scMonedaOtrosIncrementables.DataSource = dataSourceUSD_
        scMonedaOtrosIncrementables.Value = cveUSD_
        scMonedaDescuentos.DataSource = dataSourceUSD_
        scMonedaDescuentos.Value = cveUSD_

    End Sub

    Protected Function ListarDomicilios(domiciliosSeccion_ As List(Of Nodo)) _
        As List(Of Rec.Globals.Empresas.Domicilio)
        Dim listaDomicilios_ As New List(Of Rec.Globals.Empresas.Domicilio)
        For Each nodo_ In domiciliosSeccion_
            Dim domicilioAux_ As New Rec.Globals.Empresas.Domicilio
            For Each item_ In nodo_.Nodos
                Dim campo As Campo = DirectCast(item_.Nodos(0), Campo)
                With domicilioAux_
                    Select Case campo.IDUnico
                        Case CamposDomicilio.CA_CALLE
                            .calle = campo.Valor
                        Case CamposDomicilio.CA_DOMICILIO_FISCAL
                            .domicilioPresentacion = campo.Valor
                        Case CamposDomicilio.CA_NUMERO_EXTERIOR
                            .numeroexterior = campo.Valor
                        Case CamposDomicilio.CA_NUMERO_INTERIOR
                            .numerointerior = campo.Valor
                        Case CamposDomicilio.CA_CIUDAD
                            .ciudad = campo.Valor
                        Case CamposDomicilio.CA_LOCALIDAD
                            .localidad = campo.Valor
                        Case CamposDomicilio.CA_COLONIA
                            .colonia = campo.Valor
                        Case CamposDomicilio.CA_CODIGO_POSTAL
                            .codigopostal = campo.Valor
                        Case CamposDomicilio.CA_ENTIDAD_FEDERATIVA
                            .entidadfederativa = campo.Valor
                        Case CamposDomicilio.CA_ENTIDAD_MUNICIPIO
                            .municipio = campo.Valor
                        Case CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA
                            .cveEntidadfederativa = campo.Valor
                        Case CamposDomicilio.CA_ENTIDAD_MUNICIPIO
                            .cveMunicipio = campo.Valor
                        Case CamposDomicilio.CP_ID_DOMICILIO
                            ._iddomicilio = New ObjectId(campo.Valor.ToString)
                        Case CamposDomicilio.CP_SEC_DOMICILIO
                            .sec = campo.Valor
                    End Select
                End With
            Next
            listaDomicilios_.Add(domicilioAux_)
        Next
        Return listaDomicilios_
    End Function

    Protected Sub LimpiarMonedas()
        scMonedaFactura.DataSource = Nothing
        scMonedaMercancia.DataSource = Nothing
        scMonedaFacturaPartida.DataSource = Nothing
        scMonedaMercanciaPartida.DataSource = Nothing
        scMonedaPrecioUnitarioPartida.DataSource = Nothing
        scMonedaFletes.DataSource = Nothing
        scMonedaSeguros.DataSource = Nothing
        scMonedaEmbalajes.DataSource = Nothing
        scMonedaOtrosIncrementables.DataSource = Nothing
        scMonedaDescuentos.DataSource = Nothing
    End Sub

    Protected Sub ListarMonedas(ByVal pais_ As Pais)
        Dim lista_ As List(Of SelectOption) = ControladorPaises.BuscarMonedasOficiales(pais_)
        Dim monedaActiva_ = lista_(0).Value
        scMonedaFactura.DataSource = lista_
        scMonedaFactura.Value = lista_(0).Value
        scMonedaMercancia.DataSource = lista_
        scMonedaMercancia.Value = lista_(0).Value
        scMonedaFacturaPartida.DataSource = lista_
        scMonedaFacturaPartida.Value = lista_(0).Value
        scMonedaMercanciaPartida.DataSource = lista_
        scMonedaMercanciaPartida.Value = lista_(0).Value
        scMonedaPrecioUnitarioPartida.DataSource = lista_
        scMonedaPrecioUnitarioPartida.Value = lista_(0).Value
        scMonedaFletes.DataSource = lista_
        scMonedaFletes.Value = lista_(0).Value
        scMonedaSeguros.DataSource = lista_
        scMonedaSeguros.Value = lista_(0).Value
        scMonedaEmbalajes.DataSource = lista_
        scMonedaEmbalajes.Value = lista_(0).Value
        scMonedaOtrosIncrementables.DataSource = lista_
        scMonedaOtrosIncrementables.Value = lista_(0).Value
        scMonedaDescuentos.DataSource = lista_
        scMonedaDescuentos.Value = lista_(0).Value
    End Sub

    Protected Sub MostrarDescripciones(ByVal texto_ As String)
        icFraccionNico.ToolTip = texto_
        icFraccionNico.ToolTipStatus = IUIControl.ToolTipTypeStatus.Ok
        icFraccionArancelaria.ToolTipExpireTime = 6
        icFraccionNico.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        icFraccionNico.ShowToolTip()
    End Sub

    Protected Sub AvisoFraccion(ByVal aviso_ As String)
        icFraccionArancelaria.ToolTip = "Estatus clasificación: " & aviso_
        icFraccionArancelaria.ToolTipExpireTime = 6
        icFraccionArancelaria.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        icFraccionArancelaria.ShowToolTip()
    End Sub




#End Region

End Class