
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
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
#End Region

Public Class Ges022_ProveedorExtranjero
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
            .addFilter(SeccionesProvedorOperativo.SPRO2, CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR, "TAXID")
        End With
        If OperacionGenerica IsNot Nothing Then
            _cantidadDetalles = OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Seccion(SeccionesProvedorOperativo.SPRO2).CantidadPartidas
        End If
        Dim pillboxControl_ As PillboxControl = pbDetalleProveedorInternacional
        scMetodoValoracion.DataEntity = New Anexo22()
        scIncoterm.DataEntity = New Anexo22()
    End Sub

    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher
        'Datos generales (SeccionesProveedorOperativo.SPRO1)
        [Set](icClave, CP_CVE_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](cveEmpresa, CP_CVE_EMPRESA, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](idEmpresa, CP_ID_EMPRESA, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](swcTipoUso, CP_TIPO_USO, propiedadDelControl_:=PropiedadesControl.Checked)
        [Set](fcRazonSocial, CA_RAZON_SOCIAL_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Text)
        'Detalle proveedor
        If pbDetalleProveedorInternacional.PageIndex > 0 Then
            lbNumero.Text = pbDetalleProveedorInternacional.PageIndex.ToString()
        End If

        [Set](icTaxid, CA_TAX_ID_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCveTaxid, CA_CVE_TAX_ID_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icIdDomicilio, CamposDomicilio.CP_ID_DOMICILIO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icSecDomicilio, CamposDomicilio.CP_SEC_DOMICILIO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icIdPais, CamposDomicilio.CA_ID_PAIS, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCvePais, CamposDomicilio.CA_CVE_PAIS, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPais, CamposDomicilio.CA_PAIS, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](swcDestinatario, CP_DESTINATARIO_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scDomicilio, CamposDomicilio.CA_DOMICILIO_FISCAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCalle, CamposDomicilio.CA_CALLE, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icNumeroExterior, CamposDomicilio.CA_NUMERO_EXTERIOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icNumeroInterior, CamposDomicilio.CA_NUMERO_INTERIOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icNumeroExtInt, CamposDomicilio.CA_NUMERO_EXT_INT, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCodigoPostal, CamposDomicilio.CA_CODIGO_POSTAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icColonia, CamposDomicilio.CA_COLONIA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icLocalidad, CamposDomicilio.CA_LOCALIDAD, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCiudad, CamposDomicilio.CA_CIUDAD, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icMunicipio, CamposDomicilio.CA_MUNICIPIO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCveMunicipio, CamposDomicilio.CA_ENTIDAD_MUNICIPIO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCveEntidadFederativa, CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icEntidadFederativa, CamposDomicilio.CA_ENTIDAD_FEDERATIVA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](pbDetalleProveedorInternacional, Nothing, seccion_:=SeccionesProvedorOperativo.SPRO2)
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
        If pbDetalleProveedorInternacional.PageIndex > 0 Then
            lbNumero.Text = pbDetalleProveedorInternacional.PageIndex.ToString()
        End If
        PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbDetalleProveedorInternacional)
        swcDestinatario.Checked = True
        icClave.Enabled = False
        fsVinculaciones.Visible = False
        fsConfiguracionAdicional.Visible = False
        fsHistorialDomicilios.Visible = False
        icPais.Enabled = False
        ConfigurarDomicilios.Visible = True
        ConfigurarDomicilios.Enabled = True
        pbDetalleProveedorInternacional.Enabled = False
        swcTipoUso.Checked = True
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
            ElseIf icCalle.Value = "" Then
                MsgValidacionCalleVacio()
            ElseIf icPais.Value = "" Then
                MsgValidacionPaisVacio()
            Else
                If BuscarSiExisterProveedor() Then
                    'aviso.Visible = True
                    DisplayMessage("Proveedor ya registrado.", StatusMessage.Fail)
                Else
                    If Not ProcesarTransaccion(Of ConstructorProveedoresOperativos)().Status = TypeStatus.Errors Then : End If
                End If
            End If
        Else
            If Not ProcesarTransaccion(Of ConstructorProveedoresOperativos)().Status = TypeStatus.Errors Then : End If
        End If
    End Sub

    Public Overrides Sub BotoneraClicEditar()
        PreparaTarjetero(PillboxControl.ToolbarModality.Advanced, pbDetalleProveedorInternacional)
        icClave.Enabled = False
        fcRazonSocial.Enabled = False
        icPais.Enabled = False
    End Sub

    Public Overrides Sub BotoneraClicBorrar()

    End Sub

    Public Overrides Sub BotoneraClicOtros(IndexSelected_ As Integer)
        Dim itemActual_ As Integer = pbDetalleProveedorInternacional.PageIndex
        If IndexSelected_ = 7 Then
            ConfigurarDomicilios.Visible = True
            'AQUI
        ElseIf IndexSelected_ = 8 Then
            VaciarFormulario(itemActual_)
        End If
    End Sub

    Private Sub VaciarFormulario(ByVal indice_ As Integer)
        Dim pillboxControl_ As PillboxControl = pbDetalleProveedorInternacional
        pillboxControl_.SetPillbox(Sub(pillbox_ As PillBox)
                                       pillbox_.SetIndice(pillboxControl_.KeyField, indice_)
                                       pillbox_.SetFiled(False)
                                       icTaxid.Value = Nothing
                                       swcDestinatario.Checked = True
                                       icCveTaxid.Value = Nothing
                                       swcDestinatario.Checked = pbDetalleProveedorInternacional.DataSource(0).Item("swcDestinatario")
                                       icCalle.Value = Nothing
                                       icNumeroExterior.Value = Nothing
                                       icNumeroInterior.Value = Nothing
                                       icCodigoPostal.Value = Nothing
                                       icColonia.Value = Nothing
                                       icLocalidad.Value = Nothing
                                       icCiudad.Value = Nothing
                                       icMunicipio.Value = Nothing
                                       icEntidadFederativa.Value = Nothing
                                       scDomicilio.Value = Nothing
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
        Dim empresaInternacional_ As New EmpresaInternacional
        If GetVars("_empresaInternacional") IsNot Nothing Then
            empresaInternacional_ = DirectCast(GetVars("_empresaInternacional"), EmpresaInternacional)
        End If
        Dim loginUsuario_ As Dictionary(Of String, String) = Session("DatosUsuario")
        Dim secuencia_ As Secuencia = GenerarSecuencia()
        With documentoElectronico_
            .Campo(CP_TIPO_PROVEEDOR).Valor = 2
            .Campo(CP_TIPO_PROVEEDOR).ValorPresentacion = "Extranjero"

            If swcTipoUso.Checked Then
                .Campo(CP_TIPO_USO).Valor = 1
                .Campo(CP_TIPO_USO).ValorPresentacion = "Importación"
            Else
                .Campo(CP_TIPO_USO).Valor = 2
                .Campo(CP_TIPO_USO).ValorPresentacion = "Exportación"
            End If

            icClave.Value = secuencia_.sec
            idEmpresa.Value = empresaInternacional_._id.ToString
            cveEmpresa.Value = empresaInternacional_._idempresa
            .UsuarioGenerador = loginUsuario_("Nombre")
            .Id = secuencia_._id.ToString
            .IdDocumento = secuencia_.sec
            .FolioDocumento = secuencia_.sec 'DUDA
            .FolioOperacion = empresaInternacional_._idempresa 'DUDA
            .TipoPropietario = secuencia_.nombre
            .NombrePropietario = empresaInternacional_.razonsocial
            .IdPropietario = empresaInternacional_._idempresa
            .ObjectIdPropietario = empresaInternacional_._id
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
        Dim empresaInternacional_ As New EmpresaInternacional
        Dim cvePais_ As String = icPais.Value.Substring(0, 3)
        Dim controladorEmpresas_ As New Rec.Globals.Controllers.Empresas.ControladorEmpresas(New EspacioTrabajo(),
                                                                                             IControladorEmpresas.TiposEmpresas.Internacional,
                                                                                             paisEmpresa_:=cvePais_) With {.ListaEmpresas = New List(Of IEmpresa)}
        tagwatcher_ = controladorEmpresas_.ConsultarUna(New ObjectId(idEmpresa.Value))
        If tagwatcher_.Status = TypeStatus.Ok Then
            empresaInternacional_ = DirectCast(tagwatcher_.ObjectReturned, EmpresaInternacional)
            SetVars("_empresainternacional", empresaInternacional_)
            Dim listaempresasinternacionales_ As New List(Of EmpresaInternacional) From {empresaInternacional_}
            SetVars("_listaempresastemporales", listaempresasinternacionales_)
        End If
        CargarHistorialDomicilios()
        ConfigurarDomicilios.Enabled = False
        ConfigurarDomicilios.Visible = False
    End Sub

    Public Overrides Sub RealizarModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

    End Sub

    Public Overrides Sub DespuesOperadorDatosProcesar(ByRef documentoElectronico_ As DocumentoElectronico)
        With documentoElectronico_
            Dim empresaInternacional_ As New EmpresaInternacional
            Dim datosAdicionalesActuales_ As New List(Of Dictionary(Of String, String))
            Dim listaDomicilios_ As New List(Of Rec.Globals.Empresas.Domicilio)
            If GetVars("_listaDomiciliosActuales") IsNot Nothing Then
                listaDomicilios_ = DirectCast(GetVars("_listaDomiciliosActuales"), List(Of Rec.Globals.Empresas.Domicilio))
            End If

            If GetVars("_empresaInternacional") IsNot Nothing Then
                empresaInternacional_ = DirectCast(GetVars("_empresaInternacional"), EmpresaInternacional)
            End If

            If GetVars("_datosAdicionalesActuales") IsNot Nothing Then
                datosAdicionalesActuales_ = DirectCast(GetVars("_datosAdicionalesActuales"), List(Of Dictionary(Of String, String)))
            End If

            'LISTA DOMICILIOS PROVEEDORES
            Dim proveedores_ = pbDetalleProveedorInternacional.DataSource
            Dim i_ = 1
            Dim indice_ = 0
            If listaDomicilios_ IsNot Nothing Then
                For Each item_ In proveedores_
                    With .Seccion(SeccionesProvedorOperativo.SPRO2).Partida(numeroSecuencia_:=i_)
                        .Campo(CamposDomicilio.CP_ID_DOMICILIO).Valor = listaDomicilios_(indice_)._iddomicilio.ToString
                        .Campo(CamposDomicilio.CP_SEC_DOMICILIO).Valor = listaDomicilios_(indice_).sec
                        .Campo(CamposDomicilio.CA_CVE_PAIS).Valor = datosAdicionalesActuales_(indice_)("cvePais_")
                        .Campo(CamposDomicilio.CA_ID_PAIS).Valor = datosAdicionalesActuales_(indice_)("objectIdPais_")
                        .Campo(CamposProveedorOperativo.CA_CVE_TAX_ID_PROVEEDOR).Valor = datosAdicionalesActuales_(indice_)("cvetaxid_")
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
        PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbDetalleProveedorInternacional)
    End Sub

    Public Overrides Sub DespuesBuquedaGeneralSinDatos()
        Session("_tbDetalleProveedor") = PillboxControl.ToolbarModality.Default : pbDetalleProveedorInternacional.Modality = Session("_tbDetalleProveedor")
        PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbDetalleProveedorInternacional)
        pbDetalleProveedorInternacional.Enabled = True
    End Sub
    'EVENTOS DE MANTENIMIENTO
    Public Overrides Sub LimpiaSesion()
        SetVars("_empresaInternacional", Nothing)
        SetVars("_listaEmpresasTemporales", Nothing)
        SetVars("_listaDomicilios", Nothing)
        SetVars("_listaDomiciliosActuales", Nothing)
        SetVars("_opcionesLista", Nothing)
        SetVars("_indice", Nothing)
        SetVars("_cveUltimoDomicilio", Nothing)
        SetVars("_datosAdicionalesActuales", Nothing)
    End Sub

    Public Overrides Sub Limpiar()
        ccDomiciliosFiscales.DataSource = Nothing
        ccVinculaciones.DataSource = Nothing
        ccConfiguracionAdicional.DataSource = Nothing
        _cantidadDetalles = Nothing
        scDomiciliosRegistrados.DataSource = Nothing
        scDomiciliosRegistrados.Value = Nothing
        pbDetalleProveedorInternacional.DataSource = Nothing
        ConfigurarDomicilios.Visible = False
    End Sub
#End Region

#Region "████████████████  QUINTA CAPA  █████████       Reglas locales         ██████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████
    'EVENTO PARA REGRESAR CONTROLES POR CADA ACCIÓN DE TARJETA
    Public Sub RegresarControles(Optional ByVal opcion_ As Int32 = 1)
        If pbDetalleProveedorInternacional.PageIndex > 0 Then
            lbNumero.Text = pbDetalleProveedorInternacional.PageIndex.ToString()
        End If
    End Sub

    'EVENTOS PARA LA RAZON SOCIAL
    Protected Sub fcRazonSocial_TextChanged(sender As Object, e As EventArgs)
        fcRazonSocial.DataSource = ListarEmpresas(Of EmpresaInternacional)()
    End Sub

    Protected Sub fcRazonSocial_Click(sender As Object, e As EventArgs)
        If fcRazonSocial.Text <> "" Then
            BuscarSiExisterProveedor()
        Else
            aviso.Visible = False
            swcDestinatario.Checked = False
            SetVars("_listaEmpresasTemporales", Nothing)
            SetVars("_opcionesLista", Nothing)
            Limpiar()
            LimpiarTarjetero()
            icPais.Value = Nothing
            fcPaises.Value = Nothing
            fcPaises.Text = Nothing
            scDomiciliosRegistrados.Value = Nothing
            scDomiciliosRegistrados.Visible = False
            lbtitleDomicilios.Visible = False
            pbDetalleProveedorInternacional.Enabled = False
        End If
        ConfigurarDomicilios.Visible = True
    End Sub

    Protected Function BuscarSiExisterProveedor() As Boolean
        Dim buscarProveedorExistente_ As New ControladorBusqueda(Of ConstructorProveedoresOperativos)
        Dim lista_ = buscarProveedorExistente_.Buscar(fcRazonSocial.Text,
                                                              New Filtro _
                                                              With {.IdSeccion = SeccionesProvedorOperativo.SPRO1,
                                                                    .IdCampo = CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR})
        If lista_ IsNot Nothing Then
            If lista_.Count > 0 Then
                aviso.Visible = True
                'MsgValidacionRazonsocial()
                Return True
            End If
        End If

        Return False
    End Function

    Protected Sub fcPaises_Click(sender As Object, e As EventArgs)
        Dim modoEditando_ As Boolean = False
        If GetVars("isEditing") IsNot Nothing Then
            If GetVars("isEditing") = True Then
                modoEditando_ = True
            End If
        End If

        If fcPaises.Text <> "" Then
            If modoEditando_ = False Then
                If BuscarSiExisterProveedor() = False Then
                    scDomiciliosRegistrados.DataSource = Nothing
                    LlenarFormulario(fcPaises.Text)
                End If
            End If
        Else
            If modoEditando_ = False Then
                LimpiarTarjetero()
                icPais.Value = Nothing
                icCvePais.Value = Nothing
                icIdPais.Value = Nothing
                scDomiciliosRegistrados.Visible = False
                lbtitleDomicilios.Visible = False
                icPais.Value = Nothing
                fcPaises.Value = Nothing
                fcPaises.Text = Nothing
                lbtitleDomicilios.Visible = False
                scDomiciliosRegistrados.DataSource = Nothing
            End If
        End If

    End Sub

    Protected Sub fcPaises_TextChanged(sender As Object, e As EventArgs)
        CargaPaises(sender)
    End Sub

    Function CargaPaises(ByRef control_ As FindboxControl) As List(Of SelectOption)
        Dim paisesTemporales_ As New List(Of Pais)
        Dim lista_ As List(Of SelectOption) = ControladorPaises.BuscarPaises(paisesTemporales_, control_.Text)
        control_.DataSource = lista_
        Return lista_
    End Function

    'EVENTOS PARA CONTROLAR QUE PASA CON EL TIPO DE USO
    Protected Sub swcTipoUso_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    'EVENTOS PARA CONTROLAR QUE PASA CON EL TIPO DE PERSONA
    Protected Sub swcDestinatario_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    Sub VerificaCheckTipoUso(Optional ByVal opcion_ As Int32 = 1)

    End Sub

    'EVENTOS PARA CARGAR LOS CLIENTES EN LAS LISTAS
    Protected Sub scClienteVinculacion_Click(sender As Object, e As EventArgs)
        Dim controlador_ As New ControladorBusqueda(Of ConstructorCliente)
        Dim lista_ As List(Of SelectOption) = controlador_.Buscar(scClienteVinculacion.SuggestedText, New Filtro _
                                                                  With {.IdSeccion = SeccionesClientes.SCS1, .IdCampo = CamposClientes.CA_RAZON_SOCIAL})
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
        pbDetalleProveedorInternacional.ForEach(Sub(pillbox_ As PillBox)
                                                    listaIdentificadores_.Add(New SelectOption With {
                                                                 .Text = pillbox_.GetControlValue(icTaxid),
                                                                 .Value = pillbox_.GetIndice(pbDetalleProveedorInternacional.KeyField)})
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
        Select Case pbDetalleProveedorInternacional.ToolbarAction
            Case PillboxControl.ToolbarActions.Nuevo
                With pbDetalleProveedorInternacional
                    lbNumero.Text = .PageIndex.ToString()
                    Dim itemActual_ As Integer = .PageIndex
                    Dim index_ As Integer = itemActual_ - 2
                    Dim paisActual_ = .DataSource(index_).Item("icCvePais")
                    ConfiguradorDomicilios(itemActual_, paisActual_)
                    icTaxid.Value = .DataSource(index_).Item("icTaxid")
                    swcDestinatario.Checked = .DataSource(index_).Item("swcDestinatario")
                    icPais.Value = .DataSource(index_).Item("icPais")
                    icIdPais.Value = .DataSource(index_).Item("icIdPais")
                    fcPaises.Value = .DataSource(index_).Item("icIdPais")
                    fcPaises.Text = .DataSource(index_).Item("icPais")
                End With

            Case Else
        End Select
    End Sub

    Protected Sub ConfiguradorDomicilios(ByVal indice_ As Integer, ByVal pais_ As String)
        If GetVars("_empresaInternacional") IsNot Nothing Then
            Dim empresaInternacional_ As EmpresaInternacional = DirectCast(GetVars("_empresaInternacional"), EmpresaInternacional)
            Dim opcionesLista_ As New List(Of SelectOption)
            Dim listaCvesDomiciliosActuales As New List(Of String)

            pbDetalleProveedorInternacional.ForEach(Sub(pillbox_ As PillBox)
                                                        If pillbox_.GetControlValue(icIdDomicilio) IsNot Nothing Then
                                                            listaCvesDomiciliosActuales.Add(pillbox_.GetControlValue(icIdDomicilio))
                                                        End If

                                                    End Sub)

            Dim domiciliosDesdeEmpresa_ As List(Of Rec.Globals.Empresas.Domicilio) = empresaInternacional_.paisesdomicilios.Where(Function(x) x.pais = pais_).
                                                                                                                            SelectMany(Function(y) y.domicilios).ToList()

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
                opcionesLista_.Add(New SelectOption With {
                                .Value = "DOMICILIO NUEVO",
                                .Text = "DOMICILIO NUEVO"
                               })
                Dim modoEditando_ As Boolean = False
                If GetVars("isEditing") IsNot Nothing Then
                    If GetVars("isEditing") = True Then
                        modoEditando_ = True
                    End If
                End If
                SetVars("_opcionesLista", opcionesLista_)
                SetVars("_indice", indice_)
                SetVars("_cveUltimoDomicilio", opcionesLista_.Last.Value)
                If modoEditando_ Then
                    If opcionesLista_.Count > 0 Then
                        lbtitleDomicilios.Visible = True
                        scDomiciliosRegistrados.Visible = True
                    End If
                End If
                scDomiciliosRegistrados.DataSource = opcionesLista_
                scDomiciliosRegistrados.Value = opcionesLista_.Last.Value
            End If
        Else
            ConfigurarDomicilios.Visible = False
            scDomiciliosRegistrados.DataSource = Nothing
            SetVars("_opcionesLista", Nothing)
            SetVars("_indice", Nothing)
            SetVars("_cveUltimoDomicilio", Nothing)
        End If
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
        Dim modoEditando_ As Boolean = False
        If GetVars("isEditing") IsNot Nothing Then
            If GetVars("isEditing") = True Then
                modoEditando_ = True
            End If
        End If
        If fcPaises.Text <> "" Then
            If modoEditando_ Then
                LlenandoTarjetero()
            Else
                If fcRazonSocial.Text <> "" Then
                    If BuscarSiExisterProveedor() = False Then
                        LlenandoTarjetero()
                    End If
                Else
                    MsgValidacionRazonsocialVacio()
                End If
            End If
        Else
            If fcRazonSocial.Text = "" Then
                MsgValidacionRazonsocialVacio()
            End If
            MsgValidacionPais()
        End If
    End Sub

    Protected Sub LlenandoTarjetero()
        Dim modoEditando_ As Boolean = False
        If GetVars("isEditing") IsNot Nothing Then
            If GetVars("isEditing") = True Then
                modoEditando_ = True
            End If
        End If
        icPais.Value = fcPaises.Text
        icCvePais.Value = fcPaises.Text.Substring(0, 3)
        ''VALIDAR QUE SE LLENE ESTE CAMPO, SINO QUIERE DECIR QUE NO EXISTE Y POR LO TANTO ENVIAREMOS UNA INFORMACION
        If fcPaises.Value <> "" Then
            icIdPais.Value = fcPaises.Value
            pbDetalleProveedorInternacional.Enabled = True
            'Dim OKOKOK = scDomiciliosRegistrados.Visible
            'Dim existeValor = scDomiciliosRegistrados.Value
            Dim domicilioSeleccionado_ As String = scDomiciliosRegistrados.Value
            Dim indice_ As Integer = pbDetalleProveedorInternacional.PageIndex
            'Dim domicilioActual_ = scDomiciliosRegistrados.Text
            If modoEditando_ Then
                If indice_ <> 0 Then
                    If domicilioSeleccionado_ = "" Or domicilioSeleccionado_ = "DOMICILIO NUEVO" Then
                        ConfigurarDomicilios.Visible = False
                    Else
                        CambiarDomicilio(domicilioSeleccionado_, indice_)
                        ConfigurarDomicilios.Visible = False
                    End If
                End If
            Else
                If scDomiciliosRegistrados.Visible = True Then
                    If domicilioSeleccionado_ = "" Or domicilioSeleccionado_ = "DOMICILIO NUEVO" Then
                        LimpiarTarjetero()
                    Else
                        domicilioSeleccionado_ = scDomiciliosRegistrados.Value
                        CambiarDomicilio(domicilioSeleccionado_, indice_)
                    End If
                End If
                ConfigurarDomicilios.Visible = False
            End If
        Else
            MsgValidacionPaisValidoControl()
            MsgValidacionPaisValido()
        End If
    End Sub

    'Protected Sub MsgValidacionRazonsocial()
    '    fcRazonSocial.ToolTip = "Debes indicar una razón social. "
    '    fcRazonSocial.ToolTipExpireTime = 4
    '    fcRazonSocial.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
    '    fcRazonSocial.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
    '    fcRazonSocial.ShowToolTip()
    'End Sub

    Protected Sub MsgValidacionPais()
        fcPaises.ToolTip = "Indica un país. "
        fcPaises.ToolTipExpireTime = 4
        fcPaises.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
        fcPaises.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        fcPaises.ShowToolTip()
    End Sub


    'Protected Sub scDomiciliosRegistrados_SelectedIndexChanged(sender As Object, e As EventArgs)
    '    MsgBox("aqui 1")
    'End Sub

    Protected Sub scDomiciliosRegistrados_TextChanged(sender As Object, e As EventArgs)
        'MsgBox("aqui 2")
        'Dim opcionesLista_ = DirectCast(GetVars("_opcionesLista"), List(Of SelectOption))
        'scDomiciliosRegistrados.DataSource = opcionesLista_
        'scDomiciliosRegistrados.Value = opcionesLista_.Last.Value
    End Sub

    Protected Sub scDomiciliosRegistrados_Click(sender As Object, e As EventArgs)
        scDomiciliosRegistrados.Visible = True
        Dim opcionesLista_ = DirectCast(GetVars("_opcionesLista"), List(Of SelectOption))
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
    Public Overrides Function AgregarComponentesBloqueadosInicial() As List(Of WebControl)
        Return New List(Of WebControl) From {icClave, icPais}
    End Function

    Public Overrides Function AgregarComponentesBloqueadosEdicion() As List(Of WebControl)
        Return New List(Of WebControl) From {icClave, fcRazonSocial, icPais}
    End Function

    Private Sub CambiarDomicilio(ByVal cveDomicilio_ As String, Optional ByVal indice_ As Integer = 0)
        Dim tagwatcher_ As New TagWatcher
        Dim cvePais_ As String = fcPaises.Text.Substring(0, 3)
        Dim controladorEmpresas_ As New Rec.Globals.Controllers.Empresas.ControladorEmpresas(New EspacioTrabajo(),
                                                                                             IControladorEmpresas.TiposEmpresas.Internacional,
                                                                                             paisEmpresa_:=cvePais_) With {.ListaEmpresas = New List(Of IEmpresa)}
        Dim empresaInternacional_ As EmpresaInternacional = DirectCast(GetVars("_empresaInternacional"), EmpresaInternacional)
        With controladorEmpresas_
            .Modalidad = IControladorEmpresas.Modalidades.Intrinseca
            .PaisEmpresa = cvePais_
            .ListaEmpresas.Add(empresaInternacional_)
            tagwatcher_ = .ConsultarDomicilio(empresaInternacional_._id, New ObjectId(cveDomicilio_))
        End With
        Try
            If tagwatcher_.Status = TypeStatus.Ok Then
                Dim domicilioSeleccionado = DirectCast(tagwatcher_.ObjectReturned, List(Of Rec.Globals.Empresas.Domicilio))
                pbDetalleProveedorInternacional.Enabled = True
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

    Private Function GuardarEmpresa(ByVal session_ As IClientSessionHandle) As TagWatcher
        Dim tagwatcher_ As New TagWatcher
        Dim empresaInternacional_ As New EmpresaInternacional
        Dim cvePais_ As String = icPais.Value.Substring(0, 3)
        Dim controladorEmpresas_ As New Rec.Globals.Controllers.Empresas.ControladorEmpresas(New EspacioTrabajo(),
                                                                                             IControladorEmpresas.TiposEmpresas.Internacional,
                                                                                             paisEmpresa_:=cvePais_) With {.ListaEmpresas = New List(Of IEmpresa)}
        Dim existeEmpresa_ = False
        If GetVars("_empresaInternacional") IsNot Nothing Then
            empresaInternacional_ = DirectCast(GetVars("_empresaInternacional"), EmpresaInternacional)
            existeEmpresa_ = True
        End If
        Dim existeTaxid_ As Boolean = False
        Dim cveTaxId_ As String = Nothing
        Dim listaDomicilios_ As New List(Of Rec.Globals.Empresas.Domicilio)
        Dim datosAdicionalesActuales_ As New List(Of Dictionary(Of String, String))
        Dim pillboxControl_ As PillboxControl = pbDetalleProveedorInternacional

        pillboxControl_.ForEach(Sub(pillbox_ As PillBox)
                                    Dim domicilio_ As New Rec.Globals.Empresas.Domicilio(pillbox_.GetControlValue(icCalle),
                                                                                                        1,
                                                                                                        pillbox_.GetControlValue(icNumeroExterior),
                                                                                                        pillbox_.GetControlValue(icNumeroInterior),
                                                                                                        pillbox_.GetControlValue(icColonia),
                                                                                                        pillbox_.GetControlValue(icCodigoPostal),
                                                                                                        pillbox_.GetControlValue(icCiudad),
                                                                                                        pillbox_.GetControlValue(icLocalidad),
                                                                                                        pillbox_.GetControlValue(icMunicipio),
                                                                                                        pillbox_.GetControlValue(icEntidadFederativa),
                                                                                                        pais_:=pillbox_.GetControlValue(icPais))
                                    If existeEmpresa_ Then
                                        With empresaInternacional_
                                            ''TAXID
                                            Dim taxidAux_ As String = pillbox_.GetControlValue(icTaxid)
                                            If .taxids IsNot Nothing Then
                                                Dim taxid_ = .taxids.Any(Function(x) x.taxid = taxidAux_)
                                                If taxid_ = False Then
                                                    Dim Tax_ As Rec.Globals.Empresas.TaxId = New Rec.Globals.Empresas.TaxId(taxidAux_, .taxids.Count + 1)
                                                    .taxids.Add(Tax_)
                                                    existeTaxid_ = True
                                                    cveTaxId_ = Tax_.idtaxid.ToString
                                                End If
                                            Else
                                                .taxids = New List(Of Rec.Globals.Empresas.TaxId) From {New Rec.Globals.Empresas.TaxId(taxidAux_)}
                                                existeTaxid_ = True
                                                cveTaxId_ = .taxids.Last.idtaxid.ToString
                                            End If

                                            Dim domicilioPresentacionActual_ = pillbox_.GetControlValue(scDomicilio)
                                            Dim domicilioObtenido_ = domicilio_.domicilioPresentacion

                                            If Not String.Equals(domicilioObtenido_, domicilioPresentacionActual_) Then
                                                Dim domicilioPorPais_ = .paisesdomicilios.Where(Function(x) x.pais = cvePais_).AsEnumerable.ToList

                                                If domicilioPorPais_.Count > 0 Then
                                                    domicilio_.sec = domicilioPorPais_.Last.domicilios.Count + 1
                                                    .paisesdomicilios.Where(Function(x) x.pais = cvePais_).ToList.
                                                                          ForEach(Sub(item_) item_.domicilios.Add(domicilio_))
                                                Else
                                                    Dim totalPaisesDomicilios_ As Integer = .paisesdomicilios.Count + 1
                                                    .paisesdomicilios.Add(New PaisDomicilio(New ObjectId(icIdPais.Value),
                                                                               domicilio_,
                                                                               cvePais_,
                                                                               icPais.Value,
                                                                               totalPaisesDomicilios_))
                                                End If
                                                tagwatcher_ = controladorEmpresas_.Modificar(empresaInternacional_, session_)
                                            Else
                                                domicilio_._iddomicilio = New ObjectId(pillbox_.GetControlValue(icIdDomicilio))
                                                domicilio_.sec = pillbox_.GetControlValue(icSecDomicilio)
                                                cveTaxId_ = pillbox_.GetControlValue(icCveTaxid)
                                            End If
                                        End With
                                    End If
                                    If domicilio_.calle <> "" Then
                                        Dim datosAux_ As New Dictionary(Of String, String) _
                                           From
                                            {
                                                {"cvePais_", cvePais_},
                                                {"objectIdPais_", icIdPais.Value},
                                                {"cvetaxid_", cveTaxId_}
                                            }
                                        listaDomicilios_.Add(domicilio_)
                                        datosAdicionalesActuales_.Add(datosAux_)
                                    End If
                                End Sub)

        If existeEmpresa_ Then
            If existeTaxid_ Then
                tagwatcher_ = controladorEmpresas_.Modificar(empresaInternacional_, session_)
            End If
        Else
            empresaInternacional_ = controladorEmpresas_.EstructuraEmpresaInternacional(fcRazonSocial.Text, listaDomicilios_.Last, icTaxid.Value)
            tagwatcher_ = controladorEmpresas_.Agregar(empresaInternacional_, True, session_)
            datosAdicionalesActuales_ = New List(Of Dictionary(Of String, String))
            Dim datosAux_ As New Dictionary(Of String, String) _
                    From
                    {
                        {"cvePais_", cvePais_},
                        {"objectIdPais_", icIdPais.Value},
                        {"cvetaxid_", empresaInternacional_.taxids.Last.idtaxid.ToString}
                    }
            datosAdicionalesActuales_.Add(datosAux_)
        End If
        SetVars("_empresaInternacional", empresaInternacional_)
        SetVars("_listaDomiciliosActuales", listaDomicilios_)
        SetVars("_datosAdicionalesActuales", datosAdicionalesActuales_)
        If tagwatcher_.Status = TypeStatus.Ok Then
            If tagwatcher_.ObjectReturned IsNot Nothing Then
                empresaInternacional_ = DirectCast(tagwatcher_.ObjectReturned(0), EmpresaInternacional)
                SetVars("_empresaInternacional", empresaInternacional_)
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
        pbDetalleProveedorInternacional.ForEach(Sub(pillbox_ As PillBox)
                                                    ccDomiciliosFiscales.SetRow(Sub(catalogRow_ As CatalogRow)
                                                                                    catalogRow_.SetIndice(ccDomiciliosFiscales.KeyField, i)
                                                                                    catalogRow_.SetColumn(icTaxIDRFC, pillbox_.GetControlValue(icTaxid))
                                                                                    catalogRow_.SetColumn(icDomicilio, pillbox_.GetControlValue(scDomicilio))
                                                                                    catalogRow_.SetColumn(swcArchivarDomicilio, pillbox_.IsFiled())
                                                                                End Sub)
                                                    i += 1
                                                End Sub)
        ccDomiciliosFiscales.CatalogDataBinding()
    End Sub

    'Buscar empresa internacional por razon social
    Private Function ListarEmpresas(Of T)() As List(Of SelectOption)
        Dim lista_ As New List(Of SelectOption)
        Dim tagwatcher_ As New TagWatcher
        Dim controladorEmpresas_ = New Rec.Globals.Controllers.Empresas.ControladorEmpresas(New EspacioTrabajo(),
                                                                                            Rec.Globals.Controllers.Empresas.IControladorEmpresas.TiposEmpresas.Internacional) _
                                                                                            With {.ListaEmpresas = New List(Of IEmpresa)}
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
        Dim empresaInternacional_ As New EmpresaInternacional
        Dim opcionesLista_ As New List(Of SelectOption)
        If GetVars("_empresaInternacional") IsNot Nothing Then
            empresaInternacional_ = DirectCast(GetVars("_empresaInternacional"), EmpresaInternacional)
        End If
        Dim pillboxControl_ As PillboxControl = pbDetalleProveedorInternacional
        Dim domiciliosDesdeEmpresa_ As List(Of Rec.Globals.Empresas.Domicilio) = empresaInternacional_.paisesdomicilios.Last.domicilios
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
            SetVars("_opcionesLista", opcionesLista_)
            SetVars("_indice", indice_)
        End If
    End Sub

    Protected Sub LlenarFormulario(ByVal pais_ As String)
        Dim tagwatcher_ As New TagWatcher
        Dim cvePais_ As String = pais_.Substring(0, 3)
        Dim controladorEmpresas_ As New Rec.Globals.Controllers.Empresas.ControladorEmpresas(New EspacioTrabajo(),
                                                                                             IControladorEmpresas.TiposEmpresas.Internacional,
                                                                                              paisEmpresa_:=cvePais_) _
                                                                                             With {.ListaEmpresas = New List(Of IEmpresa)}
        Dim empresaInternacional_ As New EmpresaInternacional
        Dim ultimoDomicilio_ As New List(Of Rec.Globals.Empresas.Domicilio)
        If GetVars("_listaEmpresasTemporales") IsNot Nothing Then
            Dim listaEmpresasInternacional_ As List(Of EmpresaInternacional) = DirectCast(GetVars("_listaEmpresasTemporales"), List(Of EmpresaInternacional))
            If listaEmpresasInternacional_.Count > 0 Then
                Dim opcionesLista_ As New List(Of SelectOption)
                Dim pillboxControl_ As New PillboxControl
                empresaInternacional_ = DirectCast(listaEmpresasInternacional_.Find(Function(x) x.razonsocial = fcRazonSocial.Text), EmpresaInternacional)
                If empresaInternacional_ IsNot Nothing Then
                    SetVars("_empresaInternacional", empresaInternacional_)
                    controladorEmpresas_.Modalidad = IControladorEmpresas.Modalidades.Intrinseca
                    controladorEmpresas_.PaisEmpresa = cvePais_
                    controladorEmpresas_.ListaEmpresas.Add(empresaInternacional_)
                    tagwatcher_ = controladorEmpresas_.ConsultarDomicilios(empresaInternacional_._id)
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
                                lbtitleDomicilios.Visible = True
                                scDomiciliosRegistrados.Visible = True
                                SetVars("_listaDomicilios", listaDomicilios_)
                                'SetVars("_cveUltimoDomicilio", opcionesLista_.Last.Value)
                                SetVars("_opcionesLista", opcionesLista_)
                                opcionesLista_.Add(New SelectOption With
                                                    {
                                                        .Value = "DOMICILIO NUEVO",
                                                        .Text = "DOMICILIO NUEVO"
                                                    })
                                scDomiciliosRegistrados.DataSource = opcionesLista_
                                'scDomiciliosRegistrados.Value = opcionesLista_.Last.Value
                            End If
                        Else
                            lbtitleDomicilios.Visible = False
                            scDomiciliosRegistrados.Visible = False
                            scDomiciliosRegistrados.DataSource = Nothing
                            SetVars("_opcionesLista", Nothing)
                            SetVars("_listaDomicilios", Nothing)
                        End If
                    End If
                Else
                    lbtitleDomicilios.Visible = False
                    scDomiciliosRegistrados.Visible = False
                    scDomiciliosRegistrados.DataSource = Nothing
                    SetVars("_opcionesLista", Nothing)
                    SetVars("_listaEmpresasTemporales", Nothing)
                End If
            Else
                    lbtitleDomicilios.Visible = False
                    scDomiciliosRegistrados.Visible = False
                    scDomiciliosRegistrados.DataSource = Nothing
                    SetVars("_opcionesLista", Nothing)
                    SetVars("_listaEmpresasTemporales", Nothing)
                End If
            Else
            SetVars("_listaEmpresasTemporales", Nothing)
            SetVars("_opcionesLista", Nothing)
            scDomiciliosRegistrados.DataSource = Nothing
            lbtitleDomicilios.Visible = False
            scDomiciliosRegistrados.Visible = False
        End If
    End Sub

    Sub LlenarTarjetero(ByVal domicilio_ As List(Of Rec.Globals.Empresas.Domicilio),
                        ByVal indice_ As Integer)
        Dim pillboxControl_ As PillboxControl = pbDetalleProveedorInternacional
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
                                           scDomicilio.Value = item_.domicilioPresentacion
                                       End Sub)
        Next
    End Sub

    Sub CargarTarjetero(ByVal domicilio_ As List(Of Rec.Globals.Empresas.Domicilio),
                        Optional ByVal indice_ As Integer = 0)
        Dim empresaInternacional_ As New EmpresaInternacional
        If GetVars("_empresaInternacional") IsNot Nothing Then
            empresaInternacional_ = DirectCast(GetVars("_empresaInternacional"), EmpresaInternacional)
        End If
        Dim cvePais_ As String = fcPaises.Text.Substring(0, 3)
        Dim domicilioPorPais_ = empresaInternacional_.paisesdomicilios.Where(Function(x) x.pais = cvePais_).AsEnumerable.ToList
        Dim pillboxControl_ As PillboxControl = pbDetalleProveedorInternacional
        pillboxControl_.ClearRows()
        For Each item_ In domicilio_
            pillboxControl_.SetPillbox(Sub(pillbox_ As PillBox)
                                           pillbox_.SetControlValue(icTaxid, empresaInternacional_.taxids.Last.taxid)
                                           pillbox_.SetControlValue(icCveTaxid, empresaInternacional_.taxids.Last.idtaxid.ToString)
                                           pillbox_.SetControlValue(icIdPais, empresaInternacional_.paisesdomicilios.Last.idpais.ToString)
                                           pillbox_.SetControlValue(icCvePais, empresaInternacional_.paisesdomicilios.Last.pais)
                                           pillbox_.SetControlValue(icPais, empresaInternacional_.paisesdomicilios.Last.paisPresentacion)
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
        pbDetalleProveedorInternacional = pillboxControl_
        pbDetalleProveedorInternacional.PillBoxDataBinding()
        SetVars("_listaDomicilios", pbDetalleProveedorInternacional.DataSource)
    End Sub

    Sub LimpiarTarjetero()
        Dim pillboxControl_ As PillboxControl = pbDetalleProveedorInternacional
        pillboxControl_.ClearRows()
        pillboxControl_.SetPillbox(Sub(pillbox_ As PillBox)
                                       pillbox_.SetIndice(pillboxControl_.KeyField, 1)
                                       pillbox_.SetFiled(False)
                                       pillbox_.SetControlValue(icTaxid, Nothing)
                                       pillbox_.SetControlValue(icCveTaxid, Nothing)
                                       pillbox_.SetControlValue(icPais, Nothing)
                                       pillbox_.SetControlValue(icIdPais, Nothing)
                                       pillbox_.SetControlValue(icCvePais, Nothing)
                                       pillbox_.SetControlValue(icCalle, Nothing)
                                       pillbox_.SetControlValue(icNumeroExterior, Nothing)
                                       pillbox_.SetControlValue(icNumeroInterior, Nothing)
                                       pillbox_.SetControlValue(icCodigoPostal, Nothing)
                                       pillbox_.SetControlValue(icColonia, Nothing)
                                       pillbox_.SetControlValue(icLocalidad, Nothing)
                                       pillbox_.SetControlValue(icCiudad, Nothing)
                                       pillbox_.SetControlValue(icMunicipio, Nothing)
                                       pillbox_.SetControlValue(icEntidadFederativa, Nothing)
                                       pillbox_.SetControlValue(icIdDomicilio, Nothing)
                                       pillbox_.SetControlValue(icSecDomicilio, Nothing)
                                       pillbox_.SetControlValue(scDomicilio, Nothing)
                                       pillbox_.SetControlValue(icNumeroExtInt, Nothing)
                                       pillbox_.SetControlValue(icCveMunicipio, Nothing)
                                       pillbox_.SetControlValue(icCveEntidadFederativa, Nothing)
                                   End Sub)
        pbDetalleProveedorInternacional = pillboxControl_
        pbDetalleProveedorInternacional.PillBoxDataBinding()
        SetVars("_listaDomicilios", Nothing)
    End Sub

    Protected Sub RegresarControlesPorDefault()
        Dim modoEditando_ As Boolean = False
        If GetVars("isEditing") IsNot Nothing Then
            If GetVars("isEditing") = True Then
                modoEditando_ = True
            End If
        End If

        If pbDetalleProveedorInternacional.PageIndex > 0 Then
            lbNumero.Text = pbDetalleProveedorInternacional.PageIndex.ToString()
        End If

        Session("_tbDetalleProveedor") = PillboxControl.ToolbarModality.Default : pbDetalleProveedorInternacional.Modality = Session("_tbDetalleProveedor")

        If modoEditando_ Then

            PreparaTarjetero(PillboxControl.ToolbarModality.Advanced, pbDetalleProveedorInternacional)
            fsDatosGenerales.Enabled = True
            fsVinculaciones.Enabled = True
            fsConfiguracionAdicional.Enabled = True
            fsHistorialDomicilios.Enabled = True
        Else
            PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbDetalleProveedorInternacional)
            swcDestinatario.Checked = True
            fsVinculaciones.Enabled = False
            fsConfiguracionAdicional.Enabled = False
            fsHistorialDomicilios.Enabled = False

        End If
        ConfigurarDomicilios.Visible = False
        scDomiciliosRegistrados.DataSource = Nothing
        fsHistorialDomicilios.Visible = True
        fsVinculaciones.Visible = True
        fsConfiguracionAdicional.Visible = True
        CargarHistorialDomicilios()

    End Sub

    Protected Function GenerarSecuencia() As Secuencia
        Dim secuencia_ As New Secuencia
        Dim controladorSecuencias_ = New ControladorSecuencia
        Dim tagwatcher_ As TagWatcher = controladorSecuencias_.Generar(SecuenciasComercioExterior.ProveedoresOperativos.ToString, 1, 1, 1, 1, 1)
        If tagwatcher_.Status = TypeStatus.Ok Then
            secuencia_ = DirectCast(tagwatcher_.ObjectReturned, Secuencia)
            secuencia_.nombre = SecuenciasComercioExterior.ProveedoresOperativos.ToString
        End If
        Return secuencia_
    End Function

    Protected Sub MsgValidacionRazonsocial()
        fcRazonSocial.ToolTip = "Proveedor ya registrado. "
        'fcRazonSocial.ToolTipExpireTime = 4
        fcRazonSocial.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
        fcRazonSocial.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        fcRazonSocial.ShowToolTip()
    End Sub

    Protected Sub MsgValidacionRazonsocialVacio()
        fcRazonSocial.ToolTip = "Indica una razón social. "
        'fcRazonSocial.ToolTipExpireTime = 4
        fcRazonSocial.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
        fcRazonSocial.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        fcRazonSocial.ShowToolTip()
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

    Protected Sub MsgValidacionPaisValido()
        icPais.ToolTip = "País no válido."
        icPais.ToolTipExpireTime = 4
        icPais.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
        icPais.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        icPais.ShowToolTip()
    End Sub

    Protected Sub MsgValidacionPaisValidoControl()
        fcPaises.ToolTip = "País no válido."
        fcPaises.ToolTipExpireTime = 4
        fcPaises.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
        fcPaises.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        fcPaises.ShowToolTip()
    End Sub







#End Region
End Class