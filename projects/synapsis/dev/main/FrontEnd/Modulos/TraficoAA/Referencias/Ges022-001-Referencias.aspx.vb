
#Region "├┴┘├┴┘├┴┘├┴┘├┴┘|├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘   DEPENDENCIAS   ├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘"

'RECURSOS DEL CMF
Imports System.IO
Imports gsol
Imports gsol.krom
Imports gsol.Web.Components
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Driver
'OBJETOS DIMENSIONALES (ODS's) Dependencias en MongoDB
Imports Rec.Globals.Controllers
Imports Rec.Globals.Utils
'UTILERIAS/RECURSOS ADICIONALES
Imports Sax.Web
Imports Syn.CustomBrokers.Controllers
Imports Syn.CustomBrokers.Controllers.ControladorRecursosAduanales
Imports Syn.CustomBrokers.Controllers.ControladorUnidadesMedida
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Nucleo
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Nucleo.RecursosComercioExterior.CamposReferencia
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher
Imports Wma.Exceptions.TagWatcher.TypeStatus

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

    Private _tipoPedimento As String

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

        scRegimen.DataEntity = New Anexo22()

        scClaveDocumento.DataEntity = New Anexo22()

        scPais.DataEntity = New Anexo22()

        scPaisMulti.DataEntity = New Anexo22()

        scEjecutivoCuenta.DataEntity = New Ejecutivos

        scEjecutivoCuenta.FreeClauses = " and i_Cve_DivisionMiEmpresa = " & Statements.GetOfficeOnline()._id

        If scGuias.Checked = False Then

            ccGuias.Visible = False

            pnGuia.Visible = True

        Else

            ccGuias.Visible = True

            pnGuia.Visible = False

        End If

        icRFC.Enabled = False

    End Sub

    Public Overrides Sub BotoneraClicNuevo()

        If OperacionGenerica IsNot Nothing Then

            '_empresa = Nothing

        End If

        swcMaterialPeligroso.Checked = False

        swcRectificacion.Checked = False

        swcTipoOperacion.Checked = True
        swcTipoOperacion.Enabled = True

        ccDocumento.Visible = True

        swcRectificacion.Visible = False

        fscFechas.Visible = True

        fscGuia.Visible = True

        ccGuias.Visible = False

        ccDespacho.Visible = False

        scTipoDocumentos.DataSource = New List(Of SelectOption) From {New SelectOption With {.Value = 1, .Text = "BL"}}
        scTipoDocumentos.Value = 1

        fscTrackingExpo.Visible = False
        fscTrackingImpo.Visible = False

        fscFechas.Visible = False

        fscDocumentos.Visible = True

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

        fscFechas.Visible = True

        fscGuia.Visible = True

        swcRectificacion.Visible = True

        If scTipoDespacho.Value = 2 Then

            ccDespacho.Visible = True

        End If

        If swcTipoOperacion.Checked Then

            fscGuia.Visible = True

            icFechaEtd.Visible = False

            icFechaPresentacion.Visible = False

            icFechaSalida.Visible = False

            icFechaCierreFisico.Visible = False

            icFechaEta.Visible = True

            icFechaRevalidacion.Visible = True

        Else

            fscGuia.Visible = False

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

        End If

    End Sub

    Public Overrides Function AgregarComponentesBloqueadosInicial() As List(Of WebControl)

        Dim bloqueados_ As New List(Of WebControl)

        bloqueados_.Add(icRFC)
        bloqueados_.Add(icRFCFacturacion)
        bloqueados_.Add(icArchivo)
        bloqueados_.Add(icTipoArchivo)
        bloqueados_.Add(swcRectificacion)

        Return bloqueados_

    End Function

    Public Overrides Function AgregarComponentesBloqueadosEdicion() As List(Of WebControl)

        Dim bloqueadosEdicion_ As New List(Of WebControl)

        'Aquí debemos validar que se tenga publicada y ya bloquearle los campos que se definan. ¿Cómo se vera un documento electronico publiscado?

        'bloqueadosEdicion_.Add(swcTipoOperacion)
        'bloqueadosEdicion_.Add(swcRectificacion)
        'bloqueadosEdicion_.Add(scTipoDocumento)
        'bloqueadosEdicion_.Add(scEjecutivoCuenta)
        'bloqueadosEdicion_.Add(scPatente)
        'bloqueadosEdicion_.Add(icPedimentoOriginal)
        'bloqueadosEdicion_.Add(scTipoCarga)
        'bloqueadosEdicion_.Add(scTipoDespacho)
        'bloqueadosEdicion_.Add(scClaveDocumento)
        'bloqueadosEdicion_.Add(scRegimen)
        'bloqueadosEdicion_.Add(dbcReferencia)
        'bloqueadosEdicion_.Add(fbcCliente)
        'bloqueadosEdicion_.Add(icBancoPago)
        'bloqueadosEdicion_.Add(swcMaterialPeligroso)

        Return bloqueadosEdicion_

    End Function

    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher

        Dim tipoOp_ As Int32 = IIf(swcTipoOperacion.Checked, ControladorRecursosAduanales.TiposOperacionAduanal.Importacion, ControladorRecursosAduanales.TiposOperacionAduanal.Exportacion)

        [Set](dbcReferencia, CP_REFERENCIA, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](dbcReferencia, CamposPedimento.CA_NUMERO_PEDIMENTO_COMPLETO, propiedadDelControl_:=PropiedadesControl.ValueDetail)
        [Set](swcTipoOperacion, CamposPedimento.CA_TIPO_OPERACION, asignarA_:=TiposAsignacion.ValorPresentacion, propiedadDelControl_:=PropiedadesControl.Checked)
        [Set](tipoOp_, CamposPedimento.CA_TIPO_OPERACION, tipoDato_:=Campo.TiposDato.Entero)
        [Set](swcMaterialPeligroso, CP_MATERIAL_PELIGROSO, propiedadDelControl_:=PropiedadesControl.Checked)
        [Set](swcRectificacion, CP_RECTIFICACION, propiedadDelControl_:=PropiedadesControl.Checked)
        [Set](scPatente, CamposPedimento.CP_MODALIDAD_ADUANA_PATENTE)
        [Set](scRegimen, CamposPedimento.CA_REGIMEN)
        [Set](scTipoDocumento, CA_TIPO_PEDIMENTO)
        [Set](scClaveDocumento, CamposPedimento.CA_CVE_PEDIMENTO)
        [Set](scEjecutivoCuenta, CamposPedimento.CP_EJECUTIVO_CUENTA)
        [Set](scTipoCarga, CP_TIPO_CARGA_AGENCIA)
        [Set](icDescripcionCompleta, CP_DESCRIPCION_MERCANCIA_COMPLETA)
        [Set](scTipoDespacho, CP_TIPO_DESPACHO, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](icPedimentoOriginal, CP_PEDIMENTO_ORIGINAL)

        [Set](fbcCliente, CamposClientes.CP_OBJECTID_CLIENTE)
        [Set](fbcCliente, CamposClientes.CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Text)
        [Set](icRFC, CamposClientes.CA_RFC_CLIENTE)
        [Set](icRFCFacturacion, CamposClientes.CP_RFC_FACTURACION)
        [Set](icBancoPago, CamposClientes.CP_CVE_BANCO)

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

        MostrarTooltipTipoPedimento(1)

    End Sub

    'EVENTOS PARA LA INSERCIÓN DE DATOS
    Public Overrides Function AntesRealizarInsercion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim modalidadSeccionPatente_ As ControladorRecursosAduanales = GetVars("_modalidadSeccionPatente")

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

            .FolioOperacion = dbcReferencia.Value

            .IdCliente = 0

            .NombreCliente = fbcCliente.Text

        End With

    End Sub

    Public Overrides Function DespuesRealizarInsercion() As TagWatcher

        Return New TagWatcher(1)

    End Function

    'EVENTOS PARA MODIFICACIÓN DE DATOS
    Public Overrides Function AntesRealizarModificacion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim modalidadSeccionPatente_ As ControladorRecursosAduanales = GetVars("_modalidadSeccionPatente")

        If modalidadSeccionPatente_ IsNot Nothing Then

            Dim seccionesPatente_ = From data In modalidadSeccionPatente_.aduanaspatentes
                                    Where data._idmodalidadaduanapatente.ToString.Equals(scPatente.Value)
                                    Select data._idaduanaseccion, data._idpatente

            If seccionesPatente_.Count > 0 Then

                Dim seccion_ As String = seccionesPatente_(0)._idaduanaseccion.ToString

                Dim patente_ = seccionesPatente_(0)._idpatente.ToString

                [Set](seccion_, CamposPedimento.CA_ADUANA_ENTRADA_SALIDA)

                [Set](patente_, CamposPedimento.CA_PATENTE,)

            End If

        End If

        Return New TagWatcher(1)

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

        Dim swTipoOp_ = OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Attribute(CamposPedimento.CA_TIPO_OPERACION).Valor

        Dim tipoPedimento = OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Attribute(CA_TIPO_PEDIMENTO).Valor

        If swTipoOp_ = 1 Then

            swcTipoOperacion.Checked = True

        Else

            swcTipoOperacion.Checked = False

        End If

        If tipoPedimento = 4 Then

            icPedimentoOriginal.Visible = True

            scRegimen.Visible = True

        ElseIf tipoPedimento = 2 Then

            scRegimen.Visible = False

            icPedimentoOriginal.Visible = False

        Else

            icPedimentoOriginal.Visible = False

            scRegimen.Visible = True

        End If

        Dim fuente_ = OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

        If swMultiple Then

            ccGuias.Visible = True

            pnGuia.Visible = False

        Else

            ccGuias.Visible = False

            pnGuia.Visible = True

        End If

        If fuente_.Attribute(CamposPedimento.CA_TIPO_OPERACION).Valor = True Then

            fscGuia.Visible = True

            icFechaEtd.Visible = False

            icFechaPresentacion.Visible = False

            icFechaSalida.Visible = False

            icFechaCierreFisico.Visible = False

            icFechaEta.Visible = True

            icFechaRevalidacion.Visible = True

        Else

            fscGuia.Visible = False

            icFechaEtd.Visible = True

            icFechaPresentacion.Visible = True

            icFechaSalida.Visible = True

            icFechaCierreFisico.Visible = True

            icFechaEta.Visible = False

            icFechaRevalidacion.Visible = False

        End If

        fscFechas.Visible = True

        swcRectificacion.Visible = True

        fscDocumentos.Visible = True

        scGuias.Enabled = False

    End Sub

    Public Overrides Sub DespuesBuquedaGeneralSinDatos()

    End Sub

    'EVENTOS DE MANTENIMIENTO
    Public Overrides Sub LimpiaSesion()

        SetVars("_modalidadSeccionPatente", Nothing)

    End Sub

    Public Overrides Sub Limpiar()

        'Ambiguedad en nombres de catalogos y card CC
        ccGuias.DataSource = Nothing
        ccDocumentos.DataSource = Nothing

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

    Protected Sub btIr_OnClick(sender As Object, e As EventArgs)

        'Falta algo?

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

    Protected Sub swcTipoOperacion_CheckedChanged(sender As Object, e As EventArgs)

        If swcTipoOperacion.Checked Then

            fscGuia.Visible = True
            icFechaEtd.Visible = False
            icFechaPresentacion.Visible = False
            icFechaSalida.Visible = False
            icFechaCierreFisico.Visible = False
            icFechaEta.Visible = True
            icFechaRevalidacion.Visible = True

        Else

            fscGuia.Visible = False
            icFechaEtd.Visible = True
            icFechaPresentacion.Visible = True
            icFechaSalida.Visible = True
            icFechaCierreFisico.Visible = True
            icFechaEta.Visible = False
            icFechaRevalidacion.Visible = False

        End If

        'Se limpian los selectores de las asistencias
        scTipoDocumento.DataSource = Nothing
        scTipoDocumento.Value = Nothing
        LimpiaAsistencias()
        MostrarTooltipTipoPedimento(1)

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

    End Sub

    Protected Sub fbcCliente_Click(sender As Object, e As EventArgs)

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

            scTipoDocumento.Value = 4
            scTipoDocumento_SelectedIndexChanged(sender, e)
            scTipoDocumento.Enabled = False

        Else

            scTipoDocumento.Enabled = True
            scTipoDocumento.Value = 1
            scTipoDocumento_SelectedIndexChanged(sender, e)

        End If

    End Sub

    Protected Sub scTipoDocumento_Click(sender As Object, e As EventArgs)

        scTipoDocumento.DataSource = TipoDocumento()

    End Sub

    Protected Sub scTipoDocumento_SelectedIndexChanged(sender As Object, e As EventArgs)

        If scTipoDocumento.Value IsNot Nothing And scTipoDocumento.Value = 4 Then

            'Tipo: RECTIFICACIÓN
            icPedimentoOriginal.Visible = True
            scRegimen.Visible = True
            scClaveDocumento.DataSource = New List(Of SelectOption) From {New SelectOption With {.Value = "R1", .Text = "R1 - RECTIFICACION DE PEDIMENTOS."}}
            scClaveDocumento.Value = "R1"

        ElseIf scTipoDocumento.Value = 2 Then

            'Tipo: COMPLEMENTARIO
            scRegimen.Visible = False
            scRegimen.DataSource = Nothing
            scRegimen.Value = Nothing
            scClaveDocumento.DataSource = New List(Of SelectOption) From {New SelectOption With {.Value = "CT", .Text = "CT - PEDIMENTO COMPLEMENTARIO."}}
            scClaveDocumento.Value = "CT"
            icPedimentoOriginal.Visible = False

        ElseIf scTipoDocumento.Value = 5 Then

            'Tipo: GLOBAL COMPLEMENTARIO
            scClaveDocumento.DataSource = New List(Of SelectOption) From {New SelectOption With {.Value = "GC", .Text = "GC - GLOBAL COMPLEMENTARIO."}}
            scClaveDocumento.Value = "GC"
            scClaveDocumento.FreeClauses = " and t_Cve_Pedimento = GC"
            icPedimentoOriginal.Visible = False
            scRegimen.Visible = True

        Else

            icPedimentoOriginal.Visible = False
            scRegimen.Visible = True

        End If

        MostrarTooltipClavePedimento()

        scTipoDespacho.ToolTip = "Asistencia aplicada"
        scTipoDespacho.ToolTipExpireTime = 4
        scTipoDespacho.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo
        scTipoDespacho.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        scTipoDespacho.ShowToolTip()

    End Sub

    Protected Sub scTipoCarga_Click(sender As Object, e As EventArgs)

        scTipoCarga.DataSource = TipoCarga()

    End Sub

    Protected Sub AntesDeCambiarEmpresa(ByVal sender As FindbarControl, ByVal e As EventArgs)

        BusquedaGeneral(sender, e)

    End Sub

    Protected Sub scClaveDocumento_Click(sender As Object, e As EventArgs)

        'Obtenemos el tipo de pedimento en una variable para aplicar los casos necesarios
        _tipoPedimento = scTipoDocumento.Text

        'Limpiamos los selectores de asistencias relacionados por si estan llenos o tienen algún dato
        scClaveDocumento.DataSource = Nothing
        scClaveDocumento.Value = Nothing
        scRegimen.DataSource = Nothing
        scRegimen.Value = Nothing

        If _tipoPedimento <> "" Then

            'Validamos que el tipo de pedimento sea aplicado a asistencia
            Select Case _tipoPedimento

                Case "RECTIFICACIÓN"

                    scClaveDocumento.FreeClauses = " and t_Cve_Pedimento = R1"

                Case "COMPLEMENTARIO"

                    scClaveDocumento.FreeClauses = " and t_Cve_Pedimento = CT"

                Case "GLOBAL COMPLEMENTARIO"

                    scClaveDocumento.FreeClauses = " and t_Cve_Pedimento = GC"

                Case Else

                    'Abrimos el using para trabajar con el prevalidador de asistencias
                    Using prevalidadorAsistencia_ As IPrevalidadorAsistencia = New PrevalidadorAsistencia()

                        'Creación del diccionario con los parámetros
                        Dim parametrosAsistencia_ As New Dictionary(Of String, Object)

                        'Llenado por condición del primer parámetro
                        If swcTipoOperacion.Checked = True Then

                            parametrosAsistencia_.Add(CamposPedimento.CA_TIPO_OPERACION.ToString, Convert.ToString(TiposOperacionAduanal.Importacion))

                        Else

                            parametrosAsistencia_.Add(CamposPedimento.CA_TIPO_OPERACION.ToString, Convert.ToString(TiposOperacionAduanal.Exportacion))

                        End If

                        'Llenado del segundo parametro
                        parametrosAsistencia_.Add(CamposPedimento.CP_TIPO_PEDIMENTO.ToString, _tipoPedimento)

                        'Consulta de asistencia
                        prevalidadorAsistencia_.EstatusAsistencia = prevalidadorAsistencia_.ConsultarAsistencia(IPrevalidador.TiposProcesamiento.AsistirCaptura,
                                                                                                                IPrevalidador.TiposValidacion.Legal,
                                                                                                                IPrevalidadorAsistencia.TiposAsistenciaConsultar.AS_PED2,
                                                                                                                parametrosAsistencia_)

                        If prevalidadorAsistencia_.EstatusAsistencia.Status = TypeStatus.Ok Then

                            'Se extrae el tipo asistencia en una clase para luego extraer por LinQ la lista de claves
                            Dim asistencia_ As Asistencia = prevalidadorAsistencia_.EstatusAsistencia.ObjectReturned
                            Dim listaClaves_ = From camposAfectados_ In asistencia_.CamposAfectados("CA_CVE_PEDIMENTO")
                                               Select camposAfectados_

                            'Se hace el IN O EL = en la clausula libre Del componente que le corresponde
                            If listaClaves_.Count = 1 Then

                                If listaClaves_(0).ToString <> IPrevalidadorAsistencia.ErroresAsistencia.EAS_001.ToString Then

                                    scClaveDocumento.FreeClauses = " and t_Cve_Pedimento = '" + listaClaves_(0).ToString + "'"

                                Else

                                    DisplayMessage(Recursos.GetEnumDescription(IPrevalidadorAsistencia.ErroresAsistencia.EAS_001), StatusMessage.Info)
                                    'Condición para que no muestre información
                                    scClaveDocumento.FreeClauses = " and i_Cve_Estado = 5"

                                End If

                            Else

                                scClaveDocumento.FreeClauses = " and t_Cve_Pedimento in (" + String.Join(",", listaClaves_) + ")"

                            End If

                        ElseIf prevalidadorAsistencia_.EstatusAsistencia.Status = TypeStatus.OkInfo Then

                            DisplayMessage(prevalidadorAsistencia_.EstatusAsistencia.ErrorDescription, StatusMessage.Info)
                            'Condición para que no muestre información
                            scClaveDocumento.FreeClauses = " and i_Cve_Estado = 5"

                        ElseIf prevalidadorAsistencia_.EstatusAsistencia.Status = TypeStatus.OkBut Then

                            DisplayMessage(prevalidadorAsistencia_.EstatusAsistencia.ErrorDescription, StatusMessage.Info)
                            'Condición para que no muestre información
                            scClaveDocumento.FreeClauses = " and i_Cve_Estado = 5"

                        Else

                            DisplayMessage(prevalidadorAsistencia_.EstatusAsistencia.ErrorDescription, StatusMessage.Fail)
                            'Condición para que no muestre información
                            scClaveDocumento.FreeClauses = " and i_Cve_Estado = 5"

                        End If

                    End Using

            End Select

        Else

            MostrarTooltipTipoPedimento(2)
            'Condición para que no muestre información
            scClaveDocumento.FreeClauses = " and i_Cve_Estado = 5"

        End If


    End Sub

    Protected Sub scClaveDocumento_SelectedIndexChanged(sender As Object, e As EventArgs)

        scRegimen.ToolTip = "Asistencia aplicada"
        scRegimen.ToolTipExpireTime = 4
        scRegimen.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo
        scRegimen.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        scRegimen.ShowToolTip()

    End Sub

    Protected Sub scRegimen_Click(sender As Object, e As EventArgs)

        'Obtenemos el valor de la clave pedimento
        Dim clavePedimento_ As String = scClaveDocumento.Value

        If clavePedimento_ <> "" Then

            'Abrimos el using para trabajar con el prevalidador de asistencias
            Using prevalidadorAsistencia_ As IPrevalidadorAsistencia = New PrevalidadorAsistencia()

                'Creación del diccionario con los parámetros
                Dim parametrosAsistencia_ As New Dictionary(Of String, Object)

                'Llenado por condición del primer parámetro
                If swcTipoOperacion.Checked = True Then

                    parametrosAsistencia_.Add(CamposPedimento.CA_TIPO_OPERACION.ToString, Convert.ToString(TiposOperacionAduanal.Importacion))

                Else

                    parametrosAsistencia_.Add(CamposPedimento.CA_TIPO_OPERACION.ToString, Convert.ToString(TiposOperacionAduanal.Exportacion))

                End If

                'Segundo parámetro
                parametrosAsistencia_.Add(CamposPedimento.CA_CVE_PEDIMENTO.ToString, clavePedimento_)

                'Consulta de asistencia
                prevalidadorAsistencia_.EstatusAsistencia = prevalidadorAsistencia_.ConsultarAsistencia(IPrevalidador.TiposProcesamiento.AsistirCaptura,
                                                                                                                IPrevalidador.TiposValidacion.Legal,
                                                                                                                IPrevalidadorAsistencia.TiposAsistenciaConsultar.AS_PED3,
                                                                                                                parametrosAsistencia_)

                If prevalidadorAsistencia_.EstatusAsistencia.Status = TypeStatus.Ok Then

                    'Se extrae el tipo asistencia en una clase para luego extraer por LinQ la lista de regimen
                    Dim asistencia_ As Asistencia = prevalidadorAsistencia_.EstatusAsistencia.ObjectReturned

                    Dim listaRegimen_ = From camposAfectados_ In asistencia_.CamposAfectados("CA_REGIMEN")
                                        Select camposAfectados_

                    'Se hace el IN O EL = en la clausula libre Del componente que le corresponde
                    If listaRegimen_.Count = 1 Then

                        If listaRegimen_(0).ToString = IPrevalidadorAsistencia.ErroresAsistencia.EAS_002.ToString Then


                        ElseIf listaRegimen_(0).ToString <> IPrevalidadorAsistencia.ErroresAsistencia.EAS_003.ToString Then

                            scRegimen.FreeClauses = " and t_Cve_Regimen = " + listaRegimen_(0).ToString

                        Else

                            DisplayMessage(Recursos.GetEnumDescription(IPrevalidadorAsistencia.ErroresAsistencia.EAS_003), StatusMessage.Info)
                            'Condición para que no muestre información
                            scRegimen.FreeClauses = " and i_Cve_Estado = 5"

                        End If

                    Else

                        scRegimen.FreeClauses = " and t_Cve_Regimen in (" + String.Join(",", listaRegimen_) + ")"

                    End If

                ElseIf prevalidadorAsistencia_.EstatusAsistencia.Status = TypeStatus.OkInfo Then

                    DisplayMessage(prevalidadorAsistencia_.EstatusAsistencia.ErrorDescription, StatusMessage.Info)
                    'Condición para que no muestre información
                    scRegimen.FreeClauses = " and i_Cve_Estado = 5"

                ElseIf prevalidadorAsistencia_.EstatusAsistencia.Status = TypeStatus.OkBut Then

                    DisplayMessage(prevalidadorAsistencia_.EstatusAsistencia.ErrorDescription, StatusMessage.Info)
                    'Condición para que no muestre información
                    scRegimen.FreeClauses = " and i_Cve_Estado = 5"

                Else

                    DisplayMessage(prevalidadorAsistencia_.EstatusAsistencia.ErrorDescription, StatusMessage.Fail)
                    'Condición para que no muestre información
                    scRegimen.FreeClauses = " and i_Cve_Estado = 5"

                End If

            End Using

        Else

            scClaveDocumento.ToolTip = "Debe indicar la clave de pedimento"
            scClaveDocumento.ToolTipExpireTime = 4
            scClaveDocumento.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut
            scClaveDocumento.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
            scClaveDocumento.ShowToolTip()
            'Condición para que no muestre información
            scRegimen.FreeClauses = " and i_Cve_Estado = 5"

        End If

    End Sub

    Protected Sub scTipoDespacho_Click(sender As Object, e As EventArgs)

        'Obtenemos el tipo de pedimento en una variable para aplicar los casos necesarios
        _tipoPedimento = scTipoDocumento.Text

        If _tipoPedimento <> "" Then

            Dim recursos_ As ControladorRecursosAduanalesGral = ControladorRecursosAduanalesGral.Buscar(ControladorRecursosAduanalesGral.TiposRecurso.Generales)
            Dim tiposDespacho_ As IEnumerable(Of TiposDespachoDocumento)

            tiposDespacho_ = From data In recursos_.tiposdespachodocumento
                             Where data.archivado = False And data.estado = 1 And data._idtipodocumento = scTipoDocumento.Value

            If tiposDespacho_.Count > 0 Then

                Dim listaDespachos_ As New List(Of SelectOption)

                For index_ As Int32 = 0 To tiposDespacho_.Count - 1

                    listaDespachos_.Add(New SelectOption With
                                 {.Value = tiposDespacho_(index_)._idtipodespacho,
                                  .Text = tiposDespacho_(index_).descripciondespacho.ToString})

                Next

                If listaDespachos_.Count > 0 Then

                    scTipoDespacho.DataSource = listaDespachos_

                End If

            End If

        Else

            MostrarTooltipTipoPedimento(2)

        End If

    End Sub

    Protected Sub SeleccionarUnidadMedida_Click(sender As Object, e As EventArgs)

        Dim listaUnidades_ As List(Of UnidadMedida) = ControladorUnidadesMedida.BuscarUnidades(TiposUnidad.Comercial, scUnidadMedida.SuggestedText, 0)

        If listaUnidades_.Count > 0 Then

            scUnidadMedida.DataSource = ControladorUnidadesMedida.ToSelectOption(listaUnidades_, ControladorUnidadesMedida.TipoSelectOption.CveMXnombreoficiales)

        End If

    End Sub

    Protected Sub SeleccionarUnidadMedidaMulti_Click(sender As Object, e As EventArgs)

        Dim listaUnidades_ As List(Of UnidadMedida) = ControladorUnidadesMedida.BuscarUnidades(TiposUnidad.Comercial, scUnidadMedida.SuggestedText, 0)

        If listaUnidades_.Count > 0 Then

            scUnidadMedidaMulti.DataSource = ControladorUnidadesMedida.ToSelectOption(listaUnidades_, ControladorUnidadesMedida.TipoSelectOption.CveMXnombreoficiales)

        End If

    End Sub

    Protected Sub SeleccionarPais_Click(sender As Object, e As EventArgs)

    End Sub

    Protected Sub scModalidadAduanaPatente_Click(sender As Object, e As EventArgs)

        scPatente.DataSource = ModalidadSeccionPatente()

    End Sub

    Sub MostrarTooltipClavePedimento()

        scClaveDocumento.ToolTip = "Asistencia aplicada"
        scClaveDocumento.ToolTipExpireTime = 4
        scClaveDocumento.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo
        scClaveDocumento.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        scClaveDocumento.ShowToolTip()

    End Sub

    Sub LimpiaAsistencias()

        'Limpiamos los selectores de asistencias por si estan llenos o tiene algún dato
        scClaveDocumento.DataSource = Nothing
        scClaveDocumento.Value = Nothing
        scRegimen.DataSource = Nothing
        scRegimen.Value = Nothing
        scTipoDespacho.DataSource = Nothing
        scTipoDespacho.Value = Nothing

    End Sub

    Sub MostrarTooltipTipoPedimento(ByVal modalidad_ As Integer)

        Select Case modalidad_

            Case 1
                'Aplica asistencia
                scTipoDocumento.ToolTip = "Asistencia aplicada"
                scTipoDocumento.ToolTipExpireTime = 4
                scTipoDocumento.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo
                scTipoDocumento.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
                scTipoDocumento.ShowToolTip()

            Case 2
                'Requiere del campo
                scTipoDocumento.ToolTip = "Debe indicar el tipo de pedimento"
                scTipoDocumento.ToolTipExpireTime = 4
                scTipoDocumento.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut
                scTipoDocumento.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
                scTipoDocumento.ShowToolTip()

        End Select

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

                    Return scPrefijo.Text & Mid(Year(Now).ToString, 3, 2) & "-" &
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
                              .Text = aduanasSecciones_(index_).modalidad.ToString & " | " & aduanasSecciones_(index_)._idaduanaseccion & " | " & aduanasSecciones_(index_).ciudad.ToString})

            Next

            Return aduanaSeccionModalidad_

        End If

        Return Nothing

    End Function

    Private Function TipoDocumento() As List(Of SelectOption)

        LimpiaAsistencias()

        Dim recursos_ As ControladorRecursosAduanalesGral = ControladorRecursosAduanalesGral.Buscar(ControladorRecursosAduanalesGral.TiposRecurso.Generales)

        Dim tiposDocumentos_ As IEnumerable(Of TiposDocumentoOperacion)

        If swcTipoOperacion.Checked Then

            tiposDocumentos_ = From data In recursos_.tiposdocumentooperacion
                               Where data.archivado = False And data.estado = 1 And data._idtipooperacion = TiposOperacionAduanal.Importacion

        Else

            tiposDocumentos_ = From data In recursos_.tiposdocumentooperacion
                               Where data.archivado = False And data.estado = 1 And data._idtipooperacion = TiposOperacionAduanal.Exportacion

        End If

        If tiposDocumentos_.Count > 0 Then

            Dim listaDocumentos_ As New List(Of SelectOption)

            For index_ As Int32 = 0 To tiposDocumentos_.Count - 1

                listaDocumentos_.Add(New SelectOption With
                             {.Value = tiposDocumentos_(index_)._idtipodocumento,
                              .Text = tiposDocumentos_(index_).descripciondocumentocorta.ToString})

            Next

            Return listaDocumentos_

        End If

        Return Nothing

    End Function

    Public Function TipoCarga() As List(Of SelectOption)

        Dim recursos_ As ControladorRecursosAduanalesGral = ControladorRecursosAduanalesGral.Buscar(ControladorRecursosAduanalesGral.TiposRecurso.Anexo22)

        Dim tiposCarga_ = From data In recursos_.tiposcargalote
                          Where data.archivado = False And data.estado = 1
                          Select data._idtipocargalote, data.descripcion

        If tiposCarga_.Count > 0 Then

            Dim listaTiposCarga_ As New List(Of SelectOption)

            For index_ As Int32 = 0 To tiposCarga_.Count - 1

                listaTiposCarga_.Add(New SelectOption With
                                {.Value = tiposCarga_(index_)._idtipocargalote,
                                 .Text = tiposCarga_(index_).descripcion.ToString})
            Next

            Return listaTiposCarga_

        End If

        Return Nothing

    End Function

    Protected Sub InicializaCliente(ByVal datosCliente_ As OperacionGenerica)

        icRFC.Value = datosCliente_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Campo(CamposClientes.CA_RFC_CLIENTE).Valor
        icRFCFacturacion.Value = datosCliente_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Campo(CamposClientes.CA_RFC_CLIENTE).Valor

    End Sub

    Private Function ModalidadSeccionPatente() As List(Of SelectOption)

        Dim _modalidadSeccionPatente = ControladorRecursosAduanales.BuscarRecursosAduanales(ControladorRecursosAduanales.TiposRecurso.Generales)

        SetVars("_modalidadSeccionPatente", _modalidadSeccionPatente)

        Dim aduanasSeccionesPatente_ = From data In _modalidadSeccionPatente.aduanaspatentes
                                       Where data.archivado = False And data.estado = 1
                                       Select data._idmodalidadaduanapatente, data.modalidad, data.ciudad, data._idaduanaseccion, data.agenteaduanal, data._idpatente

        If aduanasSeccionesPatente_.Count > 0 Then

            Dim modalidadesAdunaSeccionPatente_ As New List(Of SelectOption)

            For index_ As Int32 = 0 To aduanasSeccionesPatente_.Count - 1

                modalidadesAdunaSeccionPatente_.Add(New SelectOption With
                             {.Value = aduanasSeccionesPatente_(index_)._idmodalidadaduanapatente,
                              .Text = aduanasSeccionesPatente_(index_).modalidad & " | " & aduanasSeccionesPatente_(index_)._idaduanaseccion & " - " & aduanasSeccionesPatente_(index_).ciudad &
                              " | " & aduanasSeccionesPatente_(index_)._idpatente & " - " & aduanasSeccionesPatente_(index_).agenteaduanal})

            Next

            Return modalidadesAdunaSeccionPatente_

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