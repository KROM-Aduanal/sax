
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
'Imports Rec.Globals.Empresa
'Imports Rec.Globals.Controllers

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



#End Region

Public Class Ges022_001_RegistroProveedores
    Inherits ControladorBackend

#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Private _controladorEmpresas As Rec.Globals.Controllers.Empresas.IControladorEmpresas

    Private _espacioTrabajo As IEspacioTrabajo

    ' Private _controladorViejito As ControladorEmpresas

    Private _empresaNacional As Rec.Globals.Empresas.EmpresaNacional
    Private _empresaInternacional As Rec.Globals.Empresas.EmpresaInternacional

    Private _secuencia As ISecuencia

    Private _listaEmpresasNacionales As List(Of EmpresaNacional)
    Private _listaEmpresasInternacionales As List(Of EmpresaInternacional)

    Private _cantidadDetalles As Int32

    Private _urlType As Boolean = False
    Private _tagwatcher As TagWatcher


    Private _paisDomicilios As List(Of PaisDomicilio)
    Private _listaDomicilios, _ultimoDomicilio As List(Of Rec.Globals.Empresas.Domicilio)
    Private _opcionesLista As List(Of SelectOption)

    Private _pillboxControl As PillboxControl

    Private _controladorSecuencias As IControladorSecuencia

#End Region

#Region "██████ Vinculación c/capas inf █████████       SAX      ████████████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Public Overrides Sub Inicializa()

        Dim _url = IIf(Request.QueryString("Type") IsNot Nothing, DirectCast(Request.QueryString("Type"), String), False)

        _urlType = CBool(_url)

        InicializarControles()

        If _urlType Then

            With Buscador

                .DataObject = New ConstructorProveedoresOperativos(True)
                .addFilter(SeccionesProvedorOperativo.SPRO1, CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, "Proveedor")
                .addFilter(SeccionesProvedorOperativo.SPRO2, CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR, "TaxID")
                .addFilter(SeccionesProvedorOperativo.SPRO2, CamposDomicilio.CA_CVE_PAIS, "País")

            End With

            __SYSTEM_MODULE_FORM.Label = "Proveedores Extranjeros"

            fbcPais.Visible = True
            fbcPais.Enabled = False
            swcTipoUso.Checked = True
            swcTipoPersona.Visible = False
            icTaxId.Visible = True
            icRFC.Visible = False

            scTaxIdVinculacion.Label = "TaxId"
            scTaxIdConfiguracion.Label = "TaxId"
            icTaxIDRFC.Label = "TaxId"

        Else

            With Buscador

                .DataObject = New ConstructorProveedoresOperativos(True)
                .addFilter(SeccionesProvedorOperativo.SPRO1, CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, "Proveedor")
                .addFilter(SeccionesProvedorOperativo.SPRO2, CamposProveedorOperativo.CA_RFC_PROVEEDOR, "RFC")

            End With

            __SYSTEM_MODULE_FORM.Label = "Proveedores Nacionales"

            fbcPais.Visible = False
            swcTipoUso.Checked = False
            swcTipoPersona.Visible = True
            icTaxId.Visible = False
            icRFC.Visible = True

            scTaxIdVinculacion.Label = "RFC"
            scTaxIdConfiguracion.Label = "RFC"
            icTaxIDRFC.Label = "RFC"

        End If

        scIncoterm.DataEntity = New krom.Anexo22()
        scMetodoValoracion.DataEntity = New krom.Anexo22()
        swcTipoUso.Enabled = False

        If OperacionGenerica IsNot Nothing Then

            _cantidadDetalles = OperacionGenerica.
                                Borrador.
                                Folder.
                                ArchivoPrincipal.
                                Dupla.
                                Fuente.
                                Seccion(SeccionesProvedorOperativo.SPRO2).
                                CantidadPartidas
        End If

    End Sub

    Protected Sub InicializarControladoresNacionales()

        _espacioTrabajo = New EspacioTrabajo

        _espacioTrabajo = HttpContext.Current.Session("EspacioTrabajoExtranet")

        _controladorEmpresas = New Rec.Globals.Controllers.Empresas.ControladorEmpresas(_espacioTrabajo,
                                                       Rec.Globals.Controllers.Empresas.IControladorEmpresas.TiposEmpresas.Nacional)

        _listaEmpresasNacionales = New List(Of EmpresaNacional)

        _empresaNacional = New EmpresaNacional

        _tagwatcher = New TagWatcher


        InicializarCamposPais()

    End Sub

    Protected Sub InicializarControladoresInternacionales()

        _espacioTrabajo = New EspacioTrabajo

        _espacioTrabajo = HttpContext.Current.Session("EspacioTrabajoExtranet")

        _controladorEmpresas = New Rec.Globals.Controllers.Empresas.ControladorEmpresas(_espacioTrabajo,
                                                       Rec.Globals.Controllers.Empresas.IControladorEmpresas.TiposEmpresas.Internacional)

        _controladorEmpresas.PaisEmpresa = icCvePais.Value

        _empresaInternacional = New EmpresaInternacional

        _listaEmpresasInternacionales = New List(Of EmpresaInternacional)

        _tagwatcher = New TagWatcher

    End Sub

    Protected Sub InicializarControles()

        If _urlType Then

            InicializarControladoresInternacionales()

        Else

            InicializarControladoresNacionales()

        End If

    End Sub

    Public Overrides Sub BotoneraClicNuevo()

        If OperacionGenerica IsNot Nothing Then

            _controladorEmpresas = Nothing
            _empresaNacional = Nothing
            _empresaInternacional = Nothing
            _listaEmpresasNacionales = Nothing
            _listaEmpresasInternacionales = Nothing
            _paisDomicilios = Nothing
            _listaDomicilios = Nothing

        End If

        LimpiaSesion()

        PreparaTarjetero(PillboxControl.ToolbarModality.Simple, pbDetalleProveedor)

        fsConfiguracionDomicilio.Visible = True
        fsConfiguracionDomicilio.Enabled = True

        fsVinculaciones.Visible = False
        fsConfiguracionAdicional.Visible = False
        fsHistorialDomicilios.Visible = False

        If pbDetalleProveedor.PageIndex > 0 Then

            lbNumero.Text = pbDetalleProveedor.PageIndex.ToString()
            CamposBloqueadosDefault()

        End If

        fsDatosGenerales.Enabled = True
        icClave.Enabled = False
        swcTipoUso.Enabled = False
        fcRazonSocial.Enabled = True

        fbcPais.Enabled = False

    End Sub

    Public Overrides Sub BotoneraClicGuardar()

        If Not ProcesarTransaccion(Of ConstructorProveedoresOperativos)().Status = TypeStatus.Errors Then : End If

    End Sub

    Public Overrides Sub BotoneraClicEditar()

        fsConfiguracionDomicilio.Visible = True

        fsConfiguracionDomicilio.Enabled = True

        scDomicilios.Enabled = True
        swcEditarDomicilio.Enabled = True
        swcEditarDomicilio.Checked = False

        Session("_tbDetalleProveedor") = PillboxControl.ToolbarModality.Advanced : pbDetalleProveedor.Modality = Session("_tbDetalleProveedor")

        PreparaTarjetero(PillboxControl.ToolbarModality.Advanced, pbDetalleProveedor)

    End Sub

    Public Overrides Sub BotoneraClicBorrar()

        'swcUtilizarDatos.Checked = True : VerificaCheckDomicilio()

        'swcEditarDomicilio.Enabled = False
        'swcEditarDomicilio.Visible = False

    End Sub

    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher

        'Datos generales (SeccionesProveedorOperativo.SPRO1)
        [Set](icClave, CP_CVE_PROVEEDOR)
        [Set](scProveedor, CP_SECUENCIA_PROVEEDOR)
        [Set](cveEmpresa, CP_CVE_EMPRESA)
        [Set](idEmpresa, CP_ID_EMPRESA)
        [Set](fcRazonSocial, CA_RAZON_SOCIAL_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Text)

        If _urlType Then

            [Set](IIf(swcTipoUso.Checked, 0, 1), CP_TIPO_USO, tipoDato_:=TiposDato.Texto)

        Else

            [Set](IIf(swcTipoUso.Checked, 1, 0), CP_TIPO_USO, tipoDato_:=TiposDato.Texto)

        End If

        'Detalle proveedor
        If pbDetalleProveedor.PageIndex > 0 Then

            lbNumero.Text = pbDetalleProveedor.PageIndex.ToString()

        End If

        [Set](icIdDomicilio, CP_ID_DOMICILIO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icSecDomicilio, CP_SEC_DOMICILIO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCvePais, CA_CVE_PAIS, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPais, CA_PAIS, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icIdPais, CA_ID_PAIS, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](swcDestinatario, CP_DESTINATARIO_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

        If _urlType Then

            [Set](icTaxId, CA_TAX_ID_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
            [Set](icCveTaxId, CA_CVE_TAX_ID_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        Else

            [Set](swcTipoPersona, CP_TIPO_PERSONA_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

            [Set](icRFC, CA_RFC_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
            [Set](icCveRfc, CA_CVE_RFC_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

            If swcTipoPersona.Checked Then

                [Set](icCURP, CA_CURP_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
                [Set](icCveCurp, CA_CVE_CURP_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

            End If

        End If

        [Set](scDomicilio, CamposDomicilio.CA_DOMICILIO_FISCAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCalle, CA_CALLE, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icNumeroExterior, CA_NUMERO_EXTERIOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icNumeroInterior, CA_NUMERO_INTERIOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icNumeroExtInt, CA_NUMERO_EXT_INT, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCodigoPostal, CA_CODIGO_POSTAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icColonia, CA_COLONIA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icLocalidad, CA_LOCALIDAD, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCiudad, CA_CIUDAD, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCveMunicipio, CA_ENTIDAD_MUNICIPIO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icMunicipio, CA_MUNICIPIO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCveEntidadFederativa, CA_CVE_ENTIDAD_FEDERATIVA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icEntidadFederativa, CA_ENTIDAD_FEDERATIVA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scDomicilios, CamposDomicilio.CA_DOMICILIO_FISCAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](pbDetalleProveedor, Nothing, seccion_:=SeccionesProvedorOperativo.SPRO2)

        ' Vinculaciones con clientes (SeccionesProvedorOperativo.SPRO4)
        [Set](scClienteVinculacion, CP_ID_CLIENTE_VINCULACION, propiedadDelControl_:=PropiedadesControl.Ninguno)

        If _urlType Then

            [Set](scTaxIdVinculacion, CP_TAX_ID_VINCULACION, propiedadDelControl_:=PropiedadesControl.Ninguno)

        Else

            [Set](scTaxIdVinculacion, CP_RFC_PROVEEDOR_VINCULACION, propiedadDelControl_:=PropiedadesControl.Ninguno)

        End If

        [Set](scVinculacion, CA_CVE_VINCULACION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPorcentajeVinculacion, CP_PORCENTAJE_VINCULACION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ccVinculaciones, Nothing, seccion_:=SeccionesProvedorOperativo.SPRO4)

        'Configuración adicional (SeccionesProvedorOperativo.SPRO5)
        [Set](scClienteConfiguracion, CP_ID_CLIENTE_CONFIGURACION, propiedadDelControl_:=PropiedadesControl.Ninguno)

        If _urlType Then

            [Set](scTaxIdConfiguracion, CP_TAX_ID_CONFIGURACION, propiedadDelControl_:=PropiedadesControl.Ninguno)

        Else

            [Set](scTaxIdConfiguracion, CP_RFC_PROVEEDOR_CONFIGURACION, propiedadDelControl_:=PropiedadesControl.Ninguno)

        End If

        [Set](scMetodoValoracion, CA_CVE_METODO_VALORACION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scIncoterm, CA_CVE_INCOTERM, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ccConfiguracionAdicional, Nothing, seccion_:=SeccionesProvedorOperativo.SPRO5)

        Return New TagWatcher(1)

    End Function

    'EVENTOS PARA LA INSERCIÓN DE DATOS
    Public Overrides Function AntesRealizarInsercion(ByVal session_ _
                                                     As IClientSessionHandle) _
                                                     As TagWatcher
        _tagwatcher = New TagWatcher

        With _tagwatcher

            If session_ IsNot Nothing Then

                If _urlType Then

                    GenerarEmpresaInternacional()

                Else

                    GenerarEmpresaNacional()

                End If

            Else

                .SetOK()

            End If

        End With

        Return _tagwatcher

    End Function

    Public Overrides Sub RealizarInsercion(ByRef documentoElectronico_ As DocumentoElectronico)

        Dim empresa_ = GetVars("_empresa")

        Dim sec_ = GenerarSecuenciaInterna()

        icClave.Value = sec_.sec

        scProveedor.Value = sec_.sec

        cveEmpresa.Value = empresa_._idempresa

        idEmpresa.Value = empresa_._id.ToString

        With documentoElectronico_

            If swcTipoUso.Checked Then

                [Set](1, CP_TIPO_USO, tipoDato_:=TiposDato.Texto)

                [Set](swcTipoUso, CP_TIPO_USO, propiedadDelControl_:=PropiedadesControl.OnText, asignarA_:=TiposAsignacion.ValorPresentacion)

            Else

                [Set](0, CP_TIPO_USO, tipoDato_:=TiposDato.Texto)

                [Set](swcTipoUso, CP_TIPO_USO, propiedadDelControl_:=PropiedadesControl.OffText, asignarA_:=TiposAsignacion.ValorPresentacion)

            End If

            .Id = sec_._id.ToString

            .IdDocumento = sec_.sec

            .FolioDocumento = empresa_._idempresa

            .FolioOperacion = sec_.sec

            .TipoPropietario = "ProveedoresOperativos"

            .NombrePropietario = empresa_.razonsocial

            .IdPropietario = empresa_._idempresa

            .ObjectIdPropietario = empresa_._id


            ''CAMBIE ESTA FORMA, PORQUE NO ME SABÍA COMO SE HACE CON LA NUEVA FORMA DE AGREGAR METADATOS:V
            .Metadatos = New List(Of CampoGenerico) _
                             From { .Attribute(CamposProveedorOperativo.CP_TIPO_USO)}

        End With

    End Sub

    Public Overrides Function DespuesRealizarInsercion() As TagWatcher

        CargarHistorialDomicilios()
        fsHistorialDomicilios.Visible = True
        fsVinculaciones.Visible = True
        fsConfiguracionAdicional.Visible = True

        ' fsConfiguracionDomicilio.Enabled

        'swcEditarDomicilio.Checked = False
        'VerificaCheckDomicilio(4)


        Return New TagWatcher(Ok)

    End Function

    'EVENTOS PARA MODIFICACIÓN DE DATOS
    Public Overrides Function AntesRealizarModificacion(ByVal session_ As IClientSessionHandle) _
                                                        As TagWatcher
        _tagwatcher = New TagWatcher

        With _tagwatcher

            If session_ IsNot Nothing Then '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 

                '''aqui iguals haz un barril

                If _urlType Then

                    GenerarEmpresaInternacional()

                Else

                    GenerarEmpresaNacional()

                End If

            Else  '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas sin transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 

                .SetOK()

            End If

        End With

        Return _tagwatcher

    End Function


    Public Overrides Sub RealizarModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

        _tagwatcher = New TagWatcher

        Dim empresa_ = GetVars("_empresa")

        With documentoElectronico_

            If GetVars("_empresaNueva") Then

                Dim sec_ = GenerarSecuenciaInterna(2)

                icClave.Value = sec_.sec

                scProveedor.Value = sec_.sec

                .Id = sec_._id.ToString

                .IdDocumento = sec_.sec

                .FolioDocumento = empresa_._idempresa

                .FolioOperacion = sec_.sec

                .TipoPropietario = "ProveedoresOperativos"

                .NombrePropietario = empresa_.razonsocial

                .IdPropietario = empresa_._idempresa

                .ObjectIdPropietario = empresa_._id

                ''CAMBIE ESTA FORMA, NO ME SABÍA LA NUEVA :V
                .Metadatos = New List(Of CampoGenerico) _
                    From { .Attribute(CamposProveedorOperativo.CP_TIPO_USO)}

            End If

        End With

    End Sub

    Public Overrides Function DespuesRealizarModificacion() As TagWatcher

        CargarHistorialDomicilios()
        fsHistorialDomicilios.Visible = True
        fsVinculaciones.Visible = True
        fsConfiguracionAdicional.Visible = True

        ' fsConfiguracionDomicilio.Enabled

        'swcEditarDomicilio.Checked = False
        'VerificaCheckDomicilio(4)


        Return New TagWatcher(Ok)

    End Function

    'EVENTOS PARA PRESENTACIÓN DE DATOS EN FRONTEND
    Public Overrides Sub PreparaModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

        InicializarControles()

        If documentoElectronico_ IsNot Nothing Then

            _cantidadDetalles = documentoElectronico_.Seccion(SeccionesProvedorOperativo.SPRO2).CantidadPartidas

        End If

        _tagwatcher = _controladorEmpresas.ConsultarUna(documentoElectronico_.Attribute(CamposProveedorOperativo.CP_ID_EMPRESA).Valor)

        With _tagwatcher

            If .Status = TypeStatus.Ok Then

                If _urlType Then

                    _empresaInternacional = DirectCast(.ObjectReturned, EmpresaInternacional)

                    SetVars("_empresa", _empresaInternacional)

                Else

                    _empresaNacional = DirectCast(.ObjectReturned, EmpresaNacional)

                    SetVars("_empresa", _empresaNacional)

                End If

            End If

        End With

        CargarHistorialDomicilios()

    End Sub

    Public Overrides Sub DespuesBuquedaGeneralConDatos()

        'swcUtilizarDatos.Checked = True

        'VerificaCheckDomicilio(4)
        If _urlType Then

            fbcPais.Text = icPais.Value
            fbcPais.Value = icIdPais.Value
            _controladorEmpresas.PaisEmpresa = icCvePais.Value

        End If

        fsVinculaciones.Visible = IIf(_cantidadDetalles > 0, True, False)
        fsConfiguracionAdicional.Visible = IIf(_cantidadDetalles > 0, True, False)
        fsHistorialDomicilios.Visible = IIf(_cantidadDetalles > 0, True, False)
        PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbDetalleProveedor)

    End Sub

    Public Overrides Sub DespuesBuquedaGeneralSinDatos()

        'Session("_tbDetalleProveedor") = PillboxControl.ToolbarModality.Default : pbDetalleProveedor.Modality = Session("_tbDetalleProveedor")
        PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbDetalleProveedor)
        pbDetalleProveedor.Enabled = True

    End Sub

    'EVENTOS DE MANTENIMIENTO
    Public Overrides Sub LimpiaSesion()

        SetVars("_listaEmpresasTemporales", Nothing)
        SetVars("_empresa", Nothing)
        SetVars("_listaDomicilios", Nothing)
        SetVars("_empresaNueva", Nothing)

    End Sub

    Public Overrides Sub Limpiar()

        ccDomiciliosFiscales.DataSource = Nothing

        ccVinculaciones.DataSource = Nothing

        ccConfiguracionAdicional.DataSource = Nothing

        _controladorEmpresas.ReiniciarControlador()

        _empresaInternacional = Nothing

        _empresaNacional = Nothing

        _listaDomicilios = Nothing

        _espacioTrabajo = Nothing

        _secuencia = Nothing

        _listaEmpresasNacionales = Nothing

        _listaEmpresasInternacionales = Nothing

        ''ONDE TE USO
        _cantidadDetalles = Nothing

        _tagwatcher = Nothing

        _paisDomicilios = Nothing

        _opcionesLista = Nothing

        _pillboxControl = Nothing

        ''VIEWSTATE

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
            scDomicilios.DataSource = Nothing

            If GetVars("isEditing") = False Then

                If GetVars("_empresas") IsNot Nothing Or
                    GetVars("_empresas") Then

                    If GetVars("_empresas").Count >= 1 Or
                        GetVars("_empresas").Count >= 1 Then

                        'VerificaCheckDomicilio(4)

                    Else

                        'swcUtilizarDatos.Checked = False
                        'swcUtilizarDatos.Enabled = False
                        scDomicilios.Enabled = False
                        swcEditarDomicilio.Enabled = False

                    End If

                End If
            Else

                Select Case opcion_

                    Case 1 'Solo mover tarjeta
                        'VerificaCheckDomicilio(4)
                        'swcUtilizarDatos.Checked = True
                        scDomicilios.Enabled = False
                        swcEditarDomicilio.Enabled = True
                        icTaxId.Enabled = icTaxId.Value Is ""
                        icRFC.Enabled = icRFC.Value Is ""
                        icCURP.Enabled = icCURP.Value Is ""

                    Case 2 'Cuando es nuevo o copiar
                        ' swcUtilizarDatos.Checked = False
                        scDomicilios.Enabled = False
                        swcEditarDomicilio.Enabled = False
                        ' VerificaCheckDomicilio(5)

                End Select

            End If

        End If

    End Sub

    'EVENTOS PARA LA RAZON SOCIAL
    Protected Sub fcRazonSocial_TextChanged(sender As Object, e As EventArgs)

        ''haz un barril
        If _urlType Then


            fcRazonSocial.DataSource = ListarEmpresas(Of EmpresaInternacional)()

        Else

            fcRazonSocial.DataSource = ListarEmpresas(Of EmpresaNacional)()

        End If

        CamposBloqueadosDefault()

    End Sub

    Protected Sub fcRazonSocial_Click(sender As Object, e As EventArgs)

        '''_urlType en true es INTERNACIONAL barril

        If _urlType Then

            fbcPais.Enabled = True

            CargarCamposInternacionales()

        Else

            CargarCamposNacionales()

        End If

    End Sub

    'EVENTOS PARA CONTROLAR QUE PASA CON EL TIPO DE USO
    Protected Sub swcTipoUso_CheckedChanged(sender As Object, e As EventArgs)

        VerificaCheckTipoUso()

    End Sub

    'EVENTOS PARA CONTROLAR QUE PASA CON EL TIPO DE PERSONA
    Protected Sub swcTipoPersona_CheckedChanged(sender As Object, e As EventArgs)

        If swcTipoPersona.Checked Then

            icCURP.Visible = True

        Else

            icCURP.Visible = False

        End If

    End Sub

    Protected Sub swcDestinatario_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    'EVENTO PARA EL RFC
    Protected Sub icRFC_GotFocus(sender As Object, e As EventArgs)

    End Sub

    Protected Sub icRFC_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Sub VerificaCheckTipoUso(Optional ByVal opcion_ As Int32 = 1)

        'Select Case opcion_

        '    Case 1 'Defaults

        '        If Not swcTipoUso.Checked Then
        '            icTaxId.Enabled = True
        '            icTaxId.Visible = True
        '            icRFC.Visible = False
        '            icRFC.Enabled = False
        '            icCURP.Visible = False
        '            icCURP.Enabled = False
        '        Else
        '            icTaxId.Visible = False
        '            icTaxId.Enabled = False
        '            icRFC.Visible = True
        '            icRFC.Enabled = True
        '            icCURP.Visible = True
        '            icCURP.Enabled = True
        '        End If

        '    Case 2

        '        If swcTipoUso.Checked Then
        '            icTaxId.Enabled = False
        '            icTaxId.Visible = False
        '            icRFC.Visible = True
        '            icRFC.Enabled = True
        '            icCURP.Visible = True
        '            icCURP.Enabled = True

        '        Else
        '            icTaxId.Visible = True
        '            icTaxId.Enabled = True
        '            icRFC.Visible = False
        '            icRFC.Enabled = False
        '            icCURP.Visible = False
        '            icCURP.Enabled = False

        '        End If

        'End Select

    End Sub

    'EVENTOS PARA CONTROLAR QUE PASA CON LA EDICIÓN DE DOMICILIO
    Protected Sub swcEditarDomicilio_CheckedChanged(sender As Object, e As EventArgs)

        'If swcEditarDomicilio.Checked Then

        'Hab'ilitarDomicilio()

        ' Else

        ' CamposBloqueadosDefault()

        ' End If

        ' GuardaCamposDomicilio()

    End Sub


    'EVENTOS PARA CARGAR LOS CLIENTES EN LAS LISTAS
    Protected Sub scClienteVinculacion_Click(sender As Object, e As EventArgs)

        scClienteVinculacion.DataSource = CargaTopClientes()

    End Sub

    Protected Sub scClienteVinculacion_TextChanged(sender As Object, e As EventArgs)

        Dim controlador_ As New ControladorBusqueda(Of ConstructorCliente)

        Dim lista_ As List(Of SelectOption) = controlador_.Buscar(scClienteVinculacion.SuggestedText,
                                                                  New Filtro With {.IdSeccion = SeccionesClientes.SCS1, .IdCampo = CamposClientes.CA_RAZON_SOCIAL})

        scClienteVinculacion.DataSource = lista_

    End Sub

    Protected Sub scClienteConfiguracion_Click(sender As Object, e As EventArgs)

        scClienteConfiguracion.DataSource = CargaTopClientes()

    End Sub

    Protected Sub scClienteConfiguracion_TextChanged(sender As Object, e As EventArgs)

        Dim controlador_ As New ControladorBusqueda(Of ConstructorCliente)

        Dim lista_ As List(Of SelectOption) = controlador_.Buscar(scClienteConfiguracion.SuggestedText,
                                                                  New Filtro With {.IdSeccion = SeccionesClientes.SCS1, .IdCampo = CamposClientes.CA_RAZON_SOCIAL})

        scClienteConfiguracion.DataSource = lista_

    End Sub

    'EVENTOS PARA CARGAR LOS TAX-ID/RFC 
    Protected Sub scTaxIdVinculacion_Click(sender As Object, e As EventArgs)

        scTaxIdVinculacion.DataSource = IdentificadoresProveedor()

    End Sub

    Private Function IdentificadoresProveedor() As List(Of SelectOption)

        Dim listaIdentificadores_ As New List(Of SelectOption)
        pbDetalleProveedor.ForEach(Sub(pillbox_ As PillBox)

                                       listaIdentificadores_.Add(New SelectOption With {.Text = pillbox_.GetControlValue(icRFC),
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

        RegresarControles(2)

    End Sub

    'EVENTOS PARA CARGAR LOS CLIENTES
    Protected Sub fbcPais_TextChanged(sender As Object, e As EventArgs)

        fbcPais.DataSource = cargarListadoPaises()

    End Sub

    Protected Sub fbcPais_Click(sender As Object, e As EventArgs)

        icPais.Value = fbcPais.Text
        icCvePais.Value = fbcPais.Text.Substring(0, 3)
        icIdPais.Value = fbcPais.Value.ToString

    End Sub

    Protected Sub DomiciliosPorPaisExtranjero()

        InicializarControles()

        _empresaInternacional = GetVars("_empresa")

        With _controladorEmpresas

            .Modalidad = IControladorEmpresas.Modalidades.Intrinseca

            .TipoEmpresa = IControladorEmpresas.TiposEmpresas.Internacional

            .ListaEmpresas.Add(_empresaInternacional)

            .PaisEmpresa = icCvePais.Value

            _tagwatcher = .ConsultarDomicilios(_empresaInternacional._id)

            If _tagwatcher.Status = TypeStatus.Ok Then

                _listaDomicilios = DirectCast(_tagwatcher.ObjectReturned, List(Of Rec.Globals.Empresas.Domicilio))

                SetVars("_listaDomicilios", _listaDomicilios)

                fsConfiguracionDomicilio.Enabled = True

            Else


                SetVars("_listaDomicilios", Nothing)

            End If

        End With

    End Sub


    Protected Sub scDomicilios_SelectedIndexChanged(sender As Object, e As EventArgs)
        'Aqui vamos a determinar si vamos por los domiclios a mongo, que yo creo que si, woa a ver


    End Sub

    Protected Sub scDomicilios_Click(sender As Object, e As EventArgs)
        ''Aqui se deben de llenar los campos con la nueva selección
        scDomicilio.Value = scDomicilios.Text

    End Sub


    Protected Sub icMunicipio_TextChanged(sender As Object, e As EventArgs)

        icCveMunicipio.Value = icMunicipio.Value.Substring(0, 3)

    End Sub


    Protected Sub icEntidadFederativa_TextChanged(sender As Object, e As EventArgs)

        icCveEntidadFederativa.Value = icEntidadFederativa.Value.Substring(0, 3)

    End Sub

    Protected Sub swcUtilizarDatos_CheckedChanged(sender As Object, e As EventArgs)

        'If swcUtilizarDatos.Checked = True Then

        '    swcEditarDomicilio.Visible = True
        '    swcEditarDomicilio.Enabled = True
        '    'scDomicilios.Enabled = True

        '    If swcEditarDomicilio.Checked Then

        '        VerificaCheckDomicilio(5)

        '    Else

        '        VerificaCheckDomicilio(4)

        '    End If

        '    GuardaCamposDomicilio()

        'Else

        '    'scDomicilios.Enabled = False
        '    VerificaCheckDomicilio(1)

        'End If

    End Sub

    Sub VerificaCheckDomicilio(Optional ByVal opcion_ As Int32 = 1)

        'Select Case opcion_

        '    Case 1 'Defaults


        'swcUtilizarDatos.Enabled = False
        'swcUtilizarDatos.Visible = False

        'If swcUtilizarDatos.Checked Then

        'If GetVars("isEditing") = False Then

        '        icCalle.Visible = False
        '        icNumeroExterior.Visible = False
        '        icNumeroInterior.Visible = False
        '        icCodigoPostal.Visible = False
        '        icColonia.Visible = False
        '        icCiudad.Visible = False
        '        icLocalidad.Visible = False
        '        icMunicipio.Visible = False
        '        icEntidadFederativa.Visible = False
        '        icPais.Visible = False

        'scDomicilios.Visible = True
        'scDomicilios.Enabled = True


        'swcUtilizarDatos.Enabled = False
        'swcUtilizarDatos.Visible = False

        'If scDomicilios.Value IsNot Nothing And Not String.IsNullOrEmpty(scDomicilios.Value) Then

        '    swcEditarDomicilio.Visible = True

        'End If

        'End If

        'Else

        'icCalle.Enabled = True
        '            icNumeroExterior.Enabled = True
        '            icNumeroInterior.Enabled = True
        '            icCodigoPostal.Enabled = True
        '            icColonia.Enabled = True
        '            icCiudad.Enabled = True
        '            icLocalidad.Enabled = True
        '            icMunicipio.Enabled = True
        '            icEntidadFederativa.Enabled = True
        '            icPais.Enabled = True
        '            icCalle.Visible = True
        '            icNumeroExterior.Visible = True
        '            icNumeroInterior.Visible = True
        '            icCodigoPostal.Visible = True
        '            icColonia.Visible = True
        '            icCiudad.Visible = True
        '            icLocalidad.Visible = True
        '            icMunicipio.Visible = True
        '            icEntidadFederativa.Visible = True
        '            icPais.Visible = True
        '            icCalle.Value = Nothing
        '            icNumeroExterior.Value = Nothing
        '            icNumeroInterior.Value = Nothing
        '            icCodigoPostal.Value = Nothing
        '            icColonia.Value = Nothing
        '            icCiudad.Value = Nothing
        '            icLocalidad.Value = Nothing
        '            icMunicipio.Value = Nothing
        '            icEntidadFederativa.Value = Nothing
        '            icPais.Value = Nothing
        'scDomicilios.Visible = False
        'scDomicilios.Enabled = False
        'swcEditarDomicilio.Visible = False
        'swcEditarDomicilio.Enabled = False
        'swcEditarDomicilio.Visible = False

        '        End If

        '    Case 2

        '        If swcUtilizarDatos.Checked Then

        '            icCalle.Enabled = True
        '            icNumeroExterior.Enabled = True
        '            icNumeroInterior.Enabled = True
        '            icCodigoPostal.Enabled = True
        '            icColonia.Enabled = True
        '            icCiudad.Enabled = True
        '            icLocalidad.Enabled = True
        '            icMunicipio.Enabled = True
        '            icEntidadFederativa.Enabled = True
        '            icPais.Enabled = True
        '            icCalle.Visible = True
        '            icNumeroExterior.Visible = True
        '            icNumeroInterior.Visible = True
        '            icCodigoPostal.Visible = True
        '            icColonia.Visible = True
        '            icCiudad.Visible = True
        '            icLocalidad.Visible = True
        '            icMunicipio.Visible = True
        '            icEntidadFederativa.Visible = True
        '            icPais.Visible = True
        '            'scDomicilios.Visible = False
        '            'scDomicilios.Enabled = False



        '            'swcUtilizarDatos.Enabled = False
        '            'swcUtilizarDatos.Visible = False

        '        Else

        '            icCalle.Enabled = False
        '            icNumeroExterior.Enabled = False
        '            icNumeroInterior.Enabled = False
        '            icCodigoPostal.Enabled = False
        '            icColonia.Enabled = False
        '            icCiudad.Enabled = False
        '            icLocalidad.Enabled = False
        '            icMunicipio.Enabled = False
        '            icEntidadFederativa.Enabled = False
        '            icPais.Enabled = False
        '            icCalle.Visible = False
        '            icNumeroExterior.Visible = False
        '            icNumeroInterior.Visible = False
        '            icCodigoPostal.Visible = False
        '            icColonia.Visible = False
        '            icCiudad.Visible = False
        '            icLocalidad.Visible = False
        '            icMunicipio.Visible = False
        '            icEntidadFederativa.Visible = False
        '            icPais.Visible = False
        '            'scDomicilios.Visible = True
        '            'scDomicilios.Enabled = True


        '            'swcUtilizarDatos.Enabled = False
        '            'swcUtilizarDatos.Visible = False

        '            'If scDomicilios.Value IsNot Nothing And Not String.IsNullOrEmpty(scDomicilios.Value) Then

        '            '    swcEditarDomicilio.Visible = True

        '            'End If

        '        End If

        '    Case 3

        '        If swcEditarDomicilio.Checked = True Then

        '            icCalle.Visible = True
        '            icNumeroExterior.Visible = True
        '            icNumeroInterior.Visible = True
        '            icCodigoPostal.Visible = True
        '            icColonia.Visible = True
        '            icCiudad.Visible = True
        '            icLocalidad.Visible = True
        '            icMunicipio.Visible = True
        '            icEntidadFederativa.Visible = True
        '            icPais.Visible = True
        '            'scDomicilios.Visible = False
        '            'scDomicilios.Enabled = False


        '            'swcUtilizarDatos.Enabled = False
        '            'swcUtilizarDatos.Visible = False

        '        Else

        '            icCalle.Enabled = False
        '            icNumeroExterior.Enabled = False
        '            icNumeroInterior.Enabled = False
        '            icCodigoPostal.Enabled = False
        '            icColonia.Enabled = False
        '            icCiudad.Enabled = False
        '            icLocalidad.Enabled = False
        '            icMunicipio.Enabled = False
        '            icEntidadFederativa.Enabled = False
        '            icPais.Enabled = False
        '            icCalle.Visible = False
        '            icNumeroExterior.Visible = False
        '            icNumeroInterior.Visible = False
        '            icCodigoPostal.Visible = False
        '            icColonia.Visible = False
        '            icCiudad.Visible = False
        '            icLocalidad.Visible = False
        '            icMunicipio.Visible = False
        '            icEntidadFederativa.Visible = False
        '            icPais.Visible = False
        '            'scDomicilios.Visible = True
        '            'scDomicilios.Enabled = True


        '            'swcUtilizarDatos.Enabled = True


        '            'swcUtilizarDatos.Enabled = False
        '            'swcUtilizarDatos.Visible = False

        '        End If

        '    Case 4 'Desactivar controles

        '        icCalle.Enabled = False
        '        icNumeroExterior.Enabled = False
        '        icNumeroInterior.Enabled = False
        '        icCodigoPostal.Enabled = False
        '        icColonia.Enabled = False
        '        icCiudad.Enabled = False
        '        icLocalidad.Enabled = False
        '        icMunicipio.Enabled = False
        '        icEntidadFederativa.Enabled = False
        '        icPais.Enabled = False

        '        swcEditarDomicilio.Enabled = True
        '        swcEditarDomicilio.Visible = True




        '    Case 5 'Activar controles

        '        icCalle.Enabled = True
        '        icNumeroExterior.Enabled = True
        '        icNumeroInterior.Enabled = True
        '        icCodigoPostal.Enabled = True
        '        icColonia.Enabled = True
        '        icCiudad.Enabled = True
        '        icLocalidad.Enabled = True
        '        icMunicipio.Enabled = True
        '        icEntidadFederativa.Enabled = True
        '        icPais.Enabled = False


        '        'swcEditarDomicilio.Enabled = False
        '        'swcEditarDomicilio.Visible = False
        'End Select

    End Sub

    'EVENTOS PARA CARGAR LAS VINCULACIONES
    Protected Sub scVinculacion_Click(sender As Object, e As EventArgs)

        scVinculacion.DataSource = Vinculacion()

    End Sub

    Protected Sub rdSeleccionarDomicilio_CheckedChanged(sender As Object, e As EventArgs)

        Dim seleccionConfig_ = rdSeleccionarDomicilio.SelectedIndex

        SetVars("seleccionConfig_", seleccionConfig_)

        If seleccionConfig_ = 0 Then

            scDomicilios.Enabled = True

            'CamposBloqueadosDefault()

        Else

            ''DATOS NUEVOS DE PAQUETEEEE

            scDomicilios.Enabled = True

            'swcEditarDomicilio.Enabled = False


            ''vaciar los campes :V

            ' LimpiarFormularioDomicilio()

        End If

        ListarDomiciliosConfiguracion()

    End Sub


    Private Sub LimpiarFormularioDomicilio()

        ''SI ES DOMICILIO NUEVO COMPLETAMENTE, AL MENOS PARA UN PAIS EXTRANJERO
        '' EL TARJETERO DEBE QUEDAR VACIO

        SetVars("listaDomiciliosPais_", Nothing)

        fsConfiguracionDomicilio.Visible = False


        ''AQUI LO QUE SUCEDE, ES QUE SE TENDRIA QUE VACIAR TODO EL PILLBOX
        ''PERO AUN NO SABER COMO YO HACER ESE :V

        pbDetalleProveedor.DataSource = Nothing

        PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbDetalleProveedor)

        If _urlType Then

            ''LA VERDAD ES QUE SOLO SE TENDRIA QUE HABILITAR EL PAIS
            ''PERO AL ESTAR BLOQUEADO TODO EL AREA DEL ENCABEZADO
            ''POR ESO NO ME DEJA HABILITAR SOLO EL PAIS :(

            scDomicilios.DataSource = Nothing
            fbcPais.DataSource = Nothing
            fbcPais.Text = Nothing
            fbcPais.Enabled = True

            icIdPais.Value = Nothing
            icCvePais.Value = Nothing
            icPais.Value = Nothing

        End If

    End Sub

    Private Sub InicializarCamposPais()

        icIdPais.Value = New ObjectId("635acf2ba8210bfa0d5843f3").ToString
        icCvePais.Value = "MEX"
        icPais.Value = "MEX - MÉXICO (ESTADOS UNIDOS MEXICANOS)"

    End Sub

#End Region

#Region "██████ Vinculación sexta capa  █████████       SAX      ████████████████████████████████████████████"
    '    ██████   Controladores utilizados                     Documentos por coding para MongoDB      ██████
    '    ██████    1.ControladorEmpresas                        1. En Empresa:                         ██████
    '    ██████    2.ControladorRecursosAduanales                  a). Domicilios                      ██████
    '    ██████    3.ControladorSecuencias                         b). Contactos                       ██████
    '    ██████                                                                                        ██████
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Private Function CargaTopClientes() As List(Of SelectOption)

        '    'Dim controlador_ As New ControladorBusqueda(Of ConstructorCliente)

        '    'controlador_.AgregarFiltro(SeccionesClientes.SCS1, CamposClientes.CA_RAZON_SOCIAL)
        '    'controlador_.Limit = 5
        '    'controlador_.Buscar("")
        '    'Dim tagwatcher_ = controlador_.ObtenerResultadosBusqueda(CamposClientes.CA_RAZON_SOCIAL)

        '    'If tagwatcher_.Count > 0 Then

        '    '    Return tagwatcher_

        '    'End If

        '    'Return Nothing

    End Function

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

    'EVENTO PARA LEER EL HISTORIAL DE DOMICILIO
    Protected Sub CargarHistorialDomicilios()

        Dim i = 1

        pbDetalleProveedor.ForEach(Sub(pillbox_ As PillBox)


                                       Dim icTaxRfc_ = Nothing

                                       If _urlType Then

                                           icTaxRfc_ = pillbox_.GetControlValue(icTaxId)

                                       Else

                                           icTaxRfc_ = pillbox_.GetControlValue(icRFC)

                                       End If

                                       ccDomiciliosFiscales.SetRow(Sub(catalogRow_ As CatalogRow)
                                                                       catalogRow_.SetIndice(ccDomiciliosFiscales.KeyField, i)
                                                                       catalogRow_.SetColumn(icTaxIDRFC, icTaxRfc_)
                                                                       catalogRow_.SetColumn(icDomicilio, pillbox_.GetControlValue(icCalle) & " #" & pillbox_.GetControlValue(icNumeroExterior) & " C.P. " & pillbox_.GetControlValue(icCodigoPostal) & " " & pillbox_.GetControlValue(icPais))
                                                                       catalogRow_.SetColumn(swcArchivarDomicilio, False)

                                                                   End Sub)

                                       i += 1

                                   End Sub)

        ccDomiciliosFiscales.CatalogDataBinding()

    End Sub

    Sub CamposBloqueadosDefault()
        swcTipoUso.Enabled = False
        icClave.Enabled = False
        icPais.Enabled = False
        sectionDomicilio.Enabled = False
    End Sub

    Sub HabilitarDomicilio()

        swcTipoUso.Enabled = False
        icClave.Enabled = False
        icPais.Enabled = False
        scDomicilios.Enabled = False
        sectionDomicilio.Enabled = True

    End Sub

    ''' <summary>
    ''' METODOS NACIONALES
    ''' </summary>
    ''' 
    Protected Sub CargarCamposNacionales()

        If GetVars("_listaEmpresasTemporales") IsNot Nothing Then

            ''INICIALIZAMOS PROPIEDADES PRIVADAS
            InicializarControladoresNacionales()

            _listaEmpresasNacionales = DirectCast(GetVars("_listaEmpresasTemporales"), List(Of EmpresaNacional))

            If _listaEmpresasNacionales.Count > 0 Then

                _empresaNacional = DirectCast(_listaEmpresasNacionales.Find(Function(x) x.razonsocial = fcRazonSocial.Text), EmpresaNacional)

                ''LLENAMOS EL ENCABEZADO DE LA VISTA

                With _empresaNacional

                    idEmpresa.Value = ._id.ToString
                    cveEmpresa.Value = ._idempresa

                    fcRazonSocial.Value = .razonsocial

                    swcTipoUso.Checked = False
                    swcTipoUso.Enabled = False

                    fsConfiguracionDomicilio.Enabled = True

                    InicializarCamposPais()

                End With

                SetVars("_empresa", _empresaNacional)

            End If

        End If

    End Sub

    ''' <summary>
    ''' METODOS INTERNACIONALES
    ''' </summary>
    '''
    Sub CargarCamposInternacionales()

        If GetVars("_listaempresastemporales") IsNot Nothing Then

            InicializarControles()

            _listaEmpresasInternacionales = DirectCast(GetVars("_listaempresastemporales"),
                                                       List(Of EmpresaInternacional))

            If _listaEmpresasInternacionales.Count > 0 Then

                _empresaInternacional = DirectCast(_listaEmpresasInternacionales.Find(Function(x) x.razonsocial = fcRazonSocial.Text), EmpresaInternacional)

                With _empresaInternacional

                    idEmpresa.Value = ._id.ToString

                    cveEmpresa.Value = ._idempresa

                    fcRazonSocial.Value = .razonsocial

                    swcTipoUso.Checked = True

                    swcTipoUso.Enabled = False

                    Dim lista_ As New List(Of SelectOption)

                    Dim listapaisespropios_ = _empresaInternacional.paisesdomicilios

                    For Each item_ In listapaisespropios_

                        lista_.Add(New SelectOption _
                                   With {
                                        .Value = item_.idpais.ToString,
                                        .Text = item_.paisPresentacion})
                    Next

                    fbcPais.DataSource = lista_

                    fbcPais.Value = lista_.Last.Value

                    SetVars("listadomiciliospais_", lista_)

                    fsConfiguracionDomicilio.Enabled = True

                End With

                SetVars("_empresa", _empresaInternacional)

            End If

        End If

    End Sub

    ''' <summary>
    ''' METODOS PARA AMBOS CASOS
    ''' </summary>
    '''
    Private Function ListarEmpresas(Of T)() As List(Of SelectOption)

        Dim lista_ As New List(Of SelectOption)

        InicializarControles()

        With _controladorEmpresas

            _tagwatcher = .Consultar(fcRazonSocial.Text)

            If _tagwatcher.Status = TypeStatus.Ok Then

                Dim listaempresas_ = _tagwatcher.ObjectReturned

                SetVars("_listaempresastemporales", listaempresas_)

                If listaempresas_.count > 0 Then

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


    Sub ListarDomiciliosConfiguracion()

        _listaDomicilios = New List(Of Rec.Globals.Empresas.Domicilio)

        _opcionesLista = New List(Of SelectOption)

        _pillboxControl = New PillboxControl

        _ultimoDomicilio = New List(Of Rec.Globals.Empresas.Domicilio)

        If GetVars("_empresa") IsNot Nothing Then

            InicializarControles()

            If _urlType Then

                _empresaInternacional = GetVars("_empresa")

            Else

                _empresaNacional = GetVars("_empresa")

            End If

            With _controladorEmpresas


                fsConfiguracionDomicilio.Enabled = True

                .Modalidad = IControladorEmpresas.Modalidades.Intrinseca

                If _urlType Then

                    .TipoEmpresa = IControladorEmpresas.TiposEmpresas.Internacional

                    .ListaEmpresas.Add(_empresaInternacional)

                    .PaisEmpresa = icCvePais.Value

                    _tagwatcher = .ConsultarDomicilios(_empresaInternacional._id)

                    DomiciliosPorPaisExtranjero()

                Else

                    SetVars("_empresa", _empresaNacional)

                    .ListaEmpresas.Add(_empresaNacional)

                    _tagwatcher = .ConsultarDomicilios(_empresaNacional._id)

                End If

                If _tagwatcher.Status = TypeStatus.Ok Then

                    _listaDomicilios = DirectCast(_tagwatcher.ObjectReturned, List(Of Rec.Globals.Empresas.Domicilio))

                    SetVars("_listaDomicilios", _listaDomicilios)


                Else

                    DisplayMessage("Razón social sin domicilios", StatusMessage.Info)

                    SetVars("_listaDomicilios", Nothing)


                End If

            End With

            If _listaDomicilios IsNot Nothing Then

                If _listaDomicilios.Count > 0 Then

                    _ultimoDomicilio.Add(_listaDomicilios.Last)

                    For Each item_ In _listaDomicilios

                        _opcionesLista.Add(New SelectOption With
                                            {
                                                .Value = item_._iddomicilio.ToString,
                                                .Text = item_.domicilioPresentacion
                                            })
                    Next

                    ''LLENAMOS EL SELECTOR DE DOMICILICIOS

                    If _opcionesLista.Count > 0 And scDomicilios.DataSource Is Nothing Then

                        scDomicilios.DataSource = _opcionesLista
                        scDomicilios.Value = _opcionesLista.Last.Value

                        CargarTarjeteroDomicilio(_ultimoDomicilio)

                        ''HABILITAMOS EL BOTON DE EDITARS :V
                        swcEditarDomicilio.Checked = True


                    Else

                        scDomicilios.DataSource = _opcionesLista
                        scDomicilios.Value = ""

                        swcEditarDomicilio.Checked = True
                        swcEditarDomicilio.Enabled = True

                    End If

                End If

            Else

                DisplayMessage("Domicilios no encontrados de empresa", StatusMessage.Info)

                If _urlType Then

                    icTaxId.Value = _empresaInternacional.taxids.Last.taxid

                    DomiciliosPorPaisExtranjero()

                Else

                    With _empresaNacional

                        icRFC.Value = .rfc

                        icCURP.Value = .curp

                        icCveRfc.Value = ._idrfc.ToString

                        icCveCurp.Value = ._idcurp.ToString

                        If .tipopersona <> 1 Then

                            swcTipoPersona.Checked = True

                        End If

                        InicializarCamposPais()

                    End With

                    ''ESTA CONFIGURACION SERÁ PARA CUANDO ES UNA EMPRESA NUEVECITA
                    ''O LA EMPRESA EXTRAÑAMENTE NO CONTIENE DOMICILIOS
                    swcEditarDomicilio.Enabled = True
                    swcEditarDomicilio.Checked = True
                    scDomicilios.DataSource = Nothing
                    scDomicilios.Value = ""

                End If

            End If

        Else

            SetVars("_empresa", Nothing)

        End If

    End Sub



    ''PORQUE SOLO ESTA CARGANDO EL ULTIMO DOMICILIO, PERO DE LA EMPRESA, NO DEL PROVEEDORRSSSSSSSSS COMO DEBE DE SER
    Sub CargarTarjeteroDomicilio(ByVal ultimoDomicilio_ As List(Of Rec.Globals.Empresas.Domicilio))

        _pillboxControl = DirectCast(pbDetalleProveedor, PillboxControl)

        _pillboxControl.ClearRows()

        Dim indice_ = 0

        For Each item_ In ultimoDomicilio_

            _pillboxControl.SetPillbox(Sub(pillbox_ As PillBox)

                                           With item_

                                               pillbox_.SetIndice(_pillboxControl.KeyField, indice_)

                                               pillbox_.SetFiled(False)

                                               If _urlType Then

                                                   With _empresaInternacional

                                                       pillbox_.SetControlValue(icTaxId, .taxids.Last.taxid)

                                                       pillbox_.SetControlValue(icCvePais, .paisesdomicilios.Last.pais)

                                                       pillbox_.SetControlValue(icPais, .paisesdomicilios.Last.paisPresentacion)

                                                       pillbox_.SetControlValue(icIdPais, .paisesdomicilios.Last.idpais.ToString)

                                                   End With

                                               Else

                                                   With _empresaNacional

                                                       pillbox_.SetControlValue(icRFC, .rfc)

                                                       pillbox_.SetControlValue(icCURP, .curp)

                                                       pillbox_.SetControlValue(icCveRfc, ._idrfc.ToString)

                                                       pillbox_.SetControlValue(icCveCurp, ._idcurp.ToString)

                                                       pillbox_.SetControlValue(icIdPais, .paisesdomicilios.Last.idpais.ToString)

                                                       pillbox_.SetControlValue(icCvePais, .paisesdomicilios.Last.pais)

                                                       pillbox_.SetControlValue(icPais, .paisesdomicilios.Last.paisPresentacion)

                                                       ''1 ES PERSON FISICA
                                                       ''0 ES PERSON MORAL
                                                       ''PERO EN LA EMPRESA SE GUARDA COMO 1 (MORAL) Y 2 (FISICA)
                                                       Dim tipoPersona_ = IIf(.tipopersona <> 1, 1, 0)

                                                       ''PONGO ESTO PORQUE ESE SWICHT NO CHAMBEA COMO DEBE :V
                                                       SetVars("tipoPersona_", tipoPersona_)

                                                       pillbox_.SetControlValue(swcTipoPersona, tipoPersona_)

                                                   End With

                                               End If

                                               pillbox_.SetControlValue(icCalle, .calle)

                                               pillbox_.SetControlValue(icNumeroExterior, .numeroexterior)

                                               pillbox_.SetControlValue(icNumeroInterior, .numerointerior)

                                               pillbox_.SetControlValue(icCodigoPostal, .codigopostal)

                                               pillbox_.SetControlValue(icColonia, .colonia)

                                               pillbox_.SetControlValue(icLocalidad, .localidad)

                                               pillbox_.SetControlValue(icCiudad, .ciudad)

                                               pillbox_.SetControlValue(icMunicipio, .municipio)

                                               pillbox_.SetControlValue(icEntidadFederativa, .entidadfederativa)

                                               pillbox_.SetControlValue(icIdDomicilio, ._iddomicilio.ToString)

                                               pillbox_.SetControlValue(icSecDomicilio, .sec)

                                               pillbox_.SetControlValue(scDomicilio, .domicilioPresentacion)

                                               pillbox_.SetControlValue(icNumeroExtInt, .numeroexterior & " - " & .numerointerior)

                                               pillbox_.SetControlValue(icCveMunicipio, .cveMunicipio)

                                               pillbox_.SetControlValue(icCveEntidadFederativa, .cveEntidadfederativa)

                                           End With

                                       End Sub)
            indice_ += 1

        Next

        pbDetalleProveedor = _pillboxControl

        pbDetalleProveedor.PillBoxDataBinding()

    End Sub


    Private Function GenerarEmpresaNacional() As TagWatcher

        _pillboxControl = New PillboxControl

        _listaDomicilios = New List(Of Rec.Globals.Empresas.Domicilio)

        InicializarControles()

        'ESTE VIENE DESDE EL RADIO BUTTON, DESDE CONFIGURACIONES DOMICILIOS
        'SI ESTA SELECCIONADO EL DOMICILIO CON REGISTRADO O COMO NUEVO
        'SI EL VALOR ES 1, ES PORQUE ES UN DOMICILIO NUEVO
        'SI ES 0 ES PORQUE UTILIZO DOMICILIO QUE LA EMPRESA OFRECIA
        'Dim seleccionConfig_ = GetVars("seleccionConfig_")

        Dim seleccionConfig_ = 0

        _pillboxControl = DirectCast(pbDetalleProveedor, PillboxControl)

        _pillboxControl.ForEach(Sub(pillbox_ As PillBox)

                                    ''OBTENER LA SECUENCIA DE LA EMPRESA
                                    Dim secDomicilio_ = 1

                                    Dim domicilioNuevo_ As New Rec.Globals.Empresas.Domicilio(pillbox_.GetControlValue(icCalle),
                                                                                             secDomicilio_,
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
                                                                                             pais_:=pillbox_.GetControlValue(icCvePais))

                                    'VAMOS A CREAR UNA NUEVA EMPRESA
                                    ''ES UNA PERSONA FISICA ES TRUE, PUEDE TRAER UN CURP
                                    ''ESTO DEBE VENIR DESDE pillbox_.GetControlValue(swcTipoPersona)
                                    Dim tipoPersona_ = False

                                    If GetVars("_empresa") IsNot Nothing Then

                                        'VAMOS ACTUALIZAR LA EMPRESA 
                                        _empresaNacional = DirectCast(GetVars("_empresa"), EmpresaNacional)

                                        With _empresaNacional

                                            ''LLENANDO CURP
                                            If tipoPersona_ Then

                                                Dim curpAux_ = pillbox_.GetControlValue(icCURP)

                                                If curpAux_ <> .curp Then

                                                    Dim secuenciaCurp_ = 1

                                                    If seleccionConfig_ <> 0 Then

                                                        .curps = New List(Of Rec.Globals.Empresas.Curp)

                                                    Else

                                                        secuenciaCurp_ = .curps.Count + 1

                                                    End If

                                                    Dim curpNuevo_ As New Rec.Globals.Empresas.Curp(curpAux_, secuenciaCurp_)

                                                    ._idcurp = curpNuevo_.idcurp
                                                    .curp = curpNuevo_.curp
                                                    .curps.Add(curpNuevo_)
                                                    .curpNuevo = True

                                                    pillbox_.SetControlValue(icCveCurp, curpNuevo_.idcurp.ToString)

                                                End If
                                            Else

                                                .curpNuevo = False

                                            End If

                                            ''LLENANDO RFC
                                            Dim rfcAux_ = pillbox_.GetControlValue(icRFC)

                                            If rfcAux_ <> .rfc Then

                                                Dim rfcNuevo_ As New Rec.Globals.Empresas.Rfc(rfcAux_, .rfcs.Count + 1)

                                                ._idrfc = rfcNuevo_.idrfc
                                                .rfc = rfcNuevo_.rfc
                                                .rfcs.Add(rfcNuevo_)
                                                .rfcNuevo = True

                                                pillbox_.SetControlValue(icCveRfc, rfcNuevo_.idrfc.ToString)

                                            Else

                                                .rfcNuevo = False

                                            End If

                                            ''CONFIGURAR DOMICILIOS
                                            ''selecccionConfig si es 1, es porque no tomo domicilio de los ofrecidos sino que 
                                            ''ocupo uno totalmente nuevo

                                            If swcEditarDomicilio.Checked And seleccionConfig_ > 0 Then

                                                Dim paisDomicilioNuevo_ As New Rec.Globals.Empresas.PaisDomicilio(New ObjectId(pillbox_.GetControlValue(icIdPais)),
                                                                                                             pillbox_.GetControlValue(icCvePais),
                                                                                                             pillbox_.GetControlValue(icPais))

                                                .paisesdomicilios = New List(Of Rec.Globals.Empresas.PaisDomicilio) From {paisDomicilioNuevo_}

                                            Else

                                                Dim domicilioActual_ = pillbox_.GetControlValue(scDomicilio)

                                                If domicilioActual_ <> domicilioNuevo_.domicilioPresentacion Then

                                                    domicilioNuevo_.sec = _empresaNacional.paisesdomicilios.Last.domicilios.Count + 1

                                                Else
                                                    ''ESTO SIGNIFICA QUE NO HICIERON CAMBIOS EN NINGUNA PARTE DEL DOMICILIO
                                                    ''QUIZAS SOLO AFECTARON AL RFC, CURP, DESTINATARIO
                                                    domicilioNuevo_._iddomicilio = New ObjectId(pillbox_.GetControlValue(icIdDomicilio))
                                                    domicilioNuevo_.sec = pillbox_.GetControlValue(icSecDomicilio)

                                                End If
                                            End If

                                            .paisesdomicilios.Last.domicilios.Add(domicilioNuevo_)

                                            _listaDomicilios.Add(domicilioNuevo_)

                                            _tagwatcher = _controladorEmpresas.Modificar(_empresaNacional)

                                            If _tagwatcher.Status = TypeStatus.Ok Then

                                                SetVars("_empresaNueva", False)

                                                SetVars("_empresa", _empresaNacional)

                                                CargarTarjeteroDomicilio(_listaDomicilios)

                                            End If

                                        End With

                                    Else

                                        If tipoPersona_ Then

                                            _empresaNacional = _controladorEmpresas.EstructuraEmpresaNacional(fcRazonSocial.Text,
                                                                                                         pillbox_.GetControlValue(icRFC),
                                                                                                         domicilioNuevo_,
                                                                                                         IEmpresaNacional.TiposPersona.Fisica,
                                                                                                         pillbox_.GetControlValue(icCURP))
                                        Else

                                            _empresaNacional = _controladorEmpresas.EstructuraEmpresaNacional(fcRazonSocial.Text,
                                                                                                         pillbox_.GetControlValue(icRFC),
                                                                                                         domicilioNuevo_)
                                        End If

                                        _tagwatcher = _controladorEmpresas.Agregar(_empresaNacional, True)

                                        If _tagwatcher.Status = TypeStatus.Ok Then

                                            SetVars("_empresaNueva", True)

                                            SetVars("_empresa", DirectCast(_tagwatcher.ObjectReturned(0), EmpresaNacional))

                                            'CargarTarjeteroDomicilio(_listaDomicilios)

                                        End If

                                    End If

                                End Sub)
        Return _tagwatcher

    End Function

    Private Function GenerarEmpresaInternacional() _
                     As TagWatcher

        _pillboxControl = New PillboxControl

        _listaDomicilios = New List(Of Rec.Globals.Empresas.Domicilio)

        InicializarControles()

        '''PORQUE ESTABA VIAJANDO VACIO :V
        _controladorEmpresas.PaisEmpresa = fbcPais.Text.Substring(0, 3)

        'ESTE VIENE DESDE EL RADIO BUTTON, DESDE CONFIGURACIONES DOMICILIOS
        'SI ESTA SELECCIONADO EL DOMICILIO CON REGISTRADO O COMO NUEVO
        'SI EL VALOR ES 1, ES PORQUE ES UN DOMICILIO NUEVO
        'SI ES 0 ES PORQUE UTILIZO DOMICILIO QUE LA EMPRESA OFRECIA
        'Dim seleccionConfig_ = GetVars("seleccionConfig_")

        Dim seleccionConfig_ = 1

        _pillboxControl = DirectCast(pbDetalleProveedor, PillboxControl)

        _pillboxControl.ForEach(Sub(pillbox_ As PillBox)

                                    ''OBTENER LA SECUENCIA DE LA EMPRESA
                                    Dim secDomicilio_ = 1

                                    Dim domicilioNuevo_ As New Rec.Globals.Empresas.Domicilio(pillbox_.GetControlValue(icCalle),
                                                                                             secDomicilio_,
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
                                                                                             pais_:=_controladorEmpresas.PaisEmpresa)

                                    If GetVars("_empresa") IsNot Nothing Then

                                        'VAMOS ACTUALIZAR LA EMPRESA 
                                        _empresaInternacional = DirectCast(GetVars("_empresa"), EmpresaInternacional)

                                        With _empresaInternacional

                                            Dim taxidAux_ = pillbox_.GetControlValue(icTaxId)

                                            If .taxids IsNot Nothing Then

                                                If .taxids.Count > 0 Then

                                                    Dim taxidExistente_ = .taxids.Where(Function(item_) item_.taxid = taxidAux_)

                                                    If Not taxidExistente_.Any() Then

                                                        Dim secTaxId_ = .taxids.Count + 1

                                                        .taxids.Add(New TaxId(taxidAux_, secTaxId_))

                                                    End If

                                                Else

                                                    .taxids = New List(Of Rec.Globals.Empresas.TaxId)

                                                    If taxidAux_ <> "" Then

                                                        .taxids.Add(New TaxId(taxidAux_))

                                                    End If

                                                End If

                                            Else

                                                .taxids = New List(Of Rec.Globals.Empresas.TaxId)

                                                If taxidAux_ <> "" Then

                                                    .taxids.Add(New TaxId(taxidAux_))

                                                End If

                                            End If


                                            pillbox_.SetControlValue(icTaxId, .taxids.Last.taxid)
                                            pillbox_.SetControlValue(icCveTaxId, .taxids.Last.idtaxid.ToString)


                                            ''CONFIGURAR DOMICILIOS
                                            ''selecccionConfig si es 1, es porque no tomo domicilio de los ofrecidos sino que 
                                            ''ocupo uno totalmente nuevo
                                            Dim secPaisesDomicilios_ = .paisesdomicilios.Count + 1

                                            If swcEditarDomicilio.Checked And seleccionConfig_ > 0 Then

                                                'AQUI VA OTRA COSSAAAAAA

                                                Dim paisDomicilioNuevo_ As New Rec.Globals.Empresas.PaisDomicilio(New ObjectId(pillbox_.GetControlValue(icIdPais)),
                                                                                                                  _controladorEmpresas.PaisEmpresa,
                                                                                                                  pillbox_.GetControlValue(icPais),
                                                                                                                  secPaisesDomicilios_)

                                                .paisesdomicilios = New List(Of Rec.Globals.Empresas.PaisDomicilio) From {paisDomicilioNuevo_}

                                            Else
                                                'ES NEUVO EL DOMICILIO

                                                If seleccionConfig_ > 0 Then


                                                    Dim paisDomicilioNuevo_ As New Rec.Globals.Empresas.PaisDomicilio(New ObjectId(pillbox_.GetControlValue(icIdPais)),
                                                                                                                        _controladorEmpresas.PaisEmpresa,
                                                                                                                       pillbox_.GetControlValue(icPais),
                                                                                                                       secPaisesDomicilios_)

                                                    .paisesdomicilios.Add(paisDomicilioNuevo_)

                                                Else

                                                    Dim domicilioActual_ = pillbox_.GetControlValue(scDomicilio)

                                                    If domicilioActual_ <> domicilioNuevo_.domicilioPresentacion Then

                                                        domicilioNuevo_.sec = _empresaInternacional.paisesdomicilios.Last.domicilios.Count + 1

                                                    Else
                                                        ''ESTO SIGNIFICA QUE NO HICIERON CAMBIOS EN NINGUNA PARTE DEL DOMICILIO
                                                        ''QUIZAS SOLO AFECTARON AL TAXID DESTINATARIO
                                                        domicilioNuevo_._iddomicilio = New ObjectId(pillbox_.GetControlValue(icIdDomicilio))

                                                        domicilioNuevo_.sec = pillbox_.GetControlValue(icSecDomicilio)

                                                    End If

                                                End If

                                            End If

                                            .paisesdomicilios.Where(Function(x) x.pais = _controladorEmpresas.PaisEmpresa).ToList.
                                                              ForEach(Sub(item_) item_.domicilios.
                                                              Add(domicilioNuevo_))

                                            _listaDomicilios.Add(domicilioNuevo_)

                                            ''PRUEBA CON CONTACTOS

                                            '.contactos = New List(Of Rec.Globals.Empresas.Contacto) _
                                            '             From {New Rec.Globals.Empresas.Contacto _
                                            '             With {.idcontacto = ObjectId.GenerateNewId,
                                            '                    .sec = 1,
                                            '                    .nombrecompleto = "JAVIER OROPEZA",
                                            '                    .correoelectronico = "SUCORREO@CORREO.COM",
                                            '                    .telefono = "2741662222",
                                            '                    .archivado = False,
                                            '                    .estado = 1}}

                                            _tagwatcher = _controladorEmpresas.Modificar(_empresaInternacional)

                                            If _tagwatcher.Status = TypeStatus.Ok Then

                                                SetVars("_empresaNueva", False)

                                                SetVars("_empresa", _empresaInternacional)

                                                CargarTarjeteroDomicilio(_listaDomicilios)

                                            End If

                                        End With

                                    Else


                                        _empresaInternacional = _controladorEmpresas.EstructuraEmpresaInternacional(fcRazonSocial.Text,
                                                                                                                    domicilioNuevo_,
                                                                                                                    pillbox_.GetControlValue(icTaxId))

                                        _tagwatcher = _controladorEmpresas.Agregar(_empresaInternacional, True)

                                        If _tagwatcher.Status = TypeStatus.Ok Then

                                            SetVars("_empresaNueva", True)

                                            SetVars("_empresa", DirectCast(_tagwatcher.ObjectReturned(0), EmpresaInternacional))

                                            ''SI PÉRO NO, DEBE CARGAR CON EL DOCUMENO ELECTRONICO EL PILLBOX ESE
                                            'CargarTarjeteroDomicilio(_listaDomicilios)

                                        End If

                                    End If

                                End Sub)
        Return _tagwatcher

    End Function


    Private Function GenerarSecuenciaInterna(Optional ByVal tipoSecuencia_ As Int32 = 1) _
    As Secuencia

        'tipoSecuencia_ = 1 Proveedor nacional, 2 Proveedor internacional

        _controladorSecuencias = New ControladorSecuencia

        _tagwatcher = New TagWatcher

        _secuencia = New Secuencia

        _tagwatcher = _controladorSecuencias.Generar(SecuenciasComercioExterior.ProveedoresOperativos.ToString, tipoSecuencia_, 1, 1, 1, 1)

        With _tagwatcher

            If .Status = TypeStatus.Ok Then

                _secuencia = DirectCast(.ObjectReturned, Secuencia)

            End If

        End With

        Return _secuencia

    End Function


    Private Function cargarListadoPaises() As List(Of SelectOption)

        Dim lista_ As New List(Of SelectOption)

        If GetVars("listaDomiciliosPais_") IsNot Nothing Then

            If GetVars("listaDomiciliosPais_").Count > 0 Then

                lista_ = DirectCast(GetVars("listaDomiciliosPais_"), List(Of SelectOption))

            End If

        Else
            ''CUANDO LA EMPRESA SEA NUEVA :V

            lista_ = listaPaisesExtrinsecos(fbcPais.Text)

        End If

        Return lista_

    End Function

    ''' <summary>
    ''' GENERAR LISTA DE PAISES
    ''' </summary>
    ''' <returns></returns>
    ''' 
    Private Function listaPaisesExtrinsecos(ByVal claveISO3_ As String) _
        As List(Of SelectOption)

        ''será temporal
        _tagwatcher = New TagWatcher

        _opcionesLista = New List(Of SelectOption)

        _tagwatcher = ControladorPaises.ConsultarListaPaisesPorClaveISO(claveISO3_)

        If _tagwatcher.Status = TypeStatus.Ok Then

            For Each item_ In _tagwatcher.ObjectReturned

                _opcionesLista.Add(New SelectOption With {.Value = item_._id.ToString,
                                                          .Text = item_.paisPresentacion})

            Next

        End If

        Return _opcionesLista

    End Function

#End Region

End Class