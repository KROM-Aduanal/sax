
#Region "├┴┘├┴┘├┴┘├┴┘├┴┘|├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘   DEPENDENCIAS   ├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘"

'RECURSOS DEL CMF
Imports gsol.Web.Components
'OBJETOS BIDIMENSIONALES (ODF's.  Dependencias Krombase/SQL Server)
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Rec.Globals.Controllers
'OBJETOS DIMENSIONALES (ODS's) Dependencias en MongoDB
Imports Rec.Globals.Utils
Imports Rec.Globals.Utils.Secuencias
'UTILERIAS/RECURSOS ADICIONALES
Imports Sax.Web
Imports Syn.Documento
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Nucleo.RecursosComercioExterior.CamposProcesamientoElectDocumentos
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher
Imports Wma.Exceptions.TagWatcher.TypeStatus
Imports Syn.CustomBrokers.Controllers.ControladorProcesamientoElectronico
Imports Syn.CustomBrokers.Controllers
Imports TipoDocumentos = Syn.CustomBrokers.Controllers.IControladorProcesamientoElectronico.ListaTiposDocumentos
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox
Imports System.IO
Imports Ia.Pln


#End Region

Public Class Ges022_ProcesamientoElectronicoDocumentos
    Inherits ControladorBackend
#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████
    Private _cantidadDetalles As Int32
    Private _controladorDocumentos As New ControladorDocumento
    Private _idDocumento As ObjectId
    'Private _controladorProcesamientoElectronico As IControladorProcesamientoElectronico
    'Private _documentAnalizer As IControllerDocumentAnalyzer

#End Region

#Region "██████ Vinculación c/capas inf █████████       SAX      ████████████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████
    Public Overrides Sub Inicializa()
        With Buscador
            .DataObject = New ConstructorProcesamientoElectDocumentos(True)
            .addFilter(SeccionesProcesamientoElectDocumentos.SPED1, CamposProcesamientoElectDocumentos.CP_PREREFERENCIA_DOCUMENTO_PROCESADO, "Pre-referencia")
            .addFilter(SeccionesProcesamientoElectDocumentos.SPED1, CamposProcesamientoElectDocumentos.CP_RAZON_SOCIAL_CLIENTE, "Razón social cliente")
        End With
        If OperacionGenerica IsNot Nothing Then
            _cantidadDetalles = OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Seccion(SeccionesProcesamientoElectDocumentos.SPED3).CantidadPartidas
        End If
        lbEstadoIntegracionCompleto.Visible = True
        fsDetallesDocumentos.Enabled = False
        icRazonSocialCliente.Enabled = False
        lbButtonIA.Visible = False
        lbButtonXML.Visible = False



    End Sub

    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher
        ''Datos generales (SeccionesProveedorOperativo.SPRO1)
        [Set](fcpreReferencia, CP_PREREFERENCIA_DOCUMENTO_PROCESADO, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](fcpreReferencia, CP_PREREFERENCIA_DOCUMENTO_PROCESADO, propiedadDelControl_:=PropiedadesControl.Text, asignarA_:=TiposAsignacion.ValorPresentacion)
        [Set](scTipoDocumentos, CP_TIPO_DOCUMENTO_PROCESADO, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](icRazonSocialCliente, CP_RAZON_SOCIAL_CLIENTE, propiedadDelControl_:=PropiedadesControl.Valor)

        Return New TagWatcher(1)
    End Function

    Public Overrides Sub BotoneraClicNuevo()
        Dim modoEditando_ As Boolean = False
        If GetVars("isEditing") IsNot Nothing Then
            If GetVars("isEditing") = True Then
                modoEditando_ = True
            End If
        End If

        If OperacionGenerica IsNot Nothing Then
        End If

        ''validar cuando sea edicion
        lbEstadoIntegracionCompleto.Visible = False
        lbEstadoIntegracionBorrador.Visible = True
        fsDetallesDocumentos.Enabled = False
        areaDetalleDocumento.Visible = False



        'Aqui



        'Dim facturaRecibida_ As String = "{""processdate"": ""2023-10-05"", ""confidence"": 85}"

        'Dim factura As Factura = JsonConvert.DeserializeObject(Of Factura)(facturaRecibida_)

        '' Acceder a las propiedades de la clase
        'Dim ok = factura.ProcessDate
        'Dim aa = factura.Confidence

        '"invoicenumber": "SINTON830118201-2A",
        '"invoicedate": "2024/02/24",
        '"invoiceseries": null,
        '"customername":   "INDUSTRIAS MÁXIMAS S.A. DE C.V.",
        '"suppliername": "SHANDONG SINTON STEEL CORD CO., LTD.",
        '"invoicecountry": "CHINA",
        '"totalinvoice": 3276.00,
        '"invoicecurrency": "USD",
        '"info": "PROFORMA INVOICE FOR IMPORTED GOODS",
        '"state": "JALISCO",
        '"analysis": {
        '    "confidence": 85,
        '    "gptanalysis": true,
        '    "gpttokensupload": 0,
        '    "gpttokensdownload": 0,
        '    "textractanalysis": false,
        '    "textractpages": 0,
        '    "quantitydifferences": 0,
        '    "temperature": 0.7,
        '    "messages": []
        '},
        '"customer": {
        '    "customername": "INDUSTRIAS MÁXIMAS S.A. DE C.V.",
        '    "rfc": "MIBJ821213",
        '    "address": "CALLE CERO DEL TEO NO. 103",
        '    "street": "CALLE CERO DEL TEO",
        '    "externalnumber": "103",
        '    "internalnumber": null,
        '    "zipcode":  "28328",
        '    "city": "COLIMA",
        '    "locality": null,
        '    "state":  "COLIMA",
        '    "country": "MÉXICO"
        '},
        '"supplier": {
        '    "supliername": "SHANDONG SINTON STEEL CORD CO., LTD.",
        '    "taxid": null,
        '    "address":  "NO. 7, ZHONGKONG ROAD KENLI",
        '    "street": "ZHONGKONG ROAD",
        '    "externalnumber": null,
        '    "internalnumber": null,
        '    "zipcode": null,
        '    "locality": null,
        '    "city":  "SHANDONG",
        '    "state": null,
        '    "country":  "CHINA"
        '},
        '"items": [{
        '    "sec": "1",
        '    "sku": null,
        '    "partnumber":  "SINTON830118201-2A",
        '    "quantity": 5,
        '    "unit": "PCS",
        '    "description": "PLASTIC PALLETS",
        '    "total": 140.00,
        '    "currency": "USD",
        '    "usdvalue": 140.00,
        '    "value": 140.00,
        '    "discount": 0.00,
        '    "unitprice": 5.00,
        '    "netweight": 28.00,
        '    "purchaseorder": null,
        '    "destinationcountry":  "MÉXICO",
        '    "origincountry": "CHINA"
        '}, {
        '    "sec": "2",
        '    "sku": null,
        '    "partnumber":  "SINTON830118201-2A",
        '    "quantity": 3,
        '    "unit": "PCS",
        '    "description": "METAL SPOOLS",
        '    "total": 3240.00,
        '    "currency": "USD",
        '    "usdvalue": 3240.00,
        '    "value": 3240.00,
        '    "discount": 0.00,
        '    "unitprice": 3.00,
        '    "netweight": 1000.00,
        '    "purchaseorder": null,
        '    "destinationcountry":  "MÉXICO",
        '    "origincountry": "CHINA"
        '}, {
        '    "sec": "3",
        '    "sku": null,
        '    "partnumber":  "SINTON830118201-2A",
        '    "quantity": 1,
        '    "unit": "PCS",
        '    "description": "PLASTIC SEPARATORS",
        '    "total": 112.00,
        '    "currency": "USD",
        '    "usdvalue": 112.00,
        '    "value": 112.00,
        '    "discount": 0.00,
        '    "unitprice": 112.00,
        '    "netweight": 112.00,
        '    "purchaseorder": null,
        '    "destinationcountry":  "MÉXICO",
        '    "origincountry": "CHINA"
        '}],
        '"additionaldetails": {
        '    "purchaseorder": null,
        '    "totalweight":  1140.00,
        '    "packages": 3,
        '    "incoterm": null,
        '    "customerreference": null,
        '    "incrementalvalues":  []
        '},
        '"consigneedetails": {
        '    "consigneedetailsname": "MANZANILLO",
        '    "taxid": null,
        '    "address": null,
        '    "street": null,
        '    "externalnumber": null,
        '    "internalnumber": null,
        '    "zipcode": null,
        '    "locality": null,
        '    "city": null,
        '    "state": null,
        '    "country": null
        '})



    End Sub

    Public Overrides Sub BotoneraClicGuardar()
        If Not ProcesarTransaccion(Of ConstructorProcesamientoElectDocumentos)().Status = TypeStatus.Errors Then : End If
    End Sub

    Public Overrides Sub BotoneraClicEditar()

    End Sub

    Public Overrides Sub BotoneraClicBorrar()

    End Sub

    Public Overrides Sub BotoneraClicOtros(IndexSelected_ As Integer)
        If IndexSelected_ = 7 Then

        ElseIf IndexSelected_ = 8 Then

        End If
    End Sub



    'EVENTOS PARA LA INSERCIÓN DE DATOS
    Public Overrides Function AntesRealizarInsercion(ByVal session_ As IClientSessionHandle) As TagWatcher
        Dim tagwatcher_ As New TagWatcher
        If session_ IsNot Nothing Then
            tagwatcher_.SetOK()
        Else
            tagwatcher_.SetOK()
        End If
        Return tagwatcher_
    End Function

    Public Overrides Sub RealizarInsercion(ByRef documentoElectronico_ As DocumentoElectronico)
        'Dim empresaInternacional_ As New EmpresaInternacional
        'If GetVars("_empresaInternacional") IsNot Nothing Then
        '    empresaInternacional_ = DirectCast(GetVars("_empresaInternacional"), EmpresaInternacional)
        'End If
        'Dim loginUsuario_ As Dictionary(Of String, String) = Session("DatosUsuario")
        'Dim secuencia_ As Secuencia = GenerarSecuencia()
        With documentoElectronico_
            '.Campo(CP_TIPO_PROVEEDOR).Valor = 2
            '.Campo(CP_TIPO_PROVEEDOR).ValorPresentacion = "Extranjero"

            'If swcTipoUso.Checked Then
            '    .Campo(CP_TIPO_USO).Valor = 2
            '    .Campo(CP_TIPO_USO).ValorPresentacion = "Exportación"
            'Else
            '    .Campo(CP_TIPO_USO).Valor = 1
            '    .Campo(CP_TIPO_USO).ValorPresentacion = "Importación"
            'End If

            'icClave.Value = secuencia_.sec
            'idEmpresa.Value = empresaInternacional_._id.ToString
            'cveEmpresa.Value = empresaInternacional_._idempresa
            '.UsuarioGenerador = loginUsuario_("Nombre")
            '.Id = secuencia_._id.ToString
            '.IdDocumento = secuencia_.sec
            '.FolioDocumento = secuencia_.sec 'DUDA
            '.FolioOperacion = empresaInternacional_._idempresa 'DUDA
            '.TipoPropietario = secuencia_.nombre
            '.NombrePropietario = empresaInternacional_.razonsocial
            '.IdPropietario = empresaInternacional_._idempresa
            '.ObjectIdPropietario = empresaInternacional_._id
        End With
    End Sub

    Public Overrides Function DespuesRealizarInsercion() As TagWatcher
        Return New TagWatcher(Ok)
    End Function

    'EVENTOS PARA MODIFICACIÓN DE DATOS
    Public Overrides Function AntesRealizarModificacion(ByVal session_ As IClientSessionHandle) As TagWatcher
        Dim tagwatcher_ As New TagWatcher
        If session_ IsNot Nothing Then '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 
            tagwatcher_.SetOK()
        Else  '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas sin transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 
            tagwatcher_.SetOK()
        End If
        Return tagwatcher_
    End Function

    Public Overrides Sub PreparaModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

    End Sub

    Public Overrides Sub RealizarModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

    End Sub

    Public Overrides Sub DespuesOperadorDatosProcesar(ByRef documentoElectronico_ As DocumentoElectronico)
        With documentoElectronico_

        End With

    End Sub

    Public Overrides Function DespuesRealizarModificacion() As TagWatcher

        Return New TagWatcher(Ok)
    End Function

    'EVENTOS PARA PRESENTACIÓN DE DATOS EN FRONTEND
    Public Overrides Sub DespuesBuquedaGeneralConDatos()

    End Sub

    Public Overrides Sub DespuesBuquedaGeneralSinDatos()

    End Sub
    'EVENTOS DE MANTENIMIENTO
    Public Overrides Sub LimpiaSesion()

    End Sub

    Public Overrides Sub Limpiar()

    End Sub
#End Region

#Region "████████████████  QUINTA CAPA  █████████       Reglas locales         ██████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████
    'EVENTO PARA REGRESAR CONTROLES POR CADA ACCIÓN DE TARJETA
    Protected Sub fcpreReferencia_Click(sender As Object, e As EventArgs)
        Dim dataSource_ As New List(Of SelectOption) From {
        New SelectOption With {.Value = "6545488b6f2eb53a98863c91", .Text = "PRE-RKU23-00000718"}}
        fcpreReferencia.DataSource = dataSource_
    End Sub

    Protected Sub fcpreReferencia_TextChanged(sender As Object, e As EventArgs)

        If fcpreReferencia.Value <> "" Then
            icRazonSocialCliente.Value = "COLGATE PALMOLIVE S.A DE C.V."
            fsDetallesDocumentos.Enabled = True
        End If

    End Sub

    Protected Sub scTipoDocumentos_Click(sender As Object, e As EventArgs)
        scTipoDocumentos.DataSource = ListaDocumentos()
    End Sub

    Protected Sub scTipoDocumentos_SelectedIndexChanged(sender As Object, e As EventArgs)
        'If scTipoDocumentos.Value = TipoDocumentos.FACTURA_COMERCIAL_IMPORTACION_PDF Or
        '    scTipoDocumentos.Value = TipoDocumentos.FACTURA_COMERCIAL_EXPORTACION_PDF Then
        '    lbButtonXML.Visible = False
        '    lbButtonIA.Visible = True
        'Else
        '    lbButtonXML.Visible = True
        '    lbButtonIA.Visible = False
        'End If
    End Sub

    Protected Sub fcTipoDocumentos_TextChanged(sender As Object, e As EventArgs)


    End Sub

    Protected Sub fcDocumento_ChooseFile(sender As PropiedadesDocumento, e As EventArgs)
        Dim id = ObjectId.GenerateNewId().ToString
        With sender
            ._idpropietario = id
            .nombrepropietario = "Factura comercial"
            .tipovinculacion = PropiedadesDocumento.TiposVinculacion.AgenciaAduanal
            .datosadicionales = New InformacionDocumento With {
                          .foliodocumento = "00000002",
                          .tipodocumento = InformacionDocumento.TiposDocumento.BL,
                          .datospropietario = New InformacionPropietario With {
                              .nombrepropietario = "Factura comercial",
                              ._id = id
                          }
                         }
            .formatoarchivo = PropiedadesDocumento.FormatosArchivo.pdf
            'lbButtonXML.Enabled = True
            'lbButtonIA.Enabled = True
        End With
    End Sub

    Protected Sub btnIAProcesar_Click(sender As Object, e As EventArgs)
        Dim listaDocumentos_ As List(Of Newtonsoft.Json.Linq.JObject) =
            Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of Newtonsoft.Json.Linq.JObject))(fcDocumento.Value)

        Dim listaIdsDocumento_ As List(Of ObjectId) = New List(Of ObjectId)
        If listaDocumentos_ IsNot Nothing Then
            panelCarga.Visible = False
            areaDetalleDocumento.Visible = True
            btnGenerarFactura.Visible = True
            Dim i_ = 1
            For Each documento_ In listaDocumentos_
                Dim icon = $"<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 512 512'><path fill='#f1f1f1' d='M464 256A208 208 0 1 0 48 256a208 208 0 1 0 416 0zM0 256a256 256 0 1 1 512 0A256 256 0 1 1 0 256zM188.3 147.1c7.6-4.2 16.8-4.1 24.3 .5l144 88c7.1 4.4 11.5 12.1 11.5 20.5s-4.4 16.1-11.5 20.5l-144 88c-7.4 4.5-16.7 4.7-24.3 .5s-12.3-12.2-12.3-20.9l0-176c0-8.7 4.7-16.7 12.3-20.9z'/></svg>"
                Dim icon_estado = $"<p><svg xmlns='http://www.w3.org/2000/svg' height='25' width='25' style='margin:5px; align-text:center' viewBox='0 0 512 512'><path fill='#0ea881' d='M448 256c0-106-86-192-192-192l0 384c106 0 192-86 192-192zM0 256a256 256 0 1 1 512 0A256 256 0 1 1 0 256z'/></svg></p>"
                Dim facturajson_ = documento_.SelectToken("fileId").ToString
                ccDocumentosProcesados.SetRow(Sub(catalogRow_ As CatalogRow)
                                                  catalogRow_.SetIndice(ccDocumentosProcesados.KeyField, facturajson_)
                                                  catalogRow_.SetColumn(icNombreDocumento, documento_.SelectToken("fileName").ToString)
                                                  catalogRow_.SetColumn(icFechaProceso, "2024-10-15")
                                                  catalogRow_.SetColumn(icEmisor, "CLIENTE (FACTURA COMERCIAL)")
                                                  catalogRow_.SetColumn(icPorcentaje, "89%")
                                                  catalogRow_.SetColumn(icEstatus, icon_estado)
                                                  i_ += 1
                                              End Sub)

                ccDocumentosProcesados.CatalogDataBinding()
                listaIdsDocumento_.Add(ObjectId.Parse(facturajson_))
            Next
        End If

    End Sub

    Protected Sub ccDocumentosProcesados_Click(sender As Object, e As EventArgs)

        'Dim ok = ccDocumentosProcesados.CanSelect

        'Dim id = ccDocumentosProcesados.ID


    End Sub

    Public Sub btnGenerarPreasignacion_Click(sender As Object, e As EventArgs)
        ''HACIENDO PRUEBAS AL VUELO
        ''PERO ES TAREA DEL CONTROLADOR DE PROCESAMIENTO
        ''DEBO GENERAR LAS FUNCIONES QUE FUTURAMENTE TENDRÁ
        ''LO HARÉ DESPUES DE COMER
        ''FUNCIONES PARA TRABAJAR CON IA
        ''FUNCIONES PARA DESEREALIZAR
        ''FUNCIONES PARA GENERAR LA PREASIGNACION, EN ESTE CASO HAREMOS LA PRUEBA CON UNA FACTURA COMERCIAL



    End Sub



    Protected Sub icDocumentos_ChooseFile(sender As PropiedadesDocumento, e As EventArgs)

        Dim id = ObjectId.GenerateNewId().ToString

        With sender
            ._idpropietario = id
            .nombrepropietario = "Factura comercial"
            .tipovinculacion = PropiedadesDocumento.TiposVinculacion.AgenciaAduanal
            .datosadicionales = New InformacionDocumento With {
                          .foliodocumento = "00000002",
                          .tipodocumento = InformacionDocumento.TiposDocumento.BL,
                          .datospropietario = New InformacionPropietario With {
                              .nombrepropietario = "Factura comercial",
                              ._id = id
                          }
                         }
            .formatoarchivo = PropiedadesDocumento.FormatosArchivo.pdf
            'lbButtonXML.Enabled = True
            'lbButtonIA.Enabled = True
        End With

        _idDocumento = ObjectId.Parse(id)

    End Sub



    Protected Sub btnXML_Click(sender As Object, e As EventArgs)

        'Dim aquiui = True
        'Dim listaDocumentos_ As List(Of Newtonsoft.Json.Linq.JObject) = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of Newtonsoft.Json.Linq.JObject))(fcDocumento.Value)
        'For Each documento_ In listaDocumentos_
        '    Dim aaaa = documento_

        'ccDocumentos.SetRow(Sub(catalogRow_ As CatalogRow)

        '                        catalogRow_.SetIndice(ccDocumentos.KeyField, 0)

        '                        catalogRow_.SetColumn(icArchivo, New SelectOption With {.Value = documento_.SelectToken("fileId").ToString, .Text = documento_.SelectToken("fileName").ToString})

        '                        catalogRow_.SetColumn(icTipoArchivo, New SelectOption With {.Value = scTipoDocumentos.Value, .Text = scTipoDocumentos.Text})

        '                    End Sub)

        'ccDocumentos.CatalogDataBinding()

        'listaIdsDocumento_.Add(ObjectId.Parse(documento_.SelectToken("fileId").ToString))

        'Next

        'Dim doc As Byte() = _controladorDocumentos.GetDocument(listaIdsDocumento_(0)).ObjectReturned

        'Dim memoryStream_ As New MemoryStream(doc)


    End Sub
#End Region

#Region "██████ Vinculación sexta capa  █████████       SAX      ████████████████████████████████████████████"
    '    ██████   Controladores utilizados                     Documentos por coding para MongoDB      ██████
    '    ██████    1.ControladorEmpresas                        1. En Empresa:                         ██████
    '    ██████    2.ControladorRecursosAduanales                  a). Domicilios                      ██████
    '    ██████    3.ControladorSecuencias                         b). Contactos                       ██████
    '    ██████                                                                                        ██████
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████
    Protected Function GenerarSecuencia() As Secuencia
        Dim secuencia_ As New Secuencia
        Dim controladorSecuencias_ As ControladorSecuencia = New ControladorSecuencia
        Dim tagwatcher_ As TagWatcher = controladorSecuencias_.Generar(SecuenciasComercioExterior.ProveedoresOperativos.ToString, 1, 1, 1, 1, 1)
        If tagwatcher_.Status = TypeStatus.Ok Then
            secuencia_ = DirectCast(tagwatcher_.ObjectReturned, Secuencia)
            secuencia_.nombre = SecuenciasComercioExterior.ProveedoresOperativos.ToString
        End If
        Return secuencia_
    End Function

    Public Overrides Function AgregarComponentesBloqueadosInicial() As List(Of WebControl)
        Dim bloqueados_ As New List(Of WebControl) From {icRazonSocialCliente}
        Return bloqueados_
    End Function

    Public Overrides Function AgregarComponentesBloqueadosEdicion() As List(Of WebControl)
        Dim bloqueadosEdicion_ As New List(Of WebControl) From {icRazonSocialCliente}
        Return bloqueadosEdicion_
    End Function

    'Protected Sub MsgEstadoCompletado()
    '    lbEstadoCompletado.ToolTip = "Estado completado"
    '    lbEstadoCompletado.ToolTipExpireTime = 4
    '    fcRazonSocial.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo
    '    fcRazonSocial.ToolTipModality = IUIControl.ToolTipModalities.Interactive
    '    fcRazonSocial.ShowToolTip()
    'End Sub

    Protected Function ListaDocumentos() As List(Of SelectOption)
        'Dim listaTiposDocumentos_ As Dictionary(Of Int16, String) = ControladorProcesamientoElectronico.ListaTiposDocumentos.ObjectReturned
        'Dim dataSource_ As New List(Of SelectOption)
        'For Each item_ In listaTiposDocumentos_
        '    dataSource_.Add(New SelectOption With {.Value = item_.Key, .Text = item_.Value})
        'Next
        'Return dataSource_
    End Function

#End Region
End Class

