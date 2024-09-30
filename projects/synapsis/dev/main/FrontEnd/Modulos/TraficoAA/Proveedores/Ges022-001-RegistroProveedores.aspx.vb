
#Region "├┴┘├┴┘├┴┘├┴┘├┴┘|├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘   DEPENDENCIAS   ├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘"

'RECURSOS DEL CMF
Imports Syn.Documento
Imports MongoDB.Driver
Imports Wma.Exceptions
Imports gsol.Web.Components
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior.CamposProveedorOperativo
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
#End Region

Public Class Ges022_001_RegistroProveedores
    Inherits ControladorBackend
#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████
    Private _cantidadDetalles As Int32
#End Region

#Region "██████ Vinculación c/capas inf █████████       SAX      ████████████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████
    Public Overrides Sub Inicializa()
        With Buscador
            .DataObject = New ConstructorProveedoresOperativos(True)
            .addFilter(SeccionesProvedorOperativo.SPRO1, CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, "Proveedor")
            .addFilter(SeccionesProvedorOperativo.SPRO2, CamposProveedorOperativo.CA_RFC_PROVEEDOR, "RFC")
        End With
        If OperacionGenerica IsNot Nothing Then
            _cantidadDetalles = OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Seccion(SeccionesProvedorOperativo.SPRO2).CantidadPartidas
        End If
        scMetodoValoracion.DataEntity = New Anexo22()
        scIncoterm.DataEntity = New Anexo22()
        swcTipoPersona.Checked = True
        swcDestinatario.Checked = True

    End Sub

    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher
        'Datos generales (SeccionesProveedorOperativo.SPRO1)
        [Set](icClave, CP_CVE_PROVEEDOR)
        [Set](cveEmpresa, CP_CVE_EMPRESA)
        [Set](idEmpresa, CP_ID_EMPRESA)
        [Set](swcTipoUso, CP_TIPO_USO, propiedadDelControl_:=PropiedadesControl.Checked)
        [Set](fcRazonSocial, CA_RAZON_SOCIAL_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Text)
        'Detalle proveedor
        If pbDetalleProveedor.PageIndex > 0 Then
            lbNumero.Text = pbDetalleProveedor.PageIndex.ToString()
        End If
        [Set](swcTipoPersona, CP_TIPO_PERSONA_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icIdDomicilio, CamposDomicilio.CP_ID_DOMICILIO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icSecDomicilio, CamposDomicilio.CP_SEC_DOMICILIO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icIdPais, CamposDomicilio.CA_ID_PAIS, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCvePais, CamposDomicilio.CA_CVE_PAIS, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPais, CamposDomicilio.CA_PAIS, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](swcDestinatario, CP_DESTINATARIO_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icRFC, CA_RFC_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCveRfc, CA_CVE_RFC_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        If swcTipoPersona.Checked = False Then
            [Set](icCURP, CA_CURP_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
            [Set](icCveCurp, CA_CVE_CURP_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        End If
        [Set](scDomicilio, CamposDomicilio.CA_DOMICILIO_FISCAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCalle, CamposDomicilio.CA_CALLE, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icNumeroExterior, CamposDomicilio.CA_NUMERO_EXTERIOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icNumeroInterior, CamposDomicilio.CA_NUMERO_INTERIOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icNumeroExtInt, CamposDomicilio.CA_NUMERO_EXT_INT, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCodigoPostal, CamposDomicilio.CA_CODIGO_POSTAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icColonia, CamposDomicilio.CA_COLONIA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icLocalidad, CamposDomicilio.CA_LOCALIDAD, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCiudad, CamposDomicilio.CA_CIUDAD, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCveMunicipio, CamposDomicilio.CA_ENTIDAD_MUNICIPIO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icMunicipio, CamposDomicilio.CA_MUNICIPIO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCveEntidadFederativa, CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icEntidadFederativa, CamposDomicilio.CA_ENTIDAD_FEDERATIVA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](pbDetalleProveedor, Nothing, seccion_:=SeccionesProvedorOperativo.SPRO2)
        ' Vinculaciones con clientes (SeccionesProvedorOperativo.SPRO4)
        [Set](scClienteVinculacion, CP_ID_CLIENTE_VINCULACION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scTaxIdVinculacion, CP_RFC_PROVEEDOR_VINCULACION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scVinculacion, CA_CVE_VINCULACION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPorcentajeVinculacion, CP_PORCENTAJE_VINCULACION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ccVinculaciones, Nothing, seccion_:=SeccionesProvedorOperativo.SPRO4)
        'Configuración adicional (SeccionesProvedorOperativo.SPRO5)
        [Set](scClienteConfiguracion, CP_ID_CLIENTE_CONFIGURACION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scTaxIdConfiguracion, CP_RFC_PROVEEDOR_CONFIGURACION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scMetodoValoracion, CA_CVE_METODO_VALORACION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scIncoterm, CA_CVE_INCOTERM, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ccConfiguracionAdicional, Nothing, seccion_:=SeccionesProvedorOperativo.SPRO5)
        Return New TagWatcher(1)
    End Function

    Public Overrides Sub BotoneraClicNuevo()
        If OperacionGenerica IsNot Nothing Then
        End If
        LimpiaSesion()

        If pbDetalleProveedor.PageIndex > 0 Then
            lbNumero.Text = pbDetalleProveedor.PageIndex.ToString()
        End If

        PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbDetalleProveedor)
        swcTipoPersona.Checked = True
        swcDestinatario.Checked = True
        fsVinculaciones.Visible = False
        fsConfiguracionAdicional.Visible = False
        fsHistorialDomicilios.Visible = False
        'swcTipoDomicilio.Checked = True
        icPais.Enabled = False
        icPais.Value = "MEX - MÉXICO (ESTADOS UNIDOS MEXICANOS)"
        pbDetalleProveedor.Enabled = True
    End Sub

    Public Overrides Sub BotoneraClicGuardar()
        Dim modoEditando_ As Boolean = False
        If GetVars("isEditing") IsNot Nothing Then
            If GetVars("isEditing") = True Then
                modoEditando_ = True
            End If
        End If
        If modoEditando_ = False Then
            If fcRazonSocial.Text = "" Then
                MsgValidacionRazonsocialVacio()
            ElseIf icRFC.Value = "" Then
                MsgValidacionRfcVacio()
            ElseIf icCalle.Value = "" Then
                MsgValidacionCalleVacio()
            ElseIf icPais.Value = "" Then
                MsgValidacionPaisVacio()
            Else
                If BuscarSiExisterProveedor() Then
                    aviso.Visible = True
                Else
                    If Not ProcesarTransaccion(Of ConstructorProveedoresOperativos)().Status = TypeStatus.Errors Then : End If
                End If
            End If
        Else
            If Not ProcesarTransaccion(Of ConstructorProveedoresOperativos)().Status = TypeStatus.Errors Then : End If
        End If
    End Sub

    Public Overrides Sub BotoneraClicEditar()
        PreparaTarjetero(PillboxControl.ToolbarModality.Advanced, pbDetalleProveedor)
        fcRazonSocial.Enabled = False
        icPais.Enabled = False
        SetVars("rfcProveedorActual", icRFC.Value)
    End Sub

    Public Overrides Sub BotoneraClicBorrar()

    End Sub

    Public Overrides Sub BotoneraClicOtros(IndexSelected_ As Integer)
        Dim indice_ As Integer = 0
        If GetVars("_indice") IsNot Nothing Then
            indice_ = Int(GetVars("_indice"))
        End If
        If IndexSelected_ = 7 Then
            ConfigurarDomicilios.Visible = True
            ConfigurarDomicilios.Enabled = True
            VaciarFormulario(indice_)
        ElseIf IndexSelected_ = 8 Then
            VaciarFormulario(indice_)
        End If
    End Sub

    Private Sub VaciarFormulario(ByVal indice_ As Integer)
        Dim pillboxControl_ As PillboxControl = pbDetalleProveedor
        pillboxControl_.SetPillbox(Sub(pillbox_ As PillBox)
                                       pillbox_.SetIndice(pillboxControl_.KeyField, indice_)
                                       pillbox_.SetFiled(False)
                                       If GetVars("isEditing") Is Nothing Then
                                           icRFC.Value = Nothing
                                           icCURP.Value = Nothing
                                           swcTipoPersona.Checked = True
                                           swcDestinatario.Checked = True
                                           icCveRfc.Value = Nothing
                                           icCveCurp.Value = Nothing
                                           swcTipoPersona.Checked = True
                                           swcDestinatario.Checked = True
                                       Else
                                           swcTipoPersona.Checked = pbDetalleProveedor.DataSource(0).Item("swcTipoPersona")
                                           swcDestinatario.Checked = pbDetalleProveedor.DataSource(0).Item("swcDestinatario")
                                       End If
                                       icCalle.Value = Nothing
                                       icNumeroExterior.Value = Nothing
                                       icNumeroInterior.Value = Nothing
                                       icCodigoPostal.Value = Nothing
                                       icColonia.Value = Nothing
                                       icLocalidad.Value = Nothing
                                       icCiudad.Value = Nothing
                                       icMunicipio.Value = Nothing
                                       icEntidadFederativa.Value = Nothing
                                       icIdDomicilio.Value = Nothing
                                       icSecDomicilio.Value = Nothing
                                       scDomicilio.Value = Nothing
                                       icNumeroExtInt.Value = Nothing
                                       icCveMunicipio.Value = Nothing
                                       icCveEntidadFederativa.Value = Nothing
                                   End Sub)
    End Sub

    'EVENTOS PARA LA INSERCIÓN DE DATOS
    Public Overrides Function AntesRealizarInsercion(ByVal session_ As IClientSessionHandle) As TagWatcher
        Dim tagwatcher_ As New TagWatcher
        If session_ IsNot Nothing Then
            GuardarEmpresa(session_)
            tagwatcher_.SetOK()
        Else
            tagwatcher_.SetOK()
        End If
        Return tagwatcher_
    End Function

    Public Overrides Sub RealizarInsercion(ByRef documentoElectronico_ As DocumentoElectronico)
        Dim empresaNacional_ As New EmpresaNacional
        If GetVars("_empresaNacional") IsNot Nothing Then
            empresaNacional_ = DirectCast(GetVars("_empresaNacional"), EmpresaNacional)
        End If
        Dim secuencia_ As New Secuencia
        Dim loginUsuario_ As Dictionary(Of String, String) = Session("DatosUsuario")
        secuencia_ = GenerarSecuencia()
        With documentoElectronico_
            .Campo(CP_TIPO_PROVEEDOR).Valor = 1
            .Campo(CP_TIPO_PROVEEDOR).ValorPresentacion = "Nacional"

            If swcTipoUso.Checked Then
                .Campo(CP_TIPO_USO).Valor = 2
                .Campo(CP_TIPO_USO).ValorPresentacion = "Exportación"
            Else
                .Campo(CP_TIPO_USO).Valor = 1
                .Campo(CP_TIPO_USO).ValorPresentacion = "Importación"
            End If

            icClave.Value = secuencia_.sec
            idEmpresa.Value = empresaNacional_._id.ToString
            cveEmpresa.Value = empresaNacional_._idempresa
            .UsuarioGenerador = loginUsuario_("Nombre")
            .Id = secuencia_._id.ToString
            .IdDocumento = secuencia_.sec
            .FolioDocumento = secuencia_.sec 'DUDA
            .FolioOperacion = empresaNacional_._idempresa 'DUDA
            .TipoPropietario = secuencia_.nombre
            .NombrePropietario = empresaNacional_.razonsocial
            .IdPropietario = empresaNacional_._idempresa
            .ObjectIdPropietario = empresaNacional_._id
        End With
    End Sub

    Public Overrides Function DespuesRealizarInsercion() As TagWatcher
        RegresarControlesPorDefault()
        ConfigurarDomicilios.Visible = False
        Return New TagWatcher(Ok)
    End Function

    'EVENTOS PARA MODIFICACIÓN DE DATOS
    Public Overrides Function AntesRealizarModificacion(ByVal session_ As IClientSessionHandle) As TagWatcher
        Dim tagwatcher_ As New TagWatcher
        If session_ IsNot Nothing Then '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 
            GuardarEmpresa(session_)
            tagwatcher_.SetOK()
        Else  '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas sin transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 
            tagwatcher_.SetOK()
        End If
        Return tagwatcher_
    End Function

    Public Overrides Sub PreparaModificacion(ByRef documentoElectronico_ As DocumentoElectronico)
        If documentoElectronico_ IsNot Nothing Then
            _cantidadDetalles = documentoElectronico_.Seccion(SeccionesProvedorOperativo.SPRO2).CantidadPartidas
        End If

        ''BUSCAR LA EMPRESA ACTUAL
        Dim tagwatcher_ As New TagWatcher
        Dim empresaNacional_ As New EmpresaNacional
        Dim controladorEmpresas_ As New Rec.Globals.Controllers.Empresas.ControladorEmpresas(New EspacioTrabajo())
        tagwatcher_ = controladorEmpresas_.ConsultarUna(New ObjectId(idEmpresa.Value))
        If tagwatcher_.Status = TypeStatus.Ok Then
            empresaNacional_ = DirectCast(tagwatcher_.ObjectReturned, EmpresaNacional)
            SetVars("_empresaNacional", empresaNacional_)
            Dim listaEmpresasNacionales_ As New List(Of EmpresaNacional) From {empresaNacional_}
            SetVars("_listaEmpresasTemporales", listaEmpresasNacionales_)
        End If
        CargarHistorialDomicilios()
        ConfigurarDomicilios.Enabled = False
        ConfigurarDomicilios.Visible = False
    End Sub

    Public Overrides Sub RealizarModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

    End Sub

    Public Overrides Sub DespuesOperadorDatosProcesar(ByRef documentoElectronico_ As DocumentoElectronico)
        With documentoElectronico_
            Dim listaDomicilios_ As New List(Of Rec.Globals.Empresas.Domicilio)
            Dim empresaNacional_ As New EmpresaNacional

            If GetVars("_listaDomiciliosActuales") IsNot Nothing Then
                listaDomicilios_ = GetVars("_listaDomiciliosActuales")
            End If

            If GetVars("_empresaNacional") IsNot Nothing Then
                empresaNacional_ = DirectCast(GetVars("_empresaNacional"), EmpresaNacional)
            End If

            'LISTA DOMICILIOS PROVEEDORES
            Dim proveedores_ = pbDetalleProveedor.DataSource
                Dim i_ = 1
                Dim indice_ = 0
                If listaDomicilios_ IsNot Nothing Then
                    For Each item_ In proveedores_
                        With .Seccion(SeccionesProvedorOperativo.SPRO2).Partida(numeroSecuencia_:=i_)
                        .Campo(CamposDomicilio.CP_ID_DOMICILIO).Valor = listaDomicilios_(indice_)._iddomicilio.ToString
                        .Campo(CamposDomicilio.CP_SEC_DOMICILIO).Valor = listaDomicilios_(indice_).sec
                        .Campo(CamposDomicilio.CA_CVE_PAIS).Valor = empresaNacional_.paisesdomicilios.Last.pais
                        .Campo(CamposDomicilio.CA_ID_PAIS).Valor = empresaNacional_.paisesdomicilios.Last.idpais.ToString
                        .Campo(CamposProveedorOperativo.CA_CVE_RFC_PROVEEDOR).Valor = empresaNacional_._idrfc.ToString
                        .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor = listaDomicilios_(indice_).domicilioPresentacion
                            .Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor = listaDomicilios_(indice_).numeroexterior + " - " + listaDomicilios_(indice_).numerointerior
                            .Campo(CamposDomicilio.CA_ENTIDAD_MUNICIPIO).Valor = listaDomicilios_(indice_).cveMunicipio
                            .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor = listaDomicilios_(indice_).cveEntidadfederativa
                            If swcDestinatario.Checked Then
                                .Campo(CamposProveedorOperativo.CP_DESTINATARIO_PROVEEDOR).Valor = 1
                                .Campo(CamposProveedorOperativo.CP_DESTINATARIO_PROVEEDOR).ValorPresentacion = "Si"
                            Else
                                .Campo(CamposProveedorOperativo.CP_DESTINATARIO_PROVEEDOR).Valor = 2
                                .Campo(CamposProveedorOperativo.CP_DESTINATARIO_PROVEEDOR).ValorPresentacion = "No"
                            End If

                            If swcTipoPersona.Checked Then
                                .Campo(CamposProveedorOperativo.CP_TIPO_PERSONA_PROVEEDOR).Valor = 1
                                .Campo(CamposProveedorOperativo.CP_TIPO_PERSONA_PROVEEDOR).ValorPresentacion = "Moral"
                            Else
                                .Campo(CamposProveedorOperativo.CP_TIPO_PERSONA_PROVEEDOR).Valor = 2
                                .Campo(CamposProveedorOperativo.CP_TIPO_PERSONA_PROVEEDOR).ValorPresentacion = "Fisica"
                                .Campo(CamposProveedorOperativo.CA_CVE_CURP_PROVEEDOR).Valor = empresaNacional_._idcurp.ToString
                            End If
                        End With
                        i_ += 1
                        indice_ += 1
                    Next
                End If
            End With
    End Sub

    Public Overrides Function DespuesRealizarModificacion() As TagWatcher
        RegresarControlesPorDefault()
        Return New TagWatcher(Ok)
    End Function

    'EVENTOS PARA PRESENTACIÓN DE DATOS EN FRONTEND
    Public Overrides Sub DespuesBuquedaGeneralConDatos()
        fsVinculaciones.Visible = IIf(_cantidadDetalles > 0, True, False)
        fsConfiguracionAdicional.Visible = IIf(_cantidadDetalles > 0, True, False)
        fsHistorialDomicilios.Visible = IIf(_cantidadDetalles > 0, True, False)
        PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbDetalleProveedor)
        fsVinculaciones.Enabled = True
        fsConfiguracionAdicional.Enabled = True
        fsHistorialDomicilios.Enabled = True
        pbDetalleProveedor.Enabled = True
        fcRazonSocial.Enabled = False
    End Sub

    Public Overrides Sub DespuesBuquedaGeneralSinDatos()
        Session("_tbDetalleProveedor") = PillboxControl.ToolbarModality.Default : pbDetalleProveedor.Modality = Session("_tbDetalleProveedor")
        PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbDetalleProveedor)
        pbDetalleProveedor.Enabled = False
        fsVinculaciones.Enabled = False
        fsConfiguracionAdicional.Enabled = False
        fsHistorialDomicilios.Enabled = False
    End Sub
    'EVENTOS DE MANTENIMIENTO
    Public Overrides Sub LimpiaSesion()
        SetVars("_empresaNacional", Nothing)
        SetVars("_listaEmpresasTemporales", Nothing)
        SetVars("_listaDomicilios", Nothing)
        SetVars("_listaDomiciliosActuales", Nothing)
        SetVars("opcionesLista_", Nothing)
        SetVars("_indice", Nothing)
        SetVars("_cveUltimoDomicilio", Nothing)
    End Sub

    Public Overrides Sub Limpiar()
        ccDomiciliosFiscales.DataSource = Nothing
        ccVinculaciones.DataSource = Nothing
        ccConfiguracionAdicional.DataSource = Nothing
        _cantidadDetalles = Nothing
        scDomiciliosRegistrados.DataSource = Nothing
        scDomiciliosRegistrados.Value = Nothing
        pbDetalleProveedor.DataSource = Nothing
        ConfigurarDomicilios.Visible = False
        icPais.Value = "MEX - MÉXICO (ESTADOS UNIDOS MEXICANOS)"
    End Sub
#End Region

#Region "████████████████  QUINTA CAPA  █████████       Reglas locales         ██████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████
    'EVENTO PARA REGRESAR CONTROLES POR CADA ACCIÓN DE TARJETA
    Public Sub RegresarControles(Optional ByVal opcion_ As Int32 = 1)
        If pbDetalleProveedor.PageIndex > 0 Then
            lbNumero.Text = pbDetalleProveedor.PageIndex.ToString()
        End If
    End Sub


    'EVENTOS PARA LA RAZON SOCIAL
    Protected Sub fcRazonSocial_TextChanged(sender As Object, e As EventArgs)
        fcRazonSocial.DataSource = ListarEmpresas(Of EmpresaNacional)()
    End Sub

    Protected Sub fcRazonSocial_Click(sender As Object, e As EventArgs)
        If fcRazonSocial.Text <> "" Then
            If BuscarSiExisterProveedor() = False Then
                LlenarFormulario()
                ConfigurarDomicilios.Visible = True
                ConfigurarDomicilios.Enabled = True
            Else
                MsgValidacionRazonsocial()
            End If
        Else
            aviso.Visible = False
            ConfigurarDomicilios.Visible = False
            swcTipoPersona.Checked = True
            swcDestinatario.Checked = True
            SetVars("_listaEmpresasTemporales", Nothing)
            SetVars("opcionesLista_", Nothing)
            Limpiar()
        End If
    End Sub

    Protected Function BuscarSiExisterProveedor() As Boolean
        Dim buscarProveedorExistente_ As New ControladorBusqueda(Of ConstructorProveedoresOperativos)
        Dim lista_ = buscarProveedorExistente_.Buscar(fcRazonSocial.Text,
                                                              New Filtro _
                                                              With {.IdSeccion = SeccionesProvedorOperativo.SPRO1,
                                                                    .IdCampo = CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR})
        If lista_ IsNot Nothing Then
            If lista_.Count > 0 Then
                'aviso.Visible = True
                'MsgValidacionRazonsocial()
                Return True
            End If
        End If

        Return False
    End Function

    'EVENTOS PARA CONTROLAR QUE PASA CON EL TIPO DE USO
    Protected Sub swcTipoUso_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    'EVENTOS PARA CONTROLAR QUE PASA CON EL TIPO DE PERSONA
    Protected Sub swcTipoPersona_CheckedChanged(sender As Object, e As EventArgs)
        If swcTipoPersona.Checked Then
            icCURP.Visible = False
            icCURP.Value = Nothing
        Else
            icCURP.Visible = True
        End If
    End Sub

    Protected Sub swcDestinatario_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    'EVENTO PARA EL RFC
    Protected Sub icRFC_GotFocus(sender As Object, e As EventArgs)

    End Sub

    'Protected Sub icRFC_TextChanged(sender As Object, e As EventArgs)
    '    Dim modoEditando_ As Boolean = False
    '    If GetVars("isEditing") IsNot Nothing Then
    '        If GetVars("isEditing") = True Then
    '            modoEditando_ = True
    '        End If
    '    End If
    '    If modoEditando_ Then
    '        CardControlRFC.Visible = True
    '    End If
    'End Sub

    'Protected Sub btnCambiarRfcDomicilios_Click(sender As Object, e As EventArgs)
    '    CardControlRFC.Visible = False
    '    Dim rfcNuevo = icRFC.Value
    '    Dim pillboxControl_ = pbDetalleProveedor
    '    Dim indice_ = 0
    '    pbDetalleProveedor.ClearRows()
    '    For Each item_ In pbDetalleProveedor.DataSource
    '        Dim archivado_ = pbDetalleProveedor.DataSource(indice_).Item("archivado")
    '        If archivado_ = False Then
    '            pillboxControl_.SetPillbox(Sub(pillbox_ As PillBox)
    '                                           pillbox_.SetIndice(pillboxControl_.KeyField, indice_)
    '                                           pillbox_.SetFiled(False)
    '                                           pillbox_.SetControlValue(icRFC, rfcNuevo)
    '                                           pillbox_.SetControlValue(icCveRfc, Nothing)
    '                                           pillbox_.SetControlValue(icIdPais, pbDetalleProveedor.DataSource(indice_).Item("icIdPais"))
    '                                           pillbox_.SetControlValue(icCvePais, pbDetalleProveedor.DataSource(indice_).Item("icCvePais"))
    '                                           pillbox_.SetControlValue(icPais, pbDetalleProveedor.DataSource(indice_).Item("icPais"))
    '                                           pillbox_.SetControlValue(icCalle, pbDetalleProveedor.DataSource(indice_).Item("icCalle"))
    '                                           pillbox_.SetControlValue(icNumeroExterior, pbDetalleProveedor.DataSource(indice_).Item("icNumeroExterior"))
    '                                           pillbox_.SetControlValue(icNumeroInterior, pbDetalleProveedor.DataSource(indice_).Item("icNumeroInterior"))
    '                                           pillbox_.SetControlValue(icCodigoPostal, pbDetalleProveedor.DataSource(indice_).Item("icCodigoPostal"))
    '                                           pillbox_.SetControlValue(icColonia, pbDetalleProveedor.DataSource(indice_).Item("icColonia"))
    '                                           pillbox_.SetControlValue(icLocalidad, pbDetalleProveedor.DataSource(indice_).Item("icLocalidad"))
    '                                           pillbox_.SetControlValue(icCiudad, pbDetalleProveedor.DataSource(indice_).Item("icCiudad"))
    '                                           pillbox_.SetControlValue(icMunicipio, pbDetalleProveedor.DataSource(indice_).Item("icMunicipio"))
    '                                           pillbox_.SetControlValue(icEntidadFederativa, pbDetalleProveedor.DataSource(indice_).Item("icEntidadFederativa"))
    '                                           pillbox_.SetControlValue(icIdDomicilio, pbDetalleProveedor.DataSource(indice_).Item("icIdDomicilio"))
    '                                           pillbox_.SetControlValue(icSecDomicilio, pbDetalleProveedor.DataSource(indice_).Item("icSecDomicilio"))
    '                                           pillbox_.SetControlValue(scDomicilio, pbDetalleProveedor.DataSource(indice_).Item("scDomicilio"))
    '                                           pillbox_.SetControlValue(icNumeroExtInt, pbDetalleProveedor.DataSource(indice_).Item("icNumeroExtInt"))
    '                                           pillbox_.SetControlValue(icCveMunicipio, pbDetalleProveedor.DataSource(indice_).Item("icCveMunicipio"))
    '                                           pillbox_.SetControlValue(icCveEntidadFederativa, pbDetalleProveedor.DataSource(indice_).Item("icCveEntidadFederativa"))
    '                                       End Sub)
    '        End If
    '        indice_ += 1
    '    Next
    '    pbDetalleProveedor = pillboxControl_
    '    pbDetalleProveedor.PillBoxDataBinding()
    'End Sub

    'Protected Sub btnNoCambiarRfcDomicilios_Click(sender As Object, e As EventArgs)
    '    CardControlRFC.Visible = False
    '    icRFC.Value = GetVars("rfcProveedorActual")
    'End Sub

    Sub VerificaCheckTipoUso(Optional ByVal opcion_ As Int32 = 1)

    End Sub

    'EVENTOS PARA CARGAR LOS CLIENTES EN LAS LISTAS
    Protected Sub scClienteVinculacion_Click(sender As Object, e As EventArgs)
        Dim controlador_ As New ControladorBusqueda(Of ConstructorCliente)
        Dim lista_ As List(Of SelectOption) = controlador_.Buscar(scClienteVinculacion.SuggestedText, New Filtro With {.IdSeccion = SeccionesClientes.SCS1, .IdCampo = CamposClientes.CA_RAZON_SOCIAL})
        scClienteVinculacion.DataSource = lista_
    End Sub

    Protected Sub scClienteVinculacion_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub scClienteConfiguracion_Click(sender As Object, e As EventArgs)
        Dim controlador_ As New ControladorBusqueda(Of ConstructorCliente)
        Dim lista_ As List(Of SelectOption) = controlador_.Buscar(scClienteConfiguracion.SuggestedText,
                                                                  New Filtro With {.IdSeccion = SeccionesClientes.SCS1, .IdCampo = CamposClientes.CA_RAZON_SOCIAL})
        scClienteConfiguracion.DataSource = lista_
    End Sub

    Protected Sub scClienteConfiguracion_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub scTaxIdVinculacion_Click(sender As Object, e As EventArgs)
        scTaxIdVinculacion.DataSource = IdentificadoresProveedor()
    End Sub

    Protected Sub scVinculacion_Click(sender As Object, e As EventArgs)
        Dim recursos_ As ControladorRecursosAduanalesGral =
            ControladorRecursosAduanalesGral.Buscar(ControladorRecursosAduanalesGral.TiposRecurso.Anexo22)
        Dim vinculaciones_ = From data In recursos_.tiposvinculacion
                             Where data.archivado = False And data.estado = 1
                             Select data._idvinculacion, data.descripcion, data.descripcioncorta
        If vinculaciones_.Count > 0 Then
            Dim dataSource_ As New List(Of SelectOption)
            For index_ As Int32 = 0 To vinculaciones_.Count - 1
                dataSource_.Add(New SelectOption With
                             {.Value = vinculaciones_(index_)._idvinculacion,
                              .Text = vinculaciones_(index_)._idvinculacion.ToString & " - " & vinculaciones_(index_).descripcioncorta})
            Next
            scVinculacion.DataSource = dataSource_
        End If
    End Sub
    Protected Sub scVinculacion_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Function IdentificadoresProveedor() As List(Of SelectOption)
        Dim listaIdentificadores_ As New List(Of SelectOption)
        pbDetalleProveedor.ForEach(Sub(pillbox_ As PillBox)
                                       listaIdentificadores_.Add(New SelectOption With {
                                                                 .Text = pillbox_.GetControlValue(icRFC),
                                                                 .Value = pillbox_.GetIndice(pbDetalleProveedor.KeyField)})
                                   End Sub)
        Return listaIdentificadores_
    End Function

    Protected Sub scTaxIdConfiguracion_Click(sender As Object, e As EventArgs)
        scTaxIdConfiguracion.DataSource = IdentificadoresProveedor()
    End Sub

    'EVENTOs PARA MODIFICAR EL LABEL CUANDO CAMBIA DE POSICIÓN EL TARJETERO
    Protected Sub pbDetalleProveedor_CheckedChange(sender As Object, e As EventArgs)
        RegresarControles()
    End Sub

    Protected Sub pbDetalleProveedor_Click(sender As Object, e As EventArgs)
        RegresarControles()
        Select Case pbDetalleProveedor.ToolbarAction
            Case PillboxControl.ToolbarActions.Nuevo
                lbNumero.Text = pbDetalleProveedor.PageIndex.ToString()
                Dim itemActual_ As Integer = pbDetalleProveedor.PageIndex
                Dim empresaNacional_ As New EmpresaNacional
                Dim opcionesLista_ As New List(Of SelectOption)
                If GetVars("_empresaNacional") IsNot Nothing Then
                    empresaNacional_ = DirectCast(GetVars("_empresaNacional"), EmpresaNacional)
                End If

                Dim listaCvesDomiciliosActuales As New List(Of String)

                pbDetalleProveedor.ForEach(Sub(pillbox_ As PillBox)
                                               If pillbox_.GetControlValue(icIdDomicilio) IsNot Nothing Then
                                                   listaCvesDomiciliosActuales.Add(pillbox_.GetControlValue(icIdDomicilio))
                                               End If
                                           End Sub)

                Dim domiciliosDesdeEmpresa_ As List(Of Rec.Globals.Empresas.Domicilio) = empresaNacional_.paisesdomicilios.Last.domicilios
                For Each item_ In domiciliosDesdeEmpresa_
                    If Not listaCvesDomiciliosActuales.Contains(item_._iddomicilio.ToString) Then
                        opcionesLista_.Add(New SelectOption With
                            {
                                .Value = item_._iddomicilio.ToString,
                                .Text = item_.domicilioPresentacion
                            })
                    End If
                Next

                If opcionesLista_.Count > 0 Then
                    ConfigurarDomicilios.Visible = True
                    scDomiciliosRegistrados.DataSource = opcionesLista_
                    scDomiciliosRegistrados.Value = opcionesLista_.First.Value
                    SetVars("opcionesLista_", opcionesLista_)
                    SetVars("_indice", itemActual_)
                    SetVars("_cveUltimoDomicilio", opcionesLista_.First.Value)
                End If
                swcTipoPersona.Checked = pbDetalleProveedor.DataSource(0).Item("swcTipoPersona")
                swcTipoPersona.Enabled = False
                icRFC.Value = pbDetalleProveedor.DataSource(0).Item("icRFC")
                icRFC.Enabled = False
                icCveRfc.Value = pbDetalleProveedor.DataSource(0).Item("icCveRfc")
                If pbDetalleProveedor.DataSource(0).Item("icCURP") <> "" Then
                    icCURP.Value = pbDetalleProveedor.DataSource(0).Item("icCURP")
                    icCveCurp.Value = pbDetalleProveedor.DataSource(0).Item("icCveCurp")
                    icCURP.Visible = True
                    icCURP.Enabled = False
                    swcTipoPersona.Checked = False
                End If
                swcDestinatario.Checked = pbDetalleProveedor.DataSource(0).Item("swcDestinatario")
                icPais.Value = pbDetalleProveedor.DataSource(0).Item("icPais")
                icCvePais.Value = pbDetalleProveedor.DataSource(0).Item("icCvePais")
                icIdPais.Value = pbDetalleProveedor.DataSource(0).Item("icIdPais")
            Case Else
        End Select
    End Sub

    Protected Sub scDomicilios_SelectedIndexChanged(sender As Object, e As EventArgs)
        'Aqui vamos a determinar si vamos por los domiclios a mongo, que yo creo que si, woa a ver
    End Sub

    Protected Sub scDomicilios_Click(sender As Object, e As EventArgs)

    End Sub

    Protected Sub icMunicipio_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub icEntidadFederativa_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub swcUtilizarDatos_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    Sub VerificaCheckDomicilio(Optional ByVal opcion_ As Int32 = 1)

    End Sub

    'EVENTOS PARA CARGAR LAS VINCULACIONES
    Protected Sub rdSeleccionarDomicilio_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub btnTipoDomicilio_Click(sender As Object, e As EventArgs)
        ConfigurarDomicilios.Visible = False
        pbDetalleProveedor.Enabled = True
        Dim domicilioSeleccionado_ As String
        Dim indice_ As Integer = 0
        If scDomiciliosRegistrados.Value = "" Then
            domicilioSeleccionado_ = GetVars("_cveUltimoDomicilio")
        Else
            domicilioSeleccionado_ = scDomiciliosRegistrados.Value
        End If

        If GetVars("_indice") IsNot Nothing Then
            indice_ = Int(GetVars("_indice"))
        End If

        CambiarDomicilio(domicilioSeleccionado_, indice_)
    End Sub


    Protected Sub scDomiciliosRegistrados_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub scDomiciliosRegistrados_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub scDomiciliosRegistrados_Click(sender As Object, e As EventArgs)
        Dim opcionesLista_ = DirectCast(GetVars("opcionesLista_"), List(Of SelectOption))
        scDomiciliosRegistrados.DataSource = opcionesLista_
        scDomiciliosRegistrados.Value = opcionesLista_.Last.Value
    End Sub
#End Region

#Region "██████ Vinculación sexta capa  █████████       SAX      ████████████████████████████████████████████"
    '    ██████   Controladores utilizados                     Documentos por coding para MongoDB      ██████
    '    ██████    1.ControladorEmpresas                        1. En Empresa:                         ██████
    '    ██████    2.ControladorRecursosAduanales                  a). Domicilios                      ██████
    '    ██████    3.ControladorSecuencias                         b). Contactos                       ██████
    '    ██████                                                                                        ██████
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████
    Protected Sub BloquearCampos(ByVal value As Boolean)
        icRFC.Enabled = value
        swcTipoPersona.Enabled = value
        swcDestinatario.Enabled = value
        icCalle.Enabled = value
        icNumeroExterior.Enabled = value
        icNumeroInterior.Enabled = value
        icCodigoPostal.Enabled = value
        icColonia.Enabled = value
        icLocalidad.Enabled = value
        icCiudad.Enabled = value
        icMunicipio.Enabled = value
        icEntidadFederativa.Enabled = value
    End Sub

    Public Overrides Function AgregarComponentesBloqueadosInicial() As List(Of WebControl)
        Dim bloqueados_ As New List(Of WebControl) From {
            icClave,
            icPais
        }
        Return bloqueados_
    End Function

    Public Overrides Function AgregarComponentesBloqueadosEdicion() As List(Of WebControl)
        Dim bloqueadosEdicion_ As New List(Of WebControl) From {icClave, fcRazonSocial, swcTipoUso, icRFC, icCURP, swcTipoPersona, icPais}
        Return bloqueadosEdicion_
    End Function

    Private Sub CambiarDomicilio(ByVal cveDomicilio_ As String, Optional ByVal indice_ As Integer = 0)
        Dim tagwatcher_ As New TagWatcher
        Dim controladorEmpresas_ As New Rec.Globals.Controllers.Empresas.ControladorEmpresas(New EspacioTrabajo()) With {.ListaEmpresas = New List(Of IEmpresa)}
        Dim empresaNacional_ As EmpresaNacional = DirectCast(GetVars("_empresaNacional"), EmpresaNacional)
        With controladorEmpresas_
            .Modalidad = IControladorEmpresas.Modalidades.Intrinseca
            .ListaEmpresas.Add(empresaNacional_)
            tagwatcher_ = .ConsultarDomicilio(empresaNacional_._id, New ObjectId(cveDomicilio_))
        End With
        Try
            If tagwatcher_.Status = TypeStatus.Ok Then
                Dim domicilioSeleccionado = DirectCast(tagwatcher_.ObjectReturned, List(Of Rec.Globals.Empresas.Domicilio))
                If indice_ <> 0 Then
                    LlenarTarjetero(domicilioSeleccionado, indice_)
                Else
                    CargarTarjetero(domicilioSeleccionado)
                End If
            End If
        Catch ex As Exception
            DisplayMessage("No hay cambios", StatusMessage.Fail)
        End Try
    End Sub

    Private Function GenerarRfc() As Rec.Globals.Empresas.Rfc
        Dim rfc_ As New Rec.Globals.Empresas.Rfc
        pbDetalleProveedor.ForEach(Sub(pillbox_ As PillBox)
                                       rfc_ = New Rec.Globals.Empresas.Rfc(pillbox_.GetControlValue(icRFC))
                                       pillbox_.SetControlValue(icCveRfc, rfc_.idrfc.ToString)
                                   End Sub)
        Return rfc_
    End Function

    Private Function GuardarEmpresa(ByVal session_ As IClientSessionHandle) As TagWatcher
        Dim tagwatcher_ As New TagWatcher
        Dim espacioTrabajo_ As New EspacioTrabajo()
        Dim empresaNacional_ As New EmpresaNacional
        Dim controladorEmpresas_ As New Rec.Globals.Controllers.Empresas.ControladorEmpresas(New EspacioTrabajo()) With {
            .ListaEmpresas = New List(Of IEmpresa)
        }
        Dim existeEmpresa_ = False
        If GetVars("_empresaNacional") IsNot Nothing Then
            empresaNacional_ = DirectCast(GetVars("_empresaNacional"), EmpresaNacional)
            existeEmpresa_ = True
        End If
        Dim rfc_ As String = Nothing
        Dim curp_ As String = Nothing
        Dim listaDomicilios_ As New List(Of Rec.Globals.Empresas.Domicilio)
        pbDetalleProveedor.ForEach(Sub(pillbox_ As PillBox)
                                       rfc_ = pillbox_.GetControlValue(icRFC)
                                       curp_ = pillbox_.GetControlValue(icCURP)
                                       If existeEmpresa_ Then
                                           With empresaNacional_
                                               If rfc_ <> .rfc Then
                                                   Dim rfcNuevo_ As New Rec.Globals.Empresas.Rfc(rfc_, IIf(.rfcs IsNot Nothing, .rfcs.Count + 1, 1))
                                                   ._idrfc = rfcNuevo_.idrfc
                                                   .rfc = rfcNuevo_.rfc
                                                   .rfcs.Add(rfcNuevo_)
                                                   .rfcNuevo = True
                                               End If

                                               If swcTipoPersona.Checked = False Then
                                                   If curp_ <> .curp Then
                                                       Dim curpNuevo_ As New Rec.Globals.Empresas.Curp(curp_, IIf(.curps IsNot Nothing, .curps.Count + 1, 1))
                                                       ._idcurp = curpNuevo_.idcurp
                                                       .curp = curpNuevo_.curp
                                                       .curps.Add(curpNuevo_)
                                                       .curpNuevo = True
                                                   End If
                                               End If
                                           End With
                                       End If

                                       Dim domicilio_ As New Rec.Globals.Empresas.Domicilio(pillbox_.GetControlValue(icCalle),
                                                                                            1,
                                                                                            pillbox_.GetControlValue(icNumeroExterior),
                                                                                            pillbox_.GetControlValue(icNumeroInterior),
                                                                                            pillbox_.GetControlValue(icColonia),
                                                                                            pillbox_.GetControlValue(icCodigoPostal),
                                                                                            pillbox_.GetControlValue(icCiudad),
                                                                                            pillbox_.GetControlValue(icLocalidad),
                                                                                            pillbox_.GetControlValue(icCveMunicipio),
                                                                                            pillbox_.GetControlValue(icMunicipio),
                                                                                            pillbox_.GetControlValue(icCveEntidadFederativa),
                                                                                            pillbox_.GetControlValue(icEntidadFederativa),
                                                                                            pais_:=pillbox_.GetControlValue(icPais)
                                                                                            )
                                       If existeEmpresa_ Then
                                           Dim domicilioPresentacionActual_ = pillbox_.GetControlValue(scDomicilio)
                                           Dim domicilioObtenido_ = domicilio_.domicilioPresentacion

                                           If Not String.Equals(domicilioObtenido_, domicilioPresentacionActual_) Then
                                               domicilio_.sec = empresaNacional_.paisesdomicilios.Last.domicilios.Count + 1
                                               empresaNacional_.paisesdomicilios.Last.domicilios.Add(domicilio_)
                                               tagwatcher_ = controladorEmpresas_.Modificar(empresaNacional_, session_)
                                           Else
                                               domicilio_._iddomicilio = New ObjectId(pillbox_.GetControlValue(icIdDomicilio))
                                               domicilio_.sec = pillbox_.GetControlValue(icSecDomicilio)
                                           End If
                                       End If
                                       If domicilio_.calle <> "" Then
                                           listaDomicilios_.Add(domicilio_)
                                       End If
                                   End Sub)
        If existeEmpresa_ Then
            If empresaNacional_.rfcNuevo Then
                tagwatcher_ = controladorEmpresas_.Modificar(empresaNacional_, session_)
            ElseIf empresaNacional_.curpNuevo Then
                tagwatcher_ = controladorEmpresas_.Modificar(empresaNacional_, session_)
            End If
        Else
            If swcTipoPersona.Checked = False Then
                empresaNacional_ = controladorEmpresas_.EstructuraEmpresaNacional(fcRazonSocial.Text,
                                                                                  rfc_,
                                                                                  listaDomicilios_.Last,
                                                                                  IEmpresaNacional.TiposPersona.Fisica,
                                                                                  curp_)
            Else
                empresaNacional_ = controladorEmpresas_.EstructuraEmpresaNacional(fcRazonSocial.Text,
                                                                                     rfc_,
                                                                                     listaDomicilios_.Last)
            End If
            tagwatcher_ = controladorEmpresas_.Agregar(empresaNacional_, True, session_)
        End If
        SetVars("_empresaNacional", empresaNacional_)
        SetVars("_listaDomiciliosActuales", listaDomicilios_)
        If tagwatcher_.Status = TypeStatus.Ok Then
            If tagwatcher_.ObjectReturned IsNot Nothing Then
                empresaNacional_ = DirectCast(tagwatcher_.ObjectReturned(0), EmpresaNacional)
                SetVars("_empresaNacional", empresaNacional_)
            End If
        End If
        Return tagwatcher_
    End Function

    Private Function Vinculacion() As List(Of SelectOption)
        Dim recursos_ As ControladorRecursosAduanalesGral =
            ControladorRecursosAduanalesGral.Buscar(ControladorRecursosAduanalesGral.TiposRecurso.Anexo22)
        Dim vinculaciones_ = From data In recursos_.tiposvinculacion
                             Where data.archivado = False And data.estado = 1
                             Select data._idvinculacion, data.descripcion, data.descripcioncorta
        If vinculaciones_.Count > 0 Then
            Dim dataSource_ As New List(Of SelectOption)
            For index_ As Int32 = 0 To vinculaciones_.Count - 1
                dataSource_.Add(New SelectOption With
                             {.Value = vinculaciones_(index_)._idvinculacion,
                              .Text = vinculaciones_(index_)._idvinculacion.ToString & " - " & vinculaciones_(index_).descripcioncorta})
            Next
            Return dataSource_
        End If
        Return Nothing
    End Function

    'EVENTO PARA LEER EL HISTORIAL DE DOMICILIO
    Protected Sub CargarHistorialDomicilios()
        Dim i = 1
        pbDetalleProveedor.ForEach(Sub(pillbox_ As PillBox)
                                       ccDomiciliosFiscales.SetRow(Sub(catalogRow_ As CatalogRow)
                                                                       catalogRow_.SetIndice(ccDomiciliosFiscales.KeyField, i)
                                                                       catalogRow_.SetColumn(icTaxIDRFC, pillbox_.GetControlValue(icRFC))
                                                                       catalogRow_.SetColumn(icDomicilio, pillbox_.GetControlValue(scDomicilio))
                                                                       catalogRow_.SetColumn(swcArchivarDomicilio, pillbox_.IsFiled())
                                                                   End Sub)
                                       i += 1
                                   End Sub)
        ccDomiciliosFiscales.CatalogDataBinding()
    End Sub

    'Buscar empresa nacional por razon social
    Private Function ListarEmpresas(Of T)() As List(Of SelectOption)
        Dim lista_ As New List(Of SelectOption)
        Dim tagwatcher_ As New TagWatcher
        Dim espacioTrabajo_ As New EspacioTrabajo()
        Dim empresaNacional_ As New EmpresaNacional
        Dim controladorEmpresas_ = New Rec.Globals.Controllers.Empresas.ControladorEmpresas(espacioTrabajo_)
        controladorEmpresas_.ListaEmpresas = New List(Of IEmpresa)
        With controladorEmpresas_
            tagwatcher_ = .Consultar(fcRazonSocial.Text)
            If tagwatcher_.Status = TypeStatus.Ok Then
                Dim listaempresas_ = tagwatcher_.ObjectReturned
                If listaempresas_.count > 0 Then
                    SetVars("_listaEmpresasTemporales", listaempresas_)
                    For Each item_ In listaempresas_
                        lista_.Add(New SelectOption _
                               With {
                                    .Value = item_._id.ToString,
                                    .Text = item_.razonsocial})
                    Next
                End If
            End If
        End With
        Return lista_
    End Function

    Protected Sub LLenarListaDomiciliosEmpresas(ByVal indice_ As Integer)
        Dim empresaNacional_ As New EmpresaNacional
        Dim opcionesLista_ As New List(Of SelectOption)
        If GetVars("_empresaNacional") IsNot Nothing Then
            empresaNacional_ = DirectCast(GetVars("_empresaNacional"), EmpresaNacional)
        End If
        Dim pillboxControl_ As PillboxControl = pbDetalleProveedor
        Dim domiciliosDesdeEmpresa_ As List(Of Rec.Globals.Empresas.Domicilio) = empresaNacional_.paisesdomicilios.Last.domicilios
        For Each item_ In domiciliosDesdeEmpresa_
            opcionesLista_.Add(New SelectOption With
                                {
                                    .Value = item_._iddomicilio.ToString,
                                    .Text = item_.domicilioPresentacion
                                })
        Next
        If opcionesLista_.Count > 0 And scDomiciliosRegistrados.DataSource Is Nothing Then
            scDomiciliosRegistrados.DataSource = opcionesLista_
            scDomiciliosRegistrados.Value = opcionesLista_.First.Value
            SetVars("opcionesLista_", opcionesLista_)
            SetVars("_indice", indice_)
        End If
    End Sub

    Protected Sub LlenarFormulario()
        Dim tagwatcher_ As New TagWatcher
        Dim controladorEmpresas_ As New Rec.Globals.Controllers.Empresas.ControladorEmpresas(New EspacioTrabajo()) With {
            .ListaEmpresas = New List(Of IEmpresa)
        }
        Dim empresaNacional_ As New EmpresaNacional
        Dim ultimoDomicilio_ As New List(Of Rec.Globals.Empresas.Domicilio)
        Dim opcionesLista_ As New List(Of SelectOption)
        Dim pillboxControl_ As New PillboxControl
        If GetVars("_listaEmpresasTemporales") IsNot Nothing Then
            Dim listaEmpresasNacionales_ As List(Of EmpresaNacional) = DirectCast(GetVars("_listaEmpresasTemporales"), List(Of EmpresaNacional))
            If listaEmpresasNacionales_.Count > 0 Then
                empresaNacional_ = DirectCast(listaEmpresasNacionales_.Find(Function(x) x.razonsocial = fcRazonSocial.Text), EmpresaNacional)
                SetVars("_empresaNacional", empresaNacional_)
                controladorEmpresas_.Modalidad = IControladorEmpresas.Modalidades.Intrinseca
                controladorEmpresas_.ListaEmpresas.Add(empresaNacional_)
                tagwatcher_ = controladorEmpresas_.ConsultarDomicilios(empresaNacional_._id)
                If tagwatcher_.Status = TypeStatus.Ok Then
                    Dim listaDomicilios_ As List(Of Rec.Globals.Empresas.Domicilio) = DirectCast(tagwatcher_.ObjectReturned, List(Of Rec.Globals.Empresas.Domicilio))
                    If listaDomicilios_.Count > 0 Then
                        ultimoDomicilio_.Add(listaDomicilios_.Last)
                        For Each item_ In listaDomicilios_
                            opcionesLista_.Add(New SelectOption With
                                                {
                                                    .Value = item_._iddomicilio.ToString,
                                                    .Text = item_.domicilioPresentacion
                                                })
                        Next
                        If opcionesLista_.Count > 0 And scDomiciliosRegistrados.DataSource Is Nothing Then
                            scDomiciliosRegistrados.DataSource = opcionesLista_
                            scDomiciliosRegistrados.Value = opcionesLista_.Last.Value
                            If empresaNacional_.curp IsNot Nothing Then
                                If empresaNacional_.curp <> "" Then
                                    swcTipoPersona.Checked = False
                                    icCURP.Visible = True
                                End If
                            End If
                            SetVars("opcionesLista_", opcionesLista_)
                            SetVars("listaDomicilios_", listaDomicilios_)
                            SetVars("_cveUltimoDomicilio", opcionesLista_.Last.Value)
                        End If
                    End If
                End If
            Else
                SetVars("_listaEmpresasTemporales", Nothing)
            End If
        Else
            SetVars("_listaEmpresasTemporales", Nothing)
        End If
    End Sub

    Sub LlenarTarjetero(ByVal domicilio_ As List(Of Rec.Globals.Empresas.Domicilio),
                        ByVal indice_ As Integer)
        Dim pillboxControl_ As PillboxControl = pbDetalleProveedor
        For Each item_ In domicilio_
            pillboxControl_.SetPillbox(Sub(pillbox_ As PillBox)
                                           pillbox_.SetIndice(pillboxControl_.KeyField, indice_)
                                           pillbox_.SetFiled(False)
                                           icCalle.Value = item_.calle
                                           icNumeroExterior.Value = item_.numeroexterior
                                           icNumeroInterior.Value = item_.numerointerior
                                           icCodigoPostal.Value = item_.codigopostal
                                           icColonia.Value = item_.colonia
                                           icLocalidad.Value = item_.localidad
                                           icCiudad.Value = item_.ciudad
                                           icMunicipio.Value = item_.municipio
                                           icEntidadFederativa.Value = item_.entidadfederativa
                                           icIdDomicilio.Value = item_._iddomicilio.ToString
                                           icSecDomicilio.Value = item_.sec
                                           scDomicilio.Value = item_.domicilioPresentacion
                                           icNumeroExtInt.Value = item_.numeroexterior & " - " & item_.numerointerior
                                           icCveMunicipio.Value = item_.cveMunicipio
                                           icCveEntidadFederativa.Value = item_.cveEntidadfederativa
                                       End Sub)
        Next
    End Sub

    Sub CargarTarjetero(ByVal domicilio_ As List(Of Rec.Globals.Empresas.Domicilio),
                        Optional ByVal indice_ As Integer = 0)
        Dim empresaNacional_ As New EmpresaNacional
        If GetVars("_empresaNacional") IsNot Nothing Then
            empresaNacional_ = DirectCast(GetVars("_empresaNacional"), EmpresaNacional)
        End If

        If empresaNacional_.curp IsNot Nothing Then
            If empresaNacional_.curp <> "" Then
                swcTipoPersona.Checked = False
                icCURP.Visible = True
            End If
        End If
        Dim pillboxControl_ As PillboxControl = pbDetalleProveedor
        pillboxControl_.ClearRows()
            For Each item_ In domicilio_
            pillboxControl_.SetPillbox(Sub(pillbox_ As PillBox)
                                           pillbox_.SetIndice(pillboxControl_.KeyField, indice_)
                                           pillbox_.SetFiled(False)
                                           pillbox_.SetControlValue(icRFC, empresaNacional_.rfc)
                                           pillbox_.SetControlValue(icCveRfc, empresaNacional_._idrfc.ToString)
                                           If empresaNacional_.curp IsNot Nothing Then
                                               If empresaNacional_.curp <> "" Then
                                                   pillbox_.SetControlValue(icCURP, empresaNacional_.curp)
                                                   pillbox_.SetControlValue(icCveCurp, empresaNacional_._idcurp.ToString)
                                               End If
                                           End If
                                           pillbox_.SetControlValue(icIdPais, empresaNacional_.paisesdomicilios.Last.idpais.ToString)
                                           pillbox_.SetControlValue(icCvePais, empresaNacional_.paisesdomicilios.Last.pais)
                                           pillbox_.SetControlValue(icPais, empresaNacional_.paisesdomicilios.Last.paisPresentacion)
                                           pillbox_.SetControlValue(icCalle, item_.calle)
                                           pillbox_.SetControlValue(icNumeroExterior, item_.numeroexterior)
                                           pillbox_.SetControlValue(icNumeroInterior, item_.numerointerior)
                                           pillbox_.SetControlValue(icCodigoPostal, item_.codigopostal)
                                           pillbox_.SetControlValue(icColonia, item_.colonia)
                                           pillbox_.SetControlValue(icLocalidad, item_.localidad)
                                           pillbox_.SetControlValue(icCiudad, item_.ciudad)
                                           pillbox_.SetControlValue(icMunicipio, item_.municipio)
                                           pillbox_.SetControlValue(icEntidadFederativa, item_.entidadfederativa)
                                           pillbox_.SetControlValue(icIdDomicilio, item_._iddomicilio.ToString)
                                           pillbox_.SetControlValue(icSecDomicilio, item_.sec)
                                           pillbox_.SetControlValue(scDomicilio, item_.domicilioPresentacion)
                                           pillbox_.SetControlValue(icNumeroExtInt, item_.numeroexterior & " - " & item_.numerointerior)
                                           pillbox_.SetControlValue(icCveMunicipio, item_.cveMunicipio)
                                           pillbox_.SetControlValue(icCveEntidadFederativa, item_.cveEntidadfederativa)
                                           indice_ += 1
                                       End Sub)
        Next
        pbDetalleProveedor = pillboxControl_
        pbDetalleProveedor.PillBoxDataBinding()
        SetVars("_listaDomicilios", pbDetalleProveedor.DataSource)
    End Sub

    Protected Sub RegresarControlesPorDefault()
        Dim modoEditando_ As Boolean = False
        If GetVars("isEditing") IsNot Nothing Then
            If GetVars("isEditing") = True Then
                modoEditando_ = True
            End If
        End If
        If pbDetalleProveedor.PageIndex > 0 Then
            If pbDetalleProveedor.PageIndex > 0 Then
                lbNumero.Text = pbDetalleProveedor.PageIndex.ToString()
            End If
            Session("_tbDetalleProveedor") = PillboxControl.ToolbarModality.Default : pbDetalleProveedor.Modality = Session("_tbDetalleProveedor")
            If modoEditando_ Then
                PreparaTarjetero(PillboxControl.ToolbarModality.Advanced, pbDetalleProveedor)
                If swcTipoPersona.Checked = False Then
                    icCURP.Visible = True
                End If
                fcRazonSocial.Enabled = False
                icRFC.Enabled = False
                icCURP.Enabled = False
                swcTipoPersona.Enabled = False
                'swcDestinatario.Enabled = False
                swcTipoUso.Enabled = False
            Else
                PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbDetalleProveedor)
                swcTipoPersona.Checked = True
                swcDestinatario.Checked = True
                If swcTipoPersona.Checked = False Then
                    icCURP.Visible = True
                Else
                    icCURP.Visible = False
                End If
            End If

            fsVinculaciones.Enabled = True
            fsConfiguracionAdicional.Enabled = True
            fsHistorialDomicilios.Enabled = True

            scDomiciliosRegistrados.DataSource = Nothing
            icClave.Enabled = False
            fsHistorialDomicilios.Visible = True
            fsVinculaciones.Visible = True
            fsConfiguracionAdicional.Visible = True
            CargarHistorialDomicilios()
        End If
    End Sub

    Protected Function GenerarSecuencia() As Secuencia
        Dim tagwatcher_ As New TagWatcher
        Dim secuencia_ As New Secuencia
        Dim controladorSecuencias_ = New ControladorSecuencia
        tagwatcher_ = controladorSecuencias_.Generar(SecuenciasComercioExterior.ProveedoresOperativos.ToString, 1, 1, 1, 1, 1)
        If tagwatcher_.Status = TypeStatus.Ok Then
            secuencia_ = DirectCast(tagwatcher_.ObjectReturned, Secuencia)
            secuencia_.nombre = SecuenciasComercioExterior.ProveedoresOperativos.ToString
        End If
        Return secuencia_
    End Function

    Protected Sub MsgValidacionRazonsocial()
        fcRazonSocial.ToolTip = "Este proveedor ya existe. "
        'fcRazonSocial.ToolTipExpireTime = 4
        fcRazonSocial.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
        fcRazonSocial.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        fcRazonSocial.ShowToolTip()
    End Sub

    Protected Sub MsgValidacionRazonsocialVacio()
        fcRazonSocial.ToolTip = "Indica una razón social. "
        fcRazonSocial.ToolTipExpireTime = 4
        fcRazonSocial.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
        fcRazonSocial.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        fcRazonSocial.ShowToolTip()
    End Sub

    Protected Sub MsgValidacionRfcVacio()
        icRFC.ToolTip = "Indica un rfc. "
        icRFC.ToolTipExpireTime = 4
        icRFC.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
        icRFC.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        icRFC.ShowToolTip()
    End Sub

    Protected Sub MsgValidacionCalleVacio()
        icCalle.ToolTip = "Indica una calle. "
        icCalle.ToolTipExpireTime = 4
        icCalle.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
        icCalle.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        icCalle.ShowToolTip()
    End Sub

    Protected Sub MsgValidacionPaisVacio()
        icPais.ToolTip = "Indica un país. "
        icPais.ToolTipExpireTime = 4
        icPais.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
        icPais.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        icPais.ShowToolTip()
    End Sub

#End Region
End Class