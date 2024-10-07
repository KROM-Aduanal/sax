#Region "├┴┘├┴┘├┴┘├┴┘├┴┘|├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘   DEPENDENCIAS   ├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘"

'RECURSOS DEL CMF
Imports System.Globalization
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip
Imports gsol.krom
'OBJETOS BIDIMENSIONALES (ODF's.  Dependencias Krombase/SQL Server)
Imports gsol.krom.Anexo22.Vt022AduanaSeccionA01
Imports gsol.Web.Components
Imports gsol.Web.Components.FormControl.ButtonbarModality
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Rec.Globals
'OBJETOS DIMENSIONALES (ODS's) Dependencias en MongoDB
Imports Rec.Globals.Controllers
'Imports Rec.Globals.Empresa
Imports Rec.Globals.Utils
Imports Sax.Web
'UTILERIAS/RECURSOS ADICIONALES
Imports Sax.Web.ControladorBackend.Cookies
Imports Sax.Web.ControladorBackend.Datos
Imports Syn.CustomBrokers.Controllers
Imports Syn.Documento
Imports Syn.Documento.Componentes.Campo
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Nucleo.RecursosComercioExterior.CamposFacturaComercial
Imports Syn.Nucleo.Recursos.CamposClientes
Imports Syn.Nucleo.RecursosComercioExterior.CamposAcuseValor
Imports Syn.Operaciones
Imports Syn.Utils.Organismo
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher
Imports Wma.Exceptions.TagWatcher.TypeStatus
Imports SharpCompress.Common
Imports MongoDB.Driver.Linq
Imports System.Runtime.ExceptionServices
Imports System.Security.Permissions
Imports Rec.Globals.Utils.Secuencias
Imports Syn.Nucleo.RecursosComercioExterior.SecuenciasComercioExterior
Imports Rec.Globals.Empresas
Imports Syn.Documento.Componentes
Imports Rec.Globals.FacturaComercial


#End Region

Public Class Ges022_FacturaComercialExportacion
    Inherits ControladorBackend

#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

#End Region

#Region "██████ Vinculación c/capas inf █████████       SAX      ████████████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████
    Public Overrides Sub Inicializa()
        With Buscador
            .DataObject = New ConstructorFacturaComercial(True)
            .addFilter(SeccionesFacturaComercial.SFAC1, CA_NUMERO_FACTURA, "Número factura")
            .addFilter(SeccionesFacturaComercial.SFAC1, CA_NUMERO_ACUSEVALOR, "COVE")
            .addFilter(SeccionesFacturaComercial.SFAC2, CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, "Emisor")
        End With
        If Not Page.IsPostBack Then
            Session("_pbPartidasItems") = PillboxControl.ToolbarModality.Default
        End If
        pbPartidasItems.Modality = Session("_pbPartidasItems")
        '  Generales
        fbcIncoterm.DataEntity = New Anexo22()
        ' Datos del proveedor
        scMetodoValoracion.DataEntity = New Anexo22()
        ' Partidas
        scMetodoValoracionPartida.DataEntity = New Anexo22()
        'Unidades de medida
        icFechaAcuseValor.Enabled = False
        icFraccionArancelaria.Enabled = False
        icFraccionNico.Enabled = False
    End Sub
    Public Overrides Sub BotoneraClicGuardar()
        If Not ProcesarTransaccion(Of ConstructorFacturaComercial)().Status = TypeStatus.Errors Then : End If
    End Sub
    Public Overrides Sub BotoneraClicNuevo()
        lbModoCapturaManual.Visible = False
        lbModoCapturaManualNuevo.Visible = True
        scVinculacion.DataSource = Vinculacion()
        scVinculacion.Value = 0
        If OperacionGenerica IsNot Nothing Then
        End If
        PreparaTarjetero(PillboxControl.ToolbarModality.Simple, pbPartidasItems)
        MonedasUsd()
        MetodoValoracionInicial()
        icFraccionArancelaria.Enabled = False
        icFraccionNico.Enabled = False
    End Sub

    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher
        'Encabezado
        [Set](dbcNumFacturaAcuseValor, CA_NUMERO_FACTURA, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](dbcNumFacturaAcuseValor, CA_NUMERO_ACUSEVALOR, propiedadDelControl_:=PropiedadesControl.ValueDetail)
        [Set](icNumeroFacturaUUID, CP_SERIE_FOLIO_FACTURA)
        [Set](icFechaFactura, CA_FECHA_FACTURA)
        [Set](icFechaAcuseValor, CA_FECHA_ACUSEVALOR)
        [Set](icPesoTotal, CP_PESO_TOTAL)
        [Set](icBultos, CP_BULTOS)
        [Set](icOrdenCompra, CP_ORDEN_COMPRA)
        [Set](icReferenciaCliente, CP_REFERENCIA_CLIENTE)
        [Set](swcEnajenacion, CP_APLICA_ENAJENACION, propiedadDelControl_:=PropiedadesControl.Checked)
        'Cliente
        [Set](fbcCliente, CamposClientes.CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](fbcCliente, CamposClientes.CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Text, asignarA_:=TiposAsignacion.ValorPresentacion)

        'Datos de factura
        [Set](fbcPais, CA_CVE_PAIS_FACTURACION, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](fbcPais, CA_PAIS_FACTURACION, propiedadDelControl_:=PropiedadesControl.Text)
        [Set](fbcIncoterm, CA_CVE_INCOTERM, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](fbcIncoterm, CA_CVE_INCOTERM, propiedadDelControl_:=PropiedadesControl.Text, asignarA_:=TiposAsignacion.ValorPresentacion)
        [Set](icValorFactura, CP_VALOR_FACTURA)
        [Set](scMonedaFactura, CA_MONEDA_FACTURACION)
        [Set](icValorMercancia, CP_VALOR_MERCANCIA)
        [Set](scMonedaMercancia, CP_MONEDA_VALOR_MERCANCIA)
        'Comprador - receptor
        [Set](fbcCompradorReceptor, CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](fbcCompradorReceptor, CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Text, asignarA_:=TiposAsignacion.ValorPresentacion)
        [Set](scDomicilioCompradorReceptor, CamposDomicilio.CP_ID_DOMICILIO, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](scVinculacion, CA_CVE_VINCULACION, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](scMetodoValoracion, CP_CVE_METODO_VALORACION, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](swcEsDestinatario, CamposProveedorOperativo.CP_DESTINATARIO_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Checked)
        'Comprador - destinatario
        [Set](fbcCompradorDestinatario, CamposDestinatario.CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](fbcCompradorDestinatario, CamposDestinatario.CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Text, asignarA_:=TiposAsignacion.ValorPresentacion)
        [Set](scDomicilioCompradorDestinatario, CamposDomicilio.CP_ID_DOMICILIO, propiedadDelControl_:=PropiedadesControl.Valor)

        If pbPartidasItems.PageIndex > 0 Then
            lbNumero.Text = pbPartidasItems.PageIndex.ToString()
        End If
        [Set](fbcProducto, CA_NUMERO_PARTE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCantidadComercial, CA_CANTIDAD_COMERCIAL_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scUnidadMedidaComercial, CA_UNIDAD_MEDIDA_COMERCIAL_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icDescripcionPartida, CA_DESCRIPCION_PARTE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icValorDolaresPartida, CA_VALOR_DOLARES_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scMonedaValorDolaresPartida, CP_MONEDA_VALOR_DOLARES_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icValorUnitarioPartida, CA_VALOR_UNITARIO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scMonedaValorUnitarioPartida, CA_MONEDA_VALOR_UNITARIO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](icValorfacturaPartida, CA_VALOR_FACTURA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](scMonedaFacturaPartida, CP_MONEDA_FACTURA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](icValorMercanciaPartida, CA_VALOR_MERCANCIA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](scMonedaMercanciaPartida, CA_MONEDA_MERCANCIA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPrecioUnitario, CA_PRECIO_UNITARIO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scMonedaPrecioUnitarioPartida, CP_MONEDA_PRECIO_UNITARIO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPesoNeto, CA_PESO_NETO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icDescripcionCOVE, CA_DESCRIPCION_COVE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](swcAplicaCOVE, CP_APLICA_DESCRIPCION_COVE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](fbcPaisPartida, CA_PAIS_DESTINO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scMetodoValoracionPartida, CA_CVE_METODO_VALORACION_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icOrdenCompraPartida, CP_ORDEN_COMPRA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        ' Partida - clasificación
        [Set](icFraccionArancelaria, CA_FRACCION_ARANCELARIA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icFraccionNico, CA_FRACCION_NICO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCantidadTarifa, CA_CANTIDAD_TARIFA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scUnidadMedidaTarifa, CA_UNIDAD_MEDIDA_TARIFA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        'Partida -detalle mercancía
        [Set](icLote, CA_LOTE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icNumeroSerie, CA_NUMERO_SERIE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icMarca, CA_MARCA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icModelo, CA_MODELO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icSubmodelo, CA_SUBMODELO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icKilometraje, CA_KILOMETRAJE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](pbPartidasItems, Nothing, seccion_:=SeccionesFacturaComercial.SFAC4)
        Return New TagWatcher(1)
    End Function

    'EVENTOS PARA LA INSERCIÓN DE DATOS
    Public Overrides Function AntesRealizarInsercion(ByVal session_ As IClientSessionHandle) As TagWatcher
        Dim tagwatcher_ As TagWatcher
        '      ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒
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
            .Campo(CamposFacturaComercial.CP_TIPO_OPERACION).Valor = 2
            .Campo(CamposFacturaComercial.CP_TIPO_OPERACION).ValorPresentacion = "Exportacion"
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
            .FolioDocumento = dbcNumFacturaAcuseValor.Value
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

    'EVENTOS PARA MODIFICACIÓN DE DATOS
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
                                Equals(New ObjectId(scDomicilioCompradorReceptor.Value))).
                                AsEnumerable().
                                ToList().
                                ForEach(Sub(item_)
                                            With .Seccion(SeccionesFacturaComercial.SFAC2)
                                                For Each data_ In datosReceptorProveedor_
                                                    If data_("ObjectIdDomicilio_") = scDomicilioCompradorReceptor.Value Then
                                                        '.Campo(CamposProveedorOperativo.CP_ID_PROVEEDOR).Valor = data_("ObjectId_")
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

            Dim domicilioDestinatario_ As List(Of Rec.Globals.Empresas.Domicilio) = Nothing
            If GetVars("_listaDomiciliosDestinatario") IsNot Nothing Then
                domicilioDestinatario_ = DirectCast(GetVars("_listaDomiciliosDestinatario"),
                                    List(Of Rec.Globals.Empresas.Domicilio))

                Dim datosDestinatario_ As New List(Of Dictionary(Of String, String))
                If GetVars("_datosDestinatario") IsNot Nothing Then
                    datosDestinatario_ = DirectCast(GetVars("_datosDestinatario"), List(Of Dictionary(Of String, String)))
                End If

                domicilioDestinatario_.Where(Function(x) x._iddomicilio.
                               Equals(New ObjectId(scDomicilioCompradorDestinatario.Value))).
                               AsEnumerable().
                               ToList().
                               ForEach(Sub(item_)
                                           With .Seccion(SeccionesFacturaComercial.SFAC3)
                                               For Each data_ In datosDestinatario_
                                                   If data_("ObjectIdDomicilio_") = scDomicilioCompradorReceptor.Value Then
                                                       .Campo(CamposDestinatario.CP_ID_DESTINATARIO).Valor = data_("ObjectId_")
                                                       .Campo(CamposDestinatario.CP_CVE_DESTINATARIO).Valor = data_("Cve_")
                                                       .Campo(CamposDomicilio.CA_CVE_PAIS).Valor = data_("CvePais_")
                                                       .Campo(CamposDomicilio.CA_PAIS).Valor = data_("Pais_")
                                                       .Campo(CamposDestinatario.CA_RFC_DESTINATARIO).Valor = data_("RFC_")
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

    Public Overrides Sub DespuesBuquedaGeneralConDatos()
        'PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbPartidas)
    End Sub

    Public Overrides Sub DespuesBuquedaGeneralSinDatos()
        'PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbPartidas)
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
        Statements.ObjectSession = Nothing
    End Sub

    Public Overrides Sub Limpiar()
    End Sub

#End Region

#Region "████████████████  QUINTA CAPA  █████████       Reglas locales         ██████████████████████████████"
    '       * Aquí se pueden colocar los eventos de los componentes, funciones o metodos exclusios del modulo
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

    Protected Sub fbcCompradorReceptor_TextChanged(sender As Object, e As EventArgs)
        Dim constructorProveedorOperativo_ As New ControladorBusqueda(Of ConstructorProveedoresOperativos)
        Dim listaCompradorReceptor_ As List(Of SelectOption) = constructorProveedorOperativo_.Buscar(fbcCompradorReceptor.Text,
                                                                              New Filtro With {.IdSeccion = SeccionesProvedorOperativo.SPRO1, .IdCampo = CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR})
        fbcCompradorReceptor.DataSource = listaCompradorReceptor_
    End Sub

    Protected Sub fbcCompradorReceptor_Click(sender As Object, e As EventArgs)
        Dim buscarProveedor_ As New ControladorBusqueda(Of ConstructorProveedoresOperativos)
        Dim tagwatcher_ As TagWatcher = buscarProveedor_.ObtenerDocumento(fbcCompradorReceptor.Value)
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
            scDomicilioCompradorReceptor.DataSource = dataSource_
            If esDestinatario_ Then
                swcEsDestinatario.Checked = True
                fbcCompradorDestinatario.Text = fbcCompradorReceptor.Text
                scDomicilioCompradorDestinatario.DataSource = dataSource_
                SetVars("_domiciliosCompradorDestinatario", dataSource_)
                SetVars("_listaDomiciliosDestinatario", listaDomiciliosProveedores_)
                SetVars("_datosDestinatario", datosReceptorProveedor_)
            End If
        End If
    End Sub

    Protected Function fbcCompradorReceptor_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Function

    Protected Sub scDomicilioCompradorReceptor_SelectedIndexChanged(sender As Object, e As EventArgs)
        If scDomicilioCompradorReceptor.Value <> "" Then
            If swcEsDestinatario.Checked Then
                scDomicilioCompradorDestinatario.Value = scDomicilioCompradorReceptor.Value
            End If
        End If
    End Sub

    Protected Sub scDomicilioCompradorReceptor_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub scDomicilioCompradorReceptor_Click(sender As Object, e As EventArgs)
        If fbcCompradorReceptor.Text = "" Then
            MsgValidacionRazonsocial()
        End If
    End Sub

    Protected Sub MsgValidacionRazonsocial()
        fbcCompradorReceptor.ToolTip = "Debes indicar una razón social. "
        fbcCompradorReceptor.ToolTipExpireTime = 4
        fbcCompradorReceptor.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
        fbcCompradorReceptor.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        fbcCompradorReceptor.ShowToolTip()
    End Sub

    Protected Sub fbcCompradorDestinatario_TextChanged(sender As Object, e As EventArgs)
        ''QUIZA HAGA FALTA EL CONTROLADOR DE PROVEEDORES
        If fbcCompradorDestinatario.Text <> "" Then
            Dim constructorProveedorOperativo_ As New ControladorBusqueda(Of ConstructorProveedoresOperativos)
            Dim listaCompradorReceptor_ As List(Of SelectOption) = constructorProveedorOperativo_.Buscar(fbcCompradorDestinatario.Text,
                                                                                  New Filtro With {.IdSeccion = SeccionesProvedorOperativo.SPRO1, .IdCampo = CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR})
            fbcCompradorDestinatario.DataSource = listaCompradorReceptor_
        End If
    End Sub

    Protected Sub fbcCompradorDestinatario_Click(sender As Object, e As EventArgs)
        If fbcCompradorDestinatario.Text = "" Then
            fbcCompradorDestinatario.Text = Nothing
            fbcCompradorDestinatario.Value = Nothing
            scDomicilioCompradorDestinatario.Value = Nothing
            scDomicilioCompradorDestinatario.DataSource = Nothing
        Else
            If swcEsDestinatario.Checked = False Then
                Dim buscarDestinatario_ As New ControladorBusqueda(Of ConstructorProveedoresOperativos)
                Dim tagwatcher_ As TagWatcher = buscarDestinatario_.ObtenerDocumento(fbcCompradorDestinatario.Value)
                If tagwatcher_.Status = TypeStatus.Ok Then
                    Dim constructorDestinatario_ As ConstructorProveedoresOperativos = DirectCast(tagwatcher_.ObjectReturned.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente, ConstructorProveedoresOperativos)
                    Dim domiciliosDestinatario_ = constructorDestinatario_.Seccion(SeccionesProvedorOperativo.SPRO2).Nodos
                    Dim listaDomiciliosDestinatario_ As List(Of Rec.Globals.Empresas.Domicilio) = ListarDomicilios(domiciliosDestinatario_)
                    Dim datosDestinatario_ As New List(Of Dictionary(Of String, String))
                    For Each nodo_ In domiciliosDestinatario_
                        Dim listaDestinatarios_ As New Dictionary(Of String, String)
                        For Each item_ In nodo_.Nodos
                            Dim campo As Campo = DirectCast(item_.Nodos(0), Campo)
                            Select Case campo.IDUnico
                                Case CamposProveedorOperativo.CA_RFC_PROVEEDOR
                                    listaDestinatarios_.Add("RFC_", campo.Valor)
                                Case CamposProveedorOperativo.CA_CURP_PROVEEDOR
                                    listaDestinatarios_.Add("CURP_", campo.Valor)
                                Case CamposDomicilio.CA_CVE_PAIS
                                    listaDestinatarios_.Add("CvePais_", campo.Valor)
                                Case CamposDomicilio.CA_PAIS
                                    listaDestinatarios_.Add("Pais_", campo.Valor)
                            End Select
                        Next
                        With constructorDestinatario_.Seccion(SeccionesProvedorOperativo.SPRO1)
                            listaDestinatarios_.Add("RazonSocial_", .Campo(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR).Valor)
                            listaDestinatarios_.Add("ObjectId_", .Campo(CamposProveedorOperativo.CP_ID_EMPRESA).Valor.ToString)
                            listaDestinatarios_.Add("Cve_", .Campo(CamposProveedorOperativo.CP_CVE_PROVEEDOR).Valor)
                        End With
                        datosDestinatario_.Add(listaDestinatarios_)
                    Next
                    SetVars("_listaDomiciliosDestinatario", listaDomiciliosDestinatario_)
                    SetVars("_datosDestinatario", datosDestinatario_)
                    Dim dataSource_ As New List(Of SelectOption)
                    For index_ As Int32 = 0 To listaDomiciliosDestinatario_.Count - 1
                        dataSource_.Add(New SelectOption With
                                     {.Value = listaDomiciliosDestinatario_(index_)._iddomicilio.ToString,
                                      .Text = listaDomiciliosDestinatario_(index_).domicilioPresentacion})
                    Next
                    scDomicilioCompradorDestinatario.DataSource = Nothing
                    scDomicilioCompradorDestinatario.DataSource = dataSource_
                End If
            End If
        End If
    End Sub

    Protected Sub scDomicilioCompradorDestinatario_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub scDomicilioCompradorDestinatario_Click(sender As Object, e As EventArgs)

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

    Protected Sub scMetodoValoracion_Click(sender As Object, e As EventArgs)

    End Sub

    Protected Sub fbcPaisDestino_TextChanged(sender As Object, e As EventArgs)
        CargaPaises(sender)
    End Sub

    Protected Sub fbcPaisDestino_Click(sender As Object, e As EventArgs)

    End Sub

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

    Protected Sub pbPartidas_CheckedChange(sender As Object, e As EventArgs)

    End Sub
    Protected Sub pbPartidasItems_Click(sender As Object, e As EventArgs)
        Select Case pbPartidasItems.ToolbarAction
            Case PillboxControl.ToolbarActions.Nuevo
                lbNumero.Text = pbPartidasItems.PageIndex.ToString()
                MonedasUsd()
                MetodoValoracionInicial()
        End Select
    End Sub

    Protected Sub scMonedaFactura_Click(sender As Object, e As EventArgs)
        BusquedaMonedas(sender)
    End Sub

    'Protected Sub scMonedaFactura_TextChanged(sender As Object, e As EventArgs)

    'End Sub
    'Protected Sub scMonedaMercancia_Click(sender As Object, e As EventArgs)
    '    BusquedaMonedas(sender)
    'End Sub

    'Protected Sub scMonedaMercancia_TextChanged(sender As Object, e As EventArgs)

    'End Sub

    'Protected Sub scMonedaValorDolaresPartida_Click(sender As Object, e As EventArgs)
    '    BusquedaMonedas(sender)
    'End Sub

    'Protected Sub scMonedaValorUnitarioPartida_Click(sender As Object, e As EventArgs)
    '    BusquedaMonedas(sender)
    'End Sub

    'Protected Sub scMonedaPrecioUnitarioPartida_Click(sender As Object, e As EventArgs)
    '    BusquedaMonedas(sender)
    'End Sub

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
    '    ██████    1.ControladorEmpresas                        1. En Empresa:                         ██████
    '    ██████    2.ControladorRecursosAduanales                  a). Domicilios                      ██████
    '    ██████    3.ControladorSecuencias                         b). Contactos                       ██████
    '    ██████                                                                                        ██████
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████
    Function CargaPaises(ByRef control_ As FindboxControl) As List(Of SelectOption)
        Dim paisesTemporales_ As New List(Of Pais)
        Dim lista_ As List(Of SelectOption) = ControladorPaises.BuscarPaises(paisesTemporales_, control_.Text)
        control_.DataSource = lista_
        Return lista_
    End Function

    Protected Sub ListarMonedas(ByVal pais_ As Pais)
        Dim lista_ As List(Of SelectOption) = ControladorPaises.BuscarMonedasOficiales(pais_)
        Dim monedaActiva_ = lista_(0).Value
        scMonedaFactura.DataSource = lista_
        scMonedaFactura.Value = lista_(0).Value
        scMonedaMercancia.DataSource = lista_
        scMonedaMercancia.Value = lista_(0).Value
        scMonedaValorDolaresPartida.DataSource = lista_
        scMonedaValorDolaresPartida.Value = lista_(0).Value
        scMonedaValorUnitarioPartida.DataSource = lista_
        scMonedaValorUnitarioPartida.Value = lista_(0).Value
        scMonedaPrecioUnitarioPartida.DataSource = lista_
        scMonedaPrecioUnitarioPartida.Value = lista_(0).Value
    End Sub

    Protected Sub LimpiarMonedas()
        scMonedaFactura.DataSource = Nothing
        scMonedaMercancia.DataSource = Nothing
        scMonedaValorDolaresPartida.DataSource = Nothing
        scMonedaValorUnitarioPartida.DataSource = Nothing
        scMonedaPrecioUnitarioPartida.DataSource = Nothing
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

    Function BusquedaMonedas(ByRef control_ As SelectControl) As List(Of SelectOption)
        Dim listaMonedas_ = ControladorPaises.BuscarTodasMonedas(control_.SuggestedText)
        If listaMonedas_.Count > 0 Then
            control_.DataSource = listaMonedas_
        End If
        Return listaMonedas_
    End Function

    Protected Sub swcEsDestinatario_CheckedChanged(sender As Object, e As EventArgs)
        If swcEsDestinatario.Checked Then
            fbcCompradorDestinatario.Text = fbcCompradorReceptor.Text
            If GetVars("_domiciliosCompradorDestinatario") IsNot Nothing Then
                scDomicilioCompradorDestinatario.DataSource = DirectCast(GetVars("_domiciliosCompradorDestinatario"), List(Of SelectOption))
            Else
                scDomicilioCompradorDestinatario.DataSource = New List(Of SelectOption) _
                    From {New SelectOption With {.Value = scDomicilioCompradorReceptor.Value, .Text = scDomicilioCompradorReceptor.Text}}
            End If
            scDomicilioCompradorDestinatario.Value = scDomicilioCompradorReceptor.Value
        Else
            fbcCompradorDestinatario.Text = Nothing
            scDomicilioCompradorDestinatario.DataSource = Nothing
        End If
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

    Protected Sub MonedasUsd()
        Dim cveUSD_ = "635acf25a8210bfa0d58434e"
        Dim nombre_ = "USD"
        Dim dataSourceUSD_ = New List(Of SelectOption) From {New SelectOption With {.Value = cveUSD_, .Text = nombre_}}
        scMonedaFactura.DataSource = dataSourceUSD_
        scMonedaFactura.Value = cveUSD_
        scMonedaMercancia.DataSource = dataSourceUSD_
        scMonedaMercancia.Value = cveUSD_
        scMonedaValorDolaresPartida.DataSource = dataSourceUSD_
        scMonedaValorDolaresPartida.Value = cveUSD_
        scMonedaValorUnitarioPartida.DataSource = dataSourceUSD_
        scMonedaValorUnitarioPartida.Value = cveUSD_
        scMonedaPrecioUnitarioPartida.DataSource = dataSourceUSD_
        scMonedaPrecioUnitarioPartida.Value = cveUSD_
    End Sub

    Protected Sub MetodoValoracionInicial()
        Dim listaMetodoValoracion_ As List(Of SelectOption) = New List(Of SelectOption) _
                                                            From {New SelectOption With {.Value = 1, .Text = "0 - VALOR COMERCIAL (EXP)."}}
        scMetodoValoracion.DataSource = listaMetodoValoracion_
        scMetodoValoracionPartida.DataSource = listaMetodoValoracion_
        scMetodoValoracion.Value = 1
        scMetodoValoracionPartida.Value = 1
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
