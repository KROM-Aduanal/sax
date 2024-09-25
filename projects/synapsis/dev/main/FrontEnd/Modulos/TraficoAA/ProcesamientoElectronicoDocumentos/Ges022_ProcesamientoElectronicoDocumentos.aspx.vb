
#Region "├┴┘├┴┘├┴┘├┴┘├┴┘|├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘   DEPENDENCIAS   ├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘"

'RECURSOS DEL CMF
Imports Syn.Documento
Imports MongoDB.Driver
Imports Wma.Exceptions
Imports gsol.Web.Components
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior.CamposProcesamientoElectDocumentos
Imports Syn.Nucleo.RecursosComercioExterior.SecuenciasComercioExterior
Imports Wma.Exceptions.TagWatcher
Imports Wma.Exceptions.TagWatcher.TypeStatus
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Nucleo.Recursos.CamposDomicilio
'UTILERIAS/RECURSOS ADICIONALES
Imports Sax.Web
'OBJETOS DIMENSIONALES (ODS's) Dependencias en MongoDB
Imports Rec.Globals
Imports Rec.Globals.Utils
'OBJETOS BIDIMENSIONALES (ODF's.  Dependencias Krombase/SQL Server)
Imports Syn.Documento.Componentes.Campo
Imports gsol
Imports System.ServiceModel.Channels
Imports gsol.krom
Imports MongoDB.Bson
Imports System.Windows.Forms.VisualStyles
Imports Syn.Documento.Componentes
Imports Syn.Nucleo
Imports Syn.CustomBrokers.Controllers
Imports Rec.Globals.Empresas
Imports Rec.Globals.Controllers.Empresas
Imports Rec.Globals.Utils.Secuencias
Imports System.Linq.Expressions
Imports Rec.Globals.Controllers
Imports SharpCompress.Archives
Imports System.Web.UI.WebControls.Expressions
Imports System.Xml.Serialization
Imports System.Runtime.InteropServices
Imports System.Reflection.Emit
Imports System.IO
Imports System.Windows.Forms
Imports System.Linq
Imports MongoDB.Bson.Serialization.Attributes
Imports System.Drawing.Imaging
Imports gsol.Web.Template
Imports Sax.Web.ControladorBackend
Imports Syn.CustomBrokers.Controllers.ControladorRecursosAduanales
Imports SharpCompress.Common

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

        fsDetallesDocumentos.Enabled = False
        icRazonSocialCliente.Enabled = False

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
        fsDetallesDocumentos.Enabled = True

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
        End If

    End Sub

    Protected Sub scTipoDocumentos_Click(sender As Object, e As EventArgs)
        Dim dataSource_ As New List(Of SelectOption) From {
            New SelectOption With {.Value = 1, .Text = "FACTURA COMERCIAL (.PDF)"},
            New SelectOption With {.Value = 2, .Text = "FACTURA COMERCIAL (.XML)"},
            New SelectOption With {.Value = 3, .Text = "BL (.pdf)"}
        }
        scTipoDocumentos.DataSource = dataSource_

        'If scTipoDocumentos.Value = "1" Then
        '    lbButtonXML.Visible = True
        '    lbButtonIA.Visible = False
        'Else
        '    lbButtonXML.Visible = False
        '    lbButtonIA.Visible = True
        'End If
    End Sub

    Protected Sub scTipoDocumentos_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim aqui = sender
        If scTipoDocumentos.Value <> 1 Then
            lbButtonXML.Visible = True
            lbButtonIA.Visible = False
        Else
            lbButtonXML.Visible = False
            lbButtonIA.Visible = True
        End If
    End Sub

    Protected Sub fcTipoDocumentos_TextChanged(sender As Object, e As EventArgs)


    End Sub

    Protected Sub fcDocumento_ChooseFile(sender As PropiedadesDocumento, e As EventArgs)
        Dim id = ObjectId.GenerateNewId().ToString
        With sender
            ._idpropietario = id
            .nombrepropietario = "Factura comercial ROX"
            .tipovinculacion = PropiedadesDocumento.TiposVinculacion.AgenciaAduanal
            .datosadicionales = New InformacionDocumento With {
                          .foliodocumento = "00000002",
                          .tipodocumento = InformacionDocumento.TiposDocumento.BL,
                          .datospropietario = New InformacionPropietario With {
                              .nombrepropietario = "Factura comercial ROX",
                              ._id = id
                          }
                         }
            .formatoarchivo = PropiedadesDocumento.FormatosArchivo.pdf
        End With
    End Sub

    Protected Sub btnIAProcesar_Click(sender As Object, e As EventArgs)

        Dim listaDocumentos_ As List(Of Newtonsoft.Json.Linq.JObject) =
            Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of Newtonsoft.Json.Linq.JObject))(fcDocumento.Value)

        Dim listaIdsDocumento_ As List(Of ObjectId) = New List(Of ObjectId)

        If listaDocumentos_ IsNot Nothing Then

            Dim i_ = 1
            For Each documento_ In listaDocumentos_

                ccDocumentosProcesados.SetRow(Sub(catalogRow_ As CatalogRow)
                                                  catalogRow_.SetIndice(ccDocumentosProcesados.KeyField, i_)
                                                  catalogRow_.SetColumn(icNombreArchivo, documento_.SelectToken("fileName").ToString)
                                                  catalogRow_.SetColumn(icTipo, scTipoDocumentos.Text)
                                                  catalogRow_.SetColumn(icEstado, "Listo para procesar")
                                                  i_ += 1
                                              End Sub)

                ccDocumentosProcesados.CatalogDataBinding()
                listaIdsDocumento_.Add(ObjectId.Parse(documento_.SelectToken("fileId").ToString))

            Next

            'Dim doc As Byte() = _controladorDocumentos.GetDocument(listaIdsDocumento_(0)).ObjectReturned

            'Dim memoryStream_ As MemoryStream = New MemoryStream(doc)

        End If


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

#End Region
End Class