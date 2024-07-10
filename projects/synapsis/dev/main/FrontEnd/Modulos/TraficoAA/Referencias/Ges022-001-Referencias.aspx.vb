
#Region "├┴┘├┴┘├┴┘├┴┘├┴┘|├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘   DEPENDENCIAS   ├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘"

'RECURSOS DEL CMF
Imports MongoDB.Driver
Imports Syn.Documento
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Operaciones
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher
Imports Wma.Exceptions.TagWatcher.TypeStatus
Imports Syn.Nucleo.RecursosComercioExterior.CamposReferencia

'OBJETOS DIMENSIONALES (ODS's) Dependencias en MongoDB
Imports Rec.Globals.Controllers
Imports gsol

'UTILERIAS/RECURSOS ADICIONALES
Imports Sax.Web
Imports Rec.Globals.Utils
Imports Syn.CustomBrokers.Controllers.ControladorRecursosAduanales
Imports Syn.CustomBrokers.Controllers
Imports gsol.krom
Imports MongoDB.Bson
Imports SharpCompress.Common
Imports System.IO
Imports System.Web.Hosting
Imports Syn.CustomBrokers.Controllers.ControladorUnidadesMedida
Imports System.Windows.Forms
Imports System.Linq
Imports gsol.Web.Components
Imports Syn.Documento.Componentes
Imports MongoDB.Bson.Serialization.Attributes
Imports System.Drawing.Imaging










#End Region

Public Class Ges022_001_Referencia
    Inherits ControladorBackend

#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Private _controladorReferencias As IControladorReferencias

    Private _controladorDocumentos As New ControladorDocumento

    Private _idDocumento As ObjectId

    Private Enum TipoReferenciaPedimento
        Prefijo = 1
        Sufijo
        Completo
    End Enum

#End Region

#Region "██████ Vinculación c/capas inf █████████       SAX      ████████████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████


    '------ Sobreescrituras del framework Sax ------------
    Public Overrides Sub Inicializa()

        With Buscador

            .DataObject = New ConstructorReferencia()


            .addFilter(SeccionesReferencias.SREF1, CamposReferencia.CP_REFERENCIA, "Referencia")
            .addFilter(SeccionesReferencias.SREF1, CamposPedimento.CA_NUMERO_PEDIMENTO_COMPLETO, "Pedimento")
            .addFilter(SeccionesReferencias.SREF2, CamposClientes.CA_RAZON_SOCIAL, "Cliente")


        End With

        'If Not Page.IsPostBack Then

        scRegimen.DataEntity = New krom.Anexo22()

        scClaveDocumento.DataEntity = New krom.Anexo22()

        scPais.DataEntity = New krom.Anexo22()

        scPaisMulti.DataEntity = New krom.Anexo22()

        'scAduanaDespacho.DataEntity = New krom.Anexo22

        scEjecutivoCuenta.DataEntity = New krom.Ejecutivos

        scEjecutivoCuenta.FreeClauses = " and i_Cve_DivisionMiEmpresa = " & Statements.GetOfficeOnline()._id

        If scGuias.Checked = False Then

            ccGuias.Visible = False

            pnGuia.Visible = True

        Else

            ccGuias.Visible = True

            pnGuia.Visible = False


        End If

        '_controladorReferencias = New ControladorReferencias

        'End If

        icRFC.Enabled = False

    End Sub

    Public Overrides Sub BotoneraClicNuevo()

        If OperacionGenerica IsNot Nothing Then

            '_empresa = Nothing

        End If

        swcMaterialPeligroso.Checked = False
        swcRectificacion.Checked = False
        swcTipoOperacion.Checked = True
        ccDocumento.Visible = True
        swcTipoOperacion.Enabled = True
        swcRectificacion.Enabled = True
        Fechas.Visible = True
        Guia.Visible = True
        ccGuias.Visible = False

        scTipoDocumentos.DataSource = New List(Of SelectOption) From {New SelectOption With {.Value = 1, .Text = "BL"}}
        scTipoDocumentos.Value = 1

        TrackingExpo.Visible = False
        TrackingImpo.Visible = False
        Fechas.Visible = False
        Documentos.Visible = True

        PreparaControles()

        icRFC.Enabled = False

        dbcReferencia.EnabledButton = False

    End Sub

    Public Overrides Sub BotoneraClicGuardar()

        If Not ProcesarTransaccion(Of ConstructorReferencia)().Status = TypeStatus.Errors Then : End If

    End Sub

    Public Overrides Sub BotoneraClicEditar()

        InicializaPrefijo()

        ccDocumento.Visible = False
        Fechas.Visible = True
        Guia.Visible = True

        If swcTipoOperacion.Checked Then

            Guia.Visible = True

            'TrackingImpo.Visible = True

            'TrackingExpo.Visible = False

            icFechaEtd.Visible = False

            icFechaPresentacion.Visible = False

            icFechaSalida.Visible = False

            icFechaCierreFisico.Visible = False

            icFechaEta.Visible = True

            icFechaRevalidacion.Visible = True

        Else

            Guia.Visible = False

            'TrackingExpo.Visible = True

            'TrackingImpo.Visible = False

            icFechaEtd.Visible = True

            icFechaPresentacion.Visible = True

            icFechaSalida.Visible = True

            icFechaCierreFisico.Visible = True

            icFechaEta.Visible = False

            icFechaRevalidacion.Visible = False



        End If


        dbcReferencia.EnabledButton = True

    End Sub

    Public Overrides Sub BotoneraClicBorrar()


    End Sub


    Public Overrides Sub BotoneraClicOtros(ByVal IndexSelected_ As Integer)



        If IndexSelected_ = 10 Then

            Dim pdfBytes_ As Byte() = File.ReadAllBytes("C:\TEMP\Ejemplo_BL.pdf")

            Dim ms_ As New MemoryStream(pdfBytes_)

            'Dim x = _controladorReferencias.CrearPrereferencia(ms_)

        End If

    End Sub

    Public Overrides Function AgregarComponentesBloqueadosInicial() As List(Of WebControl)

        Dim bloqueados_ As New List(Of WebControl)

        bloqueados_.Add(icRFC)
        bloqueados_.Add(icRFCFacturacion)
        bloqueados_.Add(icArchivo)
        bloqueados_.Add(icTipoArchivo)

        Return bloqueados_

    End Function

    Public Overrides Function AgregarComponentesBloqueadosEdicion() As List(Of WebControl)

        Dim bloqueadosEdicion_ As New List(Of WebControl)

        bloqueadosEdicion_.Add(swcTipoOperacion)
        bloqueadosEdicion_.Add(swcRectificacion)
        bloqueadosEdicion_.Add(scTipoDocumento)
        bloqueadosEdicion_.Add(scEjecutivoCuenta)
        bloqueadosEdicion_.Add(scPatente)
        bloqueadosEdicion_.Add(scClaveDocumento)
        bloqueadosEdicion_.Add(scRegimen)
        bloqueadosEdicion_.Add(dbcReferencia)
        bloqueadosEdicion_.Add(fbcCliente)
        bloqueadosEdicion_.Add(icBancoPago)

        Return bloqueadosEdicion_

    End Function


    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher

        [Set](dbcReferencia, CP_REFERENCIA, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](dbcReferencia, CamposPedimento.CA_NUMERO_PEDIMENTO_COMPLETO, propiedadDelControl_:=PropiedadesControl.ValueDetail)
        [Set](swcTipoOperacion, CamposPedimento.CA_TIPO_OPERACION, propiedadDelControl_:=PropiedadesControl.Checked)
        [Set](swcMaterialPeligroso, CP_MATERIAL_PELIGROSO, propiedadDelControl_:=PropiedadesControl.Checked)
        [Set](swcRectificacion, CP_RECTIFICACION, propiedadDelControl_:=PropiedadesControl.Checked)
        [Set](scPatente, CamposPedimento.CP_MODALIDAD_ADUANA_PATENTE)
        [Set](scRegimen, CamposPedimento.CA_REGIMEN)
        [Set](scTipoDocumento, CA_TIPO_PEDIMENTO)
        [Set](scClaveDocumento, CamposPedimento.CA_CVE_PEDIMENTO)
        '[Set](scDesaduanamiento, CP_DESADUANAMIENTO)
        [Set](scEjecutivoCuenta, CamposPedimento.CP_EJECUTIVO_CUENTA)
        [Set](scTipoCarga, CP_TIPO_CARGA_AGENCIA)
        [Set](icDescripcionCompleta, CP_DESCRIPCION_MERCANCIA_COMPLETA)

        [Set](fbcCliente, CamposClientes.CP_OBJECTID_CLIENTE)
        [Set](fbcCliente, CamposClientes.CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Text)
        [Set](icRFC, CamposClientes.CA_RFC_CLIENTE)
        [Set](icRFCFacturacion, CamposClientes.CP_RFC_FACTURACION)
        [Set](icBancoPago, CamposClientes.CP_CVE_BANCO)

        '[Set](icFechaApertura, CP_FECHA_APERTURA)
        '[Set](icFechaEntrada, CamposPedimento.CA_FECHA_ENTRADA)
        '[Set](icFechaProforma, CP_FECHA_PROFORMA)
        '[Set](icFechaCierre, CP_FECHA_CIERRE)
        '[Set](icFechaPago, CP_FECHA_PAGO)
        '[Set](icFechaDespacho, CP_FECHA_ULTIMO_DESPACHO)

        [Set](icFechaEta, CP_FECHA_ETA, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](icFechaRevalidacion, CP_FECHA_REVALIDACION, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](icFechaPrevio, CP_FECHA_PREVIO, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](icFechaPrevio, CP_FECHA_PREVIO, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](icFechaPresentacion, CP_FECHA_PRESENTACION, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](icFechaSalida, CP_FECHA_SALIDA, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](icFechaEtd, CP_FECHA_ETD, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](icFechaCierreFisico, CP_FECHA_CIERRE_FISICO, propiedadDelControl_:=PropiedadesControl.Valor)

        [Set](scRecintoFiscal, CP_RECINTO_FISCAL)
        [Set](scGuias, CP_GUIA_MULTIPLE, propiedadDelControl_:=PropiedadesControl.Checked)

        [Set](icArchivo, CP_NOMBRE_DOCUMENTO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icTipoArchivo, CP_TIPO_DOCUMENTO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ccDocumentos, Nothing, seccion_:=SeccionesReferencias.SREF6)

        [Set](icNumeroGuiaMulti, CP_NUMERO_GUIA_MULTIPLE, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scTransportistaMulti, CP_TRANSPORTISTA_MULTIPLE, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scPaisMulti, CP_PAIS_MULTIPLE, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icTipoCargaGuiaMulti, CP_TIPO_CARGA_MULTIPLE, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scPesoBrutoMulti, CP_PESOBRUTO_MULTIPLE, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scUnidadMedidaMulti, CP_UNIDADMEDIDA_MULTIPLE, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scTipoGuiaMulti, CP_TIPO_GUIA_MULTIPLE, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icFechaSalidaOrigenMulti, CP_FECHA_SALIDA_ORIGEN_MULTIPLE, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ccGuias, Nothing, seccion_:=SeccionesReferencias.SREF7)

        [Set](icNumeroGuia, CP_NUMERO_GUIA, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](scTransportista, CP_TRANSPORTISTA, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](scPais, CP_PAIS, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](icTipoCargaGuia, CP_TIPO_CARGA, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](scPesoBruto, CP_PESOBRUTO, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](scUnidadMedida, CP_UNIDADMEDIDA, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](scTipoGuia, CP_TIPO_GUIA, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](icFechaSalidaOrigen, CP_FECHA_SALIDA_ORIGEN, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](icDescripcionMercancia, CP_DESCRIPCION_MERCANCIA, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](icConsignatario, CP_CONSIGNATARIO, propiedadDelControl_:=PropiedadesControl.Valor)

        Return New TagWatcher(1)

    End Function

    Protected Sub PreparaControles()
        'Inicizliza prefijo
        InicializaPrefijo()

        'GeneraPrefijoReferencia
        dbcReferencia.Value = GeneraReferenciaPedimento(True, False, TipoReferenciaPedimento.Prefijo)

    End Sub


    'EVENTOS PARA LA INSERCIÓN DE DATOS
    Public Overrides Function AntesRealizarInsercion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim modalidadSeccionPatente_ As ControladorRecursosAduanales = GetVars("modalidadSeccionPatente")

        Dim seccionesPatente_ = From data In modalidadSeccionPatente_.aduanaspatentes
                                Where data._idmodalidadaduanapatente.ToString.Equals(scPatente.Value)
                                Select data._idaduanaseccion, data._idpatente

        If seccionesPatente_.Count > 0 Then

            Dim seccion_ As String = seccionesPatente_(0)._idaduanaseccion.ToString

            Dim patente_ = seccionesPatente_(0)._idpatente.ToString

            [Set](seccion_, CamposPedimento.CA_ADUANA_ENTRADA_SALIDA)

            [Set](patente_, CamposPedimento.CA_PATENTE,)

        End If



        _controladorReferencias = New ControladorReferencias

        dbcReferencia.Value = dbcReferencia.Value & _controladorReferencias.GeneraSecuencia("Referencias", Statements.GetOfficeOnline._id, Year(Now), 0, 0, 0, scPrefijo.Value).ToString.PadLeft(8, "0")

        Return New TagWatcher(Ok)

    End Function

    Public Overrides Sub RealizarInsercion(ByRef documentoElectronico_ As DocumentoElectronico)

        With documentoElectronico_

            .FolioDocumento = dbcReferencia.ValueDetail

            .FolioOperacion = dbcReferencia.Value '"RKU22-" & GeneraSecuencia("Referencias", Statements.GetOfficeOnline._id, Year(Now), 0, 0, 0, "RKU").ToString.PadLeft(8, "0")

            .IdCliente = 0

            .NombreCliente = fbcCliente.Text

        End With

    End Sub

    Public Overrides Function DespuesRealizarInsercion() As TagWatcher

        Return New TagWatcher(1)

    End Function


    'EVENTOS PARA MODIFICACIÓN DE DATOS
    Public Overrides Function AntesRealizarModificacion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Return New TagWatcher(1) 'tagwatcher_

    End Function

    Public Overrides Sub RealizarModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

        With documentoElectronico_

            .FolioOperacion = dbcReferencia.Value

            .FolioDocumento = dbcReferencia.ValueDetail

            .NombreCliente = fbcCliente.Text

        End With

    End Sub

    Public Overrides Function DespuesRealizarModificacion() As TagWatcher

        Return New TagWatcher(1)

    End Function


    'EVENTOS PARA PRESENTACIÓN DE DATOS EN FRONTEND
    Public Overrides Sub PreparaModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

    End Sub


    'EVENTO PARA DATOS EXTRA/AUTOMATICOS
    Protected Function GeneraSecuencia(ByVal nombre_ As String,
                                       Optional ByVal enviroment_ As Int16 = 0,
                                       Optional ByVal anio_ As Int16 = 0,
                                       Optional ByVal mes_ As Int16 = 0,
                                       Optional ByVal tipoSecuencia_ As Integer = 0,
                                       Optional ByVal subTipoSecuencia_ As Integer = 0,
                                       Optional ByVal prefijo As String = Nothing
                                       ) As Int32

        ''* ** * ** Generador de secuencias referencias ** * ** *
        Dim secuencia_ As New Syn.Operaciones.Secuencia _
                  With {.nombre = nombre_,
                      .environment = enviroment_,
                      .anio = anio_,
                      .mes = mes_,
                      .tiposecuencia = tipoSecuencia_,
                      .subtiposecuencia = subTipoSecuencia_,
                      .prefijo = prefijo
                      }

        Dim respuesta_ As TagWatcher = secuencia_.Generar().Result

        Dim sec_ As Int32 = 0

        Select Case respuesta_.Status

            Case TypeStatus.Ok

                sec_ = respuesta_.ObjectReturned.sec

            Case Else

        End Select

        Return sec_

    End Function

    Public Overrides Sub DespuesBuquedaGeneralConDatos()

        Dim swMultiple = OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Attribute(CP_GUIA_MULTIPLE).Valor

        'Dim tip = OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Attribute(CP_TIPO_DOCUMENTO).Valor

        Dim fuente_ = OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

        If swMultiple Then

            ccGuias.Visible = True

            pnGuia.Visible = False

        Else

            ccGuias.Visible = False

            pnGuia.Visible = True

        End If

        If fuente_.Attribute(CamposPedimento.CA_TIPO_OPERACION).Valor = True Then

            Guia.Visible = True

            'TrackingImpo.Visible = True

            'TrackingExpo.Visible = False

            icFechaEtd.Visible = False

            icFechaPresentacion.Visible = False

            icFechaSalida.Visible = False

            icFechaCierreFisico.Visible = False

            icFechaEta.Visible = True

            icFechaRevalidacion.Visible = True





        Else

            Guia.Visible = False

            'TrackingExpo.Visible = True

            'TrackingImpo.Visible = False

            icFechaEtd.Visible = True

            icFechaPresentacion.Visible = True

            icFechaSalida.Visible = True

            icFechaCierreFisico.Visible = True

            icFechaEta.Visible = False

            icFechaRevalidacion.Visible = False

        End If

        Fechas.Visible = True

        swcRectificacion.Visible = True

        Documentos.Visible = True

        scGuias.Enabled = False


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

    Protected Sub btGuardarDocumento_OnClick(sender As Object, e As EventArgs)

        Dim listaDocumentos_ As List(Of Newtonsoft.Json.Linq.JObject) = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of Newtonsoft.Json.Linq.JObject))(fcDocumento.Value)

        Dim listaIdsDocumento_ As New List(Of ObjectId)

        For Each documento_ In listaDocumentos_

            ccDocumentos.SetRow(Sub(catalogRow_ As CatalogRow)

                                    catalogRow_.SetIndice(ccDocumentos.KeyField, 0)

                                    catalogRow_.SetColumn(icArchivo, New SelectOption With {.Value = documento_.SelectToken("fileId").ToString, .Text = documento_.SelectToken("fileName").ToString})

                                    catalogRow_.SetColumn(icTipoArchivo, New SelectOption With {.Value = scTipoDocumentos.Value, .Text = scTipoDocumentos.Text})

                                End Sub)

            ccDocumentos.CatalogDataBinding()

            listaIdsDocumento_.Add(ObjectId.Parse(documento_.SelectToken("fileId").ToString))

        Next

        Dim doc As Byte() = _controladorDocumentos.GetDocument(listaIdsDocumento_(0)).ObjectReturned

        Dim memoryStream_ As New MemoryStream(doc)

        _controladorReferencias = New ControladorReferencias

        Dim referencia As Syn.CustomBrokers.Controllers.Referencia

        Dim facturaComercial

        Select Case scTipoDocumentos.Value
            Case 1
                referencia = _controladorReferencias.CrearPrereferencia(memoryStream_, Ia.Pln.IControllerChatGPT.DocumentoCargado.BL).ObjectReturned

                If referencia._importacion._guia._listaGuias(0)._guia.Equals("MEDUET7473626") Then

                    icNumeroGuia.Value = "MEDUE7473626"

                Else

                    icNumeroGuia.Value = referencia._importacion._guia._listaGuias(0)._guia

                End If
                scTransportista.Value = referencia._importacion._guia._listaGuias(0)._transportista
                scPais.DataSource = New List(Of SelectOption) From {New SelectOption With {.Value = 1, .Text = referencia._importacion._guia._listaGuias(0)._pais}}
                scPais.Value = 1
                scUnidadMedida.DataSource = New List(Of SelectOption) From {New SelectOption With {.Value = 1, .Text = referencia._importacion._guia._listaGuias(0)._unidadMedida}}
                scUnidadMedida.Value = 1
                scTipoGuia.DataSource = New List(Of SelectOption) From {New SelectOption With {.Value = 1, .Text = "Master"}}
                scTipoGuia.Value = 1
                scTipoGuia.ToolTip = "Sugerido por sistema, confirmar"
                scTipoGuia.ToolTipModality = Web.Components.IUIControl.ToolTipModalities.Ondemand
                scTipoGuia.ToolTipStatus = Web.Components.IUIControl.ToolTipTypeStatus.OkInfo
                icTipoCargaGuia.Value = referencia._importacion._guia._listaGuias(0)._tipoCarga
                scPesoBruto.Value = referencia._importacion._guia._listaGuias(0).perBruto
                icFechaSalidaOrigen.Value = referencia._importacion._guia._listaGuias(0)._salidaOrigen
                icDescripcionMercancia.Value = referencia._importacion._guia._listaGuias(0)._descripcionMercancia
                icConsignatario.Value = referencia._importacion._guia._listaGuias(0)._consignatario

                ccDocumento.Visible = False

                scRegimen.DataSource = New List(Of SelectOption) From {New SelectOption With {.Value = "IMD", .Text = "IMD - DEFINITIVO DE IMPORTACIÓN."}}
                scRegimen.Value = "IMD"
                scRegimen.ToolTip = "Sugerido por sistema, confirmar"
                scRegimen.ToolTipModality = Web.Components.IUIControl.ToolTipModalities.Ondemand
                scRegimen.ToolTipStatus = Web.Components.IUIControl.ToolTipTypeStatus.OkInfo

                scClaveDocumento.DataSource = New List(Of SelectOption) From {New SelectOption With {.Value = 8, .Text = "A1"}}
                scClaveDocumento.Value = 8
                scClaveDocumento.ToolTip = "Sugerido por sistema, confirmar"
                scClaveDocumento.ToolTipModality = Web.Components.IUIControl.ToolTipModalities.Ondemand
                scClaveDocumento.ToolTipStatus = Web.Components.IUIControl.ToolTipTypeStatus.OkInfo

                scPatente.DataSource = New List(Of SelectOption) From {New SelectOption With {.Value = 1, .Text = "Marítimo | Veracruz 430 | Jesús Gómez Reyes 3945"}}
                scPatente.Value = 1
                scPatente.ToolTip = "Sugerido por sistema, confirmar"
                scPatente.ToolTipModality = Web.Components.IUIControl.ToolTipModalities.Ondemand
                scPatente.ToolTipStatus = Web.Components.IUIControl.ToolTipTypeStatus.OkInfo

                scTipoDocumento.DataSource = New List(Of SelectOption) From {New SelectOption With {.Value = 1, .Text = "Normal"}}
                scTipoDocumento.Value = 1
                scTipoDocumento.ToolTip = "Sugerido por sistema, confirmar"
                scTipoDocumento.ToolTipModality = Web.Components.IUIControl.ToolTipModalities.Ondemand
                scTipoDocumento.ToolTipStatus = Web.Components.IUIControl.ToolTipTypeStatus.OkInfo

                scEjecutivoCuenta.DataSource = New List(Of SelectOption) From {New SelectOption With {.Value = 4711, .Text = "SAHAIRA ELIZABETH VILLANUEVA CONTRERAS"}}
                scEjecutivoCuenta.Value = 4711
                scEjecutivoCuenta.ToolTip = "Sugerido por sistema, confirmar"
                scEjecutivoCuenta.ToolTipModality = Web.Components.IUIControl.ToolTipModalities.Ondemand
                scEjecutivoCuenta.ToolTipStatus = Web.Components.IUIControl.ToolTipTypeStatus.OkInfo

                DisplayMessage("Documento agregado y sugerencias realizadas")

            Case 2
                facturaComercial = _controladorReferencias.CrearPrereferencia(memoryStream_, Ia.Pln.IControllerChatGPT.DocumentoCargado.FacturaImportacion).ObjectReturned
        End Select



        scRegimen.ShowToolTip()
        scClaveDocumento.ShowToolTip()
        scPatente.ShowToolTip()
        scTipoDocumento.ShowToolTip()
        scTipoGuia.ShowToolTip()
        scEjecutivoCuenta.ShowToolTip()

    End Sub

    Protected Sub btGuardarDocumentos_OnClick(sender As Object, e As EventArgs)

        Dim listaDocumentos_ As List(Of Newtonsoft.Json.Linq.JObject) = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of Newtonsoft.Json.Linq.JObject))(fcDocumentos.Value)

        For Each documento_ In listaDocumentos_

            ccDocumentos.SetRow(Sub(catalogRow_ As CatalogRow)

                                    catalogRow_.SetIndice(ccDocumentos.KeyField, 0)

                                    catalogRow_.SetColumn(icArchivo, New SelectOption With {.Value = documento_.SelectToken("fileId").ToString, .Text = documento_.SelectToken("fileName").ToString})

                                    catalogRow_.SetColumn(icTipoArchivo, New SelectOption With {.Value = scTipoDocumentosFijo.Value, .Text = scTipoDocumentosFijo.Text})

                                End Sub)

            ccDocumentos.DataSource = ccDocumentos.DataSource

            ccDocumentos.CatalogDataBindingUpdate()


        Next


        fcDocumentos.Value = Nothing

        scTipoDocumentosFijo.Value = ""

    End Sub

    Protected Sub scPrefijo_SelectedIndexChanged(sender As Object, e As EventArgs)

        dbcReferencia.Value = GeneraReferenciaPedimento(True, False, TipoReferenciaPedimento.Prefijo)

    End Sub

    Protected Sub scTipoDocumento_SelectedIndexChanged(sender As Object, e As EventArgs)

        If scTipoDocumento.Value IsNot Nothing And scTipoDocumento.Value = 4 Then

            swcRectificacion.Checked = True

        Else
            swcRectificacion.Checked = False

        End If

    End Sub

    Protected Sub swcTipoOperacion_CheckedChanged(sender As Object, e As EventArgs)

        If swcTipoOperacion.Checked Then

            Guia.Visible = True

            'TrackingImpo.Visible = True

            'TrackingExpo.Visible = False

            icFechaEtd.Visible = False

            icFechaPresentacion.Visible = False

            icFechaSalida.Visible = False

            icFechaCierreFisico.Visible = False

            icFechaEta.Visible = True

            icFechaRevalidacion.Visible = True

        Else

            Guia.Visible = False

            'TrackingExpo.Visible = True

            'TrackingImpo.Visible = False

            icFechaEtd.Visible = True

            icFechaPresentacion.Visible = True

            icFechaSalida.Visible = True

            icFechaCierreFisico.Visible = True

            icFechaEta.Visible = False

            icFechaRevalidacion.Visible = False

        End If

    End Sub

    Protected Sub swcGuiaMultiple_CheckedChanged(sender As Object, e As EventArgs)

        If scGuias.Checked = True Then

            ccGuias.Visible = True

            pnGuia.Visible = False

        Else

            ccGuias.Visible = False

            pnGuia.Visible = True

        End If

    End Sub

    Protected Sub icRutaDocumento_ChooseFile(sender As PropiedadesDocumento, e As EventArgs)

        Dim id = ObjectId.GenerateNewId().ToString

        With sender
            ._idpropietario = id
            .nombrepropietario = "Yo Merengues"
            .tipovinculacion = PropiedadesDocumento.TiposVinculacion.AgenciaAduanal
            .datosadicionales = New InformacionDocumento With {
                          .foliodocumento = "00000002",
                          .tipodocumento = InformacionDocumento.TiposDocumento.BL,
                          .datospropietario = New InformacionPropietario With {
                              .nombrepropietario = "Yo Merengues",
                              ._id = id
                          }
                         }
            .formatoarchivo = PropiedadesDocumento.FormatosArchivo.pdf
        End With

        _idDocumento = ObjectId.Parse(id)

    End Sub

    Protected Sub icRutaDocumentos_ChooseFile(sender As PropiedadesDocumento, e As EventArgs)

        Dim id = ObjectId.GenerateNewId().ToString

        With sender
            ._idpropietario = id
            .nombrepropietario = "Yo Merengues"
            .tipovinculacion = PropiedadesDocumento.TiposVinculacion.AgenciaAduanal
            .datosadicionales = New InformacionDocumento With {
                          .foliodocumento = "00000002",
                          .tipodocumento = InformacionDocumento.TiposDocumento.BL,
                          .datospropietario = New InformacionPropietario With {
                              .nombrepropietario = "Yo Merengues",
                              ._id = id
                          }
                         }
            .formatoarchivo = PropiedadesDocumento.FormatosArchivo.pdf
        End With

        _idDocumento = ObjectId.Parse(id)

    End Sub

    Protected Sub fbcCliente_TextChanged(sender As Object, e As EventArgs)

        Using controlador_ = New ControladorBusqueda(Of ConstructorCliente)

            Dim lista_ As List(Of SelectOption) = controlador_.Buscar(fbcCliente.Text, New Filtro With {.IdSeccion = SeccionesClientes.SCS1, .IdCampo = CamposClientes.CA_RAZON_SOCIAL})

            fbcCliente.DataSource = lista_

        End Using

        ' Dim controlador_ As New ControladorBusqueda(Of ConstructorCliente)

    End Sub

    Protected Sub fbcCliente_Click(sender As Object, e As EventArgs)

        'Dim controlador_ As New ControladorBusqueda(Of ConstructorCliente)

        Using controlador_ = New ControladorBusqueda(Of ConstructorCliente)

            Dim tagwatcher_ = controlador_.ObtenerDocumento(fbcCliente.Value)

            If tagwatcher_.ObjectReturned IsNot Nothing Then

                Dim documentoCliente_ = tagwatcher_.ObjectReturned

                InicializaCliente(documentoCliente_)

            End If

        End Using

    End Sub

    Protected Sub dbcReferencia_Click(sender As Object, e As EventArgs)

        If dbcReferencia.ValueDetail = "" Then 'And GetVars("IsEditing") Then

            dbcReferencia.ValueDetail = Mid(Year(Now).ToString, 4, 1) &
                GeneraSecuencia("Pedimentos", Statements.GetOfficeOnline._id, Year(Now), 0, 3210, 430).ToString.PadLeft(6, "0").ToString


        End If

    End Sub

    Protected Sub swcRectificacion_CheckedChanged(sender As Object, e As EventArgs)

        If swcRectificacion.Checked = True Then

            scTipoDocumento_Click(sender, e)

            scTipoDocumento.Value = 4

            scTipoDocumento_SelectedIndexChanged(sender, e)

            scTipoDocumento.Enabled = False

        Else

            scTipoDocumento.Enabled = True

            scTipoDocumento_Click(sender, e)

            scTipoDocumento.Value = 1

            scTipoDocumento_SelectedIndexChanged(sender, e)

        End If


    End Sub

    Protected Sub scTipoDocumento_Click(sender As Object, e As EventArgs)

        scTipoDocumento.DataSource = TipoDocumento()

    End Sub

    Protected Sub scTipoCarga_Click(sender As Object, e As EventArgs)

        scTipoCarga.DataSource = TipoCarga()

    End Sub

    Protected Sub AntesDeCambiarEmpresa(ByVal sender As FindbarControl, ByVal e As EventArgs)

        'MsgBox(OperacionGenerica.Id.ToString)

        BusquedaGeneral(sender, e)

    End Sub


#End Region

#Region "██████ Vinculación sexta capa  █████████       SAX      ████████████████████████████████████████████"
    '    ██████   Controladores utilizados                     Documentos por coding para MongoDB      ██████
    '    ██████    1.ControladorClientes                        1. En Empresa:                         ██████
    '    ██████    2.ControladorRecursosAduanales                  a). Domicilios                      ██████
    '    ██████    3.ControladorSecuencias                         b). Contactos                       ██████
    '    ██████                                                                                        ██████
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████


    Protected Function GeneraReferenciaPedimento(ByVal generaReferencia_ As Boolean, ByVal generaPedimento_ As Boolean, ByVal parte_ As Int16) As String

        If generaReferencia_ Then

            Select Case parte_
                Case 1 'Prefijo

                    Return scPrefijo.Text & Mid(Year(Now).ToString, 3, 2) & "-"

                Case 2 'Sufijo


                Case 3 'Completo

                    Return scPrefijo.Text &
            Mid(Year(Now).ToString, 3, 2) & "-" &
            GeneraSecuencia("Referencias", Statements.GetOfficeOnline._id, Year(Now), 0, 0, 0, scPrefijo.Value).ToString.PadLeft(8, "0")

            End Select

        End If

        If generaPedimento_ Then

            Select Case parte_
                Case 1 'Prefijo

                Case 2 'Sufijo

                Case 3 'Completo

            End Select

        End If

        Return Nothing

    End Function

    Protected Sub InicializaPrefijo()

        Dim tipoPrefijo_ As Int16

        tipoPrefijo_ = ControladorRecursosAduanales.TiposPrefijosEnviroment.ReferenciaOperativaNormal

        If tipoPrefijo_ = ControladorRecursosAduanales.TiposReferenciasOperativas.SinDefinir Then

            scPrefijo.DataSource = Nothing

        Else

            Dim prefijodefault_ As Int16 = 0

            scPrefijo.DataSource = PrefijosReferencia(tipoPrefijo_, prefijodefault_)

            If scPrefijo.DataSource IsNot Nothing And prefijodefault_ <> 0 Then

                scPrefijo.Value = prefijodefault_

            End If

        End If

    End Sub

    Private Function PrefijosReferencia(ByVal tipoPrefijo_ As TiposPrefijosEnviroment, Optional ByRef idprefijoDefault_ As Int16 = 0) As List(Of SelectOption)

        Dim recursos_ As ControladorRecursosAduanales = BuscarRecursosAduanales(ControladorRecursosAduanales.TiposRecurso.Generales)

        Dim prefijos_ = From enviroment In recursos_.prefijosenviroment
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

    Private Function AduanasSeccion() As List(Of SelectOption)

        Dim recursos_ As ControladorRecursosAduanales = BuscarRecursosAduanales(ControladorRecursosAduanales.TiposRecurso.Generales)

        Dim aduanasSecciones_ = From data In recursos_.aduanasmodalidad
                                Where data.archivado = False And data.estado = 1
                                Select data.modalidad, data.ciudad, data._idaduanaseccion

        If aduanasSecciones_.Count > 0 Then

            Dim aduanaSeccionModalidad_ As New List(Of SelectOption)

            For index_ As Int32 = 0 To aduanasSecciones_.Count - 1

                aduanaSeccionModalidad_.Add(New SelectOption With
                             {.Value = aduanasSecciones_(index_)._idaduanaseccion,
                              .Text = aduanasSecciones_(index_).modalidad.ToString & "|" & aduanasSecciones_(index_).ciudad & "|" & aduanasSecciones_(index_)._idaduanaseccion.ToString})


            Next

            Return aduanaSeccionModalidad_

        End If

        Return Nothing

    End Function

    Private Function TipoDocumento() As List(Of SelectOption)

        Dim recursos_ As ControladorRecursosAduanalesGral = ControladorRecursosAduanalesGral.Buscar(ControladorRecursosAduanalesGral.TiposRecurso.Generales)

        Dim tipoDocumento_ = From data In recursos_.tiposdocumento
                             Where data.archivado = False And data.estado = 1
                             Select data._idtipodocumento, data.descripcion, data.descripcioncorta

        If tipoDocumento_.Count > 0 Then

            Dim dataSource1_ As New List(Of SelectOption)

            For index_ As Int32 = 0 To tipoDocumento_.Count - 1

                dataSource1_.Add(New SelectOption With
                             {.Value = tipoDocumento_(index_)._idtipodocumento,
                              .Text = tipoDocumento_(index_).descripcioncorta.ToString})

            Next

            Return dataSource1_

        End If

        Return Nothing

    End Function

    Public Function TipoCarga() As List(Of SelectOption)

        Dim recursos_ As ControladorRecursosAduanalesGral = ControladorRecursosAduanalesGral.Buscar(ControladorRecursosAduanalesGral.TiposRecurso.Anexo22)

        Dim tipoCarga_ = From data In recursos_.tiposcargalote
                         Where data.archivado = False And data.estado = 1
                         Select data._idtipocargalote, data.descripcion

        If tipoCarga_.Count > 0 Then

            Dim dataSource1 As New List(Of SelectOption)

            For index_ As Int32 = 0 To tipoCarga_.Count - 1

                dataSource1.Add(New SelectOption With
                                {.Value = tipoCarga_(index_)._idtipocargalote,
                                 .Text = tipoCarga_(index_).descripcion.ToString})
            Next

            Return dataSource1

        End If

        Return Nothing

    End Function
    Protected Sub SeleccionarUnidadMedida_Click(sender As Object, e As EventArgs)

        Dim tipoUnidad_ As ControladorUnidadesMedida.TiposUnidad = TiposUnidad.Comercial

        Dim top_ As Int32 = 0

        Dim lista_ As List(Of UnidadMedida) = ControladorUnidadesMedida.BuscarUnidades(tipoUnidad_, scUnidadMedida.SuggestedText, top_)

        If lista_.Count > 0 Then

            scUnidadMedida.DataSource = ControladorUnidadesMedida.ToSelectOption(lista_, ControladorUnidadesMedida.TipoSelectOption.CveMXnombreoficiales)

        End If

    End Sub
    Protected Sub SeleccionarUnidadMedidaMulti_Click(sender As Object, e As EventArgs)

        Dim tipoUnidad_ As ControladorUnidadesMedida.TiposUnidad = TiposUnidad.Comercial

        Dim top_ As Int32 = 0

        Dim lista_ As List(Of UnidadMedida) = ControladorUnidadesMedida.BuscarUnidades(tipoUnidad_, scUnidadMedida.SuggestedText, top_)

        If lista_.Count > 0 Then

            scUnidadMedidaMulti.DataSource = ControladorUnidadesMedida.ToSelectOption(lista_, ControladorUnidadesMedida.TipoSelectOption.CveMXnombreoficiales)

        End If

    End Sub
    Protected Sub SeleccionarPais_Click(sender As Object, e As EventArgs)



    End Sub

    Protected Sub scModalidadAduanaPatente_Click(sender As Object, e As EventArgs)

        scPatente.DataSource = ModalidadSeccionPatente()

    End Sub

    Protected Sub InicializaCliente(ByVal datosCliente_ As OperacionGenerica)

        icRFC.Value = datosCliente_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Campo(CamposClientes.CA_RFC_CLIENTE).Valor

        'icBancoPago.Value = datosCliente_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Campo(CamposClientes.CP_CVE_BANCO_PAGO).Valor

        icRFCFacturacion.Value = datosCliente_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Campo(CamposClientes.CA_RFC_CLIENTE).Valor

    End Sub

    Private Function ModalidadSeccionPatente() As List(Of SelectOption)

        Dim _modalidadSeccionPatente = ControladorRecursosAduanales.BuscarRecursosAduanales(ControladorRecursosAduanales.TiposRecurso.Generales)

        SetVars("modalidadSeccionPatente", _modalidadSeccionPatente)

        Dim aduanasSeccionesPatente_ = From data In _modalidadSeccionPatente.aduanaspatentes
                                       Where data.archivado = False And data.estado = 1
                                       Select data._idmodalidadaduanapatente, data.modalidad, data.ciudad, data._idaduanaseccion, data.agenteaduanal, data._idpatente

        If aduanasSeccionesPatente_.Count > 0 Then

            Dim dataSource_ As New List(Of SelectOption)

            For index_ As Int32 = 0 To aduanasSeccionesPatente_.Count - 1

                dataSource_.Add(New SelectOption With
                             {.Value = aduanasSeccionesPatente_(index_)._idmodalidadaduanapatente,
                              .Text = aduanasSeccionesPatente_(index_).modalidad & "|" & aduanasSeccionesPatente_(index_).ciudad & "-" & aduanasSeccionesPatente_(index_)._idaduanaseccion &
                              "|" & aduanasSeccionesPatente_(index_).agenteaduanal & "-" & aduanasSeccionesPatente_(index_)._idpatente})

            Next

            Return dataSource_

        End If

        Return Nothing

    End Function

#End Region

End Class

<Serializable()>
Public Class FsChuck
    Public Property _id As ObjectId
    <BsonIgnoreIfNull>
    Public Property files_id As ObjectId
    <BsonIgnoreIfNull>
    Public Property n As Int32
    <BsonIgnoreIfNull>
    Public Property data As data

End Class
<Serializable()>
Public Class data
    Public Property binary As Binary

End Class
<Serializable()>
Public Class Binary
    Public Property base64 As String
    Public Property subType As String

End Class